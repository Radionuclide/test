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
            this.m_folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.m_browseFolderButton = new System.Windows.Forms.Button();
            this.m_gbTarget = new System.Windows.Forms.GroupBox();
            this.m_btBrowseTarget = new System.Windows.Forms.Button();
            this.m_cbOverwrite = new System.Windows.Forms.CheckBox();
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
            this.label1 = new System.Windows.Forms.Label();
            this.m_targetFolderTextBox = new System.Windows.Forms.TextBox();
            this.m_serverBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
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
            this.panel1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).BeginInit();
            this.panel3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.m_panelAuth.SuspendLayout();
            this.m_computer.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_browseFolderButton
            // 
            this.m_browseFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseFolderButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFolderButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseFolderButton.Location = new System.Drawing.Point(870, 16);
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.Size = new System.Drawing.Size(40, 40);
            this.m_browseFolderButton.TabIndex = 2;
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            // 
            // m_gbTarget
            // 
            this.m_gbTarget.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_gbTarget.Controls.Add(this.m_btBrowseTarget);
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
            this.m_gbTarget.Location = new System.Drawing.Point(0, 232);
            this.m_gbTarget.Name = "m_gbTarget";
            this.m_gbTarget.Size = new System.Drawing.Size(566, 258);
            this.m_gbTarget.TabIndex = 1;
            this.m_gbTarget.TabStop = false;
            this.m_gbTarget.Text = "Target";
            // 
            // m_btBrowseTarget
            // 
            this.m_btBrowseTarget.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btBrowseTarget.Image = global::iba.Properties.Resources.open;
            this.m_btBrowseTarget.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btBrowseTarget.Location = new System.Drawing.Point(504, 19);
            this.m_btBrowseTarget.Name = "m_btBrowseTarget";
            this.m_btBrowseTarget.Size = new System.Drawing.Size(40, 40);
            this.m_btBrowseTarget.TabIndex = 2;
            this.m_btBrowseTarget.UseVisualStyleBackColor = true;
            // 
            // m_cbOverwrite
            // 
            this.m_cbOverwrite.AutoSize = true;
            this.m_cbOverwrite.Checked = true;
            this.m_cbOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_cbOverwrite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_cbOverwrite.Location = new System.Drawing.Point(16, 237);
            this.m_cbOverwrite.Name = "m_cbOverwrite";
            this.m_cbOverwrite.Size = new System.Drawing.Size(214, 17);
            this.m_cbOverwrite.TabIndex = 9;
            this.m_cbOverwrite.Text = "Overwrite existing files in target directory";
            this.m_cbOverwrite.UseVisualStyleBackColor = true;
            // 
            // m_tbPass
            // 
            this.m_tbPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbPass.Location = new System.Drawing.Point(103, 79);
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.Size = new System.Drawing.Size(172, 20);
            this.m_tbPass.TabIndex = 7;
            this.m_tbPass.UseSystemPasswordChar = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(13, 82);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Password:";
            // 
            // m_tbUserName
            // 
            this.m_tbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbUserName.Location = new System.Drawing.Point(103, 53);
            this.m_tbUserName.Name = "m_tbUserName";
            this.m_tbUserName.Size = new System.Drawing.Size(172, 20);
            this.m_tbUserName.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(13, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Username:";
            // 
            // m_checkPathButton
            // 
            this.m_checkPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_checkPathButton.Image = global::iba.Properties.Resources.thumup;
            this.m_checkPathButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_checkPathButton.Location = new System.Drawing.Point(281, 53);
            this.m_checkPathButton.Name = "m_checkPathButton";
            this.m_checkPathButton.Size = new System.Drawing.Size(40, 40);
            this.m_checkPathButton.TabIndex = 5;
            this.m_checkPathButton.UseVisualStyleBackColor = true;
            // 
            // m_subfolderGroupBox
            // 
            this.m_subfolderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_subfolderGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.m_subfolderGroupBox.Location = new System.Drawing.Point(15, 105);
            this.m_subfolderGroupBox.Name = "m_subfolderGroupBox";
            this.m_subfolderGroupBox.Size = new System.Drawing.Size(529, 129);
            this.m_subfolderGroupBox.TabIndex = 8;
            this.m_subfolderGroupBox.TabStop = false;
            this.m_subfolderGroupBox.Text = "Subdirectories";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(523, 110);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(517, 49);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 49F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(517, 49);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.m_rbQuota);
            this.panel2.Controls.Add(this.m_nudQuota);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(261, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(253, 43);
            this.panel2.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(228, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Mb";
            // 
            // m_rbQuota
            // 
            this.m_rbQuota.AutoSize = true;
            this.m_rbQuota.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbQuota.Location = new System.Drawing.Point(3, 14);
            this.m_rbQuota.Name = "m_rbQuota";
            this.m_rbQuota.Size = new System.Drawing.Size(141, 17);
            this.m_rbQuota.TabIndex = 0;
            this.m_rbQuota.TabStop = true;
            this.m_rbQuota.Text = "Limit diskspace usage to";
            this.m_rbQuota.UseVisualStyleBackColor = true;
            // 
            // m_nudQuota
            // 
            this.m_nudQuota.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudQuota.Location = new System.Drawing.Point(144, 14);
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
            this.m_nudQuota.Size = new System.Drawing.Size(78, 20);
            this.m_nudQuota.TabIndex = 1;
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
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(252, 43);
            this.panel3.TabIndex = 0;
            // 
            // m_rbLimitDirectories
            // 
            this.m_rbLimitDirectories.AutoSize = true;
            this.m_rbLimitDirectories.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbLimitDirectories.Location = new System.Drawing.Point(3, 12);
            this.m_rbLimitDirectories.Name = "m_rbLimitDirectories";
            this.m_rbLimitDirectories.Size = new System.Drawing.Size(126, 17);
            this.m_rbLimitDirectories.TabIndex = 0;
            this.m_rbLimitDirectories.TabStop = true;
            this.m_rbLimitDirectories.Text = "Limit subdirectories to";
            this.m_rbLimitDirectories.UseVisualStyleBackColor = true;
            // 
            // m_nudDirs
            // 
            this.m_nudDirs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudDirs.Location = new System.Drawing.Point(135, 12);
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
            this.m_nudDirs.Size = new System.Drawing.Size(78, 20);
            this.m_nudDirs.TabIndex = 1;
            this.m_nudDirs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel3.Controls.Add(this.m_rbOriginal, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_rbNONE, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_rbHour, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbDay, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbWeek, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbMonth, 0, 1);
            this.tableLayoutPanel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel3.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 2;
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel3.Size = new System.Drawing.Size(517, 49);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // m_rbOriginal
            // 
            this.m_rbOriginal.AutoSize = true;
            this.m_rbOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbOriginal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbOriginal.Location = new System.Drawing.Point(173, 27);
            this.m_rbOriginal.Name = "m_rbOriginal";
            this.m_rbOriginal.Size = new System.Drawing.Size(164, 19);
            this.m_rbOriginal.TabIndex = 4;
            this.m_rbOriginal.TabStop = true;
            this.m_rbOriginal.Text = "Keep original structure";
            this.m_rbOriginal.UseVisualStyleBackColor = true;
            // 
            // m_rbNONE
            // 
            this.m_rbNONE.AutoSize = true;
            this.m_rbNONE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbNONE.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbNONE.Location = new System.Drawing.Point(343, 27);
            this.m_rbNONE.Name = "m_rbNONE";
            this.m_rbNONE.Size = new System.Drawing.Size(171, 19);
            this.m_rbNONE.TabIndex = 5;
            this.m_rbNONE.TabStop = true;
            this.m_rbNONE.Text = "None";
            this.m_rbNONE.UseVisualStyleBackColor = true;
            // 
            // m_rbHour
            // 
            this.m_rbHour.AutoSize = true;
            this.m_rbHour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbHour.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbHour.Location = new System.Drawing.Point(3, 3);
            this.m_rbHour.Name = "m_rbHour";
            this.m_rbHour.Size = new System.Drawing.Size(164, 18);
            this.m_rbHour.TabIndex = 0;
            this.m_rbHour.TabStop = true;
            this.m_rbHour.Text = "Each hour";
            this.m_rbHour.UseVisualStyleBackColor = true;
            // 
            // m_rbDay
            // 
            this.m_rbDay.AutoSize = true;
            this.m_rbDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbDay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbDay.Location = new System.Drawing.Point(173, 3);
            this.m_rbDay.Name = "m_rbDay";
            this.m_rbDay.Size = new System.Drawing.Size(164, 18);
            this.m_rbDay.TabIndex = 1;
            this.m_rbDay.TabStop = true;
            this.m_rbDay.Text = "Each day";
            this.m_rbDay.UseVisualStyleBackColor = true;
            // 
            // m_rbWeek
            // 
            this.m_rbWeek.AutoSize = true;
            this.m_rbWeek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbWeek.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbWeek.Location = new System.Drawing.Point(343, 3);
            this.m_rbWeek.Name = "m_rbWeek";
            this.m_rbWeek.Size = new System.Drawing.Size(171, 18);
            this.m_rbWeek.TabIndex = 2;
            this.m_rbWeek.TabStop = true;
            this.m_rbWeek.Text = "Each week";
            this.m_rbWeek.UseVisualStyleBackColor = true;
            // 
            // m_rbMonth
            // 
            this.m_rbMonth.AutoSize = true;
            this.m_rbMonth.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbMonth.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbMonth.Location = new System.Drawing.Point(3, 27);
            this.m_rbMonth.Name = "m_rbMonth";
            this.m_rbMonth.Size = new System.Drawing.Size(164, 19);
            this.m_rbMonth.TabIndex = 3;
            this.m_rbMonth.TabStop = true;
            this.m_rbMonth.Text = "Each month";
            this.m_rbMonth.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(13, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Target directory:";
            // 
            // m_targetFolderTextBox
            // 
            this.m_targetFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_targetFolderTextBox.Location = new System.Drawing.Point(103, 27);
            this.m_targetFolderTextBox.Name = "m_targetFolderTextBox";
            this.m_targetFolderTextBox.Size = new System.Drawing.Size(395, 20);
            this.m_targetFolderTextBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.m_panelAuth);
            this.groupBox1.Controls.Add(this.m_btTestConnection);
            this.groupBox1.Controls.Add(this.m_tbTableName);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.m_computer);
            this.groupBox1.Controls.Add(this.m_tbDatabaseName);
            this.groupBox1.Controls.Add(this.m_cbProvider);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(0, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(566, 226);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Database info";
            // 
            // m_panelAuth
            // 
            this.m_panelAuth.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_panelAuth.Controls.Add(this.label8);
            this.m_panelAuth.Controls.Add(this.m_tbDbPass);
            this.m_panelAuth.Controls.Add(this.m_rbNT);
            this.m_panelAuth.Controls.Add(this.label6);
            this.m_panelAuth.Controls.Add(this.m_rbOtherAuth);
            this.m_panelAuth.Controls.Add(this.m_tbDbUsername);
            this.m_panelAuth.Controls.Add(this.label7);
            this.m_panelAuth.Location = new System.Drawing.Point(3, 81);
            this.m_panelAuth.Name = "m_panelAuth";
            this.m_panelAuth.Size = new System.Drawing.Size(337, 102);
            this.m_panelAuth.TabIndex = 5;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(9, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(78, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "Authentication:";
            // 
            // m_tbDbPass
            // 
            this.m_tbDbPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbDbPass.Location = new System.Drawing.Point(191, 78);
            this.m_tbDbPass.Name = "m_tbDbPass";
            this.m_tbDbPass.Size = new System.Drawing.Size(131, 20);
            this.m_tbDbPass.TabIndex = 6;
            this.m_tbDbPass.UseSystemPasswordChar = true;
            // 
            // m_rbNT
            // 
            this.m_rbNT.AutoSize = true;
            this.m_rbNT.Location = new System.Drawing.Point(100, 6);
            this.m_rbNT.Name = "m_rbNT";
            this.m_rbNT.Size = new System.Drawing.Size(179, 17);
            this.m_rbNT.TabIndex = 1;
            this.m_rbNT.TabStop = true;
            this.m_rbNT.Text = "Use Windows NT authentication";
            this.m_rbNT.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label6.Location = new System.Drawing.Point(129, 81);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 5;
            this.label6.Text = "Password:";
            // 
            // m_rbOtherAuth
            // 
            this.m_rbOtherAuth.AutoSize = true;
            this.m_rbOtherAuth.Location = new System.Drawing.Point(100, 29);
            this.m_rbOtherAuth.Name = "m_rbOtherAuth";
            this.m_rbOtherAuth.Size = new System.Drawing.Size(150, 17);
            this.m_rbOtherAuth.TabIndex = 2;
            this.m_rbOtherAuth.TabStop = true;
            this.m_rbOtherAuth.Text = "Specify authentication info";
            this.m_rbOtherAuth.UseVisualStyleBackColor = true;
            // 
            // m_tbDbUsername
            // 
            this.m_tbDbUsername.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbDbUsername.Location = new System.Drawing.Point(191, 52);
            this.m_tbDbUsername.Name = "m_tbDbUsername";
            this.m_tbDbUsername.Size = new System.Drawing.Size(131, 20);
            this.m_tbDbUsername.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(127, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(58, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Username:";
            // 
            // m_btTestConnection
            // 
            this.m_btTestConnection.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btTestConnection.Location = new System.Drawing.Point(364, 117);
            this.m_btTestConnection.Name = "m_btTestConnection";
            this.m_btTestConnection.Size = new System.Drawing.Size(154, 32);
            this.m_btTestConnection.TabIndex = 6;
            this.m_btTestConnection.Text = "Test database connection";
            this.m_btTestConnection.UseVisualStyleBackColor = true;
            // 
            // m_tbTableName
            // 
            this.m_tbTableName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbTableName.Location = new System.Drawing.Point(103, 193);
            this.m_tbTableName.Name = "m_tbTableName";
            this.m_tbTableName.Size = new System.Drawing.Size(202, 20);
            this.m_tbTableName.TabIndex = 8;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 196);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 13);
            this.label9.TabIndex = 7;
            this.label9.Text = "Table name:";
            // 
            // m_computer
            // 
            this.m_computer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_computer.Controls.Add(this.m_btBrowseServer);
            this.m_computer.Controls.Add(this.m_tbServer);
            this.m_computer.Controls.Add(this.m_rbServer);
            this.m_computer.Controls.Add(this.m_rbLocal);
            this.m_computer.Controls.Add(this.label10);
            this.m_computer.Location = new System.Drawing.Point(253, 16);
            this.m_computer.Name = "m_computer";
            this.m_computer.Size = new System.Drawing.Size(307, 88);
            this.m_computer.TabIndex = 2;
            // 
            // m_btBrowseServer
            // 
            this.m_btBrowseServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btBrowseServer.Image = global::iba.Properties.Resources.open;
            this.m_btBrowseServer.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btBrowseServer.Location = new System.Drawing.Point(257, 42);
            this.m_btBrowseServer.Name = "m_btBrowseServer";
            this.m_btBrowseServer.Size = new System.Drawing.Size(40, 40);
            this.m_btBrowseServer.TabIndex = 4;
            this.m_btBrowseServer.UseVisualStyleBackColor = true;
            // 
            // m_tbServer
            // 
            this.m_tbServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbServer.Location = new System.Drawing.Point(93, 53);
            this.m_tbServer.Name = "m_tbServer";
            this.m_tbServer.Size = new System.Drawing.Size(158, 20);
            this.m_tbServer.TabIndex = 3;
            // 
            // m_rbServer
            // 
            this.m_rbServer.AutoSize = true;
            this.m_rbServer.Location = new System.Drawing.Point(64, 30);
            this.m_rbServer.Name = "m_rbServer";
            this.m_rbServer.Size = new System.Drawing.Size(106, 17);
            this.m_rbServer.TabIndex = 2;
            this.m_rbServer.TabStop = true;
            this.m_rbServer.Text = "Database server:";
            this.m_rbServer.UseVisualStyleBackColor = true;
            // 
            // m_rbLocal
            // 
            this.m_rbLocal.AutoSize = true;
            this.m_rbLocal.Location = new System.Drawing.Point(64, 7);
            this.m_rbLocal.Name = "m_rbLocal";
            this.m_rbLocal.Size = new System.Drawing.Size(94, 17);
            this.m_rbLocal.TabIndex = 1;
            this.m_rbLocal.TabStop = true;
            this.m_rbLocal.Text = "Local machine";
            this.m_rbLocal.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Computer:";
            // 
            // m_tbDatabaseName
            // 
            this.m_tbDatabaseName.Location = new System.Drawing.Point(115, 49);
            this.m_tbDatabaseName.Name = "m_tbDatabaseName";
            this.m_tbDatabaseName.Size = new System.Drawing.Size(135, 20);
            this.m_tbDatabaseName.TabIndex = 4;
            // 
            // m_cbProvider
            // 
            this.m_cbProvider.FormattingEnabled = true;
            this.m_cbProvider.Items.AddRange(new object[] {
            "Sql-server",
            "ODBC-database",
            "Oracle",
            "DB2-UDB"});
            this.m_cbProvider.Location = new System.Drawing.Point(115, 22);
            this.m_cbProvider.Name = "m_cbProvider";
            this.m_cbProvider.Size = new System.Drawing.Size(135, 21);
            this.m_cbProvider.TabIndex = 1;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 52);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Database name:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(97, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Database provider:";
            // 
            // UpdateDataTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.m_gbTarget);
            this.Name = "UpdateDataTaskControl";
            this.Size = new System.Drawing.Size(566, 504);
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
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.GroupBox m_gbTarget;
        private System.Windows.Forms.CheckBox m_cbOverwrite;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button m_checkPathButton;
        private System.Windows.Forms.GroupBox m_subfolderGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton m_rbQuota;
        private System.Windows.Forms.NumericUpDown m_nudQuota;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.RadioButton m_rbLimitDirectories;
        private System.Windows.Forms.NumericUpDown m_nudDirs;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton m_rbOriginal;
        private System.Windows.Forms.RadioButton m_rbNONE;
        private System.Windows.Forms.RadioButton m_rbHour;
        private System.Windows.Forms.RadioButton m_rbDay;
        private System.Windows.Forms.RadioButton m_rbWeek;
        private System.Windows.Forms.RadioButton m_rbMonth;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_targetFolderTextBox;
        private System.Windows.Forms.FolderBrowserDialog m_serverBrowserDialog;
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
    }
}
