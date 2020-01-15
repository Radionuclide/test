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

namespace iba.Controls
{
    public partial class HDEventCreationTaskControl : UserControl, IPropertyPane
    {
        

        #region Members
        HDCreateEventTaskData m_data;
        IPropertyPaneManager m_manager;

        AnalyzerManager m_analyzerManager;
        RepositoryItemChannelTreeEdit m_pulseEditor, m_channelEditor, m_textEditor;
        TriggerControl triggerControl;

        string m_pdoFilePath, m_datFilePath;
        AnalyzerTreeControl channelTree;
        AnalyzerTreeControl numericTree;
        #endregion

        public HDEventCreationTaskControl()
        {
            InitializeComponent();

            m_pdoFilePath = m_datFilePath = "";

            m_ctrlServer.SetServerFeatures(new List<ReaderFeature>(1) { ReaderFeature.ComputedValue }, new List<WriterFeature>(1) { WriterFeature.ComputedValue });
            m_ctrlServer.StoreFilter = new List<HdStoreType> { HdStoreType.Event };
            m_ctrlServer.HideStoreSelection();
            m_analyzerManager = new AnalyzerManager();
            m_pulseEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital);
            m_pulseEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);

            m_channelEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog);
            numericTree = m_channelEditor.ChannelTree;
            m_channelEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);

            m_textEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Text);
            m_textEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);
            m_textEditor.AddSpecialNode(HDCreateEventTaskData.CurrentFileExpression, Properties.Resources.HDEventTask_ChannelProcessedFile, Properties.Resources.img_file);

            m_pulseEditor.ChannelTree.Invalidated += ChannelEditorModified;
            numericTree.Invalidated += ChannelEditorModified;
            m_textEditor.ChannelTree.Invalidated += ChannelEditorModified;


            triggerControl = new TriggerControl();
            triggerControl.SetChannelEditor(m_pulseEditor, m_pulseEditor.ChannelTree);

            m_ctrlEvent.ApplicationName = "ibaDatCoordinator";

            m_ctrlEvent.DefaultNumericChannelValue = HDCreateEventTaskData.UnassignedExpression;
            m_ctrlEvent.DefaultTextChannelValue = HDCreateEventTaskData.UnassignedExpression;

            m_ctrlEvent.SetNumericChannelEditor(m_channelEditor, m_channelEditor.ChannelTree);
            m_ctrlEvent.SetTextChannelEditor(m_textEditor, m_textEditor.ChannelTree);
            channelTree = new AnalyzerTreeControl(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Text);
            m_ctrlEvent.ChannelTree = channelTree;

            m_toolTip.SetToolTip(m_btnOpenPDO, Properties.Resources.HDEventTask_ToolTip_OpenPDO);
            m_toolTip.SetToolTip(m_btnUploadPDO, Properties.Resources.HDEventTask_ToolTip_UploadPDO);
            m_toolTip.SetToolTip(m_btnTest, Properties.Resources.HDEventTask_ToolTip_Test);

            m_ctrlEvent.EventTrigger = triggerControl;
            m_ctrlServer.ServerSelectionChanged += (s, e) => 
            {
                if (m_ctrlServer.Reader.UserManager.GetCurrentUser() is PdaClientUser user)
                {
                    if (user.StoreRights[1].StoreRange == PdaClientUser.HdStoreRight.StoreRightRange.List)
                        m_ctrlEvent.StoreFilter = user.StoreRights[1].AllowedStores;
                    else if (user.StoreRights[1].StoreRange == PdaClientUser.HdStoreRight.StoreRightRange.All)
                        m_ctrlEvent.StoreFilter = null;
                }
            };
            m_ctrlEvent.InitializeEventConfig(m_ctrlServer.Reader, new List<string>(), false);
            m_ctrlEvent.EventWizard = new HDEventWizard(m_analyzerManager, GetPriorities());
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
            if (disposing)
            {
                if (m_pulseEditor != null)
                {
                    triggerControl.GrPulse.RepositoryItems.Remove(m_pulseEditor);
                    triggerControl.ColPulse.ColumnEdit = null;

                    m_pulseEditor.Dispose();
                    m_pulseEditor = null;
                }

                if (m_channelEditor != null)
                {
                    if (m_ctrlEvent != null)
                        m_ctrlEvent.SetNumericChannelEditor(null, null);

                    m_channelEditor.Dispose();
                    m_channelEditor = null;
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

                components?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region IPropertyPane
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_btnUploadPDO.Enabled = Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal;

            m_manager = manager;
            m_data = datasource as HDCreateEventTaskData;
            if (m_data.EventSettings.Count > 0 && m_ctrlServer.Server != m_data.Server && m_ctrlServer.Port != m_data.ServerPort)
                m_ctrlServer.LoadData(m_data.Server, m_data.ServerPort,
                    m_data.Username, m_data.Password, "");
            else if (m_data.EventSettings.Count == 0)
                m_ctrlServer.LoadData("localhost", 9180, "", "", "");
            m_pdoFilePath = m_data.AnalysisFile;
            m_tbPDO.Text = Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.ServiceIsLocal ? m_data.AnalysisFile : Path.GetFileName(m_data.AnalysisFile);

            m_tbPwdDAT.TextChanged -= m_tbPwdDAT_TextChanged;

            if (Environment.MachineName != m_data.DatFileHost)
            {
                m_datFilePath = "";
                m_tbDAT.Text = "";
                m_tbPwdDAT.Text = "";
            }
            else
            {
                m_datFilePath = m_data.DatFile;
                m_tbDAT.Text = m_datFilePath;
                m_tbPwdDAT.Text = "";
            }

            m_tbPwdDAT.TextChanged += m_tbPwdDAT_TextChanged;

            UpdateSources();

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (decimal)Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes, 1));


            LoadLocalData(m_data);
            Task.Factory.StartNew(() =>
            {
                while (!channelTree.Load()) ;
                while (!m_analyzerManager.IsOpened)
                {
                    Thread.Sleep(500);
                }
                while (!numericTree.Load()) ;
                while (!m_pulseEditor.ChannelTree.Load()) ;
                while (!m_textEditor.ChannelTree.Load()) ;
            });
        }

        private void LoadLocalData(HDCreateEventTaskData data)
        {
            List<LocalEventData> treeData = new List<LocalEventData>();
            foreach (HDCreateEventTaskData.EventData signal in data.EventSettings)
            {
                LocalEventData localEvent = new LocalEventData(signal.ID, signal.StoreName);
                localEvent.NumericChannels = signal.NumericFields;
                localEvent.TextChannels = signal.TextFields;
                localEvent.Tag = signal;

                treeData.Add(localEvent);
            }
            m_ctrlEvent.InitializeLocalEvents(treeData);
        }

        public void LeaveCleanup()
        {
            m_ctrlServer.Reader.Disconnect();
            m_pulseEditor.ResetChannelTree();
            m_channelEditor.ResetChannelTree();
            m_textEditor.ResetChannelTree();
            channelTree.Reset();
            m_analyzerManager.Dispose();
        }

        private void SaveServerData()
        {
            if (!m_ctrlEvent.IsReadOnly())
            {
                using (var validationForm = new HdFormValidation("save HD events"))
                {
                    validationForm.Text = "Save HD events";

                    try
                    {
                        if (m_data.FullEventConfig == null)
                            m_data.FullEventConfig = new Dictionary<string, string>();
                        m_data.FullEventConfig.Clear();
                        List<string> storeNames = m_ctrlEvent.GetStoreNames();
                        foreach (string storeName in storeNames)
                            m_data.FullEventConfig.Add(storeName, m_ctrlEvent.SerialzeServerEvents(storeName, m_ctrlServer.Server, m_ctrlServer.Port, m_data.Guid, m_data.Name, m_data.Username, m_data.Password));


                        HDCreateEventTaskWorker worker = new HDCreateEventTaskWorker(m_data);
                        validationForm.AddRange(worker.WriteEvents(storeNames, null));
                    }
                    catch (Exception ex)
                    {
                        validationForm.Add(new HdValidationMessage(HdValidationType.Error, ex.Message));
                    }

                    validationForm.Start((f) => { }); //Start with dummy to enable OK
                    validationForm.ShowDialog(this);
                }
            }
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFilePath;
            m_data.DatFileHost = Environment.MachineName;
            m_data.DatFile = m_datFilePath;
            m_data.DatFilePassword = m_tbPwdDAT.Text;
            m_data.Server = m_ctrlServer.Server;
            m_data.ServerPort = m_ctrlServer.Port;
            m_data.Username = m_ctrlServer.Username;
            m_data.Password = m_ctrlServer.Password;

            List<HDCreateEventTaskData.EventData> eventData = m_data.EventSettings;
            bool bSaveEventSettings = m_ctrlServer.Reader.IsConnected()
                                        || eventData.Count == 0
                                        || m_ctrlServer.Server != m_data.Server
                                        || m_ctrlServer.Port != m_data.ServerPort
                                        || m_ctrlServer.Username != m_data.Username
                                        || m_ctrlServer.Password != m_data.Password;

            if (bSaveEventSettings && !m_ctrlEvent.IsReadOnly())
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

                if (m_ctrlEvent.ServerEventsChanged())
                {
                    DialogResult res = MessageBox.Show(this, iba.Properties.Resources.HDEventsChanged,
                            iba.Properties.Resources.closing, MessageBoxButtons.YesNo, MessageBoxIcon.Question,
                            MessageBoxDefaultButton.Button2);
                    switch (res)
                    {
                        case DialogResult.Yes:
                            SaveServerData();
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
                        Get(node.Nodes, path, localSignals, store);
                        path.RemoveAt(path.Count - 1);
                    }
                }
                else
                {
                    LocalEventData localEventData = sigNode.localEventData;
                    EventWriterSignal eventWriterSignal = sigNode.EventWriterSignal;
                    HDCreateEventTaskData.EventData eventData = new HDCreateEventTaskData.EventData();
                    if (localEventData != null)
                    {
                        if (localEventData.Tag != null && localEventData.Tag is HDCreateEventTaskData.EventData)
                            eventData = (HDCreateEventTaskData.EventData)localEventData.Tag;
                        
                        eventData.ID = sigNode.id;
                        eventData.Name = eventWriterSignal.Name;
                        eventData.StoreName = store;

                        eventData.NumericFields = localEventData?.NumericChannels;

                        eventData.TextFields = localEventData?.TextChannels;

                        eventData.BlobFields = new List<string>(eventWriterSignal.BlobFields);

                        localSignals.Add(eventData);
                    }
                }
            }
        }
        #endregion

        void UpdateSources()
        {
            m_analyzerManager.UpdateSource(m_pdoFilePath, m_datFilePath, m_tbPwdDAT.Text);
        }

        private void m_tbPwdDAT_TextChanged(object sender, EventArgs e)
        {
            UpdateSources();
        }

        private void m_btnBrowseDAT_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlg = new OpenFileDialog())
            {
                string dir = string.IsNullOrEmpty(m_datFilePath) ? m_datFilePath : Path.GetDirectoryName(m_datFilePath);
                if (!string.IsNullOrEmpty(dir))
                    dlg.InitialDirectory = dir;

                if (File.Exists(m_datFilePath))
                    dlg.FileName = Path.GetFileName(m_datFilePath);

                dlg.Filter = Properties.Resources.DatFileFilter;

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                m_datFilePath = dlg.FileName;
            }

            m_tbDAT.Text = m_datFilePath;
            UpdateSources();
            while (!channelTree.Load()) ;
            while (!m_textEditor.ChannelTree.Load()) ;
            while (!m_channelEditor.ChannelTree.Load()) ;
            while (!m_pulseEditor.ChannelTree.Load()) ;

        }

        private void m_btnOpenPDO_Click(object sender, EventArgs e)
        {
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                object o = key.GetValue("");
                string ibaAnalyzerExe = Path.GetFullPath(o.ToString());

                if (!Utility.VersionCheck.CheckVersion(ibaAnalyzerExe, "7.1.0"))
                {
                    MessageBox.Show(this, string.Format(Properties.Resources.logAnalyzerVersionError, "7.1.0"), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!Program.RemoteFileLoader.DownloadFile(m_pdoFilePath, out string localFile, out string error))
                {
                    MessageBox.Show(this, error, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = ibaAnalyzerExe;
                    ibaProc.StartInfo.Arguments = "\"" + m_pdoFilePath + "\"";
                    ibaProc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void UploadPdoFile(bool messageOnNoChanges)
        {
            if (Disposing || IsDisposed)
                return;

            if (InvokeRequired)
            {
                Invoke(new Action<bool>(UploadPdoFile), messageOnNoChanges);
                return;
            }

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
            {
                string localFile = Program.RemoteFileLoader.GetLocalPath(m_pdoFilePath);
                if (Program.RemoteFileLoader.IsFileChangedLocally(localFile, m_pdoFilePath))
                {
                    if (MessageBox.Show(this, Properties.Resources.FileChanged_Upload, "ibaDatCoordinator", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        return;

                    Cursor = Cursors.WaitCursor;

                    try
                    {
                        bool bStarted = TaskManager.Manager.IsJobStarted(m_data.ParentConfigurationData.Guid);
                        if (bStarted)
                            TaskManager.Manager.StopAndWaitForConfiguration(m_data.ParentConfigurationData);

                        if (!Program.RemoteFileLoader.UploadFile(localFile, m_pdoFilePath, true, out string error))
                            throw new Exception(error);

                        if (bStarted)
                            TaskManager.Manager.StartConfiguration(m_data.ParentConfigurationData);

                        Cursor = Cursors.Default;
                    }
                    catch (Exception ex)
                    {
                        Cursor = Cursors.Default;
                        MessageBox.Show(this, ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (messageOnNoChanges)
                    MessageBox.Show(this, Properties.Resources.FileChanged_UploadNoChanges, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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

                HDCreateEventTaskWorker worker = new HDCreateEventTaskWorker(m_data);
                Dictionary<string, EventWriterData> eventData = worker.GenerateEvents(null, null);

                worker.WriteEvents(m_ctrlEvent.GetStoreNames(), eventData);

                Cursor = Cursors.Default;
                MessageBox.Show(this, Properties.Resources.HDEventTask_TestSuccess, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                Cursor = Cursors.Default;
                MessageBox.Show(this, ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void m_btnBrowsePDO_Click(object sender, EventArgs e)
        {
            bool bLocal = false;
            DialogResult result = DialogResult.Abort;
            string path = m_pdoFilePath;
            if (Program.RunsWithService != Program.ServiceEnum.NOSERVICE && !Program.ServiceIsLocal)
            {
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                {
                    using (iba.Controls.ServerFolderBrowser fd = new iba.Controls.ServerFolderBrowser(true))
                    {
                        fd.FixedDrivesOnly = false;
                        fd.ShowFiles = true;
                        fd.SelectedPath = path;
                        fd.Filter = Properties.Resources.PdoFileFilter;
                        result = fd.ShowDialog(this);
                        path = fd.SelectedPath;
                    }
                }
                else
                    MessageBox.Show(this, Properties.Resources.HDEventTask_PDOConnectionRequired, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                bLocal = true;
                using (var dlg = new OpenFileDialog())
                {
                    dlg.CheckFileExists = true;
                    dlg.FileName = "";
                    dlg.Filter = Properties.Resources.PdoFileFilter;
                    if (File.Exists(path))
                        dlg.FileName = Path.GetFileName(path);
                    if (!string.IsNullOrEmpty(path))
                    {
                        string dir = Path.GetDirectoryName(path);
                        if (Directory.Exists(dir))
                            dlg.InitialDirectory = dir;
                    }
                    result = dlg.ShowDialog(this);
                    path = dlg.FileName;
                }
            }
            if (result == DialogResult.OK)
            {
                m_pdoFilePath = path;
                m_tbPDO.Text = bLocal ? path : Path.GetFileName(path);
                UpdateSources();
                while (!channelTree.Load());
                while (!m_textEditor.ChannelTree.Load());
                while (!m_channelEditor.ChannelTree.Load());
                while (!m_pulseEditor.ChannelTree.Load()) ;
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
