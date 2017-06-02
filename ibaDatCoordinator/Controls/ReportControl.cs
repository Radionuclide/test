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
            ((Bitmap)m_executeIBAAButton.Image).MakeTransparent(Color.Magenta);
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
                m_executeIBAAButton.Enabled = true; //we'll give a warning when not allowed ...
            else
                m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                    File.Exists(ibaAnalyzerExe);

            m_panelFile.Enabled = m_rbFile.Checked;

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal) Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes,1));

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

        private String ibaAnalyzerExe;
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
        }

        public void LeaveCleanup() { }

        #endregion

        private void m_browseFileButton_Click(object sender, EventArgs e)
        {
            string path = m_pdoFileTextBox.Text;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
            {
                using (iba.Controls.ServerFolderBrowser fd = new iba.Controls.ServerFolderBrowser(true))
                {
                    fd.FixedDrivesOnly = false;
                    fd.ShowFiles = true;
                    fd.SelectedPath = path;
                    fd.Filter = "ibaAnalyzer PDO files (*.pdo)|*.pdo";
                    if (fd.ShowDialog(this) == DialogResult.OK)
                    {
                        m_pdoFileTextBox.Text = fd.SelectedPath;
                    }
                }
            }
            else
            {
                m_openFileDialog1.Filter = "ibaAnalyzer PDO files (*.pdo)|*.pdo";
                if (System.IO.File.Exists(path))
                    m_openFileDialog1.FileName = path;
                else if (System.IO.Directory.Exists(path))
                    m_openFileDialog1.InitialDirectory = path;
                DialogResult result = m_openFileDialog1.ShowDialog();
                if (result == DialogResult.OK)
                    m_pdoFileTextBox.Text = m_openFileDialog1.FileName;
            }
        }

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
        {
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
            {
                MessageBox.Show(String.Format(iba.Properties.Resources.ServiceRemoteAnalyserNotSupported, Program.ServiceHost), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = ibaAnalyzerExe;
                    ibaProc.StartInfo.Arguments = "\"" + m_pdoFileTextBox.Text + "\"";
                    ibaProc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
                m_executeIBAAButton.Enabled = true; //we'll give a warning when not allowed ...
            else
                m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                    File.Exists(ibaAnalyzerExe);
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
    }
}
