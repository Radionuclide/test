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

        private string m_testDatFile;
        public string TestDatFile
        {
            get { return m_testDatFile; }
            set { m_testDatFile = value; }
        }

        private string m_arguments;
        public string Arguments
        {
            get { return m_arguments; }
            set { m_arguments = value; }
        }

        public BatchFileData(ConfigurationData parent) : base(parent)
        {
            m_name = iba.Properties.Resources.batchfileTitle;
            m_testDatFile = m_arguments = m_batchFile = String.Empty;
            
        }

        public BatchFileData() : this(null)
        {

        }

        public override object Clone()
        {
            BatchFileData bfd = new BatchFileData(null);
            bfd.m_name = m_name;
            bfd.m_wtodo = m_wtodo;
            bfd.m_pdoFile = m_pdoFile;
            bfd.m_batchFile = m_batchFile;
            bfd.m_testDatFile = m_testDatFile;
            bfd.m_arguments = m_arguments;
            bfd.m_notify = m_notify;
            return bfd;
        }

        public string ParsedArguments(string filename)
        {
            throw new Exception("The method or operation is not implemented.");
        }
    }
}
