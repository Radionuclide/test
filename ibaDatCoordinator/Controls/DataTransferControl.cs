using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iba.Data;
using iba.Logging;
using iba.Processing;
using iba.Processing.IbaGrpc;
using iba.Properties;


namespace iba.Controls
{
    public partial class DataTransferControl : UserControl, IPropertyPane
    {
        private DataTransferWorker DataTransferWorker;
        private DataTransferData m_data;
        private IPropertyPaneManager m_manager;
        public DataTransferControl()
        {
            InitializeComponent();
            DataTransferWorker = new DataTransferWorker();
        }

        public void LoadData(object dataSource, IPropertyPaneManager manager)
        {
           m_data = dataSource as DataTransferData;
           m_manager = manager;
        }

        public void SaveData()
        {
          
        }

        public void LeaveCleanup()
        {
   
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                DataTransferWorker.StartServer();
                tbStatus.Text = "Server started";
            }
            else
            {
                DataTransferWorker.StopServer();
                tbStatus.Text = "Server stopped";
            }
        }
    }
}