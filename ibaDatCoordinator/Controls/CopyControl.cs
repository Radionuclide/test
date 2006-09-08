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
            this.tableLayoutPanel1.ColumnStyles[0].SizeType = SizeType.Percent;
            this.tableLayoutPanel1.ColumnStyles[0].Width = 33;
            this.tableLayoutPanel1.ColumnStyles[1].SizeType = SizeType.Percent;
            this.tableLayoutPanel1.ColumnStyles[1].Width = 33;
            this.tableLayoutPanel1.ColumnStyles[2].SizeType = SizeType.Percent;
            this.tableLayoutPanel1.ColumnStyles[2].Width = 33;
            this.tableLayoutPanel3.ColumnStyles[0].SizeType = SizeType.Percent;
            this.tableLayoutPanel3.ColumnStyles[0].Width = 33;
            this.tableLayoutPanel3.ColumnStyles[1].SizeType = SizeType.Percent;
            this.tableLayoutPanel3.ColumnStyles[1].Width = 33;
            this.tableLayoutPanel3.ColumnStyles[2].SizeType = SizeType.Percent;
            this.tableLayoutPanel3.ColumnStyles[2].Width = 33;
            this.tableLayoutPanel6.ColumnStyles[0].SizeType = SizeType.Percent;
            this.tableLayoutPanel6.ColumnStyles[0].Width = 33;
            this.tableLayoutPanel6.ColumnStyles[1].SizeType = SizeType.Percent;
            this.tableLayoutPanel6.ColumnStyles[1].Width = 33;
            this.tableLayoutPanel6.ColumnStyles[2].SizeType = SizeType.Percent;
            this.tableLayoutPanel6.ColumnStyles[2].Width = 33;
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
            m_nameTextBox.Text = m_data.Name;
            m_targetFolderTextBox.Text = m_data.DestinationMap;
            m_cbRemoveSource.Checked = m_data.RemoveSource;
            m_folderNumber.Value = m_data.SubfoldersNumber;
            m_rbNONE.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.NONE;
            m_rbHour.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.HOUR;
            m_rbMonth.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.MONTH;
            m_rbDay.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.DAY;
            m_rbWeek.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.WEEK;
            m_rbOriginal.Checked = m_data.Subfolder == CopyMoveTaskData.SubfolderChoiceA.SAME;
            if (m_data.Index == 0)
            {
                m_rbAlways.Checked = true;
                m_rbSucces.Checked = false;
                m_rbFailure.Checked = false;
                m_rb1stFailure.Checked = false;
                m_rbDisabled.Checked = false;
                groupBox4.Enabled = false;
            }
            else
            {
                groupBox4.Enabled = true;
                m_rbAlways.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
                m_rbSucces.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES;
                m_rbFailure.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_FAILURE;
                m_rb1stFailure.Checked = m_data.WhenToExecute == TaskData.WhenToDo.AFTER_1st_FAILURE;
                m_rbDisabled.Checked = m_data.WhenToExecute == TaskData.WhenToDo.DISABLED;
            }
            m_rbNotAlways.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
            m_rbNotSuccess.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES;
            m_rbNotFailure.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_FAILURE;
            m_rbNot1stFailure.Checked = m_data.WhenToNotify == TaskData.WhenToDo.AFTER_1st_FAILURE;
            m_rbNotDisabled.Checked = m_data.WhenToNotify == TaskData.WhenToDo.DISABLED;

            m_checkPathButton.Image = null;
            m_checkPathButton.Text = "?";
            m_tbPass.Text = m_data.Password;
            m_tbUserName.Text = m_data.Username;
        }

        public void SaveData()
        {
            m_data.Name = m_nameTextBox.Text;
            if (m_rbAlways.Checked)
                m_data.WhenToExecute = TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
            else if (m_rbSucces.Checked)
                m_data.WhenToExecute = TaskData.WhenToDo.AFTER_SUCCES;
            else if (m_rbFailure.Checked)
                m_data.WhenToExecute = TaskData.WhenToDo.AFTER_FAILURE;
            else if (m_rb1stFailure.Checked)
                m_data.WhenToExecute = TaskData.WhenToDo.AFTER_1st_FAILURE;
            else
                m_data.WhenToExecute = TaskData.WhenToDo.DISABLED;

            if (m_rbNotAlways.Checked)
                m_data.WhenToNotify = TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
            else if (m_rbNotSuccess.Checked)
                m_data.WhenToNotify = TaskData.WhenToDo.AFTER_SUCCES;
            else if (m_rbNotFailure.Checked)
                m_data.WhenToNotify = TaskData.WhenToDo.AFTER_FAILURE;
            else if (m_rbNot1stFailure.Checked)
                m_data.WhenToNotify = TaskData.WhenToDo.AFTER_1st_FAILURE;
            else
                m_data.WhenToNotify = TaskData.WhenToDo.DISABLED;

            m_data.RemoveSource = m_cbRemoveSource.Checked;
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            if (m_rbNONE.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.NONE;
            if (m_rbHour.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.HOUR;
            if (m_rbDay.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.DAY;
            if (m_rbWeek.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.WEEK;
            if (m_rbMonth.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.MONTH;
            if (m_rbOriginal.Checked) m_data.Subfolder = CopyMoveTaskData.SubfolderChoiceA.SAME;
            m_data.SubfoldersNumber = (uint)m_folderNumber.Value;

            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        
        #endregion
        
        private void m_browseFolderButton_Click(object sender, EventArgs e)
        {
            m_folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult result = m_folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_targetFolderTextBox.Text = m_folderBrowserDialog1.SelectedPath;
        }

        private void m_nameTextBox_TextChanged(object sender, EventArgs e)
        {
            TreeNode node = m_manager.LeftTree.SelectedNode;
            if (node != null)
                node.Text = m_nameTextBox.Text;
            m_manager.AdjustRightPaneControlTitle();
        }
        private void m_whenRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            TreeNode taskNode = m_manager.LeftTree.SelectedNode;
            MainForm.strikeOutNodeText(taskNode, m_rbDisabled.Checked);
            if (m_rb1stFailure.Checked)
                taskNode.StateImageIndex = 2;
            else if (m_rbFailure.Checked)
                taskNode.StateImageIndex = 1;
            else if (m_rbSucces.Checked)
                taskNode.StateImageIndex = 0;
            else
                taskNode.StateImageIndex = -1;
        }

        private void m_checkPathButton_Click(object sender, EventArgs e)
        {
            SaveData();
            string errormessage = null;
            bool ok;
            using (WaitCursor wait = new WaitCursor())
            {
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                    ok = TaskManager.Manager.TestPath(m_targetFolderTextBox.Text, m_tbUserName.Text, m_tbPass.Text, out errormessage, true);
                else
                    ok = SharesHandler.TestPath(m_targetFolderTextBox.Text, m_tbUserName.Text, m_tbPass.Text, out errormessage, true);
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

