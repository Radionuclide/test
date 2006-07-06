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
                try
                {
                    System.ServiceProcess.ServiceController myController =
                        new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                    if (myController.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                    {
                        myController.Start();
                        myController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running,TimeSpan.FromMinutes(1.0));
                    }
                    if (myController.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                    {
                        MessageBox.Show(String.Format(iba.Properties.Resources.ServiceConnectProblem, iba.Properties.Resources.ServiceConnectProblem2, Environment.NewLine), iba.Properties.Resources.ServiceConnectProblemCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        RunsWithService = ServiceEnum.DISCONNECTED;
                    }
                    else
                    {
                        RunsWithService = ServiceEnum.CONNECTED;
                    }
                    myController.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(String.Format(iba.Properties.Resources.ServiceConnectProblem, ex.Message, Environment.NewLine), iba.Properties.Resources.ServiceConnectProblemCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    RunsWithService = ServiceEnum.DISCONNECTED;
                }
            }
            else
                RunsWithService = ServiceEnum.NOSERVICE;
            
            if (RunsWithService == ServiceEnum.CONNECTED)
            {   
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                Hashtable props = new Hashtable();
                props["port"] = 0;
                // Pass the properties for the port setting and the server provider in the server chain argument. (Client remains null here.)
                TcpChannel channel = new TcpChannel(props, clientProvider, serverProvider);
                ChannelServices.RegisterChannel(channel,false);
                CommunicationObject com = (CommunicationObject)Activator.GetObject(typeof(CommunicationObject), "tcp://localhost:8800/IbaDatCoordinatorCommunicationObject");
                m_comWrapper = new CommunicationObjectWrapper(com);
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            System.Threading.Thread.CurrentThread.Name = "GUI thread";
            MainForm = new MainForm();
            Application.Run(MainForm);
        }
    }
}