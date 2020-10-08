namespace iba.Controls
{
    partial class TriggerControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TriggerControl));
            this.m_grPulse = new DevExpress.XtraGrid.GridControl();
            this.m_viewPulse = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.m_colPulse = new DevExpress.XtraGrid.Columns.GridColumn();
            this.m_rbTriggerBySignal = new System.Windows.Forms.RadioButton();
            this.m_rbTriggerPerFile = new System.Windows.Forms.RadioButton();
            this.m_grTime = new DevExpress.XtraGrid.GridControl();
            this.m_viewTime = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.m_colTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.ckActive = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.comboEventIn = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.comboEventOut = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_grTimeOut = new DevExpress.XtraGrid.GridControl();
            this.m_viewTimeOut = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.m_colTimeOut = new DevExpress.XtraGrid.Columns.GridColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_grPulse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewPulse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_grTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_grTimeOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewTimeOut)).BeginInit();
            this.SuspendLayout();
            // 
            // m_grPulse
            // 
            resources.ApplyResources(this.m_grPulse, "m_grPulse");
            this.m_grPulse.MainView = this.m_viewPulse;
            this.m_grPulse.Name = "m_grPulse";
            this.m_grPulse.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.m_viewPulse});
            // 
            // m_viewPulse
            // 
            this.m_viewPulse.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.m_colPulse});
            this.m_viewPulse.GridControl = this.m_grPulse;
            this.m_viewPulse.Name = "m_viewPulse";
            this.m_viewPulse.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.m_viewPulse.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.m_viewPulse.OptionsBehavior.AutoPopulateColumns = false;
            this.m_viewPulse.OptionsCustomization.AllowColumnMoving = false;
            this.m_viewPulse.OptionsCustomization.AllowColumnResizing = false;
            this.m_viewPulse.OptionsCustomization.AllowFilter = false;
            this.m_viewPulse.OptionsCustomization.AllowGroup = false;
            this.m_viewPulse.OptionsCustomization.AllowQuickHideColumns = false;
            this.m_viewPulse.OptionsCustomization.AllowSort = false;
            this.m_viewPulse.OptionsFind.AllowFindPanel = false;
            this.m_viewPulse.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.m_viewPulse.OptionsView.ShowColumnHeaders = false;
            this.m_viewPulse.OptionsView.ShowGroupPanel = false;
            this.m_viewPulse.OptionsView.ShowIndicator = false;
            // 
            // m_colPulse
            // 
            resources.ApplyResources(this.m_colPulse, "m_colPulse");
            this.m_colPulse.FieldName = "PulseID";
            this.m_colPulse.Name = "m_colPulse";
            this.m_colPulse.OptionsColumn.AllowEdit = false;
            this.m_colPulse.OptionsColumn.ReadOnly = true;
            // 
            // m_rbTriggerBySignal
            // 
            resources.ApplyResources(this.m_rbTriggerBySignal, "m_rbTriggerBySignal");
            this.m_rbTriggerBySignal.Name = "m_rbTriggerBySignal";
            this.m_rbTriggerBySignal.UseVisualStyleBackColor = true;
            this.m_rbTriggerBySignal.CheckedChanged += new System.EventHandler(this.m_rbTriggerBySignal_CheckedChanged);
            // 
            // m_rbTriggerPerFile
            // 
            resources.ApplyResources(this.m_rbTriggerPerFile, "m_rbTriggerPerFile");
            this.m_rbTriggerPerFile.Checked = true;
            this.m_rbTriggerPerFile.Name = "m_rbTriggerPerFile";
            this.m_rbTriggerPerFile.TabStop = true;
            this.m_rbTriggerPerFile.UseVisualStyleBackColor = true;
            // 
            // m_grTime
            // 
            resources.ApplyResources(this.m_grTime, "m_grTime");
            this.m_grTime.MainView = this.m_viewTime;
            this.m_grTime.Name = "m_grTime";
            this.m_grTime.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.m_viewTime});
            // 
            // m_viewTime
            // 
            this.m_viewTime.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.m_colTime});
            this.m_viewTime.GridControl = this.m_grTime;
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
            resources.ApplyResources(this.m_colTime, "m_colTime");
            this.m_colTime.FieldName = "PulseID";
            this.m_colTime.Name = "m_colTime";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // ckActive
            // 
            resources.ApplyResources(this.ckActive, "ckActive");
            this.ckActive.Name = "ckActive";
            this.ckActive.UseVisualStyleBackColor = true;
            this.ckActive.CheckedChanged += new System.EventHandler(this.ckActive_CheckedChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // comboEventIn
            // 
            this.comboEventIn.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboEventIn, "comboEventIn");
            this.comboEventIn.Name = "comboEventIn";
            this.comboEventIn.SelectedIndexChanged += new System.EventHandler(this.comboEventIn_SelectedIndexChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // comboEventOut
            // 
            this.comboEventOut.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            resources.ApplyResources(this.comboEventOut, "comboEventOut");
            this.comboEventOut.Name = "comboEventOut";
            this.comboEventOut.SelectedIndexChanged += new System.EventHandler(this.comboEventOut_SelectedIndexChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // m_grTimeOut
            // 
            resources.ApplyResources(this.m_grTimeOut, "m_grTimeOut");
            this.m_grTimeOut.MainView = this.m_viewTimeOut;
            this.m_grTimeOut.Name = "m_grTimeOut";
            this.m_grTimeOut.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.m_viewTimeOut});
            // 
            // m_viewTimeOut
            // 
            this.m_viewTimeOut.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.m_colTimeOut});
            this.m_viewTimeOut.GridControl = this.m_grTimeOut;
            this.m_viewTimeOut.Name = "m_viewTimeOut";
            this.m_viewTimeOut.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            this.m_viewTimeOut.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            this.m_viewTimeOut.OptionsBehavior.AutoPopulateColumns = false;
            this.m_viewTimeOut.OptionsCustomization.AllowColumnMoving = false;
            this.m_viewTimeOut.OptionsCustomization.AllowColumnResizing = false;
            this.m_viewTimeOut.OptionsCustomization.AllowFilter = false;
            this.m_viewTimeOut.OptionsCustomization.AllowGroup = false;
            this.m_viewTimeOut.OptionsCustomization.AllowQuickHideColumns = false;
            this.m_viewTimeOut.OptionsCustomization.AllowSort = false;
            this.m_viewTimeOut.OptionsFind.AllowFindPanel = false;
            this.m_viewTimeOut.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.m_viewTimeOut.OptionsView.ShowColumnHeaders = false;
            this.m_viewTimeOut.OptionsView.ShowGroupPanel = false;
            this.m_viewTimeOut.OptionsView.ShowIndicator = false;
            // 
            // m_colTimeOut
            // 
            resources.ApplyResources(this.m_colTimeOut, "m_colTimeOut");
            this.m_colTimeOut.FieldName = "PulseID";
            this.m_colTimeOut.Name = "m_colTimeOut";
            // 
            // TriggerControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_grTimeOut);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.comboEventOut);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.comboEventIn);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.ckActive);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_grTime);
            this.Controls.Add(this.m_grPulse);
            this.Controls.Add(this.m_rbTriggerBySignal);
            this.Controls.Add(this.m_rbTriggerPerFile);
            this.Name = "TriggerControl";
            ((System.ComponentModel.ISupportInitialize)(this.m_grPulse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewPulse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_grTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_grTimeOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewTimeOut)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.GridControl m_grPulse;
        private DevExpress.XtraGrid.Views.Grid.GridView m_viewPulse;
        private DevExpress.XtraGrid.Columns.GridColumn m_colPulse;
        private System.Windows.Forms.RadioButton m_rbTriggerBySignal;
        private System.Windows.Forms.RadioButton m_rbTriggerPerFile;
        private DevExpress.XtraGrid.GridControl m_grTime;
        private DevExpress.XtraGrid.Views.Grid.GridView m_viewTime;
        private DevExpress.XtraGrid.Columns.GridColumn m_colTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckActive;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboEventIn;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboEventOut;
        private System.Windows.Forms.Label label5;
        private DevExpress.XtraGrid.GridControl m_grTimeOut;
        private DevExpress.XtraGrid.Views.Grid.GridView m_viewTimeOut;
        private DevExpress.XtraGrid.Columns.GridColumn m_colTimeOut;
    }
}
