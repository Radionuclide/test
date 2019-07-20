using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using DevExpress.Utils;
using iba.Data;
using iba.Utility;
using Opc.Ua;
using Opc.Ua.Server;
using ibaOpcServer.IbaOpcUa;


namespace iba.ibaOPCServer
{
    /// <summary>
    /// A node manager for a server that exposes several variables.
    /// </summary>
    public class IbaUaNodeManager : CustomNodeManager2
    {

        // todo. kolesnik. probably to replace with better variant
        public const string IbaDefaultNamespace = "http://iba-ag.com";
        
        #region Constructors

        /// <summary>
        /// By OPC Foundation.
        /// Initializes the node manager.
        /// </summary>
        public IbaUaNodeManager(IbaOpcUaServer ibaUaServer, IServerInternal server, ApplicationConfiguration configuration)
            : base(server, configuration, IbaDefaultNamespace)
        {
            _ibaUaServer = ibaUaServer;
            SystemContext.NodeIdFactory = this;
        }

        #endregion

        #region IDisposable Members
        ///// <summary>
        ///// An overrideable version of the Dispose.
        ///// </summary>
        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        // TBD
        //    }
        //}
        #endregion

        #region INodeIdFactory Members
        
        /// <summary>
        /// By OPC Foundation.
        /// Creates the NodeId for the specified node.
        /// is referenced implicitly from precompiled code
        /// do not remove!!
        /// </summary>
        public override NodeId New(ISystemContext context, NodeState node)
        {
            BaseInstanceState instance = node as BaseInstanceState;

            if (instance != null && instance.Parent != null)
            {
                string id = instance.Parent.NodeId.Identifier as string;

                if (id != null)
                {
                    // Replaced underline (_) with our own NODE_ID_DELIMITER
                    return new NodeId(ComposeNodeId(id,  instance.SymbolicName), instance.Parent.NodeId.NamespaceIndex);
                    
                    // todo. kls. check uniqueness here??
                }
            }

            return node.NodeId;
        }
        #endregion

        #region INodeManager Members
        /// <summary>
        /// By OPC Foundation.
        /// Changed considerably by Kolesnik.
        /// Does any initialization required before the address space can be used.
        /// </summary>
        /// <remarks>
        /// The externalReferences is an out parameter that allows the node manager to link to nodes
        /// in other node managers. For example, the 'Objects' node is managed by the CoreNodeManager and
        /// should have a reference to the root folder node(s) exposed by this node manager.  
        /// </remarks>
        public override void CreateAddressSpace(IDictionary<NodeId, IList<IReference>> externalReferences)
        {
            lock (Lock)
            {
                IList<IReference> references;

                if (!externalReferences.TryGetValue(ObjectIds.ObjectsFolder, out references))
                {
                    externalReferences[ObjectIds.ObjectsFolder] = references = new List<IReference>();
                }

                FolderIbaRoot = CreateFolder(null, "IbaDatCoordinator" /*UpperCamel*/, "ibaDatCoordinator");
                FolderIbaRoot.Description = @"ibaDatCoordinator module";

                FolderIbaRoot.AddReference(ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
                references.Add(new NodeStateReference(ReferenceTypes.Organizes, false, FolderIbaRoot.NodeId));
                FolderIbaRoot.EventNotifier = EventNotifiers.SubscribeToEvents;
                AddRootNotifier(FolderIbaRoot);

                AddPredefinedNode(SystemContext, FolderIbaRoot);
            }
        }

        /// <summary>
        /// By OPC Foundation.
        /// Creates a new folder.
        /// </summary>
        private FolderState CreateFolder(NodeState parent, string path, string name)
        {
            FolderState folder = new FolderState(parent)
            {
                SymbolicName = name,
                ReferenceTypeId = ReferenceTypes.Organizes,
                TypeDefinitionId = ObjectTypeIds.FolderType,
                NodeId = new NodeId(path, NamespaceIndex),
                BrowseName = new QualifiedName(path, NamespaceIndex),
                DisplayName = new LocalizedText("en", name),
                WriteMask = AttributeWriteMask.None,
                UserWriteMask = AttributeWriteMask.None,
                EventNotifier = EventNotifiers.None
            };


            if (parent != null)
            {
                parent.AddChild(folder);
            }

            return folder;
        }


/*
        private ServiceResult OnWriteDataItem(
            ISystemContext context,
            NodeState node,
            NumericRange indexRange,
            QualifiedName dataEncoding,
            ref object value,
            ref StatusCode statusCode,
            ref DateTime timestamp)
        {
            DataItemState variable = node as DataItemState;

            // verify data type.
            Opc.Ua.TypeInfo typeInfo = Opc.Ua.TypeInfo.IsInstanceOfDataType(
                value,
                variable.DataType,
                variable.ValueRank,
                context.NamespaceUris,
                context.TypeTable);

            if (typeInfo == null || typeInfo == Opc.Ua.TypeInfo.Unknown)
            {
                return StatusCodes.BadTypeMismatch;
            }

            if (typeInfo.BuiltInType != BuiltInType.DateTime)
            {
                double number = Convert.ToDouble(value);
                number = Math.Round(number, (int)variable.ValuePrecision.Value);
                value = Opc.Ua.TypeInfo.Cast(number, typeInfo.BuiltInType);
            }

            return ServiceResult.Good;
        }
*/
       
/*
        private ServiceResult OnWriteAnalog(
            ISystemContext context,
            NodeState node,
            NumericRange indexRange,
            QualifiedName dataEncoding,
            ref object value,
            ref StatusCode statusCode,
            ref DateTime timestamp)
        {
            AnalogItemState variable = node as AnalogItemState;

            // verify data type.
            Opc.Ua.TypeInfo typeInfo = Opc.Ua.TypeInfo.IsInstanceOfDataType(
                value,
                variable.DataType,
                variable.ValueRank,
                context.NamespaceUris,
                context.TypeTable);

            if (typeInfo == null || typeInfo == Opc.Ua.TypeInfo.Unknown)
            {
                return StatusCodes.BadTypeMismatch;
            }

            // check index range.
            if (variable.ValueRank >= 0)
            {
                if (indexRange != NumericRange.Empty)
                {
                    object target = variable.Value;
                    ServiceResult result = indexRange.UpdateRange(ref target, value);

                    if (ServiceResult.IsBad(result))
                    {
                        return result;
                    }

                    value = target;
                }
            }

            // check instrument range.
            else
            {
                if (indexRange != NumericRange.Empty)
                {
                    return StatusCodes.BadIndexRangeInvalid;
                }

                double number = Convert.ToDouble(value);

                if (variable.InstrumentRange != null && (number < variable.InstrumentRange.Value.Low || number > variable.InstrumentRange.Value.High))
                {
                    return StatusCodes.BadOutOfRange;
                }
            }

            return ServiceResult.Good;
        }
*/

        /// <summary>
        /// By OPC Foundation.
        /// Returns a unique handle for the node.
        /// </summary>
        protected override NodeHandle GetManagerHandle(ServerSystemContext context, NodeId nodeId, IDictionary<NodeId, NodeState> cache)
        {
            lock (Lock)
            {
                // quickly exclude nodes that are not in the namespace. 
                if (!IsNodeIdInNamespace(nodeId))
                {
                    return null;
                }

                NodeState node;
                
                if (!PredefinedNodes.TryGetValue(nodeId, out node))
                {
                    return null;
                }

                NodeHandle handle = new NodeHandle();

                handle.NodeId = nodeId;
                handle.Node = node;
                handle.Validated = true;

                return handle;
            }
        }

        /// <summary>
        /// By OPC Foundation.
        /// Verifies that the specified node exists.
        /// </summary>
        protected override NodeState ValidateNode(
           ServerSystemContext context, NodeHandle handle, IDictionary<NodeId,NodeState> cache)
        {
            // not valid if no root.
            if (handle == null) return null;
            // check if previously validated.
            if (handle.Validated) return handle.Node;
            // TBD
            return null;
        }

        #endregion

        #region kolesnik  test

        public static readonly char NODE_ID_DELIMITER = '\\';
        public static readonly char NODE_ID_DEFAULT_REPLACEMENT_CHARACTER = '_';

        /// <summary> Composes string node id, using <see cref="NODE_ID_DELIMITER"/>. 
        /// E.g.: "abc\de" + "fgh" -> "abc\de\fgh" </summary>
        public static string ComposeNodeId(string parentFullId, string nodeBrowseName) 
            => parentFullId + NODE_ID_DELIMITER + GetAdaptedBrowseName(nodeBrowseName);

        private readonly IbaOpcUaServer _ibaUaServer;

        /// <summary> Root folder for all iba-specific data </summary>
        public FolderState FolderIbaRoot { get; private set; }

        //private const string FolderIbaRootName = "ibaDatCoordinator";


        private IbaOpcUaVariable _temporaryWatch; // todo. kls. delete

        // todo delete tmp variables
        public static int TmpKls___ReadCounter;

        /// <summary>
        /// By Kolesnik. 
        /// Update value for each variable being watched.
        /// </summary>
        /// <param name="watchList"></param>
        public void KlsUpdateWatchVariableValues(List<object> watchList)
        {

            try
            {
                // probabaly ua-monitored items list can differ from watchlist
                // (this is not good situation (some inconsistency take place) but still can happen)
                // if we have some monitored item that is not presented in a watchlist
                // then we should mark it as bad

                // initially suppose that all monitored variables are not available in PMAC
                // in main cycle below those which are really available will be marked as true
                foreach (KeyValuePair<NodeId, MonitoredNode2> kvp in MonitoredNodes)
                {
                    IbaOpcUaVariable monitoredIbaOpcUaVar = kvp.Value.Node as IbaOpcUaVariable;
                    if (monitoredIbaOpcUaVar == null) continue;
                    //monitoredIbaOpcUaVar.IsAvailableInPmacWatchlist = false;
                }

                // set values and mark with IsAvailableInPmacWatchlist flag 
                //foreach (var wle in watchList.Where(wle => wle != null))
                //    KlsSetValue(wle.WatchElement);

                // those monitored items which are not available in PMAC should be invalidated
                foreach (KeyValuePair<NodeId, MonitoredNode2> kvp in MonitoredNodes)
                {
                    IbaOpcUaVariable monitoredIbaOpcUaVar = kvp.Value.Node as IbaOpcUaVariable;
                    if (monitoredIbaOpcUaVar == null) continue;
                    //if (monitoredIbaOpcUaVar.IsAvailableInPmacWatchlist) continue;
                    
                    // here is a monitored but unavailable variable
                    monitoredIbaOpcUaVar.StatusCode = StatusCodes.BadNoDataAvailable;
                    // call state-change handlers
                    monitoredIbaOpcUaVar.ClearChangeMasks(SystemContext, false);
                }
            }
            catch (Exception e)
            {
                // most probable variants of Exception - that collection is changed during execution of foreach loop
                // because of multithreadede access to watchlist and monitored items
                // here i do not protect it by Lock, I prefer just to abort this update cycle and try again it the next one
                Debug.WriteLine("Exception in KlsUpdateWatchVariableValues foreach cycle:" + e.Message);
            }
        }

        /// <summary>
        /// By Kolesnik. 
        /// Update varTree structure based on supplied VarInfo data.
        /// </summary>
        /// <param name="varInfo"></param>
        /// <param name="pmacStatus"></param>
        //public void KlsUpdateVarTree(VariableInformation varInfo, PmacStatusForUaServer pmacStatus)
        //{
        //    lock (Lock)
        //    {
        //        // reset update tree counters
        //        Status.TreeUpdateCountersReset();

        //        // initially mark for deleting all our variables
        //        // then later inside main cycle we will remove these marks for those variables which are in use
        //        List<IbaVariable> allIbaVariables = _folderIbaRoot.GetFlatListOfIbaVariableChildren(SystemContext);
        //        foreach (IbaVariable iv in allIbaVariables)
        //            iv.IsMarkedForDeleting = true;

        //        // recreate Tasks folder if it does not exit; 
        //        // probabpy it will be needed
        //        // if not, the it will be deleted at the end of this function
        //        if (_folderIbaTask == null)
        //            _folderIbaTask = KlsCreateFolderAndItsNode(_folderIbaRoot, FolderIbaTasksName);

        //        // according to varlist create new nodes 
        //        if (varInfo != null)
        //        {
        //            // virtually limit varlist size according to license
        //            int limitedVarlistCount = varInfo.VarList.Count;
        //            if (limitedVarlistCount > pmacStatus.MaxDongleItems)
        //            {
        //                limitedVarlistCount = pmacStatus.MaxDongleItems;
        //                Status.AddNewMessage("Warning",
        //                    string.Format("Warning: License limit reached (Max = {0}). Some of available variables ({1}) will not be added.", 
        //                    pmacStatus.MaxDongleItems, varInfo.VarList.Count));
        //            }

        //            for (int i = 0; i < limitedVarlistCount; i++)
        //                KlsCreateOrUpdateVarTreeElement(varInfo.VarList.Values[i]);
        //        }

        //        // delete or invalidate all nodes that are marked for deleting
        //        KlsDeleteUnneededNodes();
                
        //        // paranoic check
        //        // compare actual amount of nodes in subtress to our internal counters
        //        int varsTaskCount = _folderIbaTask.GetFlatListOfIbaVariableChildren(SystemContext).Count;
        //        int varsGlobalCount = _folderIbaGlobals.GetFlatListOfIbaVariableChildren(SystemContext).Count;
        //        if (Status.VarsGlobal.Count != varsGlobalCount ||
        //            Status.VarsTask.Count != varsTaskCount ||
        //            Status.VarsTotalCount != (varsGlobalCount + varsTaskCount))
        //        {
        //            // should never happen.
        //            // only ibaUaNodeManager should create and delete its nodes, and
        //            // ibaUaNodeManager always tracks its changes
        //            Status.AddNewError(string.Format(
        //                "Nodes count inconsistency. G: {0}=={1}, T: {2}=={3}, Total: {4}=={5}",
        //                Status.VarsGlobal.Count, varsGlobalCount,
        //                Status.VarsTask.Count, varsTaskCount,
        //                Status.VarsTotalCount, varsGlobalCount + varsTaskCount));
        //        }
                
        //        // delete Tasks folder if it is empty
        //        if (Status.VarsTask.Count == 0)
        //        {
        //            KlsDeleteNodeAndSubtreeAndRemoveFromParent(_folderIbaTask, false); 
        //            _folderIbaTask = null;
        //        }


        //        // todo
        //        if (Status.VarsTotalCount > 0)
        //        {
        //            //KlsDeleteNodeAndSubtreeAndRemoveFromParent(_folderIbaGlobals, false);
        //            //KlsDeleteNodeAndSubtreeAndRemoveFromParent(_folderIbaTask, false);
        //        }
        //        // update server status
        //        Status.UpdateNodes(pmacStatus);

        //        // add new message
        //        Status.AddNewMessage(
        //            "VarTreeUpdate",
        //            string.Format("Changes: (+{0}, -{1}, Repl.{2}, Inv.{3}); TotalVars(UA/PMAC): ({4}/{5}).",
        //            Status.TreeUpdateAdded, Status.TreeUpdateDeleted, Status.TreeUpdateReplaced, Status.TreeUpdateInvalidated, 
        //            Status.VarsTotalCount, pmacStatus.AvailableVariablesCount));
        //    }

        //}

        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <returns></returns>
        public string KlsGetDescriptionStringVarTree()
        {
            // todo do it faster:  do not recalculate string unless tree was changed since last time
            // by now in release show only number
#if DEBUG

            List<IbaOpcUaVariable> vars = new List<IbaOpcUaVariable>();
            //if (_folderIbaGlobals != null)
            //    vars.AddRange(_folderIbaGlobals.GetFlatListOfIbaVariableChildren(SystemContext));
            
            //if (_folderIbaTask!=null)
            //    vars.AddRange(_folderIbaTask.GetFlatListOfIbaVariableChildren(SystemContext));

            if (vars.Count == 0)
                return "<none>";

            string s = string.Format("Count = {0}: ", vars.Count);

            // limit this string to not more than certain amount of variables
            int max = Math.Min(20, vars.Count);

            for (int i = 0; i < max; i++)
                s += vars[i].BrowseName.Name + ", ";

            // remove trailing delimiter
            s = s.TrimEnd(',', ' ');

            return s;
#else
            return string.Format("Count = {0}.", Status.VarsTotalCount);
#endif
        }

        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <returns></returns>
        public string KlsGetDescriptionStringMonitoredItems()
        {
            try
            {

            // todo do it faster: like in KlsGetDescriptionStringVarTree()
            string s = "";

            if (_temporaryWatch != null) s = string.Format("TmpWatch: {0}; ", _temporaryWatch.SymbolicName);
            if (MonitoredNodes == null || MonitoredNodes.Count == 0)
                return s;

#if DEBUG
            s += string.Format("Monitored = {0}: ", MonitoredNodes.Count);
            
            const int max = 20;
            int current = 0;
            foreach (var item in MonitoredItems)
            {

                s += string.Format("[t={0} mm={1} si={2} irtp={3} irtt={4}],   ",
                    item.Value.MonitoredItemType,
                    item.Value.MonitoringMode,
                    item.Value.SamplingInterval,
                    item.Value.IsReadyToPublish,
                    item.Value.IsReadyToTrigger

                    );
            }
            foreach (KeyValuePair<NodeId, MonitoredNode2> item in MonitoredNodes)
            {
                IbaOpcUaVariable monitoredVar = item.Value.Node as IbaOpcUaVariable;
                if (monitoredVar != null)
                    s += string.Format("{0}, ", monitoredVar.BrowseName.Name);
                
                // stop processing if list is too big
                current++;
                if (current>max) break;
            }
            // remove last ", "
            s = s.TrimEnd(' ', ',');
            return s;
#else
            return string.Format("Count = {0}.", MonitoredNodes.Count);
#endif
            }
            catch
            {
                return "errror";
            }
        }
        public string KlsGetDescriptionStringMonitoredItems2()
        {
            string s = "<none>";

            try
            {
                List<IbaOpcUaVariable> allIbaVariableChildren = FolderIbaRoot.GetFlatListOfIbaVariableChildren(SystemContext);

                foreach (IbaOpcUaVariable iv in allIbaVariableChildren)
                {
                    if (!iv.IsMonitored) continue;
                    if (s == ("<n" + "one>")) s = "";
                    s += iv.BrowseName.Name + ", ";
                } 
            }
            catch
            {
                // suppress collection-changed-exception
                // can happen under multi-threaded access
            }
            // remove last ", "
            s = s.TrimEnd(' ', ',');
            return s;
        }

        public void KlsCheckMonitoredItemsConsistency(List<IbaOpcUaVariable> allIbaVariables)
        {
            lock (Lock)
            {
                int count1 = 0;
                int count2 = 0;

                foreach (IbaOpcUaVariable iv in allIbaVariables)
                    if (iv.IsMonitored)
                        count1++;

                foreach (KeyValuePair<NodeId, MonitoredNode2> kvp in MonitoredNodes)
                {
                    IbaOpcUaVariable monitoredIbaOpcUaVar = kvp.Value.Node as IbaOpcUaVariable;
                    if (monitoredIbaOpcUaVar == null) continue;
                    if (monitoredIbaOpcUaVar.IsDeleted) continue;
                    count2++;
                    //AddNewError(string.Format("Monitored list inconcistency. Item {0}.", monitoredIbaOpcUaVar));
                    Debug.Assert(monitoredIbaOpcUaVar.IsMonitored);
                }

                // can happen sometimes... 
                // like creating of monitored item list without calling onMonitoredItemCreated handler
                Debug.Assert(count1 == count2);
            }
        }
        /// <summary>
        /// By Kolesnik. 
        /// For all variables marked for deleting do either: deleting or setting bad QC.
        /// </summary>
        public void KlsDeleteUnneededNodes()
        {
            // get all variables
            List<IbaOpcUaVariable> allIbaVariables = FolderIbaRoot.GetFlatListOfIbaVariableChildren(SystemContext);

            // for better reliability we could refresh IsMonitored flag
            // on the case if some inconsistencies appeared
            // now let's test it without flag refresh
            ////// suppose nothing is monitored
            ////foreach (IbaVariable iv in allIbaVariables)
            ////    iv.IsMonitored = false;

            ////// mark what is really monitored
            ////foreach (KeyValuePair<NodeId, MonitoredNode2> kvp in MonitoredNodes)
            ////{
            ////    IbaVariable monitoredIbaVar = kvp.Value.Node as IbaVariable;
            ////    if (monitoredIbaVar == null) continue;
            ////    monitoredIbaVar.IsMonitored = true;
            ////}

            KlsCheckMonitoredItemsConsistency(allIbaVariables);

            //// delete or mark as bad
            //foreach (IbaOpcUaVariable v in allIbaVariables.Where(v => v.IsMarkedForDeleting))
            //{
            //    // what is marked for deleting should be either deleted or marked as bad
            //    if (v.IsMonitored)
            //    {
            //        // we cannot delete invalidate inexisting in PMAC as bad
            //        v.StatusCode = StatusCodes.BadNoDataAvailable;
            //        // call state-change handlers
            //        v.ClearChangeMasks(SystemContext, false);
            //        // add this change to statistics
            //        //Status.TreeUpdateInvalidated.Increment();
            //    }
            //    else
            //    {
            //        // not monitored, so can be deleted
            //        DeleteNodeRecursively(v, true);
            //        // add this change to statistics
            //        //Status.TreeUpdateDeleted.Increment();
            //    }
            //}
        }

        //private bool KlsCheckReferenceConsistency(IbaVariable ibaVar, VariableInformation.tVariableElement ve)
        //{
        //    if (ibaVar == null) return false;
        //    if (ve == null) return false;

        //    if (!ReferenceEquals(ibaVar.VariableElement, ve)) return false;
        //    if (!ReferenceEquals(ibaVar, ve.UaVariable)) return false;

        //    return true;
        //}


        ///// <summary>
        ///// By Kolesnik. 
        ///// First try to get ve quickly using direct reference.
        ///// If not, then try to find it by name.
        ///// </summary>
        ///// <param name="ve"></param>
        ///// <returns>Returns corresponding node on success, or null if it was not found</returns>
        //private BaseInstanceState KlsGetNodeForVariableElement(VariableInformation.tVariableElement ve)
        //{
        //    if (ve.sName == null) return null;
        //    if (ve.sName == "") return null;

        //    IbaVariable ibaVar = ve.UaVariable as IbaVariable;

        //    // check if we already have a direct reference to corresponding node
        //    if (ibaVar != null)
        //    {
        //        // check names
        //        if (string.Compare(ve.VariableName, ibaVar.VeName, StringComparison.Ordinal) == 0)
        //        {
        //            // okay, this is really correct node for given ve
        //            // check back reference consistency
        //            if (!KlsCheckReferenceConsistency(ibaVar, ve))
        //            {
        //                // backward reference differs, probabaly ve was renewed
        //                // fix it
        //                ibaVar.VariableElement = ve;
        //                Status.AddNewMessage(
        //                    "Warning",
        //                    string.Format("Warning, back reference is wrong {0}, {1}, {2}", ibaVar.NodeId.Identifier, ve.VariableName, ibaVar.VeName));
        //            }
        //            TmpKls___SearchCountFast ++;
        //            return ibaVar;
        //        }

        //        // this should not happen, because names should never be changed in ve or in ibaVarialbe
        //        // (rename of variable in PMAC results to creating of absolutely new var)
        //        Status.AddNewError(string.Format("Names inconsistency {0}, {1}, {2}", ibaVar.NodeId.Identifier, ve.VariableName, ibaVar.VeName));
        //        // anyway, invalidate both references, not to be confused once again
        //        ibaVar.VariableElement = null;
        //        ve.UaVariable = null;
        //    }

        //    // reference is not defined or is incorrect
        //    // perform a search of needed node by name
        //    string symbolicPath = KlsConvertVeNameToUaSymbolicPath(ve.VariableName, false);
        //    BaseInstanceState instState = _folderIbaRoot.FindChildBySymbolicName(SystemContext, symbolicPath);
            
        //    ibaVar = instState as IbaVariable;
        //    // if node found is ibaVariable then set references for future quick use
        //    if (ibaVar!=null)
        //    {
        //        ve.UaVariable = ibaVar;
        //        ibaVar.VariableElement = ve;
        //    }

        //    // return a node found (it is == null  if seach has failed)
        //    TmpKls___SearchCountSlow++;
        //    return instState;
        //}


        ///// <summary>
        ///// By Kolesnik. 
        ///// Set a value to UA-node that corresponds to v.
        ///// </summary>
        ///// <param name="ve"></param>
        //private void KlsSetValue(VariableInformation.tVariableElement ve)
        //{
        //    BaseInstanceState instState = KlsGetNodeForVariableElement(ve);
        //    IbaVariable iv = instState as IbaVariable;

        //    // if such node not yet exist or is of not correct type
        //    if (iv == null)
        //    {
        //        // ua variable does not exist for the watch list element
        //        // this should not happen
        //        Status.AddNewError(string.Format("Error updating values. Ua IbaVariable node does not exist for the watch list element {0}.", ve.sName));
        //        return;
        //    }
            
        //    // corresponding IbaVariable exists

        //    // if it is proceessed inside this function
        //    // then we know that now this variable is currently in PMAC's watchlist
        //    iv.IsAvailableInPmacWatchlist = true;
            
        //    // set valuse
        //    KlsSetValue(iv, ve);
        //}


        ///// <summary>
        ///// By Kolesnik. 
        ///// Create variable corresponding to tVariableElement v.
        ///// If variable already exists, then function checks and fixes its type if necessary
        ///// </summary>
        ///// <param name="ve"></param>
        ///// <returns></returns>
        //private void KlsCreateOrUpdateVarTreeElement(VariableInformation.tVariableElement ve)
        //{
        //    if (ve == null) return;

        //    // send feedback to onlineServer that we processed this variable
        //    ve.bUpdated = false;

        //    // reset ua type and decoders (probably types are changed)
        //    ve.ResetUaTypeAndDecoders();
            
        //    BaseInstanceState instState = KlsGetNodeForVariableElement(ve);

        //    bool wasCreated = false;

        //    // check if corresponding node already exists but it is not an iba variable
        //    if (instState != null && !(instState is IbaVariable))
        //    {
        //        // delete old garbage to eliminate name conflict
        //        KlsDeleteNodeAndSubtreeAndRemoveFromParent(instState, false);
        //        // add this change to statistics
        //        Status.TreeUpdateDeleted.Increment();
        //        instState = null;
        //    }

        //    // now if something exists here then it is an ibaVariable
        //    IbaVariable ibaVar = instState as IbaVariable;

        //    // if no node existed or it existed but was deleted (because had wrong object type)
        //    if (ibaVar == null)
        //    {
        //        // create a new one
        //        ibaVar = KlsCreatePathAndVaribale(ve);
        //        // add this change to statistics
        //        Status.TreeUpdateAdded.Increment();

        //        wasCreated = true;
        //    }

        //    // paranoic check
        //    if (ibaVar == null)
        //    {
        //        Status.AddNewError(string.Format("Cannot create variable {0}", ve.VariableName));
        //        return;
        //    }

        //    // now we have a variable in correct place and with correct name
        //    // and it is not null

        //    // reference from ibaVar to ve should already be established
        //    // (it was set long before or it was set just above during creation of ibaVar)
        //    // now set reference from ve to ibaVar
        //    ve.UaVariable = ibaVar;

        //    // paranoic check
        //    if (!KlsCheckReferenceConsistency(ibaVar, ve))
        //    {
        //        Status.AddNewError(string.Format("Reference inconsistency {0}.{1} != {2}", ibaVar,ibaVar.VariableElement, ve));
        //        ibaVar.StatusCode = StatusCodes.BadInternalError;
        //        ibaVar.ClearChangeMasks(SystemContext, false);
        //        return;
        //    }
            
        //    // this ibavar has corresponding variable in PMAC, so we should not delete it
        //    ibaVar.IsMarkedForDeleting = false;

        //    // setup or refresh access level
        //    ibaVar.SetAccessLevel(ve.bCanRead, ve.bCanWrite);

        //    if (!wasCreated)
        //    {
        //        // iba variable already existed here
        //        // check if it is of the same type
        //        if (KlsCompareDataType(ibaVar, ve))
        //            // this variable stays the same
        //            // nothing else should be done with it
        //            return;
        //        // variable of different type
        //        // we should count that it is changed
        //        Status.TreeUpdateReplaced.Increment();
        //        // and go on with type assignment
        //    }


        //    // setup type
        //    BuiltInType uaType = ve.GetUaType();

        //    if (ve.Type == VarEnum.VT_ARRAY)
        //    {
        //        // this is an array, get its info 
        //        VariableInformation.tArrayInformation arrInfo = (VariableInformation.tArrayInformation) ve.value.extInformation;
        //        uint arraySize = (uint) (arrInfo.size/arrInfo.typesize);
        //        ibaVar.SetupAsArray(uaType, arraySize);
        //    }
        //    else
        //        ibaVar.SetupAsScalar(uaType);
        //}


        //private static bool KlsCompareDataType(IbaVariable iv, VariableInformation.tVariableElement ve)
        //{
        //    if (ve == null) return false;
        //    if (iv == null) return false;

        //    // non built-in types are not supported
        //    if (iv.DataType.IdType != IdType.Numeric)
        //        return false;

        //    // check base type
        //    if (ve.GetUaType() != (BuiltInType)(uint)iv.DataType.Identifier)
        //        return false;

        //    // check if both are scalars
        //    if (ve.Type != VarEnum.VT_ARRAY && iv.ValueRank == ValueRanks.Scalar)
        //        return true;

        //    // check if both are arrays
        //    if (ve.Type == VarEnum.VT_ARRAY && iv.ValueRank == ValueRanks.OneDimension)
        //    {
        //        // this is an array get its info 
        //        VariableInformation.tArrayInformation arrInfo = (VariableInformation.tArrayInformation)ve.value.extInformation;
                
        //        // check size
        //        if (arrInfo.typesize == 0) return false;
        //        if ((arrInfo.size / arrInfo.typesize) != (iv.ArrayDimensions[0])) return false;

        //        return true;
        //    }

        //    // one is scalar, another is array;
        //    // or one of variables is of unknown type
        //    return false;
        //}

        ///// <summary>
        ///// By Kolesnik. 
        ///// </summary>
        ///// <param name="startFolder"></param>
        ///// <param name="varFullNameAsList"></param>
        ///// <returns></returns>
        //public FolderState KlsCreatePath(FolderState startFolder, List<string> varFullNameAsList)
        //{
        //    if (startFolder == null) return null;

        //    if (varFullNameAsList == null || varFullNameAsList.Count == 0) return null;

        //    string topmost = varFullNameAsList[0];

        //    // check whether topmost folder exists among our immediate children
        //    BaseInstanceState subElemet = startFolder.FindChild(SystemContext, new QualifiedName(topmost, NamespaceIndex));
        //    FolderState subfolder = null;

        //    if (subElemet != null)
        //    {
        //        if (subElemet is FolderState)
        //        {
        //            // subfolder already exists, ok
        //            subfolder = subElemet as FolderState;
        //        }
        //        else
        //        {
        //            // element with this name already exists but it is not a folder
        //            // this is rare case (when scalar is replaced by the struct with the same name)
        //            // we should replace var with the folder
        //            // (otherwise we will have name conflict)
        //            // so, delete subelement without check of usage, because we have to
        //            DeleteNodeRecursively(subElemet, false);
        //            // subfolder with this name will be created in next statement
        //        }
        //    }

        //    if (subfolder == null)
        //    {
        //        // topmost folder does not exist
        //        // create it
        //        subfolder = CreateFolderAndItsNode(startFolder, topmost, null);
        //    }

        //    // get subpath relative to the subfolder
        //    varFullNameAsList.RemoveAt(0);
        //    if (varFullNameAsList.Count == 0)
        //        // nothing else left to find/create, path is ok
        //        // return lowermost subfolder
        //        return subfolder;
        //    else
        //    // create the rest of the path recursively
        //    // ReSharper disable once TailRecursiveCall - for better readability (recursion depth should not be big)
        //        return KlsCreatePath(subfolder, varFullNameAsList);
        //}

        /// <summary>
        /// 1. Recursively destroys the node a its children.
        /// 2. Removes node from its parent.
        /// 3. Destroys parent (and all superparents) if it is empty, unless the parent is a global root
        /// </summary>
        /// <param name="node">instance to be deleted</param>
        /// <param name="enableDeletingOfEmptyParents"></param>
        public void DeleteNodeRecursively(BaseInstanceState node, bool enableDeletingOfEmptyParents)
        {
            // remember node's parent to destroy it later if necessary
            NodeState parent = node.Parent;

            // call delete-handlers for the node (and recursively for the whole subtree)
            node.Delete(SystemContext);

            // remove the node and (all children) from manger's dictionary and remove node from parent's list
            DeleteNode(SystemContext, node.NodeId);
            
            // mark node as deleted
            if (node is IbaOpcUaVariable iv)
            {
                iv.StatusCode = StatusCodes.BadObjectDeleted;
                iv.ClearChangeMasks(SystemContext, false);
            }

            // go on with deleting of our parent (and superparents) if it is empty
            if (!enableDeletingOfEmptyParents) return;

            // if parent is null or is not a folder, then don't touch it
            if (!(parent is FolderState)) return;
            
            // if parent is one of roots, then don't touch it
            if (parent == FolderIbaRoot) 
                return;

            // if it is not-root folder and is empty now, then also delete it
            var children = GetChildren(parent as FolderState);
            if (children.Count == 0)
            {
                // parent has no children, we do not need an empty folder
                // ReSharper disable once TailRecursiveCall - for better readability (recursion depth should not be big)
                DeleteNodeRecursively(parent as FolderState, true);
            }
        }

        public void DeleteAllUserValues()
        {
            // delete all children of iba root folder
            foreach (var node in GetChildren(FolderIbaRoot))
            {
                DeleteNodeRecursively(node, false);
            }
        }

        public void DeleteEmptySubfolders(FolderState parentFolder = null, bool bPreserveItself = false)
        {
            // if parent folder is not specified, then use root folder
            parentFolder = parentFolder ?? FolderIbaRoot;

            // first try to delete all children on lower level
            foreach (var node in GetChildren(parentFolder))
            {
                if (node is FolderState folder)
                    DeleteEmptySubfolders(folder);
            }

            // if parentFolder doesn't have children anymore, then delete it
            if (parentFolder != FolderIbaRoot && bPreserveItself == false && GetChildren(parentFolder).Count == 0)
            {
                DeleteNodeRecursively(parentFolder, false);
            }
        }




        ///// <summary>
        ///// By Kolesnik. 
        ///// </summary>
        ///// <param name="startFolder"></param>
        ///// <param name="varFullNameAsList"></param>
        ///// <param name="subTreeId"></param>
        ///// <param name="ve"></param>
        ///// <returns></returns>
        //public IbaVariable KlsCreatePathAndVaribale(FolderState startFolder, List<string> varFullNameAsList, SubTreeId subTreeId, VariableInformation.tVariableElement ve)
        //{
        //    if (startFolder == null)
        //        // unable to create var inside unknown folder
        //        return null;

        //    if (varFullNameAsList.Count < 1)
        //        // unable to create var without a name
        //        return null;

        //    // last element of list is the name of the data variable
        //    string dataVarName = varFullNameAsList[varFullNameAsList.Count - 1];
        //    // all the other elements represent its path
        //    varFullNameAsList.RemoveAt(varFullNameAsList.Count - 1);

        //    // create path if necessary
        //    FolderState lowermostSubfolder = KlsCreatePath(startFolder, varFullNameAsList);
        //    if (lowermostSubfolder == null)
        //        // creation of path was unneeded
        //        // our start folder is already lowermost
        //        lowermostSubfolder = startFolder;

        //    // check if variable exists inside created path
        //    BaseInstanceState existingElement = lowermostSubfolder.FindChild(SystemContext, new QualifiedName(dataVarName, NamespaceIndex));
        //    if (existingElement != null)
        //    {
        //        if (existingElement is IbaVariable)
        //        {
        //            // already exist
        //            // should never happen, becuase this check is made on upper level
        //            // nevertheless, return what we've found
        //            // todo compare type existing to new, change flags
        //            return existingElement as IbaVariable;
        //        }
        //        else
        //        {
        //            // element exists but it is not a data variable
        //            // we should delete it without usage tests, otherwise we will have name conflicts
        //            FolderState subTreeRoot = KlsGetSubtreeRootFolder(subTreeId);
        //            KlsDeleteNodeAndSubtreeAndRemoveFromParent(existingElement, false);
        //            // go to the next statement to create a new variable with such a name
        //        }
        //    }

        //    // create variable inside created path
        //    // we create it as a scalar, but later we will set it up as array or complex if needed
        //    IbaVariable newVar = KlsCreateIbaVariableAndItsNode(lowermostSubfolder, dataVarName, subTreeId, ve);

        //    return newVar;
        //}


        ///// <summary>
        ///// By Kolesnik. 
        ///// Creates all the needed intremediate folders to in order to get the needed path. Then creates a variable there.
        ///// This function is only applicable to "Globals" and "Tasks" subtrees.
        ///// </summary>
        ///// <returns></returns>
        //public IbaVariable KlsCreatePathAndVaribale(VariableInformation.tVariableElement ve)
        //{
        //    string veFullName = ve.VariableName;

        //    // veFullName cannot be null or empty
        //    if (string.IsNullOrEmpty(veFullName)) return null;

        //    SubTreeId subTreeId = KlsGetSubtreeForGivenVeName(veFullName);

        //    // we can dynamically create only ibaVariables in one of two subtrees
        //    if (!(subTreeId == SubTreeId.Globals || subTreeId == SubTreeId.Tasks))
        //    {
        //        Status.AddNewError(string.Format("Cannot identify subtree for variable '{0}'.", veFullName));
        //        return null;
        //    }

        //    FolderState subTreeRoot = KlsGetSubtreeRootFolder(subTreeId);

        //    string uaName = KlsConvertVeNameToUaName(veFullName, false);
        //    if (uaName == "")
        //    {
        //        Status.AddNewError(string.Format("Cannot convert Pmac variable name '{0}' to uaName.", veFullName));
        //        return null;
        //    }
        //    List<string> varFullNameAsList = KlsSplitPathToList(uaName);

        //    // remove subtree root folder names (e.g. "ibaGlobals." or "ibaTask") from the beginning,
        //    // because we will start with subtree root
        //    varFullNameAsList.RemoveAt(0);

        //    return KlsCreatePathAndVaribale(subTreeRoot, varFullNameAsList, subTreeId, ve);
        //}

        /// <summary>
        /// By Kolesnik. 
        /// returns List of names; first element in list is highest element name. For empty path returns empty list. returned list is always != null.
        /// </summary>
        /// <param name="path">e.g. "Globals.Struct3.Elm1</param>
        /// <returns></returns>
        public static List<string> KlsSplitPathToList(string path)
        {
            char[] separator = {'.', '/'};

            List<string> list = new List<string>();

            if (path == "") return list; // return lis

            list.AddRange(path.Split(separator));

            // paranoic check
            foreach (string s in list)
                if (s == "")
                    throw new Exception("internal path split error. found empty name in path");
            
            return list;
        }

        //private void KlsSetValue(IbaVariable ibaVar, VariableInformation.tVariableElement varElement)
        //{
        //    // if ibaVar was deleted
        //    // usually should not happen, because we do not delete a node if it is monitored
        //    // but it can happen that we force deleting the node even if it is monitored
        //    // (for example, to avoid name conflict of struct and variable)
        //    // 
        //    if (ibaVar.IsDeleted)
        //    {
        //        ibaVar.StatusCode = StatusCodes.BadObjectDeleted;
        //        ibaVar.ClearChangeMasks(SystemContext, false);
        //        return;
        //    }

        //    // check if data types of iv and ve correspond to each other
        //    if (!KlsCompareDataType(ibaVar, varElement))
        //    {
        //        // type mismatch.
        //        // this should not happen.
        //        // types should be set correctly in Update configuration process.
        //        // but nevertheless
        //        // do not set value, not to confuse the user
        //        // set corresponding status to tell about the problem
        //        ibaVar.StatusCode = StatusCodes.BadTypeMismatch;
        //        ibaVar.ClearChangeMasks(SystemContext, false);
        //        return;
        //    }

        //    ibaOnlineServer.tLzsVariable value = varElement.value;
        //    // check if source value exists
        //    if (value == null)
        //    {
        //        ibaVar.StatusCode = StatusCodes.BadNoData;
        //        ibaVar.ClearChangeMasks(SystemContext, false);
        //        return;
        //    }

        //    if (varElement.Type == VarEnum.VT_ARRAY)
        //        KlsSetValueArray(ibaVar, varElement);
        //    else
        //        KlsSetValueScalar(ibaVar, value.data);
        //}

        // todo. kls. move to elsewhere
        private static readonly Dictionary<Type, BuiltInType> _typeDict = new Dictionary<Type, BuiltInType>
        {
            {typeof(string), BuiltInType.String},
            {typeof(bool), BuiltInType.Boolean},
            {typeof(DateTime), BuiltInType.DateTime},
            {typeof(int), BuiltInType.Int32},
            {typeof(uint), BuiltInType.UInt32},
            {typeof(float), BuiltInType.Float},
            {typeof(double), BuiltInType.Double}
        };

        public static BuiltInType GetOpcUaType(object value)
        {
            return _typeDict.TryGetValue(value.GetType(), out BuiltInType uaType) ? uaType : BuiltInType.Null;
        }


        /// <summary> Sets value, sets StatusCodes.Good, calls state-change handlers </summary>
        public void SetValueScalar(BaseDataVariableState varState, object value)
        {
            // todo. kls. perform type-check if necessary
#if DEBUG 
            FormatEnum(ref value);
            var uaType = GetOpcUaType(value);
            Debug.Assert((NodeId)(uint)uaType == varState.DataType);
#endif
            // set value
            varState.Value = value;

            // on success set status code
            varState.StatusCode = StatusCodes.Good;

            // call state-change handlers
            varState.ClearChangeMasks(SystemContext, false);
        }
        
        /// <summary> Sets value, sets StatusCodes.Good, calls state-change handlers </summary>
        public void SetValueScalar(string nodeId, object value)
        {
            if (!(Find(nodeId) is BaseDataVariableState variable))
                return;
            SetValueScalar(variable, value);
        }


        ///// <summary>
        ///// By Kolesnik. 
        ///// </summary>
        ///// <param name="ibaVar"></param>
        ///// <param name="val"></param>
        //private void KlsSetValueArray(IbaVariable ibaVar, VariableInformation.tVariableElement val)
        //{
        //    // todo probably need to make setting values more soft - do not cause state change if actual value not differs
        //    // todo __ first compare value old to new, then assign if necessary and set change mask

        //    // default value for the case if something goes wrong
        //    ibaVar.Value = null;
        //    ibaVar.StatusCode = StatusCodes.BadDataEncodingInvalid;

        //    VariableInformation.tArrayInformation arrInfo = (VariableInformation.tArrayInformation) val.value.extInformation;

        //    if (arrInfo.Dimensions != 1)
        //    {
        //        // dimensions != 1 not supported
        //        return; // not successful
        //    }

        //    if (val.DecoderScalar == null || val.DecoderArray == null)
        //    {
        //        // if decoders are not defined, define them
        //        val.DecoderArray = KlsGetByteDecoderArray_Slow(arrInfo.BaseType.ToUaType(), out val.DecoderScalar);

        //        if (val.DecoderScalar == null || val.DecoderArray == null)
        //            return; // not successful
        //    }


        //    int count = arrInfo.size/arrInfo.typesize;

        //    ibaVar.Value = val.DecoderArray((byte[])val.value.data, 0, count, arrInfo.typesize, val.DecoderScalar);
        //    ibaVar.StatusCode = StatusCodes.Good;

        //    // call state-change handlers
        //    ibaVar.ClearChangeMasks(SystemContext, false);
        //}


        /// <summary> Checks if a node with a given node ID not yet exists in a current namespace</summary>
        /// <param name="fullNodeId">For example, "Root.MyFolder.Var1"</param>
        private bool IsNodeIdUnique(string fullNodeId) => Find(fullNodeId) == null;

        /// <summary>
        /// Generates unique valid node name as close as possible to a given argument.
        /// </summary>
        private string GenerateUniqueNodeName(FolderState parent, string nodeName)
        {
            if (!IsValidBrowseName(nodeName))
                nodeName = GetAdaptedBrowseName(nodeName);

            Debug.Assert(!string.IsNullOrEmpty(nodeName));
            
            if (IsNodeIdUnique(parent, nodeName))
                return nodeName; // ok, original name is unique

            for (int i = 1; i < 1000; i++)
            {
                string nameCandidate = $"{nodeName}_{i}";
                if (IsNodeIdUnique(parent, nameCandidate))
                    return nameCandidate; // ok, suggested candidate is unique
            }

            // almost impossible 
            throw new Exception($"Cannot generate a unique name based on {parent?.NodeId}.{nodeName}") ; 
        }

        /// <summary> Finds the node by its string Node ID in a current namespace </summary>
        /// <param name="fullNodeId">For example, "Root.MyFolder.Var1"</param>
        public NodeState Find(string fullNodeId) => Find(new NodeId(fullNodeId, NamespaceIndex));

        public bool IsNodeIdUnique(FolderState parent, string nodeName) =>
            // parent.FindChild() // todo. kls. use this instead of global search? (performance?)
            IsNodeIdUnique(GetFullNodeId(parent, nodeName));

        /// <summary> It's not allowed that a browse name is null, is empty or contains dots ('.').
        /// Anything else is ok, including whitespaces, slashes, semicolons, etc. </summary>
        public static bool IsValidBrowseName(string browseName)
        {
            return !string.IsNullOrEmpty(browseName) && !browseName.Contains(NODE_ID_DELIMITER);
        }

        /// <summary>
        /// // todo. kls. comment 
        /// </summary>
        /// <param name="browseName"></param>
        /// <returns></returns>
        public static string GetAdaptedBrowseName(string browseName)
        {
            return string.IsNullOrWhiteSpace(browseName) ?
                "Node" /* a default name if no name was supplied at all */: 
                browseName.Replace(NODE_ID_DELIMITER, NODE_ID_DEFAULT_REPLACEMENT_CHARACTER);
        }

        /// <summary> Returns string NodeId for given parent and node name.
        /// Normally NodeId is generated by SDK's CreateNode() function.
        /// To check uniqueness it's necessary to know the name beforehand (before node creation).
        /// Only string NodeIds are supported.
        /// For example, ("Root\Variables", "X") -> "Root\Variables\X"
        /// </summary>
        /// <param name="parent">Parent folder for the node to be created in</param>
        /// <param name="nodeName">Browse name of the node that is going to be created</param>
        /// <returns></returns>
        public static string GetFullNodeId(NodeState parent, string nodeName)
        {
            if (parent == null)
                throw new ArgumentException("Trying to get an id for a parent==null", nameof(parent));

            if (!(parent.NodeId.Identifier is string parentStrId /*we use string IDs only*/))
                throw new ArgumentException("Trying to get a string id for non-string parent id", nameof(parent));

            if (!IsValidBrowseName(nodeName))
                throw new ArgumentException($"'{nodeName}' is not a valid browse name", nameof(nodeName));

            return $"{parentStrId}{NODE_ID_DELIMITER}{nodeName}";
        }

        public static string GetParentName(string nodeId)
        {
            var pos = nodeId.LastIndexOf(NODE_ID_DELIMITER);
            if (pos == -1)
                return null; // no parent
            string parentId = nodeId.Substring(0, pos);
            Debug.Assert(!string.IsNullOrWhiteSpace(parentId));
            return parentId;

        }

        /// <summary>Checks if it is ok to create a node with such a name in a given folder.
        /// Throws ArgumentException otherwise</summary>
        private void AssertNameCorrectnessAndUniqueness(NodeState parent, string name)
        {
            if (!IsValidBrowseName(name))
                throw new ArgumentException($"'{name}' is not a valid browse name",nameof(name));

            string expectedId = GetFullNodeId(parent, name);

            if (!IsNodeIdUnique(expectedId))
                throw new ArgumentException($"NodeId '{expectedId}' already exists (is not unique)", nameof(name));
        }

        /// <summary> Creates a folder in the default address space.  </summary>
        /// <param name="parent">Parent folder to create a node in</param>
        /// <param name="browseName"></param>
        /// <param name="displayName"> // todo. kls. BrowseName, that will be also used for creation of NodeId and a default Display name;
        /// All characters (including whitespace) except dot are allowed. For example, 'My variable'</param>
        /// <param name="description">OPC UA node description - any string or null</param>
        /// <returns>Returns a created Folder</returns>
        public FolderState CreateFolderAndItsNode(FolderState parent, string browseName, string displayName, string description) 
        {
            AssertNameCorrectnessAndUniqueness(parent, browseName);

            var folder = new FolderState(parent);

            // todo. kls. 
            //string browseName = GenerateUniqueNodeName(parent, displayName);

            // todo. kls. check uniqueness
            if (browseName != displayName)
                ;

            var qualifiedName = new QualifiedName(browseName, NamespaceIndex);
            
            CreateNode(SystemContext, parent.NodeId, ReferenceTypeIds.Organizes, qualifiedName, folder);

            folder.TypeDefinitionId = ObjectTypeIds.FolderType;
            folder.ReferenceTypeId = ReferenceTypes.Organizes;

            // set display name and description;
            // no check for null, empty, whitespace, because anything is allowed here
            folder.DisplayName = displayName; // by default DisplayName is set to BrowseName automatically, but let's set our value
            folder.Description = description; 

            return folder;
        }

        /// <summary> Converts enum value to formatted string.
        /// // todo. kls. proper enum handling? </summary>
        public static string FormatEnum(object enumValue)
        {
            Debug.Assert(enumValue.GetType().IsEnum);
            return $@"{(int)enumValue} ({enumValue})";
        }        
        
        /// <summary> Formats value if it is enum. Other values stay unchanged.
        /// // todo. kls. proper enum handling? </summary>
        public static void FormatEnum(ref object enumValue)
        {
            if (enumValue.GetType().IsEnum)
            {
                enumValue = FormatEnum(enumValue);
            }
        }

        public IbaOpcUaVariable CreateIbaVariable(FolderState parent, ExtMonData.ExtMonVariableBase xmv)
        {
            object initialValue = xmv.ObjValue;

            FormatEnum(ref initialValue);

            // get uaType automatically from initial value
            var uaType = IbaUaNodeManager.GetOpcUaType(initialValue);
            Debug.Assert(uaType != BuiltInType.Null);

            AssertNameCorrectnessAndUniqueness(parent, xmv.UaBrowseName);

            //string browseName = GenerateUniqueNodeName(parent, xmv.Caption); // todo. kls. remove or move to overridden function?

            IbaOpcUaVariable v = new IbaOpcUaVariable(parent, xmv);

            // create node for given instance
            CreateNodeForDataVariable(v, xmv.UaBrowseName);

            // ensure created NodeId looks as expected
            Debug.Assert(v.NodeId.Identifier as string == xmv.UaFullPath);

            v.Description = xmv.Description;
            v.DisplayName = xmv.Caption;

            // set access type to readonly
            v.SetAccessLevel(true, false);
            v.SetupAsScalar(uaType);

            SetValueScalar(v, initialValue);
            return v;
        }
        
        private void CreateNodeForDataVariable(BaseDataVariableState v, string browseName)
        {
            // compose browse name
            QualifiedName qualifiedName = new QualifiedName(browseName, NamespaceIndex);

            // create node for given instance
            CreateNode(SystemContext, v.Parent.NodeId, ReferenceTypeIds.Organizes, qualifiedName, v);

            // set up attributes to defaults

            v.ReferenceTypeId = ReferenceTypes.Organizes;
            v.TypeDefinitionId = VariableTypeIds.BaseDataVariableType;
            v.Timestamp = DateTime.UtcNow;
            v.Historizing = false;

            v.WriteMask = AttributeWriteMask.None;
            v.UserWriteMask = v.WriteMask;

            v.AccessLevel = AccessLevels.None;
            v.UserAccessLevel = v.AccessLevel;

            v.DataType = null;
            v.ValueRank = ValueRanks.Scalar;
            v.Value = null;

            // do not set to good until we have some data
            v.StatusCode = StatusCodes.BadNoData;
        }

        public List<BaseInstanceState> GetChildren(FolderState folder)
        {
            if (folder == null)
                return null;
            var list = new List<BaseInstanceState>();
            folder.GetChildren(SystemContext, list);
            return list;
        }

        public List<BaseInstanceState> GetFlatListOfAllChildren(FolderState folder)
        {
            if (folder == null)
                return null;
            var allChildren = new List<BaseInstanceState> {folder};

            var immediateChildren = new List<BaseInstanceState>();
            folder.GetChildren(SystemContext, immediateChildren);

            foreach (BaseInstanceState baseInstanceState in immediateChildren)
            {
                switch (baseInstanceState)
                {
                    case FolderState childFolder:
                        var subChildren = GetFlatListOfAllChildren(childFolder);
                        if (subChildren!=null)
                            allChildren.AddRange(subChildren);
                        break;
                    case BaseDataVariableState childVar:
                        allChildren.Add(childVar);
                        break;
                }
            }
            return allChildren;
        }

        public static string GetNodeIdAsString(NodeId id)
        {
            return id.IdType == IdType.String ? (string) id.Identifier : null;
        }

        public List<BaseInstanceState> GetListOfAllUserNodes()
        {
            var instances = GetFlatListOfAllChildren(this.FolderIbaRoot);

            // todo. kls. 
            // convert to string id???
            //var list = new Set<BaseInstanceState>();
            //foreach (var instance in instances)
            //{
            //    var nodeId = instance.NodeId;
            //    Debug.Assert(nodeId.IdType == IdType.String);
            //    string strNodeId = GetNodeIdAsString(nodeId);
            //    Debug.Assert(strNodeId != null);
            //    list.Add(strNodeId);
            //}
            //return list;
            return instances;
        }
        /// <summary>
        /// By Kolesnik. 
        /// Remove variable from PMAC's watch list if this variable is not monitored (e.g. by someone else)
        /// </summary>
        private void KlsRemoveWatchIfUnused(IbaOpcUaVariable ibaOpcUaVariableToRemove)
        {
            if (ibaOpcUaVariableToRemove == null) return;
            //lock (Lock)
            {
                if (MonitoredNodes == null) return;
                foreach (var item in MonitoredNodes)
                {
                    if (ibaOpcUaVariableToRemove.NodeId == item.Key)
                        // our var is still monitored by someone else,
                        // so, do NOT remove it from watch list
                        return;
                }
            }

            // it is not used, remove it
            ibaOpcUaVariableToRemove.IsMonitored = false;
            //_ibaUaServer.KlsRemoveWatch(ibaOpcUaVariableToRemove.VeName);
        }

        #endregion

        
        #region override by kolesnik

        protected override void OnMonitoredItemCreated(ServerSystemContext context, NodeHandle handle, MonitoredItem monitoredItem)
        {
            base.OnMonitoredItemCreated(context, handle, monitoredItem);

            var x = monitoredItem.MonitoringMode;

            // we do not do additional actions if user monitors non-iba node
            if (handle == null || !(handle.Node is IbaOpcUaVariable)) return;

            // user monitors iba-node
            IbaOpcUaVariable iv = (IbaOpcUaVariable) (handle.Node);

            // if node was deleted, do not try to watch it
            if (iv.IsDeleted) return;

            // remember that it is monitored
            iv.IsMonitored = true;

            //// get name in VariableElement notation
            //if (string.IsNullOrEmpty(iv.VeName))
            //{
            //    // should not happen...
            //    // do not add this to PMAC watchlist
            //    Status.AddNewError(string.Format("Cannot get ve-Name for variable '{0}'", iv));
            //    return;
            //}

            //// add it to PMAC's watchlist
            //_ibaUaServer.KlsAddWatch(iv.VeName);
        }

        protected override void OnMonitoredItemDeleted(ServerSystemContext context, NodeHandle handle, MonitoredItem monitoredItem)
        {
            base.OnMonitoredItemDeleted(context, handle, monitoredItem);

            // we are interested in non-null iba data variables
            // ignore anything else
            if (handle == null || !(handle.Node is IbaOpcUaVariable))
                return;

            // several monitored items can point to one and the same node
            // so before removing the watch, we should be sure that there are no other monitored itmes using given node
            // MonitoredNodes list is rebuilt automatically by OPC Kit
            // we need just to look for the handle inside MonitoredNodes list
            // if this handle is still inside Monitored nodes, then we should avoid removing it from PMAC's wathlist
            KlsRemoveWatchIfUnused(handle.Node as IbaOpcUaVariable);
        }

        public override void Read(OperationContext context, double maxAge, IList<ReadValueId> nodesToRead, IList<DataValue> values, IList<ServiceResult> errors)
        {
            // handle a request if user reads a value of ibaNode
            foreach (ReadValueId valueId in nodesToRead)
            {
                // we handle only read of value
                if (valueId.AttributeId != Attributes.Value) continue;

                NodeState foundNode = Find(valueId.NodeId);
                IbaOpcUaVariable v = foundNode as IbaOpcUaVariable;

                if (v == null) continue;


                // todo replace with instant single read
                // by now add a temporarty watch to be able to see the variable

                 //remove previous temporary watch
                 //todo fix deadlock in online server, then enable next call
                 //this call sometimes cause deadlock in online server
                 KlsRemoveWatchIfUnused(_temporaryWatch);

                // remember current temporary watch
                _temporaryWatch = v;

                TmpKls___ReadCounter++;
                //_ibaUaServer.KlsAddWatch(_temporaryWatch.VeName);
            }
            base.Read(context, maxAge, nodesToRead, values, errors);

        }

        #endregion

        #region Byte decoding by Kolesnik


        //private static VariableInformation.ByteDecoderForScalar GetByteDecoderScalar_Slow(BuiltInType t)
        //{
        //    // using switch/case is a very slow implementation!
        //    // todo later speedup using sorted list or using adding field in varifo
        //    switch (t)
        //    {
        //        case BuiltInType.Boolean:
        //            return ByteDecoderBoolean;
        //        case BuiltInType.SByte:
        //            return ByteDecoderSByte;
        //        case BuiltInType.Int16:
        //            return ByteDecoderInt16;
        //        case BuiltInType.Int32:
        //            return ByteDecoderInt32;
        //        case BuiltInType.Int64:
        //            return ByteDecoderInt64;

        //        case BuiltInType.Byte:
        //            return ByteDecoderByte;
        //        case BuiltInType.UInt16:
        //            return ByteDecoderUInt16;
        //        case BuiltInType.UInt32:
        //            return ByteDecoderUInt32;
        //        case BuiltInType.UInt64:
        //            return ByteDecoderUInt64;

        //        case BuiltInType.Float:
        //            return ByteDecoderSingle;
        //        case BuiltInType.Double:
        //            return ByteDecoderDouble;

        //        case BuiltInType.String:
        //            // todo
        //            // array of strings needs special handle
        //            // disable by now
        //            //return ByteDecoderString;
        //            return null;

        //        default:
        //            return null;
        //    }
        //}

        ///// <summary>
        ///// By Kolesnik. 
        ///// </summary>
        ///// <param name="t"></param>
        ///// <param name="decoderScalar"></param>
        ///// <returns></returns>
        //private static VariableInformation.ByteDecoderForArray KlsGetByteDecoderArray_Slow(
        //    BuiltInType t, out VariableInformation.ByteDecoderForScalar decoderScalar)
        //{
        //    // using switch/case is a very slow implementation!
        //    // todo later speedup using sorted list or using adding field in varifo
        //    decoderScalar = GetByteDecoderScalar_Slow(t);
        //    if (decoderScalar == null) return null;

        //    switch (t)
        //    {
        //        case BuiltInType.Boolean:
        //            return ByteDecoderArrayGeneric<Boolean>;

        //        case BuiltInType.SByte:
        //            return ByteDecoderArrayGeneric<SByte>;
        //        case BuiltInType.Int16:
        //            return ByteDecoderArrayGeneric<Int16>;
        //        case BuiltInType.Int32:
        //            return ByteDecoderArrayGeneric<Int32>;
        //        case BuiltInType.Int64:
        //            return ByteDecoderArrayGeneric<Int64>;

        //        case BuiltInType.Byte:
        //            return ByteDecoderArrayGeneric<Byte>;
        //        case BuiltInType.UInt16:
        //            return ByteDecoderArrayGeneric<UInt16>;
        //        case BuiltInType.UInt32:
        //            return ByteDecoderArrayGeneric<UInt32>;
        //        case BuiltInType.UInt64:
        //            return ByteDecoderArrayGeneric<UInt64>;

        //        case BuiltInType.Float:
        //            return ByteDecoderArrayGeneric<Single>;
        //        case BuiltInType.Double:
        //            return ByteDecoderArrayGeneric<Double>;

        //        case BuiltInType.String:
        //            // todo
        //            // array of strings needs special handle
        //            // disable by now
        //            //return ByteDecoderArrayGeneric<String>;
        //            return null;

        //        default:
        //            return null;
        //    }
        //}

        private static object ByteDecoderBoolean(byte[] value, int offset)
        {
            return BitConverter.ToBoolean(value, offset);
        }

        private static object ByteDecoderSByte(byte[] value, int offset)
        {
            return (sbyte) value[offset];
        }

        private static object ByteDecoderInt16(byte[] value, int offset)
        {
            return BitConverter.ToInt16(value, offset);
        }

        private static object ByteDecoderInt32(byte[] value, int offset)
        {
            return BitConverter.ToInt32(value, offset);
        }

        private static object ByteDecoderInt64(byte[] value, int offset)
        {
            return BitConverter.ToInt64(value, offset);
        }

        private static object ByteDecoderByte(byte[] value, int offset)
        {
            return value[offset];
        }

        private static object ByteDecoderUInt16(byte[] value, int offset)
        {
            return BitConverter.ToUInt16(value, offset);
        }

        private static object ByteDecoderUInt32(byte[] value, int offset)
        {
            return BitConverter.ToUInt32(value, offset);
        }

        private static object ByteDecoderUInt64(byte[] value, int offset)
        {
            return BitConverter.ToUInt64(value, offset);
        }

        private static object ByteDecoderSingle(byte[] value, int offset)
        {
            return BitConverter.ToSingle(value, offset);
        }

        private static object ByteDecoderDouble(byte[] value, int offset)
        {
            return BitConverter.ToDouble(value, offset);
        }


        //private static object ByteDecoderArrayGeneric<T>
        //    (byte[] data, int offset, int count, int typeSize, VariableInformation.ByteDecoderForScalar decoder) where T : new()
        //{
        //    T[] result;

        //    result = new T[count];

        //    for (int i = 0; i < count; i++)
        //        result[i] = (T) decoder(data, offset + i*typeSize);

        //    return result; // successful
        //}

        #endregion
    }
}
