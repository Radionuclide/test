using iba.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class OpcUaWriterTaskData : TaskData
    {
        [NonSerialized]
        internal AnalyzerManager m_analyzerManager;
        public List<Record> Records;
        public MonitorData MonitorData { get; set; }

        public OpcUaWriterTaskData(ConfigurationData parent)
            : base(parent)
        {
            Records = new List<Record>();
            MonitorData = new MonitorData();
        }
        public OpcUaWriterTaskData() : this(null) { }

        [Serializable]
        public class Record : ICloneable
        {
            public static readonly string[] dataTypes =
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

            public string Expression { get; set; }

            public string Name { get; set; }

            public object TestValue { get; set; }

            public object Value { get; set; }

            [XmlIgnore]
            public string TestValueString
            {
                get
                {
                    if (TestValue is double vald)
                        return (double.IsNaN(vald) || double.IsInfinity(vald)) ? "" : vald.ToString();
                    else if (TestValue is String vals)
                        return vals;
                    else if (TestValue is bool valb)
                        return valb ? "true" : "false";
                    else
                    {
                        System.Diagnostics.Debug.Assert(false);
                        return "type error";
                    }
                }
            }

            [XmlIgnore]
            public ExpressionType DataType
            {
                get; set;
            }
            public string DataTypeAsString
            {
                get { return dataTypes[(int)DataType]; }
                set
                {
                    if (value == dataTypes[(int)ExpressionType.Number])
                            DataType = ExpressionType.Number;
                    else if (value == dataTypes[(int)ExpressionType.Text])
                        DataType = ExpressionType.Text;
                    else if (value == dataTypes[(int)ExpressionType.Digital])
                        DataType = ExpressionType.Digital;
                }
            }

            public Record()
            {
                Name = "";
                Expression = "";
                TestValue = Double.NaN;
                Value = Double.NaN;
            }

            public object Clone()
            {
                System.Diagnostics.Debug.Assert(Value is double || Value is string);
                return new Record
                {
                    Expression = this.Expression,
                    Name = this.Name,
                    TestValue = this.TestValue,
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
                        Name == rec.Name &&
                        DataType == rec.DataType
                    );
            }

            public override int GetHashCode()
            {
                return Expression.GetHashCode() ^ Name.GetHashCode() ^ DataType.GetHashCode();
            }
        }

        public string TestDatFile { get; set; }

        public void EvaluateValues(string datFile, IbaAnalyzer.IbaAnalyzer analyzer)
        {
            foreach (var record in Records)
            {
                if (record.DataType == Record.ExpressionType.Text)
                {
                    analyzer.EvaluateToStringArray(record.Expression, 0, out _, out var oValues);

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
                    record.Value = analyzer.EvaluateDouble(record.Expression, 0);
                }
            }
        }

		public override TaskData CloneInternal()
		{
            OpcUaWriterTaskData d = new OpcUaWriterTaskData(null);
            d.Records = Records.Select(r => (Record)r.Clone()).ToList();
            d.Records.Add(new Record());
            Records.Clear();
            d.AnalysisFile = AnalysisFile;
            d.TestDatFile = TestDatFile;
            d.MonitorData = (MonitorData)MonitorData.Clone();
            return d;
        }

        public override bool IsSameInternal(TaskData taskData)
		{
            return
                (
                    taskData is OpcUaWriterTaskData other &&
                    Records.SequenceEqual(other.Records) &&
                    AnalysisFile == other.AnalysisFile &&
                    TestDatFile == other.TestDatFile &&
                    MonitorData == other.MonitorData
                );
		}
    }
}
