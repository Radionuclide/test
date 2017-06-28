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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Root");
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.timerRefreshClients = new System.Windows.Forms.Timer(this.components);
            this.labelNotConnected = new System.Windows.Forms.Label();
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
            this.tbObjOid = new System.Windows.Forms.TextBox();
            this.labelObjOid = new System.Windows.Forms.Label();
            this.labelObjValue = new System.Windows.Forms.Label();
            this.tbObjValue = new System.Windows.Forms.TextBox();
            this.labelObjMibName = new System.Windows.Forms.Label();
            this.labelObjType = new System.Windows.Forms.Label();
            this.tbObjType = new System.Windows.Forms.TextBox();
            this.tbObjMibName = new System.Windows.Forms.TextBox();
            this.buttonCreateMibFiles = new System.Windows.Forms.Button();
            this.tvObjects = new System.Windows.Forms.TreeView();
            this.gbConfiguration = new iba.Utility.CollapsibleGroupBox();
            this.rbDateTimeStr = new System.Windows.Forms.RadioButton();
            this.rbDateTimeTc = new System.Windows.Forms.RadioButton();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.buttonConfigurationReset = new System.Windows.Forms.Button();
            this.buttonConfigurationApply = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonShowPassword = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.buttonShowEncryptionKey = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.cmbEncryption = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbAuthentication = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbEncryptionKey = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.tbCommunity = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.gbDiagnostics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            this.gbObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).BeginInit();
            this.splitContainerObjectsFooter.Panel1.SuspendLayout();
            this.splitContainerObjectsFooter.Panel2.SuspendLayout();
            this.splitContainerObjectsFooter.SuspendLayout();
            this.gbConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
            this.SuspendLayout();
            // 
            // timerRefreshClients
            // 
            this.timerRefreshClients.Enabled = true;
            this.timerRefreshClients.Interval = 1000;
            this.timerRefreshClients.Tick += new System.EventHandler(this.timerRefreshClients_Tick);
            // 
            // labelNotConnected
            // 
            this.labelNotConnected.Dock = System.Windows.Forms.DockStyle.Top;
            this.labelNotConnected.Location = new System.Drawing.Point(0, 0);
            this.labelNotConnected.Name = "labelNotConnected";
            this.labelNotConnected.Padding = new System.Windows.Forms.Padding(15);
            this.labelNotConnected.Size = new System.Drawing.Size(720, 43);
            this.labelNotConnected.TabIndex = 3;
            this.labelNotConnected.Text = "Cannot connect to ibaDatCoordintor SNMP server.";
            this.labelNotConnected.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.labelNotConnected.Visible = false;
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
            this.gbDiagnostics.Location = new System.Drawing.Point(15, 667);
            this.gbDiagnostics.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbDiagnostics.Name = "gbDiagnostics";
            this.gbDiagnostics.Padding = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbDiagnostics.Size = new System.Drawing.Size(690, 164);
            this.gbDiagnostics.TabIndex = 2;
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
            this.label14.Location = new System.Drawing.Point(18, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Status:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(18, 50);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(129, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Connected SNMP clients:";
            // 
            // gbObjects
            // 
            this.gbObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbObjects.Controls.Add(this.splitContainerObjectsFooter);
            this.gbObjects.Controls.Add(this.buttonCreateMibFiles);
            this.gbObjects.Controls.Add(this.tvObjects);
            this.gbObjects.Location = new System.Drawing.Point(15, 243);
            this.gbObjects.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbObjects.Name = "gbObjects";
            this.gbObjects.Size = new System.Drawing.Size(690, 418);
            this.gbObjects.TabIndex = 1;
            this.gbObjects.TabStop = false;
            this.gbObjects.Text = "Objects";
            // 
            // splitContainerObjectsFooter
            // 
            this.splitContainerObjectsFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerObjectsFooter.IsSplitterFixed = true;
            this.splitContainerObjectsFooter.Location = new System.Drawing.Point(6, 355);
            this.splitContainerObjectsFooter.Name = "splitContainerObjectsFooter";
            // 
            // splitContainerObjectsFooter.Panel1
            // 
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjOid);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjOid);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjValue);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjValue);
            this.splitContainerObjectsFooter.Panel1.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
            // 
            // splitContainerObjectsFooter.Panel2
            // 
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.labelObjMibName);
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.labelObjType);
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.tbObjType);
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.tbObjMibName);
            this.splitContainerObjectsFooter.Size = new System.Drawing.Size(678, 57);
            this.splitContainerObjectsFooter.SplitterDistance = 330;
            this.splitContainerObjectsFooter.TabIndex = 2;
            // 
            // tbObjOid
            // 
            this.tbObjOid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjOid.Location = new System.Drawing.Point(82, 3);
            this.tbObjOid.Name = "tbObjOid";
            this.tbObjOid.ReadOnly = true;
            this.tbObjOid.Size = new System.Drawing.Size(233, 20);
            this.tbObjOid.TabIndex = 0;
            this.tbObjOid.TabStop = false;
            // 
            // labelObjOid
            // 
            this.labelObjOid.AutoSize = true;
            this.labelObjOid.Location = new System.Drawing.Point(9, 6);
            this.labelObjOid.Name = "labelObjOid";
            this.labelObjOid.Size = new System.Drawing.Size(26, 13);
            this.labelObjOid.TabIndex = 9;
            this.labelObjOid.Text = "OID";
            // 
            // labelObjValue
            // 
            this.labelObjValue.AutoSize = true;
            this.labelObjValue.Location = new System.Drawing.Point(9, 32);
            this.labelObjValue.Name = "labelObjValue";
            this.labelObjValue.Size = new System.Drawing.Size(34, 13);
            this.labelObjValue.TabIndex = 9;
            this.labelObjValue.Text = "Value";
            // 
            // tbObjValue
            // 
            this.tbObjValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjValue.Location = new System.Drawing.Point(82, 29);
            this.tbObjValue.Name = "tbObjValue";
            this.tbObjValue.ReadOnly = true;
            this.tbObjValue.Size = new System.Drawing.Size(233, 20);
            this.tbObjValue.TabIndex = 1;
            this.tbObjValue.TabStop = false;
            // 
            // labelObjMibName
            // 
            this.labelObjMibName.AutoSize = true;
            this.labelObjMibName.Location = new System.Drawing.Point(3, 6);
            this.labelObjMibName.Name = "labelObjMibName";
            this.labelObjMibName.Size = new System.Drawing.Size(57, 13);
            this.labelObjMibName.TabIndex = 9;
            this.labelObjMibName.Text = "MIB Name";
            // 
            // labelObjType
            // 
            this.labelObjType.AutoSize = true;
            this.labelObjType.Location = new System.Drawing.Point(3, 32);
            this.labelObjType.Name = "labelObjType";
            this.labelObjType.Size = new System.Drawing.Size(31, 13);
            this.labelObjType.TabIndex = 9;
            this.labelObjType.Text = "Type";
            // 
            // tbObjType
            // 
            this.tbObjType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjType.Location = new System.Drawing.Point(87, 29);
            this.tbObjType.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.tbObjType.Name = "tbObjType";
            this.tbObjType.ReadOnly = true;
            this.tbObjType.Size = new System.Drawing.Size(245, 20);
            this.tbObjType.TabIndex = 1;
            this.tbObjType.TabStop = false;
            // 
            // tbObjMibName
            // 
            this.tbObjMibName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjMibName.Location = new System.Drawing.Point(87, 3);
            this.tbObjMibName.Name = "tbObjMibName";
            this.tbObjMibName.ReadOnly = true;
            this.tbObjMibName.Size = new System.Drawing.Size(245, 20);
            this.tbObjMibName.TabIndex = 0;
            this.tbObjMibName.TabStop = false;
            // 
            // buttonCreateMibFiles
            // 
            this.buttonCreateMibFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonCreateMibFiles.Location = new System.Drawing.Point(514, 17);
            this.buttonCreateMibFiles.Name = "buttonCreateMibFiles";
            this.buttonCreateMibFiles.Size = new System.Drawing.Size(158, 23);
            this.buttonCreateMibFiles.TabIndex = 1;
            this.buttonCreateMibFiles.Text = "Create MIB files";
            this.buttonCreateMibFiles.UseVisualStyleBackColor = true;
            this.buttonCreateMibFiles.Click += new System.EventHandler(this.buttonCreateMibFiles_Click);
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
            this.tvObjects.Size = new System.Drawing.Size(654, 303);
            this.tvObjects.TabIndex = 0;
            this.tvObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjects_AfterSelect);
            // 
            // gbConfiguration
            // 
            this.gbConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbConfiguration.Controls.Add(this.rbDateTimeStr);
            this.gbConfiguration.Controls.Add(this.rbDateTimeTc);
            this.gbConfiguration.Controls.Add(this.cbEnabled);
            this.gbConfiguration.Controls.Add(this.buttonConfigurationReset);
            this.gbConfiguration.Controls.Add(this.buttonConfigurationApply);
            this.gbConfiguration.Controls.Add(this.label5);
            this.gbConfiguration.Controls.Add(this.buttonShowPassword);
            this.gbConfiguration.Controls.Add(this.label6);
            this.gbConfiguration.Controls.Add(this.buttonShowEncryptionKey);
            this.gbConfiguration.Controls.Add(this.label7);
            this.gbConfiguration.Controls.Add(this.cmbEncryption);
            this.gbConfiguration.Controls.Add(this.label8);
            this.gbConfiguration.Controls.Add(this.cmbAuthentication);
            this.gbConfiguration.Controls.Add(this.label9);
            this.gbConfiguration.Controls.Add(this.tbPassword);
            this.gbConfiguration.Controls.Add(this.label1);
            this.gbConfiguration.Controls.Add(this.label10);
            this.gbConfiguration.Controls.Add(this.tbEncryptionKey);
            this.gbConfiguration.Controls.Add(this.label11);
            this.gbConfiguration.Controls.Add(this.tbUserName);
            this.gbConfiguration.Controls.Add(this.label12);
            this.gbConfiguration.Controls.Add(this.tbCommunity);
            this.gbConfiguration.Controls.Add(this.label13);
            this.gbConfiguration.Controls.Add(this.numPort);
            this.gbConfiguration.Location = new System.Drawing.Point(15, 3);
            this.gbConfiguration.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbConfiguration.Name = "gbConfiguration";
            this.gbConfiguration.Size = new System.Drawing.Size(690, 234);
            this.gbConfiguration.TabIndex = 0;
            this.gbConfiguration.TabStop = false;
            this.gbConfiguration.Text = "Configuration";
            // 
            // rbDateTimeStr
            // 
            this.rbDateTimeStr.AutoSize = true;
            this.rbDateTimeStr.Location = new System.Drawing.Point(257, 209);
            this.rbDateTimeStr.Name = "rbDateTimeStr";
            this.rbDateTimeStr.Size = new System.Drawing.Size(190, 17);
            this.rbDateTimeStr.TabIndex = 11;
            this.rbDateTimeStr.Text = "String (\"yyyy-MM-dd HH:mm:ss.fff\")";
            this.rbDateTimeStr.UseVisualStyleBackColor = true;
            // 
            // rbDateTimeTc
            // 
            this.rbDateTimeTc.AutoSize = true;
            this.rbDateTimeTc.Checked = true;
            this.rbDateTimeTc.Location = new System.Drawing.Point(36, 209);
            this.rbDateTimeTc.Name = "rbDateTimeTc";
            this.rbDateTimeTc.Size = new System.Drawing.Size(197, 17);
            this.rbDateTimeTc.TabIndex = 10;
            this.rbDateTimeTc.TabStop = true;
            this.rbDateTimeTc.Text = "Binary (SNMPv2-TC - DateAndTime)";
            this.rbDateTimeTc.UseVisualStyleBackColor = true;
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Checked = true;
            this.cbEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnabled.Location = new System.Drawing.Point(9, 19);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbEnabled.TabIndex = 0;
            this.cbEnabled.Text = "Enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // buttonConfigurationReset
            // 
            this.buttonConfigurationReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigurationReset.Location = new System.Drawing.Point(516, 206);
            this.buttonConfigurationReset.Name = "buttonConfigurationReset";
            this.buttonConfigurationReset.Size = new System.Drawing.Size(75, 23);
            this.buttonConfigurationReset.TabIndex = 12;
            this.buttonConfigurationReset.Text = "Reset";
            this.buttonConfigurationReset.UseVisualStyleBackColor = true;
            this.buttonConfigurationReset.Click += new System.EventHandler(this.buttonConfigurationReset_Click);
            // 
            // buttonConfigurationApply
            // 
            this.buttonConfigurationApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigurationApply.Location = new System.Drawing.Point(597, 206);
            this.buttonConfigurationApply.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.buttonConfigurationApply.Name = "buttonConfigurationApply";
            this.buttonConfigurationApply.Size = new System.Drawing.Size(75, 23);
            this.buttonConfigurationApply.TabIndex = 13;
            this.buttonConfigurationApply.Text = "Apply";
            this.buttonConfigurationApply.UseVisualStyleBackColor = true;
            this.buttonConfigurationApply.Click += new System.EventHandler(this.buttonConfigurationApply_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(138, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Port:";
            // 
            // buttonShowPassword
            // 
            this.buttonShowPassword.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonShowPassword.Location = new System.Drawing.Point(649, 128);
            this.buttonShowPassword.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.buttonShowPassword.Name = "buttonShowPassword";
            this.buttonShowPassword.Size = new System.Drawing.Size(23, 23);
            this.buttonShowPassword.TabIndex = 6;
            this.buttonShowPassword.TabStop = false;
            this.buttonShowPassword.Text = "☼";
            this.buttonShowPassword.UseVisualStyleBackColor = true;
            this.buttonShowPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.handler_ShowPassword);
            this.buttonShowPassword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.handler_HidePassword);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 59);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(117, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "SNMP V1/V2c security";
            // 
            // buttonShowEncryptionKey
            // 
            this.buttonShowEncryptionKey.Font = new System.Drawing.Font("Arial Narrow", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonShowEncryptionKey.Location = new System.Drawing.Point(448, 153);
            this.buttonShowEncryptionKey.Name = "buttonShowEncryptionKey";
            this.buttonShowEncryptionKey.Size = new System.Drawing.Size(23, 23);
            this.buttonShowEncryptionKey.TabIndex = 9;
            this.buttonShowEncryptionKey.TabStop = false;
            this.buttonShowEncryptionKey.Text = "☼";
            this.buttonShowEncryptionKey.UseVisualStyleBackColor = true;
            this.buttonShowEncryptionKey.MouseDown += new System.Windows.Forms.MouseEventHandler(this.handler_ShowPassword);
            this.buttonShowEncryptionKey.MouseUp += new System.Windows.Forms.MouseEventHandler(this.handler_HidePassword);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(33, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(89, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Community string:";
            // 
            // cmbEncryption
            // 
            this.cmbEncryption.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbEncryption.Items.AddRange(new object[] {
            "None",
            "DES",
            "AES"});
            this.cmbEncryption.Location = new System.Drawing.Point(141, 156);
            this.cmbEncryption.Name = "cmbEncryption";
            this.cmbEncryption.Size = new System.Drawing.Size(67, 21);
            this.cmbEncryption.TabIndex = 7;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 111);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "SNMP V3 security";
            // 
            // cmbAuthentication
            // 
            this.cmbAuthentication.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbAuthentication.Items.AddRange(new object[] {
            "MD5",
            "SHA"});
            this.cmbAuthentication.Location = new System.Drawing.Point(141, 130);
            this.cmbAuthentication.Name = "cmbAuthentication";
            this.cmbAuthentication.Size = new System.Drawing.Size(67, 21);
            this.cmbAuthentication.TabIndex = 3;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(33, 133);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(78, 13);
            this.label9.TabIndex = 0;
            this.label9.Text = "Authentication:";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(543, 130);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(100, 20);
            this.tbPassword.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Date and time format:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(33, 159);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(60, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Encryption:";
            // 
            // tbEncryptionKey
            // 
            this.tbEncryptionKey.Location = new System.Drawing.Point(342, 156);
            this.tbEncryptionKey.Name = "tbEncryptionKey";
            this.tbEncryptionKey.PasswordChar = '*';
            this.tbEncryptionKey.Size = new System.Drawing.Size(100, 20);
            this.tbEncryptionKey.TabIndex = 8;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(234, 133);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "User name:";
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(342, 130);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(100, 20);
            this.tbUserName.TabIndex = 4;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(234, 159);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(80, 13);
            this.label12.TabIndex = 0;
            this.label12.Text = "Encryption key:";
            // 
            // tbCommunity
            // 
            this.tbCommunity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbCommunity.Location = new System.Drawing.Point(141, 78);
            this.tbCommunity.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.tbCommunity.Name = "tbCommunity";
            this.tbCommunity.Size = new System.Drawing.Size(531, 20);
            this.tbCommunity.TabIndex = 2;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(483, 133);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(56, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Password:";
            // 
            // numPort
            // 
            this.numPort.Location = new System.Drawing.Point(173, 18);
            this.numPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.numPort.Name = "numPort";
            this.numPort.Size = new System.Drawing.Size(69, 20);
            this.numPort.TabIndex = 1;
            // 
            // SnmpControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.gbDiagnostics);
            this.Controls.Add(this.gbObjects);
            this.Controls.Add(this.gbConfiguration);
            this.Controls.Add(this.labelNotConnected);
            this.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.MinimumSize = new System.Drawing.Size(720, 300);
            this.Name = "SnmpControl";
            this.Size = new System.Drawing.Size(720, 875);
            this.Load += new System.EventHandler(this.SnmpControl_Load);
            this.gbDiagnostics.ResumeLayout(false);
            this.gbDiagnostics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            this.gbObjects.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel1.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel1.PerformLayout();
            this.splitContainerObjectsFooter.Panel2.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).EndInit();
            this.splitContainerObjectsFooter.ResumeLayout(false);
            this.gbConfiguration.ResumeLayout(false);
            this.gbConfiguration.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private Utility.CollapsibleGroupBox gbConfiguration;
        private Utility.CollapsibleGroupBox gbDiagnostics;
        private Utility.CollapsibleGroupBox gbObjects;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.Button buttonConfigurationApply;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonShowPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button buttonShowEncryptionKey;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cmbEncryption;
        private System.Windows.Forms.Label label8;
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
        private System.Windows.Forms.Label label14;
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
        private System.Windows.Forms.Timer timerRefreshClients;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColProtocol;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColMsgCounter;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLastMsg;
        private System.Windows.Forms.RadioButton rbDateTimeStr;
        private System.Windows.Forms.RadioButton rbDateTimeTc;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelNotConnected;
    }
}
