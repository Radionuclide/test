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
using System.Xml.Serialization;
using iba.Plugins;

namespace Alunorf_sinec_h1_plugin
{
    public partial class PluginH1TaskControl : UserControl, IPluginControl
    {
        private IDatCoHost m_datcoHost;
        private static readonly int TELEGRAM_INDEX = 0;
        private static readonly int INFOFIELDS_INDEX = 1;
        private static readonly int SIGNALS_INDEX = 2;
        private static readonly int TELEGRAM_NEW_INDEX = 3;
        
        public PluginH1TaskControl(IDatCoHost host)
        {
            m_datcoHost = host;
            InitializeComponent();

            ImageList telegramImageList = new ImageList();
            telegramImageList.Images.Add(Alunorf_sinec_h1_plugin.Properties.Resources.telegram);
            telegramImageList.Images.Add(Alunorf_sinec_h1_plugin.Properties.Resources.info);
            telegramImageList.Images.Add(Alunorf_sinec_h1_plugin.Properties.Resources.signal);
            telegramImageList.Images.Add(Alunorf_sinec_h1_plugin.Properties.Resources.telegram_new);
            m_tvMessages.ImageList = telegramImageList;
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

            MacAddr temp = new MacAddr();
            temp.FirstByte = m_data.OwnAddress[0];
            temp.SecondByte = m_data.OwnAddress[1];
            temp.ThirdByte = m_data.OwnAddress[2];
            temp.FourthByte = m_data.OwnAddress[3];
            temp.FifthByte = m_data.OwnAddress[4];
            temp.SixthByte = m_data.OwnAddress[5];
            m_ownMAC.Text = temp.Address;

            temp.FirstByte = m_data.NQSAddress1[0];
            temp.SecondByte = m_data.NQSAddress1[1];
            temp.ThirdByte = m_data.NQSAddress1[2];
            temp.FourthByte = m_data.NQSAddress1[3];
            temp.FifthByte = m_data.NQSAddress1[4];
            temp.SixthByte = m_data.NQSAddress1[5];
            m_nqs1MAC.Text = temp.Address;

            temp.FirstByte = m_data.NQSAddress2[0];
            temp.SecondByte = m_data.NQSAddress2[1];
            temp.ThirdByte = m_data.NQSAddress2[2];
            temp.FourthByte = m_data.NQSAddress2[3];
            temp.FifthByte = m_data.NQSAddress2[4];
            temp.SixthByte = m_data.NQSAddress2[5];
            m_nqs2MAC.Text = temp.Address;

            m_tbTSAP_NQS1_PC.Text = m_data.OwnTSAPforNQS1;
            m_tbTSAP_NQS2_PC.Text = m_data.OwnTSAPforNQS2;
            m_tbTSAP_NQS1_NQS.Text = m_data.NQS_TSAPforNQS1;
            m_tbTSAP_NQS2_NQS.Text = m_data.NQS_TSAPforNQS2;

            BuildTree();

            m_nudRetryConnectTimeInterval.Value = (decimal)m_data.RetryConnectTimeInterval;
            m_nudTryconnectTimeout.Value = (decimal)m_data.ConnectionTimeOut;
            m_nudSendTimeout.Value = (decimal)m_data.SendTimeOut;
            m_nudAckTimeout.Value = (decimal)m_data.AckTimeOut;
            m_refreshTimer.Start();
        }

        private void BuildTree()
        {
            //build the message tree
            m_tvMessages.Nodes.Clear();
            foreach (PluginH1Task.Telegram tele in m_data.Telegrams)
            {
                TreeNode n = new TreeNode(Alunorf_sinec_h1_plugin.Properties.Resources.telegramNodeText + tele.QdtType.ToString(),TELEGRAM_INDEX,TELEGRAM_INDEX);
                m_tvMessages.Nodes.Add(n);
                TreeNode i = new TreeNode(Alunorf_sinec_h1_plugin.Properties.Resources.infoNodeText, INFOFIELDS_INDEX, INFOFIELDS_INDEX);
                n.Nodes.Add(i);
                TreeNode j = new TreeNode(Alunorf_sinec_h1_plugin.Properties.Resources.signalNodeText, SIGNALS_INDEX, SIGNALS_INDEX);
                n.Nodes.Add(j);
            }
            //add the new node.tag;
            TreeNode newTelegramNode = new TreeNode(Alunorf_sinec_h1_plugin.Properties.Resources.newTelegramNodeText, TELEGRAM_NEW_INDEX, TELEGRAM_NEW_INDEX);
            newTelegramNode.ForeColor = Color.Blue;
            m_tvMessages.Nodes.Add(newTelegramNode);

            if (m_data.LastSelectedTelegram >= IndexFromNode(newTelegramNode))
                m_data.LastSelectedTelegram = -1;

            TreeNode node = NodeFromIndex(m_data.LastSelectedTelegram);

            //select the appropriate node;
            if (m_tvMessages.SelectedNode == node)
            {
                TreeViewEventArgs args = new TreeViewEventArgs(node);
                m_tvMessages_AfterSelect(null, args);
            }
            else m_tvMessages.SelectedNode = node;
        }


        private TreeNode NodeFromIndex(int index)
        {
            if (index < 0) return null;
            else if (index % 3 == 0) return m_tvMessages.Nodes[index / 3];
            else return m_tvMessages.Nodes[index / 3].Nodes[index % 3 - 1];
        }

        private int IndexFromNode(TreeNode node)
        {
            if (node == null) return -1;
            else if (node.Parent == null) return 3 * node.Index;
            else return 3 * node.Parent.Index + node.Index + 1;
        }

        public void LeaveCleanup()
        {
            m_refreshTimer.Enabled = false;
        }

        public void  SaveData()
        {
            MacAddr temp = new MacAddr();
            temp.Address = m_ownMAC.Text;
            m_data.OwnAddress[0] = temp.FirstByte;
            m_data.OwnAddress[1] = temp.SecondByte;
            m_data.OwnAddress[2] = temp.ThirdByte;
            m_data.OwnAddress[3] = temp.FourthByte;
            m_data.OwnAddress[4] = temp.FifthByte;
            m_data.OwnAddress[5] = temp.SixthByte;

            temp.Address = m_nqs1MAC.Text;
            m_data.NQSAddress1[0] = temp.FirstByte;
            m_data.NQSAddress1[1] = temp.SecondByte;
            m_data.NQSAddress1[2] = temp.ThirdByte;
            m_data.NQSAddress1[3] = temp.FourthByte;
            m_data.NQSAddress1[4] = temp.FifthByte;
            m_data.NQSAddress1[5] = temp.SixthByte;

            temp.Address = m_nqs2MAC.Text;
            m_data.NQSAddress2[0] = temp.FirstByte;
            m_data.NQSAddress2[1] = temp.SecondByte;
            m_data.NQSAddress2[2] = temp.ThirdByte;
            m_data.NQSAddress2[3] = temp.FourthByte;
            m_data.NQSAddress2[4] = temp.FifthByte;
            m_data.NQSAddress2[5] = temp.SixthByte;

            m_data.OwnTSAPforNQS1 = m_tbTSAP_NQS1_PC.Text;
            m_data.OwnTSAPforNQS2 = m_tbTSAP_NQS2_PC.Text;
            m_data.NQS_TSAPforNQS1 = m_tbTSAP_NQS1_NQS.Text;
            m_data.NQS_TSAPforNQS2 = m_tbTSAP_NQS2_NQS.Text;

            m_data.RetryConnectTimeInterval = (int)m_nudRetryConnectTimeInterval.Value;
            m_data.ConnectionTimeOut = (int)m_nudTryconnectTimeout.Value;
            m_data.SendTimeOut = (int)m_nudSendTimeout.Value;
            m_data.AckTimeOut = (int)m_nudAckTimeout.Value;
            m_data.LastSelectedTelegram = IndexFromNode(m_tvMessages.SelectedNode);
            SaveTelegram();
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


        private void m_btOpen_Click(object sender, EventArgs e)
        {
            m_openFileDialog1.CheckFileExists = true;
            m_openFileDialog1.FileName = "messages.xml";
            m_openFileDialog1.Filter = "XML files (*.xml)|*.xml";
            if (m_openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    string filename = m_openFileDialog1.FileName;
                    XmlSerializer mySerializer = new XmlSerializer(typeof(List<PluginH1Task.Telegram>));
                    using (FileStream myFileStream = new FileStream(filename, FileMode.Open))
                    {
                        List<PluginH1Task.Telegram> teles = (List<PluginH1Task.Telegram>) mySerializer.Deserialize(myFileStream);
                        m_data.Telegrams = teles;
                        BuildTree();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Alunorf_sinec_h1_plugin.Properties.Resources.err_xml_load + ex.ToString());
                }
            }
        }

        private void m_btSave_Click(object sender, EventArgs e)
        {
            m_saveFileDialog1.CreatePrompt = false;
            m_saveFileDialog1.OverwritePrompt = true;
            m_saveFileDialog1.FileName = "messages";
            m_saveFileDialog1.DefaultExt = "xml";
            m_saveFileDialog1.Filter = "XML files (*.xml)|*.xml";

            if (m_saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(List<PluginH1Task.Telegram>));
                    using (StreamWriter myWriter = new StreamWriter(m_saveFileDialog1.FileName))
                    {
                        mySerializer.Serialize(myWriter, m_data.Telegrams);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(Alunorf_sinec_h1_plugin.Properties.Resources.err_xml_save + ex.ToString());
                }
            }
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
                    row = Algorithms.min<DataGridViewCell>(m_datagvMessages.SelectedCells, delegate(DataGridViewCell a, DataGridViewCell b) { return a.RowIndex - b.RowIndex; }).RowIndex;
                    col = Algorithms.min<DataGridViewCell>(m_datagvMessages.SelectedCells, delegate(DataGridViewCell a, DataGridViewCell b) { return a.ColumnIndex - b.ColumnIndex; }).ColumnIndex;
                }

                if (e.Control && e.KeyCode == Keys.V && Clipboard.ContainsData(DataFormats.CommaSeparatedValue))
                {
                    object o = Clipboard.GetData(DataFormats.CommaSeparatedValue);
                    Stream stream = o as Stream;
                    String asstring = o as String;
                    TextReader sr = null;
                    if (stream != null) sr = new StreamReader(stream);
                    else if (asstring != null) sr = new StringReader(asstring);
                    else return;
                    try
                    {
                        for (string s = sr.ReadLine(); s != null; s = sr.ReadLine())
                        {
                            if (string.IsNullOrEmpty(s) || s == "\0")
                            {
                                row++;
                                continue;
                            }
                            while (row + 1 >= m_datagvMessages.RowCount) //always leav
                                m_datagvMessages.RowCount++;
                            string[] sr_array = s.Split(',');
                            for (int col2 = col; col2 < sr_array.Length + col; col2++)
                            {

                                if (col2 == 0 || col2 == 2) //comment or field 
                                    (m_datagvMessages.Rows[row].Cells[col2]).Value = sr_array[col2 - col];
                                else if (col2 == 1)
                                {
                                    DataGridViewComboBoxCell cell = (m_datagvMessages.Rows[row].Cells[col2] as DataGridViewComboBoxCell);
                                    string key = sr_array[col2 - col].ToLower().Replace(" ", null);
                                    if (cell.Items.Contains(key))
                                        cell.Value = key;
                                }
                                else break;
                            }
                            row++;
                        }
                    }
                    finally
                    {
                        sr.Dispose();
                    }
                }
            }
            catch (ExternalException)
            {
            }
        }

        private PluginH1Task.Telegram m_telegram;
        private enum TelegramPart {QDT_TYPE, INFOFIELDS, SIGNALFIELDS }
        private TelegramPart m_telegramPart;

        private void m_tvMessages_AfterSelect(object sender, TreeViewEventArgs e)
        {
            TreeNode node = e.Node;

            if (node == null)
            {
                SaveTelegram();
                m_datagvMessages.AllowUserToAddRows = false;
                m_datagvMessages.AllowUserToDeleteRows = false;
                m_datagvMessages.RowCount = 0;
                m_telegram = null;
            }
            else if (node.ImageIndex == TELEGRAM_NEW_INDEX)
            {
                SaveTelegram();
                PluginH1Task.Telegram telegram = new PluginH1Task.Telegram();
                m_data.Telegrams.Add(telegram);
                TreeNode n = new TreeNode(Alunorf_sinec_h1_plugin.Properties.Resources.telegramNodeText + telegram.QdtType.ToString(), TELEGRAM_INDEX, TELEGRAM_INDEX);
                m_tvMessages.Nodes.Insert(node.Index, n);
                TreeNode i = new TreeNode(Alunorf_sinec_h1_plugin.Properties.Resources.infoNodeText, INFOFIELDS_INDEX, INFOFIELDS_INDEX);
                n.Nodes.Add(i);
                TreeNode j = new TreeNode(Alunorf_sinec_h1_plugin.Properties.Resources.signalNodeText, SIGNALS_INDEX, SIGNALS_INDEX);
                n.Nodes.Add(j);
                m_tvMessages.SelectedNode = n;
            }
            else if (node.ImageIndex == TELEGRAM_INDEX)
            {
                SaveTelegram();
                m_telegram = m_data.Telegrams[node.Index];
                m_telegramPart = TelegramPart.QDT_TYPE;
                m_datagvMessages.AllowUserToAddRows = false;
                m_datagvMessages.AllowUserToDeleteRows = false;
                m_datagvMessages.RowCount = 2;
                m_datagvMessages.Rows[0].Cells[0].Value = "QdtTyp";
                m_datagvMessages.Rows[0].Cells[1].Value = "int2";
                m_datagvMessages.Rows[0].Cells[2].Value = m_telegram.QdtType.ToString();
                m_datagvMessages.Rows[1].Cells[0].Value = "Description";
                m_datagvMessages.Rows[1].Cells[1].Value = null;
                m_datagvMessages.Rows[1].Cells[2].Value = m_telegram.Description;
                m_datagvMessages.Rows[1].Cells[2].ToolTipText = Alunorf_sinec_h1_plugin.Properties.Resources.tooltipDesc;
                m_columnFieldname.ReadOnly = true;
                m_columnDataType.ReadOnly = true;
            }
            else if (node.ImageIndex == INFOFIELDS_INDEX)
            {
                SaveTelegram();
                m_telegram = m_data.Telegrams[node.Parent.Index];
                m_telegramPart = TelegramPart.INFOFIELDS;
                m_datagvMessages.AllowUserToAddRows = true;
                m_datagvMessages.AllowUserToDeleteRows = true;
                m_columnFieldname.ReadOnly = false;
                m_columnDataType.ReadOnly = false;
                int count = m_telegram.DataInfo.Count;
                m_datagvMessages.RowCount = count + 1;
                for (int i = 0; i < count; i++)
                {
                    m_datagvMessages.Rows[i].Cells[0].Value = m_telegram.DataInfo[i].Name;
                    m_datagvMessages.Rows[i].Cells[1].Value = m_telegram.DataInfo[i].DataType;
                    m_datagvMessages.Rows[i].Cells[2].Value = m_telegram.DataInfo[i].Comment;
                    m_datagvMessages.Rows[i].Cells[2].ToolTipText = null;
                }
                m_datagvMessages.Rows[count].Cells[0].Value = null;
                m_datagvMessages.Rows[count].Cells[1].Value = null;
                m_datagvMessages.Rows[count].Cells[2].Value = null;
                m_datagvMessages.Rows[count].Cells[2].ToolTipText = null;
            }
            else if (node.ImageIndex == SIGNALS_INDEX)
            {
                SaveTelegram();
                m_telegram = m_data.Telegrams[node.Parent.Index];
                m_telegramPart = TelegramPart.SIGNALFIELDS;
                m_datagvMessages.AllowUserToAddRows = true;
                m_datagvMessages.AllowUserToDeleteRows = true;
                m_columnFieldname.ReadOnly = false;
                m_columnDataType.ReadOnly = false;
                int count = m_telegram.DataSignal.Count;
                m_datagvMessages.RowCount = count + 1;
                for (int i = 0; i < count; i++)
                {
                    m_datagvMessages.Rows[i].Cells[0].Value = m_telegram.DataSignal[i].Name;
                    m_datagvMessages.Rows[i].Cells[1].Value = m_telegram.DataSignal[i].DataType;
                    m_datagvMessages.Rows[i].Cells[2].Value = m_telegram.DataSignal[i].Comment;
                    m_datagvMessages.Rows[i].Cells[2].ToolTipText = null;
                }
                m_datagvMessages.Rows[count].Cells[0].Value = null;
                m_datagvMessages.Rows[count].Cells[1].Value = null;
                m_datagvMessages.Rows[count].Cells[2].Value = null;
                m_datagvMessages.Rows[count].Cells[2].ToolTipText = null;
            }
        }

        private void SaveTelegram()
        {
            if (m_telegram == null) return;
            switch (m_telegramPart)
            {
                case TelegramPart.QDT_TYPE:
                    m_telegram.Description = m_datagvMessages.Rows[1].Cells[2].Value as string;
                    try
                    {
                        m_telegram.QdtType = short.Parse(m_datagvMessages.Rows[0].Cells[2].Value as string);
                    }
                    catch 
                    {
                        m_telegram.QdtType = 0;
                    }
                    break;
                case TelegramPart.INFOFIELDS:
                    m_telegram.DataInfo.Clear();
                    for (int i = 0; i < m_datagvMessages.RowCount-1; i++)
                    {
                        PluginH1Task.TelegramRecord line = new PluginH1Task.TelegramRecord();
                        line.Name = m_datagvMessages.Rows[i].Cells[0].Value as string;
                        line.DataType = m_datagvMessages.Rows[i].Cells[1].Value as string;
                        line.Comment = m_datagvMessages.Rows[i].Cells[2].Value as string;
                        m_telegram.DataInfo.Add(line);
                    }
                    break;
                case TelegramPart.SIGNALFIELDS:
                    m_telegram.DataSignal.Clear();
                    for (int i = 0; i < m_datagvMessages.RowCount-1; i++)
                    {
                        PluginH1Task.TelegramRecord line = new PluginH1Task.TelegramRecord();
                        line.Name = m_datagvMessages.Rows[i].Cells[0].Value as string;
                        line.DataType = m_datagvMessages.Rows[i].Cells[1].Value as string;
                        line.Comment = m_datagvMessages.Rows[i].Cells[2].Value as string;
                        m_telegram.DataSignal.Add(line);
                    }
                    break;
            }
        }

        private void m_datagvMessages_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (m_telegram != null && m_telegramPart == TelegramPart.QDT_TYPE && m_datagvMessages.RowCount == 2)
            {
                try
                {
                    short id = short.Parse(m_datagvMessages.Rows[0].Cells[2].Value as string);
                    m_tvMessages.SelectedNode.Text = Alunorf_sinec_h1_plugin.Properties.Resources.telegramNodeText + id.ToString();
                }
                catch
                {

                }
            }
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (m_telegram == null || m_tvMessages.SelectedNode == null ) return;
            switch (m_telegramPart)
            {
                case TelegramPart.QDT_TYPE:
                    TreeNode actNode = m_tvMessages.SelectedNode;
                    TreeNode nextNode = m_tvMessages.SelectedNode.NextNode;
                    if (nextNode.ImageIndex != TELEGRAM_NEW_INDEX)
                        m_tvMessages.SelectedNode = nextNode;
                    else if (actNode.PrevNode != null)
                    {
                        m_tvMessages.SelectedNode = actNode.PrevNode;
                    }
                    else
                    {
                        m_tvMessages.SelectedNode = null;
                        m_telegram = null;
                        m_datagvMessages.AllowUserToAddRows = false;
                        m_datagvMessages.AllowUserToDeleteRows = false;
                        m_datagvMessages.RowCount = 0;
                    }
                    m_data.Telegrams.RemoveAt(actNode.Index);
                    m_tvMessages.Nodes.Remove(actNode);
                    break;
                case TelegramPart.INFOFIELDS:
                case TelegramPart.SIGNALFIELDS:
                    m_datagvMessages.RowCount = 1;
                    SaveTelegram();
                    break;
            }
        }

        private void m_contextMenu_Opening(object sender, CancelEventArgs e)
        {
            deleteToolStripMenuItem.Enabled = m_tvMessages.SelectedNode != null && m_tvMessages.SelectedNode.ImageIndex != TELEGRAM_NEW_INDEX;
        }

        private void m_tvMessages_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete && m_tvMessages.SelectedNode != null && m_tvMessages.SelectedNode.ImageIndex != TELEGRAM_NEW_INDEX)
                deleteToolStripMenuItem_Click(null,null);
        }

        private void m_tvMessages_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right)
                return;
            TreeNode node = m_tvMessages.GetNodeAt(e.X, e.Y);
            if (node != null && node.ImageIndex != TELEGRAM_NEW_INDEX) m_tvMessages.SelectedNode = node;
        }

        private void m_tvMessages_ItemDrag(object sender, ItemDragEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                TreeNode draggedNode = (TreeNode)e.Item;
                m_tvMessages.SelectedNode = draggedNode;
                if (draggedNode.ImageIndex == TELEGRAM_INDEX)
                    DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }

        private void m_tvMessages_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Effect == DragDropEffects.None) return;
            // Retrieve the client coordinates of the drop location.
            Point targetPoint = m_tvMessages.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = m_tvMessages.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));


            if (e.Effect == DragDropEffects.Move)
            {
                SaveTelegram();
                m_tvMessages.SelectedNode = null;
                int index = targetNode.Index;
                m_tvMessages.BeginUpdate();
                m_tvMessages.Nodes.Remove(draggedNode);
                m_data.Telegrams.Remove(m_telegram);
                m_data.Telegrams.Insert(index, m_telegram);
                m_tvMessages.Nodes.Insert(index, draggedNode);
                m_tvMessages.EndUpdate();
                m_tvMessages.SelectedNode = m_tvMessages.Nodes[index];
            }
            else if (e.Effect == DragDropEffects.Copy)
            {
                SaveTelegram();
                m_tvMessages.SelectedNode = null;
                int index = targetNode.Index;
                m_tvMessages.BeginUpdate();
                m_data.Telegrams.Insert(index, m_telegram.Clone());
                m_tvMessages.Nodes.Insert(index, (TreeNode) draggedNode.Clone());
                m_tvMessages.EndUpdate();
                m_tvMessages.SelectedNode = m_tvMessages.Nodes[index];
            }
        }

        private void m_tvMessages_DragOver(object sender, DragEventArgs e)
        {
            Point targetPoint = m_tvMessages.PointToClient(new Point(e.X, e.Y));

            // Retrieve the node at the drop location.
            TreeNode targetNode = m_tvMessages.GetNodeAt(targetPoint);

            // Retrieve the node that was dragged.
            TreeNode draggedNode = (TreeNode)e.Data.GetData(typeof(TreeNode));
            if (targetNode.ImageIndex != TELEGRAM_INDEX )
            {
                e.Effect = DragDropEffects.None;
            }
            else if (targetNode == draggedNode)
            {
                e.Effect = DragDropEffects.Copy;
            }
            else
            {
                if ((ModifierKeys & Keys.Control) == Keys.Control)
                    e.Effect = DragDropEffects.Copy;
                else
                    e.Effect = DragDropEffects.Move;
            }

        }
    }
}
