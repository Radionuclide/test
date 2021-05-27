using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using Confluent.SchemaRegistry;
using iba.Controls;

namespace iba.Data
{
    public class KafkaWriterTaskData : TaskData
    {
        public string clusterAddress;
        public string schemaRegistryAddress;
        public bool useSchemaRegistryServer;
        public string topicName;
        public double timeout;//in sec
        public List<Record> Records;
        public List<Param> Params;
        public string identifier;
        public MonitorData MonitorData { get; set; }
        public string TestDatFile { get; set; }
        public DigitalFormat digitalFormat;

        private string schemaRegistryAddressCached;
        private byte[] schemaFingerprintCached;



        public enum DigitalFormat
        {
            TrueFalse = 0,
            OneZero = 1
        }

        public enum DataFormat
        {
            JSONGrouped,
            JSONPerSignal,
            AVRO
        }
        public enum RequiredAcks
        {
            None = 0,   // No acknowledgments are required
            Leader, // Only an acknowledgment from the leader broker is required
            All     // An acknowledgment from all brokers is required
        }

        public byte[] schemaFingerPrint
        {
            get
            {
                if (schemaRegistryAddress == schemaRegistryAddressCached && schemaFingerprintCached.Length > 0)
                    return schemaFingerprintCached;

                Confluent.SchemaRegistry.SchemaRegistryConfig schemRegConfig = new Confluent.SchemaRegistry.SchemaRegistryConfig();
                schemRegConfig.Url = schemaRegistryAddress;
                schemRegConfig.RequestTimeoutMs = (int)timeout * 1000;

                // Add expert parameters
                foreach (var kvp in Params)
                {
                    if (string.IsNullOrEmpty(kvp.Key))
                        continue;

                    if (!kvp.Key.StartsWith("schema.registry."))
                        continue;

                    schemRegConfig.Set(kvp.Key, kvp.Value);
                }

                Confluent.SchemaRegistry.CachedSchemaRegistryClient schemRegClient = null;
                try
                {
                    schemRegClient = new Confluent.SchemaRegistry.CachedSchemaRegistryClient(schemRegConfig);

                    string subject = Confluent.SchemaRegistry.SubjectNameStrategy.Topic.ConstructValueSubjectName(topicName, null);
                    System.Threading.Tasks.Task<int> task = schemRegClient.RegisterSchemaAsync(subject, Processing.KafkaWriterTaskWorker.schemaDefault.ToString());
                    if (!task.Wait((int)timeout * 1000))
                        throw new TimeoutException();

                    if (!task.IsCompleted)
                    {
                        if (task.Exception != null)
                            throw task.Exception;
                        else
                            throw new Exception("Failed to get schema ID from address server.");
                    }
                    int schemaId = task.Result;
                    schemaRegistryAddressCached = schemaRegistryAddress;
                    schemaFingerprintCached = BitConverter.GetBytes(System.Net.IPAddress.HostToNetworkOrder(schemaId));

                    return schemaFingerprintCached;
                }
                finally
                {
                    if (schemRegClient != null)
                        schemRegClient.Dispose();
                }
            }
        }


        public string ToText(Record rec)
        {
            if (rec.DataType == Record.ExpressionType.Digital)
            {
                if (digitalFormat == DigitalFormat.TrueFalse)
                    return (double)rec.Value >= 0.5 ? "True" : "False";
                else
                    return (double)rec.Value >= 0.5 ? "1" : "0";
            }
            else 
                return rec.Value.ToString();
        }

        public RequiredAcks AckMode { get; set; }
        public DataFormat Format { get; set; }

        [Serializable]
        public class Record : ICloneable
        {
            public static readonly string[] DataTypes =
            {
                "DOUBLE",
                "STRING",
                "BOOLEAN"
            };
            public enum ExpressionType
            {
                Number = 0,
                Text = 1,
                Digital = 2
            }

            public string Metadata { get; set; }

            [XmlIgnore]
            public ExpressionType DataType
            {
                get; set;
            }
            public string DataTypeAsString
            {
                get { return DataTypes[(int)DataType]; }
                set
                {
                    if (value == DataTypes[(int)ExpressionType.Number])
                        DataType = ExpressionType.Number;
                    else if (value == DataTypes[(int)ExpressionType.Text])
                        DataType = ExpressionType.Text;
                    else if (value == DataTypes[(int)ExpressionType.Digital])
                        DataType = ExpressionType.Digital;
                }
            }
            public string Expression { get; set; }
            public object Value { get; set; }

            [XmlIgnore]
            public string TestValue { get; set; }
            public string Name { get; set; }

            public Record()
            {
                Expression = "";
                Value = double.NaN;
            }

            public object Clone()
            {
                return new Record
                {
                    Expression = this.Expression,
                    Value = this.Value,
                    DataType = this.DataType,
                    Name = this.Name,
                    Metadata = this.Metadata
                };
            }
            public override bool Equals(object obj)
            {
                if (!(obj is Record))
                    return false;
                var rec = (Record)obj;
                return
                    (
                        Expression == rec.Expression &&
                        //Name == rec.Name &&
                        DataType == rec.DataType
                    );
            }

            public override int GetHashCode()
            {
                return Expression.GetHashCode() ^ DataType.GetHashCode();

            }
        }


        [Serializable]
        public class Param : ICloneable
        {
            public string Key { get; set; }
            public string Value { get; set; }

            public object Clone()
            {
                return new Param
                {
                    Key = this.Key,
                    Value = this.Value
                };
            }
        }
        public KafkaWriterTaskData(ConfigurationData parent) : base(parent)
        {
            Records = new List<Record>();
            Params = new List<Param>();
            MonitorData = new MonitorData();
            topicName = "ibaDatCo-Test";
            clusterAddress = "192.168.150.63:9092";
            schemaRegistryAddress = "";
            useSchemaRegistryServer = false;
        }

        public override TaskData CloneInternal()
        {
            KafkaWriterTaskData d = new KafkaWriterTaskData(null);
            d.Records = Records.Select(r => (Record)r.Clone()).ToList();
            d.Params = Params.Select(r => (Param)r.Clone()).ToList();
            d.timeout = timeout;
            d.Format = Format;
            d.digitalFormat = digitalFormat;
            d.identifier = identifier;
            d.AnalysisFile = AnalysisFile;
            d.MonitorData = (MonitorData)MonitorData.Clone();
            d.schemaRegistryAddress = schemaRegistryAddress;
            d.schemaRegistryAddressCached = schemaRegistryAddressCached;
            d.schemaFingerprintCached = schemaFingerprintCached;
            d.useSchemaRegistryServer = useSchemaRegistryServer;
            return d;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            throw new System.NotImplementedException();
        }

        public void EvaluateValues(string filename, IbaAnalyzer.IbaAnalyzer ibaAnalyzer)
        {
            foreach (var record in Records)
            {
                if (record.DataType == Record.ExpressionType.Text)
                {
                    ibaAnalyzer.EvaluateToStringArray(record.Expression, 0, out _, out var oValues);

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
                    record.Value = ibaAnalyzer.EvaluateDouble(record.Expression, 0);
                }
            }
        }
    }
}
