using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using iba.Plugins;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using DevExpress.XtraGrid.Views.Grid;

namespace S7_writer_plugin
{
	public partial class S7TaskControl : UserControl, IPluginControl, IGridAnalyzer
	{
        private IDatCoHost m_datcoHost;

		private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit m_channelEditor;

		private IAnalyzerManagerUpdateSource m_analyzerManager;

		public S7TaskControl()
        {
            m_datcoHost = PluginCollection.Host;

            InitializeComponent();
            ((Bitmap)m_testButton.Image).MakeTransparent(Color.Magenta);
			dataGV.CustomDrawRowIndicator += gridExpressionTest_CustomDrawRowIndicator;
			dataGV.IndicatorWidth = 50;

            gridColumn6.Caption = Properties.Resources.ExprTblExpression;
            gridColumn7.Caption = Properties.Resources.ExprTblDB;
            gridColumn8.Caption = Properties.Resources.ExprTblAddress;
            gridColumn9.Caption = Properties.Resources.ExprTblBit;
            gridColumn10.Caption = Properties.Resources.ExprTblDataType;
            gridColumn11.Caption = Properties.Resources.ExprTblTestValue;
        }

		protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_datFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPluginControl Members

        S7TaskData m_data;
        ICommonTaskControl m_control;
        private string ibaAnalyzerExe;

		public void LoadData(object datasource, ICommonTaskControl parentcontrol)
		{
			try
			{
				RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
				object o = key.GetValue("");
				ibaAnalyzerExe = Path.GetFullPath(o.ToString());
			}
			catch
			{
				ibaAnalyzerExe = Properties.Resources.noIbaAnalyzer;
			}
			m_data = datasource as S7TaskData; //we'll assume its never null
			m_control = parentcontrol;
			m_datFileTextBox.Text = m_data.TestDatFile;
			m_pdoFileTextBox.Text = m_data.AnalysisFile;

			BindingList<S7TaskData.Record> list = new BindingList<S7TaskData.Record>(m_data.Records);
			list.AllowNew = true;
			list.AllowRemove = true;


			dataGrid.DataSource = list;
			
			m_channelEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			dataGrid.RepositoryItems.Add(m_channelEditor);
			gridColumn6.ColumnEdit = m_channelEditor;
			
			cbConnType.SelectedIndex = m_data.S7ConnectionType;
			spTimeout.SetIntValue(m_data.S7Timeout);
			tbAddress.Text = m_data.S7Address;
			spRack.SetIntValue(m_data.S7Rack);
			spSlot.SetIntValue(m_data.S7Slot);

			ckAllowErrors.Checked = m_data.AllowErrors;

			UpdateSource();

			m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
			m_cbTime.Checked = m_data.MonitorData.MonitorTime;
			m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
			m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes, 1));
		}

		public void SaveData()
        {
            m_data.TestDatFile = m_datFileTextBox.Text;
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.Records = (dataGrid.DataSource as BindingList<S7TaskData.Record>).ToList();

            m_data.S7ConnectionType = cbConnType.SelectedIndex;
            m_data.S7Timeout = spTimeout.GetIntValue();
            m_data.S7Address = tbAddress.Text;
            m_data.S7Rack = spRack.GetIntValue();
            m_data.S7Slot = spSlot.GetIntValue();
            m_data.AllowErrors = ckAllowErrors.Checked;

            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);

            m_btnUploadPDO_Click(null, null);
        }

        public void LeaveCleanup()
        {
            //nothing to do
            //throw new NotImplementedException();
        }

		#endregion

		void gridExpressionTest_CustomDrawRowIndicator(object sender, DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventArgs e)
		{
			GridView grid = sender as GridView;
			if (e.RowHandle < 0)
				return;

			string strRowNumber = (e.RowHandle + 1).ToString();
			//prepend leading zeros to the string if necessary to improve
			//appearance. For example, if there are ten rows in the grid,
			//row seven will be numbered as "07" instead of "7". Similarly, if 
			//there are 100 rows in the grid, row seven will be numbered as "007".
			while (strRowNumber.Length < dataGV.RowCount.ToString().Length) strRowNumber = "0" + strRowNumber;
			
			e.Info.DisplayText = strRowNumber;
		}

		private void m_browsePDOFileButton_Click(object sender, EventArgs e)
		{
			string path = m_pdoFileTextBox.Text;
			string localPath;
			if (m_datcoHost.BrowseForPdoFile(ref path, out localPath))
			{
				m_pdoFileTextBox.Text = path;
                UpdateSource();
            }
		}

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
		{
			m_datcoHost.OpenPDO(m_pdoFileTextBox.Text, m_datFileTextBox.Text);
		}

        private void m_browseDatFileButton_Click(object sender, EventArgs e)
        {
			string datFile = m_datFileTextBox.Text;
			if (m_datcoHost.BrowseForDatFile(ref datFile, m_data.m_parentJob))
			{
				m_datFileTextBox.Text = datFile;
                UpdateSource();
            }
		}

        private void m_testButton_Click(object sender, EventArgs e)
        {
            IbaAnalyzer.IbaAnalyzer ibaAnalyzer = null;
            //register this
            using(new WaitCursor())
            {
                //start the com object
                try
                {
                    ibaAnalyzer = m_datcoHost.CreateIbaAnalyzer();
                }
                catch(Exception ex2)
                {
                    MessageBox.Show(ex2.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            string version = ibaAnalyzer.GetVersion();
            int startindex = version.IndexOf(' ') + 1;
            if (version[startindex] == 'v') startindex++;
            int stopindex = startindex + 1;
            while(stopindex < version.Length && (char.IsDigit(version[stopindex]) || version[stopindex] == '.'))
                stopindex++;
            string[] nrs = version.Substring(startindex, stopindex - startindex).Split('.');
            if(nrs.Length < 3)
            {
                MessageBox.Show(Properties.Resources.NoVersion, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error); 
            }
            int major;
            if(!Int32.TryParse(nrs[0], out major))
            {
                MessageBox.Show(Properties.Resources.NoVersion, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int minor;
            if(!Int32.TryParse(nrs[1], out minor))
            {
                MessageBox.Show(Properties.Resources.NoVersion, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            int bugfix;
            if(!Int32.TryParse(nrs[2], out bugfix))
            {
                MessageBox.Show(Properties.Resources.NoVersion, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            if(major < 6 || (major == 6 && minor < 5))
                MessageBox.Show(string.Format(Properties.Resources.ibaAnalyzerVersionError, version.Substring(startindex)), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            if(major < 6 || (major == 6 && minor < 7))
                MessageBox.Show(string.Format(Properties.Resources.ibaAnalyzerVersionError, version.Substring(startindex)), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            bool bUseAnalysis = File.Exists(m_pdoFileTextBox.Text);
            bool bUseDatFile = File.Exists(m_datFileTextBox.Text);
            double f = 0;
            try
            {
                using(new WaitCursor())
                {
                    if(bUseAnalysis) ibaAnalyzer.OpenAnalysis(m_pdoFileTextBox.Text);
                    if(bUseDatFile) ibaAnalyzer.OpenDataFile(0,m_datFileTextBox.Text);

                    bool bOneValid = false;
                    S7TaskData.Record[] records = (dataGrid.DataSource as IList<S7TaskData.Record>).ToArray<S7TaskData.Record>();
                    foreach (S7TaskData.Record record in records)
                    {
                        if (!record.IsValid())
                            continue;

                        try
                        {
                            f = ibaAnalyzer.EvaluateDouble(record.Expression, 0);
                        }
                        catch  //might be old ibaAnalyzer
                        {
                            f = (double) ibaAnalyzer.Evaluate(record.Expression, 0);
                        }
                        record.TestValue = f;
                        if (double.IsNaN(f) || double.IsInfinity(f))
                            MessageBox.Show(String.Format(Properties.Resources.BadEvaluate, record.GetOperandName()), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        else
                            bOneValid = true;
                    }

					dataGrid.Refresh();
                    this.ParentForm.Activate();

                    if(!bOneValid)
                        MessageBox.Show(Properties.Resources.NoValidEntriesSpecified, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch(Exception ex3)
            {
                string message = ex3.Message;
                try
                {
                    string ibaMessage = ibaAnalyzer.GetLastError();
                    if(!string.IsNullOrEmpty(ibaMessage))
                        message = ibaMessage;
                }
                catch 
                {
                	
                }
                MessageBox.Show(message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if(ibaAnalyzer != null && bUseAnalysis)
                {
                    (ibaAnalyzer as IDisposable)>.Dispose();
                }
            }
            dataGV.RefreshData();
        }

        private string oldPdo;
        private void m_pdoFileTextBox_TextEnter(object sender, EventArgs e)
        {
            oldPdo = m_pdoFileTextBox.Text;
        }
        private void m_pdoFileTextBox_TextLeave(object sender, EventArgs e)
        {
            string newPdo = m_pdoFileTextBox.Text;
            if (newPdo != oldPdo)
            {
                oldPdo = newPdo;
                UpdateSource();
            }
        }


        private string oldDat;
        private void m_datFileTextBox_TextEnter(object sender, EventArgs e)
        {
            oldDat= m_datFileTextBox.Text;
        }

        private void m_datFileTextBox_TextLeave(object sender, EventArgs e)
        {
            string newDat = m_datFileTextBox.Text;
            if (newDat != oldDat)
            {
                UpdateSource();
            }
		}

		private void buttonEndpointAdd_Click(object sender, EventArgs e)
		{
			var view = dataGrid.MainView as GridView;
			m_data.Records.Add(new S7TaskData.Record());
			view.FocusedRowHandle = m_data.Records.Count - 1;
			view.ShowEditor();

			dataGrid.RefreshDataSource();
			buttonEndpointCopy.Enabled = buttonEndpointRemove.Enabled = (view.FocusedRowHandle >= 0) && (m_data.Records.Count > 0);
		}
		private void buttonEndpointCopy_Click(object sender, EventArgs e)
		{
			var view = dataGrid.MainView as GridView;
			if ((view.FocusedRowHandle >= 0) &&
				(view.FocusedRowHandle < m_data.Records.Count) &&
				view.GetRow(view.FocusedRowHandle) is S7TaskData.Record selEp)
			{
				m_data.Records.Add(selEp.Clone() as S7TaskData.Record);
				view.FocusedRowHandle = m_data.Records.Count - 1;
				view.ShowEditor();

				buttonEndpointCopy.Enabled = buttonEndpointRemove.Enabled = (view.FocusedRowHandle >= 0) && (m_data.Records.Count > 0);
			}
			dataGrid.RefreshDataSource();
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

		private void UpdateSource()
        {
            m_analyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, m_tbPwdDAT.Text, m_data.m_parentJob);
        }

		public void SetGridAnalyzer(DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit e, IAnalyzerManagerUpdateSource analyzer)
		{
			m_channelEditor = e;
			m_analyzerManager = analyzer;
		}

		private void m_btnUploadPDO_Click(object sender, EventArgs e)
		{
			m_datcoHost.UploadPdoFile(sender != null, this, m_pdoFileTextBox.Text, m_analyzerManager, m_data.m_parentJob);
            UpdateSource();
		}

        private void m_btTakeParentPass_Click(object sender, EventArgs e)
        {
            m_tbPwdDAT.Text = m_data.m_parentJob.FileEncryptionPassword;
            m_datFileTextBox_TextLeave(null, null);
        }
    }

    static class NumericUpDownHelper
    {
        public static int GetIntValue(this NumericUpDown spinner)
        {
            return Convert.ToInt32(spinner.Value);
        }

        public static void SetIntValue(this NumericUpDown spinner, int value)
        {
            spinner.Value = Math.Max(spinner.Minimum, Math.Min(spinner.Maximum, value));
        }
    }

    /// </summary>
    internal class WindowsAPI
    {
        #region Shlwapi.dll functions
        [DllImport("Shlwapi.dll")]
        public static extern int SHAutoComplete(IntPtr handle, SHAutoCompleteFlags flags);
        #endregion

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern int SendMessage(IntPtr hWnd, int msg, int wParam, int lParam);
    }



    #region SHAutoCompleteFlags
    [Flags]
    internal enum SHAutoCompleteFlags : uint
    {
        SHACF_DEFAULT = 0x00000000,  // Currently (SHACF_FILESYSTEM | SHACF_URLALL)
        SHACF_FILESYSTEM = 0x00000001,  // This includes the File System as well as the rest of the shell (Desktop\My Computer\Control Panel\)
        SHACF_URLHISTORY = 0x00000002,  // URLs in the User's History
        SHACF_URLMRU = 0x00000004,  // URLs in the User's Recently Used list.
        SHACF_USETAB = 0x00000008,  // Use the tab to move thru the autocomplete possibilities instead of to the next dialog/window control.
        SHACF_FILESYS_ONLY = 0x00000010,  // This includes the File System
        SHACF_FILESYS_DIRS = 0x00000020,  // Same as SHACF_FILESYS_ONLY except it only includes directories, UNC servers, and UNC server shares.
        SHACF_URLALL = (SHACF_URLHISTORY | SHACF_URLMRU),
        SHACF_AUTOSUGGEST_FORCE_ON = 0x10000000,  // Ignore the registry default and force the feature on.
        SHACF_AUTOSUGGEST_FORCE_OFF = 0x20000000,  // Ignore the registry default and force the feature off.
        SHACF_AUTOAPPEND_FORCE_ON = 0x40000000,  // Ignore the registry default and force the feature on. (Also know as AutoComplete)
        SHACF_AUTOAPPEND_FORCE_OFF = 0x80000000   // Ignore the registry default and force the feature off. (Also know as AutoComplete)
    }
    #endregion

    #region WaitCursor
    public class WaitCursor : IDisposable
    {
        private Cursor savedCursor;

        public WaitCursor()
        {
            savedCursor = Cursor.Current;
            Cursor.Current = Cursors.WaitCursor;
        }

        #region IDisposable Members

        public void Dispose()
        {
            Cursor.Current = savedCursor;
        }

        #endregion
    }
    #endregion
}
