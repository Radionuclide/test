using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using iba.Utility;

namespace iba.Data
{
    [Serializable]
    public class ExtractData : TaskDataUNC
    {
        public ExtractData(ConfigurationData parent) : base(parent)
        {
            m_name = iba.Properties.Resources.extractTitle;
            m_destinationMap = String.Empty;
            m_toFile = false;
            m_fileType = ExtractFileType.BINARY;
            m_monitorData = new MonitorData();
        }

        public ExtractData() : this(null)
        {
        }

        private bool m_toFile;
        public bool ExtractToFile
        {
            get { return m_toFile; }
            set { m_toFile = value; }
        }

        public enum ExtractFileType { TEXT, BINARY, COMTRADE, TDMS, PARQUET,MATLAB};
        private ExtractFileType m_fileType;
        public ExtractFileType FileType
        {
            get { return m_fileType; }
            set { m_fileType = value; }
        }

        private MonitorData m_monitorData;
        public MonitorData MonitorData
        {
            get { return m_monitorData; }
            set { m_monitorData = value; }
        }

        public bool m_bExternalVideoResultIsCached;
        public bool m_bExternalVideoCacheResult;

        public override TaskData CloneInternal()
        {
            ExtractData ed = new ExtractData(null);
            ed.m_pdoFile = m_pdoFile;
            ed.m_toFile = m_toFile;
            ed.m_fileType = m_fileType;
            CopyUNCData(ed);
            ed.m_monitorData = (MonitorData) m_monitorData.Clone();
            return ed;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            ExtractData other = taskData as ExtractData;
            if (other == null) return false;
            if (other == this) return true;
            if (!UNCDataIsSame(other)) return false;
            return
            other.m_pdoFile == m_pdoFile &&
            other.m_toFile == m_toFile &&
            other.m_fileType == m_fileType &&
            other.m_monitorData.IsSame(m_monitorData);
        }

        public override int RequiredLicense => ExtractToFile ? Licensing.LicenseId.FileExtract : Licensing.LicenseId.DBExtract;
    }
}
