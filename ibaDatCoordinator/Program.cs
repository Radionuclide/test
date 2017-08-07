using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using iba.Utility;

using ibaFilesLiteLib;
using IbaAnalyzer;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using Microsoft.Win32;

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

            //Check if not already running
            if (args.Length > 0 && String.Compare(args[0], "/service", true) == 0)
            {
                RunsWithService = ServiceEnum.DISCONNECTED;
            }
            else
                RunsWithService = ServiceEnum.NOSERVICE;

            //COMMENT OUT THESE LINES if you want to test multiple clients
            if (SingletonApp.CheckIfRunning("ibaDatCoordinatorClientCloseForm"))
                return;

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.Name = "GUI thread";

            MainForm = new MainForm();
            if (RunsWithService == ServiceEnum.DISCONNECTED)
            {
                Remoting.ClientRemotingManager.SetupRemoting();

                MainForm.TryToConnect(false);
            }

            MainForm.WindowState = FormWindowState.Normal;
            MainForm.ShowInTaskbar = true;
            Application.Run(MainForm);

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
                string serviceHost = ServiceHost;
                return serviceHost == "localhost" || serviceHost.ToLower() == System.Net.Dns.GetHostName().ToLower() ;
//#endif
            }
        }
    }
}