using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iba.Data;
using iba.Logging;
using Messages.V1;

namespace iba.Processing.IbaGrpc
{
    internal class ClientManager
    {
        public delegate void UpdateEvent(DiagnosticsData diagnosticsDatacount);

        public event UpdateEvent UpdateDiagnosticInfoCallback;
        private List<DiagnosticsData> m_diagnosticsList;
        private ConcurrentDictionary<Guid, Configuration> m_clientList;

        public ClientManager()
        {
            m_diagnosticsList = new List<DiagnosticsData>();
            m_clientList = new ConcurrentDictionary<Guid, Configuration>();

        }
        public List<DiagnosticsData> ConnectedClients
        {
            get => m_diagnosticsList;
            set => m_diagnosticsList = value;
        }

        public ConcurrentDictionary<Guid, Configuration> ClientList
        {
            get => m_clientList;
            set => m_clientList = value;
        }

        public void RegisterClient(DiagnosticsData diagnosticsData)
        {
            try
            {
                if (m_diagnosticsList.Any(x => x.ClientName != diagnosticsData.ClientName))
                {
                    m_diagnosticsList.Add(diagnosticsData);
                    UpdateDiagnosticsInfo(diagnosticsData);
                }

                if (m_diagnosticsList.All(x => diagnosticsData.ClientName != x.ClientName))
                {
                    m_diagnosticsList.Add(diagnosticsData);
                }
            }
            catch (Exception e)
            {
                LogData.Data.Log(Level.Exception, e.Message);
            }
        }

        public void UpdateDiagnosticsInfo(DiagnosticsData diagnosticsData)
        {
            var data = m_diagnosticsList.FirstOrDefault(x => x.ClientName == diagnosticsData.ClientName);
            
            if (data == null) return;
            
            data.TransferredFiles++;
            data.Filename = diagnosticsData.Filename;
            data.Path = diagnosticsData.Path;

            UpdateDiagnosticInfoCallback?.Invoke(data);
        }
    }
}
