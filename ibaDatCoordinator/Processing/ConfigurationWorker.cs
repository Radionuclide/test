using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using iba.Data;
using IBAFILESLib;
using iba.Utility;
using iba.Plugins;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace iba.Processing
{
    class ConfigurationWorker
    {
        private ConfigurationData m_cd;

        public ConfigurationData RunningConfiguration
        {
            get {return m_cd;}
        }

        private Thread m_thread;
        private StatusData m_sd;
        private Notifier m_notifier;
        public StatusData Status
        {
            get { return m_sd; }
        }

        private bool m_stop;

        public bool Stop
        {
            get { return m_stop; }
            set 
            {
                m_stop = value;
                if (m_stop)
                    m_waitEvent.Set();
                //do finalization of custom tasks
                foreach (TaskData t in m_cd.Tasks)
                {
                    CustomTaskData c = t as CustomTaskData;
                    if (c != null)
                    {
                        IPluginTaskWorker w = c.Plugin.GetWorker();
                        if (!w.OnStop())
                            Log(iba.Logging.Level.Exception, w.GetLastError(), String.Empty, t);
                    }
                }
            }
        }

        public void Signal()
        {
            m_waitEvent.Set();
        }

        public bool Join(int timeout)
        {
            Stop = true;
            if (m_sd.Started && (m_thread != null))
                return m_thread.Join(timeout);
            else
                return true;
        }

        public void Start()
        {
            if (m_sd.Started) return;
            m_stop = false;
            UpdateConfiguration();
            m_sd = new StatusData(m_cd);
            m_sd.ProcessedFiles = m_processedFiles = new Set<string>();
            m_sd.ReadFiles = m_toProcessFiles = new Set<string>();
            m_thread = new Thread(new ThreadStart(Run));
            //m_thread.SetApartmentState(ApartmentState.STA);
            m_thread.IsBackground = true;
            m_thread.Name = "workerthread for: " + m_cd.Name;
            m_thread.Start();
            m_sd.Started = true;
        }

        private ConfigurationData m_toUpdate = null;
        public ConfigurationData ConfigurationToUpdate
        {
            get { return m_toUpdate; }
            set { m_toUpdate = value; }
        }

        private bool UpdateConfiguration()
        {
            if (m_toUpdate != null)
            {
                if (m_toUpdate.NotificationData.TimeInterval < m_cd.NotificationData.TimeInterval 
                    && !m_toUpdate.NotificationData.NotifyImmediately)
                {
                    if (notifyTimer == null) notifyTimer = new System.Threading.Timer(OnNotifyTimerTick);
                    notifyTimer.Change(m_toUpdate.NotificationData.TimeInterval, TimeSpan.Zero);
                }
                if (m_toUpdate.RescanEnabled)
                {
                    if (rescanTimer == null)
                    {
                        rescanTimer = new System.Threading.Timer(OnRescanTimerTick);
                        rescanTimer.Change(m_toUpdate.RescanTimeInterval, TimeSpan.Zero);
                    }
                    else if (m_toUpdate.RescanTimeInterval < m_cd.RescanTimeInterval)
                    {
                        rescanTimer.Change(m_toUpdate.RescanTimeInterval, TimeSpan.Zero);
                    }
                }
                else
                {
                    if (rescanTimer != null)
                    {
                        rescanTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        rescanTimer.Dispose();
                        rescanTimer = null;
                    }
                }
                if (m_toUpdate.ReprocessErrorsTimeInterval < m_cd.ReprocessErrorsTimeInterval)
                {
                    if (reprocessErrorsTimer == null) reprocessErrorsTimer = new System.Threading.Timer(OnReprocessErrorsTimerTick);
                    reprocessErrorsTimer.Change(m_toUpdate.ReprocessErrorsTimeInterval, TimeSpan.Zero);
                }
                if (m_sd.Started)
                {
                    if (fswt != null)
                    {
                        fswt.Dispose();
                        fswt = null;
                    }
                    SharesHandler.Handler.ReleaseFromConfiguration(m_cd);
                }
                ConfigurationData oldConfigurationData = m_cd;
                m_cd = m_toUpdate.Clone_AlsoCopyGuids();
                object errorObject;
                SharesHandler.Handler.AddReferencesFromConfiguration(m_cd, out errorObject);
                if (errorObject != null)
                {
                    if (errorObject is ConfigurationData)
                    {
                        networkErrorOccured = true;
                        Log(iba.Logging.Level.Exception, String.Format(iba.Properties.Resources.UNCPathUnavailable, m_cd.DatDirectoryUNC));
                        tickCount = 0;
                    }
                    else
                    {
                        TaskDataUNC t = errorObject as TaskDataUNC;
                        if (t != null)
                        {
                            Log(iba.Logging.Level.Exception, String.Format(iba.Properties.Resources.UNCPathUnavailable, t.DestinationMapUNC));
                        }
                    }
                }
                else
                {
                    if (!Directory.Exists(m_cd.DatDirectoryUNC))
                    {
                        Log(Logging.Level.Exception, iba.Properties.Resources.logDatDirError);
                        SharesHandler.Handler.ReleaseFromConfiguration(m_cd);
                        Stop = true;
                    }
                    else
                    {
                        fswt = new FileSystemWatcher(m_cd.DatDirectoryUNC, "*.dat");
                        fswt.NotifyFilter = NotifyFilters.FileName;
                        fswt.IncludeSubdirectories = m_cd.SubDirs;
                        fswt.Created += new FileSystemEventHandler(OnNewDatFileOrRenameFile);
                        fswt.Renamed += new RenamedEventHandler(OnNewDatFileOrRenameFile);
                        fswt.Error += new ErrorEventHandler(OnFileSystemError);
                        fswt.EnableRaisingEvents = true;
                        networkErrorOccured = false;
                        tickCount = 0;
                    }
                }

                //also update statusdata
                lock (m_listUpdatingLock) //locked because statusdata might update it's 
                {
                    m_sd.CorrConfigurationData = m_cd;
                }

                lock (m_sd.DatFileStates)
                {
                    foreach (KeyValuePair<string, DatFileStatus> p in m_sd.DatFileStates)
                    {
                        DatFileStatus updatedStatus = new DatFileStatus();
                        foreach (TaskData task in m_cd.Tasks)
                        {
                            Nullable<DatFileStatus.State> stat = null;
                            foreach (KeyValuePair<TaskData, DatFileStatus.State> p2 in p.Value.States)
                            {
                                if (p2.Key.Guid == task.Guid)
                                    stat = p2.Value;
                            }
                            if (stat != null)
                                updatedStatus.States[task] = stat.Value;
                        }
                        p.Value.States.Clear();
                        foreach (KeyValuePair<TaskData, DatFileStatus.State> p2 in updatedStatus.States)
                        {
                            p.Value.States.Add(p2.Key, p2.Value);
                        }
                    }
                }

                if (m_notifier != null)
                {
                    m_notifier.Send();
                }
                m_notifier = new Notifier(m_cd);

                //lastly, execute plugin actions that need to happen in the case of an update
                foreach (TaskData t in m_cd.Tasks)
                {
                    TaskData oldtask = null;
                    CustomTaskData c_old = null;
                    try
                    {
                       oldtask = oldConfigurationData.Tasks[t.Index];
                       c_old = oldtask as CustomTaskData;
                    }
                    catch
                    {
                        oldtask = null;
                        c_old = null;
                    }
                    
                    CustomTaskData c_new = t as CustomTaskData;
                    if (c_old != null && c_new != null && c_old.Guid == c_new.Guid)
                    {
                        IPluginTaskWorker w = c_old.Plugin.GetWorker();
                        if (!w.OnApply(c_new.Plugin, m_cd))
                            Log(iba.Logging.Level.Exception, w.GetLastError(), String.Empty, t);
                        c_new.Plugin.SetWorker(w);
                    }
                    else if (c_new != null)
                    {
                        IPluginTaskWorker w = c_new.Plugin.GetWorker();
                        if (!w.OnApply(c_new.Plugin, m_cd))
                            Log(iba.Logging.Level.Exception, w.GetLastError(), String.Empty, t);
                    }
                }

                Log(Logging.Level.Info, iba.Properties.Resources.UpdateHappened);
                m_toUpdate = null;
                return true;
            }
            return false;
        }

        public ConfigurationWorker(ConfigurationData cd)
        {
            m_cd = cd.Clone_AlsoCopyGuids();
            m_sd = new StatusData(cd);
            m_stop = true;
            m_sd.ProcessedFiles = m_processedFiles = new Set<string>();
            m_sd.ReadFiles = m_toProcessFiles = new Set<string>();
            m_waitEvent = new AutoResetEvent(false);
            m_notifier = new Notifier(cd);
            m_candidateNewFiles = new List<string>();
            m_listUpdatingLock = new Object();
        }       
                
        Set<string> m_processedFiles;
        Set<string> m_toProcessFiles;
        List<string> m_candidateNewFiles;
        List<string> m_newFiles;

        private void Log(Logging.Level level, string message)
        {
            if (LogData.Data.Logger.IsOpen)
            {
                LogExtraData data = new LogExtraData(String.Empty, null, m_cd);
                LogData.Data.Logger.Log(level, message, (object)data);
            }
            if (level == Logging.Level.Exception)
            {
                if (message.Contains("The operation"))
                {
                    string debug = message;
                }
            }
        }

        private void Log(Logging.Level level, string message, string datfile)
        {
            if (LogData.Data.Logger.IsOpen)
            {
                LogExtraData data = new LogExtraData(datfile, null, m_cd);
                LogData.Data.Logger.Log(level, message, (object)data);
            }
            if (level == Logging.Level.Exception)
            {
                if (message.Contains("The operation"))
                {
                    string debug = message;
                }
            }
        }

        private void Log(Logging.Level level, string message, string datfile, TaskData task)
        {
            if (LogData.Data.Logger.IsOpen)
            {
                LogExtraData data = new LogExtraData(datfile, task, m_cd);
                LogData.Data.Logger.Log(level, message, (object)data);
            }
            if (level == Logging.Level.Exception)
            {
                if (message != null && message.Contains("The operation"))
                {
                    string debug = message;                
                }
            }
        }

        private IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer = null;

        private string m_previousRunExecutable;

        private System.Threading.Timer notifyTimer;
        private System.Threading.Timer reprocessErrorsTimer;
        private System.Threading.Timer rescanTimer;
        private System.Threading.Timer retryAccessTimer;
        private System.Threading.Timer testLicenseTimer;

        bool m_bTimersstopped;

        private FileSystemWatcher fswt = null;

        private void Run()
        {
            Log(Logging.Level.Info, iba.Properties.Resources.logConfigurationStarted);
            if (!Directory.Exists(m_cd.DatDirectoryUNC))
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logDatDirError);
                m_sd.Started = false;
                SharesHandler.Handler.ReleaseFromConfiguration(m_cd);
                Stop = true;
                return;
            }

            if (!TestLicensePlugins())
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logLicenseNoStart);
                m_sd.Started = false;
                Stop = true;
                return;
            }

            bool bInitialScan = true;
            bool bPostpone = false;
            int minutes = 5;
            try
            {
                Profiler.ProfileBool(true, "Settings", "DoInitialScan", ref bInitialScan, true);
                Profiler.ProfileBool(true, "Settings", "DoPostponeProcessing", ref bPostpone, false);
                Profiler.ProfileInt(true, "Settings", "PostponeMinutes", ref minutes, minutes);
            }
            catch
            {
            }

            //wait until computer is fully started (if selected so)
            if (bPostpone)
            {
                //Log(Logging.Level.Info,"postponing started, current time: " + DateTime.Now.ToString());
                //Log(Logging.Level.Info, "postponing started, current ticks: " + System.Environment.TickCount.ToString());
                while (((UInt32) System.Environment.TickCount)/60000 < minutes)
                {
                    if (m_stop)
                    {
                        m_sd.Started = false;
                        return;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(5.0));
                }
                //Log(Logging.Level.Info, "postponing stopped, current time: " + DateTime.Now.ToString());
            }

            try
            {
                Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
                if (bInitialScan)
                {
                    updateDatFileList(WhatToUpdate.ALL);
                }
                Thread.CurrentThread.Priority = ThreadPriority.Normal;
                if (m_stop)
                {
                    m_sd.Started = false;
                    return;
                }
                notifyTimer = null;
                try
                {
                    reprocessErrorsTimer = new System.Threading.Timer(new TimerCallback(OnReprocessErrorsTimerTick));
                    reprocessErrorsTimer.Change(m_cd.ReprocessErrorsTimeInterval, TimeSpan.Zero);
                    if (m_cd.RescanEnabled)
                    {
                        rescanTimer = new System.Threading.Timer(new TimerCallback(OnRescanTimerTick));
                        rescanTimer.Change(m_cd.RescanTimeInterval, TimeSpan.Zero);
                    }
                    else rescanTimer = null;
                    
                    retryAccessTimer = new System.Threading.Timer(new TimerCallback(OnAddNewDatFileTimerTick));
                    retryAccessTimer.Change(1000, Timeout.Infinite); //hard coded, each second see if no new files are available
                    m_bTimersstopped = false;

                    if (!m_cd.NotificationData.NotifyImmediately)
                    {
                        notifyTimer = new System.Threading.Timer(new TimerCallback(OnNotifyTimerTick));
                        notifyTimer.Change(m_cd.NotificationData.TimeInterval, m_cd.NotificationData.TimeInterval);
                    }

                    //do initializations of custom tasks
                    foreach(TaskData t in m_cd.Tasks)
                    {
                        CustomTaskData c = t as CustomTaskData;
                        if (c != null)
                        {
                            IPluginTaskWorker w = c.Plugin.GetWorker();
                            if (!w.OnStart())
                                Log(iba.Logging.Level.Exception, w.GetLastError(), String.Empty, t);
                        }
                    }

                    StartIbaAnalyzer();
                    while (!m_stop)
                    {
                        while (true)
                        {
                            string file = null;
                            lock (m_toProcessFiles)
                            {
                                if (m_toProcessFiles.Count > 0)
                                {
                                    DateTime mintime = DateTime.MaxValue;
                                    foreach (string fileC in m_toProcessFiles)
                                    {
                                        try
                                        {
                                            if (File.Exists(fileC))
                                            {
                                                FileInfo f = new FileInfo(fileC);
                                                DateTime n = f.LastWriteTime;
                                                if (n < mintime)
                                                {
                                                    file = fileC;
                                                    mintime = n;
                                                }
                                            }
                                        }
                                        catch //if network disconnection should happen
                                        {
                                            
                                        }
                                    }
                                }
                                else break;
                            }
                            if (file == null) //no existing file
                            {
                                lock (m_toProcessFiles)
                                {
                                    m_toProcessFiles.Clear();
                                }
                                break;
                            }
                            ProcessDatfile(file);
                            if (m_stop) break;
                            lock (m_toProcessFiles)
                            {
                                int index = m_toProcessFiles.IndexOf(file);
                                if (index >= 0)
                                    m_toProcessFiles.RemoveAt(index);
                            }
                            UpdateConfiguration();
                            if (m_previousRunExecutable != m_cd.IbaAnalyzerExe)
                            {
                                StopIbaAnalyzer();
                                StartIbaAnalyzer();
                            }
                        }
                        //stop the com object
                        m_waitEvent.WaitOne();
                        UpdateConfiguration();
                    }
                    StopIbaAnalyzer();
                }
                finally
                {
                    m_bTimersstopped = true;
                    if (notifyTimer != null)
                    {
                        notifyTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        notifyTimer.Dispose();
                        m_notifier.Send(); //send one last time
                    }
                    if (rescanTimer != null)
                    {
                        rescanTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        rescanTimer.Dispose();
                        rescanTimer = null;
                    }
                    if (reprocessErrorsTimer != null)
                    {
                        reprocessErrorsTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        reprocessErrorsTimer.Dispose();
                        reprocessErrorsTimer = null;
                    }
                    if (retryAccessTimer != null)
                    {
                        retryAccessTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        retryAccessTimer.Dispose();
                        retryAccessTimer = null;
                    }
                    if (testLicenseTimer != null)
                    {
                        testLicenseTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        testLicenseTimer.Dispose();
                        testLicenseTimer = null;
                    }
                    Debug.Assert(m_ibaAnalyzer == null, "ibaAnalyzer should have been closed");
                }
            }
            finally
            {
                if (fswt != null) fswt.Dispose();
                fswt = null;
            }

            m_sd.Started = false;
            SharesHandler.Handler.ReleaseFromConfiguration(m_cd);
            Log(Logging.Level.Info, iba.Properties.Resources.logConfigurationStopped);
        }


        private bool networkErrorOccured = false;
        int tickCount = 0;

        void OnFileSystemError(object sender, ErrorEventArgs e)
        {
            networkErrorOccured = true;
            fswt.Dispose();
            fswt = null;
            Log(iba.Logging.Level.Exception, String.Format(iba.Properties.Resources.ConnectionLostFrom, m_cd.DatDirectoryUNC,e.GetException().Message));
        }

        private bool TestLicensePlugins()
        {
            CDongleInfo info = null;
            bool ok = true;
            foreach (TaskData task in m_cd.Tasks)
            {
                CustomTaskData cust = task as CustomTaskData;
                if (cust != null)
                {
                    if (info == null) info = CDongleInfo.ReadDongle();
                    if (!info.PluginsLicensed() || !info.IsPluginLicensed(cust.Plugin.DongleBitPos))
                    {
                        ok = false;
                        Log(Logging.Level.Exception, String.Format(iba.Properties.Resources.logTaskNotLicensed,task.Name));
                    }
                }
            }
            if (info != null && ok) // start timer
            {
                if (testLicenseTimer == null) testLicenseTimer = new System.Threading.Timer(OnTestLicenseTimerTick);
                if (!m_bTimersstopped && !m_stop)
                    testLicenseTimer.Change(TimeSpan.FromMinutes(2.0), TimeSpan.Zero);
            }
            return ok;
        }

        private void StartIbaAnalyzer()
        {
            //register this
            if (m_previousRunExecutable != m_cd.IbaAnalyzerExe)
            {
                try
                {
                    if (!File.Exists(m_cd.IbaAnalyzerExe))
                    {
                        Log(Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerExecutableDoesNotExist + m_cd.IbaAnalyzerExe);
                        m_sd.Started = false;
                        Stop = true;
                        return;
                    }
                    string version = FileVersionInfo.GetVersionInfo(m_cd.IbaAnalyzerExe).FileVersion;
                    if (version.CompareTo("5.0") < 0)
                    {
                        Log(Logging.Level.Exception, iba.Properties.Resources.logFileVersionToLow);
                        m_sd.Started = false;
                        Stop = true;
                        return;
                    };
                    Process ibaProc = new Process();
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_cd.IbaAnalyzerExe;
                    ibaProc.StartInfo.Arguments = "/regserver";
                    ibaProc.Start();
                    ibaProc.WaitForExit(10000);
                    m_previousRunExecutable = m_cd.IbaAnalyzerExe;
                }
                catch (Exception ex)
                {
                    Log(Logging.Level.Exception, ex.Message);
                    m_sd.Started = false;
                    Stop = true;
                    return;
                }
            }
            //start the com object
            try
            {
                m_ibaAnalyzer = new IbaAnalyzer.IbaAnalysisClass();
            }
            catch (Exception)
            {
                try //try again by first registering
                {
                    Process ibaProc = new Process();
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_cd.IbaAnalyzerExe;
                    ibaProc.StartInfo.Arguments = "/regserver";
                    ibaProc.Start();
                    ibaProc.WaitForExit(10000);

                    m_ibaAnalyzer = new IbaAnalyzer.IbaAnalysisClass();
                }
                catch (Exception ex2)
                {
                    Log(Logging.Level.Exception, ex2.Message);
                    m_sd.Started = false;
                    Stop = true;
                    return;
                }
            }
        }

        private void StopIbaAnalyzer()
        {
            StopIbaAnalyzer(true);
        }

        private void StopIbaAnalyzer(bool stop)
        {
            try
            {
                if (m_ibaAnalyzer == null)
                    return;
                System.Runtime.InteropServices.Marshal.ReleaseComObject(m_ibaAnalyzer);
                m_ibaAnalyzer = null;
            }
            catch (Exception ex)
            {
                Log(Logging.Level.Exception, ex.Message);
                if (stop)
                {
                    m_sd.Started = false;
                    Stop = stop;
                }
                return;
            }
        }

        private string IbaAnalyzerErrorMessage()
        {
            try
            {
                if (m_ibaAnalyzer != null)
                    return m_ibaAnalyzer.GetLastError();
                else
                {
                    StopIbaAnalyzer(false);
                    StartIbaAnalyzer();
                    return iba.Properties.Resources.IbaAnalyzerUndeterminedError;
                }

            }
            catch (Exception ex)
            {
                Log(Logging.Level.Exception, ex.Message);
                StopIbaAnalyzer(false);
                StartIbaAnalyzer();
                return iba.Properties.Resources.IbaAnalyzerUndeterminedError;
            }
        }

        private void OnNewDatFileOrRenameFile(object sender, FileSystemEventArgs args)
        {
            string filename = args.FullPath;
            lock (m_candidateNewFiles)
            {
                if (!m_candidateNewFiles.Contains(filename))
                {
                    m_candidateNewFiles.Add(filename);
                    m_skipAddNewDatFileTimerTick = true; //wait one tick before checking access, to let PDA do stuff
                }
            }
        }

        private object m_listUpdatingLock; //makes the timer routines mutually exclusive
        private bool m_skipAddNewDatFileTimerTick;

        private void OnAddNewDatFileTimerTick(object ignoreMe)
        {
            if (m_bTimersstopped || m_stop) return;
            retryAccessTimer.Change(Timeout.Infinite, Timeout.Infinite);
            bool changed = false;
            List<string> toRemove = new List<string>();
            m_newFiles = new Set<string>();
            lock (m_listUpdatingLock)
            {
                lock (m_candidateNewFiles)
                {
                    if (m_skipAddNewDatFileTimerTick)
                    {
                        m_skipAddNewDatFileTimerTick = false;
                        retryAccessTimer.Change(1000, Timeout.Infinite);
                        return;
                    }

                    foreach (string filename in m_candidateNewFiles)
                    {
                        FileStream fs = null;
                        try
                        {
                            fs = new FileStream(filename, FileMode.Open, FileAccess.Write, FileShare.None);
                            fs.Close();
                            fs.Dispose();
                            bool doit = false;
                            lock (m_processedFiles)
                            {
                                lock (m_toProcessFiles)
                                {
                                    doit = !m_toProcessFiles.Contains(filename) && !m_processedFiles.Contains(filename);
                                    if (doit)
                                        m_newFiles.Add(filename);
                                }
                            }
                            toRemove.Add(filename);
                            changed = changed || doit;
                        }
                        catch //no access, do not remove from the list yet (unless the file doesnt exist), make available again when acces to it is restored
                        {
                            try
                            {
                                if (!File.Exists(filename))
                                    toRemove.Add(filename);
                            }
                            catch
                            {
                            }
                        }
                    }
                    foreach (string filename in toRemove)
                        m_candidateNewFiles.Remove(filename);
                }
                if (changed)
                {
                    updateDatFileList(WhatToUpdate.NEW);
                    m_waitEvent.Set();
                }
            }

            if (!changed && networkErrorOccured) tickCount++;

            if (tickCount >= 20) //retry restoring dataaccess every minute
            {
                tickCount = 0;
                if (SharesHandler.Handler.TryReconnect(m_cd.DatDirectoryUNC, m_cd.Username, m_cd.Password))
                {
                    if (fswt != null)
                        fswt.Dispose();
                    fswt = new FileSystemWatcher(m_cd.DatDirectoryUNC, "*.dat");
                    fswt.NotifyFilter = NotifyFilters.FileName;
                    fswt.IncludeSubdirectories = m_cd.SubDirs;
                    fswt.Created += new FileSystemEventHandler(OnNewDatFileOrRenameFile);
                    fswt.Error += new ErrorEventHandler(OnFileSystemError);
                    fswt.Renamed += new RenamedEventHandler(OnNewDatFileOrRenameFile);
                    fswt.EnableRaisingEvents = true;
                    networkErrorOccured = false;
                    Log(iba.Logging.Level.Info, String.Format(iba.Properties.Resources.ConnectionRestoredTo, m_cd.DatDirectoryUNC));
                }
            }
            if (!m_bTimersstopped && !m_stop)
                retryAccessTimer.Change(1000, Timeout.Infinite);
        }

        private void OnRescanTimerTick(object ignoreMe)
        {
            if (m_bTimersstopped || m_stop || rescanTimer==null) return;
            rescanTimer.Change(Timeout.Infinite, Timeout.Infinite);
            lock (m_listUpdatingLock)
            {
                if (!networkErrorOccured) updateDatFileList(WhatToUpdate.ALL);
                m_waitEvent.Set();
            }
            if (!m_bTimersstopped && !m_stop)
                rescanTimer.Change(m_cd.RescanTimeInterval, TimeSpan.Zero);
        }

        private void OnReprocessErrorsTimerTick(object ignoreMe)
        {
            if (networkErrorOccured) return; //wait until problem is fixed
            if (m_bTimersstopped || m_stop) return;
            reprocessErrorsTimer.Change(Timeout.Infinite, Timeout.Infinite);
            lock (m_listUpdatingLock)
            {
                if (!networkErrorOccured) updateDatFileList(WhatToUpdate.ERRORS);
                m_waitEvent.Set();
            }
            if (!m_bTimersstopped && !m_stop)
                reprocessErrorsTimer.Change(m_cd.ReprocessErrorsTimeInterval, TimeSpan.Zero);
        }

        private void OnNotifyTimerTick(object ignoreMe)
        {
            if (m_bTimersstopped || m_stop) return;
            notifyTimer.Change(Timeout.Infinite, Timeout.Infinite);
            m_notifier.Send();
            if (!m_bTimersstopped && !m_stop)
                notifyTimer.Change(m_cd.NotificationData.TimeInterval, TimeSpan.Zero);
        }

        private void OnTestLicenseTimerTick(object ignoreMe)
        {
            if (m_bTimersstopped || m_stop) return;
            testLicenseTimer.Change(Timeout.Infinite, Timeout.Infinite);
            if (!TestLicensePlugins())
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logLicenseStopped);
                Stop = true;
            }
        }

        private enum WhatToUpdate { ALL, NEW, ERRORS };
        private void updateDatFileList(WhatToUpdate what)
        {
            if (what != WhatToUpdate.NEW) Log(Logging.Level.Info, iba.Properties.Resources.logCheckingForNewDatFiles);
            string datDir = m_cd.DatDirectoryUNC;
            FileInfo[] fileInfos = null;
            if (Directory.Exists(datDir))
            {
                m_sd.UpdatingFileList = true;
                lock (m_processedFiles)
                {
                    if (what != WhatToUpdate.ALL)
                    {
                        int count = m_processedFiles.Count;
                        fileInfos = new FileInfo[count];

                        if (what == WhatToUpdate.ERRORS)
                        {
                            for (int i = 0; i < count; i++)
                            {
                                lock (m_sd.DatFileStates)
                                {
                                    if (File.Exists(m_processedFiles[i])
                                        && (m_sd.DatFileStates.ContainsKey(m_processedFiles[i])))
                                    {
                                        fileInfos[i] = new FileInfo(m_processedFiles[i]);
                                    }
                                    else
                                        fileInfos[i] = null;
                                }
                            }

                        }
                        else //NEW
                        {
                            count = m_newFiles.Count;
                            fileInfos = new FileInfo[count];
                            for (int i = 0; i < count; i++)
                            {
                                string filename = m_newFiles[i];
                                if (File.Exists(filename)
                                    && (!m_sd.DatFileStates.ContainsKey(filename)))
                                {
                                    fileInfos[i] = new FileInfo(m_newFiles[i]);
                                }
                                else
                                    fileInfos[i] = null;
                            }
                            m_newFiles.Clear();
                        }

                        for (int i = 0; i < count; i++)
                        {
                            if (fileInfos[i] != null && m_processedFiles.Contains(fileInfos[i].FullName))
                                m_processedFiles.Remove(fileInfos[i].FullName);
                        }

                        count = m_processedFiles.Count;

                        for (int i = 0; i < count; i++)
                        {
                            string filename = m_processedFiles[i];
                            lock (m_sd.DatFileStates)
                            {
                                bool allclear = true;
                                if (m_sd.DatFileStates.ContainsKey(filename))
                                    foreach (TaskData dat in m_cd.Tasks)
                                    {
                                        if (dat.Enabled)
                                            allclear = allclear && m_sd.DatFileStates[filename].States.ContainsKey(dat)
                                                && !shouldTaskBeDone(dat, filename);
                                        if (!allclear) break;
                                    }
                                if (allclear)
                                {
                                    m_processedFiles.Remove(filename);
                                    lock (m_sd.DatFileStates)
                                    {
                                        if (m_sd.DatFileStates.ContainsKey(filename))
                                            m_sd.DatFileStates.Remove(filename);
                                    }
                                    i--;
                                    count--;
                                }
                            }
                        }
                    }
                    else
                    {
                        m_processedFiles.Clear();
                        m_sd.DatFileStates.Clear();
                    }
                }
                lock (m_toProcessFiles)
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(datDir);
                    if (what == WhatToUpdate.ALL)
                    {
                        try
                        {
                            fileInfos = dirInfo.GetFiles("*.dat", m_cd.SubDirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                        }
                        catch
                        {
                            m_sd.UpdatingFileList = false;
                            Log(Logging.Level.Exception, iba.Properties.Resources.logDatDirError);
                            return;
                        }
                        m_toProcessFiles.Clear();
                    }
                    bool bPermanentErrorFilesChanged = false;
                    foreach (FileInfo fi in fileInfos)
                    {
                        if (m_stop) break;
                        if (fi == null) continue;
                        string filename = fi.FullName;
                        switch (ProcessDatfileReadyness(filename))
                        {
                            case DatFileStatus.State.NOT_STARTED:
                                if (!m_toProcessFiles.Contains(filename))
                                    m_toProcessFiles.Add(filename);
                                lock (m_sd.DatFileStates)
                                {
                                    m_sd.DatFileStates[filename] = new DatFileStatus();
                                }
                                break;
                            case DatFileStatus.State.COMPLETED_SUCCESFULY:
                                lock (m_sd.DatFileStates)
                                {
                                    if (m_sd.DatFileStates.ContainsKey(filename))
                                        m_sd.DatFileStates.Remove(filename);
                                }
                                break; //do not add
                            case DatFileStatus.State.MEMORY_EXCEEDED:
                            case DatFileStatus.State.TIMED_OUT:
                            case DatFileStatus.State.COMPLETED_FAILURE:
                                if (!m_toProcessFiles.Contains(filename))
                                    m_toProcessFiles.Add(filename);
                                break;
                            case DatFileStatus.State.NO_ACCESS:
                                lock (m_processedFiles)
                                {
                                    if (!m_processedFiles.Contains(filename))
                                        m_processedFiles.Add(filename);
                                }
                                DatFileStatus status = new DatFileStatus();
                                lock (m_sd.DatFileStates)
                                {
                                    status.TimesTried = m_sd.DatFileStates[filename].TimesTried;
                                    foreach (TaskData dat in m_cd.Tasks)
                                    {
                                        if (dat.Enabled) status.States[dat] = DatFileStatus.State.NO_ACCESS;
                                    }
                                    m_sd.DatFileStates[filename] = status;
                                }
                                break;
                            case DatFileStatus.State.RUNNING:
                                break;
                            case DatFileStatus.State.TRIED_TOO_MANY_TIMES:
                                lock (m_sd.PermanentErrorFiles)
                                {
                                    m_sd.PermanentErrorFiles.Add(filename);
                                    bPermanentErrorFilesChanged = true;
                                }
                                //DatFileStatus status2 = new DatFileStatus();
                                //lock (m_sd.DatFileStates)
                                //{
                                //    status2.TimesTried = m_sd.DatFileStates[filename].TimesTried;
                                //    foreach (TaskData dat in m_cd.Tasks)
                                //    {
                                //        if (dat.Enabled) status2.States[dat] = DatFileStatus.State.TRIED_TOO_MANY_TIMES;
                                //    }
                                //    m_sd.DatFileStates[filename] = status2;
                                //}
                                break;
                        }
                    }
                    m_sd.Changed = true;
                    m_sd.PermanentErrorFilesChanged = bPermanentErrorFilesChanged;
                }
                m_sd.TakeCopyOfFileList();
                m_sd.UpdatingFileList = false;
            }
            else
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logDatDirError);
            }
        }

        private DatFileStatus.State ProcessDatfileReadyness(string filename)
        {
            try
            {
                IbaFileUpdater ibaDatFile = new IbaFileClass();
                Nullable<DateTime> time = null;
                try
                {
                    try
                    {
                        time = File.GetLastWriteTime(filename);
                    }
                    catch
                    {
                        time = null;
                    }
                    if (time != null) ibaDatFile.OpenForUpdate(filename);
                }
                catch (FileLoadException) //no access
                {
                    //reason 1, because of RUNNING
                    lock (m_sd.DatFileStates)
                    {
                        foreach (DatFileStatus.State stat in m_sd.DatFileStates[filename].States.Values)
                            if (stat == DatFileStatus.State.RUNNING)
                                return DatFileStatus.State.RUNNING;
                    }
                    try
                    {
                        FileAttributes at = File.GetAttributes(filename);
                        if (at != FileAttributes.ReadOnly)
                            Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                        else
                            Log(Logging.Level.Exception, iba.Properties.Resources.Noaccess2, filename);
                    }
                    catch
                    {
                        Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                    }
                    return DatFileStatus.State.NO_ACCESS; //no acces, try again next time
                }
                catch (Exception ex)
                {
                    Log(Logging.Level.Exception, ex.Message, filename);
                    return DatFileStatus.State.NO_ACCESS; //no acces, try again next time
                }

                try
                {
                    string status = "";
                    try
                    {
                        status = ibaDatFile.QueryInfoByName("$DATCOOR_status");
                    }
                    catch //old way
                    {
                        status = ibaDatFile.QueryInfoByName("status");
                    }
                    if (status == "processed")
                    {
                        ibaDatFile.Close();
                        try{if (time != null) File.SetLastWriteTime(filename, time.Value);}catch{}
                        return DatFileStatus.State.COMPLETED_SUCCESFULY;
                    }
                    else if (status == "processingfailed")
                    {
                        int timesProcessed = 1;
                        try
                        {
                            string timesString = ibaDatFile.QueryInfoByName("$DATCOOR_times_tried");
                            timesProcessed = int.Parse(timesString);
                        }
                        catch
                        {
                        }
                        if (m_cd.LimitTimesTried && timesProcessed >= m_cd.NrTryTimes)
                        {
                            ibaDatFile.Close();
                            try{if (time != null) File.SetLastWriteTime(filename, time.Value);}catch{}
                            Log(Logging.Level.Warning, iba.Properties.Resources.TriedToManyTimes, filename);
                            lock (m_sd.DatFileStates)
                            {
                                m_sd.DatFileStates[filename].TimesTried = timesProcessed;
                            }
                            return DatFileStatus.State.TRIED_TOO_MANY_TIMES;
                        }
                        List<string> guids = null;
                        string guidstring = null;
                        //get guids
                        try
                        {
                            try
                            {
                                guidstring = ibaDatFile.QueryInfoByName("$DATCOOR_TasksDone");
                            }
                            catch //old way
                            {
                                guidstring = ibaDatFile.QueryInfoByName("TasksDone");
                            }
                        }
                        catch (ArgumentException ex)
                        {
                            string message = ex.Message;
                        }
                        catch (Exception) //general exception
                        {
                            Log(Logging.Level.Exception, iba.Properties.Resources.ReadStatusError, filename);
                        }
                        guidstring.Trim(new char[] { ';' });
                        guids = new List<string>(guidstring.Split(new char[] { ';' }));
                        guids.Sort();
                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].TimesTried = timesProcessed;
                            foreach (TaskData task in m_cd.Tasks)
                            {
                                if (guids.BinarySearch(task.Guid.ToString()) > 0)
                                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                            }
                        }
                        ibaDatFile.Close();
                        try{if (time != null) File.SetLastWriteTime(filename, time.Value);}catch{}
                        return DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    else if (status == "restart" || status == "readyToProcess")
                    {
                        ibaDatFile.Close();
                        return DatFileStatus.State.NOT_STARTED;
                    }
                    else
                    {
                        ibaDatFile.Close();
                        return DatFileStatus.State.COMPLETED_FAILURE;
                    }
                }
                catch
                {
                    ibaDatFile.Close();
                    ibaDatFile.OpenForUpdate(filename);

                    String frames = null;
                    try
                    {
                        ibaDatFile.QueryInfoByName("frames");
                    }
                    catch
                    {
                    }
                    if (frames != null && frames == "1000000000")
                    {
                        Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess3, filename);
                        try { if (time != null) File.SetLastWriteTime(filename, time.Value); }catch {} 
                        return DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    ibaDatFile.WriteInfoField("$DATCOOR_status", "readyToProcess");
                    ibaDatFile.Close();
                    try { if (time != null) File.SetLastWriteTime(filename, time.Value); }catch {}
                    return DatFileStatus.State.NOT_STARTED;
                }
            }
            catch //general exception that may have happened
            {
                Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                return DatFileStatus.State.COMPLETED_FAILURE;
            }
        }

        private AutoResetEvent m_waitEvent;

        private bool shouldTaskBeDone(TaskData task, string filename)
        {
            lock (m_sd.DatFileStates)
            {
                if (task.Enabled == false)
                    return false;
                else
                {
                    bool completed = m_sd.DatFileStates[filename].States[task] == DatFileStatus.State.COMPLETED_SUCCESFULY ||
                            m_sd.DatFileStates[filename].States[task] == DatFileStatus.State.COMPLETED_TRUE ||
                                m_sd.DatFileStates[filename].States[task] == DatFileStatus.State.COMPLETED_FALSE;
                    if (task.WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE)
                        return !completed;
                    else if (task.WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES)
                        return !completed && 
                            (m_sd.DatFileStates[filename].States[m_cd.Tasks[task.Index - 1]] == DatFileStatus.State.COMPLETED_SUCCESFULY
                                || m_sd.DatFileStates[filename].States[m_cd.Tasks[task.Index - 1]] == DatFileStatus.State.COMPLETED_TRUE);
                    else if (task.WhenToExecute == TaskData.WhenToDo.AFTER_FAILURE)
                        return DatFileStatus.IsError(m_sd.DatFileStates[filename].States[m_cd.Tasks[task.Index - 1]])
                            || (m_sd.DatFileStates[filename].States[m_cd.Tasks[task.Index - 1]] == DatFileStatus.State.COMPLETED_FALSE
                                && !completed);
                    else if (task.WhenToExecute == TaskData.WhenToDo.AFTER_1st_FAILURE)
                        return !completed
                            && (DatFileStatus.IsError(m_sd.DatFileStates[filename].States[m_cd.Tasks[task.Index - 1]])
                            || m_sd.DatFileStates[filename].States[m_cd.Tasks[task.Index - 1]] == DatFileStatus.State.COMPLETED_FALSE);
                }
                return false;
            }
        }

        private void ProcessDatfile(string DatFile)
        {
            lock(m_listUpdatingLock)
            {
                lock (m_processedFiles)
                {
                    if( ! m_processedFiles.Contains(DatFile))
                    m_processedFiles.Add(DatFile);
                }
                bool completeSucces = true;
                try
                {
                    FileStream fs = new FileStream(DatFile, FileMode.Open, FileAccess.Write, FileShare.None);
                    fs.Close();
                    fs.Dispose();
                    m_ibaAnalyzer.OpenDataFile(0,DatFile);
                }
                catch (Exception ex)
                {
                    Log(Logging.Level.Exception,ex.Message);
                    try
                    {
                        m_ibaAnalyzer.CloseDataFiles();
                    }
                    catch
                    {
                        Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, DatFile);
                        StopIbaAnalyzer(false);
                        StartIbaAnalyzer();
                    }
                    return;
                }
                foreach (TaskData task in m_cd.Tasks)
                {
                    if (!shouldTaskBeDone(task, DatFile))
                        continue;
                    bool failedOnce = false;
                    lock (m_sd.DatFileStates)
                    {
                        failedOnce = m_sd.DatFileStates.ContainsKey(DatFile)
                            && m_sd.DatFileStates[DatFile].States.ContainsKey(task)
                            && DatFileStatus.IsError(m_sd.DatFileStates[DatFile].States[task])
                            && m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.NO_ACCESS; //all errors except no access
                    }
                    if (task is ReportData)
                    {
                        Report(DatFile,task as ReportData);
                    }
                    else if (task is ExtractData)
                    {
                        Extract(DatFile,task as ExtractData);
                    }
                    else if (task is BatchFileData)
                    {
                        Batchfile(DatFile,task as BatchFileData);
                    }
                    else if (task is IfTaskData)
                    {
                        IfTask(DatFile, task as IfTaskData);
                    }
                    else if (task is CopyMoveTaskData)
                    {
                        CopyMoveTaskData dat = task as CopyMoveTaskData;
                        if (dat.RemoveSource)
                        {
                            try
                            {
                                m_ibaAnalyzer.CloseDataFiles();
                            }
                            catch
                            {
                                Log(iba.Logging.Level.Exception,iba.Properties.Resources.IbaAnalyzerUndeterminedError,DatFile,task);
                                StopIbaAnalyzer(false);
                                StartIbaAnalyzer();
                            }
                            CopyDatFile(DatFile, dat);
                            if (m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_SUCCESFULY && task.Index != m_cd.Tasks.Count - 1) //was not last task
                            {
                                Log(Logging.Level.Warning, iba.Properties.Resources.logNextTasksIgnored, DatFile, task);
                                m_sd.Changed = true;
                                return;
                            }
                            else if (m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.COMPLETED_SUCCESFULY)
                            {
                                try
                                {
                                    m_ibaAnalyzer.OpenDataFile(0, DatFile);
                                }
                                catch (Exception ex)
                                {
                                    Log(Logging.Level.Exception, ex.Message);
                                    try
                                    {
                                        m_ibaAnalyzer.CloseDataFiles();
                                    }
                                    catch
                                    {
                                        Log(iba.Logging.Level.Exception,iba.Properties.Resources.IbaAnalyzerUndeterminedError,DatFile,task);
                                        StopIbaAnalyzer(false);
                                        StartIbaAnalyzer();
                                    }
                                    return;
                                }
                            }
                            else
                                return;
                        }
                        else
                            CopyDatFile(DatFile, dat);
                    }
                    else if (task is CustomTaskData)
                    {
                        DoCustomTask(DatFile, task as CustomTaskData);
                    }
                    lock (m_sd.DatFileStates)
                    {
                        if (m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.COMPLETED_SUCCESFULY
                            && m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.COMPLETED_TRUE
                            && m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.COMPLETED_FALSE)
                            completeSucces = false;
                        if ((m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_SUCCESFULY ||
                            m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_TRUE ||
                            m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_FALSE)
                            && (task.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES || task.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE))
                        {
                            m_notifier.AddSuccess(task, DatFile);
                            if (m_cd.NotificationData.NotifyImmediately)
                                m_notifier.Send();
                        }
                        else if ((DatFileStatus.IsError(m_sd.DatFileStates[DatFile].States[task])
                                && m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.NO_ACCESS
                                && m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.TRIED_TOO_MANY_TIMES) 
                                && (task.WhenToNotify == TaskData.WhenToDo.AFTER_FAILURE || task.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE
                            || (task.WhenToNotify == TaskData.WhenToDo.AFTER_1st_FAILURE && !failedOnce)))
                        {
                            m_notifier.AddFailure(task, DatFile);
                            if (m_cd.NotificationData.NotifyImmediately)
                                m_notifier.Send();
                        }
                    }
                    if (m_stop)
                    {
                        if (task.Index != m_cd.Tasks.Count-1) completeSucces = false;
                        break;
                    }
                }

                try
                {
                    m_ibaAnalyzer.CloseDataFiles();
                }
                catch
                {
                    Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, DatFile);
                    StopIbaAnalyzer(false);
                    StartIbaAnalyzer();
                }
                
                IbaFileUpdater ibaDatFile = new IbaFileClass();
                DateTime time = File.GetLastWriteTime(DatFile);
                try
                {
                    ibaDatFile.OpenForUpdate(DatFile);
                }
                catch //happens when timed out and proc has not released its resources yet
                {
                    m_sd.Changed = true;
                    return;
                }
                m_sd.DatFileStates[DatFile].TimesTried++;
                if (completeSucces)
                    ibaDatFile.WriteInfoField("$DATCOOR_status", "processed");
                else
                {
                    ibaDatFile.WriteInfoField("$DATCOOR_status", "processingfailed");
                    //write GUIDs of those that were succesfull
                    lock (m_sd.DatFileStates)
                    {
                        string guids = "";
                        foreach (KeyValuePair<TaskData, DatFileStatus.State> stat in m_sd.DatFileStates[DatFile].States)
                            if (stat.Value == DatFileStatus.State.COMPLETED_SUCCESFULY)
                                guids += stat.Key.Guid.ToString() + ";";
                        ibaDatFile.WriteInfoField("$DATCOOR_TasksDone", guids);
                    }
                }
                lock (m_sd.DatFileStates)
                {
                    ibaDatFile.WriteInfoField("$DATCOOR_times_tried", m_sd.DatFileStates[DatFile].TimesTried.ToString());
                }
                ibaDatFile.Close();
                File.SetLastWriteTime(DatFile, time);
                m_sd.Changed = true;
            }
        }

        public void MovePermanentFileErrorListToProcessedList(List<string> files)
        {
            lock (m_listUpdatingLock)
            {
                m_sd.MovePermanentFileErrorListToProcessedList(files);
            }
        }
        
        private void DoCustomTask(string DatFile, CustomTaskData task)
        {
            try
            {
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[DatFile].States[task] = DatFileStatus.State.RUNNING;
                }
                bool succes = task.Plugin.GetWorker().ExecuteTask(DatFile);
                if (succes)
                {
                    //code on succes
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[DatFile].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                    }
                    string message = string.Format(iba.Properties.Resources.logTaskSuccess, task.Plugin.NameInfo);
                    Log(Logging.Level.Info, message, DatFile, task);
                }
                else
                {
                    //code on error
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[DatFile].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    string error = task.Plugin.GetWorker().GetLastError();
                    if (error == null) error = "";
                    Log(Logging.Level.Exception, task.Plugin.GetWorker().GetLastError(), DatFile, task);
                }
            }
            catch (Exception ex)
            {
                Log(Logging.Level.Exception, ex.Message, DatFile, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[DatFile].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
        }

        private void Extract(string filename, ExtractData task)
        {
            try
            {
                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, task.MonitorData))
                {
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.RUNNING;
                    }
                    mon.Execute(delegate() { m_ibaAnalyzer.OpenAnalysis(task.AnalysisFile); });
                    Log(Logging.Level.Info, iba.Properties.Resources.logExtractStarted, filename, task);
                    if (task.ExtractToFile)
                    {
                        string outFile = GetExtractFileName(filename, task);
                        if (outFile == null) return;
                        mon.Execute(delegate() { m_ibaAnalyzer.Extract(1, outFile); });
                    }
                    else
                    {
                        mon.Execute(delegate() { m_ibaAnalyzer.Extract(0, String.Empty); });
                    }
                    //code on succes
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                    }
                    Log(Logging.Level.Info, iba.Properties.Resources.logExtractSuccess, filename, task);
                }
            }
            catch (IbaAnalyzerExceedingTimeLimitException te)
            {
                Log(Logging.Level.Exception, te.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.TIMED_OUT;
                }
                StopIbaAnalyzer();
                StartIbaAnalyzer();
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                Log(Logging.Level.Exception, me.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.MEMORY_EXCEEDED;
                }
                StopIbaAnalyzer();
                StartIbaAnalyzer();
            }
            catch
            {
                Log(Logging.Level.Exception, IbaAnalyzerErrorMessage(), filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
            finally
            {
                if (m_ibaAnalyzer != null)
                {
                    try
                    {
                        m_ibaAnalyzer.CloseAnalysis();
                    }
                    catch
                    {
                        Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, filename, task);
                        StopIbaAnalyzer(false);
                        StartIbaAnalyzer();
                    }
                }
            }
        }

        private string GetExtractFileName(string filename, ExtractData task)
        {
            string actualFileName = Path.GetFileNameWithoutExtension(filename);
            string dir = task.DestinationMap;
            if (String.IsNullOrEmpty(dir))
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logNoOutputPathSpecified, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return null;
            }

            if (!Path.IsPathRooted(dir))
            {  //get Absolute path relative to dir
                dir = Path.Combine(m_cd.DatDirectoryUNC, dir);
            }
            else dir = task.DestinationMapUNC;

            if (dir == m_cd.DatDirectoryUNC)
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logOutputIsInput, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return null;
            }


            if (m_cd.SubDirs && task.Subfolder == ExtractData.SubfolderChoiceB.SAME)
            {   //concatenate subfolder corresponding to dat subfolder
                string s2 = Path.GetFullPath(m_cd.DatDirectory);
                string s1 = Path.GetFullPath(filename);
                string s0 = s1.Remove(0, s2.Length + 1);
                dir = Path.GetDirectoryName(Path.Combine(dir, s0));
            }
            if (task.Subfolder != ExtractData.SubfolderChoiceB.NONE
                && task.Subfolder != ExtractData.SubfolderChoiceB.SAME)
            {
                dir = Path.Combine(dir, SubFolder(task.Subfolder));
            }
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch
                {
                    bool failed = true;
                    if (SharesHandler.Handler.TryReconnect(dir, task.Username, task.Password))
                    {
                        failed = false;
                        if (!Directory.Exists(dir))
                        {
                            try
                            {
                                Directory.CreateDirectory(dir);
                            }
                            catch
                            {
                                failed = true;
                            }
                        }
                    }
                    if (failed)
                    {
                        Log(Logging.Level.Exception, iba.Properties.Resources.logCreateDirectoryFailed + ": " + dir, filename, task);
                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                        }
                        return null;
                    }
                }
            }
            string ext = (task.FileType == ExtractData.ExtractFileType.BINARY?"dat":"txt");
            //arg += ":" + Path.Combine(dir, actualFileName + ext);
            string arg = Path.Combine(dir, actualFileName + "." + ext);
            if (task.Subfolder != ExtractData.SubfolderChoiceB.NONE)
            {
                List<string> subdirs = new List<string>(Directory.GetDirectories(Directory.GetParent(dir).FullName));
                while (subdirs.Count > task.SubfoldersNumber)
                {
                    DateTime oldestDate = DateTime.MaxValue;
                    string candidate = Algorithms.min<string>(subdirs);
                    try
                    {
                        Directory.Delete(candidate, true);
                    }
                    catch
                    {
                        Log(Logging.Level.Warning, iba.Properties.Resources.logRemoveDirectoryFailed + ": " + candidate, filename);
                        break;
                    }
                    subdirs.Remove(candidate);
                }
            }
            return DatCoordinatorHostImpl.Host.FindSuitableFileName(arg);
        }

        private string SubFolder(ReportData.SubfolderChoice choice)
        {
	        DateTime now = DateTime.Now;
            switch (choice)
            {
                case ReportData.SubfolderChoice.HOUR:
                    return now.ToString("yyMMddHH");
                case ReportData.SubfolderChoice.DAY:
                    return now.ToString("yyMMdd");
                case ReportData.SubfolderChoice.MONTH:
                    return now.ToString("yyMM");
                case ReportData.SubfolderChoice.WEEK:
                {
                    int weekNr = GetWeekNumber(now);
                    return (now.Year - 2000).ToString("d2") + weekNr.ToString("d2");
                }
                default:
                    return null;
            }
	    }

        private string SubFolder(CopyMoveTaskData.SubfolderChoiceA choice)
        {
            DateTime now = DateTime.Now;
            switch (choice)
            {
                case CopyMoveTaskData.SubfolderChoiceA.HOUR:
                    return now.ToString("yyMMddHH");
                case CopyMoveTaskData.SubfolderChoiceA.DAY:
                    return now.ToString("yyMMdd");
                case CopyMoveTaskData.SubfolderChoiceA.MONTH:
                    return now.ToString("yyMM");
                case CopyMoveTaskData.SubfolderChoiceA.WEEK:
                    {
                        int weekNr = GetWeekNumber(now);
                        return (now.Year - 2000).ToString("d2") + weekNr.ToString("d2");
                    }
                default:
                    return null;
            }
        }

        private string SubFolder(ExtractData.SubfolderChoiceB choice)
        {
            DateTime now = DateTime.Now;
            switch (choice)
            {
                case ExtractData.SubfolderChoiceB.HOUR:
                    return now.ToString("yyMMddHH");
                case ExtractData.SubfolderChoiceB.DAY:
                    return now.ToString("yyMMdd");
                case ExtractData.SubfolderChoiceB.MONTH:
                    return now.ToString("yyMM");
                case ExtractData.SubfolderChoiceB.WEEK:
                    {
                        int weekNr = GetWeekNumber(now);
                        return (now.Year - 2000).ToString("d2") + weekNr.ToString("d2");
                    }
                default:
                    return null;
            }
        }

        private int GetWeekNumber(DateTime date)
        {
            // Get jan 1st of the year
            DateTime startOfYear = new DateTime(date.Year, 1, 1);
            // Get dec 31st of the year
            DateTime endOfYear = new DateTime(date.Year, 12, 31);

            // ISO 8601 weeks start with Monday 
            // The first week of a year includes the first Thursday 
            // DayOfWeek returns 0 for sunday up to 6 for saterday
            int[] iso8601Correction  = {6,7,8,9,10,4,5};
            int nds = date.Subtract(startOfYear).Days  + iso8601Correction[(int)startOfYear.DayOfWeek];
            int wk = nds / 7;
            if(wk == 0)
                // Return weeknumber of dec 31st of the previous year
                return GetWeekNumber(new DateTime(date.Year-1, 12, 31));
            else if((wk == 53) && (endOfYear.DayOfWeek < DayOfWeek.Thursday))
                // If dec 31st falls before thursday it is week 01 of next year
                return 1;
            else
                return wk;
        }

        private void Report(string filename, ReportData task)
        {
            string arg = "";
            lock (m_sd.DatFileStates)
            {
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.RUNNING;
            }
            if (task.Output != ReportData.OutputChoice.PRINT)
            {
                string actualFileName = Path.GetFileNameWithoutExtension(filename);
                string dir = task.DestinationMap;
                if (String.IsNullOrEmpty(dir))
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.logNoOutputPathSpecified, filename, task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    return;
                }

                if (!Path.IsPathRooted(dir))
                {  //get Absolute path relative to dir
                    dir = Path.Combine(m_cd.DatDirectoryUNC, dir);
                }
                else
                    dir = task.DestinationMapUNC;

                if (dir == m_cd.DatDirectoryUNC)
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.logOutputIsInput, filename, task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    return;
                }

                if (m_cd.SubDirs && task.Subfolder == ReportData.SubfolderChoice.SAME)
                {   //concatenate subfolder corresponding to dat subfolder
                    string s2 = Path.GetFullPath(m_cd.DatDirectory);
                    string s1 = Path.GetFullPath(filename);
                    string s0 = s1.Remove(0, s2.Length + 1);
                    dir = Path.GetDirectoryName(Path.Combine(dir, s0));
                }
                if (task.Subfolder != ReportData.SubfolderChoice.NONE 
                    && task.Subfolder != ReportData.SubfolderChoice.SAME)
                {
                    dir = Path.Combine(dir, SubFolder(task.Subfolder));
                }
                if (!Directory.Exists(dir))
                {
                    try
                    {
                        Directory.CreateDirectory(dir);
                    }
                    catch
                    {
                        bool failed = true;
                        if (SharesHandler.Handler.TryReconnect(dir, task.Username, task.Password))
                        {
                            failed = false;
                            if (!Directory.Exists(dir))
                            {
                                try
                                {
                                    Directory.CreateDirectory(dir);
                                }
                                catch
                                {
                                    failed = true;
                                }
                            }
                        }

                        if (failed)
                        {
                            Log(Logging.Level.Exception, iba.Properties.Resources.logCreateDirectoryFailed + ": " + dir, filename, task);
                            lock (m_sd.DatFileStates)
                            {
                                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                            }
                            return;
                        }
                    }
                }
                string ext = task.Extension;
                //arg += ":" + Path.Combine(dir, actualFileName + ext);
                arg = Path.Combine(dir, actualFileName + "." + ext);
                if (task.Subfolder != ReportData.SubfolderChoice.NONE)
                {
                    List<string> subdirs = new List<string>(Directory.GetDirectories(Directory.GetParent(dir).FullName));
                    while (subdirs.Count > task.SubfoldersNumber)
                    {
                        DateTime oldestDate = DateTime.MaxValue;
                        string candidate = Algorithms.min<string>(subdirs);
                        try
                        {
                            Directory.Delete(candidate, true);
                        }
                        catch
                        {
                            Log(Logging.Level.Warning, iba.Properties.Resources.logRemoveDirectoryFailed + ": " + candidate, filename);
                            break;
                        }
                        subdirs.Remove(candidate);
                    }
                }
                arg = DatCoordinatorHostImpl.Host.FindSuitableFileName(arg);
            }

            try
            {
                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, task.MonitorData))
                {
                    mon.Execute(delegate() { m_ibaAnalyzer.OpenAnalysis(task.AnalysisFile); });
                    Log(Logging.Level.Info, iba.Properties.Resources.logReportStarted, filename, task);
                    if (task.Output != ReportData.OutputChoice.PRINT)
                        mon.Execute(delegate(){m_ibaAnalyzer.Report(arg);});
                    else
                        mon.Execute(delegate(){m_ibaAnalyzer.Report("");});
                    //Thread.Sleep(500);
                    //code on succes
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                    Log(Logging.Level.Info, iba.Properties.Resources.logReportSuccess, filename, task);
                }
            }
            catch (IbaAnalyzerExceedingTimeLimitException te)
            {
                Log(Logging.Level.Exception, te.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.TIMED_OUT;
                }
                StopIbaAnalyzer();
                StartIbaAnalyzer();
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                Log(Logging.Level.Exception, me.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.MEMORY_EXCEEDED;
                }
                StopIbaAnalyzer();
                StartIbaAnalyzer();
            }
            catch
            {
                Log(Logging.Level.Exception, IbaAnalyzerErrorMessage(), filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
            finally
            {
                if (m_ibaAnalyzer != null)
                {
                    try
                    {
                        m_ibaAnalyzer.CloseAnalysis();
                    }
                    catch
                    {
                        Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, filename, task);
                        StopIbaAnalyzer(false);
                        StartIbaAnalyzer();
                    }
                }
            }
        }


        private void CopyDatFile(string filename, CopyMoveTaskData task)
        {
            string dir = task.DestinationMap;
            if (String.IsNullOrEmpty(dir))
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logNoOutputPathSpecified, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return;
            }

            if (!Path.IsPathRooted(dir))
            {  //get Absolute path relative to dir
                dir = Path.Combine(m_cd.DatDirectory, dir);
            }
            else dir = task.DestinationMapUNC;

            if (dir == m_cd.DatDirectoryUNC)
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logOutputIsInput, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return;
            }


            if (m_cd.SubDirs && task.Subfolder == CopyMoveTaskData.SubfolderChoiceA.SAME) //concatenate subfolder corresponding to dat subfolder
            {
                string s2 = Path.GetFullPath(m_cd.DatDirectory);
                string s1 = Path.GetFullPath(filename);
                string s0 = s1.Remove(0, s2.Length + 1);
                dir = Path.GetDirectoryName(Path.Combine(dir, s0));
            }
            if (task.Subfolder != CopyMoveTaskData.SubfolderChoiceA.NONE && task.Subfolder != CopyMoveTaskData.SubfolderChoiceA.SAME)
            {
                dir = Path.Combine(dir, SubFolder(task.Subfolder));
            }
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch
                {
                    bool failed = true;
                    if (SharesHandler.Handler.TryReconnect(dir, task.Username, task.Password))
                    {
                        failed = false;
                        if (!Directory.Exists(dir))
                        {
                            try
                            {
                                Directory.CreateDirectory(dir);
                            }
                            catch
                            {
                                failed = true;
                            }
                        }
                    }
                    if (failed)
                    {
                        Log(Logging.Level.Exception, iba.Properties.Resources.logCreateDirectoryFailed + ": " + dir, filename, task);
                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                        }
                        return;
                    }
                }
            }
            
            if (task.Subfolder != CopyMoveTaskData.SubfolderChoiceA.NONE)
            {
                List<string> subdirs = new List<string>(Directory.GetDirectories(Directory.GetParent(dir).FullName));
                while (subdirs.Count > task.SubfoldersNumber)
                {
                    DateTime oldestDate = DateTime.MaxValue;
                    string candidate = Algorithms.min<string>(subdirs);
                    try
                    {
                        Directory.Delete(candidate, true);
                    }
                    catch
                    {
                        Log(Logging.Level.Exception, iba.Properties.Resources.logRemoveDirectoryFailed + ": " + candidate, filename);
                        break;
                    }
                    subdirs.Remove(candidate);
                }
            }

            string dest = DatCoordinatorHostImpl.Host.FindSuitableFileName(Path.Combine(dir, Path.GetFileName(filename)));
            try
            {
                if (task.RemoveSource)
                {
                    File.Copy(filename, dest,true);
                    File.Delete(filename);
                    Log(Logging.Level.Info, iba.Properties.Resources.logMoveTaskSuccess, filename, task);
                }
                else
                {
                    File.Copy(filename,dest,true);
                    Log(Logging.Level.Info, iba.Properties.Resources.logCopyTaskSuccess, filename, task);
                }
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                }
            }
            catch
            {
                if (task.RemoveSource)
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.logMoveTaskFailed, filename, task);
                }
                else
                {
                    Log(Logging.Level.Exception,iba.Properties.Resources.logCopyTaskFailed, filename, task);
                }
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
        }

        private void Batchfile(string filename,BatchFileData task)
        {
            if( ! File.Exists(task.BatchFile) )
            {
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                Log(Logging.Level.Exception,iba.Properties.Resources.logNoBatchfile,filename,task);
                return;
            }

            using (Process ibaProc = new Process())
            {
                ibaProc.EnableRaisingEvents = false;
                ibaProc.StartInfo.FileName = task.BatchFile;
                ibaProc.StartInfo.Arguments = "\"" + filename + "\" \"" + task.AnalysisFile + "\"";
                ibaProc.StartInfo.CreateNoWindow = true;
                ibaProc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                try
                {
                    ibaProc.Start();
                    if (Program.MainForm != null)
                        Program.MainForm.ReclaimFocus = true;
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.RUNNING;
                    }
                    Log(Logging.Level.Info, iba.Properties.Resources.logBatchfileStarted, filename,task);
                    ibaProc.WaitForExit();
                    lock (m_sd.DatFileStates)
                    {
                        if (ibaProc.ExitCode == 0)
                        {
                            m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                            Log(Logging.Level.Info, iba.Properties.Resources.logBatchfileSuccess, filename,task);
                        }
                        else
                        {
                            m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                            Log(Logging.Level.Exception, iba.Properties.Resources.logBatchfileFailed, filename,task);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.batchfileTitle + ": " + ex.Message, filename, task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                }
            }
        }

        private void IfTask(string filename, IfTaskData task)
        {
            bool bUseAnalysis = File.Exists(task.AnalysisFile);
            try
            {
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.RUNNING;
                }
                Log(Logging.Level.Info, iba.Properties.Resources.logIfTaskStarted, filename, task);

                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, task.MonitorData))
                {
                    if (bUseAnalysis)
                        mon.Execute(delegate() { m_ibaAnalyzer.OpenAnalysis(task.AnalysisFile); });
                    float f = float.NaN;
                    mon.Execute(delegate() { f = m_ibaAnalyzer.Evaluate(task.Expression, (int)task.XType); });

                    if (!float.IsNaN(f) && !float.IsInfinity(f) && f >= 0.5)
                    {
                        //code on succes
                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_TRUE;
                        }
                        Log(Logging.Level.Info, iba.Properties.Resources.logIfTaskEvaluatedTrue, filename, task);
                    }
                    else
                    {
                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FALSE;
                        }
                        Log(Logging.Level.Info, iba.Properties.Resources.logIfTaskEvaluatedFalse, filename, task);
                    }
                }
            }
            catch (IbaAnalyzerExceedingTimeLimitException te)
            {
                Log(Logging.Level.Exception, te.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.TIMED_OUT;
                }
                StopIbaAnalyzer();
                StartIbaAnalyzer();
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                Log(Logging.Level.Exception, me.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.MEMORY_EXCEEDED;
                }
                StopIbaAnalyzer();
                StartIbaAnalyzer();
            }
            catch
            {
                Log(Logging.Level.Exception, IbaAnalyzerErrorMessage(), filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
            finally
            {
                if (m_ibaAnalyzer != null && bUseAnalysis)
                {
                    try
                    {
                        m_ibaAnalyzer.CloseAnalysis();
                    }
                    catch
                    {
                        Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, filename, task);
                        StopIbaAnalyzer(false);
                        StartIbaAnalyzer();
                    }
                }
            }
        }
    }
}
