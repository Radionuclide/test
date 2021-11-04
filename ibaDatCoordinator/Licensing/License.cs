using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Licensing
{
    class License
    {
        public License(int licId)
        {
            LicenseId = licId;
            LastCheck = DateTime.UtcNow;
        }

        public int LicenseId;

        public bool LicenseOk;
        
        public string ContainerId; //The hardware id of the container + location
        public string ContainerType; //MARX or license service or WIBU

        public DateTime LastCheck;
        internal object SourceInfo;

        public bool ContainerFound => !String.IsNullOrEmpty(ContainerType);

        public bool IsWibu => ContainerType?.StartsWith("WIBU") ?? false;
        public bool IsMarx => ContainerType?.StartsWith("MARX") ?? false;
        public bool IsLicenseService => ContainerType?.StartsWith("ibaLicense") ?? false;

    }
}
