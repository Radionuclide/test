using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Messages.V1;

namespace iba.Processing.IbaGrpc
{
    internal class ConfigurationValidator
    {
        public async Task<ConnectionResponse> CheckConfigurationAsync(Configuration configuration)
        {
            return await Task.Run(() =>
            {
                if (configuration.ClientVersion != DatCoVersion.GetVersion())
                {
                    return new ConnectionResponse
                    {
                        Status = "Client version does not match Server Version"
                    };
                }

                return new ConnectionResponse
                {
                    Status = "OK"
                };
            });
        }
    }
}
