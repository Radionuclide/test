namespace iba.TKS_XML_Plugin
{
    partial class PluginXMLControl
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
            this.m_rbDU = new System.Windows.Forms.RadioButton();
            this.m_rbDO = new System.Windows.Forms.RadioButton();
            this.m_gbStandort = new System.Windows.Forms.GroupBox();
            this.m_rbBO = new System.Windows.Forms.RadioButton();
            this.m_gbStandort.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_rbDU
            // 
            this.m_rbDU.AutoSize = true;
            this.m_rbDU.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbDU.Location = new System.Drawing.Point(35, 64);
            this.m_rbDU.Name = "m_rbDU";
            this.m_rbDU.Size = new System.Drawing.Size(67, 17);
            this.m_rbDU.TabIndex = 2;
            this.m_rbDU.TabStop = true;
            this.m_rbDU.Text = "Duisburg";
            this.m_rbDU.UseVisualStyleBackColor = true;
            // 
            // m_rbDO
            // 
            this.m_rbDO.AutoSize = true;
            this.m_rbDO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbDO.Location = new System.Drawing.Point(35, 41);
            this.m_rbDO.Name = "m_rbDO";
            this.m_rbDO.Size = new System.Drawing.Size(71, 17);
            this.m_rbDO.TabIndex = 1;
            this.m_rbDO.TabStop = true;
            this.m_rbDO.Text = "Dortmund";
            this.m_rbDO.UseVisualStyleBackColor = true;
            // 
            // m_gbStandort
            // 
            this.m_gbStandort.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_gbStandort.Controls.Add(this.m_rbBO);
            this.m_gbStandort.Controls.Add(this.m_rbDU);
            this.m_gbStandort.Controls.Add(this.m_rbDO);
            this.m_gbStandort.Location = new System.Drawing.Point(0, 0);
            this.m_gbStandort.Name = "m_gbStandort";
            this.m_gbStandort.Size = new System.Drawing.Size(665, 90);
            this.m_gbStandort.TabIndex = 0;
            this.m_gbStandort.TabStop = false;
            this.m_gbStandort.Text = "Standort";
            // 
            // m_rbBO
            // 
            this.m_rbBO.AutoSize = true;
            this.m_rbBO.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbBO.Location = new System.Drawing.Point(35, 18);
            this.m_rbBO.Name = "m_rbBO";
            this.m_rbBO.Size = new System.Drawing.Size(64, 17);
            this.m_rbBO.TabIndex = 0;
            this.m_rbBO.TabStop = true;
            this.m_rbBO.Text = "Bochum";
            this.m_rbBO.UseVisualStyleBackColor = true;
            // 
            // PluginXMLControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_gbStandort);
            this.Name = "PluginXMLControl";
            this.Size = new System.Drawing.Size(666, 90);
            this.m_gbStandort.ResumeLayout(false);
            this.m_gbStandort.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RadioButton m_rbDU;
        private System.Windows.Forms.RadioButton m_rbDO;
        private System.Windows.Forms.GroupBox m_gbStandort;
        private System.Windows.Forms.RadioButton m_rbBO;
    }
}
