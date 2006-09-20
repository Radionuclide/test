namespace iba.Controls
{
    partial class CustomTaskControl
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
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.m_nameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rb1stFailure = new System.Windows.Forms.RadioButton();
            this.m_rbAlways = new System.Windows.Forms.RadioButton();
            this.m_rbFailure = new System.Windows.Forms.RadioButton();
            this.m_rbDisabled = new System.Windows.Forms.RadioButton();
            this.m_rbSucces = new System.Windows.Forms.RadioButton();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbNot1stFailure = new System.Windows.Forms.RadioButton();
            this.m_rbNotAlways = new System.Windows.Forms.RadioButton();
            this.m_rbNotFailure = new System.Windows.Forms.RadioButton();
            this.m_rbNotDisabled = new System.Windows.Forms.RadioButton();
            this.m_rbNotSuccess = new System.Windows.Forms.RadioButton();
            this.m_pluginPanel = new System.Windows.Forms.Panel();
            this.groupBox5.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox5
            // 
            this.groupBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox5.Controls.Add(this.label2);
            this.groupBox5.Controls.Add(this.m_nameTextBox);
            this.groupBox5.Location = new System.Drawing.Point(28, 20);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(565, 63);
            this.groupBox5.TabIndex = 1;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "ID";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(13, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(117, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Copy plugin task name:";
            // 
            // m_nameTextBox
            // 
            this.m_nameTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_nameTextBox.Location = new System.Drawing.Point(171, 26);
            this.m_nameTextBox.Name = "m_nameTextBox";
            this.m_nameTextBox.Size = new System.Drawing.Size(376, 20);
            this.m_nameTextBox.TabIndex = 1;
            this.m_nameTextBox.TextChanged += new System.EventHandler(this.m_nameTextBox_TextChanged);
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.tableLayoutPanel1);
            this.groupBox4.Location = new System.Drawing.Point(28, 89);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(565, 75);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Execute";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel1.Controls.Add(this.m_rb1stFailure, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_rbAlways, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_rbFailure, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.m_rbDisabled, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.m_rbSucces, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(559, 56);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // m_rb1stFailure
            // 
            this.m_rb1stFailure.AutoSize = true;
            this.m_rb1stFailure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rb1stFailure.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rb1stFailure.Location = new System.Drawing.Point(371, 3);
            this.m_rb1stFailure.Name = "m_rb1stFailure";
            this.m_rb1stFailure.Size = new System.Drawing.Size(185, 22);
            this.m_rb1stFailure.TabIndex = 4;
            this.m_rb1stFailure.TabStop = true;
            this.m_rb1stFailure.Text = "on 1st failure";
            this.m_rb1stFailure.UseVisualStyleBackColor = true;
            this.m_rb1stFailure.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbAlways
            // 
            this.m_rbAlways.AutoSize = true;
            this.m_rbAlways.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbAlways.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbAlways.Location = new System.Drawing.Point(3, 31);
            this.m_rbAlways.Name = "m_rbAlways";
            this.m_rbAlways.Size = new System.Drawing.Size(178, 22);
            this.m_rbAlways.TabIndex = 1;
            this.m_rbAlways.TabStop = true;
            this.m_rbAlways.Text = "always";
            this.m_rbAlways.UseVisualStyleBackColor = true;
            this.m_rbAlways.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbFailure
            // 
            this.m_rbFailure.AutoSize = true;
            this.m_rbFailure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbFailure.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbFailure.Location = new System.Drawing.Point(187, 31);
            this.m_rbFailure.Name = "m_rbFailure";
            this.m_rbFailure.Size = new System.Drawing.Size(178, 22);
            this.m_rbFailure.TabIndex = 3;
            this.m_rbFailure.TabStop = true;
            this.m_rbFailure.Text = "on failure";
            this.m_rbFailure.UseVisualStyleBackColor = true;
            this.m_rbFailure.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbDisabled
            // 
            this.m_rbDisabled.AutoSize = true;
            this.m_rbDisabled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbDisabled.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbDisabled.Location = new System.Drawing.Point(3, 3);
            this.m_rbDisabled.Name = "m_rbDisabled";
            this.m_rbDisabled.Size = new System.Drawing.Size(178, 22);
            this.m_rbDisabled.TabIndex = 0;
            this.m_rbDisabled.TabStop = true;
            this.m_rbDisabled.Text = "disabled";
            this.m_rbDisabled.UseVisualStyleBackColor = true;
            this.m_rbDisabled.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // m_rbSucces
            // 
            this.m_rbSucces.AutoSize = true;
            this.m_rbSucces.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbSucces.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbSucces.Location = new System.Drawing.Point(187, 3);
            this.m_rbSucces.Name = "m_rbSucces";
            this.m_rbSucces.Size = new System.Drawing.Size(178, 22);
            this.m_rbSucces.TabIndex = 2;
            this.m_rbSucces.TabStop = true;
            this.m_rbSucces.Text = "on success";
            this.m_rbSucces.UseVisualStyleBackColor = true;
            this.m_rbSucces.CheckedChanged += new System.EventHandler(this.m_whenRadioButton_CheckedChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox8.Controls.Add(this.tableLayoutPanel6);
            this.groupBox8.Location = new System.Drawing.Point(28, 533);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(565, 75);
            this.groupBox8.TabIndex = 6;
            this.groupBox8.TabStop = false;
            this.groupBox8.Text = "Notify";
            // 
            // tableLayoutPanel6
            // 
            this.tableLayoutPanel6.ColumnCount = 3;
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33F));
            this.tableLayoutPanel6.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 34F));
            this.tableLayoutPanel6.Controls.Add(this.m_rbNot1stFailure, 2, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotAlways, 0, 1);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotFailure, 1, 1);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotDisabled, 0, 0);
            this.tableLayoutPanel6.Controls.Add(this.m_rbNotSuccess, 1, 0);
            this.tableLayoutPanel6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel6.Location = new System.Drawing.Point(3, 16);
            this.tableLayoutPanel6.Name = "tableLayoutPanel6";
            this.tableLayoutPanel6.RowCount = 2;
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel6.Size = new System.Drawing.Size(559, 56);
            this.tableLayoutPanel6.TabIndex = 4;
            // 
            // m_rbNot1stFailure
            // 
            this.m_rbNot1stFailure.AutoSize = true;
            this.m_rbNot1stFailure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbNot1stFailure.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbNot1stFailure.Location = new System.Drawing.Point(371, 3);
            this.m_rbNot1stFailure.Name = "m_rbNot1stFailure";
            this.m_rbNot1stFailure.Size = new System.Drawing.Size(185, 22);
            this.m_rbNot1stFailure.TabIndex = 4;
            this.m_rbNot1stFailure.TabStop = true;
            this.m_rbNot1stFailure.Text = "on 1st failure";
            this.m_rbNot1stFailure.UseVisualStyleBackColor = true;
            // 
            // m_rbNotAlways
            // 
            this.m_rbNotAlways.AutoSize = true;
            this.m_rbNotAlways.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbNotAlways.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbNotAlways.Location = new System.Drawing.Point(3, 31);
            this.m_rbNotAlways.Name = "m_rbNotAlways";
            this.m_rbNotAlways.Size = new System.Drawing.Size(178, 22);
            this.m_rbNotAlways.TabIndex = 1;
            this.m_rbNotAlways.TabStop = true;
            this.m_rbNotAlways.Text = "always";
            this.m_rbNotAlways.UseVisualStyleBackColor = true;
            // 
            // m_rbNotFailure
            // 
            this.m_rbNotFailure.AutoSize = true;
            this.m_rbNotFailure.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbNotFailure.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbNotFailure.Location = new System.Drawing.Point(187, 31);
            this.m_rbNotFailure.Name = "m_rbNotFailure";
            this.m_rbNotFailure.Size = new System.Drawing.Size(178, 22);
            this.m_rbNotFailure.TabIndex = 3;
            this.m_rbNotFailure.TabStop = true;
            this.m_rbNotFailure.Text = "on failure";
            this.m_rbNotFailure.UseVisualStyleBackColor = true;
            // 
            // m_rbNotDisabled
            // 
            this.m_rbNotDisabled.AutoSize = true;
            this.m_rbNotDisabled.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbNotDisabled.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbNotDisabled.Location = new System.Drawing.Point(3, 3);
            this.m_rbNotDisabled.Name = "m_rbNotDisabled";
            this.m_rbNotDisabled.Size = new System.Drawing.Size(178, 22);
            this.m_rbNotDisabled.TabIndex = 0;
            this.m_rbNotDisabled.TabStop = true;
            this.m_rbNotDisabled.Text = "disabled";
            this.m_rbNotDisabled.UseVisualStyleBackColor = true;
            // 
            // m_rbNotSuccess
            // 
            this.m_rbNotSuccess.AutoSize = true;
            this.m_rbNotSuccess.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_rbNotSuccess.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_rbNotSuccess.Location = new System.Drawing.Point(187, 3);
            this.m_rbNotSuccess.Name = "m_rbNotSuccess";
            this.m_rbNotSuccess.Size = new System.Drawing.Size(178, 22);
            this.m_rbNotSuccess.TabIndex = 2;
            this.m_rbNotSuccess.TabStop = true;
            this.m_rbNotSuccess.Text = "on success";
            this.m_rbNotSuccess.UseVisualStyleBackColor = true;
            // 
            // m_pluginPanel
            // 
            this.m_pluginPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_pluginPanel.Location = new System.Drawing.Point(28, 170);
            this.m_pluginPanel.Name = "m_pluginPanel";
            this.m_pluginPanel.Size = new System.Drawing.Size(565, 357);
            this.m_pluginPanel.TabIndex = 7;
            // 
            // CustomTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_pluginPanel);
            this.Controls.Add(this.groupBox8);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox5);
            this.MinimumSize = new System.Drawing.Size(620, 620);
            this.Name = "CustomTaskControl";
            this.Size = new System.Drawing.Size(620, 620);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_nameTextBox;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.GroupBox groupBox8;
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
    }
}
