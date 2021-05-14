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
using Confluent.SchemaRegistry.Serdes;
using Confluent.SchemaRegistry;

namespace iba.Processing
{
    class KafkaWriterTaskWorker
    {
        private ConfigurationWorker m_confWorker;
        private KafkaWriterTaskData m_data;
        private StatusData m_sd;
        private IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer;

        internal KafkaWriterTaskWorker(ConfigurationWorker worker, KafkaWriterTaskData task)
        {
            m_confWorker = worker;
            m_data = task;
            m_sd = worker.m_sd;
            m_ibaAnalyzer = worker.m_ibaAnalyzer;
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
                //m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logOPCUAWriterStarted, filename, m_data);

                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_data.MonitorData))
                {
                    if (bUseAnalysis)
                        mon.Execute(delegate () { m_ibaAnalyzer.OpenAnalysis(m_data.AnalysisFile); });
                    mon.Execute(delegate ()
                    {
                        m_data.EvaluateValues(filename, m_ibaAnalyzer);
                    });

                    var config = new ProducerConfig { BootstrapServers = m_data.clusterAddress };

                    config.ClientId = m_data.identifier;// TODO test
                    config.MessageTimeoutMs = (int)(m_data.timeout*1000);
                    switch (m_data.AckMode) // TODO test
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

                    {
                        try
                        {
                            if (m_data.Format == KafkaWriterTaskData.DataFormat.JSONGrouped)
                            {
                                var p = new ProducerBuilder<string, string>(config).Build();
                                    string message = "{ ";

                                for (int i = 0; i < m_data.Records.Count; i++)
                                {
                                    if (i > 0)
                                        message += ", ";
                                    message += "\"" + m_data.Records[i].Expression + "\": " + m_data.ToText(m_data.Records[i]);
                                }
                                message += "}";
                                Debug.WriteLine($"BEFORE AWAIT {System.Threading.Thread.CurrentThread.Name}");
                                //System.Threading.Thread.Sleep(7);
                                var dr = p.ProduceAsync(m_data.topicName, new Message<string, string> {Value = message }).Result;
                                Debug.WriteLine($"AFTER AWAIT {System.Threading.Thread.CurrentThread.Name}");
                            }
                            else if (m_data.Format == KafkaWriterTaskData.DataFormat.JSONPerSignal)
                            {
                                var p = new ProducerBuilder<string, string>(config).Build();
                                foreach (var rec in m_data.Records)
                                {
                                    string message = "{ \"Signal\": \"" + rec.Expression + "\", \"Value\": " + m_data.ToText(rec) + "}";
                                    var dr = p.ProduceAsync(m_data.topicName, new Message<string, string> { Value = message }).Result;
                                }
                            }
                            else if (m_data.Format == KafkaWriterTaskData.DataFormat.AVRO)
                            {
                                var s = (Avro.RecordSchema)Avro.RecordSchema.Parse(
                                                                        @"{
                                            ""namespace"": ""Confluent.Kafka.Examples.AvroSpecific"",
                                            ""type"": ""record"",
                                            ""name"": ""User"",
                                            ""fields"": [
                                                {""name"": ""name"", ""type"": ""string""},
                                                {""name"": ""favorite_number"",  ""type"": [""int"", ""null""]},
                                                {""name"": ""favorite_color"", ""type"": [""string"", ""null""]}
                                            ]
                                          }"
                                            );  

                                //using (var schemaRegistry = new CachedSchemaRegistryClient(new SchemaRegistryConfig { Url = "" }))
                                {
                                    var p = new ProducerBuilder<string, string>(new ProducerConfig { BootstrapServers = m_data.clusterAddress })// TODO <string, Avro.Generic.GenericRecord>
                                    //.SetKeySerializer(new AvroSerializer<string>(schemaRegistry))
                                    //.SetValueSerializer(new AvroSerializer<Avro.Generic.GenericRecord>(schemaRegistry))
                                    //.SetValueSerializer(Avro.Generic.GenericRecord.)
                                    .Build();


                                    Trace.WriteLine($"{p.Name} producing");

                                    int i = 0;
                                    foreach (var rec in m_data.Records)
                                    {
                                        var record = rec.Value.ToString(); // TODOnew Avro.Generic.GenericRecord(s);
                                        //record.Add("name", rec.Expression);
                                        //record.Add("favorite_number", i++);
                                        //record.Add("favorite_color", "blue");

                                        try
                                        {
                                            var dr = p.ProduceAsync(m_data.topicName, new Message<string, string> { Key = rec.Expression, Value = record });
                                            Trace.WriteLine($"produced to: {dr.Result.Offset}");
                                        }
                                        catch (ProduceException<string, Avro.Generic.GenericRecord> ex)
                                        {
                                            // In some cases (notably Schema Registry connectivity issues), the InnerException
                                            // of the ProduceException contains additional informatiom pertaining to the root
                                            // cause of the problem. This information is automatically included in the output
                                            // of the ToString() method of the ProduceException, called implicitly in the below.
                                            Trace.WriteLine($"error producing message: {ex}");
                                        }
                                    }

                                }

                                //using (var serdeProvider = new AvroSerdeProvider(avroConfig))
                                //using (var producer = new Producer<string, GenericRecord>(producerConfig, serdeProvider.GetSerializerGenerator<string>(), serdeProvider.GetSerializerGenerator<GenericRecord>()))
                                //{
                                //    Console.WriteLine($"{producer.Name} producing on {topicName}. Enter user names, q to exit.");

                                //    int i = 0;
                                //    string text;
                                //    while ((text = Console.ReadLine()) != "q")
                                //    {
                                //        var record = new GenericRecord(s);
                                //        record.Add("name", text);
                                //        record.Add("favorite_number", i++);
                                //        record.Add("favorite_color", "blue");

                                //        producer
                                //            .ProduceAsync(topicName, new Message<string, GenericRecord> { Key = text, Value = record })
                                //            .ContinueWith(task => task.IsFaulted
                                //                ? $"error producing message: {task.Exception.Message}"
                                //                : $"produced to: {task.Result.TopicPartitionOffset}");
                                //    }
                                //}

                                //cts.Cancel();
                            }
                        }
                        catch (ProduceException<Null, string> ex)
                        {
                            Debug.WriteLine($"Delivery failed: {ex.Error.Reason}");
                        }
                    }


                    m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logComputedValuesUpdated/*TODO*/, filename, m_data);
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

/*
    public enum KafkaAVROSignalType
    {
        BOOLEAN,
        BYTES,
        DOUBLE,
        FLOAT,
        INT,
        LONG,
        STRING
    }

    public class KafkaRecordSerializer_AVROPerSignal
    {
        static KafkaRecordSerializer_AVROPerSignal()
        {
            schema = Avro.RecordSchema.Parse(perSignalSchema) as Avro.RecordSchema;
            if (schema == null)
                throw new Exception("Could not parse AVRO per signal schema string");

            Avro.Field enumField;
            if (schema.TryGetField("ValueType", out enumField))
                valTypeSchema = enumField.Schema as Avro.EnumSchema;
            else
                throw new Exception("Could not parse ValueType enum schema");
        }

        private readonly bool bSendOnlyChanged;
        private bool bFieldsWritten;

        /// <summary>
        /// Creates an instance of an AVRO per signal serializer
        /// </summary>
        /// <param name="bUseSchemaRegistry">When true, serialize using Confluent standard; when false, serialize using Apache standard</param>
        public KafkaRecordSerializer_AVROPerSignal(bool bUseSchemaRegistry, bool bSendOnlyChanged)
        {
            marker = bUseSchemaRegistry ? confluentAvroMarker : avroMarker;
            if (!bUseSchemaRegistry)
                schemaFingerprint = Avro.SchemaNormalization.ParsingFingerprint("CRC-64-AVRO", schema);

            this.bSendOnlyChanged = bSendOnlyChanged;

            // In case we use a schema registry the fingerprint will be set later

            datumWriter = new Avro.Generic.GenericDatumWriter<Avro.Generic.GenericRecord>(schema);
            genRec = new Avro.Generic.GenericRecord(schema);

            estSize = schema.Count * 4;    // Estimate 4 bytes per field; we'll correct this as we go
        }

        private const string perSignalSchema = @"{""namespace"": ""de.iba"",
 ""type"": ""record"",
 ""name"": ""PdaRecord"",
 ""fields"": [
     {""name"": ""Signal"", ""type"": ""string""},
     {""name"": ""ID"", ""type"": [""null"", ""string""]},
     {""name"": ""Name"", ""type"": [""null"", ""string""]},
     {""name"": ""Unit"", ""type"": [""null"", ""string""]},
     {""name"": ""Comment1"", ""type"": [""null"", ""string""]},
     {""name"": ""Comment2"", ""type"": [""null"", ""string""]},
     {""name"": ""Timestamp"",  ""type"": [""null"", {
         ""type"" : ""long"",
         ""logicalType"" : ""timestamp-micros""
     }]},
     {""name"": ""Identifier"", ""type"": [""null"", ""string""]},
     {""name"": ""ValueType"", ""type"": { 
         ""type"": ""enum"",
         ""name"": ""ValueTypeEnum"",
         ""symbols"" : [""BOOLEAN"", ""BYTES"", ""DOUBLE"", ""FLOAT"", ""INT"", ""LONG"", ""STRING""]
     }},
     {""name"": ""BooleanValue"", ""type"": [""null"", ""boolean""]},
     {""name"": ""BytesValue"", ""type"": [""null"", ""bytes""]},
     {""name"": ""DoubleValue"", ""type"": [""null"", ""double""]},
     {""name"": ""FloatValue"", ""type"": [""null"", ""float""]},
     {""name"": ""IntValue"", ""type"": [""null"", ""int""]},
     {""name"": ""LongValue"", ""type"": [""null"", ""long""]},
     {""name"": ""StringValue"", ""type"": [""null"", ""string""]}
 ]
}
";
        /// <summary>
        /// AVRO marker
        /// </summary>
        /// <remarks>See Apache AVRO specification (version 1.8.2; Single-object encoding)</remarks>
        protected static readonly byte[] avroMarker = new byte[] { 0xC3, 0x01 };
        protected static readonly byte[] confluentAvroMarker = new byte[] { 0x00 };

        protected byte[] marker;
        protected byte[] schemaFingerprint;
        public byte[] SchemaFingerprint
        {
            get => schemaFingerprint;
            set => schemaFingerprint = value;
        }

        protected static Avro.RecordSchema schema;
        protected static Avro.EnumSchema valTypeSchema;
        protected Avro.Generic.GenericRecord genRec;
        protected Avro.Generic.GenericDatumWriter<Avro.Generic.GenericRecord> datumWriter;

        //protected KafkaAVROSignalType sigType;

        protected int estSize;

        public static Avro.RecordSchema Schema
        {
            get { return schema; }
        }

        public static string ExportSchema()
        {
            return schema?.ToString();
        }

        public void SetSignalType()//DataType pdaType
        {
            //sigType = GetAvroSignalType(pdaType);

            //string valStr = "";
            //switch (sigType)
            //{
            //    case KafkaAVROSignalType.BOOLEAN:
            //        valStr = "BOOLEAN";
            //        break;
            //    case KafkaAVROSignalType.BYTES:
            //        valStr = "BYTES";
            //        break;
            //    case KafkaAVROSignalType.DOUBLE:
            //        valStr = "DOUBLE";
            //        break;
            //    case KafkaAVROSignalType.FLOAT:
            //        valStr = "FLOAT";
            //        break;
            //    case KafkaAVROSignalType.INT:
            //        valStr = "INT";
            //        break;
            //    case KafkaAVROSignalType.LONG:
            //        valStr = "LONG";
            //        break;
            //    case KafkaAVROSignalType.STRING:
            //        valStr = "STRING";
            //        break;
            //}

            // Set ValueType in record
            genRec.Add("ValueType", new Avro.Generic.GenericEnum(valTypeSchema, "STRING"));

            // Set default value for all value fields
            genRec.Add("BooleanValue", null);
            genRec.Add("BytesValue", null);
            genRec.Add("DoubleValue", null);
            genRec.Add("FloatValue", null);
            genRec.Add("IntValue", null);
            genRec.Add("LongValue", null);
            genRec.Add("StringValue", null);
        }

        public void Validate()
        {
            // Make sure all fields are present in record
            //foreach (Avro.Field field in schema.Fields)
            //{
            //    if (!genRec.TryGetValue(field.Name, out object dummy))
            //    {
            //        genRec.Add(field.Name, field.DefaultValue);
            //    }
            //}
        }

        public void Begin()
        {
            bFieldsWritten = false;
            if (schemaFingerprint == null)
                throw new Exception("Schema ID not found");
        }

        public void WriteField(KafkaFieldDefinition field, object val)
        {
            if (field.FieldType == KafkaFieldType.Signal_Numeric)
            {
                switch (sigType)
                {
                    case KafkaAVROSignalType.BYTES:
                        WriteFieldValue(field, genRec, "BytesValue", val);
                        break;
                    case KafkaAVROSignalType.DOUBLE:
                        WriteFieldValue(field, genRec, "DoubleValue", val);
                        break;
                    case KafkaAVROSignalType.FLOAT:
                        WriteFieldValue(field, genRec, "FloatValue", val);
                        break;
                    case KafkaAVROSignalType.INT:
                        WriteFieldValue(field, genRec, "IntValue", val);
                        break;
                    case KafkaAVROSignalType.LONG:
                        WriteFieldValue(field, genRec, "LongValue", val);
                        break;
                }
            }
            else if (field.FieldType == KafkaFieldType.Signal_Digital)
                WriteFieldValue(field, genRec, "BooleanValue", (float)val > 0.5);
            else if (field.FieldType == KafkaFieldType.Signal_Text)
                WriteFieldValue(field, genRec, "StringValue", val);
            else if (field.FieldType == KafkaFieldType.Timestamp)
                WriteFieldValue(field, genRec, field.Name, (DateTime)val, false);
            else
                WriteFieldValue(field, genRec, field.Name, val);
        }

        private void WriteFieldValue<T>(KafkaFieldDefinition field, Avro.Generic.GenericRecord genRec, string name, T fieldValue, bool bSignificantForChange = true)
        {
            if (!bSendOnlyChanged || !Equals(fieldValue, field.prevValue))
            {
                genRec.Add(name, fieldValue);
                field.prevValue = fieldValue;
                if (bSignificantForChange)
                    bFieldsWritten = true;
            }
        }

        public byte[] End()
        {
            if (bFieldsWritten)
            {
                var ms = new System.IO.MemoryStream(estSize);
                ms.Write(marker, 0, marker.Length);
                ms.Write(schemaFingerprint, 0, schemaFingerprint.Length);

                Avro.IO.BinaryEncoder encoder = new Avro.IO.BinaryEncoder(ms);
                datumWriter.Write(genRec, encoder);

                int bytesWritten = (int)ms.Position;
                if (bytesWritten > estSize)
                    estSize = bytesWritten;

                return ms.ToArray();
            }
            return new byte[0];
        }

        public void Dispose()
        {
        }

        public static KafkaAVROSignalType GetAvroSignalType(DataType dType)
        {
            if (DataTypeExt.IsText(dType))
                return KafkaAVROSignalType.STRING;
            else if (DataTypeExt.IsDigital(dType))
                return KafkaAVROSignalType.BOOLEAN;

            switch (dType)
            {
                case DataType.dtSInt8:
                case DataType.dtByte:
                case DataType.dtInt16:
                case DataType.dtUInt16:
                case DataType.dtInt32:
                case DataType.dtInt32Big:
                    return KafkaAVROSignalType.INT;
                case DataType.dtUInt32:
                case DataType.dtUInt32Big:
                case DataType.dtInt64:
                    return KafkaAVROSignalType.LONG;
                case DataType.dtFloat:
                    return KafkaAVROSignalType.FLOAT;
                case DataType.dtDouble:
                    return KafkaAVROSignalType.DOUBLE;
                default:
                    return KafkaAVROSignalType.FLOAT;
            }
        }
    }

*/


}
