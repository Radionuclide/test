namespace Alunorf_plugin_test
{
    partial class MainForm
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.m_otherTSAP = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.m_ownTSAP = new System.Windows.Forms.TextBox();
            this.m_btStart = new System.Windows.Forms.Button();
            this.m_btStop = new System.Windows.Forms.Button();
            this.m_btGO = new System.Windows.Forms.Button();
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
            this.m_btLoad = new System.Windows.Forms.Button();
            this.m_messages = new System.Windows.Forms.DataGridView();
            this.m_timestampCln = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_messageCln = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_btClearGrid = new System.Windows.Forms.Button();
            this.m_openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.m_outputDir = new System.Windows.Forms.TextBox();
            this.BrowseButton = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.m_folderBrowser = new System.Windows.Forms.FolderBrowserDialog();
            this.m_otherMAC = new Alunorf_plugin_test.matb();
            this.m_ownMAC = new Alunorf_plugin_test.matb();
            this.label6 = new System.Windows.Forms.Label();
            this.BrowseButton2 = new System.Windows.Forms.Button();
            this.m_logfile = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.m_nudMaxgrid = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.m_tcpIPHost = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.m_tcpipPort = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAckTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudSendTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRetryConnectTimeInterval)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTryconnectTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_messages)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMaxgrid)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Own Station Adress:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(107, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Other Station Adress:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(450, 50);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Other TSAP:";
            // 
            // m_otherTSAP
            // 
            this.m_otherTSAP.Location = new System.Drawing.Point(523, 47);
            this.m_otherTSAP.MaxLength = 16;
            this.m_otherTSAP.Name = "m_otherTSAP";
            this.m_otherTSAP.Size = new System.Drawing.Size(165, 20);
            this.m_otherTSAP.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(450, 24);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Own TSAP:";
            // 
            // m_ownTSAP
            // 
            this.m_ownTSAP.Location = new System.Drawing.Point(523, 21);
            this.m_ownTSAP.MaxLength = 16;
            this.m_ownTSAP.Name = "m_ownTSAP";
            this.m_ownTSAP.Size = new System.Drawing.Size(165, 20);
            this.m_ownTSAP.TabIndex = 4;
            // 
            // m_btStart
            // 
            this.m_btStart.Location = new System.Drawing.Point(188, 158);
            this.m_btStart.Name = "m_btStart";
            this.m_btStart.Size = new System.Drawing.Size(159, 34);
            this.m_btStart.TabIndex = 8;
            this.m_btStart.Text = "Start";
            this.m_btStart.UseVisualStyleBackColor = true;
            this.m_btStart.Click += new System.EventHandler(this.m_btStart_Click);
            // 
            // m_btStop
            // 
            this.m_btStop.Location = new System.Drawing.Point(354, 158);
            this.m_btStop.Name = "m_btStop";
            this.m_btStop.Size = new System.Drawing.Size(159, 34);
            this.m_btStop.TabIndex = 9;
            this.m_btStop.Text = "Stop";
            this.m_btStop.UseVisualStyleBackColor = true;
            this.m_btStop.Click += new System.EventHandler(this.m_btStop_Click);
            // 
            // m_btGO
            // 
            this.m_btGO.Location = new System.Drawing.Point(530, 158);
            this.m_btGO.Name = "m_btGO";
            this.m_btGO.Size = new System.Drawing.Size(159, 34);
            this.m_btGO.TabIndex = 10;
            this.m_btGO.Text = "Send GO telegram";
            this.m_btGO.UseVisualStyleBackColor = true;
            this.m_btGO.Click += new System.EventHandler(this.m_btGO_Click);
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label16.Location = new System.Drawing.Point(446, 128);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(115, 13);
            this.label16.TabIndex = 21;
            this.label16.Text = "Acknowledge time out:";
            // 
            // m_nudAckTimeout
            // 
            this.m_nudAckTimeout.Location = new System.Drawing.Point(567, 126);
            this.m_nudAckTimeout.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.m_nudAckTimeout.Name = "m_nudAckTimeout";
            this.m_nudAckTimeout.Size = new System.Drawing.Size(66, 20);
            this.m_nudAckTimeout.TabIndex = 22;
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
            this.label17.Location = new System.Drawing.Point(639, 128);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(47, 13);
            this.label17.TabIndex = 23;
            this.label17.Text = "seconds";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(446, 102);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(75, 13);
            this.label14.TabIndex = 15;
            this.label14.Text = "Send time out:";
            // 
            // m_nudSendTimeout
            // 
            this.m_nudSendTimeout.Location = new System.Drawing.Point(567, 100);
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
            this.m_nudSendTimeout.TabIndex = 16;
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
            this.label15.Location = new System.Drawing.Point(639, 102);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = "seconds";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label12.Location = new System.Drawing.Point(12, 133);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(136, 13);
            this.label12.TabIndex = 18;
            this.label12.Text = "Retry connect time interval:";
            // 
            // m_nudRetryConnectTimeInterval
            // 
            this.m_nudRetryConnectTimeInterval.Location = new System.Drawing.Point(154, 131);
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
            this.m_nudRetryConnectTimeInterval.TabIndex = 19;
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
            this.label13.Location = new System.Drawing.Point(226, 133);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(43, 13);
            this.label13.TabIndex = 20;
            this.label13.Text = "minutes";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label11.Location = new System.Drawing.Point(12, 107);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(107, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Try connect time out:";
            // 
            // m_nudTryconnectTimeout
            // 
            this.m_nudTryconnectTimeout.Location = new System.Drawing.Point(154, 105);
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
            this.m_nudTryconnectTimeout.TabIndex = 13;
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
            this.label10.Location = new System.Drawing.Point(226, 107);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 13);
            this.label10.TabIndex = 14;
            this.label10.Text = "seconds";
            // 
            // m_btLoad
            // 
            this.m_btLoad.Location = new System.Drawing.Point(12, 158);
            this.m_btLoad.Name = "m_btLoad";
            this.m_btLoad.Size = new System.Drawing.Size(159, 34);
            this.m_btLoad.TabIndex = 24;
            this.m_btLoad.Text = "Load telegram configuration";
            this.m_btLoad.UseVisualStyleBackColor = true;
            this.m_btLoad.Click += new System.EventHandler(this.m_btLoad_Click);
            // 
            // m_messages
            // 
            this.m_messages.AllowUserToAddRows = false;
            this.m_messages.AllowUserToDeleteRows = false;
            this.m_messages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_messages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_timestampCln,
            this.m_messageCln});
            this.m_messages.Location = new System.Drawing.Point(12, 268);
            this.m_messages.Name = "m_messages";
            this.m_messages.ReadOnly = true;
            this.m_messages.Size = new System.Drawing.Size(690, 361);
            this.m_messages.TabIndex = 25;
            // 
            // m_timestampCln
            // 
            this.m_timestampCln.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_timestampCln.FillWeight = 20F;
            this.m_timestampCln.HeaderText = "Time stamp";
            this.m_timestampCln.Name = "m_timestampCln";
            this.m_timestampCln.ReadOnly = true;
            // 
            // m_messageCln
            // 
            this.m_messageCln.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_messageCln.FillWeight = 80F;
            this.m_messageCln.HeaderText = "Message";
            this.m_messageCln.Name = "m_messageCln";
            this.m_messageCln.ReadOnly = true;
            // 
            // m_btClearGrid
            // 
            this.m_btClearGrid.Location = new System.Drawing.Point(12, 198);
            this.m_btClearGrid.Name = "m_btClearGrid";
            this.m_btClearGrid.Size = new System.Drawing.Size(159, 34);
            this.m_btClearGrid.TabIndex = 26;
            this.m_btClearGrid.Text = "Clear grid";
            this.m_btClearGrid.UseVisualStyleBackColor = true;
            this.m_btClearGrid.Click += new System.EventHandler(this.m_btClearGrid_Click);
            // 
            // m_openFileDialog1
            // 
            this.m_openFileDialog1.FileName = "messages.xml";
            // 
            // m_outputDir
            // 
            this.m_outputDir.Location = new System.Drawing.Point(100, 236);
            this.m_outputDir.Name = "m_outputDir";
            this.m_outputDir.Size = new System.Drawing.Size(184, 20);
            this.m_outputDir.TabIndex = 28;
            this.m_outputDir.Text = "d:\\klad\\plugintest\\";
            // 
            // BrowseButton
            // 
            this.BrowseButton.Location = new System.Drawing.Point(290, 228);
            this.BrowseButton.Name = "BrowseButton";
            this.BrowseButton.Size = new System.Drawing.Size(66, 34);
            this.BrowseButton.TabIndex = 29;
            this.BrowseButton.Text = "Browse";
            this.BrowseButton.UseVisualStyleBackColor = true;
            this.BrowseButton.Click += new System.EventHandler(this.BrowseButton_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 239);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(85, 13);
            this.label5.TabIndex = 30;
            this.label5.Text = "Output directory:";
            // 
            // m_otherMAC
            // 
            this.m_otherMAC.BackColor = System.Drawing.SystemColors.Window;
            this.m_otherMAC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_otherMAC.Location = new System.Drawing.Point(125, 50);
            this.m_otherMAC.Name = "m_otherMAC";
            this.m_otherMAC.Size = new System.Drawing.Size(171, 24);
            this.m_otherMAC.TabIndex = 27;
            // 
            // m_ownMAC
            // 
            this.m_ownMAC.BackColor = System.Drawing.SystemColors.Window;
            this.m_ownMAC.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.m_ownMAC.Location = new System.Drawing.Point(125, 20);
            this.m_ownMAC.Name = "m_ownMAC";
            this.m_ownMAC.Size = new System.Drawing.Size(171, 24);
            this.m_ownMAC.TabIndex = 27;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(363, 239);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(37, 13);
            this.label6.TabIndex = 33;
            this.label6.Text = "logfile:";
            // 
            // BrowseButton2
            // 
            this.BrowseButton2.Location = new System.Drawing.Point(623, 228);
            this.BrowseButton2.Name = "BrowseButton2";
            this.BrowseButton2.Size = new System.Drawing.Size(66, 34);
            this.BrowseButton2.TabIndex = 32;
            this.BrowseButton2.Text = "Browse";
            this.BrowseButton2.UseVisualStyleBackColor = true;
            this.BrowseButton2.Click += new System.EventHandler(this.BrowseButton2_Click);
            // 
            // m_logfile
            // 
            this.m_logfile.Location = new System.Drawing.Point(402, 236);
            this.m_logfile.Name = "m_logfile";
            this.m_logfile.Size = new System.Drawing.Size(215, 20);
            this.m_logfile.TabIndex = 31;
            this.m_logfile.Text = "d:\\klad\\plugintest\\logfile";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label7.Location = new System.Drawing.Point(185, 209);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(185, 13);
            this.label7.TabIndex = 34;
            this.label7.Text = "Maximum number of messages in grid:";
            // 
            // m_nudMaxgrid
            // 
            this.m_nudMaxgrid.Location = new System.Drawing.Point(376, 207);
            this.m_nudMaxgrid.Maximum = new decimal(new int[] {
            3600,
            0,
            0,
            0});
            this.m_nudMaxgrid.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_nudMaxgrid.Name = "m_nudMaxgrid";
            this.m_nudMaxgrid.Size = new System.Drawing.Size(66, 20);
            this.m_nudMaxgrid.TabIndex = 35;
            this.m_nudMaxgrid.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 80);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(108, 13);
            this.label8.TabIndex = 37;
            this.label8.Text = "Other TCP/IP HOST:";
            // 
            // m_tcpIPHost
            // 
            this.m_tcpIPHost.Location = new System.Drawing.Point(125, 79);
            this.m_tcpIPHost.MaxLength = 16;
            this.m_tcpIPHost.Name = "m_tcpIPHost";
            this.m_tcpIPHost.Size = new System.Drawing.Size(171, 20);
            this.m_tcpIPHost.TabIndex = 36;
            this.m_tcpIPHost.TextChanged += new System.EventHandler(this.m_tcpIPHost_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(302, 82);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(43, 13);
            this.label9.TabIndex = 38;
            this.label9.Text = "Port Nr:";
            // 
            // m_tcpipPort
            // 
            this.m_tcpipPort.Location = new System.Drawing.Point(351, 77);
            this.m_tcpipPort.Name = "m_tcpipPort";
            this.m_tcpipPort.Size = new System.Drawing.Size(100, 20);
            this.m_tcpipPort.TabIndex = 39;
            this.m_tcpipPort.Text = "8000";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(714, 654);
            this.Controls.Add(this.m_tcpipPort);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.m_tcpIPHost);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.m_nudMaxgrid);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.BrowseButton2);
            this.Controls.Add(this.m_logfile);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.BrowseButton);
            this.Controls.Add(this.m_outputDir);
            this.Controls.Add(this.m_otherMAC);
            this.Controls.Add(this.m_ownMAC);
            this.Controls.Add(this.m_btClearGrid);
            this.Controls.Add(this.m_messages);
            this.Controls.Add(this.m_btLoad);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.m_nudAckTimeout);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.m_nudSendTimeout);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.m_nudRetryConnectTimeInterval);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.m_nudTryconnectTimeout);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.m_btGO);
            this.Controls.Add(this.m_btStop);
            this.Controls.Add(this.m_btStart);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_otherTSAP);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.m_ownTSAP);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.m_nudAckTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudSendTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudRetryConnectTimeInterval)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTryconnectTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_messages)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMaxgrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_otherTSAP;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox m_ownTSAP;
        private System.Windows.Forms.Button m_btStart;
        private System.Windows.Forms.Button m_btStop;
        private System.Windows.Forms.Button m_btGO;
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
        private System.Windows.Forms.Button m_btLoad;
        private System.Windows.Forms.DataGridView m_messages;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_timestampCln;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_messageCln;
        private System.Windows.Forms.Button m_btClearGrid;
        private matb m_ownMAC;
        private matb m_otherMAC;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog1;
        private System.Windows.Forms.TextBox m_outputDir;
        private System.Windows.Forms.Button BrowseButton;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowser;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button BrowseButton2;
        private System.Windows.Forms.TextBox m_logfile;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown m_nudMaxgrid;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox m_tcpIPHost;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox m_tcpipPort;
    }
}

