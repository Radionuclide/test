using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using Opc.Ua;

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

        public enum OpcUaSecurityMode
        {
            Unknown = -1,
            Sign = 0,
            SignEncrypt,
            SignPlusSignEncrypt,
        }
        public enum OpcUaSecurityAlgorithm
        {
            Unknown = -1,
            None = 0,
            Basic128Rsa15,
            Basic256,
        };

        public bool IsAnonymousUserAllowed { get; set; } = true;
        public bool IsNamedUserAllowed { get; set; } = true;
        public bool IsCertifiedUserAllowed { get; set; } = true;

        public string UserName { get; set; } = "User1";

        public string Password { get; set; } = "123";

        public bool HasSecurityNone { get; set; } = true;
        public bool HasSecurityBasic128 { get; set; } = true;
        public bool HasSecurityBasic256 { get; set; } = true;

        public OpcUaSecurityMode SecurityBasic128Mode { get; set; } = OpcUaSecurityMode.SignEncrypt;
        public OpcUaSecurityMode SecurityBasic256Mode { get; set; } = OpcUaSecurityMode.SignEncrypt;

        public static List<ServerSecurityPolicy> CreateSecurityPolicies(OpcUaSecurityAlgorithm alg, OpcUaSecurityMode mode)
        {
            if (mode == OpcUaSecurityMode.SignPlusSignEncrypt)
            {
                // create two policies
                var policy1 = CreateSecurityPolicy(alg, OpcUaSecurityMode.Sign);
                var policy2 = CreateSecurityPolicy(alg, OpcUaSecurityMode.SignEncrypt);
                return new List<ServerSecurityPolicy> { policy1, policy2 };
            }

            // otherwise just return a list with a single policy
            var policy = CreateSecurityPolicy(alg, mode);
            return new List<ServerSecurityPolicy> { policy };
        }

        private static ServerSecurityPolicy CreateSecurityPolicy(OpcUaSecurityAlgorithm alg, OpcUaSecurityMode mode)
        {
            // for SignPlusSignEncrypt please call CreateSecurityPolicy
            Debug.Assert(mode != OpcUaSecurityMode.SignPlusSignEncrypt);

            var policy = new ServerSecurityPolicy
            {
                SecurityMode = MessageSecurityMode.None,
                SecurityPolicyUri = "http://opcfoundation.org/UA/SecurityPolicy#None",
                SecurityLevel = 0
            };

            switch (alg)
            {
                case OpcUaSecurityAlgorithm.None:
                    policy.SecurityPolicyUri = "http://opcfoundation.org/UA/SecurityPolicy#None";
                    // for none there's nothing more to configure
                    return policy;

                case OpcUaSecurityAlgorithm.Basic128Rsa15:
                    policy.SecurityPolicyUri = "http://opcfoundation.org/UA/SecurityPolicy#Basic128Rsa15";
                    break;

                case OpcUaSecurityAlgorithm.Basic256:
                    policy.SecurityPolicyUri = "http://opcfoundation.org/UA/SecurityPolicy#Basic256";
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(alg), alg, null);
            }

            switch (mode)
            {
                case OpcUaSecurityMode.Sign:
                    policy.SecurityLevel = 2;
                    policy.SecurityMode = MessageSecurityMode.Sign;
                    return policy;

                case OpcUaSecurityMode.SignEncrypt:
                    policy.SecurityMode = MessageSecurityMode.SignAndEncrypt;
                    policy.SecurityLevel = 3;
                    return policy;

                case OpcUaSecurityMode.SignPlusSignEncrypt:
                    // should not happen
                    // CreateSecurityPolicy() should be called for this mode instead
                    Debug.Assert(false);
                    return null;

                default:
                    throw new ArgumentOutOfRangeException(nameof(mode), mode, null);
            }
        }

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

            public bool IsTrusted;
            public bool IsUsedForServer;
            public bool IsUsedForAuthentication;
            public bool HasPrivateKey;

            private string _thumbprint;
            public string Thumbprint
            {
                get
                {
                    if (_thumbprint == null)
                    {
                        // get it from Certificate and keep
                        _thumbprint = Certificate?.Thumbprint;
                    }
                    else // is already set
                    {
                        // check consistency
                        Debug.Assert(Certificate == null || Certificate?.Thumbprint == _thumbprint);
                    }

                    return _thumbprint;
                    
                }
                set
                {
                    _thumbprint = value;
                    // check consistency
                    Debug.Assert(Certificate == null || Certificate?.Thumbprint == _thumbprint);
                }
            }

            public string Name => Certificate == null ?
                null :
                !string.IsNullOrWhiteSpace(Certificate.FriendlyName) ?
                Certificate.FriendlyName : Certificate.Subject;

            public string Issuer => Certificate?.Issuer;

            public string ExpirationDate => Certificate?.GetExpirationDateString();
            
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

        /// <summary>
        /// Indicates how many changes were made in certificate configuration.
        /// This is used to tell whether we should Restart server or not.
        /// </summary>
        public int CertificateChangesCounter; // todo. kls. maybe use more straightforward approach?

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
            };
            return AddCertificate(certTag);
        }

        public CertificateTag AddCertificate(CertificateTag certTag)
        {
            Debug.Assert(certTag != null);
            // ensure th is not empty
            Debug.Assert(!string.IsNullOrWhiteSpace(certTag.Thumbprint));

            if (GetCertificate(certTag.Thumbprint) != null)
            {
                // cannot add a certificate, because such th already present in collection
                Debug.Assert(false);
                return null;
            }

            Certificates.Add(certTag);
            return certTag;
        }

        public void SetServerCertificateFlag(CertificateTag cert)
        {
            // server flag can only be single
            // reset other flags
            foreach (var certTag in Certificates)
            {
                certTag.IsUsedForServer = false;
            }
            cert.IsUsedForServer = true;
        }

        public void SetServerCertificateFlag(string thumbprint)
        {
            CertificateTag certTag = GetCertificate(thumbprint);
            if (certTag == null)
            {
                Debug.Assert(false); // not found
                return;
            }
            SetServerCertificateFlag(certTag);
        }

        public CertificateTag GetServerCertificate()
        {
            foreach (var certTag in Certificates)
            {
                if (certTag.IsUsedForServer && certTag.HasPrivateKey)
                    return certTag;
            }
            return null;
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
                this.SecurityBasic128Mode == other.SecurityBasic128Mode &&
                this.SecurityBasic256Mode == other.SecurityBasic256Mode &&
                this.CertificateChangesCounter == other.CertificateChangesCounter &&
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
