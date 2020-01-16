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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OpcUaControl));
            this.splitContainerObjectsFooter = new System.Windows.Forms.SplitContainer();
            this.labelObjType = new System.Windows.Forms.Label();
            this.tbObjType = new System.Windows.Forms.TextBox();
            this.labelObjValue = new System.Windows.Forms.Label();
            this.tbObjValue = new System.Windows.Forms.TextBox();
            this.labelObjDescription = new System.Windows.Forms.Label();
            this.tbObjDescription = new System.Windows.Forms.TextBox();
            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
            this.repositoryItemSpinEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
            this.timerRefreshStatus = new System.Windows.Forms.Timer(this.components);
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
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
            this.tbDiagTmp = new System.Windows.Forms.TextBox();
            this.tbStatus = new System.Windows.Forms.TextBox();
            this.panelTagsFooter = new System.Windows.Forms.Panel();
            this.labelObjNodeId = new System.Windows.Forms.Label();
            this.tbObjNodeId = new System.Windows.Forms.TextBox();
            this.buttonRefreshGuiTree = new System.Windows.Forms.Button();
            this.buttonRebuildTree = new System.Windows.Forms.Button();
            this.tvObjects = new System.Windows.Forms.TreeView();
            this.panelFooterWithButtons = new System.Windows.Forms.Panel();
            this.buttonShowHideDebug = new System.Windows.Forms.Button();
            this.buttonConfigurationReset = new System.Windows.Forms.Button();
            this.buttonConfigurationApply = new System.Windows.Forms.Button();
            this.gbSecurity = new System.Windows.Forms.GroupBox();
            this.comboBoxSecurity256Sha256 = new System.Windows.Forms.ComboBox();
            this.comboBoxSecurity256 = new System.Windows.Forms.ComboBox();
            this.comboBoxSecurity128 = new System.Windows.Forms.ComboBox();
            this.cbSecurity256Sha256 = new System.Windows.Forms.CheckBox();
            this.cbSecurity256 = new System.Windows.Forms.CheckBox();
            this.cbSecurity128 = new System.Windows.Forms.CheckBox();
            this.cbSecurityNone = new System.Windows.Forms.CheckBox();
            this.gbLogon = new System.Windows.Forms.GroupBox();
            this.tbPassword = new iba.Controls.PasswordEditWithConfirmation();
            this.labelPassword = new System.Windows.Forms.Label();
            this.cbLogonAnonymous = new System.Windows.Forms.CheckBox();
            this.cbLogonUserName = new System.Windows.Forms.CheckBox();
            this.cbLogonCertificate = new System.Windows.Forms.CheckBox();
            this.tbUserName = new System.Windows.Forms.TextBox();
            this.gbEndpoints = new System.Windows.Forms.GroupBox();
            this.gridCtrlEndpoints = new DevExpress.XtraGrid.GridControl();
            this.gridViewEndpoints = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colHostname = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colPort = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colURI = new DevExpress.XtraGrid.Columns.GridColumn();
            this.buttonEndpointRemove = new System.Windows.Forms.Button();
            this.buttonEndpointCopy = new System.Windows.Forms.Button();
            this.buttonEndpointAdd = new System.Windows.Forms.Button();
            this.cbEnabled = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new Crownwood.DotNetMagic.Controls.TabControl();
            this.tabConfiguration = new Crownwood.DotNetMagic.Controls.TabPage();
            this.tabCertificates = new Crownwood.DotNetMagic.Controls.TabPage();
            this.tabTags = new Crownwood.DotNetMagic.Controls.TabPage();
            this.tabDiag = new Crownwood.DotNetMagic.Controls.TabPage();
            this.btOpenLogFile = new System.Windows.Forms.Button();
            this.gridCtrlSubscriptions = new DevExpress.XtraGrid.GridControl();
            this.gridViewSubscriptions = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSubscrMonItemCount = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSubscrPublInterval = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSubscrNextSeqNr = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label3 = new System.Windows.Forms.Label();
            this.gridCtrlSessions = new DevExpress.XtraGrid.GridControl();
            this.gridViewSessions = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.colSessionName = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSessionID = new DevExpress.XtraGrid.Columns.GridColumn();
            this.colSessionLastMsgTime = new DevExpress.XtraGrid.Columns.GridColumn();
            this.label12 = new System.Windows.Forms.Label();
            this.gbDebug = new System.Windows.Forms.GroupBox();
            this.gbStatus = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).BeginInit();
            this.splitContainerObjectsFooter.Panel1.SuspendLayout();
            this.splitContainerObjectsFooter.Panel2.SuspendLayout();
            this.splitContainerObjectsFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlCerts)).BeginInit();
            this.contextMenuCerts.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCerts)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.panelTagsFooter.SuspendLayout();
            this.panelFooterWithButtons.SuspendLayout();
            this.gbSecurity.SuspendLayout();
            this.gbLogon.SuspendLayout();
            this.gbEndpoints.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlEndpoints)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEndpoints)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabConfiguration.SuspendLayout();
            this.tabCertificates.SuspendLayout();
            this.tabTags.SuspendLayout();
            this.tabDiag.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlSubscriptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSubscriptions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlSessions)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSessions)).BeginInit();
            this.gbDebug.SuspendLayout();
            this.gbStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainerObjectsFooter
            // 
            resources.ApplyResources(this.splitContainerObjectsFooter, "splitContainerObjectsFooter");
            this.splitContainerObjectsFooter.Name = "splitContainerObjectsFooter";
            // 
            // splitContainerObjectsFooter.Panel1
            // 
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjType);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjType);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.labelObjValue);
            this.splitContainerObjectsFooter.Panel1.Controls.Add(this.tbObjValue);
            resources.ApplyResources(this.splitContainerObjectsFooter.Panel1, "splitContainerObjectsFooter.Panel1");
            // 
            // splitContainerObjectsFooter.Panel2
            // 
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.labelObjDescription);
            this.splitContainerObjectsFooter.Panel2.Controls.Add(this.tbObjDescription);
            this.splitContainerObjectsFooter.TabStop = false;
            // 
            // labelObjType
            // 
            resources.ApplyResources(this.labelObjType, "labelObjType");
            this.labelObjType.Name = "labelObjType";
            // 
            // tbObjType
            // 
            resources.ApplyResources(this.tbObjType, "tbObjType");
            this.tbObjType.Name = "tbObjType";
            this.tbObjType.ReadOnly = true;
            this.tbObjType.TabStop = false;
            // 
            // labelObjValue
            // 
            resources.ApplyResources(this.labelObjValue, "labelObjValue");
            this.labelObjValue.Name = "labelObjValue";
            // 
            // tbObjValue
            // 
            resources.ApplyResources(this.tbObjValue, "tbObjValue");
            this.tbObjValue.Name = "tbObjValue";
            this.tbObjValue.ReadOnly = true;
            this.tbObjValue.TabStop = false;
            // 
            // labelObjDescription
            // 
            resources.ApplyResources(this.labelObjDescription, "labelObjDescription");
            this.labelObjDescription.Name = "labelObjDescription";
            // 
            // tbObjDescription
            // 
            resources.ApplyResources(this.tbObjDescription, "tbObjDescription");
            this.tbObjDescription.Name = "tbObjDescription";
            this.tbObjDescription.ReadOnly = true;
            this.tbObjDescription.TabStop = false;
            // 
            // repositoryItemTextEdit1
            // 
            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemSpinEdit1
            // 
            resources.ApplyResources(this.repositoryItemSpinEdit1, "repositoryItemSpinEdit1");
            this.repositoryItemSpinEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
            new DevExpress.XtraEditors.Controls.EditorButton(((DevExpress.XtraEditors.Controls.ButtonPredefines)(resources.GetObject("repositoryItemSpinEdit1.Buttons"))))});
            this.repositoryItemSpinEdit1.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.UltraFlat;
            this.repositoryItemSpinEdit1.IsFloatValue = false;
            this.repositoryItemSpinEdit1.Mask.EditMask = resources.GetString("repositoryItemSpinEdit1.Mask.EditMask");
            this.repositoryItemSpinEdit1.MaxValue = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.MinValue = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.repositoryItemSpinEdit1.Name = "repositoryItemSpinEdit1";
            // 
            // timerRefreshStatus
            // 
            this.timerRefreshStatus.Interval = 1000;
            this.timerRefreshStatus.Tick += new System.EventHandler(this.timerRefreshStatus_Tick);
            // 
            // openFileDialog
            // 
            resources.ApplyResources(this.openFileDialog, "openFileDialog");
            // 
            // saveFileDialog
            // 
            resources.ApplyResources(this.saveFileDialog, "saveFileDialog");
            // 
            // gridControlCerts
            // 
            resources.ApplyResources(this.gridControlCerts, "gridControlCerts");
            this.gridControlCerts.ContextMenuStrip = this.contextMenuCerts;
            this.gridControlCerts.MainView = this.gridViewCerts;
            this.gridControlCerts.Name = "gridControlCerts";
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
            resources.ApplyResources(this.contextMenuCerts, "contextMenuCerts");
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
            resources.ApplyResources(this.miColumns, "miColumns");
            // 
            // miColumnName
            // 
            this.miColumnName.CheckOnClick = true;
            this.miColumnName.Name = "miColumnName";
            resources.ApplyResources(this.miColumnName, "miColumnName");
            this.miColumnName.Click += new System.EventHandler(this.miColumnName_Click);
            // 
            // miColumnProperties
            // 
            this.miColumnProperties.CheckOnClick = true;
            this.miColumnProperties.Name = "miColumnProperties";
            resources.ApplyResources(this.miColumnProperties, "miColumnProperties");
            this.miColumnProperties.Click += new System.EventHandler(this.miColumnProperties_Click);
            // 
            // miColumnOrganization
            // 
            this.miColumnOrganization.CheckOnClick = true;
            this.miColumnOrganization.Name = "miColumnOrganization";
            resources.ApplyResources(this.miColumnOrganization, "miColumnOrganization");
            this.miColumnOrganization.Click += new System.EventHandler(this.miColumnOrganization_Click);
            // 
            // miColumnLocality
            // 
            this.miColumnLocality.CheckOnClick = true;
            this.miColumnLocality.Name = "miColumnLocality";
            resources.ApplyResources(this.miColumnLocality, "miColumnLocality");
            this.miColumnLocality.Click += new System.EventHandler(this.miColumnLocality_Click);
            // 
            // miColumnState
            // 
            this.miColumnState.CheckOnClick = true;
            this.miColumnState.Name = "miColumnState";
            resources.ApplyResources(this.miColumnState, "miColumnState");
            this.miColumnState.Click += new System.EventHandler(this.miColumnState_Click);
            // 
            // miColumnCountry
            // 
            this.miColumnCountry.CheckOnClick = true;
            this.miColumnCountry.Name = "miColumnCountry";
            resources.ApplyResources(this.miColumnCountry, "miColumnCountry");
            this.miColumnCountry.Click += new System.EventHandler(this.miColumnCountry_Click);
            // 
            // miColumnIssuedBy
            // 
            this.miColumnIssuedBy.CheckOnClick = true;
            this.miColumnIssuedBy.Name = "miColumnIssuedBy";
            resources.ApplyResources(this.miColumnIssuedBy, "miColumnIssuedBy");
            this.miColumnIssuedBy.Click += new System.EventHandler(this.miColumnIssuedBy_Click);
            // 
            // miColumnIssuingDate
            // 
            this.miColumnIssuingDate.CheckOnClick = true;
            this.miColumnIssuingDate.Name = "miColumnIssuingDate";
            resources.ApplyResources(this.miColumnIssuingDate, "miColumnIssuingDate");
            this.miColumnIssuingDate.Click += new System.EventHandler(this.miColumnIssuedDate_Click);
            // 
            // miColumnExpirationDate
            // 
            this.miColumnExpirationDate.CheckOnClick = true;
            this.miColumnExpirationDate.Name = "miColumnExpirationDate";
            resources.ApplyResources(this.miColumnExpirationDate, "miColumnExpirationDate");
            this.miColumnExpirationDate.Click += new System.EventHandler(this.miColumnExpirationDate_Click);
            // 
            // miColumnAlgorithm
            // 
            this.miColumnAlgorithm.CheckOnClick = true;
            this.miColumnAlgorithm.Name = "miColumnAlgorithm";
            resources.ApplyResources(this.miColumnAlgorithm, "miColumnAlgorithm");
            this.miColumnAlgorithm.Click += new System.EventHandler(this.miColumnAlgorithm_Click);
            // 
            // miColumnThumbprint
            // 
            this.miColumnThumbprint.CheckOnClick = true;
            this.miColumnThumbprint.Name = "miColumnThumbprint";
            resources.ApplyResources(this.miColumnThumbprint, "miColumnThumbprint");
            this.miColumnThumbprint.Click += new System.EventHandler(this.miColumnThumbprint_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            resources.ApplyResources(this.toolStripMenuItem1, "toolStripMenuItem1");
            // 
            // miColumnReset
            // 
            this.miColumnReset.Name = "miColumnReset";
            resources.ApplyResources(this.miColumnReset, "miColumnReset");
            this.miColumnReset.Click += new System.EventHandler(this.miColumnReset_Click);
            // 
            // miCertDelimiter1
            // 
            this.miCertDelimiter1.Name = "miCertDelimiter1";
            resources.ApplyResources(this.miCertDelimiter1, "miCertDelimiter1");
            // 
            // miCertReject
            // 
            this.miCertReject.Image = global::iba.Properties.Resources.img_shldred;
            this.miCertReject.Name = "miCertReject";
            resources.ApplyResources(this.miCertReject, "miCertReject");
            this.miCertReject.Click += new System.EventHandler(this.buttonCertReject_Click);
            // 
            // miCertTrust
            // 
            this.miCertTrust.Image = global::iba.Properties.Resources.img_shldgreen;
            this.miCertTrust.Name = "miCertTrust";
            resources.ApplyResources(this.miCertTrust, "miCertTrust");
            this.miCertTrust.Click += new System.EventHandler(this.buttonCertTrust_Click);
            // 
            // miCertToggleUserAuthentication
            // 
            this.miCertToggleUserAuthentication.Image = global::iba.Properties.Resources.img_dude;
            this.miCertToggleUserAuthentication.Name = "miCertToggleUserAuthentication";
            resources.ApplyResources(this.miCertToggleUserAuthentication, "miCertToggleUserAuthentication");
            this.miCertToggleUserAuthentication.Click += new System.EventHandler(this.buttonCertUser_Click);
            // 
            // miCertUseAsServerCert
            // 
            this.miCertUseAsServerCert.Image = global::iba.Properties.Resources.opcUaServer_icon;
            this.miCertUseAsServerCert.Name = "miCertUseAsServerCert";
            resources.ApplyResources(this.miCertUseAsServerCert, "miCertUseAsServerCert");
            this.miCertUseAsServerCert.Click += new System.EventHandler(this.buttonCertServer_Click);
            // 
            // miCertDelimiter2
            // 
            this.miCertDelimiter2.Name = "miCertDelimiter2";
            resources.ApplyResources(this.miCertDelimiter2, "miCertDelimiter2");
            // 
            // miCertCopyAsText
            // 
            this.miCertCopyAsText.Image = global::iba.Properties.Resources.copy;
            this.miCertCopyAsText.Name = "miCertCopyAsText";
            resources.ApplyResources(this.miCertCopyAsText, "miCertCopyAsText");
            this.miCertCopyAsText.Click += new System.EventHandler(this.miCertCopyAsText_Click);
            // 
            // miCertExport
            // 
            this.miCertExport.Image = global::iba.Properties.Resources.img_export;
            this.miCertExport.Name = "miCertExport";
            resources.ApplyResources(this.miCertExport, "miCertExport");
            this.miCertExport.Click += new System.EventHandler(this.buttonCertExport_Click);
            // 
            // miCertRemove
            // 
            this.miCertRemove.Image = global::iba.Properties.Resources.remove;
            this.miCertRemove.Name = "miCertRemove";
            resources.ApplyResources(this.miCertRemove, "miCertRemove");
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
            resources.ApplyResources(this.colCertName, "colCertName");
            this.colCertName.Name = "colCertName";
            // 
            // colCertProperties
            // 
            resources.ApplyResources(this.colCertProperties, "colCertProperties");
            this.colCertProperties.Name = "colCertProperties";
            // 
            // colCertOrganization
            // 
            resources.ApplyResources(this.colCertOrganization, "colCertOrganization");
            this.colCertOrganization.Name = "colCertOrganization";
            // 
            // colCertLocality
            // 
            resources.ApplyResources(this.colCertLocality, "colCertLocality");
            this.colCertLocality.Name = "colCertLocality";
            // 
            // colCertState
            // 
            resources.ApplyResources(this.colCertState, "colCertState");
            this.colCertState.Name = "colCertState";
            // 
            // colCertCountry
            // 
            resources.ApplyResources(this.colCertCountry, "colCertCountry");
            this.colCertCountry.Name = "colCertCountry";
            // 
            // colCertIssuedBy
            // 
            resources.ApplyResources(this.colCertIssuedBy, "colCertIssuedBy");
            this.colCertIssuedBy.Name = "colCertIssuedBy";
            // 
            // colCertIssuingDate
            // 
            resources.ApplyResources(this.colCertIssuingDate, "colCertIssuingDate");
            this.colCertIssuingDate.Name = "colCertIssuingDate";
            // 
            // colCertExpirationDate
            // 
            resources.ApplyResources(this.colCertExpirationDate, "colCertExpirationDate");
            this.colCertExpirationDate.Name = "colCertExpirationDate";
            // 
            // colCertAlgorithm
            // 
            resources.ApplyResources(this.colCertAlgorithm, "colCertAlgorithm");
            this.colCertAlgorithm.Name = "colCertAlgorithm";
            // 
            // colCertThumbprint
            // 
            resources.ApplyResources(this.colCertThumbprint, "colCertThumbprint");
            this.colCertThumbprint.Name = "colCertThumbprint";
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
            resources.ApplyResources(this.toolStrip1, "toolStrip1");
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            // 
            // buttonCertAdd
            // 
            this.buttonCertAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertAdd.Image = global::iba.Properties.Resources.img_add;
            resources.ApplyResources(this.buttonCertAdd, "buttonCertAdd");
            this.buttonCertAdd.Name = "buttonCertAdd";
            this.buttonCertAdd.Click += new System.EventHandler(this.buttonCertAdd_Click);
            // 
            // buttonCertGenerate
            // 
            this.buttonCertGenerate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertGenerate.Image = global::iba.Properties.Resources.img_cert;
            resources.ApplyResources(this.buttonCertGenerate, "buttonCertGenerate");
            this.buttonCertGenerate.Name = "buttonCertGenerate";
            this.buttonCertGenerate.Click += new System.EventHandler(this.buttonCertGenerate_Click);
            // 
            // buttonCertExport
            // 
            this.buttonCertExport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertExport.Image = global::iba.Properties.Resources.img_export;
            resources.ApplyResources(this.buttonCertExport, "buttonCertExport");
            this.buttonCertExport.Name = "buttonCertExport";
            this.buttonCertExport.Click += new System.EventHandler(this.buttonCertExport_Click);
            // 
            // buttonCertRemove
            // 
            this.buttonCertRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertRemove.Image = global::iba.Properties.Resources.remove;
            resources.ApplyResources(this.buttonCertRemove, "buttonCertRemove");
            this.buttonCertRemove.Name = "buttonCertRemove";
            this.buttonCertRemove.Click += new System.EventHandler(this.buttonCertRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            resources.ApplyResources(this.toolStripSeparator1, "toolStripSeparator1");
            // 
            // buttonCertTrust
            // 
            this.buttonCertTrust.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertTrust.Image = global::iba.Properties.Resources.img_shldgreen;
            resources.ApplyResources(this.buttonCertTrust, "buttonCertTrust");
            this.buttonCertTrust.Name = "buttonCertTrust";
            this.buttonCertTrust.Click += new System.EventHandler(this.buttonCertTrust_Click);
            // 
            // buttonCertReject
            // 
            this.buttonCertReject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertReject.Image = global::iba.Properties.Resources.img_shldred;
            resources.ApplyResources(this.buttonCertReject, "buttonCertReject");
            this.buttonCertReject.Name = "buttonCertReject";
            this.buttonCertReject.Click += new System.EventHandler(this.buttonCertReject_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            resources.ApplyResources(this.toolStripSeparator2, "toolStripSeparator2");
            // 
            // buttonCertUser
            // 
            this.buttonCertUser.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertUser.Image = global::iba.Properties.Resources.img_dude;
            resources.ApplyResources(this.buttonCertUser, "buttonCertUser");
            this.buttonCertUser.Name = "buttonCertUser";
            this.buttonCertUser.Click += new System.EventHandler(this.buttonCertUser_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            resources.ApplyResources(this.toolStripSeparator3, "toolStripSeparator3");
            // 
            // buttonCertServer
            // 
            this.buttonCertServer.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertServer.Image = global::iba.Properties.Resources.img_opcuaserver_cert;
            resources.ApplyResources(this.buttonCertServer, "buttonCertServer");
            this.buttonCertServer.Name = "buttonCertServer";
            this.buttonCertServer.Click += new System.EventHandler(this.buttonCertServer_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            resources.ApplyResources(this.toolStripSeparator4, "toolStripSeparator4");
            // 
            // buttonCertRefresh
            // 
            this.buttonCertRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.buttonCertRefresh.Image = global::iba.Properties.Resources.Aktualisieren;
            resources.ApplyResources(this.buttonCertRefresh, "buttonCertRefresh");
            this.buttonCertRefresh.Name = "buttonCertRefresh";
            this.buttonCertRefresh.Click += new System.EventHandler(this.buttonCertRefresh_Click);
            // 
            // tbDiagTmp
            // 
            this.tbDiagTmp.BackColor = System.Drawing.Color.MistyRose;
            resources.ApplyResources(this.tbDiagTmp, "tbDiagTmp");
            this.tbDiagTmp.Name = "tbDiagTmp";
            this.tbDiagTmp.ReadOnly = true;
            // 
            // tbStatus
            // 
            resources.ApplyResources(this.tbStatus, "tbStatus");
            this.tbStatus.Name = "tbStatus";
            this.tbStatus.ReadOnly = true;
            this.tbStatus.TabStop = false;
            // 
            // panelTagsFooter
            // 
            this.panelTagsFooter.Controls.Add(this.splitContainerObjectsFooter);
            this.panelTagsFooter.Controls.Add(this.labelObjNodeId);
            this.panelTagsFooter.Controls.Add(this.tbObjNodeId);
            resources.ApplyResources(this.panelTagsFooter, "panelTagsFooter");
            this.panelTagsFooter.Name = "panelTagsFooter";
            // 
            // labelObjNodeId
            // 
            resources.ApplyResources(this.labelObjNodeId, "labelObjNodeId");
            this.labelObjNodeId.Name = "labelObjNodeId";
            // 
            // tbObjNodeId
            // 
            resources.ApplyResources(this.tbObjNodeId, "tbObjNodeId");
            this.tbObjNodeId.Name = "tbObjNodeId";
            this.tbObjNodeId.ReadOnly = true;
            this.tbObjNodeId.TabStop = false;
            // 
            // buttonRefreshGuiTree
            // 
            resources.ApplyResources(this.buttonRefreshGuiTree, "buttonRefreshGuiTree");
            this.buttonRefreshGuiTree.BackColor = System.Drawing.Color.Linen;
            this.buttonRefreshGuiTree.Name = "buttonRefreshGuiTree";
            this.buttonRefreshGuiTree.UseVisualStyleBackColor = false;
            this.buttonRefreshGuiTree.Click += new System.EventHandler(this.buttonRefreshGuiTree_Click);
            // 
            // buttonRebuildTree
            // 
            this.buttonRebuildTree.BackColor = System.Drawing.Color.Linen;
            resources.ApplyResources(this.buttonRebuildTree, "buttonRebuildTree");
            this.buttonRebuildTree.Name = "buttonRebuildTree";
            this.buttonRebuildTree.UseVisualStyleBackColor = false;
            this.buttonRebuildTree.Click += new System.EventHandler(this.buttonRebuildTree_Click);
            // 
            // tvObjects
            // 
            resources.ApplyResources(this.tvObjects, "tvObjects");
            this.tvObjects.HideSelection = false;
            this.tvObjects.Name = "tvObjects";
            this.tvObjects.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tvObjects_AfterSelect);
            // 
            // panelFooterWithButtons
            // 
            this.panelFooterWithButtons.Controls.Add(this.buttonShowHideDebug);
            this.panelFooterWithButtons.Controls.Add(this.buttonConfigurationReset);
            this.panelFooterWithButtons.Controls.Add(this.buttonConfigurationApply);
            resources.ApplyResources(this.panelFooterWithButtons, "panelFooterWithButtons");
            this.panelFooterWithButtons.Name = "panelFooterWithButtons";
            // 
            // buttonShowHideDebug
            // 
            resources.ApplyResources(this.buttonShowHideDebug, "buttonShowHideDebug");
            this.buttonShowHideDebug.BackColor = System.Drawing.Color.MistyRose;
            this.buttonShowHideDebug.Name = "buttonShowHideDebug";
            this.buttonShowHideDebug.UseVisualStyleBackColor = false;
            this.buttonShowHideDebug.Click += new System.EventHandler(this.buttonHide_Click);
            // 
            // buttonConfigurationReset
            // 
            resources.ApplyResources(this.buttonConfigurationReset, "buttonConfigurationReset");
            this.buttonConfigurationReset.Name = "buttonConfigurationReset";
            this.buttonConfigurationReset.UseVisualStyleBackColor = true;
            this.buttonConfigurationReset.Click += new System.EventHandler(this.buttonConfigurationReset_Click);
            // 
            // buttonConfigurationApply
            // 
            resources.ApplyResources(this.buttonConfigurationApply, "buttonConfigurationApply");
            this.buttonConfigurationApply.Name = "buttonConfigurationApply";
            this.buttonConfigurationApply.UseVisualStyleBackColor = true;
            this.buttonConfigurationApply.Click += new System.EventHandler(this.buttonConfigurationApply_Click);
            // 
            // gbSecurity
            // 
            resources.ApplyResources(this.gbSecurity, "gbSecurity");
            this.gbSecurity.Controls.Add(this.comboBoxSecurity256Sha256);
            this.gbSecurity.Controls.Add(this.comboBoxSecurity256);
            this.gbSecurity.Controls.Add(this.comboBoxSecurity128);
            this.gbSecurity.Controls.Add(this.cbSecurity256Sha256);
            this.gbSecurity.Controls.Add(this.cbSecurity256);
            this.gbSecurity.Controls.Add(this.cbSecurity128);
            this.gbSecurity.Controls.Add(this.cbSecurityNone);
            this.gbSecurity.Name = "gbSecurity";
            this.gbSecurity.TabStop = false;
            // 
            // comboBoxSecurity256Sha256
            // 
            this.comboBoxSecurity256Sha256.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecurity256Sha256.FormattingEnabled = true;
            this.comboBoxSecurity256Sha256.Items.AddRange(new object[] {
            resources.GetString("comboBoxSecurity256Sha256.Items"),
            resources.GetString("comboBoxSecurity256Sha256.Items1"),
            resources.GetString("comboBoxSecurity256Sha256.Items2")});
            resources.ApplyResources(this.comboBoxSecurity256Sha256, "comboBoxSecurity256Sha256");
            this.comboBoxSecurity256Sha256.Name = "comboBoxSecurity256Sha256";
            // 
            // comboBoxSecurity256
            // 
            this.comboBoxSecurity256.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecurity256.FormattingEnabled = true;
            this.comboBoxSecurity256.Items.AddRange(new object[] {
            resources.GetString("comboBoxSecurity256.Items"),
            resources.GetString("comboBoxSecurity256.Items1"),
            resources.GetString("comboBoxSecurity256.Items2")});
            resources.ApplyResources(this.comboBoxSecurity256, "comboBoxSecurity256");
            this.comboBoxSecurity256.Name = "comboBoxSecurity256";
            // 
            // comboBoxSecurity128
            // 
            this.comboBoxSecurity128.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxSecurity128.FormattingEnabled = true;
            this.comboBoxSecurity128.Items.AddRange(new object[] {
            resources.GetString("comboBoxSecurity128.Items"),
            resources.GetString("comboBoxSecurity128.Items1"),
            resources.GetString("comboBoxSecurity128.Items2")});
            resources.ApplyResources(this.comboBoxSecurity128, "comboBoxSecurity128");
            this.comboBoxSecurity128.Name = "comboBoxSecurity128";
            // 
            // cbSecurity256Sha256
            // 
            resources.ApplyResources(this.cbSecurity256Sha256, "cbSecurity256Sha256");
            this.cbSecurity256Sha256.Name = "cbSecurity256Sha256";
            this.cbSecurity256Sha256.UseVisualStyleBackColor = true;
            this.cbSecurity256Sha256.CheckedChanged += new System.EventHandler(this.cbSecurity256Sha256_CheckedChanged);
            // 
            // cbSecurity256
            // 
            resources.ApplyResources(this.cbSecurity256, "cbSecurity256");
            this.cbSecurity256.Name = "cbSecurity256";
            this.cbSecurity256.UseVisualStyleBackColor = true;
            this.cbSecurity256.CheckedChanged += new System.EventHandler(this.cbSecurity256_CheckedChanged);
            // 
            // cbSecurity128
            // 
            resources.ApplyResources(this.cbSecurity128, "cbSecurity128");
            this.cbSecurity128.Name = "cbSecurity128";
            this.cbSecurity128.UseVisualStyleBackColor = true;
            this.cbSecurity128.CheckedChanged += new System.EventHandler(this.cbSecurity128_CheckedChanged);
            // 
            // cbSecurityNone
            // 
            resources.ApplyResources(this.cbSecurityNone, "cbSecurityNone");
            this.cbSecurityNone.Name = "cbSecurityNone";
            this.cbSecurityNone.UseVisualStyleBackColor = true;
            // 
            // gbLogon
            // 
            resources.ApplyResources(this.gbLogon, "gbLogon");
            this.gbLogon.Controls.Add(this.tbPassword);
            this.gbLogon.Controls.Add(this.labelPassword);
            this.gbLogon.Controls.Add(this.cbLogonAnonymous);
            this.gbLogon.Controls.Add(this.cbLogonUserName);
            this.gbLogon.Controls.Add(this.cbLogonCertificate);
            this.gbLogon.Controls.Add(this.tbUserName);
            this.gbLogon.Name = "gbLogon";
            this.gbLogon.TabStop = false;
            // 
            // tbPassword
            // 
            resources.ApplyResources(this.tbPassword, "tbPassword");
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Password = "";
            this.tbPassword.ReadOnly = false;
            this.tbPassword.Tooltip = null;
            // 
            // labelPassword
            // 
            resources.ApplyResources(this.labelPassword, "labelPassword");
            this.labelPassword.Name = "labelPassword";
            // 
            // cbLogonAnonymous
            // 
            resources.ApplyResources(this.cbLogonAnonymous, "cbLogonAnonymous");
            this.cbLogonAnonymous.Name = "cbLogonAnonymous";
            this.cbLogonAnonymous.UseVisualStyleBackColor = true;
            // 
            // cbLogonUserName
            // 
            resources.ApplyResources(this.cbLogonUserName, "cbLogonUserName");
            this.cbLogonUserName.Name = "cbLogonUserName";
            this.cbLogonUserName.UseVisualStyleBackColor = true;
            this.cbLogonUserName.CheckedChanged += new System.EventHandler(this.cbLogonUserName_CheckedChanged);
            // 
            // cbLogonCertificate
            // 
            resources.ApplyResources(this.cbLogonCertificate, "cbLogonCertificate");
            this.cbLogonCertificate.Name = "cbLogonCertificate";
            this.cbLogonCertificate.UseVisualStyleBackColor = true;
            // 
            // tbUserName
            // 
            resources.ApplyResources(this.tbUserName, "tbUserName");
            this.tbUserName.Name = "tbUserName";
            // 
            // gbEndpoints
            // 
            resources.ApplyResources(this.gbEndpoints, "gbEndpoints");
            this.gbEndpoints.Controls.Add(this.gridCtrlEndpoints);
            this.gbEndpoints.Controls.Add(this.buttonEndpointRemove);
            this.gbEndpoints.Controls.Add(this.buttonEndpointCopy);
            this.gbEndpoints.Controls.Add(this.buttonEndpointAdd);
            this.gbEndpoints.Name = "gbEndpoints";
            this.gbEndpoints.TabStop = false;
            // 
            // gridCtrlEndpoints
            // 
            resources.ApplyResources(this.gridCtrlEndpoints, "gridCtrlEndpoints");
            this.gridCtrlEndpoints.MainView = this.gridViewEndpoints;
            this.gridCtrlEndpoints.Name = "gridCtrlEndpoints";
            this.gridCtrlEndpoints.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewEndpoints});
            // 
            // gridViewEndpoints
            // 
            this.gridViewEndpoints.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colHostname,
            this.colPort,
            this.colURI});
            this.gridViewEndpoints.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridViewEndpoints.GridControl = this.gridCtrlEndpoints;
            resources.ApplyResources(this.gridViewEndpoints, "gridViewEndpoints");
            this.gridViewEndpoints.Name = "gridViewEndpoints";
            this.gridViewEndpoints.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewEndpoints.OptionsBehavior.KeepGroupExpandedOnSorting = false;
            this.gridViewEndpoints.OptionsCustomization.AllowColumnMoving = false;
            this.gridViewEndpoints.OptionsCustomization.AllowFilter = false;
            this.gridViewEndpoints.OptionsCustomization.AllowSort = false;
            this.gridViewEndpoints.OptionsMenu.EnableColumnMenu = false;
            this.gridViewEndpoints.OptionsMenu.EnableFooterMenu = false;
            this.gridViewEndpoints.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewEndpoints.OptionsNavigation.AutoMoveRowFocus = false;
            this.gridViewEndpoints.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewEndpoints.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewEndpoints.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridViewEndpoints.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewEndpoints.OptionsView.ShowGroupPanel = false;
            // 
            // colHostname
            // 
            resources.ApplyResources(this.colHostname, "colHostname");
            this.colHostname.ColumnEdit = this.repositoryItemTextEdit1;
            this.colHostname.FieldName = "Hostname";
            this.colHostname.Name = "colHostname";
            // 
            // colPort
            // 
            resources.ApplyResources(this.colPort, "colPort");
            this.colPort.ColumnEdit = this.repositoryItemSpinEdit1;
            this.colPort.FieldName = "Port";
            this.colPort.Name = "colPort";
            // 
            // colURI
            // 
            resources.ApplyResources(this.colURI, "colURI");
            this.colURI.ColumnEdit = this.repositoryItemTextEdit1;
            this.colURI.FieldName = "Uri";
            this.colURI.Name = "colURI";
            this.colURI.OptionsColumn.ReadOnly = true;
            // 
            // buttonEndpointRemove
            // 
            resources.ApplyResources(this.buttonEndpointRemove, "buttonEndpointRemove");
            this.buttonEndpointRemove.Image = global::iba.Properties.Resources.remove;
            this.buttonEndpointRemove.Name = "buttonEndpointRemove";
            this.buttonEndpointRemove.TabStop = false;
            this.buttonEndpointRemove.UseVisualStyleBackColor = true;
            this.buttonEndpointRemove.Click += new System.EventHandler(this.buttonEndpointRemove_Click);
            // 
            // buttonEndpointCopy
            // 
            resources.ApplyResources(this.buttonEndpointCopy, "buttonEndpointCopy");
            this.buttonEndpointCopy.Image = global::iba.Properties.Resources.copy;
            this.buttonEndpointCopy.Name = "buttonEndpointCopy";
            this.buttonEndpointCopy.TabStop = false;
            this.buttonEndpointCopy.UseVisualStyleBackColor = true;
            this.buttonEndpointCopy.Click += new System.EventHandler(this.buttonEndpointCopy_Click);
            // 
            // buttonEndpointAdd
            // 
            resources.ApplyResources(this.buttonEndpointAdd, "buttonEndpointAdd");
            this.buttonEndpointAdd.Image = global::iba.Properties.Resources.img_add;
            this.buttonEndpointAdd.Name = "buttonEndpointAdd";
            this.buttonEndpointAdd.TabStop = false;
            this.buttonEndpointAdd.UseVisualStyleBackColor = true;
            this.buttonEndpointAdd.Click += new System.EventHandler(this.buttonEndpointAdd_Click);
            // 
            // cbEnabled
            // 
            resources.ApplyResources(this.cbEnabled, "cbEnabled");
            this.cbEnabled.Name = "cbEnabled";
            this.cbEnabled.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.AllowDragReorder = false;
            this.tabControl1.Appearance = Crownwood.DotNetMagic.Controls.VisualAppearance.MultiDocument;
            resources.ApplyResources(this.tabControl1, "tabControl1");
            this.tabControl1.MediaPlayerDockSides = false;
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.OfficeDockSides = false;
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.ShowArrows = false;
            this.tabControl1.ShowClose = false;
            this.tabControl1.ShowDropSelect = false;
            this.tabControl1.Style = Crownwood.DotNetMagic.Common.VisualStyle.IDE2005;
            this.tabControl1.TabPages.AddRange(new Crownwood.DotNetMagic.Controls.TabPage[] {
            this.tabConfiguration,
            this.tabCertificates,
            this.tabTags,
            this.tabDiag});
            this.tabControl1.TextTips = true;
            this.tabControl1.SelectionChanged += new Crownwood.DotNetMagic.Controls.SelectTabHandler(this.tabControl1_SelectionChanged);
            // 
            // tabConfiguration
            // 
            resources.ApplyResources(this.tabConfiguration, "tabConfiguration");
            this.tabConfiguration.Controls.Add(this.gbEndpoints);
            this.tabConfiguration.Controls.Add(this.gbSecurity);
            this.tabConfiguration.Controls.Add(this.cbEnabled);
            this.tabConfiguration.Controls.Add(this.gbLogon);
            this.tabConfiguration.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabConfiguration.Name = "tabConfiguration";
            this.tabConfiguration.SelectBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabConfiguration.SelectTextColor = System.Drawing.Color.Empty;
            // 
            // tabCertificates
            // 
            resources.ApplyResources(this.tabCertificates, "tabCertificates");
            this.tabCertificates.Controls.Add(this.gridControlCerts);
            this.tabCertificates.Controls.Add(this.toolStrip1);
            this.tabCertificates.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabCertificates.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabCertificates.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabCertificates.Name = "tabCertificates";
            this.tabCertificates.SelectBackColor = System.Drawing.Color.Empty;
            this.tabCertificates.Selected = false;
            this.tabCertificates.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabCertificates.SelectTextColor = System.Drawing.Color.Empty;
            // 
            // tabTags
            // 
            resources.ApplyResources(this.tabTags, "tabTags");
            this.tabTags.Controls.Add(this.panelTagsFooter);
            this.tabTags.Controls.Add(this.buttonRefreshGuiTree);
            this.tabTags.Controls.Add(this.buttonRebuildTree);
            this.tabTags.Controls.Add(this.tvObjects);
            this.tabTags.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabTags.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabTags.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabTags.Name = "tabTags";
            this.tabTags.SelectBackColor = System.Drawing.Color.Empty;
            this.tabTags.Selected = false;
            this.tabTags.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabTags.SelectTextColor = System.Drawing.Color.Empty;
            // 
            // tabDiag
            // 
            resources.ApplyResources(this.tabDiag, "tabDiag");
            this.tabDiag.Controls.Add(this.btOpenLogFile);
            this.tabDiag.Controls.Add(this.gridCtrlSubscriptions);
            this.tabDiag.Controls.Add(this.label3);
            this.tabDiag.Controls.Add(this.gridCtrlSessions);
            this.tabDiag.Controls.Add(this.label12);
            this.tabDiag.InactiveBackColor = System.Drawing.Color.Empty;
            this.tabDiag.InactiveTextBackColor = System.Drawing.Color.Empty;
            this.tabDiag.InactiveTextColor = System.Drawing.Color.Empty;
            this.tabDiag.Name = "tabDiag";
            this.tabDiag.SelectBackColor = System.Drawing.Color.Empty;
            this.tabDiag.Selected = false;
            this.tabDiag.SelectTextBackColor = System.Drawing.Color.Empty;
            this.tabDiag.SelectTextColor = System.Drawing.Color.Empty;
            // 
            // btOpenLogFile
            // 
            resources.ApplyResources(this.btOpenLogFile, "btOpenLogFile");
            this.btOpenLogFile.Name = "btOpenLogFile";
            this.btOpenLogFile.UseVisualStyleBackColor = true;
            this.btOpenLogFile.Click += new System.EventHandler(this.btOpenLogFile_Click);
            // 
            // gridCtrlSubscriptions
            // 
            resources.ApplyResources(this.gridCtrlSubscriptions, "gridCtrlSubscriptions");
            this.gridCtrlSubscriptions.MainView = this.gridViewSubscriptions;
            this.gridCtrlSubscriptions.Name = "gridCtrlSubscriptions";
            this.gridCtrlSubscriptions.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewSubscriptions});
            // 
            // gridViewSubscriptions
            // 
            this.gridViewSubscriptions.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewSubscriptions.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewSubscriptions.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.gridColumn1,
            this.colSubscrMonItemCount,
            this.colSubscrPublInterval,
            this.colSubscrNextSeqNr});
            this.gridViewSubscriptions.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridViewSubscriptions.GridControl = this.gridCtrlSubscriptions;
            this.gridViewSubscriptions.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridViewSubscriptions.Name = "gridViewSubscriptions";
            this.gridViewSubscriptions.OptionsBehavior.AutoPopulateColumns = false;
            this.gridViewSubscriptions.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewSubscriptions.OptionsBehavior.AutoUpdateTotalSummary = false;
            this.gridViewSubscriptions.OptionsBehavior.Editable = false;
            this.gridViewSubscriptions.OptionsBehavior.KeepGroupExpandedOnSorting = false;
            this.gridViewSubscriptions.OptionsCustomization.AllowFilter = false;
            this.gridViewSubscriptions.OptionsCustomization.AllowGroup = false;
            this.gridViewSubscriptions.OptionsCustomization.AllowSort = false;
            this.gridViewSubscriptions.OptionsDetail.AllowZoomDetail = false;
            this.gridViewSubscriptions.OptionsDetail.EnableMasterViewMode = false;
            this.gridViewSubscriptions.OptionsMenu.EnableColumnMenu = false;
            this.gridViewSubscriptions.OptionsMenu.EnableFooterMenu = false;
            this.gridViewSubscriptions.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewSubscriptions.OptionsNavigation.AutoMoveRowFocus = false;
            this.gridViewSubscriptions.OptionsNavigation.UseTabKey = false;
            this.gridViewSubscriptions.OptionsPrint.AutoWidth = false;
            this.gridViewSubscriptions.OptionsPrint.ExpandAllGroups = false;
            this.gridViewSubscriptions.OptionsPrint.PrintFooter = false;
            this.gridViewSubscriptions.OptionsPrint.PrintGroupFooter = false;
            this.gridViewSubscriptions.OptionsPrint.PrintHeader = false;
            this.gridViewSubscriptions.OptionsPrint.PrintHorzLines = false;
            this.gridViewSubscriptions.OptionsPrint.PrintVertLines = false;
            this.gridViewSubscriptions.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewSubscriptions.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewSubscriptions.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridViewSubscriptions.OptionsView.ShowDetailButtons = false;
            this.gridViewSubscriptions.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewSubscriptions.OptionsView.ShowGroupPanel = false;
            this.gridViewSubscriptions.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            // 
            // gridColumn1
            // 
            resources.ApplyResources(this.gridColumn1, "gridColumn1");
            this.gridColumn1.FieldName = "Id";
            this.gridColumn1.Name = "gridColumn1";
            // 
            // colSubscrMonItemCount
            // 
            resources.ApplyResources(this.colSubscrMonItemCount, "colSubscrMonItemCount");
            this.colSubscrMonItemCount.FieldName = "MonitoredItemCount";
            this.colSubscrMonItemCount.Name = "colSubscrMonItemCount";
            // 
            // colSubscrPublInterval
            // 
            resources.ApplyResources(this.colSubscrPublInterval, "colSubscrPublInterval");
            this.colSubscrPublInterval.DisplayFormat.FormatString = "{0} ms";
            this.colSubscrPublInterval.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
            this.colSubscrPublInterval.FieldName = "PublishingInterval";
            this.colSubscrPublInterval.Name = "colSubscrPublInterval";
            // 
            // colSubscrNextSeqNr
            // 
            resources.ApplyResources(this.colSubscrNextSeqNr, "colSubscrNextSeqNr");
            this.colSubscrNextSeqNr.FieldName = "NextSequenceNumber";
            this.colSubscrNextSeqNr.Name = "colSubscrNextSeqNr";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // gridCtrlSessions
            // 
            resources.ApplyResources(this.gridCtrlSessions, "gridCtrlSessions");
            this.gridCtrlSessions.MainView = this.gridViewSessions;
            this.gridCtrlSessions.Name = "gridCtrlSessions";
            this.gridCtrlSessions.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridViewSessions});
            // 
            // gridViewSessions
            // 
            this.gridViewSessions.Appearance.Row.Options.UseTextOptions = true;
            this.gridViewSessions.Appearance.Row.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
            this.gridViewSessions.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
            this.colSessionName,
            this.colSessionID,
            this.colSessionLastMsgTime});
            this.gridViewSessions.FocusRectStyle = DevExpress.XtraGrid.Views.Grid.DrawFocusRectStyle.None;
            this.gridViewSessions.GridControl = this.gridCtrlSessions;
            this.gridViewSessions.HorzScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            this.gridViewSessions.Name = "gridViewSessions";
            this.gridViewSessions.OptionsBehavior.AutoPopulateColumns = false;
            this.gridViewSessions.OptionsBehavior.AutoSelectAllInEditor = false;
            this.gridViewSessions.OptionsBehavior.AutoUpdateTotalSummary = false;
            this.gridViewSessions.OptionsBehavior.Editable = false;
            this.gridViewSessions.OptionsBehavior.KeepGroupExpandedOnSorting = false;
            this.gridViewSessions.OptionsCustomization.AllowFilter = false;
            this.gridViewSessions.OptionsCustomization.AllowGroup = false;
            this.gridViewSessions.OptionsCustomization.AllowSort = false;
            this.gridViewSessions.OptionsDetail.AllowZoomDetail = false;
            this.gridViewSessions.OptionsDetail.EnableMasterViewMode = false;
            this.gridViewSessions.OptionsMenu.EnableColumnMenu = false;
            this.gridViewSessions.OptionsMenu.EnableFooterMenu = false;
            this.gridViewSessions.OptionsMenu.EnableGroupPanelMenu = false;
            this.gridViewSessions.OptionsNavigation.AutoMoveRowFocus = false;
            this.gridViewSessions.OptionsNavigation.UseTabKey = false;
            this.gridViewSessions.OptionsPrint.AutoWidth = false;
            this.gridViewSessions.OptionsPrint.ExpandAllGroups = false;
            this.gridViewSessions.OptionsPrint.PrintFooter = false;
            this.gridViewSessions.OptionsPrint.PrintGroupFooter = false;
            this.gridViewSessions.OptionsPrint.PrintHeader = false;
            this.gridViewSessions.OptionsPrint.PrintHorzLines = false;
            this.gridViewSessions.OptionsPrint.PrintVertLines = false;
            this.gridViewSessions.OptionsSelection.EnableAppearanceFocusedCell = false;
            this.gridViewSessions.OptionsSelection.EnableAppearanceFocusedRow = false;
            this.gridViewSessions.OptionsSelection.EnableAppearanceHideSelection = false;
            this.gridViewSessions.OptionsView.ShowDetailButtons = false;
            this.gridViewSessions.OptionsView.ShowFilterPanelMode = DevExpress.XtraGrid.Views.Base.ShowFilterPanelMode.Never;
            this.gridViewSessions.OptionsView.ShowGroupPanel = false;
            this.gridViewSessions.ScrollStyle = DevExpress.XtraGrid.Views.Grid.ScrollStyleFlags.LiveVertScroll;
            this.gridViewSessions.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridViewSessions_CustomDrawCell);
            this.gridViewSessions.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridViewSessions_FocusedRowChanged);
            // 
            // colSessionName
            // 
            resources.ApplyResources(this.colSessionName, "colSessionName");
            this.colSessionName.FieldName = "Name";
            this.colSessionName.Name = "colSessionName";
            // 
            // colSessionID
            // 
            resources.ApplyResources(this.colSessionID, "colSessionID");
            this.colSessionID.FieldName = "Id";
            this.colSessionID.Name = "colSessionID";
            // 
            // colSessionLastMsgTime
            // 
            resources.ApplyResources(this.colSessionLastMsgTime, "colSessionLastMsgTime");
            this.colSessionLastMsgTime.FieldName = "LastMessageTimeString";
            this.colSessionLastMsgTime.Name = "colSessionLastMsgTime";
            // 
            // label12
            // 
            resources.ApplyResources(this.label12, "label12");
            this.label12.Name = "label12";
            // 
            // gbDebug
            // 
            this.gbDebug.Controls.Add(this.tbDiagTmp);
            resources.ApplyResources(this.gbDebug, "gbDebug");
            this.gbDebug.Name = "gbDebug";
            this.gbDebug.TabStop = false;
            // 
            // gbStatus
            // 
            this.gbStatus.Controls.Add(this.tbStatus);
            resources.ApplyResources(this.gbStatus, "gbStatus");
            this.gbStatus.Name = "gbStatus";
            this.gbStatus.TabStop = false;
            // 
            // OpcUaControl
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.gbDebug);
            this.Controls.Add(this.gbStatus);
            this.Controls.Add(this.panelFooterWithButtons);
            this.Name = "OpcUaControl";
            this.Load += new System.EventHandler(this.OpcUaControl_Load);
            this.splitContainerObjectsFooter.Panel1.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel1.PerformLayout();
            this.splitContainerObjectsFooter.Panel2.ResumeLayout(false);
            this.splitContainerObjectsFooter.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainerObjectsFooter)).EndInit();
            this.splitContainerObjectsFooter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemSpinEdit1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridControlCerts)).EndInit();
            this.contextMenuCerts.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridViewCerts)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.panelTagsFooter.ResumeLayout(false);
            this.panelTagsFooter.PerformLayout();
            this.panelFooterWithButtons.ResumeLayout(false);
            this.gbSecurity.ResumeLayout(false);
            this.gbSecurity.PerformLayout();
            this.gbLogon.ResumeLayout(false);
            this.gbLogon.PerformLayout();
            this.gbEndpoints.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlEndpoints)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewEndpoints)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabConfiguration.ResumeLayout(false);
            this.tabConfiguration.PerformLayout();
            this.tabCertificates.ResumeLayout(false);
            this.tabCertificates.PerformLayout();
            this.tabTags.ResumeLayout(false);
            this.tabDiag.ResumeLayout(false);
            this.tabDiag.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlSubscriptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSubscriptions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridCtrlSessions)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridViewSessions)).EndInit();
            this.gbDebug.ResumeLayout(false);
            this.gbDebug.PerformLayout();
            this.gbStatus.ResumeLayout(false);
            this.gbStatus.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TreeView tvObjects;
        private System.Windows.Forms.CheckBox cbEnabled;
        private System.Windows.Forms.Button buttonConfigurationReset;
        private System.Windows.Forms.Button buttonConfigurationApply;
        private System.Windows.Forms.TextBox tbUserName;
        private System.Windows.Forms.TextBox tbStatus;
        private System.Windows.Forms.Timer timerRefreshStatus;
        private System.Windows.Forms.Button buttonEndpointCopy;
        private System.Windows.Forms.Button buttonEndpointRemove;
        private System.Windows.Forms.Button buttonEndpointAdd;
        private System.Windows.Forms.GroupBox gbLogon;
        private System.Windows.Forms.CheckBox cbLogonCertificate;
        private System.Windows.Forms.GroupBox gbSecurity;
        private System.Windows.Forms.ComboBox comboBoxSecurity128;
        private System.Windows.Forms.CheckBox cbSecurity256;
        private System.Windows.Forms.CheckBox cbSecurity128;
        private System.Windows.Forms.CheckBox cbSecurityNone;
        private System.Windows.Forms.ComboBox comboBoxSecurity256;
        private System.Windows.Forms.Button buttonRebuildTree;
        private System.Windows.Forms.Button buttonRefreshGuiTree;
        private System.Windows.Forms.Label labelObjNodeId;
        private System.Windows.Forms.TextBox tbObjNodeId;
        private System.Windows.Forms.Panel panelTagsFooter;
        private System.Windows.Forms.SplitContainer splitContainerObjectsFooter;
        private System.Windows.Forms.Label labelObjType;
        private System.Windows.Forms.TextBox tbObjType;
        private System.Windows.Forms.Label labelObjValue;
        private System.Windows.Forms.TextBox tbObjValue;
        private System.Windows.Forms.Label labelObjDescription;
        private System.Windows.Forms.TextBox tbObjDescription;
        private System.Windows.Forms.TextBox tbDiagTmp;
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
        private Crownwood.DotNetMagic.Controls.TabControl tabControl1;
        private Crownwood.DotNetMagic.Controls.TabPage tabConfiguration;
        private System.Windows.Forms.GroupBox gbEndpoints;
        protected DevExpress.XtraGrid.GridControl gridCtrlEndpoints;
        protected DevExpress.XtraGrid.Views.Grid.GridView gridViewEndpoints;
        private DevExpress.XtraGrid.Columns.GridColumn colHostname;
        private DevExpress.XtraGrid.Columns.GridColumn colPort;
        private DevExpress.XtraGrid.Columns.GridColumn colURI;
        private Crownwood.DotNetMagic.Controls.TabPage tabCertificates;
        private Crownwood.DotNetMagic.Controls.TabPage tabTags;
        private Crownwood.DotNetMagic.Controls.TabPage tabDiag;
        private System.Windows.Forms.Button btOpenLogFile;
        protected DevExpress.XtraGrid.Views.Grid.GridView gridViewSubscriptions;
        protected DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
        protected DevExpress.XtraGrid.Columns.GridColumn colSubscrMonItemCount;
        protected DevExpress.XtraGrid.Columns.GridColumn colSubscrPublInterval;
        protected DevExpress.XtraGrid.Columns.GridColumn colSubscrNextSeqNr;
        private System.Windows.Forms.Label label3;
        protected DevExpress.XtraGrid.GridControl gridCtrlSessions;
        protected DevExpress.XtraGrid.Views.Grid.GridView gridViewSessions;
        protected DevExpress.XtraGrid.Columns.GridColumn colSessionName;
        protected DevExpress.XtraGrid.Columns.GridColumn colSessionID;
        protected DevExpress.XtraGrid.Columns.GridColumn colSessionLastMsgTime;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.GroupBox gbDebug;
        private DevExpress.XtraGrid.GridControl gridCtrlSubscriptions;
        private System.Windows.Forms.Panel panelFooterWithButtons;
        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit repositoryItemSpinEdit1;
        private System.Windows.Forms.GroupBox gbStatus;
        private System.Windows.Forms.Button buttonShowHideDebug;
        private PasswordEditWithConfirmation tbPassword;
    }
}
