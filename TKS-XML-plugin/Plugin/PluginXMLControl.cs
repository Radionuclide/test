using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using iba.Plugins;
using XmlExtract;

namespace iba.TKS_XML_Plugin
{
    public partial class PluginXMLControl : UserControl, IPluginControlUNC
    {
        public PluginXMLControl()
        {
            InitializeComponent();
        }

        #region IPluginControlUNC Members

        public bool FixedHeight
        {
            get { return true; }
        }

        #endregion

        #region IPluginControl Members

        PluginXMLTask m_data;
        public void LoadData(object datasource, ICommonTaskControl parentcontrol)
        {
            m_data = datasource as PluginXMLTask;
            
            m_rbDO.Checked = m_data.StandOrt == StandortType.DO;
            m_rbDU.Checked = m_data.StandOrt == StandortType.DU;
            m_rbBO.Checked = m_data.StandOrt == StandortType.BO;
            m_rbAnderer.Checked = m_data.StandOrt == StandortType.Anderer;
            
            m_rbName.Checked = m_data.IdField == IdFieldLocation.Name;
            m_rbComment1.Checked = m_data.IdField == IdFieldLocation.PDA_Comment1;
            m_rbComment2.Checked = m_data.IdField == IdFieldLocation.PDA_Comment2;

            m_txtAndererStandort.Text = m_data.AndererStandort.Trim();
            m_txtSchemaLocation.Text = m_data.XmlSchemaLocation.Trim();
            m_txtXsdLocation.Text = m_data.XsdLocation.Trim();

        }

        public void SaveData()
        {
            if (m_rbBO.Checked)
                m_data.StandOrt = StandortType.BO;
            else if (m_rbDO.Checked)
                m_data.StandOrt = StandortType.DO;
            else if (m_rbAnderer.Checked)
                m_data.StandOrt = StandortType.Anderer;
            else /* if (m_rbDU.Checked) */
                m_data.StandOrt = StandortType.DU;

            if (m_rbComment1.Checked)
                m_data.IdField = IdFieldLocation.PDA_Comment1;
            else if (m_rbComment2.Checked)
                m_data.IdField = IdFieldLocation.PDA_Comment2;
            else /* if (m_rbName.Checked) */
                m_data.IdField = IdFieldLocation.Name;

            m_data.AndererStandort = m_txtAndererStandort.Text.Trim();
            m_data.XmlSchemaLocation = m_txtSchemaLocation.Text.Trim();
            m_data.XsdLocation = m_txtXsdLocation.Text.Trim();
        }

        public void LeaveCleanup()
        {
            //nothing to clean up
        }

        #endregion

    }
}
