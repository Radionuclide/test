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
        private ChannelTreeEdit channelTreeEdit;
        public SplitterTaskControl()
        {
            InitializeComponent();

            channelTreeEdit = ChannelTreeEdit.CreateInstance(null, ChannelTreeFilter.Expressions | ChannelTreeFilter.Analog | ChannelTreeFilter.Digital);
            channelTreeEdit.Size = channelTreeEditPlaceholder.Size;
            channelTreeEdit.Location = channelTreeEditPlaceholder.Location;
            channelTreeEdit.Anchor = channelTreeEditPlaceholder.Anchor;
            this.groupBox2.Controls.Add(this.channelTreeEdit);

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

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as SplitterTaskData;
			channelTreeEdit.EditValue = m_data.Expression;
			m_splitTypeCBox.SelectedIndex = (int)m_data.EdgeConditionType;
            oldPdo = m_pdoFileTextBox.Text = m_data.AnalysisFile;
            oldDat = m_datFileTextBox.Text = m_data.TestDatFile;

            m_datFileTextBox.Text = m_data.TestDatFile;
            m_tbPwdDAT.Text = m_data.DatFilePassword;

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
                m_monitorGroup.Enabled = VersionCheck.CheckIbaAnalyzerVersion("5.8.1");
            }
            catch
            {
                m_monitorGroup.Enabled = false;
            }

            UpdateSources();
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.TestDatFile = m_datFileTextBox.Text;
            m_data.DatFilePassword = m_tbPwdDAT.Text;
            m_data.Expression = (string)channelTreeEdit.EditValue;
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
                Utility.DatCoordinatorHostImpl.Host.UploadPdoFileWithReturnValue(false, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
            }
            else if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                Utility.DatCoordinatorHostImpl.Host.UploadPdoFileWithReturnValue(false, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
            }
        }

        public void LeaveCleanup()
        {
            channelTreeEdit.AnalyzerManager.OnLeave();
        }

        #endregion

        private void m_browsePDOFileButton_Click(object sender, EventArgs e)
		{
			string path = m_pdoFileTextBox.Text;
			string localPath;
			if (Utility.DatCoordinatorHostImpl.Host.BrowseForPdoFile(ref path, out localPath))
			{
				m_pdoFileTextBox.Text = path;
                UpdateSources();
            }
		}

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.OpenPDO(m_pdoFileTextBox.Text, m_datFileTextBox.Text);
		}

        private void m_browseDatFileButton_Click(object sender, EventArgs e)
        {
			string datFile = m_datFileTextBox.Text;
			if (Utility.DatCoordinatorHostImpl.Host.BrowseForDatFile(ref datFile, m_data.ParentConfigurationData))
			{
				m_datFileTextBox.Text = datFile;
                UpdateSources();
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

		private void m_btnUploadPDO_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.UploadPdoFileWithReturnValue(true, this, m_pdoFileTextBox.Text, null, m_data.ParentConfigurationData);
            UpdateSources();
        }

        string oldPdo;
        private void m_pdoFileTextBox_TextEnter(object sender, EventArgs e)
        {
            oldPdo = m_pdoFileTextBox.Text;
        }

        private void m_pdoFileTextBox_TextLeave(object sender, EventArgs e)
		{
            string newPdo = m_pdoFileTextBox.Text;
            if (newPdo != oldPdo)
            {
                UpdateSources();
                oldPdo = newPdo;
            }
		}

        private void UpdateSources()
        {
            channelTreeEdit.AnalyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, m_tbPwdDAT.Text, m_data.ParentConfigurationData);
        }

        private void m_btTakeParentPass_Click(object sender, EventArgs e)
        {
            m_tbPwdDAT.Text = m_data.ParentConfigurationData.FileEncryptionPassword;
            m_datFileTextBox_TextLeave(null, null);
        }


    }
}
