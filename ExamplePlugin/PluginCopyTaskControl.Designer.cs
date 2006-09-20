namespace ExamplePlugin
{
    partial class PluginCopyTaskControl
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
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.m_cbRemoveSource = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
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
            this.m_rbOriginal = new System.Windows.Forms.RadioButton();
            this.m_rbNONE = new System.Windows.Forms.RadioButton();
            this.m_rbHour = new System.Windows.Forms.RadioButton();
            this.m_rbDay = new System.Windows.Forms.RadioButton();
            this.m_rbWeek = new System.Windows.Forms.RadioButton();
            this.m_rbMonth = new System.Windows.Forms.RadioButton();
            this.m_browseFolderButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_targetFolderTextBox = new System.Windows.Forms.TextBox();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox6.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.m_subfolderGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_folderNumber)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox6
            // 
            this.groupBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox6.Controls.Add(this.m_cbRemoveSource);
            this.groupBox6.Location = new System.Drawing.Point(0, 0);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.Size = new System.Drawing.Size(565, 63);
            this.groupBox6.TabIndex = 4;
            this.groupBox6.TabStop = false;
            this.groupBox6.Text = "Source";
            // 
            // m_cbRemoveSource
            // 
            this.m_cbRemoveSource.AutoSize = true;
            this.m_cbRemoveSource.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_cbRemoveSource.Location = new System.Drawing.Point(16, 28);
            this.m_cbRemoveSource.Name = "m_cbRemoveSource";
            this.m_cbRemoveSource.Size = new System.Drawing.Size(138, 17);
            this.m_cbRemoveSource.TabIndex = 0;
            this.m_cbRemoveSource.Text = "Remove source .dat file";
            this.m_cbRemoveSource.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.m_tbPass);
            this.groupBox2.Controls.Add(this.label12);
            this.groupBox2.Controls.Add(this.m_tbUserName);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.m_checkPathButton);
            this.groupBox2.Controls.Add(this.m_subfolderGroupBox);
            this.groupBox2.Controls.Add(this.m_browseFolderButton);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.m_targetFolderTextBox);
            this.groupBox2.Location = new System.Drawing.Point(0, 69);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(565, 237);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Target";
            // 
            // m_tbPass
            // 
            this.m_tbPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbPass.Location = new System.Drawing.Point(103, 79);
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.Size = new System.Drawing.Size(175, 20);
            this.m_tbPass.TabIndex = 7;
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
            this.m_tbUserName.Size = new System.Drawing.Size(175, 20);
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
            this.m_checkPathButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_checkPathButton.Location = new System.Drawing.Point(284, 53);
            this.m_checkPathButton.Name = "m_checkPathButton";
            this.m_checkPathButton.Size = new System.Drawing.Size(40, 40);
            this.m_checkPathButton.TabIndex = 5;
            this.m_checkPathButton.UseVisualStyleBackColor = true;
            this.m_checkPathButton.Click += new System.EventHandler(this.m_checkPathButton_Click);
            // 
            // m_subfolderGroupBox
            // 
            this.m_subfolderGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_subfolderGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.m_subfolderGroupBox.Location = new System.Drawing.Point(15, 105);
            this.m_subfolderGroupBox.Name = "m_subfolderGroupBox";
            this.m_subfolderGroupBox.Size = new System.Drawing.Size(532, 129);
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
            this.tableLayoutPanel2.Size = new System.Drawing.Size(526, 110);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_folderNumber);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 58);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(520, 49);
            this.panel1.TabIndex = 1;
            // 
            // m_folderNumber
            // 
            this.m_folderNumber.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_folderNumber.Location = new System.Drawing.Point(133, 12);
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
            this.m_folderNumber.Size = new System.Drawing.Size(78, 20);
            this.m_folderNumber.TabIndex = 1;
            this.m_folderNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(12, 14);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Maximum directories";
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
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
            this.tableLayoutPanel3.Size = new System.Drawing.Size(520, 49);
            this.tableLayoutPanel3.TabIndex = 0;
            // 
            // m_rbOriginal
            // 
            this.m_rbOriginal.AutoSize = true;
            this.m_rbOriginal.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbOriginal.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbOriginal.Location = new System.Drawing.Point(23, 27);
            this.m_rbOriginal.Name = "m_rbOriginal";
            this.m_rbOriginal.Size = new System.Drawing.Size(14, 19);
            this.m_rbOriginal.TabIndex = 5;
            this.m_rbOriginal.TabStop = true;
            this.m_rbOriginal.Text = "Keep original structure";
            this.m_rbOriginal.UseVisualStyleBackColor = true;
            // 
            // m_rbNONE
            // 
            this.m_rbNONE.AutoSize = true;
            this.m_rbNONE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbNONE.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbNONE.Location = new System.Drawing.Point(43, 27);
            this.m_rbNONE.Name = "m_rbNONE";
            this.m_rbNONE.Size = new System.Drawing.Size(474, 19);
            this.m_rbNONE.TabIndex = 0;
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
            this.m_rbHour.Size = new System.Drawing.Size(14, 18);
            this.m_rbHour.TabIndex = 1;
            this.m_rbHour.TabStop = true;
            this.m_rbHour.Text = "Each hour";
            this.m_rbHour.UseVisualStyleBackColor = true;
            // 
            // m_rbDay
            // 
            this.m_rbDay.AutoSize = true;
            this.m_rbDay.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbDay.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbDay.Location = new System.Drawing.Point(23, 3);
            this.m_rbDay.Name = "m_rbDay";
            this.m_rbDay.Size = new System.Drawing.Size(14, 18);
            this.m_rbDay.TabIndex = 2;
            this.m_rbDay.TabStop = true;
            this.m_rbDay.Text = "Each day";
            this.m_rbDay.UseVisualStyleBackColor = true;
            // 
            // m_rbWeek
            // 
            this.m_rbWeek.AutoSize = true;
            this.m_rbWeek.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbWeek.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbWeek.Location = new System.Drawing.Point(43, 3);
            this.m_rbWeek.Name = "m_rbWeek";
            this.m_rbWeek.Size = new System.Drawing.Size(474, 18);
            this.m_rbWeek.TabIndex = 3;
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
            this.m_rbMonth.Size = new System.Drawing.Size(14, 19);
            this.m_rbMonth.TabIndex = 4;
            this.m_rbMonth.TabStop = true;
            this.m_rbMonth.Text = "Each month";
            this.m_rbMonth.UseVisualStyleBackColor = true;
            // 
            // m_browseFolderButton
            // 
            this.m_browseFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseFolderButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseFolderButton.Location = new System.Drawing.Point(507, 16);
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.Size = new System.Drawing.Size(40, 40);
            this.m_browseFolderButton.TabIndex = 2;
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            this.m_browseFolderButton.Click += new System.EventHandler(this.m_browseFolderButton_Click);
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
            this.m_targetFolderTextBox.Size = new System.Drawing.Size(398, 20);
            this.m_targetFolderTextBox.TabIndex = 1;
            // 
            // PluginCopyTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox2);
            this.MinimumSize = new System.Drawing.Size(565, 307);
            this.Name = "PluginCopyTaskControl";
            this.Size = new System.Drawing.Size(565, 307);
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.m_subfolderGroupBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_folderNumber)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox m_cbRemoveSource;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button m_checkPathButton;
        private System.Windows.Forms.GroupBox m_subfolderGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown m_folderNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton m_rbOriginal;
        private System.Windows.Forms.RadioButton m_rbNONE;
        private System.Windows.Forms.RadioButton m_rbHour;
        private System.Windows.Forms.RadioButton m_rbDay;
        private System.Windows.Forms.RadioButton m_rbWeek;
        private System.Windows.Forms.RadioButton m_rbMonth;
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_targetFolderTextBox;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
    }
}
