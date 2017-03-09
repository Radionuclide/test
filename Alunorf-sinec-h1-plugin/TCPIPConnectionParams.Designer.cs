namespace Alunorf_sinec_h1_plugin
{
    partial class TCPIPConnectionParams
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TCPIPConnectionParams));
            this.NQS1 = new System.Windows.Forms.GroupBox();
            this.m_lblAdress1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_tbPortNr1 = new System.Windows.Forms.TextBox();
            this.m_statusNQS1 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.NQS2 = new System.Windows.Forms.GroupBox();
            this.m_lblAdress2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_tbPortNr2 = new System.Windows.Forms.TextBox();
            this.m_statusNQS2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.NQS1.SuspendLayout();
            this.NQS2.SuspendLayout();
            this.SuspendLayout();
            // 
            // NQS1
            // 
            resources.ApplyResources(this.NQS1, "NQS1");
            this.NQS1.Controls.Add(this.m_lblAdress1);
            this.NQS1.Controls.Add(this.label3);
            this.NQS1.Controls.Add(this.m_tbPortNr1);
            this.NQS1.Controls.Add(this.m_statusNQS1);
            this.NQS1.Controls.Add(this.label8);
            this.NQS1.Name = "NQS1";
            this.NQS1.TabStop = false;
            // 
            // m_lblAdress1
            // 
            resources.ApplyResources(this.m_lblAdress1, "m_lblAdress1");
            this.m_lblAdress1.Name = "m_lblAdress1";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // m_tbPortNr1
            // 
            resources.ApplyResources(this.m_tbPortNr1, "m_tbPortNr1");
            this.m_tbPortNr1.Name = "m_tbPortNr1";
            // 
            // m_statusNQS1
            // 
            resources.ApplyResources(this.m_statusNQS1, "m_statusNQS1");
            this.m_statusNQS1.ForeColor = System.Drawing.Color.Red;
            this.m_statusNQS1.Name = "m_statusNQS1";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // NQS2
            // 
            resources.ApplyResources(this.NQS2, "NQS2");
            this.NQS2.Controls.Add(this.m_lblAdress2);
            this.NQS2.Controls.Add(this.label1);
            this.NQS2.Controls.Add(this.m_tbPortNr2);
            this.NQS2.Controls.Add(this.m_statusNQS2);
            this.NQS2.Controls.Add(this.label4);
            this.NQS2.Name = "NQS2";
            this.NQS2.TabStop = false;
            // 
            // m_lblAdress2
            // 
            resources.ApplyResources(this.m_lblAdress2, "m_lblAdress2");
            this.m_lblAdress2.Name = "m_lblAdress2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // m_tbPortNr2
            // 
            resources.ApplyResources(this.m_tbPortNr2, "m_tbPortNr2");
            this.m_tbPortNr2.Name = "m_tbPortNr2";
            // 
            // m_statusNQS2
            // 
            resources.ApplyResources(this.m_statusNQS2, "m_statusNQS2");
            this.m_statusNQS2.ForeColor = System.Drawing.Color.Red;
            this.m_statusNQS2.Name = "m_statusNQS2";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // TCPIPConnectionParams
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NQS2);
            this.Controls.Add(this.NQS1);
            this.Name = "TCPIPConnectionParams";
            this.NQS1.ResumeLayout(false);
            this.NQS1.PerformLayout();
            this.NQS2.ResumeLayout(false);
            this.NQS2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.GroupBox NQS1;
        public System.Windows.Forms.Label m_statusNQS1;
        public System.Windows.Forms.Label label8;
        public System.Windows.Forms.GroupBox NQS2;
        public System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox m_tbPortNr1;
        public System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox m_tbPortNr2;
        public System.Windows.Forms.Label m_statusNQS2;
        public System.Windows.Forms.Label label4;
        public System.Windows.Forms.Label m_lblAdress1;
        public System.Windows.Forms.Label m_lblAdress2;
    }
}
