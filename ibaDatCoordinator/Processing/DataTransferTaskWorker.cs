using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using iba.Data;
using iba.Logging;
using iba.Processing.IbaGrpc;
using Status = Messages.V1.Status;

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

        public async Task TransferFile(string file, TaskData task, CancellationToken cancellationToken)
        {
            try
            {
                var response = await client.TransferFileAsync(file, task, cancellationToken);

                if (response.Status == Status.Error)
                {
                    await client.channel.ShutdownAsync();
                    throw new InvalidOperationException(response.Message);
                }
            }
            finally
            {
                await client.channel.ShutdownAsync();
            }
        }
    }
}
