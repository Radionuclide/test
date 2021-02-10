
namespace iba.Controls
{
    partial class ErrorPluginTaskControl
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
            this.lbError = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbError
            // 
            this.lbError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbError.Location = new System.Drawing.Point(0, 0);
            this.lbError.Name = "lbError";
            this.lbError.Size = new System.Drawing.Size(869, 554);
            this.lbError.TabIndex = 0;
            this.lbError.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ErrorPluginTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbError);
            this.Name = "ErrorPluginTaskControl";
            this.Size = new System.Drawing.Size(869, 554);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbError;
    }
}
