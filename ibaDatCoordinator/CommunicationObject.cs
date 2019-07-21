using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.IO;
using iba.Data;
using iba.Processing;
using System.Diagnostics;
using iba.Utility;
using iba.Remoting;
using System.Runtime.Remoting.Lifetime;
using iba.Dialogs;
using System.Threading;

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

        private bool finished;
        public bool Finished
        {
            get { return finished; }
            set { finished = value; }
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

        public override object InitializeLifetimeService()
        {
            //This is only called on the server where this object must live forever
            Debug.Assert(Program.IsServer);
            return null;
        }

        public void Logging_setEventForwarder(IEventForwarder ev, Guid g, string clientName)
        {
            RemoteLogger remoteLogger = LogData.RemoteLogger;
            if (remoteLogger != null)
                remoteLogger.AddForwarder(ev, g, clientName);
        }

        public void Logging_clearEventForwarder(Guid g)
        {
            RemoteLogger remoteLogger = LogData.RemoteLogger;
            if (remoteLogger != null)
                remoteLogger.RemoveForwarder(g);
        }

        public void TestConnection() //does nothing
        {
        }

        //This function is called by a client when connecting to the server
        public int Connect(string clientName, int clientVersion, string userName, string password)
        {
            //We allow all clients
            string remoteEp = Remoting.ServerRemotingManager.RegisterClient(clientName);
            LogData.Data.Log(Logging.Level.Debug, String.Format("Client {0} (version {1}) is trying to connect from {2}.", clientName, 
                DatCoVersion.FormatVersion(clientVersion), remoteEp));
            LogData.Data.Log(Logging.Level.Info, String.Format(Properties.Resources.ClientConnected, clientName));

            return DatCoVersion.CurrentVersion();
        }

        public string GetVersion()
        {
            return DatCoVersion.GetVersion();
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

        public void RemoveMarkings(string path, string username, string pass, string filepass, bool recursive, FileProcessingProgressBar myBar)
        {
            try
            {
                using (FileProcessing fp = new FileProcessing(path, username, pass, filepass))
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
            finally
            {
                myBar.Finished = true;
            }
        }


        public void RemoveMarkings(string path, string username, string pass, string filepass, List<string> files, FileProcessingProgressBar myBar)
        {
            try
            {
                using (FileProcessing fp = new FileProcessing(path, username, pass, filepass))
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
            finally
            {
                myBar.Finished = true;
            }
        }

        public void RemoveMarkingsAsync(string path, string username, string pass, string filepass, bool recursive, FileProcessingProgressBar myBar)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                RemoveMarkings(path, username, pass, filepass, recursive, myBar);
            });
        }

        public void RemoveMarkingsAsync(string path, string username, string pass, string filepass, List<string> files, FileProcessingProgressBar myBar)
        {
            ThreadPool.QueueUserWorkItem(o =>
            {
                RemoveMarkings(path, username, pass, filepass, files, myBar);
            });
        }


        public void DeleteFiles(string path, string username, string pass, List<string> files, FileProcessingProgressBar myBar)
        {
            try
            {
                using (FileProcessing fp = new FileProcessing(path, username, pass,""))
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
            finally
            {
                myBar.Finished = true;
            }

        }


        public string GetIbaAnalyzerRegKey()
        {
            try
            {
                string regFileName = "";
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

        public IPdaServerFiles GetServerSideFileHandler()
        {
            return new PdaServerFiles();
        }

        internal float TestCondition(string expression, int index, string pdo, string datfile, string pass, out string errorMessage)
        {
            return iba.Controls.IfTaskControl.TestCondition(expression, index, pdo, datfile, pass, out errorMessage);
        }

        public bool FileExists(string file)
        {
            return File.Exists(file);
        }

        public string ReadFile(string filename, out Exception ex)
        {
            ex = null;
            try
            {
                return DataPath.ReadFile(filename);
            }
            catch (Exception ex2)
            {
                ex = ex2;
                return "";
            }
        }

        public bool IsReadOnly(string filename, out Exception ex)
        {
            ex = null;
            try
            {
                return DataPath.IsReadOnly(filename);
            }
            catch (Exception ex2)
            {
                ex = ex2;
                return false;
            }
        }

        public void WriteFile(string filename, string text, out Exception ex)
        {
            ex = null;
            try
            {
                DataPath.WriteFile(filename,text);
            }
            catch (Exception ex2)
            {
                ex = ex2;
            }
        }

        public ServerFileInfo GenerateSupportZipFile()
        {
            string tempDir = System.IO.Path.GetTempPath();
            string tempZipfile = Path.Combine(tempDir, "server.zip");
            SupportFileGenerator.GenerateServerZipFile(tempZipfile);
            return new ServerFileInfo(tempZipfile);
        }

        public ServerFileInfo[] GetPluginFiles()
        {
            return PluginManager.Manager.GetPluginFiles();
        }

        public string GetPluginPath()
        {
            return PluginManager.Manager.PluginPath;
        }

        public bool HasPlugin(string name)
        {
            return PluginManager.Manager.HasPlugin(name);
        }
    }

    public class CommunicationObjectWrapper
    {
        private CommunicationObject m_com;

        public CommunicationObjectWrapper(CommunicationObject com)
        {
            m_com = com;
        }

        //This will return the server version when connection is ok and throw if it isn't
        public int Connect()
        {
            try
            {
                return m_com.Connect(Program.ClientName, DatCoVersion.CurrentVersion(), "admin", "");
            }
            catch(Exception ex)
            {
                Remoting.ClientRemotingManager.Disconnect(m_com, ex);
                throw ex;
            }
        }

        public bool TestConnection()
        {
            try
            {
                m_com.TestConnection();
                if (m_com.Manager.Version >= 2000000)
                    return true;
                else
                    return false;
                //MessageBox.Show("OK");
                //return true;
            }
            catch (Exception) //Do Nothing
            {
                //MessageBox.Show(ex.Message);
                return false;
            }
        }

        public ServerFileInfo GenerateSupportZipFile()
        {
            try
            {
                return m_com.GenerateSupportZipFile();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return null;
            }
        }

        public ServerFileInfo[] GetPluginFiles()
        {
            try
            {
                return m_com.GetPluginFiles();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return null;
            }
        }

        public string GetPluginPath()
        {
            try
            {
                return m_com.GetPluginPath();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return null;
            }
        }


        public bool HasPlugin(string name)
        { 
            try
            {
                return m_com.HasPlugin(name);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return false;
            }
        }

        public void SaveConfigurations()
        {
            try
            {
                m_com.SaveConfigurations();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
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
                catch (Exception ex)
                {
                    HandleBrokenConnection(ex);
                    return TaskManager.Manager;
                }
            }
            set
            {
                try
                {
                    m_com.Manager = value;
                }
                catch (Exception ex)
                {
                    HandleBrokenConnection(ex);
                }
            }
        }

        public void Logging_setEventForwarder(IEventForwarder ev, Guid g)
        {
            try
            {
                m_com.Logging_setEventForwarder(ev, g, Program.ClientName);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void Logging_clearEventForwarder( Guid g)
        {
            try
            {
                m_com.Logging_clearEventForwarder(g);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        private void HandleBrokenConnectionGUI()
        {
            if (Program.MainForm.InvokeRequired)
            {
                Program.MainForm.BeginInvoke(new Action(()=>HandleBrokenConnectionGUI()));
                return;
            }
            Program.MainForm.ReplaceManagerFromTree(TaskManager.Manager);
            Program.MainForm.Icon = iba.Properties.Resources.disconnectedIcon;
            //Program.MainForm.NotifyIcon.Icon = iba.Properties.Resources.disconnectedIcon;
            //Program.MainForm.NotifyIcon.Text = iba.Properties.Resources.niDisconnected;
            Program.MainForm.StartButton.Enabled = false;
            Program.MainForm.StopButton.Enabled = false;
            Program.MainForm.SetRenderer();
            Program.MainForm.UpdateConnectionStatus();
        }

        public void HandleBrokenConnection(Exception ex)
        {
            if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
                return;

            Program.RunsWithService = Program.ServiceEnum.DISCONNECTED;
            //if (!m_stoppingService) MessageBox.Show(iba.Properties.Resources.connectionLost, iba.Properties.Resources.connectionLostCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            HandleBrokenConnectionGUI();

            TaskManager.Manager.StopAllConfigurations();
            TaskManager.Manager.StopAllGlobalCleanups();

            if (ex == null)
                ex = new Exception("Handle Broken connection called without exception");

            if (m_com != null)
                Remoting.ClientRemotingManager.Disconnect(m_com, ex);

            LogData.Data.Log(Logging.Level.Exception, ex.ToString());
        }

        public string GetVersion()
        {
            try
            {
                return m_com.GetVersion();
            }
            catch
            {
                return "?";
            }
        }
        public void TestScript(string scriptfile, string arguments, ScriptTester scripObject)
        {
            try
            {
                m_com.TestScript(scriptfile,arguments, scripObject);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void KillScript()
        {
            try
            {
                m_com.KillScript();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public string TestDbTaskConnection(UpdateDataTaskData udt)
        {
            try
            {
                return m_com.TestDbTaskConnection(udt);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return null;
            }
        }

        public void TestNotifier(ConfigurationData m_data)
        {
            try
            {
                m_com.TestNotifier(m_data);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void RemoveMarkingsAsync(string path, string username, string pass, string filepass, bool recursive, FileProcessingProgressBar myBar)
        {
            try
            {
                m_com.RemoveMarkingsAsync(path, username, pass, filepass, recursive, myBar);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void RemoveMarkingsAsync(string path, string username, string pass, string filepass, List<string> files, FileProcessingProgressBar myBar)
        {
            try
            {
                m_com.RemoveMarkingsAsync(path, username, pass, filepass, files, myBar);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public void DeleteFiles(string path, string username, string pass, List<string> files, FileProcessingProgressBar myBar)
        {
            try
            {
                m_com.DeleteFiles(path,  username,  pass, files, myBar);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }


        public string GetIbaAnalyzerRegKey()
        {
            try
            {
                return m_com.GetIbaAnalyzerRegKey();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return "";
            }
        }

        public void DeleteFile(string outFile)
        {
            try
            {
                m_com.DeleteFile(outFile);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
        }

        public IPdaServerFiles GetServerSideFileHandler()
        {
            try
            {
                return m_com.GetServerSideFileHandler();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return null;
            }
        }

        internal float TestCondition(string expression, int index, string pdo, string datfile, string pass, out string errorMessage)
        {
            try
            {
                return m_com.TestCondition(expression,index,pdo,datfile, pass, out errorMessage);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                errorMessage = iba.Properties.Resources.connectionLost;
                return float.NaN;
            }
        }

        public bool FileExists(string file)
        {
            try
            {
                return m_com.FileExists(file);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return false;
            }
        }

        public string ReadFile(string filename)
        {
            Exception exfile = null;
            string res = "";
            try
            {
                res = m_com.ReadFile(filename, out exfile);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return "";
            }
            if (exfile != null) throw exfile;
            return res;
        }


        public bool IsReadOnly(string filename)
        {
            Exception exfile = null;
            bool res;
            try
            {
                res = m_com.IsReadOnly(filename, out exfile);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return false;
            }
            if (exfile != null) throw exfile;
            return res;
        }

        public void WriteFile(string filename, string text)
        {
            Exception exfile = null;
            try
            {
                m_com.WriteFile(filename, text, out exfile);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
            }
            if (exfile != null) throw exfile;
        }

    }
}
