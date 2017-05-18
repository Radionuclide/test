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

        public TimeSpan SnmpObjectsDataValidTimePeriod { get; } = TimeSpan.FromSeconds(5);
        private const int InitialisationDelayInSeconds = 1;

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

            RegisterGeneralObjectHandlers();
            RegisterProductObjects();
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
                    IbaSnmp.LicensingIsValidRequested += IbaSnmp_LicensingIsValidRequested;

                    // ibaRoot.DatCoord.General.Licensing.2 - Serial number
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingSn] = new OidMetadata(@"Serial number");
                    IbaSnmp.LicensingSnRequested += IbaSnmp_LicensingSnRequested;

                    // ibaRoot.DatCoord.General.Licensing.3 - Hardware ID
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingHwId] = new OidMetadata(@"Hardware ID");
                    IbaSnmp.LicensingHwIdRequested += IbaSnmp_LicensingHwIdRequested;

                    // ibaRoot.DatCoord.General.Licensing.4 - Dongle type
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingType] = new OidMetadata(@"Dongle type");
                    IbaSnmp.LicensingTypeRequested += IbaSnmp_LicensingTypeRequested;

                    // ibaRoot.DatCoord.General.Licensing.5 - Customer
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingCustomer] = new OidMetadata(@"Customer");
                    IbaSnmp.LicensingCustomerRequested += IbaSnmp_LicensingCustomerRequested;

                    // ibaRoot.DatCoord.General.Licensing.6 - Time limit
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingTimeLimit] = new OidMetadata(@"Time limit");
                    IbaSnmp.LicensingTimeLimitRequested += IbaSnmp_LicensingTimeLimitRequested;

                    // ibaRoot.DatCoord.General.Licensing.7 - Demo time limit
                    OidMetadataDict[IbaSnmp.OidIbaProductGeneralLicensingDemoTimeLimit] = new OidMetadata(@"Demo time limit");
                    IbaSnmp.LicensingDemoTimeLimitRequested += IbaSnmp_LicensingDemoTimeLimitRequested;
                }
            }
        }

        private void IbaSnmp_UpTimeRequested(object sender, IbaSnmpValueRequestedEventArgs<uint> e)
        {
            // todo override?
        }

        private void IbaSnmp_LicensingCustomerRequested(object sender, IbaSnmpValueRequestedEventArgs<string> e)
        {
            try
            {
                CDongleInfo info = CDongleInfo.ReadDongle();
                e.Value = info.DongleFound ? info.Customer : @"(???)";
            }
            catch
            {
                /**/
            }
        }

        private void IbaSnmp_LicensingDemoTimeLimitRequested(object sender, IbaSnmpValueRequestedEventArgs<int> e)
        {
            // todo
        }

        private void IbaSnmp_LicensingHwIdRequested(object sender, IbaSnmpValueRequestedEventArgs<string> e)
        {
            // todo
        }

        private void IbaSnmp_LicensingIsValidRequested(object sender, IbaSnmpValueRequestedEventArgs<bool> e)
        {
            try
            {
                CDongleInfo info = CDongleInfo.ReadDongle();
                // todo is it reaaly this?
                e.Value = info.DongleFound;
            }
            catch
            {
                /**/
            }
        }

        private void IbaSnmp_LicensingSnRequested(object sender, IbaSnmpValueRequestedEventArgs<string> e)
        {
            try
            {
                CDongleInfo info = CDongleInfo.ReadDongle();
                e.Value = info.DongleFound ? info.SerialNr : @"(???)";
            }
            catch
            {
                /**/
            }
        }

        private void IbaSnmp_LicensingTimeLimitRequested(object sender, IbaSnmpValueRequestedEventArgs<int> e)
        {
            // todo
        }

        private void IbaSnmp_LicensingTypeRequested(object sender, IbaSnmpValueRequestedEventArgs<string> e)
        {
            // todo
        }

        #endregion

        #region Dat coordinator specific objects

        public void RefreshObjectData()
        {
            lock (LockObject)
            {
                if (!IsObjectsDataUpToDate())
                {
                    RebuildTreeCompletely();
                }
            }
        }

        private void RebuildTreeCompletely()
        {
            var man = TaskManager.Manager;
            IbaSnmp ibaSnmp = man?.SnmpWorker.IbaSnmp;
            if (ibaSnmp == null)
                return;

            lock (LockObject)
            {
                // todo probabaly not fully recreate but refresh ?
                man.SnmpGetStatus(ObjectsData);

                // todo this is a probabale dead lock?
                // todo needa look where from does this call comes? 
                // todo wheter we are in lock of ibaSnmp.Dict...
                ibaSnmp.DeleteAllUserValues();

                // todo Clean OidCaptions for all product-specific OIDs

                // ibaRoot.DatCoord.1 - Product-Specific
                OidMetadataDict[IbaSnmp.OidIbaProductSpecific] = new OidMetadata(@"Product");

                // ibaRoot.DatCoord.Product.1 - Global cleanup
                BuildSectionGlobalCleanup("1");

                // ibaRoot.DatCoord.Product.2 - Standard jobs
                BuildSectionStandardJobs("2");

                // ibaRoot.DatCoord.Product.3 - Scheduled jobs
                BuildSectionScheduledJobs("3");

                // ibaRoot.DatCoord.Product.4 - One time jobs
                BuildSectionOneTimeJobs("4");
            }
        }


        #region Building of tree Sections 1...4 (from 'GlobalCleanup' to 'OneTimeJobs')

        private void BuildSectionGlobalCleanup(IbaSnmpOid oidSection)
        {
            AddMetadataForOidSuffix(oidSection, @"Global cleanup", @"globalCleanup");

            for (int i = 0; i < ObjectsData.GlobalCleanup.Count; i++)
            {
                try
                {
                    var cleanupInfo = ObjectsData.GlobalCleanup[i];

                    // ibaRoot.DatCoord.Product.GlobalCleanup.(index) - Drive
                    IbaSnmpOid oidDrive = oidSection + (uint)(i + 1);
                    string mibNameDrive = $@"globalCleanupDrive{oidDrive.GetLeastId()}";
                    AddMetadataForOidSuffix(oidDrive, $@"Drive '{cleanupInfo.DriveName}'", mibNameDrive);

                    // ibaRoot.DatCoord.Product.GlobalCleanup.DriveX....
                    {
                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.0 - DriveName
                        CreateUserValue(oidDrive + 0, cleanupInfo.DriveName,
                            @"Drive Name", mibNameDrive + @"Name",
                            null,
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.1 - Active
                        CreateUserValue(oidDrive + 1, cleanupInfo.Active,
                            @"Active", mibNameDrive + @"Active",
                            null,
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.2 - Size
                        CreateUserValue(oidDrive + 2, cleanupInfo.Size,
                            @"Size", mibNameDrive + @"Size",
                            null,
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.3 - Curr. free space
                        CreateUserValue(oidDrive + 3, cleanupInfo.CurrentFreeSpace,
                            @"Curr. free space", mibNameDrive + @"CurrFreeSpace",
                            null,
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.4 - Min free space
                        CreateUserValue(oidDrive + 4, cleanupInfo.MinFreeSpace,
                            @"Min free space", mibNameDrive + @"MinFreeSpace",
                            null,
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.GlobalCleanup.Drive.5 - Rescan time
                        CreateUserValue(oidDrive + 5, cleanupInfo.RescanTime,
                            @"Rescan time", mibNameDrive + @"RescanTime",
                            null,
                            UserValueRequested);
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

        private void BuildSectionStandardJobs(IbaSnmpOid oidSection)
        {
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
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.6 - Timestamp Job started
                        CreateUserValue(oidJobGen + 6, jobInfo.TimestampJobStarted,
                            @"Timestamp job started", mibNameJobGen + @"TimestampJobStarted",
                            null,
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.7 - Last cycle scanning time
                        CreateUserValue(oidJobGen + 7, jobInfo.LastCycleScanningTime,
                            @"Last cycle scanning time", mibNameJobGen + @"LastCycleScanningTime",
                            null,
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8 - LastProcessing [Folder]
                        IbaSnmpOid oidLastProc = oidJobGen + 8;
                        AddMetadataForOidSuffix(oidLastProc, @"LastProcessing", mibNameJobGen + @"LastProcessing");

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8.0 - Last dat-File processed
                        CreateUserValue(oidLastProc + 0, jobInfo.LastProcessingLastDatFileProcessed,
                            @"Last dat-file processed", mibNameJobGen + @"LastFile",
                            null,
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8.1 - Start Timestamp last processing
                        CreateUserValue(oidLastProc + 1, jobInfo.LastProcessingStartTimeStamp,
                            @"Start timestamp", mibNameJobGen + @"StartStamp",
                            null,
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.StdJobs.Job.8.2 - Finish Timestamp last processing
                        CreateUserValue(oidLastProc + 2, jobInfo.LastProcessingFinishTimeStamp,
                            @"Finish timestamp", mibNameJobGen + @"FinishStamp",
                            null,
                            UserValueRequested);
                    }

                    // create tasks
                    BuildTasks(oidJob, mibNameJob, jobInfo.Tasks);
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

        private void BuildSectionScheduledJobs(IbaSnmpOid oidSection)
        {
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
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.SchJobs.Job.6 - TimestampLastExecution
                        CreateUserValue(oidJobGen + 6, jobInfo.TimestampLastExecution,
                            @"Timestamp last execution", mibNameJobGen + @"TimestampLastExecution",
                            null,
                            UserValueRequested);

                        // ibaRoot.DatCoord.Product.SchJobs.Job.7 - TimestampNextExecution
                        CreateUserValue(oidJobGen + 7, jobInfo.TimestampNextExecution,
                            @"Timestamp next execution", mibNameJobGen + @"TimestampNextExecution",
                            null,
                            UserValueRequested);
                    }

                    // create tasks
                    BuildTasks(oidJob, mibNameJob, jobInfo.Tasks);
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

        private void BuildSectionOneTimeJobs(IbaSnmpOid oidSection)
        {
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
                            UserValueRequested);
                    }
                    // create tasks
                    BuildTasks(oidJob, mibNameJob, jobInfo.Tasks);
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
            // ibaRoot.DatCoord.Product.XxxJobs.JobY.0 - General [Folder]
            oidJobGen = oidJob + 0;
            mibNameJobGen = mibNameJob + @"General";
            AddMetadataForOidSuffix(oidJobGen, @"General", mibNameJobGen);

            {
                // ibaRoot.DatCoord.Product.XxxJobs.JobY.0 - Job name
                CreateUserValue(oidJobGen + 0, jobInfo.JobName,
                    @"Job Name", mibNameJobGen + @"Name",
                    null,
                    UserValueRequested);

                // todo add enum
                // ibaRoot.DatCoord.Product.XxxJobs.JobY.1 - Status
                CreateUserValue(oidJobGen + 1, jobInfo.Status.ToString(),
                    @"Status", mibNameJobGen + @"Status",
                    null,
                    UserValueRequested);

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.2 - To do #
                CreateUserValue(oidJobGen + 2, jobInfo.TodoCount,
                    @"Todo #", mibNameJobGen + @"Todo",
                    null,
                    UserValueRequested);

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.3 - Done #
                CreateUserValue(oidJobGen + 3, jobInfo.DoneCount,
                    @"Done #", mibNameJobGen + @"Done",
                    null,
                    UserValueRequested);

                // ibaRoot.DatCoord.Product.XxxJobs.JobY.4 - Failed #
                CreateUserValue(oidJobGen + 4, jobInfo.FailedCount,
                    @"Failed #", mibNameJobGen + @"Failed",
                    null,
                    UserValueRequested);
            }
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
            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.0 - TaskName
            CreateUserValue(oidTask + 0, taskInfo.TaskName,
                @"Task name", mibNameTask + @"Name",
                null,
                UserValueRequested);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.1 - Tasktype 
            CreateUserValue(oidTask + 1, taskInfo.TaskType,
                @"Task type", mibNameTask + @"Type",
                null,
                UserValueRequested);


            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.2 - Success 
            CreateUserValue(oidTask + 2, taskInfo.Success,
                @"Success", mibNameTask + @"Success",
                null,
                UserValueRequested);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.3 - DurationOfLastExecution 
            CreateUserValue(oidTask + 3, taskInfo.DurationOfLastExecution,
                @"Duration of last execution", mibNameTask + @"DurationOfLastExecution",
                null,
                UserValueRequested);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.4 - CurrentMemoryUsed 
            CreateUserValue(oidTask + 4, taskInfo.CurrentMemoryUsed,
                @"Current memory used", mibNameTask + @"CurrentMemoryUsed",
                null,
                UserValueRequested);

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
                UserValueRequested);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.1 - Subdirectories
            CreateUserValue(oidCleanup + 1, ci.Subdirectories,
                @"Subdirectories", mibNameCleanup + @"Subdirectories",
                null,
                UserValueRequested);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.2 - FreeDiskSpace
            CreateUserValue(oidCleanup + 2, ci.Subdirectories,
                @"Free disk space", mibNameCleanup + @"FreeDiskSpace",
                null,
                UserValueRequested);

            // ibaRoot.DatCoord.Product.XxxJobs.JobY.TaskZ.5.3 - UsedDiskSpace
            CreateUserValue(oidCleanup + 3, ci.UsedDiskSpace,
                @"Used disk space", mibNameCleanup + @"UsedDiskSpace",
                null,
                UserValueRequested);
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


        private bool IsObjectsDataUpToDate()
        {
            if (ObjectsData.Stamp == DateTime.MinValue)
            {
                return false;
            }
            TimeSpan age = DateTime.Now - ObjectsData.Stamp;
            // if data is not too old, then it is okay
            return age < SnmpObjectsDataValidTimePeriod;
        }

        private void UserValueRequested(object sender, IbaSnmpObjectValueRequestedEventArgs ibaSnmpObjectValueRequestedEventArgs)
        {
            // refresh data if it is too old, and rebuild all the objects if necessary
            RefreshObjectData();
        }

        private void RegisterProductObjects()
        {
            // todo should be done differently
            RefreshObjectData();
        }
        

        #endregion

        #endregion

    }
}
