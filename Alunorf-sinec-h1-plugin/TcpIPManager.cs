using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using iba;
using System.Diagnostics;

namespace Alunorf_sinec_h1_plugin
{
    class TcpIPManager : IProtocolManager
    {
        private Socket[] m_dataSockets;
        private Socket[] m_serverSockets;
        private H1Result[] connectionStates;
        private H1Result[] readStates;
        private H1Result[] writeStates;

        private Object m_socketLock;
        public TcpIPManager()
        {
            m_serverSockets = new Socket[2];
            m_dataSockets = new Socket[2];
            m_socketLock = new Object();
            ClearStates();
        }

        private void ClearStates()
        {
            connectionStates = new H1Result[2] { H1Result.BAD_LINE, H1Result.BAD_LINE };
            readStates = new H1Result[2] { H1Result.NO_REQUEST, H1Result.NO_REQUEST };
            writeStates = new H1Result[2] { H1Result.NO_REQUEST, H1Result.NO_REQUEST };
        }

        public int PortNr1;
        public int PortNr2;


        #region IProtocolManager Members

        public bool FinishRead(ushort vnr, ITelegram telegram)
        {
            throw new NotImplementedException();
        }

        public void GetConnectionStatus(ushort vnr, ref H1Result result)
        {
            result =  (vnr == PortNr1 ? connectionStates[0] : connectionStates[1]);
            if (result == H1Result.OPERATING_SYSTEM_ERROR)
            { //it's reported, allow reconnect
                if (vnr == PortNr1)
                    connectionStates[0] = H1Result.BAD_LINE;
                else
                    connectionStates[1] = H1Result.BAD_LINE;
            }
        }

        //only up to date if read is started !
        public void GetReadStatus(ushort vnr, ref H1Result result)
        {
            result = (vnr == PortNr1 ? readStates[0] : readStates[1]);
        }


        //only up to date if send is started !
        public void GetSendStatus(ushort vnr, ref H1Result result)
        {
            result = (vnr == PortNr1 ? writeStates[0] : writeStates[1]);
        }

        private string m_lastError;
        public string LastError
        {
            get { 
                string res = m_lastError; 
                m_lastError = "";
                return res; 
            }
        }

        private string debugMessage;

        public string DebugMessage
        {
            get
            {
                string res = debugMessage;
                debugMessage = "";
                return res;
            }
            set { debugMessage = value; }
        }


        public bool SendNoPoll(ushort vnr, ref H1Result result, ITelegram telegram)
        {
            throw new NotImplementedException();
        }

        public bool SetSendTimeout(int timeout)
        {
            throw new NotImplementedException();
        }

        public bool StartRead(ushort vnr, ref H1Result result)
        {
            throw new NotImplementedException();
        }

        public bool StartSend(ushort vnr, ref H1Result result, ITelegram telegram)
        {
            throw new NotImplementedException();
        }

        public bool StoreBlockedBytes(ushort vnr)
        {
            throw new NotImplementedException();
        }
        #endregion

        private bool m_bStop;

        public H1Result PassiveConnect(int portNr, int index)
        {
            m_bStop = false;
            ClearStates();
            Debug.Assert(portNr == (index == 0 ? PortNr1 : PortNr2));
            if (m_serverSockets[index] == null) //setup connection
            {
                try
                {
                    m_serverSockets[index] = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                    m_serverSockets[index].Bind(new IPEndPoint(IPAddress.Any, portNr));
                    m_serverSockets[index].Listen(10);
                    m_serverSockets[index].BeginAccept(new AsyncCallback(OnAcceptConnection), m_serverSockets[index]);
                    return connectionStates[index] = H1Result.WAIT_CONNECT; 
                }
                catch (Exception ex)
                {
                    Disconnect(index);
                    m_lastError = ex.Message;
                    return connectionStates[index] = H1Result.OPERATING_SYSTEM_ERROR;
                }
            }
            else //check connection
            {
                if (m_dataSockets[index] == null)
                    return connectionStates[index] = H1Result.WAIT_CONNECT; //still waiting to connect;
                else 
                    return connectionStates[index] = H1Result.ALL_CLEAR; //connected in the mean time;
            }
        }

        private void OnAcceptConnection(IAsyncResult ar)
        {
            Socket lSocket = ar.AsyncState as Socket;
            int index = -1;
            if (lSocket == m_serverSockets[0])
                index = 0;
            if (lSocket == m_serverSockets[1])
                index = 1;
            try
            {
                Socket dSocket = lSocket.EndAccept(ar);
                if (m_bStop || index == -1)
                {
                    try
                    {  //we wish to stop
                        dSocket.Shutdown(SocketShutdown.Both);
                        dSocket.Close();
                    }
                    catch
                    {
                    }
                    return;
                }
                
                dSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.NoDelay, true);
                m_dataSockets[index]= dSocket;
                connectionStates[index] = H1Result.ALL_CLEAR;

            }
            catch (SocketException ex) //error on accept
            {
                if (m_bStop)
                    return;
                
                m_lastError = ex.Message;
                if (index >= 0)
                {
                    Disconnect(index);
                }
            }
            catch (Exception)
            {
                //Comes here if listenSocket is closed, ignore this
            }
        }
        
        public bool DisconnectAll()
        {
            m_bStop = true;
            for (int i = 0; i < 2; i++)
            {
                Disconnect(i);
                connectionStates[i] = H1Result.BAD_LINE;
            }
            return true;
        }

        public bool Disconnect(int index)
        {
            if (m_dataSockets[index]!= null)
            {
                try
                {
                    m_dataSockets[index].Shutdown(SocketShutdown.Both);
                    m_dataSockets[index].Close();
                }
                catch
                {
                }
                m_dataSockets[index] = null;
            }
            if (m_serverSockets[index] != null)
            {
                try
                {
                    m_serverSockets[index].Close();
                }
                catch
                {
                }
                m_serverSockets[index] = null;
            }
            connectionStates[index] = H1Result.OPERATING_SYSTEM_ERROR; 
            return true;
        }
    }
}
