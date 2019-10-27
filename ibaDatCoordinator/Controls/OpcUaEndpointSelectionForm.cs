using System.Windows.Forms;

namespace iba.Controls
{
    // todo. kls. rename
    public partial class OpcUaEndpointSelectionForm : Form
    {
        public OpcUaEndpointSelectionForm(object netwConf)
        {
            InitializeComponent();

			//ipAddressSelect1.AllowAll = true;
			//ipAddressSelect1.NetworkConfig = netwConf;
		}

		public string IpAddress
        {
            get => ""; // ipAddressSelect1.IpAddress;
            set
            {
                //ipAddressSelect1.IpAddress = value;
            }
        }

        private void OnOK(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void OnCancel(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
