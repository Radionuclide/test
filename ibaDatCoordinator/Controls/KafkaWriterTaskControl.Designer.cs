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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(KafkaWriterTaskControl));
            this.exprGrid = new DevExpress.XtraGrid.GridControl();
            this.dataGV = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.expressionGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.dataTypeGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.testValueGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.nameGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.expressionAddButton = new System.Windows.Forms.Button();
            this.m_monitorGroup = new iba.Utility.CollapsibleGroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            this.topicComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.timeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.dataFormatComboBox = new System.Windows.Forms.ComboBox();
            this.m_browseDatFileButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.m_datFileTextBox = new System.Windows.Forms.TextBox();
            this.m_testButton = new System.Windows.Forms.Button();
            this.m_btnUploadPDO = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browsePDOFileButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.downButton = new System.Windows.Forms.Button();
            this.upButton = new System.Windows.Forms.Button();
            this.expressionCopyButton = new System.Windows.Forms.Button();
            this.expressionRemoveButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.paramGrid = new DevExpress.XtraGrid.GridControl();
            this._viewParam = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.keyGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.valGridColumn = new DevExpress.XtraGrid.Columns.GridColumn();
            this.paramAddButton = new System.Windows.Forms.Button();
            this.paramRemoveButton = new System.Windows.Forms.Button();
            this.acknowledgmentComboBox = new System.Windows.Forms.ComboBox();
            this.label12 = new System.Windows.Forms.Label();
            this.identifierTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.digitalFormatComboBox = new System.Windows.Forms.ComboBox();
            this.testConnectionButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.schemaTextBox = new System.Windows.Forms.TextBox();
            this.connectionGroupBox = new iba.Utility.CollapsibleGroupBox();
            this.exportParamButton = new System.Windows.Forms.Button();
            this.importParamButton = new System.Windows.Forms.Button();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.targetGroupBox = new iba.Utility.CollapsibleGroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.metadataComboBox = new DevExpress.XtraEditors.CheckedComboBoxEdit();
            this.keyTextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this._toolTip = new System.Windows.Forms.ToolTip();
            ((System.ComponentModel.ISupportInitialize)(this.exprGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).BeginInit();
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.paramGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this._viewParam)).BeginInit();
            this.connectionGroupBox.SuspendLayout();
            this.targetGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metadataComboBox.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // exprGrid
            // 
            this.exprGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.exprGrid.Location = new System.Drawing.Point(17, 205);
            this.exprGrid.MainView = this.dataGV;
            this.exprGrid.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.exprGrid.MinimumSize = new System.Drawing.Size(0, 162);
            this.exprGrid.Name = "exprGrid";
            this.exprGrid.Size = new System.Drawing.Size(685, 164);
            this.exprGrid.TabIndex = 17;
            this.exprGrid.TabStop = false;
            this.exprGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataGV});
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
            // expressionAddButton
            // 
            this.expressionAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.expressionAddButton.Image = ((System.Drawing.Image)(resources.GetObject("expressionAddButton.Image")));
            this.expressionAddButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.expressionAddButton.Location = new System.Drawing.Point(707, 205);
            this.expressionAddButton.Name = "expressionAddButton";
            this.expressionAddButton.Size = new System.Drawing.Size(24, 24);
            this.expressionAddButton.TabIndex = 18;
            this.expressionAddButton.UseVisualStyleBackColor = true;
            this.expressionAddButton.Click += new System.EventHandler(this.buttonExpressionAdd_Click);
            // 
            // m_monitorGroup
            // 
            this.m_monitorGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_monitorGroup.Controls.Add(this.label4);
            this.m_monitorGroup.Controls.Add(this.label7);
            this.m_monitorGroup.Controls.Add(this.m_nudTime);
            this.m_monitorGroup.Controls.Add(this.m_nudMemory);
            this.m_monitorGroup.Controls.Add(this.m_cbTime);
            this.m_monitorGroup.Controls.Add(this.m_cbMemory);
            this.m_monitorGroup.Location = new System.Drawing.Point(0, 726);
            this.m_monitorGroup.Name = "m_monitorGroup";
            this.m_monitorGroup.Size = new System.Drawing.Size(742, 71);
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
            // topicComboBox
            // 
            this.topicComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.topicComboBox.FormattingEnabled = true;
            this.topicComboBox.Location = new System.Drawing.Point(148, 66);
            this.topicComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.topicComboBox.Name = "topicComboBox";
            this.topicComboBox.Size = new System.Drawing.Size(582, 21);
            this.topicComboBox.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 69);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Topic:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 78);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 20;
            this.label2.Text = "Cluster address:";
            // 
            // addressTextBox
            // 
            this.addressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressTextBox.Location = new System.Drawing.Point(148, 76);
            this.addressTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(582, 20);
            this.addressTextBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 130);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "Message timeout:";
            // 
            // timeoutNumericUpDown
            // 
            this.timeoutNumericUpDown.Location = new System.Drawing.Point(148, 130);
            this.timeoutNumericUpDown.Margin = new System.Windows.Forms.Padding(2);
            this.timeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeoutNumericUpDown.Name = "timeoutNumericUpDown";
            this.timeoutNumericUpDown.Size = new System.Drawing.Size(51, 20);
            this.timeoutNumericUpDown.TabIndex = 12;
            this.timeoutNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 147);
            this.label5.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(65, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Data format:";
            // 
            // dataFormatComboBox
            // 
            this.dataFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.dataFormatComboBox.FormattingEnabled = true;
            this.dataFormatComboBox.Items.AddRange(new object[] {
            "JSON (grouped)",
            "JSON (per signal)",
            "AVRO (per signal)"});
            this.dataFormatComboBox.Location = new System.Drawing.Point(148, 144);
            this.dataFormatComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.dataFormatComboBox.Name = "dataFormatComboBox";
            this.dataFormatComboBox.Size = new System.Drawing.Size(116, 21);
            this.dataFormatComboBox.TabIndex = 10;
            // 
            // m_browseDatFileButton
            // 
            this.m_browseDatFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseDatFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseDatFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseDatFileButton.Location = new System.Drawing.Point(647, 38);
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_browseDatFileButton.Size = new System.Drawing.Size(24, 24);
            this.m_browseDatFileButton.TabIndex = 5;
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(14, 43);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(84, 13);
            this.label6.TabIndex = 31;
            this.label6.Text = "Example .dat file";
            // 
            // m_datFileTextBox
            // 
            this.m_datFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_datFileTextBox.Location = new System.Drawing.Point(148, 42);
            this.m_datFileTextBox.Name = "m_datFileTextBox";
            this.m_datFileTextBox.Size = new System.Drawing.Size(493, 20);
            this.m_datFileTextBox.TabIndex = 4;
            // 
            // m_testButton
            // 
            this.m_testButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_testButton.Image = global::iba.Properties.Resources.sychronizeList;
            this.m_testButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_testButton.Location = new System.Drawing.Point(677, 38);
            this.m_testButton.Name = "m_testButton";
            this.m_testButton.Size = new System.Drawing.Size(24, 24);
            this.m_testButton.TabIndex = 6;
            this.m_testButton.UseVisualStyleBackColor = true;
            this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
            // 
            // m_btnUploadPDO
            // 
            this.m_btnUploadPDO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnUploadPDO.Image = global::iba.Properties.Resources.img_pdo_upload;
            this.m_btnUploadPDO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btnUploadPDO.Location = new System.Drawing.Point(707, 12);
            this.m_btnUploadPDO.Name = "m_btnUploadPDO";
            this.m_btnUploadPDO.Padding = new System.Windows.Forms.Padding(1);
            this.m_btnUploadPDO.Size = new System.Drawing.Size(24, 24);
            this.m_btnUploadPDO.TabIndex = 3;
            this.m_btnUploadPDO.UseVisualStyleBackColor = true;
            this.m_btnUploadPDO.Click += new System.EventHandler(this.m_btnUploadPDO_Click);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(14, 17);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(89, 13);
            this.label8.TabIndex = 26;
            this.label8.Text = "Optional analysis:";
            // 
            // m_pdoFileTextBox
            // 
            this.m_pdoFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_pdoFileTextBox.Location = new System.Drawing.Point(148, 15);
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.Size = new System.Drawing.Size(493, 20);
            this.m_pdoFileTextBox.TabIndex = 0;
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
            // 
            // m_browsePDOFileButton
            // 
            this.m_browsePDOFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browsePDOFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browsePDOFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browsePDOFileButton.Location = new System.Drawing.Point(647, 12);
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_browsePDOFileButton.Size = new System.Drawing.Size(24, 24);
            this.m_browsePDOFileButton.TabIndex = 1;
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            this.m_executeIBAAButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.ibaAnalyzer_16x16;
            this.m_executeIBAAButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_executeIBAAButton.Location = new System.Drawing.Point(677, 12);
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.Size = new System.Drawing.Size(24, 24);
            this.m_executeIBAAButton.TabIndex = 2;
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // downButton
            // 
            this.downButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.downButton.Image = ((System.Drawing.Image)(resources.GetObject("downButton.Image")));
            this.downButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.downButton.Location = new System.Drawing.Point(707, 301);
            this.downButton.Margin = new System.Windows.Forms.Padding(2);
            this.downButton.Name = "downButton";
            this.downButton.Size = new System.Drawing.Size(24, 24);
            this.downButton.TabIndex = 21;
            this.downButton.UseVisualStyleBackColor = true;
            this.downButton.Click += new System.EventHandler(this.downButton_Click);
            // 
            // upButton
            // 
            this.upButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.upButton.Image = ((System.Drawing.Image)(resources.GetObject("upButton.Image")));
            this.upButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.upButton.Location = new System.Drawing.Point(707, 269);
            this.upButton.Margin = new System.Windows.Forms.Padding(2);
            this.upButton.Name = "upButton";
            this.upButton.Size = new System.Drawing.Size(24, 24);
            this.upButton.TabIndex = 20;
            this.upButton.UseVisualStyleBackColor = true;
            this.upButton.Click += new System.EventHandler(this.upButton_Click);
            // 
            // expressionCopyButton
            // 
            this.expressionCopyButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionCopyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.expressionCopyButton.Image = ((System.Drawing.Image)(resources.GetObject("expressionCopyButton.Image")));
            this.expressionCopyButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.expressionCopyButton.Location = new System.Drawing.Point(707, 237);
            this.expressionCopyButton.Name = "expressionCopyButton";
            this.expressionCopyButton.Size = new System.Drawing.Size(24, 24);
            this.expressionCopyButton.TabIndex = 19;
            this.expressionCopyButton.UseVisualStyleBackColor = true;
            this.expressionCopyButton.Click += new System.EventHandler(this.buttonExpressionCopy_Click);
            // 
            // expressionRemoveButton
            // 
            this.expressionRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.expressionRemoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.expressionRemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("expressionRemoveButton.Image")));
            this.expressionRemoveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.expressionRemoveButton.Location = new System.Drawing.Point(707, 345);
            this.expressionRemoveButton.Name = "expressionRemoveButton";
            this.expressionRemoveButton.Size = new System.Drawing.Size(24, 24);
            this.expressionRemoveButton.TabIndex = 22;
            this.expressionRemoveButton.UseVisualStyleBackColor = true;
            this.expressionRemoveButton.Click += new System.EventHandler(this.buttonExpressionRemove_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(14, 156);
            this.label9.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(121, 13);
            this.label9.TabIndex = 39;
            this.label9.Text = "Acknowledgment mode:";
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(14, 178);
            this.label10.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(111, 13);
            this.label10.TabIndex = 42;
            this.label10.Text = "Additional parameters:";
            // 
            // paramGrid
            // 
            this.paramGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.paramGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(2);
            this.paramGrid.Location = new System.Drawing.Point(14, 198);
            this.paramGrid.MainView = this._viewParam;
            this.paramGrid.Margin = new System.Windows.Forms.Padding(2);
            this.paramGrid.Name = "paramGrid";
            this.paramGrid.Size = new System.Drawing.Size(689, 130);
            this.paramGrid.TabIndex = 23;
            this.paramGrid.TabStop = false;
            this.paramGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this._viewParam});
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
            // paramAddButton
            // 
            this.paramAddButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.paramAddButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.paramAddButton.Image = ((System.Drawing.Image)(resources.GetObject("paramAddButton.Image")));
            this.paramAddButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.paramAddButton.Location = new System.Drawing.Point(707, 198);
            this.paramAddButton.Name = "paramAddButton";
            this.paramAddButton.Size = new System.Drawing.Size(24, 24);
            this.paramAddButton.TabIndex = 24;
            this.paramAddButton.UseVisualStyleBackColor = true;
            this.paramAddButton.Click += new System.EventHandler(this.paramAddButton_Click);
            // 
            // paramRemoveButton
            // 
            this.paramRemoveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.paramRemoveButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.paramRemoveButton.Image = ((System.Drawing.Image)(resources.GetObject("paramRemoveButton.Image")));
            this.paramRemoveButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.paramRemoveButton.Location = new System.Drawing.Point(707, 304);
            this.paramRemoveButton.Name = "paramRemoveButton";
            this.paramRemoveButton.Size = new System.Drawing.Size(24, 24);
            this.paramRemoveButton.TabIndex = 25;
            this.paramRemoveButton.UseVisualStyleBackColor = true;
            this.paramRemoveButton.Click += new System.EventHandler(this.paramRemoveButton_Click);
            // 
            // acknowledgmentComboBox
            // 
            this.acknowledgmentComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.acknowledgmentComboBox.FormattingEnabled = true;
            this.acknowledgmentComboBox.Items.AddRange(new object[] {
            "None",
            "Leader",
            "All"});
            this.acknowledgmentComboBox.Location = new System.Drawing.Point(148, 154);
            this.acknowledgmentComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.acknowledgmentComboBox.Name = "acknowledgmentComboBox";
            this.acknowledgmentComboBox.Size = new System.Drawing.Size(116, 21);
            this.acknowledgmentComboBox.TabIndex = 11;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(14, 26);
            this.label12.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(50, 13);
            this.label12.TabIndex = 48;
            this.label12.Text = "Identifier:";
            // 
            // identifierTextBox
            // 
            this.identifierTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.identifierTextBox.Location = new System.Drawing.Point(148, 26);
            this.identifierTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.identifierTextBox.Name = "identifierTextBox";
            this.identifierTextBox.Size = new System.Drawing.Size(582, 20);
            this.identifierTextBox.TabIndex = 13;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(14, 175);
            this.label13.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(105, 13);
            this.label13.TabIndex = 50;
            this.label13.Text = "Digital values format:";
            // 
            // digitalFormatComboBox
            // 
            this.digitalFormatComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.digitalFormatComboBox.FormattingEnabled = true;
            this.digitalFormatComboBox.Items.AddRange(new object[] {
            "\"True\" / \"False\"",
            "\"1\" / \"0\""});
            this.digitalFormatComboBox.Location = new System.Drawing.Point(148, 173);
            this.digitalFormatComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.digitalFormatComboBox.Name = "digitalFormatComboBox";
            this.digitalFormatComboBox.Size = new System.Drawing.Size(116, 21);
            this.digitalFormatComboBox.TabIndex = 14;
            // 
            // testConnectionButton
            // 
            this.testConnectionButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.testConnectionButton.Location = new System.Drawing.Point(587, 121);
            this.testConnectionButton.Margin = new System.Windows.Forms.Padding(2);
            this.testConnectionButton.Name = "testConnectionButton";
            this.testConnectionButton.Size = new System.Drawing.Size(143, 21);
            this.testConnectionButton.TabIndex = 8;
            this.testConnectionButton.Text = "Test connection";
            this.testConnectionButton.UseVisualStyleBackColor = true;
            this.testConnectionButton.Click += new System.EventHandler(this.testConnectionButton_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(14, 104);
            this.label11.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 13);
            this.label11.TabIndex = 53;
            this.label11.Text = "Schema registry address:";
            // 
            // schemaTextBox
            // 
            this.schemaTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.schemaTextBox.Location = new System.Drawing.Point(148, 104);
            this.schemaTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.schemaTextBox.Name = "schemaTextBox";
            this.schemaTextBox.Size = new System.Drawing.Size(582, 20);
            this.schemaTextBox.TabIndex = 16;
            // 
            // connectionGroupBox
            // 
            this.connectionGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.connectionGroupBox.Controls.Add(this.exportParamButton);
            this.connectionGroupBox.Controls.Add(this.importParamButton);
            this.connectionGroupBox.Controls.Add(this.label17);
            this.connectionGroupBox.Controls.Add(this.label14);
            this.connectionGroupBox.Controls.Add(this.identifierTextBox);
            this.connectionGroupBox.Controls.Add(this.paramRemoveButton);
            this.connectionGroupBox.Controls.Add(this.schemaTextBox);
            this.connectionGroupBox.Controls.Add(this.label11);
            this.connectionGroupBox.Controls.Add(this.paramAddButton);
            this.connectionGroupBox.Controls.Add(this.acknowledgmentComboBox);
            this.connectionGroupBox.Controls.Add(this.label12);
            this.connectionGroupBox.Controls.Add(this.paramGrid);
            this.connectionGroupBox.Controls.Add(this.addressTextBox);
            this.connectionGroupBox.Controls.Add(this.label2);
            this.connectionGroupBox.Controls.Add(this.label10);
            this.connectionGroupBox.Controls.Add(this.label3);
            this.connectionGroupBox.Controls.Add(this.label9);
            this.connectionGroupBox.Controls.Add(this.timeoutNumericUpDown);
            this.connectionGroupBox.Location = new System.Drawing.Point(0, 0);
            this.connectionGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.connectionGroupBox.Name = "connectionGroupBox";
            this.connectionGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.connectionGroupBox.Size = new System.Drawing.Size(742, 339);
            this.connectionGroupBox.TabIndex = 54;
            this.connectionGroupBox.TabStop = false;
            this.connectionGroupBox.Text = "Connection";
            // 
            // exportParamButton
            // 
            this.exportParamButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.exportParamButton.Image = global::iba.Properties.Resources.img_export;
            this.exportParamButton.Location = new System.Drawing.Point(707, 262);
            this.exportParamButton.Name = "exportParamButton";
            this.exportParamButton.Size = new System.Drawing.Size(24, 24);
            this.exportParamButton.TabIndex = 57;
            this.exportParamButton.UseVisualStyleBackColor = true;
            this.exportParamButton.Click += new System.EventHandler(this.OnExportParameters);
            // 
            // importParamButton
            // 
            this.importParamButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.importParamButton.Image = global::iba.Properties.Resources.img_import;
            this.importParamButton.Location = new System.Drawing.Point(707, 230);
            this.importParamButton.Name = "importParamButton";
            this.importParamButton.Size = new System.Drawing.Size(24, 24);
            this.importParamButton.TabIndex = 56;
            this.importParamButton.UseVisualStyleBackColor = true;
            this.importParamButton.Click += new System.EventHandler(this.OnImportParameters);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label17.ForeColor = System.Drawing.SystemColors.GrayText;
            this.label17.Location = new System.Drawing.Point(148, 52);
            this.label17.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(101, 13);
            this.label17.TabIndex = 55;
            this.label17.Text = "not implemented yet";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(14, 52);
            this.label14.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(34, 13);
            this.label14.TabIndex = 54;
            this.label14.Text = "Type:";
            // 
            // targetGroupBox
            // 
            this.targetGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.targetGroupBox.Controls.Add(this.exprGrid);
            this.targetGroupBox.Controls.Add(this.expressionAddButton);
            this.targetGroupBox.Controls.Add(this.label16);
            this.targetGroupBox.Controls.Add(this.metadataComboBox);
            this.targetGroupBox.Controls.Add(this.expressionRemoveButton);
            this.targetGroupBox.Controls.Add(this.keyTextBox);
            this.targetGroupBox.Controls.Add(this.testConnectionButton);
            this.targetGroupBox.Controls.Add(this.expressionCopyButton);
            this.targetGroupBox.Controls.Add(this.label15);
            this.targetGroupBox.Controls.Add(this.upButton);
            this.targetGroupBox.Controls.Add(this.m_browseDatFileButton);
            this.targetGroupBox.Controls.Add(this.topicComboBox);
            this.targetGroupBox.Controls.Add(this.downButton);
            this.targetGroupBox.Controls.Add(this.label6);
            this.targetGroupBox.Controls.Add(this.label1);
            this.targetGroupBox.Controls.Add(this.m_datFileTextBox);
            this.targetGroupBox.Controls.Add(this.label5);
            this.targetGroupBox.Controls.Add(this.m_testButton);
            this.targetGroupBox.Controls.Add(this.digitalFormatComboBox);
            this.targetGroupBox.Controls.Add(this.m_btnUploadPDO);
            this.targetGroupBox.Controls.Add(this.dataFormatComboBox);
            this.targetGroupBox.Controls.Add(this.label8);
            this.targetGroupBox.Controls.Add(this.m_pdoFileTextBox);
            this.targetGroupBox.Controls.Add(this.label13);
            this.targetGroupBox.Controls.Add(this.m_browsePDOFileButton);
            this.targetGroupBox.Controls.Add(this.m_executeIBAAButton);
            this.targetGroupBox.Location = new System.Drawing.Point(0, 343);
            this.targetGroupBox.Margin = new System.Windows.Forms.Padding(2);
            this.targetGroupBox.Name = "targetGroupBox";
            this.targetGroupBox.Padding = new System.Windows.Forms.Padding(2);
            this.targetGroupBox.Size = new System.Drawing.Size(742, 380);
            this.targetGroupBox.TabIndex = 55;
            this.targetGroupBox.TabStop = false;
            this.targetGroupBox.Text = "Target";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(14, 121);
            this.label16.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(55, 13);
            this.label16.TabIndex = 54;
            this.label16.Text = "Metadata:";
            // 
            // metadataComboBox
            // 
            this.metadataComboBox.Location = new System.Drawing.Point(148, 118);
            this.metadataComboBox.Margin = new System.Windows.Forms.Padding(2);
            this.metadataComboBox.Name = "metadataComboBox";
            this.metadataComboBox.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.metadataComboBox.Size = new System.Drawing.Size(116, 20);
            this.metadataComboBox.TabIndex = 53;
            // 
            // keyTextBox
            // 
            this.keyTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.keyTextBox.Location = new System.Drawing.Point(148, 92);
            this.keyTextBox.Margin = new System.Windows.Forms.Padding(2);
            this.keyTextBox.Name = "keyTextBox";
            this.keyTextBox.Size = new System.Drawing.Size(582, 20);
            this.keyTextBox.TabIndex = 52;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(14, 95);
            this.label15.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(28, 13);
            this.label15.TabIndex = 51;
            this.label15.Text = "Key:";
            // 
            // KafkaWriterTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.connectionGroupBox);
            this.Controls.Add(this.targetGroupBox);
            this.Controls.Add(this.m_monitorGroup);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(0, 800);
            this.Name = "KafkaWriterTaskControl";
            this.Size = new System.Drawing.Size(742, 800);
            ((System.ComponentModel.ISupportInitialize)(this.exprGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).EndInit();
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.paramGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this._viewParam)).EndInit();
            this.connectionGroupBox.ResumeLayout(false);
            this.connectionGroupBox.PerformLayout();
            this.targetGroupBox.ResumeLayout(false);
            this.targetGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.metadataComboBox.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button expressionAddButton;
        private iba.Utility.CollapsibleGroupBox m_monitorGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
        private System.Windows.Forms.ComboBox topicComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown timeoutNumericUpDown;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox dataFormatComboBox;
        private System.Windows.Forms.Button m_browseDatFileButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox m_datFileTextBox;
        private System.Windows.Forms.Button m_testButton;
        private System.Windows.Forms.Button m_btnUploadPDO;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox m_pdoFileTextBox;
        private System.Windows.Forms.Button m_browsePDOFileButton;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private DevExpress.XtraGrid.Views.Grid.GridView dataGV;
        private DevExpress.XtraGrid.Columns.GridColumn expressionGridColumn;
        private System.Windows.Forms.Button downButton;
        private System.Windows.Forms.Button upButton;
        private System.Windows.Forms.Button expressionCopyButton;
        private System.Windows.Forms.Button expressionRemoveButton;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private DevExpress.XtraGrid.GridControl paramGrid;
        private DevExpress.XtraGrid.Columns.GridColumn keyGridColumn;
        private DevExpress.XtraGrid.Columns.GridColumn valGridColumn;
        private System.Windows.Forms.Button paramAddButton;
        private System.Windows.Forms.Button paramRemoveButton;
        private System.Windows.Forms.ComboBox acknowledgmentComboBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox identifierTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox digitalFormatComboBox;
        private System.Windows.Forms.Button testConnectionButton;
        private DevExpress.XtraGrid.Columns.GridColumn dataTypeGridColumn;
        private DevExpress.XtraGrid.Columns.GridColumn testValueGridColumn;
        private DevExpress.XtraGrid.Columns.GridColumn nameGridColumn;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox schemaTextBox;
        private iba.Utility.CollapsibleGroupBox connectionGroupBox;
        private System.Windows.Forms.Label label14;
        private iba.Utility.CollapsibleGroupBox targetGroupBox;
        private System.Windows.Forms.TextBox keyTextBox;
        private System.Windows.Forms.Label label15;
        private DevExpress.XtraEditors.CheckedComboBoxEdit metadataComboBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Button exportParamButton;
        private System.Windows.Forms.Button importParamButton;
        private System.Windows.Forms.ToolTip _toolTip;
    }
}
