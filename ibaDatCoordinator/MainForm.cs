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

namespace iba
{
    #region MainForm
    public partial class MainForm : Form, IPropertyPaneManager, IExternalCommand
    {
        public MainForm()
        {
            InitializeComponent();
            this.Text += " v" + GetType().Assembly.GetName().Version.ToString(3);
            //register nodename change with the configurationcontrol
            LogControl theLogControl; 
            propertyPanes["logControl"] = theLogControl = new LogControl();
            LogData.InitializeLogger(theLogControl.LogView, theLogControl, Program.RunsWithService == Program.ServiceEnum.CONNECTED);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
            {
                Program.CommunicationObject.Logging_setEventForwarder(new EventForwarder());
                ConfigurationData.IdCounter = TaskManager.Manager.IdCounter;
            }
            theLogControl.CreateControl();
            configurationToolStripMenuItem.Enabled = false;
            statusToolStripMenuItem.Enabled = true;
            m_filename = null;
            CreateMenuItems();
            m_watchdogPane.LargeImage = m_watchdogPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.watchdog.Handle);
            m_statusPane.LargeImage = m_statusPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.status.Handle);
            m_configPane.LargeImage = m_configPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.configuration.Handle);
            m_loggingPane.LargeImage = m_loggingPane.SmallImage = Bitmap.FromHicon(iba.Properties.Resources.logging.Handle);
            m_navBar.SelectedPane = m_configPane;
            m_toolTip.SetToolTip(m_startButton, iba.Properties.Resources.toolTipStartAll);
            m_toolTip.SetToolTip(m_stopButton, iba.Properties.Resources.toolTipStopAll);

            ImageList confsImageList = new ImageList();
            confsImageList.Images.Add(iba.Properties.Resources.configuration);
            confsImageList.Images.Add(iba.Properties.Resources.report_running);
            confsImageList.Images.Add(iba.Properties.Resources.extract_running);
            confsImageList.Images.Add(iba.Properties.Resources.batchfile_running);
            confsImageList.Images.Add(iba.Properties.Resources.copydat_running);
            confsImageList.Images.Add(iba.Properties.Resources.configuration_new);
            m_configTreeView.ImageList = confsImageList;
            
            ImageList confsImageList2 = new ImageList();
            confsImageList2.Images.Add(iba.Properties.Resources.greenarrow);
            confsImageList2.Images.Add(iba.Properties.Resources.redarrow); 
            confsImageList2.Images.Add(iba.Properties.Resources.redarrow1);
            m_configTreeView.StateImageList = confsImageList2;
            ImageList statImageList = new ImageList();
            statImageList.Images.Add(iba.Properties.Resources.configuration);
            m_statusTreeView.ImageList = statImageList;

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
            {
                Program.CommunicationObject.Logging_Log("Gui Started");
            }

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

                m_iconEx = new NotifyIconEx();
                m_iconEx.ContextMenu = m_iconMenu;
                m_iconEx.DoubleClick += new EventHandler(iconEx_DoubleClick);
                m_iconEx.Visible = true;

                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                {
                    m_iconEx.Icon = this.Icon = iba.Properties.Resources.connectedIcon;
                    m_iconEx.Text = iba.Properties.Resources.niConnected;
                }
                else
                {
                    m_startButton.Enabled = m_stopButton.Enabled = false;
                    m_iconEx.Icon = this.Icon = iba.Properties.Resources.disconnectedIcon;
                    m_iconEx.Text = iba.Properties.Resources.niDisconnected;
                }
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
        }

        private bool m_actualClose = false;

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            SaveRightPaneControl();
            if (!m_actualClose && Program.RunsWithService != Program.ServiceEnum.NOSERVICE)
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
                WindowState = FormWindowState.Minimized;
                ShowInTaskbar = false;
                e.Cancel = true;
                return;
            }
            
            string s = String.IsNullOrEmpty(m_filename) ? "not set" : m_filename;
            Profiler.ProfileString(false, "LastState", "LastSavedFile", ref s, "not set");
            LogData.Data.Logger.Close();

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && Program.CommunicationObject.TestConnection())
            {
                Program.CommunicationObject.Logging_Log("Gui Stopped");
                Program.CommunicationObject.ForwardEvents = false;
                TaskManager.Manager.IdCounter = ConfigurationData.IdCounter;
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
        }

        public void fromStatusToConfiguration()
        {
            ConfigurationData dat = (m_statusTreeView.SelectedNode.Tag as StatusTreeItemData).StatusData.CorrConfigurationData;
            foreach (TreeNode confCandidate in m_configTreeView.Nodes)
            {
                ConfigurationTreeItemData dat2 = (confCandidate.Tag as ConfigurationTreeItemData);
                if (dat2 != null && dat2.ConfigurationData.ID == dat.ID)
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
                        if ((statCandidate.Tag as StatusTreeItemData).StatusData.CorrConfigurationData == dat)
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
					doSelection(m_statusTreeView.SelectedNode, "Status");
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
                m_EntriesNumericUpDown1.Value = LogData.Data.MaxRows;
            }
            else if (m_navBar.SelectedPane == m_watchdogPane)
            {
                SaveRightPaneControl();
                Control ctrl = propertyPanes["watchdogControl"] as Control;
                if (ctrl == null) ctrl = new WatchdogControl();
                SetRightPaneControl(ctrl as Control, iba.Properties.Resources.watchdogTitle, TaskManager.Manager.WatchDogData.Clone());
                watchdogToolStripMenuItem.Enabled = false; 
                pasteToolStripMenuItem.Enabled = false;
                copyToolStripMenuItem.Enabled = false;
                cutToolStripMenuItem.Enabled = false;
                deleteToolStripMenuItem.Enabled = false;
                statusToolStripMenuItem.Enabled = true;
                configurationToolStripMenuItem.Enabled = true;
                loggingToolStripMenuItem.Enabled = true;
            }
        }
        
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            string returnvalue = "";
            Profiler.ProfileString(true, "LastState", "LastSavedFile", ref returnvalue, "not set");
            if (returnvalue != "not set" && Program.RunsWithService != Program.ServiceEnum.CONNECTED) loadFromFile(returnvalue,true);
            loadConfigurations();
            loadStatuses();
            if (WindowState == FormWindowState.Minimized)
                Hide();
            UpdateButtons();
        }

        private TreeNode CreateConfigurationNode(ConfigurationData confIt)
        {
            TreeNode confNode = new TreeNode(confIt.Name, 0, 0);
            MainForm.strikeOutNodeText(confNode, ! confIt.Enabled);
            
            confNode.Tag = new ConfigurationTreeItemData(this, confIt);

            foreach (TaskData task in confIt.Tasks)
            {
                TreeNode taskNode = null;
                if (task is ReportData)
                {
                    taskNode = new TreeNode(task.Name, 1, 1);
                    taskNode.Tag = new ReportTreeItemData(this, task as ReportData);
                }
                else if (task is ExtractData)
                {
                    taskNode = new TreeNode(task.Name, 2, 2);
                    taskNode.Tag = new ExtractTreeItemData(this, task as ExtractData);
                }
                else if (task is BatchFileData)
                {
                    taskNode = new TreeNode(task.Name, 3, 3);
                    taskNode.Tag = new BatchFileTreeItemData(this, task as BatchFileData);
                }
                else
                {
                    taskNode = new TreeNode(task.Name, 4, 4);
                    taskNode.Tag = new CopyTaskTreeItemData(this, task as CopyMoveTaskData);
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
                ConfigurationData newData = new ConfigurationData(iba.Properties.Resources.newConfigurationName);
                TaskManager.Manager.AddConfiguration(newData);
            }
            List<ConfigurationData> confs = TaskManager.Manager.Configurations;
            foreach (ConfigurationData confIt in confs)
                m_configTreeView.Nodes.Add(CreateConfigurationNode(confIt));

            //add the new Configuration node
            TreeNode newConfNode = new TreeNode(iba.Properties.Resources.addConfigurationText,5, 5);
            newConfNode.ForeColor = Color.Blue;
            newConfNode.Tag = new NewConfigurationTreeItemData(this);
            m_configTreeView.Nodes.Add(newConfNode); 
            m_configTreeView.EndUpdate();
            m_configTreeView.SelectedNode = m_configTreeView.Nodes[0];
            UpdateButtons();
        }

        private void loadStatuses()
        {
            m_statusTreeView.BeginUpdate();
            m_statusTreeView.Nodes.Clear();
            List<StatusData> stats = TaskManager.Manager.Statuses;
            foreach (StatusData statIt in stats)
            {
                TreeNode statNode = new TreeNode(statIt.CorrConfigurationData.Name,0, 0);
                statNode.Tag = new StatusTreeItemData(this as IPropertyPaneManager, statIt);
                MainForm.strikeOutNodeText(statNode, !statIt.CorrConfigurationData.Enabled);
                m_statusTreeView.Nodes.Add(statNode);
            }
            m_statusTreeView.EndUpdate();
            m_statusTreeView.SelectedNode = m_statusTreeView.Nodes[0];
        }

        private void OnStatusTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            using (WaitCursor wait = new WaitCursor())
            {
                TreeNode node = e.Node;
                doSelection(node, "Status");
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
            
            Control ctrl = propertyPanes[what + "Control"] as Control;
            switch (what)
            {
                case "Status":
                    {
                        if (m_navBar.SelectedPane != m_statusPane) return;
                        if (ctrl == null) ctrl = new StatusControl();
                        StatusTreeItemData data = node.Tag as StatusTreeItemData;
                        SetRightPaneControl(ctrl, title, data.StatusData);
                        pasteToolStripMenuItem.Enabled = false;
                        copyToolStripMenuItem.Enabled = false;
                        cutToolStripMenuItem.Enabled = false;
                        deleteToolStripMenuItem.Enabled = false;
                        break;
                    }
                case "Configuration":
                    {
                        if (m_navBar.SelectedPane != m_configPane) return;
                        // Ctrl will never be zero here as the Configurationcontrol is initialised in constructor
                        if (ctrl == null) ctrl = new ConfigurationControl();
                        ConfigurationTreeItemData data = node.Tag as ConfigurationTreeItemData;
                        SetRightPaneControl(ctrl, title, data.ConfigurationData);
                        pasteToolStripMenuItem.Enabled = m_cd_copy != null;
                        bool started = TaskManager.Manager.GetStatus(data.ConfigurationData.ID).Started;
                        if (m_task_copy != null && !m_confCopiedMostRecent)
                            pasteToolStripMenuItem.Enabled = !started;
                        copyToolStripMenuItem.Enabled = true;
                        cutToolStripMenuItem.Enabled = !started;
                        deleteToolStripMenuItem.Enabled = !started;
                        break;
                    }
                case "BatchFile":
                    {
                        if (m_navBar.SelectedPane != m_configPane) return;
                        if (ctrl == null) ctrl = new BatchFileControl();
                        BatchFileTreeItemData data = node.Tag as BatchFileTreeItemData;
                        bool started = TaskManager.Manager.GetStatus(data.BatchFileData.ParentConfigurationData.ID).Started;
                        SetRightPaneControl(ctrl, title, data.BatchFileData);
                        pasteToolStripMenuItem.Enabled = (m_task_copy != null && !started);
                        copyToolStripMenuItem.Enabled = true;
                        cutToolStripMenuItem.Enabled = !started;
                        deleteToolStripMenuItem.Enabled = !started;
                        break;
                    }
                case "Report":
                    {
                        if (m_navBar.SelectedPane != m_configPane) return;
                        if (ctrl == null) ctrl = new ReportControl();
                        ReportTreeItemData data = node.Tag as ReportTreeItemData;
                        bool started = TaskManager.Manager.GetStatus(data.ReportData.ParentConfigurationData.ID).Started;
                        SetRightPaneControl(ctrl, title, data.ReportData);
                        pasteToolStripMenuItem.Enabled = (m_task_copy != null && !started);
                        copyToolStripMenuItem.Enabled = true;
                        cutToolStripMenuItem.Enabled = !started;
                        deleteToolStripMenuItem.Enabled = !started;
                        break;
                    }
                case "Extract":
                    {
                        if (m_navBar.SelectedPane != m_configPane) return;
                        if (ctrl == null) ctrl = new ExtractControl();
                        ExtractTreeItemData data = node.Tag as ExtractTreeItemData;
                        bool started = TaskManager.Manager.GetStatus(data.ExtractData.ParentConfigurationData.ID).Started;
                        SetRightPaneControl(ctrl, title, data.ExtractData);
                        pasteToolStripMenuItem.Enabled = (m_task_copy != null && !started);
                        copyToolStripMenuItem.Enabled = true;
                        cutToolStripMenuItem.Enabled = !started;
                        deleteToolStripMenuItem.Enabled = !started;
                        break;
                    }
                case "CopyTask":
                    {
                        if (m_navBar.SelectedPane != m_configPane) return;
                        if (ctrl == null) ctrl = new CopyControl();
                        CopyTaskTreeItemData data = node.Tag as CopyTaskTreeItemData;
                        bool started = TaskManager.Manager.GetStatus(data.CopyTaskData.ParentConfigurationData.ID).Started;
                        SetRightPaneControl(ctrl, title, data.CopyTaskData);
                        pasteToolStripMenuItem.Enabled = (m_task_copy != null && !started);
                        copyToolStripMenuItem.Enabled = true;
                        cutToolStripMenuItem.Enabled = !started;
                        deleteToolStripMenuItem.Enabled = !started;
                        break;
                    }

                default:
                    break;
            }
            propertyPanes[what + "Control"] = ctrl;
        }

        private void OnConfigurationTreeViewAfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;
            using (WaitCursor wait = new WaitCursor())
            {
                if (node.Tag is NewConfigurationTreeItemData)
                {
                    SaveRightPaneControl();
                    //code to create new configuration
                    ConfigurationData newData = new ConfigurationData(iba.Properties.Resources.newConfigurationName);
                    new SetNextName(newData);
                    TaskManager.Manager.AddConfiguration(newData);
                    m_configTreeView.BeginUpdate();
                    m_configTreeView.Nodes.Insert(m_configTreeView.Nodes.Count - 1, CreateConfigurationNode(newData));
                    //loadConfigurations();
                    m_configTreeView.EndUpdate();
                    node = m_configTreeView.Nodes[m_configTreeView.Nodes.Count - 2];
                    m_configTreeView.SelectedNode = node;
                    loadStatuses();
                    UpdateButtons();
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
                    pane.SaveData();
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

            //Remove old control
            if (bOldControl && (newControl != m_rightPane.Controls[0]))
                m_rightPane.Controls.RemoveAt(0);
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
            if ((e.KeyCode != Keys.Delete) || (m_configTreeView.SelectedNode == null))
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
                    msg = String.Format(iba.Properties.Resources.deleteConfigurationQuestion, node.Text);
                else if (node.Tag is BatchFileTreeItemData)
                    msg = String.Format(iba.Properties.Resources.deleteBatchfileQuestion, node.Text,node.Parent.Text);
                else if (node.Tag is ReportTreeItemData)
                    msg = String.Format(iba.Properties.Resources.deleteReportQuestion, node.Text, node.Parent.Text);
                else if (node.Tag is ExtractTreeItemData)
                    msg = String.Format(iba.Properties.Resources.deleteExtractQuestion, node.Text, node.Parent.Text);
                else if (node.Tag is CopyTaskTreeItemData)
                    msg = String.Format(iba.Properties.Resources.deleteCopyTaskQuestion, node.Text, node.Parent.Text);

                DialogResult res = MessageBox.Show(this, msg,
                    iba.Properties.Resources.deleteTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (res != DialogResult.Yes)
                    return;
            }
            //Delete node in tree
            if (node.Tag is ConfigurationTreeItemData)
            {
                if (TaskManager.Manager.GetStatus((node.Tag as ConfigurationTreeItemData).ConfigurationData.ID).Started)
                    return;
                int newIndex = node.Index - 1;
                m_configTreeView.SelectedNode = null;
                TaskManager.Manager.RemoveConfiguration((node.Tag as ConfigurationTreeItemData).ConfigurationData);
                m_configTreeView.Nodes.Remove(node);
                if ((newIndex < m_configTreeView.Nodes.Count) && (newIndex >= 0))
                    m_configTreeView.SelectedNode = m_configTreeView.Nodes[newIndex];
                else
                    m_configTreeView.SelectedNode = m_configTreeView.Nodes[0];
            }
            else if (node.Tag is NewConfigurationTreeItemData)
            {
                //should never happen you are here
            }
            else
            {
                TreeNode nextNode = node.NextNode;
                if (nextNode == null) nextNode = node.PrevNode;
                if (nextNode == null) nextNode = node.Parent;
                ConfigurationData confParent = (node.Parent.Tag as ConfigurationTreeItemData).ConfigurationData;
                if (TaskManager.Manager.GetStatus(confParent.ID).Started) return;

                m_configTreeView.SelectedNode = null;
                TaskData task = null;
                ReportTreeItemData rti = node.Tag as ReportTreeItemData;
                if (rti != null) task = rti.ReportData;
                else
                {
                    ExtractTreeItemData eti = node.Tag as ExtractTreeItemData;
                    if (eti != null) task = eti.ExtractData;
                    else
                    {
                        BatchFileTreeItemData bti = node.Tag as BatchFileTreeItemData;
                        if (bti != null)
                            task = bti.BatchFileData;
                        else
                        {
                            CopyTaskTreeItemData cti = node.Tag as CopyTaskTreeItemData;
                            if (cti != null)
                                task = cti.CopyTaskData;
                        }
                    }
                }
                confParent.Tasks.Remove(task);
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    TaskManager.Manager.ReplaceConfiguration(confParent);
                node.Remove();
                m_configTreeView.SelectedNode = nextNode;
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
                if (node.Tag is ReportTreeItemData)
                    m_task_copy = (node.Tag as ReportTreeItemData).ReportData.Clone() as ReportData;
                else if (node.Tag is BatchFileTreeItemData)
                    m_task_copy = (node.Tag as BatchFileTreeItemData).BatchFileData.Clone() as BatchFileData;
                else if (node.Tag is BatchFileTreeItemData)
                    m_task_copy = (node.Tag as BatchFileTreeItemData).BatchFileData.Clone() as BatchFileData;
                else if (node.Tag is ExtractTreeItemData)
                    m_task_copy = (node.Tag as ExtractTreeItemData).ExtractData.Clone() as ExtractData;
                else if (node.Tag is CopyTaskTreeItemData)
                    m_task_copy = (node.Tag as CopyTaskTreeItemData).CopyTaskData.Clone() as CopyMoveTaskData;
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
                loadStatuses();
                UpdateButtons();
            }
            else if (node.Tag is ConfigurationTreeItemData && m_cd_copy != null && m_confCopiedMostRecent)
            {
                SaveRightPaneControl();
                int index = node.Index;
                //code to create new configuration
                new SetNextName(m_cd_copy);
                TaskManager.Manager.AddConfiguration(m_cd_copy);
                m_configTreeView.BeginUpdate();
                TreeNode tn = CreateConfigurationNode(m_cd_copy);
                m_configTreeView.Nodes.Insert(index, tn);
                tn.EnsureVisible();
                //loadConfigurations();
                m_configTreeView.EndUpdate();
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
                    taskNode = new TreeNode(m_task_copy.Name, 1, 1);
                    taskNode.Tag = new ReportTreeItemData(this, m_task_copy as ReportData);
                }
                else if (m_task_copy is ExtractData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, 2, 2);
                    taskNode.Tag = new ExtractTreeItemData(this, m_task_copy as ExtractData);
                }
                else if (m_task_copy is BatchFileData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, 3, 3);
                    taskNode.Tag = new BatchFileTreeItemData(this, m_task_copy as BatchFileData);
                }
                else if (m_task_copy is CopyMoveTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, 4, 4);
                    taskNode.Tag = new CopyTaskTreeItemData(this, m_task_copy as CopyMoveTaskData);
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
            else if (!(node.Tag is NewConfigurationTreeItemData) && m_task_copy != null)
            {
                SaveRightPaneControl();
                ConfigurationData origData = (node.Parent.Tag as ConfigurationTreeItemData).ConfigurationData;
                m_task_copy.ParentConfigurationData = origData;
                TreeNode taskNode = null;
                new SetNextName(m_task_copy);
                if (m_task_copy is ReportData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, 1, 1);
                    taskNode.Tag = new ReportTreeItemData(this, m_task_copy as ReportData);
                }
                else if (m_task_copy is ExtractData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, 2, 2);
                    taskNode.Tag = new ExtractTreeItemData(this, m_task_copy as ExtractData);
                }
                else if (m_task_copy is BatchFileData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, 3, 3);
                    taskNode.Tag = new BatchFileTreeItemData(this, m_task_copy as BatchFileData);
                }
                else if (m_task_copy is CopyMoveTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, 4, 4);
                    taskNode.Tag = new CopyTaskTreeItemData(this, m_task_copy as CopyMoveTaskData);
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
                int index = node.Index;
                origData.Tasks.Insert(index, m_task_copy);
                node.Parent.Nodes.Insert(index, taskNode);
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
            menuImages.Images.Add(iba.Properties.Resources.cut);
            menuImages.Images.Add(iba.Properties.Resources.copy);
            menuImages.Images.Add(iba.Properties.Resources.paste);
            menuImages.Images.Add(iba.Properties.Resources.report_running);
            menuImages.Images.Add(iba.Properties.Resources.extract_running);
            menuImages.Images.Add(iba.Properties.Resources.batchfile_running);
            menuImages.Images.Add(iba.Properties.Resources.copydat_running);
            m_menuItems = new MenuCommand[] 
			{
                new MenuCommand(iba.Properties.Resources.deleteTitle,il.List, MyImageList.Delete, Shortcut.Del,	new EventHandler(OnDeleteMenuItem)),
                new MenuCommand(iba.Properties.Resources.collapseTitle,il.List, -1, Shortcut.None,	new EventHandler(OnCollapseAllMenuItem)),
                new MenuCommand(iba.Properties.Resources.cutTitle,menuImages,0,Shortcut.CtrlX,new EventHandler(OnCutMenuItem)),
                new MenuCommand(iba.Properties.Resources.copyTitle,menuImages,1,Shortcut.CtrlC,new EventHandler(OnCopyMenuItem)),
                new MenuCommand(iba.Properties.Resources.pasteTitle,menuImages,2,Shortcut.CtrlV,new EventHandler(OnPasteMenuItem)),
                new MenuCommand(iba.Properties.Resources.NewTaskTitle,il.List, -1, Shortcut.None),            
                new MenuCommand(iba.Properties.Resources.NewReportTitle,menuImages,3,Shortcut.None, new EventHandler(OnNewReportMenuItem)),
                new MenuCommand(iba.Properties.Resources.NewExtractTitle,menuImages,4,Shortcut.None, new EventHandler(OnNewExtractMenuItem)),
                new MenuCommand(iba.Properties.Resources.NewBatchfileTitle,menuImages,5,Shortcut.None, new EventHandler(OnNewBatchfileMenuItem)),
                new MenuCommand(iba.Properties.Resources.NewCopyTaskTitle,menuImages,6,Shortcut.None, new EventHandler(OnNewCopyTaskMenuItem))
            };
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewReport]);
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewExtract]);
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewBatchfile]);
            m_menuItems[(int)MenuItemsEnum.NewTask].MenuCommands.Add(m_menuItems[(int)MenuItemsEnum.NewCopyTask]);
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
            NewCopyTask = 9
        }

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
                    started = TaskManager.Manager.GetStatus((node.Parent.Tag as ConfigurationTreeItemData).ConfigurationData.ID).Started;
                else if (node.Tag is ConfigurationTreeItemData)
                    started = TaskManager.Manager.GetStatus((node.Tag as ConfigurationTreeItemData).ConfigurationData.ID).Started;

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
                }
                else if (data is NewConfigurationTreeItemData)
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
                    foreach (MenuCommand mc2 in mc.MenuCommands)
                        mc2.Tag = node;
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
            TreeNode newNode = new TreeNode(report.Name, 1, 1);
            newNode.Tag = new ReportTreeItemData(this, report);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
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
            TreeNode newNode = new TreeNode(extract.Name, 2, 2);
            newNode.Tag = new ExtractTreeItemData(this, extract);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
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
            TreeNode newNode = new TreeNode(bat.Name, 3, 3);
            newNode.Tag = new BatchFileTreeItemData(this, bat);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
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
            TreeNode newNode = new TreeNode(cop.Name, 4, 4);
            newNode.Tag = new CopyTaskTreeItemData(this, cop);
            node.Nodes.Add(newNode);
            newNode.EnsureVisible();
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
            TreeNode node = m_configTreeView.SelectedNode; 
            copyNode(node);
            Delete(node, false);
        }

        private void copyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = m_configTreeView.SelectedNode;
            copyNode(node);
        }

        private void pasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = m_configTreeView.SelectedNode;
            pasteNode(node);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TreeNode node = m_configTreeView.SelectedNode;
            Delete(node);
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
                    XmlSerializer mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));
                    // To write to a file, create a StreamWriter object.
                    try
                    {
                        using (StreamWriter myWriter = new StreamWriter(m_filename))
                        {
                            ibaDatCoordinatorData dat = new ibaDatCoordinatorData(
                                TaskManager.Manager.WatchDogData,
                                TaskManager.Manager.Configurations, 
                                LogData.Data.FileName,
                                LogData.Data.MaxRows
                                );
                            mySerializer.Serialize(myWriter, dat);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(iba.Properties.Resources.SaveFileProblem + " " + ex.Message);
                    }
                    if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                        Program.CommunicationObject.SaveConfigurations();
                }
        }

        private string TextToSave()
        {
            SaveRightPaneControl();
            XmlSerializer mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));
            StringBuilder sb = new StringBuilder();
            try
            {
                using (StringWriter myWriter = new StringWriter(sb))
                {
                    ibaDatCoordinatorData dat = new ibaDatCoordinatorData(
                        TaskManager.Manager.WatchDogData,
                        TaskManager.Manager.Configurations,
                        LogData.Data.FileName,
                        LogData.Data.MaxRows
                        );
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
            DialogResult result = m_openFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;
                
            SaveRightPaneControl();
            loadFromFile(m_openFileDialog.FileName,true);
            loadConfigurations();
            loadStatuses();
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
                        TaskManager.Manager.ReplaceWatchdogData(dat.WatchDogData);
                        TaskManager.Manager.WatchDog.Settings = dat.WatchDogData;
                        confs = dat.Configurations;
                        if (LogData.Data.FileName != dat.Logfile)
                            LogData.OpenFromFile(dat.Logfile);
                        LogData.Data.MaxRows = dat.LogItemCount;
                    }
                    m_filename = filename;
                    this.Text = m_filename + " - ibaDatCoordinator v" + GetType().Assembly.GetName().Version.ToString(3);
                    foreach (ConfigurationData dat in confs) dat.relinkChildData();

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
                            if (dat.AutoStart) TaskManager.Manager.StartConfiguration(dat.ID);
                        }
                }
            }
            catch (Exception ex)
            {
                if (!beSilent) MessageBox.Show(iba.Properties.Resources.OpenFileProblem + "  " + ex.Message);
                return false;
            }
            return true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult res = MessageBox.Show(this, iba.Properties.Resources.saveQuestion,
                    iba.Properties.Resources.closing, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            switch (res)
            {
                case DialogResult.Cancel:
                    return;
                case DialogResult.Yes:
                    saveToolStripMenuItem_Click(sender, e);
                    break;
                case DialogResult.No:
                    break;
            }
            m_filename = null;
            clearAllConfigurations();
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

        private void m_rightPane_Enter(object sender, EventArgs e)
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

        #region buttons in the mainform
        private void m_newLogFileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog.CheckFileExists = false;
            m_openFileDialog.Filter = "logfiles (*.txt)|*.txt";
            DialogResult result = m_openFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            if (Program.RunsWithService != Program.ServiceEnum.CONNECTED)
                LogData.NewFromFile(m_openFileDialog.FileName);
            else
                Program.CommunicationObject.Logging_setNewFile(m_openFileDialog.FileName);
        }
        
        private void m_loadLogfileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog.CheckFileExists = true;
            m_openFileDialog.Filter = "logfiles (*.txt)|*.txt";
            DialogResult result = m_openFileDialog.ShowDialog();
            if (result != DialogResult.OK) return;
            if (Program.RunsWithService != Program.ServiceEnum.CONNECTED)
                LogData.OpenFromFile(m_openFileDialog.FileName);
            else
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(DoOpenFromFile), m_openFileDialog.FileName);               
        }

        private void DoOpenFromFile(object arg)
        {
            Program.CommunicationObject.Logging_openFromFile((string)arg);
        }
        
        private void m_EntriesNumericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            LogData.Data.MaxRows = (int) m_EntriesNumericUpDown1.Value;
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
            StatusBarLabel.Text = ""; //clear any errors on restart
            TaskManager.Manager.StartAllConfigurations();
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
            bool allstarted = true;
            bool allstopped = true;
            
            foreach (ConfigurationData data in TaskManager.Manager.Configurations)
            {
                bool started = TaskManager.Manager.GetStatus(data.ID).Started;
                allstarted = allstarted && started;
                allstopped = allstopped && !started;
            }
            m_startButton.Enabled = !allstarted;
            m_stopButton.Enabled = !allstopped;
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
            else if (targetNode.Tag is NewConfigurationTreeItemData)
            {
                e.Effect = DragDropEffects.None;
            }
            else if ((targetNode == draggedNode) && (draggedNode.Tag is ConfigurationTreeItemData))
            {
                e.Effect = DragDropEffects.Copy;
            }
            else if ((targetNode == draggedNode) && !(draggedNode.Tag is ConfigurationTreeItemData))
            {
                if (TaskManager.Manager.GetStatus((draggedNode.Parent.Tag as ConfigurationTreeItemData).ConfigurationData.ID).Started)
                    e.Effect = DragDropEffects.None; //do not modify a running configuration    
                else e.Effect = DragDropEffects.Copy;
            }
            else if (!(draggedNode.Tag is ConfigurationTreeItemData))
            {
                bool moveAllowed = !TaskManager.Manager.GetStatus((draggedNode.Parent.Tag as ConfigurationTreeItemData).ConfigurationData.ID).Started;
                bool placeAllowed = false;

                if (targetNode.Tag is ConfigurationTreeItemData)
                    placeAllowed = !TaskManager.Manager.GetStatus((targetNode.Tag as ConfigurationTreeItemData).ConfigurationData.ID).Started;
                else
                    placeAllowed = !TaskManager.Manager.GetStatus((targetNode.Parent.Tag as ConfigurationTreeItemData).ConfigurationData.ID).Started;

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
                if (!(draggedNode.Tag is NewConfigurationTreeItemData))
                    DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Move);
            }

        }
        #endregion


        #region Service related methods
        public void OnStartService()
        {
            //startservice dialog
            bool result = false;
            using (StartServiceDialog ssd = new StartServiceDialog())
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
                TaskManager.Manager = null; //remove previous taskmanager so it does not stay
                // alive during the online session
                Program.RunsWithService = Program.ServiceEnum.CONNECTED;
                //initialise with configurations;
                fillManagerFromTree(TaskManager.Manager);
                foreach (ConfigurationData dat in TaskManager.Manager.Configurations)
                {
                    if (dat.AutoStart) TaskManager.Manager.StartConfiguration(dat.ID);
                }
                LogData.Data.Logger.Close();
                Program.CommunicationObject.Logging_setEventForwarder(new EventForwarder());
                ConfigurationData.IdCounter = TaskManager.Manager.IdCounter;
                GridViewLogger gv = LogData.Data.Logger.Children[0] as GridViewLogger;
                LogData.InitializeLogger(gv.Grid, gv.LogControl, true);
                Program.CommunicationObject.Logging_Log(iba.Properties.Resources.logServiceStarted);
                m_iconEx.Icon = this.Icon = iba.Properties.Resources.connectedIcon;
                m_iconEx.Text = iba.Properties.Resources.niConnected;
                m_startButton.Enabled = m_stopButton.Enabled = true;
                if (m_navBar.SelectedPane == m_statusPane)
                    loadStatuses();
                else if (m_navBar.SelectedPane == m_configPane)
                    loadConfigurations();
            }
        }

        public void OnStopService()
        {
            //stopService dialog
            Program.CommunicationObject.StoppingService = true;
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
                Program.CommunicationObject.handleBrokenConnection();
                LogData.Data.Logger.Log(Logging.Level.Info, iba.Properties.Resources.logServiceStopped);
                if (m_navBar.SelectedPane == m_statusPane)
                    loadStatuses();
                else if (m_navBar.SelectedPane == m_configPane)
                    loadConfigurations();
            }
            Program.CommunicationObject.StoppingService = false;
        }

        public void fillManagerFromTree(TaskManager m)
        {
            foreach (TreeNode t in m_configTreeView.Nodes)
            {
                if (t.Tag is ConfigurationTreeItemData)
                {
                    m.AddConfiguration((t.Tag as ConfigurationTreeItemData).ConfigurationData);
                }
            }
        }

        public void OnExternalActivate()
        {
            Show();
            Activate();
            WindowState = FormWindowState.Normal;
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
        private NotifyIconEx m_iconEx;
        public NotifyIconEx NotifyIcon
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
