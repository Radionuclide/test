using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using iba.Data;
using DevExpress.XtraGrid.Views.Grid;
using iba.Utility;
using System.IO;
using Microsoft.Win32;

namespace iba.Controls
{
	public partial class OpcUaWriterTaskControl : UserControl, IPropertyPane
	{
		public OpcUaWriterTaskControl()
		{
			InitializeComponent();
            m_analyzerManager = new AnalyzerManager();
            m_channelEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Text);
            m_channelEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            dataGrid.RepositoryItems.Add(m_channelEditor);
            gridColumnExpression.ColumnEdit = m_channelEditor;

            var typeComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            foreach (var t in OpcUaWriterTaskData.Record.dataTypes)
                typeComboBox.Items.Add(t);
            gridColumn1.ColumnEdit = typeComboBox;
            gridColumn1.Caption = Properties.Resources.DataType;

            gridColumnName.View.CellValueChanged += CellNameChanged;
            gridColumnExpression.View.CellValueChanged += CellExpressionChanged;
        }

        private void CellExpressionChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column != gridColumnExpression) return;
            GridView view = sender as GridView;
            string expression = e.Value.ToString();

            if (view.GetRowCellValue(e.RowHandle, gridColumnName).ToString() == "" && expression != "")
                view.SetRowCellValue(e.RowHandle, gridColumnName, expression);
        }

        private void CellNameChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
        {
            if (e.Column != gridColumnName) return;
            GridView view = sender as GridView;
            string name = e.Value.ToString();

            name = EnsureNameUnique(name, e.RowHandle);

            gridColumnName.View.CellValueChanged -= CellNameChanged;
            view.SetRowCellValue(e.RowHandle, gridColumnName, name);
            gridColumnName.View.CellValueChanged += CellNameChanged;
        }

        private string EnsureNameUnique(string name, int row, bool sourceAlreadyUpdated = true)
        {
            var list = (BindingList<OpcUaWriterTaskData.Record>)dataGrid.DataSource;
            if (name == "")
            {
                name = dataGV.GetRowCellValue(row, gridColumnExpression).ToString();
                if (name == "")
                    return "";
                sourceAlreadyUpdated = false;
            }
            name.Replace('\\', '_');

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
			m_data = datasource as OpcUaWriterTaskData;
            if (m_data.m_analyzerManager is null)
                m_data.m_analyzerManager = m_analyzerManager;
            m_pdoFileTextBox.Text = m_data.AnalysisFile;
            m_datFileTextBox.Text = m_data.TestDatFile;
            BindingList<OpcUaWriterTaskData.Record> list = new BindingList<OpcUaWriterTaskData.Record>(m_data.Records)
            {
                AllowNew = true, 
                AllowRemove = true
            };
            dataGrid.DataSource = list;

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

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes, 1));
        }


        OpcUaWriterTaskData m_data;
        [NonSerialized]
        private AnalyzerManager m_analyzerManager;
        [NonSerialized]
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit m_channelEditor;

		private void buttonEndpointAdd_Click(object sender, EventArgs e)
		{
			var view = dataGrid.MainView as GridView;
			m_data.Records.Add(new OpcUaWriterTaskData.Record());
			view.FocusedRowHandle = m_data.Records.Count - 1;
			view.ShowEditor();

			dataGrid.RefreshDataSource();
			buttonEndpointCopy.Enabled = buttonEndpointRemove.Enabled = (view.FocusedRowHandle >= 0) && (m_data.Records.Count > 0);
		}

		private void buttonEndpointRemove_Click(object sender, EventArgs e)
		{
			var view = dataGrid.MainView as GridView;
			if ((view.FocusedRowHandle >= 0) && (view.FocusedRowHandle < m_data.Records.Count))
			{
				m_data.Records.RemoveAt(view.FocusedRowHandle);
				buttonEndpointCopy.Enabled = buttonEndpointRemove.Enabled = (view.FocusedRowHandle >= 0) && (m_data.Records.Count > 0);
			}
			dataGrid.RefreshDataSource();
		}

		private void buttonEndpointCopy_Click(object sender, EventArgs e)
		{
			var view = dataGrid.MainView as GridView;
			if ((view.FocusedRowHandle >= 0) &&
				(view.FocusedRowHandle < m_data.Records.Count) &&
				view.GetRow(view.FocusedRowHandle) is OpcUaWriterTaskData.Record oldRow)
			{
                var newRow = oldRow.Clone() as OpcUaWriterTaskData.Record;
                string removedNumberSuffix = System.Text.RegularExpressions.Regex.Replace(newRow.Name, "_[0-9]{1,3}$", "");
                if (removedNumberSuffix.Length > 0)
                    newRow.Name = removedNumberSuffix;
                newRow.Name = EnsureNameUnique(newRow.Name, view.FocusedRowHandle, false);
				m_data.Records.Add(newRow);
				view.FocusedRowHandle = m_data.Records.Count - 1;
				view.ShowEditor();

				buttonEndpointCopy.Enabled = buttonEndpointRemove.Enabled = (view.FocusedRowHandle >= 0) && (m_data.Records.Count > 0);
			}
			dataGrid.RefreshDataSource();
		}

		public void LeaveCleanup() { }

		public void SaveData()
		{
            var bindingList = (dataGrid.DataSource as BindingList<OpcUaWriterTaskData.Record>);

            var l = bindingList.ToList();
            l.RemoveAll(item => item.Expression == "");
            m_data.Records = l;

            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);
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
			DatCoordinatorHostImpl.Host.UploadPdoFile(sender != null, this, m_pdoFileTextBox.Text, m_analyzerManager, null);
            UpdateSource();
        }

        private void m_browseDatFileButton_Click(object sender, EventArgs e)
        {
            string datFile = m_datFileTextBox.Text;
            if (DatCoordinatorHostImpl.Host.BrowseForDatFile(ref datFile, m_data.ParentConfigurationData))
            {
                m_datFileTextBox.Text = datFile;
            }
        }

        private void m_testButton_Click(object sender, EventArgs e)
        {
            IbaAnalyzer.IbaAnalyzer ibaAnalyzer = null;
            //register this
            using (new WaitCursor())
            {
                //start the com object
                try
                {
                    ibaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(ex2.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string version = ibaAnalyzer.GetVersion();
            int startIndex = version.IndexOf(' ') + 1;
            int stopIndex = startIndex + 1;
            while (stopIndex < version.Length && (char.IsDigit(version[stopIndex]) || version[stopIndex] == '.'))
                stopIndex++;
            string[] nrs = version.Substring(startIndex, stopIndex - startIndex).Split('.');
            if (nrs.Length < 3)
            {
                MessageBox.Show(Properties.Resources.NoVersion, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int major;
            if (!Int32.TryParse(nrs[0], out major))
            {
                MessageBox.Show(Properties.Resources.NoVersion, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int minor;
            if (!Int32.TryParse(nrs[1], out minor))
            {
                MessageBox.Show(Properties.Resources.NoVersion, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int bugfix;
            if (!Int32.TryParse(nrs[2], out bugfix))
            {
                MessageBox.Show(Properties.Resources.NoVersion, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if (major < 6 || (major == 6 && minor < 5))
                MessageBox.Show(string.Format(Properties.Resources.ibaAnalyzerVersionError, version.Substring(startIndex)), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if (major < 6 || (major == 6 && minor < 7))
                MessageBox.Show(string.Format(Properties.Resources.ibaAnalyzerVersionError, version.Substring(startIndex)), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            bool bUseAnalysis = File.Exists(m_pdoFileTextBox.Text);
            bool bUseDatFile = File.Exists(m_datFileTextBox.Text);
            double f = 0;
            try
            {
                using (new WaitCursor())
                {
                    if (bUseAnalysis) ibaAnalyzer.OpenAnalysis(m_pdoFileTextBox.Text);
                    if (bUseDatFile) ibaAnalyzer.OpenDataFile(0, m_datFileTextBox.Text);
                    var records = dataGrid.DataSource as IList<OpcUaWriterTaskData.Record>;
                    foreach (OpcUaWriterTaskData.Record record in records)
                    {
                        if (string.IsNullOrEmpty(record.Expression)) continue;

                        if (record.DataType == OpcUaWriterTaskData.Record.ExpressionType.Text)
                        {
                            object oStamps = null;
                            object oValues = null;

                            try
                            {
                                ibaAnalyzer.EvaluateToStringArray(record.Expression, 0, out oStamps, out oValues);
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
                                MessageBox.Show(String.Format(Properties.Resources.BadEvaluate, record.Name), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                record.TestValue = "";
                            }
                        }
                        else
                        {
                            try
                            {
                                f = ibaAnalyzer.EvaluateDouble(record.Expression, 0);
                            }
                            catch  //might be old ibaAnalyzer
                            {
                                f = (double)ibaAnalyzer.Evaluate(record.Expression, 0);
                            }

                            if (double.IsNaN(f) || double.IsInfinity(f))
                            {
                                MessageBox.Show(String.Format(Properties.Resources.BadEvaluate, record.Name), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            }

                            else
                            {
                                if (record.DataType == OpcUaWriterTaskData.Record.ExpressionType.Digital)
                                    record.TestValue = (f == 0.0 ? "false" : "true");
                                else
                                    record.TestValue = f;
                            }
                        }
                    }
                    ParentForm.Activate();
                }
            }
            catch (Exception ex3)
            {
                string message = ex3.Message;
                try
                {
                    string ibaMessage = ibaAnalyzer.GetLastError();
                    if (!string.IsNullOrEmpty(ibaMessage))
                        message = ibaMessage;
                }
                catch
                {
                }
                MessageBox.Show(message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ibaAnalyzer != null && bUseAnalysis)
                {
                    ibaAnalyzer.CloseAnalysis();
                    ibaAnalyzer.CloseDataFiles();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaAnalyzer);
                }
                dataGrid.RefreshDataSource();
            }
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSource();
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
        }

        private void m_datFileTextBox_TextChanged(object sender, EventArgs e)
        {
            UpdateSource();
            m_data.TestDatFile = m_datFileTextBox.Text;
        }
        private void UpdateSource()
        {
            m_analyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, "");
        }

    }
}
