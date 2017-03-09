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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OSPCTaskControl));
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
            this.m_monitorGroup = new System.Windows.Forms.GroupBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.m_nudTime = new System.Windows.Forms.NumericUpDown();
            this.m_nudMemory = new System.Windows.Forms.NumericUpDown();
            this.m_cbTime = new System.Windows.Forms.CheckBox();
            this.m_cbMemory = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).BeginInit();
            this.m_monitorGroup.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).BeginInit();
            this.SuspendLayout();
            // 
            // m_datagvMessages
            // 
            this.m_datagvMessages.AllowUserToAddRows = false;
            this.m_datagvMessages.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.m_datagvMessages, "m_datagvMessages");
            this.m_datagvMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_datagvMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.m_columnAnalyzerExpression,
            this.m_columnProcessName,
            this.m_columnVariableName,
            this.m_columnTestValue});
            this.m_datagvMessages.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.m_datagvMessages.Name = "m_datagvMessages";
            this.m_datagvMessages.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.m_datagvMessages_RowPostPaint);
            // 
            // m_columnAnalyzerExpression
            // 
            this.m_columnAnalyzerExpression.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnAnalyzerExpression.DataPropertyName = "Expression";
            this.m_columnAnalyzerExpression.FillWeight = 30F;
            resources.ApplyResources(this.m_columnAnalyzerExpression, "m_columnAnalyzerExpression");
            this.m_columnAnalyzerExpression.Name = "m_columnAnalyzerExpression";
            this.m_columnAnalyzerExpression.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_columnProcessName
            // 
            this.m_columnProcessName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnProcessName.DataPropertyName = "ProcessName";
            this.m_columnProcessName.FillWeight = 20F;
            resources.ApplyResources(this.m_columnProcessName, "m_columnProcessName");
            this.m_columnProcessName.Name = "m_columnProcessName";
            this.m_columnProcessName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.m_columnProcessName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_columnVariableName
            // 
            this.m_columnVariableName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.m_columnVariableName.DataPropertyName = "VariableName";
            this.m_columnVariableName.FillWeight = 20F;
            resources.ApplyResources(this.m_columnVariableName, "m_columnVariableName");
            this.m_columnVariableName.Name = "m_columnVariableName";
            this.m_columnVariableName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // m_columnTestValue
            // 
            this.m_columnTestValue.DataPropertyName = "TestValueString";
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
            this.m_browsePDOFileButton.Image = global::AM_OSPC_plugin.Properties.Resources.open;
            this.m_browsePDOFileButton.Name = "m_browsePDOFileButton";
            this.m_toolTip.SetToolTip(this.m_browsePDOFileButton, resources.GetString("m_browsePDOFileButton.ToolTip"));
            this.m_browsePDOFileButton.UseVisualStyleBackColor = true;
            this.m_browsePDOFileButton.Click += new System.EventHandler(this.m_browsePDOFileButton_Click);
            // 
            // m_executeIBAAButton
            // 
            resources.ApplyResources(this.m_executeIBAAButton, "m_executeIBAAButton");
            this.m_executeIBAAButton.Image = global::AM_OSPC_plugin.Properties.Resources.Analyzer_001;
            this.m_executeIBAAButton.Name = "m_executeIBAAButton";
            this.m_toolTip.SetToolTip(this.m_executeIBAAButton, resources.GetString("m_executeIBAAButton.ToolTip"));
            this.m_executeIBAAButton.UseVisualStyleBackColor = true;
            this.m_executeIBAAButton.Click += new System.EventHandler(this.m_executeIBAAButton_Click);
            // 
            // m_browseDatFileButton
            // 
            resources.ApplyResources(this.m_browseDatFileButton, "m_browseDatFileButton");
            this.m_browseDatFileButton.Image = global::AM_OSPC_plugin.Properties.Resources.open;
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
            this.m_testButton.Image = global::AM_OSPC_plugin.Properties.Resources.select;
            this.m_testButton.Name = "m_testButton";
            this.m_toolTip.SetToolTip(this.m_testButton, resources.GetString("m_testButton.ToolTip"));
            this.m_testButton.UseVisualStyleBackColor = true;
            this.m_testButton.Click += new System.EventHandler(this.m_testButton_Click);
            // 
            // m_ospcPassword
            // 
            resources.ApplyResources(this.m_ospcPassword, "m_ospcPassword");
            this.m_ospcPassword.Name = "m_ospcPassword";
            this.m_ospcPassword.UseSystemPasswordChar = true;
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // m_ospcUsername
            // 
            resources.ApplyResources(this.m_ospcUsername, "m_ospcUsername");
            this.m_ospcUsername.Name = "m_ospcUsername";
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // m_ospcHost
            // 
            resources.ApplyResources(this.m_ospcHost, "m_ospcHost");
            this.m_ospcHost.Name = "m_ospcHost";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
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
            // OSPCTaskControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.m_monitorGroup);
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
            ((System.ComponentModel.ISupportInitialize)(this.m_datagvMessages)).EndInit();
            this.m_monitorGroup.ResumeLayout(false);
            this.m_monitorGroup.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.m_nudMemory)).EndInit();
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
        private System.Windows.Forms.GroupBox m_monitorGroup;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown m_nudTime;
        private System.Windows.Forms.NumericUpDown m_nudMemory;
        private System.Windows.Forms.CheckBox m_cbTime;
        private System.Windows.Forms.CheckBox m_cbMemory;
    }
}
