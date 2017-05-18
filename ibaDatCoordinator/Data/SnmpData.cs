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

        #region Connection settings

        public int Port { get; set; }
        public string V1V2Security { get; set; }
        public IbaSnmpUserAccount V3Security { get; set; } = new IbaSnmpUserAccount();

        #endregion

        public object Clone()
        {
            //todo check V3Security?
            object newobj = MemberwiseClone();
            return newobj;
        }

        public static SnmpData GetDefaults()
        {
            SnmpData snmpd = new SnmpData();
            snmpd.ResetToDefaults();
            return snmpd;
        }

        public void ResetToDefaults()
        {
            Enabled = false;

            Port = IbaSnmp.DefaultLocalPortBase - 1 + (int)IbaSnmpProductId.IbaDatCoordinator;

            V1V2Security = "public";
            V3Security = new IbaSnmpUserAccount(
                "public", "12345678", IbaSnmpAuthenticationAlgorithm.Md5,
                "12345678", IbaSnmpEncryptionAlgorithm.None);
        }
    }
}
