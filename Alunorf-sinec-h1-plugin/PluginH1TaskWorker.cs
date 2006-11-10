using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using iba;
using iba.Plugins;
using IBAFILESLib;

namespace Alunorf_sinec_h1_plugin
{
    public class NqsServerStatusses
    {
        public PluginH1TaskWorker.NQSStatus nqs1;
        public PluginH1TaskWorker.NQSStatus nqs2;
    }

    public class PluginH1TaskWorker : IPluginTaskWorker
    {
        #region IPluginTaskWorker Members

        public bool OnStart()
        {
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
            m_thread.Start();
            
            //wait here until initialisation succeeded
            m_waitConnectedEvent = new AutoResetEvent(false);
            m_waitConnectedEvent.WaitOne(2000* (m_data.SendTimeOut + m_data.AckTimeOut + m_data.ConnectionTimeOut + 30),true);
            if ((m_nqs1Status == NQSStatus.GO && m_nqs2Status == NQSStatus.CONNECTED) || (m_nqs2Status == NQSStatus.GO && m_nqs1Status == NQSStatus.CONNECTED))
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
            return m_stop = true;
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
                            m_acknowledgements.Remove(key);
                            m_idToFilename.Remove(key);
                            continue;
                        }
                        else //negatively acknowledged, regard the datfile as never send
                        {
                            m_acknowledgements.Remove(key);
                            string item = String.Format("{0}: '{1}'", telegram.QdtType, m_nak_errors[key]); 
                            m_nak_errors.Remove(key);
                            NAKnowledged.Add(item);
                            m_idToFilename.Remove(key);
                            continue;
                        }
                    }
                }

                lock (m_messageQueue)
                {
                    if (m_messageQueue.Count > 1000)
                    {
                        m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_queue;
                        return false;
                    }
                    QdtTelegram qdt = null;
                    if (m_nqs1Status == NQSStatus.GO)
                    {
                        qdt = new QdtTelegram(telegram, reader, m_idCounter, m_nqs1Messages++);
                    }
                    else if (m_nqs1Status == NQSStatus.GO)
                    {
                        qdt = new QdtTelegram(telegram, reader, m_idCounter, m_nqs2Messages++);
                    }
                    else
                    {
                        m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_nogo;
                        return false;
                    }
                    m_idCounter += 2;
                    m_messageQueue.Add(qdt);
                }

                //wait until acknowledged
                m_waitSendEvent = new AutoResetEvent(false);
                m_waitSendEvent.WaitOne(1000 * (m_data.SendTimeOut + m_data.AckTimeOut), true);

                lock (m_acknowledgements)
                {
                    if (m_acknowledgements.ContainsKey(key))
                    {
                        m_acknowledgements.Remove(key);
                        if (m_acknowledgements[datFile]) //positively acknowledged in the mean time;
                        {
                        }
                        else if (m_nak_errors[key].StartsWith("Malformed telegram:"))
                        {
                            string item = String.Format("{0}: '{1}'", telegram.QdtType, m_nak_errors[key].Substring(9)); ;
                            m_nak_errors.Remove(key);
                            ErrorTelegram.Add(item);
                        }
                        else if (m_nak_errors[key].StartsWith(Alunorf_sinec_h1_plugin.Properties.Resources.err_send))
                        {
                            m_nak_errors.Remove(key);
                            string item = String.Format("{0}: '{1}'", telegram.QdtType, m_nak_errors[key].Substring(Alunorf_sinec_h1_plugin.Properties.Resources.err_send.Length));
                            ErrorSendTelegram.Add(item);
                        }
                        else
                        {
                            string item = String.Format("{0}: '{1}'", telegram.QdtType, m_nak_errors[key]);
                            m_nak_errors.Remove(key);
                            NAKnowledged.Add(item);
                        }
                        m_idToFilename.Remove(key);
                    }
                    else 
                    {
                        UnTimelyAcknowledged.Add(telegram.QdtType);
                    }
                }
            }

            //to do, set errors;
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
        private bool m_retryConnect;
        private System.Threading.Timer m_reconnectTimer;
        private System.Threading.Timer m_liveTimer;
        private AutoResetEvent m_waitConnectedEvent;
        private AutoResetEvent m_waitSendEvent;
        private BiMap<int, string> m_idToFilename;
        private SortedDictionary<string, bool> m_acknowledgements;
        private List<AlunorfTelegram> m_messageQueue;
        private bool m_started;

        public bool Started
        {
            get { return m_started; }
        }


        public PluginH1TaskWorker(PluginH1Task data)
        {
            m_data = data;
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
            m_messageQueue = new List<AlunorfTelegram>();
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

    
        private void Run()
        {
            while (!m_stop)
            {
                if (m_retryConnect)
                {
                    BuildConnection();
                    if (m_nqs1Status == NQSStatus.GO || m_nqs1Status == NQSStatus.INITIALISED)
                    {
                        m_acknowledgements["live1"] = false;
                        SendTelegram(m_vnr1,new LiveTelegram(m_idCounter,m_nqs1Messages++),"live1");
                        m_idCounter +=2;
                    }
                    if (m_nqs2Status == NQSStatus.GO || m_nqs2Status == NQSStatus.INITIALISED)
                    {
                        m_acknowledgements["live2"] = false;
                        SendTelegram(m_vnr2, new LiveTelegram(m_idCounter, m_nqs2Messages++), "live2");
                        m_idCounter += 2;
                    }
                    if (m_nqs1Status == NQSStatus.GO || m_nqs1Status == NQSStatus.INITIALISED
                        || m_nqs2Status == NQSStatus.GO || m_nqs2Status == NQSStatus.INITIALISED)
                            m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);
                }
                if (!ReadMessage(m_vnr1))
                {
                    m_stop = true;
                    break;
                }
                if (!ReadMessage(m_vnr2))
                {
                    m_stop = true;
                    break;
                } 
                lock (m_messageQueue)
                {
                    if (m_messageQueue.Count > 0)
                    {
                        string key = m_idToFilename[m_messageQueue[0].AktId];
                        if (m_nqs1Status == NQSStatus.GO)
                        {
                            if (!SendTelegram(m_vnr1, m_messageQueue[0], key))
                            {
                                m_nak_errors[key] = m_error;
                                m_acknowledgements[key] = false;
                                m_waitSendEvent.Set();
                            }
                            m_messageQueue.RemoveAt(0);
                        }
                        if (m_nqs1Status == NQSStatus.GO)
                        {
                            if (!SendTelegram(m_vnr2, m_messageQueue[0], key))
                            {
                                m_nak_errors[key] = m_error;
                                m_acknowledgements[key] = false;
                                m_waitSendEvent.Set();
                            }
                            m_messageQueue.RemoveAt(0);
                        }
                    }
                }
            }
            m_h1manager.DisconnectAll();
            m_started = false;
        }

        private bool ReadMessage(ushort vnr)
        {
            if ((m_nqs1Status == NQSStatus.DISCONNECTED && vnr == m_vnr1) || (m_nqs2Status == NQSStatus.DISCONNECTED && vnr == m_vnr2))
                return true;
            CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
            m_h1manager.GetReadStatus(vnr,ref result);
            switch (result)
            {
                case CH1Manager.H1Result.ALL_CLEAR:
                    WrapperTelegram wrap = new WrapperTelegram();
                    if (m_h1manager.FinishRead(vnr, wrap))
                    {
                        NAKTelegram nak = wrap.InnerTelegram as NAKTelegram;
                        if (nak != null && m_idToFilename.Contains(nak.AktId))
                        {
                            string id = m_idToFilename[FromNonUniqueToUnique(nak.AktId,vnr)];
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
                                    m_nqs1Status = NQSStatus.INITIALISED;
                                    m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);
                                }
                                else if (id == "init2")
                                {
                                    m_nqs1Status = NQSStatus.INITIALISED;
                                    m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);
                                }
                                else if (id == "live1" || id == "live2")
                                {
                                    m_acknowledgements[id] = true;
                                }
                                else
                                {
                                    m_acknowledgements[id] = true;
                                    m_waitSendEvent.Set();
                                }
                            }
                            else
                            {
                                GoTelegram go = wrap.InnerTelegram as GoTelegram;
                                if (go != null && m_idToFilename.Contains(go.AktId))
                                {
                                    string id = m_idToFilename[FromNonUniqueToUnique(go.AktId,vnr)];
                                    if (id == "init1")
                                    {
                                        m_nqs1Status = NQSStatus.GO;
                                        AckTelegram ackgo = new AckTelegram(go.AktId, m_nqs1Messages++);
                                        if (SendTelegram(m_vnr1, ackgo))
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
                                    else if (id == "init2")
                                    {
                                        m_nqs2Status = NQSStatus.GO;

                                        AckTelegram ackgo = new AckTelegram(go.AktId, m_nqs2Messages++);
                                        if (SendTelegram(m_vnr2, ackgo))
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
                                    m_idToFilename.Remove(id);
                                }
                                else
                                {
                                    IniTelegram ini = wrap.InnerTelegram as IniTelegram;
                                    if (ini != null)
                                    {
                                        if (vnr == m_vnr1)
                                        {
                                            m_nqs1Status = NQSStatus.CONNECTED;
                                            AckTelegram ackini = new AckTelegram(ini.AktId, m_nqs1Messages++);
                                            if (!SendTelegram(m_vnr1, ackini))
                                            {
                                                m_nqs1Status = NQSStatus.DISCONNECTED;
                                                m_retryConnect = true;
                                            }
                                        }
                                        else if (vnr == m_vnr2)
                                        {
                                            m_nqs2Status = NQSStatus.CONNECTED;
                                            AckTelegram ackini = new AckTelegram(ini.AktId, m_nqs2Messages++);
                                            if (!SendTelegram(m_vnr2, ackini))
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
                                                AckTelegram acklive = new AckTelegram(live.AktId, m_nqs1Messages++);
                                                if (!SendTelegram(m_vnr1, acklive))
                                                {
                                                    m_nqs1Status = NQSStatus.DISCONNECTED;
                                                    m_retryConnect = true;
                                                }
                                            }
                                            else if (vnr == m_vnr2)
                                            {
                                                AckTelegram acklive = new AckTelegram(ini.AktId, m_nqs2Messages++);
                                                if (!SendTelegram(m_vnr2, acklive))
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
                    m_h1manager.StartRead(vnr, ref result);
                    return true;
                case CH1Manager.H1Result.NO_REQUEST:
                    m_h1manager.StartRead(vnr, ref result);
                    return true;
                case CH1Manager.H1Result.WAIT_CONNECT:
                    if (m_vnr1 == vnr) m_nqs1Status = NQSStatus.DISCONNECTED;
                    if (m_vnr2 == vnr) m_nqs2Status = NQSStatus.DISCONNECTED;
                    m_retryConnect = true;
                    return true;
                case CH1Manager.H1Result.WAIT_DATA:
                    return true;
                default: // error
                    return false; 
            }
        }

        private void BuildConnection()
        {
            m_retryConnect = false;
            if (m_nqs1Status == NQSStatus.DISCONNECTED)
            {
                CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
                m_h1manager.GetConnectionStatus(m_vnr1, ref result);
                switch (result)
                {
                    case CH1Manager.H1Result.ALL_CLEAR:
                        m_nqs1Status = NQSStatus.CONNECTED;
                        break;
                    case CH1Manager.H1Result.WAIT_CONNECT:
                        m_retryConnect = true;
                        break;
                    case CH1Manager.H1Result.BAD_LINE:
                        if (m_h1manager.Connect(ref m_vnr1, 2, false, m_data.NQSAddress1, m_data.OwnTSAPforNQS1, m_data.NQS_TSAPforNQS1, ref result, m_data.ConnectionTimeOut))
                        {
                            m_nqs1Status = NQSStatus.CONNECTED;
                            //send INI telegram
                            IniTelegram init = new IniTelegram(m_idCounter, m_nqs1Messages++);
                            m_idCounter += 2;
                            SendTelegram(m_vnr1, init, "init1");
                        }
                        else
                        {
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
            if (m_nqs2Status == NQSStatus.DISCONNECTED)
            {
                CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
                m_h1manager.GetConnectionStatus(m_vnr2, ref result);
                switch (result)
                {
                    case CH1Manager.H1Result.ALL_CLEAR:
                        m_nqs2Status = NQSStatus.CONNECTED;
                        break;
                    case CH1Manager.H1Result.WAIT_CONNECT:
                        m_retryConnect = true;
                        break;
                    case CH1Manager.H1Result.BAD_LINE:
                        if (m_h1manager.Connect(ref m_vnr2, 2, false, m_data.NQSAddress2, m_data.OwnTSAPforNQS2, m_data.NQS_TSAPforNQS2, ref result, m_data.ConnectionTimeOut))
                        {
                            m_nqs2Status = NQSStatus.CONNECTED;
                            //send INI telegram
                            IniTelegram init = new IniTelegram(m_idCounter, m_nqs1Messages++);
                            m_idCounter += 2;
                            SendTelegram(m_vnr2, init, "init2");
                        }
                        else
                        {
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
            if (m_nqs1Status == NQSStatus.GO || m_nqs1Status == NQSStatus.INITIALISED)
            {
                if (m_acknowledgements["live1"] == false)
                {
                    m_retryConnect = true;
                    m_nqs1Status = NQSStatus.DISCONNECTED;
                }
            }

            if (m_nqs2Status == NQSStatus.GO || m_nqs2Status == NQSStatus.INITIALISED)
            {
                if (m_acknowledgements["live2"] == false)
                {
                    m_retryConnect = true;
                    m_nqs2Status = NQSStatus.DISCONNECTED;
                }
            }

            if (m_nqs1Status == NQSStatus.GO || m_nqs1Status == NQSStatus.INITIALISED)
            {
                m_acknowledgements["live1"] = false;
                SendTelegram(m_vnr1, new LiveTelegram(m_idCounter, m_nqs1Messages++), "live1");
                m_idCounter += 2;
            }
            if (m_nqs2Status == NQSStatus.GO || m_nqs2Status == NQSStatus.INITIALISED)
            {
                m_acknowledgements["live2"] = false;
                SendTelegram(m_vnr2, new LiveTelegram(m_idCounter, m_nqs2Messages++), "live2");
                m_idCounter += 2;
            }
            if (m_nqs1Status == NQSStatus.GO || m_nqs1Status == NQSStatus.INITIALISED
                || m_nqs2Status == NQSStatus.GO || m_nqs2Status == NQSStatus.INITIALISED)
                m_liveTimer.Change(TimeSpan.FromMinutes(5.0), TimeSpan.Zero);
        }

        private bool SendTelegram(ushort vnr, AlunorfTelegram telegram)
        {
            return SendTelegram(vnr, telegram, null);
        }

        private bool SendTelegram(ushort vnr, AlunorfTelegram telegram,string id)
        {
            CH1Manager.H1Result result = CH1Manager.H1Result.ALL_CLEAR;
            if (m_h1manager.SendNoPoll(vnr, ref result, telegram))
            {
                if (id != null)
                {
                    m_idToFilename[FromNonUniqueToUnique(telegram.AktId,vnr)] = id;
                    if (id != "live1" && id != "live2")
                        m_acknowledgements.Remove(id);
                }
                return true;
            }
            else
            {
                if (result == CH1Manager.H1Result.WAIT_CONNECT)
                {
                    if (vnr == m_vnr1)
                        m_nqs1Status = NQSStatus.DISCONNECTED;
                    else if (vnr == m_vnr2)
                        m_nqs2Status = NQSStatus.DISCONNECTED;
                    m_retryConnect = true;
                }
                if (telegram is QdtTelegram && result == CH1Manager.H1Result.TELEGRAM_ERROR)
                {
                    m_error = "Malformed telegram:";
                    QdtTelegram qdt = telegram as QdtTelegram;
                    if (qdt.ErrInInfo)
                        m_error += String.Format(Alunorf_sinec_h1_plugin.Properties.Resources.err_tele_info, qdt.ErrPos);
                    else
                        m_error += String.Format(Alunorf_sinec_h1_plugin.Properties.Resources.err_tele_signal, qdt.ErrPos);
                }
                else
                    m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_send + m_h1manager.LastError ?? "";
                
                return false;
            }
        }
    }

}
