using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace iba.Data
{
    /// <summary> OPC UA configuration data (Endpoints, Security, etc) </summary>
    [Serializable]
    public class OpcUaData : ICloneable
    {
        public OpcUaData()
        {
            ResetToDefaults();
        }

        public bool Enabled { get; set; }
        

        #region Connection settings

        // todo. kls. move
        public struct OpcUaEndPoint
        {
            private OpcUaEndPoint(int port)
            {
                Address = IPAddress.None;
                Hostname = null;
                Port = port;
            }
            public OpcUaEndPoint(string hostname, int port) : this(port)
            {
                Hostname = hostname;
            }
            public OpcUaEndPoint(IPAddress address, int port) : this(port)
            {
                Address = address;
            }

            public readonly int Port;
            /// <summary> Set <see cref="Hostname"/> to null if you want to use <see cref="Address"/>. </summary>
            public readonly IPAddress Address;
            /// <summary> If <see cref="Hostname"/> is not null or whitespace, then <see cref="Address"/> is ignored. </summary>
            public readonly string Hostname;
            public string AddressOrHostName => string.IsNullOrWhiteSpace(Hostname) ? Address?.ToString() : Hostname;
            public string Uri => GetUriStringForEndpoint(this);
            public static string GetUriStringForEndpoint(OpcUaEndPoint ep) => GetUriStringForEndpoint(ep.AddressOrHostName, ep.Port);
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
        public string UserName { get; set; } = "Anonymous";

        public string Password { get; set; } = "";
        public bool HasUserCertificate { get; set; }

        public bool HasSecurityNone { get; set; }
        public bool HasSecurityBasic128 { get; set; }
        public bool HasSecurityBasic256 { get; set; }

        public OpcUaSecurityLevel SecurityBasic128Level { get; set; }
        public OpcUaSecurityLevel SecurityBasic256Level { get; set; }

        public static int DefaultPort { get; } = 48080;
        public static string DefaultHostname = "localhost";
        public static OpcUaEndPoint DefaultEndPoint => new OpcUaEndPoint(DefaultHostname, DefaultPort);

        public List<OpcUaEndPoint> Endpoints = new List<OpcUaEndPoint>(); // todo. kls. readonly


        #endregion

        /// <summary> Creates a deep copy </summary>
        public object Clone()
        {
            OpcUaData newObj = (OpcUaData)MemberwiseClone(); 
            newObj.Endpoints = new List<OpcUaEndPoint>(Endpoints);
            return newObj;
        }

        public void ResetToDefaults()
        {
            Enabled = true; // todo. kls.
            Endpoints.Clear();
            Endpoints.Add(DefaultEndPoint);
        }

        public override bool Equals(object obj)
        {
            var other = obj as OpcUaData;
            if (other == null)
            {
                return false;
            }

            return
                Enabled == other.Enabled
                && Endpoints.SequenceEqual(other.Endpoints); // todo. kls. 
        }

        public override int GetHashCode()
        {
            // we do not need a special hash here
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        public override string ToString()
        {
            string epString = "";
            if (Endpoints.Count == 0)
                epString = "None";
            else if (Endpoints.Count == 1)
                epString = Endpoints[0].Uri;

            return $"({Enabled}, EP:[{epString}]";
        }
    }
}
