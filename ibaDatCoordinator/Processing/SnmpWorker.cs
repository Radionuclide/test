using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using DevExpress.Accessibility;
using iba.Data;
using IbaSnmpLib;

namespace iba.Processing
{
    public class SnmpWorker
    {
        public IbaSnmp IbaSnmp { get; } =
            new IbaSnmp(IbaSnmpProductId.IbaDatCoordinator);

        private SnmpData _snmpData;

        public SnmpData SnmpData
        {
            get { return _snmpData; }
            set
            {
                // todo probabaly override Equals, because def implementation does not work good enough for this
                //  if (_snmpData != null && _snmpData.Equals(value))
                if (_snmpData != null && _snmpData.Equals(value))
                {
                    return;
                }
                _snmpData = value;
                RestartAgent();
            }
        }

        public void RestartAgent()
        {
            try
            {
                IbaSnmp.Stop();

                ApplyConfigurationToIbaSnmp();

                if (_snmpData.Enabled)
                {
                    IbaSnmp.Start();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void ApplyConfigurationToIbaSnmp()
        {
            if (IbaSnmp == null || SnmpData == null)
            {
                return;
            }

            // apply port, do not change ip addresses
            List<IPEndPoint> eps = IbaSnmp.EndPointsToListen;
            foreach (IPEndPoint ipe in eps)
            {
                ipe.Port = SnmpData.Port;
            }
            IbaSnmp.EndPointsToListen = eps;

            // security
            IbaSnmp.SetSecurityForV1AndV2(new List<string> { SnmpData.V1V2Security });
            IbaSnmp.SetSecurityForV3(new List<IbaSnmpUserAccount> { SnmpData.V3Security });

            // todo apply objects
            //SnmpData.
        }

    }

}
