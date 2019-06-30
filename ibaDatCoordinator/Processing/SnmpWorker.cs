using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Threading;
using System.Xml.Schema;
using iba.Data;
using iba.Logging;
using iba.Properties;
using IbaSnmpLib;
using Timer = System.Timers.Timer;

// all verbatim strings that are in the file (e.g. @"General") should NOT be localized.
// usual strings (e.g. "General") should be localized later.

namespace iba.Processing
{

    #region Helper classes

    public enum SnmpWorkerStatus
    {
        Started,
        Stopped,
        Errored
    }

    [Serializable]
    public class SnmpTreeNodeTag
    {
        public IbaSnmpOid Oid { get; set; }

        public bool IsFolder { get; set; }

        public string Caption { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public string MibName { get; set; }

        public string MibDescription { get; set; }

        public bool IsExpandedByDefault { get; set; }
    }

    /// <summary> Is used to send MIB file contents from Server to Client via remoting </summary>
    [Serializable]
    public struct SnmpMibFileContainer
    {
        /// <summary> Recommended filename NOT including path </summary>
        public string FileName { get; set; }

        /// <summary> String that represent file's contents. See <see cref="IbaSnmpMibGenerator"/> for details. </summary>
        public string Contents { get; set; }
    }

    #endregion


    public class SnmpWorker
    {
        
        #region Construction, Destruction, Init

        public SnmpWorker()
        {
            Status = SnmpWorkerStatus.Errored;
            StatusString = Resources.snmpStatusNotInit;
        }

        public void Init()
        {
            lock (LockObject)
            {
                if (IbaSnmp != null)
                {
                    // disable double initialization
                    return;
                }

                IbaSnmp = new IbaSnmp(IbaSnmpProductId.IbaDatCoordinator);
            }

            IbaSnmp.DosProtectionInternal.Enabled = false;
            IbaSnmp.DosProtectionExternal.Config(5000, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(60));
            RestartAgent();

            TaskManager.Manager.SnmpConfigurationChanged += TaskManager_SnmpConfigurationChanged;
            SnmpObjectsData.ExtMonGroup.AgeThreshold = SnmpObjectsDataValidTimePeriod;

            // create the timer for delayed tree rebuild
            _treeValidatorTimer = new Timer
            {
                Interval = SnmpObjectsDataValidTimePeriod.TotalMilliseconds,
                AutoReset = false // do not repeat
                // it will be re-activated only if data was invalidated
            };
            _treeValidatorTimer.Elapsed += (sender, args) =>
            {
                RebuildTreeIfItIsInvalid();

                // best option to test why it's needed
                // 1. setup SNMP manager to monitor some yet non-existent job. it will show "no such instance". ok.
                // 2. Add one or several jobs to fit the requested OID area. 
                //    Tree will be invalidated but not rebuilt. manager will still show "n.s.i." - wrong.
            };

            RegisterEnums();
            SetGeneralProductInformation();
            RebuildTree();
        }

        public void TaskManager_SnmpConfigurationChanged(object sender, EventArgs e)
        {
            // we do not need to lock something here
            // it's not a problem if structure is invalidated during rebuild is in progress

            // mark tree as invalid
            // it will be rebuilt on 1st request to any existing user node
            IsStructureValid = false;
        }

        #endregion


        #region Configuration of SNMP agent (IbaSnmp libraray)

        //private IbaSnmp IbaSnmp { get; set; }
        public IbaSnmp IbaSnmp { get; private set; }

        private SnmpData _snmpData = new SnmpData();

        public SnmpData SnmpData
        {
            get => _snmpData;
            set
            {
                if (value == null)
                {
                    // do not allow to set null data here
                    return;
                }
                if (_snmpData != null && _snmpData.Equals(value))
                {
                    // Configuration has not changed
                    // do not restart agent
                    return;
                }
                _snmpData = value;

                if (IbaSnmp != null)
                {
                    RestartAgent();
                }
            }
        }

        public SnmpWorkerStatus Status { get; private set; }

        public string StatusString { get; private set; }

        public static string GetCurrentThreadString()
        {
            var thr = Thread.CurrentThread;
            string thrNameOrId = String.IsNullOrWhiteSpace(thr.Name) ? thr.ManagedThreadId.ToString() : thr.Name;
            return $"thr=[{thrNameOrId}]";
        }

        public void RestartAgent()
        {
            var oldStatus = Status;
            Status = SnmpWorkerStatus.Errored;
            StatusString = @"";

            try
            {
                IbaSnmp.Stop();
                ApplyConfigurationToIbaSnmp();
                string logMessage;

                if (_snmpData.Enabled)
                {
                    IbaSnmp.Start();
                    Status = SnmpWorkerStatus.Started;
                    StatusString = String.Format(Resources.snmpStatusRunningOnPort, _snmpData.Port);

                    logMessage = Status == oldStatus
                        ?
                       // log 'was restarted' if status has not changed (now is 'Started' as before) 
                       String.Format(Resources.snmpStatusRunningRestarted, StatusString)
                        :
                        // log 'was started' if status has changed from 'Errored' or 'Stopped' to 'Started' 
                        String.Format(Resources.snmpStatusRunningStarted, StatusString);
                }
                else
                {
                    Status = SnmpWorkerStatus.Stopped;
                    StatusString = Resources.snmpStatusDisabled;

                    logMessage = Status == oldStatus
                        ?
                        // do not log anything if status has not changed (now is 'Stopped' as before) 
                        null
                        :
                        // log 'was stopped' if status has changed from 'Errored' or 'Started' to 'Stopped'
                        String.Format(Resources.snmpStatusStopped, StatusString);
                }

                // log the message if it necessary
                if (logMessage != null)
                {
                    LogData.Data.Logger.Log(Level.Info, logMessage);
                }
            }
            catch (Exception ex)
            {
                Status = SnmpWorkerStatus.Errored;
                StatusString = String.Format(Resources.snmpStatusError, ex.Message);
                if (LogData.Data.Logger.IsOpen) LogData.Data.Logger.Log(Level.Exception, StatusString);
            }
        }

        private void ApplyConfigurationToIbaSnmp()
        {
            if (IbaSnmp == null || SnmpData == null)
            {
                return;
            }

            // apply port, do not change ip addresses
            List<IPEndPoint> eps = IbaSnmp.EndPointsToListen;
            foreach (IPEndPoint ipe in eps)
            {
                ipe.Port = SnmpData.Port;
            }
            IbaSnmp.EndPointsToListen = eps;

            // security
            IbaSnmp.SetSecurityForV1AndV2(new List<string> {SnmpData.V1V2Security});
            IbaSnmp.SetSecurityForV3(new List<IbaSnmpUserAccount> {SnmpData.V3Security});
        }

        public bool UseSnmpV2TcForStrings => SnmpData?.UseSnmpV2TcForStrings ?? true;

        #endregion


        #region Handling object tree - building, refreshing

        #region Common functionality for all objects

        /// <summary> Lock this object while using SnmpWorker.ObjectsData </summary>
        public readonly object LockObject = new object();

        public int LockTimeout { get; } = 50;

        /// <summary> Data older than this will be trated as outdated. 
        /// When requested, such data will be refreshed first before sending via SNMP. </summary>
        public TimeSpan SnmpObjectsDataValidTimePeriod { get; } = TimeSpan.FromSeconds(2);

        /// <summary> Holds all data that is shown via SNMP. 
        /// This data is in convenient structured format, and does not contain SNMP adresses (OIDs) explicitly.
        /// This structure is filled by TaskManager and then is used by SnmpWorker to create SNMP-tree.
        /// </summary>
        internal SnmpObjectsData ObjectsData { get; } = new SnmpObjectsData();

        #region register enums

        private IbaSnmpValueType _enumJobStatus;
        private IbaSnmpValueType _enumCleanupType;

        private void RegisterEnums()
        {
            _enumJobStatus = IbaSnmp.RegisterEnumDataType(
                "JobStatus", "Current status of the job (started, stopped or disabled)",
                new Dictionary<int, string>
                {
                    {(int) SnmpObjectsData.JobStatus.Disabled, "disabled"},
                    {(int) SnmpObjectsData.JobStatus.Started, "started"},
                    {(int) SnmpObjectsData.JobStatus.Stopped, "stopped"}
                }
            );

            _enumCleanupType = IbaSnmp.RegisterEnumDataType(
                "LocalCleanupType", "Type of limitation of disk space usage",
                new Dictionary<int, string>
                {
                    {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.None, "none"},
                    {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.LimitDirectories, "limitDirectories"},
                    {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.LimitDiskspace, "limitDiskSpace"},
                    {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.SaveFreeSpace, "saveFreeSpace"}
                }
            );
        }

        #endregion

        #endregion


        #region General product information

        /// <summary> Sets product name and version. Registers license value handlers. </summary>
        private void SetGeneralProductInformation()
        {
            // change default gui caption from "ibaDatCo" to "ibaDatCoordinator"
            IbaSnmp.SetOidMetadata(IbaSnmp.OidIbaProduct, @"ibaDatCoordinator");

            // all the other captions can be left as is
            // all the general objects already have gui Captions, predefined
            // e.g.:
            // var captionSample = IbaSnmp.GetOidMetadata(IbaSnmp.OidIbaProductGeneralLicensingCustomer).GuiCaption;

            // ibaRoot.DatCoord.General.1 - Title
            //IbaSnmp.SetOidMetadata(IbaSnmp.OidIbaProductGeneralTitle, @"Title");
            IbaSnmp.ValueIbaProductGeneralTitle = @"ibaDatCoordinator";

            // ibaRoot.DatCoord.General.2 - Version
            var ver = GetType().Assembly.GetName().Version;
            IbaSnmp.SetValueIbaProductGeneralVersion(ver.Major, ver.Minor, ver.Build);

            // ibaRoot.DatCoord.General.Licensing.1 - IsValid
            IbaSnmp.LicensingIsValidRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.2 - Serial number
            IbaSnmp.LicensingSnRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.3 - Hardware ID
            IbaSnmp.LicensingHwIdRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.4 - Dongle type
            IbaSnmp.LicensingTypeRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.5 - Customer
            IbaSnmp.LicensingCustomerRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.6 - Time limit
            IbaSnmp.LicensingTimeLimitRequested += IbaSnmp_LicensingValueRequested;

            // ibaRoot.DatCoord.General.Licensing.7 - Demo time limit
            IbaSnmp.LicensingDemoTimeLimitRequested += IbaSnmp_LicensingValueRequested;
        }

        #endregion


        #region Building and rebuilding the tree

        private bool _isStructureValid;

        public bool IsStructureValid
        {
            get { return _isStructureValid; }
            set
            {
                // this implementation works properly if called from different threads
                // lock of the timer is not needed here
                _isStructureValid = value;

                // stop current cycle
                _treeValidatorTimer?.Stop();

                // if sturcture is marked ivalid
                if (!value)
                {
                    // schedule a delayed tree rebuild, 
                    // if it will not happen earlier
                    _treeValidatorTimer?.Start();
                }
            }
        }

        private Timer _treeValidatorTimer;

        /// <summary>
        /// Rebuilds a tree completely if its <see cref="IsStructureValid"/> flag is set to false. 
        /// Use returned value to know whether tree has been rebuilt.
        /// </summary>
        /// <returns> <value>true</value> if tree was rebuilt, 
        /// <value>false</value> if it is valid and has not been modified by this call.</returns>
        public bool RebuildTreeIfItIsInvalid()
        {
            // here I use double than normal timeout to give priority over other locks
            if (Monitor.TryEnter(LockObject, LockTimeout*2))
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
                    LogData.Data.Logger.Log(Level.Debug,
                        $"SNMP. Error acquiring lock when checking whether tree is valid, {GetCurrentThreadString()}.");
                }
                catch
                {
                    // logging is not critical
                }
                return false; // tree structure has not changed
            }
        }

        public bool RebuildTree()
        {
            var man = TaskManager.Manager;
            if (man == null || IbaSnmp == null)
            {
                return false; // rebuild failed
            }

            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    // snmp structure is valid until datCoordinator configuration is changed.
                    // theoretically it can be reset to false by another thread
                    // during the process of rebuild of SnmpObjectsData,
                    // but it's not a problem. 
                    // If this happens, then the tree will be rebuilt once again.
                    // this is better than to lock resetting of IsStructureValid (and consequently have potential risk of a deadlock).
                    IsStructureValid = true;

                    IbaSnmp.DeleteAllUserValues();
                    IbaSnmp.DeleteAllUserOidMetadata();

                    if (!man.SnmpRebuildObjectsData(ObjectsData))
                    {
                        return false; // rebuild failed
                    }

                    // ibaRoot.DatCoord.1 - Product-Specific
                    IbaSnmp.SetOidMetadata(IbaSnmp.OidIbaProductSpecific, "Product-specific");

                    // ibaRoot.DatCoord.Product.1 - Global cleanup
                    BuildFolderRecursively(ObjectsData.FolderGlobalCleanup);
                    // ibaRoot.DatCoord.Product.2 - Standard jobs
                    BuildFolderRecursively(ObjectsData.FolderStandardJobs);
                    // ibaRoot.DatCoord.Product.3 - Scheduled jobs
                    BuildFolderRecursively(ObjectsData.FolderScheduledJobs);
                    // ibaRoot.DatCoord.Product.4 - One time jobs
                    BuildFolderRecursively(ObjectsData.FolderOneTimeJobs);
                    // ibaRoot.DatCoord.Product.5 - Event jobs
                    BuildFolderRecursively(ObjectsData.FolderEventBasedJobs);

                    return true; // rebuilt successfully
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
                    LogData.Data.Logger.Log(Level.Debug,
                        $"SNMP. Error acquiring lock when rebuilding the tree, {GetCurrentThreadString()}.");
                }
                catch
                {
                    // logging is not critical
                }
                return false; // rebuild failed
            }
        }

        private void BuildFolderRecursively(SnmpObjectsData.ExtMonFolder startingFolder)
        {
            try
            {
                SetOidMetadata(startingFolder);
                foreach (var node in startingFolder.Children)
                {
                    switch (node)
                    {
                        case SnmpObjectsData.ExtMonFolder extMonFolder:
                            BuildFolderRecursively(extMonFolder);
                            break;
                        case SnmpObjectsData.ExtMonVariableBase extMonVariableBase:
                            CreateUserValue(extMonVariableBase);
                            break;
                        default:
                            continue;
                    }
                }
            }
            catch 
            {
                // go on with other items 
                // even if current one has failed 
            }

        }


        #region SetOidMetadata and CreateUserValue() 

        private void SetOidMetadata(SnmpObjectsData.ExtMonFolder xmf)
        {
            Debug.Assert(xmf.SnmpFullOid != null);
            IbaSnmp.SetUserOidMetadata(xmf.SnmpFullOid, xmf.SnmpFullMibName, xmf.Description, xmf.Caption);
        }

        private static readonly Dictionary<Type, IbaSnmpValueType> TYPE_DICT = new Dictionary<Type, IbaSnmpValueType>
        {
            {typeof(string), IbaSnmpValueType.String},
            {typeof(bool), IbaSnmpValueType.Bool},
            //{typeof(DateTime), IbaSnmpValueType.DateStr} /*should be handled specially*/,
            {typeof(DateTime), IbaSnmpValueType.DateTimeStr},
            //{typeof(DateTime), IbaSnmpValueType.DateTimeTc} /*should be handled specially*/,
            {typeof(int), IbaSnmpValueType.Int32},
            {typeof(uint), IbaSnmpValueType.Uint32},
            {typeof(float), IbaSnmpValueType.FloatStr},
            //{typeof(float), IbaSnmpValueType.FloatTc} /*should be handled specially*/,
            {typeof(double), IbaSnmpValueType.DoubleStr},
            //{typeof(double), IbaSnmpValueType.DoubleTc} /*should be handled specially*/,
            {typeof(IbaSnmpOid), IbaSnmpValueType.Oid}
        };

        public IbaSnmpValueType GetSnmpType(object value)
        {
            var type = TYPE_DICT.TryGetValue(value.GetType(), out IbaSnmpValueType snmpType) ? snmpType : IbaSnmpValueType.Unknown;
            // handle a special case of possible different date formats
            if (type == IbaSnmpValueType.DateTimeStr && UseSnmpV2TcForStrings)
                type = IbaSnmpValueType.DateTimeTc;
            return type;
        }

        private void CreateUserValue(SnmpObjectsData.ExtMonVariableBase xmv)
        {
            Debug.Assert(xmv.Parent != null);
            Debug.Assert(xmv.SnmpFullOid != null);

            IbaSnmpValueType type;
            if (xmv.ObjValue.GetType().IsEnum)
            {   
                //enum
                Debug.Assert(xmv.ObjValue is SnmpObjectsData.JobStatus || xmv.ObjValue is TaskWithTargetDirData.OutputLimitChoiceEnum);
                type = xmv.ObjValue is SnmpObjectsData.JobStatus ? _enumJobStatus : _enumCleanupType;

                IbaSnmp.CreateEnumUserValue(xmv.SnmpFullOid, type, (int)xmv.ObjValue, null, null,
                    ProductSpecificItemRequested, xmv.GetGroup() );

            }
            else // simple types
            {
                type = GetSnmpType(xmv.ObjValue);
                Debug.Assert(type != IbaSnmpValueType.Unknown);

                IbaSnmp.CreateUserValue(xmv.SnmpFullOid, xmv.ObjValue, type, null, null,
                    ProductSpecificItemRequested, xmv.GetGroup() );
            }

            IbaSnmp.SetUserOidMetadata(xmv.SnmpFullOid, xmv.SnmpFullMibName, xmv.Description, xmv.Caption);
        }

        #endregion


        #endregion


        #region Value-Refresh functions  

        private void SetUserValue(SnmpObjectsData.ExtMonVariableBase xmv)
        {
            // IbaSnmp does not have untyped version of SetUserValue(),
            // so we have to use some workaround here.
            // Either "type dictionary + delegate wrapping" or "switch-case".
            // Since we have mainly only FOUR types (str, DateTime, bool, uint) in use
            // in our current implementation, it's more efficient to use "switch-case". 

            switch (xmv.ObjValue)
            {
                case string val:
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case uint val:
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case bool val:
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case DateTime val:
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case SnmpObjectsData.JobStatus _:
                case TaskWithTargetDirData.OutputLimitChoiceEnum _:
                    // ReSharper disable once PossibleInvalidCastException
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, (int)xmv.ObjValue);
                    break;

                // other types (below) are not used in current implementations
                // and stand here for possible future additions.

                case int val: // not used by now
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case float val: // not used by now
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                case double val: // not used by now
                    IbaSnmp.SetUserValue(xmv.SnmpFullOid, val);
                    break;
                default:
                    // should not happen
                    Debug.Assert(false);
                    break;
            }
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private bool RefreshLicenseInfo()
        {
            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    if (ObjectsData.License.IsUpToDate())
                    {
                        // data is fresh, no need to change something
                        return false; // was not updated
                    }

                    var man = TaskManager.Manager;
                    if (!man.SnmpRefreshLicenseInfo(ObjectsData.License))
                    {
                        // should not happen
                        // failed to update data
                        // don't rebuild the tree, just return false
                        return false; // was not updated
                    }

                    // TaskManager has updated info successfully 
                    // copy it to snmp tree

                    IbaSnmp.ValueIbaProductGeneralLicensingIsValid = ObjectsData.License.IsValid;
                    IbaSnmp.ValueIbaProductGeneralLicensingSn = ObjectsData.License.Sn;
                    IbaSnmp.ValueIbaProductGeneralLicensingHwId = ObjectsData.License.HwId;
                    IbaSnmp.ValueIbaProductGeneralLicensingType = ObjectsData.License.DongleType;
                    IbaSnmp.ValueIbaProductGeneralLicensingCustomer = ObjectsData.License.Customer;
                    IbaSnmp.ValueIbaProductGeneralLicensingTimeLimit = ObjectsData.License.TimeLimit;
                    IbaSnmp.ValueIbaProductGeneralLicensingDemoTimeLimit = ObjectsData.License.DemoTimeLimit;
                    return true; // data was updated
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
                    LogData.Data.Logger.Log(Level.Debug,
                        $"SNMP. Error acquiring lock when updating license, {GetCurrentThreadString()}.");
                }
                catch
                {
                    // logging is not critical
                }
                return false; // was not updated
            }
        }

        // ReSharper disable once UnusedMethodReturnValue.Local
        private bool RefreshGlobalCleanupDriveInfo(SnmpObjectsData.GlobalCleanupDriveInfo driveInfo)
        {
            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    if (RebuildTreeIfItIsInvalid())
                    {
                        // tree was rebuilt completely
                        // no need to update some parts of it
                        // just return right now
                        return true; // data was updated
                    }

                    if (driveInfo.IsUpToDate())
                    {
                        // data is fresh, no need to change something
                        return false; // was not updated
                    }

                    var man = TaskManager.Manager;
                    if (!man.SnmpRefreshGlobalCleanupDriveInfo(driveInfo))
                    {
                        // should not happen
                        // failed to update data
                        // rebuild the tree
                        LogData.Data.Logger.Log(Level.Debug,
                            "SNMP. RefreshGlobalCleanupDriveInfo(). Failed to refresh; tree is marked invalid.");
                        IsStructureValid = false;
                        return false; // data was NOT updated
                    }

                    // TaskManager has updated driveInfo successfully 
                    // copy it to snmp tree
                    foreach (var xmv in driveInfo.GetFlatListOfAllVariables())
                    {
                        SetUserValue(xmv);
                    }

                    return true; // data was updated
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
                    LogData.Data.Logger.Log(Level.Debug,
                        $"SNMP. Error acquiring lock when updating {driveInfo.Key}, {GetCurrentThreadString()}.");
                }
                catch
                {
                    // logging is not critical
                }
                return false; // data was NOT updated
            }
        }
        
        // ReSharper disable once UnusedMethodReturnValue.Local
        private bool RefreshJobInfo(SnmpObjectsData.JobInfoBase jobInfo)
        {
            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    if (RebuildTreeIfItIsInvalid())
                    {
                        // tree was rebuilt completely
                        // no need to update some parts of it
                        // just return right now
                        return true; // data was updated
                    }

                    if (jobInfo.IsUpToDate())
                    {
                        // data is fresh, no need to change something
                        return false; // data was NOT updated
                    }

                    var man = TaskManager.Manager;
                    if (!man.SnmpRefreshJobInfo(jobInfo))
                    {
                        // should not happen
                        // failed to update data
                        // rebuild the tree
                        LogData.Data.Logger.Log(Level.Debug,
                            "SNMP. RefreshJobInfo(). Failed to refresh; tree is marked invalid.");
                        IsStructureValid = false;
                        return false; // data was NOT updated
                    }

                    // TaskManager has updated driveInfo successfully 
                    // copy it to snmp tree
                    foreach (var xmv in jobInfo.GetFlatListOfAllVariables())
                    {
                        SetUserValue(xmv);
                    }


                    return true; // was updated
                }
                catch (Exception ex)
                {
                    LogData.Data.Logger.Log(Level.Exception,
                        $"SNMP. Error during refreshing job {jobInfo.JobName.Value}. {ex.Message}.");
                    return false; // was not updated
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
                    LogData.Data.Logger.Log(Level.Debug,
                        $"SNMP. Error acquiring lock when updating {jobInfo.JobName}, {GetCurrentThreadString()}.");
                }
                catch
                {
                    // logging is not critical
                }
                return false; // was not updated
            }
        }
        
        #endregion


        #region XxxRequested event handlers

        private void IbaSnmp_LicensingValueRequested<T>(object sender, IbaSnmpValueRequestedEventArgs<T> args)
        {
            // refresh data if it is too old 
            RefreshLicenseInfo();

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            try
            {
                args.Value = (T) args.IbaSnmp.GetValue(args.Oid);
            }
            catch
            {
                // suppress possible cast exception from null to ValueType (bool, int, etc.)
            }
        }

        private void ProductSpecificItemRequested(object sender, IbaSnmpObjectValueRequestedEventArgs args)
        {
            // refresh data if it is too old (or rebuild the whole tree if necessary)
            switch (args.Tag)
            {
                case SnmpObjectsData.GlobalCleanupDriveInfo driveInfo:
                    RefreshGlobalCleanupDriveInfo(driveInfo);
                    break;
                case SnmpObjectsData.JobInfoBase jobInfo:
                    RefreshJobInfo(jobInfo);
                    break;
                default:
                    // should not happen
                    Debug.Assert(false);
                    args.Value = null;
                    return;
            }

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            args.Value = args.IbaSnmp.GetValue(args.Oid);
        }

        #endregion

        #endregion


        #region Tree Snapshot for GUI and MIB generation

        public Dictionary<IbaSnmpOid, SnmpTreeNodeTag> GetObjectTreeSnapShot()
        {
            try
            {
                // check tree structure before taking a snapshot
                RebuildTreeIfItIsInvalid();

                var result = new Dictionary<IbaSnmpOid, SnmpTreeNodeTag>();
                var objList = IbaSnmp.GetListOfAllOids();
                if (objList == null)
                {
                    return null;
                }

                var rootOid = IbaSnmp.OidIbaRoot;

                // get a set of all folders and nodes starting with the root
                var nodesSet = new HashSet<IbaSnmpOid> {rootOid};
                foreach (var oid in objList)
                {
                    // skip everything that is outside selected root
                    if (!oid.StartsWith(rootOid))
                    {
                        continue;
                    }

                    // add object itself
                    nodesSet.Add(oid);

                    // add object's parents (folder-nodes)
                    var parents = oid.GetParents();
                    foreach (var parent in parents)
                    {
                        if (parent.StartsWith(rootOid))
                        {
                            nodesSet.Add(parent);
                        }
                    }
                }

                // retrieve information about each node
                foreach (var oid in nodesSet)
                {
                    var tag = GetTreeNodeTag(oid, true);
                    if (tag != null)
                    {
                        result.Add(oid, tag);
                    }
                }

                // mark some nodes as expanded
                var nodesToExpand = new HashSet<IbaSnmpOid>
                {
                    rootOid,
                    IbaSnmp.OidIbaRoot,
                    IbaSnmp.OidIbaProduct,
                    IbaSnmp.OidIbaProductSpecific,
                    //IbaSnmp.OidIbaProductSpecific + SnmpObjectsData.GlobalCleanupOid,// not needed
                    IbaSnmp.OidIbaProductSpecific + 2, // stdJobs
                    IbaSnmp.OidIbaProductSpecific + 3, // schJobs
                    IbaSnmp.OidIbaProductSpecific + 4 // otJobs
                };

                foreach (var oid in nodesToExpand)
                {
                    SnmpTreeNodeTag tag;
                    if (result.TryGetValue(oid, out tag))
                    {
                        tag.IsExpandedByDefault = true;
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(SnmpWorker)}.{nameof(GetObjectTreeSnapShot)}. {ex.Message}");
                return null;
            }

        }

        /// <summary> Gets all information about a node in the format convenient for GUI tree. </summary>
        public SnmpTreeNodeTag GetTreeNodeTag(IbaSnmpOid oid, bool bUpdate = false)
        {
            try
            {
                var tag = new SnmpTreeNodeTag {Oid = oid};

                IbaSnmpOidMetadata metadata = IbaSnmp.GetOidMetadata(oid);
                if (metadata == null)
                {
                    // this is inexisting node
                    // leave all fields empty
                    return tag;
                }

                // fill data common for folders and leaves
                tag.MibName = metadata.MibName;
                tag.MibDescription = metadata.MibDescription;
                tag.Caption = metadata.GuiCaption;

                // try to get value (applicable only to objects=leaves)

                IbaSnmpObjectInfo objInfo;
                try
                {
                    objInfo = IbaSnmp.GetObjectInfo(oid, bUpdate);
                }
                catch (Exception ex)
                {
                    // should not happen, so better to see it if it happens
                    LogData.Data.Logger.Log(Level.Exception,
                        $"{nameof(SnmpWorker)}.{nameof(GetTreeNodeTag)}.({oid}). Error calling GetObjectInfo(). {ex.Message}");
                    return null;
                }

                // check  if this is a folder or leaf
                // object (leaf) can miss a value but anyway should have some data type
                if (!String.IsNullOrWhiteSpace(objInfo?.MibDataType))
                {
                    // this is a leaf node
                    tag.IsFolder = false;

                    tag.Value = IbaSnmp.IsEnumDataTypeRegistered(objInfo.ValueType)
                        ?
                        // enum - format it like e.g. "1 (started)"
                        $@"{objInfo.Value} ({IbaSnmp.GetEnumValueName(objInfo.ValueType, (int) objInfo.Value)})"
                        :
                        // other types - just value
                        objInfo.Value?.ToString() ?? "";


                    tag.Type = objInfo.MibDataType;
                }
                else
                {
                    // this is a folder
                    tag.IsFolder = true;
                }
                return tag;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(SnmpWorker)}.{nameof(GetTreeNodeTag)}({oid}). {ex.Message}");
                return null;
            }
        }

        public List<SnmpMibFileContainer> GenerateMibFiles()
        {
            try
            {
                IbaSnmpMibGenerator gen = new IbaSnmpMibGenerator(IbaSnmp);

                gen.Generate();

                var mibFiles = new List<SnmpMibFileContainer>
                {
                    new SnmpMibFileContainer
                    {
                        FileName = gen.GeneralMibFilename,
                        Contents = gen.GeneralFileString
                    },
                    new SnmpMibFileContainer
                    {
                        FileName = gen.ProductMibFilename,
                        Contents = gen.ProductFileString
                    }
                };

                return mibFiles;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(SnmpWorker)}.{nameof(GenerateMibFiles)}. {ex.Message}");
                return null;
            }
        }

        #endregion

    }
}
