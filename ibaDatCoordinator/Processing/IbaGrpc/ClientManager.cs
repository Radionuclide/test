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
    class ClientManager
    {
        public delegate void UpdateEvent(BindingList<DiagnosticsData> diagnosticsDatacount);

        public event UpdateEvent UpdateCountCallback;
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
                if (m_connectedClients.Any(x => x.CientName != diagnosticsData.CientName))
                {
                    m_connectedClients.Add(diagnosticsData);
                    IncreaseClientTransferredFilesCount(diagnosticsData);
                }

            }
            catch (Exception e)
            {
                LogData.Data.Log(Level.Exception, e.Message);
            }

            if (m_connectedClients.All(x => diagnosticsData.CientName != x.CientName))
            {
                m_connectedClients.Add(diagnosticsData);
            }
        }
        private int count = 0;

        public void IncreaseClientTransferredFilesCount(DiagnosticsData diagnosticsData)
        {
            Debug.WriteLine("count: " + count++);

            m_connectedClients.First(x => x.CientName == diagnosticsData.CientName).TransferredFiles++;
            if (UpdateCountCallback != null)
            {
                UpdateCountCallback(m_connectedClients);
            }
        }
    }
}
