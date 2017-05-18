using System;
using System.Collections.Generic;
using iba.Processing;
using IbaSnmpLib;

namespace iba.Data
{
    /// <summary>
    /// Is used for transferring the information about all SNMP-visible objects between
    /// the TaskManager and SnmpWorker.
    /// </summary>
    internal class SnmpObjectsData 
    {
        public bool IsStructureValid { get; set; }
        
        public void Reset()
        {
            GlobalCleanup.Clear();
            StandardJobs.Clear();
            ScheduledJobs.Clear();
            OneTimeJobs.Clear();
            EventBasedJobs.Clear();

            SnmpWorker.TmpLogLine("SnmpObjectsData.Reset()");
        }

        /// <summary> PrGeneral.3 </summary>
        public LicenseInfo License = new LicenseInfo();
        /// <summary> PrSpecific.1 </summary>
        public List<GlobalCleanupDriveInfo> GlobalCleanup = new List<GlobalCleanupDriveInfo>();
        /// <summary> PrSpecific.2 </summary>
        public List<StandardJobInfo> StandardJobs = new List<StandardJobInfo>();
        /// <summary> PrSpecific.3 </summary>
        public List<ScheduledJobInfo> ScheduledJobs = new List<ScheduledJobInfo>();
        /// <summary> PrSpecific.4 </summary>
        public List<OneTimeJobInfo> OneTimeJobs = new List<OneTimeJobInfo>();
        /// <summary> PrSpecific.5 </summary>
        public List<EventBasedJobInfo> EventBasedJobs = new List<EventBasedJobInfo>();


        #region subclasses

        internal abstract class SnmpObjectWithATimeStamp
        {
            public IbaSnmpOid Oid;

            /// <summary> A measure to tell whether data is fresh or outdated </summary>
            public static TimeSpan AgeThreshold { get; set; }
                // by default count all data as too old
                = TimeSpan.FromSeconds(0);

            /// <summary> When data has been last time updated </summary>
            public DateTime TimeStamp { get; private set; } = DateTime.MinValue;

            public bool IsUpToDate()
            {
                if (TimeStamp == DateTime.MinValue)
                {
                    return false;
                }
                TimeSpan age = DateTime.Now - TimeStamp;

                // if data younger than Threshold, then it is treated as fresh
                return age < AgeThreshold;
            }

            public void PutTimeStamp()
            {
                TimeStamp = DateTime.Now;
            }
        }

        internal class LicenseInfo : SnmpObjectWithATimeStamp
        {
            /// <summary> Oid 1 </summary>
            public bool IsValid;

            /// <summary> Oid 2 </summary>
            public string Sn;

            /// <summary> Oid 3 </summary>
            public string HwId;

            /// <summary> Oid 4 </summary>
            public string DongleType;

            /// <summary> Oid 5 </summary>
            public string Customer;

            /// <summary> Oid 6 </summary>
            public int TimeLimit;

            /// <summary> Oid 7 </summary>
            public int DemoTimeLimit;
        }

        internal enum JobStatus
        {
            Disabled = 0,
            Started = 1,
            Stopped = 2
        }

        internal class GlobalCleanupDriveInfo : SnmpObjectWithATimeStamp
        {
            /// <summary> Oid 0 </summary>
            public string DriveName;

            /// <summary> Oid 1 </summary>
            public bool Active;

            /// <summary> Oid 2 </summary>
            public uint Size;

            /// <summary> Oid 3 </summary>
            public uint CurrentFreeSpace;

            /// <summary> Oid 4 </summary>
            public uint MinFreeSpace;

            /// <summary> Oid 5 </summary>
            public uint RescanTime;

            public override string ToString()
            {
                return $@"{DriveName} [A:{Active}, {CurrentFreeSpace}/{Size}";
            }
        }

        //internal enum TaskType
        //{
        //    // todo
        //}

        /// <summary> OID 1...n - one struct per for each task </summary>
        internal class TaskInfo //: SnmpObjectWithATimeStamp
        {
            public IbaSnmpOid Oid;
            
            /// <summary> A Job the task belongs to </summary>
            public JobInfoBase Parent;

            /// <summary> Oid 0 </summary>
            public string TaskName;

            // todo convert to enum
            /// <summary> Oid 1 </summary>
            public string TaskType;

            /// <summary> Oid 2 </summary>
            public bool Success;

            /// <summary> Oid 3 </summary>
            public uint DurationOfLastExecution;

            /// <summary> Oid 4 </summary>
            public uint CurrentMemoryUsed;

            /// <summary> Oid 5, OPTIONAL, can be null for tasks that have no cleanup options </summary>
            public LocalCleanupInfo CleanupInfo;

            public override string ToString()
            {
                return $@"{TaskName} [{TaskType}, {Oid}, {Parent.JobName}]";
            }
        }

        internal class LocalCleanupInfo
        {
            /// <summary> Oid 0 </summary>
            public CleanupTaskData.OutputLimitChoiceEnum LimitChoice;

            /// <summary> Oid 1 </summary>
            public uint Subdirectories;

            /// <summary> Oid 2 </summary>
            public uint FreeDiskSpace;

            /// <summary> Oid 3 </summary>
            public uint UsedDiskSpace;
        }

        /// <summary> OID ...2 Standard Jobs </summary>
        internal abstract class JobInfoBase : SnmpObjectWithATimeStamp
        {
            /// <summary> key of the job list </summary>
            public Guid Guid;

            // general section - Oid 0
            /// <summary> Oid 0.0 </summary>
            public string JobName;

            /// <summary> Oid 0.1 (started / stopped /disabled);</summary>
            public JobStatus Status; 

            /// <summary> Oid 0.2 </summary>
            public uint TodoCount;

            /// <summary> Oid 0.3 </summary>
            public uint DoneCount;

            /// <summary> Oid 0.4 </summary>
            public uint FailedCount;

            // Oids 0.5 ... 0.8 vary depending on a Job type: Standard / Scheduled / One-time / Event-based
            // these oids are defined in derived classes

            /// <summary> Oid 1...n, where n - size of the list</summary>
            public List<TaskInfo> Tasks;

            public override string ToString()
            {
                string tasksString = (Tasks?.Count ?? 0).ToString();
                return $@"{JobName} [{Status}, {TodoCount}/{DoneCount}/{FailedCount}, T:{tasksString}]";
            }
        }

        internal class StandardJobInfo : JobInfoBase
        {
            /// <summary> Oid 0.5 </summary>
            public uint PermFailedCount;

            /// <summary> Oid 0.6 </summary>
            public string TimestampJobStarted;

            /// <summary> Oid 0.7 </summary>
            public string LastCycleScanningTime;

            /// <summary> Oid 0.8.0 </summary>
            public string LastProcessingLastDatFileProcessed;

            /// <summary> Oid 0.8.1 </summary>
            public string LastProcessingStartTimeStamp;

            /// <summary> Oid 0.8.2 </summary>
            public string LastProcessingFinishTimeStamp;
        }

        internal class ScheduledJobInfo : JobInfoBase
        {
            /// <summary> Oid 0.5 </summary>
            public uint PermFailedCount;

            /// <summary> Oid 0.6 </summary>
            public string TimestampLastExecution;

            /// <summary> Oid 0.7 </summary>
            public string TimestampNextExecution;
        }

        internal class OneTimeJobInfo : JobInfoBase
        {
            /// <summary> Oid 0.5 </summary>
            public string TimestampLastExecution;
        }

        /// <summary> reserved for future </summary> 
        internal class EventBasedJobInfo : JobInfoBase
        {
            /// <summary> Oid 0.??? </summary>
            public string EventDescription = @"This is a description of the event";
        }

        #endregion
    }
}
