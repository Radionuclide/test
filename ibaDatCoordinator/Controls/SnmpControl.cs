using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using iba.Data;
using iba.Logging;
using iba.Processing;
using iba.Properties;
using IbaSnmpLib;


// all verbatim strings that are in the file (e.g. @"General") should NOT be localized.
// usual strings (e.g. "General") should be localized later.

namespace iba.Controls
{
    public partial class SnmpControl : UserControl, IPropertyPane
    {
        #region Construction, Destruction, Init

        public SnmpControl()
        {
            InitializeComponent();
        }

        private const int ImageIndexFolder = 0;
        private const int ImageIndexLeaf = 1;

        private void SnmpControl_Load(object sender, EventArgs e)
        {
            // bind password text boxes to their show/hide buttons
            buttonShowPassword.Tag = tbPassword;
            buttonShowEncryptionKey.Tag = tbEncryptionKey;

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
        }

        #endregion


        #region IPropertyPane Members

        private SnmpData _data;

        public void LoadData(object dataSource, IPropertyPaneManager manager)
        {
            try
            {
                _data = dataSource as SnmpData; // clone of current Manager's data

                // if data is wrong, disable all controls, and cancel load
                Enabled = _data != null;
                if (_data == null)
                {
                    return;
                }

                // read from data to controls
                ConfigurationFromDataToControls();

                //// force rebuild worker's tree to ensure we have most recent information
                // /*this is unnecessary*/ TaskManager.Manager.SnmpRebuildObjectTree();

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
                LogData.Data.Logger.Log(Level.Exception, @"SnmpControl.LoadData() exception: " + ex.Message);
            }
        }

        public void SaveData()
        {
            try
            {
                ConfigurationFromControlsToData();
                // set data to manager and restart snmp agent if necessary
                TaskManager.Manager.SnmpData = _data.Clone() as SnmpData;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, @"SnmpControl.SaveData() exception: " + ex.Message);
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

        private void tabControl1_SelectionChanged(Crownwood.DotNetMagic.Controls.TabControl sender, Crownwood.DotNetMagic.Controls.TabPage oldPage, Crownwood.DotNetMagic.Controls.TabPage newPage)
        {
            // show panel with Apply and Reset buttons only for Cfg tab
            panelFooter.Visible = tabConfiguration.Selected;
        }

        private void buttonConfigurationApply_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationFromControlsToData();
                // set data to manager and restart snmp agent if necessary
                TaskManager.Manager.SnmpData = _data.Clone() as SnmpData;

                // rebuild the tree because probably textual conventions were changed
                TaskManager.Manager.SnmpRebuildObjectTree();

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
                    Resources.snmpQuestionReset,
                    Resources.snmpQuestionResetTitle,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            {
                return;
            }

            SnmpData defData = new SnmpData();

            // copy default data to current data
            // but not all data, just configuration data
            // and do not reset enabled/disabled
            _data.Port = defData.Port;
            _data.V1V2Security = defData.V1V2Security;
            _data.V3Security = defData.V3Security;
            _data.UseSnmpV2TcForStrings = defData.UseSnmpV2TcForStrings;

            try
            {
                ConfigurationFromDataToControls();
                // set data to manager and restart snmp agent if necessary
                TaskManager.Manager.SnmpData = _data.Clone() as SnmpData;

                // rebuild the tree because probably textual conventions were changed
                TaskManager.Manager.SnmpRebuildObjectTree();

                // rebuild GUI tree
                RebuildObjectsTree();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    @"SnmpControl.buttonConfigurationReset_Click() exception: " + ex.Message);
            }
        }

        private void ConfigurationFromControlsToData()
        {
            // general
            _data.Enabled = cbEnabled.Checked;
            _data.Port = (int) numPort.Value;
            // misc
            _data.UseSnmpV2TcForStrings = rbDateTimeTc.Checked;
            // security v1 v2
            _data.V1V2Security = tbCommunity.Text;

            // security v3
            IbaSnmpUserAccount v3S = new IbaSnmpUserAccount();
            int indAuth = cmbAuthentication.SelectedIndex;
            v3S.AuthAlgorithm = indAuth == -1
                ? IbaSnmpAuthenticationAlgorithm.Md5 // default
                : (IbaSnmpAuthenticationAlgorithm) indAuth; // just cast position in the list to enum

            int indEncr = cmbEncryption.SelectedIndex;
            v3S.EncrAlgorithm = indEncr == -1
                ? IbaSnmpEncryptionAlgorithm.None // default
                : (IbaSnmpEncryptionAlgorithm) indEncr; // just cast position in the list to enum

            v3S.Username = tbUserName.Text;
            v3S.Password = tbPassword.Text;
            v3S.EncryptionKey = tbEncryptionKey.Text;

            _data.V3Security = v3S;
        }

        private void ConfigurationFromDataToControls(SnmpData data = null)
        {
            if (data == null)
            {
                data = _data;
            }
            // general
            cbEnabled.Checked = data.Enabled;
            numPort.Value = data.Port;
            // misc
            rbDateTimeTc.Checked = data.UseSnmpV2TcForStrings;
            rbDateTimeStr.Checked = !rbDateTimeTc.Checked;
            // security v1 v2
            tbCommunity.Text = data.V1V2Security;
            // security v3
            var v3S = data.V3Security;
            tbUserName.Text = v3S.Username;
            tbPassword.Text = v3S.Password;
            tbEncryptionKey.Text = v3S.EncryptionKey;
            cmbAuthentication.SelectedIndex = (int) v3S.AuthAlgorithm;
            cmbEncryption.SelectedIndex = (int) v3S.EncrAlgorithm;
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

        #endregion


        #region Diagnostics

        private void RefreshBriefStatus()
        {
            try
            {
                var status = TaskManager.Manager.SnmpGetBriefStatus();
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

        public static Color StatusToColor(ExtMonWorkerStatus status)
        {
            return status == ExtMonWorkerStatus.Started
                ? Color.LimeGreen // running
                : (status == ExtMonWorkerStatus.Stopped
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
                List<IbaSnmpDiagClient> clients = TaskManager.Manager.SnmpGetClients();

                // can happen when suddenly disconnected
                if (clients == null)
                {
                    return;
                }

                foreach (var client in clients)
                {
                    dgvClients.Rows.Add(client.Address, client.Version, client.MessageCount, client.LastMessageReceived);
                }
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
                tabObjects.Enabled = tabDiag.Enabled = isConnectedOrLocal;

            if (isConnectedOrLocal)
            {
                RefreshBriefStatus();
                RefreshClientsTable();

                // update currently selected node information in object tree
                UpdateObjectTreeNodeDescription();
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

                foreach (var oid in sortedOids)
                {
                    var tag = objSnapshot[oid];

                    // get parent node
                    var parentOid = oid.GetParent();
                    var parentNode = FindSingleNodeById(parentOid);

                    // if parent node exists, add item there.
                    // otherwise add directly to the root
                    var placeToAddTo = parentNode?.Nodes ?? tvObjects.Nodes;

                    // for all but root nodes add least subId before string caption
                    string leastIdPrefix = parentNode == null ? "" : $@"{oid.GetLeastSignificantSubId()}. ";

                    string captionWithSubId = leastIdPrefix + tag.Caption;

                    int imageIndex = tag.IsFolder ? ImageIndexFolder : ImageIndexLeaf;

                    // add this item to parent node
                    var node = placeToAddTo.Add(oid.ToString(), captionWithSubId, imageIndex, imageIndex);
                    node.Tag = tag;
                }


                // nodes to expand; (order/sorting is not important; ancestors are expanded automatically)
                var nodesToExpand = new HashSet<IbaSnmpOid>
                {
                    "1.3.6.1.4.1.45120.2.1.2", // Standard jobs
                    "1.3.6.1.4.1.45120.2.1.3", // Scheduled jobs
                    "1.3.6.1.4.1.45120.2.1.4", // One time jobs
                    "1.3.6.1.4.1.45120.2.1.5" // Event jobs
                };

                // expand those which are marked for
                foreach (IbaSnmpOid oid in nodesToExpand)
                {
                    ExpandNodeAndAllAncestors(FindSingleNodeById(oid));
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $@"{nameof(SnmpControl)}.{nameof(RebuildObjectsTree)}. {ex.Message}");
            }

            // navigate to recent user selected node if possible
            SelectTreeNode(tvObjects, _recentOid?.ToString());
        }

        /// <summary> Selects a given node in a given treeView
        /// and expands all its ancestors to make it visible </summary>
        public static void SelectTreeNode(TreeView treeView, string nodeId)
        {
            if (nodeId == null)
            {
                return;
            }
            var treeNode = FindSingleNodeById(treeView.Nodes, nodeId);
            if (treeNode == null)
            {
                // requested node was not found; likely, it's is deleted
                return;
            }
            // expand all ancestors to make the node visible
            ExpandNodeAndAllAncestors(treeNode.Parent);
            // select it
            treeView.SelectedNode = treeNode;
        }

        /// <summary> Expands given node and all its parents
        /// to ensure that given node is visible </summary>
        public static void ExpandNodeAndAllAncestors(TreeNode node)
        {
            if (node == null)
                return;

            while (true)
            {
                node.Expand();
                if (node.Parent == null)
                {
                    // root is reached
                    break;
                }
                node = node.Parent;
            }
        }

        /// <summary> Looks for a given id in a given collection </summary>
        public static TreeNode FindSingleNodeById(TreeNodeCollection collection, string id)
        {
            if (id == null)
            {
                // should not happen
                Debug.Assert(false);
                return null;
            }

            // check if exists
            TreeNode[] nodes = collection.Find(id, true);

            if (nodes.Length == 1)
            {
                // ok, found exactly one match
                return nodes[0];
            }
            if (nodes.Length > 1)
            {
                // Length > 1
                // throw new Exception($@"Found more than one match for {id}");
                // should not happen. inconsistency?
                Debug.Assert(false);
                return nodes[0];
            }
            return null; // not found
        }

        /// <summary> Looks for a given id in the <see cref="tvObjects"/> </summary>
        private TreeNode FindSingleNodeById(IbaSnmpOid oid) => 
            FindSingleNodeById(tvObjects.Nodes, oid.ToString());
        
        /// <summary> The most recent node that was selected by the user </summary>
        private IbaSnmpOid _recentOid;
        
        private void UpdateObjectTreeNodeDescription(IbaSnmpOid oid = null)
        {
            // if no argument then try to use the recent one
            if (oid == null)
            {
                oid = _recentOid;
            }

            // if there is no recent one, then return and do nothing
            if (oid == null)
            {
                return;
            }

            // try to refresh node's tag
            try
            {
                var tag = TaskManager.Manager.SnmpGetTreeNodeTag(oid);
                tbObjOid.Text = tag.SnmpOid?.ToString();
                tbObjValue.Text = tag.Value;
                tbObjType.Text = tag.Type;
                tbObjMibName.Text = tag.SnmpMibName;
                tbObjMibDescription.Text = tag.Description;
            }
            catch
            {
                // reset value that we know that something is wrong
                tbObjValue.Text = String.Empty;
                tbObjType.Text = String.Empty;
            }
        }

        private void tvObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (!IsConnectedOrLocal)
            {
                return;
            }

            try
            {
                // reset recently selected node
                _recentOid = null;

                // reset all fields
                tbObjOid.Text = String.Empty;
                tbObjValue.Text = String.Empty;
                tbObjMibName.Text = String.Empty;
                tbObjMibDescription.Text = String.Empty;
                tbObjType.Text = String.Empty;

                // get existing node's tag
                if (!(e.Node.Tag is ExtMonData.GuiTreeNodeTag tag))
                {
                    // should not happen; each node should be equipped with a tag
                    Debug.Assert(false);
                    return;
                }

                UpdateObjectTreeNodeDescription(tag.SnmpOid);

                // remember recently selected node
                _recentOid = tag.SnmpOid;
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Debug,
                    $@"{nameof(SnmpControl)}.{nameof(tvObjects_AfterSelect)}. Exception: " + ex.Message);
            }
        }

        private void buttonCreateMibFiles_Click(object sender, EventArgs e)
        {
            if (!IsConnectedOrLocal)
            {
                return;
            }

            if (folderBrowserDialog.ShowDialog()
                != DialogResult.OK)
            {
                return;
            }

            try
            {
                string dir = folderBrowserDialog.SelectedPath;

                // ensure we have the latest tree structure
                var mibFiles = TaskManager.Manager.SnmpGenerateMibFiles();

                if (mibFiles == null || mibFiles.Count == 0)
                {
                    MessageBox.Show(this,
                        Resources.snmpFailedMibFiles,
                        Resources.snmpFailedMibFiles,
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // create dir if not existing
                Directory.CreateDirectory(dir);

                // create files
                string filesListForMessage = "";
                string lastFilename = "";
                foreach (var container in mibFiles)
                {
                    var fullFilename = Path.Combine(dir, container.FileName);
                    lastFilename = fullFilename;
                    // create files
                    IbaSnmpMibGenerator.CreateFileFromString(container.Contents, fullFilename);
                    filesListForMessage += $"\r\n{fullFilename}";
                }

                if (MessageBox.Show(this, String.Format(Resources.snmpCreatedMibFiles, filesListForMessage),
                    Resources.snmpCreatedMibFilesTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.Yes)
                {
                    // open folder and show files
                    Process.Start(@"explorer.exe", $"/select, \"{lastFilename}\"");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        #endregion

    }
}
