using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Xml.Serialization;
using System.Threading;
using System.IO;
using Alunorf_sinec_h1_plugin;
using iba;

namespace Alunorf_plugin_test
{
    public partial class MainForm : Form
    {
        bool m_readStarted = false;

        public MainForm()
        {
            InitializeComponent();
            MacAddr temp = new MacAddr();
            temp.Address = m_ownMAC.Text;
            m_btGO.Enabled = false;
            m_btStart.Enabled = false;
            m_btStop.Enabled = false;
            m_setMessageDelegate = SetMessage;
            m_outputDir.Text = Directory.GetCurrentDirectory();
            m_otherTSAP.Text = "LT4NQS";
            m_ownTSAP.Text = "BR 1";
            temp = new MacAddr();
            temp.FirstByte  = 0x00;
            temp.SecondByte = 0x21;
            temp.ThirdByte  = 0xA0;
            temp.FourthByte = 0x11;
            temp.FifthByte  = 0x22;
            temp.SixthByte  = 0x33;
            m_otherMAC.Text = temp.Address;
            temp.FirstByte = 0x0A;
            temp.SecondByte = 0x00;
            temp.ThirdByte = 0x8E;
            temp.FourthByte = 0x00;
            temp.FifthByte = 0x00;
            temp.SixthByte = 0x01;
            m_ownMAC.Text = temp.Address;

            byte[] otherMAC = new byte[] { temp.FirstByte, temp.SecondByte, temp.ThirdByte, temp.FourthByte, temp.FifthByte, temp.SixthByte };

            m_h1manager = new CH1Manager();
            m_idToFilename = new BiMap<int,string>();
            m_acknowledgements = new SortedDictionary<string, bool>();
            m_reconnectTimer = new System.Threading.Timer(OnReconnectTimerTick);
            m_liveTimer = new System.Threading.Timer(OnLiveTimerTick);
        }

        private void m_btGO_Click(object sender, EventArgs e)
        {
            ushort id = 0;
            if (m_idToFilename.Contains("init_pc"))
            {
                id = (ushort) m_idToFilename["init_pc"];
                m_idToFilename[id] = "go";
            }
            else if (m_idToFilename.Contains("init"))
            {
                id = (ushort)m_idToFilename["init"];
                m_idToFilename[id] = "go";
            }
            else if (m_idToFilename.Contains("go"))
            {
                id = (ushort) m_idToFilename["go"];
            }
            else throw new Exception("go while there is no init or previous go");

            GoTelegram go = new GoTelegram(id, m_messagesCount++);
            lock (m_messageQueue)
            {
                m_messageQueue.Add(go);
            }
            //m_waitSendEvent = new AutoResetEvent(false);
            //m_waitSendEvent.WaitOne(1000 * ((int) (m_nudSendTimeout.Value + m_nudAckTimeout.Value)), true);

            //lock (m_acknowledgements)
            //{
            //    if (m_acknowledgements.ContainsKey("go"))
            //    {
            //        m_acknowledgements.Remove("go");
            //        SetMessage("GO acknowledged");
            //    }
            //    else
            //    {
            //        SetMessage("GO not acknowledged in time");
            //    }
            //}
        }

        private void m_btStop_Click(object sender, EventArgs e)
        {
            m_stop = true;
            m_thread.Join(60000);
            m_btStop.Enabled = false;
            m_btGO.Enabled = false;
        }

        private void m_btStart_Click(object sender, EventArgs e)
        {
            m_thread = new Thread(new ThreadStart(Run));
            //m_thread.SetApartmentState(ApartmentState.STA);

            MacAddr temp = new MacAddr();
            temp.Address = m_ownMAC.Text;
            byte[] ownMAC = new byte[] { temp.FirstByte, temp.SecondByte, temp.ThirdByte, temp.FourthByte, temp.FifthByte, temp.SixthByte };
            if (!m_h1manager.SetStationAddress(ownMAC))
            {
                m_stop = true;
                SetMessage("could not set station adress:" + m_h1manager.LastError);
                return;
            }
            if (!m_h1manager.SetSendTimeout((int)m_nudSendTimeout.Value))
            {
                m_stop = true;
                SetMessage("could not set send time out:" + m_h1manager.LastError);
                return;
            }

            SetMessage("station address and timeout set");

            m_thread.IsBackground = true;
            m_thread.Name = "workerthread for testprogram: ";
            m_retryConnect = true;
            m_thread.Start();
            m_btStart.Enabled = false;

            SetMessage("gui thread: waiting on connect");

            //wait here until initialisation succeeded
            //m_waitConnectedEvent = new AutoResetEvent(false);
            //m_waitConnectedEvent.WaitOne(1000 * ((int)m_nudSendTimeout.Value + (int)m_nudAckTimeout.Value + (int)m_nudTryconnectTimeout.Value + 30), true);
            //if (m_pcConnected == ConnectionState.READY)
            //{
                m_btStop.Enabled = true;
                m_btGO.Enabled = true;
            //}
            //else
            //{
            //    SetMessage("gui thread: connection timed out, will retry in several seconds");
            //}
            m_messageQueue = new List<AlunorfTelegram>();
        }

        private delegate void SetMessageDelegate(string filename);

        SetMessageDelegate m_setMessageDelegate;

        private void SetMessage(string message)
        {
            try
            {

                if (m_messages.InvokeRequired)
                {
                    m_messages.BeginInvoke(m_setMessageDelegate, new object[] { message });
                }
                else
                {
                    lock (m_messages)
                    {
                        m_messages.Rows.Add();
                        m_messages.Rows[m_messages.RowCount - 1].Cells[0].Value = DateTime.Now.ToString();
                        m_messages.Rows[m_messages.RowCount - 1].Cells[1].Value = message;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("fout: " + ex.ToString());
            }
        }

        private void m_btClearGrid_Click(object sender, EventArgs e)
        {
            m_messages.RowCount = 0;
        }

        private void m_btLoad_Click(object sender, EventArgs e)
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
                        List<PluginH1Task.Telegram> teles = (List<PluginH1Task.Telegram>)mySerializer.Deserialize(myFileStream);
                        m_telegrams = teles;
                        m_btStart.Enabled = true;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("fout: " + ex.ToString());
                }
            }
        }

        private List<PluginH1Task.Telegram> m_telegrams;
        private CH1Manager m_h1manager;
        private bool m_stop;
        private ushort m_vnr;
        private enum ConnectionState {DISCONNECTED,CONNECTED,READY};
        private ConnectionState m_pcConnected;
        private ushort m_messagesCount;
        private ushort m_idCounter;
        private BiMap<int, string> m_idToFilename;
        private SortedDictionary<string, bool> m_acknowledgements;
        private Thread m_thread;
        private System.Threading.Timer m_reconnectTimer;
        private System.Threading.Timer m_liveTimer;
        //private AutoResetEvent m_waitConnectedEvent;
        //private AutoResetEvent m_waitSendEvent;
        private bool m_retryConnect;
        private List<AlunorfTelegram> m_messageQueue;

        private void Run()
        {
            SetMessage("Cycle started");
            while (!m_stop)
            {
                if (m_retryConnect)
                {
                    BuildConnection();
                    if (m_pcConnected == ConnectionState.READY)
                    {
                        m_acknowledgements["live"] = false;
                        SendTelegram(new LiveTelegram(m_idCounter, m_messagesCount++), "live");
                        m_idCounter += 2;
                        m_liveTimer.Change(TimeSpan.FromMinutes(1.0), TimeSpan.Zero);
                    }
                }
                if (!ReadMessage())
                {
                    m_stop = true;
                    break;
                }
                lock (m_messageQueue)
                {
                    if (m_messageQueue.Count > 0)
                    {
                        string key = m_idToFilename[m_messageQueue[0].AktId];
                        if (m_pcConnected == ConnectionState.READY)
                        {
                            if (!SendTelegram(m_messageQueue[0], key))
                            {
                                m_acknowledgements[key] = false;
                                //m_waitSendEvent.Set();
                            }
                            m_messageQueue.RemoveAt(0);
                        }
                    }
                }
            }
            m_h1manager.DisconnectAll();
            SetMessage("Cycle started");
        }

        private void BuildConnection()
        {
            m_retryConnect = false;
            if (m_pcConnected == ConnectionState.DISCONNECTED)
            {
                CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
                m_h1manager.GetConnectionStatus(m_vnr, ref result);
                switch (result)
                {
                    case CH1Manager.H1Result.ALL_CLEAR:
                        {
                            m_pcConnected = ConnectionState.CONNECTED;
                            //send INI telegram
                            IniTelegram init = new IniTelegram(m_idCounter, m_messagesCount++);
                            m_idCounter += 2;
                            SendTelegram(init, "init");
                            break;
                        }
                    case CH1Manager.H1Result.WAIT_CONNECT:
                        m_retryConnect = true;
                        break;
                    case CH1Manager.H1Result.BAD_LINE:
                        MacAddr temp = new MacAddr();
                        temp.Address = m_otherMAC.Text;
                        byte[] otherMAC = new byte[] { temp.FirstByte, temp.SecondByte, temp.ThirdByte, temp.FourthByte, temp.FifthByte, temp.SixthByte };
                        SetMessage("worker thread: trying to connect");
                        if (m_h1manager.Connect(ref m_vnr, 4, true, otherMAC, m_ownTSAP.Text, m_otherTSAP.Text, ref result, (int) m_nudTryconnectTimeout.Value))
                        {
                            m_pcConnected = ConnectionState.CONNECTED;
                            SetMessage("worker thread: connected");
                            //send INI telegram
                            IniTelegram init = new IniTelegram(m_idCounter, m_messagesCount++);
                            m_idCounter += 2;
                            SendTelegram(init, "init");
                        }
                        else
                        {
                            SetMessage(m_h1manager.LastError);
                            m_retryConnect = true;
                        }
                        break;
                    default:
                        SetMessage(m_h1manager.LastError);
                        m_retryConnect = true;
                        break;
                }
            }
            if (m_retryConnect)
            {
                m_retryConnect = false;
                //start reconnect timer
                m_reconnectTimer.Change(TimeSpan.FromMinutes((double)m_nudRetryConnectTimeInterval.Value), TimeSpan.Zero);
            }
        }

        private void OnReconnectTimerTick(object ignoreMe)
        {
            if (m_stop) return;
            m_reconnectTimer.Change(Timeout.Infinite, Timeout.Infinite);
            m_retryConnect = true;
        }

        private void OnLiveTimerTick(object ignoreMe)
        {
            m_liveTimer.Change(Timeout.Infinite, Timeout.Infinite);
            if (m_pcConnected == ConnectionState.READY)
            {
                if (m_acknowledgements["live"] == false)
                {
                    m_retryConnect = true;
                    m_pcConnected = ConnectionState.DISCONNECTED;
                    SetMessage("Live message not acknowledged");
                    return;
                }
                m_acknowledgements["live"] = false;
                SendTelegram(new LiveTelegram(m_idCounter, m_messagesCount++), "live");
                m_liveTimer.Change(TimeSpan.FromMinutes(1.0), TimeSpan.Zero);
                m_idCounter += 2;
            }
        }

        private bool SendTelegram(AlunorfTelegram telegram)
        {
            return SendTelegram(telegram, null);
        }

        private bool SendTelegram(AlunorfTelegram telegram, string id)
        {
            //CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
            bool succes = false;
            CH1Manager.H1Result result = CH1Manager.H1Result.WAIT_CONNECT;
            while (result == CH1Manager.H1Result.WAIT_CONNECT)
            {
                SetMessage("message about to be sent: id " + ((id == null) ? telegram.AktId.ToString() : id));
                succes = m_h1manager.SendNoPoll(m_vnr, ref result, telegram);
            }
            if (succes)
            {
                if (id != null)
                {
                    m_idToFilename[telegram.AktId] = id;
                    if (id != "live")
                        m_acknowledgements.Remove(id);
                    SetMessage("message sent: id " + id);
                }
                else
                    SetMessage("message sent: id " + telegram.AktId.ToString());
                return true;
            }
            else
            {
                if (result == CH1Manager.H1Result.WAIT_CONNECT)
                {
                    //m_pcConnected = ConnectionState.DISCONNECTED;
                    //m_retryConnect = true;
                    SetMessage("noticed disconnected while trying to send message with id:" + ((id == null)?"":id.ToString()));
                }
                else
                    SetMessage("error sending message with id" + ((id == null) ? "" : id.ToString()) + ": " + m_h1manager.LastError ?? "");
                return false;
            }
        }

        private bool ReadMessage()
        {
            if (m_pcConnected == ConnectionState.DISCONNECTED)
                return true;
            CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
            
            if (m_readStarted)
                m_h1manager.GetReadStatus(m_vnr, ref result);
            else
                m_h1manager.StartRead(m_vnr, ref result);
            switch (result)
            {
                case CH1Manager.H1Result.ALL_CLEAR:
                    WrapperTelegram wrap = new WrapperTelegram();
                    if (m_h1manager.FinishRead(m_vnr, wrap))
                    {
                        AckTelegram ack = wrap.InnerTelegram as AckTelegram;
                        if (ack != null && m_idToFilename.Contains(ack.AktId))
                        {
                            string id = m_idToFilename[ack.AktId];
                            if (id == "init")
                            {
                                m_pcConnected = ConnectionState.READY;
                                m_acknowledgements["live"] = false;
                                //live message zenden
                                SendTelegram(new LiveTelegram(m_idCounter, m_messagesCount++), "live");
                                m_idCounter += 2;
                                m_liveTimer.Change(TimeSpan.FromMinutes(1.0), TimeSpan.Zero);
                                SetMessage("acknowledgement to init recieved");
                                SetMessage("Please press te GO button");
                            }
                            else if (id == "live")
                            {
                                m_acknowledgements[id] = true;
                                SetMessage("acknowledgement to live recieved");
                            }
                            else if (id == "go")
                            {
                                m_acknowledgements[id] = true;
                                SetMessage("acknowledgement to go recieved");
                            }
                            else
                            {
                                m_acknowledgements[id] = true;
                                SetMessage("acknowledgement to message with id \"" + id + "\" received");
                                //m_waitSendEvent.Set();
                            }
                        }
                        else
                        {
                            QdtTelegram qdt = wrap.InnerTelegram as QdtTelegram;
                            if (qdt != null)
                            {
                                string filename = "qdt" + qdt.AktId.ToString() + ".txt";
                                filename = Path.Combine(m_outputDir.Text, filename);
                                using (StreamWriter w = File.AppendText(filename))
                                {
                                    w.Write(qdt.Interpret(m_telegrams));
                                    w.Close();
                                }
                                AckTelegram ackqdt = new AckTelegram(qdt.AktId, m_messagesCount++);
                                SetMessage("qdt telegram recieved, written to " + filename);
                                if (!SendTelegram(ackqdt))
                                {
                                    m_pcConnected = ConnectionState.DISCONNECTED;
                                    m_retryConnect = true;
                                }
                            }
                            else
                            {
                                IniTelegram ini = wrap.InnerTelegram as IniTelegram;
                                if (ini != null)
                                {
                                    m_pcConnected = m_pcConnected == ConnectionState.READY ? ConnectionState.READY:ConnectionState.CONNECTED;
                                    AckTelegram ackini = new AckTelegram(ini.AktId, m_messagesCount++);
                                    SetMessage("ini telegram recieved, please send a GO");
                                    if (!SendTelegram(ackini,"init_pc"))
                                    {
                                        m_pcConnected = ConnectionState.DISCONNECTED;
                                        m_retryConnect = true;
                                    }
                                }
                                else
                                {
                                    LiveTelegram live = wrap.InnerTelegram as LiveTelegram;
                                    if (live != null)
                                    {
                                        SetMessage("live telegram recieved");
                                        AckTelegram acklive = new AckTelegram(live.AktId, m_messagesCount++);
                                        if (!SendTelegram(acklive))
                                        {
                                            m_pcConnected = ConnectionState.DISCONNECTED;
                                            m_retryConnect = true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else return false;
                    return true;
                case CH1Manager.H1Result.NO_REQUEST:
                    m_readStarted = false;
                    return true;
                case CH1Manager.H1Result.WAIT_CONNECT:
                    m_readStarted = false;
                    //m_pcConnected = ConnectionState.DISCONNECTED;
                    //m_retryConnect = true;
                    return true;
                case CH1Manager.H1Result.ALREADY_RUNNING:
                case CH1Manager.H1Result.WAIT_DATA:
                    m_readStarted = true;
                    return true;
                case CH1Manager.H1Result.BLOCKED_DATA:
                    m_readStarted = true;
                    m_h1manager.StoreBlockedBytes(m_vnr);
                    return true;
                default: // error
                    return false;
            }
        }

        private void BrowseButton_Click(object sender, EventArgs e)
        {
            m_folderBrowser.ShowNewFolderButton = true;
            DialogResult result = m_folderBrowser.ShowDialog();
            if (result == DialogResult.OK)
                m_outputDir.Text = m_folderBrowser.SelectedPath;
        }
    }
}