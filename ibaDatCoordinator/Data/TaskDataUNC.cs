using System;
using System.Collections.Generic;
using System.Text;
using iba.Utility;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public abstract class TaskDataUNC : TaskData
    {
        public enum OutputLimitChoiceEnum {LimitDirectories,LimitDiskspace}
        private OutputLimitChoiceEnum m_outputLimitChoice;
        public OutputLimitChoiceEnum OutputLimitChoice
        {
            get { return m_outputLimitChoice; }
            set { m_outputLimitChoice = value; }
        }

        private bool m_overwriteFiles;
        public bool OverwriteFiles
        {
            get { return m_overwriteFiles; }
            set { m_overwriteFiles = value; }
        }

        public TaskDataUNC(ConfigurationData parent) : base(parent) {
            m_pass = "";
            m_username = "";
            m_destinationMapUNC = "";
            m_numbFolders = 10;
            m_quota = 1024;
            m_outputLimitChoice = OutputLimitChoiceEnum.LimitDirectories;
            m_overwriteFiles = false;
            m_subfolderChoice = SubfolderChoice.DAY;
            m_doDirCleanup = false;
        }

        protected uint m_numbFolders;
        public uint SubfoldersNumber
        {
            get { return m_numbFolders; }
            set { m_numbFolders = value; }
        }

        //letter B appended because of name collision in XML serialisation
        public enum SubfolderChoice { SAME, NONE, HOUR, DAY, WEEK, MONTH };
        protected SubfolderChoice m_subfolderChoice;
        public SubfolderChoice Subfolder
        {
            get { return m_subfolderChoice; }
            set { m_subfolderChoice = value; }
        }

        protected uint m_quota;
        public uint Quota
        {
            get { return m_quota; }
            set { m_quota = value; }
        }

        public TaskDataUNC() : this(null) { }

        protected string m_destinationMap;
        public string DestinationMap
        {
            get { return m_destinationMap; }
            set { m_destinationMap = value; }
        }

        protected string m_destinationMapUNC;
        public string DestinationMapUNC
        {
            get { return m_destinationMapUNC; }
            set { m_destinationMapUNC = value; }
        }

        public void UpdateUNC()
        {
            m_destinationMapUNC = Shares.PathToUnc(m_destinationMap, false);
        }

        public void CopyUNCData(TaskDataUNC uncdat)
        {
            uncdat.m_destinationMap = m_destinationMap;
            uncdat.m_numbFolders = m_numbFolders;
            uncdat.m_username = m_username;
            uncdat.m_pass = m_pass;
            uncdat.m_destinationMapUNC = m_destinationMapUNC;
            uncdat.m_quota = m_quota;
            uncdat.m_outputLimitChoice = m_outputLimitChoice;
            uncdat.m_overwriteFiles = m_overwriteFiles;
            uncdat.m_subfolderChoice = m_subfolderChoice;
        }

        protected string m_username;
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        protected string m_pass;

        [XmlIgnore]
        public string Password
        {
            get { return m_pass; }
            set { m_pass = value; }
        }

        public string PasswordCrypted
        {
            get { return Crypt.Encrypt(m_pass); }
            set { m_pass = Crypt.Decrypt(value); }
        }

        [XmlIgnore]
        public bool DoDirCleanupNow
        {
            get { return m_doDirCleanup; }
            set { m_doDirCleanup = value; }
        }
        private bool m_doDirCleanup;
    }
}
