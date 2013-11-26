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
using iba.Utility;
using System.Runtime.Remoting.Lifetime;

/**
 * Description: class used to communicate with the service
 * 
 * 
 */
namespace iba
{
    public class FileProcessingProgressBar : MarshalByRefObject
    {
        public virtual bool UpdateProgress(string file, int count) {return true;}
        public virtual string Error
        {
            get { return "";}
            set { }
        }
    }

    public abstract class ScriptTester : MarshalByRefObject
    {
        abstract public void NotifyScriptEnd(int errorCode);
        public override object InitializeLifetimeService()
        {
            ILease lease =  (ILease)base.InitializeLifetimeService();
            if (lease.CurrentState == LeaseState.Initial)
            {
                lease.InitialLeaseTime = TimeSpan.FromMinutes(16);
                //lease.SponsorshipTimeout = TimeSpan.FromMinutes(2);
                lease.RenewOnCallTime = TimeSpan.FromSeconds(2);
            }
            return lease;
        }
    }

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
                    ibaDatCoordinatorData dat = ibaDatCoordinatorData.Create(m_manager);
                    mySerializer.Serialize(myWriter, dat);
                }
            }
            catch (Exception ex )
            {
                LogData.Data.Log(Logging.Level.Exception, iba.Properties.Resources.ServerSaveFileProblem + ex.ToString());
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

        public int LoggerLogLevel
        {
            get
            {
                return LogData.Data.LogLevel;
            }
            set
            {
                LogData.Data.LogLevel = value;
            }
        }

        public void LoggerClearGrid()
        {
            if (LogData.Data.Logger == null) return;
            if (LogData.Data.Logger.ChildCount > 0)
               (LogData.Data.Logger.Children[0] as GridViewLogger).clear();
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }

        public bool ForwardEvents
        {
            get 
            {
                if (LogData.Data.Logger == null) return false;
                return (LogData.Data.Logger.GetChildAt(0) as GridViewLogger).IsForwarding; 
            }
            set 
            {
                if (LogData.Data.Logger != null && LogData.Data.Logger.ChildCount > 0 && (LogData.Data.Logger.GetChildAt(0) is GridViewLogger))
                (LogData.Data.Logger.GetChildAt(0) as GridViewLogger).IsForwarding = value; 
            }
        }

        public void Logging_setEventForwarder(EventForwarder ev)
        {
            ForwardEvents = true;
            if (LogData.Data.Logger != null && LogData.Data.Logger.ChildCount > 0 && (LogData.Data.Logger.GetChildAt(0) is GridViewLogger))
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
            LogData.Data.Logger.Log(Logging.Level.Info, message);
        }

        public void TestConnection() //does nothing
        {
        }

        private Process m_scriptProc;
        private ScriptTester m_testObject;

        public void TestScript(string scriptfile, string arguments, ScriptTester testObject)
        {
            if (m_scriptProc != null) KillScript(); //just in case;
            m_testObject = testObject;
            m_scriptProc = new Process();
            m_scriptProc.EnableRaisingEvents = true;
            m_scriptProc.StartInfo.FileName = scriptfile;
            m_scriptProc.StartInfo.Arguments = arguments;
            m_scriptProc.Exited += new EventHandler(ScriptFinished);
            m_scriptProc.Start();
        }

        public void KillScript()
        {
            if (m_scriptProc != null)
            {
                try
                {
                    m_scriptProc.Exited -= new EventHandler(ScriptFinished);
                    m_scriptProc.Kill();
                    m_scriptProc.Dispose();
                }
                catch
                {
                }
                m_scriptProc = null;
                m_testObject = null;
            }
        }

        public void ScriptFinished(object sender, System.EventArgs e)
        {
            if (m_scriptProc != null)
            {
                m_scriptProc.Exited -= new EventHandler(ScriptFinished);
                if (m_testObject != null) m_testObject.NotifyScriptEnd(m_scriptProc.ExitCode);
                m_scriptProc.Dispose();
                m_scriptProc = null;
            }
        }

        public void TestNotifier(ConfigurationData m_data)
        {
            Notifier notifier = new Notifier(m_data);
            notifier.Test();
        }

        public string TestDbTaskConnection(UpdateDataTaskData udt)
        {
            return UpdateDataTaskWorker.TestConnecton(udt);
        }

        public void RemoveMarkings(string path, string username, string pass, bool recursive, FileProcessingProgressBar myBar)
        {
            try
            {
                using (FileProcessing fp = new FileProcessing(path, username, pass))
                {
                    if (!String.IsNullOrEmpty(fp.ErrorString))
                        myBar.Error = iba.Properties.Resources.RemoveMarkingsProblem + fp.ErrorString;
                    else
                    {
                        List<string> files = fp.FindFiles(recursive);
                        if (files != null && files.Count > 0)
                        {
                            myBar.UpdateProgress("filescount", files.Count);
                            fp.RemoveMarkings(files, myBar);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                myBar.Error = iba.Properties.Resources.RemoveMarkingsProblem + ex.Message;
            }
        }

        public void DeleteFiles(string path, string username, string pass, List<string> files, FileProcessingProgressBar myBar)
        {
            try
            {
                using (FileProcessing fp = new FileProcessing(path, username, pass))
                {
                    if (!String.IsNullOrEmpty(fp.ErrorString))
                        myBar.Error = iba.Properties.Resources.DeleteFilesProblem + fp.ErrorString;
                    else
                        fp.RemoveFiles(files, myBar);
                }
            }
            catch (Exception ex)
            {
                myBar.Error = iba.Properties.Resources.RemoveMarkingsProblem + ex.Message;
            }
        }

        public void RemoveMarkings(string path, string username, string pass, List<string> files, FileProcessingProgressBar myBar)
        {
            try
            {
                using (FileProcessing fp = new FileProcessing(path, username, pass))
                {
                    if (!String.IsNullOrEmpty(fp.ErrorString))
                        myBar.Error = iba.Properties.Resources.RemoveMarkingsProblem + fp.ErrorString;
                    else
                        fp.RemoveMarkings(files, myBar);
                }
            }
            catch (Exception ex)
            {
                myBar.Error = iba.Properties.Resources.RemoveMarkingsProblem + ex.Message;
            }
        }

        public string GetIbaAnalyzerRegKey()
        {
            try
            {
                string regFileName = "";
                string destDir = Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData);
                regFileName = Path.Combine(Utility.DataPath.Folder(),"ibaanalyzer.reg");
                if (!RegistryExporter.ExportIbaAnalyzerKey(regFileName))
                    return "";
                return regFileName;
            }
            catch { return ""; }
        }

        public void DeleteFile(string outFile)
        {
            try
            {
                File.Delete(outFile);
            }
            catch
            {
            }
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

        public int LoggerLogLevel
        {
            get
            {
                try
                {
                    return m_com.LoggerLogLevel;
                }
                catch (SocketException)
                {
                    HandleBrokenConnection();
                    return LogData.Data.LogLevel;
                }
            }
            set
            {
                try
                {
                    m_com.LoggerLogLevel = value;
                }
                catch (SocketException)
                {
                    HandleBrokenConnection();
                }
            }
        }

        public void LoggerClearGrid()
        {
            try
            {
                m_com.LoggerClearGrid();
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
                LogData.Data.ClearGrid();
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
                LogData.Data.Log(Logging.Level.Info, message);
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
            LogData.StopLogger();
            GridViewLogger gv = null;
            if (LogData.Data.Logger is iba.Logging.Loggers.CompositeLogger)
                gv = LogData.Data.Logger.Children[0] as GridViewLogger;
            else
                gv = LogData.Data.Logger as GridViewLogger;

            if (gv != null)
                LogData.InitializeLogger(gv.Grid, gv.LogControl, iba.Utility.ApplicationState.CLIENTDISCONNECTED);
        }

        public void TestScript(string scriptfile, string arguments, ScriptTester scripObject)
        {
            try
            {
                m_com.TestScript(scriptfile,arguments, scripObject);
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
            }
        }

        public void KillScript()
        {
            try
            {
                m_com.KillScript();
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
            }
        }

        public string TestDbTaskConnection(UpdateDataTaskData udt)
        {
            try
            {
                return m_com.TestDbTaskConnection(udt);
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
                return null;
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

        public void RemoveMarkings(string path, string username, string pass, bool recursive, FileProcessingProgressBar myBar)
        {
            try
            {
                m_com.RemoveMarkings(path, username, pass, recursive,myBar);
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
            }
        }

        public void DeleteFiles(string path, string username, string pass, List<string> files, FileProcessingProgressBar myBar)
        {
            try
            {
                m_com.DeleteFiles(path,  username,  pass, files, myBar);
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
            }
        }

        public void RemoveMarkings(string path, string username, string pass, List<string> files, FileProcessingProgressBar myBar)
        {
            try
            {
                m_com.RemoveMarkings(path,  username,  pass, files, myBar);
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
            }
        }

        public string GetIbaAnalyzerRegKey()
        {
            try
            {
                return m_com.GetIbaAnalyzerRegKey();
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
                return "";
            }
        }

        public void DeleteFile(string outFile)
        {
            try
            {
                m_com.DeleteFile(outFile);
            }
            catch (SocketException)
            {
                HandleBrokenConnection();
            }
        }
    }
}
