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
            this.SuspendLayout();
            // 
            // buttonShowDebug
            // 
            this.buttonShowDebug.Location = new System.Drawing.Point(3, 85);
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
            // SnmpLeftPanelControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cbEnabled);
            this.Controls.Add(this.tbStatus);
            this.Controls.Add(this.buttonShowDebug);
            this.Name = "SnmpLeftPanelControl";
            this.Size = new System.Drawing.Size(230, 119);
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
    }
}
