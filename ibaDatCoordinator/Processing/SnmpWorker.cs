using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Data;
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
        private const int InitialisationDelayInSeconds = 1;

        #region tmpLog

        public static string _tmpLog = "";

        public static void TmpLog(string str)
        {
            _tmpLog += " " + str;
        }

        public static void TmpLogLine(string str)
        {
            _tmpLog += $"\r\n{DateTime.Now.ToLongTimeString()} {str}";
        }

        #endregion


        #region Construction, Destruction, Init

        public SnmpWorker()
        {
            Status = SnmpWorkerStatus.Stopped;
            // todo localize
            StatusString = "Waiting for delayed initialisation...";

            new Task(DelayedInit).Start();
        }

        private void DelayedInit()
        {
            for (int i = InitialisationDelayInSeconds - 1; i >= 0; i--)
            {
                Thread.Sleep(1000);
                // todo localize
                StatusString = $"Waiting for delayed initialisation, {i} second(s)...";
                StatusChanged?.Invoke(this,
                    new SnmpWorkerStatusChangedEventArgs(Status, StatusToColor(Status), StatusString));
            }

            Init();
        }

        public void Init()
        {
            IbaSnmp = new IbaSnmp(IbaSnmpProductId.IbaDatCoordinator);
            IbaSnmp.DosProtectionInternal.Enabled = false;
            IbaSnmp.DosProtectionExternal.Enabled = false;
            RestartAgent();

            TaskManager.Manager.SnmpConfigurationChanged += TaskManager_SnmpConfigurationChanged;
            SnmpObjectsData.SnmpObjectWithATimeStamp.AgeThreshold = SnmpObjectsDataValidTimePeriod;

            RegisterGeneralObjectHandlers();
            RebuildTreeCompletely();
        }

        public void TaskManager_SnmpConfigurationChanged(object sender, EventArgs e)
        {
            TmpLogLine("TaskManager_SnmpConfigurationChanged");

            // tree marked as invalid
            // it will be rebuilt on 1st request to any existing user node
            lock (LockObject)
            {
                ObjectsData.IsStructureValid = false;
            }

            // todo
            // it's better if we improve IbaSnmp - add generic UserValueRequested event to it
            // and in it's handler here check for (ObjectsData.IsStructureValid == false)
            // otherwise tree is not rebuilt if some value is requested that 
            // was not existing before, but exists now.
            // rare case, but nevertheless.
            // normally user will perform walk or load MIBs to his manager prior to requesting
            // so we should not have problems. 
            // but to be absolutely confident, it's better to do it

            //// Rebuilding it every time is not effective
            ////RebuildTreeCompletely();
        }

        #endregion

        public IbaSnmp IbaSnmp { get; private set; }

        public event EventHandler<SnmpWorkerStatusChangedEventArgs> StatusChanged;

        private SnmpData _snmpData;

        public SnmpData SnmpData
        {
            get { return _snmpData; }
            set
            {
                // todo probabaly override Equals, because def implementation does not work good enough for this
                //  if (_snmpData != null && _snmpData.Equals(value))
                if (_snmpData != null && _snmpData.Equals(value))
                {
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

        /// <summary> Whether Snmp Control is currently displayed in the main form window </summary>
        //public bool IsGuiVisible { get; set; }

        public void RestartAgent()
        {
            Status = SnmpWorkerStatus.Errored;
            StatusString = @"";

            try
            {
                IbaSnmp.Stop();
                Status = SnmpWorkerStatus.Stopped;
                // todo localize
                StatusString = "SNMP server is disabled";

                ApplyConfigurationToIbaSnmp();

                if (_snmpData.Enabled)
                {
                    IbaSnmp.Start();
                    Status = SnmpWorkerStatus.Started;

                    // todo localize
                    StatusString = $"SNMP server running on port {_snmpData.Port}";
                }
            }
            catch (Exception ex)
            {
                Status = SnmpWorkerStatus.Errored;
                // todo localize
                StatusString = $"Starting the SNMP server failed with error: {ex.Message}";
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

            // todo apply objects
            //SnmpData.
        }

        #region Objects

        private IbaSnmpOid _oidSectionGlobalCleanup = "1";
        private IbaSnmpOid _oidSectionStandardJobs = "2";
        private IbaSnmpOid _oidSectionScheduledJobs = "3";
        private IbaSnmpOid _oidSectionOneTimeJobs = "4";

        internal struct OidMetadata
        {
            public string GuiCaption { get; set; }
            public string MibName { get; set; }
            public string MibDescription { get; set; }

            public OidMetadata(string guiCaption, string mibName = null, string mibDescription = null)
            {
                GuiCaption = guiCaption;
                MibName = mibName;
                MibDescription = mibDescription;
            }
        }

        /// <summary> 
        /// Is filled by SnmpWorker during creation of the object tree. 
        /// Is read by SnmpControl for displaying the captions of folder-nodes
        /// </summary>
        internal Dictionary<IbaSnmpOid, OidMetadata> OidMetadataDict { get; } = new Dictionary<IbaSnmpOid, OidMetadata>();

        internal SnmpObjectsData ObjectsData { get; } = new SnmpObjectsData();

        private void PrepareOidDescriptions()
        {
            lock (LockObject)
            {
                OidMetadataDict.Clear();

                // ibaRoot
                OidMetadataDict[IbaSnmp.OidIbaRoot] = new OidMetadata(IbaSnmp.OidIbaRoot.ToString()); // caption here is just an oid
                {
                    // ibaRoot.0 - Library
                    OidMetadataDict[IbaSnmp.OidIbaSnmpLibInfo] = new OidMetadata(@"Library");
                    {
                        // ibaRoot.Library.1 - Name
                        OidMetadataDict[IbaSnmp.OidIbaSnmpLibName] = new OidMetadata(@"Name");
                        // ibaRoot.Library.2 - Version
                        OidMetadataDict[IbaSnmp.OidIbaSnmpLibVersion] = new OidMetadata(@"Version");
                        // ibaRoot.Library.3 - Hostname
                        OidMetadataDict[IbaSnmp.OidIbaSnmpHostname] = new OidMetadata(@"Hostname");
                        // ibaRoot.Library.4 - SystemTime
                        OidMetadataDict[IbaSnmp.OidIbaSnmpSystemTime] = new OidMetadata(@"System time");
                    }
                    // ibaRoot.2 - DatCoordinator
                    OidMetadataDict[IbaSnmp.OidIbaProduct] = new OidMetadata(@"ibaDatCoordinator");
                }
            }
        }

        #region General objects

        private void RegisterGeneralObjectHandlers()
        {
            PrepareOidDescriptions();

            // ibaRoot.DatCoord.0 - General
            OidMetadataDict[IbaSnmp.OidIbaProductGeneral] = new OidMetadata(@"General");
            {
                // ibaRoot.DatCoord.General.1 - Title
                OidMetadataDict[IbaSnmp.OidIbaProductGeneralTitle] = new OidMetadata(@"Title");
                IbaSnmp.ValueIbaProductGeneralTitle = @"ibaDatCoordinator";

                // ibaRoot.DatCoord.General.2 - Version
                OidMetadataDict[IbaSnmp.OidIbaProductGeneralVersion] = new OidMetadata(@"Version");
                var ver = GetType().Assembly.GetName().Version;
                IbaSnmp.SetValueIbaProductGeneralVersion(ver.Major, ver.Minor, ver.Build, null);

                // ibaRoot.DatCoord.General.3 - Licensing
                OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensing] = new OidMetadata(@"Licensing");
                {
                    // will not be displayed in the tree; so, no caption
                    IbaSnmp.UpTimeRequested += IbaSnmp_UpTimeRequested;

                    // ibaRoot.DatCoord.General.Licensing.1 - IsValid
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingIsValid] = new OidMetadata(@"Is Valid");
                    IbaSnmp.LicensingIsValidRequested += IbaSnmp_LicensingValueRequested;

                    // ibaRoot.DatCoord.General.Licensing.2 - Serial number
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingSn] = new OidMetadata(@"Serial number");
                    IbaSnmp.LicensingSnRequested += IbaSnmp_LicensingValueRequested;

                    // ibaRoot.DatCoord.General.Licensing.3 - Hardware ID
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingHwId] = new OidMetadata(@"Hardware ID");
                    IbaSnmp.LicensingHwIdRequested += IbaSnmp_LicensingValueRequested;

                    // ibaRoot.DatCoord.General.Licensing.4 - Dongle type
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingType] = new OidMetadata(@"Dongle type");
                    IbaSnmp.LicensingTypeRequested += IbaSnmp_LicensingValueRequested;

                    // ibaRoot.DatCoord.General.Licensing.5 - Customer
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingCustomer] = new OidMetadata(@"Customer");
                    IbaSnmp.LicensingCustomerRequested += IbaSnmp_LicensingValueRequested;

                    // ibaRoot.DatCoord.General.Licensing.6 - Time limit
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingTimeLimit] = new OidMetadata(@"Time limit");
                    IbaSnmp.LicensingTimeLimitRequested += IbaSnmp_LicensingValueRequested;

                    // ibaRoot.DatCoord.General.Licensing.7 - Demo time limit
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingDemoTimeLimit] = new OidMetadata(@"Demo time limit");
                    IbaSnmp.LicensingDemoTimeLimitRequested += IbaSnmp_LicensingValueRequested;
                }
            }
        }

        private void IbaSnmp_UpTimeRequested(object sender, IbaSnmpValueRequestedEventArgs<uint> e)
        {
            // todo override?
        }


        private void IbaSnmp_LicensingValueRequested<T>(object sender, IbaSnmpValueRequestedEventArgs<T> args)
        {
            // refresh data if it is too old 
            RefreshLicenseInfo();
            // re-read the value and send it back via args
            args.Value = (T)args.IbaSnmp.GetValue(args.Oid);
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
            lock (LockObject)
            {
                if (ObjectsData.IsStructureValid)
                {
                    return false; // tree structure has not changed
                }
                RebuildTreeCompletely();
                return true; // tree structure has changed
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="driveInfo"></param>
        /// <returns>sdadf</returns>
        private bool RefreshLicenseInfo()
        {
            lock (LockObject)
            {
                if (ObjectsData.License.IsUpToDate())
                {
                    // data fresh, no need to change something
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

                TmpLogLine("SnmpWrkr. Refreshed License");
            }

            return true; // data was updated
        }
        
        /// <summary>
                 /// 
                 /// </summary>
                 /// <param name="driveInfo"></param>
                 /// <returns>sdadf</returns>
        private bool RefreshGlobalCleanupDriveInfo(SnmpObjectsData.GlobalCleanupDriveInfo driveInfo)
        {
            lock (LockObject)
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
                    // data fresh, no need to change something
                    return false; // was not updated
                }

                var man = TaskManager.Manager;
                if (!man.SnmpRefreshGlobalCleanupDriveInfo(driveInfo))
                {
                    // should not happen
                    // failed to update data
                    // rebuild the tree
                    TmpLogLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    TmpLogLine("Error RefreshGlobalCleanupDriveInfo()");
                    RebuildTreeCompletely();
                    return true; // data was updated
                }

                // TaskManager has updated driveInfo successfully 
                // copy it to snmp tree

                IbaSnmpOid oidDrive = driveInfo.Oid;

                IbaSnmp.SetUserValue(oidDrive + 0, driveInfo.DriveName);
                IbaSnmp.SetUserValue(oidDrive + 1, driveInfo.Active);
                IbaSnmp.SetUserValue(oidDrive + 2, driveInfo.Size);
                IbaSnmp.SetUserValue(oidDrive + 3, driveInfo.CurrentFreeSpace);
                IbaSnmp.SetUserValue(oidDrive + 4, driveInfo.MinFreeSpace);
                IbaSnmp.SetUserValue(oidDrive + 5, driveInfo.RescanTime);

                TmpLogLine($"SnmpWrkr. Refreshed Drive {driveInfo.DriveName}");
            }

            return true; // data was updated
        }

        private bool RefreshJobInfo(SnmpObjectsData.JobInfoBase jobInfo)
        {
            lock (LockObject)
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
                    // data fresh, no need to change something
                    return false; // data was NOT updated
                }

                var man = TaskManager.Manager;
                if (!man.SnmpRefreshJobInfo(jobInfo))
                {
                    // should not happen
                    // failed to update data
                    // rebuild the tree
                    TmpLogLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    TmpLogLine("Error RefreshJobInfo()");
                    RebuildTreeCompletely();
                    return true; // data was updated
                }

                // TaskManager has updated info successfully 
                // copy it to snmp tree

                IbaSnmpOid oidJobGen = jobInfo.Oid + 0;

                try
                {
                    IbaSnmp.SetUserValue(oidJobGen + 0, jobInfo.JobName);
                    IbaSnmp.SetUserValue(oidJobGen + 1, jobInfo.Status.ToString());
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
                        IbaSnmp.SetUserValue(oidJobGen + 7, stdJi.LastCycleScanningTime);
                        IbaSnmp.SetUserValue(oidJobGen + "8.0", stdJi.LastProcessingLastDatFileProcessed);

                        var x = IbaSnmp.GetUserValue(oidJobGen + "8.0");

                        IbaSnmp.SetUserValue(oidJobGen + "8.1", stdJi.LastProcessingStartTimeStamp);
                        IbaSnmp.SetUserValue(oidJobGen + "8.2", stdJi.LastProcessingFinishTimeStamp);
                    }
                    else if (schJi != null)
                    {
                        IbaSnmp.SetUserValue(oidJobGen + 5, schJi.PermFailedCount);
                        IbaSnmp.SetUserValue(oidJobGen + 6, schJi.TimestampLastExecution);
                        IbaSnmp.SetUserValue(oidJobGen + 7, schJi.TimestampNextExecution);
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
                    TmpLogLine($"Refreshing tasks(");
                    int _debug_i_cnt = 0;
                    foreach (var taskInfo in jobInfo.Tasks)
                    {
                        RefreshTaskInfo(taskInfo);
                        TmpLog($"{++_debug_i_cnt}");
                    }
                    TmpLog(") ok");
                }
                catch (KeyNotFoundException ex)
                {
                    // value does not exist
                    // todo log?    
                    TmpLogLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    TmpLogLine($"SnmpWrkr. Error in Job {jobInfo.JobName}");
                }
                catch (FormatException ex)
                {
                    // wrong type
                    // todo log?    
                    TmpLogLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    TmpLogLine($"SnmpWrkr. Error in Job {jobInfo.JobName}");
                }

                // todo update tasks or separately?
                //IbaSnmp.SetUserValue(oidJob + 5, jobInfo.RescanTimeId5);

                TmpLogLine($"SnmpWrkr. Refreshed Job {jobInfo.JobName}");
            }
            return true; // was updated
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
                IbaSnmp.SetUserValue(oidTask + 4, taskInfo.CurrentMemoryUsed);

                var ci = taskInfo.CleanupInfo;
                if (ci != null)
                {
                    IbaSnmpOid oidCleanup = oidTask + 5;

                    IbaSnmp.SetUserValue(oidCleanup + 0, ci.LimitChoice.ToString());
                    IbaSnmp.SetUserValue(oidCleanup + 1, ci.Subdirectories);
                    IbaSnmp.SetUserValue(oidCleanup + 2, ci.FreeDiskSpace);
                    IbaSnmp.SetUserValue(oidCleanup + 3, ci.UsedDiskSpace);
                }
            }
            catch (KeyNotFoundException ex)
            {
                // value does not exist
                // todo log?    
                TmpLogLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                TmpLogLine($"SnmpWrkr. Error in Task {taskInfo.TaskName}");
            }
            catch (FormatException ex)
            {
                // wrong type
                // todo log?    
                TmpLogLine("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                TmpLogLine($"SnmpWrkr. Error in Task {taskInfo.TaskName}");
            }
        }

        private void RebuildTreeCompletely()
        {
            SnmpWorker.TmpLogLine("*****************************");
            SnmpWorker.TmpLogLine("SnmpWrkr. RebuildTreeCompletely");

            var man = TaskManager.Manager;
            IbaSnmp ibaSnmp = man?.SnmpWorker.IbaSnmp;
            if (ibaSnmp == null)
                return;

            lock (LockObject)
            {
                man.SnmpRebuildObjectsData(ObjectsData);

                // todo this is a probabale dead lock?
                // todo needa look where from does this call comes? 
                // todo wheter we are in lock of ibaSnmp.Dict...
                ibaSnmp.DeleteAllUserValues();

                // todo Clean OidCaptions for all product-specific OIDs

                // ibaRoot.DatCoord.1 - Product-Specific
                OidMetadataDict[IbaSnmp.OidIbaProductSpecific] = new OidMetadata(@"Product");

                // ibaRoot.DatCoord.Product.1 - Global cleanup
                BuildSectionGlobalCleanup();

                // ibaRoot.DatCoord.Product.2 - Standard jobs
                BuildSectionStandardJobs();

                // ibaRoot.DatCoord.Product.3 - Scheduled jobs
                BuildSectionScheduledJobs();

                // ibaRoot.DatCoord.Product.4 - One time jobs
                BuildSectionOneTimeJobs();
            }

            SnmpWorker.TmpLogLine("*****************************");

        }


        #region Building of tree Sections 1...4 (from 'GlobalCleanup' to 'OneTimeJobs')

        private void BuildSectionGlobalCleanup()
        {
            var oidSection = _oidSectionGlobalCleanup;

            AddMetadataForOidSuffix(oidSection, @"Global cleanup", @"globalCleanup");

            for (int i = 0; i < ObjectsData.GlobalCleanup.Count; i++)
            {
                try
                {
                    var driveInfo = ObjectsData.GlobalCleanup[i];
                    // ibaRoot.DatCoord.Product.GlobalCleanup.(index) - Drive
                    IbaSnmpOid oidDrive = oidSection + (uint)(i+1);
                    driveInfo.Oid = oidDrive;
                    
                    string mibNameDrive = $@"globalCleanupDrive{oidDrive.GetLeastId()}";
                    AddMetadataForOidSuffix(oidDrive, $@"Drive '{driveInfo.DriveName}'", mibNameDrive);

                    // ibaRoot.DatCoord.Product.GlobalCleanup.DriveX....
                    {
                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.0 - DriveName
                        CreateUserValue(oidDrive + 0, driveInfo.DriveName,
                            @"Drive Name", mibNameDrive + @"Name",
                            null,
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.1 - Active
                        CreateUserValue(oidDrive + 1, driveInfo.Active,
                            @"Active", mibNameDrive + @"Active",
                            null,
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.2 - Size
                        CreateUserValue(oidDrive + 2, driveInfo.Size,
                            @"Size", mibNameDrive + @"Size",
                            null,
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.3 - Curr. free space
                        CreateUserValue(oidDrive + 3, driveInfo.CurrentFreeSpace,
                            @"Curr. free space", mibNameDrive + @"CurrFreeSpace",
                            null,
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.4 - Min free space
                        CreateUserValue(oidDrive + 4, driveInfo.MinFreeSpace,
                            @"Min free space", mibNameDrive + @"MinFreeSpace",
                            null,
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.5 - Rescan time
                        CreateUserValue(oidDrive + 5, driveInfo.RescanTime,
                            @"Rescan time", mibNameDrive + @"RescanTime",
                            null,
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

            AddMetadataForOidSuffix(oidSection, @"Standard jobs", @"standardJobs");

            for (int i = 0; i < ObjectsData.StandardJobs.Count; i++)
            {
                try
                {
                    SnmpObjectsData.StandardJobInfo jobInfo = ObjectsData.StandardJobs[i];

                    // ibaRoot.DatCoord.Product.StdJobs.(index) - Job
                    IbaSnmpOid oidJob = oidSection + (uint)(i + 1);
                    string mibNameJob = $@"standardJob{oidJob.GetLeastId()}";
                    AddMetadataForOidSuffix(oidJob, $@"Job '{jobInfo.JobName}'", mibNameJob);

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
                            null,
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.6 - Timestamp Job started
                        CreateUserValue(oidJobGen + 6, jobInfo.TimestampJobStarted,
                            @"Timestamp job started", mibNameJobGen + @"TimestampJobStarted",
                            null,
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.7 - Last cycle scanning time
                        CreateUserValue(oidJobGen + 7, jobInfo.LastCycleScanningTime,
                            @"Last cycle scanning time", mibNameJobGen + @"LastCycleScanningTime",
                            null,
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8 - LastProcessing [Folder]
                        IbaSnmpOid oidLastProc = oidJobGen + 8;
                        AddMetadataForOidSuffix(oidLastProc, @"LastProcessing", mibNameJobGen + @"LastProcessing");

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8.0 - Last dat-File processed
                        CreateUserValue(oidLastProc + 0, jobInfo.LastProcessingLastDatFileProcessed,
                            @"Last dat-file processed", mibNameJobGen + @"LastFile",
                            null,
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8.1 - Start Timestamp last processing
                        CreateUserValue(oidLastProc + 1, jobInfo.LastProcessingStartTimeStamp,
                            @"Start timestamp", mibNameJobGen + @"StartStamp",
                            null,
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8.2 - Finish Timestamp last processing
                        CreateUserValue(oidLastProc + 2, jobInfo.LastProcessingFinishTimeStamp,
                            @"Finish timestamp", mibNameJobGen + @"FinishStamp",
                            null,
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

            AddMetadataForOidSuffix(oidSection, @"Scheduled jobs", @"scheduledJobs");

            for (int i = 0; i < ObjectsData.ScheduledJobs.Count; i++)
            {
                try
                {
                    var jobInfo = ObjectsData.ScheduledJobs[i];

                    // ibaRoot.DatCoord.Product.SchJobs.(index) - Job
                    IbaSnmpOid oidJob = oidSection + (uint)(i + 1);
                    string mibNameJob = $@"scheduledJob{oidJob.GetLeastId()}";
                    AddMetadataForOidSuffix(oidJob, $@"Job '{jobInfo.JobName}'", mibNameJob);

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
                            null,
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.SchJobs.Job.6 - TimestampLastExecution
                        CreateUserValue(oidJobGen + 6, jobInfo.TimestampLastExecution,
                            @"Timestamp last execution", mibNameJobGen + @"TimestampLastExecution",
                            null,
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.SchJobs.Job.7 - TimestampNextExecution
                        CreateUserValue(oidJobGen + 7, jobInfo.TimestampNextExecution,
                            @"Timestamp next execution", mibNameJobGen + @"TimestampNextExecution",
                            null,
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
            AddMetadataForOidSuffix(oidSection, @"One time jobs", @"oneTimeJobs");

            for (int i = 0; i < ObjectsData.OneTimeJobs.Count; i++)
            {
                try
                {
                    var jobInfo = ObjectsData.OneTimeJobs[i];
                    // ibaRoot.DatCoord.Product.OtJobs.(index) - Job
                    IbaSnmpOid oidJob = oidSection + (uint)(i + 1);
                    string mibNameJob = $@"oneTimeJob{oidJob.GetLeastId()}";
                    AddMetadataForOidSuffix(oidJob, $@"Job '{jobInfo.JobName}'", mibNameJob);

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
                            null,
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
            AddMetadataForOidSuffix(oidJobGen, @"General", mibNameJobGen);

            {
                // ibaRoot.DatCoord.Product.XxxJobs.JobY.0 - Job name
                CreateUserValue(oidJobGen + 0, jobInfo.JobName,
                    @"Job Name", mibNameJobGen + @"Name",
                    null,
                    JobInfoItemRequested, jobInfo);

                // todo add enum
                // ibaRoot.DatCoord.Product.XxxJobs.JobY.1 - Status
                CreateUserValue(oidJobGen + 1, jobInfo.Status.ToString(),
                    @"Status", mibNameJobGen + @"Status",
                    null,
                    JobInfoItemRequested, jobInfo);

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.2 - To do #
                CreateUserValue(oidJobGen + 2, jobInfo.TodoCount,
                    @"Todo #", mibNameJobGen + @"Todo",
                    null,
                    JobInfoItemRequested, jobInfo);

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.3 - Done #
                CreateUserValue(oidJobGen + 3, jobInfo.DoneCount,
                    @"Done #", mibNameJobGen + @"Done",
                    null,
                    JobInfoItemRequested, jobInfo);

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.4 - Failed #
                CreateUserValue(oidJobGen + 4, jobInfo.FailedCount,
                    @"Failed #", mibNameJobGen + @"Failed",
                    null,
                    JobInfoItemRequested, jobInfo);
            }

            // create tasks
            BuildTasks(oidJob, mibNameJob, jobInfo.Tasks);
        }

        #endregion


        #region Tasks subtrees

        private void BuildTasks(IbaSnmpOid oidjJob, string mibNameJob,
            List<SnmpObjectsData.TaskInfo> tasks)
        {
            if (tasks == null)
            {
                return;
            }

            for (int i = 0; i < tasks.Count; i++)
            {
                SnmpObjectsData.TaskInfo taskInfo = tasks[i];

                uint i1 = (uint)(i + 1); // index for mib

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.(index) - Task [Folder]
                AddMetadataForOidSuffix(oidjJob + i1, $@"Task '{taskInfo.TaskName}'", mibNameJob + $@"Task{i1}");

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
                null,
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.1 - Tasktype 
            CreateUserValue(oidTask + 1, taskInfo.TaskType,
                @"Task type", mibNameTask + @"Type",
                null,
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.2 - Success 
            CreateUserValue(oidTask + 2, taskInfo.Success,
                @"Success", mibNameTask + @"Success",
                null,
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.3 - DurationOfLastExecution 
            CreateUserValue(oidTask + 3, taskInfo.DurationOfLastExecution,
                @"Duration of last execution", mibNameTask + @"DurationOfLastExecution",
                null,
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.4 - CurrentMemoryUsed 
            CreateUserValue(oidTask + 4, taskInfo.CurrentMemoryUsed,
                @"Current memory used", mibNameTask + @"CurrentMemoryUsed",
                null,
                JobInfoItemRequested, parentJob);

            var ci = taskInfo.CleanupInfo;
            if (ci == null)
            {
                return;
            }

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5 - Cleanup [Folder]
            IbaSnmpOid oidCleanup = oidTask + 5;
            string mibNameCleanup = mibNameTask + @"Cleanup";
            AddMetadataForOidSuffix(oidCleanup, @"Cleanup", mibNameCleanup);

            // todo to enum
            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.0 - LimitChoice 
            CreateUserValue(oidCleanup + 0, ci.LimitChoice.ToString(),
                @"Limit choice", mibNameCleanup + @"LimitChoice",
                null,
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.1 - Subdirectories
            CreateUserValue(oidCleanup + 1, ci.Subdirectories,
                @"Subdirectories", mibNameCleanup + @"Subdirectories",
                null,
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.2 - FreeDiskSpace
            CreateUserValue(oidCleanup + 2, ci.FreeDiskSpace,
                @"Free disk space", mibNameCleanup + @"FreeDiskSpace",
                null,
                JobInfoItemRequested, parentJob);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.3 - UsedDiskSpace
            CreateUserValue(oidCleanup + 3, ci.UsedDiskSpace,
                @"Used disk space", mibNameCleanup + @"UsedDiskSpace",
                null,
                JobInfoItemRequested, parentJob);
        }

        #endregion


        #region Oid metadata and CreateUserValue() overloads
        private void AddMetadataForOidSuffix(IbaSnmpOid oidSuffix, string guiCaption, string mibName = null, string mibDescription = null)
        {
            OidMetadataDict[IbaSnmp.OidIbaProductSpecific + oidSuffix] = new OidMetadata(guiCaption, mibName, mibDescription);
        }

        public void CreateUserValue(IbaSnmpOid oidSuffix, bool initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<IbaSnmpObjectValueRequestedEventArgs> handler = null,
            object tag = null)
        {
            AddMetadataForOidSuffix(oidSuffix, caption);
            IbaSnmp.CreateUserValue(oidSuffix, initialValue, mibName, mibDescription, handler, tag);
        }

        public void CreateUserValue(IbaSnmpOid oidSuffix, string initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<IbaSnmpObjectValueRequestedEventArgs> handler = null,
            object tag = null)
        {
            AddMetadataForOidSuffix(oidSuffix, caption);
            IbaSnmp.CreateUserValue(oidSuffix, initialValue, mibName, mibDescription, handler, tag);
        }

        public void CreateUserValue(IbaSnmpOid oidSuffix, int initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<IbaSnmpObjectValueRequestedEventArgs> handler = null,
            object tag = null)
        {
            AddMetadataForOidSuffix(oidSuffix, caption);
            IbaSnmp.CreateUserValue(oidSuffix, initialValue, mibName, mibDescription, handler, tag);
        }

        public void CreateUserValue(IbaSnmpOid oidSuffix, uint initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<IbaSnmpObjectValueRequestedEventArgs> handler = null,
            object tag = null)
        {
            AddMetadataForOidSuffix(oidSuffix, caption);
            IbaSnmp.CreateUserValue(oidSuffix, initialValue, mibName, mibDescription, handler, tag);
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
            args.Value = args.IbaSnmp.GetValue(args.Oid);
        }

        #endregion

        #endregion

    }
}
