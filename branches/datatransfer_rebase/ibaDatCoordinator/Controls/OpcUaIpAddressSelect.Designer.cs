namespace iba.Controls
{
	partial class OpcUaIpAddressSelect
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
            this.cbItfs = new System.Windows.Forms.ComboBox();
            this.cbIps = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cbItfs
            // 
            this.cbItfs.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbItfs.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbItfs.FormattingEnabled = true;
            this.cbItfs.Location = new System.Drawing.Point(135, 0);
            this.cbItfs.Name = "cbItfs";
            this.cbItfs.Size = new System.Drawing.Size(245, 21);
            this.cbItfs.TabIndex = 8;
            this.cbItfs.SelectedIndexChanged += new System.EventHandler(this.OnSelectedInterfaceChanged);
            // 
            // cbIps
            // 
            this.cbIps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbIps.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbIps.FormattingEnabled = true;
            this.cbIps.Location = new System.Drawing.Point(135, 27);
            this.cbIps.Name = "cbIps";
            this.cbIps.Size = new System.Drawing.Size(245, 21);
            this.cbIps.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(0, 30);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "IP address";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(0, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 13);
            this.label2.TabIndex = 12;
            this.label2.Text = "Network interface";
            // 
            // IPAddressSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cbItfs);
            this.Controls.Add(this.cbIps);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Name = "OpcUaIpAddressSelect";
            this.Size = new System.Drawing.Size(380, 48);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion
		private System.Windows.Forms.ComboBox cbItfs;
		private System.Windows.Forms.ComboBox cbIps;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
	}
}
