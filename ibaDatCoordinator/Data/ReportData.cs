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

        public override object Clone()
        {
            ReportData rd = new ReportData(null);
            rd.m_wtodo = m_wtodo;
            rd.m_name = m_name;
            rd.m_pdoFile = m_pdoFile;
            rd.m_outputChoice = m_outputChoice;
            rd.m_notify = m_notify;

            CopyUNCData(rd);

            rd.m_extension = m_extension;
            rd.m_monitorData = (MonitorData) m_monitorData.Clone();
            return rd;
        }
    }
}
