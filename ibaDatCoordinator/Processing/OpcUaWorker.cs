﻿using System;
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
        // // todo. kls. remove
        //private BaseDataVariableState _lifeBeatVar;
        //private BaseDataVariableState _lifeBeatVar2;
        //public BaseDataVariableState _lifeBeatReactive;
        //private readonly System.Windows.Forms.Timer _lifebeatTimer = new System.Windows.Forms.Timer {Enabled = false, Interval = 300};

        //public static int TmpLifebeatValue { get; private set; } // todo. kls. only if enabled

        #region Construction, Destruction, Init

        private string _ibaDatCoordinatorUaServerStr = "ibaDatCoordinatorUaServer";

        public OpcUaWorker()
        {
            Status = ExtMonWorkerStatus.Errored;
            StatusString = Resources.opcUaStatusNotInit;

            // todo. kls. remove
            //_lifebeatTimer.Enabled = true;

            //_lifebeatTimer.Tick += (sender, args) =>
            //{
            //    try
            //    {
            //        TmpLifebeatValue++;
            //        if (_lifeBeatVar != null)
            //        {
            //            NodeManager?.SetValueScalar(_lifeBeatVar, TmpLifebeatValue + 10000);
            //            NodeManager?.SetValueScalar(_lifeBeatVar2, TmpLifebeatValue + 20000);
            //        }
            //    }
            //    catch
            //    {
            //        TmpLifebeatValue = 0;
            //        /**/
            //    }
            //};
        }

        public void Dispose()
        {
            //_lifebeatTimer.Stop(); // todo. kls. 
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
            }

            RestartServer();

            TaskManager.Manager.SnmpConfigurationChanged += TaskManager_SnmpConfigurationChanged;
            ExtMonData.ExtMonGroup.AgeThreshold = ExtMonDataValidTimePeriod;

            // create the timer for delayed tree rebuild
            _treeValidatorTimer = new System.Timers.Timer
            {
                Interval = ExtMonDataValidTimePeriod.TotalMilliseconds,
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
            RebuildTree();
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

        /// <summary> Lock this object while using <see cref="ObjectsData"/> </summary>
        public readonly object LockObject = new object(); //todo share lock with SNMP?

        /// todo. kls. share with snmp?
        public int LockTimeout { get; } = 50;


        /// <summary> Data older than this will be treated as outdated. 
        /// When requested, such data will be refreshed first before sending via SNMP.
        /// todo. kls. share with snmp?
        /// </summary>
        public TimeSpan ExtMonDataValidTimePeriod { get; } = TimeSpan.FromSeconds(2);

        /// <summary> Holds all data that is shown via SNMP. 
        /// This data is in convenient structured format, and does not contain SNMP addresses (OIDs) explicitly.
        /// This structure is filled by TaskManager and then is used by SnmpWorker to create SNMP-tree.
        /// </summary>
        internal ExtMonData ObjectsData { get; } = new ExtMonData(); // odo share data with SNMP?

        #region register enums

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
                    // obj structure is valid until datCoordinator configuration is changed.
                    // theoretically it can be reset to false by another thread
                    // during the process of rebuild of ExtMonData,
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
                FolderState uaFolder = CreateOpcUaFolder(uaParentFolder, startingFolder);
                
                foreach (var node in startingFolder.Children)
                {
                    switch (node)
                    {
                        case ExtMonData.ExtMonFolder xmFolder:
                            BuildFolderRecursively(uaFolder, xmFolder);
                            break;
                        case ExtMonData.ExtMonVariableBase xmv:
                            CreateOpcUaValue(uaFolder, xmv);
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


        #region Create/Update Value/Folder

        private FolderState CreateOpcUaFolder(FolderState uaParentFolder, ExtMonData.ExtMonFolder xmFolderToCreate)
        {
            // create
            FolderState folder = NodeManager.CreateFolderAndItsNode(
                uaParentFolder ?? NodeManager.FolderIbaRoot, xmFolderToCreate.Caption, xmFolderToCreate.Description);

            // keep UA id in ExtMon Node
            //xmFolderToCreate.UaFullId = folder.NodeId.Identifier as string; // todo. kls. 
            return folder;
        }

        private IbaOpcUaVariable CreateOpcUaValue(FolderState parent, ExtMonData.ExtMonVariableBase xmv)
        {
            IbaOpcUaVariable iv = NodeManager.CreateVariableAndItsNode(parent, xmv);

            // add handler
            iv.OnReadValue += OnReadProductSpecificValue;
            return iv;
        }

        /// <summary> Transfers value from <see cref="ExtMonData.ExtMonVariableBase"/>
        /// to a corresponding OPC UA node </summary>
        private void SetOpcUaValue(ExtMonData.ExtMonVariableBase xmv)
        {
            Debug.Assert(xmv != null);
            Debug.Assert(xmv.UaVar != null);
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
                            bSuccess = man.SnmpRefreshLicenseInfo(licenseInfo);
                            break;
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
                            $"{nameof(OpcUaWorker)}.{nameof(RefreshGroup)}. Failed to refresh group {xmGroup.Caption}; tree is marked invalid.");
                        IsStructureValid = false;
                        Debug.Assert(false);
                        return false; // data was NOT updated
                    }

                    // TaskManager has updated group successfully 
                    // copy it to UA tree
                    foreach (var xmv in xmGroup.GetFlatListOfAllVariables())
                    {
                        try
                        {
                            SetOpcUaValue(xmv);
                        }
                        catch 
                        {

                        }
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
                catch
                {
                    // logging is not critical
                }
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

            Debug.Assert(iv.Value == value); // should be the same

            Debug.Assert(iv.ExtMonVar.Group != null); 

            // refresh data if it is too old (or rebuild the whole tree if necessary)
            var bResult = RefreshGroup(iv.ExtMonVar.Group);

            // todo. kls. del tst
            if (bResult == false)
                ;

            // re-read the value and send it back via args
            // (we should do re-read independently on whether above call to RefreshXxx()
            // had updated the value or not, because the value could be updated meanwhile by a similar call
            // in another thread if multiple values are requested)
            value = iv.Value;
            statusCode = iv.StatusCode;
            Debug.Assert(iv.StatusCode == StatusCodes.Good);

            return ServiceResult.Good;
        }

        private object RefreshAndReadNode(string nodeId)
        {
            Debug.Assert(!string.IsNullOrWhiteSpace(nodeId));
            if (string.IsNullOrWhiteSpace(nodeId))
                return null;

            var node = NodeManager.Find(nodeId);

            Debug.Assert(node is IbaOpcUaVariable);

            if (!(node is IbaOpcUaVariable iv))
                return null;

            Debug.Assert(iv.ExtMonVar.Group != null);

            RefreshGroup(iv.ExtMonVar.Group);

            return iv.Value;
        }

        #endregion

        #endregion


        #region Tree Snapshot for GUI and MIB generation

        internal List<ExtMonData.GuiTreeNodeTag> GetObjectTreeSnapShot()
        {
            try
            {
                // check tree structure before taking a snapshot
                RebuildTreeIfItIsInvalid();

                var result = new List<ExtMonData.GuiTreeNodeTag>();
                var objList = IbaOpcUaServer.IbaUaNodeManager.GetListOfAllUserNodes();
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
                ////mark some nodes as expanded
                //var nodesToExpand = new HashSet<string>
                //{
                //    "ibaDatCoordinator\\Standard jobs",
                //    "ibaDatCoordinator\\Scheduled jobs",
                //    "ibaDatCoordinator\\One time jobs",
                //    "ibaDatCoordinator\\Event jobs",
                //};

                //foreach (string key in nodesToExpand)
                //{
                //    if (result.TryGetValue(key, out ExtMonData.GuiTreeNodeTag tag))
                //    {
                //        tag.IsExpandedByDefault = true;
                //    }
                //}

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
                    OpcUaNodeId = IbaUaNodeManager.GetNodeIdAsString(node.NodeId)
                };

                Debug.Assert(tag.OpcUaNodeId != null);

                // ReSharper disable once InvertIf
                if (node is IbaOpcUaVariable iv)
                {
                    tag.IsFolder = false;

                    // old val???
                    tag.Value = iv.Value.ToString();
                    tag.Value = ""; // default val

                    var type = iv.ExtMonVar.ObjValue.GetType();
                    tag.Type = type.IsEnum ? "Enum" : type.Name;


                    // try to get an updated value
                    try
                    {
                        // todo. kls. refresh group, get value....
                        object value = RefreshAndReadNode(tag.OpcUaNodeId);
                        tag.Value = $"{value}";

                        //tag.Value = IbaOpcUaServer.IsEnumDataTypeRegistered(objInfo.ValueType)
                        //    ?
                        //    // enum - format it like e.g. "1 (started)"
                        //    $@"{objInfo.Value} ({IbaOpcUaServer.GetEnumValueName(objInfo.ValueType, (int) objInfo.Value)})"
                        //    :
                        //    // other types - just value
                        //    objInfo.Value?.ToString() ?? "";
                    }
                    catch
                    {
                        Debug.Assert(false);
                        tag.Value = "errrr????"; // ??????????????????????
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
