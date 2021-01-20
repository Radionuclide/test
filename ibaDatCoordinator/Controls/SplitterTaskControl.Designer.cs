namespace iba.Controls
{
    partial class SplitterTaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SplitterTaskControl));
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.groupBox2 = new iba.Utility.CollapsibleGroupBox();
            this.m_btnUploadPDO = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.m_splitTypeCBox = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browsePDOFileButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.m_gbTarget = new iba.Utility.CollapsibleGroupBox();
            this.panelOut = new System.Windows.Forms.Panel();
            this.groupBox1 = new iba.Utility.CollapsibleGroupBox();
            this.m_testButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_datFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browseDatFileButton = new System.Windows.Forms.Button();
            this.m_monitorGroup = new iba.Utility.CollapsibleGroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            this.channelTreeEditPlaceholder = new System.Windows.Forms.TextBox();
            this.groupBox2.SuspendLayout();
            this.m_gbTarget.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.channelTreeEditPlaceholder);
            this.groupBox2.Controls.Add(this.m_btnUploadPDO);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.m_splitTypeCBox);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.m_pdoFileTextBox);
            this.groupBox2.Controls.Add(this.m_browsePDOFileButton);
            this.groupBox2.Controls.Add(this.m_executeIBAAButton);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_btnUploadPDO
            // 
            resources.ApplyResources(this.m_btnUploadPDO, "m_btnUploadPDO");
            this.m_btnUploadPDO.Image = global::iba.Properties.Resources.img_pdo_upload;
            this.m_btnUploadPDO.Name = "m_btnUploadPDO";
            this.m_btnUploadPDO.UseVisualStyleBackColor = true;
            this.m_btnUploadPDO.Click += new System.EventHandler(this.m_btnUploadPDO_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // m_splitTypeCBox
            // 
            resources.ApplyResources(this.m_splitTypeCBox, "m_splitTypeCBox");
            this.m_splitTypeCBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_splitTypeCBox.FormattingEnabled = true;
            this.m_splitTypeCBox.Items.AddRange(new object[] {
            resources.GetString("m_splitTypeCBox.Items"),
            resources.GetString("m_splitTypeCBox.Items1")});
            this.m_splitTypeCBox.Name = "m_splitTypeCBox";
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
            // m_pdoFileTextBox
            // 
            resources.ApplyResources(this.m_pdoFileTextBox, "m_pdoFileTextBox");
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
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
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.ibaAnalyzer_16x16;
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // m_gbTarget
            // 
            resources.ApplyResources(this.m_gbTarget, "m_gbTarget");
            this.m_gbTarget.Controls.Add(this.panelOut);
            this.m_gbTarget.Name = "m_gbTarget";
            this.m_gbTarget.TabStop = false;
            // 
            // panelOut
            // 
            resources.ApplyResources(this.panelOut, "panelOut");
            this.panelOut.Name = "panelOut";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.m_testButton);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.m_datFileTextBox);
            this.groupBox1.Controls.Add(this.m_browseDatFileButton);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_testButton
            // 
            resources.ApplyResources(this.m_testButton, "m_testButton");
            this.m_testButton.Name = "m_testButton";
            this.m_testButton.UseVisualStyleBackColor = true;
            this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
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
            this.m_datFileTextBox.TextChanged += new System.EventHandler(this.m_datFileTextBox_TextChanged);
            // 
            // m_browseDatFileButton
            // 
            resources.ApplyResources(this.m_browseDatFileButton, "m_browseDatFileButton");
            this.m_browseDatFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
            // 
            // m_monitorGroup
            // 
            resources.ApplyResources(this.m_monitorGroup, "m_monitorGroup");
            this.m_monitorGroup.Controls.Add(this.label5);
            this.m_monitorGroup.Controls.Add(this.label6);
            this.m_monitorGroup.Controls.Add(this.m_nudTime);
            this.m_monitorGroup.Controls.Add(this.m_nudMemory);
            this.m_monitorGroup.Controls.Add(this.m_cbTime);
            this.m_monitorGroup.Controls.Add(this.m_cbMemory);
            this.m_monitorGroup.Name = "m_monitorGroup";
            this.m_monitorGroup.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
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
            5,
            0,
            0,
            0});
            // 
            // m_nudMemory
            // 
            resources.ApplyResources(this.m_nudMemory, "m_nudMemory");
            this.m_nudMemory.Maximum = new decimal(new int[] {
            4096,
            0,
            0,
            0});
            this.m_nudMemory.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudMemory.Name = "m_nudMemory";
            this.m_nudMemory.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // m_cbTime
            // 
            resources.ApplyResources(this.m_cbTime, "m_cbTime");
            this.m_cbTime.Name = "m_cbTime";
            this.m_cbTime.UseVisualStyleBackColor = true;
            // 
            // m_cbMemory
            // 
            resources.ApplyResources(this.m_cbMemory, "m_cbMemory");
            this.m_cbMemory.Name = "m_cbMemory";
            this.m_cbMemory.UseVisualStyleBackColor = true;
            // 
            // channelTreeEditPlaceholder
            // 
            resources.ApplyResources(this.channelTreeEditPlaceholder, "channelTreeEditPlaceholder");
            this.channelTreeEditPlaceholder.Name = "channelTreeEditPlaceholder";
            // 
            // SplitterTaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_gbTarget);
            this.Controls.Add(this.m_monitorGroup);
            this.Name = "SplitterTaskControl";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.m_gbTarget.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private iba.Utility.CollapsibleGroupBox groupBox2;
        private System.Windows.Forms.TextBox m_pdoFileTextBox;
        private System.Windows.Forms.Button m_browsePDOFileButton;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private iba.Utility.CollapsibleGroupBox groupBox1;
        private System.Windows.Forms.Button m_testButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_datFileTextBox;
        private System.Windows.Forms.Button m_browseDatFileButton;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox m_splitTypeCBox;
        private iba.Utility.CollapsibleGroupBox m_monitorGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
        private Utility.CollapsibleGroupBox m_gbTarget;
        private System.Windows.Forms.Panel panelOut;
		private System.Windows.Forms.Button m_btnUploadPDO;
        private System.Windows.Forms.TextBox channelTreeEditPlaceholder;
    }
}
