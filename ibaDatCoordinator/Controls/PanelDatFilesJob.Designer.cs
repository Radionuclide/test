namespace iba.Controls
{
    partial class PanelDatFilesJob
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PanelDatFilesJob));
			this.m_toolTip = new System.Windows.Forms.ToolTip();
			this.m_folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
			this.m_selectDatFilesDialog = new System.Windows.Forms.OpenFileDialog();
			this.gbCycle = new iba.Utility.CollapsibleGroupBox();
			this.m_cbRepErr = new System.Windows.Forms.CheckBox();
			this.m_undoChangesBtn = new System.Windows.Forms.Button();
			this.m_cbDetectNewFiles = new System.Windows.Forms.CheckBox();
			this.m_cbInitialScanEnabled = new System.Windows.Forms.CheckBox();
			this.m_cbRetry = new System.Windows.Forms.CheckBox();
			this.m_retryUpDown = new System.Windows.Forms.NumericUpDown();
			this.label14 = new System.Windows.Forms.Label();
			this.m_cbRescanEnabled = new System.Windows.Forms.CheckBox();
			this.m_refreshDats = new System.Windows.Forms.Button();
			this.m_stopButton = new System.Windows.Forms.Button();
			this.m_applyToRunningBtn = new System.Windows.Forms.Button();
			this.m_startButton = new System.Windows.Forms.Button();
			this.m_autoStartCheckBox = new System.Windows.Forms.CheckBox();
			this.m_enableCheckBox = new System.Windows.Forms.CheckBox();
			this.m_failTimeUpDown = new System.Windows.Forms.NumericUpDown();
			this.label10 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.m_scanTimeUpDown = new System.Windows.Forms.NumericUpDown();
			this.gbDatFiles = new iba.Utility.CollapsibleGroupBox();
			this.tbFilePassword = new System.Windows.Forms.TextBox();
			this.lblFilePassword = new System.Windows.Forms.Label();
			this.m_browseDatFilesButton = new System.Windows.Forms.Button();
			this.m_tbPass = new System.Windows.Forms.TextBox();
			this.label12 = new System.Windows.Forms.Label();
			this.m_tbUserName = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.m_checkPathButton = new System.Windows.Forms.Button();
			this.m_browseFolderButton = new System.Windows.Forms.Button();
			this.m_subMapsCheckBox = new System.Windows.Forms.CheckBox();
			this.m_datDirTextBox = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.gbCycle.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_retryUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_failTimeUpDown)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.m_scanTimeUpDown)).BeginInit();
			this.gbDatFiles.SuspendLayout();
			this.SuspendLayout();
			// 
			// m_selectDatFilesDialog
			// 
			this.m_selectDatFilesDialog.DefaultExt = "dat";
			this.m_selectDatFilesDialog.FileName = "openFileDialog1";
			this.m_selectDatFilesDialog.Multiselect = true;
			// 
			// groupBox3
			// 
			resources.ApplyResources(this.gbCycle, "gbCycle");
			this.gbCycle.Controls.Add(this.m_cbRepErr);
			this.gbCycle.Controls.Add(this.m_undoChangesBtn);
			this.gbCycle.Controls.Add(this.m_cbDetectNewFiles);
			this.gbCycle.Controls.Add(this.m_cbInitialScanEnabled);
			this.gbCycle.Controls.Add(this.m_cbRetry);
			this.gbCycle.Controls.Add(this.m_retryUpDown);
			this.gbCycle.Controls.Add(this.label14);
			this.gbCycle.Controls.Add(this.m_cbRescanEnabled);
			this.gbCycle.Controls.Add(this.m_refreshDats);
			this.gbCycle.Controls.Add(this.m_stopButton);
			this.gbCycle.Controls.Add(this.m_applyToRunningBtn);
			this.gbCycle.Controls.Add(this.m_startButton);
			this.gbCycle.Controls.Add(this.m_autoStartCheckBox);
			this.gbCycle.Controls.Add(this.m_enableCheckBox);
			this.gbCycle.Controls.Add(this.m_failTimeUpDown);
			this.gbCycle.Controls.Add(this.label10);
			this.gbCycle.Controls.Add(this.label4);
			this.gbCycle.Controls.Add(this.m_scanTimeUpDown);
			this.gbCycle.Name = "gbCycle";
			this.gbCycle.TabStop = false;
			// 
			// m_cbRepErr
			// 
			resources.ApplyResources(this.m_cbRepErr, "m_cbRepErr");
			this.m_cbRepErr.Name = "m_cbRepErr";
			this.m_cbRepErr.UseVisualStyleBackColor = true;
			// 
			// m_undoChangesBtn
			// 
			resources.ApplyResources(this.m_undoChangesBtn, "m_undoChangesBtn");
			this.m_undoChangesBtn.Name = "m_undoChangesBtn";
			this.m_undoChangesBtn.UseVisualStyleBackColor = true;
			// 
			// m_cbDetectNewFiles
			// 
			resources.ApplyResources(this.m_cbDetectNewFiles, "m_cbDetectNewFiles");
			this.m_cbDetectNewFiles.Name = "m_cbDetectNewFiles";
			this.m_cbDetectNewFiles.UseVisualStyleBackColor = true;
			this.m_cbDetectNewFiles.CheckedChanged += new System.EventHandler(this.m_cbDetectNewFiles_CheckedChanged);
			// 
			// m_cbInitialScanEnabled
			// 
			resources.ApplyResources(this.m_cbInitialScanEnabled, "m_cbInitialScanEnabled");
			this.m_cbInitialScanEnabled.Name = "m_cbInitialScanEnabled";
			this.m_cbInitialScanEnabled.UseVisualStyleBackColor = true;
			// 
			// m_cbRetry
			// 
			resources.ApplyResources(this.m_cbRetry, "m_cbRetry");
			this.m_cbRetry.Name = "m_cbRetry";
			this.m_cbRetry.UseVisualStyleBackColor = true;
			this.m_cbRetry.CheckedChanged += new System.EventHandler(this.m_cbRetry_CheckedChanged);
			// 
			// m_retryUpDown
			// 
			resources.ApplyResources(this.m_retryUpDown, "m_retryUpDown");
			this.m_retryUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.m_retryUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.m_retryUpDown.Name = "m_retryUpDown";
            this.m_retryUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// label14
			// 
			resources.ApplyResources(this.label14, "label14");
			this.label14.Name = "label14";
			// 
			// m_cbRescanEnabled
			// 
			resources.ApplyResources(this.m_cbRescanEnabled, "m_cbRescanEnabled");
			this.m_cbRescanEnabled.Name = "m_cbRescanEnabled";
			this.m_cbRescanEnabled.UseVisualStyleBackColor = true;
			// 
			// m_refreshDats
			// 
			resources.ApplyResources(this.m_refreshDats, "m_refreshDats");
			this.m_refreshDats.Name = "m_refreshDats";
			this.m_refreshDats.UseVisualStyleBackColor = true;
			this.m_refreshDats.Click += new System.EventHandler(this.m_refreshDats_Click);
			// 
			// m_stopButton
			// 
			resources.ApplyResources(this.m_stopButton, "m_stopButton");
			this.m_stopButton.Name = "m_stopButton";
			this.m_stopButton.UseVisualStyleBackColor = true;
			this.m_stopButton.Click += new System.EventHandler(this.m_stopButton_Click);
			// 
			// m_applyToRunningBtn
			// 
			resources.ApplyResources(this.m_applyToRunningBtn, "m_applyToRunningBtn");
			this.m_applyToRunningBtn.Name = "m_applyToRunningBtn";
			this.m_applyToRunningBtn.UseVisualStyleBackColor = true;
			// 
			// m_startButton
			// 
			resources.ApplyResources(this.m_startButton, "m_startButton");
			this.m_startButton.Name = "m_startButton";
			this.m_startButton.UseVisualStyleBackColor = true;
			this.m_startButton.Click += new System.EventHandler(this.m_startButton_Click);
			// 
			// m_autoStartCheckBox
			// 
			resources.ApplyResources(this.m_autoStartCheckBox, "m_autoStartCheckBox");
			this.m_autoStartCheckBox.Name = "m_autoStartCheckBox";
			this.m_autoStartCheckBox.UseVisualStyleBackColor = true;
			// 
			// m_enableCheckBox
			// 
			resources.ApplyResources(this.m_enableCheckBox, "m_enableCheckBox");
			this.m_enableCheckBox.Name = "m_enableCheckBox";
			this.m_enableCheckBox.UseVisualStyleBackColor = true;
			// 
			// m_failTimeUpDown
			// 
			this.m_failTimeUpDown.DecimalPlaces = 1;
			resources.ApplyResources(this.m_failTimeUpDown, "m_failTimeUpDown");
			this.m_failTimeUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.m_failTimeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.m_failTimeUpDown.Name = "m_failTimeUpDown";
            this.m_failTimeUpDown.Value = new decimal(new int[] {
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
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // m_scanTimeUpDown
            // 
            this.m_scanTimeUpDown.DecimalPlaces = 1;
            resources.ApplyResources(this.m_scanTimeUpDown, "m_scanTimeUpDown");
            this.m_scanTimeUpDown.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.m_scanTimeUpDown.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.m_scanTimeUpDown.Name = "m_scanTimeUpDown";
            this.m_scanTimeUpDown.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
			// 
			// groupBox1
			// 
			resources.ApplyResources(this.gbDatFiles, "gbDatFiles");
			this.gbDatFiles.Controls.Add(this.tbFilePassword);
			this.gbDatFiles.Controls.Add(this.lblFilePassword);
			this.gbDatFiles.Controls.Add(this.m_browseDatFilesButton);
			this.gbDatFiles.Controls.Add(this.m_tbPass);
			this.gbDatFiles.Controls.Add(this.label12);
			this.gbDatFiles.Controls.Add(this.m_tbUserName);
			this.gbDatFiles.Controls.Add(this.label3);
			this.gbDatFiles.Controls.Add(this.m_checkPathButton);
			this.gbDatFiles.Controls.Add(this.m_browseFolderButton);
			this.gbDatFiles.Controls.Add(this.m_subMapsCheckBox);
			this.gbDatFiles.Controls.Add(this.m_datDirTextBox);
			this.gbDatFiles.Controls.Add(this.label2);
			this.gbDatFiles.Name = "gbDatFiles";
			this.gbDatFiles.TabStop = false;
			// 
			// m_tbFilePwd
			// 
			resources.ApplyResources(this.tbFilePassword, "tbFilePassword");
			this.tbFilePassword.Name = "tbFilePassword";
			this.tbFilePassword.UseSystemPasswordChar = true;
			// 
			// label21
			// 
			resources.ApplyResources(this.lblFilePassword, "lblFilePassword");
			this.lblFilePassword.Name = "lblFilePassword";
			// 
			// m_browseDatFilesButton
			// 
			resources.ApplyResources(this.m_browseDatFilesButton, "m_browseDatFilesButton");
			this.m_browseDatFilesButton.Name = "m_browseDatFilesButton";
			this.m_browseDatFilesButton.UseVisualStyleBackColor = true;
			this.m_browseDatFilesButton.Click += new System.EventHandler(this.m_browseDatFilesButton_Click);
			// 
			// m_tbPass
			// 
			resources.ApplyResources(this.m_tbPass, "m_tbPass");
			this.m_tbPass.Name = "m_tbPass";
			this.m_tbPass.UseSystemPasswordChar = true;
			// 
			// label12
			// 
			resources.ApplyResources(this.label12, "label12");
			this.label12.Name = "label12";
			// 
			// m_tbUserName
			// 
			resources.ApplyResources(this.m_tbUserName, "m_tbUserName");
			this.m_tbUserName.Name = "m_tbUserName";
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			// 
			// m_checkPathButton
			// 
			resources.ApplyResources(this.m_checkPathButton, "m_checkPathButton");
			this.m_checkPathButton.Name = "m_checkPathButton";
			this.m_checkPathButton.UseVisualStyleBackColor = true;
			this.m_checkPathButton.Click += new System.EventHandler(this.m_checkPathButton_Click);
			// 
			// m_browseFolderButton
			// 
			resources.ApplyResources(this.m_browseFolderButton, "m_browseFolderButton");
			this.m_browseFolderButton.Name = "m_browseFolderButton";
			this.m_browseFolderButton.UseVisualStyleBackColor = true;
			this.m_browseFolderButton.Click += new System.EventHandler(this.OnClickFolderBrowserButton);
			// 
			// m_subMapsCheckBox
			// 
			resources.ApplyResources(this.m_subMapsCheckBox, "m_subMapsCheckBox");
			this.m_subMapsCheckBox.Name = "m_subMapsCheckBox";
			this.m_subMapsCheckBox.UseVisualStyleBackColor = true;
			// 
			// m_datDirTextBox
			// 
			this.m_datDirTextBox.AllowDrop = true;
			resources.ApplyResources(this.m_datDirTextBox, "m_datDirTextBox");
			this.m_datDirTextBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.FileSystem;
			this.m_datDirTextBox.Name = "m_datDirTextBox";
			this.m_datDirTextBox.DragDrop += new System.Windows.Forms.DragEventHandler(this.m_datDirTextBox_DragDrop);
			this.m_datDirTextBox.DragOver += new System.Windows.Forms.DragEventHandler(this.m_datDirTextBox_DragOver);
			this.m_datDirTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.m_datDirTextBox_Validating);
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			// 
			// PanelDatFilesJob
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.gbCycle);
			this.Controls.Add(this.gbDatFiles);
			this.Name = "PanelDatFilesJob";
			this.gbCycle.ResumeLayout(false);
			this.gbCycle.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.m_retryUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_failTimeUpDown)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.m_scanTimeUpDown)).EndInit();
			this.gbDatFiles.ResumeLayout(false);
			this.gbDatFiles.PerformLayout();
			this.ResumeLayout(false);

        }

        #endregion

        private iba.Utility.CollapsibleGroupBox gbCycle;
        public System.Windows.Forms.Button m_undoChangesBtn;
        private System.Windows.Forms.CheckBox m_cbDetectNewFiles;
        private System.Windows.Forms.CheckBox m_cbInitialScanEnabled;
        private System.Windows.Forms.CheckBox m_cbRetry;
        private System.Windows.Forms.NumericUpDown m_retryUpDown;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox m_cbRescanEnabled;
        private System.Windows.Forms.Button m_refreshDats;
        public System.Windows.Forms.Button m_stopButton;
        public System.Windows.Forms.Button m_applyToRunningBtn;
        public System.Windows.Forms.Button m_startButton;
        public System.Windows.Forms.CheckBox m_autoStartCheckBox;
        public System.Windows.Forms.CheckBox m_enableCheckBox;
        private System.Windows.Forms.NumericUpDown m_failTimeUpDown;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown m_scanTimeUpDown;
        private iba.Utility.CollapsibleGroupBox gbDatFiles;
        private System.Windows.Forms.TextBox m_tbPass;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox m_tbUserName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button m_checkPathButton;
        private System.Windows.Forms.Button m_browseFolderButton;
        private System.Windows.Forms.CheckBox m_subMapsCheckBox;
        private System.Windows.Forms.TextBox m_datDirTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ToolTip m_toolTip;
        private System.Windows.Forms.FolderBrowserDialog m_folderBrowserDialog1;
        private System.Windows.Forms.OpenFileDialog m_selectDatFilesDialog;
        private System.Windows.Forms.CheckBox m_cbRepErr;
        internal System.Windows.Forms.Button m_browseDatFilesButton;
        private System.Windows.Forms.TextBox tbFilePassword;
        private System.Windows.Forms.Label lblFilePassword;
    }
}
