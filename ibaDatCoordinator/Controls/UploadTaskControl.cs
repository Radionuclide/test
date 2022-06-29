using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DevExpress.Utils.Extensions;
using DevExpress.XtraPrinting.Native;
using iba.Data;
using iba.Processing;
using iba.Properties;
using iba.Utility;

namespace iba.Controls
{
    public partial class UploadTaskControl : UserControl, IPropertyPane
    {
        public UploadTaskControl()
        {
            InitializeComponent();

            m_uncControl = new UNCTaskControl();
            panelOut.Controls.Add(m_uncControl);
            m_uncControl.Dock = DockStyle.Fill;
            m_uncControl.HideModifyDateOption();
            
            ConfigureSearchFileTextBox();
        }

        private UNCTaskControl m_uncControl;

        public void LeaveCleanup()
        {
        }


        IPropertyPaneManager m_manager;
        private UploadTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as UploadTaskData;

            m_uncControl.SetData(m_data);

            m_btnCheckConnection.Image = Icons.Gui.All.Images.CircleQuestionFilledBlue(32);

            m_tbServer.Text = m_data.Server;
            m_tbPort.Text = m_data.Port;
            m_tbUsername.Text = m_data.Username;
            m_tbPassword.Text = m_data.Password;
            m_tbRemotePath.Text = m_data.RemotePath;
            m_tbPathToPrivateKey.Text = m_data.PathToPrivateKey;
            m_tbPathToCertificate.Text = m_data.PathToCertificate;
            m_createZipArchive.Checked = m_data.CreateZipArchive;

            m_chkAnonymous.Checked = m_data.Anonymous;
            m_chkAcceptAnySshHostKey.Checked = m_data.AcceptAnySshHostKey;
            m_chkAcceptAnyTlsCertificate.Checked = m_data.AcceptAnyTlsCertificate;

            m_cbProtocol.SelectedIndex = (int)m_data.Protocol;
            m_cbEncryption.SelectedIndex = (int)m_data.EncryptionChoice;
            m_cmbMode.SelectedIndex = (int)m_data.Mode;
            
            m_rbDatFile.Checked = m_data.WhatFileUpload == UploadTaskData.WhatFileUploadEnum.DATFILE;
            m_rbPrevOutput.Checked = m_data.WhatFileUpload == UploadTaskData.WhatFileUploadEnum.PREVOUTPUT;
        }

        public void SaveData()
        {
            m_data.Server = m_tbServer.Text;
            m_data.Port = m_tbPort.Text;
            m_data.Username = m_tbUsername.Text;
            m_data.Password = m_tbPassword.Text;
            m_data.RemotePath = m_tbRemotePath.Text;
            m_data.PathToPrivateKey = m_tbPathToPrivateKey.Text;
            m_data.PathToCertificate = m_tbPathToCertificate.Text;
            m_data.CreateZipArchive = m_createZipArchive.Checked;

            m_data.Protocol = (UploadTaskData.TransferProtocol)m_cbProtocol.SelectedIndex;
            m_data.EncryptionChoice = (UploadTaskData.EncryptionChoiceEnum)m_cbEncryption.SelectedIndex;
            m_data.Mode = (UploadTaskData.FtpMode)m_cmbMode.SelectedIndex;

            m_data.Anonymous = m_chkAnonymous.Checked;
            m_data.AcceptAnySshHostKey = m_chkAcceptAnySshHostKey.Checked;
            m_data.AcceptAnyTlsCertificate = m_chkAcceptAnyTlsCertificate.Checked;

            if (m_rbPrevOutput.Checked)
                m_data.WhatFileUpload = UploadTaskData.WhatFileUploadEnum.PREVOUTPUT;
            else if (m_rbDatFile.Checked)
                m_data.WhatFileUpload = UploadTaskData.WhatFileUploadEnum.DATFILE;

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);

            m_uncControl.SaveData();
            m_data.UpdateUNC();
        }

        private enum Protocol
        {
            FTP,
            SFTP,
            SCP,
            AMAZON_S3,
            AzureDataLake
        }

        private enum Encryption
        {
            Unencrypted,
            Explicit,
            Implicit
        }

        private void m_cbProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            SuspendLayout();
            var selectedProtocol = (Protocol)((ComboBox)sender).SelectedIndex;
            var selectedEncryption = (Encryption)m_cbEncryption.SelectedIndex;

            ForAllControls(m_gbOption, (control) => control.Visible = true);
            ForAllControls(m_gbProtocol, (control) => control.Visible = true);

            switch (selectedProtocol)
            {
                case Protocol.FTP:
                    SetupFtpControls(selectedEncryption);
                    break;
                case Protocol.SFTP:
                case Protocol.SCP:
                    SetupSftpAndScpControls();
                    break;
                case Protocol.AMAZON_S3:
                    SetupAmazonS3Controls();
                    break;
                case Protocol.AzureDataLake:
                    SetupAzureControls();
                    break; 
            }


            SetStandardPorts(selectedProtocol);
            ResumeLayout();
        }
        private void m_cbEncryption_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedEncryption = (Encryption)((ComboBox)sender).SelectedIndex;
            var selectedProtocol = (Protocol)m_cbProtocol.SelectedIndex;

            var ftpEncryptionControls = new List<Control>
            {
                m_chkAcceptAnyTlsCertificate, m_tlpPathtoCertificate
            };
            ftpEncryptionControls.ForEach(control => control.Visible = selectedProtocol == Protocol.FTP &&
                                                                       selectedEncryption != Encryption.Unencrypted);

            var sshEncryptionControls = new List<Control> { m_chkAcceptAnySshHostKey, m_tlpPathToKey };
            sshEncryptionControls.ForEach(control => control.Visible = selectedProtocol == Protocol.SFTP ||
                                                                       selectedProtocol == Protocol.SCP);

            SetStandardPorts(selectedProtocol);
        }
        private static void ForAllControls(Control parent, Action<Control> action)
        {
            foreach (Control c in parent.Controls)
            {
                action(c);
                ForAllControls(c, action);
            }
        }

        private void SetupFtpControls(Encryption selectedEncryption)
        {
            m_lbUsername.Text = $"{Resources.Username}:";
            m_lbPassword.Text = $"{Resources.Password}:";

            if (selectedEncryption == Encryption.Unencrypted)
            {
                m_chkAcceptAnyTlsCertificate.Visible = false;
                m_tlpPathtoCertificate.Visible = false;
            }

            m_chkAcceptAnySshHostKey.Visible = false;
            m_tlpPathToKey.Visible = false;
        }

        private void SetupSftpAndScpControls()
        {
            m_lbUsername.Text = $"{Resources.Username}:";
            m_lbPassword.Text = $"{Resources.Password}:";

            m_panelFtpEncryption.Visible = false;
            m_chkAnonymous.Visible = false;
            m_tlpPathtoCertificate.Visible = false;
            m_chkAcceptAnyTlsCertificate.Visible = false;
        }

        private void SetupAmazonS3Controls()
        {
            m_lbUsername.Text = $"{Resources.Access_key_ID}:";
            m_lbPassword.Text = $"{Resources.Secret_key}:";

            if (string.IsNullOrEmpty(m_tbServer.Text))
            {
                m_tbServer.Text = "s3.amazonaws.com";
            }

            m_panelFtpEncryption.Visible = false;
            m_chkAnonymous.Visible = false;
            m_panelChkBoxControls.Visible = false;
            m_panelFilePath.Visible = false;
        }

        private void SetupAzureControls()
        {
            m_lbUsername.Text = $"{Resources.AccountName}:";
            m_lbPassword.Text = $"{Resources.SharedKey}:";

            m_chkAnonymous.Visible = false;
            m_lbServer.Visible = false;
            m_tbServer.Visible = false;
            m_lbPort.Visible = false;
            m_tbPort.Visible = false;
            m_panelFtpEncryption.Visible = false;
            m_panelChkBoxControls.Visible = false;
            m_panelFilePath.Visible = false;
        }

        private void SetStandardPorts(Protocol selectedProtocol)
        {
            var selectedEncryption = (Encryption)m_cbEncryption.SelectedIndex;

            const string ftp = "21";
            const string implicitFtp = "990";
            const string sftp = "22";
            const string amazonS3 = "443";

            var isStandardPort = m_tbPort.Text == ftp ||
                                 m_tbPort.Text == sftp ||
                                 m_tbPort.Text == implicitFtp ||
                                 m_tbPort.Text == amazonS3;

            if (!isStandardPort)
                return;

            switch (selectedProtocol)
            {
                case Protocol.FTP:
                    m_tbPort.Text = selectedEncryption == Encryption.Implicit ? implicitFtp : ftp;
                    break;
                case Protocol.SFTP:
                case Protocol.SCP:
                    m_tbPort.Text = sftp;
                    break;
                case Protocol.AMAZON_S3:
                    m_tbPort.Text = amazonS3;
                    break;
                case Protocol.AzureDataLake:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(selectedProtocol), selectedProtocol, null);
            }
        }

        private void m_chkAnonymous_CheckedChanged(object sender, EventArgs e)
        {
            var checkbox = (CheckBox)sender;
            const string anonymous = "anonymous";

            if (checkbox.Checked)
            {
                m_tbUsername.Text = anonymous;
                m_tbPassword.Text = anonymous;
                return;
            }

            checkbox.Checked = IsAnonymousInserted();
        }

        private void m_tbUsername_TextChanged(object sender, EventArgs e)
        {
            m_chkAnonymous.Checked = IsAnonymousInserted();
        }

        private void m_tbPassword_TextChanged(object sender, EventArgs e)
        {
            m_chkAnonymous.Checked = IsAnonymousInserted();
        }

        private bool IsAnonymousInserted()
        {
            const string anonymous = "anonymous";

            return m_tbUsername.Text == anonymous && m_tbPassword.Text == anonymous;
        }

        private void m_btnCheckConnection_Click(object sender, EventArgs e)
        {
            SaveData();
            var errorMessage = string.Empty;
            var ok = false;

            using (new WaitCursor())
            {
                var uploadTaskWorker = UploadTaskWorker.CreateWorker(m_data);

                try
                {
                    ok = uploadTaskWorker.TestConnection(out errorMessage, m_data);
                }
                catch (Exception ex)
                {
                    LogData.Data.Log(Logging.Level.Exception, iba.Properties.Resources.logUploadTaskFailed + ": " + ex.Message);
                }
            }

            if (ok)
            {
                m_btnCheckConnection.Image = Icons.Gui.All.Images.ThumbUp(32);
            }
            else
            {
                MessageBox.Show(errorMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_btnCheckConnection.Image = Icons.Gui.All.Images.ThumbDown(32);
            }
        }

        private void ConfigureSearchFileTextBox()
        {
            var searchPrivateKeyBtn = new Button();
            var searchCertificateBtn = new Button();

            var buttonlist = new List<Button> { searchCertificateBtn, searchPrivateKeyBtn };

            buttonlist.ForEach(btn => {
                btn.Size = new Size(25, m_tbPathToPrivateKey.ClientSize.Height + 2);
                btn.Dock = DockStyle.Right;
                btn.Cursor = Cursors.Default;
                btn.Text = ". . .";
                btn.FlatStyle = FlatStyle.Flat;
                btn.ForeColor = Color.Black;
                btn.Font = new Font("Arial", 6, FontStyle.Bold);
                btn.FlatAppearance.BorderColor = Color.Black;
                btn.FlatAppearance.BorderSize = 15;
                btn.BackColor = Color.DarkGray;
                btn.FlatAppearance.BorderSize = 0;
                btn.Click += SearchPrivateKeyBtn_Click;
            });


            m_tbPathToCertificate.Controls.Add(searchCertificateBtn);
            m_tbPathToPrivateKey.Controls.Add(searchPrivateKeyBtn);

            SendMessage(m_tbPathToCertificate.Handle, 0xd3, (IntPtr)2, (IntPtr)(searchPrivateKeyBtn.Width << 16));
            SendMessage(m_tbPathToPrivateKey.Handle, 0xd3, (IntPtr)2, (IntPtr)(searchPrivateKeyBtn.Width << 16));
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private void SearchPrivateKeyBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            var filename = openFileDialog1.FileName;

            if (m_cbProtocol.SelectedIndex == (int)Protocol.FTP)
            {
                m_tbPathToCertificate.Text = filename;
            }
            else
            {
                m_tbPathToPrivateKey.Text = filename;
            }
        }
    }
}
