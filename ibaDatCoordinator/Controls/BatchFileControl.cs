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
            m_textEditor.Document.TextContentChanged += new EventHandler(Document_TextContentChanged);
            m_textEditor.Document.DocumentChanged += new ICSharpCode.TextEditor.Document.DocumentEventHandler(Document_DocumentChanged);
            m_tooltip.SetToolTip(m_browseBATCHFileButton, iba.Properties.Resources.tooltipOpenScript);
            m_tooltip.SetToolTip(m_newButton, iba.Properties.Resources.tooltipNewScript);
            m_tooltip.SetToolTip(m_saveButton, iba.Properties.Resources.tooltipSaveScript);
            m_tooltip.SetToolTip(m_executeBatchFile, iba.Properties.Resources.tooltipExecuteScript);
            m_tooltip.SetToolTip(m_browseDatFileButton, iba.Properties.Resources.tooltipBrowseTestFile);
            if(Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                m_lblTestSide.Visible = false;
                m_panelSide.Visible = false;
                int height = this.Size.Height - 30;
                int height2 = m_executeGroupBox.Size.Height - 30;
                this.Size = new Size(this.Size.Width, height);
                this.MinimumSize = new Size(this.Size.Width, height);
                m_executeGroupBox.Size = new Size(m_executeGroupBox.Size.Width, height2);
            }
        }

        void Document_DocumentChanged(object sender, ICSharpCode.TextEditor.Document.DocumentEventArgs e)
        {
            if (!m_bChanged)
            {
                m_bChanged = true;
                m_executeBatchFile.Enabled = true;
                m_saveButton.Enabled = true;
            }
        }

        void Document_TextContentChanged(object sender, EventArgs e)
        {
            if (!m_bChanged)
            {
                m_bChanged = true;
                m_executeBatchFile.Enabled = true;
                m_saveButton.Enabled = true;
            }
        }
    
        void m_textEditor_ActiveTextAreaControlChanged(object sender, EventArgs e)
        {
            if (!m_bChanged)
            {
                m_bChanged = true;
                m_executeBatchFile.Enabled = true;
                m_saveButton.Enabled = true;
            }
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    base.OnLoad(e);
        //    WindowsAPI.SHAutoComplete(m_batchFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
        //    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        //    WindowsAPI.SHAutoComplete(m_datFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
        //    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        //}

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        private BatchFileData m_data;
        
        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            if (Program.RunsWithService != Program.ServiceEnum.CONNECTED || Program.ServiceIsLocal) //will be called multiple times, causes leak in XP
            {
                WindowsAPI.SHAutoComplete(m_batchFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
                    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
                WindowsAPI.SHAutoComplete(m_datFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
                    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            }
            else
            {
                WindowsAPI.SHAutoComplete(m_batchFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
                    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_OFF | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_OFF);
                WindowsAPI.SHAutoComplete(m_datFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
                    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_OFF | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_OFF);
            }
            m_manager = manager;
            m_data = datasource as BatchFileData;
            if (m_data.WhatFile == BatchFileData.WhatFileEnum.DATFILE)
            {
                m_rbDatFile.Checked = true;
                m_rbPrevOutput.Checked = false;
            }
            else
            {
                m_rbDatFile.Checked = false;
                m_rbPrevOutput.Checked = true;
            }
            m_batchFileTextBox.Text = m_data.BatchFile;
            m_argumentsTextBox.Text = m_data.Arguments;
            m_datFileTextBox.Text = m_data.TestDatFile;

            if (DataPath.FileExists(m_batchFileTextBox.Text) && loadBatchFile())
            {
                m_executeBatchFile.Enabled = true;
            }
            else
            {
                if (string.IsNullOrEmpty(m_batchFileTextBox.Text ))
                    m_batchFileTextBox.Text = iba.Properties.Resources.Untitled;
                m_textEditor.Text = "";
                m_executeBatchFile.Enabled = false; 
                m_textEditor.SetHighlighting("VBNET");
                m_textEditor.Invalidate();
            }

			m_nudTime.Value = (int)m_data.TimeOut.TotalMinutes;

			m_bChanged = false;
            m_saveButton.Enabled = false;

            m_rbClientSide.Checked = m_data.TestOnClientSide;
            m_rbServerSide.Checked = !m_data.TestOnClientSide;
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
            m_data.WhatFile = m_rbDatFile.Checked ? BatchFileData.WhatFileEnum.DATFILE : BatchFileData.WhatFileEnum.PREVOUTPUT;
            m_data.TestOnClientSide = m_rbClientSide.Checked;
			try
			{
				m_data.TimeOut = TimeSpan.FromMinutes((int)m_nudTime.Value);
			}
			catch
			{
				m_data.TimeOut = TimeSpan.FromMinutes(15);
			}

			if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        public void LeaveCleanup() { return; }

        #endregion

        private void m_executeBatchFile_Click(object sender, EventArgs e)
        {
            SaveData();
            if (!DataPath.FileExists(m_data.BatchFile)) return;
            string arguments = m_data.ParsedArguments(m_datFileTextBox.Text);
            if (arguments==null)
            {
                MessageBox.Show(iba.Properties.Resources.ScriptArgumentsCouldNotBeParsed, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            using (iba.Dialogs.TestScriptDlg testdlg = new iba.Dialogs.TestScriptDlg(m_data))
            {
                testdlg.StartPosition = FormStartPosition.CenterParent;
                testdlg.ShowDialog(this);
                switch (testdlg.Result)
                {
                    case iba.Dialogs.TestScriptDlg.ScriptResult.TIMEOUT:
                        MessageBox.Show(iba.Properties.Resources.logBatchfileTimeout, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                    case iba.Dialogs.TestScriptDlg.ScriptResult.SUCCESS:
                        MessageBox.Show(iba.Properties.Resources.logBatchfileSuccess, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    case iba.Dialogs.TestScriptDlg.ScriptResult.ERROR:
                        MessageBox.Show(string.Format(iba.Properties.Resources.logBatchfileFailed, testdlg.ExitCode), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        break;
                }
            }
        }

        private void m_browseBATCHFileButton_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Abort;
            String path = m_batchFileTextBox.Text;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
            {
                using (iba.Controls.ServerFolderBrowser fd = new iba.Controls.ServerFolderBrowser(true))
                {
                    fd.FixedDrivesOnly = false;
                    fd.ShowFiles = true;
                    fd.SelectedPath = path;
                    fd.Filter = "Batch files (*.bat)|*.bat|Visual Basic scripts (*.vbs)|*.vbs|Java scripts (*.js)|*.js|All files (*.*)|*.*";
                    result = fd.ShowDialog(this);
                    path = fd.SelectedPath;
                }
            }
            else
            {
                m_openFileDialog1.CheckFileExists = true;
                m_openFileDialog1.FileName = "";
                m_openFileDialog1.Filter = "Batch files (*.bat)|*.bat|Visual Basic scripts (*.vbs)|*.vbs|Java scripts (*.js)|*.js|All files (*.*)|*.*";
                if (System.IO.File.Exists(path))
                    m_openFileDialog1.FileName = path;
                else if (System.IO.Directory.Exists(path))
                    m_openFileDialog1.InitialDirectory = path;
                result = m_openFileDialog1.ShowDialog(this);
                path = m_openFileDialog1.FileName;
            }

            if (result == DialogResult.OK)
            {
                m_batchFileTextBox.Text = path;
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
                    bool exists = DataPath.FileExists(m_batchFileTextBox.Text);
                    if (exists && !DataPath.IsReadOnly(m_batchFileTextBox.Text))
                    {
                        DataPath.WriteFile(filename, m_textEditor.Text);
                        m_saveButton.Enabled = m_bChanged = false;
                        return;
                    }
                    else
                    {
                        MessageBox.Show(exists ? iba.Properties.Resources.ScriptReadOnly : iba.Properties.Resources.ScriptNoExist, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            //do a saveAs
            m_saveFileDialog1.CheckFileExists = false;
            m_saveFileDialog1.FileName = "myscript.bat";
            m_saveFileDialog1.Filter = "Batch files (*.bat)|*.bat|Visual Basic scripts (*.vbs)|*.vbs|Java scripts (*.js)|*.js|All files (*.*)|*.*";
            if (m_saveFileDialog1.ShowDialog(this) == DialogResult.OK)
            {
                filename = m_saveFileDialog1.FileName;
                try
                {
                    FileInfo ifd = new FileInfo(filename);
                    if (ifd.Exists && (ifd.Attributes & FileAttributes.ReadOnly) == 0)
                    {
                        using (StreamWriter bfile = ifd.CreateText())
                        {
                            bfile.Write(m_textEditor.Text);
                        }
                    }
                    else
                    {
                        using (StreamWriter bfile = new StreamWriter(filename))
                        {
                            bfile.Write(m_textEditor.Text);
                        }
                    }
                    ifd = new FileInfo(filename);
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
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        
        private bool m_bChanged = false;
        
        private void m_newButton_Click(object sender, EventArgs e)
        {
            m_textEditor.Text = "";
            m_textEditor.Refresh();
            //m_textEditor.SetHighlighting("VBNET");
            //m_textEditor.Invalidate(); 
            m_batchFileTextBox.Text = iba.Properties.Resources.Untitled;
            m_saveButton.Enabled = m_bChanged = false;
        }

        private bool loadBatchFile()
        {
            try 
            {
                FileInfo ifd = new FileInfo(m_batchFileTextBox.Text);
                string btext = "";
                if (DataPath.FileExists(m_batchFileTextBox.Text))
                {
                    btext = DataPath.ReadFile(m_batchFileTextBox.Text);
                    m_textEditor.Text = btext;
                    m_textEditor.Refresh();

                    if (ifd.Extension == ".vbs" || ifd.Extension == ".VBS")
                        m_textEditor.SetHighlighting("VBNET");
                    else if (ifd.Extension == ".js" || ifd.Extension == ".JS")
                        m_textEditor.SetHighlighting("JavaScript");
                    else
                        m_textEditor.SetHighlighting("Default");
                }
                else
                {
                    MessageBox.Show(iba.Properties.Resources.ScriptNoExist, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            m_executeBatchFile.Enabled = true;
            m_executeGroupBox.Enabled = true;
            return true;
        }

        private void m_browseDatFileButton_Click(object sender, EventArgs e)
        {
            DialogResult result = DialogResult.Abort;
            String path = m_datFileTextBox.Text;
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
            {
                using (iba.Controls.ServerFolderBrowser fd = new iba.Controls.ServerFolderBrowser(true))
                {
                    fd.FixedDrivesOnly = false;
                    fd.ShowFiles = true;
                    fd.SelectedPath = path;
					bool isDat = true;
					if (m_data != null && m_data.ParentConfigurationData != null 
						&&  m_data.ParentConfigurationData.JobType != ConfigurationData.JobTypeEnum.DatTriggered
						&& m_data.ParentConfigurationData.JobType != ConfigurationData.JobTypeEnum.OneTime)
						isDat = false;
					fd.Filter = isDat?Properties.Resources.DatFileFilter: Properties.Resources.HdqFileFilter;
					result = fd.ShowDialog(this);
                    path = fd.SelectedPath;
                }
            }
            else
            {
                m_openFileDialog1.CheckFileExists = true;
                m_openFileDialog1.FileName = "";
				bool isDat = true;
				if (m_data != null && m_data.ParentConfigurationData != null 
					&& m_data.ParentConfigurationData.JobType != ConfigurationData.JobTypeEnum.DatTriggered
					&& m_data.ParentConfigurationData.JobType != ConfigurationData.JobTypeEnum.OneTime)
					isDat = false;
				m_openFileDialog1.Filter = isDat ? Properties.Resources.DatFileFilter : Properties.Resources.HdqFileFilter;
                if (System.IO.File.Exists(path))
                    m_openFileDialog1.FileName = path;
                else if (System.IO.Directory.Exists(path))
                    m_openFileDialog1.InitialDirectory = path;
                result = m_openFileDialog1.ShowDialog(this);
                path = m_openFileDialog1.FileName;
            }
            if (result == DialogResult.OK)
                m_datFileTextBox.Text = path;
        }
    }
}
