using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeMeter;
using iba.Logging;
using iba.Utility;
using Newtonsoft.Json;

namespace iba.Licensing
{
    class WibuInfoCollector
    {
        public WibuInfoCollector()
        {
            api = new Api();
        }

        Api api;

        /// <summary>
        /// Collect information about specific Wibu container.
        /// </summary>
        /// <param name="contents">The identity of the Wibu container</param>
        /// <param name="destDir">Directory where the info file should be written to.</param>
        /// <returns>The file name of the file that contains the information.</returns>
        public string CollectInfo(LicenseContents contents, string destDir)
        {
            string zipFileName = Path.Combine(destDir, "wibu.zip");
            using (ZipFileCreator zipFile = new ZipFileCreator(zipFileName))
            {
                //Retrieve Wibu license information
                WibuLicenseInformation licInfo = CreateWibuLicenseInformation(contents);
                string containerName = licInfo.Containers != null ? licInfo.Containers[0].ContainerId : "WIBU";
                string jsonFileName = Path.Combine(destDir, containerName + ".json");
                if (File.Exists(jsonFileName))
                    File.Delete(jsonFileName);

                //Create Wibu license information json file
                try
                {
                    var jsonSettings = new JsonSerializerSettings()
                    {
                        NullValueHandling = NullValueHandling.Ignore,
                        DefaultValueHandling = DefaultValueHandling.Ignore
                    };
                    string jsonString = JsonConvert.SerializeObject(licInfo, Formatting.Indented, jsonSettings);
                    File.WriteAllText(jsonFileName, jsonString);

                    zipFile.AddFile(jsonFileName);
                    TryDelete(jsonFileName);
                }
                catch (Exception ex)
                {
                    ibaLogger.DebugFormat("Failed to save WIBU container info to {0}: {1}", jsonFileName, ex);
                }

                //Create receipt file
                if (licInfo.Containers != null)
                {
                    string receiptFileName = Path.Combine(destDir, containerName + ".WibuCmRar");
                    if (CreateReceiptFile(contents, receiptFileName))
                    {
                        zipFile.AddFile(receiptFileName);
                        TryDelete(receiptFileName);
                    }
                }

                //Export CodeMeter registry settings
                using (var baseKey = Microsoft.Win32.RegistryKey.OpenBaseKey(Microsoft.Win32.RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? Microsoft.Win32.RegistryView.Registry64 : Microsoft.Win32.RegistryView.Registry32))
                using (var codeMeterKey = baseKey.OpenSubKey(@"SOFTWARE\WIBU-SYSTEMS\CodeMeter\Server"))
                { 
                    if (codeMeterKey != null)
                    {
                        string codeMeterSettingsFile = Path.Combine(destDir, "CodeMeter.Server.reg");
                        if (RegistryExporter.ExportRegistry(codeMeterKey, codeMeterSettingsFile, true))
                        {
                            zipFile.AddFile(codeMeterSettingsFile);
                            TryDelete(codeMeterSettingsFile);
                        }
                    }
                }

                //Export CodeMeter log files
                string cmLogsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), @"CodeMeter\Logs");
                if (Directory.Exists(cmLogsPath))
                {
                    foreach (var file in Directory.EnumerateFiles(cmLogsPath, "CodeMeter*.log", SearchOption.TopDirectoryOnly))
                        zipFile.AddFile(file, $"Logs\\{Path.GetFileName(file)}");
                }
            }

            return zipFileName;
        }

        private void TryDelete(string file)
        {
            try
            {
                File.Delete(file);
            }
            catch(Exception)
            { }
        }

        private WibuLicenseInformation CreateWibuLicenseInformation(LicenseContents contents)
        {
            WibuLicenseInformation licInfo = new();
            licInfo.Host = Environment.MachineName;

            Version ver = api.CmGetVersion(null);
            if (ver == null)
            {
                licInfo.CodeMeterVersion = "Not installed";
                return licInfo;
            }
            else
                licInfo.CodeMeterVersion = ver.ToString();

            if (String.IsNullOrEmpty(contents?.ContainerId))
                return licInfo;

            if (!WibuDongle.SplitContainerId(contents.ContainerId, out ushort boxMask, out uint boxSerial, out string location))
                return licInfo;

            CmAccess2 access = new CmAccess2()
            {
                BoxMask = boxMask,
                SerialNumber = boxSerial,
                Servername = location
            };

            using (var hcmseContainer = api.CmAccess2(CmAccessOption.LocalLan, access))
            {
                WibuContainer container = new WibuContainer();
                container.ContainerId = $"{boxMask}-{boxSerial}";
                container.ContainerLocation = location;
                container.ContainerType = contents.ContainerType;

                CmBoxTime boxTime = api.CmGetInfo(hcmseContainer, CmGetInfoOption.BoxTime) as CmBoxTime;
                if(boxTime != null)
                {
                    container.PCTime = boxTime.SystemTime.GetDateTime();
                    container.ContainerTime = boxTime.BoxTime.GetDateTime();
                    container.CertifiedTime = boxTime.BoxTime.GetDateTime();
                }
                else
                {
                    container.PCTime = DateTime.UtcNow;
                    container.ContainerTime = DateTime.MinValue;
                    container.CertifiedTime = DateTime.MinValue;
                }

                licInfo.Containers = new List<WibuContainer>();
                licInfo.Containers.Add(container);               

                // Get entries with iba firm code in this container
                CmBoxInfo boxInfo = new CmBoxInfo();
                boxInfo.BoxMask = boxMask;
                boxInfo.SerialNumber = boxSerial;
                CmBoxEntry2[] entries = api.CmGetBoxContents2(hcmseContainer, CmGetBoxContentsOption.FirmItem | CmGetBoxContentsOption.Box, WibuDongle.IbaFirmCode, boxInfo);
                if (entries != null)
                    container.Entries = ProcessEntries(entries, access);
            }

            return licInfo;
        }

        private List<WibuEntry> ProcessEntries(CmBoxEntry2[] entries, CmAccess2 parentAccess)
        {
            List<WibuEntry> wibuEntries = new List<WibuEntry>(entries.Length);

            foreach (var entry in entries)
            {
                WibuEntry wibuEntry = new WibuEntry();

                wibuEntry.ProductCode = entry.ProductCode;

                if ((entry.SetEffectivePios & CmBoxEntry2.PioMaintenancePeriod) == CmBoxEntry2.PioMaintenancePeriod)
                    wibuEntry.MaintenancePeriodEnd = entry.MaintenancePeriodEnd.GetDateTime();

                if ((entry.SetEffectivePios & CmBoxEntry2.PioExpirationTime) == CmBoxEntry2.PioExpirationTime)
                    wibuEntry.ExpirationTime = entry.ExpirationTime.GetDateTime();

                if ((entry.SetEffectivePios & CmBoxEntry2.PioUnitCounter) == CmBoxEntry2.PioUnitCounter)
                    wibuEntry.UnitCounter = (int)entry.UnitCounter;

                if ((entry.SetEffectivePios & CmBoxEntry2.PioLicenseQuantity) == CmBoxEntry2.PioLicenseQuantity)
                    wibuEntry.LicenseQuantity = (int)entry.LicenseQuantity;

                if ((entry.SetEffectivePios & CmBoxEntry2.PioFeatureMap) == CmBoxEntry2.PioFeatureMap)
                    wibuEntry.FeatureMap = entry.FeatureMap;

                bool bHasText = (entry.SetEffectivePios & CmBoxEntry2.PioText) == CmBoxEntry2.PioText;
                bool bHasProtectedData = (entry.SetEffectivePios & CmBoxEntry2.PioProtectedData) == CmBoxEntry2.PioProtectedData;
                bool bHasExtProtectedData = (entry.SetEffectivePios & CmBoxEntry2.PioExtendedProtectedData) == CmBoxEntry2.PioExtendedProtectedData;

                if (bHasText || bHasProtectedData || bHasExtProtectedData)
                {
                    CmAccess2 access = new CmAccess2()
                    {
                        Ctrl = CmAccess.Option.NoUserLimit | CmAccess.Option.Force,
                        FirmCode = WibuDongle.IbaFirmCode,
                        ProductCode = entry.ProductCode,
                        BoxMask = parentAccess.BoxMask,
                        SerialNumber = parentAccess.SerialNumber,
                        Servername = parentAccess.Servername
                    };

                    using (var hcmse = api.CmAccess2(WibuDongle.AccessOption, access))
                    {
                        if (hcmse != null)
                        {
                            if (bHasText)
                            {
                                string rawText = api.CmGetInfo(hcmse, CmGetInfoOption.PiText) as string;
                                if (rawText != null)
                                {
                                    //For some reason the runtime seems to include garbage in front of the actual text
                                    int lastZero = rawText.LastIndexOf('\0');
                                    if (lastZero >= 0)
                                        rawText = rawText.Substring(lastZero + 1);

                                    wibuEntry.Text = rawText;
                                }
                            }

                            if (bHasProtectedData || bHasExtProtectedData)
                            {
                                var entryDataList = api.CmGetInfo(hcmse, CmGetInfoOption.EntryData) as CmEntryData[];
                                if (entryDataList != null)
                                {
                                    foreach (var entryData in entryDataList)
                                    {
                                        GlobalEntryOption option = CmConvert.ToGlobalEntryOption(entryData.Ctrl);
                                        if (!(option == GlobalEntryOption.ProtectedData || option == GlobalEntryOption.ExtendedProtectedData))
                                            continue;

                                        byte[] bytes = new byte[entryData.DataLen];
                                        Array.Copy(entryData.Data, bytes, entryData.DataLen);
                                        string strHex = BitConverter.ToString(bytes).Replace("-", "");

                                        if (option == GlobalEntryOption.ProtectedData)
                                            wibuEntry.ProtectedData = strHex;
                                        else if (option == GlobalEntryOption.ExtendedProtectedData)
                                        {
                                            if (wibuEntry.ExtendedProtectedData == null)
                                                wibuEntry.ExtendedProtectedData = new List<string>();

                                            wibuEntry.ExtendedProtectedData.Add(strHex);

                                            // Decode coli
                                            if ((entryData.Ctrl & WibuDongle.Coli) == WibuDongle.Coli)
                                                wibuEntry.Coli = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
                                        }
                                    }
                                }
                            }

                            api.CmRelease(hcmse);
                        }
                    }
                }

                wibuEntries.Add(wibuEntry);
            }

            return wibuEntries;
        }

        [System.Reflection.Obfuscation]
        class WibuLicenseInformation
        {
            public string Host { get; set; }
            public string CodeMeterVersion { get; set; }

            public List<WibuContainer> Containers { get; set; }
        }

        [System.Reflection.Obfuscation]
        class WibuContainer
        {
            public string ContainerId { get; set; }
            public string ContainerType { get; set; }
            public string ContainerLocation { get; set; }
            public DateTime PCTime { get; set; }
            public DateTime ContainerTime { get; set; }
            public DateTime CertifiedTime { get; set; }

            public List<WibuEntry> Entries { get; set; }
        }

        [System.Reflection.Obfuscation]
        class WibuEntry
        {
            public uint ProductCode { get; set; }
            public string Text { get; set; }
            public uint FeatureMap { get; set; }
            public DateTime ExpirationTime { get; set; }
            public DateTime MaintenancePeriodEnd { get; set; }
            [DefaultValue(-1)]
            public int UnitCounter { get; set; }
            [DefaultValue(-1)]
            public int LicenseQuantity { get; set; }
            public string ProtectedData { get; set; }
            public List<string> ExtendedProtectedData { get; set; }
            public string Coli { get; set; } //Customer owned license information

            public WibuEntry()
            {
                UnitCounter = -1;
                LicenseQuantity = -1;
            }
        }

        private bool CreateReceiptFile(LicenseContents contents, string fileName)
        {
            try
            {
                if (!WibuDongle.SplitContainerId(contents.ContainerId, out ushort boxMask, out uint boxSerial, out string location))
                    return false;

                CmAccess2 access = new CmAccess2();
                access.BoxMask = boxMask;
                access.SerialNumber = boxSerial;
                access.Servername = location;

                using (var hcmseContainer = api.CmAccess2(WibuDongle.AccessOption, access))
                {
                    if (hcmseContainer == null)
                        return false;

                    CmLtRequest request = new CmLtRequest()
                    {
                        FirmCode = WibuDongle.IbaFirmCode,
                        Ctrl = CmLtRequest.Option.None
                    };

                    if (!api.CmLtCreateReceipt(hcmseContainer, CmLtFlags.None, request, out byte[] bytesReceipt))
                        return false;

                    if (File.Exists(fileName))
                        File.Delete(fileName);

                    File.WriteAllBytes(fileName, bytesReceipt);

                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

    }
}
