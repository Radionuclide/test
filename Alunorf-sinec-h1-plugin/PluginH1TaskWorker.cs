using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using iba;
using iba.Plugins;

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
            m_data = newtask as PluginH1Task;
            return true;
        }

        public bool ExecuteTask(string datFile)
        {
            lock (m_acknowledgements)
            {
                if (m_acknowledgements.ContainsKey(datFile))
                {
                    if (m_acknowledgements[datFile]) //positively acknowledged in the mean time;
                    {
                        m_acknowledgements.Remove(datFile);
                        m_idToFilename.Remove(datFile);
                        return true;
                    }
                    else //negatively acknowledged, regard the datfile as never send
                    {
                        m_acknowledgements.Remove(datFile);
                        m_idToFilename.Remove(datFile);
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
                    qdt = new QdtTelegram(m_idCounter, m_nqs1Messages++);
                }
                else if (m_nqs1Status == NQSStatus.GO)
                {
                    qdt = new QdtTelegram(m_idCounter, m_nqs2Messages++);
                }
                else
                {
                    m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_nogo;
                    return false;
                }
                //TODO: fill in data in qdt diagram


                m_idCounter += 2;
                m_messageQueue.Add(qdt);            
            }

            //wait until acknowledged
            m_waitSendEvent = new AutoResetEvent(false);
            m_waitSendEvent.WaitOne(1000 * (m_data.SendTimeOut + m_data.AckTimeOut), true);

            if (m_acknowledgements.ContainsKey(datFile))
            {
                if (m_acknowledgements[datFile])
                {
                    m_acknowledgements.Remove(datFile);
                    m_idToFilename.Remove(datFile);
                    return true;
                }
                else
                {
                    m_acknowledgements.Remove(datFile);
                    m_idToFilename.Remove(datFile);
                    m_error = m_nak_error;
                    return false;
                }
            }
            else
            {
                m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_noAck;
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
        private string m_nak_error;
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
                        if (m_nqs1Status == NQSStatus.GO)
                        {
                            if (!SendTelegram(m_vnr1, m_messageQueue[0], m_idToFilename[m_messageQueue[0].AktId]))
                            {
                                m_nak_error = m_error;
                                m_acknowledgements[m_idToFilename[m_messageQueue[0].AktId]] = false;
                                m_waitSendEvent.Set();
                            }
                        }
                        if (m_nqs1Status == NQSStatus.GO)
                        {
                            if (!SendTelegram(m_vnr2, m_messageQueue[0], m_idToFilename[m_messageQueue[0].AktId]))
                            {
                                m_nak_error = m_error;
                                m_acknowledgements[m_idToFilename[m_messageQueue[0].AktId]] = false;
                                m_waitSendEvent.Set();
                            }
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
                            m_nak_error = nak.FehlerText;
                            string id = m_idToFilename[nak.AktId];
                            m_acknowledgements[id] = false;
                            if (id == "init1" || id == "init2" )
                                m_waitConnectedEvent.Set();
                            else if (id != "live1" && id != "live2")
                                m_waitSendEvent.Set();
                        }
                        else
                        {
                            AckTelegram ack = wrap.InnerTelegram as AckTelegram;
                            if (ack != null && m_idToFilename.Contains(ack.AktId))
                            {
                                string id = m_idToFilename[ack.AktId];
                                if (id == "init1")
                                {
                                    m_nqs1Status = NQSStatus.INITIALISED;
                                }
                                else if (id == "init2")
                                {
                                    m_nqs1Status = NQSStatus.INITIALISED;
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
                                    string id = m_idToFilename[ack.AktId];
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
                                        m_nqs1Status = NQSStatus.GO;

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
                                        m_idToFilename.Remove(id);
                                    }
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
                    m_idToFilename[telegram.AktId] = id;
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
                }
                m_error = Alunorf_sinec_h1_plugin.Properties.Resources.err_send + m_h1manager.LastError ?? "";
                return false;
            }
        }

    }

}
