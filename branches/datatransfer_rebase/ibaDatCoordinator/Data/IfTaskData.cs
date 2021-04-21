using iba.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class IfTaskData : TaskData
    {
        private string m_expression;
        public string Expression
        {
            get { return m_expression; }
            set { m_expression = value; }
        }

        private string m_testDatFile;
        public string TestDatFile
        {
            get { return m_testDatFile; }
            set { m_testDatFile = value; }
        }


        private string m_datFilePassword;
        [XmlIgnore]
        public string DatFilePassword
        {
            get { return m_datFilePassword; }
            set { m_datFilePassword = value; }
        }

        public string EncryptedDatFilePassword
        {
            get { return Crypt.Encrypt(m_datFilePassword); }
            set { m_datFilePassword = Crypt.Decrypt(value); }
        }

        public enum XTypeEnum 
        {
            XTimeBased=0,
            XLengthBased=1,
            XFrequencyBased=2,
            XInvLengthBased=3
        };

        private XTypeEnum m_xtype;
        public XTypeEnum XType
        {
            get { return m_xtype; }
            set { m_xtype = value; }
        }

        public IfTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.iftaskTitle;
            m_testDatFile = String.Empty;
            m_expression = String.Empty;
            m_xtype = 0;
            m_monitorData = new MonitorData();
            m_datFilePassword = String.Empty;
        }

        public IfTaskData()
            : this(null)
        {

        }

        private MonitorData m_monitorData;
        public MonitorData MonitorData
        {
            get { return m_monitorData; }
            set { m_monitorData = value; }
        }

        public override TaskData CloneInternal()
        {
            IfTaskData ifd = new IfTaskData(null);
            ifd.m_xtype = m_xtype;
            ifd.m_pdoFile = m_pdoFile;
            ifd.m_testDatFile = m_testDatFile;
            ifd.m_expression = m_expression;
            ifd.m_monitorData = (MonitorData) m_monitorData.Clone();
            ifd.m_datFilePassword = m_datFilePassword;
            return ifd;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            IfTaskData other = taskData as IfTaskData;
            if (other == null) return false;
            if (other == this) return true;
            return
            other.m_xtype == m_xtype &&
            other.m_pdoFile == m_pdoFile &&
            other.m_testDatFile == m_testDatFile &&
            other.m_datFilePassword == m_datFilePassword &&
            other.m_expression == m_expression &&
            other.m_monitorData.IsSame(m_monitorData);
        }
    }
}
