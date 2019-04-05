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
using iba.HD.Client;
using iba;
using System.Threading.Tasks;
using iba.HD.Client.Interfaces;

namespace iba.Controls
{
    public partial class PanelScheduledJob : UserControl, IPropertyPane
    {
        #region Members
        IHdReader m_hdReader;

        string m_server;
        int m_port;
        string m_username;
        string m_password;

        List<string> m_currStores;

        static ImageList imageListError;
        ListViewItem m_lviErrorStores;
        #endregion

        static PanelScheduledJob()
        {
            //DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel = new DevExpress.LookAndFeel.DefaultLookAndFeel();
            //defaultLookAndFeel.LookAndFeel.Style = DevExpress.LookAndFeel.LookAndFeelStyle.Flat;
            //defaultLookAndFeel.LookAndFeel.UseWindowsXPTheme = true;

            imageListError = new ImageList();
            imageListError.Images.Add(Properties.Resources.img_error);
        }

        public PanelScheduledJob()
        {
            InitializeComponent();
            //((Bitmap)m_refreshDats.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_applyToRunningBtn.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_undoChangesBtn.Image).MakeTransparent(Color.Magenta);

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
            m_hdReader.UserLoginInfo.SavePassword = true;
            m_hdReader.UserLoginInfo.AllowSavePassword = false;

            m_hdReader.UserLoginOptions = HdUserLoginOptions.Never;

            m_weekSettingsCtrl = new WeeklyTriggerSettingsControl();
            m_daySettingsCtrl = new DailyTriggerSettingsControl();
            m_monthSettingsCtrl = new MonthlyTriggerSettingsControl();
            m_weekSettingsCtrl.Visible = m_daySettingsCtrl.Visible = m_monthSettingsCtrl.Visible = false;
            m_weekSettingsCtrl.Dock = m_daySettingsCtrl.Dock = m_monthSettingsCtrl.Dock = DockStyle.Fill;
            m_gbSubProperties.Controls.Add(m_weekSettingsCtrl);
            m_gbSubProperties.Controls.Add(m_daySettingsCtrl);
            m_gbSubProperties.Controls.Add(m_monthSettingsCtrl);
            m_dtStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            m_dtStart.CustomFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + "  " + System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.LongTimePattern;
            m_rbs = new RadioButton[] { m_rbOneTime, m_rbDaily, m_rbWeekly, m_rbMonthly };

            string[] itemStrs = iba.Properties.Resources.TimeSelectionChoices.Split(';');
            long ms = 10000; //10000 * 100 nanosec = 1 ms
            long s = 1000 * ms;
            m_timeBases = new long[] { 0, 1 * ms, 10 * ms, 100 * ms, s, 60 * s, 3600 * s, 24 * 3600 * s };
            int n = itemStrs.Count();
            //Debug.Assert(n == tmpStamps.Count());
            for (int i = 0; i < n; i++)
                m_cbTimeBase.Items.Add(new TimeCbItem(itemStrs[i], m_timeBases[i]));
            AddEventsToTriggerControls(gbTrigger);

            OnHdConnectionChanged();

            m_hdReader.ConnectionChanged += OnHdConnectionChanged;
        }

        private RadioButton[] m_rbs;
        private long[] m_timeBases;

        IPropertyPaneManager m_manager;
        ConfigurationData m_confData;
        ScheduledJobData m_scheduleData;

        private WeeklyTriggerSettingsControl m_weekSettingsCtrl;
        private DailyTriggerSettingsControl m_daySettingsCtrl;
        private MonthlyTriggerSettingsControl m_monthSettingsCtrl;

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

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_confData = datasource as ConfigurationData;
            m_scheduleData = m_confData.ScheduleData; //should not be null
            
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

            //base trigger time
            m_dtStart.Value = m_scheduleData.BaseTriggerTime;
            //selected trigger type
            for (int i = 0 ; i < m_rbs.Length; i++)
                m_rbs[i].Checked = (i == (int)m_scheduleData.TriggerType);
            //daily
            m_daySettingsCtrl.m_nudDays.Value = (System.Decimal)(m_scheduleData.DayTriggerEveryNDay);
            // weekly
            m_weekSettingsCtrl.m_nudWeeks.Value = (System.Decimal)(m_scheduleData.WeekTriggerEveryNWeek);
            m_weekSettingsCtrl.SetDaysFromList(m_scheduleData.WeekTriggerWeekDays);
            //monthly
            m_monthSettingsCtrl.m_rbDays.Checked = m_scheduleData.MonthTriggerUseDays;
            m_monthSettingsCtrl.m_rbOn.Checked = !m_scheduleData.MonthTriggerUseDays;
            m_monthSettingsCtrl.m_cbMonths.FromIntegerList(m_scheduleData.MonthTriggerMonths, false);
            m_monthSettingsCtrl.m_cbDays.FromIntegerList(m_scheduleData.MonthTriggerDays, false);
            m_monthSettingsCtrl.m_cbOnPart1.FromIntegerList(m_scheduleData.MonthTriggerOn, true);
            m_monthSettingsCtrl.m_cbOnPartWeekday.FromIntegerList(m_scheduleData.MonthTriggerWeekDay, true);
            //repeat options
            m_cbRepeat.Checked = m_scheduleData.Repeat;

            RepeatInterval = m_scheduleData.RepeatEvery;
            m_nudRepeatTimes.Value = m_scheduleData.RepeatTimes;
            m_nudRepeatHours.Enabled = m_nudRepeatMinutes.Enabled = m_nudRepeatTimes.Enabled = m_cbRepeat.Checked;

            //hdStore
            m_currStores = new List<string>(m_scheduleData.HDStores);
            //timeSelection
            bIgnoreTriggerChanged = true;
            m_cbUseTriggerAsStart.Checked = m_scheduleData.UsePreviousTriggerAsStart;
            Start = m_scheduleData.StartRangeFromTrigger;
            Stop = m_scheduleData.StopRangeFromTrigger;
            bIgnoreTriggerChanged = false;

            UpdateQueryRange();
            if (m_scheduleData.PreferredTimeBaseIsAuto)
                m_cbTimeBase.SelectedIndex = 0;
            else
                m_cbTimeBase.SelectedIndex = Array.FindIndex(m_timeBases, ticks => ticks == m_scheduleData.PreferredTimeBaseTicks);

            m_server = m_scheduleData.HDServer;
            m_port = m_scheduleData.HDPort;
            m_username = m_scheduleData.HDUsername;
            m_password = m_scheduleData.HDPassword;
            ChangeHDServer(m_scheduleData.HDServer, m_scheduleData.HDPort, m_scheduleData.HDUsername, m_scheduleData.HDPassword, SelectServer);
        }


        public void SaveData()
        {
            //options of ConfData
            m_confData.InitialScanEnabled = m_cbInitialScanEnabled.Checked;
            m_confData.ReprocessErrors = m_cbRepErr.Checked;
            m_confData.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double)m_failTimeUpDown.Value);
            m_confData.NrTryTimes = (int)m_retryUpDown.Value;
            m_confData.LimitTimesTried = m_cbRetry.Checked;
            //base trigger time
            m_scheduleData.BaseTriggerTime = m_dtStart.Value;
            //selected trigger type
            m_scheduleData.TriggerType = (ScheduledJobData.TriggerTypeEnum) ( m_rbs.Select((value, index) => new { value, index }).First(z => z.value.Checked).index);
            //daily
            m_scheduleData.DayTriggerEveryNDay = (int) m_daySettingsCtrl.m_nudDays.Value;
            // weekly
            m_scheduleData.WeekTriggerEveryNWeek = (int)m_weekSettingsCtrl.m_nudWeeks.Value;
            m_scheduleData.WeekTriggerWeekDays = m_weekSettingsCtrl.GetListFromDays();
            //monthly
            m_scheduleData.MonthTriggerUseDays = m_monthSettingsCtrl.m_rbDays.Checked;
            m_scheduleData.MonthTriggerMonths = new List<int>(m_monthSettingsCtrl.m_cbMonths.ToIntegerList(false));
            m_scheduleData.MonthTriggerDays = new List<int>(m_monthSettingsCtrl.m_cbDays.ToIntegerList(false));
            m_scheduleData.MonthTriggerOn = new List<int>(m_monthSettingsCtrl.m_cbOnPart1.ToIntegerList(true));
            m_scheduleData.MonthTriggerWeekDay = new List<int>(m_monthSettingsCtrl.m_cbOnPartWeekday.ToIntegerList(true));
            //repeat options
            m_scheduleData.Repeat = m_cbRepeat.Checked;
            m_scheduleData.RepeatTimes = (int)m_nudRepeatTimes.Value;
            m_scheduleData.RepeatEvery = RepeatInterval;
            //hdStore
            m_scheduleData.HDServer = m_server ?? string.Empty;
            m_scheduleData.HDPort = m_port;
            m_scheduleData.HDUsername = m_username ?? string.Empty;
            m_scheduleData.HDPassword = m_password ?? string.Empty;
            m_scheduleData.HDStores = m_currStores.ToArray();
            //time selection
            m_scheduleData.StartRangeFromTrigger = Start;
            m_scheduleData.StopRangeFromTrigger = Stop;
            m_scheduleData.UsePreviousTriggerAsStart = m_cbUseTriggerAsStart.Checked;
            if(m_cbTimeBase.SelectedIndex == 0) //auto
            {
                m_scheduleData.PreferredTimeBaseTicks = GetAutoItem().m_timebaseLength;
                m_scheduleData.PreferredTimeBaseIsAuto = true;
            }
            else if(m_cbTimeBase.SelectedIndex > 0)
            {
                m_scheduleData.PreferredTimeBaseTicks = m_timeBases[m_cbTimeBase.SelectedIndex];
                m_scheduleData.PreferredTimeBaseIsAuto = false;
            }
        }

        public void LeaveCleanup() {}

        private void m_cbRetry_CheckedChanged(object sender, EventArgs e)
        {
            m_retryUpDown.Enabled = m_cbRetry.Checked;
        }

        private void m_cbRepeat_CheckedChanged(object sender, EventArgs e)
        {
            m_nudRepeatTimes.Enabled = m_nudRepeatHours.Enabled = m_nudRepeatMinutes.Enabled = m_cbRepeat.Checked;
        }

        private void OnTriggerRBChanged(object sender, EventArgs e)
        {
            m_weekSettingsCtrl.Visible = m_rbWeekly.Checked;
            m_monthSettingsCtrl.Visible = m_rbMonthly.Checked;
            m_daySettingsCtrl.Visible = m_rbDaily.Checked;
        }

        private TimeSpan m_repeatInterval;
        public System.TimeSpan RepeatInterval
        {
            get { return m_repeatInterval; }
            set 
            { 
                m_repeatInterval = value;
                if(m_repeatInterval > TimeSpan.FromDays(1))
                    m_repeatInterval = TimeSpan.FromDays(1);
                if(m_repeatInterval < TimeSpan.FromMinutes(1))
                    m_repeatInterval = TimeSpan.FromMinutes(1);
                DisableRepeatChanged = true;
                m_nudRepeatHours.Value = m_repeatInterval.Hours;
                m_nudRepeatMinutes.Value = m_repeatInterval.Minutes;
                DisableRepeatChanged = false;
            }
        }

        private TimeSpan m_tsStart;
        public TimeSpan Start
        {
            get { return m_tsStart; }
            set 
            { 
                m_tsStart = value;
                UpdateTimeControls(true);
            }
        }

        private TimeSpan m_tsStop;
        public TimeSpan Stop
        {
            get { return m_tsStop; }
            set
            {
                m_tsStop = value;
                UpdateTimeControls(false);
            }
        }

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

        bool DisableStartChangedEvent;
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
                if(!m_cbUseTriggerAsStart.Checked)
                {
                    long diff = TimeSpan.FromSeconds(1).Ticks;
                    if(sp.Ticks >= m_tsStop.Ticks + diff)
                    {
                        Start = sp;
                    }
                    else if(sp.Ticks >= diff)
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
                else
                    Start = sp;
            }
            finally
            {
                DisableStartChangedEvent = false;
            }
        }

        
        bool DisableStopChangedEvent;

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
                if(!m_cbUseTriggerAsStart.Checked)
                {
                    long diff = TimeSpan.FromSeconds(1).Ticks;
                    if(m_tsStart.Ticks < sp.Ticks + diff)
                    {
                        Start = TimeSpan.FromTicks(sp.Ticks + diff);
                    }
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
            long duration = 0;
            if(m_cbUseTriggerAsStart.Checked)
                duration = m_queryRangeUseTriggerAsStart.Ticks;
            else
                duration = Start.Ticks - Stop.Ticks;
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

        private void m_btShowTrigger_Click(object sender, EventArgs e)
        {
            SaveData();
            TriggerCalculator calc = new TriggerCalculator(m_scheduleData);

            StringBuilder sb = new StringBuilder();

            DateTime from = DateTime.Now;
            int count = 10;
            for(int i = 0; i < 10; i++)
            {
                DateTime next;
                if(!calc.NextTrigger(from, out next))
                {
                    count = i;
                    break;
                }

                string rangeText;
                if(m_scheduleData.UsePreviousTriggerAsStart)
                {
                    DateTime startTime;
                    DateTime stopTime = next - m_scheduleData.StopRangeFromTrigger;
                    rangeText = "";
                    if(!calc.PrevTrigger(next, out startTime))
                    {
                        rangeText = iba.Properties.Resources.BadRangeStart;
                        return;
                    }
                    else
                    {
                        startTime -= m_scheduleData.StartRangeFromTrigger;
                        if(startTime >= stopTime)
                        {
                            rangeText = String.Format(iba.Properties.Resources.BadRangeTextFormatter, startTime, stopTime);
                        }
                        else
                            rangeText = String.Format(iba.Properties.Resources.RangeTextFormatter, startTime, stopTime);
                    }
                }
                else
                {
                    DateTime startTime = next - m_scheduleData.StartRangeFromTrigger;
                    DateTime stopTime = next - m_scheduleData.StopRangeFromTrigger;
                    rangeText = String.Format(iba.Properties.Resources.RangeTextFormatter, startTime, stopTime);
                }
                sb.AppendLine(next.ToString() + " (" + next.DayOfWeek.ToString() + ") " + rangeText);
                from = next;
            }
            string caption = m_confData.Name;
            if (count == 0)
            {
                MessageBox.Show(iba.Properties.Resources.NoTriggers,caption);
            }
            else
            {
                string message = string.Format(iba.Properties.Resources.NextNTriggers, count) + Environment.NewLine + sb.ToString();
                message = message.TrimEnd();
                if(count < 10) message += Environment.NewLine + iba.Properties.Resources.NoTriggersAfter;
                FlexibleMessageBox.Show(message,caption);
            }
        }

        private void m_btTriggerNow_Click(object sender, EventArgs e)
        {
            SaveData();
            TaskManager.Manager.ForceTrigger(m_confData);
        }

        bool DisableRepeatChanged;
        private void m_nudRepeat_ValueChanged(object sender, EventArgs e)
        {
            if(DisableRepeatChanged)
                return;

            DisableRepeatChanged = true;
            RepeatInterval = TimeSpan.FromHours((int)(m_nudRepeatHours.Value)).Add(TimeSpan.FromMinutes(((int)(m_nudRepeatMinutes.Value))));
            DisableRepeatChanged = false;

            if(m_cbUseTriggerAsStart.Checked)
            {
                m_scheduleData.RepeatEvery = RepeatInterval;
                UpdateQueryRange();
                m_cbTimeBase.Invalidate();
                m_cbTimeBase_SelectedIndexChanged(null, null);
            }
        }

        bool bIgnoreTriggerChanged;
        private void m_cbUseTriggerAsStart_CheckedChanged(object sender, EventArgs e)
        {
            if (bIgnoreTriggerChanged)
                return;

            //Control[] toHide = new Control[] {label5,label6,label7,label8,label9, m_nudStartDays, m_nudStartHours, m_nudRepeatMinutes, m_nudStartSeconds };
            //foreach(Control ctrl in toHide)
            //{
            //    ctrl.Enabled = !ck;
            //}
            bool ck = m_cbUseTriggerAsStart.Checked;
            if(ck)
                OnTriggerControlValidated(sender, e);
            OnStartChanged(sender, e);
        }

        private TimeSpan m_queryRangeUseTriggerAsStart;

        private void OnTriggerControlValidated(object sender, EventArgs e)
        {
            if (!m_cbUseTriggerAsStart.Checked)
                return; //not interested in case of fixed query range

            ScheduledJobData oldData = m_scheduleData.Clone() as ScheduledJobData;
            SaveData();
            if (oldData.IsSame(m_scheduleData))
                return; //nothing relevant changed

            UpdateQueryRange();
            m_cbTimeBase.Invalidate();
        }

        private void UpdateQueryRange()
        {
            if (!m_cbUseTriggerAsStart.Checked)
                return;

            TriggerCalculator c = new TriggerCalculator(m_scheduleData);
            m_queryRangeUseTriggerAsStart = c.MaxQueryRange();
            long correct = Start.Ticks - Stop.Ticks;
            if (m_queryRangeUseTriggerAsStart.Ticks <= 0 || m_queryRangeUseTriggerAsStart.Ticks + correct <= 0)
                m_queryRangeUseTriggerAsStart = TimeSpan.Zero;
            else
                m_queryRangeUseTriggerAsStart += TimeSpan.FromTicks(correct);
        }

        private void AddEventsToTriggerControls(Control ctrl)
        {
            ctrl.Validated += new System.EventHandler(this.OnTriggerControlValidated);
            foreach(Control child in ctrl.Controls)
            {
                AddEventsToTriggerControls(child);
            }
        }

        #region HD server
        private void btnHdServer_Click(object sender, EventArgs e)
        {
            SelectServer();
        }

        private void btnChangeUser_Click(object sender, EventArgs e)
        {
            ChangeHDServer(m_server, m_port, m_username, m_password,
                () => { ChangeHDServer(m_server, m_port, m_username, m_password, null, HdUserLoginOptions.Never); },
                HdUserLoginOptions.Always);
        }

        void SelectServer()
        {
            if (Disposing || IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(SelectServer));
                return;
            }

            string newServer = string.Empty;
            int newPort = 0;

            using (HdFormServerPicker serverPicker = new HdFormServerPicker(m_server, m_port))
            {
                serverPicker.SetCheckedFeatures(ReaderFeature.Analyzer, new List<WriterFeature>());
                if (serverPicker.ShowDialog() != DialogResult.OK)
                {
                    ChangeHDServer(m_server, m_port, m_username, m_password, null, HdUserLoginOptions.Never);
                    return;
                }

                newServer = serverPicker.SelectedServer;
                newPort = serverPicker.SelectedPort;
            }

            ChangeHDServer(newServer, newPort, m_username, m_password, SelectServer, HdUserLoginOptions.Always);
        }

        void ChangeHDServer(string server, int port, string username, string password, Action abortAction = null, HdUserLoginOptions showLoginForm = HdUserLoginOptions.Failure)
        {
            if (Disposing || IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<string, int, string, string, Action, HdUserLoginOptions>(ChangeHDServer), server, port, username, password, abortAction, showLoginForm);
                return;
            }

            username = username ?? "";
            password = password ?? "";

            m_lvStores.Items.Clear();

            Task.Factory.StartNew((stateObj) =>
            {
                Tuple<string, int> tpl = stateObj as Tuple<string, int>;

                if (m_hdReader != null)
                    m_hdReader.ConnectionChanged -= OnHdConnectionChanged;

                if (m_hdReader != null && m_hdReader.IsConnected())
                    m_hdReader.Disconnect();

                m_hdReader.UserLoginInfo.Parent = this;
                m_hdReader.UserLoginInfo.UserName = username;
                m_hdReader.UserLoginInfo.Password = password;
                m_hdReader.UserLoginOptions = showLoginForm;

                HdConnectResult res = m_hdReader?.ConnectEx(tpl.Item1, tpl.Item2) ?? HdConnectResult.ConnectionFailure;
                m_hdReader.UserLoginOptions = HdUserLoginOptions.Never;

                if (res == HdConnectResult.AbortedByUser && abortAction != null)
                {
                    abortAction();
                    return;
                }

                if (server != m_server || port != m_port || username != m_hdReader.UserLoginInfo.UserName || password != m_hdReader.UserLoginInfo.Password)
                {
                    //also clear on new user credentials because the new user might not have read access for certain stores
                    m_currStores.Clear();
                }

                m_server = server;
                m_port = port;
                m_username = m_hdReader.UserLoginInfo.UserName;
                m_password = m_hdReader.UserLoginInfo.Password;

                UpdateHDServerSettings();

                OnHdConnectionChanged();

                if (m_hdReader != null)
                    m_hdReader.ConnectionChanged += OnHdConnectionChanged;
            }, Tuple.Create(server, port));
        }

        void UpdateHDServerSettings()
        {
            if (Disposing || IsDisposed)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(UpdateHDServerSettings));
                return;
            }

            m_tbEventServer.Text = m_server;
            m_tbEventServerPort.Text = m_port.ToString();
            m_tbUsername.Text = m_username;
            btnChangeUser.Enabled = !string.IsNullOrWhiteSpace(m_server);
        }

        void OnHdConnectionChanged()
        {
            if (IsDisposed || Disposing)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action(OnHdConnectionChanged));
                return;
            }

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

        private void m_lvStores_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            if (m_hdReader == null || !m_hdReader.IsConnected())
                return;

            m_currStores = new List<string>();
            foreach (ListViewItem item in m_lvStores.CheckedItems)
                m_currStores.Add(item.Text);
        }
        #endregion
    }

    public class CustomNumericUpDown : NumericUpDown
    {
        protected override void UpdateEditText()
        {
            if(this.Value == 0)
                this.Text = iba.Properties.Resources.Indefinite;
            else
                this.Text = this.Value.ToString();
        }
    }

    public enum TimeBaseAcceptability
    {
        Allowed,
        Forbidden,
        Questionable,
        Unknown
    }

    internal class TimeCbItem
    {
        private string m_title;
        public long m_timebaseLength;
        public override string ToString()
        {
            return m_title;
        }
        public TimeCbItem(string title, long timelength)
        {
            m_title = title;
            m_timebaseLength = timelength;
        }
    }
}
