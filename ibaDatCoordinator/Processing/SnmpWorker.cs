using System;
using System.Collections.Generic;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using iba.Data;
using IbaSnmpLib;

namespace iba.Processing
{
    public enum SnmpWorkerStatus
    {
        Started,
        Stopped,
        Errored,
    }

    public class StatusChangedEventArgs : EventArgs
    {
        public SnmpWorkerStatus Status { get; }
        public Color Color { get; }
        public string Message { get; }
        public StatusChangedEventArgs(SnmpWorkerStatus status, Color color, string message)
        {
            Status = status;
            Color = color;
            Message = message;
        }
    }


    public class SnmpWorker
    {
        public IbaSnmp IbaSnmp { get; } =
            new IbaSnmp(IbaSnmpProductId.IbaDatCoordinator);

        public event EventHandler<StatusChangedEventArgs> StatusChanged;

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

        public SnmpWorkerStatus Status { get; private set; } = SnmpWorkerStatus.Stopped;
        public string StatusString { get; private set; }

        public void ApplyStatusToTextBox(TextBox tb)
        {
            // todo remove
            tb.Text = StatusString;
            tb.BackColor = StatusToColor(Status);
        }

        public static Color StatusToColor(SnmpWorkerStatus status)
        {
            return status == SnmpWorkerStatus.Started
                ? Color.LimeGreen // running
                : (status == SnmpWorkerStatus.Stopped
                    ? Color.LightGray // stopped
                    : Color.Red); // error
        }

        /// <summary> Whether Snmp Control is currently displayed in the main form window </summary>
        //public bool IsGuiVisible { get; set; }

        public void RestartAgent()
        {
            Status = SnmpWorkerStatus.Errored;
            StatusString = @"";

            try
            {
                IbaSnmp.Stop();
                Status = SnmpWorkerStatus.Stopped;
                // todo to resource
                StatusString = "SNMP server is disabled";

                ApplyConfigurationToIbaSnmp();

                if (_snmpData.Enabled)
                {
                    IbaSnmp.Start();
                    Status = SnmpWorkerStatus.Started;
                    // todo to resource
                    StatusString = $"SNMP server running on port {_snmpData.Port}";
                    //SNMP server is disabled
                }
            }
            catch (Exception ex)
            {
                Status = SnmpWorkerStatus.Errored;
                // todo to resource
                StatusString = $"Starting the SNMP server failed with error: {ex.Message}";
            }

            // trigger status event
            StatusChanged?.Invoke(this, 
                new StatusChangedEventArgs(Status, StatusToColor(Status), StatusString));

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
