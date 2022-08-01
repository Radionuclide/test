﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;
using iba.CertificateStore;
using iba.Utility;

namespace iba.Data
{
    [Serializable]
    public class KafkaWriterTaskData : TaskData, ICertifiable
    {
        public const string StartTime = "*STARTTIME*";
        public const string EndTime = "*ENDTIME*";
        public string clusterAddress { get; set; }
        public string topicName { get; set; }
        public double timeout { get; set; } //in seconds
        public List<KafkaRecord> Records { get; set; }
        public List<Param> Params { get; set; }
        public string identifier { get; set; }
        public DigitalFormat digitalFormat { get; set; }
        public string schemaRegistryAddress { get; set; }
        public string key { get; set; }
        public List<string> metadata { get; set; }
        public string timeStampExpression { get; set; }
        public TimestampUTCOffset timestampUTCOffset { get; set; }
        public MonitorData MonitorData { get; set; }
        public string TestDatFile { get; set; }
        public RequiredAcks AckMode { get; set; }
        public DataFormat Format { get; set; }
        public ClusterType ClusterMode { get; set; }
        public ClusterSecurityType ClusterSecurityMode { get; set; }
        public SchemaRegistrySecurityType SchemaRegistrySecurityMode { get; set; }
        public SASLMechanismType SASLMechanismMode { get; set; }
        public string SSLClientThumbprint { get; set; }
        public string SASLUsername { get; set; }
        [XmlIgnore]
        public string SASLPass { get; set; }

        public string SASLPassEncrypted {
            get { return Crypt.Encrypt(SASLPass); }
            set { SASLPass = Crypt.Decrypt(value); }
        }
        public bool enableSSLVerification { get; set; }
        public bool enableSchema { get; set; }
        public string schemaSSLClientThumbprint { get; set; }
        public string schemaUsername { get; set; }
        [XmlIgnore]
        public string schemaPass { get; set; }
        public string schemaPassEncrypted
        {
            get { return Crypt.Encrypt(schemaPass); }
            set { schemaPass = Crypt.Decrypt(value); }
        }
        public bool schemaEnableSSLVerification { get; set; }
        public string signalReference { get; set; }

        public enum TimestampUTCOffset
        {
            DoNotRespect = 0,
            ConvertToUniversalTime = 1,
            ConcatenateWithTimestamp = 2
        }

        public enum DigitalFormat
        {
            TrueFalse = 0,
            OneZero = 1
        }

        public enum DataFormat
        {
            JSONGrouped,
            JSONPerSignal,
            AVRO
        }
        public enum RequiredAcks
        {
            None = 0,   // No acknowledgments are required
            Leader, // Only an acknowledgment from the leader broker is required
            All     // An acknowledgment from all brokers is required
        }

        public enum ClusterType
        {
            Kafka = 0,
            EventHub = 1
        }

        public enum ClusterSecurityType
        {
            PLAINTEXT = 0,
            SSL = 1,
            SASL_PLAINTEXT = 2,
            SASL_SSL = 3
        }

        public enum SASLMechanismType
        {
            PLAIN = 0,
            SCRAM_SHA_256 = 1,
            SCRAM_SHA_512 = 2
        }

        public enum SchemaRegistrySecurityType
        {
            HTTP = 0,
            HTTPS = 1,
            HTTP_AUTHENTICATION = 2,
            HTTPS_AUTHENTICATION = 3
        }

        public string ToText(KafkaRecord rec)
        {
            if (rec.DataType == KafkaRecord.ExpressionType.Digital)
            {
                if (digitalFormat == DigitalFormat.TrueFalse)
                    return (double)rec.Value >= 0.5 ? "True" : "False";
                else
                    return (double)rec.Value >= 0.5 ? "1" : "0";
            }
            else 
                return ((double)rec.Value).ToString(System.Globalization.CultureInfo.InvariantCulture);
        }

        [Serializable]
        public class KafkaRecord : ICloneable
        {
            public static readonly string[] DataTypes =
            {
                "DOUBLE",
                "STRING",
                "BOOLEAN"
            };
            public enum ExpressionType
            {
                Double = 0,
                Text = 1,
                Digital = 2
            }

            [XmlIgnore]
            public string TestValue { get; set; }
            [XmlIgnore]
            public object Value { get; set; }
            public ExpressionType DataType { get; set; }
            [XmlIgnore]
            public string DataTypeAsString
            {
                get => DataTypes[(int)DataType];
                set
                {
                    if (value == DataTypes[(int)ExpressionType.Double])
                        DataType = ExpressionType.Double;
                    else if (value == DataTypes[(int)ExpressionType.Text])
                        DataType = ExpressionType.Text;
                    else if (value == DataTypes[(int)ExpressionType.Digital])
                        DataType = ExpressionType.Digital;
                }
            }
            public string Expression { get; set; }
            [XmlIgnore]
            public string Name { get; set; }
            [XmlIgnore]
            public string Comment1 { get; set; }
            [XmlIgnore]
            public string Comment2 { get; set; }
            [XmlIgnore]
            public string Unit { get; set; }

            public KafkaRecord()
            {
                Expression = "";
                Value = double.NaN;
                Name = "";
                Comment1 = "";
                Comment2 = "";
                Unit = "";
            }

            public object Clone()
            {
                return new KafkaRecord
                {
                    Expression = this.Expression,
                    Value = this.Value,
                    DataType = this.DataType,
                    Name = this.Name,
                    Comment1 = this.Comment1,
                    Comment2 = this.Comment2,
                    Unit = this.Unit
                };
            }
            public override bool Equals(object obj)
            {
                if (!(obj is KafkaRecord))
                    return false;
                var rec = (KafkaRecord)obj;
                return
                    Expression == rec.Expression &&
                    Name == rec.Name &&
                    DataType == rec.DataType &&
                    Comment1 == rec.Comment1 &&
                    Comment2 == rec.Comment2 &&
                    Unit == rec.Unit;
            }

            public override int GetHashCode()
            {
                return Expression.GetHashCode() ^ DataType.GetHashCode() ^ Name.GetHashCode();
            }
        }


        [Serializable]
        public class Param : ICloneable
        {
            public string Key { get; set; }
            public string Value { get; set; }

            public object Clone()
            {
                return new Param
                {
                    Key = this.Key,
                    Value = this.Value
                };
            }
        }

        public KafkaWriterTaskData(ConfigurationData parent) : base(parent)
        {
            Records = new List<KafkaRecord>();
            Params = new List<Param>();
            metadata = new List<string>();
            MonitorData = new MonitorData();
            timeout = 3;
            Format = DataFormat.JSONGrouped;
            digitalFormat = DigitalFormat.TrueFalse;
            identifier = "";
            AnalysisFile = "";
            topicName = "";
            clusterAddress = "";
            schemaRegistryAddress = "";
            key = "";
            TestDatFile = "";
            AckMode = RequiredAcks.None;
            ClusterMode = ClusterType.Kafka;
            ClusterSecurityMode = ClusterSecurityType.PLAINTEXT;
            SchemaRegistrySecurityMode = SchemaRegistrySecurityType.HTTP;
            SASLMechanismMode = SASLMechanismType.PLAIN;
            SSLClientThumbprint = "";
            SASLUsername = "";
            SASLPass = "";
            schemaSSLClientThumbprint = "";
            schemaUsername = "";
            schemaPass = "";
            signalReference = "$signalname";
            enableSchema = false;
            timeStampExpression = StartTime;
            timestampUTCOffset = TimestampUTCOffset.DoNotRespect;
        }

        public KafkaWriterTaskData() : this(null) { }

        public override TaskData CloneInternal()
        {
            KafkaWriterTaskData d = new KafkaWriterTaskData(null);
            d.Records = Records.Select(r => (KafkaRecord)r.Clone()).ToList();
            d.Params = Params.Select(r => (Param)r.Clone()).ToList();
            d.MonitorData = (MonitorData)MonitorData.Clone();
            d.timeout = timeout;
            d.Format = Format;
            d.digitalFormat = digitalFormat;
            d.identifier = identifier;
            d.AnalysisFile = AnalysisFile;
            d.topicName = topicName;
            d.clusterAddress = clusterAddress;
            d.schemaRegistryAddress = schemaRegistryAddress;
            d.AckMode = AckMode;
            d.ClusterMode = ClusterMode;
            d.ClusterSecurityMode = ClusterSecurityMode;
            d.SchemaRegistrySecurityMode = SchemaRegistrySecurityMode;
            d.key = key;
            d.TestDatFile = TestDatFile;
            d.metadata = metadata.Select(r => (string)r.Clone()).ToList();
            d.Name = Name;
            d.SASLMechanismMode = SASLMechanismMode;
            d.SSLClientThumbprint = SSLClientThumbprint;
            d.SASLUsername = SASLUsername;
            d.SASLPass = SASLPass;
            d.enableSSLVerification = enableSSLVerification;
            d.schemaSSLClientThumbprint = schemaSSLClientThumbprint;
            d.schemaUsername = schemaUsername;
            d.schemaPass = schemaPass;
            d.signalReference = signalReference;
            d.schemaEnableSSLVerification = schemaEnableSSLVerification;
            d.enableSchema = enableSchema;
            d.timeStampExpression = timeStampExpression;
            d.timestampUTCOffset = timestampUTCOffset;
            return d;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            if (!(taskData is KafkaWriterTaskData other)) return false;
            if (other == this) return true;

            return
                Records.SequenceEqual(other.Records) &&
                Params.SequenceEqual(other.Params) &&
                MonitorData.IsSame(other.MonitorData) &&
                timeout == other.timeout &&
                Format == other.Format &&
                digitalFormat == other.digitalFormat &&
                identifier == other.identifier &&
                AnalysisFile == other.AnalysisFile &&
                topicName == other.topicName &&
                clusterAddress == other.clusterAddress &&
                schemaRegistryAddress == other.schemaRegistryAddress &&
                AckMode == other.AckMode &&
                ClusterMode == other.ClusterMode &&
                ClusterSecurityMode == other.ClusterSecurityMode &&
                SchemaRegistrySecurityMode == other.SchemaRegistrySecurityMode &&
                key == other.key &&
                TestDatFile == other.TestDatFile &&
                metadata.SequenceEqual(other.metadata) &&
                SASLMechanismMode == other.SASLMechanismMode &&
                SSLClientThumbprint == other.SSLClientThumbprint &&
                SASLUsername == other.SASLUsername &&
                SASLPass == other.SASLPass &&
                enableSSLVerification == other.enableSSLVerification &&
                schemaSSLClientThumbprint == other.schemaSSLClientThumbprint &&
                schemaUsername == other.schemaUsername &&
                schemaPass == other.schemaPass &&
                signalReference == other.signalReference &&
                schemaEnableSSLVerification == other.schemaEnableSSLVerification &&
                enableSchema == other.enableSchema &&
                timeStampExpression == other.timeStampExpression &&
                timestampUTCOffset == other.timestampUTCOffset;
        }

        public static bool IsAnalyzerVersionNewer(IbaAnalyzer.IbaAnalyzer ibaAnalyzer, int majorMinVersion, int minorMinVersion = -1, int bugfixMinVersion = -1)
        {
            int majorRef = 0, minorRef = 0, bugfixRef = 0;
            if (!GetIbaAnalyzerVersion(ibaAnalyzer, ref majorRef, ref minorRef, ref bugfixRef))
                return false;
            if (majorRef < majorMinVersion)
                return false;
            if (minorMinVersion >= 0 && majorRef == majorMinVersion && minorRef < minorMinVersion)
                return false;
            if (bugfixMinVersion >= 0 && majorRef == majorMinVersion && minorRef == minorMinVersion && bugfixRef < bugfixMinVersion)
                return false;
            return true;
        }
        public static bool GetIbaAnalyzerVersion(IbaAnalyzer.IbaAnalyzer ibaAnalyzer, ref int major, ref int minor, ref int bugfix)
        {
            string version = ibaAnalyzer.GetVersion();
            int startindex = version.IndexOf(' ') + 1;
            if (version[startindex] == 'v') startindex++;
            int stopindex = startindex + 1;
            while (stopindex < version.Length && (char.IsDigit(version[stopindex]) || version[stopindex] == '.'))
                stopindex++;
            string[] nrs = version.Substring(startindex, stopindex - startindex).Split('.');
            if (nrs.Length < 3)
            {
                return false;
            }
            if (!int.TryParse(nrs[0], out major))
            {
                return false;
            }
            if (!int.TryParse(nrs[1], out minor))
            {
                return false;
            }
            if (!int.TryParse(nrs[2], out bugfix))
            {
                return false;
            }
            return true;
        }

        public IEnumerable<ICertifiable> GetCertifiableChildItems()
        {
            yield break;
        }

        public IEnumerable<ICertificateInfo> GetCertificateInfo()
        {
            if (SSLClientThumbprint != null && SSLClientThumbprint != "")
                yield return new CertificateInfoForwarder(
                       () => SSLClientThumbprint,
                       value => SSLClientThumbprint = value,
                       CertificateRequirement.Valid | CertificateRequirement.Trusted | CertificateRequirement.PrivateKey,
                       m_name);
        }

        public override int RequiredLicense => Licensing.LicenseId.Publish;
    }
}
