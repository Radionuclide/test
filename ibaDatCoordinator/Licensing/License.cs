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
        public string LicenseIdentifier; //License identifier in Wibu dongle (PC 1000), serial number in Marx dongle, empty for license service

        public DateTime LastCheck;
        internal object SourceInfo;

        public bool ContainerFound => !String.IsNullOrEmpty(ContainerType);

        public bool IsWibu => ContainerType?.StartsWith("WIBU") ?? false;
        public bool IsMarx => ContainerType?.StartsWith("MARX") ?? false;
        public bool IsLicenseService => ContainerType?.StartsWith("ibaLicense") ?? false;

        public string FullId
        {
            get
            {
                if (!ContainerFound)
                    return "No container";
                else if (!String.IsNullOrEmpty(LicenseIdentifier))
                    return $"{LicenseIdentifier} {ContainerId}";
                else
                    return ContainerId;
            }
        }

        internal void Update(License newLic)
        {
            System.Diagnostics.Debug.Assert(LicenseId == newLic.LicenseId);
            LicenseOk = newLic.LicenseOk;
            ContainerId = newLic.ContainerId;
            ContainerType = newLic.ContainerType;
            LicenseIdentifier = newLic.LicenseIdentifier;
            LastCheck = newLic.LastCheck;
            SourceInfo = newLic.SourceInfo;
        }
    }
}
