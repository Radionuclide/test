using System;
using System.Collections.Generic;
using System.Diagnostics;
using iba.ibaOPCServer;
using Opc.Ua;

namespace ibaOpcServer.IbaOpcUa
{
    public class IbaUaNodeManagerStatus
    {
        // we need a reference to our node manager to be able to 
        // manage our status nodes from inside of IbaUaNodeManagerStatus class
        private readonly IbaUaNodeManager _mgr;

        // status nodes
        private BaseDataVariableState _nodePmacIsOnline;
        private BaseDataVariableState _nodePmacIsRunning;
        private BaseDataVariableState _nodePmacProjectName;
        private BaseDataVariableState _nodePmacMaxDongleItems;
        private BaseDataVariableState _nodePmacVariablesCount;

        private BaseDataVariableState _nodeVarsCountTotal;
        private BaseDataVariableState _nodeVarsCountGlobal;
        private BaseDataVariableState _nodeVarsCountTask;

        private FolderState _folderMessages;
        private readonly List<BaseDataVariableState> _nodesMessage = new List<BaseDataVariableState>();
        private int _msgCounter;
        private const int maxMessagesCount = 10;

        private BaseDataVariableState _nodeLastTreeUpdateTime;
        private BaseDataVariableState _nodeLastTreeUpdateAdded;
        private BaseDataVariableState _nodeLastTreeUpdateDeleted;
        private BaseDataVariableState _nodeLastTreeUpdateReplaced;
        private BaseDataVariableState _nodeLastTreeUpdateInvalidated;

        #region Construction and Initialization

        public IbaUaNodeManagerStatus(IbaUaNodeManager nodeManager)
        {
            _mgr = nodeManager;

            // do not call CreateStatusTree() here!
            // let IbaUaNodeManager to call it later.
            // first IbaUaNodeManagerStatus should be constructed fully.
            // IbaUaNodeManager uses IbaUaNodeManagerStatus, and IbaUaNodeManagerStatus uses IbaUaNodeManager
            // so be careful with their insatiation!
        }

        /// <summary>
        /// Creates status nodes inside Status Folder. Call this only once.
        /// </summary>
        /// <param name="statusRootFolder"></param>
        public void CreateStatusTree(FolderState statusRootFolder)
        {
            // it should be called only once
            // cancel succeeding calls
            if (_nodePmacIsOnline != null) return;

            // create status subfolders
            // create these instancess as local variables
            // we do not need to keep references to them.
            // outside the function we will not have easy access to folders
            // but we will have access to underlying nodes. that's enough
            FolderState folderPmac = _mgr.KlsCreateFolderAndItsNode(statusRootFolder, "PMAC");
            _folderMessages = _mgr.KlsCreateFolderAndItsNode(statusRootFolder, "Messages");
            FolderState folderVars = _mgr.KlsCreateFolderAndItsNode(statusRootFolder, "DataVariables");

            // create status elements
            // PMAC information
            _nodePmacIsOnline =
                _mgr.KlsCreateStatusVariableAndItsNode(folderPmac,  "OnlineStatus", BuiltInType.String);

            _nodePmacIsRunning =
                _mgr.KlsCreateStatusVariableAndItsNode(folderPmac,  "RunningStatus", BuiltInType.String);

            _nodePmacMaxDongleItems =
                _mgr.KlsCreateStatusVariableAndItsNode(folderPmac,  "License_MaxItems", BuiltInType.Int32);

            _nodePmacProjectName =
                _mgr.KlsCreateStatusVariableAndItsNode(folderPmac,  "ProjectName", BuiltInType.String);

            _nodePmacVariablesCount =
                _mgr.KlsCreateStatusVariableAndItsNode(folderPmac,  "AvailableVariables", BuiltInType.Int32);

            // variable nodes information
            _nodeLastTreeUpdateTime =
                _mgr.KlsCreateStatusVariableAndItsNode(folderVars,  "LastTreeUpdateTime", BuiltInType.DateTime);
            _nodeLastTreeUpdateAdded =
                _mgr.KlsCreateStatusVariableAndItsNode(folderVars,  "LastTreeUpdateVarsAdded", BuiltInType.Int32);
            _nodeLastTreeUpdateDeleted =
                _mgr.KlsCreateStatusVariableAndItsNode(folderVars,  "LastTreeUpdateVarsDeleted", BuiltInType.Int32);
            _nodeLastTreeUpdateReplaced =
                _mgr.KlsCreateStatusVariableAndItsNode(folderVars,  "LastTreeUpdateVarsReplaced", BuiltInType.Int32);
            _nodeLastTreeUpdateInvalidated =
                _mgr.KlsCreateStatusVariableAndItsNode(folderVars,  "LastTreeUpdateVarsInvalidated", BuiltInType.Int32);

            _nodeVarsCountTotal =
                _mgr.KlsCreateStatusVariableAndItsNode(folderVars,  "VariablesCountTotal", BuiltInType.Int32);
            _nodeVarsCountGlobal =
                _mgr.KlsCreateStatusVariableAndItsNode(folderVars,  "VariablesCountGlobal", BuiltInType.Int32);
            _nodeVarsCountTask =
                _mgr.KlsCreateStatusVariableAndItsNode(folderVars,  "VariablesCountTask", BuiltInType.Int32);
        }

        #endregion


        #region VarCounters
        public int VarsTotalCount { get { return VarsGlobal.Count + VarsTask.Count; } }
        public readonly VarCounter VarsGlobal = new VarCounter();
        public readonly VarCounter VarsTask = new VarCounter();

        public VarCounter TreeUpdateAdded = new VarCounter();
        public VarCounter TreeUpdateDeleted = new VarCounter();
        public VarCounter TreeUpdateReplaced = new VarCounter();
        public VarCounter TreeUpdateInvalidated = new VarCounter();


        /// <summary>
        /// Reset all counters concerning tree update
        /// </summary>
        public void TreeUpdateCountersReset()
        {
            TreeUpdateAdded.Reset();
            TreeUpdateDeleted.Reset();
            TreeUpdateReplaced.Reset();
            TreeUpdateInvalidated.Reset();
        }

        private void AlterVarCounter(SubTreeId id, int delta)
        {
            // ReSharper disable once SwitchStatementMissingSomeCases - we are not interested in anything but Globals and Tasks
            switch (id)
            {
                case SubTreeId.Globals:
                    VarsGlobal.AlterBy(delta);
                    break;
                case SubTreeId.Tasks:
                    VarsTask.AlterBy(delta);
                    break;
                //default:
                //    // we do not have statistics for status-node count or unknown nodes
                //    break;
            }
        }
        public void IncrementVarCounter(SubTreeId id)
        { AlterVarCounter(id, 1); }
        public void DecrementVarCounter(SubTreeId id)
        { AlterVarCounter(id, -1); }

        #endregion


        /// <summary>
        /// Update all underlying status nodes, except error nodes
        /// (error nodes are served on occurence).
        /// </summary>
        /// <param name="pmacStatus"></param>
        public void UpdateNodes(PmacStatusForUaServer pmacStatus)
        {
            // fill all nodes concerning Pmac
            // Pmac data should come to us from above level
            UpdatePmacNodes(pmacStatus);

            // error data
            // we do not fill error data here
            // separate functions serve errors on corresponding occurrence
            { }

            // Ovarall amount of variables in our address space
            _mgr.KlsSetValueScalar(_nodeVarsCountTotal, VarsTotalCount);
            _mgr.KlsSetValueScalar(_nodeVarsCountGlobal, VarsGlobal.Count);
            _mgr.KlsSetValueScalar(_nodeVarsCountTask, VarsTask.Count);

            // news about last tree update
            _mgr.KlsSetValueScalar(_nodeLastTreeUpdateAdded, TreeUpdateAdded.Count);
            _mgr.KlsSetValueScalar(_nodeLastTreeUpdateDeleted, TreeUpdateDeleted.Count);
            _mgr.KlsSetValueScalar(_nodeLastTreeUpdateReplaced, TreeUpdateReplaced.Count);
            _mgr.KlsSetValueScalar(_nodeLastTreeUpdateInvalidated, TreeUpdateInvalidated.Count);
            _mgr.KlsSetValueScalar(_nodeLastTreeUpdateTime, DateTime.Now);
        }

        private void UpdatePmacNodes(PmacStatusForUaServer pmacStatus)
        {
            if (pmacStatus.IsOnline)
            {
                _mgr.KlsSetValueScalar(_nodePmacIsOnline, "Online");
                _mgr.KlsSetValueScalar(_nodePmacIsRunning, pmacStatus.IsRunning ? "Running" : "Stopped");
                _mgr.KlsSetValueScalar(_nodePmacMaxDongleItems, pmacStatus.MaxDongleItems);
                _mgr.KlsSetValueScalar(_nodePmacProjectName, pmacStatus.ProjectName);
                _mgr.KlsSetValueScalar(_nodePmacVariablesCount, pmacStatus.AvailableVariablesCount);
            }
            else
            {
                _mgr.KlsSetValueScalar(_nodePmacIsOnline, "Offline");

                // all the other info has no real meaning
                // do not touch the value but set bad status
                _nodePmacIsRunning.StatusCode = StatusCodes.BadNoDataAvailable;
                _nodePmacIsRunning.ClearChangeMasks(_mgr.SystemContext, false);

                _nodePmacMaxDongleItems.StatusCode = StatusCodes.BadNoDataAvailable;
                _nodePmacMaxDongleItems.ClearChangeMasks(_mgr.SystemContext, false);

                _nodePmacProjectName.StatusCode = StatusCodes.BadNoDataAvailable;
                _nodePmacProjectName.ClearChangeMasks(_mgr.SystemContext, false);

                _nodePmacVariablesCount.StatusCode = StatusCodes.BadNoDataAvailable;
                _nodePmacVariablesCount.ClearChangeMasks(_mgr.SystemContext, false);
            }
        }

        public void AddNewError(string message)
        {
            AddNewMessage("Error", message);
        }

        public void AddNewMessage(string messageType, string message)
        {
            // todo create not 3 but arbitary amount of messages
            
            // create new message node
            string errName = string.Format("Msg_{0}_{1}", ++_msgCounter, messageType);
            BaseDataVariableState msg =
                _mgr.KlsCreateStatusVariableAndItsNode(_folderMessages, errName, BuiltInType.String);

            // fill value with message
            string fullMessage = string.Format("{0}. {1}", DateTime.Now, message);
            msg.Value = fullMessage;
            msg.StatusCode = StatusCodes.Good;
            msg.ClearChangeMasks(_mgr.SystemContext, false);

            // remember the error (to be able to delete it later)
            _nodesMessage.Add(msg);

            // delete old error
            if (_nodesMessage.Count > maxMessagesCount)
            {
                _mgr.KlsDeleteNodeAndSubtreeAndRemoveFromParent(_nodesMessage[0], false);
                _nodesMessage.RemoveAt(0);
            }

            Debug.WriteLine(fullMessage);
        }
    }
}
