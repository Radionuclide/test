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
using iba.Logging;
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
            
            var extension = Path.GetExtension(path);

            path = Path.ChangeExtension(path, ".temp");

            var milliseconds = CalculateDelayTime(clientId);

            using (var fs = new FileStream(path, FileMode.OpenOrCreate))
            {

                while (await requestStream.MoveNext())
                {
                    var byteArray = requestStream.Current.Chunk.ToByteArray();
                    await fs.WriteAsync(byteArray, 0, byteArray.Length);
                    DelayWritingChunk(milliseconds);
                }
            }

            await RenameFile(path, extension);
        }

        public async Task RenameFile(string path, string extension)
        {
            var currentFile = new FileInfo(path);

            var renamedPath = Path.ChangeExtension(path, extension);

            await Task.Factory.StartNew(() =>
            {
                if (File.Exists(renamedPath))
                {
                    File.Delete(renamedPath);
                }

                currentFile.MoveTo(Path.ChangeExtension(path, extension));
            });
        }

        private int CalculateDelayTime(Guid clientId)
        {
            const int bufferSize = 64 * 1024;

            var conf = _clientManager.GetClientInfo(clientId);
            if (conf.Maxbandwidth == 0)
            {
                return 0;
            }

            const int milliseconds = 1000;
            const int kiloByte = (bufferSize / 1024);

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

        public static Task DeleteFileAsync(string file)
        {
            try
            {
                return Task.Run(() => File.Delete(file));
            }
            catch (Exception e)
            {
                LogData.Data.Log(Level.Exception, e.Message);
                throw;
            }
        }
    }
}
