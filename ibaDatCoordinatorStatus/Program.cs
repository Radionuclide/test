using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;

using iba.DatCoordinator.Status.Utility;


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
            if (args.Length > 0 && String.Compare(args[0], "/restartservice", true) == 0)
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
            else if (args.Length > 0 && String.Compare(args[0], "/optimizeregistry", true) == 0)
            {
                RegistryOptimizer.DoWork();
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
