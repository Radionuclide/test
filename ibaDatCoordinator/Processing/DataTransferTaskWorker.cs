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
        private DataTransferTaskData m_data;

        public DataTransferTaskWorker(DataTransferTaskData task)
        {
            m_data = task;
            client = new GrpcClient(m_data);
        }

        public async Task TransferFile(string file, TaskData task)
        {
            await client.TransferFileAsync(file, task);
        }
    }
}
