using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Data;
using Confluent.Kafka;
using DevExpress.XtraGrid.Views.Grid;
using iba.Utility;

namespace iba.Controls
{
    internal partial class KafkaWriterTaskControl : UserControl, IPropertyPane
    {
        readonly BindingList<KafkaWriterTaskData.Record> _expressionTableData;
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
            _expressionTableData = new BindingList<KafkaWriterTaskData.Record>();
            exprGrid.DataSource = _expressionTableData;
            _viewExpr = (GridView)exprGrid.MainView;
            _viewExpr.FocusedRowChanged += (sender, e) => UpdateExprTableButtons();

            var channelEditor = new RepositoryItemChannelTreeEdit(_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Text);
            channelEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            exprGrid.RepositoryItems.Add(channelEditor);
            gridColumnExpression.ColumnEdit = channelEditor;
            
            var typeComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            foreach (var t in KafkaWriterTaskData.Record.DataTypes)
                typeComboBox.Items.Add(t);
            dataTypeGridColumn.ColumnEdit = typeComboBox;

            _paramTableData = new BindingList<KafkaWriterTaskData.Param>();
            paramGrid.DataSource = _paramTableData;
            _viewParam = (GridView)paramGrid.MainView;
            _viewParam.FocusedRowChanged += (sender, e) => UpdateParamTableButtons();
            keyGridColumn.Caption = Properties.Resources.Key;
            valGridColumn.Caption = Properties.Resources.Value;
        }

        //public string TestKafkaConnection()
        //{
        //    KafkaTopicsCollection coll = GetKafkaTopics(connConfig);
        //    if (coll == null)
        //        return "Unexpected error";

        //    // Test connection to schema registry
        //    if (!string.IsNullOrEmpty(connConfig.SchemaRegistryAddress))
        //    {
        //        string errorMsg;
        //        if (!TestSchemaRegistryConnection(connConfig, out errorMsg))
        //            return string.Concat("Error connecting to schema registry: ", errorMsg);
        //    }

        //    return coll.ErrorMsg;
        //}
        //public static KafkaTopicsCollection GetKafkaTopics(KafkaConnectionConfig connConfig)
        //{
        //    KafkaTopicsCollection result = new KafkaTopicsCollection();

        //    if ((connConfig == null) || string.IsNullOrEmpty(connConfig.ClusterAddress))
        //    {
        //        result.ErrorMsg = "Invalid cluster address";
        //        return result;
        //    }

        //    Confluent.Kafka.IAdminClient adminClient = null;
        //    try
        //    {
        //        Confluent.Kafka.AdminClientConfig admCfg = new Confluent.Kafka.AdminClientConfig();
        //        CKafkaArchiverLevel.PrepareConfig(admCfg, connConfig);

        //        adminClient = (new Confluent.Kafka.AdminClientBuilder(admCfg)).Build();
        //        Confluent.Kafka.Metadata metaData = adminClient.GetMetadata(TimeSpan.FromSeconds(5));

        //        int nrTopics = metaData.Topics.Count;
        //        result.Topics = new string[nrTopics];

        //        for (int i = 0; i < nrTopics; i++)
        //            result.Topics[i] = metaData.Topics[i].Topic;

        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.ErrorMsg = ex.Message;
        //        return result;
        //    }
        //    finally
        //    {
        //        if (adminClient != null)
        //            adminClient.Dispose();
        //    }
        //}

        //public static bool TestSchemaRegistryConnection(KafkaConnectionConfig connConfig, out string errorMsg)
        //{
        //    if ((connConfig == null) || string.IsNullOrEmpty(connConfig.SchemaRegistryAddress))
        //    {
        //        errorMsg = "Invalid schema registry address";
        //        return false;
        //    }

        //    Confluent.SchemaRegistry.CachedSchemaRegistryClient schemRegClient = null;
        //    try
        //    {
        //        Confluent.SchemaRegistry.SchemaRegistryConfig schemRegConfig = new Confluent.SchemaRegistry.SchemaRegistryConfig();
        //        schemRegConfig.Url = connConfig.SchemaRegistryAddress;
        //        schemRegConfig.RequestTimeoutMs = connConfig.MsgTimeoutMs;

        //        // Add expert parameters
        //        foreach (System.Collections.Generic.KeyValuePair<string, string> kvp in connConfig.ExpertParams)
        //        {
        //            if (string.IsNullOrEmpty(kvp.Key))
        //                continue;

        //            if (!kvp.Key.StartsWith("schema.registry."))
        //                continue;

        //            schemRegConfig.Set(kvp.Key, kvp.Value);
        //        }

        //        schemRegClient = new Confluent.SchemaRegistry.CachedSchemaRegistryClient(schemRegConfig);
        //        if (!(schemRegClient.GetAllSubjectsAsync()).Wait(connConfig.MsgTimeoutMs))
        //        {
        //            errorMsg = "Timeout";
        //            return false;
        //        }
        //    }
        //    catch (AggregateException aggrEx)
        //    {
        //        errorMsg = null;
        //        foreach (Exception exc in aggrEx.InnerExceptions)
        //        {
        //            string curMsg = exc.Message;
        //            if (exc is System.Net.Http.HttpRequestException httpExc)
        //            {
        //                if (httpExc.InnerException is iba.Kafka.SchemaRegistry.SslVerificationException sslVerExc)
        //                {
        //                    // Log extra info
        //                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
        //                    sb.AppendLine("Kafka Schema Registry authentication error details:");
        //                    sb.AppendLine();
        //                    sb.AppendLine($"Request URI: {sslVerExc.RequestURI}");
        //                    sb.AppendLine($"SSL Policy errors: {sslVerExc.SSLErrors}");
        //                    sb.AppendLine();
        //                    sb.AppendLine($"Certificate:");
        //                    sb.AppendLine();
        //                    sb.AppendLine(sslVerExc.Certificate);
        //                    sb.AppendLine();
        //                    sb.AppendLine($"Certificate chain:");
        //                    sb.AppendLine();
        //                    sb.AppendLine(sslVerExc.Chain);

        //                    iba.Logging.ibaLogger.DebugFormat(sb.ToString());
        //                }
        //            }

        //            errorMsg = (errorMsg == null) ? curMsg : string.Concat(errorMsg, Environment.NewLine, curMsg);
        //        }

        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        errorMsg = ex.Message;
        //        return false;
        //    }
        //    finally
        //    {
        //        if (schemRegClient != null)
        //            schemRegClient.Dispose();
        //    }

        //    errorMsg = null;
        //    return true;
        //}



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
                _expressionTableData.Add((KafkaWriterTaskData.Record)rec.Clone());
            foreach (var par in _data.Params)
                _paramTableData.Add((KafkaWriterTaskData.Param)par.Clone());

            addressTextBox.Text = _data.clusterAddress;
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
                case KafkaWriterTaskData.DataFormat.JSONGrouped :
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


            UpdateSource();

            m_cbMemory.Checked = _data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = _data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, _data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(_data.MonitorData.TimeLimit.TotalMinutes, 1));
        }

        private async void FillTopicListAsync()
        {
            if (addressTextBox.Text == "")
                return;

            loadingLabel.Visible = true;
            await Task.Run(() =>
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
                    MessageBox.Show(Properties.Resources.ConnectionTestSucceeded, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception e)
                {
                    MessageBox.Show(e.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    loadingLabel.Invoke((MethodInvoker)delegate { loadingLabel.Visible = false; });
                }
            });
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
            _data.topicName = topicComboBox.Text;
            _data.timeout = Decimal.ToDouble(timeoutNumericUpDown.Value);
            _data.identifier = identifierTextBox.Text;
            _data.digitalFormat = (KafkaWriterTaskData.DigitalFormat)digitalFormatComboBox.SelectedIndex;

            _data.Format = (KafkaWriterTaskData.DataFormat)dataFormatComboBox.SelectedIndex;
            _data.AckMode = (KafkaWriterTaskData.RequiredAcks)acknowledgmentComboBox.SelectedIndex;
            _data.AnalysisFile = m_pdoFileTextBox.Text;
            _data.TestDatFile = m_datFileTextBox.Text;

            _data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            _data.MonitorData.MonitorTime = m_cbTime.Checked;
            _data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            _data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);
        }


        private void buttonExpressionAdd_Click(object sender, System.EventArgs e)
        {
            _expressionTableData.Add(new KafkaWriterTaskData.Record());
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
            MessageBox.Show("not implemented");
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSource();
        }

        private void buttonExpressionCopy_Click(object sender, EventArgs e)
        {
            if ((_viewExpr.FocusedRowHandle >= 0) &&
                (_viewExpr.FocusedRowHandle < _expressionTableData.Count) &&
                _viewExpr.GetRow(_viewExpr.FocusedRowHandle) is KafkaWriterTaskData.Record oldRow)
            {
                var newRow = (KafkaWriterTaskData.Record)oldRow.Clone();
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
            FillTopicListAsync();
        }
    }
}