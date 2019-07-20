using System.Diagnostics;
using iba.Data;
using iba.ibaOPCServer;
using Opc.Ua;

namespace ibaOpcServer.IbaOpcUa
{
    public class IbaOpcUaVariable : BaseDataVariableState
    {
        public bool IsMarkedForDeleting = false;
        public bool IsMonitored = false;

        public ExtMonData.ExtMonVariableBase ExtMonVar;

        /// <summary>
        /// This flag is created for quick check.
        /// The same can be done slowly, using manager.Find() function, that performs
        /// search in manager's nodes dictionary. (if Find() fails, then node is deleted).
        /// Deleted nodes do not appear in browse tree. But if user tries to access it directly, 
        /// (for example using a reference from old monitoring list) we should deny access to this node.
        /// </summary>
        public bool IsDeleted { get; protected set; }

        // todo. kls. comment
        public bool IsDeletionPending { get; set; }

        protected readonly IbaUaNodeManager _mgr;

        public IbaOpcUaVariable(NodeState parent, ExtMonData.ExtMonVariableBase xmv, IbaUaNodeManager mgr)
            : base(parent)
        {
            Debug.Assert(xmv != null);

            SetCrossReference(xmv);

            // remember ve name to bind this ibaVariable to certain name
            // in future even if Ve is renewed in online server,
            // this ibaVariable will 'cooperate' with another Ve ONLY if it has the same name
            //VeName = ve.VariableName;

            // remember our manager
            _mgr = mgr;
        }

        public void SetCrossReference(ExtMonData.ExtMonVariableBase xmv)
        {
            // add references to each other
            ExtMonVar = xmv;
            xmv.UaVar = this;

            // todo. kls. destroy old references
        }

        protected override void OnAfterCreate(ISystemContext context, NodeState node)
        {
           
            base.OnAfterCreate(context, node);
        }

        protected override void OnBeforeDelete(ISystemContext context)
        {
            // set this flag; it will be checked when user will try to monitor it
            IsDeleted = true;

            base.OnBeforeDelete(context);
        }

    }


}
