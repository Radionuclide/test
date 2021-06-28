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
        public readonly GrpcClient client;
        private readonly string file;
        private string basePath = AppDomain.CurrentDomain.BaseDirectory;

        private DataTransferTaskData m_data;

        public DataTransferTaskWorker(string fileToCopy, DataTransferTaskData task)
        {
            file = fileToCopy;
            m_data = task;
            client = new GrpcClient(m_data.Server, m_data.Port);
        }

        public async Task TransferFile()
        {
            await client.TransferFileAsync(file);
        }
    }
}
