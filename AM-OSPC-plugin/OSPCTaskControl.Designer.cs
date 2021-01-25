namespace AM_OSPC_plugin
{
    partial class OSPCTaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OSPCTaskControl));
            this.label2 = new System.Windows.Forms.Label();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browsePDOFileButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.m_browseDatFileButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_datFileTextBox = new System.Windows.Forms.TextBox();
            this.m_testButton = new System.Windows.Forms.Button();
            this.m_toolTip = new System.Windows.Forms.ToolTip();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_ospcPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.m_ospcUsername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_ospcHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_monitorGroup = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            this.dataGrid = new DevExpress.XtraGrid.GridControl();
            this.dataGV = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn6 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn7 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn8 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.gridColumn9 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.m_btnUploadPDO = new System.Windows.Forms.Button();
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).BeginInit();
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
            this.m_browsePDOFileButton.Image = global::AM_OSPC_plugin.Properties.Resources.open;
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_toolTip.SetToolTip(this.m_browsePDOFileButton, resources.GetString("m_browsePDOFileButton.ToolTip"));
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_toolTip.SetToolTip(this.m_executeIBAAButton, resources.GetString("m_executeIBAAButton.ToolTip"));
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // m_browseDatFileButton
            // 
            resources.ApplyResources(this.m_browseDatFileButton, "m_browseDatFileButton");
            this.m_browseDatFileButton.Image = global::AM_OSPC_plugin.Properties.Resources.open;
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
            this.m_testButton.Image = global::AM_OSPC_plugin.Properties.Resources.select;
            this.m_testButton.Name = "m_testButton";
            this.m_toolTip.SetToolTip(this.m_testButton, resources.GetString("m_testButton.ToolTip"));
            this.m_testButton.UseVisualStyleBackColor = true;
            this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
            // 
            // m_ospcPassword
            // 
            resources.ApplyResources(this.m_ospcPassword, "m_ospcPassword");
            this.m_ospcPassword.Name = "m_ospcPassword";
            this.m_ospcPassword.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // m_ospcUsername
            // 
            resources.ApplyResources(this.m_ospcUsername, "m_ospcUsername");
            this.m_ospcUsername.Name = "m_ospcUsername";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // m_ospcHost
            // 
            resources.ApplyResources(this.m_ospcHost, "m_ospcHost");
            this.m_ospcHost.Name = "m_ospcHost";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // dataGrid
            // 
            resources.ApplyResources(this.dataGrid, "dataGrid");
            this.dataGrid.EmbeddedNavigator.Margin = ((System.Windows.Forms.Padding)(resources.GetObject("dataGrid.EmbeddedNavigator.Margin")));
            this.dataGrid.MainView = this.dataGV;
            this.dataGrid.Name = "dataGrid";
            this.dataGrid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.dataGV});
            // 
            // dataGV
            // 
            this.dataGV.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn6,
            this.gridColumn7,
            this.gridColumn8,
            this.gridColumn9});
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
            this.gridColumn7.FieldName = "ProcessName";
            this.gridColumn7.Name = "gridColumn7";
            // 
            // gridColumn8
            // 
            resources.ApplyResources(this.gridColumn8, "gridColumn8");
            this.gridColumn8.FieldName = "VariableName";
            this.gridColumn8.Name = "gridColumn8";
            // 
            // gridColumn9
            // 
            resources.ApplyResources(this.gridColumn9, "gridColumn9");
            this.gridColumn9.FieldName = "TestValueString";
            this.gridColumn9.Name = "gridColumn9";
            this.gridColumn9.OptionsColumn.AllowEdit = false;
            this.gridColumn9.OptionsColumn.ReadOnly = true;
            // 
            // m_btnUploadPDO
            // 
            resources.ApplyResources(this.m_btnUploadPDO, "m_btnUploadPDO");
            this.m_btnUploadPDO.Image = global::AM_OSPC_plugin.Properties.Resources.img_pdo_upload;
            this.m_btnUploadPDO.Name = "m_btnUploadPDO";
            this.m_btnUploadPDO.UseVisualStyleBackColor = true;
            this.m_btnUploadPDO.Click += new System.EventHandler(this.m_btnUploadPDO_Click);
            // 
            // OSPCTaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_btnUploadPDO);
            this.Controls.Add(this.dataGrid);
            this.Controls.Add(this.m_monitorGroup);
            this.Controls.Add(this.m_ospcPassword);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.m_ospcUsername);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.m_ospcHost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_browseDatFileButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_datFileTextBox);
            this.Controls.Add(this.m_testButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_pdoFileTextBox);
            this.Controls.Add(this.m_browsePDOFileButton);
            this.Controls.Add(this.m_executeIBAAButton);
            this.Name = "OSPCTaskControl";
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGrid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGV)).EndInit();
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
        private System.Windows.Forms.TextBox m_ospcPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox m_ospcUsername;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox m_ospcHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox m_monitorGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
		protected DevExpress.XtraGrid.GridControl dataGrid;
		private DevExpress.XtraGrid.Views.Grid.GridView dataGV;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn7;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn8;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn9;
		private DevExpress.XtraGrid.Columns.GridColumn gridColumn6;
		private System.Windows.Forms.Button m_btnUploadPDO;
	}
}
