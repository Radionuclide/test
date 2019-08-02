using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

namespace iba.Data
{
    /// <summary> OPC UA configuration data (Endpoints, Security, etc) </summary>
    [Serializable]
    public class OpcUaData : ICloneable
    {
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

        #region Certificates
        
        [Serializable]
        public class CertificateTag
        {
            [XmlIgnore]
            public X509Certificate2 Certificate;

            public string Thumbprint;
            public string Name;
            public string Issuer;
            public string ExpirationDate;

            public bool IsTrusted;
            public bool HasPrivateKey;
            public bool IsUsedForServer;
            public bool IsUsedForAuthentication;

            
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

            public void FillTextFieldsFromCertificate()
            {
                Thumbprint = "";
                Name = "";
                Issuer = "";
                ExpirationDate = "";
                HasPrivateKey = false;

                if (Certificate == null)
                    return;

                Thumbprint = Certificate.Thumbprint;
                Name = !string.IsNullOrWhiteSpace(Certificate.FriendlyName) ?
                    Certificate.FriendlyName : Certificate.Subject;
                Issuer = Certificate.Issuer;
                ExpirationDate = Certificate.GetExpirationDateString();
                HasPrivateKey = Certificate.HasPrivateKey;
            }

            public override bool Equals(object obj)
            {
                var other = obj as CertificateTag;
                if (other == null)
                {
                    return false;
                }

                // ReSharper disable ArrangeThisQualifier
                return
                    this.Thumbprint == other.Thumbprint &&
                    this.Name == other.Name &&
                    this.Issuer == other.Issuer &&
                    this.ExpirationDate == other.ExpirationDate &&
                    this.IsTrusted == other.IsTrusted &&
                    this.HasPrivateKey == other.HasPrivateKey &&
                    this.IsUsedForServer == other.IsUsedForServer &&
                    this.IsUsedForAuthentication == other.IsUsedForAuthentication;
                // ReSharper restore ArrangeThisQualifier
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    // ReSharper disable NonReadonlyMemberInGetHashCode
                    var hashCode = (Thumbprint != null ? Thumbprint.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (Issuer != null ? Issuer.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (ExpirationDate != null ? ExpirationDate.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ IsTrusted.GetHashCode();
                    hashCode = (hashCode * 397) ^ HasPrivateKey.GetHashCode();
                    hashCode = (hashCode * 397) ^ IsUsedForServer.GetHashCode();
                    hashCode = (hashCode * 397) ^ IsUsedForAuthentication.GetHashCode();
                    return hashCode;
                    // ReSharper restore NonReadonlyMemberInGetHashCode
                }
            }
        }

        public List<CertificateTag> Certificates = new List<CertificateTag>();

        public CertificateTag GetCertificate(string thumbprint)
        {
            if (string.IsNullOrWhiteSpace(thumbprint))
                return null;

            foreach (var certTag in Certificates)
            {
                if (certTag.Thumbprint == thumbprint)
                    return certTag;
            }
            return null;
        }

        public CertificateTag AddCertificate(X509Certificate2 cert)
        {
            var certTag = new CertificateTag
            {
                Certificate = cert,
                Thumbprint = cert.Thumbprint
            };
            return AddCertificate(certTag);
        }
        public CertificateTag AddCertificate(CertificateTag certTag)
        {
            Debug.Assert(certTag != null);
            // ensure sn consistency
            Debug.Assert(certTag.Certificate == null || certTag.Certificate.Thumbprint == certTag.Thumbprint);
            // ensure sn is not empty
            Debug.Assert(!string.IsNullOrWhiteSpace(certTag.Thumbprint));

            if (GetCertificate(certTag.Thumbprint) != null)
            {
                // cannot add a certificate, because such sn already present in collection
                Debug.Assert(false);
                return null;
            }

            if (certTag.Certificate != null)
                certTag.FillTextFieldsFromCertificate();

            Certificates.Add(certTag);
            return certTag;
        }

        public void SetServerFlag(CertificateTag cert)
        {
            // server flag con only be single
            // reset other flags
            foreach (var certTag in Certificates)
            {
                certTag.IsUsedForServer = false;
            }
            cert.IsUsedForServer = true;
        }

        public void SetServerFlag(string thumbprint)
        {
            CertificateTag certTag = GetCertificate(thumbprint);
            if (certTag == null)
            {
                Debug.Assert(false); // not found
                return;
            }
            SetServerFlag(certTag);
        }

        #endregion
        
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

            // ReSharper disable ArrangeThisQualifier
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
                this.Endpoints.SequenceEqual(other.Endpoints) &&
                this.Certificates.SequenceEqual(other.Certificates);
            // ReSharper restore ArrangeThisQualifier
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
