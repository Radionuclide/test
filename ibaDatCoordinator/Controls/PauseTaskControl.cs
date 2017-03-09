using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using iba.Data;
using iba.Processing;

namespace iba.Controls
{
    public partial class PauseTaskControl : UserControl, IPropertyPane
    {
        public PauseTaskControl()
        {
            InitializeComponent();
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private PauseTaskData m_data;
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as PauseTaskData;
            m_nudInterval.Value = (decimal) m_data.Interval.TotalSeconds;
            m_rbAbsolutePause.Checked = !m_data.MeasureFromFileTime;
            m_rbDatFileTime.Checked = m_data.MeasureFromFileTime;
            if (m_data.ParentConfigurationData.JobType == ConfigurationData.JobTypeEnum.Scheduled)
                m_rbDatFileTime.Text = iba.Properties.Resources.FromTriggerTextForLabel;
            else
                m_rbDatFileTime.Text = iba.Properties.Resources.FromDatFileTextForLabel;
        }

        public void LeaveCleanup()
        {
            return;
        }

        public void SaveData()
        {
            m_data.MeasureFromFileTime = m_rbDatFileTime.Checked;
            m_data.Interval = TimeSpan.FromSeconds((double)m_nudInterval.Value);
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        #endregion
    }
}
