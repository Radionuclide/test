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
            m_cbRemoveSource.Checked = m_data.RemoveSource;
            m_folderNumber.Value = m_data.SubfoldersNumber;
            m_rbNONE.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.NONE;
            m_rbHour.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.HOUR;
            m_rbMonth.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.MONTH;
            m_rbDay.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.DAY;
            m_rbWeek.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.WEEK;
            m_rbOriginal.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.SAME;
             m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;
            m_rbDatFile.Checked = m_data.WhatFile == CopyMoveTaskData.WhatFileEnumA.DATFILE;
            m_rbOriginal.Checked = m_data.WhatFile == CopyMoveTaskData.WhatFileEnumA.PREVOUTPUT;
        }

        public void SaveData()
        {
            m_data.RemoveSource = m_cbRemoveSource.Checked;
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            if (m_rbNONE.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.NONE;
            if (m_rbHour.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.HOUR;
            if (m_rbDay.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.DAY;
            if (m_rbWeek.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.WEEK;
            if (m_rbMonth.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.MONTH;
            if (m_rbOriginal.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.SAME;

            if (m_rbOriginal.Checked)
                m_data.WhatFile = CopyMoveTaskData.WhatFileEnumA.PREVOUTPUT;
            else if (m_rbDatFile.Checked)
                m_data.WhatFile = CopyMoveTaskData.WhatFileEnumA.DATFILE;

            m_data.SubfoldersNumber = (uint)m_folderNumber.Value;

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
    }
}

