namespace iba.Controls
{
    partial class LogControl
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogControl));
            this.m_dataGridView = new System.Windows.Forms.DataGridView();
            this.time = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ConfigurationData = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Task = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.datFile = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.message = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // m_dataGridView
            // 
            this.m_dataGridView.AccessibleDescription = null;
            this.m_dataGridView.AccessibleName = null;
            this.m_dataGridView.AllowUserToAddRows = false;
            this.m_dataGridView.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.m_dataGridView, "m_dataGridView");
            this.m_dataGridView.BackgroundImage = null;
            this.m_dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.m_dataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.time,
            this.ConfigurationData,
            this.Task,
            this.datFile,
            this.message});
            this.m_dataGridView.Font = null;
            this.m_dataGridView.MultiSelect = false;
            this.m_dataGridView.Name = "m_dataGridView";
            this.m_dataGridView.ReadOnly = true;
            this.m_dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            // 
            // time
            // 
            this.time.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.time, "time");
            this.time.Name = "time";
            this.time.ReadOnly = true;
            // 
            // ConfigurationData
            // 
            this.ConfigurationData.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.ConfigurationData, "ConfigurationData");
            this.ConfigurationData.Name = "ConfigurationData";
            this.ConfigurationData.ReadOnly = true;
            // 
            // Task
            // 
            this.Task.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.Task, "Task");
            this.Task.Name = "Task";
            this.Task.ReadOnly = true;
            // 
            // datFile
            // 
            this.datFile.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            resources.ApplyResources(this.datFile, "datFile");
            this.datFile.Name = "datFile";
            this.datFile.ReadOnly = true;
            // 
            // message
            // 
            this.message.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.message.FillWeight = 200F;
            resources.ApplyResources(this.message, "message");
            this.message.Name = "message";
            this.message.ReadOnly = true;
            // 
            // LogControl
            // 
            this.AccessibleDescription = null;
            this.AccessibleName = null;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = null;
            this.Controls.Add(this.m_dataGridView);
            this.Font = null;
            this.Name = "LogControl";
            ((System.ComponentModel.ISupportInitialize)(this.m_dataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView m_dataGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn time;
        private System.Windows.Forms.DataGridViewTextBoxColumn ConfigurationData;
        private System.Windows.Forms.DataGridViewTextBoxColumn Task;
        private System.Windows.Forms.DataGridViewTextBoxColumn datFile;
        private System.Windows.Forms.DataGridViewTextBoxColumn message;
    }
}
