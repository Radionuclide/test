using System;
using System.Windows.Forms;
using iba.Controls;

namespace iba.Dialogs
{
    /// <summary> Copied from ibaPDA.
    /// Is used in <see cref="OpcUaControl"/> and <see cref="SnmpControl"/>
    /// </summary>
    partial class PasswordEditForm : Form
    {
        public PasswordEditForm()
        {
            InitializeComponent();
        }

        public string Password => tbNew.Text.Trim();

        private void btOK_Click(object sender, EventArgs e)
        {
            if (ValidatePasswordEvent != null)
            {
                bool bOk = ValidatePasswordEvent.Invoke(this, Password, out string errorMessage);
                if(!bOk)
                {
                    MessageBox.Show(this, errorMessage, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            if (tbNew.Text != tbVerify.Text)
            {
                // todo. kls. localize
                // Properties.Resources.PasswordVerificationError
                MessageBox.Show(this, "The passwords don't match. Please correct them.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
            DialogResult = DialogResult.OK;
        }

        private void ckShowPass_CheckedChanged(object sender, EventArgs e)
        {
            tbNew.UseSystemPasswordChar = !ckShowPass.Checked;
            tbVerify.UseSystemPasswordChar = !ckShowPass.Checked;
        }

        public event ValidatePasswordDelegate ValidatePasswordEvent;
    }
}
