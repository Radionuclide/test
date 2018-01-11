using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using iba.Utility;

namespace iba.Data
{

    [Serializable]
    public class GlobalCleanupData : ICloneable
    {

        [XmlAttribute]
        public string DriveName { get; set; }
        [XmlAttribute]
        public string VolumeLabel { get; set; }
        [XmlAttribute]
        public string WorkingFolder { get; set; }
        [XmlAttribute]
        public int RescanTime { get; set; }
        [XmlAttribute]
        public int PercentageFree { get; set; }
        [XmlAttribute]
        public bool Active { get; set; }

        // addl. attributes needed only for processing. Will be skipped in config file
        [XmlIgnore]
        public long TotalSize { get; set; }
        [XmlIgnore]
        public long TotalFreeSpace { get; set; }
        [XmlIgnore]
        public bool IsReady { get; set; }
        [XmlIgnore]
        public bool IsSystemDrive { get; set; }


        protected GlobalCleanupData(SerializationInfo info, StreamingContext context)
        {
            DriveName = info.GetString(nameof(DriveName));
            VolumeLabel = info.GetString(nameof(VolumeLabel));
            WorkingFolder = info.GetString(nameof(WorkingFolder));
            RescanTime = info.GetInt32(nameof(RescanTime));
            PercentageFree = info.GetInt32(nameof(PercentageFree));
            Active = info.GetBoolean(nameof(Active));

            TotalSize = info.GetInt64(nameof(TotalSize));
            TotalFreeSpace = info.GetInt64(nameof(TotalFreeSpace));
            IsReady = info.GetBoolean(nameof(IsReady));
            IsSystemDrive = info.GetBoolean(nameof(IsSystemDrive));
        }

        public GlobalCleanupData()
        {
            DriveName = String.Empty;
            VolumeLabel = String.Empty;
            WorkingFolder = String.Empty;
            RescanTime = 1;
            PercentageFree = 10;
            Active = false;
        }

        public object Clone()
        {
            var gc = new GlobalCleanupData();
            gc.DriveName = DriveName;
            gc.VolumeLabel = VolumeLabel;
            gc.WorkingFolder = WorkingFolder;
            gc.RescanTime = RescanTime;
            gc.PercentageFree = PercentageFree;
            gc.Active = Active;

            gc.TotalSize = TotalSize;
            gc.TotalFreeSpace = TotalFreeSpace;
            gc.IsReady = IsReady;
            gc.IsSystemDrive = IsSystemDrive;

            return gc;
        }


        public override bool Equals(object obj)
        {
            var temp = obj as GlobalCleanupData;
            if (temp == null) return false;
            return temp.DriveName == DriveName
                && temp.VolumeLabel == VolumeLabel
                && temp.WorkingFolder == WorkingFolder
                && temp.RescanTime == RescanTime
                && temp.PercentageFree == PercentageFree
                && temp.Active == Active
                && temp.TotalSize == TotalSize;
        }

        public override int GetHashCode()
        {
            return DriveName.GetHashCode()
                ^ VolumeLabel.GetHashCode()
                ^ WorkingFolder.GetHashCode()
                ^ RescanTime.GetHashCode()
                ^ PercentageFree.GetHashCode()
                ^ Active.GetHashCode()
                ^ TotalSize.GetHashCode();
        }
    }
}
