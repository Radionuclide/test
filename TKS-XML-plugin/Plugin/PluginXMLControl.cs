using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using iba.Plugins;

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
            switch (m_data.StandOrt)
            {
                case XmlExtract.StandortType.BO:
                    m_rbBO.Checked = true;
                    m_rbDO.Checked = false;
                    m_rbDU.Checked = false;
                    break;
                case XmlExtract.StandortType.DO:
                    m_rbBO.Checked = false;
                    m_rbDO.Checked = true;
                    m_rbDU.Checked = false;
                    break;
                case XmlExtract.StandortType.DU:
                    m_rbBO.Checked = false;
                    m_rbDO.Checked = false;
                    m_rbDU.Checked = true;
                    break;
            }
        }

        public void SaveData()
        {
            if (m_rbBO.Checked) m_data.StandOrt = XmlExtract.StandortType.BO;
            else if (m_rbDO.Checked) m_data.StandOrt = XmlExtract.StandortType.DO;
            else /*if (m_rbDU.Checked)*/ m_data.StandOrt = XmlExtract.StandortType.DU;
        }

        public void LeaveCleanup()
        {
            //nothing to clean up
        }

        #endregion
    }
}
