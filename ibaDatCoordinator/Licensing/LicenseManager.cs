using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Licensing
{
    class LicenseManager
    {
        public LicenseManager()
        {
            licenseLock = new object();
        }

        public void Initialize()
        {
            lock (licenseLock)
            {
                wibuDongle = new WibuDongle();
                wibuDongle.ReadContents();

                marxDongle = new MarxDongle();
                marxDongle.ReadContents();
            }
        }

        public void Uninitialize()
        {
            lock (licenseLock)
            {
                wibuDongle.Dispose();
            }
        }

        readonly object licenseLock;

        WibuDongle wibuDongle;
        MarxDongle marxDongle;

        /// <summary>
        /// Acquire a license
        /// </summary>
        /// <param name="licenseId">The id of the license</param>
        /// <returns>A license object that contains information about the acquired license. If LicenseOk is false then no license was obtained.</returns>
        public License AcquireLicense(int licenseId)
        {
            lock (licenseLock)
            {
                var option = LicenseOptionInfo.GetOptionInfo(licenseId);
                if(option == null)
                {
                    //Assume this is directly a Wibu id. This can come from an unknown plugin.
                    option = new LicenseOptionInfo(licenseId, $"Unknown license {licenseId}", LicenseOptionType.Counter);
                }

                if (option.MarxIds != null)
                {
                    //Let's try with Marx
                    License lic = marxDongle.AcquireLicense(option);
                    if (lic.LicenseOk)
                        return lic;
                }

                //Let's try with Wibu
                return wibuDongle.AcquireLicense(option);
            }
        }

        /// <summary>
        /// Check if license is still valid. 
        /// </summary>
        /// <param name="lic">The license to check</param>
        /// <returns>True if license is still valid.</returns>
        public bool CheckLicense(License lic)
        {
            DateTime now = DateTime.UtcNow;
            double secondsSinceLastCheck = Math.Abs((now - lic.LastCheck).TotalSeconds);

            if (!lic.LicenseOk)
            {
                if (secondsSinceLastCheck > 10)
                    TryAcquireLicense(lic);

                return lic.LicenseOk;
            }

            //Only check when it has been longer than 60s since the last check
            if (secondsSinceLastCheck < 60)
                return true;

            lock (licenseLock)
            {
                lic.LastCheck = now;
                lic.LicenseOk = lic.IsWibu ? wibuDongle.CheckLicense(lic) : marxDongle.CheckLicense(lic);

                if(!lic.LicenseOk)
                    TryAcquireLicense(lic);

                return lic.LicenseOk;
            }
        }

        /// <summary>
        /// Release a previously acquired license
        /// </summary>
        /// <param name="lic">The license that needs to be released</param>
        public void ReleaseLicense(License lic)
        {
            if (!lic.LicenseOk)
                return;

            lock (licenseLock)
            {
                if (lic.IsWibu)
                    wibuDongle.ReleaseLicense(lic);
                else
                    marxDongle.ReleaseLicense(lic);

                lic.LicenseOk = false;
            }
        }

        public LicenseContents GetLicenseContents()
        {
            lock (licenseLock)
            {
                if (wibuDongle.Contents.ContainerFound)
                    return wibuDongle.Contents;

                if (marxDongle.Contents.ContainerFound)
                    return marxDongle.Contents;

                return new LicenseContents();
            }
        }

        private void TryAcquireLicense(License lic)
        {
            License newLic = AcquireLicense(lic.LicenseId);
            lic.Update(newLic);
        }
    }
}
