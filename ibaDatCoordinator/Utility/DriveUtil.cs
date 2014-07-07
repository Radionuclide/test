namespace iba.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.IO;

    public static class DriveUtil
    {
        private static string systemDriveName = Path.GetPathRoot(Environment.GetFolderPath(Environment.SpecialFolder.System));

        public static IEnumerable<DriveInfo> LocalDrives()
        {
            return DriveInfo.GetDrives().OrderBy(d => d.Name).Where(d => d.DriveType == DriveType.Fixed || d.DriveType == DriveType.Removable);
        }

        public static bool IsSystemDrive(this DriveInfo drive)
        {
            return (drive.Name == systemDriveName);
        }

        public static bool IsDriveReady(string driveName)
        {
            var drive = new DriveInfo(driveName);
            return IsDriveReady(drive);
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
