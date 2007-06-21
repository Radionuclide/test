using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using iba.Utility;

using IBAFILESLib;
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

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string [] args)
        {
            //System.Threading.Thread.CurrentThread.CurrentCulture =
            //    new System.Globalization.CultureInfo("fr-fr");
            //System.Threading.Thread.CurrentThread.CurrentUICulture =
            //    new System.Globalization.CultureInfo("fr-fr");

            //Check if not already running
            if (SingletonApp.CheckIfRunning())
                return;                
            if (args.Length > 0 && String.Compare(args[0], "/service", true) == 0)
            {
                RunsWithService = ServiceEnum.DISCONNECTED;
            }
            else
                RunsWithService = ServiceEnum.NOSERVICE;

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
    }
}