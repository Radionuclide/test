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

        public Configuration GetClientInfo(Guid guid)
        {
            return m_clientList[guid];
        }

        public void AddOrUpdate(Configuration configuration)
        {
            var confId = Guid.Parse(configuration.ConfigurationId);

            m_clientList.AddOrUpdate(confId, configuration,
                (guid, newConfiguration) =>
                {
                    newConfiguration.FileName = configuration.FileName;
                    newConfiguration.Path = configuration.Path;
                    newConfiguration.ApiKey = configuration.ApiKey;
                    newConfiguration.ClientName = configuration.ClientName;
                    newConfiguration.ClientVersion = configuration.ClientVersion;
                    newConfiguration.TaskName = configuration.TaskName;
                    newConfiguration.RequestDate = configuration.RequestDate;
                    newConfiguration.Maxbandwidth = configuration.Maxbandwidth;
                    return newConfiguration;
                });
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
