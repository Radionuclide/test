﻿namespace iba.Controls
{
    partial class OpcUaControl
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Root");
            this.timerRefreshStatus = new System.Windows.Forms.Timer(this.components);
            this.gbDiagnostics = new iba.Utility.CollapsibleGroupBox();
            this.dgvClients = new System.Windows.Forms.DataGridView();
            this.ColAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColProtocol = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColMsgCounter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLastMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.buttonClearClients = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.gbObjects = new iba.Utility.CollapsibleGroupBox();
            this.splitContainerObjectsFooter = new System.Windows.Forms.SplitContainer();
            this.labelObjType = new System.Windows.Forms.Label();
            this.tbObjType = new System.Windows.Forms.TextBox();
            this.labelObjValue = new System.Windows.Forms.Label();
            this.tbObjValue = new System.Windows.Forms.TextBox();
            this.tvObjects = new System.Windows.Forms.TreeView();
            this.gbConfiguration = new iba.Utility.CollapsibleGroupBox();
            this.splitContainerSecurity = new System.Windows.Forms.SplitContainer();
            this.gbLogon = new System.Windows.Forms.GroupBox();
            this.cbLogonCertificate = new System.Windows.Forms.CheckBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.buttonShowPassword = new System.Windows.Forms.Button();
            this.gbSecurity = new System.Windows.Forms.GroupBox();
            this.comboBoxSecurity256 = new System.Windows.Forms.ComboBox();
            this.comboBoxSecurity128 = new System.Windows.Forms.ComboBox();
            this.cbSecurity256 = new System.Windows.Forms.CheckBox();
            this.cbSecurity128 = new System.Windows.Forms.CheckBox();
            this.cbSecurityNone = new System.Windows.Forms.CheckBox();
            this.gbEndpoints = new System.Windows.Forms.GroupBox();
            this.buttonCopyToClipboard = new System.Windows.Forms.Button();
            this.buttonEndpointCopy = new System.Windows.Forms.Button();
            this.buttonEndpointDelete = new System.Windows.Forms.Button();
            this.dgvEndpoints = new System.Windows.Forms.DataGridView();
            this.dgvColumnHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColumnPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColumnUri = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonEndpointAdd = new System.Windows.Forms.Button();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.buttonTest = new System.Windows.Forms.Button();
            this.buttonConfigurationReset = new System.Windows.Forms.Button();
            this.buttonConfigurationApply = new System.Windows.Forms.Button();
            this.gbDiagnostics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            this.gbObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).BeginInit();
            this.splitContainerObjectsFooter.Panel1.SuspendLayout();
            this.splitContainerObjectsFooter.SuspendLayout();
            this.gbConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSecurity)).BeginInit();
            this.splitContainerSecurity.Panel1.SuspendLayout();
            this.splitContainerSecurity.Panel2.SuspendLayout();
            this.splitContainerSecurity.SuspendLayout();
            this.gbLogon.SuspendLayout();
            this.gbSecurity.SuspendLayout();
            this.gbEndpoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEndpoints)).BeginInit();
            this.SuspendLayout();
            // 
            // timerRefreshStatus
            // 
            this.timerRefreshStatus.Interval = 1000;
            this.timerRefreshStatus.Tick += new System.EventHandler(this.timerRefreshStatus_Tick);
            // 
            // gbDiagnostics
            // 
            this.gbDiagnostics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDiagnostics.Controls.Add(this.dgvClients);
            this.gbDiagnostics.Controls.Add(this.tbStatus);
            this.gbDiagnostics.Controls.Add(this.buttonClearClients);
            this.gbDiagnostics.Controls.Add(this.label14);
            this.gbDiagnostics.Controls.Add(this.label15);
            this.gbDiagnostics.Location = new System.Drawing.Point(15, 582);
            this.gbDiagnostics.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbDiagnostics.Name = "gbDiagnostics";
            this.gbDiagnostics.Padding = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbDiagnostics.Size = new System.Drawing.Size(690, 164);
            this.gbDiagnostics.TabIndex = 4;
            this.gbDiagnostics.TabStop = false;
            this.gbDiagnostics.Text = "Diagnostics";
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            this.dgvClients.AllowUserToResizeRows = false;
            this.dgvClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColAddress,
            this.ColProtocol,
            this.ColMsgCounter,
            this.ColLastMsg});
            this.dgvClients.Location = new System.Drawing.Point(18, 74);
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.ReadOnly = true;
            this.dgvClients.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dgvClients.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClients.Size = new System.Drawing.Size(654, 84);
            this.dgvClients.StandardTab = true;
            this.dgvClients.TabIndex = 1;
            // 
            // ColAddress
            // 
            this.ColAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColAddress.HeaderText = "Address";
            this.ColAddress.MinimumWidth = 50;
            this.ColAddress.Name = "ColAddress";
            this.ColAddress.ReadOnly = true;
            // 
            // ColProtocol
            // 
            this.ColProtocol.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColProtocol.HeaderText = "Protocol";
            this.ColProtocol.MinimumWidth = 50;
            this.ColProtocol.Name = "ColProtocol";
            this.ColProtocol.ReadOnly = true;
            // 
            // ColMsgCounter
            // 
            this.ColMsgCounter.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColMsgCounter.HeaderText = "Message counter";
            this.ColMsgCounter.MinimumWidth = 50;
            this.ColMsgCounter.Name = "ColMsgCounter";
            this.ColMsgCounter.ReadOnly = true;
            // 
            // ColLastMsg
            // 
            this.ColLastMsg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColLastMsg.HeaderText = "Last message";
            this.ColLastMsg.MinimumWidth = 50;
            this.ColLastMsg.Name = "ColLastMsg";
            this.ColLastMsg.ReadOnly = true;
            // 
            // tbStatus
            // 
            this.tbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStatus.Location = new System.Drawing.Point(88, 19);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(584, 20);
            this.tbStatus.TabIndex = 0;
            this.tbStatus.TabStop = false;
            // 
            // buttonClearClients
            // 
            this.buttonClearClients.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonClearClients.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonClearClients.Location = new System.Drawing.Point(483, 45);
            this.buttonClearClients.Name = "buttonClearClients";
            this.buttonClearClients.Size = new System.Drawing.Size(189, 23);
            this.buttonClearClients.TabIndex = 2;
            this.buttonClearClients.Text = "Clear client list";
            this.buttonClearClients.UseVisualStyleBackColor = true;
            this.buttonClearClients.Click += new System.EventHandler(this.buttonClearClients_Click);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(18, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Status:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(18, 50);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(138, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Connected OPC UA clients:";
            // 
            // gbObjects
            // 
            this.gbObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbObjects.Controls.Add(this.splitContainerObjectsFooter);
            this.gbObjects.Controls.Add(this.tvObjects);
            this.gbObjects.Location = new System.Drawing.Point(15, 350);
            this.gbObjects.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbObjects.Name = "gbObjects";
            this.gbObjects.Size = new System.Drawing.Size(690, 226);
            this.gbObjects.TabIndex = 3;
            this.gbObjects.TabStop = false;
            this.gbObjects.Text = "Objects";
            // 
            // splitContainerObjectsFooter
            // 
            this.splitContainerObjectsFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerObjectsFooter.IsSplitterFixed = true;
            this.splitContainerObjectsFooter.Location = new System.Drawing.Point(6, 135);
            this.splitContainerObjectsFooter.Name = "splitContainerObjectsFooter";
            // 
            // splitContainerObjectsFooter.Panel1
            // 
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjType);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjType);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjValue);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjValue);
            this.splitContainerObjectsFooter.Panel1.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
            this.splitContainerObjectsFooter.Size = new System.Drawing.Size(678, 85);
            this.splitContainerObjectsFooter.SplitterDistance = 310;
            this.splitContainerObjectsFooter.TabIndex = 2;
            // 
            // labelObjType
            // 
            this.labelObjType.AutoSize = true;
            this.labelObjType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelObjType.Location = new System.Drawing.Point(9, 58);
            this.labelObjType.Name = "labelObjType";
            this.labelObjType.Size = new System.Drawing.Size(34, 13);
            this.labelObjType.TabIndex = 9;
            this.labelObjType.Text = "Type:";
            // 
            // tbObjType
            // 
            this.tbObjType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjType.Location = new System.Drawing.Point(71, 55);
            this.tbObjType.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.tbObjType.Name = "tbObjType";
            this.tbObjType.ReadOnly = true;
            this.tbObjType.Size = new System.Drawing.Size(232, 20);
            this.tbObjType.TabIndex = 1;
            this.tbObjType.TabStop = false;
            // 
            // labelObjValue
            // 
            this.labelObjValue.AutoSize = true;
            this.labelObjValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelObjValue.Location = new System.Drawing.Point(9, 32);
            this.labelObjValue.Name = "labelObjValue";
            this.labelObjValue.Size = new System.Drawing.Size(37, 13);
            this.labelObjValue.TabIndex = 9;
            this.labelObjValue.Text = "Value:";
            // 
            // tbObjValue
            // 
            this.tbObjValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjValue.Location = new System.Drawing.Point(71, 29);
            this.tbObjValue.Name = "tbObjValue";
            this.tbObjValue.ReadOnly = true;
            this.tbObjValue.Size = new System.Drawing.Size(232, 20);
            this.tbObjValue.TabIndex = 1;
            this.tbObjValue.TabStop = false;
            // 
            // tvObjects
            // 
            this.tvObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvObjects.HideSelection = false;
            this.tvObjects.Location = new System.Drawing.Point(18, 46);
            this.tvObjects.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.tvObjects.Name = "tvObjects";
            treeNode1.Name = "NodeRoot";
            treeNode1.Text = "Root";
            this.tvObjects.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.tvObjects.Size = new System.Drawing.Size(654, 83);
            this.tvObjects.TabIndex = 0;
            // 
            // gbConfiguration
            // 
            this.gbConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbConfiguration.Controls.Add(this.splitContainerSecurity);
            this.gbConfiguration.Controls.Add(this.gbEndpoints);
            this.gbConfiguration.Controls.Add(this.cbEnabled);
            this.gbConfiguration.Controls.Add(this.buttonTest);
            this.gbConfiguration.Controls.Add(this.buttonConfigurationReset);
            this.gbConfiguration.Controls.Add(this.buttonConfigurationApply);
            this.gbConfiguration.Location = new System.Drawing.Point(15, 3);
            this.gbConfiguration.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbConfiguration.Name = "gbConfiguration";
            this.gbConfiguration.Size = new System.Drawing.Size(690, 341);
            this.gbConfiguration.TabIndex = 2;
            this.gbConfiguration.TabStop = false;
            this.gbConfiguration.Text = "Configuration";
            // 
            // splitContainerSecurity
            // 
            this.splitContainerSecurity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerSecurity.IsSplitterFixed = true;
            this.splitContainerSecurity.Location = new System.Drawing.Point(9, 42);
            this.splitContainerSecurity.Name = "splitContainerSecurity";
            // 
            // splitContainerSecurity.Panel1
            // 
            this.splitContainerSecurity.Panel1.Controls.Add(this.gbLogon);
            this.splitContainerSecurity.Panel1.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
            // 
            // splitContainerSecurity.Panel2
            // 
            this.splitContainerSecurity.Panel2.Controls.Add(this.gbSecurity);
            this.splitContainerSecurity.Size = new System.Drawing.Size(675, 104);
            this.splitContainerSecurity.SplitterDistance = 330;
            this.splitContainerSecurity.TabIndex = 2;
            // 
            // gbLogon
            // 
            this.gbLogon.Controls.Add(this.cbLogonCertificate);
            this.gbLogon.Controls.Add(this.tbUserName);
            this.gbLogon.Controls.Add(this.label11);
            this.gbLogon.Controls.Add(this.tbPassword);
            this.gbLogon.Controls.Add(this.label13);
            this.gbLogon.Controls.Add(this.buttonShowPassword);
            this.gbLogon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLogon.Location = new System.Drawing.Point(0, 0);
            this.gbLogon.Name = "gbLogon";
            this.gbLogon.Size = new System.Drawing.Size(330, 104);
            this.gbLogon.TabIndex = 0;
            this.gbLogon.TabStop = false;
            this.gbLogon.Text = "Logon policies";
            // 
            // cbLogonCertificate
            // 
            this.cbLogonCertificate.AutoSize = true;
            this.cbLogonCertificate.Location = new System.Drawing.Point(12, 71);
            this.cbLogonCertificate.Name = "cbLogonCertificate";
            this.cbLogonCertificate.Size = new System.Drawing.Size(73, 17);
            this.cbLogonCertificate.TabIndex = 7;
            this.cbLogonCertificate.Text = "Certificate";
            this.cbLogonCertificate.UseVisualStyleBackColor = true;
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(99, 19);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(100, 20);
            this.tbUserName.TabIndex = 4;
            this.tbUserName.Text = "Anonymous";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(9, 22);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "User name:";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(99, 45);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(100, 20);
            this.tbPassword.TabIndex = 5;
            this.tbPassword.Text = "***";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(9, 48);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Password:";
            // 
            // buttonShowPassword
            // 
            this.buttonShowPassword.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonShowPassword.Image = global::iba.Properties.Resources.Eye;
            this.buttonShowPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonShowPassword.Location = new System.Drawing.Point(205, 43);
            this.buttonShowPassword.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.buttonShowPassword.Name = "buttonShowPassword";
            this.buttonShowPassword.Size = new System.Drawing.Size(23, 23);
            this.buttonShowPassword.TabIndex = 6;
            this.buttonShowPassword.TabStop = false;
            this.buttonShowPassword.UseVisualStyleBackColor = true;
            this.buttonShowPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.handler_ShowPassword);
            this.buttonShowPassword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.handler_HidePassword);
            // 
            // gbSecurity
            // 
            this.gbSecurity.Controls.Add(this.comboBoxSecurity256);
            this.gbSecurity.Controls.Add(this.comboBoxSecurity128);
            this.gbSecurity.Controls.Add(this.cbSecurity256);
            this.gbSecurity.Controls.Add(this.cbSecurity128);
            this.gbSecurity.Controls.Add(this.cbSecurityNone);
            this.gbSecurity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSecurity.Location = new System.Drawing.Point(0, 0);
            this.gbSecurity.Name = "gbSecurity";
            this.gbSecurity.Size = new System.Drawing.Size(341, 104);
            this.gbSecurity.TabIndex = 1;
            this.gbSecurity.TabStop = false;
            this.gbSecurity.Text = "Security policies";
            // 
            // comboBoxSecurity256
            // 
            this.comboBoxSecurity256.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecurity256.FormattingEnabled = true;
            this.comboBoxSecurity256.Items.AddRange(new object[] {
            "Sign",
            "Sign & Encrypt",
            "Sign + Sign & Encrypt"});
            this.comboBoxSecurity256.Location = new System.Drawing.Point(115, 72);
            this.comboBoxSecurity256.Name = "comboBoxSecurity256";
            this.comboBoxSecurity256.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSecurity256.TabIndex = 8;
            // 
            // comboBoxSecurity128
            // 
            this.comboBoxSecurity128.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecurity128.FormattingEnabled = true;
            this.comboBoxSecurity128.Items.AddRange(new object[] {
            "Sign",
            "Sign & Encrypt",
            "Sign + Sign & Encrypt"});
            this.comboBoxSecurity128.Location = new System.Drawing.Point(115, 45);
            this.comboBoxSecurity128.Name = "comboBoxSecurity128";
            this.comboBoxSecurity128.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSecurity128.TabIndex = 8;
            // 
            // cbSecurity256
            // 
            this.cbSecurity256.AutoSize = true;
            this.cbSecurity256.Location = new System.Drawing.Point(6, 74);
            this.cbSecurity256.Name = "cbSecurity256";
            this.cbSecurity256.Size = new System.Drawing.Size(70, 17);
            this.cbSecurity256.TabIndex = 7;
            this.cbSecurity256.Text = "Basic256";
            this.cbSecurity256.UseVisualStyleBackColor = true;
            this.cbSecurity256.CheckedChanged += new System.EventHandler(this.cbSecurity256_CheckedChanged);
            // 
            // cbSecurity128
            // 
            this.cbSecurity128.AutoSize = true;
            this.cbSecurity128.Location = new System.Drawing.Point(6, 47);
            this.cbSecurity128.Name = "cbSecurity128";
            this.cbSecurity128.Size = new System.Drawing.Size(101, 17);
            this.cbSecurity128.TabIndex = 7;
            this.cbSecurity128.Text = "Basic128Rsa15";
            this.cbSecurity128.UseVisualStyleBackColor = true;
            this.cbSecurity128.CheckedChanged += new System.EventHandler(this.cbSecurity128_CheckedChanged);
            // 
            // cbSecurityNone
            // 
            this.cbSecurityNone.AutoSize = true;
            this.cbSecurityNone.Location = new System.Drawing.Point(6, 21);
            this.cbSecurityNone.Name = "cbSecurityNone";
            this.cbSecurityNone.Size = new System.Drawing.Size(52, 17);
            this.cbSecurityNone.TabIndex = 7;
            this.cbSecurityNone.Text = "None";
            this.cbSecurityNone.UseVisualStyleBackColor = true;
            // 
            // gbEndpoints
            // 
            this.gbEndpoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEndpoints.Controls.Add(this.buttonCopyToClipboard);
            this.gbEndpoints.Controls.Add(this.buttonEndpointCopy);
            this.gbEndpoints.Controls.Add(this.buttonEndpointDelete);
            this.gbEndpoints.Controls.Add(this.dgvEndpoints);
            this.gbEndpoints.Controls.Add(this.buttonEndpointAdd);
            this.gbEndpoints.Location = new System.Drawing.Point(9, 152);
            this.gbEndpoints.Name = "gbEndpoints";
            this.gbEndpoints.Size = new System.Drawing.Size(675, 154);
            this.gbEndpoints.TabIndex = 16;
            this.gbEndpoints.TabStop = false;
            this.gbEndpoints.Text = "Endpoints";
            // 
            // buttonCopyToClipboard
            // 
            this.buttonCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonCopyToClipboard.AutoSize = true;
            this.buttonCopyToClipboard.BackColor = System.Drawing.Color.Linen;
            this.buttonCopyToClipboard.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCopyToClipboard.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonCopyToClipboard.Location = new System.Drawing.Point(63, -1);
            this.buttonCopyToClipboard.Name = "buttonCopyToClipboard";
            this.buttonCopyToClipboard.Size = new System.Drawing.Size(76, 19);
            this.buttonCopyToClipboard.TabIndex = 14;
            this.buttonCopyToClipboard.Text = "Copy to clipboard";
            this.buttonCopyToClipboard.UseVisualStyleBackColor = false;
            this.buttonCopyToClipboard.Click += new System.EventHandler(this.buttonCopyToClipboard_Click);
            // 
            // buttonEndpointCopy
            // 
            this.buttonEndpointCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEndpointCopy.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonEndpointCopy.Image = global::iba.Properties.Resources.copy;
            this.buttonEndpointCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonEndpointCopy.Location = new System.Drawing.Point(646, 48);
            this.buttonEndpointCopy.Name = "buttonEndpointCopy";
            this.buttonEndpointCopy.Size = new System.Drawing.Size(23, 23);
            this.buttonEndpointCopy.TabIndex = 16;
            this.buttonEndpointCopy.TabStop = false;
            this.buttonEndpointCopy.UseVisualStyleBackColor = true;
            this.buttonEndpointCopy.Click += new System.EventHandler(this.buttonEndpointCopy_Click);
            // 
            // buttonEndpointDelete
            // 
            this.buttonEndpointDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEndpointDelete.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonEndpointDelete.Image = global::iba.Properties.Resources.img_error;
            this.buttonEndpointDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonEndpointDelete.Location = new System.Drawing.Point(646, 125);
            this.buttonEndpointDelete.Name = "buttonEndpointDelete";
            this.buttonEndpointDelete.Size = new System.Drawing.Size(23, 23);
            this.buttonEndpointDelete.TabIndex = 16;
            this.buttonEndpointDelete.TabStop = false;
            this.buttonEndpointDelete.UseVisualStyleBackColor = true;
            this.buttonEndpointDelete.Click += new System.EventHandler(this.buttonEndpointDelete_Click);
            // 
            // dgvEndpoints
            // 
            this.dgvEndpoints.AllowUserToAddRows = false;
            this.dgvEndpoints.AllowUserToDeleteRows = false;
            this.dgvEndpoints.AllowUserToResizeRows = false;
            this.dgvEndpoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEndpoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEndpoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColumnHost,
            this.dgvColumnPort,
            this.dgvColumnUri});
            this.dgvEndpoints.Location = new System.Drawing.Point(6, 19);
            this.dgvEndpoints.Name = "dgvEndpoints";
            this.dgvEndpoints.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dgvEndpoints.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvEndpoints.Size = new System.Drawing.Size(634, 129);
            this.dgvEndpoints.StandardTab = true;
            this.dgvEndpoints.TabIndex = 15;
            this.dgvEndpoints.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEndpoints_CellEnter);
            this.dgvEndpoints.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dgvEndpoints_CellParsing);
            this.dgvEndpoints.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEndpoints_CellValueChanged);
            this.dgvEndpoints.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvEndpoints_DataError);
            // 
            // dgvColumnHost
            // 
            this.dgvColumnHost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvColumnHost.HeaderText = "IP address/hostname";
            this.dgvColumnHost.MinimumWidth = 50;
            this.dgvColumnHost.Name = "dgvColumnHost";
            // 
            // dgvColumnPort
            // 
            this.dgvColumnPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvColumnPort.HeaderText = "Port";
            this.dgvColumnPort.MinimumWidth = 50;
            this.dgvColumnPort.Name = "dgvColumnPort";
            // 
            // dgvColumnUri
            // 
            this.dgvColumnUri.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvColumnUri.HeaderText = "URI";
            this.dgvColumnUri.MinimumWidth = 50;
            this.dgvColumnUri.Name = "dgvColumnUri";
            this.dgvColumnUri.ReadOnly = true;
            // 
            // buttonEndpointAdd
            // 
            this.buttonEndpointAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEndpointAdd.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonEndpointAdd.Image = global::iba.Properties.Resources.plus;
            this.buttonEndpointAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonEndpointAdd.Location = new System.Drawing.Point(646, 19);
            this.buttonEndpointAdd.Name = "buttonEndpointAdd";
            this.buttonEndpointAdd.Size = new System.Drawing.Size(23, 23);
            this.buttonEndpointAdd.TabIndex = 6;
            this.buttonEndpointAdd.TabStop = false;
            this.buttonEndpointAdd.UseVisualStyleBackColor = true;
            this.buttonEndpointAdd.Click += new System.EventHandler(this.buttonEndpointAdd_Click);
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Checked = true;
            this.cbEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnabled.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbEnabled.Location = new System.Drawing.Point(9, 19);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbEnabled.TabIndex = 0;
            this.cbEnabled.Text = "Enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // buttonTest
            // 
            this.buttonTest.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTest.BackColor = System.Drawing.Color.Linen;
            this.buttonTest.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonTest.Location = new System.Drawing.Point(245, 312);
            this.buttonTest.Name = "buttonTest";
            this.buttonTest.Size = new System.Drawing.Size(75, 23);
            this.buttonTest.TabIndex = 12;
            this.buttonTest.Text = "Test";
            this.buttonTest.UseVisualStyleBackColor = false;
            this.buttonTest.Click += new System.EventHandler(this.buttonTest2_Click);
            // 
            // buttonConfigurationReset
            // 
            this.buttonConfigurationReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigurationReset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonConfigurationReset.Location = new System.Drawing.Point(516, 312);
            this.buttonConfigurationReset.Name = "buttonConfigurationReset";
            this.buttonConfigurationReset.Size = new System.Drawing.Size(75, 23);
            this.buttonConfigurationReset.TabIndex = 12;
            this.buttonConfigurationReset.Text = "Reset";
            this.buttonConfigurationReset.UseVisualStyleBackColor = true;
            this.buttonConfigurationReset.Click += new System.EventHandler(this.buttonConfigurationReset_Click);
            // 
            // buttonConfigurationApply
            // 
            this.buttonConfigurationApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigurationApply.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonConfigurationApply.Location = new System.Drawing.Point(597, 312);
            this.buttonConfigurationApply.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.buttonConfigurationApply.Name = "buttonConfigurationApply";
            this.buttonConfigurationApply.Size = new System.Drawing.Size(75, 23);
            this.buttonConfigurationApply.TabIndex = 13;
            this.buttonConfigurationApply.Text = "Apply";
            this.buttonConfigurationApply.UseVisualStyleBackColor = true;
            this.buttonConfigurationApply.Click += new System.EventHandler(this.buttonConfigurationApply_Click);
            // 
            // OpcUaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.gbDiagnostics);
            this.Controls.Add(this.gbObjects);
            this.Controls.Add(this.gbConfiguration);
            this.MinimumSize = new System.Drawing.Size(720, 300);
            this.Name = "OpcUaControl";
            this.Size = new System.Drawing.Size(720, 761);
            this.Load += new System.EventHandler(this.OpcUaControl_Load);
            this.gbDiagnostics.ResumeLayout(false);
            this.gbDiagnostics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            this.gbObjects.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel1.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).EndInit();
            this.splitContainerObjectsFooter.ResumeLayout(false);
            this.gbConfiguration.ResumeLayout(false);
            this.gbConfiguration.PerformLayout();
            this.splitContainerSecurity.Panel1.ResumeLayout(false);
            this.splitContainerSecurity.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSecurity)).EndInit();
            this.splitContainerSecurity.ResumeLayout(false);
            this.gbLogon.ResumeLayout(false);
            this.gbLogon.PerformLayout();
            this.gbSecurity.ResumeLayout(false);
            this.gbSecurity.PerformLayout();
            this.gbEndpoints.ResumeLayout(false);
            this.gbEndpoints.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEndpoints)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Utility.CollapsibleGroupBox gbObjects;
        private System.Windows.Forms.SplitContainer splitContainerObjectsFooter;
        private System.Windows.Forms.Label labelObjType;
        private System.Windows.Forms.TextBox tbObjType;
        private System.Windows.Forms.Label labelObjValue;
        private System.Windows.Forms.TextBox tbObjValue;
        private System.Windows.Forms.TreeView tvObjects;
        private Utility.CollapsibleGroupBox gbConfiguration;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.Button buttonConfigurationReset;
        private System.Windows.Forms.Button buttonConfigurationApply;
        private System.Windows.Forms.Button buttonShowPassword;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button buttonTest;
        private Utility.CollapsibleGroupBox gbDiagnostics;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProtocol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMsgCounter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLastMsg;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Button buttonClearClients;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Timer timerRefreshStatus;
        private System.Windows.Forms.Button buttonCopyToClipboard;
        private System.Windows.Forms.GroupBox gbEndpoints;
        private System.Windows.Forms.Button buttonEndpointCopy;
        private System.Windows.Forms.Button buttonEndpointDelete;
        private System.Windows.Forms.DataGridView dgvEndpoints;
        private System.Windows.Forms.Button buttonEndpointAdd;
        private System.Windows.Forms.SplitContainer splitContainerSecurity;
        private System.Windows.Forms.GroupBox gbLogon;
        private System.Windows.Forms.CheckBox cbLogonCertificate;
        private System.Windows.Forms.GroupBox gbSecurity;
        private System.Windows.Forms.ComboBox comboBoxSecurity128;
        private System.Windows.Forms.CheckBox cbSecurity256;
        private System.Windows.Forms.CheckBox cbSecurity128;
        private System.Windows.Forms.CheckBox cbSecurityNone;
        private System.Windows.Forms.ComboBox comboBoxSecurity256;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColumnHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColumnPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColumnUri;
    }
}
