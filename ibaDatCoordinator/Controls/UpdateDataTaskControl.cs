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
            m_uncControl = new UNCTaskControl();
            panelOut.Controls.Add(m_uncControl);
            m_uncControl.Dock = DockStyle.Fill;
            m_uncControl.HideInfofieldFileNameOptions();
        }

        private UNCTaskControl m_uncControl;

        #region IPropertyPane Members
        IPropertyPaneManager m_manager;
        UpdateDataTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as UpdateDataTaskData;


            m_uncControl.SetData(m_data);

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
            m_uncControl.SaveData();
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
    }
}
