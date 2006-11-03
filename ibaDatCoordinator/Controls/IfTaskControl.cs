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
            m_expressionTextBox.Text = m_data.Expression;
            m_pdoFileTextBox.Text = m_data.AnalysisFile;
            m_datFileTextBox.Text = m_data.TestDatFile;
            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                File.Exists(m_data.ParentConfigurationData.IbaAnalyzerExe) &&
                File.Exists(m_data.ParentConfigurationData.IbaAnalyzerExe);
            m_testButton.Enabled = File.Exists(m_datFileTextBox.Text);
            m_XTypeComboBox.SelectedIndex = (int)m_data.XType;
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.TestDatFile = m_datFileTextBox.Text;
            m_data.Expression = m_expressionTextBox.Text;
            m_data.XType = (IfTaskData.XTypeEnum) m_XTypeComboBox.SelectedIndex;
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
                    ibaProc.StartInfo.FileName = m_data.ParentConfigurationData.IbaAnalyzerExe;
                    ibaProc.StartInfo.Arguments = m_pdoFileTextBox.Text;
                    ibaProc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
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

        private string m_previousRunExecutable;
        private void m_testButton_Click(object sender, EventArgs e)
        {
            IbaAnalyzer.IbaAnalyzer ibaAnalyzer = null;
            //register this
            using (new Utility.WaitCursor())
            {
                if (m_previousRunExecutable != m_data.ParentConfigurationData.IbaAnalyzerExe)
                {
                    try
                    {
                        //version check not necessary, as we're not going to enable this button if the file version is to low
                        Process ibaProc = new Process();
                        ibaProc.EnableRaisingEvents = false;
                        ibaProc.StartInfo.FileName = m_data.ParentConfigurationData.IbaAnalyzerExe;
                        ibaProc.StartInfo.Arguments = "/regserver";
                        ibaProc.Start();
                        ibaProc.WaitForExit(10000);
                        m_previousRunExecutable = m_data.ParentConfigurationData.IbaAnalyzerExe;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                //start the com object
                try
                {
                    ibaAnalyzer = new IbaAnalyzer.IbaAnalysisClass();
                }
                catch (Exception)
                {
                    try //try again by first registering
                    {
                        Process ibaProc = new Process();
                        ibaProc.EnableRaisingEvents = false;
                        ibaProc.StartInfo.FileName = m_data.ParentConfigurationData.IbaAnalyzerExe;
                        ibaProc.StartInfo.Arguments = "/regserver";
                        ibaProc.Start();
                        ibaProc.WaitForExit(10000);

                        ibaAnalyzer = new IbaAnalyzer.IbaAnalysisClass();
                    }
                    catch (Exception ex2)
                    {
                        MessageBox.Show(ex2.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
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
            File.Exists(m_data.ParentConfigurationData.IbaAnalyzerExe);
        }

        private void m_datFileTextBox_TextChanged(object sender, EventArgs e)
        {
            m_testButton.Enabled = File.Exists(m_datFileTextBox.Text) &&
                File.Exists(m_data.ParentConfigurationData.IbaAnalyzerExe);
        }

    }
}
