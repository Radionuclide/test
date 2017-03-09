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
                string ver = FileVersionInfo.GetVersionInfo(file).FileVersion;
                return (new Version(ver)) >= (new Version(version));
            }
            catch
            {
                return false;
            }
        }
    }
}
