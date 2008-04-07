namespace iba.Controls
{
    partial class ConfigurationControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationControl));
            this.m_nameTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_datDirTextBox = new System.Windows.Forms.TextBox();
            this.m_scanTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.m_subMapsCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.m_tbPass = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.m_tbUserName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_checkPathButton = new System.Windows.Forms.Button();
            this.m_browseFolderButton = new System.Windows.Forms.Button();
            this.m_refreshDats = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_cbInitialScanEnabled = new System.Windows.Forms.CheckBox();
            this.m_cbRetry = new System.Windows.Forms.CheckBox();
            this.m_retryUpDown = new System.Windows.Forms.NumericUpDown();
            this.label14 = new System.Windows.Forms.Label();
            this.m_cbRescanEnabled = new System.Windows.Forms.CheckBox();
            this.m_stopButton = new System.Windows.Forms.Button();
            this.m_applyToRunningButton = new System.Windows.Forms.Button();
            this.m_startButton = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.m_autoStartCheckBox = new System.Windows.Forms.CheckBox();
            this.m_enableCheckBox = new System.Windows.Forms.CheckBox();
            this.m_failTimeUpDown = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.m_cbRestartIbaAnalyzer = new System.Windows.Forms.CheckBox();
            this.m_nudRestartIbaAnalyzer = new System.Windows.Forms.NumericUpDown();
            this.m_browseExecutableButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.m_analyserTextBox = new System.Windows.Forms.TextBox();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.m_newTaskToolstrip = new System.Windows.Forms.ToolStrip();
            this.m_newReportButton = new System.Windows.Forms.ToolStripButton();
            this.m_newExtractButton = new System.Windows.Forms.ToolStripButton();
            this.m_newBatchfileButton = new System.Windows.Forms.ToolStripButton();
            this.m_newCopyTaskButton = new System.Windows.Forms.ToolStripButton();
            this.m_newIfTaskButton = new System.Windows.Forms.ToolStripButton();
            this.m_subfolderGroupBox = new System.Windows.Forms.GroupBox();
            this.labelmailpass = new System.Windows.Forms.Label();
            this.m_tbMailPass = new System.Windows.Forms.TextBox();
            this.labelmailuser = new System.Windows.Forms.Label();
            this.m_tbMailUsername = new System.Windows.Forms.TextBox();
            this.m_cbAuthentication = new System.Windows.Forms.CheckBox();
            this.m_testNotification = new System.Windows.Forms.Button();
            this.labelnetsendhost = new System.Windows.Forms.Label();
            this.labelmailsmtp = new System.Windows.Forms.Label();
            this.labelmailrecipient = new System.Windows.Forms.Label();
            this.m_rbNetSend = new System.Windows.Forms.RadioButton();
            this.m_tbSMTP = new System.Windows.Forms.TextBox();
            this.m_tbEmail = new System.Windows.Forms.TextBox();
            this.m_rbEmail = new System.Windows.Forms.RadioButton();
            this.m_tbNetSend = new System.Windows.Forms.TextBox();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.m_nudNotifyTime = new System.Windows.Forms.NumericUpDown();
            this.m_rbTime = new System.Windows.Forms.RadioButton();
            this.m_rbImmediate = new System.Windows.Forms.RadioButton();
            this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.m_cbCloseIbaAnalyzer = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_scanTimeUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_retryUpDown)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_failTimeUpDown)).BeginInit();
            this.groupBox5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRestartIbaAnalyzer)).BeginInit();
            this.groupBox4.SuspendLayout();
            this.m_newTaskToolstrip.SuspendLayout();
            this.m_subfolderGroupBox.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudNotifyTime)).BeginInit();
            this.SuspendLayout();
            // 
            // m_nameTextBox
            // 
            resources.ApplyResources(this.m_nameTextBox, "m_nameTextBox");
            this.m_nameTextBox.Name = "m_nameTextBox";
            this.m_nameTextBox.TextChanged += new System.EventHandler(this.m_nameTextBox_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_datDirTextBox
            // 
            resources.ApplyResources(this.m_datDirTextBox, "m_datDirTextBox");
            this.m_datDirTextBox.Name = "m_datDirTextBox";
            this.m_datDirTextBox.TextChanged += new System.EventHandler(this.m_datDirInfoChanged);
            // 
            // m_scanTimeUpDown
            // 
            resources.ApplyResources(this.m_scanTimeUpDown, "m_scanTimeUpDown");
            this.m_scanTimeUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.m_scanTimeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_scanTimeUpDown.Name = "m_scanTimeUpDown";
            this.m_scanTimeUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // m_subMapsCheckBox
            // 
            resources.ApplyResources(this.m_subMapsCheckBox, "m_subMapsCheckBox");
            this.m_subMapsCheckBox.Name = "m_subMapsCheckBox";
            this.m_subMapsCheckBox.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.m_tbPass);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.m_tbUserName);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.m_checkPathButton);
            this.groupBox1.Controls.Add(this.m_browseFolderButton);
            this.groupBox1.Controls.Add(this.m_subMapsCheckBox);
            this.groupBox1.Controls.Add(this.m_datDirTextBox);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // m_tbPass
            // 
            resources.ApplyResources(this.m_tbPass, "m_tbPass");
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.UseSystemPasswordChar = true;
            this.m_tbPass.TextChanged += new System.EventHandler(this.m_datDirInfoChanged);
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
            this.m_tbUserName.TextChanged += new System.EventHandler(this.m_datDirInfoChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // m_checkPathButton
            // 
            resources.ApplyResources(this.m_checkPathButton, "m_checkPathButton");
            this.m_checkPathButton.Image = global::iba.Properties.Resources.thumup;
            this.m_checkPathButton.Name = "m_checkPathButton";
            this.m_checkPathButton.UseVisualStyleBackColor = true;
            this.m_checkPathButton.Click += new System.EventHandler(this.m_checkPathButton_Click);
            // 
            // m_browseFolderButton
            // 
            resources.ApplyResources(this.m_browseFolderButton, "m_browseFolderButton");
            this.m_browseFolderButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            this.m_browseFolderButton.Click += new System.EventHandler(this.OnClickFolderBrowserButton);
            // 
            // m_refreshDats
            // 
            resources.ApplyResources(this.m_refreshDats, "m_refreshDats");
            this.m_refreshDats.Image = global::iba.Properties.Resources.refreshdats;
            this.m_refreshDats.Name = "m_refreshDats";
            this.m_refreshDats.UseVisualStyleBackColor = true;
            this.m_refreshDats.Click += new System.EventHandler(this.m_refreshDats_Click);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.m_nameTextBox);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.m_cbInitialScanEnabled);
            this.groupBox3.Controls.Add(this.m_cbRetry);
            this.groupBox3.Controls.Add(this.m_retryUpDown);
            this.groupBox3.Controls.Add(this.label14);
            this.groupBox3.Controls.Add(this.m_cbRescanEnabled);
            this.groupBox3.Controls.Add(this.m_refreshDats);
            this.groupBox3.Controls.Add(this.m_stopButton);
            this.groupBox3.Controls.Add(this.m_applyToRunningButton);
            this.groupBox3.Controls.Add(this.m_startButton);
            this.groupBox3.Controls.Add(this.label11);
            this.groupBox3.Controls.Add(this.m_autoStartCheckBox);
            this.groupBox3.Controls.Add(this.m_enableCheckBox);
            this.groupBox3.Controls.Add(this.m_failTimeUpDown);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.m_scanTimeUpDown);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // m_cbInitialScanEnabled
            // 
            resources.ApplyResources(this.m_cbInitialScanEnabled, "m_cbInitialScanEnabled");
            this.m_cbInitialScanEnabled.Name = "m_cbInitialScanEnabled";
            this.m_cbInitialScanEnabled.UseVisualStyleBackColor = true;
            // 
            // m_cbRetry
            // 
            resources.ApplyResources(this.m_cbRetry, "m_cbRetry");
            this.m_cbRetry.Name = "m_cbRetry";
            this.m_cbRetry.UseVisualStyleBackColor = true;
            this.m_cbRetry.CheckedChanged += new System.EventHandler(this.m_cbRetry_CheckedChanged);
            // 
            // m_retryUpDown
            // 
            resources.ApplyResources(this.m_retryUpDown, "m_retryUpDown");
            this.m_retryUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.m_retryUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_retryUpDown.Name = "m_retryUpDown";
            this.m_retryUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // m_cbRescanEnabled
            // 
            resources.ApplyResources(this.m_cbRescanEnabled, "m_cbRescanEnabled");
            this.m_cbRescanEnabled.Name = "m_cbRescanEnabled";
            this.m_cbRescanEnabled.UseVisualStyleBackColor = true;
            this.m_cbRescanEnabled.CheckedChanged += new System.EventHandler(this.m_cbRescanEnabled_CheckedChanged);
            // 
            // m_stopButton
            // 
            resources.ApplyResources(this.m_stopButton, "m_stopButton");
            this.m_stopButton.Image = global::iba.Properties.Resources.Stop;
            this.m_stopButton.Name = "m_stopButton";
            this.m_stopButton.UseVisualStyleBackColor = true;
            this.m_stopButton.Click += new System.EventHandler(this.m_stopButton_Click);
            // 
            // m_applyToRunningButton
            // 
            resources.ApplyResources(this.m_applyToRunningButton, "m_applyToRunningButton");
            this.m_applyToRunningButton.Image = global::iba.Properties.Resources.refreshconfs;
            this.m_applyToRunningButton.Name = "m_applyToRunningButton";
            this.m_applyToRunningButton.UseVisualStyleBackColor = true;
            this.m_applyToRunningButton.Click += new System.EventHandler(this.m_applyToRunningButton_Click);
            // 
            // m_startButton
            // 
            resources.ApplyResources(this.m_startButton, "m_startButton");
            this.m_startButton.Image = global::iba.Properties.Resources.Start;
            this.m_startButton.Name = "m_startButton";
            this.m_startButton.UseVisualStyleBackColor = true;
            this.m_startButton.Click += new System.EventHandler(this.m_startButton_Click);
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // m_autoStartCheckBox
            // 
            resources.ApplyResources(this.m_autoStartCheckBox, "m_autoStartCheckBox");
            this.m_autoStartCheckBox.Name = "m_autoStartCheckBox";
            this.m_autoStartCheckBox.UseVisualStyleBackColor = true;
            // 
            // m_enableCheckBox
            // 
            resources.ApplyResources(this.m_enableCheckBox, "m_enableCheckBox");
            this.m_enableCheckBox.Name = "m_enableCheckBox";
            this.m_enableCheckBox.UseVisualStyleBackColor = true;
            this.m_enableCheckBox.CheckedChanged += new System.EventHandler(this.m_enableCheckBox_CheckedChanged);
            // 
            // m_failTimeUpDown
            // 
            resources.ApplyResources(this.m_failTimeUpDown, "m_failTimeUpDown");
            this.m_failTimeUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.m_failTimeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_failTimeUpDown.Name = "m_failTimeUpDown";
            this.m_failTimeUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.m_cbCloseIbaAnalyzer);
            this.groupBox5.Controls.Add(this.label7);
            this.groupBox5.Controls.Add(this.m_cbRestartIbaAnalyzer);
            this.groupBox5.Controls.Add(this.m_nudRestartIbaAnalyzer);
            this.groupBox5.Controls.Add(this.m_browseExecutableButton);
            this.groupBox5.Controls.Add(this.m_executeIBAAButton);
            this.groupBox5.Controls.Add(this.label5);
            this.groupBox5.Controls.Add(this.m_analyserTextBox);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // m_cbRestartIbaAnalyzer
            // 
            resources.ApplyResources(this.m_cbRestartIbaAnalyzer, "m_cbRestartIbaAnalyzer");
            this.m_cbRestartIbaAnalyzer.Name = "m_cbRestartIbaAnalyzer";
            this.m_cbRestartIbaAnalyzer.UseVisualStyleBackColor = true;
            this.m_cbRestartIbaAnalyzer.CheckedChanged += new System.EventHandler(this.m_cbRestartIbaAnalyzer_CheckedChanged);
            // 
            // m_nudRestartIbaAnalyzer
            // 
            resources.ApplyResources(this.m_nudRestartIbaAnalyzer, "m_nudRestartIbaAnalyzer");
            this.m_nudRestartIbaAnalyzer.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.m_nudRestartIbaAnalyzer.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudRestartIbaAnalyzer.Name = "m_nudRestartIbaAnalyzer";
            this.m_nudRestartIbaAnalyzer.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // m_browseExecutableButton
            // 
            resources.ApplyResources(this.m_browseExecutableButton, "m_browseExecutableButton");
            this.m_browseExecutableButton.Image = global::iba.Properties.Resources.open;
            this.m_browseExecutableButton.Name = "m_browseExecutableButton";
            this.m_browseExecutableButton.UseVisualStyleBackColor = true;
            this.m_browseExecutableButton.Click += new System.EventHandler(this.m_browseExecutableButton_Click);
            // 
            // m_executeIBAAButton
            // 
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.Analyzer_001;
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.OnClickExecuteButton);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // m_analyserTextBox
            // 
            resources.ApplyResources(this.m_analyserTextBox, "m_analyserTextBox");
            this.m_analyserTextBox.Name = "m_analyserTextBox";
            this.m_analyserTextBox.TextChanged += new System.EventHandler(this.m_analyserTextBox_TextChanged);
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.m_newTaskToolstrip);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // m_newTaskToolstrip
            // 
            resources.ApplyResources(this.m_newTaskToolstrip, "m_newTaskToolstrip");
            this.m_newTaskToolstrip.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.m_newTaskToolstrip.ImageScalingSize = new System.Drawing.Size(32, 32);
            this.m_newTaskToolstrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.m_newReportButton,
            this.m_newExtractButton,
            this.m_newBatchfileButton,
            this.m_newCopyTaskButton,
            this.m_newIfTaskButton});
            this.m_newTaskToolstrip.Name = "m_newTaskToolstrip";
            this.m_newTaskToolstrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // m_newReportButton
            // 
            resources.ApplyResources(this.m_newReportButton, "m_newReportButton");
            this.m_newReportButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_newReportButton.Name = "m_newReportButton";
            this.m_newReportButton.Click += new System.EventHandler(this.m_newReportButton_Click);
            // 
            // m_newExtractButton
            // 
            resources.ApplyResources(this.m_newExtractButton, "m_newExtractButton");
            this.m_newExtractButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_newExtractButton.Name = "m_newExtractButton";
            this.m_newExtractButton.Click += new System.EventHandler(this.m_newExtractButton_Click);
            // 
            // m_newBatchfileButton
            // 
            resources.ApplyResources(this.m_newBatchfileButton, "m_newBatchfileButton");
            this.m_newBatchfileButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_newBatchfileButton.Name = "m_newBatchfileButton";
            this.m_newBatchfileButton.Click += new System.EventHandler(this.m_newBatchfileButton_Click);
            // 
            // m_newCopyTaskButton
            // 
            resources.ApplyResources(this.m_newCopyTaskButton, "m_newCopyTaskButton");
            this.m_newCopyTaskButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_newCopyTaskButton.Name = "m_newCopyTaskButton";
            this.m_newCopyTaskButton.Click += new System.EventHandler(this.m_newCopyTaskButton_Click);
            // 
            // m_newIfTaskButton
            // 
            resources.ApplyResources(this.m_newIfTaskButton, "m_newIfTaskButton");
            this.m_newIfTaskButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.m_newIfTaskButton.Name = "m_newIfTaskButton";
            this.m_newIfTaskButton.Click += new System.EventHandler(this.m_newIfTaskButton_Click);
            // 
            // m_subfolderGroupBox
            // 
            resources.ApplyResources(this.m_subfolderGroupBox, "m_subfolderGroupBox");
            this.m_subfolderGroupBox.Controls.Add(this.labelmailpass);
            this.m_subfolderGroupBox.Controls.Add(this.m_tbMailPass);
            this.m_subfolderGroupBox.Controls.Add(this.labelmailuser);
            this.m_subfolderGroupBox.Controls.Add(this.m_tbMailUsername);
            this.m_subfolderGroupBox.Controls.Add(this.m_cbAuthentication);
            this.m_subfolderGroupBox.Controls.Add(this.m_testNotification);
            this.m_subfolderGroupBox.Controls.Add(this.labelnetsendhost);
            this.m_subfolderGroupBox.Controls.Add(this.labelmailsmtp);
            this.m_subfolderGroupBox.Controls.Add(this.labelmailrecipient);
            this.m_subfolderGroupBox.Controls.Add(this.m_rbNetSend);
            this.m_subfolderGroupBox.Controls.Add(this.m_tbSMTP);
            this.m_subfolderGroupBox.Controls.Add(this.m_tbEmail);
            this.m_subfolderGroupBox.Controls.Add(this.m_rbEmail);
            this.m_subfolderGroupBox.Controls.Add(this.m_tbNetSend);
            this.m_subfolderGroupBox.Name = "m_subfolderGroupBox";
            this.m_subfolderGroupBox.TabStop = false;
            // 
            // labelmailpass
            // 
            resources.ApplyResources(this.labelmailpass, "labelmailpass");
            this.labelmailpass.Name = "labelmailpass";
            // 
            // m_tbMailPass
            // 
            resources.ApplyResources(this.m_tbMailPass, "m_tbMailPass");
            this.m_tbMailPass.Name = "m_tbMailPass";
            this.m_tbMailPass.UseSystemPasswordChar = true;
            // 
            // labelmailuser
            // 
            resources.ApplyResources(this.labelmailuser, "labelmailuser");
            this.labelmailuser.Name = "labelmailuser";
            // 
            // m_tbMailUsername
            // 
            resources.ApplyResources(this.m_tbMailUsername, "m_tbMailUsername");
            this.m_tbMailUsername.Name = "m_tbMailUsername";
            // 
            // m_cbAuthentication
            // 
            resources.ApplyResources(this.m_cbAuthentication, "m_cbAuthentication");
            this.m_cbAuthentication.Name = "m_cbAuthentication";
            this.m_cbAuthentication.UseVisualStyleBackColor = true;
            this.m_cbAuthentication.CheckedChanged += new System.EventHandler(this.m_cbAuthentication_CheckedChanged);
            // 
            // m_testNotification
            // 
            resources.ApplyResources(this.m_testNotification, "m_testNotification");
            this.m_testNotification.Image = global::iba.Properties.Resources.Fax;
            this.m_testNotification.Name = "m_testNotification";
            this.m_testNotification.UseVisualStyleBackColor = true;
            this.m_testNotification.Click += new System.EventHandler(this.m_testNotification_Click);
            // 
            // labelnetsendhost
            // 
            resources.ApplyResources(this.labelnetsendhost, "labelnetsendhost");
            this.labelnetsendhost.Name = "labelnetsendhost";
            // 
            // labelmailsmtp
            // 
            resources.ApplyResources(this.labelmailsmtp, "labelmailsmtp");
            this.labelmailsmtp.Name = "labelmailsmtp";
            // 
            // labelmailrecipient
            // 
            resources.ApplyResources(this.labelmailrecipient, "labelmailrecipient");
            this.labelmailrecipient.Name = "labelmailrecipient";
            // 
            // m_rbNetSend
            // 
            resources.ApplyResources(this.m_rbNetSend, "m_rbNetSend");
            this.m_rbNetSend.Name = "m_rbNetSend";
            this.m_rbNetSend.TabStop = true;
            this.m_rbNetSend.UseVisualStyleBackColor = true;
            this.m_rbNetSend.CheckedChanged += new System.EventHandler(this.m_rbOutputCheckedChanged);
            // 
            // m_tbSMTP
            // 
            resources.ApplyResources(this.m_tbSMTP, "m_tbSMTP");
            this.m_tbSMTP.Name = "m_tbSMTP";
            // 
            // m_tbEmail
            // 
            resources.ApplyResources(this.m_tbEmail, "m_tbEmail");
            this.m_tbEmail.Name = "m_tbEmail";
            // 
            // m_rbEmail
            // 
            resources.ApplyResources(this.m_rbEmail, "m_rbEmail");
            this.m_rbEmail.Name = "m_rbEmail";
            this.m_rbEmail.TabStop = true;
            this.m_rbEmail.UseVisualStyleBackColor = true;
            this.m_rbEmail.CheckedChanged += new System.EventHandler(this.m_rbOutputCheckedChanged);
            // 
            // m_tbNetSend
            // 
            resources.ApplyResources(this.m_tbNetSend, "m_tbNetSend");
            this.m_tbNetSend.Name = "m_tbNetSend";
            // 
            // groupBox6
            // 
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Controls.Add(this.groupBox7);
            this.groupBox6.Controls.Add(this.m_subfolderGroupBox);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // groupBox7
            // 
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Controls.Add(this.label6);
            this.groupBox7.Controls.Add(this.m_nudNotifyTime);
            this.groupBox7.Controls.Add(this.m_rbTime);
            this.groupBox7.Controls.Add(this.m_rbImmediate);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // m_nudNotifyTime
            // 
            resources.ApplyResources(this.m_nudNotifyTime, "m_nudNotifyTime");
            this.m_nudNotifyTime.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.m_nudNotifyTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudNotifyTime.Name = "m_nudNotifyTime";
            this.m_nudNotifyTime.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // m_rbTime
            // 
            resources.ApplyResources(this.m_rbTime, "m_rbTime");
            this.m_rbTime.Name = "m_rbTime";
            this.m_rbTime.TabStop = true;
            this.m_rbTime.UseVisualStyleBackColor = true;
            this.m_rbTime.CheckedChanged += new System.EventHandler(this.m_rbImmediate_CheckedChanged);
            // 
            // m_rbImmediate
            // 
            resources.ApplyResources(this.m_rbImmediate, "m_rbImmediate");
            this.m_rbImmediate.Name = "m_rbImmediate";
            this.m_rbImmediate.TabStop = true;
            this.m_rbImmediate.UseVisualStyleBackColor = true;
            this.m_rbImmediate.CheckedChanged += new System.EventHandler(this.m_rbImmediate_CheckedChanged);
            // 
            // m_openFileDialog1
            // 
            this.m_openFileDialog1.FileName = "openFileDialog1";
            // 
            // m_cbCloseIbaAnalyzer
            // 
            resources.ApplyResources(this.m_cbCloseIbaAnalyzer, "m_cbCloseIbaAnalyzer");
            this.m_cbCloseIbaAnalyzer.Name = "m_cbCloseIbaAnalyzer";
            this.m_cbCloseIbaAnalyzer.UseVisualStyleBackColor = true;
            // 
            // ConfigurationControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox2);
            this.MinimumSize = new System.Drawing.Size(620, 250);
            this.Name = "ConfigurationControl";
            ((System.ComponentModel.ISupportInitialize)(this.m_scanTimeUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_retryUpDown)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_failTimeUpDown)).EndInit();
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRestartIbaAnalyzer)).EndInit();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.m_newTaskToolstrip.ResumeLayout(false);
            this.m_newTaskToolstrip.PerformLayout();
            this.m_subfolderGroupBox.ResumeLayout(false);
            this.m_subfolderGroupBox.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudNotifyTime)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox m_nameTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_datDirTextBox;
        private System.Windows.Forms.NumericUpDown m_scanTimeUpDown;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox m_subMapsCheckBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox m_analyserTextBox;
        private System.Windows.Forms.CheckBox m_enableCheckBox;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.Button m_browseExecutableButton;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog1;
        private System.Windows.Forms.Button m_stopButton;
        private System.Windows.Forms.Button m_startButton;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button m_refreshDats;
        private System.Windows.Forms.CheckBox m_autoStartCheckBox;
        private System.Windows.Forms.GroupBox m_subfolderGroupBox;
        private System.Windows.Forms.RadioButton m_rbNetSend;
        private System.Windows.Forms.TextBox m_tbEmail;
        private System.Windows.Forms.RadioButton m_rbEmail;
        private System.Windows.Forms.TextBox m_tbNetSend;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton m_rbTime;
        private System.Windows.Forms.RadioButton m_rbImmediate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown m_nudNotifyTime;
        private System.Windows.Forms.Label labelmailrecipient;
        private System.Windows.Forms.TextBox m_tbSMTP;
        private System.Windows.Forms.Label labelnetsendhost;
        private System.Windows.Forms.Label labelmailsmtp;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown m_failTimeUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.CheckBox m_cbRescanEnabled;
        private System.Windows.Forms.Button m_applyToRunningButton;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button m_checkPathButton;
        private System.Windows.Forms.ToolStrip m_newTaskToolstrip;
        private System.Windows.Forms.ToolStripButton m_newReportButton;
        private System.Windows.Forms.ToolStripButton m_newExtractButton;
        private System.Windows.Forms.ToolStripButton m_newBatchfileButton;
        private System.Windows.Forms.ToolStripButton m_newCopyTaskButton;
        private System.Windows.Forms.ToolStripButton m_newIfTaskButton;
        private System.Windows.Forms.CheckBox m_cbRetry;
        private System.Windows.Forms.NumericUpDown m_retryUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button m_testNotification;
        private System.Windows.Forms.Label labelmailpass;
        private System.Windows.Forms.TextBox m_tbMailPass;
        private System.Windows.Forms.Label labelmailuser;
        private System.Windows.Forms.TextBox m_tbMailUsername;
        private System.Windows.Forms.CheckBox m_cbAuthentication;
        private System.Windows.Forms.CheckBox m_cbInitialScanEnabled;
        private System.Windows.Forms.NumericUpDown m_nudRestartIbaAnalyzer;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox m_cbRestartIbaAnalyzer;
        private System.Windows.Forms.CheckBox m_cbCloseIbaAnalyzer;
    }
}
