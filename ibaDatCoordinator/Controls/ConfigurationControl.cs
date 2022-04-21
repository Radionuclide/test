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
using iba.Logging;

namespace iba.Controls
{
    public partial class ConfigurationControl : UserControl, IPropertyPane, IPluginsUpdatable
    {
        public ConfigurationControl(ConfigurationData.JobTypeEnum type)
        {
            m_jobType = type;
            InitializeComponent();

            m_sourcePanel.Visible = false;

            if(m_jobType != ConfigurationData.JobTypeEnum.Scheduled && m_jobType != ConfigurationData.JobTypeEnum.Event)
            {
                m_panelDatFilesJob = new PanelDatFilesJob(m_jobType == ConfigurationData.JobTypeEnum.OneTime);
                m_panel = m_panelDatFilesJob;
                m_panelDatFilesJob.Size = m_sourcePanel.Size;
                m_panelDatFilesJob.Location = m_sourcePanel.Location;
                this.Controls.Add(m_panelDatFilesJob);
                if(m_jobType == ConfigurationData.JobTypeEnum.OneTime)
                {
                    m_panelDatFilesJob.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
                    gbNewTask.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                    gbNotifications.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                }
                else
                {
                    m_panelDatFilesJob.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top;
                }
            }
            else
            {
                UserControl lPanel = null;
                if (m_jobType == ConfigurationData.JobTypeEnum.Event)
                {
                    m_panelEventJob = new PanelEventJob();
                    m_panel = m_panelEventJob;
                    lPanel = m_panelEventJob;
                }
                else
                {
                    m_panelScheduledJob = new PanelScheduledJob();
                    m_panel = m_panelScheduledJob;
                    lPanel = m_panelScheduledJob;
                }
                
                int diff = lPanel.Size.Height - m_sourcePanel.Size.Height;
                lPanel.Location = m_sourcePanel.Location;
                lPanel.Width = m_sourcePanel.Width;
                lPanel.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
                this.AutoScrollMinSize = new Size(0, this.AutoScrollMinSize.Height + diff);
                gbNewTask.Location = new Point(gbNewTask.Location.X, gbNewTask.Location.Y + diff);
                gbNewTask.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                gbNotifications.Location = new Point(gbNotifications.Location.X, gbNotifications.Location.Y + diff);
                gbNotifications.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
                this.Controls.Add(lPanel);
            }

            ((Bitmap)m_testNotification.Image).MakeTransparent(Color.Magenta);
            m_toolTip.SetToolTip(m_testNotification, Properties.Resources.testNotifications);

            m_newBatchfileButton.Image = Icons.Gui.All.Images.TerminalCode();
            m_newReportButton.Image = Icons.Gui.All.Images.Report2();
            m_newExtractButton.Image = Icons.Gui.All.Images.DatabaseImport();
            m_newCopyTaskButton.Image = Icons.Gui.All.Images.Copy();
            m_newIfTaskButton.Image = Icons.Gui.All.Images.Condition();
            m_newUpdateDataTaskButton.Image = Icons.Gui.All.Images.DatabaseRefresh();
            m_newPauseTaskButton.Image = Icons.Gui.All.Images.PauseOutline();
            m_newCleanupTaskButton.Image = Icons.Gui.All.Images.CleanErase();
            m_newSplitterTaskButton.Image = Icons.Gui.All.Images.SplitDivide();
            m_newHdCreateEventTaskButton.Image = Icons.Gui.All.Images.HdFlash();
            m_newOPCUAWriterTaskButton.Image = Icons.Gui.All.Images.Opcua();
            m_newSNMPWriterTaskButton.Image = Icons.Gui.All.Images.Snmp();
            m_newUploadTaskButton.Image = Icons.Gui.All.Images.Extract();
            m_newKafkaWriterTaskButton.Image = Icons.Gui.All.Images.ApacheKafka();
            m_newDataTransferTaskButton.Image = Bitmap.FromHicon(Properties.Resources.DataTransferIcon.Handle);

            m_newReportButton.ToolTipText = Properties.Resources.reportButton;
            m_newExtractButton.ToolTipText = Properties.Resources.extractButton;
            m_newBatchfileButton.ToolTipText = Properties.Resources.batchfileButton;
            m_newCopyTaskButton.ToolTipText = Properties.Resources.copytaskButton;
            m_newIfTaskButton.ToolTipText = Properties.Resources.iftaskButton;
            m_newUpdateDataTaskButton.ToolTipText = Properties.Resources.updatedatataskButton;
            m_newPauseTaskButton.ToolTipText = Properties.Resources.pausetaskButton;
            m_newCleanupTaskButton.ToolTipText = Properties.Resources.cleanuptaskButton;
            m_newSplitterTaskButton.ToolTipText = Properties.Resources.splittertaskButton;
            m_newHdCreateEventTaskButton.ToolTipText = Properties.Resources.hdcreateeventtaskButton;
            m_newOPCUAWriterTaskButton.ToolTipText = Properties.Resources.opcUaWriterTaskButton;
            m_newSNMPWriterTaskButton.ToolTipText = Properties.Resources.snmpWriterTaskButton;
            m_newUploadTaskButton.ToolTipText = Properties.Resources.UploadTaskButton;
            m_newKafkaWriterTaskButton.ToolTipText = Properties.Resources.addKafkaWriterTask;
            m_newDataTransferTaskButton.ToolTipText = Properties.Resources.DataTransferTaskButton;

            m_taskCount = m_newTaskToolstrip.Items.Count;
            UpdatePlugins();

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
            else if (m_panelEventJob != null)
            {
                m_stopButton = m_panelEventJob.m_stopButton;
                m_startButton = m_panelEventJob.m_startButton;
                m_applyToRunningBtn = m_panelEventJob.m_applyToRunningBtn;
                m_autoStartCheckBox = m_panelEventJob.m_autoStartCheckBox;
                m_enableCheckBox = m_panelEventJob.m_enableCheckBox;
                m_undoChangesBtn = m_panelEventJob.m_undoChangesBtn;
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
            gbJobName.Init();
            gbNewTask.Init();
            gbNotifications.Init();

            //groupBox managers
            m_ceManager = new CollapsibleElementManager(this);
            m_ceManager.AddElement(gbJobName);
            if(m_panelDatFilesJob != null)
                m_ceManager.AddSubManagerFromControl(m_panelDatFilesJob);
            else if (m_panelEventJob != null)
                m_ceManager.AddSubManagerFromControl(m_panelEventJob);
            else
                m_ceManager.AddSubManagerFromControl(m_panelScheduledJob);
            m_ceManager.AddElement(gbNewTask);
            m_ceManager.AddElement(gbNotifications);

            CueProvider.SetCue(m_tbSender,@"ibaDatCoordinator <noreply@iba-ag.com>");
        }

		public void UpdateLanguage()
		{
			if (m_panelScheduledJob != null)
				m_panelScheduledJob.SetWeekDays();
		}
			

		private int m_taskCount;

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
        private PanelEventJob m_panelEventJob;
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

            UpdateEnabledState();
        }

        public void UpdateEnabledState()
        {
            if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                m_applyToRunningBtn.Enabled = false;
                m_startButton.Enabled = false;
                m_stopButton.Enabled = false;
                m_newTaskToolstrip.Enabled = true;
                if (m_panelScheduledJob != null)
                    m_panelScheduledJob.m_btTriggerNow.Enabled = false;
                if (m_panelDatFilesJob != null && m_data.JobType == ConfigurationData.JobTypeEnum.DatTriggered)
                    m_panelDatFilesJob.m_browseDatFilesButton.Enabled = false;
            }
            else if (TaskManager.Manager.IsJobStarted(m_data.Guid))
            {
                m_applyToRunningBtn.Enabled = true;
                m_startButton.Enabled = false;
                m_newTaskToolstrip.Enabled = false;
                m_stopButton.Enabled = true;
                if (m_panelScheduledJob != null)
                    m_panelScheduledJob.m_btTriggerNow.Enabled = true;
                if (m_panelDatFilesJob != null && m_data.JobType == ConfigurationData.JobTypeEnum.DatTriggered)
                    m_panelDatFilesJob.m_browseDatFilesButton.Enabled = true;
            }
            else
            {
                m_applyToRunningBtn.Enabled = false;
                m_startButton.Enabled = m_data.Enabled;
                m_newTaskToolstrip.Enabled = true;
                m_stopButton.Enabled = false;
                if (m_panelScheduledJob != null)
                    m_panelScheduledJob.m_btTriggerNow.Enabled = false;
                if (m_panelDatFilesJob != null && m_data.JobType == ConfigurationData.JobTypeEnum.DatTriggered)
                    m_panelDatFilesJob.m_browseDatFilesButton.Enabled = false;
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

        public void LeaveCleanup()
        {
            m_panel.LeaveCleanup();
        }

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

           LoadData(m_data, m_manager);

            if (t != null)
            {
                t.UpdateButtons();
                t.SwitchToStatusPane();
                t.StatusBarLabelErrors.Text = ""; //clear any errors on restart
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
        private bool TestTaskCount()
        {
            if (m_data.Tasks.Count >= 1000)
            {
                MessageBox.Show(iba.Properties.Resources.TasksCountExceeded, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        /// <summary>
        /// Creates a <see cref="TreeNode"/> for the task, adds it to job's <see cref="TreeNode"/>,
        /// adds the task to <see cref="ConfigurationData"/> and does other important post-processing like:
        /// replacing configuration in TaskManager, informing ExtMonData about changes, etc.
        /// </summary>
        private void AddNewTaskHelper(TaskData taskData, int imageIndex, TreeItemData treeItemData)
        {
            taskData.SetNextName();
            m_data.Tasks.Add(taskData);
            if (m_data.AdjustDependencies()) Program.MainForm.AdjustFrontIcons(m_data);
            TreeNode newNode = new TreeNode(taskData.Name, imageIndex, imageIndex)
            {
                Tag = treeItemData
            };

            m_manager.LeftTree.SelectedNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            m_manager.LeftTree.SelectedNode = newNode;

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data);

            MainForm.InformExtMonDataAboutTreeStructureChange();
        }

        private void m_newReportButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;
            
            var taskData = new ReportData(m_data);
            var treeItemData = new ReportTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.REPORTTASK_INDEX, treeItemData);
        }

        private void m_newExtractButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new ExtractData(m_data);
            var treeItemData = new ExtractTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.EXTRACTTASK_INDEX, treeItemData);
        }

        private void m_newBatchfileButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new BatchFileData(m_data);
            var treeItemData = new BatchFileTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.BATCHFILETASK_INDEX, treeItemData);
        }

        private void m_newCopyTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new CopyMoveTaskData(m_data);
            var treeItemData = new CopyTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.COPYTASK_INDEX, treeItemData);
        }

        private void m_newIfTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new IfTaskData(m_data);
            var treeItemData = new IfTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.IFTASK_INDEX, treeItemData);
        }

        private void m_newUpdateDataTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new UpdateDataTaskData(m_data);
            var treeItemData = new UpdateDataTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.UPDATEDATATASK_INDEX, treeItemData);
        }

        private void m_newPauseTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new PauseTaskData(m_data);
            var treeItemData = new PauseTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.PAUSETASK_INDEX, treeItemData);
        }

        private void m_newCleanupTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new CleanupTaskData(m_data);
            var treeItemData = new CleanupTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.CLEANUPTASK_INDEX, treeItemData);
        }

        private void m_newSplitterTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new SplitterTaskData(m_data);
            var treeItemData = new SplitterTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.SPLITTERTASK_INDEX, treeItemData);
        }

        private void m_newHDCreateEventTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new HDCreateEventTaskData(m_data);
            var treeItemData = new HDCreateEventTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.HDEVENTTASK_INDEX, treeItemData);
        }

        private void m_newOPCUAWriterTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new OpcUaWriterTaskData(m_data);
            var treeItemData = new OpcUaWriterTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.OPCUA_WRITERTASK_INDEX, treeItemData);
        }

        private void m_newSNMPWriterTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new SnmpWriterTaskData(m_data);
            var treeItemData = new SnmpWriterTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.SNMP_WRITERTASK_INDEX, treeItemData);
        }

        private void m_newKafkaWriterTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new KafkaWriterTaskData(m_data);
            var treeItemData = new KafkaWriterTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.KAFKAWRITERTASK_INDEX, treeItemData);
        }

        private void m_newUploadTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new UploadTaskData(m_data);
            var treeItemData = new UploadTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.UPLOADTASK_INDEX, treeItemData);
        }        
        
        private void m_newDataTransferTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            var taskData = new DataTransferTaskData(m_data);
            var treeItemData = new DataTransferTaskTreeItemData(m_manager, taskData);
            AddNewTaskHelper(taskData, MainForm.DATATRANSFER_TASK_INDEX, treeItemData);
        }

        void newCustomTaskButton_Click(object sender, EventArgs e)
        {
            if (!TestTaskCount())
                return;

            PluginTaskInfo info = (PluginTaskInfo)(((ToolStripButton)sender).Tag);

            TaskData taskData;
            if (info is PluginTaskInfoUNC)
                taskData = new CustomTaskDataUNC(m_data, info);
            else
                taskData = new CustomTaskData(m_data, info);

            ICustomTaskData iCust = (ICustomTaskData)taskData;

            var treeItemData = new CustomTaskTreeItemData(m_manager, iCust);
            int imageIndex = PluginManager.Manager.GetCustomTaskImageIndex(iCust);
            AddNewTaskHelper(taskData, imageIndex, treeItemData);
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
            {
                // If this does not run with a service, changing to false will change this in the local task manager before the option is processed.
                // This is then set in the task manager
                if (m_data.EventData != null)
                    m_data.EventData.HdQueryTimeSpanChanged = false;
                else if (m_data.ScheduleData != null)
                    m_data.ScheduleData.ProcessHistorical = false;

                Program.CommunicationObject.SaveConfigurations();
            }
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

        public void UpdatePlugins()
        {
            while (m_newTaskToolstrip.Items.Count > m_taskCount)
            {
                var but = m_newTaskToolstrip.Items[m_taskCount];
                m_newTaskToolstrip.Items.RemoveAt(m_taskCount);
                but.Dispose();
            }

            foreach (PluginTaskInfo info in PluginManager.Manager.PluginInfos)
            {
                if (info.IsOutdated)
                    continue;

                ToolStripButton bt = new ToolStripButton();
                bt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
                bt.AutoSize = false;
                bt.Height = bt.Width = 40;
                Icon icon = info.Icon;
                if(icon.Size.Width == 16)
                    bt.Image = icon.ToBitmap();
                else
                    bt.Image = new Bitmap(icon.ToBitmap(), new Size(16, 16));               
                bt.ImageScaling = ToolStripItemImageScaling.None;
                bt.ToolTipText = info.Description;
                bt.Tag = info;
                bt.Click += new EventHandler(newCustomTaskButton_Click);
                m_newTaskToolstrip.Items.Add(bt);
            }
        }
    }
}
