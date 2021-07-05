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
        private DataTransferData _data;

        public DataTransferImpl(ClientManager clientManager)
        {
            _clientManager = clientManager;
            _configurationValidator = new ConfigurationValidator();
        }

        public DataTransferData Data
        {
            get => _data;
            set => _data = value;
        }

        public override async Task<ConnectionResponse> Connect(ConnectionRequest request, ServerCallContext context)
        {
            var configuration = request.Configurataion;

            var connectionResponse = await _configurationValidator.CheckConfigurationAsync(configuration);

            //DiagnosticsData diagnosticsData = new DiagnosticsData
            //{
            //    ClientName = configuration.ClientName,
            //    ClientVersion = configuration.ClientVersion,
            //    Path = configuration.Path,
            //    ApiKey = configuration.ApiKey,
            //    TransferredFiles = 0,
            //    Filename = configuration.FileName
            //};

            if (request.ClientId == string.Empty)
            {
                var newGuid = Guid.NewGuid();
                connectionResponse.ClientId = newGuid.ToString();
                _clientManager.ClientList.TryAdd(newGuid, request.Configurataion);
            }
            else
            {
                _clientManager.ClientList
                    .First(x => x.Key == Guid.Parse(request.ClientId)).Value.FileName = request.Configurataion.FileName;
                connectionResponse.ClientId = request.ClientId;
            }

            //_clientManager.RegisterClient(diagnosticsData);

            return connectionResponse;
        }

        public override async Task<TransferResponse> TransferFile(IAsyncStreamReader<TransferRequest> requestStream, ServerCallContext context)
        {
            var clientId = Guid.Parse(context.RequestHeaders.GetValue("clientid"));
            
            
            var tmp = _clientManager.ClientList
                .First(x => x.Key == clientId).Value;

            var path = tmp.Path;
            var filename = tmp.FileName;

            var data = new List<byte>();

            while (await requestStream.MoveNext())
            {
                Debug.WriteLine("Received " +
                                  requestStream.Current.Chunk.Length + " bytes");
                data.AddRange(requestStream.Current.Chunk);
            }

            var dir = Directory.CreateDirectory(Path.Combine(_data.RootPath, path.Trim('/')));
            var file = Path.GetFileName(filename);


            File.WriteAllBytes(Path.Combine(dir.FullName, file), data.ToArray());

            //_clientManager.UpdateDiagnosticsInfo(clientId);

            return new TransferResponse()
            {
                Status = "OK"
            };
        }
    }
}
