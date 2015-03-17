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

namespace iba
{
    static class Program
    {
        static public MainForm MainForm;
        public enum ServiceEnum {CONNECTED, DISCONNECTED, NOSERVICE}

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
                return String.Format("tcp://localhost:{0}/IbaDatCoordinatorCommunicationObject", ServicePortNr);
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
            else if (args.Length > 0 && String.Compare(args[0], "/setautomaticservicestart", true) == 0)
            {
                try
                {
                    iba.Controls.ServiceControllerEx myController = new iba.Controls.ServiceControllerEx("IbaDatCoordinatorService");
                    myController.ServiceStart = iba.Controls.ServiceStart.Automatic;
                    myController.Close();
                }
                catch 
                {
                }
                return;
            }
            else if (args.Length > 0 && String.Compare(args[0], "/setmanualservicestart", true) == 0)
            {
                try
                {
                    iba.Controls.ServiceControllerEx myController = new iba.Controls.ServiceControllerEx("IbaDatCoordinatorService");
                    myController.ServiceStart = iba.Controls.ServiceStart.Manual;
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

            IsServer = false;
            SetupLanguage(args);

            //Check if not already running
            if (args.Length > 0 && String.Compare(args[0], "/service", true) == 0)
            {
                RunsWithService = ServiceEnum.DISCONNECTED;
            }
            else
                RunsWithService = ServiceEnum.NOSERVICE;

            if (SingletonApp.CheckIfRunning(RunsWithService == ServiceEnum.NOSERVICE))
                return;      

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.Name = "GUI thread";
            MainForm = new MainForm();
            if (RunsWithService == ServiceEnum.DISCONNECTED)
            {   
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                Hashtable props = new Hashtable();
                props["port"] = 0;
                props["machineName"] = "localhost";
                // Pass the properties for the port setting and the server provider in the server chain argument. (Client remains null here.)
                TcpChannel channel = new TcpChannel(props, clientProvider, serverProvider);
                ChannelServices.RegisterChannel(channel,false);
                MainForm.TryToConnect(null);    
            }

            if (RunsWithService != ServiceEnum.NOSERVICE)
            {
                MainForm.WindowState = FormWindowState.Minimized;
                MainForm.ShowInTaskbar = false;
            }
            else
            {
                MainForm.WindowState = FormWindowState.Normal;
                MainForm.ShowInTaskbar = true;
            }
            Application.Run(MainForm);
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

        public static int ServicePortNr
        {
            get
            {
                if(m_servicePortNr < 0)
                {
                    var key =
                    Microsoft.Win32.Registry.LocalMachine.OpenSubKey(String.Format(@"SOFTWARE\{0}\{1}", "iba", "ibaDatCoordinator"));
                    if(key == null)
                        m_servicePortNr = 8800;
                    else
                        m_servicePortNr = (int)key.GetValue("PortNr", 8800);
                }
               return m_servicePortNr;
            }
            set
            {
                if(m_servicePortNr != value)
                {
                    m_servicePortNr = value;
                    var key =
                    Microsoft.Win32.Registry.LocalMachine.CreateSubKey(String.Format(@"SOFTWARE\{0}\{1}",
                    "iba", "ibaDatCoordinator"));
                    if (key != null)
                    {
                        key.SetValue("PortNr", m_servicePortNr);
                        key.Close();
                    }
                }
            }
        }
    }
}