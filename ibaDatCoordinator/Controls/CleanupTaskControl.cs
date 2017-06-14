using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using iba.Utility;
using iba.Data;
using iba.Processing;

namespace iba.Controls
{
    public partial class CleanupTaskControl : UserControl, IPropertyPane
    {
        public CleanupTaskControl()
        {
            InitializeComponent();
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
                if ( !String.IsNullOrEmpty(m_targetFolderTextBox.Text))
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
                    uncline = Shares.PathToUnc(uncline, true);
                }
                catch
                {

                }
                m_targetFolderTextBox.Text = uncline;
            }
        }

        CleanupTaskData m_data;
        IPropertyPaneManager m_manager;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
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

            m_manager = manager;
            m_data = datasource as CleanupTaskData;

            m_nudDirs.Value = m_data.SubfoldersNumber;
            m_nudQuota.Value = m_data.Quota;
            m_nudFree.Value = m_data.QuotaFree;

            m_rbLimitDirectories.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories;
            m_rbQuota.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_rbLimitFree.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.SaveFreeSpace;
            if(m_data.OutputLimitChoice == CleanupTaskData.OutputLimitChoiceEnum.None) //someones been manually editing
            {
                m_rbLimitDirectories.Checked = true;
            }
            m_targetFolderTextBox.Text = m_data.DestinationMap;
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;

            m_tbExtension.Text = m_data.Extension;
        }

        public void LeaveCleanup()
        {
        }

        public void SaveData()
        {
            m_data.SubfoldersNumber = (uint)m_nudDirs.Value;
            m_data.Quota = (uint)m_nudQuota.Value;
            m_data.QuotaFree = (uint)m_nudFree.Value;

            if(m_rbLimitDirectories.Checked)
                m_data.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories;
            else if(m_rbQuota.Checked)
                m_data.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            else if(m_rbLimitFree.Checked)
                m_data.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.SaveFreeSpace;

            m_data.DestinationMap = m_targetFolderTextBox.Text;
            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.Extension = m_tbExtension.Text;
            m_data.UpdateUNC();
        }

        private void m_checkPathButton_Click(object sender, EventArgs e)
        {
            SaveData();
            string errormessage = null;
            bool ok;
            using(WaitCursor wait = new WaitCursor())
            {
                if(Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    ok = TaskManager.Manager.TestPath(Shares.PathToUnc(m_targetFolderTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true, true);
                else
                    ok = SharesHandler.TestPath(Shares.PathToUnc(m_targetFolderTextBox.Text, false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true, true);
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

        private void m_nudDirs_ValueChanged(object sender, EventArgs e)
        {
            m_rbLimitDirectories.Checked = true;
            m_rbQuota.Checked = false;
            m_rbLimitFree.Checked = false;
        }

        private void m_nudQuota_ValueChanged(object sender, EventArgs e)
        {
            m_rbLimitDirectories.Checked = false;
            m_rbQuota.Checked = true;
            m_rbLimitFree.Checked = false;
        }

        private void m_nudFree_ValueChanged(object sender, EventArgs e)
        {
            m_rbLimitDirectories.Checked = false;
            m_rbQuota.Checked = false;
            m_rbLimitFree.Checked = true;
        }

        private void m_targetFolderTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || !Program.ServiceIsLocal) return; //leave it be...
            try
            {
                m_targetFolderTextBox.Text = Shares.PathToUnc(m_targetFolderTextBox.Text, true);
            }
            catch
            {
            }          
        }
    }
}
