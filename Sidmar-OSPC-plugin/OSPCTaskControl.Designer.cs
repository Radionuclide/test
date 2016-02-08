namespace AM_OSPC_plugin
{
    partial class OSPCTaskControl
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
            if(disposing && (components != null))
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
            this.m_datagvMessages = new System.Windows.Forms.DataGridView();
            this.m_columnAnalyzerExpression = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_columnProcessName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_columnVariableName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_columnTestValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label2 = new System.Windows.Forms.Label();
            this.m_pdoFileTextBox = new System.Windows.Forms.TextBox();
            this.m_browsePDOFileButton = new System.Windows.Forms.Button();
            this.m_executeIBAAButton = new System.Windows.Forms.Button();
            this.m_browseDatFileButton = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.m_datFileTextBox = new System.Windows.Forms.TextBox();
            this.m_testButton = new System.Windows.Forms.Button();
            this.m_toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.m_openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.m_ospcPassword = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.m_ospcUsername = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.m_ospcHost = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).BeginInit();
            this.SuspendLayout();
            // 
            // m_datagvMessages
            // 
            this.m_datagvMessages.AllowUserToAddRows = false;
            this.m_datagvMessages.AllowUserToDeleteRows = false;
            this.m_datagvMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_datagvMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_datagvMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_columnAnalyzerExpression,
            this.m_columnProcessName,
            this.m_columnVariableName,
            this.m_columnTestValue});
            this.m_datagvMessages.Location = new System.Drawing.Point(0, 129);
            this.m_datagvMessages.Name = "m_datagvMessages";
            this.m_datagvMessages.Size = new System.Drawing.Size(565, 185);
            this.m_datagvMessages.TabIndex = 1;
            this.m_datagvMessages.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.m_datagvMessages_RowPostPaint);
            // 
            // m_columnAnalyzerExpression
            // 
            this.m_columnAnalyzerExpression.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnAnalyzerExpression.DataPropertyName = "Expression";
            this.m_columnAnalyzerExpression.FillWeight = 30F;
            this.m_columnAnalyzerExpression.HeaderText = "ibaAnalyzer Expression";
            this.m_columnAnalyzerExpression.Name = "m_columnAnalyzerExpression";
            this.m_columnAnalyzerExpression.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_columnProcessName
            // 
            this.m_columnProcessName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnProcessName.DataPropertyName = "ProcessName";
            this.m_columnProcessName.FillWeight = 20F;
            this.m_columnProcessName.HeaderText = "OSPC Process Name";
            this.m_columnProcessName.Name = "m_columnProcessName";
            this.m_columnProcessName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.m_columnProcessName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_columnVariableName
            // 
            this.m_columnVariableName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnVariableName.DataPropertyName = "VariableName";
            this.m_columnVariableName.FillWeight = 20F;
            this.m_columnVariableName.HeaderText = "OSPC Variable Name";
            this.m_columnVariableName.Name = "m_columnVariableName";
            this.m_columnVariableName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_columnTestValue
            // 
            this.m_columnTestValue.DataPropertyName = "TestValueString";
            this.m_columnTestValue.HeaderText = "Test Value";
            this.m_columnTestValue.Name = "m_columnTestValue";
            this.m_columnTestValue.ReadOnly = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label2.Location = new System.Drawing.Point(3, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Optional analysis:";
            // 
            // m_pdoFileTextBox
            // 
            this.m_pdoFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_pdoFileTextBox.Location = new System.Drawing.Point(110, 94);
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.Size = new System.Drawing.Size(357, 20);
            this.m_pdoFileTextBox.TabIndex = 5;
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
            // 
            // m_browsePDOFileButton
            // 
            this.m_browsePDOFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browsePDOFileButton.Image = global::AM_OSPC_plugin.Properties.Resources.open;
            this.m_browsePDOFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browsePDOFileButton.Location = new System.Drawing.Point(473, 83);
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_browsePDOFileButton.Size = new System.Drawing.Size(40, 40);
            this.m_browsePDOFileButton.TabIndex = 6;
            this.m_toolTip.SetToolTip(this.m_browsePDOFileButton, "Browse for analysis file");
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            this.m_executeIBAAButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.m_executeIBAAButton.Image = global::AM_OSPC_plugin.Properties.Resources.Analyzer_001;
            this.m_executeIBAAButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_executeIBAAButton.Location = new System.Drawing.Point(519, 83);
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_executeIBAAButton.Size = new System.Drawing.Size(40, 40);
            this.m_executeIBAAButton.TabIndex = 7;
            this.m_toolTip.SetToolTip(this.m_executeIBAAButton, "Start ibaAnalyzer");
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // m_browseDatFileButton
            // 
            this.m_browseDatFileButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_browseDatFileButton.Image = global::AM_OSPC_plugin.Properties.Resources.open;
            this.m_browseDatFileButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_browseDatFileButton.Location = new System.Drawing.Point(473, 320);
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_browseDatFileButton.Size = new System.Drawing.Size(40, 40);
            this.m_browseDatFileButton.TabIndex = 17;
            this.m_toolTip.SetToolTip(this.m_browseDatFileButton, "Browse for .dat file");
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label3.Location = new System.Drawing.Point(3, 334);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(84, 13);
            this.label3.TabIndex = 15;
            this.label3.Text = "Example .dat file";
            // 
            // m_datFileTextBox
            // 
            this.m_datFileTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_datFileTextBox.Location = new System.Drawing.Point(110, 331);
            this.m_datFileTextBox.Name = "m_datFileTextBox";
            this.m_datFileTextBox.Size = new System.Drawing.Size(357, 20);
            this.m_datFileTextBox.TabIndex = 16;
            this.m_datFileTextBox.TextChanged += new System.EventHandler(this.m_datFileTextBox_TextChanged);
            // 
            // m_testButton
            // 
            this.m_testButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.m_testButton.Image = global::AM_OSPC_plugin.Properties.Resources.select;
            this.m_testButton.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.m_testButton.Location = new System.Drawing.Point(519, 320);
            this.m_testButton.Name = "m_testButton";
            this.m_testButton.Size = new System.Drawing.Size(40, 40);
            this.m_testButton.TabIndex = 18;
            this.m_toolTip.SetToolTip(this.m_testButton, "Test by updating values in grid");
            this.m_testButton.UseVisualStyleBackColor = true;
            this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
            // 
            // m_ospcPassword
            // 
            this.m_ospcPassword.Location = new System.Drawing.Point(110, 59);
            this.m_ospcPassword.Name = "m_ospcPassword";
            this.m_ospcPassword.Size = new System.Drawing.Size(189, 20);
            this.m_ospcPassword.TabIndex = 24;
            this.m_ospcPassword.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(43, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 13);
            this.label6.TabIndex = 23;
            this.label6.Text = "Password:";
            // 
            // m_ospcUsername
            // 
            this.m_ospcUsername.Location = new System.Drawing.Point(110, 33);
            this.m_ospcUsername.Name = "m_ospcUsername";
            this.m_ospcUsername.Size = new System.Drawing.Size(189, 20);
            this.m_ospcUsername.TabIndex = 22;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(43, 36);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(58, 13);
            this.label5.TabIndex = 21;
            this.label5.Text = "Username:";
            // 
            // m_ospcHost
            // 
            this.m_ospcHost.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.m_ospcHost.Location = new System.Drawing.Point(110, 7);
            this.m_ospcHost.Name = "m_ospcHost";
            this.m_ospcHost.Size = new System.Drawing.Size(449, 20);
            this.m_ospcHost.TabIndex = 20;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(98, 13);
            this.label1.TabIndex = 19;
            this.label1.Text = "OSPC Server Host:";
            // 
            // OSPCTaskControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_ospcPassword);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.m_ospcUsername);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.m_ospcHost);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.m_browseDatFileButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_datFileTextBox);
            this.Controls.Add(this.m_testButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_pdoFileTextBox);
            this.Controls.Add(this.m_browsePDOFileButton);
            this.Controls.Add(this.m_executeIBAAButton);
            this.Controls.Add(this.m_datagvMessages);
            this.MinimumSize = new System.Drawing.Size(0, 230);
            this.Name = "OSPCTaskControl";
            this.Size = new System.Drawing.Size(565, 370);
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView m_datagvMessages;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox m_pdoFileTextBox;
        private System.Windows.Forms.Button m_browsePDOFileButton;
        private System.Windows.Forms.Button m_executeIBAAButton;
        private System.Windows.Forms.Button m_browseDatFileButton;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox m_datFileTextBox;
        private System.Windows.Forms.Button m_testButton;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.OpenFileDialog m_openFileDialog;
        private System.Windows.Forms.TextBox m_ospcPassword;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox m_ospcUsername;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox m_ospcHost;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_columnAnalyzerExpression;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_columnProcessName;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_columnVariableName;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_columnTestValue;
    }
}
