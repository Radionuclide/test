using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;

namespace iba.Data
{
    /// <summary> OPC UA configuration data (Endpoints, Security, etc) </summary>
    [Serializable]
    public class OpcUaData : ICloneable
    {
        public OpcUaData()
        {
        }

        public bool Enabled { get; set; }


        #region Connection settings

        [Serializable]
        public struct OpcUaEndPoint
        {
            public OpcUaEndPoint(int port)
            {
                Hostname = IPAddress.None.ToString();
                Port = port;
            }
            public OpcUaEndPoint(string hostname, int port) : this(port)
            {
                Hostname = hostname;
            }
            public OpcUaEndPoint(IPAddress address, int port) : this(address.ToString(), port)
            {
            }

            public int Port;

            /// <summary> If <see cref="Hostname"/> is not null or whitespace, then <see cref="Address"/> is ignored. </summary>
            public string Hostname;

            public string Uri => GetUriStringForEndpoint(this);
            public static string GetUriStringForEndpoint(OpcUaEndPoint ep) => GetUriStringForEndpoint(ep.Hostname, ep.Port);
            public static string GetUriStringForEndpoint(string addressOrHostName, int port) => $@"opc.tcp://{addressOrHostName}:{port}";
            public override string ToString()
            {
                return Uri;
            }
        }

        public enum OpcUaSecurityLevel
        {
            Unknown = -1,
            Sign = 0,
            SignEncrypt,
            SignSignEncrypt,
        };

        public bool IsAnonymousUserAllowed { get; set; }
        public bool IsNamedUserAllowed { get; set; }
        public bool IsCertifiedUserAllowed { get; set; }

        public string UserName { get; set; } = "Anonymous";

        public string Password { get; set; } = "";

        public bool HasSecurityNone { get; set; }
        public bool HasSecurityBasic128 { get; set; }
        public bool HasSecurityBasic256 { get; set; }

        public OpcUaSecurityLevel SecurityBasic128Level { get; set; }
        public OpcUaSecurityLevel SecurityBasic256Level { get; set; }

        public static int DefaultPort { get; } = 48080;
        public static string DefaultHostname = "localhost";
        public static OpcUaEndPoint DefaultEndPoint => new OpcUaEndPoint(DefaultHostname, DefaultPort);

        public List<OpcUaEndPoint> Endpoints = new List<OpcUaEndPoint>();


        public class CertificateTag
        {
            public X509Certificate2 Certificate;
            public string Name;
            public bool IsTrusted;
            public bool HasPrivateKey;
            public bool IsUsedForServer;
            public bool IsUsedForAuthentication;

            public string Issuer;
            public DateTime ExpirationDate;

            /// <summary> Is used for tooltip </summary>
            public string GetPropertyString()
            {
                string result = $@"{(IsTrusted ? "Trusted" : "Rejected")}; ";

                if (HasPrivateKey) result += "Private key; ";
                if (IsUsedForServer) result += "OPC UA Server certificate; ";
                if (IsUsedForAuthentication) result += "Authentication; ";
                result = result.TrimEnd(';', ' ');
                return result;
            }
        }

        public List<CertificateTag> Certificates = new List<CertificateTag>(); 
        public int CertificateChangedMask; // todo. kls. 

        #endregion

        /// <summary> Creates a deep copy </summary>
        public object Clone()
        {
            OpcUaData newObj = (OpcUaData)MemberwiseClone(); 
            newObj.Endpoints = new List<OpcUaEndPoint>(Endpoints);
            return newObj;
        }

        public static OpcUaData DefaultData
        {
            get
            {
                if (!((new OpcUaData()).Clone() is OpcUaData data))
                    return null;
                data.Endpoints = new List<OpcUaEndPoint> { DefaultEndPoint };
                return data;
            }
        }

        public override bool Equals(object obj)
        {
            var other = obj as OpcUaData;
            if (other == null)
            {
                return false;
            }

            return
                this.Enabled == other.Enabled &&
                this.UserName == other.UserName &&
                this.Password == other.Password &&
                this.HasSecurityNone == other.HasSecurityNone &&
                this.HasSecurityBasic128 == other.HasSecurityBasic128 &&
                this.HasSecurityBasic256 == other.HasSecurityBasic256 &&
                this.IsAnonymousUserAllowed == other.IsAnonymousUserAllowed &&
                this.IsNamedUserAllowed == other.IsNamedUserAllowed &&
                this.IsCertifiedUserAllowed == other.IsCertifiedUserAllowed &&
                this.SecurityBasic128Level == other.SecurityBasic128Level &&
                this.SecurityBasic256Level == other.SecurityBasic256Level &&
                this.CertificateChangedMask == other.CertificateChangedMask && // todo. kls. compare certificate list?
                this.Endpoints.SequenceEqual(other.Endpoints); 
        }

        public override int GetHashCode()
        {
            // we do not need a special hash here
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        public override string ToString()
        {
            var epString = Endpoints?.Count == 1 ? Endpoints[0].Uri : $"{Endpoints?.Count}";

            return $"{Enabled}, EP:[{epString}]";
        }
    }
}
