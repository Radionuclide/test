using System;
using System.Collections.Generic;
using System.Diagnostics;
using iba.Utility;
using ibaOpcServer.IbaOpcUa;
using IbaSnmpLib;

namespace iba.Data
{
    /// <summary>
    /// External Monitoring Data.
    /// Contains all data that can be monitored from outside of DatCoordinator - via SNMP or OPC UA.
    /// Is used as a middle layer between: TaskManager on the one side
    /// and SnmpWorker or OpcUaWorker on the other side.
    /// </summary>
    internal class ExtMonData 
    {
        #region Body
        
        #region Fields and Props

        public readonly ExtMonFolder FolderRoot;

        /// <summary> SNMP: PrGeneral.3.
        /// In SNMP it is handled specially, because it is located not in product-specific area.
        /// In OPC UA it is handled as a usual folder </summary>
        public readonly LicenseInfo License;

        /// <summary> SNMP: PrSpecific.1 </summary>
        public readonly ExtMonFolder FolderGlobalCleanup;

        /// <summary> SNMP: PrSpecific.2 </summary>
        public readonly ExtMonFolder FolderStandardJobs;

        /// <summary> SNMP: PrSpecific.3 </summary>
        public readonly ExtMonFolder FolderScheduledJobs;

        /// <summary> SNMP: PrSpecific.4 </summary>
        public readonly ExtMonFolder FolderOneTimeJobs;

        /// <summary> SNMP: PrSpecific.5 </summary>
        public readonly ExtMonFolder FolderEventBasedJobs;


        /// <summary> SNMP: PrSpecific.1 </summary> // todo. kls. eliminate two lists??? Typed list as encapsulation of folder??
        public readonly List<GlobalCleanupDriveInfo> GlobalCleanup = new List<GlobalCleanupDriveInfo>();

        /// <summary> SNMP: PrSpecific.2 </summary>
        public readonly List<StandardJobInfo> StandardJobs = new List<StandardJobInfo>();

        /// <summary> SNMP: PrSpecific.3 </summary>
        public readonly List<ScheduledJobInfo> ScheduledJobs = new List<ScheduledJobInfo>();

        /// <summary> SNMP: PrSpecific.4 </summary>
        public readonly List<OneTimeJobInfo> OneTimeJobs = new List<OneTimeJobInfo>();

        /// <summary> SNMP: PrSpecific.5 </summary>
        public readonly List<EventBasedJobInfo> EventBasedJobs = new List<EventBasedJobInfo>();

        #endregion


        #region Construction and Reset

        public ExtMonData()
        {
            FolderRoot = new ExtMonFolder(null, 0, @"Root", @"Root", @"Root")
            { SnmpFullMibName = "Root" };
            // FolderRoot.SnmpFullOid = ; // it has no SNMP OID

            FolderRoot.Children.Add(
                License = new LicenseInfo(FolderRoot));
            //License.SnmpFullOid = ; // is not used. License OID is actually "PrGeneral.3", but it's handled specially

            FolderRoot.Children.Add(
                FolderGlobalCleanup = new ExtMonFolder(FolderRoot, new IbaSnmpOid(1),
                @"Global cleanup", @"globalCleanup",
                "Global cleanup settings for all local drives."));

            FolderRoot.Children.Add(
                FolderStandardJobs = new ExtMonFolder(FolderRoot, new IbaSnmpOid(2),
                @"Standard jobs", @"standardJobs",
                @"List of all standard jobs."));

            FolderRoot.Children.Add(
                FolderScheduledJobs = new ExtMonFolder(FolderRoot, new IbaSnmpOid(3),
                @"Scheduled jobs", @"scheduledJobs",
                @"List of all scheduled jobs."));

            FolderRoot.Children.Add(
                FolderOneTimeJobs = new ExtMonFolder(FolderRoot, new IbaSnmpOid(4),
                @"One time jobs", @"oneTimeJobs",
                @"List of all one-time jobs."));

            FolderRoot.Children.Add(
                FolderEventBasedJobs = new ExtMonFolder(FolderRoot, new IbaSnmpOid(5),
                @"Event jobs", @"eventJobs",
                @"List of all event jobs."));
        }

        public void Reset()
        {
            License.Reset();

            GlobalCleanup.Clear();
            FolderGlobalCleanup.Children.Clear();

            StandardJobs.Clear();
            FolderStandardJobs.Children.Clear();

            ScheduledJobs.Clear();
            FolderScheduledJobs.Children.Clear();

            OneTimeJobs.Clear();
            FolderOneTimeJobs.Children.Clear();

            EventBasedJobs.Clear();
            FolderEventBasedJobs.Children.Clear();
        }

        #endregion


        #region AddNewXxx() and auxiliary

        public GlobalCleanupDriveInfo AddNewGlobalCleanup(string driveName)
        {
            var driveInfo = new GlobalCleanupDriveInfo(FolderGlobalCleanup, (uint)GlobalCleanup.Count + 1, driveName);
            GlobalCleanup.Add(driveInfo);
            FolderGlobalCleanup.Children.Add(driveInfo);
            return driveInfo;
        }

        public JobInfoBase AddNewJob(ConfigurationData.JobTypeEnum jobType, string jobName, Guid guid)
        {
            JobInfoBase jobInfo;
            ExtMonFolder folder;
            
            switch (jobType)
            {
                case ConfigurationData.JobTypeEnum.DatTriggered: // standard Job
                    folder = FolderStandardJobs;
                    jobInfo = new StandardJobInfo(folder, (uint) StandardJobs.Count + 1, jobName);
                    StandardJobs.Add((StandardJobInfo) jobInfo);
                    break;

                case ConfigurationData.JobTypeEnum.Scheduled:
                    folder = FolderScheduledJobs;
                    jobInfo = new ScheduledJobInfo(folder, (uint)ScheduledJobs.Count + 1, jobName);
                    ScheduledJobs.Add((ScheduledJobInfo) jobInfo);
                    // fill the data
                    break;

                case ConfigurationData.JobTypeEnum.OneTime:
                    folder = FolderOneTimeJobs;
                    jobInfo = new OneTimeJobInfo(folder, (uint)OneTimeJobs.Count + 1, jobName);
                    OneTimeJobs.Add((OneTimeJobInfo) jobInfo);
                    // fill the data
                    break;

                case ConfigurationData.JobTypeEnum.Event:
                    folder = FolderEventBasedJobs;
                    jobInfo = new EventBasedJobInfo(folder, (uint)EventBasedJobs.Count + 1, jobName);
                    EventBasedJobs.Add((EventBasedJobInfo) jobInfo);
                    // fill the data
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            jobInfo.Guid = guid;
            folder.Children.Add(jobInfo);
            return jobInfo;
        }

        public static TaskInfo AddNewTask(JobInfoBase parentJob, string taskName)
        {
            var taskInfo = new TaskInfo(parentJob.FolderTasks, (uint)parentJob.Tasks.Count + 1, taskName);
            parentJob.Tasks.Add(taskInfo);
            parentJob.FolderTasks.Children.Add(taskInfo);
            return taskInfo;
        }

        public List<ExtMonNode> GetFlatListOfAllChildren() => FolderRoot.GetFlatListOfAllChildren();

        public bool CheckConsistency()
        {
            Debug.Assert(StandardJobs != null);

            // todo. kls. check tree structure (parent-child cross ref)

            // get list of all nodes
            var children = GetFlatListOfAllChildren();

            Set<IbaSnmpOid> oids = new Set<IbaSnmpOid>();
            Set<string> mibNames = new Set<string>();

            foreach (var child in children)
            {
                // check OIDs uniqueness
                var oid = child.SnmpFullOid;
                if (oids.Contains(oid))
                    return false;
                oids.Add(oid);

                // check mib name uniqueness
                var mibName = child.SnmpFullMibName;
                if (mibNames.Contains(mibName))
                    return false;
                mibNames.Add(mibName);
            }

            return true;
        }

        #endregion
        
        #endregion

        

        #region Nested classes


        #region Abstract and common classes

        /// <summary>
        /// 'Node' is either folder or variable. // todo. kls. to comment
        /// </summary>
        internal abstract class ExtMonNode
        {
            public readonly ExtMonFolder Parent; // todo. kls. try make readonly 

            public string Caption; // todo. kls. try make readonly 
            public string Description; // todo. kls. try make readonly 


            /// <summary> SNMP: Least significant (rightmost) subId of SNMP OID for corresponding object </summary>
            public readonly uint SnmpLeastId;

            public IbaSnmpOid _snmpFullOid;
            /// <summary> Can be set directly or can be evaluated dynamically from SnmpLeastId and parent's OID</summary>
            public IbaSnmpOid SnmpFullOid
            {
                get
                {
                    if (_snmpFullOid == null)
                    {
                        _snmpFullOid = Parent == null || Parent.SnmpFullOid == null
                            ? null
                            : Parent.SnmpFullOid + SnmpLeastId;
                    }

                    Debug.Assert(SnmpLeastId == 0 || _snmpFullOid == null || SnmpLeastId == _snmpFullOid.GetLeastSignificantSubId());
                    // calculate (and keep) full OID on first request
                    return _snmpFullOid;
                }
                set => _snmpFullOid = value;
            }

            /// <summary> e.g. "Status" for the item "standardJob3Task2Status" </summary>
            public readonly string SnmpMibNameSuffix;

            private string _snmpFullMibName;

            /// <summary> Can be set directly or can be evaluated dynamically from MibSuffix and parent's MibName </summary>
            public string SnmpFullMibName // todo rename
            {
                get
                {
                    if (_snmpFullMibName == null)
                    {
                        _snmpFullMibName = Parent?.SnmpFullMibName == null
                            ? null
                            : Parent.SnmpFullMibName + SnmpMibNameSuffix;

                    }

                    Debug.Assert(SnmpMibNameSuffix == null || _snmpFullMibName == null || _snmpFullMibName.Contains(SnmpMibNameSuffix));
                    // calculate full MIB name on first request
                    return _snmpFullMibName;
                }
                set => _snmpFullMibName = value;
            }


            /// <summary> Full OPC UA NodeId (path) of the node in UA address space</summary>
            public string UaFullId;
            // todo UaName ?? is equal to Caption??


            protected ExtMonNode(ExtMonFolder parent, uint snmpLeastId, 
                string caption = null, string snmpMibNameSuffix = null, string description = null)
            {
                Parent = parent;
                SnmpLeastId = snmpLeastId;
                Caption = caption;
                Description = description;
                SnmpMibNameSuffix = snmpMibNameSuffix;
            }

            public override string ToString()
            {
                return $@"Node '{Caption}', OID={SnmpFullOid}, MIB='{SnmpFullMibName}'";
            }
        }

        internal abstract class ExtMonVariableBase : ExtMonNode
        {
            /// <summary> Gets contained data as untyped <see cref="object"/> </summary>
            public abstract object ObjValue { get; }

            /// <summary>  // todo. kls. to comment  </summary>
            public readonly ExtMonGroup Group;

            public object handler;
            protected ExtMonVariableBase(ExtMonFolder parent, string caption, string snmpMibNameSuffix, string description,
                uint snmpLeastId)
                : base(parent, snmpLeastId, caption, snmpMibNameSuffix, description)
            {
                Group = GetGroup();
            }

            public ExtMonGroup GetGroup() // todo. kls. optimize to lazy
            {
                if (Parent == null)
                    return null;
                ExtMonGroup group = null;

                ExtMonNode ancestor = Parent;
                for (int i = 0; i < 4; i++)
                {
                    if (ancestor == null)
                    {
                        break;
                    }

                    if (ancestor is ExtMonGroup ancestorAsGroup)
                    {
                        group = ancestorAsGroup;
                        break;
                    }

                    // look higher
                    ancestor = ancestor.Parent;
                }

                Debug.Assert(group != null);
                return group;
            }

            public override string ToString()
            {
                return $@"Variable '{Caption}', OID={SnmpFullOid}, MIB='{SnmpFullMibName}', G='{Group?.Caption}', UA='{UaVar?.BrowseName}'";
            }

            //#region SNMP - specific
            ///// <summary> Least significant (rightmost) subId of SNMP OID for corresponding object </summary>
            //public readonly uint SnmpLeastId;
            ///// <summary> SNMP OID for corresponding object </summary>
            //public IbaSnmpOid SnmpOid => (Parent == null || Parent.Oid == null) ? null : Parent.Oid + SnmpLeastId;
            ////public readonly IbaSnmpOid SnmpOid;
            //public readonly string SnmpMibNameSuffix;
            //#endregion

            #region OPC UA - specific

            public IbaOpcUaVariable UaVar = null;

            #endregion
        }

        internal class ExtMonVariable<T> : ExtMonVariableBase
        {
            public T Value;

            public ExtMonVariable(ExtMonFolder parent, string caption, string snmpMibNameSuffix, string description,
                uint snmpLeastId)
                : base(parent, caption, snmpMibNameSuffix, description, snmpLeastId)
            {
            }

            public override object ObjValue => Value;

            public override string ToString()
            {
                return $@"({typeof(T).Name}){Value}: " + base.ToString();
            }
        }

        internal class ExtMonFolder : ExtMonNode // todo. kls. Rename to ExternalMonitoringGroup 
        {
            public readonly List<ExtMonNode> Children = new List<ExtMonNode>();

            public List<ExtMonNode> GetFlatListOfAllChildren()
            {
                var list = new List<ExtMonNode>();
                foreach (var child in Children)
                {
                    switch (child)
                    {
                        case ExtMonFolder folder:
                            list.AddRange(folder.GetFlatListOfAllChildren());
                            break;
                        case ExtMonVariableBase variable:
                            list.Add(variable);
                            break;
                    }
                }
                return list;
            }

            public List<ExtMonVariableBase> GetFlatListOfAllVariables()
            {
                var list = new List<ExtMonVariableBase>();
                foreach (var child in Children)
                {
                    switch (child)
                    {
                        case ExtMonFolder folder:
                            list.AddRange(folder.GetFlatListOfAllVariables());
                            break;
                        case ExtMonVariableBase variable:
                            list.Add(variable);
                            break;
                    }
                }
                return list;
            }

            /// <summary>
            /// // todo. kls. to comment
            /// Is used for "Big" folders - section roots.
            /// Note, that here FULL OID and FULL MIB name are used (not suffixes).
            /// </summary>
            public ExtMonFolder(ExtMonFolder parent, IbaSnmpOid fullOid,
                string caption = null, string snmpFullMibName = null, string description = null)
                : base(parent, 0, caption, null, description)
            {
                SnmpFullMibName = snmpFullMibName;
                SnmpFullOid = fullOid;
            }

            /// <summary>
            /// // todo. kls. to comment
            /// Is used for "small" folders.
            /// Note, that here suffixes are used (not full OID or MIB name).
            /// </summary>
            public ExtMonFolder(ExtMonFolder parent, uint snmpLeastId,
                string caption = null, string snmpMibNameSuffix = null, string description = null)
                : base(parent, snmpLeastId, caption, snmpMibNameSuffix, description)
            {
            }

            public override string ToString()
            {
                return $@"Folder '{Caption}', OID={SnmpFullOid}, MIB='{SnmpFullMibName}'";
            }
        }

        /// <summary> Group is a special kind of folder.
        /// its items are updated altogether.
        /// // todo. kls. to comment
        /// </summary>
        internal abstract class ExtMonGroup : ExtMonFolder
        {

            //protected ExtMonGroup(ExtMonFolder parent, uint snmpLeastId) : // todo. kls. remove parameterless

            //    this(parent, null, null, null, snmpLeastId)
            //{ }

            protected ExtMonGroup(ExtMonFolder parent, uint snmpLeastId,
                string caption = null, string snmpMibNameSuffix = null, string description = null)
                : base(parent, snmpLeastId, caption, snmpMibNameSuffix, description)
            {
            }

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


        internal class LicenseInfo : ExtMonGroup
        {
            public LicenseInfo(ExtMonFolder parent) 
                : base(parent, 0 /*not used*/)
            {
            }

            // todo. kls. need captions, but don't need OIDs and MIBs

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


        internal class GlobalCleanupDriveInfo : ExtMonGroup
        {
            /// <summary> Primary key for the drive info collection (is uniquely identified by drive name) </summary>
            public string Key => DriveName.Value;

            public readonly ExtMonVariable<string> DriveName;
            public readonly ExtMonVariable<bool> Active;


            public readonly ExtMonVariable<uint> SizeInMb;
            public readonly ExtMonVariable<uint> CurrentFreeSpaceInMb;
            public readonly ExtMonVariable<uint> MinFreeSpaceInPercent;
            public readonly ExtMonVariable<uint> RescanTime;

            public GlobalCleanupDriveInfo(ExtMonFolder parent, uint snmpLeastId, string driveName) 
                : base(parent, snmpLeastId)
            {
                // set Caption, Description, etc.
                Caption = $@"Drive '{driveName}'";
                SnmpFullMibName = $@"globalCleanupDrive{snmpLeastId}"; 
                Description = $@"Global cleanup settings for the drive '{driveName}'";

                // create variables and add them to collection

                Children.Add(
                DriveName = new ExtMonVariable<string>(this, @"Drive Name", @"Name",
                    @"Drive name like it appears in operating system.",
                    1));

                Children.Add(
                    Active = new ExtMonVariable<bool>(this, @"Active", @"Active",
                    @"Whether or not the global cleanup is enabled for the drive.",
                    2));

                Children.Add(
                    SizeInMb = new ExtMonVariable<uint>(this, @"Size", @"Size",
                    @"Size of the drive (in megabytes).",
                    3));

                Children.Add(
                    CurrentFreeSpaceInMb = new ExtMonVariable<uint>(this, @"Current free space", @"CurrFreeSpace",
                    @"Current free space of the drive (in megabytes).",
                    4));

                Children.Add(
                    MinFreeSpaceInPercent = new ExtMonVariable<uint>(this, @"Min free space", @"MinFreeSpace",
                    @"Minimum disk space that is kept free on the drive by deleting the oldest iba dat files (in percent).",
                    5));

                Children.Add(
                    RescanTime = new ExtMonVariable<uint>(this, @"Rescan time", @"RescanTime",
                    @"How often the application rescans the drive parameters (in minutes).", 
                    6));

                // set DriveName.Value right now, because it will serve as a primary key of DriveInfo collection
                DriveName.Value = driveName;
                
                // set default values
                Reset();
            }

            /// <summary> Resets to default values everything except DriveName (its key) </summary>
            public void Reset()
            {
                // DriveName =; // do NOT reset primary key
                Active.Value = false;
                SizeInMb.Value = 0;
                CurrentFreeSpaceInMb.Value = 0;
                MinFreeSpaceInPercent.Value = 0;
                RescanTime.Value = 0;
            }

            public override string ToString()
            {
                return $@"Drive '{Key}' [A:{Active.Value}, {CurrentFreeSpaceInMb.Value}/{SizeInMb.Value}]";
            }
        }

        
        #region Tasks

        /// <summary> OID 1...n - one class instance per each task </summary>
        internal class TaskInfo : ExtMonFolder
        {
            public readonly ExtMonVariable<string> TaskName;
            public readonly ExtMonVariable<string> TaskType;
            public readonly ExtMonVariable<bool> Success;
            public readonly ExtMonVariable<uint> DurationOfLastExecutionInSec;
            public readonly ExtMonVariable<uint> MemoryUsedForLastExecutionInMb;

            /// <summary> A Job the task belongs to </summary>
            public JobInfoBase ParentJob; // todo remove

            /// <summary> Oid 6. Folder is OPTIONAL, can be empty for tasks that have no cleanup settings </summary>
            public LocalCleanupInfo CleanupInfo { get; private set; }

            public TaskInfo(ExtMonFolder parent, uint snmpLeastId, string taskName)
                : base(parent, snmpLeastId)
            {
                // determine parent job
                Debug.Assert(parent != null); // "job.tasks" folder
                Debug.Assert(parent.Parent != null); // job
                ParentJob = parent?.Parent as JobInfoBase;
                Debug.Assert(ParentJob != null);

                // set Caption, Description, etc.
                Caption = $@"Task '{taskName}'"; // can be non-unique
                SnmpFullMibName = ParentJob.SnmpFullMibName + $@"Task{snmpLeastId}";
                Description = $@"Information about task '{taskName}' of the job '{ParentJob.JobName.Value}'.";

                // create variables and add them to collection

                // todo. kls. change MIB name and desc on TaskName change?
                Children.Add(
                    TaskName = new ExtMonVariable<string>(this,
                    @"Task name", @"Name",
                    @"The name of the task as it appears in GUI.",
                    1));

                Children.Add(
                    TaskType = new ExtMonVariable<string>(this,
                    @"Task type", "Type",
                    @"The type of the task (copy, extract, report, etc.).",
                    2));

                Children.Add(
                    Success = new ExtMonVariable<bool>(this, 
                    @"Success", @"Success", 
                    @"Whether or not the last executed task was completed successfully, i.e. without errors. " +
                    @"For Condition task this means that the expression was successfully evaluated as " +
                    @"TRUE or FALSE - both results are treated as success.",
                    3));

                Children.Add(
                    DurationOfLastExecutionInSec = new ExtMonVariable<uint>(this,
                    @"Duration of last execution", @"DurationOfLastExecution",
                    @"Duration of the last task execution (in seconds).",
                    4));

                Children.Add(
                    MemoryUsedForLastExecutionInMb = new ExtMonVariable<uint>(this,
                    @"Memory used for last execution", @"LastMemoryUsed",
                    @"Amount of memory used during the last execution of the task (in megabytes). " +
                    @"This is applicable only to tasks that use ibaAnalyzer for their processing e.g., " +
                    @"Condition, Report, Extract and some custom tasks.",
                    5));

                // set default values
                Reset();
            }

            // create Cleanup folder and add it to collection
            public LocalCleanupInfo AddCleanupInfo()
            {
                Children.Add(CleanupInfo = new LocalCleanupInfo(this, 6));
                return CleanupInfo;
            }
            public void ResetCleanupInfo()
            {
                Children.Remove(CleanupInfo);
                CleanupInfo = null;
            }

            /// <summary> Resets to default values everything except Oid and Parent </summary>
            public void Reset()
            {
                TaskName.Value = "";
                TaskType.Value = "";
                Success.Value = false;
                DurationOfLastExecutionInSec.Value = 0;
                MemoryUsedForLastExecutionInMb.Value = 0;
                ResetCleanupInfo();
            }

            public override string ToString()
            {
                return $@"{TaskName.Value} [{TaskType.Value}, {SnmpFullOid}, {ParentJob.JobName.Value}]";
            }
        }
        

        internal class LocalCleanupInfo : ExtMonFolder
        {
            /// <summary> Oid 1 </summary>
            public readonly ExtMonVariable<TaskWithTargetDirData.OutputLimitChoiceEnum> LimitChoice;
            /// <summary> Oid 2 </summary>
            public readonly ExtMonVariable<uint> Subdirectories;
            /// <summary> Oid 3 </summary>
            public readonly ExtMonVariable<uint> UsedDiskSpace;
            /// <summary> Oid 4 </summary>
            public readonly ExtMonVariable<uint> FreeDiskSpace;

            public LocalCleanupInfo(ExtMonFolder parent, uint snmpLeastId)
                : base(parent, snmpLeastId, @"Cleanup", @"Cleanup",
                    @"Cleanup parameters of the task.")
            {
                // create variables and add them to collection

                Children.Add(
                    LimitChoice = new ExtMonVariable<TaskWithTargetDirData.OutputLimitChoiceEnum>(this,
                        @"Limit choice", @"LimitChoice",
                        @"Option selected as limit for the disk space usage. " +
                        @"(0 = None, 1 = Maximum subdirectories, 2 = Maximum used disk space, 3 = Minimum free disk space).",
                    1));

                Children.Add(
                    Subdirectories = new ExtMonVariable<uint>(this,
                        @"Subdirectories", @"Subdirectories",
                        @"Maximum count of directories the task can use.",
                    2));

                Children.Add(
                    UsedDiskSpace = new ExtMonVariable<uint>(this,
                        @"Used disk space", @"UsedDiskSpace",
                        @"Maximum disk space that can be used by the task (in megabytes).",
                    3));

                Children.Add(
                    FreeDiskSpace = new ExtMonVariable<uint>(this,
                        @"Free disk space", @"FreeDiskSpace",
                        @"Minimum disk space that is kept free (in megabytes).",
                    4));
            }
        }

        #endregion


        #region Jobs

        internal enum JobStatus
        {
            Disabled = 0,
            Started = 1,
            Stopped = 2
        }


        /// <summary> OID ...2 Standard Jobs </summary>
        internal abstract class JobInfoBase : ExtMonGroup
        {
            /// <summary> key of the job list </summary>
            public Guid Guid;

            /// <summary> Oid 1 </summary> 
            public ExtMonFolder FolderGeneral;
            /// <summary> Oid 2 </summary> 
            public ExtMonFolder FolderTasks;

            /// <summary> Oid General.1 </summary>  //todo add ua names to comments
            public readonly ExtMonVariable<string> JobName;
            /// <summary> Oid General.2 </summary>
            public readonly ExtMonVariable<JobStatus> Status;
            /// <summary> Oid General.3 </summary>
            public readonly ExtMonVariable<uint> TodoCount;
            /// <summary> Oid General.4 </summary>
            public readonly ExtMonVariable<uint> DoneCount;
            /// <summary> Oid General.5 </summary>
            public readonly ExtMonVariable<uint> FailedCount;

            /// <summary> Oid Tasks.1...Tasks.n, where n - size of the list</summary>
            public List<TaskInfo> Tasks; // todo. kls. encapsulate FolderTasks?


            protected JobInfoBase(ExtMonFolder parent, uint snmpLeastId, string jobName) 
                : base(parent, snmpLeastId)
            {
                Caption = $@"Job '{jobName}'";
                // SnmpFullMibName = ""; // is set in derived classes
                // Description = ""; // is set in derived classes


                // create folders and add them to collection

                // todo. kls. desc on Job rename (this was NOT implemented in original version also) 
                Children.Add(
                    FolderGeneral = new ExtMonFolder(this, 1,
                    @"General", @"General",
                    $@"General properties of job '{jobName}'.")); 

                Children.Add(
                    FolderTasks = new ExtMonFolder(this, 2,
                    @"Tasks", @"Tasks", 
                    $@"Information about all tasks of the job '{jobName}'."));

                // create variables and add them to collection

                FolderGeneral.Children.Add(
                    JobName = new ExtMonVariable<string>(FolderGeneral,
                    @"Job Name", @"Name",
                    @"The name of the job as it appears in GUI.",
                    1));

                FolderGeneral.Children.Add(
                    Status = new ExtMonVariable<JobStatus>(FolderGeneral,
                    @"Status", @"Status", 
                    @"Current status of the job (started, stopped or disabled).",
                    2));

                FolderGeneral.Children.Add(
                    TodoCount = new ExtMonVariable<uint>(FolderGeneral,
                    @"Todo #", @"Todo",
                    @"Number of dat files to be processed.",
                    3));

                FolderGeneral.Children.Add(
                    DoneCount = new ExtMonVariable<uint>(FolderGeneral,
                    @"Done #", @"Done",
                    @"Number of processed files.",
                    4));

                FolderGeneral.Children.Add(
                    FailedCount = new ExtMonVariable<uint>(FolderGeneral,
                    @"Failed #", @"Failed",
                    @"Number of errors occurred during processing.",
                    5));

                PrivateReset();
            }

            private void PrivateReset()
            {
                JobName.Value = "";
                Status.Value = JobStatus.Disabled;
                TodoCount.Value = 0;
                DoneCount.Value = 0;
                FailedCount.Value = 0;
            }

            /// <summary> Resets to default values everything except Guid (primary key) and Tasks </summary>
            public virtual void Reset()
            {
                PrivateReset();
            }

            // todo. kls. 
            public void ResetTasks()
            {
            }

            public override string ToString()
            {
                string tasksString = (Tasks?.Count ?? 0).ToString();
                return $@"{JobName.Value} [{Status}, {TodoCount}/{DoneCount}/{FailedCount}, T:{tasksString}]";
            }
        }


        internal class StandardJobInfo : JobInfoBase
        {
            /// <summary> Oid 10 </summary>
            public ExtMonFolder FolderLastProcessing;

            /// <summary> Oid 6 </summary>
            public readonly ExtMonVariable<uint> PermFailedCount;
            /// <summary> Oid 7 </summary>
            public readonly ExtMonVariable<DateTime> TimestampJobStarted;
            /// <summary> Oid 8 </summary>
            public readonly ExtMonVariable<DateTime> TimestampLastDirectoryScan;
            /// <summary> Oid 9 </summary>
            public readonly ExtMonVariable<DateTime> TimestampLastReprocessErrorsScan;

            /// <summary> Oid 10.1 </summary>
            public readonly ExtMonVariable<string> LastProcessingLastDatFileProcessed;
            /// <summary> Oid 10.2 </summary>
            public readonly ExtMonVariable<DateTime> LastProcessingStartTimeStamp;
            /// <summary> Oid 10.3 </summary>
            public readonly ExtMonVariable<DateTime> LastProcessingFinishTimeStamp;

            public StandardJobInfo(ExtMonFolder parent, uint snmpLeastId, string jobName) 
                : base(parent, snmpLeastId, jobName)
            {
                // set Description and MIB. (Caption is set in the base class)
                SnmpFullMibName = $@"standardJob{snmpLeastId}";
                Description = $@"Properties of standard job '{jobName}'.";
                
                // create variables and add them to collection

                FolderGeneral.Children.Add(
                    PermFailedCount = new ExtMonVariable<uint>(FolderGeneral,
                    @"Perm. Failed #", @"PermFailed",
                    @"Number of files with persistent errors.",
                    6));

                FolderGeneral.Children.Add(
                    TimestampJobStarted = new ExtMonVariable<DateTime>(FolderGeneral,
                    @"Timestamp job started", @"TimestampJobStarted",
                    @"Time when the job was started. For a stopped job, it relates to the last start of the job. " +
                    @"If job was never started, then value is '01.01.0001 0:00:00'.",
                    7));

                FolderGeneral.Children.Add(
                    TimestampLastDirectoryScan = new ExtMonVariable<DateTime>(FolderGeneral,
                    @"Timestamp last directory scan", @"TimestampLastDirectoryScan",
                    @"Time when the last scan for new (unprocessed) .dat files was performed. " +
                    @"If scan was never performed, then value is '01.01.0001 0:00:00'.",
                    8));

                FolderGeneral.Children.Add(
                    TimestampLastReprocessErrorsScan = new ExtMonVariable<DateTime>(FolderGeneral,
                    @"Timestamp last reprocess errors scan", @"TimestampLastReprocessErrorsScan",
                    @"Time when the last reprocess scan was performed " +
                    @"(reprocess scan is a scan for .dat files that previously were processed with errors).  " +
                    @"If scan was never performed, then value is '01.01.0001 0:00:00'.",
                    9));

                // last processing Folder
                Children.Add(
                    FolderLastProcessing = new ExtMonFolder(FolderGeneral, 10,
                    @"LastProcessing", @"LastProcessing",
                    @"Information about the last successfully processed file."));

                // last processing

                FolderLastProcessing.Children.Add(
                    LastProcessingLastDatFileProcessed = new ExtMonVariable<string>(FolderLastProcessing,
                    @"Last dat-file processed", @"LastFile", // todo. kls. derive mib name from job, not from last proc
                    @"Filename of the last successfully processed file. If no files were successfully processed, then value is empty.",
                    1));

                FolderLastProcessing.Children.Add(
                    LastProcessingStartTimeStamp = new ExtMonVariable<DateTime>(FolderLastProcessing,
                    @"Start timestamp", @"StartStamp", // todo. kls. derive mib name from job, not from last proc
                    @"Time when processing of the last successfully processed file was started. " +
                    @"If no files were successfully processed, then value is '01.01.0001 0:00:00'.",
                    2));

                FolderLastProcessing.Children.Add(
                    LastProcessingFinishTimeStamp = new ExtMonVariable<DateTime>(FolderLastProcessing,
                    @"Finish timestamp", @"FinishStamp", // todo. kls. derive mib name from job, not from last proc
                    @"Time when processing of the last successfully processed file was finished. " +
                    @"If no files were successfully processed, then value is '01.01.0001 0:00:00'.",
                    3));

                PrivateReset();
            }

            private void PrivateReset()
            {
                PermFailedCount.Value = 0;
                TimestampJobStarted.Value = DateTime.MinValue;
                TimestampLastDirectoryScan.Value = DateTime.MinValue;
                TimestampLastReprocessErrorsScan.Value = DateTime.MinValue;
                LastProcessingLastDatFileProcessed.Value = "";
                LastProcessingStartTimeStamp.Value = DateTime.MinValue;
                LastProcessingFinishTimeStamp.Value = DateTime.MinValue;
            }

            public override void Reset()
            {
                base.Reset();
                PrivateReset();
            }
        }


        internal class ScheduledJobInfo : JobInfoBase
        {
            /// <summary> Oid 6 </summary>
            public readonly ExtMonVariable<uint> PermFailedCount;
            /// <summary> Oid 7 </summary>
            public readonly ExtMonVariable<DateTime> TimestampJobStarted;
            /// <summary> Oid 8 </summary>
            public readonly ExtMonVariable<DateTime> TimestampLastExecution;
            /// <summary> Oid 9 </summary>
            public readonly ExtMonVariable<DateTime> TimestampNextExecution;

            public ScheduledJobInfo(ExtMonFolder parent, uint snmpLeastId, string jobName)
                : base(parent, snmpLeastId, jobName)
            {
                // set Description and MIB. (Caption is set in the base class)
                SnmpFullMibName = $@"scheduledJob{snmpLeastId}";
                Description = $@"Properties of scheduled job '{jobName}'.";

                // create variables and add them to collection

                FolderGeneral.Children.Add(
                    PermFailedCount = new ExtMonVariable<uint>(FolderGeneral,
                        @"Perm. Failed #", @"PermFailedCount",
                        @"Number of files with persistent errors.",
                        6));

                FolderGeneral.Children.Add(
                    TimestampJobStarted = new ExtMonVariable<DateTime>(FolderGeneral,
                        @"Timestamp job started", @"TimestampJobStarted",
                        @"Time when job was started (starting of the scheduled job does NOT mean that it will be executed immediately).  " +
                        @"For a stopped job, it relates to the last start of the job.  " +
                        @"If job was never started, then value is '01.01.0001 0:00:00'.",
                        7));

                FolderGeneral.Children.Add(
                    TimestampLastExecution = new ExtMonVariable<DateTime>(FolderGeneral,
                        @"Timestamp last execution", @"TimestampLastExecution",
                        @"Time when the job was last executed. " +
                        @"(This does not mean the moment when job was started, but the moment when configured trigger was fired last time); " +
                        @"If job was never executed, then value is '01.01.0001 0:00:00'.",
                        8));

                FolderGeneral.Children.Add(
                    TimestampNextExecution = new ExtMonVariable<DateTime>(FolderGeneral,
                        @"Timestamp next execution", @"TimestampNextExecution",
                        @"Time of the next scheduled execution. " +
                        @"If there is no execution scheduled, then value is '01.01.0001 0:00:00'.",
                        9));

                PrivateReset();
            }

            private void PrivateReset()
            {
                PermFailedCount.Value = 0;
                TimestampJobStarted.Value = DateTime.MinValue;
                TimestampLastExecution.Value = DateTime.MinValue;
                TimestampNextExecution.Value = DateTime.MinValue;
            }

            public override void Reset()
            {
                base.Reset();
                PrivateReset();
            }
        }
        

        internal class OneTimeJobInfo : JobInfoBase
        {
            /// <summary> Oid 6 </summary>
            public readonly ExtMonVariable<DateTime> TimestampLastExecution;

            public OneTimeJobInfo(ExtMonFolder parent, uint snmpLeastId, string jobName)
                : base(parent, snmpLeastId, jobName)
            {
                // set Description and MIB. (Caption is set in the base class)
                SnmpFullMibName = $@"oneTimeJob{snmpLeastId}";
                Description = $@"Properties of one-time job '{jobName}'.";

                // create variables and add them to collection

                FolderGeneral.Children.Add(
                    TimestampLastExecution = new ExtMonVariable<DateTime>(FolderGeneral,
                        @"Timestamp last execution", @"TimestampLastExecution",
                        @"Time when the last execution was started. " +
                        @"If job was never executed, then value is '01.01.0001 0:00:00'.",
                        6));


                PrivateReset();
            }

            private void PrivateReset()
            {
                TimestampLastExecution.Value = DateTime.MinValue;
            }

            public override void Reset()
            {
                base.Reset();
                PrivateReset();
            }
        }


        internal class EventBasedJobInfo : JobInfoBase
        {
            /// <summary> Oid 6 </summary>
            public readonly ExtMonVariable<uint> PermFailedCount;
            /// <summary> Oid 7 </summary>
            public readonly ExtMonVariable<DateTime> TimestampJobStarted;
            /// <summary> Oid 8 </summary>
            public readonly ExtMonVariable<DateTime> TimestampLastExecution;

            public EventBasedJobInfo(ExtMonFolder parent, uint snmpLeastId, string jobName)
                : base(parent, snmpLeastId, jobName)
            {
                // set Description and MIB. (Caption is set in base class)
                SnmpFullMibName = $@"eventJob{snmpLeastId}";
                Description = $@"Properties of event job '{jobName}'.";

                // create variables and add them to collection

                FolderGeneral.Children.Add(
                    PermFailedCount = new ExtMonVariable<uint>(FolderGeneral,
                        @"Perm. Failed #", @"PermFailed",
                        @"Number of files with persistent errors.",
                        6));

                FolderGeneral.Children.Add(
                    TimestampJobStarted = new ExtMonVariable<DateTime>(FolderGeneral,
                        @"Timestamp job started", @"TimestampJobStarted",
                        @"Time when job was started (starting of the event job does NOT mean that it will be executed immediately).  " +
                        @"For a stopped job, it relates to the last start of the job.  " +
                        @"If job was never started, then value is '01.01.0001 0:00:00'.",
                        7));

                FolderGeneral.Children.Add(
                    TimestampLastExecution = new ExtMonVariable<DateTime>(FolderGeneral,
                        @"Timestamp last execution", @"TimestampLastExecution",
                        @"Time when the job was last executed. " +
                        @"(This does not mean the moment when job was started, but the last occurrence of a monitored event); " +
                        @"If job was never executed, then value is '01.01.0001 0:00:00'.",
                        8));

                PrivateReset();
            }

            private void PrivateReset()
            {
                PermFailedCount.Value = 0;
                TimestampJobStarted.Value = DateTime.MinValue;
                TimestampLastExecution.Value = DateTime.MinValue;
            }

            public override void Reset()
            {
                base.Reset();
                PrivateReset();
            }
        }


        #endregion


        #endregion
    }
}
