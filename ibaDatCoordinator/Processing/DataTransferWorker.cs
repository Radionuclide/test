using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Interceptors;
using iba.Data;
using iba.Logging;
using iba.Processing.IbaGrpc;
using iba.Remoting;
using Messages.V1;

namespace iba.Processing
{
    internal class DataTransferWorker
    {
        private DataTransferData _data = new DataTransferData();
        private DataTransferImpl _dataTransferImpl;
        private readonly ClientManager _clientManager;
        private const string HOST = "localhost";
        public string Status { get; set; }
        public bool IsPortTextBoxEnabled { get; set; }
        public bool IsSelectRootPathBtnEnabled { get; set; }
        public bool IsSelectCertificateBtnEnabled { get; set; }

        private Server m_server;

        private static DataTransferWorker _instance;
        public static DataTransferWorker GetInstance()
        {
            return _instance ?? (_instance = new DataTransferWorker());
        }

        private DataTransferWorker()
        {
            _clientManager = new ClientManager();
            _dataTransferImpl = new DataTransferImpl(ClientManager);
        }

        public DataTransferData DataTransferData
        {
            get => _data;
            set
            {
                if (value != null)
                {
                    _data = value;
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
                _dataTransferImpl.Data = _data;
                m_server = CreateNewServer();
                m_server.Start();
                
                SetStatus(false);

                LogData.Data.Logger.Log(Level.Info, $"Data Transfer Service started on port: {_data.Port}");
            }
            catch (Exception e)
            {
                LogData.Data.Logger.Log(Level.Exception, $"Data Transfer Service could not start: {e.Message}");
            }
        }

        private  void SetStatus(bool isControlEnabled)
        {
            IsPortTextBoxEnabled = isControlEnabled;
            IsSelectCertificateBtnEnabled = isControlEnabled;
            IsSelectRootPathBtnEnabled = isControlEnabled;
            Status = isControlEnabled ? "Server not started" : "Server started";

        }

        public (string status, bool IsPortTextBoxEnabled, bool IsSelectRootPathBtnEnabledbool, bool IsSelectCertificateBtnEnabled) 
            GetStatus()
        { 
            return (Status, IsPortTextBoxEnabled, IsSelectRootPathBtnEnabled, IsSelectCertificateBtnEnabled);
        }


        public void StopServer()
        {
            try
            {
                if (m_server == null) return;

                m_server.ShutdownAsync().Wait();

                SetStatus(true);

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
                Ports = { new ServerPort(HOST, _data.Port, ServerCredentials.Insecure) },
            };
        }

        public void Init()
        {
            if (_data.IsServerEnabled)
            {
                StartServer();
            }
            else
            {
                SetStatus(true);
            }
        }
    }
}
