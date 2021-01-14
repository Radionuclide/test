using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Diagnostics;

using iba.Utility;
using iba.Data;
using iba.Processing;
using iba.Dialogs;
using Microsoft.Win32;

namespace iba.Controls
{
    public partial class SplitterTaskControl : UserControl, IPropertyPane
    {
        public SplitterTaskControl()
        {
            InitializeComponent();
            ((Bitmap)m_executeIBAAButton.Image).MakeTransparent(Color.Magenta);
            WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_datFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            m_uncControl = new UNCTaskControl();
            panelOut.Controls.Add(m_uncControl);
            m_uncControl.Dock = DockStyle.Fill;
        }

        private UNCTaskControl m_uncControl;

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private SplitterTaskData m_data;
        private string ibaAnalyzerExe;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as SplitterTaskData;
			channelTreeEdit1.EditValue = m_data.Expression;
			m_splitTypeCBox.SelectedIndex = (int)m_data.EdgeConditionType;
            m_pdoFileTextBox.Text = m_data.AnalysisFile;
            m_datFileTextBox.Text = m_data.TestDatFile;

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

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
                m_testButton.Enabled = true;
            else
            {
                m_testButton.Enabled = File.Exists(m_datFileTextBox.Text) && m_executeIBAAButton.Enabled;
            }

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes, 1));

            m_uncControl.SetData(m_data);

            try
            {
                m_monitorGroup.Enabled = VersionCheck.CheckVersion(ibaAnalyzerExe, "5.8.1");
            }
            catch
            {
                m_monitorGroup.Enabled = false;
            }
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.TestDatFile = m_datFileTextBox.Text;
			m_data.Expression = (string)channelTreeEdit1.EditValue;
			m_data.EdgeConditionType = m_splitTypeCBox.SelectedIndex == 1 ? SplitterTaskData.EdgeConditionTypeEnum.RISINGTOFALLING : SplitterTaskData.EdgeConditionTypeEnum.RISINGTORISING;
            
            m_uncControl.SaveData();
            m_data.UpdateUNC();

            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
            {
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
                Utility.DatCoordinatorHostImpl.Host.UploadPdoFile(false, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
            }
            else if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                Utility.DatCoordinatorHostImpl.Host.UploadPdoFile(false, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
            }
        }

        public void LeaveCleanup()
        {
            return;
        }

        #endregion

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
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
            {
                MessageBox.Show(String.Format(iba.Properties.Resources.ServiceRemoteSplitterNotSupported,Program.ServiceHost), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SaveData();
            TestSplitterTaskDialog dlg = new TestSplitterTaskDialog(m_data);
            if (!dlg.IsDisposed)
                dlg.ShowDialog();
        }

		private void m_datFileTextBox_TextChanged(object sender, EventArgs e)
        {
			channelTreeEdit1.analyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, "");
			if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
                m_testButton.Enabled = true; //we'll give a warning when not allowed ...
            else
                m_testButton.Enabled = File.Exists(m_datFileTextBox.Text) &&
                    File.Exists(m_data.ParentConfigurationData.IbaAnalyzerExe);
        }

		private void m_btnUploadPDO_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.UploadPdoFile(true, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
		}

		private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
		{
			channelTreeEdit1.analyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, "");
		}
	}
}
