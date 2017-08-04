namespace iba.DatCoordinator.Status
{
    partial class StatusForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusForm));
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.m_btTransferAnalyzerSettings = new System.Windows.Forms.Button();
            this.m_tbAnalyzerExe = new System.Windows.Forms.TextBox();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.m_gbApp = new System.Windows.Forms.GroupBox();
            this.m_btChange = new System.Windows.Forms.Button();
            this.m_tbPort = new System.Windows.Forms.TextBox();
            this.m_btnRestart = new System.Windows.Forms.Button();
            this.m_lblServiceStatus = new System.Windows.Forms.Label();
            this.m_btnStop = new System.Windows.Forms.Button();
            this.m_btnStart = new System.Windows.Forms.Button();
            this.m_lbServPort = new System.Windows.Forms.Label();
            this.m_lbServStatus = new System.Windows.Forms.Label();
            this.m_btnOptimize = new System.Windows.Forms.Button();
            this.m_comboPriority = new System.Windows.Forms.ComboBox();
            this.m_lblPriority = new System.Windows.Forms.Label();
            this.m_cbAutoStart = new System.Windows.Forms.CheckBox();
            this.m_timer = new System.Windows.Forms.Timer(this.components);
            this.groupBox5.SuspendLayout();
            this.m_gbApp.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.m_btTransferAnalyzerSettings);
            this.groupBox5.Controls.Add(this.m_tbAnalyzerExe);
            this.groupBox5.Controls.Add(this.m_executeIBAAButton);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // m_btTransferAnalyzerSettings
            // 
            resources.ApplyResources(this.m_btTransferAnalyzerSettings, "m_btTransferAnalyzerSettings");
            this.m_btTransferAnalyzerSettings.Name = "m_btTransferAnalyzerSettings";
            this.m_btTransferAnalyzerSettings.UseVisualStyleBackColor = true;
            this.m_btTransferAnalyzerSettings.Click += new System.EventHandler(this.m_btTransferAnalyzerSettings_Click);
            // 
            // m_tbAnalyzerExe
            // 
            resources.ApplyResources(this.m_tbAnalyzerExe, "m_tbAnalyzerExe");
            this.m_tbAnalyzerExe.Name = "m_tbAnalyzerExe";
            this.m_tbAnalyzerExe.ReadOnly = true;
            // 
            // m_executeIBAAButton
            // 
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.Image = global::iba.DatCoordinator.Status.Properties.Resources.Analyzer_001;
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // m_gbApp
            // 
            resources.ApplyResources(this.m_gbApp, "m_gbApp");
            this.m_gbApp.Controls.Add(this.m_btChange);
            this.m_gbApp.Controls.Add(this.m_tbPort);
            this.m_gbApp.Controls.Add(this.m_btnRestart);
            this.m_gbApp.Controls.Add(this.m_lblServiceStatus);
            this.m_gbApp.Controls.Add(this.m_btnStop);
            this.m_gbApp.Controls.Add(this.m_btnStart);
            this.m_gbApp.Controls.Add(this.m_lbServPort);
            this.m_gbApp.Controls.Add(this.m_lbServStatus);
            this.m_gbApp.Controls.Add(this.m_btnOptimize);
            this.m_gbApp.Controls.Add(this.m_comboPriority);
            this.m_gbApp.Controls.Add(this.m_lblPriority);
            this.m_gbApp.Controls.Add(this.m_cbAutoStart);
            this.m_gbApp.Name = "m_gbApp";
            this.m_gbApp.TabStop = false;
            // 
            // m_btChange
            // 
            resources.ApplyResources(this.m_btChange, "m_btChange");
            this.m_btChange.Name = "m_btChange";
            this.m_btChange.UseVisualStyleBackColor = true;
            this.m_btChange.Click += new System.EventHandler(this.btChangePort_Click);
            // 
            // m_tbPort
            // 
            resources.ApplyResources(this.m_tbPort, "m_tbPort");
            this.m_tbPort.Name = "m_tbPort";
            this.m_tbPort.ReadOnly = true;
            // 
            // m_btnRestart
            // 
            resources.ApplyResources(this.m_btnRestart, "m_btnRestart");
            this.m_btnRestart.Name = "m_btnRestart";
            this.m_btnRestart.UseVisualStyleBackColor = true;
            this.m_btnRestart.Click += new System.EventHandler(this.m_btnRestart_Click);
            // 
            // m_lblServiceStatus
            // 
            this.m_lblServiceStatus.BackColor = System.Drawing.Color.Red;
            this.m_lblServiceStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.m_lblServiceStatus, "m_lblServiceStatus");
            this.m_lblServiceStatus.Name = "m_lblServiceStatus";
            // 
            // m_btnStop
            // 
            resources.ApplyResources(this.m_btnStop, "m_btnStop");
            this.m_btnStop.Name = "m_btnStop";
            this.m_btnStop.UseVisualStyleBackColor = true;
            this.m_btnStop.Click += new System.EventHandler(this.m_btnStop_Click);
            // 
            // m_btnStart
            // 
            resources.ApplyResources(this.m_btnStart, "m_btnStart");
            this.m_btnStart.Name = "m_btnStart";
            this.m_btnStart.UseVisualStyleBackColor = true;
            this.m_btnStart.Click += new System.EventHandler(this.m_btnStart_Click);
            // 
            // m_lbServPort
            // 
            resources.ApplyResources(this.m_lbServPort, "m_lbServPort");
            this.m_lbServPort.Name = "m_lbServPort";
            // 
            // m_lbServStatus
            // 
            resources.ApplyResources(this.m_lbServStatus, "m_lbServStatus");
            this.m_lbServStatus.Name = "m_lbServStatus";
            // 
            // m_btnOptimize
            // 
            resources.ApplyResources(this.m_btnOptimize, "m_btnOptimize");
            this.m_btnOptimize.Name = "m_btnOptimize";
            this.m_btnOptimize.UseVisualStyleBackColor = true;
            this.m_btnOptimize.Click += new System.EventHandler(this.m_btnOptimize_Click);
            // 
            // m_comboPriority
            // 
            this.m_comboPriority.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_comboPriority.FormattingEnabled = true;
            this.m_comboPriority.Items.AddRange(new object[] {
            resources.GetString("m_comboPriority.Items"),
            resources.GetString("m_comboPriority.Items1"),
            resources.GetString("m_comboPriority.Items2"),
            resources.GetString("m_comboPriority.Items3"),
            resources.GetString("m_comboPriority.Items4"),
            resources.GetString("m_comboPriority.Items5")});
            resources.ApplyResources(this.m_comboPriority, "m_comboPriority");
            this.m_comboPriority.Name = "m_comboPriority";
            this.m_comboPriority.SelectedIndexChanged += new System.EventHandler(this.m_comboPriority_SelectedIndexChanged);
            // 
            // m_lblPriority
            // 
            resources.ApplyResources(this.m_lblPriority, "m_lblPriority");
            this.m_lblPriority.Name = "m_lblPriority";
            // 
            // m_cbAutoStart
            // 
            resources.ApplyResources(this.m_cbAutoStart, "m_cbAutoStart");
            this.m_cbAutoStart.Name = "m_cbAutoStart";
            this.m_cbAutoStart.UseVisualStyleBackColor = true;
            this.m_cbAutoStart.CheckedChanged += new System.EventHandler(this.m_cbAutoStart_CheckedChanged);
            // 
            // m_timer
            // 
            this.m_timer.Interval = 1000;
            this.m_timer.Tick += new System.EventHandler(this.m_timer_Tick);
            // 
            // StatusForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.m_gbApp);
            this.Name = "StatusForm";
            this.ShowInTaskbar = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.m_gbApp.ResumeLayout(false);
            this.m_gbApp.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button m_btTransferAnalyzerSettings;
        private System.Windows.Forms.TextBox m_tbAnalyzerExe;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox m_gbApp;
        private System.Windows.Forms.Label m_lblServiceStatus;
        private System.Windows.Forms.Button m_btnStop;
        private System.Windows.Forms.Button m_btnStart;
        private System.Windows.Forms.Label m_lbServPort;
        private System.Windows.Forms.Label m_lbServStatus;
        private System.Windows.Forms.Button m_btnOptimize;
        private System.Windows.Forms.ComboBox m_comboPriority;
        private System.Windows.Forms.Label m_lblPriority;
        private System.Windows.Forms.CheckBox m_cbAutoStart;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.Button m_btnRestart;
        private System.Windows.Forms.Timer m_timer;
        private System.Windows.Forms.Button m_btChange;
        private System.Windows.Forms.TextBox m_tbPort;
    }
}