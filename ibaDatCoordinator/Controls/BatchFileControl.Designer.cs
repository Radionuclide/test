namespace iba.Controls
{
    partial class BatchFileControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchFileControl));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_batchFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browseBATCHFileButton = new System.Windows.Forms.Button();
            this.m_executeBatchFile = new System.Windows.Forms.Button();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_browsePDOFileButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.m_editorGroupBox = new System.Windows.Forms.GroupBox();
            this.m_batchFileEditTextBox = new System.Windows.Forms.TextBox();
            this.m_refreshButton = new System.Windows.Forms.Button();
            this.m_saveButton = new System.Windows.Forms.Button();
            this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.m_editorGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.m_batchFileTextBox);
            this.groupBox1.Controls.Add(this.m_browseBATCHFileButton);
            this.groupBox1.Controls.Add(this.m_executeBatchFile);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_batchFileTextBox
            // 
            resources.ApplyResources(this.m_batchFileTextBox, "m_batchFileTextBox");
            this.m_batchFileTextBox.Name = "m_batchFileTextBox";
            this.m_batchFileTextBox.TextChanged += new System.EventHandler(this.m_batchFileTextBox_TextChanged);
            // 
            // m_browseBATCHFileButton
            // 
            resources.ApplyResources(this.m_browseBATCHFileButton, "m_browseBATCHFileButton");
            this.m_browseBATCHFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseBATCHFileButton.Name = "m_browseBATCHFileButton";
            this.m_browseBATCHFileButton.UseVisualStyleBackColor = true;
            this.m_browseBATCHFileButton.Click += new System.EventHandler(this.m_browseBATCHFileButton_Click);
            // 
            // m_executeBatchFile
            // 
            resources.ApplyResources(this.m_executeBatchFile, "m_executeBatchFile");
            this.m_executeBatchFile.Image = global::iba.Properties.Resources.DOS;
            this.m_executeBatchFile.Name = "m_executeBatchFile";
            this.m_executeBatchFile.UseVisualStyleBackColor = true;
            this.m_executeBatchFile.Click += new System.EventHandler(this.m_executeBatchFile_Click);
            // 
            // m_pdoFileTextBox
            // 
            resources.ApplyResources(this.m_pdoFileTextBox, "m_pdoFileTextBox");
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.m_pdoFileTextBox);
            this.groupBox2.Controls.Add(this.m_browsePDOFileButton);
            this.groupBox2.Controls.Add(this.m_executeIBAAButton);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_browsePDOFileButton
            // 
            resources.ApplyResources(this.m_browsePDOFileButton, "m_browsePDOFileButton");
            this.m_browsePDOFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.Analyzer_001;
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // m_editorGroupBox
            // 
            resources.ApplyResources(this.m_editorGroupBox, "m_editorGroupBox");
            this.m_editorGroupBox.Controls.Add(this.m_batchFileEditTextBox);
            this.m_editorGroupBox.Controls.Add(this.m_refreshButton);
            this.m_editorGroupBox.Controls.Add(this.m_saveButton);
            this.m_editorGroupBox.Name = "m_editorGroupBox";
            this.m_editorGroupBox.TabStop = false;
            // 
            // m_batchFileEditTextBox
            // 
            resources.ApplyResources(this.m_batchFileEditTextBox, "m_batchFileEditTextBox");
            this.m_batchFileEditTextBox.Name = "m_batchFileEditTextBox";
            // 
            // m_refreshButton
            // 
            resources.ApplyResources(this.m_refreshButton, "m_refreshButton");
            this.m_refreshButton.Image = global::iba.Properties.Resources.Aktualisieren;
            this.m_refreshButton.Name = "m_refreshButton";
            this.m_refreshButton.UseVisualStyleBackColor = true;
            this.m_refreshButton.Click += new System.EventHandler(this.m_refreshButton_Click);
            // 
            // m_saveButton
            // 
            resources.ApplyResources(this.m_saveButton, "m_saveButton");
            this.m_saveButton.Image = global::iba.Properties.Resources.Speichern;
            this.m_saveButton.Name = "m_saveButton";
            this.m_saveButton.UseVisualStyleBackColor = true;
            this.m_saveButton.Click += new System.EventHandler(this.m_saveButton_Click);
            // 
            // BatchFileControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_editorGroupBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(566, 370);
            this.Name = "BatchFileControl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.m_editorGroupBox.ResumeLayout(false);
            this.m_editorGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Button m_browsePDOFileButton;
        private System.Windows.Forms.TextBox m_pdoFileTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox m_batchFileTextBox;
        private System.Windows.Forms.Button m_browseBATCHFileButton;
        private System.Windows.Forms.Button m_executeBatchFile;
        private System.Windows.Forms.GroupBox m_editorGroupBox;
        private System.Windows.Forms.TextBox m_batchFileEditTextBox;
        private System.Windows.Forms.Button m_refreshButton;
        private System.Windows.Forms.Button m_saveButton;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog1;

    }
}
