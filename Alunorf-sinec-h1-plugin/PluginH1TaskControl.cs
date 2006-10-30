using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Globalization;
using System.Runtime.InteropServices;
using System.IO;
using iba.Plugins;

namespace Alunorf_sinec_h1_plugin
{
    public partial class PluginH1TaskControl : UserControl, IPluginControl
    {
        private IDatCoHost m_datcoHost;
        public PluginH1TaskControl(IDatCoHost host)
        {
            m_datcoHost = host;
            InitializeComponent();
        }

        private void m_pcmac1_Validating(object sender, CancelEventArgs e)
        {
            TextBox tb = sender as TextBox;
            if (tb.Text.Length == 1)
            {
                char num1 = tb.Text.ToCharArray()[0];
                if ((num1 < '0' || num1 > '9') && (num1 < 'A' || num1 > 'F'))
                {
                    e.Cancel = true;
                    return;
                }

                tb.Text = "0" + tb.Text;
            }
            else if (tb.Text.Length == 2)
            {
                char num1 = tb.Text.ToCharArray()[0];
                char num2 = tb.Text.ToCharArray()[1];
                if (((num1 < '0' || num1 > '9') && (num1 < 'A' || num1 > 'F'))
                    || ((num2 < '0' || num2 > '9') && (num2 < 'A' || num2 > 'F')))
                {
                    e.Cancel = true;
                }
            }
            else
                e.Cancel = true;
        }
    
        #region IPluginControl Members

        PluginH1Task m_data;
        ICommonTaskControl m_control;

        public void  LoadData(object datasource, ICommonTaskControl parentcontrol)
        {
 	        m_data = datasource as PluginH1Task;
            m_control = parentcontrol;
            m_pcmac1.Text = m_data.OwnAddress[0].ToString("X2");
            m_pcmac2.Text = m_data.OwnAddress[1].ToString("X2");
            m_pcmac3.Text = m_data.OwnAddress[2].ToString("X2");
            m_pcmac4.Text = m_data.OwnAddress[3].ToString("X2");
            m_pcmac5.Text = m_data.OwnAddress[4].ToString("X2");
            m_pcmac6.Text = m_data.OwnAddress[5].ToString("X2");

            m_nqs1mac1.Text = m_data.NQSAddress1[0].ToString("X2");
            m_nqs1mac2.Text = m_data.NQSAddress1[1].ToString("X2");
            m_nqs1mac3.Text = m_data.NQSAddress1[2].ToString("X2");
            m_nqs1mac4.Text = m_data.NQSAddress1[3].ToString("X2");
            m_nqs1mac5.Text = m_data.NQSAddress1[4].ToString("X2");
            m_nqs1mac6.Text = m_data.NQSAddress1[5].ToString("X2");
            
            m_nqs2mac1.Text = m_data.NQSAddress2[0].ToString("X2");
            m_nqs2mac2.Text = m_data.NQSAddress2[1].ToString("X2");
            m_nqs2mac3.Text = m_data.NQSAddress2[2].ToString("X2");
            m_nqs2mac4.Text = m_data.NQSAddress2[3].ToString("X2");
            m_nqs2mac5.Text = m_data.NQSAddress2[4].ToString("X2");
            m_nqs2mac6.Text = m_data.NQSAddress2[5].ToString("X2");

            m_tbTSAP_NQS1_PC.Text = m_data.OwnTSAPforNQS1;
            m_tbTSAP_NQS2_PC.Text = m_data.OwnTSAPforNQS2;
            m_tbTSAP_NQS1_NQS.Text = m_data.NQS_TSAPforNQS1;
            m_tbTSAP_NQS2_NQS.Text = m_data.NQS_TSAPforNQS2;

            m_nudRetryConnectTimeInterval.Value = (decimal)m_data.RetryConnectTimeInterval;
            m_nudTryconnectTimeout.Value = (decimal)m_data.ConnectionTimeOut;
            m_nudSendTimeout.Value = (decimal)m_data.SendTimeOut;
            m_nudAckTimeout.Value = (decimal)m_data.AckTimeOut;
            m_refreshTimer.Start();

        }

        public void  SaveData()
        {
            m_data.OwnAddress[0] = Byte.Parse(m_pcmac1.Text, NumberStyles.HexNumber);
            m_data.OwnAddress[1] = Byte.Parse(m_pcmac2.Text, NumberStyles.HexNumber);
            m_data.OwnAddress[2] = Byte.Parse(m_pcmac3.Text, NumberStyles.HexNumber);
            m_data.OwnAddress[3] = Byte.Parse(m_pcmac4.Text, NumberStyles.HexNumber);
            m_data.OwnAddress[4] = Byte.Parse(m_pcmac5.Text, NumberStyles.HexNumber);
            m_data.OwnAddress[5] = Byte.Parse(m_pcmac6.Text, NumberStyles.HexNumber);

            m_data.NQSAddress1[0] = Byte.Parse(m_nqs1mac1.Text, NumberStyles.HexNumber);
            m_data.NQSAddress1[1] = Byte.Parse(m_nqs1mac2.Text, NumberStyles.HexNumber);
            m_data.NQSAddress1[2] = Byte.Parse(m_nqs1mac3.Text, NumberStyles.HexNumber);
            m_data.NQSAddress1[3] = Byte.Parse(m_nqs1mac4.Text, NumberStyles.HexNumber);
            m_data.NQSAddress1[4] = Byte.Parse(m_nqs1mac5.Text, NumberStyles.HexNumber);
            m_data.NQSAddress1[5] = Byte.Parse(m_nqs1mac6.Text, NumberStyles.HexNumber);

            m_data.NQSAddress2[0] = Byte.Parse(m_nqs2mac1.Text, NumberStyles.HexNumber);
            m_data.NQSAddress2[1] = Byte.Parse(m_nqs2mac2.Text, NumberStyles.HexNumber);
            m_data.NQSAddress2[2] = Byte.Parse(m_nqs2mac3.Text, NumberStyles.HexNumber);
            m_data.NQSAddress2[3] = Byte.Parse(m_nqs2mac4.Text, NumberStyles.HexNumber);
            m_data.NQSAddress2[4] = Byte.Parse(m_nqs2mac5.Text, NumberStyles.HexNumber);
            m_data.NQSAddress2[5] = Byte.Parse(m_nqs2mac6.Text, NumberStyles.HexNumber);

            m_data.OwnTSAPforNQS1 = m_tbTSAP_NQS1_PC.Text;
            m_data.OwnTSAPforNQS2 = m_tbTSAP_NQS2_PC.Text;
            m_data.NQS_TSAPforNQS1 = m_tbTSAP_NQS1_NQS.Text;
            m_data.NQS_TSAPforNQS2 = m_tbTSAP_NQS2_NQS.Text;

            m_data.RetryConnectTimeInterval = (int)m_nudRetryConnectTimeInterval.Value;
            m_data.ConnectionTimeOut = (int)m_nudTryconnectTimeout.Value;
            m_data.SendTimeOut = (int)m_nudSendTimeout.Value;
            m_data.AckTimeOut = (int)m_nudAckTimeout.Value;
        }
        #endregion

        private void m_refreshTimer_Tick(object sender, EventArgs e)
        {
            m_refreshTimer.Enabled = false;
            PluginTaskWorkerStatus stat = m_datcoHost.GetStatusPlugin(m_control.ParentConfigurationID(), m_control.TaskIndex());
            switch ((stat.extraData as NqsServerStatusses).nqs1)
            { 
                case PluginH1TaskWorker.NQSStatus.DISCONNECTED:
                    m_statusNQS1.Text = Alunorf_sinec_h1_plugin.Properties.Resources.disconnected;
                    m_statusNQS1.ForeColor = Color.Red;
                    break;
                case PluginH1TaskWorker.NQSStatus.CONNECTED:
                    m_statusNQS1.Text = Alunorf_sinec_h1_plugin.Properties.Resources.connected;
                    m_statusNQS1.ForeColor = Color.Orange;
                    break;
                case PluginH1TaskWorker.NQSStatus.INITIALISED:
                    m_statusNQS1.Text = Alunorf_sinec_h1_plugin.Properties.Resources.initialised;
                    m_statusNQS1.ForeColor = Color.Green;
                    break;
                case PluginH1TaskWorker.NQSStatus.GO:
                    m_statusNQS1.Text = Alunorf_sinec_h1_plugin.Properties.Resources.go;
                    m_statusNQS1.ForeColor = Color.Green;
                    break;
            }
            m_refreshTimer.Enabled = true;
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (!Visible)
                m_refreshTimer.Stop();
        }

        private void m_btOpen_Click(object sender, EventArgs e)
        {

        }

        private void m_btSave_Click(object sender, EventArgs e)
        {

        }

        private void m_datagvMessages_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                int row = 0;
                int col = 0;

                if (m_datagvMessages.CurrentCell != null)
                {
                    row = m_datagvMessages.CurrentCell.RowIndex;
                    col = m_datagvMessages.CurrentCell.ColumnIndex;
                }
                if (m_datagvMessages.SelectedCells.Count > 2) //multiselect, only take first
                {
                    row = Algorithms.min< DataGridViewCell>(m_datagvMessages.SelectedCells, delegate(DataGridViewCell a, DataGridViewCell b) { return a.RowIndex - b.RowIndex; }).RowIndex;
                    col = Algorithms.min<DataGridViewCell>(m_datagvMessages.SelectedCells, delegate(DataGridViewCell a, DataGridViewCell b) { return a.ColumnIndex - b.ColumnIndex; }).ColumnIndex;
                }

                if (e.Control && e.KeyCode == Keys.V && Clipboard.ContainsData(DataFormats.CommaSeparatedValue))
                {
                    object o = Clipboard.GetData(DataFormats.CommaSeparatedValue);
                    using (StreamReader sr = new StreamReader((Stream)Clipboard.GetData(DataFormats.CommaSeparatedValue)))
                    {
                        for (string s = sr.ReadLine(); s != null; s = sr.ReadLine())
                        {
                            if (string.IsNullOrEmpty(s) || s == "\0")
                            {
                                row++;
                                continue;
                            }
                            while (row+1 >= m_datagvMessages.RowCount) //always leav
                                m_datagvMessages.RowCount++;
                            string[] sr_array = s.Split(',');
                            for (int col2 = col; col2 < sr_array.Length + col; col2++)
                            {

                                if (col2 == 0 || col2 == 2) //comment or field 
                                    (m_datagvMessages.Rows[row].Cells[col2]).Value = sr_array[col2 - col];
                                else if (col2 == 1)
                                {
                                    DataGridViewComboBoxCell cell = (m_datagvMessages.Rows[row].Cells[col2] as DataGridViewComboBoxCell);
                                    if (cell.Items.Contains(sr_array[col2 - col]))
                                        cell.Value = sr_array[col2 - col];
                                }
                                else break;
                            }
                            row++;
                        }
                    }
                }
            }
            catch (ExternalException ex)
            {
            }
        }
    }
}
