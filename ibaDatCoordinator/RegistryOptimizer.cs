using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace iba
{
    class RegistryOptimizer
    {
        private static bool cached = false;
        private static bool cachedValue;
        public static bool OptimizationPossible
        {
            get
            {
                try
                {
                    if (cached) return cachedValue;
                    RegistryKey keySubSystems = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Session Manager\SubSystems", false);

                    string keySubSystemsText = keySubSystems.GetValue("Windows", "", RegistryValueOptions.DoNotExpandEnvironmentNames) as string;
                    int keySubSystemsStartVals = keySubSystemsText.IndexOf("SharedSection=");
                    if (keySubSystemsStartVals < 0) return false;
                    keySubSystemsStartVals += 14;
                    int keySubSystemsStopVals = keySubSystemsText.IndexOf("Windows=On") - 1;
                    if (keySubSystemsStopVals < keySubSystemsStartVals)
                    {
                        cached = true;
                        return cachedValue = false;
                    }
                    string subsystemstring = keySubSystemsText.Substring(keySubSystemsStartVals, keySubSystemsStopVals - keySubSystemsStartVals);
                    string[] vals = subsystemstring.Split(',');
                    int v1, v2, v3;
                    if (vals.Length == 2 && int.TryParse(vals[0], out v1) && int.TryParse(vals[0], out v2))
                    {
                        v3 = v2;
                    }
                    else if (!(vals.Length == 3 && int.TryParse(vals[0], out v1) && int.TryParse(vals[1], out v2) && int.TryParse(vals[2], out v3)))
                    {
                        cached = true;
                        return cachedValue = false;
                    }

                    bool RegistrySubSystemsOK = true;

                    bool isOS64Bit = Environment.Is64BitOperatingSystem; 
                    if (v2 < 12288 && !isOS64Bit)
                    {
                        v2 = 12288;
                        RegistrySubSystemsOK = false;
                    }
                    if (v2 < 20480 && isOS64Bit)
                    {
                        v2 = 20480;
                        RegistrySubSystemsOK = false;
                    }
                    if (v3 < 2048 && !isOS64Bit)
                    {
                        v3 = 2048;
                        RegistrySubSystemsOK = false;
                    }
                    if (v3 < 4096 && isOS64Bit)
                    {
                        v3 = 4096;
                        RegistrySubSystemsOK = false;
                    }

                    bool GDIHandles32OK = true;
                    bool UserHandles32OK = true;
                    bool GDIHandles64OK = true;
                    bool UserHandles64OK = true;


                    RegistryKey keyHandles = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", true);
                    UInt32 gdiVal = Convert.ToUInt32(keyHandles.GetValue("GDIProcessHandleQuota"));
                    if (gdiVal < 16384)
                    {
                        GDIHandles32OK = false;
                        gdiVal = 16384;
                    }
                    UInt32 userVal = Convert.ToUInt32(keyHandles.GetValue("USERProcessHandleQuota"));
                    if (userVal < 18000)
                    {
                        UserHandles32OK = false;
                        userVal = 18000;
                    }

                    if (isOS64Bit)
                    {
                        keyHandles = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", true);
                        gdiVal = Convert.ToUInt32(keyHandles.GetValue("GDIProcessHandleQuota"));
                        if (gdiVal < 16384)
                        {
                            GDIHandles64OK = false;
                            gdiVal = 16384;
                        }
                        userVal = Convert.ToUInt32(keyHandles.GetValue("USERProcessHandleQuota"));
                        if (userVal < 18000)
                        {
                            UserHandles64OK = false;
                            userVal = 18000;
                        }
                    }

                    cached = true;
                    return cachedValue = !(RegistrySubSystemsOK && GDIHandles32OK && UserHandles32OK && GDIHandles64OK && UserHandles64OK);
                }
                catch (Exception)
                {
                    cached = true;
                    return cachedValue = false;
                }
            }
        }

        public static void DoWork(bool fromInstaller=false)
        {
            int action = 1; //1 = reading, 2 = exporting
            try
            {
                RegistryKey keySubSystems = Registry.LocalMachine.OpenSubKey(@"System\CurrentControlSet\Control\Session Manager\SubSystems", true);

                string keySubSystemsText = keySubSystems.GetValue("Windows", "", RegistryValueOptions.DoNotExpandEnvironmentNames) as string;
                int keySubSystemsStartVals = keySubSystemsText.IndexOf("SharedSection=");
                if (keySubSystemsStartVals < 0)
                {
                    if (!fromInstaller) MessageBox.Show(iba.Properties.Resources.regOptFormatProblem, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                keySubSystemsStartVals += 14;
                int keySubSystemsStopVals = keySubSystemsText.IndexOf("Windows=On") - 1;
                if (keySubSystemsStopVals < keySubSystemsStartVals)
                {
                    if (!fromInstaller) MessageBox.Show(iba.Properties.Resources.regOptFormatProblem, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string subsystemstring = keySubSystemsText.Substring(keySubSystemsStartVals, keySubSystemsStopVals - keySubSystemsStartVals);
                string[] vals = subsystemstring.Split(',');
                int v1, v2, v3;
                if (vals.Length == 2 && int.TryParse(vals[0], out v1) && int.TryParse(vals[0], out v2))
                {
                    v3 = v2;
                }
                else if (!(vals.Length == 3 && int.TryParse(vals[0], out v1) && int.TryParse(vals[1], out v2) && int.TryParse(vals[2], out v3)))
                {
                    if (!fromInstaller) MessageBox.Show(iba.Properties.Resources.regOptFormatProblem, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                bool RegistrySubSystemsOK = true;

                bool isOS64Bit = Environment.Is64BitOperatingSystem;
                if (v2 < 12288 && !isOS64Bit)
                {
                    v2 = 12288;
                    RegistrySubSystemsOK = false;
                }
                if (v2 < 20480 && isOS64Bit)
                {
                    v2 = 20480;
                    RegistrySubSystemsOK = false;
                }
                if (v3 < 2048 && !isOS64Bit)
                {
                    v3 = 2048;
                    RegistrySubSystemsOK = false;
                }
                if (v3 < 4096 && isOS64Bit)
                {
                    v3 = 4096;
                    RegistrySubSystemsOK = false;
                }

                bool GDIHandles32OK = true;
                bool UserHandles32OK = true;
                bool GDIHandles64OK = true;
                bool UserHandles64OK = true;


                RegistryKey keyHandles = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", true);
                UInt32 gdiVal = Convert.ToUInt32(keyHandles.GetValue("GDIProcessHandleQuota"));
                if (gdiVal < 16384)
                {
                    GDIHandles32OK = false;
                    gdiVal = 16384;
                }
                UInt32 userVal = Convert.ToUInt32(keyHandles.GetValue("USERProcessHandleQuota"));
                if (userVal < 18000)
                {
                    UserHandles32OK = false;
                    userVal = 18000;
                }

                RegistryKey keyHandles64 = null;

                if (isOS64Bit)
                {
                    keyHandles64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64).OpenSubKey(@"SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows", true);
                    gdiVal = Convert.ToUInt32(keyHandles64.GetValue("GDIProcessHandleQuota"));
                    if (gdiVal < 16384)
                    {
                        GDIHandles64OK = false;
                        gdiVal = 16384;
                    }
                    userVal = Convert.ToUInt32(keyHandles64.GetValue("USERProcessHandleQuota"));
                    if (userVal < 18000)
                    {
                        UserHandles64OK = false;
                        userVal = 18000;
                    }
                }

                if (RegistrySubSystemsOK && GDIHandles32OK && UserHandles32OK && GDIHandles64OK && UserHandles64OK)
                {
                    if (!fromInstaller) MessageBox.Show(iba.Properties.Resources.regOptEveryThingOK, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                else
                {
                    string msg = iba.Properties.Resources.regOptListKeys + Environment.NewLine + Environment.NewLine;
                    if (!RegistrySubSystemsOK)
                        msg += "     " + keySubSystems.Name + Environment.NewLine;
                    if (!GDIHandles32OK || !UserHandles32OK || !GDIHandles64OK || !UserHandles64OK)
                        msg += "     " + keyHandles.Name + Environment.NewLine;
                    if (fromInstaller)
                    {
                        msg = iba.Properties.Resources.regOptInstaller + Environment.NewLine + msg + iba.Properties.Resources.regOptInstaller2;
                        if (MessageBox.Show(msg, "ibaDatCoordinator", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                            return;
                    }
                    else
                    {
                        if (MessageBox.Show(msg, "ibaDatCoordinator", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) != DialogResult.OK)
                            return;
                    }
                }

                //take a backup
                RegExport(Path.GetDirectoryName(typeof(MainForm).Assembly.Location), true);

                //modify:
                action = 2;
                if (!RegistrySubSystemsOK)
                {
                    //TODO: Ask to Save registry keys with option to cancel ...
                    StringBuilder sb = new StringBuilder();
                    sb.Append(keySubSystemsText.Substring(0, keySubSystemsStartVals));
                    sb.Append(v1);
                    sb.Append(',');
                    sb.Append(v2);
                    sb.Append(',');
                    sb.Append(v3);
                    sb.Append(keySubSystemsText.Substring(keySubSystemsStopVals));
                    keySubSystems.SetValue("Windows", sb.ToString(), RegistryValueKind.ExpandString);
                }
                if (!GDIHandles32OK)
                    keyHandles.SetValue("GDIProcessHandleQuota", gdiVal, RegistryValueKind.DWord);
                if (!UserHandles32OK)
                    keyHandles.SetValue("USERProcessHandleQuota", userVal, RegistryValueKind.DWord);

                if (!GDIHandles64OK && keyHandles64 != null)
                    keyHandles64.SetValue("GDIProcessHandleQuota", gdiVal, RegistryValueKind.DWord);
                if (!UserHandles64OK && keyHandles64 != null)
                    keyHandles64.SetValue("USERProcessHandleQuota", userVal, RegistryValueKind.DWord);

                //TODO: Ask to reboot system, with option to postpone
                if (MessageBox.Show(iba.Properties.Resources.regOptRestart, "ibaDatCoordinator", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {                 
                    //ExitWindows.Reboot();
                    System.Diagnostics.Process.Start("ShutDown", "-r"); //restart
                };
            }
            catch (Exception ex)
            {
                string msg;
                switch (action)
                {
                    case 1:
                        msg = string.Format(iba.Properties.Resources.regOptReadingProblem, ex.Message); break;
                    //case 1:
                    //    msg = string.Format(iba.Properties.Resources.regOptSavingProblem, ex.Message); break;
                    case 2:
                        msg = string.Format(iba.Properties.Resources.regOptWritingProblem, ex.Message); break;
                    default:
                        msg = ex.Message; break;
                }
                MessageBox.Show(msg, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public static void RegExport(string targetDir, bool backup)
        {
            string key1 = @"HKLM\System\CurrentControlSet\Control\Session Manager\SubSystems";
            string key2 = @"HKLM\SOFTWARE\Microsoft\Windows NT\CurrentVersion\Windows";
            string filename1 = Path.Combine(targetDir, backup ? "subsystems_backup.reg" : "subsystems_current.reg");
            string filename2 = Path.Combine(targetDir, backup ? "handles_backup.reg" : "handles_current.reg");
            RegExportFile(key1, filename1);
            RegExportFile(key2, filename2);
        }

        private static void RegExportFile(string key, string filename)
        {
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = "REG";
                    ibaProc.StartInfo.CreateNoWindow = true;
                    ibaProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    ibaProc.StartInfo.Arguments = String.Format("EXPORT \"{0}\" \"{1}\"", key, filename);
                    if (File.Exists(filename))
                        File.Delete(filename);
                    //if (System.Environment.OSVersion.Version.Major >= 6)
                    //{
                     //   ibaProc.StartInfo.Arguments += " /y";
                    //
                    //}
                    ibaProc.Start();
                    ibaProc.WaitForExit();
                }
            }
            catch 
            { 
            }
        }
    }

    static class ExitWindows
    {

        private struct LUID
        {
            public int LowPart;
            public int HighPart;
        }
        private struct LUID_AND_ATTRIBUTES
        {
            public LUID pLuid;
            public int Attributes;
        }
        private struct TOKEN_PRIVILEGES
        {
            public int PrivilegeCount;
            public LUID_AND_ATTRIBUTES Privileges;
        }

        [DllImport("advapi32.dll")]
        static extern int OpenProcessToken(IntPtr ProcessHandle,
            int DesiredAccess, out IntPtr TokenHandle);

        [DllImport("advapi32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AdjustTokenPrivileges(IntPtr TokenHandle,
            [MarshalAs(UnmanagedType.Bool)]bool DisableAllPrivileges,
            ref TOKEN_PRIVILEGES NewState,
            UInt32 BufferLength,
            IntPtr PreviousState,
            IntPtr ReturnLength);

        [DllImport("advapi32.dll")]
        static extern int LookupPrivilegeValue(string lpSystemName,
            string lpName, out LUID lpLuid);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int ExitWindowsEx(uint uFlags, uint dwReason);

        const string SE_SHUTDOWN_NAME = "SeShutdownPrivilege";
        const short SE_PRIVILEGE_ENABLED = 2;
        const short TOKEN_ADJUST_PRIVILEGES = 32;
        const short TOKEN_QUERY = 8;

        const ushort EWX_LOGOFF = 0;
        const ushort EWX_POWEROFF = 0x00000008;
        const ushort EWX_REBOOT = 0x00000002;
        const ushort EWX_RESTARTAPPS = 0x00000040;
        const ushort EWX_SHUTDOWN = 0x00000001;
        const ushort EWX_FORCE = 0x00000004;

        private static void getPrivileges()
        {
            IntPtr hToken;
            TOKEN_PRIVILEGES tkp;

            OpenProcessToken(Process.GetCurrentProcess().Handle,
                TOKEN_ADJUST_PRIVILEGES | TOKEN_QUERY, out hToken);
            tkp.PrivilegeCount = 1;
            tkp.Privileges.Attributes = SE_PRIVILEGE_ENABLED;
            LookupPrivilegeValue("", SE_SHUTDOWN_NAME,
                out tkp.Privileges.pLuid);
            AdjustTokenPrivileges(hToken, false, ref tkp,
                0U, IntPtr.Zero, IntPtr.Zero);
        }

        public static void Shutdown() { Shutdown(false); }
        public static void Shutdown(bool force)
        {
            getPrivileges();
            ExitWindowsEx(EWX_SHUTDOWN |
                (uint)(force ? EWX_FORCE : 0) | EWX_POWEROFF, 0);
        }

        public static void Reboot() { Reboot(false); }
        public static void Reboot(bool force)
        {
            getPrivileges();
            ExitWindowsEx(EWX_REBOOT |
                (uint)(force ? EWX_FORCE : 0), 0);
        }

        public static void LogOff() { LogOff(false); }
        public static void LogOff(bool force)
        {
            getPrivileges();
            ExitWindowsEx(EWX_LOGOFF |
                (uint)(force ? EWX_FORCE : 0), 0);
        }
    }
}
