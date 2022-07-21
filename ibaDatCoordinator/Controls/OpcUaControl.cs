using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using iba.CertificateStore;
using iba.CertificateStore.Forms;
using iba.CertificateStore.Proxy;
using iba.Data;
using iba.Logging;
using iba.Processing;
using iba.Processing.IbaOpcUa;
using iba.Properties;
using iba.Utility;

namespace iba.Controls
{
    public partial class OpcUaControl : UserControl, IPropertyPane, ICertificatesControlHost
    {
        #region Construction, Destruction, Init

        public OpcUaControl()
        {
            InitializeComponent();
            InitializeIcons();

            // set up endpoints grid
            gridCtrlEndpoints.DataSource = _endpoints;
            var hostnameEditor = new HostnameEditor {GridView = gridViewEndpoints};
            DevExpress.XtraEditors.Repository.RepositoryItem item = hostnameEditor.Editor;
            item.Name = "HostnameEditor";
            item.AllowFocused = false;
            item.AutoHeight = false;
            item.BorderStyle = BorderStyles.NoBorder;
            gridCtrlEndpoints.RepositoryItems.Add(item);
            colHostname.ColumnEdit = item;

            // tune tbObjDescription.Height:
            // (if screen DPI is not 100%, then one-line textBoxes and multi-line textBox can be scaled differently;
            // we want to ensure that Description has the same height as Type+Value)
            tbObjDescription.Height = tbObjType.Bottom - tbObjValue.Top;

            serverCertCb = CertificatesComboBox.ReplaceCombobox(ServerCertPlaceholder, "", false);

#if DEBUG
            buttonShowHideDebug.Visible = true;
            gbDebug.Visible = true;
            gbDebug.Dock = DockStyle.Top;
#endif
        }

        private void InitializeIcons()
        {
            this.buttonEndpointRemove.Image = Icons.Gui.Standard.Images.Delete();
            this.buttonEndpointCopy.Image = Icons.Gui.All.Images.Copy();
            this.buttonEndpointAdd.Image = Icons.Gui.All.Images.PlusGreen();
        }

        private const int ImageIndexFolder = 0;
        private const int ImageIndexLeaf = 1;

        private readonly BindingList<OpcUaData.OpcUaEndPoint> _endpoints = new BindingList<OpcUaData.OpcUaEndPoint>();

        private void OpcUaControl_Load(object sender, EventArgs e)
        {
            // image list for objects TreeView
            ImageList tvObjectsImageList = new ImageList();
            // folder
            tvObjectsImageList.Images.Add(Icons.Gui.All.Images.FolderOpen());
            // leaf
            tvObjectsImageList.Images.Add(Icons.Gui.All.Images.TagBlueOutline());
            tvObjects.ImageList = tvObjectsImageList;
            tvObjects.ImageIndex = ImageIndexFolder;
        }

        #endregion

        #region ICertificatesControlHost

        CertificatesComboBox serverCertCb;
        CertificateInfo serverCertParams;
        IPropertyPaneManager m_manager;
        class CertificateInfo : ICertificateInfo
        {
            public string Thumbprint { get; set; }

            public CertificateRequirement CertificateRequirements { get; } =
                CertificateRequirement.Trusted |
                CertificateRequirement.PrivateKey;

            public string DisplayName { get; } = "Certificate for OPC UA server";
        }

        public bool IsLocalHost { get; }
        public string ServerAddress { get; }
        public ICertificateManagerProxy CertificateManagerProxy { get; } = new CertificateManagerProxyJsonAdapter(new AppCertificateManagerJsonProxy());
        public bool IsCertificatesReadonly => false;
        public bool IsReadOnly => false; // set to true in case of user restriction
        public string UsagePart { get; } = "EX"; // IO and DS are used in PDA
        public IWin32Window Instance => this;
        public ContextMenuStrip PopupMenu { get; } = new ContextMenuStrip(); // or reuse the context menu of an other control
        public List<int> ServerUsageIds => new List<int>();

        public void OnSaveDataSource()
        {
            throw new NotImplementedException();
        }

        public ICertifiable GetCertifiableRootNode()
        {
            throw new NotImplementedException();
        }

        public void ManageCertificates()
        {
            (m_manager as MainForm)?.MoveToSettigsTab();
        }

        public void JumpToCertificateInfoNode(string displayName)
        {
            throw new NotImplementedException();
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
            m_manager = manager;
            serverCertParams = new CertificateInfo();
            serverCertParams.Thumbprint = _data.serverSertificateThumbprint;
            serverCertCb.UnsetEnvironment();
            serverCertCb.SetEnvironment(this, serverCertParams);
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


        #region Configuration - General

        private void tabControl1_SelectionChanged(Crownwood.DotNetMagic.Controls.TabControl sender, Crownwood.DotNetMagic.Controls.TabPage oldPage, Crownwood.DotNetMagic.Controls.TabPage newPage)
        {
            // show panel with Apply and Reset buttons only for Cfg tabs
            panelFooterWithButtons.Visible = tabConfiguration.Selected;
            // show Reset only for Cfg tab
            buttonConfigurationReset.Visible = tabConfiguration.Selected;
        }

        private void buttonConfigurationApply_Click(object sender, EventArgs e)
        {
            LongActionInProgress = true; // indicate long action is started

            try
            {
                using (new WaitCursor())
                {
                    ConfigurationFromControlsToData();
                    // set data to manager and restart server if necessary
                    TaskManager.Manager.OpcUaData = _data.Clone() as OpcUaData;

                    // rebuild GUI tree
                    RebuildObjectsTree();
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception,
                    $@"{nameof(OpcUaControl)}.{nameof(buttonConfigurationApply_Click)}. {ex.Message}");
            }
            LongActionInProgress = false; // indicate long action is finished
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
            
            // show default data in controls
            ConfigurationFromDataToControls();
            tabConfiguration.Update();

            // apply new configuration - do the same like for usual apply
            buttonConfigurationApply_Click(null, null);
        }

        private bool LongActionInProgress
        {
            set
            {
                tbStatus.Enabled = buttonConfigurationApply.Enabled = buttonConfigurationReset.Enabled = !value;
                if (value)
                {
                    // gray-out status line before long action to indicate that we are busy
                    tbStatus.Text = "";
                    tbStatus.BackColor = BackColor;
                    tbStatus.Update();
                }
                else
                {
                    // refresh status line and other things after long action
                    timerRefreshStatus_Tick(null, null);
                }
            }
        }

        private void ConfigurationFromControlsToData()
        {
            // general
            _data.Enabled = cbEnabled.Checked;

            // logon
            _data.IsAnonymousUserAllowed = cbLogonAnonymous.Checked;
            _data.IsNamedUserAllowed = cbLogonUserName.Checked;
            _data.IsCertifiedUserAllowed = cbLogonCertificate.Checked;
            _data.UserName = tbUserName.Text;
            _data.Password = tbPassword.Text;

            // security policies
            _data.HasSecurityNone = cbSecurityNone.Checked;
            _data.HasSecurityBasic128 = cbSecurity128.Checked;
            _data.HasSecurityBasic256 = cbSecurity256.Checked;
            _data.HasSecurityBasic256Sha256 = cbSecurity256Sha256.Checked;

            _data.SecurityBasic128Mode = (OpcUaData.OpcUaSecurityMode)comboBoxSecurity128.SelectedIndex;
            _data.SecurityBasic256Mode = (OpcUaData.OpcUaSecurityMode)comboBoxSecurity256.SelectedIndex;
            _data.SecurityBasic256Sha256Mode = (OpcUaData.OpcUaSecurityMode)comboBoxSecurity256Sha256.SelectedIndex;

            // endpoints
            _data.Endpoints.Clear();
            foreach (var ep in _endpoints)
                _data.Endpoints.Add(new OpcUaData.OpcUaEndPoint(ep));

            _data.serverSertificateThumbprint = serverCertParams.Thumbprint;
        }

        private void ConfigurationFromDataToControls()
        {
            // general
            cbEnabled.Checked = _data.Enabled;

            // logon
            cbLogonAnonymous.Checked = _data.IsAnonymousUserAllowed;
            cbLogonUserName.Checked = _data.IsNamedUserAllowed;
            cbLogonCertificate.Checked = _data.IsCertifiedUserAllowed;
            tbUserName.Text = _data.UserName;
            tbPassword.Text = _data.Password;

            // security policies
            cbSecurityNone.Checked = _data.HasSecurityNone;
            cbSecurity128.Checked = _data.HasSecurityBasic128;
            cbSecurity256.Checked = _data.HasSecurityBasic256;
            cbSecurity256Sha256.Checked = _data.HasSecurityBasic256Sha256;

            comboBoxSecurity128.SelectedIndex = (int)_data.SecurityBasic128Mode;
            comboBoxSecurity256.SelectedIndex = (int)_data.SecurityBasic256Mode;
            comboBoxSecurity256Sha256.SelectedIndex = (int)_data.SecurityBasic256Sha256Mode;

            // endpoints
            _endpoints.Clear();
            foreach (var ep in _data.Endpoints)
                _endpoints.Add(new OpcUaData.OpcUaEndPoint(ep));
            ApplyEndpointsButtonsEnabling();

            // disable/enable elements
            cbLogonUserName_CheckedChanged(null, null);
            cbSecurity128_CheckedChanged(null, null);
            cbSecurity256_CheckedChanged(null, null);
            cbSecurity256Sha256_CheckedChanged(null, null);
        }

        private void cbSecurity128_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxSecurity128.Enabled = cbSecurity128.Checked;
        }

        private void cbSecurity256_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxSecurity256.Enabled = cbSecurity256.Checked;
        }

        private void cbSecurity256Sha256_CheckedChanged(object sender, EventArgs e)
        {
            comboBoxSecurity256Sha256.Enabled = cbSecurity256Sha256.Checked;
        }

        private void cbLogonUserName_CheckedChanged(object sender, EventArgs e)
        {
            tbUserName.Enabled = tbPassword.Enabled = cbLogonUserName.Checked;
        }
        
        
        #endregion


        #region Configuration - Endpoints

        private class HostnameEditor
        {
            public DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit Editor { get; }

            public GridView GridView;

            public HostnameEditor()
            {
                Editor = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit { Name = "HostnameEditor" };
                Editor.Buttons.Clear();
                Editor.ButtonsStyle = BorderStyles.UltraFlat;
                Editor.Buttons.Add(new EditorButton(ButtonPredefines.Ellipsis));

                Editor.ButtonClick += Edit_ButtonClick;
            }

            private void Edit_ButtonClick(object sender, ButtonPressedEventArgs e)
            {
                DevExpress.XtraEditors.ButtonEdit btnEdit = sender as DevExpress.XtraEditors.ButtonEdit;
                if (btnEdit == null)
                    return;
                Control parent = btnEdit.Parent;
                while (parent?.Parent != null)
                    parent = parent.Parent;

                var netwConf = TaskManager.Manager.OpcUaGetNetworkConfiguration();
                if (netwConf == null)
                {
                    MessageBox.Show(parent, Resources.opcUaErrorTryingToGetNetworkConfiguration, Resources.opcUaTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (OpcUaEndpointSelectionForm selForm = new OpcUaEndpointSelectionForm(netwConf))
                {
                    if (!(GridView.GetRow(GridView.FocusedRowHandle) is OpcUaData.OpcUaEndPoint curEp))
                        return;

                    // Save current value
                    curEp.Hostname = btnEdit.Text;

                    selForm.IpAddress = curEp.Hostname;
                    if (DialogResult.OK == selForm.ShowDialog(parent))
                    {
                        btnEdit.EditValue = selForm.IpAddress;
                        btnEdit.SelectAll();

                        curEp.Hostname = selForm.IpAddress;

                        GridView.CloseEditor();
                    }
                }
            }
        }

        private void buttonEndpointAdd_Click(object sender, EventArgs e)
        {
            var dep = OpcUaData.DefaultEndPoint;

            _endpoints.Add(dep);
            gridViewEndpoints.FocusedRowHandle = _endpoints.Count - 1;
            gridViewEndpoints.FocusedColumn = colHostname;
            gridViewEndpoints.ShowEditor();

            ApplyEndpointsButtonsEnabling();
        }

        private void buttonEndpointCopy_Click(object sender, EventArgs e)
        {
            if ((gridViewEndpoints.FocusedRowHandle >= 0) &&
                (gridViewEndpoints.FocusedRowHandle < _endpoints.Count) &&
                gridViewEndpoints.GetRow(gridViewEndpoints.FocusedRowHandle) is OpcUaData.OpcUaEndPoint selEp)
            {
                _endpoints.Add(new OpcUaData.OpcUaEndPoint(selEp));
                gridViewEndpoints.FocusedRowHandle = _endpoints.Count - 1;
                gridViewEndpoints.FocusedColumn = colHostname;
                gridViewEndpoints.ShowEditor();

                ApplyEndpointsButtonsEnabling();
            }
        }

        private void buttonEndpointRemove_Click(object sender, EventArgs e)
        {
            if ((gridViewEndpoints.FocusedRowHandle >= 0) && (gridViewEndpoints.FocusedRowHandle < _endpoints.Count))
            {
                _endpoints.RemoveAt(gridViewEndpoints.FocusedRowHandle);
                ApplyEndpointsButtonsEnabling();
            }
        }

        private void ApplyEndpointsButtonsEnabling()
        {
            buttonEndpointCopy.Enabled =
                buttonEndpointRemove.Enabled = (gridViewEndpoints.FocusedRowHandle >= 0) && (_endpoints.Count > 0);
        }

        #endregion

        #region Diagnostics

        private List<IbaOpcUaDiagClient> _diagClients;

        private void RefreshBriefStatus()
        {
            try
            {
                var status = TaskManager.Manager.OpcUaGetBriefStatus();
                tbStatus.Text = status.Item2;
                tbStatus.BackColor = SnmpControl.StatusToColor(status.Item1);
#if DEBUG
         tbDiagTmp.BackColor = SnmpControl.StatusToColor(status.Item1);
#endif
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
                // remember selected line and scroll position
                var selectedLines = gridViewSessions.GetSelectedRows();
                int scrollPosition = gridViewSessions.TopRowIndex;

                // clear old diagnostics
                gridCtrlSessions.DataSource = null;
                gridCtrlSubscriptions.DataSource = null;
                tbDiagTmp.Text = "";

                // get and show new data
                var diag = TaskManager.Manager.OpcUaGetDiagnostics();
                _diagClients = diag.Item1;
                var diagStr = diag.Item2;

                gridCtrlSessions.DataSource = _diagClients;

                // can happen when suddenly disconnected
                if (_diagClients == null)
                    return;
                
                tbDiagTmp.Text = diagStr;
                
                // restore selected line and scroll position
                if (selectedLines.Length > 0)
                    gridViewSessions.FocusedRowHandle = selectedLines[0];
                gridViewSessions.TopRowIndex = scrollPosition;

                RefreshSubscriptionsTable();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(RefreshClientsTable)}. {ex.Message}");
            }
        }
        
        private void RefreshSubscriptionsTable()
        {
            // clear list
            gridCtrlSubscriptions.DataSource = null;

            if (_diagClients == null)
                return;
            
            // get selected session row
            var selectedLines = gridViewSessions.GetSelectedRows();
            int ind = selectedLines.Length > 0 ? selectedLines[0] : -1;

            // show selected session details
            if (ind >= 0 && ind < _diagClients.Count)
                gridCtrlSubscriptions.DataSource = _diagClients[ind].Subscriptions;
        }

        private void gridViewSessions_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            RefreshSubscriptionsTable();
        }

        private void gridViewSessions_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {

        }

        private void timerRefreshStatus_Tick(object sender, EventArgs e)
        {
            bool isConnectedOrLocal = IsConnectedOrLocal;

            buttonConfigurationReset.Enabled = buttonConfigurationApply.Enabled =
                tabTags.Enabled = tabDiag.Enabled = isConnectedOrLocal;

            if (isConnectedOrLocal)
            {
                RefreshBriefStatus();
                RefreshClientsTable();

                // update currently selected node information in object tree
                if (tabTags.Selected)
                {
                    UpdateObjectTreeNodeDescription();
                }
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

        private void btOpenLogFile_Click(object sender, EventArgs e)
        {
            var networkCfg = TaskManager.Manager.OpcUaGetNetworkConfiguration();
            if (networkCfg == null || string.IsNullOrWhiteSpace(networkCfg.Hostname) ||
                string.IsNullOrWhiteSpace(networkCfg.UaTraceFilePath))
            {
                // should not happen
                MessageBox.Show(Resources.opcUaErrorTryingToGetNetworkConfiguration, 
                    Resources.opcUaTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Debug.Assert(false);
                return;
            }

            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE ||
                (string.Compare(networkCfg.Hostname, Dns.GetHostName(), StringComparison.OrdinalIgnoreCase) == 0))
            {
                // is standalone or client is running on the same machine as server
                Process.Start($"\"{networkCfg.UaTraceFilePath}\"");
            }
            else
            {
                // client is running on a different machine
                MessageBox.Show(
                    string.Format(Resources.opcUaFileLocatedOnRemoteMachine,
                        networkCfg.Hostname, networkCfg.UaTraceFilePath),
                    Resources.opcUaTitle,
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
                    // can happen if OpcUaWorker is not initialized
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

                    string parentId = IbaOpcUaNodeManager.GetParentName(tag.OpcUaNodeId);
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

        private void UpdateObjectTreeNodeDescription(string id = null)
        {
            // if no argument then try to use the recent one
            if (string.IsNullOrWhiteSpace(id))
            {
                id = _recentId;
            }

            // if there is no recent one, then return and do nothing
            if (string.IsNullOrWhiteSpace(id))
            {
                return;
            }

            // try to refresh node's tag
            try
            {
                ExtMonData.GuiTreeNodeTag tag = TaskManager.Manager.OpcUaGetTreeNodeTag(id);
                tbObjValue.Text = tag.Value;
                tbObjType.Text = tag.Type;
                tbObjNodeId.Text = tag.OpcUaNodeId;
                tbObjDescription.Text = tag.Description;
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

                UpdateObjectTreeNodeDescription(tag.OpcUaNodeId);

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


        #region Tmp and Debug

        /// <summary>
        /// This handler is used only for debug purposes.
        /// Normally rebuilding is handled automatically.
        /// </summary>
        private void buttonRebuildTree_Click(object sender, EventArgs e)
        {
            TaskManager.Manager.OpcUaRebuildObjectTree();
        }

        /// <summary>
        /// This handler is used only for debug purposes.
        /// Normally GUI tree refresh is handled automatically.
        /// </summary>
        private void buttonRefreshGuiTree_Click(object sender, EventArgs e)
        {
            RebuildObjectsTree();
        }

        private void buttonHide_Click(object sender, EventArgs e)
        {
            gbDebug.Visible ^= true;
        }

        #endregion

    }
}
