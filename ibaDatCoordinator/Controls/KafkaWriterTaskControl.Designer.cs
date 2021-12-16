﻿namespace iba.Controls
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
            this.schemaCACertPlaceholder = new System.Windows.Forms.ComboBox();
            this.schemaCACertificateLabel = new System.Windows.Forms.Label();
            this.schemaEnableSSLVerificationCb = new System.Windows.Forms.CheckBox();
            this.schemaClientCertificateLabel = new System.Windows.Forms.Label();
            this.schemaClientCertPlaceholder = new System.Windows.Forms.ComboBox();
            this.schemaPassTextBox = new System.Windows.Forms.TextBox();
            this.schemaNameTextBox = new System.Windows.Forms.TextBox();
            this.schemaPassLabel = new System.Windows.Forms.Label();
            this.schemaNameLabel = new System.Windows.Forms.Label();
            this.testConnectionButton = new System.Windows.Forms.Button();
            this.CACertPlaceholder = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.CACertificateLabel = new System.Windows.Forms.Label();
            this.timeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.SASLPassTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.SASLNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.enableSSLVerificationCb = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.clientCertificateLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SASLPassLabel = new System.Windows.Forms.Label();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.SASLNameLabel = new System.Windows.Forms.Label();
            this.paramGrid = new DevExpress.XtraGrid.GridControl();
            this._viewParam = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.keyGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.valGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.SASLMechanismComboBox = new System.Windows.Forms.ComboBox();
            this.acknowledgmentComboBox = new System.Windows.Forms.ComboBox();
            this.SASLMechLabel = new System.Windows.Forms.Label();
            this.paramAddButton = new System.Windows.Forms.Button();
            this.clientCertPlaceholder = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.schemaRegistryConnectionSecurityComboBox = new System.Windows.Forms.ComboBox();
            this.schemaTextBox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.paramRemoveButton = new System.Windows.Forms.Button();
            this.clusterConnectionSecurityComboBox = new System.Windows.Forms.ComboBox();
            this.identifierTextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.typeComboBox = new System.Windows.Forms.ComboBox();
            this.importParamButton = new System.Windows.Forms.Button();
            this.exportParamButton = new System.Windows.Forms.Button();
            this.tabConnection = new Crownwood.DotNetMagic.Controls.TabPage();
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
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabTarget.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paramGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._viewParam)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.tabConnection.SuspendLayout();
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
            this.m_monitorGroup.Location = new System.Drawing.Point(3, 730);
            this.m_monitorGroup.Name = "m_monitorGroup";
            this.m_monitorGroup.Size = new System.Drawing.Size(797, 71);
            this.m_monitorGroup.TabIndex = 26;
            this.m_monitorGroup.TabStop = false;
            this.m_monitorGroup.Text = "Monitor ibaAnalyzer";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(412, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "minutes to complete";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(412, 24);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Mbytes of memory";
            // 
            // m_nudTime
            // 
            this.m_nudTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudTime.Location = new System.Drawing.Point(328, 46);
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
            this.m_nudTime.Size = new System.Drawing.Size(78, 20);
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
            this.m_nudMemory.Location = new System.Drawing.Point(328, 20);
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
            this.m_nudMemory.Size = new System.Drawing.Size(78, 20);
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
            this.m_cbTime.Location = new System.Drawing.Point(15, 46);
            this.m_cbTime.Name = "m_cbTime";
            this.m_cbTime.Size = new System.Drawing.Size(266, 17);
            this.m_cbTime.TabIndex = 3;
            this.m_cbTime.Text = "Time limit: abort task if ibaAnalyzer takes more than";
            this.m_cbTime.UseVisualStyleBackColor = true;
            // 
            // m_cbMemory
            // 
            this.m_cbMemory.AutoSize = true;
            this.m_cbMemory.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_cbMemory.Location = new System.Drawing.Point(15, 22);
            this.m_cbMemory.Name = "m_cbMemory";
            this.m_cbMemory.Size = new System.Drawing.Size(307, 17);
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
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.MediaPlayerDockSides = false;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.OfficeDockSides = false;
            this.tabControl1.OfficeStyle = Crownwood.DotNetMagic.Controls.OfficeStyle.SoftWhite;
            this.tabControl1.SelectedIndex = 1;
            this.tabControl1.ShowArrows = false;
            this.tabControl1.ShowClose = false;
            this.tabControl1.ShowDropSelect = false;
            this.tabControl1.Size = new System.Drawing.Size(797, 725);
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
            this.tabTarget.Controls.Add(this.schemaCACertPlaceholder);
            this.tabTarget.Controls.Add(this.schemaCACertificateLabel);
            this.tabTarget.Controls.Add(this.schemaEnableSSLVerificationCb);
            this.tabTarget.Controls.Add(this.schemaClientCertificateLabel);
            this.tabTarget.Controls.Add(this.schemaClientCertPlaceholder);
            this.tabTarget.Controls.Add(this.schemaPassTextBox);
            this.tabTarget.Controls.Add(this.schemaNameTextBox);
            this.tabTarget.Controls.Add(this.schemaPassLabel);
            this.tabTarget.Controls.Add(this.schemaNameLabel);
            this.tabTarget.Controls.Add(this.testConnectionButton);
            this.tabTarget.Controls.Add(this.CACertPlaceholder);
            this.tabTarget.Controls.Add(this.label12);
            this.tabTarget.Controls.Add(this.CACertificateLabel);
            this.tabTarget.Controls.Add(this.timeoutNumericUpDown);
            this.tabTarget.Controls.Add(this.SASLPassTextBox);
            this.tabTarget.Controls.Add(this.label9);
            this.tabTarget.Controls.Add(this.SASLNameTextBox);
            this.tabTarget.Controls.Add(this.label3);
            this.tabTarget.Controls.Add(this.enableSSLVerificationCb);
            this.tabTarget.Controls.Add(this.label10);
            this.tabTarget.Controls.Add(this.clientCertificateLabel);
            this.tabTarget.Controls.Add(this.label2);
            this.tabTarget.Controls.Add(this.SASLPassLabel);
            this.tabTarget.Controls.Add(this.addressTextBox);
            this.tabTarget.Controls.Add(this.SASLNameLabel);
            this.tabTarget.Controls.Add(this.paramGrid);
            this.tabTarget.Controls.Add(this.SASLMechanismComboBox);
            this.tabTarget.Controls.Add(this.acknowledgmentComboBox);
            this.tabTarget.Controls.Add(this.SASLMechLabel);
            this.tabTarget.Controls.Add(this.paramAddButton);
            this.tabTarget.Controls.Add(this.clientCertPlaceholder);
            this.tabTarget.Controls.Add(this.label11);
            this.tabTarget.Controls.Add(this.schemaRegistryConnectionSecurityComboBox);
            this.tabTarget.Controls.Add(this.schemaTextBox);
            this.tabTarget.Controls.Add(this.label18);
            this.tabTarget.Controls.Add(this.paramRemoveButton);
            this.tabTarget.Controls.Add(this.clusterConnectionSecurityComboBox);
            this.tabTarget.Controls.Add(this.identifierTextBox);
            this.tabTarget.Controls.Add(this.label17);
            this.tabTarget.Controls.Add(this.label14);
            this.tabTarget.Controls.Add(this.typeComboBox);
            this.tabTarget.Controls.Add(this.importParamButton);
            this.tabTarget.Controls.Add(this.exportParamButton);
            this.tabTarget.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabTarget.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabTarget.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabTarget.Location = new System.Drawing.Point(1, 24);
            this.tabTarget.Name = "tabTarget";
            this.tabTarget.Padding = new System.Windows.Forms.Padding(3);
            this.tabTarget.SelectBackColor = System.Drawing.Color.Empty;
            this.tabTarget.Selected = false;
            this.tabTarget.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabTarget.SelectTextColor = System.Drawing.Color.Empty;
            this.tabTarget.Size = new System.Drawing.Size(795, 700);
            this.tabTarget.TabIndex = 0;
            this.tabTarget.Text = "tabTarget";
            this.tabTarget.Title = "Connection";
            this.tabTarget.ToolTip = "Page";
            // 
            // schemaCACertPlaceholder
            // 
            this.schemaCACertPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaCACertPlaceholder.FormattingEnabled = true;
            this.schemaCACertPlaceholder.Location = new System.Drawing.Point(351, 403);
            this.schemaCACertPlaceholder.Margin = new System.Windows.Forms.Padding(2);
            this.schemaCACertPlaceholder.Name = "schemaCACertPlaceholder";
            this.schemaCACertPlaceholder.Size = new System.Drawing.Size(354, 21);
            this.schemaCACertPlaceholder.TabIndex = 93;
            // 
            // schemaCACertificateLabel
            // 
            this.schemaCACertificateLabel.AutoSize = true;
            this.schemaCACertificateLabel.Location = new System.Drawing.Point(228, 406);
            this.schemaCACertificateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.schemaCACertificateLabel.Name = "schemaCACertificateLabel";
            this.schemaCACertificateLabel.Size = new System.Drawing.Size(73, 13);
            this.schemaCACertificateLabel.TabIndex = 92;
            this.schemaCACertificateLabel.Text = "CA certificate:";
            // 
            // schemaEnableSSLVerificationCb
            // 
            this.schemaEnableSSLVerificationCb.AutoSize = true;
            this.schemaEnableSSLVerificationCb.Location = new System.Drawing.Point(230, 380);
            this.schemaEnableSSLVerificationCb.Margin = new System.Windows.Forms.Padding(2);
            this.schemaEnableSSLVerificationCb.Name = "schemaEnableSSLVerificationCb";
            this.schemaEnableSSLVerificationCb.Size = new System.Drawing.Size(136, 17);
            this.schemaEnableSSLVerificationCb.TabIndex = 91;
            this.schemaEnableSSLVerificationCb.Text = "Enable SSL verification";
            this.schemaEnableSSLVerificationCb.UseVisualStyleBackColor = true;
            // 
            // schemaClientCertificateLabel
            // 
            this.schemaClientCertificateLabel.AutoSize = true;
            this.schemaClientCertificateLabel.Location = new System.Drawing.Point(228, 356);
            this.schemaClientCertificateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.schemaClientCertificateLabel.Name = "schemaClientCertificateLabel";
            this.schemaClientCertificateLabel.Size = new System.Drawing.Size(85, 13);
            this.schemaClientCertificateLabel.TabIndex = 90;
            this.schemaClientCertificateLabel.Text = "Client certificate:";
            // 
            // schemaClientCertPlaceholder
            // 
            this.schemaClientCertPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaClientCertPlaceholder.FormattingEnabled = true;
            this.schemaClientCertPlaceholder.Location = new System.Drawing.Point(351, 353);
            this.schemaClientCertPlaceholder.Margin = new System.Windows.Forms.Padding(2);
            this.schemaClientCertPlaceholder.Name = "schemaClientCertPlaceholder";
            this.schemaClientCertPlaceholder.Size = new System.Drawing.Size(354, 21);
            this.schemaClientCertPlaceholder.TabIndex = 89;
            // 
            // schemaPassTextBox
            // 
            this.schemaPassTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaPassTextBox.Location = new System.Drawing.Point(305, 327);
            this.schemaPassTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.schemaPassTextBox.Name = "schemaPassTextBox";
            this.schemaPassTextBox.PasswordChar = '●';
            this.schemaPassTextBox.Size = new System.Drawing.Size(224, 20);
            this.schemaPassTextBox.TabIndex = 88;
            // 
            // schemaNameTextBox
            // 
            this.schemaNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaNameTextBox.Location = new System.Drawing.Point(305, 301);
            this.schemaNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.schemaNameTextBox.Name = "schemaNameTextBox";
            this.schemaNameTextBox.Size = new System.Drawing.Size(224, 20);
            this.schemaNameTextBox.TabIndex = 87;
            // 
            // schemaPassLabel
            // 
            this.schemaPassLabel.AutoSize = true;
            this.schemaPassLabel.Location = new System.Drawing.Point(228, 330);
            this.schemaPassLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.schemaPassLabel.Name = "schemaPassLabel";
            this.schemaPassLabel.Size = new System.Drawing.Size(56, 13);
            this.schemaPassLabel.TabIndex = 86;
            this.schemaPassLabel.Text = "Password:";
            // 
            // schemaNameLabel
            // 
            this.schemaNameLabel.AutoSize = true;
            this.schemaNameLabel.Location = new System.Drawing.Point(228, 304);
            this.schemaNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.schemaNameLabel.Name = "schemaNameLabel";
            this.schemaNameLabel.Size = new System.Drawing.Size(58, 13);
            this.schemaNameLabel.TabIndex = 85;
            this.schemaNameLabel.Text = "Username:";
            // 
            // testConnectionButton
            // 
            this.testConnectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.testConnectionButton.Location = new System.Drawing.Point(57, 896);
            this.testConnectionButton.Margin = new System.Windows.Forms.Padding(2);
            this.testConnectionButton.Name = "testConnectionButton";
            this.testConnectionButton.Size = new System.Drawing.Size(143, 21);
            this.testConnectionButton.TabIndex = 84;
            this.testConnectionButton.Text = "Test connection";
            this.testConnectionButton.UseVisualStyleBackColor = true;
            // 
            // CACertPlaceholder
            // 
            this.CACertPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CACertPlaceholder.FormattingEnabled = true;
            this.CACertPlaceholder.Location = new System.Drawing.Point(364, 221);
            this.CACertPlaceholder.Margin = new System.Windows.Forms.Padding(2);
            this.CACertPlaceholder.Name = "CACertPlaceholder";
            this.CACertPlaceholder.Size = new System.Drawing.Size(425, 21);
            this.CACertPlaceholder.TabIndex = 83;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 11);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "Identifier:";
            // 
            // CACertificateLabel
            // 
            this.CACertificateLabel.AutoSize = true;
            this.CACertificateLabel.Location = new System.Drawing.Point(227, 224);
            this.CACertificateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.CACertificateLabel.Name = "CACertificateLabel";
            this.CACertificateLabel.Size = new System.Drawing.Size(73, 13);
            this.CACertificateLabel.TabIndex = 82;
            this.CACertificateLabel.Text = "CA certificate:";
            // 
            // timeoutNumericUpDown
            // 
            this.timeoutNumericUpDown.Location = new System.Drawing.Point(229, 430);
            this.timeoutNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.timeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeoutNumericUpDown.Name = "timeoutNumericUpDown";
            this.timeoutNumericUpDown.Size = new System.Drawing.Size(41, 20);
            this.timeoutNumericUpDown.TabIndex = 12;
            this.timeoutNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // SASLPassTextBox
            // 
            this.SASLPassTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SASLPassTextBox.Location = new System.Drawing.Point(565, 144);
            this.SASLPassTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.SASLPassTextBox.Name = "SASLPassTextBox";
            this.SASLPassTextBox.PasswordChar = '●';
            this.SASLPassTextBox.Size = new System.Drawing.Size(224, 20);
            this.SASLPassTextBox.TabIndex = 81;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 459);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 13);
            this.label9.TabIndex = 39;
            this.label9.Text = "Acknowledgment mode:";
            // 
            // SASLNameTextBox
            // 
            this.SASLNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SASLNameTextBox.Location = new System.Drawing.Point(565, 118);
            this.SASLNameTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.SASLNameTextBox.Name = "SASLNameTextBox";
            this.SASLNameTextBox.Size = new System.Drawing.Size(224, 20);
            this.SASLNameTextBox.TabIndex = 80;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 432);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Message timeout:";
            // 
            // enableSSLVerificationCb
            // 
            this.enableSSLVerificationCb.AutoSize = true;
            this.enableSSLVerificationCb.Location = new System.Drawing.Point(230, 197);
            this.enableSSLVerificationCb.Margin = new System.Windows.Forms.Padding(2);
            this.enableSSLVerificationCb.Name = "enableSSLVerificationCb";
            this.enableSSLVerificationCb.Size = new System.Drawing.Size(136, 17);
            this.enableSSLVerificationCb.TabIndex = 79;
            this.enableSSLVerificationCb.Text = "Enable SSL verification";
            this.enableSSLVerificationCb.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(8, 483);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(111, 13);
            this.label10.TabIndex = 42;
            this.label10.Text = "Additional parameters:";
            // 
            // clientCertificateLabel
            // 
            this.clientCertificateLabel.AutoSize = true;
            this.clientCertificateLabel.Location = new System.Drawing.Point(228, 173);
            this.clientCertificateLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.clientCertificateLabel.Name = "clientCertificateLabel";
            this.clientCertificateLabel.Size = new System.Drawing.Size(85, 13);
            this.clientCertificateLabel.TabIndex = 78;
            this.clientCertificateLabel.Text = "Client certificate:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(8, 64);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Cluster address:";
            // 
            // SASLPassLabel
            // 
            this.SASLPassLabel.AutoSize = true;
            this.SASLPassLabel.Location = new System.Drawing.Point(478, 147);
            this.SASLPassLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SASLPassLabel.Name = "SASLPassLabel";
            this.SASLPassLabel.Size = new System.Drawing.Size(56, 13);
            this.SASLPassLabel.TabIndex = 77;
            this.SASLPassLabel.Text = "Password:";
            // 
            // addressTextBox
            // 
            this.addressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressTextBox.Location = new System.Drawing.Point(228, 61);
            this.addressTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(465, 20);
            this.addressTextBox.TabIndex = 7;
            // 
            // SASLNameLabel
            // 
            this.SASLNameLabel.AutoSize = true;
            this.SASLNameLabel.Location = new System.Drawing.Point(478, 121);
            this.SASLNameLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SASLNameLabel.Name = "SASLNameLabel";
            this.SASLNameLabel.Size = new System.Drawing.Size(58, 13);
            this.SASLNameLabel.TabIndex = 76;
            this.SASLNameLabel.Text = "Username:";
            // 
            // paramGrid
            // 
            this.paramGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paramGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.paramGrid.Location = new System.Drawing.Point(228, 483);
            this.paramGrid.MainView = this._viewParam;
            this.paramGrid.Margin = new System.Windows.Forms.Padding(2);
            this.paramGrid.Name = "paramGrid";
            this.paramGrid.Size = new System.Drawing.Size(532, 210);
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
            this.keyGridColumn.Name = "keyGridColumn";
            this.keyGridColumn.Visible = true;
            this.keyGridColumn.VisibleIndex = 0;
            // 
            // valGridColumn
            // 
            this.valGridColumn.Caption = "gridColumn2";
            this.valGridColumn.FieldName = "Value";
            this.valGridColumn.Name = "valGridColumn";
            this.valGridColumn.Visible = true;
            this.valGridColumn.VisibleIndex = 1;
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.paramGrid;
            this.gridView1.Name = "gridView1";
            // 
            // SASLMechanismComboBox
            // 
            this.SASLMechanismComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SASLMechanismComboBox.FormattingEnabled = true;
            this.SASLMechanismComboBox.Items.AddRange(new object[] {
            "PLAIN",
            "SCRAM-SHA-256",
            "SCRAM-SHA-512"});
            this.SASLMechanismComboBox.Location = new System.Drawing.Point(364, 118);
            this.SASLMechanismComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.SASLMechanismComboBox.Name = "SASLMechanismComboBox";
            this.SASLMechanismComboBox.Size = new System.Drawing.Size(109, 21);
            this.SASLMechanismComboBox.TabIndex = 75;
            // 
            // acknowledgmentComboBox
            // 
            this.acknowledgmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.acknowledgmentComboBox.FormattingEnabled = true;
            this.acknowledgmentComboBox.Items.AddRange(new object[] {
            "None",
            "Leader",
            "All"});
            this.acknowledgmentComboBox.Location = new System.Drawing.Point(228, 456);
            this.acknowledgmentComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.acknowledgmentComboBox.Name = "acknowledgmentComboBox";
            this.acknowledgmentComboBox.Size = new System.Drawing.Size(121, 21);
            this.acknowledgmentComboBox.TabIndex = 11;
            // 
            // SASLMechLabel
            // 
            this.SASLMechLabel.AutoSize = true;
            this.SASLMechLabel.Location = new System.Drawing.Point(228, 121);
            this.SASLMechLabel.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.SASLMechLabel.Name = "SASLMechLabel";
            this.SASLMechLabel.Size = new System.Drawing.Size(93, 13);
            this.SASLMechLabel.TabIndex = 74;
            this.SASLMechLabel.Text = "SASL mechanism:";
            // 
            // paramAddButton
            // 
            this.paramAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.paramAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.paramAddButton.Image = ((System.Drawing.Image)(resources.GetObject("paramAddButton.Image")));
            this.paramAddButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.paramAddButton.Location = new System.Drawing.Point(765, 483);
            this.paramAddButton.Name = "paramAddButton";
            this.paramAddButton.Size = new System.Drawing.Size(24, 24);
            this.paramAddButton.TabIndex = 24;
            this.paramAddButton.UseVisualStyleBackColor = true;
            this.paramAddButton.Click += new System.EventHandler(this.paramAddButton_Click);
            // 
            // clientCertPlaceholder
            // 
            this.clientCertPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clientCertPlaceholder.FormattingEnabled = true;
            this.clientCertPlaceholder.Location = new System.Drawing.Point(364, 170);
            this.clientCertPlaceholder.Margin = new System.Windows.Forms.Padding(2);
            this.clientCertPlaceholder.Name = "clientCertPlaceholder";
            this.clientCertPlaceholder.Size = new System.Drawing.Size(425, 21);
            this.clientCertPlaceholder.TabIndex = 63;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(8, 251);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 13);
            this.label11.TabIndex = 53;
            this.label11.Text = "Schema registry address:";
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
            this.schemaRegistryConnectionSecurityComboBox.Location = new System.Drawing.Point(228, 274);
            this.schemaRegistryConnectionSecurityComboBox.Name = "schemaRegistryConnectionSecurityComboBox";
            this.schemaRegistryConnectionSecurityComboBox.Size = new System.Drawing.Size(140, 21);
            this.schemaRegistryConnectionSecurityComboBox.TabIndex = 62;
            this.schemaRegistryConnectionSecurityComboBox.SelectedIndexChanged += new System.EventHandler(this.schemaRegistryConnectionSecurityComboBox_SelectedIndexChanged);
            // 
            // schemaTextBox
            // 
            this.schemaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaTextBox.Location = new System.Drawing.Point(228, 248);
            this.schemaTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.schemaTextBox.Name = "schemaTextBox";
            this.schemaTextBox.Size = new System.Drawing.Size(465, 20);
            this.schemaTextBox.TabIndex = 16;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(8, 277);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(180, 13);
            this.label18.TabIndex = 61;
            this.label18.Text = "Schema registry connection security:";
            // 
            // paramRemoveButton
            // 
            this.paramRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.paramRemoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.paramRemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("paramRemoveButton.Image")));
            this.paramRemoveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.paramRemoveButton.Location = new System.Drawing.Point(765, 669);
            this.paramRemoveButton.Name = "paramRemoveButton";
            this.paramRemoveButton.Size = new System.Drawing.Size(24, 24);
            this.paramRemoveButton.TabIndex = 25;
            this.paramRemoveButton.UseVisualStyleBackColor = true;
            this.paramRemoveButton.Click += new System.EventHandler(this.paramRemoveButton_Click);
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
            this.clusterConnectionSecurityComboBox.Location = new System.Drawing.Point(228, 89);
            this.clusterConnectionSecurityComboBox.Name = "clusterConnectionSecurityComboBox";
            this.clusterConnectionSecurityComboBox.Size = new System.Drawing.Size(136, 21);
            this.clusterConnectionSecurityComboBox.TabIndex = 60;
            this.clusterConnectionSecurityComboBox.SelectedIndexChanged += new System.EventHandler(this.clusterConnectionSecurityComboBox_SelectedIndexChanged);
            // 
            // identifierTextBox
            // 
            this.identifierTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.identifierTextBox.Location = new System.Drawing.Point(228, 8);
            this.identifierTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.identifierTextBox.Name = "identifierTextBox";
            this.identifierTextBox.Size = new System.Drawing.Size(465, 20);
            this.identifierTextBox.TabIndex = 13;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(8, 92);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(137, 13);
            this.label17.TabIndex = 59;
            this.label17.Text = "Cluster connection security:";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(8, 37);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(34, 13);
            this.label14.TabIndex = 54;
            this.label14.Text = "Type:";
            // 
            // typeComboBox
            // 
            this.typeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.typeComboBox.FormattingEnabled = true;
            this.typeComboBox.Items.AddRange(new object[] {
            "Kafka",
            "Event Hub"});
            this.typeComboBox.Location = new System.Drawing.Point(228, 34);
            this.typeComboBox.Name = "typeComboBox";
            this.typeComboBox.Size = new System.Drawing.Size(135, 21);
            this.typeComboBox.TabIndex = 58;
            // 
            // importParamButton
            // 
            this.importParamButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importParamButton.Image = global::iba.Properties.Resources.img_import;
            this.importParamButton.Location = new System.Drawing.Point(765, 514);
            this.importParamButton.Name = "importParamButton";
            this.importParamButton.Size = new System.Drawing.Size(24, 24);
            this.importParamButton.TabIndex = 56;
            this.importParamButton.UseVisualStyleBackColor = true;
            this.importParamButton.Click += new System.EventHandler(this.OnImportParameters);
            // 
            // exportParamButton
            // 
            this.exportParamButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportParamButton.Image = global::iba.Properties.Resources.img_export;
            this.exportParamButton.Location = new System.Drawing.Point(765, 545);
            this.exportParamButton.Name = "exportParamButton";
            this.exportParamButton.Size = new System.Drawing.Size(24, 24);
            this.exportParamButton.TabIndex = 57;
            this.exportParamButton.UseVisualStyleBackColor = true;
            this.exportParamButton.Click += new System.EventHandler(this.OnExportParameters);
            // 
            // tabConnection
            // 
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
            this.tabConnection.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabConnection.SelectTextColor = System.Drawing.Color.Empty;
            this.tabConnection.Size = new System.Drawing.Size(795, 700);
            this.tabConnection.TabIndex = 1;
            this.tabConnection.Text = "tabConnection";
            this.tabConnection.Title = "Target";
            this.tabConnection.ToolTip = "Page";
            // 
            // exprGrid
            // 
            this.exprGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exprGrid.Location = new System.Drawing.Point(8, 201);
            this.exprGrid.MainView = this.dataGV;
            this.exprGrid.Margin = new System.Windows.Forms.Padding(15, 16, 15, 16);
            this.exprGrid.MinimumSize = new System.Drawing.Size(0, 162);
            this.exprGrid.Name = "exprGrid";
            this.exprGrid.Size = new System.Drawing.Size(749, 491);
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
            this.expressionGridColumn.Name = "expressionGridColumn";
            this.expressionGridColumn.Visible = true;
            this.expressionGridColumn.VisibleIndex = 0;
            this.expressionGridColumn.Width = 181;
            // 
            // dataTypeGridColumn
            // 
            this.dataTypeGridColumn.Caption = "dataTypeGridColumn";
            this.dataTypeGridColumn.FieldName = "DataTypeAsString";
            this.dataTypeGridColumn.Name = "dataTypeGridColumn";
            this.dataTypeGridColumn.Visible = true;
            this.dataTypeGridColumn.VisibleIndex = 2;
            // 
            // testValueGridColumn
            // 
            this.testValueGridColumn.Caption = "testValueGridColumn";
            this.testValueGridColumn.FieldName = "TestValue";
            this.testValueGridColumn.Name = "testValueGridColumn";
            this.testValueGridColumn.Visible = true;
            this.testValueGridColumn.VisibleIndex = 3;
            // 
            // nameGridColumn
            // 
            this.nameGridColumn.Caption = "gridColumnName";
            this.nameGridColumn.FieldName = "Name";
            this.nameGridColumn.Name = "nameGridColumn";
            this.nameGridColumn.Visible = true;
            this.nameGridColumn.VisibleIndex = 1;
            // 
            // gridView2
            // 
            this.gridView2.GridControl = this.exprGrid;
            this.gridView2.Name = "gridView2";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(8, 11);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Optional analysis:";
            // 
            // expressionAddButton
            // 
            this.expressionAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.expressionAddButton.Image = ((System.Drawing.Image)(resources.GetObject("expressionAddButton.Image")));
            this.expressionAddButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.expressionAddButton.Location = new System.Drawing.Point(765, 201);
            this.expressionAddButton.Name = "expressionAddButton";
            this.expressionAddButton.Size = new System.Drawing.Size(24, 24);
            this.expressionAddButton.TabIndex = 18;
            this.expressionAddButton.UseVisualStyleBackColor = true;
            this.expressionAddButton.Click += new System.EventHandler(this.buttonExpressionAdd_Click);
            // 
            // m_executeIBAAButton
            // 
            this.m_executeIBAAButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.ibaAnalyzer_16x16;
            this.m_executeIBAAButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_executeIBAAButton.Location = new System.Drawing.Point(736, 6);
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.Size = new System.Drawing.Size(24, 24);
            this.m_executeIBAAButton.TabIndex = 2;
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(8, 116);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(55, 13);
            this.label16.TabIndex = 54;
            this.label16.Text = "Metadata:";
            // 
            // m_browsePDOFileButton
            // 
            this.m_browsePDOFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browsePDOFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browsePDOFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browsePDOFileButton.Location = new System.Drawing.Point(706, 6);
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_browsePDOFileButton.Size = new System.Drawing.Size(24, 24);
            this.m_browsePDOFileButton.TabIndex = 1;
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // metadataComboBox
            // 
            this.metadataComboBox.Location = new System.Drawing.Point(149, 113);
            this.metadataComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.metadataComboBox.Name = "metadataComboBox";
            this.metadataComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.metadataComboBox.Size = new System.Drawing.Size(116, 20);
            this.metadataComboBox.TabIndex = 53;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(8, 169);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(105, 13);
            this.label13.TabIndex = 50;
            this.label13.Text = "Digital values format:";
            // 
            // expressionRemoveButton
            // 
            this.expressionRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionRemoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.expressionRemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("expressionRemoveButton.Image")));
            this.expressionRemoveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.expressionRemoveButton.Location = new System.Drawing.Point(765, 668);
            this.expressionRemoveButton.Name = "expressionRemoveButton";
            this.expressionRemoveButton.Size = new System.Drawing.Size(24, 24);
            this.expressionRemoveButton.TabIndex = 22;
            this.expressionRemoveButton.UseVisualStyleBackColor = true;
            this.expressionRemoveButton.Click += new System.EventHandler(this.buttonExpressionRemove_Click);
            // 
            // m_pdoFileTextBox
            // 
            this.m_pdoFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_pdoFileTextBox.Location = new System.Drawing.Point(149, 8);
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.Size = new System.Drawing.Size(551, 20);
            this.m_pdoFileTextBox.TabIndex = 0;
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
            // 
            // keyTextBox
            // 
            this.keyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyTextBox.Location = new System.Drawing.Point(149, 87);
            this.keyTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.Size = new System.Drawing.Size(457, 20);
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
            this.dataFormatComboBox.Location = new System.Drawing.Point(149, 139);
            this.dataFormatComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.dataFormatComboBox.Name = "dataFormatComboBox";
            this.dataFormatComboBox.Size = new System.Drawing.Size(116, 21);
            this.dataFormatComboBox.TabIndex = 10;
            // 
            // m_btnUploadPDO
            // 
            this.m_btnUploadPDO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnUploadPDO.Image = global::iba.Properties.Resources.img_pdo_upload;
            this.m_btnUploadPDO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btnUploadPDO.Location = new System.Drawing.Point(765, 6);
            this.m_btnUploadPDO.Name = "m_btnUploadPDO";
            this.m_btnUploadPDO.Padding = new System.Windows.Forms.Padding(1);
            this.m_btnUploadPDO.Size = new System.Drawing.Size(24, 24);
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
            this.expressionCopyButton.Location = new System.Drawing.Point(765, 233);
            this.expressionCopyButton.Name = "expressionCopyButton";
            this.expressionCopyButton.Size = new System.Drawing.Size(24, 24);
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
            this.digitalFormatComboBox.Location = new System.Drawing.Point(149, 166);
            this.digitalFormatComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.digitalFormatComboBox.Name = "digitalFormatComboBox";
            this.digitalFormatComboBox.Size = new System.Drawing.Size(116, 21);
            this.digitalFormatComboBox.TabIndex = 14;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(8, 90);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(28, 13);
            this.label15.TabIndex = 51;
            this.label15.Text = "Key:";
            // 
            // m_testButton
            // 
            this.m_testButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_testButton.Image = global::iba.Properties.Resources.sychronizeList;
            this.m_testButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_testButton.Location = new System.Drawing.Point(736, 32);
            this.m_testButton.Name = "m_testButton";
            this.m_testButton.Size = new System.Drawing.Size(24, 24);
            this.m_testButton.TabIndex = 6;
            this.m_testButton.UseVisualStyleBackColor = true;
            this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Image = ((System.Drawing.Image)(resources.GetObject("upButton.Image")));
            this.upButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.upButton.Location = new System.Drawing.Point(765, 265);
            this.upButton.Margin = new System.Windows.Forms.Padding(2);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(24, 24);
            this.upButton.TabIndex = 20;
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(8, 142);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Data format:";
            // 
            // m_browseDatFileButton
            // 
            this.m_browseDatFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseDatFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseDatFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseDatFileButton.Location = new System.Drawing.Point(706, 32);
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_browseDatFileButton.Size = new System.Drawing.Size(24, 24);
            this.m_browseDatFileButton.TabIndex = 5;
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
            // 
            // m_datFileTextBox
            // 
            this.m_datFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_datFileTextBox.Location = new System.Drawing.Point(149, 34);
            this.m_datFileTextBox.Name = "m_datFileTextBox";
            this.m_datFileTextBox.Size = new System.Drawing.Size(551, 20);
            this.m_datFileTextBox.TabIndex = 4;
            // 
            // topicComboBox
            // 
            this.topicComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topicComboBox.FormattingEnabled = true;
            this.topicComboBox.Location = new System.Drawing.Point(149, 60);
            this.topicComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.topicComboBox.Name = "topicComboBox";
            this.topicComboBox.Size = new System.Drawing.Size(551, 21);
            this.topicComboBox.TabIndex = 9;
            // 
            // label154t5
            // 
            this.label154t5.AutoSize = true;
            this.label154t5.Location = new System.Drawing.Point(8, 63);
            this.label154t5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label154t5.Name = "label154t5";
            this.label154t5.Size = new System.Drawing.Size(37, 13);
            this.label154t5.TabIndex = 18;
            this.label154t5.Text = "Topic:";
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Image = ((System.Drawing.Image)(resources.GetObject("downButton.Image")));
            this.downButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.downButton.Location = new System.Drawing.Point(765, 297);
            this.downButton.Margin = new System.Windows.Forms.Padding(2);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(24, 24);
            this.downButton.TabIndex = 21;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(8, 37);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(87, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Example .dat file:";
            // 
            // KafkaWriterTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.m_monitorGroup);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(675, 738);
            this.Name = "KafkaWriterTaskControl";
            this.Size = new System.Drawing.Size(797, 810);
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabTarget.ResumeLayout(false);
            this.tabTarget.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paramGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._viewParam)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.tabConnection.ResumeLayout(false);
            this.tabConnection.PerformLayout();
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
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label CACertificateLabel;
        private System.Windows.Forms.NumericUpDown timeoutNumericUpDown;
        private System.Windows.Forms.TextBox SASLPassTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox SASLNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox enableSSLVerificationCb;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label clientCertificateLabel;
        private System.Windows.Forms.Label label2;
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
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox schemaRegistryConnectionSecurityComboBox;
        private System.Windows.Forms.TextBox schemaTextBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Button paramRemoveButton;
        private System.Windows.Forms.ComboBox clusterConnectionSecurityComboBox;
        private System.Windows.Forms.TextBox identifierTextBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox typeComboBox;
        private System.Windows.Forms.Button importParamButton;
        private System.Windows.Forms.Button exportParamButton;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
        private System.Windows.Forms.Button testConnectionButton;
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
    }
}
