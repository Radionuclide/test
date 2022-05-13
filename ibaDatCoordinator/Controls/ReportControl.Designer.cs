namespace iba.Controls
{
    partial class ReportControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportControl));
            this.groupBox2 = new iba.Utility.CollapsibleGroupBox();
            this.m_cbImageSubDirs = new System.Windows.Forms.CheckBox();
            this.m_panelFile = new System.Windows.Forms.Panel();
            this.m_extensionComboBox = new System.Windows.Forms.ComboBox();
            this.m_rbPrint = new System.Windows.Forms.RadioButton();
            this.m_rbFile = new System.Windows.Forms.RadioButton();
            this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browseFileButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.groupBox1 = new iba.Utility.CollapsibleGroupBox();
            this.m_btnUploadPDO = new System.Windows.Forms.Button();
            this.m_monitorGroup = new iba.Utility.CollapsibleGroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.m_cbImageSubDirs);
            this.groupBox2.Controls.Add(this.m_panelFile);
            this.groupBox2.Controls.Add(this.m_extensionComboBox);
            this.groupBox2.Controls.Add(this.m_rbPrint);
            this.groupBox2.Controls.Add(this.m_rbFile);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_cbImageSubDirs
            // 
            resources.ApplyResources(this.m_cbImageSubDirs, "m_cbImageSubDirs");
            this.m_cbImageSubDirs.Name = "m_cbImageSubDirs";
            this.m_cbImageSubDirs.UseVisualStyleBackColor = true;
            // 
            // m_panelFile
            // 
            resources.ApplyResources(this.m_panelFile, "m_panelFile");
            this.m_panelFile.Name = "m_panelFile";
            // 
            // m_extensionComboBox
            // 
            this.m_extensionComboBox.FormattingEnabled = true;
            resources.ApplyResources(this.m_extensionComboBox, "m_extensionComboBox");
            this.m_extensionComboBox.Name = "m_extensionComboBox";
            this.m_extensionComboBox.SelectedIndexChanged += new System.EventHandler(this.m_extensionComboBox_SelectedIndexChanged);
            // 
            // m_rbPrint
            // 
            resources.ApplyResources(this.m_rbPrint, "m_rbPrint");
            this.m_rbPrint.Name = "m_rbPrint";
            this.m_rbPrint.TabStop = true;
            this.m_rbPrint.UseVisualStyleBackColor = true;
            this.m_rbPrint.CheckedChanged += new System.EventHandler(this.m_rbPrint_CheckedChanged);
            // 
            // m_rbFile
            // 
            resources.ApplyResources(this.m_rbFile, "m_rbFile");
            this.m_rbFile.Name = "m_rbFile";
            this.m_rbFile.TabStop = true;
            this.m_rbFile.UseVisualStyleBackColor = true;
            // 
            // m_pdoFileTextBox
            // 
            resources.ApplyResources(this.m_pdoFileTextBox, "m_pdoFileTextBox");
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            // 
            // m_browseFileButton
            // 
            resources.ApplyResources(this.m_browseFileButton, "m_browseFileButton");
            this.m_browseFileButton.Image = Icons.Gui.All.Images.FolderOpen(16);
            this.m_browseFileButton.Name = "m_browseFileButton";
            this.m_browseFileButton.UseVisualStyleBackColor = true;
            this.m_browseFileButton.Click += new System.EventHandler(this.m_browseFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.Image = Icons.SystemTray.Images.IbaAnalyzer(16);
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
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
            // m_monitorGroup
            // 
            resources.ApplyResources(this.m_monitorGroup, "m_monitorGroup");
            this.m_monitorGroup.Controls.Add(this.label5);
            this.m_monitorGroup.Controls.Add(this.label3);
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
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
            // ReportControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.m_monitorGroup);
            this.Name = "ReportControl";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.OpenFileDialog m_openFileDialog1;
        private System.Windows.Forms.ComboBox m_extensionComboBox;
        private System.Windows.Forms.RadioButton m_rbPrint;
        private System.Windows.Forms.RadioButton m_rbFile;
        private System.Windows.Forms.TextBox m_pdoFileTextBox;
        private System.Windows.Forms.Button m_browseFileButton;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private iba.Utility.CollapsibleGroupBox groupBox1;
        private iba.Utility.CollapsibleGroupBox groupBox2;
        private System.Windows.Forms.Panel m_panelFile;
        private iba.Utility.CollapsibleGroupBox m_monitorGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
        private System.Windows.Forms.CheckBox m_cbImageSubDirs;
		private System.Windows.Forms.Button m_btnUploadPDO;
        private System.Windows.Forms.ToolTip m_toolTip;
    }
}
