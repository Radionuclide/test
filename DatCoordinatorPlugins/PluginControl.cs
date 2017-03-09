using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace iba.Plugins
{
    /// <summary>
    /// Interface specifying an appropriate GUI for a plugin
    /// </summary>
    public interface IPluginControl
    {
        void LoadData(object datasource, ICommonTaskControl parentcontrol);
        void SaveData();
        void LeaveCleanup();
    }

    public interface ICommonTaskControl
    {
        Guid ParentConfigurationGuid();
        int TaskIndex();
    }

    /// <summary>
    /// Interface specifying an appropriate GUI for a plugin in the case of an UNC plugin
    /// </summary>
    public interface IPluginControlUNC : IPluginControl
    {
        /// <summary>
        /// Property that specifies wheter the control resizes in height or not.
        /// If the control can have a variable height, datco needs to now so he can set anchors for this control and the UNC part properly.
        /// </summary>
        /// <returns>
        /// True if the height is fixed.
        /// </returns>
        bool FixedHeight
        {
            get;
        }
    }
}
