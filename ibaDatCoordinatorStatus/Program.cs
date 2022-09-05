//#define DEBUG_SERVICE_COMMANDS
using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

using iba.DatCoordinator.Status.Utility;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Threading.Tasks;

namespace iba.DatCoordinator.Status
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {

#if DEBUG && DEBUG_SERVICE_COMMANDS
            // uncomment the first line to debug commands in the status application
            if (!Debugger.IsAttached)
            {
                Debugger.Launch();
            }
#endif
            System.Security.Principal.WindowsIdentity identity = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal principal = new System.Security.Principal.WindowsPrincipal(identity);
            m_IsAdmin = principal.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator);

            if (args.Length > 0 && String.Compare(args[0], "/startservice", true) == 0)
            {
                try
                {
                    System.ServiceProcess.ServiceController myController = new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                    myController.Start();
                    myController.Close();
                }
                catch
                {
                }
                return;
            }
            else if (args.Length > 0 && String.Compare(args[0], "/startservice0", true) == 0)
            {
                try
                {
                    System.ServiceProcess.ServiceController myController = new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                    string lastsaved = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "lastSaved.xml");
                    string lastsaved_backup = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "lastsaved_backup.xml");
                    if (System.IO.File.Exists(lastsaved))
                    {
                        try
                        {
                            System.IO.File.Move(lastsaved, lastsaved_backup);
                        }
                        catch
                        {

                        }
                    }
                    myController.Start();
                    myController.Close();
                }
                catch
                {
                }
                return;
            }
            else if (args.Length > 0 && String.Compare(args[0], "/stopservice", true) == 0)
            {
                try
                {
                    System.ServiceProcess.ServiceController myController = new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                    myController.Stop();
                    myController.Close();
                }
                catch
                {
                }
                return;
            }
            else if (args.Length > 0 && String.Compare(args[0], "/restartservice0", true) == 0)
            {
                try
                {
                    System.ServiceProcess.ServiceController myController = new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                    myController.Stop();
                    myController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped, TimeSpan.FromHours(1.0));
                    string lastsaved = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "lastSaved.xml");
                    string lastsaved_backup = System.IO.Path.Combine(System.IO.Path.GetDirectoryName(Application.ExecutablePath), "lastsaved_backup.xml");
                    if (System.IO.File.Exists(lastsaved))
                    {
                        try
                        {
                            System.IO.File.Move(lastsaved, lastsaved_backup);
                        }
                        catch
                        {

                        }
                    }
                    myController.Start();
                    myController.Close();
                }
                catch
                {
                }
                return;
            }
            else if (args.Length > 0 && String.Compare(args[0], "/restartservice", true) == 0)
            {
                try
                {
                    System.ServiceProcess.ServiceController myController = new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                    myController.Stop();
                    myController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped, TimeSpan.FromHours(1.0));
                    myController.Start();
                    myController.Close();
                }
                catch
                {
                }
                return;
            }
            else if (args.Length > 0 && String.Compare(args[0], "/setautomaticservicestart", true) == 0)
            {
                try
                {
                    ServiceControllerEx myController = new ServiceControllerEx("IbaDatCoordinatorService");
                    myController.ServiceStart = ServiceStart.Automatic;
                    myController.Close();
                }
                catch
                {
                }
                return;
            }
            else if (args.Length > 0 && String.Compare(args[0], "/toggleservicestart", true) == 0)
            {
                try
                {
                    ServiceControllerEx myController = new ServiceControllerEx("IbaDatCoordinatorService");
                    if (myController.ServiceStart != ServiceStart.Manual)
                        myController.ServiceStart = ServiceStart.Manual;
                    else
                        myController.ServiceStart = ServiceStart.Automatic;
                    myController.Close();
                }
                catch
                {
                }
                return;
            }
            else if (args.Length > 0 && String.Compare(args[0], "/optimizeregistryInstaller", true) == 0)
            {
                RegistryOptimizer.DoWork(true);
                return;
            }
            else if (args.Length > 0 && String.Compare(args[0], "/optimizeregistry", true) == 0)
            {
                RegistryOptimizer.DoWork(false);
                return;
            }
            else if (args.Length > 0 && args[0].Contains("/setportnumber:"))
            {
                try
                {
                    int portNumber = int.Parse(args[0].Substring(args[0].IndexOf("/setportnumber:") + 15));
                    Program.ServicePortNr = portNumber;
                }
                catch
                {
                }
                return;
            }
            else if (args.Length > 0 && args[0].Contains("/setServicePriority:"))
            {
                try
                {
                    int number = int.Parse(args[0].Substring(args[0].IndexOf("/setServicePriority:") + 20));
                    StatusForm.SetServicePriority(number);
                }
                catch
                {
                }
                return;
            }
            else if (args.Length > 0 && args[0].Contains("/stopforce:"))
            {
                StopForceFully();
                return;
            }
            else if (args.Length > 2 && args[0].Contains("/transfersettings"))
            {
                StatusForm.OnTransferSettings(args[1], args[2]);
                return;
            }

            iba.Utility.LanguageHelper.SetupLanguage(args);

            if (SingletonApp.CheckIfRunning("ibaDatCoordinatorStatusCloseForm"))
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.Name = "GUI thread";

            StatusForm statusForm = new StatusForm();
            statusForm.WindowState = FormWindowState.Minimized;
            statusForm.ShowInTaskbar = false;
            Application.Run(statusForm);
        }

        private static string GetProcessUser(Process process)
        {
            IntPtr processHandle = IntPtr.Zero;
            try
            {
                OpenProcessToken(process.Handle, 8, out processHandle);
                WindowsIdentity wi = new WindowsIdentity(processHandle);
                string user = wi.Name;
                return user.Contains(@"\") ? user.Substring(user.IndexOf(@"\") + 1) : user;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (processHandle != IntPtr.Zero)
                {
                    CloseHandle(processHandle);
                }
            }
        }

        [DllImport("advapi32.dll", SetLastError = true)]
        private static extern bool OpenProcessToken(IntPtr ProcessHandle, uint DesiredAccess, out IntPtr TokenHandle);
        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseHandle(IntPtr hObject);

        public static void StopForceFully()
        {
            try
            {
                Process[] services = Process.GetProcessesByName("ibaDatCoordinatorService");
                Process service = null;
                string serviceUser = null;
                if (services != null && services.Length > 0)
                {
                    service = services[0];
                    serviceUser = GetProcessUser(service);
                }
                var ibaAnalyzers = Process.GetProcessesByName("ibaAnalyzer").Where(p => (service == null || GetProcessUser(p) == serviceUser));
                if (ibaAnalyzers != null)
                {
                    foreach (Process p in ibaAnalyzers)
                    {
                        try
                        {
                            p.Kill();
                        }
                        catch
                        {

                        }
                    }
                }
                if (service != null)
                {
                    try
                    {
                        service.Kill();
                    }
                    catch
                    {

                    }
                }
            }
            catch
            {
                
            }
        }

        static int m_servicePortNr = -1;

        public static int ServicePortNr
        {
            get
            {
                if (m_servicePortNr < 0)
                {
                    RegistryKey key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\iba\ibaDatCoordinator");
                    if (key == null)
                        m_servicePortNr = 8800;
                    else
                        m_servicePortNr = (int)key.GetValue("PortNr", 8800);
                }
                return m_servicePortNr;
            }
            set
            {
                if (m_servicePortNr != value)
                {
                    m_servicePortNr = value;
                    RegistryKey key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(@"SOFTWARE\iba\ibaDatCoordinator");
                    if (key != null)
                    {
                        key.SetValue("PortNr", m_servicePortNr);
                        key.Close();
                    }
                }
            }
        }

        static bool m_IsAdmin;
        static public bool IsAdmin
        {
            get { return m_IsAdmin; }
        }

    }
}
