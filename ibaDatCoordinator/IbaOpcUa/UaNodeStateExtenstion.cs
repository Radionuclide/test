using System.Collections.Generic;
using Opc.Ua;

namespace ibaOpcServer.IbaOpcUa
{
    public static class UaNodeStateExtenstion
    {
        /// <summary>
        /// Recursively gets the list of all children that are of IbaVariable type.
        /// </summary>
        /// <param name="startFolder">Start folder to begin search with</param>
        /// <param name="systemContext">NodeManagers' context</param>
        /// <returns>Newly created flat list</returns>
        public static List<IbaVariable> GetFlatListOfIbaVariableChildren(this FolderState startFolder, SystemContext systemContext)
        {
            List<IbaVariable> allChildren = new List<IbaVariable>();
            List<BaseInstanceState> immediateChildren = new List<BaseInstanceState>();
            if (startFolder == null) return null;

            startFolder.GetChildren(systemContext, immediateChildren);

            foreach (BaseInstanceState elm in immediateChildren)
            {
                if (elm is FolderState)
                    allChildren.AddRange(((FolderState)(elm)).GetFlatListOfIbaVariableChildren(systemContext));
                else if (elm is IbaVariable)
                    allChildren.Add(elm as IbaVariable);
            }
            return allChildren;
        }

        //public static List<BaseInstanceState> GetFlatListOfChildren(this BaseInstanceState startFolder, SystemContext systemContext)
        //{
        //    List<BaseInstanceState> allChildren = new List<BaseInstanceState>();
        //    List<BaseInstanceState> immediateChildren = new List<BaseInstanceState>();
        //    startFolder.GetChildren(systemContext, immediateChildren);

        //    allChildren.Add(startFolder);
        //    foreach (BaseInstanceState elm in immediateChildren)
        //        allChildren.AddRange(elm.GetFlatListOfChildren(systemContext));

        //    return allChildren;
        //}

        /// <summary>
        /// Sets AccessLevel and UserAccessLevel.
        /// </summary>
        public static void SetAccessLevel(this BaseDataVariableState v, bool readAccess, bool writeAccess)
        {
            v.AccessLevel = AccessLevels.None;
            if (readAccess) v.AccessLevel += AccessLevels.CurrentRead;
            if (writeAccess) v.AccessLevel += AccessLevels.CurrentWrite;
            v.UserAccessLevel = v.AccessLevel;
        }

        /// <summary>
        /// Sets TypeDefinitionId and ValueRank to Scalar; also sets given data type
        /// </summary>
        /// <param name="v"></param>
        /// <param name="dataType"></param>
        public static void SetupAsScalar(this BaseDataVariableState v, BuiltInType dataType)
        {
            v.TypeDefinitionId = VariableTypeIds.BaseDataVariableType;

            v.DataType = (uint)dataType;
            v.ValueRank = ValueRanks.Scalar;
        }

        /// <summary>
        /// Sets TypeDefinitionId and ValueRank to 1-dimensional array; also sets given data type and array size
        /// </summary>
        /// <param name="v"></param>
        /// <param name="dataType">data type of array's items</param>
        /// <param name="itemsCount">items count (not bytes count)</param>
        public static void SetupAsArray(this IbaVariable v, BuiltInType dataType, uint itemsCount)
        {
            v.TypeDefinitionId = VariableTypeIds.ArrayItemType;

            v.DataType = (uint)dataType;
            v.ValueRank = ValueRanks.OneDimension;
            // currently we support only one dimension
            // todo implement arbitrary dimensions
            v.ArrayDimensions = new[] { itemsCount };

        }
    }
}
