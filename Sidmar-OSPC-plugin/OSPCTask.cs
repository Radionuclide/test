using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iba.Plugins;
using System.Xml.Serialization;

namespace AM_OSPC_plugin
{
    [Serializable]
    public class OSPCTask : IPluginTaskData, IPluginTaskDataIsSame
    {

        #region IPluginTaskData Members

        [NonSerialized]
        private OSPCTaskControl m_control;
        public IPluginControl GetControl()
        {
            if(m_control == null) m_control = new OSPCTaskControl(m_datcoHost);
            return m_control;
        }

        [NonSerialized]
        private OSPCTaskWorker m_worker;

        public IPluginTaskWorker GetWorker()
        {
            if(m_worker == null) m_worker = new OSPCTaskWorker(this);
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

        public OSPCTask()
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
            OSPCTask res = new OSPCTask(m_nameInfo, m_datcoHost, m_parentJob);
            res.m_testDatFile = m_testDatFile;
            res.m_pdoFile = m_pdoFile;
            m_records = Enumerable.Range(0, 20).Select(_ => new Record()).ToArray();
            res.m_ospcServerHost = m_ospcServerHost;
            res.m_ospcServerUser = m_ospcServerUser;
            res.m_ospcServerPass = m_ospcServerPass;
            return res;
        }

        #endregion

        public OSPCTask(string name, IDatCoHost host, IJobData parentJob)
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

            m_records = Enumerable.Range(0, 20).Select(_ => new Record()).ToArray();

            m_ospcServerHost = "";
            m_ospcServerUser = "";
            m_ospcServerPass = "";
        }

        public bool IsSame(IPluginTaskDataIsSame data)
        {
            var other = data as OSPCTask;
            if(other == null) return false;
            if(m_testDatFile != other.m_testDatFile) return false;
            if(m_pdoFile != other.m_pdoFile) return false;
            if(!m_records.SequenceEqual(other.m_records)) return false;
            if(m_ospcServerHost != other.m_ospcServerHost) return false;
            if(m_ospcServerUser != other.m_ospcServerUser) return false;
            if(m_ospcServerPass != other.m_ospcServerPass) return false;
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

        public class Record
        {
            private string m_expression;
            public string Expression
            {
                get { return m_expression; }
                set { m_expression = value; }
            }
            private string m_processName;
            public string ProcessName
            {
                get { return m_processName; }
                set { m_processName = value; }
            }
            private string m_variableName;
            public string VariableName
            {
                get { return m_variableName; }
                set { m_variableName = value; }
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
        }

        // Custom comparer for the Record class
        class RecordComparer : IEqualityComparer<Record>
        {
            // Records are equal if their names and Record numbers are equal.
            public bool Equals(Record x, Record y)
            {

                //Check whether the compared objects reference the same data.
                if(Object.ReferenceEquals(x, y)) return true;

                //Check whether any of the compared objects is null.
                if(Object.ReferenceEquals(x, null) || Object.ReferenceEquals(y, null))
                    return false;

                //Check whether the Records' properties are equal.
                return x.Expression == y.Expression && x.ProcessName == y.ProcessName && x.VariableName == y.VariableName;
            }

            // If Equals() returns true for a pair of objects 
            // then GetHashCode() must return the same value for these objects.

            public int GetHashCode(Record record)
            {
                //Check whether the object is null
                if(Object.ReferenceEquals(record, null)) return 0;
                return record.Expression.GetHashCode() ^ record.Expression.GetHashCode();
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
            get { return Crypt.Encrypt(m_ospcServerPass); }
            set { m_ospcServerPass = Crypt.Decrypt(value); }
        }

        private string m_ospcServerHost;
        public string OspcServerHost
        {
            get { return m_ospcServerHost; }
            set { m_ospcServerHost = value; }
        }

        #endregion
    }

    public class Crypt
    {
        static byte[] key = new byte[] { 12, 34, 179, 69, 231, 92 };

        public static string Encrypt(string msg)
        {
            if(msg == "")
                return msg;
            try
            {
                System.Text.StringBuilder sb = new System.Text.StringBuilder("0x");
                System.Text.Encoding enc = new System.Text.UTF8Encoding();
                byte[] b = enc.GetBytes(msg);
                for(int i = 0; i < b.Length; i++)
                {
                    b[i] = (byte)(b[i] ^ key[i % key.Length]);
                    sb.Append(b[i].ToString("X2"));
                }
                return sb.ToString();
            }
            catch(Exception)
            {
            }
            return msg;
        }

        public static string Decrypt(string msg)
        {
            if(!msg.StartsWith("0x"))
                return msg;
            try
            {
                msg = msg.Substring(2);
                byte[] b = new byte[msg.Length / 2];
                for(int i = 0; i < b.Length; i++)
                {
                    b[i] = Byte.Parse(msg.Substring(i * 2, 2), System.Globalization.NumberStyles.HexNumber);
                    b[i] = (byte)(b[i] ^ key[i % key.Length]);
                }
                System.Text.Encoding enc = new System.Text.UTF8Encoding();
                return enc.GetString(b);
            }
            catch(Exception)
            {
            }
            return msg;
        }
    }
}
