using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;
using iba.Utility;
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
        public class OpcUaEndPoint
        {
            public OpcUaEndPoint(int port)
            {
                Hostname = IPAddress.None.ToString();
                Port = port;
            }
            public OpcUaEndPoint() : this(0)
            {
            }
            public OpcUaEndPoint(string hostname, int port) : this(port)
            {
                Hostname = hostname;
            }
            public OpcUaEndPoint(OpcUaEndPoint ep) : this(ep.Hostname, ep.Port)
            {
            }

            public int Port { get; set; }

            public string Hostname { get; set; }

            public static OpcUaEndPoint ParseUri(string strUri)
            {
                return System.Uri.TryCreate(strUri, UriKind.Absolute, out Uri uri)
                    ? new OpcUaEndPoint(uri.Host, uri.Port)
                    : DefaultEndPoint;
            }

            public string Uri => GetUriStringForEndpoint(this);
            
            public static string GetUriStringForEndpoint(OpcUaEndPoint ep) => GetUriStringForEndpoint(ep.Hostname, ep.Port);
            public static string GetUriStringForEndpoint(string addressOrHostName, int port) => $@"opc.tcp://{addressOrHostName}:{port}";

            public override bool Equals(object obj)
            {
                if (!(obj is OpcUaEndPoint other))
                {
                    return false;
                }
                return Port == other.Port && string.Equals(Hostname, other.Hostname);
            }
            
            [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
            public override int GetHashCode()
            {
                unchecked
                {
                    return (Port * 397) ^ (Hostname != null ? Hostname.GetHashCode() : 0);
                }
            }

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
            None = 0,
            Basic128Rsa15,
            Basic256,
            Basic256Sha256,
        };

        public bool IsAnonymousUserAllowed { get; set; } = false;
        public bool IsNamedUserAllowed { get; set; } = true; // only this is enabled by default
        public bool IsCertifiedUserAllowed { get; set; } = false;

        public string UserName { get; set; } = "User1";

        [XmlIgnore]
        public string Password { get; set; } = "User1";

        //In old configurations the password was present in plain text
        [XmlElement("Password")]
        public string OldPasswordPlainText
        {
            get => null; //return null so that this won't be written in the configuration anymore
            set
            {
                //If password hasn't been replaced then use it from the old config
                if(Password == "User1")
                    Password = value;
            }
        }

        //Encrypted password used when saving password in configuration file.
        public string PasswordCrypted
        {
            get => Crypt.Encrypt(Password);
            set => Password = Crypt.Decrypt(value);
        }

        public bool HasSecurityNone { get; set; } = false; 
        public bool HasSecurityBasic128 { get; set; } = false;
        public bool HasSecurityBasic256 { get; set; } = false; 
        public bool HasSecurityBasic256Sha256 { get; set; } = true; // only this is enabled by default

        public OpcUaSecurityMode SecurityBasic128Mode { get; set; } = OpcUaSecurityMode.SignEncrypt;
        public OpcUaSecurityMode SecurityBasic256Mode { get; set; } = OpcUaSecurityMode.SignEncrypt;
        public OpcUaSecurityMode SecurityBasic256Sha256Mode { get; set; } = OpcUaSecurityMode.SignEncrypt; // only this is enabled by default

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
                SecurityPolicyUri = SecurityPolicies.None,
            };

            switch (alg)
            {
                case OpcUaSecurityAlgorithm.None:
                    policy.SecurityPolicyUri = SecurityPolicies.None;
                    // for none there's nothing more to configure
                    return policy;

                case OpcUaSecurityAlgorithm.Basic128Rsa15:
                    policy.SecurityPolicyUri = SecurityPolicies.Basic128Rsa15;
                    break;

                case OpcUaSecurityAlgorithm.Basic256:
                    policy.SecurityPolicyUri = SecurityPolicies.Basic256;
                    break;

                case OpcUaSecurityAlgorithm.Basic256Sha256:
                    policy.SecurityPolicyUri = SecurityPolicies.Basic256Sha256;
                    break;

                default:
                    throw new ArgumentOutOfRangeException(nameof(alg), alg, null);
            }

            switch (mode)
            {
                case OpcUaSecurityMode.Sign:
                    policy.SecurityMode = MessageSecurityMode.Sign;
                    return policy;

                case OpcUaSecurityMode.SignEncrypt:
                    policy.SecurityMode = MessageSecurityMode.SignAndEncrypt;
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

        public static int DefaultPort { get; } = 48081;
        public static string DefaultHostname => Dns.GetHostName();
        public static OpcUaEndPoint DefaultEndPoint => new OpcUaEndPoint(DefaultHostname, DefaultPort);

        public List<OpcUaEndPoint> Endpoints = new List<OpcUaEndPoint>();

        [Serializable]
        public class NetworkConfiguration
        {
            /// <summary> Fills <see cref="Hostname"/> and <see cref="Adapters"/> fields with
            /// information of current machine </summary>
            public void Initialize()
            {
                Hostname = IPGlobalProperties.GetIPGlobalProperties().HostName;

                // add existing Network Interfaces
                NetworkInterface[] allNis = NetworkInterface.GetAllNetworkInterfaces();

                Adapters = new List<NetworkAdapter>();

                foreach (NetworkInterface ni in allNis)
                {
                    if (ni.NetworkInterfaceType == NetworkInterfaceType.Loopback ||
                        ni.NetworkInterfaceType == NetworkInterfaceType.Tunnel)
                        continue;

                    var adapter = new NetworkAdapter { Name = ni.Name };
                    foreach (var address in GetV4IpAddressesOfNetworkInterface(ni))
                    {
                        adapter.Addresses.Add(address.ToString());
                    }

                    // don't add adapters without addresses
                    if (adapter.Addresses.Count > 0)
                        Adapters.Add(adapter);
                }
            }

            public string Hostname;

            public List<NetworkAdapter> Adapters = new List<NetworkAdapter>();

            /// <summary> The filed is present in this class because
            /// trace file path should be calculated on the server side,
            /// because special folders as %CommonApplicationData%
            /// can be resolved differently on different machines </summary>
            public string UaTraceFilePath;

            public static List<IPAddress> GetV4IpAddressesOfNetworkInterface(NetworkInterface ni)
            {
                var list = new List<IPAddress>();
                if (ni == null)
                    return list;

                foreach (UnicastIPAddressInformation ipInfo in ni.GetIPProperties().UnicastAddresses)
                {
                    if (ipInfo.Address.AddressFamily == AddressFamily.InterNetwork)
                        list.Add(ipInfo.Address);
                }
                return list;
            }

            [Serializable]
            public class NetworkAdapter
            {
                public List<string> Addresses = new List<string>();
                public string Name { get; set; }
            }
        }


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

                    /* these items are not needed to be compared
                       because thumbprint includes them indirectly:
                    // this.Name == other.Name && 
                    // this.Issuer == other.Issuer && // not needed; thumbprint cmp is sufficient
                    // this.ExpirationDate == other.ExpirationDate && // not needed; thumbprint cmp is sufficient
                    */

                    /* our app-specific flags (that not contained to Thumbprint) should also be compared: */
                    this.HasPrivateKey == other.HasPrivateKey &&
                    this.IsTrusted == other.IsTrusted && 
                    this.IsUsedForServer == other.IsUsedForServer &&
                    this.IsUsedForAuthentication == other.IsUsedForAuthentication;
                // ReSharper restore ArrangeThisQualifier
            }

            public override int GetHashCode() => Thumbprint?.GetHashCode() ?? 0;
        }

        public List<CertificateTag> Certificates = new List<CertificateTag>();

        /// <summary> (copied from ibaPda project) </summary>
        [Serializable]
        public class CGenerateCertificateArgs
        {
            public CGenerateCertificateArgs()
            {
                ApplicationName = "";
                ApplicationUri = "";
                Lifetime = 120;
                UseSha256 = true;
                KeySize = 2048;
            }
            public CGenerateCertificateArgs(string appName, string appUri, int lifeTimeMonths, bool bUseSha256, int keySize)
            {
                ApplicationName = appName;
                ApplicationUri = appUri;
                Lifetime = lifeTimeMonths;
                UseSha256 = bUseSha256;
                KeySize = keySize;
            }

            public string ApplicationName;
            public string ApplicationUri;
            public int Lifetime;
            public bool UseSha256;
            public int KeySize; // in bytes
        }

        /// <summary>
        /// Indicates how many changes were made in certificate configuration.
        /// This is used to tell whether we should Restart server or not.
        /// (Certificates are not fully stored in OpcUaData (only some attributes are stored);
        /// therefore they need a special handling).
        /// </summary>
        public int CertificateChangesCounter;

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
            newObj.Endpoints = new List<OpcUaEndPoint>();
            foreach (var ep in Endpoints)
            {
                newObj.Endpoints.Add(new OpcUaEndPoint(ep));
            }
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
            if (!(obj is OpcUaData other))
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
                this.HasSecurityBasic256Sha256 == other.HasSecurityBasic256Sha256 &&
                this.IsAnonymousUserAllowed == other.IsAnonymousUserAllowed &&
                this.IsNamedUserAllowed == other.IsNamedUserAllowed &&
                this.IsCertifiedUserAllowed == other.IsCertifiedUserAllowed &&
                this.SecurityBasic128Mode == other.SecurityBasic128Mode &&
                this.SecurityBasic256Mode == other.SecurityBasic256Mode &&
                this.SecurityBasic256Sha256Mode == other.SecurityBasic256Sha256Mode &&
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
            string epString = Endpoints?.Count == 1 ? Endpoints[0].Uri : $"{Endpoints?.Count}";

            return $"{Enabled}, EP:[{epString}]";
        }
    }
}
