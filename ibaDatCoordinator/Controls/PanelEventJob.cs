﻿using System;
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

namespace iba.Controls
{
    //TODO embed server selection

    public partial class PanelEventJob : UserControl, IPropertyPane
    {
        #region Members
        IPropertyPaneManager m_manager;
        ConfigurationData m_confData;
        EventJobData m_eventData;

        IHdReader m_hdReader;

        List<string> m_currEvents;
        IHdSignalTree m_treeEvents;

        static ImageList imageListError;
        ListViewItem m_lviErrorStores;

        bool m_bEventServerChanged;
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
            //((Bitmap)m_refreshDats.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_applyToRunningBtn.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_undoChangesBtn.Image).MakeTransparent(Color.Magenta);

            m_currEvents = new List<string>();

            m_hdReader = HdClient.CreateReader(HdUserType.Analyzer);
            m_hdReader.ShowConnectionError = false;

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
            ChangeEventServer(m_eventData.HDServer, m_eventData.HDPort);
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
        private void btnEventServer_Click(object sender, EventArgs e)
        {
            int port = 0;
            if (!int.TryParse(m_tbEventServerPort.Text, out port))
                port = 9180;

            string newServer = string.Empty;
            int newPort = 0;
            
            using (HdFormServerPicker serverPicker = new HdFormServerPicker(m_tbEventServer.Text, port))
            {
                serverPicker.SetCheckedFeatures(new List<ReaderFeature>() { ReaderFeature.Event }, new List<WriterFeature>());
                if (serverPicker.ShowDialog() != DialogResult.OK)
                    return;

                newServer = serverPicker.SelectedServer;
                newPort = serverPicker.SelectedPort;
            }

            if (m_tbEventServer.Text == newServer && m_tbEventServerPort.Text == newPort.ToString())
                return;

            m_bEventServerChanged = true;
            m_currEvents.Clear();
            ChangeEventServer(newServer, newPort);
        }

        void ChangeEventServer(string server, int port)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, int>(ChangeEventServer), server, port);
                return;
            }

            m_tbEventServer.Text = server;
            m_tbEventServerPort.Text = port.ToString();

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

                string filename = string.Format("{0}_{1:yyyy-MM-dd_HH-mm-ss}.hdq", CPathCleaner.CleanFile(m_confData.Name), dtStart);
                using (SaveFileDialog dlg = new SaveFileDialog())
                {
                    dlg.FileName = filename;
                    dlg.AddExtension = true;
                    dlg.Title = Properties.Resources.EventJob_GenerateTestFile;
                    dlg.Filter = Properties.Resources.EventJob_GenerateTestFile_Filter;
                    dlg.DefaultExt = ".hdq";

                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;

                    filename = dlg.FileName;
                }

                m_confData.GenerateHDQFile(dtStart, dtStop, filename);
            }
            catch (Exception ex)
            {
                ibaLogger.LogFormat(Level.Exception, "Generating an HDQ test file failed: {0}", ex.Message);
            }
        }
        #endregion
    }
}
