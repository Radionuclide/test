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

			var b = Bitmap.FromHicon(iba.Properties.Resources.dat_file.Handle);
			m_browseDatFilesButton.Image = new Bitmap(b, new Size(16, 16));

            if(oneTimeJob) //make this a onetime job dialog
            {
                this.SuspendLayout();
                m_startButton.Location = new Point(m_undoChangesBtn.Location.X, 18);
                m_stopButton.Location = new Point(m_refreshDats.Location.X, 18);
                //m_stopButton.Anchor = m_startButton.Anchor = AnchorStyles.Left | AnchorStyles.Top;
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
                m_datDirTextBox.ScrollBars = ScrollBars.Vertical;
                EventHandler eh = new EventHandler(tb_Changed);
                m_datDirTextBox.TextChanged += eh;
                m_datDirTextBox.ClientSizeChanged += eh;  

                foreach(Control c in groupBox1.Controls)
                {
                    if(c == label2)
                        c.Anchor = AnchorStyles.Left | AnchorStyles.Top;
                    else if(c == m_datDirTextBox)
                        c.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
                    else if(c == m_browseFolderButton || c == m_browseDatFilesButton || c == m_checkPathButton)
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
                m_autoStartCheckBox.Visible = false;
                this.ResumeLayout();
                m_toolTip.SetToolTip(m_browseDatFilesButton, iba.Properties.Resources.browseDatFile);
                m_toolTip.SetToolTip(m_browseFolderButton, iba.Properties.Resources.browseFolderDatFile);
            }
            else
            {
                m_browseDatFilesButton.Hide();
                m_checkPathButton.Location = m_browseDatFilesButton.Location;
                m_toolTip.SetToolTip(m_browseFolderButton, iba.Properties.Resources.browseFolderDatFile);
            }

            m_toolTip.SetToolTip(m_refreshDats, iba.Properties.Resources.refreshDatButton);
            m_toolTip.SetToolTip(m_checkPathButton, iba.Properties.Resources.checkPathButton);

            ((Bitmap)m_refreshDats.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_applyToRunningBtn.Image).MakeTransparent(Color.Magenta);
            ((Bitmap)m_undoChangesBtn.Image).MakeTransparent(Color.Magenta);
        }

        private bool busy = false;
        void tb_Changed(object sender, EventArgs e)
        {
            if(busy) return;
            busy = true;
            TextBox tb = sender as TextBox;
            Size tS = TextRenderer.MeasureText(tb.Text, tb.Font);
            bool Hsb = tb.ClientSize.Height < tS.Height + Convert.ToInt32(tb.Font.Size);
            bool Vsb = tb.ClientSize.Width < tS.Width;

            if(Hsb && Vsb)
                tb.ScrollBars = ScrollBars.Both;
            else if(!Hsb && !Vsb)
                tb.ScrollBars = ScrollBars.None;
            else if(Hsb && !Vsb)
                tb.ScrollBars = ScrollBars.Vertical;
            else if(!Hsb && Vsb)
                tb.ScrollBars = ScrollBars.Horizontal;

            sender = tb as object;
            busy = false;
        }  

        private bool m_oneTimeJob;
        IPropertyPaneManager m_manager;
        ConfigurationData m_data;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            if (!m_oneTimeJob)
            {
                if (Program.RunsWithService != Program.ServiceEnum.CONNECTED || Program.ServiceIsLocal)
                    WindowsAPI.SHAutoComplete(m_datDirTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
                    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
                else
                    WindowsAPI.SHAutoComplete(m_datDirTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
                    SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_OFF | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_OFF);
            }
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
            m_cbRepErr.Checked = m_data.ReprocessErrors;
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
            m_tbFilePwd.Text = m_data.FileEncryptionPassword;
        }


        public void SaveData()
        {
            m_data.DatDirectory = m_datDirTextBox.Text;
            m_data.SubDirs = m_subMapsCheckBox.Checked;
            m_data.ReprocessErrors = m_cbRepErr.Checked;
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
            m_data.FileEncryptionPassword = m_tbFilePwd.Text;
        }

        public void LeaveCleanup() {}

        private void OnClickFolderBrowserButton(object sender, EventArgs e)
        {
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
                BrowseFolderRemote();
            else
                BrowseFolderLocal();
        }

        private void m_cbDetectNewFiles_CheckedChanged(object sender, EventArgs e)
        {
            if(!m_cbDetectNewFiles.Checked) m_cbRescanEnabled.Checked = true;
        }

        private void BrowseFolderRemote()
        {
            bool oneTime = m_data.JobType == ConfigurationData.JobTypeEnum.OneTime;
            DialogResult result = DialogResult.Abort;
            string path = "";
            string[] lines = null;
            using (iba.Controls.ServerFolderBrowser fd = new iba.Controls.ServerFolderBrowser(true))
            {
                fd.FixedDrivesOnly = false;
                fd.ShowFiles = false;
                fd.Filter = ".dat files (*.dat)|*.dat";
                if (!oneTime && !String.IsNullOrEmpty(m_datDirTextBox.Text)  && System.IO.Directory.Exists(m_datDirTextBox.Text))
                    fd.SelectedPath = m_datDirTextBox.Text;
                else
                {
                    lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    if ((lines.Length > 0) && (System.IO.File.Exists(lines[lines.Length - 1]) || System.IO.Directory.Exists(lines[lines.Length - 1])))
                        fd.SelectedPath = lines[lines.Length - 1];
                }
                result = fd.ShowDialog(this);
                path = fd.SelectedPath;
            }
            if (result != DialogResult.OK)
                return;
            if (!m_oneTimeJob)
                m_datDirTextBox.Text = path;
            else
            {
                StringBuilder sb = new StringBuilder();
                foreach (string line in lines)
                    sb.AppendLine(line);
                sb.AppendLine();

                string uncline = path;
                try
                {
                    uncline = Shares.PathToUnc(uncline, false);
                }
                catch
                {
                }
                sb.AppendLine(uncline);
                m_datDirTextBox.Text = sb.ToString();
            }
        }

        private void BrowseFolderLocal()
        {
            m_folderBrowserDialog1.ShowNewFolderButton = false;
            if (!m_oneTimeJob)
            {
                m_folderBrowserDialog1.SelectedPath = m_datDirTextBox.Text;
                DialogResult result = m_folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    string uncline = m_folderBrowserDialog1.SelectedPath;
                    try
                    {
                        uncline = Shares.PathToUnc(uncline, false);
                    }
                    catch
                    {

                    }
                    m_datDirTextBox.Text = uncline;
                }
            }
            else
            {
                string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                //         ShowEditBox = true,
                //         //NewStyle = false,
                if ((lines.Length > 0) && (System.IO.File.Exists(lines[lines.Length - 1]) || System.IO.Directory.Exists(lines[lines.Length - 1])))
                    m_folderBrowserDialog1.SelectedPath = lines[lines.Length - 1];
                DialogResult result = m_folderBrowserDialog1.ShowDialog();
                if (result == DialogResult.OK)
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (string line in lines)
                        sb.AppendLine(line);
                    sb.AppendLine();

                    string uncline = m_folderBrowserDialog1.SelectedPath;
                    try
                    {
                        uncline = Shares.PathToUnc(uncline, false);
                    }
                    catch
                    {

                    }
                    sb.AppendLine(uncline);
                    m_datDirTextBox.Text = sb.ToString();
                }
            }
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
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
                BrowseDatFileRemote();
            else
                BrowseDatFileLocal();
        }

        private void BrowseDatFileRemote()
        {
            bool oneTime = m_data.JobType == ConfigurationData.JobTypeEnum.OneTime;
            DialogResult result = DialogResult.Abort;
            string path = "";
            using (iba.Controls.ServerFolderBrowser fd = new iba.Controls.ServerFolderBrowser(true))
            { 
                if (!oneTime && !String.IsNullOrEmpty(m_datDirTextBox.Text))
                    fd.SelectedPath = m_datDirTextBox.Text;
                {
                    fd.FixedDrivesOnly = false;
                    fd.ShowFiles = true;
                    fd.Filter = ".dat files (*.dat)|*.dat";
                    result = fd.ShowDialog(this);
                    path = fd.SelectedPath;
                }
            }
            if (result != DialogResult.OK)
                return;


            if (oneTime)
            {
                string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                foreach (string line in lines)
                {
                    sb.AppendLine(line);
                }
                sb.AppendLine(path);
                m_datDirTextBox.Text = sb.ToString();
            }
            else // DatTriggered, clean and reprocess
            {
                TaskManager.Manager.CleanAndProcessFileNow(m_data, path);
            }
        }

        private void BrowseDatFileLocal()
        {
            m_selectDatFilesDialog.CheckFileExists = true;
            m_selectDatFilesDialog.FileName = "";
            m_selectDatFilesDialog.Filter = ".dat files (*.dat)|*.dat";
            bool oneTime = m_data.JobType == ConfigurationData.JobTypeEnum.OneTime;
            m_selectDatFilesDialog.Multiselect = oneTime;
			if (!oneTime && !String.IsNullOrEmpty(m_datDirTextBox.Text) && Directory.Exists(m_datDirTextBox.Text))
				m_selectDatFilesDialog.InitialDirectory = m_datDirTextBox.Text;

			if (m_selectDatFilesDialog.ShowDialog() == DialogResult.OK)
            {
                if (oneTime)
                {
                    string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    StringBuilder sb = new StringBuilder();
                    foreach (string line in lines)
                    {
                        string uncline = line;
                        try
                        {
                            uncline = Shares.PathToUnc(line, false);
                        }
                        catch
                        {

                        }
                        sb.AppendLine(uncline);
                    }
                    foreach (string line in m_selectDatFilesDialog.FileNames)
                    {
                        string uncline = line;
                        try
                        {
                            uncline = Shares.PathToUnc(line, false);
                        }
                        catch
                        {

                        }
                        sb.AppendLine(uncline);
                    }
                    m_datDirTextBox.Text = sb.ToString();
                }
                else //DatTriggered, clean and reprocess
                {
                    string file = "";
                    try
                    {
                        file = Shares.PathToUnc(m_selectDatFilesDialog.FileName, false);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, iba.Properties.Resources.invalidPath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    if (!string.IsNullOrEmpty(file))
                    {
                        TaskManager.Manager.CleanAndProcessFileNow(m_data, file);
                    }
                }
            }
        }

        private void m_datDirTextBox_Validating(object sender, CancelEventArgs e)
        {   //translate to UNC paths...
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || !Program.ServiceIsLocal) return; //leave it be...
            bool oneTime = m_data.JobType == ConfigurationData.JobTypeEnum.OneTime;
            if (oneTime)
            {
                string[] lines = m_datDirTextBox.Text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                foreach (string line in lines)
                {
                    if (!string.IsNullOrEmpty(line))
                    {
                        try
                        {
                            string newline = Shares.PathToUnc(line, false);
                            sb.AppendLine(newline);
                        }
                        catch
                        {
                            sb.AppendLine(line);
                        }
                    }
                }
                m_datDirTextBox.Text = sb.ToString();
            }
            else
            {
                try
                {
                    m_datDirTextBox.Text = Shares.PathToUnc(m_datDirTextBox.Text, false);
                }
                catch
                {
                }
            }
        }

        private void btnShowPwd_MouseDown(object sender, MouseEventArgs e)
        {
            m_tbPass.UseSystemPasswordChar = false;
        }

        private void btnShowPwd_MouseUp(object sender, MouseEventArgs e)
        {
            m_tbPass.UseSystemPasswordChar = true;
        }

        private void btnShowFilePwd_MouseDown(object sender, MouseEventArgs e)
        {
            m_tbFilePwd.UseSystemPasswordChar = false;
        }

        private void btnShowFilePwd_MouseUp(object sender, MouseEventArgs e)
        {
            m_tbFilePwd.UseSystemPasswordChar = true;
        }

    }
}
