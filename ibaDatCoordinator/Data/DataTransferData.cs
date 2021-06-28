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

        public DataTransferData()
        {
            ServerEnabled = false;
        }
        public object Clone()
        {
            var data = new DataTransferData
            {
                ServerEnabled = this.ServerEnabled,
                Port =  this.Port
            };
            return data;
        }

        public override bool Equals(object obj)
        {
            var temp = obj as DataTransferData;
            if (temp == null) { return false; }

            return temp.ServerEnabled == ServerEnabled
                   && temp.Port == Port;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (ServerEnabled.GetHashCode() * 397) ^ Port;
            }
        }
    }
}
