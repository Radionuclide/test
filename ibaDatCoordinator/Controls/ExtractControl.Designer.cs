namespace iba.Controls
{
    partial class ExtractControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractControl));
            this.m_rbTextFile = new System.Windows.Forms.RadioButton();
            this.m_rbBinaryFile = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new iba.Utility.CollapsibleGroupBox();
            this.m_btnUploadPDO = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.m_browseFileButton = new System.Windows.Forms.Button();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new iba.Utility.CollapsibleGroupBox();
            this.m_rbDbase = new System.Windows.Forms.RadioButton();
            this.m_rbFile = new System.Windows.Forms.RadioButton();
            this.m_panelFile = new System.Windows.Forms.Panel();
            this.m_groupBoxFileType = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbParquet = new System.Windows.Forms.RadioButton();
            this.m_rbTDMS = new System.Windows.Forms.RadioButton();
            this.m_rbComtrade = new System.Windows.Forms.RadioButton();
            this.m_rbMatLab = new System.Windows.Forms.RadioButton();
            this.m_monitorGroup = new iba.Utility.CollapsibleGroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.m_panelFile.SuspendLayout();
            this.m_groupBoxFileType.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            this.SuspendLayout();
            // 
            // m_rbTextFile
            // 
            resources.ApplyResources(this.m_rbTextFile, "m_rbTextFile");
            this.m_rbTextFile.Name = "m_rbTextFile";
            this.m_rbTextFile.TabStop = true;
            this.m_rbTextFile.UseVisualStyleBackColor = true;
            // 
            // m_rbBinaryFile
            // 
            resources.ApplyResources(this.m_rbBinaryFile, "m_rbBinaryFile");
            this.m_rbBinaryFile.Name = "m_rbBinaryFile";
            this.m_rbBinaryFile.TabStop = true;
            this.m_rbBinaryFile.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.m_btnUploadPDO);
            this.groupBox1.Controls.Add(this.m_executeIBAAButton);
            this.groupBox1.Controls.Add(this.m_browseFileButton);
            this.groupBox1.Controls.Add(this.m_pdoFileTextBox);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_btnUploadPDO
            // 
            resources.ApplyResources(this.m_btnUploadPDO, "m_btnUploadPDO");
            this.m_btnUploadPDO.Image = global::iba.Properties.Resources.img_pdo_upload;
            this.m_btnUploadPDO.Name = "m_btnUploadPDO";
            this.m_btnUploadPDO.UseVisualStyleBackColor = true;
            this.m_btnUploadPDO.Click += new System.EventHandler(this.m_btnUploadPDO_Click);
            // 
            // m_executeIBAAButton
            // 
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.ibaAnalyzer_16x16;
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // m_browseFileButton
            // 
            resources.ApplyResources(this.m_browseFileButton, "m_browseFileButton");
            this.m_browseFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFileButton.Name = "m_browseFileButton";
            this.m_browseFileButton.UseVisualStyleBackColor = true;
            this.m_browseFileButton.Click += new System.EventHandler(this.m_browseFileButton_Click);
            // 
            // m_pdoFileTextBox
            // 
            resources.ApplyResources(this.m_pdoFileTextBox, "m_pdoFileTextBox");
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.m_rbDbase);
            this.groupBox3.Controls.Add(this.m_rbFile);
            this.groupBox3.Controls.Add(this.m_panelFile);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // m_rbDbase
            // 
            resources.ApplyResources(this.m_rbDbase, "m_rbDbase");
            this.m_rbDbase.Name = "m_rbDbase";
            this.m_rbDbase.TabStop = true;
            this.m_rbDbase.UseVisualStyleBackColor = true;
            this.m_rbDbase.CheckedChanged += new System.EventHandler(this.m_rbDbase_CheckedChanged);
            // 
            // m_rbFile
            // 
            resources.ApplyResources(this.m_rbFile, "m_rbFile");
            this.m_rbFile.Name = "m_rbFile";
            this.m_rbFile.TabStop = true;
            this.m_rbFile.UseVisualStyleBackColor = true;
            this.m_rbFile.CheckedChanged += new System.EventHandler(this.m_rbDbase_CheckedChanged);
            // 
            // m_panelFile
            // 
            resources.ApplyResources(this.m_panelFile, "m_panelFile");
            this.m_panelFile.Controls.Add(this.m_groupBoxFileType);
            this.m_panelFile.Name = "m_panelFile";
            // 
            // m_groupBoxFileType
            // 
            resources.ApplyResources(this.m_groupBoxFileType, "m_groupBoxFileType");
            this.m_groupBoxFileType.Controls.Add(this.tableLayoutPanel4);
            this.m_groupBoxFileType.Name = "m_groupBoxFileType";
            this.m_groupBoxFileType.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.m_rbParquet, 4, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_rbTDMS, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_rbTextFile, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_rbComtrade, 2, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_rbBinaryFile, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_rbMatLab, 5, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // m_rbParquet
            // 
            resources.ApplyResources(this.m_rbParquet, "m_rbParquet");
            this.m_rbParquet.Name = "m_rbParquet";
            this.m_rbParquet.TabStop = true;
            this.m_rbParquet.UseVisualStyleBackColor = true;
            // 
            // m_rbTDMS
            // 
            resources.ApplyResources(this.m_rbTDMS, "m_rbTDMS");
            this.m_rbTDMS.Name = "m_rbTDMS";
            this.m_rbTDMS.TabStop = true;
            this.m_rbTDMS.UseVisualStyleBackColor = true;
            // 
            // m_rbComtrade
            // 
            resources.ApplyResources(this.m_rbComtrade, "m_rbComtrade");
            this.m_rbComtrade.Name = "m_rbComtrade";
            this.m_rbComtrade.TabStop = true;
            this.m_rbComtrade.UseVisualStyleBackColor = true;
            // 
            // m_rbMatLab
            // 
            resources.ApplyResources(this.m_rbMatLab, "m_rbMatLab");
            this.m_rbMatLab.Name = "m_rbMatLab";
            this.m_rbMatLab.TabStop = true;
            this.m_rbMatLab.UseVisualStyleBackColor = true;
            // 
            // m_monitorGroup
            // 
            resources.ApplyResources(this.m_monitorGroup, "m_monitorGroup");
            this.m_monitorGroup.Controls.Add(this.label5);
            this.m_monitorGroup.Controls.Add(this.label1);
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // ExtractControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.m_monitorGroup);
            this.Name = "ExtractControl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.m_panelFile.ResumeLayout(false);
            this.m_groupBoxFileType.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private iba.Utility.CollapsibleGroupBox groupBox1;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Button m_browseFileButton;
        private System.Windows.Forms.TextBox m_pdoFileTextBox;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog1;
        private iba.Utility.CollapsibleGroupBox groupBox3;
        private System.Windows.Forms.GroupBox m_groupBoxFileType;
        private System.Windows.Forms.RadioButton m_rbTextFile;
        private System.Windows.Forms.RadioButton m_rbBinaryFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton m_rbDbase;
        private System.Windows.Forms.RadioButton m_rbFile;
        private System.Windows.Forms.Panel m_panelFile;
        private iba.Utility.CollapsibleGroupBox m_monitorGroup;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton m_rbComtrade;
        private System.Windows.Forms.RadioButton m_rbTDMS;
        private System.Windows.Forms.RadioButton m_rbParquet;
		private System.Windows.Forms.RadioButton m_rbMatLab;
		private System.Windows.Forms.Button m_btnUploadPDO;
        private System.Windows.Forms.ToolTip m_toolTip;
    }
}
