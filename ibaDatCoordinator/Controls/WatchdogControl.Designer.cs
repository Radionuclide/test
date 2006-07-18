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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WatchdogControl));
            this.m_statusGroupBox = new System.Windows.Forms.GroupBox();
            this.m_tbStatus = new System.Windows.Forms.TextBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.m_ApplyButton = new System.Windows.Forms.Button();
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
            this.m_timerStatus = new System.Windows.Forms.Timer(this.components);
            this.m_statusGroupBox.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_cycleUpDown)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_statusGroupBox
            // 
            this.m_statusGroupBox.AccessibleDescription = null;
            this.m_statusGroupBox.AccessibleName = null;
            resources.ApplyResources(this.m_statusGroupBox, "m_statusGroupBox");
            this.m_statusGroupBox.BackgroundImage = null;
            this.m_statusGroupBox.Controls.Add(this.m_tbStatus);
            this.m_statusGroupBox.Font = null;
            this.m_statusGroupBox.Name = "m_statusGroupBox";
            this.m_statusGroupBox.TabStop = false;
            // 
            // m_tbStatus
            // 
            this.m_tbStatus.AccessibleDescription = null;
            this.m_tbStatus.AccessibleName = null;
            resources.ApplyResources(this.m_tbStatus, "m_tbStatus");
            this.m_tbStatus.BackgroundImage = null;
            this.m_tbStatus.Font = null;
            this.m_tbStatus.Name = "m_tbStatus";
            this.m_tbStatus.ReadOnly = true;
            // 
            // groupBox3
            // 
            this.groupBox3.AccessibleDescription = null;
            this.groupBox3.AccessibleName = null;
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.BackgroundImage = null;
            this.groupBox3.Controls.Add(this.m_ApplyButton);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Controls.Add(this.m_enableCheckBox);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.m_cycleUpDown);
            this.groupBox3.Font = null;
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // m_ApplyButton
            // 
            this.m_ApplyButton.AccessibleDescription = null;
            this.m_ApplyButton.AccessibleName = null;
            resources.ApplyResources(this.m_ApplyButton, "m_ApplyButton");
            this.m_ApplyButton.BackgroundImage = null;
            this.m_ApplyButton.Font = null;
            this.m_ApplyButton.Name = "m_ApplyButton";
            this.m_ApplyButton.UseVisualStyleBackColor = true;
            this.m_ApplyButton.Click += new System.EventHandler(this.m_applyButton_Click);
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // m_enableCheckBox
            // 
            this.m_enableCheckBox.AccessibleDescription = null;
            this.m_enableCheckBox.AccessibleName = null;
            resources.ApplyResources(this.m_enableCheckBox, "m_enableCheckBox");
            this.m_enableCheckBox.BackgroundImage = null;
            this.m_enableCheckBox.Font = null;
            this.m_enableCheckBox.Name = "m_enableCheckBox";
            this.m_enableCheckBox.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AccessibleDescription = null;
            this.label4.AccessibleName = null;
            resources.ApplyResources(this.label4, "label4");
            this.label4.Font = null;
            this.label4.Name = "label4";
            // 
            // m_cycleUpDown
            // 
            this.m_cycleUpDown.AccessibleDescription = null;
            this.m_cycleUpDown.AccessibleName = null;
            resources.ApplyResources(this.m_cycleUpDown, "m_cycleUpDown");
            this.m_cycleUpDown.Font = null;
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
            this.m_cycleUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.AccessibleDescription = null;
            this.groupBox1.AccessibleName = null;
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.BackgroundImage = null;
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.m_tbPort);
            this.groupBox1.Controls.Add(this.m_tbHost);
            this.groupBox1.Font = null;
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label8
            // 
            this.label8.AccessibleDescription = null;
            this.label8.AccessibleName = null;
            resources.ApplyResources(this.label8, "label8");
            this.label8.Font = null;
            this.label8.Name = "label8";
            // 
            // label7
            // 
            this.label7.AccessibleDescription = null;
            this.label7.AccessibleName = null;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Font = null;
            this.label7.Name = "label7";
            // 
            // m_tbPort
            // 
            this.m_tbPort.AccessibleDescription = null;
            this.m_tbPort.AccessibleName = null;
            resources.ApplyResources(this.m_tbPort, "m_tbPort");
            this.m_tbPort.BackgroundImage = null;
            this.m_tbPort.Font = null;
            this.m_tbPort.Name = "m_tbPort";
            // 
            // m_tbHost
            // 
            this.m_tbHost.AccessibleDescription = null;
            this.m_tbHost.AccessibleName = null;
            resources.ApplyResources(this.m_tbHost, "m_tbHost");
            this.m_tbHost.BackgroundImage = null;
            this.m_tbHost.Font = null;
            this.m_tbHost.Name = "m_tbHost";
            // 
            // groupBox2
            // 
            this.groupBox2.AccessibleDescription = null;
            this.groupBox2.AccessibleName = null;
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.BackgroundImage = null;
            this.groupBox2.Controls.Add(this.m_rbPassiveNode);
            this.groupBox2.Controls.Add(this.m_rbActiveNode);
            this.groupBox2.Font = null;
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_rbPassiveNode
            // 
            this.m_rbPassiveNode.AccessibleDescription = null;
            this.m_rbPassiveNode.AccessibleName = null;
            resources.ApplyResources(this.m_rbPassiveNode, "m_rbPassiveNode");
            this.m_rbPassiveNode.BackgroundImage = null;
            this.m_rbPassiveNode.Font = null;
            this.m_rbPassiveNode.Name = "m_rbPassiveNode";
            this.m_rbPassiveNode.TabStop = true;
            this.m_rbPassiveNode.UseVisualStyleBackColor = true;
            // 
            // m_rbActiveNode
            // 
            this.m_rbActiveNode.AccessibleDescription = null;
            this.m_rbActiveNode.AccessibleName = null;
            resources.ApplyResources(this.m_rbActiveNode, "m_rbActiveNode");
            this.m_rbActiveNode.BackgroundImage = null;
            this.m_rbActiveNode.Font = null;
            this.m_rbActiveNode.Name = "m_rbActiveNode";
            this.m_rbActiveNode.TabStop = true;
            this.m_rbActiveNode.UseVisualStyleBackColor = true;
            // 
            // m_timerStatus
            // 
            this.m_timerStatus.Interval = 500;
            this.m_timerStatus.Tick += new System.EventHandler(this.m_timerStatus_Tick);
            // 
            // WatchdogControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.m_statusGroupBox);
            this.Font = null;
            this.MinimumSize = new System.Drawing.Size(620, 300);
            this.Name = "WatchdogControl";
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
        private System.Windows.Forms.Timer m_timerStatus;
        private System.Windows.Forms.Button m_ApplyButton;
    }
}
