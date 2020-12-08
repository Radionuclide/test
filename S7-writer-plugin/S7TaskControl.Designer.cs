namespace S7_writer_plugin
{
    partial class S7TaskControl
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
            if(disposing && (components != null))
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(S7TaskControl));
			this.label2 = new System.Windows.Forms.Label();
			this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
			this.m_browsePDOFileButton = new System.Windows.Forms.Button();
			this.m_executeIBAAButton = new System.Windows.Forms.Button();
			this.m_browseDatFileButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.m_datFileTextBox = new System.Windows.Forms.TextBox();
			this.m_testButton = new System.Windows.Forms.Button();
			this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
			this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
			this.m_monitorGroup = new System.Windows.Forms.GroupBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.m_nudTime = new System.Windows.Forms.NumericUpDown();
			this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
			this.m_cbTime = new System.Windows.Forms.CheckBox();
			this.m_cbMemory = new System.Windows.Forms.CheckBox();
			this.spTimeout = new System.Windows.Forms.NumericUpDown();
			this.cbConnType = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.spRack = new System.Windows.Forms.NumericUpDown();
			this.label1 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.tbAddress = new System.Windows.Forms.TextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.spSlot = new System.Windows.Forms.NumericUpDown();
			this.ckAllowErrors = new System.Windows.Forms.CheckBox();
			this.dataGrid = new DevExpress.XtraGrid.GridControl();
			this.dataGV = new DevExpress.XtraGrid.Views.Grid.GridView();
			this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.gridColumn10 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.repositoryItemComboBoxS7DataTypes = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
			this.gridColumn11 = new DevExpress.XtraGrid.Columns.GridColumn();
			this.buttonEndpointAdd = new System.Windows.Forms.Button();
			this.buttonEndpointRemove = new System.Windows.Forms.Button();
			this.buttonEndpointCopy = new System.Windows.Forms.Button();
			this.m_btnUploadPDO = new System.Windows.Forms.Button();
			this.m_monitorGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spTimeout)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spRack)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.spSlot)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGV)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxS7DataTypes)).BeginInit();
			this.SuspendLayout();
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// m_pdoFileTextBox
			// 
			resources.ApplyResources(this.m_pdoFileTextBox, "m_pdoFileTextBox");
			this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
			this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
			// 
			// m_browsePDOFileButton
			// 
			resources.ApplyResources(this.m_browsePDOFileButton, "m_browsePDOFileButton");
			this.m_browsePDOFileButton.Image = global::S7_writer_plugin.Properties.Resources.open;
			this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
			this.m_toolTip.SetToolTip(this.m_browsePDOFileButton, resources.GetString("m_browsePDOFileButton.ToolTip"));
			this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
			this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
			// 
			// m_executeIBAAButton
			// 
			resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
			this.m_executeIBAAButton.Image = global::S7_writer_plugin.Properties.Resources.ibaAnalyzer_16x16;
			this.m_executeIBAAButton.Name = "m_executeIBAAButton";
			this.m_toolTip.SetToolTip(this.m_executeIBAAButton, resources.GetString("m_executeIBAAButton.ToolTip"));
			this.m_executeIBAAButton.UseVisualStyleBackColor = true;
			this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
			// 
			// m_browseDatFileButton
			// 
			resources.ApplyResources(this.m_browseDatFileButton, "m_browseDatFileButton");
			this.m_browseDatFileButton.Image = global::S7_writer_plugin.Properties.Resources.open;
			this.m_browseDatFileButton.Name = "m_browseDatFileButton";
			this.m_toolTip.SetToolTip(this.m_browseDatFileButton, resources.GetString("m_browseDatFileButton.ToolTip"));
			this.m_browseDatFileButton.UseVisualStyleBackColor = true;
			this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// m_datFileTextBox
			// 
			resources.ApplyResources(this.m_datFileTextBox, "m_datFileTextBox");
			this.m_datFileTextBox.Name = "m_datFileTextBox";
			this.m_datFileTextBox.TextChanged += new System.EventHandler(this.m_datFileTextBox_TextChanged);
			// 
			// m_testButton
			// 
			resources.ApplyResources(this.m_testButton, "m_testButton");
			this.m_testButton.Image = global::S7_writer_plugin.Properties.Resources.select;
			this.m_testButton.Name = "m_testButton";
			this.m_toolTip.SetToolTip(this.m_testButton, resources.GetString("m_testButton.ToolTip"));
			this.m_testButton.UseVisualStyleBackColor = true;
			this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
			// 
			// m_monitorGroup
			// 
			resources.ApplyResources(this.m_monitorGroup, "m_monitorGroup");
			this.m_monitorGroup.Controls.Add(this.label4);
			this.m_monitorGroup.Controls.Add(this.label7);
			this.m_monitorGroup.Controls.Add(this.m_nudTime);
			this.m_monitorGroup.Controls.Add(this.m_nudMemory);
			this.m_monitorGroup.Controls.Add(this.m_cbTime);
			this.m_monitorGroup.Controls.Add(this.m_cbMemory);
			this.m_monitorGroup.Name = "m_monitorGroup";
			this.m_monitorGroup.TabStop = false;
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// m_nudTime
			// 
			resources.ApplyResources(this.m_nudTime, "m_nudTime");
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
			this.m_nudTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
			// 
			// m_nudMemory
			// 
			resources.ApplyResources(this.m_nudMemory, "m_nudMemory");
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
			this.m_nudMemory.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
			// 
			// m_cbTime
			// 
			resources.ApplyResources(this.m_cbTime, "m_cbTime");
			this.m_cbTime.Name = "m_cbTime";
			this.m_cbTime.UseVisualStyleBackColor = true;
			// 
			// m_cbMemory
			// 
			resources.ApplyResources(this.m_cbMemory, "m_cbMemory");
			this.m_cbMemory.Name = "m_cbMemory";
			this.m_cbMemory.UseVisualStyleBackColor = true;
			// 
			// spTimeout
			// 
			resources.ApplyResources(this.spTimeout, "spTimeout");
			this.spTimeout.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
			this.spTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.spTimeout.Name = "spTimeout";
			this.spTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// cbConnType
			// 
			resources.ApplyResources(this.cbConnType, "cbConnType");
			this.cbConnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.cbConnType.Items.AddRange(new object[] {
            resources.GetString("cbConnType.Items"),
            resources.GetString("cbConnType.Items1"),
            resources.GetString("cbConnType.Items2")});
			this.cbConnType.Name = "cbConnType";
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			// 
			// label9
			// 
			resources.ApplyResources(this.label9, "label9");
			this.label9.Name = "label9";
			// 
			// spRack
			// 
			resources.ApplyResources(this.spRack, "spRack");
			this.spRack.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.spRack.Name = "spRack";
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// tbAddress
			// 
			resources.ApplyResources(this.tbAddress, "tbAddress");
			this.tbAddress.Name = "tbAddress";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// spSlot
			// 
			resources.ApplyResources(this.spSlot, "spSlot");
			this.spSlot.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
			this.spSlot.Name = "spSlot";
			// 
			// ckAllowErrors
			// 
			resources.ApplyResources(this.ckAllowErrors, "ckAllowErrors");
			this.ckAllowErrors.Name = "ckAllowErrors";
			this.ckAllowErrors.UseVisualStyleBackColor = true;
			// 
			// dataGrid
			// 
			resources.ApplyResources(this.dataGrid, "dataGrid");
			this.dataGrid.EmbeddedNavigator.Margin = ((System.Windows.Forms.Padding)(resources.GetObject("dataGrid.EmbeddedNavigator.Margin")));
			this.dataGrid.MainView = this.dataGV;
			this.dataGrid.Name = "dataGrid";
			this.dataGrid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
            this.repositoryItemComboBoxS7DataTypes});
			this.dataGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataGV});
			// 
			// dataGV
			// 
			this.dataGV.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9,
            this.gridColumn10,
            this.gridColumn11});
			this.dataGV.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
			this.dataGV.GridControl = this.dataGrid;
			resources.ApplyResources(this.dataGV, "dataGV");
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
			// gridColumn6
			// 
			resources.ApplyResources(this.gridColumn6, "gridColumn6");
			this.gridColumn6.FieldName = "Expression";
			this.gridColumn6.Name = "gridColumn6";
			// 
			// gridColumn7
			// 
			resources.ApplyResources(this.gridColumn7, "gridColumn7");
			this.gridColumn7.FieldName = "DBNr";
			this.gridColumn7.Name = "gridColumn7";
			// 
			// gridColumn8
			// 
			resources.ApplyResources(this.gridColumn8, "gridColumn8");
			this.gridColumn8.FieldName = "Address";
			this.gridColumn8.Name = "gridColumn8";
			// 
			// gridColumn9
			// 
			resources.ApplyResources(this.gridColumn9, "gridColumn9");
			this.gridColumn9.FieldName = "BitNr";
			this.gridColumn9.Name = "gridColumn9";
			// 
			// gridColumn10
			// 
			resources.ApplyResources(this.gridColumn10, "gridColumn10");
			this.gridColumn10.ColumnEdit = this.repositoryItemComboBoxS7DataTypes;
			this.gridColumn10.FieldName = "DataTypeAsString";
			this.gridColumn10.Name = "gridColumn10";
			// 
			// repositoryItemComboBoxS7DataTypes
			// 
			resources.ApplyResources(this.repositoryItemComboBoxS7DataTypes, "repositoryItemComboBoxS7DataTypes");
			this.repositoryItemComboBoxS7DataTypes.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItemComboBoxS7DataTypes.Buttons"))))});
			this.repositoryItemComboBoxS7DataTypes.DropDownRows = 8;
			this.repositoryItemComboBoxS7DataTypes.Items.AddRange(new object[] {
            resources.GetString("repositoryItemComboBoxS7DataTypes.Items"),
            resources.GetString("repositoryItemComboBoxS7DataTypes.Items1"),
            resources.GetString("repositoryItemComboBoxS7DataTypes.Items2"),
            resources.GetString("repositoryItemComboBoxS7DataTypes.Items3"),
            resources.GetString("repositoryItemComboBoxS7DataTypes.Items4"),
            resources.GetString("repositoryItemComboBoxS7DataTypes.Items5"),
            resources.GetString("repositoryItemComboBoxS7DataTypes.Items6"),
            resources.GetString("repositoryItemComboBoxS7DataTypes.Items7")});
			this.repositoryItemComboBoxS7DataTypes.Name = "repositoryItemComboBoxS7DataTypes";
			// 
			// gridColumn11
			// 
			resources.ApplyResources(this.gridColumn11, "gridColumn11");
			this.gridColumn11.FieldName = "TestValueString";
			this.gridColumn11.Name = "gridColumn11";
			this.gridColumn11.OptionsColumn.AllowEdit = false;
			this.gridColumn11.OptionsColumn.ReadOnly = true;
			// 
			// buttonEndpointAdd
			// 
			resources.ApplyResources(this.buttonEndpointAdd, "buttonEndpointAdd");
			this.buttonEndpointAdd.Name = "buttonEndpointAdd";
			this.buttonEndpointAdd.TabStop = false;
			this.buttonEndpointAdd.UseVisualStyleBackColor = true;
			this.buttonEndpointAdd.Click += new System.EventHandler(this.buttonEndpointAdd_Click);
			// 
			// buttonEndpointRemove
			// 
			resources.ApplyResources(this.buttonEndpointRemove, "buttonEndpointRemove");
			this.buttonEndpointRemove.Name = "buttonEndpointRemove";
			this.buttonEndpointRemove.TabStop = false;
			this.buttonEndpointRemove.UseVisualStyleBackColor = true;
			this.buttonEndpointRemove.Click += new System.EventHandler(this.buttonEndpointRemove_Click);
			// 
			// buttonEndpointCopy
			// 
			resources.ApplyResources(this.buttonEndpointCopy, "buttonEndpointCopy");
			this.buttonEndpointCopy.Name = "buttonEndpointCopy";
			this.buttonEndpointCopy.TabStop = false;
			this.buttonEndpointCopy.UseVisualStyleBackColor = true;
			this.buttonEndpointCopy.Click += new System.EventHandler(this.buttonEndpointCopy_Click);
			// 
			// m_btnUploadPDO
			// 
			resources.ApplyResources(this.m_btnUploadPDO, "m_btnUploadPDO");
			this.m_btnUploadPDO.Image = global::S7_writer_plugin.Properties.Resources.img_pdo_upload;
			this.m_btnUploadPDO.Name = "m_btnUploadPDO";
			this.m_btnUploadPDO.UseVisualStyleBackColor = true;
			this.m_btnUploadPDO.Click += new System.EventHandler(this.m_btnUploadPDO_Click);
			// 
			// S7TaskControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_btnUploadPDO);
			this.Controls.Add(this.buttonEndpointCopy);
			this.Controls.Add(this.buttonEndpointRemove);
			this.Controls.Add(this.buttonEndpointAdd);
			this.Controls.Add(this.dataGrid);
			this.Controls.Add(this.ckAllowErrors);
			this.Controls.Add(this.spRack);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.tbAddress);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.spSlot);
			this.Controls.Add(this.spTimeout);
			this.Controls.Add(this.cbConnType);
			this.Controls.Add(this.label8);
			this.Controls.Add(this.label9);
			this.Controls.Add(this.m_monitorGroup);
			this.Controls.Add(this.m_browseDatFileButton);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_datFileTextBox);
			this.Controls.Add(this.m_testButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_pdoFileTextBox);
			this.Controls.Add(this.m_browsePDOFileButton);
			this.Controls.Add(this.m_executeIBAAButton);
			this.Name = "S7TaskControl";
			this.m_monitorGroup.ResumeLayout(false);
			this.m_monitorGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spTimeout)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spRack)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.spSlot)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.dataGV)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.repositoryItemComboBoxS7DataTypes)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_pdoFileTextBox;
        private System.Windows.Forms.Button m_browsePDOFileButton;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Button m_browseDatFileButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_datFileTextBox;
        private System.Windows.Forms.Button m_testButton;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.GroupBox m_monitorGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
        private System.Windows.Forms.NumericUpDown spTimeout;
        private System.Windows.Forms.ComboBox cbConnType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown spRack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown spSlot;
        private System.Windows.Forms.CheckBox ckAllowErrors;
		protected DevExpress.XtraGrid.GridControl dataGrid;
		private System.Windows.Forms.Button buttonEndpointAdd;
		private System.Windows.Forms.Button buttonEndpointRemove;
		private DevExpress.XtraEditors.Repository.RepositoryItemComboBox repositoryItemComboBoxS7DataTypes;
		private DevExpress.XtraGrid.Views.Grid.GridView dataGV;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn10;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn11;
		private System.Windows.Forms.Button buttonEndpointCopy;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
		private System.Windows.Forms.Button m_btnUploadPDO;
	}
}
