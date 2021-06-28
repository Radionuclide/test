using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iba.Data;
using iba.Processing.IbaGrpc;

namespace iba.Processing
{
    internal class DataTransferWorker
    {
        private readonly GrpcServer _server = new GrpcServer();
        private DataTransferData _dataTransferData = new DataTransferData();

        public DataTransferData DataTransferData
        {
            get => _dataTransferData;
            set => _dataTransferData = value;
        }

        public void StartServer()
        {
            _server.Start();
        }

        public void StopServer()
        {
            _server.Stop();
        }

        public void Init()
        {
            if (_dataTransferData.ServerEnabled)
            {
                StartServer();
            }
        }
    }
}
