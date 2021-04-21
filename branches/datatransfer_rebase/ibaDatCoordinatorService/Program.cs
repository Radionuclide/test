using System;
using System.Collections.Generic;
using System.IO;
using System.ServiceProcess;
using System.Text;
using System.Reflection;
using System.Threading;
using iba.Processing;

namespace iba.Services
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            ServiceBase[] ServicesToRun;

            // More than one user Service may run within the same process. To add
            // another service to this process, change the following line to
            // create a second service object. For example,
            //
            //   ServicesToRun = new ServiceBase[] {new Service1(), new MySecondUserService()};
            //
            ServicesToRun = new ServiceBase[] { new IbaDatCoordinatorService() };

#if DEBUG
            // when started from IDE or command line then Environment.UserInteractive will be true
            // when started from service manager ServiceBase.Run() will be called
            if (Environment.UserInteractive)
                RunInteractive(ServicesToRun);
            else
                ServiceBase.Run(ServicesToRun);

#else
                ServiceBase.Run(ServicesToRun);
#endif
        }



        static void RunInteractive(ServiceBase[] servicesToRun)
        {
            Console.WriteLine("Services running in interactive mode.");
            Console.WriteLine();

            MethodInfo onStartMethod = typeof(ServiceBase).GetMethod("OnStart", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Starting {0}... ", service.ServiceName);
                onStartMethod.Invoke(service, new object[] { new string[] { } });
                Console.Write("Started");
            }

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Press any key to stop the services and end the process...");
            Console.ReadKey();
            Console.WriteLine();

            MethodInfo onStopMethod = typeof(ServiceBase).GetMethod("OnStop", BindingFlags.Instance | BindingFlags.NonPublic);
            foreach (ServiceBase service in servicesToRun)
            {
                Console.Write("Stopping {0}... ", service.ServiceName);
                onStopMethod.Invoke(service, null);
                Console.WriteLine("Stopped");
            }

            Console.WriteLine("All services stopped.");
            // Keep the console alive for a second to allow the user to see the message.
            Thread.Sleep(1000);
        }

        static int m_servicePortNr = -1;
        public static int ServicePortNr
        {
            get
            {
                if(m_servicePortNr < 0)
                {
                    var key =
                    Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\iba\ibaDatCoordinator");
                    if(key == null)
                        m_servicePortNr = 8800;
                    else
                        m_servicePortNr = (int)key.GetValue("PortNr", 8800);
                }
                return m_servicePortNr;
            }

            //no setter on service side
            //set
            //{
            //    if(m_servicePortNr != value)
            //    {
            //        m_servicePortNr = value;
            //        var key =
            //        Microsoft.Win32.Registry.LocalMachine.CreateSubKey(String.Format(@"SOFTWARE\{0}\{1}",
            //        "iba", "ibaDatCoordinator"));
            //        if(key != null)
            //        {
            //            key.SetValue("PortNr", m_servicePortNr);
            //            key.Close();
            //        }
            //    }
            //}
        }
    }
}