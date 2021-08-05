using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraPrinting.Native;
using Messages.V1;

namespace iba.Processing.IbaGrpc
{
    internal class ConfigurationValidator
    {
        private readonly ClientManager m_clientManager;

        public ConfigurationValidator(ClientManager clientManager)
        {
            m_clientManager = clientManager;
        }

        public async Task<ConnectionResponse> CheckConfigurationAsync(Configuration configuration)
        {
            return await Task.Run(() =>
            {
                if (!CheckVersion(configuration))
                {
                    return new ConnectionResponse
                    {
                        Status = "Client version does not match Server Version"
                    };
                }

                if (!CheckIfDirectoryIsFree(configuration))
                {
                    return new ConnectionResponse
                    {
                        Status = "Directory already exists"
                    };
                }

                return new ConnectionResponse
                {
                    Status = "OK"
                };
            });
        }

        public bool CheckIfDirectoryIsFree(Configuration configuration)
        {
            var confId = Guid.Parse(configuration.ConfigurationId);

            return m_clientManager.ClientList
                .Where(conf => conf.Key != confId)
                .All(conf => conf.Value.Path != configuration.Path);
        }

        public bool CheckVersion(Configuration configuration)
        {
            return configuration.ClientVersion == DatCoVersion.GetVersion();
        }
    }
}
