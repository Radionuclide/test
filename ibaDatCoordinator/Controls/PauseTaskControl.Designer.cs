namespace iba.Controls
{
    partial class PauseTaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PauseTaskControl));
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.m_rbDatFileTime = new System.Windows.Forms.RadioButton();
            this.m_rbAbsolutePause = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.m_nudInterval = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            resources.ApplyResources(this.groupBox, "groupBox");
            this.groupBox.Controls.Add(this.m_rbDatFileTime);
            this.groupBox.Controls.Add(this.m_rbAbsolutePause);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.m_nudInterval);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.label3);
            this.groupBox.Name = "groupBox";
            this.groupBox.TabStop = false;
            // 
            // m_rbDatFileTime
            // 
            resources.ApplyResources(this.m_rbDatFileTime, "m_rbDatFileTime");
            this.m_rbDatFileTime.Name = "m_rbDatFileTime";
            this.m_rbDatFileTime.TabStop = true;
            this.m_rbDatFileTime.UseVisualStyleBackColor = true;
            // 
            // m_rbAbsolutePause
            // 
            resources.ApplyResources(this.m_rbAbsolutePause, "m_rbAbsolutePause");
            this.m_rbAbsolutePause.Name = "m_rbAbsolutePause";
            this.m_rbAbsolutePause.TabStop = true;
            this.m_rbAbsolutePause.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_nudInterval
            // 
            resources.ApplyResources(this.m_nudInterval, "m_nudInterval");
            this.m_nudInterval.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.m_nudInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudInterval.Name = "m_nudInterval";
            this.m_nudInterval.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // PauseTaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "PauseTaskControl";
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown m_nudInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton m_rbDatFileTime;
        private System.Windows.Forms.RadioButton m_rbAbsolutePause;
    }
}
