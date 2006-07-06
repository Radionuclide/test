namespace iba.Controls
{
    partial class CopyControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CopyControl));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rb1stFailure = new System.Windows.Forms.RadioButton();
            this.m_rbAlways = new System.Windows.Forms.RadioButton();
            this.m_rbFailure = new System.Windows.Forms.RadioButton();
            this.m_rbDisabled = new System.Windows.Forms.RadioButton();
            this.m_rbSucces = new System.Windows.Forms.RadioButton();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_nameTextBox = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_subfolderGroupBox = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_folderNumber = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbOriginal = new System.Windows.Forms.RadioButton();
            this.m_rbNONE = new System.Windows.Forms.RadioButton();
            this.m_rbHour = new System.Windows.Forms.RadioButton();
            this.m_rbDay = new System.Windows.Forms.RadioButton();
            this.m_rbWeek = new System.Windows.Forms.RadioButton();
            this.m_rbMonth = new System.Windows.Forms.RadioButton();
            this.m_browseFolderButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.m_targetFolderTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.radioButton2 = new System.Windows.Forms.RadioButton();
            this.radioButton3 = new System.Windows.Forms.RadioButton();
            this.radioButton4 = new System.Windows.Forms.RadioButton();
            this.radioButton5 = new System.Windows.Forms.RadioButton();
            this.radioButton6 = new System.Windows.Forms.RadioButton();
            this.groupBox6 = new System.Windows.Forms.GroupBox();
            this.m_cbRemoveSource = new System.Windows.Forms.CheckBox();
            this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.tableLayoutPanel6 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbNot1stFailure = new System.Windows.Forms.RadioButton();
            this.m_rbNotAlways = new System.Windows.Forms.RadioButton();
            this.m_rbNotFailure = new System.Windows.Forms.RadioButton();
            this.m_rbNotDisabled = new System.Windows.Forms.RadioButton();
            this.m_rbNotSuccess = new System.Windows.Forms.RadioButton();
            this.groupBox4.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.m_subfolderGroupBox.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_folderNumber)).BeginInit();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.tableLayoutPanel5.SuspendLayout();
            this.groupBox6.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tableLayoutPanel6.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            resources.ApplyResources(this.groupBox4, "groupBox4");
            this.groupBox4.Controls.Add(this.tableLayoutPanel1);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.TabStop = false;
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
            // groupBox3
            // 
            resources.ApplyResources(this.groupBox3, "groupBox3");
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Controls.Add(this.m_nameTextBox);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // m_nameTextBox
            // 
            resources.ApplyResources(this.m_nameTextBox, "m_nameTextBox");
            this.m_nameTextBox.Name = "m_nameTextBox";
            this.m_nameTextBox.TextChanged += new System.EventHandler(this.m_nameTextBox_TextChanged);
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.m_subfolderGroupBox);
            this.groupBox2.Controls.Add(this.m_browseFolderButton);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.m_targetFolderTextBox);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_subfolderGroupBox
            // 
            resources.ApplyResources(this.m_subfolderGroupBox, "m_subfolderGroupBox");
            this.m_subfolderGroupBox.Controls.Add(this.tableLayoutPanel2);
            this.m_subfolderGroupBox.Name = "m_subfolderGroupBox";
            this.m_subfolderGroupBox.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.tableLayoutPanel3, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.m_folderNumber);
            this.panel1.Controls.Add(this.label2);
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // m_folderNumber
            // 
            resources.ApplyResources(this.m_folderNumber, "m_folderNumber");
            this.m_folderNumber.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.m_folderNumber.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_folderNumber.Name = "m_folderNumber";
            this.m_folderNumber.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.m_rbOriginal, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_rbNONE, 2, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_rbHour, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbDay, 1, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbWeek, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_rbMonth, 0, 1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // m_rbOriginal
            // 
            resources.ApplyResources(this.m_rbOriginal, "m_rbOriginal");
            this.m_rbOriginal.Name = "m_rbOriginal";
            this.m_rbOriginal.TabStop = true;
            this.m_rbOriginal.UseVisualStyleBackColor = true;
            // 
            // m_rbNONE
            // 
            resources.ApplyResources(this.m_rbNONE, "m_rbNONE");
            this.m_rbNONE.Name = "m_rbNONE";
            this.m_rbNONE.TabStop = true;
            this.m_rbNONE.UseVisualStyleBackColor = true;
            // 
            // m_rbHour
            // 
            resources.ApplyResources(this.m_rbHour, "m_rbHour");
            this.m_rbHour.Name = "m_rbHour";
            this.m_rbHour.TabStop = true;
            this.m_rbHour.UseVisualStyleBackColor = true;
            // 
            // m_rbDay
            // 
            resources.ApplyResources(this.m_rbDay, "m_rbDay");
            this.m_rbDay.Name = "m_rbDay";
            this.m_rbDay.TabStop = true;
            this.m_rbDay.UseVisualStyleBackColor = true;
            // 
            // m_rbWeek
            // 
            resources.ApplyResources(this.m_rbWeek, "m_rbWeek");
            this.m_rbWeek.Name = "m_rbWeek";
            this.m_rbWeek.TabStop = true;
            this.m_rbWeek.UseVisualStyleBackColor = true;
            // 
            // m_rbMonth
            // 
            resources.ApplyResources(this.m_rbMonth, "m_rbMonth");
            this.m_rbMonth.Name = "m_rbMonth";
            this.m_rbMonth.TabStop = true;
            this.m_rbMonth.UseVisualStyleBackColor = true;
            // 
            // m_browseFolderButton
            // 
            resources.ApplyResources(this.m_browseFolderButton, "m_browseFolderButton");
            this.m_browseFolderButton.Image = global::iba.Properties.Resources.open;
            this.m_browseFolderButton.Name = "m_browseFolderButton";
            this.m_browseFolderButton.UseVisualStyleBackColor = true;
            this.m_browseFolderButton.Click += new System.EventHandler(this.m_browseFolderButton_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_targetFolderTextBox
            // 
            resources.ApplyResources(this.m_targetFolderTextBox, "m_targetFolderTextBox");
            this.m_targetFolderTextBox.Name = "m_targetFolderTextBox";
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // groupBox5
            // 
            resources.ApplyResources(this.groupBox5, "groupBox5");
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.panel2, 0, 1);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.numericUpDown1);
            this.panel2.Controls.Add(this.label3);
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Name = "panel2";
            // 
            // numericUpDown1
            // 
            resources.ApplyResources(this.numericUpDown1, "numericUpDown1");
            this.numericUpDown1.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.radioButton1, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.radioButton2, 1, 0);
            this.tableLayoutPanel5.Controls.Add(this.radioButton3, 2, 0);
            this.tableLayoutPanel5.Controls.Add(this.radioButton4, 2, 1);
            this.tableLayoutPanel5.Controls.Add(this.radioButton5, 1, 1);
            this.tableLayoutPanel5.Controls.Add(this.radioButton6, 0, 1);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // radioButton1
            // 
            resources.ApplyResources(this.radioButton1, "radioButton1");
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.TabStop = true;
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // radioButton2
            // 
            resources.ApplyResources(this.radioButton2, "radioButton2");
            this.radioButton2.Name = "radioButton2";
            this.radioButton2.TabStop = true;
            this.radioButton2.UseVisualStyleBackColor = true;
            // 
            // radioButton3
            // 
            resources.ApplyResources(this.radioButton3, "radioButton3");
            this.radioButton3.Name = "radioButton3";
            this.radioButton3.TabStop = true;
            this.radioButton3.UseVisualStyleBackColor = true;
            // 
            // radioButton4
            // 
            resources.ApplyResources(this.radioButton4, "radioButton4");
            this.radioButton4.Name = "radioButton4";
            this.radioButton4.TabStop = true;
            this.radioButton4.UseVisualStyleBackColor = true;
            // 
            // radioButton5
            // 
            resources.ApplyResources(this.radioButton5, "radioButton5");
            this.radioButton5.Name = "radioButton5";
            this.radioButton5.TabStop = true;
            this.radioButton5.UseVisualStyleBackColor = true;
            // 
            // radioButton6
            // 
            resources.ApplyResources(this.radioButton6, "radioButton6");
            this.radioButton6.Name = "radioButton6";
            this.radioButton6.TabStop = true;
            this.radioButton6.UseVisualStyleBackColor = true;
            // 
            // groupBox6
            // 
            resources.ApplyResources(this.groupBox6, "groupBox6");
            this.groupBox6.Controls.Add(this.m_cbRemoveSource);
            this.groupBox6.Name = "groupBox6";
            this.groupBox6.TabStop = false;
            // 
            // m_cbRemoveSource
            // 
            resources.ApplyResources(this.m_cbRemoveSource, "m_cbRemoveSource");
            this.m_cbRemoveSource.Name = "m_cbRemoveSource";
            this.m_cbRemoveSource.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            resources.ApplyResources(this.groupBox7, "groupBox7");
            this.groupBox7.Controls.Add(this.tableLayoutPanel6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.TabStop = false;
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
            // CopyControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox7);
            this.Controls.Add(this.groupBox6);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.MinimumSize = new System.Drawing.Size(620, 545);
            this.Name = "CopyControl";
            this.groupBox4.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.m_subfolderGroupBox.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_folderNumber)).EndInit();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.groupBox6.ResumeLayout(false);
            this.groupBox6.PerformLayout();
            this.groupBox7.ResumeLayout(false);
            this.tableLayoutPanel6.ResumeLayout(false);
            this.tableLayoutPanel6.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RadioButton m_rb1stFailure;
        private System.Windows.Forms.RadioButton m_rbAlways;
        private System.Windows.Forms.RadioButton m_rbFailure;
        private System.Windows.Forms.RadioButton m_rbDisabled;
        private System.Windows.Forms.RadioButton m_rbSucces;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox m_nameTextBox;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox m_subfolderGroupBox;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.NumericUpDown m_folderNumber;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.RadioButton m_rbNONE;
        private System.Windows.Forms.RadioButton m_rbHour;
        private System.Windows.Forms.RadioButton m_rbMonth;
        private System.Windows.Forms.RadioButton m_rbDay;
        private System.Windows.Forms.RadioButton m_rbWeek;
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_targetFolderTextBox;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton radioButton2;
        private System.Windows.Forms.RadioButton radioButton3;
        private System.Windows.Forms.RadioButton radioButton4;
        private System.Windows.Forms.RadioButton radioButton5;
        private System.Windows.Forms.RadioButton radioButton6;
        private System.Windows.Forms.GroupBox groupBox6;
        private System.Windows.Forms.CheckBox m_cbRemoveSource;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel6;
        private System.Windows.Forms.RadioButton m_rbNot1stFailure;
        private System.Windows.Forms.RadioButton m_rbNotAlways;
        private System.Windows.Forms.RadioButton m_rbNotFailure;
        private System.Windows.Forms.RadioButton m_rbNotDisabled;
        private System.Windows.Forms.RadioButton m_rbNotSuccess;
        private System.Windows.Forms.RadioButton m_rbOriginal;
    }
}
