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
            this.macGroupbox = new System.Windows.Forms.GroupBox();
            this.m_tabControl = new System.Windows.Forms.TabControl();
            this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.m_saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.label9 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.m_lblMACVal = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.m_cbAdapters = new System.Windows.Forms.ComboBox();
            this.m_splitContainer = new System.Windows.Forms.SplitContainer();
            this.NQS1 = new System.Windows.Forms.GroupBox();
            this.m_statusNQS1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_tbTSAP_NQS1_NQS = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_tbTSAP_NQS1_PC = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.NQS2 = new System.Windows.Forms.GroupBox();
            this.m_statusNQS2 = new System.Windows.Forms.Label();
            this.m_tbTSAP_NQS2_NQS = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.m_tbTSAP_NQS2_PC = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.m_nqs1MAC = new ISEAGE.May610.Diagrammer.matb();
            this.m_nqs2MAC = new ISEAGE.May610.Diagrammer.matb();
            this.m_ownMAC = new ISEAGE.May610.Diagrammer.matb();
            this.m_splitContainer2.Panel1.SuspendLayout();
            this.m_splitContainer2.Panel2.SuspendLayout();
            this.m_splitContainer2.SuspendLayout();
            this.m_contextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAckTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudSendTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRetryConnectTimeInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTryconnectTimeout)).BeginInit();
            this.macGroupbox.SuspendLayout();
            this.m_tabControl.SuspendLayout();
            this.m_splitContainer.Panel1.SuspendLayout();
            this.m_splitContainer.Panel2.SuspendLayout();
            this.m_splitContainer.SuspendLayout();
            this.NQS1.SuspendLayout();
            this.NQS2.SuspendLayout();
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
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.m_splitContainer);
            this.tabPage1.Controls.Add(this.macGroupbox);
            resources.ApplyResources(this.tabPage1, "tabPage1");
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
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
            // macGroupbox
            // 
            resources.ApplyResources(this.macGroupbox, "macGroupbox");
            this.macGroupbox.Controls.Add(this.m_cbAdapters);
            this.macGroupbox.Controls.Add(this.label19);
            this.macGroupbox.Controls.Add(this.m_lblMACVal);
            this.macGroupbox.Controls.Add(this.label18);
            this.macGroupbox.Controls.Add(this.label9);
            this.macGroupbox.Controls.Add(this.m_ownMAC);
            this.macGroupbox.Name = "macGroupbox";
            this.macGroupbox.TabStop = false;
            // 
            // m_tabControl
            // 
            this.m_tabControl.Controls.Add(this.tabPage1);
            this.m_tabControl.Controls.Add(this.tabPage2);
            resources.ApplyResources(this.m_tabControl, "m_tabControl");
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // label18
            // 
            resources.ApplyResources(this.label18, "label18");
            this.label18.Name = "label18";
            // 
            // m_lblMACVal
            // 
            resources.ApplyResources(this.m_lblMACVal, "m_lblMACVal");
            this.m_lblMACVal.Name = "m_lblMACVal";
            // 
            // label19
            // 
            resources.ApplyResources(this.label19, "label19");
            this.label19.Name = "label19";
            // 
            // m_cbAdapters
            // 
            resources.ApplyResources(this.m_cbAdapters, "m_cbAdapters");
            this.m_cbAdapters.FormattingEnabled = true;
            this.m_cbAdapters.Name = "m_cbAdapters";
            this.m_cbAdapters.SelectedIndexChanged += new System.EventHandler(this.m_cbAdapters_SelectedIndexChanged);
            // 
            // m_splitContainer
            // 
            resources.ApplyResources(this.m_splitContainer, "m_splitContainer");
            this.m_splitContainer.Name = "m_splitContainer";
            // 
            // m_splitContainer.Panel1
            // 
            this.m_splitContainer.Panel1.Controls.Add(this.NQS1);
            // 
            // m_splitContainer.Panel2
            // 
            this.m_splitContainer.Panel2.Controls.Add(this.NQS2);
            // 
            // NQS1
            // 
            this.NQS1.Controls.Add(this.m_nqs1MAC);
            this.NQS1.Controls.Add(this.m_statusNQS1);
            this.NQS1.Controls.Add(this.label7);
            this.NQS1.Controls.Add(this.m_tbTSAP_NQS1_NQS);
            this.NQS1.Controls.Add(this.label3);
            this.NQS1.Controls.Add(this.m_tbTSAP_NQS1_PC);
            this.NQS1.Controls.Add(this.label2);
            this.NQS1.Controls.Add(this.label1);
            resources.ApplyResources(this.NQS1, "NQS1");
            this.NQS1.Name = "NQS1";
            this.NQS1.TabStop = false;
            // 
            // m_statusNQS1
            // 
            resources.ApplyResources(this.m_statusNQS1, "m_statusNQS1");
            this.m_statusNQS1.ForeColor = System.Drawing.Color.Red;
            this.m_statusNQS1.Name = "m_statusNQS1";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // m_tbTSAP_NQS1_NQS
            // 
            resources.ApplyResources(this.m_tbTSAP_NQS1_NQS, "m_tbTSAP_NQS1_NQS");
            this.m_tbTSAP_NQS1_NQS.Name = "m_tbTSAP_NQS1_NQS";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // m_tbTSAP_NQS1_PC
            // 
            resources.ApplyResources(this.m_tbTSAP_NQS1_PC, "m_tbTSAP_NQS1_PC");
            this.m_tbTSAP_NQS1_PC.Name = "m_tbTSAP_NQS1_PC";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // NQS2
            // 
            this.NQS2.Controls.Add(this.m_nqs2MAC);
            this.NQS2.Controls.Add(this.m_statusNQS2);
            this.NQS2.Controls.Add(this.m_tbTSAP_NQS2_NQS);
            this.NQS2.Controls.Add(this.label8);
            this.NQS2.Controls.Add(this.m_tbTSAP_NQS2_PC);
            this.NQS2.Controls.Add(this.label4);
            this.NQS2.Controls.Add(this.label5);
            this.NQS2.Controls.Add(this.label6);
            resources.ApplyResources(this.NQS2, "NQS2");
            this.NQS2.Name = "NQS2";
            this.NQS2.TabStop = false;
            // 
            // m_statusNQS2
            // 
            resources.ApplyResources(this.m_statusNQS2, "m_statusNQS2");
            this.m_statusNQS2.ForeColor = System.Drawing.Color.Red;
            this.m_statusNQS2.Name = "m_statusNQS2";
            // 
            // m_tbTSAP_NQS2_NQS
            // 
            resources.ApplyResources(this.m_tbTSAP_NQS2_NQS, "m_tbTSAP_NQS2_NQS");
            this.m_tbTSAP_NQS2_NQS.Name = "m_tbTSAP_NQS2_NQS";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // m_tbTSAP_NQS2_PC
            // 
            resources.ApplyResources(this.m_tbTSAP_NQS2_PC, "m_tbTSAP_NQS2_PC");
            this.m_tbTSAP_NQS2_PC.Name = "m_tbTSAP_NQS2_PC";
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
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // m_nqs1MAC
            // 
            this.m_nqs1MAC.BackColor = System.Drawing.SystemColors.Window;
            this.m_nqs1MAC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.m_nqs1MAC, "m_nqs1MAC");
            this.m_nqs1MAC.Name = "m_nqs1MAC";
            // 
            // m_nqs2MAC
            // 
            this.m_nqs2MAC.BackColor = System.Drawing.SystemColors.Window;
            this.m_nqs2MAC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.m_nqs2MAC, "m_nqs2MAC");
            this.m_nqs2MAC.Name = "m_nqs2MAC";
            // 
            // m_ownMAC
            // 
            this.m_ownMAC.BackColor = System.Drawing.SystemColors.Window;
            this.m_ownMAC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.m_ownMAC, "m_ownMAC");
            this.m_ownMAC.Name = "m_ownMAC";
            // 
            // PluginH1TaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_tabControl);
            this.MinimumSize = new System.Drawing.Size(565, 400);
            this.Name = "PluginH1TaskControl";
            this.m_splitContainer2.Panel1.ResumeLayout(false);
            this.m_splitContainer2.Panel2.ResumeLayout(false);
            this.m_splitContainer2.ResumeLayout(false);
            this.m_contextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAckTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudSendTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRetryConnectTimeInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTryconnectTimeout)).EndInit();
            this.macGroupbox.ResumeLayout(false);
            this.macGroupbox.PerformLayout();
            this.m_tabControl.ResumeLayout(false);
            this.m_splitContainer.Panel1.ResumeLayout(false);
            this.m_splitContainer.Panel2.ResumeLayout(false);
            this.m_splitContainer.ResumeLayout(false);
            this.NQS1.ResumeLayout(false);
            this.NQS1.PerformLayout();
            this.NQS2.ResumeLayout(false);
            this.NQS2.PerformLayout();
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
        private System.Windows.Forms.SplitContainer m_splitContainer;
        private System.Windows.Forms.GroupBox NQS1;
        private ISEAGE.May610.Diagrammer.matb m_nqs1MAC;
        private System.Windows.Forms.Label m_statusNQS1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox m_tbTSAP_NQS1_NQS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_tbTSAP_NQS1_PC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox NQS2;
        private ISEAGE.May610.Diagrammer.matb m_nqs2MAC;
        private System.Windows.Forms.Label m_statusNQS2;
        private System.Windows.Forms.TextBox m_tbTSAP_NQS2_NQS;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox m_tbTSAP_NQS2_PC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox macGroupbox;
        private ISEAGE.May610.Diagrammer.matb m_ownMAC;
        private System.Windows.Forms.TabControl m_tabControl;
        private System.Windows.Forms.ComboBox m_cbAdapters;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.Label m_lblMACVal;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.Label label9;
    }
}
