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
            this.m_gbTarget = new System.Windows.Forms.GroupBox();
            this.m_tbPass = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.m_tbUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_checkPathButton = new System.Windows.Forms.Button();
            this.m_subfolderGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.m_rbQuota = new System.Windows.Forms.RadioButton();
            this.m_nudQuota = new System.Windows.Forms.NumericUpDown();
            this.panel3 = new System.Windows.Forms.Panel();
            this.m_rbLimitDirectories = new System.Windows.Forms.RadioButton();
            this.m_nudDirs = new System.Windows.Forms.NumericUpDown();
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
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbPrevOutput = new System.Windows.Forms.RadioButton();
            this.m_rbDatFile = new System.Windows.Forms.RadioButton();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbMove = new System.Windows.Forms.RadioButton();
            this.m_rbDelete = new System.Windows.Forms.RadioButton();
            this.m_rbCopy = new System.Windows.Forms.RadioButton();
            this.m_cbOverwrite = new System.Windows.Forms.CheckBox();
            this.m_gbTarget.SuspendLayout();
            this.m_subfolderGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_gbTarget
            // 
            resources.ApplyResources(this.m_gbTarget, "m_gbTarget");
            this.m_gbTarget.Controls.Add(this.m_cbOverwrite);
            this.m_gbTarget.Controls.Add(this.m_tbPass);
            this.m_gbTarget.Controls.Add(this.label12);
            this.m_gbTarget.Controls.Add(this.m_tbUserName);
            this.m_gbTarget.Controls.Add(this.label4);
            this.m_gbTarget.Controls.Add(this.m_checkPathButton);
            this.m_gbTarget.Controls.Add(this.m_subfolderGroupBox);
            this.m_gbTarget.Controls.Add(this.m_browseFolderButton);
            this.m_gbTarget.Controls.Add(this.label1);
            this.m_gbTarget.Controls.Add(this.m_targetFolderTextBox);
            this.m_gbTarget.Name = "m_gbTarget";
            this.m_gbTarget.TabStop = false;
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
            this.groupBox6.Controls.Add(this.tableLayoutPanel4);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.m_rbPrevOutput, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_rbDatFile, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
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
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.radioButton1, 1, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.TabStop = true;
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.tableLayoutPanel6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.Controls.Add(this.m_rbMove, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbDelete, 1, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbCopy, 0, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // m_rbMove
            // 
            resources.ApplyResources(this.m_rbMove, "m_rbMove");
            this.m_rbMove.Name = "m_rbMove";
            this.m_rbMove.TabStop = true;
            this.m_rbMove.UseVisualStyleBackColor = true;
            // 
            // m_rbDelete
            // 
            resources.ApplyResources(this.m_rbDelete, "m_rbDelete");
            this.m_rbDelete.Name = "m_rbDelete";
            this.m_rbDelete.TabStop = true;
            this.m_rbDelete.UseVisualStyleBackColor = true;
            this.m_rbDelete.CheckedChanged += new System.EventHandler(this.m_rbDelete_CheckedChanged);
            // 
            // m_rbCopy
            // 
            resources.ApplyResources(this.m_rbCopy, "m_rbCopy");
            this.m_rbCopy.Name = "m_rbCopy";
            this.m_rbCopy.TabStop = true;
            this.m_rbCopy.UseVisualStyleBackColor = true;
            // 
            // m_cbOverwrite
            // 
            resources.ApplyResources(this.m_cbOverwrite, "m_cbOverwrite");
            this.m_cbOverwrite.Checked = true;
            this.m_cbOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_cbOverwrite.Name = "m_cbOverwrite";
            this.m_cbOverwrite.UseVisualStyleBackColor = true;
            // 
            // CopyControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.m_gbTarget);
            this.MinimumSize = new System.Drawing.Size(566, 315);
            this.Name = "CopyControl";
            this.m_gbTarget.ResumeLayout(false);
            this.m_gbTarget.PerformLayout();
            this.m_subfolderGroupBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).EndInit();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_gbTarget;
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
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton m_rbMove;
        private System.Windows.Forms.RadioButton m_rbDelete;
        private System.Windows.Forms.RadioButton m_rbCopy;
        private System.Windows.Forms.CheckBox m_cbOverwrite;
    }
}
