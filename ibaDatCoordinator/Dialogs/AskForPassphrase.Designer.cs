
namespace iba.Dialogs
{
    partial class AskForPassphrase
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
            this.textBox = new System.Windows.Forms.TextBox();
            this.textLabel = new System.Windows.Forms.Label();
            this.confirmationBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // textBox
            // 
            this.textBox.Location = new System.Drawing.Point(50, 50);
            this.textBox.MaximumSize = new System.Drawing.Size(400, 0);
            this.textBox.Name = "textBox";
            this.textBox.Size = new System.Drawing.Size(400, 20);
            this.textBox.TabIndex = 1;
            this.textBox.UseSystemPasswordChar = true;
            // 
            // textLabel
            // 
            this.textLabel.AutoSize = true;
            this.textLabel.Location = new System.Drawing.Point(50, 20);
            this.textLabel.Name = "textLabel";
            this.textLabel.Size = new System.Drawing.Size(35, 13);
            this.textLabel.TabIndex = 2;
            this.textLabel.Text = "label1";
            // 
            // confirmationBtn
            // 
            this.confirmationBtn.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.confirmationBtn.Location = new System.Drawing.Point(350, 75);
            this.confirmationBtn.Name = "confirmationBtn";
            this.confirmationBtn.Size = new System.Drawing.Size(100, 23);
            this.confirmationBtn.TabIndex = 3;
            this.confirmationBtn.Text = "OK";
            this.confirmationBtn.UseVisualStyleBackColor = true;
            // 
            // AskForPassphrase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 111);
            this.Controls.Add(this.confirmationBtn);
            this.Controls.Add(this.textLabel);
            this.Controls.Add(this.textBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "AskForPassphrase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AskForPassphrase";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBox;
        private System.Windows.Forms.Label textLabel;
        private System.Windows.Forms.Button confirmationBtn;
    }
}