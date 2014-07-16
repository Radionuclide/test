namespace iba.Controls
{
    partial class WeeklyTriggerSettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WeeklyTriggerSettingsControl));
            this.label2 = new System.Windows.Forms.Label();
            this.m_nudWeeks = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.m_cbSunday = new System.Windows.Forms.CheckBox();
            this.m_cbMonday = new System.Windows.Forms.CheckBox();
            this.m_cbWednesday = new System.Windows.Forms.CheckBox();
            this.m_cbTuesday = new System.Windows.Forms.CheckBox();
            this.m_cbSaturday = new System.Windows.Forms.CheckBox();
            this.m_cbFriday = new System.Windows.Forms.CheckBox();
            this.m_cbThursday = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudWeeks)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_nudWeeks
            // 
            resources.ApplyResources(this.m_nudWeeks, "m_nudWeeks");
            this.m_nudWeeks.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.m_nudWeeks.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudWeeks.Name = "m_nudWeeks";
            this.m_nudWeeks.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_cbSunday
            // 
            resources.ApplyResources(this.m_cbSunday, "m_cbSunday");
            this.m_cbSunday.Name = "m_cbSunday";
            this.m_cbSunday.UseVisualStyleBackColor = true;
            // 
            // m_cbMonday
            // 
            resources.ApplyResources(this.m_cbMonday, "m_cbMonday");
            this.m_cbMonday.Name = "m_cbMonday";
            this.m_cbMonday.UseVisualStyleBackColor = true;
            // 
            // m_cbWednesday
            // 
            resources.ApplyResources(this.m_cbWednesday, "m_cbWednesday");
            this.m_cbWednesday.Name = "m_cbWednesday";
            this.m_cbWednesday.UseVisualStyleBackColor = true;
            // 
            // m_cbTuesday
            // 
            resources.ApplyResources(this.m_cbTuesday, "m_cbTuesday");
            this.m_cbTuesday.Name = "m_cbTuesday";
            this.m_cbTuesday.UseVisualStyleBackColor = true;
            // 
            // m_cbSaturday
            // 
            resources.ApplyResources(this.m_cbSaturday, "m_cbSaturday");
            this.m_cbSaturday.Name = "m_cbSaturday";
            this.m_cbSaturday.UseVisualStyleBackColor = true;
            // 
            // m_cbFriday
            // 
            resources.ApplyResources(this.m_cbFriday, "m_cbFriday");
            this.m_cbFriday.Name = "m_cbFriday";
            this.m_cbFriday.UseVisualStyleBackColor = true;
            // 
            // m_cbThursday
            // 
            resources.ApplyResources(this.m_cbThursday, "m_cbThursday");
            this.m_cbThursday.Name = "m_cbThursday";
            this.m_cbThursday.UseVisualStyleBackColor = true;
            // 
            // WeeklyTriggerSettingsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cbSaturday);
            this.Controls.Add(this.m_cbFriday);
            this.Controls.Add(this.m_cbThursday);
            this.Controls.Add(this.m_cbWednesday);
            this.Controls.Add(this.m_cbTuesday);
            this.Controls.Add(this.m_cbMonday);
            this.Controls.Add(this.m_cbSunday);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_nudWeeks);
            this.Controls.Add(this.label1);
            this.Name = "WeeklyTriggerSettingsControl";
            ((System.ComponentModel.ISupportInitialize)(this.m_nudWeeks)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        public System.Windows.Forms.NumericUpDown m_nudWeeks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox m_cbSunday;
        private System.Windows.Forms.CheckBox m_cbMonday;
        private System.Windows.Forms.CheckBox m_cbWednesday;
        private System.Windows.Forms.CheckBox m_cbTuesday;
        private System.Windows.Forms.CheckBox m_cbSaturday;
        private System.Windows.Forms.CheckBox m_cbFriday;
        private System.Windows.Forms.CheckBox m_cbThursday;
    }
}
