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
            gridColumn6.ColumnEdit = m_channelEditor;

            var typeComboBox = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
            foreach (var t in OPCUAWriterTaskData.Record.dataTypes)
                typeComboBox.Items.Add(t);
            gridColumn1.ColumnEdit = typeComboBox;
            gridColumn1.Caption = "Type";
        }

		public void LoadData(object datasource, IPropertyPaneManager manager)
		{
			m_data = datasource as OPCUAWriterTaskData;
            if (m_data.m_analyzerManager is null) // TODO test
                m_data.m_analyzerManager = m_analyzerManager;
            m_pdoFileTextBox.Text = m_data.AnalysisFile;
            m_datFileTextBox.Text = m_data.TestDatFile;
            m_folderNameTextBox.Text = m_data.FolderName;
			BindingList<OPCUAWriterTaskData.Record> list = new BindingList<OPCUAWriterTaskData.Record>(m_data.Records);
			list.AllowNew = true;
			list.AllowRemove = true;
			dataGrid.DataSource = list;
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
				view.GetRow(view.FocusedRowHandle) is OPCUAWriterTaskData.Record selEp)
			{
				m_data.Records.Add(selEp.Clone() as OPCUAWriterTaskData.Record);
				view.FocusedRowHandle = m_data.Records.Count - 1;
				view.ShowEditor();

				buttonEndpointCopy.Enabled = buttonEndpointRemove.Enabled = (view.FocusedRowHandle >= 0) && (m_data.Records.Count > 0);
			}
			dataGrid.RefreshDataSource();
		}

		public void LeaveCleanup()
		{
			//throw new NotImplementedException();
		}

		public void SaveData()
		{
			m_data.Records = (dataGrid.DataSource as BindingList<OPCUAWriterTaskData.Record>).ToList();
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
			m_analyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, "");
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
            // TODO
            //MessageBox.Show("Not implemented");
            m_data.EvaluateTestValues();
            dataGrid.RefreshDataSource();
            ////COPIED FROM S7
            ///
            //IbaAnalyzer.IbaAnalyzer ibaAnalyzer = null;
            ////register this
            //using (new WaitCursor())
            //{
            //    //start the com object
            //    try
            //    {
            //        ibaAnalyzer = new IbaAnalyzer.IbaAnalysis();
            //    }
            //    catch (Exception ex2)
            //    {
            //        MessageBox.Show(ex2.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //        return;
            //    }
            //}

            //string version = ibaAnalyzer.GetVersion();
            //int startindex = version.IndexOf(' ') + 1;
            //int stopindex = startindex + 1;
            //while (stopindex < version.Length && (char.IsDigit(version[stopindex]) || version[stopindex] == '.'))
            //    stopindex++;
            //string[] nrs = version.Substring(startindex, stopindex - startindex).Split('.');
            //if (nrs.Length < 3)
            //{
            //    MessageBox.Show("Properties.Resources.NoVersion1", "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //int major;
            //if (!Int32.TryParse(nrs[0], out major))
            //{
            //    MessageBox.Show("Properties.Resources.NoVersion2", "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //int minor;
            //if (!Int32.TryParse(nrs[1], out minor))
            //{
            //    MessageBox.Show("Properties.Resources.NoVersion2", "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //int bugfix;
            //if (!Int32.TryParse(nrs[2], out bugfix))
            //{
            //    MessageBox.Show("Properties.Resources.NoVersion4", "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //if (major < 6 || (major == 6 && minor < 5))
            //    MessageBox.Show(string.Format("Properties.Resources.ibaAnalyzerVersionError", version.Substring(startindex)), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //if (major < 6 || (major == 6 && minor < 7))
            //    MessageBox.Show(string.Format("Properties.Resources.ibaAnalyzerVersionError", version.Substring(startindex)), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //bool bUseAnalysis = File.Exists(m_pdoFileTextBox.Text);
            //bool bUseDatFile = File.Exists(m_datFileTextBox.Text);
            //double f = 0;
            //try
            //{
            //    using (new WaitCursor())
            //    {
            //        if (bUseAnalysis) ibaAnalyzer.OpenAnalysis(m_pdoFileTextBox.Text);
            //        if (bUseDatFile) ibaAnalyzer.OpenDataFile(0, m_datFileTextBox.Text);

            //        bool bOneValid = false;
            //        var records = (dataGrid.DataSource as BindingList<OPCUAWriterTaskData.Record>).ToList();
            //        foreach (OPCUAWriterTaskData.Record record in records)
            //        {
            //            if (!record.IsValid())
            //                continue;

            //            try
            //            {
            //                f = ibaAnalyzer.EvaluateDouble(record.Expression, 0);
            //            }
            //            catch  //might be old ibaAnalyzer
            //            {
            //                f = (double)ibaAnalyzer.Evaluate(record.Expression, 0);
            //            }
            //            record.TestValue = f;
            //            if (double.IsNaN(f) || double.IsInfinity(f))
            //                MessageBox.Show(String.Format("Properties.Resources.BadEvaluate", record.Expression), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            //            else
            //                bOneValid = true;
            //        }

            //        dataGrid.Refresh();
            //        this.ParentForm.Activate();

            //        if (!bOneValid)
            //            MessageBox.Show("Properties.Resources.NoValidEntriesSpecified", "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //    }
            //}
            //catch (Exception ex3)
            //{
            //    string message = ex3.Message;
            //    try
            //    {
            //        string ibaMessage = ibaAnalyzer.GetLastError();
            //        if (!string.IsNullOrEmpty(ibaMessage))
            //            message = ibaMessage;
            //    }
            //    catch
            //    {

            //    }
            //    MessageBox.Show(message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
            //finally
            //{
            //    if (ibaAnalyzer != null && bUseAnalysis)
            //    {
            //        ibaAnalyzer.CloseAnalysis();
            //        ibaAnalyzer.CloseDataFiles();
            //        System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaAnalyzer);
            //    }
            //}
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            m_analyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, "");
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
        }

        private void m_datFileTextBox_TextChanged(object sender, EventArgs e)
        {
            m_analyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, "");
            m_data.TestDatFile = m_datFileTextBox.Text;
        }
    }
}
