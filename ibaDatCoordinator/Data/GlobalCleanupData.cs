using System.Runtime.Serialization;

namespace iba.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;

    [Serializable]
    public class GlobalCleanupData : ICloneable
    {
        [XmlAttribute]
        public string DriveName { get; set; }
        [XmlAttribute]
        public string WorkingFolder { get; set; }
        [XmlAttribute]
        public int RescanTime { get; set; }
        [XmlAttribute]
        public int PercentageFree { get; set; }
        [XmlAttribute]
        public bool Active { get; set; }

        [XmlIgnore]
        public bool IsSystemDrive { get; set; }
        [XmlIgnore]
        public long TotalSize { get; set; }
        [XmlIgnore]
        public string VolumeLabel { get; set; }

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

            IsSystemDrive = false;
            TotalSize = 0L;
            VolumeLabel = String.Empty;
        }

        public object Clone()
        {
            var gc = new GlobalCleanupData();
            gc.DriveName = DriveName;
            gc.WorkingFolder = WorkingFolder;
            gc.RescanTime = RescanTime;
            gc.PercentageFree = PercentageFree;
            gc.Active = Active;

            gc.IsSystemDrive = IsSystemDrive;
            gc.TotalSize = TotalSize;
            gc.VolumeLabel = VolumeLabel;

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
