﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using iba.Data;
using iba.ibaOPCServer;
using iba.Logging;
using iba.Processing;
using iba.Properties;
using iba.Utility;

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

            ImageList pdaList = new ImageList
            {
                ImageSize = new Size(16, 16),
                TransparentColor = Color.Magenta,
                ColorDepth = ColorDepth.Depth24Bit
            };
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

                // rebuild gui-tree
                RebuildObjectsTree(); 

                // user has selected the control, 
                // enable clients monitoring
                timerRefreshStatus.Enabled = true;

                // first time refresh status immediately
                timerRefreshStatus_Tick(null, null);
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $@"{nameof(OpcUaControl)}.{nameof(LoadData)}: " + ex.Message);
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
                LogData.Data.Logger.Log(Level.Exception, $@"{nameof(OpcUaControl)}.{nameof(SaveData)}: " + ex.Message);
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

                // rebuild GUI tree
                RebuildObjectsTree();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $@"{nameof(OpcUaControl)}.{nameof(buttonConfigurationApply_Click)}. {ex.Message}");
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
            _data = OpcUaData.DefaultData;
            _data.Enabled = bOriginalEnabledState;

            try
            {
                ConfigurationFromDataToControls();
                // set data to manager and restart UA server if necessary
                TaskManager.Manager.OpcUaData = _data.Clone() as OpcUaData;

                // rebuild GUI tree
                RebuildObjectsTree();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $@"{nameof(OpcUaControl)}.{nameof(buttonConfigurationReset_Click)}. {ex.Message}");
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

       
        #region Endpoints

        // todo. kls. delete
        private void buttonCopyToClipboard_Click(object sender, EventArgs e)
        {
            try
            {
                Clipboard.Clear();
                Clipboard.SetText("Hello");

                if (dgvEndpoints.RowCount < 1)
                    return;

                string uri = dgvEndpoints.Rows[0].Cells[2].Value as string;
                if (string.IsNullOrWhiteSpace
                    (uri))
                    MessageBox.Show("err");
                else
                    Clipboard.SetText(uri);
            }
            catch
            {
                MessageBox.Show("Clipboard err");
            }
        }

        private void buttonEndpointAdd_Click(object sender, EventArgs e)
        {
            var dep = OpcUaData.DefaultEndPoint;

            dgvEndpoints.Rows.Add(dep.Hostname, dep.Port, dep.Uri);
        }

        private void dgvEndpoints_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (!(sender is DataGridView view))
                return;
            DataGridViewCell cell = view.Rows[e.RowIndex].Cells[e.ColumnIndex];

            string val = cell.EditedFormattedValue as string;
            if (val is null)
                return;

            switch (e.ColumnIndex)
            {
                case 0: // host

                    break;
                case 1: // port
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
            return new object[] { ep.Hostname, ep.Port, ep.Uri };
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

        /// <summary> Gets a list of selected cells,
        /// including the case when no entire string is selected but only one cell </summary>
        /// <returns></returns>
        private List<DataGridViewRow> GetSelectedRowsIncludingSingleCell()
        {
            List<DataGridViewRow> rows = new List<DataGridViewRow>();

            if (dgvEndpoints.RowCount < 1)
            {
                // table is empty, nothing to copy
                return rows;
            }

            if (dgvEndpoints.SelectedRows.Count < 1)
            {
                // no rows selected, but probably at least one cell is selected
                DataGridViewSelectedCellCollection sc = dgvEndpoints.SelectedCells;

                int rowToProcess = sc.Count > 0
                    ? sc[0].RowIndex /*process the cell's row*/
                    : dgvEndpoints.RowCount - 1 /*process last row*/;

                rows.Add(dgvEndpoints.Rows[rowToProcess]);
            }
            else
            {
                for (int i = dgvEndpoints.SelectedRows.Count - 1; i >= 0; i--)
                    rows.Add(dgvEndpoints.SelectedRows[i]);
            }
            return rows;
        }

        private void buttonEndpointCopy_Click(object sender, EventArgs e)
        {
            // copy selected rows
            foreach (var row in GetSelectedRowsIncludingSingleCell())
            {
                OpcUaData.OpcUaEndPoint ep = RowToEndpoint(row);
                dgvEndpoints.Rows.Add(EndpointToRow(ep));
            }
        }

        private void buttonEndpointDelete_Click(object sender, EventArgs e)
        {
            // delete selected rows
            foreach (var row in GetSelectedRowsIncludingSingleCell())
            {
                dgvEndpoints.Rows.Remove(row);
            }
        }

        #endregion


        #endregion


        #region Diagnostics

        private void RefreshBriefStatus()
        {
            try
            {
                var status = TaskManager.Manager.OpcUaGetBriefStatus();
                tbStatus.Text = status.Item2;
                tbStatus.BackColor = SnmpControl.StatusToColor(status.Item1);
            }
            catch (Exception ex)
            {
                tbStatus.Text = "";
                tbStatus.BackColor = BackColor;
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(RefreshBriefStatus)}. {ex.Message}");
            }
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


        // todo. kls. delete?
        private void buttonClearClients_Click(object sender, EventArgs e)
        {
            // reset monitoring counters in ibaSnmp
            try
            {
                //TaskManager.Manager.OpcUaClearClients();
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
                var objSnapshot = TaskManager.Manager.OpcUaGetObjectTreeSnapShot();

                if (objSnapshot == null)
                {
                    Debug.Assert(false); // should not happen
                    return;
                }

                foreach (var tag in objSnapshot)
                {
                    if (tag == null)
                    {
                        Debug.Assert(false); // should not happen
                        // ReSharper disable once HeuristicUnreachableCode
                        continue;
                    }

                    string parentId = IbaUaNodeManager.GetParentName(tag.OpcUaNodeId);
                    TreeNode parentGuiNode = string.IsNullOrWhiteSpace(parentId) ? 
                        null : FindSingleNodeById(parentId);

                    TreeNodeCollection collection = parentGuiNode == null ? 
                        tvObjects.Nodes /* add to root by default */: 
                        parentGuiNode.Nodes;

                    int imageIndex = tag.IsFolder ? ImageIndexFolder : ImageIndexLeaf;

                    TreeNode guiNode = collection.Add(
                        tag.OpcUaNodeId, tag.Caption, imageIndex, imageIndex);
                    guiNode.Tag = tag;
                }

                // nodes to expand; (order/sorting is not important; ancestors are expanded automatically)
                var nodesToExpand = new HashSet<string>
                {
                    "ibaDatCoordinator\\StandardJobs",
                    "ibaDatCoordinator\\ScheduledJobs",
                    "ibaDatCoordinator\\OneTimeJobs",
                    "ibaDatCoordinator\\EventJobs"
                };

                // expand those which are marked for
                foreach (var str in nodesToExpand)
                {
                    SnmpControl.ExpandNodeAndAllAncestors(FindSingleNodeById(str));
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $@"{nameof(SnmpControl)}.{nameof(RebuildObjectsTree)}. {ex.Message}");
            }

            // navigate to recent user selected node if possible
            SnmpControl.SelectTreeNode(tvObjects, _recentId);
        }

        /// <summary> Looks for a given id in the <see cref="tvObjects"/> </summary>
        private TreeNode FindSingleNodeById(string id) =>
            SnmpControl.FindSingleNodeById(tvObjects.Nodes, id);

        /// <summary> The most recent node that was selected by the user </summary>
        private string _recentId;

        private void tvObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!IsConnectedOrLocal)
            {
                return;
            }

            try
            {
                // reset recently selected node
                _recentId = null;

                // reset all fields
                tbObjValue.Text = String.Empty;
                tbObjType.Text = String.Empty;
                tbObjNodeId.Text = String.Empty;
                tbObjDescription.Text = String.Empty;

                // get existing node's tag
                if (!(e.Node.Tag is ExtMonData.GuiTreeNodeTag tag))
                {
                    // should not happen; each node should be equipped with a tag
                    Debug.Assert(false); 
                    return;
                }

                // try to refresh node's tag
                try
                {
                    tag = TaskManager.Manager.OpcUaGetTreeNodeTag(tag.OpcUaNodeId);
                }
                catch
                {
                    // reset value that we know that something is wrong
                    tag.Value = String.Empty;
                    tag.Type = String.Empty;
                }

                tbObjValue.Text = tag.Value;
                tbObjType.Text = tag.Type;
                tbObjNodeId.Text = tag.OpcUaNodeId;
                tbObjDescription.Text = tag.Description;

                // remember recently selected node
                _recentId = tag.OpcUaNodeId;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Debug,
                    $@"{nameof(OpcUaControl)}.{nameof(tvObjects_AfterSelect)}. Exception: " + ex.Message);
            }
        }

        #endregion


        // todo. kls. delete
        private void buttonSetTestCfg_Click(object sender, EventArgs e)
        {
            // copy default data to current data except enabled/disabled
            _data = (new OpcUaData()).Clone() as OpcUaData;
            Debug.Assert(_data != null);
            if (_data == null)
                return;

            _data.Enabled = true;

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

        // todo. kls. delete
        private void buttonRebuildTree_Click(object sender, EventArgs e)
        {
            TaskManager.Manager.OpcUaRebuildObjectTree();
        }

        // todo. kls. delete
        private void buttonRefreshGuiTree_Click(object sender, EventArgs e)
        {
            this.RebuildObjectsTree();
        }
    }
}
