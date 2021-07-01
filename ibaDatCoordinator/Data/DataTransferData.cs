using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Data
{
    [Serializable]
    public class DataTransferData : ICloneable
    {
        public bool ServerEnabled { get; set; }
        public int Port { get; set; }
        public string RootPath { get; set; }

        public DataTransferData()
        {
            ServerEnabled = false;
        }
        public object Clone()
        {
            var data = new DataTransferData
            {
                ServerEnabled = this.ServerEnabled,
                Port =  this.Port,
                RootPath = this.RootPath
            };
            return data;
        }

        public override bool Equals(object obj)
        {
            var temp = obj as DataTransferData;
            if (temp == null) { return false; }

            return temp.ServerEnabled == ServerEnabled
                   && temp.Port == Port
                   && temp.RootPath == RootPath;
        }

        protected bool Equals(DataTransferData other)
        {
            return ServerEnabled == other.ServerEnabled && Port == other.Port && RootPath == other.RootPath;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = ServerEnabled.GetHashCode();
                hashCode = (hashCode * 397) ^ Port;
                hashCode = (hashCode * 397) ^ (RootPath != null ? RootPath.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
