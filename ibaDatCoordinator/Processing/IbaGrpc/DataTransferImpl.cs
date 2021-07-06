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
        private readonly ClientManager _clientManager;
        private readonly ConfigurationValidator _configurationValidator;

        public DataTransferImpl(ClientManager clientManager)
        {
            _clientManager = clientManager;
            _configurationValidator = new ConfigurationValidator();
        }

        public DataTransferData Data { get; set; }

        public override async Task<ConnectionResponse> Connect(ConnectionRequest request, ServerCallContext context)
        {
            var configuration = request.Configurataion;

            var connectionResponse = await _configurationValidator.CheckConfigurationAsync(configuration);

            if (request.ClientId == string.Empty)
            {
                var newGuid = Guid.NewGuid();
                connectionResponse.ClientId = newGuid.ToString();
                _clientManager.RegisterClient(newGuid, request.Configurataion);
            }
            else
            {
                _clientManager.UpdateClient(request.ClientId, request.Configurataion);
                connectionResponse.ClientId = request.ClientId;
            }

            return connectionResponse;
        }

        public override async Task<TransferResponse> TransferFile(IAsyncStreamReader<TransferRequest> requestStream, ServerCallContext context)
        {
            var clientId = Guid.Parse(context.RequestHeaders.GetValue("clientid"));

            var data = new List<byte>();

            while (await requestStream.MoveNext())
            {
                data.AddRange(requestStream.Current.Chunk);
            }

            var dir = Directory.CreateDirectory(Path.Combine(Data.RootPath, _clientManager.GetClientInfo(clientId).Path.Trim('/')));
            var file = Path.GetFileName(_clientManager.GetClientInfo(clientId).FileName);

            File.WriteAllBytes(Path.Combine(dir.FullName, file), data.ToArray());

            _clientManager.UpdateDiagnosticsInfo(clientId);

            return new TransferResponse()
            {
                Status = "OK"
            };
        }
    }
}
