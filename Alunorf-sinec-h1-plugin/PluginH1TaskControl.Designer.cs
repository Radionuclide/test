namespace Alunorf_sinec_h1_plugin
{
    partial class PluginH1TaskControl
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PluginH1TaskControl));
            this.m_splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.m_tvMessages = new System.Windows.Forms.TreeView();
            this.m_contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.m_datagvMessages = new System.Windows.Forms.DataGridView();
            this.m_columnFieldname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_columnDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.m_saveXML = new System.Windows.Forms.Button();
            this.m_openXML = new System.Windows.Forms.Button();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_btOpen = new System.Windows.Forms.Button();
            this.m_btSave = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.m_connectParametersPanel = new System.Windows.Forms.Panel();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.m_splitContainer3 = new System.Windows.Forms.SplitContainer();
            this.m_rbSinecH1 = new System.Windows.Forms.RadioButton();
            this.m_rbTCPIP = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label16 = new System.Windows.Forms.Label();
            this.m_nudAckTimeout = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.m_nudSendTimeout = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.m_nudRetryConnectTimeInterval = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.m_nudTryconnectTimeout = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.m_tabControl = new System.Windows.Forms.TabControl();
            this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.m_saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.m_splitContainer2.Panel1.SuspendLayout();
            this.m_splitContainer2.Panel2.SuspendLayout();
            this.m_splitContainer2.SuspendLayout();
            this.m_contextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.m_splitContainer3.Panel1.SuspendLayout();
            this.m_splitContainer3.Panel2.SuspendLayout();
            this.m_splitContainer3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAckTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudSendTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRetryConnectTimeInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTryconnectTimeout)).BeginInit();
            this.m_tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // m_splitContainer2
            // 
            resources.ApplyResources(this.m_splitContainer2, "m_splitContainer2");
            this.m_splitContainer2.Name = "m_splitContainer2";
            // 
            // m_splitContainer2.Panel1
            // 
            this.m_splitContainer2.Panel1.Controls.Add(this.m_tvMessages);
            // 
            // m_splitContainer2.Panel2
            // 
            this.m_splitContainer2.Panel2.Controls.Add(this.m_datagvMessages);
            // 
            // m_tvMessages
            // 
            this.m_tvMessages.AllowDrop = true;
            this.m_tvMessages.ContextMenuStrip = this.m_contextMenu;
            resources.ApplyResources(this.m_tvMessages, "m_tvMessages");
            this.m_tvMessages.HideSelection = false;
            this.m_tvMessages.Name = "m_tvMessages";
            this.m_tvMessages.DragDrop += new System.Windows.Forms.DragEventHandler(this.m_tvMessages_DragDrop);
            this.m_tvMessages.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.m_tvMessages_AfterSelect);
            this.m_tvMessages.MouseDown += new System.Windows.Forms.MouseEventHandler(this.m_tvMessages_MouseDown);
            this.m_tvMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_tvMessages_KeyDown);
            this.m_tvMessages.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.m_tvMessages_ItemDrag);
            this.m_tvMessages.DragOver += new System.Windows.Forms.DragEventHandler(this.m_tvMessages_DragOver);
            // 
            // m_contextMenu
            // 
            this.m_contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteToolStripMenuItem});
            this.m_contextMenu.Name = "m_contextMenu";
            resources.ApplyResources(this.m_contextMenu, "m_contextMenu");
            this.m_contextMenu.Opening += new System.ComponentModel.CancelEventHandler(this.m_contextMenu_Opening);
            // 
            // deleteToolStripMenuItem
            // 
            this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
            resources.ApplyResources(this.deleteToolStripMenuItem, "deleteToolStripMenuItem");
            this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
            // 
            // m_datagvMessages
            // 
            this.m_datagvMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_datagvMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_columnFieldname,
            this.m_columnDataType,
            this.dataGridViewTextBoxComment});
            resources.ApplyResources(this.m_datagvMessages, "m_datagvMessages");
            this.m_datagvMessages.Name = "m_datagvMessages";
            this.m_datagvMessages.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.m_datagvMessages_CellValueChanged);
            this.m_datagvMessages.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.m_datagvMessages_RowPostPaint);
            this.m_datagvMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_datagvMessages_KeyDown);
            // 
            // m_columnFieldname
            // 
            this.m_columnFieldname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnFieldname.FillWeight = 30F;
            resources.ApplyResources(this.m_columnFieldname, "m_columnFieldname");
            this.m_columnFieldname.Name = "m_columnFieldname";
            this.m_columnFieldname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_columnDataType
            // 
            this.m_columnDataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnDataType.FillWeight = 20F;
            resources.ApplyResources(this.m_columnDataType, "m_columnDataType");
            this.m_columnDataType.Items.AddRange(new object[] {
            "int1",
            "int2",
            "int4",
            "u. int1",
            "u. int2",
            "u. int4",
            "float4",
            "char1",
            "char2",
            "char3",
            "char4",
            "char5",
            "char6",
            "char7",
            "char8",
            "char9",
            "char10",
            "char11",
            "char12",
            "char13",
            "char14",
            "char15",
            "char16",
            "char17",
            "char18",
            "char19",
            "char20",
            "char21",
            "char22",
            "char23",
            "char24",
            "char25",
            "char26",
            "char27",
            "char28",
            "char29",
            "char30",
            "char31",
            "char32",
            "char33",
            "char34",
            "char35",
            "char36",
            "char37",
            "char38",
            "char39",
            "char40"});
            this.m_columnDataType.MaxDropDownItems = 10;
            this.m_columnDataType.Name = "m_columnDataType";
            // 
            // dataGridViewTextBoxComment
            // 
            this.dataGridViewTextBoxComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxComment.FillWeight = 40F;
            resources.ApplyResources(this.dataGridViewTextBoxComment, "dataGridViewTextBoxComment");
            this.dataGridViewTextBoxComment.Name = "dataGridViewTextBoxComment";
            this.dataGridViewTextBoxComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_refreshTimer
            // 
            this.m_refreshTimer.Interval = 1000;
            this.m_refreshTimer.Tick += new System.EventHandler(this.m_refreshTimer_Tick);
            // 
            // m_saveXML
            // 
            resources.ApplyResources(this.m_saveXML, "m_saveXML");
            this.m_saveXML.Name = "m_saveXML";
            this.m_saveXML.UseVisualStyleBackColor = true;
            // 
            // m_openXML
            // 
            resources.ApplyResources(this.m_openXML, "m_openXML");
            this.m_openXML.Name = "m_openXML";
            this.m_openXML.UseVisualStyleBackColor = true;
            // 
            // m_btOpen
            // 
            resources.ApplyResources(this.m_btOpen, "m_btOpen");
            this.m_btOpen.Image = global::Alunorf_sinec_h1_plugin.Properties.Resources.open;
            this.m_btOpen.Name = "m_btOpen";
            this.m_toolTip.SetToolTip(this.m_btOpen, resources.GetString("m_btOpen.ToolTip"));
            this.m_btOpen.UseVisualStyleBackColor = true;
            this.m_btOpen.Click += new System.EventHandler(this.m_btOpen_Click);
            // 
            // m_btSave
            // 
            resources.ApplyResources(this.m_btSave, "m_btSave");
            this.m_btSave.Image = global::Alunorf_sinec_h1_plugin.Properties.Resources.Speichern;
            this.m_btSave.Name = "m_btSave";
            this.m_toolTip.SetToolTip(this.m_btSave, resources.GetString("m_btSave.ToolTip"));
            this.m_btSave.UseVisualStyleBackColor = true;
            this.m_btSave.Click += new System.EventHandler(this.m_btSave_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.m_btSave);
            this.tabPage2.Controls.Add(this.m_splitContainer2);
            this.tabPage2.Controls.Add(this.m_btOpen);
            resources.ApplyResources(this.tabPage2, "tabPage2");
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.m_connectParametersPanel);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // m_connectParametersPanel
            // 
            resources.ApplyResources(this.m_connectParametersPanel, "m_connectParametersPanel");
            this.m_connectParametersPanel.Name = "m_connectParametersPanel";
            // 
            // groupBox2
            // 
            resources.ApplyResources(this.groupBox2, "groupBox2");
            this.groupBox2.Controls.Add(this.m_splitContainer3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.TabStop = false;
            // 
            // m_splitContainer3
            // 
            resources.ApplyResources(this.m_splitContainer3, "m_splitContainer3");
            this.m_splitContainer3.Name = "m_splitContainer3";
            // 
            // m_splitContainer3.Panel1
            // 
            this.m_splitContainer3.Panel1.Controls.Add(this.m_rbSinecH1);
            // 
            // m_splitContainer3.Panel2
            // 
            this.m_splitContainer3.Panel2.Controls.Add(this.m_rbTCPIP);
            // 
            // m_rbSinecH1
            // 
            resources.ApplyResources(this.m_rbSinecH1, "m_rbSinecH1");
            this.m_rbSinecH1.Name = "m_rbSinecH1";
            this.m_rbSinecH1.TabStop = true;
            this.m_rbSinecH1.UseVisualStyleBackColor = true;
            this.m_rbSinecH1.CheckedChanged += new System.EventHandler(this.m_rbSinecH1_CheckedChanged);
            // 
            // m_rbTCPIP
            // 
            resources.ApplyResources(this.m_rbTCPIP, "m_rbTCPIP");
            this.m_rbTCPIP.Name = "m_rbTCPIP";
            this.m_rbTCPIP.TabStop = true;
            this.m_rbTCPIP.UseVisualStyleBackColor = true;
            this.m_rbTCPIP.CheckedChanged += new System.EventHandler(this.m_rbSinecH1_CheckedChanged);
            // 
            // groupBox1
            // 
            resources.ApplyResources(this.groupBox1, "groupBox1");
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.m_nudAckTimeout);
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.m_nudSendTimeout);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.m_nudRetryConnectTimeInterval);
            this.groupBox1.Controls.Add(this.label13);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.m_nudTryconnectTimeout);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.TabStop = false;
            // 
            // label16
            // 
            resources.ApplyResources(this.label16, "label16");
            this.label16.Name = "label16";
            // 
            // m_nudAckTimeout
            // 
            resources.ApplyResources(this.m_nudAckTimeout, "m_nudAckTimeout");
            this.m_nudAckTimeout.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.m_nudAckTimeout.Name = "m_nudAckTimeout";
            this.m_nudAckTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label17
            // 
            resources.ApplyResources(this.label17, "label17");
            this.label17.Name = "label17";
            // 
            // label14
            // 
            resources.ApplyResources(this.label14, "label14");
            this.label14.Name = "label14";
            // 
            // m_nudSendTimeout
            // 
            resources.ApplyResources(this.m_nudSendTimeout, "m_nudSendTimeout");
            this.m_nudSendTimeout.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.m_nudSendTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudSendTimeout.Name = "m_nudSendTimeout";
            this.m_nudSendTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label15
            // 
            resources.ApplyResources(this.label15, "label15");
            this.label15.Name = "label15";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // m_nudRetryConnectTimeInterval
            // 
            resources.ApplyResources(this.m_nudRetryConnectTimeInterval, "m_nudRetryConnectTimeInterval");
            this.m_nudRetryConnectTimeInterval.Maximum = new decimal(new int[] {
            60,
            0,
            0,
            0});
            this.m_nudRetryConnectTimeInterval.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudRetryConnectTimeInterval.Name = "m_nudRetryConnectTimeInterval";
            this.m_nudRetryConnectTimeInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label13
            // 
            resources.ApplyResources(this.label13, "label13");
            this.label13.Name = "label13";
            // 
            // label11
            // 
            resources.ApplyResources(this.label11, "label11");
            this.label11.Name = "label11";
            // 
            // m_nudTryconnectTimeout
            // 
            resources.ApplyResources(this.m_nudTryconnectTimeout, "m_nudTryconnectTimeout");
            this.m_nudTryconnectTimeout.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.m_nudTryconnectTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudTryconnectTimeout.Name = "m_nudTryconnectTimeout";
            this.m_nudTryconnectTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label10
            // 
            resources.ApplyResources(this.label10, "label10");
            this.label10.Name = "label10";
            // 
            // m_tabControl
            // 
            this.m_tabControl.Controls.Add(this.tabPage1);
            this.m_tabControl.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.m_tabControl, "m_tabControl");
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            // 
            // PluginH1TaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_tabControl);
            this.MinimumSize = new System.Drawing.Size(565, 430);
            this.Name = "PluginH1TaskControl";
            this.m_splitContainer2.Panel1.ResumeLayout(false);
            this.m_splitContainer2.Panel2.ResumeLayout(false);
            this.m_splitContainer2.ResumeLayout(false);
            this.m_contextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.m_splitContainer3.Panel1.ResumeLayout(false);
            this.m_splitContainer3.Panel1.PerformLayout();
            this.m_splitContainer3.Panel2.ResumeLayout(false);
            this.m_splitContainer3.Panel2.PerformLayout();
            this.m_splitContainer3.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAckTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudSendTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRetryConnectTimeInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTryconnectTimeout)).EndInit();
            this.m_tabControl.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer m_refreshTimer;
        private System.Windows.Forms.Button m_saveXML;
        private System.Windows.Forms.Button m_openXML;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.ContextMenuStrip m_contextMenu;
        private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog1;
        private System.Windows.Forms.SaveFileDialog m_saveFileDialog1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button m_btSave;
        private System.Windows.Forms.SplitContainer m_splitContainer2;
        private System.Windows.Forms.TreeView m_tvMessages;
        private System.Windows.Forms.DataGridView m_datagvMessages;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_columnFieldname;
        private System.Windows.Forms.DataGridViewComboBoxColumn m_columnDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxComment;
        private System.Windows.Forms.Button m_btOpen;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.NumericUpDown m_nudAckTimeout;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.NumericUpDown m_nudSendTimeout;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.NumericUpDown m_nudRetryConnectTimeInterval;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown m_nudTryconnectTimeout;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TabControl m_tabControl;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.SplitContainer m_splitContainer3;
        private System.Windows.Forms.RadioButton m_rbSinecH1;
        private System.Windows.Forms.RadioButton m_rbTCPIP;
        private System.Windows.Forms.Panel m_connectParametersPanel;
    }
}
