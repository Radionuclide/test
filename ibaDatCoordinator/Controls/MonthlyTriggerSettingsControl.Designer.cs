namespace iba.Controls
{
    partial class MonthlyTriggerSettingsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MonthlyTriggerSettingsControl));
            this.label1 = new System.Windows.Forms.Label();
            this.m_cbMonths = new iba.Controls.CustomComboMonths();
            this.m_rbDays = new System.Windows.Forms.RadioButton();
            this.m_rbOn = new System.Windows.Forms.RadioButton();
            this.m_cbDays = new iba.Controls.CustomComboDays();
            this.m_cbOnPart1 = new iba.Controls.CustomComboOnPart1();
            this.m_cbOnPartWeekday = new iba.Controls.CustomComboOnWeekdays();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_cbMonths
            // 
            resources.ApplyResources(this.m_cbMonths, "m_cbMonths");
            this.m_cbMonths.Name = "m_cbMonths";
            // 
            // m_rbDays
            // 
            resources.ApplyResources(this.m_rbDays, "m_rbDays");
            this.m_rbDays.Name = "m_rbDays";
            this.m_rbDays.TabStop = true;
            this.m_rbDays.UseVisualStyleBackColor = true;
            // 
            // m_rbOn
            // 
            resources.ApplyResources(this.m_rbOn, "m_rbOn");
            this.m_rbOn.Name = "m_rbOn";
            this.m_rbOn.TabStop = true;
            this.m_rbOn.UseVisualStyleBackColor = true;
            // 
            // m_cbDays
            // 
            resources.ApplyResources(this.m_cbDays, "m_cbDays");
            this.m_cbDays.Name = "m_cbDays";
            // 
            // m_cbOnPart1
            // 
            resources.ApplyResources(this.m_cbOnPart1, "m_cbOnPart1");
            this.m_cbOnPart1.Name = "m_cbOnPart1";
            // 
            // m_cbOnPartWeekday
            // 
            resources.ApplyResources(this.m_cbOnPartWeekday, "m_cbOnPartWeekday");
            this.m_cbOnPartWeekday.Name = "m_cbOnPartWeekday";
            // 
            // MonthlyTriggerSettingsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cbOnPartWeekday);
            this.Controls.Add(this.m_cbOnPart1);
            this.Controls.Add(this.m_cbDays);
            this.Controls.Add(this.m_rbOn);
            this.Controls.Add(this.m_rbDays);
            this.Controls.Add(this.m_cbMonths);
            this.Controls.Add(this.label1);
            this.Name = "MonthlyTriggerSettingsControl";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private CustomComboMonths m_cbMonths;
        private System.Windows.Forms.RadioButton m_rbDays;
        private System.Windows.Forms.RadioButton m_rbOn;
        private CustomComboDays m_cbDays;
        private CustomComboOnPart1 m_cbOnPart1;
        private CustomComboOnWeekdays m_cbOnPartWeekday;
    }
}
