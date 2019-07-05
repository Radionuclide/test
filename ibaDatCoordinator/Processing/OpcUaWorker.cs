using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using iba.Data;
using iba.ibaOPCServer;
using iba.Logging;
using iba.Properties;
using ibaOpcServer.IbaOpcUa;
using Opc.Ua;
using Opc.Ua.Configuration;

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

    #region Event handler classes
    
    public abstract class IbaOpcUaValueRequestedEventArgs : EventArgs
    {
        public OpcUaWorker Worker { get; }

        // todo. kls. to comment
        public string Id { get; }

        /// <summary> Reference to object-specific data needed to refresh the value </summary>
        public object Tag { get; }

        protected IbaOpcUaValueRequestedEventArgs(OpcUaWorker worker, string id, object tag)
        {
            Worker = worker;
            Id = id;
            Tag = tag;
        }

        protected IbaOpcUaValueRequestedEventArgs(IbaOpcUaValueRequestedEventArgs e)
            : this(e.Worker, e.Id, e.Tag)
        {
        }
    }

    public class IbaOpcUaValueRequestedEventArgs<T> : IbaOpcUaValueRequestedEventArgs
    {
        public T Value { get; set; }

        public IbaOpcUaValueRequestedEventArgs(OpcUaWorker worker, string id, object tag)
            : base(worker, id, tag)
        {
        }

        public IbaOpcUaValueRequestedEventArgs(IbaOpcUaValueRequestedEventArgs e)
            : base(e)
        {
        }
    }

    public class IbaOpcUaObjectValueRequestedEventArgs : IbaOpcUaValueRequestedEventArgs<object>
    {
        public Type ValueType { get; }

        public IbaOpcUaObjectValueRequestedEventArgs(OpcUaWorker worker, string id, Type valueType,object tag)
            : base(worker, id, tag)
        {
            this.ValueType = valueType;
        }

        public IbaOpcUaObjectValueRequestedEventArgs(IbaOpcUaValueRequestedEventArgs e, Type valueType)
            : base(e)
        {
            this.ValueType = valueType;
        }


    }

    #endregion

    #endregion

    public class OpcUaWorker : IDisposable
    {
        private BaseDataVariableState _lifeBeatVar;
        private BaseDataVariableState _lifeBeatVar2;
        public BaseDataVariableState _lifeBeatReactive;
        private readonly System.Windows.Forms.Timer _lifebeatTimer = new System.Windows.Forms.Timer {Enabled = false, Interval = 300};

        public static int TmpLifebeatValue { get; private set; } // todo. kls. only if enabled

        #region Construction, Destruction, Init

        private string _ibaDatCoordinatorUaServerStr = "ibaDatCoordinatorUaServer";

        public OpcUaWorker()
        {
            Status = SnmpWorkerStatus.Errored;
            StatusString = Resources.opcUaStatusNotInit;

            _lifebeatTimer.Enabled = true;

            _lifebeatTimer.Tick += (sender, args) =>
            {
                try
                {
                    TmpLifebeatValue++;
                    if (_lifeBeatVar != null)
                    {
                        NodeManager?.SetValueScalar(_lifeBeatVar, TmpLifebeatValue + 10000);
                        NodeManager?.SetValueScalar(_lifeBeatVar2, TmpLifebeatValue + 20000);
                    }
                }
                catch
                {
                    TmpLifebeatValue = 0;
                    /**/
                }
            };
        }

        public void Dispose()
        {
            _lifebeatTimer.Stop(); // todo. kls. 
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

                // todo. kls. Test exceptions on init

                // start the server.
                if (Environment.UserInteractive)
                    _uaApplication.Start(IbaOpcUaServer);
                else
                    // todo - launch as service not tested!
                    _uaApplication.StartAsService(IbaOpcUaServer);

                //_uaServer.OnTrustModeChanged += trustModeChangedHandler;



                IbaOpcUaServer.KlsInitialize(null, null, false);

                //_opcUaData.EndPointString = IbaOpcUaServer.KlsStrEndpointTcp; // todo. kls. 

                Tst__CreateTestTree();
            }

            RestartServer();

            TaskManager.Manager.SnmpConfigurationChanged += TaskManager_SnmpConfigurationChanged;
            ExtMonData.ExtMonGroup.AgeThreshold = SnmpObjectsDataValidTimePeriod;

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
                // 1. setup SNMP manager to monitor some yet non-existing job. it will show "no such instance". ok.
                // 2. Add one ore several jobs to fit the requested OID area. 
                //    Tree will be invalidated but not rebuilt. manager will still show "n.s.i." - wrong.
            };

            RegisterEnums();
            SetGeneralProductInformation();
            RebuildTree();
        }

        public void Tst__CreateTestTree()
        {
            FolderState ibaRootFolder = NodeManager.FolderIbaRoot;
            //var folderTest = NodeManager.KlsCreateFolderAndItsNode(parentFolder, "Test");

            _lifeBeatVar =
                NodeManager.CreateVariableAndItsNode(NodeManager.FolderIbaStatus, BuiltInType.Int32, "Lifebeat");
            _lifeBeatReactive =
                NodeManager.CreateVariableAndItsNode(NodeManager.FolderIbaStatus, BuiltInType.Int32, "Lifebeat reactive");

            _lifeBeatReactive.OnReadValue += OnReadValue1; // todo. kls. 
            //IbaVariable iv;
            //iv.StateChanged

            NodeManager.SetValueScalar(_lifeBeatVar, 0);
            NodeManager.SetValueScalar(_lifeBeatReactive, 0);
        }


        private int _tmpval = 1;
        private ServiceResult OnReadValue1(ISystemContext context, NodeState node,
            NumericRange indexrange, QualifiedName dataencoding, ref object value, ref StatusCode statuscode, ref DateTime timestamp)

        {

            value = (int)value + 1000000;
            //value = _tmpval++;
            //statuscode = 
            //timestamp = DateTime.Now;
            return ServiceResult.Good;
        }


        public string Tst__GetInternalEndpoints()
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


        #region Configuration of UA server

        ApplicationInstance _uaApplication;
        private IbaUaNodeManager NodeManager => IbaOpcUaServer.IbaUaNodeManager;

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
            try
            {
                var oldStatus = Status;
                Status = SnmpWorkerStatus.Errored;
                StatusString = @"";

                IbaOpcUaServer.Stop(); 
                ApplyConfigurationToUaServer(); // todo
                string logMessage;

                //_uaApplication.Stop();
                //_uaApplication.Start(IbaOpcUaServer);

                if (_opcUaData.Enabled)
                {
                    IbaOpcUaServer.Start(_uaApplication.ApplicationConfiguration);
                    Tst__CreateTestTree();

                    //IbaOpcUaServer.Start(_uaApplication.ApplicationConfiguration, uri);
                    Status = SnmpWorkerStatus.Started;
                    StatusString = String.Format(Resources.opcUaStatusRunningOnPort, _opcUaData.Endpoints[0]); // todo simplify strings???

                    logMessage = Status == oldStatus
                        ?
                       // log 'was restarted' if status has not changed (now is 'Started' as before) 
                       String.Format(Resources.opcUaStatusRunningRestarted, StatusString)
                        :
                        // log 'was started' if status has changed from 'Errored' or 'Stopped' to 'Started' 
                        String.Format(Resources.opcUaStatusRunningStarted, StatusString);
                }
                else
                {
                    Status = SnmpWorkerStatus.Stopped;
                    StatusString = Resources.opcUaStatusDisabled;

                    logMessage = Status == oldStatus
                        ?
                        // do not log anything if status has not changed (now is 'Stopped' as before) 
                        null
                        :
                        // log 'was stopped' if status has changed from 'Errored' or 'Started' to 'Stopped'
                        String.Format(Resources.opcUaStatusStopped, StatusString);
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
                StatusString = String.Format(Resources.opcUaStatusError, ex.Message);
                if (LogData.Data.Logger.IsOpen) LogData.Data.Logger.Log(Level.Exception, StatusString);
            }
        }

        private void ApplyConfigurationToUaServer()
        {
            if (IbaOpcUaServer == null || OpcUaData == null)
            {
                return;
            }

            _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Clear();
            foreach (var ep in _opcUaData.Endpoints)
                _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Add(ep.Uri);

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
        public readonly object LockObject = new object(); //todo share lock with SNMP?

        public int LockTimeout { get; } = 50;

        /// <summary> Data older than this will be trated as outdated. 
        /// When requested, such data will be refreshed first before sending via SNMP. </summary>
        public TimeSpan SnmpObjectsDataValidTimePeriod { get; } = TimeSpan.FromSeconds(2);

        /// <summary> Holds all data that is shown via SNMP. 
        /// This data is in convenient structured format, and does not contain SNMP adresses (OIDs) explicitly.
        /// This structure is filled by TaskManager and then is used by SnmpWorker to create SNMP-tree.
        /// </summary>
        internal ExtMonData ObjectsData { get; } = new ExtMonData(); // odo share data with SNMP?

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
            // here I use double than normal timeout to give priority over other locks
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

                    IbaOpcUaServer.IbaUaNodeManager.DeleteAllUserValues();

                    if (!man.SnmpRebuildObjectsData(ObjectsData))
                    {
                        return false; // rebuild failed
                    }

                    foreach (var node in ObjectsData.FolderRoot.Children)
                    {
                        Debug.Assert(node is ExtMonData.ExtMonFolder);
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                        if (node is ExtMonData.ExtMonFolder xmFolder)
                            BuildFolderRecursively(null, xmFolder);
                    }


                    // todo. kls. remove
                    //_lifeBeatVar2 =
                    //    NodeManager.CreateVariableAndItsNode(ObjectsData.FolderGlobalCleanup.UaFullId, BuiltInType.Int32, "Lifebeat2");
                    //_lifeBeatVar2 =
                    //    NodeManager.CreateVariableAndItsNode(null, BuiltInType.Int32, "Lifebeat2");

                    // uniqueness test 
                    //CreateUaFolder(sectionFolder, null, "null");
                    //CreateUaFolder(sectionFolder, "", "empty");
                    //CreateUaFolder(sectionFolder, " ", "wh1");
                    //CreateUaFolder(sectionFolder, " ", "wh1");
                    //CreateUaFolder(sectionFolder, "  ", "wh2");
                    //CreateUaFolder(sectionFolder, "Abc", " ");
                    //CreateUaFolder(sectionFolder, "Abc", " ");
                    //CreateUaFolder(sectionFolder, "Abc", " ");
                    //CreateUaFolder(sectionFolder, "A.B", " ");
                    //CreateUaFolder(sectionFolder, "A.B", " ");

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

        private void BuildFolderRecursively(FolderState uaParentFolder, ExtMonData.ExtMonFolder startingFolder)
        {
            try
            {
                FolderState uaFolder = CreateUaFolder(uaParentFolder, startingFolder);
                
                foreach (var node in startingFolder.Children)
                {
                    switch (node)
                    {
                        case ExtMonData.ExtMonFolder xmFolder:
                            BuildFolderRecursively(uaFolder, xmFolder);

                            break;
                        case ExtMonData.ExtMonVariableBase xmv:
                            CreateUserValue2(uaFolder, xmv, GlobalCleanupDriveInfoItemRequested); // todo. kls. handler
                            break;
                        default:
                            continue;
                    }
                }
            }
            catch
            {
                Debug.Assert(false); // should not happen
                // go on with other items 
                // even if current one has failed 
            }

        }


        #region Create Value, Create Folder helper functions

        private FolderState CreateUaFolder(FolderState uaParentFolder, string displayName, string description)
            => NodeManager.CreateFolderAndItsNode(uaParentFolder ?? NodeManager.FolderIbaRoot, displayName, description);

        private FolderState CreateUaFolder(FolderState uaParentFolder, ExtMonData.ExtMonFolder xmFolderToCreate)
        {
            // create
            FolderState folder = CreateUaFolder(uaParentFolder, xmFolderToCreate.Caption, xmFolderToCreate.Description);
            // keep UA id in ExtMon Node
            xmFolderToCreate.UaFullId = folder.NodeId.Identifier as string;
            return folder;
        }

        private IbaOpcUaVariable CreateUserValue(FolderState parent, object initialValue,
            string name, string description = null,
            EventHandler<IbaOpcUaObjectValueRequestedEventArgs> handler = null, object tag = null)
        {
            // get uaType automatically from initial value
            var uaType = IbaUaNodeManager.GetOpcUaType(initialValue);
            Debug.Assert(uaType != BuiltInType.Null);

            IbaOpcUaVariable iv = NodeManager.CreateVariableAndItsNode(parent, uaType, name, description);

            NodeManager.SetValueScalar(iv, initialValue);
            iv.OnReadValue += OnReadDriveInfoValue;
            return iv;
        }

        private IbaOpcUaVariable CreateUserValue2(FolderState parent, ExtMonData.ExtMonVariableBase xmv, 
            EventHandler<IbaOpcUaObjectValueRequestedEventArgs> handler = null)
        {
            IbaOpcUaVariable iv = NodeManager.CreateVariableAndItsNode(parent, xmv.ObjValue, xmv.Caption, xmv.Description);
            
            // keep cross reference between internal variable and UA variable for instant access
            xmv.UaVar = iv;
            iv.ExtMonVar = xmv;

            // add handler
            iv.OnReadValue += OnReadDriveInfoValue;
            return iv;
        }

        
        // todo. kls. to comment
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
        private bool RefreshGroup(ExtMonData.ExtMonGroup xmGroup)
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

                    if (xmGroup.IsUpToDate())
                    {
                        // data is fresh, no need to change something
                        return false; // was not updated
                    }

                    var man = TaskManager.Manager;

                    bool bSuccess;
                    switch (xmGroup)
                    {
                        case ExtMonData.GlobalCleanupDriveInfo driveInfo:
                            bSuccess = man.SnmpRefreshGlobalCleanupDriveInfo(driveInfo);
                            break;
                        case ExtMonData.JobInfoBase jobInfo:
                            bSuccess = man.SnmpRefreshJobInfo(jobInfo);
                            break;
                        default:
                            // should not happen
                            Debug.Assert(false);
                            bSuccess = false;
                            break;
                    }

                    if (!bSuccess)
                    {
                        // should not happen
                        // failed to update data
                        // rebuild the tree
                        LogData.Data.Logger.Log(Level.Debug,
                            $"OPC UA. {nameof(RefreshGroup)}. Failed to refresh group {xmGroup.Caption}; tree is marked invalid.");
                        IsStructureValid = false;
                        Debug.Assert(false);
                        return false; // data was NOT updated
                    }

                    // TaskManager has updated driveInfo successfully 
                    // copy it to UA tree

                    foreach (var xmv in xmGroup.GetFlatListOfAllVariables())
                    {
                        // todo. kls. 
                        NodeManager.SetValueScalar(xmv.UaVar, xmv.ObjValue);
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
                        $"SNMP. // todo. kls. to comment Error acquiring lock when updating {xmGroup.Caption}, {GetCurrentThreadString()}.");
                }
                catch
                {
                    // logging is not critical
                }
                return false; // data was NOT updated
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

        private void GlobalCleanupDriveInfoItemRequested(object sender, IbaOpcUaObjectValueRequestedEventArgs args)
        {
            var driveInfo = args.Tag as ExtMonData.GlobalCleanupDriveInfo;

            if (driveInfo == null)
            {
                // should not happen
                args.Value = null;
            }

            // refresh data if it is too old (or rebuild the whole tree if necessary)
            RefreshGroup(driveInfo);

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            //args.Value = args.Worker.NodeManager.g.GetValue(args.Oid);
        }

        private ServiceResult OnReadDriveInfoValue(ISystemContext context,
            NodeState node, NumericRange indexrange, QualifiedName dataencoding, ref object value, ref StatusCode statuscode, ref DateTime timestamp)
        {
            if (!(node is IbaOpcUaVariable iv) /*we handle only iba variables here*/|| 
                !(iv.ExtMonVar.Parent is ExtMonData.GlobalCleanupDriveInfo driveInfo))
            {
                value = null;
                statuscode = StatusCodes.Bad;
                return ServiceResult.Good; // // todo. kls. Good?
            }


            Debug.Assert(iv.Value == value); // should be the same

            var oldValue = iv.Value; // todo. kls. tmp

            // refresh data if it is too old (or rebuild the whole tree if necessary)
            RefreshGroup(driveInfo);

            if (!oldValue.Equals(iv.Value))
                // changed
                ; // todo. kls. 

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            value = iv.Value;
            statuscode = iv.StatusCode;

            return ServiceResult.Good;
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

        public Dictionary<NodeId, SnmpTreeNodeTag> GetObjectTreeSnapShot()
        {
            try
            {
                // check tree structure before taking a snapshot
                RebuildTreeIfItIsInvalid();

                var result = new Dictionary<NodeId, SnmpTreeNodeTag>();
                //var objList = IbaOpcUaServer.GetListOfAllOids();
                //if (objList == null)
                //{
                //    return null;
                //}

                //var rootOid = IbaOpcUaServer.KlsStrVarTree.OidIbaRoot;

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
                var nodesToExpand = new HashSet<string>();

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
