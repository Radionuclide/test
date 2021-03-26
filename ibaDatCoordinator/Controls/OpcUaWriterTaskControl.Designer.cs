namespace iba.Controls
{
	partial class OpcUaWriterTaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpcUaWriterTaskControl));
            this.dataGrid = new DevExpress.XtraGrid.GridControl();
            this.dataGV = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumnName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumnExpression = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBoxS7DataTypes = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.buttonExpressionCopy = new System.Windows.Forms.Button();
            this.buttonExpressionRemove = new System.Windows.Forms.Button();
            this.buttonExpressionAdd = new System.Windows.Forms.Button();
            this.m_btnUploadPDO = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browsePDOFileButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.m_browseDatFileButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_datFileTextBox = new System.Windows.Forms.TextBox();
            this.m_testButton = new System.Windows.Forms.Button();
            this.m_monitorGroup = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxS7DataTypes)).BeginInit();
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrid
            // 
            this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid.Location = new System.Drawing.Point(15, 80);
            this.dataGrid.MainView = this.dataGV;
            this.dataGrid.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.dataGrid.MinimumSize = new System.Drawing.Size(0, 162);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBoxS7DataTypes});
            this.dataGrid.Size = new System.Drawing.Size(550, 162);
            this.dataGrid.TabIndex = 23;
            this.dataGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataGV});
            // 
            // dataGV
            // 
            this.dataGV.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumnName,
            this.gridColumn11,
            this.gridColumnExpression,
            this.gridColumn1});
            this.dataGV.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.dataGV.GridControl = this.dataGrid;
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
            // gridColumnName
            // 
            this.gridColumnName.Caption = "OPC UA variable name";
            this.gridColumnName.FieldName = "Name";
            this.gridColumnName.Name = "gridColumnName";
            this.gridColumnName.Visible = true;
            this.gridColumnName.VisibleIndex = 1;
            this.gridColumnName.Width = 93;
            // 
            // gridColumn11
            // 
            this.gridColumn11.Caption = "Test Value";
            this.gridColumn11.FieldName = "TestValueString";
            this.gridColumn11.Name = "gridColumn11";
            this.gridColumn11.OptionsColumn.AllowEdit = false;
            this.gridColumn11.OptionsColumn.ReadOnly = true;
            this.gridColumn11.Visible = true;
            this.gridColumn11.VisibleIndex = 3;
            this.gridColumn11.Width = 113;
            // 
            // gridColumnExpression
            // 
            this.gridColumnExpression.Caption = "ibaAnalyzer Expression";
            this.gridColumnExpression.FieldName = "Expression";
            this.gridColumnExpression.Name = "gridColumnExpression";
            this.gridColumnExpression.Visible = true;
            this.gridColumnExpression.VisibleIndex = 0;
            this.gridColumnExpression.Width = 181;
            // 
            // gridColumn1
            // 
            this.gridColumn1.Caption = "gridColumn1";
            this.gridColumn1.FieldName = "DataTypeAsString";
            this.gridColumn1.Name = "gridColumn1";
            this.gridColumn1.Visible = true;
            this.gridColumn1.VisibleIndex = 2;
            // 
            // repositoryItemComboBoxS7DataTypes
            // 
            this.repositoryItemComboBoxS7DataTypes.AutoHeight = false;
            this.repositoryItemComboBoxS7DataTypes.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
            this.repositoryItemComboBoxS7DataTypes.DropDownRows = 8;
            this.repositoryItemComboBoxS7DataTypes.Items.AddRange(new object[] {
            "BOOL",
            "BYTE",
            "CHAR",
            "WORD",
            "INT",
            "DWORD",
            "DINT",
            "REAL"});
            this.repositoryItemComboBoxS7DataTypes.Name = "repositoryItemComboBoxS7DataTypes";
            // 
            // buttonExpressionCopy
            // 
            this.buttonExpressionCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExpressionCopy.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.buttonExpressionCopy.Image = ((System.Drawing.Image)(resources.GetObject("buttonExpressionCopy.Image")));
            this.buttonExpressionCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonExpressionCopy.Location = new System.Drawing.Point(571, 110);
            this.buttonExpressionCopy.Name = "buttonExpressionCopy";
            this.buttonExpressionCopy.Size = new System.Drawing.Size(24, 24);
            this.buttonExpressionCopy.TabIndex = 28;
            this.buttonExpressionCopy.TabStop = false;
            this.buttonExpressionCopy.UseVisualStyleBackColor = true;
            this.buttonExpressionCopy.Click += new System.EventHandler(this.buttonExpressionCopy_Click);
            // 
            // buttonExpressionRemove
            // 
            this.buttonExpressionRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExpressionRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.buttonExpressionRemove.Image = ((System.Drawing.Image)(resources.GetObject("buttonExpressionRemove.Image")));
            this.buttonExpressionRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonExpressionRemove.Location = new System.Drawing.Point(571, 218);
            this.buttonExpressionRemove.Name = "buttonExpressionRemove";
            this.buttonExpressionRemove.Size = new System.Drawing.Size(24, 24);
            this.buttonExpressionRemove.TabIndex = 27;
            this.buttonExpressionRemove.TabStop = false;
            this.buttonExpressionRemove.UseVisualStyleBackColor = true;
            this.buttonExpressionRemove.Click += new System.EventHandler(this.buttonExpressionRemove_Click);
            // 
            // buttonExpressionAdd
            // 
            this.buttonExpressionAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonExpressionAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.buttonExpressionAdd.Image = ((System.Drawing.Image)(resources.GetObject("buttonExpressionAdd.Image")));
            this.buttonExpressionAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonExpressionAdd.Location = new System.Drawing.Point(571, 80);
            this.buttonExpressionAdd.Name = "buttonExpressionAdd";
            this.buttonExpressionAdd.Size = new System.Drawing.Size(24, 24);
            this.buttonExpressionAdd.TabIndex = 26;
            this.buttonExpressionAdd.TabStop = false;
            this.buttonExpressionAdd.UseVisualStyleBackColor = true;
            this.buttonExpressionAdd.Click += new System.EventHandler(this.buttonExpressionAdd_Click);
            // 
            // m_btnUploadPDO
            // 
            this.m_btnUploadPDO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnUploadPDO.Image = global::iba.Properties.Resources.img_pdo_upload;
            this.m_btnUploadPDO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btnUploadPDO.Location = new System.Drawing.Point(571, 11);
            this.m_btnUploadPDO.Name = "m_btnUploadPDO";
            this.m_btnUploadPDO.Padding = new System.Windows.Forms.Padding(1);
            this.m_btnUploadPDO.Size = new System.Drawing.Size(24, 24);
            this.m_btnUploadPDO.TabIndex = 33;
            this.m_btnUploadPDO.UseVisualStyleBackColor = true;
            this.m_btnUploadPDO.Click += new System.EventHandler(this.m_btnUploadPDO_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(3, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 30;
            this.label2.Text = "Optional analysis:";
            // 
            // m_pdoFileTextBox
            // 
            this.m_pdoFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_pdoFileTextBox.Location = new System.Drawing.Point(128, 15);
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.Size = new System.Drawing.Size(378, 20);
            this.m_pdoFileTextBox.TabIndex = 29;
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
            // 
            // m_browsePDOFileButton
            // 
            this.m_browsePDOFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browsePDOFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browsePDOFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browsePDOFileButton.Location = new System.Drawing.Point(511, 11);
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_browsePDOFileButton.Size = new System.Drawing.Size(24, 24);
            this.m_browsePDOFileButton.TabIndex = 31;
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            this.m_executeIBAAButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.ibaAnalyzer_16x16;
            this.m_executeIBAAButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_executeIBAAButton.Location = new System.Drawing.Point(541, 11);
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.Size = new System.Drawing.Size(24, 24);
            this.m_executeIBAAButton.TabIndex = 32;
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // m_browseDatFileButton
            // 
            this.m_browseDatFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseDatFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseDatFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseDatFileButton.Location = new System.Drawing.Point(511, 40);
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_browseDatFileButton.Size = new System.Drawing.Size(24, 24);
            this.m_browseDatFileButton.TabIndex = 37;
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(3, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 38;
            this.label3.Text = "Example .dat file";
            // 
            // m_datFileTextBox
            // 
            this.m_datFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_datFileTextBox.Location = new System.Drawing.Point(128, 42);
            this.m_datFileTextBox.Name = "m_datFileTextBox";
            this.m_datFileTextBox.Size = new System.Drawing.Size(378, 20);
            this.m_datFileTextBox.TabIndex = 36;
            this.m_datFileTextBox.TextChanged += new System.EventHandler(this.m_datFileTextBox_TextChanged);
            // 
            // m_testButton
            // 
            this.m_testButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_testButton.Image = global::iba.Properties.Resources.sychronizeList;
            this.m_testButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_testButton.Location = new System.Drawing.Point(541, 40);
            this.m_testButton.Name = "m_testButton";
            this.m_testButton.Size = new System.Drawing.Size(24, 24);
            this.m_testButton.TabIndex = 39;
            this.m_testButton.UseVisualStyleBackColor = true;
            this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
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
            this.m_monitorGroup.Location = new System.Drawing.Point(3, 255);
            this.m_monitorGroup.Name = "m_monitorGroup";
            this.m_monitorGroup.Size = new System.Drawing.Size(592, 73);
            this.m_monitorGroup.TabIndex = 40;
            this.m_monitorGroup.TabStop = false;
            this.m_monitorGroup.Text = "Monitor ibaAnalyzer";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(412, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "minutes to complete";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(412, 20);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(92, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Mbytes of memory";
            // 
            // m_nudTime
            // 
            this.m_nudTime.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudTime.Location = new System.Drawing.Point(328, 44);
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
            this.m_nudTime.TabIndex = 1;
            this.m_nudTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // m_nudMemory
            // 
            this.m_nudMemory.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudMemory.Location = new System.Drawing.Point(328, 18);
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
            this.m_nudMemory.TabIndex = 0;
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
            this.m_cbTime.Location = new System.Drawing.Point(15, 42);
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
            this.m_cbMemory.Location = new System.Drawing.Point(15, 19);
            this.m_cbMemory.Name = "m_cbMemory";
            this.m_cbMemory.Size = new System.Drawing.Size(307, 17);
            this.m_cbMemory.TabIndex = 0;
            this.m_cbMemory.Text = "Memory limit: abort task if ibaAnalyzer starts using more than";
            this.m_cbMemory.UseVisualStyleBackColor = true;
            // 
            // OpcUaWriterTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_monitorGroup);
            this.Controls.Add(this.m_browseDatFileButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_datFileTextBox);
            this.Controls.Add(this.m_testButton);
            this.Controls.Add(this.m_btnUploadPDO);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_pdoFileTextBox);
            this.Controls.Add(this.m_browsePDOFileButton);
            this.Controls.Add(this.m_executeIBAAButton);
            this.Controls.Add(this.buttonExpressionCopy);
            this.Controls.Add(this.buttonExpressionRemove);
            this.Controls.Add(this.buttonExpressionAdd);
            this.Controls.Add(this.dataGrid);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MinimumSize = new System.Drawing.Size(0, 325);
            this.Name = "OpcUaWriterTaskControl";
            this.Size = new System.Drawing.Size(598, 346);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxS7DataTypes)).EndInit();
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		protected DevExpress.XtraGrid.GridControl dataGrid;
		private DevExpress.XtraGrid.Views.Grid.GridView dataGV;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumnExpression;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumnName;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxS7DataTypes;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
		private System.Windows.Forms.Button buttonExpressionCopy;
		private System.Windows.Forms.Button buttonExpressionRemove;
		private System.Windows.Forms.Button buttonExpressionAdd;
		private System.Windows.Forms.Button m_btnUploadPDO;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox m_pdoFileTextBox;
		private System.Windows.Forms.Button m_browsePDOFileButton;
		private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Button m_browseDatFileButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_datFileTextBox;
        private System.Windows.Forms.Button m_testButton;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        private System.Windows.Forms.GroupBox m_monitorGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
    }
}
