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
            m_freeze = false;
        }

        public void SaveData()
        {
        }

        public void LeaveCleanup() { }

        #endregion

        private bool m_freeze;
        public bool Freeze
        {
            get {return m_freeze;}
            set { m_freeze = value; }
        }


        private void m_dataGridView_Scroll(object sender, ScrollEventArgs e)
        {
            if (m_dataGridView.FirstDisplayedScrollingRowIndex==0)
                m_freeze = false;
        }

        private void m_dataGridView_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex != m_dataGridView.RowCount - 1)
                m_freeze = true;
        }
   }
}
