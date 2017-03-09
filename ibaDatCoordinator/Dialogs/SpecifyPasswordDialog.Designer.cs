namespace iba.Dialogs
{
    partial class SpecifyPasswordDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SpecifyPasswordDialog));
            this.m_tbPassConfirm = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_tbPass = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.m_btCancel = new System.Windows.Forms.Button();
            this.m_btOK = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // m_tbPassConfirm
            // 
            resources.ApplyResources(this.m_tbPassConfirm, "m_tbPassConfirm");
            this.m_tbPassConfirm.Name = "m_tbPassConfirm";
            this.m_tbPassConfirm.UseSystemPasswordChar = true;
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // m_tbPass
            // 
            resources.ApplyResources(this.m_tbPass, "m_tbPass");
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_btCancel
            // 
            this.m_btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.m_btCancel, "m_btCancel");
            this.m_btCancel.Name = "m_btCancel";
            this.m_btCancel.UseVisualStyleBackColor = true;
            this.m_btCancel.Click += new System.EventHandler(this.m_btCancel_Click);
            // 
            // m_btOK
            // 
            resources.ApplyResources(this.m_btOK, "m_btOK");
            this.m_btOK.Name = "m_btOK";
            this.m_btOK.UseVisualStyleBackColor = true;
            this.m_btOK.Click += new System.EventHandler(this.m_btOK_Click);
            // 
            // SpecifyPassword
            // 
            this.AcceptButton = this.m_btOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_btCancel;
            this.ControlBox = false;
            this.Controls.Add(this.m_btCancel);
            this.Controls.Add(this.m_btOK);
            this.Controls.Add(this.m_tbPassConfirm);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_tbPass);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SpecifyPassword";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox m_tbPassConfirm;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button m_btCancel;
        private System.Windows.Forms.Button m_btOK;
    }
}