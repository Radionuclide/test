using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using Opc.Ua;
using Opc.Ua.Server;
using ibaOpcServer.IbaOpcUa;


// ReSharper disable once CheckNamespace
namespace iba.ibaOPCServer
{
    public enum SubTreeId
    {
        Unknown,
        Globals,
        Tasks
    }


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
            Status = new IbaUaNodeManagerStatus(this);
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
                    // changed by kolesnik: replaced underline (_) with dot (.) delimiter
                    return new NodeId(id + "." + instance.SymbolicName, instance.Parent.NodeId.NamespaceIndex);
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

                _folderIbaRoot = CreateFolder(null, FolderIbaRootName, FolderIbaRootName);

                _folderIbaRoot.AddReference(ReferenceTypes.Organizes, true, ObjectIds.ObjectsFolder);
                references.Add(new NodeStateReference(ReferenceTypes.Organizes, false, _folderIbaRoot.NodeId));
                _folderIbaRoot.EventNotifier = EventNotifiers.SubscribeToEvents;
                AddRootNotifier(_folderIbaRoot);

                AddPredefinedNode(SystemContext, _folderIbaRoot);


                // create additional folders
                _folderIbaStatus = KlsCreateFolderAndItsNode(_folderIbaRoot, FolderIbaStatusName);
                _folderIbaGlobals = KlsCreateFolderAndItsNode(_folderIbaRoot, FolderIbaGlobalsName);
                // do not create Tasks now; it will be created if needed on update of vartree
                ////_folderIbaTask = KlsCreateFolderAndItsNode(_folderIbaRoot, _folderIbaTasksName);

                // create status nodes
                // this can be done only here, not in constructor
                Status.CreateStatusTree(_folderIbaStatus);

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

        private readonly IbaOpcUaServer _ibaUaServer;

        public readonly IbaUaNodeManagerStatus Status;
        
        /// <summary>
        /// Root folder for all iba-specific data
        /// </summary>
        private FolderState _folderIbaRoot;
        /// <summary>
        /// contains iba-ua-server-status nodes
        /// </summary>
        private FolderState _folderIbaStatus;
        /// <summary>
        /// Contains gloabl ibaLogic off-task connectors e.g. "Globals.OTC_Result"
        /// </summary>
        private FolderState _folderIbaGlobals; // contains 
        /// <summary>
        /// Contains non-global ibaLogic variables if ibaLogic is in the mode when everything is enabled for reading
        /// </summary>
        private FolderState _folderIbaTask;

        #region prefixes for "Tasks" and "Globals" varialbes

        private const string FolderIbaRootName = "Root";
        private const string FolderIbaStatusName = "Status";
        private const string FolderIbaGlobalsName = "Globals";
        private const string FolderIbaTasksName = "Tasks";
        public const string InternalPrefixForGlobalVariables = FolderIbaRootName + "." + FolderIbaGlobalsName + ".";
        public const string InternalPrefixForTaskVariables = FolderIbaRootName + "." + FolderIbaTasksName + ".";

        #endregion

        private IbaVariable _temporaryWatch;

        // todo delete tmp varialbes
        public static int TmpKls___ReadCounter;
        public static int TmpKls___SearchCountFast;
        public static int TmpKls___SearchCountSlow;

        /// <summary>
        /// By Kolesnik. 
        /// Update value for each variable being watched.
        /// </summary>
        /// <param name="watchList"></param>
        public void KlsUpdateWatchVariableValues(List<object> watchList)
        {

            try
            {
                // probabaly ua-monitored items list can differ from wathclist
                // (this is not good situation (some inconsistecy take place) but still can happen)
                // if we have some monitored item that is not presented in a watchlist
                // then we should mark it as bad

                // initially suppose that all monitored variables are not available in PMAC
                // in main cycle below those which are really available will be marked as true
                foreach (KeyValuePair<NodeId, MonitoredNode2> kvp in MonitoredNodes)
                {
                    IbaVariable monitoredIbaVar = kvp.Value.Node as IbaVariable;
                    if (monitoredIbaVar == null) continue;
                    monitoredIbaVar.IsAvailableInPmacWatchlist = false;
                }

                // set values and mark with IsAvailableInPmacWatchlist flag 
                //foreach (var wle in watchList.Where(wle => wle != null))
                //    KlsSetValue(wle.WatchElement);

                // those monitored items which are not available in PMAC should be invalidated
                foreach (KeyValuePair<NodeId, MonitoredNode2> kvp in MonitoredNodes)
                {
                    IbaVariable monitoredIbaVar = kvp.Value.Node as IbaVariable;
                    if (monitoredIbaVar == null) continue;
                    if (monitoredIbaVar.IsAvailableInPmacWatchlist) continue;
                    
                    // here is a monitored but unavailable variable
                    monitoredIbaVar.StatusCode = StatusCodes.BadNoDataAvailable;
                    // call state-change handlers
                    monitoredIbaVar.ClearChangeMasks(SystemContext, false);
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

        public void KlsTst___ForceDeleteTree()
        {
            lock (Lock)
            {
                KlsDeleteNodeAndSubtreeAndRemoveFromParent(_folderIbaGlobals, false);
                KlsDeleteNodeAndSubtreeAndRemoveFromParent(_folderIbaTask, false);
                
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
            // todo do it faster:  do not recalculate stirng unless tree was changed since last time
            // by now in release show only number
#if DEBUG

            List<IbaVariable> vars = new List<IbaVariable>();
            if (_folderIbaGlobals != null)
                vars.AddRange(_folderIbaGlobals.GetFlatListOfIbaVariableChildren(SystemContext));
            
            if (_folderIbaTask!=null)
                vars.AddRange(_folderIbaTask.GetFlatListOfIbaVariableChildren(SystemContext));

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
                IbaVariable monitoredVar = item.Value.Node as IbaVariable;
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
                List<IbaVariable> allIbaVariableChildren = _folderIbaRoot.GetFlatListOfIbaVariableChildren(SystemContext);

                foreach (IbaVariable iv in allIbaVariableChildren)
                {
                    if (!iv.IsMonitored) continue;
                    if (s == ("<n" + "one>")) s = "";
                    s += iv.BrowseName.Name + ", ";
                } 
            }
            catch
            {
                // suppress collection-changed-exception
                // can happen uder multithreaded access
            }
            // remove last ", "
            s = s.TrimEnd(' ', ',');
            return s;
        }

        public void KlsCheckMonitoredItemsConsistency(List<IbaVariable> allIbaVariables)
        {
            lock (Lock)
            {
                int count1 = 0;
                int count2 = 0;

                foreach (IbaVariable iv in allIbaVariables)
                    if (iv.IsMonitored)
                        count1++;

                foreach (KeyValuePair<NodeId, MonitoredNode2> kvp in MonitoredNodes)
                {
                    IbaVariable monitoredIbaVar = kvp.Value.Node as IbaVariable;
                    if (monitoredIbaVar == null) continue;
                    if (monitoredIbaVar.IsDeleted) continue;
                    count2++;
                    if (!monitoredIbaVar.IsMonitored)
                        Status.AddNewError(string.Format("Monitored list inconcistency. Item {0}.", monitoredIbaVar));
                }                

                if (count1 != count2)
                    // can happen sometimes... 
                    // like creating of monitored item list without calling onMonitoredItemCreated handler
                    Status.AddNewError(string.Format("Monitored list inconcistency. (Flags = {0}) != (Items = {1})", count1,count2));
            }
        }
        /// <summary>
        /// By Kolesnik. 
        /// For all variables marked for deleting do either: deleting or seting bad QC.
        /// </summary>
        public void KlsDeleteUnneededNodes()
        {
            // get all variables
            List<IbaVariable> allIbaVariables = _folderIbaRoot.GetFlatListOfIbaVariableChildren(SystemContext);

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

            // delete or mark as bad
            foreach (IbaVariable v in allIbaVariables.Where(v => v.IsMarkedForDeleting))
            {
                // what is marked for deleting should be either deleted or marked as bad
                if (v.IsMonitored)
                {
                    // we cannot delete invalidate inexisting in PMAC as bad
                    v.StatusCode = StatusCodes.BadNoDataAvailable;
                    // call state-change handlers
                    v.ClearChangeMasks(SystemContext, false);
                    // add this change to statistics
                    Status.TreeUpdateInvalidated.Increment();
                }
                else
                {
                    // not monitored, so can be deleted
                    KlsDeleteNodeAndSubtreeAndRemoveFromParent(v, true);
                    // add this change to statistics
                    Status.TreeUpdateDeleted.Increment();
                }
            }
        }
        
        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <param name="subTreeId"></param>
        /// <returns></returns>
        public FolderState KlsGetSubtreeRootFolder(SubTreeId subTreeId)
        {
            switch (subTreeId)
            {
                case SubTreeId.Globals:
                    return _folderIbaGlobals;
                case SubTreeId.Tasks:
                    return _folderIbaTask;
                case SubTreeId.Unknown:
                    return null;
            }
            return null;
        }

        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <param name="variableName"></param>
        /// <param name="includeIbaRootNodeName"></param>
        /// <returns></returns>
        private string KlsConvertVeNameToUaSymbolicPath(string variableName, bool includeIbaRootNodeName)
        {
            string path = KlsConvertVeNameToUaName(variableName, includeIbaRootNodeName);
            
            if (path == "") return "";

            // finally apply another delimiter for symbolic path
            return path.Replace('.', '/');
        }

        /// <summary>
        /// By Kolesnik. 
        /// For the given variable name in terms of VariableInformation.tVariableElement get ua internal full name.
        /// For example "Globals.Struct1.Var2" -> "ibaRoot.ibaGlobals.Struct1.Var2" 
        /// (or something like this depending on defined gloabl prefixes).
        /// </summary>
        /// <returns>Returns "" on error, or string name on success</returns>
        private string KlsConvertVeNameToUaName(string veName, bool includeIbaRootNodeName)
        {
            // declare prefixes for better code readability 
            string internalPrefix, externalPrefix;

            // determine variable group Globals or Tasks
            SubTreeId id = KlsGetSubtreeForGivenVeName(veName);

            switch (id)
            {
                case SubTreeId.Globals:
                    // variable name is from Gloabal group
                    internalPrefix = InternalPrefixForGlobalVariables;
                    //externalPrefix = VariableInformation.PrefixGlobalsDot;
                    break;
                case SubTreeId.Tasks:
                    // variable name is from Tasks group
                    internalPrefix = InternalPrefixForTaskVariables;
                    //externalPrefix = VariableInformation.PrefixTasksDot;
                    break;
                default:
                    // cannot identify variable group 
                    return "";
            }

            // replace pmac prefix with internal prefix
            string uaName = internalPrefix;// + veName.Substring(externalPrefix.Length);

            // now it should look like for example "ibaRoot.ibaGlobals.Struct1.Elm2"

            // paranoic check
            // path should always start with "ibaRoot."
            string prefix = string.Format("{0}.", _folderIbaRoot.BrowseName.Name);
            if (!uaName.StartsWith(prefix))
            {
                Status.AddNewError(string.Format("Found uaName '{0}' not starting with ibaRootFolder name.", uaName));
                return "";
            }

            // if we do not need to include ibaRoot, then remove it from name
            if (!includeIbaRootNodeName)
                // remove "ibaRoot." prefix as requested
                // this can be needed to start browsing down from ibaRoot
                uaName = uaName.Substring(prefix.Length);

            return uaName;
        }


        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <param name="veName"></param>
        /// <returns></returns>
        public static SubTreeId KlsGetSubtreeForGivenVeName(string veName)
        {
            //if (veName.StartsWith(VariableInformation.PrefixGlobalsDot)) return SubTreeId.Globals;
            
            //// currently will always return true since PmacPrefixForTaskVariables == "";
            //// is added here for compatibility if PmacPrefixForTaskVariables will be non-empty
            //if (veName.StartsWith(VariableInformation.PrefixTasksDot)) return SubTreeId.Tasks;

            // unknown prefix - unknown subtree
            return SubTreeId.Unknown;
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

        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <param name="startFolder"></param>
        /// <param name="varFullNameAsList"></param>
        /// <returns></returns>
        public FolderState KlsCreatePath(FolderState startFolder, List<string> varFullNameAsList)
        {
            if (startFolder == null) return null;

            if (varFullNameAsList == null || varFullNameAsList.Count == 0) return null;

            string topmost = varFullNameAsList[0];

            // check whether topmost folder exists among our immediate children
            BaseInstanceState subElemet = startFolder.FindChild(SystemContext, new QualifiedName(topmost, NamespaceIndex));
            FolderState subfolder = null;

            if (subElemet != null)
            {
                if (subElemet is FolderState)
                {
                    // subfolder already exists, ok
                    subfolder = subElemet as FolderState;
                }
                else
                {
                    // element with this name already exists but it is not a folder
                    // this is rare case (when scalar is replaced by the struct with the same name)
                    // we should replace var with the folder
                    // (otherwise we will have name conflict)
                    // so, delete subelement without check of usage, because we have to
                    KlsDeleteNodeAndSubtreeAndRemoveFromParent(subElemet, false);
                    // subfolder with this name will be created in next statement
                }
            }

            if (subfolder == null)
            {
                // topmost folder does not exist
                // create it
                subfolder = KlsCreateFolderAndItsNode(startFolder, topmost);
            }

            // get subpath relative to the subfolder
            varFullNameAsList.RemoveAt(0);
            if (varFullNameAsList.Count == 0)
                // nothing else left to find/create, path is ok
                // return lowermost subfolder
                return subfolder;
            else
            // create the rest of the path recursively
            // ReSharper disable once TailRecursiveCall - for better readability (recursion depth should not be big)
                return KlsCreatePath(subfolder, varFullNameAsList);
        }

        /// <summary>
        /// By Kolesnik. 
        /// 1. Recursively destroys the subtree of given node.
        /// 2. Removes node from its parent.
        /// 3. Destroys parent (and all superparents) if it is empty, unless the parent is a global root
        /// </summary>
        /// <param name="node">instance to be deleted</param>
        /// <param name="enableDeletingOfEmptyParents"></param>
        public void KlsDeleteNodeAndSubtreeAndRemoveFromParent(BaseInstanceState node, bool enableDeletingOfEmptyParents)
        {
            // remember node's parent to destroy it later if necessary
            NodeState parent = node.Parent;

            // call delete-handlers for the node (and recursively for the whole subtree)
            node.Delete(SystemContext);

            // remove the node and (all children) from manger's dictionary and remove node from parent's list
            DeleteNode(SystemContext, node.NodeId);
            
            // mark node as deleted
            IbaVariable iv = node as IbaVariable;
            if (iv != null)
            {
                iv.StatusCode = StatusCodes.BadObjectDeleted;
                iv.ClearChangeMasks(SystemContext, false);
            }
            // go on with deleting of our parent (and superparents) if it is empty
            if (!enableDeletingOfEmptyParents) return;

            // if parent is null or is not a folder, then don't touch it
            if (!(parent is FolderState)) return;
            
            // if parent is one of roots, then don't touch it
            // (though _folderIbaTask should be deleted if we have no task-nodes, we do not delete it automatically here; 
            // we do in the UpdateVarTree function)
            if (parent == _folderIbaRoot || parent == _folderIbaGlobals || parent == _folderIbaTask)
                return;

            // if it is not-root folder and is empty now, then also delete it
            List<BaseInstanceState> children = new List<BaseInstanceState>();
            parent.GetChildren(SystemContext, children);
            if (children.Count == 0)
            {
                // parent has no children, we do not need an empty folder
                // ReSharper disable once TailRecursiveCall - for better readability (recursion depth should not be big)
                KlsDeleteNodeAndSubtreeAndRemoveFromParent(parent as FolderState, true);
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


        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <param name="varState"></param>
        /// <param name="value"></param>
        public void KlsSetValueScalar(BaseDataVariableState varState, object value)
        {

            // set value
            varState.Value = value;

            // on success set status code
            varState.StatusCode = StatusCodes.Good;

            // call state-change handlers
            varState.ClearChangeMasks(SystemContext, false);
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

        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public FolderState KlsCreateFolderAndItsNode(NodeState parent, string name)
        {
            FolderState folder = new FolderState(parent);

            QualifiedName browseName = new QualifiedName(name, NamespaceIndex);

            CreateNode(SystemContext, parent.NodeId, ReferenceTypeIds.Organizes, browseName, folder);

            folder.TypeDefinitionId = ObjectTypeIds.FolderType;
            folder.WriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
            folder.UserWriteMask = AttributeWriteMask.DisplayName | AttributeWriteMask.Description;
            folder.ReferenceTypeId = ReferenceTypes.Organizes;

            return folder;
        }

        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="dataType"></param>
        /// <returns></returns>
        public BaseDataVariableState KlsCreateStatusVariableAndItsNode(NodeState parent, string name, BuiltInType dataType)
        {
            BaseDataVariableState v = new BaseDataVariableState(parent);

            // create node for given instance
            KlsCreateNodeForDataVariable(v, name);
            
            // set access type to readonly
            v.SetAccessLevel(true, false);
            v.SetupAsScalar(BuiltInType.String);
            return v;
        }

        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <param name="subTreeId"></param>
        /// <param name="ve"></param>
        /// <returns></returns>
        private IbaVariable KlsCreateIbaVariableAndItsNode(NodeState parent, string name, SubTreeId subTreeId, object ve)
        {
            IbaVariable v = new IbaVariable(parent, subTreeId, null, this);

            // create node for given instance
            KlsCreateNodeForDataVariable(v, name);

            v.OnSimpleWriteValue += OnWriteIbaNode;

            return v;
        }


        /// <summary>
        /// By Kolesnik. 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="fullName"></param>
        private void  KlsCreateNodeForDataVariable(BaseDataVariableState v, string fullName)
        {
            // compose browse name
            QualifiedName browseName = new QualifiedName(fullName, NamespaceIndex);

            // create node for given instance
            CreateNode(SystemContext, v.Parent.NodeId, ReferenceTypeIds.Organizes, browseName, v);

            // set up attributes to defaults
            {
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

                // do not set to good until we have data
                v.StatusCode = StatusCodes.BadNoData;
            }
        }



        


        /// <summary>
        /// By Kolesnik. 
        /// Remove variable from PMAC's watch list if this variable is not monitored (e.g. by someone else)
        /// </summary>
        private void KlsRemoveWatchIfUnused(IbaVariable ibaVariableToRemove)
        {
            if (ibaVariableToRemove == null) return;
            //lock (Lock)
            {
                if (MonitoredNodes == null) return;
                foreach (var item in MonitoredNodes)
                {
                    if (ibaVariableToRemove.NodeId == item.Key)
                        // our var is still monitored by someone else,
                        // so, do NOT remove it from watch list
                        return;
                }
            }

            // it is not used, remove it
            ibaVariableToRemove.IsMonitored = false;
            _ibaUaServer.KlsRemoveWatch(ibaVariableToRemove.VeName);
        }

        #endregion

        #region ua event handlers by Kolesnik

        private ServiceResult OnWriteIbaNode(ISystemContext context, NodeState node, ref object value)
        {
            // check user acess rights
            IbaOpcUaUserAccount acc = _ibaUaServer.KlsGetUserForSession(context.SessionId);
            if (!acc.CanWrite) return new ServiceResult(StatusCodes.BadUserAccessDenied);
            
            // todo
            // in future we will have coversion table with delegates and including sizes
            // big switch here is not good


            IbaVariable iv = node as IbaVariable;

            // paranoic check
            if (iv == null)
                // should not happen
                // we add this event handler only to iba variables.
                // nevertheless cancel furhter actions
                return new ServiceResult(StatusCodes.BadInternalError);

            //// paranoic check - no reference?
            //if (iv.VariableElement == null)
            //    // should not happen
            //    // during varTree update all ibaVAriables should be supplied with correct reference to their ve
            //    // nevertheless cancel furhter actions
            //    return new ServiceResult(StatusCodes.BadInternalError);

            //// paranoic check - reference consistency
            //if (!ReferenceEquals(iv.VariableElement.UaVariable, iv))
            //    // should not happen
            //    // during varTree update all references should be checked for consistency
            //    // nevertheless cancel furhter actions
            //    return new ServiceResult(StatusCodes.BadInternalError);

            ushort sz;
            try
            {
                switch ((BuiltInType)(uint)(iv.DataType.Identifier))
                {
                    case BuiltInType.Boolean:
                        sz = 1;
                        break;
                    case BuiltInType.SByte:
                        sz = 1;
                        break;
                    case BuiltInType.Int16:
                        sz = 2;
                        break;
                    case BuiltInType.Int32:
                        sz = 4;
                        break;
                    case BuiltInType.Int64:
                        sz = 4;
                        break;

                    case BuiltInType.Byte:
                        sz = 1;
                        break;
                    case BuiltInType.UInt16:
                        sz = 2;
                        break;
                    case BuiltInType.UInt32:
                        sz = 4;
                        break;
                    case BuiltInType.UInt64:
                        sz = 8;
                        break;
                    case BuiltInType.Float:
                        sz = 4;
                        break;
                    case BuiltInType.Double:
                        sz = 8;
                        break;

                    default:
                        // strings and arrays are not supported yet
                        // todo add support for strings and arrays
                        sz = 0;
                        break;
                }
            }
            catch
            {
                // if id is not a built-in type, then it references to a TypeNode and has a complex type
                // it is not supported
                // todo implement complex types
                return new ServiceResult(StatusCodes.BadDataTypeIdUnknown);
            }

            // unknown/unsupported type
            if (sz == 0)
                return new ServiceResult(StatusCodes.BadDataTypeIdUnknown);


            // todo typecheck
            // todo use StatusCodes.BadTypeMismatch

            // okay
            _ibaUaServer.KlsWrite(null, value, sz);


            return ServiceResult.Good;
        }

        #endregion

        #region override by kolesnik

        protected override void OnMonitoredItemCreated(ServerSystemContext context, NodeHandle handle, MonitoredItem monitoredItem)
        {
            base.OnMonitoredItemCreated(context, handle, monitoredItem);

            var x = monitoredItem.MonitoringMode;

            // we do not do additional actions if user monitors non-iba node
            if (handle == null || !(handle.Node is IbaVariable)) return;

            // user monitors iba-node
            IbaVariable iv = (IbaVariable) (handle.Node);

            // if node was deleted, do not try to watch it
            if (iv.IsDeleted) return;

            // remember that it is monitored
            iv.IsMonitored = true;

            // get name in VariableElement notation
            if (string.IsNullOrEmpty(iv.VeName))
            {
                // should not happen...
                // do not add this to PMAC watchlist
                Status.AddNewError(string.Format("Cannot get ve-Name for variable '{0}'", iv));
                return;
            }

            // add it to PMAC's watchlist
            _ibaUaServer.KlsAddWatch(iv.VeName);
        }

        protected override void OnMonitoredItemDeleted(ServerSystemContext context, NodeHandle handle, MonitoredItem monitoredItem)
        {
            base.OnMonitoredItemDeleted(context, handle, monitoredItem);

            // we are interested in non-null iba data variables
            // ignore anything else
            if (handle == null || !(handle.Node is IbaVariable))
                return;

            // several monitored items can point to one and the same node
            // so before removing the watch, we should be sure that there are no other monitored itmes using given node
            // MonitoredNodes list is rebuilt automatically by OPC Kit
            // we need just to look for the handle inside MonitoredNodes list
            // if this handle is still inside Monitored nodes, then we should avoid removing it from PMAC's wathlist
            KlsRemoveWatchIfUnused(handle.Node as IbaVariable);
        }

        public override void Read(OperationContext context, double maxAge, IList<ReadValueId> nodesToRead, IList<DataValue> values, IList<ServiceResult> errors)
        {
            // handle a request if user reads a value of ibaNode
            foreach (ReadValueId valueId in nodesToRead)
            {
                // we handle only read of value
                if (valueId.AttributeId != Attributes.Value) continue;

                NodeState foundNode = Find(valueId.NodeId);
                IbaVariable v = foundNode as IbaVariable;

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
                _ibaUaServer.KlsAddWatch(_temporaryWatch.VeName);
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
