namespace iba.Controls
{
    partial class OpcUaControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timerRefreshStatus = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.gbCertificates = new iba.Utility.CollapsibleGroupBox();
            this.gridControlCerts = new DevExpress.XtraGrid.GridControl();
            this.contextMenuCerts = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miColumns = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnName = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnProperties = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnOrganization = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnLocality = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnState = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnCountry = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnIssuedBy = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnIssuingDate = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnExpirationDate = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnAlgorithm = new System.Windows.Forms.ToolStripMenuItem();
            this.miColumnThumbprint = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.miColumnReset = new System.Windows.Forms.ToolStripMenuItem();
            this.miCertDelimiter1 = new System.Windows.Forms.ToolStripSeparator();
            this.miCertReject = new System.Windows.Forms.ToolStripMenuItem();
            this.miCertTrust = new System.Windows.Forms.ToolStripMenuItem();
            this.miCertToggleUserAuthentication = new System.Windows.Forms.ToolStripMenuItem();
            this.miCertUseAsServerCert = new System.Windows.Forms.ToolStripMenuItem();
            this.miCertDelimiter2 = new System.Windows.Forms.ToolStripSeparator();
            this.miCertCopyAsText = new System.Windows.Forms.ToolStripMenuItem();
            this.miCertExport = new System.Windows.Forms.ToolStripMenuItem();
            this.miCertRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.gridViewCerts = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colCertName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCertProperties = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCertOrganization = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCertLocality = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCertState = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCertCountry = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCertIssuedBy = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCertIssuingDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCertExpirationDate = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCertAlgorithm = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colCertThumbprint = new DevExpress.XtraGrid.Columns.GridColumn();
            this.toolTipController = new DevExpress.Utils.ToolTipController(this.components);
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.buttonCertAdd = new System.Windows.Forms.ToolStripButton();
            this.buttonCertGenerate = new System.Windows.Forms.ToolStripButton();
            this.buttonCertExport = new System.Windows.Forms.ToolStripButton();
            this.buttonCertRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonCertTrust = new System.Windows.Forms.ToolStripButton();
            this.buttonCertReject = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonCertUser = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonCertServer = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.buttonCertRefresh = new System.Windows.Forms.ToolStripButton();
            this.gbDiagnostics = new iba.Utility.CollapsibleGroupBox();
            this.tbDiagTmp = new System.Windows.Forms.TextBox();
            this.dgvSubscriptions = new System.Windows.Forms.DataGridView();
            this.colSubscrId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubscrMonitoredItemsCount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubscrPublishingInterval = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSubscrNextSeq = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvClients = new System.Windows.Forms.DataGridView();
            this.ColName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColLastMsg = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.buttonOpenLogFile = new System.Windows.Forms.Button();
            this.label14 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.gbObjects = new iba.Utility.CollapsibleGroupBox();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.splitContainerObjectsFooter = new System.Windows.Forms.SplitContainer();
            this.labelObjType = new System.Windows.Forms.Label();
            this.tbObjType = new System.Windows.Forms.TextBox();
            this.labelObjValue = new System.Windows.Forms.Label();
            this.tbObjValue = new System.Windows.Forms.TextBox();
            this.labelObjDescription = new System.Windows.Forms.Label();
            this.tbObjDescription = new System.Windows.Forms.TextBox();
            this.labelObjNodeId = new System.Windows.Forms.Label();
            this.tbObjNodeId = new System.Windows.Forms.TextBox();
            this.buttonRefreshGuiTree = new System.Windows.Forms.Button();
            this.buttonRebuildTree = new System.Windows.Forms.Button();
            this.tvObjects = new System.Windows.Forms.TreeView();
            this.gbConfiguration = new iba.Utility.CollapsibleGroupBox();
            this.splitContainerSecurity = new System.Windows.Forms.SplitContainer();
            this.gbLogon = new System.Windows.Forms.GroupBox();
            this.labelPassword = new System.Windows.Forms.Label();
            this.cbLogonAnonymous = new System.Windows.Forms.CheckBox();
            this.cbLogonUserName = new System.Windows.Forms.CheckBox();
            this.cbLogonCertificate = new System.Windows.Forms.CheckBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.gbSecurity = new System.Windows.Forms.GroupBox();
            this.comboBoxSecurity256Sha256 = new System.Windows.Forms.ComboBox();
            this.comboBoxSecurity256 = new System.Windows.Forms.ComboBox();
            this.comboBoxSecurity128 = new System.Windows.Forms.ComboBox();
            this.cbSecurity256Sha256 = new System.Windows.Forms.CheckBox();
            this.cbSecurity256 = new System.Windows.Forms.CheckBox();
            this.cbSecurity128 = new System.Windows.Forms.CheckBox();
            this.cbSecurityNone = new System.Windows.Forms.CheckBox();
            this.gbEndpoints = new System.Windows.Forms.GroupBox();
            this.dgvEndpoints = new System.Windows.Forms.DataGridView();
            this.dgvColumnHost = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColumnPort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dgvColumnUri = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonEndpointCopy = new System.Windows.Forms.Button();
            this.buttonEndpointDelete = new System.Windows.Forms.Button();
            this.buttonEndpointAdd = new System.Windows.Forms.Button();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.buttonConfigurationReset = new System.Windows.Forms.Button();
            this.buttonConfigurationApply = new System.Windows.Forms.Button();
            this.gbCertificates.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlCerts)).BeginInit();
            this.contextMenuCerts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCerts)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.gbDiagnostics.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubscriptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            this.gbObjects.SuspendLayout();
            this.panelFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).BeginInit();
            this.splitContainerObjectsFooter.Panel1.SuspendLayout();
            this.splitContainerObjectsFooter.Panel2.SuspendLayout();
            this.splitContainerObjectsFooter.SuspendLayout();
            this.gbConfiguration.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSecurity)).BeginInit();
            this.splitContainerSecurity.Panel1.SuspendLayout();
            this.splitContainerSecurity.Panel2.SuspendLayout();
            this.splitContainerSecurity.SuspendLayout();
            this.gbLogon.SuspendLayout();
            this.gbSecurity.SuspendLayout();
            this.gbEndpoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEndpoints)).BeginInit();
            this.SuspendLayout();
            // 
            // timerRefreshStatus
            // 
            this.timerRefreshStatus.Interval = 1000;
            this.timerRefreshStatus.Tick += new System.EventHandler(this.timerRefreshStatus_Tick);
            // 
            // openFileDialog
            // 
            this.openFileDialog.Filter = "DER encoded certificates|*.der|Certificates|*.cer|Personal Information Exchange f" +
    "iles|*.pfx|All files|*.*";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Filter = "DER encoded certificates|*.der|Certificates|*.cer|Personal Information Exchange f" +
    "iles|*.pfx|All files|*.*";
            // 
            // gbCertificates
            // 
            this.gbCertificates.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbCertificates.Controls.Add(this.gridControlCerts);
            this.gbCertificates.Controls.Add(this.toolStrip1);
            this.gbCertificates.Location = new System.Drawing.Point(15, 394);
            this.gbCertificates.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbCertificates.Name = "gbCertificates";
            this.gbCertificates.Size = new System.Drawing.Size(690, 201);
            this.gbCertificates.TabIndex = 17;
            this.gbCertificates.TabStop = false;
            this.gbCertificates.Text = "Certificates";
            // 
            // gridControlCerts
            // 
            this.gridControlCerts.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gridControlCerts.ContextMenuStrip = this.contextMenuCerts;
            this.gridControlCerts.Location = new System.Drawing.Point(18, 44);
            this.gridControlCerts.MainView = this.gridViewCerts;
            this.gridControlCerts.Name = "gridControlCerts";
            this.gridControlCerts.Size = new System.Drawing.Size(654, 151);
            this.gridControlCerts.TabIndex = 18;
            this.gridControlCerts.ToolTipController = this.toolTipController;
            this.gridControlCerts.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewCerts});
            // 
            // contextMenuCerts
            // 
            this.contextMenuCerts.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miColumns,
            this.miCertDelimiter1,
            this.miCertReject,
            this.miCertTrust,
            this.miCertToggleUserAuthentication,
            this.miCertUseAsServerCert,
            this.miCertDelimiter2,
            this.miCertCopyAsText,
            this.miCertExport,
            this.miCertRemove});
            this.contextMenuCerts.Name = "contextMenuCerts";
            this.contextMenuCerts.Size = new System.Drawing.Size(265, 224);
            // 
            // miColumns
            // 
            this.miColumns.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miColumnName,
            this.miColumnProperties,
            this.miColumnOrganization,
            this.miColumnLocality,
            this.miColumnState,
            this.miColumnCountry,
            this.miColumnIssuedBy,
            this.miColumnIssuingDate,
            this.miColumnExpirationDate,
            this.miColumnAlgorithm,
            this.miColumnThumbprint,
            this.toolStripMenuItem1,
            this.miColumnReset});
            this.miColumns.Name = "miColumns";
            this.miColumns.Size = new System.Drawing.Size(264, 26);
            this.miColumns.Text = "Columns";
            // 
            // miColumnName
            // 
            this.miColumnName.CheckOnClick = true;
            this.miColumnName.Name = "miColumnName";
            this.miColumnName.Size = new System.Drawing.Size(185, 26);
            this.miColumnName.Text = "Name";
            this.miColumnName.Click += new System.EventHandler(this.miColumnName_Click);
            // 
            // miColumnProperties
            // 
            this.miColumnProperties.CheckOnClick = true;
            this.miColumnProperties.Name = "miColumnProperties";
            this.miColumnProperties.Size = new System.Drawing.Size(185, 26);
            this.miColumnProperties.Text = "Properties";
            this.miColumnProperties.Click += new System.EventHandler(this.miColumnProperties_Click);
            // 
            // miColumnOrganization
            // 
            this.miColumnOrganization.CheckOnClick = true;
            this.miColumnOrganization.Name = "miColumnOrganization";
            this.miColumnOrganization.Size = new System.Drawing.Size(185, 26);
            this.miColumnOrganization.Text = "Organization";
            this.miColumnOrganization.Click += new System.EventHandler(this.miColumnOrganization_Click);
            // 
            // miColumnLocality
            // 
            this.miColumnLocality.CheckOnClick = true;
            this.miColumnLocality.Name = "miColumnLocality";
            this.miColumnLocality.Size = new System.Drawing.Size(185, 26);
            this.miColumnLocality.Text = "Locality";
            this.miColumnLocality.Click += new System.EventHandler(this.miColumnLocality_Click);
            // 
            // miColumnState
            // 
            this.miColumnState.CheckOnClick = true;
            this.miColumnState.Name = "miColumnState";
            this.miColumnState.Size = new System.Drawing.Size(185, 26);
            this.miColumnState.Text = "State";
            this.miColumnState.Click += new System.EventHandler(this.miColumnState_Click);
            // 
            // miColumnCountry
            // 
            this.miColumnCountry.CheckOnClick = true;
            this.miColumnCountry.Name = "miColumnCountry";
            this.miColumnCountry.Size = new System.Drawing.Size(185, 26);
            this.miColumnCountry.Text = "Country";
            this.miColumnCountry.Click += new System.EventHandler(this.miColumnCountry_Click);
            // 
            // miColumnIssuedBy
            // 
            this.miColumnIssuedBy.CheckOnClick = true;
            this.miColumnIssuedBy.Name = "miColumnIssuedBy";
            this.miColumnIssuedBy.Size = new System.Drawing.Size(185, 26);
            this.miColumnIssuedBy.Text = "Issued By";
            this.miColumnIssuedBy.Click += new System.EventHandler(this.miColumnIssuedBy_Click);
            // 
            // miColumnIssuingDate
            // 
            this.miColumnIssuingDate.CheckOnClick = true;
            this.miColumnIssuingDate.Name = "miColumnIssuingDate";
            this.miColumnIssuingDate.Size = new System.Drawing.Size(185, 26);
            this.miColumnIssuingDate.Text = "Issued Date";
            this.miColumnIssuingDate.Click += new System.EventHandler(this.miColumnIssuedDate_Click);
            // 
            // miColumnExpirationDate
            // 
            this.miColumnExpirationDate.Name = "miColumnExpirationDate";
            this.miColumnExpirationDate.Size = new System.Drawing.Size(185, 26);
            this.miColumnExpirationDate.Text = "Expiration Date";
            // 
            // miColumnAlgorithm
            // 
            this.miColumnAlgorithm.CheckOnClick = true;
            this.miColumnAlgorithm.Name = "miColumnAlgorithm";
            this.miColumnAlgorithm.Size = new System.Drawing.Size(185, 26);
            this.miColumnAlgorithm.Text = "Algorithm";
            this.miColumnAlgorithm.Click += new System.EventHandler(this.miColumnAlgorithm_Click);
            // 
            // miColumnThumbprint
            // 
            this.miColumnThumbprint.CheckOnClick = true;
            this.miColumnThumbprint.Name = "miColumnThumbprint";
            this.miColumnThumbprint.Size = new System.Drawing.Size(185, 26);
            this.miColumnThumbprint.Text = "Thumbprint";
            this.miColumnThumbprint.Click += new System.EventHandler(this.miColumnThumbprint_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(182, 6);
            // 
            // miColumnReset
            // 
            this.miColumnReset.Name = "miColumnReset";
            this.miColumnReset.Size = new System.Drawing.Size(185, 26);
            this.miColumnReset.Text = "Reset";
            this.miColumnReset.Click += new System.EventHandler(this.miColumnReset_Click);
            // 
            // miCertDelimiter1
            // 
            this.miCertDelimiter1.Name = "miCertDelimiter1";
            this.miCertDelimiter1.Size = new System.Drawing.Size(261, 6);
            // 
            // miCertReject
            // 
            this.miCertReject.Image = global::iba.Properties.Resources.img_shldred;
            this.miCertReject.Name = "miCertReject";
            this.miCertReject.Size = new System.Drawing.Size(264, 26);
            this.miCertReject.Text = "Reject";
            this.miCertReject.Click += new System.EventHandler(this.buttonCertReject_Click);
            // 
            // miCertTrust
            // 
            this.miCertTrust.Image = global::iba.Properties.Resources.img_shldgreen;
            this.miCertTrust.Name = "miCertTrust";
            this.miCertTrust.Size = new System.Drawing.Size(264, 26);
            this.miCertTrust.Text = "Trust";
            this.miCertTrust.Click += new System.EventHandler(this.buttonCertTrust_Click);
            // 
            // miCertToggleUserAuthentication
            // 
            this.miCertToggleUserAuthentication.Image = global::iba.Properties.Resources.img_dude;
            this.miCertToggleUserAuthentication.Name = "miCertToggleUserAuthentication";
            this.miCertToggleUserAuthentication.Size = new System.Drawing.Size(264, 26);
            this.miCertToggleUserAuthentication.Text = "Toggle user authentication";
            this.miCertToggleUserAuthentication.Click += new System.EventHandler(this.buttonCertUser_Click);
            // 
            // miCertUseAsServerCert
            // 
            this.miCertUseAsServerCert.Image = global::iba.Properties.Resources.opcUaServer_icon;
            this.miCertUseAsServerCert.Name = "miCertUseAsServerCert";
            this.miCertUseAsServerCert.Size = new System.Drawing.Size(264, 26);
            this.miCertUseAsServerCert.Text = "Use as server certificate";
            this.miCertUseAsServerCert.Click += new System.EventHandler(this.buttonCertServer_Click);
            // 
            // miCertDelimiter2
            // 
            this.miCertDelimiter2.Name = "miCertDelimiter2";
            this.miCertDelimiter2.Size = new System.Drawing.Size(261, 6);
            // 
            // miCertCopyAsText
            // 
            this.miCertCopyAsText.Image = global::iba.Properties.Resources.copy;
            this.miCertCopyAsText.Name = "miCertCopyAsText";
            this.miCertCopyAsText.Size = new System.Drawing.Size(264, 26);
            this.miCertCopyAsText.Text = "Copy to clipboard as text";
            this.miCertCopyAsText.Click += new System.EventHandler(this.miCertCopyAsText_Click);
            // 
            // miCertExport
            // 
            this.miCertExport.Image = global::iba.Properties.Resources.img_export;
            this.miCertExport.Name = "miCertExport";
            this.miCertExport.Size = new System.Drawing.Size(264, 26);
            this.miCertExport.Text = "Export";
            this.miCertExport.Click += new System.EventHandler(this.buttonCertExport_Click);
            // 
            // miCertRemove
            // 
            this.miCertRemove.Image = global::iba.Properties.Resources.remove;
            this.miCertRemove.Name = "miCertRemove";
            this.miCertRemove.Size = new System.Drawing.Size(264, 26);
            this.miCertRemove.Text = "Remove";
            this.miCertRemove.Click += new System.EventHandler(this.buttonCertRemove_Click);
            // 
            // gridViewCerts
            // 
            this.gridViewCerts.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colCertName,
            this.colCertProperties,
            this.colCertOrganization,
            this.colCertLocality,
            this.colCertState,
            this.colCertCountry,
            this.colCertIssuedBy,
            this.colCertIssuingDate,
            this.colCertExpirationDate,
            this.colCertAlgorithm,
            this.colCertThumbprint});
            this.gridViewCerts.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.RowFocus;
            this.gridViewCerts.GridControl = this.gridControlCerts;
            this.gridViewCerts.Name = "gridViewCerts";
            this.gridViewCerts.OptionsBehavior.Editable = false;
            this.gridViewCerts.OptionsBehavior.KeepGroupExpandedOnSorting = false;
            this.gridViewCerts.OptionsBehavior.ReadOnly = true;
            this.gridViewCerts.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewCerts.OptionsCustomization.AllowFilter = false;
            this.gridViewCerts.OptionsCustomization.AllowGroup = false;
            this.gridViewCerts.OptionsCustomization.AllowSort = false;
            this.gridViewCerts.OptionsDetail.AllowZoomDetail = false;
            this.gridViewCerts.OptionsDetail.EnableMasterViewMode = false;
            this.gridViewCerts.OptionsDetail.ShowDetailTabs = false;
            this.gridViewCerts.OptionsDetail.SmartDetailExpand = false;
            this.gridViewCerts.OptionsFilter.AllowColumnMRUFilterList = false;
            this.gridViewCerts.OptionsFilter.AllowFilterEditor = false;
            this.gridViewCerts.OptionsFilter.AllowMRUFilterList = false;
            this.gridViewCerts.OptionsHint.ShowCellHints = false;
            this.gridViewCerts.OptionsHint.ShowColumnHeaderHints = false;
            this.gridViewCerts.OptionsMenu.EnableColumnMenu = false;
            this.gridViewCerts.OptionsMenu.EnableFooterMenu = false;
            this.gridViewCerts.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewCerts.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewCerts.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewCerts.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridViewCerts.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewCerts.OptionsView.ShowGroupExpandCollapseButtons = false;
            this.gridViewCerts.OptionsView.ShowGroupPanel = false;
            this.gridViewCerts.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridViewCerts_CustomDrawCell);
            this.gridViewCerts.PopupMenuShowing += new DevExpress.XtraGrid.Views.Grid.PopupMenuShowingEventHandler(this.gridViewCerts_PopupMenuShowing);
            // 
            // colCertName
            // 
            this.colCertName.Caption = "Name";
            this.colCertName.Name = "colCertName";
            this.colCertName.Visible = true;
            this.colCertName.VisibleIndex = 0;
            // 
            // colCertProperties
            // 
            this.colCertProperties.Caption = "Properties";
            this.colCertProperties.Name = "colCertProperties";
            this.colCertProperties.Visible = true;
            this.colCertProperties.VisibleIndex = 1;
            // 
            // colCertOrganization
            // 
            this.colCertOrganization.Caption = "Organization";
            this.colCertOrganization.Name = "colCertOrganization";
            this.colCertOrganization.Visible = true;
            this.colCertOrganization.VisibleIndex = 2;
            // 
            // colCertLocality
            // 
            this.colCertLocality.Caption = "Locality";
            this.colCertLocality.Name = "colCertLocality";
            this.colCertLocality.Visible = true;
            this.colCertLocality.VisibleIndex = 3;
            // 
            // colCertState
            // 
            this.colCertState.Caption = "State";
            this.colCertState.Name = "colCertState";
            this.colCertState.Visible = true;
            this.colCertState.VisibleIndex = 4;
            // 
            // colCertCountry
            // 
            this.colCertCountry.Caption = "Country";
            this.colCertCountry.Name = "colCertCountry";
            this.colCertCountry.Visible = true;
            this.colCertCountry.VisibleIndex = 5;
            // 
            // colCertIssuedBy
            // 
            this.colCertIssuedBy.Caption = "Issued By";
            this.colCertIssuedBy.Name = "colCertIssuedBy";
            this.colCertIssuedBy.Visible = true;
            this.colCertIssuedBy.VisibleIndex = 6;
            // 
            // colCertIssuingDate
            // 
            this.colCertIssuingDate.Caption = "Issuing Date";
            this.colCertIssuingDate.Name = "colCertIssuingDate";
            this.colCertIssuingDate.Visible = true;
            this.colCertIssuingDate.VisibleIndex = 7;
            // 
            // colCertExpirationDate
            // 
            this.colCertExpirationDate.Caption = "Expiration Date";
            this.colCertExpirationDate.Name = "colCertExpirationDate";
            this.colCertExpirationDate.Visible = true;
            this.colCertExpirationDate.VisibleIndex = 8;
            // 
            // colCertAlgorithm
            // 
            this.colCertAlgorithm.Caption = "Algorithm";
            this.colCertAlgorithm.Name = "colCertAlgorithm";
            this.colCertAlgorithm.Visible = true;
            this.colCertAlgorithm.VisibleIndex = 9;
            // 
            // colCertThumbprint
            // 
            this.colCertThumbprint.Caption = "Thumbprint";
            this.colCertThumbprint.Name = "colCertThumbprint";
            this.colCertThumbprint.Visible = true;
            this.colCertThumbprint.VisibleIndex = 10;
            // 
            // toolTipController
            // 
            this.toolTipController.InitialDelay = 1;
            this.toolTipController.ReshowDelay = 1;
            this.toolTipController.ToolTipType = DevExpress.Utils.ToolTipType.Standard;
            this.toolTipController.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.toolTipController_GetActiveObjectInfo);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buttonCertAdd,
            this.buttonCertGenerate,
            this.buttonCertExport,
            this.buttonCertRemove,
            this.toolStripSeparator1,
            this.buttonCertTrust,
            this.buttonCertReject,
            this.toolStripSeparator2,
            this.buttonCertUser,
            this.toolStripSeparator3,
            this.buttonCertServer,
            this.toolStripSeparator4,
            this.buttonCertRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(3, 16);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(684, 25);
            this.toolStrip1.TabIndex = 17;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // buttonCertAdd
            // 
            this.buttonCertAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertAdd.Image = global::iba.Properties.Resources.img_add;
            this.buttonCertAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCertAdd.Name = "buttonCertAdd";
            this.buttonCertAdd.Size = new System.Drawing.Size(23, 22);
            this.buttonCertAdd.Text = "Add";
            this.buttonCertAdd.ToolTipText = "Add an existing certificate file";
            this.buttonCertAdd.Click += new System.EventHandler(this.buttonCertAdd_Click);
            // 
            // buttonCertGenerate
            // 
            this.buttonCertGenerate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertGenerate.Image = global::iba.Properties.Resources.img_cert;
            this.buttonCertGenerate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCertGenerate.Name = "buttonCertGenerate";
            this.buttonCertGenerate.Size = new System.Drawing.Size(23, 22);
            this.buttonCertGenerate.Text = "Generate";
            this.buttonCertGenerate.ToolTipText = "Generate a certificate";
            this.buttonCertGenerate.Click += new System.EventHandler(this.buttonCertGenerate_Click);
            // 
            // buttonCertExport
            // 
            this.buttonCertExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertExport.Image = global::iba.Properties.Resources.img_export;
            this.buttonCertExport.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.buttonCertExport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCertExport.Name = "buttonCertExport";
            this.buttonCertExport.Size = new System.Drawing.Size(23, 22);
            this.buttonCertExport.Text = "Export";
            this.buttonCertExport.ToolTipText = "Export the selected certificate to a file";
            this.buttonCertExport.Click += new System.EventHandler(this.buttonCertExport_Click);
            // 
            // buttonCertRemove
            // 
            this.buttonCertRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertRemove.Image = global::iba.Properties.Resources.remove;
            this.buttonCertRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCertRemove.Name = "buttonCertRemove";
            this.buttonCertRemove.Size = new System.Drawing.Size(23, 22);
            this.buttonCertRemove.Text = "Remove";
            this.buttonCertRemove.ToolTipText = "Remove the selected certificate";
            this.buttonCertRemove.Click += new System.EventHandler(this.buttonCertRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // buttonCertTrust
            // 
            this.buttonCertTrust.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertTrust.Image = global::iba.Properties.Resources.img_shldgreen;
            this.buttonCertTrust.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCertTrust.Name = "buttonCertTrust";
            this.buttonCertTrust.Size = new System.Drawing.Size(23, 22);
            this.buttonCertTrust.Text = "Trust the selected certificate";
            this.buttonCertTrust.Click += new System.EventHandler(this.buttonCertTrust_Click);
            // 
            // buttonCertReject
            // 
            this.buttonCertReject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertReject.Image = global::iba.Properties.Resources.img_shldred;
            this.buttonCertReject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCertReject.Name = "buttonCertReject";
            this.buttonCertReject.Size = new System.Drawing.Size(23, 22);
            this.buttonCertReject.Text = "Reject the selected certificate";
            this.buttonCertReject.Click += new System.EventHandler(this.buttonCertReject_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // buttonCertUser
            // 
            this.buttonCertUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertUser.Image = global::iba.Properties.Resources.img_dude;
            this.buttonCertUser.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCertUser.Name = "buttonCertUser";
            this.buttonCertUser.Size = new System.Drawing.Size(23, 22);
            this.buttonCertUser.Text = "Toggles whether the selected certificate can be used for the user authentication";
            this.buttonCertUser.Click += new System.EventHandler(this.buttonCertUser_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // buttonCertServer
            // 
            this.buttonCertServer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertServer.Image = global::iba.Properties.Resources.img_opcuaserver_cert;
            this.buttonCertServer.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCertServer.Name = "buttonCertServer";
            this.buttonCertServer.Size = new System.Drawing.Size(23, 22);
            this.buttonCertServer.Text = "Use the selected certificate as OPC UA server certificate";
            this.buttonCertServer.Click += new System.EventHandler(this.buttonCertServer_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // buttonCertRefresh
            // 
            this.buttonCertRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertRefresh.Image = global::iba.Properties.Resources.Aktualisieren;
            this.buttonCertRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.buttonCertRefresh.Name = "buttonCertRefresh";
            this.buttonCertRefresh.Size = new System.Drawing.Size(23, 22);
            this.buttonCertRefresh.Text = "Refresh the table";
            this.buttonCertRefresh.Click += new System.EventHandler(this.buttonCertRefresh_Click);
            // 
            // gbDiagnostics
            // 
            this.gbDiagnostics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbDiagnostics.Controls.Add(this.tbDiagTmp);
            this.gbDiagnostics.Controls.Add(this.dgvSubscriptions);
            this.gbDiagnostics.Controls.Add(this.dgvClients);
            this.gbDiagnostics.Controls.Add(this.tbStatus);
            this.gbDiagnostics.Controls.Add(this.buttonOpenLogFile);
            this.gbDiagnostics.Controls.Add(this.label14);
            this.gbDiagnostics.Controls.Add(this.label1);
            this.gbDiagnostics.Controls.Add(this.label15);
            this.gbDiagnostics.Location = new System.Drawing.Point(15, 1006);
            this.gbDiagnostics.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbDiagnostics.Name = "gbDiagnostics";
            this.gbDiagnostics.Padding = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbDiagnostics.Size = new System.Drawing.Size(690, 338);
            this.gbDiagnostics.TabIndex = 4;
            this.gbDiagnostics.TabStop = false;
            this.gbDiagnostics.Text = "Diagnostics";
            // 
            // tbDiagTmp
            // 
            this.tbDiagTmp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDiagTmp.BackColor = System.Drawing.Color.MistyRose;
            this.tbDiagTmp.Location = new System.Drawing.Point(18, 257);
            this.tbDiagTmp.Multiline = true;
            this.tbDiagTmp.Name = "tbDiagTmp";
            this.tbDiagTmp.ReadOnly = true;
            this.tbDiagTmp.Size = new System.Drawing.Size(654, 72);
            this.tbDiagTmp.TabIndex = 4;
            // 
            // dgvSubscriptions
            // 
            this.dgvSubscriptions.AllowUserToAddRows = false;
            this.dgvSubscriptions.AllowUserToDeleteRows = false;
            this.dgvSubscriptions.AllowUserToResizeRows = false;
            this.dgvSubscriptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvSubscriptions.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSubscriptions.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSubscrId,
            this.colSubscrMonitoredItemsCount,
            this.colSubscrPublishingInterval,
            this.colSubscrNextSeq});
            this.dgvSubscriptions.Location = new System.Drawing.Point(18, 182);
            this.dgvSubscriptions.Name = "dgvSubscriptions";
            this.dgvSubscriptions.ReadOnly = true;
            this.dgvSubscriptions.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dgvSubscriptions.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvSubscriptions.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSubscriptions.Size = new System.Drawing.Size(654, 69);
            this.dgvSubscriptions.StandardTab = true;
            this.dgvSubscriptions.TabIndex = 3;
            // 
            // colSubscrId
            // 
            this.colSubscrId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSubscrId.HeaderText = "ID";
            this.colSubscrId.MinimumWidth = 50;
            this.colSubscrId.Name = "colSubscrId";
            this.colSubscrId.ReadOnly = true;
            // 
            // colSubscrMonitoredItemsCount
            // 
            this.colSubscrMonitoredItemsCount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSubscrMonitoredItemsCount.HeaderText = "Monitored items count";
            this.colSubscrMonitoredItemsCount.MinimumWidth = 50;
            this.colSubscrMonitoredItemsCount.Name = "colSubscrMonitoredItemsCount";
            this.colSubscrMonitoredItemsCount.ReadOnly = true;
            // 
            // colSubscrPublishingInterval
            // 
            this.colSubscrPublishingInterval.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSubscrPublishingInterval.HeaderText = "Publishing interval";
            this.colSubscrPublishingInterval.MinimumWidth = 50;
            this.colSubscrPublishingInterval.Name = "colSubscrPublishingInterval";
            this.colSubscrPublishingInterval.ReadOnly = true;
            // 
            // colSubscrNextSeq
            // 
            this.colSubscrNextSeq.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colSubscrNextSeq.HeaderText = "Next sequence number";
            this.colSubscrNextSeq.MinimumWidth = 50;
            this.colSubscrNextSeq.Name = "colSubscrNextSeq";
            this.colSubscrNextSeq.ReadOnly = true;
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            this.dgvClients.AllowUserToResizeRows = false;
            this.dgvClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClients.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColName,
            this.ColId,
            this.ColLastMsg});
            this.dgvClients.Location = new System.Drawing.Point(18, 58);
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.ReadOnly = true;
            this.dgvClients.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dgvClients.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClients.Size = new System.Drawing.Size(654, 105);
            this.dgvClients.StandardTab = true;
            this.dgvClients.TabIndex = 1;
            this.dgvClients.SelectionChanged += new System.EventHandler(this.dgvClients_SelectionChanged);
            // 
            // ColName
            // 
            this.ColName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColName.FillWeight = 76.14214F;
            this.ColName.HeaderText = "Name";
            this.ColName.MinimumWidth = 50;
            this.ColName.Name = "ColName";
            this.ColName.ReadOnly = true;
            // 
            // ColId
            // 
            this.ColId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColId.FillWeight = 50F;
            this.ColId.HeaderText = "ID";
            this.ColId.MinimumWidth = 50;
            this.ColId.Name = "ColId";
            this.ColId.ReadOnly = true;
            // 
            // ColLastMsg
            // 
            this.ColLastMsg.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColLastMsg.FillWeight = 76.14214F;
            this.ColLastMsg.HeaderText = "Last message time";
            this.ColLastMsg.MinimumWidth = 50;
            this.ColLastMsg.Name = "ColLastMsg";
            this.ColLastMsg.ReadOnly = true;
            // 
            // tbStatus
            // 
            this.tbStatus.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbStatus.Location = new System.Drawing.Point(88, 19);
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.Size = new System.Drawing.Size(453, 20);
            this.tbStatus.TabIndex = 0;
            this.tbStatus.TabStop = false;
            // 
            // buttonOpenLogFile
            // 
            this.buttonOpenLogFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonOpenLogFile.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonOpenLogFile.Location = new System.Drawing.Point(547, 18);
            this.buttonOpenLogFile.Name = "buttonOpenLogFile";
            this.buttonOpenLogFile.Size = new System.Drawing.Size(125, 23);
            this.buttonOpenLogFile.TabIndex = 2;
            this.buttonOpenLogFile.Text = "Open log file";
            this.buttonOpenLogFile.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label14.Location = new System.Drawing.Point(18, 22);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 13);
            this.label14.TabIndex = 0;
            this.label14.Text = "Status:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label1.Location = new System.Drawing.Point(18, 166);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Subscriptions:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.label15.Location = new System.Drawing.Point(18, 42);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(138, 13);
            this.label15.TabIndex = 1;
            this.label15.Text = "Connected OPC UA clients:";
            // 
            // gbObjects
            // 
            this.gbObjects.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbObjects.Controls.Add(this.panelFooter);
            this.gbObjects.Controls.Add(this.buttonRefreshGuiTree);
            this.gbObjects.Controls.Add(this.buttonRebuildTree);
            this.gbObjects.Controls.Add(this.tvObjects);
            this.gbObjects.Location = new System.Drawing.Point(15, 602);
            this.gbObjects.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbObjects.Name = "gbObjects";
            this.gbObjects.Size = new System.Drawing.Size(690, 398);
            this.gbObjects.TabIndex = 3;
            this.gbObjects.TabStop = false;
            this.gbObjects.Text = "Objects";
            // 
            // panelFooter
            // 
            this.panelFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelFooter.Controls.Add(this.splitContainerObjectsFooter);
            this.panelFooter.Controls.Add(this.labelObjNodeId);
            this.panelFooter.Controls.Add(this.tbObjNodeId);
            this.panelFooter.Location = new System.Drawing.Point(15, 311);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(669, 81);
            this.panelFooter.TabIndex = 13;
            // 
            // splitContainerObjectsFooter
            // 
            this.splitContainerObjectsFooter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerObjectsFooter.IsSplitterFixed = true;
            this.splitContainerObjectsFooter.Location = new System.Drawing.Point(0, 26);
            this.splitContainerObjectsFooter.Name = "splitContainerObjectsFooter";
            // 
            // splitContainerObjectsFooter.Panel1
            // 
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjType);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjType);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjValue);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjValue);
            this.splitContainerObjectsFooter.Panel1.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
            // 
            // splitContainerObjectsFooter.Panel2
            // 
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.labelObjDescription);
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.tbObjDescription);
            this.splitContainerObjectsFooter.Size = new System.Drawing.Size(669, 52);
            this.splitContainerObjectsFooter.SplitterDistance = 240;
            this.splitContainerObjectsFooter.TabIndex = 2;
            // 
            // labelObjType
            // 
            this.labelObjType.AutoSize = true;
            this.labelObjType.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelObjType.Location = new System.Drawing.Point(3, 32);
            this.labelObjType.Name = "labelObjType";
            this.labelObjType.Size = new System.Drawing.Size(34, 13);
            this.labelObjType.TabIndex = 9;
            this.labelObjType.Text = "Type:";
            // 
            // tbObjType
            // 
            this.tbObjType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjType.Location = new System.Drawing.Point(67, 29);
            this.tbObjType.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.tbObjType.Name = "tbObjType";
            this.tbObjType.ReadOnly = true;
            this.tbObjType.Size = new System.Drawing.Size(164, 20);
            this.tbObjType.TabIndex = 1;
            this.tbObjType.TabStop = false;
            // 
            // labelObjValue
            // 
            this.labelObjValue.AutoSize = true;
            this.labelObjValue.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelObjValue.Location = new System.Drawing.Point(3, 6);
            this.labelObjValue.Name = "labelObjValue";
            this.labelObjValue.Size = new System.Drawing.Size(37, 13);
            this.labelObjValue.TabIndex = 9;
            this.labelObjValue.Text = "Value:";
            // 
            // tbObjValue
            // 
            this.tbObjValue.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjValue.Location = new System.Drawing.Point(67, 3);
            this.tbObjValue.Name = "tbObjValue";
            this.tbObjValue.ReadOnly = true;
            this.tbObjValue.Size = new System.Drawing.Size(164, 20);
            this.tbObjValue.TabIndex = 1;
            this.tbObjValue.TabStop = false;
            // 
            // labelObjDescription
            // 
            this.labelObjDescription.AutoSize = true;
            this.labelObjDescription.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelObjDescription.Location = new System.Drawing.Point(3, 6);
            this.labelObjDescription.Name = "labelObjDescription";
            this.labelObjDescription.Size = new System.Drawing.Size(63, 13);
            this.labelObjDescription.TabIndex = 12;
            this.labelObjDescription.Text = "Description:";
            // 
            // tbObjDescription
            // 
            this.tbObjDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjDescription.Location = new System.Drawing.Point(79, 3);
            this.tbObjDescription.Multiline = true;
            this.tbObjDescription.Name = "tbObjDescription";
            this.tbObjDescription.ReadOnly = true;
            this.tbObjDescription.Size = new System.Drawing.Size(333, 46);
            this.tbObjDescription.TabIndex = 10;
            this.tbObjDescription.TabStop = false;
            // 
            // labelObjNodeId
            // 
            this.labelObjNodeId.AutoSize = true;
            this.labelObjNodeId.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.labelObjNodeId.Location = new System.Drawing.Point(3, 6);
            this.labelObjNodeId.Name = "labelObjNodeId";
            this.labelObjNodeId.Size = new System.Drawing.Size(50, 13);
            this.labelObjNodeId.TabIndex = 13;
            this.labelObjNodeId.Text = "Node ID:";
            // 
            // tbObjNodeId
            // 
            this.tbObjNodeId.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbObjNodeId.Location = new System.Drawing.Point(67, 3);
            this.tbObjNodeId.Name = "tbObjNodeId";
            this.tbObjNodeId.ReadOnly = true;
            this.tbObjNodeId.Size = new System.Drawing.Size(590, 20);
            this.tbObjNodeId.TabIndex = 11;
            this.tbObjNodeId.TabStop = false;
            // 
            // buttonRefreshGuiTree
            // 
            this.buttonRefreshGuiTree.BackColor = System.Drawing.Color.Linen;
            this.buttonRefreshGuiTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRefreshGuiTree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonRefreshGuiTree.Location = new System.Drawing.Point(171, -1);
            this.buttonRefreshGuiTree.Name = "buttonRefreshGuiTree";
            this.buttonRefreshGuiTree.Size = new System.Drawing.Size(92, 19);
            this.buttonRefreshGuiTree.TabIndex = 12;
            this.buttonRefreshGuiTree.Text = "Force Refresh GUI";
            this.buttonRefreshGuiTree.UseVisualStyleBackColor = false;
            this.buttonRefreshGuiTree.Visible = false;
            this.buttonRefreshGuiTree.Click += new System.EventHandler(this.buttonRefreshGuiTree_Click);
            // 
            // buttonRebuildTree
            // 
            this.buttonRebuildTree.BackColor = System.Drawing.Color.Linen;
            this.buttonRebuildTree.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonRebuildTree.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonRebuildTree.Location = new System.Drawing.Point(73, -1);
            this.buttonRebuildTree.Name = "buttonRebuildTree";
            this.buttonRebuildTree.Size = new System.Drawing.Size(92, 19);
            this.buttonRebuildTree.TabIndex = 12;
            this.buttonRebuildTree.Text = "Force Rebuild Tree";
            this.buttonRebuildTree.UseVisualStyleBackColor = false;
            this.buttonRebuildTree.Visible = false;
            this.buttonRebuildTree.Click += new System.EventHandler(this.buttonRebuildTree_Click);
            // 
            // tvObjects
            // 
            this.tvObjects.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tvObjects.HideSelection = false;
            this.tvObjects.Location = new System.Drawing.Point(18, 19);
            this.tvObjects.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.tvObjects.Name = "tvObjects";
            this.tvObjects.Size = new System.Drawing.Size(654, 286);
            this.tvObjects.TabIndex = 0;
            this.tvObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjects_AfterSelect);
            // 
            // gbConfiguration
            // 
            this.gbConfiguration.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbConfiguration.Controls.Add(this.splitContainerSecurity);
            this.gbConfiguration.Controls.Add(this.gbEndpoints);
            this.gbConfiguration.Controls.Add(this.cbEnabled);
            this.gbConfiguration.Controls.Add(this.buttonConfigurationReset);
            this.gbConfiguration.Controls.Add(this.buttonConfigurationApply);
            this.gbConfiguration.Location = new System.Drawing.Point(15, 3);
            this.gbConfiguration.Margin = new System.Windows.Forms.Padding(15, 3, 15, 3);
            this.gbConfiguration.Name = "gbConfiguration";
            this.gbConfiguration.Size = new System.Drawing.Size(690, 385);
            this.gbConfiguration.TabIndex = 2;
            this.gbConfiguration.TabStop = false;
            this.gbConfiguration.Text = "Configuration";
            // 
            // splitContainerSecurity
            // 
            this.splitContainerSecurity.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainerSecurity.IsSplitterFixed = true;
            this.splitContainerSecurity.Location = new System.Drawing.Point(9, 42);
            this.splitContainerSecurity.Name = "splitContainerSecurity";
            // 
            // splitContainerSecurity.Panel1
            // 
            this.splitContainerSecurity.Panel1.Controls.Add(this.gbLogon);
            this.splitContainerSecurity.Panel1.Margin = new System.Windows.Forms.Padding(15, 0, 0, 0);
            // 
            // splitContainerSecurity.Panel2
            // 
            this.splitContainerSecurity.Panel2.Controls.Add(this.gbSecurity);
            this.splitContainerSecurity.Size = new System.Drawing.Size(663, 130);
            this.splitContainerSecurity.SplitterDistance = 390;
            this.splitContainerSecurity.TabIndex = 2;
            // 
            // gbLogon
            // 
            this.gbLogon.Controls.Add(this.labelPassword);
            this.gbLogon.Controls.Add(this.cbLogonAnonymous);
            this.gbLogon.Controls.Add(this.cbLogonUserName);
            this.gbLogon.Controls.Add(this.cbLogonCertificate);
            this.gbLogon.Controls.Add(this.tbUserName);
            this.gbLogon.Controls.Add(this.tbPassword);
            this.gbLogon.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbLogon.Location = new System.Drawing.Point(0, 0);
            this.gbLogon.Name = "gbLogon";
            this.gbLogon.Size = new System.Drawing.Size(390, 130);
            this.gbLogon.TabIndex = 0;
            this.gbLogon.TabStop = false;
            this.gbLogon.Text = "Logon policies";
            // 
            // labelPassword
            // 
            this.labelPassword.AutoSize = true;
            this.labelPassword.Location = new System.Drawing.Point(29, 75);
            this.labelPassword.Name = "labelPassword";
            this.labelPassword.Size = new System.Drawing.Size(56, 13);
            this.labelPassword.TabIndex = 9;
            this.labelPassword.Text = "Password:";
            // 
            // cbLogonAnonymous
            // 
            this.cbLogonAnonymous.AutoSize = true;
            this.cbLogonAnonymous.Location = new System.Drawing.Point(12, 21);
            this.cbLogonAnonymous.Name = "cbLogonAnonymous";
            this.cbLogonAnonymous.Size = new System.Drawing.Size(81, 17);
            this.cbLogonAnonymous.TabIndex = 8;
            this.cbLogonAnonymous.Text = "Anonymous";
            this.cbLogonAnonymous.UseVisualStyleBackColor = true;
            // 
            // cbLogonUserName
            // 
            this.cbLogonUserName.AutoSize = true;
            this.cbLogonUserName.Location = new System.Drawing.Point(12, 48);
            this.cbLogonUserName.Name = "cbLogonUserName";
            this.cbLogonUserName.Size = new System.Drawing.Size(80, 17);
            this.cbLogonUserName.TabIndex = 8;
            this.cbLogonUserName.Text = "User name:";
            this.cbLogonUserName.UseVisualStyleBackColor = true;
            this.cbLogonUserName.CheckedChanged += new System.EventHandler(this.cbLogonUserName_CheckedChanged);
            // 
            // cbLogonCertificate
            // 
            this.cbLogonCertificate.AutoSize = true;
            this.cbLogonCertificate.Location = new System.Drawing.Point(12, 102);
            this.cbLogonCertificate.Name = "cbLogonCertificate";
            this.cbLogonCertificate.Size = new System.Drawing.Size(73, 17);
            this.cbLogonCertificate.TabIndex = 7;
            this.cbLogonCertificate.Text = "Certificate";
            this.cbLogonCertificate.UseVisualStyleBackColor = true;
            // 
            // tbUserName
            // 
            this.tbUserName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbUserName.Location = new System.Drawing.Point(122, 46);
            this.tbUserName.Name = "tbUserName";
            this.tbUserName.Size = new System.Drawing.Size(262, 20);
            this.tbUserName.TabIndex = 4;
            // 
            // tbPassword
            // 
            this.tbPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPassword.Location = new System.Drawing.Point(122, 72);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.PasswordChar = '*';
            this.tbPassword.Size = new System.Drawing.Size(262, 20);
            this.tbPassword.TabIndex = 5;
            // 
            // gbSecurity
            // 
            this.gbSecurity.Controls.Add(this.comboBoxSecurity256Sha256);
            this.gbSecurity.Controls.Add(this.comboBoxSecurity256);
            this.gbSecurity.Controls.Add(this.comboBoxSecurity128);
            this.gbSecurity.Controls.Add(this.cbSecurity256Sha256);
            this.gbSecurity.Controls.Add(this.cbSecurity256);
            this.gbSecurity.Controls.Add(this.cbSecurity128);
            this.gbSecurity.Controls.Add(this.cbSecurityNone);
            this.gbSecurity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gbSecurity.Location = new System.Drawing.Point(0, 0);
            this.gbSecurity.Name = "gbSecurity";
            this.gbSecurity.Size = new System.Drawing.Size(269, 130);
            this.gbSecurity.TabIndex = 1;
            this.gbSecurity.TabStop = false;
            this.gbSecurity.Text = "Security policies";
            // 
            // comboBoxSecurity256Sha256
            // 
            this.comboBoxSecurity256Sha256.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecurity256Sha256.FormattingEnabled = true;
            this.comboBoxSecurity256Sha256.Items.AddRange(new object[] {
            "Sign",
            "Sign & Encrypt",
            "Sign + Sign & Encrypt"});
            this.comboBoxSecurity256Sha256.Location = new System.Drawing.Point(115, 100);
            this.comboBoxSecurity256Sha256.Name = "comboBoxSecurity256Sha256";
            this.comboBoxSecurity256Sha256.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSecurity256Sha256.TabIndex = 8;
            // 
            // comboBoxSecurity256
            // 
            this.comboBoxSecurity256.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecurity256.FormattingEnabled = true;
            this.comboBoxSecurity256.Items.AddRange(new object[] {
            "Sign",
            "Sign & Encrypt",
            "Sign + Sign & Encrypt"});
            this.comboBoxSecurity256.Location = new System.Drawing.Point(115, 73);
            this.comboBoxSecurity256.Name = "comboBoxSecurity256";
            this.comboBoxSecurity256.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSecurity256.TabIndex = 8;
            // 
            // comboBoxSecurity128
            // 
            this.comboBoxSecurity128.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecurity128.FormattingEnabled = true;
            this.comboBoxSecurity128.Items.AddRange(new object[] {
            "Sign",
            "Sign & Encrypt",
            "Sign + Sign & Encrypt"});
            this.comboBoxSecurity128.Location = new System.Drawing.Point(115, 46);
            this.comboBoxSecurity128.Name = "comboBoxSecurity128";
            this.comboBoxSecurity128.Size = new System.Drawing.Size(121, 21);
            this.comboBoxSecurity128.TabIndex = 8;
            // 
            // cbSecurity256Sha256
            // 
            this.cbSecurity256Sha256.AutoSize = true;
            this.cbSecurity256Sha256.Location = new System.Drawing.Point(6, 102);
            this.cbSecurity256Sha256.Name = "cbSecurity256Sha256";
            this.cbSecurity256Sha256.Size = new System.Drawing.Size(107, 17);
            this.cbSecurity256Sha256.TabIndex = 7;
            this.cbSecurity256Sha256.Text = "Basic256Sha256";
            this.cbSecurity256Sha256.UseVisualStyleBackColor = true;
            this.cbSecurity256Sha256.CheckedChanged += new System.EventHandler(this.cbSecurity256Sha256_CheckedChanged);
            // 
            // cbSecurity256
            // 
            this.cbSecurity256.AutoSize = true;
            this.cbSecurity256.Location = new System.Drawing.Point(6, 75);
            this.cbSecurity256.Name = "cbSecurity256";
            this.cbSecurity256.Size = new System.Drawing.Size(70, 17);
            this.cbSecurity256.TabIndex = 7;
            this.cbSecurity256.Text = "Basic256";
            this.cbSecurity256.UseVisualStyleBackColor = true;
            this.cbSecurity256.CheckedChanged += new System.EventHandler(this.cbSecurity256_CheckedChanged);
            // 
            // cbSecurity128
            // 
            this.cbSecurity128.AutoSize = true;
            this.cbSecurity128.Location = new System.Drawing.Point(6, 48);
            this.cbSecurity128.Name = "cbSecurity128";
            this.cbSecurity128.Size = new System.Drawing.Size(101, 17);
            this.cbSecurity128.TabIndex = 7;
            this.cbSecurity128.Text = "Basic128Rsa15";
            this.cbSecurity128.UseVisualStyleBackColor = true;
            this.cbSecurity128.CheckedChanged += new System.EventHandler(this.cbSecurity128_CheckedChanged);
            // 
            // cbSecurityNone
            // 
            this.cbSecurityNone.AutoSize = true;
            this.cbSecurityNone.Location = new System.Drawing.Point(6, 21);
            this.cbSecurityNone.Name = "cbSecurityNone";
            this.cbSecurityNone.Size = new System.Drawing.Size(52, 17);
            this.cbSecurityNone.TabIndex = 7;
            this.cbSecurityNone.Text = "None";
            this.cbSecurityNone.UseVisualStyleBackColor = true;
            // 
            // gbEndpoints
            // 
            this.gbEndpoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gbEndpoints.Controls.Add(this.dgvEndpoints);
            this.gbEndpoints.Controls.Add(this.buttonEndpointCopy);
            this.gbEndpoints.Controls.Add(this.buttonEndpointDelete);
            this.gbEndpoints.Controls.Add(this.buttonEndpointAdd);
            this.gbEndpoints.Location = new System.Drawing.Point(9, 178);
            this.gbEndpoints.Name = "gbEndpoints";
            this.gbEndpoints.Size = new System.Drawing.Size(663, 172);
            this.gbEndpoints.TabIndex = 16;
            this.gbEndpoints.TabStop = false;
            this.gbEndpoints.Text = "Endpoints";
            // 
            // dgvEndpoints
            // 
            this.dgvEndpoints.AllowUserToAddRows = false;
            this.dgvEndpoints.AllowUserToDeleteRows = false;
            this.dgvEndpoints.AllowUserToResizeRows = false;
            this.dgvEndpoints.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvEndpoints.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEndpoints.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dgvColumnHost,
            this.dgvColumnPort,
            this.dgvColumnUri});
            this.dgvEndpoints.Location = new System.Drawing.Point(9, 19);
            this.dgvEndpoints.Name = "dgvEndpoints";
            this.dgvEndpoints.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToFirstHeader;
            this.dgvEndpoints.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvEndpoints.Size = new System.Drawing.Size(619, 147);
            this.dgvEndpoints.StandardTab = true;
            this.dgvEndpoints.TabIndex = 15;
            this.dgvEndpoints.CellParsing += new System.Windows.Forms.DataGridViewCellParsingEventHandler(this.dgvEndpoints_CellParsing);
            this.dgvEndpoints.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEndpoints_CellValueChanged);
            this.dgvEndpoints.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvEndpoints_DataError);
            // 
            // dgvColumnHost
            // 
            this.dgvColumnHost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvColumnHost.HeaderText = "IP address/hostname";
            this.dgvColumnHost.MinimumWidth = 50;
            this.dgvColumnHost.Name = "dgvColumnHost";
            // 
            // dgvColumnPort
            // 
            this.dgvColumnPort.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvColumnPort.HeaderText = "Port";
            this.dgvColumnPort.MinimumWidth = 50;
            this.dgvColumnPort.Name = "dgvColumnPort";
            // 
            // dgvColumnUri
            // 
            this.dgvColumnUri.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.dgvColumnUri.HeaderText = "URI";
            this.dgvColumnUri.MinimumWidth = 50;
            this.dgvColumnUri.Name = "dgvColumnUri";
            this.dgvColumnUri.ReadOnly = true;
            // 
            // buttonEndpointCopy
            // 
            this.buttonEndpointCopy.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEndpointCopy.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonEndpointCopy.Image = global::iba.Properties.Resources.copy;
            this.buttonEndpointCopy.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonEndpointCopy.Location = new System.Drawing.Point(634, 48);
            this.buttonEndpointCopy.Name = "buttonEndpointCopy";
            this.buttonEndpointCopy.Size = new System.Drawing.Size(23, 23);
            this.buttonEndpointCopy.TabIndex = 16;
            this.buttonEndpointCopy.TabStop = false;
            this.buttonEndpointCopy.UseVisualStyleBackColor = true;
            this.buttonEndpointCopy.Click += new System.EventHandler(this.buttonEndpointCopy_Click);
            // 
            // buttonEndpointDelete
            // 
            this.buttonEndpointDelete.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEndpointDelete.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonEndpointDelete.Image = global::iba.Properties.Resources.remove;
            this.buttonEndpointDelete.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonEndpointDelete.Location = new System.Drawing.Point(634, 143);
            this.buttonEndpointDelete.Name = "buttonEndpointDelete";
            this.buttonEndpointDelete.Size = new System.Drawing.Size(23, 23);
            this.buttonEndpointDelete.TabIndex = 16;
            this.buttonEndpointDelete.TabStop = false;
            this.buttonEndpointDelete.UseVisualStyleBackColor = true;
            this.buttonEndpointDelete.Click += new System.EventHandler(this.buttonEndpointDelete_Click);
            // 
            // buttonEndpointAdd
            // 
            this.buttonEndpointAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonEndpointAdd.Font = new System.Drawing.Font("Arial Narrow", 8.25F);
            this.buttonEndpointAdd.Image = global::iba.Properties.Resources.img_add;
            this.buttonEndpointAdd.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonEndpointAdd.Location = new System.Drawing.Point(634, 19);
            this.buttonEndpointAdd.Name = "buttonEndpointAdd";
            this.buttonEndpointAdd.Size = new System.Drawing.Size(23, 23);
            this.buttonEndpointAdd.TabIndex = 6;
            this.buttonEndpointAdd.TabStop = false;
            this.buttonEndpointAdd.UseVisualStyleBackColor = true;
            this.buttonEndpointAdd.Click += new System.EventHandler(this.buttonEndpointAdd_Click);
            // 
            // cbEnabled
            // 
            this.cbEnabled.AutoSize = true;
            this.cbEnabled.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.cbEnabled.Location = new System.Drawing.Point(9, 19);
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.Size = new System.Drawing.Size(65, 17);
            this.cbEnabled.TabIndex = 0;
            this.cbEnabled.Text = "Enabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // buttonConfigurationReset
            // 
            this.buttonConfigurationReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigurationReset.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonConfigurationReset.Location = new System.Drawing.Point(516, 356);
            this.buttonConfigurationReset.Name = "buttonConfigurationReset";
            this.buttonConfigurationReset.Size = new System.Drawing.Size(75, 23);
            this.buttonConfigurationReset.TabIndex = 12;
            this.buttonConfigurationReset.Text = "Reset";
            this.buttonConfigurationReset.UseVisualStyleBackColor = true;
            this.buttonConfigurationReset.Click += new System.EventHandler(this.buttonConfigurationReset_Click);
            // 
            // buttonConfigurationApply
            // 
            this.buttonConfigurationApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConfigurationApply.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.buttonConfigurationApply.Location = new System.Drawing.Point(597, 356);
            this.buttonConfigurationApply.Margin = new System.Windows.Forms.Padding(3, 3, 15, 3);
            this.buttonConfigurationApply.Name = "buttonConfigurationApply";
            this.buttonConfigurationApply.Size = new System.Drawing.Size(75, 23);
            this.buttonConfigurationApply.TabIndex = 13;
            this.buttonConfigurationApply.Text = "Apply";
            this.buttonConfigurationApply.UseVisualStyleBackColor = true;
            this.buttonConfigurationApply.Click += new System.EventHandler(this.buttonConfigurationApply_Click);
            // 
            // OpcUaControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.Controls.Add(this.gbCertificates);
            this.Controls.Add(this.gbDiagnostics);
            this.Controls.Add(this.gbObjects);
            this.Controls.Add(this.gbConfiguration);
            this.MinimumSize = new System.Drawing.Size(720, 300);
            this.Name = "OpcUaControl";
            this.Size = new System.Drawing.Size(720, 1345);
            this.Load += new System.EventHandler(this.OpcUaControl_Load);
            this.gbCertificates.ResumeLayout(false);
            this.gbCertificates.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlCerts)).EndInit();
            this.contextMenuCerts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCerts)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.gbDiagnostics.ResumeLayout(false);
            this.gbDiagnostics.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSubscriptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            this.gbObjects.ResumeLayout(false);
            this.panelFooter.ResumeLayout(false);
            this.panelFooter.PerformLayout();
            this.splitContainerObjectsFooter.Panel1.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel1.PerformLayout();
            this.splitContainerObjectsFooter.Panel2.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).EndInit();
            this.splitContainerObjectsFooter.ResumeLayout(false);
            this.gbConfiguration.ResumeLayout(false);
            this.gbConfiguration.PerformLayout();
            this.splitContainerSecurity.Panel1.ResumeLayout(false);
            this.splitContainerSecurity.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerSecurity)).EndInit();
            this.splitContainerSecurity.ResumeLayout(false);
            this.gbLogon.ResumeLayout(false);
            this.gbLogon.PerformLayout();
            this.gbSecurity.ResumeLayout(false);
            this.gbSecurity.PerformLayout();
            this.gbEndpoints.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEndpoints)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Utility.CollapsibleGroupBox gbObjects;
        private System.Windows.Forms.TreeView tvObjects;
        private Utility.CollapsibleGroupBox gbConfiguration;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.Button buttonConfigurationReset;
        private System.Windows.Forms.Button buttonConfigurationApply;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.TextBox tbUserName;
        private Utility.CollapsibleGroupBox gbDiagnostics;
        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Button buttonOpenLogFile;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.Timer timerRefreshStatus;
        private System.Windows.Forms.GroupBox gbEndpoints;
        private System.Windows.Forms.Button buttonEndpointCopy;
        private System.Windows.Forms.Button buttonEndpointDelete;
        private System.Windows.Forms.DataGridView dgvEndpoints;
        private System.Windows.Forms.Button buttonEndpointAdd;
        private System.Windows.Forms.SplitContainer splitContainerSecurity;
        private System.Windows.Forms.GroupBox gbLogon;
        private System.Windows.Forms.CheckBox cbLogonCertificate;
        private System.Windows.Forms.GroupBox gbSecurity;
        private System.Windows.Forms.ComboBox comboBoxSecurity128;
        private System.Windows.Forms.CheckBox cbSecurity256;
        private System.Windows.Forms.CheckBox cbSecurity128;
        private System.Windows.Forms.CheckBox cbSecurityNone;
        private System.Windows.Forms.ComboBox comboBoxSecurity256;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColumnHost;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColumnPort;
        private System.Windows.Forms.DataGridViewTextBoxColumn dgvColumnUri;
        private System.Windows.Forms.Button buttonRebuildTree;
        private System.Windows.Forms.Button buttonRefreshGuiTree;
        private System.Windows.Forms.Label labelObjNodeId;
        private System.Windows.Forms.TextBox tbObjNodeId;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.SplitContainer splitContainerObjectsFooter;
        private System.Windows.Forms.Label labelObjType;
        private System.Windows.Forms.TextBox tbObjType;
        private System.Windows.Forms.Label labelObjValue;
        private System.Windows.Forms.TextBox tbObjValue;
        private System.Windows.Forms.Label labelObjDescription;
        private System.Windows.Forms.TextBox tbObjDescription;
        private System.Windows.Forms.DataGridView dgvSubscriptions;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubscrId;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubscrMonitoredItemsCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubscrPublishingInterval;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSubscrNextSeq;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDiagTmp;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColName;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColLastMsg;
        private Utility.CollapsibleGroupBox gbCertificates;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton buttonCertAdd;
        private System.Windows.Forms.ToolStripButton buttonCertGenerate;
        private System.Windows.Forms.ToolStripButton buttonCertExport;
        private System.Windows.Forms.ToolStripButton buttonCertRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton buttonCertTrust;
        private System.Windows.Forms.ToolStripButton buttonCertReject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton buttonCertUser;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton buttonCertServer;
        private System.Windows.Forms.CheckBox cbLogonUserName;
        private System.Windows.Forms.CheckBox cbLogonAnonymous;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ComboBox comboBoxSecurity256Sha256;
        private System.Windows.Forms.CheckBox cbSecurity256Sha256;
        private System.Windows.Forms.Label labelPassword;
        private DevExpress.XtraGrid.GridControl gridControlCerts;
        private DevExpress.XtraGrid.Views.Grid.GridView gridViewCerts;
        private System.Windows.Forms.ContextMenuStrip contextMenuCerts;
        private DevExpress.Utils.ToolTipController toolTipController;
        private System.Windows.Forms.ToolStripMenuItem miColumns;
        private System.Windows.Forms.ToolStripMenuItem miColumnName;
        private System.Windows.Forms.ToolStripMenuItem miColumnProperties;
        private System.Windows.Forms.ToolStripMenuItem miColumnOrganization;
        private System.Windows.Forms.ToolStripMenuItem miColumnLocality;
        private System.Windows.Forms.ToolStripMenuItem miColumnState;
        private System.Windows.Forms.ToolStripMenuItem miColumnCountry;
        private System.Windows.Forms.ToolStripMenuItem miColumnIssuedBy;
        private System.Windows.Forms.ToolStripMenuItem miColumnIssuingDate;
        private System.Windows.Forms.ToolStripMenuItem miColumnAlgorithm;
        private System.Windows.Forms.ToolStripMenuItem miColumnThumbprint;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem miColumnReset;
        private System.Windows.Forms.ToolStripSeparator miCertDelimiter1;
        private System.Windows.Forms.ToolStripMenuItem miCertReject;
        private System.Windows.Forms.ToolStripMenuItem miCertTrust;
        private System.Windows.Forms.ToolStripMenuItem miCertToggleUserAuthentication;
        private System.Windows.Forms.ToolStripMenuItem miCertUseAsServerCert;
        private System.Windows.Forms.ToolStripSeparator miCertDelimiter2;
        private System.Windows.Forms.ToolStripMenuItem miCertCopyAsText;
        private System.Windows.Forms.ToolStripMenuItem miCertExport;
        private System.Windows.Forms.ToolStripMenuItem miCertRemove;
        private DevExpress.XtraGrid.Columns.GridColumn colCertName;
        private DevExpress.XtraGrid.Columns.GridColumn colCertProperties;
        private DevExpress.XtraGrid.Columns.GridColumn colCertOrganization;
        private DevExpress.XtraGrid.Columns.GridColumn colCertLocality;
        private DevExpress.XtraGrid.Columns.GridColumn colCertState;
        private DevExpress.XtraGrid.Columns.GridColumn colCertCountry;
        private DevExpress.XtraGrid.Columns.GridColumn colCertIssuedBy;
        private DevExpress.XtraGrid.Columns.GridColumn colCertIssuingDate;
        private DevExpress.XtraGrid.Columns.GridColumn colCertAlgorithm;
        private DevExpress.XtraGrid.Columns.GridColumn colCertThumbprint;
        private System.Windows.Forms.ToolStripMenuItem miColumnExpirationDate;
        private DevExpress.XtraGrid.Columns.GridColumn colCertExpirationDate;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton buttonCertRefresh;
    }
}
