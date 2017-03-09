using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using iba;
using iba.Data;
using iba.Utility;
using iba.Processing;

namespace iba.Controls
{
    public partial class CopyControl : UserControl, IPropertyPane
    {
        public CopyControl()
        {
            InitializeComponent();
            m_uncControl = new UNCTaskControl();
            panelOut.Controls.Add(m_uncControl);
            m_uncControl.Dock = DockStyle.Fill;
            m_uncControl.HideModifyDateOption();
        }

        private UNCTaskControl m_uncControl;

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        CopyMoveTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as CopyMoveTaskData;
            m_uncControl.SetData(m_data);

            if (m_data.ActionDelete)
            {
                m_rbDelete.Checked = true;
                m_rbCopy.Checked = false;
                m_rbMove.Checked = false;
                m_gbTarget.Enabled = false;
            }
            else
            {
                m_gbTarget.Enabled = true;
                m_rbDelete.Checked = false;
                m_rbCopy.Checked = !m_data.RemoveSource;
                m_rbMove.Checked = m_data.RemoveSource;
            }
            m_rbDatFile.Checked = m_data.WhatFile == CopyMoveTaskData.WhatFileEnumA.DATFILE;
            m_rbPrevOutput.Checked = m_data.WhatFile == CopyMoveTaskData.WhatFileEnumA.PREVOUTPUT;
        }

        public void SaveData()
        {
            m_data.RemoveSource = m_rbMove.Checked || m_rbDelete.Checked;
            m_data.ActionDelete = m_rbDelete.Checked;
            
            if (m_rbPrevOutput.Checked)
                m_data.WhatFile = CopyMoveTaskData.WhatFileEnumA.PREVOUTPUT;
            else if (m_rbDatFile.Checked)
                m_data.WhatFile = CopyMoveTaskData.WhatFileEnumA.DATFILE;
                        
            m_uncControl.SaveData();
            m_data.UpdateUNC();

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        public void LeaveCleanup() { return; }
        
        #endregion
        
        private void m_rbDelete_CheckedChanged(object sender, EventArgs e)
        {
            m_gbTarget.Enabled = !m_rbDelete.Checked;
        }
    }
}

