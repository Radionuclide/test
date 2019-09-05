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

namespace iba.Controls
{
    public partial class HDEventCreationTaskControl : UserControl, IPropertyPane
    {
        private class PulseSignal
        {
            public string PulseID { get; set; }

            public PulseSignal()
                :this("")
            { }

            public PulseSignal(string id)
            {
                PulseID = id ?? "";
            }
        }

        #region Members
        HDCreateEventTaskData m_data;
        IPropertyPaneManager m_manager;

        AnalyzerManager m_analyzerManager;
        RepositoryItemChannelTreeEdit m_pulseEditor, m_channelEditor, m_textEditor;

        string m_pdoFilePath, m_datFilePath;
        #endregion

        public HDEventCreationTaskControl()
        {
            InitializeComponent();

            m_pdoFilePath = m_datFilePath = "";

            m_ctrlServer.SetServerFeatures(new List<ReaderFeature>(1) { ReaderFeature.ComputedValue }, new List<WriterFeature>(1) { WriterFeature.ComputedValue });
            m_ctrlServer.StoreFilter = new List<HdStoreType> { HdStoreType.Event };
            m_ctrlEvent.Init(m_ctrlServer.Reader);
            m_ctrlServer.StoreSelectionChanged += (s, e) => { m_ctrlEvent.StoreFilter = string.IsNullOrEmpty(m_ctrlServer.StoreName) ? new List<string>() : new List<string>(1){ m_ctrlServer.StoreName }; };

            m_analyzerManager = new AnalyzerManager();

            m_pulseEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital);
            m_pulseEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);

            m_channelEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog);
            m_channelEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);

            m_textEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Text);
            m_textEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);
            m_textEditor.AddSpecialNode(HDCreateEventTaskData.CurrentFileExpression, Properties.Resources.HDEventTask_ChannelProcessedFile, Properties.Resources.img_file);

            m_grPulse.RepositoryItems.Add(m_pulseEditor);
            m_colPulse.ColumnEdit = m_pulseEditor;

            m_ctrlEvent.AllowNoStoreFilter = false;

            m_ctrlEvent.DefaultChannelValue = HDCreateEventTaskData.UnassignedExpression;
            m_ctrlEvent.DefaultTextChannelValue = HDCreateEventTaskData.UnassignedExpression;

            m_ctrlEvent.ChannelEditor = m_channelEditor;
            m_ctrlEvent.TextChannelEditor = m_textEditor;

            m_toolTip.SetToolTip(m_btnOpenPDO, Properties.Resources.HDEventTask_ToolTip_OpenPDO);
            m_toolTip.SetToolTip(m_btnUploadPDO, Properties.Resources.HDEventTask_ToolTip_UploadPDO);
            m_toolTip.SetToolTip(m_btnTest, Properties.Resources.HDEventTask_ToolTip_Test);
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
                    m_grPulse.RepositoryItems.Remove(m_pulseEditor);
                    m_colPulse.ColumnEdit = null;

                    m_pulseEditor.Dispose();
                    m_pulseEditor = null;
                }

                if (m_channelEditor != null)
                {
                    if (m_ctrlEvent != null)
                        m_ctrlEvent.ChannelEditor = null;

                    m_channelEditor.Dispose();
                    m_channelEditor = null;
                }

                if (m_textEditor != null)
                {
                    if (m_ctrlEvent != null)
                        m_ctrlEvent.TextChannelEditor = null;

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

            m_ctrlServer.LoadData(m_data.EventSettings.Server, m_data.EventSettings.ServerPort,
                m_data.EventSettings.Username, m_data.EventSettings.Password, m_data.EventSettings.StoreName);

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

            m_grPulse.DataSource = new List<PulseSignal>(1) { new PulseSignal(m_data.PulseSignal) };
            if (m_data.TriggerMode == HDCreateEventTaskData.HDEventTriggerEnum.PerFile)
                m_rbTriggerPerFile.Checked = true;
            else
                m_rbTriggerBySignal.Checked = true;

            ControlEvent.EventSettings eventSettings = new ControlEvent.EventSettings(m_data.EventSettings.ID, m_data.EventSettings.Name,
                m_data.EventSettings.NumericFields, m_data.EventSettings.TextFields, m_data.EventSettings.BlobFields);

            m_ctrlEvent.LoadSettings(eventSettings);

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (decimal)Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes, 1));
        }

        public void LeaveCleanup()
        {
            m_ctrlServer.Reader.Disconnect();
            m_pulseEditor.ResetChannelTree();
            m_channelEditor.ResetChannelTree();
            m_textEditor.ResetChannelTree();
            m_analyzerManager.Dispose();
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFilePath;
            m_data.DatFileHost = Environment.MachineName;
            m_data.DatFile = m_datFilePath;
            m_data.DatFilePassword = m_tbPwdDAT.Text;

            m_data.TriggerMode = m_rbTriggerBySignal.Checked ? HDCreateEventTaskData.HDEventTriggerEnum.PerSignalPulse : HDCreateEventTaskData.HDEventTriggerEnum.PerFile;
            m_data.PulseSignal = (m_grPulse.DataSource as List<PulseSignal>)[0].PulseID;

            HDCreateEventTaskData.EventData eventData = m_data.EventSettings;
            bool bSaveEventSettings = m_ctrlServer.Reader.IsConnected()
                                        || m_ctrlServer.Server != eventData.Server
                                        || m_ctrlServer.Port != eventData.ServerPort
                                        || m_ctrlServer.StoreName != eventData.StoreName
                                        || m_ctrlServer.Username != eventData.Username
                                        || m_ctrlServer.Password != eventData.Password;

            if (bSaveEventSettings)
            {
                eventData.Server = m_ctrlServer.Server;
                eventData.ServerPort = m_ctrlServer.Port;
                eventData.StoreName = m_ctrlServer.StoreName;
                eventData.Username = m_ctrlServer.Username;
                eventData.Password = m_ctrlServer.Password;

                ControlEvent.EventSettings eventSettings = m_ctrlEvent.GetSettings();
                eventData.ID = eventSettings.ID;
                eventData.Name = eventSettings.Name;
                List<Tuple<string, string>> numFields = new List<Tuple<string, string>>();
                foreach (var field in eventSettings.FloatFields)
                    numFields.Add(Tuple.Create(field.Item1, field.Item2));
                eventData.NumericFields = numFields;
                eventData.TextFields = new List<Tuple<string, string>>(eventSettings.TextFields);
                eventData.BlobFields = new List<string>(eventSettings.BlobFields);
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
        #endregion

        void UpdateSources()
        {
            m_analyzerManager.UpdateSource(m_data.AnalysisFile, m_datFilePath, m_tbPwdDAT.Text);
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
                EventWriterData eventData = worker.GenerateEvents(null, null);
                worker.WriteEvents(eventData);

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
            }
        }

        private void m_rbTriggerBySignal_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbTriggerBySignal.Checked)
            {
                m_colPulse.OptionsColumn.AllowEdit = true;
                m_colPulse.OptionsColumn.ReadOnly = false;
            }
            else
            {
                m_colPulse.OptionsColumn.AllowEdit = false;
                m_colPulse.OptionsColumn.ReadOnly = true;
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
