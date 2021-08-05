using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraPrinting.Native;
using iba.Data;
using iba.Logging;
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
                try
                {
                    if (!CheckVersion(configuration))
                    {
                        return CreateConnectionResponse(Status.Error, "Client version does not match Server Version");
                    }

                    if (!CheckIfDirectoryIsFree(configuration))
                    {
                        return CreateConnectionResponse(Status.Error, "Directory already in use");
                    }

                    if (!CheckAuthorization(configuration))
                    {
                        return CreateConnectionResponse(Status.Error, "Authorization failed");
                    }
                }
                catch (Exception e)
                {
                    LogData.Data.Log(Level.Exception, $"{e.Message} \n {e?.InnerException} \n {e.StackTrace}");
                    return CreateConnectionResponse(Status.Error, e.Message);
                }

                return CreateConnectionResponse(Status.Ok, string.Empty);
            });
        }

        private bool CheckIfDirectoryIsFree(Configuration configuration)
        {
            var confId = Guid.Parse(configuration.ConfigurationId);

            return m_clientManager.ClientList
                .Where(conf => conf.Key != confId)
                .All(conf => conf.Value.Path != configuration.Path);
        }

        private static bool CheckVersion(Configuration configuration)
        {
            return configuration.ClientVersion == DatCoVersion.GetVersion();
        }

        private static bool CheckAuthorization(Configuration configuration)
        {
            //Todo
            return true;
        }

        private ConnectionResponse CreateConnectionResponse(Status status, string message)
        {
            return new ConnectionResponse
            {
                Status = status,
                Message = message
            };
        }
    }
}
