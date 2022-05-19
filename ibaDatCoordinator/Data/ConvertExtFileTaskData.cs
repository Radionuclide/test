using System;
using System.Xml.Serialization;
using DevExpress.XtraSplashScreen;
using iba.Utility;

namespace iba.Data
{
    [Serializable]
    public class ConvertExtFileTaskData : TaskDataUNC
    {
        public ConvertExtFileTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.ConvertExternalFileTitle;
            m_monitorData = new MonitorData();
        }
        public ConvertExtFileTaskData()
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
            ConvertExtFileTaskData cd = new ConvertExtFileTaskData(null);
            cd.m_monitorData = (MonitorData) m_monitorData.Clone();
            cd.AnalysisFile = AnalysisFile;
            CopyUNCData(cd);
            return cd;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            ConvertExtFileTaskData other = taskData as ConvertExtFileTaskData;
            if (other == null) return false;
            if (other == this) return true;
            if (!UNCDataIsSame(other)) return false;
            
            return 
                other.m_monitorData.IsSame(m_monitorData) &&
                other.AnalysisFile == AnalysisFile;

        }
    }
}
