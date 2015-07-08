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
        public ConfigurationControl(ConfigurationData.JobTypeEnum type)
        {
            m_jobType = type;
            InitializeComponent();

            m_sourcePanel.Visible = false;

            if(m_jobType != ConfigurationData.JobTypeEnum.Scheduled)
            {
                m_panelDatFilesJob = new PanelDatFilesJob(m_jobType == ConfigurationData.JobTypeEnum.OneTime);
                m_panel = m_panelDatFilesJob;
                m_panelDatFilesJob.Size = m_sourcePanel.Size;
                m_panelDatFilesJob.Location = m_sourcePanel.Location;
                this.Controls.Add(m_panelDatFilesJob);
                if(m_jobType == ConfigurationData.JobTypeEnum.OneTime)
                {
                    m_panelDatFilesJob.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
                    groupBox4.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    groupBox6.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                }
                else
                {
                    m_panelDatFilesJob.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                }
            }
            else
            {
                m_panelScheduledJob = new PanelScheduledJob();
                m_panel = m_panelScheduledJob;
                int diff = m_panelScheduledJob.Size.Height - m_sourcePanel.Size.Height;
                //m_panelScheduledJob.Size = m_sourcePanel.Size;
                m_panelScheduledJob.Location = m_sourcePanel.Location;
                m_panelScheduledJob.Width = m_sourcePanel.Width;
                //this.MinimumSize = new Size(0, this.MinimumSize.Height + diff);
                m_panelScheduledJob.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                this.AutoScrollMinSize = new Size(0, this.AutoScrollMinSize.Height + diff);
                groupBox4.Location = new Point(groupBox4.Location.X, groupBox4.Location.Y + diff);
                groupBox4.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                groupBox6.Location = new Point(groupBox6.Location.X, groupBox6.Location.Y + diff);
                groupBox6.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                this.Controls.Add(m_panelScheduledJob);
            }

            ((Bitmap)m_testNotification.Image).MakeTransparent(Color.Magenta);
            m_toolTip.SetToolTip(m_testNotification, iba.Properties.Resources.testNotifications);

            m_newBatchfileButton.Image = Bitmap.FromHicon(iba.Properties.Resources.batchfile_running.Handle);
            m_newReportButton.Image = Bitmap.FromHicon(iba.Properties.Resources.report_running.Handle);
            m_newExtractButton.Image = Bitmap.FromHicon(iba.Properties.Resources.extract_running.Handle);
            m_newCopyTaskButton.Image = Bitmap.FromHicon(iba.Properties.Resources.copydat_running.Handle);
            m_newIfTaskButton.Image = Bitmap.FromHicon(iba.Properties.Resources.iftask.Handle);
            m_newUpdateDataTaskButton.Image = iba.Properties.Resources.updatedatatask;
            m_newPauseTaskButton.Image = iba.Properties.Resources.pausetask;
            m_newCleanupTaskButton.Image = iba.Properties.Resources.broom;

            m_newReportButton.ToolTipText = iba.Properties.Resources.reportButton;
            m_newExtractButton.ToolTipText = iba.Properties.Resources.extractButton;
            m_newBatchfileButton.ToolTipText = iba.Properties.Resources.batchfileButton;
            m_newCopyTaskButton.ToolTipText = iba.Properties.Resources.copytaskButton;
            m_newIfTaskButton.ToolTipText = iba.Properties.Resources.iftaskButton;
            m_newUpdateDataTaskButton.ToolTipText = iba.Properties.Resources.updatedatataskButton;
            m_newPauseTaskButton.ToolTipText = iba.Properties.Resources.pausetaskButton;
            m_newCleanupTaskButton.ToolTipText = iba.Properties.Resources.cleanuptaskButton;

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

            //common buttons
            if(m_panelDatFilesJob != null)
            {
                m_stopButton = m_panelDatFilesJob.m_stopButton;
                m_startButton = m_panelDatFilesJob.m_startButton;
                m_applyToRunningBtn = m_panelDatFilesJob.m_applyToRunningBtn;
                m_autoStartCheckBox = m_panelDatFilesJob.m_autoStartCheckBox;
                m_enableCheckBox = m_panelDatFilesJob.m_enableCheckBox;
                m_undoChangesBtn = m_panelDatFilesJob.m_undoChangesBtn;
            }
            else
            {
                m_stopButton = m_panelScheduledJob.m_stopButton;
                m_startButton = m_panelScheduledJob.m_startButton;
                m_applyToRunningBtn = m_panelScheduledJob.m_applyToRunningBtn;
                m_autoStartCheckBox = m_panelScheduledJob.m_autoStartCheckBox;
                m_enableCheckBox = m_panelScheduledJob.m_enableCheckBox;
                m_undoChangesBtn = m_panelScheduledJob.m_undoChangesBtn;
            }

            //events of common controls
            this.m_enableCheckBox.CheckedChanged += new System.EventHandler(this.m_enableCheckBox_CheckedChanged);
            this.m_startButton.Click += new System.EventHandler(this.m_startButton_Click);
            this.m_stopButton.Click += new System.EventHandler(this.m_stopButton_Click);
            this.m_applyToRunningBtn.Click += new System.EventHandler(this.m_applyToRunningButton_Click);
            this.m_undoChangesBtn.Click += new System.EventHandler(this.m_undoChangesBtn_Click);


            //tooltips common controls
            m_toolTip.SetToolTip(m_startButton, iba.Properties.Resources.startButton);
            m_toolTip.SetToolTip(m_stopButton, iba.Properties.Resources.stopButton);
            m_toolTip.SetToolTip(m_autoStartCheckBox, iba.Properties.Resources.toolTipAutoStart);
            m_toolTip.SetToolTip(m_applyToRunningBtn, iba.Properties.Resources.applyStartedButton);
            m_toolTip.SetToolTip(m_undoChangesBtn, iba.Properties.Resources.undoChangesButton);


            //init groupBoxes
            groupBox2.Init();
            groupBox4.Init();
            groupBox6.Init();

            //groupBox managers
            m_ceManager = new CollapsibleElementManager(this);
            m_ceManager.AddElement(groupBox2);
            if(m_panelDatFilesJob != null)
                m_ceManager.AddSubManagerFromControl(m_panelDatFilesJob);
            else
                m_ceManager.AddSubManagerFromControl(m_panelScheduledJob);
            m_ceManager.AddElement(groupBox4);
            m_ceManager.AddElement(groupBox6);

            CueProvider.SetCue(m_tbSender,@"ibaDatCoordinator <noreply@iba-ag.com>");
        }

        private CollapsibleElementManager m_ceManager;

        public System.Windows.Forms.Button m_stopButton;
        public System.Windows.Forms.Button m_applyToRunningBtn;
        public System.Windows.Forms.Button m_startButton;
        public System.Windows.Forms.Button m_undoChangesBtn;

        public System.Windows.Forms.CheckBox m_autoStartCheckBox;
        public System.Windows.Forms.CheckBox m_enableCheckBox;

        private ConfigurationData.JobTypeEnum m_jobType;

        private PanelDatFilesJob m_panelDatFilesJob;
        private PanelScheduledJob m_panelScheduledJob;
        private IPropertyPane m_panel;

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        ConfigurationData m_data;
       
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as ConfigurationData;
            m_nameTextBox.Text = m_data.Name;

            m_panel.LoadData(datasource, manager);

            m_autoStartCheckBox.Checked = m_data.AutoStart;
            m_enableCheckBox.Checked = m_data.Enabled;

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

            if(Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                m_applyToRunningBtn.Enabled = false;
                m_startButton.Enabled = false;
                m_stopButton.Enabled = false;
                m_newTaskToolstrip.Enabled = true;
                if(m_panelScheduledJob != null)
                    m_panelScheduledJob.m_btTriggerNow.Enabled = false;
            }
            else if(TaskManager.Manager.IsJobStarted(m_data.Guid))
            {
                m_applyToRunningBtn.Enabled = true;
                m_startButton.Enabled = false;
                m_newTaskToolstrip.Enabled = false;
                m_stopButton.Enabled = true;
                if(m_panelScheduledJob != null)
                    m_panelScheduledJob.m_btTriggerNow.Enabled = true;
            }
            else
            {
                m_applyToRunningBtn.Enabled = false;
                m_startButton.Enabled = m_data.Enabled;
                m_newTaskToolstrip.Enabled = true;
                m_stopButton.Enabled = false;
                if(m_panelScheduledJob != null)
                    m_panelScheduledJob.m_btTriggerNow.Enabled = false;
            }
        }

        public void SaveData()
        {
            m_panel.SaveData();

            m_data.Name = m_nameTextBox.Text;
            m_data.Enabled = m_enableCheckBox.Checked;
            m_data.AutoStart = m_autoStartCheckBox.Checked;
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

            //Debug.Assert(m_data.Clone_AlsoCopyGuids().IsSame(m_data));
                

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

        private void m_enableCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            m_startButton.Enabled = m_enableCheckBox.Checked;
            MainForm.strikeOutNodeText(m_manager.LeftTree.SelectedNode, !m_enableCheckBox.Checked);
            int index = MainForm.DataToRootNodeIndex(m_data);
            if(m_manager.getLeftTree("status").Nodes.Count > 0)
            {
                foreach(TreeNode node in m_manager.getLeftTree("status").Nodes[index].Nodes)
                {
                    if((node.Tag as StatusTreeItemData).CorrConfigurationData.Guid == m_data.Guid)
                        MainForm.strikeOutNodeText(node, !m_enableCheckBox.Checked);
                }
            }
            if(!m_enableCheckBox.Checked)
                TaskManager.Manager.StopConfiguration(m_data.Guid);
            MainForm t = m_manager as MainForm;
            if(t != null) t.UpdateButtons();
        }

        private void m_startButton_Click(object sender, EventArgs e)
        {
            MainForm t = m_manager as MainForm;
            if(!Utility.Crypt.CheckPassword(t)) return;
            SaveData();
            TaskManager.Manager.StartConfiguration(m_data);
            if(Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                Program.CommunicationObject.SaveConfigurations();
            m_startButton.Enabled = false;
            m_newTaskToolstrip.Enabled = false;
            m_applyToRunningBtn.Enabled = true;
            m_stopButton.Enabled = true;
            if(t != null)
            {
                t.UpdateButtons();
                t.SwitchToStatusPane();
                t.StatusBarLabel.Text = ""; //clear any errors on restart
            }
        }

        private void m_stopButton_Click(object sender, EventArgs e)
        {
            MainForm t = m_manager as MainForm;
            if(!Utility.Crypt.CheckPassword(t)) return;
            using(StopWaitDialog w = new StopWaitDialog(m_data))
            {
                w.ShowDialog(ParentForm);
            }
            m_startButton.Enabled = true;
            m_applyToRunningBtn.Enabled = false;
            m_newTaskToolstrip.Enabled = true;
            m_stopButton.Enabled = false;
            if(t != null) t.UpdateButtons();
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
        
        private void m_newCleanupTaskButton_Click(object sender, EventArgs e)
        {
            CleanupTaskData cleanup = new CleanupTaskData(m_data);
            new SetNextName(cleanup);
            m_data.Tasks.Add(cleanup);
            if(m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            TreeNode newNode = new TreeNode(cleanup.Name, MainForm.CLEANUPTASK_INDEX, MainForm.CLEANUPTASK_INDEX);
            newNode.Tag = new CleanupTaskTreeItemData(m_manager, cleanup);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if(Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            m_manager.LeftTree.SelectedNode = newNode;
        }

        void newCustomTaskButton_Click(object sender, EventArgs e)
        {
            PluginTaskInfo info = (PluginTaskInfo)(((ToolStripButton)sender).Tag);
            TaskData cust;
            if (info is PluginTaskInfoUNC)
                cust = new CustomTaskDataUNC(m_data, info);
            else
                cust = new CustomTaskData(m_data, info);
            ICustomTaskData icust = (ICustomTaskData)cust;
            bool IsLicensed = false;
            try
            {
                CDongleInfo dinfo = CDongleInfo.ReadDongle();
                if (dinfo.IsPluginLicensed(icust.Plugin.DongleBitPos))
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

            new SetNextName(cust);
            m_data.Tasks.Add(cust);
 

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);
            int index = PluginManager.Manager.PluginInfos.FindIndex(i => i.Name == info.Name);
            TreeNode newNode = new TreeNode(icust.Name, MainForm.CUSTOMTASK_INDEX + index, MainForm.CUSTOMTASK_INDEX + index);
            newNode.Tag = new CustomTaskTreeItemData(m_manager, icust);
            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            m_manager.LeftTree.SelectedNode = newNode;
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
        }

        private void m_rbImmediate_CheckedChanged(object sender, EventArgs e)
        {
            m_nudNotifyTime.Enabled = m_rbTime.Checked;
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

    }
}
