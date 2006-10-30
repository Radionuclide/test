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
            this.m_refreshTimer = new System.Windows.Forms.Timer(this.components);
            this.m_tabControl = new System.Windows.Forms.TabControl();
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
            this.m_splitContainer = new System.Windows.Forms.SplitContainer();
            this.NQS1 = new System.Windows.Forms.GroupBox();
            this.m_statusNQS1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_tbTSAP_NQS1_NQS = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.m_tbTSAP_NQS1_PC = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.m_nqs1mac6 = new System.Windows.Forms.TextBox();
            this.m_nqs1mac1 = new System.Windows.Forms.TextBox();
            this.m_nqs1mac5 = new System.Windows.Forms.TextBox();
            this.m_nqs1mac2 = new System.Windows.Forms.TextBox();
            this.m_nqs1mac4 = new System.Windows.Forms.TextBox();
            this.m_nqs1mac3 = new System.Windows.Forms.TextBox();
            this.NQS2 = new System.Windows.Forms.GroupBox();
            this.m_statusNQS2 = new System.Windows.Forms.Label();
            this.m_tbTSAP_NQS2_NQS = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.m_tbTSAP_NQS2_PC = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_nqs2mac3 = new System.Windows.Forms.TextBox();
            this.m_nqs2mac4 = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_nqs2mac2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.m_nqs2mac5 = new System.Windows.Forms.TextBox();
            this.m_nqs2mac6 = new System.Windows.Forms.TextBox();
            this.m_nqs2mac1 = new System.Windows.Forms.TextBox();
            this.macGroupbox = new System.Windows.Forms.GroupBox();
            this.m_pcmac6 = new System.Windows.Forms.TextBox();
            this.m_pcmac5 = new System.Windows.Forms.TextBox();
            this.m_pcmac4 = new System.Windows.Forms.TextBox();
            this.m_pcmac3 = new System.Windows.Forms.TextBox();
            this.m_pcmac2 = new System.Windows.Forms.TextBox();
            this.m_pcmac1 = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.m_btSave = new System.Windows.Forms.Button();
            this.m_splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.m_tvMessages = new System.Windows.Forms.TreeView();
            this.m_datagvMessages = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxFieldname = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewComboBoxDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewTextBoxComment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_btOpen = new System.Windows.Forms.Button();
            this.m_saveXML = new System.Windows.Forms.Button();
            this.m_openXML = new System.Windows.Forms.Button();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_tabControl.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAckTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudSendTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRetryConnectTimeInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTryconnectTimeout)).BeginInit();
            this.m_splitContainer.Panel1.SuspendLayout();
            this.m_splitContainer.Panel2.SuspendLayout();
            this.m_splitContainer.SuspendLayout();
            this.NQS1.SuspendLayout();
            this.NQS2.SuspendLayout();
            this.macGroupbox.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.m_splitContainer2.Panel1.SuspendLayout();
            this.m_splitContainer2.Panel2.SuspendLayout();
            this.m_splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // m_refreshTimer
            // 
            this.m_refreshTimer.Interval = 1000;
            this.m_refreshTimer.Tick += new System.EventHandler(this.m_refreshTimer_Tick);
            // 
            // m_tabControl
            // 
            this.m_tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tabControl.Controls.Add(this.tabPage1);
            this.m_tabControl.Controls.Add(this.tabPage2);
            this.m_tabControl.Location = new System.Drawing.Point(0, 0);
            this.m_tabControl.Name = "m_tabControl";
            this.m_tabControl.SelectedIndex = 0;
            this.m_tabControl.Size = new System.Drawing.Size(565, 404);
            this.m_tabControl.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.m_splitContainer);
            this.tabPage1.Controls.Add(this.macGroupbox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(557, 378);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "protocol configuration";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
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
            this.groupBox1.Location = new System.Drawing.Point(0, 245);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(557, 81);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Timing";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(295, 53);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(115, 13);
            this.label16.TabIndex = 9;
            this.label16.Text = "Acknowledge time out:";
            // 
            // m_nudAckTimeout
            // 
            this.m_nudAckTimeout.Location = new System.Drawing.Point(416, 51);
            this.m_nudAckTimeout.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.m_nudAckTimeout.Name = "m_nudAckTimeout";
            this.m_nudAckTimeout.Size = new System.Drawing.Size(66, 20);
            this.m_nudAckTimeout.TabIndex = 10;
            this.m_nudAckTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label17.Location = new System.Drawing.Point(488, 53);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 13);
            this.label17.TabIndex = 11;
            this.label17.Text = "seconds";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(295, 27);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 13);
            this.label14.TabIndex = 3;
            this.label14.Text = "Send time out:";
            // 
            // m_nudSendTimeout
            // 
            this.m_nudSendTimeout.Location = new System.Drawing.Point(416, 25);
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
            this.m_nudSendTimeout.Size = new System.Drawing.Size(66, 20);
            this.m_nudSendTimeout.TabIndex = 4;
            this.m_nudSendTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(488, 27);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 13);
            this.label15.TabIndex = 5;
            this.label15.Text = "seconds";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(11, 53);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(136, 13);
            this.label12.TabIndex = 6;
            this.label12.Text = "Retry connect time interval:";
            // 
            // m_nudRetryConnectTimeInterval
            // 
            this.m_nudRetryConnectTimeInterval.Location = new System.Drawing.Point(153, 51);
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
            this.m_nudRetryConnectTimeInterval.Size = new System.Drawing.Size(66, 20);
            this.m_nudRetryConnectTimeInterval.TabIndex = 7;
            this.m_nudRetryConnectTimeInterval.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label13.Location = new System.Drawing.Point(225, 53);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(43, 13);
            this.label13.TabIndex = 8;
            this.label13.Text = "minutes";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(11, 27);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(107, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Try connect time out:";
            // 
            // m_nudTryconnectTimeout
            // 
            this.m_nudTryconnectTimeout.Location = new System.Drawing.Point(153, 25);
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
            this.m_nudTryconnectTimeout.Size = new System.Drawing.Size(66, 20);
            this.m_nudTryconnectTimeout.TabIndex = 1;
            this.m_nudTryconnectTimeout.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label10.Location = new System.Drawing.Point(225, 27);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 2;
            this.label10.Text = "seconds";
            // 
            // m_splitContainer
            // 
            this.m_splitContainer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_splitContainer.Location = new System.Drawing.Point(0, 59);
            this.m_splitContainer.Name = "m_splitContainer";
            // 
            // m_splitContainer.Panel1
            // 
            this.m_splitContainer.Panel1.Controls.Add(this.NQS1);
            // 
            // m_splitContainer.Panel2
            // 
            this.m_splitContainer.Panel2.Controls.Add(this.NQS2);
            this.m_splitContainer.Size = new System.Drawing.Size(556, 180);
            this.m_splitContainer.SplitterDistance = 276;
            this.m_splitContainer.TabIndex = 1;
            // 
            // NQS1
            // 
            this.NQS1.Controls.Add(this.m_statusNQS1);
            this.NQS1.Controls.Add(this.label7);
            this.NQS1.Controls.Add(this.m_tbTSAP_NQS1_NQS);
            this.NQS1.Controls.Add(this.label3);
            this.NQS1.Controls.Add(this.m_tbTSAP_NQS1_PC);
            this.NQS1.Controls.Add(this.label2);
            this.NQS1.Controls.Add(this.label1);
            this.NQS1.Controls.Add(this.m_nqs1mac6);
            this.NQS1.Controls.Add(this.m_nqs1mac1);
            this.NQS1.Controls.Add(this.m_nqs1mac5);
            this.NQS1.Controls.Add(this.m_nqs1mac2);
            this.NQS1.Controls.Add(this.m_nqs1mac4);
            this.NQS1.Controls.Add(this.m_nqs1mac3);
            this.NQS1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NQS1.Location = new System.Drawing.Point(0, 0);
            this.NQS1.Name = "NQS1";
            this.NQS1.Size = new System.Drawing.Size(276, 180);
            this.NQS1.TabIndex = 0;
            this.NQS1.TabStop = false;
            this.NQS1.Text = "NQS 1";
            // 
            // m_statusNQS1
            // 
            this.m_statusNQS1.AutoSize = true;
            this.m_statusNQS1.ForeColor = System.Drawing.Color.Red;
            this.m_statusNQS1.Location = new System.Drawing.Point(68, 148);
            this.m_statusNQS1.Name = "m_statusNQS1";
            this.m_statusNQS1.Size = new System.Drawing.Size(71, 13);
            this.m_statusNQS1.TabIndex = 12;
            this.m_statusNQS1.Text = "disconnected";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 148);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "Status:";
            // 
            // m_tbTSAP_NQS1_NQS
            // 
            this.m_tbTSAP_NQS1_NQS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbTSAP_NQS1_NQS.Location = new System.Drawing.Point(13, 120);
            this.m_tbTSAP_NQS1_NQS.MaxLength = 16;
            this.m_tbTSAP_NQS1_NQS.Name = "m_tbTSAP_NQS1_NQS";
            this.m_tbTSAP_NQS1_NQS.Size = new System.Drawing.Size(194, 20);
            this.m_tbTSAP_NQS1_NQS.TabIndex = 10;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 104);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(64, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "TSAP NQS:";
            // 
            // m_tbTSAP_NQS1_PC
            // 
            this.m_tbTSAP_NQS1_PC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbTSAP_NQS1_PC.Location = new System.Drawing.Point(13, 77);
            this.m_tbTSAP_NQS1_PC.MaxLength = 16;
            this.m_tbTSAP_NQS1_PC.Name = "m_tbTSAP_NQS1_PC";
            this.m_tbTSAP_NQS1_PC.Size = new System.Drawing.Size(194, 20);
            this.m_tbTSAP_NQS1_PC.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(10, 61);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(55, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "TSAP PC:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(83, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Station address:";
            // 
            // m_nqs1mac6
            // 
            this.m_nqs1mac6.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs1mac6.Location = new System.Drawing.Point(158, 32);
            this.m_nqs1mac6.MaxLength = 2;
            this.m_nqs1mac6.Name = "m_nqs1mac6";
            this.m_nqs1mac6.Size = new System.Drawing.Size(23, 20);
            this.m_nqs1mac6.TabIndex = 6;
            this.m_nqs1mac6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_nqs1mac1
            // 
            this.m_nqs1mac1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs1mac1.Location = new System.Drawing.Point(13, 32);
            this.m_nqs1mac1.MaxLength = 2;
            this.m_nqs1mac1.Name = "m_nqs1mac1";
            this.m_nqs1mac1.Size = new System.Drawing.Size(23, 20);
            this.m_nqs1mac1.TabIndex = 1;
            this.m_nqs1mac1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_nqs1mac5
            // 
            this.m_nqs1mac5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs1mac5.Location = new System.Drawing.Point(129, 32);
            this.m_nqs1mac5.MaxLength = 2;
            this.m_nqs1mac5.Name = "m_nqs1mac5";
            this.m_nqs1mac5.Size = new System.Drawing.Size(23, 20);
            this.m_nqs1mac5.TabIndex = 5;
            this.m_nqs1mac5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_nqs1mac2
            // 
            this.m_nqs1mac2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs1mac2.Location = new System.Drawing.Point(42, 32);
            this.m_nqs1mac2.MaxLength = 2;
            this.m_nqs1mac2.Name = "m_nqs1mac2";
            this.m_nqs1mac2.Size = new System.Drawing.Size(23, 20);
            this.m_nqs1mac2.TabIndex = 2;
            this.m_nqs1mac2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_nqs1mac4
            // 
            this.m_nqs1mac4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs1mac4.Location = new System.Drawing.Point(100, 32);
            this.m_nqs1mac4.MaxLength = 2;
            this.m_nqs1mac4.Name = "m_nqs1mac4";
            this.m_nqs1mac4.Size = new System.Drawing.Size(23, 20);
            this.m_nqs1mac4.TabIndex = 4;
            this.m_nqs1mac4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_nqs1mac3
            // 
            this.m_nqs1mac3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs1mac3.Location = new System.Drawing.Point(71, 32);
            this.m_nqs1mac3.MaxLength = 2;
            this.m_nqs1mac3.Name = "m_nqs1mac3";
            this.m_nqs1mac3.Size = new System.Drawing.Size(23, 20);
            this.m_nqs1mac3.TabIndex = 3;
            this.m_nqs1mac3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // NQS2
            // 
            this.NQS2.Controls.Add(this.m_statusNQS2);
            this.NQS2.Controls.Add(this.m_tbTSAP_NQS2_NQS);
            this.NQS2.Controls.Add(this.label8);
            this.NQS2.Controls.Add(this.m_tbTSAP_NQS2_PC);
            this.NQS2.Controls.Add(this.label4);
            this.NQS2.Controls.Add(this.m_nqs2mac3);
            this.NQS2.Controls.Add(this.m_nqs2mac4);
            this.NQS2.Controls.Add(this.label5);
            this.NQS2.Controls.Add(this.m_nqs2mac2);
            this.NQS2.Controls.Add(this.label6);
            this.NQS2.Controls.Add(this.m_nqs2mac5);
            this.NQS2.Controls.Add(this.m_nqs2mac6);
            this.NQS2.Controls.Add(this.m_nqs2mac1);
            this.NQS2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NQS2.Location = new System.Drawing.Point(0, 0);
            this.NQS2.Name = "NQS2";
            this.NQS2.Size = new System.Drawing.Size(276, 180);
            this.NQS2.TabIndex = 0;
            this.NQS2.TabStop = false;
            this.NQS2.Text = "NQS 2";
            // 
            // m_statusNQS2
            // 
            this.m_statusNQS2.AutoSize = true;
            this.m_statusNQS2.ForeColor = System.Drawing.Color.Red;
            this.m_statusNQS2.Location = new System.Drawing.Point(69, 148);
            this.m_statusNQS2.Name = "m_statusNQS2";
            this.m_statusNQS2.Size = new System.Drawing.Size(71, 13);
            this.m_statusNQS2.TabIndex = 12;
            this.m_statusNQS2.Text = "disconnected";
            // 
            // m_tbTSAP_NQS2_NQS
            // 
            this.m_tbTSAP_NQS2_NQS.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbTSAP_NQS2_NQS.Location = new System.Drawing.Point(14, 120);
            this.m_tbTSAP_NQS2_NQS.MaxLength = 16;
            this.m_tbTSAP_NQS2_NQS.Name = "m_tbTSAP_NQS2_NQS";
            this.m_tbTSAP_NQS2_NQS.Size = new System.Drawing.Size(191, 20);
            this.m_tbTSAP_NQS2_NQS.TabIndex = 10;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(11, 148);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 13);
            this.label8.TabIndex = 11;
            this.label8.Text = "Status:";
            // 
            // m_tbTSAP_NQS2_PC
            // 
            this.m_tbTSAP_NQS2_PC.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_tbTSAP_NQS2_PC.Location = new System.Drawing.Point(14, 77);
            this.m_tbTSAP_NQS2_PC.MaxLength = 16;
            this.m_tbTSAP_NQS2_PC.Name = "m_tbTSAP_NQS2_PC";
            this.m_tbTSAP_NQS2_PC.Size = new System.Drawing.Size(191, 20);
            this.m_tbTSAP_NQS2_PC.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 104);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "TSAP NQS:";
            // 
            // m_nqs2mac3
            // 
            this.m_nqs2mac3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs2mac3.Location = new System.Drawing.Point(72, 32);
            this.m_nqs2mac3.MaxLength = 2;
            this.m_nqs2mac3.Name = "m_nqs2mac3";
            this.m_nqs2mac3.Size = new System.Drawing.Size(23, 20);
            this.m_nqs2mac3.TabIndex = 3;
            this.m_nqs2mac3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_nqs2mac4
            // 
            this.m_nqs2mac4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs2mac4.Location = new System.Drawing.Point(101, 32);
            this.m_nqs2mac4.MaxLength = 2;
            this.m_nqs2mac4.Name = "m_nqs2mac4";
            this.m_nqs2mac4.Size = new System.Drawing.Size(23, 20);
            this.m_nqs2mac4.TabIndex = 4;
            this.m_nqs2mac4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 61);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "TSAP PC:";
            // 
            // m_nqs2mac2
            // 
            this.m_nqs2mac2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs2mac2.Location = new System.Drawing.Point(43, 32);
            this.m_nqs2mac2.MaxLength = 2;
            this.m_nqs2mac2.Name = "m_nqs2mac2";
            this.m_nqs2mac2.Size = new System.Drawing.Size(23, 20);
            this.m_nqs2mac2.TabIndex = 2;
            this.m_nqs2mac2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 16);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(83, 13);
            this.label6.TabIndex = 0;
            this.label6.Text = "Station address:";
            // 
            // m_nqs2mac5
            // 
            this.m_nqs2mac5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs2mac5.Location = new System.Drawing.Point(130, 32);
            this.m_nqs2mac5.MaxLength = 2;
            this.m_nqs2mac5.Name = "m_nqs2mac5";
            this.m_nqs2mac5.Size = new System.Drawing.Size(23, 20);
            this.m_nqs2mac5.TabIndex = 5;
            this.m_nqs2mac5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_nqs2mac6
            // 
            this.m_nqs2mac6.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs2mac6.Location = new System.Drawing.Point(159, 32);
            this.m_nqs2mac6.MaxLength = 2;
            this.m_nqs2mac6.Name = "m_nqs2mac6";
            this.m_nqs2mac6.Size = new System.Drawing.Size(23, 20);
            this.m_nqs2mac6.TabIndex = 6;
            this.m_nqs2mac6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // m_nqs2mac1
            // 
            this.m_nqs2mac1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_nqs2mac1.Location = new System.Drawing.Point(14, 32);
            this.m_nqs2mac1.MaxLength = 2;
            this.m_nqs2mac1.Name = "m_nqs2mac1";
            this.m_nqs2mac1.Size = new System.Drawing.Size(23, 20);
            this.m_nqs2mac1.TabIndex = 1;
            this.m_nqs2mac1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // macGroupbox
            // 
            this.macGroupbox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.macGroupbox.Controls.Add(this.m_pcmac6);
            this.macGroupbox.Controls.Add(this.m_pcmac5);
            this.macGroupbox.Controls.Add(this.m_pcmac4);
            this.macGroupbox.Controls.Add(this.m_pcmac3);
            this.macGroupbox.Controls.Add(this.m_pcmac2);
            this.macGroupbox.Controls.Add(this.m_pcmac1);
            this.macGroupbox.Location = new System.Drawing.Point(0, 0);
            this.macGroupbox.Name = "macGroupbox";
            this.macGroupbox.Size = new System.Drawing.Size(557, 53);
            this.macGroupbox.TabIndex = 0;
            this.macGroupbox.TabStop = false;
            this.macGroupbox.Text = "PC Station address";
            // 
            // m_pcmac6
            // 
            this.m_pcmac6.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_pcmac6.Location = new System.Drawing.Point(158, 19);
            this.m_pcmac6.MaxLength = 2;
            this.m_pcmac6.Name = "m_pcmac6";
            this.m_pcmac6.Size = new System.Drawing.Size(23, 20);
            this.m_pcmac6.TabIndex = 5;
            this.m_pcmac6.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m_pcmac6.Validating += new System.ComponentModel.CancelEventHandler(this.m_pcmac1_Validating);
            // 
            // m_pcmac5
            // 
            this.m_pcmac5.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_pcmac5.Location = new System.Drawing.Point(129, 19);
            this.m_pcmac5.MaxLength = 2;
            this.m_pcmac5.Name = "m_pcmac5";
            this.m_pcmac5.Size = new System.Drawing.Size(23, 20);
            this.m_pcmac5.TabIndex = 4;
            this.m_pcmac5.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m_pcmac5.Validating += new System.ComponentModel.CancelEventHandler(this.m_pcmac1_Validating);
            // 
            // m_pcmac4
            // 
            this.m_pcmac4.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_pcmac4.Location = new System.Drawing.Point(100, 19);
            this.m_pcmac4.MaxLength = 2;
            this.m_pcmac4.Name = "m_pcmac4";
            this.m_pcmac4.Size = new System.Drawing.Size(23, 20);
            this.m_pcmac4.TabIndex = 3;
            this.m_pcmac4.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m_pcmac4.Validating += new System.ComponentModel.CancelEventHandler(this.m_pcmac1_Validating);
            // 
            // m_pcmac3
            // 
            this.m_pcmac3.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_pcmac3.Location = new System.Drawing.Point(71, 19);
            this.m_pcmac3.MaxLength = 2;
            this.m_pcmac3.Name = "m_pcmac3";
            this.m_pcmac3.Size = new System.Drawing.Size(23, 20);
            this.m_pcmac3.TabIndex = 2;
            this.m_pcmac3.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m_pcmac3.Validating += new System.ComponentModel.CancelEventHandler(this.m_pcmac1_Validating);
            // 
            // m_pcmac2
            // 
            this.m_pcmac2.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_pcmac2.Location = new System.Drawing.Point(42, 19);
            this.m_pcmac2.MaxLength = 2;
            this.m_pcmac2.Name = "m_pcmac2";
            this.m_pcmac2.Size = new System.Drawing.Size(23, 20);
            this.m_pcmac2.TabIndex = 1;
            this.m_pcmac2.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m_pcmac2.Validating += new System.ComponentModel.CancelEventHandler(this.m_pcmac1_Validating);
            // 
            // m_pcmac1
            // 
            this.m_pcmac1.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.m_pcmac1.Location = new System.Drawing.Point(13, 19);
            this.m_pcmac1.MaxLength = 2;
            this.m_pcmac1.Name = "m_pcmac1";
            this.m_pcmac1.Size = new System.Drawing.Size(23, 20);
            this.m_pcmac1.TabIndex = 0;
            this.m_pcmac1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.m_pcmac1.Validating += new System.ComponentModel.CancelEventHandler(this.m_pcmac1_Validating);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.m_btSave);
            this.tabPage2.Controls.Add(this.m_splitContainer2);
            this.tabPage2.Controls.Add(this.m_btOpen);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(557, 378);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "messages layout";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // m_btSave
            // 
            this.m_btSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btSave.Image = global::Alunorf_sinec_h1_plugin.Properties.Resources.Speichern;
            this.m_btSave.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btSave.Location = new System.Drawing.Point(511, 52);
            this.m_btSave.Name = "m_btSave";
            this.m_btSave.Size = new System.Drawing.Size(40, 40);
            this.m_btSave.TabIndex = 1;
            this.m_toolTip.SetToolTip(this.m_btSave, "Export message layout to XML file");
            this.m_btSave.UseVisualStyleBackColor = true;
            this.m_btSave.Click += new System.EventHandler(this.m_btSave_Click);
            // 
            // m_splitContainer2
            // 
            this.m_splitContainer2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.m_splitContainer2.Location = new System.Drawing.Point(9, 6);
            this.m_splitContainer2.Name = "m_splitContainer2";
            // 
            // m_splitContainer2.Panel1
            // 
            this.m_splitContainer2.Panel1.Controls.Add(this.m_tvMessages);
            // 
            // m_splitContainer2.Panel2
            // 
            this.m_splitContainer2.Panel2.Controls.Add(this.m_datagvMessages);
            this.m_splitContainer2.Size = new System.Drawing.Size(496, 372);
            this.m_splitContainer2.SplitterDistance = 140;
            this.m_splitContainer2.TabIndex = 7;
            // 
            // m_tvMessages
            // 
            this.m_tvMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tvMessages.Location = new System.Drawing.Point(0, 0);
            this.m_tvMessages.Name = "m_tvMessages";
            this.m_tvMessages.Size = new System.Drawing.Size(140, 372);
            this.m_tvMessages.TabIndex = 0;
            // 
            // m_datagvMessages
            // 
            this.m_datagvMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_datagvMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxFieldname,
            this.dataGridViewComboBoxDataType,
            this.dataGridViewTextBoxComment});
            this.m_datagvMessages.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_datagvMessages.Location = new System.Drawing.Point(0, 0);
            this.m_datagvMessages.Name = "m_datagvMessages";
            this.m_datagvMessages.Size = new System.Drawing.Size(352, 372);
            this.m_datagvMessages.TabIndex = 0;
            this.m_datagvMessages.KeyDown += new System.Windows.Forms.KeyEventHandler(this.m_datagvMessages_KeyDown);
            // 
            // dataGridViewTextBoxFieldname
            // 
            this.dataGridViewTextBoxFieldname.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxFieldname.FillWeight = 30F;
            this.dataGridViewTextBoxFieldname.HeaderText = "Fieldname";
            this.dataGridViewTextBoxFieldname.Name = "dataGridViewTextBoxFieldname";
            this.dataGridViewTextBoxFieldname.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewComboBoxDataType
            // 
            this.dataGridViewComboBoxDataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewComboBoxDataType.FillWeight = 20F;
            this.dataGridViewComboBoxDataType.HeaderText = "Datatype";
            this.dataGridViewComboBoxDataType.Items.AddRange(new object[] {
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
            this.dataGridViewComboBoxDataType.MaxDropDownItems = 10;
            this.dataGridViewComboBoxDataType.Name = "dataGridViewComboBoxDataType";
            // 
            // dataGridViewTextBoxComment
            // 
            this.dataGridViewTextBoxComment.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dataGridViewTextBoxComment.FillWeight = 40F;
            this.dataGridViewTextBoxComment.HeaderText = "Comment";
            this.dataGridViewTextBoxComment.Name = "dataGridViewTextBoxComment";
            this.dataGridViewTextBoxComment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_btOpen
            // 
            this.m_btOpen.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_btOpen.Image = global::Alunorf_sinec_h1_plugin.Properties.Resources.open;
            this.m_btOpen.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_btOpen.Location = new System.Drawing.Point(511, 6);
            this.m_btOpen.Name = "m_btOpen";
            this.m_btOpen.Size = new System.Drawing.Size(40, 40);
            this.m_btOpen.TabIndex = 0;
            this.m_toolTip.SetToolTip(this.m_btOpen, "Import message layout from XML file");
            this.m_btOpen.UseVisualStyleBackColor = true;
            this.m_btOpen.Click += new System.EventHandler(this.m_btOpen_Click);
            // 
            // m_saveXML
            // 
            this.m_saveXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_saveXML.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_saveXML.Location = new System.Drawing.Point(542, 60);
            this.m_saveXML.Name = "m_saveXML";
            this.m_saveXML.Size = new System.Drawing.Size(40, 40);
            this.m_saveXML.TabIndex = 6;
            this.m_saveXML.UseVisualStyleBackColor = true;
            // 
            // m_openXML
            // 
            this.m_openXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_openXML.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_openXML.Location = new System.Drawing.Point(542, 14);
            this.m_openXML.Name = "m_openXML";
            this.m_openXML.Size = new System.Drawing.Size(40, 40);
            this.m_openXML.TabIndex = 6;
            this.m_openXML.UseVisualStyleBackColor = true;
            // 
            // PluginH1TaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_tabControl);
            this.MinimumSize = new System.Drawing.Size(565, 400);
            this.Name = "PluginH1TaskControl";
            this.Size = new System.Drawing.Size(565, 410);
            this.m_tabControl.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAckTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudSendTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRetryConnectTimeInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTryconnectTimeout)).EndInit();
            this.m_splitContainer.Panel1.ResumeLayout(false);
            this.m_splitContainer.Panel2.ResumeLayout(false);
            this.m_splitContainer.ResumeLayout(false);
            this.NQS1.ResumeLayout(false);
            this.NQS1.PerformLayout();
            this.NQS2.ResumeLayout(false);
            this.NQS2.PerformLayout();
            this.macGroupbox.ResumeLayout(false);
            this.macGroupbox.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.m_splitContainer2.Panel1.ResumeLayout(false);
            this.m_splitContainer2.Panel2.ResumeLayout(false);
            this.m_splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer m_refreshTimer;
        private System.Windows.Forms.TabControl m_tabControl;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.SplitContainer m_splitContainer;
        private System.Windows.Forms.GroupBox macGroupbox;
        private System.Windows.Forms.TextBox m_pcmac6;
        private System.Windows.Forms.TextBox m_pcmac5;
        private System.Windows.Forms.TextBox m_pcmac4;
        private System.Windows.Forms.TextBox m_pcmac3;
        private System.Windows.Forms.TextBox m_pcmac2;
        private System.Windows.Forms.TextBox m_pcmac1;
        private System.Windows.Forms.GroupBox NQS1;
        private System.Windows.Forms.Label m_statusNQS1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox m_tbTSAP_NQS1_NQS;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_tbTSAP_NQS1_PC;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox m_nqs1mac6;
        private System.Windows.Forms.TextBox m_nqs1mac1;
        private System.Windows.Forms.TextBox m_nqs1mac5;
        private System.Windows.Forms.TextBox m_nqs1mac2;
        private System.Windows.Forms.TextBox m_nqs1mac4;
        private System.Windows.Forms.TextBox m_nqs1mac3;
        private System.Windows.Forms.GroupBox NQS2;
        private System.Windows.Forms.Label m_statusNQS2;
        private System.Windows.Forms.TextBox m_tbTSAP_NQS2_NQS;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox m_tbTSAP_NQS2_PC;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox m_nqs2mac3;
        private System.Windows.Forms.TextBox m_nqs2mac4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox m_nqs2mac2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox m_nqs2mac5;
        private System.Windows.Forms.TextBox m_nqs2mac6;
        private System.Windows.Forms.TextBox m_nqs2mac1;
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
        private System.Windows.Forms.Button m_btOpen;
        private System.Windows.Forms.DataGridView m_datagvMessages;
        private System.Windows.Forms.Button m_saveXML;
        private System.Windows.Forms.Button m_openXML;
        private System.Windows.Forms.TreeView m_tvMessages;
        private System.Windows.Forms.SplitContainer m_splitContainer2;
        private System.Windows.Forms.Button m_btSave;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxFieldname;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxComment;
    }
}
