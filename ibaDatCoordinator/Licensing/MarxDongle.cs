using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Licensing
{
    class MarxDongle
    {
        public MarxDongle()
        {
            contents = new LicenseContents();
            lastRead = DateTime.MinValue;
        }

        LicenseContents contents;
        byte[] options;
        DateTime lastRead;

        public LicenseContents Contents => contents;

        public void ReadContents()
        {
            LicenseContents contents = new LicenseContents();
            byte[] options = null;

            CDongleInfo dongleInfo = CDongleInfo.ReadDongle();
            if (dongleInfo.DongleFound)
            {
                contents.ContainerType = dongleInfo.DongleType;
                contents.ContainerId = dongleInfo.HwId;
                contents.Identifier = dongleInfo.SerialNr;
                contents.Customer = dongleInfo.Customer;
                if (dongleInfo.TimeLimit > 0)
                    contents.DemoDays = LicenseHelper.DateTimeToDays(DateTime.Today + TimeSpan.FromDays(dongleInfo.TimeLimit));
                else if (dongleInfo.TimeLimit < 0)
                    contents.DemoDays = LicenseHelper.DateTimeToDays(DateTime.Today - TimeSpan.FromDays(1));
                else
                    contents.DemoDays = 0;

                options = dongleInfo.Options;
            }
            else if (IsLicenseServiceRunning())
                contents.ContainerType = "ibaLicenseService";

            this.lastRead = DateTime.UtcNow;
            this.contents = contents;
            this.options = options;
        }

        private bool IsLicenseServiceRunning()
        {
            var procs = Process.GetProcessesByName("ibaLicenseService");
            if (procs.Length == 0)
                return false;

            bool bRunning = false;
            foreach(var proc in procs)
            {
                try
                {
                    if (!proc.HasExited)
                        bRunning = true;
                }
                catch(Exception)
                {
                    //An exception can occur when ibaDatCoordinator isn't running with administrator rights
                    bRunning = true;
                }

                proc.Dispose();
            }
            return bRunning;
        }

        public License AcquireLicense(LicenseOptionInfo option)
        {
            License lic = new License(option.Id);

            int[] ids = option.MarxIds;
            if (ids == null || ids.Length == 0)
                return lic;

            //Check if there is a container and if it hasn't been too long since the last read
            if (!contents.ContainerFound || Math.Abs((DateTime.UtcNow - lastRead).TotalSeconds) > 120)
            {
                ReadContents();
                if (!contents.ContainerFound)
                    return lic;
            }

            bool licenseServiceAllowed = ids.All(id => id < 1000); //The ibaDatCoordinator plugin bits can't come from the license service

            if (licenseServiceAllowed && IsLicenseServiceRunning())
            {
                lic.ContainerType = "ibaLicenseService";
                lic.ContainerId = "ibaLicenseService";
                lic.LicenseIdentifier = "";

                //Get license from license service
                int usedId = CDongleInfo.AcquireAnyLicenseFromLicenseService(ids);
                if (usedId < 0)
                    return lic;

                lic.LicenseOk = true;
                lic.SourceInfo = usedId;
                lic.LastCheck = DateTime.UtcNow;
                return lic;
            }

            //Check dongle info
            foreach (int id in ids)
            {
                if (IsOptionAvailable(id))
                {
                    lic.LicenseOk = true;
                    lic.SourceInfo = id;
                    lic.LastCheck = DateTime.UtcNow;
                    lic.ContainerType = contents.ContainerType;
                    lic.ContainerId = contents.ContainerId;
                    lic.LicenseIdentifier = contents.Identifier;
                    return lic;
                }
            }

            return lic;
        }

        public bool CheckLicense(License lic)
        {
            int id = (int)lic.SourceInfo;
            if(lic.IsLicenseService)
            {
                //Check license service
                int usedId = CDongleInfo.AcquireAnyLicenseFromLicenseService(new int[] { id });
                if (usedId == id)
                    return true;
            }

            //Check if we need to read again
            if (Math.Abs((DateTime.UtcNow - lastRead).TotalSeconds) > 120)
                ReadContents();

            return IsOptionAvailable(id);
        }

        public void ReleaseLicense(License lic)
        {
            //Nothing to do since the licenses acquired from the license service can't be released
        }

        private bool IsOptionAvailable(int id)
        {
            if (options == null)
                return false;

            int byteNr = id;
            int bitMask = 0xFF;
            if (id >= 1000)
            {
                byteNr = id / 1000;
                bitMask = 1 << (id % 1000);
            }

            return ((byteNr >= 0) && (byteNr < options.Length) && ((options[byteNr] & bitMask) != 0));
        }

        public static string GenerateTransferString(License lic)
        {
            if (!(lic.SourceInfo is int byteNr))
                return null;

            return $"License\tM\t{byteNr}\0";
        }
    }
}
