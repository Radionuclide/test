using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iba.Plugins;
using System.Xml.Serialization;

namespace AM_OSPC_plugin
{
    [Serializable]
    public class OSPCTaskData : IPluginTaskData, IPluginTaskDataIsSame, IPluginTaskDataIbaAnalyzer, IGridAnalyzer
	{

        #region IPluginTaskData Members

        [NonSerialized]
        private OSPCTaskControl m_control;
        public IPluginControl GetControl()
        {
            if(m_control == null) 
                m_control = new OSPCTaskControl();
            return m_control;
        }

        [NonSerialized]
        private OSPCTaskWorker m_worker;

        public IPluginTaskWorker GetWorker()
        {
            if(m_worker == null) 
                m_worker = new OSPCTaskWorker(this);
            return m_worker;
        }

        public void SetWorker(IPluginTaskWorker worker)
        {
            m_worker = worker as OSPCTaskWorker;
        }

        public string NameInfo
        {
            get { return m_nameInfo; }
            set { m_nameInfo = value; }
        }

        public OSPCTaskData()
        {
            InitData(null, null);
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
        }

        public void SetParentJob(IJobData data)
        {
            m_parentJob = data;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            OSPCTaskData res = new OSPCTaskData(m_nameInfo, m_parentJob);
            res.m_testDatFile = m_testDatFile;
            res.m_pdoFile = m_pdoFile;
            for(int i = 0; i < m_records.Length; i++)
                res.m_records[i] = (Record)m_records[i].Clone();
            res.m_ospcServerHost = m_ospcServerHost;
            res.m_ospcServerUser = m_ospcServerUser;
            res.m_ospcServerPass = m_ospcServerPass;
            res.m_monitorData = (iba.Data.MonitorData) m_monitorData.Clone();
            return res;
        }

        #endregion

        public OSPCTaskData(string name, IJobData parentJob)
        {
            //try
            //{
            //    IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            //    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //    m_ownAddress = nics[0].GetPhysicalAddress().GetAddressBytes();
            //}
            //catch (Exception)
            //{
            //    m_ownAddress = new Byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            //}

            InitData(name, parentJob);
        }

        internal IJobData m_parentJob;

        private string m_nameInfo;

        private void InitData(string name, IJobData parentJob)
        {
            m_parentJob = parentJob;
            m_nameInfo = name;

            m_testDatFile = "";
            m_pdoFile = "";

            m_records = Enumerable.Range(0, 20).Select(_ => new Record()).ToArray();

            m_ospcServerHost = "";
            m_ospcServerUser = "";
            m_ospcServerPass = "";
            m_monitorData = new iba.Data.MonitorData();
        }

        public bool IsSame(IPluginTaskDataIsSame data)
        {
            var other = data as OSPCTaskData;
            if(other == null) return false;
            if(m_testDatFile != other.m_testDatFile) return false;
            if(m_pdoFile != other.m_pdoFile) return false;
            if(!m_records.SequenceEqual(other.m_records)) return false;
            if(m_ospcServerHost != other.m_ospcServerHost) return false;
            if(m_ospcServerUser != other.m_ospcServerUser) return false;
            if(m_ospcServerPass != other.m_ospcServerPass) return false;
            if(!m_monitorData.IsSame(other.m_monitorData)) return false;
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
        public class Record : ICloneable
        {
            private string m_expression;
            public string Expression
            {
                get { return m_expression; }
                set
                {
                    m_expression = value ?? "";
                }
            }
            private string m_processName;
            public string ProcessName
            {
                get { return m_processName; }
                set { m_processName = value ?? ""; }
            }
            private string m_variableName;
            public string VariableName
            {
                get { return m_variableName; }
                set { m_variableName = value ?? ""; }
            }
            private double m_testValue;
            public double TestValue
            {
                get { return m_testValue; }
                set { m_testValue = value; }
            }

            [XmlIgnore]
            public string TestValueString
            {
                get { return (double.IsNaN(m_testValue) || double.IsInfinity(m_testValue))?"":m_testValue.ToString(); }
            }

            public Record() //no nulls
            {
                m_processName = "";
                m_variableName= "";
                m_expression = "";
                m_testValue = Double.NaN;
            }

            #region ICloneable Members

            public object Clone()
            {
                Record res = new Record();
                res.m_processName = m_processName;
                res.m_variableName = m_variableName;
                res.m_processName = m_processName;
                res.m_expression = m_expression;
                res.m_testValue = m_testValue;
                return res;
            }

            #endregion

            public override bool Equals(object obj)
            {
                if (obj == null)
                    return false;

                return Equals(obj as Record);
            }

            public bool Equals(Record other)
            {
                Record x = this;
                Record y = other;
                //Check whether the compared objects reference the same data.
                if (Object.ReferenceEquals(x, y)) return true;

                //Check whether any of the compared objects is null.
                if (Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                //Check whether the Records' properties are equal.
                return x.Expression == y.Expression && x.ProcessName == y.ProcessName && x.VariableName == y.VariableName;
            }

            public override int GetHashCode()
            {
                //Check whether the object is null
                return Expression.GetHashCode() ^ Expression.GetHashCode() ^ ProcessName.GetHashCode() ^ VariableName.GetHashCode();
            }
        }

        private Record[] m_records;

	    public Record[] Records
	    {
		    get { return m_records; }
		    set { m_records = value; }
	    }

        private string m_ospcServerUser;
        public string OspcServerUser
        {
            get { return m_ospcServerUser; }
            set { m_ospcServerUser = value; }
        }
        private string m_ospcServerPass;
        
        [XmlIgnore]
        public string OspcServerPassword
        {
            get { return m_ospcServerPass; }
            set { m_ospcServerPass = value; }
        }

        public string OspcServerPasswordCrypted
        {
            get { return PluginCollection.Host.GetService<IEncryptionService>().Encrypt(m_ospcServerPass); }
            set { m_ospcServerPass = PluginCollection.Host.GetService<IEncryptionService>().Decrypt(value); }
        }

        private string m_ospcServerHost;
        public string OspcServerHost
        {
            get { return m_ospcServerHost; }
            set { m_ospcServerHost = value; }
        }

		#endregion
		public void SetGridAnalyzer(object e, IAnalyzerManagerUpdateSource analyzer)
		{
			if (m_control == null)
				m_control = new OSPCTaskControl();
			m_control.SetGridAnalyzer(e as DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit, analyzer);
		}

		#region IPluginTaskDataIbaAnalyzer Members

		public void SetIbaAnalyzer(IbaAnalyzer.IbaAnalyzer Analyzer, iba.Processing.IIbaAnalyzerMonitor Monitor)
        {
            if(m_worker == null) m_worker = new OSPCTaskWorker(this);
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

		public bool DatTriggered
		{
			get
			{
				return m_parentJob.DatTriggered;
			}
		}

		#endregion
	}

}
