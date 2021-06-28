using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iba.Processing.IbaGrpc;

namespace iba.Processing
{
    class DataTransferTaskWorker
    {
        public readonly GrpcClient Server = new GrpcClient();

    }
}
