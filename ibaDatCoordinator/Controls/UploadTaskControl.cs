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
using iba.Data;
using iba.Processing;
using iba.Utility;

namespace iba.Controls
{
    public partial class UploadTaskControl : UserControl, IPropertyPane
    {
        public UploadTaskControl()
        {
            InitializeComponent();
            ConfigureSearchFileTextBox();
        }

        public void LeaveCleanup()
        {
        }


        IPropertyPaneManager m_manager;
        private UploadTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as UploadTaskData;

            m_btnCheckConnection.Image = null;
            m_btnCheckConnection.Text = "?";

            m_tbServer.Text = m_data.Server;
            m_tbPort.Text = m_data.Port;
            m_tbUsername.Text = m_data.Username;
            m_tbPassword.Text = m_data.Password;
            m_tbRemotePath.Text = m_data.RemotePath;
            m_tbPathToPrivateKey.Text = m_data.PathToPrivateKey;
            m_tbPathToCertificate.Text = m_data.PathToCertificate;

            m_cbProtocol.SelectedIndex = (int)m_data.Protocol;
            m_cbEncryption.SelectedIndex = (int)m_data.EncryptionChoice;
            m_cmbMode.SelectedIndex = (int)m_data.Mode;


            m_rbDatFile.Checked = m_data.WhatFileUpload == UploadTaskData.WhatFileUploadEnum.DATFILE;
            m_rbPrevOutput.Checked = m_data.WhatFileUpload == UploadTaskData.WhatFileUploadEnum.PREVOUTPUT;

            m_chkAnonymous.Checked = m_data.Anonymous;
            m_chkAcceptAnySshHostKey.Checked = m_data.AcceptAnySshHostKey;
            m_chkAcceptAnyTlsCertificate.Checked = m_data.AcceptAnyTlsCertificate;
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
        }

        private void m_cbProtocol_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedProtocol = (string)((ComboBox)sender).SelectedItem;
            var selectedEncryption = (string)m_cbEncryption.SelectedItem;

            var ftpControls = new List<Control>
            {
                m_lbEncryption, m_cbEncryption, m_lbFtpMode, m_cmbMode, m_chkAnonymous
            };

            ftpControls.ForEach(control => control.Visible = selectedProtocol == "FTP");

            var ftpControlsUnencrypted = new List<Control>
            {
                m_chkAcceptAnyTlsCertificate, m_tlpPathtoCertificate
            };

            ftpControlsUnencrypted.ForEach(control => control.Visible = selectedProtocol == "FTP" && selectedEncryption != "Use only unencrypted FTP (insecure)");

            var sshControls = new List<Control>
            {
                m_chkAcceptAnySshHostKey, m_tlpPathToKey
            };

            sshControls.ForEach(control => control.Visible = selectedProtocol == "SFTP" || selectedProtocol == "SCP");

            SetStandardPorts(selectedProtocol);
        }

        private void m_cbEncryption_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedEncryption = (string)((ComboBox)sender).SelectedItem;
            var selectedProtocol = (string)m_cbProtocol.SelectedItem;

            var ftpEncryptionControls = new List<Control>
            {
                m_chkAcceptAnyTlsCertificate, m_tlpPathtoCertificate
            };
            ftpEncryptionControls.ForEach(control => control.Visible = selectedProtocol == "FTP" && selectedEncryption != "Use only unencrypted FTP (insecure)");

            var sshEncryptionControls = new List<Control> { m_chkAcceptAnySshHostKey, m_tlpPathToKey };
            sshEncryptionControls.ForEach(control => control.Visible = selectedProtocol == "SFTP" || selectedProtocol == "SCP");

            SetStandardPorts(selectedProtocol);
        }

        private void SetStandardPorts(string selectedProtocol)
        {
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
                case "FTP":
                    m_tbPort.Text = "Require implicit FTP over TLS" == (string)m_cbEncryption.SelectedItem ? implicitFtp : ftp;
                    break;
                case "SFTP":
                case "SCP":
                    m_tbPort.Text = sftp;
                    break;
                case "Amazon S3":
                    m_tbPort.Text = amazonS3;
                    break;
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
            var errormessage = string.Empty;
            var ok = false;

            using (var wait = new WaitCursor())
            {

                var uploadTaskWorker = new UploadTaskWorker(string.Empty, m_data);

                try
                {
                    ok = uploadTaskWorker.TestConnection(out errormessage);
                }
                catch (Exception ex)
                {
                    LogData.Data.Log(Logging.Level.Exception, iba.Properties.Resources.logUploadTaskFailed + ": " + ex.Message);

                }

                m_btnCheckConnection.Text = null;
                m_btnCheckConnection.Image = iba.Properties.Resources.thumbdown;
                ((Bitmap)m_btnCheckConnection.Image).MakeTransparent(Color.Magenta);
            }

            if (ok)
            {
                m_btnCheckConnection.Text = null;
                m_btnCheckConnection.Image = iba.Properties.Resources.thumup;
            }
            else
            {
                MessageBox.Show(errormessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_btnCheckConnection.Text = null;
                m_btnCheckConnection.Image = iba.Properties.Resources.thumbdown;
            }
            ((Bitmap)m_btnCheckConnection.Image).MakeTransparent(Color.Magenta);
        }

        private void ConfigureSearchFileTextBox()
        {
            var SearchPrivateKeyBtn = new Button();
            var SearchCertificateBtn = new Button();

            var buttonlist = new List<Button> { SearchCertificateBtn, SearchPrivateKeyBtn };

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


            m_tbPathToCertificate.Controls.Add(SearchCertificateBtn);
            m_tbPathToPrivateKey.Controls.Add(SearchPrivateKeyBtn);

            SendMessage(m_tbPathToCertificate.Handle, 0xd3, (IntPtr)2, (IntPtr)(SearchPrivateKeyBtn.Width << 16));
            SendMessage(m_tbPathToPrivateKey.Handle, 0xd3, (IntPtr)2, (IntPtr)(SearchPrivateKeyBtn.Width << 16));
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr SendMessage(IntPtr hWnd, int msg, IntPtr wp, IntPtr lp);

        private void SearchPrivateKeyBtn_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            var filename = openFileDialog1.FileName;

            if (m_cbProtocol.SelectedItem.ToString() == "FTP")
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
