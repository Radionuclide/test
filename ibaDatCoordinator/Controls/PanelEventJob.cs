using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using iba.Data;
using iba.Utility;
using iba.Processing;
using iba.HD.Common;
using iba.HD.Client.Interfaces;
using iba.HD.Client;
using System.Threading.Tasks;

namespace iba.Controls
{
    public partial class PanelEventJob : UserControl, IPropertyPane
    {
        #region Members
        private long[] m_timeBases;

        private HdControlStorePicker m_hdStorePicker;
        IPropertyPaneManager m_manager;
        ConfigurationData m_confData;
        EventJobData m_eventData;

        IHdSignalTree m_treeEvents;
        CheckBox m_cbAutoSelectAll;
        Button m_lbErrorEventServer;

        IHdReader m_hdReader;

        bool DisableStartChangedEvent, DisableStopChangedEvent;

        private TimeSpan m_tsStart, m_tsStop;

        bool m_bEventServerChanged;
        #endregion

        #region Properties
        public TimeSpan Start
        {
            get { return m_tsStart; }
            set
            {
                m_tsStart = value;
                UpdateTimeControls(true);
            }
        }

        public TimeSpan Stop
        {
            get { return m_tsStop; }
            set
            {
                m_tsStop = value;
                UpdateTimeControls(false);
            }
        }
        #endregion

        #region Initialize
        public PanelEventJob()
        {
            InitializeComponent();
            //((Bitmap)m_refreshDats.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_applyToRunningBtn.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_undoChangesBtn.Image).MakeTransparent(Color.Magenta);

            m_hdReader = HdClient.CreateReader(HdUserType.Analyzer);
            m_hdReader.ShowConnectionError = false;
            m_hdReader.ConnectionChanged += OnHdConnectionChanged;

            m_hdStorePicker = new HdControlStorePicker();
            m_hdStorePicker.Dock = DockStyle.Fill;
            this.gbHD.Controls.Add(this.m_hdStorePicker);
            m_hdStorePicker.StoreMultiSelect = true;
            m_hdStorePicker.SelectedPort = 9180;
            m_hdStorePicker.SelectedServer = "localhost";
            m_hdStorePicker.SelectedStoreName = "";
            m_hdStorePicker.SelectedStoreNames = new string[0];
            m_hdStorePicker.StoreTypeFilter = HdStoreType.Time /*| HdStoreType.Length*/;
            m_hdStorePicker.HideConfigureButton();
            m_hdStorePicker.SetCheckedFeatures(ReaderFeature.Analyzer, new List<WriterFeature>());

            string[] itemStrs = iba.Properties.Resources.TimeSelectionChoices.Split(';');
            long ms = 10000; //10000 * 100 nanosec = 1 ms
            long s = 1000 * ms;
            m_timeBases = new long[] { 0, 1 * ms, 10 * ms, 100 * ms, s, 60 * s, 3600 * s, 24 * 3600 * s };
            int n = itemStrs.Count();
            //Debug.Assert(n == tmpStamps.Count());
            for (int i = 0; i < n; i++)
                m_cbTimeBase.Items.Add(new TimeCbItem(itemStrs[i], m_timeBases[i]));

            JobTriggerCbItem[] jobTriggerItems = new JobTriggerCbItem[3]
            {
                new JobTriggerCbItem(JobTrigger.Incoming),
                new JobTriggerCbItem(JobTrigger.Outgoing),
                new JobTriggerCbItem(JobTrigger.Both)
            };
            m_cbJobTrigger.Items.AddRange(jobTriggerItems);
        }
        #endregion

        #region Dispose
        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (m_hdReader != null)
                {
                    m_hdReader.ConnectionChanged -= OnHdConnectionChanged;
                    m_hdReader.Dispose();
                    m_hdReader = null;
                }

                components?.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion

        #region Load/Save
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_confData = datasource as ConfigurationData;
            m_eventData = m_confData.EventData; //should not be null
            
            //options of ConfData
            if(m_failTimeUpDown.Minimum > (decimal)m_confData.ReprocessErrorsTimeInterval.TotalMinutes)
                m_confData.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double)m_failTimeUpDown.Minimum);
            else if(m_failTimeUpDown.Maximum < (decimal)m_confData.ReprocessErrorsTimeInterval.TotalMinutes)
                m_confData.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double)m_failTimeUpDown.Maximum);

            m_cbInitialScanEnabled.Checked = m_confData.InitialScanEnabled;
            m_cbRepErr.Checked = m_confData.ReprocessErrors;
            m_failTimeUpDown.Value = (decimal)m_confData.ReprocessErrorsTimeInterval.TotalMinutes;
            m_retryUpDown.Value = (decimal)m_confData.NrTryTimes;
            m_retryUpDown.Enabled = m_cbRetry.Checked = m_confData.LimitTimesTried;

            //hdStore
            m_hdStorePicker.SelectedServer = m_eventData.HDServer;
            m_hdStorePicker.SelectedPort = m_eventData.HDPort;
            m_hdStorePicker.SelectedStoreNames = m_eventData.HDStores;
            //timeSelection
            Start = m_eventData.StartRangeFromTrigger;
            Stop = m_eventData.StopRangeFromTrigger;

            if (m_eventData.PreferredTimeBaseIsAuto)
                m_cbTimeBase.SelectedIndex = 0;
            else
                m_cbTimeBase.SelectedIndex = Array.FindIndex(m_timeBases, ticks => ticks == m_eventData.PreferredTimeBaseTicks);

            //event selection
            m_cbJobTrigger.SelectedItem = new JobTriggerCbItem(m_eventData.JobTriggerEvent);
            ChangeEventServer(m_eventData.EventHDServer, m_eventData.EventHDPort);
        }

        public void SaveData()
        {
            //options of ConfData
            m_confData.InitialScanEnabled = m_cbInitialScanEnabled.Checked;
            m_confData.ReprocessErrors = m_cbRepErr.Checked;
            m_confData.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double)m_failTimeUpDown.Value);
            m_confData.NrTryTimes = (int)m_retryUpDown.Value;
            m_confData.LimitTimesTried = m_cbRetry.Checked;
            //hdStore
            m_eventData.HDServer = m_hdStorePicker.SelectedServer;
            m_eventData.HDPort = m_hdStorePicker.SelectedPort;
            m_eventData.HDStores = m_hdStorePicker.SelectedStoreNames;// new string[]{m_hdStorePicker.SelectedStoreName};
            //time selection
            m_eventData.StartRangeFromTrigger = Start;
            m_eventData.StopRangeFromTrigger = Stop;
            if(m_cbTimeBase.SelectedIndex == 0) //auto
            {
                m_eventData.PreferredTimeBaseTicks = GetAutoItem().m_timebaseLength;
                m_eventData.PreferredTimeBaseIsAuto = true;
            }
            else if(m_cbTimeBase.SelectedIndex > 0)
            {
                m_eventData.PreferredTimeBaseTicks = m_timeBases[m_cbTimeBase.SelectedIndex];
                m_eventData.PreferredTimeBaseIsAuto = false;
            }

            // event selection
            m_eventData.JobTriggerEvent = (m_cbJobTrigger.SelectedItem as JobTriggerCbItem)?.m_jobTrigger ?? JobTrigger.Incoming;
            if (m_treeEvents != null)
            {
                if (m_cbAutoSelectAll.Checked)
                {
                    m_eventData.MonitorAllEvents = true;
                    m_eventData.EventIDs = new List<string>();
                }
                else
                {
                    m_eventData.MonitorAllEvents = false;
                    m_eventData.EventIDs = new List<string>(m_treeEvents.GetCheckedSignalIds());
                }
            }
            else if (m_bEventServerChanged)
            {
                m_eventData.MonitorAllEvents = false;
                m_eventData.EventIDs = new List<string>();
            }
            else
            {
                //Keep old settings
            }
        }
        #endregion

        public void LeaveCleanup() {}

        private void m_cbRetry_CheckedChanged(object sender, EventArgs e)
        {
            m_retryUpDown.Enabled = m_cbRetry.Checked;
        }

        #region Event HD server
        class JobTriggerCbItem : IEquatable<JobTriggerCbItem>
        {
            private string m_title;
            public JobTrigger m_jobTrigger;
            public override string ToString()
            {
                return m_title;
            }
            public JobTriggerCbItem(JobTrigger jobTrigger)
            {
                m_title = "??";
                switch (jobTrigger)
                {
                    case JobTrigger.Incoming:
                        m_title = Properties.Resources.EventJobTrigger_Incoming;
                        break;
                    case JobTrigger.Outgoing:
                        m_title = Properties.Resources.EventJobTrigger_Outgoing;
                        break;
                    case JobTrigger.Both:
                        m_title = Properties.Resources.EventJobTrigger_Both;
                        break;
                }
                m_jobTrigger = jobTrigger;
            }

            #region IEquatable
            public override int GetHashCode()
            {
                return m_jobTrigger.GetHashCode();
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as JobTriggerCbItem);
            }

            public bool Equals(JobTriggerCbItem other)
            {
                return other != null && other.m_jobTrigger == m_jobTrigger;
            }
            #endregion
        }

        private void btnEventServer_Click(object sender, EventArgs e)
        {
            int port = 0;
            if (!int.TryParse(tbEventServerPort.Text, out port))
                port = 9180;

            string newServer = string.Empty;
            int newPort = 0;
            
            using (HdFormServerPicker serverPicker = new HdFormServerPicker(tbEventServer.Text, port))
            {
                serverPicker.SetCheckedFeatures(new List<ReaderFeature>() { ReaderFeature.Event }, new List<WriterFeature>());
                if (serverPicker.ShowDialog() != DialogResult.OK)
                    return;

                newServer = serverPicker.SelectedServer;
                newPort = serverPicker.SelectedPort;
            }

            if (tbEventServer.Text == newServer && tbEventServerPort.Text == newPort.ToString())
                return;

            m_bEventServerChanged = true;
            ChangeEventServer(newServer, newPort);
        }

        void ChangeEventServer(string server, int port)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, int>(ChangeEventServer), server, port);
                return;
            }

            tbEventServer.Text = server;
            tbEventServerPort.Text = port.ToString();

            fpnlEvent.Controls.Clear();

            if (m_treeEvents != null)
                m_treeEvents.CheckedChanged -= m_treeEvents_CheckedChanged;
            m_treeEvents = null;

            Task.Factory.StartNew((stateObj) =>
            {
                object[] stateParams = stateObj as object[];
                string srv = stateParams[0] as string;
                int prt = (int)stateParams[1];

                m_hdReader.ConnectionChanged -= OnHdConnectionChanged;
                if (m_hdReader.IsConnected())
                    m_hdReader.Disconnect();

                m_hdReader.Connect(srv, prt);
                OnHdConnectionChanged();
                m_hdReader.ConnectionChanged += OnHdConnectionChanged;
            }, new object[] { server, port });
        }

        void OnHdConnectionChanged()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnHdConnectionChanged));
                return;
            }

            fpnlEvent.Controls.Clear();

            if (!m_hdReader.IsConnected() && m_treeEvents == null)
            {
                if (m_lbErrorEventServer == null)
                {
                    m_lbErrorEventServer = new Button();
                    m_lbErrorEventServer.FlatAppearance.BorderColor = BackColor;
                    m_lbErrorEventServer.FlatAppearance.BorderSize = 0;
                    m_lbErrorEventServer.FlatAppearance.MouseDownBackColor = BackColor;
                    m_lbErrorEventServer.FlatAppearance.MouseOverBackColor = BackColor;
                    m_lbErrorEventServer.FlatStyle = FlatStyle.Flat;
                    m_lbErrorEventServer.Image = Properties.Resources.img_error;
                    m_lbErrorEventServer.ImageAlign = ContentAlignment.MiddleLeft;
                    m_lbErrorEventServer.TextAlign = ContentAlignment.MiddleLeft;
                    m_lbErrorEventServer.TextImageRelation = TextImageRelation.ImageBeforeText;
                    m_lbErrorEventServer.AutoSize = true;
                    m_lbErrorEventServer.ForeColor = Color.Red;
                }

                m_lbErrorEventServer.Text = m_hdReader.ConnectionError;

                fpnlEvent.Controls.Add(m_lbErrorEventServer);
            }
            else if (m_treeEvents == null)
            {
                if (m_cbAutoSelectAll == null)
                {
                    m_cbAutoSelectAll = new CheckBox();
                    m_cbAutoSelectAll.Text = Properties.Resources.eventJob_AutoSelect;
                    m_cbAutoSelectAll.CheckedChanged += m_cbAutoSelectAll_CheckedChanged;
                }

                if (m_bEventServerChanged)
                    m_cbAutoSelectAll.Checked = false;
                else
                    m_cbAutoSelectAll.Checked = m_eventData.MonitorAllEvents;

                fpnlEvent.Controls.Add(m_cbAutoSelectAll);

                m_treeEvents = m_hdReader.CreateSignalTree(false);
                m_treeEvents.BeginStateChange();
                m_treeEvents.ShowCheckboxes = true;
                m_treeEvents.SetComparer(new PdaSignalComparer());
                m_treeEvents.StoreTypeFilter = HdTreeTypeFilter.Event;
                m_treeEvents.LogicalFilter = HdTreeLogicalFilter.Event | HdTreeLogicalFilter.Annotation;
                m_treeEvents.ContextOptions = HdTreeContextOptions.None;
                m_treeEvents.EndStateChange();
                m_treeEvents.ExpandAll();
                m_treeEvents.Control.MaximumSize = new Size(int.MaxValue, 165);

                if (!m_bEventServerChanged && !m_cbAutoSelectAll.Checked)
                    m_treeEvents.CheckSignalIds(m_eventData.EventIDs);
                else if (m_cbAutoSelectAll.Checked)
                    m_treeEvents.CheckAll();

                m_treeEvents.CheckedChanged += m_treeEvents_CheckedChanged;

                fpnlEvent.Controls.Add(m_treeEvents.Control);
            }
            else
            {
                fpnlEvent.Controls.Add(m_cbAutoSelectAll);
                fpnlEvent.Controls.Add(m_treeEvents.Control);
            }
        }

        private void m_treeEvents_CheckedChanged()
        {
            m_cbAutoSelectAll.Checked = false;
        }

        private void m_cbAutoSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (m_treeEvents != null && m_cbAutoSelectAll.Checked)
            {
                m_treeEvents.CheckedChanged -= m_treeEvents_CheckedChanged;
                m_treeEvents.CheckAll();
                m_treeEvents.CheckedChanged += m_treeEvents_CheckedChanged;
            }
        }
        #endregion

        #region Time selection
        private void UpdateTimeControls(bool start)
        {
            if (start)
            {
                DisableStartChangedEvent = true;
                m_nudStartDays.Value = Math.Min(m_nudStartDays.Maximum, m_tsStart.Days);
                m_nudStartHours.Value = m_tsStart.Hours;
                m_nudStartMinutes.Value = m_tsStart.Minutes;
                m_nudStartSeconds.Value = m_tsStart.Seconds;
                DisableStartChangedEvent = false;

            }
            else
            {
                DisableStopChangedEvent = true;
                m_nudStopDays.Value = Math.Min(m_nudStopDays.Maximum, m_tsStop.Days);
                m_nudStopHours.Value = m_tsStop.Hours;
                m_nudStopMinutes.Value = m_tsStop.Minutes;
                m_nudStopSeconds.Value = m_tsStop.Seconds;
                DisableStopChangedEvent = false;
            }
            m_cbTimeBase.Invalidate();
            m_cbTimeBase_SelectedIndexChanged(null, null);
        }

        private void OnStartChanged(object sender, EventArgs e)
        {
            if(DisableStartChangedEvent)
                return;

            try
            {
                DisableStartChangedEvent = true;
                TimeSpan sp = new TimeSpan((int)m_nudStartDays.Value, (int)m_nudStartHours.Value, (int)m_nudStartMinutes.Value, (int)m_nudStartSeconds.Value);

                if(Control.ModifierKeys == Keys.Control)
                {
                    if(sender == m_nudStartDays)
                    {
                        sp = TimeSpan.FromDays((int)m_nudStartDays.Value);
                    }
                    else if(sender == m_nudStartHours)
                    {
                        sp = TimeSpan.FromHours((int)m_nudStartHours.Value);
                    }
                    else if(sender == m_nudStartMinutes)
                    {
                        sp = TimeSpan.FromMinutes((int)m_nudStartMinutes.Value);
                    }
                    else if(sender == m_nudStartSeconds)
                    {
                        sp = TimeSpan.FromSeconds((int)m_nudStartSeconds.Value);
                    }
                }

                long diff = TimeSpan.FromSeconds(1).Ticks;
                if (sp.Ticks >= m_tsStop.Ticks + diff)
                {
                    Start = sp;
                }
                else if (sp.Ticks >= diff)
                {
                    Start = sp;
                    Stop = TimeSpan.FromTicks(sp.Ticks - diff);
                }
                else
                {
                    Stop = TimeSpan.FromTicks(0);
                    Start = TimeSpan.FromTicks(diff);
                }
            }
            finally
            {
                DisableStartChangedEvent = false;
            }
        }

        private void OnStopChanged(object sender, EventArgs e)
        {
            if(DisableStopChangedEvent)
                return;

            try
            {
                DisableStopChangedEvent = true;

                TimeSpan sp = new TimeSpan((int)m_nudStopDays.Value, (int)m_nudStopHours.Value, (int)m_nudStopMinutes.Value, (int)m_nudStopSeconds.Value);

                if(Control.ModifierKeys == Keys.Control)
                {
                    if(sender == m_nudStopDays)
                    {
                        sp = TimeSpan.FromDays((int)m_nudStopDays.Value);
                    }
                    else if(sender == m_nudStopHours)
                    {
                        sp = TimeSpan.FromHours((int)m_nudStopHours.Value);
                    }
                    else if(sender == m_nudStopMinutes)
                    {
                        sp = TimeSpan.FromMinutes((int)m_nudStopMinutes.Value);
                    }
                    else if(sender == m_nudStopSeconds)
                    {
                        sp = TimeSpan.FromSeconds((int)m_nudStopSeconds.Value);
                    }
                }
                if(sp.Ticks < 0) sp = TimeSpan.Zero;
                Stop = sp;

                long diff = TimeSpan.FromSeconds(1).Ticks;
                if(m_tsStart.Ticks < sp.Ticks + diff)
                {
                    Start = TimeSpan.FromTicks(sp.Ticks + diff);
                }
            }
            finally
            {
                DisableStopChangedEvent = false;
            }
        }

        private void m_cbTimeBase_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0)
                return;

            string tooltipText = null;
            Brush brush = null;
            bool unknowntba = false;
            switch (CheckTimeBaseAcceptability(((ComboBox)sender).Items[e.Index] as TimeCbItem))
            {
                case TimeBaseAcceptability.Unknown:
                    brush = new SolidBrush(SystemColors.WindowText);
                    unknowntba = true;
                    break;
                case TimeBaseAcceptability.Allowed:
                    brush = Brushes.Green;
                    tooltipText = iba.Properties.Resources.TooltipGreen;
                    break;
                case TimeBaseAcceptability.Questionable:
                    brush = Brushes.DarkOrange;
                    tooltipText = iba.Properties.Resources.TooltipOrange;
                    break;
                case TimeBaseAcceptability.Forbidden:
                    brush = Brushes.Red;
                    tooltipText = iba.Properties.Resources.TooltipRed;
                    break;
            }

            string text = ((ComboBox)sender).Items[e.Index].ToString();
            if(e.Index == 0)
            {
                if (unknowntba)
                    text = string.Format(text, "?");
                else
                    text = string.Format(text, GetAutoItem());
            }

            if (e.State.HasFlag(DrawItemState.Focus) || e.State.HasFlag(DrawItemState.Selected))
                e.Graphics.FillRectangle(new SolidBrush(Color.Lavender), e.Bounds);
            else
                e.DrawBackground();

            e.Graphics.DrawString(text, ((Control)sender).Font, brush, e.Bounds.X, e.Bounds.Y);
            if (e.State.HasFlag(DrawItemState.Selected) && tooltipText != null && m_cbTimeBase.DroppedDown)
            {
                m_toolTip.Show(tooltipText, m_cbTimeBase, e.Bounds.Right, e.Bounds.Bottom + m_cbTimeBase.Height);
            }
        }

        private TimeCbItem GetAutoItem() //auto
        {
            for (int i = 1; i < m_cbTimeBase.Items.Count; i++)
            {
                TimeCbItem item = m_cbTimeBase.Items[i] as TimeCbItem;
                if (CheckTimeBaseAcceptability(item) == TimeBaseAcceptability.Allowed)
                    return item;
            }
            //none ok, return last
            return m_cbTimeBase.Items[m_cbTimeBase.Items.Count - 1] as TimeCbItem;
        }

        private void m_cbTimeBase_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_cbTimeBase.SelectedItem != null)
            {
                switch (CheckTimeBaseAcceptability(m_cbTimeBase.SelectedItem as TimeCbItem))
                {
                    case TimeBaseAcceptability.Unknown:
                        m_toolTip.SetToolTip(m_cbTimeBase, null);
                        break;
                    case TimeBaseAcceptability.Allowed:
                        m_toolTip.SetToolTip(m_cbTimeBase, iba.Properties.Resources.TooltipGreen);
                        break;
                    case TimeBaseAcceptability.Questionable:
                        m_toolTip.SetToolTip(m_cbTimeBase, iba.Properties.Resources.TooltipOrange);
                        break;
                    case TimeBaseAcceptability.Forbidden:
                        m_toolTip.SetToolTip(m_cbTimeBase, iba.Properties.Resources.TooltipRed);
                        break;
                }
            }
            else
                m_toolTip.SetToolTip(m_cbTimeBase, null);
        }

        private void m_cbTimeBase_DropDownClosed(object sender, EventArgs e)
        {
            m_toolTip.Hide(m_cbTimeBase);
        }

        double ReqTimeBaseFactor()
        {
            return Math.Sqrt(40.0); //in the future we might need to ask the reader
        }

        TimeBaseAcceptability CheckTimeBaseAcceptability(TimeCbItem item)
        {
            long duration = Start.Ticks - Stop.Ticks;
            if (duration <= 0)
                return TimeBaseAcceptability.Unknown;

            if (item.m_timebaseLength == 0)
                return TimeBaseAcceptability.Allowed; //auto

            double samples = ((double)(duration)) / ((double)(item.m_timebaseLength)) * ReqTimeBaseFactor();
            if (samples > 1.0e9)
                return TimeBaseAcceptability.Forbidden;
            else if (samples > 1.0e7)
                return TimeBaseAcceptability.Questionable;
            else
                return TimeBaseAcceptability.Allowed;
        }
        #endregion
    }
}
