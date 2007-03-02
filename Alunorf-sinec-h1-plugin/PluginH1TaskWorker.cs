using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.IO;

using iba;
using iba.Plugins;
using IBAFILESLib;
using iba.Logging;
using iba.Logging.Loggers;
using iba.Utility;

namespace Alunorf_sinec_h1_plugin
{
   

    [Serializable]
    public class NqsServerStatusses
    {
        public PluginH1TaskWorker.NQSStatus nqs1;
        public PluginH1TaskWorker.NQSStatus nqs2;
    }

    public class PluginH1TaskWorker : IPluginTaskWorker
    {
        private const int THREADSLEEPTIME = 100; // sleep time between different receive/send cycles
        private const int DISCONNECTIONCOUNTMAX = 500; //How many read operations can fail
        //so: total time before considering disconnected: THREADSLEEPTIME*DISCONNECTIONCOUNTMAX

        #region IPluginTaskWorker Members

        private bool m_readStarted1;
        private bool m_readStarted2;

        private FileLogger m_logger;

        //public class LogExtraDataFormatter : DataFormatter
        //{
        //    BiMap<int, string> m_idToFilename;

        //    public LogExtraDataFormatter(BiMap<int, string> dict)
        //    {
        //        m_idToFilename = dict;
        //    }

        //    public override string Format(object o)
        //    {
        //        StringBuilder sb = new StringBuilder();
        //        Format(o,sb);
        //        return sb.ToString();
        //    }

        //    public override void Format(object o, StringBuilder sb)
        //    {
        //        if (o == null) return;
        //        else if (o is int && m_idToFilename.Contains((int) o))
        //        {
        //            sb.Append("id: ");
        //            sb.Append((int) o);
        //            sb.Append(", ");
        //            sb.Append(m_idToFilename[(int) o]);
        //        }
        //        else if (o is string && m_idToFilename.Contains(o as string))
        //        {
        //            sb.Append("id: ");
        //            sb.Append(m_idToFilename[o as string]);
        //            sb.Append(", ");
        //            sb.Append(o as string);
        //        }
        //    }

        //    public override DataFormatter Copy()
        //    {
        //        return new LogExtraDataFormatter(m_idToFilename);
        //    }
        //}

        private Object m_idCounterLock;

        public bool OnStart()
        {
            //set the logger;
            string directory = Path.Combine(Path.GetDirectoryName(typeof(PluginH1Task).Assembly.Location), "logfiles");
            if (!Directory.Exists(directory))
                Directory.CreateDirectory(directory);
            string filename = Path.Combine(directory, "sinec-h1.log");
            
            if (m_logger != null && m_logger.IsOpen)
                m_logger.Close();

            FileBackup.Backup(filename, Path.GetDirectoryName(filename), "ibaDatCoordinatorLog", 10);

            m_logger = Logger.CreateFileLogger(filename, "{ts}\t{ln}\t{msg}\t{data}");
            m_logger.IsBufferingEnabled = false;
            m_logger.IsContextEnabled = true;
            m_logger.AutoFlushInterval = 1000;
            m_logger.BufferSize = 1000;
            m_logger.Level = Level.All;
            m_logger.MakeDailyArchive = true;
            m_logger.MaximumArchiveFiles = 10;
            //m_logger.EventFormatter.DataFormatter = new LogExtraDataFormatter(m_idToFilename);
            m_readStarted1 = m_readStarted2 = false;
            m_logger.Open();

            bool ok = m_h1manager.SetStationAddress(m_data.OwnAddress);
            if (ok)
                ok = m_h1manager.SetSendTimeout(m_data.SendTimeOut);
            if (!ok)
            {
                m_started = false;
                m_error = m_h1manager.LastError;
                return false;
            }
            
            m_retryConnect = true;
            m_thread = new Thread(new ThreadStart(Run));
            //m_thread.SetApartmentState(ApartmentState.STA);
            m_thread.IsBackground = true;
            m_thread.Name = "workerthread for: " + m_data.NameInfo;
            m_started = true;
            InitMemberData();
            m_thread.Start();
            
            //wait here until initialisation succeeded
            if (!m_stop)
            {
                m_waitConnectedEvent = new AutoResetEvent(false);
                m_waitConnectedEvent.WaitOne(2000 * (m_data.SendTimeOut + m_data.AckTimeOut + m_data.ConnectionTimeOut + 30), true);
            }
            else
                return true;

            if ((m_nqs1Status == NQSStatus.GO && m_nqs2Status != NQSStatus.DISCONNECTED) || (m_nqs2Status == NQSStatus.GO && m_nqs1Status != NQSStatus.DISCONNECTED))
                return true;
            else if (m_nqs2Status == NQSStatus.GO) 
            {
                if (m_error == null) m_error = "";
                m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_connect1 + m_error;
            }
            else if (m_nqs1Status == NQSStatus.GO)
            {
                if (m_error == null) m_error = "";
                m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_connect2 + m_error;
            }
            else
            {
                if (m_error == null) m_error = "";
                m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_connect3 + m_error;
            }
            return false;
        }

        public bool OnStop()
        {
            m_stop = true;
            lock (m_messageQueue)
            {
                foreach (Message telegram in m_messageQueue)
                {
                    QdtTelegram qdt = telegram.telegram as QdtTelegram;
                    if (qdt != null) qdt.Dispose();
                }
                m_messageQueue.Clear();
            }
            if (m_logger != null && m_logger.IsOpen)
            {
                m_logger.Close();
                m_logger = null;
            }
            if (m_started == false)
            {
                return true;
            }
            else
            {

                //signal any waiting items
                if (m_waitSendEvent != null) m_waitSendEvent.Set();
                if (m_waitConnectedEvent != null) m_waitConnectedEvent.Set();
                return m_thread.Join(30000);
            }
        }

        public bool OnApply(IPluginTaskData newtask, IJobData newParentJob)
        {
            PluginH1Task data = newtask as PluginH1Task;

            bool ok = true;

            if (m_data.SendTimeOut != data.SendTimeOut)
            {
                ok = m_h1manager.SetSendTimeout(data.SendTimeOut);
                if (!ok)
                {
                    m_stop = true;
                    m_error = m_h1manager.LastError;
                    return false;
                }
            }
            if (data.NQS_TSAPforNQS1 != data.NQS_TSAPforNQS1
                || m_data.NQS_TSAPforNQS2 != data.NQS_TSAPforNQS2
                || m_data.OwnTSAPforNQS1 != data.OwnTSAPforNQS1
                || m_data.OwnTSAPforNQS2 != data.OwnTSAPforNQS2
                || m_data.NQSAddress1[0] != data.NQSAddress1[0]
                || m_data.NQSAddress1[1] != data.NQSAddress1[1]
                || m_data.NQSAddress1[2] != data.NQSAddress1[2]
                || m_data.NQSAddress1[3] != data.NQSAddress1[3]
                || m_data.NQSAddress1[4] != data.NQSAddress1[4]
                || m_data.NQSAddress1[5] != data.NQSAddress1[5]
                || m_data.NQSAddress2[0] != data.NQSAddress2[0]
                || m_data.NQSAddress2[1] != data.NQSAddress2[1]
                || m_data.NQSAddress2[2] != data.NQSAddress2[2]
                || m_data.NQSAddress2[3] != data.NQSAddress2[3]
                || m_data.NQSAddress2[4] != data.NQSAddress2[4]
                || m_data.NQSAddress2[5] != data.NQSAddress2[5]
                || m_data.OwnAddress[0] != data.OwnAddress[0]
                || m_data.OwnAddress[1] != data.OwnAddress[1]
                || m_data.OwnAddress[2] != data.OwnAddress[2]
                || m_data.OwnAddress[3] != data.OwnAddress[3]
                || m_data.OwnAddress[4] != data.OwnAddress[4]
                || m_data.OwnAddress[5] != data.OwnAddress[5])
            {
                m_h1manager.DisconnectAll();
                m_nqs1Status = m_nqs2Status = NQSStatus.DISCONNECTED;
                m_retryConnect = true;

                if (m_data.OwnAddress[0] != data.OwnAddress[0]
                || m_data.OwnAddress[1] != data.OwnAddress[1]
                || m_data.OwnAddress[2] != data.OwnAddress[2]
                || m_data.OwnAddress[3] != data.OwnAddress[3]
                || m_data.OwnAddress[4] != data.OwnAddress[4]
                || m_data.OwnAddress[5] != data.OwnAddress[5])
                {
                    if (!m_h1manager.SetStationAddress(m_data.OwnAddress))
                    {
                        m_stop = true;
                        m_error = m_h1manager.LastError;
                        return false;
                    }
                }
            }

            bool telsAreEqual = true;
            if (data.Telegrams.Count == m_data.Telegrams.Count)
            {
                for (int i = 0; i < data.Telegrams.Count && telsAreEqual; i++)
                {
                    if (data.Telegrams[i].DataInfo.Count != m_data.Telegrams[i].DataInfo.Count
                        || data.Telegrams[i].DataSignal.Count != m_data.Telegrams[i].DataSignal.Count
                        || data.Telegrams[i].QdtType != m_data.Telegrams[i].QdtType)
                        telsAreEqual = false;
                    else
                    {
                        for (int j = 0; j < data.Telegrams[i].DataInfo.Count && telsAreEqual;j++ )
                        {
                            telsAreEqual = telsAreEqual && data.Telegrams[i].DataInfo[j].DataType == m_data.Telegrams[i].DataInfo[j].DataType
                            && data.Telegrams[i].DataInfo[j].Name == m_data.Telegrams[i].DataInfo[j].Name;
                        }
                        for (int j = 0; j < data.Telegrams[i].DataSignal.Count && telsAreEqual; j++)
                        {
                            telsAreEqual = telsAreEqual && data.Telegrams[i].DataSignal[j].DataType == m_data.Telegrams[i].DataSignal[j].DataType
                            && data.Telegrams[i].DataSignal[j].Name == m_data.Telegrams[i].DataSignal[j].Name;
                        }
                    }
                }
            }
            else telsAreEqual = false;

            if (!telsAreEqual) // cleanup back log
            {
                m_idToFilename.Clear();
                m_acknowledgements.Clear();
                m_nak_errors.Clear();
            }
            m_data = data;
            return true;
        }

        public bool ExecuteTask(string datFile)
        {
            IbaFileReader reader = new IbaFileClass();
            try
            {
                reader.Open(datFile);
            }
            catch
            {
                reader.Close();
                m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_no_open;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(reader);
                return false;
            }

            List<short> UnTimelyAcknowledged = new List<short>();
            List<string> NAKnowledged = new List<string>();
            List<string> ErrorTelegram = new List<string>();
            List<string> ErrorSendTelegram = new List<string>();

            int count = 0;
            foreach (PluginH1Task.Telegram telegram in m_data.Telegrams)
            {
                count++;
                string key = datFile + "#" + count.ToString();
                lock (m_acknowledgements)
                {
                    if (m_acknowledgements.ContainsKey(key))
                    {
                        if (m_acknowledgements[key]) //positively acknowledged in the mean time;
                        {
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Info, "telegram allready positively acknowledged " + key);
                            m_acknowledgements.Remove(key);
                            m_idToFilename.Remove(key);
                            continue;
                        }
                        else //negatively acknowledged, regard the datfile as never send
                        {
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Info, "telegram allready negatively acknowledged " + key);
                            m_acknowledgements.Remove(key);
                            string item = String.Format("{0}: '{1}'", telegram.QdtType, m_nak_errors[key]); 
                            m_nak_errors.Remove(key);
                            //NAKnowledged.Add(item);
                            m_idToFilename.Remove(key);
                        }
                    }
                }
                if (m_messageQueue.Count > 500)
                {
                    if (m_logger != null && m_logger.IsOpen)
                        m_logger.Log(Level.Exception, "message queue full");
                    m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_queue;
                    reader.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(reader);
                    return false;
                }
                QdtTelegram qdt = null;
                if (m_nqs1Status == NQSStatus.GO)
                {
                    qdt = new QdtTelegram(telegram, reader, m_idCounter, m_nqs1Messages++);
                }
                else if (m_nqs2Status == NQSStatus.GO)
                {
                    qdt = new QdtTelegram(telegram, reader, m_idCounter, m_nqs2Messages++);
                }
                else
                {
                    m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_nogo;
                    if (m_logger != null && m_logger.IsOpen)
                        m_logger.Log(Level.Exception, m_error);
                    reader.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(reader);
                    return false;
                }

                if (qdt.ErrPos != -1)
                {
                    string subitem = String.Format(qdt.ErrInInfo ? Alunorf_sinec_h1_plugin.Properties.Resources.err_tele_info : Alunorf_sinec_h1_plugin.Properties.Resources.err_tele_signal, qdt.ErrPos);
                    string item = String.Format("{0}: '{1}'", telegram.QdtType, subitem);
                    ErrorTelegram.Add(item);
                }
                else
                {
                    lock (m_idCounterLock)
                    {
                        qdt.AktId = m_idCounter;
                        m_idCounter += 2;
                    }
                    QueueTelegram(qdt, key);
                    //wait until acknowledged
                    m_waitSendEvent = new AutoResetEvent(false);
                    m_waitSendEvent.WaitOne(1000 * (m_data.SendTimeOut + m_data.AckTimeOut), true);
                }


                lock (m_acknowledgements)
                {
                    if (m_acknowledgements.ContainsKey(key))
                    {
                        if (m_acknowledgements[key]) //positively acknowledged in the mean time;
                        {
                            m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_nogo;
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Info, "telegram positively acknowledged in the mean time " + key);

                        }
                        else if (m_nak_errors[key].StartsWith(Alunorf_sinec_h1_plugin.Properties.Resources.err_send))
                        {
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Exception, m_nak_errors[key]);
                            m_nak_errors.Remove(key);
                            string item = String.Format("{0}: '{1}'", telegram.QdtType, m_nak_errors[key].Substring(Alunorf_sinec_h1_plugin.Properties.Resources.err_send.Length));
                            ErrorSendTelegram.Add(item);
                        }
                        else
                        {
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Exception, m_nak_errors[key]);
                            string item = String.Format("{0}: '{1}'", telegram.QdtType, m_nak_errors[key]);
                            m_nak_errors.Remove(key);
                            NAKnowledged.Add(item);
                        }
                        m_idToFilename.Remove(key);
                    }
                    else 
                    {
                        if (m_logger != null && m_logger.IsOpen)
                            m_logger.Log(Level.Exception, "telegram not acknowledged in time " + key);
                        UnTimelyAcknowledged.Add(telegram.QdtType);
                    }
                }
            }
            reader.Close();
            System.Runtime.InteropServices.Marshal.ReleaseComObject(reader);

            if (UnTimelyAcknowledged.Count == 0 && NAKnowledged.Count == 0 && ErrorTelegram.Count == 0)
                return true;
            else
            {
                StringBuilder b = new StringBuilder(Alunorf_sinec_h1_plugin.Properties.Resources.err_tele);
                if (ErrorTelegram.Count > 0)
                {
                   b.Append( Environment.NewLine);
                   b.Append( Alunorf_sinec_h1_plugin.Properties.Resources.err_tele_malformed);
                   foreach(string item in ErrorTelegram)
                   {
                       b.Append( Environment.NewLine);
                       b.Append( item);
                   }         
                }
                if (NAKnowledged.Count > 0)
                {
                    b.Append(Environment.NewLine);
                    b.Append( Alunorf_sinec_h1_plugin.Properties.Resources.err_tele_NAK);
                    foreach(string item in NAKnowledged)
                    {
                        b.Append(Environment.NewLine);
                        b.Append(item);
                    }
                }
                if (ErrorSendTelegram.Count > 0)
                {
                    b.Append(Environment.NewLine);
                    b.Append(Alunorf_sinec_h1_plugin.Properties.Resources.err_tele_send);
                    foreach (string item in ErrorSendTelegram)
                    {
                        b.Append(Environment.NewLine);
                        b.Append(item);
                    }
                }
                if (UnTimelyAcknowledged.Count > 0)
                {
                    b.Append(Environment.NewLine);
                    b.Append(Alunorf_sinec_h1_plugin.Properties.Resources.err_tele_time);
                    foreach (short item in UnTimelyAcknowledged)
                    {
                        b.Append(Environment.NewLine);
                        b.Append(item);
                    }
                }
                m_error = b.ToString();
                return false;
            }
        }

        public string GetLastError()
        {
            return m_error;
        }

        public PluginTaskWorkerStatus GetWorkerStatus()
        {
            PluginTaskWorkerStatus answer = new PluginTaskWorkerStatus();
            answer.started = m_started;
            NqsServerStatusses statusses = new NqsServerStatusses();
            statusses.nqs1 = m_nqs1Status;
            statusses.nqs2 = m_nqs2Status;
            answer.extraData = statusses;
            return answer;
        }

        #endregion

        public enum NQSStatus { DISCONNECTED, CONNECTED, INITIALISED, GO }
        public enum ReadState {NONE, NQS1, NQS2 }
        private ushort m_nqs1Messages;
        private ushort m_nqs2Messages;
        private ushort m_idCounter;
        private NQSStatus m_nqs1Status;
        private NQSStatus m_nqs2Status;
        private bool m_stop;
        private string m_error;
        private SortedDictionary<string,string> m_nak_errors;
        private PluginH1Task m_data;
        private Thread m_thread;
        private CH1Manager m_h1manager;
        private ushort m_vnr1;
        private ushort m_vnr2;
        private ushort m_lastgo;
        private bool m_retryConnect;
        private System.Threading.Timer m_reconnectTimer;
        private System.Threading.Timer m_liveTimer;
        private AutoResetEvent m_waitConnectedEvent;
        private AutoResetEvent m_waitSendEvent;
        private BiMap<int, string> m_idToFilename;
        private SortedDictionary<string, bool> m_acknowledgements;
        private List<Message> m_messageQueue;
        private bool m_started;

        public bool Started
        {
            get { return m_started; }
        }


        public PluginH1TaskWorker(PluginH1Task data)
        {
            m_idCounterLock = new Object();
            m_data = data;
            InitMemberData();
        }

        private void InitMemberData()
        {
            m_stop = false;
            m_nqs1Messages = 0;
            m_nqs2Messages = 0;
            m_nqs1Status = NQSStatus.DISCONNECTED;
            m_nqs2Status = NQSStatus.DISCONNECTED;
            m_error = null;
            m_retryConnect = true;
            m_h1manager = new CH1Manager();
            m_idCounter = 1;
            m_idToFilename = new BiMap<int, string>();
            m_acknowledgements = new SortedDictionary<string, bool>();
            m_messageQueue = new List<Message>();
            m_reconnectTimer = new System.Threading.Timer(OnReconnectTimerTick);
            m_liveTimer = new System.Threading.Timer(OnLiveTimerTick);
            m_nak_errors = new SortedDictionary<string, string>();
        }

        //because NQS1 and NQS2 possibly don't know each others generated AktIds
        //the next two methods guarantee uniqueness
        private int FromNonUniqueToUnique(int from, ushort vnr)
        {
            if (from % 2 == 1) return from; //generated on ALR (PC) -> odd stays odd
            else if (vnr == m_vnr1) return 2 * from;
            else if (vnr == m_vnr2) return 2 * from + 2;
            else throw new ArgumentException("wrong vnr given");
        }

        private int FromUniqueToNonUnique(int from)
        {
            if (from % 2 == 1) return from; //generated on ALR (PC) -> odd stays odd
            else if (from % 4 == 0) return from/2;
            else return (from-2)/ 2;
        }

        private int m_disconnectedCounter1;
        private int m_disconnectedCounter2;

        private void Run()
        {
            while (!m_stop)
            {
                if (m_retryConnect)
                {
                    NQSStatus oldStatus1 = m_nqs1Status;
                    NQSStatus oldStatus2 = m_nqs2Status;
                    bool initMade = false;
                    BuildConnection();
                    if ((m_nqs1Status == NQSStatus.GO || m_nqs1Status == NQSStatus.INITIALISED)&& (oldStatus1==NQSStatus.DISCONNECTED||oldStatus1==NQSStatus.CONNECTED))
                    {
                        m_acknowledgements["live1"] = false;
                        lock (m_idCounterLock)
                        {
                            QueueTelegram(m_vnr1, new LiveTelegram(m_idCounter, m_nqs1Messages++), "live1");
                            m_idCounter += 2;
                        }
                        initMade = true;
                    }
                    if ((m_nqs2Status == NQSStatus.GO || m_nqs2Status == NQSStatus.INITIALISED) && (oldStatus2==NQSStatus.DISCONNECTED||oldStatus2==NQSStatus.CONNECTED))
                    {
                        m_acknowledgements["live2"] = false;
                        lock (m_idCounterLock)
                        {
                            QueueTelegram(m_vnr2, new LiveTelegram(m_idCounter, m_nqs2Messages++), "live2");
                            m_idCounter += 2;
                        }
                        initMade = true;
                    }
                    if (initMade)
                            m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);

                    if ((m_nqs1Status == NQSStatus.CONNECTED || m_nqs1Status == NQSStatus.INITIALISED) && m_lastgo == m_vnr2 && m_nqs2Status == NQSStatus.DISCONNECTED)
                    { //try to switch to NQS1 server by sending init telegram
                        lock (m_idCounterLock)
                        {
                            QueueTelegram(m_vnr1, new IniTelegram(m_idCounter, m_nqs1Messages++), "init1");
                            m_idCounter += 2;
                        }
                        
                    }
                    else if ((m_nqs2Status == NQSStatus.CONNECTED || m_nqs2Status == NQSStatus.INITIALISED) && m_lastgo == m_vnr1 && m_nqs1Status == NQSStatus.DISCONNECTED)
                    { //try to switch to NQS1 server by sending init telegram
                        lock (m_idCounterLock)
                        {
                            QueueTelegram(m_vnr2, new IniTelegram(m_idCounter, m_nqs2Messages++), "init2");
                            m_idCounter += 2;
                        }
                    }
                }
                if (!ReadMessage(m_vnr1))
                {
                    m_stop = true;
                    break;
                }
                else if (m_disconnectedCounter1 > DISCONNECTIONCOUNTMAX && m_nqs1Status != NQSStatus.DISCONNECTED)
                {
                    if ((m_logger != null) && m_logger.IsOpen)
                        m_logger.Log(Level.Exception, "disconnection detected with NQS1 by repeatable failure to read a message");
                    m_disconnectedCounter1 = 0;
                    m_nqs1Status = NQSStatus.DISCONNECTED;
                    m_retryConnect = true;
                }
                if (!ReadMessage(m_vnr2))
                {
                    m_stop = true;
                    break;
                }
                else if (m_disconnectedCounter2 > DISCONNECTIONCOUNTMAX && m_nqs2Status != NQSStatus.DISCONNECTED)
                {
                    if ((m_logger != null) && m_logger.IsOpen)
                        m_logger.Log(Level.Exception, "disconnection detected with NQS2 by repeatable failure to read a message");
                    m_disconnectedCounter2 = 0;
                    m_nqs2Status = NQSStatus.DISCONNECTED;
                    m_retryConnect = true;
                }
                if (!ProcessTelegram())
                {
                    m_stop = true;
                    break;
                }
                Thread.Sleep(THREADSLEEPTIME);
            }
            m_h1manager.DisconnectAll();
            m_liveTimer.Change(Timeout.Infinite, Timeout.Infinite);
            m_liveTimer.Dispose();
            m_liveTimer = null;
            m_reconnectTimer.Change(Timeout.Infinite, Timeout.Infinite);
            m_reconnectTimer.Dispose();
            m_reconnectTimer = null;
            m_nqs1Status = m_nqs2Status = NQSStatus.DISCONNECTED;
            m_started = false;
        }

        private bool ReadMessage(ushort vnr)
        {
            if ((m_nqs1Status == NQSStatus.DISCONNECTED && vnr == m_vnr1) || (m_nqs2Status == NQSStatus.DISCONNECTED && vnr == m_vnr2))
                return true;
            CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
            
            if ((vnr == m_vnr1 && !m_readStarted1) || (vnr == m_vnr2 && !m_readStarted2))
                m_h1manager.StartRead(vnr, ref result);
            else
                m_h1manager.GetReadStatus(vnr,ref result);
            switch (result)
            {
                case CH1Manager.H1Result.ALL_CLEAR:
                    if (vnr == m_vnr1)
                        m_disconnectedCounter1=0;
                    else if (vnr == m_vnr2)
                        m_disconnectedCounter2=0;
                    WrapperTelegram wrap = new WrapperTelegram();
                    if (m_h1manager.FinishRead(vnr, wrap))
                    {
                        NAKTelegram nak = wrap.InnerTelegram as NAKTelegram;
                        if (nak != null && m_idToFilename.Contains(FromNonUniqueToUnique(nak.AktId,vnr)))
                        {
                            string id = m_idToFilename[FromNonUniqueToUnique(nak.AktId,vnr)];
                            if ((m_logger != null) && m_logger.IsOpen)
                                m_logger.Log(Level.Info, "NAK telegram recieved from vnr " + vnr.ToString() + " " + m_idToFilename.itemToString(id));
                            m_nak_errors[id] = nak.FehlerText; 
                            m_acknowledgements[id] = false;
                            if (id == "init1" || id == "init2" )
                                m_waitConnectedEvent.Set();
                            else if (id != "live1" && id != "live2")
                                m_waitSendEvent.Set();
                        }
                        else
                        {
                            AckTelegram ack = wrap.InnerTelegram as AckTelegram;
                            if (ack != null && m_idToFilename.Contains(FromNonUniqueToUnique(ack.AktId, vnr)))
                            {
                                string id = m_idToFilename[FromNonUniqueToUnique(ack.AktId, vnr)];
                                if (id == "init1")
                                {
                                    if ((m_logger != null) && m_logger.IsOpen)
                                        m_logger.Log(Level.Info, "ACK init from NQS1 " + m_idToFilename.itemToString(id));
                                    m_nqs1Status = NQSStatus.INITIALISED;
                                    m_acknowledgements["live1"] = false;
                                    m_acknowledgements["init1"] = true;
                                    lock (m_idCounterLock)
                                    {
                                        QueueTelegram(m_vnr1, new LiveTelegram(m_idCounter, m_nqs1Messages++), "live1");
                                        m_idCounter+=2;
                                    }
                                    m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);
                                }
                                else if (id == "init2")
                                {
                                    if ((m_logger != null) && m_logger.IsOpen)
                                        m_logger.Log(Level.Info, "ACK init from NQS2 " + m_idToFilename.itemToString(id));
                                    m_nqs1Status = NQSStatus.INITIALISED;
                                    m_acknowledgements["live2"] = false;
                                    m_acknowledgements["init2"] = true;
                                    lock (m_idCounterLock)
                                    {
                                        QueueTelegram(m_vnr2, new LiveTelegram(m_idCounter, m_nqs1Messages++), "live2");
                                        m_idCounter += 2;
                                    }
                                    m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);
                                }
                                else if (id == "live1" || id == "live2")
                                {
                                    if ((m_logger != null) && m_logger.IsOpen)
                                        m_logger.Log(Level.Info, "ACK live from " + vnr.ToString() + m_idToFilename.itemToString(id));
                                    m_acknowledgements[id] = true;
                                }
                                else
                                {
                                    if ((m_logger != null) && m_logger.IsOpen)
                                        m_logger.Log(Level.Info, "ACK received: " + m_idToFilename.itemToString(id));
                                    m_acknowledgements[id] = true;
                                    m_waitSendEvent.Set();
                                }
                            }
                            else
                            {
                                GoTelegram go = wrap.InnerTelegram as GoTelegram;
                                //if (go != null && m_idToFilename.Contains(FromNonUniqueToUnique(go.AktId,vnr)))
                                if (go != null)
                                {
                                    bool previousInitAcknowledged = false;
                                    if (vnr == m_vnr1)
                                    {
                                        if ((m_acknowledgements.ContainsKey("init1") && m_acknowledgements["init1"]) 
                                            || m_idToFilename.Contains("init1_nqs"))
                                            previousInitAcknowledged = true;
                                    }
                                    else if (vnr == m_vnr2)
                                    {
                                        if ((m_acknowledgements.ContainsKey("init2") && m_acknowledgements["init2"]) 
                                            || m_idToFilename.Contains("init2_nqs"))
                                            previousInitAcknowledged = true;
                                    }
                                    if (previousInitAcknowledged && m_vnr1== vnr)
                                    {
                                        if ((m_logger != null) && m_logger.IsOpen)
                                            m_logger.Log(Level.Info, "GO recieved from NQS1");
                                        m_nqs1Status = NQSStatus.GO;
                                        AckTelegram ackgo = new AckTelegram(go.AktId, m_nqs1Messages++);
                                        if (QueueTelegram(m_vnr1, ackgo))
                                        {
                                            if (m_nqs2Status == NQSStatus.GO) m_nqs2Status = NQSStatus.INITIALISED;
                                            m_waitConnectedEvent.Set();
                                        }
                                        else
                                        {
                                            m_nqs1Status = NQSStatus.DISCONNECTED;
                                            m_retryConnect = true;
                                        }
                                    }
                                    else if (previousInitAcknowledged && m_vnr2 == vnr)
                                    {
                                        if ((m_logger != null) && m_logger.IsOpen)
                                            m_logger.Log(Level.Info, "GO recieved from NQS2 ");
                                        m_nqs2Status = NQSStatus.GO;
                                        AckTelegram ackgo = new AckTelegram(go.AktId, m_nqs2Messages++);
                                        if (QueueTelegram(m_vnr2, ackgo))
                                        {
                                            if (m_nqs1Status == NQSStatus.GO) m_nqs1Status = NQSStatus.INITIALISED;
                                            m_waitConnectedEvent.Set();
                                        }
                                        else
                                        {
                                            m_nqs2Status = NQSStatus.DISCONNECTED;
                                            m_retryConnect = true;
                                        }
                                    }
                                    else
                                    {
                                        if ((m_logger != null) && m_logger.IsOpen)
                                        {
                                            if (vnr == m_vnr1)
                                                m_logger.Log(Level.Info, "GO recieved from NQS1 but with no previous init " + go.AktId.ToString());
                                            else
                                                m_logger.Log(Level.Info, "GO recieved from NQS2 but with no previous init " + go.AktId.ToString());
                                        }
                                        m_error = "go recieved with no previous init";
                                    }
                                }
                                //else if (go != null)
                                //{
                                //    if ((m_logger != null) && m_logger.IsOpen)
                                //        m_logger.Log(Level.Info, "GO recieved from NQS2 but with no previous init " + go.AktId.ToString()); 
                                //    m_error = "go recieved with no previous init";
                                //}
                                else
                                {
                                    IniTelegram ini = wrap.InnerTelegram as IniTelegram;
                                    if (ini != null)
                                    {
                                        if (vnr == m_vnr1)
                                        {
                                            if ((m_logger != null) && m_logger.IsOpen)
                                                m_logger.Log(Level.Info, "INI recieved from NQS1 " + m_idToFilename.itemToString(FromNonUniqueToUnique(ini.AktId, vnr)));
                                            m_nqs1Status = NQSStatus.INITIALISED;
                                            AckTelegram ackini = new AckTelegram(ini.AktId, m_nqs1Messages++);
                                            if (!QueueTelegram(m_vnr1, ackini, "init1_nqs"))
                                            {
                                                m_nqs1Status = NQSStatus.DISCONNECTED;
                                                m_retryConnect = true;
                                            }
                                        }
                                        else if (vnr == m_vnr2)
                                        {
                                            if ((m_logger != null) && m_logger.IsOpen)
                                                m_logger.Log(Level.Info, "INI recieved from NQS2 " + m_idToFilename.itemToString(FromNonUniqueToUnique(ini.AktId, vnr)));
                                            m_nqs2Status = NQSStatus.INITIALISED;
                                            AckTelegram ackini = new AckTelegram(ini.AktId, m_nqs2Messages++);
                                            if (!QueueTelegram(m_vnr2, ackini, "init2_nqs"))
                                            {
                                                m_nqs2Status = NQSStatus.DISCONNECTED;
                                                m_retryConnect = true;
                                            }
                                        }
                                    }
                                    else
                                    {
                                        LiveTelegram live = wrap.InnerTelegram as LiveTelegram;
                                        if (live != null)
                                        {
                                            if (vnr == m_vnr1)
                                            {
                                                if ((m_logger != null) && m_logger.IsOpen)
                                                    m_logger.Log(Level.Info, "LIVE recieved from NQS1 " + m_idToFilename.itemToString(FromNonUniqueToUnique(live.AktId, vnr)));
                                                AckTelegram acklive = new AckTelegram(live.AktId, m_nqs1Messages++);
                                                if (!QueueTelegram(m_vnr1, acklive))
                                                {
                                                    m_nqs1Status = NQSStatus.DISCONNECTED;
                                                    m_retryConnect = true;
                                                }
                                            }
                                            else if (vnr == m_vnr2)
                                            {
                                                if ((m_logger != null) && m_logger.IsOpen)
                                                    m_logger.Log(Level.Info, "LIVE recieved from NQS2 " + m_idToFilename.itemToString(FromNonUniqueToUnique(live.AktId, vnr)));
                                                AckTelegram acklive = new AckTelegram(live.AktId, m_nqs2Messages++);
                                                if (!QueueTelegram(m_vnr2, acklive))
                                                {
                                                    m_nqs2Status = NQSStatus.DISCONNECTED;
                                                    m_retryConnect = true;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    else return false;
                    goto case CH1Manager.H1Result.NO_REQUEST;
                case CH1Manager.H1Result.NO_REQUEST:
                    if (vnr == m_vnr1)
                    {
                        m_readStarted1 = false;
                        m_disconnectedCounter1=0;
                    }
                    else if (vnr == m_vnr2)
                    {
                        m_readStarted2 = false;
                        m_disconnectedCounter2=0;
                    }
                    return true;
                case CH1Manager.H1Result.WAIT_CONNECT:
                    if (vnr == m_vnr1)
                    {
                        m_readStarted1 = false;
                        m_disconnectedCounter1++;
                    }
                    else if (vnr == m_vnr2)
                    {
                        m_readStarted2 = false;
                        m_disconnectedCounter2++;
                    }
                    return true;
                case CH1Manager.H1Result.ALREADY_RUNNING:
                case CH1Manager.H1Result.WAIT_DATA:
                    if (vnr == m_vnr1)
                    {
                        m_readStarted1 = true;
                        m_disconnectedCounter1=0;
                    }
                    else if (vnr == m_vnr2)
                    {
                        m_readStarted2 = true;
                        m_disconnectedCounter2=0;
                    }
                    return true;
                default: // error
                    return false; 
            }
        }

        private void BuildConnection()
        {
            if ((m_logger != null) && m_logger.IsOpen)
                m_logger.Log(Level.Info, "starting build connection : " + m_vnr1.ToString() + ":" + m_nqs1Status.ToString() + " " + m_vnr2.ToString() + ":"  + m_nqs2Status.ToString());
            m_retryConnect = false;
            if (m_nqs1Status == NQSStatus.DISCONNECTED)
            {
                CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
                if (m_vnr1 == 0)
                    result = CH1Manager.H1Result.BAD_LINE;
                else
                    m_h1manager.GetConnectionStatus(m_vnr1, ref result);
                switch (result)
                {
                    case CH1Manager.H1Result.ALL_CLEAR:
                        {
                            m_nqs1Status = NQSStatus.CONNECTED;
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Info, "connection to nqs1 was still ok");
                            //send INI telegram
                            lock (m_idCounterLock)
                            {
                                QueueTelegram(m_vnr1, new IniTelegram(m_idCounter, m_nqs1Messages++),"init1");
                                m_idCounter += 2;
                            }
                            break;
                        }
                    case CH1Manager.H1Result.OPERATING_SYSTEM_ERROR:
                        m_retryConnect = true;
                        m_error = m_h1manager.LastError;
                        break;
                    case CH1Manager.H1Result.WAIT_CONNECT:
                        m_retryConnect = true;
                        break;
                    case CH1Manager.H1Result.BAD_LINE:
                        if (m_h1manager.Connect(ref m_vnr1, 4, false, m_data.NQSAddress1, m_data.OwnTSAPforNQS1, m_data.NQS_TSAPforNQS1, ref result, m_data.ConnectionTimeOut))
                        {
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Info, "connection to nqs1 restored");
                            m_nqs1Status = NQSStatus.CONNECTED;
                            //send INI telegram
                            lock (m_idCounterLock)
                            {
                                QueueTelegram(m_vnr1, new IniTelegram(m_idCounter, m_nqs1Messages++), "init1");
                                m_idCounter += 2;
                            }
                    }
                        else
                        {
                            m_error = m_h1manager.LastError;
                            m_retryConnect = true;
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Info, "timeout trying to connect to nqs1");
                        }
                        break;
                    default:
                        m_error = m_h1manager.LastError;
                        m_retryConnect = true;
                        break;
                }
            }
           
            if (m_nqs2Status == NQSStatus.DISCONNECTED)
            {
                CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
                if (m_vnr2 == 0)
                    result = CH1Manager.H1Result.BAD_LINE;
                else
                    m_h1manager.GetConnectionStatus(m_vnr2, ref result);
                switch (result)
                {
                    case CH1Manager.H1Result.ALL_CLEAR:
                        {
                            m_nqs2Status = NQSStatus.CONNECTED;
                            //send INI telegram
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Info, "connection to nqs2 was still ok");
                            lock (m_idCounterLock)
                            {
                                QueueTelegram(m_vnr2, new IniTelegram(m_idCounter, m_nqs2Messages++), "init2");
                                m_idCounter += 2;
                            }
                            break;
                        }
                    case CH1Manager.H1Result.OPERATING_SYSTEM_ERROR:
                        m_retryConnect = true;
                        m_error = m_h1manager.LastError;
                        break;
                    case CH1Manager.H1Result.WAIT_CONNECT:
                        m_retryConnect = true;
                        break;
                    case CH1Manager.H1Result.BAD_LINE:
                        if (m_h1manager.Connect(ref m_vnr2, 4, false, m_data.NQSAddress2, m_data.OwnTSAPforNQS2, m_data.NQS_TSAPforNQS2, ref result, m_data.ConnectionTimeOut))
                        {
                            m_nqs2Status = NQSStatus.CONNECTED;
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Info, "connection to nqs2 restored");
                            //send INI telegram
                            lock (m_idCounterLock)
                            {
                                QueueTelegram(m_vnr2, new IniTelegram(m_idCounter, m_nqs2Messages++), "init2");
                                m_idCounter += 2;
                            }
                        }
                        else
                        {
                            if (m_logger != null && m_logger.IsOpen)
                                m_logger.Log(Level.Info, "timeout trying to connect to nqs2");
                            m_error = m_h1manager.LastError;
                            m_retryConnect = true;
                        }
                        break;
                    default:
                        m_error = m_h1manager.LastError;
                        m_retryConnect = true;
                        break;
                }
            }

            if (m_retryConnect)
            {
                m_retryConnect = false;
                //start reconnect timer
                m_reconnectTimer.Change(TimeSpan.FromMinutes((double)m_data.RetryConnectTimeInterval), TimeSpan.Zero);
            }

            if ((m_logger != null) && m_logger.IsOpen)
                m_logger.Log(Level.Info, "exiting buildconnection : " + m_vnr1.ToString() + ":" + m_nqs1Status.ToString() + " " + m_vnr2.ToString() + ":"  + m_nqs2Status.ToString());
        }

        private void OnReconnectTimerTick(object ignoreMe)
        {
            if (m_stop) return;
            m_reconnectTimer.Change(Timeout.Infinite, Timeout.Infinite);
            m_retryConnect = true;
        }

        private void OnLiveTimerTick(object ignoreMe)
        {
            if (m_stop) return;
            m_liveTimer.Change(Timeout.Infinite, Timeout.Infinite);
            if (m_nqs1Status == NQSStatus.GO || m_nqs1Status == NQSStatus.INITIALISED)
            {
                if (m_acknowledgements["live1"] == false)
                {
                    m_nqs1Status = NQSStatus.DISCONNECTED;
                    m_retryConnect = true;
                    if ((m_logger != null) && m_logger.IsOpen)
                        m_logger.Log(Level.Exception, "LiveTimer: nqs1 timed out");
                }
            }

            if (m_nqs2Status == NQSStatus.GO || m_nqs2Status == NQSStatus.INITIALISED)
            {
                if (m_acknowledgements["live2"] == false)
                {
                    m_nqs2Status = NQSStatus.DISCONNECTED;
                    m_retryConnect = true;
                    if (m_logger != null && m_logger.IsOpen)
                        m_logger.Log(Level.Exception, "LiveTimer: nqs1 timed out");
                }
            }

            if (m_nqs1Status == NQSStatus.GO || m_nqs1Status == NQSStatus.INITIALISED)
            {
                m_acknowledgements["live1"] = false;
                lock (m_idCounterLock)
                {
                    QueueTelegram(m_vnr1, new LiveTelegram(m_idCounter, m_nqs1Messages++), "live1");
                    m_idCounter += 2;
                }
            }
            if (m_nqs2Status == NQSStatus.GO || m_nqs2Status == NQSStatus.INITIALISED)
            {
                m_acknowledgements["live2"] = false;
                lock (m_idCounterLock)
                {
                    QueueTelegram(m_vnr2, new LiveTelegram(m_idCounter, m_nqs2Messages++), "live2");
                    m_idCounter += 2;
                }
            }
            if (m_nqs1Status == NQSStatus.GO || m_nqs1Status == NQSStatus.INITIALISED
                || m_nqs2Status == NQSStatus.GO || m_nqs2Status == NQSStatus.INITIALISED)
                m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);
        }

        private struct Message
        {
            public AlunorfTelegram telegram;
            public ushort vnr;
            public string id;

            public Message(ushort p_vnr, AlunorfTelegram p_telegram, string p_id)
            {
                vnr = p_vnr;
                id = p_id;
                telegram = p_telegram;
            }
        }

        private bool QueueTelegram(AlunorfTelegram telegram)
        {
            return QueueTelegram(0, telegram, null);
        }

        private bool QueueTelegram(AlunorfTelegram telegram, string id)
        {
            return QueueTelegram(0, telegram, id);
        }

        private bool QueueTelegram(ushort vnr, AlunorfTelegram telegram)
        {
            return QueueTelegram(vnr, telegram, null);
        }
        
        private bool QueueTelegram(ushort vnr, AlunorfTelegram telegram, string id)
        {
            lock (m_messageQueue)
            {
                if (telegram is QdtTelegram && m_messageQueue.FindIndex(delegate(Message m) { return m.id == id; }) != -1)
                    return true;
                if (m_messageQueue.Count > 500) return false;
                m_messageQueue.Add(new Message(vnr, telegram, id));
                return true;
            }
        }

        private bool ProcessTelegram()
        {
            lock (m_messageQueue)
            {
                if (m_messageQueue.Count == 0) return true;
                
                int pos = -1;
                ushort vnr = 0;
                for (int i = 0; i < m_messageQueue.Count && vnr == 0; i++)
                {
                    vnr = m_messageQueue[i].vnr;
                    if (vnr == 0 && m_nqs1Status == NQSStatus.GO)
                    {
                        vnr = m_vnr1;
                        pos = i;
                        break;
                    }
                    else if (vnr == 0 && m_nqs2Status == NQSStatus.GO)
                    {
                        vnr = m_vnr2;
                        pos = i;
                        break;
                    }
                    else if (vnr == m_vnr1 && m_nqs1Status != NQSStatus.DISCONNECTED)
                    {
                        pos = i;
                        break;
                    }
                    else if (vnr == m_vnr2 && m_nqs2Status != NQSStatus.DISCONNECTED)
                    {
                        pos = i;
                        break;
                    }
                }

                if (pos == -1) return true; //no messages to process
                m_lastgo = vnr;
                AlunorfTelegram telegram = m_messageQueue[pos].telegram;

                CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
                m_h1manager.GetSendStatus(vnr, ref result);
                switch (result)
                {
                    case CH1Manager.H1Result.WAIT_CONNECT:
                        if (vnr == m_vnr1)
                            m_disconnectedCounter1++;
                        else if (vnr == m_vnr2)
                            m_disconnectedCounter2++;
                        return true;
                    case CH1Manager.H1Result.WAIT_SEND:
                    case CH1Manager.H1Result.ALREADY_RUNNING:
                        return true; //stay in queue, sending is busy with previous telegram
                }

                m_h1manager.StartSend(vnr, ref result, telegram);
                if ((m_logger != null) && m_logger.IsOpen)
                    m_logger.Log(Level.Info, "started sending on vnr: " + vnr.ToString() 
                    + " " + m_idToFilename.itemToString(FromNonUniqueToUnique(telegram.AktId,vnr)));
                QdtTelegram qdt = null;
                switch (result)
                {
                    case CH1Manager.H1Result.TELEGRAM_ERROR:
                        if ((m_logger != null) && m_logger.IsOpen)
                            m_logger.Log(Level.Info, "telegram contained an error" + m_idToFilename.itemToString(FromNonUniqueToUnique(telegram.AktId, vnr)));
                        m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_send + m_h1manager.LastError ?? "";
                        qdt = telegram as QdtTelegram;
                        if (qdt != null)
                            qdt.Dispose();
                        m_messageQueue.RemoveAt(pos);
                        return false;
                    case CH1Manager.H1Result.WAIT_CONNECT:
                        if (vnr == m_vnr1)
                            m_disconnectedCounter1++;
                        else if (vnr == m_vnr2)
                            m_disconnectedCounter2++;
                        return true;
                    case CH1Manager.H1Result.WAIT_SEND:
                    case CH1Manager.H1Result.ALL_CLEAR:
                        string id = m_messageQueue[pos].id;
                        if (id != null)
                        {
                            m_idToFilename[FromNonUniqueToUnique(telegram.AktId, vnr)] = id;
                            if (id != "live1" && id != "live2")
                                m_acknowledgements.Remove(id);
                        }
                        qdt = telegram as QdtTelegram;
                        if(qdt != null)
                            qdt.Dispose();
                        m_messageQueue.RemoveAt(pos);
                        if (vnr == m_vnr1)
                            m_disconnectedCounter1=0;
                        else if (vnr == m_vnr2)
                            m_disconnectedCounter2=0;
                        return true;
                    case CH1Manager.H1Result.ALREADY_RUNNING:
                        return true; //stay in queue, sending is busy with previous telegram
                }
                return false;
            }
        }
    }
}
