using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IbaSnmpLib;

namespace iba.Data
{
    [Serializable]
    public class SnmpData : ICloneable
    {
        private IbaSnmp _ibaSnmp = null;
        /// <summary> reference to the SNMP agent engine </summary>
        public IbaSnmp IbaSnmp
        {
            get { return _ibaSnmp; }
            set { _ibaSnmp = value; }
        }

        private bool _restartByDefault = false;
        public bool RestartByDefault
        {
            get { return _restartByDefault; }
            set { _restartByDefault = value; }
        }


        public SnmpData()
        {
        }

        /// <summary> reference to the SNMP agent engine </summary>
        
        public object Clone()
        {
            //todo check
            object newobj = MemberwiseClone();
            return newobj;
        }
    }
}
