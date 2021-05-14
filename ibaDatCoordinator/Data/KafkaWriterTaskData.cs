using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using iba.Controls;

namespace iba.Data
{
    public class KafkaWriterTaskData : TaskData
    {
        public string clusterAddress;
        public string topicName;
        public double timeout;//in sec
        public List<Record> Records;
        public List<Param> Params;
        public string identifier;
        public MonitorData MonitorData { get; set; }
        public string TestDatFile { get; set; }
        public DigitalFormat digitalFormat;

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
                "Analog",
                "Text",
                "Digital"
            };
            public enum ExpressionType
            {
                Number = 0,
                Text = 1,
                Digital = 2
            }

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
                    DataType = this.DataType
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
