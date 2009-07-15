using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

using iba;
using iba.Data;
using iba.Utility;
using iba.Processing;

namespace iba.Controls
{
    public partial class CopyControl : UserControl, IPropertyPane
    {
        public CopyControl()
        {
            InitializeComponent();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_targetFolderTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        CopyMoveTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as CopyMoveTaskData;
            m_targetFolderTextBox.Text = m_data.DestinationMap;

            if (m_data.ActionDelete)
            {
                m_rbDelete.Checked = true;
                m_rbCopy.Checked = false;
                m_rbMove.Checked = false;
                m_gbTarget.Enabled = false;
            }
            else
            {
                m_gbTarget.Enabled = true;
                m_rbDelete.Checked = false;
                m_rbCopy.Checked = !m_data.RemoveSource;
                m_rbMove.Checked = m_data.RemoveSource;
            }

            m_rbLimitDirectories.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories;
            m_rbQuota.Checked = m_data.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_nudDirs.Value = m_data.SubfoldersNumber;
            m_nudQuota.Value = m_data.Quota;
            
            m_rbNONE.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.NONE;
            m_rbHour.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.HOUR;
            m_rbMonth.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.MONTH;
            m_rbDay.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.DAY;
            m_rbWeek.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.WEEK;
            m_rbOriginal.Checked = m_data.Subfolder == TaskDataUNC.SubfolderChoice.SAME;
            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;
            m_rbDatFile.Checked = m_data.WhatFile == CopyMoveTaskData.WhatFileEnumA.DATFILE;
            m_rbPrevOutput.Checked = m_data.WhatFile == CopyMoveTaskData.WhatFileEnumA.PREVOUTPUT;
            m_cbOverwrite.Checked = m_data.OverwriteFiles;
        }

        public void SaveData()
        {
            m_data.RemoveSource = m_rbMove.Checked || m_rbDelete.Checked;
            m_data.ActionDelete = m_rbDelete.Checked;
            
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            if (m_rbNONE.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.NONE;
            if (m_rbHour.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.HOUR;
            if (m_rbDay.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.DAY;
            if (m_rbWeek.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.WEEK;
            if (m_rbMonth.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.MONTH;
            if (m_rbOriginal.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.SAME;

            if (m_rbPrevOutput.Checked)
                m_data.WhatFile = CopyMoveTaskData.WhatFileEnumA.PREVOUTPUT;
            else if (m_rbDatFile.Checked)
                m_data.WhatFile = CopyMoveTaskData.WhatFileEnumA.DATFILE;

            m_data.SubfoldersNumber = (uint)m_nudDirs.Value;
            m_data.Quota = (uint)m_nudQuota.Value;
            m_data.OutputLimitChoice = m_rbLimitDirectories.Checked ? TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories : TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_data.OverwriteFiles = m_cbOverwrite.Checked;

            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        public void LeaveCleanup() { return; }
        
        #endregion
        
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
                    ok = TaskManager.Manager.TestPath(Shares.PathToUnc(m_targetFolderTextBox.Text,false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true);
                else
                    ok = SharesHandler.TestPath(Shares.PathToUnc(m_targetFolderTextBox.Text,false), m_tbUserName.Text, m_tbPass.Text, out errormessage, true);
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

        private void m_targetDirInfoChanged(object sender, EventArgs e)
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
                    m_nudQuota.Enabled = false;
                    m_nudDirs.Enabled = true;
                }
                else
                {
                    m_rbLimitDirectories.Checked = false;
                    m_nudQuota.Enabled = true;
                    m_nudDirs.Enabled = false;
                }
            }
        }

        private void m_rbDelete_CheckedChanged(object sender, EventArgs e)
        {
            m_gbTarget.Enabled = !m_rbDelete.Checked;
        }

    }
}

