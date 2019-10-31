using System;
using System.Windows.Forms;
using iba.Data;

namespace iba.Controls
{
    // imported from PDA with some changes
    public partial class OpcUaIpAddressSelect : UserControl
    {
        public OpcUaIpAddressSelect()
        {
            InitializeComponent();
        }

        OpcUaData.NetworkConfiguration _networkConfig;
        public OpcUaData.NetworkConfiguration NetworkConfig
        {
            get => _networkConfig;
            set
            {
                if (_networkConfig == value)
                    return;

                _networkConfig = value;

                RefreshItems();
            }
        }

        private void RefreshItems()
        {
            cbItfs.Items.Clear();

            if (_networkConfig == null)
                return;

            cbItfs.Items.Add(new InterfaceItem());

            foreach (OpcUaData.NetworkConfiguration.NetworkAdapter ad in _networkConfig.Adapters)
            {
                cbItfs.Items.Add(new InterfaceItem(ad));
            }
        }

        public string IpAddress
        {
            get => cbIps.SelectedItem as string ?? "";
            set
            {
                if (_networkConfig == null)
                    return;

                string newIp = value;

                // Try to set comboBoxes accordingly
                if (string.Compare(newIp, _networkConfig.Hostname, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    foreach (InterfaceItem itfIt in cbItfs.Items)
                    {
                        if (itfIt.All)
                        {
                            cbItfs.SelectedItem = itfIt;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (InterfaceItem itfIt in cbItfs.Items)
                    {
                        if (itfIt.All)
                            continue;

                        // Check if this adapter contains our IP address and 
                        foreach (string addr in itfIt.Adapter.Addresses)
                        {
                            if (string.Compare(addr ?? "", newIp, StringComparison.OrdinalIgnoreCase) == 0)
                            {
                                // select adapter
                                cbItfs.SelectedItem = itfIt;

                                // Now select IP address
                                cbIps.SelectedItem = addr;
                                break;
                            }
                        }
                    }
                }
            }
        }


        private void OnSelectedInterfaceChanged(object sender, EventArgs e)
        {
            InterfaceItem itfIt = cbItfs.SelectedItem as InterfaceItem;

            cbIps.Enabled = (itfIt != null);

            if (itfIt == null)
                return;

            cbIps.Items.Clear();

            if (itfIt.All)
            {
                cbIps.Items.Add(_networkConfig.Hostname);
                cbIps.SelectedIndex = 0;
            }
            else
            {
                OpcUaData.NetworkConfiguration.NetworkAdapter selAd = itfIt.Adapter;
                if (selAd == null)
                    return;

                foreach (string addr in selAd.Addresses)
                {
                    if (addr == null)
                        continue;

                    cbIps.Items.Add(addr);
                }

                if (cbIps.Items.Count > 0)
                    cbIps.SelectedIndex = 0;
            }
        }

        private class InterfaceItem
        {
            public InterfaceItem(OpcUaData.NetworkConfiguration.NetworkAdapter adapter)
            {
                All = false;
                Adapter = adapter;
            }

            public InterfaceItem()
            {
                All = true;
                Adapter = null;
            }

            public readonly bool All;
            public readonly OpcUaData.NetworkConfiguration.NetworkAdapter Adapter;

            public override string ToString()
            {
                //todo.kls.localize
                //return All ? iba.Utility.Localizer.GetString("All") : Adapter.Name;
                return All ? ("All") : Adapter.Name;
            }
        }
    }
}
