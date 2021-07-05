using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iba.Data;
using iba.Logging;

namespace iba.Processing.IbaGrpc
{
    internal class ClientManager
    {
        public delegate void UpdateEvent(DiagnosticsData diagnosticsDatacount);

        public event UpdateEvent UpdateDiagnosticInfoCallback;
        private BindingList<DiagnosticsData> m_connectedClients;

        public ClientManager()
        {
            m_connectedClients = new BindingList<DiagnosticsData>();

        }
        public BindingList<DiagnosticsData> ConnectedClients
        {
            get => m_connectedClients;
            set => m_connectedClients = value;
        }

        public void RegisterClient(DiagnosticsData diagnosticsData)
        {
            try
            {
                if (m_connectedClients.Any(x => x.ClientName != diagnosticsData.ClientName))
                {
                    m_connectedClients.Add(diagnosticsData);
                    UpdateDiagnosticsInfo(diagnosticsData);
                }

                if (m_connectedClients.All(x => diagnosticsData.ClientName != x.ClientName))
                {
                    m_connectedClients.Add(diagnosticsData);
                }
            }
            catch (Exception e)
            {
                LogData.Data.Log(Level.Exception, e.Message);
            }
        }

        public void UpdateDiagnosticsInfo(DiagnosticsData diagnosticsData)
        {
            var data = m_connectedClients.FirstOrDefault(x => x.ClientName == diagnosticsData.ClientName);
            
            if (data == null) return;
            
            data.TransferredFiles++;
            data.Filename = diagnosticsData.Filename;
            data.Path = diagnosticsData.Path;

            UpdateDiagnosticInfoCallback?.Invoke(data);
        }
    }
}
