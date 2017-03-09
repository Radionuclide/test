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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
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
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_progressBar = new System.Windows.Forms.ProgressBar();
            this.m_lblProgress = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.m_gvEntries)).BeginInit();
            this.SuspendLayout();
            // 
            // m_btOK
            // 
            this.m_btOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btOK.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btOK.Location = new System.Drawing.Point(453, 313);
            this.m_btOK.Name = "m_btOK";
            this.m_btOK.Size = new System.Drawing.Size(68, 23);
            this.m_btOK.TabIndex = 5;
            this.m_btOK.Text = "OK";
            this.m_btOK.UseVisualStyleBackColor = true;
            this.m_btOK.Click += new System.EventHandler(this.m_btOK_Click);
            // 
            // m_browseFolderButton
            // 
            this.m_browseFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseFolderButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFolderButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseFolderButton.Location = new System.Drawing.Point(435, 258);
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.Size = new System.Drawing.Size(40, 40);
            this.m_browseFolderButton.TabIndex = 9;
            this.m_toolTip.SetToolTip(this.m_browseFolderButton, "Browse for folder...");
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            this.m_browseFolderButton.Click += new System.EventHandler(this.m_browseFolderButton_Click);
            // 
            // m_targetFolderTextBox
            // 
            this.m_targetFolderTextBox.AllowDrop = true;
            this.m_targetFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_targetFolderTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
            this.m_targetFolderTextBox.Location = new System.Drawing.Point(5, 269);
            this.m_targetFolderTextBox.Name = "m_targetFolderTextBox";
            this.m_targetFolderTextBox.Size = new System.Drawing.Size(424, 20);
            this.m_targetFolderTextBox.TabIndex = 8;
            this.m_targetFolderTextBox.WordWrap = false;
            this.m_targetFolderTextBox.TextChanged += new System.EventHandler(this.m_targetFolderTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(-127, 269);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(86, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Dat file directory:";
            // 
            // m_btPerform
            // 
            this.m_btPerform.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btPerform.Image = global::iba.Properties.Resources.open;
            this.m_btPerform.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btPerform.Location = new System.Drawing.Point(481, 258);
            this.m_btPerform.Name = "m_btPerform";
            this.m_btPerform.Size = new System.Drawing.Size(40, 40);
            this.m_btPerform.TabIndex = 10;
            this.m_btPerform.UseVisualStyleBackColor = true;
            this.m_btPerform.Click += new System.EventHandler(this.m_btPerform_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(323, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "The .dat file will be split into new .dat files at the following locations:";
            // 
            // m_gvEntries
            // 
            this.m_gvEntries.AllowUserToAddRows = false;
            this.m_gvEntries.AllowUserToDeleteRows = false;
            this.m_gvEntries.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_gvEntries.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.m_gvEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_gvEntries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colStart,
            this.colStop});
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.m_gvEntries.DefaultCellStyle = dataGridViewCellStyle11;
            this.m_gvEntries.Location = new System.Drawing.Point(5, 37);
            this.m_gvEntries.Name = "m_gvEntries";
            this.m_gvEntries.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.m_gvEntries.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.m_gvEntries.Size = new System.Drawing.Size(516, 193);
            this.m_gvEntries.TabIndex = 12;
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.FillWeight = 50F;
            this.colName.HeaderText = "File Name";
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colStart
            // 
            this.colStart.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colStart.FillWeight = 25F;
            this.colStart.HeaderText = "Start";
            this.colStart.Name = "colStart";
            this.colStart.ReadOnly = true;
            // 
            // colStop
            // 
            this.colStop.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colStop.FillWeight = 25F;
            this.colStop.HeaderText = "Stop";
            this.colStop.Name = "colStop";
            this.colStop.ReadOnly = true;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(2, 253);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(93, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Test output folder:";
            // 
            // m_progressBar
            // 
            this.m_progressBar.Location = new System.Drawing.Point(5, 313);
            this.m_progressBar.Name = "m_progressBar";
            this.m_progressBar.Size = new System.Drawing.Size(424, 23);
            this.m_progressBar.TabIndex = 14;
            this.m_progressBar.Visible = false;
            // 
            // m_lblProgress
            // 
            this.m_lblProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_lblProgress.AutoEllipsis = true;
            this.m_lblProgress.Location = new System.Drawing.Point(2, 297);
            this.m_lblProgress.Name = "m_lblProgress";
            this.m_lblProgress.Size = new System.Drawing.Size(427, 13);
            this.m_lblProgress.TabIndex = 15;
            this.m_lblProgress.Text = "label4";
            this.m_lblProgress.Visible = false;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.WorkerReportsProgress = true;
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // TestSplitterTaskDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(528, 348);
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
            this.Text = "TestSplitterTaskDialog";
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
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}