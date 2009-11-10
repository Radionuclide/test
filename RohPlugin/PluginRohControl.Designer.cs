namespace Alunorf_roh_plugin
{
    partial class PluginRohControl
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
            this.m_tabControl = new System.Windows.Forms.TabControl();
            this.m_stichTab = new System.Windows.Forms.TabPage();
            this.m_kommentareTab = new System.Windows.Forms.TabPage();
            this.m_Panel = new System.Windows.Forms.Panel();
            this.m_browseDatFileButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_datFileTextBox = new System.Windows.Forms.TextBox();
            this.m_selectButton = new System.Windows.Forms.Button();
            this.m_kopfTab = new System.Windows.Forms.TabPage();
            this.m_schlussTab = new System.Windows.Forms.TabPage();
            this.m_kurzbezeichnerTab = new System.Windows.Forms.TabPage();
            this.m_parameterTab = new System.Windows.Forms.TabPage();
            this.m_kanalTab = new System.Windows.Forms.TabPage();
            this.m_ftpTab = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.m_ftpHost = new System.Windows.Forms.TextBox();
            this.m_ftpDirectory = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_nudFtpPort = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.m_ftpUsername = new System.Windows.Forms.TextBox();
            this.m_ftpPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.m_tabControl.SuspendLayout();
            this.m_Panel.SuspendLayout();
            this.m_ftpTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudFtpPort)).BeginInit();
            this.SuspendLayout();
            // 
            // m_tabControl
            // 
            this.m_tabControl.Controls.Add(this.m_stichTab);
            this.m_tabControl.Controls.Add(this.m_kommentareTab);
            this.m_tabControl.Controls.Add(this.m_kopfTab);
            this.m_tabControl.Controls.Add(this.m_schlussTab);
            this.m_tabControl.Controls.Add(this.m_kurzbezeichnerTab);
            this.m_tabControl.Controls.Add(this.m_parameterTab);
            this.m_tabControl.Controls.Add(this.m_kanalTab);
            this.m_tabControl.Controls.Add(this.m_ftpTab);
            this.m_tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tabControl.Location = new System.Drawing.Point(0, 0);
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            this.m_tabControl.Size = new System.Drawing.Size(565, 410);
            this.m_tabControl.TabIndex = 0;
            // 
            // m_stichTab
            // 
            this.m_stichTab.Location = new System.Drawing.Point(4, 22);
            this.m_stichTab.Name = "m_stichTab";
            this.m_stichTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_stichTab.Size = new System.Drawing.Size(557, 384);
            this.m_stichTab.TabIndex = 0;
            this.m_stichTab.Text = "Stichdaten";
            this.m_stichTab.UseVisualStyleBackColor = true;
            // 
            // m_kommentareTab
            // 
            this.m_kommentareTab.Location = new System.Drawing.Point(4, 22);
            this.m_kommentareTab.Name = "m_kommentareTab";
            this.m_kommentareTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_kommentareTab.Size = new System.Drawing.Size(557, 384);
            this.m_kommentareTab.TabIndex = 1;
            this.m_kommentareTab.Text = "Kommentare";
            this.m_kommentareTab.UseVisualStyleBackColor = true;
            // 
            // m_Panel
            // 
            this.m_Panel.Controls.Add(this.m_browseDatFileButton);
            this.m_Panel.Controls.Add(this.label3);
            this.m_Panel.Controls.Add(this.m_datFileTextBox);
            this.m_Panel.Controls.Add(this.m_selectButton);
            this.m_Panel.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.m_Panel.Location = new System.Drawing.Point(0, 358);
            this.m_Panel.Name = "m_Panel";
            this.m_Panel.Size = new System.Drawing.Size(565, 52);
            this.m_Panel.TabIndex = 1;
            // 
            // m_browseDatFileButton
            // 
            this.m_browseDatFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseDatFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseDatFileButton.Location = new System.Drawing.Point(460, 6);
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_browseDatFileButton.Size = new System.Drawing.Size(40, 40);
            this.m_browseDatFileButton.TabIndex = 13;
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(18, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(87, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "template .dat file:";
            // 
            // m_datFileTextBox
            // 
            this.m_datFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_datFileTextBox.Location = new System.Drawing.Point(108, 17);
            this.m_datFileTextBox.Name = "m_datFileTextBox";
            this.m_datFileTextBox.Size = new System.Drawing.Size(346, 20);
            this.m_datFileTextBox.TabIndex = 12;
            // 
            // m_selectButton
            // 
            this.m_selectButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_selectButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_selectButton.Location = new System.Drawing.Point(506, 6);
            this.m_selectButton.Name = "m_selectButton";
            this.m_selectButton.Size = new System.Drawing.Size(40, 40);
            this.m_selectButton.TabIndex = 14;
            this.m_selectButton.UseVisualStyleBackColor = true;
            // 
            // m_kopfTab
            // 
            this.m_kopfTab.Location = new System.Drawing.Point(4, 22);
            this.m_kopfTab.Name = "m_kopfTab";
            this.m_kopfTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_kopfTab.Size = new System.Drawing.Size(557, 384);
            this.m_kopfTab.TabIndex = 2;
            this.m_kopfTab.Text = "Kopfdaten";
            this.m_kopfTab.UseVisualStyleBackColor = true;
            // 
            // m_schlussTab
            // 
            this.m_schlussTab.Location = new System.Drawing.Point(4, 22);
            this.m_schlussTab.Name = "m_schlussTab";
            this.m_schlussTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_schlussTab.Size = new System.Drawing.Size(557, 384);
            this.m_schlussTab.TabIndex = 3;
            this.m_schlussTab.Text = "Schlussdaten";
            this.m_schlussTab.UseVisualStyleBackColor = true;
            // 
            // m_kurzbezeichnerTab
            // 
            this.m_kurzbezeichnerTab.Location = new System.Drawing.Point(4, 22);
            this.m_kurzbezeichnerTab.Name = "m_kurzbezeichnerTab";
            this.m_kurzbezeichnerTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_kurzbezeichnerTab.Size = new System.Drawing.Size(557, 384);
            this.m_kurzbezeichnerTab.TabIndex = 4;
            this.m_kurzbezeichnerTab.Text = "Kurzbezeichner";
            this.m_kurzbezeichnerTab.UseVisualStyleBackColor = true;
            // 
            // m_parameterTab
            // 
            this.m_parameterTab.Location = new System.Drawing.Point(4, 22);
            this.m_parameterTab.Name = "m_parameterTab";
            this.m_parameterTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_parameterTab.Size = new System.Drawing.Size(557, 384);
            this.m_parameterTab.TabIndex = 5;
            this.m_parameterTab.Text = "Parameter";
            this.m_parameterTab.UseVisualStyleBackColor = true;
            // 
            // m_kanalTab
            // 
            this.m_kanalTab.Location = new System.Drawing.Point(4, 22);
            this.m_kanalTab.Name = "m_kanalTab";
            this.m_kanalTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_kanalTab.Size = new System.Drawing.Size(557, 384);
            this.m_kanalTab.TabIndex = 6;
            this.m_kanalTab.Text = "Kanalbeschreibung";
            this.m_kanalTab.UseVisualStyleBackColor = true;
            // 
            // m_ftpTab
            // 
            this.m_ftpTab.Controls.Add(this.m_ftpPassword);
            this.m_ftpTab.Controls.Add(this.label6);
            this.m_ftpTab.Controls.Add(this.m_ftpUsername);
            this.m_ftpTab.Controls.Add(this.label5);
            this.m_ftpTab.Controls.Add(this.label4);
            this.m_ftpTab.Controls.Add(this.m_nudFtpPort);
            this.m_ftpTab.Controls.Add(this.m_ftpDirectory);
            this.m_ftpTab.Controls.Add(this.label2);
            this.m_ftpTab.Controls.Add(this.m_ftpHost);
            this.m_ftpTab.Controls.Add(this.label1);
            this.m_ftpTab.Location = new System.Drawing.Point(4, 22);
            this.m_ftpTab.Name = "m_ftpTab";
            this.m_ftpTab.Padding = new System.Windows.Forms.Padding(3);
            this.m_ftpTab.Size = new System.Drawing.Size(557, 384);
            this.m_ftpTab.TabIndex = 7;
            this.m_ftpTab.Text = "FTP";
            this.m_ftpTab.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(14, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Host:";
            // 
            // m_ftpHost
            // 
            this.m_ftpHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ftpHost.Location = new System.Drawing.Point(72, 25);
            this.m_ftpHost.Name = "m_ftpHost";
            this.m_ftpHost.Size = new System.Drawing.Size(424, 20);
            this.m_ftpHost.TabIndex = 1;
            // 
            // m_ftpDirectory
            // 
            this.m_ftpDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ftpDirectory.Location = new System.Drawing.Point(72, 51);
            this.m_ftpDirectory.Name = "m_ftpDirectory";
            this.m_ftpDirectory.Size = new System.Drawing.Size(424, 20);
            this.m_ftpDirectory.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 54);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(52, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Directory:";
            // 
            // m_nudFtpPort
            // 
            this.m_nudFtpPort.Location = new System.Drawing.Point(72, 77);
            this.m_nudFtpPort.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.m_nudFtpPort.Name = "m_nudFtpPort";
            this.m_nudFtpPort.Size = new System.Drawing.Size(66, 20);
            this.m_nudFtpPort.TabIndex = 4;
            this.m_nudFtpPort.Value = new decimal(new int[] {
            31,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(14, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(29, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Port:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(14, 106);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Username:";
            // 
            // m_ftpUsername
            // 
            this.m_ftpUsername.Location = new System.Drawing.Point(72, 103);
            this.m_ftpUsername.Name = "m_ftpUsername";
            this.m_ftpUsername.Size = new System.Drawing.Size(189, 20);
            this.m_ftpUsername.TabIndex = 7;
            // 
            // m_ftpPassword
            // 
            this.m_ftpPassword.Location = new System.Drawing.Point(72, 129);
            this.m_ftpPassword.Name = "m_ftpPassword";
            this.m_ftpPassword.Size = new System.Drawing.Size(189, 20);
            this.m_ftpPassword.TabIndex = 9;
            this.m_ftpPassword.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 132);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "Password:";
            // 
            // PluginRohControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_Panel);
            this.Controls.Add(this.m_tabControl);
            this.Name = "PluginRohControl";
            this.Size = new System.Drawing.Size(565, 410);
            this.m_tabControl.ResumeLayout(false);
            this.m_Panel.ResumeLayout(false);
            this.m_Panel.PerformLayout();
            this.m_ftpTab.ResumeLayout(false);
            this.m_ftpTab.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudFtpPort)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl m_tabControl;
        private System.Windows.Forms.TabPage m_stichTab;
        private System.Windows.Forms.TabPage m_kommentareTab;
        private System.Windows.Forms.Panel m_Panel;
        private System.Windows.Forms.Button m_browseDatFileButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_datFileTextBox;
        private System.Windows.Forms.Button m_selectButton;
        private System.Windows.Forms.TabPage m_kopfTab;
        private System.Windows.Forms.TabPage m_schlussTab;
        private System.Windows.Forms.TabPage m_kurzbezeichnerTab;
        private System.Windows.Forms.TabPage m_parameterTab;
        private System.Windows.Forms.TabPage m_kanalTab;
        private System.Windows.Forms.TabPage m_ftpTab;
        private System.Windows.Forms.TextBox m_ftpHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_ftpDirectory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_ftpPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox m_ftpUsername;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown m_nudFtpPort;
    }
}
