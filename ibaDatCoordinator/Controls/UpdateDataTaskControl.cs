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
    public partial class UpdateDataTaskControl : UserControl, IPropertyPane
    {
        public UpdateDataTaskControl()
        {
            InitializeComponent();
        }

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        UpdateDataTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as UpdateDataTaskData;
            
            //output file stuff
            m_targetFolderTextBox.Text = m_data.DestinationMap;
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
            m_cbOverwrite.Checked = m_data.OverwriteFiles;
            m_cbTakeDatTime.Checked = m_data.UseDatModTimeForDirs;

            //database stuff
            m_tbDatabaseName.Text = m_data.DbName;
            m_tbTableName.Text = m_data.DbTblName;
            m_tbDbUsername.Text = m_data.DbUserName;
            m_tbDbPass.Text = m_data.DbPassword;
            m_tbServer.Text = m_data.DbServer;
            m_rbServer.Checked = m_data.DbNamedServer;
            m_rbLocal.Checked = !m_data.DbNamedServer;
            m_cbProvider.SelectedIndex = (int)m_data.DbProvider;
            m_rbNT.Checked = m_data.DbAuthenticateNT;
            m_rbOtherAuth.Checked = !m_data.DbAuthenticateNT;
            m_cbProvider_SelectedIndexChanged(null, null);
        }

        public void LeaveCleanup()
        {
            return; 
        }

        public void SaveData()
        {
            m_data.DestinationMap = m_targetFolderTextBox.Text;
            if (m_rbNONE.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.NONE;
            if (m_rbHour.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.HOUR;
            if (m_rbDay.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.DAY;
            if (m_rbWeek.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.WEEK;
            if (m_rbMonth.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.MONTH;
            if (m_rbOriginal.Checked) m_data.Subfolder = TaskDataUNC.SubfolderChoice.SAME;
            m_data.SubfoldersNumber = (uint)m_nudDirs.Value;
            m_data.Quota = (uint)m_nudQuota.Value;
            m_data.OutputLimitChoice = m_rbLimitDirectories.Checked ? TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories : TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            m_data.OverwriteFiles = m_cbOverwrite.Checked;
            m_data.UseDatModTimeForDirs = m_cbTakeDatTime.Checked;

            m_data.Password = m_tbPass.Text;
            m_data.Username = m_tbUserName.Text;
            m_data.UpdateUNC();


            //save database stuff
            m_data.DbName = m_tbDatabaseName.Text;
            m_data.DbTblName = m_tbTableName.Text;
            m_data.DbUserName = m_tbDbUsername.Text;
            m_data.DbPassword = m_tbDbPass.Text;
            m_data.DbServer = m_tbServer.Text;
            m_data.DbNamedServer = m_rbServer.Checked;
            m_data.DbProvider =  (UpdateDataTaskData.DbProviderEnum) m_cbProvider.SelectedIndex;
            m_data.DbAuthenticateNT = m_rbNT.Checked;

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        #endregion

        private void m_cbProvider_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (m_cbProvider.SelectedIndex == 0) //Msql
            {
                m_computer.Enabled = true;
                m_rbNT.Enabled = true;
            }
            else if (m_cbProvider.SelectedIndex > 0) //other
            {
                m_rbServer.Checked = false;
                m_rbLocal.Checked = true;
                m_computer.Enabled = false;
                m_rbNT.Checked = false;
                m_rbOtherAuth.Checked = true;
                m_rbNT.Enabled = false;
            }
        }

        private void m_tbDbUsername_TextChanged(object sender, EventArgs e)
        {
            m_rbNT.Checked = false;
            m_rbOtherAuth.Checked = true;
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

        private void m_btBrowseTarget_Click(object sender, EventArgs e)
        {
            m_folderBrowserDialog.ShowNewFolderButton = true;
            m_folderBrowserDialog.SelectedPath = m_targetFolderTextBox.Text;
            DialogResult result = m_folderBrowserDialog.ShowDialog();
            if (result == DialogResult.OK)
                m_targetFolderTextBox.Text = m_folderBrowserDialog.SelectedPath;
        }

        private void m_btBrowseServer_Click(object sender, EventArgs e)
        {
            string computer = BrowseForComputer.Browse();
            m_tbServer.Text = computer;
            m_rbServer.Checked = true;
            m_rbLocal.Checked = false;
        }

        private void m_btTestConnection_Click(object sender, EventArgs e)
        {
            SaveData();
            try
            {
                string message = null;
                using (WaitCursor wait = new WaitCursor())
                {
                    if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                        message = Program.CommunicationObject.TestDbTaskConnection(m_data);
                    else
                        message = UpdateDataTaskWorker.TestConnecton(m_data);
                }
                if (message != null && message == "success")
                    MessageBox.Show(this, iba.Properties.Resources.logUDTConnectSuccess, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                else if (message != null)
                    MessageBox.Show(this, message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
    }
}
