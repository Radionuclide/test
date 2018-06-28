using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
namespace iba.Controls
{
    using iba.Data;
    using iba.Utility;
    using iba.Processing;

    public partial class UNCTaskControl : UserControl
    {
        TaskDataUNC m_data;
        public UNCTaskControl()
        {
            InitializeComponent();
        }

        public void SetData(TaskDataUNC data)
        {
            m_data = data;
            if (Program.RunsWithService != Program.ServiceEnum.CONNECTED || Program.ServiceIsLocal) //will be called multiple times, causes leak in XP
            {
                WindowsAPI.SHAutoComplete(m_targetFolderTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
                SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            }
            else
            {
                WindowsAPI.SHAutoComplete(m_targetFolderTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
                SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_OFF | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_OFF);
            }

            m_rbNONE.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.NONE;
            m_rbOriginal.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.SAME;
            m_rbHour.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.HOUR;
            m_rbMonth.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.MONTH;
            m_rbDay.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.DAY;
            m_rbWeek.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.WEEK;
            m_rbInfofieldForDir.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.INFOFIELD;

            m_cbSplitSubdirs.Checked = m_data.SplitSubdirs;
            m_cbUse4Numbers.Checked = m_data.Year4Chars;

            m_nudDirs.Value = m_data.SubfoldersNumber;
            m_nudQuota.Value = m_data.Quota;
            m_nudFree.Value = m_data.QuotaFree;

            m_rbLimitDirectories.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories;
            m_rbQuota.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_rbLimitNone.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.None;
            m_rbLimitFree.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.SaveFreeSpace;
            
            m_cbOverwrite.Checked = m_data.OverwriteFiles;
            m_cbTimeDir.SelectedIndex = (int) m_data.DirTimeChoice;

            m_targetFolderTextBox.Text = m_data.DestinationMap;
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;

            m_cbModifyDate.Checked = m_data.CopyModTime;

            m_cbUseInfofieldForName.Checked = m_data.UseInfoFieldForOutputFile;
            m_tbInfoField.Text = m_data.InfoFieldForOutputFile;
            m_nudInfoStart.Value = m_data.InfoFieldForOutputFileStart;
            m_nudInfoLength.Value = m_data.InfoFieldForOutputFileLength;
            m_cbInfoEndBlanks.Checked = m_data.InfoFieldForOutputFileRemoveBlanksEnd;
            m_cbInfoAllBlanks.Checked = m_data.InfoFieldForOutputFileRemoveBlanksAll;

            this.m_tbInfoFieldDir.TextChanged -= new System.EventHandler(this.m_tbInfoFieldDir_TextChanged);
            m_tbInfoFieldDir.Text = m_data.InfoFieldForSubdir;
            this.m_tbInfoFieldDir.TextChanged += new System.EventHandler(this.m_tbInfoFieldDir_TextChanged);
            m_nudInfoStartDir.Value = m_data.InfoFieldForSubdirStart;
            m_nudInfoLengthDir.Value = m_data.InfoFieldForSubdirLength;
            m_cbInfoEndBlanksDir.Checked = m_data.InfoFieldForSubdirRemoveBlanksEnd;
            m_cbInfoAllBlanksDir.Checked = m_data.InfoFieldForSubdirRemoveBlanksAll;
            UpdateTooltips();
        }

        private void UpdateTooltips()
        {
            m_toolTip.SetToolTip(m_rbHour, String.Format(iba.Properties.Resources.ExampleFolder, System.IO.Path.Combine(m_targetFolderTextBox.Text, TaskDataUNC.GetSubDir(TaskDataUNC.SubfolderChoice.HOUR, DateTime.Now, m_cbUse4Numbers.Checked, m_cbSplitSubdirs.Checked))));
            m_toolTip.SetToolTip(m_rbDay, String.Format(iba.Properties.Resources.ExampleFolder, System.IO.Path.Combine(m_targetFolderTextBox.Text, TaskDataUNC.GetSubDir(TaskDataUNC.SubfolderChoice.DAY, DateTime.Now, m_cbUse4Numbers.Checked, m_cbSplitSubdirs.Checked))));
            m_toolTip.SetToolTip(m_rbWeek, String.Format(iba.Properties.Resources.ExampleFolder, System.IO.Path.Combine(m_targetFolderTextBox.Text, TaskDataUNC.GetSubDir(TaskDataUNC.SubfolderChoice.WEEK, DateTime.Now, m_cbUse4Numbers.Checked, m_cbSplitSubdirs.Checked))));
            m_toolTip.SetToolTip(m_rbMonth, String.Format(iba.Properties.Resources.ExampleFolder, System.IO.Path.Combine(m_targetFolderTextBox.Text, TaskDataUNC.GetSubDir(TaskDataUNC.SubfolderChoice.MONTH, DateTime.Now, m_cbUse4Numbers.Checked, m_cbSplitSubdirs.Checked))));
        }

        public void SaveData()
        {
            if(m_rbNONE.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.NONE;
            if(m_rbHour.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.HOUR;
            if(m_rbDay.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.DAY;
            if(m_rbWeek.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.WEEK;
            if(m_rbMonth.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.MONTH;
            if(m_rbOriginal.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.SAME;
            if(m_rbInfofieldForDir.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.INFOFIELD;
            m_data.SplitSubdirs = m_cbSplitSubdirs.Checked;
            m_data.Year4Chars = m_cbUse4Numbers.Checked;


            m_data.SubfoldersNumber = (uint)m_nudDirs.Value;
            m_data.Quota = (uint) m_nudQuota.Value;
            m_data.QuotaFree = (uint) m_nudFree.Value;

            if (m_rbLimitDirectories.Checked)
                m_data.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories;
            else if (m_rbQuota.Checked)
                m_data.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            else if (m_rbLimitNone.Checked)
                m_data.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.None;
            else if (m_rbLimitFree.Checked)
                m_data.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.SaveFreeSpace;

            m_data.OverwriteFiles = m_cbOverwrite.Checked;
            m_data.DirTimeChoice = (TaskDataUNC.DirTimeChoiceEnum) m_cbTimeDir.SelectedIndex;

            m_data.CopyModTime = m_cbModifyDate.Checked;
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;

            m_data.UseInfoFieldForOutputFile = m_cbUseInfofieldForName.Checked;
            m_data.InfoFieldForOutputFile = m_tbInfoField.Text;
            m_data.InfoFieldForOutputFileStart = (int) m_nudInfoStart.Value;
            m_data.InfoFieldForOutputFileLength = (int) m_nudInfoLength.Value;
            m_data.InfoFieldForOutputFileRemoveBlanksEnd = m_cbInfoEndBlanks.Checked;
            m_data.InfoFieldForOutputFileRemoveBlanksAll = m_cbInfoAllBlanks.Checked;

            
            m_data.InfoFieldForSubdir = m_tbInfoFieldDir.Text;
            m_data.InfoFieldForSubdirStart = (int) m_nudInfoStartDir.Value;
            m_data.InfoFieldForSubdirLength = (int) m_nudInfoLengthDir.Value;
            m_data.InfoFieldForSubdirRemoveBlanksEnd = m_cbInfoEndBlanksDir.Checked;
            m_data.InfoFieldForSubdirRemoveBlanksAll = m_cbInfoAllBlanksDir.Checked;
            m_data.UpdateUNC();
        }

        private void m_browseFolderButton_Click(object sender, EventArgs e)
        {
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
                BrowseFolderRemote();
            else
                BrowseFolderLocal();
        }


        private void BrowseFolderRemote()
        {
            DialogResult result = DialogResult.Abort;
            string path = "";
            using (iba.Controls.ServerFolderBrowser fd = new iba.Controls.ServerFolderBrowser(true))
            {
                fd.FixedDrivesOnly = false;
                fd.ShowFiles = false;
                fd.Filter = ".dat files (*.dat)|*.dat";
                if (!String.IsNullOrEmpty(m_targetFolderTextBox.Text))
                    fd.SelectedPath = m_targetFolderTextBox.Text;
                result = fd.ShowDialog(this);
                path = fd.SelectedPath;
            }
            if (result != DialogResult.OK)
                return;
            m_targetFolderTextBox.Text = path;
        }

        private void BrowseFolderLocal()
        {
            m_folderBrowserDialog1.ShowNewFolderButton = false;
            m_folderBrowserDialog1.SelectedPath = m_targetFolderTextBox.Text;
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
                m_targetFolderTextBox.Text = uncline;
            }
        }

        private void m_checkPathButton_Click(object sender, EventArgs e)
        {
            SaveData();
            string errormessage = null;
            bool ok;
            using (WaitCursor wait = new WaitCursor())
            {
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    ok = TaskManager.Manager.TestPath(Shares.PathToUnc(m_targetFolderTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true,true);
                else
                    ok = SharesHandler.TestPath(Shares.PathToUnc(m_targetFolderTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true,true);
            }
            if (ok)
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

        private void TargetDirInfoChanged(object sender, EventArgs e)
        {
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";

            UpdateTooltips();
        }

        private void m_rbLimitUsageChoiceChanged(object sender, EventArgs e)
        {
            RadioButton rb = sender as RadioButton;
            if (rb.Checked)
            {
                if (rb == m_rbLimitDirectories)
                {
                    m_rbQuota.Checked = false;
                    m_rbLimitNone.Checked = false;
                    m_rbLimitFree.Checked = false;
                }
                else if (rb == m_rbQuota)
                {
                    m_rbLimitDirectories.Checked = false;
                    m_rbLimitNone.Checked = false;
                    m_rbLimitFree.Checked = false;
                }
                else if (rb == m_rbLimitFree)
                {
                    m_rbQuota.Checked = false;
                    m_rbLimitDirectories.Checked = false;
                    m_rbLimitNone.Checked = false;
                }
                else
                {
                    m_rbQuota.Checked = false;
                    m_rbLimitDirectories.Checked = false;
                    m_rbLimitFree.Checked = false;
                }
            }
        }

        private void m_tbInfoField_TextChanged(object sender, EventArgs e)
        {
            m_cbUseInfofieldForName.Checked = true;
        }

        public void HideInfofieldFileNameOptions()
        {
            m_cbUseInfofieldForName.Visible = false;
            m_tbInfoField.Visible = false;
            m_nudInfoLength.Visible = false;
            m_nudInfoStart.Visible = false;
            m_lblInfoLength.Visible = false;
            m_lblInfoStart.Visible = false;
            m_cbInfoAllBlanks.Visible = false;
            m_cbInfoEndBlanks.Visible = false;
        }

        private void TimeDirRbCheckedChanged(object sender, EventArgs e)
        {
            if ((sender as RadioButton).Checked && m_rbInfofieldForDir.Checked)
                m_rbInfofieldForDir.Checked = false;
        }

        private void m_rbInfofieldForDir_CheckedChanged(object sender, EventArgs e)
        {
            if (m_rbInfofieldForDir.Checked)
            {
                RadioButton[] buttons = { m_rbNONE, m_rbHour, m_rbDay, m_rbWeek, m_rbMonth, m_rbOriginal };
                foreach(RadioButton rb in buttons)
                {
                    if (rb.Checked)
                    {
                        rb.Checked = false;
                        break;
                    }
                }
            }
        }

        private void m_tbInfoFieldDir_TextChanged(object sender, EventArgs e)
        {
            m_rbInfofieldForDir.Checked = true;
        }

        private void flowLayoutPanel1_Resize(object sender, EventArgs e)
        {
            Control control = (Control)sender;
            panel1.Size = new Size(control.Width-2, panel1.Height);
            int MaxChecks = Math.Max(m_cbInfoAllBlanks.Width, m_cbInfoEndBlanks.Width);
            int widthAvailable = panel1.Right - m_tbInfoField.Right - MaxChecks - 10;
            m_tbInfoField.Size = new Size(m_tbInfoField.Width + widthAvailable, m_tbInfoField.Height);
            m_cbInfoEndBlanks.Left = m_cbInfoAllBlanks.Left = m_tbInfoField.Right + 5;
        }

        private void panel2_Resize(object sender, EventArgs e)
        {
            int MaxChecks = Math.Max(m_cbInfoAllBlanksDir.Width, m_cbInfoEndBlanksDir.Width);
            int widthAvailable = panel2.Right - m_tbInfoFieldDir.Right - MaxChecks - 10;
            m_tbInfoFieldDir.Size = new Size(m_tbInfoFieldDir.Width + widthAvailable, m_tbInfoFieldDir.Height);
            m_cbInfoEndBlanksDir.Left = m_cbInfoAllBlanksDir.Left = m_tbInfoFieldDir.Right + 5;
        }

        private void m_cbSplitSubdirs_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTooltips();
        }

        private void m_cbUse4Numbers_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTooltips();
        }

        private void m_nudQuota_ValueChanged(object sender, EventArgs e)
        {
            m_rbQuota.Checked = true;
            m_rbLimitNone.Checked = false;
            m_rbLimitFree.Checked = false;
            m_rbLimitDirectories.Checked = false;
        }

        private void m_nudDirs_ValueChanged(object sender, EventArgs e)
        {
            m_rbQuota.Checked = false;
            m_rbLimitNone.Checked = false;
            m_rbLimitFree.Checked = false;
            m_rbLimitDirectories.Checked = true;
        }

        private void m_nudFree_ValueChanged(object sender, EventArgs e)
        {
            m_rbQuota.Checked = false;
            m_rbLimitNone.Checked = false;
            m_rbLimitFree.Checked = true;
            m_rbLimitDirectories.Checked = false;
        }
    }
}
