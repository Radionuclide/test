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

        bool m_delete;
        public bool ActionDelete
        {
            get { return m_delete; }
            set { m_delete = value; }
        }

        public CopyMoveTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.copyTitle;
            m_removeSource = false;
            m_delete = false;
            m_subfolderChoice = SubfolderChoice.NONE;
            m_whatFile = WhatFileEnumA.DATFILE;
        }

        public enum WhatFileEnumA { DATFILE, PREVOUTPUT }
        private WhatFileEnumA m_whatFile;

        public WhatFileEnumA WhatFile
        {
            get { return m_whatFile; }
            set { m_whatFile = value; }
        }

        public bool CreateZipArchive { get; set; }

        public CopyMoveTaskData()
            : this(null)
        {
        }

        public override TaskData CloneInternal()
        {
            CopyMoveTaskData cd = new CopyMoveTaskData(null);
            cd.m_removeSource = m_removeSource;
            cd.m_delete = m_delete;
            CopyUNCData(cd);
            cd.m_whatFile = m_whatFile;
            cd.CreateZipArchive = CreateZipArchive;
            return cd;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            CopyMoveTaskData other = taskData as CopyMoveTaskData;
            if (other == null) return false;
            if (other == this) return true;
            if (!UNCDataIsSame(other)) return false;
            return
                other.m_removeSource == m_removeSource &&
                other.m_delete == m_delete &&
                other.m_whatFile == m_whatFile &&
                other.CreateZipArchive == CreateZipArchive;
        }
    }
}
