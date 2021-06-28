using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iba.Processing.IbaGrpc;

namespace iba.Processing
{
    class DataTransferWorker
    {
        public readonly GrpcServer Server = new GrpcServer();

        public void StartServer()
        {
            Server.Start();
        }

        public void StopServer()
        {
            Server.Stop();
        }
    }
}
