using System;
using System.Collections.Generic;
using iba.Processing;
using IbaSnmpLib;

namespace iba.Data
{

    // todo move inside SnmpObjectsData
    internal abstract class SnmpObjectWithATimeStamp
    {
        public static TimeSpan AgeThreshold { get; set; } = TimeSpan.FromSeconds(5);

        public DateTime TimeStamp { get; private set; } = DateTime.MinValue;

        public bool IsUpToDate()
        {
            if (TimeStamp == DateTime.MinValue)
            {
                return false;
            }
            TimeSpan age = DateTime.Now - TimeStamp;
            // if data is not too old, then it is okay
            return age < AgeThreshold;
        }

        public void PutTimeStamp()
        {
            TimeStamp = DateTime.Now;
        }
    }

    /// <summary>
    /// Is used for transferring the information about all SNMP-visible objects between
    /// the TaskManager and SnmpWorker.
    /// </summary>
    internal class SnmpObjectsData 
        // todo remove inheritance when all included stamps will be used properly
        : SnmpObjectWithATimeStamp
    {
        public bool IsStructureValid { get; set; }

        public int _tmp_reset_cnt { get; private set; }
        public int _tmp_updated_cnt { get; set; }

        public SnmpObjectsData()
        {
            Reset();
        }
        
        public void Reset()
        {
            GlobalCleanup.Clear();
            StandardJobs.Clear();
            ScheduledJobs.Clear();
            OneTimeJobs.Clear();
            EventBasedJobs.Clear();

            // tmp
            // todo remove
            _tmp_reset_cnt++;
            SnmpWorker.TmpLogLine("SnmpObjectsData.Reset()");
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

        internal class GlobalCleanupDriveInfo : SnmpObjectWithATimeStamp
        {
            /// <summary> least id in the snmp oid of this item </summary>
            public uint Id;

            /// <summary> Oid 0 </summary>
            public string DriveNameId0;

            /// <summary> Oid 1 </summary>
            public bool ActiveId1;

            /// <summary> Oid 2 </summary>
            public uint SizeId2;

            /// <summary> Oid 3 </summary>
            public uint CurrentFreeSpaceId3;

            /// <summary> Oid 4 </summary>
            public uint MinFreeSpaceId4;

            /// <summary> Oid 5 </summary>
            public uint RescanTimeId5;
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
        internal class TaskInfo : SnmpObjectWithATimeStamp
        {
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
