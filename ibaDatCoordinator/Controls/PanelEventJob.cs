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

namespace iba.Controls
{
    public partial class PanelEventJob : UserControl, IPropertyPane
    {
        #region Members
        HdControlStorePicker m_hdStorePicker;

        IPropertyPaneManager m_manager;
        ConfigurationData m_confData;
        EventJobData m_eventData;

        IHdReader m_hdReader;

        List<string> m_currEvents;
        IHdSignalTree m_treeEvents;

        static ImageList imageListError;
        ListViewItem m_lviErrorStores;

        bool m_bEventServerChanged;

        const int locationOffsetXRangeCenter = 145; //400 - m_pbRangeCenter.Location.X(= 255)
        #endregion

        #region Initialize
        static PanelEventJob()
        {
            DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            defaultLookAndFeel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            defaultLookAndFeel.LookAndFeel.UseWindowsXPTheme = true;

            imageListError = new ImageList();
            imageListError.Images.Add(Properties.Resources.img_error);
        }

        public PanelEventJob()
        {
            InitializeComponent();
            ((Bitmap)m_applyToRunningBtn.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_undoChangesBtn.Image).MakeTransparent(Color.Magenta);

            m_currEvents = new List<string>();

            m_hdReader = HdClient.CreateReader(HdUserType.Analyzer);
            m_hdReader.ShowConnectionError = false;

            m_hdStorePicker = new HdControlStorePicker();
            m_hdStorePicker.Dock = DockStyle.Fill;
            m_hdStorePicker.Margin = new Padding(0, 0, 0, 0);
            m_pnlServer.Controls.Add(m_hdStorePicker);
            m_hdStorePicker.SelectedPort = 9180;
            m_hdStorePicker.SelectedServer = "localhost";
            m_hdStorePicker.HideConfigureButton();
            m_hdStorePicker.HideStoreSelection();
            List<ReaderFeature> features = new List<ReaderFeature>() { ReaderFeature.Event };
            features.AddRange(ReaderFeature.Analyzer);
            m_hdStorePicker.SetCheckedFeatures(features, new List<WriterFeature>());

            m_treeEvents = m_hdReader.CreateSignalTree(false);
            m_treeEvents.BeginStateChange();
            m_treeEvents.ShowCheckboxes = true;
            m_treeEvents.SetComparer(new PdaSignalComparer());
            m_treeEvents.StoreTypeFilter = HdTreeTypeFilter.Event;
            m_treeEvents.LogicalFilter = HdTreeLogicalFilter.Event | HdTreeLogicalFilter.Annotation;
            m_treeEvents.ContextOptions = HdTreeContextOptions.None;
            m_treeEvents.EndStateChange();
            m_treeEvents.Control.MaximumSize = new Size(int.MaxValue, 165);
            m_fpnlEvent.Controls.Add(m_treeEvents.Control);

            m_rbtIncoming.Tag = EventJobRangeCenter.Incoming;
            m_rbtOutgoing.Tag = EventJobRangeCenter.Outgoing;
            m_rbtBoth.Tag = EventJobRangeCenter.Both;

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
                m_hdStorePicker.SelectedServerChanged -= OnServerChanged;
                m_hdStorePicker.SelectedPortChanged -= OnServerChanged;

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

            //event selection
            m_currEvents = new List<string>(m_eventData.EventIDs);
            m_hdStorePicker.SelectedServerChanged -= OnServerChanged;
            m_hdStorePicker.SelectedPortChanged -= OnServerChanged;
            m_hdStorePicker.SelectedServer = m_eventData.HDServer;
            m_hdStorePicker.SelectedPort = m_eventData.HDPort;
            ChangeEventServer(m_eventData.HDServer, m_eventData.HDPort);
            m_hdStorePicker.SelectedServerChanged += OnServerChanged;
            m_hdStorePicker.SelectedPortChanged += OnServerChanged;
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

            List<string> stores = new List<string>();
            foreach (ListViewItem item in m_lvStores.Items)
            {
                if (item.Checked)
                    stores.Add(item.Text);
            }

            if (!m_bEventServerChanged && m_lvStores.Items.Count == 1 && m_lvStores.Items[0] == m_lviErrorStores)
            {
                //Keep old settings
            }
            else
                m_eventData.HDStores = stores.ToArray();

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

            // event selection
            m_eventData.EventIDs = new List<string>(m_currEvents);

            m_bEventServerChanged = false;
        }
        #endregion

        public void LeaveCleanup() {}

        private void m_cbRetry_CheckedChanged(object sender, EventArgs e)
        {
            m_retryUpDown.Enabled = m_cbRetry.Checked;
        }

        #region Event HD server
        void OnServerChanged()
        {
            m_bEventServerChanged = true;
            m_currEvents.Clear();
            ChangeEventServer(m_hdStorePicker.SelectedServer, m_hdStorePicker.SelectedPort);
        }

        void ChangeEventServer(string server, int port)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, int>(ChangeEventServer), server, port);
                return;
            }

            m_lvStores.Items.Clear();

            Task.Factory.StartNew((stateObj) =>
            {
                object[] stateParams = stateObj as object[];
                string srv = stateParams[0] as string;
                int prt = (int)stateParams[1];

                m_hdReader.ConnectionChanged -= OnHdConnectionChanged;
                m_treeEvents.CheckedChanged -= m_treeEvents_CheckedChanged;
                if (m_hdReader.IsConnected())
                    m_hdReader.Disconnect();

                m_hdReader.Connect(srv, prt);
                m_treeEvents.CheckedChanged += m_treeEvents_CheckedChanged;
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
            if (!m_hdReader.IsConnected())
            {
                m_lvStores.CheckBoxes = false;
                m_lvStores.SmallImageList = imageListError;
                if (m_lviErrorStores == null)
                {
                    m_lviErrorStores = new ListViewItem();
                    m_lviErrorStores.ImageIndex = 0;
                }

                m_lviErrorStores.Text = string.IsNullOrWhiteSpace(m_hdReader.ConnectionError) ? Properties.Resources.EventJob_DefaultHDConnectionErr : m_hdReader.ConnectionError;

                m_lvStores.Items.Clear();
                m_lvStores.Items.Add(m_lviErrorStores);
            }
            else if (m_lvStores.Items.Count <= 0 || m_lvStores.Items[0] == m_lviErrorStores)
            {
                m_lvStores.Items.Clear();
                m_lvStores.CheckBoxes = true;
                m_lvStores.SmallImageList = HdTreeControl.ImageList;

                foreach (var store in m_hdReader.Stores)
                {
                    if (store.IsEnabled() && store.Type == HdStoreType.Time && !store.Id.StoreName.Contains("<DIAGNOSTIC>"))
                        m_lvStores.Items.Add(store.Id.StoreName, HdTreeControl.GetImageIndex(store.Type, store.IsBackup(), store.IsEnabled()));
                }

                if (!m_bEventServerChanged)
                {
                    foreach (var name in m_eventData.HDStores)
                    {
                        foreach(ListViewItem item in m_lvStores.Items)
                        {
                            if (item.Text == name)
                            {
                                item.Checked = true;
                                break;
                            }
                        }
                    }
                }
            }
        }

        private void m_treeEvents_CheckedChanged()
        {
            if (m_hdReader == null || !m_hdReader.IsConnected())
                return;

            m_currEvents = new List<string>(m_treeEvents.GetCheckedSignalIds());
        }
        #endregion

        #region Range selection
        private void m_cbPreTrigger_CheckedChanged(object sender, EventArgs e)
        {
            m_nudDaysPre.Enabled = m_nudHoursPre.Enabled = m_nudMinsPre.Enabled = m_nudSecsPre.Enabled = m_cbPreTrigger.Checked;
        }

        private void m_cbPostTrigger_CheckedChanged(object sender, EventArgs e)
        {
            m_nudDaysPost.Enabled = m_nudHoursPost.Enabled = m_nudMinsPost.Enabled = m_nudSecsPost.Enabled = m_cbPostTrigger.Checked;
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
                m_pbRangeCenter.Image = Properties.Resources.img_eventjob_range_both;

                m_lbOutgoing.Visible = true;
                m_lbIncoming.Visible = true;

                m_lbIncoming.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - m_lbIncoming.Width / 2), m_pbRangeCenter.Location.Y - 3);
                m_lbOutgoing.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter + 120 - m_lbOutgoing.Width / 2), m_pbRangeCenter.Location.Y - 3);
                m_lbPre.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - 53 - m_lbPre.Width / 2), m_pbRangeCenter.Location.Y + 55);
                m_lbPost.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter + 120 + 52 - m_lbPost.Width / 2), m_pbRangeCenter.Location.Y + 55);
                m_lbMaximum.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter + 60 - m_lbMaximum.Width / 2), m_pbRangeCenter.Location.Y + 94);
            }
            else
            {
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

                m_lbPre.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - 52 - m_lbPre.Width / 2), m_pbRangeCenter.Location.Y + 55);
                m_lbPost.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter + 52 - m_lbPost.Width / 2), m_pbRangeCenter.Location.Y + 55);
                m_lbMaximum.Location = new Point(m_pbRangeCenter.Location.X + (locationOffsetXRangeCenter - m_lbMaximum.Width / 2), m_pbRangeCenter.Location.Y + 94);
            }
        }
        #endregion

        #region Test file
        private void m_btGenerateTest_Click(object sender, EventArgs e)
        {
            SaveData();

            try
            {
                DateTime dtNow = DateTime.UtcNow;

                TimeSpan preRange = m_eventData.EnablePreTriggerRange ? m_eventData.PreTriggerRange : TimeSpan.Zero;
                TimeSpan postRange = m_eventData.EnablePostTriggerRange ? m_eventData.PostTriggerRange : TimeSpan.Zero;
                DateTime dtStart = dtNow.Subtract(preRange);
                DateTime dtMax = dtStart.Add(m_eventData.MaxTriggerRange);
                DateTime dtStop = dtNow.Add(postRange);
                if (dtMax < dtStop)
                    dtStop = dtMax;

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
