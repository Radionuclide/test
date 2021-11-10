using CodeMeter;
using iba.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Licensing
{
    class WibuDongle
    {
        public WibuDongle()
        {
            contents = new LicenseContents();
            lastRead = DateTime.MinValue;

            hcmEntries = new List<HCMSysEntry>();

            if (api == null)
            {
                api = new Api();

                //Log local CodeMeter runtime version
                Version ver = api.CmGetVersion(null);
                if (ver != null)
                    ibaLogger.DebugFormat("CodeMeter Runtime v{0}", ver.ToString(4));

                lastUsageCheckTime = DateTime.UtcNow;
            }
        }

        public void Dispose()
        {
            if (hcmBase != null)
            {
                api.CmRelease(hcmBase);
                hcmBase = null;
            }

            foreach (var hcm in hcmEntries)
                api.CmRelease(hcm);
            hcmEntries.Clear();
        }

        LicenseContents contents;
        DateTime lastRead;

        public LicenseContents Contents => contents;

        private uint firmCode;
        private ushort boxMask;
        private uint boxSerial;
        private HCMSysEntry hcmBase; //Entry of base ibaDatCoordinator product code 14000
        private List<HCMSysEntry> hcmEntries; //All entries related to different product codes

        private static Api api;
        private static DateTime lastUsageCheckTime;
        private static CmCrypt2 cmCrypt;
        private static Byte[] cmCryptBuffer;

        internal const int IbaFirmCode = 6000506;
        const int IbaDevFirmCode = 6000010;
        internal const CmAccessOption AccessOption = CmAccessOption.LocalLan;

        const int IbaIdentPC = 1000;
        const int IbaDatCoPC = 14000;

        const UInt32 DemoLicense = 1;
        const UInt32 FullLicense = 2;

        internal const int Coli = 16384000; //Customer owned license information

        static WibuDongle()
        {
            cmCrypt = new CmCrypt2();
            cmCrypt.BaseCrypt = new CmBaseCrypt2();
            cmCrypt.BaseCrypt.Ctrl = (CmBaseCrypt.Option)GlobalEncryptionOption.FirmKey;
            cmCrypt.BaseCrypt.EncryptionCodeOptions = CmBaseCrypt.EncCodeOption.UnitCounterIgnore;
            cmCrypt.InitKey = new byte[16];

            cmCryptBuffer = new byte[16];
        }

        public void ReadContents()
        {
            //The previous data should have been freed
            System.Diagnostics.Debug.Assert(hcmBase == null);

            LicenseContents contents = new LicenseContents();

            try
            {
                // Get version
                Version ver = api.CmGetVersion(null);
                if (ver == null || ver < new Version(7, 10, 4196, 501))
                {
                    //Minimum CM version is 7.10a for security reasons
                    ibaLogger.Log(Level.Warning, "ibaDatCoordinator requires CodeMeter Runtime v7.10a or higher when using a WIBU license");
                    return;
                }

                //Get base license
                hcmBase = GetBaseLicense();
                if (hcmBase == null)
                    return; // No license

                //Retrieve info about container
                if (!ReadContainerInfo(contents))
                {
                    //If we can't get information about the container then consider this an invalid license
                    api.CmRelease(hcmBase);
                    hcmBase = null;
                    return;
                }

                //Check all time limits of the main license
                ReadTimeLimits(contents);

                //Retrieve customer information
                ReadCustomer(contents);
            }
            finally
            {
                this.lastRead = DateTime.UtcNow;
                this.contents = contents;
            }
        }

        public License AcquireLicense(LicenseOptionInfo option)
        {
            License lic = new License(option.Id);

            //Check if there is a container and if it is still valid
            if (!contents.ContainerFound || !CheckHandle(hcmBase))
            {
                //Clear current data
                Dispose();

                ReadContents();
                if (!contents.ContainerFound)
                    return lic;
            }

            //Try to get product code in container
            if(GetFeature(option.Id, option.OptionType == LicenseOptionType.Counter ? CmAccess.Option.UserLimit : CmAccess.Option.Stationshare, out HCMSysEntry entry))
            {
                lic.ContainerType = contents.ContainerType;
                lic.ContainerId = contents.ContainerId;
                lic.LicenseIdentifier = contents.Identifier;
                lic.LicenseOk = true;
                lic.SourceInfo = entry;
                return lic;
            }

            //Try to get optional ones. We use station share here because they are ibaAnalyzer ones that can be reused.
            if(option.OptionalWibuIds != null)
            {
                foreach(int id in option.OptionalWibuIds)
                {
                    if(GetFeature(id, CmAccess.Option.Stationshare, out entry))
                    {
                        lic.ContainerType = contents.ContainerType;
                        lic.ContainerId = contents.ContainerId;
                        lic.LicenseIdentifier = contents.Identifier;
                        lic.LicenseOk = true;
                        lic.SourceInfo = entry;
                        return lic;
                    }
                }
            }

            return lic;
        }

        public bool CheckLicense(License lic)
        {
            HCMSysEntry entry = lic.SourceInfo as HCMSysEntry;
            if (!hcmEntries.Contains(entry))
                return false;

            if(!CheckHandle(entry) || IsDongleDisabled(entry))
            {
                lic.SourceInfo = null;
                hcmEntries.Remove(entry);
                api.CmRelease(entry);
                return false;
            }

            return true;
        }

        public void ReleaseLicense(License lic)
        {
            HCMSysEntry entry = lic.SourceInfo as HCMSysEntry;
            lic.SourceInfo = null;

            if (!hcmEntries.Contains(entry))
                return;

            hcmEntries.Remove(entry);
            api.CmRelease(entry);
        }

        public static bool SplitContainerId(string id, out ushort boxMask, out uint boxSerial, out string location)
        {
            boxMask = 0;
            boxSerial = 0;
            location = "";

            if (String.IsNullOrEmpty(id))
                return false;

            int hyphenIndex = id.IndexOf('-');
            if (hyphenIndex <= 0)
                return false;

            if (!UInt16.TryParse(id.Substring(0, hyphenIndex), out boxMask))
                return false;

            int spaceIndex = id.IndexOf(" (", hyphenIndex);
            if (spaceIndex <= 0)
                return false;

            if (!UInt32.TryParse(id.Substring(hyphenIndex + 1, spaceIndex - hyphenIndex - 1), out boxSerial))
                return false;

            location = id.Substring(spaceIndex + 2, id.Length - spaceIndex - 3);
            return true;
        }

        private HCMSysEntry GetBaseLicense()
        {
            firmCode = IbaFirmCode;

            CmAccess2 cmBase = new CmAccess2();
            cmBase.Ctrl = CmAccess.Option.UserLimit; //Subtract one from license quantity
            cmBase.FirmCode = IbaFirmCode; //Try with official firm code
            cmBase.ProductCode = IbaDatCoPC;
            HCMSysEntry baseLic = api.CmAccess2(AccessOption, cmBase);

            //Check that dongle isn't disabled
            if (baseLic != null && (IsDongleDisabled(baseLic) || !CheckHandle(baseLic)))
            {
                api.CmRelease(baseLic);
                baseLic = null;
            }

#if DEBUG
            if (baseLic == null)
            {
                //Try with dev firm code
                cmBase.FirmCode = IbaDevFirmCode;
                baseLic = api.CmAccess2(AccessOption, cmBase);

                //Check that dongle isn't disabled
                if (baseLic != null && (IsDongleDisabled(baseLic) || !CheckHandle(baseLic)))
                {
                    api.CmRelease(baseLic);
                    baseLic = null;
                }

                if (baseLic != null)
                    firmCode = IbaDevFirmCode;
            }
#endif

            return baseLic;
        }

        private bool IsDongleDisabled(HCMSysEntry hcm)
        {
            CmBoxStatus boxStatus = api.CmGetInfo(hcm, CmGetInfoOption.BoxStatus) as CmBoxStatus;
            if (boxStatus == null)
                return true;

            CmBoxStatus.Flags status = boxStatus.Status;
            if (status.HasFlag(CmBoxStatus.Flags.IsCmAct))
                return !status.HasFlag(CmBoxStatus.Flags.Active); //A soft license is disabled when it isn't activated
            else
                return status.HasFlag(CmBoxStatus.Flags.Disabled); //Normal dongle can be disabled
        }

        private bool CheckHandle(HCMSysEntry hcmSysEntry)
        {
            return api.CmCrypt2(hcmSysEntry, CmCryptOption.DirectEncryption, cmCrypt, cmCryptBuffer);
        }

        private bool ReadContainerInfo(LicenseContents contents)
        {
            CmBoxInfo boxInfo = api.CmGetInfo(hcmBase, CmGetInfoOption.BoxInfo) as CmBoxInfo;
            if (boxInfo == null)
                return false;

            boxMask = boxInfo.BoxMask;
            boxSerial = boxInfo.SerialNumber;

            string location = null;
            CmCommunication commInfo = api.CmGetInfo(hcmBase, CmGetInfoOption.Communication) as CmCommunication;
            if (commInfo != null)
            {
                location = commInfo.ComputerName;
                if (String.IsNullOrEmpty(location) && commInfo.IpV4Address != null)
                    location = commInfo.IpV4Address;
            }

            if (String.IsNullOrEmpty(location))
                location = Environment.MachineName;

            contents.ContainerId = $"{boxMask}-{boxSerial} ({location})";

            if (boxMask == 3)
                contents.ContainerType = $"WIBU CmStick v{boxInfo.MajorVersion}.{boxInfo.MinorVersion}";
            else if (boxMask == 130)
                contents.ContainerType = $"WIBU CmActLicense v{boxInfo.MajorVersion}.{boxInfo.MinorVersion}";
            else
                contents.ContainerType = $"WIBU Container type {boxMask}";

            return true;
        }

        private void ReadTimeLimits(LicenseContents contents)
        {
            CmBoxEntry2 entryInfo2 = api.CmGetInfo(hcmBase, CmGetInfoOption.EntryInfo2) as CmBoxEntry2;
            if (entryInfo2 == null)
            {
                contents.DemoDays = 0;
                return;
            }

            //Demo in days since 1/1/2016
            if ((entryInfo2.SetEffectivePios & CmBoxEntry2.PioExpirationTime) != 0)
            {
                contents.DemoDays = ConvertExpirationTimeToDays(entryInfo2.ExpirationTime);
                //if (contents.DemoDays <= LicenseHelper.DateTimeToDays(DateTime.Today))
                //    bTimeValid = false;
            }
            else
                contents.DemoDays = 0;

        }

        private int ConvertExpirationTimeToDays(CmTime cmTime)
        {
            DateTime expTime = DateTime.SpecifyKind(cmTime.GetDateTime(), DateTimeKind.Utc).ToLocalTime();

            //Add 1 second because they normally specify valid until 23:59:59.
            if (expTime.Second == 59)
                expTime = expTime + TimeSpan.FromSeconds(1);

            return LicenseHelper.DateTimeToDays(expTime.Date);
        }

        private void ReadCustomer(LicenseContents contents)
        {
            CmAccess2 cmAccess = new CmAccess2();
            cmAccess.Ctrl = CmAccess.Option.NoUserLimit | CmAccess.Option.Force;
            cmAccess.FirmCode = firmCode;
            cmAccess.ProductCode = IbaIdentPC;
            cmAccess.BoxMask = boxMask;
            cmAccess.SerialNumber = boxSerial;

            HCMSysEntry hcmIbaIdent = api.CmAccess2(AccessOption, cmAccess);
            if (hcmIbaIdent == null)
                return;

            string identifier = api.CmGetInfo(hcmIbaIdent, CmGetInfoOption.PiText) as string;
            if (!String.IsNullOrEmpty(identifier))
            {
                //Sometimes the runtime adds some garbage to the identifier. We have to remove this.
                int index = identifier.IndexOf("iba License Identifier");
                if (index >= 0)
                    contents.Identifier = identifier.Substring(index + "iba License Identifier".Length).Trim();
            }

            CmEntryData[] entryDataList = api.CmGetInfo(hcmIbaIdent, CmGetInfoOption.EntryData) as CmEntryData[];
            if (entryDataList != null)
            {
                foreach (CmEntryData entry in entryDataList)
                {
                    if ((CmConvert.ToGlobalEntryOption(entry.Ctrl) == GlobalEntryOption.ExtendedProtectedData) && ((entry.Ctrl & Coli) == Coli))
                    {
                        contents.Customer = System.Text.Encoding.UTF8.GetString(entry.Data, 0, (int)entry.DataLen);
                        break;
                    }
                }
            }

            api.CmRelease(hcmIbaIdent);
        }

        private bool GetFeature(int productCode, CmAccess.Option accessMode, out HCMSysEntry hcmEntry)
        {
            hcmEntry = null;

            //Acquire license
            CmAccess2 cm2 = new CmAccess2();
            cm2.BoxMask = boxMask;
            cm2.SerialNumber = boxSerial;
            cm2.Ctrl = accessMode;
            cm2.FirmCode = firmCode;
            cm2.ProductCode = (uint)productCode;
            cm2.LicenseQuantity = 1;
            HCMSysEntry hcmLic = api.CmAccess2(AccessOption, cm2);
            if (hcmLic == null)
                return false;

            //Check if handle is valid because CmAccess2 only does a preliminary check that doesn't go inside the container.
            if (!CheckHandle(hcmLic) || IsDongleDisabled(hcmLic))
            {
                api.CmRelease(hcmLic);
                return false;
            }

            //We got a valid handle
            hcmEntries.Add(hcmLic);

            hcmEntry = hcmLic;
            return true;
        }
    }
}
