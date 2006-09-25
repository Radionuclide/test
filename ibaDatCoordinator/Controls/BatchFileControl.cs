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

namespace iba.Controls
{
    public partial class BatchFileControl : UserControl, IPropertyPane
    {
        public BatchFileControl()
        {
            InitializeComponent();
            ((Bitmap)m_refreshButton.Image).MakeTransparent(Color.Magenta);
       
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_batchFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private BatchFileData m_data;
        
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as BatchFileData;

            m_pdoFileTextBox.Text = m_data.AnalysisFile;
            m_batchFileTextBox.Text = m_data.BatchFile;
            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                File.Exists(m_data.ParentConfigurationData.IbaAnalyserExe);
            if (File.Exists(m_batchFileTextBox.Text))
            {
                m_executeBatchFile.Enabled = true;
                loadBatchFile();
            }
            else
            {
                m_executeBatchFile.Enabled = false;
                m_batchFileEditTextBox.Text = "";
                m_editorGroupBox.Enabled = false;
            }
        }

        public void SaveData()
        {
            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.BatchFile = m_batchFileTextBox.Text;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }
        #endregion

        private void m_batchFileTextBox_TextChanged(object sender, EventArgs e)
        {
            if (File.Exists(m_batchFileTextBox.Text))
            {
                loadBatchFile();
            }
            else
            {
                m_executeBatchFile.Enabled = false;
                m_batchFileEditTextBox.Text = "";
                m_editorGroupBox.Enabled = false;
            }
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                File.Exists(m_data.ParentConfigurationData.IbaAnalyserExe);
        }

        private void m_executeBatchFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_batchFileTextBox.Text;
                    ibaProc.StartInfo.Arguments = "dummy.dat " + m_pdoFileTextBox.Text;
                    ibaProc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_data.ParentConfigurationData.IbaAnalyserExe;
                    ibaProc.StartInfo.Arguments = m_pdoFileTextBox.Text;
                    ibaProc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void m_browseBATCHFileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog1.CheckFileExists = false;
            m_openFileDialog1.FileName = "newfile.bat";
            m_openFileDialog1.Filter = "Batch files (*.bat)|*.bat";
            DialogResult result = m_openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                m_batchFileTextBox.Text = m_openFileDialog1.FileName;
                if (!File.Exists(m_batchFileTextBox.Text))
                {
                    try
                    {
                        File.Create(m_batchFileTextBox.Text);
                    }
                    catch
                    {
                        MessageBox.Show("Unable to create batchfile");
                        return;
                    }
                }
                loadBatchFile();
            }
        }

        private void m_browsePDOFileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog1.CheckFileExists = true;
            m_openFileDialog1.Filter = "ibaAnalyzer PDO files (*.pdo)|*.pdo";
            DialogResult result = m_openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_pdoFileTextBox.Text = m_openFileDialog1.FileName;
        }

        private void m_saveButton_Click(object sender, EventArgs e)
        {
            try
            {
                FileInfo ofd = new FileInfo(m_batchFileTextBox.Text);
                if (ofd.Exists && (ofd.Attributes & FileAttributes.ReadOnly)==0)
                {
                    using (StreamWriter bfile = ofd.CreateText())
                    {
                        foreach (string str in m_batchFileEditTextBox.Lines)
                            bfile.WriteLine(str);
                    }
                }
                else 
                    MessageBox.Show("Batchfile does not exist or is readonly");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
        }

        private void m_refreshButton_Click(object sender, EventArgs e)
        {
            loadBatchFile();
        }

        private void loadBatchFile()
        {
            try {
                FileInfo ifd = new FileInfo(m_batchFileTextBox.Text);
                string btext = "";
                if (ifd.Exists)
                {
                    using (StreamReader bfile = ifd.OpenText())
                    {
                        string str;
                        while ((str = bfile.ReadLine()) != null)
                            btext += str + Environment.NewLine;
                    }
                    m_batchFileEditTextBox.Text = btext;
                }
                else
                    MessageBox.Show("Batchfile does not exist");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);    
            }
            m_executeBatchFile.Enabled = true;
            m_editorGroupBox.Enabled = true;
        }
    }
}
