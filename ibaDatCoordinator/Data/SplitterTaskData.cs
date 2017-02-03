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
            m_triggerType = TriggerTypeEnum.RISINGTORISING;
            m_testDatFile = m_expression = String.Empty;
        }

        public SplitterTaskData() : this(null)
        {
        }

        public enum TriggerTypeEnum { RISINGTORISING, RISINGTOFALLING }
        private TriggerTypeEnum m_triggerType;
        public TriggerTypeEnum TriggerType
        {
            get { return m_triggerType; }
            set { m_triggerType = value; }
        }

        public override TaskData CloneInternal()
        {
            SplitterTaskData std = new SplitterTaskData(null);
            std.m_pdoFile = m_pdoFile;
            std.m_testDatFile = m_testDatFile;
            std.m_expression = m_expression;
            std.m_monitorData = (MonitorData) m_monitorData.Clone();
            std.m_triggerType = m_triggerType;
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
                other.m_triggerType == m_triggerType &&
                other.m_monitorData.IsSame(m_monitorData);
        }
    }
}
