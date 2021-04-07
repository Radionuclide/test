using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;

namespace iba.Utility
{
    public class VersionCheck
    {
        public static bool CheckVersion(string file, string version)
        {
            try
            {
                Version ver = GetVersion(file);
                return ver != null && ver >= (new Version(version));
            }
            catch
            {
                return false;
            }
        }

        public static Version GetVersion(string file)
        {
            try
            {
                return new Version(FileVersionInfo.GetVersionInfo(file).FileVersion);
            }
            catch
            {
                return null;
            }
        }

        public static bool CheckIbaAnalyzerVersion(string version)
        {
            if (Program.MyState() == Program.ApplicationState.CLIENTCONNECTED)
            {
                return Program.CommunicationObject.CheckIbaAnalyzerVersion(version);
            }
            else
            {
                try
                {
                    RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                    object o = key.GetValue("");
                    string ibaAnalyzerExe = Path.GetFullPath(o.ToString());
                    return VersionCheck.CheckVersion(ibaAnalyzerExe, version);
                }
                catch
                {
                    return false;
                }
            }
        }
    }
}
