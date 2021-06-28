using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Messages.V1;
using TransferRequest = Messages.V1.TransferRequest;

namespace iba.Processing.IbaGrpc
{
    class DataTransferImpl : DataTransfer.DataTransferBase
    {
        readonly string rootPath = Path.Combine(Directory.GetCurrentDirectory(), "rootPath");
        string targetPath = string.Empty;
        string fileName = string.Empty;


        public override async Task<TransferResponse> TransferFile(IAsyncStreamReader<TransferRequest> requestStream, ServerCallContext context)
        {
            var host = context.RequestHeaders.Get("host");
            var data = new List<byte>();
            string file = null;
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

            return new TransferResponse()
            {
                Status = "OK",
                Progress = 1
            };
        }
    }
}
