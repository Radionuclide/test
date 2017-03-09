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

namespace iba.Controls
{
    public partial class IfTaskControl : UserControl, IPropertyPane
    {
        public IfTaskControl()
        {
            InitializeComponent();
            m_XTypeComboBox.Items.AddRange(new object[]{
                iba.Properties.Resources.XTime,
                iba.Properties.Resources.XLength,
                iba.Properties.Resources.XFrequency,
                iba.Properties.Resources.XInvLength
            });
            ((Bitmap)m_executeIBAAButton.Image).MakeTransparent(Color.Magenta);
            WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_datFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private IfTaskData m_data;
        private string ibaAnalyzerExe;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as IfTaskData;
            m_expressionTextBox.Text = m_data.Expression;
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

            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                File.Exists(ibaAnalyzerExe);

            m_testButton.Enabled = File.Exists(m_datFileTextBox.Text) && m_executeIBAAButton.Enabled;
            m_XTypeComboBox.SelectedIndex = (int)m_data.XType;

            m_cbMemory.Checked = m_data.MonitorData.MonitorMemoryUsage;
            m_cbTime.Checked = m_data.MonitorData.MonitorTime;
            m_nudMemory.Value = Math.Max(m_nudMemory.Minimum, Math.Min(m_nudMemory.Maximum, m_data.MonitorData.MemoryLimit));
            m_nudTime.Value = (Decimal)Math.Min(300, Math.Max(m_data.MonitorData.TimeLimit.TotalMinutes, 1));
            try
            {
                m_monitorGroup.Enabled = VersionCheck.CheckVersion(ibaAnalyzerExe, "5.8.1");
            }
            catch
            {
                m_monitorGroup.Enabled = false;
            }

            m_testButton.Enabled = File.Exists(m_datFileTextBox.Text) &&
            File.Exists(m_data.ParentConfigurationData.IbaAnalyzerExe);
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.TestDatFile = m_datFileTextBox.Text;
            m_data.Expression = m_expressionTextBox.Text;
            m_data.XType = (IfTaskData.XTypeEnum) m_XTypeComboBox.SelectedIndex;

            m_data.MonitorData.MonitorMemoryUsage = m_cbMemory.Checked;
            m_data.MonitorData.MonitorTime = m_cbTime.Checked;
            m_data.MonitorData.MemoryLimit = (uint)m_nudMemory.Value;
            m_data.MonitorData.TimeLimit = TimeSpan.FromMinutes((double)m_nudTime.Value);

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        public void LeaveCleanup()
        {
            return;
        }

        #endregion

        private void m_browsePDOFileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog.CheckFileExists = true;
            m_openFileDialog.Filter = "ibaAnalyzer PDO files (*.pdo)|*.pdo";
            DialogResult result = m_openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                m_pdoFileTextBox.Text = m_openFileDialog.FileName;
        }

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
        {
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

        private void m_browseDatFileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog.CheckFileExists = true;
            m_openFileDialog.Filter = "iba dat files (*.dat)|*.dat";
            DialogResult result = m_openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                m_datFileTextBox.Text = m_openFileDialog.FileName;
        }

        private void m_testButton_Click(object sender, EventArgs e)
        {
            IbaAnalyzer.IbaAnalyzer ibaAnalyzer = null;
            //register this
            using (new Utility.WaitCursor())
            {
                //start the com object
                try
                {
                    ibaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                }
                catch (Exception ex2)
                {
                    MessageBox.Show(ex2.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }

            bool bUseAnalysis = File.Exists(m_pdoFileTextBox.Text);
            float f = 0;
            try
            {
                using (new Utility.WaitCursor())
                {
                    if (bUseAnalysis) ibaAnalyzer.OpenAnalysis(m_pdoFileTextBox.Text);
                    ibaAnalyzer.OpenDataFile(0,m_datFileTextBox.Text);
                    f = ibaAnalyzer.Evaluate(m_expressionTextBox.Text, m_XTypeComboBox.SelectedIndex);
                    this.ParentForm.Activate();
                }
            }
            catch (Exception ex3)
            {
                MessageBox.Show(ex3.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (ibaAnalyzer != null && bUseAnalysis)
                {
                    ibaAnalyzer.CloseAnalysis();
                    ibaAnalyzer.CloseDataFiles();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaAnalyzer);
                }
            }
            if (float.IsNaN(f) || float.IsInfinity(f))
                MessageBox.Show(iba.Properties.Resources.IfTestBadEvaluation, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else if (f >= 0.5)
                MessageBox.Show(iba.Properties.Resources.IfTestPositiveEvaluation, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show(iba.Properties.Resources.IfTestNegativeEvaluation, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
            File.Exists(ibaAnalyzerExe);
        }

        private void m_datFileTextBox_TextChanged(object sender, EventArgs e)
        {
            m_testButton.Enabled = File.Exists(m_datFileTextBox.Text) &&
                File.Exists(m_data.ParentConfigurationData.IbaAnalyzerExe);
        }

    }
}
