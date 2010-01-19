namespace iba.Controls
{
    partial class UpdateDataTaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UpdateDataTaskControl));
            this.m_folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.m_gbTarget = new System.Windows.Forms.GroupBox();
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
            this.panel3 = new System.Windows.Forms.Panel();
            this.m_cbTakeDatTime = new System.Windows.Forms.CheckBox();
            this.m_btBrowseTarget = new System.Windows.Forms.Button();
            this.m_cbOverwrite = new System.Windows.Forms.CheckBox();
            this.m_tbPass = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.m_tbUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_checkPathButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_targetFolderTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_panelAuth = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.m_tbDbPass = new System.Windows.Forms.TextBox();
            this.m_rbNT = new System.Windows.Forms.RadioButton();
            this.label6 = new System.Windows.Forms.Label();
            this.m_rbOtherAuth = new System.Windows.Forms.RadioButton();
            this.m_tbDbUsername = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.m_btTestConnection = new System.Windows.Forms.Button();
            this.m_tbTableName = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.m_computer = new System.Windows.Forms.Panel();
            this.m_btBrowseServer = new System.Windows.Forms.Button();
            this.m_tbServer = new System.Windows.Forms.TextBox();
            this.m_rbServer = new System.Windows.Forms.RadioButton();
            this.m_rbLocal = new System.Windows.Forms.RadioButton();
            this.label10 = new System.Windows.Forms.Label();
            this.m_tbDatabaseName = new System.Windows.Forms.TextBox();
            this.m_cbProvider = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_gbTarget.SuspendLayout();
            this.m_subfolderGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).BeginInit();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.m_panelAuth.SuspendLayout();
            this.m_computer.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_gbTarget
            // 
            resources.ApplyResources(this.m_gbTarget, "m_gbTarget");
            this.m_gbTarget.Controls.Add(this.m_subfolderGroupBox);
            this.m_gbTarget.Controls.Add(this.m_btBrowseTarget);
            this.m_gbTarget.Controls.Add(this.m_cbOverwrite);
            this.m_gbTarget.Controls.Add(this.m_tbPass);
            this.m_gbTarget.Controls.Add(this.label12);
            this.m_gbTarget.Controls.Add(this.m_tbUserName);
            this.m_gbTarget.Controls.Add(this.label4);
            this.m_gbTarget.Controls.Add(this.m_checkPathButton);
            this.m_gbTarget.Controls.Add(this.label1);
            this.m_gbTarget.Controls.Add(this.m_targetFolderTextBox);
            this.m_gbTarget.Name = "m_gbTarget";
            this.m_gbTarget.TabStop = false;
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
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel1, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 1);
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
            // panel3
            // 
            this.panel3.Controls.Add(this.m_cbTakeDatTime);
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // m_cbTakeDatTime
            // 
            resources.ApplyResources(this.m_cbTakeDatTime, "m_cbTakeDatTime");
            this.m_cbTakeDatTime.Checked = true;
            this.m_cbTakeDatTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_cbTakeDatTime.Name = "m_cbTakeDatTime";
            this.m_cbTakeDatTime.UseVisualStyleBackColor = true;
            // 
            // m_btBrowseTarget
            // 
            resources.ApplyResources(this.m_btBrowseTarget, "m_btBrowseTarget");
            this.m_btBrowseTarget.Image = global::iba.Properties.Resources.open;
            this.m_btBrowseTarget.Name = "m_btBrowseTarget";
            this.m_btBrowseTarget.UseVisualStyleBackColor = true;
            this.m_btBrowseTarget.Click += new System.EventHandler(this.m_btBrowseTarget_Click);
            // 
            // m_cbOverwrite
            // 
            resources.ApplyResources(this.m_cbOverwrite, "m_cbOverwrite");
            this.m_cbOverwrite.Checked = true;
            this.m_cbOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_cbOverwrite.Name = "m_cbOverwrite";
            this.m_cbOverwrite.UseVisualStyleBackColor = true;
            // 
            // m_tbPass
            // 
            resources.ApplyResources(this.m_tbPass, "m_tbPass");
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.UseSystemPasswordChar = true;
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
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_targetFolderTextBox
            // 
            resources.ApplyResources(this.m_targetFolderTextBox, "m_targetFolderTextBox");
            this.m_targetFolderTextBox.Name = "m_targetFolderTextBox";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.m_panelAuth);
            this.groupBox1.Controls.Add(this.m_btTestConnection);
            this.groupBox1.Controls.Add(this.m_tbTableName);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.m_computer);
            this.groupBox1.Controls.Add(this.m_tbDatabaseName);
            this.groupBox1.Controls.Add(this.m_cbProvider);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_panelAuth
            // 
            resources.ApplyResources(this.m_panelAuth, "m_panelAuth");
            this.m_panelAuth.Controls.Add(this.label8);
            this.m_panelAuth.Controls.Add(this.m_tbDbPass);
            this.m_panelAuth.Controls.Add(this.m_rbNT);
            this.m_panelAuth.Controls.Add(this.label6);
            this.m_panelAuth.Controls.Add(this.m_rbOtherAuth);
            this.m_panelAuth.Controls.Add(this.m_tbDbUsername);
            this.m_panelAuth.Controls.Add(this.label7);
            this.m_panelAuth.Name = "m_panelAuth";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // m_tbDbPass
            // 
            resources.ApplyResources(this.m_tbDbPass, "m_tbDbPass");
            this.m_tbDbPass.Name = "m_tbDbPass";
            this.m_tbDbPass.UseSystemPasswordChar = true;
            this.m_tbDbPass.TextChanged += new System.EventHandler(this.m_tbDbUsername_TextChanged);
            // 
            // m_rbNT
            // 
            resources.ApplyResources(this.m_rbNT, "m_rbNT");
            this.m_rbNT.Name = "m_rbNT";
            this.m_rbNT.TabStop = true;
            this.m_rbNT.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // m_rbOtherAuth
            // 
            resources.ApplyResources(this.m_rbOtherAuth, "m_rbOtherAuth");
            this.m_rbOtherAuth.Name = "m_rbOtherAuth";
            this.m_rbOtherAuth.TabStop = true;
            this.m_rbOtherAuth.UseVisualStyleBackColor = true;
            // 
            // m_tbDbUsername
            // 
            resources.ApplyResources(this.m_tbDbUsername, "m_tbDbUsername");
            this.m_tbDbUsername.Name = "m_tbDbUsername";
            this.m_tbDbUsername.TextChanged += new System.EventHandler(this.m_tbDbUsername_TextChanged);
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // m_btTestConnection
            // 
            resources.ApplyResources(this.m_btTestConnection, "m_btTestConnection");
            this.m_btTestConnection.Name = "m_btTestConnection";
            this.m_btTestConnection.UseVisualStyleBackColor = true;
            this.m_btTestConnection.Click += new System.EventHandler(this.m_btTestConnection_Click);
            // 
            // m_tbTableName
            // 
            resources.ApplyResources(this.m_tbTableName, "m_tbTableName");
            this.m_tbTableName.Name = "m_tbTableName";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // m_computer
            // 
            resources.ApplyResources(this.m_computer, "m_computer");
            this.m_computer.Controls.Add(this.m_btBrowseServer);
            this.m_computer.Controls.Add(this.m_tbServer);
            this.m_computer.Controls.Add(this.m_rbServer);
            this.m_computer.Controls.Add(this.m_rbLocal);
            this.m_computer.Controls.Add(this.label10);
            this.m_computer.Name = "m_computer";
            // 
            // m_btBrowseServer
            // 
            resources.ApplyResources(this.m_btBrowseServer, "m_btBrowseServer");
            this.m_btBrowseServer.Image = global::iba.Properties.Resources.open;
            this.m_btBrowseServer.Name = "m_btBrowseServer";
            this.m_btBrowseServer.UseVisualStyleBackColor = true;
            this.m_btBrowseServer.Click += new System.EventHandler(this.m_btBrowseServer_Click);
            // 
            // m_tbServer
            // 
            resources.ApplyResources(this.m_tbServer, "m_tbServer");
            this.m_tbServer.Name = "m_tbServer";
            // 
            // m_rbServer
            // 
            resources.ApplyResources(this.m_rbServer, "m_rbServer");
            this.m_rbServer.Name = "m_rbServer";
            this.m_rbServer.TabStop = true;
            this.m_rbServer.UseVisualStyleBackColor = true;
            // 
            // m_rbLocal
            // 
            resources.ApplyResources(this.m_rbLocal, "m_rbLocal");
            this.m_rbLocal.Name = "m_rbLocal";
            this.m_rbLocal.TabStop = true;
            this.m_rbLocal.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // m_tbDatabaseName
            // 
            resources.ApplyResources(this.m_tbDatabaseName, "m_tbDatabaseName");
            this.m_tbDatabaseName.Name = "m_tbDatabaseName";
            // 
            // m_cbProvider
            // 
            this.m_cbProvider.FormattingEnabled = true;
            this.m_cbProvider.Items.AddRange(new object[] {
            resources.GetString("m_cbProvider.Items"),
            resources.GetString("m_cbProvider.Items1"),
            resources.GetString("m_cbProvider.Items2"),
            resources.GetString("m_cbProvider.Items3")});
            resources.ApplyResources(this.m_cbProvider, "m_cbProvider");
            this.m_cbProvider.Name = "m_cbProvider";
            this.m_cbProvider.SelectedIndexChanged += new System.EventHandler(this.m_cbProvider_SelectedIndexChanged);
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
            // UpdateDataTaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_gbTarget);
            this.Name = "UpdateDataTaskControl";
            this.m_gbTarget.ResumeLayout(false);
            this.m_gbTarget.PerformLayout();
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
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.m_panelAuth.ResumeLayout(false);
            this.m_panelAuth.PerformLayout();
            this.m_computer.ResumeLayout(false);
            this.m_computer.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog;
        private System.Windows.Forms.GroupBox m_gbTarget;
        private System.Windows.Forms.CheckBox m_cbOverwrite;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button m_checkPathButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_targetFolderTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_tbDatabaseName;
        private System.Windows.Forms.ComboBox m_cbProvider;
        private System.Windows.Forms.Panel m_panelAuth;
        private System.Windows.Forms.TextBox m_tbDbPass;
        private System.Windows.Forms.RadioButton m_rbNT;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RadioButton m_rbOtherAuth;
        private System.Windows.Forms.TextBox m_tbDbUsername;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel m_computer;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox m_tbTableName;
        private System.Windows.Forms.Button m_btTestConnection;
        private System.Windows.Forms.RadioButton m_rbServer;
        private System.Windows.Forms.RadioButton m_rbLocal;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button m_btBrowseTarget;
        private System.Windows.Forms.Button m_btBrowseServer;
        private System.Windows.Forms.TextBox m_tbServer;
        private System.Windows.Forms.GroupBox m_subfolderGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton m_rbOriginal;
        private System.Windows.Forms.RadioButton m_rbNONE;
        private System.Windows.Forms.RadioButton m_rbHour;
        private System.Windows.Forms.RadioButton m_rbDay;
        private System.Windows.Forms.RadioButton m_rbWeek;
        private System.Windows.Forms.RadioButton m_rbMonth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton m_rbQuota;
        private System.Windows.Forms.NumericUpDown m_nudQuota;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton m_rbLimitDirectories;
        private System.Windows.Forms.NumericUpDown m_nudDirs;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox m_cbTakeDatTime;
    }
}
