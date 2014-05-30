using System;
using System.Collections.Generic;
using System.Text;

using iba;

namespace Alunorf_sinec_h1_plugin
{
    class TcpIPManager : IProtocolManager
    {
        #region IProtocolManager Members

        public bool DisconnectAll()
        {
            throw new NotImplementedException();
        }

        public bool FinishRead(ushort vnr, ITelegram telegram)
        {
            throw new NotImplementedException();
        }

        public bool GetConnectionStatus(ushort vnr, ref H1Result result)
        {
            throw new NotImplementedException();
        }

        public bool GetReadStatus(ushort vnr, ref H1Result result)
        {
            throw new NotImplementedException();
        }

        public bool GetSendStatus(ushort vnr, ref H1Result result)
        {
            throw new NotImplementedException();
        }

        public string LastError
        {
            get { throw new NotImplementedException(); }
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

        internal bool SetPort(int p)
        {
            throw new NotImplementedException();
        }

        internal bool Connect(ref ushort m_vnr1, ref H1Result result, int p)
        {
            throw new NotImplementedException();
        }
    }
}
