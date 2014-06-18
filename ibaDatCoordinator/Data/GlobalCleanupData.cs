using System.Runtime.Serialization;

namespace iba.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    [Serializable]
    public class GlobalCleanupData : ICloneable
    {
        public string DriveName { get; set; }
        public string WorkingFolder { get; set; }
        public int RescanTime { get; set; }
        public int PercentageFree { get; set; }
        public bool Active { get; set; }
        
        [NonSerialized]
        public bool IsSystem { get; set; }

        protected GlobalCleanupData(SerializationInfo info, StreamingContext context)
        {
            DriveName = info.GetString("DriveName");
            WorkingFolder = info.GetString("WorkingFolder");
            RescanTime = info.GetInt32("RescanTime");
            PercentageFree = info.GetInt32("PercentageFree");
            Active = info.GetBoolean("Active");
        }

        public GlobalCleanupData()
        {
            DriveName = String.Empty;
            WorkingFolder = String.Empty;
            RescanTime = 1;
            PercentageFree = 10;
            Active = false;
        }

        public object Clone()
        {
            var gc = new GlobalCleanupData();
            gc.DriveName = DriveName;
            gc.WorkingFolder = WorkingFolder;
            gc.RescanTime = RescanTime;
            gc.PercentageFree = PercentageFree;
            gc.Active = Active;
            return gc;
        }

        public override bool Equals(object obj)
        {
            var temp = obj as GlobalCleanupData;
            if (temp == null) return false;
            return temp.DriveName == DriveName
                && temp.WorkingFolder == WorkingFolder
                && temp.RescanTime == RescanTime
                && temp.PercentageFree == PercentageFree
                && temp.Active == Active;
        }

        public override int GetHashCode()
        {
            return DriveName.GetHashCode()
                ^ WorkingFolder.GetHashCode()
                ^ RescanTime.GetHashCode()
                ^ PercentageFree.GetHashCode()
                ^ Active.GetHashCode();
        }
    }
}
