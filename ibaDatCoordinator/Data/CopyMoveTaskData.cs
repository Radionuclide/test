using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Data
{
    [Serializable]
    public class CopyMoveTaskData : TaskDataUNC
    {
        bool m_removeSource;
        public bool RemoveSource
        {
            get { return m_removeSource; }
            set { m_removeSource = value; }
        }

        private uint m_numbFolders;
        public uint SubfoldersNumber
        {
            get { return m_numbFolders; }
            set { m_numbFolders = value; }
        }

        public CopyMoveTaskData(ConfigurationData parent) : base(parent)
        {
            m_name = iba.Properties.Resources.copyTitle;
            m_removeSource = false;
            m_subfolderChoice = SubfolderChoiceA.NONE;
            m_numbFolders = 10;
        }

        //letter A added because namecollision in XML-Serialiser with SubFolderCHoice from reportdata
        public enum SubfolderChoiceA { NONE, HOUR, DAY, WEEK, MONTH,SAME};
        private SubfolderChoiceA m_subfolderChoice;
        public SubfolderChoiceA Subfolder
        {
            get { return m_subfolderChoice; }
            set { m_subfolderChoice = value; }
        }

        public CopyMoveTaskData()
            : this(null)
        {
        }
        
        public override object Clone()
        {
            CopyMoveTaskData cd = new CopyMoveTaskData(null);
            cd.m_removeSource = m_removeSource;
            cd.m_subfolderChoice = m_subfolderChoice;
            cd.m_destinationMap = m_destinationMap;
            cd.m_numbFolders = m_numbFolders;
            cd.m_wtodo = m_wtodo;
            cd.m_name = m_name;
            cd.m_notify = m_notify;
            cd.m_username = m_username;
            cd.m_pass = m_pass;
            cd.m_destinationMapUNC = m_destinationMapUNC;
            return cd;
        }
    }
}
