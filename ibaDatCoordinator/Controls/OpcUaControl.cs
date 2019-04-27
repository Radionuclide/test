using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using iba.Data;
using iba.Logging;
using iba.Processing;
using iba.Properties;
using iba.Utility;
using IbaSnmpLib;

namespace iba.Controls
{
    public partial class OpcUaControl : UserControl, IPropertyPane
    {
        #region Construction, Destruction, Init

        public OpcUaControl()
        {
            InitializeComponent();
        }

        private const int ImageIndexFolder = 0;
        private const int ImageIndexLeaf = 1;

        private CollapsibleElementManager _ceManager;

        private void OpcUaControl_Load(object sender, EventArgs e)
        {
            // initialize collapsible group boxes
            _ceManager = new CollapsibleElementManager(this);

            gbConfiguration.Init();
            _ceManager.AddElement(gbConfiguration);

            gbObjects.Init();
            _ceManager.AddElement(gbObjects);

            gbDiagnostics.Init();
            _ceManager.AddElement(gbDiagnostics);

            // bind password text boxes to their show/hide buttons
            buttonShowPassword.Tag = tbPassword;

            ImageList pdaList = new ImageList();
            pdaList.ImageSize = new Size(16, 16);
            pdaList.TransparentColor = Color.Magenta;
            pdaList.ColorDepth = ColorDepth.Depth24Bit;
            pdaList.Images.AddStrip(Resources.snmp_images);

            // image list for objects TreeView
            ImageList tvObjectsImageList = new ImageList();
            // folder
            tvObjectsImageList.Images.Add(pdaList.Images[0]);
            // leaf
            tvObjectsImageList.Images.Add(pdaList.Images[2]);
            tvObjects.ImageList = tvObjectsImageList;
            tvObjects.ImageIndex = ImageIndexFolder;

            // set up DataGridView
            dgvColumnHost.ValueType = typeof(string);
            dgvColumnPort.ValueType = typeof(int);
            dgvColumnUri.ValueType = typeof(string);
        }

        #endregion


        #region IPropertyPane Members

        private OpcUaData _data;

        public void LoadData(object dataSource, IPropertyPaneManager manager)
        {
            try
            {
                _data = dataSource as OpcUaData; // clone of current Manager's data

                // if data is wrong, disable all controls, and cancel load
                Enabled = _data != null;
                if (_data == null)
                {
                    return;
                }

                // read from data to controls
                ConfigurationFromDataToControls();

                //// force rebuild snmpworker's tree to ensure we have most recent information
                //TaskManager.Manager.SnmpRebuildObjectTree();

                // rebuild gui-tree
                //RebuildObjectsTree();

                // user has selected the control, 
                // enable clients monitoring
                timerRefreshStatus.Enabled = true;

                // first time refresh status immediately
                timerRefreshStatus_Tick(null, null);
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, @"OpcUaControl.LoadData() exception: " + ex.Message);
            }
        }

        public void SaveData()
        {
            try
            {
                ConfigurationFromControlsToData();
                // set data to manager and restart snmp agent if necessary
                TaskManager.Manager.OpcUaData = _data.Clone() as OpcUaData;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, @"OpcUaControl.SaveData() exception: " + ex.Message);
            }
        }

        public void LeaveCleanup()
        {
            //  user has hidden the control, 
            // disable clients monitoring
            timerRefreshStatus.Enabled = false;
        }

        #endregion




        #region Configuration

        private void buttonConfigurationApply_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationFromControlsToData();
                // set data to manager and restart snmp agent if necessary
                TaskManager.Manager.OpcUaData = _data.Clone() as OpcUaData;

                // rebuild the tree because probably textual conventions were changed
                //TaskManager.Manager.SnmpRebuildObjectTree();// todo. kls. 

                // rebuild GUI tree
                RebuildObjectsTree();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    @"SnmpControl.buttonConfigurationApply_Click() exception: " + ex.Message);
            }
        }

        private void buttonConfigurationReset_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this,
                    Resources.snmpQuestionReset, /*snmp string here is ok for opc ua also*/
                    Resources.snmpQuestionResetTitle, /*snmp string here is ok for opc ua also*/
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            // copy default data to current data except enabled/disabled
            bool bOriginalEnabledState = _data.Enabled;
            _data = (new OpcUaData()).Clone() as OpcUaData;
            Debug.Assert(_data != null);
            if (_data == null)
                return;
            _data.Enabled = bOriginalEnabledState;

            try
            {
                ConfigurationFromDataToControls();
                // set data to manager and restart snmp agent if necessary
                TaskManager.Manager.OpcUaData = _data.Clone() as OpcUaData;

                // rebuild the tree because probably textual conventions were changed
                TaskManager.Manager.SnmpRebuildObjectTree();

                // rebuild GUI tree
                RebuildObjectsTree();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    @"OpcUaControl.buttonConfigurationReset_Click() exception: " + ex.Message);
            }
        }

        private void ConfigurationFromControlsToData()
        {
            // general
            _data.Enabled = cbEnabled.Checked;

            // logon
            _data.UserName = tbUserName.Text;
            _data.Password = tbPassword.Text;
            _data.HasUserCertificate = cbLogonCertificate.Checked;

            // security policies
            _data.HasSecurityNone = cbSecurityNone.Checked;
            _data.HasSecurityBasic128 = cbSecurity128.Checked;
            _data.HasSecurityBasic256 = cbSecurity256.Checked;

            _data.SecurityBasic128Level = (OpcUaData.OpcUaSecurityLevel)comboBoxSecurity128.SelectedIndex;
            _data.SecurityBasic256Level = (OpcUaData.OpcUaSecurityLevel)comboBoxSecurity256.SelectedIndex;

            // endpoints
            _data.Endpoints.Clear();
            for (int i = 0; i < dgvEndpoints.RowCount; i++)
            {
                OpcUaData.OpcUaEndPoint ep = RowToEndpoint(dgvEndpoints.Rows[i]);
                _data.Endpoints.Add(ep);
            }
        }

        private void ConfigurationFromDataToControls(OpcUaData data = null)
        {
            if (data == null)
            {
                data = _data;
            }

            // general
            cbEnabled.Checked = data.Enabled;

            // logon
            tbUserName.Text = data.UserName;
            tbPassword.Text = data.Password;
            cbLogonCertificate.Checked = data.HasUserCertificate;

            // security policies
            cbSecurityNone.Checked = data.HasSecurityNone;
            cbSecurity128.Checked = data.HasSecurityBasic128;
            cbSecurity256.Checked = data.HasSecurityBasic256;

            comboBoxSecurity128.SelectedIndex = (int)data.SecurityBasic128Level;
            comboBoxSecurity256.SelectedIndex = (int)data.SecurityBasic256Level;

            // endpoints
            dgvEndpoints.Rows.Clear();
            if (_data.Endpoints != null)
            {
                foreach (var ep in _data.Endpoints)
                    //dgvEndpoints.Rows.Add("a",1,"bb");
                                    dgvEndpoints.Rows.Add(EndpointToRow(ep));
            }

            // show/hide elements
            cbSecurity128_CheckedChanged(null, null);
            cbSecurity256_CheckedChanged(null, null);
        }

        /// <summary>
        /// Shows password in a bound TextBox. 
        /// Before using, first connect your Button and TextBox like:
        /// "showHideButton.Tag = TextBoxToBeControlled"
        /// </summary>
        private void handler_ShowPassword(object sender, MouseEventArgs e)
        {
            TextBox tb = (sender as Button)?.Tag as TextBox;
            if (tb == null)
            {
                return;
            }
            tb.PasswordChar = '\0';
        }


        /// <summary>
        /// Hides password in a bound TextBox. 
        /// Before using, first connect your Button and TextBox like:
        /// "showHideButton.Tag = TextBoxToBeControlled"
        /// </summary>
        private void handler_HidePassword(object sender, MouseEventArgs e)
        {
            TextBox tb = (sender as Button)?.Tag as TextBox;
            if (tb == null)
            {
                return;
            }
            tb.PasswordChar = '*';
        }

        private void cbSecurity128_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxSecurity128.Enabled = cbSecurity128.Checked;
        }

        private void cbSecurity256_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxSecurity256.Enabled = cbSecurity256.Checked;
        }
        #endregion


        #region Diagnostics

        private void RefreshBriefStatus()
        {
            try
            {
                var status = TaskManager.Manager.OpcUaGetBriefStatus();
                tbStatus.Text = status.Item2;
                tbStatus.BackColor = StatusToColor(status.Item1);
            }
            catch (Exception ex)
            {
                tbStatus.Text = "";
                tbStatus.BackColor = BackColor;
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(RefreshBriefStatus)}. {ex.Message}");
            }
        }

        public static Color StatusToColor(SnmpWorkerStatus status)
        {
            return status == SnmpWorkerStatus.Started
                ? Color.LimeGreen // running
                : (status == SnmpWorkerStatus.Stopped
                    ? Color.LightGray // stopped
                    : Color.Red); // error
        }

        private void RefreshClientsTable()
        {
            try
            {
                // clear list
                dgvClients.Rows.Clear();

                // show new data
                //List<IbaSnmpDiagClient> clients = TaskManager.Manager.SnmpGetClients();

                //// can happen when suddenly disconnected
                //if (clients == null)
                //{
                //    return;
                //}

                //foreach (var client in clients)
                //{
                //    dgvClients.Rows.Add(client.Address, client.Version, client.MessageCount, client.LastMessageReceived);
                //}
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(RefreshClientsTable)}. {ex.Message}");
            }
        }

        private void timerRefreshStatus_Tick(object sender, EventArgs e)
        {
            bool isConnectedOrLocal = IsConnectedOrLocal;

            buttonConfigurationReset.Enabled = buttonConfigurationApply.Enabled =
                gbObjects.Enabled = gbDiagnostics.Enabled = isConnectedOrLocal;

            if (isConnectedOrLocal)
            {
                RefreshBriefStatus();
                RefreshClientsTable();
            }
            else
            {
                tbStatus.Text = "";
                tbStatus.BackColor = BackColor;
            }
        }

        private static bool IsConnectedOrLocal =>
            Program.RunsWithService == Program.ServiceEnum.NOSERVICE ||
            Program.RunsWithService == Program.ServiceEnum.CONNECTED;


        private void buttonClearClients_Click(object sender, EventArgs e)
        {
            // reset monitoring counters in ibaSnmp
            try
            {
                TaskManager.Manager.SnmpClearClients();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(buttonClearClients_Click)}. {ex.Message}");
            }

            // refresh it in GUI
            RefreshClientsTable();
        }

        #endregion


        #region Objects

        public void RebuildObjectsTree()
        {
            if (!IsConnectedOrLocal)
            {
                return;
            }

            tvObjects.Nodes.Clear();

            try
            {
                var objSnapshot = TaskManager.Manager.SnmpGetObjectTreeSnapShot();

                if (objSnapshot == null)
                {
                    return;
                }

                // get sorted oids, to ensure we create nodes according to depth-first search order
                var sortedOids = objSnapshot.Keys.ToList();
                sortedOids.Sort();


                var nodesToExpand = new List<TreeNode>();

                foreach (var oid in sortedOids)
                {
                    var tag = objSnapshot[oid];

                    // get parent node
                    var parentOid = oid.GetParent();
                    var parentNode = FindSingleNodeById(null /*parentOid*/);

                    // if parent node exists, add item there.
                    // otherwise add directly to the root
                    var placeToAddTo = parentNode?.Nodes ?? tvObjects.Nodes;

                    // for all but root nodes add least subId before string caption
                    string leastIdPrefix = parentNode == null ? "" : $@"{oid.GetLeastSignificantSubId()}. ";

                    string captionWithSubid = leastIdPrefix + tag.Caption;

                    int imageindex = tag.IsFolder ? ImageIndexFolder : ImageIndexLeaf;

                    // add this item to parent node
                    var node = placeToAddTo.Add(oid.ToString(), captionWithSubid, imageindex, imageindex);
                    node.Tag = tag;

                    // mark for expanding
                    if (tag.IsExpandedByDefault)
                    {
                        nodesToExpand.Add(node);
                    }
                }

                // expand those which are marked for
                foreach (var treeNode in nodesToExpand)
                {
                    treeNode.Expand();
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $@"{nameof(SnmpControl)}.{nameof(RebuildObjectsTree)}. {ex.Message}");
            }

            // navigate to last selected oid if possible
            if (_lastId == null)
            {
                return;
            }

            //var parents = null;//_lastId.GetParents();
            //foreach (IbaSnmpOid oid in parents)
            //{
            //    try
            //    {
            //        FindSingleNodeById(oid)?.Expand();
            //    }
            //    catch
            //    {
            //        // just go on with others
            //    }
            //}

            tvObjects.SelectedNode = FindSingleNodeById(_lastId);

            tvObjects.Select();
            tvObjects.Focus();
        }

        private TreeNode FindSingleNodeById(string id)
        {
            if (id == null)
            {
                // should not happen
                throw new ArgumentNullException(nameof(id));
            }

            // check if exists
            TreeNode[] nodes = tvObjects.Nodes.Find(id, true);

            if (nodes.Length == 1)
            {
                // ok, found exactly one match
                return nodes[0];
            }
            if (nodes.Length > 1)
            {
                // Length > 1
                // should not happen
                // inconsisnensy?
                throw new Exception($@"Found more than one match for {id}");
            }
            return null; // not found
        }


        /// <summary> The last Oid that was selected by the user </summary>
        private string _lastId;

        private void tvObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!IsConnectedOrLocal)
            {
                return;
            }

            try
            {
                // reset last selected oid
                _lastId = null;

                // reset all fields
                //tbObjOid.Text = String.Empty;
                tbObjValue.Text = String.Empty;
                tbObjType.Text = String.Empty;

                // get existing node's tag
                var tag = (SnmpTreeNodeTag)e.Node.Tag;

                // try to refresh node's tag
                try
                {
                    tag = TaskManager.Manager.SnmpGetTreeNodeTag(tag.Oid);
                }
                catch (Exception)
                {
                    // reset value that we know that something is wrong
                    tag.Value = String.Empty;
                    tag.Type = String.Empty;
                }

                //tbObjOid.Text = tag.Oid?.ToString();
                tbObjValue.Text = tag.Value;
                tbObjType.Text = tag.Type;

                // remember last selected oid
                _lastId = null;// tag.Oid;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Debug,
                    @"SnmpControl.tvObjects_AfterSelect(). Exception: " + ex.Message);
            }
        }


        #endregion


        // todo. kls. delete
        private void buttonCopyToClipboard_Click(object sender, EventArgs e)
        {
            if (dgvEndpoints.RowCount < 1)
                return;

            string uri = dgvEndpoints.Rows[0].Cells[2].Value as string;
            if (string.IsNullOrWhiteSpace(uri))
                MessageBox.Show("err");
            else
                Clipboard.SetText(uri);
        }

        private void buttonEndpointAdd_Click(object sender, EventArgs e)
        {
            var dep = OpcUaData.DefaultEndPoint;

            int r = dgvEndpoints.Rows.Add(dep.Hostname, dep.Port, dep.Uri);

            var x = dgvEndpoints.ReadOnly;
            var y = dgvEndpoints.Rows[r].ReadOnly;

            OpcUaData.OpcUaEndPoint ep = new OpcUaData.OpcUaEndPoint(IPAddress.None, 80);
        }

        // todo. kls. 
        private void RefreshEndpoints()
        {
            try
            {
                dgvEndpoints.Rows.Clear();
                if (_data.Endpoints == null)
                    return;
                foreach (var ep in _data.Endpoints)
                {
                    dgvClients.Rows.Add(ep.AddressOrHostName, ep.Port, ep.Uri);
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(RefreshClientsTable)}. {ex.Message}");
            }
        }

        private void dgvEndpoints_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!(sender is DataGridView view))
                return;
            DataGridViewRow row = view.Rows[e.RowIndex];
            DataGridViewCell cell = view.Rows[e.RowIndex].Cells[e.ColumnIndex];

            //cell.
        }

        private void dgvEndpoints_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (!(sender is DataGridView view))
                return;
            DataGridViewRow row = view.Rows[e.RowIndex];
            DataGridViewCell cell = view.Rows[e.RowIndex].Cells[e.ColumnIndex];

            string val = cell.EditedFormattedValue as string;
            if (val is null)
                return;

            switch (e.ColumnIndex)
            {
                case 0: // host

                    break;
                case 1: // port
                    //var x = cell.EditedFormattedValue;
                    if (int.TryParse(val, out int intVal))
                    {
                        if (intVal < 0) intVal = 0;
                        if (intVal > 65535) intVal = 65535;
                        e.Value = intVal;
                    }
                    else
                        e.Value = 0;
                    e.ParsingApplied = true;
                    break;
            }
        }

        private static OpcUaData.OpcUaEndPoint RowToEndpoint(DataGridViewRow row)
        {
            try
            {
                // todo. kls. reuse cell parsing
                string hostVal = row.Cells[0].Value as string;
                int port = (int)row.Cells[1].Value;

                if (string.IsNullOrWhiteSpace(hostVal))
                    return OpcUaData.DefaultEndPoint;

                var ep = IPAddress.TryParse(hostVal, out IPAddress address) ?
                    new OpcUaData.OpcUaEndPoint(address, port) :
                    new OpcUaData.OpcUaEndPoint(hostVal /*treat it as hostname*/, port);

                return ep;
            }
            catch
            {
                return OpcUaData.DefaultEndPoint;
            }
        }

        private static object[] EndpointToRow(OpcUaData.OpcUaEndPoint ep)
        {
            return new object[] { ep.AddressOrHostName, ep.Port, ep.Uri };
        }

        private void UpdateRowUri(DataGridViewRow row)
        {
            OpcUaData.OpcUaEndPoint ep = RowToEndpoint(row);
            row.Cells[2].Value = ep.Uri;
        }

        private void dgvEndpoints_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (!(sender is DataGridView view))
                return;
            DataGridViewRow row = view.Rows[e.RowIndex];
            DataGridViewCell cell = view.Rows[e.RowIndex].Cells[e.ColumnIndex];

            e.ThrowException = false;
        }

        private void dgvEndpoints_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!(sender is DataGridView view))
                return;
            if (e.RowIndex < 0)
                return;

            // update URI only if host or port is changed
            if (e.ColumnIndex < 0 || e.ColumnIndex > 1)
                return;

            DataGridViewRow row = view.Rows[e.RowIndex];
            UpdateRowUri(row);
        }

        private void buttonEndpointCopy_Click(object sender, EventArgs e)
        {
            if (dgvEndpoints.SelectedRows.Count < 1)
            {
                // todo. kls. handle single cell selection
                DataGridViewSelectedCellCollection sc = dgvEndpoints.SelectedCells;
                if (sc.Count != 1)
                    return;
                dgvEndpoints.SelectedRows.Insert(0, dgvEndpoints.Rows[sc[0].RowIndex]);
            }

            for (int i = dgvEndpoints.SelectedRows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dgvEndpoints.SelectedRows[i];

                OpcUaData.OpcUaEndPoint ep = RowToEndpoint(row);
                dgvEndpoints.Rows.Add(EndpointToRow(ep));
            }
        }

        private void buttonEndpointDelete_Click(object sender, EventArgs e)
        {
            if (dgvEndpoints.SelectedRows.Count < 1)
            {
                // todo. kls. handle single cell selection
                return;
            }

            for (int i = dgvEndpoints.SelectedRows.Count - 1; i >= 0; i--)
            {
                DataGridViewRow row = dgvEndpoints.SelectedRows[i];
                dgvEndpoints.Rows.Remove(row);
            }
        }

        private void buttonSetTestCfg_Click(object sender, EventArgs e)
        {
            // copy default data to current data except enabled/disabled
            _data = (new OpcUaData()).Clone() as OpcUaData;
            Debug.Assert(_data != null);
            if (_data == null)
                return;

            _data.UserName = "Anonymous2";
            _data.Password = "123456";

            _data.HasSecurityBasic128 = true;
            _data.SecurityBasic128Level = OpcUaData.OpcUaSecurityLevel.SignSignEncrypt;

            OpcUaData.OpcUaEndPoint ep = new OpcUaData.OpcUaEndPoint("LsWork", 21060);
            _data.Endpoints.Clear();
            _data.Endpoints.Add(ep);
            _data.Endpoints.Add(OpcUaData.DefaultEndPoint);


            ConfigurationFromDataToControls();

            buttonCopyToClipboard_Click(null,null);
            
            //// set data to manager and restart snmp agent if necessary
            //TaskManager.Manager.OpcUaData = _data.Clone() as OpcUaData;

        }

        private OpcUaWorker Tst__Worker => Manager.Tst___OpcUaWorker; // todo. kls. delete

        private void buttonTest2_Click(object sender, EventArgs e)
        {
        }

        private void buttonTest1_Click(object sender, EventArgs e)
        {

        }

        static TaskManager Manager => TaskManager.Manager;

        private void buttonRebuildTree_Click(object sender, EventArgs e)
        {
            Manager.OpcUaRebuildObjectTree();
        }
    }
}
