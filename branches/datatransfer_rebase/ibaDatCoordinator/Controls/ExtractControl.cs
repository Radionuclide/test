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
    public partial class ExtractControl : UserControl, IPropertyPane
    {
        public ExtractControl()
        {
            InitializeComponent();
            m_uncControl = new UNCTaskControl();
            m_panelFile.Controls.Add(m_uncControl);
            Controls.Remove(m_groupBoxFileType);
            int newtabindex = m_groupBoxFileType.TabIndex;
            int offset = 6 + m_groupBoxFileType.Size.Height;
            int fromHere = m_uncControl.m_subfolderGroupBox.Location.Y;
            foreach (Control c in m_uncControl.Controls)
            {
                if (c.Location.Y >= fromHere)
                    c.Location = new Point(c.Location.X, c.Location.Y + offset);
                c.TabIndex++;
            }
            int newHeight = m_monitorGroup.Bottom + 5;
            this.MinimumSize = new System.Drawing.Size(this.MinimumSize.Width, newHeight);
            this.Height = newHeight;
            m_uncControl.Controls.Add(m_groupBoxFileType);
            m_groupBoxFileType.Location = new Point(m_uncControl.m_subfolderGroupBox.Location.X,fromHere);
            m_groupBoxFileType.Width = m_uncControl.m_subfolderGroupBox.Width;
            m_groupBoxFileType.TabIndex = newtabindex;
            m_uncControl.Dock = DockStyle.Fill;
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
        ExtractData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as ExtractData;

            m_pdoFileTextBox.Text = m_data.AnalysisFile;

            m_uncControl.SetData(m_data);

            m_rbFile.Checked = m_data.ExtractToFile;
            m_rbDbase.Checked = !m_data.ExtractToFile;
            m_panelFile.Enabled = m_rbFile.Checked;

            m_rbBinaryFile.Checked = m_data.FileType == ExtractData.ExtractFileType.BINARY;
            m_rbTextFile.Checked = m_data.FileType == ExtractData.ExtractFileType.TEXT;
            m_rbTDMS.Checked = m_data.FileType == ExtractData.ExtractFileType.TDMS;
            m_rbComtrade.Checked = m_data.FileType == ExtractData.ExtractFileType.COMTRADE;
            m_rbParquet.Checked = m_data.FileType == ExtractData.ExtractFileType.PARQUET;
			m_rbMatLab.Checked = m_data.FileType == ExtractData.ExtractFileType.MATLAB;


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
            m_data.ExtractToFile = m_rbFile.Checked;

            if (m_rbBinaryFile.Checked)
                m_data.FileType = ExtractData.ExtractFileType.BINARY;
            else if (m_rbTextFile.Checked)
                m_data.FileType = ExtractData.ExtractFileType.TEXT;
            else if (m_rbComtrade.Checked)
                m_data.FileType = ExtractData.ExtractFileType.COMTRADE;
            else if (m_rbTDMS.Checked)
                m_data.FileType = ExtractData.ExtractFileType.TDMS;
            else if (m_rbParquet.Checked)
                m_data.FileType = ExtractData.ExtractFileType.PARQUET;
			else
				m_data.FileType = ExtractData.ExtractFileType.MATLAB;
			m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint) m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double) m_nudTime.Value);

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

        private void m_rbDbase_CheckedChanged(object sender, EventArgs e)
        {
            m_panelFile.Enabled = m_rbFile.Checked;
        }

		private void m_btnUploadPDO_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.UploadPdoFileWithReturnValue(true, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
		}
	}
}
