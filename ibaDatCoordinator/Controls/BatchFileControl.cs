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
            ((Bitmap)m_newButton.Image).MakeTransparent(Color.Magenta);
       
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_batchFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_datFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private BatchFileData m_data;
        
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as BatchFileData;

            m_batchFileTextBox.Text = m_data.BatchFile;
            if (File.Exists(m_batchFileTextBox.Text) && loadBatchFile())
            {
                m_executeBatchFile.Enabled = true;
            }
            else
            {
                m_executeBatchFile.Enabled = false;
                m_batchFileTextBox.Text = iba.Properties.Resources.Untitled;
                m_textEditor.Text = "";
                m_textEditor.SetHighlighting("Default");
            }
            m_bChanged = false;
            m_saveButton.Enabled = false;
        }

        public void SaveData()
        {
            if (m_bChanged)
            {
                DialogResult res = MessageBox.Show(this, iba.Properties.Resources.AskToSaveScript,
                    iba.Properties.Resources.batchfileTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                if (res == DialogResult.Yes)
                    m_saveButton_Click(null, null);
            }
            m_data.BatchFile = m_batchFileTextBox.Text;
            m_data.Arguments = m_argumentsTextBox.Text;
            m_data.TestDatFile = m_datFileTextBox.Text;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        public void LeaveCleanup() { return; }

        #endregion

        private void m_executeBatchFile_Click(object sender, EventArgs e)
        {
            SaveData();
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_batchFileTextBox.Text;
                    ibaProc.StartInfo.Arguments = m_data.ParsedArguments(m_datFileTextBox.Text);
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
            m_openFileDialog1.CheckFileExists = true;
            m_openFileDialog1.FileName = "";
            m_openFileDialog1.Filter = "Batch files (*.bat)|*.bat|Visual Basic scripts (*.vbs)|*.vbs|Java scripts (*.js)|*.js|All files (*.*)|*.*";
            DialogResult result = m_openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                m_batchFileTextBox.Text = m_openFileDialog1.FileName;
                if (loadBatchFile())
                {
                    m_executeBatchFile.Enabled = true;
                }
                else
                {
                    m_executeBatchFile.Enabled = false;
                    m_batchFileTextBox.Text = iba.Properties.Resources.Untitled;
                    m_textEditor.Text = "";
                }
                m_bChanged = m_saveButton.Enabled = false;
            }
        }

        private void m_saveButton_Click(object sender, EventArgs e)
        {
            string filename = null;
            if (!String.IsNullOrEmpty(m_batchFileTextBox.Text) && m_batchFileTextBox.Text != iba.Properties.Resources.Untitled)
            {
                filename = m_batchFileTextBox.Text;
                try
                {
                    FileInfo ofd = new FileInfo(filename);
                    if (ofd.Exists && (ofd.Attributes & FileAttributes.ReadOnly) == 0)
                    {
                        using (StreamWriter bfile = ofd.CreateText())
                        {
                            bfile.Write(m_textEditor.Text);
                        }
                        m_saveButton.Enabled = m_bChanged = false;
                        return;
                    }
                    else
                    {
                        MessageBox.Show(ofd.Exists ? iba.Properties.Resources.ScriptReadOnly : iba.Properties.Resources.ScriptNoExist);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            //do a saveAs
            m_saveFileDialog1.CheckFileExists = true;
            m_saveFileDialog1.FileName = "myscript.bat";
            m_saveFileDialog1.Filter = "Batch files (*.bat)|*.bat|Visual Basic scripts (*.vbs)|*.vbs|Java scripts (*.js)|*.js|All files (*.*)|*.*";
            if (m_saveFileDialog1.ShowDialog())
            {
                filename = m_saveFileDialog1.FileName;
                try
                {
                    FileInfo ifd = new FileInfo(filename);
                    if (ifd.Exists && (ifd.Attributes & FileAttributes.ReadOnly) == 0)
                    {
                        using (StreamWriter bfile = ofd.CreateText())
                        {
                            bfile.Write(m_textEditor.Text);
                        }
                        if (ifd.Extension == ".vbs" || ifd.Extension == ".VBS")
                            m_textEditor.SetHighlighting("VBNET");
                        else if (ifd.Extension == ".js" || ifd.Extension == ".JS")
                            m_textEditor.SetHighlighting("JavaScript");
                        //else if (ifd.Extension == ".cs")
                        //    m_textEditor.SetHighlighting("C#");
                        else
                            m_textEditor.SetHighlighting("Default");
                        //m_textEditor.Text = btext;
                        m_textEditor.Invalidate();
                        m_saveButton.Enabled = m_bChanged = false;
                        m_batchFileTextBox.Text = filename;
                        return;
                    }
                    else
                    {
                        MessageBox.Show(ifd.Exists ? iba.Properties.Resources.ScriptReadOnly : iba.Properties.Resources.ScriptNoExist);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        
        private bool m_bChanged = false;
        
        private void m_newButton_Click(object sender, EventArgs e)
        {
            m_textEditor.Text = "";
            m_textEditor.SetHighlighting("Default");
            m_batchFileTextBox.Text = iba.Properties.Resources.Untitled;
            m_saveButton.Enabled = m_bChanged = false;
        }

        private void loadBatchFile()
        {
            try 
            {
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
                    m_textEditor.Text = btext;
                    //if (ifd.Extension == ".bat" || ifd.Extension == ".BAT")
                    //    m_textEditor.SetHighlighting("Default");
                    if (ifd.Extension == ".vbs" || ifd.Extension == ".VBS")
                        m_textEditor.SetHighlighting("VBNET");
                    else if (ifd.Extension == ".js" || ifd.Extension == ".JS")
                        m_textEditor.SetHighlighting("JavaScript");
                    //else if (ifd.Extension == ".cs")
                    //    m_textEditor.SetHighlighting("C#");
                    else
                        m_textEditor.SetHighlighting("Default");
                    //m_textEditor.Text = btext;
                    m_textEditor.Invalidate();
                }
                else
                {
                    MessageBox.Show(iba.Properties.Resources.ScriptNoExist);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            m_executeBatchFile.Enabled = true;
            m_editorGroupBox.Enabled = true;
            return true;
        }

        private void m_browseDatFileButton_Click(object sender, EventArgs e)
        {

        }

        private void m_textEditor_Changed(object sender, EventArgs e)
        {
            if (!m_bChanged)
            {
                m_bChanged = true;
                m_executeBatchFile.Enabled = true;
                m_saveButton.Enabled = true;
            }
        }
    }
}
