using System.Collections.Generic;
using System.Windows.Forms;
using iba.HD.Common;
using iba.Data;
using iba.HD.Client;
using System;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using System.IO;
using iba.Processing;
using Microsoft.Win32;
using System.Diagnostics;
using iba.HD.Client.Interfaces;
using Crownwood.DotNetMagic.Controls;
using iba.Client.Archiver;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using iba.Utility;

namespace iba.Controls
{
    public partial class HDEventCreationTaskControl : UserControl, IPropertyPane
    {
        #region Members
        HDCreateEventTaskData m_data;
        IPropertyPaneManager m_manager;

        AnalyzerManager m_analyzerManager;
        RepositoryItemChannelTreeEdit m_pulseEditor, m_timeEditor, m_timeEditorOut, m_channelEditor, m_textEditor;
        TriggerControl triggerControl;

        AnalyzerTreeControl channelTree;
        AnalyzerTreeControl numericTree;

        bool bLoadingChannelTree;
        bool bReloadChannelTree;
        bool bResetChannelTree;
        bool bDisposeChannelTree;
        #endregion

        public HDEventCreationTaskControl()
        {
            InitializeComponent();

            ImageList list = new ImageList();
            list.ImageSize = new System.Drawing.Size(16, 16);
            list.TransparentColor = Color.Magenta;
            list.ColorDepth = ColorDepth.Depth24Bit;
            list.Images.AddStrip(iba.Properties.Resources.Toolbars16);

            m_ctrlServer.SetServerFeatures(new List<ReaderFeature>(1) { ReaderFeature.ComputedValue }, new List<WriterFeature>(1) { WriterFeature.ComputedValue });
            m_ctrlServer.StoreFilter = new List<HdStoreType> { HdStoreType.Event };
            m_ctrlServer.HideStoreSelection();
            m_ctrlServer.AllowCurrentWindowsUser = false;
            m_analyzerManager = new AnalyzerManager();

            m_pulseEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Expressions);
            m_pulseEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);
            m_pulseEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            m_timeEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Analog | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Infofields);
            m_timeEditor.AddSpecialNode(HDCreateEventTaskData.EndTime, Properties.Resources.EndTime, Properties.Resources.pausetask);
            m_timeEditor.AddSpecialNode(HDCreateEventTaskData.StartTime, Properties.Resources.StartTime, Properties.Resources.pausetask);
            m_timeEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            m_timeEditorOut = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Analog | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Infofields);
            m_timeEditorOut.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);
            m_timeEditorOut.AddSpecialNode(HDCreateEventTaskData.EndTime, Properties.Resources.EndTime, Properties.Resources.pausetask);
            m_timeEditorOut.AddSpecialNode(HDCreateEventTaskData.StartTime, Properties.Resources.StartTime, Properties.Resources.pausetask);
            m_timeEditorOut.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            m_channelEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Text | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Infofields);
            numericTree = m_channelEditor.ChannelTree;
            m_channelEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);
            m_channelEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            m_textEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Text | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Infofields);
            m_textEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);
            m_textEditor.AddSpecialNode(HDCreateEventTaskData.CurrentFileExpression, Properties.Resources.HDEventTask_ChannelProcessedFile, Properties.Resources.img_file);
            m_textEditor.AddSpecialNode(HDCreateEventTaskData.ClientIDExpression, Properties.Resources.HDEventTask_ChannelClientId, list.Images[10]);
            m_textEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;

            m_pulseEditor.ChannelTree.Invalidated += ChannelEditorModified;
            m_timeEditor.ChannelTree.Invalidated += ChannelEditorModified;
            m_timeEditorOut.ChannelTree.Invalidated += ChannelEditorModified;
            numericTree.Invalidated += ChannelEditorModified;
            m_textEditor.ChannelTree.Invalidated += ChannelEditorModified;

            triggerControl = new TriggerControl();
            triggerControl.SetPulseChannelEditor(m_pulseEditor, m_pulseEditor.ChannelTree);
            triggerControl.SetTimeChannelEditor(m_timeEditor, m_timeEditor.ChannelTree);
            triggerControl.SetTimeOutgoingChannelEditor(m_timeEditorOut, m_timeEditorOut.ChannelTree);

            m_ctrlEvent.DefaultNumericChannelValue = HDCreateEventTaskData.UnassignedExpression;
            m_ctrlEvent.DefaultTextChannelValue = HDCreateEventTaskData.UnassignedExpression;

            m_ctrlEvent.SetNumericChannelEditor(m_channelEditor, m_channelEditor.ChannelTree);
            m_ctrlEvent.SetTextChannelEditor(m_textEditor, m_textEditor.ChannelTree);
            channelTree = new AnalyzerTreeControl(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Text | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Infofields);
            m_ctrlEvent.ChannelTree = channelTree;
            m_ctrlEvent.SetChannelTreeCtrl(channelTree);

            m_toolTip.SetToolTip(m_btnOpenPDO, Properties.Resources.HDEventTask_ToolTip_OpenPDO);
            m_toolTip.SetToolTip(m_btnUploadPDO, Program.RunsWithService==Program.ServiceEnum.NOSERVICE?Properties.Resources.HDEventTask_ToolTip_UploadPDOStandAlone:Properties.Resources.HDEventTask_ToolTip_UploadPDO);

            m_toolTip.SetToolTip(m_btnTest, Properties.Resources.HDEventTask_ToolTip_Test);

            m_ctrlEvent.EventTrigger = triggerControl;
            m_ctrlServer.ServerSelectionChanged += (s, e) =>
            {
                if (m_ctrlServer.Reader.IsConnected())
                {
                    m_ctrlEvent.ReadOnly = false;

                    if (m_ctrlServer.Reader.UserManager.GetCurrentUser() is PdaClientUser user)
                    {
                        if (m_ctrlServer.Reader.UserManager.IsActive() && user.StoreRights[1].StoreRange == PdaClientUser.HdStoreRight.StoreRightRange.List)
                            m_ctrlEvent.StoreFilter = user.StoreRights[1].AllowedStores;
                        else if (!m_ctrlServer.Reader.UserManager.IsActive() || user.StoreRights[1].StoreRange == PdaClientUser.HdStoreRight.StoreRightRange.All)
                            m_ctrlEvent.StoreFilter = null;
                        m_ctrlEvent.RequestEditRightsHDServer(m_ctrlEvent.GetStoreNames());
                    }
                }
                else
                {
                    m_ctrlEvent.ReadOnly = true;
                    m_ctrlEvent.StoreFilter = null;
                }
            };
            m_ctrlEvent.InitializeEventConfig(m_ctrlServer.Reader, new List<string>(), null, false);
            m_ctrlEvent.EventWizard = new HDEventWizard(m_analyzerManager, GetPriorities());

            WindowsAPI.SHAutoComplete(m_tbPDO.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_tbDAT.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);


            bLoadingChannelTree = false;
            bResetChannelTree = false;
            bDisposeChannelTree = false;
        }

        public void ChannelEditorModified(object sender, EventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, EventArgs>(ChannelEditorModified), sender, args);
                return;
            }
            m_ctrlEvent.Invalidate(true);
        }

        #region Dispose
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<bool>(Dispose), disposing);
                return;
            }

            if (disposing)
            {
                bDisposeChannelTree = true;
                if (bLoadingChannelTree)
                    return;

                if (m_timeEditor != null)
                {

                    triggerControl.GrTime.RepositoryItems.Remove(m_timeEditor);
                    triggerControl.ColTime.ColumnEdit = null;
                    m_timeEditor.Dispose();
                    m_timeEditor = null;
                }
                if (m_timeEditorOut != null)
                {

                    triggerControl.GrTime.RepositoryItems.Remove(m_timeEditorOut);
                    triggerControl.ColTime.ColumnEdit = null;
                    m_timeEditorOut.Dispose();
                    m_timeEditorOut = null;
                }
                if (m_pulseEditor != null)
                {
                    triggerControl.GrPulse.RepositoryItems.Remove(m_pulseEditor);
                    triggerControl.ColPulse.ColumnEdit = null;

                    m_pulseEditor.Dispose();
                    m_pulseEditor = null;
                }

                if (m_textEditor != null)
                {
                    if (m_ctrlEvent != null)
                        m_ctrlEvent.SetTextChannelEditor(null, null);

                    m_textEditor.Dispose();
                    m_textEditor = null;
                }

                if (m_analyzerManager != null)
                {
                    m_analyzerManager.Dispose();
                    m_analyzerManager = null;
                }

                if (channelTree != null)
                {
                    m_ctrlEvent.SetChannelTreeCtrl(null);
                    channelTree.Dispose();
                    channelTree = null;
                }
                if (numericTree != null)
                {
                    m_ctrlEvent.SetChannelTreeCtrl(null);
                    numericTree.Invalidated -= ChannelEditorModified;
                    numericTree.Dispose();
                    numericTree = null;
                }

                if (m_channelEditor != null)
                {
                    if (m_ctrlEvent != null)
                        m_ctrlEvent.SetNumericChannelEditor(null, null);

                    m_channelEditor.Dispose();
                    m_channelEditor = null;
                }
                bDisposeChannelTree = false;

                components?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region IPropertyPane
        private void loadAnalyzerTreeDataTask()
        {

            if (bLoadingChannelTree)
                bReloadChannelTree = true;
            else
            {
                bLoadingChannelTree = true;

                Task.Factory.StartNew(() =>
                {
                    while (!bResetChannelTree && !bReloadChannelTree && !bDisposeChannelTree && channelTree != null && !channelTree.Load()) ;
                    int i = 0;
                    while (!bResetChannelTree && !bReloadChannelTree && !bDisposeChannelTree && m_analyzerManager != null && !m_analyzerManager.IsOpened && i < 40)
                    {
                        i++;
                        Thread.Sleep(500);
                    }
                    while (!bResetChannelTree && !bReloadChannelTree && !bDisposeChannelTree && numericTree != null && !numericTree.Load()) ;
                    while (!bResetChannelTree && !bReloadChannelTree && !bDisposeChannelTree && m_pulseEditor != null && !m_pulseEditor.ChannelTree.Load()) ;
                    while (!bResetChannelTree && !bReloadChannelTree && !bDisposeChannelTree && m_timeEditor != null && !m_timeEditor.ChannelTree.Load()) ;
                    while (!bResetChannelTree && !bReloadChannelTree && !bDisposeChannelTree && m_timeEditorOut != null && !m_timeEditorOut.ChannelTree.Load()) ;
                    while (!bResetChannelTree && !bReloadChannelTree && !bDisposeChannelTree && m_textEditor != null && !m_textEditor.ChannelTree.Load()) ;

                    bLoadingChannelTree = false;

                    if (bReloadChannelTree)
                    {
                        bReloadChannelTree = false;
                        loadAnalyzerTreeDataTask();
                    }

                    if (bResetChannelTree)
                        ResetChannelTrees();

                    if (bDisposeChannelTree)
                        Dispose(true);

                });
            }

        }


        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            //m_btnUploadPDO.Enabled = Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.RunsWithService == Program.ServiceEnum.CONNECTED;
            m_manager = manager;
            m_data = datasource as HDCreateEventTaskData;
            m_ctrlServer.LoadData(m_data.Server, m_data.ServerPort, false, m_data.Username, m_data.HDPassword, "");

            if (m_ctrlServer.Reader.IsConnected())
                m_ctrlEvent.ReadOnly = false;
            else
                m_ctrlEvent.ReadOnly = true;

            if (m_data.FullEventConfig?.Count > 0 && MessageBox.Show(this, iba.Properties.Resources.ReloadHdEvents,
                            iba.Properties.Resources.ReloadHdEventsCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2) == DialogResult.Yes)
                m_ctrlEvent.InitializeEventConfig(m_ctrlServer.Reader, new List<string>(), m_data.FullEventConfig.Values, m_ctrlEvent.ReadOnly);
            else
                m_ctrlEvent.StoreFilter = null;      // Set the storeFilter to null as all dataStores should be shown. This will also request edit locks on all hd event stores.

            m_tbPDO.Text = m_data.AnalysisFile;
            m_tbPwdDAT.Leave -= m_tbPwdDAT_TextChanged;
            m_tbPwdDAT.Text = "";

            if (Environment.MachineName != m_data.DatFileHost)
            {
                m_tbDAT.Text = "";
            }
            else
            {
                m_tbPwdDAT.Text = m_data.DatFilePassword;
                m_tbDAT.Text = m_data.DatFile;
            }

            m_tbPwdDAT.Leave += m_tbPwdDAT_TextChanged;
            
            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (decimal)Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes, 1));


            UpdateSources();
            LoadLocalData(m_data);
        }

        private void LoadLocalData(HDCreateEventTaskData data)
        {
            List<LocalEventData> treeData = new List<LocalEventData>();
            foreach (HDCreateEventTaskData.EventData signal in data.EventSettings)
            {
                LocalEventData localEvent = new LocalEventData(signal.ID, signal.StoreName);
                localEvent.Active = signal.Active;
                localEvent.NumericChannels = signal.NumericFields;
                localEvent.TextChannels = signal.TextFields;
                localEvent.Tag = signal;

                treeData.Add(localEvent);
            }
            m_ctrlEvent.InitializeLocalEvents(treeData);
        }

        public void LeaveCleanup()
        {
            m_ctrlEvent.ReleaseEditRightsServer();
            Task.Run(() => m_ctrlServer.Reader?.Disconnect()); // Could result into deadlocks when executing on the GUI thread when login form needs to be shown
            ResetChannelTrees();
            m_analyzerManager.OnLeave();
        }

        private void ResetChannelTrees()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(ResetChannelTrees));
                return;
            }
            if (bLoadingChannelTree)
            {
                return;
            }

            bResetChannelTree = true;
            m_pulseEditor.ResetChannelTree();
            m_timeEditor.ResetChannelTree();
            m_timeEditorOut.ResetChannelTree();
            m_channelEditor.ResetChannelTree();
            m_textEditor.ResetChannelTree();
            channelTree.Reset();
            numericTree.Reset();
            m_analyzerManager.Dispose();

            bResetChannelTree = false;
        }

		private bool SaveServerData(List<EventConfig> changedConfigs)
        {
            bool saveSuccessfull = true;
            if (!m_ctrlEvent.ReadOnly)
            {
                using (var validationForm = new HdFormValidation("save HD events"))
                {
                    validationForm.Text = "Save HD events";

                    try
                    {
                        if (m_data.FullEventConfig == null)
                            m_data.FullEventConfig = new Dictionary<string, string>();
                        m_data.FullEventConfig.Clear();
                        List<string> storeNames = new List<string>();
                        foreach (EventConfig config in changedConfigs)
                        {
                            m_data.FullEventConfig.Add(config.StoreName, m_ctrlEvent.SerialzeServerEvents(config.StoreName, m_ctrlServer.Server, m_ctrlServer.Port, m_data.Guid, m_data.Name, m_data.Username, m_data.HDPassword));
                            storeNames.Add(config.StoreName);
                        }

                        HDCreateEventTaskWorker worker = new HDCreateEventTaskWorker(m_data, null);
                        worker.WriteEvents(storeNames, null, validationForm.AddRange);

                    }
                    catch (HDCreateEventException)
                    {
                        saveSuccessfull = false;
                    }
                    catch (Exception ex)
                    {
                        validationForm.Add(new HdValidationMessage(HdValidationType.Error, ex.Message));
                        saveSuccessfull = false;
                    }

                    validationForm.Start((f) => { }); //Start with dummy to enable OK
                    validationForm.ShowDialog(this);
                }
            }
            return saveSuccessfull;
        }

        public void SaveData()
        {
            oldPdo = m_data.AnalysisFile = m_tbPDO.Text;
            oldDat = m_data.DatFileHost = Environment.MachineName;
            m_data.DatFile = m_tbDAT.Text;
            m_data.DatFilePassword = m_tbPwdDAT.Text;
            m_data.Server = m_ctrlServer.Server;
            m_data.ServerPort = m_ctrlServer.Port;
            m_data.Username = m_ctrlServer.Username;
            m_data.HDPassword = m_ctrlServer.Password;

            List<HDCreateEventTaskData.EventData> eventData = m_data.EventSettings;
            bool bSaveEventSettings = m_ctrlServer.Reader.IsConnected()
                                        || eventData.Count == 0
                                        || m_ctrlServer.Server != m_data.Server
                                        || m_ctrlServer.Port != m_data.ServerPort
                                        || m_ctrlServer.Username != m_data.Username
                                        || m_ctrlServer.Password != m_data.HDPassword;

            if (bSaveEventSettings && !m_ctrlEvent.ReadOnly)
            {
                //Fill list of events again from tree
                List<HDCreateEventTaskData.EventData> localEvents = new List<HDCreateEventTaskData.EventData>();
                Get(m_ctrlEvent.GetEventNodes(), new List<String>(), localEvents, "");
                if (localEvents.Count > 0)
                {
                    m_data.EventSettings.Clear();
                    foreach (HDCreateEventTaskData.EventData data in localEvents)
                        m_data.EventSettings.Add(data);
                }
                List<EventConfig > changedConfigs = m_ctrlEvent.ServerEventsChanged();
                if (changedConfigs!= null && changedConfigs.Count > 0)
                {
                    DialogResult res = MessageBox.Show(this, iba.Properties.Resources.HDEventsChanged,
                            iba.Properties.Resources.closing, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2);
                    switch (res)
                    {
                        case DialogResult.Yes:
                            if (SaveServerData(changedConfigs))
                            {
                                m_data.FullEventConfig.Clear();
                            }
                            else
                            {
                                var storeNames = m_ctrlEvent.GetStoreNames();
                                m_data.FullEventConfig.Clear();
                                foreach (string storeName in storeNames)
                                {
                                    m_data.FullEventConfig[storeName] = m_ctrlEvent.SerialzeServerEvents(storeName, m_data.Server, m_data.ServerPort, m_data.Guid, m_data.Name, m_data.Username, m_data.HDPassword);
                                }
                            }
                            break;
                        case DialogResult.No:
                            break;
                    }
                }
            }
            else
            {

                //Seems like the original HD server cannot be reached => don't store changes to prevent a configuration loss
            }

            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);

			if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
			{
				TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
				UploadPdoFile(false);
			}
			else if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
			{
				UploadPdoFile(false);
			}
        }

        List<string> GetPriorities()
        {
            HashSet<string> prios = new HashSet<string>();

            prios.Add("Low");
            prios.Add("Normal");
            prios.Add("High");

            List<string> priorities = new List<string>(prios);
            priorities.Sort();

            return priorities;
        }

        void CheckLocalEventData(LocalEventData localEventData, EventWriterSignal serverData)
        {
            // Check synchronisation between numeric/text fields between ibaHD data and local data.
            if (localEventData == null || serverData == null || serverData.FloatFields == null || serverData.TextFields == null)
                return;

            for (int i = localEventData.NumericChannels.Count - 1; i >= 0; i--)
            {
                Tuple<string, string> numericField = localEventData.NumericChannels[i];
                if (!serverData.FloatFields.Contains(numericField.Item1))
                    localEventData.NumericChannels.RemoveAt(i);
            }

            for (int i = localEventData.TextChannels.Count - 1; i >= 0; i--)
            {
                Tuple<string, string> textField = localEventData.TextChannels[i];
                if (!serverData.TextFields.Contains(textField.Item1))
                    localEventData.TextChannels.RemoveAt(i);
            }
        }

        void Get(NodeCollection nodes, List<string> path, List<HDCreateEventTaskData.EventData> localSignals, string store)
        {
            foreach (Node node in nodes)
            {
                ControlEventTreeData sigNode = node.Tag as ControlEventTreeData;
                if (sigNode == null)
                {
                    if (node.Tag is EventConfig)
                        Get(node.Nodes, path, localSignals, node.Text);
                    else
                    {
                        path.Add(node.Text);
                        // If a folder does not have any children, add an emptyFolder event
                        if (node.Nodes.Count == 0 && node.Tag == null)
                        {
                            node.Nodes.Add(m_ctrlEvent.CreateEmptyFolderEvent(node));
                        }
                        Get(node.Nodes, path, localSignals, store);
                        path.RemoveAt(path.Count - 1);
                    }
                }
                else
                {
                    // Only add the emptyFolderEvent in case it is the only event in the folder.
                    if (sigNode.EventWriterSignal?.Name == ClientConstants.EmptyFolderEventName && nodes.Count != 1)
                        continue;

                    LocalEventData localEventData = sigNode.LocalEventData;
                    EventWriterSignal eventWriterSignal = sigNode.EventWriterSignal;
                    HDCreateEventTaskData.EventData eventData = new HDCreateEventTaskData.EventData();
                    if (localEventData != null)
                    {
                        if (localEventData.Tag != null && localEventData.Tag is HDCreateEventTaskData.EventData)
                            eventData = (HDCreateEventTaskData.EventData)localEventData.Tag;

                        eventData.ID = sigNode.id;
                        eventData.Name = eventWriterSignal.Name;
                        eventData.StoreName = store;

                        CheckLocalEventData(localEventData, eventWriterSignal);

                        eventData.NumericFields = localEventData?.NumericChannels;

                        eventData.TextFields = localEventData?.TextChannels;

                        eventData.BlobFields = new List<string>(eventWriterSignal.BlobFields);
                        eventData.Active = localEventData.Active;

                        localSignals.Add(eventData);
                    }
                }
            }
        }
        #endregion

        void UpdateSources()
        {
            m_analyzerManager.UpdateSource(m_tbPDO.Text, m_tbDAT.Text, m_tbPwdDAT.Text, m_data.ParentConfigurationData);
            loadAnalyzerTreeDataTask();
        }

        private void m_tbPwdDAT_TextChanged(object sender, EventArgs e)
        {
            UpdateSources();
        }

        private void m_btnBrowseDAT_Click(object sender, EventArgs e)
        {
			string datFile = m_tbDAT.Text;
			if (Utility.DatCoordinatorHostImpl.Host.BrowseForDatFile(ref datFile, m_data.ParentConfigurationData))
			{
				m_tbDAT.Text = datFile;
                UpdateSources();
            }
        }

        private void m_btnOpenPDO_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.OpenPDO(m_tbPDO.Text, m_tbDAT.Text);
		}

        void UploadPdoFile(bool messageOnNoChanges)
        {
			Utility.DatCoordinatorHostImpl.Host.UploadPdoFile(messageOnNoChanges, this, m_tbPDO.Text, m_analyzerManager, m_data.ParentConfigurationData);
			UpdateSources();
		}

        string oldDat = "";
        private void DatTextEntered(object sender, EventArgs e)
        {
            oldDat = m_tbDAT.Text;
        }

        private void DatTextLeave(object sender, EventArgs e)
        {
            string newDat = m_tbDAT.Text;
            if (newDat != oldDat)
            {
                UpdateSources();
            }
        }

        string oldPdo = "";
        private void PDOTextEntered(object sender, EventArgs e)
        {
            oldPdo = m_tbPDO.Text;
        }

        private void PDOTextLeave(object sender, EventArgs e)
        {
            string newPdo = m_tbPDO.Text;
            if (newPdo != oldPdo)
            {
                UpdateSources();
            }
        }

        private void m_btTakeParentPass_Click(object sender, EventArgs e)
        {
            m_tbPwdDAT.Text = m_data.ParentConfigurationData.FileEncryptionPassword;
            DatTextLeave(null, null);
        }



        private void m_btnUploadPDO_Click(object sender, EventArgs e)
        {
            UploadPdoFile(true);
        }

        private void m_btnTest_Click(object sender, EventArgs e)
        {
            SaveData();
            try
            {
                Cursor = Cursors.WaitCursor;
                
                string datFile = m_tbDAT.Text;
                string error = "";
                bool ok = true;
                if (string.IsNullOrEmpty(datFile) || !DataPath.FileExists(datFile))
                {
                    error = Properties.Resources.logHDEventTaskDATError;
                    ok = false;
                }
                if (ok)
                {
                    if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.ServiceIsLocal)
                    {
                        TestTask(m_data);
                        MessageBox.Show(this, Properties.Resources.HDEventTask_TestSuccess, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
                    {
                        error = Properties.Resources.ClientNeedsConnectionForTestTask;
                        ok = false;
                    }
                    else //connected... because the worker uses ibaFiles and hdq ini parsers, not yet possible to test locally
                    {
                        Program.CommunicationObject.TestHDEventCreationTask(m_data);
                    }
                }
                
                if (!ok)
                {
                    MessageBox.Show(this, error, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(this, ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void TestTask(HDCreateEventTaskData dat)
        {
            HDCreateEventTaskWorker worker = new HDCreateEventTaskWorker(dat, null);
            Dictionary<string, EventWriterData> eventData = worker.GenerateEvents(null, m_tbDAT.Text);
        }

        private void m_btnBrowsePDO_Click(object sender, EventArgs e)
        {
			string localPath;
            string pdoFilePath = m_tbPDO.Text;
            if (Utility.DatCoordinatorHostImpl.Host.BrowseForPdoFile(ref pdoFilePath, out localPath))
			{
                m_tbPDO.Text = pdoFilePath;
                UpdateSources();
            }
        }

        void m_viewPulse_RowCellStyle(object sender, RowCellStyleEventArgs e)
        {
            GridView view = sender as GridView;
            if (view == null)
                return;

            if (e.Column.ReadOnly)
            {
                e.Appearance.BackColor = SystemColors.Control;
                e.Appearance.ForeColor = SystemColors.ControlText;
            }
        }
    }
}
