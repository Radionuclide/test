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
using iba.Plugins;

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
            m_newIfTaskButton.Image = Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle); 

            m_newReportButton.ToolTipText = iba.Properties.Resources.reportButton;
            m_newExtractButton.ToolTipText = iba.Properties.Resources.extractButton;
            m_newBatchfileButton.ToolTipText = iba.Properties.Resources.batchfileButton;
            m_newCopyTaskButton.ToolTipText = iba.Properties.Resources.copytaskButton;
            m_newIfTaskButton.ToolTipText = iba.Properties.Resources.iftaskButton;

            m_toolTip.SetToolTip(m_startButton, iba.Properties.Resources.startButton);
            m_toolTip.SetToolTip(m_stopButton, iba.Properties.Resources.stopButton);
            m_toolTip.SetToolTip(m_refreshDats, iba.Properties.Resources.refreshDatButton);
            m_toolTip.SetToolTip(m_autoStartCheckBox, iba.Properties.Resources.toolTipAutoStart);
            m_toolTip.SetToolTip(m_applyToRunningButton, iba.Properties.Resources.applyStartedButton);
            m_toolTip.SetToolTip(m_checkPathButton, iba.Properties.Resources.checkPathButton);
            ((Bitmap)m_refreshDats.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_applyToRunningButton.Image).MakeTransparent(Color.Magenta);

            foreach (PluginTaskInfo info in PluginManager.Manager.PluginInfos)
            {
                ToolStripButton bt = new ToolStripButton();
                bt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                bt.AutoSize = false;
                bt.Height = bt.Width = 40;
                bt.Image = Bitmap.FromHicon(info.Icon.Handle);
                bt.ImageScaling = ToolStripItemImageScaling.None;
                bt.ToolTipText = info.Description;
                bt.Tag = info;
                bt.Click += new EventHandler(newCustomTaskButton_Click);
                m_newTaskToolstrip.Items.Add(bt);
            }
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
            m_analyserTextBox.Text = m_data.IbaAnalyzerExe;

            if (m_failTimeUpDown.Minimum >  (decimal) m_data.ReprocessErrorsTimeInterval.TotalMinutes)
                m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double) m_failTimeUpDown.Minimum);
            else if (m_failTimeUpDown.Maximum < (decimal) m_data.ReprocessErrorsTimeInterval.TotalMinutes)
                m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double) m_failTimeUpDown.Maximum);
            
            if (m_scanTimeUpDown.Minimum > (decimal)m_data.RescanTimeInterval.TotalMinutes)
                m_data.RescanTimeInterval = TimeSpan.FromMinutes((double)m_scanTimeUpDown.Minimum);
            else if (m_scanTimeUpDown.Maximum < (decimal)m_data.RescanTimeInterval.TotalMinutes)
                m_data.RescanTimeInterval = TimeSpan.FromMinutes((double)m_scanTimeUpDown.Maximum);
                        
            m_scanTimeUpDown.Value = (decimal) m_data.RescanTimeInterval.TotalMinutes;
            m_scanTimeUpDown.Enabled = m_cbRescanEnabled.Checked = m_data.RescanEnabled;
            m_failTimeUpDown.Value = (decimal)m_data.ReprocessErrorsTimeInterval.TotalMinutes;
            m_retryUpDown.Value = (decimal) m_data.NrTryTimes;
            m_retryUpDown.Enabled = m_cbRetry.Checked = m_data.LimitTimesTried;

            m_executeIBAAButton.Enabled = File.Exists(m_analyserTextBox.Text);

            if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                m_applyToRunningButton.Enabled = false;
                m_startButton.Enabled = false;
                m_stopButton.Enabled = false;
                m_refreshDats.Enabled = true;
            }
            else if (TaskManager.Manager.GetStatus(m_data.Guid).Started)
            {
                m_applyToRunningButton.Enabled = true;
                m_startButton.Enabled = false;
                m_newTaskToolstrip.Enabled = false;
                m_stopButton.Enabled = true;
                m_refreshDats.Enabled = false;
            }
            else
            {
                m_applyToRunningButton.Enabled = false;
                m_startButton.Enabled = m_data.Enabled;
                m_newTaskToolstrip.Enabled = true;
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
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;
            try
            {
                string version = FileVersionInfo.GetVersionInfo(m_data.IbaAnalyzerExe).FileVersion;
                m_newIfTaskButton.Enabled = (version.CompareTo("5.3.4") >= 0);
            }
            catch
            {
                m_newIfTaskButton.Enabled = false;
            }
        }

        public void SaveData()
        {
            m_data.Name = m_nameTextBox.Text;
            m_data.DatDirectory = m_datDirTextBox.Text;
            m_data.SubDirs = m_subMapsCheckBox.Checked;
            m_data.Enabled = m_enableCheckBox.Checked;
            m_data.AutoStart = m_autoStartCheckBox.Checked;
            m_data.IbaAnalyzerExe = m_analyserTextBox.Text;
            m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double) m_failTimeUpDown.Value);
            m_data.RescanTimeInterval = TimeSpan.FromMinutes((double)m_scanTimeUpDown.Value);
            m_data.RescanEnabled = m_cbRescanEnabled.Checked;
            m_data.NrTryTimes = (int) m_retryUpDown.Value;
            m_data.LimitTimesTried = m_cbRetry.Checked;

            m_data.NotificationData.Email = m_tbEmail.Text;
            m_data.NotificationData.SMTPServer = m_tbSMTP.Text;
            m_data.NotificationData.Host = m_tbNetSend.Text;
            m_data.NotificationData.NotifyOutput = m_rbNetSend.Checked ? NotificationData.NotifyOutputChoice.NETSEND : NotificationData.NotifyOutputChoice.EMAIL;
            m_data.NotificationData.NotifyImmediately = m_rbImmediate.Checked;
            m_data.NotificationData.TimeInterval = TimeSpan.FromMinutes((double)m_nudNotifyTime.Value);
            
            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);

        }

        public void LeaveCleanup() {}

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
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            TreeNode newNode = new TreeNode(report.Name, MainForm.REPORTTASK_INDEX, MainForm.REPORTTASK_INDEX);
            newNode.Tag = new ReportTreeItemData(m_manager, report);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            m_manager.LeftTree.SelectedNode = newNode;
            (m_manager as MainForm).m_rightPane_Enter(null, null);
        }

        private void m_newExtractButton_Click(object sender, EventArgs e)
        {
            ExtractData extract = new ExtractData(m_data);
            new SetNextName(extract);
            m_data.Tasks.Add(extract);
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            TreeNode newNode = new TreeNode(extract.Name, MainForm.EXTRACTTASK_INDEX, MainForm.EXTRACTTASK_INDEX);
            newNode.Tag = new ExtractTreeItemData(m_manager, extract);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            m_manager.LeftTree.SelectedNode = newNode;
            (m_manager as MainForm).m_rightPane_Enter(null, null);
        }

        private void m_newBatchfileButton_Click(object sender, EventArgs e)
        {
            BatchFileData batchfile = new BatchFileData(m_data);
            new SetNextName(batchfile);
            m_data.Tasks.Add(batchfile);
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            TreeNode newNode = new TreeNode(batchfile.Name, MainForm.BATCHFILETASK_INDEX, MainForm.BATCHFILETASK_INDEX);
            newNode.Tag = new BatchFileTreeItemData(m_manager, batchfile);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            m_manager.LeftTree.SelectedNode = newNode;
            (m_manager as MainForm).m_rightPane_Enter(null, null);
        }

        private void m_newCopyTaskButton_Click(object sender, EventArgs e)
        {
            CopyMoveTaskData copy = new CopyMoveTaskData(m_data);
            new SetNextName(copy);
            m_data.Tasks.Add(copy);
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            TreeNode newNode = new TreeNode(copy.Name, MainForm.COPYTASK_INDEX, MainForm.COPYTASK_INDEX);
            newNode.Tag = new CopyTaskTreeItemData(m_manager, copy);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            m_manager.LeftTree.SelectedNode = newNode;
            (m_manager as MainForm).m_rightPane_Enter(null, null);
        }

        private void m_newIfTaskButton_Click(object sender, EventArgs e)
        {
            IfTaskData condo = new IfTaskData(m_data);
            new SetNextName(condo);
            m_data.Tasks.Add(condo);
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            TreeNode newNode = new TreeNode(condo.Name, MainForm.IFTASK_INDEX, MainForm.IFTASK_INDEX);
            newNode.Tag = new IfTaskTreeItemData(m_manager, condo);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            m_manager.LeftTree.SelectedNode = newNode;
            (m_manager as MainForm).m_rightPane_Enter(null, null);
        }

        void newCustomTaskButton_Click(object sender, EventArgs e)
        {
            PluginTaskInfo info = (PluginTaskInfo)(((ToolStripButton) sender).Tag);
            CustomTaskData cust = new CustomTaskData(m_data, info);
            new SetNextName(cust);
            m_data.Tasks.Add(cust);
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            int index = PluginManager.Manager.PluginInfos.FindIndex(delegate(PluginTaskInfo i) { return i.Icon == info.Icon; });
            TreeNode newNode = new TreeNode(cust.Name, MainForm.CUSTOMTASK_INDEX + index, MainForm.CUSTOMTASK_INDEX + index);
            newNode.Tag = new CustomTaskTreeItemData(m_manager,cust);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode); 
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            m_manager.LeftTree.SelectedNode = newNode;
            (m_manager as MainForm).m_rightPane_Enter(null, null);
        }


        private void m_enableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_startButton.Enabled = m_enableCheckBox.Checked;
            MainForm.strikeOutNodeText(m_manager.LeftTree.SelectedNode, !m_enableCheckBox.Checked);

            foreach (TreeNode node in m_manager.getLeftTree("status").Nodes)
                if ((node.Tag as StatusTreeItemData).StatusData.CorrConfigurationData == m_data)
                    MainForm.strikeOutNodeText(node, !m_enableCheckBox.Checked);
            if (!m_enableCheckBox.Checked)
                TaskManager.Manager.StopConfiguration(m_data.Guid);
            MainForm t = m_manager as MainForm;
            if (t != null) t.UpdateButtons();
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
            TaskManager.Manager.StartConfiguration(m_data);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                Program.CommunicationObject.SaveConfigurations();
            m_startButton.Enabled = false;
            m_newTaskToolstrip.Enabled = false;
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
            m_newTaskToolstrip.Enabled = true;
            m_refreshDats.Enabled = true;
            m_stopButton.Enabled = false;
            MainForm t = m_manager as MainForm;
            if (t != null) t.UpdateButtons();
        }

        private void m_refreshDats_Click(object sender, EventArgs e)
        {
            SaveData();
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


        private void m_cbRetry_CheckedChanged(object sender, EventArgs e)
        {
            m_retryUpDown.Enabled = m_cbRetry.Checked;
        }

        private void m_applyToRunningButton_Click(object sender, EventArgs e)
        {
            SaveData();
            TaskManager.Manager.UpdateConfiguration(m_data);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                Program.CommunicationObject.SaveConfigurations();
        }

        private void m_checkPathButton_Click(object sender, EventArgs e)
        {
            SaveData();
            string errormessage = null;
            bool ok;
            using (WaitCursor wait = new WaitCursor())
            {
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    ok = TaskManager.Manager.TestPath(Shares.PathToUnc(m_datDirTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, false);
                else
                    ok = SharesHandler.TestPath(Shares.PathToUnc(m_datDirTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, false);
            }
            if (ok)
            {
                m_checkPathButton.Text = null;
                m_checkPathButton.Image = iba.Properties.Resources.thumup;
            }
            else
            {
                MessageBox.Show(errormessage,iba.Properties.Resources.invalidPath,MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_checkPathButton.Text = null;
                m_checkPathButton.Image = iba.Properties.Resources.thumbdown;
            }
            ((Bitmap)m_checkPathButton.Image).MakeTransparent(Color.Magenta);
        }

        private void m_datDirInfoChanged(object sender, EventArgs e)
        {
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
        }

    }
}
