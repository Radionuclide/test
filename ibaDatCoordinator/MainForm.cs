using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceProcess;
using System.Linq;
using System.Threading;
using iba.Data;
using iba.Controls;
using iba.Utility;
using iba.Processing;
using iba.Plugins;
using Microsoft.Win32;
using ICSharpCode.SharpZipLib.Zip;
using iba.Dialogs;
using iba.Remoting;
using System.Globalization;
// ReSharper disable RedundantNameQualifier

namespace iba
{
    #region MainForm
    public partial class MainForm : Form, IPropertyPaneManager, IExternalCommand
    {
        public static readonly int CONFIGURATION_INDEX = 0;
        public static readonly int SCHEDULED_CONFIGURATION_INDEX = 1;
        public static readonly int EVENT_CONFIGURATION_INDEX = 2;
        public static readonly int ONETIME_CONFIGURATION_INDEX = 3;
        public static readonly int REPORTTASK_INDEX = 4;
        public static readonly int EXTRACTTASK_INDEX = 5;
        public static readonly int BATCHFILETASK_INDEX = 6;
        public static readonly int COPYTASK_INDEX = 7;
        public static readonly int IFTASK_INDEX = 8;
        public static readonly int UPDATEDATATASK_INDEX = 9;
        public static readonly int PAUSETASK_INDEX = 10;
        public static readonly int CLEANUPTASK_INDEX = 11;
        public static readonly int SPLITTERTASK_INDEX = 12;
        // add here any additional indices for new tasks, increase the next numbers
        public static readonly int NEWCONF_INDEX = 13;
        public static readonly int NEW_ONETIME_CONF_INDEX = 14;
        public static readonly int NEW_SCHEDULED_CONF_INDEX = 15;
        public static readonly int NEW_EVENT_CONF_INDEX = 16;
        public static readonly int CUSTOMTASK_INDEX = 17;
        public static readonly int NR_TASKS = 9;

        private QuitForm m_quitForm;

        public MainForm()
        {
            m_firstConnectToService = true;
            InitializeComponent();

            m_quitForm = new QuitForm(this, "ibaDatCoordinatorClientCloseForm", false);
            m_quitForm.CreateHandle(new CreateParams());

            //Setup default toolbar and menu looks
            ToolStripManager.VisualStylesEnabled = true;
            ToolStripManager.Renderer = new ibaToolstripRenderer();

            this.Text = "ibaDatCoordinator v" + DatCoVersion.GetVersion();

            //Initialize logger
            LogControl theLogControl; 
            propertyPanes["logControl"] = theLogControl = new LogControl();
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
                LogData.InitializeStandAloneLogger(theLogControl.LogView, theLogControl);
            else
                LogData.InitializeClientLogger(theLogControl.LogView, theLogControl);
            theLogControl.CreateControl();

            //load plugins
            PluginManager.Manager.LoadPlugins();

            configurationToolStripMenuItem.Enabled = false;
            statusToolStripMenuItem.Enabled = true;
            m_filename = null;
            CreateMenuItems();
            m_watchdogPane.LargeImage = m_watchdogPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.watchdog.Handle);
            // added by kolesnik - begin
            m_snmpPane.LargeImage = m_snmpPane.SmallImage = iba.Properties.Resources.snmp_icon;
            // added by kolesnik - end
            m_statusPane.LargeImage = m_statusPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.status.Handle);
            m_configPane.LargeImage = m_configPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.configuration.Handle);
            m_loggingPane.LargeImage = m_loggingPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.logging.Handle);
            m_settingsPane.LargeImage = m_settingsPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.settings.Handle);
            m_toolTip.SetToolTip(m_startButton, iba.Properties.Resources.toolTipStartAll);
            m_toolTip.SetToolTip(m_stopButton, iba.Properties.Resources.toolTipStopAll);

            ImageList confsImageList = new ImageList();
            confsImageList.Images.Add(iba.Properties.Resources.configuration);
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.scheduled_configuration.ToBitmap()));
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.event_configuration.ToBitmap()));
            confsImageList.Images.Add(iba.Properties.Resources.onetimeconfiguration);
            confsImageList.Images.Add(iba.Properties.Resources.report_running);
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.extract_running.ToBitmap()));
            confsImageList.Images.Add(iba.Properties.Resources.batchfile_running);
            confsImageList.Images.Add(iba.Properties.Resources.copydat_running);
            confsImageList.Images.Add(iba.Properties.Resources.iftask);
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.updatedatatask));
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.pausetask));
            confsImageList.Images.Add(iba.Properties.Resources.broom);
            confsImageList.Images.Add(iba.Properties.Resources.SplitDat);
            confsImageList.Images.Add(iba.Properties.Resources.configuration_new);
            confsImageList.Images.Add(iba.Properties.Resources.onetime_configuration_new);
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.scheduled_configuration_new.ToBitmap()));
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.event_configuration_new.ToBitmap()));
            m_configTreeView.ImageList = confsImageList;
            UpdateImageListConfTree();
                        
            ImageList confsImageList2 = new ImageList();
            confsImageList2.Images.Add(iba.Properties.Resources.greenarrow);
            confsImageList2.Images.Add(iba.Properties.Resources.redarrow); 
            confsImageList2.Images.Add(iba.Properties.Resources.redarrow1);
            m_configTreeView.StateImageList = confsImageList2;
            ImageList statImageList = new ImageList();
            statImageList.Images.Add(iba.Properties.Resources.configuration);
            statImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.scheduled_configuration.ToBitmap()));
            statImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.event_configuration.ToBitmap()));
            statImageList.Images.Add(iba.Properties.Resources.onetimeconfiguration);
            statImageList.Images.Add(iba.Properties.Resources.brokenfile);
            m_statusTreeView.ImageList = statImageList;

            CreateLanguageMenuItems();

            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                m_menuStrip.Items.Remove(serviceToolStripMenuItem);
                this.Icon = iba.Properties.Resources.standalone;
            }
            m_navBar.SelectedPane = m_configPane;
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                //Start connection timer here
                Debug.Assert(m_tryConnectTimer == null);
                m_tryConnectTimer = new System.Threading.Timer(OnConnectTimer);
                m_tryConnectTimer.Change(TimeSpan.FromMilliseconds(1), TimeSpan.Zero);
            }
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                FormStateSerializer.SaveSettings(this, "MainForm");

            SaveRightPaneControl();
           
            string s = String.IsNullOrEmpty(m_filename) ? "not set" : m_filename;
            Profiler.ProfileString(false, "LastState", "LastSavedFile", ref s, "not set");

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && Program.CommunicationObject != null && Program.CommunicationObject.TestConnection())
            {
                //Program.CommunicationObject.Logging_Log(Environment.MachineName + ": Gui Stopped");
                if (m_ef != null)
                    Program.CommunicationObject.Logging_clearEventForwarder(m_ef.Guid);
                Program.CommunicationObject.SaveConfigurations();
            }
            else if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                if (!Utility.Crypt.CheckPassword(this))
                {
                    e.Cancel = true;
                    return;
                }
                string s1 = TextFromLoad();
                string s2 = TextToSave();
                if (!string.IsNullOrEmpty(s2) && s1 != s2)
                {
                    DialogResult res = MessageBox.Show(this, iba.Properties.Resources.saveQuestion,
                            iba.Properties.Resources.closing, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, 
                            MessageBoxDefaultButton.Button2);
                    switch (res)
                    {
                        case DialogResult.Cancel:
                            e.Cancel = true;
                            return;
                        case DialogResult.Yes:
                            saveToolStripMenuItem_Click(null, null);
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
                TaskManager.Manager.StopAllGlobalCleanups();
                using (StopWaitDialog waiter = new StopWaitDialog())
                {
                    waiter.ShowDialog(this);
                }
            }
            
            base.OnClosing(e);
        }

        bool bHandleResize = false;

        public void fromStatusToConfiguration()
        {
            ConfigurationData dat = null;
            StatusTreeItemData stid = m_statusTreeView.SelectedNode.Tag as StatusTreeItemData;
            if (stid == null)
            {
                StatusPermanentlyErrorFilesTreeItemData speftid = m_statusTreeView.SelectedNode.Tag as StatusPermanentlyErrorFilesTreeItemData;
                if (speftid == null) return;
                dat = speftid.CorrConfigurationData;
            }
            else
                dat = stid.CorrConfigurationData;

            foreach (TreeNode confCandidate in m_configTreeView.Nodes)
            {
                ConfigurationTreeItemData dat2 = (confCandidate.Tag as ConfigurationTreeItemData);
                if (dat2 != null && dat2.ConfigurationData.Guid == dat.Guid)
                {
                    m_configTreeView.SelectedNode = confCandidate;
                    break;
                }
            }
            m_navBar.SelectedPane = m_configPane;
        }

        private void navbar_SelectedPaneChanged(object sender, EventArgs e)
        {
            if (m_navBar.SelectedPane == m_statusPane)
            {
                SaveRightPaneControl();
                loadStatuses();
                TreeNode confNode = m_configTreeView.SelectedNode;
                if (confNode != null)
                {
                    bool skipFurther = false;
                    for(int index = 0; index < 3; index++)
                    {
                        if(confNode == m_configTreeView.Nodes[index])
                        {
                            m_statusTreeView.SelectedNode = m_statusTreeView.Nodes[index];
                            skipFurther = true;
                        }
                    }
                    if (!skipFurther && confNode.Tag != null && !(confNode.Tag is ConfigurationTreeItemData))
                        confNode = confNode.Parent;
                    if (!skipFurther && confNode.Tag is ConfigurationTreeItemData)
                    {

                        ConfigurationData dat = (confNode.Tag as ConfigurationTreeItemData).ConfigurationData;
                        TreeNode statNode = null;
                        for(int index = 0; index < 3; index++ )
                            foreach(TreeNode statCandidate in m_statusTreeView.Nodes[index].Nodes)
                            {
                                if(!(statCandidate.Tag is StatusTreeItemData)) continue;
                                if((statCandidate.Tag as StatusTreeItemData).CorrConfigurationData.Guid == dat.Guid)
                                {
                                    statNode = statCandidate;
                                    break;
                                }
                            }
                        if (statNode != null)
                            m_statusTreeView.SelectedNode = statNode;
                    }
                }
                if(m_statusTreeView.SelectedNode == null)
				{
                    if(m_statusTreeView.Nodes.Count > 0) 
                        m_statusTreeView.SelectedNode = m_statusTreeView.Nodes[0];
                    else
                        SetRightPaneControl(null, m_statusPane.Text, null);
                }
				else
				{
                    if(m_statusTreeView.SelectedNode.Tag != null)
                        doSelection(m_statusTreeView.SelectedNode, (m_statusTreeView.SelectedNode.Tag as TreeItemData).What);
				}

                // changed by kolesnik - begin
                EnableAllButOnePaneToolStripMenuItems(statusToolStripMenuItem);
                DisableCopyPasteCutDeleteMenuItems();
                // changed by kolesnik - end
            }
            else if (m_navBar.SelectedPane == m_configPane)
            {
                SaveRightPaneControl();
                TreeNode statNode = m_statusTreeView.SelectedNode;
                bool skipFurther = false;
                if (statNode != null)
                    for(int index = 0; index < 3; index++)
                    {
                        if(statNode == m_statusTreeView.Nodes[index])
                        {
                            m_configTreeView.SelectedNode = m_configTreeView.Nodes[index];
                            skipFurther = true;
                        }
                    }
                if (!skipFurther && statNode != null && statNode.Parent != null && statNode.Parent.Parent != null) statNode = statNode.Parent; //was on permanent error files
                if (!skipFurther && statNode != null && statNode.Parent != null)
                {
                    ConfigurationData confDat = (statNode.Tag as StatusTreeItemData).CorrConfigurationData;

                    for(int index = 0; index < 3; index++)
                        foreach(TreeNode confCandidateNode in m_configTreeView.Nodes[index].Nodes)
                        {
                            if(confCandidateNode.Tag is ConfigurationTreeItemData)
                            {
                                ConfigurationData confCandidateDat = (confCandidateNode.Tag as ConfigurationTreeItemData).ConfigurationData;
                                if(confCandidateDat.Guid == confDat.Guid)
                                {
                                    m_configTreeView.SelectedNode = confCandidateNode;
                                    break;
                                }
                            }
                        }
                }
                if (m_configTreeView.SelectedNode == null || m_configTreeView.SelectedNode.Parent==null) //no node selected or root node (i.e. parentnode of jobs, which has no data)...
                {
                    SetRightPaneControl(null, m_configPane.Text, null);
                }
                else
                {
                    TreeItemData ti = m_configTreeView.SelectedNode.Tag as TreeItemData;
                    if (ti != null) doSelection(m_configTreeView.SelectedNode, ti.What);
                }

                // changed by kolesnik - begin
                EnableAllButOnePaneToolStripMenuItems(configurationToolStripMenuItem);
                // changed by kolesnik - end

                UpdateButtons();
            }
            else if (m_navBar.SelectedPane == m_loggingPane)
            {
                SaveRightPaneControl();
                SetRightPaneControl(propertyPanes["logControl"] as Control, iba.Properties.Resources.logTitle, LogData.Data);

                // changed by kolesnik - begin
                EnableAllButOnePaneToolStripMenuItems(loggingToolStripMenuItem);
                DisableCopyPasteCutDeleteMenuItems();
                // changed by kolesnik - end

                m_EntriesNumericUpDown1.Value = Math.Max(1,LogData.Data.MaxRows);
                int loglevel = LogData.Data.LogLevel;
                m_rbAllLog.Checked = loglevel == 0;
                m_rbErrorsWarnings.Checked = loglevel == 1;
                m_rbOnlyErrors.Checked = loglevel == 2;
            }
            else if (m_navBar.SelectedPane == m_watchdogPane)
            {
                SaveRightPaneControl();
                Control ctrl = propertyPanes["watchdogControl"] as Control;
                if (ctrl == null)
                {
                    ctrl = new WatchdogControl();
                    propertyPanes["watchdogControl"] = ctrl;
                }
                SetRightPaneControl(ctrl as Control, iba.Properties.Resources.watchdogTitle, TaskManager.Manager.WatchDogData.Clone());

                // changed by kolesnik - begin
                EnableAllButOnePaneToolStripMenuItems(watchdogToolStripMenuItem);
                DisableCopyPasteCutDeleteMenuItems();
                // changed by kolesnik - end
            }
            // added by kolesnik - begin
            else if (m_navBar.SelectedPane == m_snmpPane)
            {
                SaveRightPaneControl();
                Control ctrl = propertyPanes["snmpControl"] as Control;
                if (ctrl == null)
                {
                    ctrl = new SnmpControl();
                    propertyPanes["snmpControl"] = ctrl;
                }

                SetRightPaneControl(ctrl as Control, iba.Properties.Resources.snmpTitle,
                    TaskManager.Manager.SnmpData?.Clone());
                EnableAllButOnePaneToolStripMenuItems(snmpToolStripMenuItem);
                DisableCopyPasteCutDeleteMenuItems();
            }
            // added by kolesnik - end

            else if (m_navBar.SelectedPane == m_settingsPane)
            {
                SaveRightPaneControl();
                if (!Utility.Crypt.CheckPassword(this))
                {
                    m_navBar.SelectedPane = m_configPane;
                    return;
                }
                Control ctrl = propertyPanes["settingsControl"] as Control;
                if (ctrl == null)
                {
                    ctrl = new ServiceSettingsControl();
                    propertyPanes["settingsControl"] = ctrl;
                }
                SetRightPaneControl(ctrl as Control, iba.Properties.Resources.settingsTitle, null);

                // changed by kolesnik - begin
                EnableAllButOnePaneToolStripMenuItems(settingsToolStripMenuItem);
                DisableCopyPasteCutDeleteMenuItems();
                // changed by kolesnik - end
            }
        }

        // added by kolesnik - begin
        /// <summary> Enable all the items xxxToolStripMenuItems 
        /// (e.g.  statusToolStripMenuItem, loggingToolStripMenuItem, etc)
        /// except the given one. The given one will be disabled. </summary>
        /// <param name="itemToDisable"></param>
        private void EnableAllButOnePaneToolStripMenuItems(ToolStripMenuItem itemToDisable)
        {
            // enable all menuitems
            statusToolStripMenuItem.Enabled = true;
            configurationToolStripMenuItem.Enabled = true;
            loggingToolStripMenuItem.Enabled = true;
            watchdogToolStripMenuItem.Enabled = true;
            snmpToolStripMenuItem.Enabled = true;
            settingsToolStripMenuItem.Enabled = true;

            // disable the only one of them
            if (itemToDisable!=null)
                itemToDisable.Enabled = false;
        }
        /// <summary> Disables Copy, Paste, Cut and Delete menu items </summary>
        private void DisableCopyPasteCutDeleteMenuItems()
        {
            pasteToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Enabled = false;
            cutToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
        }
        // added by kolesnik - end

        private void ReloadRightPane()
        { //only handles the cases of settings or watchdog panes, the rest is handled differently
            if (m_navBar.SelectedPane == m_watchdogPane)
            {
                WatchdogControl pane = propertyPanes["watchdogControl"] as WatchdogControl;
                if (pane != null)
                {
                    pane.LoadData(TaskManager.Manager.WatchDogData.Clone(), this);
                }
            }
            // added by kolesnik - begin
            if (m_navBar.SelectedPane == m_snmpPane)
            {
                SnmpControl pane = propertyPanes["snmpControl"] as SnmpControl;
                pane?.LoadData(TaskManager.Manager.SnmpData.Clone(), this);
            }
            // added by kolesnik - end
            else if (m_navBar.SelectedPane == m_settingsPane)
            {
                ServiceSettingsControl pane = propertyPanes["settingsControl"] as ServiceSettingsControl;
                if (pane!=null)
                {
                    pane.LoadData(null, this);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FormStateSerializer.LoadSettings(this, "MainForm");

            SetRenderer();
            UpdateConnectionStatus();
            SetupHelp();
            string returnvalue = "";
            Profiler.ProfileString(true, "LastState", "LastSavedFile", ref returnvalue, "not set");
            if (returnvalue != "not set" && Program.RunsWithService != Program.ServiceEnum.CONNECTED) loadFromFile(returnvalue,true);
            loadConfigurations();
            loadStatuses();
            UpdateButtons();
            m_navBar.SelectedPane = m_configPane;
            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                bHandleResize = Program.RunsWithService != Program.ServiceEnum.NOSERVICE;
            }
            // added by kolesnik - begin
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                // Initialize it here only if the app is standalone
                //TaskManager.Manager.SnmpWorker = new SnmpWorker();
                TaskManager.Manager.SnmpWorkerInit();
            }
            // added by kolesnik - end
        }

        private TreeNode CreateConfigurationNode(ConfigurationData confIt)
        {
            TreeNode confNode = null;
            int cindex = 0;
            switch(confIt.JobType)
            {
                case ConfigurationData.JobTypeEnum.DatTriggered: cindex = CONFIGURATION_INDEX; break;
                case ConfigurationData.JobTypeEnum.OneTime: cindex = ONETIME_CONFIGURATION_INDEX; break;
                case ConfigurationData.JobTypeEnum.Scheduled: cindex = SCHEDULED_CONFIGURATION_INDEX; break;
                case ConfigurationData.JobTypeEnum.Event: cindex = EVENT_CONFIGURATION_INDEX; break;
            }
            confNode = new TreeNode(confIt.Name, cindex, cindex);
            MainForm.strikeOutNodeText(confNode, ! confIt.Enabled);
            
            confNode.Tag = new ConfigurationTreeItemData(this, confIt);

            foreach (TaskData task in confIt.Tasks)
            {
                TreeNode taskNode = null;
                if (task is ReportData)
                {
                    taskNode = new TreeNode(task.Name, REPORTTASK_INDEX, REPORTTASK_INDEX);
                    taskNode.Tag = new ReportTreeItemData(this, task as ReportData);
                }
                else if (task is ExtractData)
                {
                    taskNode = new TreeNode(task.Name, EXTRACTTASK_INDEX, EXTRACTTASK_INDEX);
                    taskNode.Tag = new ExtractTreeItemData(this, task as ExtractData);
                }
                else if (task is BatchFileData)
                {
                    taskNode = new TreeNode(task.Name, BATCHFILETASK_INDEX, BATCHFILETASK_INDEX);
                    taskNode.Tag = new BatchFileTreeItemData(this, task as BatchFileData);
                }
                else if (task is CopyMoveTaskData)
                {
                    taskNode = new TreeNode(task.Name, COPYTASK_INDEX, COPYTASK_INDEX);
                    taskNode.Tag = new CopyTaskTreeItemData(this, task as CopyMoveTaskData);
                }
                else if (task is IfTaskData)
                {
                    taskNode = new TreeNode(task.Name, IFTASK_INDEX, IFTASK_INDEX);
                    taskNode.Tag = new IfTaskTreeItemData(this, task as IfTaskData);
                }
                else if (task is UpdateDataTaskData)
                {
                    taskNode = new TreeNode(task.Name, UPDATEDATATASK_INDEX, UPDATEDATATASK_INDEX);
                    taskNode.Tag = new UpdateDataTaskTreeItemData(this, task as UpdateDataTaskData);
                }
                else if (task is PauseTaskData)
                {
                    taskNode = new TreeNode(task.Name, PAUSETASK_INDEX, PAUSETASK_INDEX);
                    taskNode.Tag = new PauseTaskTreeItemData(this, task as PauseTaskData);
                }
                else if(task.GetType() == typeof(TaskWithTargetDirData) || task.GetType() == typeof(CleanupTaskData))
                {
                    taskNode = new TreeNode(task.Name, CLEANUPTASK_INDEX, CLEANUPTASK_INDEX);
                    taskNode.Tag = new CleanupTaskTreeItemData(this, task as TaskWithTargetDirData);
                }
                else if (task.GetType() == typeof(SplitterTaskData))
                {
                    taskNode = new TreeNode(task.Name, SPLITTERTASK_INDEX, SPLITTERTASK_INDEX);
                    taskNode.Tag = new SplitterTaskTreeItemData(this, task as SplitterTaskData);
                }
                else
                {
                    ICustomTaskData cust = task as ICustomTaskData;
                    string name = cust.Plugin.NameInfo;
                    int index = PluginManager.Manager.PluginInfos.FindIndex(delegate(PluginTaskInfo i) { return i.Name == name; });
                    taskNode = new TreeNode(cust.Name, CUSTOMTASK_INDEX + index, CUSTOMTASK_INDEX + index);
                    taskNode.Tag = new CustomTaskTreeItemData(this, cust);
                }

                MainForm.strikeOutNodeText(taskNode, !task.Enabled);
                switch (task.WhenToExecute)
                {
                    case TaskData.WhenToDo.AFTER_SUCCES:
                        taskNode.StateImageIndex = 0;
                        break;
                    case TaskData.WhenToDo.AFTER_FAILURE:
                        taskNode.StateImageIndex = 1;
                        break;
                    case TaskData.WhenToDo.AFTER_1st_FAILURE:
                        taskNode.StateImageIndex = 2;
                        break;
                    default:
                        taskNode.StateImageIndex = -1;
                        break;
                }
                confNode.Nodes.Add(taskNode);
            }
            return confNode;
        }

        private void clearAllConfigurations()
        {
            TaskManager.Manager.ClearConfigurations();
            loadConfigurations();
            //loadStatuses();
        }

        private TreeNode InsertNewConf(ConfigurationData data)
        {
            TreeNode newnode = CreateConfigurationNode(data);
            int index = DataToRootNodeIndex(data);
            TreeNodeCollection coll = m_configTreeView.Nodes[index].Nodes;
            coll.Insert(coll.Count - 1, newnode);
            return newnode;
        }

        private void loadConfigurations()
        {
            m_configTreeView.BeginUpdate();
			m_configTreeView.Nodes.Clear();
            //if (TaskManager.Manager.Count == 0)
            //{
            //    ConfigurationData newData = new ConfigurationData(iba.Properties.Resources.newConfigurationName, ConfigurationData.JobTypeEnum.DatTriggered);
            //    TaskManager.Manager.AddConfiguration(newData);
            //}

            //add three top nodes
            m_configTreeView.Nodes.Add(new TreeNode(iba.Properties.Resources.StandardJobsNodeParent, CONFIGURATION_INDEX, CONFIGURATION_INDEX));
            m_configTreeView.Nodes.Add(new TreeNode(iba.Properties.Resources.ScheduledJobsNodeParent, SCHEDULED_CONFIGURATION_INDEX, SCHEDULED_CONFIGURATION_INDEX));
            m_configTreeView.Nodes.Add(new TreeNode(iba.Properties.Resources.EventJobsNodeParent, EVENT_CONFIGURATION_INDEX, EVENT_CONFIGURATION_INDEX));
            m_configTreeView.Nodes.Add(new TreeNode(iba.Properties.Resources.OneTimeJobsNodeParent, ONETIME_CONFIGURATION_INDEX, ONETIME_CONFIGURATION_INDEX));

            //add the new Configuration node
            TreeNode newConfNode = new TreeNode(iba.Properties.Resources.addConfigurationText, NEWCONF_INDEX, NEWCONF_INDEX);
            newConfNode.ForeColor = Color.Blue;
            newConfNode.Tag = new NewConfigurationTreeItemData(this);
            m_configTreeView.Nodes[0].Nodes.Add(newConfNode);

            //add the scheduled Configuration node
            TreeNode newSConfNode = new TreeNode(iba.Properties.Resources.addScheduledConfigurationText, NEW_SCHEDULED_CONF_INDEX, NEW_SCHEDULED_CONF_INDEX);
            newSConfNode.ForeColor = Color.Blue;
            newSConfNode.Tag = new NewScheduledConfigurationTreeItemData(this);
            m_configTreeView.Nodes[1].Nodes.Add(newSConfNode);

            //add the event Configuration node
            TreeNode newEConfNode = new TreeNode(iba.Properties.Resources.addEventConfigurationText, NEW_EVENT_CONF_INDEX, NEW_EVENT_CONF_INDEX);
            newEConfNode.ForeColor = Color.Blue;
            newEConfNode.Tag = new NewEventConfigurationTreeItemData(this);
            m_configTreeView.Nodes[2].Nodes.Add(newEConfNode);

            //add the new one time Configuration node
            TreeNode new1ConfNode = new TreeNode(iba.Properties.Resources.addOneTimeConfigurationText, NEW_ONETIME_CONF_INDEX, NEW_ONETIME_CONF_INDEX);
            new1ConfNode.ForeColor = Color.Blue;
            new1ConfNode.Tag = new NewOneTimeConfigurationTreeItemData(this);
            m_configTreeView.Nodes[3].Nodes.Add(new1ConfNode);

            TreeNode firstNode = null;

            List<ConfigurationData> confs = new List<ConfigurationData>(TaskManager.Manager.Configurations.Where(c=> c.JobType == ConfigurationData.JobTypeEnum.DatTriggered));
            confs.Sort(delegate (ConfigurationData a, ConfigurationData b) {return a.TreePosition.CompareTo(b.TreePosition);});
            foreach(ConfigurationData confIt in confs)
            {
                TreeNode node = InsertNewConf(confIt);
                if(firstNode == null) firstNode = node;
            }
            confs = new List<ConfigurationData>(TaskManager.Manager.Configurations.Where(c => c.JobType == ConfigurationData.JobTypeEnum.Scheduled));
            confs.Sort(delegate(ConfigurationData a, ConfigurationData b) { return a.TreePosition.CompareTo(b.TreePosition); });
            foreach(ConfigurationData confIt in confs)
            {
                TreeNode node = InsertNewConf(confIt);
                if(firstNode == null) firstNode = node;
            }
            confs = new List<ConfigurationData>(TaskManager.Manager.Configurations.Where(c => c.JobType == ConfigurationData.JobTypeEnum.Event));
            confs.Sort(delegate (ConfigurationData a, ConfigurationData b) { return a.TreePosition.CompareTo(b.TreePosition); });
            foreach (ConfigurationData confIt in confs)
            {
                TreeNode node = InsertNewConf(confIt);
                if (firstNode == null) firstNode = node;
            }
            confs = new List<ConfigurationData>(TaskManager.Manager.Configurations.Where(c => c.JobType == ConfigurationData.JobTypeEnum.OneTime));
            confs.Sort(delegate(ConfigurationData a, ConfigurationData b) { return a.TreePosition.CompareTo(b.TreePosition); });
            foreach(ConfigurationData confIt in confs)
            {
                TreeNode node = InsertNewConf(confIt);
                if(firstNode == null) firstNode = node;
            }

            if (firstNode == null)
            {
                ConfigurationData newData = new ConfigurationData(iba.Properties.Resources.newConfigurationName, ConfigurationData.JobTypeEnum.DatTriggered);
                TaskManager.Manager.AddConfiguration(newData);
                firstNode = InsertNewConf(newData);
            }

            m_configTreeView.EndUpdate();
            m_configTreeView.SelectedNode = firstNode;
            UpdateTreePositions();
            UpdateButtons();
        }

        private void loadStatuses()
        {
            m_statusTreeView.BeginUpdate();
            m_statusTreeView.Nodes.Clear();
            m_statusTreeView.Nodes.Add(new TreeNode(iba.Properties.Resources.StandardJobsNodeParent, CONFIGURATION_INDEX, CONFIGURATION_INDEX));
            m_statusTreeView.Nodes.Add(new TreeNode(iba.Properties.Resources.ScheduledJobsNodeParent, SCHEDULED_CONFIGURATION_INDEX, SCHEDULED_CONFIGURATION_INDEX));
            m_statusTreeView.Nodes.Add(new TreeNode(iba.Properties.Resources.EventJobsNodeParent, EVENT_CONFIGURATION_INDEX, EVENT_CONFIGURATION_INDEX));
            m_statusTreeView.Nodes.Add(new TreeNode(iba.Properties.Resources.OneTimeJobsNodeParent, ONETIME_CONFIGURATION_INDEX, ONETIME_CONFIGURATION_INDEX));
            List<ConfigurationData> allconfs = TaskManager.Manager.Configurations;
            List<ConfigurationData>[] splitconfs = {new List<ConfigurationData>(allconfs.Where(c=>c.JobType == ConfigurationData.JobTypeEnum.DatTriggered)),
                                                    new List<ConfigurationData>(allconfs.Where(c=>c.JobType == ConfigurationData.JobTypeEnum.Scheduled)),
                                                    new List<ConfigurationData>(allconfs.Where(c=>c.JobType == ConfigurationData.JobTypeEnum.Event)),
                                                    new List<ConfigurationData>(allconfs.Where(c=>c.JobType == ConfigurationData.JobTypeEnum.OneTime)),
                                                        };
            for(int jobtypeindex = 0; jobtypeindex < 4; jobtypeindex++)
            {
                splitconfs[jobtypeindex].Sort(delegate(ConfigurationData a, ConfigurationData b) { return a.TreePosition.CompareTo(b.TreePosition); });
                foreach(ConfigurationData confIt in splitconfs[jobtypeindex])
                {
                    TreeNode statNode;
                    statNode = new TreeNode(confIt.Name, jobtypeindex, jobtypeindex);
                    statNode.Tag = new StatusTreeItemData(this as IPropertyPaneManager, confIt);
                    MainForm.strikeOutNodeText(statNode, !confIt.Enabled);
                    m_statusTreeView.Nodes[jobtypeindex].Nodes.Add(statNode);
                    if(confIt.LimitTimesTried)
                    {
                        string text = jobtypeindex==1?iba.Properties.Resources.PermanentlyFailedTriggers:iba.Properties.Resources.PermanentlyFailedDatFiles;
                        TreeNode permFailedNode = new TreeNode(text, 3, 3);
                        permFailedNode.Tag = new StatusPermanentlyErrorFilesTreeItemData(this as IPropertyPaneManager, confIt);
                        statNode.Nodes.Add(permFailedNode);
                    }
                }
            }
            m_statusTreeView.EndUpdate();
            m_statusTreeView.SelectedNode = m_statusTreeView.Nodes[0];
        }

        private void OnStatusTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if (node == null) return;
            if(node.Tag == null) //root items
            {
                SetRightPaneControl(null, "", null);
                return;
            }
            using (WaitCursor wait = new WaitCursor())
            {
                doSelection(node, (m_statusTreeView.SelectedNode.Tag as TreeItemData).What);
            }
        }

        private void doSelection(TreeNode node, string what)
        {
            string title = node.Text;
            TreeNode copyNode = node;
            while (copyNode.Parent != null && copyNode.Parent.Parent != null)
			{
				copyNode = copyNode.Parent;
                title = copyNode.Text + " - " + title;
			}
            title =  m_navBar.SelectedPane.Text + ": " + title;

            Control ctrl = null;
            switch (what)
            {
                case "Status":
                    {
                        if (m_navBar.SelectedPane != m_statusPane) return;
                        StatusTreeItemData data = node.Tag as StatusTreeItemData;
                        if (data == null) return;
                        ctrl = data.CreateControl();
                        SetRightPaneControl(ctrl, title, TaskManager.Manager.GetMinimalStatus(data.CorrConfigurationData.Guid,false));
                        pasteToolStripMenuItem.Enabled = false;
                        copyToolStripMenuItem.Enabled = false;
                        cutToolStripMenuItem.Enabled = false;
                        deleteToolStripMenuItem.Enabled = false;
                        break;
                    }
                case "PermanentlyErrorFiles":
                    {
                        if (m_navBar.SelectedPane != m_statusPane) return;
                        StatusPermanentlyErrorFilesTreeItemData data = node.Tag as StatusPermanentlyErrorFilesTreeItemData;
                        if (data == null) return;
                        ctrl = data.CreateControl();
                        SetRightPaneControl(ctrl, title, TaskManager.Manager.GetMinimalStatus(data.CorrConfigurationData.Guid,true));
                        pasteToolStripMenuItem.Enabled = false;
                        copyToolStripMenuItem.Enabled = false;
                        cutToolStripMenuItem.Enabled = false;
                        deleteToolStripMenuItem.Enabled = false;
                        break;
                    }
                case "Configuration":
                    {
                        if (m_navBar.SelectedPane != m_configPane) return;
                        ConfigurationTreeItemData data = node.Tag as ConfigurationTreeItemData;
                        if (data == null) return;
                        ctrl = data.CreateControl();
                        SetRightPaneControl(ctrl, title, data.ConfigurationData);
                        pasteToolStripMenuItem.Enabled = m_cd_copy != null;
                        bool started = TaskManager.Manager.IsJobStarted(data.ConfigurationData.Guid);
                        if (m_task_copy != null && !m_confCopiedMostRecent)
                            pasteToolStripMenuItem.Enabled = !started;
                        copyToolStripMenuItem.Enabled = true;
                        cutToolStripMenuItem.Enabled = !started;
                        deleteToolStripMenuItem.Enabled = !started;
                        break;
                    }
                case "BatchFile":
                case "Report":
                case "Extract":
                case "CopyTask":
                case "IfTask":
                case "UpdateDataTask":
                case "PauseTask":
                case "CleanupTask":
                case "SplitterTask":
                case "CustomTaskUNC":
                case "CustomTask":
                case "task":
                    {
                        if (m_navBar.SelectedPane != m_configPane) return;
                        TreeItemData data = node.Tag as TreeItemData;
                        if (data == null) return;
                        ctrl = data.CreateControl();
                        bool started = TaskManager.Manager.IsJobStarted((data.DataSource as TaskData).ParentConfigurationData.Guid);
                        SetRightPaneControl(ctrl, title, data.DataSource);
                        pasteToolStripMenuItem.Enabled = (m_task_copy != null && !started);
                        copyToolStripMenuItem.Enabled = true;
                        cutToolStripMenuItem.Enabled = !started;
                        deleteToolStripMenuItem.Enabled = !started;
                        break;
                    }
                default:
                    break;
            }
        }

        private void OnConfigurationTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            if(node == null) return;
            if(node.Tag == null) //root items
            {
                SetRightPaneControl(null, "", null);
                return;
            }
            using (WaitCursor wait = new WaitCursor())
            {
                bool c1 = node.Tag is NewConfigurationTreeItemData;
                bool c2 = node.Tag is NewOneTimeConfigurationTreeItemData;
                bool c3 = node.Tag is NewScheduledConfigurationTreeItemData;
                bool c4 = node.Tag is NewEventConfigurationTreeItemData;
                if (c1 || c2 || c3 || c4)
                {
                    if (!Utility.Crypt.CheckPassword(this)) return;
                    SaveRightPaneControl();
                    //code to create new configuration
                    ConfigurationData.JobTypeEnum type = ConfigurationData.JobTypeEnum.DatTriggered;
                    if (c2)
                        type = ConfigurationData.JobTypeEnum.OneTime;
                    else if (c3)
                        type = ConfigurationData.JobTypeEnum.Scheduled;
                    else if (c4)
                        type = ConfigurationData.JobTypeEnum.Event;
                    ConfigurationData newData = new ConfigurationData(iba.Properties.Resources.newConfigurationName, type);
                    new SetNextName(newData);
                    TaskManager.Manager.AddConfiguration(newData);
                    m_configTreeView.BeginUpdate();
                    node = InsertNewConf(newData);
                    //loadConfigurations();
                    m_configTreeView.EndUpdate();
                    loadStatuses();
                    m_configTreeView.SelectedNode = node;
                    UpdateButtons();
                    UpdateTreePositions();
                }
                else if (node.Tag != null)
                {
                    TreeItemData t = node.Tag as TreeItemData;
                    doSelection(node, t.What);
                }
            }
        }

        public void SetRightPaneControl(Control newControl, string title, object datasource)
        {
            //Save data from old control
            bool bOldControl = this.m_rightPane.Controls.Count > 0;
            if (bOldControl)
            {
                Debug.Assert(m_rightPane.Controls.Count == 1, "Only 1 control allowed in rightpane");
                IPropertyPane pane = m_rightPane.Controls[0] as IPropertyPane;
                if (pane != null)
                {
                    pane.LeaveCleanup();
                    pane.SaveData();
                }
            }

            //Set new control
            m_rightPane.Text = title;
            if (newControl != null)
            {
                IPropertyPane pane = newControl as IPropertyPane;
                if (pane != null)
                    pane.LoadData(datasource, this);
                newControl.Dock = DockStyle.Fill;
                m_rightPane.Controls.Add(newControl);
                //m_rightPane.AutoScrollMinSize = new Size(newControl.MinimumSize.Width,newControl.MinimumSize.Height+20);
            }

            //Remove old control, do necessary cleanup
            if (bOldControl && (newControl != m_rightPane.Controls[0]))
            {
                m_rightPane.Controls.RemoveAt(0);
            }
        }

        public void SaveRightPaneControl()
        {
            if (m_rightPane.Controls.Count > 0)
            {
                Debug.Assert(m_rightPane.Controls.Count == 1, "Only 1 control allowed in rightpane");
                IPropertyPane pane = m_rightPane.Controls[0] as IPropertyPane;
                if (pane != null)
                    pane.SaveData();
            }
        }

        private void m_configTreeView_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode != Keys.Delete) || (m_configTreeView.SelectedNode == null) || !(m_configTreeView.Focused) || (m_configTreeView.SelectedNode.Tag == null))
                return;
            Delete(m_configTreeView.SelectedNode);
        }

        private void Delete(TreeNode node)
        {
            Delete(node, true);
        }

        private void Delete(TreeNode node, bool ask)
        {
            if (node == null || node.Tag == null) return;
            if (ask)
            {
                string msg = null;
                if (node.Tag is ConfigurationTreeItemData)
                {
                    if (!Utility.Crypt.CheckPassword(this)) return;
                    if (TaskManager.Manager.IsJobStarted((node.Tag as ConfigurationTreeItemData).ConfigurationData.Guid)) return;
                    msg = String.Format(iba.Properties.Resources.deleteConfigurationQuestion, node.Text);
                }
                else if (node.Parent != null && node.Parent.Tag is ConfigurationTreeItemData)
                {
                    if (TaskManager.Manager.IsJobStarted((node.Parent.Tag as ConfigurationTreeItemData).ConfigurationData.Guid)) return;
                    if (node.Tag is BatchFileTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteBatchfileQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is ReportTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteReportQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is ExtractTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteExtractQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is CopyTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteCopyTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is IfTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteIfTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is UpdateDataTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteUpdateDataTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is PauseTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deletePauseTaskQuestion, node.Text, node.Parent.Text);
                    else if(node.Tag is CleanupTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteCleanupTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is SplitterTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteSplitterTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is CustomTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteCustomTaskQuestion,
                        (((CustomTaskTreeItemData)(node.Tag)).DataSource as ICustomTaskData).Plugin.NameInfo,
                            node.Text, node.Parent.Text);
                }
                DialogResult res = MessageBox.Show(this, msg,
                    iba.Properties.Resources.deleteTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (res != DialogResult.Yes)
                    return;
            }
            //Delete node in tree
            if (node.Tag is ConfigurationTreeItemData)
            {
                TreeNode parent = node.Parent;
                if (TaskManager.Manager.IsJobStarted((node.Tag as ConfigurationTreeItemData).ConfigurationData.Guid))
                    return;
                TreeNode nextNode = node.NextNode;
                if(nextNode == null || nextNode.Index == nextNode.Parent.Nodes.Count-1) nextNode = node.PrevNode;
                if(nextNode == null) nextNode = node.Parent;
                
                
                m_configTreeView.SelectedNode = null;
                TaskManager.Manager.RemoveConfiguration((node.Tag as ConfigurationTreeItemData).ConfigurationData);
                m_configTreeView.Nodes.Remove(node);
                m_configTreeView.SelectedNode = nextNode;
                UpdateTreePositions();
                UpdateButtons();
            }
            else if (node.Tag is NewConfigurationTreeItemDataBase)
            {
                //should never happen you are here
            }
            else
            {
                TreeNode nextNode = node.NextNode;
                if (nextNode == null) nextNode = node.PrevNode;
                if (nextNode == null) nextNode = node.Parent;
                ConfigurationData confParent = (node.Parent.Tag as ConfigurationTreeItemData).ConfigurationData;
                if (TaskManager.Manager.IsJobStarted(confParent.Guid)) return;

                m_configTreeView.SelectedNode = null;
                TaskData task = (node.Tag as TreeItemData).DataSource as TaskData;
               
                m_configTreeView.SelectedNode = nextNode;
                confParent.Tasks.Remove(task);
                if (confParent.AdjustDependencies()) AdjustFrontIcons(confParent);
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    TaskManager.Manager.ReplaceConfiguration(confParent);
                node.Remove();
            }
        }

        private bool m_confCopiedMostRecent;

        private void copyNode(TreeNode node)
        {
            if (node == null) return;
            SaveRightPaneControl();
            if (node.Tag is ConfigurationTreeItemData)
            {
                m_cd_copy = (node.Tag as ConfigurationTreeItemData).ConfigurationData.Clone() as ConfigurationData;
                m_confCopiedMostRecent = true;
            }
            else 
            {
                TreeItemData tr = (node.Tag as TreeItemData);
                if (tr != null && tr.DataSource != null && tr.DataSource is TaskData)
                {
                m_task_copy = (tr.DataSource as TaskData).Clone() as TaskData;
                }
                m_confCopiedMostRecent = false;
            }
        }

        private void pasteNode(TreeNode node)
        {
            if ((node == null || node.Tag==null) && m_cd_copy != null) //add configurationData
            {
                SaveRightPaneControl();
                //code to create new configuration
                new SetNextName(m_cd_copy);
                foreach (TaskData t in m_cd_copy.Tasks)
                    new SetNextName(t);
                TaskManager.Manager.AddConfiguration(m_cd_copy);
                m_configTreeView.BeginUpdate();
                node = InsertNewConf(m_cd_copy);
                node.EnsureVisible();
                //loadConfigurations();
                m_configTreeView.EndUpdate();
                m_configTreeView.SelectedNode = node;
                m_cd_copy = m_cd_copy.Clone() as ConfigurationData;
                UpdateTreePositions();
                //loadStatuses();
                UpdateButtons();
            }
            else if (node.Tag is ConfigurationTreeItemData && m_cd_copy != null && m_confCopiedMostRecent)
            {
                SaveRightPaneControl();
                int index = node.Index;
                //code to create new configuration
                new SetNextName(m_cd_copy);
                foreach (TaskData t in m_cd_copy.Tasks)
                    new SetNextName(t);
                TaskManager.Manager.AddConfiguration(m_cd_copy);
                m_configTreeView.BeginUpdate();
                TreeNode tn = CreateConfigurationNode(m_cd_copy);
                m_configTreeView.Nodes[DataToRootNodeIndex(m_cd_copy)].Nodes.Insert(index, tn);
                tn.EnsureVisible();
                //loadConfigurations();
                m_configTreeView.EndUpdate();
                UpdateTreePositions();
                m_configTreeView.SelectedNode = tn;
                m_cd_copy = m_cd_copy.Clone() as ConfigurationData;
                //loadStatuses();
                UpdateButtons();
            }
            else if (node.Tag is ConfigurationTreeItemData && m_task_copy != null)
            {
                SaveRightPaneControl();
                ConfigurationData origData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
                if (!TestTaskCount(origData))
                    return;
                m_task_copy.ParentConfigurationData = origData;
                new SetNextName(m_task_copy);
                origData.Tasks.Add(m_task_copy);
                TreeNode taskNode = null;
                if (m_task_copy is ReportData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, REPORTTASK_INDEX, REPORTTASK_INDEX);
                    taskNode.Tag = new ReportTreeItemData(this, m_task_copy as ReportData);
                }
                else if (m_task_copy is ExtractData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, EXTRACTTASK_INDEX, EXTRACTTASK_INDEX);
                    taskNode.Tag = new ExtractTreeItemData(this, m_task_copy as ExtractData);
                }
                else if (m_task_copy is BatchFileData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, BATCHFILETASK_INDEX, BATCHFILETASK_INDEX);
                    taskNode.Tag = new BatchFileTreeItemData(this, m_task_copy as BatchFileData);
                }
                else if (m_task_copy is CopyMoveTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, COPYTASK_INDEX, COPYTASK_INDEX);
                    taskNode.Tag = new CopyTaskTreeItemData(this, m_task_copy as CopyMoveTaskData);
                }
                else if (m_task_copy is IfTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, IFTASK_INDEX, IFTASK_INDEX);
                    taskNode.Tag = new IfTaskTreeItemData(this, m_task_copy as IfTaskData);
                }
                else if (m_task_copy is UpdateDataTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, UPDATEDATATASK_INDEX, UPDATEDATATASK_INDEX);
                    taskNode.Tag = new UpdateDataTaskTreeItemData(this, m_task_copy as UpdateDataTaskData);
                }
                else if (m_task_copy is PauseTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, PAUSETASK_INDEX, PAUSETASK_INDEX);
                    taskNode.Tag = new PauseTaskTreeItemData(this, m_task_copy as PauseTaskData);
                }
                else if(m_task_copy.GetType() == typeof(TaskWithTargetDirData))
                {
                    taskNode = new TreeNode(m_task_copy.Name, CLEANUPTASK_INDEX, CLEANUPTASK_INDEX);
                    taskNode.Tag = new CleanupTaskTreeItemData(this, m_task_copy as TaskWithTargetDirData);
                }
                else if (m_task_copy.GetType() == typeof(SplitterTaskData))
                {
                    taskNode = new TreeNode(m_task_copy.Name, SPLITTERTASK_INDEX, SPLITTERTASK_INDEX);
                    taskNode.Tag = new SplitterTaskTreeItemData(this, m_task_copy as SplitterTaskData);
                }
                else if (m_task_copy is ICustomTaskData)
                {
                    ICustomTaskData cust = (ICustomTaskData) m_task_copy;
                    string name = cust.Plugin.NameInfo;
                    int index = PluginManager.Manager.PluginInfos.FindIndex(delegate(PluginTaskInfo i) { return i.Name== name; });
                    taskNode = new TreeNode(m_task_copy.Name, CUSTOMTASK_INDEX + index, CUSTOMTASK_INDEX + index);
                    taskNode.Tag = new CustomTaskTreeItemData(this,cust);
                }

                MainForm.strikeOutNodeText(taskNode, !m_task_copy.Enabled);
                switch (m_task_copy.WhenToExecute)
                {
                    case TaskData.WhenToDo.AFTER_1st_FAILURE:
                        taskNode.StateImageIndex = 2;
                        break;
                    case TaskData.WhenToDo.AFTER_FAILURE:
                        taskNode.StateImageIndex = 1;
                        break;
                    case TaskData.WhenToDo.AFTER_SUCCES:
                        taskNode.StateImageIndex = 0;
                        break;
                    default:
                        taskNode.StateImageIndex = -1;
                        break;
                }
                node.Nodes.Add(taskNode); 
                taskNode.EnsureVisible();
                m_configTreeView.SelectedNode = taskNode;
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    TaskManager.Manager.ReplaceConfiguration(origData);
                m_task_copy = m_task_copy.Clone() as TaskData;
            }
            else if (!(node.Tag is NewConfigurationTreeItemDataBase) && m_task_copy != null)
            {
                SaveRightPaneControl();
                ConfigurationData origData = (node.Parent.Tag as ConfigurationTreeItemData).ConfigurationData;
                m_task_copy.ParentConfigurationData = origData;
                if (!TestTaskCount(origData))
                    return;
                TreeNode taskNode = null;
                new SetNextName(m_task_copy);
                if (m_task_copy is ReportData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, REPORTTASK_INDEX, REPORTTASK_INDEX);
                    taskNode.Tag = new ReportTreeItemData(this, m_task_copy as ReportData);
                }
                else if (m_task_copy is ExtractData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, EXTRACTTASK_INDEX, EXTRACTTASK_INDEX);
                    taskNode.Tag = new ExtractTreeItemData(this, m_task_copy as ExtractData);
                }
                else if (m_task_copy is BatchFileData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, BATCHFILETASK_INDEX, BATCHFILETASK_INDEX);
                    taskNode.Tag = new BatchFileTreeItemData(this, m_task_copy as BatchFileData);
                }
                else if (m_task_copy is CopyMoveTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, COPYTASK_INDEX, COPYTASK_INDEX);
                    taskNode.Tag = new CopyTaskTreeItemData(this, m_task_copy as CopyMoveTaskData);
                }
                else if (m_task_copy is IfTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, IFTASK_INDEX, IFTASK_INDEX);
                    taskNode.Tag = new IfTaskTreeItemData(this, m_task_copy as IfTaskData);
                }
                else if (m_task_copy is UpdateDataTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, UPDATEDATATASK_INDEX, UPDATEDATATASK_INDEX);
                    taskNode.Tag = new UpdateDataTaskTreeItemData(this, m_task_copy as UpdateDataTaskData);
                }
                else if (m_task_copy is PauseTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, PAUSETASK_INDEX, PAUSETASK_INDEX);
                    taskNode.Tag = new PauseTaskTreeItemData(this, m_task_copy as PauseTaskData);
                }
                else if(m_task_copy.GetType() == typeof(TaskWithTargetDirData))
                {
                    taskNode = new TreeNode(m_task_copy.Name, CLEANUPTASK_INDEX, CLEANUPTASK_INDEX);
                    taskNode.Tag = new CleanupTaskTreeItemData(this, m_task_copy as TaskWithTargetDirData);
                }
                else if (m_task_copy.GetType() == typeof(SplitterTaskData))
                {
                    taskNode = new TreeNode(m_task_copy.Name, SPLITTERTASK_INDEX, SPLITTERTASK_INDEX);
                    taskNode.Tag = new SplitterTaskTreeItemData(this, m_task_copy as SplitterTaskData);
                }
                else if (m_task_copy is ICustomTaskData)
                {
                    ICustomTaskData cust = (ICustomTaskData)m_task_copy;
                    string name = cust.Plugin.NameInfo;
                    int index = PluginManager.Manager.PluginInfos.FindIndex(delegate(PluginTaskInfo i) { return i.Name == name; });
                    taskNode = new TreeNode(m_task_copy.Name, CUSTOMTASK_INDEX + index, CUSTOMTASK_INDEX + index);
                    taskNode.Tag = new CustomTaskTreeItemData(this, cust);
                }


                MainForm.strikeOutNodeText(taskNode, !m_task_copy.Enabled);
                switch (m_task_copy.WhenToExecute)
                {
                    case TaskData.WhenToDo.AFTER_1st_FAILURE:
                        taskNode.StateImageIndex = 2;
                        break;
                    case TaskData.WhenToDo.AFTER_FAILURE:
                        taskNode.StateImageIndex = 1;
                        break;
                    case TaskData.WhenToDo.AFTER_SUCCES:
                        taskNode.StateImageIndex = 0;
                        break;
                    default:
                        taskNode.StateImageIndex = -1;
                        break;
                }
                int index2 = node.Index;
                origData.Tasks.Insert(index2, m_task_copy);
                node.Parent.Nodes.Insert(index2, taskNode);
                if (origData.AdjustDependencies()) AdjustFrontIcons(origData); 
                taskNode.EnsureVisible();
                m_configTreeView.SelectedNode = taskNode;
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    TaskManager.Manager.ReplaceConfiguration(origData);
                m_task_copy = m_task_copy.Clone() as TaskData;
            }
        }

        #region Popup menu stuff
        private ContextMenuStrip m_popupMenu = new ContextMenuStrip();
        private ToolStripMenuItem[] m_menuItems;


        //methods for rightclicking copy paste
        private ConfigurationData m_cd_copy = null;
        private TaskData m_task_copy = null;

        private void CreateMenuItems()
        {
            if (m_menuItems != null) //dispose the old items
            {
                foreach(ToolStripMenuItem item in m_menuItems)
                {
                    item.Dispose();
                }
                m_menuItems = null;
            }

            MyImageList il = new MyImageList();
            ImageList menuImages = new ImageList();
            menuImages.ColorDepth = ColorDepth.Depth32Bit;
            menuImages.Images.Add(iba.Properties.Resources.cut);
            menuImages.Images.Add(iba.Properties.Resources.copy);
            menuImages.Images.Add(iba.Properties.Resources.paste);
            menuImages.Images.Add(iba.Properties.Resources.report_running);
            menuImages.Images.Add(iba.Properties.Resources.extract_running);
            menuImages.Images.Add(iba.Properties.Resources.batchfile_running);
            menuImages.Images.Add(iba.Properties.Resources.copydat_running);
            menuImages.Images.Add(iba.Properties.Resources.iftask);
            menuImages.Images.Add(iba.Properties.Resources.updatedatatask);
            menuImages.Images.Add(iba.Properties.Resources.pausetask);
            menuImages.Images.Add(iba.Properties.Resources.broom);
            menuImages.Images.Add(iba.Properties.Resources.SplitDat);
            foreach (PluginTaskInfo info in PluginManager.Manager.PluginInfos)
                menuImages.Images.Add(info.Icon);

            int customcount = PluginManager.Manager.PluginInfos.Count;
            m_menuItems = new ToolStripMenuItem[15 + customcount];
            m_menuItems[(int)MenuItemsEnum.Delete] = new ToolStripMenuItem(iba.Properties.Resources.deleteTitle, il.List.Images[MyImageList.Delete], new EventHandler(OnDeleteMenuItem), Keys.Delete);
            m_menuItems[(int)MenuItemsEnum.CollapseAll] = new ToolStripMenuItem(iba.Properties.Resources.collapseTitle, null,new EventHandler(OnCollapseAllMenuItem));
            m_menuItems[(int)MenuItemsEnum.Cut] = new ToolStripMenuItem(iba.Properties.Resources.cutTitle, menuImages.Images[0], new EventHandler(OnCutMenuItem), Keys.X | Keys.Control);
            m_menuItems[(int)MenuItemsEnum.Copy] = new ToolStripMenuItem(iba.Properties.Resources.copyTitle, menuImages.Images[1], new EventHandler(OnCopyMenuItem), Keys.C | Keys.Control);
            m_menuItems[(int)MenuItemsEnum.Paste] = new ToolStripMenuItem(iba.Properties.Resources.pasteTitle, menuImages.Images[2], new EventHandler(OnPasteMenuItem), Keys.V | Keys.Control);
            m_menuItems[(int)MenuItemsEnum.NewTask] = new ToolStripMenuItem(iba.Properties.Resources.NewTaskTitle, null, null, iba.Properties.Resources.NewTaskTitle);
            

            m_menuItems[(int)MenuItemsEnum.NewTask] = new ToolStripMenuItem(iba.Properties.Resources.NewTaskTitle);
            m_menuItems[(int)MenuItemsEnum.NewReport] = new ToolStripMenuItem(iba.Properties.Resources.NewReportTitle, menuImages.Images[3], new EventHandler(OnNewReportMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewExtract] = new ToolStripMenuItem(iba.Properties.Resources.NewExtractTitle,  menuImages.Images[4], new EventHandler(OnNewExtractMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewBatchfile] = new ToolStripMenuItem(iba.Properties.Resources.NewBatchfileTitle, menuImages.Images[5], new EventHandler(OnNewBatchfileMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewCopyTask] = new ToolStripMenuItem(iba.Properties.Resources.NewCopyTaskTitle, menuImages.Images[6], new EventHandler(OnNewCopyTaskMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewIfTask] = new ToolStripMenuItem(iba.Properties.Resources.NewIfTaskTitle, menuImages.Images[7], new EventHandler(OnNewIfTaskMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewUpdateDataTask] = new ToolStripMenuItem(iba.Properties.Resources.NewUpdateDataTaskTitle, iba.Properties.Resources.updatedatatask, new EventHandler(OnNewUpdateDataTaskMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewPauseTask] = new ToolStripMenuItem(iba.Properties.Resources.NewPauseTaskTitle, iba.Properties.Resources.pausetask, new EventHandler(OnNewPauseTaskMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewCleanupTask] = new ToolStripMenuItem(iba.Properties.Resources.NewCleanupTaskTitle, iba.Properties.Resources.broom, new EventHandler(OnNewCleanupTaskMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewSplitterTask] = new ToolStripMenuItem(iba.Properties.Resources.NewSplitterTaskTitle, menuImages.Images[11], new EventHandler(OnNewSplitterTaskMenuItem));
            
            for (int i = 0; i < customcount; i++)
            {
                string title = String.Format(iba.Properties.Resources.NewCustomTaskTitle, PluginManager.Manager.PluginInfos[i].Name);
                m_menuItems[i + (int)MenuItemsEnum.NewCustomTask] = new ToolStripMenuItem(title, menuImages.Images[NR_TASKS+3+i], new EventHandler(OnNewCustomTaskMenuItem));
            }
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown = new ContextMenuStrip();

            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewReport]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewExtract]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewBatchfile]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewCopyTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewIfTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewUpdateDataTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewPauseTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewCleanupTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewSplitterTask]);
            for (int i = 0; i < customcount; i++)
            {
                m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[i + (int)MenuItemsEnum.NewCustomTask]);
            }
        }

        internal enum MenuItemsEnum
        {
            Delete = 0,
            CollapseAll = 1,
            Cut = 2,
            Copy = 3,
            Paste = 4,
            NewTask = 5,
            NewReport = 6,
            NewExtract = 7,
            NewBatchfile = 8,
            NewCopyTask = 9,
            NewIfTask = 10,
            NewUpdateDataTask = 11,
            NewPauseTask = 12,
            NewCleanupTask = 13,
            NewSplitterTask = 14,
            NewCustomTask = 15
        }

        private string ibaAnalyzerExe;

        private void m_configTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            TreeNode node = m_configTreeView.GetNodeAt(e.X, e.Y);
            ArrayList items = new ArrayList();
            if (node != null && node.Tag != null)
            {
                bool started = false;
                if (node.Parent != null && node.Parent.Parent != null) //tasknode
                    started = TaskManager.Manager.IsJobStarted((node.Parent.Tag as ConfigurationTreeItemData).ConfigurationData.Guid);
                else if (node.Tag is ConfigurationTreeItemData)
                    started = TaskManager.Manager.IsJobStarted((node.Tag as ConfigurationTreeItemData).ConfigurationData.Guid);

                TreeItemData data = node.Tag as TreeItemData;
                m_configTreeView.SelectedNode = node;
                deleteToolStripMenuItem.Enabled = m_menuItems[(int)MenuItemsEnum.Delete].Enabled = !started;
                cutToolStripMenuItem.Enabled = m_menuItems[(int)MenuItemsEnum.Cut].Enabled = !started;

                items.Add(MenuItemsEnum.Delete);
                items.Add(MenuItemsEnum.Cut);
                items.Add(MenuItemsEnum.Copy);
                items.Add(MenuItemsEnum.Paste);
                if (data is ConfigurationTreeItemData)
                {
                    pasteToolStripMenuItem.Enabled = m_menuItems[(int)MenuItemsEnum.Paste].Enabled = (m_cd_copy != null || m_task_copy != null) && !started;
                    items.Add(MenuItemsEnum.NewTask);
                    m_menuItems[(int)MenuItemsEnum.NewTask].Enabled = !started;

                    try
                    {
                        if (String.IsNullOrEmpty(ibaAnalyzerExe))
                        {
                            RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                            object o = key.GetValue("");
                            ibaAnalyzerExe = Path.GetFullPath(o.ToString());
                        }
                    }
                    catch
                    {
                        ibaAnalyzerExe = null;
                    }

                    try
                    {
                        m_menuItems[(int)MenuItemsEnum.NewIfTask].Enabled = ibaAnalyzerExe != null && VersionCheck.CheckVersion(ibaAnalyzerExe, "5.3.4");
                    }
                    catch //version could not be determined
                    {
                        m_menuItems[(int)MenuItemsEnum.NewIfTask].Enabled = false;
                    }
                }
                else if (data is NewConfigurationTreeItemDataBase)
                    pasteToolStripMenuItem.Enabled = m_menuItems[(int)MenuItemsEnum.Paste].Enabled = false;
                else
                    pasteToolStripMenuItem.Enabled = m_menuItems[(int)MenuItemsEnum.Paste].Enabled = (m_task_copy != null) && !started;
            }
            else
            {
                items.Add(MenuItemsEnum.CollapseAll);
                m_menuItems[(int)MenuItemsEnum.Paste].Enabled = (m_cd_copy != null);
                items.Add(MenuItemsEnum.Paste);
            }
            m_popupMenu.Items.Clear();
            foreach (MenuItemsEnum item in items)
            {
                ToolStripMenuItem mc = m_menuItems[(int)item];
                mc.Tag = node;
                m_popupMenu.Items.Add(mc);
                if (item == MenuItemsEnum.NewTask)
                {
                    int index = PluginManager.Manager.PluginInfos.Count - mc.DropDown.Items.Count;
                    foreach(ToolStripMenuItem mc2 in mc.DropDown.Items)
                    {
                        if (index < 0)
                            mc2.Tag = node;
                        else
                            mc2.Tag = new Pair<TreeNode, int>(node, index);
                        index++;
                    }
                }
            }
            m_popupMenu.Show(Cursor.Position);
        }

        private void OnDeleteMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            Delete(node);
        }

        private void OnCutMenuItem(object sender, EventArgs e)
        {
            OnCopyMenuItem(sender, e);
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            if (node != null && node.Tag is ConfigurationTreeItemData && !Utility.Crypt.CheckPassword(this)) return;
            Delete(node, false);
        }

        private void OnPasteMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            pasteNode(node);
        }

        private void OnCopyMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            copyNode(node);
        }

        private void OnCollapseAllMenuItem(object sender, EventArgs e)
        {
            TreeNode node = m_configTreeView.SelectedNode;
            while (node != null && node.Parent != null) node = node.Parent;
            m_configTreeView.CollapseAll();
            m_configTreeView.SelectedNode = null;
            m_configTreeView.SelectedNode = node;
        }


        private bool TestTaskCount(ConfigurationData confData)
        {
            if (confData == null) return true;
            if (confData.Tasks.Count >= 1000)
            {
                MessageBox.Show(iba.Properties.Resources.TasksCountExceeded, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }


        private void OnNewReportMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            if (!TestTaskCount(confData))
                return;
            ReportData report = new ReportData(confData);
            new SetNextName(report);
            confData.Tasks.Add(report);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(report.Name, REPORTTASK_INDEX, REPORTTASK_INDEX);
            newNode.Tag = new ReportTreeItemData(this, report);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData); 
        }

        private void OnNewExtractMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            if (!TestTaskCount(confData))
                return;
            ExtractData extract = new ExtractData(confData);
            new SetNextName(extract);
            confData.Tasks.Add(extract);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(extract.Name, EXTRACTTASK_INDEX, EXTRACTTASK_INDEX);
            newNode.Tag = new ExtractTreeItemData(this, extract);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData); 
        }

        private void OnNewBatchfileMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            if (!TestTaskCount(confData))
                return;
            BatchFileData bat = new BatchFileData(confData);
            new SetNextName(bat);
            confData.Tasks.Add(bat);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(bat.Name, BATCHFILETASK_INDEX, BATCHFILETASK_INDEX);
            newNode.Tag = new BatchFileTreeItemData(this, bat);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData); 
        }

        private void OnNewIfTaskMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            if (!TestTaskCount(confData))
                return;
            IfTaskData condo = new IfTaskData(confData);
            new SetNextName(condo);
            confData.Tasks.Add(condo);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(condo.Name, IFTASK_INDEX, IFTASK_INDEX);
            newNode.Tag = new IfTaskTreeItemData(this, condo);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData); 
        }

        private void OnNewUpdateDataTaskMenuItem(object sender, EventArgs e)
        {
            CDongleInfo info;
            bool IsLicensed = false;
            try
            {
                info = CDongleInfo.ReadDongle();
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

            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            if (!TestTaskCount(confData))
                return;
            UpdateDataTaskData udt = new UpdateDataTaskData(confData);
            new SetNextName(udt);
            confData.Tasks.Add(udt);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(udt.Name, UPDATEDATATASK_INDEX, UPDATEDATATASK_INDEX);
            newNode.Tag = new UpdateDataTaskTreeItemData(this, udt);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData);
        }

        private void OnNewCopyTaskMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            if (!TestTaskCount(confData))
                return;
            CopyMoveTaskData cop = new CopyMoveTaskData(confData);
            new SetNextName(cop);
            confData.Tasks.Add(cop);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(cop.Name, COPYTASK_INDEX, COPYTASK_INDEX);
            newNode.Tag = new CopyTaskTreeItemData(this, cop);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData); 
        }

        private void OnNewPauseTaskMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            if (!TestTaskCount(confData))
                return;
            PauseTaskData pause = new PauseTaskData(confData);
            new SetNextName(pause);
            confData.Tasks.Add(pause);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(pause.Name, PAUSETASK_INDEX, PAUSETASK_INDEX);
            newNode.Tag = new PauseTaskTreeItemData(this, pause);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData);
        }

        private void OnNewSplitterTaskMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            if (!TestTaskCount(confData))
                return;
            SplitterTaskData Splitter = new SplitterTaskData(confData);
            new SetNextName(Splitter);
            confData.Tasks.Add(Splitter);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(Splitter.Name, SPLITTERTASK_INDEX, SPLITTERTASK_INDEX);
            newNode.Tag = new SplitterTaskTreeItemData(this, Splitter);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData);
        }

        private void OnNewCleanupTaskMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            if (!TestTaskCount(confData))
                return;
            CleanupTaskData cleanup = new CleanupTaskData(confData);
            new SetNextName(cleanup);
            confData.Tasks.Add(cleanup);
            if(Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(cleanup.Name, CLEANUPTASK_INDEX, CLEANUPTASK_INDEX);
            newNode.Tag = new CleanupTaskTreeItemData(this, cleanup);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if(confData.AdjustDependencies()) AdjustFrontIcons(confData);
        }

        private void OnNewCustomTaskMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            Pair<TreeNode, int> p = mc.Tag as Pair<TreeNode, int>;
            TreeNode node = p.First;
            int index = p.Second;
            
            PluginTaskInfo info = PluginManager.Manager.PluginInfos[index];
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            if (!TestTaskCount(confData))
                return;
            TaskData cust;
            if (info is PluginTaskInfoUNC)
                cust = new CustomTaskDataUNC(confData, info);
            else
                cust = new CustomTaskData(confData, info);

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
            confData.Tasks.Add(cust);

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(icust.Name, CUSTOMTASK_INDEX + index, CUSTOMTASK_INDEX + index);
            newNode.Tag = new CustomTaskTreeItemData(this, icust);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData);
        }

        static public int DataToRootNodeIndex(ConfigurationData data)
        {
            switch (data.JobType)
            {
                case ConfigurationData.JobTypeEnum.DatTriggered: return 0;
                case ConfigurationData.JobTypeEnum.Scheduled: return 1;
                case ConfigurationData.JobTypeEnum.Event: return 2;
                case ConfigurationData.JobTypeEnum.OneTime: return 3;
            }
            return 0;
        }

        public void AdjustFrontIcons(ConfigurationData data)
        {
            int index = DataToRootNodeIndex(data);
            foreach (TreeNode node in m_configTreeView.Nodes[index].Nodes)
            {
                ConfigurationTreeItemData ctid = (node.Tag as ConfigurationTreeItemData);
                if (ctid != null && ctid.ConfigurationData == data)
                {
                    for (int i = 0; i < data.Tasks.Count; i++)
                    {
                        if (data.Tasks[i].WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE)
                            node.Nodes[i].StateImageIndex = -1;
                    }
                }
            }
        }

        #endregion

        #region Main menu stuff
        private void configurationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_navBar.SelectedPane = m_configPane;
        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_navBar.SelectedPane = m_statusPane;
        }

        private void watchdogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_navBar.SelectedPane = m_watchdogPane;
        }

        // added by kolesnik - begin
        private void snmpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_navBar.SelectedPane = m_snmpPane;
        }
        // added by kolesnik - end

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_navBar.SelectedPane = m_settingsPane;
        }

        private void loggingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_navBar.SelectedPane = m_loggingPane;
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_configTreeView.Focused)
            {
                TreeNode node = m_configTreeView.SelectedNode;
                if (node != null && node.Tag is ConfigurationTreeItemData && !Utility.Crypt.CheckPassword(this)) return;
                copyNode(node);
                Delete(node, false);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_configTreeView.Focused)
            {
                if (!Utility.Crypt.CheckPassword(this)) return;
                TreeNode node = m_configTreeView.SelectedNode;
                copyNode(node);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_configTreeView.Focused)
            {
                if (!Utility.Crypt.CheckPassword(this)) return;
                TreeNode node = m_configTreeView.SelectedNode;
                pasteNode(node);
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_configTreeView.Focused)
            {
                TreeNode node = m_configTreeView.SelectedNode;
                if (m_configTreeView.Focused)
                    Delete(node);
            }
        }

        private string m_filename;

        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender != null && !Utility.Crypt.CheckPassword(this)) return;
            if (m_filename == null)
            {
                saveAsToolStripMenuItem_Click(null, e);
            }
            else using (WaitCursor wait = new WaitCursor())
            {
                SaveRightPaneControl();
                XmlSerializer mySerializer = null;    
                try
                {
                    mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));
                }
                catch (Exception ex)
                {
                    MessageBox.Show(iba.Properties.Resources.SaveFileProblem + " " + ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogData.Data.Logger.Log(Logging.Level.Debug, "Saving, serialization failed (step1): " + ex.ToString());
                    return;
                }


                // To write to a file, create a StreamWriter object.
                try
                {
                    using (StreamWriter myWriter = new StreamWriter(m_filename))
                    {
                        ibaDatCoordinatorData dat = ibaDatCoordinatorData.Create(TaskManager.Manager);
                        mySerializer.Serialize(myWriter, dat);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(iba.Properties.Resources.SaveFileProblem + " " + ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    LogData.Data.Logger.Log(Logging.Level.Debug, "Saving, serialization failed (step2):" + ex.ToString());
                }
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    Program.CommunicationObject.SaveConfigurations();
            }
        }

        private string TextToSave()
        {
            try
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));
                StringBuilder sb = new StringBuilder(); 
                using (StringWriter myWriter = new StringWriter(sb))
                {
                    ibaDatCoordinatorData dat = ibaDatCoordinatorData.Create(TaskManager.Manager);
                    mySerializer.Serialize(myWriter, dat);
                }
                string s = sb.ToString();
                s = s.Remove(0, s.IndexOf(Environment.NewLine));
                return s.Remove(0, s.IndexOf('<'));
            }
            catch
            {
                return String.Empty;
            }
        }

        private string TextFromLoad()
        {
            if (m_filename == null) return String.Empty;
            try
            {
                using (StreamReader reader = (new FileInfo(m_filename).OpenText()))
                {
                    reader.ReadLine(); //not interested in headerline
                    return reader.ReadToEnd();
                }
            }
            catch 
            {
                return String.Empty;
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender != null && !Utility.Crypt.CheckPassword(this)) return;
            m_saveFileDialog.CreatePrompt = false;
            m_saveFileDialog.OverwritePrompt = true;
            if (String.IsNullOrEmpty(m_filename))
                m_saveFileDialog.FileName = "myConfigurations";
            else
            {
                m_saveFileDialog.InitialDirectory = Path.GetDirectoryName(m_filename);
                m_saveFileDialog.FileName = Path.GetFileName(m_filename);
            }
            m_saveFileDialog.DefaultExt = "xml";
	    	m_saveFileDialog.Filter = "XML files (*.xml)|*.xml";
            if (m_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                m_filename = m_saveFileDialog.FileName;
                this.Text = m_filename + " - ibaDatCoordinator v" + DatCoVersion.GetVersion();
                saveToolStripMenuItem_Click(null, e);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword(this)) return;
            m_openFileDialog.CheckFileExists = true;
            m_openFileDialog.Filter = "XML files (*.xml)|*.xml";
            m_openFileDialog.FileName = "";
            DialogResult result = m_openFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;
                
            SaveRightPaneControl();
            loadFromFile(m_openFileDialog.FileName,false);
            loadConfigurations();
            loadStatuses();
            m_navBar.SelectedPane = m_configPane;
            ReloadRightPane();
        }

        private bool loadFromFile(string filename, bool beSilent)
        {
            try
            {
                using (WaitCursor wait = new WaitCursor())
                {
                    //XmlSerializer mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));

                    //using (FileStream myFileStream = new FileStream(filename, FileMode.Open))
                    //{
                    //    ibaDatCoordinatorData dat = ibaDatCoordinatorData.SerializeFromStream(mySerializer, myFileStream);
                    //    confs = dat.ApplyToManager(TaskManager.Manager, Program.ClientName);
                    //}
                    ibaDatCoordinatorData data = ibaDatCoordinatorData.SerializeFromFile(filename);

                    List<string> missingPlugins = TaskManager.Manager.CheckPluginsAvailable(data.PluginList());
                    if (missingPlugins != null && missingPlugins.Count > 0)
                    {
                        MessageBox.Show(iba.Properties.Resources.uploadFileProblem + " "  + iba.Properties.Resources.missingPlugins + Environment.NewLine + String.Join(Environment.NewLine, PluginManager.Manager.FullNames(missingPlugins))
                            , "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }


                    List<ConfigurationData> confs = data.ApplyToManager(TaskManager.Manager, Program.ClientName); ;

                    m_filename = filename;
                    this.Text = m_filename + " - ibaDatCoordinator v" + DatCoVersion.GetVersion();
                    foreach (ConfigurationData dat in confs)
                    {
                        dat.relinkChildData();
                        dat.UpdateUNC(true);
                    }

                    if (TaskManager.Manager.Count > 0)
                    {
                        using (StopWaitDialog waiter = new StopWaitDialog())
                        {
                            waiter.ShowDialog(this);
                        }
                    }
                    try
                    {
                        TaskManager.Manager.Configurations = confs;
                    }
                    catch (Exception serEx) //likely serialization exception
                    {
                        MessageBox.Show(iba.Properties.Resources.uploadFileProblem + " " + serEx.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //remoting will likely be in an inconsistent state, reconnect
                        Reconnect();

                        return false;
                    }
                    if (Program.RunsWithService != Program.ServiceEnum.DISCONNECTED)
                    {
                        TaskManager.Manager.StartAllEnabledGlobalCleanups();
                        foreach (ConfigurationData dat in confs)
                        {
                            if (dat.AutoStart && dat.Enabled && dat.JobType != ConfigurationData.JobTypeEnum.OneTime) TaskManager.Manager.StartConfiguration(dat);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.InnerException != null && ex.InnerException is ApplicationException)
                    ex = ex.InnerException;
                if (!beSilent) MessageBox.Show(iba.Properties.Resources.OpenFileProblem + "  " + ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword(this)) return;
            SaveRightPaneControl();
            string s1 = TextFromLoad();
            string s2 = TextToSave();
            if (!string.IsNullOrEmpty(s2) && s1 != s2)
            {
                DialogResult res = MessageBox.Show(this, iba.Properties.Resources.saveQuestion,
                        iba.Properties.Resources.closing, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                switch (res)
                {
                    case DialogResult.Cancel:
                        return;
                    case DialogResult.Yes:
                        saveToolStripMenuItem_Click(null, null);
                        break;
                    case DialogResult.No:
                        break;
                }
            }
            m_filename = null;
            clearAllConfigurations();
            this.Text = "ibaDatCoordinator v" + DatCoVersion.GetVersion();
            m_navBar.SelectedPane = m_configPane;
        }

        internal struct PreviousMenuItems
        {
            public bool p, c, t, d;
        }

        PreviousMenuItems m_previousMenuItems;

        private void m_rightPane_Leave(object sender, EventArgs e)
        {
            pasteToolStripMenuItem.Enabled = m_previousMenuItems.p;
            copyToolStripMenuItem.Enabled = m_previousMenuItems.c;
            cutToolStripMenuItem.Enabled = m_previousMenuItems.t;
            deleteToolStripMenuItem.Enabled = m_previousMenuItems.d;
        }

        public void m_rightPane_Enter(object sender, EventArgs e)
        {
            m_previousMenuItems.p = pasteToolStripMenuItem.Enabled;
            m_previousMenuItems.c = copyToolStripMenuItem.Enabled;
            m_previousMenuItems.t = cutToolStripMenuItem.Enabled;
            m_previousMenuItems.d = deleteToolStripMenuItem.Enabled;
            pasteToolStripMenuItem.Enabled = false;
            copyToolStripMenuItem.Enabled = false;
            cutToolStripMenuItem.Enabled = false;
            deleteToolStripMenuItem.Enabled = false;
        }

        #region Help menu

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (About dlg = new About())
            {
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
            }
        }

        private void saveInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword(this)) return;
            if (!String.IsNullOrEmpty(m_filename)) saveToolStripMenuItem_Click(null, null);
            new SupportFileGenerator(this, m_filename);
        }


        private void VersionHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string readmeFile = "";
            try
            {
               readmeFile = Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), "versions_dat.htm");
               System.Diagnostics.Process.Start(readmeFile);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message + " " + readmeFile);
            }
        }

        //private void licenseAgreementToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    string readmeFile = "";
        //    try
        //    {
        //        readmeFile = Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), "License_Agreement_DatCoordinator.pdf");
        //        System.Diagnostics.Process.Start(readmeFile);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + " " + readmeFile);
        //    };
        //}
    

    #endregion

    #endregion

    #region IPropertyPaneManager Members

    private Hashtable propertyPanes = new Hashtable();
        public Hashtable PropertyPanes
        {
            get { return propertyPanes; }
        }

        public TreeView LeftTree
        {
            get
            {
                if (m_navBar.SelectedPane == m_configPane)
                    return m_configTreeView;
                else if (m_navBar.SelectedPane == m_statusPane)
                    return m_statusTreeView;
                else
                    return null;
            }
        }

        public TreeView getLeftTree(string name)
        {
            if (name == "configuration")
                return m_configTreeView;
            else if (name == "status")
                return m_statusTreeView;
            else return null;
        }


        public IPropertyPane CurrentPane
        {
            get
            {
                if (m_rightPane.Controls.Count > 0)
                    return m_rightPane.Controls[0] as IPropertyPane;
                else
                    return null;
            }
        }

        public void AdjustRightPaneControlTitle()
        {
            TreeNode node = m_configTreeView.SelectedNode;
            string title = node.Text;
            while (node.Parent != null)
            {
                node = node.Parent;
                title = node.Text + " - " + title;
            }
            title = m_navBar.SelectedPane.Text + ": " + title;
            m_rightPane.Text = title;
        }
        #endregion

        bool m_reclaimFocus;
        public bool ReclaimFocus
        {
            get { return m_reclaimFocus; }
            set {
                m_reclaimFocus = value; 
            }
        }
        
        private void MainForm_Deactivate(object sender, EventArgs e)
        {
            if (m_reclaimFocus)
            {
                Activate();
                Focus();
                m_reclaimFocus = false;
            }
        }

        private void m_EntriesNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            LogData.Data.MaxRows = Math.Max(1,(int) m_EntriesNumericUpDown1.Value);
        }

        private void m_rbLogLevelRbCheckedChanged(object sender, EventArgs e)
        {
            //loglevel 0 = all, 1 = warnings,errors, 2 = only errors
            int loglevel = 0;
            if (m_rbErrorsWarnings.Checked) loglevel = 1;
            else if (m_rbOnlyErrors.Checked) loglevel = 2;
            LogData.Data.LogLevel = loglevel;
        }

        private void m_btnClearLogging_Click(object sender, EventArgs e)
        {
            LogData.Data.ClearGrid();
        }

        private void m_stopButton_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword(this)) return;
            SaveRightPaneControl();
            using (StopWaitDialog waiter = new StopWaitDialog())
            {
                waiter.ShowDialog(this);
            }
            if (m_configTreeView.SelectedNode != null && m_configTreeView.SelectedNode.Tag is ConfigurationTreeItemData && m_rightPane.Controls.Count>0)
            {
                ConfigurationControl ctrl = (m_rightPane.Controls[0] as ConfigurationControl);
                if (ctrl != null)
                    ctrl.LoadData((m_configTreeView.SelectedNode.Tag as ConfigurationTreeItemData).ConfigurationData, this);
            }
            UpdateButtons();
        }

        private void m_startButton_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword(this))
                return;
            SaveRightPaneControl();
            if (!String.IsNullOrEmpty(m_filename)) saveToolStripMenuItem_Click(null, null);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                Program.CommunicationObject.SaveConfigurations();

            StatusBarLabelErrors.Text = ""; //clear any errors on restart
            TaskManager.Manager.StartAllEnabledConfigurationsNoOneTime();
            if (m_configTreeView.SelectedNode != null && m_configTreeView.SelectedNode.Tag is ConfigurationTreeItemData)
            {
                if (m_rightPane.Controls.Count >= 1 && m_rightPane.Controls[0] != null && m_rightPane.Controls[0] is ConfigurationControl)
                    (m_rightPane.Controls[0] as ConfigurationControl).LoadData((m_configTreeView.SelectedNode.Tag as ConfigurationTreeItemData).ConfigurationData,this);
            }
            UpdateButtons();
            SwitchToStatusPane();
        }

        public void SwitchToStatusPane()
        {
            m_navBar.SelectedPane = m_statusPane;
        }

        public void UpdateButtons()
        {
            if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                this.Icon = iba.Properties.Resources.disconnectedIcon;
                m_startButton.Enabled = m_stopButton.Enabled = false;
                return;
            }
            bool allEnabledStarted = true;
            bool allStopped = true;
            
            foreach (ConfigurationData data in TaskManager.Manager.Configurations)
            {
                bool started = TaskManager.Manager.IsJobStarted(data.Guid);
                if (data.Enabled && !(data.OnetimeJob) )
                {
                    allEnabledStarted = allEnabledStarted && started;
                }
                allStopped = allStopped && !started;
            }
            m_startButton.Enabled = !allEnabledStarted;
            m_stopButton.Enabled = !allStopped;

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
            {
                if (!allStopped)
                {
                    this.Icon = iba.Properties.Resources.runningIcon;
                }
                else
                {
                    this.Icon = iba.Properties.Resources.connectedIcon;
                }
            }
        }

        public void SetRenderer()
        {
            TD.Eyefinder.Rendering.Office2007Renderer renderer = (TD.Eyefinder.Rendering.Office2007Renderer)m_navBar.Renderer;
            if (Program.RunsWithService != Program.ServiceEnum.DISCONNECTED)
            {
                renderer.HeaderBackgroundColor1 = Color.FromArgb(255, 89, 135, 214);
                renderer.HeaderBackgroundColor2 = Color.FromArgb(255, 0, 45, 150);
                renderer.HeaderTextColor = Color.White;
                m_rightPane.SetActiveRenderer(renderer);
                m_navBar.Invalidate();
                m_rightPane.Invalidate();
            }
            else
            {
                renderer.HeaderBackgroundColor1 = Color.MistyRose;
                renderer.HeaderBackgroundColor2 = Color.Crimson;
                renderer.HeaderTextColor = Color.White;
                m_rightPane.SetActiveRenderer(renderer);
                m_navBar.Invalidate();
                m_rightPane.Invalidate();
            }
        }

        public Button StartButton
        {
            get { return m_startButton; }
        }

        public Button StopButton
        {
            get { return m_stopButton; }
        }

        static public void strikeOutNodeText(TreeNode node, bool cross)
        {
            if (cross)
            {
                Font font = node.NodeFont;
                if (font == null)
                    if (node.TreeView != null)
                        font = node.TreeView.Font;
                    else
                    {
                        using (TreeView dummytree = new TreeView())
                        {
                            font = dummytree.Font;
                        }
                    }
                node.NodeFont = new Font(font,FontStyle.Strikeout);
                node.ForeColor = Color.Gray;
            }
            else
            {
                node.NodeFont = null;
                node.ForeColor = Color.Black;
            }
        }

        public void UpdateTreeNode(ConfigurationData olddata, ConfigurationData newdata)
        { //called when undo changes is clicked
            TreeNode node = m_configTreeView.SelectedNode;
            if (node == null) return;
            ConfigurationTreeItemData dat = node.Tag as ConfigurationTreeItemData;
            m_configTreeView.SelectedNode = null;
            if (dat == null || dat.ConfigurationData.Guid != olddata.Guid) return;
            TreeNode replacingNode = CreateConfigurationNode(newdata);
            int index = node.Index;
            m_configTreeView.Nodes[DataToRootNodeIndex(newdata)].Nodes.Remove(node);
            m_configTreeView.Nodes[DataToRootNodeIndex(newdata)].Nodes.Insert(index, replacingNode);
            m_configTreeView.SelectedNode = replacingNode;
            AdjustRightPaneControlTitle();
            m_configTreeView.Update();
        }

        
        public ToolStripStatusLabel StatusBarLabelErrors
        {
            get {return m_statusBarStripLabel;}
            set { m_statusBarStripLabel = value; }
        }

        #region ConfigurationTree DragDrop

        private void m_configTreeView_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.None) return;
            // Retrieve the client coordinates of the drop location.

            Point targetPoint = m_configTreeView.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = m_configTreeView.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (!Utility.Crypt.CheckPassword(this)) return;

            if (draggedNode.Tag is ConfigurationTreeItemData && e.Effect == DragDropEffects.Move)
            {
                int rootIndex1 = DataToRootNodeIndex((draggedNode.Tag as ConfigurationTreeItemData ).ConfigurationData);
                int rootIndex2 = targetNode.Parent==null?targetNode.Index:
                    DataToRootNodeIndex((draggedNode.Tag as ConfigurationTreeItemData ).ConfigurationData);
                if(rootIndex1 != rootIndex2) return;
                //just move the nodes, no actual change
                m_configTreeView.SelectedNode = null;
                int index = targetNode.Index;
                m_configTreeView.BeginUpdate();
                m_configTreeView.Nodes[rootIndex1].Nodes.Remove(draggedNode);
                m_configTreeView.Nodes[rootIndex1].Nodes.Insert(index, draggedNode);
                m_configTreeView.EndUpdate();
                m_configTreeView.SelectedNode = m_configTreeView.Nodes[rootIndex1].Nodes[index];
                UpdateTreePositions();
            }
            else
            {
                if (e.Effect == DragDropEffects.Move && !(targetNode.Tag is ConfigurationTreeItemData))
                {
                    bool draggedIsLower = false;
                    if (targetNode.Parent == draggedNode.Parent)
                        draggedIsLower = draggedNode.Index < targetNode.Index;
                    else
                        draggedIsLower = draggedNode.Parent.Index < targetNode.Parent.Index;
                    if (draggedIsLower)
                    {
                        TreeNode nextNode = targetNode.NextNode;
                        if (nextNode == null) nextNode = targetNode.Parent;
                        targetNode = nextNode;
                    }
                }
                TaskData temptask = m_task_copy;
                bool tempbool = m_confCopiedMostRecent;
                copyNode(draggedNode);
                if (e.Effect == DragDropEffects.Move)
                    Delete(draggedNode, false);
                pasteNode(targetNode);
                m_task_copy = temptask;
                m_confCopiedMostRecent = tempbool;
            }
        }

        private void m_configTreeView_DragOver(object sender, DragEventArgs e)
        {
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = m_configTreeView.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = m_configTreeView.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (draggedNode == null) return;
            if(targetNode == null)
            {
                if(!(draggedNode.Tag is ConfigurationTreeItemData))
                    e.Effect = DragDropEffects.None;
                else
                    e.Effect = DragDropEffects.All;
            }
            else if(targetNode.Tag == null && !(draggedNode.Tag is ConfigurationTreeItemData))
            {
                e.Effect = DragDropEffects.None;
            }
            else if(targetNode.Tag == null)
            {
                if(DataToRootNodeIndex((draggedNode.Tag as ConfigurationTreeItemData).ConfigurationData) == targetNode.Index)
                {
                    if((ModifierKeys & Keys.Control) == Keys.Control)
                        e.Effect = DragDropEffects.Copy;
                    else e.Effect = DragDropEffects.Copy | DragDropEffects.Move;
                }
            }
            else if ((draggedNode.Tag is ConfigurationTreeItemData) && !(targetNode.Tag is ConfigurationTreeItemData))
            {
                e.Effect = DragDropEffects.None;
            }
            else if (targetNode.Tag is NewConfigurationTreeItemDataBase)
            {
                e.Effect = DragDropEffects.None;
            }
            else if ((targetNode == draggedNode) && (draggedNode.Tag is ConfigurationTreeItemData))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if ((targetNode == draggedNode) && !(draggedNode.Tag is ConfigurationTreeItemData))
            {
                if (TaskManager.Manager.IsJobStarted((draggedNode.Parent.Tag as ConfigurationTreeItemData).ConfigurationData.Guid))
                    e.Effect = DragDropEffects.None; //do not modify a running configuration    
                else e.Effect = DragDropEffects.Copy;
            }
            else if (!(draggedNode.Tag is ConfigurationTreeItemData))
            {
                bool moveAllowed = !TaskManager.Manager.IsJobStarted((draggedNode.Parent.Tag as ConfigurationTreeItemData).ConfigurationData.Guid);
                bool placeAllowed = false;

                if (targetNode.Tag is ConfigurationTreeItemData)
                    placeAllowed = !TaskManager.Manager.IsJobStarted((targetNode.Tag as ConfigurationTreeItemData).ConfigurationData.Guid);
                else
                    placeAllowed = !TaskManager.Manager.IsJobStarted((targetNode.Parent.Tag as ConfigurationTreeItemData).ConfigurationData.Guid);

                if (!placeAllowed)
                    e.Effect = DragDropEffects.None;
                else if (!moveAllowed)
                    e.Effect = DragDropEffects.Copy;
                else if ((ModifierKeys & Keys.Control) == Keys.Control)
                    e.Effect = DragDropEffects.Copy;
                else e.Effect = DragDropEffects.Move;
            }
            else
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control)
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.Move;
            }
        }

        private void m_configTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Move the dragged node when the left mouse button is used.
            if (e.Button == MouseButtons.Left)
            {
                TreeNode draggedNode = (TreeNode) e.Item;
                m_configTreeView.SelectedNode = draggedNode;
                if (!(draggedNode.Tag is NewConfigurationTreeItemDataBase) || draggedNode.Tag == null)
                    DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }
        #endregion


        private void UpdateTreePositions()
        {
            int suffix = 0;
            foreach(TreeNode parentNode in m_configTreeView.Nodes)
            {
                suffix += 1000;
                foreach(TreeNode node in parentNode.Nodes)
                {
                    ConfigurationTreeItemData dat = (node.Tag as ConfigurationTreeItemData);
                    if(dat != null)
                    {
                        dat.ConfigurationData.TreePosition = suffix + node.Index;
                        if(Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                        {
                            TaskManager.Manager.UpdateTreePosition(dat.ConfigurationData.Guid, dat.ConfigurationData.TreePosition);
                        }
                    }
                }
            }
        }

        public delegate void IbaAnalyzerCall();

        private void miConnectServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OnConnectService();
        }

        private void Reconnect()
        {
            CommunicationObjectWrapper old = Program.CommunicationObject;
            if (old != null)
            {
                //Remove event forwarder
                if (m_ef != null)
                {
                    old.Logging_clearEventForwarder(m_ef.Guid);
                    m_ef.Dispose();
                    m_ef = null;
                }

                //Disconnect
                old.HandleBrokenConnection(new Exception("Forced disconnect because connecting to other server"));
            }
            Program.CommunicationObject = null; //will kill it
            Program.RunsWithService = Program.ServiceEnum.DISCONNECTED;
            UpdateConnectionStatus();

            TryToConnect(true);
        }

        private void OnConnectService()
        {
            SaveRightPaneControl();
            int port = Program.ServicePortNr;
            string server = Program.ServiceHost;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                AskToSaveConnection();
            ServerConfiguration cf = new ServerConfiguration();
            cf.Address = server;
            cf.PortNr = port;
            using (ServerSelectionForm ssf = new ServerSelectionForm(cf))
            {
                DialogResult r = ssf.ShowDialog();
                if (r == DialogResult.OK /*&& (port != cf.PortNr || server != cf.Address)*/)
                {
                    m_suppresUpload = true;
                    Program.ServicePortNr = cf.PortNr;
                    Program.ServiceHost = cf.Address;

                    Reconnect();

                }
            }
        }

        private void AskToSaveConnection()
        {
            if (IsServerClientDifference() &&
                MessageBox.Show(this,
                    iba.Properties.Resources.AskToUploadToCurrentServer, 
                    "ibaDatCoordinator", 
                     MessageBoxButtons.YesNo, MessageBoxIcon.Question, 
                     MessageBoxDefaultButton.Button1)
                     ==DialogResult.Yes)
            {
                for (int index = 0; index < 3; index++)
                {
                    foreach (TreeNode t in m_configTreeView.Nodes[index].Nodes)
                    {
                        if (t.Tag is ConfigurationTreeItemData)
                        {
                            ConfigurationData data = (t.Tag as ConfigurationTreeItemData).ConfigurationData;
                            if (!TaskManager.Manager.CompareConfiguration(data))
                            {
                                TaskManager.Manager.UpdateConfiguration(data);
                            }
                        }
                    }
                }
            }
        }

        private bool m_suppresUpload = false;
        private bool m_firstConnectToService;
        private int m_lastStopID = -1;
        private int m_lastTaskManagerID = -1;
        private EventForwarder m_ef;

        void OnConnectTimer(object ignoreMe)
        {
            TryToConnect(false);
        }

        public void TryToConnect(bool bInteractive)
        {
            if (m_updateClientTimer != null)
                m_updateClientTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            if (m_tryConnectTimer != null)
                m_tryConnectTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            bool resetUpdateTimer = false;
            try
            {
                if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
                {
                    CommunicationObject com = (CommunicationObject)Activator.GetObject(typeof(CommunicationObject), Program.CommObjectString);
                    CommunicationObjectWrapper wrapper = new CommunicationObjectWrapper(com);

                    try
                    {

                        //Let's try connecting, this will throw in case the connection fails
                        int serverVersion = wrapper.Connect();

                        if (PluginManager.Manager.PluginActionsOnConnect(wrapper))
                        {
                            Application.Restart(); //should close ourselves
                            return;
                        }
                        //We are connected!
                        if (m_tryConnectTimer != null) //this is not the first call, restore stuff
                        {
                            MethodInvoker m = delegate ()
                            {
                                Program.RunsWithService = Program.ServiceEnum.CONNECTED;
                                ibaDatCoordinatorData data = null;
                                if (TaskManager.ClientManager != null)
                                {
                                    data = ibaDatCoordinatorData.Create(TaskManager.ClientManager);
                                }
                                TaskManager.ClientManager = null; //remove previous client taskmanager so it does not stay
                                // alive during the online session
                                Program.CommunicationObject = wrapper;
                                if (NeedUploadToServer())
                                {
                                    //initialise with configurations;
                                    SaveRightPaneControl();
                                    List<string> missingPlugins = TaskManager.Manager.CheckPluginsAvailable(data.PluginList());
                                    if (missingPlugins != null && missingPlugins.Count > 0)
                                    {
                                        MessageBox.Show(iba.Properties.Resources.uploadFileProblem + " " + iba.Properties.Resources.missingPlugins + Environment.NewLine + String.Join(Environment.NewLine, missingPlugins)
                                            , "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                        ReloadClient();
                                    }
                                    else
                                    {
                                        if (data != null) data.ApplyToManager(TaskManager.Manager, Program.ClientName);
                                        ReplaceManagerFromTree(TaskManager.Manager);
                                        TaskManager.Manager.StartAllEnabledGlobalCleanups();
                                        foreach (ConfigurationData dat in TaskManager.Manager.Configurations)
                                        {
                                            if (dat.AutoStart && dat.Enabled && dat.JobType != ConfigurationData.JobTypeEnum.OneTime) TaskManager.Manager.StartConfiguration(dat);
                                        }
                                        if (m_navBar.SelectedPane == m_statusPane)
                                            loadStatuses();
                                        else if (m_navBar.SelectedPane == m_configPane)
                                            loadConfigurations();
                                        ReloadRightPane();
                                        UpdateButtons();
                                    }
                                }
                                else //download
                                {
                                    ReloadClient();
                                }

                            };

                            this.SafeInvoke(m, true);
                        }
                        else
                        {
                            Program.RunsWithService = Program.ServiceEnum.CONNECTED;
                            TaskManager.Manager = null; //remove previous client taskmanager so it does not stay
                            // alive during the online session
                            Program.CommunicationObject = wrapper;
                        }

                        if (m_ef != null) //clear any previous attempt
                        {
                            Program.CommunicationObject.Logging_clearEventForwarder(m_ef.Guid);
                            m_ef.Dispose();
                        }

                        LogData.Data.ClearGrid();

                        m_ef = new EventForwarder();
                        Program.CommunicationObject.Logging_setEventForwarder(m_ef, m_ef.Guid);

                        m_firstConnectToService = false;
                        MethodInvoker m2 = delegate ()
                        {
                            SetRenderer();
                            UpdateConnectionStatus();
                        };
                        this.SafeInvoke(m2, true);
                        resetUpdateTimer = true;
                    }
                    catch (Exception connEx)
                    {
                        Program.RunsWithService = Program.ServiceEnum.DISCONNECTED;
                        LogData.Data.Logger.Log(Logging.Level.Debug, String.Format("Failed to connect to {0} with error {1}", Program.CommObjectString, connEx.Message));
                        if (bInteractive)
                            MessageBox.Show(connEx.Message, String.Format(Properties.Resources.ErrorServerConnect, Program.ServiceHost), MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                {
                    try
                    {
                        if (!Program.CommunicationObject.TestConnection())
                        {
                            MethodInvoker m2 = delegate ()
                            {
                                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(new Exception("testconnection failed"));
                            };
                            this.SafeInvoke(m2, true);
                        }
                    }
                    catch { }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }

            if (m_tryConnectTimer == null)
                m_tryConnectTimer = new System.Threading.Timer(OnConnectTimer);
            m_tryConnectTimer.Change(TimeSpan.FromSeconds(5.0), TimeSpan.Zero);

            if (resetUpdateTimer && !updateTimerBusy) //if update is busy, it will set its own timer ...
            {
                if (m_updateClientTimer == null)
                    m_updateClientTimer = new System.Threading.Timer(UpdateClientTimerTick);
                m_updateClientTimer.Change(TimeSpan.FromSeconds(5.0), TimeSpan.Zero);
                m_lastStopID = -1;
                m_lastTaskManagerID = -1;
            }
            else if (!updateTimerBusy)
            {
                if (m_updateClientTimer == null)
                    m_updateClientTimer = new System.Threading.Timer(UpdateClientTimerTick);
                m_updateClientTimer.Change(TimeSpan.FromSeconds(1.0), TimeSpan.Zero);
            }//timer still needs to be set...
        }

        public delegate void InvokeHandler();

        public void UpdateConnectionStatus()
        {
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                m_statusBarStripLabelConnection.Text = string.Format(iba.Properties.Resources.ConnectedTo, Program.ServiceHost);
            else if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
                m_statusBarStripLabelConnection.Text = string.Format(iba.Properties.Resources.Disconnected, Program.ServiceHost);
            else
                m_statusBarStripLabelConnection.Text = Properties.Resources.StandaloneText;
        }

        private void ReloadClient()
        {
            var prevPane = m_navBar.SelectedPane;
            loadConfigurations();
            loadStatuses();
            if (prevPane != m_settingsPane)
                ReloadRightPane();

            //Force update
            m_navBar.SelectedPane = null;
            m_navBar.SelectedPane = prevPane;
            UpdateButtons();
        }

        private bool updateTimerBusy;

        private void UpdateClientTimerTick(object state)
        {
            if (m_updateClientTimer != null)
                m_updateClientTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            if (Program.RunsWithService != Program.ServiceEnum.CONNECTED)
            { //timer, will be enabled again on reconnnect
                return;
            }
            updateTimerBusy = true;
            if (m_lastTaskManagerID == -1) m_lastTaskManagerID = TaskManager.Manager.TaskManagerID;
            if (m_lastStopID == -1) m_lastStopID = TaskManager.Manager.ConfStoppedID;

            try
            {
                bool doStartButtons = false;
                if (TaskManager.Manager.TaskManagerID != m_lastTaskManagerID)
                {
                    m_lastTaskManagerID = TaskManager.Manager.TaskManagerID;

                    if (IsServerClientDifference())
                    {
                        //do download or kill.
                        MethodInvoker m = delegate ()
                        {
                            if (MessageBox.Show(this,
                                iba.Properties.Resources.AskSaveLocal,
                                "ibaDatCoordinator",
                                 MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                                 MessageBoxDefaultButton.Button1)
                                == DialogResult.Yes)
                            {
                                saveAsToolStripMenuItem_Click(null, null);
                            }
                            ReloadClient();
                        };
                        this.SafeInvoke(m, true);
                    }
                    else
                    {
                        doStartButtons = true;
                    }
                }

                if (doStartButtons || (m_lastStopID != TaskManager.Manager.ConfStoppedID))
                {
                    MethodInvoker m = delegate ()
                    {
                        UpdateButtons();
                        if (m_configPane == m_navBar.SelectedPane)
                        {
                            ConfigurationControl c = (m_rightPane.Controls.Count > 0) ? (m_rightPane.Controls[0] as ConfigurationControl) : null;
                            if (c != null)
                            {
                                c.UpdateEnabledState();
                            }
                        }
                    };
                    this.SafeInvoke(m,true);
                    m_lastStopID = TaskManager.Manager.ConfStoppedID;
                }
                if (m_updateClientTimer == null)
                    m_updateClientTimer = new System.Threading.Timer(UpdateClientTimerTick);
                m_updateClientTimer.Change(TimeSpan.FromSeconds(1.0), TimeSpan.Zero); //every second now ...
            }
            catch(Exception)
            {
                if (IsDisposed)
                    return;
                throw;
            }
            finally
            {
                updateTimerBusy = false;
            }
        }

        private bool NeedUploadToServer()
        {
            if (m_suppresUpload)
            {
                m_suppresUpload = false; //reset the flag
                return false;
            }
            if (TaskManager.Manager.Count == 0) return true; //nothing on server side, upload the minimum configuration of one
            if (m_firstConnectToService) return false;
            //test if there's a difference between the client and server configurations
            if (IsServerClientDifference())
            {
                using (UploadOrDownloadConfigurationsDialog uodDiag = new UploadOrDownloadConfigurationsDialog())
                {
                    if (WindowState == FormWindowState.Minimized)
                    {
                        uodDiag.StartPosition = FormStartPosition.CenterScreen;
                        uodDiag.ShowDialog();
                    }
                    else
                    {
                        uodDiag.StartPosition = FormStartPosition.CenterParent;
                        uodDiag.ShowDialog(this);
                    }
                    return uodDiag.Upload;
                }
            }
            return false;
        }

        private bool IsServerClientDifference()
        {
            int count = 0;
            for (int index = 0; index < 3; index++)
            {
                foreach (TreeNode t in m_configTreeView.Nodes[index].Nodes)
                {
                    if (t.Tag is ConfigurationTreeItemData)
                    {
                        count++;
                        ConfigurationData data = (t.Tag as ConfigurationTreeItemData).ConfigurationData;
                        if (!TaskManager.Manager.CompareConfiguration(data))
                        {
                            return true;
                        }
                    }
                }
            }
            if (TaskManager.Manager.Count != count) return true; //added or removed
            return false;
        }

        private System.Threading.Timer m_tryConnectTimer;
        private System.Threading.Timer m_updateClientTimer;

        public void ReplaceManagerFromTree(TaskManager m)
        {
            List<ConfigurationData> toReplace = new List<ConfigurationData>();
            for (int index = 0; index < 3; index++)
            {
                foreach (TreeNode t in m_configTreeView.Nodes[index].Nodes)
                {
                    if (t.Tag is ConfigurationTreeItemData)
                        toReplace.Add((t.Tag as ConfigurationTreeItemData).ConfigurationData);
                }
            }
            TaskManager.Manager.ReplaceConfigurations(toReplace);
        }

        #region Online Help
        private HelpProvider helpProvider;

        private void SetupHelp()
        {
            string helpFile = "";
            string culture = System.Threading.Thread.CurrentThread.CurrentUICulture.Name;
            if(culture.Length >= 2)
                culture = culture.Substring(0, 2);
            helpFile = Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), "ibaDatCo_" + culture + ".chm");
            if(!System.IO.File.Exists(helpFile))
                helpFile = Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), "ibaDatCo.chm");

            if(helpProvider != null)
            {
                helpProvider.ResetShowHelp(this);
                helpProvider.Dispose();
                helpProvider = null;
            }

            if(System.IO.File.Exists(helpFile))
            {
                helpProvider = new HelpProvider();
                helpProvider.HelpNamespace = helpFile;
                helpProvider.SetShowHelp(this, true);
                helpProvider.SetHelpNavigator(this, HelpNavigator.Topic);
                helpProvider.SetHelpKeyword(this, "14989.htm");

                //helpProvider.SetShowHelp(recorderContainer, true);
                //helpProvider.SetHelpNavigator(recorderContainer, HelpNavigator.Topic);
                //helpProvider.SetHelpKeyword(recorderContainer, "605.htm");
            }
            else
            {
                helpProvider = null;
            }

            helpMenuItem.Enabled = helpProvider != null;
        }



        private void OnHelpClick(object sender, EventArgs e)
        {
            using (WaitCursor wait = new WaitCursor())
            {
                try
                {
                    Help.ShowHelp(this, helpProvider.HelpNamespace, HelpNavigator.Topic, "10180.htm");
                }
                catch (Exception)
                {
                }
            }
        }

        public void OnExternalActivate()
        {
            Show();
            Activate();
            WindowState = FormWindowState.Normal;
            FormStateSerializer.LoadSettings(this, "MainForm", true);
        }

        public void OnExternalClose()
        {
            Close();
        }

        public void OnStartService()
        {

        }


        #endregion

        #region Plugins

        public void UpdatePluginGUIElements()
        {
            //throw new NotImplementedException();
            //update imagelist of configuration tree
            m_cd_copy = null;
            m_task_copy = null; //no old copy pastes...
            UpdateImageListConfTree();
            CreateMenuItems(); //recreate menu items
            foreach(var pane in PropertyPanes.Values)
            {
                (pane as IPluginsUpdatable)?.UpdatePlugins();
            }
        }

        private void UpdateImageListConfTree()
        {
            ImageList confsImageList = m_configTreeView.ImageList;

            //remove old plugins
            while (confsImageList.Images.Count > CUSTOMTASK_INDEX)
                confsImageList.Images.RemoveAt(CUSTOMTASK_INDEX);

            foreach (PluginTaskInfo info in PluginManager.Manager.PluginInfos)
            {
                confsImageList.Images.Add(info.Icon);
            }
        }

        #endregion

        #region Language

        private void CreateLanguageMenuItems()
        {
            //Add system
            ToolStripMenuItem mi = new ToolStripMenuItem(iba.Properties.Resources.LangSystem, null, OnChangeLanguage);
            mi.Name = "do not translate";
            mi.Tag = null;
            languageToolStripMenuItem.DropDownItems.Add(mi);


            IList<CultureInfo> supportedCultures = new List<CultureInfo>();
            //hardcoded languages, datco has never been translated in 12  years to other languages so unlikely they ever will in the future
            supportedCultures.Add(CultureInfo.GetCultureInfo("de"));
            supportedCultures.Add(CultureInfo.GetCultureInfo("en"));
            supportedCultures.Add(CultureInfo.GetCultureInfo("fr"));

            foreach (var supportedCulture in supportedCultures)
            {
                mi = new ToolStripMenuItem(supportedCulture.NativeName, null, OnChangeLanguage)
                {
                    Name = "do not translate",
                    Tag = supportedCulture
                };
                languageToolStripMenuItem.DropDownItems.Add(mi);
            }
        }

        private void OnChangeLanguage(object sender, EventArgs e)
        {
            var item = sender as ToolStripMenuItem;
            if (item == null)
                return;

            var culture = item.Tag as System.Globalization.CultureInfo;
            if (culture == null)
            {
                LanguageHelper.SelectedLanguage = "";
                culture = System.Globalization.CultureInfo.InstalledUICulture;
            }
            else
            {
                LanguageHelper.SelectedLanguage = culture.Name;
            }

            Profiler.ProfileString(false, "Client", "Language", ref LanguageHelper.SelectedLanguage, "");
            if (culture.Name != System.Globalization.CultureInfo.CurrentUICulture.Name)
            {
                string message;
                if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
                    message = iba.Properties.Resources.SwitchLanguageRestartRequiredStandAlone;
                else
                    message = iba.Properties.Resources.SwitchLanguageRestartRequired;
                if (MessageBox.Show(this, message, languageToolStripMenuItem.Text, MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning, MessageBoxDefaultButton.Button1) == DialogResult.Yes)
                {
                    Application.Restart(); //should close ourselves
                    return;
                }
            }
        }


        private void languageToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            string selectedLang = LanguageHelper.SelectedLanguage;
            bool bFound = false;
            foreach (ToolStripMenuItem item in languageToolStripMenuItem.DropDownItems)
            {
                System.Globalization.CultureInfo culture = (System.Globalization.CultureInfo)item.Tag;
                if (culture != null)
                    item.Checked = selectedLang == culture.Name;
                else
                    item.Checked = selectedLang == "";

                if (item.Checked)
                    bFound = true;
            }

            if (!bFound)
            {
                //Add item for current culture
                System.Globalization.CultureInfo culture = System.Globalization.CultureInfo.CurrentUICulture;
                ToolStripMenuItem mi = new ToolStripMenuItem(culture.NativeName, null, new EventHandler(OnChangeLanguage));
                mi.Name = "do not translate";
                mi.Tag = culture;
                mi.Checked = true;
                languageToolStripMenuItem.DropDownItems.Add(mi);
            }
        }
        #endregion
    }
    #endregion

    #region ImageList
    internal class MyImageList
    {
        private ImageList list;
        public MyImageList()
        {
            list = new ImageList();
            list.ColorDepth = ColorDepth.Depth24Bit;
            list.ImageSize = new Size(16, 16);
            list.TransparentColor = Color.Magenta;

            Assembly assem = Assembly.GetAssembly(typeof(MyImageList));
            System.IO.Stream stream = assem.GetManifestResourceStream("iba.Resources.icons.bmp");
            Bitmap pics = new Bitmap(stream);
            list.Images.AddStrip(pics);
            //ImageList_SetOverlayImage(list.Handle, 0, OverlayNew); //we'll do this another time
        }

        public ImageList List { get { return list; } }
        public const int OverlayNew = 1;
        public const int Conf = 1;
        public const int NewConf = 2;
        public const int Status = 3;
        public const int NewQuote = 4;
        public const int Report = 5;
        public const int BatchFile = 6;
        public const int NewDeliveryNote = 7;
        public const int DeliveryNotes = 6;
        public const int Extract = 9;
        public const int NewInOrder = 10;
        public const int InOrders = 11;
        public const int OutOrder = 12;
        public const int NewOutOrder = 13;
        public const int OutOrders = 12;
        public const int Invoice = 15;
        public const int NewInvoice = 16;
        public const int Invoices = 15;
        public const int Items = 18;
        public const int Options = 19;
        public const int Print = 20;
        public const int Delete = 21;
        public const int Contacts = 22;
        public const int CreditNote = 23;
        public const int CreditNotes = 23;
        public const int NewCreditNote = 24;
    }
    #endregion
}
