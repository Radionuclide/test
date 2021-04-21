using System;
using System.Net;


namespace iba.Utility
{
    public class DatcoServerDefaults
    {
        public static IPAddress GroupAddress = IPAddress.Parse("226.254.92.220");
        public static Guid ServerGuid = new Guid("1CFCC362-DE03-4446-BA8D-44A48EF9A35F");
        public static int GroupServerPort = 12861;
        public static int Version = 1;
    }
}
