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
            this.m_rbDays = new System.Windows.Forms.RadioButton();
            this.m_rbOn = new System.Windows.Forms.RadioButton();
            this.m_cbOnPartWeekday = new iba.Utility.CheckBoxComboBox();
            this.m_cbOnPart1 = new iba.Utility.CheckBoxComboBox();
            this.m_cbDays = new iba.Utility.IntegerCheckComboBox();
            this.m_cbMonths = new iba.Utility.CheckBoxComboBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_rbDays
            // 
            resources.ApplyResources(this.m_rbDays, "m_rbDays");
            this.m_rbDays.Name = "m_rbDays";
            this.m_rbDays.TabStop = true;
            this.m_rbDays.UseVisualStyleBackColor = true;
            this.m_rbDays.CheckedChanged += new System.EventHandler(this.m_rbDays_CheckedChanged);
            // 
            // m_rbOn
            // 
            resources.ApplyResources(this.m_rbOn, "m_rbOn");
            this.m_rbOn.Name = "m_rbOn";
            this.m_rbOn.TabStop = true;
            this.m_rbOn.UseVisualStyleBackColor = true;
            this.m_rbOn.CheckedChanged += new System.EventHandler(this.m_rbOn_CheckedChanged);
            // 
            // m_cbOnPartWeekday
            // 
            resources.ApplyResources(this.m_cbOnPartWeekday, "m_cbOnPartWeekday");
            this.m_cbOnPartWeekday.Name = "m_cbOnPartWeekday";
            // 
            // m_cbOnPart1
            // 
            resources.ApplyResources(this.m_cbOnPart1, "m_cbOnPart1");
            this.m_cbOnPart1.Name = "m_cbOnPart1";
            // 
            // m_cbDays
            // 
            resources.ApplyResources(this.m_cbDays, "m_cbDays");
            this.m_cbDays.Name = "m_cbDays";
            // 
            // m_cbMonths
            // 
            resources.ApplyResources(this.m_cbMonths, "m_cbMonths");
            this.m_cbMonths.Name = "m_cbMonths";
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
        public iba.Utility.IntegerCheckComboBox m_cbDays;
        public iba.Utility.CheckBoxComboBox m_cbOnPart1;
        public iba.Utility.CheckBoxComboBox m_cbOnPartWeekday;
        public Utility.CheckBoxComboBox m_cbMonths;
        public System.Windows.Forms.RadioButton m_rbDays;
        public System.Windows.Forms.RadioButton m_rbOn;
    }
}
