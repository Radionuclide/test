using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.CertificateStore;
using iba.CertificateStore.Forms;
using iba.CertificateStore.Proxy;
using iba.Data;
using iba.Dialogs;
using iba.Processing;
using iba.Processing.IbaGrpc;
using iba.Utility;
using Messages.V1;

namespace iba.Controls
{
    public partial class DataTransferTaskControl : UserControl, IPropertyPane, ICertificatesControlHost
    {
        public DataTransferTaskControl()
        {
            InitializeComponent();
            InitializeIcons();
            m_numBandwidth.Maximum = decimal.MaxValue;
            serverCertCb = CertificatesComboBox.ReplaceCombobox(ServerCertPlaceholder, useRegistry: false);
        }

        private void InitializeIcons()
        {
            this.m_btnCheckConnection.Image = Icons.Gui.All.Images.ThumbUp(32);
        }

        public void LeaveCleanup()
        {
        }


        IPropertyPaneManager m_manager;
        private DataTransferTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as DataTransferTaskData;

            m_cbDeleteAfterTransfer.Checked = m_data.ShouldDeleteAfterTransfer;
            m_CreateZipArchive.Checked = m_data.ShouldCreateZipArchive;
            
            m_btnCheckConnection.Image = Icons.Gui.All.Images.CircleQuestionFilledBlue(32);

            m_tbServer.Text = m_data.Server;
            m_numericUpDownPort.Text = m_data.Port;
            m_tbRemotePath.Text = m_data.RemotePath;
            m_numBandwidth.Text = m_data.MaxBandwidth.ToString();
            
            m_chkLimitBandwidth.Checked = m_data.ChkLimitBandwidth;
            m_cbBandwidth.SelectedIndex = m_data.CbBandwidth;
            m_numBandwidth.Value = m_data.NumBandwidth;

            m_cbBandwidth.Enabled = m_data.ChkLimitBandwidth;
            m_numBandwidth.Enabled = m_data.ChkLimitBandwidth;

            m_rbDatFile.Checked = m_data.WhatFileTransfer == DataTransferTaskData.WhatFileTransferEnum.DATFILE;
            m_rbPrevOutput.Checked = m_data.WhatFileTransfer == DataTransferTaskData.WhatFileTransferEnum.PREVOUTPUT;

            serverCertParams = new CertificateInfo
            {
                Thumbprint = m_data.ServerCertificateThumbprint
            };

            serverCertCb.UnsetEnvironment();
            serverCertCb.SetEnvironment(this, serverCertParams);
        }

        public void SaveData()
        {
            m_data.Server = m_tbServer.Text;
            m_data.Port = m_numericUpDownPort.Text;
            m_data.RemotePath = m_tbRemotePath.Text;
            m_data.MaxBandwidth = CalculateMaxBandwidth();
            m_data.ShouldDeleteAfterTransfer = m_cbDeleteAfterTransfer.Checked;
            m_data.ShouldCreateZipArchive = m_CreateZipArchive.Checked;

            m_data.ChkLimitBandwidth = m_chkLimitBandwidth.Checked;
            m_data.CbBandwidth = m_cbBandwidth.SelectedIndex;
            m_data.NumBandwidth = m_numBandwidth.Value;

            if (m_rbPrevOutput.Checked)
                m_data.WhatFileTransfer = DataTransferTaskData.WhatFileTransferEnum.PREVOUTPUT;
            else if (m_rbDatFile.Checked)
                m_data.WhatFileTransfer = DataTransferTaskData.WhatFileTransferEnum.DATFILE;

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
            m_data.ServerCertificateThumbprint = serverCertParams.Thumbprint;
        }

        private int CalculateMaxBandwidth()
        {
            if (!m_chkLimitBandwidth.Checked) return 0;

            switch (m_cbBandwidth.SelectedItem.ToString())
            {
                case "kbps":
                    return int.Parse(m_numBandwidth.Text);
                case "mbps":
                    return int.Parse(m_numBandwidth.Text) * 1024;
            }

            return 0;
        }

        private async void m_btnCheckConnection_Click(object sender, EventArgs e)
        {
            SaveData();
            var errormessage = string.Empty;
            var ok = false;

            try
            {
                var client = new GrpcClient(m_data);
                
                this.Cursor = Cursors.WaitCursor;
                this.m_btnCheckConnection.Enabled = false;

                var result = await client.TestConnectionAsync(m_data);


                if (result.Status == Status.Ok)
                {
                    ok = true;
                }
                else
                {
                    errormessage = result.Message;
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Log(Logging.Level.Exception, iba.Properties.Resources.logUploadTaskFailed + ": " + ex.Message);
                errormessage = ex.Message;
                ok = false;
            }

            this.Cursor = Cursors.Default;
            this.m_btnCheckConnection.Enabled = true;
            m_btnCheckConnection.Image = Icons.Gui.All.Images.ThumbDown(32);

            if (ok)
            {
                m_btnCheckConnection.Text = null;
                m_btnCheckConnection.Image = Icons.Gui.All.Images.ThumbUp(32);
            }
            else
            {
                MessageBox.Show(errormessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_btnCheckConnection.Image = Icons.Gui.All.Images.ThumbDown(32);
            }
        }

        private void m_btnSelectServer_Click(object sender, EventArgs e)
        {
            using (var ssf = new ServerSelectionForm(new ServerConfiguration(), true))
            {
                ssf.OnServerInfoSelected += FillServerFields;
                ssf.ShowDialog();
            }
        }

        private void FillServerFields(string server, string port)
        {
            m_tbServer.Text = server;
            m_numericUpDownPort.Text = port;
        }


        private void m_cbLimitBandwidth_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is CheckBox checkBox)) return;

            m_cbBandwidth.Enabled = checkBox.Checked;
            m_numBandwidth.Enabled = checkBox.Checked;
        }

        private void m_cbBandwidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                m_numBandwidth.Minimum = comboBox.SelectedItem.ToString() == "kbps" ? 64 : 1;
            }
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
            (m_manager as MainForm)?.MoveToSettigsTab();
        }

        public void JumpToCertificateInfoNode(string displayName)
        {
            throw new NotImplementedException();
        }

        class CertificateInfo : ICertificateInfo
        {
            public string Thumbprint { get; set; }
            public CertificateRequirement CertificateRequirements { get; }
            public string DisplayName => "Certificate for Data Transfer Task";
        }

        public bool IsLocalHost { get; }
        public string ServerAddress { get; }
        public ICertificateManagerProxy CertificateManagerProxy { get; } = new CertificateManagerProxyJsonAdapter(new AppCertificateManagerJsonProxy());
        public bool IsCertificatesReadonly => false;
        public bool IsReadOnly { get; }
        public string UsagePart => "EX";
        public IWin32Window Instance => this;
        public ContextMenuStrip PopupMenu => new ContextMenuStrip();

        public List<int> ServerUsageIds => new List<int>();

        #endregion
    }
}
