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

        public override bool Equals(object obj)
        {
            if (!(obj is DataTransferServerInfo temp)) { return false; }

            return temp.IsServerEnabled == IsServerEnabled
                   && temp.Port == Port
                   && temp.RunsWithService == RunsWithService;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsServerEnabled.GetHashCode();
                hashCode = (hashCode * 397) ^ Port;
                hashCode = (hashCode * 397) ^ (int) RunsWithService;
                return hashCode;
            }
        }

        protected bool Equals(DataTransferServerInfo other)
        {
            return IsServerEnabled == other.IsServerEnabled && Port == other.Port && RunsWithService == other.RunsWithService;
        }
    }
}
