using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using iba.Plugins;

namespace Alunorf_roh_plugin
{
    public partial class PluginRohControl : UserControl, IPluginControl
    {
        private IDatCoHost m_datcoHost;
        public PluginRohControl(IDatCoHost host)
        {
            m_datcoHost = host;
            InitializeComponent();
        }

        #region IPluginControl Members

        public void LoadData(object datasource, ICommonTaskControl parentcontrol)
        {
            throw new NotImplementedException();
        }

        public void SaveData()
        {
            throw new NotImplementedException();
        }

        public void LeaveCleanup()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
