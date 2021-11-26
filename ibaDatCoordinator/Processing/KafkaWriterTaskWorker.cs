﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iba.Data;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using iba.Utility;
using iba.ibaFilesLiteDotNet;

namespace iba.Processing
{
    class KafkaWriterTaskWorker
    {
        private ConfigurationWorker m_confWorker;
        private KafkaWriterTaskData m_data;
        private StatusData m_sd;
        private IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer;
        private static byte[] schemaFingerPrintDefault;
        public static readonly Avro.RecordSchema schemaDefault;
        static KafkaWriterTaskWorker()
        {
            schemaDefault = (Avro.RecordSchema)Avro.RecordSchema.Parse(
                                                            @"{""namespace"": ""de.iba"",
                                                ""type"": ""record"",
                                                ""name"": ""PdaRecord"",
                                                ""fields"": [
                                                    {""name"": ""Signal"", ""type"": ""string""},
                                                    {""name"": ""ID"", ""type"": [""null"", ""string""]},
                                                    {""name"": ""Name"", ""type"": [""null"", ""string""]},
                                                    {""name"": ""Unit"", ""type"": [""null"", ""string""]},
                                                    {""name"": ""Comment1"", ""type"": [""null"", ""string""]},
                                                    {""name"": ""Comment2"", ""type"": [""null"", ""string""]},
                                                    {""name"": ""Identifier"", ""type"": [""null"", ""string""]},
                                                    {""name"": ""ValueType"", ""type"": { 
                                                        ""type"": ""enum"",
                                                        ""name"": ""ValueTypeEnum"",
                                                        ""symbols"" : [""BOOLEAN"", ""DOUBLE"", ""STRING""]
                                                    }},
                                                    {""name"": ""BooleanValue"", ""type"": [""null"", ""boolean""]},
                                                    {""name"": ""DoubleValue"", ""type"": [""null"", ""double""]},
                                                    {""name"": ""StringValue"", ""type"": [""null"", ""string""]}
                                                ]
                                            }
                                            "
                                );
            var estSize = schemaDefault.Count * 4;
            using (var ms = new System.IO.MemoryStream(estSize))
            {
                byte[] marker = { 0xC3, 0x01 };
                ms.Write(marker, 0, marker.Length);
                schemaFingerPrintDefault = Avro.SchemaNormalization.ParsingFingerprint("CRC-64-AVRO", schemaDefault);
            }
        }

        internal KafkaWriterTaskWorker(ConfigurationWorker worker, KafkaWriterTaskData task)
        {
            m_confWorker = worker;
            m_data = task;
            m_sd = worker.m_sd;
            m_ibaAnalyzer = worker.m_ibaAnalyzer;
        }
        bool TryGetUTCTimes(string filename, out DateTime startTime, out DateTime endTime) // TODO move to utility
        {
            startTime = endTime = DateTime.MinValue;

            try
            {
                if (Path.GetExtension(filename)?.ToLower() == ".hdq")
                {
                    IniParser parser = new IniParser(filename);
                    if (parser.Read() && !parser.Sections.ContainsKey("HDQ file"))
                        return false;

                    string strStart = "";
                    if (!parser.Sections["HDQ file"].TryGetValue("starttime", out strStart))
                        return false;

                    string strEnd = "";
                    if (!parser.Sections["HDQ file"].TryGetValue("stoptime", out strEnd))
                        return false;

                    startTime = DateTime.ParseExact(strStart, "dd.MM.yyyy HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
                    endTime = DateTime.ParseExact(strEnd, "dd.MM.yyyy HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
                }
                else
                {
                    IbaShortFileInfo sfi = IbaFileReader.ReadShortFileInfo(filename);
                    DateTime dtEnd = sfi.EndTime;
                    DateTime dtStart = new DateTime();
                    int microSeconds = 0;
                    m_ibaAnalyzer.GetStartTime(ref dtStart, ref microSeconds);
                    dtStart = dtStart.AddTicks(microSeconds * 10);

                    if (sfi.UtcOffsetValid)
                    {
                        var currOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow);
                        var fileOffset = TimeSpan.FromMinutes(sfi.UtcOffset);
                        dtEnd = dtEnd.AddTicks((currOffset - fileOffset).Ticks);
                    }

                    startTime = dtStart;
                    endTime = dtEnd;
                }
            }
            catch
            {
                return false;
            }

            startTime = startTime.ToUniversalTime();
            endTime = endTime.ToUniversalTime();
            return true;
        }

        internal void DoWork(string filename)
        {
            bool bUseAnalysis = !String.IsNullOrEmpty(m_data.AnalysisFile);
            if (bUseAnalysis && !File.Exists(m_data.AnalysisFile))
            {
                string message = iba.Properties.Resources.AnalysisFileNotFound + m_data.AnalysisFile;
                m_confWorker.Log(Logging.Level.Exception, message, filename, m_data);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return;
            }
            try
            {
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.RUNNING;
                }
                m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logKafkaStarted, filename, m_data);

                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_data.MonitorData))
                {
                    if (bUseAnalysis)
                        mon.Execute(delegate () {
                            m_ibaAnalyzer.OpenAnalysis(m_data.AnalysisFile);
                            m_data.EvaluateValues(filename, m_ibaAnalyzer);
                        });

                    var config = new ProducerConfig();

                    if (m_data.ClusterMode == KafkaWriterTaskData.ClusterType.Kafka)
                    {
                        config.BootstrapServers = m_data.clusterAddress;
                    }
                    else
                    {
                        config.BootstrapServers = "";

                        string[] connStringParts = m_data.clusterAddress.Split(';');
                        foreach (string it in connStringParts)
                        {
                            if (it.StartsWith("Endpoint=sb://", StringComparison.CurrentCultureIgnoreCase))
                            {
                                string uri = it.Substring(14, it.Length - 14);

                                // Also remove trailing / if present
                                if (uri.EndsWith("/"))
                                    uri = uri.Substring(0, uri.Length - 1);

                                config.BootstrapServers = string.Concat(uri, ":9093");
                            }
                        }
                    }

                    config.ClientId = m_data.identifier;
                    config.MessageTimeoutMs = (int)(m_data.timeout*1000);
                    switch (m_data.AckMode)
                    {
                        case KafkaWriterTaskData.RequiredAcks.None:
                            config.Acks = Confluent.Kafka.Acks.None;
                            break;
                        case KafkaWriterTaskData.RequiredAcks.Leader:
                            config.Acks = Confluent.Kafka.Acks.Leader;
                            break;
                        case KafkaWriterTaskData.RequiredAcks.All:
                            config.Acks = Confluent.Kafka.Acks.All;
                            break;
                    }

                    switch (m_data.ClusterSecurityMode)
                    {
                        case KafkaWriterTaskData.ClusterSecurityType.PLAINTEXT:
                            config.SecurityProtocol = Confluent.Kafka.SecurityProtocol.Plaintext;
                            break;
                        case KafkaWriterTaskData.ClusterSecurityType.SSL:
                            config.SecurityProtocol = Confluent.Kafka.SecurityProtocol.Ssl;
                            //// TODO implement sertificate
                            ////config.EnableSslCertificateVerification = connConfig.ClusterSecurity.SSLConfig.EnableVerification;
                            break;
                        case KafkaWriterTaskData.ClusterSecurityType.SASL_PLAINTEXT:
                            config.SecurityProtocol = Confluent.Kafka.SecurityProtocol.SaslPlaintext;
                            break;
                        case KafkaWriterTaskData.ClusterSecurityType.SASL_SSL:
                            config.SecurityProtocol = Confluent.Kafka.SecurityProtocol.SaslSsl;
                            ////config.EnableSslCertificateVerification = connConfig.ClusterSecurity.SSLConfig.EnableVerification;
                            break;
                    }
                    // Add expert parameters
                    foreach (var kvp in m_data.Params)
                    {
                        if (string.IsNullOrEmpty(kvp.Key))
                            continue;

                        if (kvp.Key.StartsWith("schema.registry."))
                            continue;

                        config.Set(kvp.Key, kvp.Value);
                    }

                    // If serializers are not specified, default serializers from
                    // `Confluent.Kafka.Serializers` will be automatically used where
                    // available. Note: by default strings are encoded as UTF8.
                    
                    try
                    {
                        // todo implement timestamp metadata
                        if (m_data.Format == KafkaWriterTaskData.DataFormat.JSONGrouped)
                        {
                            var p = new ProducerBuilder<string, string>(config).Build();
                            string message = "{ ";

                            // todo maybe change format to   message += $", \"{rec.Name}.Comment1\": \"{rec.Comment1}\""; and change Name matadata to Expression metadata
                            for (int i = 0; i < m_data.Records.Count; i++)
                            {
                                var rec = m_data.Records[i];
                                if (i > 0)  message += ", ";
                                message += $"\"{rec.Expression}\": \"{m_data.ToText(rec)}\"";
                                if (m_data.metadata.Contains("Identifier"))
                                    message += $", \"Identifier\": \"{m_data.identifier}\"";
                                if (m_data.metadata.Contains("Name"))
                                    message += $", \"Name\": \"{rec.Name}\"";
                                if (m_data.metadata.Contains("Unit"))
                                    message += $", \"Unit\": \"{rec.Unit}\"";
                                if (m_data.metadata.Contains("Comment 1"))
                                    message += $", \"Comment1\": \"{rec.Comment1}\"";
                                if (m_data.metadata.Contains("Comment 2"))
                                    message += $", \"Comment2\": \"{rec.Comment2}\"";
                                if (m_data.metadata.Contains("Signal ID"))
                                    message += $", \"Signal ID\": \"{rec.Id}\"";
                            }
                            message += "}";
                            var dr = p.ProduceAsync(m_data.topicName, new Message<string, string> { Key = m_data.key, Value = message }).Result;
                        }
                        else if (m_data.Format == KafkaWriterTaskData.DataFormat.JSONPerSignal)
                        {
                            var p = new ProducerBuilder<string, string>(config).Build();
                            foreach (var rec in m_data.Records)
                            {
                                string message = "";
                                if (m_data.metadata.Contains("Identifier"))
                                    message += $", \"Identifier\": \"{m_data.identifier}\"";
                                if (m_data.metadata.Contains("Name"))
                                    message += $", \"Name\": \"{rec.Name}\"";
                                if (m_data.metadata.Contains("Unit"))
                                    message += $", \"Unit\": \"{rec.Unit}\"";
                                if (m_data.metadata.Contains("Comment 1"))
                                    message += $", \"Comment1\": \"{rec.Comment1}\"";
                                if (m_data.metadata.Contains("Comment 2"))
                                    message += $", \"Comment2\": \"{rec.Comment2}\"";
                                if (m_data.metadata.Contains("Signal ID"))
                                    message += $", \"Signal ID\": \"{rec.Id}\"";
                                message = $"{{\"Signal\": \"{rec.Expression}\", \"Value\": \"{m_data.ToText(rec)}\"{message}}}";
                                var dr = p.ProduceAsync(m_data.topicName, new Message<string, string> { Key = m_data.key, Value = message }).Result;
                            }
                        }
                        else if (m_data.Format == KafkaWriterTaskData.DataFormat.AVRO)
                        {
                            var schemaFingerPrint = schemaFingerPrintDefault;
                            if (m_data.schemaRegistryAddress != "")
                                schemaFingerPrint = m_data.schemaFingerPrint;
                            var schema = schemaDefault;
                            

                            Confluent.Kafka.ProducerBuilder<byte[], byte[]> producerBuilder = new Confluent.Kafka.ProducerBuilder<byte[], byte[]>(config);
                            var p = producerBuilder.Build();

                            foreach (var rec in m_data.Records)
                            {
                                var record = rec.Value.ToString();

                                var msg = new Confluent.Kafka.Message<byte[], byte[]>();
                                if (TryGetUTCTimes(filename, out DateTime startTime, out DateTime _))
                                    msg.Timestamp = new Confluent.Kafka.Timestamp(startTime, Confluent.Kafka.TimestampType.CreateTime);

                                var r = new Avro.Generic.GenericRecord(schema);
                                schema.TryGetField("ValueType", out Avro.Field enumField);
                                Avro.EnumSchema valTypeSchema = enumField.Schema as Avro.EnumSchema;
                                r.Add("ValueType", new Avro.Generic.GenericEnum(valTypeSchema, rec.DataTypeAsString));
                                r.Add("Signal", rec.Expression);
                                if (m_data.metadata.Contains("Identifier"))
                                    r.Add("Identifier", "[" + m_data.identifier + "]");
                                if (m_data.metadata.Contains("Name"))
                                    r.Add("Name", rec.Name);
                                if (m_data.metadata.Contains("Unit"))
                                    r.Add("Unit", rec.Unit);
                                if (m_data.metadata.Contains("Comment 1"))
                                    r.Add("Comment1", rec.Comment1);
                                if (m_data.metadata.Contains("Comment 2"))
                                    r.Add("Comment2", rec.Comment2);
                                if (m_data.metadata.Contains("Signal ID"))
                                    r.Add("Signal ID", rec.Id);

                                r.Add("BooleanValue", rec.Value as bool?);
                                r.Add("DoubleValue", rec.Value as double?);
                                r.Add("StringValue", rec.Value as string);
                                var datumWriter = new Avro.Generic.GenericDatumWriter<Avro.Generic.GenericRecord>(schema);

                                var estSize = schema.Count * 4;
                                using (var ms = new System.IO.MemoryStream(estSize))
                                {
                                    ms.Write(schemaFingerPrint, 0, schemaFingerPrint.Length);

                                    Avro.IO.BinaryEncoder encoder = new Avro.IO.BinaryEncoder(ms);
                                    datumWriter.Write(r, encoder);

                                    int bytesWritten = (int)ms.Position;
                                    if (bytesWritten > estSize)
                                        estSize = bytesWritten;

                                    msg.Value = ms.ToArray();
                                }

                                msg.Key = Encoding.UTF8.GetBytes(m_data.key.ToCharArray());
                                var dr = p.ProduceAsync(m_data.topicName, msg);
                            }
                        }
                    }
                    catch (ProduceException<Null, string> ex)
                    {
                        string message = string.Format(iba.Properties.Resources.logKafkaFailed, ex.Error.Reason);
                        m_confWorker.Log(Logging.Level.Exception, message, filename, m_data);
                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.COMPLETED_FAILURE;
                        }
                    }

                    m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logKafkaSuccess, filename, m_data);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                    }
                }
            }
            catch (IbaAnalyzerExceedingTimeLimitException te)
            {
                m_confWorker.Log(Logging.Level.Exception, te.Message, filename, m_data);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.TIMED_OUT;
                }
                m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                m_confWorker.Log(Logging.Level.Exception, me.Message, filename, m_data);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.MEMORY_EXCEEDED;
                }
                m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (Exception e)
            {
                m_confWorker.Log(Logging.Level.Exception, e.Message, filename, m_data);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
            finally
            {
                if (m_ibaAnalyzer != null && bUseAnalysis)
                {
                    try
                    {
                        m_ibaAnalyzer.CloseAnalysis();
                    }
                    catch
                    {
                        m_confWorker.Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, filename, m_data);
                        m_confWorker.RestartIbaAnalyzer();
                    }
                }
            }
        }
    }
}
