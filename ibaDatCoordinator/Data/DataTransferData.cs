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
        public bool IsServerEnabled { get; set; }
        public int Port { get; set; }
        public string RootPath { get; set; }
        public string CertificatePath { get; set; }

        public DataTransferData()
        {
            IsServerEnabled = false;
        }
        public object Clone()
        {
            var data = new DataTransferData
            {
                IsServerEnabled = this.IsServerEnabled,
                Port =  this.Port,
                RootPath = this.RootPath,
                CertificatePath = this.CertificatePath
            };
            return data;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DataTransferData temp)) { return false; }

            return temp.IsServerEnabled == IsServerEnabled
                   && temp.Port == Port
                   && temp.RootPath == RootPath
                   && temp.CertificatePath == CertificatePath;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsServerEnabled.GetHashCode();
                hashCode = (hashCode * 397) ^ Port;
                hashCode = (hashCode * 397) ^ (RootPath != null ? RootPath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (CertificatePath != null ? CertificatePath.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}
