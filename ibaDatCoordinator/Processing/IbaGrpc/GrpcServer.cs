using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Messages.V1;

namespace iba.Processing.IbaGrpc
{
    class GrpcServer
    {
        private const int Port = 30051;
        private Server _server { get; set; }
        public void Start()
        {
            _server = CreateNewServer();
            _server.Start();
        }


        public void Stop()
        {
            _server?.ShutdownAsync().Wait();
        }

        private Server CreateNewServer()
        {
            return new Server
            {
                Services = { DataTransfer.BindService(new DataTransferImpl()) },
                Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) },
            };
        }
    }
}
