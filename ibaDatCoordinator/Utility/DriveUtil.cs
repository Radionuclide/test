using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace iba.Utility
{

    public static class DriveUtil
    {
        private static string systemDriveName = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));

        public static IEnumerable<DriveInfo> LocalDrives()
        {
            return DriveInfo.GetDrives().OrderBy(d => d.Name).Where(d => d.DriveType == DriveType.Fixed);
        }

        public static bool IsSystemDrive(this DriveInfo drive)
        {
            return (drive.Name == systemDriveName);
        }

        public static bool IsDriveReady(string driveName)
        {
            try
            {
                return new DriveInfo(driveName).IsDriveReady();
            }
            catch (Exception)
            { }
            return false; 
        }

        public static bool IsDriveReady(this DriveInfo drive)
        {
            try
            {
                return drive.IsReady;
            }
            catch (Exception)
            { }
            return false;
        }
    }
}
