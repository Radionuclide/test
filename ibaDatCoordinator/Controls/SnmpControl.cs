using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;
using iba.Data;
using iba.Processing;
using IbaSnmpLib;

namespace iba.Controls
{
    public partial class SnmpControl : UserControl, IPropertyPane
    {
        public SnmpControl()
        {
            InitializeComponent();
            _tmp___instCounter++;
        }

        private static int _tmp___instCounter = 0;
        private int _tmp___cnt1;
        private int _tmp___cnt2;
        private int _tmp___cnt3;
        private int _tmp___cntTimer;

        #region IPropertyPane Members

        private SnmpData _data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            // todo
            label1.Text = $"Data loaded {_tmp___cnt1++}";

            timerStatus.Enabled = true;

            _data = datasource as SnmpData;
            //m_rbActiveNode.Checked = m_data.ActiveNode;
            //m_rbPassiveNode.Checked = !m_data.ActiveNode;
            //m_rbBinary.Checked = m_data.Binary;
            //m_rbText.Checked = !m_data.Binary;
            //m_tbStatus.Text = "";
            //m_timerStatus.Enabled = m_enableCheckBox.Checked = m_data.Enabled;
            //m_cycleUpDown.Value = m_data.CycleTime;
            //m_tbHost.Text = m_data.Address;
            //m_tbPort.Text = m_data.PortNr.ToString();
            //m_ApplyButton.Enabled = Program.RunsWithService != Program.ServiceEnum.DISCONNECTED;
        }

        public void SaveData()
        {
            // todo
            label3.Text = $"Data Saved {_tmp___cnt3++}";

            //_data.Address = m_tbHost.Text;
            //try
            //{
            //    _data.PortNr = int.Parse(m_tbPort.Text);
            //}
            //catch (Exception) { }
            //_data.CycleTime = (int)m_cycleUpDown.Value;
            //_data.ActiveNode = m_rbActiveNode.Checked;
            //_data.Enabled = m_enableCheckBox.Checked;
            //_data.Binary = m_rbBinary.Checked;

            //todo
            //TaskManager.Manager.ReplaceSnmpData(_data.Clone() as SnmpData);
            timerStatus.Enabled = false;
        }

        public void LeaveCleanup()
        {
            // todo
            label2.Text = $"Data cleaned {_tmp___cnt2++}";
        }

        #endregion


        private void timerStatus_Tick(object sender, EventArgs e)
        {
            _tmp___cntTimer++;

            label4.Text = $@"Instance {_tmp___instCounter} " + (_tmp___cntTimer%2 == 0 ? "|" : "-");


            IbaSnmp ibaSnmp = null;
            //ibaSnmp = _data.IbaSnmp;
            if (ibaSnmp == null)
            {
                tbDebug.Text = "ibaSnmp == null";
                return;
            }

            //tbDebug.Text = GetLibraryDescriptionString(ibaSnmp);
        }

        /// <summary>
        /// Get list of endpoints and configured traps. This function is not effective and can be use in debug-time only
        /// </summary>
        /// <returns></returns>
        public string GetLibraryDescriptionString(IbaSnmp _lib)
        {
            var str = "";

            // general
            str += $"Is started: {_lib.IsStarted}; ";
            str += $"Want to start: {_lib.WantToStart}; ";
            str += $"Product: {_lib.IbaProductId}\r\n";

//            if (cbShowEndpoints.Checked)
            {
                str += $"\r\nEndpoints: {_lib.ActiveEndPoints.Count}\r\n";
                foreach (var endPoint in _lib.ActiveEndPoints)
                {
                    var af = "???";
                    if (endPoint.AddressFamily == AddressFamily.InterNetwork)
                    {
                        af = "IPv4";
                    }
                    if (endPoint.AddressFamily == AddressFamily.InterNetworkV6)
                    {
                        af = "IPv6";
                    }

                    str += $"{af,-5} {endPoint.Address,-28} :{endPoint.Port}\r\n";
                }
            }

            //if (cbShowTrapInform.Checked)
            //{
            //    str += $"\r\nTrap/Inform: {_lib.NotificationList.Count}\r\n";
            //    for (var i = 0; i < _lib.NotificationList.Count; i++)
            //    {
            //        var ntfBase = _lib.NotificationList[i];

            //        str += $"Trap[{i}] ep: {ntfBase.DestinationEndpoint}\r\n";
            //        str += $"Trap[{i}] v: {ntfBase.ProtocolVersion}\r\n";
            //        str += $"Trap[{i}] community: {ntfBase.CommunityString}\r\n";
            //        str +=
            //            $"Trap[{i}] user: {ntfBase.User.Username} {ntfBase.User.Password} {ntfBase.User.EncryptionKey} {ntfBase.User.AuthAlgorithm} {ntfBase.User.EncrAlgorithm}\r\n";
            //    }
            //}

            //if (cbShowVariables.Checked)
            {
                var oids = _lib.GetListOfAllOids();
                str += $"\r\nTotal objects: {oids.Count}";

                try
                {
                    foreach (var oid in oids)
                    {
                        string oidString = $"{oid}";
                        string eventString = "";
                        try
                        {
                            IbaSnmpOid suff = _lib.GetOidSuffixForFullOid(oid);
                            if (suff != null)
                            {
                                oidString += $" (suff {suff})";
                                if (_lib.IsEventHandlerRegistered(suff))
                                {
                                    eventString = " [Has EVENT handler]";
                                }
                            }
                        }
                        catch
                        {
                            //
                        }

                        IbaSnmpValueType type;
                        object val = _lib.GetValue(oid, out type);
                        string valString = val?.ToString() ?? "<null>";

                        string typeString = $"{type}";

                        if (IbaSnmp.IsInEnumRegion(type) && _lib.IsEnumDataTypeRegistered(type))
                        {
                            string typeName = _lib.GetEnumDataTypeName(type);
                            typeString = $"{(int) type}={typeName}";

                            if (val is int)
                            {
                                string enumValueString = _lib.GetEnumValueName(type, (int) val) ?? "<???>";
                                valString += $" ({enumValueString})";
                            }
                        }

                        str += $"\r\n{oidString} = ({typeString}) {valString}{eventString}";
                    }

                }
                catch
                {
                    str += " [Ex]";
                }
            }

            return str;
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                _data.IbaSnmp.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonStop_Click(object sender, EventArgs e)
        {
            try
            {
                _data.IbaSnmp.Stop();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
