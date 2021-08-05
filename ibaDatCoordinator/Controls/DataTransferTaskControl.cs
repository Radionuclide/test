using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Data;
using iba.Dialogs;
using iba.Processing;
using iba.Processing.IbaGrpc;
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
            m_numericUpDownPort.Text = m_data.Port;
            m_tbRemotePath.Text = m_data.RemotePath;
            m_tbMaxBandwidth.Text = m_data.MaxBandwidth.ToString();
            m_rbDatFile.Checked = m_data.WhatFileTransfer == DataTransferTaskData.WhatFileTransferEnum.DATFILE;
            m_rbPrevOutput.Checked = m_data.WhatFileTransfer == DataTransferTaskData.WhatFileTransferEnum.PREVOUTPUT;
        }

        public void SaveData()
        {
            m_data.Server = m_tbServer.Text;
            m_data.Port = m_numericUpDownPort.Text;
            m_data.RemotePath = m_tbRemotePath.Text;
            m_data.MaxBandwidth = int.Parse(m_tbMaxBandwidth.Text);

            if (m_rbPrevOutput.Checked)
                m_data.WhatFileTransfer = DataTransferTaskData.WhatFileTransferEnum.PREVOUTPUT;
            else if (m_rbDatFile.Checked)
                m_data.WhatFileTransfer = DataTransferTaskData.WhatFileTransferEnum.DATFILE;

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
                    GrpcClient client = new GrpcClient(m_data);
                    client.TestConnection();
                    ok = true;
                }
                catch (Exception ex)
                {
                    LogData.Data.Log(Logging.Level.Exception, iba.Properties.Resources.logUploadTaskFailed + ": " + ex.Message);
                    errormessage = ex.Message;
                    ok = false;
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

        private void trackBarMaxBandwidth_Scroll(object sender, EventArgs e)
        {
            var trackBar = (sender as TrackBar);
            if (trackBar?.Value == null) return;

            m_data.MaxBandwidth = trackBar.Value;
            m_tbMaxBandwidth.Text = trackBar.Value.ToString();
        }

        private void m_tbMaxBandwidth_TextChanged(object sender, EventArgs e)
        {
            var control = (sender as TextBox);

            var result = int.TryParse(control?.Text, out var value);

            if (!result) return;

            if (value > trackBarMaxBandwidth.Maximum)
            {
                return;
            }

            if (value < 64)
            {
                trackBarMaxBandwidth.Value = 0;
                m_tbMaxBandwidth.Text = 0.ToString();
            }

            trackBarMaxBandwidth.Value = int.Parse(control?.Text ?? string.Empty);
        }

        private void m_btnSelectServer_Click(object sender, EventArgs e)
        {
            ServerConfiguration cf = new ServerConfiguration();
            cf.Address = Program.ServiceHost;
            cf.PortNr = Program.ServicePortNr;

            using (ServerSelectionForm ssf = new ServerSelectionForm(cf))
            {
                ssf.OnServerInfoSelected += (server, port) =>
                {
                    m_tbServer.Text = server;
                    m_numericUpDownPort.Text = port;
                };

                DialogResult r = ssf.ShowDialog();
                if (r == DialogResult.OK /*&& (port != cf.PortNr || server != cf.Address)*/)
                {
                    
                }
            }
        }
    }
}
