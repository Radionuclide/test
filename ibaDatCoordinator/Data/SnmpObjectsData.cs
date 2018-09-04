using System;
using System.Collections.Generic;
using IbaSnmpLib;

namespace iba.Data
{
    /// <summary>
    /// Is used for transferring information about all SNMP-visible objects between
    /// TaskManager and SnmpWorker.
    /// </summary>
    internal class SnmpObjectsData 
    {        
        /// <summary> PrGeneral.3 </summary>
        public LicenseInfo License = new LicenseInfo();

        /// <summary> PrSpecific.1 </summary>
        public List<GlobalCleanupDriveInfo> GlobalCleanup = new List<GlobalCleanupDriveInfo>();
        /// <summary> Least significant (rightmost) subid for corresponding object </summary>
        public const uint GlobalCleanupOid = 1;

        /// <summary> PrSpecific.2 </summary>
        public List<StandardJobInfo> StandardJobs = new List<StandardJobInfo>();
        /// <summary> Least significant (rightmost) subid for corresponding object </summary>
        public const uint StandardJobsOid = 2;

        /// <summary> PrSpecific.3 </summary>
        public List<ScheduledJobInfo> ScheduledJobs = new List<ScheduledJobInfo>();
        /// <summary> Least significant (rightmost) subid for corresponding object </summary>
        public const uint ScheduledJobsOid = 3;

        /// <summary> PrSpecific.4 </summary>
        public List<OneTimeJobInfo> OneTimeJobs = new List<OneTimeJobInfo>();
        /// <summary> Least significant (rightmost) subid for corresponding object </summary>
        public const uint OneTimeJobsOid = 4;

        /// <summary> PrSpecific.5 </summary>
        public List<EventBasedJobInfo> EventBasedJobs = new List<EventBasedJobInfo>();
        /// <summary> Least significant (rightmost) subid for corresponding object </summary>
        public const uint EventBasedJobsOid = 5;

        public void Reset()
        {
            License.Reset();

            GlobalCleanup.Clear();

            StandardJobs.Clear();
            ScheduledJobs.Clear();
            OneTimeJobs.Clear();
            EventBasedJobs.Clear();
        }

        #region Common

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

                // if data is younger than Threshold, then it is treated as fresh
                return age < AgeThreshold;
            }

            public void PutTimeStamp()
            {
                TimeStamp = DateTime.Now;
            }
        }

        #endregion


        #region License

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

            /// <summary> Resets all the fields to default values </summary>
            public void Reset()
            {
                IsValid = false;
                Sn = "";
                HwId = "";
                DongleType = "";
                Customer = "";
                TimeLimit = 0;
                DemoTimeLimit = 0;
            }
        }

        #endregion


        #region Global Cleanup

        internal class GlobalCleanupDriveInfo : SnmpObjectWithATimeStamp
        {
            /// <summary> Oid 1 </summary>
            public string DriveName;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint DriveNameOid = 1;

            /// <summary> Oid 2 </summary>
            public bool Active;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint ActiveOid = 2;

            /// <summary> Oid 3 </summary>
            public uint SizeInMb;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint SizeInMbOid = 3;

            /// <summary> Oid 4 </summary>
            public uint CurrentFreeSpaceInMb;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint CurrentFreeSpaceInMbOid = 4;

            /// <summary> Oid 5 </summary>
            public uint MinFreeSpaceInPercent;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint MinFreeSpaceInPercentOid = 5;

            /// <summary> Oid 6 </summary>
            public uint RescanTime;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint RescanTimeOid = 6;

            /// <summary> Resets to default values everything except DriveName (its key) </summary>
            public void Reset()
            {
                // DriveName =; // do NOT reset primary key
                Active = false;
                SizeInMb = 0;
                CurrentFreeSpaceInMb = 0;
                MinFreeSpaceInPercent = 0;
                RescanTime = 0;
            }

            public override string ToString()
            {
                return $@"{DriveName} [A:{Active}, {CurrentFreeSpaceInMb}/{SizeInMb}";
            }
        }

        #endregion


        #region Jobs and tasks

        internal enum JobStatus
        {
            Disabled = 0,
            Started = 1,
            Stopped = 2
        }

        /// <summary> OID 1...n - one struct per for each task </summary>
        internal class TaskInfo
        {
            public IbaSnmpOid Oid;
            
            /// <summary> A Job the task belongs to </summary>
            public JobInfoBase Parent;

            /// <summary> Oid 1 </summary>
            public string TaskName;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TaskNameOid = 1;

            /// <summary> Oid 2 </summary>
            public string TaskType;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TaskTypeOid = 2;

            /// <summary> Oid 3 </summary>
            public bool Success;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint SuccessOid = 3;

            /// <summary> Oid 4, in seconds </summary>
            public uint DurationOfLastExecution;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint DurationOfLastExecutionOid = 4;

            /// <summary> Oid 5 in megabytes </summary>
            public uint MemoryUsedForLastExecution;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint MemoryUsedForLastExecutionOid = 5;

            /// <summary> Oid 6, OPTIONAL, can be null for tasks that have no cleanup settings </summary>
            public LocalCleanupInfo CleanupInfo;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint CleanupInfoOid = 6;

            /// <summary> Resets to default values everything except Oid and Parent </summary>
            public void Reset()
            {
                TaskName = "";
                TaskType = "";
                Success = false;
                DurationOfLastExecution = 0;
                MemoryUsedForLastExecution = 0;
                CleanupInfo = null;
            }

            public override string ToString()
            {
                return $@"{TaskName} [{TaskType}, {Oid}, {Parent.JobName}]";
            }
        }


        internal class LocalCleanupInfo
        {
            /// <summary> Oid 1 </summary>
            public TaskWithTargetDirData.OutputLimitChoiceEnum LimitChoice;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint LimitChoiceOid = 1;

            /// <summary> Oid 2 </summary>
            public uint Subdirectories;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint SubdirectoriesOid = 2;

            /// <summary> Oid 3 </summary>
            public uint UsedDiskSpace;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint UsedDiskSpaceOid = 3;

            /// <summary> Oid 4 </summary>
            public uint FreeDiskSpace;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint FreeDiskSpaceOid = 4;

        }

        /// <summary> OID ...2 Standard Jobs </summary>
        internal abstract class JobInfoBase : SnmpObjectWithATimeStamp
        {
            /// <summary> key of the job list </summary>
            public Guid Guid;

            // general section
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint GeneralOid = 1;

            /// <summary> Oid General.1 </summary>
            public string JobName;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint JobNameOid = 1;

            /// <summary> Oid General.2 (started / stopped /disabled);</summary>
            public JobStatus Status;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint StatusOid = 2;

            /// <summary> Oid General.3 </summary>
            public uint TodoCount;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TodoCountOid = 3;

            /// <summary> Oid General.4 </summary>
            public uint DoneCount;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint DoneCountOid = 4;

            /// <summary> Oid General.5 </summary>
            public uint FailedCount;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint FailedCountOid = 5;

            // Oids General.5 ... General.8 vary depending on a Job type: Standard / Scheduled / One-time / Event-based
            // these oids are defined in derived classes

            /// <summary> Oid Tasks.1...Tasks.n, where n - size of the list</summary>
            public List<TaskInfo> Tasks;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TasksOid = 2;

            /// <summary> Resets to default values everything except Guid (primary key) and Tasks </summary>
            public virtual void Reset()
            {
                JobName = "";
                Status = JobStatus.Disabled;
                TodoCount = 0;
                DoneCount = 0;
                FailedCount = 0;
            }

            public override string ToString()
            {
                string tasksString = (Tasks?.Count ?? 0).ToString();
                return $@"{JobName} [{Status}, {TodoCount}/{DoneCount}/{FailedCount}, T:{tasksString}]";
            }
        }

        internal class StandardJobInfo : JobInfoBase
        {
            /// <summary> Oid 6 </summary>
            public uint PermFailedCount;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint PermFailedCountOid = 6;

            /// <summary> Oid 7 </summary>
            public DateTime TimestampJobStarted;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TimestampJobStartedOid = 7;

            /// <summary> Oid 8 </summary>
            public DateTime TimestampLastDirectoryScan;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TimestampLastDirectoryScanOid = 8;

            /// <summary> Oid 9 </summary>
            public DateTime TimestampLastReprocessErrorsScan;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TimestampLastReprocessErrorsScanOid = 9;

            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint LastProcessingOid = 10;

            /// <summary> Oid 10.1 </summary>
            public string LastProcessingLastDatFileProcessed;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint LastProcessingLastDatFileProcessedOid = 1;

            /// <summary> Oid 10.2 </summary>
            public DateTime LastProcessingStartTimeStamp;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint LastProcessingStartTimeStampOid = 2;

            /// <summary> Oid 10.3 </summary>
            public DateTime LastProcessingFinishTimeStamp;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint LastProcessingFinishTimeStampOid = 3;

            public override void Reset()
            {
                base.Reset();

                PermFailedCount = 0;
                TimestampJobStarted = DateTime.MinValue;
                TimestampLastDirectoryScan = DateTime.MinValue;
                TimestampLastReprocessErrorsScan = DateTime.MinValue;
                LastProcessingLastDatFileProcessed = "";
                LastProcessingStartTimeStamp = DateTime.MinValue;
                LastProcessingFinishTimeStamp = DateTime.MinValue;
            }
        }

        internal class ScheduledJobInfo : JobInfoBase
        {
            /// <summary> Oid 6 </summary>
            public uint PermFailedCount;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint PermFailedCountOid = 6;

            /// <summary> Oid 7 </summary>
            public DateTime TimestampJobStarted;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TimestampJobStartedOid = 7;

            /// <summary> Oid 8 </summary>
            public DateTime TimestampLastExecution;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TimestampLastExecutionOid = 8;

            /// <summary> Oid 9 </summary>
            public DateTime TimestampNextExecution;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TimestampNextExecutionOid = 9;

            public override void Reset()
            {
                base.Reset();
                PermFailedCount = 0;
                TimestampJobStarted = DateTime.MinValue;
                TimestampLastExecution = DateTime.MinValue;
                TimestampNextExecution = DateTime.MinValue;
            }
        }

        internal class EventBasedJobInfo : JobInfoBase
        {
            /// <summary> Oid 6 </summary>
            public uint PermFailedCount;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint PermFailedCountOid = 6;

            /// <summary> Oid 7 </summary>
            public DateTime TimestampJobStarted;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TimestampJobStartedOid = 7;

            /// <summary> Oid 8 </summary>
            public DateTime TimestampLastExecution;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TimestampLastExecutionOid = 8;

            public override void Reset()
            {
                base.Reset();
                PermFailedCount = 0;
                TimestampJobStarted = DateTime.MinValue;
                TimestampLastExecution = DateTime.MinValue;
            }
        }

        internal class OneTimeJobInfo : JobInfoBase
        {
            /// <summary> Oid 6 </summary>
            public DateTime TimestampLastExecution;
            /// <summary> Least significant (rightmost) subid for corresponding object </summary>
            public const uint TimestampLastExecutionOid = 6;

            public override void Reset()
            {
                base.Reset();
                TimestampLastExecution = DateTime.MinValue;
            }
        }
        #endregion
        
    }
}
