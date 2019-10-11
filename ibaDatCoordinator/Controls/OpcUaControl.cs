using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using iba.Data;
using iba.Logging;
using iba.Processing;
using iba.Processing.IbaOpcUa;
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

            gbCertificates.Init();
            _ceManager.AddElement(gbCertificates);

            gbObjects.Init();
            _ceManager.AddElement(gbObjects);

            gbDiagnostics.Init();
            _ceManager.AddElement(gbDiagnostics);

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


        #region Configuration - General

        private void buttonConfigurationApply_Click(object sender, EventArgs e)
        {
            try
            {
                ConfigurationFromControlsToData();
                // set data to manager and restart server if necessary
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
            for (int i = 0; i < dgvEndpoints.RowCount; i++)
            {
                OpcUaData.OpcUaEndPoint ep = RowToEndpoint(dgvEndpoints.Rows[i]);
                _data.Endpoints.Add(ep);
            }

            // certificates
            _data.Certificates.Clear();
            for (int i = 0; i < dgvCertificates.RowCount; i++)
            {
                OpcUaData.CertificateTag certTag = RowToCertificate(dgvCertificates.Rows[i]);
                _data.AddCertificate(certTag);
            }
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
            dgvEndpoints.Rows.Clear();
            if (_data.Endpoints != null)
            {
                foreach (var ep in _data.Endpoints)
                    dgvEndpoints.Rows.Add(EndpointToRow(ep));
            }

            // disable/enable elements
            cbLogonUserName_CheckedChanged(null, null);
            cbSecurity128_CheckedChanged(null, null);
            cbSecurity256_CheckedChanged(null, null);
            dgvCertificates_SelectionChanged(null, null);

            // synchronize with OpcUaWorker
            var certs = TaskManager.Manager.OpcUaHandleCertificate("sync");

            // copy to controls
            RefreshCertificatesTable(certs);
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

        #region Configuration - Certificates

        private void RefreshCertificatesTable(List<OpcUaData.CertificateTag> certs)
        {
            _lastCertTableUpdateStamp = DateTime.Now;
            if (certs == null)
            {
                dgvCertificates.Rows.Clear();
                return;
            }

            dgvCertificates.RowCount = certs.Count;
            for (var i = 0; i < certs.Count; i++)
            {
                SetCertificateRow(dgvCertificates.Rows[i], certs[i]);
            }
        }

        private static OpcUaData.CertificateTag RowToCertificate(DataGridViewRow row)
        {
            try
            {
                Debug.Assert(row.Cells.Count == 5);
                OpcUaData.CertificateTag certTag = row.Cells[0].Value as OpcUaData.CertificateTag;

                return certTag;
            }
            catch
            {
                return null;
            }
        }

        //private static object[] CertificateToRow(OpcUaData.CertificateTag certTag)
        //{
        //    return new object[]
        //    {
        //        certTag, /*hidden column*/
        //        certTag.Name, certTag.GetPropertyString(), certTag.Issuer, certTag.ExpirationDate
        //    };
        //}

        private static void SetCertificateRow(DataGridViewRow row, OpcUaData.CertificateTag certTag)
        {
            Debug.Assert(row.Cells.Count == 5);

            row.Cells[0].Value = certTag; /*hidden column*/
            row.Cells[1].Value = certTag.Name;
            row.Cells[2].Value = certTag.GetPropertyString();
            row.Cells[3].Value = certTag.Issuer;
            row.Cells[4].Value = certTag.ExpirationDate;
        }

        private void buttonCertAdd_Click(object sender, EventArgs e)
        {
            if (openFileDialog.ShowDialog() != DialogResult.OK)
                return;

            var cert = new X509Certificate2();
            cert.Import(openFileDialog.FileName);
            OpcUaData.CertificateTag certTag = new OpcUaData.CertificateTag
            {
                Certificate = cert
            };

            var certs = TaskManager.Manager.OpcUaHandleCertificate("add", certTag);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertGenerate_Click(object sender, EventArgs e)
        {
            // ReSharper disable once UseObjectOrCollectionInitializer
            var certTag = new OpcUaData.CertificateTag();

            // todo. kls. Create a form to request cert parameters like in PDA
            // Name, Uri, Lifetime, Algorithm

            var certs = TaskManager.Manager.OpcUaHandleCertificate("generate", certTag);

            RefreshCertificatesTable(certs);

            // todo. kls. localize?
            if (MessageBox.Show(
                    "Do you want to use the generated certificate as OPC UA Server certificate", 
                    "Generate certificate",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                dgvCertificates.ClearSelection();
                dgvCertificates.Rows[dgvCertificates.RowCount - 1].Selected = true;
                buttonCertServer_Click(null, null);
            }

        }

        private void buttonCertExport_Click(object sender, EventArgs e)
        {
            var certTag = GetSelectedCertificate();
            if (certTag?.Certificate == null)
                return;

            if (saveFileDialog.ShowDialog() != DialogResult.OK)
                return;

            var fileName = saveFileDialog.FileName;
            var ext = Path.GetExtension(fileName);

            ext = string.IsNullOrWhiteSpace(ext) ? null : ext.ToLower();

            try
            {
                X509ContentType contentType;
                switch (ext)
                {
                    case ".der":
                    case ".cer":
                        // der and cer are actually the same
                        contentType = X509ContentType.Cert;
                        break;
                    case ".pfx":
                        contentType = X509ContentType.Pfx;
                        break;
                    default:
                        throw new Exception("Unsupported destination certificate format");
                }

                using (var file = File.Create(fileName))
                {
                    var bytes = certTag.Certificate.Export(contentType);
                    file.Write(bytes, 0, bytes.Length);
                }

                // todo. kls. localize?
                MessageBox.Show("Successfully exported certificate", "Export certificate", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch
            {
                // todo. kls. localize?
                MessageBox.Show("Error exporting certificate", "Export certificate",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void buttonCertRemove_Click(object sender, EventArgs e)
        {
            var certTag = GetSelectedCertificate();
            if (certTag == null)
                return;

            // todo. kls. low. localize?
            if (MessageBox.Show(
                    "Are you sure you want to delete the certificate permanently?", "",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Question) != DialogResult.OK)
                return;

            var certs = TaskManager.Manager.OpcUaHandleCertificate("remove", certTag);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertTrust_Click(object sender, EventArgs e)
        {
            var certTag = GetSelectedCertificate();
            if (certTag == null)
                return;
            var certs = TaskManager.Manager.OpcUaHandleCertificate("trust", certTag);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertReject_Click(object sender, EventArgs e)
        {
            var certTag = GetSelectedCertificate();
            if (certTag == null)
                return;
            var certs = TaskManager.Manager.OpcUaHandleCertificate("reject", certTag);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertUser_Click(object sender, EventArgs e)
        {
            var certTag = GetSelectedCertificate();
            if (certTag == null)
                return;
            var certs = TaskManager.Manager.OpcUaHandleCertificate("asUser", certTag);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertServer_Click(object sender, EventArgs e)
        {
            var certTag = GetSelectedCertificate();
            if (certTag == null)
                return;

            if (!certTag.HasPrivateKey)
            {
                // todo. kls. low. localize?
                MessageBox.Show(
                    "The selected certificate does not contain a private key", "", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var certs = TaskManager.Manager.OpcUaHandleCertificate("asServer", certTag);
            RefreshCertificatesTable(certs);
        }

        private OpcUaData.CertificateTag GetSelectedCertificate()
        {
            if (dgvCertificates.Rows.Count < 1 || dgvCertificates.SelectedRows.Count < 1)
                return null;

            var row = dgvCertificates.SelectedRows[0];
            return RowToCertificate(row);
        }

        private void dgvCertificates_SelectionChanged(object sender, EventArgs e)
        {
            buttonCertExport.Enabled = buttonCertRemove.Enabled =
                buttonCertTrust.Enabled = buttonCertReject.Enabled =
                    buttonCertUser.Enabled = buttonCertServer.Enabled =
                        dgvCertificates.SelectedRows.Count > 0;
        }

        #endregion


        #region Configuration - Endpoints

        // todo. kls. delete before last beta
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
            //DataGridViewRow row = view.Rows[e.RowIndex];
            //DataGridViewCell cell = view.Rows[e.RowIndex].Cells[e.ColumnIndex];

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

        private List<IbaOpcUaDiagClient> _diagClients;

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
                // clear diagnostics
                dgvSubscriptions.Rows.Clear();
                tbDiagTmp.Text = "";

                // show new data
                var diag = TaskManager.Manager.OpcUaGetDiagnostics();
                _diagClients = diag.Item1;
                var diagStr = diag.Item2;

                // can happen when suddenly disconnected
                if (_diagClients == null)
                {
                    dgvClients.Rows.Clear();
                    return;
                }

                // todo. kls. delete before last beta
                tbDiagTmp.Text = diagStr;

                // update rows
                dgvClients.RowCount = _diagClients.Count;
                for (var index = 0; index < _diagClients.Count; index++)
                {
                    UpdateClientRow(index, _diagClients[index]);
                }

                RefreshSubscriptionsTable();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(RefreshClientsTable)}. {ex.Message}");
            }
        }

        private void UpdateClientRow(int index, IbaOpcUaDiagClient client)
        {
            var cells = dgvClients.Rows[index].Cells;
            cells[0].Value = client.Name;
            cells[1].Value = client.Id;
            cells[2].Value = client.LastMessageTime;
        }

        private void dgvClients_SelectionChanged(object sender, EventArgs e)
        {
            RefreshSubscriptionsTable();
        }

        private void RefreshSubscriptionsTable()
        {
            // clear list
            dgvSubscriptions.Rows.Clear();

            if (_diagClients == null || dgvClients.RowCount == 0)
                return;

            // show for selected or first row
            DataGridViewRow row = dgvClients.SelectedRows.Count == 0 ? dgvClients.Rows[0] : dgvClients.SelectedRows[0];

            // get id
            if (!(row.Cells[1].Value is string idStr))
                return;

            foreach (var client in _diagClients)
            {
                if (client.Id != idStr)
                    continue;
                RefreshSubscriptionsTable(client);
                return;
            }
        }

        private void RefreshSubscriptionsTable(IbaOpcUaDiagClient client)
        {
            // clear list
            dgvSubscriptions.Rows.Clear();

            // can happen when suddenly disconnected
            if (client?.Subscriptions == null)
            {
                return;
            }

            foreach (var sub in client.Subscriptions)
            {
                dgvSubscriptions.Rows.Add(sub.Id, sub.MonitoredItemCount, sub.PublishingInterval, sub.NextSequenceNumber);
            }
        }

        private DateTime _lastCertTableUpdateStamp = DateTime.MinValue;
        private void timerRefreshStatus_Tick(object sender, EventArgs e)
        {
            bool isConnectedOrLocal = IsConnectedOrLocal;

            buttonConfigurationReset.Enabled = buttonConfigurationApply.Enabled =
                gbObjects.Enabled = gbDiagnostics.Enabled = gbCertificates.Enabled = isConnectedOrLocal;

            if (isConnectedOrLocal)
            {
                RefreshBriefStatus();
                RefreshClientsTable();

                // Re-read certificates.
                // if a new client tries to connect to our server
                // OPC UA SDK automatically adds its certificate to "rejected" store.
                // It is known on the level of the Server,
                // but in is not known on the level of the control.
                // Control should be informed somehow to reflect changes in the table here.
                // I use a timer here, because it is much simpler and more reliable than informing client via event.
                // This Timer is active ONLY if the OPC UA pane is visible, so it doesn't have any computational impact most of time.
                if (DateTime.Now - _lastCertTableUpdateStamp > TimeSpan.FromSeconds(3) /*not more often than once per 3 sec*/)
                {
                    var certs = TaskManager.Manager.OpcUaHandleCertificate("sync");
                    RefreshCertificatesTable(certs);
                }

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
            if (string.IsNullOrWhiteSpace(id))
                id = _recentId;

            if (string.IsNullOrWhiteSpace(id))
                return;

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
                tbObjValue.Text = "";
                tbObjType.Text = "";
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

        // todo. kls. delete before last beta
        private void buttonRebuildTree_Click(object sender, EventArgs e)
        {
            TaskManager.Manager.OpcUaRebuildObjectTree();
        }

        // todo. kls. delete before last beta
        private void buttonRefreshGuiTree_Click(object sender, EventArgs e)
        {
            RebuildObjectsTree();
        }


        #endregion
    
    }
}
