namespace iba.Controls
{
    partial class CleanupTaskControl
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
            if(disposing && (components != null))
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
            this.m_checkPathButton = new System.Windows.Forms.Button();
            this.m_tbPass = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.m_tbUserName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_browseFolderButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_targetFolderTextBox = new System.Windows.Forms.TextBox();
            this.m_tbExtension = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_cleanupGroupBox = new System.Windows.Forms.GroupBox();
            this.m_rbLimitDirectories = new System.Windows.Forms.RadioButton();
            this.m_nudDirs = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.m_rbQuota = new System.Windows.Forms.RadioButton();
            this.m_nudQuota = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.m_rbLimitFree = new System.Windows.Forms.RadioButton();
            this.m_nudFree = new System.Windows.Forms.NumericUpDown();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_cleanupGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudFree)).BeginInit();
            this.SuspendLayout();
            // 
            // m_checkPathButton
            // 
            this.m_checkPathButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_checkPathButton.Image = global::iba.Properties.Resources.thumup;
            this.m_checkPathButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_checkPathButton.Location = new System.Drawing.Point(392, 45);
            this.m_checkPathButton.Name = "m_checkPathButton";
            this.m_checkPathButton.Size = new System.Drawing.Size(40, 40);
            this.m_checkPathButton.TabIndex = 15;
            this.m_checkPathButton.UseVisualStyleBackColor = true;
            this.m_checkPathButton.Click += new System.EventHandler(this.m_checkPathButton_Click);
            // 
            // m_tbPass
            // 
            this.m_tbPass.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbPass.Location = new System.Drawing.Point(110, 69);
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.Size = new System.Drawing.Size(276, 20);
            this.m_tbPass.TabIndex = 14;
            this.m_tbPass.UseSystemPasswordChar = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(14, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(56, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Password:";
            // 
            // m_tbUserName
            // 
            this.m_tbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbUserName.Location = new System.Drawing.Point(110, 43);
            this.m_tbUserName.Name = "m_tbUserName";
            this.m_tbUserName.Size = new System.Drawing.Size(276, 20);
            this.m_tbUserName.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(12, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(58, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Username:";
            // 
            // m_browseFolderButton
            // 
            this.m_browseFolderButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseFolderButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFolderButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseFolderButton.Location = new System.Drawing.Point(612, 6);
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.Size = new System.Drawing.Size(40, 40);
            this.m_browseFolderButton.TabIndex = 10;
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            this.m_browseFolderButton.Click += new System.EventHandler(this.m_browseFolderButton_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(12, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(92, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Cleanup directory:";
            // 
            // m_targetFolderTextBox
            // 
            this.m_targetFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_targetFolderTextBox.Location = new System.Drawing.Point(110, 17);
            this.m_targetFolderTextBox.Name = "m_targetFolderTextBox";
            this.m_targetFolderTextBox.Size = new System.Drawing.Size(496, 20);
            this.m_targetFolderTextBox.TabIndex = 9;
            // 
            // m_tbExtension
            // 
            this.m_tbExtension.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbExtension.Location = new System.Drawing.Point(112, 111);
            this.m_tbExtension.Name = "m_tbExtension";
            this.m_tbExtension.Size = new System.Drawing.Size(135, 20);
            this.m_tbExtension.TabIndex = 17;
            this.m_toolTip.SetToolTip(this.m_tbExtension, "To cleanup several types of files, specify the extensions separated by \';\'\r\ne.g.:" +
        " \".dat;.txt;.csv\"");
            this.m_tbExtension.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(14, 114);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "File extension:";
            // 
            // m_cleanupGroupBox
            // 
            this.m_cleanupGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_cleanupGroupBox.Controls.Add(this.label5);
            this.m_cleanupGroupBox.Controls.Add(this.m_rbLimitFree);
            this.m_cleanupGroupBox.Controls.Add(this.m_nudFree);
            this.m_cleanupGroupBox.Controls.Add(this.label2);
            this.m_cleanupGroupBox.Controls.Add(this.m_rbQuota);
            this.m_cleanupGroupBox.Controls.Add(this.m_nudQuota);
            this.m_cleanupGroupBox.Controls.Add(this.m_rbLimitDirectories);
            this.m_cleanupGroupBox.Controls.Add(this.m_nudDirs);
            this.m_cleanupGroupBox.Location = new System.Drawing.Point(15, 137);
            this.m_cleanupGroupBox.Name = "m_cleanupGroupBox";
            this.m_cleanupGroupBox.Size = new System.Drawing.Size(635, 95);
            this.m_cleanupGroupBox.TabIndex = 18;
            this.m_cleanupGroupBox.TabStop = false;
            this.m_cleanupGroupBox.Text = "Cleanup strategy";
            // 
            // m_rbLimitDirectories
            // 
            this.m_rbLimitDirectories.AutoSize = true;
            this.m_rbLimitDirectories.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbLimitDirectories.Location = new System.Drawing.Point(6, 19);
            this.m_rbLimitDirectories.Name = "m_rbLimitDirectories";
            this.m_rbLimitDirectories.Size = new System.Drawing.Size(126, 17);
            this.m_rbLimitDirectories.TabIndex = 2;
            this.m_rbLimitDirectories.TabStop = true;
            this.m_rbLimitDirectories.Text = "Limit subdirectories to";
            this.m_rbLimitDirectories.UseVisualStyleBackColor = true;
            // 
            // m_nudDirs
            // 
            this.m_nudDirs.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudDirs.Location = new System.Drawing.Point(147, 19);
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
            this.m_nudDirs.TabIndex = 3;
            this.m_nudDirs.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(231, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(22, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Mb";
            // 
            // m_rbQuota
            // 
            this.m_rbQuota.AutoSize = true;
            this.m_rbQuota.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbQuota.Location = new System.Drawing.Point(6, 45);
            this.m_rbQuota.Name = "m_rbQuota";
            this.m_rbQuota.Size = new System.Drawing.Size(141, 17);
            this.m_rbQuota.TabIndex = 4;
            this.m_rbQuota.TabStop = true;
            this.m_rbQuota.Text = "Limit diskspace usage to";
            this.m_rbQuota.UseVisualStyleBackColor = true;
            // 
            // m_nudQuota
            // 
            this.m_nudQuota.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudQuota.Location = new System.Drawing.Point(147, 45);
            this.m_nudQuota.Maximum = new decimal(new int[] {
            1000000000,
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
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label5.Location = new System.Drawing.Point(231, 73);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Mb";
            // 
            // m_rbLimitFree
            // 
            this.m_rbLimitFree.AutoSize = true;
            this.m_rbLimitFree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbLimitFree.Location = new System.Drawing.Point(6, 71);
            this.m_rbLimitFree.Name = "m_rbLimitFree";
            this.m_rbLimitFree.Size = new System.Drawing.Size(135, 17);
            this.m_rbLimitFree.TabIndex = 7;
            this.m_rbLimitFree.TabStop = true;
            this.m_rbLimitFree.Text = "Minimal free disk space";
            this.m_rbLimitFree.UseVisualStyleBackColor = true;
            // 
            // m_nudFree
            // 
            this.m_nudFree.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.m_nudFree.Location = new System.Drawing.Point(147, 71);
            this.m_nudFree.Maximum = new decimal(new int[] {
            1000000000,
            0,
            0,
            0});
            this.m_nudFree.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudFree.Name = "m_nudFree";
            this.m_nudFree.Size = new System.Drawing.Size(78, 20);
            this.m_nudFree.TabIndex = 8;
            this.m_nudFree.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            // 
            // CleanupTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_cleanupGroupBox);
            this.Controls.Add(this.m_tbExtension);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_checkPathButton);
            this.Controls.Add(this.m_tbPass);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.m_tbUserName);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_browseFolderButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_targetFolderTextBox);
            this.Name = "CleanupTaskControl";
            this.Size = new System.Drawing.Size(666, 238);
            this.m_cleanupGroupBox.ResumeLayout(false);
            this.m_cleanupGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudFree)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_checkPathButton;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_targetFolderTextBox;
        private System.Windows.Forms.TextBox m_tbExtension;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox m_cleanupGroupBox;
        private System.Windows.Forms.RadioButton m_rbLimitDirectories;
        private System.Windows.Forms.NumericUpDown m_nudDirs;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton m_rbQuota;
        private System.Windows.Forms.NumericUpDown m_nudQuota;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.RadioButton m_rbLimitFree;
        private System.Windows.Forms.NumericUpDown m_nudFree;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
        private System.Windows.Forms.ToolTip m_toolTip;
    }
}
