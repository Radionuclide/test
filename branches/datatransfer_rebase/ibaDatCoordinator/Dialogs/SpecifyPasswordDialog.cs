using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace iba.Dialogs
{
    public partial class SpecifyPasswordDialog : Form
    {
        public SpecifyPasswordDialog()
        {
            InitializeComponent();
            m_cancelled = false;
        }

        string m_pass;
        public string Pass
        {
            get { return m_pass; }
            set { 
                m_pass = value;
                m_tbPass.Text = m_tbPassConfirm.Text = m_pass = value;
            }
        }

        bool m_cancelled;
        public bool Cancelled
        {
            get { return m_cancelled; }
        }

        private void m_btOK_Click(object sender, EventArgs e)
        {
            if (m_tbPass.Text == m_tbPassConfirm.Text)
            {
                m_pass = m_tbPass.Text;
                Close();
            }
            else
            {
                MessageBox.Show(this, iba.Properties.Resources.ConfirmAndPassDiffer, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_btCancel_Click(object sender, EventArgs e)
        {
            m_cancelled = true;
            m_pass = "";
            Close();
        }
    }
}