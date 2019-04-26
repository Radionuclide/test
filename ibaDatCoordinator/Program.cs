using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using iba.Utility;

using IbaAnalyzer;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Microsoft.Win32;
using System.Threading;
using System.Net;

namespace iba
{
    static class Program
    {
        static public MainForm MainForm;

        public enum ServiceEnum { CONNECTED, DISCONNECTED, NOSERVICE }

        static public ServiceEnum RunsWithService;
        static private CommunicationObjectWrapper m_comWrapper;
        static public CommunicationObjectWrapper CommunicationObject
        {
            get { return m_comWrapper; }
            set { m_comWrapper = value; }
        }

        static public string CommObjectString
        {
            get
            {
                return String.Format("gtcp://{0}:{1}/IbaDatCoordinatorCommunicationObject", ServiceHost, ServicePortNr);
            }
        }

        static public string ClientName
        {
            get
            {
                return String.Concat(Environment.MachineName, "\\", Environment.UserName);
            }
        }

        //IsServer means that we are part of a service;
        static public bool IsServer = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            IsServer = false;
            LanguageHelper.SetupLanguage(args);

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.Name = "GUI thread";

            //Check if not already running
            if (args.Length > 0 && String.Compare(args[0], "/service", true) == 0)
                RunsWithService = ServiceEnum.DISCONNECTED;
            else
                RunsWithService = ServiceEnum.NOSERVICE;

            if (RunsWithService == ServiceEnum.DISCONNECTED && PluginManager.Manager.CopyCacheNeeded()) //not needed standalone, check if we are restarted in order to copy plugins
            {
                for (int i = 0; i < 5; i++)
                {
                    if (!SingletonApp.CheckIfRunning("ibaDatCoordinatorClientCloseForm"))
                        break;
                    Thread.Sleep(500);
                }
                if (SingletonApp.CheckIfRunning("ibaDatCoordinatorClientCloseForm"))
                    return;
                switch (PluginManager.Manager.CopyPluginCache())
                {
                    case 0: break; //
                    case 1:
                        MessageBox.Show(string.Format(iba.Properties.Resources.PluginCopyFailed, PluginManager.Manager.CachePath, PluginManager.Manager.PluginPath), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.Exit();
                        return;
                    case 2:
                        MessageBox.Show(string.Format(iba.Properties.Resources.PluginSourceFailed, PluginManager.Manager.CachePath), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        Application.Exit();
                        return;
                }
            }
            else
            {
                if (SingletonApp.CheckIfRunning("ibaDatCoordinatorClientCloseForm"))
                    return;
            }

            try
            {
                if (RunsWithService == ServiceEnum.DISCONNECTED)
                    Remoting.ClientRemotingManager.SetupRemoting();

                MainForm = new MainForm();
                MainForm.WindowState = FormWindowState.Normal;
                MainForm.ShowInTaskbar = true;
                Application.Run(MainForm);
            }
            catch(Exception ex)
            {
                //This shouldn't happen
                MessageBox.Show(ex.ToString(), "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Data.LogData.StopLogger();
        }

        static int m_servicePortNr = -1;
        static string m_serverHost = "";

        internal static int ServicePortNr
        {
            get
            {
                if (m_servicePortNr < 0)
                {
                    RegistryKey key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\IBA\DATCoordinator");
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

                    RegistryKey key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\IBA\DATCoordinator");
                    if (key != null)
                    {
                        key.SetValue("PortNr", m_servicePortNr);
                        key.Close();
                    }
                }
            }
        }

        internal static string ServiceHost //should only be set or gotten by client
        {
            get
            {
                System.Diagnostics.Debug.Assert(Program.RunsWithService == ServiceEnum.CONNECTED || Program.RunsWithService == ServiceEnum.DISCONNECTED);
                if (string.IsNullOrEmpty(m_serverHost))
                {
                    var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"SOFTWARE\IBA\DATCoordinator");
                    if (key == null)
                        m_serverHost = "localhost";
                    else
                        m_serverHost = (string)key.GetValue("ServerHost", "localhost");

                    if (string.IsNullOrEmpty(m_serverHost))
                        m_serverHost = "localhost";
                }
                return m_serverHost;
            }
            set
            {
                System.Diagnostics.Debug.Assert(Program.RunsWithService == ServiceEnum.CONNECTED || Program.RunsWithService == ServiceEnum.DISCONNECTED);
                if (m_serverHost != value)
                {
                    m_serverHost = value;
                    var key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(@"SOFTWARE\IBA\DATCoordinator");
                    if (key != null)
                    {
                        key.SetValue("ServerHost", m_serverHost);
                        key.Close();
                    }
                }
            }
        }

        internal static bool ServiceIsLocal
        {
            get
            {
                //#if DEBUG
                //              return false;
                //#else
                try
                { // get host IP addresses
                    IPAddress[] hostIPs = Dns.GetHostAddresses(ServiceHost);
                    // get local IP addresses
                    IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());

                    // test if any host IP equals to any local IP or to localhost
                    foreach (IPAddress hostIP in hostIPs)
                    {
                        // is localhost
                        if (IPAddress.IsLoopback(hostIP))
                            return true;

                        // is local address
                        foreach (IPAddress localIP in localIPs)
                        {
                            // use IPAddress.Equals() to handle IPv6 addresses also
                            if (hostIP.Equals(localIP))
                                return true;
                        }
                    }
                }
                catch { }
                return false;

                //string serviceHost = ServiceHost;
                //return serviceHost == "localhost" || serviceHost.ToLower() == System.Net.Dns.GetHostName().ToLower() ;
//#endif
            }
        }
    }
}