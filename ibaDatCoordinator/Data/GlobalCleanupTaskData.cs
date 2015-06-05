

namespace iba.Data
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    using iba.Data;

    [Serializable]
    public class GlobalCleanupTaskData : TaskDataUNC
    {
        public GlobalCleanupTaskData(ConfigurationData parent)
            : base(parent)
        {
            
        }
        public GlobalCleanupTaskData() 
            : this (null)
        {
            
        }

        public override TaskData CloneInternal()
        {
            GlobalCleanupTaskData cd = new GlobalCleanupTaskData(null);
            CopyUNCData(cd);
            return cd;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            GlobalCleanupTaskData other = taskData as GlobalCleanupTaskData;
            if (other == null) return false;
            if (other == this) return true;
            if (!UNCDataIsSame(other)) return false;
            return true;
        }
    }
}


