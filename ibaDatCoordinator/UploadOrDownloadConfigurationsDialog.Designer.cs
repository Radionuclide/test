namespace iba
{
    partial class UploadOrDownloadConfigurationsDialog
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UploadOrDownloadConfigurationsDialog));
            this.m_btDownload = new System.Windows.Forms.Button();
            this.m_btUpload = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // m_btDownload
            // 
            resources.ApplyResources(this.m_btDownload, "m_btDownload");
            this.m_btDownload.Name = "m_btDownload";
            this.m_btDownload.UseVisualStyleBackColor = true;
            this.m_btDownload.Click += new System.EventHandler(this.m_btDownload_Click);
            // 
            // m_btUpload
            // 
            resources.ApplyResources(this.m_btUpload, "m_btUpload");
            this.m_btUpload.Name = "m_btUpload";
            this.m_btUpload.UseVisualStyleBackColor = true;
            this.m_btUpload.Click += new System.EventHandler(this.m_btUpload_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // UploadOrDownloadConfigurationsDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ControlBox = false;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_btUpload);
            this.Controls.Add(this.m_btDownload);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "UploadOrDownloadConfigurationsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btDownload;
        private System.Windows.Forms.Button m_btUpload;
        private System.Windows.Forms.Label label1;

    }
}