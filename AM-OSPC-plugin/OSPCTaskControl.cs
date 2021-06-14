using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iba.Plugins;
using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.IO;
using DevExpress.XtraGrid.Views.Grid;

namespace AM_OSPC_plugin
{
    public partial class OSPCTaskControl : UserControl, IPluginControl, IGridAnalyzer
	{
        private IDatCoHost m_datcoHost;
		[NonSerialized]
		private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit m_channelEditor;
		[NonSerialized]
		private IAnalyzerManagerUpdateSource m_analyzerManager;

		public OSPCTaskControl()
        {
            m_datcoHost = PluginCollection.Host;
            InitializeComponent();
            ((Bitmap)m_testButton.Image).MakeTransparent(Color.Magenta);
			dataGV.CustomDrawRowIndicator += gridExpressionTest_CustomDrawRowIndicator;
			dataGV.IndicatorWidth = 50;

            m_datcoHost.
            gridColumn6.Caption = Properties.Resources.ExprTblExpression;
            gridColumn7.Caption = Properties.Resources.ExprTblProcessName;
            gridColumn8.Caption = Properties.Resources.ExprTblVarName;
            gridColumn9.Caption = Properties.Resources.ExprTblTestVal;
            m_toolTip.SetToolTip(m_btnUploadPDO, Program.RunsWithService == Program.ServiceEnum.NOSERVICE ? Properties.Resources.HDEventTask_ToolTip_UploadPDOStandAlone : Properties.Resources.HDEventTask_ToolTip_UploadPDO);
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

        OSPCTaskData m_data;
        ICommonTaskControl m_control;

        public void LoadData(object datasource, ICommonTaskControl parentcontrol)
        {
            m_data = datasource as OSPCTaskData; //we'll assume its never null
            m_control = parentcontrol;
            oldDat = m_datFileTextBox.Text = m_data.TestDatFile;
            oldPdo = m_pdoFileTextBox.Text = m_data.AnalysisFile;
            dataGrid.DataSource = m_data.Records.ToList();

			m_channelEditor.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard;
			dataGrid.RepositoryItems.Add(m_channelEditor);
			gridColumn6.ColumnEdit = m_channelEditor;

			m_ospcHost.Text = m_data.OspcServerHost;
            m_ospcUsername.Text = m_data.OspcServerUser;
            m_ospcPassword.Text = m_data.OspcServerPassword;
            UpdateSources();

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes, 1));
        }

        
        public void SaveData()
        {
            m_data.TestDatFile = m_datFileTextBox.Text;
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.Records = (dataGrid.DataSource as IList<OSPCTaskData.Record>).ToArray<OSPCTaskData.Record>();

            m_data.OspcServerHost = m_ospcHost.Text;
            m_data.OspcServerUser = m_ospcUsername.Text;
            m_data.OspcServerPassword = m_ospcPassword.Text;

            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);

            m_btnUploadPDO_Click(null, null);
        }

        public void LeaveCleanup()
        {
            m_analyzerManager.OnLeave();
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
                UpdateSources();
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
                UpdateSources();
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
            bool bUseAnalysis = m_datcoHost.FileExists(m_pdoFileTextBox.Text);
            bool bUseDatFile = m_datcoHost.FileExists(m_datFileTextBox.Text);
            double f = 0;
            try
            {
                using(new WaitCursor())
                {
                    if(bUseAnalysis) ibaAnalyzer.OpenAnalysis(m_pdoFileTextBox.Text);
                    if (bUseDatFile)
                    {

                        ibaAnalyzer.OpenDataFile(0, m_datFileTextBox.Text);
                    }
                    OSPCTaskData.Record[] records = (dataGrid.DataSource as IList<OSPCTaskData.Record>).ToArray<OSPCTaskData.Record>();
                    foreach (OSPCTaskData.Record record in records)
                    {
                        if (string.IsNullOrEmpty(record.VariableName) && string.IsNullOrEmpty(record.Expression)) continue;
                        try
                        {
                            f = ibaAnalyzer.EvaluateDouble(record.Expression, 0);
                        }
                        catch  //might be old ibaAnalyzer
                        {
                            f = (double) ibaAnalyzer.Evaluate(record.Expression, 0);
                        }
                        record.TestValue = f;
                        if(double.IsNaN(f) || double.IsInfinity(f))
                        {
                           
                            MessageBox.Show(String.Format(Properties.Resources.BadEvaluate,record.VariableName), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    dataGrid.DataSource = records.ToList();
                    this.ParentForm.Activate();
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
                if(ibaAnalyzer != null )
                {
                    (ibaAnalyzer as IDisposable)?.Dispose();
                }
            }
        }

        private void m_datagvMessages_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            DataGridView grid = sender as DataGridView;
            if(grid == null) return;
            //store a string representation of the row number in 'strRowNumber'
            string strRowNumber = (e.RowIndex + 1).ToString();

            //prepend leading zeros to the string if necessary to improve
            //appearance. For example, if there are ten rows in the grid,
            //row seven will be numbered as "07" instead of "7". Similarly, if 
            //there are 100 rows in the grid, row seven will be numbered as "007".
            while(strRowNumber.Length < grid.RowCount.ToString().Length) strRowNumber = "0" + strRowNumber;

            //determine the display size of the row number string using
            //the DataGridView's current font.
            SizeF size = e.Graphics.MeasureString(strRowNumber, this.Font);

            //adjust the width of the column that contains the row header cells 
            //if necessary
            if(grid.RowHeadersWidth < (int)(size.Width + 20)) grid.RowHeadersWidth = (int)(size.Width + 20);

            //this brush will be used to draw the row number string on the
            //row header cell using the system's current ControlText color
            Brush b = SystemBrushes.ControlText;

            //draw the row number string on the current row header cell using
            //the brush defined above and the DataGridView's default font
            e.Graphics.DrawString(strRowNumber, this.Font, b, e.RowBounds.Location.X + 15, e.RowBounds.Location.Y + ((e.RowBounds.Height - size.Height) / 2));   
        }


        string oldPdo;
        private void m_pdoFileTextBox_TextEnter(object sender, EventArgs e)
        {
            oldPdo = m_pdoFileTextBox.Text;
        }

        private void m_pdoFileTextBox_TextLeave(object sender, EventArgs e)
        {
            string newPdo = m_pdoFileTextBox.Text;
            if (oldPdo != newPdo)
            {
                oldPdo = newPdo;
                UpdateSources();
            }
		}

        string oldDat;
        private void m_datFileTextBox_TextEnter(object sender, EventArgs e)
        {
            oldDat = m_datFileTextBox.Text;
        }

        private void m_datFileTextBox_TextLeave(object sender, EventArgs e)
        {
            string newDat = m_datFileTextBox.Text;
            if (oldDat != newDat)
            {
                UpdateSources();
                oldDat = newDat;
            }
        }

        private void UpdateSources()
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
            UpdateSources();
        }

        private void m_btTakeParentPass_Click(object sender, EventArgs e)
        {
            m_tbPwdDAT.Text = m_data.m_parentJob.FileEncryptionPassword;
            m_datFileTextBox_TextLeave(null, null);
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
