using System;
using System.Windows.Forms;

namespace iba.Controls
{
    public partial class OpcUaCertificateEnterPasswordForm : Form
    {
        public OpcUaCertificateEnterPasswordForm()
        {
            InitializeComponent();
        }

        public string Password => tbPassword.Text;
    }
}
