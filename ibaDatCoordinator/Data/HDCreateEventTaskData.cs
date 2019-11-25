using iba.HD.Common;
using iba.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class HDCreateEventTaskData : TaskData
    {
        public const string CurrentFileExpression = "*CURRENT_FILE_NAME*";
        public const string UnassignedExpression = "*UNASSIGNED*";

        [Serializable]
        public class EventData : ICloneable
        {
            private string m_storeName;
            public string StoreName
            {
                get { return m_storeName; }
                set { m_storeName = value; }
            }

            private string m_id;
            public string ID
            {
                get { return m_id; }
                set { m_id = value; }
            }

            private string m_name;
            public string Name
            {
                get { return m_name; }
                set { m_name = value; }
            }

            private List<Tuple<string, string>> m_numericFields;
            [XmlIgnore]
            public List<Tuple<string, string>> NumericFields
            {
                get { return m_numericFields; }
                set { m_numericFields = value; }
            }

            public string[][] SerializableNumericFields
            {
                get { return m_numericFields.ToPairArray(); }
                set { m_numericFields.SetFromPairArray(value); }
            }

            private List<Tuple<string, string>> m_textFields;
            [XmlIgnore]
            public List<Tuple<string, string>> TextFields
            {
                get { return m_textFields; }
                set { m_textFields = value; }
            }

            public string[][] SerializableTextFields
            {
                get { return m_textFields.ToPairArray(); }
                set { m_textFields.SetFromPairArray(value); }
            }

            private List<string> m_blobFields;
            public List<string> BlobFields
            {
                get { return m_blobFields; }
                set { m_blobFields = value; }
            }

            private HDEventTriggerEnum m_triggerMode;
            public HDEventTriggerEnum TriggerMode
            {
                get { return m_triggerMode; }
                set { m_triggerMode = value; }
            }

            private string m_pulseSignal;
            public string PulseSignal
            {
                get { return m_pulseSignal; }
                set { m_pulseSignal = value; }
            }

            public EventData()
            {
                m_storeName = "";
                m_id = "";
                m_name = "";
                m_numericFields = new List<Tuple<string, string>>();
                m_textFields = new List<Tuple<string, string>>();
                m_blobFields = new List<string>();
                m_triggerMode = HDEventTriggerEnum.PerFile;
                m_pulseSignal = UnassignedExpression;
            }

            public object Clone()
            {
                EventData cpy = new EventData();
                cpy.m_storeName = m_storeName;
                cpy.m_id = m_id;
                cpy.m_name = m_name;
                cpy.m_numericFields = new List<Tuple<string, string>>(m_numericFields);
                cpy.m_textFields = new List<Tuple<string, string>>(m_textFields);
                cpy.m_blobFields = new List<string>(m_blobFields);
                cpy.m_triggerMode = m_triggerMode;
                cpy.m_pulseSignal = m_pulseSignal;
                return cpy;
            }

            public bool IsSame(EventData other)
            {
                if (other == null)
                    return false;
                if (other == this)
                    return true;

                return m_storeName == other.m_storeName
                    && m_id == other.m_id
                    && m_name == other.m_name
                    && m_triggerMode == other.m_triggerMode
                    && m_pulseSignal == other.m_pulseSignal
                    && m_numericFields.SequenceEqual(other.m_numericFields)
                    && m_textFields.SequenceEqual(other.m_textFields)
                    && m_blobFields.SequenceEqual(other.m_blobFields);
            }
        }

        private string m_server;
        public string Server
        {
            get { return m_server; }
            set { m_server = value; }
        }

        private int m_serverPort;
        public int ServerPort
        {
            get { return m_serverPort; }
            set { m_serverPort = value; }
        }

        private string m_datFileHost;
        public string DatFileHost
        {
            get { return m_datFileHost; }
            set { m_datFileHost = value; }
        }

        private string m_datFile;
        public string DatFile
        {
            get { return m_datFile; }
            set { m_datFile = value; }
        }

        private string m_datFilePassword;
        [XmlIgnore]
        public string DatFilePassword
        {
            get { return m_datFilePassword; }
            set { m_datFilePassword = value; }
        }

        public string EncryptedPassword
        {
            get { return Crypt.Encrypt(m_datFilePassword); }
            set { m_datFilePassword = Crypt.Decrypt(value); }
        }

        public enum HDEventTriggerEnum
        {
            PerFile = 0,
            PerSignalPulse
        }

        private List<EventData> m_eventSettings;
        public List<EventData> EventSettings
        {
            get { return m_eventSettings; }
            set { m_eventSettings = value; }
        }

        private string m_username;
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        private string m_password;
        [XmlIgnore]
        public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        public string EncryptedEventPassword
        {
            get { return Crypt.Decrypt(m_password); }
            set { m_password = Crypt.Encrypt(value); }
        }

        private Dictionary<string, string> m_fullEventConfig;
        [XmlIgnore]
        public Dictionary<string, string> FullEventConfig
        {
            get { return m_fullEventConfig; }
            set { m_fullEventConfig = value; }
        }

        private MonitorData m_monitorData;
        public MonitorData MonitorData
        {
            get { return m_monitorData; }
            set { m_monitorData = value; }
        }

        public HDCreateEventTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.HDEventTaskTitle;
            m_datFileHost = "";
            m_datFile = "";
            m_datFilePassword = "";
            m_server = "";
            m_serverPort = -1;
            m_username = "";
            m_password = "";
            m_eventSettings = new List<EventData>();
            m_monitorData = new MonitorData();
        }

        public HDCreateEventTaskData()
            :this(null)
        { }

        #region TaskData
        public override TaskData CloneInternal()
        {
            HDCreateEventTaskData ced = new HDCreateEventTaskData(null);
            ced.m_pdoFile = m_pdoFile;
            ced.m_datFileHost = m_datFileHost;
            ced.m_datFile = m_datFile;
            ced.m_datFilePassword = m_datFilePassword;
            ced.m_server = m_server;
            ced.m_serverPort = m_serverPort;
            ced.m_username = m_username;
            ced.m_password = m_password;
            ced.m_eventSettings = new List<EventData>(m_eventSettings);
            ced.m_monitorData = (MonitorData)m_monitorData.Clone();
            return ced;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            HDCreateEventTaskData other = taskData as HDCreateEventTaskData;
            if (other == null) return false;
            if (other == this) return true;

            return m_server == other.m_server
                && m_serverPort == other.m_serverPort
                && m_username == other.m_username
                && m_password == other.m_password
                && other.m_pdoFile == m_pdoFile
                && other.m_datFileHost == m_datFileHost
                && other.m_datFile == m_datFile
                && other.m_datFilePassword == m_datFilePassword
                && Enumerable.SequenceEqual(other.m_eventSettings, m_eventSettings)
                && other.m_monitorData.IsSame(m_monitorData);
        }
        #endregion
    }
}
