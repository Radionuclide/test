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
            AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            LogData.InitializeLogger(null, null, false); //dummy gridlogger
            try
            {
                PluginManager.Manager.LoadPlugins();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Logging.Level.Exception, ex.Message);
                Stop();
            }

            m_communicationObject = new CommunicationObject();
            //publish this manager
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            IDictionary props = new Hashtable();
            props["port"] = 8800;
            props["machineName"] = "localhost";
            TcpChannel localChannel = new TcpChannel(props, clientProvider, serverProvider);
            try
            {
                ChannelServices.RegisterChannel(localChannel, false);
                RemotingServices.Marshal(m_communicationObject, "IbaDatCoordinatorCommunicationObject", typeof(CommunicationObject));
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Logging.Level.Exception, ex.Message);
                Stop();
            }

            string filename = Path.Combine(Path.GetDirectoryName(typeof(IbaDatCoordinatorService).Assembly.Location), "lastsaved.xml");
            m_communicationObject.FileName = filename;

            if (args.Length > 0 && String.Compare(args[0], "loadnotfromfile", true) == 0)
                return;
            if (File.Exists(filename))
            {
                try
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));
                    List<ConfigurationData> confs;
                    using (FileStream myFileStream = new FileStream(filename, FileMode.Open))
                    {
                        ibaDatCoordinatorData dat = null;
                        try
                        {
                            dat = (ibaDatCoordinatorData)mySerializer.Deserialize(myFileStream);
                        }
                        catch (Exception ex)
                        { //last saved could not be deserialised, could be from a previous install or otherwise corrupted file
                            LogData.Data.Logger.Log(Logging.Level.Exception, ex.Message);
                            return;
                        }
                        m_communicationObject.Manager.ReplaceWatchdogData(dat.WatchDogData);
                        m_communicationObject.Manager.WatchDog.Settings = dat.WatchDogData;
                        confs = dat.Configurations;
                        if (LogData.Data.FileName != dat.Logfile)
                            LogData.OpenFromFile(dat.Logfile);
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
                catch (Exception ex)
                {
                    LogData.Data.Logger.Log(Logging.Level.Exception, ex.Message);
                    Stop();
                }
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
