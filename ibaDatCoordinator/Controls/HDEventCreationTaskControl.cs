using System.Collections.Generic;
using System.Windows.Forms;
using iba.HD.Common;
using iba.Data;
using iba.HD.Client;
using System;
using DevExpress.XtraGrid.Views.Grid;
using System.Drawing;
using System.IO;

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

        string m_datFilePath;
        #endregion

        public HDEventCreationTaskControl()
        {
            InitializeComponent();

            m_datFilePath = "";

            m_ctrlServer.SetServerFeatures(new List<ReaderFeature>(1) { ReaderFeature.ComputedValue }, new List<WriterFeature>(1) { WriterFeature.ComputedValue });
            m_ctrlServer.StoreFilter = new List<HdStoreType> { HdStoreType.Event };
            m_ctrlEvent.Init(m_ctrlServer.Reader);
            m_ctrlServer.StoreSelectionChanged += (s, e) => { m_ctrlEvent.StoreFilter = string.IsNullOrEmpty(m_ctrlServer.StoreName) ? new List<string>() : new List<string>(1){ m_ctrlServer.StoreName }; };

            m_analyzerManager = new AnalyzerManager();

            m_pulseEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog); //TODO remove analog when expression types are available
            m_pulseEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);

            m_channelEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog);
            m_channelEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);

            m_textEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Text | ChannelTreeFilter.Analog); //TODO remove analog when expression types are available
            m_textEditor.AddSpecialNode(HDCreateEventTaskData.UnassignedExpression, Properties.Resources.HDEventTask_ChannelUnassigned, Properties.Resources.img_warning);
            m_textEditor.AddSpecialNode(HDCreateEventTaskData.CurrentFileExpression, Properties.Resources.HDEventTask_ChannelProcessedFile, Properties.Resources.img_file);

            m_grPulse.RepositoryItems.Add(m_pulseEditor);
            m_colPulse.ColumnEdit = m_pulseEditor;

            m_ctrlEvent.DefaultChannelValue = HDCreateEventTaskData.UnassignedExpression;
            m_ctrlEvent.DefaultTextChannelValue = HDCreateEventTaskData.UnassignedExpression;

            m_ctrlEvent.ChannelEditor = m_channelEditor;
            m_ctrlEvent.TextChannelEditor = m_textEditor;
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
            //TODO disable controls, etc.
            //TODO remote files??
            //TODO editors
            //TODO test button

            m_manager = manager;
            m_data = datasource as HDCreateEventTaskData;

            m_ctrlServer.LoadData(m_data.EventSettings.Server, m_data.EventSettings.ServerPort,
                m_data.EventSettings.Username, m_data.EventSettings.Password, m_data.EventSettings.StoreName);

            m_tbPDO.Text = m_data.AnalysisFile; //TODO

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
                m_tbDAT.Text = Path.GetFileName(m_datFilePath);
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
            m_pulseEditor.ResetChannelTree();
            m_channelEditor.ResetChannelTree();
            m_textEditor.ResetChannelTree();
            m_analyzerManager.Dispose();
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_tbPDO.Text;
            m_data.DatFileHost = Environment.MachineName;
            m_data.DatFile = m_datFilePath;
            m_data.DatFilePassword = m_tbPwdDAT.Text;

            m_data.TriggerMode = m_rbTriggerBySignal.Checked ? HDCreateEventTaskData.HDEventTriggerEnum.PerSignalPulse : HDCreateEventTaskData.HDEventTriggerEnum.PerFile;
            m_data.PulseSignal = (m_grPulse.DataSource as List<PulseSignal>)[0].PulseID;

            HDCreateEventTaskData.EventData eventData = m_data.EventSettings;
            eventData.Server = m_ctrlServer.Server;
            eventData.ServerPort = m_ctrlServer.Port;
            eventData.StoreName = m_ctrlServer.StoreName;
            eventData.Username = m_ctrlServer.Username;
            eventData.Password = m_ctrlServer.Password;

            ControlEvent.EventSettings eventSettings = m_ctrlEvent.GetSettings();
            eventData.ID = eventSettings.ID;
            eventData.Name = eventSettings.Name;
            eventData.NumericFields = new List<Tuple<string, string>>(eventSettings.FloatFields);
            eventData.TextFields = new List<Tuple<string, string>>(eventSettings.TextFields);
            eventData.BlobFields = new List<string>(eventSettings.BlobFields);

            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);

            //TODO other settings
            //TODO remote stuff
        }
        #endregion

        void UpdateSources()
        {
            //TODO pdo stuff
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
                string dir = string.IsNullOrEmpty(m_datFilePath) ? m_datFilePath : Path.GetFullPath(m_datFilePath);
                if (!string.IsNullOrEmpty(dir))
                    dlg.InitialDirectory = dir;

                dlg.Filter = Properties.Resources.DatFileFilter;

                if (dlg.ShowDialog() != DialogResult.OK)
                    return;

                m_datFilePath = dlg.FileName;
            }

            m_tbDAT.Text = Path.GetFileName(m_datFilePath);
            UpdateSources();
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
