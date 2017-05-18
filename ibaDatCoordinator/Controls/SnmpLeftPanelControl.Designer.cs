namespace iba.Controls
{
    partial class SnmpLeftPanelControl
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
            this.buttonShowDebug = new System.Windows.Forms.Button();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.timerStatus = new System.Windows.Forms.Timer(this.components);
            this.label14 = new System.Windows.Forms.Label();
            this.tbGeneralInfo = new System.Windows.Forms.TextBox();
            this.buttonCleanLog = new System.Windows.Forms.Button();
            this.tbLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // buttonShowDebug
            // 
            this.buttonShowDebug.Location = new System.Drawing.Point(3, 244);
            this.buttonShowDebug.Name = "buttonShowDebug";
            this.buttonShowDebug.Size = new System.Drawing.Size(106, 23);
            this.buttonShowDebug.TabIndex = 0;
            this.buttonShowDebug.Text = "Debug";
            this.buttonShowDebug.UseVisualStyleBackColor = false;
            this.buttonShowDebug.Click += new System.EventHandler(this.buttonShowDebug_Click);
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.Checked = true;
            this.cbEnabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.cbEnabled.Enabled = false;
            this.cbEnabled.Location = new System.Drawing.Point(3, 3);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbEnabled.TabIndex = 2;
            this.cbEnabled.Text = "Enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // tbStatus
            // 
            this.tbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStatus.Location = new System.Drawing.Point(3, 59);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.Size = new System.Drawing.Size(224, 20);
            this.tbStatus.TabIndex = 4;
            // 
            // timerStatus
            // 
            this.timerStatus.Enabled = true;
            this.timerStatus.Interval = 1000;
            this.timerStatus.Tick += new System.EventHandler(this.timerStatus_Tick);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(3, 43);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 5;
            this.label14.Text = "Status:";
            // 
            // tbGeneralInfo
            // 
            this.tbGeneralInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbGeneralInfo.Location = new System.Drawing.Point(3, 85);
            this.tbGeneralInfo.Multiline = true;
            this.tbGeneralInfo.Name = "tbGeneralInfo";
            this.tbGeneralInfo.Size = new System.Drawing.Size(224, 153);
            this.tbGeneralInfo.TabIndex = 4;
            // 
            // buttonCleanLog
            // 
            this.buttonCleanLog.Location = new System.Drawing.Point(115, 244);
            this.buttonCleanLog.Name = "buttonCleanLog";
            this.buttonCleanLog.Size = new System.Drawing.Size(76, 23);
            this.buttonCleanLog.TabIndex = 6;
            this.buttonCleanLog.Text = "C";
            this.buttonCleanLog.UseVisualStyleBackColor = true;
            this.buttonCleanLog.Click += new System.EventHandler(this.buttonCleanLog_Click);
            // 
            // tbLog
            // 
            this.tbLog.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbLog.Location = new System.Drawing.Point(6, 273);
            this.tbLog.Multiline = true;
            this.tbLog.Name = "tbLog";
            this.tbLog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbLog.Size = new System.Drawing.Size(212, 424);
            this.tbLog.TabIndex = 7;
            this.tbLog.Text = "Log";
            // 
            // SnmpLeftPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.buttonCleanLog);
            this.Controls.Add(this.tbLog);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cbEnabled);
            this.Controls.Add(this.tbGeneralInfo);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.buttonShowDebug);
            this.Name = "SnmpLeftPanelControl";
            this.Size = new System.Drawing.Size(230, 737);
            this.Load += new System.EventHandler(this.SnmpLeftPanelControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonShowDebug;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Timer timerStatus;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox tbGeneralInfo;
        private System.Windows.Forms.Button buttonCleanLog;
        private System.Windows.Forms.TextBox tbLog;
    }
}
