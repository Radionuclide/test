using iba.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class OPCUAWriterTaskData : TaskData
    {
        [NonSerialized]
        internal AnalyzerManager m_analyzerManager;
        public List<Record> Records;
        public MonitorData MonitorData { get; set; }

        public OPCUAWriterTaskData(ConfigurationData parent)
            : base(parent)
        {
            Records = new List<Record>();
            MonitorData = new MonitorData();
        }
        public OPCUAWriterTaskData() : this(null) { }

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
                        return valb ? "true" : "false"; // TODO localization
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
                if (obj == null || !(obj is Record))
                    return false;
                var rec = obj as Record;
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
            Evaluate(false, analyzer);
            ExtMonData.Instance.UpdateComputedValuesFolderValues(this);
        }

        public void UpdateStructure()
        {
            Records.RemoveAll
            (
                (Record rec) =>
                {
                    return rec.Name == "" || rec.Expression == "";
                }
            );
            ExtMonData.Instance.RebuildComputedValuesFolder(this);
        }

        public void Remove()
        {
            ExtMonData.Instance.RemoveComputedValuesJobFolder(this);
        }
		public override TaskData CloneInternal()
		{
            OPCUAWriterTaskData d = new OPCUAWriterTaskData(null);
            d.Records = Records.Select(r => (Record)r.Clone()).ToList();
            d.AnalysisFile = AnalysisFile;
            d.TestDatFile = TestDatFile;
            d.MonitorData = (MonitorData)MonitorData.Clone();
            return d;
        }
        public override bool IsSameInternal(TaskData taskData)
		{
            var other = taskData as OPCUAWriterTaskData;
            return
                (
                    other != null &&
                    Records.SequenceEqual(other.Records) &&
                    AnalysisFile == other.AnalysisFile &&
                    TestDatFile == other.TestDatFile &&
                    MonitorData == other.MonitorData
                );
		}

        public void EvaluateTestValues()
        {
            string err;
            m_analyzerManager.UpdateSource(AnalysisFile, TestDatFile, "");
            m_analyzerManager.OpenAnalyzer(out err);
            if (err.Length > 0)
            {
                var msg = string.Format(iba.Properties.Resources.logOPCUAWriterEvaluatingTestValue, err);
                MessageBox.Show(msg, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Evaluate(true, m_analyzerManager.Analyzer);
        }

        private void Evaluate(bool isTest, IbaAnalyzer.IbaAnalyzer analyzer)
        {
            foreach (var record in Records)
            {
                if (record.DataType == Record.ExpressionType.Text)
                {
                    object oStamps = null;
                    object oValues = null;
                    analyzer.EvaluateToStringArray(record.Expression, 0, out oStamps, out oValues);

                    if (oValues != null)
                    {
                        string[] values = oValues as string[];
                        foreach (string str in values)
                        {
                            if (!string.IsNullOrEmpty(str))
                            {
                                if (isTest)
                                    record.TestValue = str;
                                else
                                    record.Value = str;
                                return;
                            }
                        }
                    }
                    if (isTest)
                        record.TestValue = "";
                    else
                        record.Value = "";
                }
                else
                {
                    if (isTest)
                        record.TestValue = analyzer.EvaluateDouble(record.Expression, 0);
                    else
                        record.Value = analyzer.EvaluateDouble(record.Expression, 0);
                }
            }
        }
    }
}
