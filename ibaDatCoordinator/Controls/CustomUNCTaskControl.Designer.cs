namespace iba.Controls
{
    partial class CustomUNCTaskControl
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
            this.m_gbTarget = new iba.Utility.CollapsibleGroupBox();
            this.panelOut = new System.Windows.Forms.Panel();
            this.m_pluginPanel = new System.Windows.Forms.Panel();
            this.m_gbTarget.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_gbTarget
            // 
            this.m_gbTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_gbTarget.Controls.Add(this.panelOut);
            this.m_gbTarget.Location = new System.Drawing.Point(0, 96);
            this.m_gbTarget.Name = "m_gbTarget";
            this.m_gbTarget.Size = new System.Drawing.Size(666, 498);
            this.m_gbTarget.TabIndex = 1;
            this.m_gbTarget.TabStop = false;
            this.m_gbTarget.Text = "Target";
            // 
            // panelOut
            // 
            this.panelOut.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelOut.Location = new System.Drawing.Point(11, 14);
            this.panelOut.Name = "panelOut";
            this.panelOut.Size = new System.Drawing.Size(644, 478);
            this.panelOut.TabIndex = 0;
            // 
            // m_pluginPanel
            // 
            this.m_pluginPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_pluginPanel.BackColor = System.Drawing.SystemColors.Control;
            this.m_pluginPanel.Location = new System.Drawing.Point(0, 0);
            this.m_pluginPanel.Name = "m_pluginPanel";
            this.m_pluginPanel.Size = new System.Drawing.Size(666, 91);
            this.m_pluginPanel.TabIndex = 0;
            // 
            // CustomUNCTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_pluginPanel);
            this.Controls.Add(this.m_gbTarget);
            this.MinimumSize = new System.Drawing.Size(0, 595);
            this.Name = "CustomUNCTaskControl";
            this.Size = new System.Drawing.Size(666, 600);
            this.m_gbTarget.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private iba.Utility.CollapsibleGroupBox m_gbTarget;
        private System.Windows.Forms.Panel panelOut;
        private System.Windows.Forms.Panel m_pluginPanel;
    }
}
