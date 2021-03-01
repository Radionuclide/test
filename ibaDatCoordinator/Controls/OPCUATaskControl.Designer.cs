namespace iba.Controls
{
	partial class OPCUAWriterTaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OPCUAWriterTaskControl));
            this.dataGrid = new DevExpress.XtraGrid.GridControl();
            this.dataGV = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.repositoryItemComboBoxS7DataTypes = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            this.buttonEndpointCopy = new System.Windows.Forms.Button();
            this.buttonEndpointRemove = new System.Windows.Forms.Button();
            this.buttonEndpointAdd = new System.Windows.Forms.Button();
            this.m_btnUploadPDO = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browsePDOFileButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_folderNameTextBox = new System.Windows.Forms.TextBox();
            this.m_browseDatFileButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_datFileTextBox = new System.Windows.Forms.TextBox();
            this.m_testButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxS7DataTypes)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGrid
            // 
            this.dataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGrid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(4);
            this.dataGrid.Location = new System.Drawing.Point(20, 98);
            this.dataGrid.MainView = this.dataGV;
            this.dataGrid.Margin = new System.Windows.Forms.Padding(20, 4, 20, 4);
            this.dataGrid.MinimumSize = new System.Drawing.Size(0, 100);
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBoxS7DataTypes});
            this.dataGrid.Size = new System.Drawing.Size(705, 140);
            this.dataGrid.TabIndex = 23;
            this.dataGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataGV});
            // 
            // dataGV
            // 
            this.dataGV.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn10,
            this.gridColumn11,
            this.gridColumn6,
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
            // gridColumn10
            // 
            this.gridColumn10.Caption = "OPC UA variable name";
            this.gridColumn10.FieldName = "Name";
            this.gridColumn10.Name = "gridColumn10";
            this.gridColumn10.Visible = true;
            this.gridColumn10.VisibleIndex = 1;
            this.gridColumn10.Width = 93;
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
            // gridColumn6
            // 
            this.gridColumn6.Caption = "ibaAnalyzer Expression";
            this.gridColumn6.FieldName = "Expression";
            this.gridColumn6.Name = "gridColumn6";
            this.gridColumn6.Visible = true;
            this.gridColumn6.VisibleIndex = 0;
            this.gridColumn6.Width = 181;
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
            // buttonEndpointCopy
            // 
            this.buttonEndpointCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEndpointCopy.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonEndpointCopy.Image = ((System.Drawing.Image)(resources.GetObject("buttonEndpointCopy.Image")));
            this.buttonEndpointCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonEndpointCopy.Location = new System.Drawing.Point(733, 135);
            this.buttonEndpointCopy.Margin = new System.Windows.Forms.Padding(4);
            this.buttonEndpointCopy.Name = "buttonEndpointCopy";
            this.buttonEndpointCopy.Size = new System.Drawing.Size(32, 30);
            this.buttonEndpointCopy.TabIndex = 28;
            this.buttonEndpointCopy.TabStop = false;
            this.buttonEndpointCopy.UseVisualStyleBackColor = true;
            this.buttonEndpointCopy.Click += new System.EventHandler(this.buttonEndpointCopy_Click);
            // 
            // buttonEndpointRemove
            // 
            this.buttonEndpointRemove.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEndpointRemove.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonEndpointRemove.Image = ((System.Drawing.Image)(resources.GetObject("buttonEndpointRemove.Image")));
            this.buttonEndpointRemove.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonEndpointRemove.Location = new System.Drawing.Point(733, 210);
            this.buttonEndpointRemove.Margin = new System.Windows.Forms.Padding(4);
            this.buttonEndpointRemove.Name = "buttonEndpointRemove";
            this.buttonEndpointRemove.Size = new System.Drawing.Size(32, 30);
            this.buttonEndpointRemove.TabIndex = 27;
            this.buttonEndpointRemove.TabStop = false;
            this.buttonEndpointRemove.UseVisualStyleBackColor = true;
            this.buttonEndpointRemove.Click += new System.EventHandler(this.buttonEndpointRemove_Click);
            // 
            // buttonEndpointAdd
            // 
            this.buttonEndpointAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEndpointAdd.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonEndpointAdd.Image = ((System.Drawing.Image)(resources.GetObject("buttonEndpointAdd.Image")));
            this.buttonEndpointAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonEndpointAdd.Location = new System.Drawing.Point(733, 98);
            this.buttonEndpointAdd.Margin = new System.Windows.Forms.Padding(4);
            this.buttonEndpointAdd.Name = "buttonEndpointAdd";
            this.buttonEndpointAdd.Size = new System.Drawing.Size(32, 30);
            this.buttonEndpointAdd.TabIndex = 26;
            this.buttonEndpointAdd.TabStop = false;
            this.buttonEndpointAdd.UseVisualStyleBackColor = true;
            this.buttonEndpointAdd.Click += new System.EventHandler(this.buttonEndpointAdd_Click);
            // 
            // m_btnUploadPDO
            // 
            this.m_btnUploadPDO.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btnUploadPDO.Image = global::iba.Properties.Resources.img_pdo_upload;
            this.m_btnUploadPDO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btnUploadPDO.Location = new System.Drawing.Point(733, 38);
            this.m_btnUploadPDO.Margin = new System.Windows.Forms.Padding(4);
            this.m_btnUploadPDO.Name = "m_btnUploadPDO";
            this.m_btnUploadPDO.Padding = new System.Windows.Forms.Padding(1);
            this.m_btnUploadPDO.Size = new System.Drawing.Size(32, 30);
            this.m_btnUploadPDO.TabIndex = 33;
            this.m_btnUploadPDO.UseVisualStyleBackColor = true;
            this.m_btnUploadPDO.Click += new System.EventHandler(this.m_btnUploadPDO_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(4, 46);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 17);
            this.label2.TabIndex = 30;
            this.label2.Text = "Optional analysis:";
            // 
            // m_pdoFileTextBox
            // 
            this.m_pdoFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_pdoFileTextBox.Location = new System.Drawing.Point(171, 42);
            this.m_pdoFileTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.Size = new System.Drawing.Size(475, 22);
            this.m_pdoFileTextBox.TabIndex = 29;
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
            // 
            // m_browsePDOFileButton
            // 
            this.m_browsePDOFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browsePDOFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browsePDOFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browsePDOFileButton.Location = new System.Drawing.Point(653, 38);
            this.m_browsePDOFileButton.Margin = new System.Windows.Forms.Padding(4);
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_browsePDOFileButton.Size = new System.Drawing.Size(32, 30);
            this.m_browsePDOFileButton.TabIndex = 31;
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            this.m_executeIBAAButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.ibaAnalyzer_16x16;
            this.m_executeIBAAButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_executeIBAAButton.Location = new System.Drawing.Point(693, 38);
            this.m_executeIBAAButton.Margin = new System.Windows.Forms.Padding(4);
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.Size = new System.Drawing.Size(32, 30);
            this.m_executeIBAAButton.TabIndex = 32;
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(147, 17);
            this.label1.TabIndex = 34;
            this.label1.Text = "OPC UA Folder name:";
            // 
            // m_folderNameTextBox
            // 
            this.m_folderNameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_folderNameTextBox.Location = new System.Drawing.Point(171, 7);
            this.m_folderNameTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.m_folderNameTextBox.Name = "m_folderNameTextBox";
            this.m_folderNameTextBox.Size = new System.Drawing.Size(475, 22);
            this.m_folderNameTextBox.TabIndex = 35;
            this.m_folderNameTextBox.TextChanged += new System.EventHandler(this.folderNameTextBox_TextChanged);
            // 
            // m_browseDatFileButton
            // 
            this.m_browseDatFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseDatFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseDatFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseDatFileButton.Location = new System.Drawing.Point(653, 257);
            this.m_browseDatFileButton.Margin = new System.Windows.Forms.Padding(4);
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_browseDatFileButton.Size = new System.Drawing.Size(32, 30);
            this.m_browseDatFileButton.TabIndex = 37;
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(8, 263);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 17);
            this.label3.TabIndex = 38;
            this.label3.Text = "Example .dat file";
            // 
            // m_datFileTextBox
            // 
            this.m_datFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_datFileTextBox.Location = new System.Drawing.Point(151, 260);
            this.m_datFileTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.m_datFileTextBox.Name = "m_datFileTextBox";
            this.m_datFileTextBox.Size = new System.Drawing.Size(495, 22);
            this.m_datFileTextBox.TabIndex = 36;
            this.m_datFileTextBox.TextChanged += new System.EventHandler(this.m_datFileTextBox_TextChanged);
            // 
            // m_testButton
            // 
            this.m_testButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_testButton.Image = global::iba.Properties.Resources.sychronizeList;
            this.m_testButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_testButton.Location = new System.Drawing.Point(693, 257);
            this.m_testButton.Margin = new System.Windows.Forms.Padding(4);
            this.m_testButton.Name = "m_testButton";
            this.m_testButton.Size = new System.Drawing.Size(32, 30);
            this.m_testButton.TabIndex = 39;
            this.m_testButton.UseVisualStyleBackColor = true;
            this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
            // 
            // OPCUAWriterTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_browseDatFileButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_datFileTextBox);
            this.Controls.Add(this.m_testButton);
            this.Controls.Add(this.m_folderNameTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_btnUploadPDO);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_pdoFileTextBox);
            this.Controls.Add(this.m_browsePDOFileButton);
            this.Controls.Add(this.m_executeIBAAButton);
            this.Controls.Add(this.buttonEndpointCopy);
            this.Controls.Add(this.buttonEndpointRemove);
            this.Controls.Add(this.buttonEndpointAdd);
            this.Controls.Add(this.dataGrid);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.Name = "OPCUAWriterTaskControl";
            this.Size = new System.Drawing.Size(769, 481);
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxS7DataTypes)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		protected DevExpress.XtraGrid.GridControl dataGrid;
		private DevExpress.XtraGrid.Views.Grid.GridView dataGV;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxS7DataTypes;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
		private System.Windows.Forms.Button buttonEndpointCopy;
		private System.Windows.Forms.Button buttonEndpointRemove;
		private System.Windows.Forms.Button buttonEndpointAdd;
		private System.Windows.Forms.Button m_btnUploadPDO;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox m_pdoFileTextBox;
		private System.Windows.Forms.Button m_browsePDOFileButton;
		private System.Windows.Forms.Button m_executeIBAAButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox m_folderNameTextBox;
        private System.Windows.Forms.Button m_browseDatFileButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_datFileTextBox;
        private System.Windows.Forms.Button m_testButton;
        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
    }
}
