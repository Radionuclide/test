namespace iba.Controls
{
    partial class PanelScheduledJob
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelScheduledJob));
            this.gbSchedule = new System.Windows.Forms.GroupBox();
            this.m_repeatDurationCombo = new System.Windows.Forms.ComboBox();
            this.m_lblDuration = new System.Windows.Forms.Label();
            this.m_repeatIntervalCombo = new System.Windows.Forms.ComboBox();
            this.m_cbRepeat = new System.Windows.Forms.CheckBox();
            this.gbTrigger = new System.Windows.Forms.GroupBox();
            this.m_gbSubProperties = new System.Windows.Forms.GroupBox();
            this.m_timePicker = new System.Windows.Forms.DateTimePicker();
            this.m_datePicker = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_rbMonthly = new System.Windows.Forms.RadioButton();
            this.m_rbWeekly = new System.Windows.Forms.RadioButton();
            this.m_rbDaily = new System.Windows.Forms.RadioButton();
            this.m_rbOneTime = new System.Windows.Forms.RadioButton();
            this.m_undoChangesBtn = new System.Windows.Forms.Button();
            this.m_cbRetry = new System.Windows.Forms.CheckBox();
            this.m_retryUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.m_refreshDats = new System.Windows.Forms.Button();
            this.m_stopButton = new System.Windows.Forms.Button();
            this.m_applyToRunningBtn = new System.Windows.Forms.Button();
            this.m_startButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.m_autoStartCheckBox = new System.Windows.Forms.CheckBox();
            this.m_enableCheckBox = new System.Windows.Forms.CheckBox();
            this.m_failTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.gbSchedule.SuspendLayout();
            this.gbTrigger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_retryUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_failTimeUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // gbSchedule
            // 
            resources.ApplyResources(this.gbSchedule, "gbSchedule");
            this.gbSchedule.Controls.Add(this.m_repeatDurationCombo);
            this.gbSchedule.Controls.Add(this.m_lblDuration);
            this.gbSchedule.Controls.Add(this.m_repeatIntervalCombo);
            this.gbSchedule.Controls.Add(this.m_cbRepeat);
            this.gbSchedule.Controls.Add(this.gbTrigger);
            this.gbSchedule.Controls.Add(this.m_undoChangesBtn);
            this.gbSchedule.Controls.Add(this.m_cbRetry);
            this.gbSchedule.Controls.Add(this.m_retryUpDown);
            this.gbSchedule.Controls.Add(this.label14);
            this.gbSchedule.Controls.Add(this.m_refreshDats);
            this.gbSchedule.Controls.Add(this.m_stopButton);
            this.gbSchedule.Controls.Add(this.m_applyToRunningBtn);
            this.gbSchedule.Controls.Add(this.m_startButton);
            this.gbSchedule.Controls.Add(this.label11);
            this.gbSchedule.Controls.Add(this.m_autoStartCheckBox);
            this.gbSchedule.Controls.Add(this.m_enableCheckBox);
            this.gbSchedule.Controls.Add(this.m_failTimeUpDown);
            this.gbSchedule.Controls.Add(this.label10);
            this.gbSchedule.Name = "gbSchedule";
            this.gbSchedule.TabStop = false;
            // 
            // m_repeatDurationCombo
            // 
            this.m_repeatDurationCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_repeatDurationCombo.FormattingEnabled = true;
            this.m_repeatDurationCombo.Items.AddRange(new object[] {
            resources.GetString("m_repeatDurationCombo.Items"),
            resources.GetString("m_repeatDurationCombo.Items1"),
            resources.GetString("m_repeatDurationCombo.Items2"),
            resources.GetString("m_repeatDurationCombo.Items3"),
            resources.GetString("m_repeatDurationCombo.Items4"),
            resources.GetString("m_repeatDurationCombo.Items5")});
            resources.ApplyResources(this.m_repeatDurationCombo, "m_repeatDurationCombo");
            this.m_repeatDurationCombo.Name = "m_repeatDurationCombo";
            // 
            // m_lblDuration
            // 
            resources.ApplyResources(this.m_lblDuration, "m_lblDuration");
            this.m_lblDuration.Name = "m_lblDuration";
            // 
            // m_repeatIntervalCombo
            // 
            this.m_repeatIntervalCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_repeatIntervalCombo.FormattingEnabled = true;
            this.m_repeatIntervalCombo.Items.AddRange(new object[] {
            resources.GetString("m_repeatIntervalCombo.Items"),
            resources.GetString("m_repeatIntervalCombo.Items1"),
            resources.GetString("m_repeatIntervalCombo.Items2"),
            resources.GetString("m_repeatIntervalCombo.Items3"),
            resources.GetString("m_repeatIntervalCombo.Items4")});
            resources.ApplyResources(this.m_repeatIntervalCombo, "m_repeatIntervalCombo");
            this.m_repeatIntervalCombo.Name = "m_repeatIntervalCombo";
            // 
            // m_cbRepeat
            // 
            resources.ApplyResources(this.m_cbRepeat, "m_cbRepeat");
            this.m_cbRepeat.Name = "m_cbRepeat";
            this.m_cbRepeat.UseVisualStyleBackColor = true;
            this.m_cbRepeat.CheckedChanged += new System.EventHandler(this.m_cbRepeat_CheckedChanged);
            // 
            // gbTrigger
            // 
            resources.ApplyResources(this.gbTrigger, "gbTrigger");
            this.gbTrigger.Controls.Add(this.m_gbSubProperties);
            this.gbTrigger.Controls.Add(this.m_timePicker);
            this.gbTrigger.Controls.Add(this.m_datePicker);
            this.gbTrigger.Controls.Add(this.label2);
            this.gbTrigger.Controls.Add(this.label1);
            this.gbTrigger.Controls.Add(this.m_rbMonthly);
            this.gbTrigger.Controls.Add(this.m_rbWeekly);
            this.gbTrigger.Controls.Add(this.m_rbDaily);
            this.gbTrigger.Controls.Add(this.m_rbOneTime);
            this.gbTrigger.Name = "gbTrigger";
            this.gbTrigger.TabStop = false;
            // 
            // m_gbSubProperties
            // 
            resources.ApplyResources(this.m_gbSubProperties, "m_gbSubProperties");
            this.m_gbSubProperties.Name = "m_gbSubProperties";
            this.m_gbSubProperties.TabStop = false;
            // 
            // m_timePicker
            // 
            this.m_timePicker.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            resources.ApplyResources(this.m_timePicker, "m_timePicker");
            this.m_timePicker.Name = "m_timePicker";
            this.m_timePicker.ShowUpDown = true;
            // 
            // m_datePicker
            // 
            this.m_datePicker.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            resources.ApplyResources(this.m_datePicker, "m_datePicker");
            this.m_datePicker.Name = "m_datePicker";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_rbMonthly
            // 
            resources.ApplyResources(this.m_rbMonthly, "m_rbMonthly");
            this.m_rbMonthly.Name = "m_rbMonthly";
            this.m_rbMonthly.TabStop = true;
            this.m_rbMonthly.UseVisualStyleBackColor = true;
            this.m_rbMonthly.CheckedChanged += new System.EventHandler(this.OnTriggerRBChanged);
            // 
            // m_rbWeekly
            // 
            resources.ApplyResources(this.m_rbWeekly, "m_rbWeekly");
            this.m_rbWeekly.Name = "m_rbWeekly";
            this.m_rbWeekly.TabStop = true;
            this.m_rbWeekly.UseVisualStyleBackColor = true;
            this.m_rbWeekly.CheckedChanged += new System.EventHandler(this.OnTriggerRBChanged);
            // 
            // m_rbDaily
            // 
            resources.ApplyResources(this.m_rbDaily, "m_rbDaily");
            this.m_rbDaily.Name = "m_rbDaily";
            this.m_rbDaily.TabStop = true;
            this.m_rbDaily.UseVisualStyleBackColor = true;
            this.m_rbDaily.CheckedChanged += new System.EventHandler(this.OnTriggerRBChanged);
            // 
            // m_rbOneTime
            // 
            resources.ApplyResources(this.m_rbOneTime, "m_rbOneTime");
            this.m_rbOneTime.Name = "m_rbOneTime";
            this.m_rbOneTime.TabStop = true;
            this.m_rbOneTime.UseVisualStyleBackColor = true;
            this.m_rbOneTime.CheckedChanged += new System.EventHandler(this.OnTriggerRBChanged);
            // 
            // m_undoChangesBtn
            // 
            resources.ApplyResources(this.m_undoChangesBtn, "m_undoChangesBtn");
            this.m_undoChangesBtn.Image = global::iba.Properties.Resources.undoconfs;
            this.m_undoChangesBtn.Name = "m_undoChangesBtn";
            this.m_undoChangesBtn.UseVisualStyleBackColor = true;
            // 
            // m_cbRetry
            // 
            resources.ApplyResources(this.m_cbRetry, "m_cbRetry");
            this.m_cbRetry.Name = "m_cbRetry";
            this.m_cbRetry.UseVisualStyleBackColor = true;
            // 
            // m_retryUpDown
            // 
            resources.ApplyResources(this.m_retryUpDown, "m_retryUpDown");
            this.m_retryUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.m_retryUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_retryUpDown.Name = "m_retryUpDown";
            this.m_retryUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // m_refreshDats
            // 
            resources.ApplyResources(this.m_refreshDats, "m_refreshDats");
            this.m_refreshDats.Image = global::iba.Properties.Resources.refreshdats;
            this.m_refreshDats.Name = "m_refreshDats";
            this.m_refreshDats.UseVisualStyleBackColor = true;
            // 
            // m_stopButton
            // 
            resources.ApplyResources(this.m_stopButton, "m_stopButton");
            this.m_stopButton.Image = global::iba.Properties.Resources.Stop;
            this.m_stopButton.Name = "m_stopButton";
            this.m_stopButton.UseVisualStyleBackColor = true;
            // 
            // m_applyToRunningBtn
            // 
            resources.ApplyResources(this.m_applyToRunningBtn, "m_applyToRunningBtn");
            this.m_applyToRunningBtn.Image = global::iba.Properties.Resources.refreshconfs;
            this.m_applyToRunningBtn.Name = "m_applyToRunningBtn";
            this.m_applyToRunningBtn.UseVisualStyleBackColor = true;
            // 
            // m_startButton
            // 
            resources.ApplyResources(this.m_startButton, "m_startButton");
            this.m_startButton.Image = global::iba.Properties.Resources.Start;
            this.m_startButton.Name = "m_startButton";
            this.m_startButton.UseVisualStyleBackColor = true;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // m_autoStartCheckBox
            // 
            resources.ApplyResources(this.m_autoStartCheckBox, "m_autoStartCheckBox");
            this.m_autoStartCheckBox.Name = "m_autoStartCheckBox";
            this.m_autoStartCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_enableCheckBox
            // 
            resources.ApplyResources(this.m_enableCheckBox, "m_enableCheckBox");
            this.m_enableCheckBox.Name = "m_enableCheckBox";
            this.m_enableCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_failTimeUpDown
            // 
            this.m_failTimeUpDown.DecimalPlaces = 1;
            resources.ApplyResources(this.m_failTimeUpDown, "m_failTimeUpDown");
            this.m_failTimeUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.m_failTimeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.m_failTimeUpDown.Name = "m_failTimeUpDown";
            this.m_failTimeUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // button1
            // 
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // PanelScheduledJob
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gbSchedule);
            this.Name = "PanelScheduledJob";
            this.gbSchedule.ResumeLayout(false);
            this.gbSchedule.PerformLayout();
            this.gbTrigger.ResumeLayout(false);
            this.gbTrigger.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_retryUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_failTimeUpDown)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbSchedule;
        public System.Windows.Forms.Button m_undoChangesBtn;
        private System.Windows.Forms.CheckBox m_cbRetry;
        private System.Windows.Forms.NumericUpDown m_retryUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button m_refreshDats;
        public System.Windows.Forms.Button m_stopButton;
        public System.Windows.Forms.Button m_applyToRunningBtn;
        public System.Windows.Forms.Button m_startButton;
        private System.Windows.Forms.Label label11;
        public System.Windows.Forms.CheckBox m_autoStartCheckBox;
        public System.Windows.Forms.CheckBox m_enableCheckBox;
        private System.Windows.Forms.NumericUpDown m_failTimeUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.ComboBox m_repeatDurationCombo;
        private System.Windows.Forms.Label m_lblDuration;
        private System.Windows.Forms.ComboBox m_repeatIntervalCombo;
        private System.Windows.Forms.CheckBox m_cbRepeat;
        private System.Windows.Forms.GroupBox gbTrigger;
        private System.Windows.Forms.GroupBox m_gbSubProperties;
        private System.Windows.Forms.DateTimePicker m_timePicker;
        private System.Windows.Forms.DateTimePicker m_datePicker;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton m_rbMonthly;
        private System.Windows.Forms.RadioButton m_rbWeekly;
        private System.Windows.Forms.RadioButton m_rbDaily;
        private System.Windows.Forms.RadioButton m_rbOneTime;
        private System.Windows.Forms.Button button1;
    }
}
