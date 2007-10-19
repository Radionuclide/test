using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

using iba;
using iba.Data;
using iba.Utility;
using iba.Processing;

namespace iba.Controls
{
    public partial class ExtractControl : UserControl, IPropertyPane
    {
        public ExtractControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_targetFolderTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        ExtractData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as ExtractData;

            m_pdoFileTextBox.Text = m_data.AnalysisFile;
            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                File.Exists(m_data.ParentConfigurationData.IbaAnalyzerExe);
            m_targetFolderTextBox.Text = m_data.DestinationMap;
            m_rbFile.Checked = m_data.ExtractToFile;
            m_rbDbase.Checked = !m_data.ExtractToFile;
            m_panelFile.Enabled = m_rbFile.Checked;

            m_rbNONE.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.NONE;
            m_rbOriginal.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.SAME;
            m_rbHour.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.HOUR;
            m_rbMonth.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.MONTH;
            m_rbDay.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.DAY;
            m_rbWeek.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.WEEK;

            m_rbBinaryFile.Checked = m_data.FileType == ExtractData.ExtractFileType.BINARY;
            m_rbTextFile.Checked = m_data.FileType == ExtractData.ExtractFileType.TEXT;

            m_rbLimitDirectories.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories;
            m_rbQuota.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_nudDirs.Value = m_data.SubfoldersNumber;
            m_nudQuota.Value = m_data.Quota;

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = m_data.MonitorData.MemoryLimit;
            m_nudTime.Value = m_data.MonitorData.TimeLimit.Minutes;

            try
            {
                m_monitorGroup.Enabled = VersionCheck.CheckVersion(m_data.ParentConfigurationData.IbaAnalyzerExe, "5.8.1");
            }
            catch
            {
                m_monitorGroup.Enabled = false;
            }


            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            m_data.ExtractToFile = m_rbFile.Checked;
            m_data.FileType = m_rbBinaryFile.Checked?ExtractData.ExtractFileType.BINARY:ExtractData.ExtractFileType.TEXT;

            if (m_rbNONE.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.NONE;
            if (m_rbHour.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.HOUR;
            if (m_rbDay.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.DAY;
            if (m_rbWeek.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.WEEK;
            if (m_rbMonth.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.MONTH;
            if (m_rbOriginal.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.SAME;
            m_data.SubfoldersNumber = (uint) m_nudDirs.Value;
            m_data.Quota = (uint) m_nudQuota.Value;
            m_data.OutputLimitChoice = m_rbLimitDirectories.Checked?TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories:TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;

            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint) m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double) m_nudTime.Value);

            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        public void LeaveCleanup() { }

        #endregion

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_data.ParentConfigurationData.IbaAnalyzerExe;
                    ibaProc.StartInfo.Arguments = m_pdoFileTextBox.Text;
                    ibaProc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                File.Exists(m_data.ParentConfigurationData.IbaAnalyzerExe);
        }

        private void m_browseFileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog1.Filter = "ibaAnalyzer PDO files (*.pdo)|*.pdo";
            DialogResult result = m_openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_pdoFileTextBox.Text = m_openFileDialog1.FileName;
        }

        private void m_browseFolderButton_Click(object sender, EventArgs e)
        {
            m_folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult result = m_folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_targetFolderTextBox.Text = m_folderBrowserDialog1.SelectedPath;
        }

        private void m_rbDbase_CheckedChanged(object sender, EventArgs e)
        {
            m_panelFile.Enabled = m_rbFile.Checked;
        }

        private void m_checkPathButton_Click(object sender, EventArgs e)
        {
            SaveData();
            string errormessage = null;
            bool ok;
            using (WaitCursor wait = new WaitCursor())
            {
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    ok = TaskManager.Manager.TestPath(Shares.PathToUnc(m_targetFolderTextBox.Text,false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true);
                else
                    ok = SharesHandler.TestPath(Shares.PathToUnc(m_targetFolderTextBox.Text,false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true);
            }
            if (ok)
            {
                m_checkPathButton.Text = null;
                m_checkPathButton.Image = iba.Properties.Resources.thumup;
            }
            else
            {
                MessageBox.Show(errormessage, iba.Properties.Resources.invalidPath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_checkPathButton.Text = null;
                m_checkPathButton.Image = iba.Properties.Resources.thumbdown;
            }
            ((Bitmap)m_checkPathButton.Image).MakeTransparent(Color.Magenta);
        }

        private void m_extractDirInfoChanged(object sender, EventArgs e)
        {
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
        }

        private void m_rbLimitUsageChoiceChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked) 
            {
                if (rb == m_rbLimitDirectories)
                {
                    m_rbQuota.Checked = false;
                    m_nudQuota.Enabled = false;
                    m_nudDirs.Enabled = true;
                }
                else
                {
                    m_rbLimitDirectories.Checked = false;
                    m_nudQuota.Enabled = true;
                    m_nudDirs.Enabled = false;
                }
            }
        }
    }
}
