using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
[assembly: AssemblyTitle("ibaDatCoordinator")]
[assembly: AssemblyDescription("")]
[assembly: AssemblyConfiguration("")]
[assembly: AssemblyCompany("iba")]
[assembly: AssemblyProduct("ibaDatCoordinator")]
[assembly: AssemblyCopyright("Copyright © iba 2005")]
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
[assembly: AssemblyVersion("1.23.3.0")]
[assembly: AssemblyFileVersion("1.23.3.0")]

namespace iba
{
    public class DatCoVersion
    {
        public static string GetClientVersion()
        {
            string ver = typeof(iba.MainForm).Assembly.GetName().Version.ToString(3);
            ver = ver + " BETA2";
            return ver;
        }
    }
}
