using System.Collections.Generic;
using System.Windows.Forms;
using iba.HD.Common;
using iba.Data;
using iba.HD.Client;
using System;

namespace iba.Controls
{
    public partial class HDEventCreationTaskControl : UserControl, IPropertyPane
    {
        #region Members
        HDCreateEventTaskData m_data;
        IPropertyPaneManager m_manager;
        #endregion

        public HDEventCreationTaskControl()
        {
            InitializeComponent();

            m_ctrlServer.SetServerFeatures(new List<ReaderFeature>(1) { ReaderFeature.ComputedValue }, new List<WriterFeature>(1) { WriterFeature.ComputedValue });
            m_ctrlServer.StoreFilter = new List<HdStoreType> { HdStoreType.Event };
            m_ctrlEvent.Init(m_ctrlServer.Reader);
            m_ctrlServer.StoreSelectionChanged += (s, e) => { m_ctrlEvent.StoreFilter = string.IsNullOrEmpty(m_ctrlServer.StoreName) ? new List<string>() : new List<string>(1){ m_ctrlServer.StoreName }; };
        }

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

            m_tbPDO.Text = m_data.AnalysisFile;

            m_tbPulseSignal.Text = m_data.PulseSignal;
            if (m_data.TriggerMode == HDCreateEventTaskData.HDEventTriggerEnum.PerFile)
                m_rbTriggerPerFile.Checked = true; //TODO disable pulse signal
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
        { }

        public void SaveData()
        {
            m_data.AnalysisFile = m_tbPDO.Text;

            m_data.TriggerMode = m_rbTriggerBySignal.Checked ? HDCreateEventTaskData.HDEventTriggerEnum.PerSignalPulse : HDCreateEventTaskData.HDEventTriggerEnum.PerFile;
            m_data.PulseSignal = m_tbPulseSignal.Text;

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
    }
}
