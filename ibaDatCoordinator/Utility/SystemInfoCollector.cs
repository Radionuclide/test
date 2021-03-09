using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Management;
using System.Globalization;
using System.Diagnostics;
using System.Linq;

namespace iba.Utility
{
    [System.Reflection.Obfuscation]
    public enum CrashDumpMode
    {
        None = 0,
        Complete = 1,
        Kernel = 2,
        Minidump = 3,
        Autodump = 7
    }

    public class SystemInfoCollector
    {
        public static void SaveSystemInfo(string prefix, string postfix, string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                writer.Write(prefix);

                WriteOSVersion(writer);
                writer.WriteLine("Current culture: " + CultureInfo.CurrentCulture.EnglishName);
                writer.WriteLine("Current UI culture: " + CultureInfo.CurrentUICulture.EnglishName);
                writer.WriteLine("CLR version: " + Environment.Version.ToString());
                writer.WriteLine("Installed .NET version: " + GetInstalledDotNetVersion());

                DateTime currentUtcTime = DateTime.UtcNow;
                DateTime currentTime = currentUtcTime.ToLocalTime();
                TimeSpan utcOffset = currentTime - currentUtcTime;
                writer.WriteLine(String.Format("Current time: {0} (UTC{1}:{2})", currentTime.ToString("yyyy-MM-dd HH:mm:ss"),
                    utcOffset.Hours.ToString("+00;-00"), utcOffset.Minutes.ToString("00")));

                writer.WriteLine("Current user: " + Environment.UserName);
                writer.WriteLine("PC name: " + Environment.MachineName);

                try
                {
                    // Get domain/workgroup
                    ManagementObjectSearcher objMOS = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_ComputerSystem");
                    foreach (ManagementObject obj in objMOS.Get())
                    {
                        bool bDomain = (bool)(obj.GetPropertyValue("PartOfDomain"));
                        if (bDomain)
                            writer.WriteLine("Domain: " + (string)(obj.GetPropertyValue("Domain")));
                        else
                            writer.WriteLine("Workgroup: " + (string)(obj.GetPropertyValue("Workgroup")));

                        int domainRole = Convert.ToUInt16(obj.GetPropertyValue("DomainRole"));
                        string domainRoleStr = "UNKNOWN";

                        switch (domainRole)
                        {
                            case 0:
                                domainRoleStr = "Standalone Workstation";
                                break;
                            case 1:
                                domainRoleStr = "Member Workstation";
                                break;
                            case 2:
                                domainRoleStr = "Standalone Server";
                                break;
                            case 3:
                                domainRoleStr = "Member Server";
                                break;
                            case 4:
                                domainRoleStr = "Backup Domain Controller";
                                break;
                            case 5:
                                domainRoleStr = "Primary Domain Controller";
                                break;
                        }

                        writer.WriteLine(string.Format("Role: {0} ({1})", domainRoleStr, domainRole));
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("WMI query failed with error: {0}", ex);
                }

                writer.WriteLine();

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

                    if (!String.IsNullOrEmpty(manufacturer))
                        writer.WriteLine(String.Format("System Manufacturer: {0}", manufacturer));

                    UInt64 totalMemory = 0;
                    objMOS = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_PhysicalMemory ");
                    foreach (ManagementObject obj in objMOS.Get())
                        totalMemory += Convert.ToUInt64(obj.GetPropertyValue("Capacity"));

                    writer.WriteLine(String.Format("Total Physical Memory: {0:f2} GB ({1:f2} GB usable)", totalMemory / (1024.0 * 1024.0 * 1024.0), usableMemory / (1024.0 * 1024.0 * 1024.0)));
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("WMI query failed with error: {0}", ex);
                }

                try
                {
                    //Retrieve memory usage
                    writer.WriteLine($"Bytes in all heaps: {GC.GetTotalMemory(true) / (1024 * 1024)} MB");

                    using (Process curProcess = Process.GetCurrentProcess())
                    {
                        writer.WriteLine($"Used private bytes: {curProcess.PrivateMemorySize64 / (1024 * 1024)} MB");
                        writer.WriteLine($"Used handles: {curProcess.HandleCount}");
                    }
                }
                catch (System.Exception)
                {

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
                if (mbInfo != null)
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

                // Log BSOD info

                string minidumpDir;
                string fullDumpFile;
                CrashDumpMode dumpMode;

                writer.WriteLine();
                writer.WriteLine("System memory dumps");
                writer.WriteLine("*******************");
                writer.WriteLine();

                if (GetCrashDumpSettings(true, out dumpMode, out minidumpDir, out fullDumpFile))
                {
                    writer.WriteLine($"Crash dump mode: {dumpMode.ToString()}");
                    writer.WriteLine();

                    try
                    {
                        if (!string.IsNullOrEmpty(fullDumpFile))
                        {
                            FileInfo memDmpFile = new FileInfo(fullDumpFile);
                            if (memDmpFile.Exists)
                            {
                                writer.WriteLine($"Full memory dump ({fullDumpFile}):");
                                writer.WriteLine(string.Format("{0,-18}   {1,-11}   {2}", memDmpFile.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"), (memDmpFile.Length / 1024).ToString() + " KB", memDmpFile.Name));
                                writer.WriteLine();
                            }
                        }

                        if (!string.IsNullOrEmpty(minidumpDir))
                        {
                            DirectoryInfo miniDmpInfo = new DirectoryInfo(minidumpDir);
                            if (miniDmpInfo.Exists)
                            {

                                StringBuilder miniDumpSb = new StringBuilder();
                                foreach (FileInfo fileInfo in miniDmpInfo.EnumerateFiles())
                                {
                                    miniDumpSb.AppendLine(string.Format("{0,-18}   {1,-11}   {2}", fileInfo.LastWriteTime.ToString("yyyy-MM-dd HH:mm:ss"), (fileInfo.Length / 1024).ToString() + " KB", fileInfo.Name));
                                }

                                if (miniDumpSb.Length > 0)
                                {
                                    writer.WriteLine($"Minidump files ({minidumpDir}):");
                                    writer.WriteLine(miniDumpSb.ToString());
                                    writer.WriteLine();
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        writer.WriteLine($"Error getting system memory dump information: {ex.Message}");
                    }
                }
                else
                {
                    writer.WriteLine("Failed to retrieve crash dump settings");
                }

                try
                {
                    writer.WriteLine();
                    writer.WriteLine("Power");
                    writer.WriteLine("*******");

                    RunProcessWithLogging("powercfg", "/q", 10000, writer);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine("Power info gathering failed with error: {0}", ex);
                }

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

        public static void SaveInstalledSoftware(string fileName)
        {
            using (StreamWriter writer = new StreamWriter(fileName, false, Encoding.UTF8))
            {
                writer.WriteLine("Installed software (x86)");
                writer.WriteLine("------------------------");

                List<string> instSw = GetInstalledSoftware(false);
                if (instSw != null)
                {
                    instSw.Sort();

                    foreach (string str in instSw)
                    {
                        writer.WriteLine(str);
                    }
                }

                if (!Environment.Is64BitOperatingSystem)
                    return;

                writer.WriteLine();
                writer.WriteLine("Installed software (x64)");
                writer.WriteLine("------------------------");

                instSw = GetInstalledSoftware(true);
                if (instSw != null)
                {
                    instSw.Sort();

                    foreach (string str in instSw)
                    {
                        writer.WriteLine(str);
                    }
                }

                writer.WriteLine();
                /*
				 * Collect information about installed updates
				 * 
				 * For Windows 10/Windows Server 2016 on, all updates are cumulative updates.
				 * Those updates can be found quickly and they are not numerous.
				 *
				 * For earlier versions of Windows, not all updates are cumulative.
				 * The WMI query can then take minutes to complete, which is unacceptable for users
				 * who already require support. Therefore, in earlier Windows versions we fall back
				 * to the Windows update history, which can be retrieved fast.
				 * 
				 */
                if (Environment.OSVersion.Version.Major >= 10)
                {
                    // Versions of Windows from 10 or Server 2016 onwards
                    writer.WriteLine("Installed Windows updates");
                    writer.WriteLine("-------------------------");

                    try
                    {
                        foreach (var windowsUpdate in GetInstalledWindowsUpdates())
                            writer.WriteLine(windowsUpdate);
                    }
                    catch
                    {
                        writer.WriteLine("Failed to fetch windows updates");
                    }
                }
                else
                {
                    // Versions of Windows earlier than 10 or Server 2016
                    writer.WriteLine("Windows Update history");
                    writer.WriteLine("----------------------");

                    try
                    {
                        foreach (var windowsUpdate in GetWindowsUpdateHistory())
                            writer.WriteLine(windowsUpdate);
                    }
                    catch
                    {
                        writer.WriteLine("Failed to fetch further windows updates");
                    }
                }
            }
        }

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
            // https://docs.microsoft.com/en-us/dotnet/framework/migration-guide/how-to-determine-which-versions-are-installed#net_b
            // new versions must be added on top of this list.
            var dotNetVersionMapper = new[]
            {
                new { Number = 528040, DisplayText = "4.8" },
                new { Number = 461808, DisplayText = "4.7.2" },
                new { Number = 461308, DisplayText = "4.7.1" },
                new { Number = 460798, DisplayText = "4.7" },
                new { Number = 394802, DisplayText = "4.6.2" },
                new { Number = 394254, DisplayText = "4.6.1" },
                new { Number = 393295, DisplayText = "4.6" },
                new { Number = 379893, DisplayText = "4.5.2" },
                new { Number = 378675, DisplayText = "4.5.1" },
                new { Number = 378389, DisplayText = "4.5" },
            };


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

                        var ver = dotNetVersionMapper.FirstOrDefault(v => release >= v.Number);
                        if (ver != null)
                        {
                            version += ": " + ver.DisplayText;
                            if (ver.Number == dotNetVersionMapper[0].Number)
                                version += " or later";
                        }
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
            //Try the default for x86
            installHistoryX86 = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "iba\\InstallHistory.txt");
            if (!File.Exists(installHistoryX86))
            {
                installHistoryX86 = null;
                if (!Environment.Is64BitProcess)
                {
                    //Try relative to the installation directory
                    string installDir = Path.GetDirectoryName(typeof(SystemInfoCollector).Assembly.Location);
                    installHistoryX86 = FindFileInParentDirectories(installDir, "InstallHistory.txt");
                }
            }

            if (!Environment.Is64BitOperatingSystem)
            {
                installHistoryX64 = null;
                return;
            }

            //Get 64 bit program files directory
            string programFiles64 = Environment.Is64BitProcess ? Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) : Environment.GetEnvironmentVariable("ProgramW6432");
            if (programFiles64 == null)
            {
                installHistoryX64 = null;
                return;
            }

            //Try the default for x64
            installHistoryX64 = Path.Combine(programFiles64, "iba\\InstallHistory.txt");
            if (!File.Exists(installHistoryX64))
            {
                installHistoryX64 = null;
                if (Environment.Is64BitProcess)
                {
                    //Try relative to the installation directory
                    string installDir = Path.GetDirectoryName(typeof(SystemInfoCollector).Assembly.Location);
                    installHistoryX64 = FindFileInParentDirectories(installDir, "InstallHistory.txt");
                }
            }
        }

        private static string FindFileInParentDirectories(string dirName, string fileName)
        {
            DirectoryInfo dirInfo = Directory.GetParent(dirName);
            while (dirInfo != null)
            {
                string fileToTest = Path.Combine(dirInfo.FullName, fileName);
                if (File.Exists(fileToTest))
                    return fileToTest;

                dirInfo = dirInfo.Parent;
            }

            return null;
        }

        private static void LogIbaProductsVersionInfo(StreamWriter writer)
        {
            writer.WriteLine();
            writer.WriteLine("iba Products\r\n************");

            RegistryKey pdaKey = OpenRegistryKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ibaPDA", false, false);
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
                        if (verInfo != null)
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
            catch (Exception)
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

            RegistryKey camKey = OpenRegistryKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ibaCapture-CAM", false, true);
            if (camKey == null)
                camKey = OpenRegistryKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\ibaCapture-CAM", false, false);
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

            LogIbaProductVersion(writer, "ibaVision", "ibaVision", false);
            LogIbaProductVersion(writer, "ibaCapture-ScreenCam", "ibaCapture-HMI", false);

            LogIbaProductVersion(writer, "ibaLogic", "ibaLogicV5", false);

            try
            {
                //ibaLogic V4
                RegistryKey ibaLogicV4Key = OpenRegistryKey("Software\\iba\\ibaLogic", false, false);
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

            LogComProductVersion(writer, "ibaFiles Lite", "{089CC1F3-E635-490B-86F8-7731A185DFD9}");
            LogComProductVersion(writer, "ibaFiles Pro", "{98AE4E1A-BA50-4325-AF7F-B491CADD4F0A}");
            LogComProductVersion(writer, "ibaFiles V7 Lite", "{089CC1F3-E635-490B-86F8-7731A185DFDA}");
            LogComProductVersion(writer, "ibaFiles V7 Pro", "{98AE4E1A-BA50-4325-AF7F-B491CADD4FAA}");

            //Old license service
            LogServiceProductVersion(writer, "ibaLicenseService", "ibaLicenseService");

            LogServiceProductVersion(writer, "ibaDaVIS", "ibaDaVISService");
        }

        private static void LogServiceProductVersion(StreamWriter writer, string productName, string serviceName)
        {
            try
            {
                RegistryKey licKey = Registry.LocalMachine.OpenSubKey($"SYSTEM\\CurrentControlSet\\services\\{serviceName}", false);
                if (licKey != null)
                {
                    string location = licKey.GetValue("ImagePath") as string;
                    if (location != null)
                    {
                        if (location.StartsWith("\""))
                        {
                            int endQuote = location.IndexOf('"', 1);
                            if (endQuote > 0)
                                location = location.Substring(1, endQuote - 1);
                        }

                        if (File.Exists(location))
                        {
                            System.Diagnostics.FileVersionInfo verInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(location);
                            if (verInfo.FileVersion != null)
                                writer.WriteLine($"{productName}: {verInfo.FileVersion}");
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
            RegistryKey key = OpenRegistryKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + keyName, bSearchInUserKeys, true);
            if (key == null)
                key = OpenRegistryKey("Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\" + keyName, bSearchInUserKeys, false);

            if (key != null)
            {
                string ver = key.GetValue("DisplayVersion") as string;
                if (ver != null)
                    writer.WriteLine("{0}: {1}", productName, ver);
                key.Close();
            }
        }

        static void LogComProductVersion(StreamWriter writer, string productName, string guid)
        {
            try
            {
                //ibaFiles Lite
                for (int i = 0; i < 2; i++)
                {
                    RegistryKey baseKey = null;
                    if (i == 0)
                        baseKey = Registry.ClassesRoot;
                    else if (System.Environment.Is64BitOperatingSystem)
                        baseKey = RegistryKey.OpenBaseKey(RegistryHive.ClassesRoot, RegistryView.Registry64);
                    else
                        break;

                    RegistryKey ibaFilesLiteKey = baseKey.OpenSubKey($"CLSID\\{guid}\\InprocServer32", false);
                    if (ibaFilesLiteKey != null)
                    {
                        String location = ibaFilesLiteKey.GetValue("") as string;
                        if ((location != null) && File.Exists(location))
                        {
                            System.Diagnostics.FileVersionInfo verInfo = System.Diagnostics.FileVersionInfo.GetVersionInfo(location);
                            if (verInfo.FileVersion != null)
                                writer.WriteLine($"{productName} {(i == 0 ? "32 bit" : "64 bit")}: {verInfo.FileVersion}");
                            else
                                writer.WriteLine($"{productName} {(i == 0 ? "32 bit" : "64 bit")}: unknown version");
                        }
                        ibaFilesLiteKey.Close();
                    }

                    if (i != 0)
                        baseKey.Close();

                }
            }
            catch (Exception)
            {
            }
        }

        static RegistryKey OpenRegistryKey(string keyName, bool bSearchInUserKeys, bool b64Bit)
        {
            if (b64Bit && !Environment.Is64BitOperatingSystem)
                return null;

            try
            {
                using (RegistryKey hklmKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, b64Bit ? RegistryView.Registry64 : RegistryView.Registry32))
                {
                    RegistryKey regKey = hklmKey.OpenSubKey(keyName, false);
                    if (regKey != null)
                        return regKey;
                }

                if (bSearchInUserKeys)
                {
                    using (RegistryKey usersKey = RegistryKey.OpenBaseKey(RegistryHive.Users, b64Bit ? RegistryView.Registry64 : RegistryView.Registry32))
                    {
                        string[] subNames = usersKey.GetSubKeyNames();
                        foreach (string sub in subNames)
                        {
                            RegistryKey regKey = usersKey.OpenSubKey(sub + "\\" + keyName, false);
                            if (regKey != null)
                                return regKey;
                        }
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

        private static List<string> GetInstalledSoftware(bool b64Bit)
        {
            if (b64Bit && !Environment.Is64BitOperatingSystem)
                return null;

            List<string> instSw = new List<string>();

            try
            {
                using (RegistryKey baseKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, b64Bit ? RegistryView.Registry64 : RegistryView.Registry32))
                {
                    if (baseKey == null)
                        return null;

                    using (RegistryKey uninstallKey = baseKey.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall"))
                    {
                        foreach (string swKeyName in uninstallKey.GetSubKeyNames())
                        {
                            using (RegistryKey swKey = uninstallKey.OpenSubKey(swKeyName))
                            {
                                try
                                {
                                    string dispName = (string)swKey.GetValue("DisplayName", "");
                                    if (dispName != "")
                                    {
                                        string dispVersion = (string)swKey.GetValue("DisplayVersion", "");
                                        if (dispVersion == "")
                                            instSw.Add(dispName);
                                        else
                                            instSw.Add(string.Format("{0} [{1}]", dispName, dispVersion));
                                    }
                                }
                                catch (Exception)
                                { }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            { }

            return instSw;
        }

        /// <summary>
        /// Finds the history of Windows updates on the local machine. Should in principle be run in an STA thread.
        /// </summary>
        /// <returns>Windows update history descriptions in string format</returns>
        private static IEnumerable<string> GetWindowsUpdateHistory()
        {
            // Create IUpdateSession type from COM interface
            // Microsoft.Update.Session is the Windows Update Agent COM
            var sessionType = Type.GetTypeFromProgID("Microsoft.Update.Session");
            // Create a new instance of the COM class IUpdateSession, along with a .NET wrapper
            // use dynamic typing to work with COM interface wrappers
            dynamic session = Activator.CreateInstance(sessionType); // instead of `UpdateSession session = new IUpdateSession`
            dynamic searcher = session.CreateUpdateSearcher(); // instead of `UpdateSearcher searcher = ...`
            int historyCount = searcher.GetTotalHistoryCount();
            if (historyCount > 0)
            {
                dynamic history = searcher.QueryHistory(0, historyCount);
                for (int i = 0; i < historyCount; i++)
                {
                    dynamic historyItem = history.Item(i);
                    if (historyItem.HResult == 0) // Show only successful installations
                    {
                        DateTime date = historyItem.Date; // installation date
                        string title = historyItem.Title; // the title, which usually contains the KB number that can be googled

                        yield return $"[{date:yyyy-MM-dd HH:mm}] {title}";
                    }
                    // clean up COM references to the current history item (set reference count to 0)
                    System.Runtime.InteropServices.Marshal.FinalReleaseComObject(historyItem); // Set reference count to COM interface to 0
                }
                // clean up all references to COM components after enumerating the results (set reference counts to 0)
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(history);
            }
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(searcher);
            System.Runtime.InteropServices.Marshal.FinalReleaseComObject(session);
        }

        /// <summary>
        /// Finds installed Windows updates on the local machine.
        /// </summary>
        /// <returns>Windows update history descriptions in string format</returns>
        private static IEnumerable<string> GetInstalledWindowsUpdates()
        {
            try
            {
                ManagementObjectSearcher objMOS = new ManagementObjectSearcher("root\\CIMV2", "SELECT * FROM Win32_QuickFixEngineering");
                List<string> res = new List<string>();
                foreach (ManagementObject obj in objMOS.Get())
                {
                    string supportUrl = obj.GetPropertyValue("Caption")?.ToString();
                    string description = obj.GetPropertyValue("Description")?.ToString();
                    string kb = obj.GetPropertyValue("HotFixID")?.ToString();

                    string installed = obj.GetPropertyValue("InstalledOn")?.ToString();
                    if (!String.IsNullOrEmpty(installed) && DateTime.TryParse(installed, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out DateTime installDate))
                        installed = installDate.ToString("yyyy-MM-dd");

                    res.Add($"[{installed}] {kb} ({description}) - {supportUrl}");
                }
                return res;
            }
            catch (Exception ex)
            {
                Debug.WriteLine("WMI query failed with error: {0}", ex);
            }
            return Enumerable.Empty<string>();
        }

        /// <summary>
        /// Retrieve system crash dump settings
        /// </summary>
        /// <param name="bExpand">When true, environment variables in the minidump directory and full memory dump file name are expanded</param>
        /// <param name="mode"></param>
        /// <param name="miniDumpDir"></param>
        /// <param name="fullDumpFile"></param>
        /// <returns></returns>
        public static bool GetCrashDumpSettings(bool bExpand, out CrashDumpMode mode, out string miniDumpDir, out string fullDumpFile)
        {
            miniDumpDir = "";
            fullDumpFile = "";
            mode = CrashDumpMode.None;

            try
            {
                using (RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SYSTEM\CurrentControlSet\Control\CrashControl"))
                {
                    if (key != null)
                    {
                        miniDumpDir = (string)key.GetValue("MinidumpDir", "");
                        fullDumpFile = (string)key.GetValue("DumpFile", "");
                        mode = (CrashDumpMode)key.GetValue("CrashDumpEnabled", 0);
                    }
                }
            }
            catch
            {
                return false;
            }

            if (bExpand)
            {
                if (!string.IsNullOrEmpty(miniDumpDir))
                    miniDumpDir = Environment.ExpandEnvironmentVariables(miniDumpDir);

                if (!string.IsNullOrEmpty(fullDumpFile))
                    fullDumpFile = Environment.ExpandEnvironmentVariables(fullDumpFile);
            }

            return true;
        }

        /// <summary>
        /// Export the dongle information by using the installed ibaDongleViewer
        /// </summary>
        /// <param name="destFile">File name of the export file that should be generated.</param>
        /// <returns>True if the file was generated successfully.</returns>
        public static bool ExportDongleInfo(string destFile)
        {
            //Export dongle information
            try
            {
                RegistryKey dongleViewerKey = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows\\CurrentVersion\\App Paths\\ibaDongleViewer.exe", false);
                if (dongleViewerKey == null)
                    return false;

                string dongleViewerExe = dongleViewerKey.GetValue(null) as string;
                if (String.IsNullOrEmpty(dongleViewerExe) || !File.Exists(dongleViewerExe))
                    return false;

                FileVersionInfo fileVerInfo = FileVersionInfo.GetVersionInfo(dongleViewerExe);
                Version ver = new Version(fileVerInfo.FileMajorPart, fileVerInfo.FileMinorPart, fileVerInfo.FileBuildPart, fileVerInfo.FilePrivatePart);
                if (ver < new Version(1, 6, 1, 0))
                    return false;

                using (Process dongleViewerProc = Process.Start(dongleViewerExe, "/export:\"" + destFile + "\""))
                {
                    return dongleViewerProc.WaitForExit(5000);
                }

            }
            catch (Exception)
            {
                return false;
            }
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
