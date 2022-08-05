
namespace iba.Controls
{
    partial class UploadTaskControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadTaskControl));
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.m_gbOption = new iba.Utility.CollapsibleGroupBox();
            this.m_tplOptions = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblRemotePath = new System.Windows.Forms.Label();
            this.m_tbRemotePath = new System.Windows.Forms.TextBox();
            this.panel6 = new System.Windows.Forms.Panel();
            this.m_createZipArchive = new System.Windows.Forms.CheckBox();
            this.m_gbProtocol = new iba.Utility.CollapsibleGroupBox();
            this.m_tplAuthorization = new System.Windows.Forms.TableLayoutPanel();
            this.m_panelChkBoxControls = new System.Windows.Forms.Panel();
            this.m_chkAcceptAnySshHostKey = new System.Windows.Forms.CheckBox();
            this.m_chkAcceptAnyTlsCertificate = new System.Windows.Forms.CheckBox();
            this.m_panelFilePath = new System.Windows.Forms.Panel();
            this.m_tlpPathToKey = new System.Windows.Forms.TableLayoutPanel();
            this.m_tbPathToPrivateKey = new System.Windows.Forms.TextBox();
            this.m_lbFileToPrivateKey = new System.Windows.Forms.Label();
            this.m_tlpPathtoCertificate = new System.Windows.Forms.TableLayoutPanel();
            this.m_tbPathToCertificate = new System.Windows.Forms.TextBox();
            this.m_lbFileWithCertificate = new System.Windows.Forms.Label();
            this.m_panelFtp = new System.Windows.Forms.Panel();
            this.m_lbUsername = new System.Windows.Forms.Label();
            this.m_chkAnonymous = new System.Windows.Forms.CheckBox();
            this.m_lbPassword = new System.Windows.Forms.Label();
            this.m_tbPassword = new System.Windows.Forms.TextBox();
            this.m_tbUsername = new System.Windows.Forms.TextBox();
            this.m_btnCheckConnection = new System.Windows.Forms.Button();
            this.m_tplProtocol = new System.Windows.Forms.TableLayoutPanel();
            this.m_panelFtpEncryption = new System.Windows.Forms.Panel();
            this.m_lbFtpMode = new System.Windows.Forms.Label();
            this.m_lbEncryption = new System.Windows.Forms.Label();
            this.m_cbEncryption = new System.Windows.Forms.ComboBox();
            this.m_cmbMode = new System.Windows.Forms.ComboBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_tbPort = new System.Windows.Forms.TextBox();
            this.m_lbPort = new System.Windows.Forms.Label();
            this.m_cbProtocol = new System.Windows.Forms.ComboBox();
            this.m_lbProtocol = new System.Windows.Forms.Label();
            this.m_lbServer = new System.Windows.Forms.Label();
            this.m_tbServer = new System.Windows.Forms.TextBox();
            this.m_gbSource = new iba.Utility.CollapsibleGroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbPrevOutput = new System.Windows.Forms.RadioButton();
            this.m_rbDatFile = new System.Windows.Forms.RadioButton();
            this.m_gbTarget = new iba.Utility.CollapsibleGroupBox();
            this.panelOut = new System.Windows.Forms.Panel();
            this.m_gbOption.SuspendLayout();
            this.m_tplOptions.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.panel6.SuspendLayout();
            this.m_gbProtocol.SuspendLayout();
            this.m_tplAuthorization.SuspendLayout();
            this.m_panelChkBoxControls.SuspendLayout();
            this.m_panelFilePath.SuspendLayout();
            this.m_tlpPathToKey.SuspendLayout();
            this.m_tlpPathtoCertificate.SuspendLayout();
            this.m_panelFtp.SuspendLayout();
            this.m_tplProtocol.SuspendLayout();
            this.m_panelFtpEncryption.SuspendLayout();
            this.panel1.SuspendLayout();
            this.m_gbSource.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.m_gbTarget.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_gbOption
            // 
            resources.ApplyResources(this.m_gbOption, "m_gbOption");
            this.m_gbOption.BackColor = System.Drawing.SystemColors.Control;
            this.m_gbOption.Controls.Add(this.m_tplOptions);
            this.m_gbOption.Name = "m_gbOption";
            this.m_gbOption.TabStop = false;
            // 
            // m_tplOptions
            // 
            resources.ApplyResources(this.m_tplOptions, "m_tplOptions");
            this.m_tplOptions.Controls.Add(this.panel5, 0, 0);
            this.m_tplOptions.Controls.Add(this.panel6, 0, 2);
            this.m_tplOptions.Name = "m_tplOptions";
            // 
            // panel5
            // 
            resources.ApplyResources(this.panel5, "panel5");
            this.m_tplOptions.SetColumnSpan(this.panel5, 2);
            this.panel5.Controls.Add(this.tableLayoutPanel5);
            this.panel5.Name = "panel5";
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.m_lblRemotePath, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.m_tbRemotePath, 1, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // m_lblRemotePath
            // 
            resources.ApplyResources(this.m_lblRemotePath, "m_lblRemotePath");
            this.m_lblRemotePath.Name = "m_lblRemotePath";
            // 
            // m_tbRemotePath
            // 
            resources.ApplyResources(this.m_tbRemotePath, "m_tbRemotePath");
            this.m_tbRemotePath.Name = "m_tbRemotePath";
            // 
            // panel6
            // 
            this.m_tplOptions.SetColumnSpan(this.panel6, 2);
            this.panel6.Controls.Add(this.m_createZipArchive);
            resources.ApplyResources(this.panel6, "panel6");
            this.panel6.Name = "panel6";
            // 
            // m_createZipArchive
            // 
            resources.ApplyResources(this.m_createZipArchive, "m_createZipArchive");
            this.m_createZipArchive.Name = "m_createZipArchive";
            this.m_createZipArchive.UseVisualStyleBackColor = true;
            // 
            // m_gbProtocol
            // 
            resources.ApplyResources(this.m_gbProtocol, "m_gbProtocol");
            this.m_gbProtocol.BackColor = System.Drawing.SystemColors.Control;
            this.m_gbProtocol.Controls.Add(this.m_tplAuthorization);
            this.m_gbProtocol.Controls.Add(this.m_tplProtocol);
            this.m_gbProtocol.Name = "m_gbProtocol";
            this.m_gbProtocol.TabStop = false;
            // 
            // m_tplAuthorization
            // 
            resources.ApplyResources(this.m_tplAuthorization, "m_tplAuthorization");
            this.m_tplAuthorization.BackColor = System.Drawing.SystemColors.Control;
            this.m_tplAuthorization.Controls.Add(this.m_panelChkBoxControls, 1, 2);
            this.m_tplAuthorization.Controls.Add(this.m_panelFilePath, 0, 2);
            this.m_tplAuthorization.Controls.Add(this.m_panelFtp, 0, 0);
            this.m_tplAuthorization.Controls.Add(this.m_btnCheckConnection, 1, 0);
            this.m_tplAuthorization.Name = "m_tplAuthorization";
            // 
            // m_panelChkBoxControls
            // 
            resources.ApplyResources(this.m_panelChkBoxControls, "m_panelChkBoxControls");
            this.m_panelChkBoxControls.Controls.Add(this.m_chkAcceptAnySshHostKey);
            this.m_panelChkBoxControls.Controls.Add(this.m_chkAcceptAnyTlsCertificate);
            this.m_panelChkBoxControls.Name = "m_panelChkBoxControls";
            // 
            // m_chkAcceptAnySshHostKey
            // 
            resources.ApplyResources(this.m_chkAcceptAnySshHostKey, "m_chkAcceptAnySshHostKey");
            this.m_chkAcceptAnySshHostKey.Name = "m_chkAcceptAnySshHostKey";
            this.m_chkAcceptAnySshHostKey.UseVisualStyleBackColor = true;
            // 
            // m_chkAcceptAnyTlsCertificate
            // 
            resources.ApplyResources(this.m_chkAcceptAnyTlsCertificate, "m_chkAcceptAnyTlsCertificate");
            this.m_chkAcceptAnyTlsCertificate.Name = "m_chkAcceptAnyTlsCertificate";
            this.m_chkAcceptAnyTlsCertificate.UseVisualStyleBackColor = true;
            // 
            // m_panelFilePath
            // 
            resources.ApplyResources(this.m_panelFilePath, "m_panelFilePath");
            this.m_panelFilePath.Controls.Add(this.m_tlpPathToKey);
            this.m_panelFilePath.Controls.Add(this.m_tlpPathtoCertificate);
            this.m_panelFilePath.Name = "m_panelFilePath";
            // 
            // m_tlpPathToKey
            // 
            resources.ApplyResources(this.m_tlpPathToKey, "m_tlpPathToKey");
            this.m_tlpPathToKey.Controls.Add(this.m_tbPathToPrivateKey, 1, 0);
            this.m_tlpPathToKey.Controls.Add(this.m_lbFileToPrivateKey, 0, 0);
            this.m_tlpPathToKey.Name = "m_tlpPathToKey";
            // 
            // m_tbPathToPrivateKey
            // 
            resources.ApplyResources(this.m_tbPathToPrivateKey, "m_tbPathToPrivateKey");
            this.m_tbPathToPrivateKey.Name = "m_tbPathToPrivateKey";
            // 
            // m_lbFileToPrivateKey
            // 
            resources.ApplyResources(this.m_lbFileToPrivateKey, "m_lbFileToPrivateKey");
            this.m_lbFileToPrivateKey.Name = "m_lbFileToPrivateKey";
            // 
            // m_tlpPathtoCertificate
            // 
            resources.ApplyResources(this.m_tlpPathtoCertificate, "m_tlpPathtoCertificate");
            this.m_tlpPathtoCertificate.Controls.Add(this.m_tbPathToCertificate, 1, 0);
            this.m_tlpPathtoCertificate.Controls.Add(this.m_lbFileWithCertificate, 0, 0);
            this.m_tlpPathtoCertificate.Name = "m_tlpPathtoCertificate";
            // 
            // m_tbPathToCertificate
            // 
            resources.ApplyResources(this.m_tbPathToCertificate, "m_tbPathToCertificate");
            this.m_tbPathToCertificate.Name = "m_tbPathToCertificate";
            // 
            // m_lbFileWithCertificate
            // 
            resources.ApplyResources(this.m_lbFileWithCertificate, "m_lbFileWithCertificate");
            this.m_lbFileWithCertificate.Name = "m_lbFileWithCertificate";
            // 
            // m_panelFtp
            // 
            resources.ApplyResources(this.m_panelFtp, "m_panelFtp");
            this.m_panelFtp.BackColor = System.Drawing.SystemColors.Control;
            this.m_panelFtp.Controls.Add(this.m_lbUsername);
            this.m_panelFtp.Controls.Add(this.m_chkAnonymous);
            this.m_panelFtp.Controls.Add(this.m_lbPassword);
            this.m_panelFtp.Controls.Add(this.m_tbPassword);
            this.m_panelFtp.Controls.Add(this.m_tbUsername);
            this.m_panelFtp.Name = "m_panelFtp";
            // 
            // m_lbUsername
            // 
            resources.ApplyResources(this.m_lbUsername, "m_lbUsername");
            this.m_lbUsername.Name = "m_lbUsername";
            // 
            // m_chkAnonymous
            // 
            resources.ApplyResources(this.m_chkAnonymous, "m_chkAnonymous");
            this.m_chkAnonymous.Name = "m_chkAnonymous";
            this.m_chkAnonymous.UseVisualStyleBackColor = true;
            this.m_chkAnonymous.CheckedChanged += new System.EventHandler(this.m_chkAnonymous_CheckedChanged);
            // 
            // m_lbPassword
            // 
            resources.ApplyResources(this.m_lbPassword, "m_lbPassword");
            this.m_lbPassword.Name = "m_lbPassword";
            // 
            // m_tbPassword
            // 
            resources.ApplyResources(this.m_tbPassword, "m_tbPassword");
            this.m_tbPassword.Name = "m_tbPassword";
            this.m_tbPassword.UseSystemPasswordChar = true;
            this.m_tbPassword.TextChanged += new System.EventHandler(this.m_tbPassword_TextChanged);
            // 
            // m_tbUsername
            // 
            resources.ApplyResources(this.m_tbUsername, "m_tbUsername");
            this.m_tbUsername.Name = "m_tbUsername";
            this.m_tbUsername.TextChanged += new System.EventHandler(this.m_tbUsername_TextChanged);
            // 
            // m_btnCheckConnection
            // 
            resources.ApplyResources(this.m_btnCheckConnection, "m_btnCheckConnection");
            this.m_btnCheckConnection.Name = "m_btnCheckConnection";
            this.m_tplAuthorization.SetRowSpan(this.m_btnCheckConnection, 2);
            this.m_btnCheckConnection.UseVisualStyleBackColor = true;
            this.m_btnCheckConnection.Click += new System.EventHandler(this.m_btnCheckConnection_Click);
            // 
            // m_tplProtocol
            // 
            resources.ApplyResources(this.m_tplProtocol, "m_tplProtocol");
            this.m_tplProtocol.BackColor = System.Drawing.SystemColors.Control;
            this.m_tplProtocol.Controls.Add(this.m_panelFtpEncryption, 1, 1);
            this.m_tplProtocol.Controls.Add(this.panel1, 0, 1);
            this.m_tplProtocol.Name = "m_tplProtocol";
            // 
            // m_panelFtpEncryption
            // 
            resources.ApplyResources(this.m_panelFtpEncryption, "m_panelFtpEncryption");
            this.m_panelFtpEncryption.BackColor = System.Drawing.SystemColors.Control;
            this.m_panelFtpEncryption.Controls.Add(this.m_lbFtpMode);
            this.m_panelFtpEncryption.Controls.Add(this.m_lbEncryption);
            this.m_panelFtpEncryption.Controls.Add(this.m_cbEncryption);
            this.m_panelFtpEncryption.Controls.Add(this.m_cmbMode);
            this.m_panelFtpEncryption.Name = "m_panelFtpEncryption";
            // 
            // m_lbFtpMode
            // 
            resources.ApplyResources(this.m_lbFtpMode, "m_lbFtpMode");
            this.m_lbFtpMode.Name = "m_lbFtpMode";
            // 
            // m_lbEncryption
            // 
            resources.ApplyResources(this.m_lbEncryption, "m_lbEncryption");
            this.m_lbEncryption.Name = "m_lbEncryption";
            // 
            // m_cbEncryption
            // 
            this.m_cbEncryption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbEncryption.FormattingEnabled = true;
            this.m_cbEncryption.Items.AddRange(new object[] {
            resources.GetString("m_cbEncryption.Items"),
            resources.GetString("m_cbEncryption.Items1"),
            resources.GetString("m_cbEncryption.Items2")});
            resources.ApplyResources(this.m_cbEncryption, "m_cbEncryption");
            this.m_cbEncryption.Name = "m_cbEncryption";
            this.m_cbEncryption.SelectedIndexChanged += new System.EventHandler(this.m_cbEncryption_SelectedIndexChanged);
            // 
            // m_cmbMode
            // 
            this.m_cmbMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.m_cmbMode, "m_cmbMode");
            this.m_cmbMode.Items.AddRange(new object[] {
            resources.GetString("m_cmbMode.Items"),
            resources.GetString("m_cmbMode.Items1")});
            this.m_cmbMode.Name = "m_cmbMode";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackColor = System.Drawing.SystemColors.Control;
            this.panel1.Controls.Add(this.m_tbPort);
            this.panel1.Controls.Add(this.m_lbPort);
            this.panel1.Controls.Add(this.m_cbProtocol);
            this.panel1.Controls.Add(this.m_lbProtocol);
            this.panel1.Controls.Add(this.m_lbServer);
            this.panel1.Controls.Add(this.m_tbServer);
            this.panel1.Name = "panel1";
            // 
            // m_tbPort
            // 
            resources.ApplyResources(this.m_tbPort, "m_tbPort");
            this.m_tbPort.Name = "m_tbPort";
            // 
            // m_lbPort
            // 
            resources.ApplyResources(this.m_lbPort, "m_lbPort");
            this.m_lbPort.Name = "m_lbPort";
            // 
            // m_cbProtocol
            // 
            this.m_cbProtocol.BackColor = System.Drawing.SystemColors.Window;
            this.m_cbProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbProtocol.FormattingEnabled = true;
            this.m_cbProtocol.Items.AddRange(new object[] {
            resources.GetString("m_cbProtocol.Items"),
            resources.GetString("m_cbProtocol.Items1"),
            resources.GetString("m_cbProtocol.Items2"),
            resources.GetString("m_cbProtocol.Items3"),
            resources.GetString("m_cbProtocol.Items4")});
            resources.ApplyResources(this.m_cbProtocol, "m_cbProtocol");
            this.m_cbProtocol.Name = "m_cbProtocol";
            this.m_cbProtocol.SelectedIndexChanged += new System.EventHandler(this.m_cbProtocol_SelectedIndexChanged);
            // 
            // m_lbProtocol
            // 
            resources.ApplyResources(this.m_lbProtocol, "m_lbProtocol");
            this.m_lbProtocol.Name = "m_lbProtocol";
            // 
            // m_lbServer
            // 
            resources.ApplyResources(this.m_lbServer, "m_lbServer");
            this.m_lbServer.Name = "m_lbServer";
            // 
            // m_tbServer
            // 
            resources.ApplyResources(this.m_tbServer, "m_tbServer");
            this.m_tbServer.Name = "m_tbServer";
            // 
            // m_gbSource
            // 
            resources.ApplyResources(this.m_gbSource, "m_gbSource");
            this.m_gbSource.BackColor = System.Drawing.SystemColors.Control;
            this.m_gbSource.Controls.Add(this.tableLayoutPanel4);
            this.m_gbSource.Name = "m_gbSource";
            this.m_gbSource.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.m_rbPrevOutput, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_rbDatFile, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // m_rbPrevOutput
            // 
            resources.ApplyResources(this.m_rbPrevOutput, "m_rbPrevOutput");
            this.m_rbPrevOutput.Name = "m_rbPrevOutput";
            this.m_rbPrevOutput.TabStop = true;
            this.m_rbPrevOutput.UseVisualStyleBackColor = true;
            // 
            // m_rbDatFile
            // 
            resources.ApplyResources(this.m_rbDatFile, "m_rbDatFile");
            this.m_rbDatFile.Name = "m_rbDatFile";
            this.m_rbDatFile.TabStop = true;
            this.m_rbDatFile.UseVisualStyleBackColor = true;
            // 
            // m_gbTarget
            // 
            this.m_gbTarget.Controls.Add(this.panelOut);
            resources.ApplyResources(this.m_gbTarget, "m_gbTarget");
            this.m_gbTarget.Name = "m_gbTarget";
            this.m_gbTarget.TabStop = false;
            // 
            // panelOut
            // 
            resources.ApplyResources(this.panelOut, "panelOut");
            this.panelOut.Name = "panelOut";
            // 
            // UploadTaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_gbTarget);
            this.Controls.Add(this.m_gbOption);
            this.Controls.Add(this.m_gbProtocol);
            this.Controls.Add(this.m_gbSource);
            this.Name = "UploadTaskControl";
            this.m_gbOption.ResumeLayout(false);
            this.m_gbOption.PerformLayout();
            this.m_tplOptions.ResumeLayout(false);
            this.m_tplOptions.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.m_gbProtocol.ResumeLayout(false);
            this.m_gbProtocol.PerformLayout();
            this.m_tplAuthorization.ResumeLayout(false);
            this.m_tplAuthorization.PerformLayout();
            this.m_panelChkBoxControls.ResumeLayout(false);
            this.m_panelChkBoxControls.PerformLayout();
            this.m_panelFilePath.ResumeLayout(false);
            this.m_panelFilePath.PerformLayout();
            this.m_tlpPathToKey.ResumeLayout(false);
            this.m_tlpPathToKey.PerformLayout();
            this.m_tlpPathtoCertificate.ResumeLayout(false);
            this.m_tlpPathtoCertificate.PerformLayout();
            this.m_panelFtp.ResumeLayout(false);
            this.m_panelFtp.PerformLayout();
            this.m_tplProtocol.ResumeLayout(false);
            this.m_tplProtocol.PerformLayout();
            this.m_panelFtpEncryption.ResumeLayout(false);
            this.m_panelFtpEncryption.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.m_gbSource.ResumeLayout(false);
            this.m_gbSource.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.m_gbTarget.ResumeLayout(false);
            this.m_gbTarget.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Utility.CollapsibleGroupBox m_gbProtocol;
        private Utility.CollapsibleGroupBox m_gbSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton m_rbPrevOutput;
        private System.Windows.Forms.RadioButton m_rbDatFile;
        private Utility.CollapsibleGroupBox m_gbOption;
        private System.Windows.Forms.Label m_lbServer;
        private System.Windows.Forms.TextBox m_tbServer;
        private System.Windows.Forms.Label m_lbProtocol;
        private System.Windows.Forms.ComboBox m_cbProtocol;
        private System.Windows.Forms.Label m_lbEncryption;
        private System.Windows.Forms.ComboBox m_cmbMode;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ComboBox m_cbEncryption;
        private System.Windows.Forms.TableLayoutPanel m_tplProtocol;
        private System.Windows.Forms.Panel m_panelFtpEncryption;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel m_tplAuthorization;
        private System.Windows.Forms.Panel m_panelFtp;
        private System.Windows.Forms.Label m_lbUsername;
        private System.Windows.Forms.CheckBox m_chkAnonymous;
        private System.Windows.Forms.Label m_lbPassword;
        private System.Windows.Forms.TextBox m_tbPassword;
        private System.Windows.Forms.TextBox m_tbUsername;
        private System.Windows.Forms.TableLayoutPanel m_tplOptions;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label m_lbFtpMode;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label m_lblRemotePath;
        private System.Windows.Forms.TextBox m_tbRemotePath;
        private System.Windows.Forms.Button m_btnCheckConnection;
        private System.Windows.Forms.TextBox m_tbPort;
        private System.Windows.Forms.Label m_lbPort;
        private Utility.CollapsibleGroupBox m_gbTarget;
        private System.Windows.Forms.Panel panelOut;
        private System.Windows.Forms.Panel m_panelFilePath;
        private System.Windows.Forms.TableLayoutPanel m_tlpPathToKey;
        private System.Windows.Forms.TextBox m_tbPathToPrivateKey;
        private System.Windows.Forms.Label m_lbFileToPrivateKey;
        private System.Windows.Forms.TableLayoutPanel m_tlpPathtoCertificate;
        private System.Windows.Forms.TextBox m_tbPathToCertificate;
        private System.Windows.Forms.Label m_lbFileWithCertificate;
        private System.Windows.Forms.Panel m_panelChkBoxControls;
        private System.Windows.Forms.CheckBox m_chkAcceptAnySshHostKey;
        private System.Windows.Forms.CheckBox m_chkAcceptAnyTlsCertificate;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.CheckBox m_createZipArchive;
    }
}
