using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using iba.Annotations;
using iba.Data;
using iba.Logging;
using iba.Processing;
using iba.Processing.IbaGrpc;
using iba.Properties;
using Messages.V1;


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
            DataTransferWorker = TaskManager.Manager.DataTransferWorker;
            DataTransferWorker.ClientManager.UpdateDiagnosticInfoCallback += UpdateDiagnosticInfo;
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

        private void buttonClearClients_Click(object sender, EventArgs e)
        {
        }

        delegate void UpdateDiagnosticInfoCallback(BindingList<DiagnosticsData> diagnosticsData);
        private void UpdateDiagnosticInfoSafe(BindingList<DiagnosticsData> diagnosticsData)
        {
            if (this.dgvClients.InvokeRequired)
            {
                var callback = new UpdateDiagnosticInfoCallback(UpdateDiagnosticInfoSafe);
                this.Invoke(callback, new object[] { diagnosticsData });
            }

            ConfigureDiagnosticGrid(diagnosticsData);
        }

        private void UpdateDiagnosticInfo(BindingList<DiagnosticsData> diagnosticsDatas)
        {
            UpdateDiagnosticInfoSafe(diagnosticsDatas);
        }

        private void ConfigureDiagnosticGrid(BindingList<DiagnosticsData> diagnosticsData)
        {
            if (dgvClients.DataSource != null) return;


            dgvClients.DataSource = diagnosticsData;

            dgvClients.Columns["ClientName"].HeaderText = "Hostname";
            dgvClients.Columns["ClientVersion"].HeaderText = "Version";
            dgvClients.Columns["Path"].HeaderText = "Path";
            dgvClients.Columns["Filename"].HeaderText = "FileName";
            dgvClients.Columns["ApiKey"].Visible = false;
            dgvClients.Columns["TransferredFiles"].HeaderText = "Transferrred Files";

            dgvClients.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        }
    }
}