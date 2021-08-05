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
using iba.Logging;
using Status = Messages.V1.Status;

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
            try
            {
                var connectionCall = client.ConnectAsync(request);

                var connectionResponse = await connectionCall.ResponseAsync;

                return connectionResponse;
            }
            catch (Exception ex)
            {
                LogData.Data.Log(Level.Exception,  $"{ex.Message}\n{ex.StackTrace}", ex);

                return new ConnectionResponse
                {
                    Status = Status.Error,
                    Message = "An error occurred when establishing the connection"
                };
            }
        }

        public async Task<TransferResponse> TransferFileAsync(string file, TaskData task)
        {
            var connectionRequest = CreateConnectionRequest(file, task);
            var connectionResponse = await ConnectAsync(connectionRequest);
            
            m_data.ClientId = connectionResponse.ConfigurationId;

            if (connectionResponse.Status == Status.Error)
            {
                return new TransferResponse
                {
                    Status = connectionResponse.Status,
                    Message = connectionResponse.Message
                };
            }

            var options = CreateMetadata(connectionResponse);

            using (var call = client.TransferFile(options))
            {
                var stream = call.RequestStream;

                await SendData(file, stream);

                await stream.CompleteAsync();

                var response = await call.ResponseAsync;

                return response;
            }
        }

        private static CallOptions CreateMetadata(ConnectionResponse connectionResponse)
        {
            var metadata = new Metadata
            {
                { "configurationId", connectionResponse.ConfigurationId },
            };

            var options = new CallOptions(metadata);
            return options;
        }

        private async Task SendData(string file, IClientStreamWriter<TransferRequest> stream)
        {
            using (var fs = File.OpenRead(Path.Combine(file)))
            {
                while (true)
                {
                    const int bufferSize = 64 * 1024;
                    var buffer = new byte[bufferSize];
                    var numRead = await fs.ReadAsync(buffer, 0, buffer.Length);

                    if (numRead == 0)
                    {
                        break;
                    }

                    if (numRead < buffer.Length)
                    {
                        Array.Resize(ref buffer, numRead);
                    }

                    await DelaySendingChunk(bufferSize);

                    await stream.WriteAsync(new TransferRequest()
                    {
                        Chunk = ByteString.CopyFrom(buffer)
                    });
                }
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

        public async Task<ConnectionResponse> TestConnection(DataTransferTaskData dataTransferTaskData)
        {
            var connectionRequest = CreateConnectionRequest(string.Empty, dataTransferTaskData);
            var connectionResponse = await ConnectAsync(connectionRequest);

            return connectionResponse;
        }
    }
}
