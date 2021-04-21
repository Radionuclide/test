using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using iba.Utility;
using IbaSnmpLib;

namespace iba.Data
{
    [Serializable]
    public class SnmpData : ICloneable, IXmlSerializable
    {
        public SnmpData()
        {
            ResetToDefaults();
        }

        public bool Enabled { get; set; }

        public bool UseSnmpV2TcForStrings { get; set; }


        #region Connection settings

        public int Port { get; set; }
        public string V1V2Security { get; set; }

        public IbaSnmpUserAccount V3Security { get; set; } = new IbaSnmpUserAccount();

        #endregion

        public object Clone()
        {
            var newObj = MemberwiseClone();
            return newObj;
        }

        public void ResetToDefaults()
        {
            Enabled = false;

            UseSnmpV2TcForStrings = true;

            Port = IbaSnmp.DefaultLocalPortBase - 1 + (int)IbaSnmpProductId.IbaDatCoordinator;

            V1V2Security = "public";
            V3Security = new IbaSnmpUserAccount(
                "public", "12345678", IbaSnmpAuthenticationAlgorithm.Md5,
                "12345678", IbaSnmpEncryptionAlgorithm.None);
        }

        public override bool Equals(object obj)
        {
            var other = obj as SnmpData;
            if (other == null)
            {
                return false;
            }

            return
                Enabled == other.Enabled &&
                UseSnmpV2TcForStrings == other.UseSnmpV2TcForStrings &&
                Port == other.Port &&
                V1V2Security == other.V1V2Security &&
                V3Security == other.V3Security;
        }

        public override string ToString()
        {
            return $"({Enabled}, {Port}, {V1V2Security})";
        }

        public override int GetHashCode()
        {
            // we do not need a special hash here
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        #region XML serialization (required to encrypt the secrets from V3Security)

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            try
            {
                reader.ReadStartElement();

                do
                {
                    switch (reader.Name)
                    {
                        case "Enabled":
                            Enabled = reader.ReadElementContentAsBoolean();
                            break;

                        case "UseSnmpV2TcForStrings":
                            UseSnmpV2TcForStrings = reader.ReadElementContentAsBoolean();
                            break;

                        case "Port":
                            Port = reader.ReadElementContentAsInt();
                            break;

                        case "V1V2Security":
                            V1V2Security = reader.ReadElementContentAsString();
                            break;

                        case "V3Security":
                            ReadV3Security(reader);
                            break;

                        default:
                            if (!reader.Read())
                                return;
                            break;
                    }
                }
                while (reader.NodeType != XmlNodeType.EndElement);

                reader.ReadEndElement();
            }
            catch (Exception)
            {
            }
        }

        private void ReadV3Security(XmlReader reader)
        {
            reader.ReadStartElement();

            do
            {
                switch (reader.Name)
                {
                    case "Username":
                        V3Security.Username = reader.ReadElementContentAsString();
                        break;

                    case "Password":
                        V3Security.Password = reader.ReadElementContentAsString(); //Plain text from old configuration
                        break;

                    case "PasswordCrypted":
                        V3Security.Password = Crypt.Decrypt(reader.ReadElementContentAsString());
                        break;

                    case "EncryptionKey":
                        V3Security.EncryptionKey = reader.ReadElementContentAsString(); //Plain text from old configuration
                        break;

                    case "EncryptionKeyCrypted":
                        V3Security.EncryptionKey = Crypt.Decrypt(reader.ReadElementContentAsString());
                        break;

                    case "AuthAlgorithm":
                        string authText = reader.ReadElementContentAsString();
                        if (Enum.TryParse(authText, out IbaSnmpAuthenticationAlgorithm auth))
                            V3Security.AuthAlgorithm = auth;
                        break;

                    case "EncrAlgorithm":
                        string encrText = reader.ReadElementContentAsString();
                        if (Enum.TryParse(encrText, out IbaSnmpEncryptionAlgorithm encr))
                            V3Security.EncrAlgorithm = encr;
                        break;

                    default:
                        if (!reader.Read())
                            return;
                        break;
                }
            }
            while (reader.NodeType != XmlNodeType.EndElement);

            reader.ReadEndElement();
        }

        public void WriteXml(XmlWriter writer)
        {
            writer.WriteElementString("Enabled", Enabled ? "true" : "false");
            writer.WriteElementString("UseSnmpV2TcForStrings", UseSnmpV2TcForStrings ? "true" : "false");
            writer.WriteElementString("Port", Port.ToString());
            writer.WriteElementString("V1V2Security", V1V2Security);

            writer.WriteStartElement("V3Security");
            writer.WriteElementString("Username", V3Security.Username);
            writer.WriteElementString("PasswordCrypted", Crypt.Encrypt(V3Security.Username));
            writer.WriteElementString("EncryptionKeyCrypted", Crypt.Encrypt(V3Security.EncryptionKey));
            writer.WriteElementString("AuthAlgorithm", V3Security.AuthAlgorithm.ToString());
            writer.WriteElementString("EncrAlgorithm", V3Security.EncrAlgorithm.ToString());
            writer.WriteEndElement();
        }

        #endregion
    }
}
