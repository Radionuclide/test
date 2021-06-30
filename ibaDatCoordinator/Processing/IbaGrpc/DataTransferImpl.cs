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
using Messages.V1;
using TransferRequest = Messages.V1.TransferRequest;

namespace iba.Processing.IbaGrpc
{
    class DataTransferImpl : DataTransfer.DataTransferBase
    {
        private readonly ClientManager _clientManager;

        public DataTransferImpl(ClientManager clientManager)
        {
            _clientManager = clientManager;
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

            string file = null;
            var data = new List<byte>();

            while (await requestStream.MoveNext())
            {
                Debug.WriteLine("Received " +
                                  requestStream.Current.Chunk.Length + " bytes");
                data.AddRange(requestStream.Current.Chunk);
                file = requestStream.Current.FileName;
            }


            Directory.CreateDirectory(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test"));

            file = file.Substring(file.LastIndexOf("\\") +1);

            File.WriteAllBytes(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test", file), data.ToArray());

            _clientManager.UpdateDiagnosticsInfo(diagnosticsData);

            return new TransferResponse()
            {
                Status = "OK",
                Progress = 1
            };
        }
    }
}
