using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows.Forms;
using iba.Data;
using iba.ibaOPCServer;
using iba.Logging;
using Opc.Ua;
using Opc.Ua.Configuration;
using Timer = System.Threading.Timer;

namespace iba.Processing
{


    #region Helper classes

    //public enum OpcUaWorkerStatus // todo share with snmp 
    //{
    //    Started,
    //    Stopped,
    //    Errored
    //}

    [Serializable]
    public class OpcUaTreeNodeTag
    {
        public string Id { get; set; }

        public bool IsFolder { get; set; }

        public string Caption { get; set; }

        public string Value { get; set; }

        public string Type { get; set; }

        public string MibName { get; set; }

        public string MibDescription { get; set; }

        public bool IsExpandedByDefault { get; set; }
    }


    #endregion

    class OpcUaWorker
    {
        private BaseDataVariableState _lifeBeatVar;


        public static int TmpLifebeatValue { get; private set; } = 0; // // todo. kls. only if enabled

        #region Construction, Destruction, Init

        private string _ibaDatCoordinatorUaServerStr = "ibaDatCoordinatorUaServer";

        public OpcUaWorker()
        {
            Status = SnmpWorkerStatus.Errored;
            StatusString = iba.Properties.Resources.opcUaStatusNotInit;

            System.Windows.Forms.Timer lifebeatTimer = new System.Windows.Forms.Timer {Enabled = true, Interval = 300};
            lifebeatTimer.Tick += (sender, args) =>
            {
                try
                {
                    TmpLifebeatValue++;
                    if (_lifeBeatVar != null)
                        NodeManager?.KlsSetValueScalar(_lifeBeatVar, TmpLifebeatValue);
                }
                catch { /**/ }
            };
        }

        public void Init()
        {
            lock (LockObject)
            {
                if (_uaApplication != null) //todo
                {
                    // disable double initialization
                    return;
                }

                _uaApplication = new ApplicationInstance();
//                IbaOpcUaServer = new IbaOpcUaServer(); 
            }

            //_uaApplication?.Stop();
            _uaApplication.ApplicationType = ApplicationType.Server;
            _uaApplication.ConfigSectionName = $"iba_ag.{_ibaDatCoordinatorUaServerStr}";
            //try
            {
                // create ua server
                IbaOpcUaServer = new IbaOpcUaServer();

                // load the application configuration.
                _uaApplication.LoadApplicationConfiguration(false);
                if (!string.IsNullOrEmpty(_uaApplication.ApplicationConfiguration.TraceConfiguration.OutputFilePath))
                {
                    _uaApplication.ApplicationConfiguration.TraceConfiguration.OutputFilePath =
                        "c:\\Temp\\datCo_OpcUaServer_log.txt"; //todo
                    _uaApplication.ApplicationConfiguration.TraceConfiguration.ApplySettings();
                }

                String sysName = IbaOpcUaServer.GetFQDN(); //todo
                //if ((_uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Count == 0) ||
                //    !_uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses[0].StartsWith(
                //        "opc.tcp://" + sysName + ":21060") ||
                //    !_uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses[0].StartsWith(
                //        "http://" + sysName + ":21061"))
                {
                    // todo. kls. default EP?

                    //string base1 = "opc.tcp://" + sysName + $":21060/{_ibaDatCoordinatorUaServerStr}";
                    ////string base2 = "http://" + sysName + $":21061/{_ibaDatCoordinatorUaServerStr}";
                    _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Clear();
                    _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Add(OpcUaData.DefaultEndPoint.Uri);
                    //_uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Add(Base2);
                }

                // check the application certificate.
                _uaApplication.CheckApplicationInstanceCertificate(false, 0);


                // start the server.
                if (Environment.UserInteractive)
                    _uaApplication.Start(IbaOpcUaServer);
                else
                    // todo - launch as service not tested!
                    _uaApplication.StartAsService(IbaOpcUaServer);

                //_uaServer.OnTrustModeChanged += trustModeChangedHandler;



                IbaOpcUaServer.KlsInitialize(null, null, false);

                //_opcUaData.EndPointString = IbaOpcUaServer.KlsStrEndpointTcp; // todo. kls. 

                FolderState globalsFolder = NodeManager.KlsGetSubtreeRootFolder(SubTreeId.Globals);
                var folderTest = NodeManager.KlsCreateFolderAndItsNode(globalsFolder, "Test");
                _lifeBeatVar =
                    NodeManager.KlsCreateStatusVariableAndItsNode(folderTest, "Lifebeat", BuiltInType.Int32);

                NodeManager.KlsSetValueScalar(_lifeBeatVar, TmpLifebeatValue);
            }

            RestartServer();

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
            SetGeneralProductInformation();
            RebuildTree();
        }

        private void ApplyEndpoints()
        {
            _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Clear();
            foreach (var ep in _opcUaData.Endpoints)
                _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Add(ep.Uri);
        }

        public string Tmp____GetInternalEndpoints()
        {
            string str = "";
            foreach (string adr in _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses)
            {
                str +=  adr + "; ";
            }

            return str.Trim(' ', ';');

            //_uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Clear();
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

        ApplicationInstance _uaApplication;
        private IbaUaNodeManager NodeManager => IbaOpcUaServer._ibaUaNodeManager;

        public IbaOpcUaServer IbaOpcUaServer { get; private set; }

        private OpcUaData _opcUaData = new OpcUaData();

        public OpcUaData OpcUaData
        {
            get => _opcUaData;
            set
            {
                if (value == null)
                {
                    // do not allow to set null data here
                    return;
                }
                if (_opcUaData != null && _opcUaData.Equals(value))
                {
                    // Configuration has not changed
                    // do not restart agent
                    return;
                }
                _opcUaData = value;

                if (IbaOpcUaServer != null)
                {
                    RestartServer();
                }
            }
        }

        //public OpcUaWorkerStatus Status { get; private set; }
        public SnmpWorkerStatus Status { get; private set; }
        
        public string StatusString { get; private set; }

        public static string GetCurrentThreadString()
        {
            var thr = Thread.CurrentThread;
            string thrNameOrId = String.IsNullOrWhiteSpace(thr.Name) ? thr.ManagedThreadId.ToString() : thr.Name;
            return $"thr=[{thrNameOrId}]";
        }

        public void RestartServer()
        {
            var oldStatus = Status;
            Status = SnmpWorkerStatus.Errored;
            StatusString = @"";

            try
            {
                IbaOpcUaServer.Stop(); // todo. kls. 
                ApplyConfigurationToXxxxxxxxxxx(); // todo
                string logMessage;

                if (_opcUaData.Enabled)
                {
                    var uri = new Uri(_opcUaData.Endpoints[0].Uri);
                    //ApplyEndpoints(); // todo. kls. 
                    _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Clear();
                    foreach (var ep in _opcUaData.Endpoints)
                        _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Add(ep.Uri);
                    _uaApplication.Stop();
                    _uaApplication.Start(IbaOpcUaServer);

                    //IbaOpcUaServer.Start(_uaApplication.ApplicationConfiguration, uri);
                    Status = SnmpWorkerStatus.Started;
                    StatusString = String.Format(iba.Properties.Resources.opcUaStatusRunningOnPort, _opcUaData.Endpoints[0]); // todo simplify strings???

                    logMessage = Status == oldStatus
                        ?
                       // log 'was restarted' if status has not changed (now is 'Started' as before) 
                       String.Format(iba.Properties.Resources.opcUaStatusRunningRestarted, StatusString)
                        :
                        // log 'was started' if status has changed from 'Errored' or 'Stopped' to 'Started' 
                        String.Format(iba.Properties.Resources.opcUaStatusRunningStarted, StatusString);
                }
                else
                {
                    Status = SnmpWorkerStatus.Stopped;
                    StatusString = iba.Properties.Resources.opcUaStatusDisabled;

                    logMessage = Status == oldStatus
                        ?
                        // do not log anything if status has not changed (now is 'Stopped' as before) 
                        null
                        :
                        // log 'was stopped' if status has changed from 'Errored' or 'Started' to 'Stopped'
                        String.Format(iba.Properties.Resources.opcUaStatusStopped, StatusString);
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
                StatusString = String.Format(iba.Properties.Resources.opcUaStatusError, ex.Message);
                if (LogData.Data.Logger.IsOpen) LogData.Data.Logger.Log(Level.Exception, StatusString);
            }
        }

        private void ApplyConfigurationToXxxxxxxxxxx()
        {
            if (IbaOpcUaServer == null || OpcUaData == null)
            {
                return;
            }

            // apply port, do not change ip addresses
            //List<IPEndPoint> eps = IbaOpcUaServer.EndPointsToListen;
            //foreach (IPEndPoint ipe in eps)
            //{
            //    ipe.Port = SnmpData.Port;
            //}
            //IbaOpcUaServer.EndPointsToListen = eps;

            // security
            //IbaOpcUaServer.SetSecurityForV1AndV2(new List<string> { SnmpData.V1V2Security });
        }

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

        private string _enumJobStatus;
        private string _enumCleanupType;

        private void RegisterEnums()
        {
            //_enumJobStatus = IbaOpcUaServer.RegisterEnumDataType(
            //    "JobStatus", "Current status of the job (started, stopped or disabled)",
            //    new Dictionary<int, string>
            //    {
            //        {(int) SnmpObjectsData.JobStatus.Disabled, "disabled"},
            //        {(int) SnmpObjectsData.JobStatus.Started, "started"},
            //        {(int) SnmpObjectsData.JobStatus.Stopped, "stopped"}
            //    }
            //);

            //_enumCleanupType = IbaOpcUaServer.RegisterEnumDataType(
            //    "LocalCleanupType", "Type of limitation of disk space usage",
            //    new Dictionary<int, string>
            //    {
            //        {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.None, "none"},
            //        {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.LimitDirectories, "limitDirectories"},
            //        {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.LimitDiskspace, "limitDiskSpace"},
            //        {(int) TaskWithTargetDirData.OutputLimitChoiceEnum.SaveFreeSpace, "saveFreeSpace"}
            //    }
            //);
        }

        #endregion

        #endregion


        #region General product information

        /// <summary> Sets product name and version. Registers license value handlers. </summary>
        private void SetGeneralProductInformation()
        {
            // change default gui caption from "ibaDatCo" to "ibaDatCoordinator"
            //IbaOpcUaServer.SetOidMetadata(IbaOpcUaServer.OidIbaProduct, @"ibaDatCoordinator");

            //// all the other captions can be left as is
            //// all the general objects already have gui Captions, predefined
            //// e.g.:
            //// var captionSample = IbaSnmp.GetOidMetadata(IbaSnmp.OidIbaProductGeneralLicensingCustomer).GuiCaption;

            //// ibaRoot.DatCoord.General.1 - Title
            ////IbaSnmp.SetOidMetadata(IbaSnmp.OidIbaProductGeneralTitle, @"Title");
            //IbaOpcUaServer.ValueIbaProductGeneralTitle = @"ibaDatCoordinator";

            //// ibaRoot.DatCoord.General.2 - Version
            //var ver = GetType().Assembly.GetName().Version;
            //IbaOpcUaServer.SetValueIbaProductGeneralVersion(ver.Major, ver.Minor, ver.Build);

            //// ibaRoot.DatCoord.General.Licensing.1 - IsValid
            //IbaOpcUaServer.LicensingIsValidRequested += IbaSnmp_LicensingValueRequested;

            //// ibaRoot.DatCoord.General.Licensing.2 - Serial number
            //IbaOpcUaServer.LicensingSnRequested += IbaSnmp_LicensingValueRequested;

            //// ibaRoot.DatCoord.General.Licensing.3 - Hardware ID
            //IbaOpcUaServer.LicensingHwIdRequested += IbaSnmp_LicensingValueRequested;

            //// ibaRoot.DatCoord.General.Licensing.4 - Dongle type
            //IbaOpcUaServer.LicensingTypeRequested += IbaSnmp_LicensingValueRequested;

            //// ibaRoot.DatCoord.General.Licensing.5 - Customer
            //IbaOpcUaServer.LicensingCustomerRequested += IbaSnmp_LicensingValueRequested;

            //// ibaRoot.DatCoord.General.Licensing.6 - Time limit
            //IbaOpcUaServer.LicensingTimeLimitRequested += IbaSnmp_LicensingValueRequested;

            //// ibaRoot.DatCoord.General.Licensing.7 - Demo time limit
            //IbaOpcUaServer.LicensingDemoTimeLimitRequested += IbaSnmp_LicensingValueRequested;
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

        private System.Timers.Timer _treeValidatorTimer;

        /// <summary>
        /// Rebuilds a tree completely if its <see cref="IsStructureValid"/> flag is set to false. 
        /// Use returned value to know whether tree has been rebuilt.
        /// </summary>
        /// <returns> <value>true</value> if tree was rebuilt, 
        /// <value>false</value> if it is valid and has not been modified by this call.</returns>
        public bool RebuildTreeIfItIsInvalid()
        {
            // here I use double thamn normal timeout to give priority over other locks
            if (Monitor.TryEnter(LockObject, LockTimeout * 2))
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
            if (man == null || IbaOpcUaServer == null)
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

                    //IbaOpcUaServer.DeleteAllUserValues();

                    if (!man.SnmpRebuildObjectsData(ObjectsData))
                    {
                        return false; // rebuild failed
                    }

                    // ibaRoot.DatCoord.1 - Product-Specific
                    //IbaOpcUaServer.SetOidMetadata(IbaOpcUaServer.OidIbaProductSpecific, "Product-specific");

                    // ibaRoot.DatCoord.Product.1 - Global cleanup
                    BuildSectionGlobalCleanup();

                    // ibaRoot.DatCoord.Product.2 - Standard jobs
                    BuildSectionStandardJobs();

                    // ibaRoot.DatCoord.Product.3 - Scheduled jobs
                    BuildSectionScheduledJobs();

                    // ibaRoot.DatCoord.Product.4 - One time jobs
                    BuildSectionOneTimeJobs();

                    // ibaRoot.DatCoord.Product.5 - Event jobs
                    BuildSectionEventJobs();

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


        #region Building tree Sections 1...4 (from 'GlobalCleanup' to 'OneTimeJobs')

        private void BuildSectionGlobalCleanup()
        {
            var oidSection = "";// = new string(SnmpObjectsData.GlobalCleanupOid);

            AddMetadataForOidSuffix(oidSection, @"Global cleanup", @"globalCleanup",
                "Global cleanup settings for all local drives.");

            for (int i = 0; i < ObjectsData.GlobalCleanup.Count; i++)
            {
                try
                {
                    var driveInfo = ObjectsData.GlobalCleanup[i];

                    // ibaRoot.DatCoord.Product.GlobalCleanup.(index) - Drive [Folder]
                    string oidDrive = oidSection + (uint)(i + 1);
                    driveInfo.Oid = oidDrive;

                    string mibNameDrive = $@"globalCleanupDrive{oidDrive}";
                    AddMetadataForOidSuffix(oidDrive, $@"Drive '{driveInfo.DriveName}'", mibNameDrive,
                        $@"Global cleanup settings for the drive '{driveInfo.DriveName}'.");

                    // ibaRoot.DatCoord.Product.GlobalCleanup.DriveX....
                    {
                        CreateUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.DriveNameOid, driveInfo.DriveName,
                            @"Drive Name", mibNameDrive + @"Name",
                            @"Drive name like it appears in operating system.",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        CreateUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.ActiveOid, driveInfo.Active,
                            @"Active", mibNameDrive + @"Active",
                            @"Whether or not the global cleanup is enabled for the drive.",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        CreateUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.SizeInMbOid, driveInfo.SizeInMb,
                            @"Size", mibNameDrive + @"Size",
                            @"Size of the drive (in megabytes).",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        CreateUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.CurrentFreeSpaceInMbOid, driveInfo.CurrentFreeSpaceInMb,
                            @"Curr. free space", mibNameDrive + @"CurrFreeSpace",
                            @"Current free space of the drive (in megabytes).",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        CreateUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.MinFreeSpaceInPercentOid, driveInfo.MinFreeSpaceInPercent,
                            @"Min free space", mibNameDrive + @"MinFreeSpace",
                            @"Minimum disk space that is kept free on the drive by deleting the oldest iba dat files (in percent).",
                            GlobalCleanupDriveInfoItemRequested, driveInfo);

                        CreateUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.RescanTimeOid, driveInfo.RescanTime,
                            @"Rescan time", mibNameDrive + @"RescanTime",
                            @"How often the application rescans the drive parameters (in minutes).",
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
            var oidSection = "";//  = new string(SnmpObjectsData.StandardJobsOid);

            // ibaRoot.DatCoord.Product.2 - StandardJobs [Folder]
            AddMetadataForOidSuffix(oidSection, @"Standard jobs", @"standardJobs",
                @"List of all standard jobs.");

            for (int i = 0; i < ObjectsData.StandardJobs.Count; i++)
            {
                try
                {
                    SnmpObjectsData.StandardJobInfo jobInfo = ObjectsData.StandardJobs[i];

                    // ibaRoot.DatCoord.Product.StdJobs.(index) - Job [Folder]
                    string oidJob = oidSection + (uint)(i + 1);
                    string mibNameJob = $@"standardJob{oidJob}";
                    AddMetadataForOidSuffix(oidJob, $@"Job '{jobInfo.JobName}'", mibNameJob,
                        $@"Properties of standard job '{jobInfo.JobName}'.");

                    // create objects that are common for all the job types
                    string oidJobGen;
                    string mibNameJobGen;
                    BuildCommonGeneralJobSubsection(
                        oidJob, out oidJobGen,
                        mibNameJob, out mibNameJobGen,
                        jobInfo);

                    // create all the rest of general job objects
                    // ibaRoot.DatCoord.Product.StdJobs.Job.General ...
                    {
                        CreateUserValue(oidJobGen + SnmpObjectsData.StandardJobInfo.PermFailedCountOid,
                            jobInfo.PermFailedCount,
                            @"Perm. Failed #", mibNameJobGen + @"PermFailed",
                            @"Number of files with persistent errors.",
                            JobInfoItemRequested, jobInfo);

                        CreateUserValue(oidJobGen + SnmpObjectsData.StandardJobInfo.TimestampJobStartedOid,
                            jobInfo.TimestampJobStarted,
                            @"Timestamp job started", mibNameJobGen + @"TimestampJobStarted",
                            @"Time when the job was started. For a stopped job, it relates to the last start of the job. " +
                            @"If job was never started, then value is '01.01.0001 0:00:00'.",
                            JobInfoItemRequested, jobInfo);

                        CreateUserValue(oidJobGen + SnmpObjectsData.StandardJobInfo.TimestampLastDirectoryScanOid,
                            jobInfo.TimestampLastDirectoryScan,
                            @"Timestamp last directory scan", mibNameJobGen + @"TimestampLastDirectoryScan",
                            @"Time when the last scan for new (unprocessed) .dat files was performed. " +
                            @"If scan was never performed, then value is '01.01.0001 0:00:00'.",
                            JobInfoItemRequested, jobInfo);

                        CreateUserValue(oidJobGen + SnmpObjectsData.StandardJobInfo.TimestampLastReprocessErrorsScanOid,
                            jobInfo.TimestampLastReprocessErrorsScan,
                            @"Timestamp last reprocess errors scan", mibNameJobGen + @"TimestampLastReprocessErrorsScan",
                            @"Time when the last reprocess scan was performed " +
                            @"(reprocess scan is a scan for .dat files that previously were processed with errors).  " +
                            @"If scan was never performed, then value is '01.01.0001 0:00:00'.",
                            JobInfoItemRequested, jobInfo);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.General.10 - LastProcessing [Folder]
                        string oidLastProc = oidJobGen + SnmpObjectsData.StandardJobInfo.LastProcessingOid;
                        AddMetadataForOidSuffix(oidLastProc, @"LastProcessing", mibNameJobGen + @"LastProcessing",
                            @"Information about the last successfully processed file.");

                        CreateUserValue(oidLastProc + SnmpObjectsData.StandardJobInfo.LastProcessingLastDatFileProcessedOid,
                            jobInfo.LastProcessingLastDatFileProcessed,
                            @"Last dat-file processed", mibNameJobGen + @"LastFile",
                            @"Filename of the last successfully processed file. If no files were successfully processed, then value is empty.",
                            JobInfoItemRequested, jobInfo);

                        CreateUserValue(oidLastProc + SnmpObjectsData.StandardJobInfo.LastProcessingStartTimeStampOid,
                            jobInfo.LastProcessingStartTimeStamp,
                            @"Start timestamp", mibNameJobGen + @"StartStamp",
                            @"Time when processing of the last successfully processed file was started. " +
                            @"If no files were successfully processed, then value is '01.01.0001 0:00:00'.",
                            JobInfoItemRequested, jobInfo);

                        CreateUserValue(oidLastProc + SnmpObjectsData.StandardJobInfo.LastProcessingFinishTimeStampOid,
                            jobInfo.LastProcessingFinishTimeStamp,
                            @"Finish timestamp", mibNameJobGen + @"FinishStamp",
                            @"Time when processing of the last successfully processed file was finished. " +
                            @"If no files were successfully processed, then value is '01.01.0001 0:00:00'.",
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
            var oidSection = "";//  = new string(SnmpObjectsData.ScheduledJobsOid);

            AddMetadataForOidSuffix(oidSection, @"Scheduled jobs", @"scheduledJobs",
                @"List of all scheduled jobs.");

            for (int i = 0; i < ObjectsData.ScheduledJobs.Count; i++)
            {
                try
                {
                    var jobInfo = ObjectsData.ScheduledJobs[i];

                    // ibaRoot.DatCoord.Product.SchJobs.(index) - Job [Folder]
                    string oidJob = oidSection + (uint)(i + 1);
                    string mibNameJob = $@"scheduledJob{oidJob}";
                    AddMetadataForOidSuffix(oidJob, $@"Job '{jobInfo.JobName}'", mibNameJob,
                        $@"Properties of scheduled job '{jobInfo.JobName}'.");

                    // create objects that are common for all the job types
                    string oidJobGen;
                    string mibNameJobGen;
                    BuildCommonGeneralJobSubsection(
                        oidJob, out oidJobGen,
                        mibNameJob, out mibNameJobGen,
                        jobInfo);

                    // create all the rest of general job objects
                    // ibaRoot.DatCoord.Product.SchJobs.Job xxx
                    {
                        CreateUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.PermFailedCountOid, jobInfo.PermFailedCount,
                            @"Perm. Failed #", mibNameJobGen + @"PermFailedCount",
                            @"Number of files with persistent errors.",
                            JobInfoItemRequested, jobInfo);

                        CreateUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.TimestampJobStartedOid, jobInfo.TimestampJobStarted,
                            @"Timestamp job started", mibNameJobGen + @"TimestampJobStarted",
                            @"Time when job was started (starting of the scheduled job does NOT mean that it will be executed immediately).  " +
                            @"For a stopped job, it relates to the last start of the job.  " +
                            @"If job was never started, then value is '01.01.0001 0:00:00'.",
                            JobInfoItemRequested, jobInfo);

                        CreateUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.TimestampLastExecutionOid, jobInfo.TimestampLastExecution,
                            @"Timestamp last execution", mibNameJobGen + @"TimestampLastExecution",
                            @"Time when the job was last executed. " +
                            @"(This does not mean the moment when job was started, but the moment when configured trigger was fired last time); " +
                            @"If job was never executed, then value is '01.01.0001 0:00:00'.",
                            JobInfoItemRequested, jobInfo);

                        CreateUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.TimestampNextExecutionOid, jobInfo.TimestampNextExecution,
                            @"Timestamp next execution", mibNameJobGen + @"TimestampNextExecution",
                            @"Time of the next scheduled execution. " +
                            @"If there is no execution scheduled, then value is '01.01.0001 0:00:00'.",
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
            var oidSection = "";//  = new string(SnmpObjectsData.OneTimeJobsOid);

            AddMetadataForOidSuffix(oidSection, @"One time jobs", @"oneTimeJobs",
                @"List of all one-time jobs.");

            for (int i = 0; i < ObjectsData.OneTimeJobs.Count; i++)
            {
                try
                {
                    var jobInfo = ObjectsData.OneTimeJobs[i];
                    // ibaRoot.DatCoord.Product.OtJobs.(index) - Job [folder]
                    string oidJob = oidSection + (uint)(i + 1);
                    string mibNameJob = $@"oneTimeJob{oidJob}";
                    AddMetadataForOidSuffix(oidJob, $@"Job '{jobInfo.JobName}'", mibNameJob,
                        $@"Properties of one-time job '{jobInfo.JobName}'.");

                    // create objects that are common for all the job types
                    string oidJobGen;
                    string mibNameJobGen;
                    BuildCommonGeneralJobSubsection(
                        oidJob, out oidJobGen,
                        mibNameJob, out mibNameJobGen,
                        jobInfo);

                    // create all the rest of general job objects
                    // ibaRoot.DatCoord.Product.OtJobs.Job xxx
                    {
                        CreateUserValue(oidJobGen + SnmpObjectsData.OneTimeJobInfo.TimestampLastExecutionOid, jobInfo.TimestampLastExecution,
                            @"Timestamp last execution", mibNameJobGen + @"TimestampLastExecution",
                            @"Time when the last execution was started. " +
                            @"If job was never executed, then value is '01.01.0001 0:00:00'.",
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

        private void BuildSectionEventJobs()
        {
            var oidSection = "";//  = new string(SnmpObjectsData.EventBasedJobsOid);

            AddMetadataForOidSuffix(oidSection, @"Event jobs", @"eventJobs",
                @"List of all event jobs.");

            for (int i = 0; i < ObjectsData.EventBasedJobs.Count; i++)
            {
                try
                {
                    var jobInfo = ObjectsData.EventBasedJobs[i];

                    // ibaRoot.DatCoord.Product.SchJobs.(index) - Job [Folder]
                    string oidJob = oidSection + (uint)(i + 1);
                    string mibNameJob = $@"eventJob{oidJob}";
                    AddMetadataForOidSuffix(oidJob, $@"Job '{jobInfo.JobName}'", mibNameJob,
                        $@"Properties of event job '{jobInfo.JobName}'.");

                    // create objects that are common for all the job types
                    string oidJobGen;
                    string mibNameJobGen;
                    BuildCommonGeneralJobSubsection(
                        oidJob, out oidJobGen,
                        mibNameJob, out mibNameJobGen,
                        jobInfo);

                    // create all the rest of general job objects
                    // ibaRoot.DatCoord.Product.EvtJobs.Job xxx
                    {
                        CreateUserValue(oidJobGen + SnmpObjectsData.EventBasedJobInfo.PermFailedCountOid, jobInfo.PermFailedCount,
                            @"Perm. Failed #", mibNameJobGen + @"PermFailedCount",
                            @"Number of files with persistent errors.",
                            JobInfoItemRequested, jobInfo);

                        CreateUserValue(oidJobGen + SnmpObjectsData.EventBasedJobInfo.TimestampJobStartedOid, jobInfo.TimestampJobStarted,
                            @"Timestamp job started", mibNameJobGen + @"TimestampJobStarted",
                            @"Time when job was started (starting of the event job does NOT mean that it will be executed immediately).  " +
                            @"For a stopped job, it relates to the last start of the job.  " +
                            @"If job was never started, then value is '01.01.0001 0:00:00'.",
                            JobInfoItemRequested, jobInfo);

                        CreateUserValue(oidJobGen + SnmpObjectsData.EventBasedJobInfo.TimestampLastExecutionOid, jobInfo.TimestampLastExecution,
                            @"Timestamp last execution", mibNameJobGen + @"TimestampLastExecution",
                            @"Time when the job was last executed. " +
                            @"(This does not mean the moment when job was started, but the last occurrence of a monitored event); " +
                            @"If job was never executed, then value is '01.01.0001 0:00:00'.",
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
            string oidJob, out string oidJobGen,
            string mibNameJob, out string mibNameJobGen,
            SnmpObjectsData.JobInfoBase jobInfo)
        {

            jobInfo.Oid = oidJob;

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.1 - General [Folder]
            oidJobGen = oidJob + SnmpObjectsData.JobInfoBase.GeneralOid;
            mibNameJobGen = mibNameJob + @"General";
            AddMetadataForOidSuffix(oidJobGen, @"General", mibNameJobGen,
                $@"General properties of job '{jobInfo.JobName}'.");

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.General ...
            {
                CreateUserValue(oidJobGen + SnmpObjectsData.JobInfoBase.JobNameOid, jobInfo.JobName,
                    @"Job Name", mibNameJobGen + @"Name",
                    @"The name of the job as it appears in GUI.",
                    JobInfoItemRequested, jobInfo);

                CreateEnumUserValue(oidJobGen + SnmpObjectsData.JobInfoBase.StatusOid, _enumJobStatus, (int)jobInfo.Status,
                    @"Status", mibNameJobGen + @"Status",
                    @"Current status of the job (started, stopped or disabled).",
                    JobInfoItemRequested, jobInfo);

                CreateUserValue(oidJobGen + SnmpObjectsData.JobInfoBase.TodoCountOid, jobInfo.TodoCount,
                    @"Todo #", mibNameJobGen + @"Todo",
                    @"Number of dat files to be processed.",
                    JobInfoItemRequested, jobInfo);

                CreateUserValue(oidJobGen + SnmpObjectsData.JobInfoBase.DoneCountOid, jobInfo.DoneCount,
                    @"Done #", mibNameJobGen + @"Done",
                    @"Number of processed files.",
                    JobInfoItemRequested, jobInfo);

                CreateUserValue(oidJobGen + SnmpObjectsData.JobInfoBase.FailedCountOid, jobInfo.FailedCount,
                    @"Failed #", mibNameJobGen + @"Failed",
                    @"Number of errors occurred during processing.",
                    JobInfoItemRequested, jobInfo);
            }

            // create tasks
            BuildTasks(oidJob, mibNameJob, jobInfo);
        }

        #endregion


        #region Tasks subtrees

        private void BuildTasks(string oidjJob, string mibNameJob, SnmpObjectsData.JobInfoBase jobInfo)
        {
            var tasks = jobInfo?.Tasks;
            if (tasks == null)
            {
                return;
            }

            var oidTasks = oidjJob + SnmpObjectsData.JobInfoBase.TasksOid;

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.2 - Tasks [Folder]
            AddMetadataForOidSuffix(oidTasks, @"Tasks", mibNameJob + @"Tasks",
                $@"Information about all tasks of the job '{jobInfo.JobName}'.");

            for (int i = 0; i < tasks.Count; i++)
            {
                SnmpObjectsData.TaskInfo taskInfo = tasks[i];

                uint i1 = (uint)(i + 1); // index for mib

                string mibNameTask = mibNameJob + $@"Task{i1}";
                // ibaRoot.DatCoord.Product.XxxJobs.JobY.Tasks.(index) - Task [Folder]
                AddMetadataForOidSuffix(oidTasks + i1, $@"Task '{taskInfo.TaskName}'", mibNameTask,
                    $@"Information about task '{taskInfo.TaskName}' of the job '{jobInfo.JobName}'.");

                // create task contents
                // ibaRoot.DatCoord.Product.XxxJobs.JobY.Tasks.TaskZ ...
                try
                {
                    BuildTask(oidTasks + i1, mibNameTask, taskInfo);
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

        private void BuildTask(string oidTask, string mibNameTask, SnmpObjectsData.TaskInfo taskInfo)
        {
            var parentJob = taskInfo.Parent;

            taskInfo.Oid = oidTask;

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ ... 

            CreateUserValue(oidTask + SnmpObjectsData.TaskInfo.TaskNameOid, taskInfo.TaskName,
                @"Task name", mibNameTask + @"Name",
                @"The name of the task as it appears in GUI.",
                JobInfoItemRequested, parentJob);

            CreateUserValue(oidTask + SnmpObjectsData.TaskInfo.TaskTypeOid, taskInfo.TaskType,
                @"Task type", mibNameTask + @"Type",
                @"The type of the task (copy, extract, report, etc.).",
                JobInfoItemRequested, parentJob);

            CreateUserValue(oidTask + SnmpObjectsData.TaskInfo.SuccessOid, taskInfo.Success,
                @"Success", mibNameTask + @"Success",
                @"Whether or not the last executed task was completed successfully, i.e. without errors. " +
                @"For Condition task this means that the expression was successfully evaluated as TRUE or FALSE - both results are treated as success.",
                JobInfoItemRequested, parentJob);

            CreateUserValue(oidTask + SnmpObjectsData.TaskInfo.DurationOfLastExecutionOid, taskInfo.DurationOfLastExecution,
                @"Duration of last execution", mibNameTask + @"DurationOfLastExecution",
                @"Duration of the last task execution (in seconds).",
                JobInfoItemRequested, parentJob);

            CreateUserValue(oidTask + SnmpObjectsData.TaskInfo.MemoryUsedForLastExecutionOid, taskInfo.MemoryUsedForLastExecution,
                @"Memory used for last execution", mibNameTask + @"LastMemoryUsed",
                @"Amount of memory used during the last execution of the task (in megabytes). " +
                @"This is applicable only to tasks that use ibaAnalyzer for their processing e.g., Condition, Report, Extract and some custom tasks.",
                JobInfoItemRequested, parentJob);

            var ci = taskInfo.CleanupInfo;
            if (ci == null)
            {
                return;
            }

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.Cleanup [Folder]
            string oidCleanup = oidTask + SnmpObjectsData.TaskInfo.CleanupInfoOid;
            string mibNameCleanup = mibNameTask + @"Cleanup";
            AddMetadataForOidSuffix(oidCleanup, @"Cleanup", mibNameCleanup,
                @"Cleanup parameters of the task.");

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.Cleanup ...

            CreateEnumUserValue(oidCleanup + SnmpObjectsData.LocalCleanupInfo.LimitChoiceOid, _enumCleanupType, (int)ci.LimitChoice,
                @"Limit choice", mibNameCleanup + @"LimitChoice",
                @"Option selected as limit for the disk space usage. " +
                @"(0 = None, 1 = Maximum subdirectories, 2 = Maximum used disk space, 3 = Minimum free disk space).",
                JobInfoItemRequested, parentJob);

            CreateUserValue(oidCleanup + SnmpObjectsData.LocalCleanupInfo.SubdirectoriesOid, ci.Subdirectories,
                @"Subdirectories", mibNameCleanup + @"Subdirectories",
                @"Maximum count of directories the task can use.",
                JobInfoItemRequested, parentJob);

            CreateUserValue(oidCleanup + SnmpObjectsData.LocalCleanupInfo.UsedDiskSpaceOid, ci.UsedDiskSpace,
                @"Used disk space", mibNameCleanup + @"UsedDiskSpace",
                @"Maximum disk space that can be used by the task (in megabytes).",
                JobInfoItemRequested, parentJob);

            CreateUserValue(oidCleanup + SnmpObjectsData.LocalCleanupInfo.FreeDiskSpaceOid, ci.FreeDiskSpace,
                @"Free disk space", mibNameCleanup + @"FreeDiskSpace",
                @"Minimum disk space that is kept free (in megabytes).",
                JobInfoItemRequested, parentJob);
        }

        #endregion


        #region Oid metadata and CreateUserValue() overloads

        private void AddMetadataForOidSuffix(string oidSuffix, string guiCaption, string mibName,
            string mibDescription)
        {
            //IbaOpcUaServer.SetUserOidMetadata(oidSuffix, mibName, mibDescription, guiCaption);
        }

        private void CreateUserValue(string oidSuffix, bool initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<object> handler = null,
            object tag = null)
        {
            //IbaOpcUaServer.CreateUserValue(oidSuffix, initialValue, null, null, handler, tag);
            //IbaOpcUaServer.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        private void CreateUserValue(string oidSuffix, string initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<object> handler = null,
            object tag = null)
        {
            //IbaOpcUaServer.CreateUserValue(oidSuffix, initialValue, null, null, handler, tag);
            //IbaOpcUaServer.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        // ReSharper disable once UnusedMember.Local
        private void CreateUserValue(string oidSuffix, int initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<object> handler = null,
            object tag = null)
        {
            //IbaOpcUaServer.CreateUserValue(oidSuffix, initialValue, null, null, handler, tag);
            //IbaOpcUaServer.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        private void CreateUserValue(string oidSuffix, uint initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<object> handler = null,
            object tag = null)
        {
            //IbaOpcUaServer.CreateUserValue(oidSuffix, initialValue, null, null, handler, tag);
            //IbaOpcUaServer.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        private void CreateUserValue(string oidSuffix, DateTime initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<object> handler = null,
            object tag = null)
        {
            //IbaOpcUaServer.CreateUserValue(oidSuffix, initialValue,
            //    UseSnmpV2TcForStrings ? IbaSnmpValueType.DateTimeTc : IbaSnmpValueType.DateTimeStr,
            //    null, null, handler, tag);
            //IbaOpcUaServer.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        private void CreateEnumUserValue(string oidSuffix, object valueType, int initialValue,
            string caption, string mibName = null, string mibDescription = null,
            EventHandler<object> handler = null,
            object tag = null)
        {
            //IbaOpcUaServer.CreateEnumUserValue(oidSuffix, valueType, initialValue, null, null, handler, tag);
            //IbaOpcUaServer.SetUserOidMetadata(oidSuffix, mibName, mibDescription, caption);
        }

        #endregion

        #endregion


        #endregion


        #region Value-Refresh functions  

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

                    //IbaOpcUaServer.ValueIbaProductGeneralLicensingIsValid = ObjectsData.License.IsValid;
                    //IbaOpcUaServer.ValueIbaProductGeneralLicensingSn = ObjectsData.License.Sn;
                    //IbaOpcUaServer.ValueIbaProductGeneralLicensingHwId = ObjectsData.License.HwId;
                    //IbaOpcUaServer.ValueIbaProductGeneralLicensingType = ObjectsData.License.DongleType;
                    //IbaOpcUaServer.ValueIbaProductGeneralLicensingCustomer = ObjectsData.License.Customer;
                    //IbaOpcUaServer.ValueIbaProductGeneralLicensingTimeLimit = ObjectsData.License.TimeLimit;
                    //IbaOpcUaServer.ValueIbaProductGeneralLicensingDemoTimeLimit = ObjectsData.License.DemoTimeLimit;
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

                    string oidDrive = "";//driveInfo.Oid;

                    //IbaOpcUaServer.SetUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.DriveNameOid, driveInfo.DriveName);
                    //IbaOpcUaServer.SetUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.ActiveOid, driveInfo.Active);
                    //IbaOpcUaServer.SetUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.SizeInMbOid, driveInfo.SizeInMb);
                    //IbaOpcUaServer.SetUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.CurrentFreeSpaceInMbOid, driveInfo.CurrentFreeSpaceInMb);
                    //IbaOpcUaServer.SetUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.MinFreeSpaceInPercentOid, driveInfo.MinFreeSpaceInPercent);
                    //IbaOpcUaServer.SetUserValue(oidDrive + SnmpObjectsData.GlobalCleanupDriveInfo.RescanTimeOid, driveInfo.RescanTime);
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

                    string oidJobGen = "";//jobInfo.Oid + SnmpObjectsData.JobInfoBase.GeneralOid;

                    //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.JobInfoBase.JobNameOid, jobInfo.JobName);
                    //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.JobInfoBase.StatusOid, (int)jobInfo.Status);
                    //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.JobInfoBase.TodoCountOid, jobInfo.TodoCount);
                    //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.JobInfoBase.DoneCountOid, jobInfo.DoneCount);
                    //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.JobInfoBase.FailedCountOid, jobInfo.FailedCount);

                    var stdJi = jobInfo as SnmpObjectsData.StandardJobInfo;
                    var schJi = jobInfo as SnmpObjectsData.ScheduledJobInfo;
                    var otJi = jobInfo as SnmpObjectsData.OneTimeJobInfo;
                    var evtJi = jobInfo as SnmpObjectsData.EventBasedJobInfo;
                    if (stdJi != null)
                    {
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.StandardJobInfo.PermFailedCountOid, stdJi.PermFailedCount);
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.StandardJobInfo.TimestampJobStartedOid, stdJi.TimestampJobStarted);
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.StandardJobInfo.TimestampLastDirectoryScanOid, stdJi.TimestampLastDirectoryScan);
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.StandardJobInfo.TimestampLastReprocessErrorsScanOid, stdJi.TimestampLastReprocessErrorsScan);
                        string oidJobGenLastproc = oidJobGen + SnmpObjectsData.StandardJobInfo.LastProcessingOid;
                        //IbaOpcUaServer.SetUserValue(oidJobGenLastproc + SnmpObjectsData.StandardJobInfo.LastProcessingLastDatFileProcessedOid, stdJi.LastProcessingLastDatFileProcessed);
                        //IbaOpcUaServer.SetUserValue(oidJobGenLastproc + SnmpObjectsData.StandardJobInfo.LastProcessingStartTimeStampOid, stdJi.LastProcessingStartTimeStamp);
                        //IbaOpcUaServer.SetUserValue(oidJobGenLastproc + SnmpObjectsData.StandardJobInfo.LastProcessingFinishTimeStampOid, stdJi.LastProcessingFinishTimeStamp);
                    }
                    else if (schJi != null)
                    {
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.PermFailedCountOid, schJi.PermFailedCount);
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.TimestampJobStartedOid, schJi.TimestampJobStarted);
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.TimestampLastExecutionOid, schJi.TimestampLastExecution);
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.TimestampNextExecutionOid, schJi.TimestampNextExecution);
                    }
                    else if (otJi != null)
                    {
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.OneTimeJobInfo.TimestampLastExecutionOid, otJi.TimestampLastExecution);
                    }
                    else if (evtJi != null)
                    {
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.PermFailedCountOid, evtJi.PermFailedCount);
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.TimestampJobStartedOid, evtJi.TimestampJobStarted);
                        //IbaOpcUaServer.SetUserValue(oidJobGen + SnmpObjectsData.ScheduledJobInfo.TimestampLastExecutionOid, evtJi.TimestampLastExecution);
                    }
                    else
                    {
                        // should not happen
                        throw new Exception($"Unknown job type {jobInfo}");
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
            string oidTask = "";// taskInfo.Oid;

            try
            {
                //IbaOpcUaServer.SetUserValue(oidTask + SnmpObjectsData.TaskInfo.TaskNameOid, taskInfo.TaskName);
                //IbaOpcUaServer.SetUserValue(oidTask + SnmpObjectsData.TaskInfo.TaskTypeOid, taskInfo.TaskType);
                //IbaOpcUaServer.SetUserValue(oidTask + SnmpObjectsData.TaskInfo.SuccessOid, taskInfo.Success);
                //IbaOpcUaServer.SetUserValue(oidTask + SnmpObjectsData.TaskInfo.DurationOfLastExecutionOid, taskInfo.DurationOfLastExecution);
                //IbaOpcUaServer.SetUserValue(oidTask + SnmpObjectsData.TaskInfo.MemoryUsedForLastExecutionOid, taskInfo.MemoryUsedForLastExecution);

                var ci = taskInfo.CleanupInfo;
                // ReSharper disable once InvertIf
                if (ci != null)
                {
                    string oidCleanup = oidTask + SnmpObjectsData.TaskInfo.CleanupInfoOid;

                    //IbaOpcUaServer.SetUserValue(oidCleanup + SnmpObjectsData.LocalCleanupInfo.LimitChoiceOid, (int)ci.LimitChoice);
                    //IbaOpcUaServer.SetUserValue(oidCleanup + SnmpObjectsData.LocalCleanupInfo.SubdirectoriesOid, ci.Subdirectories);
                    //IbaOpcUaServer.SetUserValue(oidCleanup + SnmpObjectsData.LocalCleanupInfo.UsedDiskSpaceOid, ci.UsedDiskSpace);
                    //IbaOpcUaServer.SetUserValue(oidCleanup + SnmpObjectsData.LocalCleanupInfo.FreeDiskSpaceOid, ci.FreeDiskSpace);
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"SNMP. Error during refreshing task {taskInfo.TaskName}. {ex.Message}.");
            }
        }

        #endregion


        #region XxxRequested event handlers

        private void IbaSnmp_LicensingValueRequested<T>(object sender, EventArgs args)
        {
            // refresh data if it is too old 
            RefreshLicenseInfo();

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            try
            {
                //args.Value = (T)args.IbaSnmp.GetValue(args.Oid);
            }
            catch
            {
                // suppress possible cast exception from null to ValueType (bool, int, etc.)
            }
        }

        private void GlobalCleanupDriveInfoItemRequested(object sender, object args)
        {
            var driveInfo = (object)null;// args.Tag as SnmpObjectsData.GlobalCleanupDriveInfo;

            if (driveInfo == null)
            {
                // should not happen
                //args.Value = null;
                return;
            }

            // refresh data if it is too old (or rebuild the whole tree if necessary)
            //RefreshGlobalCleanupDriveInfo(driveInfo);

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            //args.Value = args.IbaSnmp.GetValue(args.Oid);
        }

        private void JobInfoItemRequested(object sender, object args)
        {
            //var jobInfo = args.Tag as SnmpObjectsData.JobInfoBase;

            //if (jobInfo == null)
            //{
            //    // should not happen
            //    args.Value = null;
            //    return;
            //}

            //// refresh data if it is too old (or rebuild the whole tree if necessary)
            //RefreshJobInfo(jobInfo);

            //// re-read the value and send it back via args
            //// (we should do re-read independently on whether above call to RefreshXxx()
            //// had updated the value or not, because the value could be updated meanwhile by a similar call
            //// in another thread if multiple values are requested)
            //args.Value = args.IbaSnmp.GetValue(args.Oid);
        }

        #endregion

        #endregion


        #region Tree Snapshot for GUI and MIB generation

        public Dictionary<string, SnmpTreeNodeTag> GetObjectTreeSnapShot()
        {
            try
            {
                // check tree structure before taking a snapshot
                RebuildTreeIfItIsInvalid();

                //var result = new Dictionary<string, SnmpTreeNodeTag>();
                //var objList = IbaOpcUaServer.GetListOfAllOids();
                //if (objList == null)
                //{
                //    return null;
                //}

                //var rootOid = IbaOpcUaServer.OidIbaRoot;

                //// get a set of all folders and nodes starting with the root
                //var nodesSet = new HashSet<string> { rootOid };
                //foreach (var oid in objList)
                //{
                //    // skip everything that is outside selected root
                //    if (!oid.StartsWith(rootOid))
                //    {
                //        continue;
                //    }

                //    // add object itself
                //    nodesSet.Add(oid);

                //    // add object's parents (folder-nodes)
                //    var parents = oid.GetParents();
                //    foreach (var parent in parents)
                //    {
                //        if (parent.StartsWith(rootOid))
                //        {
                //            nodesSet.Add(parent);
                //        }
                //    }
                //}

                // retrieve information about each node
                //foreach (var oid in nodesSet)
                //{
                //    var tag = GetTreeNodeTag(oid, true);
                //    if (tag != null)
                //    {
                //        result.Add(oid, tag);
                //    }
                //}

                // mark some nodes as expanded
                var nodesToExpand = new HashSet<string>
                {
                    //rootOid,
                    //IbaOpcUaServer.OidIbaRoot,
                    //IbaOpcUaServer.OidIbaProduct,
                    //IbaOpcUaServer.OidIbaProductSpecific,
                    ////IbaSnmp.OidIbaProductSpecific + SnmpObjectsData.GlobalCleanupOid,// not needed
                    //IbaOpcUaServer.OidIbaProductSpecific + SnmpObjectsData.StandardJobsOid,
                    //IbaOpcUaServer.OidIbaProductSpecific + SnmpObjectsData.ScheduledJobsOid,
                    //IbaOpcUaServer.OidIbaProductSpecific + SnmpObjectsData.OneTimeJobsOid
                };

                //foreach (var oid in nodesToExpand)
                //{
                //    SnmpTreeNodeTag tag;
                //    if (result.TryGetValue(oid, out tag))
                //    {
                //        tag.IsExpandedByDefault = true;
                //    }
                //}

                //return result;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(SnmpWorker)}.{nameof(GetObjectTreeSnapShot)}. {ex.Message}");
                return null;
            }

            return null;
        }

        /// <summary> Gets all information about a node in the format convenient for GUI tree. </summary>
        public SnmpTreeNodeTag GetTreeNodeTag(string oid, bool bUpdate = false)
        {
            try
            {
                //var tag = new SnmpTreeNodeTag { Oid = oid };

                //stringMetadata metadata = IbaOpcUaServer.GetOidMetadata(oid);
                //if (metadata == null)
                //{
                //    // this is inexisting node
                //    // leave all fields empty
                //    return tag;
                //}

                //// fill data common for folders and leaves
                //tag.MibName = metadata.MibName;
                //tag.MibDescription = metadata.MibDescription;
                //tag.Caption = metadata.GuiCaption;

                //// try to get value (applicable only to objects=leaves)

                //IbaSnmpObjectInfo objInfo;
                //try
                //{
                //    objInfo = IbaOpcUaServer.GetObjectInfo(oid, bUpdate);
                //}
                //catch (Exception ex)
                //{
                //    // should not happen, so better to see it if it happens
                //    LogData.Data.Logger.Log(Level.Exception,
                //        $"{nameof(SnmpWorker)}.{nameof(GetTreeNodeTag)}.({oid}). Error calling GetObjectInfo(). {ex.Message}");
                //    return null;
                //}

                //// check  if this is a folder or leaf
                //// object (leaf) can miss a value but anyway should have some data type
                //if (!String.IsNullOrWhiteSpace(objInfo?.MibDataType))
                //{
                //    // this is a leaf node
                //    tag.IsFolder = false;

                //    tag.Value = IbaOpcUaServer.IsEnumDataTypeRegistered(objInfo.ValueType)
                //        ?
                //        // enum - format it like e.g. "1 (started)"
                //        $@"{objInfo.Value} ({IbaOpcUaServer.GetEnumValueName(objInfo.ValueType, (int)objInfo.Value)})"
                //        :
                //        // other types - just value
                //        objInfo.Value?.ToString() ?? "";


                //    tag.Type = objInfo.MibDataType;
                //}
                //else
                //{
                //    // this is a folder
                //    tag.IsFolder = true;
                //}
                //return tag;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(SnmpWorker)}.{nameof(GetTreeNodeTag)}({oid}). {ex.Message}");
                return null;
            }

            return null;
        }

        #endregion

    }
}
