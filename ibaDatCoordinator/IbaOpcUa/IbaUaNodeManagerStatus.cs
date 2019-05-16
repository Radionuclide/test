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
            FolderState folderPmac = _mgr.CreateFolderAndItsNode(statusRootFolder, "PMAC", null);
            _folderMessages = _mgr.CreateFolderAndItsNode(statusRootFolder, "Messages", null);
            FolderState folderVars = _mgr.CreateFolderAndItsNode(statusRootFolder, "DataVariables", null);

            // create status elements
            // PMAC information
            _nodePmacIsOnline =
                _mgr.CreateVariableAndItsNode(folderPmac, BuiltInType.String, "OnlineStatus");

            _nodePmacIsRunning =
                _mgr.CreateVariableAndItsNode(folderPmac, BuiltInType.String, "RunningStatus");

            _nodePmacMaxDongleItems =
                _mgr.CreateVariableAndItsNode(folderPmac, BuiltInType.Int32, "License_MaxItems");

            _nodePmacProjectName =
                _mgr.CreateVariableAndItsNode(folderPmac, BuiltInType.String, "ProjectName");

            _nodePmacVariablesCount =
                _mgr.CreateVariableAndItsNode(folderPmac, BuiltInType.Int32, "AvailableVariables");

            // variable nodes information
            _nodeLastTreeUpdateTime =
                _mgr.CreateVariableAndItsNode(folderVars, BuiltInType.DateTime, "LastTreeUpdateTime");
            _nodeLastTreeUpdateAdded =
                _mgr.CreateVariableAndItsNode(folderVars, BuiltInType.Int32, "LastTreeUpdateVarsAdded");
            _nodeLastTreeUpdateDeleted =
                _mgr.CreateVariableAndItsNode(folderVars, BuiltInType.Int32, "LastTreeUpdateVarsDeleted");
            _nodeLastTreeUpdateReplaced =
                _mgr.CreateVariableAndItsNode(folderVars, BuiltInType.Int32, "LastTreeUpdateVarsReplaced");
            _nodeLastTreeUpdateInvalidated =
                _mgr.CreateVariableAndItsNode(folderVars, BuiltInType.Int32, "LastTreeUpdateVarsInvalidated");

            _nodeVarsCountTotal =
                _mgr.CreateVariableAndItsNode(folderVars, BuiltInType.Int32, "VariablesCountTotal");
            _nodeVarsCountGlobal =
                _mgr.CreateVariableAndItsNode(folderVars, BuiltInType.Int32, "VariablesCountGlobal");
            _nodeVarsCountTask =
                _mgr.CreateVariableAndItsNode(folderVars, BuiltInType.Int32, "VariablesCountTask");
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

        private void AlterVarCounter(int delta)
        {
                    VarsGlobal.AlterBy(delta);
        }

        public void IncrementVarCounter() => AlterVarCounter( 1);

        public void DecrementVarCounter() => AlterVarCounter( -1);

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
            _mgr.SetValueScalar(_nodeVarsCountTotal, VarsTotalCount);
            _mgr.SetValueScalar(_nodeVarsCountGlobal, VarsGlobal.Count);
            _mgr.SetValueScalar(_nodeVarsCountTask, VarsTask.Count);

            // news about last tree update
            _mgr.SetValueScalar(_nodeLastTreeUpdateAdded, TreeUpdateAdded.Count);
            _mgr.SetValueScalar(_nodeLastTreeUpdateDeleted, TreeUpdateDeleted.Count);
            _mgr.SetValueScalar(_nodeLastTreeUpdateReplaced, TreeUpdateReplaced.Count);
            _mgr.SetValueScalar(_nodeLastTreeUpdateInvalidated, TreeUpdateInvalidated.Count);
            _mgr.SetValueScalar(_nodeLastTreeUpdateTime, DateTime.Now);
        }

        private void UpdatePmacNodes(PmacStatusForUaServer pmacStatus)
        {
            if (pmacStatus.IsOnline)
            {
                _mgr.SetValueScalar(_nodePmacIsOnline, "Online");
                _mgr.SetValueScalar(_nodePmacIsRunning, pmacStatus.IsRunning ? "Running" : "Stopped");
                _mgr.SetValueScalar(_nodePmacMaxDongleItems, pmacStatus.MaxDongleItems);
                _mgr.SetValueScalar(_nodePmacProjectName, pmacStatus.ProjectName);
                _mgr.SetValueScalar(_nodePmacVariablesCount, pmacStatus.AvailableVariablesCount);
            }
            else
            {
                _mgr.SetValueScalar(_nodePmacIsOnline, "Offline");

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
                _mgr.CreateVariableAndItsNode(_folderMessages, BuiltInType.String, errName);

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
                _mgr.DeleteNodeRecursively(_nodesMessage[0], false);
                _nodesMessage.RemoveAt(0);
            }

            Debug.WriteLine(fullMessage);
        }
    }
}
