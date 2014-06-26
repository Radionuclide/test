using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using iba.Data;
using iba.Utility;
using iba.Processing;
using iba.Dialogs;
using System.IO;

namespace iba.Controls
{
    public partial class PanelDatFilesJob : UserControl, IPropertyPane
    {
        public PanelDatFilesJob(bool oneTimeJob)
        {
            InitializeComponent();
            m_oneTimeJob = oneTimeJob;

            if(oneTimeJob) //make this a onetime job dialog
            {
                this.SuspendLayout();
                m_startButton.Location = new Point(m_undoChangesBtn.Location.X, 18);
                m_stopButton.Location = new Point(m_refreshDats.Location.X, 18);
                //m_stopButton.Anchor = m_startButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                m_browseDatFilesButton.Image = Bitmap.FromHicon(iba.Properties.Resources.standalone.Handle);
                foreach(Control c in groupBox3.Controls)
                {
                    if(c != m_startButton && c != m_stopButton && c != m_enableCheckBox)
                    {
                        c.Enabled = false;
                        c.Visible = false;
                    }
                }
                int groupbox3OldHeight = groupBox3.Size.Height;
                int groupbox3NewHeight = m_stopButton.Size.Height + 16 + 8;
                int diff = groupbox3OldHeight - groupbox3NewHeight;
                groupBox3.Size = new Size(groupBox3.Size.Width, groupbox3NewHeight);

                m_datDirTextBox.Multiline = true;
                foreach(Control c in groupBox1.Controls)
                {
                    if(c == label2)
                        c.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    else if(c == m_datDirTextBox)
                        c.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                    else if(c == m_browseFolderButton || c == m_browseDatFilesButton)
                        c.Anchor = AnchorStyles.Right | AnchorStyles.Top; //actually this is unchanged
                    else
                        c.Anchor = (c.Anchor & ~AnchorStyles.Top) | AnchorStyles.Bottom;
                }

                groupBox1.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
                groupBox1.Location = new Point(groupBox1.Location.X, groupBox1.Location.Y - diff);
                int newHeight = groupBox1.Size.Height + diff;
                groupBox1.Size = new Size(groupBox1.Size.Width, newHeight);
                groupBox1.MinimumSize = new Size(0,newHeight);
                m_toolTip.SetToolTip(m_datDirTextBox, iba.Properties.Resources.DatDirDragAndDrop);

                this.ResumeLayout();
            }
            else
            {
                m_browseDatFilesButton.Visible = false;
                int diff = m_refreshDats.Location.X - m_undoChangesBtn.Location.X;
                m_browseFolderButton.Location = new Point(m_browseFolderButton.Location.X + diff, m_browseFolderButton.Location.Y);
                m_datDirTextBox.Size = new Size(m_datDirTextBox.Size.Width + diff, m_datDirTextBox.Size.Height);
            }


            m_toolTip.SetToolTip(m_refreshDats, iba.Properties.Resources.refreshDatButton);
            m_toolTip.SetToolTip(m_checkPathButton, iba.Properties.Resources.checkPathButton);

            ((Bitmap)m_refreshDats.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_applyToRunningBtn.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_undoChangesBtn.Image).MakeTransparent(Color.Magenta);
        }

        private bool m_oneTimeJob;
        IPropertyPaneManager m_manager;
        ConfigurationData m_data;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if(!m_oneTimeJob)
                WindowsAPI.SHAutoComplete(m_datDirTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
                    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as ConfigurationData;
            m_datDirTextBox.Text = m_data.DatDirectory;
            m_subMapsCheckBox.Checked = m_data.SubDirs;


            if(m_failTimeUpDown.Minimum > (decimal)m_data.ReprocessErrorsTimeInterval.TotalMinutes)
                m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double)m_failTimeUpDown.Minimum);
            else if(m_failTimeUpDown.Maximum < (decimal)m_data.ReprocessErrorsTimeInterval.TotalMinutes)
                m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double)m_failTimeUpDown.Maximum);

            if(m_scanTimeUpDown.Minimum > (decimal)m_data.RescanTimeInterval.TotalMinutes)
                m_data.RescanTimeInterval = TimeSpan.FromMinutes((double)m_scanTimeUpDown.Minimum);
            else if(m_scanTimeUpDown.Maximum < (decimal)m_data.RescanTimeInterval.TotalMinutes)
                m_data.RescanTimeInterval = TimeSpan.FromMinutes((double)m_scanTimeUpDown.Maximum);

            m_scanTimeUpDown.Value = (decimal)m_data.RescanTimeInterval.TotalMinutes;
            m_scanTimeUpDown.Enabled = m_cbRescanEnabled.Checked = m_data.RescanEnabled;
            m_cbInitialScanEnabled.Checked = m_data.InitialScanEnabled;
            m_failTimeUpDown.Value = (decimal)m_data.ReprocessErrorsTimeInterval.TotalMinutes;
            m_retryUpDown.Value = (decimal)m_data.NrTryTimes;
            m_retryUpDown.Enabled = m_cbRetry.Checked = m_data.LimitTimesTried;

            if(Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                m_refreshDats.Enabled = true;
            }
            else if(TaskManager.Manager.IsJobStarted(m_data.Guid))
            {
                m_refreshDats.Enabled = false;
            }
            else
            {
                m_refreshDats.Enabled = true;
            }

            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;
            m_cbDetectNewFiles.Checked = m_data.DetectNewFiles;
        }


        public void SaveData()
        {
            m_data.DatDirectory = m_datDirTextBox.Text;
            m_data.SubDirs = m_subMapsCheckBox.Checked;
            m_data.ReprocessErrorsTimeInterval = TimeSpan.FromMinutes((double)m_failTimeUpDown.Value);
            m_data.RescanTimeInterval = TimeSpan.FromMinutes((double)m_scanTimeUpDown.Value);
            m_data.RescanEnabled = m_cbRescanEnabled.Checked;
            m_data.InitialScanEnabled = m_cbInitialScanEnabled.Checked;
            m_data.NrTryTimes = (int)m_retryUpDown.Value;
            m_data.LimitTimesTried = m_cbRetry.Checked;

            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();
            m_data.DetectNewFiles = m_cbDetectNewFiles.Checked;
        }

        public void LeaveCleanup() {}

        private void OnClickFolderBrowserButton(object sender, EventArgs e)
        {
            m_folderBrowserDialog1.ShowNewFolderButton = false;
            if(!m_oneTimeJob)
            {
                m_folderBrowserDialog1.SelectedPath = m_datDirTextBox.Text;
                DialogResult result = m_folderBrowserDialog1.ShowDialog();
                if(result == DialogResult.OK)
                    m_datDirTextBox.Text = m_folderBrowserDialog1.SelectedPath;
            }
            else
            {
                string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                //         ShowEditBox = true,
                //         //NewStyle = false,
                if((lines.Length > 0) && (System.IO.File.Exists(lines[lines.Length - 1]) || System.IO.Directory.Exists(lines[lines.Length - 1])))
                    m_folderBrowserDialog1.SelectedPath = lines[lines.Length - 1];
                DialogResult result = m_folderBrowserDialog1.ShowDialog();
                if(result == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach(string line in lines)
                        sb.AppendLine(line);
                    sb.AppendLine(m_folderBrowserDialog1.SelectedPath);
                    m_datDirTextBox.Text = sb.ToString();
                }
            }
        }

        private void m_cbDetectNewFiles_CheckedChanged(object sender, EventArgs e)
        {
            if(!m_cbDetectNewFiles.Checked) m_cbRescanEnabled.Checked = true;
        }

        private void m_refreshDats_Click(object sender, EventArgs e)
        {
            MainForm t = m_manager as MainForm;
            if(!Utility.Crypt.CheckPassword(t)) return;
            DialogResult res = MessageBox.Show(this, iba.Properties.Resources.refreshDatWarning,
            iba.Properties.Resources.refreshDatButton, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            if(res != DialogResult.Yes)
                return;

            SaveData();
            using(RemoveMarkingsDialog dialog = new RemoveMarkingsDialog(m_data))
            {
                dialog.ShowDialog(ParentForm);
            }
        }

        private void m_cbRescanEnabled_CheckedChanged(object sender, EventArgs e)
        {
            m_scanTimeUpDown.Enabled = m_cbRescanEnabled.Checked;
            if(!m_cbRescanEnabled.Checked) m_cbDetectNewFiles.Checked = true;
        }


        private void m_cbRetry_CheckedChanged(object sender, EventArgs e)
        {
            m_retryUpDown.Enabled = m_cbRetry.Checked;
        }

        private void m_startButton_Click(object sender, EventArgs e)
        {
            m_refreshDats.Enabled = false;
        }

        private void m_stopButton_Click(object sender, EventArgs e)
        {
            m_refreshDats.Enabled = true;
        }

        private void m_checkPathButton_Click(object sender, EventArgs e)
        {
            SaveData();
            string errormessage = null;
            bool ok = true;
            using(WaitCursor wait = new WaitCursor())
            {
                if(!m_data.OnetimeJob)
                {
                    if(Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                        ok = TaskManager.Manager.TestPath(Shares.PathToUnc(m_datDirTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, false, false);
                    else
                        ok = SharesHandler.TestPath(Shares.PathToUnc(m_datDirTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, false, false);
                }
                else
                {
                    string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach(string line in lines)
                    {
                        if(Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                            ok = TaskManager.Manager.TestPath(Shares.PathToUnc(line, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, false, false);
                        else
                            ok = SharesHandler.TestPath(Shares.PathToUnc(line, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, false, false);
                        if(!ok)
                        {
                            errormessage = "\"" + line + "\": " + errormessage;
                            break;
                        }
                    }
                }
            }
            if(ok)
            {
                m_checkPathButton.Text = null;
                m_checkPathButton.Image = iba.Properties.Resources.thumup;
            }
            else
            {
                MessageBox.Show(errormessage, iba.Properties.Resources.invalidPath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_checkPathButton.Text = null;
                m_checkPathButton.Image = iba.Properties.Resources.thumbdown;
            }
            ((Bitmap)m_checkPathButton.Image).MakeTransparent(Color.Magenta);
        }

        private void m_datDirInfoChanged(object sender, EventArgs e)
        {
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
        }

        private void m_datDirTextBox_DragOver(object sender, DragEventArgs e)
        {
            if(e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = e.AllowedEffect;
            else
                e.Effect = DragDropEffects.None;
        }

        private void m_datDirTextBox_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = e.Data.GetData(DataFormats.FileDrop) as string[];
            if(files != null && files.Length > 0 && !m_oneTimeJob && Directory.Exists(files[0]))
            {
                m_datDirTextBox.Text = files[0];
            }
            else if(files != null)
            {
                string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                foreach(string line in lines)
                    sb.AppendLine(line);
                foreach(string file in files)
                {
                    if(Directory.Exists(file) || (File.Exists(file) && (new FileInfo(file)).Extension == ".dat"))
                        sb.AppendLine(file);
                }
                m_datDirTextBox.Text = sb.ToString();
            }
        }

        private void m_browseDatFilesButton_Click(object sender, EventArgs e)
        {
            m_selectDatFilesDialog.CheckFileExists = true;
            m_selectDatFilesDialog.FileName = "";
            m_selectDatFilesDialog.Filter = ".dat files (*.dat)|*.dat";
            if(m_selectDatFilesDialog.ShowDialog() == DialogResult.OK)
            {
                string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                foreach(string line in lines)
                    sb.AppendLine(line);
                foreach(string line in m_selectDatFilesDialog.FileNames)
                    sb.AppendLine(line);
                m_datDirTextBox.Text = sb.ToString();
            }
        }

        public iba.Utility.CollapsibleElementSubManager CreateGroupBoxManager()
        {
            groupBox3.Init();
            groupBox1.Init();
            CollapsibleElementSubManager manager = new CollapsibleElementSubManager(this);
            manager.AddElement(groupBox3);
            manager.AddElement(groupBox1);
            return manager;
        }
    }
}
