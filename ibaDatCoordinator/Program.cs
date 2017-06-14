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
        static public StatusForm StatusForm;
        public enum ServiceEnum {CONNECTED, DISCONNECTED, NOSERVICE, STATUS}

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
                return String.Format("tcp://{1}:{0}/IbaDatCoordinatorCommunicationObject", ServicePortNr, ServiceHost);
                //return String.Format("tcp://NOTE-ELEWOUT:{0}/IbaDatCoordinatorCommunicationObject", ServicePortNr);
            }
        }

        static public string CloseFormCaption
        {
            get
            {
                if (RunsWithService == ServiceEnum.STATUS)
                    return "ibaDatCoordinatorStatusCloseForm";
                else
                    return "ibaDatCoordinatorClientCloseForm";
            }
        }

        //IsServer means that we are part of a service;
        static public bool IsServer = true;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
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
            else if (args.Length > 0 && args[0].Contains("/setportnumber:") )
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

            IsServer = false;
            SetupLanguage(args);

            //Check if not already running
            if (args.Length > 0 && String.Compare(args[0], "/service", true) == 0)
            {
                RunsWithService = ServiceEnum.DISCONNECTED;
            }
            else if (args.Length > 0 && String.Compare(args[0], "/status", true) == 0)
            {
                RunsWithService = ServiceEnum.STATUS;
            }
            else
                RunsWithService = ServiceEnum.NOSERVICE;

            //COMMENT OUT THESE LINES if you want to test multiple clients
            if (SingletonApp.CheckIfRunning())
                return;      

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.Name = "GUI thread";
            if (RunsWithService == ServiceEnum.STATUS)
            {
                StatusForm = new StatusForm();
                StatusForm.WindowState = FormWindowState.Minimized;
                StatusForm.ShowInTaskbar = false;
                Application.Run(StatusForm);
            }
            else
            {
                MainForm = new MainForm();
                if (RunsWithService == ServiceEnum.DISCONNECTED)
                {
                    BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                    BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                    serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                    Hashtable props = new Hashtable();
                    props["port"] = 0;
                    // Pass the properties for the port setting and the server provider in the server chain argument. (Client remains null here.)
                    TcpChannel channel = new TcpChannel(props, clientProvider, serverProvider);
                    ChannelServices.RegisterChannel(channel, false);
                    MainForm.TryToConnect(null);
                }

                MainForm.WindowState = FormWindowState.Normal;
                MainForm.ShowInTaskbar = true;
                Application.Run(MainForm);
            }
        }

        static void SetupLanguage(string[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                string arg1 = args[i].ToUpper();
                if (arg1.StartsWith("/LANG:"))
                {
                    string language = args[i].Substring(6, arg1.Length - 6);
                    if (language.IndexOf('-') < 0)
                    {
                        if (language == "en")
                            language = "en-us";
                        else
                            language = language + "-" + language;
                    }

                    try
                    {
                        System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo(language);
                        System.Threading.Thread.CurrentThread.CurrentUICulture = culture;
                        //System.Threading.Thread.CurrentThread.CurrentCulture = culture;
                    }
                    catch (Exception)
                    {
                    }
                    break;
                }
            }
        }


        static int m_servicePortNr = -1;
        static string m_serverHost = "";

        public static int ServicePortNr
        {
            get
            {
                if (m_servicePortNr < 0)
                {
                    RegistryKey key = null;
                    if (Program.RunsWithService == ServiceEnum.CONNECTED || Program.RunsWithService == ServiceEnum.DISCONNECTED)
                        key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(String.Format(@"SOFTWARE\{0}\{1}", "IBA", "DATCoordinator"));
                    else
                        key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(String.Format(@"SOFTWARE\{0}\{1}", "iba", "ibaDatCoordinator"));
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
                    RegistryKey key = null;
                    if (Program.RunsWithService == ServiceEnum.CONNECTED || Program.RunsWithService == ServiceEnum.DISCONNECTED)
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(String.Format(@"SOFTWARE\{0}\{1}",
                    "IBA", "DATCoordinator"));
                    else
                        key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(String.Format(@"SOFTWARE\{0}\{1}",
                    "iba", "ibaDatCoordinator"));
                    if (key != null)
                    {
                        key.SetValue("PortNr", m_servicePortNr);
                        key.Close();
                    }
                }
            }
        }

        public static string ServiceHost //should only be set or gotten by client
        {
            get
            {
                System.Diagnostics.Debug.Assert(Program.RunsWithService == ServiceEnum.CONNECTED || Program.RunsWithService == ServiceEnum.DISCONNECTED);
                if (string.IsNullOrEmpty(m_serverHost))
                {
                    var key =
                    Microsoft.Win32.Registry.CurrentUser.OpenSubKey(String.Format(@"SOFTWARE\{0}\{1}", "IBA", "DATCoordinator"));
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
                    var key =
                    Microsoft.Win32.Registry.CurrentUser.CreateSubKey(String.Format(@"SOFTWARE\{0}\{1}",
                    "IBA", "DATCoordinator"));
                    if (key != null)
                    {
                        key.SetValue("ServerHost", m_serverHost);
                        key.Close();
                    }
                }
            }
        }

        public static bool ServiceIsLocal
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