using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iba.Data
{
    [ Serializable ]
    public class BatchFileData : TaskData
    {
        private string m_batchFile;
        public string BatchFile
        {
            get { return m_batchFile; }
            set { m_batchFile = value; }
        }

        public BatchFileData(ConfigurationData parent) : base(parent)
        {
            m_name = iba.Properties.Resources.batchfileTitle;
            m_batchFile = String.Empty;
        }

        public BatchFileData() : this(null)
        {

        }

        public override object Clone()
        {
            BatchFileData bfd = new BatchFileData(null);
            bfd.m_name = m_name.Clone() as string;
            bfd.m_wtodo = m_wtodo;
            bfd.m_pdoFile = m_pdoFile.Clone() as string;
            bfd.m_batchFile = m_batchFile.Clone() as string;
            bfd.m_notify = m_notify;
            return bfd;
        }
    }
}
