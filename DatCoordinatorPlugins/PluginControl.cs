using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;


namespace iba.Plugins
{
    /// <summary>
    /// Interface specifying an appropriate GUI for a TaskData plugin
    /// </summary>
    public interface IPluginControl
    {
        void LoadData(object datasource, Control parentcontrol);
        void SaveData();
    }
}
