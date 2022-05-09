using System;
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
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using static iba.Data.KafkaWriterTaskData;
using IbaAnalyzer;

namespace iba.Processing
{
    class KafkaWriterTaskWorker
    {
        private ConfigurationWorker m_confWorker;
        private KafkaWriterTaskData m_data;
        private StatusData m_sd;
        private IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer;
        private static byte[] schemaFingerPrintDefault;
        private string schemaRegistryAddressCached;
        private byte[] schemaFingerprintCached;
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

        string ReplacePlaceholders(KafkaWriterTaskData.KafkaRecord rec, string str)
        {
            str = str.Replace("$identifier", m_data.identifier);
            str = str.Replace("$signalname", rec.Name);
            str = str.Replace("$unit", rec.Unit);
            str = str.Replace("$comment1", rec.Comment1);
            str = str.Replace("$comment2", rec.Comment2);

            return str;
        }

        string ReplacePlaceholdersKey(string str)
        {
            str = str.Replace("$identifier", m_data.identifier);
            str = str.Replace("$signalname", "");
            str = str.Replace("$unit", "");
            str = str.Replace("$comment1", "");
            str = str.Replace("$comment2", "");

            return str;
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
                        mon.Execute(delegate () { { m_ibaAnalyzer.OpenAnalysis(m_data.AnalysisFile); } });

                    bool getMetadata = IsAnalyzerVersionNewer(m_ibaAnalyzer, 7, 3, 2);
                    foreach (var record in m_data.Records)
                    {
                        if (record.DataType == KafkaRecord.ExpressionType.Text)
                        {
                            object oValues = null;
                            mon.Execute(delegate () {m_ibaAnalyzer.EvaluateToStringArray(record.Expression, 0, out _, out oValues);});

                            if (oValues != null)
                            {
                                var values = (string[])oValues;
                                foreach (string str in values)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        record.Value = str;
                                        break;
                                    }
                                }
                            }
                            else
                                record.Value = "";
                        }
                        else
                        {
                            mon.Execute(delegate () { record.Value = m_ibaAnalyzer.EvaluateDouble(record.Expression, 0); });
                        }

                        if (getMetadata)
                        {
                            IChannelMetaData channelMetaData = null;
                            mon.Execute(delegate () { record.Value = m_ibaAnalyzer.GetChannelMetaData(record.Expression); });
                            if (channelMetaData != null)
                            {
                                record.Name = channelMetaData.name;
                                record.Unit = channelMetaData.Unit;
                                record.Comment1 = channelMetaData.Comment1;
                                record.Comment2 = channelMetaData.Comment2;
                            }
                            else
                            {
                                record.Name = "";
                                record.Unit = "";
                                record.Comment1 = "";
                                record.Comment2 = "";
                            }
                        }
                    }
                    if (m_data.metadata.Contains("Timestamp"))
                    {
                        DateTime timeStampDt;

                        if (m_data.timeStampExpression == StartTime)
                        {
                            timeStampDt = IbaFileReader.ReadShortFileInfo(filename).StartTime;
                        }
                        else if (m_data.timeStampExpression == EndTime)
                        {
                            timeStampDt = IbaFileReader.ReadShortFileInfo(filename).EndTime;
                        }
                        else
                        {
                            timeStampDt = IbaFileReader.ReadShortFileInfo(filename).StartTime;
                            double timeOffset = double.NaN;
                            mon.Execute(delegate () { timeOffset = m_ibaAnalyzer.Evaluate(m_data.timeStampExpression, 0); });
                            if (double.IsNaN(timeOffset))
                            {
                                m_confWorker.Log(Logging.Level.Warning, $"Cannot evaluate expression {m_data.timeStampExpression}", filename, m_data);
                            }
                            else
                            {
                                timeStampDt = timeStampDt.AddTicks((int)(timeOffset * 10000000));
                            }
                        }

                        var infofields = new IbaFileReader(filename, false).InfoFields;
                        if (infofields.TryGetValue("$PDA_UtcOffset", out string utcField))
                        {
                            timeStampDt = new DateTime(timeStampDt.Ticks, DateTimeKind.Unspecified);
                            m_data.timeStamp = timeStampDt.ToString("o", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                        }
                        else // use local UTC
                            m_data.timeStamp = timeStampDt.ToString("o", System.Globalization.CultureInfo.GetCultureInfo("en-US"));
                        m_data.timeStamp += "Z";
                    }
                    var config = InitConfig(m_data);

                    // If serializers are not specified, default serializers from
                    // `Confluent.Kafka.Serializers` will be automatically used where
                    // available. Note: by default strings are encoded as UTF8.

                    try
                    {
                        if (m_data.Format == KafkaWriterTaskData.DataFormat.JSONGrouped)
                        {
                            var producerBuilder = new ProducerBuilder<string, string>(config);
                            InitIBuilder(m_data, producerBuilder);
                            
                            string message = "{ \n";

                            for (int i = 0; i < m_data.Records.Count; i++)
                            {
                                var rec = m_data.Records[i];
                                var sigRef = ReplacePlaceholders(rec, m_data.signalReference);
                                if (i > 0)  
                                    message += ",\n ";
                                message += $"\"{ReplacePlaceholders(rec, m_data.signalReference)}\": {m_data.ToText(rec)}";
                                if (m_data.metadata.Contains("Identifier"))
                                    message += $",\n \"{sigRef}.Identifier\": \"{m_data.identifier}\"";
                                if (m_data.metadata.Contains("Name"))
                                    message += $",\n \"{sigRef}.Name\": \"{rec.Name}\"";
                                if (m_data.metadata.Contains("Unit"))
                                    message += $",\n \"{sigRef}.Unit\": \"{rec.Unit}\"";
                                if (m_data.metadata.Contains("Comment 1"))
                                    message += $",\n \"{sigRef}.Comment1\": \"{rec.Comment1}\"";
                                if (m_data.metadata.Contains("Comment 2"))
                                    message += $",\n \"{sigRef}.Comment2\": \"{rec.Comment2}\"";
                                if (m_data.metadata.Contains("Timestamp"))
                                    message += $",\n \"{sigRef}.Timestamp\": \"{m_data.timeStamp}\"";
                            }
                            message += "\n}";
                            using (var p = producerBuilder.Build())
                            {
                                var dr = p.ProduceAsync(m_data.topicName, new Message<string, string> { Key = ReplacePlaceholdersKey(m_data.key), Value = message }).Result;
                            }
                        }
                        else if (m_data.Format == KafkaWriterTaskData.DataFormat.JSONPerSignal)
                        {
                            var producerBuilder = new ProducerBuilder<string, string>(config);
                            InitIBuilder(m_data, producerBuilder);
                            foreach (var rec in m_data.Records)
                            {
                                string message = $"{{ \n\"Signal\": \"{ReplacePlaceholders(rec, m_data.signalReference)}\",\n \"Value\":{m_data.ToText(rec)}";
                                if (m_data.metadata.Contains("Identifier"))
                                    message += $",\n \"Identifier\": \"{m_data.identifier}\"";
                                if (m_data.metadata.Contains("Name"))
                                    message += $",\n \"Name\": \"{rec.Name}\"";
                                if (m_data.metadata.Contains("Unit"))
                                    message += $",\n \"Unit\": \"{rec.Unit}\"";
                                if (m_data.metadata.Contains("Comment 1"))
                                    message += $",\n \"Comment1\": \"{rec.Comment1}\"";
                                if (m_data.metadata.Contains("Comment 2"))
                                    message += $",\n \"Comment2\": \"{rec.Comment2}\"";
                                if (m_data.metadata.Contains("Timestamp"))
                                    message += $",\n \"Timestamp\": \"{m_data.timeStamp}\"";
                                message += "\n}";
                                using (var p = producerBuilder.Build())
                                {
                                    var dr = p.ProduceAsync(m_data.topicName, new Message<string, string> { Key = ReplacePlaceholders(rec, m_data.key), Value = message }).Result;
                                }
                            }
                        }
                        else if (m_data.Format == KafkaWriterTaskData.DataFormat.AVRO)
                        {
                            var schemaFingerPrint = schemaFingerPrintDefault;
                            if (m_data.schemaRegistryAddress != "")
                                schemaFingerPrint = GetSchemaFingerprint(m_data);
                            var schema = schemaDefault;
                            
                            ProducerBuilder<byte[], byte[]> producerBuilder = new Confluent.Kafka.ProducerBuilder<byte[], byte[]>(config);
                            InitIBuilder(m_data, producerBuilder);

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
                                r.Add("Signal", ReplacePlaceholders(rec, m_data.signalReference));
                                if (m_data.metadata.Contains("Identifier"))
                                    r.Add("Identifier", m_data.identifier);
                                if (m_data.metadata.Contains("Name"))
                                    r.Add("Name", rec.Name);
                                if (m_data.metadata.Contains("Unit"))
                                    r.Add("Unit", rec.Unit);
                                if (m_data.metadata.Contains("Comment 1"))
                                    r.Add("Comment1", rec.Comment1);
                                if (m_data.metadata.Contains("Comment 2"))
                                    r.Add("Comment2", rec.Comment2);

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

                                msg.Key = Encoding.UTF8.GetBytes(ReplacePlaceholders(rec, m_data.key).ToCharArray());
                                using (var p = producerBuilder.Build())
                                {
                                    var dr = p.ProduceAsync(m_data.topicName, msg);
                                }
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
    
        internal byte[] GetSchemaFingerprint(KafkaWriterTaskData data)
        {
            if (data.schemaRegistryAddress == schemaRegistryAddressCached && schemaFingerprintCached != null && schemaFingerprintCached.Length > 0)
                return schemaFingerprintCached;

            SchemaRegistryConfig schemaRegConfig = new SchemaRegistryConfig();
            schemaRegConfig.Url = data.schemaRegistryAddress;
            schemaRegConfig.RequestTimeoutMs = (int)data.timeout * 1000;
            schemaRegConfig.EnableSslCertificateVerification = data.schemaEnableSSLVerification;
            if (data.SchemaRegistrySecurityMode == KafkaWriterTaskData.SchemaRegistrySecurityType.HTTP_AUTHENTICATION ||
                data.SchemaRegistrySecurityMode == KafkaWriterTaskData.SchemaRegistrySecurityType.HTTPS_AUTHENTICATION)
            {
                schemaRegConfig.BasicAuthCredentialsSource = AuthCredentialsSource.UserInfo;
                schemaRegConfig.BasicAuthUserInfo = data.schemaUsername + ":" + data.schemaPass;
            }


            // Add expert parameters
            foreach (var kvp in data.Params)
            {
                if (string.IsNullOrEmpty(kvp.Key) || !kvp.Key.StartsWith("schema.registry."))
                    continue;

                schemaRegConfig.Set(kvp.Key, kvp.Value);
            }

            CachedSchemaRegistryClient schemRegClient = null;
            try
            {
                if (data.SchemaRegistrySecurityMode == KafkaWriterTaskData.SchemaRegistrySecurityType.HTTPS ||
                    data.SchemaRegistrySecurityMode == KafkaWriterTaskData.SchemaRegistrySecurityType.HTTPS_AUTHENTICATION)
                {
                    X509Certificate2 CACert = null;
                    X509Certificate2 clientCert = null;
                    if (!String.IsNullOrEmpty(data.schemaSSLCAThumbprint) && data.schemaEnableSSLVerification)
                    {
                        var c = TaskManager.Manager.CertificateManager.GetCertificate(data.schemaSSLCAThumbprint);
                        if (c != null)
                            CACert = c.Certificate;
                    }
                    if (!String.IsNullOrEmpty(data.schemaSSLClientThumbprint))
                    {
                        var c = TaskManager.Manager.CertificateManager.GetCertificate(data.schemaSSLClientThumbprint);
                        if (c != null)
                            clientCert = c.Certificate;
                    }
                    schemRegClient = new CachedSchemaRegistryClient(schemaRegConfig, CACert, clientCert);
                }
                else
                {
                    schemRegClient = new CachedSchemaRegistryClient(schemaRegConfig);
                }

                string subject = SubjectNameStrategy.Topic.ConstructValueSubjectName(data.topicName, null);
                Task<int> task = schemRegClient.RegisterSchemaAsync(subject, KafkaWriterTaskWorker.schemaDefault.ToString());
                if (!task.Wait((int)data.timeout * 1000))
                    throw new TimeoutException();

                if (!task.IsCompleted)
                {
                    if (task.Exception != null)
                        throw task.Exception;
                    else
                        throw new Exception("Failed to get schema ID from address server.");
                }
                int schemaId = task.Result;
                schemaRegistryAddressCached = data.schemaRegistryAddress;
                schemaFingerprintCached = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(schemaId));

                return schemaFingerprintCached;
            }
            finally
            {
                schemRegClient?.Dispose();
            }
        }


        internal static ProducerConfig InitConfig(KafkaWriterTaskData data)
        {
            var config = new ProducerConfig();

            if (data.ClusterMode == KafkaWriterTaskData.ClusterType.Kafka)
            {
                config.BootstrapServers = data.clusterAddress;
            }
            else
            {
                config.BootstrapServers = "";

                string[] connStringParts = data.clusterAddress.Split(';');
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

                config.SecurityProtocol = SecurityProtocol.SaslSsl;
                config.SaslMechanism = SaslMechanism.Plain;
                config.SaslUsername = "$ConnectionString";
                config.SaslPassword = data.clusterAddress;
            }

            config.EnableSslCertificateVerification = data.enableSSLVerification;
            config.ClientId = data.identifier;
            config.MessageTimeoutMs = (int)(data.timeout * 1000);
            switch (data.AckMode)
            {
                case KafkaWriterTaskData.RequiredAcks.None:
                    config.Acks = Acks.None;
                    break;
                case KafkaWriterTaskData.RequiredAcks.Leader:
                    config.Acks = Acks.Leader;
                    break;
                case KafkaWriterTaskData.RequiredAcks.All:
                    config.Acks = Acks.All;
                    break;
            }

            switch (data.ClusterSecurityMode)
            {
                case KafkaWriterTaskData.ClusterSecurityType.PLAINTEXT:
                    config.SecurityProtocol = SecurityProtocol.Plaintext;
                    break;
                case KafkaWriterTaskData.ClusterSecurityType.SSL:
                    config.SecurityProtocol = SecurityProtocol.Ssl;
                    break;
                case KafkaWriterTaskData.ClusterSecurityType.SASL_PLAINTEXT:
                    config.SecurityProtocol = SecurityProtocol.SaslPlaintext;
                    break;
                case KafkaWriterTaskData.ClusterSecurityType.SASL_SSL:
                    config.SecurityProtocol = SecurityProtocol.SaslSsl;
                    break;
            }

            if (data.ClusterSecurityMode == KafkaWriterTaskData.ClusterSecurityType.SASL_PLAINTEXT ||
                data.ClusterSecurityMode == KafkaWriterTaskData.ClusterSecurityType.SASL_SSL)
            {
                switch (data.SASLMechanismMode)
                {
                    case KafkaWriterTaskData.SASLMechanismType.PLAIN:
                        config.SaslMechanism = SaslMechanism.Plain;
                        break;
                    case KafkaWriterTaskData.SASLMechanismType.SCRAM_SHA_256:
                        config.SaslMechanism = SaslMechanism.ScramSha256;
                        break;
                    case KafkaWriterTaskData.SASLMechanismType.SCRAM_SHA_512:
                        config.SaslMechanism = SaslMechanism.ScramSha512;
                        break;

                }
                config.SaslUsername = data.SASLUsername;
                config.SaslPassword = data.SASLPass;
            }
            // Add expert parameters
            foreach (var kvp in data.Params)
            {
                if (string.IsNullOrEmpty(kvp.Key) || kvp.Key.StartsWith("schema.registry."))
                    continue;

                config.Set(kvp.Key, kvp.Value);
            }

            return config;
        }

        internal static void InitIBuilder(KafkaWriterTaskData data, IBuilder builder)
        {
            if (data.SSLClientThumbprint != null && data.SSLClientThumbprint != "")
            {
                var cert = TaskManager.Manager.CertificateManager.GetCertificate(data.SSLClientThumbprint);
                if (cert is null)
                    throw new Exception("No client certificate found with thumbprint " + data.SSLClientThumbprint);
                builder.SetSSLClientCert(cert.Certificate);
            }
            if (data.enableSSLVerification && data.SSLCAThumbprint != null && data.SSLCAThumbprint != "")
            {
                var cert = TaskManager.Manager.CertificateManager.GetCertificate(data.SSLCAThumbprint);
                if (cert is null)
                    throw new Exception("No client certificate found with thumbprint " + data.SSLCAThumbprint);
                builder.SetSSLCaCert(cert.Certificate);
            }
        }

        internal static object TestConnection(KafkaWriterTaskData data)
        {
            CachedSchemaRegistryClient schemRegClient = null;
            IAdminClient adminClient = null;
            try 
            {
                string[] topics = {};
                var conf = InitConfig(data);

                var adminClientBuilder = new AdminClientBuilder(conf);
                InitIBuilder(data, adminClientBuilder);
                adminClient = adminClientBuilder.Build();

                Metadata m = adminClient.GetMetadata(TimeSpan.FromSeconds(data.timeout));
                topics = m.Topics.Select(t => t.Topic).ToArray();
                if (data.schemaRegistryAddress == "")
                    return topics;

                Confluent.SchemaRegistry.SchemaRegistryConfig schemRegConfig = new Confluent.SchemaRegistry.SchemaRegistryConfig();
                schemRegConfig.Url = data.schemaRegistryAddress;
                schemRegConfig.RequestTimeoutMs = (int)(data.timeout * 1000);

                // Add expert parameters
                foreach (var par in data.Params)
                {
                    if (string.IsNullOrEmpty(par.Key) || !par.Key.StartsWith("schema.registry."))
                        continue;

                    schemRegConfig.Set(par.Key, par.Value);
                }

                schemRegClient = new Confluent.SchemaRegistry.CachedSchemaRegistryClient(schemRegConfig);
                if (!(schemRegClient.GetAllSubjectsAsync()).Wait((int)(data.timeout * 1000)))
                {
                    throw new Exception("Error connecting to schema registry: Timeout.");
                }
                return topics;
            }
            catch (AggregateException aggrEx)
            {
                string errorMsg = null;
                foreach (Exception exc in aggrEx.InnerExceptions)
                {
                    string curMsg = exc.Message;
                    if (exc is System.Net.Http.HttpRequestException httpExc)
                    {
                        //if (httpExc.InnerException is iba.Kafka.SchemaRegistry.SslVerificationException sslVerExc)
                        //{
                        //    // Log extra info
                        //    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                        //    sb.AppendLine("Kafka Schema Registry authentication error details:");
                        //    sb.AppendLine();
                        //    sb.AppendLine($"Request URI: {sslVerExc.RequestURI}");
                        //    sb.AppendLine($"SSL Policy errors: {sslVerExc.SSLErrors}");
                        //    sb.AppendLine();
                        //    sb.AppendLine($"Certificate:");
                        //    sb.AppendLine();
                        //    sb.AppendLine(sslVerExc.Certificate);
                        //    sb.AppendLine();
                        //    sb.AppendLine($"Certificate chain:");
                        //    sb.AppendLine();
                        //    sb.AppendLine(sslVerExc.Chain);

                        //    iba.Logging.ibaLogger.DebugFormat(sb.ToString());
                        //}
                    }

                    errorMsg = (errorMsg == null) ? curMsg : string.Concat(errorMsg, Environment.NewLine, curMsg);
                }
                if (errorMsg != null)
                    return new Exception(errorMsg);
            }
            catch (Exception e)
            {
                return new Exception(e.Message);
            }
            finally
            {
                if (schemRegClient != null)
                    schemRegClient.Dispose();
                if (adminClient != null)
                    adminClient.Dispose();
            }
            return null;
        }    
    }
}
