using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;

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

        private bool m_testOnClientSide;
        public bool TestOnClientSide
        {
            get { return m_testOnClientSide; }
            set { m_testOnClientSide = value; }
        }     

        private string m_arguments;
        public string Arguments
        {
            get { return m_arguments; }
            set { m_arguments = value; }
        }

        public enum WhatFileEnum {DATFILE,PREVOUTPUT}
        private WhatFileEnum m_whatFile;

        public WhatFileEnum WhatFile
        {
            get { return m_whatFile; }
            set { m_whatFile = value; }
        }

        public BatchFileData(ConfigurationData parent) : base(parent)
        {
            m_name = iba.Properties.Resources.batchfileTitle;
            m_testDatFile = m_arguments = m_batchFile = String.Empty;
            m_whatFile = WhatFileEnum.DATFILE;
            m_testOnClientSide = false;
        }

        public BatchFileData() : this(null)
        {

        }

        public override TaskData CloneInternal()
        {
            BatchFileData bfd = new BatchFileData(null);
            bfd.m_pdoFile = m_pdoFile;
            bfd.m_batchFile = m_batchFile;
            bfd.m_testDatFile = m_testDatFile;
            bfd.m_arguments = m_arguments;
            bfd.m_whatFile = m_whatFile;
            bfd.m_testOnClientSide = m_testOnClientSide;
            return bfd;
        }

        public string ParsedArguments(string filename)
        {
            try
            {
                string args = m_arguments.Trim();
                args = args.Replace("%F", "%f");
                args = args.Replace("\"%f\"", "%f");
                args = args.Replace("%f", "\"{0}\"");
                args = args.Replace("%G", "%g");
                args = args.Replace("%g", "{1}");
                args = args.Replace("%H", "%h");
                args = args.Replace("%h", "{2}");
                args = string.Format(args, filename, Path.GetFileName(filename), Path.GetFileNameWithoutExtension(filename));
                return args;
            }
            catch
            {
                return null;
            }
        }

        public override void AdditionalFileNames(List<KeyValuePair<string, string>> myList, string safeConfName)
        {
            StringBuilder sb = new StringBuilder(safeConfName);
            sb.Append('\\');
            sb.Append(iba.Utility.PathUtil.FilterInvalidFileNameChars(m_name));
            sb.Append('\\');
            sb.Append(Path.GetFileName(BatchFile));
            myList.Add(new KeyValuePair<string, string>(BatchFile, sb.ToString()));
        }

        override public bool IsSameInternal(TaskData taskData)
        {
            BatchFileData other = taskData as BatchFileData;
            if (other == null) return false;
            if (other == this) return true;
            return other.m_pdoFile == m_pdoFile &&
            other.m_batchFile == m_batchFile &&
            other.m_testDatFile == m_testDatFile &&
            other.m_arguments == m_arguments &&
            other.m_whatFile == m_whatFile &&
            other.m_testOnClientSide == m_testOnClientSide;
        }
    }
}
