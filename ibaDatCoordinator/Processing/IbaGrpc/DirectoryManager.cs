using System;
using System.Collections.Generic;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using iba.Data;
using Messages.V1;

namespace iba.Processing.IbaGrpc
{
    class DirectoryManager
    {
        private readonly ClientManager _clientManager;
        public DirectoryManager(ClientManager clientManager)
        {
            _clientManager = clientManager;
        }

        public string GetRootPath()
        {
            return TaskManager.Manager.DataTransferData.RootPath;
        }

        public string GetFilePath(Guid clientId)
        {
            var conf = _clientManager.GetClientInfo(clientId);
            
            var _rootPath = GetRootPath();

            var dir = Directory.CreateDirectory(Path.Combine(_rootPath, conf.Path.Trim('/'))).FullName;
            var file = Path.GetFileName(_clientManager.GetClientInfo(clientId).FileName);
            
            return Path.Combine(dir, file);
        }

        public async Task WriteFileAsync(IAsyncStreamReader<TransferRequest> requestStream, Guid clientId)
        {
            var path = GetFilePath(clientId);
            
            const int bufferSize = 64 * 1024;

            int milliseconds = CalculateDelayTime(bufferSize, clientId);

            using (var sw = new FileStream(path, FileMode.OpenOrCreate))
            {

                while (await requestStream.MoveNext())
                {
                    var byteArray = requestStream.Current.Chunk.ToByteArray();
                    await sw.WriteAsync(byteArray, 0, byteArray.Length);
                    DelayWritingChunk(milliseconds);
                }
            }
        }

        private int CalculateDelayTime(int bufferSize, Guid clientId)
        {
            var conf = _clientManager.GetClientInfo(clientId);
            if (conf.Maxbandwidth == 0)
            {
                return 0;
            }

            const int milliseconds = 1000;
            var kiloByte = (bufferSize / 1024);

            if (conf.Maxbandwidth > kiloByte)
            {
                return  (milliseconds / (conf.Maxbandwidth / kiloByte));
            }

            return 0;
        }

        private void DelayWritingChunk(int milliseconds)
        {
             Thread.Sleep(milliseconds);
        }
    }
}
