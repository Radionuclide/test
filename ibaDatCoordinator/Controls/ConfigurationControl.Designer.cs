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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ConfigurationControl));
			this.m_toolTip = new System.Windows.Forms.ToolTip();
			this.m_sourcePanel = new System.Windows.Forms.Panel();
			this.gbNotifications = new iba.Utility.CollapsibleGroupBox();
			this.groupBox7 = new System.Windows.Forms.GroupBox();
			this.label6 = new System.Windows.Forms.Label();
			this.m_nudNotifyTime = new System.Windows.Forms.NumericUpDown();
			this.m_rbTime = new System.Windows.Forms.RadioButton();
			this.m_rbImmediate = new System.Windows.Forms.RadioButton();
			this.m_subfolderGroupBox = new System.Windows.Forms.GroupBox();
			this.labelsender = new System.Windows.Forms.Label();
			this.m_tbSender = new System.Windows.Forms.TextBox();
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
			this.gbNewTask = new iba.Utility.CollapsibleGroupBox();
			this.m_newTaskToolstrip = new System.Windows.Forms.ToolStrip();
			this.m_newReportButton = new System.Windows.Forms.ToolStripButton();
			this.m_newExtractButton = new System.Windows.Forms.ToolStripButton();
			this.m_newBatchfileButton = new System.Windows.Forms.ToolStripButton();
			this.m_newCopyTaskButton = new System.Windows.Forms.ToolStripButton();
			this.m_newIfTaskButton = new System.Windows.Forms.ToolStripButton();
			this.m_newUpdateDataTaskButton = new System.Windows.Forms.ToolStripButton();
			this.m_newPauseTaskButton = new System.Windows.Forms.ToolStripButton();
			this.m_newCleanupTaskButton = new System.Windows.Forms.ToolStripButton();
			this.m_newSplitterTaskButton = new System.Windows.Forms.ToolStripButton();
			this.m_newHdCreateEventTaskButton = new System.Windows.Forms.ToolStripButton();
			this.gbJobName = new iba.Utility.CollapsibleGroupBox();
			this.label1 = new System.Windows.Forms.Label();
			this.m_nameTextBox = new System.Windows.Forms.TextBox();
			this.gbNotifications.SuspendLayout();
			this.groupBox7.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_nudNotifyTime)).BeginInit();
			this.m_subfolderGroupBox.SuspendLayout();
			this.gbNewTask.SuspendLayout();
			this.m_newTaskToolstrip.SuspendLayout();
			this.gbJobName.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_sourcePanel
			// 
			resources.ApplyResources(this.m_sourcePanel, "m_sourcePanel");
			this.m_sourcePanel.Name = "m_sourcePanel";
			// 
			// gbNotifications
			// 
			resources.ApplyResources(this.gbNotifications, "gbNotifications");
			this.gbNotifications.Controls.Add(this.groupBox7);
			this.gbNotifications.Controls.Add(this.m_subfolderGroupBox);
			this.gbNotifications.Name = "gbNotifications";
			this.gbNotifications.TabStop = false;
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
			// m_subfolderGroupBox
			// 
			resources.ApplyResources(this.m_subfolderGroupBox, "m_subfolderGroupBox");
			this.m_subfolderGroupBox.Controls.Add(this.labelsender);
			this.m_subfolderGroupBox.Controls.Add(this.m_tbSender);
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
			// labelsender
			// 
			resources.ApplyResources(this.labelsender, "labelsender");
			this.labelsender.Name = "labelsender";
			// 
			// m_tbSender
			// 
			resources.ApplyResources(this.m_tbSender, "m_tbSender");
			this.m_tbSender.Name = "m_tbSender";
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
			// gbNewTask
			// 
			resources.ApplyResources(this.gbNewTask, "gbNewTask");
			this.gbNewTask.Controls.Add(this.m_newTaskToolstrip);
			this.gbNewTask.Name = "gbNewTask";
			this.gbNewTask.TabStop = false;
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
            this.m_newIfTaskButton,
            this.m_newUpdateDataTaskButton,
            this.m_newPauseTaskButton,
            this.m_newCleanupTaskButton,
            this.m_newSplitterTaskButton,
            this.m_newHdCreateEventTaskButton});
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
			// m_newUpdateDataTaskButton
			// 
			resources.ApplyResources(this.m_newUpdateDataTaskButton, "m_newUpdateDataTaskButton");
			this.m_newUpdateDataTaskButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_newUpdateDataTaskButton.Name = "m_newUpdateDataTaskButton";
			this.m_newUpdateDataTaskButton.Click += new System.EventHandler(this.m_newUpdateDataTaskButton_Click);
			// 
			// m_newPauseTaskButton
			// 
			resources.ApplyResources(this.m_newPauseTaskButton, "m_newPauseTaskButton");
			this.m_newPauseTaskButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_newPauseTaskButton.Name = "m_newPauseTaskButton";
			this.m_newPauseTaskButton.Click += new System.EventHandler(this.m_newPauseTaskButton_Click);
			// 
			// m_newCleanupTaskButton
			// 
			resources.ApplyResources(this.m_newCleanupTaskButton, "m_newCleanupTaskButton");
			this.m_newCleanupTaskButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_newCleanupTaskButton.Name = "m_newCleanupTaskButton";
			this.m_newCleanupTaskButton.Click += new System.EventHandler(this.m_newCleanupTaskButton_Click);
			// 
			// m_newSplitterTaskButton
			// 
			resources.ApplyResources(this.m_newSplitterTaskButton, "m_newSplitterTaskButton");
			this.m_newSplitterTaskButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_newSplitterTaskButton.Name = "m_newSplitterTaskButton";
			this.m_newSplitterTaskButton.Click += new System.EventHandler(this.m_newSplitterTaskButton_Click);
			// 
			// m_newHdCreateEventTaskButton
			// 
			resources.ApplyResources(this.m_newHdCreateEventTaskButton, "m_newHdCreateEventTaskButton");
			this.m_newHdCreateEventTaskButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.m_newHdCreateEventTaskButton.Name = "m_newHdCreateEventTaskButton";
			this.m_newHdCreateEventTaskButton.Click += new System.EventHandler(this.m_newHDCreateEventTaskButton_Click);
			// 
			// gbJobName
			// 
			resources.ApplyResources(this.gbJobName, "gbJobName");
			this.gbJobName.Controls.Add(this.label1);
			this.gbJobName.Controls.Add(this.m_nameTextBox);
			this.gbJobName.Name = "gbJobName";
			this.gbJobName.TabStop = false;
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			// 
			// m_nameTextBox
			// 
			resources.ApplyResources(this.m_nameTextBox, "m_nameTextBox");
			this.m_nameTextBox.Name = "m_nameTextBox";
			this.m_nameTextBox.TextChanged += new System.EventHandler(this.m_nameTextBox_TextChanged);
			// 
			// ConfigurationControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.m_sourcePanel);
			this.Controls.Add(this.gbNotifications);
			this.Controls.Add(this.gbNewTask);
			this.Controls.Add(this.gbJobName);
			this.Name = "ConfigurationControl";
			this.gbNotifications.ResumeLayout(false);
			this.groupBox7.ResumeLayout(false);
			this.groupBox7.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_nudNotifyTime)).EndInit();
			this.m_subfolderGroupBox.ResumeLayout(false);
			this.m_subfolderGroupBox.PerformLayout();
			this.gbNewTask.ResumeLayout(false);
			this.gbNewTask.PerformLayout();
			this.m_newTaskToolstrip.ResumeLayout(false);
			this.m_newTaskToolstrip.PerformLayout();
			this.gbJobName.ResumeLayout(false);
			this.gbJobName.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox m_nameTextBox;
        private System.Windows.Forms.Label label1;
        private iba.Utility.CollapsibleGroupBox gbJobName;
        private System.Windows.Forms.ToolTip m_toolTip;
        private iba.Utility.CollapsibleGroupBox gbNewTask;
        private System.Windows.Forms.GroupBox m_subfolderGroupBox;
        private System.Windows.Forms.RadioButton m_rbNetSend;
        private System.Windows.Forms.TextBox m_tbEmail;
        private System.Windows.Forms.RadioButton m_rbEmail;
        private System.Windows.Forms.TextBox m_tbNetSend;
        private iba.Utility.CollapsibleGroupBox gbNotifications;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.RadioButton m_rbTime;
        private System.Windows.Forms.RadioButton m_rbImmediate;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown m_nudNotifyTime;
        private System.Windows.Forms.Label labelmailrecipient;
        private System.Windows.Forms.TextBox m_tbSMTP;
        private System.Windows.Forms.Label labelnetsendhost;
        private System.Windows.Forms.Label labelmailsmtp;
        private System.Windows.Forms.ToolStrip m_newTaskToolstrip;
        private System.Windows.Forms.ToolStripButton m_newReportButton;
        private System.Windows.Forms.ToolStripButton m_newExtractButton;
        private System.Windows.Forms.ToolStripButton m_newBatchfileButton;
        private System.Windows.Forms.ToolStripButton m_newCopyTaskButton;
        private System.Windows.Forms.ToolStripButton m_newIfTaskButton;
        private System.Windows.Forms.Button m_testNotification;
        private System.Windows.Forms.Label labelmailpass;
        private System.Windows.Forms.TextBox m_tbMailPass;
        private System.Windows.Forms.Label labelmailuser;
        private System.Windows.Forms.TextBox m_tbMailUsername;
        private System.Windows.Forms.CheckBox m_cbAuthentication;
        private System.Windows.Forms.ToolStripButton m_newUpdateDataTaskButton;
        private System.Windows.Forms.ToolStripButton m_newPauseTaskButton;
        private System.Windows.Forms.Label labelsender;
        private System.Windows.Forms.TextBox m_tbSender;
        private System.Windows.Forms.Panel m_sourcePanel;
        private System.Windows.Forms.ToolStripButton m_newCleanupTaskButton;
        private System.Windows.Forms.ToolStripButton m_newSplitterTaskButton;
        private System.Windows.Forms.ToolStripButton m_newHdCreateEventTaskButton;
    }
}
