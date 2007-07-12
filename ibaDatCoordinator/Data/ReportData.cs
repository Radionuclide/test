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
        public string Extension
        {
            get { return m_extension; }
            set { if (m_allowedExtensions.Contains(value)) m_extension = value;}
        }


        public enum SubfolderChoice { SAME, NONE, HOUR, DAY, WEEK, MONTH };
        private SubfolderChoice m_subfolderChoice;
        public SubfolderChoice Subfolder
        {
            get { return m_subfolderChoice; }
            set { m_subfolderChoice = value; }
        }

        private uint m_numbFolders;
        public uint SubfoldersNumber
        {
            get { return m_numbFolders; }
            set { m_numbFolders = value; }
        }

        public ReportData(ConfigurationData parent) : base(parent)
        {
            m_name = iba.Properties.Resources.reportTitle;
            m_outputChoice = OutputChoice.FILE;
            m_subfolderChoice = SubfolderChoice.DAY;
            m_numbFolders = 10;
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

        public override object Clone()
        {
            ReportData rd = new ReportData(null);
            rd.m_wtodo = m_wtodo;
            rd.m_name = m_name;
            rd.m_pdoFile = m_pdoFile;
            rd.m_numbFolders = m_numbFolders;
            rd.m_subfolderChoice = m_subfolderChoice;
            rd.m_outputChoice = m_outputChoice;
            rd.m_destinationMap = m_destinationMap;
            rd.m_notify = m_notify;
            rd.m_username = m_username;
            rd.m_pass = m_pass;
            rd.m_destinationMapUNC = m_destinationMapUNC;
            rd.m_extension = m_extension;
            rd.m_monitorData = (MonitorData) m_monitorData.Clone();
            return rd;
        }
    }
}
