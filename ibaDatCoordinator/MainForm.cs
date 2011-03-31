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
using Crownwood.DotNetMagic.Menus;
using System.Xml.Serialization;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.ServiceProcess;
using iba.Data;
using iba.Controls;
using iba.Utility;
using iba.Processing;
using iba.Plugins;
using Microsoft.Win32;

namespace iba
{
    #region MainForm
    public partial class MainForm : Form, IPropertyPaneManager, IExternalCommand
    {
        public static readonly int CONFIGURATION_INDEX = 0;
        public static readonly int ONETIME_CONFIGURATION_INDEX = 1;
        public static readonly int REPORTTASK_INDEX = 2;
        public static readonly int EXTRACTTASK_INDEX = 3;
        public static readonly int BATCHFILETASK_INDEX = 4;
        public static readonly int COPYTASK_INDEX = 5;
        public static readonly int IFTASK_INDEX = 6;
        public static readonly int UPDATEDATATASK_INDEX = 7;
        public static readonly int PAUSETASK_INDEX = 8;
        // add here any additional indices for new tasks, increase the next numbers
        public static readonly int NEWCONF_INDEX = 9;
        public static readonly int NEW_ONETIME_CONF_INDEX = 10;
        public static readonly int CUSTOMTASK_INDEX = 11;
        public static readonly int NR_TASKS = 7; 

        public MainForm()
        {
            m_firstConnectToService = true;
            InitializeComponent();
            //load any optional plugins
            PluginManager.Manager.LoadPlugins();

            this.Text += " v" + GetType().Assembly.GetName().Version.ToString(3);
            LogControl theLogControl; 
            propertyPanes["logControl"] = theLogControl = new LogControl();
            LogData.ApplicationState state = LogData.ApplicationState.CLIENTSTANDALONE;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                state = LogData.ApplicationState.CLIENTCONNECTED;
            else if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
                state = LogData.ApplicationState.CLIENTDISCONNECTED;
            LogData.InitializeLogger(theLogControl.LogView, theLogControl, state);
            theLogControl.CreateControl();

            //if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
            //{
            //    LogData.Data.MaxRows = Program.CommunicationObject.LoggerMaxRows;
            //    Program.CommunicationObject.Logging_setEventForwarder(new EventForwarder());
            //    ConfigurationData.IdCounter = TaskManager.Manager.IdCounter;
            //    string lf = LogData.Data.FileName;
            //    //if (lf != null)
            //    //    LogData.OpenFromFile(LogData.Data.FileName);
            //}

            configurationToolStripMenuItem.Enabled = false;
            statusToolStripMenuItem.Enabled = true;
            m_filename = null;
            CreateMenuItems();
            m_watchdogPane.LargeImage = m_watchdogPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.watchdog.Handle);
            m_statusPane.LargeImage = m_statusPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.status.Handle);
            m_configPane.LargeImage = m_configPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.configuration.Handle);
            m_loggingPane.LargeImage = m_loggingPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.logging.Handle);
            m_settingsPane.LargeImage = m_settingsPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.settings.Handle);
            m_toolTip.SetToolTip(m_startButton, iba.Properties.Resources.toolTipStartAll);
            m_toolTip.SetToolTip(m_stopButton, iba.Properties.Resources.toolTipStopAll);

            ImageList confsImageList = new ImageList();
            confsImageList.Images.Add(iba.Properties.Resources.configuration);
            confsImageList.Images.Add(iba.Properties.Resources.onetimeconfiguration);
            confsImageList.Images.Add(iba.Properties.Resources.report_running);
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.extract_running.ToBitmap()));
            confsImageList.Images.Add(iba.Properties.Resources.batchfile_running);
            confsImageList.Images.Add(iba.Properties.Resources.copydat_running);
            confsImageList.Images.Add(iba.Properties.Resources.iftask);
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.updatedatatask));
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(iba.Properties.Resources.pausetask));

            confsImageList.Images.Add(iba.Properties.Resources.configuration_new);
            confsImageList.Images.Add(iba.Properties.Resources.onetime_configuration_new);
            foreach (PluginTaskInfo info in PluginManager.Manager.PluginInfos)
            {
                confsImageList.Images.Add(info.Icon);
            }

            m_configTreeView.ImageList = confsImageList;
            
            ImageList confsImageList2 = new ImageList();
            confsImageList2.Images.Add(iba.Properties.Resources.greenarrow);
            confsImageList2.Images.Add(iba.Properties.Resources.redarrow); 
            confsImageList2.Images.Add(iba.Properties.Resources.redarrow1);
            m_configTreeView.StateImageList = confsImageList2;
            ImageList statImageList = new ImageList();
            statImageList.Images.Add(iba.Properties.Resources.configuration);
            statImageList.Images.Add(iba.Properties.Resources.brokenfile);
            m_statusTreeView.ImageList = statImageList;

            m_quitForm = new QuitForm(this);
            m_quitForm.CreateHandle(new CreateParams());
            if (Program.RunsWithService != Program.ServiceEnum.NOSERVICE)
            {
                WindowState = FormWindowState.Minimized;
                m_miRestoreCoordinator = new MenuItem(iba.Properties.Resources.notifyIconMenuItemRestore,miRestore_Click);
                m_miStartService = new MenuItem(iba.Properties.Resources.notifyIconMenuItemStartService, miStartService_Click);
                m_miStopService = new MenuItem(iba.Properties.Resources.notifyIconMenuItemStopService, miStopService_Click);
                m_miExit = new MenuItem(iba.Properties.Resources.notifyIconMenuItemExit, miExit_Click);
                MenuItem seperator = new MenuItem("-");
                MenuItem seperator2 = new MenuItem("-");
                m_miRestoreCoordinator.DefaultItem = true;
                m_iconMenu = 
                    new ContextMenu(new MenuItem[] 
                    { 
                        m_miRestoreCoordinator, 
                        seperator, 
                        m_miStartService, 
                        m_miStopService, 
                        seperator2, 
                        m_miExit 
                    }
                );
                m_iconMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
                m_iconMenu.Popup += new System.EventHandler(iconMenu_PopUp);

                m_iconEx = new NotifyIcon();
                m_iconEx.ContextMenu = m_iconMenu;
                m_iconEx.DoubleClick += new EventHandler(iconEx_DoubleClick);
                m_iconEx.Visible = false;
            }
            else
            {
                WindowState = FormWindowState.Normal;
                m_menuStrip.Items.Remove(serviceToolStripMenuItem);
                m_iconEx = null;
                m_iconMenu = null;
                m_miRestoreCoordinator = null;
                m_miStartService = null;
                m_miStopService = null;
                m_miExit = null;
                this.Icon = iba.Properties.Resources.standalone;
            }
            m_navBar.SelectedPane = m_configPane;
        }

        private bool m_actualClose = false;

        protected override void OnClosing(CancelEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized || Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
                FormStateSerializer.SaveSettings(this, "MainForm");

            SaveRightPaneControl();
            if (!m_actualClose && Program.RunsWithService != Program.ServiceEnum.NOSERVICE)
            {
                //if (m_tryConnectTimer != null)
                //{
                //    m_tryConnectTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                //    m_tryConnectTimer.Dispose();
                //    m_tryConnectTimer = null;
                //}
                if (WindowState != FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Minimized;
                    ShowInTaskbar = false;
                    e.Cancel = true;
                }
                return;
            }
            
            string s = String.IsNullOrEmpty(m_filename) ? "not set" : m_filename;
            Profiler.ProfileString(false, "LastState", "LastSavedFile", ref s, "not set");

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && Program.CommunicationObject.TestConnection())
            {
                Program.CommunicationObject.Logging_Log("Gui Stopped");
                Program.CommunicationObject.ForwardEvents = false;
                Program.CommunicationObject.SaveConfigurations();
            }
            else if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                string s1 = TextFromLoad();
                string s2 = TextToSave();
                if (s1 != s2)
                {
                    DialogResult res = MessageBox.Show(this, iba.Properties.Resources.saveQuestion,
                            iba.Properties.Resources.closing, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
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
                using (StopWaitDialog waiter = new StopWaitDialog())
                {
                    waiter.ShowDialog(this);
                }
            }
            LogData.Data.Logger.Close();
            base.OnClosing(e);
        }

        bool bHandleResize = false;
        protected override void OnResize(EventArgs e)
        {
            if (bHandleResize && WindowState == FormWindowState.Minimized)
            {
                ShowInTaskbar = false;
                Hide();
            }
            base.OnResize(e);
        }

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
                    if (!(confNode.Tag is ConfigurationTreeItemData))
                        confNode = confNode.Parent;
                    ConfigurationData dat = (confNode.Tag as ConfigurationTreeItemData).ConfigurationData;
                    TreeNode statNode = null;
                    foreach (TreeNode statCandidate in m_statusTreeView.Nodes)
                    {
                        if ((statCandidate.Tag as StatusTreeItemData).CorrConfigurationData.Guid == dat.Guid)
                        {
                            statNode = statCandidate;
                            break;
                        }
                    }
                    if (statNode != null)
                        m_statusTreeView.SelectedNode = statNode;
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
                    doSelection(m_statusTreeView.SelectedNode, (m_statusTreeView.SelectedNode.Tag as TreeItemData).What);
				}
                statusToolStripMenuItem.Enabled = false;
                pasteToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
                cutToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                configurationToolStripMenuItem.Enabled = true;
                loggingToolStripMenuItem.Enabled = true;
                watchdogToolStripMenuItem.Enabled = true;
            }
            else if (m_navBar.SelectedPane == m_configPane)
            {
                SaveRightPaneControl();
                TreeNode statNode = m_statusTreeView.SelectedNode;
                if (statNode != null && statNode.Parent != null) statNode = statNode.Parent; //was on permanent error files
                if (statNode != null)
                {
                    ConfigurationData confDat = (statNode.Tag as StatusTreeItemData).CorrConfigurationData;

                    foreach (TreeNode confCandidateNode in m_configTreeView.Nodes)
                    {
                        if (confCandidateNode.Tag is ConfigurationTreeItemData)
                        {
                            ConfigurationData confCandidateDat = (confCandidateNode.Tag as ConfigurationTreeItemData).ConfigurationData;
                            if (confCandidateDat.Guid == confDat.Guid)
                            {
                                m_configTreeView.SelectedNode = confCandidateNode;
                                break;
                            }
                        }
                    }
                }
                if (m_configTreeView.SelectedNode == null)
                {
                    if (m_configTreeView.Nodes.Count > 0) //do not choose the new config node
                        m_configTreeView.SelectedNode = m_configTreeView.Nodes[0];
                    else
                        SetRightPaneControl(null, m_configPane.Text, null);
                }
                else
                {
                    TreeItemData ti = m_configTreeView.SelectedNode.Tag as TreeItemData;
                    doSelection(m_configTreeView.SelectedNode, ti.What);
                }
                configurationToolStripMenuItem.Enabled = false;
                statusToolStripMenuItem.Enabled = true;
                loggingToolStripMenuItem.Enabled = true;
                watchdogToolStripMenuItem.Enabled = true;
                settingsToolStripMenuItem.Enabled = true;
                UpdateButtons();
            }
            else if (m_navBar.SelectedPane == m_loggingPane)
            {
                SaveRightPaneControl();
                SetRightPaneControl(propertyPanes["logControl"] as Control, iba.Properties.Resources.logTitle, LogData.Data);
                loggingToolStripMenuItem.Enabled = false;
                pasteToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
                cutToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                statusToolStripMenuItem.Enabled = true;
                configurationToolStripMenuItem.Enabled = true;
                watchdogToolStripMenuItem.Enabled = true;
                m_EntriesNumericUpDown1.Value = Math.Max(1,LogData.Data.MaxRows);
                settingsToolStripMenuItem.Enabled = true;
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
                watchdogToolStripMenuItem.Enabled = false; 
                pasteToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
                cutToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                statusToolStripMenuItem.Enabled = true;
                configurationToolStripMenuItem.Enabled = true;
                loggingToolStripMenuItem.Enabled = true;
                settingsToolStripMenuItem.Enabled = true;
            }
            else if (m_navBar.SelectedPane == m_settingsPane)
            {
                SaveRightPaneControl();
                Control ctrl = propertyPanes["settingsControl"] as Control;
                if (ctrl == null)
                {
                    ctrl = new ServiceSettingsControl();
                    propertyPanes["settingsControl"] = ctrl;
                }
                SetRightPaneControl(ctrl as Control, iba.Properties.Resources.settingsTitle, null);
                pasteToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
                cutToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                statusToolStripMenuItem.Enabled = true;
                configurationToolStripMenuItem.Enabled = true;
                loggingToolStripMenuItem.Enabled = true;
                watchdogToolStripMenuItem.Enabled = true;
                settingsToolStripMenuItem.Enabled = false;
            }
        }


        private void ReloadRightPane()
        { //only handles the cases of settings or watchdog panes, the rest is handled differently
            if (m_navBar.SelectedPane == m_watchdogPane)
            {
                WatchdogControl pane = propertyPanes["watchdogControl"] as WatchdogControl;
                if (pane!=null)
                {
                    pane.LoadData(TaskManager.Manager.WatchDogData.Clone(), this);
                }
            }
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
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
                FormStateSerializer.LoadSettings(this, "MainForm");
            SetRenderer();
            string returnvalue = "";
            Profiler.ProfileString(true, "LastState", "LastSavedFile", ref returnvalue, "not set");
            if (returnvalue != "not set" && Program.RunsWithService != Program.ServiceEnum.CONNECTED) loadFromFile(returnvalue,true);
            loadConfigurations();
            loadStatuses();
            UpdateButtons();

            if (WindowState == FormWindowState.Minimized)
            {
                Hide();
                bHandleResize = true;
            }
            if (Program.RunsWithService != Program.ServiceEnum.NOSERVICE)
                m_iconEx.Visible = true;
        }

        private TreeNode CreateConfigurationNode(ConfigurationData confIt)
        {
            TreeNode confNode = null;
            if (confIt.OnetimeJob)
                confNode = new TreeNode(confIt.Name, ONETIME_CONFIGURATION_INDEX, ONETIME_CONFIGURATION_INDEX);
            else
                confNode = new TreeNode(confIt.Name, CONFIGURATION_INDEX, CONFIGURATION_INDEX);
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
                else
                {
                    CustomTaskData cust = (CustomTaskData)task;
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
            loadStatuses();
        }

        private void loadConfigurations()
        {
            m_configTreeView.BeginUpdate();
			m_configTreeView.Nodes.Clear();
            if (TaskManager.Manager.Count == 0)
            {
                ConfigurationData newData = new ConfigurationData(iba.Properties.Resources.newConfigurationName,false);
                TaskManager.Manager.AddConfiguration(newData);
            }
            List<ConfigurationData> confs = TaskManager.Manager.Configurations;
            confs.Sort(delegate (ConfigurationData a, ConfigurationData b) {return a.TreePosition.CompareTo(b.TreePosition);});
            foreach (ConfigurationData confIt in confs)
                m_configTreeView.Nodes.Add(CreateConfigurationNode(confIt));

            //add the new Configuration node
            TreeNode newConfNode = new TreeNode(iba.Properties.Resources.addConfigurationText, NEWCONF_INDEX, NEWCONF_INDEX);
            newConfNode.ForeColor = Color.Blue;
            newConfNode.Tag = new NewConfigurationTreeItemData(this);
            m_configTreeView.Nodes.Add(newConfNode);

            //add the one time Configuration node
            TreeNode new1ConfNode = new TreeNode(iba.Properties.Resources.addOneTimeConfigurationText, NEW_ONETIME_CONF_INDEX, NEW_ONETIME_CONF_INDEX);
            new1ConfNode.ForeColor = Color.Blue;
            new1ConfNode.Tag = new NewOneTimeConfigurationTreeItemData(this);
            m_configTreeView.Nodes.Add(new1ConfNode);
 
            m_configTreeView.EndUpdate();
            m_configTreeView.SelectedNode = m_configTreeView.Nodes[0];
            UpdateTreePositions();
            UpdateButtons();
        }

        private void loadStatuses()
        {
            m_statusTreeView.BeginUpdate();
            m_statusTreeView.Nodes.Clear();

            List<ConfigurationData> confs = TaskManager.Manager.Configurations;
            confs.Sort(delegate(ConfigurationData a, ConfigurationData b) { return a.TreePosition.CompareTo(b.TreePosition); });
            foreach (ConfigurationData confIt in confs)
            {
                TreeNode statNode = new TreeNode(confIt.Name,0, 0);
                statNode.Tag = new StatusTreeItemData(this as IPropertyPaneManager, confIt);
                MainForm.strikeOutNodeText(statNode, !confIt.Enabled);
                m_statusTreeView.Nodes.Add(statNode);
                if (confIt.LimitTimesTried)
                {
                    TreeNode permFailedNode = new TreeNode(iba.Properties.Resources.PermanentlyFailedDatFiles, 1, 1);
                    permFailedNode.Tag = new StatusPermanentlyErrorFilesTreeItemData(this as IPropertyPaneManager, confIt);
                    statNode.Nodes.Add(permFailedNode);
                }
            }
            m_statusTreeView.EndUpdate();
            m_statusTreeView.SelectedNode = m_statusTreeView.Nodes[0];
        }

        private void OnStatusTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            using (WaitCursor wait = new WaitCursor())
            {
                TreeNode node = e.Node;
                if (node == null)
                    doSelection(node, "Status");
                else
                    doSelection(node, (m_statusTreeView.SelectedNode.Tag as TreeItemData).What);
            }
        }

        private void doSelection(TreeNode node, string what)
        {
            string title = node.Text;
            TreeNode copyNode = node;
            while (copyNode.Parent != null)
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
                    goto case "task";
                case "Report":
                    goto case "task";
                case "Extract":
                    goto case "task";
                case "CopyTask":
                    goto case "task";
                case "IfTask":
                    goto case "task";
                case "UpdateDataTask":
                    goto case "task";
                case "PauseTask":
                    goto case "task";
                case "CustomTask":
                    goto case "task";
                case "task":
                    {
                        if (m_navBar.SelectedPane != m_configPane) return;
                        TreeItemData data = node.Tag as TreeItemData;
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
            using (WaitCursor wait = new WaitCursor())
            {
                bool c1 = node.Tag is NewConfigurationTreeItemData;
                bool c2 = node.Tag is NewOneTimeConfigurationTreeItemData;
                if (c1 || c2)
                {
                    SaveRightPaneControl();
                    //code to create new configuration
                    ConfigurationData newData = new ConfigurationData(iba.Properties.Resources.newConfigurationName, c2);
                    new SetNextName(newData);
                    TaskManager.Manager.AddConfiguration(newData);
                    m_configTreeView.BeginUpdate();
                    m_configTreeView.Nodes.Insert(m_configTreeView.Nodes.Count - 2, CreateConfigurationNode(newData));
                    //loadConfigurations();
                    m_configTreeView.EndUpdate();
                    node = m_configTreeView.Nodes[m_configTreeView.Nodes.Count - 3];
                    m_configTreeView.SelectedNode = node;
                    loadStatuses();
                    UpdateButtons();
                    UpdateTreePositions();
                }
                else 
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
            if ((e.KeyCode != Keys.Delete) || (m_configTreeView.SelectedNode == null) || !(m_configTreeView.Focused))
                return;
            Delete(m_configTreeView.SelectedNode);
        }

        private void Delete(TreeNode node)
        {
            Delete(node, true);
        }

        private void Delete(TreeNode node, bool ask)
        {
            if (ask)
            {
                string msg = null;
                if (node.Tag is ConfigurationTreeItemData)
                {
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
                    else if (node.Tag is CustomTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteCustomTaskQuestion,
                        (((CustomTaskTreeItemData)(node.Tag)).DataSource as CustomTaskData).Plugin.NameInfo,
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
                if (TaskManager.Manager.IsJobStarted((node.Tag as ConfigurationTreeItemData).ConfigurationData.Guid))
                    return;
                int newIndex = node.Index - 1;
                m_configTreeView.SelectedNode = null;
                TaskManager.Manager.RemoveConfiguration((node.Tag as ConfigurationTreeItemData).ConfigurationData);
                m_configTreeView.Nodes.Remove(node);
                if ((newIndex < m_configTreeView.Nodes.Count) && (newIndex >= 0))
                    m_configTreeView.SelectedNode = m_configTreeView.Nodes[newIndex];
                else
                    m_configTreeView.SelectedNode = m_configTreeView.Nodes[0];
                UpdateTreePositions();
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
            if (node == null && m_cd_copy != null) //add configurationData
            {
                SaveRightPaneControl();
                //code to create new configuration
                new SetNextName(m_cd_copy);
                foreach (TaskData t in m_cd_copy.Tasks)
                    new SetNextName(t);
                TaskManager.Manager.AddConfiguration(m_cd_copy);
                m_configTreeView.BeginUpdate();
                TreeNode tn = CreateConfigurationNode(m_cd_copy);
                m_configTreeView.Nodes.Insert(m_configTreeView.Nodes.Count - 1, tn);
                tn.EnsureVisible();
                //loadConfigurations();
                m_configTreeView.EndUpdate();
                node = m_configTreeView.Nodes[m_configTreeView.Nodes.Count - 2];
                m_configTreeView.SelectedNode = node;
                m_cd_copy = m_cd_copy.Clone() as ConfigurationData;
                UpdateTreePositions();
                loadStatuses();
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
                m_configTreeView.Nodes.Insert(index, tn);
                tn.EnsureVisible();
                //loadConfigurations();
                m_configTreeView.EndUpdate();
                UpdateTreePositions();
                node = m_configTreeView.Nodes[index];
                m_configTreeView.SelectedNode = node;
                m_cd_copy = m_cd_copy.Clone() as ConfigurationData;
                loadStatuses();
                UpdateButtons();
            }
            else if (node.Tag is ConfigurationTreeItemData && m_task_copy != null)
            {
                SaveRightPaneControl();
                ConfigurationData origData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
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
                else if (m_task_copy is CustomTaskData)
                {
                    CustomTaskData cust = (CustomTaskData) m_task_copy;
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
                else if (m_task_copy is CustomTaskData)
                {
                    CustomTaskData cust = (CustomTaskData)m_task_copy;
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
        private PopupMenu m_popupMenu = new PopupMenu();
        private MenuCommand[] m_menuItems;


        //methods for rightclicking copy paste
        private ConfigurationData m_cd_copy = null;
        private TaskData m_task_copy = null;

        private void CreateMenuItems()
        {
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
            foreach (PluginTaskInfo info in PluginManager.Manager.PluginInfos)
                menuImages.Images.Add(info.Icon);


            int customcount = PluginManager.Manager.PluginInfos.Count;
            m_menuItems = new MenuCommand[13 + customcount];
            m_menuItems[(int)MenuItemsEnum.Delete] = new MenuCommand(iba.Properties.Resources.deleteTitle, il.List, MyImageList.Delete, Shortcut.Del, new EventHandler(OnDeleteMenuItem));
            m_menuItems[(int)MenuItemsEnum.CollapseAll] = new MenuCommand(iba.Properties.Resources.collapseTitle, il.List, -1, Shortcut.None, new EventHandler(OnCollapseAllMenuItem));
            m_menuItems[(int)MenuItemsEnum.Cut] = new MenuCommand(iba.Properties.Resources.cutTitle, menuImages, 0, Shortcut.CtrlX, new EventHandler(OnCutMenuItem));
            m_menuItems[(int)MenuItemsEnum.Copy] = new MenuCommand(iba.Properties.Resources.copyTitle, menuImages, 1, Shortcut.CtrlC, new EventHandler(OnCopyMenuItem));
            m_menuItems[(int)MenuItemsEnum.Paste] = new MenuCommand(iba.Properties.Resources.pasteTitle, menuImages, 2, Shortcut.CtrlV, new EventHandler(OnPasteMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewTask] = new MenuCommand(iba.Properties.Resources.NewTaskTitle, il.List, -1, Shortcut.None);
            m_menuItems[(int)MenuItemsEnum.NewReport] = new MenuCommand(iba.Properties.Resources.NewReportTitle, menuImages, 3, Shortcut.None, new EventHandler(OnNewReportMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewExtract] = new MenuCommand(iba.Properties.Resources.NewExtractTitle, menuImages, 4, Shortcut.None, new EventHandler(OnNewExtractMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewBatchfile] = new MenuCommand(iba.Properties.Resources.NewBatchfileTitle, menuImages, 5, Shortcut.None, new EventHandler(OnNewBatchfileMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewCopyTask] = new MenuCommand(iba.Properties.Resources.NewCopyTaskTitle,menuImages, 6,Shortcut.None, new EventHandler(OnNewCopyTaskMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewIfTask] = new MenuCommand(iba.Properties.Resources.NewIfTaskTitle, menuImages, 7, Shortcut.None, new EventHandler(OnNewIfTaskMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewUpdateDataTask] = new MenuCommand(iba.Properties.Resources.NewUpdateDataTaskTitle, Shortcut.None, new EventHandler(OnNewUpdateDataTaskMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewUpdateDataTask].Image = iba.Properties.Resources.updatedatatask;//png, don't use imagelist because imagelist has problem with alpha channel pixels in the png
            m_menuItems[(int)MenuItemsEnum.NewPauseTask] = new MenuCommand(iba.Properties.Resources.NewPauseTaskTitle, Shortcut.None, new EventHandler(OnNewPauseTaskMenuItem));
            m_menuItems[(int)MenuItemsEnum.NewPauseTask].Image = iba.Properties.Resources.pausetask;//png, don't use imagelist because imagelist has problem with alpha channel pixels in the png

            for (int i = 0; i < customcount; i++)
            {
                string title = String.Format(iba.Properties.Resources.NewCustomTaskTitle, PluginManager.Manager.PluginInfos[i].Name);
                m_menuItems[i + (int)MenuItemsEnum.NewCustomTask] = new MenuCommand(title, menuImages, NR_TASKS+3+i, Shortcut.None, new EventHandler(OnNewCustomTaskMenuItem));
            }
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewReport]);
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewExtract]);
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewBatchfile]);
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewCopyTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewIfTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewUpdateDataTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewPauseTask]);
            for (int i = 0; i < customcount; i++)
            {
                m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[i + (int)MenuItemsEnum.NewCustomTask]);
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
            NewCustomTask = 13
        }

        private string ibaAnalyzerExe;

        private void m_configTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            TreeNode node = m_configTreeView.GetNodeAt(e.X, e.Y);
            ArrayList items = new ArrayList();
            if (node != null)
            {
                bool started = false;
                if (node.Parent != null) //tasknode
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
            m_popupMenu.MenuCommands.Clear();
            foreach (MenuItemsEnum item in items)
            {
                MenuCommand mc = m_menuItems[(int)item];
                mc.Tag = node;
                m_popupMenu.MenuCommands.Add(mc);
                if (item == MenuItemsEnum.NewTask)
                {
                    int index = PluginManager.Manager.PluginInfos.Count - mc.MenuCommands.Count;
                    foreach (MenuCommand mc2 in mc.MenuCommands)
                    {
                        if (index < 0)
                            mc2.Tag = node;
                        else
                            mc2.Tag = new Pair<TreeNode, int>(node, index);
                        index++;
                    }
                }
            }
            m_popupMenu.TrackPopup(Cursor.Position);
        }

        private void OnDeleteMenuItem(object sender, EventArgs e)
        {
            MenuCommand mc = (MenuCommand)sender;
            TreeNode node = mc.Tag as TreeNode;
            Delete(node);
        }

        private void OnCutMenuItem(object sender, EventArgs e)
        {
            OnCopyMenuItem(sender, e);
            MenuCommand mc = (MenuCommand)sender;
            TreeNode node = mc.Tag as TreeNode;
            Delete(node, false);
        }

        private void OnPasteMenuItem(object sender, EventArgs e)
        {
            MenuCommand mc = (MenuCommand)sender;
            TreeNode node = mc.Tag as TreeNode;
            pasteNode(node);
        }

        private void OnCopyMenuItem(object sender, EventArgs e)
        {
            MenuCommand mc = (MenuCommand)sender;
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

        private void OnNewReportMenuItem(object sender, EventArgs e)
        {
            MenuCommand mc = (MenuCommand)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
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
            MenuCommand mc = (MenuCommand)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
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
            MenuCommand mc = (MenuCommand)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
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
            MenuCommand mc = (MenuCommand)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
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

            MenuCommand mc = (MenuCommand)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
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
            MenuCommand mc = (MenuCommand)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
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
            MenuCommand mc = (MenuCommand)sender;
            TreeNode node = mc.Tag as TreeNode;
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
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

        private void OnNewCustomTaskMenuItem(object sender, EventArgs e)
        {
            MenuCommand mc = (MenuCommand)sender;
            Pair<TreeNode, int> p = mc.Tag as Pair<TreeNode, int>;
            TreeNode node = p.First;
            int index = p.Second;
            PluginTaskInfo info = PluginManager.Manager.PluginInfos[index];
            ConfigurationData confData = (node.Tag as ConfigurationTreeItemData).ConfigurationData;
            CustomTaskData cust = new CustomTaskData(confData, info);
            new SetNextName(cust);
            confData.Tasks.Add(cust);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            TreeNode newNode = new TreeNode(cust.Name, CUSTOMTASK_INDEX + index, CUSTOMTASK_INDEX + index);
            newNode.Tag = new CustomTaskTreeItemData(this, cust);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData);
        }

        public void AdjustFrontIcons(ConfigurationData data)
        {
            foreach (TreeNode node in m_configTreeView.Nodes)
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

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_navBar.SelectedPane = m_settingsPane;
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutBox ab = new AboutBox())
            {
                ab.StartPosition = FormStartPosition.CenterParent;
                ab.ShowDialog(this);
            }
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
                copyNode(node);
                Delete(node, false);
            }
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_configTreeView.Focused)
            {
                TreeNode node = m_configTreeView.SelectedNode;
                copyNode(node);
            }
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_configTreeView.Focused)
            {
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
            if (m_filename == null)
            {
                saveAsToolStripMenuItem_Click(sender, e);
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

                }
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    Program.CommunicationObject.SaveConfigurations();
            }
        }

        private string TextToSave()
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));
            StringBuilder sb = new StringBuilder();
            try
            {
                using (StringWriter myWriter = new StringWriter(sb))
                {
                    ibaDatCoordinatorData dat = ibaDatCoordinatorData.Create(TaskManager.Manager);
                    mySerializer.Serialize(myWriter, dat);
                }
            }
            catch
            {
                return String.Empty;
            }
            string s = sb.ToString();
            s = s.Remove(0, s.IndexOf(Environment.NewLine));
            return s.Remove(0, s.IndexOf('<'));
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
	        m_saveFileDialog.CreatePrompt = false;
		    m_saveFileDialog.OverwritePrompt = true;
	    	m_saveFileDialog.FileName = "myConfigurations";
	    	m_saveFileDialog.DefaultExt = "xml";
	    	m_saveFileDialog.Filter = "XML files (*.xml)|*.xml";
            if (m_saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                m_filename = m_saveFileDialog.FileName;
                this.Text = m_filename + " - ibaDatCoordinator v" + GetType().Assembly.GetName().Version.ToString(3);
                saveToolStripMenuItem_Click(sender, e);
            }
        }

        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {

            m_openFileDialog.CheckFileExists = true;
            m_openFileDialog.Filter = "XML files (*.xml)|*.xml";
            m_openFileDialog.FileName = "";
            DialogResult result = m_openFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;
                
            SaveRightPaneControl();
            loadFromFile(m_openFileDialog.FileName,false);
            loadConfigurations();
            loadStatuses();
            ReloadRightPane();
        }

        private bool loadFromFile(string filename, bool beSilent)
        {
            try
            {
                using (WaitCursor wait = new WaitCursor())
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));
                    List<ConfigurationData> confs;
                    using (FileStream myFileStream = new FileStream(filename, FileMode.Open))
                    {
                        ibaDatCoordinatorData dat = (ibaDatCoordinatorData) mySerializer.Deserialize(myFileStream);
                        confs = dat.ApplyToManager(TaskManager.Manager);
                    }
                    m_filename = filename;
                    this.Text = m_filename + " - ibaDatCoordinator v" + GetType().Assembly.GetName().Version.ToString(3);
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
                    TaskManager.Manager.Configurations = confs;
                    if (Program.RunsWithService != Program.ServiceEnum.DISCONNECTED)
                        foreach (ConfigurationData dat in confs)
                        {
                            if (dat.AutoStart) TaskManager.Manager.StartConfiguration(dat);
                        }
                }
            }
            catch (Exception ex)
            {
                if (!beSilent) MessageBox.Show(iba.Properties.Resources.OpenFileProblem + "  " + ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveRightPaneControl();
           string s1 = TextFromLoad();
            string s2 = TextToSave();
            if (s1 != s2)
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
            this.Text = "ibaDatCoordinator v" + GetType().Assembly.GetName().Version.ToString(3);
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
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                Program.CommunicationObject.LoggerMaxRows = LogData.Data.MaxRows;
        }

        private void m_stopButton_Click(object sender, EventArgs e)
        {
            SaveRightPaneControl();
            using (StopWaitDialog waiter = new StopWaitDialog())
            {
                waiter.ShowDialog(this);
            }
            if (m_configTreeView.SelectedNode != null && m_configTreeView.SelectedNode.Tag is ConfigurationTreeItemData)
            {
                (m_rightPane.Controls[0] as ConfigurationControl).LoadData((m_configTreeView.SelectedNode.Tag as ConfigurationTreeItemData).ConfigurationData, this);
            }
            UpdateButtons();
        }

        private void m_startButton_Click(object sender, EventArgs e)
        {
            SaveRightPaneControl();
            if (!String.IsNullOrEmpty(m_filename)) saveToolStripMenuItem_Click(null, null);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                Program.CommunicationObject.SaveConfigurations();

            StatusBarLabel.Text = ""; //clear any errors on restart
            TaskManager.Manager.StartAllEnabledConfigurations();
            if (m_configTreeView.SelectedNode != null && m_configTreeView.SelectedNode.Tag is ConfigurationTreeItemData)
            {
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
                m_startButton.Enabled = m_stopButton.Enabled = false;
                m_iconEx.Icon = this.Icon = iba.Properties.Resources.disconnectedIcon;
                m_iconEx.Text = iba.Properties.Resources.niDisconnected;
                return;
            }
            bool allEnabledStarted = true;
            bool allStopped = true;
            
            foreach (ConfigurationData data in TaskManager.Manager.Configurations)
            {
                bool started = TaskManager.Manager.IsJobStarted(data.Guid);
                if (data.Enabled)
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
                    m_iconEx.Icon = this.Icon = iba.Properties.Resources.runningIcon;
                    m_iconEx.Text = iba.Properties.Resources.niRunning;
                }
                else
                {
                    m_iconEx.Icon = this.Icon = iba.Properties.Resources.connectedIcon;
                    m_iconEx.Text = iba.Properties.Resources.niConnected;
                }
            }
        }

        public void SetRenderer()
        {
            TD.Eyefinder.Office2003Renderer renderer = (TD.Eyefinder.Office2003Renderer)m_navBar.Renderer;
            if (Program.RunsWithService != Program.ServiceEnum.DISCONNECTED)
            {
                renderer.HeaderBackgroundColor1 = Color.FromArgb(255, 89, 135, 214);
                renderer.HeaderBackgroundColor2 = Color.FromArgb(255, 0, 45, 150);
                m_rightPane.SetActiveRenderer(renderer);
                m_navBar.Invalidate();
                m_rightPane.Invalidate();
            }
            else
            {
                renderer.HeaderBackgroundColor1 = Color.MistyRose;
                renderer.HeaderBackgroundColor2 = Color.Crimson;
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

        #endregion

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
        
        public ToolStripStatusLabel StatusBarLabel
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

            if (draggedNode.Tag is ConfigurationTreeItemData && e.Effect == DragDropEffects.Move)
            {
                //just move the nodes, no actual change
                m_configTreeView.SelectedNode = null;
                int index = targetNode.Index;
                m_configTreeView.BeginUpdate();
                m_configTreeView.Nodes.Remove(draggedNode);
                m_configTreeView.Nodes.Insert(index, draggedNode);
                m_configTreeView.EndUpdate();
                m_configTreeView.SelectedNode = m_configTreeView.Nodes[index];
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
            if ((draggedNode.Tag is ConfigurationTreeItemData) && !(targetNode.Tag is ConfigurationTreeItemData))
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
                if (!(draggedNode.Tag is NewConfigurationTreeItemDataBase))
                    DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }
        #endregion


        private void UpdateTreePositions()
        {
            foreach (TreeNode node in m_configTreeView.Nodes)
            {
                ConfigurationTreeItemData dat = (node.Tag as ConfigurationTreeItemData);
                if (dat != null)
                {
                    dat.ConfigurationData.TreePosition = node.Index;
                    if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    {
                        TaskManager.Manager.UpdateTreePosition(dat.ConfigurationData.Guid, node.Index);
                    }
                }
            }
        }


        #region Service related methods
        public void OnStartService()
        {
            SaveRightPaneControl();
            m_firstConnectToService = false; //user starts service manually, do not automatically load user stuff
            //startservice dialog
            using (StartServiceDialog ssd = new StartServiceDialog())
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ssd.StartPosition = FormStartPosition.CenterScreen;
                    ssd.ShowDialog();
                }
                else
                {
                    ssd.StartPosition = FormStartPosition.CenterParent;
                    ssd.ShowDialog(this);
                }
            }
        }

        public void OnStopService()
        {
            //stopService dialog
            SaveRightPaneControl();
            //Program.CommunicationObject.StoppingService = true;
            bool result = false;
            using (StopServiceDialog ssd = new StopServiceDialog())
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ssd.StartPosition = FormStartPosition.CenterScreen;
                    ssd.ShowDialog();
                    result = ssd.Result;
                }
                else
                {
                    ssd.StartPosition = FormStartPosition.CenterParent;
                    ssd.ShowDialog(this);
                    result = ssd.Result;
                }
            }
            if (result)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                LogData.Data.Logger.Log(Logging.Level.Info, iba.Properties.Resources.logServiceStopped);
                if (m_navBar.SelectedPane == m_statusPane)
                    loadStatuses();
                else if (m_navBar.SelectedPane == m_configPane)
                    loadConfigurations();
            }
            //Program.CommunicationObject.StoppingService = false;
        }

        public delegate void IbaAnalyzerCall();


        private bool m_firstConnectToService;

        public void TryToConnect(object ignoreMe)
        {
            if (m_tryConnectTimer != null)
                m_tryConnectTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                CommunicationObject com = (CommunicationObject)Activator.GetObject(typeof(CommunicationObject), "tcp://localhost:8800/IbaDatCoordinatorCommunicationObject");
                CommunicationObjectWrapper wrapper = new CommunicationObjectWrapper(com);
                if (wrapper.TestConnection()) //succesfully connected
                {
                    if (m_tryConnectTimer != null) //this is not the first call, restore stuff
                    {
                        MethodInvoker m = delegate()
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
                                if (data != null) data.ApplyToManager(TaskManager.Manager);
                                ReplaceManagerFromTree(TaskManager.Manager);
                                foreach (ConfigurationData dat in TaskManager.Manager.Configurations)
                                {
                                    if (dat.AutoStart) TaskManager.Manager.StartConfiguration(dat);
                                }
                                if (m_navBar.SelectedPane == m_statusPane)
                                    loadStatuses();
                                else if (m_navBar.SelectedPane == m_configPane)
                                    loadConfigurations();
                                ReloadRightPane();
                            }
                            else //download
                            {
                                loadConfigurations();
                                loadStatuses();
                                ReloadRightPane();
                            }
                            UpdateButtons();
                        };
                        Invoke(m);
                    }
                    else
                    {
                        Program.RunsWithService = Program.ServiceEnum.CONNECTED;
                        TaskManager.Manager = null; //remove previous client taskmanager so it does not stay
                        // alive during the online session
                        Program.CommunicationObject = wrapper;
                    }
                    LogData.Data.Logger.Close();
                    GridViewLogger gv = null; ;
                    if (LogData.Data.Logger is iba.Logging.Loggers.CompositeLogger)
                        gv = LogData.Data.Logger.Children[0] as GridViewLogger;
                    else
                        gv = LogData.Data.Logger as GridViewLogger;
                    LogData.InitializeLogger(gv.Grid, gv.LogControl, LogData.ApplicationState.CLIENTCONNECTED);
                    Program.CommunicationObject.Logging_setEventForwarder(new EventForwarder());
                    m_firstConnectToService = false;
                    SetRenderer();
                }
                else
                {
                    Program.RunsWithService = Program.ServiceEnum.DISCONNECTED;
                }
            }
            else if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
            {
                try
                {
                    if (!Program.CommunicationObject.TestConnection())
                    {
                        MethodInvoker m2 = delegate()
                        {
                            Program.CommunicationObject.HandleBrokenConnection();
                        };
                        Invoke(m2);
                    }
                }
                catch { }
            }

            if (m_tryConnectTimer == null)
                m_tryConnectTimer = new System.Threading.Timer(TryToConnect);
            m_tryConnectTimer.Change(TimeSpan.FromSeconds(5.0), TimeSpan.Zero);
        }

        private bool NeedUploadToServer()
        {
            if (TaskManager.Manager.Count == 0) return true; //nothing on server side, upload the minimum configuration of one
            if (m_firstConnectToService) return false;
            //test if there's a difference between the client and server configurations
            foreach (TreeNode t in m_configTreeView.Nodes)
            {
                if (t.Tag is ConfigurationTreeItemData)
                {
                    ConfigurationData data = (t.Tag as ConfigurationTreeItemData).ConfigurationData;
                    if (!TaskManager.Manager.CompareConfiguration(data))
                    {
                        UploadOrDownloadConfigurationsDialog uodDiag = new UploadOrDownloadConfigurationsDialog();
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
            }
            return false;
        }

        private System.Threading.Timer m_tryConnectTimer;
        
        public void ReplaceManagerFromTree(TaskManager m)
        {
            List<ConfigurationData> toReplace = new List<ConfigurationData>();
            foreach (TreeNode t in m_configTreeView.Nodes)
            {
                if (t.Tag is ConfigurationTreeItemData)
                    toReplace.Add((t.Tag as ConfigurationTreeItemData).ConfigurationData);
            }
            TaskManager.Manager.ReplaceConfigurations(toReplace);
        }

        public void OnExternalActivate()
        {
            Show();
            Activate();
            WindowState = FormWindowState.Normal;
            FormStateSerializer.LoadSettings(this, "MainForm");
            ShowInTaskbar = true;
        }

        private void miRestore_Click(object sender, System.EventArgs e)
        {
            OnExternalActivate();
        }

        private void miExit_Click(object sender, System.EventArgs e)
        {
            OnExternalClose();
        }

        private void miStartService_Click(object sender, System.EventArgs e)
        {
            OnStartService();
        }

        private void miStopService_Click(object sender, System.EventArgs e)
        {
            OnStopService();
        }

        private void iconEx_DoubleClick(object sender, System.EventArgs e)
        {
            if (m_iconEx.ContextMenu != null)
            {
                foreach (MenuItem item in m_iconEx.ContextMenu.MenuItems)
                {
                    if (item.DefaultItem && item.Enabled)
                    {
                        item.PerformClick();
                        return;
                    }
                }
            }
        }

        public void OnExternalClose()
        {
            m_actualClose = true;
            if (m_iconEx != null)
                m_iconEx.Dispose();
            Close();
        }

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_QUERYENDSESSION = 0x011;
            if (m.Msg == WM_QUERYENDSESSION)
            {
                m_actualClose = true;
            }
            base.WndProc(ref m);

        } //WndProc 

        private void serviceToolStripMenuItem_DropDownOpening(object sender, EventArgs e)
        {
            ServiceController service = new ServiceController("IbaDatCoordinatorService");
            try
            {
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    startServiceToolStripMenuItem.Enabled = true;
                    stopServiceToolStripMenuItem.Enabled = false;
                }
                else
                {
                    startServiceToolStripMenuItem.Enabled = false;
                    stopServiceToolStripMenuItem.Enabled = true;
                }
            }
            catch (Exception)
            {
                startServiceToolStripMenuItem.Enabled = false;
                stopServiceToolStripMenuItem.Enabled = false;
            }
            service.Close();
        }

        private void iconMenu_PopUp(object sender, EventArgs e)
        {
            ServiceController service = new ServiceController("IbaDatCoordinatorService");
            try
            {
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    m_miStartService.Enabled = true;
                    m_miStopService.Enabled = false;
                }
                else
                {
                    m_miStartService.Enabled = false;
                    m_miStopService.Enabled = true;
                }
            }
            catch (Exception)
            {
                m_miStartService.Enabled = false;
                m_miStopService.Enabled = false;
            }
            service.Close();
        }

        private QuitForm m_quitForm;
        private NotifyIcon m_iconEx;
        public NotifyIcon NotifyIcon
        {
            get { return m_iconEx; }
        }

        private ContextMenu m_iconMenu;
        private MenuItem m_miRestoreCoordinator;
        private MenuItem m_miStartService; 
        private MenuItem m_miStopService;
        private MenuItem m_miExit;
        #endregion
    }

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
