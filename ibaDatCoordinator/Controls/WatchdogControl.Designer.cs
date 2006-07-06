namespace iba.Controls
{
    partial class WatchdogControl
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
            this.m_statusGroupBox = new System.Windows.Forms.GroupBox();
            this.m_tbStatus = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_enableCheckBox = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_cycleUpDown = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_tbPort = new System.Windows.Forms.TextBox();
            this.m_tbHost = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_rbPassiveNode = new System.Windows.Forms.RadioButton();
            this.m_rbActiveNode = new System.Windows.Forms.RadioButton();
            this.m_statusGroupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_cycleUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_statusGroupBox
            // 
            this.m_statusGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_statusGroupBox.Controls.Add(this.m_tbStatus);
            this.m_statusGroupBox.Location = new System.Drawing.Point(28, 256);
            this.m_statusGroupBox.Name = "m_statusGroupBox";
            this.m_statusGroupBox.Size = new System.Drawing.Size(565, 172);
            this.m_statusGroupBox.TabIndex = 5;
            this.m_statusGroupBox.TabStop = false;
            this.m_statusGroupBox.Text = "Status";
            // 
            // m_tbStatus
            // 
            this.m_tbStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tbStatus.Location = new System.Drawing.Point(3, 16);
            this.m_tbStatus.Multiline = true;
            this.m_tbStatus.Name = "m_tbStatus";
            this.m_tbStatus.ReadOnly = true;
            this.m_tbStatus.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.m_tbStatus.Size = new System.Drawing.Size(559, 153);
            this.m_tbStatus.TabIndex = 0;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.m_enableCheckBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.m_cycleUpDown);
            this.groupBox3.Location = new System.Drawing.Point(28, 20);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(565, 73);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Cyclus";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(85, 44);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "cycle";
            // 
            // m_enableCheckBox
            // 
            this.m_enableCheckBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_enableCheckBox.AutoSize = true;
            this.m_enableCheckBox.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_enableCheckBox.Location = new System.Drawing.Point(16, 19);
            this.m_enableCheckBox.Name = "m_enableCheckBox";
            this.m_enableCheckBox.Size = new System.Drawing.Size(65, 17);
            this.m_enableCheckBox.TabIndex = 0;
            this.m_enableCheckBox.Text = "Enabled";
            this.m_enableCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label4.Location = new System.Drawing.Point(199, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "seconds";
            // 
            // m_cycleUpDown
            // 
            this.m_cycleUpDown.Location = new System.Drawing.Point(127, 42);
            this.m_cycleUpDown.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_cycleUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_cycleUpDown.Name = "m_cycleUpDown";
            this.m_cycleUpDown.Size = new System.Drawing.Size(66, 20);
            this.m_cycleUpDown.TabIndex = 3;
            this.m_cycleUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.m_tbPort);
            this.groupBox1.Controls.Add(this.m_tbHost);
            this.groupBox1.Location = new System.Drawing.Point(28, 99);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(565, 77);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "TCP/IP";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label8.Location = new System.Drawing.Point(85, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(28, 13);
            this.label8.TabIndex = 7;
            this.label8.Text = "port:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(13, 25);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(108, 13);
            this.label7.TabIndex = 5;
            this.label7.Text = "address or hostname:";
            // 
            // m_tbPort
            // 
            this.m_tbPort.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbPort.Location = new System.Drawing.Point(127, 48);
            this.m_tbPort.Name = "m_tbPort";
            this.m_tbPort.Size = new System.Drawing.Size(79, 20);
            this.m_tbPort.TabIndex = 8;
            // 
            // m_tbHost
            // 
            this.m_tbHost.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbHost.Location = new System.Drawing.Point(127, 22);
            this.m_tbHost.Name = "m_tbHost";
            this.m_tbHost.Size = new System.Drawing.Size(418, 20);
            this.m_tbHost.TabIndex = 6;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.m_rbPassiveNode);
            this.groupBox2.Controls.Add(this.m_rbActiveNode);
            this.groupBox2.Location = new System.Drawing.Point(29, 182);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(564, 68);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Node";
            // 
            // m_rbPassiveNode
            // 
            this.m_rbPassiveNode.AutoSize = true;
            this.m_rbPassiveNode.Location = new System.Drawing.Point(15, 42);
            this.m_rbPassiveNode.Name = "m_rbPassiveNode";
            this.m_rbPassiveNode.Size = new System.Drawing.Size(121, 17);
            this.m_rbPassiveNode.TabIndex = 1;
            this.m_rbPassiveNode.TabStop = true;
            this.m_rbPassiveNode.Text = "This node is passive";
            this.m_rbPassiveNode.UseVisualStyleBackColor = true;
            // 
            // m_rbActiveNode
            // 
            this.m_rbActiveNode.AutoSize = true;
            this.m_rbActiveNode.Location = new System.Drawing.Point(15, 19);
            this.m_rbActiveNode.Name = "m_rbActiveNode";
            this.m_rbActiveNode.Size = new System.Drawing.Size(114, 17);
            this.m_rbActiveNode.TabIndex = 0;
            this.m_rbActiveNode.TabStop = true;
            this.m_rbActiveNode.Text = "This node is active";
            this.m_rbActiveNode.UseVisualStyleBackColor = true;
            // 
            // WatchdogControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.m_statusGroupBox);
            this.MinimumSize = new System.Drawing.Size(620, 300);
            this.Name = "WatchdogControl";
            this.Size = new System.Drawing.Size(620, 452);
            this.m_statusGroupBox.ResumeLayout(false);
            this.m_statusGroupBox.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_cycleUpDown)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox m_statusGroupBox;
        private System.Windows.Forms.TextBox m_tbStatus;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox m_enableCheckBox;
        private System.Windows.Forms.NumericUpDown m_cycleUpDown;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox m_tbPort;
        private System.Windows.Forms.TextBox m_tbHost;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton m_rbPassiveNode;
        private System.Windows.Forms.RadioButton m_rbActiveNode;
    }
}
