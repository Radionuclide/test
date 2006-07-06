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
    public partial class WatchdogControl : UserControl, IPropertyPane
    {
        public WatchdogControl()
        {
            InitializeComponent();
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private WatchDogData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as WatchDogData;
            m_rbActiveNode.Checked = m_data.ActiveNode;
            m_rbPassiveNode.Checked = !m_data.ActiveNode;
            m_enableCheckBox.Checked = m_data.Enabled;
            m_cycleUpDown.Value = m_data.CycleTime;
            m_tbHost.Text = m_data.Address;
            m_tbPort.Text = m_data.PortNr.ToString();
        }

        public void SaveData()
        {
            m_data.Address = m_tbHost.Text;
            try
            {
                m_data.PortNr = int.Parse(m_tbPort.Text);
            }
            catch (Exception) { }
            m_data.CycleTime = (int) m_cycleUpDown.Value;
            m_data.ActiveNode = m_rbActiveNode.Checked;
            m_data.Enabled = m_enableCheckBox.Checked;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceWatchdogData(m_data);
        }
        #endregion
    }
}
