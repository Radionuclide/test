using System;
using System.ComponentModel;
using System.Windows.Forms;
using iba.Data;
using Confluent.Kafka;
using DevExpress.XtraGrid.Views.Grid;
using iba.Utility;
using System.IO;
using System.Collections.Generic;
using DevExpress.XtraEditors.Controls;
using DevExpress.Utils;
using iba.Remoting;
using IbaAnalyzer;
using iba.CertificateStore.Forms;
using iba.CertificateStore;
using iba.CertificateStore.Proxy;
using System.Security.Cryptography.X509Certificates;
using System.Linq;
using System.Drawing;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Columns;
using DevExpress.Data.Extensions;

namespace iba.Controls
{
    internal partial class KafkaWriterTaskControl : UserControl, IPropertyPane, ICertificatesControlHost
    {
        readonly BindingList<KafkaWriterTaskData.KafkaRecord> _expressionTableData;
        readonly BindingList<KafkaWriterTaskData.Param> _paramTableData;
        private readonly AnalyzerManager _analyzerManager;
        private KafkaWriterTaskData _data;
        private readonly GridView _viewExpr;
        CertificatesComboBox clientCertCBox, CACertCBox, schemaClientCertCBox, schemaCACertCBox;
        CertificateInfo CACertParams, schemaCACertParams;
        CertificateInfoWithPrivateKey clientCertParams, schemaClientCertParams;
        IPropertyPaneManager m_manager;
        HidebleControlBlock clusterSSL, clusterSASL, schemaSSL, schemaAuth;
        InterfaceModeSwitcher _modeSwitcher;
        #region ICertificatesControlHost
        public bool IsLocalHost { get; }
        public string ServerAddress { get; }
        public ICertificateManagerProxy CertificateManagerProxy { get; } = new CertificateManagerProxyJsonAdapter(new AppCertificateManagerJsonProxy());
        public bool IsCertificatesReadonly => false;
        public bool IsReadOnly => false; // set to true in case of user restriction
        public string UsagePart { get; } = "EX"; // IO and DS are used in PDA
        public IWin32Window Instance => this;
        public ContextMenuStrip PopupMenu { get; } = new ContextMenuStrip(); // or reuse the context menu of an other control
        #endregion

        private class HidebleControlBlock
        {
            List<Control> controls = new List<Control>();
            List<Control> underlyingControls = new List<Control>();
            int height;
            bool hidden = false;
            public HidebleControlBlock(params Control[] controlsParam)
            {
                this.controls.AddRange(controlsParam);
                int topPos = controls.Min(c => c.Location.Y);
                int lowestPos = controls.Max(c => c.Location.Y);
                foreach (Control c in controls[0].Parent.Controls)
                {
                    int pos = c.Location.Y;
                    if (pos > lowestPos && c.Anchor.HasFlag(AnchorStyles.Top))
                        underlyingControls.Add(c);
                }
                int underlyingControlPos = underlyingControls.Min(c => c.Location.Y);

                height = underlyingControlPos - topPos;
            }

            public void Hide()
            {
                if (!hidden)
                {
                    hidden = true;
                    var controlsToMove = underlyingControls.Where(c => !c.Anchor.HasFlag(AnchorStyles.Bottom));
                    foreach (var c in controlsToMove)
                        c.Top -= height;
                    var controlToResize = underlyingControls.Where(c => c.Anchor.HasFlag(AnchorStyles.Bottom));
                    foreach (var c in controlToResize)
                    {
                        c.Top -= height;
                        c.Height += height;
                    }
                }
                foreach (var c in controls)
                    c.Hide();
            }

            public void Show()
            {
                if (hidden)
                {
                    hidden = false;
                    var controlsToMove = underlyingControls.Where(c => !c.Anchor.HasFlag(AnchorStyles.Bottom));
                    foreach (var c in controlsToMove)
                        c.Top += height;
                    var controlToResize = underlyingControls.Where(c => c.Anchor.HasFlag(AnchorStyles.Bottom));
                    foreach (var c in controlToResize)
                    {
                        c.Top += height;
                        c.Height -= height;
                    }
                }
                foreach (var c in controls)
                    c.Show();
            }
        }

        // Switch interface layout between Kafka and EventHub
        private class InterfaceModeSwitcher
        {
            KafkaWriterTaskData.ClusterType currentlySelectedType;
            List<Control> ctrlsToHideInEventHubMode; //controls not used in Event Hub mode
            KafkaWriterTaskControl parent;
            List<Point> controlPositions = new List<Point>();

            public InterfaceModeSwitcher(KafkaWriterTaskControl ctrl)
            {
                parent = ctrl;
                currentlySelectedType = KafkaWriterTaskData.ClusterType.Kafka;
                var ctrlsToNotHideInEventHubMode = new Control[] { parent.idLabel, parent.identifierTextBox, parent.typeLabel, parent.testConnectionBtn, parent.addressTextBox, parent.clusterTypeComboBox, parent.enableSSLVerificationCb, parent.CACertificateLabel, parent.CACertCBox, parent.messageTimeoutLabel, parent.timeoutNumericUpDown, parent.connectionStrLabel };
                ctrlsToHideInEventHubMode = parent.tabControl1.TabPages[0].Controls.Cast<Control>().Where(c => !ctrlsToNotHideInEventHubMode.Contains(c)).ToList<Control>();

            }

            // Kafka mode is selected by default.
            // When mode changes to Event Hub, we remember control locations and restore them when change to Kafka
            public void SwitchInterface(KafkaWriterTaskData.ClusterType type)
            {
                if (currentlySelectedType == type)
                    return;
                currentlySelectedType = type;
                parent.SuspendLayout();
                var controlsToMoveEventHub = new Control[] { parent.enableSSLVerificationCb, parent.CACertificateLabel, parent.CACertCBox, parent.messageTimeoutLabel, parent.timeoutNumericUpDown, parent.secLabel };
                var pos = parent.clusterConnSecurityLabel.Location;
                if (type == KafkaWriterTaskData.ClusterType.Kafka)
                {
                    parent.connectionStrLabel.Visible = false;
                    parent.clusterAddressLabel.Visible = true;
                    foreach (Control c in ctrlsToHideInEventHubMode)
                        c.Visible = true;

                    for (int i = 0; i < controlPositions.Count; i++)
                        controlsToMoveEventHub[i].Location = controlPositions[i];
                    parent.CACertCBox.Width = parent.clientCertCBox.Width;
                    parent.clusterConnectionSecurityComboBox_SelectedIndexChanged(null, null);
                    parent.schemaRegistryConnectionSecurityComboBox_SelectedIndexChanged(null, null);
                }
                else // event hub
                {
                    controlPositions.Clear();
                    foreach (var c in controlsToMoveEventHub)
                        controlPositions.Add(c.Location);
                    parent.connectionStrLabel.Location = parent.clusterAddressLabel.Location;
                    parent.connectionStrLabel.Visible = true;
                    parent.clusterAddressLabel.Visible = false;

                    var Xshift = parent.clusterConnSecurityLabel.Location.X - parent.enableSSLVerificationCb.Location.X;
                    var Yshift = parent.clusterConnSecurityLabel.Location.Y - parent.enableSSLVerificationCb.Location.Y;
                    parent.enableSSLVerificationCb.Location = new Point(parent.enableSSLVerificationCb.Location.X + Xshift, parent.enableSSLVerificationCb.Location.Y + Yshift);
                    parent.CACertificateLabel.Location = new Point(parent.CACertificateLabel.Location.X + Xshift, parent.CACertificateLabel.Location.Y + Yshift);
                    parent.CACertCBox.Location = new Point(parent.identifierTextBox.Location.X, parent.CACertCBox.Location.Y + Yshift);
                    parent.CACertCBox.Width = parent.identifierTextBox.Width;
                    Yshift = parent.SASLPassLabel.Location.Y - parent.messageTimeoutLabel.Location.Y;


                    parent.messageTimeoutLabel.Location = new Point(parent.messageTimeoutLabel.Location.X, parent.messageTimeoutLabel.Location.Y + Yshift);
                    parent.timeoutNumericUpDown.Location = new Point(parent.timeoutNumericUpDown.Location.X, parent.timeoutNumericUpDown.Location.Y + Yshift);
                    parent.secLabel.Location = new Point(parent.secLabel.Location.X, parent.secLabel.Location.Y + Yshift);

                    foreach (Control c in ctrlsToHideInEventHubMode)
                        c.Visible = false;
                    foreach (Control c in controlsToMoveEventHub)
                        c.Visible = true;

                }
                parent.ResumeLayout();
            }
        }

        public KafkaWriterTaskControl()
        {
            InitializeComponent();

            tabControl1.SelectedIndex = 0;

            _analyzerManager = new AnalyzerManager();
            _expressionTableData = new BindingList<KafkaWriterTaskData.KafkaRecord>();
            exprGrid.DataSource = _expressionTableData;
            _viewExpr = (GridView)exprGrid.MainView;
            _viewExpr.FocusedRowChanged += (sender, e) => UpdateExprTableButtons();

            expressionGridColumn.View.CellValueChanged += CellExpressionChanged;

            var channelEditor = new RepositoryItemChannelTreeEdit(_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Text);
            channelEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            exprGrid.RepositoryItems.Add(channelEditor);
            expressionGridColumn.ColumnEdit = channelEditor;

            metadataComboBox.Properties.Items.Add("Unit");
            metadataComboBox.Properties.Items.Add("Comment 1");
            metadataComboBox.Properties.Items.Add("Comment 2");
            metadataComboBox.Properties.Items.Add("Name");
            metadataComboBox.Properties.Items.Add("Identifier");

            var typeComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            foreach (var t in KafkaWriterTaskData.KafkaRecord.DataTypes)
                typeComboBox.Items.Add(t);
            dataTypeGridColumn.ColumnEdit = typeComboBox;

            _paramTableData = new BindingList<KafkaWriterTaskData.Param>();
            paramGrid.DataSource = _paramTableData;
            _viewParam = (GridView)paramGrid.MainView;
            _viewParam.FocusedRowChanged += (sender, e) => UpdateParamTableButtons();

            keyGridColumn.Caption = Properties.Resources.Key;
            valGridColumn.Caption = Properties.Resources.Value;
            dataTypeGridColumn.Caption = Properties.Resources.DataType;
            expressionGridColumn.Caption = Properties.Resources.ibaAnalyzerExpression;
            testValueGridColumn.Caption = Properties.Resources.TestValue;
            nameGridColumn.Caption = Properties.Resources.Name;
            keyTextBox.GotFocus += ShowToolTipOnFocus;
            keyTextBox.LostFocus += OnLostFocusHideTooltip;
            signalRefTextBox.GotFocus += ShowToolTipOnFocus;
            signalRefTextBox.LostFocus += OnLostFocusHideTooltip;
            tabConnection.Click += OnLostFocusHideTooltip;


            _toolTip.SetToolTip(importParamButton, iba.Properties.Resources.ImportParametersFromCSV);
            _toolTip.SetToolTip(exportParamButton, iba.Properties.Resources.ExportParametersToCSV);

            clientCertCBox = CertificatesComboBox.ReplaceCombobox(clientCertPlaceholder, "", false);
            CACertCBox = CertificatesComboBox.ReplaceCombobox(CACertPlaceholder, "", false);
            schemaClientCertCBox = CertificatesComboBox.ReplaceCombobox(schemaClientCertPlaceholder, "", false);
            schemaCACertCBox = CertificatesComboBox.ReplaceCombobox(schemaCACertPlaceholder, "", false);

            clusterSSL = new HidebleControlBlock(clientCertificateLabel, enableSSLVerificationCb, CACertificateLabel, clientCertCBox, CACertCBox);
            clusterSASL = new HidebleControlBlock(SASLMechLabel, SASLMechanismComboBox, SASLNameTextBox, SASLPassTextBox, SASLNameLabel, SASLPassLabel);
            schemaSSL = new HidebleControlBlock(schemaClientCertificateLabel, schemaClientCertCBox, schemaEnableSSLVerificationCb, schemaCACertificateLabel, schemaCACertCBox);
            schemaAuth = new HidebleControlBlock(schemaNameLabel, schemaNameTextBox, schemaPassLabel, schemaPassTextBox);

            _modeSwitcher = new InterfaceModeSwitcher(this);
        }

        class CertificateInfo : ICertificateInfo
        {
            public string Thumbprint { get; set; }

            public CertificateRequirement CertificateRequirements { get; } =
                CertificateRequirement.Trusted |
                CertificateRequirement.Valid;

            public string DisplayName { get; } = "Cert for Kafka";
        }

        class CertificateInfoWithPrivateKey : ICertificateInfo
        {
            public string Thumbprint { get; set; }

            public CertificateRequirement CertificateRequirements { get; } =
                CertificateRequirement.Trusted |
                CertificateRequirement.Valid |
                CertificateRequirement.PrivateKey;

            public string DisplayName { get; } = "Cert for Kafka";
        }

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            _data = datasource as KafkaWriterTaskData;

            clientCertParams = new CertificateInfoWithPrivateKey();
            clientCertParams.Thumbprint = _data.SSLClientThumbprint;
            clientCertCBox.UnsetEnvironment();
            clientCertCBox.SetEnvironment(this, clientCertParams);

            CACertParams = new CertificateInfo();
            CACertParams.Thumbprint = _data.SSLCAThumbprint;
            CACertCBox.UnsetEnvironment();
            CACertCBox.SetEnvironment(this, CACertParams);

            schemaClientCertParams = new CertificateInfoWithPrivateKey();
            schemaClientCertParams.Thumbprint = _data.schemaSSLClientThumbprint;
            schemaClientCertCBox.UnsetEnvironment();
            schemaClientCertCBox.SetEnvironment(this, schemaClientCertParams);

            schemaCACertParams = new CertificateInfo();
            schemaCACertParams.Thumbprint = _data.schemaSSLCAThumbprint;
            schemaCACertCBox.UnsetEnvironment();
            schemaCACertCBox.SetEnvironment(this, schemaCACertParams);

            SASLNameTextBox.Text = _data.SASLUsername;
            SASLPassTextBox.Text = _data.SASLPass;
            enableSSLVerificationCb.Checked = _data.enableSSLVerification;
            enableSSLVerificationCb_CheckedChanged(null, null);

            schemaRegistryCb.Checked = _data.enableSchema;
            schemaRegistryCb_CheckedChanged(null, null);

            schemaNameTextBox.Text = _data.schemaUsername;
            schemaPassTextBox.Text = _data.schemaPass;
            schemaEnableSSLVerificationCb.Checked = _data.schemaEnableSSLVerification;
            schemaEnableSSLVerificationCb_CheckedChanged(null, null);

            m_pdoFileTextBox.Text = _data.AnalysisFile;
            m_datFileTextBox.Text = _data.TestDatFile;
            keyTextBox.Text = _data.key;
            signalRefTextBox.Text = _data.signalReference;
            _expressionTableData.Clear();
            foreach (var rec in _data.Records)
                _expressionTableData.Add((KafkaWriterTaskData.KafkaRecord)rec.Clone());
            _paramTableData.Clear();
            foreach (var par in _data.Params)
                _paramTableData.Add((KafkaWriterTaskData.Param)par.Clone());

            foreach (CheckedListBoxItem i in metadataComboBox.Properties.Items)
                i.CheckState = _data.metadata.Contains(i.ToString()) ? CheckState.Checked : CheckState.Unchecked;

            addressTextBox.Text = _data.clusterAddress;
            schemaTextBox.Text = _data.schemaRegistryAddress;
            topicComboBox.Items.Clear();
            topicComboBox.Text = _data.topicName;
            digitalFormatComboBox.SelectedIndex = (int)_data.digitalFormat;
            timeoutNumericUpDown.Value =
                new Decimal(
                Math.Min(
                    Math.Max(_data.timeout, Decimal.ToDouble(timeoutNumericUpDown.Minimum)),
                    Decimal.ToDouble(timeoutNumericUpDown.Maximum))
            );
            identifierTextBox.Text = _data.identifier;
            switch (_data.Format)
            {
                case KafkaWriterTaskData.DataFormat.JSONGrouped:
                    dataFormatComboBox.SelectedIndex = 0;
                    break;
                case KafkaWriterTaskData.DataFormat.JSONPerSignal:
                    dataFormatComboBox.SelectedIndex = 1;
                    break;
                case KafkaWriterTaskData.DataFormat.AVRO:
                    dataFormatComboBox.SelectedIndex = 2;
                    break;
            }

            switch (_data.AckMode)
            {
                case KafkaWriterTaskData.RequiredAcks.None:
                    acknowledgmentComboBox.SelectedIndex = 0;
                    break;
                case KafkaWriterTaskData.RequiredAcks.Leader:
                    acknowledgmentComboBox.SelectedIndex = 1;
                    break;
                case KafkaWriterTaskData.RequiredAcks.All:
                    acknowledgmentComboBox.SelectedIndex = 2;
                    break;
            }

            switch (_data.ClusterSecurityMode)
            {
                case KafkaWriterTaskData.ClusterSecurityType.PLAINTEXT:
                    clusterConnectionSecurityComboBox.SelectedIndex = 0;
                    break;
                case KafkaWriterTaskData.ClusterSecurityType.SSL:
                    clusterConnectionSecurityComboBox.SelectedIndex = 1;
                    break;
                case KafkaWriterTaskData.ClusterSecurityType.SASL_PLAINTEXT:
                    clusterConnectionSecurityComboBox.SelectedIndex = 2;
                    break;
                case KafkaWriterTaskData.ClusterSecurityType.SASL_SSL:
                    clusterConnectionSecurityComboBox.SelectedIndex = 3;
                    break;
            }

            switch (_data.SchemaRegistrySecurityMode)
            {
                case KafkaWriterTaskData.SchemaRegistrySecurityType.HTTP:
                    schemaRegistryConnectionSecurityComboBox.SelectedIndex = 0;
                    break;
                case KafkaWriterTaskData.SchemaRegistrySecurityType.HTTPS:
                    schemaRegistryConnectionSecurityComboBox.SelectedIndex = 1;
                    break;
                case KafkaWriterTaskData.SchemaRegistrySecurityType.HTTP_AUTHENTICATION:
                    schemaRegistryConnectionSecurityComboBox.SelectedIndex = 2;
                    break;
                case KafkaWriterTaskData.SchemaRegistrySecurityType.HTTPS_AUTHENTICATION:
                    schemaRegistryConnectionSecurityComboBox.SelectedIndex = 3;
                    break;
            }

            switch (_data.SASLMechanismMode)
            {
                case KafkaWriterTaskData.SASLMechanismType.PLAIN:
                    SASLMechanismComboBox.SelectedIndex = 0;
                    break;
                case KafkaWriterTaskData.SASLMechanismType.SCRAM_SHA_256:
                    SASLMechanismComboBox.SelectedIndex = 1;
                    break;
                case KafkaWriterTaskData.SASLMechanismType.SCRAM_SHA_512:
                    SASLMechanismComboBox.SelectedIndex = 2;
                    break;
            }

            switch (_data.ClusterMode)
            {
                case KafkaWriterTaskData.ClusterType.Kafka:
                    clusterTypeComboBox.SelectedIndex = 0;
                    break;
                case KafkaWriterTaskData.ClusterType.EventHub:
                    clusterTypeComboBox.SelectedIndex = 1;
                    break;
            }

            UpdateSource();
            UpdateExprTableButtons();
            UpdateParamTableButtons();

            m_cbMemory.Checked = _data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = _data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, _data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(_data.MonitorData.TimeLimit.TotalMinutes, 1));
        }

        public void LeaveCleanup() { }

        public void SaveData()
        {
            _data.Records.Clear();
            foreach (var rec in _expressionTableData)
                if (!String.IsNullOrWhiteSpace(rec.Expression))
                    _data.Records.Add(rec);

            _data.Params.Clear();
            foreach (var par in _paramTableData)
                if (!String.IsNullOrWhiteSpace(par.Key))
                    _data.Params.Add(par);

            _data.clusterAddress = addressTextBox.Text;
            _data.schemaRegistryAddress = schemaTextBox.Text;
            _data.topicName = topicComboBox.Text;
            _data.timeout = Decimal.ToDouble(timeoutNumericUpDown.Value);
            _data.identifier = identifierTextBox.Text;
            _data.digitalFormat = (KafkaWriterTaskData.DigitalFormat)digitalFormatComboBox.SelectedIndex;

            _data.metadata.Clear();
            foreach (CheckedListBoxItem i in metadataComboBox.Properties.Items)
                if (i.CheckState == CheckState.Checked)
                    _data.metadata.Add(i.ToString());

            _data.Format = (KafkaWriterTaskData.DataFormat)dataFormatComboBox.SelectedIndex;
            _data.AckMode = (KafkaWriterTaskData.RequiredAcks)acknowledgmentComboBox.SelectedIndex;
            _data.ClusterMode = (KafkaWriterTaskData.ClusterType)clusterTypeComboBox.SelectedIndex;
            _data.ClusterSecurityMode = (KafkaWriterTaskData.ClusterSecurityType)clusterConnectionSecurityComboBox.SelectedIndex;
            _data.SchemaRegistrySecurityMode = (KafkaWriterTaskData.SchemaRegistrySecurityType)schemaRegistryConnectionSecurityComboBox.SelectedIndex;
            _data.SASLMechanismMode = (KafkaWriterTaskData.SASLMechanismType)schemaRegistryConnectionSecurityComboBox.SelectedIndex;
            _data.AnalysisFile = m_pdoFileTextBox.Text;
            _data.TestDatFile = m_datFileTextBox.Text;
            _data.key = keyTextBox.Text;
            _data.signalReference = signalRefTextBox.Text;

            _data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            _data.MonitorData.MonitorTime = m_cbTime.Checked;
            _data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            _data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);

            _data.enableSchema = schemaRegistryCb.Checked;

            _data.SSLClientThumbprint = clientCertParams.Thumbprint;
            _data.SSLCAThumbprint = CACertParams.Thumbprint;

            _data.enableSSLVerification = enableSSLVerificationCb.Checked;

            _data.SASLUsername = SASLNameTextBox.Text;
            _data.SASLPass = SASLPassTextBox.Text;

            _data.schemaSSLClientThumbprint = schemaClientCertParams.Thumbprint;
            _data.schemaSSLCAThumbprint = schemaCACertParams.Thumbprint;

            _data.schemaEnableSSLVerification = schemaEnableSSLVerificationCb.Checked;

            _data.schemaUsername = schemaNameTextBox.Text;
            _data.schemaPass = schemaPassTextBox.Text;
        }
        public void ShowToolTipOnFocus(object sender, EventArgs e)
        {
            if (keyTextBox.Focused)
            {
                placeholdersKeyToolTip.Show(Properties.Resources.KafkaPlaceholdersKeyHint, tabConnection, metadataComboBox.Location);
            }
            else if (signalRefTextBox.Focused)
            {
                placeholdersToolTip.Show(Properties.Resources.KafkaPlaceholdersSignalHint, tabConnection, dataFormatComboBox.Location);
            }
        }


        public void OnLostFocusHideTooltip(object sender, EventArgs e)
        {
            placeholdersToolTip.Hide(this);
            placeholdersKeyToolTip.Hide(this);
        }

        private void CellExpressionChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column != expressionGridColumn || e.Value is null) return;
            string expression = e.Value.ToString();

            if (expression.StartsWith("[") && expression.EndsWith("]"))
                expression = expression.Substring(1, expression.Length - 2);

            if (_viewExpr.GetRowCellValue(e.RowHandle, nameGridColumn).ToString() == "" && expression != "")
                _viewExpr.SetRowCellValue(e.RowHandle, nameGridColumn, expression);
        }

        private void buttonExpressionAdd_Click(object sender, System.EventArgs e)
        {
            _expressionTableData.Add(new KafkaWriterTaskData.KafkaRecord());
            _viewExpr.FocusedRowHandle = _expressionTableData.Count - 1;
            _viewExpr.ShowEditor();

            UpdateExprTableButtons();
        }

        private void UpdateExprTableButtons()
        {
            expressionCopyButton.Enabled = expressionRemoveButton.Enabled = (_viewExpr.RowCount > 0 && _viewExpr.FocusedRowHandle >= 0);
            upButton.Enabled = (_viewExpr.RowCount > 0 && _viewExpr.FocusedRowHandle > 0);
            downButton.Enabled = (_viewExpr.RowCount > 0 && _viewExpr.FocusedRowHandle < _viewExpr.RowCount - 1);
        }

        private void UpdateParamTableButtons()
        {
            paramRemoveButton.Enabled = _viewParam.RowCount > 0 && _viewParam.FocusedRowHandle >= 0;
        }

        private void m_browsePDOFileButton_Click(object sender, EventArgs e)
        {
            string path = m_pdoFileTextBox.Text;
            if (DatCoordinatorHostImpl.Host.BrowseForPdoFile(ref path, out string _))
            {
                m_pdoFileTextBox.Text = path;
            }
        }

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
        {
            DatCoordinatorHostImpl.Host.OpenPDO(m_pdoFileTextBox.Text);
        }

        private void m_btnUploadPDO_Click(object sender, EventArgs e)
        {
            DatCoordinatorHostImpl.Host.UploadPdoFileWithReturnValue(sender != null, this, m_pdoFileTextBox.Text, _analyzerManager, null);
            UpdateSource();
        }

        private void m_browseDatFileButton_Click(object sender, EventArgs e)
        {
            string datFile = m_datFileTextBox.Text;
            if (DatCoordinatorHostImpl.Host.BrowseForDatFile(ref datFile, _data.ParentConfigurationData))
            {
                m_datFileTextBox.Text = datFile;
            }
        }
        private void UpdateSource()
        {
            _analyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, "", _data.ParentConfigurationData);
        }

        private void m_testButton_Click(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                IbaAnalyzer.IbaAnalyzer ibaAnalyzer = null;
                try
                { 
                    ibaAnalyzer = ibaAnalyzerExt.Create(true);
              
                    bool bUseAnalysis = File.Exists(m_pdoFileTextBox.Text);
                    bool bUseDatFile = File.Exists(m_datFileTextBox.Text);

                    if (bUseAnalysis) ibaAnalyzer.OpenAnalysis(m_pdoFileTextBox.Text);
                    if (bUseDatFile) ibaAnalyzer.OpenDataFile(0, m_datFileTextBox.Text);
                    var records = (IList<KafkaWriterTaskData.KafkaRecord>)exprGrid.DataSource;
                    foreach (KafkaWriterTaskData.KafkaRecord record in records)
                    {
                        if (string.IsNullOrEmpty(record.Expression)) continue;

                        if (record.DataType == KafkaWriterTaskData.KafkaRecord.ExpressionType.Text)
                        {
                            object oValues = null;

                            try
                            {
                                ibaAnalyzer.EvaluateToStringArray(record.Expression, 0, out _, out oValues);
                            }
                            catch { }

                            if (oValues != null)
                            {
                                string[] values = oValues as string[];
                                foreach (string str in values)
                                {
                                    if (!string.IsNullOrEmpty(str))
                                    {
                                        record.TestValue = str;
                                        ParentForm.Activate();
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show(String.Format(Properties.Resources.BadEvaluate, record.Expression), "ibaDatCoordinator",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                record.TestValue = "";
                            }
                        }
                        else
                        {
                            double f;
                            try
                            {
                                f = ibaAnalyzer.EvaluateDouble(record.Expression, 0);
                            }
                            catch //might be old ibaAnalyzer
                            {
                                f = ibaAnalyzer.Evaluate(record.Expression, 0);
                            }

                            if (double.IsNaN(f) || double.IsInfinity(f))
                            {
                                MessageBox.Show(String.Format(Properties.Resources.BadEvaluate, record.Expression), "ibaDatCoordinator",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }
                            else
                            {
                                if (record.DataType == KafkaWriterTaskData.KafkaRecord.ExpressionType.Digital)
                                {
                                    if (_data.digitalFormat == KafkaWriterTaskData.DigitalFormat.TrueFalse)
                                        record.TestValue = (f >= 0.5 ? "true" : "false");
                                    else
                                        record.TestValue = (f >= 0.5 ? "1" : "0");
                                }
                                else
                                    record.TestValue = f.ToString();
                            }
                        }
                    }

                    ParentForm.Activate();
                }
                catch (Exception ex3)
                {
                    string message = ex3.Message;
                    try
                    {
                        if (ibaAnalyzer != null)
                        {
                            string ibaMessage = ibaAnalyzer.GetLastError();
                            if (!string.IsNullOrEmpty(ibaMessage))
                                message = ibaMessage;
                        }
                    }
                    catch { }

                    MessageBox.Show(message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    ParentForm?.Activate();
                    ((IDisposable)ibaAnalyzer)?.Dispose();
                    exprGrid.RefreshDataSource();
                }
            }
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSource();
        }

        private void buttonExpressionCopy_Click(object sender, EventArgs e)
        {
            if ((_viewExpr.FocusedRowHandle >= 0) &&
                (_viewExpr.FocusedRowHandle < _expressionTableData.Count) &&
                _viewExpr.GetRow(_viewExpr.FocusedRowHandle) is KafkaWriterTaskData.KafkaRecord oldRow)
            {
                var newRow = (KafkaWriterTaskData.KafkaRecord)oldRow.Clone();
                _expressionTableData.Add(newRow);
                _viewExpr.FocusedRowHandle = _expressionTableData.Count - 1;
                _viewExpr.ShowEditor();

                UpdateExprTableButtons();
            }
        }

        private void upButton_Click(object sender, EventArgs e)
        {
            if (_viewExpr.FocusedRowHandle <= 0)
                return;
            var row = _expressionTableData[_viewExpr.FocusedRowHandle];
            _expressionTableData[_viewExpr.FocusedRowHandle] = _expressionTableData[_viewExpr.FocusedRowHandle - 1];
            _expressionTableData[_viewExpr.FocusedRowHandle - 1] = row;
            _viewExpr.FocusedRowHandle--;
            UpdateExprTableButtons();
        }

        private void downButton_Click(object sender, EventArgs e)
        {
            if (_viewExpr.FocusedRowHandle >= _viewExpr.RowCount - 1)
                return;
            var row = _expressionTableData[_viewExpr.FocusedRowHandle];
            _expressionTableData[_viewExpr.FocusedRowHandle] = _expressionTableData[_viewExpr.FocusedRowHandle + 1];
            _expressionTableData[_viewExpr.FocusedRowHandle + 1] = row;
            _viewExpr.FocusedRowHandle++;
            UpdateExprTableButtons();
        }

        private void buttonExpressionRemove_Click(object sender, EventArgs e)
        {
            if ((_viewExpr.FocusedRowHandle >= 0) && (_viewExpr.FocusedRowHandle < _expressionTableData.Count))
            {
                _expressionTableData.RemoveAt(_viewExpr.FocusedRowHandle);
                UpdateExprTableButtons();
            }
        }

        private void paramAddButton_Click(object sender, EventArgs e)
        {
            _paramTableData.Add(new KafkaWriterTaskData.Param());
            _viewParam.FocusedRowHandle = _paramTableData.Count - 1;
            UpdateParamTableButtons();
        }

        private void paramRemoveButton_Click(object sender, EventArgs e)
        {
            if ((_viewParam.FocusedRowHandle >= 0) && (_viewParam.FocusedRowHandle < _paramTableData.Count))
            {
                _paramTableData.RemoveAt(_viewParam.FocusedRowHandle);
                UpdateParamTableButtons();
            }
        }

        private void testConnectionButton_Click(object sender, EventArgs e)
        {
            SaveData();
            if (addressTextBox.Text == "")
                return;

            var res = Processing.TaskManager.Manager.KafkaTestConnection(_data);
            if (res is null)
                return;
            if (res is Exception)
            {
                MessageBox.Show((res as Exception).Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            var selectedTopic = topicComboBox.SelectedItem;
            topicComboBox.Items.Clear();
            topicComboBox.Items.AddRange(res as string[]);
            if (selectedTopic != null && selectedTopic as string != "")
            {
                int ind = topicComboBox.Items.IndexOf(selectedTopic);
                topicComboBox.SelectedIndex = ind != -1 ? ind : 0;
            }
            else if (topicComboBox.Items.Count > 0)
                topicComboBox.SelectedIndex = 0;
            MessageBox.Show(Properties.Resources.ConnectionTestSucceeded, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void schemaRegistryCb_CheckedChanged(object sender, EventArgs e)
        {
            Control[] controls = { schemaTextBox, schemaRegistryConnectionSecurityComboBox, schemaNameTextBox, schemaPassTextBox,
                schemaClientCertCBox, schemaEnableSSLVerificationCb, schemaCACertCBox, schemaNameLabel, schemaPassLabel, schemaClientCertificateLabel, schemaCACertificateLabel, schemaRegSecurityLabel };
            foreach (var c in controls)
                c.Enabled = schemaRegistryCb.Checked;
        }

        private void enableSSLVerificationCb_CheckedChanged(object sender, EventArgs e)
        {
            CACertificateLabel.Enabled = enableSSLVerificationCb.Checked;
            CACertCBox.Enabled = enableSSLVerificationCb.Checked;
        }

        private void schemaEnableSSLVerificationCb_CheckedChanged(object sender, EventArgs e)
        {
            schemaCACertificateLabel.Enabled = schemaEnableSSLVerificationCb.Checked;
            schemaCACertCBox.Enabled = schemaEnableSSLVerificationCb.Checked;
        }

        private void typeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            _modeSwitcher.SwitchInterface((KafkaWriterTaskData.ClusterType)clusterTypeComboBox.SelectedIndex);
        }

        private void OnImportParameters(object sender, EventArgs e)
        {
            string fileName = "";

            using (OpenFileDialog fd = new OpenFileDialog())
            {
                fd.Filter = Properties.Resources.CSVFileFilter;
                fd.DefaultExt = ".csv";
                fd.Title = Properties.Resources.ImportParametersFromCSV;
                fd.Multiselect = false;
                fd.CheckFileExists = true;
                if (fd.ShowDialog(this) != DialogResult.OK)
                    return;

                fileName = fd.FileName;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show(this, Properties.Resources.FileNotFound, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (StreamReader sr = File.OpenText(fileName))
                {
                    string line;
                    while ((line = sr.ReadLine()) != null)
                    {
                        if (string.IsNullOrEmpty(line))
                            continue;

                        string[] parts = line.Split(',');
                        if (parts.Length != 2)
                            continue;

                        if (_paramTableData.FindIndex(param => param.Key == parts[0]) != -1)
                            continue;

                        _paramTableData.Add(new KafkaWriterTaskData.Param{Key = parts[0], Value = parts[1]});
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void clusterConnectionSecurityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((KafkaWriterTaskData.ClusterSecurityType)clusterConnectionSecurityComboBox.SelectedIndex == KafkaWriterTaskData.ClusterSecurityType.SSL ||
                (KafkaWriterTaskData.ClusterSecurityType)clusterConnectionSecurityComboBox.SelectedIndex == KafkaWriterTaskData.ClusterSecurityType.SASL_SSL)
                clusterSSL.Show();
            else
                clusterSSL.Hide();

            if ((KafkaWriterTaskData.ClusterSecurityType)clusterConnectionSecurityComboBox.SelectedIndex == KafkaWriterTaskData.ClusterSecurityType.SASL_PLAINTEXT ||
                (KafkaWriterTaskData.ClusterSecurityType)clusterConnectionSecurityComboBox.SelectedIndex == KafkaWriterTaskData.ClusterSecurityType.SASL_SSL)
                clusterSASL.Show();
            else
                clusterSASL.Hide();
        }

        private void schemaRegistryConnectionSecurityComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if ((KafkaWriterTaskData.SchemaRegistrySecurityType)schemaRegistryConnectionSecurityComboBox.SelectedIndex == KafkaWriterTaskData.SchemaRegistrySecurityType.HTTPS ||
                (KafkaWriterTaskData.SchemaRegistrySecurityType)schemaRegistryConnectionSecurityComboBox.SelectedIndex == KafkaWriterTaskData.SchemaRegistrySecurityType.HTTPS_AUTHENTICATION)
                schemaSSL.Show();
            else
                schemaSSL.Hide();

            if ((KafkaWriterTaskData.SchemaRegistrySecurityType)schemaRegistryConnectionSecurityComboBox.SelectedIndex == KafkaWriterTaskData.SchemaRegistrySecurityType.HTTP_AUTHENTICATION ||
                (KafkaWriterTaskData.SchemaRegistrySecurityType)schemaRegistryConnectionSecurityComboBox.SelectedIndex == KafkaWriterTaskData.SchemaRegistrySecurityType.HTTPS_AUTHENTICATION)
                schemaAuth.Show();
            else
                schemaAuth.Hide();
        }

        private void OnExportParameters(object sender, EventArgs e)
        {
            string fileName;

            using (SaveFileDialog fd = new SaveFileDialog())
            {
                fd.Filter = Properties.Resources.CSVFileFilter;
                fd.DefaultExt = ".csv";
                fd.AddExtension = true;
                fd.Title = Properties.Resources.ExportParametersToCSV;
                fd.OverwritePrompt = true;
                fd.ValidateNames = true;
                if (fd.ShowDialog(this) != DialogResult.OK)
                    return;

                fileName = fd.FileName;
            }

            if (string.IsNullOrEmpty(fileName))
            {
                MessageBox.Show(this, Properties.Resources.FileNotFound, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (System.IO.StreamWriter sw = new System.IO.StreamWriter(fileName))
                {
                    foreach (KafkaWriterTaskData.Param p in _paramTableData)
                    {
                        sw.WriteLine($"{p.Key},{p.Value}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

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
    }
}