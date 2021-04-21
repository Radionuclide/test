﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using iba.Logging;
using iba.Processing;
using iba.Processing.IbaOpcUa;
using iba.Utility;
using IbaSnmpLib;
using Timer = System.Timers.Timer;

namespace iba.Data
{
    /// <summary>
    /// External Monitoring Data.
    /// Contains all data that can be monitored from outside of DatCoordinator - via SNMP or OPC UA.
    /// Is used as a middle layer between: TaskManager on the one side
    /// and SnmpWorker or OpcUaWorker on the other side.
    /// </summary>
    public class ExtMonData 
    {
        #region Constants / settings / tuning

        /// <summary> Recommended timeout for trying to acquire
        /// a lock to <see cref="LockObject"/> </summary>
        public static int LockTimeout { get; } = 100;

        /// <summary> Minimal possible AgeThreshold for any group.
        /// AgeThreshold - is a measure to tell whether some data is fresh enough (and can be used as is) or
        /// is probably too old (and should be updated before sending via SNMP or OPC UA).
        /// From point of view of a user it's like a sampling rate </summary>
        public static TimeSpan MinimalAgeThreshold { get; set; } = TimeSpan.FromSeconds(0.5);

        /// <summary> AgeThreshold (sampling rate) for all values within any Job;
        /// It's rather easy (computationally) to update a job info
        /// and job's variables can be changing quickly,
        /// so the value can be set to a reasonable minimum. </summary>
        public static TimeSpan JobAgeThreshold { get; set; } = MinimalAgeThreshold;

        /// <summary> AgeThreshold (sampling rate) for all values within <see cref="LicenseInfo"/> group;
        /// Dongle usually is not plugged/unplugged too often,
        /// and retrieving of dongle information is rather time-consuming procedure,
        /// so this value is recommended NOT to be too small </summary>
        public static TimeSpan LicenseAgeThreshold { get; set; } = TimeSpan.FromSeconds(10);

        /// <summary> AgeThreshold (sampling rate) for all values within any <see cref="GlobalCleanupDriveInfo"/> group.
        /// Drive info variables usually do not change too often,
        /// so this value can be considerably bigger than the minimal value</summary>
        public static TimeSpan DriveInfoAgeThreshold { get; set; } = TimeSpan.FromSeconds(3); 

        #endregion
        
        
        #region Static / Singleton

        // ReSharper disable once InconsistentNaming
        private static readonly Lazy<ExtMonData> _lazyInstance = new Lazy<ExtMonData>();
        public static ExtMonData Instance => _lazyInstance.Value;

        #endregion


        #region Body

        #region Fields and Props

        public readonly object LockObject = new object();

        /// <summary> Supply this value to AddChildXxx() functions to set snmpLeastId automatically. </summary>
        public const uint SNMP_AUTO_LEAST_ID = uint.MaxValue;

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

        /// <summary> SNMP: PrSpecific.100 </summary>
        public readonly ExtMonFolder FolderComputedValues;

        #endregion


        #region Construction and Reset

        public ExtMonData()
        {
            InitializeTreeValidator();

            CreateEnumDescriptions();

            FolderRoot = new ExtMonFolder(null,
                @"ExtMonDataRoot", @"ExtMonDataRoot", @"ExtMonDataRoot", 0)
            {
                UaBrowseName = "IbaDatCoordinator"
            };
            // FolderRoot.SnmpFullMibName = ; // is not used
            // FolderRoot.SnmpFullOid = ; // is not used

            FolderRoot.Children.Add(License = new LicenseInfo(FolderRoot));

            FolderRoot.Children.Add(
                FolderGlobalCleanup = new ExtMonFolder(FolderRoot,
                @"Global cleanup", @"globalCleanup", @"GlobalCleanup",
                "Global cleanup settings for all local drives.", new IbaSnmpOid(1)));

            FolderRoot.Children.Add(
                FolderStandardJobs = new ExtMonFolder(FolderRoot,
                @"Standard jobs", @"standardJobs", @"StandardJobs",
                @"List of all standard jobs.", new IbaSnmpOid(2)));

            FolderRoot.Children.Add(
                FolderScheduledJobs = new ExtMonFolder(FolderRoot,
                @"Scheduled jobs", @"scheduledJobs", @"ScheduledJobs",
                @"List of all scheduled jobs.", new IbaSnmpOid(3)));

            FolderRoot.Children.Add(
                FolderOneTimeJobs = new ExtMonFolder(FolderRoot,
                @"One time jobs", @"oneTimeJobs", @"OneTimeJobs",
                @"List of all one-time jobs.", new IbaSnmpOid(4)));

            FolderRoot.Children.Add(
                FolderEventBasedJobs = new ExtMonFolder(FolderRoot,
                @"Event jobs", @"eventJobs", @"EventJobs",
                @"List of all event jobs.", new IbaSnmpOid(5)));

            FolderRoot.Children.Add(
                FolderComputedValues = new ExtMonFolder(FolderRoot,
                @"Computed values", @"computedValues", @"ComputedValues",
                @"List of all computed values.", new IbaSnmpOid(100)));
        }

        public void Reset()
        {
            License.Reset();

            FolderGlobalCleanup.ClearChildren();
            FolderStandardJobs.ClearChildren();
            FolderScheduledJobs.ClearChildren();
            FolderOneTimeJobs.ClearChildren();
            FolderEventBasedJobs.ClearChildren();
            FolderComputedValues.ClearChildren();
        }

        #endregion


        #region Tree Validity

        public event EventHandler<EventArgs> ExtMonStructureChanged;

        /// <summary> Backing field for <see cref="IsStructureValid"/> </summary>
        private bool _isStructureValid;

        /// <summary>
        /// Whether <see cref="ExtMonData"/> structure is relevant to the current structure in <see cref="TaskManager"/>.
        /// It is set to false every time when structure
        /// is changed in <see cref="TaskManager"/> (see <see cref="TaskManager.ExtMonConfigurationChanged"/>).
        /// It is set to true after each successful tree rebuild (see <see cref="RebuildTree"/>).
        /// </summary>
        public bool IsStructureValid
        {
            get => _isStructureValid;
            set
            {
                // this implementation works properly if called from different threads;
                // lock of the timer is not needed here
                _isStructureValid = value;

                // stop current cycle
                _treeValidatorTimer?.Stop();

                // if structure is marked invalid
                if (!value)
                {
                    // schedule a delayed tree rebuild, 
                    // if it will not happen earlier
                    _treeValidatorTimer?.Start();
                }
            }
        }

        /// <summary>
        /// The reason for having this timer is a compromise between responsiveness, reliability and computational efforts.
        /// Without this timer we had to rebuild the tree on any
        /// firing of <see cref="TaskManager.ExtMonConfigurationChanged"/> event.
        /// With this timer it's guaranteed that:
        ///  1. Tree will not be rebuilt too often
        ///  2. Tree will be rebuilt not later than after a certain time
        /// </summary>
        private readonly Timer _treeValidatorTimer = new Timer {Enabled = false};

        private void InitializeTreeValidator()
        {
            TaskManager.Manager.ExtMonConfigurationChanged += (sender, args) =>
            {
                // we do not need to lock something here
                // it's not a problem if structure is invalidated during rebuild is in progress

                // mark tree as invalid
                // it will be rebuilt either:
                //  * on first request to any existing node
                //  * or automatically on tick of _treeValidatorTimer
                IsStructureValid = false;
            };

            // create the timer for delayed tree rebuild
            _treeValidatorTimer.Interval = MinimalAgeThreshold.TotalMilliseconds;
            _treeValidatorTimer.AutoReset = false;
            _treeValidatorTimer.Enabled = true;
            _treeValidatorTimer.Elapsed += (sender, args) =>
            {
                DebugWriteLine(nameof(ExtMonData), $"{nameof(_treeValidatorTimer)}.Elapsed");
                RebuildTreeIfItIsInvalid();

                // best option to test why timer is needed (additionally to rebuild on first request):
                // 1. setup SNMP manager to monitor some yet non-existent job. it will show "no such instance". ok.
                // 2. Add one or several jobs to fit the requested OID area. 
                //  If there were no such timer, tree would be invalidated but not rebuilt.
                //  Manager would still show "no such instance", though OID is really present.
            };
        }

        /// <summary>
        /// Rebuilds a tree completely if its <see cref="IsStructureValid"/> flag is set to false. 
        /// Use returned value to know whether tree has been rebuilt.
        /// </summary>
        /// <returns> <value>true</value> if tree was rebuilt, 
        /// <value>false</value> if it is valid and has not been modified by this call.</returns>
        public bool RebuildTreeIfItIsInvalid()
        {
            // here I use a multiple of the normal timeout to give an overwhelming priority over request-item locks
            if (Monitor.TryEnter(LockObject, LockTimeout * 20))
            {
                try
                {
                    if (IsStructureValid)
                    {
                        return false; // tree structure has not changed
                    }
                    RebuildTree();
                    return true; // tree structure has changed
                }
                finally
                {
                    Monitor.Exit(LockObject);
                }
            }
            // ReSharper disable once RedundantIfElseBlock
            else
            {
                // failed to acquire a lock
                try
                {
                    DebugWriteLine(nameof(ExtMonData), "WARNING! Failed to acquire a lock when checking whether tree is valid");
                    // 'Level.Warning' because we have a big timeout here, so, normally it should not happen 
                    // (though not critical, but may indicate that something is wrong)
                    LogData.Data.Logger.Log(Level.Warning,
                    $"{nameof(ExtMonData)}. Failed to acquire a lock when checking whether tree is valid.");
                }
                catch { /* logging is not critical */ }

                return false; // tree structure has not changed
            }
        }

        public bool RebuildTree()
        {
            var man = TaskManager.Manager;
            if (man == null)
            {
                return false; // rebuild failed
            }

            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    // obj structure is valid until datCoordinator configuration is changed.
                    // theoretically it can be reset to false by another thread
                    // during the process of rebuild of ExtMonData,
                    // but it's not a problem. 
                    // If this happens, then the tree will be rebuilt once again.
                    // this is better than to lock resetting of IsStructureValid (and consequently have potential risk of a deadlock).
                    IsStructureValid = true;

                    bool bSuccess = man.ExtMonRebuildObjectsData();

                    try
                    {
                        ExtMonStructureChanged?.Invoke(this, EventArgs.Empty);
                    }
                    catch
                    {
                        LogData.Data.Logger.Log(Level.Debug,
                            $"{nameof(ExtMonData)}.{nameof(RebuildTree)}. Error triggering event {nameof(ExtMonStructureChanged)}.");
                    }

                    // fire an event to let SNMP worker and UA worker know that 
                    // structure was changed and that they also need to rebuild their trees accordingly
                    return bSuccess;
                }
                finally
                {
                    Monitor.Exit(LockObject);
                }
            }

            // failed to acquire a lock
            try
            {
                // Level.Exception because normally the function is called only from RebuildTreeIfItIsInvalid(),
                // so lock should ALWAYS be ours at this point;
                // if we are here, then something is wrong
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(ExtMonData)}.{nameof(RebuildTree)}. Failed to acquire a lock when rebuilding the tree.");
            }
            catch { /* logging is not critical */ }

            // should not happen (see comments above)
            Debug.Assert(false);

            return false; // rebuild failed
        }

        #endregion


        #region AddNewXxx() and auxiliary

        public GlobalCleanupDriveInfo AddNewGlobalCleanup(string driveName)
        {
            var driveInfo = new GlobalCleanupDriveInfo(
                FolderGlobalCleanup, (uint) FolderGlobalCleanup.Children.Count + 1, driveName)
            {
                UaBrowseName = $@"Drive_{driveName.Substring(0, 1)}:"
            };
            FolderGlobalCleanup.Children.Add(driveInfo);
            // check consistency between mib name and oid
            Debug.Assert(driveInfo.SnmpFullMibName.Contains($"Drive{driveInfo.SnmpLeastId}"));
            return driveInfo;
        }

        public JobInfoBase AddNewJob(ConfigurationData.JobTypeEnum jobType, string jobName, Guid guid)
        {
            JobInfoBase jobInfo;
            ExtMonFolder folder;
            uint indexWithinFolder;

            switch (jobType)
            {
                case ConfigurationData.JobTypeEnum.DatTriggered: // standard Job
                    folder = FolderStandardJobs;
                    indexWithinFolder = (uint) folder.Children.Count + 1;
                    jobInfo = new StandardJobInfo(folder, indexWithinFolder, jobName);
                    break;

                case ConfigurationData.JobTypeEnum.Scheduled:
                    folder = FolderScheduledJobs;
                    indexWithinFolder = (uint)folder.Children.Count + 1;
                    jobInfo = new ScheduledJobInfo(folder, indexWithinFolder, jobName);
                    break;

                case ConfigurationData.JobTypeEnum.OneTime:
                    folder = FolderOneTimeJobs;
                    indexWithinFolder = (uint)folder.Children.Count + 1;
                    jobInfo = new OneTimeJobInfo(folder, indexWithinFolder, jobName);
                    break;

                case ConfigurationData.JobTypeEnum.Event:
                    folder = FolderEventBasedJobs;
                    indexWithinFolder = (uint)folder.Children.Count + 1;
                    jobInfo = new EventBasedJobInfo(folder, indexWithinFolder, jobName);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
            jobInfo.Guid = guid;
            jobInfo.UaBrowseName = $@"Job{{{guid}}}";
            // jobInfo.UaBrowseName = $@"Job{indexWithinFolder}"; // uncomment this line to use index instead of Guid
            folder.Children.Add(jobInfo);
            // check consistency between mib name and oid
            Debug.Assert(jobInfo.SnmpLeastId == indexWithinFolder);
            Debug.Assert(jobInfo.SnmpFullMibName.Contains($"Job{jobInfo.SnmpLeastId}"));
            return jobInfo;
        }

        public ExtMonFolder AddNewComputedValueJob(string jobName, Guid guid)
        {
            ExtMonFolder parentFolder = Instance.FolderComputedValues; // all computed value nodes are grouped here
            var indexWithinFolder = (uint)parentFolder.Children.Count + 1; // is used for SNMP

            var jobFolder = new ExtMonFolder(
                parentFolder,
                jobName,
                $@"Job{indexWithinFolder}",
                $@"Computed values of the job '{jobName}'",
                indexWithinFolder)
            {
                UaBrowseName = $@"Job{{{guid}}}"
            };

            parentFolder.Children.Add(jobFolder);

            return jobFolder;
        }

        public ComputedValuesInfo AddNewComputedValueTask(ExtMonFolder parentJobFolder, OpcUaWriterTaskData computedValueTaskData)
        {
            var indexWithinFolder = (uint)parentJobFolder.Children.Count + 1; // is used for SNMP
            var taskInfo = new ComputedValuesInfo(parentJobFolder, indexWithinFolder, computedValueTaskData);
            parentJobFolder.Children.Add(taskInfo);
            return taskInfo;
        }

        public List<ExtMonNode> GetFlatListOfAllChildren() => FolderRoot.GetFlatListOfAllChildren();

        public bool CheckChildParentConsistency(ExtMonFolder folder)
        {
            foreach (var node in folder.Children)
            {
                if (node.Parent != folder)
                    return false;

                // ReSharper disable once InvertIf
                if (node is ExtMonFolder xmFolder)
                {
                    if (!CheckChildParentConsistency(xmFolder))
                        return false;
                }
            }
            return true; // consistency is ok
        }

        public bool CheckConsistency()
        {
            // Check tree structure (parent-child) consistency
            Debug.Assert(CheckChildParentConsistency(FolderRoot));

            // get list of all nodes
            var children = GetFlatListOfAllChildren();

            Set<IbaSnmpOid> oids = new Set<IbaSnmpOid>();
            Set<string> mibNames = new Set<string>();
            Set<string> uaPaths = new Set<string>();

            foreach (var child in children)
            {
                var oid = child.SnmpFullOid;

                // OID should not end with 0
                if (oid == null || oid.GetLeastSignificantSubId() == 0)
                    return false;
                // check OIDs uniqueness
                if (oids.Contains(oid))
                    return false;
                oids.Add(oid);

                var mibName = child.SnmpFullMibName;
                // mib name should not be null or whitespace
                if (string.IsNullOrWhiteSpace(mibName))
                    return false;
                // check mib name uniqueness
                if (mibNames.Contains(mibName))
                    return false;
                mibNames.Add(mibName);

                var uaPath = child.UaFullPath;
                // should not be null or whitespace
                if (string.IsNullOrWhiteSpace(uaPath))
                    return false;
                // check uniqueness
                if (uaPaths.Contains(uaPath))
                    return false;
                uaPaths.Add(uaPath);
            }
            return true;
        }

        public static string GetCurrentThreadString()
        {
            var thr = Thread.CurrentThread;
            string thrNameOrId = String.IsNullOrWhiteSpace(thr.Name) ? thr.ManagedThreadId.ToString() : thr.Name;
            return $"thr=[{thrNameOrId}]";
        }

        public static void DebugWriteLine(string sender, string msg)
        {
            Debug.WriteLine($"{DateTime.Now}. {GetCurrentThreadString()}. {sender}. {msg}");
        }

        #endregion

        #endregion


        #region Nested classes


        #region Abstract and common classes

        /// <summary> Node is a basic element of <see cref="ExtMonData"/> hierarchy.
        /// It is either folder or variable. </summary>
        public abstract class ExtMonNode
        {
            public readonly ExtMonFolder Parent;

            public string Caption; // todo. kls. low priority. try make readonly 
            public string Description; // todo. kls. low priority. try make readonly 

            /// <summary> Least significant (rightmost) subId of SNMP OID for corresponding object </summary>
            public readonly uint SnmpLeastId;

            /// <summary> Backing field for <see cref="SnmpFullOid"/> </summary>
            public IbaSnmpOid _snmpFullOid;

            /// <summary> Full SNMP OID.
            /// Can be set directly or can be evaluated dynamically from SnmpLeastId and parent's full OID. </summary>
            public IbaSnmpOid SnmpFullOid
            {
                get
                {
                    if (_snmpFullOid == null)
                    {
                        // calculate (and keep) full OID on first request
                        _snmpFullOid = Parent == null || Parent.SnmpFullOid == null
                            ? null
                            : Parent.SnmpFullOid + SnmpLeastId;
                    }

                    Debug.Assert(SnmpLeastId == 0 || _snmpFullOid == null || SnmpLeastId == _snmpFullOid.GetLeastSignificantSubId());
                    return _snmpFullOid;
                }
                set => _snmpFullOid = value;
            }

            /// <summary> Rightmost piece of the MIB name;
            /// e.g. "Status" for the item "standardJob3Task2Status" </summary>
            public readonly string SnmpMibNameSuffix;

            /// <summary> Backing field for <see cref="SnmpFullMibName"/> </summary>
            private string _snmpFullMibName;

            /// <summary> Full SNMP MIB name. 
            /// Can be set directly or can be evaluated dynamically from MibSuffix and parent's FullMibName </summary>
            public string SnmpFullMibName
            {
                get
                {
                    if (_snmpFullMibName == null)
                    {
                        // calculate (and keep) full MIB name on first request
                        _snmpFullMibName = Parent?.SnmpFullMibName == null
                            ? null
                            : Parent.SnmpFullMibName + SnmpMibNameSuffix;

                    }
                    Debug.Assert( _snmpFullMibName != null);
                    Debug.Assert(SnmpMibNameSuffix == null || _snmpFullMibName.Contains(SnmpMibNameSuffix));
                    return _snmpFullMibName;
                }
                set => _snmpFullMibName = value;
            }
            

            /// <summary> Backing field for <see cref="UaBrowseName"/> </summary>
            private string _uaBrowseName;
            /// <summary> Normally, in most cases UaBrowseName is
            /// equal to <see cref="SnmpMibNameSuffix"/>; but it can be set separately. </summary>
            public string UaBrowseName
            {
                get => _uaBrowseName ?? SnmpMibNameSuffix ?? SnmpFullMibName;
                set => _uaBrowseName = value;
            }

            /// <summary> Backing field for <see cref="UaFullPath"/> </summary>
            private string _uaFullPath;

            /// <summary> Full OPC UA NodeId (path) of the node in UA address space</summary>
            public string UaFullPath
            {
                get
                {
                    if (_uaFullPath == null)
                    {
                        // calculate (and keep) it first request
                        _uaFullPath = Parent == null
                            ? UaBrowseName
                            : IbaOpcUaNodeManager.ComposeNodeId(Parent.UaFullPath, UaBrowseName);
                    }
                    Debug.Assert(_uaFullPath != null);
                    Debug.Assert(_uaFullPath.Contains(UaBrowseName));
                    return _uaFullPath;
                }
            }

            public ExtMonGroup GetGroup()
            {
                switch (Parent)
                {
                    case null:
                        return null;
                    case ExtMonGroup group:
                        return group;
                    default:
                        return Parent.GetGroup();
                }
            }

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
                try
                {
                    return $@"Node '{Caption}', OID={SnmpFullOid}, MIB='{SnmpFullMibName}'";
                }
                catch /* paranoic */
                {
                    Debug.Assert(false);
                    return base.ToString();
                }
            }
        }

        public abstract class ExtMonVariableBase : ExtMonNode
        {
            /// <summary> Gets contained data as untyped <see cref="object"/> </summary>
            public abstract object ObjValue { get; }

            /// <summary> The <see cref="Group"/> a variable belongs to </summary>
            public readonly ExtMonGroup Group;

            public object handler;
            protected ExtMonVariableBase(ExtMonFolder parent, string caption, string snmpMibNameSuffix, string description,
                uint snmpLeastId)
                : base(parent, snmpLeastId, caption, snmpMibNameSuffix, description)
            {
                Group = GetGroup();
            }
            
            public override string ToString()
            {
                try
                {
                    return $@"Variable '{Caption}', OID={SnmpFullOid}, MIB='{SnmpFullMibName}', G='{GetGroup()?.Caption}', UA='{UaVar?.BrowseName}'";
                }
                catch /* paranoic */
                {
                    Debug.Assert(false);
                    return base.ToString();
                }
            }

            #region OPC UA - specific

            public IbaOpcUaVariable UaVar = null;

            #endregion
        }

        public class ExtMonVariable<T> : ExtMonVariableBase
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
                try
                {
                    return $@"({typeof(T).Name}){Value}: " + base.ToString();
                }
                catch /* paranoic */
                {
                    Debug.Assert(false);
                    return base.ToString();
                }
            }
        }

        public class ExtMonFolder : ExtMonNode
        {
            /// <summary>
            /// Try avoid writing this collection directly or be careful with parent-child consistency.
            /// Use <see cref="AddChildFolder"/> or <see cref="AddChildVariable{T}"/> if possible.
            /// </summary>
            public readonly List<ExtMonNode> Children = new List<ExtMonNode>();

            /// <summary>
            /// Is used for "Big" folders - section roots - like "Standard Jobs".
            /// Note, that here FULL OID and FULL MIB name are used here (not suffixes).
            /// </summary>
            public ExtMonFolder(ExtMonFolder parent,
                string caption, string snmpFullMibName, string uaBrowseName, string description, IbaSnmpOid fullOid)
                : base(parent, 0, caption, null, description)
            {
                SnmpFullMibName = snmpFullMibName;
                SnmpFullOid = fullOid;
                UaBrowseName = uaBrowseName;
            }

            /// <summary>
            /// Is used for "small" folders (i.e. all other folders except section roots).
            /// Note, that here OID suffixes MIB suffixes are used (not full OID, not full MIB name).
            /// </summary>
            public ExtMonFolder(ExtMonFolder parent,
                string caption, string snmpMibNameSuffix, string description, uint snmpLeastId)
                : base(parent, snmpLeastId, caption, snmpMibNameSuffix, description)
            {
            }

            /// <summary> Adds a child folder, preserving parent-child consistency </summary>
            public ExtMonFolder AddChildFolder(string caption, string snmpMibNameSuffix,
                string description, uint snmpLeastId)
            {
                // set a valid id for the case of auto-numbering
                ApplySnmpLeastIdAutoNumbering(ref snmpLeastId);

                // create a child and add to collection
                var child = new ExtMonFolder(this,caption, snmpMibNameSuffix, description, snmpLeastId);
                Children.Add(child);
                return child;
            }


            /// <summary> Adds a child variable, preserving parent-child consistency </summary>
            public ExtMonVariable<T> AddChildVariable<T>(string caption, string snmpMibNameSuffix,
                string description, uint snmpLeastId)
            {
                // set a valid id for the case of auto-numbering
                ApplySnmpLeastIdAutoNumbering(ref snmpLeastId);

                // create a child and add to collection
                var child = new ExtMonVariable<T>(this, caption, snmpMibNameSuffix, description, snmpLeastId);
                Children.Add(child);
                return child;
            }

            private void ApplySnmpLeastIdAutoNumbering(ref uint snmpLeastId)
            {
                if (snmpLeastId == SNMP_AUTO_LEAST_ID)
                    snmpLeastId = (uint)Children.Count + 1;
            }


            public List<ExtMonNode> GetFlatListOfAllChildren()
            {
                var list = new List<ExtMonNode>();
                foreach (var child in Children)
                {
                    switch (child)
                    {
                        case ExtMonFolder folder:
                            list.Add(folder);
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

            public void ClearChildren()
            {
                Children.Clear();
            }

            public override string ToString()
            {
                try
                {
                    return $@"Folder '{Caption}', OID={SnmpFullOid}, MIB='{SnmpFullMibName}'";
                }
                catch /* paranoic */
                {
                    Debug.Assert(false);
                    return base.ToString();
                }
            }
        }

        public enum GroupUpdateMode
        {
            /// <summary> Actual values are requested from TaskManager only when external request
            /// (from SNMP, OPC, etc.) occurs.
            /// If there are no external requests, data is not updated at all.
            /// Timestamps are used to track whether data is 'fresh' enough. </summary>
            /// <remarks> License info, GlobalCleanup are good examples of this mode.
            /// It makes no sense to actively track e.g. free space of a drive if it is not monitored from outside </remarks>
            UpdatedOnExternalRequest,

            /// <summary> Is kept up-to-date by workers inside the core of datCo
            /// independently on whether the data is monitored/requested from outside or not. </summary>
            /// <remarks> ComputedValues folder is an example of such mode.
            /// Since we have to calculate expressions for each file anyway, it's easier to update values
            /// after each calculation, rather than to wait for external requests to do this.
            /// Timestamps are ignored for this mode. </remarks>
            UpdatedContinuouslyByTaskManager
        }

        /// <summary>
        /// Group is a special kind of folder.
        /// It has a timestamp of last update, and it's assumed that its items are updated altogether if one of
        /// group's items is requested from outside.
        /// </summary>
        public abstract class ExtMonGroup : ExtMonFolder
        {
            public GroupUpdateMode UpdateMode { get; }

            protected ExtMonGroup(ExtMonFolder parent, GroupUpdateMode updateMode, TimeSpan ageThreshold, uint snmpLeastId,
                string caption = null, string snmpMibNameSuffix = null, string description = null)
                : base(parent, caption, snmpMibNameSuffix, description, snmpLeastId)
            {
                UpdateMode = updateMode;

                if (ageThreshold < MinimalAgeThreshold)
                {
                    // it makes no sense to set group's threshold to level lower than minimal;
                    // (you should change MinimalAgeThreshold also if you need a better resolution)
                    ageThreshold = MinimalAgeThreshold;
                    Debug.Assert(false, 
                        $"Warning! {nameof(ExtMonGroup)}.{nameof(AgeThreshold)} was truncated to {nameof(MinimalAgeThreshold)}.");
                }
                TimeStamp = new TimeStampWithThreshold(ageThreshold);
                SnmpTimeStamp = new TimeStampWithThreshold(ageThreshold);
                UaTimeStamp = new TimeStampWithThreshold(ageThreshold);

                Debug.Assert(AgeThreshold == SnmpTimeStamp.AgeThreshold &&
                             AgeThreshold == UaTimeStamp.AgeThreshold);
            }


            public TimeSpan AgeThreshold => TimeStamp.AgeThreshold;

            /// <summary> When data has been last time requested from the TaskManager </summary>
            public TimeStampWithThreshold TimeStamp { get; }

            /// <summary> When data was last time copied to SNMP </summary>
            public TimeStampWithThreshold SnmpTimeStamp { get; }

            /// <summary> When data was last time copied to OPC UA </summary>
            public TimeStampWithThreshold UaTimeStamp { get; }


            public class TimeStampWithThreshold
            {
                public DateTime Stamp { get; private set; } = DateTime.MinValue;

                /// <summary> A measure to tell whether some data is fresh enough (and can be used as is) or
                /// is probably too old (and should be updated before sending via SNMP or OPC UA). </summary>
                public TimeSpan AgeThreshold { get; }

                public TimeSpan Age => Stamp == DateTime.MinValue
                        ? TimeSpan.MaxValue : DateTime.Now - Stamp;

                /// <summary> Tells if data is younger than Threshold (then it is treated as fresh enough). </summary>
                public bool IsUpToDate => Age < AgeThreshold;

                public TimeStampWithThreshold(TimeSpan ageThreshold)
                {
                    AgeThreshold = ageThreshold;
                }

                public void PutStamp()
                {
                    Stamp = DateTime.Now;
                }
            }

        }

        [Serializable]
        public class GuiTreeNodeTag
        {
            public string Caption { get; set; }

            public string Description { get; set; }

            public bool IsFolder { get; set; }

            /// <summary> applicable only to variables </summary>
            public string Value { get; set; }

            /// <summary> applicable only to variables </summary>
            public string Type { get; set; }

            /// <summary> key for SNMP </summary>
            public IbaSnmpOid SnmpOid { get; set; }

            public string SnmpMibName { get; set; }
            
            /// <summary> key for OPC UA </summary>
            public string OpcUaNodeId { get; set; }
        }

        #endregion


        public class LicenseInfo : ExtMonGroup
        {

            /// <summary> Oid 1 </summary>
            public readonly ExtMonVariable<bool>  IsValid;
            /// <summary> Oid 2 </summary>
            public readonly ExtMonVariable<string> Sn;
            /// <summary> Oid 3 </summary>
            public readonly ExtMonVariable<string> HwId;
            /// <summary> Oid 4 </summary>
            public readonly ExtMonVariable<string> DongleType;
            /// <summary> Oid 5 </summary>
            public readonly ExtMonVariable<string> Customer;
            /// <summary> Oid 6 </summary>
            public readonly ExtMonVariable<int> TimeLimit;
            /// <summary> Oid 7 </summary>
            public readonly ExtMonVariable<int> DemoTimeLimit;

            public LicenseInfo(ExtMonFolder parent) 
                : base(parent, GroupUpdateMode.UpdatedOnExternalRequest, LicenseAgeThreshold, 0 /*not used*/)
            {
                // set Caption, Description, etc.
                Caption = @"Licensing";
                Description = "License information."; // is used only for OPC UA (in SNMP it's predefined in ibaSnmp.dll)
                SnmpFullOid = "999999"; // is not used. License OID is actually "PrGeneral.3", but it's predefined in ibaSnmp.dll
                SnmpFullMibName = "License"; // is not used. it's predefined in ibaSnmp.dll


                // create variables and add them to collection

                IsValid = AddChildVariable<bool>(
                    @"Is valid", "IsValid",
                    @"Is license valid.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(IsValid.SnmpLeastId == 1);

                Sn = AddChildVariable<string>(
                    @"Dongle serial number", "DongleSerialNumber",
                    @"Dongle serial number.",
                    SNMP_AUTO_LEAST_ID);

                HwId = AddChildVariable<string>(
                    @"Dongle hardware ID", "DongleHwId",
                    @"Dongle hardware ID.",
                    SNMP_AUTO_LEAST_ID);

                DongleType = AddChildVariable<string>(
                    @"Dongle type", "DongleType",
                    @"Dongle type.",
                    SNMP_AUTO_LEAST_ID);

                Customer = AddChildVariable<string>(
                    @"Customer", "Customer",
                    @"License customer.",
                    SNMP_AUTO_LEAST_ID);

                TimeLimit = AddChildVariable<int>(
                    @"Time limit", "TimeLimit",
                    @"Time limit.",
                    SNMP_AUTO_LEAST_ID);

                DemoTimeLimit = AddChildVariable<int>(
                    @"Demo time limit", "DemoTimeLimit",
                    @"Demo time limit.",
                    SNMP_AUTO_LEAST_ID);

                Debug.Assert(DemoTimeLimit.SnmpLeastId == 7);
            }

            /// <summary> Resets all the fields to default values </summary>
            public void Reset()
            {
                IsValid.Value = false;
                Sn.Value = "";
                HwId.Value = "";
                DongleType.Value = "";
                Customer.Value = "";
                TimeLimit.Value = 0;
                DemoTimeLimit.Value = 0;
            }
        }


        public class GlobalCleanupDriveInfo : ExtMonGroup
        {
            /// <summary> Primary key for the drive info collection (is uniquely identified by drive name) </summary>
            public string Key => DriveName.Value;

            /// <summary> Oid 1 </summary>
            public readonly ExtMonVariable<string> DriveName;
            /// <summary> Oid 2 </summary>
            public readonly ExtMonVariable<bool> Active;
            /// <summary> Oid 3 </summary>
            public readonly ExtMonVariable<uint> SizeInMb;
            /// <summary> Oid 4 </summary>
            public readonly ExtMonVariable<uint> CurrentFreeSpaceInMb;
            /// <summary> Oid 5 </summary>
            public readonly ExtMonVariable<uint> MinFreeSpaceInPercent;
            /// <summary> Oid 6 </summary>
            public readonly ExtMonVariable<uint> RescanTime;

            public GlobalCleanupDriveInfo(ExtMonFolder parent, uint snmpLeastId, string driveName) 
                : base(parent, GroupUpdateMode.UpdatedOnExternalRequest, DriveInfoAgeThreshold, snmpLeastId)
            {
                // set Caption, Description, etc.
                Caption = $@"Drive '{driveName}'";
                SnmpFullMibName = $@"globalCleanupDrive{snmpLeastId}"; 
                Description = $@"Global cleanup settings for the drive '{driveName}'.";

                // create variables and add them to collection

                DriveName = AddChildVariable<string>(
                    @"Drive Name", @"Name",
                    @"Drive name like it appears in operating system.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(DriveName.SnmpLeastId == 1); // ensure id has an expected value

                Active = AddChildVariable<bool>(
                    @"Active", @"Active",
                    @"Whether or not the global cleanup is enabled for the drive.",
                    SNMP_AUTO_LEAST_ID);

                SizeInMb = AddChildVariable<uint>(
                    @"Size", @"Size",
                    @"Size of the drive (in megabytes).",
                    SNMP_AUTO_LEAST_ID);

                CurrentFreeSpaceInMb = AddChildVariable<uint>(
                    @"Current free space", @"CurrFreeSpace",
                    @"Current free space of the drive (in megabytes).",
                    SNMP_AUTO_LEAST_ID);

                MinFreeSpaceInPercent = AddChildVariable<uint>(
                    @"Min free space", @"MinFreeSpace",
                    @"Minimum disk space that is kept free on the drive by deleting the oldest iba dat files (in percent).",
                    SNMP_AUTO_LEAST_ID);

                RescanTime = AddChildVariable<uint>(
                    @"Rescan time", @"RescanTime",
                    @"How often the application rescans the drive parameters (in minutes).",
                SNMP_AUTO_LEAST_ID);
                Debug.Assert(RescanTime.SnmpLeastId == 6); // ensure id has an expected value

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
                try
                {
                    return $@"Drive '{Key}' [A:{Active.Value}, {CurrentFreeSpaceInMb.Value}/{SizeInMb.Value}]";
                }
                catch /* paranoic */
                {
                    Debug.Assert(false);
                    return base.ToString();
                }
            }
        }


        #region Tasks

        /// <summary> OID 1...n - one class instance per each task </summary>
        public class TaskInfo : ExtMonFolder
        {
            /// <summary> Key of the task list; corresponds to TaskManager's cfg guid </summary>
            public Guid Guid;

            /// <summary> Oid 1 </summary>
            public readonly ExtMonVariable<string> TaskName;
            /// <summary> Oid 2 </summary>
            public readonly ExtMonVariable<string> TaskType;
            /// <summary> Oid 3 </summary>
            public readonly ExtMonVariable<bool> Success;
            /// <summary> Oid 4 </summary>
            public readonly ExtMonVariable<uint> DurationOfLastExecutionInSec;
            /// <summary> Oid 5 </summary>
            public readonly ExtMonVariable<uint> MemoryUsedForLastExecutionInMb;

            /// <summary> Oid 7 </summary>
            public readonly ExtMonVariable<bool> Enabled;

            /// <summary> Oid 8 </summary>
            public readonly ExtMonVariable<TaskData.WhenToDo> WhenToExecute;

            /// <summary> A Job the task belongs to </summary>
            public JobInfoBase ParentJob;

            /// <summary> Oid 6. Folder is OPTIONAL, can be empty for tasks that have no cleanup settings </summary>
            public LocalCleanupInfo CleanupInfo { get; private set; }

            public TaskInfo(ExtMonFolder parent, uint snmpLeastId, string taskName)
                : base(parent, null, null, null, snmpLeastId)
            {
                // determine parent job
                Debug.Assert(parent != null); // "job.tasks" folder
                Debug.Assert(parent.Parent != null); // job
                // ReSharper disable once ConstantConditionalAccessQualifier (because of Debug.Assert)
                ParentJob = parent?.Parent as JobInfoBase;
                Debug.Assert(ParentJob != null);

                // set Caption, Description, etc.
                Caption = $@"Task '{taskName}'"; // can be non-unique
                SnmpFullMibName = ParentJob.SnmpFullMibName + $@"Task{snmpLeastId}";
                Description = $@"Information about task '{taskName}' of the job '{ParentJob.JobName.Value}'.";

                // create variables and add them to collection

                // todo. kls. low priority. Do description change on Task rename (this was NOT implemented in original version also) 
                TaskName = AddChildVariable<string>(
                    @"Task name", @"Name",
                    @"The name of the task as it appears in GUI.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(TaskName.SnmpLeastId == 1); // ensure id has an expected value

                TaskType = AddChildVariable<string>(
                    @"Task type", "Type",
                    @"The type of the task (copy, extract, report, etc.).",
                    SNMP_AUTO_LEAST_ID);

                Success = AddChildVariable<bool>(
                    @"Success", @"Success", 
                    @"Whether or not the last executed task was completed successfully, i.e. without errors. " +
                    @"For Condition task this means that the expression was successfully evaluated as " +
                    @"TRUE or FALSE - both results are treated as success.",
                    SNMP_AUTO_LEAST_ID);

                DurationOfLastExecutionInSec = AddChildVariable<uint>(
                    @"Duration of last execution", @"DurationOfLastExecution",
                    @"Duration of the last task execution (in seconds).",
                    SNMP_AUTO_LEAST_ID);

                MemoryUsedForLastExecutionInMb = AddChildVariable<uint>(
                    @"Memory used for last execution", @"LastMemoryUsed",
                    @"Amount of memory used during the last execution of the task (in megabytes). " +
                    @"This is applicable only to tasks that use ibaAnalyzer for their processing e.g., " +
                    @"Condition, Report, Extract and some custom tasks.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(MemoryUsedForLastExecutionInMb.SnmpLeastId == 5); // ensure id has an expected value

                Enabled = AddChildVariable<bool>(
                    @"Enabled", @"Enabled",
                    @"Whether or not the task is enabled",
                    7);
                //Debug.Assert(.SnmpLeastId == ); // makes no sense to assert because is OID is explicit (not SNMP_AUTO_LEAST_ID)

                WhenToExecute = AddChildVariable<TaskData.WhenToDo>(
                    @"Execute condition", @"ExecuteCondition",
                    $@"Condition when the task is executed ({GetEnumTextDescription(typeof(TaskData.WhenToDo).Name)}).",
                    8);
                //Debug.Assert(.SnmpLeastId == ); // makes no sense to assert because is OID is explicit (not SNMP_AUTO_LEAST_ID)

                // set default values
                Reset();
            }

            // create Cleanup folder and add it to collection
            public LocalCleanupInfo AddCleanupInfo()
            {
                if (CleanupInfo != null)
                {
                    // we already have it
                    return CleanupInfo;
                }

                Children.Add(CleanupInfo = new LocalCleanupInfo(this, 6));
                return CleanupInfo;
            }

            public void DeleteCleanupInfo()
            {
                Children.Remove(CleanupInfo);
                CleanupInfo = null;
            }

            /// <summary> Resets to default all values;
            /// (keys and structure is not reset (Guid, Oid, mibName, Parent, etc) </summary>
            public void Reset()
            {
                TaskName.Value = "";
                TaskType.Value = "";
                Success.Value = false;
                DurationOfLastExecutionInSec.Value = 0;
                MemoryUsedForLastExecutionInMb.Value = 0;
                CleanupInfo?.Reset();
            }

            public override string ToString()
            {
                try
                {
                    return $@"{TaskName.Value} [{TaskType.Value}, {SnmpFullOid}, {ParentJob.JobName.Value}]";
                }
                catch /* paranoic */
                {
                    Debug.Assert(false);
                    return base.ToString();
                }
            }
        }
        

        public class LocalCleanupInfo : ExtMonFolder
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
                : base(parent, @"Cleanup", @"Cleanup",
                    @"Cleanup parameters of the task.", snmpLeastId)
            {
                // create variables and add them to collection

                LimitChoice = AddChildVariable<TaskWithTargetDirData.OutputLimitChoiceEnum>(
                        @"Limit choice", @"LimitChoice",
                        @"Option selected as limit for the disk space usage " +
                        $@"({GetEnumTextDescription(typeof(TaskWithTargetDirData.OutputLimitChoiceEnum).Name)}).",
                        SNMP_AUTO_LEAST_ID);
                Debug.Assert(LimitChoice.SnmpLeastId == 1); // ensure id has an expected value

                Subdirectories = AddChildVariable<uint>(
                        @"Subdirectories", @"Subdirectories",
                        @"Maximum count of directories the task can use.",
                        SNMP_AUTO_LEAST_ID);

                UsedDiskSpace = AddChildVariable<uint>(
                        @"Used disk space", @"UsedDiskSpace",
                        @"Maximum disk space that can be used by the task (in megabytes).",
                        SNMP_AUTO_LEAST_ID);

                FreeDiskSpace = AddChildVariable<uint>(
                        @"Free disk space", @"FreeDiskSpace",
                        @"Minimum disk space that is kept free (in megabytes).",
                        SNMP_AUTO_LEAST_ID);
                Debug.Assert(FreeDiskSpace.SnmpLeastId == 4); // ensure id has an expected value

                Reset();
            }

            public void Reset()
            {
                LimitChoice.Value = TaskWithTargetDirData.OutputLimitChoiceEnum.None;
                Subdirectories.Value = 0;
                UsedDiskSpace.Value = 0;
                FreeDiskSpace.Value = 0;
            }
        }

        #endregion


        #region Jobs

        public enum JobStatus
        {
            Disabled = 0,
            Started = 1, 
            Stopped = 2
        }


        /// <summary> OID ...2 Standard Jobs </summary>
        public abstract class JobInfoBase : ExtMonGroup
        {
            /// <summary> Key of the job list; corresponds to TaskManager's cfg guid  </summary>
            public Guid Guid;

            /// <summary> Oid 1 - Contains everything except tasks </summary> 
            public ExtMonFolder FolderGeneral;
            /// <summary> Oid 2 - Contains tasks (one subfolder per each task) </summary> 
            public ExtMonFolder FolderTasks;

            /// <summary> Oid General.1 </summary> 
            public readonly ExtMonVariable<string> JobName;
            /// <summary> Oid General.2 </summary>
            public readonly ExtMonVariable<JobStatus> Status;
            /// <summary> Oid General.3 </summary>
            public readonly ExtMonVariable<uint> TodoCount;
            /// <summary> Oid General.4 </summary>
            public readonly ExtMonVariable<uint> DoneCount;
            /// <summary> Oid General.5 </summary>
            public readonly ExtMonVariable<uint> FailedCount;

            /// <summary> In General folder OIDs from 6 to BaseExtraVariablesStartNumber are reserved for JobType-specific values.
            /// Additional common items should be added with OIDs greater than this number. </summary>
            public const int BaseExtraVariablesStartNumber = 100;

            /// <summary> Oid General.(<see cref="BaseExtraVariablesStartNumber"/>+1) (is added using <see cref="AddBaseExtraVariablesToTheEnd"/>) </summary>
            public ExtMonVariable<int> UpTime;
            /// <summary> Oid General.(<see cref="BaseExtraVariablesStartNumber"/>+2) (is added using <see cref="AddBaseExtraVariablesToTheEnd"/>)</summary>
            public ExtMonVariable<int> Lifebeat;

            protected JobInfoBase(ExtMonFolder parent, uint snmpLeastId, string jobName) 
                : base(parent, GroupUpdateMode.UpdatedOnExternalRequest, JobAgeThreshold, snmpLeastId)
            {
                Caption = $@"Job '{jobName}'";
                // SnmpFullMibName = ""; // is set in derived classes
                // Description = ""; // is set in derived classes

                // create folders and add them to collection

                // todo. kls. low priority. Do description change on Job rename (this was NOT implemented in original version also) 
                FolderGeneral = AddChildFolder(
                    @"General", @"General",
                    $@"General properties of job '{jobName}'.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(FolderGeneral.SnmpLeastId == 1); // ensure id has an expected value

                FolderTasks = AddChildFolder(
                    @"Tasks", @"Tasks",
                    $@"Information about all tasks of the job '{jobName}'.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(FolderTasks.SnmpLeastId == 2); // ensure id has an expected value

                // create variables and add them to collection

                JobName = FolderGeneral.AddChildVariable<string>(
                    @"Job Name", @"Name",
                    @"The name of the job as it appears in GUI.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(JobName.SnmpLeastId == 1); // ensure id has an expected value
                
                Status = FolderGeneral.AddChildVariable<JobStatus>(
                    @"Status", @"Status",
                    $@"Current status of the job ({GetEnumTextDescription(typeof(JobStatus).Name)}).",
                    SNMP_AUTO_LEAST_ID);

                TodoCount = FolderGeneral.AddChildVariable<uint>(
                    @"Todo #", @"Todo",
                    @"Number of dat files to be processed.",
                    SNMP_AUTO_LEAST_ID);

                DoneCount = FolderGeneral.AddChildVariable<uint>(
                    @"Done #", @"Done",
                    @"Number of processed files.",
                    SNMP_AUTO_LEAST_ID);

                FailedCount = FolderGeneral.AddChildVariable<uint>(
                    @"Failed #", @"Failed",
                    @"Number of errors occurred during processing.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(FailedCount.SnmpLeastId == 5); // ensure id has an expected value

                PrivateReset();
            }

            /// <summary> Adds variables that are common for all job types but are located at the end of the list
            /// (after jobType-specific variables).
            /// Numbering of this part starts with <see cref="BaseExtraVariablesStartNumber"/>
            /// to reserve some OID space for jobType-specific variables. </summary>
            protected void AddBaseExtraVariablesToTheEnd()
            {
                UpTime = FolderGeneral.AddChildVariable<int>(
                    @"Up time", @"UpTime",
                    @"Time in seconds elapsed since the job start" +
                    @" (-1 if the job is stopped or disabled).",
                    BaseExtraVariablesStartNumber + 1); 
                Lifebeat = FolderGeneral.AddChildVariable<int>(
                    @"Lifebeat", @"Lifebeat",
                    @"A special value that should be continuously changing, " +
                    @"indicating that is everything is ok with connection, processing, etc.",
                    BaseExtraVariablesStartNumber + 2);
            }

            /// <summary> Resets everything except tasks </summary>
            private void PrivateReset()
            {
                JobName.Value = "";
                Status.Value = JobStatus.Disabled;
                TodoCount.Value = 0;
                DoneCount.Value = 0;
                FailedCount.Value = 0;
                // task reset is not needed here; is done with a separate function
            }

            /// <summary> Resets to default values everything except Guid (primary key) and Tasks </summary>
            public virtual void Reset()
            {
                PrivateReset();
            }

            public int TaskCount => FolderTasks.Children.Count;


            /// <summary> Gets Job's task with given index </summary>
            public TaskInfo this[int index]
            {
                get
                {
                    ExtMonNode child = FolderTasks.Children[index];
                    Debug.Assert(child is TaskInfo);
                    return child as TaskInfo;
                }
            }

            public TaskInfo AddTask(string taskName, Guid guid)
            {
                var taskInfo = new TaskInfo(FolderTasks, (uint) FolderTasks.Children.Count + 1, taskName)
                {
                    Guid = guid,
                    UaBrowseName = $@"Task{{{guid}}}" 
                    //UaBrowseName = $@"Task{FolderTasks.Children.Count + 1}" // uncomment this line to use index instead of Guid
                };
                FolderTasks.Children.Add(taskInfo);

                // check consistency between mib name and oid
                Debug.Assert(taskInfo.SnmpFullMibName.Contains($"Task{taskInfo.SnmpLeastId}"));
                return taskInfo;
            }
            public void ResetTasks()
            {
                FolderTasks.ClearChildren();
            }

            public override string ToString()
            {
                try
                {
                    return $@"{JobName.Value} [{Status}, {TodoCount}/{DoneCount}/{FailedCount}, T:{TaskCount}]";
                }
                catch /* paranoic */
                {
                    Debug.Assert(false);
                    return base.ToString();
                }
            }
        }


        public class StandardJobInfo : JobInfoBase
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

                PermFailedCount = FolderGeneral.AddChildVariable<uint>(
                    @"Perm. Failed #", @"PermFailedCount",
                    @"Number of files with persistent errors.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(PermFailedCount.SnmpLeastId == 6); // ensure id has an expected value

                TimestampJobStarted = FolderGeneral.AddChildVariable<DateTime>(
                    @"Timestamp job started", @"TimestampJobStarted",
                    @"Time when the job was started. For a stopped job, it relates to the last start of the job. " +
                    @"If job was never started, then value is '01.01.0001 0:00:00'.",
                    SNMP_AUTO_LEAST_ID);

                TimestampLastDirectoryScan = FolderGeneral.AddChildVariable<DateTime>(
                    @"Timestamp last directory scan", @"TimestampLastDirectoryScan",
                    @"Time when the last scan for new (unprocessed) .dat files was performed. " +
                    @"If scan was never performed, then value is '01.01.0001 0:00:00'.",
                    SNMP_AUTO_LEAST_ID);

                TimestampLastReprocessErrorsScan = FolderGeneral.AddChildVariable<DateTime>(
                    @"Timestamp last reprocess errors scan", @"TimestampLastReprocessErrorsScan",
                    @"Time when the last reprocess scan was performed " +
                    @"(reprocess scan is a scan for .dat files that previously were processed with errors).  " +
                    @"If scan was never performed, then value is '01.01.0001 0:00:00'.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(TimestampLastReprocessErrorsScan.SnmpLeastId == 9); // ensure id has an expected value

                // last processing Folder
                FolderLastProcessing = FolderGeneral.AddChildFolder(@"LastProcessing", @"LastProcessing",
                    @"Information about the last successfully processed file.",
                    SNMP_AUTO_LEAST_ID);
                Debug.Assert(FolderLastProcessing.SnmpLeastId == 10); // ensure id has an expected value


                // last processing

                LastProcessingLastDatFileProcessed = FolderLastProcessing.AddChildVariable<string>(
                    @"Last dat-file processed", null /*is special, see below*/, 
                    @"Filename of the last successfully processed file. If no files were successfully processed, then value is empty.",
                    SNMP_AUTO_LEAST_ID);
                // set special mib name (skip name of FolderLastProcessing) 
                LastProcessingLastDatFileProcessed.SnmpFullMibName = FolderGeneral.SnmpFullMibName + @"LastFile";
                // set UA name differently to mib name 
                LastProcessingLastDatFileProcessed.UaBrowseName = @"LastFile";
                Debug.Assert(LastProcessingLastDatFileProcessed.SnmpLeastId == 1); // ensure id has an expected value

                LastProcessingStartTimeStamp = FolderLastProcessing.AddChildVariable<DateTime>(
                    @"Start timestamp", null /*is special, see below*/,
                    @"Time when processing of the last successfully processed file was started. " +
                    @"If no files were successfully processed, then value is '01.01.0001 0:00:00'.",
                    SNMP_AUTO_LEAST_ID);
                // set special mib name (skip name of FolderLastProcessing) 
                LastProcessingStartTimeStamp.SnmpFullMibName = FolderGeneral.SnmpFullMibName + @"StartStamp";
                // set UA name differently to mib name 
                LastProcessingStartTimeStamp.UaBrowseName = @"StartStamp";

                LastProcessingFinishTimeStamp = FolderLastProcessing.AddChildVariable<DateTime>(
                    @"Finish timestamp", null /*is special, see below*/,
                    @"Time when processing of the last successfully processed file was finished. " +
                    @"If no files were successfully processed, then value is '01.01.0001 0:00:00'.",
                    SNMP_AUTO_LEAST_ID);
                // set special mib name (skip name of FolderLastProcessing) 
                LastProcessingFinishTimeStamp.SnmpFullMibName = FolderGeneral.SnmpFullMibName + @"FinishStamp";
                // set UA name differently to mib name 
                LastProcessingFinishTimeStamp.UaBrowseName = @"FinishStamp";
                Debug.Assert(LastProcessingFinishTimeStamp.SnmpLeastId == 3); // ensure id has an expected value

                AddBaseExtraVariablesToTheEnd();

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


        public class ScheduledJobInfo : JobInfoBase
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

                PermFailedCount = FolderGeneral.AddChildVariable<uint>(
                        @"Perm. Failed #", @"PermFailedCount",
                        @"Number of files with persistent errors.",
                        SNMP_AUTO_LEAST_ID);
                Debug.Assert(PermFailedCount.SnmpLeastId == 6); // ensure id has an expected value

                TimestampJobStarted = FolderGeneral.AddChildVariable<DateTime>(
                        @"Timestamp job started", @"TimestampJobStarted",
                        @"Time when job was started (starting of the scheduled job does NOT mean that it will be executed immediately).  " +
                        @"For a stopped job, it relates to the last start of the job.  " +
                        @"If job was never started, then value is '01.01.0001 0:00:00'.",
                        SNMP_AUTO_LEAST_ID);

                TimestampLastExecution = FolderGeneral.AddChildVariable<DateTime>(
                        @"Timestamp last execution", @"TimestampLastExecution",
                        @"Time when the job was last executed. " +
                        @"(This does not mean the moment when job was started, but the moment when configured trigger was fired last time); " +
                        @"If job was never executed, then value is '01.01.0001 0:00:00'.",
                        SNMP_AUTO_LEAST_ID);

                TimestampNextExecution = FolderGeneral.AddChildVariable<DateTime>(
                        @"Timestamp next execution", @"TimestampNextExecution",
                        @"Time of the next scheduled execution. " +
                        @"If there is no execution scheduled, then value is '01.01.0001 0:00:00'.",
                        SNMP_AUTO_LEAST_ID);
                Debug.Assert(TimestampNextExecution.SnmpLeastId == 9); // ensure id has an expected value

                AddBaseExtraVariablesToTheEnd();

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
        

        public class OneTimeJobInfo : JobInfoBase
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

                TimestampLastExecution = FolderGeneral.AddChildVariable<DateTime>(
                        @"Timestamp last execution", @"TimestampLastExecution",
                        @"Time when the last execution was started. " +
                        @"If job was never executed, then value is '01.01.0001 0:00:00'.",
                SNMP_AUTO_LEAST_ID);
                Debug.Assert(TimestampLastExecution.SnmpLeastId == 6); // ensure id has an expected value

                AddBaseExtraVariablesToTheEnd();

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


        public class EventBasedJobInfo : JobInfoBase
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

                PermFailedCount = FolderGeneral.AddChildVariable<uint>(
                        @"Perm. Failed #", @"PermFailedCount",
                        @"Number of files with persistent errors.",
                        SNMP_AUTO_LEAST_ID);
                Debug.Assert(PermFailedCount.SnmpLeastId == 6); // ensure id has an expected value

                TimestampJobStarted = FolderGeneral.AddChildVariable<DateTime>(
                        @"Timestamp job started", @"TimestampJobStarted",
                        @"Time when job was started (starting of the event job does NOT mean that it will be executed immediately).  " +
                        @"For a stopped job, it relates to the last start of the job.  " +
                        @"If job was never started, then value is '01.01.0001 0:00:00'.",
                        SNMP_AUTO_LEAST_ID);

                TimestampLastExecution = FolderGeneral.AddChildVariable<DateTime>(
                        @"Timestamp last execution", @"TimestampLastExecution",
                        @"Time when the job was last executed. " +
                        @"(This does not mean the moment when job was started, but the last occurrence of a monitored event); " +
                        @"If job was never executed, then value is '01.01.0001 0:00:00'.",
                        SNMP_AUTO_LEAST_ID);
                Debug.Assert(TimestampLastExecution.SnmpLeastId == 8); // ensure id has an expected value

                AddBaseExtraVariablesToTheEnd();

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
        
        public class ComputedValuesInfo : ExtMonGroup
        {
            public Guid DataId {get; }

			public ComputedValuesInfo(ExtMonFolder parent, uint snmpLeastId, OpcUaWriterTaskData computedValueTaskData)
				: base(parent, GroupUpdateMode.UpdatedContinuouslyByTaskManager,
                    JobAgeThreshold, snmpLeastId, computedValueTaskData.Name, $@"Task{snmpLeastId}")
			{
                DataId = computedValueTaskData.Guid;
                Description = $@"Computed values of the task '{computedValueTaskData.Name}' of the job '{computedValueTaskData.ParentConfigurationData.Name}'";
                UaBrowseName = $@"Task{{{DataId}}}";

                for (int i = 0; i < computedValueTaskData.Records.Count; i++)
				{
                    var record = computedValueTaskData.Records[i];

                    // just for case
                    if (record.Name == "")
                        continue;

                    // SNMP MIB name character set is rather limited, use index for names
                    string snmpMibNameSuffix = $@"Value{i+1}";
                    string description = $@"Computed value '{record.Name}' for expression '{record.Expression}'";

                    ExtMonVariableBase child = null;
                    switch (record.DataType) 
                    {
                        case OpcUaWriterTaskData.Record.ExpressionType.Number:
                            child = AddChildVariable<double>(record.Name, snmpMibNameSuffix, description, SNMP_AUTO_LEAST_ID);
                            break;
                        case OpcUaWriterTaskData.Record.ExpressionType.Text:
                            child = AddChildVariable<string>(record.Name, snmpMibNameSuffix, description, SNMP_AUTO_LEAST_ID);
                            break;
                        case OpcUaWriterTaskData.Record.ExpressionType.Digital:
                            child = AddChildVariable<bool>(record.Name, snmpMibNameSuffix, description, SNMP_AUTO_LEAST_ID);
                            break;
                        default:
                            Debug.Assert(false);
                            break;
                    }

                    // record.Name is unique within the task
                    child.UaBrowseName = record.Name;
                }
                
                UpdateValues(computedValueTaskData);
			}

            public void UpdateValues(OpcUaWriterTaskData data)
            {
                if (Children.Count != data.Records.Count)
                   Debug.Assert(false);
                for (int i = 0; i < Children.Count; i++)
                {
                    if (Children[i] is ExtMonVariable<double> childd && data.Records[i].Value is double vald && data.Records[i].DataType == OpcUaWriterTaskData.Record.ExpressionType.Number)
                        childd.Value = vald;
                    else if (Children[i] is ExtMonVariable<string> childs && data.Records[i].DataType == OpcUaWriterTaskData.Record.ExpressionType.Text)
                    {
                        if (data.Records[i].Value is string vals)
                            childs.Value = vals;
                        else if (double.IsNaN((double)data.Records[i].Value))
                            childs.Value = "";
                        else
                            Debug.Assert(false);
                    }
                    else if (Children[i] is ExtMonVariable<bool> childb && data.Records[i].Value is double valb && data.Records[i].DataType == OpcUaWriterTaskData.Record.ExpressionType.Digital)
                        childb.Value = valb >= 0.5;
                    else 
                        Debug.Assert(false);
                }
            }
		}

		#endregion


		#endregion


		#region Enums handling

		public struct EnumDescription
        {
            /// <summary> Enum value description; this is needed because in-code enum item names are
            /// sometimes not so self-descriptive or not so user-friendly, to be shown in GUI and MIB
            /// (e.g. <see cref="TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE"/> that is replaced here with "always")
            /// </summary>
            public struct ValDsc
            {
                /// <summary> Short name in lowerCamelStyle without spaces; is used:
                ///  * in GUI to show value, e.g. something like "1 (limitDirectories)"
                ///  * as SNMP MIB name </summary>
                public readonly string Name;
                /// <summary> User-friendly descriptions (spaces allowed);
                /// is used to create explication of values inside descriptions (in GUI and in SNMP MIB) </summary>
                public readonly string Description;
                public ValDsc(string name, string description)
                {
                    Name = name;
                    Description = description;
                }
            }
            //public string InternalName;
            public string SnmpMibName;
            public string Description;
            public Dictionary<int, ValDsc> Values;
        }

        public static readonly Dictionary<string, EnumDescription> RegisteredEnums = new Dictionary<string, EnumDescription>();

        private static void CreateEnumDescriptions()
        {
            EnumDescription enumJobStatus = new EnumDescription
            {
                SnmpMibName = "JobStatus",
                Description = "Current status of the job (started, stopped or disabled)",
                Values = new Dictionary<int, EnumDescription.ValDsc>
                {
                    {(int) JobStatus.Disabled, new EnumDescription.ValDsc("disabled", nameof(JobStatus.Disabled))},
                    {(int) JobStatus.Started, new EnumDescription.ValDsc("started", nameof(JobStatus.Started))},
                    {(int) JobStatus.Stopped, new EnumDescription.ValDsc("stopped", nameof(JobStatus.Stopped))}
                }
            };
            RegisteredEnums.Add(typeof(JobStatus).Name, enumJobStatus);

            EnumDescription enumCleanupType = new EnumDescription
            {
                SnmpMibName = "LocalCleanupType",
                Description = "Type of limitation of disk space usage",
                Values = new Dictionary<int, EnumDescription.ValDsc>
                {
                    {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.None, new EnumDescription.ValDsc("none","None")},
                    {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.LimitDirectories, new EnumDescription.ValDsc( "limitDirectories", "Maximum subdirectories")},
                    {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.LimitDiskspace, new EnumDescription.ValDsc("limitDiskSpace", "Maximum used disk space")},
                    {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.SaveFreeSpace, new EnumDescription.ValDsc("saveFreeSpace", "Minimum free disk space")}
                }
            };
            RegisteredEnums.Add(typeof(TaskWithTargetDirData.OutputLimitChoiceEnum).Name, enumCleanupType);

            EnumDescription enumExecutionCondition = new EnumDescription
            {
                SnmpMibName = "ExecutionCondition",
                Description = "Condition when a task is executed",
                Values = new Dictionary<int, EnumDescription.ValDsc>
                {
                    {(int) TaskData.WhenToDo.AFTER_SUCCES,  new EnumDescription.ValDsc("onSuccess", "On success")},
                    {(int) TaskData.WhenToDo.AFTER_FAILURE,  new EnumDescription.ValDsc("onFailure", "On failure")},
                    {(int) TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE,  new EnumDescription.ValDsc("always", "Always")},
                    {(int) TaskData.WhenToDo.AFTER_1st_FAILURE_DAT,  new EnumDescription.ValDsc("on1stFailureFile", "On 1st failure of file")},
                    {(int) TaskData.WhenToDo.AFTER_1st_FAILURE_TASK,  new EnumDescription.ValDsc("on1stFailureTask", "On 1st failure of task")},
                    {(int) TaskData.WhenToDo.DISABLED,  new EnumDescription.ValDsc("disabled", "Disabled")},
                }
            };
            RegisteredEnums.Add(typeof(TaskData.WhenToDo).Name, enumExecutionCondition);
        }

        /// <summary> For a given enum type creates a textual explication of values like "0=Disabled, 1=Started, 2=Stopped" </summary>
        public static string GetEnumTextDescription(EnumDescription desc)
        {
            string str = "";
            foreach (var kvp in desc.Values)
            {
                str += $"{kvp.Key} = {kvp.Value.Description}, ";
            }
            return str.TrimEnd(',', ' ');
        }

        /// <summary> For a given enum type creates a textual explication of values like "0=Disabled, 1=Started, 2=Stopped" </summary>
        public static string GetEnumTextDescription(string internalEnumName)
        {
            Debug.Assert(RegisteredEnums.ContainsKey(internalEnumName));
            return GetEnumTextDescription(RegisteredEnums[internalEnumName]);
        }


        #endregion
    }
}
