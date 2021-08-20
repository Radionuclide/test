using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Skins;
using Grpc.Core;
using iba.Controls;
using iba.Data;
using Messages.V1;
using Google.Protobuf.WellKnownTypes;
using iba.Logging;
using Empty = Messages.V1.Empty;
using Status = Messages.V1.Status;

namespace iba.Processing.IbaGrpc
{
    class DataTransferImpl : DataTransfer.DataTransferBase
    {
        public readonly ClientManager _clientManager;
        private readonly ConfigurationValidator _configurationValidator;
        private readonly DirectoryManager _directoryManger;
        
        public ClientManager ClientManager => _clientManager;

        public DataTransferImpl()
        {
            _clientManager = new ClientManager();
            _directoryManger = new DirectoryManager(ClientManager);
            _configurationValidator = new ConfigurationValidator(ClientManager);
        }

        public override async Task<ConnectionResponse> Connect(ConnectionRequest request, ServerCallContext context)
        {
            ConnectionResponse connectionResponse;

            try
            {
                var configuration = request.Configurataion;

                bool.TryParse(context.RequestHeaders.GetValue("istestconnection"), out var testConnection);

                connectionResponse = await _configurationValidator.CheckConfigurationAsync(configuration);

                if (connectionResponse.Status == Status.Ok && !testConnection)
                {
                    ClientManager.AddOrUpdate(configuration);
                }

                connectionResponse.ConfigurationId = configuration.ConfigurationId;

            }
            catch (Exception ex)
            {
                LogData.Data.Log(Level.Exception, $"{ex.Message} \n {ex.StackTrace}");
                return new ConnectionResponse
                {
                    Status = Status.Error,
                    Message = ex.Message,
                    ConfigurationId = request.Configurataion.ConfigurationId,
                    RequestDate = Timestamp.FromDateTime(DateTime.UtcNow)
                };
            }

            return connectionResponse;
        }

        public override async Task<TransferResponse> TransferFile(IAsyncStreamReader<TransferRequest> requestStream, ServerCallContext context)
        {
            try
            {
                var configurationId = Guid.Parse(context.RequestHeaders.GetValue("configurationid"));
                var cancellationToken = context.CancellationToken;

                var succeeded = await _directoryManger.WriteFileAsync(requestStream, configurationId, cancellationToken);

                if (succeeded)
                {
                    ClientManager.UpdateDiagnosticsInfo(configurationId);
                }
            }
            catch (Exception ex)
            {
                LogData.Data.Log(Level.Exception, $"{ex.Message} \n {ex.StackTrace}");
                return new TransferResponse()
                {
                    Status = Status.Error,
                    Message = ex.Message
                };
            }

            return new TransferResponse()
            {
                Status = Status.Ok
            };
        }

        public override Task<Empty> TestConnect(Empty request, ServerCallContext context)
        {
            return Task.FromResult(new Empty());
        }
    }
}
