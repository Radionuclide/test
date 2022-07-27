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
using iba.CertificateStore;

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
                                                ""name"": ""DatCoordinatorRecord"",
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
                    string timeStamp = "";
                    DateTime timeStampDt = DateTime.Now;

                    foreach (var record in m_data.Records)
                    {
                        if (record.DataType == KafkaRecord.ExpressionType.Text)
                        {
                            object oValues = null;
                            mon.Execute(delegate () { m_ibaAnalyzer.EvaluateToStringArray(record.Expression, 0, out _, out oValues); });

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
                            mon.Execute(delegate () { channelMetaData = m_ibaAnalyzer.GetChannelMetaData(record.Expression); });
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
                            (channelMetaData as IDisposable)?.Dispose();
                        }
                    }


                    if (m_data.metadata.Contains("Timestamp") && m_data.Format != KafkaWriterTaskData.DataFormat.AVRO)
                    {
                        var fileInfo = IbaFileReader.ReadShortFileInfo(filename);

                        if (m_data.timeStampExpression == StartTime)
                        {
                            timeStampDt = fileInfo.StartTime;
                        }
                        else if (m_data.timeStampExpression == EndTime)
                        {
                            timeStampDt = fileInfo.EndTime;
                        }
                        else
                        {
                            timeStampDt = fileInfo.StartTime;
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

                        TimeSpan UTCOffset;
                        if (fileInfo.UtcOffsetValid)
                            UTCOffset = new TimeSpan(0, fileInfo.UtcOffset, 0); // fileInfo.UtcOffset in minutes
                        else 
                            UTCOffset = TimeZoneInfo.Local.GetUtcOffset(DateTime.UtcNow); // use local UTC

                        string sighn;

                        if (UTCOffset.Hours > 14)
                        {
                            UTCOffset = new TimeSpan(1, 0, 0, 0).Subtract(UTCOffset);
                            sighn = "-";
                        }
                        else
                            sighn = "+";

                        if (m_data.timestampUTCOffset == TimestampUTCOffset.ConvertToUniversalTime)
                        {
                            if (sighn == "+")
                                timeStampDt = timeStampDt.Subtract(UTCOffset);
                            else
                                timeStampDt = timeStampDt.Add(UTCOffset);
                            timeStamp = timeStampDt.ToString("yyyy.MM.ddTHH:mm:ss:fffffff") + "Z";
                        }
                        else if(m_data.timestampUTCOffset == TimestampUTCOffset.ConcatenateWithTimestamp || m_data.Format != KafkaWriterTaskData.DataFormat.AVRO)
                        {


                            timeStamp = 
                                timeStampDt.ToString("yyyy.MM.ddTHH:mm:ss:fffffff") +
                                sighn +
                                UTCOffset.ToString(@"hh\:mm");
                        }
                        // else ignore

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
                                message += $"\"{sigRef}\": {m_data.ToText(rec)}";
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
                                    message += $",\n \"{sigRef}.Timestamp\": \"{timeStamp}\"";
                            }
                            message += "\n}";
                            using (var p = producerBuilder.Build())
                            {
                                _ = p.ProduceAsync(m_data.topicName, new Message<string, string> { Key = ReplacePlaceholdersKey(m_data.key), Value = message }).Result;
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
                                    message += $",\n \"Timestamp\": \"{timeStamp}\"";
                                message += "\n}";
                                using (var p = producerBuilder.Build())
                                {
                                    _ = p.ProduceAsync(m_data.topicName, new Message<string, string> { Key = ReplacePlaceholders(rec, m_data.key), Value = message }).Result;
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
                                msg.Timestamp = new Confluent.Kafka.Timestamp(timeStampDt, Confluent.Kafka.TimestampType.CreateTime); 

                                var r = new Avro.Generic.GenericRecord(schema);
                                schema.TryGetField("ValueType", out Avro.Field enumField);
                                Avro.EnumSchema valTypeSchema = enumField.Schema as Avro.EnumSchema;
                                r.Add("ValueType", new Avro.Generic.GenericEnum(valTypeSchema, rec.DataTypeAsString));
                                r.Add("Signal", ReplacePlaceholders(rec, m_data.signalReference));
                                r.Add("Identifier", m_data.identifier);
                                r.Add("ID", "");
                                r.Add("Name", rec.Name);
                                r.Add("Unit", rec.Unit);
                                r.Add("Comment1", rec.Comment1);
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
                                    _ = p.ProduceAsync(m_data.topicName, msg).Result;
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
                    X509Certificate2 clientCert = null;
                    if (!String.IsNullOrEmpty(data.schemaSSLClientThumbprint))
                    {
                        var c = TaskManager.Manager.CertificateManager.GetCertificate(data.schemaSSLClientThumbprint, CertificateRequirement.None, out var _);
                        if (c != null)
                            clientCert = c.GetX509Certificate2();
                    }
                    KafkaSchemaRegistrySSLVerifier sslVerCtxt = new KafkaSchemaRegistrySSLVerifier(data.schemaRegistryAddress);
                    schemRegClient = new CachedSchemaRegistryClient(schemaRegConfig, null, null, clientCert, sslVerCtxt.SchemaRegistrySSLVerifyCallback);
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
                var cert = TaskManager.Manager.CertificateManager.GetCertificate(data.SSLClientThumbprint, CertificateStore.CertificateRequirement.None, out var _);
                if (cert is null)
                    throw new Exception("No client certificate found with thumbprint " + data.SSLClientThumbprint);
                builder.SetSSLClientCert(cert.GetX509Certificate2());
            }
            if (data.enableSSLVerification)
            {
                var sslVerCtxt = new KafkaSSLVerifier(data.clusterAddress);
                builder.SetSSLVerificationCallback(sslVerCtxt.OnSSLVerification);
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
                if (!data.enableSchema || data.schemaRegistryAddress == "")
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


        public class KafkaSSLVerifier
        {
            public KafkaSSLVerifier(string clusterAddress)
            {
                this.clusterAddress = clusterAddress;
                errorMsg = "";
                chainList = new System.Collections.Generic.List<Org.BouncyCastle.X509.X509Certificate>();
            }

            string clusterAddress;

            string errorMsg;
            public string ErrorMessage => errorMsg;

            System.Collections.Generic.List<Org.BouncyCastle.X509.X509Certificate> chainList;

            public bool OnSSLVerification(string broker_name, int broker_id, ref int x509_error, int depth, byte[] derCert, ref string errorStr)
            {
                Org.BouncyCastle.X509.X509Certificate cert = new Org.BouncyCastle.X509.X509Certificate(derCert);
                if (!cert.Equals((chainList.Count == 0) ? null : chainList[0]))
                    chainList.Insert(0, cert);
                if (depth > 0)
                {
                    // Build a chain until depth becomes 0; then we'll decide
                    // Clear X509 error
                    x509_error = 0;
                    return true;
                }
                else
                {
                    if (TaskManager.Manager.CertificateManager.VerifyChain(cert, chainList.ToArray(), out errorMsg))
                    {
                        // Clear X509 error
                        x509_error = 0;
                        return true;
                    }
                }
                System.Text.StringBuilder sb = new System.Text.StringBuilder();
                sb.AppendLine($"Kafka data store - {clusterAddress} - Cluster SSL verification failed. Provided certificates:");
                sb.AppendLine($"  Certificate: {GetCertificateString(cert)}");
                for (int i = 0; i < chainList.Count; i++)
                    sb.AppendLine($"  Chain_{i}: {GetCertificateString(chainList[i])}");

                return false;
            }

            public static string GetCertificateString(Org.BouncyCastle.X509.X509Certificate cert)
            {
                return $"Thumbprint: {Certificate.GetThumbprintString(cert)} | Subject: {cert.SubjectDN} | Issuer: {cert.IssuerDN} | Validity: {cert.NotBefore.ToLocalTime()} until {cert.NotAfter.ToLocalTime()}";
            }
        }


        public class KafkaSchemaRegistrySSLVerifier
        {
            public KafkaSchemaRegistrySSLVerifier(string clusterAddress)
            {
                this.clusterAddress = clusterAddress;
                errorMsg = "";
            }

            string clusterAddress;
            string errorMsg;
            public string ErrorMessage => errorMsg;
            public bool SchemaRegistrySSLVerifyCallback(System.Net.Http.HttpRequestMessage httpRequestMessage, System.Security.Cryptography.X509Certificates.X509Certificate2 cert, System.Security.Cryptography.X509Certificates.X509Chain certChain, System.Net.Security.SslPolicyErrors policyErrors)
            {
                System.Collections.Generic.List<Org.BouncyCastle.X509.X509Certificate> chainList = new System.Collections.Generic.List<Org.BouncyCastle.X509.X509Certificate>();
                foreach (var chainEl in certChain.ChainElements)
                {
                    chainList.Add(Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(chainEl.Certificate));
                }

                Org.BouncyCastle.X509.X509Certificate bcCert = Org.BouncyCastle.Security.DotNetUtilities.FromX509Certificate(cert);

                if (!TaskManager.Manager.CertificateManager.VerifyChain(bcCert, chainList.ToArray(), out errorMsg))
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    sb.AppendLine($"Kafka data store - {clusterAddress} - Schema registry SSL verification failed. Provided certificates:");
                    sb.AppendLine($"  Certificate: {KafkaSSLVerifier.GetCertificateString(bcCert)}");
                    for (int i = 0; i < chainList.Count; i++)
                        sb.AppendLine($"  Chain_{i}: {KafkaSSLVerifier.GetCertificateString(chainList[i])}");

                    return false;
                }
                return true;
            }
        }
    }
}
