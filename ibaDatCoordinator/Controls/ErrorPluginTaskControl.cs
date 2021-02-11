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

            lbError.Font = new Font(lbError.Font.FontFamily, lbError.Font.SizeInPoints + 2, FontStyle.Bold);
        }

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            ICustomTaskData data = datasource as ICustomTaskData;
            ErrorPluginTaskData errorData = data?.Plugin as ErrorPluginTaskData;
            if (String.IsNullOrEmpty(errorData?.ErrorMessage))
            {
                lbError.Text = "Something unknown went wrong here";
                return;
            }

            string text = errorData.ErrorMessage;
            text = text.Replace(". ", ".\r\n"); //New line after each sentence
            lbError.Text = text;
        }

        public void LoadData(object datasource, ICommonTaskControl parentcontrol)
        {
            //This won't be called because we are not part of the CommonTaskControl
        }

        public void SaveData()
        {
        }

        public void LeaveCleanup()
        {
        }

    }
}
