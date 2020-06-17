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
            this.label1 = new System.Windows.Forms.Label();
            this.m_rbTriggerBySignal = new System.Windows.Forms.RadioButton();
            this.m_rbTriggerPerFile = new System.Windows.Forms.RadioButton();
            this.m_grTime = new DevExpress.XtraGrid.GridControl();
            this.m_viewTime = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.m_colTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.ckActive = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_grPulse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewPulse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_grTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewTime)).BeginInit();
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // TriggerControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ckActive);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_grTime);
            this.Controls.Add(this.m_grPulse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_rbTriggerBySignal);
            this.Controls.Add(this.m_rbTriggerPerFile);
            this.Name = "TriggerControl";
            ((System.ComponentModel.ISupportInitialize)(this.m_grPulse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewPulse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_grTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private DevExpress.XtraGrid.GridControl m_grPulse;
        private DevExpress.XtraGrid.Views.Grid.GridView m_viewPulse;
        private DevExpress.XtraGrid.Columns.GridColumn m_colPulse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton m_rbTriggerBySignal;
        private System.Windows.Forms.RadioButton m_rbTriggerPerFile;
        private DevExpress.XtraGrid.GridControl m_grTime;
        private DevExpress.XtraGrid.Views.Grid.GridView m_viewTime;
        private DevExpress.XtraGrid.Columns.GridColumn m_colTime;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox ckActive;
    }
}
