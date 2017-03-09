using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace iba.Dialogs
{
    public partial class PasswordConfirm : Form
    {
        public PasswordConfirm(string pass)
        {
            InitializeComponent();
            m_cancelled = false;
            m_pass = pass;
        }

        private void m_btOK_Click(object sender, EventArgs e)
        {
            if (m_pass == m_tbPass.Text) Close();
            else
            {
                MessageBox.Show(iba.Properties.Resources.incorrectPassword, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_btCancel_Click(object sender, EventArgs e)
        {
            m_cancelled = true;
            Close();
        }

        bool m_cancelled;
        public bool Cancelled
        {
            get { return m_cancelled; }
            set { m_cancelled = value; }
        }

        string m_pass;

    }
}
