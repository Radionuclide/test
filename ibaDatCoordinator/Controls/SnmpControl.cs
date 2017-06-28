﻿using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using iba.Data;
using iba.Logging;
using iba.Processing;
using iba.Properties;
using iba.Utility;
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

        private CollapsibleElementManager _ceManager;

        private void SnmpControl_Load(object sender, EventArgs e)
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
            buttonShowEncryptionKey.Tag = tbEncryptionKey;

            // image list for objects TreeView
            ImageList tvObjectsImageList = new ImageList();
            // folder
            tvObjectsImageList.Images.Add(Resources.copydat_running); // todo Kls for Michael use another one
            // leaf
            tvObjectsImageList.Images.Add(Resources.batchfile_running); // todo Kls for Michael use another one
            tvObjects.ImageList = tvObjectsImageList;
            tvObjects.ImageIndex = ImageIndexFolder;
        }

        #endregion


        #region IPropertyPane Members

        private SnmpData _data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            _data = datasource as SnmpData; // clone of current Manager's data

            // if data is wrong, disable all controls, and cancel load
            Enabled = _data != null;
            if (_data == null)
            {
                return;
            }

            SnmpWorker snmpWorker = TaskManager.Manager?.SnmpWorker;
            if (snmpWorker != null)
            {
                // re-subscribe to status event
                // (probabaly now we are connected to another server)
                snmpWorker.StatusChanged -= SnmpWorker_StatusChanged;
                snmpWorker.StatusChanged += SnmpWorker_StatusChanged;
            }

            // if we have no snmp worker or IbaSnmp
            // then disable all controls and cancel load
            bool isServerAvailable = snmpWorker?.IbaSnmp != null;
            labelNotConnected.Visible = !isServerAvailable;
            gbConfiguration.Visible = 
                gbDiagnostics.Visible =
                gbObjects.Visible = isServerAvailable;
            if (!isServerAvailable)
            {
                return;
            }

            // read from data to controls
            try
            {
                ConfigurationFromDataToControls();
                
                // force rebuild snmpworker's tree to ensure we have most recent information
                snmpWorker.RebuildTreeCompletely();
                // rebuild gui-tree
                RebuildObjectsTree();
                snmpWorker.ApplyStatusToTextBox(tbStatus);

                // user has selected the control, 
                // enable clients monitoring
                timerRefreshClients.Enabled = true;
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
            timerRefreshClients.Enabled = false;
        }

        #endregion


        #region Configuration

        private void buttonConfigurationApply_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationFromControlsToData();
                // set data to manager and restart snmp agent if necessary
                TaskManager.Manager.SnmpData = _data.Clone() as SnmpData;

                // invalidate structure and rebuild the tree
                // because probabaly textual conventions were changed
                TaskManager.Manager.SnmpWorker.IsStructureValid = false;
                RebuildObjectsTree();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, @"SnmpControl.buttonConfigurationApply_Click() exception: " + ex.Message);
            }
        }

        private void buttonConfigurationReset_Click(object sender, EventArgs e)
        {
            // todo Kls localize
            if (MessageBox.Show(this,
                    "Are you sure you want to reset configuration to default?",
                    "Reset configuration?",
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

                // invalidate structure and rebuild the tree
                // because probabaly textual conventions were changed
                TaskManager.Manager.SnmpWorker.IsStructureValid = false;
                RebuildObjectsTree();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, @"SnmpControl.buttonConfigurationReset_Click() exception: " + ex.Message);
            }
        }

        private void ConfigurationFromControlsToData()
        {
            // general
            _data.Enabled = cbEnabled.Checked;
            _data.Port = (int)numPort.Value;
            // misc
            _data.UseSnmpV2TcForStrings = rbDateTimeTc.Checked;
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
        }

        private void ConfigurationFromDataToControls()
        {
            // general
            cbEnabled.Checked = _data.Enabled;
            numPort.Value = _data.Port;
            // misc
            rbDateTimeTc.Checked = _data.UseSnmpV2TcForStrings;
            rbDateTimeStr.Checked = !rbDateTimeTc.Checked;
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

        private void SnmpWorker_StatusChanged(object sender, SnmpWorkerStatusChangedEventArgs e)
        {
            if (tbStatus.InvokeRequired)
            {
                Invoke(new EventHandler<SnmpWorkerStatusChangedEventArgs>(SnmpWorker_StatusChanged), sender, e);
            }
            else
            {
                tbStatus.BackColor = e.Color;
                tbStatus.Text = e.Message;
            }
        }

        private void RefreshClientsTable()
        {
            // get library  
            IbaSnmp ibaSnmp = TaskManager.Manager?.SnmpWorker?.IbaSnmp;
            if (ibaSnmp == null)
            {
                return;
            }

            // clear list
            dgvClients.Rows.Clear();

            // show new data
            List<IbaSnmpDiagClient> clients = ibaSnmp.GetClients();
            foreach (var client in clients)
            {
                dgvClients.Rows.Add(client.Address, client.Version, client.MessageCount, client.LastMessageReceived);
            }
        }

        private void timerRefreshClients_Tick(object sender, EventArgs e)
        {
            RefreshClientsTable();
        }

        private void buttonClearClients_Click(object sender, EventArgs e)
        {
            // get library  
            IbaSnmp ibaSnmp = TaskManager.Manager?.SnmpWorker?.IbaSnmp;
            if (ibaSnmp == null)
            {
                return;
            }

            // reset monitoring list
            ibaSnmp.ClearClients();

            // refresh
            RefreshClientsTable();
        }

        #endregion


        #region Objects

        private IbaSnmpOid _oidGuiTreeRoot = "1.3.6.1";
        // todo Kls. whether we want to have mib2.system subtree to be shown in gui - set the value accordingly
        private readonly bool _whetherToShowMib2SystemItems = false;

        public void RebuildObjectsTree()
        {
            // first of all, clear the tree
            tvObjects.Nodes.Clear();

            // now add the new contents
            var worker = TaskManager.Manager?.SnmpWorker;

            IbaSnmp ibaSnmp = worker?.IbaSnmp;
            if (ibaSnmp == null)
            {
                return;
            }

            // the topmost possible node to be displayed in our tree
            if (!_whetherToShowMib2SystemItems)
            {
                // narrow down lookup area if mib2.system items are not needed
                _oidGuiTreeRoot = ibaSnmp.OidIbaRoot;
            }

            // update worker's tree if necessary
            worker.RebuildTreeIfItIsInvalid();

            if (Monitor.TryEnter(worker.LockObject, worker.LockTimeout))
            {
                try
                {
                    // create root nodes
                    if (_whetherToShowMib2SystemItems)
                    {
                        tvObjects.Nodes.Add(ibaSnmp.OidMib2System.ToString(), ibaSnmp.OidMib2System.ToString()).Tag = ibaSnmp.OidMib2System;
                    }
                    tvObjects.Nodes.Add(ibaSnmp.OidIbaRoot.ToString(), ibaSnmp.OidIbaRoot.ToString()).Tag = ibaSnmp.OidIbaRoot;

                    // dynamically create nodes for all the objects in ibaSnmp
                    var allObjs = ibaSnmp.GetListOfAllOids();
                    foreach (IbaSnmpOid oid in allObjs)
                    {
                        if (!oid.StartsWith(_oidGuiTreeRoot))
                        {
                            // ignore everything what is outside defined gui root area
                            continue;
                        }

                        // find or create a parent (folder) node
                        var parentNode = FindOrCreateFolderNode(worker, oid.GetParent());

                        string caption = $@"{oid.GetLeastId()}. {GetOidGuiCaption(worker, oid)}";

                        var node = parentNode.Nodes.Add(oid.ToString(), caption, ImageIndexLeaf, ImageIndexLeaf);
                        node.Tag = oid;
                    }
                }
                finally
                {
                    Monitor.Exit(worker.LockObject);
                }
            }
            else
            {
                // failed to acquire a lock
                try
                {
                    LogData.Data.Logger.Log(Level.Debug,
                        $"SNMP. Error acquiring lock when rebuilding TreeView, {SnmpWorker.GetCurrentThreadString()}");
                }
                catch
                {
                    // logging is not critical
                }
            }

            // expand some nodes
            FindSingleNodeByOid(worker, ibaSnmp.OidIbaRoot)?.Expand();
            FindSingleNodeByOid(worker, ibaSnmp.OidIbaProduct)?.Expand();
            FindSingleNodeByOid(worker, ibaSnmp.OidIbaProductSpecific)?.Expand();
            //FindSingleNodeByOid(worker, ibaSnmp.OidIbaProductSpecific + 1)?.Expand(); // global cleanup
            FindSingleNodeByOid(worker, ibaSnmp.OidIbaProductSpecific + 2)?.Expand(); // std job
            FindSingleNodeByOid(worker, ibaSnmp.OidIbaProductSpecific + 3)?.Expand(); // sch job
            FindSingleNodeByOid(worker, ibaSnmp.OidIbaProductSpecific + 4)?.Expand(); // one t job
            if (_whetherToShowMib2SystemItems)
            {
                // uncomment the following line to show mib2.sys items expanded
                // FindSingleNodeByOid(worker, ibaSnmp.OidMib2System)?.Expand();
            }

            // navigate to last selected oid if possible
            if (_lastOid == null)
            {
                return;
            }

            var parents = _lastOid.GetParents();
            foreach (IbaSnmpOid oid in parents)
            {
                try
                {
                    FindSingleNodeByOid(worker, oid)?.Expand();
                }
                catch
                {
                    // just go on with others
                }
            }

            tvObjects.SelectedNode = FindSingleNodeByOid(worker, _lastOid);

            //tvObjects.Select();
            tvObjects.Focus();
        }

        private TreeNode FindSingleNodeByOid(SnmpWorker worker, IbaSnmpOid oid)
        {
            if (worker == null)
            {
                // should not happen
                throw new ArgumentNullException(nameof(worker));
            }

            if (oid == null)
            {
                // should not happen
                throw new ArgumentNullException(nameof(oid));
            }

            if (!oid.StartsWith(_oidGuiTreeRoot))
            {
                // should not happen
                throw new ArgumentOutOfRangeException();
            }

            // check if exists
            TreeNode[] nodes = tvObjects.Nodes.Find(oid.ToString(), true);

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
                throw new Exception($@"Found more than one match for {oid}");
            }
            return null; // not found
        }

        private TreeNode FindOrCreateFolderNode(SnmpWorker worker, IbaSnmpOid oid)
        {
            if (worker == null)
            {
                // should not happen
                throw new ArgumentNullException(nameof(worker));
            }

            if (oid == null)
            {
                // should not happen
                throw new ArgumentNullException(nameof(oid));
            }

            //IbaSnmp ibaSnmp = worker.IbaSnmp;

            if (!oid.StartsWith(_oidGuiTreeRoot))
            {
                // should not happen
                throw new ArgumentOutOfRangeException();
            }

            // check if exists
            TreeNode node = FindSingleNodeByOid(worker, oid);

            if (node != null)
            {
                return node;
            }

            // not found, then create it

            // if this is a root node (recursion stop point)
            if (oid == _oidGuiTreeRoot)
            {
                    // then add it to the top of the tree
                    node = tvObjects.Nodes.Add(oid.ToString(), oid.ToString());
            }
            else
            {
                // first, find/create the parent node recursively
                var parentNode = FindOrCreateFolderNode(worker, oid.GetParent());
                // add the node to the parent node
                node = parentNode.Nodes.Add(oid.ToString(), $@"{oid.GetLeastId()}. {GetOidGuiCaption(worker, oid)}");
            }

            node.Tag = oid;
            return node;
        }

        private static string GetOidGuiCaption(SnmpWorker worker, IbaSnmpOid oid)
        {
            IbaSnmpOidMetadata metadata = worker?.IbaSnmp?.GetOidMetadata(oid);
            return metadata?.GuiCaption ?? String.Empty;
        }

        /// <summary> The last Oid that was selected by the user </summary>
        private IbaSnmpOid _lastOid;

        private void tvObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tbObjOid.Text = String.Empty;
            tbObjValue.Text = String.Empty;
            tbObjMibName.Text = String.Empty;
            tbObjType.Text = String.Empty;

            // reset last selected oid
            _lastOid = null;

            // get library  
            var worker = TaskManager.Manager?.SnmpWorker;
            if (worker == null)
            {
                return;
            }

            var od = worker.ObjectsData;
            IbaSnmp ibaSnmp = worker.IbaSnmp;
            if (od == null || ibaSnmp == null)
            {
                return;
            }

            // get OID from node's tag
            IbaSnmpOid oid = e.Node.Tag as IbaSnmpOid;
            if (oid == null)
            {
                return;
            }

            tbObjOid.Text = oid.ToString();

            var objInfo = ibaSnmp.GetObjectInfo(oid, true);

            // requesting some data from ibaSnmp can theoretically cause tree invalidation
            if (worker.RebuildTreeIfItIsInvalid())
            {
                // should not happen 
                // because cfg theoretically should not change while we are on the snmp page

                LogData.Data.Logger.Log(Level.Debug, @"SnmpControl.tvObjects_AfterSelect(). " +
                    "SNMP tree was changed since last data loading. This should not happen. Rebuilding the tree.");
                
                // rebuild our tree according to worker's tree
                RebuildObjectsTree();
                // do nothing else
                // let the user select another item in a new tree later
                return;
            }

            // remember last selected oid
            // do this only now, after possible call to RebuildObjectsTree() to prevent recursion
            _lastOid = oid;

            if (objInfo != null)
            {
                tbObjValue.Text =
                    ibaSnmp.IsEnumDataTypeRegistered(objInfo.ValueType) ?
                    // enum - format it like e.g. "1 (started)"
                    $@"{objInfo.Value} ({ibaSnmp.GetEnumValueName(objInfo.ValueType, (int)objInfo.Value)})" :
                    // other types - just value
                    objInfo.Value.ToString();

                tbObjMibName.Text = objInfo.MibName;

                // todo Kls remove after testing of MIB descriptions or move to another textbox
                if (!String.IsNullOrWhiteSpace(objInfo.MibDescription))
                {
                    tbObjMibName.Text += $@"; {objInfo.MibDescription}";
                }

                tbObjType.Text = objInfo.MibDataType;
            }
            else
            {
                // probabaly this is a folder, that has no corresponding snmp object 
                // try to get it's description from the worker.
                IbaSnmpOidMetadata metadata = worker.IbaSnmp?.GetOidMetadata(oid);
                tbObjMibName.Text = metadata?.MibName ?? String.Empty;
                // todo Kls remove after testing of MIB descriptions or move to another textbox
                if (!String.IsNullOrWhiteSpace(metadata?.MibDescription))
                {
                    tbObjMibName.Text += $@"; {metadata.MibDescription}";
                }
            }
        }

        private void buttonCreateMibFiles_Click(object sender, EventArgs e)
        {
            SnmpWorker snmpWorker = TaskManager.Manager?.SnmpWorker;
            var ibaSnmp = snmpWorker?.IbaSnmp;
            if (ibaSnmp == null)
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
                if (!snmpWorker.RebuildTreeCompletely())
                {
                    // failed to Rebuild the tree

                    // todo Kls localize
                    MessageBox.Show(this,
                        "Failed to create MIB files. \r\nFailed to rebuild SNMP tree.",
                        "Create MIB files failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                IbaSnmpMibGenerator gen = new IbaSnmpMibGenerator(ibaSnmp);

                gen.Generate();
                gen.SaveToFile(dir);

                string fullFileName1 = $@"{dir}\{gen.GeneralMibFilename}";
                string fullFileName2 = $@"{dir}\{gen.ProductMibFilename}";

                // todo Kls localize
                if (MessageBox.Show(this, 
                    "Successfully created the following MIB files:" +
                    $"\r\n{fullFileName1}\r\n{fullFileName2}"+
                    "\r\n\r\nDo you wish to navigate to these files?",
                    "Create MIB files", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
                    == DialogResult.Yes)
                {
                    // open folder and show files
                    Process.Start(@"explorer.exe", $"/select, \"{fullFileName2}\"");
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
