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
using iba.HD.Client;
using iba.Utilities.Forms;

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
        public static readonly int HDEVENTTASK_INDEX = 13;
		public static readonly int OPCUA_WRITERTASK_INDEX = 14;
        public static readonly int SNMP_WRITERTASK_INDEX = 15;
        public static readonly int UPLOADTASK_INDEX = 16;
        public static readonly int KAFKAWRITERTASK_INDEX = 17;
        public static readonly int DATATRANSFER_TASK_INDEX = 18;
        // add here any additional indices for new tasks, increase the next numbers
        public static readonly int UNKNOWNTASK_INDEX = 19;
        public static readonly int NEWCONF_INDEX = 20;
        public static readonly int NEW_ONETIME_CONF_INDEX = 21;
        public static readonly int NEW_SCHEDULED_CONF_INDEX = 22;
        public static readonly int NEW_EVENT_CONF_INDEX = 23;
        public static readonly int CUSTOMTASK_INDEX = 24;
        public static readonly int NR_TASKS = 15;

        private QuitForm m_quitForm;

        static MainForm()
        {
            Crypt.InitializeKeys(
                new byte[] { 12, 34, 179, 69, 231, 92 },
                new byte[] {
                    0xC4, 0x52, 0xC0, 0xC4, 0x9B, 0x80, 0xA6, 0xE8,
                    0x51, 0xCA, 0xE1, 0x24, 0x40, 0x6D, 0xBE, 0x89,
                    0xEF, 0xFA, 0xB1, 0x7B, 0x45, 0x0F, 0x13, 0x54,
                    0xE9, 0x8F, 0x84, 0xD0, 0x45, 0x81, 0x24, 0xF7
               });
        }

        public MainForm()
        {
            DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            defaultLookAndFeel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            defaultLookAndFeel.LookAndFeel.UseWindowsXPTheme = true;

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
            CreateToolStripItemsIcons();
            
            m_watchdogPane.LargeImage = m_watchdogPane.SmallImage = Icons.Gui.All.Images.TcpIp();
            // added by kolesnik - begin
            m_snmpPane.LargeImage = m_snmpPane.SmallImage = Icons.Gui.All.Images.Snmp();
            m_opcUaPane.LargeImage = m_opcUaPane.SmallImage = Icons.Gui.All.Images.Opcua();
            // added by kolesnik - end
            m_dataTransferPane.LargeImage = m_dataTransferPane.SmallImage = Bitmap.FromHicon(Properties.Resources.DataTransferIcon.Handle);
            m_statusPane.LargeImage = m_statusPane.SmallImage = Icons.Gui.All.Images.Table();
            m_configPane.LargeImage = m_configPane.SmallImage = Icons.Gui.All.Images.Batch();
            m_loggingPane.LargeImage = m_loggingPane.SmallImage = Icons.Gui.All.Images.Table();
            m_settingsPane.LargeImage = m_settingsPane.SmallImage = Icons.Gui.All.Images.ToolboxService();
            m_toolTip.SetToolTip(m_startButton, Properties.Resources.toolTipStartAll);
            m_toolTip.SetToolTip(m_stopButton, Properties.Resources.toolTipStopAll);

            ImageList confsImageList = new ImageList();
            confsImageList.Images.Add(Icons.Gui.All.Images.FileDat());
            confsImageList.Images.Add(Icons.Gui.All.Images.ScheduleCalendarDate());
            confsImageList.Images.Add(Icons.Gui.All.Images.FlashFilledGreen());
            confsImageList.Images.Add(Properties.Resources.onetimeconfiguration);
            confsImageList.Images.Add(Icons.Gui.All.Images.Report2());
            confsImageList.Images.Add(Icons.Gui.All.Images.DatabaseImport());
            confsImageList.Images.Add(Icons.Gui.All.Images.TerminalCode());
            confsImageList.Images.Add(Icons.Gui.All.Images.Copy());
            confsImageList.Images.Add(Icons.Gui.All.Images.Condition());
            confsImageList.Images.Add(Icons.Gui.All.Images.DatabaseRefresh());
            confsImageList.Images.Add(Icons.Gui.All.Images.PauseOutline());
            confsImageList.Images.Add(Icons.Gui.All.Images.CleanErase());
            confsImageList.Images.Add(Icons.Gui.All.Images.SplitDivide());
            confsImageList.Images.Add(Icons.Gui.All.Images.HdFlash());
			confsImageList.Images.Add(Icons.Gui.All.Images.Opcua());
            confsImageList.Images.Add(Icons.Gui.All.Images.Snmp());
            confsImageList.Images.Add(Icons.Gui.All.Images.Extract());
            confsImageList.Images.Add(Icons.Gui.All.Images.ApacheKafka());
            confsImageList.Images.Add(Properties.Resources.DataTransferIcon.ToBitmap());
            confsImageList.Images.Add(Properties.Resources.img_question);
            confsImageList.Images.Add(Properties.Resources.configuration_new);
            confsImageList.Images.Add(Properties.Resources.onetime_configuration_new);
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(Properties.Resources.scheduled_configuration_new.ToBitmap()));
            confsImageList.Images.Add(GraphicsUtilities.PaintOnWhite(Properties.Resources.event_configuration_new.ToBitmap()));
            m_configTreeView.ImageList = confsImageList;
            
            UpdateImageListConfTree();
                        
            ImageList confsImageList2 = new();
            confsImageList2.Images.Add(Properties.Resources.greenarrow);
            confsImageList2.Images.Add(Properties.Resources.redarrow); 
            confsImageList2.Images.Add(Properties.Resources.redarrow1);
            m_configTreeView.StateImageList = confsImageList2;

            ImageList statImageList = new();
            statImageList.Images.Add(Icons.Gui.All.Images.FileDat());
            statImageList.Images.Add(Icons.Gui.All.Images.ScheduleCalendarDate());
            statImageList.Images.Add(Icons.Gui.All.Images.FlashFilledGreen());
            statImageList.Images.Add(Properties.Resources.onetimeconfiguration);
            statImageList.Images.Add(Properties.Resources.brokenfile);
            m_statusTreeView.ImageList = statImageList;

            CreateLanguageMenuItems();

            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                m_menuStrip.Items.Remove(serviceToolStripMenuItem);
                this.Icon = Properties.Resources.standalone;
            }
            m_navBar.SelectedPane = m_configPane;

            statusImgConnectedInsecure = Icons.Gui.All.Images.LockOpenOrange();
            statusImgConnectedSecure = Icons.Gui.All.Images.LockClosedOrange();
            statusImgDisconnected = Properties.Resources.img_networkError.ToBitmap();
            statusImgStandalone = Properties.Resources.img_server;

            iba.Controls.AppCertificatesControl.Init();
        }

        private void CreateToolStripItemsIcons()
        {
            newToolStripMenuItem.Image = Icons.Gui.All.Images.FileDocumentOutlineEmpty();
            openToolStripMenuItem.Image = Icons.Gui.All.Images.FolderOpen();
            saveToolStripMenuItem.Image = Icons.Gui.Standard.Images.Save();
                
            copyToolStripMenuItem.Image = Icons.Gui.Standard.Images.Copy();
            cutToolStripMenuItem.Image = Icons.Gui.Standard.Images.Cut();
            pasteToolStripMenuItem.Image = Icons.Gui.Standard.Images.Paste();
            deleteToolStripMenuItem.Image = Icons.Gui.Standard.Images.Delete();

            configurationToolStripMenuItem.Image = Icons.Gui.All.Images.Batch();
            dataTransferToolStripMenuItem.Image = Bitmap.FromHicon(Properties.Resources.DataTransferIcon.Handle);
            statusToolStripMenuItem.Image = Icons.Gui.All.Images.Table();
            loggingToolStripMenuItem.Image = Icons.Gui.All.Images.Table();
            watchdogToolStripMenuItem.Image = Icons.Gui.All.Images.TcpIp();
            snmpToolStripMenuItem.Image = Icons.Gui.All.Images.Snmp();
            opcUaToolStripMenuItem.Image = Icons.Gui.All.Images.Opcua();
            settingsToolStripMenuItem.Image = Icons.Gui.All.Images.ToolboxService();
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
            DatCoProfiler.ProfileString(false, "LastState", "LastSavedFile", ref s, "not set");

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && Program.CommunicationObject != null && Program.CommunicationObject.TestConnection())
            {
                TryUploadChangedFiles();

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

                if (IsConfigModified())
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

                TaskManager.Manager.UninitializeLicenseManager();
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

        public void MoveToSettigsTab()
        {
            m_navBar.SelectedPane = m_settingsPane;
        }

        public void MoveToTaskByName(string name)
        {
            if (name == OpcUaData.certificateUserName)
            {
                m_navBar.SelectedPane = m_opcUaPane;
                return;
            }

            if (name == DataTransferData.certificateUserName)
            {
                m_navBar.SelectedPane = m_dataTransferPane;
                return;
            }

            foreach (TreeNode topLevelNode in m_configTreeView.Nodes)
                foreach (TreeNode jobLevelNode in topLevelNode.Nodes)
                    foreach (TreeNode taskLevelNode in jobLevelNode.Nodes)
                    {
                        TaskData dat = ((taskLevelNode.Tag as TreeItemData)?.DataSource as TaskData);
                        if (dat != null && dat.Name == name)
                        {
                            m_navBar.SelectedPane = m_configPane;
                            m_configTreeView.SelectedNode = taskLevelNode;
                            return;
                        }
                    }
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
                    for(int index = 0; index < m_statusTreeView.Nodes.Count; index++)
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
                        for(int index = 0; index < m_statusTreeView.Nodes.Count; index++ )
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

                SetRightPaneControl(ctrl, Properties.Resources.snmpTitle,
                    TaskManager.Manager.SnmpData?.Clone());
                EnableAllButOnePaneToolStripMenuItems(snmpToolStripMenuItem);
                DisableCopyPasteCutDeleteMenuItems();
            }
            else if (m_navBar.SelectedPane == m_opcUaPane)
            {
                SaveRightPaneControl();
                Control ctrl = propertyPanes["opcUaControl"] as Control;
                if (ctrl == null)
                {
                    ctrl = new OpcUaControl();
                    propertyPanes["opcUaControl"] = ctrl;
                }

                SetRightPaneControl(ctrl, Properties.Resources.opcUaTitle, 
                    TaskManager.Manager.OpcUaData?.Clone());
                EnableAllButOnePaneToolStripMenuItems(opcUaToolStripMenuItem);  
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
            else if(m_navBar.SelectedPane == m_dataTransferPane)
            {
                SaveRightPaneControl();
                Control ctrl = propertyPanes["dataTransferControl"] as Control;
                if (ctrl == null)
                {
                    ctrl = new DataTransferControl();
                    propertyPanes["dataTransferControl"] = ctrl;
                }

                SetRightPaneControl(ctrl, Properties.Resources.dataTransferTitle, 
                    TaskManager.Manager.DataTransferData?.Clone());
                    
                EnableAllButOnePaneToolStripMenuItems(dataTransferToolStripMenuItem);
                DisableCopyPasteCutDeleteMenuItems();
            }

            //certificates table update timer should work only if Settings tab is visible
            if (Program.RunsWithService != Program.ServiceEnum.NOSERVICE)
            {
                if (m_navBar.SelectedPane == m_settingsPane)
                {
                    var ctrl = propertyPanes["settingsControl"] as ServiceSettingsControl;
                    ctrl.IsCertificateTableTimerTicking = true;
                }
                else
                {
                    var ctrl = propertyPanes["settingsControl"] as ServiceSettingsControl;
                    if (ctrl != null)
                        ctrl.IsCertificateTableTimerTicking = false;
                }
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
            opcUaToolStripMenuItem.Enabled = true;
            dataTransferToolStripMenuItem.Enabled = true;
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
            if (m_navBar.SelectedPane == m_snmpPane)
            {
                SnmpControl pane = propertyPanes["snmpControl"] as SnmpControl;
                pane?.LoadData(TaskManager.Manager.SnmpData.Clone(), this);
            }
            else if (m_navBar.SelectedPane == m_opcUaPane)
            {
                OpcUaControl pane = propertyPanes["opcUaControl"] as OpcUaControl;
                pane?.LoadData(TaskManager.Manager.OpcUaData.Clone(), this); 
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

        protected override async void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            FormStateSerializer.LoadSettings(this, "MainForm");

            SetRenderer();
            UpdateConnectionStatus();
            SetupHelp();

            //Initialize licenses in case app is running standalone
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
                TaskManager.Manager.InitializeLicenseManager();

            string returnvalue = "";
            DatCoProfiler.ProfileString(true, "LastState", "LastSavedFile", ref returnvalue, "not set");
            if (returnvalue != "not set" && Program.RunsWithService == Program.ServiceEnum.NOSERVICE) 
                loadFromFile(returnvalue,true);
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
                TaskManager.Manager.SnmpWorkerInit();
                TaskManager.Manager.OpcUaWorkerInit();
                // added by kolesnik - end
                TaskManager.Manager.DataTransferWorkerInit();
            }
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
                else if (task is HDCreateEventTaskData)
                {
                    taskNode = new TreeNode(task.Name, HDEVENTTASK_INDEX, HDEVENTTASK_INDEX);
                    taskNode.Tag = new HDCreateEventTaskTreeItemData(this, task as HDCreateEventTaskData);
                }
                else if (task is OpcUaWriterTaskData)
                {
                    taskNode = new TreeNode(task.Name, OPCUA_WRITERTASK_INDEX, OPCUA_WRITERTASK_INDEX);
                    taskNode.Tag = new OpcUaWriterTaskTreeItemData(this, task as OpcUaWriterTaskData);
                }
                else if (task is SnmpWriterTaskData)
                {
                    taskNode = new TreeNode(task.Name, SNMP_WRITERTASK_INDEX, SNMP_WRITERTASK_INDEX);
                    taskNode.Tag = new SnmpWriterTaskTreeItemData(this, task as SnmpWriterTaskData);
                }
                else if (task is KafkaWriterTaskData)
                {
                    taskNode = new TreeNode(task.Name, KAFKAWRITERTASK_INDEX, KAFKAWRITERTASK_INDEX);
                    taskNode.Tag = new KafkaWriterTaskTreeItemData(this, task as KafkaWriterTaskData);
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
                else if (task.GetType() == typeof(UploadTaskData))
                {
                    taskNode = new TreeNode(task.Name, UPLOADTASK_INDEX, UPLOADTASK_INDEX);
                    taskNode.Tag = new UploadTaskTreeItemData(this, task as UploadTaskData);
                }
                else if (task.GetType() == typeof(DataTransferTaskData))
                {
                    taskNode = new TreeNode(task.Name, DATATRANSFER_TASK_INDEX, DATATRANSFER_TASK_INDEX);
                    taskNode.Tag = new DataTransferTaskTreeItemData(this, task as DataTransferTaskData);
                }
                else if(task is ICustomTaskData cust)
                {
                    int index = GetCustomTaskImageIndex(cust);
                    taskNode = new TreeNode(cust.Name, index, index);
                    taskNode.Tag = new CustomTaskTreeItemData(this, cust);
                }
                else
                {
                    Debug.Assert(false, "Unknown task type");
                    throw new Exception($"Unknown task type: {task.GetType()}");
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
                    case TaskData.WhenToDo.AFTER_1st_FAILURE_DAT:
					case TaskData.WhenToDo.AFTER_1st_FAILURE_TASK:
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
            m_configTreeView.Nodes.Add(new TreeNode(iba.Properties.Resources.DatTriggeredJobsNodeParent, CONFIGURATION_INDEX, CONFIGURATION_INDEX));
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
            try
            {
                m_bLoadingConfigurations = true;
                m_configTreeView.SelectedNode = firstNode;
            }
            finally
            {
                m_bLoadingConfigurations = false;
            }
            UpdateTreePositions();
            UpdateButtons();
        }

        private void loadStatuses()
        {
            m_statusTreeView.BeginUpdate();
            m_statusTreeView.Nodes.Clear();
            m_statusTreeView.Nodes.Add(new TreeNode(iba.Properties.Resources.DatTriggeredJobsNodeParent, CONFIGURATION_INDEX, CONFIGURATION_INDEX));
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
                case "UploadTask":
                case "DataTransferTask":
                case "CustomTask":
                case "HDCreateEventTask":
				case "OPCUAWriterTask":
                case "SNMPWriterTask":
                case "KafkaWriterTask":
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
                    newData.SetNextName();
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

        private bool m_bLoadingConfigurations = false;

        public void SetRightPaneControl(Control newControl, string title, object datasource)
        {
            //Save data from old control
            bool bOldControl = this.m_rightPane.Controls.Count > 0;
            if (bOldControl)
            {
                Debug.Assert(m_rightPane.Controls.Count == 1, "Only 1 control allowed in rightpane");
                IPropertyPane pane = m_rightPane.Controls[0] as IPropertyPane;
                if (pane != null && !m_bLoadingConfigurations)
                {
                    pane.SaveData();
                    pane.LeaveCleanup();
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
                    else if (node.Tag is CleanupTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteCleanupTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is SplitterTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteSplitterTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is HDCreateEventTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteCreateEventTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is UploadTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteUploadTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is CustomTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteCustomTaskQuestion,
                        (((CustomTaskTreeItemData)(node.Tag)).DataSource as ICustomTaskData).Plugin.NameInfo,
                            node.Text, node.Parent.Text);
                    else if (node.Tag is OpcUaWriterTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteOPCUATastQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is SnmpWriterTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteSNMPTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is KafkaWriterTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteKafkaTaskQuestion, node.Text, node.Parent.Text);
                    else if (node.Tag is DataTransferTaskTreeItemData)
                        msg = String.Format(iba.Properties.Resources.deleteDataTransferTaskQuestion, node.Text, node.Parent.Text);
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
                InformExtMonDataAboutTreeStructureChange();
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
                m_cd_copy.SetNextName();
                foreach (TaskData t in m_cd_copy.Tasks)
                    t.SetNextName();
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
                m_cd_copy.SetNextName();
                foreach (TaskData t in m_cd_copy.Tasks)
                    t.SetNextName();
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
                m_task_copy.SetNextName();
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
                else if (m_task_copy is HDCreateEventTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, HDEVENTTASK_INDEX, HDEVENTTASK_INDEX);
                    taskNode.Tag = new HDCreateEventTaskTreeItemData(this, m_task_copy as HDCreateEventTaskData);
                }
                else if (m_task_copy is OpcUaWriterTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, OPCUA_WRITERTASK_INDEX, OPCUA_WRITERTASK_INDEX);
                    taskNode.Tag = new OpcUaWriterTaskTreeItemData(this, m_task_copy as OpcUaWriterTaskData);
                }
                else if (m_task_copy is SnmpWriterTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, SNMP_WRITERTASK_INDEX, SNMP_WRITERTASK_INDEX);
                    taskNode.Tag = new SnmpWriterTaskTreeItemData(this, m_task_copy as SnmpWriterTaskData);
                }
                else if (m_task_copy is KafkaWriterTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, KAFKAWRITERTASK_INDEX, KAFKAWRITERTASK_INDEX);
                    taskNode.Tag = new KafkaWriterTaskTreeItemData(this, m_task_copy as KafkaWriterTaskData);
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
                else if (m_task_copy.GetType() == typeof(UploadTaskData))
                {
                    taskNode = new TreeNode(m_task_copy.Name, UPLOADTASK_INDEX, UPLOADTASK_INDEX);
                    taskNode.Tag = new UploadTaskTreeItemData(this, m_task_copy as UploadTaskData);
                }
                else if (m_task_copy.GetType() == typeof(DataTransferTaskData))
                {
                    taskNode = new TreeNode(m_task_copy.Name, DATATRANSFER_TASK_INDEX, DATATRANSFER_TASK_INDEX);
                    taskNode.Tag = new DataTransferTaskTreeItemData(this, m_task_copy as DataTransferTaskData);
                }
                else if (m_task_copy is ICustomTaskData cust)
                {
                    int index = GetCustomTaskImageIndex(cust);
                    taskNode = new TreeNode(m_task_copy.Name, index, index);
                    taskNode.Tag = new CustomTaskTreeItemData(this,cust);
                }

                MainForm.strikeOutNodeText(taskNode, !m_task_copy.Enabled);
                switch (m_task_copy.WhenToExecute)
                {
                    case TaskData.WhenToDo.AFTER_1st_FAILURE_DAT:
					case TaskData.WhenToDo.AFTER_1st_FAILURE_TASK:
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
                ConfigurationData origData = ((ConfigurationTreeItemData)node.Parent.Tag).ConfigurationData;
                m_task_copy.ParentConfigurationData = origData;
                if (!TestTaskCount(origData))
                    return;
                TreeNode taskNode = null;
                m_task_copy.SetNextName();
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
                else if (m_task_copy is HDCreateEventTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, HDEVENTTASK_INDEX, HDEVENTTASK_INDEX);
                    taskNode.Tag = new HDCreateEventTaskTreeItemData(this, m_task_copy as HDCreateEventTaskData);
                }
                else if (m_task_copy is OpcUaWriterTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, OPCUA_WRITERTASK_INDEX, OPCUA_WRITERTASK_INDEX);
                    taskNode.Tag = new OpcUaWriterTaskTreeItemData(this, m_task_copy as OpcUaWriterTaskData);
                }
                else if (m_task_copy is SnmpWriterTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, SNMP_WRITERTASK_INDEX, SNMP_WRITERTASK_INDEX);
                    taskNode.Tag = new SnmpWriterTaskTreeItemData(this, m_task_copy as SnmpWriterTaskData);
                }
                else if (m_task_copy is KafkaWriterTaskData)
                {
                    taskNode = new TreeNode(m_task_copy.Name, KAFKAWRITERTASK_INDEX, KAFKAWRITERTASK_INDEX);
                    taskNode.Tag = new KafkaWriterTaskTreeItemData(this, m_task_copy as KafkaWriterTaskData);
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
                else if (m_task_copy.GetType() == typeof(UploadTaskData))
                {
                    taskNode = new TreeNode(m_task_copy.Name, UPLOADTASK_INDEX, UPLOADTASK_INDEX);
                    taskNode.Tag = new UploadTaskTreeItemData(this, m_task_copy as UploadTaskData);
                }
                else if (m_task_copy.GetType() == typeof(DataTransferTaskData))
                {
                    taskNode = new TreeNode(m_task_copy.Name, DATATRANSFER_TASK_INDEX, DATATRANSFER_TASK_INDEX);
                    taskNode.Tag = new DataTransferTaskTreeItemData(this, m_task_copy as DataTransferTaskData);
                }
                else if (m_task_copy is ICustomTaskData cust)
                {
                    int index = GetCustomTaskImageIndex(cust);
                    taskNode = new TreeNode(m_task_copy.Name, index, index);
                    taskNode.Tag = new CustomTaskTreeItemData(this, cust);
                }


                MainForm.strikeOutNodeText(taskNode, !m_task_copy.Enabled);
                switch (m_task_copy.WhenToExecute)
                {
                    case TaskData.WhenToDo.AFTER_1st_FAILURE_TASK:
					case TaskData.WhenToDo.AFTER_1st_FAILURE_DAT:
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
            menuImages.Images.Add(Icons.Gui.All.Images.Cut());
            menuImages.Images.Add(Icons.Gui.All.Images.Copy());
            menuImages.Images.Add(Icons.Gui.All.Images.Paste());
            menuImages.Images.Add(Icons.Gui.All.Images.Report2());
            menuImages.Images.Add(Icons.Gui.All.Images.DatabaseImport());
            menuImages.Images.Add(Icons.Gui.All.Images.TerminalCode());
            menuImages.Images.Add(Icons.Gui.All.Images.Copy());
            menuImages.Images.Add(Icons.Gui.All.Images.Condition());
            menuImages.Images.Add(Icons.Gui.All.Images.DatabaseRefresh());
            menuImages.Images.Add(Icons.Gui.All.Images.PauseOutline());
            menuImages.Images.Add(Icons.Gui.All.Images.CleanErase());
            menuImages.Images.Add(Icons.Gui.All.Images.SplitDivide());
            menuImages.Images.Add(Icons.Gui.All.Images.HdFlash());
            menuImages.Images.Add(Icons.Gui.All.Images.Opcua());
            menuImages.Images.Add(Icons.Gui.All.Images.Snmp());
            menuImages.Images.Add(Icons.Gui.All.Images.ApacheKafka());
            menuImages.Images.Add(Icons.Gui.All.Images.Extract());
            menuImages.Images.Add(Properties.Resources.DataTransferIcon);

            int pluginsStartImageIndex = menuImages.Images.Count;
            List<PluginTaskInfo> filteredPlugins = PluginManager.Manager.PluginInfos.Where(a => !a.IsOutdated).ToList();
            foreach (PluginTaskInfo info in filteredPlugins)
                menuImages.Images.Add(info.Icon);

            int customcount = filteredPlugins.Count;
            m_menuItems = new ToolStripMenuItem[((int)MenuItemsEnum.NewCustomTask) + customcount];
            m_menuItems[(int)MenuItemsEnum.Delete] = new ToolStripMenuItem(Properties.Resources.deleteTitle, il.List.Images[MyImageList.Delete], OnDeleteMenuItem, Keys.Delete);
            m_menuItems[(int)MenuItemsEnum.CollapseAll] = new ToolStripMenuItem(Properties.Resources.collapseTitle, null,OnCollapseAllMenuItem);
            m_menuItems[(int)MenuItemsEnum.Cut] = new ToolStripMenuItem(Properties.Resources.cutTitle, menuImages.Images[0], OnCutMenuItem, Keys.X | Keys.Control);
            m_menuItems[(int)MenuItemsEnum.Copy] = new ToolStripMenuItem(Properties.Resources.copyTitle, menuImages.Images[1], OnCopyMenuItem, Keys.C | Keys.Control);
            m_menuItems[(int)MenuItemsEnum.Paste] = new ToolStripMenuItem(Properties.Resources.pasteTitle, menuImages.Images[2], OnPasteMenuItem, Keys.V | Keys.Control);
            m_menuItems[(int)MenuItemsEnum.NewTask] = new ToolStripMenuItem(Properties.Resources.NewTaskTitle, null, null, Properties.Resources.NewTaskTitle);
            

            m_menuItems[(int)MenuItemsEnum.NewTask] = new ToolStripMenuItem(Properties.Resources.NewTaskTitle);
            m_menuItems[(int)MenuItemsEnum.NewReport] = new ToolStripMenuItem(Properties.Resources.NewReportTitle, menuImages.Images[3], OnNewReportMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewExtract] = new ToolStripMenuItem(Properties.Resources.NewExtractTitle,  menuImages.Images[4], OnNewExtractMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewBatchfile] = new ToolStripMenuItem(Properties.Resources.NewBatchfileTitle, menuImages.Images[5], OnNewBatchfileMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewCopyTask] = new ToolStripMenuItem(Properties.Resources.NewCopyTaskTitle, menuImages.Images[6], OnNewCopyTaskMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewIfTask] = new ToolStripMenuItem(Properties.Resources.NewIfTaskTitle, menuImages.Images[7], OnNewIfTaskMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewUpdateDataTask] = new ToolStripMenuItem(Properties.Resources.NewUpdateDataTaskTitle, menuImages.Images[8], OnNewUpdateDataTaskMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewPauseTask] = new ToolStripMenuItem(Properties.Resources.NewPauseTaskTitle, menuImages.Images[9], OnNewPauseTaskMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewCleanupTask] = new ToolStripMenuItem(Properties.Resources.NewCleanupTaskTitle, menuImages.Images[10], OnNewCleanupTaskMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewSplitterTask] = new ToolStripMenuItem(Properties.Resources.NewSplitterTaskTitle, menuImages.Images[11], OnNewSplitterTaskMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewHDCreateEventTask] = new ToolStripMenuItem(Properties.Resources.NewHDCreateEventTaskTitle, menuImages.Images[12], OnNewHDCreateEventTaskMenuItem);
			m_menuItems[(int)MenuItemsEnum.NewOPCUATask] = new ToolStripMenuItem(Properties.Resources.NewOpcUaTaskTitle, menuImages.Images[13], OnNewOPCUATaskMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewSNMPTask] = new ToolStripMenuItem(Properties.Resources.NewSnmpTaskTitle, menuImages.Images[14], OnNewSNMPTaskMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewKafkaTask] = new ToolStripMenuItem(Properties.Resources.NewKafkaTaskTitle, menuImages.Images[15], OnNewKafkaTaskMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewUploadTask] = new ToolStripMenuItem(Properties.Resources.NewUploadTaskTitle, menuImages.Images[16], OnNewUploadTaskMenuItem);
            m_menuItems[(int)MenuItemsEnum.NewDataTransferTask] = new ToolStripMenuItem(Properties.Resources.NewDataTransferTaskTitle, menuImages.Images[17], OnNewDataTransferTaskMenuItem);

            for (int i = 0; i < filteredPlugins.Count; i++)
            {
                PluginTaskInfo info = filteredPlugins[i];
                string title = String.Format(iba.Properties.Resources.NewCustomTaskTitle, info.Name);
                m_menuItems[i + (int)MenuItemsEnum.NewCustomTask] = new ToolStripMenuItem(title, menuImages.Images[pluginsStartImageIndex+i], new EventHandler(OnNewCustomTaskMenuItem));
            }
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown = new ContextMenuStrip();

            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewReport]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewExtract]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewBatchfile]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewCopyTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewUploadTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewIfTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewUpdateDataTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewPauseTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewCleanupTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewSplitterTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewHDCreateEventTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewOPCUATask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewSNMPTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewKafkaTask]);
            m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(m_menuItems[(int)MenuItemsEnum.NewDataTransferTask]);

            for (int i = 0; i < filteredPlugins.Count; i++)
            {
                var item = m_menuItems[i + (int)MenuItemsEnum.NewCustomTask];
                item.Tag = new Pair<TreeNode, PluginTaskInfo>(null, filteredPlugins[i]);
                if (item != null)
                    m_menuItems[(int)MenuItemsEnum.NewTask].DropDown.Items.Add(item);
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
            NewHDCreateEventTask = 15,
            NewOPCUATask = 16,
            NewSNMPTask = 17,
            NewUploadTask = 18,
            NewKafkaTask = 19,
            NewDataTransferTask = 20,
			NewCustomTask = 21
        }


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
                    m_menuItems[(int)MenuItemsEnum.NewIfTask].Enabled = Utility.VersionCheck.CheckIbaAnalyzerVersion("5.3.4");
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
                    foreach(ToolStripMenuItem mc2 in mc.DropDown.Items)
                    {
                        if (mc2.Tag is Pair<TreeNode, PluginTaskInfo> pair)
                            pair.First = node;
                        else
                            mc2.Tag = node;
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

        /// <summary>
        /// Gets parent <see cref="TreeNode"/> and parent job data from menuItem's tag;
        /// Checks if there's a room for a new item (max limit of child items not reached).
        /// </summary>
        /// <returns>true if another task can be added</returns>
        private bool AddNewTaskPreHelper(ToolStripMenuItem menuItem, out TreeNode node, out ConfigurationData confData)
        {
            node = (TreeNode)menuItem.Tag;
            confData = ((ConfigurationTreeItemData)node.Tag).ConfigurationData;

            // return true if there's enough room for another new task,
            // false otherwise (limit reached, cannot add more tasks)
            return TestTaskCount(confData);
        }

        /// <summary>
        /// Creates a <see cref="TreeNode"/> for the task, adds it to job's <see cref="TreeNode"/>,
        /// adds the task to <see cref="ConfigurationData"/> and does other important post-processing like:
        /// replacing configuration in TaskManager, informing ExtMonData about changes, etc.
        /// </summary>
        private void AddNewTaskPostHelper(ConfigurationData confData, TreeNode parentNode, TaskData taskData, int imageIndex, TreeItemData treeItemData)
        {
            taskData.SetNextName();
            confData.Tasks.Add(taskData);
            TreeNode newNode = new TreeNode(taskData.Name, imageIndex, imageIndex)
            {
                Tag = treeItemData
            };
            parentNode.Nodes.Add(newNode);
            newNode.EnsureVisible();
            if (confData.AdjustDependencies()) AdjustFrontIcons(confData);

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(confData);
            
            InformExtMonDataAboutTreeStructureChange();
        }

        /// <summary>
        /// Informs ExtMon subsystem about node tree structure change
        /// (actually, is needed only in Standalone mode, see comments inside)
        /// </summary>
        internal static void InformExtMonDataAboutTreeStructureChange()
        {
            // in case of Standalone inform ExtMon data immediately;
            // otherwise do nothing right now, because this will be done on Job Start or Update
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                // inform ExtMonData that job/task tree structure was changed
                ExtMonData.Instance.InvalidateTree();
            }
        }

        private void OnNewReportMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new ReportData(parentConfData);
            var treeItemData = new ReportTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, REPORTTASK_INDEX, treeItemData);
        }

        private void OnNewExtractMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new ExtractData(parentConfData);
            var treeItemData = new ExtractTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, REPORTTASK_INDEX, treeItemData);
        }

        private void OnNewBatchfileMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new BatchFileData(parentConfData);
            var treeItemData = new BatchFileTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, BATCHFILETASK_INDEX, treeItemData);
        }

        private void OnNewIfTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new IfTaskData(parentConfData);
            var treeItemData = new IfTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, IFTASK_INDEX, treeItemData);
        }

        private void OnNewUpdateDataTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new UpdateDataTaskData(parentConfData);
            var treeItemData = new UpdateDataTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, UPDATEDATATASK_INDEX, treeItemData);
        }

        private void OnNewCopyTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new CopyMoveTaskData(parentConfData);
            var treeItemData = new CopyTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, COPYTASK_INDEX, treeItemData);
        }

        private void OnNewPauseTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new PauseTaskData(parentConfData);
            var treeItemData = new PauseTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, PAUSETASK_INDEX, treeItemData);
        }

        private void OnNewHDCreateEventTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new HDCreateEventTaskData(parentConfData);
            var treeItemData = new HDCreateEventTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, HDEVENTTASK_INDEX, treeItemData);
        }

        private void OnNewOPCUATaskMenuItem(object sender, EventArgs e)
		{
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new OpcUaWriterTaskData(parentConfData);
            var treeItemData = new OpcUaWriterTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, OPCUA_WRITERTASK_INDEX, treeItemData);
        }

        private void OnNewSNMPTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new SnmpWriterTaskData(parentConfData);
            var treeItemData = new SnmpWriterTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, SNMP_WRITERTASK_INDEX, treeItemData);
        }

        private void OnNewKafkaTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new KafkaWriterTaskData(parentConfData);
            var treeItemData = new KafkaWriterTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, KAFKAWRITERTASK_INDEX, treeItemData);
        }

        private void OnNewSplitterTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new SplitterTaskData(parentConfData);
            var treeItemData = new SplitterTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, SPLITTERTASK_INDEX, treeItemData);
        }

        private void OnNewUploadTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new UploadTaskData(parentConfData);
            var treeItemData = new UploadTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, UPLOADTASK_INDEX, treeItemData);
        }
        private void OnNewDataTransferTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new DataTransferTaskData(parentConfData);
            var treeItemData = new DataTransferTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, DATATRANSFER_TASK_INDEX, treeItemData);
        }

        private void OnNewCleanupTaskMenuItem(object sender, EventArgs e)
        {
            // get parent job from tags, check free room
            if (!AddNewTaskPreHelper((ToolStripMenuItem)sender, out var parentNode, out var parentConfData))
                return;

            var taskData = new CleanupTaskData(parentConfData);
            var treeItemData = new CleanupTaskTreeItemData(this, taskData);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, CLEANUPTASK_INDEX, treeItemData);
        }

        private void OnNewCustomTaskMenuItem(object sender, EventArgs e)
        {
            ToolStripMenuItem mc = (ToolStripMenuItem)sender;
            var p = (Pair<TreeNode, PluginTaskInfo>)mc.Tag;
            TreeNode parentNode = p.First;
            PluginTaskInfo info = p.Second;
            
            ConfigurationData parentConfData = ((ConfigurationTreeItemData)parentNode.Tag).ConfigurationData;
            if (!TestTaskCount(parentConfData))
                return;

            TaskData taskData;
            if (info is PluginTaskInfoUNC)
                taskData = new CustomTaskDataUNC(parentConfData, info);
            else
                taskData = new CustomTaskData(parentConfData, info);

            ICustomTaskData iCust = (ICustomTaskData)taskData;
            int imageIndex = GetCustomTaskImageIndex(iCust);
            var treeItemData = new CustomTaskTreeItemData(this, iCust);

            // create tree node and do other things common for any task
            AddNewTaskPostHelper(parentConfData, parentNode, taskData, imageIndex, treeItemData);
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
        private void opcUaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_navBar.SelectedPane = m_opcUaPane;
        }
        // added by kolesnik - end
        private void dataTransferToolStripMenuItem_Click(object sender, EventArgs e)
        {
            m_navBar.SelectedPane = m_dataTransferPane;
        }
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
            if (sender != null && !Utility.Crypt.CheckPassword(this)) 
                return;

            if (m_filename == null)
            {
                saveAsToolStripMenuItem_Click(null, e);
                return;
            }

            using (WaitCursor wait = new WaitCursor())
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
                    ibaDatCoordinatorData dat = ibaDatCoordinatorData.Create(TaskManager.Manager);

                    using (StreamWriter myWriter = new StreamWriter(m_filename))
                    {
                        mySerializer.Serialize(myWriter, dat);
                    }

                    SaveConfigForModifiedCheck(dat, mySerializer);
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

        private string savedConfigAsText;

        private bool IsConfigModified()
        {
            string currentConfig = null;
            try
            {
                ibaDatCoordinatorData dat = ibaDatCoordinatorData.Create(TaskManager.Manager);
                XmlSerializer mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));
                currentConfig = SaveConfigForModifiedCheck(dat, mySerializer);

                return currentConfig != savedConfigAsText;
            }
            catch(Exception)
            {
                return false;
            }
        }

        private string SaveConfigForModifiedCheck(ibaDatCoordinatorData dat, XmlSerializer ser)
        {
            StringBuilder sb = new StringBuilder();
            using (StringWriter myWriter = new StringWriter(sb))
            {
                //Force encryption to use a fixed IV so that the encrypted data will not change
                EncryptionService.UseFixedIV.Value = true;
                try
                {
                    ser.Serialize(myWriter, dat);
                }
                finally
                {
                    EncryptionService.UseFixedIV.Value = false;
                }
            }
            return sb.ToString();
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
                    ibaDatCoordinatorData data = ibaDatCoordinatorData.SerializeFromFile(filename);

                    List<string> missingPlugins = TaskManager.Manager.CheckPluginsAvailable(data.PluginList());
                    if (missingPlugins != null && missingPlugins.Count > 0)
                    {
                        MessageBox.Show(iba.Properties.Resources.uploadFileProblem + " "  + iba.Properties.Resources.missingPlugins + Environment.NewLine + String.Join(Environment.NewLine, PluginManager.Manager.FullNames(missingPlugins))
                            , "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                    List<ConfigurationData> confs = data.ApplyToManager(TaskManager.Manager, Program.ClientName);

                    m_filename = filename;
                    this.Text = m_filename + " - ibaDatCoordinator v" + DatCoVersion.GetVersion();

                    savedConfigAsText = SaveConfigForModifiedCheck(data, new XmlSerializer(typeof(ibaDatCoordinatorData)));

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

                if (!beSilent) 
                    MessageBox.Show(iba.Properties.Resources.OpenFileProblem + "  " + ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return false;
            }
            return true;
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword(this)) 
                return;

            SaveRightPaneControl();

            if (IsConfigModified())
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
            savedConfigAsText = null;

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
            using (var form = new AboutForm())
            {
                form.ProductNameAndVersion = $"ibaDatCoordinator v{DatCoVersion.GetVersion()}";
                form.ProductIcon = iba.Properties.Resources.About_Image_80x80;

                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && Program.CommunicationObject != null && Program.CommunicationObject.TestConnection())
                {
                    form.Caption1 = "ibaDatCoordinatorService";
                    form.Value1 = Program.CommunicationObject.GetVersion();
                }

                form.Caption2 = "ibaAnalyzer";
                form.Value2 = GetAnalyzerVersion();

                form.Caption3 = "ibaFiles";
                form.Value3 = GetibaFilesVersion();

                form.IbaLicenseFile = "License_iba-software.rtf";
                form.ThirdPartyLicensesFile = "LicenseInformation.txt";

                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }

            // local functions
            string GetAnalyzerVersion()
            {
                string analyzerVersion  = "?";
                try
                {
                    var ibaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                    analyzerVersion = ibaAnalyzer.GetVersion().Remove(0, 12);
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaAnalyzer);
                }
                catch (Exception)
                {
                    // ignore exception
                }
                return analyzerVersion;
            }
            string GetibaFilesVersion()
            {
                string ibaFilesVersion = "?";
                try
                {
                    using (var ibaFile = new ibaFilesLiteDotNet.IbaFileReader())
                    {
                        ibaFilesVersion = ibaFile.GetVersion();
                        ibaFile.Dispose();
                    }
                }
                catch
                {
                    // ignore exception
                }

                return ibaFilesVersion;
            }
        }

        private void saveInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword(this)) return;
            if (!String.IsNullOrEmpty(m_filename)) saveToolStripMenuItem_Click(null, null);

            var gen = new SupportFileGenerator(this, m_filename);
            gen.SaveInformation();
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
        //        readmeFile = Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), "License_ibaDatCoordinator.pdf");
        //        System.Diagnostics.Process.Start(readmeFile);
        //    }
        //    catch (System.Exception ex)
        //    {
        //        MessageBox.Show(ex.Message + " " + readmeFile);
        //    };
        //}

        private void supportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string readmeFile = "";
            try
            {
                readmeFile = Path.Combine(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), "support.htm");
                System.Diagnostics.Process.Start(readmeFile);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message + " " + readmeFile);
            };
        }

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
                {
                    if (node.TreeView != null)
                        font = node.TreeView.Font;
                    else
                    {
                        using (TreeView dummytree = new TreeView())
                        {
                            font = dummytree.Font;
                        }
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

        private int GetCustomTaskImageIndex(ICustomTaskData cust)
        {
            string name = cust.Plugin.NameInfo;
            int index = PluginManager.Manager.PluginInfos.FindIndex(ti => ti.Name == name);
            if (index < 0)
                return UNKNOWNTASK_INDEX;

            return CUSTOMTASK_INDEX + index;
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
            InformExtMonDataAboutTreeStructureChange();
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

        private void TryUploadChangedFiles(ConfigurationData data)
        {
            if (!Program.ServiceIsLocal)
            {
                List<string> localFiles = new List<string>();
                List<string> remoteFiles = new List<string>();
                foreach (var task in data.Tasks)
                {
                    if (!(task is HDCreateEventTaskData createEventTask))
                        continue;

                    string localFile = Program.RemoteFileLoader.GetLocalPath(createEventTask.AnalysisFile);
                    if (Program.RemoteFileLoader.IsFileChangedLocally(localFile, createEventTask.AnalysisFile))
                    {
                        localFiles.Add(localFile);
                        remoteFiles.Add(createEventTask.AnalysisFile);
                    }
                }

                if (localFiles.Count > 0)
                {
                    if (MessageBox.Show(this, Properties.Resources.FileChanged_Upload, "ibaDatCoordinator", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    Cursor = Cursors.WaitCursor;

                    bool bStarted = TaskManager.Manager.IsJobStarted(data.Guid);
                    try
                    {
                        if (bStarted)
                            TaskManager.Manager.StopAndWaitForConfiguration(data);

                        if (!Program.RemoteFileLoader.UploadFiles(localFiles.ToArray(), remoteFiles.ToArray(), true, out Dictionary<string, string> errors))
                        {
                            StringBuilder sb = new StringBuilder();
                            foreach (var val in errors.Values)
                                sb.AppendLine(val);

                            throw new Exception(sb.ToString());
                        }

                        Cursor = Cursors.Default;
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show(this, ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    finally
                    {
                        try
                        {
                            if (bStarted)
                                TaskManager.Manager.StartConfiguration(data);
                        }
                        catch
                        { }
                    }
                }
            }
        }

        private void TryUploadChangedFiles()
        {
            List<ConfigurationData> cfgs = TaskManager.Manager.Configurations;
            if (cfgs != null)
            {
                foreach (var cfg in cfgs)
                    TryUploadChangedFiles(cfg);
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

                            TryUploadChangedFiles(data);
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
                    CommunicationObjectWrapper wrapper = new CommunicationObjectWrapper();

                    try
                    {

                        //Let's try connecting, this will throw in case the connection fails
                        int serverVersion = wrapper.Connect(Program.ServiceHost, Program.ServicePortNr);

                        LogData.Data.Logger.Log(Logging.Level.Debug, String.Format("Successfully connected to ibaDatCoordinator service at {0} (v{1} {2})",
                            Program.ServiceHost, wrapper.ServerVersion, wrapper.IsSecure ? "encrypted" : "unencrypted"));

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
                                        TryUploadChangedFiles();
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
                        LogData.Data.Logger.Log(Logging.Level.Debug, String.Format("Failed to connect to {0}:{1} with error {2}", Program.ServiceHost, Program.ServicePortNr, connEx.Message));
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

        Image statusImgConnectedInsecure;
        Image statusImgConnectedSecure;
        Image statusImgDisconnected;
        Image statusImgStandalone;

        public void UpdateConnectionStatus()
        {
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
            {
                if (!String.IsNullOrEmpty(Program.CommunicationObject.ServerVersion))
                    m_statusBarStripLabelConnection.Text = string.Format(iba.Properties.Resources.ConnectedToWithVersion, Program.ServiceHost, Program.CommunicationObject.ServerVersion);
                else
                    m_statusBarStripLabelConnection.Text = string.Format(iba.Properties.Resources.ConnectedTo, Program.ServiceHost);
                m_statusBarStripLabelConnection.Image = Program.CommunicationObject.IsSecure ? statusImgConnectedSecure : statusImgConnectedInsecure;
            }
            else if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                m_statusBarStripLabelConnection.Text = string.Format(iba.Properties.Resources.Disconnected, Program.ServiceHost);
                m_statusBarStripLabelConnection.Image = statusImgDisconnected;
            }
            else
            {
                m_statusBarStripLabelConnection.Text = Properties.Resources.StandaloneText;
                m_statusBarStripLabelConnection.Image = statusImgStandalone;
            }
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
                                MessageBoxButtons.YesNo, 
                                MessageBoxIcon.Question,
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
            catch(Exception ex)
            {
                if (IsDisposed)
                    return;

                LogData.Data?.Logger?.Log(Logging.Level.Debug, ex.ToString());
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
			if (TaskManager.Manager.Count == 0)
			{
				bool clean = TaskManager.Manager.ServiceRestartedClean; //nothing on server side, upload the minimum configuration of one
				TaskManager.Manager.ServiceRestartedClean = false;
				return !clean;
			}
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
            for (int index = 0; index < 4; index++)
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
            if (TaskManager.Manager.Count != count)
                return true; //added or removed
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
                helpProvider.SetHelpKeyword(this, "d9e4.html");

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
            if (!IsDisposed)
            {
                Show();
                Activate();
                WindowState = FormWindowState.Normal;
                FormStateSerializer.LoadSettings(this, "MainForm", true);
            }
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
            supportedCultures.Add(CultureInfo.GetCultureInfo("de"));
            supportedCultures.Add(CultureInfo.GetCultureInfo("en"));
            supportedCultures.Add(CultureInfo.GetCultureInfo("fr"));
			supportedCultures.Add(CultureInfo.GetCultureInfo("es"));

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

            DatCoProfiler.ProfileString(false, "Client", "Language", ref LanguageHelper.SelectedLanguage, "");
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
