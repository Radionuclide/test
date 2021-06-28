using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Data;
using iba.Processing;
using iba.Utility;

namespace iba.Controls
{
    public partial class DataTransferTaskControl : UserControl, IPropertyPane
    {
        public DataTransferTaskControl()
        {
            InitializeComponent();
        }

        public void LeaveCleanup()
        {
        }


        IPropertyPaneManager m_manager;
        private DataTransferTaskData m_data;

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            m_data = datasource as DataTransferTaskData;

            m_btnCheckConnection.Image = null;
            m_btnCheckConnection.Text = "?";

            m_tbServer.Text = m_data.Server;
            m_tbPort.Text = m_data.Port;
            m_tbRemotePath.Text = m_data.RemotePath;
        }

        public void SaveData()
        {
            m_data.Server = m_tbServer.Text;
            m_data.Port = m_tbPort.Text;
            m_data.RemotePath = m_tbRemotePath.Text;


            if (m_rbPrevOutput.Checked)
                m_data.WhatFileUpload = DataTransferTaskData.WhatFileUploadEnum.PREVOUTPUT;
            else if (m_rbDatFile.Checked)
                m_data.WhatFileUpload = DataTransferTaskData.WhatFileUploadEnum.DATFILE;

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        private void m_btnCheckConnection_Click(object sender, EventArgs e)
        {
            SaveData();
            var errormessage = string.Empty;
            var ok = false;

            using (var wait = new WaitCursor())
            {
                try
                {
                }
                catch (Exception ex)
                {
                    LogData.Data.Log(Logging.Level.Exception, iba.Properties.Resources.logUploadTaskFailed + ": " + ex.Message);

                }

                m_btnCheckConnection.Text = null;
                m_btnCheckConnection.Image = iba.Properties.Resources.thumbdown;
                ((Bitmap)m_btnCheckConnection.Image).MakeTransparent(Color.Magenta);
            }

            if (ok)
            {
                m_btnCheckConnection.Text = null;
                m_btnCheckConnection.Image = iba.Properties.Resources.thumup;
            }
            else
            {
                MessageBox.Show(errormessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_btnCheckConnection.Text = null;
                m_btnCheckConnection.Image = iba.Properties.Resources.thumbdown;
            }
            ((Bitmap)m_btnCheckConnection.Image).MakeTransparent(Color.Magenta);
        }
    }
}
