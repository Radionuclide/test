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
            this.m_newButton = new System.Windows.Forms.Button();
            this.m_textEditor = new ICSharpCode.TextEditor.TextEditorControl();
            this.m_batchFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browseBATCHFileButton = new System.Windows.Forms.Button();
            this.m_saveButton = new System.Windows.Forms.Button();
            this.m_executeBatchFile = new System.Windows.Forms.Button();
            this.m_editorGroupBox = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_browseDatFileButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_argumentsTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_datFileTextBox = new System.Windows.Forms.TextBox();
            this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.m_saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.m_rbDatFile = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.m_editorGroupBox.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.m_newButton);
            this.groupBox1.Controls.Add(this.m_textEditor);
            this.groupBox1.Controls.Add(this.m_batchFileTextBox);
            this.groupBox1.Controls.Add(this.m_browseBATCHFileButton);
            this.groupBox1.Controls.Add(this.m_saveButton);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_newButton
            // 
            resources.ApplyResources(this.m_newButton, "m_newButton");
            this.m_newButton.Image = global::iba.Properties.Resources.Aktualisieren;
            this.m_newButton.Name = "m_newButton";
            this.m_newButton.UseVisualStyleBackColor = true;
            // 
            // m_textEditor
            // 
            resources.ApplyResources(this.m_textEditor, "m_textEditor");
            this.m_textEditor.Name = "m_textEditor";
            this.m_textEditor.ShowEOLMarkers = true;
            this.m_textEditor.ShowSpaces = true;
            this.m_textEditor.ShowTabs = true;
            this.m_textEditor.ShowVRuler = true;
            this.m_textEditor.Changed += new System.EventHandler(this.m_textEditor_Changed);
            // 
            // m_batchFileTextBox
            // 
            resources.ApplyResources(this.m_batchFileTextBox, "m_batchFileTextBox");
            this.m_batchFileTextBox.Name = "m_batchFileTextBox";
            this.m_batchFileTextBox.ReadOnly = true;
            // 
            // m_browseBATCHFileButton
            // 
            resources.ApplyResources(this.m_browseBATCHFileButton, "m_browseBATCHFileButton");
            this.m_browseBATCHFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseBATCHFileButton.Name = "m_browseBATCHFileButton";
            this.m_browseBATCHFileButton.UseVisualStyleBackColor = true;
            this.m_browseBATCHFileButton.Click += new System.EventHandler(this.m_browseBATCHFileButton_Click);
            // 
            // m_saveButton
            // 
            resources.ApplyResources(this.m_saveButton, "m_saveButton");
            this.m_saveButton.Image = global::iba.Properties.Resources.Speichern;
            this.m_saveButton.Name = "m_saveButton";
            this.m_saveButton.UseVisualStyleBackColor = true;
            this.m_saveButton.Click += new System.EventHandler(this.m_saveButton_Click);
            // 
            // m_executeBatchFile
            // 
            resources.ApplyResources(this.m_executeBatchFile, "m_executeBatchFile");
            this.m_executeBatchFile.Image = global::iba.Properties.Resources.DOS;
            this.m_executeBatchFile.Name = "m_executeBatchFile";
            this.m_executeBatchFile.UseVisualStyleBackColor = true;
            this.m_executeBatchFile.Click += new System.EventHandler(this.m_executeBatchFile_Click);
            // 
            // m_editorGroupBox
            // 
            resources.ApplyResources(this.m_editorGroupBox, "m_editorGroupBox");
            this.m_editorGroupBox.Controls.Add(this.radioButton1);
            this.m_editorGroupBox.Controls.Add(this.m_rbDatFile);
            this.m_editorGroupBox.Controls.Add(this.label5);
            this.m_editorGroupBox.Controls.Add(this.m_browseDatFileButton);
            this.m_editorGroupBox.Controls.Add(this.label4);
            this.m_editorGroupBox.Controls.Add(this.label2);
            this.m_editorGroupBox.Controls.Add(this.label1);
            this.m_editorGroupBox.Controls.Add(this.m_argumentsTextBox);
            this.m_editorGroupBox.Controls.Add(this.label3);
            this.m_editorGroupBox.Controls.Add(this.m_datFileTextBox);
            this.m_editorGroupBox.Controls.Add(this.m_executeBatchFile);
            this.m_editorGroupBox.Name = "m_editorGroupBox";
            this.m_editorGroupBox.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // m_browseDatFileButton
            // 
            resources.ApplyResources(this.m_browseDatFileButton, "m_browseDatFileButton");
            this.m_browseDatFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_argumentsTextBox
            // 
            resources.ApplyResources(this.m_argumentsTextBox, "m_argumentsTextBox");
            this.m_argumentsTextBox.Name = "m_argumentsTextBox";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // m_datFileTextBox
            // 
            resources.ApplyResources(this.m_datFileTextBox, "m_datFileTextBox");
            this.m_datFileTextBox.Name = "m_datFileTextBox";
            // 
            // m_rbDatFile
            // 
            resources.ApplyResources(this.m_rbDatFile, "m_rbDatFile");
            this.m_rbDatFile.Name = "m_rbDatFile";
            this.m_rbDatFile.TabStop = true;
            this.m_rbDatFile.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // BatchFileControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_editorGroupBox);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(566, 370);
            this.Name = "BatchFileControl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.m_editorGroupBox.ResumeLayout(false);
            this.m_editorGroupBox.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox m_batchFileTextBox;
        private System.Windows.Forms.Button m_browseBATCHFileButton;
        private System.Windows.Forms.Button m_executeBatchFile;
        private System.Windows.Forms.GroupBox m_editorGroupBox;
        private System.Windows.Forms.Button m_newButton;
        private System.Windows.Forms.Button m_saveButton;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog1;
        private ICSharpCode.TextEditor.TextEditorControl m_textEditor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_datFileTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_argumentsTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button m_browseDatFileButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.SaveFileDialog m_saveFileDialog1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton m_rbDatFile;

    }
}
