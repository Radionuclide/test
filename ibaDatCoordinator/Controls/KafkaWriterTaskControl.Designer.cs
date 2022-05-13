namespace iba.Controls
{
    partial class KafkaWriterTaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KafkaWriterTaskControl));
            this.m_monitorGroup = new iba.Utility.CollapsibleGroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            this._toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.tabControl1 = new Crownwood.DotNetMagic.Controls.TabControl();
            this.tabTarget = new Crownwood.DotNetMagic.Controls.TabPage();
            this.panelKafka = new System.Windows.Forms.Panel();
            this.secLabel = new System.Windows.Forms.Label();
            this.paramRemoveButton = new System.Windows.Forms.Button();
            this.exportParamButton = new System.Windows.Forms.Button();
            this.schemaRegistryCb = new System.Windows.Forms.CheckBox();
            this.importParamButton = new System.Windows.Forms.Button();
            this.clusterConnSecurityLabel = new System.Windows.Forms.Label();
            this.schemaCACertPlaceholder = new System.Windows.Forms.ComboBox();
            this.clusterConnectionSecurityComboBox = new System.Windows.Forms.ComboBox();
            this.schemaCACertificateLabel = new System.Windows.Forms.Label();
            this.schemaRegSecurityLabel = new System.Windows.Forms.Label();
            this.schemaEnableSSLVerificationCb = new System.Windows.Forms.CheckBox();
            this.schemaTextBox = new System.Windows.Forms.TextBox();
            this.schemaClientCertificateLabel = new System.Windows.Forms.Label();
            this.schemaRegistryConnectionSecurityComboBox = new System.Windows.Forms.ComboBox();
            this.schemaClientCertPlaceholder = new System.Windows.Forms.ComboBox();
            this.clientCertPlaceholder = new System.Windows.Forms.ComboBox();
            this.schemaPassTextBox = new System.Windows.Forms.TextBox();
            this.paramAddButton = new System.Windows.Forms.Button();
            this.schemaNameTextBox = new System.Windows.Forms.TextBox();
            this.SASLMechLabel = new System.Windows.Forms.Label();
            this.schemaPassLabel = new System.Windows.Forms.Label();
            this.acknowledgmentComboBox = new System.Windows.Forms.ComboBox();
            this.schemaNameLabel = new System.Windows.Forms.Label();
            this.SASLMechanismComboBox = new System.Windows.Forms.ComboBox();
            this.CACertPlaceholder = new System.Windows.Forms.ComboBox();
            this.paramGrid = new DevExpress.XtraGrid.GridControl();
            this._viewParam = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.keyGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.valGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.SASLNameLabel = new System.Windows.Forms.Label();
            this.CACertificateLabel = new System.Windows.Forms.Label();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.timeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SASLPassLabel = new System.Windows.Forms.Label();
            this.SASLPassTextBox = new System.Windows.Forms.TextBox();
            this.clusterAddressLabel = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.clientCertificateLabel = new System.Windows.Forms.Label();
            this.SASLNameTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.messageTimeoutLabel = new System.Windows.Forms.Label();
            this.enableSSLVerificationCb = new System.Windows.Forms.CheckBox();
            this.testConnectionBtn = new System.Windows.Forms.Button();
            this.idLabel = new System.Windows.Forms.Label();
            this.identifierTextBox = new System.Windows.Forms.TextBox();
            this.typeLabel = new System.Windows.Forms.Label();
            this.clusterTypeComboBox = new System.Windows.Forms.ComboBox();
            this.tabConnection = new Crownwood.DotNetMagic.Controls.TabPage();
            this.timestampLabel = new System.Windows.Forms.Label();
            this.timeGrid = new DevExpress.XtraGrid.GridControl();
            this.m_viewTime = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.m_colTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.signalRefTextBox = new System.Windows.Forms.TextBox();
            this.exprGrid = new DevExpress.XtraGrid.GridControl();
            this.dataGV = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.expressionGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dataTypeGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.testValueGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.nameGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.label8 = new System.Windows.Forms.Label();
            this.expressionAddButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.m_browsePDOFileButton = new System.Windows.Forms.Button();
            this.metadataComboBox = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.label13 = new System.Windows.Forms.Label();
            this.expressionRemoveButton = new System.Windows.Forms.Button();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.keyTextBox = new System.Windows.Forms.TextBox();
            this.dataFormatComboBox = new System.Windows.Forms.ComboBox();
            this.m_btnUploadPDO = new System.Windows.Forms.Button();
            this.expressionCopyButton = new System.Windows.Forms.Button();
            this.digitalFormatComboBox = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.m_testButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.m_browseDatFileButton = new System.Windows.Forms.Button();
            this.m_datFileTextBox = new System.Windows.Forms.TextBox();
            this.topicComboBox = new System.Windows.Forms.ComboBox();
            this.label154t5 = new System.Windows.Forms.Label();
            this.downButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.placeholdersToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.placeholdersKeyToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabTarget.SuspendLayout();
            this.panelKafka.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paramGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._viewParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericUpDown)).BeginInit();
            this.tabConnection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.exprGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.metadataComboBox.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // m_monitorGroup
            // 
            this.m_monitorGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_monitorGroup.Controls.Add(this.label4);
            this.m_monitorGroup.Controls.Add(this.label7);
            this.m_monitorGroup.Controls.Add(this.m_nudTime);
            this.m_monitorGroup.Controls.Add(this.m_nudMemory);
            this.m_monitorGroup.Controls.Add(this.m_cbTime);
            this.m_monitorGroup.Controls.Add(this.m_cbMemory);
            this.m_monitorGroup.Location = new System.Drawing.Point(0, 788);
            this.m_monitorGroup.Margin = new System.Windows.Forms.Padding(4);
            this.m_monitorGroup.Name = "m_monitorGroup";
            this.m_monitorGroup.Padding = new System.Windows.Forms.Padding(4);
            this.m_monitorGroup.Size = new System.Drawing.Size(968, 89);
            this.m_monitorGroup.TabIndex = 26;
            this.m_monitorGroup.TabStop = false;
            this.m_monitorGroup.Text = "Monitor ibaAnalyzer";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(549, 57);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(134, 17);
            this.label4.TabIndex = 5;
            this.label4.Text = "minutes to complete";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(549, 30);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 17);
            this.label7.TabIndex = 2;
            this.label7.Text = "Mbytes of memory";
            // 
            // m_nudTime
            // 
            this.m_nudTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudTime.Location = new System.Drawing.Point(437, 57);
            this.m_nudTime.Margin = new System.Windows.Forms.Padding(4);
            this.m_nudTime.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.m_nudTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudTime.Name = "m_nudTime";
            this.m_nudTime.Size = new System.Drawing.Size(104, 22);
            this.m_nudTime.TabIndex = 4;
            this.m_nudTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // m_nudMemory
            // 
            this.m_nudMemory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudMemory.Location = new System.Drawing.Point(437, 25);
            this.m_nudMemory.Margin = new System.Windows.Forms.Padding(4);
            this.m_nudMemory.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.m_nudMemory.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudMemory.Name = "m_nudMemory";
            this.m_nudMemory.Size = new System.Drawing.Size(104, 22);
            this.m_nudMemory.TabIndex = 1;
            this.m_nudMemory.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // m_cbTime
            // 
            this.m_cbTime.AutoSize = true;
            this.m_cbTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_cbTime.Location = new System.Drawing.Point(20, 57);
            this.m_cbTime.Margin = new System.Windows.Forms.Padding(4);
            this.m_cbTime.Name = "m_cbTime";
            this.m_cbTime.Size = new System.Drawing.Size(355, 21);
            this.m_cbTime.TabIndex = 3;
            this.m_cbTime.Text = "Time limit: abort task if ibaAnalyzer takes more than";
            this.m_cbTime.UseVisualStyleBackColor = true;
            // 
            // m_cbMemory
            // 
            this.m_cbMemory.AutoSize = true;
            this.m_cbMemory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_cbMemory.Location = new System.Drawing.Point(20, 27);
            this.m_cbMemory.Margin = new System.Windows.Forms.Padding(4);
            this.m_cbMemory.Name = "m_cbMemory";
            this.m_cbMemory.Size = new System.Drawing.Size(413, 21);
            this.m_cbMemory.TabIndex = 0;
            this.m_cbMemory.Text = "Memory limit: abort task if ibaAnalyzer starts using more than";
            this.m_cbMemory.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDragReorder = false;
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Appearance = Crownwood.DotNetMagic.Controls.VisualAppearance.MultiDocument;
            this.tabControl1.ForeColor = System.Drawing.Color.Empty;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.MediaPlayerDockSides = false;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.OfficeDockSides = false;
            this.tabControl1.OfficeStyle = Crownwood.DotNetMagic.Controls.OfficeStyle.SoftWhite;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowArrows = false;
            this.tabControl1.ShowClose = false;
            this.tabControl1.ShowDropSelect = false;
            this.tabControl1.Size = new System.Drawing.Size(971, 782);
            this.tabControl1.Style = Crownwood.DotNetMagic.Common.VisualStyle.IDE2005;
            this.tabControl1.TabIndex = 0;
            this.tabControl1.TabPages.AddRange(new Crownwood.DotNetMagic.Controls.TabPage[] {
            this.tabTarget,
            this.tabConnection});
            this.tabControl1.TextColor = System.Drawing.Color.Empty;
            this.tabControl1.TextTips = true;
            // 
            // tabTarget
            // 
            this.tabTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabTarget.Controls.Add(this.panelKafka);
            this.tabTarget.Controls.Add(this.testConnectionBtn);
            this.tabTarget.Controls.Add(this.idLabel);
            this.tabTarget.Controls.Add(this.identifierTextBox);
            this.tabTarget.Controls.Add(this.typeLabel);
            this.tabTarget.Controls.Add(this.clusterTypeComboBox);
            this.tabTarget.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabTarget.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabTarget.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabTarget.Location = new System.Drawing.Point(1, 24);
            this.tabTarget.Name = "tabTarget";
            this.tabTarget.Padding = new System.Windows.Forms.Padding(3);
            this.tabTarget.SelectBackColor = System.Drawing.Color.Empty;
            this.tabTarget.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabTarget.SelectTextColor = System.Drawing.Color.Empty;
            this.tabTarget.Size = new System.Drawing.Size(969, 757);
            this.tabTarget.TabIndex = 0;
            this.tabTarget.Text = "tabTarget";
            this.tabTarget.Title = "Connection";
            this.tabTarget.ToolTip = "Page";
            // 
            // panelKafka
            // 
            this.panelKafka.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelKafka.Controls.Add(this.secLabel);
            this.panelKafka.Controls.Add(this.paramRemoveButton);
            this.panelKafka.Controls.Add(this.exportParamButton);
            this.panelKafka.Controls.Add(this.schemaRegistryCb);
            this.panelKafka.Controls.Add(this.importParamButton);
            this.panelKafka.Controls.Add(this.clusterConnSecurityLabel);
            this.panelKafka.Controls.Add(this.schemaCACertPlaceholder);
            this.panelKafka.Controls.Add(this.clusterConnectionSecurityComboBox);
            this.panelKafka.Controls.Add(this.schemaCACertificateLabel);
            this.panelKafka.Controls.Add(this.schemaRegSecurityLabel);
            this.panelKafka.Controls.Add(this.schemaEnableSSLVerificationCb);
            this.panelKafka.Controls.Add(this.schemaTextBox);
            this.panelKafka.Controls.Add(this.schemaClientCertificateLabel);
            this.panelKafka.Controls.Add(this.schemaRegistryConnectionSecurityComboBox);
            this.panelKafka.Controls.Add(this.schemaClientCertPlaceholder);
            this.panelKafka.Controls.Add(this.clientCertPlaceholder);
            this.panelKafka.Controls.Add(this.schemaPassTextBox);
            this.panelKafka.Controls.Add(this.paramAddButton);
            this.panelKafka.Controls.Add(this.schemaNameTextBox);
            this.panelKafka.Controls.Add(this.SASLMechLabel);
            this.panelKafka.Controls.Add(this.schemaPassLabel);
            this.panelKafka.Controls.Add(this.acknowledgmentComboBox);
            this.panelKafka.Controls.Add(this.schemaNameLabel);
            this.panelKafka.Controls.Add(this.SASLMechanismComboBox);
            this.panelKafka.Controls.Add(this.CACertPlaceholder);
            this.panelKafka.Controls.Add(this.paramGrid);
            this.panelKafka.Controls.Add(this.SASLNameLabel);
            this.panelKafka.Controls.Add(this.CACertificateLabel);
            this.panelKafka.Controls.Add(this.addressTextBox);
            this.panelKafka.Controls.Add(this.timeoutNumericUpDown);
            this.panelKafka.Controls.Add(this.SASLPassLabel);
            this.panelKafka.Controls.Add(this.SASLPassTextBox);
            this.panelKafka.Controls.Add(this.clusterAddressLabel);
            this.panelKafka.Controls.Add(this.label9);
            this.panelKafka.Controls.Add(this.clientCertificateLabel);
            this.panelKafka.Controls.Add(this.SASLNameTextBox);
            this.panelKafka.Controls.Add(this.label10);
            this.panelKafka.Controls.Add(this.messageTimeoutLabel);
            this.panelKafka.Controls.Add(this.enableSSLVerificationCb);
            this.panelKafka.Location = new System.Drawing.Point(0, 70);
            this.panelKafka.MinimumSize = new System.Drawing.Size(0, 679);
            this.panelKafka.Name = "panelKafka";
            this.panelKafka.Size = new System.Drawing.Size(970, 679);
            this.panelKafka.TabIndex = 97;
            // 
            // secLabel
            // 
            this.secLabel.AutoSize = true;
            this.secLabel.Location = new System.Drawing.Point(341, 463);
            this.secLabel.Name = "secLabel";
            this.secLabel.Size = new System.Drawing.Size(15, 17);
            this.secLabel.TabIndex = 96;
            this.secLabel.Text = "s";
            // 
            // paramRemoveButton
            // 
            this.paramRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.paramRemoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.paramRemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("paramRemoveButton.Image")));
            this.paramRemoveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.paramRemoveButton.Location = new System.Drawing.Point(926, 641);
            this.paramRemoveButton.Margin = new System.Windows.Forms.Padding(4);
            this.paramRemoveButton.Name = "paramRemoveButton";
            this.paramRemoveButton.Size = new System.Drawing.Size(32, 30);
            this.paramRemoveButton.TabIndex = 25;
            this.paramRemoveButton.UseVisualStyleBackColor = true;
            this.paramRemoveButton.Click += new System.EventHandler(this.paramRemoveButton_Click);
            // 
            // exportParamButton
            // 
            this.exportParamButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportParamButton.Image = Icons.Gui.All.Images.Export(16);
            this.exportParamButton.Location = new System.Drawing.Point(926, 603);
            this.exportParamButton.Margin = new System.Windows.Forms.Padding(4);
            this.exportParamButton.Name = "exportParamButton";
            this.exportParamButton.Size = new System.Drawing.Size(32, 30);
            this.exportParamButton.TabIndex = 57;
            this.exportParamButton.UseVisualStyleBackColor = true;
            this.exportParamButton.Click += new System.EventHandler(this.OnExportParameters);
            // 
            // schemaRegistryCb
            // 
            this.schemaRegistryCb.AutoSize = true;
            this.schemaRegistryCb.Location = new System.Drawing.Point(11, 238);
            this.schemaRegistryCb.Margin = new System.Windows.Forms.Padding(4);
            this.schemaRegistryCb.Name = "schemaRegistryCb";
            this.schemaRegistryCb.Size = new System.Drawing.Size(191, 21);
            this.schemaRegistryCb.TabIndex = 94;
            this.schemaRegistryCb.Text = "Schema registry address:";
            this.schemaRegistryCb.UseVisualStyleBackColor = true;
            this.schemaRegistryCb.CheckedChanged += new System.EventHandler(this.schemaRegistryCb_CheckedChanged);
            // 
            // importParamButton
            // 
            this.importParamButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importParamButton.Image = Icons.Gui.All.Images.Import(16);
            this.importParamButton.Location = new System.Drawing.Point(926, 565);
            this.importParamButton.Margin = new System.Windows.Forms.Padding(4);
            this.importParamButton.Name = "importParamButton";
            this.importParamButton.Size = new System.Drawing.Size(32, 30);
            this.importParamButton.TabIndex = 56;
            this.importParamButton.UseVisualStyleBackColor = true;
            this.importParamButton.Click += new System.EventHandler(this.OnImportParameters);
            // 
            // clusterConnSecurityLabel
            // 
            this.clusterConnSecurityLabel.AutoSize = true;
            this.clusterConnSecurityLabel.Location = new System.Drawing.Point(11, 42);
            this.clusterConnSecurityLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.clusterConnSecurityLabel.Name = "clusterConnSecurityLabel";
            this.clusterConnSecurityLabel.Size = new System.Drawing.Size(182, 17);
            this.clusterConnSecurityLabel.TabIndex = 59;
            this.clusterConnSecurityLabel.Text = "Cluster connection security:";
            // 
            // schemaCACertPlaceholder
            // 
            this.schemaCACertPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaCACertPlaceholder.FormattingEnabled = true;
            this.schemaCACertPlaceholder.Location = new System.Drawing.Point(443, 427);
            this.schemaCACertPlaceholder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.schemaCACertPlaceholder.Name = "schemaCACertPlaceholder";
            this.schemaCACertPlaceholder.Size = new System.Drawing.Size(475, 24);
            this.schemaCACertPlaceholder.TabIndex = 93;
            // 
            // clusterConnectionSecurityComboBox
            // 
            this.clusterConnectionSecurityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clusterConnectionSecurityComboBox.FormattingEnabled = true;
            this.clusterConnectionSecurityComboBox.Items.AddRange(new object[] {
            "PLAINTEXT",
            "SSL",
            "SASL/PLAINTEXT",
            "SASL/SSL"});
            this.clusterConnectionSecurityComboBox.Location = new System.Drawing.Point(279, 39);
            this.clusterConnectionSecurityComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.clusterConnectionSecurityComboBox.Name = "clusterConnectionSecurityComboBox";
            this.clusterConnectionSecurityComboBox.Size = new System.Drawing.Size(144, 24);
            this.clusterConnectionSecurityComboBox.TabIndex = 60;
            this.clusterConnectionSecurityComboBox.SelectedIndexChanged += new System.EventHandler(this.clusterConnectionSecurityComboBox_SelectedIndexChanged);
            // 
            // schemaCACertificateLabel
            // 
            this.schemaCACertificateLabel.AutoSize = true;
            this.schemaCACertificateLabel.Location = new System.Drawing.Point(279, 431);
            this.schemaCACertificateLabel.Name = "schemaCACertificateLabel";
            this.schemaCACertificateLabel.Size = new System.Drawing.Size(95, 17);
            this.schemaCACertificateLabel.TabIndex = 92;
            this.schemaCACertificateLabel.Text = "CA certificate:";
            // 
            // schemaRegSecurityLabel
            // 
            this.schemaRegSecurityLabel.AutoSize = true;
            this.schemaRegSecurityLabel.Location = new System.Drawing.Point(11, 270);
            this.schemaRegSecurityLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.schemaRegSecurityLabel.MaximumSize = new System.Drawing.Size(240, 39);
            this.schemaRegSecurityLabel.Name = "schemaRegSecurityLabel";
            this.schemaRegSecurityLabel.Size = new System.Drawing.Size(240, 17);
            this.schemaRegSecurityLabel.TabIndex = 61;
            this.schemaRegSecurityLabel.Text = "Schema registry connection security:";
            // 
            // schemaEnableSSLVerificationCb
            // 
            this.schemaEnableSSLVerificationCb.AutoSize = true;
            this.schemaEnableSSLVerificationCb.Location = new System.Drawing.Point(279, 397);
            this.schemaEnableSSLVerificationCb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.schemaEnableSSLVerificationCb.Name = "schemaEnableSSLVerificationCb";
            this.schemaEnableSSLVerificationCb.Size = new System.Drawing.Size(176, 21);
            this.schemaEnableSSLVerificationCb.TabIndex = 91;
            this.schemaEnableSSLVerificationCb.Text = "Enable SSL verification";
            this.schemaEnableSSLVerificationCb.UseVisualStyleBackColor = true;
            this.schemaEnableSSLVerificationCb.CheckedChanged += new System.EventHandler(this.schemaEnableSSLVerificationCb_CheckedChanged);
            // 
            // schemaTextBox
            // 
            this.schemaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaTextBox.Location = new System.Drawing.Point(279, 234);
            this.schemaTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.schemaTextBox.Name = "schemaTextBox";
            this.schemaTextBox.Size = new System.Drawing.Size(639, 22);
            this.schemaTextBox.TabIndex = 16;
            // 
            // schemaClientCertificateLabel
            // 
            this.schemaClientCertificateLabel.AutoSize = true;
            this.schemaClientCertificateLabel.Location = new System.Drawing.Point(279, 370);
            this.schemaClientCertificateLabel.Name = "schemaClientCertificateLabel";
            this.schemaClientCertificateLabel.Size = new System.Drawing.Size(112, 17);
            this.schemaClientCertificateLabel.TabIndex = 90;
            this.schemaClientCertificateLabel.Text = "Client certificate:";
            // 
            // schemaRegistryConnectionSecurityComboBox
            // 
            this.schemaRegistryConnectionSecurityComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.schemaRegistryConnectionSecurityComboBox.FormattingEnabled = true;
            this.schemaRegistryConnectionSecurityComboBox.Items.AddRange(new object[] {
            "HTTP",
            "HTTPS",
            "HTTP + Authentication",
            "HTTPS + Authentication"});
            this.schemaRegistryConnectionSecurityComboBox.Location = new System.Drawing.Point(279, 266);
            this.schemaRegistryConnectionSecurityComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.schemaRegistryConnectionSecurityComboBox.Name = "schemaRegistryConnectionSecurityComboBox";
            this.schemaRegistryConnectionSecurityComboBox.Size = new System.Drawing.Size(188, 24);
            this.schemaRegistryConnectionSecurityComboBox.TabIndex = 62;
            this.schemaRegistryConnectionSecurityComboBox.SelectedIndexChanged += new System.EventHandler(this.schemaRegistryConnectionSecurityComboBox_SelectedIndexChanged);
            // 
            // schemaClientCertPlaceholder
            // 
            this.schemaClientCertPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaClientCertPlaceholder.FormattingEnabled = true;
            this.schemaClientCertPlaceholder.Location = new System.Drawing.Point(443, 366);
            this.schemaClientCertPlaceholder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.schemaClientCertPlaceholder.Name = "schemaClientCertPlaceholder";
            this.schemaClientCertPlaceholder.Size = new System.Drawing.Size(475, 24);
            this.schemaClientCertPlaceholder.TabIndex = 89;
            // 
            // clientCertPlaceholder
            // 
            this.clientCertPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientCertPlaceholder.FormattingEnabled = true;
            this.clientCertPlaceholder.Location = new System.Drawing.Point(443, 138);
            this.clientCertPlaceholder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.clientCertPlaceholder.Name = "clientCertPlaceholder";
            this.clientCertPlaceholder.Size = new System.Drawing.Size(475, 24);
            this.clientCertPlaceholder.TabIndex = 63;
            // 
            // schemaPassTextBox
            // 
            this.schemaPassTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaPassTextBox.Location = new System.Drawing.Point(443, 333);
            this.schemaPassTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.schemaPassTextBox.Name = "schemaPassTextBox";
            this.schemaPassTextBox.PasswordChar = '●';
            this.schemaPassTextBox.Size = new System.Drawing.Size(199, 22);
            this.schemaPassTextBox.TabIndex = 88;
            // 
            // paramAddButton
            // 
            this.paramAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.paramAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.paramAddButton.Image = ((System.Drawing.Image)(resources.GetObject("paramAddButton.Image")));
            this.paramAddButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.paramAddButton.Location = new System.Drawing.Point(926, 527);
            this.paramAddButton.Margin = new System.Windows.Forms.Padding(4);
            this.paramAddButton.Name = "paramAddButton";
            this.paramAddButton.Size = new System.Drawing.Size(32, 30);
            this.paramAddButton.TabIndex = 24;
            this.paramAddButton.UseVisualStyleBackColor = true;
            this.paramAddButton.Click += new System.EventHandler(this.paramAddButton_Click);
            // 
            // schemaNameTextBox
            // 
            this.schemaNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaNameTextBox.Location = new System.Drawing.Point(443, 299);
            this.schemaNameTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.schemaNameTextBox.Name = "schemaNameTextBox";
            this.schemaNameTextBox.Size = new System.Drawing.Size(199, 22);
            this.schemaNameTextBox.TabIndex = 87;
            // 
            // SASLMechLabel
            // 
            this.SASLMechLabel.AutoSize = true;
            this.SASLMechLabel.Location = new System.Drawing.Point(279, 78);
            this.SASLMechLabel.Name = "SASLMechLabel";
            this.SASLMechLabel.Size = new System.Drawing.Size(122, 17);
            this.SASLMechLabel.TabIndex = 74;
            this.SASLMechLabel.Text = "SASL mechanism:";
            // 
            // schemaPassLabel
            // 
            this.schemaPassLabel.AutoSize = true;
            this.schemaPassLabel.Location = new System.Drawing.Point(279, 336);
            this.schemaPassLabel.Name = "schemaPassLabel";
            this.schemaPassLabel.Size = new System.Drawing.Size(73, 17);
            this.schemaPassLabel.TabIndex = 86;
            this.schemaPassLabel.Text = "Password:";
            // 
            // acknowledgmentComboBox
            // 
            this.acknowledgmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.acknowledgmentComboBox.FormattingEnabled = true;
            this.acknowledgmentComboBox.Items.AddRange(new object[] {
            "None",
            "Leader",
            "All"});
            this.acknowledgmentComboBox.Location = new System.Drawing.Point(279, 493);
            this.acknowledgmentComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.acknowledgmentComboBox.Name = "acknowledgmentComboBox";
            this.acknowledgmentComboBox.Size = new System.Drawing.Size(144, 24);
            this.acknowledgmentComboBox.TabIndex = 11;
            // 
            // schemaNameLabel
            // 
            this.schemaNameLabel.AutoSize = true;
            this.schemaNameLabel.Location = new System.Drawing.Point(279, 303);
            this.schemaNameLabel.Name = "schemaNameLabel";
            this.schemaNameLabel.Size = new System.Drawing.Size(77, 17);
            this.schemaNameLabel.TabIndex = 85;
            this.schemaNameLabel.Text = "Username:";
            // 
            // SASLMechanismComboBox
            // 
            this.SASLMechanismComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SASLMechanismComboBox.FormattingEnabled = true;
            this.SASLMechanismComboBox.Items.AddRange(new object[] {
            "PLAIN",
            "SCRAM-SHA-256",
            "SCRAM-SHA-512"});
            this.SASLMechanismComboBox.Location = new System.Drawing.Point(443, 74);
            this.SASLMechanismComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SASLMechanismComboBox.Name = "SASLMechanismComboBox";
            this.SASLMechanismComboBox.Size = new System.Drawing.Size(144, 24);
            this.SASLMechanismComboBox.TabIndex = 75;
            // 
            // CACertPlaceholder
            // 
            this.CACertPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CACertPlaceholder.FormattingEnabled = true;
            this.CACertPlaceholder.Location = new System.Drawing.Point(443, 201);
            this.CACertPlaceholder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CACertPlaceholder.Name = "CACertPlaceholder";
            this.CACertPlaceholder.Size = new System.Drawing.Size(475, 24);
            this.CACertPlaceholder.TabIndex = 83;
            // 
            // paramGrid
            // 
            this.paramGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paramGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.paramGrid.Location = new System.Drawing.Point(279, 527);
            this.paramGrid.MainView = this._viewParam;
            this.paramGrid.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.paramGrid.Name = "paramGrid";
            this.paramGrid.Size = new System.Drawing.Size(640, 144);
            this.paramGrid.TabIndex = 23;
            this.paramGrid.TabStop = false;
            this.paramGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this._viewParam,
            this.gridView1});
            // 
            // _viewParam
            // 
            this._viewParam.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.keyGridColumn,
            this.valGridColumn});
            this._viewParam.DetailHeight = 431;
            this._viewParam.GridControl = this.paramGrid;
            this._viewParam.Name = "_viewParam";
            this._viewParam.OptionsBehavior.AutoSelectAllInEditor = false;
            this._viewParam.OptionsBehavior.KeepGroupExpandedOnSorting = false;
            this._viewParam.OptionsCustomization.AllowColumnMoving = false;
            this._viewParam.OptionsCustomization.AllowFilter = false;
            this._viewParam.OptionsCustomization.AllowGroup = false;
            this._viewParam.OptionsCustomization.AllowSort = false;
            this._viewParam.OptionsMenu.EnableColumnMenu = false;
            this._viewParam.OptionsMenu.EnableFooterMenu = false;
            this._viewParam.OptionsMenu.EnableGroupPanelMenu = false;
            this._viewParam.OptionsNavigation.AutoMoveRowFocus = false;
            this._viewParam.OptionsSelection.EnableAppearanceFocusedCell = false;
            this._viewParam.OptionsSelection.EnableAppearanceFocusedRow = false;
            this._viewParam.OptionsSelection.EnableAppearanceHideSelection = false;
            this._viewParam.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this._viewParam.OptionsView.ShowGroupPanel = false;
            // 
            // keyGridColumn
            // 
            this.keyGridColumn.Caption = "gridColumn1";
            this.keyGridColumn.FieldName = "Key";
            this.keyGridColumn.MinWidth = 27;
            this.keyGridColumn.Name = "keyGridColumn";
            this.keyGridColumn.Visible = true;
            this.keyGridColumn.VisibleIndex = 0;
            this.keyGridColumn.Width = 100;
            // 
            // valGridColumn
            // 
            this.valGridColumn.Caption = "gridColumn2";
            this.valGridColumn.FieldName = "Value";
            this.valGridColumn.MinWidth = 27;
            this.valGridColumn.Name = "valGridColumn";
            this.valGridColumn.Visible = true;
            this.valGridColumn.VisibleIndex = 1;
            this.valGridColumn.Width = 100;
            // 
            // gridView1
            // 
            this.gridView1.DetailHeight = 431;
            this.gridView1.GridControl = this.paramGrid;
            this.gridView1.Name = "gridView1";
            // 
            // SASLNameLabel
            // 
            this.SASLNameLabel.AutoSize = true;
            this.SASLNameLabel.Location = new System.Drawing.Point(612, 77);
            this.SASLNameLabel.Name = "SASLNameLabel";
            this.SASLNameLabel.Size = new System.Drawing.Size(77, 17);
            this.SASLNameLabel.TabIndex = 76;
            this.SASLNameLabel.Text = "Username:";
            // 
            // CACertificateLabel
            // 
            this.CACertificateLabel.AutoSize = true;
            this.CACertificateLabel.Location = new System.Drawing.Point(279, 205);
            this.CACertificateLabel.Name = "CACertificateLabel";
            this.CACertificateLabel.Size = new System.Drawing.Size(95, 17);
            this.CACertificateLabel.TabIndex = 82;
            this.CACertificateLabel.Text = "CA certificate:";
            // 
            // addressTextBox
            // 
            this.addressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressTextBox.Location = new System.Drawing.Point(279, 4);
            this.addressTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(639, 22);
            this.addressTextBox.TabIndex = 7;
            // 
            // timeoutNumericUpDown
            // 
            this.timeoutNumericUpDown.Location = new System.Drawing.Point(279, 459);
            this.timeoutNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.timeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeoutNumericUpDown.Name = "timeoutNumericUpDown";
            this.timeoutNumericUpDown.Size = new System.Drawing.Size(55, 22);
            this.timeoutNumericUpDown.TabIndex = 12;
            this.timeoutNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SASLPassLabel
            // 
            this.SASLPassLabel.AutoSize = true;
            this.SASLPassLabel.Location = new System.Drawing.Point(612, 109);
            this.SASLPassLabel.Name = "SASLPassLabel";
            this.SASLPassLabel.Size = new System.Drawing.Size(73, 17);
            this.SASLPassLabel.TabIndex = 77;
            this.SASLPassLabel.Text = "Password:";
            // 
            // SASLPassTextBox
            // 
            this.SASLPassTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SASLPassTextBox.Location = new System.Drawing.Point(728, 106);
            this.SASLPassTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SASLPassTextBox.Name = "SASLPassTextBox";
            this.SASLPassTextBox.PasswordChar = '●';
            this.SASLPassTextBox.Size = new System.Drawing.Size(190, 22);
            this.SASLPassTextBox.TabIndex = 81;
            // 
            // clusterAddressLabel
            // 
            this.clusterAddressLabel.AutoSize = true;
            this.clusterAddressLabel.Location = new System.Drawing.Point(11, 8);
            this.clusterAddressLabel.Name = "clusterAddressLabel";
            this.clusterAddressLabel.Size = new System.Drawing.Size(111, 17);
            this.clusterAddressLabel.TabIndex = 20;
            this.clusterAddressLabel.Text = "Cluster address:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(11, 496);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(157, 17);
            this.label9.TabIndex = 39;
            this.label9.Text = "Acknowledgment mode:";
            // 
            // clientCertificateLabel
            // 
            this.clientCertificateLabel.AutoSize = true;
            this.clientCertificateLabel.Location = new System.Drawing.Point(279, 142);
            this.clientCertificateLabel.Name = "clientCertificateLabel";
            this.clientCertificateLabel.Size = new System.Drawing.Size(112, 17);
            this.clientCertificateLabel.TabIndex = 78;
            this.clientCertificateLabel.Text = "Client certificate:";
            // 
            // SASLNameTextBox
            // 
            this.SASLNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SASLNameTextBox.Location = new System.Drawing.Point(728, 74);
            this.SASLNameTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.SASLNameTextBox.Name = "SASLNameTextBox";
            this.SASLNameTextBox.Size = new System.Drawing.Size(190, 22);
            this.SASLNameTextBox.TabIndex = 80;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(11, 527);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(150, 17);
            this.label10.TabIndex = 42;
            this.label10.Text = "Additional parameters:";
            // 
            // messageTimeoutLabel
            // 
            this.messageTimeoutLabel.AutoSize = true;
            this.messageTimeoutLabel.Location = new System.Drawing.Point(11, 461);
            this.messageTimeoutLabel.Name = "messageTimeoutLabel";
            this.messageTimeoutLabel.Size = new System.Drawing.Size(119, 17);
            this.messageTimeoutLabel.TabIndex = 22;
            this.messageTimeoutLabel.Text = "Message timeout:";
            // 
            // enableSSLVerificationCb
            // 
            this.enableSSLVerificationCb.AutoSize = true;
            this.enableSSLVerificationCb.Location = new System.Drawing.Point(279, 171);
            this.enableSSLVerificationCb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.enableSSLVerificationCb.Name = "enableSSLVerificationCb";
            this.enableSSLVerificationCb.Size = new System.Drawing.Size(176, 21);
            this.enableSSLVerificationCb.TabIndex = 79;
            this.enableSSLVerificationCb.Text = "Enable SSL verification";
            this.enableSSLVerificationCb.UseVisualStyleBackColor = true;
            this.enableSSLVerificationCb.CheckedChanged += new System.EventHandler(this.enableSSLVerificationCb_CheckedChanged);
            // 
            // testConnectionBtn
            // 
            this.testConnectionBtn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.testConnectionBtn.Location = new System.Drawing.Point(540, 39);
            this.testConnectionBtn.Margin = new System.Windows.Forms.Padding(4);
            this.testConnectionBtn.Name = "testConnectionBtn";
            this.testConnectionBtn.Size = new System.Drawing.Size(380, 31);
            this.testConnectionBtn.TabIndex = 56;
            this.testConnectionBtn.Text = "Test connection and populate topic list";
            this.testConnectionBtn.UseVisualStyleBackColor = true;
            this.testConnectionBtn.Click += new System.EventHandler(this.testConnectionButton_Click);
            // 
            // idLabel
            // 
            this.idLabel.AutoSize = true;
            this.idLabel.Location = new System.Drawing.Point(11, 14);
            this.idLabel.Name = "idLabel";
            this.idLabel.Size = new System.Drawing.Size(66, 17);
            this.idLabel.TabIndex = 48;
            this.idLabel.Text = "Identifier:";
            // 
            // identifierTextBox
            // 
            this.identifierTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.identifierTextBox.Location = new System.Drawing.Point(279, 10);
            this.identifierTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.identifierTextBox.Name = "identifierTextBox";
            this.identifierTextBox.Size = new System.Drawing.Size(640, 22);
            this.identifierTextBox.TabIndex = 13;
            // 
            // typeLabel
            // 
            this.typeLabel.AutoSize = true;
            this.typeLabel.Location = new System.Drawing.Point(11, 46);
            this.typeLabel.Name = "typeLabel";
            this.typeLabel.Size = new System.Drawing.Size(44, 17);
            this.typeLabel.TabIndex = 54;
            this.typeLabel.Text = "Type:";
            // 
            // clusterTypeComboBox
            // 
            this.clusterTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.clusterTypeComboBox.FormattingEnabled = true;
            this.clusterTypeComboBox.Items.AddRange(new object[] {
            "Kafka",
            "Event Hub"});
            this.clusterTypeComboBox.Location = new System.Drawing.Point(279, 42);
            this.clusterTypeComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.clusterTypeComboBox.Name = "clusterTypeComboBox";
            this.clusterTypeComboBox.Size = new System.Drawing.Size(144, 24);
            this.clusterTypeComboBox.TabIndex = 58;
            this.clusterTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.typeComboBox_SelectedIndexChanged);
            // 
            // tabConnection
            // 
            this.tabConnection.Controls.Add(this.timestampLabel);
            this.tabConnection.Controls.Add(this.timeGrid);
            this.tabConnection.Controls.Add(this.label1);
            this.tabConnection.Controls.Add(this.signalRefTextBox);
            this.tabConnection.Controls.Add(this.exprGrid);
            this.tabConnection.Controls.Add(this.label8);
            this.tabConnection.Controls.Add(this.expressionAddButton);
            this.tabConnection.Controls.Add(this.m_executeIBAAButton);
            this.tabConnection.Controls.Add(this.label16);
            this.tabConnection.Controls.Add(this.m_browsePDOFileButton);
            this.tabConnection.Controls.Add(this.metadataComboBox);
            this.tabConnection.Controls.Add(this.label13);
            this.tabConnection.Controls.Add(this.expressionRemoveButton);
            this.tabConnection.Controls.Add(this.m_pdoFileTextBox);
            this.tabConnection.Controls.Add(this.keyTextBox);
            this.tabConnection.Controls.Add(this.dataFormatComboBox);
            this.tabConnection.Controls.Add(this.m_btnUploadPDO);
            this.tabConnection.Controls.Add(this.expressionCopyButton);
            this.tabConnection.Controls.Add(this.digitalFormatComboBox);
            this.tabConnection.Controls.Add(this.label15);
            this.tabConnection.Controls.Add(this.m_testButton);
            this.tabConnection.Controls.Add(this.upButton);
            this.tabConnection.Controls.Add(this.label5);
            this.tabConnection.Controls.Add(this.m_browseDatFileButton);
            this.tabConnection.Controls.Add(this.m_datFileTextBox);
            this.tabConnection.Controls.Add(this.topicComboBox);
            this.tabConnection.Controls.Add(this.label154t5);
            this.tabConnection.Controls.Add(this.downButton);
            this.tabConnection.Controls.Add(this.label6);
            this.tabConnection.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabConnection.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabConnection.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabConnection.Location = new System.Drawing.Point(1, 24);
            this.tabConnection.Name = "tabConnection";
            this.tabConnection.Padding = new System.Windows.Forms.Padding(3);
            this.tabConnection.SelectBackColor = System.Drawing.Color.Empty;
            this.tabConnection.Selected = false;
            this.tabConnection.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabConnection.SelectTextColor = System.Drawing.Color.Empty;
            this.tabConnection.Size = new System.Drawing.Size(969, 757);
            this.tabConnection.TabIndex = 1;
            this.tabConnection.Text = "tabConnection";
            this.tabConnection.Title = "Target";
            this.tabConnection.ToolTip = "Page";
            // 
            // timestampLabel
            // 
            this.timestampLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timestampLabel.AutoSize = true;
            this.timestampLabel.Enabled = false;
            this.timestampLabel.Location = new System.Drawing.Point(459, 175);
            this.timestampLabel.Name = "timestampLabel";
            this.timestampLabel.Size = new System.Drawing.Size(81, 17);
            this.timestampLabel.TabIndex = 57;
            this.timestampLabel.Text = "Timestamp:";
            // 
            // timeGrid
            // 
            this.timeGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.timeGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.timeGrid.Enabled = false;
            this.timeGrid.Location = new System.Drawing.Point(547, 170);
            this.timeGrid.MainView = this.m_viewTime;
            this.timeGrid.Margin = new System.Windows.Forms.Padding(4);
            this.timeGrid.Name = "timeGrid";
            this.timeGrid.Size = new System.Drawing.Size(292, 26);
            this.timeGrid.TabIndex = 8;
            this.timeGrid.TabStop = false;
            this.timeGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.m_viewTime});
            // 
            // m_viewTime
            // 
            this.m_viewTime.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.m_colTime});
            this.m_viewTime.DetailHeight = 431;
            this.m_viewTime.GridControl = this.timeGrid;
            this.m_viewTime.Name = "m_viewTime";
            this.m_viewTime.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.m_viewTime.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.m_viewTime.OptionsBehavior.AutoPopulateColumns = false;
            this.m_viewTime.OptionsCustomization.AllowColumnMoving = false;
            this.m_viewTime.OptionsCustomization.AllowColumnResizing = false;
            this.m_viewTime.OptionsCustomization.AllowFilter = false;
            this.m_viewTime.OptionsCustomization.AllowGroup = false;
            this.m_viewTime.OptionsCustomization.AllowQuickHideColumns = false;
            this.m_viewTime.OptionsCustomization.AllowSort = false;
            this.m_viewTime.OptionsFind.AllowFindPanel = false;
            this.m_viewTime.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.m_viewTime.OptionsView.ShowColumnHeaders = false;
            this.m_viewTime.OptionsView.ShowGroupPanel = false;
            this.m_viewTime.OptionsView.ShowIndicator = false;
            // 
            // m_colTime
            // 
            this.m_colTime.Caption = "Pulse signal";
            this.m_colTime.FieldName = "Expression";
            this.m_colTime.MinWidth = 27;
            this.m_colTime.Name = "m_colTime";
            this.m_colTime.Visible = true;
            this.m_colTime.VisibleIndex = 0;
            this.m_colTime.Width = 100;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 142);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(116, 17);
            this.label1.TabIndex = 56;
            this.label1.Text = "Signal reference:";
            // 
            // signalRefTextBox
            // 
            this.signalRefTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.signalRefTextBox.Location = new System.Drawing.Point(199, 138);
            this.signalRefTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.signalRefTextBox.Name = "signalRefTextBox";
            this.signalRefTextBox.Size = new System.Drawing.Size(640, 22);
            this.signalRefTextBox.TabIndex = 55;
            // 
            // exprGrid
            // 
            this.exprGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exprGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.exprGrid.Location = new System.Drawing.Point(11, 281);
            this.exprGrid.MainView = this.dataGV;
            this.exprGrid.Margin = new System.Windows.Forms.Padding(20);
            this.exprGrid.MinimumSize = new System.Drawing.Size(0, 199);
            this.exprGrid.Name = "exprGrid";
            this.exprGrid.Size = new System.Drawing.Size(907, 460);
            this.exprGrid.TabIndex = 17;
            this.exprGrid.TabStop = false;
            this.exprGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataGV,
            this.gridView2});
            // 
            // dataGV
            // 
            this.dataGV.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.expressionGridColumn,
            this.dataTypeGridColumn,
            this.testValueGridColumn,
            this.nameGridColumn});
            this.dataGV.DetailHeight = 431;
            this.dataGV.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.dataGV.GridControl = this.exprGrid;
            this.dataGV.GroupFormat = "";
            this.dataGV.Name = "dataGV";
            this.dataGV.OptionsBehavior.AutoSelectAllInEditor = false;
            this.dataGV.OptionsBehavior.KeepGroupExpandedOnSorting = false;
            this.dataGV.OptionsCustomization.AllowColumnMoving = false;
            this.dataGV.OptionsCustomization.AllowFilter = false;
            this.dataGV.OptionsCustomization.AllowSort = false;
            this.dataGV.OptionsMenu.EnableColumnMenu = false;
            this.dataGV.OptionsMenu.EnableFooterMenu = false;
            this.dataGV.OptionsMenu.EnableGroupPanelMenu = false;
            this.dataGV.OptionsNavigation.AutoMoveRowFocus = false;
            this.dataGV.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.dataGV.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.dataGV.OptionsSelection.EnableAppearanceHideSelection = false;
            this.dataGV.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.dataGV.OptionsView.ShowGroupPanel = false;
            // 
            // expressionGridColumn
            // 
            this.expressionGridColumn.Caption = "gridColumnExpression";
            this.expressionGridColumn.FieldName = "Expression";
            this.expressionGridColumn.MinWidth = 27;
            this.expressionGridColumn.Name = "expressionGridColumn";
            this.expressionGridColumn.Visible = true;
            this.expressionGridColumn.VisibleIndex = 0;
            this.expressionGridColumn.Width = 241;
            // 
            // dataTypeGridColumn
            // 
            this.dataTypeGridColumn.Caption = "dataTypeGridColumn";
            this.dataTypeGridColumn.FieldName = "DataTypeAsString";
            this.dataTypeGridColumn.MinWidth = 27;
            this.dataTypeGridColumn.Name = "dataTypeGridColumn";
            this.dataTypeGridColumn.Visible = true;
            this.dataTypeGridColumn.VisibleIndex = 2;
            this.dataTypeGridColumn.Width = 100;
            // 
            // testValueGridColumn
            // 
            this.testValueGridColumn.Caption = "testValueGridColumn";
            this.testValueGridColumn.FieldName = "TestValue";
            this.testValueGridColumn.MinWidth = 27;
            this.testValueGridColumn.Name = "testValueGridColumn";
            this.testValueGridColumn.Visible = true;
            this.testValueGridColumn.VisibleIndex = 3;
            this.testValueGridColumn.Width = 100;
            // 
            // nameGridColumn
            // 
            this.nameGridColumn.Caption = "gridColumnName";
            this.nameGridColumn.FieldName = "Name";
            this.nameGridColumn.MinWidth = 27;
            this.nameGridColumn.Name = "nameGridColumn";
            this.nameGridColumn.Visible = true;
            this.nameGridColumn.VisibleIndex = 1;
            this.nameGridColumn.Width = 100;
            // 
            // gridView2
            // 
            this.gridView2.DetailHeight = 431;
            this.gridView2.GridControl = this.exprGrid;
            this.gridView2.Name = "gridView2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(11, 14);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(120, 17);
            this.label8.TabIndex = 26;
            this.label8.Text = "Optional analysis:";
            // 
            // expressionAddButton
            // 
            this.expressionAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.expressionAddButton.Image = ((System.Drawing.Image)(resources.GetObject("expressionAddButton.Image")));
            this.expressionAddButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.expressionAddButton.Location = new System.Drawing.Point(927, 281);
            this.expressionAddButton.Margin = new System.Windows.Forms.Padding(4);
            this.expressionAddButton.Name = "expressionAddButton";
            this.expressionAddButton.Size = new System.Drawing.Size(32, 30);
            this.expressionAddButton.TabIndex = 18;
            this.expressionAddButton.UseVisualStyleBackColor = true;
            this.expressionAddButton.Click += new System.EventHandler(this.buttonExpressionAdd_Click);
            // 
            // m_executeIBAAButton
            // 
            this.m_executeIBAAButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_executeIBAAButton.Image = Icons.SystemTray.Images.IbaAnalyzer(16);
            this.m_executeIBAAButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_executeIBAAButton.Location = new System.Drawing.Point(888, 7);
            this.m_executeIBAAButton.Margin = new System.Windows.Forms.Padding(4);
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.Size = new System.Drawing.Size(32, 30);
            this.m_executeIBAAButton.TabIndex = 2;
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(11, 176);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(71, 17);
            this.label16.TabIndex = 54;
            this.label16.Text = "Metadata:";
            // 
            // m_browsePDOFileButton
            // 
            this.m_browsePDOFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browsePDOFileButton.Image = Icons.Gui.All.Images.FolderOpen(16);
            this.m_browsePDOFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browsePDOFileButton.Location = new System.Drawing.Point(848, 7);
            this.m_browsePDOFileButton.Margin = new System.Windows.Forms.Padding(4);
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_browsePDOFileButton.Size = new System.Drawing.Size(32, 30);
            this.m_browsePDOFileButton.TabIndex = 1;
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // metadataComboBox
            // 
            this.metadataComboBox.Location = new System.Drawing.Point(199, 172);
            this.metadataComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.metadataComboBox.Name = "metadataComboBox";
            this.metadataComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.metadataComboBox.Size = new System.Drawing.Size(155, 22);
            this.metadataComboBox.TabIndex = 53;
            this.metadataComboBox.EditValueChanged += new System.EventHandler(this.metadataComboBox_EditValueChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 241);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(140, 17);
            this.label13.TabIndex = 50;
            this.label13.Text = "Digital values format:";
            // 
            // expressionRemoveButton
            // 
            this.expressionRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionRemoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.expressionRemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("expressionRemoveButton.Image")));
            this.expressionRemoveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.expressionRemoveButton.Location = new System.Drawing.Point(927, 713);
            this.expressionRemoveButton.Margin = new System.Windows.Forms.Padding(4);
            this.expressionRemoveButton.Name = "expressionRemoveButton";
            this.expressionRemoveButton.Size = new System.Drawing.Size(32, 30);
            this.expressionRemoveButton.TabIndex = 22;
            this.expressionRemoveButton.UseVisualStyleBackColor = true;
            this.expressionRemoveButton.Click += new System.EventHandler(this.buttonExpressionRemove_Click);
            // 
            // m_pdoFileTextBox
            // 
            this.m_pdoFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_pdoFileTextBox.Location = new System.Drawing.Point(199, 10);
            this.m_pdoFileTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.Size = new System.Drawing.Size(640, 22);
            this.m_pdoFileTextBox.TabIndex = 0;
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
            // 
            // keyTextBox
            // 
            this.keyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyTextBox.Location = new System.Drawing.Point(199, 107);
            this.keyTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.Size = new System.Drawing.Size(640, 22);
            this.keyTextBox.TabIndex = 52;
            // 
            // dataFormatComboBox
            // 
            this.dataFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataFormatComboBox.FormattingEnabled = true;
            this.dataFormatComboBox.Items.AddRange(new object[] {
            "JSON (grouped)",
            "JSON (per signal)",
            "AVRO (per signal)"});
            this.dataFormatComboBox.Location = new System.Drawing.Point(199, 204);
            this.dataFormatComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.dataFormatComboBox.Name = "dataFormatComboBox";
            this.dataFormatComboBox.Size = new System.Drawing.Size(153, 24);
            this.dataFormatComboBox.TabIndex = 10;
            // 
            // m_btnUploadPDO
            // 
            this.m_btnUploadPDO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnUploadPDO.Image = global::iba.Properties.Resources.img_pdo_upload;
            this.m_btnUploadPDO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btnUploadPDO.Location = new System.Drawing.Point(927, 7);
            this.m_btnUploadPDO.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnUploadPDO.Name = "m_btnUploadPDO";
            this.m_btnUploadPDO.Padding = new System.Windows.Forms.Padding(1);
            this.m_btnUploadPDO.Size = new System.Drawing.Size(32, 30);
            this.m_btnUploadPDO.TabIndex = 3;
            this.m_btnUploadPDO.UseVisualStyleBackColor = true;
            this.m_btnUploadPDO.Click += new System.EventHandler(this.m_btnUploadPDO_Click);
            // 
            // expressionCopyButton
            // 
            this.expressionCopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionCopyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.expressionCopyButton.Image = ((System.Drawing.Image)(resources.GetObject("expressionCopyButton.Image")));
            this.expressionCopyButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.expressionCopyButton.Location = new System.Drawing.Point(927, 320);
            this.expressionCopyButton.Margin = new System.Windows.Forms.Padding(4);
            this.expressionCopyButton.Name = "expressionCopyButton";
            this.expressionCopyButton.Size = new System.Drawing.Size(32, 30);
            this.expressionCopyButton.TabIndex = 19;
            this.expressionCopyButton.UseVisualStyleBackColor = true;
            this.expressionCopyButton.Click += new System.EventHandler(this.buttonExpressionCopy_Click);
            // 
            // digitalFormatComboBox
            // 
            this.digitalFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.digitalFormatComboBox.FormattingEnabled = true;
            this.digitalFormatComboBox.Items.AddRange(new object[] {
            "\"True\" / \"False\"",
            "\"1\" / \"0\""});
            this.digitalFormatComboBox.Location = new System.Drawing.Point(199, 238);
            this.digitalFormatComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.digitalFormatComboBox.Name = "digitalFormatComboBox";
            this.digitalFormatComboBox.Size = new System.Drawing.Size(153, 24);
            this.digitalFormatComboBox.TabIndex = 14;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(11, 111);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(36, 17);
            this.label15.TabIndex = 51;
            this.label15.Text = "Key:";
            // 
            // m_testButton
            // 
            this.m_testButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_testButton.Image = Icons.Gui.All.Images.CircleQuestionFilledBlue(16);
            this.m_testButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_testButton.Location = new System.Drawing.Point(888, 39);
            this.m_testButton.Margin = new System.Windows.Forms.Padding(4);
            this.m_testButton.Name = "m_testButton";
            this.m_testButton.Size = new System.Drawing.Size(32, 30);
            this.m_testButton.TabIndex = 6;
            this.m_testButton.UseVisualStyleBackColor = true;
            this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Image = ((System.Drawing.Image)(resources.GetObject("upButton.Image")));
            this.upButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.upButton.Location = new System.Drawing.Point(927, 359);
            this.upButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(32, 30);
            this.upButton.TabIndex = 20;
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 208);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 17);
            this.label5.TabIndex = 24;
            this.label5.Text = "Data format:";
            // 
            // m_browseDatFileButton
            // 
            this.m_browseDatFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseDatFileButton.Image = Icons.Gui.All.Images.FolderOpen(16);
            this.m_browseDatFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseDatFileButton.Location = new System.Drawing.Point(848, 39);
            this.m_browseDatFileButton.Margin = new System.Windows.Forms.Padding(4);
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_browseDatFileButton.Size = new System.Drawing.Size(32, 30);
            this.m_browseDatFileButton.TabIndex = 5;
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
            // 
            // m_datFileTextBox
            // 
            this.m_datFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_datFileTextBox.Location = new System.Drawing.Point(199, 42);
            this.m_datFileTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.m_datFileTextBox.Name = "m_datFileTextBox";
            this.m_datFileTextBox.Size = new System.Drawing.Size(640, 22);
            this.m_datFileTextBox.TabIndex = 4;
            this.m_datFileTextBox.TextChanged += new System.EventHandler(this.m_datFileTextBox_TextChanged);
            // 
            // topicComboBox
            // 
            this.topicComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topicComboBox.FormattingEnabled = true;
            this.topicComboBox.Location = new System.Drawing.Point(199, 74);
            this.topicComboBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.topicComboBox.Name = "topicComboBox";
            this.topicComboBox.Size = new System.Drawing.Size(640, 24);
            this.topicComboBox.TabIndex = 9;
            // 
            // label154t5
            // 
            this.label154t5.AutoSize = true;
            this.label154t5.Location = new System.Drawing.Point(11, 78);
            this.label154t5.Name = "label154t5";
            this.label154t5.Size = new System.Drawing.Size(47, 17);
            this.label154t5.TabIndex = 18;
            this.label154t5.Text = "Topic:";
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Image = ((System.Drawing.Image)(resources.GetObject("downButton.Image")));
            this.downButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.downButton.Location = new System.Drawing.Point(927, 399);
            this.downButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(32, 30);
            this.downButton.TabIndex = 21;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(11, 46);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(115, 17);
            this.label6.TabIndex = 31;
            this.label6.Text = "Example .dat file:";
            // 
            // KafkaWriterTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.m_monitorGroup);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(900, 879);
            this.Name = "KafkaWriterTaskControl";
            this.Size = new System.Drawing.Size(971, 883);
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabTarget.ResumeLayout(false);
            this.tabTarget.PerformLayout();
            this.panelKafka.ResumeLayout(false);
            this.panelKafka.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.paramGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._viewParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericUpDown)).EndInit();
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.exprGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.metadataComboBox.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private iba.Utility.CollapsibleGroupBox m_monitorGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
        private System.Windows.Forms.ToolTip _toolTip;
        private Crownwood.DotNetMagic.Controls.TabControl tabControl1;
        private Crownwood.DotNetMagic.Controls.TabPage tabTarget;
        private System.Windows.Forms.ComboBox CACertPlaceholder;
        private System.Windows.Forms.Label idLabel;
        private System.Windows.Forms.Label CACertificateLabel;
        private System.Windows.Forms.NumericUpDown timeoutNumericUpDown;
        private System.Windows.Forms.TextBox SASLPassTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox SASLNameTextBox;
        private System.Windows.Forms.Label messageTimeoutLabel;
        private System.Windows.Forms.CheckBox enableSSLVerificationCb;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label clientCertificateLabel;
        private System.Windows.Forms.Label clusterAddressLabel;
        private System.Windows.Forms.Label SASLPassLabel;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Label SASLNameLabel;
        private DevExpress.XtraGrid.GridControl paramGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView _viewParam;
        private DevExpress.XtraGrid.Columns.GridColumn keyGridColumn;
        private DevExpress.XtraGrid.Columns.GridColumn valGridColumn;
        private System.Windows.Forms.ComboBox SASLMechanismComboBox;
        private System.Windows.Forms.ComboBox acknowledgmentComboBox;
        private System.Windows.Forms.Label SASLMechLabel;
        private System.Windows.Forms.Button paramAddButton;
        private System.Windows.Forms.ComboBox clientCertPlaceholder;
        private System.Windows.Forms.ComboBox schemaRegistryConnectionSecurityComboBox;
        private System.Windows.Forms.TextBox schemaTextBox;
        private System.Windows.Forms.Label schemaRegSecurityLabel;
        private System.Windows.Forms.Button paramRemoveButton;
        private System.Windows.Forms.ComboBox clusterConnectionSecurityComboBox;
        private System.Windows.Forms.TextBox identifierTextBox;
        private System.Windows.Forms.Label clusterConnSecurityLabel;
        private System.Windows.Forms.Label typeLabel;
        private System.Windows.Forms.ComboBox clusterTypeComboBox;
        private System.Windows.Forms.Button importParamButton;
        private System.Windows.Forms.Button exportParamButton;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private Crownwood.DotNetMagic.Controls.TabPage tabConnection;
        protected DevExpress.XtraGrid.GridControl exprGrid;
        private DevExpress.XtraGrid.Views.Grid.GridView dataGV;
        private DevExpress.XtraGrid.Columns.GridColumn expressionGridColumn;
        private DevExpress.XtraGrid.Columns.GridColumn dataTypeGridColumn;
        private DevExpress.XtraGrid.Columns.GridColumn testValueGridColumn;
        private DevExpress.XtraGrid.Columns.GridColumn nameGridColumn;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button expressionAddButton;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Button m_browsePDOFileButton;
        private DevExpress.XtraEditors.CheckedComboBoxEdit metadataComboBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button expressionRemoveButton;
        private System.Windows.Forms.TextBox m_pdoFileTextBox;
        private System.Windows.Forms.TextBox keyTextBox;
        private System.Windows.Forms.ComboBox dataFormatComboBox;
        private System.Windows.Forms.Button m_btnUploadPDO;
        private System.Windows.Forms.Button expressionCopyButton;
        private System.Windows.Forms.ComboBox digitalFormatComboBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Button m_testButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button m_browseDatFileButton;
        private System.Windows.Forms.TextBox m_datFileTextBox;
        private System.Windows.Forms.ComboBox topicComboBox;
        private System.Windows.Forms.Label label154t5;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Label label6;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
        private System.Windows.Forms.ComboBox schemaCACertPlaceholder;
        private System.Windows.Forms.Label schemaCACertificateLabel;
        private System.Windows.Forms.CheckBox schemaEnableSSLVerificationCb;
        private System.Windows.Forms.Label schemaClientCertificateLabel;
        private System.Windows.Forms.ComboBox schemaClientCertPlaceholder;
        private System.Windows.Forms.TextBox schemaPassTextBox;
        private System.Windows.Forms.TextBox schemaNameTextBox;
        private System.Windows.Forms.Label schemaPassLabel;
        private System.Windows.Forms.Label schemaNameLabel;
        private System.Windows.Forms.Button testConnectionBtn;
        private System.Windows.Forms.CheckBox schemaRegistryCb;
        private System.Windows.Forms.Label secLabel;
        private System.Windows.Forms.ToolTip placeholdersToolTip;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ToolTip placeholdersKeyToolTip;
        private System.Windows.Forms.TextBox signalRefTextBox;
        private System.Windows.Forms.Panel panelKafka;
        private DevExpress.XtraGrid.Views.Grid.GridView m_viewTime;
        private DevExpress.XtraGrid.Columns.GridColumn m_colTime;
        protected DevExpress.XtraGrid.GridControl timeGrid;
        private System.Windows.Forms.Label timestampLabel;
    }
}
