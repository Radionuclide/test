using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
namespace iba.Data
{
    [ Serializable ]
    public class ReportData : TaskDataUNC
    {
        public enum OutputChoice { FILE, PRINT };
        private OutputChoice m_outputChoice;
        public OutputChoice Output
        {
            get { return m_outputChoice; }
            set { m_outputChoice = value; }
        }

        static private List<string> m_allowedExtensions = new List<string>(new string[] {                
                "pdf",
                "htm",
                "html",
                "mht",
                "mhtml",
                "txt",
                "xls",
                "rtf",
                "tif",
                "tiff",
                "emf",
                "jpg",
                "jpeg",
                "bmp",
                "xml"});

        private string m_extension;
        public override string Extension
        {
            get { return m_extension; }
            set { if (m_allowedExtensions.Contains(value)) m_extension = value;}
        }

        public ReportData(ConfigurationData parent) : base(parent)
        {
            m_name = iba.Properties.Resources.reportTitle;
            m_outputChoice = OutputChoice.FILE;
            m_extension = "pdf";
            m_monitorData = new MonitorData();
        }

        public ReportData() : this(null)
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
            ReportData rd = new ReportData(null);
            rd.m_pdoFile = m_pdoFile;
            rd.m_outputChoice = m_outputChoice;
            CopyUNCData(rd);
            rd.m_extension = m_extension;
            rd.m_monitorData = (MonitorData) m_monitorData.Clone();
            return rd;
        }

        public override void AdditionalFileNames(List<KeyValuePair<string, string>> myList, string safeConfName)
        {
            //the analysis
            base.AdditionalFileNames(myList, safeConfName);
            //perhaps the lst is in the same location ?
            try
            {
                string[] lstFiles = System.IO.Directory.GetFiles(System.IO.Path.GetDirectoryName(AnalysisFile),@"*.lst");
                if (lstFiles != null && lstFiles.Length == 1)
                {
                    StringBuilder sb = new StringBuilder(safeConfName);
                    sb.Append('\\');
                    sb.Append(Utility.PathUtil.FilterInvalidFileNameChars(lstFiles[0]));
                    sb.Append('\\');
                    sb.Append(System.IO.Path.GetFileName(lstFiles[0]));
                    myList.Add(new KeyValuePair<string, string>(lstFiles[0], sb.ToString()));
                }
            }
            catch 
            {
            }
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            ReportData other = taskData as ReportData;
            if (other == null) return false;
            if (other == this) return true;
            if (!UNCDataIsSame(other)) return false;
            return 
            other.m_pdoFile == m_pdoFile &&
            other.m_outputChoice == m_outputChoice &&
            other.m_extension == m_extension &&
            other.m_monitorData.IsSame(m_monitorData);
        }
    }
}
