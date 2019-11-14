namespace iba.Controls
{
    partial class SnmpControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SnmpControl));
            this.splitContainerObjectsFooter = new System.Windows.Forms.SplitContainer();
            this.tbObjOid = new System.Windows.Forms.TextBox();
            this.labelObjType = new System.Windows.Forms.Label();
            this.labelObjOid = new System.Windows.Forms.Label();
            this.tbObjType = new System.Windows.Forms.TextBox();
            this.labelObjValue = new System.Windows.Forms.Label();
            this.tbObjValue = new System.Windows.Forms.TextBox();
            this.LabObjMibDescription = new System.Windows.Forms.Label();
            this.labelObjMibName = new System.Windows.Forms.Label();
            this.tbObjMibDescription = new System.Windows.Forms.TextBox();
            this.tbObjMibName = new System.Windows.Forms.TextBox();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.timerRefreshStatus = new System.Windows.Forms.Timer(this.components);
            this.dgvClients = new System.Windows.Forms.DataGridView();
            this.ColAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProtocol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMsgCounter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLastMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.buttonClearClients = new System.Windows.Forms.Button();
            this.label15 = new System.Windows.Forms.Label();
            this.buttonCreateMibFiles = new System.Windows.Forms.Button();
            this.tvObjects = new System.Windows.Forms.TreeView();
            this.rbDateTimeStr = new System.Windows.Forms.RadioButton();
            this.rbDateTimeTc = new System.Windows.Forms.RadioButton();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.buttonConfigurationReset = new System.Windows.Forms.Button();
            this.buttonConfigurationApply = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonShowPassword = new System.Windows.Forms.Button();
            this.buttonShowEncryptionKey = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbEncryption = new System.Windows.Forms.ComboBox();
            this.cmbAuthentication = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbEncryptionKey = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbCommunity = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.tabControl1 = new Crownwood.DotNetMagic.Controls.TabControl();
            this.tabConfiguration = new Crownwood.DotNetMagic.Controls.TabPage();
            this.gbGeneral = new System.Windows.Forms.GroupBox();
            this.gbV2Security = new System.Windows.Forms.GroupBox();
            this.gbV3Security = new System.Windows.Forms.GroupBox();
            this.gbDateFormat = new System.Windows.Forms.GroupBox();
            this.tabObjects = new Crownwood.DotNetMagic.Controls.TabPage();
            this.tabDiag = new Crownwood.DotNetMagic.Controls.TabPage();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).BeginInit();
            this.splitContainerObjectsFooter.Panel1.SuspendLayout();
            this.splitContainerObjectsFooter.Panel2.SuspendLayout();
            this.splitContainerObjectsFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.panelFooter.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabConfiguration.SuspendLayout();
            this.gbGeneral.SuspendLayout();
            this.gbV2Security.SuspendLayout();
            this.gbV3Security.SuspendLayout();
            this.gbDateFormat.SuspendLayout();
            this.tabObjects.SuspendLayout();
            this.tabDiag.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerObjectsFooter
            // 
            resources.ApplyResources(this.splitContainerObjectsFooter, "splitContainerObjectsFooter");
            this.splitContainerObjectsFooter.Name = "splitContainerObjectsFooter";
            // 
            // splitContainerObjectsFooter.Panel1
            // 
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjOid);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjType);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjOid);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjType);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjValue);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjValue);
            resources.ApplyResources(this.splitContainerObjectsFooter.Panel1, "splitContainerObjectsFooter.Panel1");
            // 
            // splitContainerObjectsFooter.Panel2
            // 
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.LabObjMibDescription);
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.labelObjMibName);
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.tbObjMibDescription);
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.tbObjMibName);
            // 
            // tbObjOid
            // 
            resources.ApplyResources(this.tbObjOid, "tbObjOid");
            this.tbObjOid.Name = "tbObjOid";
            this.tbObjOid.ReadOnly = true;
            this.tbObjOid.TabStop = false;
            // 
            // labelObjType
            // 
            resources.ApplyResources(this.labelObjType, "labelObjType");
            this.labelObjType.Name = "labelObjType";
            // 
            // labelObjOid
            // 
            resources.ApplyResources(this.labelObjOid, "labelObjOid");
            this.labelObjOid.Name = "labelObjOid";
            // 
            // tbObjType
            // 
            resources.ApplyResources(this.tbObjType, "tbObjType");
            this.tbObjType.Name = "tbObjType";
            this.tbObjType.ReadOnly = true;
            this.tbObjType.TabStop = false;
            // 
            // labelObjValue
            // 
            resources.ApplyResources(this.labelObjValue, "labelObjValue");
            this.labelObjValue.Name = "labelObjValue";
            // 
            // tbObjValue
            // 
            resources.ApplyResources(this.tbObjValue, "tbObjValue");
            this.tbObjValue.Name = "tbObjValue";
            this.tbObjValue.ReadOnly = true;
            this.tbObjValue.TabStop = false;
            // 
            // LabObjMibDescription
            // 
            resources.ApplyResources(this.LabObjMibDescription, "LabObjMibDescription");
            this.LabObjMibDescription.Name = "LabObjMibDescription";
            // 
            // labelObjMibName
            // 
            resources.ApplyResources(this.labelObjMibName, "labelObjMibName");
            this.labelObjMibName.Name = "labelObjMibName";
            // 
            // tbObjMibDescription
            // 
            resources.ApplyResources(this.tbObjMibDescription, "tbObjMibDescription");
            this.tbObjMibDescription.Name = "tbObjMibDescription";
            this.tbObjMibDescription.ReadOnly = true;
            this.tbObjMibDescription.TabStop = false;
            // 
            // tbObjMibName
            // 
            resources.ApplyResources(this.tbObjMibName, "tbObjMibName");
            this.tbObjMibName.Name = "tbObjMibName";
            this.tbObjMibName.ReadOnly = true;
            this.tbObjMibName.TabStop = false;
            // 
            // timerRefreshStatus
            // 
            this.timerRefreshStatus.Interval = 1000;
            this.timerRefreshStatus.Tick += new System.EventHandler(this.timerRefreshStatus_Tick);
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
            this.ColProtocol,
            this.ColMsgCounter,
            this.ColLastMsg});
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.ReadOnly = true;
            this.dgvClients.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dgvClients.RowTemplate.DefaultCellStyle.BackColor = System.Drawing.SystemColors.Window;
            this.dgvClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClients.StandardTab = true;
            // 
            // ColAddress
            // 
            this.ColAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColAddress, "ColAddress");
            this.ColAddress.Name = "ColAddress";
            this.ColAddress.ReadOnly = true;
            // 
            // ColProtocol
            // 
            this.ColProtocol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColProtocol, "ColProtocol");
            this.ColProtocol.Name = "ColProtocol";
            this.ColProtocol.ReadOnly = true;
            // 
            // ColMsgCounter
            // 
            this.ColMsgCounter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColMsgCounter, "ColMsgCounter");
            this.ColMsgCounter.Name = "ColMsgCounter";
            this.ColMsgCounter.ReadOnly = true;
            // 
            // ColLastMsg
            // 
            this.ColLastMsg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ColLastMsg, "ColLastMsg");
            this.ColLastMsg.Name = "ColLastMsg";
            this.ColLastMsg.ReadOnly = true;
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
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // buttonCreateMibFiles
            // 
            resources.ApplyResources(this.buttonCreateMibFiles, "buttonCreateMibFiles");
            this.buttonCreateMibFiles.Name = "buttonCreateMibFiles";
            this.buttonCreateMibFiles.UseVisualStyleBackColor = true;
            this.buttonCreateMibFiles.Click += new System.EventHandler(this.buttonCreateMibFiles_Click);
            // 
            // tvObjects
            // 
            resources.ApplyResources(this.tvObjects, "tvObjects");
            this.tvObjects.HideSelection = false;
            this.tvObjects.Name = "tvObjects";
            this.tvObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjects_AfterSelect);
            // 
            // rbDateTimeStr
            // 
            resources.ApplyResources(this.rbDateTimeStr, "rbDateTimeStr");
            this.rbDateTimeStr.Name = "rbDateTimeStr";
            this.rbDateTimeStr.UseVisualStyleBackColor = true;
            // 
            // rbDateTimeTc
            // 
            resources.ApplyResources(this.rbDateTimeTc, "rbDateTimeTc");
            this.rbDateTimeTc.Checked = true;
            this.rbDateTimeTc.Name = "rbDateTimeTc";
            this.rbDateTimeTc.TabStop = true;
            this.rbDateTimeTc.UseVisualStyleBackColor = true;
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
            this.buttonConfigurationReset.Click += new System.EventHandler(this.buttonConfigurationReset_Click);
            // 
            // buttonConfigurationApply
            // 
            resources.ApplyResources(this.buttonConfigurationApply, "buttonConfigurationApply");
            this.buttonConfigurationApply.Name = "buttonConfigurationApply";
            this.buttonConfigurationApply.UseVisualStyleBackColor = true;
            this.buttonConfigurationApply.Click += new System.EventHandler(this.buttonConfigurationApply_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // buttonShowPassword
            // 
            resources.ApplyResources(this.buttonShowPassword, "buttonShowPassword");
            this.buttonShowPassword.Image = global::iba.Properties.Resources.Eye;
            this.buttonShowPassword.Name = "buttonShowPassword";
            this.buttonShowPassword.TabStop = false;
            this.buttonShowPassword.UseVisualStyleBackColor = true;
            this.buttonShowPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.handler_ShowPassword);
            this.buttonShowPassword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.handler_HidePassword);
            // 
            // buttonShowEncryptionKey
            // 
            resources.ApplyResources(this.buttonShowEncryptionKey, "buttonShowEncryptionKey");
            this.buttonShowEncryptionKey.Image = global::iba.Properties.Resources.Eye;
            this.buttonShowEncryptionKey.Name = "buttonShowEncryptionKey";
            this.buttonShowEncryptionKey.TabStop = false;
            this.buttonShowEncryptionKey.UseVisualStyleBackColor = true;
            this.buttonShowEncryptionKey.MouseDown += new System.Windows.Forms.MouseEventHandler(this.handler_ShowPassword);
            this.buttonShowEncryptionKey.MouseUp += new System.Windows.Forms.MouseEventHandler(this.handler_HidePassword);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // cmbEncryption
            // 
            this.cmbEncryption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncryption.Items.AddRange(new object[] {
            resources.GetString("cmbEncryption.Items"),
            resources.GetString("cmbEncryption.Items1"),
            resources.GetString("cmbEncryption.Items2")});
            resources.ApplyResources(this.cmbEncryption, "cmbEncryption");
            this.cmbEncryption.Name = "cmbEncryption";
            // 
            // cmbAuthentication
            // 
            this.cmbAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAuthentication.Items.AddRange(new object[] {
            resources.GetString("cmbAuthentication.Items"),
            resources.GetString("cmbAuthentication.Items1")});
            resources.ApplyResources(this.cmbAuthentication, "cmbAuthentication");
            this.cmbAuthentication.Name = "cmbAuthentication";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // tbPassword
            // 
            resources.ApplyResources(this.tbPassword, "tbPassword");
            this.tbPassword.Name = "tbPassword";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // tbEncryptionKey
            // 
            resources.ApplyResources(this.tbEncryptionKey, "tbEncryptionKey");
            this.tbEncryptionKey.Name = "tbEncryptionKey";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // tbUserName
            // 
            resources.ApplyResources(this.tbUserName, "tbUserName");
            this.tbUserName.Name = "tbUserName";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // tbCommunity
            // 
            resources.ApplyResources(this.tbCommunity, "tbCommunity");
            this.tbCommunity.Name = "tbCommunity";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
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
            this.tabObjects,
            this.tabDiag});
            this.tabControl1.TextTips = true;
            this.tabControl1.SelectionChanged += new Crownwood.DotNetMagic.Controls.SelectTabHandler(this.tabControl1_SelectionChanged);
            // 
            // tabConfiguration
            // 
            resources.ApplyResources(this.tabConfiguration, "tabConfiguration");
            this.tabConfiguration.Controls.Add(this.gbGeneral);
            this.tabConfiguration.Controls.Add(this.gbV2Security);
            this.tabConfiguration.Controls.Add(this.gbV3Security);
            this.tabConfiguration.Controls.Add(this.gbDateFormat);
            this.tabConfiguration.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabConfiguration.Name = "tabConfiguration";
            this.tabConfiguration.SelectBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.SelectTextColor = System.Drawing.Color.Empty;
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
            // gbV2Security
            // 
            resources.ApplyResources(this.gbV2Security, "gbV2Security");
            this.gbV2Security.Controls.Add(this.tbCommunity);
            this.gbV2Security.Controls.Add(this.label7);
            this.gbV2Security.Name = "gbV2Security";
            this.gbV2Security.TabStop = false;
            // 
            // gbV3Security
            // 
            resources.ApplyResources(this.gbV3Security, "gbV3Security");
            this.gbV3Security.Controls.Add(this.cmbAuthentication);
            this.gbV3Security.Controls.Add(this.tbUserName);
            this.gbV3Security.Controls.Add(this.buttonShowPassword);
            this.gbV3Security.Controls.Add(this.buttonShowEncryptionKey);
            this.gbV3Security.Controls.Add(this.label12);
            this.gbV3Security.Controls.Add(this.tbPassword);
            this.gbV3Security.Controls.Add(this.label11);
            this.gbV3Security.Controls.Add(this.tbEncryptionKey);
            this.gbV3Security.Controls.Add(this.label9);
            this.gbV3Security.Controls.Add(this.cmbEncryption);
            this.gbV3Security.Controls.Add(this.label13);
            this.gbV3Security.Controls.Add(this.label10);
            this.gbV3Security.Name = "gbV3Security";
            this.gbV3Security.TabStop = false;
            // 
            // gbDateFormat
            // 
            resources.ApplyResources(this.gbDateFormat, "gbDateFormat");
            this.gbDateFormat.Controls.Add(this.rbDateTimeTc);
            this.gbDateFormat.Controls.Add(this.rbDateTimeStr);
            this.gbDateFormat.Name = "gbDateFormat";
            this.gbDateFormat.TabStop = false;
            // 
            // tabObjects
            // 
            resources.ApplyResources(this.tabObjects, "tabObjects");
            this.tabObjects.Controls.Add(this.splitContainerObjectsFooter);
            this.tabObjects.Controls.Add(this.buttonCreateMibFiles);
            this.tabObjects.Controls.Add(this.tvObjects);
            this.tabObjects.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabObjects.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabObjects.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabObjects.Name = "tabObjects";
            this.tabObjects.SelectBackColor = System.Drawing.Color.Empty;
            this.tabObjects.Selected = false;
            this.tabObjects.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabObjects.SelectTextColor = System.Drawing.Color.Empty;
            // 
            // tabDiag
            // 
            resources.ApplyResources(this.tabDiag, "tabDiag");
            this.tabDiag.Controls.Add(this.label15);
            this.tabDiag.Controls.Add(this.dgvClients);
            this.tabDiag.Controls.Add(this.buttonClearClients);
            this.tabDiag.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabDiag.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabDiag.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabDiag.Name = "tabDiag";
            this.tabDiag.SelectBackColor = System.Drawing.Color.Empty;
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
            // SnmpControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.gbStatus);
            this.Name = "SnmpControl";
            this.Load += new System.EventHandler(this.SnmpControl_Load);
            this.splitContainerObjectsFooter.Panel1.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel1.PerformLayout();
            this.splitContainerObjectsFooter.Panel2.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).EndInit();
            this.splitContainerObjectsFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.panelFooter.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabConfiguration.ResumeLayout(false);
            this.gbGeneral.ResumeLayout(false);
            this.gbGeneral.PerformLayout();
            this.gbV2Security.ResumeLayout(false);
            this.gbV2Security.PerformLayout();
            this.gbV3Security.ResumeLayout(false);
            this.gbV3Security.PerformLayout();
            this.gbDateFormat.ResumeLayout(false);
            this.gbDateFormat.PerformLayout();
            this.tabObjects.ResumeLayout(false);
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
        private System.Windows.Forms.Button buttonShowPassword;
        private System.Windows.Forms.Button buttonShowEncryptionKey;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbEncryption;
        private System.Windows.Forms.ComboBox cmbAuthentication;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbEncryptionKey;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox tbCommunity;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Button buttonClearClients;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button buttonCreateMibFiles;
        private System.Windows.Forms.TreeView tvObjects;
        private System.Windows.Forms.Button buttonConfigurationReset;
        private System.Windows.Forms.Label labelObjValue;
        private System.Windows.Forms.Label labelObjOid;
        private System.Windows.Forms.TextBox tbObjOid;
        private System.Windows.Forms.TextBox tbObjValue;
        private System.Windows.Forms.SplitContainer splitContainerObjectsFooter;
        private System.Windows.Forms.Label labelObjType;
        private System.Windows.Forms.Label labelObjMibName;
        private System.Windows.Forms.TextBox tbObjMibName;
        private System.Windows.Forms.TextBox tbObjType;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Timer timerRefreshStatus;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProtocol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMsgCounter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLastMsg;
        private System.Windows.Forms.RadioButton rbDateTimeStr;
        private System.Windows.Forms.RadioButton rbDateTimeTc;
        private System.Windows.Forms.Label LabObjMibDescription;
        private System.Windows.Forms.TextBox tbObjMibDescription;
        private System.Windows.Forms.Panel panelFooter;
        private Crownwood.DotNetMagic.Controls.TabControl tabControl1;
        private Crownwood.DotNetMagic.Controls.TabPage tabConfiguration;
        private Crownwood.DotNetMagic.Controls.TabPage tabObjects;
        private Crownwood.DotNetMagic.Controls.TabPage tabDiag;
        private System.Windows.Forms.GroupBox gbGeneral;
        private System.Windows.Forms.GroupBox gbV2Security;
        private System.Windows.Forms.GroupBox gbV3Security;
        private System.Windows.Forms.GroupBox gbDateFormat;
        private System.Windows.Forms.GroupBox gbStatus;
    }
}
