using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Data;
using DevExpress.XtraGrid.Views.Grid;
using iba.Utility;
using System.IO;
using Microsoft.Win32;

namespace iba.Controls
{
	public partial class OPCUAWriterTaskControl : UserControl, IPropertyPane
	{
		public OPCUAWriterTaskControl()
		{
			InitializeComponent();
            m_analyzerManager = new AnalyzerManager();
            m_channelEditor = new RepositoryItemChannelTreeEdit(m_analyzerManager, ChannelTreeFilter.Digital | ChannelTreeFilter.Analog | ChannelTreeFilter.Logicals | ChannelTreeFilter.Expressions | ChannelTreeFilter.Text);
            m_channelEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
            dataGrid.RepositoryItems.Add(m_channelEditor);
            gridColumnExpression.ColumnEdit = m_channelEditor;

            var typeComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            foreach (var t in OPCUAWriterTaskData.Record.dataTypes)
                typeComboBox.Items.Add(t);
            gridColumn1.ColumnEdit = typeComboBox;
            gridColumn1.Caption = iba.Properties.Resources.DataType;

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
            var list = (BindingList<OPCUAWriterTaskData.Record>)dataGrid.DataSource;
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
			m_data = datasource as OPCUAWriterTaskData;
            if (m_data.m_analyzerManager is null)
                m_data.m_analyzerManager = m_analyzerManager;
            m_pdoFileTextBox.Text = m_data.AnalysisFile;
            m_datFileTextBox.Text = m_data.TestDatFile;
            m_folderNameTextBox.Text = m_data.FolderName;
			BindingList<OPCUAWriterTaskData.Record> list = new BindingList<OPCUAWriterTaskData.Record>(m_data.Records);
			list.AllowNew = true;
			list.AllowRemove = true;
			dataGrid.DataSource = list;

            string ibaAnalyzerExe;
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                object o = key.GetValue("");
                ibaAnalyzerExe = Path.GetFullPath(o.ToString());
            }
            catch
            {
                ibaAnalyzerExe = iba.Properties.Resources.noIbaAnalyser;
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


        OPCUAWriterTaskData m_data;
        [NonSerialized]
        private AnalyzerManager m_analyzerManager;
        [NonSerialized]
        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit m_channelEditor;

		private void buttonEndpointAdd_Click(object sender, EventArgs e)
		{
			var view = dataGrid.MainView as GridView;
			m_data.Records.Add(new OPCUAWriterTaskData.Record());
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
				view.GetRow(view.FocusedRowHandle) is OPCUAWriterTaskData.Record oldRow)
			{
                var newRow = oldRow.Clone() as OPCUAWriterTaskData.Record;
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
            var bindingList = (dataGrid.DataSource as BindingList<OPCUAWriterTaskData.Record>);

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
			string localPath;
			if (Utility.DatCoordinatorHostImpl.Host.BrowseForPdoFile(ref path, out localPath))
			{
				m_pdoFileTextBox.Text = path;
			}
		}

		private void m_executeIBAAButton_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.OpenPDO(m_pdoFileTextBox.Text);
		}

		private void m_btnUploadPDO_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.UploadPdoFile(sender != null, this, m_pdoFileTextBox.Text, m_analyzerManager, null);
            UpdateSource();
        }

		private void folderNameTextBox_TextChanged(object sender, EventArgs e)
		{
			m_data.FolderName = m_folderNameTextBox.Text;
		}

        private void m_browseDatFileButton_Click(object sender, EventArgs e)
        {
            string datFile = m_datFileTextBox.Text;
            if (Utility.DatCoordinatorHostImpl.Host.BrowseForDatFile(ref datFile, m_data.ParentConfigurationData))
            {
                m_datFileTextBox.Text = datFile;
            }
        }

        private void m_testButton_Click(object sender, EventArgs e)
        {
            using (new WaitCursor())
            {
                m_data.EvaluateTestValues();
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
