namespace iba.Controls
{
    partial class ServiceSettingsControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceSettingsControl));
            this.m_gbApp = new iba.Utility.CollapsibleGroupBox();
            this.m_btnOptimize = new System.Windows.Forms.Button();
            this.m_comboPriority = new System.Windows.Forms.ComboBox();
            this.m_lblPriority = new System.Windows.Forms.Label();
            this.m_cbAutoStart = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new iba.Utility.CollapsibleGroupBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_nudResourceCritical = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.m_nudPostponeTime = new System.Windows.Forms.NumericUpDown();
            this.m_cbPostpone = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new iba.Utility.CollapsibleGroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.m_nudRestartIbaAnalyzer = new System.Windows.Forms.NumericUpDown();
            this.m_nudMaxIbaAnalyzers = new System.Windows.Forms.NumericUpDown();
            this.m_cbRestartIbaAnalyzer = new System.Windows.Forms.CheckBox();
            this.m_tbAnalyzerExe = new System.Windows.Forms.TextBox();
            this.m_browseIbaAnalyzerButton = new System.Windows.Forms.Button();
            this.m_registerButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2 = new iba.Utility.CollapsibleGroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_ClearPassBtn = new System.Windows.Forms.Button();
            this.m_nudRememberTime = new System.Windows.Forms.NumericUpDown();
            this.m_SetChangePassBtn = new System.Windows.Forms.Button();
            this.m_cbRememberPassword = new System.Windows.Forms.CheckBox();
            this.m_passwordStatusLabel = new System.Windows.Forms.Label();
            this.gb_GlobalCleanup = new iba.Utility.CollapsibleGroupBox();
            this.tbl_GlobalCleanup = new System.Windows.Forms.TableLayoutPanel();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.m_gbApp.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudResourceCritical)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudPostponeTime)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRestartIbaAnalyzer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMaxIbaAnalyzers)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRememberTime)).BeginInit();
            this.gb_GlobalCleanup.SuspendLayout();
            this.tbl_GlobalCleanup.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_gbApp
            // 
            resources.ApplyResources(this.m_gbApp, "m_gbApp");
            this.m_gbApp.Controls.Add(this.m_btnOptimize);
            this.m_gbApp.Controls.Add(this.m_comboPriority);
            this.m_gbApp.Controls.Add(this.m_lblPriority);
            this.m_gbApp.Controls.Add(this.m_cbAutoStart);
            this.m_gbApp.Name = "m_gbApp";
            this.m_gbApp.TabStop = false;
            // 
            // m_btnOptimize
            // 
            resources.ApplyResources(this.m_btnOptimize, "m_btnOptimize");
            this.m_btnOptimize.Name = "m_btnOptimize";
            this.m_toolTip.SetToolTip(this.m_btnOptimize, resources.GetString("m_btnOptimize.ToolTip"));
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
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.m_nudResourceCritical);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.m_nudPostponeTime);
            this.groupBox1.Controls.Add(this.m_cbPostpone);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // m_nudResourceCritical
            // 
            resources.ApplyResources(this.m_nudResourceCritical, "m_nudResourceCritical");
            this.m_nudResourceCritical.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.m_nudResourceCritical.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudResourceCritical.Name = "m_nudResourceCritical";
            this.m_nudResourceCritical.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_nudPostponeTime
            // 
            resources.ApplyResources(this.m_nudPostponeTime, "m_nudPostponeTime");
            this.m_nudPostponeTime.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.m_nudPostponeTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudPostponeTime.Name = "m_nudPostponeTime";
            this.m_nudPostponeTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // m_cbPostpone
            // 
            resources.ApplyResources(this.m_cbPostpone, "m_cbPostpone");
            this.m_cbPostpone.Name = "m_cbPostpone";
            this.m_cbPostpone.UseVisualStyleBackColor = true;
            this.m_cbPostpone.CheckedChanged += new System.EventHandler(this.m_cbPostpone_CheckedChanged);
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.label6);
            this.groupBox5.Controls.Add(this.label4);
            this.groupBox5.Controls.Add(this.m_nudRestartIbaAnalyzer);
            this.groupBox5.Controls.Add(this.m_nudMaxIbaAnalyzers);
            this.groupBox5.Controls.Add(this.m_cbRestartIbaAnalyzer);
            this.groupBox5.Controls.Add(this.m_tbAnalyzerExe);
            this.groupBox5.Controls.Add(this.m_browseIbaAnalyzerButton);
            this.groupBox5.Controls.Add(this.m_registerButton);
            this.groupBox5.Controls.Add(this.m_executeIBAAButton);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // m_nudRestartIbaAnalyzer
            // 
            resources.ApplyResources(this.m_nudRestartIbaAnalyzer, "m_nudRestartIbaAnalyzer");
            this.m_nudRestartIbaAnalyzer.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.m_nudRestartIbaAnalyzer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudRestartIbaAnalyzer.Name = "m_nudRestartIbaAnalyzer";
            this.m_nudRestartIbaAnalyzer.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // m_nudMaxIbaAnalyzers
            // 
            resources.ApplyResources(this.m_nudMaxIbaAnalyzers, "m_nudMaxIbaAnalyzers");
            this.m_nudMaxIbaAnalyzers.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.m_nudMaxIbaAnalyzers.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudMaxIbaAnalyzers.Name = "m_nudMaxIbaAnalyzers";
            this.m_nudMaxIbaAnalyzers.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // m_cbRestartIbaAnalyzer
            // 
            resources.ApplyResources(this.m_cbRestartIbaAnalyzer, "m_cbRestartIbaAnalyzer");
            this.m_cbRestartIbaAnalyzer.Name = "m_cbRestartIbaAnalyzer";
            this.m_cbRestartIbaAnalyzer.UseVisualStyleBackColor = true;
            this.m_cbRestartIbaAnalyzer.CheckedChanged += new System.EventHandler(this.m_cbRestartIbaAnalyzer_CheckedChanged);
            // 
            // m_tbAnalyzerExe
            // 
            resources.ApplyResources(this.m_tbAnalyzerExe, "m_tbAnalyzerExe");
            this.m_tbAnalyzerExe.Name = "m_tbAnalyzerExe";
            this.m_tbAnalyzerExe.TextChanged += new System.EventHandler(this.m_tbAnalyzerExe_TextChanged);
            // 
            // m_browseIbaAnalyzerButton
            // 
            resources.ApplyResources(this.m_browseIbaAnalyzerButton, "m_browseIbaAnalyzerButton");
            this.m_browseIbaAnalyzerButton.Image = global::iba.Properties.Resources.open;
            this.m_browseIbaAnalyzerButton.Name = "m_browseIbaAnalyzerButton";
            this.m_browseIbaAnalyzerButton.UseVisualStyleBackColor = true;
            this.m_browseIbaAnalyzerButton.Click += new System.EventHandler(this.m_browseIbaAnalyzerButton_Click);
            // 
            // m_registerButton
            // 
            resources.ApplyResources(this.m_registerButton, "m_registerButton");
            this.m_registerButton.Image = global::iba.Properties.Resources.Register;
            this.m_registerButton.Name = "m_registerButton";
            this.m_registerButton.UseVisualStyleBackColor = true;
            this.m_registerButton.Click += new System.EventHandler(this.m_registerButton_Click);
            // 
            // m_executeIBAAButton
            // 
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.Analyzer_001;
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.m_ClearPassBtn);
            this.groupBox2.Controls.Add(this.m_nudRememberTime);
            this.groupBox2.Controls.Add(this.m_SetChangePassBtn);
            this.groupBox2.Controls.Add(this.m_cbRememberPassword);
            this.groupBox2.Controls.Add(this.m_passwordStatusLabel);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_ClearPassBtn
            // 
            resources.ApplyResources(this.m_ClearPassBtn, "m_ClearPassBtn");
            this.m_ClearPassBtn.Name = "m_ClearPassBtn";
            this.m_ClearPassBtn.UseVisualStyleBackColor = true;
            this.m_ClearPassBtn.Click += new System.EventHandler(this.m_ClearPassBtn_Click);
            // 
            // m_nudRememberTime
            // 
            resources.ApplyResources(this.m_nudRememberTime, "m_nudRememberTime");
            this.m_nudRememberTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudRememberTime.Name = "m_nudRememberTime";
            this.m_nudRememberTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // m_SetChangePassBtn
            // 
            resources.ApplyResources(this.m_SetChangePassBtn, "m_SetChangePassBtn");
            this.m_SetChangePassBtn.Name = "m_SetChangePassBtn";
            this.m_SetChangePassBtn.UseVisualStyleBackColor = true;
            this.m_SetChangePassBtn.Click += new System.EventHandler(this.m_SetChangePassBtn_Click);
            // 
            // m_cbRememberPassword
            // 
            resources.ApplyResources(this.m_cbRememberPassword, "m_cbRememberPassword");
            this.m_cbRememberPassword.Name = "m_cbRememberPassword";
            this.m_cbRememberPassword.UseVisualStyleBackColor = true;
            this.m_cbRememberPassword.CheckedChanged += new System.EventHandler(this.m_cbRememberPassword_CheckedChanged);
            // 
            // m_passwordStatusLabel
            // 
            resources.ApplyResources(this.m_passwordStatusLabel, "m_passwordStatusLabel");
            this.m_passwordStatusLabel.Name = "m_passwordStatusLabel";
            // 
            // gb_GlobalCleanup
            // 
            resources.ApplyResources(this.gb_GlobalCleanup, "gb_GlobalCleanup");
            this.gb_GlobalCleanup.Controls.Add(this.tbl_GlobalCleanup);
            this.gb_GlobalCleanup.Name = "gb_GlobalCleanup";
            this.gb_GlobalCleanup.TabStop = false;
            // 
            // tbl_GlobalCleanup
            // 
            resources.ApplyResources(this.tbl_GlobalCleanup, "tbl_GlobalCleanup");
            this.tbl_GlobalCleanup.Controls.Add(this.label7, 0, 0);
            this.tbl_GlobalCleanup.Controls.Add(this.label8, 1, 0);
            this.tbl_GlobalCleanup.Controls.Add(this.label9, 2, 0);
            this.tbl_GlobalCleanup.Controls.Add(this.label10, 3, 0);
            this.tbl_GlobalCleanup.Controls.Add(this.label11, 4, 0);
            this.tbl_GlobalCleanup.Controls.Add(this.label12, 5, 0);
            this.tbl_GlobalCleanup.Name = "tbl_GlobalCleanup";
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
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // ServiceSettingsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gb_GlobalCleanup);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_gbApp);
            this.MinimumSize = new System.Drawing.Size(720, 250);
            this.Name = "ServiceSettingsControl";
            this.m_gbApp.ResumeLayout(false);
            this.m_gbApp.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudResourceCritical)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudPostponeTime)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRestartIbaAnalyzer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMaxIbaAnalyzers)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRememberTime)).EndInit();
            this.gb_GlobalCleanup.ResumeLayout(false);
            this.gb_GlobalCleanup.PerformLayout();
            this.tbl_GlobalCleanup.ResumeLayout(false);
            this.tbl_GlobalCleanup.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private iba.Utility.CollapsibleGroupBox m_gbApp;
        private System.Windows.Forms.CheckBox m_cbAutoStart;
        private System.Windows.Forms.Label m_lblPriority;
        private System.Windows.Forms.ComboBox m_comboPriority;
        private iba.Utility.CollapsibleGroupBox groupBox1;
        private System.Windows.Forms.CheckBox m_cbPostpone;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown m_nudPostponeTime;
        private iba.Utility.CollapsibleGroupBox groupBox5;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button m_browseIbaAnalyzerButton;
        private System.Windows.Forms.Button m_registerButton;
        private System.Windows.Forms.TextBox m_tbAnalyzerExe;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.ToolTip m_toolTip;
        private iba.Utility.CollapsibleGroupBox groupBox2;
        private System.Windows.Forms.Label m_passwordStatusLabel;
        private System.Windows.Forms.Button m_ClearPassBtn;
        private System.Windows.Forms.Button m_SetChangePassBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown m_nudRememberTime;
        private System.Windows.Forms.CheckBox m_cbRememberPassword;
        private System.Windows.Forms.NumericUpDown m_nudResourceCritical;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown m_nudMaxIbaAnalyzers;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown m_nudRestartIbaAnalyzer;
        private System.Windows.Forms.CheckBox m_cbRestartIbaAnalyzer;
        private System.Windows.Forms.Button m_btnOptimize;
        private iba.Utility.CollapsibleGroupBox gb_GlobalCleanup;
        private System.Windows.Forms.TableLayoutPanel tbl_GlobalCleanup;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;

    }
}
