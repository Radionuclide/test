namespace iba.Controls
{
    partial class StatusControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(StatusControl));
            this.label1 = new System.Windows.Forms.Label();
            this.m_confNameLinkLabel = new System.Windows.Forms.LinkLabel();
            this.m_infoText = new System.Windows.Forms.Label();
            this.m_refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.m_gridView = new System.Windows.Forms.DataGridView();
            this.DatFiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // m_confNameLinkLabel
            // 
            this.m_confNameLinkLabel.AccessibleDescription = null;
            this.m_confNameLinkLabel.AccessibleName = null;
            resources.ApplyResources(this.m_confNameLinkLabel, "m_confNameLinkLabel");
            this.m_confNameLinkLabel.Font = null;
            this.m_confNameLinkLabel.Name = "m_confNameLinkLabel";
            this.m_confNameLinkLabel.TabStop = true;
            this.m_confNameLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.m_confNameLinkLabel_LinkClicked);
            // 
            // m_infoText
            // 
            this.m_infoText.AccessibleDescription = null;
            this.m_infoText.AccessibleName = null;
            resources.ApplyResources(this.m_infoText, "m_infoText");
            this.m_infoText.Font = null;
            this.m_infoText.Name = "m_infoText";
            // 
            // m_refreshTimer
            // 
            this.m_refreshTimer.Tick += new System.EventHandler(this.OnChangedData);
            // 
            // m_gridView
            // 
            this.m_gridView.AccessibleDescription = null;
            this.m_gridView.AccessibleName = null;
            this.m_gridView.AllowUserToAddRows = false;
            this.m_gridView.AllowUserToDeleteRows = false;
            this.m_gridView.AllowUserToResizeColumns = false;
            this.m_gridView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.m_gridView, "m_gridView");
            this.m_gridView.BackgroundImage = null;
            this.m_gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_gridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DatFiles});
            this.m_gridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.m_gridView.Font = null;
            this.m_gridView.Name = "m_gridView";
            this.m_gridView.ReadOnly = true;
            this.m_gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // DatFiles
            // 
            this.DatFiles.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.DatFiles, "DatFiles");
            this.DatFiles.Name = "DatFiles";
            this.DatFiles.ReadOnly = true;
            // 
            // StatusControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.m_gridView);
            this.Controls.Add(this.m_infoText);
            this.Controls.Add(this.m_confNameLinkLabel);
            this.Controls.Add(this.label1);
            this.Font = null;
            this.MinimumSize = new System.Drawing.Size(620, 430);
            this.Name = "StatusControl";
            ((System.ComponentModel.ISupportInitialize)(this.m_gridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel m_confNameLinkLabel;
        private System.Windows.Forms.Label m_infoText;
        private System.Windows.Forms.Timer m_refreshTimer;
        private System.Windows.Forms.DataGridView m_gridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn DatFiles;
    }
}
