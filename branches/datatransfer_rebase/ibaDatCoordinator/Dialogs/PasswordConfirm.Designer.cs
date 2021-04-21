namespace iba.Dialogs
{
    partial class PasswordConfirm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordConfirm));
            this.m_btOK = new System.Windows.Forms.Button();
            this.m_btCancel = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.m_tbPass = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // m_btOK
            // 
            resources.ApplyResources(this.m_btOK, "m_btOK");
            this.m_btOK.Name = "m_btOK";
            this.m_btOK.UseVisualStyleBackColor = true;
            this.m_btOK.Click += new System.EventHandler(this.m_btOK_Click);
            // 
            // m_btCancel
            // 
            this.m_btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.m_btCancel, "m_btCancel");
            this.m_btCancel.Name = "m_btCancel";
            this.m_btCancel.UseVisualStyleBackColor = true;
            this.m_btCancel.Click += new System.EventHandler(this.m_btCancel_Click);
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
            // m_tbPass
            // 
            resources.ApplyResources(this.m_tbPass, "m_tbPass");
            this.m_tbPass.Name = "m_tbPass";
            this.m_tbPass.UseSystemPasswordChar = true;
            // 
            // PasswordConfirm
            // 
            this.AcceptButton = this.m_btOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.m_btCancel;
            this.ControlBox = false;
            this.Controls.Add(this.m_tbPass);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_btCancel);
            this.Controls.Add(this.m_btOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PasswordConfirm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button m_btOK;
        private System.Windows.Forms.Button m_btCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_tbPass;
    }
}