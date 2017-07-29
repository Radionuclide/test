using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iba.Plugins;
using System.Xml.Serialization;

using S7_writer;

namespace S7_writer_plugin
{
    [Serializable]
    public class S7TaskData : IPluginTaskData, IPluginTaskDataIsSame, IPluginTaskDataIbaAnalyzer
    {

        #region IPluginTaskData Members

        [NonSerialized]
        private S7TaskControl m_control;
        public IPluginControl GetControl()
        {
            if(m_control == null)
                m_control = new S7TaskControl(m_datcoHost);
            return m_control;
        }

        [NonSerialized]
        private S7TaskWorker m_worker;

        public IPluginTaskWorker GetWorker()
        {
            if(m_worker == null) m_worker = new S7TaskWorker(this);
            return m_worker;
        }

        public void SetWorker(IPluginTaskWorker worker)
        {
            m_worker = worker as S7TaskWorker;
        }

        public string NameInfo
        {
            get { return m_nameInfo; }
            set { m_nameInfo = value; }
        }

        public S7TaskData()
        {
            InitData(null, null, null);
        }

        public int DongleBitPos
        {
            get
            {
                return 5;
            } 
        }

        public void Reset(IDatCoHost host)
        {
            m_datcoHost = host;
        }

        public void SetParentJob(IJobData data)
        {
            m_parentJob = data;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            S7TaskData res = new S7TaskData(m_nameInfo, m_datcoHost, m_parentJob);
            res.m_testDatFile = m_testDatFile;
            res.m_pdoFile = m_pdoFile;

            res.Records = new List<Record>(m_records.Count);
            for(int i = 0; i < m_records.Count; i++)
                res.m_records.Add((Record)m_records[i].Clone());

            res.S7Address = S7Address;
            res.S7Rack = S7Rack;
            res.S7Slot = S7Slot;
            res.S7Timeout = S7Timeout;
            res.S7ConnectionType = S7ConnectionType;
            res.AllowErrors = AllowErrors;

            res.m_monitorData = (iba.Data.MonitorData) m_monitorData.Clone();

            return res;
        }

        #endregion

        public S7TaskData(string name, IDatCoHost host, IJobData parentJob)
        {
            InitData(name, host, parentJob);
        }

        private IDatCoHost m_datcoHost;
        private IJobData m_parentJob;

        private string m_nameInfo;

        private void InitData(string name, IDatCoHost host, IJobData parentJob)
        {
            m_parentJob = parentJob;
            m_datcoHost = host;
            m_nameInfo = name;

            m_testDatFile = "";
            m_pdoFile = "";

            m_records = new List<Record>();

            m_s7ConnPars = new S7ConnectionParameters();
            m_bAllowErrors = false;
            m_monitorData = new iba.Data.MonitorData();
        }

        public bool IsSame(IPluginTaskDataIsSame data)
        {
            var other = data as S7TaskData;
            if (other == null) return false;
            if (m_testDatFile != other.m_testDatFile) return false;
            if (m_pdoFile != other.m_pdoFile) return false;
            if (!m_records.SequenceEqual(other.m_records)) return false;
            if (S7Address != other.S7Address) return false;
            if (S7Rack != other.S7Rack) return false;
            if (S7Slot != other.S7Slot) return false;
            if (S7Timeout != other.S7Timeout) return false;
            if (S7ConnectionType != other.S7ConnectionType) return false;
            if (AllowErrors != other.AllowErrors) return false;
            if (!m_monitorData.IsSame(other.m_monitorData)) return false;
            return true;
        }

        #region Data

        protected string m_pdoFile;
        public string AnalysisFile
        {
            get { return m_pdoFile; }
            set { m_pdoFile = value; }
        }

        private string m_testDatFile;
        public string TestDatFile
        {
            get { return m_testDatFile; }
            set { m_testDatFile = value; }
        }

        [Serializable]
        public class Record : ICloneable, IComparable<Record>
        {
            private string m_expression;
            public string Expression
            {
                get { return m_expression; }
                set { m_expression = value; }
            }

            private int m_dbNr;
            public int DBNr
            {
                get { return m_dbNr; }
                set { m_dbNr = Math.Max(0, value); }
            }

            private int m_address;
            public int Address
            {
                get { return m_address; }
                set { m_address = Math.Max(0, value); }
            }

            private int m_bitNr;
            public int BitNr
            {
                get { return m_bitNr; }
                set { m_bitNr = Math.Max(0, Math.Min(7, value)); }
            }

            private S7DataTypeEnum m_dataType;
            public S7DataTypeEnum DataType
            {
                get { return m_dataType; }
            }

            [XmlIgnore]
            public string DataTypeAsString
            {
                get { return S7.DataTypes[(int)m_dataType].name; }
                set
                {
                    string newVal = value.ToUpper();
                    for(int i=0; i<S7.DataTypes.Length; i++)
                    {
                        if(newVal == S7.DataTypes[i].name)
                        {
                            m_dataType = (S7DataTypeEnum)i;
                            return;
                        }
                    }
                    
                    //Unknown data type -> don't change
                }
            }

            private double m_testValue;
            public double TestValue
            {
                get { return m_testValue; }
                set { m_testValue = value; }
            }

            public string GetOperandName()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("DB ");
                sb.Append(DBNr);
                sb.Append(".DB");

                switch(S7.DataTypes[(int)m_dataType].size)
                {
                    case 0:
                        sb.Append("X");
                        sb.Append(Address);
                        sb.Append(".");
                        sb.Append(BitNr);
                        return sb.ToString();
                    case 1:
                        sb.Append("B");
                        break;
                    case 2:
                        sb.Append("W");
                        break;
                    default:
                        sb.Append("D");
                        break;
                }

                sb.Append(Address);
                return sb.ToString();
            }

            [XmlIgnore]
            public string TestValueString
            {
                get { return (double.IsNaN(m_testValue) || double.IsInfinity(m_testValue))?"":m_testValue.ToString(); }
            }

            public Record() //no nulls
            {
                m_dbNr = 1;
                m_address = 0;
                m_bitNr = 0;
                m_dataType = S7DataTypeEnum.S7Real;
                m_expression = "";
                m_testValue = Double.NaN;
            }

            public bool IsValid()
            {
                return !String.IsNullOrEmpty(m_expression);
            }

            #region ICloneable Members

            public object Clone()
            {
                Record res = new Record();
                res.m_dbNr = m_dbNr;
                res.m_address = m_address;
                res.m_bitNr = m_bitNr;
                res.m_dataType = m_dataType;
                res.m_expression = m_expression;
                res.m_testValue = m_testValue;
                return res;
            }
            #endregion

            public int CompareTo(Record other)
            {
                int diff = DBNr - other.DBNr;
                if (diff != 0)
                    return diff;

                diff = Address - other.Address;
                if (diff != 0)
                    return diff;

                int typeSize = S7.DataTypes[(int)DataType].size;
                int otherTypeSize = S7.DataTypes[(int)other.DataType].size;
                diff = otherTypeSize - typeSize; //Biggest data type first
                if (diff != 0)
                    return diff;

                if (DataType == S7DataTypeEnum.S7Bool)
                {
                    diff = BitNr - other.BitNr;
                    if (diff != 0)
                        return diff;
                }

                return m_expression.CompareTo(other.m_expression);
            }

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                return Equals(obj as Record);
            }

            public bool Equals(Record other)
            {
                if (other == null)
                    return false;
                else if (CompareTo(other) != 0)
                    return false;
                else if (DataType != other.DataType)
                    return false;
                else if (Expression != other.Expression)
                    return false;
                else
                    return true;
            }

            public override int GetHashCode()
            {
                if (Expression != null)
                    return Expression.GetHashCode();
                else
                    return base.GetHashCode();
            }
        }

        //// Custom comparer for the Record class
        //class RecordComparer : IEqualityComparer<Record>
        //{
        //    // Records are equal if their names and Record numbers are equal.
        //    public bool Equals(Record x, Record y)
        //    {

        //        //Check whether the compared objects reference the same data.
        //        if(Object.ReferenceEquals(x, y)) return true;

        //        //Check whether any of the compared objects is null.
        //        if(Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
        //            return false;

        //        //Check whether the Records' properties are equal.
        //        return x.Expression == y.Expression && x.DBNr == y.DBNr && x.Address == y.Address && x.BitNr == y.BitNr && x.DataType == y.DataType;
        //    }

        //    // If Equals() returns true for a pair of objects 
        //    // then GetHashCode() must return the same value for these objects.

        //    public int GetHashCode(Record record)
        //    {
        //        //Check whether the object is null
        //        if(Object.ReferenceEquals(record, null)) return 0;
        //        return record.Expression.GetHashCode() ^ record.DBNr ^ record.Address ^ record.BitNr ^ (int)record.DataType;
        //    }

        //}


        private List<Record> m_records;

	    public List<Record> Records
	    {
		    get { return m_records; }
		    set { m_records = value; }
	    }

        private S7ConnectionParameters m_s7ConnPars;
        public string S7Address
        {
            get { return m_s7ConnPars.Address; }
            set { m_s7ConnPars.Address = value ?? ""; }
        }

        public int S7Rack
        {
            get { return m_s7ConnPars.Rack; }
            set { m_s7ConnPars.Rack = Math.Max(0, value); }
        }

        public int S7Slot
        {
            get { return m_s7ConnPars.Slot; }
            set { m_s7ConnPars.Slot = Math.Max(0, value); }
        }

        public int S7Timeout
        {
            get { return m_s7ConnPars.TimeoutInSec; }
            set { m_s7ConnPars.TimeoutInSec = Math.Max(1, value); }
        }

        public int S7ConnectionType
        {
            get { return m_s7ConnPars.ConnType; }
            set { m_s7ConnPars.ConnType = Math.Max(0, Math.Min(2, value)); }
        }

        private bool m_bAllowErrors;
        public bool AllowErrors
        {
            get { return m_bAllowErrors; }
            set { m_bAllowErrors = value; }
        }

        public S7ConnectionParameters GetConnectionParameters()
        {
            return m_s7ConnPars;
        }

        #endregion

        #region IPluginTaskDataIbaAnalyzer Members

        public void SetIbaAnalyzer(IbaAnalyzer.IbaAnalyzer Analyzer, iba.Processing.IIbaAnalyzerMonitor Monitor)
        {
            if(m_worker == null) m_worker = new S7TaskWorker(this);
            m_worker.SetIbaAnalyzer(Analyzer, Monitor);
        }

        #endregion

        #region IPluginTaskDataIbaAnalyzer Members

        public bool UsesAnalysis
        {
            get { return !string.IsNullOrEmpty(m_pdoFile); }
        }

        private iba.Data.MonitorData m_monitorData;
        public iba.Data.MonitorData MonitorData
        {
            get { return m_monitorData; }
        }

        #endregion
    }

}
