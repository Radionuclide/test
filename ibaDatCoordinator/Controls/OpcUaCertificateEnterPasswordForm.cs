using System.Windows.Forms;

namespace iba.Controls
{
    /// <summary> (copied from ibaPda project) </summary>
    public partial class OpcUaCertificateEnterPasswordForm : Form
    {
        public OpcUaCertificateEnterPasswordForm()
        {
            InitializeComponent();
        }

        public string Password => tbPassword.Text;
    }
}
