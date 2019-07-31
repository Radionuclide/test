using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

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
    }
}
