﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using iba.Data;
using DevExpress.XtraGrid.Views.Grid;
using iba.Utility;
using System.IO;
using DevExpress.XtraGrid.Views.Base;
using iba.Remoting;
using Microsoft.Win32;
using DevExpress.Data.Extensions;

namespace iba.Controls
{
	public partial class ComputedValuesTaskControl : UserControl, IPropertyPane
    {
        readonly BindingList<ComputedValuesTaskData.Record> _expressionTableData;
        ComputedValuesTaskData _data;
        [NonSerialized]
        private readonly AnalyzerManager _analyzerManager;

        private readonly GridView _view;

        public ComputedValuesTaskControl()
		{
            InitializeComponent();
            InitializeIcons();
            _analyzerManager = new AnalyzerManager();
            var channelEditor = new RepositoryItemChannelTreeEdit(_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Text);
            channelEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            dataGrid.RepositoryItems.Add(channelEditor);
            gridColumnExpression.ColumnEdit = channelEditor;

            var typeComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            foreach (var t in ComputedValuesTaskData.Record.DataTypes)
                typeComboBox.Items.Add(t);
            gridColumn1.ColumnEdit = typeComboBox;

            gridColumn1.Caption = Properties.Resources.DataType;
            gridColumnExpression.Caption = Properties.Resources.ibaAnalyzerExpression;
            gridColumn11.Caption = Properties.Resources.TestValue;
            gridColumnName.Caption = Properties.Resources.VariableName;

            gridColumnName.View.CellValueChanged += CellNameChanged;
            gridColumnExpression.View.CellValueChanged += CellExpressionChanged;

            _expressionTableData = new BindingList<ComputedValuesTaskData.Record>();
            dataGrid.DataSource = _expressionTableData;
            _view = (GridView)dataGrid.MainView;
            _view.FocusedRowChanged += (sender, e) => UpdateTableButtons();
        }

        private void InitializeIcons()
        {
            this.buttonExpressionCopy.Image = iba.Icons.Gui.All.Images.Copy();
            this.buttonExpressionRemove.Image = iba.Icons.Gui.All.Images.CrossRed();
            this.buttonExpressionAdd.Image = iba.Icons.Gui.All.Images.PlusGreen();
            this.m_btnUploadPDO.Image = Icons.Gui.All.Images.FilePdoUpload(16);
            this.m_browsePDOFileButton.Image = Icons.Gui.All.Images.FolderOpen(16);
            this.m_executeIBAAButton.Image = Icons.SystemTray.Images.IbaAnalyzer(16);
            this.m_browseDatFileButton.Image = Icons.Gui.All.Images.FolderOpen(16);
            this.downButton.Image = Icons.Gui.All.Images.ArrowDownBoxed(16);
            this.m_testButton.Image = Icons.Gui.All.Images.CircleQuestionFilledBlue(16);
            this.upButton.Image = Icons.Gui.All.Images.ArrowUpBoxed(16);
        }

        private void CellExpressionChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column != gridColumnExpression || e.Value is null)  return;
            string expression = e.Value.ToString();

            if (_view.GetRowCellValue(e.RowHandle, gridColumnName).ToString() == "" && expression != "")
                _view.SetRowCellValue(e.RowHandle, gridColumnName, expression);
        }

        private void CellNameChanged(object sender, CellValueChangedEventArgs e)
        {
            if (e.Column != gridColumnName || e.Value is null) return;
            string name = e.Value.ToString();

            name = EnsureNameUnique(name, e.RowHandle);

            gridColumnName.View.CellValueChanged -= CellNameChanged;
            _view.SetRowCellValue(e.RowHandle, gridColumnName, name);
            gridColumnName.View.CellValueChanged += CellNameChanged;
        }

        private string EnsureNameUnique(string name, int row, bool sourceAlreadyUpdated = true)
        {
            var list = (BindingList<ComputedValuesTaskData.Record>)dataGrid.DataSource;
            if (name == "")
            {
                name = dataGV.GetRowCellValue(row, gridColumnExpression).ToString();
                if (name == "")
                    return "";
                sourceAlreadyUpdated = false;
            }

            // left slash is not allowed in our UA Node ID convention
            name = name.Replace('\\', '_');

            var r = list.Count(item => item.Name == name);
            if (r > 1 || (!sourceAlreadyUpdated && r > 0))
            {
                int index = 1;
                while (list.FindIndex(item => item.Name == name + "_" + index.ToString()) > 0)
                    index++;
                name += "_" + index.ToString();
            }
            return name;
        }

        public void LoadData(object datasource, IPropertyPaneManager manager)
		{
			_data = (ComputedValuesTaskData)datasource;
            if (_data.m_analyzerManager is null)
                _data.m_analyzerManager = _analyzerManager;
            m_pdoFileTextBox.Text = _data.AnalysisFile;
            m_datFileTextBox.Text = _data.TestDatFile;

            _expressionTableData.Clear();
            foreach (var rec in _data.Records)
                _expressionTableData.Add((ComputedValuesTaskData.Record)rec.Clone());
            UpdateTableButtons();

            string ibaAnalyzerExe;
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                // ReSharper disable once PossibleNullReferenceException - ok, we use it in try/catch
                object o = key.GetValue("");
                ibaAnalyzerExe = Path.GetFullPath(o.ToString());
            }
            catch
            {
                ibaAnalyzerExe = Properties.Resources.noIbaAnalyser;
            }
            try
            {
                m_monitorGroup.Enabled = VersionCheck.CheckVersion(ibaAnalyzerExe, "5.8.1");
            }
            catch
            {
                m_monitorGroup.Enabled = false;
            }
            UpdateSource();

            m_cbMemory.Checked = _data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = _data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, _data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(_data.MonitorData.TimeLimit.TotalMinutes, 1));
        }


		private void ButtonExpressionAdd_Click(object sender, EventArgs e)
		{
            _expressionTableData.Add(new ComputedValuesTaskData.Record());
			_view.FocusedRowHandle = _expressionTableData.Count - 1;
			_view.ShowEditor();

            UpdateTableButtons();
		}

		private void ButtonExpressionRemove_Click(object sender, EventArgs e)
		{
			if ((_view.FocusedRowHandle >= 0) && (_view.FocusedRowHandle < _expressionTableData.Count))
			{
                _expressionTableData.RemoveAt(_view.FocusedRowHandle);
                UpdateTableButtons();
			}
		}

		private void ButtonExpressionCopy_Click(object sender, EventArgs e)
		{
			if ((_view.FocusedRowHandle >= 0) &&
				(_view.FocusedRowHandle < _expressionTableData.Count) &&
				_view.GetRow(_view.FocusedRowHandle) is ComputedValuesTaskData.Record oldRow)
			{
                var newRow = (ComputedValuesTaskData.Record)oldRow.Clone();
                string removedNumberSuffix = System.Text.RegularExpressions.Regex.Replace(newRow.Name, "_[0-9]{1,3}$", "");
                if (removedNumberSuffix.Length > 0)
                    newRow.Name = removedNumberSuffix;
                newRow.Name = EnsureNameUnique(newRow.Name, _view.FocusedRowHandle, false);
                _expressionTableData.Add(newRow);
				_view.FocusedRowHandle = _expressionTableData.Count - 1;
				_view.ShowEditor();
                
                UpdateTableButtons();
			}
        }
        private void UpButton_Click(object sender, EventArgs e)
        {
            if (_view.FocusedRowHandle <= 0)
                return;
            var row = _expressionTableData[_view.FocusedRowHandle];
            _expressionTableData[_view.FocusedRowHandle] = _expressionTableData[_view.FocusedRowHandle - 1];
            _expressionTableData[_view.FocusedRowHandle - 1] = row;
            _view.FocusedRowHandle--;
            UpdateTableButtons();
        }

        private void DownButton_Click(object sender, EventArgs e)
        {
            if (_view.FocusedRowHandle >= _view.RowCount - 1)
                return;
            var row = _expressionTableData[_view.FocusedRowHandle];
            _expressionTableData[_view.FocusedRowHandle] = _expressionTableData[_view.FocusedRowHandle + 1];
            _expressionTableData[_view.FocusedRowHandle + 1] = row;
            _view.FocusedRowHandle++;
            UpdateTableButtons();
        }

        private void UpdateTableButtons()
        {
            buttonExpressionCopy.Enabled = buttonExpressionRemove.Enabled = (_view.FocusedRowHandle >= 0);
            upButton.Enabled = (_view.FocusedRowHandle > 0);
            downButton.Enabled = (_view.RowCount > 0 && _view.FocusedRowHandle < _view.RowCount - 1);
        }

        public void LeaveCleanup()
        {
            MainForm.InformExtMonDataAboutTreeStructureChange();
        }

		public void SaveData()
        {
            _data.Records.Clear();
            foreach (var rec in _expressionTableData)
                if (!String.IsNullOrWhiteSpace(rec.Expression))
                    _data.Records.Add(rec);

            _data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            _data.MonitorData.MonitorTime = m_cbTime.Checked;
            _data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            _data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);
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

        private void TestButton_Click(object sender, EventArgs e)
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
                    var records = (IList<ComputedValuesTaskData.Record>)dataGrid.DataSource;
                    foreach (ComputedValuesTaskData.Record record in records)
                    {
                        if (string.IsNullOrEmpty(record.Expression)) continue;

                        if (record.DataType == ComputedValuesTaskData.Record.ExpressionType.Text)
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
                                        break;
                                    }
                                }
                            }
                            else
                            {
                                MessageBox.Show(String.Format(Properties.Resources.BadEvaluate, record.Name),
                                        "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                                MessageBox.Show(String.Format(Properties.Resources.BadEvaluate, record.Name), "ibaDatCoordinator", MessageBoxButtons.OK,
                                    MessageBoxIcon.Warning);
                            }
                            else
                            {
                                if (record.DataType == ComputedValuesTaskData.Record.ExpressionType.Digital)
                                    record.TestValue = (f >= 0.5 ? "true" : "false");
                                else
                                    record.TestValue = f;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string message = ex.Message;
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
                    dataGrid.RefreshDataSource();
                }
            }
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSource();
            _data.AnalysisFile = m_pdoFileTextBox.Text;
        }

        private void m_datFileTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSource();
            _data.TestDatFile = m_datFileTextBox.Text;
        }
        private void UpdateSource()
        {
            _analyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, "", _data.ParentConfigurationData);
        }
    }
}
