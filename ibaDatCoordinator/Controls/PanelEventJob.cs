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
using iba.Logging;
using iba.Dialogs;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace iba.Controls
{
    public partial class PanelEventJob : UserControl, IPropertyPane
    {
        #region Members
        IPropertyPaneManager m_manager;
        ConfigurationData m_confData;
        EventJobData m_eventData;

        IHdReader m_hdReader;

        string m_username;
        string m_password;

        List<string> m_currEvents;
        List<string> m_currStores;
        IHdSignalTree m_treeEvents;

        static ImageList imageListError;
        ListViewItem m_lviErrorStores;

        private long[] m_timeBases;

        const int locationOffsetXRangeCenter = 150; //400 - m_pbRangeCenter.Location.X(= 255)
        #endregion

        #region Initialize
        static PanelEventJob()
        {
            //DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            //defaultLookAndFeel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            //defaultLookAndFeel.LookAndFeel.UseWindowsXPTheme = true;

            imageListError = new ImageList();
            imageListError.Images.Add(Properties.Resources.img_error);
        }

        public PanelEventJob()
        {
            InitializeComponent();
            ((Bitmap)m_applyToRunningBtn.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_undoChangesBtn.Image).MakeTransparent(Color.Magenta);

            m_currEvents = new List<string>();
            m_currStores = new List<string>();

            m_username = "";
            m_password = "";

            m_hdReader = HdClient.CreateReader(HdUserType.PdaClient);
            object obj = m_hdReader.Authenticate(null);
            obj = HdReaderAuthenticator.GetInfo(obj);
            obj = m_hdReader.Authenticate(obj);
            m_hdReader.ShowConnectionError = false;

            m_hdReader.UserLoginInfo.UserName = m_username;
            m_hdReader.UserLoginInfo.Password = m_password;
            m_hdReader.UserLoginInfo.AllowSavePassword = false;
            m_hdReader.UserLoginInfo.SavePassword = true;

            m_hdReader.UserLoginOptions = HdUserLoginOptions.Never;

            m_treeEvents = m_hdReader.CreateSignalTree(false);
            m_treeEvents.BeginStateChange();
            m_treeEvents.ShowCheckboxes = true;
            m_treeEvents.SetComparer(new PdaSignalComparer());
            m_treeEvents.StoreTypeFilter = HdTreeTypeFilter.Event;
            m_treeEvents.LogicalFilter = HdTreeLogicalFilter.Event;
            m_treeEvents.ContextOptions = HdTreeContextOptions.None;
            m_treeEvents.EndStateChange();
            m_treeEvents.Control.Size = new Size(btnHdServer.Right-label2.Left, 165);
            m_treeEvents.Control.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            m_pnlEvents.Controls.Add(m_treeEvents.Control);

            m_rbtIncoming.Tag = EventJobRangeCenter.Incoming;
            m_rbtOutgoing.Tag = EventJobRangeCenter.Outgoing;
            m_rbtBoth.Tag = EventJobRangeCenter.Both;

            string[] itemStrs = iba.Properties.Resources.TimeSelectionChoices.Split(';');
            long ms = 10000; //10000 * 100 nanosec = 1 ms
            long s = 1000 * ms;
            m_timeBases = new long[] { 0, 1 * ms, 10 * ms, 100 * ms, s, 60 * s, 3600 * s, 24 * 3600 * s };
            int n = itemStrs.Count();
            for (int i = 0; i < n; i++)
                m_cbTimeBase.Items.Add(new TimeCbItem(itemStrs[i], m_timeBases[i]));

            UpdateStoreTree();

            m_hdReader.ConnectionChanged += OnHdConnectionChanged;
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

            //range selection
            switch (m_eventData.RangeCenter)
            {
                case EventJobRangeCenter.Incoming:
                    m_rbtIncoming.Checked = true;
                    break;
                case EventJobRangeCenter.Outgoing:
                    m_rbtOutgoing.Checked = true;
                    break;
                case EventJobRangeCenter.Both:
                    m_rbtBoth.Checked = true;
                    break;
            }

            m_cbPreTrigger.Checked = m_eventData.EnablePreTriggerRange;
            m_cbPostTrigger.Checked = m_eventData.EnablePostTriggerRange;

            m_nudDaysMax.Value = Math.Min(m_nudDaysMax.Maximum, m_eventData.MaxTriggerRange.Days);
            m_nudHoursMax.Value = Math.Min(m_nudHoursMax.Maximum, m_eventData.MaxTriggerRange.Hours);
            m_nudMinsMax.Value = Math.Min(m_nudMinsMax.Maximum, m_eventData.MaxTriggerRange.Minutes);
            m_nudSecsMax.Value = Math.Min(m_nudSecsMax.Maximum, m_eventData.MaxTriggerRange.Seconds);

            m_nudDaysPre.Value = Math.Min(m_nudDaysPre.Maximum, m_eventData.PreTriggerRange.Days);
            m_nudHoursPre.Value = Math.Min(m_nudHoursPre.Maximum, m_eventData.PreTriggerRange.Hours);
            m_nudMinsPre.Value = Math.Min(m_nudMinsPre.Maximum, m_eventData.PreTriggerRange.Minutes);
            m_nudSecsPre.Value = Math.Min(m_nudSecsPre.Maximum, m_eventData.PreTriggerRange.Seconds);

            m_nudDaysPost.Value = Math.Min(m_nudDaysPost.Maximum, m_eventData.PostTriggerRange.Days);
            m_nudHoursPost.Value = Math.Min(m_nudHoursPost.Maximum, m_eventData.PostTriggerRange.Hours);
            m_nudMinsPost.Value = Math.Min(m_nudMinsPost.Maximum, m_eventData.PostTriggerRange.Minutes);
            m_nudSecsPost.Value = Math.Min(m_nudSecsPost.Maximum, m_eventData.PostTriggerRange.Seconds);

            m_cbTimeBase.SelectedIndex = m_eventData.PreferredTimeBaseIsAuto ? 0 : Array.FindIndex(m_timeBases, ticks => ticks == m_eventData.PreferredTimeBaseTicks);

            //event selection
            m_currEvents = new List<string>(m_eventData.EventIDs);
            m_currStores = new List<string>(m_eventData.HDStores);
            ChangeHDServer(m_eventData.HDServer, m_eventData.HDPort, m_eventData.HDUsername, m_eventData.HDPassword);
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
            m_eventData.HDServer = m_tbEventServer.Text ?? string.Empty;
            int port = 0;
            m_eventData.HDPort = int.TryParse(m_tbEventServerPort.Text, out port) ? port : -1;
            m_eventData.HDUsername = m_username ?? string.Empty;
            m_eventData.HDPassword = m_password ?? string.Empty;

            m_eventData.HDStores = m_currStores.ToArray();

            //range selection
            if (m_rbtIncoming.Checked)
                m_eventData.RangeCenter = (EventJobRangeCenter)m_rbtIncoming.Tag;
            else if (m_rbtOutgoing.Checked)
                m_eventData.RangeCenter = (EventJobRangeCenter)m_rbtOutgoing.Tag;
            else if (m_rbtBoth.Checked)
                m_eventData.RangeCenter = (EventJobRangeCenter)m_rbtBoth.Tag;

            m_eventData.EnablePreTriggerRange = m_cbPreTrigger.Checked;
            m_eventData.EnablePostTriggerRange = m_cbPostTrigger.Checked;

            m_eventData.MaxTriggerRange = new TimeSpan((int)m_nudDaysMax.Value, (int)m_nudHoursMax.Value, (int)m_nudMinsMax.Value, (int)m_nudSecsMax.Value);
            m_eventData.PreTriggerRange = new TimeSpan((int)m_nudDaysPre.Value, (int)m_nudHoursPre.Value, (int)m_nudMinsPre.Value, (int)m_nudSecsPre.Value);
            m_eventData.PostTriggerRange = new TimeSpan((int)m_nudDaysPost.Value, (int)m_nudHoursPost.Value, (int)m_nudMinsPost.Value, (int)m_nudSecsPost.Value);

            if (m_cbTimeBase.SelectedIndex == 0) //auto
            {
                m_eventData.PreferredTimeBaseTicks = GetAutoItem().m_timebaseLength;
                m_eventData.PreferredTimeBaseIsAuto = true;
            }
            else if (m_cbTimeBase.SelectedIndex > 0)
            {
                m_eventData.PreferredTimeBaseTicks = m_timeBases[m_cbTimeBase.SelectedIndex];
                m_eventData.PreferredTimeBaseIsAuto = false;
            }

            // event selection
            m_eventData.EventIDs = new List<string>(m_currEvents);
        }
        #endregion

        public void LeaveCleanup() {}

        private void m_cbRetry_CheckedChanged(object sender, EventArgs e)
        {
            m_retryUpDown.Enabled = m_cbRetry.Checked;
        }

        #region Event HD server
        private void btnHdServer_Click(object sender, EventArgs e)
        {
            int port = 0;
            if (!int.TryParse(m_tbEventServerPort.Text, out port))
                port = 9180;

            string newServer = string.Empty;
            int newPort = 0;

            using (HdFormServerPicker serverPicker = new HdFormServerPicker(m_tbEventServer.Text, port))
            {
                List<ReaderFeature> features = new List<ReaderFeature>() { ReaderFeature.Event };
                features.AddRange(ReaderFeature.Analyzer);
                serverPicker.SetCheckedFeatures(features, new List<WriterFeature>());
                if (serverPicker.ShowDialog() != DialogResult.OK)
                    return;

                newServer = serverPicker.SelectedServer;
                newPort = serverPicker.SelectedPort;
            }

            m_currEvents.Clear();
            m_currStores.Clear();
            ChangeHDServer(newServer, newPort, m_username, m_password, true);
        }

        void ChangeHDServer(string server, int port, string username, string password, bool forceLoginForm = false)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, int, string, string, bool>(ChangeHDServer), server, port, username, password, forceLoginForm);
                return;
            }

            username = username ?? "";
            password = password ?? "";

            m_tbEventServer.Text = server;
            m_tbEventServerPort.Text = port.ToString();

            m_lvStores.Items.Clear();

            Task.Factory.StartNew((stateObj) =>
            {
                Tuple<string, int> tpl = stateObj as Tuple<string, int>;

                if (m_hdReader != null)
                    m_hdReader.ConnectionChanged -= OnHdConnectionChanged;

                m_treeEvents.CheckedChanged -= m_treeEvents_CheckedChanged;

                if (m_hdReader != null && m_hdReader.IsConnected())
                    m_hdReader.Disconnect();

                m_hdReader.UserLoginInfo.Parent = this;
                m_hdReader.UserLoginInfo.UserName = username;
                m_hdReader.UserLoginInfo.Password = password;
                m_hdReader.UserLoginOptions = forceLoginForm ? HdUserLoginOptions.Always : HdUserLoginOptions.Failure;

                m_hdReader?.Connect(tpl.Item1, tpl.Item2);
                m_hdReader.UserLoginOptions = HdUserLoginOptions.Never;

                m_username = m_hdReader.UserLoginInfo.UserName;
                m_password = m_hdReader.UserLoginInfo.Password;

                OnHdConnectionChanged();

                if (m_hdReader != null)
                    m_hdReader.ConnectionChanged += OnHdConnectionChanged;
            }, Tuple.Create(server, port));
        }

        void OnHdConnectionChanged()
        {
            //Remove handler here; otherwise selection gets reset on a reconnect
            //Handler will be readded on reconnect in UpdateEventTree
            if (m_hdReader == null || !m_hdReader.IsConnected())
                m_treeEvents.CheckedChanged -= m_treeEvents_CheckedChanged;

            UpdateTrees();
        }

        void UpdateTrees()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateTrees));
                return;
            }

            UpdateEventTree();
            UpdateStoreTree();
        }

        void UpdateEventTree()
        {
            if (m_hdReader == null || !m_hdReader.IsConnected())
                return;

            m_treeEvents.CheckedChanged -= m_treeEvents_CheckedChanged;
            m_treeEvents.CheckSignalIds(m_currEvents);
            m_treeEvents.ExpandAll();
            m_treeEvents.CheckedChanged += m_treeEvents_CheckedChanged;
        }

        void UpdateStoreTree()
        {
            m_lvStores.ItemChecked -= m_lvStores_ItemChecked;
            m_lvStores.Items.Clear();
            if (m_hdReader == null || !m_hdReader.IsConnected())
            {
                m_lvStores.CheckBoxes = false;
                m_lvStores.SmallImageList = imageListError;
                if (m_lviErrorStores == null)
                {
                    m_lviErrorStores = new ListViewItem();
                    m_lviErrorStores.ImageIndex = 0;
                }
                if (String.IsNullOrEmpty(m_hdReader.ServerHost)) //don't show an error message if server is not specified yet
                    m_lviErrorStores.Text = Properties.Resources.NoHDServerConnected;
                else
                    m_lviErrorStores.Text = m_hdReader == null || string.IsNullOrWhiteSpace(m_hdReader.ConnectionError) ? Properties.Resources.EventJob_DefaultHDConnectionErr : m_hdReader.ConnectionError;

                m_lvStores.Items.Add(m_lviErrorStores);
            }
            else
            {
                m_lvStores.CheckBoxes = true;
                m_lvStores.SmallImageList = HdTreeNodes.ImageList;

                foreach (var store in m_hdReader.Stores)
                {
                    if (store.IsEnabled() && store.Type == HdStoreType.Time && !store.Id.StoreName.Contains("<DIAGNOSTIC>"))
                        m_lvStores.Items.Add(store.Id.StoreName, HdTreeNodes.GetImageIndex(store.Type, store.IsBackup(), store.IsEnabled()));
                }

                foreach (var name in m_currStores)
                {
                    foreach (ListViewItem item in m_lvStores.Items)
                    {
                        if (item.Text == name)
                        {
                            item.Checked = true;
                            break;
                        }
                    }
                }
            }
            m_lvStores.ItemChecked += m_lvStores_ItemChecked;
        }

        private void m_treeEvents_CheckedChanged()
        {
            if (m_hdReader == null || !m_hdReader.IsConnected())
                return;

            m_currEvents = new List<string>(m_treeEvents.GetCheckedSignalIds());
        }

        private void m_lvStores_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (m_hdReader == null || !m_hdReader.IsConnected())
                return;

            m_currStores = new List<string>();
            foreach (ListViewItem item in m_lvStores.CheckedItems)
                m_currStores.Add(item.Text);
        }
        #endregion

        #region Range selection
        void ChangeVisibilityMaxRange(bool bShow)
        {
            m_nudDaysMax.Visible = m_nudHoursMax.Visible = m_nudMinsMax.Visible = m_nudSecsMax.Visible = bShow;
            m_lblMaxCaption.Visible = m_lblMaxDays.Visible = m_lblMaxHours.Visible = m_lblMaxMins.Visible = m_lblMaxSecs.Visible = bShow;
            m_lbMaximum.Visible = bShow;
        }

        private void m_cbPreTrigger_CheckedChanged(object sender, EventArgs e)
        {
            m_nudDaysPre.Enabled = m_nudHoursPre.Enabled = m_nudMinsPre.Enabled = m_nudSecsPre.Enabled = m_cbPreTrigger.Checked;
            m_cbTimeBase.Invalidate();
            m_cbTimeBase_SelectedIndexChanged(null, null);
        }

        private void m_cbPostTrigger_CheckedChanged(object sender, EventArgs e)
        {
            m_nudDaysPost.Enabled = m_nudHoursPost.Enabled = m_nudMinsPost.Enabled = m_nudSecsPost.Enabled = m_cbPostTrigger.Checked;
            m_cbTimeBase.Invalidate();
            m_cbTimeBase_SelectedIndexChanged(null, null);
        }

        private void rbtRangeCenter_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rbt = sender as RadioButton;
            if (rbt == null)
                return;

            if (rbt.Tag == null || !(rbt.Tag is EventJobRangeCenter))
                return;

            //X constants are based on pixel distances in the images
            //Y constants are based on designer distances of the labels
            EventJobRangeCenter lRangeCenter = (EventJobRangeCenter)rbt.Tag;
            if (lRangeCenter == EventJobRangeCenter.Both)
            {
                ChangeVisibilityMaxRange(true);
                m_pbRangeCenter.Image = Properties.Resources.img_eventjob_range_both;

                m_lbOutgoing.Visible = true;
                m_lbIncoming.Visible = true;

                m_lbIncoming.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - 50 - m_lbIncoming.Width / 2), m_pbRangeCenter.Location.Y - 3);
                m_lbOutgoing.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter + 50 - m_lbOutgoing.Width / 2), m_pbRangeCenter.Location.Y - 3);
                m_lbPre.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - 50 - 37 - m_lbPre.Width / 2), m_pbRangeCenter.Location.Y + 35);
                m_lbPost.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter + 50 + 40 - m_lbPost.Width / 2), m_pbRangeCenter.Location.Y + 35);
                m_lbMaximum.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - m_lbMaximum.Width / 2), m_pbRangeCenter.Location.Y + 60);
            }
            else
            {
                ChangeVisibilityMaxRange(false);
                m_pbRangeCenter.Image = Properties.Resources.img_eventjob_range_single;

                if (lRangeCenter == EventJobRangeCenter.Incoming)
                {
                    m_lbOutgoing.Visible = false;
                    m_lbIncoming.Visible = true;

                    m_lbIncoming.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - m_lbIncoming.Width / 2), m_pbRangeCenter.Location.Y - 3);
                }
                else
                {
                    m_lbOutgoing.Visible = true;
                    m_lbIncoming.Visible = false;

                    m_lbOutgoing.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - m_lbOutgoing.Width / 2), m_pbRangeCenter.Location.Y - 3);
                }

                m_lbPre.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - 37 - m_lbPre.Width / 2), m_pbRangeCenter.Location.Y + 35);
                m_lbPost.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter + 40 - m_lbPost.Width / 2), m_pbRangeCenter.Location.Y + 35);
                //m_lbMaximum.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - m_lbMaximum.Width / 2), m_pbRangeCenter.Location.Y + 60);
            }

            m_cbTimeBase.Invalidate();
            m_cbTimeBase_SelectedIndexChanged(null, null);
        }

        private void m_nud_ValueChanged(object sender, EventArgs e)
        {
            m_cbTimeBase.Invalidate();
            m_cbTimeBase_SelectedIndexChanged(null, null);
        }

        double ReqTimeBaseFactor()
        {
            return Math.Sqrt(40.0); //in the future we might need to ask the reader
        }

        TimeBaseAcceptability CheckTimeBaseAcceptability(TimeCbItem item)
        {
            long duration = 0;
            if (m_rbtBoth.Checked)
                duration = new TimeSpan((int)m_nudDaysMax.Value, (int)m_nudHoursMax.Value, (int)m_nudMinsMax.Value, (int)m_nudSecsMax.Value).Ticks;
            else
            {
                if (m_cbPreTrigger.Checked)
                    duration = new TimeSpan((int)m_nudDaysPre.Value, (int)m_nudHoursPre.Value, (int)m_nudMinsPre.Value, (int)m_nudSecsPre.Value).Ticks;
                if (m_cbPostTrigger.Checked)
                    duration += new TimeSpan((int)m_nudDaysPost.Value, (int)m_nudHoursPost.Value, (int)m_nudMinsPost.Value, (int)m_nudSecsPost.Value).Ticks;
            }

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

        TimeCbItem GetAutoItem() //auto
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
            if (e.Index == 0)
            {
                if (unknowntba)
                    text = String.Format(text, "?");
                else
                    text = String.Format(text, GetAutoItem());
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
        #endregion

        #region Test file
        private void m_btGenerateTest_Click(object sender, EventArgs e)
        {
            SaveData();

            try
            {
                string filename = string.Empty;
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.AddExtension = true;
                    dlg.Title = Properties.Resources.EventJob_GenerateTestFile;
                    dlg.Filter = Properties.Resources.EventJob_GenerateTestFile_Filter;
                    dlg.DefaultExt = ".hdq";

                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;

                    filename = dlg.FileName;
                }

                if (string.IsNullOrWhiteSpace(filename))
                    return;


                List<string> generatedFiles = null;
                using (GenerateEventJobTestFileDlg dlg = new GenerateEventJobTestFileDlg(m_confData, filename))
                {
                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;

                    generatedFiles = dlg.GeneratedFiles;
                }

                if (generatedFiles == null || generatedFiles.Count <= 0)
                    return;

                foreach (var file in generatedFiles)
                {
                    if (File.Exists(file))
                    {
                        ProcessStartInfo si = new ProcessStartInfo(file);
                        Process.Start(si);
                    }
                }
            }
            catch (Exception ex)
            {
                ibaLogger.LogFormat(Level.Exception, "Generating an HDQ test file failed: {0}", ex.Message);
            }
        }
        #endregion
    }
}
