using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Net.Sockets;
using System.Windows.Forms;
using iba.Data;
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
            gbDebug.Init();
            gbConfiguration.Init();
            gbDiagnostics.Init();
            gbObjects.Init();

            SnmpWorker snmpWorker = TaskManager.Manager?.SnmpWorker;
            if (snmpWorker != null)
            {
                snmpWorker.StatusChanged += SnmpWorker_StatusChanged;
            }

            // image list for objects TreeView
            ImageList tvObjectsImageList = new ImageList();
            // folder
            tvObjectsImageList.Images.Add(Resources.copydat_running); // todo use another one
            // leaf
            tvObjectsImageList.Images.Add(Resources.batchfile_running); // todo use another one
            tvObjects.ImageList = tvObjectsImageList;
            tvObjects.ImageIndex = ImageIndexFolder;

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

        private int _tmpCntDataLoaded;
        private int _tmpCntDataCleaned;
        private int _tmpCndDataSaved;
        private int _tmpCntTimerTicks;

        private void buttonStart_Click(object sender, EventArgs e)
        {
            try
            {
                IbaSnmp ibaSnmp = TaskManager.Manager?.SnmpWorker?.IbaSnmp;
                ibaSnmp?.Start();
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
                IbaSnmp ibaSnmp = TaskManager.Manager?.SnmpWorker?.IbaSnmp;
                ibaSnmp?.Stop();
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

        private void buttonDebugRefresh_Click(object sender, EventArgs e)
        {
            var man = TaskManager.Manager;
            SnmpWorker snmpWorker = man?.SnmpWorker;

            snmpWorker?.CheckSnmpTreeStructure();
        }

        private void buttonImitateCfgInvalidated_Click(object sender, EventArgs e)
        {
            TaskManager.Manager.SnmpWorker.TaskManager_SnmpConfigurationChanged(null, null);

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

            // let the manager know that GUI is visible
            // so GUI-specific things can be started suspended
            SnmpWorker snmpWorker = TaskManager.Manager?.SnmpWorker;
            if (snmpWorker == null)
            {
                return;
            }

            // read from data to controls
            try
            {
                ConfigurationFromDataToControls();
                InitializeObjectsTree();
                snmpWorker.ApplyStatusToTextBox(tbStatus);
            }
            catch (Exception ex)
            {
                // todo details
                MessageBox.Show(ex.ToString());
            }

            timerStatus.Enabled = true;

            _tmpCntDataLoaded++;
            label1.Text = $@"Data Loaded {_tmpCntDataLoaded}";
            SnmpWorker.TmpLogLine($@"SnmpCtrl. Data Loaded { _tmpCntDataLoaded}");
        }

        public void SaveData()
        {
            TaskManager.Manager.SnmpData = _data.Clone() as SnmpData;

            // todo ask Michael. Save = Load * 2. why?
            _tmpCndDataSaved++;
            label3.Text = $@"Data Saved {_tmpCndDataSaved}";
            SnmpWorker.TmpLogLine($@"SnmpCtrl. Data Saved { _tmpCndDataSaved}");
        }

        public void LeaveCleanup()
        {
            // let the manager know that GUI is not visible
            // so GUI-specific things can be suspended
            SnmpWorker snmpWorker = TaskManager.Manager?.SnmpWorker;
            if (snmpWorker == null)
            {
                return;
            }
            //snmpWorker.IsGuiVisible = false;
            timerStatus.Enabled = false;

            _tmpCntDataCleaned++;
            label2.Text = $@"Data cleaned {_tmpCntDataCleaned}";
            SnmpWorker.TmpLogLine($@"SnmpCtrl. Data cleaned { _tmpCntDataCleaned}");
        }

        #endregion


        #region Configuration

        private void buttonConfigurationApply_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationFromControlsToData();
                // set data to manager and restart snmp agent if necessary
                ApplyConfigurationToManager();
            }
            catch (Exception ex)
            {
                // todo details
                MessageBox.Show(ex.ToString());
            }
        }

        private void buttonConfigurationReset_Click(object sender, EventArgs e)
        {
            // todo localize
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
            _data.Port = defData.Port;
            _data.V1V2Security = defData.V1V2Security;
            _data.V3Security = defData.V3Security;
            // do not reset enabled/disabled : _data.Enabled = ...

            try
            {
                ConfigurationFromDataToControls();
                ApplyConfigurationToManager();
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

        // todo rename and move if it will concern objects also
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

        #endregion


        #region Diagnostics

        private void timerStatus_Tick(object sender, EventArgs e)
        {
            _tmpCntTimerTicks++;

            label4.Text = (_tmpCntTimerTicks % 2 == 0 ? "|" : "-");

            IbaSnmp ibaSnmp = TaskManager.Manager?.SnmpWorker?.IbaSnmp;

            string str = "";
            str += ibaSnmp == null
                ? @"ibaSnmp == null"
                : _tmp___GetLibraryDescriptionString(ibaSnmp);

            tbDebug.Text = str;
        }
        
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

        #endregion


        #region Objects


        public void InitializeObjectsTree()
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

            // uppdate tree if necessary
            worker.CheckSnmpTreeStructure();

            lock (worker.LockObject)
            {
                var allObjs = ibaSnmp.GetListOfAllOids();
                foreach (IbaSnmpOid oid in allObjs)
                {
                    if (!oid.StartsWith(ibaSnmp.OidIbaRoot))
                    {
                        // ignore everything what is outside iba root area
                        // (e.g. UpTime or whataver)
                        continue;
                    }

                    // find or create a parent (folder) node
                    var parentNode = FindOrCreateFolderNode(worker, oid.GetParent());

                    string caption = $@"{oid.GetLeastId()}. {GetOidGuiCaption(worker, oid)}";

                    var node = parentNode.Nodes.Add(oid.ToString(), caption, ImageIndexLeaf, ImageIndexLeaf);
                    node.Tag = oid;
                }

                FindSingleNodeByOid(worker, ibaSnmp.OidIbaRoot)?.Expand();
                FindSingleNodeByOid(worker, ibaSnmp.OidIbaProduct)?.Expand();
                FindSingleNodeByOid(worker, ibaSnmp.OidIbaProductSpecific)?.Expand();
                FindSingleNodeByOid(worker, ibaSnmp.OidIbaProductSpecific + 1)?.Expand(); // global cleanup
                FindSingleNodeByOid(worker, ibaSnmp.OidIbaProductSpecific + 2)?.Expand(); // std job
                FindSingleNodeByOid(worker, ibaSnmp.OidIbaProductSpecific + 3)?.Expand(); // sch job
                FindSingleNodeByOid(worker, ibaSnmp.OidIbaProductSpecific + 4)?.Expand(); // one t job
            }
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

            IbaSnmp ibaSnmp = worker.IbaSnmp;

            if (!oid.StartsWith(ibaSnmp.OidIbaRoot))
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

            IbaSnmp ibaSnmp = worker.IbaSnmp;

            if (!oid.StartsWith(ibaSnmp.OidIbaRoot))
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
            if (oid == ibaSnmp.OidIbaRoot)
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
            lock (worker.LockObject)
            {
                SnmpWorker.OidMetadata metadata;
                if (!worker.OidMetadataDict.TryGetValue(oid, out metadata))
                {
                    return @"???";
                }
                return metadata.GuiCaption;
            }
        }

        private void tvObjects_AfterSelect(object sender, TreeViewEventArgs e)
        {
            tbObjOid.Text = "";
            tbObjValue.Text = "";
            tbObjMibName.Text = "";
            tbObjType.Text = "";

            // get library  
            var worker = TaskManager.Manager?.SnmpWorker;
            if (worker == null)
            {
                return;
            }

            worker.CheckSnmpTreeStructure();

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

            if (objInfo != null)
            {
                tbObjValue.Text = objInfo.Value.ToString();
                tbObjMibName.Text = objInfo.MibName;
                
                // todo remove after testing of MIB descriptions
                tbObjMibName.Text = objInfo.MibName + @"; " + objInfo.MibDescription; 

                tbObjType.Text = objInfo.MibDataType;
            }
            else
            {
                // todo remove this block after testing of MIB descriptions
                {
                    // probabaly this is a folder, that has no corresponding snmp object 
                    // try to get it's description from the worker.
                    SnmpWorker.OidMetadata metadata;
                    lock (worker.LockObject)
                    {
                        if (worker.OidMetadataDict.TryGetValue(oid, out metadata) != true)
                        {
                            return;
                        }
                    }

                    tbObjMibName.Text = (metadata.MibName ?? "") + @"; " + (metadata.MibDescription ?? "");
                }
            }
        }

        private void buttonObjectsRefresh_Click(object sender, EventArgs e)
        {
            InitializeObjectsTree();
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

                IbaSnmpMibGenerator gen = new IbaSnmpMibGenerator(ibaSnmp);

                gen.DescriptionRequested += MibGenerator_OnDescriptionRequested;
                gen.Generate();
                gen.SaveToFile(dir);

                string fullFileName1 = $@"{dir}\{gen.GeneralMibFilename}";
                string fullFileName2 = $@"{dir}\{gen.ProductMibFilename}";

                // todo localize
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

        private static void MibGenerator_OnDescriptionRequested(object sender, IbaSnmpMibDescrRequestedEventArgs eventArgs)
        {
            var worker = TaskManager.Manager?.SnmpWorker;

            if (worker == null)
            {
                return;
            }

            SnmpWorker.OidMetadata metadata;
            lock (worker.LockObject)
            {
                if (worker.OidMetadataDict.TryGetValue(eventArgs.Oid, out metadata) != true)
                {
                    return;
                }
            }

            if (metadata.MibName != null)
            {
                eventArgs.Name = metadata.MibName;
            }
            if (metadata.MibDescription != null)
            {
                eventArgs.Name = metadata.MibDescription;
            }
        }


        #endregion


    }
}
