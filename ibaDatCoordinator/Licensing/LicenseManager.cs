using iba.Utility;
using System;
using System.Collections.Generic;
using System.IO;
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

        public bool TransferLicense(License lic, IbaAnalyzer.IbaAnalyzer analyzer)
        {
            try
            {
                if (analyzer == null)
                    return false;

                if (!lic.LicenseOk)
                    return false;

                LicenseOptionInfo info = LicenseOptionInfo.GetOptionInfo(lic.LicenseId);
                if (info == null || info.AnalyzerLicenseTransferId < 0)
                    return false;

                //Pack license information into a text that allows ibaAnalyzer to check the license
                string licenseText = null;
                if (lic.IsWibu)
                    licenseText = WibuDongle.GenerateTransferString(lic);
                else if (lic.IsMarx)
                    licenseText = MarxDongle.GenerateTransferString(lic);

                if (String.IsNullOrEmpty(licenseText))
                    return false;

                //Encode text
                byte[] licenseTextData = Encoding.Unicode.GetBytes(licenseText);

                //Take current UTC time without the subsecond part
                DateTime currentTime = DateTime.UtcNow;
                currentTime = new DateTime((currentTime.Ticks / TimeSpan.TicksPerSecond) * TimeSpan.TicksPerSecond, DateTimeKind.Utc);
                long fileTime = currentTime.ToFileTimeUtc();

                Random rand = new Random();
                byte[] randData = new byte[8];
                rand.NextBytes(randData);

                //Create encryption key consisting of the current UTC time and 8 random bytes
                uint[] key = new uint[4];
                key[0] = (uint)(fileTime & 0xFFFFFFFF);
                key[1] = (uint)((fileTime >> 32) & 0xFFFFFFFF);
                key[2] = BitConverter.ToUInt32(randData, 0);
                key[3] = BitConverter.ToUInt32(randData, 4);

                byte[] encryptedData = Btea.Encrypt(licenseTextData, key);

                //The license data consists of the encrypted string followed by the 8 last (random) bytes of the key and a byte containing the seconds of the current time
                byte[] licenseData = new byte[encryptedData.Length + 12];
                rand.NextBytes(licenseData);
                Buffer.BlockCopy(encryptedData, 0, licenseData, 0, encryptedData.Length);
                Buffer.BlockCopy(key, 8, licenseData, encryptedData.Length, 8);
                licenseData[encryptedData.Length + 8] = (byte)currentTime.Second;

                return analyzer.TransferLicense(info.AnalyzerLicenseTransferId, licenseData);
            }
            catch(Exception)
            {
                return false;
            }
        }

        public void RevokeLicense(License lic, IbaAnalyzer.IbaAnalyzer analyzer)
        {
            try
            {
                if (analyzer == null)
                    return;

                LicenseOptionInfo info = LicenseOptionInfo.GetOptionInfo(lic.LicenseId);
                if (info == null || info.AnalyzerLicenseTransferId < 0)
                    return;

                analyzer.RevokeLicense(info.AnalyzerLicenseTransferId);
            }
            catch (Exception)
            {
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

        public void AddInfoToSupportFile(ICSharpCode.SharpZipLib.Zip.ZipFile zip, string tempDir)
        {
            //MARX info
            LicenseContents marxContents = marxDongle?.Contents;
            if(marxContents != null && marxContents.ContainerFound)
            {
                string dongleFile = Path.Combine(tempDir, !String.IsNullOrEmpty(marxContents.Identifier) ? marxContents.Identifier + ".vwr" : "dongle.vwr");
                if (SystemInfoCollector.ExportDongleInfo(dongleFile))
                    SupportFileGenerator.AddFile(zip, dongleFile, "License\\" + Path.GetFileName(dongleFile), delete: true);
            }

            //WIBU info
            LicenseContents wibuContents = wibuDongle?.Contents;
            if(wibuContents != null && wibuContents.ContainerFound)
            {
                WibuInfoCollector wibuColl = new WibuInfoCollector();
                string dongleFile = wibuColl.CollectInfo(wibuContents, tempDir);
                if(dongleFile != null)
                    SupportFileGenerator.AddFile(zip, dongleFile, "License\\" + Path.GetFileName(dongleFile), delete: true);
            }
        }
    }
}
