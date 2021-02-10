using iba.Data;
using iba.Plugins;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.Controls
{
    public partial class ErrorPluginTaskControl : UserControl, IPluginControl, IPropertyPane
    {
        public ErrorPluginTaskControl()
        {
            InitializeComponent();
        }

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            ICustomTaskData data = datasource as ICustomTaskData;
            ErrorPluginTaskData errorData = data?.Plugin as ErrorPluginTaskData;
            if (!String.IsNullOrEmpty(errorData?.ErrorMessage))
                lbError.Text = errorData.ErrorMessage;
            else
                lbError.Text = "Something unknown went wrong here";
        }

        public void LoadData(object datasource, ICommonTaskControl parentcontrol)
        {
            //This won't be called because we are not part of the CommonTaskControl
            //ErrorPluginTaskData data = datasource as ErrorPluginTaskData;
            //if (!String.IsNullOrEmpty(data?.ErrorMessage))
            //    lbError.Text = data.ErrorMessage;
            //else
            //    lbError.Text = "Something unknown went wrong here";
        }

        public void SaveData()
        {
        }

        public void LeaveCleanup()
        {
        }

    }
}
