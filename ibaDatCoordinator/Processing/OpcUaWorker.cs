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
    public class OpcUaWorker : IDisposable
    {
        #region Construction, Destruction, Init

        public OpcUaWorker()
        {
            Status = ExtMonWorkerStatus.Errored;
            StatusString = Resources.opcUaStatusNotInit;
        }

        public void Dispose()
        {
        }

        public void Init()
        {
            if (_uaApplication != null) //todo
            {
                // disable double initialization
                return;
            }

            // turn on monitoring timer
            InitializeMonitoringTimer();


            _uaApplication = new ApplicationInstance();
            //                IbaOpcUaServer = new IbaOpcUaServer(); 

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
            }

            RestartServer();

            TaskManager.Manager.SnmpConfigurationChanged += TaskManager_SnmpConfigurationChanged;

            // create the timer for delayed tree rebuild
            _treeValidatorTimer = new System.Timers.Timer
            {
                Interval = ExtMonData.AgeThreshold.TotalMilliseconds,
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

            if (OpcUaData.Enabled)
                RebuildTree();
        }


        public string Tst__GetInternalEndpoints()
        {
            string str = "";
            foreach (string adr in _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses)
            {
                str += adr + "; ";
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

        public IbaOpcUaServer IbaOpcUaServer { get; private set; }

        /// <summary> A quick reference to <see cref="IbaOpcUaServer"/>.<see cref="IbaUaNodeManager"/> </summary>
        private IbaUaNodeManager NodeManager => IbaOpcUaServer.IbaUaNodeManager;

        private string _ibaDatCoordinatorUaServerStr = "ibaDatCoordinatorUaServer";

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

        /// <summary> Started / Stopped / Errored </summary>
        public ExtMonWorkerStatus Status { get; private set; }

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
                Status = ExtMonWorkerStatus.Errored;
                StatusString = @"";

                IbaOpcUaServer.Stop();
                ApplyConfigurationToUaServer(); // todo
                string logMessage;

                //_uaApplication.Stop();
                //_uaApplication.Start(IbaOpcUaServer);

                if (_opcUaData.Enabled)
                {
                    IbaOpcUaServer.Start(_uaApplication.ApplicationConfiguration);

                    //IbaOpcUaServer.Start(_uaApplication.ApplicationConfiguration, uri);
                    Status = ExtMonWorkerStatus.Started;
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
                    Status = ExtMonWorkerStatus.Stopped;
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
                Status = ExtMonWorkerStatus.Errored;
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

            // todo. kls. remove tst
            if (_opcUaData.Endpoints.Count > 1)
                _opcUaData.Endpoints.RemoveAt(1);
            if (_opcUaData.Endpoints.Count > 1)
                _opcUaData.Endpoints.RemoveAt(1);

            foreach (var ep in _opcUaData.Endpoints)
                _uaApplication.ApplicationConfiguration.ServerConfiguration.BaseAddresses.Add(ep.Uri);
        }

        #endregion


        #region Handling object tree - building, refreshing

        /// <summary> A quick reference to <see cref="ExtMonData"/> instance lock </summary>
        private object LockObject => ExtMonData.InstanceLockObject;

        /// <summary> A quick reference to <see cref="ExtMonData"/> recommended lock timeout </summary>
        private int LockTimeout => ExtMonData.LockTimeout;
        
        #region Register enums

        // todo. kls. use or delete?
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


        #region Building and rebuilding the tree

        private bool _isStructureValid;

        public bool IsStructureValid
        {
            get => _isStructureValid;
            set
            {
                // this implementation works properly if called from different threads
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
                        $"{nameof(OpcUaWorker)}. Error acquiring lock when checking whether tree is valid, {GetCurrentThreadString()}.");
                }
                catch { /* logging is not critical */ }

                return false; // tree structure has not changed
            }
        }

        HashSet<IbaOpcUaVariable> _deletionPendingNodes;

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
                    // obj structure is valid until datCoordinator configuration is changed.
                    // theoretically it can be reset to false by another thread
                    // during the process of rebuild of ExtMonData,
                    // but it's not a problem. 
                    // If this happens, then the tree will be rebuilt once again.
                    // this is better than to lock resetting of IsStructureValid (and consequently have potential risk of a deadlock).
                    IsStructureValid = true;

                    _deletionPendingNodes = new HashSet<IbaOpcUaVariable>(NodeManager.GetFlatListOfAllIbaVariables());

                    if (!man.ExtMonRebuildObjectsData(ExtMonData.Instance))
                    {
                        return false; // rebuild failed
                    }

                    foreach (var node in ExtMonData.Instance.FolderRoot.Children)
                    {
                        Debug.Assert(node is ExtMonData.ExtMonFolder);
                        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                        if (node is ExtMonData.ExtMonFolder xmFolder)
                            BuildFolderRecursively(null, xmFolder);
                    }

                    // delete nodes that were marked for deletion and were not updated
                    foreach (var node in _deletionPendingNodes)
                    {
                        if (node is IbaOpcUaVariable iv )
                        {
                            // remove handler
                            // ReSharper disable once DelegateSubtraction
                            iv.OnReadValue -= OnReadProductSpecificValue;

                            // delete node, but don't delete empty folders yet (to preserve section folders)
                            NodeManager.DeleteNodeRecursively(iv, false);
                        }
                    }

                    // delete all empty folders. (section folders will be preserved even if empty)
                    NodeManager.DeleteEmptySubfolders();

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
                        $"{nameof(OpcUaWorker)}.{nameof(RebuildTree)}. Error acquiring lock when rebuilding the tree, {GetCurrentThreadString()}.");
                }
                catch { /* logging is not critical */ }

                return false; // rebuild failed
            }
        }

        private void BuildFolderRecursively(FolderState uaParentFolder, ExtMonData.ExtMonFolder startingFolder)
        {
            try
            {
                FolderState uaFolder = CreateOrUpdateOpcUaFolder(uaParentFolder, startingFolder);

                foreach (var node in startingFolder.Children)
                {
                    switch (node)
                    {
                        case ExtMonData.ExtMonFolder xmFolder:
                            BuildFolderRecursively(uaFolder, xmFolder);
                            break;
                        case ExtMonData.ExtMonVariableBase xmv:
                            CreateOrUpdateOpcUaValue(uaFolder, xmv);
                            break;
                        default:
                            continue;
                    }
                }
            }
            catch
            {
                ;// todo. kls. 
                ;
                //Debug.Assert(false); // should not happen
                // go on with other items 
                // even if current one has failed 
            }

        }


        #region Create/Update Value/Folder

        private FolderState CreateOrUpdateOpcUaFolder(FolderState uaParentFolder, ExtMonData.ExtMonFolder xmFolderToCreate)
        {
            if (uaParentFolder == null)
                uaParentFolder = NodeManager.FolderIbaRoot;

            Debug.Assert(IbaUaNodeManager.IsValidBrowseName(xmFolderToCreate.UaBrowseName));

            NodeState node = NodeManager.Find(xmFolderToCreate.UaFullPath);

            Debug.Assert(node == null || node.NodeId.Identifier as string == xmFolderToCreate.UaFullPath);

            switch (node)
            {
                case null:
                    // node does not exist. Create it
                    return CreateOpcUaFolder(uaParentFolder, xmFolderToCreate);
                case FolderState folder:
                    // such folder already exists;
                    // update its Caption and Description
                    // because it could be renamed or reordered
                    folder.DisplayName = xmFolderToCreate.Caption;
                    folder.Description = xmFolderToCreate.Description;
                    return folder;
                default:
                    // node exists but it's not a folder
                    // it's strange, and it should not happen...
                    Debug.Assert(false);
                    return null;
            }
        }

        private FolderState CreateOpcUaFolder(FolderState uaParentFolder, ExtMonData.ExtMonFolder xmFolderToCreate)
        {
            // create
            FolderState folder = NodeManager.CreateFolderAndItsNode(
                uaParentFolder ?? NodeManager.FolderIbaRoot,
                xmFolderToCreate.UaBrowseName, xmFolderToCreate.Caption, xmFolderToCreate.Description);

            // ensure created NodeId looks as expected
            Debug.Assert(folder.NodeId.Identifier as string == xmFolderToCreate.UaFullPath);

            return folder;
        }


        private IbaOpcUaVariable CreateOrUpdateOpcUaValue(FolderState uaParentFolder, ExtMonData.ExtMonVariableBase xmv)
        {
            Debug.Assert(IbaUaNodeManager.IsValidBrowseName(xmv.UaBrowseName));

            NodeState node = NodeManager.Find(xmv.UaFullPath);

            switch (node)
            {
                case null:
                    // node does not exist. Create it
                    return CreateOpcUaValue(uaParentFolder, xmv);
                case IbaOpcUaVariable iv:
                    // such variable already exists
                    // let's update cross reference
                    iv.SetCrossReference(xmv);
                    // this node is present in ExtMonData and should NOT be deleted
                    _deletionPendingNodes.Remove(iv);
                    // Caption and Description theoretically never change for variables
                    Debug.Assert(iv.DisplayName == xmv.Caption);
                    Debug.Assert(iv.Description == xmv.Description);
                    return iv;
                default:
                    // node exists but it's not IbaOpcUaVariable
                    // it's strange, and it should not happen...
                    Debug.Assert(false);
                    return null;
            }
        }

        private IbaOpcUaVariable CreateOpcUaValue(FolderState uaParentFolder, ExtMonData.ExtMonVariableBase xmv)
        {
            IbaOpcUaVariable iv = NodeManager.CreateIbaVariable(uaParentFolder, xmv);

            // ensure created NodeId looks as expected
            Debug.Assert(iv.NodeId.Identifier as string == xmv.UaFullPath);

            // add handler
            iv.OnReadValue += OnReadProductSpecificValue;
            return iv;
        }

        /// <summary> Transfers value from <see cref="ExtMonData.ExtMonVariableBase"/>
        /// to a corresponding OPC UA node </summary>
        private void SetOpcUaValue(ExtMonData.ExtMonVariableBase xmv)
        {
            if (xmv?.UaVar == null)
                return;
            NodeManager.SetValueScalar(xmv.UaVar, xmv.ObjValue);
        }

        #endregion

        #endregion


        #region Value refresh and OnRead event handlers


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
                        case ExtMonData.LicenseInfo licenseInfo:
                            bSuccess = man.ExtMonRefreshLicenseInfo(licenseInfo);
                            break;
                        case ExtMonData.GlobalCleanupDriveInfo driveInfo:
                            bSuccess = man.ExtMonRefreshGlobalCleanupDriveInfo(driveInfo);
                            break;
                        case ExtMonData.JobInfoBase jobInfo:
                            bSuccess = man.ExtMonRefreshJobInfo(jobInfo);
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
                            $"{nameof(OpcUaWorker)}.{nameof(RefreshGroup)}. Failed to refresh group {xmGroup.Caption}; tree is marked invalid.");
                        IsStructureValid = false;
                        return false; // data was NOT updated
                    }

                    // TaskManager has updated group successfully;
                    // copy it to UA tree
                    foreach (var xmv in xmGroup.GetFlatListOfAllVariables())
                    {
                        SetOpcUaValue(xmv);
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
                        $"{nameof(OpcUaWorker)}.{nameof(RefreshGroup)}. Error acquiring lock when updating {xmGroup.Caption}, {GetCurrentThreadString()}.");
                }
                catch { /* logging is not critical */ }

                return false; // data was NOT updated
            }
        }


        private ServiceResult OnReadProductSpecificValue(ISystemContext context,
            NodeState node, NumericRange indexRange, QualifiedName dataEncoding,
            ref object value, ref StatusCode statusCode, ref DateTime timestamp)
        {
            if (!(node is IbaOpcUaVariable iv)) //we handle only iba variables here 
            {
                Debug.Assert(false); // should not happen
                value = null;
                statusCode = StatusCodes.Bad;
                return ServiceResult.Good; // statusCode is bad; serviceResult is good
            }

            if (iv.IsDeleted)
            {
                // don't try to refresh deleted variables
                iv.Value = null;
                Debug.Assert(iv.StatusCode == StatusCodes.BadObjectDeleted);
                iv.StatusCode = StatusCodes.BadObjectDeleted;
            }
            else
            {
                Debug.Assert(iv.Value == value); // should be the same at this point
                Debug.Assert(iv.ExtMonVar.Group != null);

                // refresh data if it is too old (or rebuild the whole tree if necessary)
                RefreshGroup(iv.ExtMonVar.Group);
            }

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            value = iv.Value;
            statusCode = iv.StatusCode;
            Debug.Assert(iv.StatusCode == StatusCodes.Good || iv.StatusCode == StatusCodes.BadObjectDeleted);

            return ServiceResult.Good;
        }

        private object RefreshAndReadNode(string nodeId)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(nodeId));
            if (string.IsNullOrWhiteSpace(nodeId))
                return null;

            var node = NodeManager.Find(nodeId);

            // not found
            if (node == null)
                return null;

            Debug.Assert(node is IbaOpcUaVariable);

            if (!(node is IbaOpcUaVariable iv))
                return null;

            Debug.Assert(iv.ExtMonVar.Group != null);

            RefreshGroup(iv.ExtMonVar.Group);

            return iv.Value;
        }

        #endregion


        #region Monitored itmes handling

        private readonly System.Windows.Forms.Timer _monitoringTimer = new System.Windows.Forms.Timer { Enabled = false };

        private void InitializeMonitoringTimer()
        {
            // set interval to value slightly bigger than AgeThreshold,
            // to ensure we don't request update twice within one interval
            _monitoringTimer.Interval = (int)(ExtMonData.AgeThreshold.TotalMilliseconds * 1.1);
            // let it be always enabled as long as worker lives;
            // if OPC UA is disabled then timer handler does nothing and returns immediately
            _monitoringTimer.Enabled = true;
            _monitoringTimer.Tick += OnMonitoringTimerTick;
        }

        private void OnMonitoringTimerTick(object sender, EventArgs args)
        {
            if (!OpcUaData.Enabled)
                return;

            if (Monitor.TryEnter(LockObject, LockTimeout))
            {
                try
                {
                    var monitoredNodes = NodeManager.GetMonitoredNodes();

                    // todo. kls. remove tmp
                    string tmp___Nodes = "";
                    string tmp___Groups = "";

                    // prepare a set of groups to be refreshed
                    var groups = new HashSet<ExtMonData.ExtMonGroup>();
                    foreach (var kvp in monitoredNodes)
                    {
                        NodeState node = kvp.Value.Node;
                        if (node is IbaOpcUaVariable iv && !iv.IsDeleted && iv.ExtMonVar?.Group != null)
                        {
                            groups.Add(iv.ExtMonVar.Group);
                            tmp___Nodes += $@"{iv.ExtMonVar.Caption}, ";
                        }
                    }

                    // refresh all groups that have monitored items
                    foreach (var group in groups)
                    {
                        RefreshGroup(group);
                        tmp___Groups += $@"{group.Caption}, ";
                    }

                    tmp___Groups = tmp___Groups.TrimEnd(' ', ',');
                    tmp___Nodes = tmp___Nodes.TrimEnd(' ', ',');
                    // todo. kls. remove override
                    StatusString = $@"Monitored {monitoredNodes.Count}: {{ I=[{tmp___Nodes}] G=[{tmp___Groups}] }}";
                }
                catch
                {
                    // todo. kls. 
                    Debug.Assert(false);
                    ;
                    /**/
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
                        $"{nameof(OpcUaWorker)}.{nameof(OnMonitoringTimerTick)}. Error acquiring lock, {GetCurrentThreadString()}.");
                }
                catch { /* logging is not critical */ }
            }
        }

        #endregion

        #endregion


        #region Tree Snapshot for GUI

        internal List<ExtMonData.GuiTreeNodeTag> GetObjectTreeSnapShot()
        {
            try
            {
                // check tree structure before taking a snapshot
                RebuildTreeIfItIsInvalid();

                var result = new List<ExtMonData.GuiTreeNodeTag>();
                var objList = NodeManager.GetFlatListOfAllChildren();
                if (objList == null)
                {
                    return null;
                }

                //retrieve information about each node
                foreach (BaseInstanceState uaNode in objList)
                {
                    var tag = GetTreeNodeTag(uaNode, true);
                    if (tag != null)
                    {
                        result.Add(tag);
                    }
                }

                return result;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(OpcUaWorker)}.{nameof(GetObjectTreeSnapShot)}. {ex.Message}");
                return null;
            }
        }

        /// <summary> Gets all information about a node in the format convenient for GUI tree. </summary>
        internal ExtMonData.GuiTreeNodeTag GetTreeNodeTag(BaseInstanceState node, bool bUpdate = false)
        {
            try
            {
                Debug.Assert(node != null);

                // ReSharper disable once ConditionIsAlwaysTrueOrFalse
                if (node == null)
                    // ReSharper disable once HeuristicUnreachableCode
                    return null;

                ExtMonData.GuiTreeNodeTag tag = new ExtMonData.GuiTreeNodeTag
                {
                    Type = "",
                    Value = "",
                    Caption = node.DisplayName.Text,
                    Description = node.Description.Text,
                    IsFolder = true,
                    OpcUaNodeId = node.NodeId.Identifier as string /*we use only string ids*/
                };

                Debug.Assert(tag.OpcUaNodeId != null);

                // ReSharper disable once InvertIf
                if (node is IbaOpcUaVariable iv)
                {
                    tag.IsFolder = false;
                    tag.Value = ""; // default val

                    if (iv.IsDeleted)
                    {
                        tag.Type = "";
                        tag.Value = "";
                    }
                    else /*not deleted; is a valid variable*/
                    {
                        var type = iv.ExtMonVar.ObjValue.GetType();
                        tag.Type = type.IsEnum ? "Enum" : type.Name;

                        // try to get an updated value
                        try
                        {
                            object value = RefreshAndReadNode(tag.OpcUaNodeId);
                            tag.Value = $"{value}";
                        }
                        catch
                        {
                            Debug.Assert(false);
                            tag.Value = "(error)";
                        }
                    }
                }

                return tag;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(OpcUaWorker)}.{nameof(GetTreeNodeTag)}({node}). {ex.Message}");
                return null;
            }
        }

        /// <summary> Gets all information about a node in the format convenient for GUI tree. </summary>
        internal ExtMonData.GuiTreeNodeTag GetTreeNodeTag(string nodeId, bool bUpdate = false)
        {
            try
            {
                Debug.Assert(!string.IsNullOrWhiteSpace(nodeId));

                if (string.IsNullOrWhiteSpace(nodeId))
                {
                    // illegal nodeId
                    return null;
                }

                BaseInstanceState uaNode = NodeManager.Find(nodeId) as BaseInstanceState;
                // ReSharper disable once ConvertIfStatementToReturnStatement
                if (uaNode == null)
                {
                    // requested node is not found; likely is deleted
                    return null;
                }

                return GetTreeNodeTag(uaNode, bUpdate);
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(OpcUaWorker)}.{nameof(GetTreeNodeTag)}({nodeId}). {ex.Message}");
                return null;
            }
        }

        #endregion
    }
}
