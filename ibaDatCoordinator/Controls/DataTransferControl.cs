﻿using System;
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
            try
            {
                m_data = dataSource as DataTransferData;
                m_manager = manager;
                m_cbEnabled.Checked = m_data.ServerEnabled;
                m_numPort.Value = m_data.Port;
            }
            catch (Exception e)
            {
                LogData.Data.Logger.Log(Level.Exception, @"DataTransferControl.SaveData() exception: " + e.Message);
            }
        }

        public void SaveData()
        {
            try
            {
                m_data.ServerEnabled = m_cbEnabled.Checked;
                m_data.Port = (int)m_numPort.Value;
                TaskManager.Manager.DataTransferData = m_data.Clone() as DataTransferData;
            }
            catch (Exception e)
            {
                LogData.Data.Logger.Log(Level.Exception, @"DataTransferControl.LoadData() exception: " + e.Message);
            }
        }

        public void LeaveCleanup()
        {
   
        }

        private void cbEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (((CheckBox)sender).Checked)
            {
                //TaskManager.Manager.DataTransferWorkerInit();
                DataTransferWorker.Port = (int)m_numPort.Value;
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