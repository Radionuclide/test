using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

using iba;
using iba.Data;
using iba.Utility;
using iba.Processing;
using Microsoft.Win32;

namespace iba.Controls
{
    public partial class ReportControl : UserControl, IPropertyPane
    {
        public ReportControl()
        {
            InitializeComponent();
            m_extensionComboBox.Items.AddRange(new object[]{
                "pdf",
                "htm",
                "html",
                "mht",
                "mhtml",
                "txt",
                "xls",
                "rtf",
                "tif",
                "tiff",
                "emf",
                "jpg",
                "jpeg",
                "bmp",
                "xml"
            });
            m_uncControl = new UNCTaskControl();
            m_panelFile.Controls.Add(m_uncControl);
            m_uncControl.Dock = DockStyle.Fill;
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);

        //}

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        ReportData m_data;
        
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            if (Program.RunsWithService != Program.ServiceEnum.CONNECTED || Program.ServiceIsLocal) //will be called multiple times, causes leak in XP
            {
                WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
                    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            }
            else
            {
                WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
                    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_OFF | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_OFF);
            }
            m_manager = manager;
            m_data = datasource as ReportData;
            m_pdoFileTextBox.Text = m_data.AnalysisFile;

            m_rbFile.Checked = m_data.Output == ReportData.OutputChoice.FILE;
            m_rbPrint.Checked = m_data.Output == ReportData.OutputChoice.PRINT;
            m_extensionComboBox.SelectedIndex = m_extensionComboBox.FindString(m_data.Extension);
            m_cbImageSubDirs.Checked = m_data.CreateSubFolderForImageTypes;

            m_panelFile.Enabled = m_rbFile.Checked;

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal) Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes,1));

            m_uncControl.SetData(m_data);
            m_monitorGroup.Enabled = VersionCheck.CheckIbaAnalyzerVersion("5.8.1");
        }

        private UNCTaskControl m_uncControl;

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            if (m_rbPrint.Checked) m_data.Output = ReportData.OutputChoice.PRINT;
            else if (m_rbFile.Checked) m_data.Output = ReportData.OutputChoice.FILE;
            m_data.Extension = (string) m_extensionComboBox.SelectedItem;
            m_data.CreateSubFolderForImageTypes = m_cbImageSubDirs.Checked;

            m_uncControl.SaveData();
            m_data.UpdateUNC();

            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);


            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
            {
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
                Utility.DatCoordinatorHostImpl.Host.UploadPdoFileWithReturnValue(false, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
            }
            else if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                Utility.DatCoordinatorHostImpl.Host.UploadPdoFileWithReturnValue(false, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
            }
        }

        public void LeaveCleanup() { }

        #endregion

        private void m_browseFileButton_Click(object sender, EventArgs e)
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
      
        private void m_rbPrint_CheckedChanged(object sender, EventArgs e)
        {
            m_panelFile.Enabled = m_rbFile.Checked;
        }

        private void m_extensionComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            string ext = (string) m_extensionComboBox.SelectedItem;
            m_cbImageSubDirs.Enabled = String.IsNullOrEmpty(ext) || ReportData.ImageExtensions.Contains(ext);
        }

		private void m_btnUploadPDO_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.UploadPdoFileWithReturnValue(true, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
		}
	}
}
