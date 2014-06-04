using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Net;
using System.Net.Sockets;

using iba;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Alunorf_sinec_h1_plugin
{
    class ReceiveState
	{
		public ReceiveState(Byte[] buffer, Socket s, int nqsindex)
		{
            recvBuffer = buffer;
            socket = s;
            recvOffset = 0;
            index = nqsindex;
		}
		public Socket socket;
        public Byte[] recvBuffer;
        public int recvOffset;
        public int index;
	};

    class TcpIPManager : IProtocolManager
    {
        const int RECBUFFERLENGTH = 2048*20; //20 'small messages
        const int SENDBUFFERLENGTH = 16384; //twice as large as previously
        private Socket[] m_dataSockets;
        private Socket[] m_serverSockets;
        private H1Result[] connectionStates;
        private H1Result[] readStates;
        private H1Result[] sendStates;
        private byte[][] readBuffer;
        private byte[][] writeBuffer;
        private int[] bytesSent;
        private int[] bytesSendRequired;

        public string[] ipadress;

        private Queue<WrapperTelegram>[] ReceivedMessages;
        private Object m_socketLock;
        public TcpIPManager()
        {
            m_serverSockets = new Socket[2];
            m_dataSockets = new Socket[2];
            m_socketLock = new Object();
            readBuffer = new byte[2][];
            readBuffer[0] = new byte[RECBUFFERLENGTH]; //should be enough
            readBuffer[1] = new byte[RECBUFFERLENGTH]; //should be enough
            writeBuffer = new byte[2][];
            writeBuffer[0] = new byte[SENDBUFFERLENGTH]; //should be enough
            writeBuffer[1] = new byte[SENDBUFFERLENGTH]; //should be enough
            ReceivedMessages = new Queue<WrapperTelegram>[2];
            ReceivedMessages[0] = new Queue<WrapperTelegram>();
            ReceivedMessages[1] = new Queue<WrapperTelegram>();
            bytesSent = new int[2] { 0, 0 };
            bytesSendRequired = new int[2] { 0, 0 };
            ipadress = new string[2];
            sendTimeOut = 1000; //1 sec
            ClearStates();
        }

        private void ClearStates()
        {
            connectionStates = new H1Result[2] { H1Result.BAD_LINE, H1Result.BAD_LINE };
            readStates = new H1Result[2] { H1Result.NO_REQUEST, H1Result.NO_REQUEST };
            sendStates = new H1Result[2] { H1Result.ALL_CLEAR, H1Result.ALL_CLEAR };
        }

        public int PortNr1;
        public int PortNr2;

        private int PortToIndex(int port)
        {
            return port == PortNr1 ? 0 : 1;
        }

        #region IProtocolManager Members

        public bool FinishRead(ushort vnr, ref ITelegram telegram)
        {
            int index = PortToIndex(vnr);
            lock (ReceivedMessages[index])
            {
                if (ReceivedMessages[index].Count > 0)
                {
                    telegram = ReceivedMessages[index].Dequeue();
                    return telegram != null;
                }
                else
                    return false;
            }
        }

        public void GetConnectionStatus(ushort vnr, ref H1Result result)
        {
            result = connectionStates[PortToIndex(vnr)];
            if (result == H1Result.OPERATING_SYSTEM_ERROR)
            { //it's reported, allow reconnect
                if (vnr == PortNr1)
                    connectionStates[PortToIndex(vnr)] = H1Result.BAD_LINE;
                else
                    connectionStates[1] = H1Result.BAD_LINE;
            }
        }

        public void GetReadStatus(ushort vnr, ref H1Result result)
        {
            result = readStates[PortToIndex(vnr)];
            //don't reset OPERATION_ERROR after being reported, new Connect will do so
        }


        public void GetSendStatus(ushort vnr, ref H1Result result)
        {
            result = sendStates[PortToIndex(vnr)];
            //don't reset OPERATION_ERROR after being reported, new Connect will do so
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

        //private string debugMessage;

        //public string DebugMessage
        //{
        //    get
        //    {
        //        string res = debugMessage;
        //        debugMessage = "";
        //        return res;
        //    }
        //    set { debugMessage = value; }
        //}

        public bool StartRead(ushort vnr, ref H1Result result)
        {
            throw new NotImplementedException();
        }

        public bool StoreBlockedBytes(ushort vnr)
        {
            throw new NotImplementedException(); //we don't do block in tcpip
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
                dSocket.SendTimeout = sendTimeOut;
                ipadress[index] = (dSocket.RemoteEndPoint as IPEndPoint).Address.ToString();
                m_dataSockets[index]= dSocket;
                connectionStates[index] = H1Result.ALL_CLEAR;
                //start reads
                ReceiveState state = new ReceiveState(readBuffer[index],m_dataSockets[index],index);
                m_dataSockets[index].BeginReceive(state.recvBuffer, state.recvOffset, state.recvBuffer.Length - state.recvOffset, SocketFlags.None, new AsyncCallback(OnReceivedMessage), state);
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

        public unsafe bool StartSend(ushort vnr, ref H1Result result, ITelegram telegram)
        {
            int index = PortToIndex(vnr);
            Debug.Assert(sendStates[index] == H1Result.ALL_CLEAR);
            sendStates[index] = H1Result.ALREADY_RUNNING;
            //write entire message in buffer
            bytesSent[index] = 0;
            bytesSendRequired[index] = (int)( telegram.ActSize + 2); //two extra bytes for length
            fixed (byte* pBuffer = &(writeBuffer[index][0]))
            {
                if (!telegram.WriteToBuffer(new H1ByteStream(new IntPtr((void*) (pBuffer+2)),telegram.ActSize,true)))
                {
                    sendStates[index] = H1Result.ALL_CLEAR; //clear for next request
                    result = H1Result.TELEGRAM_ERROR;
                    return false;
                }
                *((UInt16*)(pBuffer)) = (UInt16)(bytesSendRequired[index]);
            }

            try
            {
                sendStates[index] = H1Result.WAIT_SEND;
                m_dataSockets[index].BeginSend(writeBuffer[index], 0, bytesSendRequired[index], 0, new AsyncCallback(OnMessageSend), m_dataSockets[index]);
            }
            catch (Exception ex)
            {
                sendStates[index] = result = H1Result.OPERATING_SYSTEM_ERROR;
                Disconnect(index);
                m_lastError = ex.Message;
                return false;
            }
            return true;
        }

        private void OnMessageSend(IAsyncResult ar)
        {
            Socket dSocket = ar.AsyncState as Socket;
            int index = -1;
            if (dSocket == m_dataSockets[0])
                index = 0;
            if (dSocket == m_dataSockets[1])
                index = 1;
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
            try
            {
                Debug.Assert(sendStates[index] == H1Result.WAIT_SEND);
                bytesSent[index] += dSocket.EndSend(ar);
                if (bytesSent[index] >= bytesSendRequired[index])
                {
                    sendStates[index] = H1Result.ALL_CLEAR; //finished
                }
                else //keep sending
                {
                    m_dataSockets[index].BeginSend(writeBuffer[index], bytesSent[index], bytesSendRequired[index] - bytesSent[index], 0, new AsyncCallback(OnMessageSend), m_dataSockets[index]);
                }
            }
            catch (Exception ex)
            {
                sendStates[index] = H1Result.OPERATING_SYSTEM_ERROR;
                Disconnect(index);
                m_lastError = ex.Message;
                return;
            }
        }

        private int sendTimeOut;

        public bool SetSendTimeout(int timeout)
        {
            sendTimeOut = 1000*timeout;
            try
            {
                if (m_dataSockets[0] != null)
                  m_dataSockets[0].SendTimeout = sendTimeOut;
            }
            catch
            { 
            }
            try
            {
                if (m_dataSockets[1] != null)
                    m_dataSockets[1].SendTimeout = sendTimeOut;
            }
            catch
            {
            }
            return true;
        }

        public unsafe bool SendNoPoll(ushort vnr, ref H1Result result, ITelegram telegram)
        {
            int index = PortToIndex(vnr);
            Debug.Assert(sendStates[index] == H1Result.ALL_CLEAR);
            sendStates[index] = H1Result.ALREADY_RUNNING;
            //write entire message in buffer
            bytesSent[index] = 0;
            bytesSendRequired[index] = (int)(telegram.ActSize + 2); //two extra bytes for length
            fixed (byte* pBuffer = &(writeBuffer[index][0]))
            {
                if (!telegram.WriteToBuffer(new H1ByteStream(new IntPtr((void*)(pBuffer + 2)), telegram.ActSize, true)))
                {
                    sendStates[index] = H1Result.ALL_CLEAR; //clear for next request
                    result = H1Result.TELEGRAM_ERROR;
                    return false;
                }
                *((UInt16*)(pBuffer)) = (UInt16)(bytesSendRequired[index]);
            }

            try
            {
                sendStates[index] = result = H1Result.WAIT_SEND;
                while (bytesSent[index] < bytesSendRequired[index])
                {
                    bytesSent[index] += m_dataSockets[index].Send(writeBuffer[index], bytesSent[index], bytesSendRequired[index] - bytesSent[index], 0);
                    Thread.Sleep(100);
                }
                sendStates[index] = result =  H1Result.ALL_CLEAR;
            }
            catch (Exception ex)
            {
                sendStates[index] = result = H1Result.OPERATING_SYSTEM_ERROR;
                Disconnect(index);
                m_lastError = ex.Message;
                return false;
            }
            return true;
        }

        private unsafe void OnReceivedMessage(IAsyncResult ar)
	    {
            if (m_bStop) return;
            int index = -1;
		    try
		    {
			    ReceiveState state = ar.AsyncState as ReceiveState;
			    int recvLength = state.socket.EndReceive(ar);
                index = state.index;
			    if(recvLength == 0)
			    {
				    Disconnect(state.index);
				    return;
			    }
			    recvLength += state.recvOffset;

			    //if(readsBusy == 0)
			    //	return;

                fixed (byte* pBuffer = &(state.recvBuffer[0]))
                {
			        Byte* pStart = pBuffer;
                    Byte* pBuf = pBuffer;
			        while(recvLength >= 3) //3 bytes, length (two bytes) + Magic byte 97
			        {
				        //Check if we have received the complete message
                        //CODREQ_HEADER* pMsg = (CODREQ_HEADER*) pBuf;
                        UInt32 msgLength = (UInt32)(*((UInt16*)pBuf));
                        if(recvLength < msgLength)
                        {
                            //Not enough data available yet

                            //Move data to front of buffer
                            if(pBuf != pStart)
                                MoveMemory(new IntPtr(pStart), new IntPtr(pBuf), recvLength);
                            break;
                        }

                        Byte magic = pBuf[3];
                        if (magic != 97)
                        {
                            lock (ReceivedMessages[state.index])
                            {
                                ReceivedMessages[state.index].Enqueue(null);
                            }
                        }
                        else
                        {
                            WrapperTelegram telegram = new WrapperTelegram();
                            if (telegram.ReadFromBuffer(new H1ByteStream(new IntPtr((void*)(pBuf+2)),msgLength-2,true)))
                            {
                                 lock (ReceivedMessages[state.index])
                                {
                                    ReceivedMessages[state.index].Enqueue(telegram);
                                }
                            }
                            else
                            {
                                lock (ReceivedMessages[state.index])
                                {
                                    ReceivedMessages[state.index].Enqueue(null);
                                }
                            }
                        }
                        //if((pMsg->byCmdType & CODREQ_MSG_TYPE_RESP_FLAG) != CODREQ_MSG_TYPE_RESP_FLAG)
                        //    CodesysLogger::LogError(nullptr, String::Format("Received message without response flag from {0}: {1}", ToString(), pMsg->byCmdType.ToString("X2")));
                        //else
                        //{
                        //    msgRecvCount++;
                        //    Byte msgType = pMsg->byCmdType & ~CODREQ_MSG_TYPE_RESP_FLAG;
                        //    if(msgType == CODREQ_MSG_TYPE_WATCHDOG)
                        //        OnRecvWatchdog(pMsg);
                        //    else if((msgType == CODREQ_MSG_TYPE_SYMBOL_REQUEST) || (msgType == CODREQ_MSG_TYPE_ADDRESS_REQUEST))
                        //        OnRecvRequest(pMsg);
                        //    else
                        //        CodesysLogger::LogError(nullptr, String::Format("Received unknown message from {0}: {1}", ToString(), pMsg->byCmdType.ToString("X2")));
                        //}

				        //Advance to next command
				        pBuf = pBuf + msgLength;
				        recvLength -= (int)msgLength;				
			        }
                }

			    state.recvOffset = Math.Max(0, Math.Min(recvLength, state.recvBuffer.Length-1));

			    if(state.recvBuffer.Length == state.recvOffset)
				    throw new Exception("Receive buffer is too small");

			    //Receive next response
			    state.socket.BeginReceive(state.recvBuffer, state.recvOffset, state.recvBuffer.Length - state.recvOffset, SocketFlags.None, 
				    new AsyncCallback(OnReceivedMessage), state);
		    }
		    catch(Exception ex)
		    {
                m_lastError = ex.Message;
			    if (index !=-1)
                    Disconnect(index);
		    }
	    }
        [DllImport("Kernel32.dll", EntryPoint = "RtlMoveMemory", SetLastError = false)]
        static extern void MoveMemory(IntPtr dest, IntPtr src, int size);
    }
}

