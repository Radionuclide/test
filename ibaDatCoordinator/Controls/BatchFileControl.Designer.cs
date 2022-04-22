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
			this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.m_saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
			this.m_tooltip = new System.Windows.Forms.ToolTip();
			this.groupBox1 = new iba.Utility.CollapsibleGroupBox();
			this.m_newButton = new System.Windows.Forms.Button();
			this.m_textEditor = new ICSharpCode.TextEditor.TextEditorControl();
			this.m_batchFileTextBox = new System.Windows.Forms.TextBox();
			this.m_browseBATCHFileButton = new System.Windows.Forms.Button();
			this.m_saveButton = new System.Windows.Forms.Button();
			this.m_executeGroupBox = new iba.Utility.CollapsibleGroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.m_nudTime = new System.Windows.Forms.NumericUpDown();
			this.m_lblTestSide = new System.Windows.Forms.Label();
			this.m_panelSide = new System.Windows.Forms.Panel();
			this.m_rbServerSide = new System.Windows.Forms.RadioButton();
			this.m_rbClientSide = new System.Windows.Forms.RadioButton();
			this.m_rbPrevOutput = new System.Windows.Forms.RadioButton();
			this.m_rbDatFile = new System.Windows.Forms.RadioButton();
			this.label5 = new System.Windows.Forms.Label();
			this.m_browseDatFileButton = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.m_argumentsTextBox = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.m_datFileTextBox = new System.Windows.Forms.TextBox();
			this.m_executeBatchFile = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.m_executeGroupBox.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
			this.m_panelSide.SuspendLayout();
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
			this.m_newButton.Image = global::iba.Properties.Resources.NewDocument;
			this.m_newButton.Name = "m_newButton";
			this.m_newButton.UseVisualStyleBackColor = true;
			this.m_newButton.Click += new System.EventHandler(this.m_newButton_Click);
			// 
			// m_textEditor
			// 
			resources.ApplyResources(this.m_textEditor, "m_textEditor");
			this.m_textEditor.Name = "m_textEditor";
			this.m_textEditor.ShowEOLMarkers = true;
			this.m_textEditor.ShowSpaces = true;
			this.m_textEditor.ShowTabs = true;
			this.m_textEditor.ShowVRuler = true;
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
			this.m_browseBATCHFileButton.Image = Icons.Gui.All.Images.FolderOpen();
			this.m_browseBATCHFileButton.Name = "m_browseBATCHFileButton";
			this.m_browseBATCHFileButton.UseVisualStyleBackColor = true;
			this.m_browseBATCHFileButton.Click += new System.EventHandler(this.m_browseBATCHFileButton_Click);
			// 
			// m_saveButton
			// 
			resources.ApplyResources(this.m_saveButton, "m_saveButton");
			this.m_saveButton.Image = Icons.Gui.Standard.Images.Save();
			this.m_saveButton.Name = "m_saveButton";
			this.m_saveButton.UseVisualStyleBackColor = true;
			this.m_saveButton.Click += new System.EventHandler(this.m_saveButton_Click);
			// 
			// m_executeGroupBox
			// 
			resources.ApplyResources(this.m_executeGroupBox, "m_executeGroupBox");
			this.m_executeGroupBox.Controls.Add(this.label7);
			this.m_executeGroupBox.Controls.Add(this.label6);
			this.m_executeGroupBox.Controls.Add(this.m_nudTime);
			this.m_executeGroupBox.Controls.Add(this.m_lblTestSide);
			this.m_executeGroupBox.Controls.Add(this.m_panelSide);
			this.m_executeGroupBox.Controls.Add(this.m_rbPrevOutput);
			this.m_executeGroupBox.Controls.Add(this.m_rbDatFile);
			this.m_executeGroupBox.Controls.Add(this.label5);
			this.m_executeGroupBox.Controls.Add(this.m_browseDatFileButton);
			this.m_executeGroupBox.Controls.Add(this.label4);
			this.m_executeGroupBox.Controls.Add(this.label2);
			this.m_executeGroupBox.Controls.Add(this.label1);
			this.m_executeGroupBox.Controls.Add(this.m_argumentsTextBox);
			this.m_executeGroupBox.Controls.Add(this.label3);
			this.m_executeGroupBox.Controls.Add(this.m_datFileTextBox);
			this.m_executeGroupBox.Controls.Add(this.m_executeBatchFile);
			this.m_executeGroupBox.Name = "m_executeGroupBox";
			this.m_executeGroupBox.TabStop = false;
			// 
			// label7
			// 
			resources.ApplyResources(this.label7, "label7");
			this.label7.Name = "label7";
			// 
			// label6
			// 
			resources.ApplyResources(this.label6, "label6");
			this.label6.Name = "label6";
			// 
			// m_nudTime
			// 
			resources.ApplyResources(this.m_nudTime, "m_nudTime");
			this.m_nudTime.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
			this.m_nudTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.m_nudTime.Name = "m_nudTime";
			this.m_nudTime.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
			// 
			// m_lblTestSide
			// 
			resources.ApplyResources(this.m_lblTestSide, "m_lblTestSide");
			this.m_lblTestSide.Name = "m_lblTestSide";
			// 
			// m_panelSide
			// 
			this.m_panelSide.Controls.Add(this.m_rbServerSide);
			this.m_panelSide.Controls.Add(this.m_rbClientSide);
			resources.ApplyResources(this.m_panelSide, "m_panelSide");
			this.m_panelSide.Name = "m_panelSide";
			// 
			// m_rbServerSide
			// 
			resources.ApplyResources(this.m_rbServerSide, "m_rbServerSide");
			this.m_rbServerSide.Name = "m_rbServerSide";
			this.m_rbServerSide.TabStop = true;
			this.m_rbServerSide.UseVisualStyleBackColor = true;
			// 
			// m_rbClientSide
			// 
			resources.ApplyResources(this.m_rbClientSide, "m_rbClientSide");
			this.m_rbClientSide.Name = "m_rbClientSide";
			this.m_rbClientSide.TabStop = true;
			this.m_rbClientSide.UseVisualStyleBackColor = true;
			// 
			// m_rbPrevOutput
			// 
			resources.ApplyResources(this.m_rbPrevOutput, "m_rbPrevOutput");
			this.m_rbPrevOutput.Name = "m_rbPrevOutput";
			this.m_rbPrevOutput.TabStop = true;
			this.m_rbPrevOutput.UseVisualStyleBackColor = true;
			// 
			// m_rbDatFile
			// 
			resources.ApplyResources(this.m_rbDatFile, "m_rbDatFile");
			this.m_rbDatFile.Name = "m_rbDatFile";
			this.m_rbDatFile.TabStop = true;
			this.m_rbDatFile.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			// 
			// m_browseDatFileButton
			// 
			resources.ApplyResources(this.m_browseDatFileButton, "m_browseDatFileButton");
			this.m_browseDatFileButton.Image = Icons.Gui.All.Images.FolderOpen();
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
			// m_executeBatchFile
			// 
			resources.ApplyResources(this.m_executeBatchFile, "m_executeBatchFile");
			this.m_executeBatchFile.Image = Icons.Gui.All.Images.TerminalCode();
			this.m_executeBatchFile.Name = "m_executeBatchFile";
			this.m_executeBatchFile.UseVisualStyleBackColor = true;
			this.m_executeBatchFile.Click += new System.EventHandler(this.m_executeBatchFile_Click);
			// 
			// BatchFileControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.m_executeGroupBox);
			this.Name = "BatchFileControl";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.m_executeGroupBox.ResumeLayout(false);
			this.m_executeGroupBox.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
			this.m_panelSide.ResumeLayout(false);
			this.m_panelSide.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private iba.Utility.CollapsibleGroupBox groupBox1;
        private System.Windows.Forms.TextBox m_batchFileTextBox;
        private System.Windows.Forms.Button m_browseBATCHFileButton;
        private System.Windows.Forms.Button m_executeBatchFile;
        private iba.Utility.CollapsibleGroupBox m_executeGroupBox;
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
        private System.Windows.Forms.RadioButton m_rbPrevOutput;
        private System.Windows.Forms.RadioButton m_rbDatFile;
        private System.Windows.Forms.ToolTip m_tooltip;
        private System.Windows.Forms.Label m_lblTestSide;
        private System.Windows.Forms.Panel m_panelSide;
        private System.Windows.Forms.RadioButton m_rbServerSide;
        private System.Windows.Forms.RadioButton m_rbClientSide;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown m_nudTime;

    }
}
