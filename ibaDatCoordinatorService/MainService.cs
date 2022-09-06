using iba.Data;
using iba.Logging;
using iba.Processing;
using iba.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading;
using System.Xml.Serialization;

namespace iba.Services
{
    public partial class IbaDatCoordinatorService : ServiceBase
    {
        public IbaDatCoordinatorService()
        {
            InitializeComponent();
        }

        private CommunicationObject m_communicationObject;

        private ServicePublisher m_servicePublisher;

        protected override void OnStart(string[] args)
        {
            try
            {
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);

                LogData.InitializeServerLogger();

                PluginManager.Manager.LoadPlugins();

                m_communicationObject = new CommunicationObject();
                TaskManager.Manager = m_communicationObject.Manager;

                string filename = Path.Combine(Path.GetDirectoryName(typeof(IbaDatCoordinatorService).Assembly.Location), "lastsaved.xml");
                string filename2 = Path.Combine(Path.GetDirectoryName(typeof(IbaDatCoordinatorService).Assembly.Location), "lastsaved_backup.xml");
                m_communicationObject.FileName = filename;

                SetServicePriority();

                m_communicationObject.Manager.InitializeLicenseManager();

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
                            dat = ibaDatCoordinatorData.SerializeFromStream(mySerializer, myFileStream);
                            confs = dat.ApplyToManager(m_communicationObject.Manager, null);
                        }
                        foreach (ConfigurationData dat in confs)
                        {
                            dat.relinkChildData();
                        }
                        m_communicationObject.Manager.Configurations = confs;
                        m_communicationObject.Manager.StartAllEnabledGlobalCleanups();
                        foreach (ConfigurationData dat in confs)
                        {
                            if (dat.AutoStart && dat.Enabled) m_communicationObject.Manager.StartConfiguration(dat);
                        }
                    }
                    else if (File.Exists(filename2))
                    {
                        m_communicationObject.Manager.ServiceRestartedClean = true;
                        try
                        {
                            File.Delete(filename2);
                        }
                        catch
                        {

                        }
                    }
                }
                catch (Exception ex)
                { //last saved could not be deserialised, could be from a previous install or otherwise corrupted file
                    LogData.Data.Logger.Log(Logging.Level.Exception, ex.Message);
                    LogData.Data.Logger.Log(Logging.Level.Debug, ex.ToString());
                }

                Remoting.ServerRemotingManager.SetupRemoting(m_communicationObject, Program.ServicePortNr);

                m_communicationObject.Manager.PublishService();

                // added by kolesnik - begin
                m_communicationObject.Manager.SnmpWorkerInit();
                m_communicationObject.Manager.OpcUaWorkerInit();
                // added by kolesnik - end
                m_communicationObject.Manager.DataTransferWorkerInit();
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

                try
                {
                    string file = Path.Combine(Path.GetDirectoryName(typeof(IbaDatCoordinatorService).Assembly.Location), "exception.txt");
                    using (StreamWriter sw = new StreamWriter(file))
                    {
                        sw.WriteLine("exception occured at" + DateTime.Now.ToString());
                        sw.WriteLine(ex.ToString());
                        sw.WriteLine("logfile: " + LogData.Data.FileName);
                    }
                }
                catch
                {
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
            m_communicationObject.Manager.StopAllGlobalCleanups();
            m_communicationObject.Manager.StopAndWaitForAllConfigurations();
            m_communicationObject.Manager.UninitializeLicenseManager();
            m_communicationObject.SaveConfigurations();

            //Stop publishing service
            if (m_servicePublisher != null)
            {
                try
                {
                    m_servicePublisher.StopPublishing();
                }
                catch (Exception)
                {
                }
            }
            m_servicePublisher = null;

            Remoting.ServerRemotingManager.StopRemoting(m_communicationObject);

            LogData.StopLogger();

            //try
            //{
            //    localChannel.StopListening(null);
            //    ChannelServices.UnregisterChannel(localChannel);
            //}
            //catch
            //{

            //}
        }

        protected override void OnShutdown()
        {
            OnStop();
            base.OnShutdown();
        }

        protected override void OnCustomCommand(int command) => OnCustomCommand((DatCoServiceCommand)command);
        private void OnCustomCommand(DatCoServiceCommand command)
        {
            switch (command)
            {
                case DatCoServiceCommand.TransferSettings:
                    TransferSettings();
                    break;
                case DatCoServiceCommand.SetPriority:
                    SetServicePriority();
                    break;

                default:
                    string msg = $"Unknown datCoordinator service command: {command}";
                    Debug.Assert(false, msg);
                    LogData.Data.Log(Level.Warning, msg);
                    break;
            }
        }

        private void TransferSettings()
        {
            LogData logger = LogData.Data;
            try
            {
                logger.Log(Level.Info, "Transfer ibaAnalyzer settings");


                using var key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\iba\ibaDatCoordinator");
                if (key == null) return;
                string sourcePath = (string)key.GetValue("AnalyzerFolder", "");
                string localUserSid = key.GetValue("AnalyzerUser", "") as string;

                CopyIbaAnalyzerFiles(sourcePath);

                bool transferred = RegisterIbaAnalyzerSettings(localUserSid);

                // Set the wait handle, so that the status application can continue and assume
                // that the transfer is successful
                string waitHandleName = $"Global\\ibaDatCo-transfer-settings-{localUserSid}";
                if (EventWaitHandle.TryOpenExisting(waitHandleName, out EventWaitHandle fileLock))
                {
                    try
                    {
                        fileLock.Set(); // allows the status application to delete regFile
                    }
                    finally
                    {
                        fileLock.Dispose(); // should leave the kernel object manager when the status application also releases it.
                    }
                }
            }
            catch (Exception e)
            {
                logger.Log(Level.Exception, e.Message);
            }

            return;
        }

        private void SetServicePriority()
        {
            try
            {
                int number = 2;
                var key = Microsoft.Win32.Registry.LocalMachine.OpenSubKey(@"SOFTWARE\iba\ibaDatCoordinator");
                if (key == null)
                    number = 2;
                else
                {
                    number = (int)key.GetValue("Priority", 2);
                    key.Close();
                }
                if (number >= 0 && number < 6)
                {
                    ProcessPriorityClass[] prios = new ProcessPriorityClass[]
                    {
                        System.Diagnostics.ProcessPriorityClass.Idle,
                        System.Diagnostics.ProcessPriorityClass.BelowNormal,
                        System.Diagnostics.ProcessPriorityClass.Normal,
                        System.Diagnostics.ProcessPriorityClass.AboveNormal,
                        System.Diagnostics.ProcessPriorityClass.High,
                        System.Diagnostics.ProcessPriorityClass.RealTime
                    };
                    ProcessPriorityClass prio = prios[number];
                    System.Diagnostics.Process.GetCurrentProcess().PriorityClass = prio;
                }
            }
            catch
            {
            }
        }

        public virtual void CopyIbaAnalyzerFiles(string sourcePath)
        {
            string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            targetPath = Path.Combine(targetPath, "iba", "ibaAnalyzer");
            if (!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);
            var extensions = new[] { ".mcr", ".fil", ".xml" };
            var files = (from file in Directory.EnumerateFiles(sourcePath)
                         where extensions.Contains(Path.GetExtension(file), StringComparer.InvariantCultureIgnoreCase) // comment this out if you don't want to filter extensions
                         select new
                         {
                             Source = file,
                             Destination = Path.Combine(targetPath, Path.GetFileName(file))
                         });

            foreach (var file in files)
            {
                File.Copy(file.Source, file.Destination, true);
                if (LogData.Data.Logger != null) LogData.Data.Logger.Log("copied ", file.Source, file.Destination);
            }
        }

        private bool RegisterIbaAnalyzerSettings(string localUserSid)
        {
            // TODO reg view depending on the following?
            //Environment.Is64BitOperatingSystem
            //Environment.Is64BitProcess
            string regExe = Path.Combine(Environment.SystemDirectory, "reg.exe");
            Process proc = new Process()
            {
                StartInfo = new ProcessStartInfo(regExe, $@"copy ""HKEY_USERS\{localUserSid}\Software\iba\ibaAnalyzer"" ""HKCU\Software\iba\ibaAnalyzer"" /s /f")
                {
                    UseShellExecute = false,
                    RedirectStandardError = true,
                    RedirectStandardOutput = true,
                    StandardOutputEncoding = System.Text.Encoding.UTF8,
                    StandardErrorEncoding = System.Text.Encoding.UTF8,
                    WindowStyle = ProcessWindowStyle.Hidden,
                }
            };

            proc.Start();

            proc.WaitForExit();
            string stdError = proc.StandardError.ReadToEnd();

            if (proc.ExitCode != 0)
            {
                LogData.Data.Log(Level.Exception, stdError);
                return false;
            }
            return true;
        }
    }
}
