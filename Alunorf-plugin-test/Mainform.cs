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
using iba.Utility;
using iba.Logging;
using iba.Logging.Loggers;

namespace Alunorf_plugin_test
{
    public partial class MainForm : Form
    {
        bool m_readStarted = false;
        private FileLogger m_logger;
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
            m_logfile.Text = Path.Combine(Directory.GetCurrentDirectory(),"logfile.txt");
            m_otherTSAP.Text = "LT4NQI";
            m_ownTSAP.Text = "BR 1";
            temp = new MacAddr();
            temp.FirstByte  = 0x00;
            temp.SecondByte = 0x15;
            temp.ThirdByte  = 0xBA;
            temp.FourthByte = 0x00;
            temp.FifthByte  = 0x03;
            temp.SixthByte  = 0x7A;
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
            //ushort id = 0;
            if (!m_idToFilename.Contains("init_pc") && m_idToFilename.Contains("init"))
                SetMessage("go while there is no init or previous go");

            GoTelegram go = new GoTelegram(m_idCounter, m_messagesCount++);
            m_idToFilename[m_idCounter] = "go";
            m_idCounter += 2;
            lock (m_messageQueue)
            {
                m_messageQueue.Add(go);
            }
            SetMessage("go added to messagequeue");

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
            m_sendFurtherGoes = true;
        }

        private void m_btStop_Click(object sender, EventArgs e)
        {
            m_stop = true;
            m_thread.Join(60000);
            m_btStop.Enabled = false;
            m_btStart.Enabled = true;
            m_btGO.Enabled = false;
            m_logger.Close();
        }

        private bool m_sendFurtherGoes;

        private void m_btStart_Click(object sender, EventArgs e)
        {
            m_thread = new Thread(new ThreadStart(Run));
            //m_thread.SetApartmentState(ApartmentState.STA);
            string filename = m_logfile.Text;
            FileBackup.Backup(filename, Path.GetDirectoryName(filename), "logfile", 10);
            m_stop = false;
            m_logger = Logger.CreateFileLogger(filename, "{ts}\t{ln}\t{msg}");
            m_logger.IsBufferingEnabled = false;
            m_logger.IsContextEnabled = true;
            m_logger.AutoFlushInterval = 1000;
            m_logger.BufferSize = 1000;
            m_logger.Level = Level.All;
            m_logger.MakeDailyArchive = true;
            m_logger.DailyString = "ibaDatCoordinator v" + DatCoVersion.GetClientVersion() + "\r\n";
            m_logger.MaximumArchiveFiles = 10;
            //m_logger.EventFormatter.DataFormatter = new LogExtraDataFormatter(m_idToFilename);
            m_logger.Open();


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
                        if (m_messages.Rows.Count > (int)(m_nudMaxgrid.Value))
                            m_messages.Rows.Remove(m_messages.Rows[0]);

                        m_messages.Rows.Add();
                        m_messages.Rows[m_messages.RowCount - 1].Cells[0].Value = DateTime.Now.ToString();
                        m_messages.Rows[m_messages.RowCount - 1].Cells[1].Value = message;
                    }

                    if (m_logger != null && m_logger.IsOpen)
                        m_logger.Log(Level.Info, message);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
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
                    MessageBox.Show("exception: " + ex.ToString());
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
            SetMessage("started");
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
                        m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);
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
                if (m_sendLive)
                {
                    SendTelegram(new LiveTelegram(m_idCounter, m_messagesCount++), "live");
                    m_idCounter += 2;
                    m_sendLive = false;
                }
                Thread.Sleep(100);
            }
            m_h1manager.DisconnectAll();
            m_pcConnected = ConnectionState.DISCONNECTED;
            SetMessage("stopped");
        }

        private bool m_sendLive;

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
                            SetMessage("state is connected");
                            //send INI telegram
                            //IniTelegram init = new IniTelegram(m_idCounter, m_messagesCount++);
                            //m_idCounter += 2;
                            //SendTelegram(init, "init");
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
                            //IniTelegram init = new IniTelegram(m_idCounter, m_messagesCount++);
                            //m_idCounter += 2;
                            //SendTelegram(init, "init");
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
                    SetMessage("Live message not acknowledged, state is disconnected");
                    return;
                }
                m_acknowledgements["live"] = false;
                m_sendLive = true;
                m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);
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
            int i = 0;
            for (; result == CH1Manager.H1Result.WAIT_CONNECT && i < 10; i++ )
            {
                SetMessage("message about to be sent: id " + ((id == null) ? "" : id) + " " + telegram.AktId.ToString() + telegram.GetType().ToString());
                succes = m_h1manager.SendNoPoll(m_vnr, ref result, telegram);
                Thread.Sleep(500);
            }
            if (succes && i < 10)
            {
                if (id != null)
                {
                    m_idToFilename[telegram.AktId] = id;
                    if (id != "live")
                        m_acknowledgements.Remove(id);
                    SetMessage("message sent: id " + id  + " " + telegram.AktId.ToString());
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
                                m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);
                                SetMessage("acknowledgement to init recieved, state is ready: " + ack.AktId.ToString());
                                SetMessage("Please press te GO button: " + ack.AktId.ToString());
                            }
                            else if (id == "live")
                            {
                                m_acknowledgements[id] = true;
                                SetMessage("acknowledgement to live recieved: " + ack.AktId.ToString());
                            }
                            else if (id == "go")
                            {
                                m_acknowledgements[id] = true;
                                SetMessage("acknowledgement to go recieved: " + ack.AktId.ToString());
                            }
                            else
                            {
                                m_acknowledgements[id] = true;
                                SetMessage("acknowledgement to message with id \"" + id + "\" received: " + ack.AktId.ToString());
                                //m_waitSendEvent.Set();
                            }
                        }
                        else
                        {
                            QdtTelegram qdt = wrap.InnerTelegram as QdtTelegram;
                            try
                            {
                                if (qdt != null)
                                {
                                    //                                string filename = "qdt" + qdt.AktId.ToString() + ".txt";
                                    string filename = "qdt" + qdt.AktId.ToString() + ".dat";
                                    filename = Path.Combine(m_outputDir.Text, filename);
                                    //using (StreamWriter w = File.AppendText(filename))
                                    //{
                                    //    w.Write(qdt.Interpret(m_telegrams));
                                    //    w.Close();
                                    //}
                                    qdt.WriteToDatFile(m_telegrams, filename);
                                    AckTelegram ackqdt = new AckTelegram(qdt.AktId, m_messagesCount++);
                                    SetMessage("qdt telegram recieved: " + qdt.AktId.ToString() + ", written to " + filename);
                                    if (!SendTelegram(ackqdt))
                                    {
                                        SetMessage("could not set ack to qdt: state is disconnected");
                                        m_pcConnected = ConnectionState.DISCONNECTED;
                                        m_retryConnect = true;
                                    }
                                }
                                else
                                {
                                    IniTelegram ini = wrap.InnerTelegram as IniTelegram;
                                    if (ini != null)
                                    {
                                        m_pcConnected = ConnectionState.READY;
                                        AckTelegram ackini = new AckTelegram(ini.AktId, m_messagesCount++);
                                        SetMessage("ini telegram recieved: " + ini.AktId.ToString() + ", please send a GO");
                                        if (!SendTelegram(ackini, "init_pc"))
                                        {
                                            SetMessage("could not send ack to ini: state is disconnected");
                                            m_pcConnected = ConnectionState.DISCONNECTED;
                                            m_retryConnect = true;
                                        }
                                        if (m_sendFurtherGoes)
                                        {
                                            GoTelegram go = new GoTelegram(m_idCounter, m_messagesCount++);
                                            m_idCounter += 2;
                                            if (!SendTelegram(go, "go"))
                                            {
                                                SetMessage("could not send a further go: state is disconnected");
                                                m_pcConnected = ConnectionState.DISCONNECTED;
                                                m_retryConnect = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LiveTelegram live = wrap.InnerTelegram as LiveTelegram;
                                        if (live != null)
                                        {
                                            SetMessage("live telegram recieved: " + live.AktId.ToString());
                                            AckTelegram acklive = new AckTelegram(live.AktId, m_messagesCount++);
                                            if (!SendTelegram(acklive))
                                            {
                                                SetMessage("could not set ack to live: state is disconnected");
                                                m_pcConnected = ConnectionState.DISCONNECTED;
                                                m_retryConnect = true;
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show(ex.ToString());
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

        private void BrowseButton2_Click(object sender, EventArgs e)
        {
            m_openFileDialog1.CheckFileExists = true;
            m_openFileDialog1.FileName = "logfile.txt";
            m_openFileDialog1.Filter = "Text files (*.txt)|*.txt|All files (*.*)|*.*";
            if (m_openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                m_logfile.Text = m_openFileDialog1.FileName;
            }
        }
    }
}