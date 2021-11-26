
namespace iba.Controls
{
    partial class DataTransferTaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataTransferTaskControl));
            this.m_gbOption = new iba.Utility.CollapsibleGroupBox();
            this.tableLayoutPanel3 = new System.Windows.Forms.TableLayoutPanel();
            this.panel5 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel5 = new System.Windows.Forms.TableLayoutPanel();
            this.m_lblRemotePath = new System.Windows.Forms.Label();
            this.m_tbRemotePath = new System.Windows.Forms.TextBox();
            this.m_cbDeleteAfterTransfer = new System.Windows.Forms.CheckBox();
            this.m_CreateZipArchive = new System.Windows.Forms.CheckBox();
            this.m_gbTarget = new iba.Utility.CollapsibleGroupBox();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.panel4 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.m_cbBandwidth = new System.Windows.Forms.ComboBox();
            this.m_chkLimitBandwidth = new System.Windows.Forms.CheckBox();
            this.m_numBandwidth = new System.Windows.Forms.NumericUpDown();
            this.m_btnCheckConnection = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.m_btnSelectServer = new System.Windows.Forms.Button();
            this.m_numericUpDownPort = new System.Windows.Forms.NumericUpDown();
            this.m_lbServer = new System.Windows.Forms.Label();
            this.m_lbPort = new System.Windows.Forms.Label();
            this.m_tbServer = new System.Windows.Forms.TextBox();
            this.m_gbSource = new iba.Utility.CollapsibleGroupBox();
            this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
            this.m_rbPrevOutput = new System.Windows.Forms.RadioButton();
            this.m_rbDatFile = new System.Windows.Forms.RadioButton();
            this.m_gbOption.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.panel5.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.m_gbTarget.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numBandwidth)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownPort)).BeginInit();
            this.m_gbSource.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_gbOption
            // 
            resources.ApplyResources(this.m_gbOption, "m_gbOption");
            this.m_gbOption.Controls.Add(this.tableLayoutPanel3);
            this.m_gbOption.Name = "m_gbOption";
            this.m_gbOption.TabStop = false;
            // 
            // tableLayoutPanel3
            // 
            resources.ApplyResources(this.tableLayoutPanel3, "tableLayoutPanel3");
            this.tableLayoutPanel3.Controls.Add(this.panel5, 0, 0);
            this.tableLayoutPanel3.Controls.Add(this.m_cbDeleteAfterTransfer, 0, 1);
            this.tableLayoutPanel3.Controls.Add(this.m_CreateZipArchive, 1, 1);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            // 
            // panel5
            // 
            resources.ApplyResources(this.panel5, "panel5");
            this.tableLayoutPanel3.SetColumnSpan(this.panel5, 2);
            this.panel5.Controls.Add(this.tableLayoutPanel5);
            this.panel5.Name = "panel5";
            // 
            // tableLayoutPanel5
            // 
            resources.ApplyResources(this.tableLayoutPanel5, "tableLayoutPanel5");
            this.tableLayoutPanel5.Controls.Add(this.m_lblRemotePath, 0, 0);
            this.tableLayoutPanel5.Controls.Add(this.m_tbRemotePath, 1, 0);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            // 
            // m_lblRemotePath
            // 
            resources.ApplyResources(this.m_lblRemotePath, "m_lblRemotePath");
            this.m_lblRemotePath.Name = "m_lblRemotePath";
            // 
            // m_tbRemotePath
            // 
            resources.ApplyResources(this.m_tbRemotePath, "m_tbRemotePath");
            this.m_tbRemotePath.Name = "m_tbRemotePath";
            // 
            // m_cbDeleteAfterTransfer
            // 
            resources.ApplyResources(this.m_cbDeleteAfterTransfer, "m_cbDeleteAfterTransfer");
            this.m_cbDeleteAfterTransfer.Name = "m_cbDeleteAfterTransfer";
            this.m_cbDeleteAfterTransfer.UseVisualStyleBackColor = true;
            // 
            // m_CreateZipArchive
            // 
            resources.ApplyResources(this.m_CreateZipArchive, "m_CreateZipArchive");
            this.m_CreateZipArchive.Name = "m_CreateZipArchive";
            this.m_CreateZipArchive.UseVisualStyleBackColor = true;
            // 
            // m_gbTarget
            // 
            resources.ApplyResources(this.m_gbTarget, "m_gbTarget");
            this.m_gbTarget.Controls.Add(this.tableLayoutPanel2);
            this.m_gbTarget.Controls.Add(this.tableLayoutPanel1);
            this.m_gbTarget.Name = "m_gbTarget";
            this.m_gbTarget.TabStop = false;
            // 
            // tableLayoutPanel2
            // 
            resources.ApplyResources(this.tableLayoutPanel2, "tableLayoutPanel2");
            this.tableLayoutPanel2.Controls.Add(this.panel4, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            // 
            // panel4
            // 
            resources.ApplyResources(this.panel4, "panel4");
            this.panel4.Name = "panel4";
            // 
            // panel3
            // 
            resources.ApplyResources(this.panel3, "panel3");
            this.panel3.Name = "panel3";
            // 
            // tableLayoutPanel1
            // 
            resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel1, 0, 1);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            // 
            // panel2
            // 
            resources.ApplyResources(this.panel2, "panel2");
            this.panel2.Controls.Add(this.m_cbBandwidth);
            this.panel2.Controls.Add(this.m_chkLimitBandwidth);
            this.panel2.Controls.Add(this.m_numBandwidth);
            this.panel2.Controls.Add(this.m_btnCheckConnection);
            this.panel2.Name = "panel2";
            // 
            // m_cbBandwidth
            // 
            this.m_cbBandwidth.FormattingEnabled = true;
            this.m_cbBandwidth.Items.AddRange(new object[] {
            resources.GetString("m_cbBandwidth.Items"),
            resources.GetString("m_cbBandwidth.Items1")});
            resources.ApplyResources(this.m_cbBandwidth, "m_cbBandwidth");
            this.m_cbBandwidth.Name = "m_cbBandwidth";
            this.m_cbBandwidth.SelectedIndexChanged += new System.EventHandler(this.m_cbBandwidth_SelectedIndexChanged);
            // 
            // m_chkLimitBandwidth
            // 
            resources.ApplyResources(this.m_chkLimitBandwidth, "m_chkLimitBandwidth");
            this.m_chkLimitBandwidth.Name = "m_chkLimitBandwidth";
            this.m_chkLimitBandwidth.UseVisualStyleBackColor = true;
            this.m_chkLimitBandwidth.CheckedChanged += new System.EventHandler(this.m_cbLimitBandwidth_CheckedChanged);
            // 
            // m_numBandwidth
            // 
            resources.ApplyResources(this.m_numBandwidth, "m_numBandwidth");
            this.m_numBandwidth.Name = "m_numBandwidth";
            // 
            // m_btnCheckConnection
            // 
            this.m_btnCheckConnection.Image = global::iba.Properties.Resources.thumup;
            resources.ApplyResources(this.m_btnCheckConnection, "m_btnCheckConnection");
            this.m_btnCheckConnection.Name = "m_btnCheckConnection";
            this.m_btnCheckConnection.UseVisualStyleBackColor = true;
            this.m_btnCheckConnection.Click += new System.EventHandler(this.m_btnCheckConnection_Click);
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Controls.Add(this.m_btnSelectServer);
            this.panel1.Controls.Add(this.m_numericUpDownPort);
            this.panel1.Controls.Add(this.m_lbServer);
            this.panel1.Controls.Add(this.m_lbPort);
            this.panel1.Controls.Add(this.m_tbServer);
            this.panel1.Name = "panel1";
            // 
            // m_btnSelectServer
            // 
            resources.ApplyResources(this.m_btnSelectServer, "m_btnSelectServer");
            this.m_btnSelectServer.Name = "m_btnSelectServer";
            this.m_btnSelectServer.UseVisualStyleBackColor = true;
            this.m_btnSelectServer.Click += new System.EventHandler(this.m_btnSelectServer_Click);
            // 
            // m_numericUpDownPort
            // 
            resources.ApplyResources(this.m_numericUpDownPort, "m_numericUpDownPort");
            this.m_numericUpDownPort.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.m_numericUpDownPort.Name = "m_numericUpDownPort";
            // 
            // m_lbServer
            // 
            resources.ApplyResources(this.m_lbServer, "m_lbServer");
            this.m_lbServer.Name = "m_lbServer";
            // 
            // m_lbPort
            // 
            resources.ApplyResources(this.m_lbPort, "m_lbPort");
            this.m_lbPort.Name = "m_lbPort";
            // 
            // m_tbServer
            // 
            resources.ApplyResources(this.m_tbServer, "m_tbServer");
            this.m_tbServer.Name = "m_tbServer";
            // 
            // m_gbSource
            // 
            resources.ApplyResources(this.m_gbSource, "m_gbSource");
            this.m_gbSource.Controls.Add(this.tableLayoutPanel4);
            this.m_gbSource.Name = "m_gbSource";
            this.m_gbSource.TabStop = false;
            // 
            // tableLayoutPanel4
            // 
            resources.ApplyResources(this.tableLayoutPanel4, "tableLayoutPanel4");
            this.tableLayoutPanel4.Controls.Add(this.m_rbPrevOutput, 1, 0);
            this.tableLayoutPanel4.Controls.Add(this.m_rbDatFile, 0, 0);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            // 
            // m_rbPrevOutput
            // 
            resources.ApplyResources(this.m_rbPrevOutput, "m_rbPrevOutput");
            this.m_rbPrevOutput.Name = "m_rbPrevOutput";
            this.m_rbPrevOutput.TabStop = true;
            this.m_rbPrevOutput.UseVisualStyleBackColor = true;
            // 
            // m_rbDatFile
            // 
            resources.ApplyResources(this.m_rbDatFile, "m_rbDatFile");
            this.m_rbDatFile.Name = "m_rbDatFile";
            this.m_rbDatFile.TabStop = true;
            this.m_rbDatFile.UseVisualStyleBackColor = true;
            // 
            // DataTransferTaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_gbOption);
            this.Controls.Add(this.m_gbTarget);
            this.Controls.Add(this.m_gbSource);
            this.Name = "DataTransferTaskControl";
            this.m_gbOption.ResumeLayout(false);
            this.m_gbOption.PerformLayout();
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel5.PerformLayout();
            this.m_gbTarget.ResumeLayout(false);
            this.m_gbTarget.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numBandwidth)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_numericUpDownPort)).EndInit();
            this.m_gbSource.ResumeLayout(false);
            this.m_gbSource.PerformLayout();
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Utility.CollapsibleGroupBox m_gbTarget;
        private Utility.CollapsibleGroupBox m_gbSource;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
        private System.Windows.Forms.RadioButton m_rbPrevOutput;
        private System.Windows.Forms.RadioButton m_rbDatFile;
        private Utility.CollapsibleGroupBox m_gbOption;
        private System.Windows.Forms.Label m_lbServer;
        private System.Windows.Forms.TextBox m_tbServer;
        private System.Windows.Forms.Label m_lbPort;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button m_btnCheckConnection;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel3;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel5;
        private System.Windows.Forms.Label m_lblRemotePath;
        private System.Windows.Forms.TextBox m_tbRemotePath;
        private System.Windows.Forms.NumericUpDown m_numericUpDownPort;
        private System.Windows.Forms.Button m_btnSelectServer;
        private System.Windows.Forms.CheckBox m_cbDeleteAfterTransfer;
        private System.Windows.Forms.ComboBox m_cbBandwidth;
        private System.Windows.Forms.CheckBox m_chkLimitBandwidth;
        private System.Windows.Forms.NumericUpDown m_numBandwidth;
        private System.Windows.Forms.CheckBox m_CreateZipArchive;
    }
}
