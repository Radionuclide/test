using System;
using IbaSnmpLib;

namespace iba.Data
{
    [Serializable]
    public class SnmpData : ICloneable
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
    }
}
