namespace iba.Dialogs
{
    partial class TestSplitterTaskDialog
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TestSplitterTaskDialog));
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
			this.m_btOK = new System.Windows.Forms.Button();
			this.m_browseFolderButton = new System.Windows.Forms.Button();
			this.m_targetFolderTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.m_btPerform = new System.Windows.Forms.Button();
			this.label1 = new System.Windows.Forms.Label();
			this.m_gvEntries = new System.Windows.Forms.DataGridView();
			this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colStart = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.colStop = new System.Windows.Forms.DataGridViewTextBoxColumn();
			this.label3 = new System.Windows.Forms.Label();
			this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.m_toolTip = new System.Windows.Forms.ToolTip();
			this.m_progressBar = new System.Windows.Forms.ProgressBar();
			this.m_lblProgress = new System.Windows.Forms.Label();
			this.m_bgwSplit = new System.ComponentModel.BackgroundWorker();
			this.m_bgwCalc = new System.ComponentModel.BackgroundWorker();
			((System.ComponentModel.ISupportInitialize)(this.m_gvEntries)).BeginInit();
			this.SuspendLayout();
			// 
			// m_btOK
			// 
			resources.ApplyResources(this.m_btOK, "m_btOK");
			this.m_btOK.Name = "m_btOK";
			this.m_btOK.UseVisualStyleBackColor = true;
			this.m_btOK.Click += new System.EventHandler(this.m_btOK_Click);
			// 
			// m_browseFolderButton
			// 
			resources.ApplyResources(this.m_browseFolderButton, "m_browseFolderButton");
			this.m_browseFolderButton.Image = Icons.Gui.All.Images.FolderOpen();
			this.m_browseFolderButton.Name = "m_browseFolderButton";
			this.m_toolTip.SetToolTip(this.m_browseFolderButton, resources.GetString("m_browseFolderButton.ToolTip"));
			this.m_browseFolderButton.UseVisualStyleBackColor = true;
			this.m_browseFolderButton.Click += new System.EventHandler(this.m_browseFolderButton_Click);
			// 
			// m_targetFolderTextBox
			// 
			this.m_targetFolderTextBox.AllowDrop = true;
			resources.ApplyResources(this.m_targetFolderTextBox, "m_targetFolderTextBox");
			this.m_targetFolderTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.m_targetFolderTextBox.Name = "m_targetFolderTextBox";
			this.m_targetFolderTextBox.TextChanged += new System.EventHandler(this.m_targetFolderTextBox_TextChanged);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// m_btPerform
			// 
			resources.ApplyResources(this.m_btPerform, "m_btPerform");
			this.m_btPerform.Image = Icons.Gui.All.Images.FolderOpen();
			this.m_btPerform.Name = "m_btPerform";
			this.m_btPerform.UseVisualStyleBackColor = true;
			this.m_btPerform.Click += new System.EventHandler(this.m_btPerform_Click);
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// m_gvEntries
			// 
			this.m_gvEntries.AllowUserToAddRows = false;
			this.m_gvEntries.AllowUserToDeleteRows = false;
			resources.ApplyResources(this.m_gvEntries, "m_gvEntries");
			dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_gvEntries.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
			this.m_gvEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.m_gvEntries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colStart,
            this.colStop});
			dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
			dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
			dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.m_gvEntries.DefaultCellStyle = dataGridViewCellStyle2;
			this.m_gvEntries.Name = "m_gvEntries";
			this.m_gvEntries.ReadOnly = true;
			dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
			dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
			dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.m_gvEntries.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
			// 
			// colName
			// 
			this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colName.FillWeight = 50F;
			resources.ApplyResources(this.colName, "colName");
			this.colName.Name = "colName";
			this.colName.ReadOnly = true;
			// 
			// colStart
			// 
			this.colStart.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colStart.FillWeight = 25F;
			resources.ApplyResources(this.colStart, "colStart");
			this.colStart.Name = "colStart";
			this.colStart.ReadOnly = true;
			// 
			// colStop
			// 
			this.colStop.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
			this.colStop.FillWeight = 25F;
			resources.ApplyResources(this.colStop, "colStop");
			this.colStop.Name = "colStop";
			this.colStop.ReadOnly = true;
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// m_progressBar
			// 
			resources.ApplyResources(this.m_progressBar, "m_progressBar");
			this.m_progressBar.Name = "m_progressBar";
			// 
			// m_lblProgress
			// 
			resources.ApplyResources(this.m_lblProgress, "m_lblProgress");
			this.m_lblProgress.AutoEllipsis = true;
			this.m_lblProgress.Name = "m_lblProgress";
			// 
			// m_bgwSplit
			// 
			this.m_bgwSplit.WorkerReportsProgress = true;
			this.m_bgwSplit.DoWork += new System.ComponentModel.DoWorkEventHandler(this.m_bgwSplit_DoWork);
			this.m_bgwSplit.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.m_bgwSplit_RunWorkerCompleted);
			// 
			// m_bgwCalc
			// 
			this.m_bgwCalc.WorkerReportsProgress = true;
			this.m_bgwCalc.DoWork += new System.ComponentModel.DoWorkEventHandler(this.m_bgwCalc_DoWork);
			this.m_bgwCalc.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.m_bgwCalc_RunWorkerCompleted);
			// 
			// TestSplitterTaskDialog
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_lblProgress);
			this.Controls.Add(this.m_progressBar);
			this.Controls.Add(this.label3);
			this.Controls.Add(this.m_gvEntries);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.m_btPerform);
			this.Controls.Add(this.m_browseFolderButton);
			this.Controls.Add(this.m_targetFolderTextBox);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.m_btOK);
			this.Name = "TestSplitterTaskDialog";
			((System.ComponentModel.ISupportInitialize)(this.m_gvEntries)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btOK;
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.TextBox m_targetFolderTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button m_btPerform;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView m_gvEntries;
        private System.Windows.Forms.DataGridViewTextBoxColumn colName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.ProgressBar m_progressBar;
        private System.Windows.Forms.Label m_lblProgress;
        private System.ComponentModel.BackgroundWorker m_bgwSplit;
        private System.ComponentModel.BackgroundWorker m_bgwCalc;
    }
}