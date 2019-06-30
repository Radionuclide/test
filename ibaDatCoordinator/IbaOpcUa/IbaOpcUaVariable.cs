using iba.Data;
using iba.ibaOPCServer;
using Opc.Ua;

namespace ibaOpcServer.IbaOpcUa
{

    public class IbaOpcUaVariable : BaseDataVariableState
    {
        public bool IsMarkedForDeleting = false;
        public bool IsMonitored = false;
        public bool IsAvailableInPmacWatchlist = false;

        internal ExtMonData.ExtMonVariableBase ExtMonVar; // todo. kls. 

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

        public IbaOpcUaVariable(NodeState parent, object ve, IbaUaNodeManager mgr)
            : base(parent)
        {
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

        //public bool IsReadable
        //{
        //    get { return (UserAccessLevel & AccessLevels.CurrentRead) != 0; }
        //}
        //public bool IsWritable
        //{
        //    get { return (UserAccessLevel & AccessLevels.CurrentWrite) != 0; }
        //}

        protected override void OnAfterCreate(ISystemContext context, NodeState node)
        {
            // take this into account in statistics
            _mgr?.Status.IncrementVarCounter();
            
            base.OnAfterCreate(context, node);
        }

        protected override void OnBeforeDelete(ISystemContext context)
        {
            // take this into account in statistics
            _mgr?.Status?.DecrementVarCounter();
            
            // set this flag; it will be checked when user will try to monitor it
            IsDeleted = true;

            base.OnBeforeDelete(context);
        }

    }


}
