using iba.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public abstract class ComputedValuesTaskData : TaskData
    {
        [NonSerialized]
        internal AnalyzerManager m_analyzerManager;
        public List<Record> Records;
        public MonitorData MonitorData { get; set; }

        public ComputedValuesTaskData(ConfigurationData parent)
            : base(parent)
        {
            Records = new List<Record>();
            MonitorData = new MonitorData();
        }
        public ComputedValuesTaskData() : this(null) { }

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
            ComputedValuesTaskData d;
            if (this is OpcUaWriterTaskData)
                d = new OpcUaWriterTaskData(null);
            else
                d = new SnmpWriterTaskData(null);
            d.Records = Records.Select(r => (Record)r.Clone()).ToList();
            d.AnalysisFile = AnalysisFile;
            d.TestDatFile = TestDatFile;
            d.MonitorData = (MonitorData)MonitorData.Clone();
            return d;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            if (!(taskData is ComputedValuesTaskData other)) return false;
            if (other == this) return true;

            bool seq = Records.SequenceEqual(other.Records);
            return seq &&
                    AnalysisFile == other.AnalysisFile &&
                    TestDatFile == other.TestDatFile &&
                    MonitorData.IsSame(other.MonitorData);
		}

        /// <summary> Ensures there are no duplicate names in <see cref="ComputedValuesTaskData"/> records.
        /// Logs the Exception if duplicates are detected. </summary>
        /// <returns> true=ok if there are no duplicates, false=problems if we have duplicates </returns>
        public static bool AssertNoDuplicates(ComputedValuesTaskData cvTask)
        {
            var duplicatedName = cvTask.Records.GroupBy(r => r.Name).
                Where(g => g.Count() > 1).Select(y => y.Key).FirstOrDefault();

            if (duplicatedName != null)
            {
                var err = string.Format(iba.Properties.Resources.errExprNameNonUnique, duplicatedName);
                LogExtraData eData = new LogExtraData("", cvTask, cvTask.ParentConfigurationData);
                LogData.Data.Logger.Log(iba.Logging.Level.Exception, err, eData);
                return false; // we have duplicates
            }

            return true; // ok, no duplicates
        }

        public override int RequiredLicense => Licensing.LicenseId.Publish;
    }
}
