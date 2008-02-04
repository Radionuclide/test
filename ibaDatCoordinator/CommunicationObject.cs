using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.IO;
using iba.Data;
using iba.Processing;
using System.Diagnostics;

/**
 * Description: class used to communicate with the service
 * 
 * 
 */
namespace iba
{
    public class CommunicationObject: MarshalByRefObject
    {
        private TaskManager m_manager;
        public TaskManager Manager
        {
            get { return m_manager; }
            set { m_manager = value; }
        }

        public void SaveConfigurations()
        {
            XmlSerializer mySerializer = new XmlSerializer(typeof(ibaDatCoordinatorData));
            // To write to a file, create a StreamWriter object.
            try
            {
                using (StreamWriter myWriter = new StreamWriter(m_filename))
                {
                    ibaDatCoordinatorData dat = new ibaDatCoordinatorData(
                        Manager.WatchDogData,
                        Manager.Configurations,
                        LogData.Data.MaxRows
                        );
                    mySerializer.Serialize(myWriter, dat);
                }
            }
            catch (Exception)
            {
                if (LogData.Data.Logger.IsOpen)
                    LogData.Data.Logger.Log(Logging.Level.Exception, iba.Properties.Resources.ServerSaveFileProblem);
            }
        }

        public void SetPriority(System.Diagnostics.ProcessPriorityClass priority)
        {
            try
            {
                System.Diagnostics.Process.GetCurrentProcess().PriorityClass = priority;
            }
            catch
            {
            }
        }

        public CommunicationObject()
        {
            m_manager = new TaskManager();
            m_filename = null;
            m_manager.WatchDog.SetCommunication(this);
        }

        private string m_filename;
        public string FileName
        {
            get { return m_filename; }
            set { m_filename = value; }
        }

        public int LoggerMaxRows
        {
            get
            {
                return LogData.Data.MaxRows;
            }
            set
            {
                LogData.Data.MaxRows = value;
            }
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public bool ForwardEvents
        {
            get { return (LogData.Data.Logger.GetChildAt(0) as GridViewLogger).IsForwarding; }
            set { if (LogData.Data.Logger.ChildCount>0)
                (LogData.Data.Logger.GetChildAt(0) as GridViewLogger).IsForwarding = value; 
            }
        }

        public void Logging_setEventForwarder(EventForwarder ev)
        {
            ForwardEvents = true;
            (LogData.Data.Logger.GetChildAt(0) as GridViewLogger).Forwarder = ev;
        }

        public string Logging_fileName
        {
            get
            {
                return LogData.Data.FileName;
            }
        }

        public void Logging_Log(string message)
        {
            if (LogData.Data.Logger.IsOpen) 
                LogData.Data.Logger.Log(Logging.Level.Info, message);
        }

        public void TestConnection() //does nothing
        {
        }

        public int TestScript(string scriptfile, string arguments)
        {
            using (Process ibaProc = new Process())
            {
                ibaProc.EnableRaisingEvents = false;
                ibaProc.StartInfo.FileName = scriptfile;
                ibaProc.StartInfo.Arguments = arguments;
                ibaProc.Start();
                if (!ibaProc.WaitForExit(15 * 3600 * 1000))
                {   //wait max 15 minutes
                    ibaProc.Kill();
                    return -666;
                }
                return ibaProc.ExitCode;
            }
        }

        public void TestNotifier(ConfigurationData m_data)
        {
            Notifier notifier = new Notifier(m_data);
            notifier.Test();
        }
    }

    public class CommunicationObjectWrapper
    {
        private CommunicationObject m_com;

        public CommunicationObjectWrapper(CommunicationObject com)
        {
            m_com = com;
        }

        public bool TestConnection()
        {
            try
            {
                m_com.TestConnection();
                return true;
            }
            catch (SocketException)
            {
                return false;
            }
        }

        public void SaveConfigurations()
        {
            try
            {
                m_com.SaveConfigurations();
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
            }
        }

        public void SetPriority(System.Diagnostics.ProcessPriorityClass priority)
        {
            m_com.SetPriority(priority);
        }

        public int LoggerMaxRows
        {
            get
            {
                try
                {
                    return m_com.LoggerMaxRows;
                }
                catch (SocketException)
                {
                    HandleBrokenConnection();
                    return LogData.Data.MaxRows;
                }
            }
            set
            {
                try
                {
                    m_com.LoggerMaxRows = value;
                }
                catch (SocketException)
                {
                    HandleBrokenConnection();
                }
            }
        }

        public TaskManager Manager
        {
            get 
            {
                try
                {
                    return m_com.Manager;
                }
                catch (SocketException)
                {
                    HandleBrokenConnection();
                    return TaskManager.Manager;
                }
            }
            set
            {
                try
                {
                    m_com.Manager = value;
                }
                catch (SocketException)
                {
                    HandleBrokenConnection();
                }
            }
        }

        public bool ForwardEvents
        {
            get
            {
                try
                {
                    return m_com.ForwardEvents;
                }
                catch (SocketException)
                {
                    HandleBrokenConnection();
                    return false;
                }
            }
            set
            {
                try
                {
                    m_com.ForwardEvents = value;
                }
                catch (SocketException)
                {
                    HandleBrokenConnection();
                }
            }
        }

        public string Logging_fileName
        {
            get
            {
                try
                {
                    return m_com.Logging_fileName;
                }
                catch (SocketException)
                {
                    HandleBrokenConnection();
                    return null;
                }
            }
        }


        public void Logging_Log(string message)
        {
            try
            {
                m_com.Logging_Log(message);
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
                if (LogData.Data.Logger.IsOpen)
                    LogData.Data.Logger.Log(Logging.Level.Info, message);
            }
        }

        public void Logging_setEventForwarder(EventForwarder ev)
        {
            try
            {
                m_com.Logging_setEventForwarder(ev);
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
            }
        }

        public void HandleBrokenConnection()
        {
            if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED) return;
            Program.RunsWithService = Program.ServiceEnum.DISCONNECTED;
            //if (!m_stoppingService) MessageBox.Show(iba.Properties.Resources.connectionLost, iba.Properties.Resources.connectionLostCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            Program.MainForm.ReplaceManagerFromTree(TaskManager.Manager);
            Program.MainForm.Icon = iba.Properties.Resources.disconnectedIcon;
            Program.MainForm.NotifyIcon.Icon = iba.Properties.Resources.disconnectedIcon;
            Program.MainForm.NotifyIcon.Text = iba.Properties.Resources.niDisconnected;
            Program.MainForm.StartButton.Enabled = false;
            Program.MainForm.StopButton.Enabled = false;
            Program.MainForm.SetRenderer();
            TaskManager.Manager.StopAllConfigurations();
            //logger resetten
            LogData.Data.Logger.Close();
            GridViewLogger gv = null;
            if (LogData.Data.Logger is iba.Logging.Loggers.CompositeLogger)
                gv = LogData.Data.Logger.Children[0] as GridViewLogger;
            else
                gv = LogData.Data.Logger as GridViewLogger;

            LogData.InitializeLogger(gv.Grid, gv.LogControl, LogData.ApplicationState.CLIENTDISCONNECTED);
        }

        public int TestScript(string scriptfile, string arguments)
        {
            try
            {
                return m_com.TestScript(scriptfile,arguments);
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
                return -1;
            }
        }

        public void TestNotifier(ConfigurationData m_data)
        {
            try
            {
                m_com.TestNotifier(m_data);
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
            }
        }
    }
}
