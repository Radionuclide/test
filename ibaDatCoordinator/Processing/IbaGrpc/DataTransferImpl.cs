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
using TransferRequest = Messages.V1.TransferRequest;

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

            var result = await _configurationValidator.CheckConfigurationAsync(configuration);
            return result;
        }

        public override async Task<TransferResponse> TransferFile(IAsyncStreamReader<TransferRequest> requestStream, ServerCallContext context)
        {
            DiagnosticsData diagnosticsData = new DiagnosticsData
            {
                ClientName = context.RequestHeaders.Get("clientname").Value,
                ClientVersion = context.RequestHeaders.Get("clientversion").Value,
                Path = context.RequestHeaders.Get("path").Value,
                ApiKey = context.RequestHeaders.Get("apikey").Value,
                TransferredFiles = 0,
                Filename = context.RequestHeaders.Get("filename").Value
            };


            _clientManager.RegisterClient(diagnosticsData);

            var data = new List<byte>();

            while (await requestStream.MoveNext())
            {
                Debug.WriteLine("Received " +
                                  requestStream.Current.Chunk.Length + " bytes");
                data.AddRange(requestStream.Current.Chunk);
            }

            var dir = Directory.CreateDirectory(Path.Combine(_data.RootPath, diagnosticsData.Path.Trim('/')));
            var file = Path.GetFileName(diagnosticsData.Filename);


            File.WriteAllBytes(Path.Combine(dir.FullName, file), data.ToArray());

            _clientManager.UpdateDiagnosticsInfo(diagnosticsData);

            return new TransferResponse()
            {
                Status = "OK"
            };
        }
    }
}
