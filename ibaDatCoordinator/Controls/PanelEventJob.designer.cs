namespace iba.Controls
{
    partial class PanelEventJob
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        static PanelEventJob()
        {
            DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            defaultLookAndFeel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            defaultLookAndFeel.LookAndFeel.UseWindowsXPTheme = true;
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelEventJob));
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbEvent = new iba.Utility.CollapsibleGroupBox();
            this.gbSelection = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.m_cbJobTrigger = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fpnlEvent = new System.Windows.Forms.FlowLayoutPanel();
            this.tbEventServer = new System.Windows.Forms.TextBox();
            this.btnEventServer = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.tbEventServerPort = new System.Windows.Forms.TextBox();
            this.m_cbRepErr = new System.Windows.Forms.CheckBox();
            this.m_cbInitialScanEnabled = new System.Windows.Forms.CheckBox();
            this.m_undoChangesBtn = new System.Windows.Forms.Button();
            this.m_cbRetry = new System.Windows.Forms.CheckBox();
            this.m_retryUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.m_stopButton = new System.Windows.Forms.Button();
            this.m_applyToRunningBtn = new System.Windows.Forms.Button();
            this.m_startButton = new System.Windows.Forms.Button();
            this.m_autoStartCheckBox = new System.Windows.Forms.CheckBox();
            this.m_enableCheckBox = new System.Windows.Forms.CheckBox();
            this.m_failTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.gbHD = new iba.Utility.CollapsibleGroupBox();
            this.gbTimeSelection = new iba.Utility.CollapsibleGroupBox();
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
            this.gbEvent.SuspendLayout();
            this.gbSelection.SuspendLayout();
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
            // gbEvent
            // 
            resources.ApplyResources(this.gbEvent, "gbEvent");
            this.gbEvent.Controls.Add(this.gbSelection);
            this.gbEvent.Controls.Add(this.m_cbRepErr);
            this.gbEvent.Controls.Add(this.m_cbInitialScanEnabled);
            this.gbEvent.Controls.Add(this.m_undoChangesBtn);
            this.gbEvent.Controls.Add(this.m_cbRetry);
            this.gbEvent.Controls.Add(this.m_retryUpDown);
            this.gbEvent.Controls.Add(this.label14);
            this.gbEvent.Controls.Add(this.m_stopButton);
            this.gbEvent.Controls.Add(this.m_applyToRunningBtn);
            this.gbEvent.Controls.Add(this.m_startButton);
            this.gbEvent.Controls.Add(this.m_autoStartCheckBox);
            this.gbEvent.Controls.Add(this.m_enableCheckBox);
            this.gbEvent.Controls.Add(this.m_failTimeUpDown);
            this.gbEvent.Controls.Add(this.label10);
            this.gbEvent.Name = "gbEvent";
            this.gbEvent.TabStop = false;
            // 
            // gbSelection
            // 
            this.gbSelection.Controls.Add(this.label11);
            this.gbSelection.Controls.Add(this.m_cbJobTrigger);
            this.gbSelection.Controls.Add(this.label1);
            this.gbSelection.Controls.Add(this.fpnlEvent);
            this.gbSelection.Controls.Add(this.tbEventServer);
            this.gbSelection.Controls.Add(this.btnEventServer);
            this.gbSelection.Controls.Add(this.label2);
            this.gbSelection.Controls.Add(this.tbEventServerPort);
            resources.ApplyResources(this.gbSelection, "gbSelection");
            this.gbSelection.Name = "gbSelection";
            this.gbSelection.TabStop = false;
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // m_cbJobTrigger
            // 
            this.m_cbJobTrigger.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbJobTrigger.FormattingEnabled = true;
            resources.ApplyResources(this.m_cbJobTrigger, "m_cbJobTrigger");
            this.m_cbJobTrigger.Name = "m_cbJobTrigger";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // fpnlEvent
            // 
            resources.ApplyResources(this.fpnlEvent, "fpnlEvent");
            this.fpnlEvent.Name = "fpnlEvent";
            // 
            // tbEventServer
            // 
            resources.ApplyResources(this.tbEventServer, "tbEventServer");
            this.tbEventServer.Name = "tbEventServer";
            this.tbEventServer.ReadOnly = true;
            // 
            // btnEventServer
            // 
            resources.ApplyResources(this.btnEventServer, "btnEventServer");
            this.btnEventServer.Name = "btnEventServer";
            this.btnEventServer.UseVisualStyleBackColor = true;
            this.btnEventServer.Click += new System.EventHandler(this.btnEventServer_Click);
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tbEventServerPort
            // 
            resources.ApplyResources(this.tbEventServerPort, "tbEventServerPort");
            this.tbEventServerPort.Name = "tbEventServerPort";
            this.tbEventServerPort.ReadOnly = true;
            // 
            // m_cbRepErr
            // 
            resources.ApplyResources(this.m_cbRepErr, "m_cbRepErr");
            this.m_cbRepErr.Name = "m_cbRepErr";
            this.m_cbRepErr.UseVisualStyleBackColor = true;
            // 
            // m_cbInitialScanEnabled
            // 
            resources.ApplyResources(this.m_cbInitialScanEnabled, "m_cbInitialScanEnabled");
            this.m_cbInitialScanEnabled.Name = "m_cbInitialScanEnabled";
            this.m_cbInitialScanEnabled.UseVisualStyleBackColor = true;
            // 
            // m_undoChangesBtn
            // 
            resources.ApplyResources(this.m_undoChangesBtn, "m_undoChangesBtn");
            this.m_undoChangesBtn.Name = "m_undoChangesBtn";
            this.m_undoChangesBtn.UseVisualStyleBackColor = true;
            // 
            // m_cbRetry
            // 
            resources.ApplyResources(this.m_cbRetry, "m_cbRetry");
            this.m_cbRetry.Name = "m_cbRetry";
            this.m_cbRetry.UseVisualStyleBackColor = true;
            this.m_cbRetry.CheckedChanged += new System.EventHandler(this.m_cbRetry_CheckedChanged);
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
            // m_stopButton
            // 
            resources.ApplyResources(this.m_stopButton, "m_stopButton");
            this.m_stopButton.Name = "m_stopButton";
            this.m_stopButton.UseVisualStyleBackColor = true;
            // 
            // m_applyToRunningBtn
            // 
            resources.ApplyResources(this.m_applyToRunningBtn, "m_applyToRunningBtn");
            this.m_applyToRunningBtn.Name = "m_applyToRunningBtn";
            this.m_applyToRunningBtn.UseVisualStyleBackColor = true;
            // 
            // m_startButton
            // 
            resources.ApplyResources(this.m_startButton, "m_startButton");
            this.m_startButton.Name = "m_startButton";
            this.m_startButton.UseVisualStyleBackColor = true;
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
            this.m_cbTimeBase.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.m_cbTimeBase.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbTimeBase.FormattingEnabled = true;
            resources.ApplyResources(this.m_cbTimeBase, "m_cbTimeBase");
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
            this.m_nudStopSeconds.ValueChanged += new System.EventHandler(this.OnStopChanged);
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
            this.m_nudStopMinutes.ValueChanged += new System.EventHandler(this.OnStopChanged);
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
            this.m_nudStopHours.ValueChanged += new System.EventHandler(this.OnStopChanged);
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
            this.m_nudStopDays.ValueChanged += new System.EventHandler(this.OnStopChanged);
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
            this.m_nudStartSeconds.ValueChanged += new System.EventHandler(this.OnStartChanged);
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
            this.m_nudStartMinutes.ValueChanged += new System.EventHandler(this.OnStartChanged);
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
            this.m_nudStartHours.ValueChanged += new System.EventHandler(this.OnStartChanged);
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
            this.m_nudStartDays.ValueChanged += new System.EventHandler(this.OnStartChanged);
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // PanelEventJob
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbEvent);
            this.Controls.Add(this.gbHD);
            this.Controls.Add(this.gbTimeSelection);
            this.Name = "PanelEventJob";
            this.gbEvent.ResumeLayout(false);
            this.gbEvent.PerformLayout();
            this.gbSelection.ResumeLayout(false);
            this.gbSelection.PerformLayout();
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

        private iba.Utility.CollapsibleGroupBox gbEvent;
        public System.Windows.Forms.Button m_undoChangesBtn;
        private System.Windows.Forms.CheckBox m_cbRetry;
        private System.Windows.Forms.NumericUpDown m_retryUpDown;
        private System.Windows.Forms.Label label14;
        public System.Windows.Forms.Button m_stopButton;
        public System.Windows.Forms.Button m_applyToRunningBtn;
        public System.Windows.Forms.Button m_startButton;
        public System.Windows.Forms.CheckBox m_autoStartCheckBox;
        public System.Windows.Forms.CheckBox m_enableCheckBox;
        private System.Windows.Forms.NumericUpDown m_failTimeUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ToolTip m_toolTip;
        private iba.Utility.CollapsibleGroupBox gbHD;
        private iba.Utility.CollapsibleGroupBox gbTimeSelection;
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
        private System.Windows.Forms.CheckBox m_cbInitialScanEnabled;
        private System.Windows.Forms.CheckBox m_cbRepErr;
        private System.Windows.Forms.Button btnEventServer;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbEventServerPort;
        private System.Windows.Forms.TextBox tbEventServer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.FlowLayoutPanel fpnlEvent;
        private System.Windows.Forms.GroupBox gbSelection;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox m_cbJobTrigger;
    }
}
