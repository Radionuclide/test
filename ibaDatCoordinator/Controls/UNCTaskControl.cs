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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_targetFolderTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        public void SetData(TaskDataUNC data)
        {
            m_data = data;

            m_rbNONE.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.NONE;
            m_rbOriginal.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.SAME;
            m_rbHour.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.HOUR;
            m_rbMonth.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.MONTH;
            m_rbDay.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.DAY;
            m_rbWeek.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.WEEK;

            m_rbLimitDirectories.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories;
            m_rbQuota.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_rbLimitNone.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.None;
            m_rbLimitFree.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.SaveFreeSpace;
            
            m_nudDirs.Value = m_data.SubfoldersNumber;
            m_nudQuota.Value = m_data.Quota;
            m_nudFree.Value = m_data.QuotaFree;
            
            m_cbOverwrite.Checked = m_data.OverwriteFiles;
            m_cbTakeDatTime.Checked = m_data.UseDatModTimeForDirs;

            m_targetFolderTextBox.Text = m_data.DestinationMap;
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;

            m_cbModifyDate.Checked = m_data.CopyModTime;
        }

        public void SaveData()
        {
            if (m_rbNONE.Checked) m_data.Subfolder = ReportData.SubfolderChoice.NONE;
            if (m_rbHour.Checked) m_data.Subfolder = ReportData.SubfolderChoice.HOUR;
            if (m_rbDay.Checked) m_data.Subfolder = ReportData.SubfolderChoice.DAY;
            if (m_rbWeek.Checked) m_data.Subfolder = ReportData.SubfolderChoice.WEEK;
            if (m_rbMonth.Checked) m_data.Subfolder = ReportData.SubfolderChoice.MONTH;
            if (m_rbOriginal.Checked) m_data.Subfolder = ReportData.SubfolderChoice.SAME;

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
            m_data.UseDatModTimeForDirs = m_cbTakeDatTime.Checked;

            m_data.CopyModTime = m_cbModifyDate.Checked;
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();
        }

        private void m_browseFolderButton_Click(object sender, EventArgs e)
        {
            m_folderBrowserDialog1.ShowNewFolderButton = true;
            m_folderBrowserDialog1.SelectedPath = m_targetFolderTextBox.Text;
            DialogResult result = m_folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_targetFolderTextBox.Text = m_folderBrowserDialog1.SelectedPath;
        }

        private void m_checkPathButton_Click(object sender, EventArgs e)
        {
            SaveData();
            string errormessage = null;
            bool ok;
            using (WaitCursor wait = new WaitCursor())
            {
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    ok = TaskManager.Manager.TestPath(Shares.PathToUnc(m_targetFolderTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true);
                else
                    ok = SharesHandler.TestPath(Shares.PathToUnc(m_targetFolderTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true);
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
                    m_nudQuota.Enabled = false;
                    m_nudFree.Enabled = false;
                    m_nudDirs.Enabled = true;
                }
                else if (rb == m_rbQuota)
                {
                    m_rbLimitDirectories.Checked = false;
                    m_rbLimitNone.Checked = false;
                    m_rbLimitFree.Checked = false;
                    m_nudQuota.Enabled = true;
                    m_nudFree.Enabled = false ;
                    m_nudDirs.Enabled = false;
                }
                else if (rb == m_rbLimitFree)
                {
                    m_rbQuota.Checked = false;
                    m_rbLimitDirectories.Checked = false;
                    m_rbLimitNone.Checked = false;
                    m_nudQuota.Enabled = false;
                    m_nudFree.Enabled = true ;
                    m_nudDirs.Enabled = false;
                }
                else
                {
                    m_rbQuota.Checked = false;
                    m_rbLimitDirectories.Checked = false;
                    m_rbLimitFree.Checked = false;
                    m_nudQuota.Enabled = false;
                    m_nudFree.Enabled = false;
                    m_nudDirs.Enabled = false;
                }
            }
        }
    }
}
