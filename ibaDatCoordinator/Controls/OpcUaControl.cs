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

            // set up endpoints grid
            gridCtrlEndpoints.DataSource = _endpoints;
            var hostnameEditor = new HostnameEditor(this) {GridView = gridViewEndpoints};
            DevExpress.XtraEditors.Repository.RepositoryItem item = hostnameEditor.Editor;
            item.Name = "HostnameEditor";
            item.AllowFocused = false;
            item.AutoHeight = false;
            item.BorderStyle = BorderStyles.NoBorder;
            gridCtrlEndpoints.RepositoryItems.Add(item);
            colHostname.ColumnEdit = item;

#if DEBUG
            gbDebug.Visible = true;
#endif
        }

        private const int ImageIndexFolder = 0;
        private const int ImageIndexLeaf = 1;

        private readonly BindingList<OpcUaData.OpcUaEndPoint> _endpoints = new BindingList<OpcUaData.OpcUaEndPoint>(); // todo. kls. rename

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
            foreach (var ep in _endpoints)
                _data.Endpoints.Add(new OpcUaData.OpcUaEndPoint(ep));

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
            _endpoints.Clear();
            foreach (var ep in _data.Endpoints)
                _endpoints.Add(new OpcUaData.OpcUaEndPoint(ep));
            ApplyEndpointsButtonsEnabling();

            // disable/enable elements
            cbLogonUserName_CheckedChanged(null, null);
            cbSecurity128_CheckedChanged(null, null);
            cbSecurity256_CheckedChanged(null, null);
            cbSecurity256Sha256_CheckedChanged(null, null);

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
            try
            {
                cert.Import(openFileDialog.FileName);
            }
            catch 
            {
                // try again with a password
                using (OpcUaCertificateEnterPasswordForm passForm = new OpcUaCertificateEnterPasswordForm())
                {
                    if (passForm.ShowDialog(this) != DialogResult.OK)
                        return;

                    try
                    {
                        cert = new X509Certificate2();
                        cert.Import(openFileDialog.FileName, passForm.Password, X509KeyStorageFlags.DefaultKeySet);
                    }
                    catch (Exception ex)
                    {
                        // todo. kls. localize - AddCertificateFailed/AddCertificateCaption
                        MessageBox.Show(this,
                            string.Format("Failed to add certificate: {0}", ex.Message), "Add certificate",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

            }

            var certs = TaskManager.Manager.OpcUaHandleCertificate("add", cert);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertGenerate_Click(object sender, EventArgs e)
        {
            OpcUaData.CGenerateCertificateArgs certArgs = new OpcUaData.CGenerateCertificateArgs
            {
                ApplicationName = $"ibaDatCoordinator OPC UA Server@{TaskManager.Manager.GetServerHostName()}",
                UseSha256 = true
            };

            using (OpcUaCertificateGenerateForm genForm = new OpcUaCertificateGenerateForm())
            {
                genForm.LoadDataSource(certArgs);

                if (genForm.ShowDialog() != DialogResult.OK)
                    return;

                genForm.ApplySettings(certArgs);
            }
            
            var certs = TaskManager.Manager.OpcUaHandleCertificate("generate", certArgs);

            if (certs == null)
            {
                // todo. kls. localize
                MessageBox.Show(this,
                    "Failed to generate the certificate", "Generate certificate",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

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

            var certs = TaskManager.Manager.OpcUaHandleCertificate("remove", certTag.Thumbprint);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertTrust_Click(object sender, EventArgs e)
        {
            var certTag = SelectedCertificate;
            if (certTag == null)
                return;
            var certs = TaskManager.Manager.OpcUaHandleCertificate("trust", certTag.Thumbprint);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertReject_Click(object sender, EventArgs e)
        {
            var certTag = SelectedCertificate;
            if (certTag == null)
                return;
            var certs = TaskManager.Manager.OpcUaHandleCertificate("reject", certTag.Thumbprint);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertUser_Click(object sender, EventArgs e)
        {
            var certTag = SelectedCertificate;
            if (certTag == null)
                return;
            var certs = TaskManager.Manager.OpcUaHandleCertificate("asUser", certTag.Thumbprint);
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

            var certs = TaskManager.Manager.OpcUaHandleCertificate("asServer", certTag.Thumbprint);
            RefreshCertificatesTable(certs);
        }

        private void buttonCertRefresh_Click(object sender, EventArgs e)
        {
            var certs = TaskManager.Manager.OpcUaHandleCertificate("forceSync");
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
                e.DisplayText = cert.NotBefore.ToString(CultureInfo.CurrentCulture);
            }
            else if (e.Column == colCertExpirationDate)
            {
                e.DisplayText = cert.NotAfter.ToString(CultureInfo.CurrentCulture);
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

                    float heightFactor = Math.Min(e.Bounds.Height / (float)ImgDimension, 1.0F);
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

            int idxAtt = subject.IndexOf(attribute, StringComparison.Ordinal);
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

        private void miColumnExpirationDate_Click(object sender, EventArgs e)
        {
            if (!(sender is ToolStripMenuItem mi))
                return;
            GridColumn col = colCertExpirationDate;
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

        private class HostnameEditor
        {
            public DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit Editor { get; }

            public GridView GridView;

            public HostnameEditor(OpcUaControl parentControl)
            {
                this.parentControl = parentControl;

                Editor = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
                Editor.Name = "HostnameEditor";
                Editor.Buttons.Clear();
                //var btn = new EditorButton(ButtonPredefines.Ellipsis);
                Editor.ButtonsStyle = BorderStyles.Simple;
                Editor.ButtonsStyle = BorderStyles.UltraFlat;
                Editor.Buttons.Add(new EditorButton(ButtonPredefines.Ellipsis));

                Editor.ButtonClick += Edit_ButtonClick;
            }

            private OpcUaControl parentControl;

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
                    // todo. kls. localize
                    MessageBox.Show(parent, "Error trying to get network configuration", "OpcUaServer", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                
                // todo. kls. delete before last beta
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
                    {
                        try
                        {
                            RefreshCertificatesTable(certs);
                        }
                        catch { /* not critical; can happen if UaWorker is not initialized */}
                    }
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

        private void btOpenLogFile_Click(object sender, EventArgs e)
        {
            //IOConfiguratorDlg.Instance.OpenLogFile("%serverpath%log", "OpcUAServerLog.txt");
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
