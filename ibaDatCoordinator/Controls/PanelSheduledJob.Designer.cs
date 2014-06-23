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
            this.m_repeatEveryCombo = new System.Windows.Forms.ComboBox();
            this.m_cbRepeat = new System.Windows.Forms.CheckBox();
            this.gbTrigger = new System.Windows.Forms.GroupBox();
            this.m_gbSubProperties = new System.Windows.Forms.GroupBox();
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
            this.gbHD = new System.Windows.Forms.GroupBox();
            this.gbTimeSelection = new System.Windows.Forms.GroupBox();
            this.m_lblTimebase = new System.Windows.Forms.Label();
            this.m_cbTimeBase = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_nudStopSeconds = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.m_nudStopMinutes = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.m_nudStopHours = new System.Windows.Forms.NumericUpDown();
            this.m_nudStopDays = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.m_nudStartSeconds = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.m_nudStartMinutes = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.m_nudStartHours = new System.Windows.Forms.NumericUpDown();
            this.m_nudStartDays = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.m_dtStart = new iba.Utility.RippleDateTimePicker();
            this.gbSchedule.SuspendLayout();
            this.gbTrigger.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_retryUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_failTimeUpDown)).BeginInit();
            this.gbTimeSelection.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStopSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStopMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStopHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStopDays)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStartSeconds)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStartMinutes)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStartHours)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStartDays)).BeginInit();
            this.SuspendLayout();
            // 
            // gbSchedule
            // 
            resources.ApplyResources(this.gbSchedule, "gbSchedule");
            this.gbSchedule.Controls.Add(this.m_repeatDurationCombo);
            this.gbSchedule.Controls.Add(this.m_lblDuration);
            this.gbSchedule.Controls.Add(this.m_repeatEveryCombo);
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
            // m_repeatEveryCombo
            // 
            this.m_repeatEveryCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_repeatEveryCombo.FormattingEnabled = true;
            this.m_repeatEveryCombo.Items.AddRange(new object[] {
            resources.GetString("m_repeatEveryCombo.Items"),
            resources.GetString("m_repeatEveryCombo.Items1"),
            resources.GetString("m_repeatEveryCombo.Items2"),
            resources.GetString("m_repeatEveryCombo.Items3"),
            resources.GetString("m_repeatEveryCombo.Items4")});
            resources.ApplyResources(this.m_repeatEveryCombo, "m_repeatEveryCombo");
            this.m_repeatEveryCombo.Name = "m_repeatEveryCombo";
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
            this.gbTrigger.Controls.Add(this.m_dtStart);
            this.gbTrigger.Controls.Add(this.m_gbSubProperties);
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
            // gbHD
            // 
            resources.ApplyResources(this.gbHD, "gbHD");
            this.gbHD.Name = "gbHD";
            this.gbHD.TabStop = false;
            // 
            // gbTimeSelection
            // 
            resources.ApplyResources(this.gbTimeSelection, "gbTimeSelection");
            this.gbTimeSelection.Controls.Add(this.m_lblTimebase);
            this.gbTimeSelection.Controls.Add(this.m_cbTimeBase);
            this.gbTimeSelection.Controls.Add(this.label3);
            this.gbTimeSelection.Controls.Add(this.m_nudStopSeconds);
            this.gbTimeSelection.Controls.Add(this.label4);
            this.gbTimeSelection.Controls.Add(this.m_nudStopMinutes);
            this.gbTimeSelection.Controls.Add(this.label12);
            this.gbTimeSelection.Controls.Add(this.label13);
            this.gbTimeSelection.Controls.Add(this.m_nudStopHours);
            this.gbTimeSelection.Controls.Add(this.m_nudStopDays);
            this.gbTimeSelection.Controls.Add(this.label15);
            this.gbTimeSelection.Controls.Add(this.label5);
            this.gbTimeSelection.Controls.Add(this.m_nudStartSeconds);
            this.gbTimeSelection.Controls.Add(this.label6);
            this.gbTimeSelection.Controls.Add(this.m_nudStartMinutes);
            this.gbTimeSelection.Controls.Add(this.label7);
            this.gbTimeSelection.Controls.Add(this.label8);
            this.gbTimeSelection.Controls.Add(this.m_nudStartHours);
            this.gbTimeSelection.Controls.Add(this.m_nudStartDays);
            this.gbTimeSelection.Controls.Add(this.label9);
            this.gbTimeSelection.Name = "gbTimeSelection";
            this.gbTimeSelection.TabStop = false;
            // 
            // m_lblTimebase
            // 
            resources.ApplyResources(this.m_lblTimebase, "m_lblTimebase");
            this.m_lblTimebase.Name = "m_lblTimebase";
            // 
            // m_cbTimeBase
            // 
            resources.ApplyResources(this.m_cbTimeBase, "m_cbTimeBase");
            this.m_cbTimeBase.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.m_cbTimeBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbTimeBase.FormattingEnabled = true;
            this.m_cbTimeBase.Name = "m_cbTimeBase";
            this.m_cbTimeBase.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.m_cbTimeBase_DrawItem);
            this.m_cbTimeBase.SelectedIndexChanged += new System.EventHandler(this.m_cbTimeBase_SelectedIndexChanged);
            this.m_cbTimeBase.DropDownClosed += new System.EventHandler(this.m_cbTimeBase_DropDownClosed);
            this.m_cbTimeBase.MouseLeave += new System.EventHandler(this.m_cbTimeBase_DropDownClosed);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // m_nudStopSeconds
            // 
            resources.ApplyResources(this.m_nudStopSeconds, "m_nudStopSeconds");
            this.m_nudStopSeconds.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_nudStopSeconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.m_nudStopSeconds.Name = "m_nudStopSeconds";
            this.m_nudStopSeconds.ValueChanged += new System.EventHandler(this.StopChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // m_nudStopMinutes
            // 
            resources.ApplyResources(this.m_nudStopMinutes, "m_nudStopMinutes");
            this.m_nudStopMinutes.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_nudStopMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.m_nudStopMinutes.Name = "m_nudStopMinutes";
            this.m_nudStopMinutes.ValueChanged += new System.EventHandler(this.StopChanged);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // m_nudStopHours
            // 
            resources.ApplyResources(this.m_nudStopHours, "m_nudStopHours");
            this.m_nudStopHours.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_nudStopHours.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.m_nudStopHours.Name = "m_nudStopHours";
            this.m_nudStopHours.ValueChanged += new System.EventHandler(this.StopChanged);
            // 
            // m_nudStopDays
            // 
            resources.ApplyResources(this.m_nudStopDays, "m_nudStopDays");
            this.m_nudStopDays.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.m_nudStopDays.Name = "m_nudStopDays";
            this.m_nudStopDays.ValueChanged += new System.EventHandler(this.StopChanged);
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // m_nudStartSeconds
            // 
            resources.ApplyResources(this.m_nudStartSeconds, "m_nudStartSeconds");
            this.m_nudStartSeconds.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_nudStartSeconds.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.m_nudStartSeconds.Name = "m_nudStartSeconds";
            this.m_nudStartSeconds.ValueChanged += new System.EventHandler(this.StartChanged);
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // m_nudStartMinutes
            // 
            resources.ApplyResources(this.m_nudStartMinutes, "m_nudStartMinutes");
            this.m_nudStartMinutes.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_nudStartMinutes.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.m_nudStartMinutes.Name = "m_nudStartMinutes";
            this.m_nudStartMinutes.ValueChanged += new System.EventHandler(this.StartChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // m_nudStartHours
            // 
            resources.ApplyResources(this.m_nudStartHours, "m_nudStartHours");
            this.m_nudStartHours.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_nudStartHours.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            -2147483648});
            this.m_nudStartHours.Name = "m_nudStartHours";
            this.m_nudStartHours.ValueChanged += new System.EventHandler(this.StartChanged);
            // 
            // m_nudStartDays
            // 
            resources.ApplyResources(this.m_nudStartDays, "m_nudStartDays");
            this.m_nudStartDays.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.m_nudStartDays.Name = "m_nudStartDays";
            this.m_nudStartDays.ValueChanged += new System.EventHandler(this.StartChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // m_dtStart
            // 
            resources.ApplyResources(this.m_dtStart, "m_dtStart");
            this.m_dtStart.Name = "m_dtStart";
            // 
            // PanelScheduledJob
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbTimeSelection);
            this.Controls.Add(this.gbHD);
            this.Controls.Add(this.gbSchedule);
            this.Name = "PanelScheduledJob";
            this.gbSchedule.ResumeLayout(false);
            this.gbSchedule.PerformLayout();
            this.gbTrigger.ResumeLayout(false);
            this.gbTrigger.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_retryUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_failTimeUpDown)).EndInit();
            this.gbTimeSelection.ResumeLayout(false);
            this.gbTimeSelection.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStopSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStopMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStopHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStopDays)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStartSeconds)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStartMinutes)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStartHours)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudStartDays)).EndInit();
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
        private System.Windows.Forms.ComboBox m_repeatEveryCombo;
        private System.Windows.Forms.CheckBox m_cbRepeat;
        private System.Windows.Forms.GroupBox gbTrigger;
        private System.Windows.Forms.GroupBox m_gbSubProperties;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton m_rbMonthly;
        private System.Windows.Forms.RadioButton m_rbWeekly;
        private System.Windows.Forms.RadioButton m_rbDaily;
        private System.Windows.Forms.RadioButton m_rbOneTime;
        private iba.Utility.RippleDateTimePicker m_dtStart;
        private System.Windows.Forms.GroupBox gbHD;
        private System.Windows.Forms.GroupBox gbTimeSelection;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown m_nudStopSeconds;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown m_nudStopMinutes;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.NumericUpDown m_nudStopHours;
        private System.Windows.Forms.NumericUpDown m_nudStopDays;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown m_nudStartSeconds;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown m_nudStartMinutes;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown m_nudStartHours;
        private System.Windows.Forms.NumericUpDown m_nudStartDays;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label m_lblTimebase;
        private System.Windows.Forms.ComboBox m_cbTimeBase;
    }
}
