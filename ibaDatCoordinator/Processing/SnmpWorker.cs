﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using iba.Data;
using iba.Logging;
using IbaSnmpLib;

// all verbatim strings that are in the file (e.g. @"General") should NOT be localized.
// usual strings (e.g. "General") should be localized later.

namespace iba.Processing
{

    #region Helper classes

    public enum SnmpWorkerStatus
    {
        Started,
        Stopped,
        Errored,
    }

    public class SnmpWorkerStatusChangedEventArgs : EventArgs
    {
        public SnmpWorkerStatus Status { get; }
        public Color Color { get; }
        public string Message { get; }

        public SnmpWorkerStatusChangedEventArgs(SnmpWorkerStatus status, Color color, string message)
        {
            Status = status;
            Color = color;
            Message = message;
        }
    }

    public class SnmpTreeNodeTag
    {
        public bool IsFolder { get; set; }
        public string Caption { get; set; }
        public IbaSnmpOid Oid { get; set; }
    }

    #endregion


    public class SnmpWorker
    {
        /// <summary> Lock this object while using SnmpWorker.ObjectsData </summary>
        public readonly object LockObject = new object();

        public TimeSpan SnmpObjectsDataValidTimePeriod { get; } = TimeSpan.FromSeconds(2);

        public int LockTimeout { get; } = 50;

        #region Construction, Destruction, Init

        public SnmpWorker()
        {
            Status = SnmpWorkerStatus.Errored;
            // todo Kls localize
            StatusString = "SNMP server is not initialized.";
        }

        public void Init()
        {
            lock (LockObject)
            {
                if (IbaSnmp != null)
                {
                    // disable double initialisation
                    return;
                }

                IbaSnmp = new IbaSnmp(IbaSnmpProductId.IbaDatCoordinator);
            }

            IbaSnmp.DosProtectionInternal.Enabled = false;
            IbaSnmp.DosProtectionExternal.Config(5000, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(60));
            RestartAgent();

            TaskManager.Manager.SnmpConfigurationChanged += TaskManager_SnmpConfigurationChanged;
            SnmpObjectsData.SnmpObjectWithATimeStamp.AgeThreshold = SnmpObjectsDataValidTimePeriod;

            // create the timer for delayed tree rebuild
            _treeValidatorTimer = new System.Timers.Timer
            {
                Interval = SnmpObjectsDataValidTimePeriod.TotalMilliseconds,
                AutoReset = false // do not repeat
                // it will be re-activated only if data was invalidated
            };
            _treeValidatorTimer.Elapsed += (sender, args) =>
            {
                RebuildTreeIfItIsInvalid();

                // best option to test why it's needed
                // 1. setup SNMP manager to monitor some yet inexisting job. it will show "no such instance". ok.
                // 2. Add one ore several jobs to fit the requested OID area. 
                //    Tree will be invalidated but not rebuilt. manager will still show "n.s.i." - wrong.
            };

            RegisterEnums();
            RegisterGeneralObjectHandlers();
            RebuildTreeCompletely();
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

        public IbaSnmp IbaSnmp { get; private set; }

        public event EventHandler<SnmpWorkerStatusChangedEventArgs> StatusChanged;

        private SnmpData _snmpData = new SnmpData();

        public SnmpData SnmpData
        {
            get { return _snmpData; }
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

        public void ApplyStatusToTextBox(TextBox tb)
        {
            tb.Text = StatusString;
            tb.BackColor = StatusToColor(Status);
        }

        public static Color StatusToColor(SnmpWorkerStatus status)
        {
            return status == SnmpWorkerStatus.Started
                ? Color.LimeGreen // running
                : (status == SnmpWorkerStatus.Stopped
                    ? Color.LightGray // stopped
                    : Color.Red); // error
        }

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
                    // todo Kls localize
                    StatusString = $"SNMP server running on port {_snmpData.Port}";

                    // todo Kls localize
                    logMessage = Status == oldStatus ?
                        // log 'was restarted' if status has not changed (now is 'Started' as before) 
                        $"Snmp agent was successfully restarted. Current status: {StatusString}" :
                        // log 'was started' if status has changed from 'Errored' or 'Stopped' to 'Started' 
                        $"Snmp agent was successfully started. Current status: {StatusString}";
                }
                else
                {
                    Status = SnmpWorkerStatus.Stopped;
                    // todo Kls localize
                    StatusString = "SNMP server is disabled";

                    // todo Kls localize
                    logMessage = Status == oldStatus ?
                        // do not log anything if status has not changed (now is 'Stopped' as before) 
                        null :
                        // log 'was stopped' if status has changed from 'Errored' or 'Started' to 'Stopped'
                        $"Snmp agent was successfully stopped. Current status: {StatusString}";
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
                // todo Kls localize
                StatusString = $"Starting the SNMP server failed with error: {ex.Message}";

                LogData.Data.Logger.Log(Level.Exception,
                    // todo Kls localize
                    $"Starting the SNMP server failed with exception: {ex.Message}");
            }

            // trigger status event
            StatusChanged?.Invoke(this,
                new SnmpWorkerStatusChangedEventArgs(Status, StatusToColor(Status), StatusString));
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

        #region Objects

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
                    {(int) SnmpObjectsData.JobStatus.Stopped, "stopped"},
                }
            );

            _enumCleanupType = IbaSnmp.RegisterEnumDataType(
                "LocalCleanupType", "Type of limitation of disk space usage",
                new Dictionary<int, string>
                {
                    {(int) CleanupTaskData.OutputLimitChoiceEnum.None, "none"},
                    {(int) CleanupTaskData.OutputLimitChoiceEnum.LimitDirectories, "limitDirectories"},
                    {(int) CleanupTaskData.OutputLimitChoiceEnum.LimitDiskspace, "limitDiskSpace"},
                    {(int) CleanupTaskData.OutputLimitChoiceEnum.SaveFreeSpace, "saveFreeSpace"}
                }
            );
        }

        private System.Timers.Timer _treeValidatorTimer;

        private readonly IbaSnmpOid _oidSectionGlobalCleanup = "1";
        private readonly IbaSnmpOid _oidSectionStandardJobs = "2";
        private readonly IbaSnmpOid _oidSectionScheduledJobs = "3";
        private readonly IbaSnmpOid _oidSectionOneTimeJobs = "4";

        internal SnmpObjectsData ObjectsData { get; } = new SnmpObjectsData();

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


        #region General objects

        private void RegisterGeneralObjectHandlers()
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

        private void IbaSnmp_LicensingValueRequested<T>(object sender, IbaSnmpValueRequestedEventArgs<T> args)
        {
            // refresh data if it is too old 
            RefreshLicenseInfo();

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            args.Value = (T) args.IbaSnmp.GetValue(args.Oid);
        }

        #endregion

        #region Dat coordinator specific objects

        /// <summary>
        /// Rebuilds a tree completely if its <code>IsStructureValid</code> flag is set to true. 
        /// Use returned value to know whether tree has been rebuilt.
        /// </summary>
        /// <returns> <value>true</value> if tree was rebuilt, 
        /// <value>false</value> if it is valid and has not been modified by this call.</returns>
        public bool RebuildTreeIfItIsInvalid()
        {
            // here I use double than normal timeout to give priority over other locks
            if (Monitor.TryEnter(LockObject, LockTimeout * 2))
            {
                try
                {
                    if (IsStructureValid)
                    {
                        return false; // tree structure has not changed
                    }
                    RebuildTreeCompletely();
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

                    IbaSnmpOid oidDrive = driveInfo.Oid;

                    IbaSnmp.SetUserValue(oidDrive + 0, driveInfo.DriveName);
                    IbaSnmp.SetUserValue(oidDrive + 1, driveInfo.Active);
                    IbaSnmp.SetUserValue(oidDrive + 2, driveInfo.SizeInMb);
                    IbaSnmp.SetUserValue(oidDrive + 3, driveInfo.CurrentFreeSpaceInMb);
                    IbaSnmp.SetUserValue(oidDrive + 4, driveInfo.MinFreeSpaceInPercent);
                    IbaSnmp.SetUserValue(oidDrive + 5, driveInfo.RescanTime);
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
                        $"SNMP. Error acquiring lock when updating {driveInfo.DriveName}, {GetCurrentThreadString()}.");
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

                    // TaskManager has updated info successfully 
                    // copy it to snmp tree

                    IbaSnmpOid oidJobGen = jobInfo.Oid + 0;

                    IbaSnmp.SetUserValue(oidJobGen + 0, jobInfo.JobName);
                    IbaSnmp.SetUserValue(oidJobGen + 1, (int) jobInfo.Status);
                    IbaSnmp.SetUserValue(oidJobGen + 2, jobInfo.TodoCount);
                    IbaSnmp.SetUserValue(oidJobGen + 3, jobInfo.DoneCount);
                    IbaSnmp.SetUserValue(oidJobGen + 4, jobInfo.FailedCount);

                    var stdJi = jobInfo as SnmpObjectsData.StandardJobInfo;
                    var schJi = jobInfo as SnmpObjectsData.ScheduledJobInfo;
                    var otJi = jobInfo as SnmpObjectsData.OneTimeJobInfo;
                    if (stdJi != null)
                    {
                        IbaSnmp.SetUserValue(oidJobGen + 5, stdJi.PermFailedCount);
                        IbaSnmp.SetUserValue(oidJobGen + 6, stdJi.TimestampJobStarted);
                        IbaSnmp.SetUserValue(oidJobGen + 7, stdJi.TimestampLastDirectoryScan);
                        IbaSnmp.SetUserValue(oidJobGen + 8, stdJi.TimestampLastReprocessErrorsScan);
                        IbaSnmpOid oidJobGenLastproc = oidJobGen + 9;
                        IbaSnmp.SetUserValue(oidJobGenLastproc + 0, stdJi.LastProcessingLastDatFileProcessed);
                        IbaSnmp.SetUserValue(oidJobGenLastproc + 1, stdJi.LastProcessingStartTimeStamp);
                        IbaSnmp.SetUserValue(oidJobGenLastproc + 2, stdJi.LastProcessingFinishTimeStamp);
                    }
                    else if (schJi != null)
                    {
                        IbaSnmp.SetUserValue(oidJobGen + 5, schJi.PermFailedCount);
                        IbaSnmp.SetUserValue(oidJobGen + 6, schJi.TimestampJobStarted);
                        IbaSnmp.SetUserValue(oidJobGen + 7, schJi.TimestampLastExecution);
                        IbaSnmp.SetUserValue(oidJobGen + 8, schJi.TimestampNextExecution);
                    }
                    else if (otJi != null)
                    {
                        IbaSnmp.SetUserValue(oidJobGen + 5, otJi.TimestampLastExecution);
                    }
                    else
                    {
                        // should not happen
                        throw new Exception("Unknown job type");
                    }

                    // refresh tasks
                    foreach (var taskInfo in jobInfo.Tasks)
                    {
                        RefreshTaskInfo(taskInfo);
                    }
                    return true; // was updated
                }
                catch (Exception ex)
                {
                    LogData.Data.Logger.Log(Level.Exception,
                        $"SNMP. Error during refreshing job {jobInfo.JobName}. {ex.Message}.");
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


        private void RefreshTaskInfo(SnmpObjectsData.TaskInfo taskInfo)
        {
            IbaSnmpOid oidTask = taskInfo.Oid;

            try
            {
                IbaSnmp.SetUserValue(oidTask + 0, taskInfo.TaskName);
                IbaSnmp.SetUserValue(oidTask + 1, taskInfo.TaskType);
                IbaSnmp.SetUserValue(oidTask + 2, taskInfo.Success);
                IbaSnmp.SetUserValue(oidTask + 3, taskInfo.DurationOfLastExecution);
                IbaSnmp.SetUserValue(oidTask + 4, taskInfo.MemoryUsedForLastExecution);

                var ci = taskInfo.CleanupInfo;
                // ReSharper disable once InvertIf
                if (ci != null)
                {
                    IbaSnmpOid oidCleanup = oidTask + 5;

                    IbaSnmp.SetUserValue(oidCleanup + 0, (int)ci.LimitChoice);
                    IbaSnmp.SetUserValue(oidCleanup + 1, ci.Subdirectories);
                    IbaSnmp.SetUserValue(oidCleanup + 2, ci.FreeDiskSpace);
                    IbaSnmp.SetUserValue(oidCleanup + 3, ci.UsedDiskSpace);
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"SNMP. Error during refreshing task {taskInfo.TaskName}. {ex.Message}.");
            }
        }

        public bool RebuildTreeCompletely()
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
                    // snmp structure is valid until datcoordinator configuration is changed.
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
                    BuildSectionGlobalCleanup();

                    // ibaRoot.DatCoord.Product.2 - Standard jobs
                    BuildSectionStandardJobs();

                    // ibaRoot.DatCoord.Product.3 - Scheduled jobs
                    BuildSectionScheduledJobs();

                    // ibaRoot.DatCoord.Product.4 - One time jobs
                    BuildSectionOneTimeJobs();

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

        #region Building of tree Sections 1...4 (from 'GlobalCleanup' to 'OneTimeJobs')

        private void BuildSectionGlobalCleanup()
        {
            var oidSection = _oidSectionGlobalCleanup;

            AddMetadataForOidSuffix(oidSection, @"Global cleanup", @"globalCleanup",
                "Global cleanup settings for all local drives.");

            for (int i = 0; i < ObjectsData.GlobalCleanup.Count; i++)
            {
                try
                {
                    var driveInfo = ObjectsData.GlobalCleanup[i];
                    // ibaRoot.DatCoord.Product.GlobalCleanup.(index) - Drive
                    IbaSnmpOid oidDrive = oidSection + (uint)(i + 1);
                    driveInfo.Oid = oidDrive;

                    string mibNameDrive = $@"globalCleanupDrive{oidDrive.GetLeastId()}";
                    AddMetadataForOidSuffix(oidDrive, $@"Drive '{driveInfo.DriveName}'", mibNameDrive,
                        $@"Global cleanup settings for the drive ‘{driveInfo.DriveName}’.");

                    // ibaRoot.DatCoord.Product.GlobalCleanup.DriveX....
                    {
                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.0 - DriveName
                        CreateUserValue(oidDrive + 0, driveInfo.DriveName,
                            @"Drive Name", mibNameDrive + @"Name",
                            @"Drive name like it appears in operating system.",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.1 - Active
                        CreateUserValue(oidDrive + 1, driveInfo.Active,
                            @"Active", mibNameDrive + @"Active",
                            @"Whether global cleanup is enabled for the drive.",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.2 - Size
                        CreateUserValue(oidDrive + 2, driveInfo.SizeInMb,
                            @"Size", mibNameDrive + @"Size",
                            @"Size of the drive (in megabytes).",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.3 - Curr. free space
                        CreateUserValue(oidDrive + 3, driveInfo.CurrentFreeSpaceInMb,
                            @"Curr. free space", mibNameDrive + @"CurrFreeSpace",
                            @"Current free space of the drive (in megabytes).",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.4 - Min free space
                        CreateUserValue(oidDrive + 4, driveInfo.MinFreeSpaceInPercent,
                            @"Min free space", mibNameDrive + @"MinFreeSpace",
                            @"Minimal free space that should be kept on the drive by deleting iba dat files (in percent).",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.5 - Rescan time
                        CreateUserValue(oidDrive + 5, driveInfo.RescanTime,
                            @"Rescan time", mibNameDrive + @"RescanTime",
                            @"How often the application should rescan the drive parameters (in minutes).",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);
                    }
                }
                catch
                {
                    // ReSharper disable once RedundantJumpStatement
                    continue;
                    // go on with other items 
                    // even if current one has failed 
                }
            }
        }

        private void BuildSectionStandardJobs()
        {
            var oidSection = _oidSectionStandardJobs;

            AddMetadataForOidSuffix(oidSection, @"Standard jobs", @"standardJobs",
                @"List of all standard jobs.");

            for (int i = 0; i < ObjectsData.StandardJobs.Count; i++)
            {
                try
                {
                    SnmpObjectsData.StandardJobInfo jobInfo = ObjectsData.StandardJobs[i];

                    // ibaRoot.DatCoord.Product.StdJobs.(index) - Job
                    IbaSnmpOid oidJob = oidSection + (uint)(i + 1);
                    string mibNameJob = $@"standardJob{oidJob.GetLeastId()}";
                    AddMetadataForOidSuffix(oidJob, $@"Job '{jobInfo.JobName}'", mibNameJob,
                        $@"Properties of standard job ‘{jobInfo.JobName}’.");

                    // create objects that are common for all the job types
                    IbaSnmpOid oidJobGen;
                    string mibNameJobGen;
                    BuildCommonGeneralJobSubsection(
                        oidJob, out oidJobGen,
                        mibNameJob, out mibNameJobGen,
                        jobInfo);

                    // create all the rest of general job objects
                    {
                        // ibaRoot.DatCoord.Product.StdJobs.Job.5 - Perm. Failed #
                        CreateUserValue(oidJobGen + 5, jobInfo.PermFailedCount,
                            @"Perm. Failed #", mibNameJobGen + @"PermFailed",
                            @"Count of files with permanent errors.",
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.6 - Timestamp Job started
                        CreateUserValue(oidJobGen + 6, jobInfo.TimestampJobStarted,
                            @"Timestamp job started", mibNameJobGen + @"TimestampJobStarted",
                            @"Time when job was started. For a stopped job it relates to the last start of the job. " + 
                            @"If job was never started then value is ‘01.01.0001 0:00:00’.",
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.7 - Timestamp Last Directory Scan
                        CreateUserValue(oidJobGen + 7, jobInfo.TimestampLastDirectoryScan,
                            @"Timestamp last directory scan", mibNameJobGen + @"TimestampLastDirectoryScan",
                            @"Time when the last scan for new (unprocessed) .dat files was performed. " + 
                            @"If scan was never performed then value is ‘01.01.0001 0:00:00’.",
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8 - Timestamp Last Reprocess ErrorsScan
                        CreateUserValue(oidJobGen + 8, jobInfo.TimestampLastReprocessErrorsScan,
                            @"Timestamp last reprocess errors scan", mibNameJobGen + @"TimestampLastReprocessErrorsScan",
                            @"Time when the last reprocess scan was performed " +
                            @"(reprocess scan is a scan for .dat files that previously were processed with errors). " + 
                            @"If scan was never performed then value is ‘01.01.0001 0:00:00’.",
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.9 - LastProcessing [Folder]
                        IbaSnmpOid oidLastProc = oidJobGen + 9;
                        AddMetadataForOidSuffix(oidLastProc, @"LastProcessing", mibNameJobGen + @"LastProcessing",
                            @"Information about the last successfully processed file.");

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8.0 - Last dat-File processed
                        CreateUserValue(oidLastProc + 0, jobInfo.LastProcessingLastDatFileProcessed,
                            @"Last dat-file processed", mibNameJobGen + @"LastFile",
                            @"Filename of the last successfully processed file. If no files were successfully processed then value is empty.",
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8.1 - Start Timestamp last processing
                        CreateUserValue(oidLastProc + 1, jobInfo.LastProcessingStartTimeStamp,
                            @"Start timestamp", mibNameJobGen + @"StartStamp",
                            @"Time when processing of the last successfully processed file was started. " +
                            @"If no files were proc-essed then value is ‘01.01.0001 0:00:00’.",
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8.2 - Finish Timestamp last processing
                        CreateUserValue(oidLastProc + 2, jobInfo.LastProcessingFinishTimeStamp,
                            @"Finish timestamp", mibNameJobGen + @"FinishStamp",
                            @"Time when processing of the last successfully processed file was finished. " + 
                            @"If no files were processed then value is ‘01.01.0001 0:00:00’.",
                            JobInfoItemRequested, jobInfo);
                    }
                }
                catch
                {
                    // ReSharper disable once RedundantJumpStatement
                    continue;
                    // go on with other items 
                    // even if current one has failed 
                }
            }
        }

        private void BuildSectionScheduledJobs()
        {
            var oidSection = _oidSectionScheduledJobs;

            AddMetadataForOidSuffix(oidSection, @"Scheduled jobs", @"scheduledJobs",
                @"List of all scheduled jobs.");

            for (int i = 0; i < ObjectsData.ScheduledJobs.Count; i++)
            {
                try
                {
                    var jobInfo = ObjectsData.ScheduledJobs[i];

                    // ibaRoot.DatCoord.Product.SchJobs.(index) - Job
                    IbaSnmpOid oidJob = oidSection + (uint)(i + 1);
                    string mibNameJob = $@"scheduledJob{oidJob.GetLeastId()}";
                    AddMetadataForOidSuffix(oidJob, $@"Job '{jobInfo.JobName}'", mibNameJob,
                        $@"Properties of scheduled job ‘{jobInfo.JobName}’.");

                    // create objects that are common for all the job types
                    IbaSnmpOid oidJobGen;
                    string mibNameJobGen;
                    BuildCommonGeneralJobSubsection(
                        oidJob, out oidJobGen,
                        mibNameJob, out mibNameJobGen,
                        jobInfo);

                    // create all the rest of general job objects
                    {
                        // ibaRoot.DatCoord.Product.SchJobs.Job.5 - Perm. Failed #
                        CreateUserValue(oidJobGen + 5, jobInfo.PermFailedCount,
                            @"Perm. Failed #", mibNameJobGen + @"PermFailedCount",
                            @"Count of files with permanent errors.",
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.SchJobs.Job.6 - TimestampJobStarted
                        CreateUserValue(oidJobGen + 6, jobInfo.TimestampJobStarted,
                            @"Timestamp job started", mibNameJobGen + @"TimestampJobStarted",
                            @"Time when job was started (start-ing of the scheduled job does NOT mean that it will be executed immediately). " +
                            @"For a stopped job it relates to the last start of the job. " + 
                            @"If job was never started then value is ‘01.01.0001 0:00:00’.",
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.SchJobs.Job.7 - TimestampLastExecution
                        CreateUserValue(oidJobGen + 7, jobInfo.TimestampLastExecution,
                            @"Timestamp last execution", mibNameJobGen + @"TimestampLastExecution",
                            @"Time when job was last executed. " +
                            @"(This does not mean the moment when job was started, but the moment when configured trigger was fired last time); " + 
                            @"If job was never executed then value is ‘01.01.0001 0:00:00’.",
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.SchJobs.Job.8 - TimestampNextExecution
                        CreateUserValue(oidJobGen + 8, jobInfo.TimestampNextExecution,
                            @"Timestamp next execution", mibNameJobGen + @"TimestampNextExecution",
                            @"Time when the next execution is scheduled. " + 
                            @"If there is no execution scheduled then value is ‘01.01.0001 0:00:00’.",
                            JobInfoItemRequested, jobInfo);
                    }
                }
                catch
                {
                    // ReSharper disable once RedundantJumpStatement
                    continue;
                    // go on with other items 
                    // even if current one has failed 
                }
            }
        }

        private void BuildSectionOneTimeJobs()
        {
            var oidSection = _oidSectionOneTimeJobs;
            AddMetadataForOidSuffix(oidSection, @"One time jobs", @"oneTimeJobs",
                @"List of all one-time jobs.");

            for (int i = 0; i < ObjectsData.OneTimeJobs.Count; i++)
            {
                try
                {
                    var jobInfo = ObjectsData.OneTimeJobs[i];
                    // ibaRoot.DatCoord.Product.OtJobs.(index) - Job
                    IbaSnmpOid oidJob = oidSection + (uint)(i + 1);
                    string mibNameJob = $@"oneTimeJob{oidJob.GetLeastId()}";
                    AddMetadataForOidSuffix(oidJob,  $@"Job '{jobInfo.JobName}'", mibNameJob,
                        $@"Properties of one-time job ‘{jobInfo.JobName}’.");

                    // create objects that are common for all the job types
                    IbaSnmpOid oidJobGen;
                    string mibNameJobGen;
                    BuildCommonGeneralJobSubsection(
                        oidJob, out oidJobGen,
                        mibNameJob, out mibNameJobGen,
                        jobInfo);


                    // create all the rest of general job objects
                    {
                        // ibaRoot.DatCoord.Product.OtJobs.Job.5 - TimestampLastExecution
                        CreateUserValue(oidJobGen + 5, jobInfo.TimestampLastExecution,
                            @"Timestamp last execution", mibNameJobGen + @"TimestampLastExecution",
                            @"Time when the last execution was started. " +
                            @"If job was never executed then value is ‘01.01.0001 0:00:00’.",
                            JobInfoItemRequested, jobInfo);
                    }
                }
                catch
                {
                    // ReSharper disable once RedundantJumpStatement
                    continue;
                    // go on with other items 
                    // even if current one has failed 
                }
            }
        }

        #endregion


        #region helper functions for building the tree - Common Subsections, Tasks, CreateUserValue overloads

        #region Common for all the jobs

        /// <summary> Build the part that is common for all the Jobs 
        /// (items that are present in the base class SnmpObjectsData.JobInfoBase)  </summary>
        private void BuildCommonGeneralJobSubsection(
            IbaSnmpOid oidJob, out IbaSnmpOid oidJobGen,
            string mibNameJob, out string mibNameJobGen,
            SnmpObjectsData.JobInfoBase jobInfo)
        {

            jobInfo.Oid = oidJob;

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.0 - General [Folder]
            oidJobGen = oidJob + 0;
            mibNameJobGen = mibNameJob + @"General";
            AddMetadataForOidSuffix(oidJobGen,@"General", mibNameJobGen,
                $@"General properties of job ‘{jobInfo.JobName}’.");

            {
                // ibaRoot.DatCoord.Product.XxxJobs.JobY.0 - Job name
                CreateUserValue(oidJobGen + 0, jobInfo.JobName,
                    @"Job Name", mibNameJobGen + @"Name",
                    @"The name of the job as it appears in GUI.",
                    JobInfoItemRequested, jobInfo);

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.1 - Status
                CreateEnumUserValue(oidJobGen + 1, _enumJobStatus, (int)jobInfo.Status,
                    @"Status", mibNameJobGen + @"Status",
                    @"Current status of the job (started, stopped or disabled).",
                    JobInfoItemRequested, jobInfo);

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.2 - To do #
                CreateUserValue(oidJobGen + 2, jobInfo.TodoCount,
                    @"Todo #", mibNameJobGen + @"Todo",
                    @"Count of dat files to be processed.",
                    JobInfoItemRequested, jobInfo);

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.3 - Done #
                CreateUserValue(oidJobGen + 3, jobInfo.DoneCount,
                    @"Done #", mibNameJobGen + @"Done",
                    @"Count of processed files.",
                    JobInfoItemRequested, jobInfo);

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.4 - Failed #
                CreateUserValue(oidJobGen + 4, jobInfo.FailedCount,
                    @"Failed #", mibNameJobGen + @"Failed",
                    @"Count of errors occurred during processing.",
                    JobInfoItemRequested, jobInfo);
            }

            // create tasks
            BuildTasks(oidJob, mibNameJob, jobInfo);
        }

        #endregion


        #region Tasks subtrees

        private void BuildTasks(IbaSnmpOid oidjJob, string mibNameJob, SnmpObjectsData.JobInfoBase jobInfo)
        {
            var tasks = jobInfo?.Tasks;
            if (tasks == null)
            {
                return;
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                SnmpObjectsData.TaskInfo taskInfo = tasks[i];

                uint i1 = (uint)(i + 1); // index for mib

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.(index) - Task [Folder]
                AddMetadataForOidSuffix(oidjJob + i1, $@"Task '{taskInfo.TaskName}'", mibNameJob + $@"Task{i1}",
                    $@"Information about task ‘{taskInfo.TaskName}’ of job ‘{jobInfo.JobName}’.");

                // create task contents
                // ibaRoot.DatCoord.Product.XxxJobs.JobY.(index).(contents)
                try
                {
                    BuildTask(oidjJob + i1, mibNameJob + $@"Task{i1}", taskInfo);
                }
                catch
                {
                    // ReSharper disable once RedundantJumpStatement
                    continue;
                    // go on with other tasks 
                    // even if some task has failed 
                }
            }
        }

        private void BuildTask(IbaSnmpOid oidTask, string mibNameTask, SnmpObjectsData.TaskInfo taskInfo)
        {
            var parentJob = taskInfo.Parent;

            taskInfo.Oid = oidTask;

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.0 - TaskName
            CreateUserValue(oidTask + 0, taskInfo.TaskName,
                @"Task name", mibNameTask + @"Name",
                @"The name of the task as it appears in GUI.",
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.1 - Tasktype 
            CreateUserValue(oidTask + 1, taskInfo.TaskType,
                @"Task type", mibNameTask + @"Type",
                @"The type of the task (copy, extract, report, etc.).",
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.2 - Success 
            CreateUserValue(oidTask + 2, taskInfo.Success,
                @"Success", mibNameTask + @"Success",
                @"Whether the last executed task was completed successfully, i.e. without errors. " + 
                @"For Condition task this means that expression was successfully evaluated as TRUE or FALSE – both results are treated as success.",
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.3 - DurationOfLastExecution 
            CreateUserValue(oidTask + 3, taskInfo.DurationOfLastExecution,
                @"Duration of last execution", mibNameTask + @"DurationOfLastExecution",
                @"Last execution time of the task (in seconds).",
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.4 - CurrentMemoryUsed 
            CreateUserValue(oidTask + 4, taskInfo.MemoryUsedForLastExecution,
                @"Memory used for last execution", mibNameTask + @"LastMemoryUsed",
                @"Used memory during the last execution of the task (in megabytes). " + 
                @"This is applicable only to tasks that use ibaAnalyzer for their processing e.g., Condition, Report, Extract and some custom tasks.",
                JobInfoItemRequested, parentJob);

            var ci = taskInfo.CleanupInfo;
            if (ci == null)
            {
                return;
            }

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5 - Cleanup [Folder]
            IbaSnmpOid oidCleanup = oidTask + 5;
            string mibNameCleanup = mibNameTask + @"Cleanup";
            AddMetadataForOidSuffix(oidCleanup, @"Cleanup", mibNameCleanup,
                @"Cleanup parameters of the task.");

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.0 - LimitChoice 
            CreateEnumUserValue(oidCleanup + 0, _enumCleanupType, (int)ci.LimitChoice,
                @"Limit choice", mibNameCleanup + @"LimitChoice",
                @"Selected option as limit for the disk space usage.",
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.1 - Subdirectories
            CreateUserValue(oidCleanup + 1, ci.Subdirectories,
                @"Subdirectories", mibNameCleanup + @"Subdirectories",
                @"Maximum count of directories the task can use.",
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.2 - FreeDiskSpace
            CreateUserValue(oidCleanup + 2, ci.FreeDiskSpace,
                @"Free disk space", mibNameCleanup + @"FreeDiskSpace",
                @"Minimum disk space that should stay free (in megabytes).",
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.3 - UsedDiskSpace
            CreateUserValue(oidCleanup + 3, ci.UsedDiskSpace,
                @"Used disk space", mibNameCleanup + @"UsedDiskSpace",
                @"Maximum disk space that can be used by the task (in megabytes).",
                JobInfoItemRequested, parentJob);
        }

        #endregion


        #region Oid metadata and CreateUserValue() overloads

        private void AddMetadataForOidSuffix(IbaSnmpOid oidSuffix, string guiCaption, string mibName, string mibDescription)
        {
            IbaSnmp.SetUserOidMetadata(oidSuffix, mibName, mibDescription, guiCaption);
        }

        private void CreateUserValue(IbaSnmpOid oidSuffix, bool initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<IbaSnmpObjectValueRequestedEventArgs> handler = null,
            object tag = null)
        {
            IbaSnmp.CreateUserValue(oidSuffix, initialValue, null, null, handler, tag);
            IbaSnmp.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        private void CreateUserValue(IbaSnmpOid oidSuffix, string initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<IbaSnmpObjectValueRequestedEventArgs> handler = null,
            object tag = null)
        {
            IbaSnmp.CreateUserValue(oidSuffix, initialValue, null, null, handler, tag);
            IbaSnmp.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        // ReSharper disable once UnusedMember.Local
        private void CreateUserValue(IbaSnmpOid oidSuffix, int initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<IbaSnmpObjectValueRequestedEventArgs> handler = null,
            object tag = null)
        {
            IbaSnmp.CreateUserValue(oidSuffix, initialValue, null, null, handler, tag);
            IbaSnmp.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        private void CreateUserValue(IbaSnmpOid oidSuffix, uint initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<IbaSnmpObjectValueRequestedEventArgs> handler = null,
            object tag = null)
        {
            IbaSnmp.CreateUserValue(oidSuffix, initialValue, null, null, handler, tag);
            IbaSnmp.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }
        private void CreateUserValue(IbaSnmpOid oidSuffix, DateTime initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<IbaSnmpObjectValueRequestedEventArgs> handler = null,
            object tag = null)
        {
            IbaSnmp.CreateUserValue(oidSuffix, initialValue, 
                UseSnmpV2TcForStrings ? IbaSnmpValueType.DateTimeTc : IbaSnmpValueType.DateTimeStr,
                null, null, handler, tag);
            IbaSnmp.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        private void CreateEnumUserValue(IbaSnmpOid oidSuffix, IbaSnmpValueType valueType, int initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<IbaSnmpObjectValueRequestedEventArgs> handler = null,
            object tag = null)
        {
            IbaSnmp.CreateEnumUserValue(oidSuffix, valueType, initialValue, null, null, handler, tag);
            IbaSnmp.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        #endregion

        #endregion

        private void GlobalCleanupDriveInfoItemRequested(object sender, IbaSnmpObjectValueRequestedEventArgs args)
        {
            var driveInfo = args.Tag as SnmpObjectsData.GlobalCleanupDriveInfo;

            if (driveInfo == null)
            {
                // should not happen
                args.Value = null;
                return;
            }

            // refresh data if it is too old (or rebuild the whole tree if necessary)
            RefreshGlobalCleanupDriveInfo(driveInfo);

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            args.Value = args.IbaSnmp.GetValue(args.Oid);
        }


        private void JobInfoItemRequested(object sender, IbaSnmpObjectValueRequestedEventArgs args)
        {
            var jobInfo = args.Tag as SnmpObjectsData.JobInfoBase;

            if (jobInfo == null)
            {
                // should not happen
                args.Value = null;
                return;
            }

            // refresh data if it is too old (or rebuild the whole tree if necessary)
            RefreshJobInfo(jobInfo);

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            args.Value = args.IbaSnmp.GetValue(args.Oid);
        }

        #endregion

        #endregion

    }
}
