namespace iba.Controls
{
    partial class HDEventCreationTaskControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(HDEventCreationTaskControl));
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.gbDataSource = new iba.Utility.CollapsibleGroupBox();
            this.m_btnTest = new System.Windows.Forms.Button();
            this.m_btnUploadPDO = new System.Windows.Forms.Button();
            this.m_tbPwdDAT = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_btnOpenPDO = new System.Windows.Forms.Button();
            this.m_btnBrowseDAT = new System.Windows.Forms.Button();
            this.m_btnBrowsePDO = new System.Windows.Forms.Button();
            this.m_tbDAT = new System.Windows.Forms.TextBox();
            this.m_tbPDO = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gbEvent = new iba.Utility.CollapsibleGroupBox();
            this.m_ctrlEvent = new iba.HD.Client.ControlEventConfig();
            this.gbStoreSelection = new System.Windows.Forms.GroupBox();
            this.m_ctrlServer = new iba.HD.Client.ControlServerSelection();
            this.m_monitorGroup = new iba.Utility.CollapsibleGroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            this.gbDataSource.SuspendLayout();
            this.gbEvent.SuspendLayout();
            this.gbStoreSelection.SuspendLayout();
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            this.SuspendLayout();
            // 
            // gbDataSource
            // 
            resources.ApplyResources(this.gbDataSource, "gbDataSource");
            this.gbDataSource.Controls.Add(this.m_btnTest);
            this.gbDataSource.Controls.Add(this.m_btnUploadPDO);
            this.gbDataSource.Controls.Add(this.m_tbPwdDAT);
            this.gbDataSource.Controls.Add(this.label4);
            this.gbDataSource.Controls.Add(this.m_btnOpenPDO);
            this.gbDataSource.Controls.Add(this.m_btnBrowseDAT);
            this.gbDataSource.Controls.Add(this.m_btnBrowsePDO);
            this.gbDataSource.Controls.Add(this.m_tbDAT);
            this.gbDataSource.Controls.Add(this.m_tbPDO);
            this.gbDataSource.Controls.Add(this.label3);
            this.gbDataSource.Controls.Add(this.label2);
            this.gbDataSource.Name = "gbDataSource";
            this.gbDataSource.TabStop = false;
            // 
            // m_btnTest
            // 
            resources.ApplyResources(this.m_btnTest, "m_btnTest");
            this.m_btnTest.Name = "m_btnTest";
            this.m_btnTest.UseVisualStyleBackColor = true;
            this.m_btnTest.Click += new System.EventHandler(this.m_btnTest_Click);
            // 
            // m_btnUploadPDO
            // 
            resources.ApplyResources(this.m_btnUploadPDO, "m_btnUploadPDO");
            this.m_btnUploadPDO.Image = global::iba.Properties.Resources.img_pdo_upload;
            this.m_btnUploadPDO.Name = "m_btnUploadPDO";
            this.m_btnUploadPDO.UseVisualStyleBackColor = true;
            this.m_btnUploadPDO.Click += new System.EventHandler(this.m_btnUploadPDO_Click);
            // 
            // m_tbPwdDAT
            // 
            resources.ApplyResources(this.m_tbPwdDAT, "m_tbPwdDAT");
            this.m_tbPwdDAT.Name = "m_tbPwdDAT";
            this.m_tbPwdDAT.UseSystemPasswordChar = true;
            this.m_tbPwdDAT.TextChanged += new System.EventHandler(this.m_tbPwdDAT_TextChanged);
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // m_btnOpenPDO
            // 
            resources.ApplyResources(this.m_btnOpenPDO, "m_btnOpenPDO");
            this.m_btnOpenPDO.Image = global::iba.Properties.Resources.ibaAnalyzer_16x16;
            this.m_btnOpenPDO.Name = "m_btnOpenPDO";
            this.m_btnOpenPDO.UseVisualStyleBackColor = true;
            this.m_btnOpenPDO.Click += new System.EventHandler(this.m_btnOpenPDO_Click);
            // 
            // m_btnBrowseDAT
            // 
            resources.ApplyResources(this.m_btnBrowseDAT, "m_btnBrowseDAT");
            this.m_btnBrowseDAT.Image = global::iba.Properties.Resources.open;
            this.m_btnBrowseDAT.Name = "m_btnBrowseDAT";
            this.m_btnBrowseDAT.UseVisualStyleBackColor = true;
            this.m_btnBrowseDAT.Click += new System.EventHandler(this.m_btnBrowseDAT_Click);
            // 
            // m_btnBrowsePDO
            // 
            resources.ApplyResources(this.m_btnBrowsePDO, "m_btnBrowsePDO");
            this.m_btnBrowsePDO.Image = global::iba.Properties.Resources.open;
            this.m_btnBrowsePDO.Name = "m_btnBrowsePDO";
            this.m_btnBrowsePDO.UseVisualStyleBackColor = true;
            this.m_btnBrowsePDO.Click += new System.EventHandler(this.m_btnBrowsePDO_Click);
            // 
            // m_tbDAT
            // 
            resources.ApplyResources(this.m_tbDAT, "m_tbDAT");
            this.m_tbDAT.Name = "m_tbDAT";
            this.m_tbDAT.Leave += new System.EventHandler(this.DatTextChanged);
            // 
            // m_tbPDO
            // 
            resources.ApplyResources(this.m_tbPDO, "m_tbPDO");
            this.m_tbPDO.Name = "m_tbPDO";
            this.m_tbPDO.Leave += new System.EventHandler(this.PDOTextChanged);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // gbEvent
            // 
            resources.ApplyResources(this.gbEvent, "gbEvent");
            this.gbEvent.Controls.Add(this.m_ctrlEvent);
            this.gbEvent.Controls.Add(this.gbStoreSelection);
            this.gbEvent.Name = "gbEvent";
            this.gbEvent.TabStop = false;
            // 
            // m_ctrlEvent
            // 
            this.m_ctrlEvent.AllowDrop = true;
            this.m_ctrlEvent.ChannelTree = null;
            this.m_ctrlEvent.DefaultNumericChannelValue = null;
            this.m_ctrlEvent.DefaultTextChannelValue = null;
            resources.ApplyResources(this.m_ctrlEvent, "m_ctrlEvent");
            this.m_ctrlEvent.EventTrigger = null;
            this.m_ctrlEvent.EventWizard = null;
            this.m_ctrlEvent.Local = false;
            this.m_ctrlEvent.Name = "m_ctrlEvent";
            this.m_ctrlEvent.ReadOnly = true;
            // 
            // gbStoreSelection
            // 
            this.gbStoreSelection.Controls.Add(this.m_ctrlServer);
            resources.ApplyResources(this.gbStoreSelection, "gbStoreSelection");
            this.gbStoreSelection.Name = "gbStoreSelection";
            this.gbStoreSelection.TabStop = false;
            // 
            // m_ctrlServer
            // 
            this.m_ctrlServer.AllowBackups = true;
            this.m_ctrlServer.AllowDatFiles = true;
            resources.ApplyResources(this.m_ctrlServer, "m_ctrlServer");
            this.m_ctrlServer.Name = "m_ctrlServer";
            // 
            // m_monitorGroup
            // 
            resources.ApplyResources(this.m_monitorGroup, "m_monitorGroup");
            this.m_monitorGroup.Controls.Add(this.label5);
            this.m_monitorGroup.Controls.Add(this.label6);
            this.m_monitorGroup.Controls.Add(this.m_nudTime);
            this.m_monitorGroup.Controls.Add(this.m_nudMemory);
            this.m_monitorGroup.Controls.Add(this.m_cbTime);
            this.m_monitorGroup.Controls.Add(this.m_cbMemory);
            this.m_monitorGroup.Name = "m_monitorGroup";
            this.m_monitorGroup.TabStop = false;
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // m_nudTime
            // 
            resources.ApplyResources(this.m_nudTime, "m_nudTime");
            this.m_nudTime.Maximum = new decimal(new int[] {
            300,
            0,
            0,
            0});
            this.m_nudTime.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudTime.Name = "m_nudTime";
            this.m_nudTime.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            // 
            // m_nudMemory
            // 
            resources.ApplyResources(this.m_nudMemory, "m_nudMemory");
            this.m_nudMemory.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.m_nudMemory.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudMemory.Name = "m_nudMemory";
            this.m_nudMemory.Value = new decimal(new int[] {
            512,
            0,
            0,
            0});
            // 
            // m_cbTime
            // 
            resources.ApplyResources(this.m_cbTime, "m_cbTime");
            this.m_cbTime.Name = "m_cbTime";
            this.m_cbTime.UseVisualStyleBackColor = true;
            // 
            // m_cbMemory
            // 
            resources.ApplyResources(this.m_cbMemory, "m_cbMemory");
            this.m_cbMemory.Name = "m_cbMemory";
            this.m_cbMemory.UseVisualStyleBackColor = true;
            // 
            // HDEventCreationTaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.gbDataSource);
            this.Controls.Add(this.gbEvent);
            this.Controls.Add(this.m_monitorGroup);
            this.Name = "HDEventCreationTaskControl";
            this.gbDataSource.ResumeLayout(false);
            this.gbDataSource.PerformLayout();
            this.gbEvent.ResumeLayout(false);
            this.gbStoreSelection.ResumeLayout(false);
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Utility.CollapsibleGroupBox gbDataSource;
        private Utility.CollapsibleGroupBox gbEvent;
        private System.Windows.Forms.TextBox m_tbPwdDAT;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button m_btnOpenPDO;
        private System.Windows.Forms.Button m_btnBrowseDAT;
        private System.Windows.Forms.Button m_btnBrowsePDO;
        private System.Windows.Forms.TextBox m_tbDAT;
        private System.Windows.Forms.TextBox m_tbPDO;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox gbStoreSelection;
        private Utility.CollapsibleGroupBox m_monitorGroup;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
        private System.Windows.Forms.Button m_btnUploadPDO;
        private System.Windows.Forms.Button m_btnTest;
        private System.Windows.Forms.ToolTip m_toolTip;
        private HD.Client.ControlEventConfig m_ctrlEvent;
        private HD.Client.ControlServerSelection m_ctrlServer;
    }
}
