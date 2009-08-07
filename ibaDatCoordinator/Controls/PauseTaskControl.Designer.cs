namespace iba.Controls
{
    partial class PauseTaskControl
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
            this.groupBox = new System.Windows.Forms.GroupBox();
            this.m_rbDatFileTime = new System.Windows.Forms.RadioButton();
            this.m_rbAbsolutePause = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.m_nudInterval = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox
            // 
            this.groupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox.Controls.Add(this.m_rbDatFileTime);
            this.groupBox.Controls.Add(this.m_rbAbsolutePause);
            this.groupBox.Controls.Add(this.label2);
            this.groupBox.Controls.Add(this.m_nudInterval);
            this.groupBox.Controls.Add(this.label1);
            this.groupBox.Controls.Add(this.label3);
            this.groupBox.Location = new System.Drawing.Point(0, 0);
            this.groupBox.Name = "groupBox";
            this.groupBox.Size = new System.Drawing.Size(565, 113);
            this.groupBox.TabIndex = 2;
            this.groupBox.TabStop = false;
            this.groupBox.Text = "Wait time";
            // 
            // m_rbDatFileTime
            // 
            this.m_rbDatFileTime.AutoSize = true;
            this.m_rbDatFileTime.Location = new System.Drawing.Point(35, 88);
            this.m_rbDatFileTime.Name = "m_rbDatFileTime";
            this.m_rbDatFileTime.Size = new System.Drawing.Size(182, 17);
            this.m_rbDatFileTime.TabIndex = 6;
            this.m_rbDatFileTime.TabStop = true;
            this.m_rbDatFileTime.Text = "last time the .dat file was modified";
            this.m_rbDatFileTime.UseVisualStyleBackColor = true;
            // 
            // m_rbAbsolutePause
            // 
            this.m_rbAbsolutePause.AutoSize = true;
            this.m_rbAbsolutePause.Location = new System.Drawing.Point(35, 67);
            this.m_rbAbsolutePause.Name = "m_rbAbsolutePause";
            this.m_rbAbsolutePause.Size = new System.Drawing.Size(130, 17);
            this.m_rbAbsolutePause.TabIndex = 5;
            this.m_rbAbsolutePause.TabStop = true;
            this.m_rbAbsolutePause.Text = "start of the pause task";
            this.m_rbAbsolutePause.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(282, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Pause until the specified amount of time has elapsed since";
            // 
            // m_nudInterval
            // 
            this.m_nudInterval.Location = new System.Drawing.Point(69, 21);
            this.m_nudInterval.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.m_nudInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudInterval.Name = "m_nudInterval";
            this.m_nudInterval.Size = new System.Drawing.Size(120, 20);
            this.m_nudInterval.TabIndex = 3;
            this.m_nudInterval.Value = new decimal(new int[] {
            30,
            0,
            0,
            0});
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(195, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "seconds";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(13, 23);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Duration:";
            // 
            // PauseTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox);
            this.Name = "PauseTaskControl";
            this.Size = new System.Drawing.Size(566, 120);
            this.groupBox.ResumeLayout(false);
            this.groupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudInterval)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown m_nudInterval;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton m_rbDatFileTime;
        private System.Windows.Forms.RadioButton m_rbAbsolutePause;
    }
}
