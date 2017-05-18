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

        #region Construction, Destruction, Init

        public SnmpControl()
        {
            InitializeComponent();
            _tmp___instCounter++;

            gbDebug.Init();
            gbConfiguration.Init();
            gbDiagnostics.Init();
            gbObjects.Init();

            // todo
            // fill combo boxes not by hand but from enums 
            // to ensure reliability
            //for (int i = (int)IbaSnmpAuthenticationAlgorithm.Md5; i < (int)IbaSnmpAuthenticationAlgorithm.Sha; i++)
            //{
            //    cmbAuthentication.Items;
            //}
        }
        
        #endregion


        #region Debug

        private static int _tmp___instCounter = 0;
        private int _tmp___cnt1;
        private int _tmp___cnt2;
        private int _tmp___cnt3;
        private int _tmp___cntTimer;

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                IbaSnmp ibaSnmp = TaskManager.Manager.SnmpWorker.IbaSnmp;
                ibaSnmp.Start();
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
                IbaSnmp ibaSnmp = TaskManager.Manager.SnmpWorker.IbaSnmp;
                ibaSnmp.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        /// <summary>
        /// Get list of endpoints and configured traps. This function is not effective and can be use in debug-time only
        /// </summary>
        /// <returns></returns>
        public string _tmp___GetLibraryDescriptionString(IbaSnmp _lib)
        {
            if (_lib == null)
            {
                return "";
            }

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
                            typeString = $"{(int)type}={typeName}";

                            if (val is int)
                            {
                                string enumValueString = _lib.GetEnumValueName(type, (int)val) ?? "<???>";
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

        #endregion


        #region IPropertyPane Members

        private SnmpData _data;
        //private IbaSnmp _ibaSnmp;

        private bool _isUserModeActive;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            _data = datasource as SnmpData; // clone of current Manager's data
            //_ibaSnmp = TaskManager.Manager.SnmpWorker.IbaSnmp;

            this.Enabled = _data != null;
            if (_data == null)
            {
                return;
            }

            // todo
            label1.Text = $"Data loaded {_tmp___cnt1++}";

            _isUserModeActive = false;

            // read from data to controls
            try
            {
                ConfigurationFromDataToControls();
                InitializeObjectsTree();
                UpdateStatusText();
            }
            catch (Exception ex)
            {
                // todo details
                MessageBox.Show(ex.ToString());
            }

            timerStatus.Enabled = true;
            _isUserModeActive = true;
        }

        public void SaveData()
        {
            label3.Text = $"Data Saved {_tmp___cnt3++}";

            TaskManager.Manager.SnmpData = _data.Clone() as SnmpData;
        }

        public void LeaveCleanup()
        {
            label2.Text = $"Data cleaned {_tmp___cnt2++}";

            timerStatus.Enabled = false;
        }

        #endregion


        #region Configuration


        private void buttonConfigurationApply_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationFromControlsToData();
            }
            catch (Exception ex)
            {
                // todo details
                MessageBox.Show(ex.ToString());
            }
        }

        private void ConfigurationFromControlsToData()
        {
            // general
            _data.Enabled = cbEnabled.Checked;
            _data.Port = (int)numPort.Value;
            // security v1 v2
            _data.V1V2Security = tbCommunity.Text;
            // security v3
            IbaSnmpUserAccount v3S = new IbaSnmpUserAccount();

            int indAuth = cmbAuthentication.SelectedIndex;
            v3S.AuthAlgorithm = indAuth == -1
                ? IbaSnmpAuthenticationAlgorithm.Md5 // default
                : (IbaSnmpAuthenticationAlgorithm)indAuth; // just cast position in the list to enum

            int indEncr = cmbEncryption.SelectedIndex;
            v3S.EncrAlgorithm = indEncr == -1
                ? IbaSnmpEncryptionAlgorithm.None // default
                : (IbaSnmpEncryptionAlgorithm)indEncr; // just cast position in the list to enum

            v3S.Username = tbUserName.Text;
            v3S.Password = tbPassword.Text;
            v3S.EncryptionKey = tbEncryptionKey.Text;

            _data.V3Security = v3S;

            // set data to manager and restart snmp agent if necessary
            ApplyConfigurationToManager();
        }

        private void ConfigurationFromDataToControls()
        {
            // general
            cbEnabled.Checked = _data.Enabled;
            numPort.Value = _data.Port;
            // security v1 v2
            tbCommunity.Text = _data?.V1V2Security;
            // security v3
            var v3S = _data.V3Security;
            tbUserName.Text = v3S.Username;
            tbPassword.Text = v3S.Password;
            tbEncryptionKey.Text = v3S.EncryptionKey;
            cmbAuthentication.SelectedIndex = (int)v3S.AuthAlgorithm;
            cmbEncryption.SelectedIndex = (int)v3S.EncrAlgorithm;
        }
        
        #endregion


        #region Diagnostics

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            _tmp___cntTimer++;

            label4.Text = $@"Instance {_tmp___instCounter} " + (_tmp___cntTimer%2 == 0 ? "|" : "-");


            if (_data == null)
            {
                tbDebug.Text = @"ibaSnmp == null";
                return;
            }
            IbaSnmp ibaSnmp = TaskManager.Manager.SnmpWorker.IbaSnmp;
            if (ibaSnmp!=null)

            tbDebug.Text = _tmp___GetLibraryDescriptionString(ibaSnmp);

            UpdateStatusText();
        }
        
        private void UpdateStatusText()
        {
            IbaSnmp ibaSnmp = TaskManager.Manager.SnmpWorker.IbaSnmp;
            if (_data == null || ibaSnmp == null)
            {
                return;
            }

            // todo use resource text
            // todo add errored status
            // todo move to manager

            tbStatus.Text = ibaSnmp.IsStarted ? "Started" : "Stopped";
            tbStatus.BackColor = ibaSnmp.IsStarted ? Color.LimeGreen : Color.Gray;
        }


        #endregion


        #region Objects
        
        public void InitializeObjectsTree()
        {
            TreeNodeCollection nodes = tvObjects.Nodes;
            nodes.Clear();

            if (_data == null)
            {
                return;
            }

            var nodeRoot = nodes.Add("1.3.6.1.4.1.45120");

            tvObjects.ExpandAll();
        }

        private void buttonRefresh_Click(object sender, EventArgs e)
        {
            InitializeObjectsTree();
        }

        #endregion

        private void ApplyConfigurationToManager()
        {
            try
            {
                TaskManager.Manager.SnmpData = _data.Clone() as SnmpData;
            }
            catch (Exception ex)
            {
                // todo details
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
