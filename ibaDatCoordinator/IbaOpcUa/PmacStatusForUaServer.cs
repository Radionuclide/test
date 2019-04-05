using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ibaOpcServer.IbaOpcUa
{
    public class PmacStatusForUaServer
    {
        public bool IsOnline;
        public bool IsRunning;
        public int MaxDongleItems;
        public string ProjectName = "";
        public int AvailableVariablesCount;
    }
}
