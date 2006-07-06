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
    public partial class ExtractControl : UserControl, IPropertyPane
    {
        public ExtractControl()
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
            WindowsAPI.SHAutoComplete(m_pdoFileTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
            WindowsAPI.SHAutoComplete(m_targetFolderTextBox.Handle, SHAutoCompleteFlags.SHACF_FILESYS_DIRS |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        ExtractData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as ExtractData;
            m_nameTextBox.Text = m_data.Name;

            m_pdoFileTextBox.Text = m_data.AnalysisFile;
            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                File.Exists(m_data.ParentConfigurationData.IbaAnalyserExe);
            m_targetFolderTextBox.Text = m_data.DestinationMap;
            m_rbFile.Checked = m_data.ExtractToFile;
            m_rbDbase.Checked = !m_data.ExtractToFile;
            //disable the when groupbox if this is the first task
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

            m_rbNONE.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.NONE;
            m_rbOriginal.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.SAME;
            m_rbHour.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.HOUR;
            m_rbMonth.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.MONTH;
            m_rbDay.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.DAY;
            m_rbWeek.Checked = m_data.Subfolder == ExtractData.SubfolderChoiceB.WEEK;

            m_rbBinaryFile.Checked = m_data.FileType == ExtractData.ExtractFileType.BINARY;
            m_rbTextFile.Checked = m_data.FileType == ExtractData.ExtractFileType.TEXT;
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

            m_data.AnalysisFile = m_pdoFileTextBox.Text;
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            m_data.ExtractToFile = m_rbFile.Checked;
            m_data.FileType = m_rbBinaryFile.Checked?ExtractData.ExtractFileType.BINARY:ExtractData.ExtractFileType.TEXT;

            if (m_rbNONE.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.NONE;
            if (m_rbHour.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.HOUR;
            if (m_rbDay.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.DAY;
            if (m_rbWeek.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.WEEK;
            if (m_rbMonth.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.MONTH;
            if (m_rbOriginal.Checked) m_data.Subfolder = ExtractData.SubfolderChoiceB.SAME;

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }
        #endregion

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_data.ParentConfigurationData.IbaAnalyserExe;
                    ibaProc.StartInfo.Arguments = m_pdoFileTextBox.Text;
                    ibaProc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void m_pdoFileTextBox_TextChanged(object sender, EventArgs e)
        {
            m_executeIBAAButton.Enabled = File.Exists(m_pdoFileTextBox.Text) &&
                File.Exists(m_data.ParentConfigurationData.IbaAnalyserExe);
        }

        private void m_browseFileButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog1.Filter = "ibaAnalyzer PDO files (*.pdo)|*.pdo";
            DialogResult result = m_openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_pdoFileTextBox.Text = m_openFileDialog1.FileName;
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

        private void m_browseFolderButton_Click(object sender, EventArgs e)
        {
            m_folderBrowserDialog1.ShowNewFolderButton = true;
            DialogResult result = m_folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
                m_targetFolderTextBox.Text = m_folderBrowserDialog1.SelectedPath;
        }

        private void m_rbDbase_CheckedChanged(object sender, EventArgs e)
        {
            m_panelFile.Enabled = m_rbFile.Checked;
        }
    }
}
