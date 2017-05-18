using System;
using System.Collections.Generic;

namespace iba.Data
{
    /// <summary>
    /// Is used for transferring the information about all SNMP-exposable objects between
    /// the TaskManager and SnmpWorker.
    /// </summary>
    internal class SnmpObjectsData
    {
        /// <summary> Time stamp when this information was refreshed.
        /// Used Not for SNMP exposure, just for internal purposes. </summary>
        public DateTime Stamp;

        public int _tmp_reset_cnt { get; private set; }
        public int _tmp_updated_cnt { get; set; }

        public SnmpObjectsData()
        {
            Reset();
        }

        public void Reset()
        {
            // contained data is not valid
            Stamp = DateTime.MinValue;
            GlobalCleanup.Clear();
            StandardJobs.Clear();
            ScheduledJobs.Clear();
            OneTimeJobs.Clear();
            EventBasedJobs.Clear();

            // tmp
            // todo remove
            _tmp_reset_cnt++;
        }
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
        // todo find existing enums
        // job status, task type
        // internal enum 

        internal enum JobStatus
        {
            Disabled = 0,
            Started = 1,
            Stopped = 2
        }

        internal class GlobalCleanupDriveInfo
        {
            /// <summary> Oid 0 </summary>
            public string DriveName;

            /// <summary> Oid 1 </summary>
            public bool Active;

            /// <summary> Oid 2 </summary>
            public int Size;

            /// <summary> Oid 3 </summary>
            public int CurrentFreeSpace;

            /// <summary> Oid 4 </summary>
            public int MinFreeSpace;

            /// <summary> Oid 5 </summary>
            public int RescanTime;
        }

        internal class LocalCleanupInfo
        {
            // todo discuss this oid
            /// <summary> Oid 0 </summary>
            public CleanupTaskData.OutputLimitChoiceEnum LimitChoice;

            /// <summary> Oid 1 </summary>
            public uint Subdirectories;

            /// <summary> Oid 2 </summary>
            public uint FreeDiskSpace;

            /// <summary> Oid 3 </summary>
            public uint UsedDiskSpace;
        }

        /// <summary> OID 1...n - one struct per for each task </summary>
        internal class TaskInfo
        {
            /// <summary> Oid 0 </summary>
            public string TaskName;

            // todo convert to enum
            /// <summary> Oid 1 </summary>
            public string TaskType;

            /// <summary> Oid 2 </summary>
            public bool Success;

            /// <summary> Oid 3 </summary>
            public int DurationOfLastExecution;

            /// <summary> Oid 4 </summary>
            public int CurrentMemoryUsed;

            /// <summary> Oid 5, OPTIONAL, can be null for tasks that have no cleanup options </summary>
            public LocalCleanupInfo CleanupInfo;
        }

        /// <summary> OID ...2 Standard Jobs </summary>
        internal abstract class JobInfoBase
        {
            // general section - Oid 0
            /// <summary> Oid 0.0 </summary>
            public string JobName;

            /// <summary> Oid 0.1 (started / stopped /disabled);</summary>
            public JobStatus Status; 

            /// <summary> Oid 0.2 </summary>
            public int TodoCount;

            /// <summary> Oid 0.3 </summary>
            public int DoneCount;

            /// <summary> Oid 0.4 </summary>
            public int FailedCount;

            // Oids 0.5 ... 0.8 vary depending on a Job type: Standard / Scheduled / One-time / Event-based
            // these oids are defined in derived classes

            /// <summary> Oid 1...n, where n - size of the list</summary>
            public List<TaskInfo> Tasks;
        }

        internal class StandardJobInfo : JobInfoBase
        {
            /// <summary> Oid 0.5 </summary>
            public int PermFailedCount;

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
            public int PermFailedCount;

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
            public string EventDescription = "This is a description of the event";
        }

        #endregion
    }
}
