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


namespace iba.Controls
{
    public partial class LogControl : UserControl, IPropertyPane
    {
        public LogControl()
        {
            InitializeComponent();
        }

        public DataGridView LogView
        {
            get { return m_dataGridView; }
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private LogData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as LogData;
        }

        public void SaveData()
        {
        }

        #endregion

    }
}
