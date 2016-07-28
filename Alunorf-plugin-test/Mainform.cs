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
using ibaFilesProLib;

namespace Alunorf_plugin_test
{
    public partial class MainForm : Form
    {
        bool m_readStarted = false;
        private FileLogger m_logger;
        private object m_counterLock;
        public MainForm()
        {
            m_counterLock = new object();
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

            m_idToFilename = new BiMap<int,string>();
            m_acknowledgements = new SortedDictionary<string, bool>();
            m_reconnectTimer = new System.Threading.Timer(OnReconnectTimerTick);
            m_liveTimer = new System.Threading.Timer(OnLiveTimerTick);
        }

        private void m_btGO_Click(object sender, EventArgs e)
        {
            //ushort id = 0;
            lock (m_idToFilename)
            {
                if (!m_idToFilename.Contains("init_pc") && m_idToFilename.Contains("init"))
                    SetMessage("go while there is no init or previous go");
            }
            UInt16 id = 0;
            lock (m_counterLock)
            {
                id = m_idCounter;
                m_idCounter++;
            }
            GoTelegram go = new GoTelegram(id, m_messagesCount++);
            lock (m_idToFilename)
            {
                m_idToFilename[id] = "go";
            }
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
            //m_sendFurtherGoes = true;
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

        //private bool m_sendFurtherGoes;

        private void m_btStart_Click(object sender, EventArgs e)
        {
            m_thread = new Thread(new ThreadStart(Run));
            m_thread.SetApartmentState(ApartmentState.STA);
            string filename = m_logfile.Text;

            m_stop = false;
            if (m_logger != null)
            {
                m_logger.Close();
            }

            m_logger = Logger.CreateFileLogger(filename, "{ts}\t{ln}\t{msg}");
            m_logger.IsBufferingEnabled = false;
            m_logger.IsContextEnabled = true;
            m_logger.AutoFlushInterval = 1000;
            m_logger.BufferSize = 1000;
            m_logger.Level = Level.All;
            m_logger.MakeDailyArchive = true;
            m_logger.DailyString = "ibaDatCoordinator v";// + DatCoVersion.GetClientVersion() + "\r\n";
            m_logger.MaximumArchiveFiles = 10;
            //m_logger.EventFormatter.DataFormatter = new LogExtraDataFormatter(m_idToFilename);
            m_logger.Open();


            if (this.doTCPIP)
            {
                TcpIPManager ipmanager = new TcpIPManager();
                m_tcpManager = ipmanager;
                if (!m_tcpManager.SetSendTimeout((int)m_nudSendTimeout.Value))
                {
                    m_stop = true;
                    SetMessage("could not set send time out:" + m_tcpManager.LastError);
                    return;
                }
            }
            else
            {
                CH1Manager h1manager = new CH1Manager();
                m_tcpManager = h1manager;
                MacAddr temp = new MacAddr();
                temp.Address = m_ownMAC.Text;
                byte[] ownMAC = new byte[] { temp.FirstByte, temp.SecondByte, temp.ThirdByte, temp.FourthByte, temp.FifthByte, temp.SixthByte };
                if (!h1manager.SetStationAddress(ownMAC))
                {
                    m_stop = true;
                    SetMessage("could not set station adress:" + m_tcpManager.LastError);
                    return;
                }
                if (!h1manager.SetSendTimeout((int)m_nudSendTimeout.Value))
                {
                    m_stop = true;
                    SetMessage("could not set send time out:" + m_tcpManager.LastError);
                    return;
                }

                SetMessage("station address and timeout set");
            }
            m_thread.IsBackground = true;
            m_thread.Name = "workerthread for testprogram: ";
            m_retryConnect = true;
            m_thread.Start();
            m_btStart.Enabled = false;

            SetMessage("gui thread: waiting on connect");

            m_btStop.Enabled = true;
            m_btGO.Enabled = true;
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

        protected override void OnClosing(CancelEventArgs e)
        {
            if (m_logger != null)
            {
                m_logger.Close();
            }
            base.OnClosing(e);
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
        private IProtocolManager m_tcpManager;
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
                        string key;
                        lock (m_idToFilename)
                        {
                            key = m_idToFilename[m_messageQueue[0].AktId];
                        }
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
                    lock (m_counterLock)
                    {
                        SendTelegram(new LiveTelegram(m_idCounter, m_messagesCount++), "live");
                        m_idCounter += 2;
                        m_sendLive = false;
                    }
                }
                Thread.Sleep(100);
            }
            m_tcpManager.DisconnectAll();
            m_pcConnected = ConnectionState.DISCONNECTED;
            SetMessage("stopped");
        }

        private bool m_sendLive;

        private void BuildConnection()
        {
            m_retryConnect = false;
            if (m_pcConnected == ConnectionState.DISCONNECTED)
            {
                H1Result result = H1Result.ALL_CLEAR;
                m_tcpManager.GetConnectionStatus(m_vnr, ref result);
                switch (result)
                {
                    case H1Result.ALL_CLEAR:
                        {
                            m_pcConnected = ConnectionState.CONNECTED;
                            SetMessage("state is connected");
                            //send INI telegram
                            //IniTelegram init = new IniTelegram(m_idCounter, m_messagesCount++);
                            //m_idCounter += 2;
                            //SendTelegram(init, "init");
                            break;
                        }
                    case H1Result.WAIT_CONNECT:
                        m_retryConnect = true;
                        break;
                    case H1Result.BAD_LINE:
                        MacAddr temp = new MacAddr();
                        temp.Address = m_otherMAC.Text;
                        byte[] otherMAC = new byte[] { temp.FirstByte, temp.SecondByte, temp.ThirdByte, temp.FourthByte, temp.FifthByte, temp.SixthByte };
                        SetMessage("worker thread: trying to connect");

                        bool connected = false;
                        if (doTCPIP)
                        {
                            m_vnr = ushort.Parse(m_tcpipPort.Text);
                            result = (m_tcpManager as TcpIPManager).ActiveConnect((int)m_vnr, m_tcpIPHost.Text,  (int)m_nudTryconnectTimeout.Value);
                            connected = result == H1Result.ALL_CLEAR;
                        }
                        else
                        {
                            connected = (m_tcpManager as CH1Manager).Connect(ref m_vnr, 4, true, otherMAC, m_ownTSAP.Text, m_otherTSAP.Text, ref result, (int)m_nudTryconnectTimeout.Value);
                        }

                        if (connected)
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
                            SetMessage(m_tcpManager.LastError);
                            m_retryConnect = true;
                        }
                        break;
                    default:
                        SetMessage(m_tcpManager.LastError);
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
            H1Result result = H1Result.WAIT_CONNECT;
            int i = 0;
            for (; result == H1Result.WAIT_CONNECT && i < 10; i++ )
            {
                SetMessage("message about to be sent: id " + ((id == null) ? "" : id) + " " + telegram.AktId.ToString() + telegram.GetType().ToString());
                succes = m_tcpManager.SendNoPoll(m_vnr, ref result, telegram);
                Thread.Sleep(500);
            }
            if (succes && i < 10)
            {
                if (id != null)
                {
                    lock (m_idToFilename)
                    {
                        m_idToFilename[telegram.AktId] = id;
                    }
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
                if (result == H1Result.WAIT_CONNECT)
                {
                    //m_pcConnected = ConnectionState.DISCONNECTED;
                    //m_retryConnect = true;
                    SetMessage("noticed disconnected while trying to send message with id:" + ((id == null) ? "" : id.ToString()));
                }
                else
                {
                    SetMessage("error sending message with id" + ((id == null) ? "" : id.ToString()) + ": " + m_tcpManager.LastError ?? "");
                    m_retryConnect = true;
                }
                return false;
            }
        }

        private bool ReadMessage()
        {
            if (m_pcConnected == ConnectionState.DISCONNECTED)
                return true;
            H1Result result = H1Result.ALL_CLEAR;
            
            if (m_readStarted)
                m_tcpManager.GetReadStatus(m_vnr, ref result);
            else
                m_tcpManager.StartRead(m_vnr, ref result);
            switch (result)
            {
                case H1Result.ALL_CLEAR:
                    ITelegram it = new WrapperTelegram();
                    if (m_tcpManager.FinishRead(m_vnr, ref it))
                    {
                        WrapperTelegram wrap = it as WrapperTelegram;
                        AckTelegram ack = wrap.InnerTelegram as AckTelegram;
                        if (ack != null && m_idToFilename.Contains(ack.AktId))
                        {
                            string id = m_idToFilename[ack.AktId];
                            if (id == "init")
                            {
                                m_pcConnected = ConnectionState.READY;
                                m_acknowledgements["live"] = false;
                                //live message zenden
                                lock (m_counterLock)
                                {
                                    SendTelegram(new LiveTelegram(m_idCounter, m_messagesCount++), "live");
                                    m_idCounter += 2;
                                }
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
                                    WriteToDatFile(qdt, m_telegrams, filename);
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
                                        //if (m_sendFurtherGoes)
                                        //{
                                        //    UInt16 id = 0;
                                        //    lock (m_counterLock)
                                        //    {
                                        //        id = m_idCounter;
                                        //        m_idCounter += 2;
                                        //    }
                                        //    GoTelegram go = new GoTelegram(id, m_messagesCount++);
                                        //    if (!SendTelegram(go, "go"))
                                        //    {
                                        //        SetMessage("could not send a further go: state is disconnected");
                                        //        m_pcConnected = ConnectionState.DISCONNECTED;
                                        //        m_retryConnect = true;
                                        //    }
                                        //}
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
                case H1Result.NO_REQUEST:
                    m_readStarted = false;
                    return true;
                case H1Result.WAIT_CONNECT:
                    m_readStarted = false;
                    //m_pcConnected = ConnectionState.DISCONNECTED;
                    //m_retryConnect = true;
                    return true;
                case H1Result.ALREADY_RUNNING:
                case H1Result.WAIT_DATA:
                    m_readStarted = true;
                    return true;
                case H1Result.BLOCKED_DATA:
                    m_readStarted = true;
                    m_tcpManager.StoreBlockedBytes(m_vnr);
                    return true;
                default: // error
                    SetMessage("Read error: " + m_tcpManager.LastError);
                    m_pcConnected = ConnectionState.DISCONNECTED;
                    m_retryConnect = true;
                    return true;
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

        bool doTCPIP;

        private void m_tcpIPHost_TextChanged(object sender, EventArgs e)
        {
            doTCPIP = !String.IsNullOrEmpty((m_tcpIPHost.Text));
            m_ownMAC.Enabled = !doTCPIP;
            m_ownTSAP.Enabled = !doTCPIP;
            m_otherMAC.Enabled = !doTCPIP;
            m_otherTSAP.Enabled = !doTCPIP;
        }

        public void WriteToDatFile(QdtTelegram qdt, List<PluginH1Task.Telegram> telegrams, string filename)
        { //to test the QDT telegram, reading has to implemented at well, by writing the values to a string
            IbaFileWriter filewriter = new IbaFileCreatorClass();
            filewriter.Create(filename, 1);
            int errPos = 0;

            SByte v_sbyte = 0;
            Byte v_byte = 0;
            Int16 v_int16 = 0;
            Int32 v_int32 = 0;
            string v_string = "";
            UInt16 v_uint16 = 0;
            UInt32 v_uint32 = 0;
            float v_float = 0;
            qdt.Stream.ReadInt16(ref v_int16);
            qdt.TelegramData = telegrams.Find(delegate(PluginH1Task.Telegram tele) { return tele.QdtType == v_int16; });
            int count = 0;
            foreach (PluginH1Task.TelegramRecord rec in qdt.TelegramData.DataInfo)
            {
                count++;
                try
                {
                    int pos = rec.DataType.IndexOfAny(new char[] { '1', '2', '3', '4', '5', '6', '7', '8', '9' });
                    uint size = uint.Parse(rec.DataType.Substring(pos));
                    string type = rec.DataType.Substring(0, pos);
                    if (type == "int" && size == 1)
                    {
                        qdt.Stream.ReadSByte(ref v_sbyte);
                        filewriter.WriteInfoField(rec.Name, v_sbyte.ToString());
                    }
                    else if (type == "int" && size == 2)
                    {
                        qdt.Stream.ReadInt16(ref v_int16);
                        filewriter.WriteInfoField(rec.Name, v_int16.ToString());
                    }
                    else if (type == "int" && size == 4)
                    {
                        qdt.Stream.ReadInt32(ref v_int32);
                        filewriter.WriteInfoField(rec.Name, v_int32.ToString());
                    }
                    else if (type == "u. int" && size == 1)
                    {
                        qdt.Stream.ReadByte(ref v_byte);
                        filewriter.WriteInfoField(rec.Name, v_byte.ToString());
                    }
                    else if (type == "u. int" && size == 2)
                    {
                        qdt.Stream.ReadUInt16(ref v_uint16);
                        filewriter.WriteInfoField(rec.Name, v_uint16.ToString());
                    }
                    else if (type == "u. int" && size == 4)
                    {
                        qdt.Stream.ReadUInt32(ref v_uint32);
                        filewriter.WriteInfoField(rec.Name, v_uint32.ToString());
                    }
                    else if (type == "float" && size == 4)
                    {
                        qdt.Stream.ReadFloat32(ref v_float);
                        filewriter.WriteInfoField(rec.Name, v_float.ToString());
                    }
                    else if (type == "char" && size > 0)
                    {
                        qdt.Stream.ReadString(ref v_string, size);
                        filewriter.WriteInfoField(rec.Name, v_string);
                    }
                    else
                    {
                        errPos = count;
                        filewriter.Close();
                        throw new Exception("Error reading infofield at pos " + errPos.ToString());
                    }
                }
                catch (Exception inner)
                {
                    errPos = count;
                    filewriter.Close();
                    throw new Exception("Error reading infofield at pos " + errPos.ToString(), inner);
                }
            }

            count = 0;
            foreach (PluginH1Task.TelegramRecord rec in qdt.TelegramData.DataSignal)
            {
                count++;
                try
                {
                    float[] vals = new float[400];
                    for (int i = 0; i < 400; i++)
                    {
                        qdt.Stream.ReadFloat32(ref v_float);
                        vals[i] = v_float;
                    }
                    float offset = 0;
                    float lengthbase = 0;
                    for (int i = 0; i < 400; i++)
                    {
                        qdt.Stream.ReadFloat32(ref v_float);
                        if (i == 0) offset = v_float;
                        if (i == 1) lengthbase = v_float - offset;
                    }
                    IbaChannelWriter channelwriter = filewriter.CreateAnalogChannel(0, count, rec.Name, lengthbase) as IbaChannelWriter;
                    channelwriter.LengthBased = 1;
                    channelwriter.xOffset = offset;
                    for (int i = 0; i < 400; i++) channelwriter.WriteData(vals[i]);
                }
                catch (Exception inner)
                {
                    errPos = count;
                    throw new Exception("Error reading signal at pos " + errPos.ToString(), inner);
                }
            }
            if (count == 0) //when no signals, write a dummy one, others ibaFiles won't write the file
            {
                IbaChannelWriter channelwriter = filewriter.CreateAnalogChannel(0, 0, "dummysignal", 1) as IbaChannelWriter;
                channelwriter.LengthBased = 1;
                channelwriter.xOffset = 0;
                channelwriter.WriteData(1.0f);
                channelwriter.WriteData(2.0f);
                channelwriter.WriteData(3.0f);
            }
            try
            {
                filewriter.Close();
            }
            catch (Exception)
            {

            }
        }

    }
}