using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class ExtractData : TaskData
    {
        public ExtractData(ConfigurationData parent) : base(parent)
        {
            m_name = iba.Properties.Resources.extractTitle;
            m_destinationMap = String.Empty;
            m_toFile = false;
            m_subfolderChoice = SubfolderChoiceB.DAY;
            m_numbFolders = 10;
            m_fileType = ExtractFileType.BINARY;
        }

        public ExtractData() : base(null)
        {
            m_destinationMap = String.Empty;
            m_toFile = false;
        }

        private bool m_toFile;
        public bool ExtractToFile
        {
            get { return m_toFile; }
            set { m_toFile = value; }
        }

        private string m_destinationMap;
        public string DestinationMap
        {
            get { return m_destinationMap; }
            set { m_destinationMap = value; }
        }



        //letter B appended because of name collision in XML serialisation
        public enum SubfolderChoiceB { SAME, NONE, HOUR, DAY, WEEK, MONTH };
        private SubfolderChoiceB m_subfolderChoice;
        public SubfolderChoiceB Subfolder
        {
            get { return m_subfolderChoice; }
            set { m_subfolderChoice = value; }
        }

        public enum ExtractFileType { TEXT, BINARY };
        private ExtractFileType m_fileType;
        public ExtractFileType FileType
        {
            get { return m_fileType; }
            set { m_fileType = value; }
        }

        private uint m_numbFolders;
        public uint SubfoldersNumber
        {
            get { return m_numbFolders; }
            set { m_numbFolders = value; }
        }

        public override object Clone()
        {
            ExtractData ed = new ExtractData(null);
            ed.m_name = m_name.Clone() as string;
            ed.m_wtodo= m_wtodo;
            ed.m_pdoFile = m_pdoFile.Clone() as string;
            ed.m_destinationMap = m_destinationMap.Clone() as string;
            ed.m_toFile = m_toFile;
            ed.m_notify = m_notify;
            ed.m_subfolderChoice = m_subfolderChoice;
            ed.m_numbFolders = m_numbFolders;
            ed.m_fileType = m_fileType;
            return ed;
        }

    }
}
