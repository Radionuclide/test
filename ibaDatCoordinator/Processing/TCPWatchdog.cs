using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;
using iba.Data;

namespace iba.Processing
{
    public class TCPWatchdog
    {
        private bool m_bStopThread;
        private Thread m_thread;
        private AutoResetEvent m_waitEvent;
        private Object m_socketLock;
        private List<Socket> m_dataSockets;
        private Socket m_serverSocket;
        private System.Text.Encoder m_coder;

        public TCPWatchdog()
        {
            m_settings = new WatchDogData();
            m_bStopThread = false;
            m_thread = null;
            m_waitEvent = new AutoResetEvent(false);
            m_socketLock = new Object();
            m_dataSockets = new List<Socket>();
            m_serverSocket = null;
            m_statusString = iba.Properties.Resources.textFileStopped;
            m_coder = Encoding.GetEncoding(1252).GetEncoder();
            m_com = null;
        }

        private CommunicationObject m_com;

        public void SetCommunication(CommunicationObject com)
        {
            m_com = com;
        }

        private WatchDogData m_settings;
        public WatchDogData Settings
        {
            get 
            { 
                return m_settings; 
            }
            set
            {
                if ((m_settings == null) || !m_settings.Equals(value))
                {
                    Stop();
                    m_settings = value;
                    if (m_settings.Enabled) Start();
                }
            }
        }

        public void Stop()
        {
            if (m_thread != null)
            {
                try
                {
                    m_bStopThread = true;
                    m_waitEvent.Set();
                    m_thread.Join(TimeSpan.FromSeconds(10));
                }
                catch (Exception ex)
                {
                   if (LogData.Data.Logger.IsOpen)
                       LogData.Data.Logger.Log(Logging.Level.Exception, string.Format(iba.Properties.Resources.wdErrorStop,ex.Message));
                }
                m_thread = null;
            }
            m_serverSocket = null;
            m_dataSockets.Clear();
        }

        private void Start()
        {
            m_bStopThread = false;
            m_thread = new Thread(new ThreadStart(Run));
            m_thread.Name = "TCP/IP watchdog thread";
            m_thread.IsBackground = true;
            m_thread.Start();
        }

        private String m_statusString;
        public String StatusString
        {
            get { return m_statusString; }
        }

        private void UpdateStatusString()
        {
            if (m_settings.ActiveNode) return;
            lock (m_socketLock)
            {
                if (m_dataSockets.Count == 0)
                    m_statusString = iba.Properties.Resources.wdTcpIpListening;
                else
                {
                    m_statusString = "";
                    foreach (Socket socket in m_dataSockets)
                    {
                        IPEndPoint remoteEndPoint = socket.RemoteEndPoint as IPEndPoint;
                        m_statusString += string.Format(iba.Properties.Resources.wdTcpIpConnected + "\r\n", remoteEndPoint.Address);
                    }
                }
            }
        }

        private void OnAccept(IAsyncResult ar)
        {
            Socket lSocket = ar.AsyncState as Socket;

            try
            {
                Socket dSocket = lSocket.EndAccept(ar);
                if (m_bStopThread)
                {
                    try
                    {  //we wish to stop
                        dSocket.Shutdown(SocketShutdown.Both);
                        dSocket.Close();
                    }
                    catch
                    {
                    }
                }
            
                //Add datasocket to list
                lock (m_socketLock)
                {
                    //ibaLogger::DebugFormat(S"TCP/IP watchdog connected to {0}", dSocket->RemoteEndPoint->ToString());
                    dSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                    m_dataSockets.Add(dSocket);
                    UpdateStatusString();

                    //start listening again
                    if (m_serverSocket == lSocket)
                        lSocket.BeginAccept(new AsyncCallback(OnAccept), lSocket);
                
                    //signal a watchdog send
                    m_waitEvent.Set();
                }
            }
            catch (SocketException ex)
            {
                if (LogData.Data.Logger.IsOpen)
                    LogData.Data.Logger.Log(Logging.Level.Exception, string.Format(iba.Properties.Resources.wdErrorAccept,ex.Message));
                if (m_serverSocket == lSocket)
                    m_serverSocket = null;
            }
            catch(Exception)
            {
                //Comes here if listenSocket is closed
            }
        }

        Byte[] CreateWatchdogMessage()
        {
            string message = string.Format("{0},DatCo:", DateTime.Now.ToString("dd/MM/yy HH:mm:ss.fff"));
            string result = null;
            //todo: get function from taskmanager
            if (m_com != null)
                result = m_com.Manager.GetStatusForWatchdog();
            else
                result = TaskManager.Manager.GetStatusForWatchdog();
            message += result;

            Byte[] data = new Byte[message.Length];
            m_coder.GetBytes(message.ToCharArray(), 0, message.Length, data, 0, true);
            return data;
        }

        private void Run()
        {
            Byte[] buffer = new Byte[65536];
            bool bConnected = false;
            m_serverSocket = null;

            while (!m_bStopThread)
            {
                DateTime start = DateTime.Now;
                if (!bConnected || m_serverSocket == null)
                {
                    try
                    {
                        if (m_serverSocket == null)
                            m_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                        if (m_settings.ActiveNode)
                        {
                            //IPHostEntry entry = Dns.Resolve(m_settings.Address);
                            IPHostEntry entry = Dns.GetHostEntry(m_settings.Address);
                            IPAddress address = entry.AddressList[0];
                            m_serverSocket.Connect(new IPEndPoint(address, m_settings.PortNr));
                            m_serverSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                            bConnected = true;
                            m_dataSockets.Add(m_serverSocket);
                            m_statusString = string.Format(iba.Properties.Resources.wdTcpIpConnected, address);
                        }
                        else
                        {
                            m_serverSocket.Bind(new IPEndPoint(IPAddress.Any, m_settings.PortNr));
                            m_serverSocket.Listen(10);
                            m_serverSocket.BeginAccept(new AsyncCallback(OnAccept), m_serverSocket);
                            bConnected = true;
                            m_statusString = iba.Properties.Resources.wdTcpIpListening;
                        }
                    }
                    catch (Exception ex)
                    {
                       m_statusString = string.Format(iba.Properties.Resources.wdErrorConnect, ex.Message);
                       if (LogData.Data.Logger.IsOpen)
                           LogData.Data.Logger.Log(Logging.Level.Exception, m_statusString);
                    }
                }

                bool bReconnectNeeded = false;
                if (bConnected && m_dataSockets.Count > 0)
                {
                    Byte[] data = CreateWatchdogMessage();

                    lock (m_socketLock)
                    {
                        for (int i = m_dataSockets.Count-1; i >= 0; i--)
                        {
                            Socket dataSocket = m_dataSockets[i];
                            try
                            {
                                dataSocket.Send(data);
                                while (dataSocket.Available > 0)
                                    dataSocket.Receive(buffer); //other side shouldn't send, disregard in buffer
                            }
                            catch (Exception ex)
                            {
                                string errMessage = string.Format(iba.Properties.Resources.wdErrorSend,ex.Message);
                                if (LogData.Data.Logger.IsOpen)
                                    LogData.Data.Logger.Log(Logging.Level.Exception, errMessage);
                                m_dataSockets.RemoveAt(i);
                                try
                                {
                                    dataSocket.Close();
                                }
                                catch{}

                                if (m_settings.ActiveNode)
                                {
                                    m_serverSocket = null;
                                    bConnected = false;
                                    bReconnectNeeded = true;
                                    m_statusString = errMessage;
                                }
                                else
                                    UpdateStatusString();
                            }
                        }
                    }
                }
                int remtime = (int)(Math.Min(m_settings.CycleTime * 1000, m_settings.CycleTime * 1000 - (DateTime.Now.Ticks - start.Ticks) / 10000));
                if (!bReconnectNeeded && remtime > 0)
                    m_waitEvent.WaitOne(remtime, true);
            }

            if(m_serverSocket != null)
            {
                if(m_serverSocket.Connected)
                    m_serverSocket.Shutdown(SocketShutdown.Both);
                m_serverSocket.Close();
                m_serverSocket = null;
            }

            foreach(Socket sock in m_dataSockets) 
            {
                if(sock.Connected)
                {
                    sock.Shutdown(SocketShutdown.Both);
                    sock.Close();
                }
            }
            m_statusString = iba.Properties.Resources.textFileStopped;
        }
    }
}
