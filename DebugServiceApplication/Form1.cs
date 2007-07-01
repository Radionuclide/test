using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.Xml.Serialization;
using System.IO;

using iba.Processing;
using iba.Data;
using iba.Utility;
namespace iba
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            m_communicationObject = new CommunicationObject();
            timer1.Start();
        }

        private void btStop_Click(object sender, EventArgs e)
        {
            m_communicationObject.Manager.StopAndWaitForAllConfigurations();
            m_communicationObject.ForwardEvents = false;
            LogData.StopLogger();
        }

        private CommunicationObject m_communicationObject;

        private void btStart_Click(object sender, EventArgs e)
        {
            LogData.InitializeLogger(null, null,LogData.ApplicationState.SERVICE); //dummy gridlogger
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
                //Stop();
            }

            string filename = @"C:\Program Files\iba\ibaDatCoordinator\lastsaved.xml";
            m_communicationObject.FileName = filename;


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
                        LogData.Data.MaxRows = dat.LogItemCount;
                    }
                    foreach (ConfigurationData dat in confs) dat.relinkChildData();
                    m_communicationObject.Manager.Configurations = confs;
                    foreach (ConfigurationData dat in confs)
                    {
                        if (dat.AutoStart && dat.Enabled) m_communicationObject.Manager.StartConfiguration(dat);
                    }
                }
                catch (Exception ex)
                {
                    LogData.Data.Logger.Log(Logging.Level.Exception, ex.Message);
                    //Stop();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string str = "";
            foreach (ConfigurationData cd in m_communicationObject.Manager.Configurations)
            {
                str += cd.Name + "(" + cd.Guid.ToString() + ")" + Environment.NewLine;
                foreach (TaskData td in cd.Tasks)
                    str += "-" + td.Name + Environment.NewLine; ;
            }
            Lable1.Text = str;
        }

    }
}