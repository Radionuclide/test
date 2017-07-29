namespace S7_writer_plugin
{
    partial class S7TaskControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(S7TaskControl));
            this.m_datagvMessages = new System.Windows.Forms.DataGridView();
            this.m_columnAnalyzerExpression = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_columnDBNr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.columnBitNr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.m_columnDataType = new System.Windows.Forms.DataGridViewComboBoxColumn();
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
            this.m_monitorGroup = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            this.spTimeout = new System.Windows.Forms.NumericUpDown();
            this.cbConnType = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.spRack = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.tbAddress = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.spSlot = new System.Windows.Forms.NumericUpDown();
            this.ckAllowErrors = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).BeginInit();
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTimeout)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spRack)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSlot)).BeginInit();
            this.SuspendLayout();
            // 
            // m_datagvMessages
            // 
            resources.ApplyResources(this.m_datagvMessages, "m_datagvMessages");
            this.m_datagvMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_datagvMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_columnAnalyzerExpression,
            this.m_columnDBNr,
            this.columnAddress,
            this.columnBitNr,
            this.m_columnDataType,
            this.m_columnTestValue});
            this.m_datagvMessages.Name = "m_datagvMessages";
            this.m_datagvMessages.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.m_datagvMessages_DataError);
            this.m_datagvMessages.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.m_datagvMessages_RowPostPaint);
            // 
            // m_columnAnalyzerExpression
            // 
            this.m_columnAnalyzerExpression.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnAnalyzerExpression.DataPropertyName = "Expression";
            this.m_columnAnalyzerExpression.FillWeight = 40F;
            resources.ApplyResources(this.m_columnAnalyzerExpression, "m_columnAnalyzerExpression");
            this.m_columnAnalyzerExpression.Name = "m_columnAnalyzerExpression";
            this.m_columnAnalyzerExpression.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_columnDBNr
            // 
            this.m_columnDBNr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnDBNr.DataPropertyName = "DBNr";
            this.m_columnDBNr.FillWeight = 10F;
            resources.ApplyResources(this.m_columnDBNr, "m_columnDBNr");
            this.m_columnDBNr.Name = "m_columnDBNr";
            this.m_columnDBNr.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.m_columnDBNr.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // columnAddress
            // 
            this.columnAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnAddress.DataPropertyName = "Address";
            this.columnAddress.FillWeight = 10F;
            resources.ApplyResources(this.columnAddress, "columnAddress");
            this.columnAddress.Name = "columnAddress";
            // 
            // columnBitNr
            // 
            this.columnBitNr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.columnBitNr.DataPropertyName = "BitNr";
            this.columnBitNr.FillWeight = 10F;
            resources.ApplyResources(this.columnBitNr, "columnBitNr");
            this.columnBitNr.Name = "columnBitNr";
            // 
            // m_columnDataType
            // 
            this.m_columnDataType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnDataType.DataPropertyName = "DataTypeAsString";
            this.m_columnDataType.FillWeight = 20F;
            resources.ApplyResources(this.m_columnDataType, "m_columnDataType");
            this.m_columnDataType.Items.AddRange(new object[] {
            "BOOL",
            "BYTE",
            "CHAR",
            "WORD",
            "INT",
            "DWORD",
            "DINT",
            "REAL"});
            this.m_columnDataType.Name = "m_columnDataType";
            this.m_columnDataType.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // m_columnTestValue
            // 
            this.m_columnTestValue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnTestValue.DataPropertyName = "TestValueString";
            this.m_columnTestValue.FillWeight = 20F;
            resources.ApplyResources(this.m_columnTestValue, "m_columnTestValue");
            this.m_columnTestValue.Name = "m_columnTestValue";
            this.m_columnTestValue.ReadOnly = true;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // m_pdoFileTextBox
            // 
            resources.ApplyResources(this.m_pdoFileTextBox, "m_pdoFileTextBox");
            this.m_pdoFileTextBox.Name = "m_pdoFileTextBox";
            this.m_pdoFileTextBox.TextChanged += new System.EventHandler(this.m_pdoFileTextBox_TextChanged);
            // 
            // m_browsePDOFileButton
            // 
            resources.ApplyResources(this.m_browsePDOFileButton, "m_browsePDOFileButton");
            this.m_browsePDOFileButton.Image = global::S7_writer_plugin.Properties.Resources.open;
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_toolTip.SetToolTip(this.m_browsePDOFileButton, resources.GetString("m_browsePDOFileButton.ToolTip"));
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.Image = global::S7_writer_plugin.Properties.Resources.Analyzer_001;
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_toolTip.SetToolTip(this.m_executeIBAAButton, resources.GetString("m_executeIBAAButton.ToolTip"));
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // m_browseDatFileButton
            // 
            resources.ApplyResources(this.m_browseDatFileButton, "m_browseDatFileButton");
            this.m_browseDatFileButton.Image = global::S7_writer_plugin.Properties.Resources.open;
            this.m_browseDatFileButton.Name = "m_browseDatFileButton";
            this.m_toolTip.SetToolTip(this.m_browseDatFileButton, resources.GetString("m_browseDatFileButton.ToolTip"));
            this.m_browseDatFileButton.UseVisualStyleBackColor = true;
            this.m_browseDatFileButton.Click += new System.EventHandler(this.m_browseDatFileButton_Click);
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // m_datFileTextBox
            // 
            resources.ApplyResources(this.m_datFileTextBox, "m_datFileTextBox");
            this.m_datFileTextBox.Name = "m_datFileTextBox";
            this.m_datFileTextBox.TextChanged += new System.EventHandler(this.m_datFileTextBox_TextChanged);
            // 
            // m_testButton
            // 
            resources.ApplyResources(this.m_testButton, "m_testButton");
            this.m_testButton.Image = global::S7_writer_plugin.Properties.Resources.select;
            this.m_testButton.Name = "m_testButton";
            this.m_toolTip.SetToolTip(this.m_testButton, resources.GetString("m_testButton.ToolTip"));
            this.m_testButton.UseVisualStyleBackColor = true;
            this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
            // 
            // m_monitorGroup
            // 
            resources.ApplyResources(this.m_monitorGroup, "m_monitorGroup");
            this.m_monitorGroup.Controls.Add(this.label4);
            this.m_monitorGroup.Controls.Add(this.label7);
            this.m_monitorGroup.Controls.Add(this.m_nudTime);
            this.m_monitorGroup.Controls.Add(this.m_nudMemory);
            this.m_monitorGroup.Controls.Add(this.m_cbTime);
            this.m_monitorGroup.Controls.Add(this.m_cbMemory);
            this.m_monitorGroup.Name = "m_monitorGroup";
            this.m_monitorGroup.TabStop = false;
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
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
            // spTimeout
            // 
            resources.ApplyResources(this.spTimeout, "spTimeout");
            this.spTimeout.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.spTimeout.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spTimeout.Name = "spTimeout";
            this.spTimeout.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbConnType
            // 
            resources.ApplyResources(this.cbConnType, "cbConnType");
            this.cbConnType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbConnType.Items.AddRange(new object[] {
            resources.GetString("cbConnType.Items"),
            resources.GetString("cbConnType.Items1"),
            resources.GetString("cbConnType.Items2")});
            this.cbConnType.Name = "cbConnType";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // label9
            // 
            resources.ApplyResources(this.label9, "label9");
            this.label9.Name = "label9";
            // 
            // spRack
            // 
            resources.ApplyResources(this.spRack, "spRack");
            this.spRack.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spRack.Name = "spRack";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // tbAddress
            // 
            resources.ApplyResources(this.tbAddress, "tbAddress");
            this.tbAddress.Name = "tbAddress";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // spSlot
            // 
            resources.ApplyResources(this.spSlot, "spSlot");
            this.spSlot.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.spSlot.Name = "spSlot";
            // 
            // ckAllowErrors
            // 
            resources.ApplyResources(this.ckAllowErrors, "ckAllowErrors");
            this.ckAllowErrors.Name = "ckAllowErrors";
            this.ckAllowErrors.UseVisualStyleBackColor = true;
            // 
            // S7TaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.ckAllowErrors);
            this.Controls.Add(this.spRack);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbAddress);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.spSlot);
            this.Controls.Add(this.spTimeout);
            this.Controls.Add(this.cbConnType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.m_monitorGroup);
            this.Controls.Add(this.m_browseDatFileButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.m_datFileTextBox);
            this.Controls.Add(this.m_testButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.m_pdoFileTextBox);
            this.Controls.Add(this.m_browsePDOFileButton);
            this.Controls.Add(this.m_executeIBAAButton);
            this.Controls.Add(this.m_datagvMessages);
            this.Name = "S7TaskControl";
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).EndInit();
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spTimeout)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spRack)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spSlot)).EndInit();
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
        private System.Windows.Forms.GroupBox m_monitorGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
        private System.Windows.Forms.NumericUpDown spTimeout;
        private System.Windows.Forms.ComboBox cbConnType;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown spRack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbAddress;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown spSlot;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_columnAnalyzerExpression;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_columnDBNr;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn columnBitNr;
        private System.Windows.Forms.DataGridViewComboBoxColumn m_columnDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn m_columnTestValue;
        private System.Windows.Forms.CheckBox ckAllowErrors;
    }
}
