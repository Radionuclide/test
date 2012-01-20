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
            this.m_gbApp = new System.Windows.Forms.GroupBox();
            this.m_comboPriority = new System.Windows.Forms.ComboBox();
            this.m_lblPriority = new System.Windows.Forms.Label();
            this.m_cbAutoStart = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_nudPostponeTime = new System.Windows.Forms.NumericUpDown();
            this.m_cbPostpone = new System.Windows.Forms.CheckBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.m_tbAnalyzerExe = new System.Windows.Forms.TextBox();
            this.m_browseIbaAnalyzerButton = new System.Windows.Forms.Button();
            this.m_registerButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_ClearPassBtn = new System.Windows.Forms.Button();
            this.m_SetChangePassBtn = new System.Windows.Forms.Button();
            this.m_passwordStatusLabel = new System.Windows.Forms.Label();
            this.m_gbApp.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudPostponeTime)).BeginInit();
            this.groupBox5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_gbApp
            // 
            resources.ApplyResources(this.m_gbApp, "m_gbApp");
            this.m_gbApp.Controls.Add(this.m_comboPriority);
            this.m_gbApp.Controls.Add(this.m_lblPriority);
            this.m_gbApp.Controls.Add(this.m_cbAutoStart);
            this.m_gbApp.Name = "m_gbApp";
            this.m_gbApp.TabStop = false;
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
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.m_nudPostponeTime);
            this.groupBox1.Controls.Add(this.m_cbPostpone);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
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
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.m_tbAnalyzerExe);
            this.groupBox5.Controls.Add(this.m_browseIbaAnalyzerButton);
            this.groupBox5.Controls.Add(this.m_registerButton);
            this.groupBox5.Controls.Add(this.m_executeIBAAButton);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
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
            this.groupBox2.Controls.Add(this.m_ClearPassBtn);
            this.groupBox2.Controls.Add(this.m_SetChangePassBtn);
            this.groupBox2.Controls.Add(this.m_passwordStatusLabel);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_ClearPassBtn
            // 
            resources.ApplyResources(this.m_ClearPassBtn, "m_ClearPassBtn");
            this.m_ClearPassBtn.Name = "m_ClearPassBtn";
            this.m_ClearPassBtn.UseVisualStyleBackColor = true;
            this.m_ClearPassBtn.Click += new System.EventHandler(this.m_ClearPassBtn_Click);
            // 
            // m_SetChangePassBtn
            // 
            resources.ApplyResources(this.m_SetChangePassBtn, "m_SetChangePassBtn");
            this.m_SetChangePassBtn.Name = "m_SetChangePassBtn";
            this.m_SetChangePassBtn.UseVisualStyleBackColor = true;
            this.m_SetChangePassBtn.Click += new System.EventHandler(this.m_SetChangePassBtn_Click);
            // 
            // m_passwordStatusLabel
            // 
            resources.ApplyResources(this.m_passwordStatusLabel, "m_passwordStatusLabel");
            this.m_passwordStatusLabel.Name = "m_passwordStatusLabel";
            // 
            // ServiceSettingsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_gbApp);
            this.MinimumSize = new System.Drawing.Size(620, 250);
            this.Name = "ServiceSettingsControl";
            this.m_gbApp.ResumeLayout(false);
            this.m_gbApp.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudPostponeTime)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_gbApp;
        private System.Windows.Forms.CheckBox m_cbAutoStart;
        private System.Windows.Forms.Label m_lblPriority;
        private System.Windows.Forms.ComboBox m_comboPriority;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox m_cbPostpone;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown m_nudPostponeTime;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button m_browseIbaAnalyzerButton;
        private System.Windows.Forms.Button m_registerButton;
        private System.Windows.Forms.TextBox m_tbAnalyzerExe;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label m_passwordStatusLabel;
        private System.Windows.Forms.Button m_ClearPassBtn;
        private System.Windows.Forms.Button m_SetChangePassBtn;

    }
}
