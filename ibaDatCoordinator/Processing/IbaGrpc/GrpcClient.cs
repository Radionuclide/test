using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Protobuf;
using Grpc.Core;
using iba.Data;
using Messages.V1;
using Google.Protobuf.WellKnownTypes;

namespace iba.Processing.IbaGrpc
{
    class GrpcClient
    {
        internal  readonly Channel channel;
        private DataTransfer.DataTransferClient client;
        private readonly DataTransferTaskData m_data;

        public GrpcClient(DataTransferTaskData data)
        {
            m_data = data;
            channel = new Channel($"{m_data.Server}:{m_data.Port}", ChannelCredentials.Insecure);
            client = new DataTransfer.DataTransferClient(channel);
        }

        public async Task<ConnectionResponse> ConnectAsync(ConnectionRequest request)
        {
            var connectionCall = client.ConnectAsync(request);
            
            var connectionResponse  = await connectionCall.ResponseAsync;

            return connectionResponse;
        }

        public async Task<TransferResponse> TransferFileAsync(string file, TaskData task)
        {
            var connectionRequest = CreateConnectionRequest(file, task);
            var connectionResponse = await ConnectAsync(connectionRequest);
            
            m_data.ClientId = connectionResponse.ConfigurationId;

            if (connectionResponse.Status != "OK")
            {
                return new TransferResponse
                {
                    Status = connectionResponse.Status
                };
            }

            var metadata = new Metadata
            {
                {"configurationId", connectionResponse.ConfigurationId},
            };

            var options = new CallOptions(metadata);

            using (AsyncClientStreamingCall<TransferRequest, TransferResponse> call = client.TransferFile(options))
            {
                IClientStreamWriter<TransferRequest> stream = call.RequestStream;

                using (FileStream fs = File.OpenRead(Path.Combine(file)))
                {
                    while (true)
                    {
                        const int bufferSize = 64 * 1024;
                        byte[] buffer = new byte[bufferSize];
                        int numRead = await fs.ReadAsync(buffer, 0, buffer.Length);

                        if (numRead == 0)
                        {
                            break;
                        }

                        if (numRead < buffer.Length)
                        {
                            Array.Resize(ref buffer, numRead);
                        }

                        await DelaySendingChunk(bufferSize);

                        await stream.WriteAsync(new TransferRequest() { 
                            Chunk = ByteString.CopyFrom(buffer)
                        });
                    }
                }

                await stream.CompleteAsync();

                TransferResponse response = await call.ResponseAsync;

                return response;
            }
        }

        private async Task DelaySendingChunk(int bufferSize)
        {
            if (m_data.MaxBandwidth == 0)
            {
                return;
            }

            const int milliseconds = 1000;

            try
            {
                var kiloByte = (bufferSize / 1024);

                if (m_data.MaxBandwidth > kiloByte)
                {
                    await Task.Delay(milliseconds / (m_data.MaxBandwidth / kiloByte));
                }
            }
            catch (Exception e)
            {
                //Todo
            }
        }

        private ConnectionRequest CreateConnectionRequest(string file, TaskData task)
        {
            var fileName = Path.GetFileName(file);

            return new ConnectionRequest
            {
                
                Configurataion = new Configuration
                {

                    ConfigurationId = task.Guid.ToString(),
                    RequestDate = Timestamp.FromDateTime(DateTime.UtcNow),
                    ClientName = m_data.Hostname,
                    TaskName = task.Name,
                    ClientVersion = m_data.Version,
                    FileName = fileName,
                    Path = m_data.RemotePath,
                    ApiKey = string.Empty,
                    Maxbandwidth =  m_data.MaxBandwidth
                }
            };
        }

        public void TestConnection()
        {
            //ToDo
        }
    }
}
