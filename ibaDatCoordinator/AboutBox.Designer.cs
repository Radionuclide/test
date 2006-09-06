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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutBox));
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
            this.pictureBox1.AccessibleDescription = null;
            this.pictureBox1.AccessibleName = null;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.BackgroundImage = null;
            this.pictureBox1.Font = null;
            this.pictureBox1.Image = global::iba.Properties.Resources.ibalogo_transparent;
            this.pictureBox1.ImageLocation = null;
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            this.label1.AccessibleDescription = null;
            this.label1.AccessibleName = null;
            resources.ApplyResources(this.label1, "label1");
            this.label1.Font = null;
            this.label1.Name = "label1";
            // 
            // label2
            // 
            this.label2.AccessibleDescription = null;
            this.label2.AccessibleName = null;
            resources.ApplyResources(this.label2, "label2");
            this.label2.Font = null;
            this.label2.Name = "label2";
            // 
            // m_vDATC
            // 
            this.m_vDATC.AccessibleDescription = null;
            this.m_vDATC.AccessibleName = null;
            resources.ApplyResources(this.m_vDATC, "m_vDATC");
            this.m_vDATC.Font = null;
            this.m_vDATC.Name = "m_vDATC";
            // 
            // m_vANAL
            // 
            this.m_vANAL.AccessibleDescription = null;
            this.m_vANAL.AccessibleName = null;
            resources.ApplyResources(this.m_vANAL, "m_vANAL");
            this.m_vANAL.Font = null;
            this.m_vANAL.Name = "m_vANAL";
            // 
            // label5
            // 
            this.label5.AccessibleDescription = null;
            this.label5.AccessibleName = null;
            resources.ApplyResources(this.label5, "label5");
            this.label5.Font = null;
            this.label5.Name = "label5";
            // 
            // m_vFILES
            // 
            this.m_vFILES.AccessibleDescription = null;
            this.m_vFILES.AccessibleName = null;
            resources.ApplyResources(this.m_vFILES, "m_vFILES");
            this.m_vFILES.Font = null;
            this.m_vFILES.Name = "m_vFILES";
            // 
            // label7
            // 
            this.label7.AccessibleDescription = null;
            this.label7.AccessibleName = null;
            resources.ApplyResources(this.label7, "label7");
            this.label7.Font = null;
            this.label7.Name = "label7";
            // 
            // m_vLogger
            // 
            this.m_vLogger.AccessibleDescription = null;
            this.m_vLogger.AccessibleName = null;
            resources.ApplyResources(this.m_vLogger, "m_vLogger");
            this.m_vLogger.Font = null;
            this.m_vLogger.Name = "m_vLogger";
            // 
            // label9
            // 
            this.label9.AccessibleDescription = null;
            this.label9.AccessibleName = null;
            resources.ApplyResources(this.label9, "label9");
            this.label9.Font = null;
            this.label9.Name = "label9";
            // 
            // button1
            // 
            this.button1.AccessibleDescription = null;
            this.button1.AccessibleName = null;
            resources.ApplyResources(this.button1, "button1");
            this.button1.BackgroundImage = null;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Font = null;
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // AboutBox
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.CancelButton = this.button1;
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
            this.Font = null;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = null;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AboutBox";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
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