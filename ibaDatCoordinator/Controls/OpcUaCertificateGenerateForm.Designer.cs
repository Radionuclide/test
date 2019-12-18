namespace iba.Controls
{
    partial class OpcUaCertificateGenerateForm
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
            if(disposing && (components != null))
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpcUaCertificateGenerateForm));
            this.tbCertName = new System.Windows.Forms.TextBox();
            this.tbAppUri = new System.Windows.Forms.TextBox();
            this.numLifeTime = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.cbCertAlgorithm = new System.Windows.Forms.ComboBox();
            this.btOk = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numLifeTime)).BeginInit();
            this.SuspendLayout();
            // 
            // tbCertName
            // 
            resources.ApplyResources(this.tbCertName, "tbCertName");
            this.tbCertName.Name = "tbCertName";
            // 
            // tbAppUri
            // 
            resources.ApplyResources(this.tbAppUri, "tbAppUri");
            this.tbAppUri.Name = "tbAppUri";
            // 
            // numLifeTime
            // 
            resources.ApplyResources(this.numLifeTime, "numLifeTime");
            this.numLifeTime.Maximum = new decimal(new int[] {
            240,
            0,
            0,
            0});
            this.numLifeTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numLifeTime.Name = "numLifeTime";
            this.numLifeTime.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
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
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // cbCertAlgorithm
            // 
            this.cbCertAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbCertAlgorithm.FormattingEnabled = true;
            resources.ApplyResources(this.cbCertAlgorithm, "cbCertAlgorithm");
            this.cbCertAlgorithm.Name = "cbCertAlgorithm";
            // 
            // btOk
            // 
            resources.ApplyResources(this.btOk, "btOk");
            this.btOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btOk.Name = "btOk";
            this.btOk.UseVisualStyleBackColor = true;
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Name = "btCancel";
            this.btCancel.UseVisualStyleBackColor = true;
            // 
            // OpcUaCertificateGenerateForm
            // 
            this.AcceptButton = this.btOk;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOk);
            this.Controls.Add(this.cbCertAlgorithm);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.numLifeTime);
            this.Controls.Add(this.tbAppUri);
            this.Controls.Add(this.tbCertName);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OpcUaCertificateGenerateForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.numLifeTime)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbCertName;
        private System.Windows.Forms.TextBox tbAppUri;
        private System.Windows.Forms.NumericUpDown numLifeTime;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cbCertAlgorithm;
        private System.Windows.Forms.Button btOk;
        private System.Windows.Forms.Button btCancel;
    }
}