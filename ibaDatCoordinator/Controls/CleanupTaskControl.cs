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

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_targetFolderTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        private void m_browseFolderButton_Click(object sender, EventArgs e)
        {
            m_folderBrowserDialog1.ShowNewFolderButton = true;
            m_folderBrowserDialog1.SelectedPath = m_targetFolderTextBox.Text;
            DialogResult result = m_folderBrowserDialog1.ShowDialog();
            if(result == DialogResult.OK)
                m_targetFolderTextBox.Text = m_folderBrowserDialog1.SelectedPath;
        }

        CleanupTaskData m_data;
        IPropertyPaneManager m_manager;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
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
    }
}
