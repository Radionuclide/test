namespace iba.Processing
{
    partial class DailyTriggerSettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DailyTriggerSettingsControl));
            this.label1 = new System.Windows.Forms.Label();
            this.m_nudDays = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDays)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_nudDays
            // 
            resources.ApplyResources(this.m_nudDays, "m_nudDays");
            this.m_nudDays.Maximum = new decimal(new int[] {
            999,
            0,
            0,
            0});
            this.m_nudDays.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudDays.Name = "m_nudDays";
            this.m_nudDays.Value = new decimal(new int[] {
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
            // DailyTriggerSettingsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_nudDays);
            this.Controls.Add(this.label1);
            this.Name = "DailyTriggerSettingsControl";
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDays)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.NumericUpDown m_nudDays;
        private System.Windows.Forms.Label label2;
    }
}
