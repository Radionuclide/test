using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using iba.Data;
using iba.Logging;
using Messages.V1;

namespace iba.Processing.IbaGrpc
{
    class GrpcServer
    {
        private const string HOST = "localhost";
        private Server _server { get; set; }

        public int Port
        {
            get => 30051;
            set
            {
                if (value <= 0) throw new ArgumentOutOfRangeException(nameof(value));
                Port = value;
            }
        }

        public void Start()
        {
            try
            {
                _server = CreateNewServer();
                _server.Start();
                LogData.Data.Logger.Log(Level.Info, "Data Transfer Service started");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                LogData.Data.Logger.Log(Level.Exception, "Data Transfer Service could not start", e.Message);
            }
        }


        public void Stop()
        {
            try
            {
                _server?.ShutdownAsync().Wait();
                LogData.Data.Logger.Log(Level.Info, "Data Transfer Service stopped");
            }
            catch (Exception e)
            {
                LogData.Data.Logger.Log(Level.Exception, "Data Transfer Service could not stop", e.Message);
            }
        }

        private Server CreateNewServer()
        {
            return new Server
            {
                Services = { DataTransfer.BindService(new DataTransferImpl()) },
                Ports = { new ServerPort(HOST, Port, ServerCredentials.Insecure) },
            };
        }
    }
}
