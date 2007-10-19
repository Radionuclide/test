namespace iba.Controls
{
    partial class CopyControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyControl));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_tbPass = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.m_tbUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_checkPathButton = new System.Windows.Forms.Button();
            this.m_subfolderGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbOriginal = new System.Windows.Forms.RadioButton();
            this.m_rbNONE = new System.Windows.Forms.RadioButton();
            this.m_rbHour = new System.Windows.Forms.RadioButton();
            this.m_rbDay = new System.Windows.Forms.RadioButton();
            this.m_rbWeek = new System.Windows.Forms.RadioButton();
            this.m_rbMonth = new System.Windows.Forms.RadioButton();
            this.m_browseFolderButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_targetFolderTextBox = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.m_rbPrevOutput = new System.Windows.Forms.RadioButton();
            this.m_rbDatFile = new System.Windows.Forms.RadioButton();
            this.m_cbRemoveSource = new System.Windows.Forms.CheckBox();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.m_rbQuota = new System.Windows.Forms.RadioButton();
            this.m_nudQuota = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.m_rbLimitDirectories = new System.Windows.Forms.RadioButton();
            this.m_nudDirs = new System.Windows.Forms.NumericUpDown();
            this.groupBox2.SuspendLayout();
            this.m_subfolderGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.m_tbPass);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.m_tbUserName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.m_checkPathButton);
            this.groupBox2.Controls.Add(this.m_subfolderGroupBox);
            this.groupBox2.Controls.Add(this.m_browseFolderButton);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.m_targetFolderTextBox);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_tbPass
            // 
            resources.ApplyResources(this.m_tbPass, "m_tbPass");
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.UseSystemPasswordChar = true;
            this.m_tbPass.TextChanged += new System.EventHandler(this.m_targetDirInfoChanged);
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // m_tbUserName
            // 
            resources.ApplyResources(this.m_tbUserName, "m_tbUserName");
            this.m_tbUserName.Name = "m_tbUserName";
            this.m_tbUserName.TextChanged += new System.EventHandler(this.m_targetDirInfoChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // m_checkPathButton
            // 
            resources.ApplyResources(this.m_checkPathButton, "m_checkPathButton");
            this.m_checkPathButton.Image = global::iba.Properties.Resources.thumup;
            this.m_checkPathButton.Name = "m_checkPathButton";
            this.m_checkPathButton.UseVisualStyleBackColor = true;
            this.m_checkPathButton.Click += new System.EventHandler(this.m_checkPathButton_Click);
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
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
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
            // m_browseFolderButton
            // 
            resources.ApplyResources(this.m_browseFolderButton, "m_browseFolderButton");
            this.m_browseFolderButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            this.m_browseFolderButton.Click += new System.EventHandler(this.m_browseFolderButton_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_targetFolderTextBox
            // 
            resources.ApplyResources(this.m_targetFolderTextBox, "m_targetFolderTextBox");
            this.m_targetFolderTextBox.Name = "m_targetFolderTextBox";
            this.m_targetFolderTextBox.TextChanged += new System.EventHandler(this.m_targetDirInfoChanged);
            // 
            // groupBox6
            // 
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Controls.Add(this.m_rbPrevOutput);
            this.groupBox6.Controls.Add(this.m_rbDatFile);
            this.groupBox6.Controls.Add(this.m_cbRemoveSource);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
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
            // m_cbRemoveSource
            // 
            resources.ApplyResources(this.m_cbRemoveSource, "m_cbRemoveSource");
            this.m_cbRemoveSource.Name = "m_cbRemoveSource";
            this.m_cbRemoveSource.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
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
            // panel3
            // 
            this.panel3.Controls.Add(this.m_rbLimitDirectories);
            this.panel3.Controls.Add(this.m_nudDirs);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
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
            // CopyControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.MinimumSize = new System.Drawing.Size(566, 315);
            this.Name = "CopyControl";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.m_subfolderGroupBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox m_subfolderGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton m_rbNONE;
        private System.Windows.Forms.RadioButton m_rbHour;
        private System.Windows.Forms.RadioButton m_rbMonth;
        private System.Windows.Forms.RadioButton m_rbDay;
        private System.Windows.Forms.RadioButton m_rbWeek;
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_targetFolderTextBox;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox m_cbRemoveSource;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
        private System.Windows.Forms.RadioButton m_rbOriginal;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button m_checkPathButton;
        private System.Windows.Forms.RadioButton m_rbPrevOutput;
        private System.Windows.Forms.RadioButton m_rbDatFile;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton m_rbQuota;
        private System.Windows.Forms.NumericUpDown m_nudQuota;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton m_rbLimitDirectories;
        private System.Windows.Forms.NumericUpDown m_nudDirs;
    }
}
