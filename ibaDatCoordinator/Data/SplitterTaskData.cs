using iba.Utility;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class SplitterTaskData : TaskDataUNC
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

        private MonitorData m_monitorData;
        public MonitorData MonitorData
        {
            get { return m_monitorData; }
            set { m_monitorData = value; }
        }

        public SplitterTaskData(ConfigurationData parent) : base(parent)
        {
            m_name = iba.Properties.Resources.splitterTaskTitle;
            m_monitorData = new MonitorData();
            m_edgeConditionType = EdgeConditionTypeEnum.RISINGTORISING;
            m_datFilePassword = m_testDatFile = m_expression = String.Empty;
        }

        public SplitterTaskData() : this(null)
        {
        }

        public enum EdgeConditionTypeEnum { RISINGTORISING, RISINGTOFALLING }
        private EdgeConditionTypeEnum m_edgeConditionType;
        public EdgeConditionTypeEnum EdgeConditionType
        {
            get { return m_edgeConditionType; }
            set { m_edgeConditionType = value; }
        }

        public override TaskData CloneInternal()
        {
            SplitterTaskData std = new SplitterTaskData(null);
            CopyUNCData(std);
            std.m_pdoFile = m_pdoFile;
            std.m_testDatFile = m_testDatFile;
            std.m_expression = m_expression;
            std.m_monitorData = (MonitorData) m_monitorData.Clone();
            std.m_edgeConditionType = m_edgeConditionType;
            std.m_datFilePassword = m_datFilePassword;
            return std;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            SplitterTaskData other = taskData as SplitterTaskData;
            if (other == null) return false;
            if (other == this) return true;
            return
                other.m_pdoFile == m_pdoFile &&
                other.m_testDatFile == m_testDatFile &&
                other.m_expression == m_expression &&
                other.m_edgeConditionType == m_edgeConditionType &&
                other.m_datFilePassword == m_datFilePassword &&
                other.m_monitorData.IsSame(m_monitorData);
        }
    }
}
