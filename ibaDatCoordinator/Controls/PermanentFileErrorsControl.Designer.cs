namespace iba.Controls
{
    partial class PermanentFileErrorsControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PermanentFileErrorsControl));
            this.m_infoLabel = new System.Windows.Forms.Label();
            this.m_confNameLinkLabel = new System.Windows.Forms.LinkLabel();
            this.m_refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.m_gridView = new System.Windows.Forms.DataGridView();
            this.m_check = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.DatFiles = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_attempts = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_refreshDats = new System.Windows.Forms.Button();
            this.m_deleteDats = new System.Windows.Forms.Button();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.m_gridView)).BeginInit();
            this.SuspendLayout();
            // 
            // m_infoLabel
            // 
            resources.ApplyResources(this.m_infoLabel, "m_infoLabel");
            this.m_infoLabel.Name = "m_infoLabel";
            // 
            // m_confNameLinkLabel
            // 
            resources.ApplyResources(this.m_confNameLinkLabel, "m_confNameLinkLabel");
            this.m_confNameLinkLabel.Name = "m_confNameLinkLabel";
            this.m_confNameLinkLabel.TabStop = true;
            this.m_confNameLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.m_confNameLinkLabel_LinkClicked);
            // 
            // m_refreshTimer
            // 
            this.m_refreshTimer.Interval = 500;
            this.m_refreshTimer.Tick += new System.EventHandler(this.OnChangedData);
            // 
            // m_gridView
            // 
            this.m_gridView.AllowUserToAddRows = false;
            this.m_gridView.AllowUserToDeleteRows = false;
            this.m_gridView.AllowUserToResizeColumns = false;
            this.m_gridView.AllowUserToResizeRows = false;
            resources.ApplyResources(this.m_gridView, "m_gridView");
            this.m_gridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_gridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_check,
            this.DatFiles,
            this.m_attempts});
            this.m_gridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.m_gridView.Name = "m_gridView";
            this.m_gridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.m_gridView.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_gridView_ColumnHeaderMouseClick);
            this.m_gridView.CurrentCellDirtyStateChanged += new System.EventHandler(this.m_gridView_CurrentCellDirtyStateChanged);
            // 
            // m_check
            // 
            this.m_check.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            resources.ApplyResources(this.m_check, "m_check");
            this.m_check.Name = "m_check";
            // 
            // DatFiles
            // 
            this.DatFiles.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.DatFiles.FillWeight = 90F;
            resources.ApplyResources(this.DatFiles, "DatFiles");
            this.DatFiles.Name = "DatFiles";
            this.DatFiles.ReadOnly = true;
            this.DatFiles.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_attempts
            // 
            this.m_attempts.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
            this.m_attempts.FillWeight = 15F;
            resources.ApplyResources(this.m_attempts, "m_attempts");
            this.m_attempts.Name = "m_attempts";
            this.m_attempts.ReadOnly = true;
            this.m_attempts.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_refreshDats
            // 
            resources.ApplyResources(this.m_refreshDats, "m_refreshDats");
            this.m_refreshDats.Image = Icons.Gui.All.Images.FileDatReprocessing(32);
            this.m_refreshDats.Name = "m_refreshDats";
            this.m_refreshDats.UseVisualStyleBackColor = true;
            this.m_refreshDats.Click += new System.EventHandler(this.m_refreshDats_Click);
            // 
            // m_deleteDats
            // 
            resources.ApplyResources(this.m_deleteDats, "m_deleteDats");
            this.m_deleteDats.Image = Icons.Gui.All.Images.FileDatDelete(32);
            this.m_deleteDats.Name = "m_deleteDats";
            this.m_deleteDats.UseVisualStyleBackColor = true;
            this.m_deleteDats.Click += new System.EventHandler(this.m_deleteDats_Click);
            // 
            // PermanentFileErrorsControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_deleteDats);
            this.Controls.Add(this.m_refreshDats);
            this.Controls.Add(this.m_gridView);
            this.Controls.Add(this.m_confNameLinkLabel);
            this.Controls.Add(this.m_infoLabel);
            this.MinimumSize = new System.Drawing.Size(720, 430);
            this.Name = "PermanentFileErrorsControl";
            ((System.ComponentModel.ISupportInitialize)(this.m_gridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label m_infoLabel;
        private System.Windows.Forms.LinkLabel m_confNameLinkLabel;
        private System.Windows.Forms.Timer m_refreshTimer;
        private System.Windows.Forms.DataGridView m_gridView;
        private System.Windows.Forms.Button m_refreshDats;
        private System.Windows.Forms.Button m_deleteDats;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.DataGridViewCheckBoxColumn m_check;
        private System.Windows.Forms.DataGridViewTextBoxColumn DatFiles;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_attempts;
    }
}
