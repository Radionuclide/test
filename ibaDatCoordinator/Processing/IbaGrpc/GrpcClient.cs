using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Protobuf;
using Grpc.Core;
using Messages.V1;

namespace iba.Processing.IbaGrpc
{
    class GrpcClient
    {
        internal static readonly Channel channel = new Channel("127.0.0.1:30051", ChannelCredentials.Insecure);
        private DataTransfer.DataTransferClient client;
        public GrpcClient()
        {
            client = new DataTransfer.DataTransferClient(channel);
        }

        public async Task<ConfigurationResponse> SendConfigurationAsync()
        {
            var response = await client.SendConfigurationAsync(new ConfigurationRequest
            {
                FileName = "test.txt", Path = "testpath"
            });

            if (response.Status == "OK")
            {
                MessageBox.Show(response.FileName);
            }

            return response;
        }
        public async Task<TransferResponse> TransferFileAsync(string file)
        {
            using (AsyncClientStreamingCall<TransferRequest, TransferResponse> call = client.TransferFile())
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

                        await stream.WriteAsync(new TransferRequest() { Chunk = ByteString.CopyFrom(buffer), FileName = file});
                    }
                }

                await stream.CompleteAsync();

                TransferResponse response = await call.ResponseAsync;

                return response;
            }
        }
        public void TestConnection()
        {
            var response = client.SendConfiguration(new ConfigurationRequest
            {
                FileName = "test.txt",
                Path = "testpath"
            });

            if (response.Status == "OK")
            {
                MessageBox.Show(response.FileName);
            }
        }
    }
}
