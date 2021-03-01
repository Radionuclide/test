using iba.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class OPCUAWriterTaskData : TaskData
    {
        internal AnalyzerManager m_analyzerManager;
        public List<Record> Records;

        public OPCUAWriterTaskData(ConfigurationData parent)
            : base(parent)
        {
            Records = new List<Record>();
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
            public bool IsValid()
            {
                return !String.IsNullOrEmpty(Expression);
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

            public int CompareTo(Record other)
            {
                throw new NotImplementedException();
            }
        }

        public string FolderName { get; set; }

        public string TestDatFile { get; set; }

        public void EvaluateValues(string datFile)
        {
            Evaluate(datFile, false);
            ExtMonData.Instance.UpdateComputedValuesFolderValues(this);
        }

        public void UpdateStructure()
        {
            foreach (var record in Records)
            {
                // TODO must names be unique?
                if (record.Name.Length == 0)
                    record.Name = record.Expression;
            }
            ExtMonData.Instance.RebuildComputedValuesFolder(this);
        }

        public void Remove()
        {
            ExtMonData.Instance.RemoveComputedValuesFolder(this);
        }
		public override TaskData CloneInternal()
		{
            OPCUAWriterTaskData d = new OPCUAWriterTaskData(null);
            d.Records = Records.Select(r => (Record)r.Clone()).ToList();
            d.FolderName = FolderName;
            d.m_analyzerManager = m_analyzerManager;
            d.AnalysisFile = AnalysisFile;
            d.TestDatFile = TestDatFile;
            return d;
        }
        public override bool IsSameInternal(TaskData taskData)
		{
            System.Diagnostics.Debug.Assert(false);
			throw new NotImplementedException();
		}

        public void EvaluateTestValues()
        {
            Evaluate(TestDatFile, true);
        }

        private void Evaluate(string datFile, bool isTest)
        {
            m_analyzerManager.UpdateSource(AnalysisFile, datFile, "");
            string err;
            m_analyzerManager.OpenAnalyzer(out err);
            if (err.Length == 0)
                foreach (var record in Records)
                {
                    if (record.DataType == Record.ExpressionType.Text)
                    {
                        object oStamps = null;
                        object oValues = null;
                        m_analyzerManager.Analyzer.EvaluateToStringArray(record.Expression, 0, out oStamps, out oValues);

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
                            record.TestValue = m_analyzerManager.Analyzer.EvaluateDouble(record.Expression, 0);
                        else
                            record.Value = m_analyzerManager.Analyzer.EvaluateDouble(record.Expression, 0);
                    }
                }
        }
    }
}
