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
            m_rbBinary.Checked = m_data.Binary;
            m_rbText.Checked = !m_data.Binary;
            m_timerStatus.Enabled = m_enableCheckBox.Checked = m_data.Enabled;
            m_cycleUpDown.Value = m_data.CycleTime;
            m_tbHost.Text = m_data.Address;
            m_tbPort.Text = m_data.PortNr.ToString();
            m_ApplyButton.Enabled = Program.RunsWithService != Program.ServiceEnum.DISCONNECTED;
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
            m_data.Binary = m_rbBinary.Checked;
            TaskManager.Manager.ReplaceWatchdogData(m_data.Clone() as WatchDogData);
            m_timerStatus.Enabled = false;
        }

        public void LeaveCleanup() { }

        #endregion

        private void m_timerStatus_Tick(object sender, EventArgs e)
        {
            m_timerStatus.Enabled = false;
            m_tbStatus.Text = TaskManager.Manager.GetWatchdogStatus();
            m_timerStatus.Enabled = m_enableCheckBox.Checked;
        }

        private void m_applyButton_Click(object sender, EventArgs e)
        {
            SaveData();
            m_timerStatus.Enabled = m_enableCheckBox.Checked;
        }
    }
}
