using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Processing.IbaGrpc
{
    [Serializable]
    class DataTransferServerInfo
    {
        public bool IsServerEnabled { get; set; }
        public int Port { get; set; }
        public Program.ServiceEnum RunsWithService { get; set; }
    }
}
