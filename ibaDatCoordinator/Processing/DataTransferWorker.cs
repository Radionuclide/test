using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraWaitForm;
using Grpc.Core;
using Grpc.Core.Interceptors;
using iba.Data;
using iba.Logging;
using iba.Processing.IbaGrpc;
using iba.Properties;
using iba.Remoting;
using Messages.V1;

namespace iba.Processing
{
    internal class DataTransferWorker
    {
        private DataTransferData _data = new DataTransferData();
        private DataTransferImpl _dataTransferImpl;
        private static readonly string HOST = Dns.GetHostName();
        public string Status { get; set; } = string.Empty;

        private Server m_server;

        private static DataTransferWorker _instance;
        public static DataTransferWorker GetInstance()
        {
            return _instance ?? (_instance = new DataTransferWorker());
        }

        private DataTransferWorker()
        {
            _dataTransferImpl = new DataTransferImpl();
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

        private void SetStatus(bool status)
        {
            Status = status ? Resources.DatatransferServerStarted : Resources.DatatransferServerNotStarted;
        }

        public async Task StartServer()
        {
            try
            {
                var deadline = DateTime.UtcNow.AddSeconds(5);

                var isRunning = await IsReadyAsync(deadline);

                if (isRunning)
                {
                    await StopServer();
                }

                m_server = CreateNewServer();
                m_server.Start();
                
                SetStatus(true);

                OnUpdateServerStatus?.Invoke(Status);

                LogData.Data.Logger.Log(Level.Info, $"Data Transfer Service started on port: {_data.Port}");
            }
            catch (Exception e)
            {
                LogData.Data.Logger.Log(Level.Exception, $"Data Transfer Service could not start: {e.Message}");
            }
        }

        public async Task StopServer()
        {
            try
            {
                if (m_server == null) return;

                var deadline = DateTime.UtcNow.AddSeconds(3);

                var isRunning = await IsReadyAsync(deadline);

                if (isRunning)
                {
                    await m_server.ShutdownAsync();
                }

                SetStatus(false);

                OnUpdateServerStatus?.Invoke(Status);

                LogData.Data.Logger.Log(Level.Info, "Data Transfer Service stopped");
            }
            catch (Exception e)
            {
                LogData.Data.Logger.Log(Level.Exception, $"Data Transfer Service could not stop: {e.Message}");
            }
        }

        public async Task<bool> IsReadyAsync(DateTime? deadline)
        {
            var client = new GrpcClient(Dns.GetHostName(), _data.Port.ToString());

            try
            {
                await client.TestConnectAsync(new Empty(), deadline: deadline);
            }
            catch (TaskCanceledException)
            {
                return false;
            }
            catch (Exception)
            {
                return false;
            }

            var isReadyAsync = client.channel.State == ChannelState.Ready;

            await client.channel.ShutdownAsync();
            
            return isReadyAsync;
        }

        private Server CreateNewServer()
        {
            return new Server
            {
                Services = { DataTransfer.BindService(TransferImpl) },
                Ports = { new ServerPort(HOST, _data.Port, ServerCredentials.Insecure) },
            };
        }

        public async Task Init()
        {
            if (_data.IsServerEnabled)
            {
                await StartServer();
            }
            else
            {
                OnUpdateServerStatus?.Invoke(Status);
            }
        }

        public void SetDiagnosticInfoCallback(Action<DiagnosticsData> updateDiagnosticInfo)
        {
            _dataTransferImpl.ClientManager.UpdateDiagnosticInfoCallback += updateDiagnosticInfo;
        }

        public event Action<string> OnUpdateServerStatus;

        public void SetUpdateStatusCallback(Action<string> statusCallback)
        {
            OnUpdateServerStatus += statusCallback;
        }

        public List<DiagnosticsData> GetAllClients()
        {
            return TransferImpl.ClientManager.GetAllClients();
        }
    }
}
