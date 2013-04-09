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
using Microsoft.Win32;
using iba.Dialogs;

namespace iba.Controls
{
    public partial class ConfigurationControl : UserControl, IPropertyPane
    {
        public ConfigurationControl(bool oneTimeJob)
        {
            m_oneTimeJob = oneTimeJob;
            InitializeComponent();
            if (oneTimeJob) //make this a onetime job dialog
            {
                this.SuspendLayout();
                m_startButton.Location = new Point(m_undoChangesBtn.Location.X, 18);
                m_stopButton.Location = new Point(m_refreshDats.Location.X, 18);
                //m_stopButton.Anchor = m_startButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                m_browseDatFilesButton.Image = Bitmap.FromHicon(iba.Properties.Resources.standalone.Handle);
                foreach (Control c in groupBox3.Controls)
                {
                    if (c != m_startButton && c != m_stopButton && c != m_enableCheckBox)
                    {
                        c.Enabled = false;
                        c.Visible = false;
                    }
                }
                int groupbox3OldHeight = groupBox3.Size.Height;
                int groupbox3NewHeight = m_stopButton.Size.Height + 16 + 8;
                int diff = groupbox3OldHeight - groupbox3NewHeight;
                groupBox3.Size = new Size(groupBox3.Size.Width, groupbox3NewHeight);

                m_datDirTextBox.Multiline = true;
                foreach (Control c in groupBox1.Controls)
                {
                    if (c == label2)
                        c.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    else if (c == m_datDirTextBox)
                        c.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                    else if (c == m_browseFolderButton || c == m_browseDatFilesButton)
                        c.Anchor = AnchorStyles.Right | AnchorStyles.Top; //actually this is unchanged
                    else
                        c.Anchor = (c.Anchor & ~AnchorStyles.Top) | AnchorStyles.Bottom;
                }

                groupBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
                groupBox1.Location = new Point(groupBox1.Location.X, groupBox1.Location.Y - diff);
                groupBox1.Size = new Size(groupBox1.Size.Width, groupBox1.Size.Height + diff);

                groupBox4.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                groupBox5.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                groupBox6.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;

                m_toolTip.SetToolTip(m_datDirTextBox, iba.Properties.Resources.DatDirDragAndDrop);

                this.ResumeLayout();
            }
            else
            {
                m_browseDatFilesButton.Visible = false;
                int diff = m_refreshDats.Location.X - m_undoChangesBtn.Location.X;
                m_browseFolderButton.Location = new Point(m_browseFolderButton.Location.X + diff, m_browseFolderButton.Location.Y);
                m_datDirTextBox.Size = new Size(m_datDirTextBox.Size.Width+diff, m_datDirTextBox.Size.Height);
            }

            m_newBatchfileButton.Image = Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle);
            m_newReportButton.Image = Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle);
            m_newExtractButton.Image = Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle);
            m_newCopyTaskButton.Image = Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle);
            m_newIfTaskButton.Image = Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle);
            m_newUpdateDataTaskButton.Image = iba.Properties.Resources.updatedatatask;
            m_newPauseTaskButton.Image = iba.Properties.Resources.pausetask;

            m_newReportButton.ToolTipText = iba.Properties.Resources.reportButton;
            m_newExtractButton.ToolTipText = iba.Properties.Resources.extractButton;
            m_newBatchfileButton.ToolTipText = iba.Properties.Resources.batchfileButton;
            m_newCopyTaskButton.ToolTipText = iba.Properties.Resources.copytaskButton;
            m_newIfTaskButton.ToolTipText = iba.Properties.Resources.iftaskButton;
            m_newUpdateDataTaskButton.ToolTipText = iba.Properties.Resources.updatedatataskButton;
            m_newPauseTaskButton.ToolTipText = iba.Properties.Resources.pausetaskButton;

            m_toolTip.SetToolTip(m_startButton, iba.Properties.Resources.startButton);
            m_toolTip.SetToolTip(m_stopButton, iba.Properties.Resources.stopButton);
            m_toolTip.SetToolTip(m_refreshDats, iba.Properties.Resources.refreshDatButton);
            m_toolTip.SetToolTip(m_autoStartCheckBox, iba.Properties.Resources.toolTipAutoStart);
            m_toolTip.SetToolTip(m_applyToRunningBtn, iba.Properties.Resources.applyStartedButton);
            m_toolTip.SetToolTip(m_undoChangesBtn, iba.Properties.Resources.undoChangesButton);
            m_toolTip.SetToolTip(m_checkPathButton, iba.Properties.Resources.checkPathButton);
            ((Bitmap)m_refreshDats.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_applyToRunningBtn.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_testNotification.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_undoChangesBtn.Image).MakeTransparent(Color.Magenta);
            m_toolTip.SetToolTip(m_testNotification,iba.Properties.Resources.testNotifications);

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

        private bool m_oneTimeJob;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!m_oneTimeJob)
                WindowsAPI.SHAutoComplete(m_datDirTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
                    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        ConfigurationData m_data;
        string ibaAnalyzerExe;
       
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as ConfigurationData;
            m_nameTextBox.Text = m_data.Name;
            m_datDirTextBox.Text = m_data.DatDirectory;
            m_subMapsCheckBox.Checked = m_data.SubDirs;
            m_autoStartCheckBox.Checked = m_data.AutoStart;
            m_enableCheckBox.Checked = m_data.Enabled;

            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                 object o = key.GetValue("");
                ibaAnalyzerExe = Path.GetFullPath(o.ToString());
                m_lblIbaAnalyzerPath.Text = ibaAnalyzerExe;
            }
            catch
            {
                m_lblIbaAnalyzerPath.Text = ibaAnalyzerExe =iba.Properties.Resources.noIbaAnalyser;
            }

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
            m_cbInitialScanEnabled.Checked = m_data.InitialScanEnabled;
            m_failTimeUpDown.Value = (decimal)m_data.ReprocessErrorsTimeInterval.TotalMinutes;
            m_retryUpDown.Value = (decimal) m_data.NrTryTimes;
            m_retryUpDown.Enabled = m_cbRetry.Checked = m_data.LimitTimesTried;

            m_executeIBAAButton.Enabled = File.Exists(m_lblIbaAnalyzerPath.Text);

            if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                m_applyToRunningBtn.Enabled = false;
                m_startButton.Enabled = false;
                m_stopButton.Enabled = false;
                m_refreshDats.Enabled = true;
            }
            else if (TaskManager.Manager.IsJobStarted(m_data.Guid))
            {
                m_applyToRunningBtn.Enabled = true;
                m_startButton.Enabled = false;
                m_newTaskToolstrip.Enabled = false;
                m_stopButton.Enabled = true;
                m_refreshDats.Enabled = false;
            }
            else
            {
                m_applyToRunningBtn.Enabled = false;
                m_startButton.Enabled = m_data.Enabled;
                m_newTaskToolstrip.Enabled = true;
                m_stopButton.Enabled = false;
                m_refreshDats.Enabled = true;
            }

            m_tbEmail.Text = m_data.NotificationData.Email;
            m_tbSMTP.Text = m_data.NotificationData.SMTPServer;
            m_tbNetSend.Text = m_data.NotificationData.Host;
            m_tbMailPass.Text = m_data.NotificationData.Password;
            m_tbMailUsername.Text = m_data.NotificationData.Username;
            m_cbAuthentication.Checked = m_data.NotificationData.AuthenticationRequired;
            m_tbSender.Text = m_data.NotificationData.Sender;

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
                m_newIfTaskButton.Enabled = VersionCheck.CheckVersion(ibaAnalyzerExe,"5.3.4");
            }
            catch
            {
                m_newIfTaskButton.Enabled = false;
            }

            m_nudRestartIbaAnalyzer.Value = (decimal)m_data.TimesAfterWhichtToRestartIbaAnalyzer;
            m_cbRestartIbaAnalyzer.Checked = m_nudRestartIbaAnalyzer.Enabled = m_data.BRestartIbaAnalyzer;
            m_cbCloseIbaAnalyzer.Checked = m_data.IbaAnalyzerSleepsWhenNoDatFiles;
            m_cbDetectNewFiles.Checked = m_data.DetectNewFiles;
        }

        public void SaveData()
        {
            m_data.Name = m_nameTextBox.Text;
            m_data.DatDirectory = m_datDirTextBox.Text;
            m_data.SubDirs = m_subMapsCheckBox.Checked;
            m_data.Enabled = m_enableCheckBox.Checked;
            m_data.AutoStart = m_autoStartCheckBox.Checked;
            m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double) m_failTimeUpDown.Value);
            m_data.RescanTimeInterval = TimeSpan.FromMinutes((double)m_scanTimeUpDown.Value);
            m_data.RescanEnabled = m_cbRescanEnabled.Checked;
            m_data.InitialScanEnabled = m_cbInitialScanEnabled.Checked;
            m_data.NrTryTimes = (int) m_retryUpDown.Value;
            m_data.LimitTimesTried = m_cbRetry.Checked;

            m_data.NotificationData.Email = m_tbEmail.Text;
            m_data.NotificationData.SMTPServer = m_tbSMTP.Text;
            m_data.NotificationData.Host = m_tbNetSend.Text;
            m_data.NotificationData.NotifyOutput = m_rbNetSend.Checked ? NotificationData.NotifyOutputChoice.NETSEND : NotificationData.NotifyOutputChoice.EMAIL;
            m_data.NotificationData.NotifyImmediately = m_rbImmediate.Checked;
            m_data.NotificationData.TimeInterval = TimeSpan.FromMinutes((double)m_nudNotifyTime.Value);
            m_data.NotificationData.AuthenticationRequired = m_cbAuthentication.Checked;
            m_data.NotificationData.Username = m_tbMailUsername.Text;
            m_data.NotificationData.Password = m_tbMailPass.Text;
            m_data.NotificationData.Sender = m_tbSender.Text;

            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();
            m_data.BRestartIbaAnalyzer = m_cbRestartIbaAnalyzer.Checked;
            m_data.TimesAfterWhichtToRestartIbaAnalyzer = (int) m_nudRestartIbaAnalyzer.Value;
            m_data.IbaAnalyzerSleepsWhenNoDatFiles = m_cbCloseIbaAnalyzer.Checked;

            m_data.DetectNewFiles = m_cbDetectNewFiles.Checked;

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
            if (!m_oneTimeJob)
            {
                m_folderBrowserDialog1.SelectedPath = m_datDirTextBox.Text;
                DialogResult result = m_folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                    m_datDirTextBox.Text = m_folderBrowserDialog1.SelectedPath;
            }
            else
            {
                string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                //         ShowEditBox = true,
                //         //NewStyle = false,
                if ((lines.Length>0) && (System.IO.File.Exists(lines[lines.Length-1])||System.IO.Directory.Exists(lines[lines.Length-1])))
                    m_folderBrowserDialog1.SelectedPath = lines[lines.Length - 1];
                DialogResult result = m_folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string line in lines)
                        sb.AppendLine(line);
                    sb.AppendLine(m_folderBrowserDialog1.SelectedPath);
                    m_datDirTextBox.Text = sb.ToString();
                }
            }
        }

        private void OnClickExecuteButton(object sender, EventArgs e)
        {
            try
            {
                Process ibaProc = new Process();
                ibaProc.EnableRaisingEvents = false;
                ibaProc.StartInfo.FileName = m_lblIbaAnalyzerPath.Text;
                ibaProc.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        }

        private void m_newUpdateDataTaskButton_Click(object sender, EventArgs e)
        {
            bool IsLicensed = false;
            try
            {
                CDongleInfo info = CDongleInfo.ReadDongle();
                if (info.IsPluginLicensed(2))
                    IsLicensed = true;
            }
            catch 
            {
            }
            if (!IsLicensed)
            {
                MessageBox.Show(this, iba.Properties.Resources.logTaskNotLicensed,
                        iba.Properties.Resources.updateDataTaskTitle, MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button2);
                return;
            }
            UpdateDataTaskData udt = new UpdateDataTaskData(m_data);
            new SetNextName(udt);
            m_data.Tasks.Add(udt);
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            TreeNode newNode = new TreeNode(udt.Name, MainForm.UPDATEDATATASK_INDEX, MainForm.UPDATEDATATASK_INDEX);
            newNode.Tag = new UpdateDataTaskTreeItemData(m_manager, udt);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            m_manager.LeftTree.SelectedNode = newNode;
        }

        private void m_newPauseTaskButton_Click(object sender, EventArgs e)
        {
            PauseTaskData pause = new PauseTaskData(m_data);
            new SetNextName(pause);
            m_data.Tasks.Add(pause);
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            TreeNode newNode = new TreeNode(pause.Name, MainForm.PAUSETASK_INDEX, MainForm.PAUSETASK_INDEX);
            newNode.Tag = new PauseTaskTreeItemData(m_manager, pause);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            m_manager.LeftTree.SelectedNode = newNode;
        }

        void newCustomTaskButton_Click(object sender, EventArgs e)
        {
            PluginTaskInfo info = (PluginTaskInfo)(((ToolStripButton) sender).Tag);
            ICustomTaskData icust;
            string name;
            if (info is PluginTaskInfoUNC)
            {
                CustomTaskDataUNC cust = new CustomTaskDataUNC(m_data, info as PluginTaskInfo);
                m_data.Tasks.Add(cust);
                name = cust.Name;
                icust = cust;
            }
            else
            {
                CustomTaskData cust = new CustomTaskData(m_data, info as PluginTaskInfo);
                m_data.Tasks.Add(cust);
                name = cust.Name;
                icust = cust;
            }
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            int index = PluginManager.Manager.PluginInfos.FindIndex(delegate(PluginTaskInfo i) { return i.Icon == info.Icon; });
            TreeNode newNode = new TreeNode(name, MainForm.CUSTOMTASK_INDEX + index, MainForm.CUSTOMTASK_INDEX + index);
            newNode.Tag = new CustomTaskTreeItemData(m_manager, icust);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            m_manager.LeftTree.SelectedNode = newNode;

        }


        private void m_enableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_startButton.Enabled = m_enableCheckBox.Checked;
            MainForm.strikeOutNodeText(m_manager.LeftTree.SelectedNode, !m_enableCheckBox.Checked);

            foreach (TreeNode node in m_manager.getLeftTree("status").Nodes)
                if ((node.Tag as StatusTreeItemData).CorrConfigurationData.Guid == m_data.Guid)
                    MainForm.strikeOutNodeText(node, !m_enableCheckBox.Checked);
            if (!m_enableCheckBox.Checked)
                TaskManager.Manager.StopConfiguration(m_data.Guid);
            MainForm t = m_manager as MainForm;
            if (t != null) t.UpdateButtons();
        }

        private void m_startButton_Click(object sender, EventArgs e)
        {
            MainForm t = m_manager as MainForm;
            if (!Utility.Crypt.CheckPassword(t)) return;
            SaveData();
            TaskManager.Manager.StartConfiguration(m_data);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                Program.CommunicationObject.SaveConfigurations();
            m_startButton.Enabled = false;
            m_newTaskToolstrip.Enabled = false;
            m_applyToRunningBtn.Enabled = true;
            m_stopButton.Enabled = true;
            m_refreshDats.Enabled = false;
            if (t != null)
            {
                t.UpdateButtons();
                t.SwitchToStatusPane();
                t.StatusBarLabel.Text = ""; //clear any errors on restart
            }
        }

        private void m_stopButton_Click(object sender, EventArgs e)
        {
            MainForm t = m_manager as MainForm;
            if (!Utility.Crypt.CheckPassword(t)) return;
            using (StopWaitDialog w = new StopWaitDialog(m_data))
            {
                w.ShowDialog(ParentForm);
            }
            m_startButton.Enabled = true;
            m_applyToRunningBtn.Enabled = false;
            m_newTaskToolstrip.Enabled = true;
            m_refreshDats.Enabled = true;
            m_stopButton.Enabled = false;
            if (t != null) t.UpdateButtons();
        }

        private void m_refreshDats_Click(object sender, EventArgs e)
        {
            MainForm t = m_manager as MainForm;
            if (!Utility.Crypt.CheckPassword(t)) return;
            DialogResult res = MessageBox.Show(this, iba.Properties.Resources.refreshDatWarning,
            iba.Properties.Resources.refreshDatButton, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if (res != DialogResult.Yes)
                return;

            SaveData();
            using (RemoveMarkingsDialog dialog = new RemoveMarkingsDialog(m_data))
            {
                dialog.ShowDialog(ParentForm);
            }
        }

        private void m_rbOutputCheckedChanged(object sender, EventArgs e)
        {
            labelnetsendhost.Enabled = m_tbNetSend.Enabled = !m_rbEmail.Checked;
            labelmailrecipient.Enabled = m_tbEmail.Enabled = !m_rbNetSend.Checked;
            labelmailsmtp.Enabled = m_tbSMTP.Enabled = !m_rbNetSend.Checked;
            m_tbMailPass.Enabled = labelmailpass.Enabled
            = m_tbMailUsername.Enabled = labelmailuser.Enabled
            = m_cbAuthentication.Checked && m_rbEmail.Checked;
        }

        private void m_rbImmediate_CheckedChanged(object sender, EventArgs e)
        {
            m_nudNotifyTime.Enabled = m_rbTime.Checked;
        }

        private void m_cbRescanEnabled_CheckedChanged(object sender, EventArgs e)
        {
            m_scanTimeUpDown.Enabled = m_cbRescanEnabled.Checked;
            if (!m_cbRescanEnabled.Checked) m_cbDetectNewFiles.Checked = true;
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

        private void m_undoChangesBtn_Click(object sender, EventArgs e)
        {
            SaveData();
            ConfigurationData undonedata = TaskManager.Manager.GetConfigurationFromWorker(m_data.Guid);
            ConfigurationData donedata = m_data;
            if (undonedata == null) return;
            if (donedata.IsSame(undonedata))
            { //they're equal
                MessageBox.Show(iba.Properties.Resources.SameConfMessage, iba.Properties.Resources.SameConfCaption, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                undonedata = undonedata.Clone_AlsoCopyGuids(); //if we don't take copy, any changes will immediately reflect
                LoadData(undonedata,m_manager);
                (m_manager as MainForm).UpdateTreeNode(donedata, undonedata);
                TaskManager.Manager.ReplaceConfiguration(m_data);
            }
        }

        private void m_checkPathButton_Click(object sender, EventArgs e)
        {
            SaveData();
            string errormessage = null;
            bool ok = true;
            using (WaitCursor wait = new WaitCursor())
            {
                if (!m_data.OnetimeJob)
                {
                    if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                        ok = TaskManager.Manager.TestPath(Shares.PathToUnc(m_datDirTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, false);
                    else
                        ok = SharesHandler.TestPath(Shares.PathToUnc(m_datDirTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, false);
                }
                else
                {
                    string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string line in lines)
                    {
                        if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                            ok = TaskManager.Manager.TestPath(Shares.PathToUnc(line, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, false);
                        else
                            ok = SharesHandler.TestPath(Shares.PathToUnc(line, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, false);
                        if (!ok)
                        {
                            errormessage = "\"" + line + "\": " + errormessage;
                            break;
                        }
                    }
                }
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

        private void m_testNotification_Click(object sender, EventArgs e)
        {
            SaveData();
            try
            {
                if (Program.RunsWithService != Program.ServiceEnum.CONNECTED)
                {
                    Notifier notifier = new Notifier(m_data);
                    notifier.Test();
                }
                else
                {
                    Program.CommunicationObject.TestNotifier(m_data);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_cbAuthentication_CheckedChanged(object sender, EventArgs e)
        {
            m_tbMailPass.Enabled = labelmailpass.Enabled
            = m_tbMailUsername.Enabled = labelmailuser.Enabled
            = m_cbAuthentication.Checked && m_rbEmail.Checked;
        }

        private void m_cbRestartIbaAnalyzer_CheckedChanged(object sender, EventArgs e)
        {
            m_nudRestartIbaAnalyzer.Enabled = m_cbRestartIbaAnalyzer.Checked;
        }

        private void m_cbDetectNewFiles_CheckedChanged(object sender, EventArgs e)
        {
            if (!m_cbDetectNewFiles.Checked) m_cbRescanEnabled.Checked = true;
        }

        private void m_datDirTextBox_DragOver(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = e.AllowedEffect;
            else
                e.Effect = DragDropEffects.None;
        }

        private void m_datDirTextBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if (files != null && files.Length > 0 && !m_oneTimeJob && Directory.Exists(files[0]))
            {
                m_datDirTextBox.Text = files[0];
            }
            else if (files != null)
            {
                string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                foreach (string line in lines)
                    sb.AppendLine(line);
                foreach (string file in files)
                {
                    if (Directory.Exists(file) || (File.Exists(file) && (new FileInfo(file)).Extension == ".dat"))
                        sb.AppendLine(file);
                }
                m_datDirTextBox.Text = sb.ToString();
            }
        }

        private void m_browseDatFilesButton_Click(object sender, EventArgs e)
        {
            m_selectDatFilesDialog.CheckFileExists = true;
            m_selectDatFilesDialog.FileName = "";
            m_selectDatFilesDialog.Filter = ".dat files (*.dat)|*.dat";
            if (m_selectDatFilesDialog.ShowDialog() == DialogResult.OK)
            {
                string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                foreach (string line in lines)
                    sb.AppendLine(line);
                foreach (string line in m_selectDatFilesDialog.FileNames)
                    sb.AppendLine(line);
                m_datDirTextBox.Text = sb.ToString();
            }
        }
    }
}
