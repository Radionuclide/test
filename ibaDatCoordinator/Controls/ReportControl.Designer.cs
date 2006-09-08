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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ReportControl));
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_panelFile = new System.Windows.Forms.Panel();
            this.m_tbPass = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.m_tbUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_checkPathButton = new System.Windows.Forms.Button();
            this.m_subfolderGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_folderNumber = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbHour = new System.Windows.Forms.RadioButton();
            this.m_rbDay = new System.Windows.Forms.RadioButton();
            this.m_rbWeek = new System.Windows.Forms.RadioButton();
            this.m_rbMonth = new System.Windows.Forms.RadioButton();
            this.m_rbNONE = new System.Windows.Forms.RadioButton();
            this.m_rbOriginal = new System.Windows.Forms.RadioButton();
            this.m_browseFolderButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_targetFolderTextBox = new System.Windows.Forms.TextBox();
            this.m_extensionComboBox = new System.Windows.Forms.ComboBox();
            this.m_rbPrint = new System.Windows.Forms.RadioButton();
            this.m_rbFile = new System.Windows.Forms.RadioButton();
            this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_nameTextBox = new System.Windows.Forms.TextBox();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browseFileButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rb1stFailure = new System.Windows.Forms.RadioButton();
            this.m_rbAlways = new System.Windows.Forms.RadioButton();
            this.m_rbFailure = new System.Windows.Forms.RadioButton();
            this.m_rbDisabled = new System.Windows.Forms.RadioButton();
            this.m_rbSucces = new System.Windows.Forms.RadioButton();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbNot1stFailure = new System.Windows.Forms.RadioButton();
            this.m_rbNotAlways = new System.Windows.Forms.RadioButton();
            this.m_rbNotFailure = new System.Windows.Forms.RadioButton();
            this.m_rbNotDisabled = new System.Windows.Forms.RadioButton();
            this.m_rbNotSuccess = new System.Windows.Forms.RadioButton();
            this.groupBox2.SuspendLayout();
            this.m_panelFile.SuspendLayout();
            this.m_subfolderGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_folderNumber)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.m_panelFile);
            this.groupBox2.Controls.Add(this.m_extensionComboBox);
            this.groupBox2.Controls.Add(this.m_rbPrint);
            this.groupBox2.Controls.Add(this.m_rbFile);
            this.groupBox2.Font = null;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_panelFile
            // 
            this.m_panelFile.AccessibleDescription = null;
            this.m_panelFile.AccessibleName = null;
            resources.ApplyResources(this.m_panelFile, "m_panelFile");
            this.m_panelFile.BackgroundImage = null;
            this.m_panelFile.Controls.Add(this.m_tbPass);
            this.m_panelFile.Controls.Add(this.label12);
            this.m_panelFile.Controls.Add(this.m_tbUserName);
            this.m_panelFile.Controls.Add(this.label4);
            this.m_panelFile.Controls.Add(this.m_checkPathButton);
            this.m_panelFile.Controls.Add(this.m_subfolderGroupBox);
            this.m_panelFile.Controls.Add(this.m_browseFolderButton);
            this.m_panelFile.Controls.Add(this.label1);
            this.m_panelFile.Controls.Add(this.m_targetFolderTextBox);
            this.m_panelFile.Font = null;
            this.m_panelFile.Name = "m_panelFile";
            // 
            // m_tbPass
            // 
            this.m_tbPass.AccessibleDescription = null;
            this.m_tbPass.AccessibleName = null;
            resources.ApplyResources(this.m_tbPass, "m_tbPass");
            this.m_tbPass.BackgroundImage = null;
            this.m_tbPass.Font = null;
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.TextChanged += new System.EventHandler(this.m_reportDirInfoChanged);
            // 
            // label12
            // 
            this.label12.AccessibleDescription = null;
            this.label12.AccessibleName = null;
            resources.ApplyResources(this.label12, "label12");
            this.label12.Font = null;
            this.label12.Name = "label12";
            // 
            // m_tbUserName
            // 
            this.m_tbUserName.AccessibleDescription = null;
            this.m_tbUserName.AccessibleName = null;
            resources.ApplyResources(this.m_tbUserName, "m_tbUserName");
            this.m_tbUserName.BackgroundImage = null;
            this.m_tbUserName.Font = null;
            this.m_tbUserName.Name = "m_tbUserName";
            this.m_tbUserName.TextChanged += new System.EventHandler(this.m_reportDirInfoChanged);
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.Name = "label4";
            // 
            // m_checkPathButton
            // 
            this.m_checkPathButton.AccessibleDescription = null;
            this.m_checkPathButton.AccessibleName = null;
            resources.ApplyResources(this.m_checkPathButton, "m_checkPathButton");
            this.m_checkPathButton.BackgroundImage = null;
            this.m_checkPathButton.Font = null;
            this.m_checkPathButton.Image = global::iba.Properties.Resources.thumup;
            this.m_checkPathButton.Name = "m_checkPathButton";
            this.m_checkPathButton.UseVisualStyleBackColor = true;
            this.m_checkPathButton.Click += new System.EventHandler(this.m_checkPathButton_Click);
            // 
            // m_subfolderGroupBox
            // 
            this.m_subfolderGroupBox.AccessibleDescription = null;
            this.m_subfolderGroupBox.AccessibleName = null;
            resources.ApplyResources(this.m_subfolderGroupBox, "m_subfolderGroupBox");
            this.m_subfolderGroupBox.BackgroundImage = null;
            this.m_subfolderGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.m_subfolderGroupBox.Font = null;
            this.m_subfolderGroupBox.Name = "m_subfolderGroupBox";
            this.m_subfolderGroupBox.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.AccessibleDescription = null;
            this.tableLayoutPanel2.AccessibleName = null;
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.BackgroundImage = null;
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Font = null;
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // panel1
            // 
            this.panel1.AccessibleDescription = null;
            this.panel1.AccessibleName = null;
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.BackgroundImage = null;
            this.panel1.Controls.Add(this.m_folderNumber);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Font = null;
            this.panel1.Name = "panel1";
            // 
            // m_folderNumber
            // 
            this.m_folderNumber.AccessibleDescription = null;
            this.m_folderNumber.AccessibleName = null;
            resources.ApplyResources(this.m_folderNumber, "m_folderNumber");
            this.m_folderNumber.Font = null;
            this.m_folderNumber.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_folderNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_folderNumber.Name = "m_folderNumber";
            this.m_folderNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.AccessibleDescription = null;
            this.tableLayoutPanel3.AccessibleName = null;
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.BackgroundImage = null;
            this.tableLayoutPanel3.Controls.Add(this.m_rbHour, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbDay, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbWeek, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbMonth, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_rbNONE, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_rbOriginal, 1, 1);
            this.tableLayoutPanel3.Font = null;
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // m_rbHour
            // 
            this.m_rbHour.AccessibleDescription = null;
            this.m_rbHour.AccessibleName = null;
            resources.ApplyResources(this.m_rbHour, "m_rbHour");
            this.m_rbHour.BackgroundImage = null;
            this.m_rbHour.Font = null;
            this.m_rbHour.Name = "m_rbHour";
            this.m_rbHour.TabStop = true;
            this.m_rbHour.UseVisualStyleBackColor = true;
            // 
            // m_rbDay
            // 
            this.m_rbDay.AccessibleDescription = null;
            this.m_rbDay.AccessibleName = null;
            resources.ApplyResources(this.m_rbDay, "m_rbDay");
            this.m_rbDay.BackgroundImage = null;
            this.m_rbDay.Font = null;
            this.m_rbDay.Name = "m_rbDay";
            this.m_rbDay.TabStop = true;
            this.m_rbDay.UseVisualStyleBackColor = true;
            // 
            // m_rbWeek
            // 
            this.m_rbWeek.AccessibleDescription = null;
            this.m_rbWeek.AccessibleName = null;
            resources.ApplyResources(this.m_rbWeek, "m_rbWeek");
            this.m_rbWeek.BackgroundImage = null;
            this.m_rbWeek.Font = null;
            this.m_rbWeek.Name = "m_rbWeek";
            this.m_rbWeek.TabStop = true;
            this.m_rbWeek.UseVisualStyleBackColor = true;
            // 
            // m_rbMonth
            // 
            this.m_rbMonth.AccessibleDescription = null;
            this.m_rbMonth.AccessibleName = null;
            resources.ApplyResources(this.m_rbMonth, "m_rbMonth");
            this.m_rbMonth.BackgroundImage = null;
            this.m_rbMonth.Font = null;
            this.m_rbMonth.Name = "m_rbMonth";
            this.m_rbMonth.TabStop = true;
            this.m_rbMonth.UseVisualStyleBackColor = true;
            // 
            // m_rbNONE
            // 
            this.m_rbNONE.AccessibleDescription = null;
            this.m_rbNONE.AccessibleName = null;
            resources.ApplyResources(this.m_rbNONE, "m_rbNONE");
            this.m_rbNONE.BackgroundImage = null;
            this.m_rbNONE.Font = null;
            this.m_rbNONE.Name = "m_rbNONE";
            this.m_rbNONE.TabStop = true;
            this.m_rbNONE.UseVisualStyleBackColor = true;
            // 
            // m_rbOriginal
            // 
            this.m_rbOriginal.AccessibleDescription = null;
            this.m_rbOriginal.AccessibleName = null;
            resources.ApplyResources(this.m_rbOriginal, "m_rbOriginal");
            this.m_rbOriginal.BackgroundImage = null;
            this.m_rbOriginal.Font = null;
            this.m_rbOriginal.Name = "m_rbOriginal";
            this.m_rbOriginal.TabStop = true;
            this.m_rbOriginal.UseVisualStyleBackColor = true;
            // 
            // m_browseFolderButton
            // 
            this.m_browseFolderButton.AccessibleDescription = null;
            this.m_browseFolderButton.AccessibleName = null;
            resources.ApplyResources(this.m_browseFolderButton, "m_browseFolderButton");
            this.m_browseFolderButton.BackgroundImage = null;
            this.m_browseFolderButton.Font = null;
            this.m_browseFolderButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            this.m_browseFolderButton.Click += new System.EventHandler(this.m_browseFolderButton_Click);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // m_targetFolderTextBox
            // 
            this.m_targetFolderTextBox.AccessibleDescription = null;
            this.m_targetFolderTextBox.AccessibleName = null;
            resources.ApplyResources(this.m_targetFolderTextBox, "m_targetFolderTextBox");
            this.m_targetFolderTextBox.BackgroundImage = null;
            this.m_targetFolderTextBox.Font = null;
            this.m_targetFolderTextBox.Name = "m_targetFolderTextBox";
            this.m_targetFolderTextBox.TextChanged += new System.EventHandler(this.m_reportDirInfoChanged);
            // 
            // m_extensionComboBox
            // 
            this.m_extensionComboBox.AccessibleDescription = null;
            this.m_extensionComboBox.AccessibleName = null;
            resources.ApplyResources(this.m_extensionComboBox, "m_extensionComboBox");
            this.m_extensionComboBox.BackgroundImage = null;
            this.m_extensionComboBox.Font = null;
            this.m_extensionComboBox.FormattingEnabled = true;
            this.m_extensionComboBox.Name = "m_extensionComboBox";
            // 
            // m_rbPrint
            // 
            this.m_rbPrint.AccessibleDescription = null;
            this.m_rbPrint.AccessibleName = null;
            resources.ApplyResources(this.m_rbPrint, "m_rbPrint");
            this.m_rbPrint.BackgroundImage = null;
            this.m_rbPrint.Font = null;
            this.m_rbPrint.Name = "m_rbPrint";
            this.m_rbPrint.TabStop = true;
            this.m_rbPrint.UseVisualStyleBackColor = true;
            this.m_rbPrint.CheckedChanged += new System.EventHandler(this.m_rbPrint_CheckedChanged);
            // 
            // m_rbFile
            // 
            this.m_rbFile.AccessibleDescription = null;
            this.m_rbFile.AccessibleName = null;
            resources.ApplyResources(this.m_rbFile, "m_rbFile");
            this.m_rbFile.BackgroundImage = null;
            this.m_rbFile.Font = null;
            this.m_rbFile.Name = "m_rbFile";
            this.m_rbFile.TabStop = true;
            this.m_rbFile.UseVisualStyleBackColor = true;
            // 
            // m_openFileDialog1
            // 
            resources.ApplyResources(this.m_openFileDialog1, "m_openFileDialog1");
            // 
            // m_folderBrowserDialog1
            // 
            resources.ApplyResources(this.m_folderBrowserDialog1, "m_folderBrowserDialog1");
            // 
            // groupBox3
            // 
            this.groupBox3.AccessibleDescription = null;
            this.groupBox3.AccessibleName = null;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.BackgroundImage = null;
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.m_nameTextBox);
            this.groupBox3.Font = null;
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Font = null;
            this.label5.Name = "label5";
            // 
            // m_nameTextBox
            // 
            this.m_nameTextBox.AccessibleDescription = null;
            this.m_nameTextBox.AccessibleName = null;
            resources.ApplyResources(this.m_nameTextBox, "m_nameTextBox");
            this.m_nameTextBox.BackgroundImage = null;
            this.m_nameTextBox.Font = null;
            this.m_nameTextBox.Name = "m_nameTextBox";
            this.m_nameTextBox.TextChanged += new System.EventHandler(this.m_nameTextBox_TextChanged);
            // 
            // m_pdoFileTextBox
            // 
            this.m_pdoFileTextBox.AccessibleDescription = null;
            this.m_pdoFileTextBox.AccessibleName = null;
            resources.ApplyResources(this.m_pdoFileTextBox, "m_pdoFileTextBox");
            this.m_pdoFileTextBox.BackgroundImage = null;
            this.m_pdoFileTextBox.Font = null;
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
            // 
            // m_browseFileButton
            // 
            this.m_browseFileButton.AccessibleDescription = null;
            this.m_browseFileButton.AccessibleName = null;
            resources.ApplyResources(this.m_browseFileButton, "m_browseFileButton");
            this.m_browseFileButton.BackgroundImage = null;
            this.m_browseFileButton.Font = null;
            this.m_browseFileButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFileButton.Name = "m_browseFileButton";
            this.m_browseFileButton.UseVisualStyleBackColor = true;
            this.m_browseFileButton.Click += new System.EventHandler(this.m_browseFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            this.m_executeIBAAButton.AccessibleDescription = null;
            this.m_executeIBAAButton.AccessibleName = null;
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.BackgroundImage = null;
            this.m_executeIBAAButton.Font = null;
            this.m_executeIBAAButton.Image = global::iba.Properties.Resources.Analyzer_001;
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.m_executeIBAAButton);
            this.groupBox1.Controls.Add(this.m_browseFileButton);
            this.groupBox1.Controls.Add(this.m_pdoFileTextBox);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox4
            // 
            this.groupBox4.AccessibleDescription = null;
            this.groupBox4.AccessibleName = null;
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.BackgroundImage = null;
            this.groupBox4.Controls.Add(this.tableLayoutPanel1);
            this.groupBox4.Font = null;
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.AccessibleDescription = null;
            this.tableLayoutPanel1.AccessibleName = null;
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.BackgroundImage = null;
            this.tableLayoutPanel1.Controls.Add(this.m_rb1stFailure, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_rbAlways, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_rbFailure, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_rbDisabled, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_rbSucces, 1, 0);
            this.tableLayoutPanel1.Font = null;
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // m_rb1stFailure
            // 
            this.m_rb1stFailure.AccessibleDescription = null;
            this.m_rb1stFailure.AccessibleName = null;
            resources.ApplyResources(this.m_rb1stFailure, "m_rb1stFailure");
            this.m_rb1stFailure.BackgroundImage = null;
            this.m_rb1stFailure.Font = null;
            this.m_rb1stFailure.Name = "m_rb1stFailure";
            this.m_rb1stFailure.TabStop = true;
            this.m_rb1stFailure.UseVisualStyleBackColor = true;
            this.m_rb1stFailure.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbAlways
            // 
            this.m_rbAlways.AccessibleDescription = null;
            this.m_rbAlways.AccessibleName = null;
            resources.ApplyResources(this.m_rbAlways, "m_rbAlways");
            this.m_rbAlways.BackgroundImage = null;
            this.m_rbAlways.Font = null;
            this.m_rbAlways.Name = "m_rbAlways";
            this.m_rbAlways.TabStop = true;
            this.m_rbAlways.UseVisualStyleBackColor = true;
            this.m_rbAlways.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbFailure
            // 
            this.m_rbFailure.AccessibleDescription = null;
            this.m_rbFailure.AccessibleName = null;
            resources.ApplyResources(this.m_rbFailure, "m_rbFailure");
            this.m_rbFailure.BackgroundImage = null;
            this.m_rbFailure.Font = null;
            this.m_rbFailure.Name = "m_rbFailure";
            this.m_rbFailure.TabStop = true;
            this.m_rbFailure.UseVisualStyleBackColor = true;
            this.m_rbFailure.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbDisabled
            // 
            this.m_rbDisabled.AccessibleDescription = null;
            this.m_rbDisabled.AccessibleName = null;
            resources.ApplyResources(this.m_rbDisabled, "m_rbDisabled");
            this.m_rbDisabled.BackgroundImage = null;
            this.m_rbDisabled.Font = null;
            this.m_rbDisabled.Name = "m_rbDisabled";
            this.m_rbDisabled.TabStop = true;
            this.m_rbDisabled.UseVisualStyleBackColor = true;
            this.m_rbDisabled.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbSucces
            // 
            this.m_rbSucces.AccessibleDescription = null;
            this.m_rbSucces.AccessibleName = null;
            resources.ApplyResources(this.m_rbSucces, "m_rbSucces");
            this.m_rbSucces.BackgroundImage = null;
            this.m_rbSucces.Font = null;
            this.m_rbSucces.Name = "m_rbSucces";
            this.m_rbSucces.TabStop = true;
            this.m_rbSucces.UseVisualStyleBackColor = true;
            this.m_rbSucces.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // groupBox7
            // 
            this.groupBox7.AccessibleDescription = null;
            this.groupBox7.AccessibleName = null;
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.BackgroundImage = null;
            this.groupBox7.Controls.Add(this.tableLayoutPanel6);
            this.groupBox7.Font = null;
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.AccessibleDescription = null;
            this.tableLayoutPanel6.AccessibleName = null;
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.BackgroundImage = null;
            this.tableLayoutPanel6.Controls.Add(this.m_rbNot1stFailure, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotAlways, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotFailure, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotDisabled, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotSuccess, 1, 0);
            this.tableLayoutPanel6.Font = null;
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // m_rbNot1stFailure
            // 
            this.m_rbNot1stFailure.AccessibleDescription = null;
            this.m_rbNot1stFailure.AccessibleName = null;
            resources.ApplyResources(this.m_rbNot1stFailure, "m_rbNot1stFailure");
            this.m_rbNot1stFailure.BackgroundImage = null;
            this.m_rbNot1stFailure.Font = null;
            this.m_rbNot1stFailure.Name = "m_rbNot1stFailure";
            this.m_rbNot1stFailure.TabStop = true;
            this.m_rbNot1stFailure.UseVisualStyleBackColor = true;
            // 
            // m_rbNotAlways
            // 
            this.m_rbNotAlways.AccessibleDescription = null;
            this.m_rbNotAlways.AccessibleName = null;
            resources.ApplyResources(this.m_rbNotAlways, "m_rbNotAlways");
            this.m_rbNotAlways.BackgroundImage = null;
            this.m_rbNotAlways.Font = null;
            this.m_rbNotAlways.Name = "m_rbNotAlways";
            this.m_rbNotAlways.TabStop = true;
            this.m_rbNotAlways.UseVisualStyleBackColor = true;
            // 
            // m_rbNotFailure
            // 
            this.m_rbNotFailure.AccessibleDescription = null;
            this.m_rbNotFailure.AccessibleName = null;
            resources.ApplyResources(this.m_rbNotFailure, "m_rbNotFailure");
            this.m_rbNotFailure.BackgroundImage = null;
            this.m_rbNotFailure.Font = null;
            this.m_rbNotFailure.Name = "m_rbNotFailure";
            this.m_rbNotFailure.TabStop = true;
            this.m_rbNotFailure.UseVisualStyleBackColor = true;
            // 
            // m_rbNotDisabled
            // 
            this.m_rbNotDisabled.AccessibleDescription = null;
            this.m_rbNotDisabled.AccessibleName = null;
            resources.ApplyResources(this.m_rbNotDisabled, "m_rbNotDisabled");
            this.m_rbNotDisabled.BackgroundImage = null;
            this.m_rbNotDisabled.Font = null;
            this.m_rbNotDisabled.Name = "m_rbNotDisabled";
            this.m_rbNotDisabled.TabStop = true;
            this.m_rbNotDisabled.UseVisualStyleBackColor = true;
            // 
            // m_rbNotSuccess
            // 
            this.m_rbNotSuccess.AccessibleDescription = null;
            this.m_rbNotSuccess.AccessibleName = null;
            resources.ApplyResources(this.m_rbNotSuccess, "m_rbNotSuccess");
            this.m_rbNotSuccess.BackgroundImage = null;
            this.m_rbNotSuccess.Font = null;
            this.m_rbNotSuccess.Name = "m_rbNotSuccess";
            this.m_rbNotSuccess.TabStop = true;
            this.m_rbNotSuccess.UseVisualStyleBackColor = true;
            // 
            // ReportControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Font = null;
            this.MinimumSize = new System.Drawing.Size(620, 620);
            this.Name = "ReportControl";
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.m_panelFile.ResumeLayout(false);
            this.m_panelFile.PerformLayout();
            this.m_subfolderGroupBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_folderNumber)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.TextBox m_targetFolderTextBox;
        private System.Windows.Forms.GroupBox m_subfolderGroupBox;
        private System.Windows.Forms.RadioButton m_rbHour;
        private System.Windows.Forms.RadioButton m_rbNONE;
        private System.Windows.Forms.RadioButton m_rbWeek;
        private System.Windows.Forms.RadioButton m_rbDay;
        private System.Windows.Forms.NumericUpDown m_folderNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton m_rbMonth;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox m_nameTextBox;
        private System.Windows.Forms.ComboBox m_extensionComboBox;
        private System.Windows.Forms.RadioButton m_rbPrint;
        private System.Windows.Forms.RadioButton m_rbFile;
        private System.Windows.Forms.TextBox m_pdoFileTextBox;
        private System.Windows.Forms.Button m_browseFileButton;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton m_rb1stFailure;
        private System.Windows.Forms.RadioButton m_rbAlways;
        private System.Windows.Forms.RadioButton m_rbFailure;
        private System.Windows.Forms.RadioButton m_rbDisabled;
        private System.Windows.Forms.RadioButton m_rbSucces;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton m_rbNot1stFailure;
        private System.Windows.Forms.RadioButton m_rbNotAlways;
        private System.Windows.Forms.RadioButton m_rbNotFailure;
        private System.Windows.Forms.RadioButton m_rbNotDisabled;
        private System.Windows.Forms.RadioButton m_rbNotSuccess;
        private System.Windows.Forms.RadioButton m_rbOriginal;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button m_checkPathButton;
        private System.Windows.Forms.Panel m_panelFile;

    }
}
