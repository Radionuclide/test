using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Grpc.Core;
using iba.Data;
using iba.Logging;
using iba.Processing.IbaGrpc;
using iba.Properties;
using iba.Utility;
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
                await StopServer();
                
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
            if (m_server == null) return;

            try
            {
                await m_server.ShutdownAsync();

                SetStatus(false);

                OnUpdateServerStatus?.Invoke(Status);

                LogData.Data.Logger.Log(Level.Info, "Data Transfer Service stopped");
            }
            catch (Exception e)
            {
                LogData.Data.Logger.Log(Level.Exception, $"Data Transfer Service could not stop: {e.Message}");
            }
        }

        private Server CreateNewServer()
        {
            var server = new Server();

            var sslCredentials = CreateChannelCredentials();

            server.Services.Add(DataTransfer.BindService(TransferImpl));
            server.Ports.Add(HOST, _data.Port, sslCredentials);

            return server;
        }

        private ServerCredentials CreateChannelCredentials()
        {
            var serverCert = TaskManager.Manager.CertificateManager.GetCertificate(DataTransferData.ServerCertificateThumbprint);

            if (serverCert == null)
                return ServerCredentials.Insecure;
            if (!serverCert.Trusted)
                throw new InvalidOperationException(Resources.DataTransferServerErrorCertNotTrusted);

            var certificate = CertificateExtractor.GetCertificate(serverCert.Certificate);

            var exportableCert = new X509Certificate2(serverCert.Certificate.Export(X509ContentType.Pkcs12, ""), "",
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);

            var privateKey = CertificateExtractor.GetPrivateKey(exportableCert);

            var keypair = new KeyCertificatePair(certificate, privateKey);

            return new SslServerCredentials(new List<KeyCertificatePair>() { keypair });
        }

        public async Task Init()
        {
            if (_data.IsServerEnabled)
            {
                await StartServer();
            }
            else
            {
                Status = Resources.DatatransferServerNotStarted;
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
