using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iba.CertificateStore;

namespace iba.Data
{
    [Serializable]
    public class DataTransferData : ICloneable, ICertifiable
    {
        public bool IsServerEnabled { get; set; }
        public int Port { get; set; }
        public string RootPath { get; set; }
        public string ServerCertificateThumbprint { get; set; }

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
                ServerCertificateThumbprint = this.ServerCertificateThumbprint
            };
            return data;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is DataTransferData temp)) { return false; }

            return temp.IsServerEnabled == IsServerEnabled
                   && temp.Port == Port
                   && temp.RootPath == RootPath
                   && temp.ServerCertificateThumbprint == ServerCertificateThumbprint;
        }
        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = IsServerEnabled.GetHashCode();
                hashCode = (hashCode * 397) ^ Port;
                hashCode = (hashCode * 397) ^ (RootPath != null ? RootPath.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ServerCertificateThumbprint != null ? ServerCertificateThumbprint.GetHashCode() : 0);
                return hashCode;
            }
        }


        public static string certificateUserName = "Data Transfer Server";
        public IEnumerable<ICertifiable> GetCertifiableChildItems()
        {
            yield break;
        }

        public IEnumerable<ICertificateInfo> GetCertificateInfo()
        {
            if (ServerCertificateThumbprint != null && ServerCertificateThumbprint != "")
                yield return new CertificateInfoForwarder(
                    () => ServerCertificateThumbprint,
                    value => ServerCertificateThumbprint = value,
                    CertificateRequirement.Valid | CertificateRequirement.Trusted | CertificateRequirement.PrivateKey,
                    certificateUserName);
        }
    }
}
