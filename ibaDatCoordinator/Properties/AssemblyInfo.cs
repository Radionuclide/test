using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ibaDatCoordinator")]
[assembly: AssemblyDescription("ibaDatCoordinator standalone or client")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("iba AG")]
[assembly: AssemblyProduct("ibaDatCoordinator")]
[assembly: AssemblyCopyright("© iba AG. All rights reserved")]
[assembly: AssemblyTrademark("")]
[assembly: AssemblyCulture("")]

// Setting ComVisible to false makes the types in this assembly not visible 
// to COM components.  If you need to access a type in this assembly from 
// COM, set the ComVisible attribute to true on that type.
[assembly: ComVisible(false)]

// The following GUID is for the ID of the typelib if this project is exposed to COM
[assembly: Guid("dbf18f93-2396-468f-8e79-4fe266454300")]

// Version information for an assembly consists of the following four values:
//
//      Major Version
//      Minor Version 
//      Build Number
//      Revision
//
[assembly: AssemblyVersion("2.0.3.0")]
[assembly: AssemblyFileVersion("2.0.3.0")]

[assembly: InternalsVisibleTo("ibaDatCoordinatorStatus")]

namespace iba
{
    public class DatCoVersion
    {
        //this build, version of the client should be same as the service (update ibaDatCoordinatorServer.AssemblyInfo
        public static string GetVersion()
        {
            string ver = typeof(iba.MainForm).Assembly.GetName().Version.ToString(3);
            //ver = ver;
            return ver;
        }

        public static int MinimumClientVersion()
        {
            ///modify this if eventually a real minimum client is necessary (because features have been added)
            Version v = new Version(2,0,0,0);
            return ((v.Major * 1000) + v.Minor) * 1000 + v.Build;
        }

        public static int CurrentVersion() //serves as both client and server version
        {
            ///current client version, serves
            Version v = typeof(iba.MainForm).Assembly.GetName().Version;
            return ((v.Major * 1000) + v.Minor) * 1000 + v.Build;
        }

        public static string FormatVersion(int ver)
        {
            return String.Format("{0}.{1}.{2}", ver / 1000000, (ver % 1000000) / 1000, ver % 1000);
        }
    }
}
