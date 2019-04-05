using iba.ibaOPCServer;
using Opc.Ua;

namespace ibaOpcServer.IbaOpcUa
{

    public class IbaVariable : BaseDataVariableState
    {
        public readonly SubTreeId SubTreeId;

        public bool IsMarkedForDeleting = false;
        public bool IsMonitored = false;
        public bool IsAvailableInPmacWatchlist = false;

        /// <summary>
        /// This flag is created for quick check.
        /// The same can be done slowly, using manager.Find() function, that performs
        /// search in manager's nodes dictionary. (if Find() fails, then node is deleted).
        /// Deleted nodes do not appear in browse tree. But if user tries to access it directly, 
        /// (for example using a reference from old monitoring list) we should deny access to this node.
        /// </summary>
        public bool IsDeleted { get; protected set; }

        protected readonly IbaUaNodeManager _mgr;

        /// <summary>
        /// This is a name of the variable in VariableInformation.tVariableElement-notation.
        /// We never renamme folders dynamically and we do not move nodes inside a tree, so
        /// this name stays unchanged. 
        /// Though we have direct reference to VariableElement, VeName is still needed for better reliability.
        /// </summary>
        public readonly string VeName;

        public IbaVariable(NodeState parent, SubTreeId subTreeId, object ve, IbaUaNodeManager mgr)
            : base(parent)
        {
            SubTreeId = subTreeId;

            // add references to each other
            //VariableElement = ve;
            //ve.UaVariable = this;

            // remember ve name to bind this ibaVariable to certain name
            // in future even if Ve is renewed in online server,
            // this ibaVariable will 'cooperate' with another Ve ONLY if it has the same name
            //VeName = ve.VariableName;

            // remember our manager
            _mgr = mgr;

            // not deleted, can be used normally
            IsDeleted = false;
        }

        public bool IsReadable
        {
            get { return (UserAccessLevel & AccessLevels.CurrentRead) != 0; }
        }
        public bool IsWtitable
        {
            get { return (UserAccessLevel & AccessLevels.CurrentWrite) != 0; }
        }

        protected override void OnAfterCreate(ISystemContext context, NodeState node)
        {
            // take this into account in statistics
            _mgr.Status.IncrementVarCounter(SubTreeId);
            
            base.OnAfterCreate(context, node);
        }

        protected override void OnBeforeDelete(ISystemContext context)
        {
            // take this into account in statistics
            _mgr.Status.DecrementVarCounter(SubTreeId);
            
            // set this flag; it will be checked when user will try to monitor it
            IsDeleted = true;

            base.OnBeforeDelete(context);
        }

    }


}
