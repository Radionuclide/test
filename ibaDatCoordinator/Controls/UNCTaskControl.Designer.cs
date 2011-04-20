namespace iba.Controls
{
    partial class UNCTaskControl
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
            this.m_subfolderGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbOriginal = new System.Windows.Forms.RadioButton();
            this.m_rbNONE = new System.Windows.Forms.RadioButton();
            this.m_rbHour = new System.Windows.Forms.RadioButton();
            this.m_rbDay = new System.Windows.Forms.RadioButton();
            this.m_rbWeek = new System.Windows.Forms.RadioButton();
            this.m_rbMonth = new System.Windows.Forms.RadioButton();
            this.panel3 = new System.Windows.Forms.Panel();
            this.m_cbTakeDatTime = new System.Windows.Forms.CheckBox();
            this.m_cleanupGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.m_rbLimitDirectories = new System.Windows.Forms.RadioButton();
            this.m_nudDirs = new System.Windows.Forms.NumericUpDown();
            this.panel5 = new System.Windows.Forms.Panel();
            this.label2 = new System.Windows.Forms.Label();
            this.m_rbQuota = new System.Windows.Forms.RadioButton();
            this.m_nudQuota = new System.Windows.Forms.NumericUpDown();
            this.panel6 = new System.Windows.Forms.Panel();
            this.panel7 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.m_cbOverwrite = new System.Windows.Forms.CheckBox();
            this.m_tbPass = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.m_tbUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_browseFolderButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_targetFolderTextBox = new System.Windows.Forms.TextBox();
            this.m_checkPathButton = new System.Windows.Forms.Button();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.m_subfolderGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel3.SuspendLayout();
            this.m_cleanupGroupBox.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).BeginInit();
            this.panel5.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).BeginInit();
            this.panel6.SuspendLayout();
            this.panel7.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.SuspendLayout();
            // 
            // m_subfolderGroupBox
            // 
            this.m_subfolderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_subfolderGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.m_subfolderGroupBox.Location = new System.Drawing.Point(0, 105);
            this.m_subfolderGroupBox.Name = "m_subfolderGroupBox";
            this.m_subfolderGroupBox.Size = new System.Drawing.Size(535, 98);
            this.m_subfolderGroupBox.TabIndex = 10;
            this.m_subfolderGroupBox.TabStop = false;
            this.m_subfolderGroupBox.Text = "Subdirectories";
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 1;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 1);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(529, 79);
            this.tableLayoutPanel2.TabIndex = 0;
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
            this.tableLayoutPanel3.Size = new System.Drawing.Size(523, 46);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // m_rbOriginal
            // 
            this.m_rbOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbOriginal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbOriginal.Location = new System.Drawing.Point(175, 26);
            this.m_rbOriginal.Name = "m_rbOriginal";
            this.m_rbOriginal.Size = new System.Drawing.Size(166, 17);
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
            this.m_rbNONE.Location = new System.Drawing.Point(347, 26);
            this.m_rbNONE.Name = "m_rbNONE";
            this.m_rbNONE.Size = new System.Drawing.Size(173, 17);
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
            this.m_rbHour.Size = new System.Drawing.Size(166, 17);
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
            this.m_rbDay.Location = new System.Drawing.Point(175, 3);
            this.m_rbDay.Name = "m_rbDay";
            this.m_rbDay.Size = new System.Drawing.Size(166, 17);
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
            this.m_rbWeek.Location = new System.Drawing.Point(347, 3);
            this.m_rbWeek.Name = "m_rbWeek";
            this.m_rbWeek.Size = new System.Drawing.Size(173, 17);
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
            this.m_rbMonth.Location = new System.Drawing.Point(3, 26);
            this.m_rbMonth.Name = "m_rbMonth";
            this.m_rbMonth.Size = new System.Drawing.Size(166, 17);
            this.m_rbMonth.TabIndex = 3;
            this.m_rbMonth.TabStop = true;
            this.m_rbMonth.Text = "Each month";
            this.m_rbMonth.UseVisualStyleBackColor = true;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.m_cbTakeDatTime);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(3, 55);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(523, 21);
            this.panel3.TabIndex = 2;
            // 
            // m_cbTakeDatTime
            // 
            this.m_cbTakeDatTime.AutoSize = true;
            this.m_cbTakeDatTime.Checked = true;
            this.m_cbTakeDatTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_cbTakeDatTime.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_cbTakeDatTime.Location = new System.Drawing.Point(3, 1);
            this.m_cbTakeDatTime.Name = "m_cbTakeDatTime";
            this.m_cbTakeDatTime.Size = new System.Drawing.Size(290, 17);
            this.m_cbTakeDatTime.TabIndex = 11;
            this.m_cbTakeDatTime.Text = "Use last modification time of .dat file to create directories";
            this.m_cbTakeDatTime.UseVisualStyleBackColor = true;
            // 
            // m_cleanupGroupBox
            // 
            this.m_cleanupGroupBox.Controls.Add(this.tableLayoutPanel4);
            this.m_cleanupGroupBox.Location = new System.Drawing.Point(0, 209);
            this.m_cleanupGroupBox.Name = "m_cleanupGroupBox";
            this.m_cleanupGroupBox.Size = new System.Drawing.Size(535, 82);
            this.m_cleanupGroupBox.TabIndex = 11;
            this.m_cleanupGroupBox.TabStop = false;
            this.m_cleanupGroupBox.Text = "Cleanup strategy";
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 2;
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.panel5, 0, 1);
            this.tableLayoutPanel4.Controls.Add(this.panel6, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.panel7, 1, 1);
            this.tableLayoutPanel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel4.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 2;
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel4.Size = new System.Drawing.Size(529, 63);
            this.tableLayoutPanel4.TabIndex = 0;
            // 
            // panel4
            // 
            this.panel4.Controls.Add(this.m_rbLimitDirectories);
            this.panel4.Controls.Add(this.m_nudDirs);
            this.panel4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel4.Location = new System.Drawing.Point(267, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(259, 25);
            this.panel4.TabIndex = 6;
            // 
            // m_rbLimitDirectories
            // 
            this.m_rbLimitDirectories.AutoSize = true;
            this.m_rbLimitDirectories.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbLimitDirectories.Location = new System.Drawing.Point(6, 3);
            this.m_rbLimitDirectories.Name = "m_rbLimitDirectories";
            this.m_rbLimitDirectories.Size = new System.Drawing.Size(126, 17);
            this.m_rbLimitDirectories.TabIndex = 5;
            this.m_rbLimitDirectories.TabStop = true;
            this.m_rbLimitDirectories.Text = "Limit subdirectories to";
            this.m_rbLimitDirectories.UseVisualStyleBackColor = true;
            // 
            // m_nudDirs
            // 
            this.m_nudDirs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudDirs.Location = new System.Drawing.Point(147, 3);
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
            this.m_nudDirs.TabIndex = 4;
            this.m_nudDirs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // panel5
            // 
            this.panel5.Controls.Add(this.label2);
            this.panel5.Controls.Add(this.m_rbQuota);
            this.panel5.Controls.Add(this.m_nudQuota);
            this.panel5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel5.Location = new System.Drawing.Point(3, 34);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(258, 26);
            this.panel5.TabIndex = 7;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(231, 5);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Mb";
            // 
            // m_rbQuota
            // 
            this.m_rbQuota.AutoSize = true;
            this.m_rbQuota.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbQuota.Location = new System.Drawing.Point(6, 3);
            this.m_rbQuota.Name = "m_rbQuota";
            this.m_rbQuota.Size = new System.Drawing.Size(141, 17);
            this.m_rbQuota.TabIndex = 6;
            this.m_rbQuota.TabStop = true;
            this.m_rbQuota.Text = "Limit diskspace usage to";
            this.m_rbQuota.UseVisualStyleBackColor = true;
            // 
            // m_nudQuota
            // 
            this.m_nudQuota.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudQuota.Location = new System.Drawing.Point(147, 3);
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
            this.m_nudQuota.TabIndex = 5;
            this.m_nudQuota.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // panel6
            // 
            this.panel6.Controls.Add(this.radioButton2);
            this.panel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel6.Location = new System.Drawing.Point(3, 3);
            this.panel6.Name = "panel6";
            this.panel6.Size = new System.Drawing.Size(258, 25);
            this.panel6.TabIndex = 8;
            // 
            // panel7
            // 
            this.panel7.Controls.Add(this.label1);
            this.panel7.Controls.Add(this.radioButton1);
            this.panel7.Controls.Add(this.numericUpDown1);
            this.panel7.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel7.Location = new System.Drawing.Point(267, 34);
            this.panel7.Name = "panel7";
            this.panel7.Size = new System.Drawing.Size(259, 26);
            this.panel7.TabIndex = 9;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(231, 5);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(22, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Mb";
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButton1.Location = new System.Drawing.Point(6, 3);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(135, 17);
            this.radioButton1.TabIndex = 9;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Minimal free disc space";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.numericUpDown1.Location = new System.Drawing.Point(147, 3);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(78, 20);
            this.numericUpDown1.TabIndex = 8;
            this.numericUpDown1.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // radioButton2
            // 
            this.radioButton2.AutoSize = true;
            this.radioButton2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.radioButton2.Location = new System.Drawing.Point(6, 3);
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.Size = new System.Drawing.Size(51, 17);
            this.radioButton2.TabIndex = 6;
            this.radioButton2.TabStop = true;
            this.radioButton2.Text = "None";
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // m_cbOverwrite
            // 
            this.m_cbOverwrite.AutoSize = true;
            this.m_cbOverwrite.Checked = true;
            this.m_cbOverwrite.CheckState = System.Windows.Forms.CheckState.Checked;
            this.m_cbOverwrite.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_cbOverwrite.Location = new System.Drawing.Point(6, 297);
            this.m_cbOverwrite.Name = "m_cbOverwrite";
            this.m_cbOverwrite.Size = new System.Drawing.Size(214, 17);
            this.m_cbOverwrite.TabIndex = 12;
            this.m_cbOverwrite.Text = "Overwrite existing files in target directory";
            this.m_cbOverwrite.UseVisualStyleBackColor = true;
            // 
            // m_tbPass
            // 
            this.m_tbPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbPass.Location = new System.Drawing.Point(96, 75);
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.Size = new System.Drawing.Size(176, 20);
            this.m_tbPass.TabIndex = 19;
            this.m_tbPass.UseSystemPasswordChar = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(9, 78);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Password:";
            // 
            // m_tbUserName
            // 
            this.m_tbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbUserName.Location = new System.Drawing.Point(96, 49);
            this.m_tbUserName.Name = "m_tbUserName";
            this.m_tbUserName.Size = new System.Drawing.Size(176, 20);
            this.m_tbUserName.TabIndex = 17;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(9, 52);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Username:";
            // 
            // m_browseFolderButton
            // 
            this.m_browseFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseFolderButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFolderButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseFolderButton.Location = new System.Drawing.Point(457, 12);
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.Size = new System.Drawing.Size(40, 40);
            this.m_browseFolderButton.TabIndex = 15;
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            this.m_browseFolderButton.Click += new System.EventHandler(this.m_browseFolderButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(6, 24);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Target directory:";
            // 
            // m_targetFolderTextBox
            // 
            this.m_targetFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_targetFolderTextBox.Location = new System.Drawing.Point(96, 21);
            this.m_targetFolderTextBox.Name = "m_targetFolderTextBox";
            this.m_targetFolderTextBox.Size = new System.Drawing.Size(355, 20);
            this.m_targetFolderTextBox.TabIndex = 14;
            // 
            // m_checkPathButton
            // 
            this.m_checkPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_checkPathButton.Image = global::iba.Properties.Resources.thumup;
            this.m_checkPathButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_checkPathButton.Location = new System.Drawing.Point(278, 51);
            this.m_checkPathButton.Name = "m_checkPathButton";
            this.m_checkPathButton.Size = new System.Drawing.Size(40, 40);
            this.m_checkPathButton.TabIndex = 20;
            this.m_checkPathButton.UseVisualStyleBackColor = true;
            this.m_checkPathButton.Click += new System.EventHandler(this.m_checkPathButton_Click);
            // 
            // UNCTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_checkPathButton);
            this.Controls.Add(this.m_tbPass);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.m_tbUserName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_browseFolderButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_targetFolderTextBox);
            this.Controls.Add(this.m_cbOverwrite);
            this.Controls.Add(this.m_cleanupGroupBox);
            this.Controls.Add(this.m_subfolderGroupBox);
            this.Name = "UNCTaskControl";
            this.Size = new System.Drawing.Size(535, 326);
            this.m_subfolderGroupBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.m_cleanupGroupBox.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).EndInit();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).EndInit();
            this.panel6.ResumeLayout(false);
            this.panel6.PerformLayout();
            this.panel7.ResumeLayout(false);
            this.panel7.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox m_subfolderGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton m_rbOriginal;
        private System.Windows.Forms.RadioButton m_rbNONE;
        private System.Windows.Forms.RadioButton m_rbHour;
        private System.Windows.Forms.RadioButton m_rbDay;
        private System.Windows.Forms.RadioButton m_rbWeek;
        private System.Windows.Forms.RadioButton m_rbMonth;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox m_cbTakeDatTime;
        private System.Windows.Forms.GroupBox m_cleanupGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.RadioButton m_rbLimitDirectories;
        private System.Windows.Forms.NumericUpDown m_nudDirs;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton m_rbQuota;
        private System.Windows.Forms.NumericUpDown m_nudQuota;
        private System.Windows.Forms.Panel panel6;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.Panel panel7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.CheckBox m_cbOverwrite;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_targetFolderTextBox;
        private System.Windows.Forms.Button m_checkPathButton;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
    }
}
