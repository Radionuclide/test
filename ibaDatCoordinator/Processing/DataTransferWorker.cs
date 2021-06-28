using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using iba.Data;
using iba.Logging;
using iba.Processing.IbaGrpc;
using Messages.V1;

namespace iba.Processing
{
    internal class DataTransferWorker
    {
        private DataTransferData _dataTransferData = new DataTransferData();
        
        private const string HOST = "localhost";
        private Server m_server;
        private int m_port;

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
                Services = { DataTransfer.BindService(new DataTransferImpl()) },
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
