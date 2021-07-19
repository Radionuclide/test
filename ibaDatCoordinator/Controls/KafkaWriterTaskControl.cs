﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using iba.Data;
using Confluent.Kafka;
using DevExpress.XtraGrid.Views.Grid;
using iba.Utility;
using System.IO;
using System.Collections.Generic;
using DevExpress.XtraEditors.Controls;
using iba.Remoting;
using IbaAnalyzer;

namespace iba.Controls
{
    internal partial class KafkaWriterTaskControl : UserControl, IPropertyPane
    {
        readonly BindingList<KafkaWriterTaskData.KafkaRecord> _expressionTableData;
        readonly BindingList<KafkaWriterTaskData.Param> _paramTableData;
        private readonly AnalyzerManager _analyzerManager;
        protected DevExpress.XtraGrid.GridControl exprGrid;
        private KafkaWriterTaskData _data;
        private readonly GridView _viewExpr;
        private GridView _viewParam;

        public KafkaWriterTaskControl()
        {
            InitializeComponent();
            _analyzerManager = new AnalyzerManager();
            _expressionTableData = new BindingList<KafkaWriterTaskData.KafkaRecord>();
            exprGrid.DataSource = _expressionTableData;
            _viewExpr = (GridView)exprGrid.MainView;
            _viewExpr.FocusedRowChanged += (sender, e) => UpdateExprTableButtons();

            var channelEditor = new RepositoryItemChannelTreeEdit(_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Text);
            channelEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            exprGrid.RepositoryItems.Add(channelEditor);
            expressionGridColumn.ColumnEdit = channelEditor;

            metadataComboBox.Properties.Items.Add("Unit");
            metadataComboBox.Properties.Items.Add("Comment 1");
            metadataComboBox.Properties.Items.Add("Comment 2");
            metadataComboBox.Properties.Items.Add("Signal name");
            metadataComboBox.Properties.Items.Add("Signal ID");
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

            _toolTip.SetToolTip(importParamButton, iba.Properties.Resources.ImportParametersFromCSV);
            _toolTip.SetToolTip(exportParamButton, iba.Properties.Resources.ExportParametersToCSV);
        }

        private void UpdateParamTableButtons()
        {
            paramRemoveButton.Enabled = _viewParam.FocusedRowHandle >= 0;
        }

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            _data = datasource as KafkaWriterTaskData;

            m_pdoFileTextBox.Text = _data.AnalysisFile;
            m_datFileTextBox.Text = _data.TestDatFile;
            _expressionTableData.Clear();
            foreach (var rec in _data.Records)
                _expressionTableData.Add((KafkaWriterTaskData.KafkaRecord)rec.Clone());
            foreach (var par in _data.Params)
                _paramTableData.Add((KafkaWriterTaskData.Param)par.Clone());

            foreach (CheckedListBoxItem i in metadataComboBox.Properties.Items)
                i.CheckState = _data.metadata.Contains(i.ToString()) ? CheckState.Checked : CheckState.Unchecked;

            addressTextBox.Text = _data.clusterAddress;
            schemaTextBox.Text = _data.schemaRegistryAddress;
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

            switch (_data.ClusterMode)
            {
                case KafkaWriterTaskData.ClusterType.Kafka:
                    typeComboBox.SelectedIndex = 0;
                    break;
                case KafkaWriterTaskData.ClusterType.EventHub:
                    typeComboBox.SelectedIndex = 1;
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

            UpdateSource();

            m_cbMemory.Checked = _data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = _data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, _data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(_data.MonitorData.TimeLimit.TotalMinutes, 1));
        }

        private bool FillTopicList()
        {
            try
            {
                using (var adminClient = new AdminClientBuilder(new AdminClientConfig { BootstrapServers = addressTextBox.Text }).Build())
                {
                    var meta = adminClient.GetMetadata(TimeSpan.FromSeconds(Decimal.ToDouble(timeoutNumericUpDown.Value)));

                    topicComboBox.Invoke((MethodInvoker)delegate {
                        topicComboBox.Items.Clear();
                        meta.Topics.ForEach(topic => { topicComboBox.Items.Add(topic.Topic); });
                    });
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
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
            _data.ClusterMode = (KafkaWriterTaskData.ClusterType)typeComboBox.SelectedIndex;
            _data.ClusterSecurityMode = (KafkaWriterTaskData.ClusterSecurityType)clusterConnectionSecurityComboBox.SelectedIndex;
            _data.SchemaRegistrySecurityMode = (KafkaWriterTaskData.SchemaRegistrySecurityType)schemaRegistryConnectionSecurityComboBox.SelectedIndex;
            _data.AnalysisFile = m_pdoFileTextBox.Text;
            _data.TestDatFile = m_datFileTextBox.Text;

            _data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            _data.MonitorData.MonitorTime = m_cbTime.Checked;
            _data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            _data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);
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
            expressionCopyButton.Enabled = expressionRemoveButton.Enabled = (_viewExpr.FocusedRowHandle >= 0);
            upButton.Enabled = (_viewExpr.FocusedRowHandle > 0);
            downButton.Enabled = (_viewExpr.RowCount > 0 && _viewExpr.FocusedRowHandle < _viewExpr.RowCount - 1);
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
            if (addressTextBox.Text == "")
                return;

            bool clusterAvailable = FillTopicList();

            if (!clusterAvailable)
                return;

            if (schemaTextBox.Text == "")
            {
                MessageBox.Show(Properties.Resources.ConnectionTestSucceeded, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }


            Confluent.SchemaRegistry.CachedSchemaRegistryClient schemRegClient = null;
            try
            {
                Confluent.SchemaRegistry.SchemaRegistryConfig schemRegConfig = new Confluent.SchemaRegistry.SchemaRegistryConfig();
                schemRegConfig.Url = schemaTextBox.Text;
                schemRegConfig.RequestTimeoutMs = Decimal.ToInt32(timeoutNumericUpDown.Value) * 1000;

                // Add expert parameters
                foreach (var par in _paramTableData)
                {
                    if (string.IsNullOrEmpty(par.Key))
                        continue;

                    if (!par.Key.StartsWith("schema.registry."))
                        continue;

                    schemRegConfig.Set(par.Key, par.Value);
                }

                schemRegClient = new Confluent.SchemaRegistry.CachedSchemaRegistryClient(schemRegConfig);
                if (!(schemRegClient.GetAllSubjectsAsync()).Wait(Decimal.ToInt32(timeoutNumericUpDown.Value) * 1000))
                {
                    MessageBox.Show("Error connecting to schema registry: Timeout.", "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            catch (AggregateException aggrEx)
            {
                string errorMsg = null;
                foreach (Exception exc in aggrEx.InnerExceptions)
                {
                    string curMsg = exc.Message;
                    if (exc is System.Net.Http.HttpRequestException httpExc)
                    {
                        if (httpExc.InnerException is iba.Kafka.SchemaRegistry.SslVerificationException sslVerExc)
                        {
                            // Log extra info
                            System.Text.StringBuilder sb = new System.Text.StringBuilder();
                            sb.AppendLine("Kafka Schema Registry authentication error details:");
                            sb.AppendLine();
                            sb.AppendLine($"Request URI: {sslVerExc.RequestURI}");
                            sb.AppendLine($"SSL Policy errors: {sslVerExc.SSLErrors}");
                            sb.AppendLine();
                            sb.AppendLine($"Certificate:");
                            sb.AppendLine();
                            sb.AppendLine(sslVerExc.Certificate);
                            sb.AppendLine();
                            sb.AppendLine($"Certificate chain:");
                            sb.AppendLine();
                            sb.AppendLine(sslVerExc.Chain);

                            iba.Logging.ibaLogger.DebugFormat(sb.ToString());
                        }
                    }

                    errorMsg = (errorMsg == null) ? curMsg : string.Concat(errorMsg, Environment.NewLine, curMsg);
                }
                if (errorMsg != null)
                    MessageBox.Show(errorMsg, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.InnerException != null ? ex.InnerException.Message : ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
                return;
            }
            finally
            {
                if (schemRegClient != null)
                    schemRegClient.Dispose();
            }
            MessageBox.Show(Properties.Resources.ConnectionTestSucceeded, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
    }
}