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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CleanupTaskControl));
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
            this.label5 = new System.Windows.Forms.Label();
            this.m_rbLimitFree = new System.Windows.Forms.RadioButton();
            this.m_nudFree = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.m_rbQuota = new System.Windows.Forms.RadioButton();
            this.m_nudQuota = new System.Windows.Forms.NumericUpDown();
            this.m_rbLimitDirectories = new System.Windows.Forms.RadioButton();
            this.m_nudDirs = new System.Windows.Forms.NumericUpDown();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_cleanupGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudFree)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).BeginInit();
            this.SuspendLayout();
            // 
            // m_checkPathButton
            // 
            resources.ApplyResources(this.m_checkPathButton, "m_checkPathButton");
            this.m_checkPathButton.Image = global::iba.Properties.Resources.thumup;
            this.m_checkPathButton.Name = "m_checkPathButton";
            this.m_checkPathButton.UseVisualStyleBackColor = true;
            this.m_checkPathButton.Click += new System.EventHandler(this.m_checkPathButton_Click);
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
            // m_browseFolderButton
            // 
            resources.ApplyResources(this.m_browseFolderButton, "m_browseFolderButton");
            this.m_browseFolderButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            this.m_browseFolderButton.Click += new System.EventHandler(this.m_browseFolderButton_Click);
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
            this.m_targetFolderTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.m_targetFolderTextBox_Validating);
            // 
            // m_tbExtension
            // 
            resources.ApplyResources(this.m_tbExtension, "m_tbExtension");
            this.m_tbExtension.Name = "m_tbExtension";
            this.m_toolTip.SetToolTip(this.m_tbExtension, resources.GetString("m_tbExtension.ToolTip"));
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_cleanupGroupBox
            // 
            resources.ApplyResources(this.m_cleanupGroupBox, "m_cleanupGroupBox");
            this.m_cleanupGroupBox.Controls.Add(this.label5);
            this.m_cleanupGroupBox.Controls.Add(this.m_rbLimitFree);
            this.m_cleanupGroupBox.Controls.Add(this.m_nudFree);
            this.m_cleanupGroupBox.Controls.Add(this.label2);
            this.m_cleanupGroupBox.Controls.Add(this.m_rbQuota);
            this.m_cleanupGroupBox.Controls.Add(this.m_nudQuota);
            this.m_cleanupGroupBox.Controls.Add(this.m_rbLimitDirectories);
            this.m_cleanupGroupBox.Controls.Add(this.m_nudDirs);
            this.m_cleanupGroupBox.Name = "m_cleanupGroupBox";
            this.m_cleanupGroupBox.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // m_rbLimitFree
            // 
            resources.ApplyResources(this.m_rbLimitFree, "m_rbLimitFree");
            this.m_rbLimitFree.Name = "m_rbLimitFree";
            this.m_rbLimitFree.TabStop = true;
            this.m_rbLimitFree.UseVisualStyleBackColor = true;
            // 
            // m_nudFree
            // 
            resources.ApplyResources(this.m_nudFree, "m_nudFree");
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
            this.m_nudFree.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.m_nudFree.ValueChanged += new System.EventHandler(this.m_nudFree_ValueChanged);
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
            // 
            // m_nudQuota
            // 
            resources.ApplyResources(this.m_nudQuota, "m_nudQuota");
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
            this.m_nudQuota.Value = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.m_nudQuota.ValueChanged += new System.EventHandler(this.m_nudQuota_ValueChanged);
            // 
            // m_rbLimitDirectories
            // 
            resources.ApplyResources(this.m_rbLimitDirectories, "m_rbLimitDirectories");
            this.m_rbLimitDirectories.Name = "m_rbLimitDirectories";
            this.m_rbLimitDirectories.TabStop = true;
            this.m_rbLimitDirectories.UseVisualStyleBackColor = true;
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
            this.m_nudDirs.ValueChanged += new System.EventHandler(this.m_nudDirs_ValueChanged);
            // 
            // CleanupTaskControl
            // 
            resources.ApplyResources(this, "$this");
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
            this.m_cleanupGroupBox.ResumeLayout(false);
            this.m_cleanupGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudFree)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudQuota)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudDirs)).EndInit();
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
