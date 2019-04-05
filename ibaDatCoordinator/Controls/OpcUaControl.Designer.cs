namespace iba.Controls
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
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Root");
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
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.buttonTest1 = new System.Windows.Forms.Button();
            this.buttonTest2 = new System.Windows.Forms.Button();
            this.buttonConfigurationReset = new System.Windows.Forms.Button();
            this.buttonConfigurationApply = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.buttonShowPassword = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.tbServer = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.numPort = new System.Windows.Forms.NumericUpDown();
            this.button1 = new System.Windows.Forms.Button();
            this.gbDiagnostics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            this.gbObjects.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).BeginInit();
            this.splitContainerObjectsFooter.Panel1.SuspendLayout();
            this.splitContainerObjectsFooter.SuspendLayout();
            this.gbConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).BeginInit();
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
            this.gbDiagnostics.Location = new System.Drawing.Point(15, 475);
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
            this.gbObjects.Location = new System.Drawing.Point(15, 243);
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
            treeNode2.Name = "NodeRoot";
            treeNode2.Text = "Root";
            this.tvObjects.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode2});
            this.tvObjects.Size = new System.Drawing.Size(654, 83);
            this.tvObjects.TabIndex = 0;
            // 
            // gbConfiguration
            // 
            this.gbConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbConfiguration.Controls.Add(this.button1);
            this.gbConfiguration.Controls.Add(this.cbEnabled);
            this.gbConfiguration.Controls.Add(this.buttonTest1);
            this.gbConfiguration.Controls.Add(this.buttonTest2);
            this.gbConfiguration.Controls.Add(this.buttonConfigurationReset);
            this.gbConfiguration.Controls.Add(this.buttonConfigurationApply);
            this.gbConfiguration.Controls.Add(this.label5);
            this.gbConfiguration.Controls.Add(this.buttonShowPassword);
            this.gbConfiguration.Controls.Add(this.label7);
            this.gbConfiguration.Controls.Add(this.label8);
            this.gbConfiguration.Controls.Add(this.tbPassword);
            this.gbConfiguration.Controls.Add(this.label11);
            this.gbConfiguration.Controls.Add(this.tbUserName);
            this.gbConfiguration.Controls.Add(this.tbServer);
            this.gbConfiguration.Controls.Add(this.label13);
            this.gbConfiguration.Controls.Add(this.numPort);
            this.gbConfiguration.Location = new System.Drawing.Point(15, 3);
            this.gbConfiguration.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbConfiguration.Name = "gbConfiguration";
            this.gbConfiguration.Size = new System.Drawing.Size(690, 234);
            this.gbConfiguration.TabIndex = 2;
            this.gbConfiguration.TabStop = false;
            this.gbConfiguration.Text = "Configuration";
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
            // buttonTest1
            // 
            this.buttonTest1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTest1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonTest1.Location = new System.Drawing.Point(232, 115);
            this.buttonTest1.Name = "buttonTest1";
            this.buttonTest1.Size = new System.Drawing.Size(75, 23);
            this.buttonTest1.TabIndex = 12;
            this.buttonTest1.Text = "Test1";
            this.buttonTest1.UseVisualStyleBackColor = true;
            // 
            // buttonTest2
            // 
            this.buttonTest2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonTest2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonTest2.Location = new System.Drawing.Point(313, 115);
            this.buttonTest2.Name = "buttonTest2";
            this.buttonTest2.Size = new System.Drawing.Size(75, 23);
            this.buttonTest2.TabIndex = 12;
            this.buttonTest2.Text = "Test2";
            this.buttonTest2.UseVisualStyleBackColor = true;
            // 
            // buttonConfigurationReset
            // 
            this.buttonConfigurationReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigurationReset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonConfigurationReset.Location = new System.Drawing.Point(516, 115);
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
            this.buttonConfigurationApply.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonConfigurationApply.Location = new System.Drawing.Point(597, 115);
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
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(138, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Port:";
            // 
            // buttonShowPassword
            // 
            this.buttonShowPassword.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonShowPassword.Image = global::iba.Properties.Resources.Eye;
            this.buttonShowPassword.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonShowPassword.Location = new System.Drawing.Point(557, 68);
            this.buttonShowPassword.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.buttonShowPassword.Name = "buttonShowPassword";
            this.buttonShowPassword.Size = new System.Drawing.Size(23, 23);
            this.buttonShowPassword.TabIndex = 6;
            this.buttonShowPassword.TabStop = false;
            this.buttonShowPassword.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(6, 47);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Server:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(6, 73);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(45, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Security";
            // 
            // tbPassword
            // 
            this.tbPassword.Location = new System.Drawing.Point(451, 70);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(100, 20);
            this.tbPassword.TabIndex = 5;
            this.tbPassword.Text = "***";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(138, 73);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "User name:";
            // 
            // tbUserName
            // 
            this.tbUserName.Location = new System.Drawing.Point(250, 70);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(100, 20);
            this.tbUserName.TabIndex = 4;
            this.tbUserName.Text = "Anonymous";
            // 
            // tbServer
            // 
            this.tbServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbServer.Location = new System.Drawing.Point(141, 44);
            this.tbServer.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.tbServer.Name = "tbServer";
            this.tbServer.Size = new System.Drawing.Size(531, 20);
            this.tbServer.TabIndex = 2;
            this.tbServer.Text = "opc.tcp://lswork:62547/Quickstarts/DataAccessServer";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(391, 73);
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
            this.numPort.Value = new decimal(new int[] {
            21060,
            0,
            0,
            0});
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.AutoSize = true;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.button1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.button1.Location = new System.Drawing.Point(586, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(89, 19);
            this.button1.TabIndex = 14;
            this.button1.Text = "Copy to clipboard";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // OpcUaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDiagnostics);
            this.Controls.Add(this.gbObjects);
            this.Controls.Add(this.gbConfiguration);
            this.MinimumSize = new System.Drawing.Size(720, 300);
            this.Name = "OpcUaControl";
            this.Size = new System.Drawing.Size(720, 690);
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
            ((System.ComponentModel.ISupportInitialize)(this.numPort)).EndInit();
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button buttonShowPassword;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbServer;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown numPort;
        private System.Windows.Forms.Button buttonTest1;
        private System.Windows.Forms.Button buttonTest2;
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
        private System.Windows.Forms.Button button1;
    }
}
