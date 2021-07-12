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
    public class ClientManager
    {
        public delegate void UpdateEvent(DiagnosticsData diagnosticsDatacount);

        public event UpdateEvent UpdateDiagnosticInfoCallback;
        private ConcurrentDictionary<Guid, Configuration> m_clientList;

        public ClientManager()
        {
            m_clientList = new ConcurrentDictionary<Guid, Configuration>();

        }

        public ConcurrentDictionary<Guid, Configuration> ClientList 
        {
            get => m_clientList;
            set => m_clientList = value;
        }

        public void RegisterClient(Guid newGuid, Configuration configuration)
        {
            try
            {
                m_clientList.TryAdd(newGuid, configuration);
            }
            catch (Exception e)
            {
                LogData.Data.Log(Level.Exception, e.Message);
            }
        }
        public void UpdateClient(string requestClientId, Configuration requestConfigurataion)
        {
            try
            {
                m_clientList[Guid.Parse(requestClientId)].ClientName = requestConfigurataion.ClientName;
                m_clientList[Guid.Parse(requestClientId)].TaskName = requestConfigurataion.TaskName;
                m_clientList[Guid.Parse(requestClientId)].Path = requestConfigurataion.Path;
                m_clientList[Guid.Parse(requestClientId)].FileName = requestConfigurataion.FileName;
                m_clientList[Guid.Parse(requestClientId)].Maxbandwidth = requestConfigurataion.Maxbandwidth;
                m_clientList[Guid.Parse(requestClientId)].ApiKey = requestConfigurataion.ApiKey;
                m_clientList[Guid.Parse(requestClientId)].ClientVersion = requestConfigurataion.ClientVersion;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Configuration GetClientInfo(Guid guid)
        {
            return m_clientList[guid];
        }

        public void UpdateDiagnosticsInfo(Guid guid)
        {
            var data = GetClientInfo(guid);

            data.Transferredfiles++;

            var diagnosticsData = new DiagnosticsData
            {
                ClientId = guid.ToString(),
                TaskName = data.TaskName,
                Filename = data.FileName,
                ClientName = data.ClientName,
                ClientVersion = data.ClientVersion,
                Path = data.Path,
                MaxBandwidth = data.Maxbandwidth,
                TransferredFiles = data.Transferredfiles
            };

            UpdateDiagnosticInfoCallback?.Invoke(diagnosticsData);
        }
    }
}
