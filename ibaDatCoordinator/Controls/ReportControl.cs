using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using iba;
using iba.Data;
using iba.Utility;
using iba.Processing;
using Microsoft.Win32;

namespace iba.Controls
{
    public partial class ReportControl : UserControl, IPropertyPane
    {
        public ReportControl()
        {
            InitializeComponent();
            m_extensionComboBox.Items.AddRange(new object[]{
                "pdf",
                "htm",
                "html",
                "mht",
                "mhtml",
                "txt",
                "xls",
                "rtf",
                "tif",
                "tiff",
                "emf",
                "jpg",
                "jpeg",
                "bmp",
                "xml"
            });
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_targetFolderTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        ReportData m_data;
        
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as ReportData;
            m_pdoFileTextBox.Text = m_data.AnalysisFile;
            m_targetFolderTextBox.Text = m_data.DestinationMap;

            m_rbFile.Checked = m_data.Output == ReportData.OutputChoice.FILE;
            m_rbPrint.Checked = m_data.Output == ReportData.OutputChoice.PRINT;
            m_extensionComboBox.SelectedIndex = m_extensionComboBox.FindString(m_data.Extension);
            m_rbNONE.Checked = m_data.Subfolder == ReportData.SubfolderChoice.NONE;
            m_rbOriginal.Checked = m_data.Subfolder == ReportData.SubfolderChoice.SAME;
            m_rbHour.Checked = m_data.Subfolder == ReportData.SubfolderChoice.HOUR;
            m_rbMonth.Checked = m_data.Subfolder == ReportData.SubfolderChoice.MONTH;
            m_rbDay.Checked = m_data.Subfolder == ReportData.SubfolderChoice.DAY;
            m_rbWeek.Checked = m_data.Subfolder == ReportData.SubfolderChoice.WEEK;

            m_rbLimitDirectories.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories;
            m_rbQuota.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_nudDirs.Value = m_data.SubfoldersNumber;
            m_nudQuota.Value = m_data.Quota;

            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                object o = key.GetValue("");
                ibaAnalyzerExe = Path.GetFullPath(o.ToString());
            }
            catch
            {
                ibaAnalyzerExe = iba.Properties.Resources.noIbaAnalyser;
            }

            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                File.Exists(ibaAnalyzerExe);
            m_panelFile.Enabled = m_rbFile.Checked;

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = m_data.MonitorData.MemoryLimit;
            m_nudTime.Value = m_data.MonitorData.TimeLimit.Minutes;
            m_cbOverwrite.Checked = m_data.OverwriteFiles;
            m_cbTakeDatTime.Checked = m_data.UseDatModTimeForDirs;

            try
            {
                m_monitorGroup.Enabled = VersionCheck.CheckVersion(ibaAnalyzerExe, "5.8.1");
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

        private String ibaAnalyzerExe;

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            
            if (m_rbPrint.Checked) m_data.Output = ReportData.OutputChoice.PRINT;
            else if (m_rbFile.Checked) m_data.Output = ReportData.OutputChoice.FILE;
            m_data.Extension = (string) m_extensionComboBox.SelectedItem;            

            if (m_rbNONE.Checked) m_data.Subfolder = ReportData.SubfolderChoice.NONE;
            if (m_rbHour.Checked) m_data.Subfolder = ReportData.SubfolderChoice.HOUR;
            if (m_rbDay.Checked) m_data.Subfolder = ReportData.SubfolderChoice.DAY;
            if (m_rbWeek.Checked) m_data.Subfolder = ReportData.SubfolderChoice.WEEK;
            if (m_rbMonth.Checked) m_data.Subfolder = ReportData.SubfolderChoice.MONTH;
            if (m_rbOriginal.Checked) m_data.Subfolder = ReportData.SubfolderChoice.SAME;

            m_data.SubfoldersNumber = (uint)m_nudDirs.Value;
            m_data.Quota = (uint)m_nudQuota.Value;
            m_data.OutputLimitChoice = m_rbLimitDirectories.Checked ? TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories : TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_data.OverwriteFiles = m_cbOverwrite.Checked;
            m_data.UseDatModTimeForDirs = m_cbTakeDatTime.Checked;

            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();

            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        public void LeaveCleanup() { }

        #endregion

        private void m_browseFileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog1.Filter = "ibaAnalyzer PDO files (*.pdo)|*.pdo";
            DialogResult result = m_openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_pdoFileTextBox.Text = m_openFileDialog1.FileName;
        }

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = ibaAnalyzerExe;
                    ibaProc.StartInfo.Arguments = "\"" + m_pdoFileTextBox.Text + "\"";
                    ibaProc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_browseFolderButton_Click(object sender, EventArgs e)
        {
            m_folderBrowserDialog1.ShowNewFolderButton = true;
            m_folderBrowserDialog1.SelectedPath = m_targetFolderTextBox.Text;            
            DialogResult result = m_folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_targetFolderTextBox.Text = m_folderBrowserDialog1.SelectedPath;
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                File.Exists(ibaAnalyzerExe);
        }

      
        private void m_rbPrint_CheckedChanged(object sender, EventArgs e)
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

        private void m_reportDirInfoChanged(object sender, EventArgs e)
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
