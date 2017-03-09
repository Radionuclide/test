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
            this.m_textEditorUrl = new System.Windows.Forms.LinkLabel();
            this.m_vICSharpTextEditor = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::iba.Properties.Resources.ibalogo_transparent;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_vDATC
            // 
            resources.ApplyResources(this.m_vDATC, "m_vDATC");
            this.m_vDATC.Name = "m_vDATC";
            // 
            // m_vANAL
            // 
            resources.ApplyResources(this.m_vANAL, "m_vANAL");
            this.m_vANAL.Name = "m_vANAL";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // m_vFILES
            // 
            resources.ApplyResources(this.m_vFILES, "m_vFILES");
            this.m_vFILES.Name = "m_vFILES";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // m_vLogger
            // 
            resources.ApplyResources(this.m_vLogger, "m_vLogger");
            this.m_vLogger.Name = "m_vLogger";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // button1
            // 
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.button1, "button1");
            this.button1.Name = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // m_textEditorUrl
            // 
            resources.ApplyResources(this.m_textEditorUrl, "m_textEditorUrl");
            this.m_textEditorUrl.Name = "m_textEditorUrl";
            this.m_textEditorUrl.TabStop = true;
            this.m_textEditorUrl.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.m_textEditorUrl_LinkClicked);
            // 
            // m_vICSharpTextEditor
            // 
            resources.ApplyResources(this.m_vICSharpTextEditor, "m_vICSharpTextEditor");
            this.m_vICSharpTextEditor.Name = "m_vICSharpTextEditor";
            // 
            // AboutBox
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.button1;
            this.Controls.Add(this.m_vICSharpTextEditor);
            this.Controls.Add(this.m_textEditorUrl);
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
        private System.Windows.Forms.LinkLabel m_textEditorUrl;
        private System.Windows.Forms.Label m_vICSharpTextEditor;
    }
}