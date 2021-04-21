using System.Windows.Forms;
using iba.Data;

namespace iba.Controls
{
    // imported from PDA with some changes
    public partial class OpcUaEndpointSelectionForm : Form
    {
        public OpcUaEndpointSelectionForm(OpcUaData.NetworkConfiguration netwConf)
        {
            InitializeComponent();

            ipAddressSelect1.NetworkConfig = netwConf;
		}

		public string IpAddress
		{
			get => ipAddressSelect1.IpAddress;
            set => ipAddressSelect1.IpAddress = value;
        }
    }
}
