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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataTransferControl));
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.timerRefreshStatus = new System.Windows.Forms.Timer(this.components);
            this.dgvClients = new System.Windows.Forms.DataGridView();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.buttonClearClients = new System.Windows.Forms.Button();
            this.lblConnectedClients = new System.Windows.Forms.Label();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.buttonConfigurationReset = new System.Windows.Forms.Button();
            this.buttonConfigurationApply = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tbCommunity = new System.Windows.Forms.TextBox();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.tabControl1 = new Crownwood.DotNetMagic.Controls.TabControl();
            this.tabConfiguration = new Crownwood.DotNetMagic.Controls.TabPage();
            this.gbSecurity = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.lblCertificate = new System.Windows.Forms.Label();
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.gbDirectory = new System.Windows.Forms.GroupBox();
            this.tabDiag = new Crownwood.DotNetMagic.Controls.TabPage();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            this.ColAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColDestinationPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColCurrentBandwidth = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColClientVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
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
            // timerRefreshStatus
            // 
            this.timerRefreshStatus.Interval = 1000;
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            this.dgvClients.AllowUserToResizeRows = false;
            resources.ApplyResources(this.dgvClients, "dgvClients");
            this.dgvClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColAddress,
            this.ColDestinationPath,
            this.ColCurrentBandwidth,
            this.ColClientVersion});
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
            // 
            // lblConnectedClients
            // 
            resources.ApplyResources(this.lblConnectedClients, "lblConnectedClients");
            this.lblConnectedClients.Name = "lblConnectedClients";
            // 
            // cbEnabled
            // 
            resources.ApplyResources(this.cbEnabled, "cbEnabled");
            this.cbEnabled.Checked = true;
            this.cbEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // buttonConfigurationReset
            // 
            resources.ApplyResources(this.buttonConfigurationReset, "buttonConfigurationReset");
            this.buttonConfigurationReset.Name = "buttonConfigurationReset";
            this.buttonConfigurationReset.UseVisualStyleBackColor = true;
            // 
            // buttonConfigurationApply
            // 
            resources.ApplyResources(this.buttonConfigurationApply, "buttonConfigurationApply");
            this.buttonConfigurationApply.Name = "buttonConfigurationApply";
            this.buttonConfigurationApply.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // tbCommunity
            // 
            resources.ApplyResources(this.tbCommunity, "tbCommunity");
            this.tbCommunity.Name = "tbCommunity";
            // 
            // numPort
            // 
            resources.ApplyResources(this.numPort, "numPort");
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
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
            this.gbSecurity.Controls.Add(this.textBox1);
            this.gbSecurity.Controls.Add(this.lblCertificate);
            this.gbSecurity.Name = "gbSecurity";
            this.gbSecurity.TabStop = false;
            // 
            // textBox1
            // 
            resources.ApplyResources(this.textBox1, "textBox1");
            this.textBox1.Name = "textBox1";
            // 
            // lblCertificate
            // 
            resources.ApplyResources(this.lblCertificate, "lblCertificate");
            this.lblCertificate.Name = "lblCertificate";
            // 
            // gbGeneral
            // 
            resources.ApplyResources(this.gbGeneral, "gbGeneral");
            this.gbGeneral.Controls.Add(this.cbEnabled);
            this.gbGeneral.Controls.Add(this.label5);
            this.gbGeneral.Controls.Add(this.numPort);
            this.gbGeneral.Name = "gbGeneral";
            this.gbGeneral.TabStop = false;
            // 
            // gbDirectory
            // 
            resources.ApplyResources(this.gbDirectory, "gbDirectory");
            this.gbDirectory.Controls.Add(this.tbCommunity);
            this.gbDirectory.Controls.Add(this.label7);
            this.gbDirectory.Name = "gbDirectory";
            this.gbDirectory.TabStop = false;
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
            // ColAddress
            // 
            this.ColAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColAddress, "ColAddress");
            this.ColAddress.Name = "ColAddress";
            this.ColAddress.ReadOnly = true;
            // 
            // ColDestinationPath
            // 
            this.ColDestinationPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColDestinationPath, "ColDestinationPath");
            this.ColDestinationPath.Name = "ColDestinationPath";
            this.ColDestinationPath.ReadOnly = true;
            // 
            // ColCurrentBandwidth
            // 
            this.ColCurrentBandwidth.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColCurrentBandwidth, "ColCurrentBandwidth");
            this.ColCurrentBandwidth.Name = "ColCurrentBandwidth";
            this.ColCurrentBandwidth.ReadOnly = true;
            // 
            // ColClientVersion
            // 
            this.ColClientVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColClientVersion, "ColClientVersion");
            this.ColClientVersion.Name = "ColClientVersion";
            this.ColClientVersion.ReadOnly = true;
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
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
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
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.Button buttonConfigurationApply;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbCommunity;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Button buttonClearClients;
        private System.Windows.Forms.Label lblConnectedClients;
        private System.Windows.Forms.Button buttonConfigurationReset;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Timer timerRefreshStatus;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.Panel panelFooter;
        private Crownwood.DotNetMagic.Controls.TabControl tabControl1;
        private Crownwood.DotNetMagic.Controls.TabPage tabConfiguration;
        private Crownwood.DotNetMagic.Controls.TabPage tabDiag;
        private System.Windows.Forms.GroupBox gbGeneral;
        private System.Windows.Forms.GroupBox gbDirectory;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.GroupBox gbSecurity;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label lblCertificate;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColDestinationPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColCurrentBandwidth;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColClientVersion;
    }
}
