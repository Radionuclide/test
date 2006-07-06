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
            LogData.StopLogger();
            this.Close();
        }

        private CommunicationObject m_communicationObject;

        private void btStart_Click(object sender, EventArgs e)
        {
            LogData.InitializeLogger(null, null, false); //dummy gridlogger
            m_communicationObject = new CommunicationObject();
            //publish this manager
            BinaryServerFormatterSinkProvider serverProvider = new BinaryServerFormatterSinkProvider();
            serverProvider.TypeFilterLevel = System.Runtime.Serialization.Formatters.TypeFilterLevel.Full;
            BinaryClientFormatterSinkProvider clientProvider = new BinaryClientFormatterSinkProvider();
            IDictionary props = new Hashtable();
            props["port"] = 8800;
            TcpChannel localChannel = new TcpChannel(props, clientProvider, serverProvider);
            try
            {
                ChannelServices.RegisterChannel(localChannel, false);
                RemotingServices.Marshal(m_communicationObject, "IbaDatCoordinatorCommunicationObject", typeof(CommunicationObject));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }

            string filename = "";
            Profiler.ProfileString(true, "LastState", "LastSavedFile", ref filename, "not set");
            if (filename != "not set")
            {
                try
                {
                    XmlSerializer mySerializer = new XmlSerializer(typeof(List<ConfigurationData>));
                    List<ConfigurationData> confs;
                    using (FileStream myFileStream = new FileStream(filename, FileMode.Open))
                    {
                        confs = (List<ConfigurationData>)mySerializer.Deserialize(myFileStream);
                    }
                    m_communicationObject.FileName = filename;
                    foreach (ConfigurationData dat in confs) dat.relinkChildData();
                    m_communicationObject.Manager.Configurations = confs;
                    foreach (ConfigurationData dat in confs)
                    {
                        if (dat.AutoStart) m_communicationObject.Manager.StartConfiguration(dat.ID);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string str = "";
            foreach (ConfigurationData cd in m_communicationObject.Manager.Configurations)
            {
                str += cd.Name + "(" + cd.ID + ")" + Environment.NewLine;
                foreach (TaskData td in cd.Tasks)
                    str += "-" + td.Name + Environment.NewLine; ;
            }
            Lable1.Text = str;
        }

    }
}