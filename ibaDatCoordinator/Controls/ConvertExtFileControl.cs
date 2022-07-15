using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;

using iba;
using iba.Data;
using iba.Utility;
using iba.Processing;
using Microsoft.Win32;

namespace iba.Controls
{
    public partial class ConvertExtFileControl : UserControl, IPropertyPane
    {
        public ConvertExtFileControl()
        {
            InitializeComponent();
            InitializeIcons();
            m_uncControl = new UNCTaskControl();
            groupBox3.Controls.Add(m_uncControl);
            
            m_uncControl.Dock = DockStyle.Fill;

            m_toolTip.SetToolTip(m_executeIBAAButton, Properties.Resources.HDEventTask_ToolTip_OpenPDO);
            m_toolTip.SetToolTip(m_btnUploadPDO, Program.RunsWithService == Program.ServiceEnum.NOSERVICE ? Properties.Resources.HDEventTask_ToolTip_UploadPDOStandAlone : Properties.Resources.HDEventTask_ToolTip_UploadPDO);
            m_toolTip.SetToolTip(m_browseFileButton, Properties.Resources.ToolTip_BrowsePDO);
        }

        private void InitializeIcons()
        {
            this.m_btnUploadPDO.Image = Icons.Gui.All.Images.FilePdoUpload();
            this.m_executeIBAAButton.Image = Icons.SystemTray.Images.IbaAnalyzer();
            this.m_browseFileButton.Image = Icons.Gui.All.Images.FolderOpen();
        }

        private UNCTaskControl m_uncControl;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        ConvertExtFileTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as ConvertExtFileTaskData;

            m_pdoFileTextBox.Text = m_data.AnalysisFile;

            m_uncControl.SetData(m_data);

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum,Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes, 1));
            try
            {
                m_monitorGroup.Enabled = VersionCheck.CheckIbaAnalyzerVersion("5.8.1");
            }
            catch
            {
                m_monitorGroup.Enabled = false;
            }
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFileTextBox.Text;

            
            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);

            m_uncControl.SaveData();
            m_data.UpdateUNC();

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

        public void LeaveCleanup()
        {
        }

        #endregion

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.OpenPDO(m_pdoFileTextBox.Text);
		}

		private void m_browseFileButton_Click(object sender, EventArgs e)
		{
			string path = m_pdoFileTextBox.Text;
			string localPath;
			if (Utility.DatCoordinatorHostImpl.Host.BrowseForPdoFile(ref path, out localPath))
			{
				m_pdoFileTextBox.Text = path;
			}
		}

        private void m_btnUploadPDO_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.UploadPdoFileWithReturnValue(true, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
		}
	}
}
