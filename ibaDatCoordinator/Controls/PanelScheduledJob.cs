using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using iba.Data;
using iba.Utility;
using iba.Processing;
using iba.Dialogs;
using System.IO;
using iba.HD.Common;

namespace iba.Controls
{
    public partial class PanelScheduledJob : UserControl, IPropertyPane
    {
        public PanelScheduledJob()
        {
            InitializeComponent();
            ((Bitmap)m_refreshDats.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_applyToRunningBtn.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_undoChangesBtn.Image).MakeTransparent(Color.Magenta);

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
            m_hdStorePicker = new iba.HD.Client.HdControlStorePicker();
            m_hdStorePicker.Dock = DockStyle.Fill;
            this.gbHD.Controls.Add(this.m_hdStorePicker);
            m_hdStorePicker.SelectedPort = 9180;
            m_hdStorePicker.SelectedServer = "localhost";
            m_hdStorePicker.SelectedStore = "";
            m_hdStorePicker.SelectedStores = new string[0];
            m_hdStorePicker.AllowedStoreType = HdStoreType.Time /*| HdStoreType.Length*/;
            m_hdStorePicker.HideConfigureButton();
            m_hdStorePicker.SetCheckedFeatures(ReaderFeature.Analyzer, new List<WriterFeature>());

            string[] itemStrs = Properties.Resources.TimeSelectionChoices.Split(';');
            long ms = 10000; //10000 * 100 nanosec = 1 ms
            long s = 1000 * ms;
            m_timeBases = new long[] { 0, 1 * ms, 10 * ms, 100 * ms, s, 60 * s, 3600 * s, 24 * 3600 * s };
            int n = itemStrs.Count();
            //Debug.Assert(n == tmpStamps.Count());
            for (int i = 0; i < n; i++)
                m_cbTimeBase.Items.Add(new TimeCbItem(itemStrs[i], m_timeBases[i]));
        }

        private RadioButton[] m_rbs;
        private long[] m_timeBases;

        private iba.HD.Client.HdControlStorePicker m_hdStorePicker;
        IPropertyPaneManager m_manager;
        ConfigurationData m_confData;
        ScheduledJobData m_scheduleData;

        private WeeklyTriggerSettingsControl m_weekSettingsCtrl;
        private DailyTriggerSettingsControl m_daySettingsCtrl;
        private MonthlyTriggerSettingsControl m_monthSettingsCtrl;

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

            //hdStore
            m_hdStorePicker.SelectedServer = m_scheduleData.HDServer;
            m_hdStorePicker.SelectedPort = m_scheduleData.HDPort;
            m_hdStorePicker.SelectedStore = m_scheduleData.HDStores[0];
            //timeSelection
            Start = m_scheduleData.StartRangeFromTrigger;
            Stop = m_scheduleData.StopRangeFromTrigger;
            if(m_scheduleData.PreferredTimeBaseIsAuto)
                m_cbTimeBase.SelectedIndex = 0;
            else
                m_cbTimeBase.SelectedIndex = Array.FindIndex(m_timeBases, ticks => ticks == m_scheduleData.PreferredTimeBaseTicks);
        }


        public void SaveData()
        {
            //options of ConfData
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
            m_scheduleData.HDServer = m_hdStorePicker.SelectedServer;
            m_scheduleData.HDPort = m_hdStorePicker.SelectedPort;
            m_scheduleData.HDStores = new string[]{m_hdStorePicker.SelectedStore};
            //time selection
            m_scheduleData.StartRangeFromTrigger = Start;
            m_scheduleData.StopRangeFromTrigger = Stop;
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
            set { 
                m_repeatInterval = value;
                if(m_repeatInterval > TimeSpan.FromDays(1))
                    m_repeatInterval = TimeSpan.FromDays(1);
                if(m_repeatInterval < TimeSpan.FromMinutes(1))
                    m_repeatInterval = TimeSpan.FromMinutes(1);
                m_nudRepeatHours.Value = m_repeatInterval.Hours;
                m_nudRepeatMinutes.Value = m_repeatInterval.Minutes;
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
                m_nudStartDays.Value = Math.Min(m_nudStartDays.Maximum, m_tsStart.Days);
                m_nudStartHours.Value = m_tsStart.Hours;
                m_nudStartMinutes.Value = m_tsStart.Minutes;
                m_nudStartSeconds.Value = m_tsStart.Seconds;
            }
            else
            {
                m_nudStopDays.Value = Math.Min(m_nudStopDays.Maximum, m_tsStop.Days);
                m_nudStopHours.Value = m_tsStop.Hours;
                m_nudStopMinutes.Value = m_tsStop.Minutes;
                m_nudStopSeconds.Value = m_tsStop.Seconds;
            }
            m_cbTimeBase.Invalidate();
            m_cbTimeBase_SelectedIndexChanged(null, null);
        }

        private void StartChanged(object sender, EventArgs e)
        {
            TimeSpan sp = new TimeSpan((int)m_nudStartDays.Value, (int)m_nudStartHours.Value, (int)m_nudStartMinutes.Value, (int)m_nudStartSeconds.Value);

            if (Control.ModifierKeys == Keys.Control)
            {
                if (sender == m_nudStartDays)
                {
                    sp = TimeSpan.FromDays((int)m_nudStartDays.Value);
                }
                else if (sender == m_nudStartHours)
                {
                    sp = TimeSpan.FromHours((int)m_nudStartHours.Value);
                }
                else if (sender == m_nudStartMinutes)
                {
                    sp = TimeSpan.FromMinutes((int)m_nudStartMinutes.Value);
                }
                else if (sender == m_nudStartSeconds)
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
        private void StopChanged(object sender, EventArgs e)
        {
            TimeSpan sp = new TimeSpan((int)m_nudStopDays.Value, (int)m_nudStopHours.Value, (int)m_nudStopMinutes.Value, (int)m_nudStopSeconds.Value);

            if (Control.ModifierKeys == Keys.Control)
            {
                if (sender == m_nudStopDays)
                {
                    sp = TimeSpan.FromDays((int)m_nudStopDays.Value);
                }
                else if (sender == m_nudStopHours)
                {
                    sp = TimeSpan.FromHours((int)m_nudStopHours.Value);
                }
                else if (sender == m_nudStopMinutes)
                {
                    sp = TimeSpan.FromMinutes((int)m_nudStopMinutes.Value);
                }
                else if (sender == m_nudStopSeconds)
                {
                    sp = TimeSpan.FromSeconds((int)m_nudStopSeconds.Value);
                }
            }
            Stop = sp;
            long diff = TimeSpan.FromSeconds(1).Ticks;
            if (m_tsStart.Ticks < sp.Ticks + diff)
            {
                Start = TimeSpan.FromTicks(sp.Ticks + diff);
            }
        }

        private void m_cbTimeBase_DrawItem(object sender, DrawItemEventArgs e)
        {
            e.DrawBackground();
            if (e.Index < 0) return;
            string tooltipText = null;
            string text = ((ComboBox)sender).Items[e.Index].ToString();
            if (e.Index == 0)
                text = String.Format(text, GetAutoItem());
            Brush brush = null;
            switch (CheckTimeBaseAcceptability(((ComboBox)sender).Items[e.Index] as TimeCbItem))
            {
                case TimeBaseAcceptability.Unknown:
                    brush = new SolidBrush(e.ForeColor);
                    break;
                case TimeBaseAcceptability.Allowed:
                    brush = Brushes.Green;
                    tooltipText = Properties.Resources.TooltipGreen;
                    break;
                case TimeBaseAcceptability.Questionable:
                    brush = Brushes.DarkOrange;
                    tooltipText = Properties.Resources.TooltipOrange;
                    break;
                case TimeBaseAcceptability.Forbidden:
                    brush = Brushes.Red;
                    tooltipText = Properties.Resources.TooltipRed;
                    break;
            }
            if ((e.State & DrawItemState.Focus) == DrawItemState.Focus)
            {
                e.Graphics.FillRectangle(new SolidBrush(Color.Lavender), e.Bounds);
            }
            e.Graphics.DrawString(text, ((Control)sender).Font, brush, e.Bounds.X, e.Bounds.Y);
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected && tooltipText != null && m_cbTimeBase.DroppedDown)
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
                        m_toolTip.SetToolTip(m_cbTimeBase, Properties.Resources.TooltipGreen);
                        break;
                    case TimeBaseAcceptability.Questionable:
                        m_toolTip.SetToolTip(m_cbTimeBase, Properties.Resources.TooltipOrange);
                        break;
                    case TimeBaseAcceptability.Forbidden:
                        m_toolTip.SetToolTip(m_cbTimeBase, Properties.Resources.TooltipRed);
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

        public double ReqTimeBaseFactor()
        {
            return Math.Sqrt(40.0); //in the future we might need to ask the reader
        }

        public TimeBaseAcceptability CheckTimeBaseAcceptability(TimeCbItem item)
        {
            long duration = Start.Ticks - Stop.Ticks;
            if (duration <= 0) return TimeBaseAcceptability.Unknown;
            if (item.m_timebaseLength == 0) return TimeBaseAcceptability.Allowed; //auto
            double samples = ((double)(duration)) / ((double)(item.m_timebaseLength)) * ReqTimeBaseFactor();
            if (samples > 1.0e9) return TimeBaseAcceptability.Forbidden;
            else if (samples > 1.0e7) return TimeBaseAcceptability.Questionable;
            else return TimeBaseAcceptability.Allowed;
        }

        private void m_btShowTrigger_Click(object sender, EventArgs e)
        {
            SaveData();
            TriggerCalculator calc = new TriggerCalculator(m_scheduleData);

            StringBuilder sb = new StringBuilder();

            DateTime from = DateTime.Now;
            for(int i = 0; i < 10; i++)
            {
                DateTime next;
                if (!calc.NextTrigger(from,out next)) break;
                sb.AppendLine(next.ToString() + " (" + next.DayOfWeek.ToString() + ")");
                from = next;
            }
            MessageBox.Show(sb.ToString());
        }

        private void m_btTriggerNow_Click(object sender, EventArgs e)
        {

        }

        private void m_nudRepeat_ValueChanged(object sender, EventArgs e)
        {
            RepeatInterval = TimeSpan.FromHours((int)(m_nudRepeatHours.Value)).Add(TimeSpan.FromMinutes(((int)(m_nudRepeatMinutes.Value))));
        }
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

    public class TimeCbItem
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
