using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Data;
using System.IO;
using System.Text;
using System.Windows.Forms;

using iba;
using iba.Data;
using iba.Utility;
using iba.Processing;

namespace iba.Controls
{
    public partial class ConfigurationControl : UserControl, IPropertyPane
    {
        public ConfigurationControl()
        {
            InitializeComponent();
            m_newBatchfileButton.Image = Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle);
            m_newReportButton.Image = Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle);
            m_newExtractButton.Image = Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle);
            m_newCopyTaskButton.Image = Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle); 
            m_toolTip.SetToolTip(m_newExtractButton, iba.Properties.Resources.extractButton);
            m_toolTip.SetToolTip(m_newReportButton, iba.Properties.Resources.reportButton);
            m_toolTip.SetToolTip(m_newBatchfileButton, iba.Properties.Resources.batchfileButton);
            m_toolTip.SetToolTip(m_newCopyTaskButton, iba.Properties.Resources.copytaskButton);
            m_toolTip.SetToolTip(m_startButton, iba.Properties.Resources.startButton);
            m_toolTip.SetToolTip(m_stopButton, iba.Properties.Resources.stopButton);
            m_toolTip.SetToolTip(m_refreshDats, iba.Properties.Resources.refreshDatButton);
            m_toolTip.SetToolTip(m_autoStartCheckBox, iba.Properties.Resources.toolTipAutoStart);
            m_toolTip.SetToolTip(m_applyToRunningButton, iba.Properties.Resources.applyStartedButton);
            ((Bitmap)m_refreshDats.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_applyToRunningButton.Image).MakeTransparent(Color.Magenta);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_datDirTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_analyserTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        ConfigurationData m_data;
       
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as ConfigurationData;
            m_nameTextBox.Text = m_data.Name;
            m_datDirTextBox.Text = m_data.DatDirectory;
            m_subMapsCheckBox.Checked = m_data.SubDirs;
            m_autoStartCheckBox.Checked = m_data.AutoStart;
            m_enableCheckBox.Checked = m_data.Enabled;
            m_analyserTextBox.Text = m_data.IbaAnalyserExe;

            if (m_failTimeUpDown.Minimum >  (decimal) m_data.ReprocessErrorsTimeInterval.TotalMinutes)
                m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double) m_failTimeUpDown.Minimum);
            else if (m_failTimeUpDown.Maximum < (decimal) m_data.ReprocessErrorsTimeInterval.TotalMinutes)
                m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double) m_failTimeUpDown.Maximum);
            
            if (m_scanTimeUpDown.Minimum > (decimal)m_data.RescanTimeInterval.TotalMinutes)
                m_data.RescanTimeInterval = TimeSpan.FromMinutes((double)m_scanTimeUpDown.Minimum);
            else if (m_scanTimeUpDown.Maximum < (decimal)m_data.RescanTimeInterval.TotalMinutes)
                m_data.RescanTimeInterval = TimeSpan.FromMinutes((double)m_scanTimeUpDown.Maximum);
                        
            m_scanTimeUpDown.Value = (decimal) m_data.RescanTimeInterval.TotalMinutes;
            m_failTimeUpDown.Value = (decimal) m_data.ReprocessErrorsTimeInterval.TotalMinutes;

            m_scanTimeUpDown.Enabled = m_cbRescanEnabled.Checked = m_data.RescanEnabled;

            m_executeIBAAButton.Enabled = File.Exists(m_analyserTextBox.Text);

            if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                m_applyToRunningButton.Enabled = false;
                m_startButton.Enabled = false;
                m_stopButton.Enabled = false;
                m_refreshDats.Enabled = true;
            }
            else if (TaskManager.Manager.GetStatus(m_data.ID).Started)
            {
                m_applyToRunningButton.Enabled = true;
                m_startButton.Enabled = false;
                m_newBatchfileButton.Enabled = false;
                m_newExtractButton.Enabled = false;
                m_newReportButton.Enabled = false;
                m_newCopyTaskButton.Enabled = false;
                m_stopButton.Enabled = true;
                m_refreshDats.Enabled = false;
            }
            else
            {
                m_applyToRunningButton.Enabled = false;
                m_startButton.Enabled = true;
                m_newBatchfileButton.Enabled = true;
                m_newExtractButton.Enabled = true;
                m_newReportButton.Enabled = true;
                m_newCopyTaskButton.Enabled = true;
                m_stopButton.Enabled = false;
                m_refreshDats.Enabled = true;
            }

            m_tbEmail.Text = m_data.NotificationData.Email;
            m_tbSMTP.Text = m_data.NotificationData.SMTPServer;
            m_tbNetSend.Text = m_data.NotificationData.Host;
            if (m_data.NotificationData.NotifyOutput == NotificationData.NotifyOutputChoice.NETSEND)
            {
                m_rbNetSend.Checked = true;
                m_rbEmail.Checked = false;
            }
            else
            {
                m_rbNetSend.Checked = false;
                m_rbEmail.Checked = true;
            }
            if (m_nudNotifyTime.Minimum > (decimal)m_data.NotificationData.TimeInterval.TotalMinutes)
                m_data.NotificationData.TimeInterval = TimeSpan.FromMinutes((double)m_nudNotifyTime.Minimum);
            else if (m_nudNotifyTime.Maximum < (decimal)m_data.NotificationData.TimeInterval.TotalMinutes)
                m_data.NotificationData.TimeInterval = TimeSpan.FromMinutes((double)m_nudNotifyTime.Maximum);
            m_nudNotifyTime.Value = (decimal)m_data.NotificationData.TimeInterval.TotalMinutes;
            if (m_data.NotificationData.NotifyImmediately)
            {
                m_rbImmediate.Checked = true;
                m_rbTime.Checked = false;
            }
            else
            {
                m_rbImmediate.Checked = false;
                m_rbTime.Checked = true;
            }
        }

        public void SaveData()
        {
            m_data.Name = m_nameTextBox.Text;
            m_data.DatDirectory = m_datDirTextBox.Text;
            m_data.SubDirs = m_subMapsCheckBox.Checked;
            m_data.Enabled = m_enableCheckBox.Checked;
            m_data.AutoStart = m_autoStartCheckBox.Checked;
            m_data.IbaAnalyserExe = m_analyserTextBox.Text;
            m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double) m_failTimeUpDown.Value);
            m_data.RescanTimeInterval = TimeSpan.FromMinutes((double)m_scanTimeUpDown.Value);
            m_data.RescanEnabled = m_cbRescanEnabled.Checked;
            m_data.NotificationData.Email = m_tbEmail.Text;
            m_data.NotificationData.SMTPServer = m_tbSMTP.Text;
            m_data.NotificationData.Host = m_tbNetSend.Text;
            m_data.NotificationData.NotifyOutput = m_rbNetSend.Checked ? NotificationData.NotifyOutputChoice.NETSEND : NotificationData.NotifyOutputChoice.EMAIL;
            m_data.NotificationData.NotifyImmediately = m_rbImmediate.Checked;
            m_data.NotificationData.TimeInterval = TimeSpan.FromMinutes((double)m_nudNotifyTime.Value);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
        }
        #endregion

        private void m_nameTextBox_TextChanged(object sender, EventArgs e)
        {
            TreeNode node = m_manager.LeftTree.SelectedNode;
            if (node != null)
                node.Text = m_nameTextBox.Text;
            m_manager.AdjustRightPaneControlTitle();
        }

        private void OnClickFolderBrowserButton(object sender, EventArgs e)
        {
            m_folderBrowserDialog1.ShowNewFolderButton = false;
            DialogResult result = m_folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_datDirTextBox.Text = m_folderBrowserDialog1.SelectedPath;
        }

        private void OnClickExecuteButton(object sender, EventArgs e)
        {
            try
            {
                Process ibaProc = new Process();
                ibaProc.EnableRaisingEvents = false;
                ibaProc.StartInfo.FileName = m_analyserTextBox.Text;
                ibaProc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void m_analyserTextBox_TextChanged(object sender, EventArgs e)
        {
            m_executeIBAAButton.Enabled = File.Exists(m_analyserTextBox.Text);
        }

        private void m_newReportButton_Click(object sender, EventArgs e)
        {
            ReportData report = new ReportData(m_data);
            new SetNextName(report);
            m_data.Tasks.Add(report);
            TreeNode newNode = new TreeNode(report.Name, 1, 1);
            newNode.Tag = new ReportTreeItemData(m_manager, report);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
        }

        private void m_newExtractButton_Click(object sender, EventArgs e)
        {
            ExtractData extract = new ExtractData(m_data);
            new SetNextName(extract);
            m_data.Tasks.Add(extract);
            TreeNode newNode = new TreeNode(extract.Name, 2,2);
            newNode.Tag = new ExtractTreeItemData(m_manager, extract);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
        }

        private void m_newBatchfileButton_Click(object sender, EventArgs e)
        {
            BatchFileData batchfile = new BatchFileData(m_data);
            new SetNextName(batchfile);
            m_data.Tasks.Add(batchfile);
            TreeNode newNode = new TreeNode(batchfile.Name, 3, 3);
            newNode.Tag = new BatchFileTreeItemData(m_manager, batchfile);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
        }

        private void m_newCopyTaskButton_Click(object sender, EventArgs e)
        {
            CopyMoveTaskData copy = new CopyMoveTaskData(m_data);
            new SetNextName(copy);
            m_data.Tasks.Add(copy);
            TreeNode newNode = new TreeNode(copy.Name, 4, 4);
            newNode.Tag = new CopyTaskTreeItemData(m_manager, copy);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
        }

        private void m_enableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            MainForm.strikeOutNodeText(m_manager.LeftTree.SelectedNode, !m_enableCheckBox.Checked);

            foreach (TreeNode node in m_manager.getLeftTree("status").Nodes)
                if ((node.Tag as StatusTreeItemData).StatusData.CorrConfigurationData == m_data)
                    MainForm.strikeOutNodeText(node, !m_enableCheckBox.Checked);
            if (!m_enableCheckBox.Checked)
                TaskManager.Manager.StopConfiguration(m_data.ID);
        }

        private void m_browseExecutableButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog1.CheckFileExists = true;
            m_openFileDialog1.Filter = "executables (*.exe)|*.exe";
            DialogResult result = m_openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_analyserTextBox.Text = m_openFileDialog1.FileName;
        }

        private void m_startButton_Click(object sender, EventArgs e)
        {
            SaveData();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            TaskManager.Manager.StartConfiguration(m_data.ID);
            m_startButton.Enabled = false;
            m_newBatchfileButton.Enabled = false;
            m_newExtractButton.Enabled = false;
            m_newReportButton.Enabled = false;
            m_newCopyTaskButton.Enabled = false;
            m_applyToRunningButton.Enabled = true;
            m_stopButton.Enabled = true;
            m_refreshDats.Enabled = false;
            MainForm t = m_manager as MainForm;
            if (t != null)
            {
                t.UpdateButtons();
                t.SwitchToStatusPane();
                t.StatusBarLabel.Text = ""; //clear any errors on restart
            }
        }

        private void m_stopButton_Click(object sender, EventArgs e)
        {
            using (StopWaitDialog w = new StopWaitDialog(m_data))
            {
                w.ShowDialog(ParentForm);
            }
            m_startButton.Enabled = true;
            m_applyToRunningButton.Enabled = false;
            m_newBatchfileButton.Enabled = true;
            m_newExtractButton.Enabled = true;
            m_newReportButton.Enabled = true;
            m_newCopyTaskButton.Enabled = true;
            m_refreshDats.Enabled = true;
            m_stopButton.Enabled = false;
            MainForm t = m_manager as MainForm;
            if (t != null) t.UpdateButtons();
        }

        private void m_refreshDats_Click(object sender, EventArgs e)
        {
            using (RemoveMarkingsDialog dialog = new RemoveMarkingsDialog(m_data))
            {
                dialog.ShowDialog(ParentForm);
            }
        }

        private void m_rbOutputCheckedChanged(object sender, EventArgs e)
        {
            m_tbNetSend.Enabled = !m_rbEmail.Checked;
            m_tbEmail.Enabled = !m_rbNetSend.Checked;
            m_tbSMTP.Enabled = !m_rbNetSend.Checked;
        }

        private void m_rbImmediate_CheckedChanged(object sender, EventArgs e)
        {
            m_nudNotifyTime.Enabled = m_rbTime.Checked;
        }

        private void m_cbRescanEnabled_CheckedChanged(object sender, EventArgs e)
        {
            m_scanTimeUpDown.Enabled = m_cbRescanEnabled.Checked;
        }

        private void m_applyToRunningButton_Click(object sender, EventArgs e)
        {
            SaveData();
            TaskManager.Manager.UpdateConfiguration(m_data);
        }
    }
}
