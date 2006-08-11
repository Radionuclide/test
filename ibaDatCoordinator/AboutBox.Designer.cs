namespace iba
{
    partial class AboutBox
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_vDATC = new System.Windows.Forms.Label();
            this.m_vANAL = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.m_vFILES = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_vLogger = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::iba.Properties.Resources.ibalogo_transparent;
            this.pictureBox1.Location = new System.Drawing.Point(24, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(62, 65);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 26);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "ibaDatCoordinator";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(119, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(152, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Copyright © 2005-2006 iba-AG";
            // 
            // m_vDATC
            // 
            this.m_vDATC.AutoSize = true;
            this.m_vDATC.Location = new System.Drawing.Point(217, 26);
            this.m_vDATC.Name = "m_vDATC";
            this.m_vDATC.Size = new System.Drawing.Size(42, 13);
            this.m_vDATC.TabIndex = 3;
            this.m_vDATC.Text = "vDATC";
            // 
            // m_vANAL
            // 
            this.m_vANAL.AutoSize = true;
            this.m_vANAL.Location = new System.Drawing.Point(104, 99);
            this.m_vANAL.Name = "m_vANAL";
            this.m_vANAL.Size = new System.Drawing.Size(35, 13);
            this.m_vANAL.TabIndex = 5;
            this.m_vANAL.Text = "vANA";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(21, 99);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(61, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "ibaAnalyzer";
            // 
            // m_vFILES
            // 
            this.m_vFILES.AutoSize = true;
            this.m_vFILES.Location = new System.Drawing.Point(104, 121);
            this.m_vFILES.Name = "m_vFILES";
            this.m_vFILES.Size = new System.Drawing.Size(35, 13);
            this.m_vFILES.TabIndex = 7;
            this.m_vFILES.Text = "vFILE";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(21, 121);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(42, 13);
            this.label7.TabIndex = 6;
            this.label7.Text = "ibaFiles";
            // 
            // m_vLogger
            // 
            this.m_vLogger.AutoSize = true;
            this.m_vLogger.Location = new System.Drawing.Point(104, 143);
            this.m_vLogger.Name = "m_vLogger";
            this.m_vLogger.Size = new System.Drawing.Size(58, 13);
            this.m_vLogger.TabIndex = 9;
            this.m_vLogger.Text = "vLOGGER";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(21, 143);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(54, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "ibaLogger";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(196, 138);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AboutBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.ClientSize = new System.Drawing.Size(288, 175);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.m_vLogger);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.m_vFILES);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.m_vANAL);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.m_vDATC);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "About ibaDatCoordinator";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label m_vDATC;
        private System.Windows.Forms.Label m_vANAL;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label m_vFILES;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label m_vLogger;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button button1;
    }
}