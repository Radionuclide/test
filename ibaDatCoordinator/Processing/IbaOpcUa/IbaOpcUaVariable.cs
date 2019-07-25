using System.Diagnostics;
using iba.Data;
using Opc.Ua;

namespace iba.Processing.IbaOpcUa
{
    public class IbaOpcUaVariable : BaseDataVariableState
    {
        public ExtMonData.ExtMonVariableBase ExtMonVar;

        /// <summary>
        /// This flag is created for quick check.
        /// The same can be done slowly, using manager.Find() function, that performs
        /// search in manager's nodes dictionary. (if Find() fails, then node is deleted).
        /// Deleted nodes do not appear in browse tree. But if user tries to access it directly, 
        /// (for example using a reference from old monitoring list) we should deny access to this node.
        /// </summary>
        public bool IsDeleted { get; protected set; }

        public IbaOpcUaVariable(NodeState parent, ExtMonData.ExtMonVariableBase xmv)
            : base(parent)
        {
            Debug.Assert(xmv != null);

            SetCrossReference(xmv);
        }

        public void SetCrossReference(ExtMonData.ExtMonVariableBase xmv)
        {
            var oldXmv = ExtMonVar;
            // add references to each other
            ExtMonVar = xmv;
            xmv.UaVar = this;

            // destroy old reference
            if (!ReferenceEquals(oldXmv, xmv) && oldXmv != null)
                oldXmv.UaVar = null;
        }

        protected override void OnBeforeDelete(ISystemContext context)
        {
            // set this flag; it will be checked when user will try to monitor it
            IsDeleted = true;

            // ensure we don't have access to ExtMonData even if 
            // the instance is cached somewhere
            ExtMonVar = null;

            base.OnBeforeDelete(context);
        }

    }
}
