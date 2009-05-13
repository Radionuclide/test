using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.ServiceProcess;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Text;
using System.Xml.Serialization;
using System.IO;

using iba.Processing;
using iba.Data;
using iba.Utility;

namespace iba.Services
{
    public partial class IbaDatCoordinatorService : ServiceBase
    {
        public IbaDatCoordinatorService()
        {
            InitializeComponent();
        }

        private CommunicationObject m_communicationObject;

        protected override void OnStart(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
                LogData.InitializeLogger(null, null, LogData.ApplicationState.SERVICE); //dummy gridlogger
                PluginManager.Manager.LoadPlugins();
                m_communicationObject = new CommunicationObject();

                string filename = Path.Combine(Path.GetDirectoryName(typeof(IbaDatCoordinatorService).Assembly.Location), "lastsaved.xml");
                m_communicationObject.FileName = filename;

                //if (args.Length > 0 && String.Compare(args[0], "loadnotfromfile", true) == 0)
                //return;
                try
                {
                    if (File.Exists(filename))
                    {
                        XmlSerializer mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));
                        List<ConfigurationData> confs;
                        using (FileStream myFileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            ibaDatCoordinatorData dat = null;
                                dat = (ibaDatCoordinatorData)mySerializer.Deserialize(myFileStream);
                            m_communicationObject.Manager.ReplaceWatchdogData(dat.WatchDogData);
                            m_communicationObject.Manager.WatchDog.Settings = dat.WatchDogData;
                            confs = dat.Configurations;
                            if (dat.LogItemCount == 0) dat.LogItemCount = 50;
                            LogData.Data.MaxRows = dat.LogItemCount;
                        }
                        foreach (ConfigurationData dat in confs)
                        {
                            dat.relinkChildData();
                        }
                        m_communicationObject.Manager.Configurations = confs;
                        foreach (ConfigurationData dat in confs)
                        {
                            if (dat.AutoStart && dat.Enabled) m_communicationObject.Manager.StartConfiguration(dat);
                        }
                    }
                }
                catch (Exception ex)
                { //last saved could not be deserialised, could be from a previous install or otherwise corrupted file
                    LogData.Data.Logger.Log(Logging.Level.Exception, ex.Message);
                }

                //publish this manager
                BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
                serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
                BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
                IDictionary props = new Hashtable();
                props["port"] = 8800;
                props["machineName"] = "localhost";
                TcpChannel localChannel = new TcpChannel(props, clientProvider, serverProvider);
                ChannelServices.RegisterChannel(localChannel, false);
                RemotingServices.Marshal(m_communicationObject, "IbaDatCoordinatorCommunicationObject", typeof(CommunicationObject));
            }
            catch (Exception ex)
            {
                try
                {
                    LogData.Data.Logger.Log(Logging.Level.Exception, ex.Message);
                }
                catch
                { 
                }
                
                string file = Path.Combine(Path.GetDirectoryName(typeof(IbaDatCoordinatorService).Assembly.Location), "exception.txt");
                using (StreamWriter sw = new StreamWriter(file))
                {
                    sw.WriteLine("exception occured at" + DateTime.Now.ToString());
                    sw.Write(ex.ToString());
                    sw.WriteLine("logfile: " + LogData.Data.FileName);
                }
                Stop();
            }
        }

        void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            string file = Path.Combine(Path.GetDirectoryName(typeof(IbaDatCoordinatorService).Assembly.Location), "exception.txt");
            using (StreamWriter sw = new StreamWriter(file))
            {
                sw.WriteLine("exception occured at" + DateTime.Now.ToString());
                sw.Write((e.ExceptionObject as Exception).ToString());
            }
        }

        protected override void OnStop()
        {
            m_communicationObject.Manager.StopAndWaitForAllConfigurations();
            m_communicationObject.SaveConfigurations();

            m_communicationObject.ForwardEvents = false;
            LogData.StopLogger();
        }

        protected override void OnShutdown()
        {
            OnStop();
            base.OnShutdown();
        }
    }
}
