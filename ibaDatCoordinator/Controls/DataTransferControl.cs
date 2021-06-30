﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using DevExpress.XtraPrinting.Native;
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
        private readonly DataTransferWorker DataTransferWorker;
        private DataTransferData _data;
        private BindingList<DiagnosticsData> _diagnosticsDataList;
        private IPropertyPaneManager _manager;
        public DataTransferControl()
        {
            InitializeComponent();
            DataTransferWorker = TaskManager.Manager.DataTransferWorker;
            DataTransferWorker.ClientManager.UpdateDiagnosticInfoCallback += UpdateDiagnosticInfo;
            ConfigureDiagnosticGrid();
        }

        public void LoadData(object dataSource, IPropertyPaneManager manager)
        {
            try
            {
                _data = dataSource as DataTransferData;
                _manager = manager;
                m_cbEnabled.Checked = _data.ServerEnabled;
                m_numPort.Value = _data.Port;
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
                _data.ServerEnabled = m_cbEnabled.Checked;
                _data.Port = (int)m_numPort.Value;
                TaskManager.Manager.DataTransferData = _data.Clone() as DataTransferData;
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
            dgvClients.Rows.Clear();
        }

        private delegate void UpdateDiagnosticInfoCallback(DiagnosticsData diagnosticsData);
        private void UpdateDiagnosticInfoSafe(DiagnosticsData diagnosticsData)
        {
            if (this.dgvClients.InvokeRequired)
            {
                var callback = new UpdateDiagnosticInfoCallback(UpdateDiagnosticInfoSafe);
                this.Invoke(callback, new object[] { diagnosticsData });
            }
            
            var elem = _diagnosticsDataList.FirstOrDefault(x => x.ClientName == diagnosticsData.ClientName);

            if (elem == null)
            {
                _diagnosticsDataList.Add(diagnosticsData);
                return;
            }

            elem.ClientName = diagnosticsData.ClientName;
            elem.TransferredFiles = diagnosticsData.TransferredFiles;
        }

        private void UpdateDiagnosticInfo(DiagnosticsData diagnosticsData)
        {
            UpdateDiagnosticInfoSafe(diagnosticsData);
        }

        private void ConfigureDiagnosticGrid()
        {
            _diagnosticsDataList = new BindingList<DiagnosticsData>();
            
            dgvClients.DataSource = _diagnosticsDataList;

            dgvClients.Columns[nameof(DiagnosticsData.ClientName)].HeaderText = Resources.Hostname;
            dgvClients.Columns[nameof(DiagnosticsData.ClientName)].DisplayIndex = 0;

            dgvClients.Columns[nameof(DiagnosticsData.ClientVersion)].HeaderText = Resources.Version;
            dgvClients.Columns[nameof(DiagnosticsData.ClientVersion)].DisplayIndex = 1;

            dgvClients.Columns[Resources.Path].HeaderText = Resources.Path;
            dgvClients.Columns[Resources.Path].DisplayIndex = 2;

            dgvClients.Columns[nameof(DiagnosticsData.TransferredFiles)].HeaderText = Resources.TransferredFiles;
            dgvClients.Columns[nameof(DiagnosticsData.TransferredFiles)].DisplayIndex = 3;

            dgvClients.Columns[nameof(DiagnosticsData.Filename)].HeaderText = Resources.Last_transferred_file;
            dgvClients.Columns[nameof(DiagnosticsData.Filename)].DisplayIndex = 4;

            dgvClients.Columns[nameof(DiagnosticsData.ApiKey)].Visible = false;

            dgvClients.Columns.OfType<DataGridViewColumn>()
                .ForEach(column => column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill);
        }
    }
}