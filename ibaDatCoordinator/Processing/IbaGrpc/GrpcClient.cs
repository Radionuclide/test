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

        public async Task<TransferResponse> TransferFileAsync(string file)
        {
            var connectionRequest = CreateConnectionRequest(file);
            var connectionResponse = await ConnectAsync(connectionRequest);
            
            m_data.ClientId = connectionResponse.ClientId;

            if (connectionResponse.Status != "OK")
            {
                return new TransferResponse
                {
                    Status = connectionResponse.Status
                };
            }

            var metadata = new Metadata
            {
                {"clientId", connectionResponse.ClientId},
            };

            var options = new CallOptions(metadata);

            using (AsyncClientStreamingCall<TransferRequest, TransferResponse> call = client.TransferFile(options))
            {
                IClientStreamWriter<TransferRequest> stream = call.RequestStream;

                using (FileStream fs = File.OpenRead(Path.Combine(file)))
                {
                    while (true)
                    {
                        byte[] buffer = new byte[64 * 1024];
                        int numRead = await fs.ReadAsync(buffer, 0, buffer.Length);

                        if (numRead == 0)
                        {
                            break;
                        }

                        if (numRead < buffer.Length)
                        {
                            Array.Resize(ref buffer, numRead);
                        }

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

        private ConnectionRequest CreateConnectionRequest(string file)
        {
            var fileName = Path.GetFileName(file);

            if (m_data.ClientId == null)
            {
                m_data.ClientId = string.Empty;
            }

            return new ConnectionRequest
            {
                ClientId = m_data.ClientId != string.Empty ? m_data.ClientId : string.Empty,
                Configurataion = new Configuration
                {
                    ClientName = m_data.Hostname,
                    ClientVersion = m_data.Version,
                    FileName = fileName,
                    Path = m_data.RemotePath,
                    ApiKey = string.Empty,
                }
            };
        }

        public void TestConnection()
        {
            //ToDo
        }
    }
}
