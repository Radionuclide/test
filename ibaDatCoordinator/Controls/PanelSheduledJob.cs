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
        }

        IPropertyPaneManager m_manager;
        ConfigurationData m_data;

        private WeeklyTriggerSettingsControl m_weekSettingsCtrl;
        private DailyTriggerSettingsControl m_daySettingsCtrl;
        private MonthlyTriggerSettingsControl m_monthSettingsCtrl;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as ConfigurationData;

            if(m_failTimeUpDown.Minimum > (decimal)m_data.ReprocessErrorsTimeInterval.TotalMinutes)
                m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double)m_failTimeUpDown.Minimum);
            else if(m_failTimeUpDown.Maximum < (decimal)m_data.ReprocessErrorsTimeInterval.TotalMinutes)
                m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double)m_failTimeUpDown.Maximum);

            m_failTimeUpDown.Value = (decimal)m_data.ReprocessErrorsTimeInterval.TotalMinutes;
            m_retryUpDown.Value = (decimal)m_data.NrTryTimes;
            m_retryUpDown.Enabled = m_cbRetry.Checked = m_data.LimitTimesTried;
        }


        public void SaveData()
        {
            m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double)m_failTimeUpDown.Value);
            m_data.NrTryTimes = (int)m_retryUpDown.Value;
            m_data.LimitTimesTried = m_cbRetry.Checked;

        }

        public void LeaveCleanup() {}

        private void m_cbRetry_CheckedChanged(object sender, EventArgs e)
        {
            m_retryUpDown.Enabled = m_cbRetry.Checked;
        }

        private void m_cbRepeat_CheckedChanged(object sender, EventArgs e)
        {
            m_repeatDurationCombo.Enabled = m_repeatIntervalCombo.Enabled = m_lblDuration.Enabled = m_cbRepeat.Checked;
        }

        private void OnTriggerRBChanged(object sender, EventArgs e)
        {
            m_weekSettingsCtrl.Visible = m_rbWeekly.Checked;
            m_monthSettingsCtrl.Visible = m_rbMonthly.Checked;
            m_daySettingsCtrl.Visible = m_rbDaily.Checked;
        }
    }
}
