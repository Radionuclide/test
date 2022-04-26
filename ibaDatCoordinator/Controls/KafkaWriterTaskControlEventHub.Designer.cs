
namespace iba.Controls
{
    partial class KafkaWriterTaskControlEventHub
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
            this.clusterAddressLabel = new System.Windows.Forms.Label();
            this.addressTextBox = new System.Windows.Forms.TextBox();
            this.enableSSLVerificationCb = new System.Windows.Forms.CheckBox();
            this.CACertPlaceholder = new System.Windows.Forms.ComboBox();
            this.CACertificateLabel = new System.Windows.Forms.Label();
            this.secLabel = new System.Windows.Forms.Label();
            this.timeoutNumericUpDown = new System.Windows.Forms.NumericUpDown();
            this.messageTimeoutLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericUpDown)).BeginInit();
            this.SuspendLayout();
            // 
            // clusterAddressLabel
            // 
            this.clusterAddressLabel.AutoSize = true;
            this.clusterAddressLabel.Location = new System.Drawing.Point(11, 8);
            this.clusterAddressLabel.Name = "clusterAddressLabel";
            this.clusterAddressLabel.Size = new System.Drawing.Size(122, 17);
            this.clusterAddressLabel.TabIndex = 22;
            this.clusterAddressLabel.Text = "Connection string:";
            // 
            // addressTextBox
            // 
            this.addressTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.addressTextBox.Location = new System.Drawing.Point(279, 8);
            this.addressTextBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.addressTextBox.Name = "addressTextBox";
            this.addressTextBox.Size = new System.Drawing.Size(587, 22);
            this.addressTextBox.TabIndex = 21;
            // 
            // enableSSLVerificationCb
            // 
            this.enableSSLVerificationCb.AutoSize = true;
            this.enableSSLVerificationCb.Location = new System.Drawing.Point(14, 44);
            this.enableSSLVerificationCb.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.enableSSLVerificationCb.Name = "enableSSLVerificationCb";
            this.enableSSLVerificationCb.Size = new System.Drawing.Size(176, 21);
            this.enableSSLVerificationCb.TabIndex = 80;
            this.enableSSLVerificationCb.Text = "Enable SSL verification";
            this.enableSSLVerificationCb.UseVisualStyleBackColor = true;
            this.enableSSLVerificationCb.CheckedChanged += new System.EventHandler(this.enableSSLVerificationCb_CheckedChanged);
            // 
            // CACertPlaceholder
            // 
            this.CACertPlaceholder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CACertPlaceholder.FormattingEnabled = true;
            this.CACertPlaceholder.Location = new System.Drawing.Point(279, 79);
            this.CACertPlaceholder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.CACertPlaceholder.Name = "CACertPlaceholder";
            this.CACertPlaceholder.Size = new System.Drawing.Size(587, 24);
            this.CACertPlaceholder.TabIndex = 85;
            // 
            // CACertificateLabel
            // 
            this.CACertificateLabel.AutoSize = true;
            this.CACertificateLabel.Location = new System.Drawing.Point(11, 82);
            this.CACertificateLabel.Name = "CACertificateLabel";
            this.CACertificateLabel.Size = new System.Drawing.Size(95, 17);
            this.CACertificateLabel.TabIndex = 84;
            this.CACertificateLabel.Text = "CA certificate:";
            // 
            // secLabel
            // 
            this.secLabel.AutoSize = true;
            this.secLabel.Location = new System.Drawing.Point(340, 113);
            this.secLabel.Name = "secLabel";
            this.secLabel.Size = new System.Drawing.Size(15, 17);
            this.secLabel.TabIndex = 99;
            this.secLabel.Text = "s";
            // 
            // timeoutNumericUpDown
            // 
            this.timeoutNumericUpDown.Location = new System.Drawing.Point(279, 111);
            this.timeoutNumericUpDown.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.timeoutNumericUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.timeoutNumericUpDown.Name = "timeoutNumericUpDown";
            this.timeoutNumericUpDown.Size = new System.Drawing.Size(55, 22);
            this.timeoutNumericUpDown.TabIndex = 97;
            this.timeoutNumericUpDown.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // messageTimeoutLabel
            // 
            this.messageTimeoutLabel.AutoSize = true;
            this.messageTimeoutLabel.Location = new System.Drawing.Point(11, 116);
            this.messageTimeoutLabel.Name = "messageTimeoutLabel";
            this.messageTimeoutLabel.Size = new System.Drawing.Size(119, 17);
            this.messageTimeoutLabel.TabIndex = 98;
            this.messageTimeoutLabel.Text = "Message timeout:";
            // 
            // KafkaWriterTaskControlEventHub
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.clusterAddressLabel);
            this.Controls.Add(this.secLabel);
            this.Controls.Add(this.addressTextBox);
            this.Controls.Add(this.CACertPlaceholder);
            this.Controls.Add(this.timeoutNumericUpDown);
            this.Controls.Add(this.CACertificateLabel);
            this.Controls.Add(this.enableSSLVerificationCb);
            this.Controls.Add(this.messageTimeoutLabel);
            this.Name = "KafkaWriterTaskControlEventHub";
            this.Size = new System.Drawing.Size(910, 151);
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumericUpDown)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label clusterAddressLabel;
        private System.Windows.Forms.TextBox addressTextBox;
        private System.Windows.Forms.CheckBox enableSSLVerificationCb;
        private System.Windows.Forms.ComboBox CACertPlaceholder;
        private System.Windows.Forms.Label CACertificateLabel;
        private System.Windows.Forms.Label secLabel;
        private System.Windows.Forms.NumericUpDown timeoutNumericUpDown;
        private System.Windows.Forms.Label messageTimeoutLabel;
    }
}
