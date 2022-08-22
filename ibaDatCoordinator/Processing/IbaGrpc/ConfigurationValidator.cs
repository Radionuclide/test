using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraPrinting.Native;
using iba.Data;
using iba.HD.Client.Properties;
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
                        return CreateConnectionResponse(Status.Error, string.Format(Properties.Resources.IncompatibleClientVersion, DatCoVersion.MinimumClientVersion()));
                    }

                    if (!CheckIfDirectoryIsFree(configuration))
                    {
                        return CreateConnectionResponse(Status.Error, string.Format(Properties.Resources.DirectoryAlreadyInUse, configuration.Path) );
                    }

                    if (!CheckAuthorization(configuration))
                    {
                        return CreateConnectionResponse(Status.Error, Properties.Resources.Authorizationfailed);
                    }
                    if (!CheckRemotePath(configuration))
                    {
                        return CreateConnectionResponse(Status.Error, string.Format(Properties.Resources.RemotePathNotValid, configuration.Path));
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

        private bool CheckRemotePath(Configuration configuration)
        {
            var remotePath = configuration.Path.Trim('/', '\\');

            if (Path.IsPathRooted(remotePath))
            {
                return false;
            }

            var fullPath = Path.GetFullPath(Path.Combine(DirectoryManager.GetRootPath(), remotePath));
            
            return DirectoryManager.IsValidPath(fullPath);
        }

        private bool CheckIfDirectoryIsFree(Configuration configuration)
        {
            var confId = Guid.Parse(configuration.ConfigurationId);

            var fullPath = DirectoryManager.GetFullPath(configuration.Path);

            return m_clientManager.ClientList
                .Where(client => client.Key != confId)
                .Select(client => client.Value.Path)
                .All(path => path != fullPath);
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
