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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServiceSettingsControl));
            this.m_gbApp = new System.Windows.Forms.GroupBox();
            this.m_comboPriority = new System.Windows.Forms.ComboBox();
            this.m_lblPriority = new System.Windows.Forms.Label();
            this.m_cbAutoStart = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_nudPostponeTime = new System.Windows.Forms.NumericUpDown();
            this.m_cbPostpone = new System.Windows.Forms.CheckBox();
            this.m_gbApp.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudPostponeTime)).BeginInit();
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
            // ServiceSettingsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_gbApp);
            this.MinimumSize = new System.Drawing.Size(620, 250);
            this.Name = "ServiceSettingsControl";
            this.m_gbApp.ResumeLayout(false);
            this.m_gbApp.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudPostponeTime)).EndInit();
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

    }
}
