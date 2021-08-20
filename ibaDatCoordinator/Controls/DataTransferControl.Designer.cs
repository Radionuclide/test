namespace iba.Controls
{
    partial class DataTransferControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataTransferControl));
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.dgvClients = new System.Windows.Forms.DataGridView();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.buttonClearClients = new System.Windows.Forms.Button();
            this.lblConnectedClients = new System.Windows.Forms.Label();
            this.m_cbEnabled = new System.Windows.Forms.CheckBox();
            this.buttonConfigurationReset = new System.Windows.Forms.Button();
            this.buttonConfigurationApply = new System.Windows.Forms.Button();
            this.lbsPort = new System.Windows.Forms.Label();
            this.lblRootPath = new System.Windows.Forms.Label();
            this.m_numPort = new System.Windows.Forms.NumericUpDown();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.tabControl1 = new Crownwood.DotNetMagic.Controls.TabControl();
            this.tabConfiguration = new Crownwood.DotNetMagic.Controls.TabPage();
            this.gbSecurity = new System.Windows.Forms.GroupBox();
            this.btnCertificatePath = new System.Windows.Forms.Button();
            this.tbCertificatePath = new System.Windows.Forms.TextBox();
            this.lblCertificatePath = new System.Windows.Forms.Label();
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.gbDirectory = new System.Windows.Forms.GroupBox();
            this.btnRootPath = new System.Windows.Forms.Button();
            this.tbRootPath = new System.Windows.Forms.TextBox();
            this.tabDiag = new Crownwood.DotNetMagic.Controls.TabPage();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numPort)).BeginInit();
            this.panelFooter.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabConfiguration.SuspendLayout();
            this.gbSecurity.SuspendLayout();
            this.gbGeneral.SuspendLayout();
            this.gbDirectory.SuspendLayout();
            this.tabDiag.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            this.dgvClients.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvClients, "dgvClients");
            this.dgvClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.ReadOnly = true;
            this.dgvClients.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dgvClients.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgvClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClients.StandardTab = true;
            // 
            // tbStatus
            // 
            resources.ApplyResources(this.tbStatus, "tbStatus");
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.TabStop = false;
            // 
            // buttonClearClients
            // 
            resources.ApplyResources(this.buttonClearClients, "buttonClearClients");
            this.buttonClearClients.Name = "buttonClearClients";
            this.buttonClearClients.UseVisualStyleBackColor = true;
            this.buttonClearClients.Click += new System.EventHandler(this.buttonClearClients_Click);
            // 
            // lblConnectedClients
            // 
            resources.ApplyResources(this.lblConnectedClients, "lblConnectedClients");
            this.lblConnectedClients.Name = "lblConnectedClients";
            // 
            // m_cbEnabled
            // 
            resources.ApplyResources(this.m_cbEnabled, "m_cbEnabled");
            this.m_cbEnabled.Checked = true;
            this.m_cbEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_cbEnabled.Name = "m_cbEnabled";
            this.m_cbEnabled.UseVisualStyleBackColor = true;
            // 
            // buttonConfigurationReset
            // 
            resources.ApplyResources(this.buttonConfigurationReset, "buttonConfigurationReset");
            this.buttonConfigurationReset.Name = "buttonConfigurationReset";
            this.buttonConfigurationReset.UseVisualStyleBackColor = true;
            this.buttonConfigurationReset.Click += new System.EventHandler(this.buttonConfigurationReset_Click);
            // 
            // buttonConfigurationApply
            // 
            resources.ApplyResources(this.buttonConfigurationApply, "buttonConfigurationApply");
            this.buttonConfigurationApply.Name = "buttonConfigurationApply";
            this.buttonConfigurationApply.UseVisualStyleBackColor = true;
            this.buttonConfigurationApply.Click += new System.EventHandler(this.buttonConfigurationApply_Click);
            // 
            // lbsPort
            // 
            resources.ApplyResources(this.lbsPort, "lbsPort");
            this.lbsPort.Name = "lbsPort";
            // 
            // lblRootPath
            // 
            resources.ApplyResources(this.lblRootPath, "lblRootPath");
            this.lblRootPath.Name = "lblRootPath";
            // 
            // m_numPort
            // 
            resources.ApplyResources(this.m_numPort, "m_numPort");
            this.m_numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.m_numPort.Name = "m_numPort";
            // 
            // panelFooter
            // 
            this.panelFooter.Controls.Add(this.buttonConfigurationReset);
            this.panelFooter.Controls.Add(this.buttonConfigurationApply);
            resources.ApplyResources(this.panelFooter, "panelFooter");
            this.panelFooter.Name = "panelFooter";
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDragReorder = false;
            this.tabControl1.Appearance = Crownwood.DotNetMagic.Controls.VisualAppearance.MultiDocument;
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.MediaPlayerDockSides = false;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.OfficeDockSides = false;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowArrows = false;
            this.tabControl1.ShowClose = false;
            this.tabControl1.ShowDropSelect = false;
            this.tabControl1.Style = Crownwood.DotNetMagic.Common.VisualStyle.IDE2005;
            this.tabControl1.TabPages.AddRange(new Crownwood.DotNetMagic.Controls.TabPage[] {
            this.tabConfiguration,
            this.tabDiag});
            this.tabControl1.TextTips = true;
            // 
            // tabConfiguration
            // 
            resources.ApplyResources(this.tabConfiguration, "tabConfiguration");
            this.tabConfiguration.Controls.Add(this.gbSecurity);
            this.tabConfiguration.Controls.Add(this.gbGeneral);
            this.tabConfiguration.Controls.Add(this.gbDirectory);
            this.tabConfiguration.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabConfiguration.Name = "tabConfiguration";
            this.tabConfiguration.SelectBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.SelectTextColor = System.Drawing.Color.Empty;
            // 
            // gbSecurity
            // 
            resources.ApplyResources(this.gbSecurity, "gbSecurity");
            this.gbSecurity.Controls.Add(this.btnCertificatePath);
            this.gbSecurity.Controls.Add(this.tbCertificatePath);
            this.gbSecurity.Controls.Add(this.lblCertificatePath);
            this.gbSecurity.Name = "gbSecurity";
            this.gbSecurity.TabStop = false;
            // 
            // btnCertificatePath
            // 
            resources.ApplyResources(this.btnCertificatePath, "btnCertificatePath");
            this.btnCertificatePath.Name = "btnCertificatePath";
            this.btnCertificatePath.UseVisualStyleBackColor = true;
            this.btnCertificatePath.Click += new System.EventHandler(this.btnRootPathOrBtnCertificatePath_Click);
            // 
            // tbCertificatePath
            // 
            resources.ApplyResources(this.tbCertificatePath, "tbCertificatePath");
            this.tbCertificatePath.Name = "tbCertificatePath";
            // 
            // lblCertificatePath
            // 
            resources.ApplyResources(this.lblCertificatePath, "lblCertificatePath");
            this.lblCertificatePath.Name = "lblCertificatePath";
            // 
            // gbGeneral
            // 
            resources.ApplyResources(this.gbGeneral, "gbGeneral");
            this.gbGeneral.Controls.Add(this.m_cbEnabled);
            this.gbGeneral.Controls.Add(this.lbsPort);
            this.gbGeneral.Controls.Add(this.m_numPort);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.TabStop = false;
            // 
            // gbDirectory
            // 
            resources.ApplyResources(this.gbDirectory, "gbDirectory");
            this.gbDirectory.Controls.Add(this.btnRootPath);
            this.gbDirectory.Controls.Add(this.tbRootPath);
            this.gbDirectory.Controls.Add(this.lblRootPath);
            this.gbDirectory.Name = "gbDirectory";
            this.gbDirectory.TabStop = false;
            // 
            // btnRootPath
            // 
            resources.ApplyResources(this.btnRootPath, "btnRootPath");
            this.btnRootPath.Name = "btnRootPath";
            this.btnRootPath.UseVisualStyleBackColor = true;
            this.btnRootPath.Click += new System.EventHandler(this.btnRootPathOrBtnCertificatePath_Click);
            // 
            // tbRootPath
            // 
            resources.ApplyResources(this.tbRootPath, "tbRootPath");
            this.tbRootPath.Name = "tbRootPath";
            // 
            // tabDiag
            // 
            resources.ApplyResources(this.tabDiag, "tabDiag");
            this.tabDiag.Controls.Add(this.lblConnectedClients);
            this.tabDiag.Controls.Add(this.dgvClients);
            this.tabDiag.Controls.Add(this.buttonClearClients);
            this.tabDiag.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabDiag.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabDiag.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabDiag.Name = "tabDiag";
            this.tabDiag.SelectBackColor = System.Drawing.Color.Empty;
            this.tabDiag.Selected = false;
            this.tabDiag.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabDiag.SelectTextColor = System.Drawing.Color.Empty;
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.tbStatus);
            resources.ApplyResources(this.gbStatus, "gbStatus");
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.TabStop = false;
            // 
            // DataTransferControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.gbStatus);
            this.Name = "DataTransferControl";
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_numPort)).EndInit();
            this.panelFooter.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabConfiguration.ResumeLayout(false);
            this.gbSecurity.ResumeLayout(false);
            this.gbSecurity.PerformLayout();
            this.gbGeneral.ResumeLayout(false);
            this.gbGeneral.PerformLayout();
            this.gbDirectory.ResumeLayout(false);
            this.gbDirectory.PerformLayout();
            this.tabDiag.ResumeLayout(false);
            this.tabDiag.PerformLayout();
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox m_cbEnabled;
        private System.Windows.Forms.Button buttonConfigurationApply;
        private System.Windows.Forms.Label lbsPort;
        private System.Windows.Forms.Label lblRootPath;
        private System.Windows.Forms.NumericUpDown m_numPort;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Button buttonClearClients;
        private System.Windows.Forms.Label lblConnectedClients;
        private System.Windows.Forms.Button buttonConfigurationReset;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.Panel panelFooter;
        private Crownwood.DotNetMagic.Controls.TabControl tabControl1;
        private Crownwood.DotNetMagic.Controls.TabPage tabConfiguration;
        private Crownwood.DotNetMagic.Controls.TabPage tabDiag;
        private System.Windows.Forms.GroupBox gbGeneral;
        private System.Windows.Forms.GroupBox gbDirectory;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.GroupBox gbSecurity;
        private System.Windows.Forms.TextBox tbCertificatePath;
        private System.Windows.Forms.Label lblCertificatePath;
        private System.Windows.Forms.Button btnRootPath;
        private System.Windows.Forms.TextBox tbRootPath;
        private System.Windows.Forms.Button btnCertificatePath;
    }
}
