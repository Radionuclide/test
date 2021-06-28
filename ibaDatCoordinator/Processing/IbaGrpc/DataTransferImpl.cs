using System;
using System.Collections.Generic;
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

        public override async Task<ConfigurationResponse> SendConfiguration(ConfigurationRequest request, ServerCallContext context)
        {
            targetPath = Path.Combine(rootPath, request.Path);
            fileName = request.FileName;

            if (Directory.Exists(targetPath))
            {
                return await Task.Run(() => new ConfigurationResponse()
                    { FileName = request.FileName, Path = request.Path, Status = "Error: Directory already exists" });
            }


            return await Task.FromResult(new ConfigurationResponse() { FileName = request.FileName, Path = request.Path, Status = "OK" });
        }

        public override async Task<TransferResponse> UploadFile(IAsyncStreamReader<TransferRequest> requestStream, ServerCallContext context)
        {
            var data = new List<byte>();
            while (await requestStream.MoveNext())
            {
                Console.WriteLine("Received " +
                                  requestStream.Current.Chunk.Length + " bytes");
                data.AddRange(requestStream.Current.Chunk);
            }
            Console.WriteLine("Received file with " + data.Count + " bytes");

            Directory.CreateDirectory(Path.Combine(targetPath));

            File.WriteAllBytes(Path.Combine(targetPath, fileName), data.ToArray());

            return new TransferResponse()
            {
                Status = "OK"
            };
        }
    }
}
