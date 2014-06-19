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

namespace iba.Controls
{
    public partial class PanelScheduledJob : UserControl, IPropertyPane
    {
        private TimeSpan[] m_repeatEveryOptions;
        private TimeSpan[] m_repeatDurationOptions;
        RadioButton[] m_rbs;
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
            m_dtStart.CustomFormat = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern + " HH:mm:ss";
            m_rbs = new RadioButton[] { m_rbOneTime, m_rbDaily, m_rbWeekly, m_rbMonthly };
            m_repeatEveryOptions = new TimeSpan[] {TimeSpan.FromMinutes(5),TimeSpan.FromMinutes(10),TimeSpan.FromMinutes(15),TimeSpan.FromMinutes(30),TimeSpan.FromHours(1)};
            m_repeatDurationOptions = new TimeSpan[] {TimeSpan.FromMinutes(15),TimeSpan.FromMinutes(30),TimeSpan.FromHours(1),TimeSpan.FromHours(12),TimeSpan.FromDays(1)};
        }

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
            //monthly
            //repeat options
            m_repeatEveryCombo.SelectedIndex = Array.FindIndex(m_repeatEveryOptions, ts => ts == m_scheduleData.RepeatEvery);
            m_repeatDurationCombo.SelectedIndex = Array.FindIndex(m_repeatDurationOptions, ts => ts == m_scheduleData.RepeatDuration);
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
            //monthly
            //repeat options
            if (m_repeatEveryCombo.SelectedIndex >= 0) m_scheduleData.RepeatEvery = m_repeatEveryOptions[m_repeatEveryCombo.SelectedIndex];
            if(m_repeatDurationCombo.SelectedIndex >= 0) m_scheduleData.RepeatEvery = m_repeatDurationOptions[m_repeatDurationCombo.SelectedIndex];
        }

        public void LeaveCleanup() {}

        private void m_cbRetry_CheckedChanged(object sender, EventArgs e)
        {
            m_retryUpDown.Enabled = m_cbRetry.Checked;
        }

        private void m_cbRepeat_CheckedChanged(object sender, EventArgs e)
        {
            m_repeatDurationCombo.Enabled = m_repeatEveryCombo.Enabled = m_lblDuration.Enabled = m_cbRepeat.Checked;
        }

        private void OnTriggerRBChanged(object sender, EventArgs e)
        {
            m_weekSettingsCtrl.Visible = m_rbWeekly.Checked;
            m_monthSettingsCtrl.Visible = m_rbMonthly.Checked;
            m_daySettingsCtrl.Visible = m_rbDaily.Checked;
        }
    }
}
