using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
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

            // Load images for certificate properties
            imgTrusted = Resources.img_shldgreen;
            imgRejected = Resources.img_shldred;
            imgKey = Resources.img_key;
            imgUaServer = Resources.opcUaServer_icon;
            imgDude = Resources.img_dude;
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

            // show default cert columns
            miColumnReset_Click(null, null);

            // prevent Columns submenu from closing on item click
            miColumns.DropDown.Closing +=
                (o, args) => args.Cancel = (args.CloseReason == ToolStripDropDownCloseReason.ItemClicked);

            // set up Endpoints DataGridView
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
            if (CertificatesDataSource != null)
            {
                _data.Certificates.AddRange(CertificatesDataSource);
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

            // synchronize with OpcUaWorker
            var certs = TaskManager.Manager.OpcUaHandleCertificate("forceSync");

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

        private readonly Image imgTrusted;
        private readonly Image imgRejected;
        private readonly Image imgKey;
        private readonly Image imgUaServer;
        private readonly Image imgDude;
        private readonly StringFormat textAlign = new StringFormat {Alignment = StringAlignment.Center};
        private const int ImgDimension = 16;
        private const int ImgGapPix = 4;

        private int toolTipLastRowHandle = -1;
        /// <summary> Stores cached info about currently pointed item; is needed to decrease computational load. </summary>
        private ToolTipControlInfo lastTooltip;

        private void RefreshCertificatesTable(List<OpcUaData.CertificateTag> certs)
        {
            _lastCertTableUpdateStamp = DateTime.Now;

            // remember selected line and scroll position
            var selectedLine = SelectedCertificate;
            var scrollPosition = gridViewCerts.TopRowIndex;

            CertificatesDataSource = certs;

            // restore selected line and scroll position
            SelectedCertificate = selectedLine;
            gridViewCerts.TopRowIndex = scrollPosition;
        }


        #region cert buttons

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
                // get last row
                var lastRowHandle = gridViewCerts.GetRowHandle(certs.Count - 1);
                SelectedCertificate = (gridViewCerts.GetRow(lastRowHandle) as OpcUaData.CertificateTag);
                buttonCertServer_Click(null, null);
            }
        }

        private void buttonCertExport_Click(object sender, EventArgs e)
        {
            var certTag = SelectedCertificate;
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
            var certTag = SelectedCertificate;
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
            var certTag = SelectedCertificate;
            if (certTag == null)
                return;
            var certs = TaskManager.Manager.OpcUaHandleCertificate("trust", certTag);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertReject_Click(object sender, EventArgs e)
        {
            var certTag = SelectedCertificate;
            if (certTag == null)
                return;
            var certs = TaskManager.Manager.OpcUaHandleCertificate("reject", certTag);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertUser_Click(object sender, EventArgs e)
        {
            var certTag = SelectedCertificate;
            if (certTag == null)
                return;
            var certs = TaskManager.Manager.OpcUaHandleCertificate("asUser", certTag);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertServer_Click(object sender, EventArgs e)
        {
            var certTag = SelectedCertificate;
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

        private void buttonCertRefresh_Click(object sender, EventArgs e)
        {
            var certs = TaskManager.Manager.OpcUaHandleCertificate("forceSync", null);
            RefreshCertificatesTable(certs);
        }

        #endregion

        /// <summary> Typed wrapper of <see cref="gridControlCerts"/> DataSource </summary>
        private List<OpcUaData.CertificateTag> CertificatesDataSource
        {
            get => gridControlCerts.DataSource as List<OpcUaData.CertificateTag>;
            set
            {
                gridControlCerts.DataSource = value;
                gridControlCerts.RefreshDataSource();
                toolTipLastRowHandle = -1;
            }
        }

        public OpcUaData.CertificateTag SelectedCertificate
        {
            get
            {
                // Get selected row
                int[] rowInd = gridViewCerts.GetSelectedRows();
                if ((rowInd != null) && (rowInd.Length > 0) && (rowInd[0] != -1))
                    return gridViewCerts.GetRow(rowInd[0]) as OpcUaData.CertificateTag;

                return null;
            }
            set
            {
                gridViewCerts.FocusedRowHandle = -1;
                if (value == null || CertificatesDataSource == null)
                    return;

                gridViewCerts.FocusedRowHandle = CertificatesDataSource.FindIndex(el => el.Thumbprint == value.Thumbprint);
            }
        }

        private void gridViewCerts_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            if (!(sender is GridView view))
                return;

            if (!(view.GetRow(e.RowHandle) is OpcUaData.CertificateTag certTag))
                return;

            X509Certificate2 cert = certTag.Certificate;
            if (cert == null)
                return;

            // highlight current server certificate with blue
            var foreColor = certTag.IsUsedForServer ? Color.Blue : SystemColors.WindowText;
            e.Appearance.ForeColor = foreColor;

            e.Appearance.TextOptions.HAlignment = HorzAlignment.Center;

            if (e.Column == colCertName)
            {
                e.DisplayText = GetCertificateAttribute(cert.Subject, "CN=");
            }
            else if (e.Column == colCertOrganization)
            {
                e.DisplayText = GetCertificateAttribute(cert.Subject, "O=");
            }
            else if (e.Column == colCertLocality)
            {
                e.DisplayText = GetCertificateAttribute(cert.Subject, "L=");
            }
            else if (e.Column == colCertState)
            {
                e.DisplayText = GetCertificateAttribute(cert.Subject, "S=");
            }
            else if (e.Column == colCertCountry)
            {
                e.DisplayText = GetCertificateAttribute(cert.Subject, "C=");
            }
            else if (e.Column == colCertIssuedBy)
            {
                e.DisplayText = GetCertificateAttribute(cert.Issuer, "CN=");
            }
            else if (e.Column == colCertIssuingDate)
            {
                e.DisplayText = cert.NotBefore.ToString();
            }
            else if (e.Column == colCertExpirationDate)
            {
                e.DisplayText = cert.NotAfter.ToString();
            }
            else if (e.Column == colCertAlgorithm)
            {
                e.DisplayText = cert.SignatureAlgorithm.FriendlyName;
            }
            else if (e.Column == colCertThumbprint)
            {
                e.DisplayText = certTag.Thumbprint;
            }
            else if (e.Column == colCertProperties)
            {
                int actIcons = 1;
                int maxIcons = (e.Bounds.Width + ImgGapPix) / (ImgDimension + ImgGapPix);
                // 4 is maximum number of icons we want to display (Tr/Rej, Key, Auth, Srv)
                maxIcons = Math.Min(maxIcons, 4);

                if (certTag.HasPrivateKey)
                    actIcons = 2;

                if (certTag.IsUsedForServer)
                    actIcons = 3;

                if (certTag.IsUsedForAuthentication)
                    actIcons = 4;

                int totalWidth = maxIcons * ImgDimension + (maxIcons - 1) * ImgGapPix;

                Point corner = new Point(e.Bounds.X + e.Bounds.Width / 2, e.Bounds.Y);

                if (actIcons > maxIcons)
                {
                    e.Graphics.DrawString("...", e.Appearance.Font, SystemBrushes.ActiveCaptionText, corner, textAlign);
                }
                else
                {
                    corner.Offset(-totalWidth / 2, 0);

                    float heightFactor = Math.Min((float)e.Bounds.Height / (float)ImgDimension, 1.0F);
                    int newHeight = (int)Math.Floor(ImgDimension * heightFactor);
                    int newWidth = (int)Math.Floor(ImgDimension * heightFactor);

                    //int xIndex = 0;
                    //e.Graphics.DrawImage(certTag.IsTrusted ? imgTrusted : imgRejected, corner.X + (imgDimension + imgGapPix) * xIndex, corner.Y, newHeight, newWidth);
                    //xIndex++;

                    // ReSharper disable once UseObjectOrCollectionInitializer
                    List<Image> imgToDraw = new List<Image>();

                    imgToDraw.Add(certTag.IsTrusted ? imgTrusted : imgRejected);
                    imgToDraw.Add(certTag.HasPrivateKey ? imgKey: null);
                    imgToDraw.Add(certTag.IsUsedForServer ? imgUaServer: null);
                    imgToDraw.Add(certTag.IsUsedForAuthentication ? imgDude: null);

                    for (int i = 0; i < imgToDraw.Count; i++)
                    {
                        var img = imgToDraw[i];
                        if (imgToDraw[i] == null)
                            continue;
                        
                        e.Graphics.DrawImage(img, 
                            corner.X + (ImgDimension + ImgGapPix) * i, corner.Y,
                            newHeight, newWidth);
                    }
                }

                e.Handled = true;
            }
        }

        private static string GetCertificateAttribute(string subject, string attribute)
        {
            int lenAtt = attribute.Length;

            int idxAtt = subject.IndexOf(attribute);
            if (idxAtt == -1)
            {
                return "";
            }
            else
            {
                int idxCom = subject.IndexOf(',', idxAtt);
                if (idxCom != -1)
                    return subject.Substring(idxAtt + lenAtt, idxCom - idxAtt - lenAtt);
                else
                    return subject.Substring(idxAtt + lenAtt);
            }
        }

        private void toolTipController_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
        {
            if (e.SelectedControl != gridControlCerts)
                return;

            GridView view = gridControlCerts.GetViewAt(e.ControlMousePosition) as GridView;
            if (view == null)
                return;

            DevExpress.XtraGrid.Views.Grid.ViewInfo.GridHitInfo hitInfo = view.CalcHitInfo(e.ControlMousePosition);
            if (hitInfo.InRowCell && hitInfo.Column == colCertProperties)
            {
                // check if user points to something different (not the previously pointed item)
                if (toolTipLastRowHandle != hitInfo.RowHandle)
                {
                    // remember recent item
                    toolTipLastRowHandle = hitInfo.RowHandle;

                    // and generate a new tooltip
                    string ttInfo = null;

                    if (view.GetRow(toolTipLastRowHandle) is OpcUaData.CertificateTag cert)
                    {
                        // todo. kls. localize
                        ttInfo = cert.IsTrusted ? "Trusted" : "Rejected";//Properties.Resources.CertificateIsTrusted : Properties.Resources.CertificateIsRejected;

                        if (cert.HasPrivateKey)
                            ttInfo += $"; {"Private key"}";//string.Concat(ttInfo, "; ", Properties.Resources.CertificateHasPrivateKey);

                        if (cert.IsUsedForServer)
                            ttInfo += $"; {"OPC UA Server certificate"}";//string.Concat(ttInfo, "; ", Properties.Resources.CertificateHasPrivateKey);

                        if (cert.IsUsedForAuthentication)
                            ttInfo += $"; {"Authentication"}";//string.Concat(ttInfo, "; ", Properties.Resources.CertificateHasPrivateKey);
                    }

                    lastTooltip = string.IsNullOrEmpty(ttInfo) ? null :
                        new ToolTipControlInfo(hitInfo.HitTest.ToString() + hitInfo.RowHandle.ToString(), ttInfo);
                }

                if (lastTooltip != null)
                    e.Info = lastTooltip;
            }
        }



        #region Cert context menu

        private void gridViewCerts_PopupMenuShowing(object sender, PopupMenuShowingEventArgs e)
        {
            // let all dataRow-specific commands be unavailable by default
            miCertDelimiter1.Visible = miCertDelimiter2.Visible =
                miCertCopyAsText.Visible = miCertExport.Visible =
                miCertRemove.Visible = miCertToggleUserAuthentication.Visible =
                miCertTrust.Visible = miCertReject.Visible = miCertUseAsServerCert.Visible = false;

            // columns submenu - visible/checked consistency
            miColumnName.Checked = colCertName.Visible;
            miColumnProperties.Checked = colCertProperties.Visible;
            miColumnOrganization.Checked = colCertOrganization.Visible;
            miColumnLocality.Checked = colCertLocality.Visible;
            miColumnState.Checked = colCertState.Visible;
            miColumnCountry.Checked = colCertCountry.Visible;
            miColumnIssuedBy.Checked = colCertIssuedBy.Visible;
            miColumnIssuingDate.Checked = colCertIssuingDate.Visible;
            miColumnExpirationDate.Checked = colCertExpirationDate.Visible;
            miColumnAlgorithm.Checked = colCertAlgorithm.Visible;
            miColumnThumbprint.Checked = colCertThumbprint.Visible;

            if (!(gridControlCerts.GetViewAt(e.Point) is GridView view))
                return;

            var hitInfo = e.HitInfo;

            if (hitInfo.InRowCell && view.GetRow(hitInfo.RowHandle) is OpcUaData.CertificateTag certTag)
            {
                // unconditional commands for any cert
                miCertDelimiter1.Visible = miCertDelimiter2.Visible =
                    miCertCopyAsText.Visible = miCertExport.Visible =
                    miCertRemove.Visible = miCertToggleUserAuthentication.Visible = true;

                // conditional commands
                miCertTrust.Visible = !certTag.IsTrusted;
                miCertReject.Visible = certTag.IsTrusted;
                miCertUseAsServerCert.Visible = certTag.HasPrivateKey && !certTag.IsUsedForAuthentication;
            }
        }

        private void miCertCopyAsText_Click(object sender, EventArgs e)
        {
            try
            {
                var certTag = SelectedCertificate;
                var cert = certTag?.Certificate;
                if (cert == null)
                    return;

                // todo. kls. localize?
                string str = "";
                str += $"Name = {GetCertificateAttribute(cert.Subject, "CN=")}\r\n";
                str += $"Organization = {GetCertificateAttribute(cert.Subject, "CN=")}\r\n";
                str += $"Locality = {GetCertificateAttribute(cert.Subject, "L=")}\r\n";
                str += $"State = {GetCertificateAttribute(cert.Subject, "S=")}\r\n";
                str += $"Country = {GetCertificateAttribute(cert.Subject, "C=")}\r\n";
                str += $"Issued By = {GetCertificateAttribute(cert.Issuer, "CN=")}\r\n";
                str += $"Issuing Date = {cert.NotBefore}\r\n";
                str += $"Expiration Date = {cert.NotAfter}\r\n";
                str += $"Algorithm = {cert.SignatureAlgorithm.FriendlyName}\r\n";
                str += $"Thumbprint = {certTag.Thumbprint}\r\n";
                
                Clipboard.Clear();
                Clipboard.SetText(str);
            }
            catch { /*non critical*/ }
        }


        #region Cert context menu - Grid Columns visibility

        private void miColumnName_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertName;
            col.Visible = mi.Checked;
        }

        private void miColumnProperties_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertProperties;
            col.Visible = mi.Checked;
        }

        private void miColumnOrganization_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertOrganization;
            col.Visible = mi.Checked;
        }

        private void miColumnLocality_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertLocality;
            col.Visible = mi.Checked;
        }

        private void miColumnState_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertState;
            col.Visible = mi.Checked;
        }

        private void miColumnCountry_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertCountry;
            col.Visible = mi.Checked;
        }

        private void miColumnIssuedBy_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertIssuedBy;
            col.Visible = mi.Checked;
        }

        private void miColumnIssuedDate_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertIssuingDate;
            col.Visible = mi.Checked;
        }

        private void miColumnAlgorithm_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertAlgorithm;
            col.Visible = mi.Checked;
        }

        private void miColumnThumbprint_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertThumbprint;
            col.Visible = mi.Checked;
        }

        private void miColumnReset_Click(object sender, EventArgs e)
        {
            // visible by default
            colCertName.Visible = true;
            colCertProperties.Visible = true;
            colCertIssuedBy.Visible = true;
            colCertExpirationDate.Visible = true;

            // visible by default
            colCertOrganization.Visible = false;
            colCertLocality.Visible = false;
            colCertState.Visible = false;
            colCertCountry.Visible = false;
            colCertIssuingDate.Visible = false;
            colCertAlgorithm.Visible = false;
            colCertThumbprint.Visible = false;

            contextMenuCerts.Close();
        }

        #endregion


        #endregion


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
                    if (certs != null)
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
