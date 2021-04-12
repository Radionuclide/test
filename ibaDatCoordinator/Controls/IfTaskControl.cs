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
using Microsoft.Win32;
using iba.Remoting;

namespace iba.Controls
{
    public partial class IfTaskControl : UserControl, IPropertyPane
    {
        private ChannelTreeEdit channelTreeEdit;
        public IfTaskControl()
        {
            InitializeComponent();

            channelTreeEdit = new iba.Controls.ChannelTreeEdit();
            channelTreeEdit.Size = channelTreeEditPlaceholder.Size;
            channelTreeEdit.Location = channelTreeEditPlaceholder.Location;
            channelTreeEdit.Anchor = channelTreeEditPlaceholder.Anchor;
            this.groupBox2.Controls.Add(this.channelTreeEdit);

            m_XTypeComboBox.Items.AddRange(new object[]{
                iba.Properties.Resources.XTime,
                iba.Properties.Resources.XLength,
                iba.Properties.Resources.XFrequency,
                iba.Properties.Resources.XInvLength
            });
            WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_datFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
		}

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private IfTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as IfTaskData;
			channelTreeEdit.EditValue = m_data.Expression;
            oldPdo = m_pdoFileTextBox.Text = m_data.AnalysisFile;
            oldDat = m_datFileTextBox.Text = m_data.TestDatFile;
            m_tbPwdDAT.Text = m_data.DatFilePassword;


            m_testButton.Enabled = DataPath.FileExists(m_datFileTextBox.Text);
            m_XTypeComboBox.SelectedIndex = (int)m_data.XType;

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes, 1));
            m_monitorGroup.Enabled = VersionCheck.CheckIbaAnalyzerVersion("5.8.1");
            m_testButton.Enabled = DataPath.FileExists(m_datFileTextBox.Text); 
            UpdateSources();
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.TestDatFile = m_datFileTextBox.Text;
            m_data.DatFilePassword = m_tbPwdDAT.Text;
            m_data.Expression = (string)channelTreeEdit.EditValue;
			m_data.XType = (IfTaskData.XTypeEnum) m_XTypeComboBox.SelectedIndex;

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
            channelTreeEdit.analyzerManager.OnLeave();
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
			if (Utility.DatCoordinatorHostImpl.Host.BrowseForDatFile(ref datFile, m_data?.ParentConfigurationData))
			{
				m_datFileTextBox.Text = datFile;
			}
            if (datFile != oldDat)
            {
                oldDat = datFile;
                UpdateSources();
            }
        }

        private void UpdateSources()
        {
            channelTreeEdit.analyzerManager.UpdateSource(m_pdoFileTextBox.Text, m_datFileTextBox.Text, m_tbPwdDAT.Text, m_data.ParentConfigurationData);
        }

        private void m_testButton_Click(object sender, EventArgs e)
        {
            float f;
            string errorMessage;
            using (new Utility.WaitCursor())
            {
                string pass = m_tbPwdDAT.Text;
                string user = "";
                bool doHD = false;
                if (!String.IsNullOrEmpty(m_datFileTextBox.Text) && Path.GetExtension(m_datFileTextBox.Text) == ".hdq")
                {
                    if( m_data.ParentConfigurationData.JobType == ConfigurationData.JobTypeEnum.Event)
                    {
                        if (m_data.ParentConfigurationData.EventData != null && !string.IsNullOrEmpty(m_data.ParentConfigurationData.EventData.HDPassword))
                        {
                            doHD = true;
                            pass = m_data.ParentConfigurationData.EventData.HDPassword;
                            user = m_data.ParentConfigurationData.EventData.HDUsername;
                        }
                    }
                    if (m_data.ParentConfigurationData.JobType == ConfigurationData.JobTypeEnum.Scheduled)
                    {
                        if (m_data.ParentConfigurationData.ScheduleData != null && !string.IsNullOrEmpty(m_data.ParentConfigurationData.ScheduleData.HDPassword))
                        {
                            doHD = true;
                            pass = m_data.ParentConfigurationData.ScheduleData.HDPassword;
                            user = m_data.ParentConfigurationData.ScheduleData.HDUsername;
                        }
                    }
                }
                try
                {
                    if (doHD)
                        f = TestConditionHD(channelTreeEdit.Text, m_XTypeComboBox.SelectedIndex, m_pdoFileTextBox.Text, m_datFileTextBox.Text, user, pass, out errorMessage);
                    else
                        f = TestCondition(channelTreeEdit.Text, m_XTypeComboBox.SelectedIndex, m_pdoFileTextBox.Text, m_datFileTextBox.Text, pass, out errorMessage);
                }
                catch (Exception ex)
                {
                    f = float.NaN;
                    errorMessage = ex.Message;
                }
            }
            if (float.IsNaN(f) || float.IsInfinity(f))
                MessageBox.Show(errorMessage, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (f >= 0.5)
                MessageBox.Show(iba.Properties.Resources.IfTestPositiveEvaluation, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(iba.Properties.Resources.IfTestNegativeEvaluation, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        static internal float TestCondition(string expression, int index, string pdo, string datfile, string passwd, out string errorMessage)
        {
            return TestConditionHD(expression, index, pdo, datfile, "", passwd, out errorMessage);
        }

        static internal float TestConditionHD(string expression, int index, string pdo, string datfile, string user, string passwd, out string errorMessage)
        {
            IbaAnalyzer.IbaAnalyzer ibaAnalyzer = null;
            //register this
            //start the com object
            try
            {
                ibaAnalyzer = ibaAnalyzerExt.Create(true);
            }
            catch (Exception ex2)
            {
                errorMessage = ex2.Message;
                return float.NaN;
            }
            bool bUseAnalysis = DataPath.FileExists(pdo);
            float f = 0;
            try
            {
                if (bUseAnalysis) ibaAnalyzer.OpenAnalysis(pdo);
                if (!string.IsNullOrEmpty(user)) //actual HD case
                {
                    ibaAnalyzer.SetHDCredentials(user, passwd);
                }
                else if (!String.IsNullOrEmpty(passwd))
                    ibaAnalyzer.SetFilePassword("", passwd);
                ibaAnalyzer.OpenDataFile(0, datfile);
                f = ibaAnalyzer.Evaluate(expression, index);
            }
            catch (Exception ex3)
            {
                errorMessage = ex3.Message;
                return float.NaN;
            }
            finally
            {
                if (ibaAnalyzer != null)
                {
                    ((IDisposable)ibaAnalyzer)?.Dispose();
                }
            }
            errorMessage = (float.IsNaN(f) || float.IsInfinity(f)) ? iba.Properties.Resources.IfTestBadEvaluation : "";
            return f;

        }

        private string oldPdo;
        private void m_pdoFileTextBox_TextLeave(object sender, EventArgs e)
        {
            string newPdo = m_pdoFileTextBox.Text;
            if (newPdo != oldPdo)
            {
                oldPdo = newPdo;
                UpdateSources();
            }
        }

        private string oldDat;
        private void m_datFileTextBox_TextLeave(object sender, EventArgs e)
		{
            string newDat = m_datFileTextBox.Text;
            if (oldDat != newDat)
            {
                oldDat = newDat;
                UpdateSources();
            }
        }

		private void m_btnUploadPDO_Click(object sender, EventArgs e)
		{
			Utility.DatCoordinatorHostImpl.Host.UploadPdoFileWithReturnValue(true, this, m_pdoFileTextBox.Text, channelTreeEdit.analyzerManager, m_data.ParentConfigurationData);
            UpdateSources();
		}

        private void m_btTakeParentPass_Click(object sender, EventArgs e)
        {
            m_tbPwdDAT.Text = m_data.ParentConfigurationData.FileEncryptionPassword;
        }

        private void m_pdoFileTextBox_TextEnter(object sender, EventArgs e)
        {
            oldPdo = m_pdoFileTextBox.Text;
        }

        private void m_datFileTextBox_TextEnter(object sender, EventArgs e)
        {
            oldDat = m_datFileTextBox.Text;
        }
    }
}
