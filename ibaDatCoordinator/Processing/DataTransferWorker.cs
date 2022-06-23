using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Grpc.Core;
using iba.CertificateStore;
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

        public void StartServer()
        {
            try
            {
                StopServer();
                
                m_server = CreateNewServer();
                m_server.Start();
                
                SetStatus(true);

                LogData.Data.Logger.Log(Level.Info, $"Data Transfer Service started on port: {_data.Port}");
            }
            catch (Exception e)
            {
                LogData.Data.Logger.Log(Level.Exception, $"Data Transfer Service could not start: {e.Message}");
            }
        }

        public void StopServer()
        {
            if (m_server == null) return;

            try
            {
                SetStatus(false);
                m_server.ShutdownAsync();

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
            var serverCert = TaskManager.Manager.CertificateManager.GetCertificate(DataTransferData.ServerCertificateThumbprint, CertificateRequirement.None, out var _);

            if (serverCert == null)
                return ServerCredentials.Insecure;
            if (!serverCert.Trusted)
                throw new InvalidOperationException(Resources.DataTransferServerErrorCertNotTrusted);

            var certificate = CertificateExtractor.GetCertificate(serverCert.GetX509Certificate2());

            var exportableCert = new X509Certificate2(serverCert.GetX509Certificate2().Export(X509ContentType.Pkcs12, ""), "",
                X509KeyStorageFlags.Exportable | X509KeyStorageFlags.EphemeralKeySet);

            var privateKey = CertificateExtractor.GetPrivateKey(exportableCert);

            var keypair = new KeyCertificatePair(certificate, privateKey);

            return new SslServerCredentials(new List<KeyCertificatePair>() { keypair });
        }

        public void Init()
        {
            if (_data.IsServerEnabled)
            {
                StartServer();
            }
            else
            {
                Status = Resources.DatatransferServerNotStarted;
            }
        }


        public List<DiagnosticsData> GetAllClients()
        {
            return TransferImpl.ClientManager.GetAllClients();
        }
    }
}
