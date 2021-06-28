using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iba.Data;
using iba.Processing.IbaGrpc;

namespace iba.Processing
{
    internal class DataTransferTaskWorker
    {
        public readonly GrpcClient client = new GrpcClient();
        private readonly string file;
        private string basePath = AppDomain.CurrentDomain.BaseDirectory;

        private DataTransferTaskData m_data;

        public DataTransferTaskWorker(string fileToCopy, DataTransferTaskData task)
        {
            file = fileToCopy;
            m_data = task;
        }

        public async Task TransferFile()
        {
            client.TransferFileAsync(file).Wait();
        }
    }
}
