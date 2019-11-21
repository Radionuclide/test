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
            this.m_grPulse = new DevExpress.XtraGrid.GridControl();
            this.m_viewPulse = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.m_colPulse = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label1 = new System.Windows.Forms.Label();
            this.m_rbTriggerBySignal = new System.Windows.Forms.RadioButton();
            this.m_rbTriggerPerFile = new System.Windows.Forms.RadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.m_grPulse)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewPulse)).BeginInit();
            this.SuspendLayout();
            // 
            // m_grPulse
            // 
            this.m_grPulse.Location = new System.Drawing.Point(126, 68);
            this.m_grPulse.MainView = this.m_viewPulse;
            this.m_grPulse.Name = "m_grPulse";
            this.m_grPulse.Size = new System.Drawing.Size(130, 20);
            this.m_grPulse.TabIndex = 6;
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
            this.m_colPulse.Caption = "Pulse signal";
            this.m_colPulse.FieldName = "PulseID";
            this.m_colPulse.Name = "m_colPulse";
            this.m_colPulse.OptionsColumn.AllowEdit = false;
            this.m_colPulse.OptionsColumn.ReadOnly = true;
            this.m_colPulse.Visible = true;
            this.m_colPulse.VisibleIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(36, 70);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Pulse signal:";
            // 
            // m_rbTriggerBySignal
            // 
            this.m_rbTriggerBySignal.AutoSize = true;
            this.m_rbTriggerBySignal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbTriggerBySignal.Location = new System.Drawing.Point(17, 36);
            this.m_rbTriggerBySignal.Name = "m_rbTriggerBySignal";
            this.m_rbTriggerBySignal.Size = new System.Drawing.Size(302, 17);
            this.m_rbTriggerBySignal.TabIndex = 1;
            this.m_rbTriggerBySignal.Text = "Generate an event occurrence for every rectangular pulse:";
            this.m_rbTriggerBySignal.UseVisualStyleBackColor = true;
            this.m_rbTriggerBySignal.CheckedChanged += new System.EventHandler(this.m_rbTriggerBySignal_CheckedChanged);
            // 
            // m_rbTriggerPerFile
            // 
            this.m_rbTriggerPerFile.AutoSize = true;
            this.m_rbTriggerPerFile.Checked = true;
            this.m_rbTriggerPerFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbTriggerPerFile.Location = new System.Drawing.Point(17, 13);
            this.m_rbTriggerPerFile.Name = "m_rbTriggerPerFile";
            this.m_rbTriggerPerFile.Size = new System.Drawing.Size(263, 17);
            this.m_rbTriggerPerFile.TabIndex = 0;
            this.m_rbTriggerPerFile.TabStop = true;
            this.m_rbTriggerPerFile.Text = "Generate one event occurrence per processed file";
            this.m_rbTriggerPerFile.UseVisualStyleBackColor = true;
            // 
            // TriggerControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_grPulse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_rbTriggerBySignal);
            this.Controls.Add(this.m_rbTriggerPerFile);
            this.Name = "TriggerControl";
            this.Size = new System.Drawing.Size(494, 105);
            ((System.ComponentModel.ISupportInitialize)(this.m_grPulse)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_viewPulse)).EndInit();
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
    }
}
