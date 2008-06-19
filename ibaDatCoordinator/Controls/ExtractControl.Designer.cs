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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExtractControl));
            this.m_rbTextFile = new System.Windows.Forms.RadioButton();
            this.m_rbBinaryFile = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.m_browseFileButton = new System.Windows.Forms.Button();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_rbDbase = new System.Windows.Forms.RadioButton();
            this.m_rbFile = new System.Windows.Forms.RadioButton();
            this.m_panelFile = new System.Windows.Forms.Panel();
            this.m_tbPass = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_targetFolderTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.m_tbUserName = new System.Windows.Forms.TextBox();
            this.m_browseFolderButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.m_subfolderGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbOriginal = new System.Windows.Forms.RadioButton();
            this.m_rbNONE = new System.Windows.Forms.RadioButton();
            this.m_rbHour = new System.Windows.Forms.RadioButton();
            this.m_rbDay = new System.Windows.Forms.RadioButton();
            this.m_rbWeek = new System.Windows.Forms.RadioButton();
            this.m_rbMonth = new System.Windows.Forms.RadioButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.m_rbQuota = new System.Windows.Forms.RadioButton();
            this.m_nudQuota = new System.Windows.Forms.NumericUpDown();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_rbLimitDirectories = new System.Windows.Forms.RadioButton();
            this.m_nudDirs = new System.Windows.Forms.NumericUpDown();
            this.m_checkPathButton = new System.Windows.Forms.Button();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.m_monitorGroup = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            this.m_cbOverwrite = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.m_panelFile.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.m_subfolderGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).BeginInit();
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
            this.groupBox1.Controls.Add(this.m_executeIBAAButton);
            this.groupBox1.Controls.Add(this.m_browseFileButton);
            this.groupBox1.Controls.Add(this.m_pdoFileTextBox);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_executeIBAAButton
            // 
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.Analyzer_001;
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
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
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
            this.m_panelFile.Controls.Add(this.m_cbOverwrite);
            this.m_panelFile.Controls.Add(this.m_tbPass);
            this.m_panelFile.Controls.Add(this.label3);
            this.m_panelFile.Controls.Add(this.m_targetFolderTextBox);
            this.m_panelFile.Controls.Add(this.label12);
            this.m_panelFile.Controls.Add(this.groupBox5);
            this.m_panelFile.Controls.Add(this.m_tbUserName);
            this.m_panelFile.Controls.Add(this.m_browseFolderButton);
            this.m_panelFile.Controls.Add(this.label4);
            this.m_panelFile.Controls.Add(this.m_subfolderGroupBox);
            this.m_panelFile.Controls.Add(this.m_checkPathButton);
            this.m_panelFile.Name = "m_panelFile";
            // 
            // m_tbPass
            // 
            resources.ApplyResources(this.m_tbPass, "m_tbPass");
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.UseSystemPasswordChar = true;
            this.m_tbPass.TextChanged += new System.EventHandler(this.m_extractDirInfoChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // m_targetFolderTextBox
            // 
            resources.ApplyResources(this.m_targetFolderTextBox, "m_targetFolderTextBox");
            this.m_targetFolderTextBox.Name = "m_targetFolderTextBox";
            this.m_targetFolderTextBox.TextChanged += new System.EventHandler(this.m_extractDirInfoChanged);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.tableLayoutPanel4);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.m_rbTextFile, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_rbBinaryFile, 1, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // m_tbUserName
            // 
            resources.ApplyResources(this.m_tbUserName, "m_tbUserName");
            this.m_tbUserName.Name = "m_tbUserName";
            this.m_tbUserName.TextChanged += new System.EventHandler(this.m_extractDirInfoChanged);
            // 
            // m_browseFolderButton
            // 
            resources.ApplyResources(this.m_browseFolderButton, "m_browseFolderButton");
            this.m_browseFolderButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            this.m_browseFolderButton.Click += new System.EventHandler(this.m_browseFolderButton_Click);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // m_subfolderGroupBox
            // 
            resources.ApplyResources(this.m_subfolderGroupBox, "m_subfolderGroupBox");
            this.m_subfolderGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.m_subfolderGroupBox.Name = "m_subfolderGroupBox";
            this.m_subfolderGroupBox.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 1);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.m_rbOriginal, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_rbNONE, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_rbHour, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbDay, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbWeek, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbMonth, 0, 1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // m_rbOriginal
            // 
            resources.ApplyResources(this.m_rbOriginal, "m_rbOriginal");
            this.m_rbOriginal.Name = "m_rbOriginal";
            this.m_rbOriginal.TabStop = true;
            this.m_rbOriginal.UseVisualStyleBackColor = true;
            // 
            // m_rbNONE
            // 
            resources.ApplyResources(this.m_rbNONE, "m_rbNONE");
            this.m_rbNONE.Name = "m_rbNONE";
            this.m_rbNONE.TabStop = true;
            this.m_rbNONE.UseVisualStyleBackColor = true;
            // 
            // m_rbHour
            // 
            resources.ApplyResources(this.m_rbHour, "m_rbHour");
            this.m_rbHour.Name = "m_rbHour";
            this.m_rbHour.TabStop = true;
            this.m_rbHour.UseVisualStyleBackColor = true;
            // 
            // m_rbDay
            // 
            resources.ApplyResources(this.m_rbDay, "m_rbDay");
            this.m_rbDay.Name = "m_rbDay";
            this.m_rbDay.TabStop = true;
            this.m_rbDay.UseVisualStyleBackColor = true;
            // 
            // m_rbWeek
            // 
            resources.ApplyResources(this.m_rbWeek, "m_rbWeek");
            this.m_rbWeek.Name = "m_rbWeek";
            this.m_rbWeek.TabStop = true;
            this.m_rbWeek.UseVisualStyleBackColor = true;
            // 
            // m_rbMonth
            // 
            resources.ApplyResources(this.m_rbMonth, "m_rbMonth");
            this.m_rbMonth.Name = "m_rbMonth";
            this.m_rbMonth.TabStop = true;
            this.m_rbMonth.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.m_rbQuota);
            this.panel2.Controls.Add(this.m_nudQuota);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_rbQuota
            // 
            resources.ApplyResources(this.m_rbQuota, "m_rbQuota");
            this.m_rbQuota.Name = "m_rbQuota";
            this.m_rbQuota.TabStop = true;
            this.m_rbQuota.UseVisualStyleBackColor = true;
            this.m_rbQuota.CheckedChanged += new System.EventHandler(this.m_rbLimitUsageChoiceChanged);
            // 
            // m_nudQuota
            // 
            resources.ApplyResources(this.m_nudQuota, "m_nudQuota");
            this.m_nudQuota.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.m_nudQuota.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudQuota.Name = "m_nudQuota";
            this.m_nudQuota.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_rbLimitDirectories);
            this.panel1.Controls.Add(this.m_nudDirs);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // m_rbLimitDirectories
            // 
            resources.ApplyResources(this.m_rbLimitDirectories, "m_rbLimitDirectories");
            this.m_rbLimitDirectories.Name = "m_rbLimitDirectories";
            this.m_rbLimitDirectories.TabStop = true;
            this.m_rbLimitDirectories.UseVisualStyleBackColor = true;
            this.m_rbLimitDirectories.CheckedChanged += new System.EventHandler(this.m_rbLimitUsageChoiceChanged);
            // 
            // m_nudDirs
            // 
            resources.ApplyResources(this.m_nudDirs, "m_nudDirs");
            this.m_nudDirs.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_nudDirs.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudDirs.Name = "m_nudDirs";
            this.m_nudDirs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // m_checkPathButton
            // 
            resources.ApplyResources(this.m_checkPathButton, "m_checkPathButton");
            this.m_checkPathButton.Image = global::iba.Properties.Resources.thumup;
            this.m_checkPathButton.Name = "m_checkPathButton";
            this.m_checkPathButton.UseVisualStyleBackColor = true;
            this.m_checkPathButton.Click += new System.EventHandler(this.m_checkPathButton_Click);
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
            512,
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
            // m_cbOverwrite
            // 
            resources.ApplyResources(this.m_cbOverwrite, "m_cbOverwrite");
            this.m_cbOverwrite.Checked = true;
            this.m_cbOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_cbOverwrite.Name = "m_cbOverwrite";
            this.m_cbOverwrite.UseVisualStyleBackColor = true;
            // 
            // ExtractControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_monitorGroup);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.MinimumSize = new System.Drawing.Size(100, 470);
            this.Name = "ExtractControl";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.m_panelFile.ResumeLayout(false);
            this.m_panelFile.PerformLayout();
            this.groupBox5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.m_subfolderGroupBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).EndInit();
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Button m_browseFileButton;
        private System.Windows.Forms.TextBox m_pdoFileTextBox;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.TextBox m_targetFolderTextBox;
        private System.Windows.Forms.GroupBox m_subfolderGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton m_rbNONE;
        private System.Windows.Forms.RadioButton m_rbHour;
        private System.Windows.Forms.RadioButton m_rbMonth;
        private System.Windows.Forms.RadioButton m_rbDay;
        private System.Windows.Forms.RadioButton m_rbWeek;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton m_rbOriginal;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.RadioButton m_rbTextFile;
        private System.Windows.Forms.RadioButton m_rbBinaryFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton m_rbDbase;
        private System.Windows.Forms.RadioButton m_rbFile;
        private System.Windows.Forms.Panel m_panelFile;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button m_checkPathButton;
        private System.Windows.Forms.GroupBox m_monitorGroup;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton m_rbLimitDirectories;
        private System.Windows.Forms.NumericUpDown m_nudDirs;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton m_rbQuota;
        private System.Windows.Forms.NumericUpDown m_nudQuota;
        private System.Windows.Forms.CheckBox m_cbOverwrite;

    }
}
