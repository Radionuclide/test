using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using iba.Data;
using iba.Logging;
using iba.Processing.IbaGrpc;
using iba.Remoting;
using Messages.V1;

namespace iba.Processing
{
    internal class DataTransferWorker
    {
        private DataTransferData _dataTransferData = new DataTransferData();
        private DataTransferImpl _dataTransferImpl;
        private readonly ClientManager _clientManager;
        
        private const string HOST = "localhost";
        private Server m_server;
        private int m_port;

        public DataTransferWorker()
        {
            _clientManager = new ClientManager();
            TransferImpl = new DataTransferImpl(ClientManager);
        }


        public int Port
        {
            get => m_port;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                m_port = value;
            }
        }

        public DataTransferData DataTransferData
        {
            get => _dataTransferData;
            set
            {
                if (value != null)
                {
                    _dataTransferData = value;
                }
            }
        }

        public DataTransferImpl TransferImpl
        {
            get => _dataTransferImpl;
            set => _dataTransferImpl = value;
        }

        public ClientManager ClientManager => _clientManager;

        public void StartServer()
        {
            try
            {
                m_server = CreateNewServer();
                m_server.Start();
                LogData.Data.Logger.Log(Level.Info, "Data Transfer Service started");
            }
            catch (Exception e)
            {
                LogData.Data.Logger.Log(Level.Exception, $"Data Transfer Service could not start: {e.Message}");
            }
        }


        public void StopServer()
        {
            try
            {
                if (m_server == null) return;

                m_server.ShutdownAsync().Wait();
                LogData.Data.Logger.Log(Level.Info, "Data Transfer Service stopped");

            }
            catch (Exception e)
            {
                LogData.Data.Logger.Log(Level.Exception, $"Data Transfer Service could not stop: {e.Message}");
            }
        }

        private Server CreateNewServer()
        {
            return new Server
            {
                Services = { DataTransfer.BindService(TransferImpl) },
                Ports = { new ServerPort(HOST, Port, ServerCredentials.Insecure) },
            };
        }

        public void Init()
        {
            Port = _dataTransferData.Port;
            
            if (_dataTransferData.ServerEnabled)
            {
                StartServer();
            }
        }
    }
}
