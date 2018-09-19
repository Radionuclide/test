namespace iba.Dialogs
{
    partial class GenerateEventJobTestFileDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GenerateEventJobTestFileDlg));
            this.m_btClose = new System.Windows.Forms.Button();
            this.m_rtbStatus = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // m_btClose
            // 
            resources.ApplyResources(this.m_btClose, "m_btClose");
            this.m_btClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.m_btClose.Name = "m_btClose";
            this.m_btClose.UseVisualStyleBackColor = true;
            // 
            // m_rtbStatus
            // 
            resources.ApplyResources(this.m_rtbStatus, "m_rtbStatus");
            this.m_rtbStatus.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.m_rtbStatus.DetectUrls = false;
            this.m_rtbStatus.Name = "m_rtbStatus";
            this.m_rtbStatus.ReadOnly = true;
            this.m_rtbStatus.ShortcutsEnabled = false;
            // 
            // GenerateEventJobTestFileDlg
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_btClose;
            this.Controls.Add(this.m_rtbStatus);
            this.Controls.Add(this.m_btClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GenerateEventJobTestFileDlg";
            this.ShowInTaskbar = false;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GenerateEventJobTestFileDlg_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button m_btClose;
        private System.Windows.Forms.RichTextBox m_rtbStatus;
    }
}