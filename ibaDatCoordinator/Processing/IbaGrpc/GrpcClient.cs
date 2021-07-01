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

        public async Task<TransferResponse> TransferFileAsync(string file)
        {
            Metadata metadata = new Metadata();
            metadata.Add("clientname", m_data.Hostname);
            metadata.Add("clientversion", m_data.Version);
            metadata.Add("filename", file);
            metadata.Add("path", m_data.RemotePath);
            metadata.Add("apikey", string.Empty);

            CallOptions options = new CallOptions(metadata);
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
        public void TestConnection()
        {
            //ToDo
        }
    }
}
