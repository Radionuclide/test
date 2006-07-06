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
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_nameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rb1stFailure = new System.Windows.Forms.RadioButton();
            this.m_rbAlways = new System.Windows.Forms.RadioButton();
            this.m_rbFailure = new System.Windows.Forms.RadioButton();
            this.m_rbDisabled = new System.Windows.Forms.RadioButton();
            this.m_rbSucces = new System.Windows.Forms.RadioButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbNot1stFailure = new System.Windows.Forms.RadioButton();
            this.m_rbNotAlways = new System.Windows.Forms.RadioButton();
            this.m_rbNotFailure = new System.Windows.Forms.RadioButton();
            this.m_rbNotDisabled = new System.Windows.Forms.RadioButton();
            this.m_rbNotSuccess = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.m_editorGroupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.m_batchFileTextBox);
            this.groupBox1.Controls.Add(this.m_browseBATCHFileButton);
            this.groupBox1.Controls.Add(this.m_executeBatchFile);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_batchFileTextBox
            // 
            this.m_batchFileTextBox.AccessibleDescription = null;
            this.m_batchFileTextBox.AccessibleName = null;
            resources.ApplyResources(this.m_batchFileTextBox, "m_batchFileTextBox");
            this.m_batchFileTextBox.BackgroundImage = null;
            this.m_batchFileTextBox.Font = null;
            this.m_batchFileTextBox.Name = "m_batchFileTextBox";
            this.m_batchFileTextBox.TextChanged += new System.EventHandler(this.m_batchFileTextBox_TextChanged);
            // 
            // m_browseBATCHFileButton
            // 
            this.m_browseBATCHFileButton.AccessibleDescription = null;
            this.m_browseBATCHFileButton.AccessibleName = null;
            resources.ApplyResources(this.m_browseBATCHFileButton, "m_browseBATCHFileButton");
            this.m_browseBATCHFileButton.BackgroundImage = null;
            this.m_browseBATCHFileButton.Font = null;
            this.m_browseBATCHFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseBATCHFileButton.Name = "m_browseBATCHFileButton";
            this.m_browseBATCHFileButton.UseVisualStyleBackColor = true;
            this.m_browseBATCHFileButton.Click += new System.EventHandler(this.m_browseBATCHFileButton_Click);
            // 
            // m_executeBatchFile
            // 
            this.m_executeBatchFile.AccessibleDescription = null;
            this.m_executeBatchFile.AccessibleName = null;
            resources.ApplyResources(this.m_executeBatchFile, "m_executeBatchFile");
            this.m_executeBatchFile.BackgroundImage = null;
            this.m_executeBatchFile.Font = null;
            this.m_executeBatchFile.Image = global::iba.Properties.Resources.DOS;
            this.m_executeBatchFile.Name = "m_executeBatchFile";
            this.m_executeBatchFile.UseVisualStyleBackColor = true;
            this.m_executeBatchFile.Click += new System.EventHandler(this.m_executeBatchFile_Click);
            // 
            // m_pdoFileTextBox
            // 
            this.m_pdoFileTextBox.AccessibleDescription = null;
            this.m_pdoFileTextBox.AccessibleName = null;
            resources.ApplyResources(this.m_pdoFileTextBox, "m_pdoFileTextBox");
            this.m_pdoFileTextBox.BackgroundImage = null;
            this.m_pdoFileTextBox.Font = null;
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.m_pdoFileTextBox);
            this.groupBox2.Controls.Add(this.m_browsePDOFileButton);
            this.groupBox2.Controls.Add(this.m_executeIBAAButton);
            this.groupBox2.Font = null;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_browsePDOFileButton
            // 
            this.m_browsePDOFileButton.AccessibleDescription = null;
            this.m_browsePDOFileButton.AccessibleName = null;
            resources.ApplyResources(this.m_browsePDOFileButton, "m_browsePDOFileButton");
            this.m_browsePDOFileButton.BackgroundImage = null;
            this.m_browsePDOFileButton.Font = null;
            this.m_browsePDOFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            this.m_executeIBAAButton.AccessibleDescription = null;
            this.m_executeIBAAButton.AccessibleName = null;
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.BackgroundImage = null;
            this.m_executeIBAAButton.Font = null;
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.Analyzer_001;
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // m_editorGroupBox
            // 
            this.m_editorGroupBox.AccessibleDescription = null;
            this.m_editorGroupBox.AccessibleName = null;
            resources.ApplyResources(this.m_editorGroupBox, "m_editorGroupBox");
            this.m_editorGroupBox.BackgroundImage = null;
            this.m_editorGroupBox.Controls.Add(this.m_batchFileEditTextBox);
            this.m_editorGroupBox.Controls.Add(this.m_refreshButton);
            this.m_editorGroupBox.Controls.Add(this.m_saveButton);
            this.m_editorGroupBox.Font = null;
            this.m_editorGroupBox.Name = "m_editorGroupBox";
            this.m_editorGroupBox.TabStop = false;
            // 
            // m_batchFileEditTextBox
            // 
            this.m_batchFileEditTextBox.AccessibleDescription = null;
            this.m_batchFileEditTextBox.AccessibleName = null;
            resources.ApplyResources(this.m_batchFileEditTextBox, "m_batchFileEditTextBox");
            this.m_batchFileEditTextBox.BackgroundImage = null;
            this.m_batchFileEditTextBox.Font = null;
            this.m_batchFileEditTextBox.Name = "m_batchFileEditTextBox";
            // 
            // m_refreshButton
            // 
            this.m_refreshButton.AccessibleDescription = null;
            this.m_refreshButton.AccessibleName = null;
            resources.ApplyResources(this.m_refreshButton, "m_refreshButton");
            this.m_refreshButton.BackgroundImage = null;
            this.m_refreshButton.Font = null;
            this.m_refreshButton.Image = global::iba.Properties.Resources.Aktualisieren;
            this.m_refreshButton.Name = "m_refreshButton";
            this.m_refreshButton.UseVisualStyleBackColor = true;
            this.m_refreshButton.Click += new System.EventHandler(this.m_refreshButton_Click);
            // 
            // m_saveButton
            // 
            this.m_saveButton.AccessibleDescription = null;
            this.m_saveButton.AccessibleName = null;
            resources.ApplyResources(this.m_saveButton, "m_saveButton");
            this.m_saveButton.BackgroundImage = null;
            this.m_saveButton.Font = null;
            this.m_saveButton.Image = global::iba.Properties.Resources.Speichern;
            this.m_saveButton.Name = "m_saveButton";
            this.m_saveButton.UseVisualStyleBackColor = true;
            this.m_saveButton.Click += new System.EventHandler(this.m_saveButton_Click);
            // 
            // m_openFileDialog1
            // 
            resources.ApplyResources(this.m_openFileDialog1, "m_openFileDialog1");
            // 
            // groupBox3
            // 
            this.groupBox3.AccessibleDescription = null;
            this.groupBox3.AccessibleName = null;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.BackgroundImage = null;
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.m_nameTextBox);
            this.groupBox3.Font = null;
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // m_nameTextBox
            // 
            this.m_nameTextBox.AccessibleDescription = null;
            this.m_nameTextBox.AccessibleName = null;
            resources.ApplyResources(this.m_nameTextBox, "m_nameTextBox");
            this.m_nameTextBox.BackgroundImage = null;
            this.m_nameTextBox.Font = null;
            this.m_nameTextBox.Name = "m_nameTextBox";
            this.m_nameTextBox.TextChanged += new System.EventHandler(this.m_nameTextBox_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.AccessibleDescription = null;
            this.groupBox4.AccessibleName = null;
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.BackgroundImage = null;
            this.groupBox4.Controls.Add(this.tableLayoutPanel1);
            this.groupBox4.Font = null;
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AccessibleDescription = null;
            this.tableLayoutPanel1.AccessibleName = null;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackgroundImage = null;
            this.tableLayoutPanel1.Controls.Add(this.m_rb1stFailure, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_rbAlways, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_rbFailure, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_rbDisabled, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_rbSucces, 1, 0);
            this.tableLayoutPanel1.Font = null;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // m_rb1stFailure
            // 
            this.m_rb1stFailure.AccessibleDescription = null;
            this.m_rb1stFailure.AccessibleName = null;
            resources.ApplyResources(this.m_rb1stFailure, "m_rb1stFailure");
            this.m_rb1stFailure.BackgroundImage = null;
            this.m_rb1stFailure.Font = null;
            this.m_rb1stFailure.Name = "m_rb1stFailure";
            this.m_rb1stFailure.TabStop = true;
            this.m_rb1stFailure.UseVisualStyleBackColor = true;
            this.m_rb1stFailure.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbAlways
            // 
            this.m_rbAlways.AccessibleDescription = null;
            this.m_rbAlways.AccessibleName = null;
            resources.ApplyResources(this.m_rbAlways, "m_rbAlways");
            this.m_rbAlways.BackgroundImage = null;
            this.m_rbAlways.Font = null;
            this.m_rbAlways.Name = "m_rbAlways";
            this.m_rbAlways.TabStop = true;
            this.m_rbAlways.UseVisualStyleBackColor = true;
            this.m_rbAlways.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbFailure
            // 
            this.m_rbFailure.AccessibleDescription = null;
            this.m_rbFailure.AccessibleName = null;
            resources.ApplyResources(this.m_rbFailure, "m_rbFailure");
            this.m_rbFailure.BackgroundImage = null;
            this.m_rbFailure.Font = null;
            this.m_rbFailure.Name = "m_rbFailure";
            this.m_rbFailure.TabStop = true;
            this.m_rbFailure.UseVisualStyleBackColor = true;
            this.m_rbFailure.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbDisabled
            // 
            this.m_rbDisabled.AccessibleDescription = null;
            this.m_rbDisabled.AccessibleName = null;
            resources.ApplyResources(this.m_rbDisabled, "m_rbDisabled");
            this.m_rbDisabled.BackgroundImage = null;
            this.m_rbDisabled.Font = null;
            this.m_rbDisabled.Name = "m_rbDisabled";
            this.m_rbDisabled.TabStop = true;
            this.m_rbDisabled.UseVisualStyleBackColor = true;
            this.m_rbDisabled.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbSucces
            // 
            this.m_rbSucces.AccessibleDescription = null;
            this.m_rbSucces.AccessibleName = null;
            resources.ApplyResources(this.m_rbSucces, "m_rbSucces");
            this.m_rbSucces.BackgroundImage = null;
            this.m_rbSucces.Font = null;
            this.m_rbSucces.Name = "m_rbSucces";
            this.m_rbSucces.TabStop = true;
            this.m_rbSucces.UseVisualStyleBackColor = true;
            this.m_rbSucces.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.AccessibleDescription = null;
            this.groupBox7.AccessibleName = null;
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.BackgroundImage = null;
            this.groupBox7.Controls.Add(this.tableLayoutPanel6);
            this.groupBox7.Font = null;
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AccessibleDescription = null;
            this.tableLayoutPanel6.AccessibleName = null;
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.BackgroundImage = null;
            this.tableLayoutPanel6.Controls.Add(this.m_rbNot1stFailure, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotAlways, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotFailure, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotDisabled, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotSuccess, 1, 0);
            this.tableLayoutPanel6.Font = null;
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // m_rbNot1stFailure
            // 
            this.m_rbNot1stFailure.AccessibleDescription = null;
            this.m_rbNot1stFailure.AccessibleName = null;
            resources.ApplyResources(this.m_rbNot1stFailure, "m_rbNot1stFailure");
            this.m_rbNot1stFailure.BackgroundImage = null;
            this.m_rbNot1stFailure.Font = null;
            this.m_rbNot1stFailure.Name = "m_rbNot1stFailure";
            this.m_rbNot1stFailure.TabStop = true;
            this.m_rbNot1stFailure.UseVisualStyleBackColor = true;
            // 
            // m_rbNotAlways
            // 
            this.m_rbNotAlways.AccessibleDescription = null;
            this.m_rbNotAlways.AccessibleName = null;
            resources.ApplyResources(this.m_rbNotAlways, "m_rbNotAlways");
            this.m_rbNotAlways.BackgroundImage = null;
            this.m_rbNotAlways.Font = null;
            this.m_rbNotAlways.Name = "m_rbNotAlways";
            this.m_rbNotAlways.TabStop = true;
            this.m_rbNotAlways.UseVisualStyleBackColor = true;
            // 
            // m_rbNotFailure
            // 
            this.m_rbNotFailure.AccessibleDescription = null;
            this.m_rbNotFailure.AccessibleName = null;
            resources.ApplyResources(this.m_rbNotFailure, "m_rbNotFailure");
            this.m_rbNotFailure.BackgroundImage = null;
            this.m_rbNotFailure.Font = null;
            this.m_rbNotFailure.Name = "m_rbNotFailure";
            this.m_rbNotFailure.TabStop = true;
            this.m_rbNotFailure.UseVisualStyleBackColor = true;
            // 
            // m_rbNotDisabled
            // 
            this.m_rbNotDisabled.AccessibleDescription = null;
            this.m_rbNotDisabled.AccessibleName = null;
            resources.ApplyResources(this.m_rbNotDisabled, "m_rbNotDisabled");
            this.m_rbNotDisabled.BackgroundImage = null;
            this.m_rbNotDisabled.Font = null;
            this.m_rbNotDisabled.Name = "m_rbNotDisabled";
            this.m_rbNotDisabled.TabStop = true;
            this.m_rbNotDisabled.UseVisualStyleBackColor = true;
            // 
            // m_rbNotSuccess
            // 
            this.m_rbNotSuccess.AccessibleDescription = null;
            this.m_rbNotSuccess.AccessibleName = null;
            resources.ApplyResources(this.m_rbNotSuccess, "m_rbNotSuccess");
            this.m_rbNotSuccess.BackgroundImage = null;
            this.m_rbNotSuccess.Font = null;
            this.m_rbNotSuccess.Name = "m_rbNotSuccess";
            this.m_rbNotSuccess.TabStop = true;
            this.m_rbNotSuccess.UseVisualStyleBackColor = true;
            // 
            // BatchFileControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.m_editorGroupBox);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = null;
            this.MinimumSize = new System.Drawing.Size(620, 620);
            this.Name = "BatchFileControl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.m_editorGroupBox.ResumeLayout(false);
            this.m_editorGroupBox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
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
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_nameTextBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton m_rb1stFailure;
        private System.Windows.Forms.RadioButton m_rbAlways;
        private System.Windows.Forms.RadioButton m_rbFailure;
        private System.Windows.Forms.RadioButton m_rbDisabled;
        private System.Windows.Forms.RadioButton m_rbSucces;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton m_rbNot1stFailure;
        private System.Windows.Forms.RadioButton m_rbNotAlways;
        private System.Windows.Forms.RadioButton m_rbNotFailure;
        private System.Windows.Forms.RadioButton m_rbNotDisabled;
        private System.Windows.Forms.RadioButton m_rbNotSuccess;

    }
}
