using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Management;
using System.Globalization;
using System.Diagnostics;

namespace iba.Utility
{
    public class SystemInfoCollector
    {
        public static void SaveSystemInfo(string prefix, string postfix, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                writer.Write(prefix);

                WriteOSVersion(writer);
                writer.WriteLine("CLR version: " + Environment.Version.ToString());
                writer.WriteLine("Installed .NET version: " + GetInstalledDotNetVersion());

                DateTime currentUtcTime = DateTime.UtcNow;
                DateTime currentTime = currentUtcTime.ToLocalTime();
                TimeSpan utcOffset = currentTime - currentUtcTime;
                writer.WriteLine(String.Format("Current time: {0} (UTC{1}:{2})", currentTime.ToString("dd/MM/yyyy HH:mm:ss"), 
                    utcOffset.Hours.ToString("+00;-00"), utcOffset.Minutes.ToString("00")));
                
                writer.WriteLine("Current user: " + Environment.UserName);
                writer.WriteLine("PC name: " + Environment.MachineName);

                writer.WriteLine("Nr CPUs: " + Environment.ProcessorCount);
                RegistryKey rootKey = Registry.LocalMachine.OpenSubKey(@"Hardware\Description\System\CentralProcessor", false);
                if (rootKey != null)
                {
                    for (int i = 0; i < Environment.ProcessorCount; i++)
                    {
                        string processorName = null;
                        RegistryKey cpuKey = rootKey.OpenSubKey(i.ToString());
                        if (cpuKey != null)
                        {
                            processorName = cpuKey.GetValue("ProcessorNameString") as string;
                            if (processorName != null)
                                processorName = processorName.Trim();
                        }
                        if (processorName == null)
                            processorName = "?";
                        writer.WriteLine("CPU {0}: {1}", i, processorName);
                    }
                }

                try
                {
                    writer.WriteLine();

                    //Retrieve memory 
                    ManagementObjectSearcher objMOS = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
                    UInt64 usableMemory = 0;
                    string manufacturer = null;
                    foreach (ManagementObject obj in objMOS.Get())
                    {
                        usableMemory += Convert.ToUInt64(obj.GetPropertyValue("TotalPhysicalMemory"));

                        string newManufacturer = obj.GetPropertyValue("Manufacturer") as string;
                        if (String.IsNullOrEmpty(manufacturer) && !String.IsNullOrEmpty(newManufacturer))
                            manufacturer = newManufacturer;
                    }
                    UInt64 totalMemory = 0;
                    objMOS = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory ");
                    foreach (ManagementObject obj in objMOS.Get())
                        totalMemory += Convert.ToUInt64(obj.GetPropertyValue("Capacity"));

                    writer.WriteLine(String.Format("Total Physical Memory: {0:f2} GB ({1:f2} GB usable)", totalMemory / (1024.0 * 1024.0 * 1024.0), usableMemory / (1024.0 * 1024.0 * 1024.0)));

                    if (!String.IsNullOrEmpty(manufacturer))
                        writer.WriteLine(String.Format("System Manufacturer: {0}", manufacturer));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("WMI query failed with error: {0}", ex);
                }

                try
                {
                    writer.WriteLine();
                    writer.WriteLine("Storage");
                    writer.WriteLine("*******");

                    List<Dictionary<string, string>> results = new List<Dictionary<string, string>>();

                    using (ManagementClass diskDriveClass = new ManagementClass(@"Win32_Diskdrive"))
                    {
                        using (ManagementObjectCollection diskDrives = diskDriveClass.GetInstances())
                        {
                            foreach (ManagementObject diskDrive in diskDrives)
                            {
                                using (ManagementObjectCollection relatedPartitions = diskDrive.GetRelated("Win32_DiskPartition"))
                                {
                                    foreach (ManagementObject relatedPartition in relatedPartitions)
                                    {
                                        writer.WriteLine(relatedPartition["Name"]);

                                        using (ManagementObjectCollection relatedLogicalDisks = relatedPartition.GetRelated("Win32_LogicalDisk"))
                                        {
                                            foreach (ManagementBaseObject relatedLogicalDisk in relatedLogicalDisks)
                                            {
                                                string logicalDisk = relatedLogicalDisk["Name"] + "\\";
                                                ulong total = (ulong)relatedLogicalDisk["Size"] / 1048576L;
                                                ulong free = (ulong)relatedLogicalDisk["FreeSpace"] / 1048576L;

                                                writer.WriteLine(logicalDisk + " " + relatedLogicalDisk["FileSystem"] + " " + total + " MB (" + free + " MB free)");
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("WMI query failed with error: {0}", ex);
                }

                //Retrieve motherboard settings
                MotherboardInfo mbInfo = GetMotherboardInfo();
                if(mbInfo != null)
                {
                    writer.WriteLine();
                    writer.WriteLine("Motherboard");
                    writer.WriteLine("***********");
                    writer.WriteLine("Manufacturer: " + mbInfo.Manufacturer);
                    writer.WriteLine("Model:        " + mbInfo.Model);
                    writer.WriteLine("Name:         " + mbInfo.Name);
                    writer.WriteLine("PartNumber:   " + mbInfo.PartNumber);
                    writer.WriteLine("Product:      " + mbInfo.Product);
                    writer.WriteLine("SerialNumber: " + mbInfo.SerialNumber);
                    writer.WriteLine("SKU:          " + mbInfo.SKU);
                    writer.WriteLine("Version:      " + mbInfo.Version);
                }

                try
                {
                    writer.WriteLine();
                    writer.WriteLine("Network");
                    writer.WriteLine("*******");

                    RunProcessWithLogging("netsh", "int show int", 10000, writer);
                    RunProcessWithLogging("netsh", "int ip show config", 20000, writer);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Network info gathering failed with error: {0}", ex);
                }

                LogIbaProductsVersionInfo(writer);

                if (postfix != null)
                    writer.Write(postfix);
            }
        }

        public static MotherboardInfo GetMotherboardInfo()
        {
            try
            {
                //Retrieve motherboard settings
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_BaseBoard");
                foreach (ManagementObject obj in objMOS.Get())
                {
                    MotherboardInfo mbInfo = new MotherboardInfo();

                    mbInfo.Manufacturer = Convert.ToString(obj.GetPropertyValue("Manufacturer"));
                    mbInfo.Model = Convert.ToString(obj.GetPropertyValue("Model"));
                    mbInfo.Name = Convert.ToString(obj.GetPropertyValue("Name"));
                    mbInfo.PartNumber = Convert.ToString(obj.GetPropertyValue("PartNumber"));
                    mbInfo.Product = Convert.ToString(obj.GetPropertyValue("Product"));
                    mbInfo.SerialNumber = Convert.ToString(obj.GetPropertyValue("SerialNumber"));
                    mbInfo.SKU = Convert.ToString(obj.GetPropertyValue("SKU"));
                    mbInfo.Version = Convert.ToString(obj.GetPropertyValue("Version"));

                    return mbInfo;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("WMI query failed with error: {0}", ex);
            }

            return null;
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll")]
        public static extern int GetSystemDefaultLCID();

        private static bool RunProcessWithLogging(string cmd, string args, int timeoutInMs, StreamWriter writer)
        {
            try
            {
                writer.WriteLine(cmd + " " + args);
                ConsoleWriter = writer;

                ProcessStartInfo si = new ProcessStartInfo(cmd, args);
                si.CreateNoWindow = true;
                si.UseShellExecute = false;
                si.ErrorDialog = false;
                si.RedirectStandardOutput = true;

                //Use default console codepage
                int lcid = GetSystemDefaultLCID();
                CultureInfo ci = CultureInfo.GetCultureInfo(lcid);
                si.StandardOutputEncoding = Encoding.GetEncoding(ci.TextInfo.OEMCodePage);

                Process ins = new Process();
                ins.StartInfo = si;
                ins.OutputDataReceived += new DataReceivedEventHandler(OnConsoleLineReceived);
                ins.Start();
                ins.BeginOutputReadLine();

                bool bFinished = ins.WaitForExit(timeoutInMs);
                ins.OutputDataReceived -= new DataReceivedEventHandler(OnConsoleLineReceived);
                ins.CancelOutputRead();

                int exitCode = ins.ExitCode;
                ins.Close();

                if (exitCode != 0)
                    writer.WriteLine("Exitcode = " + exitCode.ToString());

                return bFinished;
            }
            catch (Exception ex)
            {
                writer.WriteLine(ex.Message);
                return false;
            }
            finally
            {
                ConsoleWriter = null;
            }
        }

        static StreamWriter ConsoleWriter;

        private static void OnConsoleLineReceived(object sender, DataReceivedEventArgs e)
        {
            if ((e.Data != null) && (ConsoleWriter != null))
                ConsoleWriter.WriteLine(e.Data);
        }

        private static void WriteOSVersion(StreamWriter writer)
        {
            try
            {
                writer.WriteLine("OS Version: {0} ({1})", Environment.OSVersion.VersionString, Environment.Is64BitOperatingSystem ? "x64" : "x86");

                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_OperatingSystem");
                foreach (ManagementObject obj in objMOS.Get())
                {
                    writer.WriteLine("OS Name: {0}", Convert.ToString(obj.GetPropertyValue("Caption")));
                    UInt32 lang = Convert.ToUInt32(obj.GetPropertyValue("OSLanguage"));
                    CultureInfo culture = new CultureInfo((int)lang);
                    writer.WriteLine("OS Language: {0}", culture.EnglishName);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("WMI query failed with error: {0}", ex);
            }
        }

        private static string GetInstalledDotNetVersion()
        {
            try
            {
                string version = "? (not found)";
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\v4\Full");
                if (key != null)
                {
                    version = key.GetValue("Version", version).ToString();

                    UInt32 release = Convert.ToUInt32(key.GetValue("Release", 0));
                    if (release != 0)
                    {
                        version += " (" + release.ToString();
                        if (release >= 381029)
                            version += ": 4.6 preview or later";
                        else if (release >= 379893)
                            version += ": 4.5.2 or later";
                        else if (release >= 378675)
                            version += ": 4.5.1 or later";
                        else if (release >= 378389)
                            version += ": 4.5 or later";

                        version += ")";
                    }

                    key.Close();
                }
                return version;
            }
            catch (Exception ex)
            {
                return "? (" + ex.Message + ")";
            }
        }

        public static void GetInstallHistoryFiles(out string installHistoryX86, out string installHistoryX64)
        {
            installHistoryX86 = null;
            installHistoryX64 = null;

            installHistoryX86 = Path.GetDirectoryName(typeof(SystemInfoCollector).Assembly.Location);
            installHistoryX86 = Path.Combine(installHistoryX86, "..\\..\\InstallHistory.txt");
            if (!File.Exists(installHistoryX86))
                installHistoryX86 = null;

            if (Environment.Is64BitOperatingSystem && (installHistoryX86 != null))
            {
                installHistoryX64 = installHistoryX86.Replace(" (x86)", "");
                if ((installHistoryX64 == installHistoryX86) || !File.Exists(installHistoryX64))
                    installHistoryX64 = null;
            }
        }

        private static void LogIbaProductsVersionInfo(StreamWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("iba Products\r\n************");

            RegistryKey pdaKey = OpenRegistryKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ibaPDA", false);
            if (pdaKey != null)
            {
                string ver = pdaKey.GetValue("DisplayVersion") as string;
                if (ver != null)
                {
                    string installDir = pdaKey.GetValue("InstallDir") as string;
                    if (pdaKey.GetValue("Client") != null)
                        writer.WriteLine("ibaPDA client: {0}", ver);
                    if (pdaKey.GetValue("ActiveX") != null)
                        writer.WriteLine("ibaPDA Active-X: {0}", ver);
                    if (pdaKey.GetValue("Server") != null)
                        writer.WriteLine("ibaPDA Server: {0}", ver);
                    if (pdaKey.GetValue("S7AnaProxy") != null)
                    {
                        string proxy = Path.Combine(installDir, "ibaPDA-S7-Xplorer Proxy\\ibaPDA-S7-Xplorer Proxy.exe");
                        System.Diagnostics.FileVersionInfo verInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(proxy);
                        if(verInfo != null)
                            writer.WriteLine("ibaPDA S7-Xplorer Proxy: {0}", verInfo.FileVersion);
                    }
                    if (pdaKey.GetValue("ExcludeNonIbaHW") != null)
                    {
                        if (Convert.ToInt32(pdaKey.GetValue("ExcludeNonIbaHW")) == 1)
                            writer.WriteLine("No iba driver installed for third-party hardware");
                    }
                }
                pdaKey.Close();
            }

            try
            {
                LogIbaFobDNetworkDriverVersion(writer);
            }
            catch(Exception)
            {
            }

            LogIbaProductVersion(writer, "ibaAnalyzer", "ibaAnalyzer", true);
            LogIbaProductVersion(writer, "ibaCapture-HMI", "ibaCapture", false);
            LogIbaProductVersion(writer, "ibaHD Server", "iba Historical Data", false);
            LogIbaProductVersion(writer, "ibaDatCoordinator", "ibaDatCoordinator", false);
            LogIbaProductVersion(writer, "ibaDatManager", "ibaDatManager", false);
            LogIbaProductVersion(writer, "ibaLicenseService V2", "ibaLicenseService-V2", false);
            LogIbaProductVersion(writer, "ibaDongleViewer", "ibaDongleViewer", false);
            LogIbaProductVersion(writer, "ibaUpdate", "ibaUpdate", false);

            RegistryKey camKey = OpenRegistryKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ibaCapture-CAM", false);
            if (camKey != null)
            {
                string ver = camKey.GetValue("DisplayVersion") as string;
                if (ver != null)
                {
                    if (camKey.GetValue("Player") != null)
                        writer.WriteLine("ibaCapture Player: {0}", ver);
                    if (camKey.GetValue("Manager") != null)
                        writer.WriteLine("ibaCapture Manager: {0}", ver);
                    if (camKey.GetValue("Server") != null)
                        writer.WriteLine("ibaCapture Server: {0}", ver);
                    if (camKey.GetValue("Encoder") != null)
                        writer.WriteLine("ibaCapture Encoder: {0}", ver);
                }
                camKey.Close();
            }

            LogIbaProductVersion(writer, "ibaLogic", "ibaLogicV5", false);

            try
            {
                //ibaLogic V4
                RegistryKey ibaLogicV4Key = OpenRegistryKey("Software\\iba\\ibaLogic", false);
                if (ibaLogicV4Key != null)
                {
                    string location = ibaLogicV4Key.GetValue("Path") as string;
                    if ((location != null) && Directory.Exists(location))
                    {
                        location = Path.Combine(location, "ibaLogicServerUi.exe");
                        System.Diagnostics.FileVersionInfo verInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(location);
                        if (verInfo.FileVersion != null)
                            writer.WriteLine("ibaLogic: {0}", verInfo.FileVersion);
                    }
                    ibaLogicV4Key.Close();
                }
            }
            catch (Exception)
            {

            }

            try
            {
                //ibaFiles PRO
                for (int i = 0; i < 2; i++)
                {
                    RegistryKey baseKey = null;
                    if (i == 0)
                        baseKey = Registry.ClassesRoot;
                    else if (System.Environment.Is64BitOperatingSystem)
                        baseKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
                    else
                        break;

                    RegistryKey ibaFilesProKey = baseKey.OpenSubKey("CLSID\\{98AE4E1A-BA50-4325-AF7F-B491CADD4F0A}\\InprocServer32", false);
                    if (ibaFilesProKey != null)
                    {
                        String location = ibaFilesProKey.GetValue("") as string;
                        if ((location != null) && File.Exists(location))
                        {
                            System.Diagnostics.FileVersionInfo verInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(location);
                            if (verInfo.FileVersion != null)
                                writer.WriteLine("ibaFiles PRO {0}: {1}", i == 0 ? "32 bit" : "64 bit", verInfo.FileVersion);
                        }
                        ibaFilesProKey.Close();
                    }

                    if (i != 0)
                        baseKey.Close();

                }
            }
            catch (Exception)
            {
            }

            try
            {
                //Old license service
                RegistryKey licKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\services\\ibaLicenseService", false);
                if (licKey != null)
                {
                    string location = licKey.GetValue("ImagePath") as string;
                    if (location != null)
                    {
                        location = location.Replace("\"", "");
                        if (File.Exists(location))
                        {
                            System.Diagnostics.FileVersionInfo verInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(location);
                            if (verInfo.FileVersion != null)
                                writer.WriteLine("ibaLicenseService: {0}", verInfo.FileVersion);
                        }
                    }
                    licKey.Close();
                }
            }
            catch (Exception)
            {
            }
        }

        static void LogIbaProductVersion(StreamWriter writer, string productName, string keyName, bool bSearchInUserKeys)
        {
            RegistryKey key = OpenRegistryKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + keyName, bSearchInUserKeys);
            if (key != null)
            {
                string ver = key.GetValue("DisplayVersion") as string;
                if (ver != null)
                    writer.WriteLine("{0}: {1}", productName, ver);
                key.Close();
            }
        }

        static RegistryKey OpenRegistryKey(string keyName, bool bSearchInUserKeys)
        {
            try
            {
                RegistryKey regKey = Registry.LocalMachine.OpenSubKey(keyName, false);
                if (regKey != null)
                    return regKey;

                if (bSearchInUserKeys)
                {
                    string[] subNames = Registry.Users.GetSubKeyNames();
                    foreach (string sub in subNames)

                    {
                        regKey = Registry.Users.OpenSubKey(sub + "\\" + keyName, false);
                        if (regKey != null)
                            return regKey;
                    }
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64DisableWow64FsRedirection(ref IntPtr ptr);

        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        static extern bool Wow64RevertWow64FsRedirection(IntPtr ptr);

        static void LogIbaFobDNetworkDriverVersion(StreamWriter writer)
        {
            bool bIsWow64Disabled = false;
            IntPtr bOldValue = new IntPtr();
            string driverName = "ibaFobDNetwork.sys";
            if (Environment.Is64BitOperatingSystem)
            {
                bIsWow64Disabled = Wow64DisableWow64FsRedirection(ref bOldValue);
                driverName = "ibaFobDNetworkX64.sys";
            }

            string path = Path.Combine(Environment.SystemDirectory, "drivers", driverName);
            if (File.Exists(path))
            {
                try
                {
                    System.Diagnostics.FileVersionInfo oldSysInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(path);
                    writer.WriteLine("ibaFOB-D network driver: {0}", oldSysInfo.FileVersion);
                }
                catch (Exception)
                {
                }
            }

            if (bIsWow64Disabled)
                Wow64RevertWow64FsRedirection(bOldValue);
        }
    }

    [Serializable]
    public class MotherboardInfo
    {
        public string Manufacturer = "";
        public string Model = "";
        public string Name = "";
        public string PartNumber = "";
        public string Product = "";
        public string SerialNumber = "";
        public string SKU = "";
        public string Version = "";
    }
}
