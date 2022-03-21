using System;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.XtraPrinting.Native;
using iba.CertificateStore;
using iba.CertificateStore.Forms;
using iba.CertificateStore.Proxy;
using iba.Data;
using iba.Logging;
using iba.Processing;
using iba.Processing.IbaGrpc;
using iba.Properties;


namespace iba.Controls
{
    public partial class DataTransferControl : UserControl, IPropertyPane, ICertificatesControlHost
    {
        private DataTransferData _data;
        private BindingList<DiagnosticsData> _diagnosticsDataList;
        private readonly string _defaultPath = AppDomain.CurrentDomain.BaseDirectory;
        private readonly string _defaultCertPath = AppDomain.CurrentDomain.BaseDirectory;
        private readonly int _defaultPort = 30051;
        private IPropertyPaneManager _manager;
        public DataTransferControl()
        {
            InitializeComponent();
            TaskManager.Manager.DataTransferWorkerSetUpdateDiagnosticInfoCallback(UpdateDiagnosticInfo);
            TaskManager.Manager.DataTransferWorkerSetUpdateServerStatusCallback(UpdateServerStatus);
            ConfigureDiagnosticGrid();
            GetAllClients();
            serverCertCb = CertificatesComboBox.ReplaceCombobox(ServerCertPlaceholder, useRegistry: false);
        }


        public async void LoadData(object dataSource, IPropertyPaneManager manager)
        {
            try
            {
                _data = dataSource as DataTransferData;
                _manager = manager;

                if (_data?.Port == null || _data?.RootPath == null)
                {
                    SetDefaultSettings();
                }
                else
                {
                    m_cbEnabled.Checked = _data.IsServerEnabled;
                    m_numPort.Value = _data.Port;

                    tbRootPath.Text = _data.RootPath;
                }

                tbStatus.Text = await Task.Run((Func<String>)TaskManager.Manager.DataTransferWorkerGetBriefStatus);

                serverCertParams = new CertificateInfo
                {
                    Thumbprint = _data.ServerCertificateThumbprint
                };

                serverCertCb.UnsetEnvironment();
                serverCertCb.SetEnvironment(this, serverCertParams);
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
                _data.ServerCertificateThumbprint = serverCertParams.Thumbprint;
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

        public void UpdateServerStatusSafe(string status)
        {
            if (this.tbStatus.InvokeRequired)
            {
                var callback = new Action<string>(UpdateServerStatusSafe);
                this.Invoke(callback, new object[] { status });
            }
            else
            {
                tbStatus.Text = status;
            }
        }

        public void UpdateServerStatus(string status)
        {
            UpdateServerStatusSafe(status);
        }

        private void UpdateDiagnosticInfoSafe(DiagnosticsData diagnosticsData)
        {
            if (this.dgvClients.InvokeRequired)
            {
                var callback = new Action<DiagnosticsData>(UpdateDiagnosticInfoSafe);
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
                elem.TaskName = diagnosticsData.TaskName;
                elem.Filename = diagnosticsData.Filename;
                elem.Path = diagnosticsData.Path;
                elem.MaxBandwidth = diagnosticsData.MaxBandwidth;
                elem.TransferredFiles = diagnosticsData.TransferredFiles;
            }
        }

        public void UpdateDiagnosticInfo(DiagnosticsData diagnosticsData)
        {
            UpdateDiagnosticInfoSafe(diagnosticsData);
        }

        private void buttonClearClients_Click(object sender, EventArgs e)
        {
            dgvClients.Rows.Clear();
        }

        private void ConfigureDiagnosticGrid()
        {
            _diagnosticsDataList = new BindingList<DiagnosticsData>();
            
            dgvClients.DataSource = _diagnosticsDataList;

            dgvClients.Columns[nameof(DiagnosticsData.ClientName)].HeaderText = Resources.Hostname;
            dgvClients.Columns[nameof(DiagnosticsData.ClientName)].DisplayIndex = 0;

            dgvClients.Columns[nameof(DiagnosticsData.ClientVersion)].HeaderText = Resources.Version;
            dgvClients.Columns[nameof(DiagnosticsData.ClientVersion)].DisplayIndex = 1;
            
            dgvClients.Columns[nameof(DiagnosticsData.TaskName)].HeaderText = Resources.TaskName;
            dgvClients.Columns[nameof(DiagnosticsData.TaskName)].DisplayIndex = 2;

            dgvClients.Columns[Resources.Path].HeaderText = Resources.Path;
            dgvClients.Columns[Resources.Path].DisplayIndex = 3;

            dgvClients.Columns[nameof(DiagnosticsData.TransferredFiles)].HeaderText = Resources.TransferredFiles;
            dgvClients.Columns[nameof(DiagnosticsData.TransferredFiles)].DisplayIndex = 4;

            dgvClients.Columns[nameof(DiagnosticsData.Filename)].HeaderText = Resources.Last_transferred_file;
            dgvClients.Columns[nameof(DiagnosticsData.Filename)].DisplayIndex = 5;

            dgvClients.Columns[nameof(DiagnosticsData.MaxBandwidth)].HeaderText = Resources.Max__bandwidth;
            dgvClients.Columns[nameof(DiagnosticsData.MaxBandwidth)].DisplayIndex = 6;

            dgvClients.Columns[nameof(DiagnosticsData.ApiKey)].Visible = false;
            dgvClients.Columns[nameof(DiagnosticsData.ClientId)].Visible = false;

            dgvClients.Columns.OfType<DataGridViewColumn>()
                .ForEach(column => column.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill);
        }

        private void btnRootPathOrBtnCertificatePath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog.RootFolder = Environment.SpecialFolder.Desktop;
            folderBrowserDialog.Description = "Select root directory";

            if (folderBrowserDialog.ShowDialog() != DialogResult.OK) 
                return;

            _data.RootPath = tbRootPath.Text = folderBrowserDialog.SelectedPath;
        }

        private async void  buttonConfigurationApply_Click(object sender, EventArgs e)
        {
            if (!ValidateInput())
            {
                return;
            }
            
            SaveData();

            if (m_cbEnabled.Checked)
            {
                await Task.Run((Action)TaskManager.Manager.DataTransferWorkerStartServer);
            }
            else
            {
                await Task.Run((Action)TaskManager.Manager.DataTransferWorkerStopServer);
            }
        }

        private bool ValidateInput()
        {
            if (!DirectoryManager.IsValidPath(tbRootPath.Text))
            {
                MessageBox.Show($"{lblRootPath.Text} not valid", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (m_numPort.Value == 0)
            {
                MessageBox.Show($"Port number 0 is not allowed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

        private void buttonConfigurationReset_Click(object sender, EventArgs e)
        {
            SetDefaultSettings();
            SaveData();
        }

        private void SetDefaultSettings()
        {
            tbStatus.Text = TaskManager.Manager.DataTransferWorkerGetBriefStatus();
            m_cbEnabled.Checked = false;
            m_numPort.Value = _defaultPort;
            tbRootPath.Text = _defaultPath;
        }

        private void GetAllClients()
        {
            var clients = TaskManager.Manager.DataTransferWorkerGetAllClients();
            
            clients.ForEach(item => _diagnosticsDataList.Add(item));
        }

        #region ICertificatesControlHost

        CertificatesComboBox serverCertCb;
        private CertificateInfo serverCertParams;

        public void OnSaveDataSource()
        {
            throw new NotImplementedException();
        }

        public ICertifiable GetCertifiableRootNode()
        {
            throw new NotImplementedException();
        }

        public void ManageCertificates()
        {
            (_manager as MainForm)?.MoveToSettigsTab();
        }

        public void JumpToCertificateInfoNode(string displayName)
        {
            throw new NotImplementedException();
        }

        class CertificateInfo : ICertificateInfo
        {
            public string Thumbprint { get; set; }
            public CertificateRequirement CertificateRequirements { get; }
            public string DisplayName => "Certificate for Data Transfer Server";
        }

        public bool IsLocalHost { get; }
        public string ServerAddress { get; }
        public ICertificateManagerProxy CertificateManagerProxy { get; } = new CertificateManagerProxyJsonAdapter(new AppCertificateManagerJsonProxy());
        public bool IsCertificatesReadonly => false;
        public bool IsReadOnly { get; }
        public string UsagePart => "EX";
        public IWin32Window Instance => this;
        public ContextMenuStrip PopupMenu => new ContextMenuStrip();

        #endregion
    }
}