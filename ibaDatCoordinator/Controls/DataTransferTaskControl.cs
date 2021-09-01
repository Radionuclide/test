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
using Messages.V1;

namespace iba.Controls
{
    public partial class DataTransferTaskControl : UserControl, IPropertyPane
    {
        public DataTransferTaskControl()
        {
            InitializeComponent();
            m_numBandwidth.Maximum = decimal.MaxValue;
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

            m_cbDeleteAfterTransfer.Checked = m_data.ShouldDeleteAfterTransfer;
            m_btnCheckConnection.Image = null;
            m_btnCheckConnection.Text = "?";
            m_tbServer.Text = m_data.Server;
            m_numericUpDownPort.Text = m_data.Port;
            m_tbRemotePath.Text = m_data.RemotePath;
            m_numBandwidth.Text = m_data.MaxBandwidth.ToString();
            
            m_chkLimitBandwidth.Checked = m_data.ChkLimitBandwidth;
            m_cbBandwidth.SelectedIndex = m_data.CbBandwidth;
            m_numBandwidth.Value = m_data.NumBandwidth;

            m_cbBandwidth.Enabled = m_data.ChkLimitBandwidth;
            m_numBandwidth.Enabled = m_data.ChkLimitBandwidth;

            m_rbDatFile.Checked = m_data.WhatFileTransfer == DataTransferTaskData.WhatFileTransferEnum.DATFILE;
            m_rbPrevOutput.Checked = m_data.WhatFileTransfer == DataTransferTaskData.WhatFileTransferEnum.PREVOUTPUT;
        }

        public void SaveData()
        {
            m_data.Server = m_tbServer.Text;
            m_data.Port = m_numericUpDownPort.Text;
            m_data.RemotePath = m_tbRemotePath.Text;
            m_data.MaxBandwidth = CalculateMaxBandwidth();
            m_data.ShouldDeleteAfterTransfer = m_cbDeleteAfterTransfer.Checked;

            m_data.ChkLimitBandwidth = m_chkLimitBandwidth.Checked;
            m_data.CbBandwidth = m_cbBandwidth.SelectedIndex;
            m_data.NumBandwidth = m_numBandwidth.Value;

            if (m_rbPrevOutput.Checked)
                m_data.WhatFileTransfer = DataTransferTaskData.WhatFileTransferEnum.PREVOUTPUT;
            else if (m_rbDatFile.Checked)
                m_data.WhatFileTransfer = DataTransferTaskData.WhatFileTransferEnum.DATFILE;

            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                TaskManager.Manager.ReplaceConfiguration(m_data.ParentConfigurationData);
        }

        private int CalculateMaxBandwidth()
        {
            if (!m_chkLimitBandwidth.Checked) return 0;

            switch (m_cbBandwidth.SelectedItem.ToString())
            {
                case "kbps":
                    return int.Parse(m_numBandwidth.Text);
                case "mbps":
                    return int.Parse(m_numBandwidth.Text) * 1024;
            }

            return 0;
        }

        private async void m_btnCheckConnection_Click(object sender, EventArgs e)
        {
            SaveData();
            var errormessage = string.Empty;
            var ok = false;

            try
            {
                var client = new GrpcClient(m_data);
                
                this.Cursor = Cursors.WaitCursor;
                this.m_btnCheckConnection.Enabled = false;

                var result = await client.TestConnectionAsync(m_data);


                if (result.Status == Status.Ok)
                {
                    ok = true;
                }
                else
                {
                    errormessage = result.Message;
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Log(Logging.Level.Exception, iba.Properties.Resources.logUploadTaskFailed + ": " + ex.Message);
                errormessage = ex.Message;
                ok = false;
            }

            this.Cursor = Cursors.Default;
            this.m_btnCheckConnection.Enabled = true;
            m_btnCheckConnection.Text = null;
            m_btnCheckConnection.Image = iba.Properties.Resources.thumbdown;
            ((Bitmap)m_btnCheckConnection.Image).MakeTransparent(Color.Magenta);


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

        private void m_btnSelectServer_Click(object sender, EventArgs e)
        {
            using (var ssf = new ServerSelectionForm(new ServerConfiguration(), true))
            {
                ssf.OnServerInfoSelected += FillServerFields;
                ssf.ShowDialog();
            }
        }

        private void FillServerFields(string server, string port)
        {
            m_tbServer.Text = server;
            m_numericUpDownPort.Text = port;
        }


        private void m_cbLimitBandwidth_CheckedChanged(object sender, EventArgs e)
        {
            if (!(sender is CheckBox checkBox)) return;

            m_cbBandwidth.Enabled = checkBox.Checked;
            m_numBandwidth.Enabled = checkBox.Checked;
        }

        private void m_cbBandwidth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (sender is ComboBox comboBox)
            {
                m_numBandwidth.Minimum = comboBox.SelectedItem.ToString() == "kbps" ? 64 : 1;
            }
        }
    }
}
