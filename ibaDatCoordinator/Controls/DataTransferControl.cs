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
        private readonly DataTransferWorker _dataTransferWorker;
        private DataTransferData _data;
        private BindingList<DiagnosticsData> _diagnosticsDataList;
        private readonly string _defaultPath = AppDomain.CurrentDomain.BaseDirectory;
        private readonly int _defaultPort = 30051;
        private IPropertyPaneManager _manager;
        public DataTransferControl()
        {
            InitializeComponent();
            _dataTransferWorker = TaskManager.Manager.DataTransferWorker;
            _dataTransferWorker.ClientManager.UpdateDiagnosticInfoCallback += UpdateDiagnosticInfo;
            ConfigureDiagnosticGrid();
        }

        public void LoadData(object dataSource, IPropertyPaneManager manager)
        {
            try
            {
                _data = dataSource as DataTransferData;
                _manager = manager;
                m_cbEnabled.Checked = _data.IsServerEnabled;
                m_numPort.Value = _data.Port;
                
                (tbStatus.Text, m_numPort.Enabled, btnRootPath.Enabled, btnCertificatePath.Enabled) 
                    = _dataTransferWorker.GetStatus();


                if (string.IsNullOrEmpty(_data.RootPath))
                {
                    _data.RootPath = _defaultPath;
                }
                
                if (string.IsNullOrEmpty(_data.Port.ToString()))
                {
                    _data.Port = _defaultPort;
                }

                tbRootPath.Text = _data.RootPath;
                tbCertificatePath.Text = _data.CertificatePath;
                m_numPort.Text = _data.Port.ToString();
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
                _data.IsServerEnabled = m_cbEnabled.Checked;
                _data.Port = (int)m_numPort.Value;
                _data.RootPath = tbRootPath.Text;
                _data.CertificatePath = tbCertificatePath.Text;
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
                _dataTransferWorker.StartServer();

                (tbStatus.Text, m_numPort.Enabled, btnRootPath.Enabled, btnCertificatePath.Enabled)
                    = _dataTransferWorker.GetStatus();
            }
            else
            {
                _dataTransferWorker.StopServer();

                (tbStatus.Text, m_numPort.Enabled, btnRootPath.Enabled, btnCertificatePath.Enabled)
                    = _dataTransferWorker.GetStatus();
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
            else
            {
                var elem = _diagnosticsDataList.FirstOrDefault(x => x.ClientId == diagnosticsData.ClientId);

                if (elem == null)
                {
                    _diagnosticsDataList.Add(diagnosticsData);
                    return;
                }

                elem.ClientName = diagnosticsData.ClientName;
                elem.Filename = diagnosticsData.Filename;
                elem.Path = diagnosticsData.Path;
                elem.MaxBandwidth = diagnosticsData.MaxBandwidth;
                elem.TransferredFiles = diagnosticsData.TransferredFiles;
            }
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

            dgvClients.Columns[nameof(DiagnosticsData.MaxBandwidth)].HeaderText = Resources.Max__bandwidth;
            dgvClients.Columns[nameof(DiagnosticsData.MaxBandwidth)].DisplayIndex = 5;

            dgvClients.Columns[nameof(DiagnosticsData.ApiKey)].Visible = false;
            dgvClients.Columns[nameof(DiagnosticsData.ClientId)].Visible = false;

            dgvClients.Columns.OfType<DataGridViewColumn>()
                .ForEach(column => column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill);
        }

        private void btnRootPathOrBtnCertificatePath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            var isRootPath = ((Button)sender).Text.Equals("Select root path");

            folderBrowserDialog.Description = isRootPath ? "Select root directory" : "Select path to certificate";

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;

            if (isRootPath)
            {
                _data.RootPath = tbRootPath.Text = folderBrowserDialog.SelectedPath;
            }
            else
            {
                _data.CertificatePath = tbCertificatePath.Text = folderBrowserDialog.SelectedPath;
            }
        }
    }
}