using System;

namespace iba.Data
{
    [Serializable]
    public class OpcUaData : ICloneable
    {
        public OpcUaData()
        {
            ResetToDefaults();
        }

        public bool Enabled { get; set; }


        #region Connection settings

        public int Port { get; set; }

        public string EndPointString = "";

        #endregion

        public object Clone()
        {
            var newObj = MemberwiseClone();
            return newObj;
        }

        public void ResetToDefaults()
        {
            Enabled = true; // todo. kls. 
            Port = 21060; // todo. kls. 
        }

        public override bool Equals(object obj)
        {
            var other = obj as OpcUaData;
            if (other == null)
            {
                return false;
            }

            return
                Enabled == other.Enabled &&
                Port == other.Port;
        }

        public override int GetHashCode()
        {
            // we do not need a special hash here
            // ReSharper disable once BaseObjectGetHashCodeCallInGetHashCode
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return $"({Enabled}, {Port})";
        }
    }
}
