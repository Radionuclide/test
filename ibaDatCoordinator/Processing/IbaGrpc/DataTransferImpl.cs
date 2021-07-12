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
            _configurationValidator = new ConfigurationValidator();
        }

        public override async Task<ConnectionResponse> Connect(ConnectionRequest request, ServerCallContext context)
        {
            var configuration = request.Configurataion;

            var connectionResponse = await _configurationValidator.CheckConfigurationAsync(configuration);

            if (ClientManager.ClientList.Any( x => x.Key.ToString() == request.ClientId))
            {
                ClientManager.UpdateClient(request.ClientId, request.Configurataion);
            }
            else
            {
                var clientId = Guid.Parse(request.ClientId);
                ClientManager.RegisterClient(clientId, request.Configurataion);
            }

            connectionResponse.ClientId = request.ClientId;

            return connectionResponse;
        }

        public override async Task<TransferResponse> TransferFile(IAsyncStreamReader<TransferRequest> requestStream, ServerCallContext context)
        {
            var clientId = Guid.Parse(context.RequestHeaders.GetValue("clientid"));
            
            await _directoryManger.WriteFileAsync(requestStream, clientId);

            ClientManager.UpdateDiagnosticsInfo(clientId);

            return new TransferResponse()
            {
                Status = "OK"
            };
        }
    }
}
