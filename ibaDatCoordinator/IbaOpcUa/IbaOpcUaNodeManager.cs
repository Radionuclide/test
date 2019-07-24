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
            if (!(node is BaseInstanceState instance) || instance.Parent == null)
                return node.NodeId;

            if (instance.Parent.NodeId.Identifier is string strId)
            {
                // Replaced underline (_) with our own NODE_ID_DELIMITER
                return new NodeId(ComposeNodeId(strId, instance.SymbolicName), instance.Parent.NodeId.NamespaceIndex);
                    
                // todo. kls. check uniqueness here??
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

                // paranoic check
                Debug.Assert(Find(FolderIbaRoot.NodeId) != null);
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


            parent?.AddChild(folder);

            return folder;
        }

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

        #region ibaDatCoordinator-Specific

        public static readonly char NODE_ID_DELIMITER = '\\';
        public static readonly char NODE_ID_DEFAULT_REPLACEMENT_CHARACTER = '_';

        /// <summary> Composes string node id, using <see cref="NODE_ID_DELIMITER"/>. 
        /// E.g.: "abc\de" + "fgh" -> "abc\de\fgh" </summary>
        public static string ComposeNodeId(string parentFullId, string nodeBrowseName) 
            => parentFullId + NODE_ID_DELIMITER + GetAdaptedBrowseName(nodeBrowseName);

        private readonly IbaOpcUaServer _ibaUaServer;

        /// <summary> Root folder for all iba-specific data </summary>
        public FolderState FolderIbaRoot { get; private set; }

       
        public Dictionary<NodeId, MonitoredNode2> GetMonitoredNodes() =>
            MonitoredNodes;

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

        /// <summary> Deletes all empty folders below a given folder and then
        /// also deletes the given folder if it is (or it became) empty.
        /// <see cref="FolderIbaRoot"/> and its immediate children are always preserved.
        /// </summary>
        /// <param name="parentFolder">A folder to cleanup; or null to cleanup the whole tree</param>
        public void DeleteEmptySubfolders(FolderState parentFolder = null)
        {
            // if parent folder is not specified, then use root folder
            parentFolder = parentFolder ?? FolderIbaRoot;

            // first try to delete all children on lower level
            foreach (var node in GetChildren(parentFolder))
            {
                if (node is FolderState folder)
                    DeleteEmptySubfolders(folder);
            }

            // check if we can delete parentFolder itself
            if (parentFolder == FolderIbaRoot /* is root */ || 
                parentFolder.Parent == FolderIbaRoot /* is immediate child of root (section folder) */ ||
                GetChildren(parentFolder).Count > 0 /*is not empty*/)
                return; // we cannot delete it
            
            // it is a simple empty folder, it can be deleted
            DeleteNodeRecursively(parentFolder, false);
        }


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
            FormatEnum(ref value);
            var uaType = GetOpcUaType(value);
            Debug.Assert((NodeId)(uint)uaType == varState.DataType);

            // set value
            varState.Value = value;

            // on success set status code
            varState.StatusCode = StatusCodes.Good;

            // call state-change handlers
            varState.ClearChangeMasks(SystemContext, false);
        }

        public void SetNullValueAndMarkAsDeleted(BaseDataVariableState varState)
        {
            // set value
            varState.Value = null;

            // on success set status code
            varState.StatusCode = StatusCodes.BadObjectDeleted;

            // call state-change handlers
            varState.ClearChangeMasks(SystemContext, false);
        }

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
            v.UserAccessLevel = v.AccessLevel = AccessLevels.CurrentRead;

            // set up as scalar
            v.TypeDefinitionId = VariableTypeIds.BaseDataVariableType;
            v.DataType = (uint)uaType;
            v.ValueRank = ValueRanks.Scalar;

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

        public List<BaseInstanceState> GetFlatListOfAllChildren(FolderState folder = null)
        {
            folder = folder ?? FolderIbaRoot;
            Debug.Assert(folder != null);

            var allChildren = new List<BaseInstanceState> {folder};

            foreach (BaseInstanceState baseInstanceState in GetChildren(folder))
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

        public List<IbaOpcUaVariable> GetFlatListOfAllIbaVariables(FolderState folder = null)
        {
            folder = folder ?? FolderIbaRoot;
            Debug.Assert(folder != null);

            var allVariables = new List<IbaOpcUaVariable>();

            foreach (BaseInstanceState baseInstanceState in GetChildren(folder))
            {
                switch (baseInstanceState)
                {
                    case FolderState childFolder:
                        var subChildren = GetFlatListOfAllIbaVariables(childFolder);
                        if (subChildren != null)
                            allVariables.AddRange(subChildren);
                        break;
                    case IbaOpcUaVariable childVar:
                        allVariables.Add(childVar);
                        break;
                }
            }
            return allVariables;
        }

        #endregion

    }
}
