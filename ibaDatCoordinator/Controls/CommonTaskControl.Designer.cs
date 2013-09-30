namespace iba.Controls
{
    partial class CommonTaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CommonTaskControl));
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_nameTextBox = new System.Windows.Forms.TextBox();
            this.m_ExecutegroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rb1stFailure = new System.Windows.Forms.RadioButton();
            this.m_rbAlways = new System.Windows.Forms.RadioButton();
            this.m_rbFailure = new System.Windows.Forms.RadioButton();
            this.m_rbDisabled = new System.Windows.Forms.RadioButton();
            this.m_rbSucces = new System.Windows.Forms.RadioButton();
            this.groupBoxNotify = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbNot1stFailure = new System.Windows.Forms.RadioButton();
            this.m_rbNotAlways = new System.Windows.Forms.RadioButton();
            this.m_rbNotFailure = new System.Windows.Forms.RadioButton();
            this.m_rbNotDisabled = new System.Windows.Forms.RadioButton();
            this.m_rbNotSuccess = new System.Windows.Forms.RadioButton();
            this.m_pluginPanel = new System.Windows.Forms.Panel();
            this.m_cbResourceCritical = new System.Windows.Forms.CheckBox();
            this.groupBox5.SuspendLayout();
            this.m_ExecutegroupBox.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBoxNotify.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Controls.Add(this.m_cbResourceCritical);
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.m_nameTextBox);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_nameTextBox
            // 
            resources.ApplyResources(this.m_nameTextBox, "m_nameTextBox");
            this.m_nameTextBox.Name = "m_nameTextBox";
            this.m_nameTextBox.TextChanged += new System.EventHandler(this.m_nameTextBox_TextChanged);
            // 
            // m_ExecutegroupBox
            // 
            resources.ApplyResources(this.m_ExecutegroupBox, "m_ExecutegroupBox");
            this.m_ExecutegroupBox.Controls.Add(this.tableLayoutPanel1);
            this.m_ExecutegroupBox.Name = "m_ExecutegroupBox";
            this.m_ExecutegroupBox.TabStop = false;
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.m_rb1stFailure, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_rbAlways, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_rbFailure, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_rbDisabled, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_rbSucces, 1, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // m_rb1stFailure
            // 
            resources.ApplyResources(this.m_rb1stFailure, "m_rb1stFailure");
            this.m_rb1stFailure.Name = "m_rb1stFailure";
            this.m_rb1stFailure.TabStop = true;
            this.m_rb1stFailure.UseVisualStyleBackColor = true;
            this.m_rb1stFailure.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbAlways
            // 
            resources.ApplyResources(this.m_rbAlways, "m_rbAlways");
            this.m_rbAlways.Name = "m_rbAlways";
            this.m_rbAlways.TabStop = true;
            this.m_rbAlways.UseVisualStyleBackColor = true;
            this.m_rbAlways.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbFailure
            // 
            resources.ApplyResources(this.m_rbFailure, "m_rbFailure");
            this.m_rbFailure.Name = "m_rbFailure";
            this.m_rbFailure.TabStop = true;
            this.m_rbFailure.UseVisualStyleBackColor = true;
            this.m_rbFailure.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbDisabled
            // 
            resources.ApplyResources(this.m_rbDisabled, "m_rbDisabled");
            this.m_rbDisabled.Name = "m_rbDisabled";
            this.m_rbDisabled.TabStop = true;
            this.m_rbDisabled.UseVisualStyleBackColor = true;
            this.m_rbDisabled.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbSucces
            // 
            resources.ApplyResources(this.m_rbSucces, "m_rbSucces");
            this.m_rbSucces.Name = "m_rbSucces";
            this.m_rbSucces.TabStop = true;
            this.m_rbSucces.UseVisualStyleBackColor = true;
            this.m_rbSucces.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // groupBoxNotify
            // 
            resources.ApplyResources(this.groupBoxNotify, "groupBoxNotify");
            this.groupBoxNotify.Controls.Add(this.tableLayoutPanel6);
            this.groupBoxNotify.Name = "groupBoxNotify";
            this.groupBoxNotify.TabStop = false;
            // 
            // tableLayoutPanel6
            // 
            resources.ApplyResources(this.tableLayoutPanel6, "tableLayoutPanel6");
            this.tableLayoutPanel6.Controls.Add(this.m_rbNot1stFailure, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotAlways, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotFailure, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotDisabled, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotSuccess, 1, 0);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            // 
            // m_rbNot1stFailure
            // 
            resources.ApplyResources(this.m_rbNot1stFailure, "m_rbNot1stFailure");
            this.m_rbNot1stFailure.Name = "m_rbNot1stFailure";
            this.m_rbNot1stFailure.TabStop = true;
            this.m_rbNot1stFailure.UseVisualStyleBackColor = true;
            // 
            // m_rbNotAlways
            // 
            resources.ApplyResources(this.m_rbNotAlways, "m_rbNotAlways");
            this.m_rbNotAlways.Name = "m_rbNotAlways";
            this.m_rbNotAlways.TabStop = true;
            this.m_rbNotAlways.UseVisualStyleBackColor = true;
            // 
            // m_rbNotFailure
            // 
            resources.ApplyResources(this.m_rbNotFailure, "m_rbNotFailure");
            this.m_rbNotFailure.Name = "m_rbNotFailure";
            this.m_rbNotFailure.TabStop = true;
            this.m_rbNotFailure.UseVisualStyleBackColor = true;
            // 
            // m_rbNotDisabled
            // 
            resources.ApplyResources(this.m_rbNotDisabled, "m_rbNotDisabled");
            this.m_rbNotDisabled.Name = "m_rbNotDisabled";
            this.m_rbNotDisabled.TabStop = true;
            this.m_rbNotDisabled.UseVisualStyleBackColor = true;
            // 
            // m_rbNotSuccess
            // 
            resources.ApplyResources(this.m_rbNotSuccess, "m_rbNotSuccess");
            this.m_rbNotSuccess.Name = "m_rbNotSuccess";
            this.m_rbNotSuccess.TabStop = true;
            this.m_rbNotSuccess.UseVisualStyleBackColor = true;
            // 
            // m_pluginPanel
            // 
            resources.ApplyResources(this.m_pluginPanel, "m_pluginPanel");
            this.m_pluginPanel.BackColor = System.Drawing.SystemColors.Control;
            this.m_pluginPanel.Name = "m_pluginPanel";
            // 
            // m_cbResourceCritical
            // 
            resources.ApplyResources(this.m_cbResourceCritical, "m_cbResourceCritical");
            this.m_cbResourceCritical.Name = "m_cbResourceCritical";
            this.m_cbResourceCritical.UseVisualStyleBackColor = true;
            // 
            // CommonTaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.m_pluginPanel);
            this.Controls.Add(this.groupBoxNotify);
            this.Controls.Add(this.m_ExecutegroupBox);
            this.Controls.Add(this.groupBox5);
            this.MinimumSize = new System.Drawing.Size(720, 250);
            this.Name = "CommonTaskControl";
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.m_ExecutegroupBox.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBoxNotify.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_nameTextBox;
        private System.Windows.Forms.GroupBox m_ExecutegroupBox;
        private System.Windows.Forms.GroupBox groupBoxNotify;
        private System.Windows.Forms.Panel m_pluginPanel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton m_rb1stFailure;
        private System.Windows.Forms.RadioButton m_rbAlways;
        private System.Windows.Forms.RadioButton m_rbFailure;
        private System.Windows.Forms.RadioButton m_rbDisabled;
        private System.Windows.Forms.RadioButton m_rbSucces;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton m_rbNot1stFailure;
        private System.Windows.Forms.RadioButton m_rbNotAlways;
        private System.Windows.Forms.RadioButton m_rbNotFailure;
        private System.Windows.Forms.RadioButton m_rbNotDisabled;
        private System.Windows.Forms.RadioButton m_rbNotSuccess;
        private System.Windows.Forms.CheckBox m_cbResourceCritical;
    }
}
