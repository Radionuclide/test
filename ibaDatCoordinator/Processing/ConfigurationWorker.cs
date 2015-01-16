using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;
using iba.Data;
using ibaFilesLiteLib;
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
        internal StatusData m_sd;
        private Notifier m_notifier;
        public StatusData Status
        {
            get { return m_sd; }
        }

        private bool m_stop;

        public bool Stop
        {
            get 
            { 
                return m_stop; 
            }

            set 
            {
                m_stop = value;
                if (m_stop)
                    m_waitEvent.Set();
                //do finalization of custom tasks
                foreach (TaskData t in m_cd.Tasks)
                {
                    ICustomTaskData c = t as ICustomTaskData;
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
            m_sd.ProcessedFiles = m_processedFiles = new FileSetWithTimeStamps();
            m_sd.ReadFiles = m_toProcessFiles = new FileSetWithTimeStamps();
            networkErrorOccured = false;
            if (m_cd.OnetimeJob)
                m_thread = new Thread(new ThreadStart(RunOneTimeJob));
            else
                m_thread = new Thread(new ThreadStart(Run));


            m_thread.SetApartmentState(ApartmentState.STA);
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

        private object m_fswtLock;
        void DisposeFswt()
        {
            lock (m_fswtLock)
            {
                if (fswt != null)
                {
                    fswt.Created -= new FileSystemEventHandler(OnNewDatFileOrRenameFile);
                    fswt.Renamed -= new RenamedEventHandler(OnNewDatFileOrRenameFile);
                    fswt.Error -= new ErrorEventHandler(OnFileSystemError);
                    fswt.Dispose();
                    fswt = null;
                }
            }
        }

        void RenewFswt()
        {
            lock (m_fswtLock)
            {
                DisposeFswt();
                fswt = new FileSystemWatcher(m_cd.DatDirectoryUNC, "*.dat");
                fswt.NotifyFilter = NotifyFilters.FileName;
                fswt.IncludeSubdirectories = m_cd.SubDirs;
                fswt.Created += new FileSystemEventHandler(OnNewDatFileOrRenameFile);
                fswt.Renamed += new RenamedEventHandler(OnNewDatFileOrRenameFile);
                fswt.Error += new ErrorEventHandler(OnFileSystemError);
                fswt.EnableRaisingEvents = true;
            }
        }

        private bool UpdateConfiguration()
        {
            if (m_toUpdate != null)
            {
                lock (m_listUpdatingLock)
                {
                    if (m_toUpdate.NotificationData.TimeInterval < m_cd.NotificationData.TimeInterval
                        && !m_toUpdate.NotificationData.NotifyImmediately)
                    {
                        if (m_notifyTimer == null) m_notifyTimer = new System.Threading.Timer(OnNotifyTimerTick);
                        m_notifyTimer.Change(m_toUpdate.NotificationData.TimeInterval, TimeSpan.Zero);
                    }
                    if (m_toUpdate.RescanEnabled && m_toUpdate.JobType == ConfigurationData.JobTypeEnum.DatTriggered)
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
                    if (m_toUpdate.ReprocessErrorsTimeInterval < m_cd.ReprocessErrorsTimeInterval && !m_toUpdate.OnetimeJob)
                    {
                        if (reprocessErrorsTimer == null) reprocessErrorsTimer = new System.Threading.Timer(OnReprocessErrorsTimerTick);
                        reprocessErrorsTimer.Change(m_toUpdate.ReprocessErrorsTimeInterval, TimeSpan.Zero);
                    }
                    else if (m_toUpdate.OnetimeJob) //disable reprocess errors
                    {
                        if (reprocessErrorsTimer != null)
                        {
                            reprocessErrorsTimer.Change(Timeout.Infinite, Timeout.Infinite);
                            reprocessErrorsTimer.Dispose();
                            reprocessErrorsTimer = null;
                        }
                    }

                    if (m_sd.Started)
                    {
                        DisposeFswt();
                        if (m_cd.OnetimeJob) SharesHandler.Handler.ReleaseFromConfiguration(m_cd); //in case of onetimejob only tasks were added
                    }

                    ConfigurationData oldConfigurationData = m_cd;
                    m_cd = m_toUpdate.Clone_AlsoCopyGuids();

                    if (!m_cd.OnetimeJob)
                    {
                        object errorObject;
                        SharesHandler.Handler.AddReferencesFromConfiguration(m_cd, out errorObject);
                        bool doSourceDirectory = true;
                        if (errorObject != null)
                        {
                            doSourceDirectory = false;
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
                                //task was the problem, not the source -> doSource
                                doSourceDirectory = true;
                            }
                        }
                        
                        if (doSourceDirectory)
                        {
                            if (!Directory.Exists(m_cd.DatDirectoryUNC)) //share exist but folder does not, this situation is handled by main Run loop
                            {
                                Log(Logging.Level.Exception, iba.Properties.Resources.logDatDirError);
                                networkErrorOccured = true;
                            }
                            else if (m_cd.DetectNewFiles)
                            {
                                RenewFswt();
                                networkErrorOccured = false;
                            }
                            tickCount = 0;
                        }

                        if (!m_cd.DetectNewFiles)
                            DisposeFswt();
                    }
                    else
                    {
                        object errorObject;
                        SharesHandler.Handler.AddReferencesFromConfiguration(m_cd, out errorObject); //for a one time job those are only the tasks
                        if (errorObject != null)
                        {
                            TaskDataUNC t = errorObject as TaskDataUNC;
                            if (t != null)
                            {
                                Log(iba.Logging.Level.Exception, String.Format(iba.Properties.Resources.UNCPathUnavailable, t.DestinationMapUNC));
                            }
                        }
                    }

                    //also update statusdata
                    m_sd.CorrConfigurationData = m_cd;

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


                    //test if output type specified in datco file matches output type specified in pdo

                    foreach (TaskData task in m_cd.Tasks)
                    {

                        //check if fileType matches
                        ExtractData ed = task as ExtractData;
                        if (ed != null && ed.ExtractToFile)
                        {
                            ExtractData.ExtractFileType[] indexToType = 
                                    {ExtractData.ExtractFileType.BINARY, ExtractData.ExtractFileType.TEXT, 
                                        ExtractData.ExtractFileType.COMTRADE, ExtractData.ExtractFileType.TDMS};
                            ed.m_bExternalVideoResultIsCached = false;
                            int index = ExtractTaskWorker.FileTypeAsInt(ed);
                            if (index >= 0 && index < 4 && indexToType[index] != ed.FileType)
                            {
                                string errorMessage = string.Format(iba.Properties.Resources.WarningFileTypeMismatch, ed.FileType, indexToType[index]);
                                Log(Logging.Level.Warning, errorMessage, string.Empty, task);
                                ed.FileType = indexToType[index];
                            }
                        }
                    }

                    //update unc tasks
                    if (m_cd.OnetimeJob) //one time job, reset all quotacleanups
                    {
                        m_quotaCleanups.Clear();
                    }
                    else
                    {
                        List<Guid> toDelete = new List<Guid>();
                        //remove old
                        foreach (KeyValuePair<Guid, FileQuotaCleanup> pair in m_quotaCleanups)
                        {
                            TaskDataUNC task = m_cd.Tasks.Find(delegate(TaskData t) { return t.Guid == pair.Key; }) as TaskDataUNC;
                            if (task == null || !task.UsesQuota)
                                toDelete.Add(pair.Key);
                            else
                            {
                                ExtractData ed = task as ExtractData;
                                if (ed != null)
                                {
                                    if (ed.ExtractToFile)
                                        pair.Value.ResetTask(task, ExtractTaskWorker.GetBothExtractExtensions(ed));
                                    else
                                        toDelete.Add(pair.Key);
                                }
                                else
                                {
                                    CopyMoveTaskData cmtd = task as CopyMoveTaskData;
                                    if (cmtd != null)
                                    {
                                        if (cmtd.ActionDelete)
                                            toDelete.Add(pair.Key);
                                        else
                                            pair.Value.ResetTask(task, ".dat");
                                    }
                                    else
                                    {
                                        ReportData rd = task as ReportData;
                                        if (rd != null)
                                        {
                                            if (rd.Output == ReportData.OutputChoice.FILE)
                                            {
                                                if (rd.Extension == "html" || rd.Extension == "htm")
                                                    pair.Value.ResetTask(task, "." + rd.Extension+ ",*.jpg");
                                                else
                                                    pair.Value.ResetTask(task, "." + rd.Extension);
                                            }
                                            else
                                                toDelete.Add(pair.Key);
                                        }
                                        else
                                        {
                                            UpdateDataTaskData ud = task as UpdateDataTaskData;
                                            if (ud != null)
                                            {
                                                pair.Value.ResetTask(task, ".dat");
                                            }
                                            else
                                                toDelete.Add(pair.Key);
                                        }
                                    }
                                }
                            }
                        }
                        //no need to add new tasks to the list, they will get added when first executed
                        foreach (Guid guid in toDelete)
                            m_quotaCleanups.Remove(guid);
                    }
                    m_needIbaAnalyzer = false;
                    //lastly, execute plugin actions that need to happen in the case of an update
                    foreach (TaskData t in m_cd.Tasks)
                    {
                        TaskData oldtask = null;
                        ICustomTaskData c_old = null;
                        try
                        {
                            oldtask = oldConfigurationData.Tasks[t.Index];
                            c_old = oldtask as ICustomTaskData;
                        }
                        catch
                        {
                            oldtask = null;
                            c_old = null;
                        }

                        ICustomTaskData c_new = t as ICustomTaskData;
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


                        TaskDataUNC uncTask = t as TaskDataUNC;

                        //see if a report or extract or if task is present
                        if (t is ExtractData || t is ReportData || t is IfTaskData || 
                            (uncTask != null && uncTask.DirTimeChoice == TaskDataUNC.DirTimeChoiceEnum.InFile)
                            )
                            m_needIbaAnalyzer = true;


                        if (uncTask != null && uncTask.Subfolder != TaskDataUNC.SubfolderChoice.NONE 
                            && uncTask.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories)
                        {
                            uncTask.DoDirCleanupNow = true;
                        }
                    }

                    Log(Logging.Level.Info, iba.Properties.Resources.UpdateHappened);
                    m_toUpdate = null;
                    return true;
                }
            }
            return false;
        }

        public ConfigurationWorker(ConfigurationData cd)
        {
            m_cd = cd.Clone_AlsoCopyGuids();
            m_sd = new StatusData(cd);
            m_stop = true;
            m_sd.ProcessedFiles = m_processedFiles = new FileSetWithTimeStamps();
            m_sd.ReadFiles = m_toProcessFiles = new FileSetWithTimeStamps();
            m_waitEvent = new AutoResetEvent(false);
            m_notifier = new Notifier(cd);
            m_candidateNewFiles = new List<Pair<string, DateTime> >();
            m_listUpdatingLock = new Object();
            m_quotaCleanups = new SortedDictionary<Guid, FileQuotaCleanup>();
            m_fswtLock = new Object();
            m_udtWorkers = new SortedDictionary<Guid,UpdateDataTaskWorker>();
            m_onNewDatFileOrRenameFileLastCalled = DateTime.MinValue;
            m_licensedTasks = new Dictionary<TaskData, bool>();
        }       
                
        FileSetWithTimeStamps m_processedFiles;
        FileSetWithTimeStamps m_toProcessFiles;
        List<Pair<string,DateTime> > m_candidateNewFiles;
        List<string> m_newFiles;
        List<string> m_directoryFiles;
        SortedDictionary<Guid, UpdateDataTaskWorker> m_udtWorkers;
        Dictionary<TaskData, bool> m_licensedTasks;

        internal void Log(Logging.Level level, string message)
        {
            LogExtraData data = new LogExtraData(String.Empty, null, m_cd);
            LogData.Data.Log(level, message, (object)data);
        }

        internal void Log(Logging.Level level, string message, string datfile)
        {
            LogExtraData data = new LogExtraData(datfile, null, m_cd);
            LogData.Data.Log(level, message, (object)data);
            if (level == Logging.Level.Exception)
            {
                if (message.Contains("The operation"))
                {
                    string debug = message;
                }
            }
        }

        internal void Log(Logging.Level level, string message, string datfile, TaskData task)
        {
            LogExtraData data = new LogExtraData(datfile, task, m_cd);
            LogData.Data.Log(level, message, (object)data);
            if (level == Logging.Level.Exception)
            {
                if (message != null && message.Contains("The operation"))
                {
                    string debug = message;
                }
            }
        }

        internal IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer = null;
        private System.Threading.Timer m_notifyTimer;
        private System.Threading.Timer reprocessErrorsTimer;
        private System.Threading.Timer rescanTimer;
        private SafeTimer retryAccessTimer;
        private System.Threading.Timer testLicenseTimer;

        private bool m_bTimersstopped;
        private bool m_needIbaAnalyzer;

        private FileSystemWatcher fswt = null;

        private string fileCurrentlyBeingProcessed;
        private void Run()
        {
            Log(Logging.Level.Info, iba.Properties.Resources.logConfigurationStarted);

            m_bTimersstopped = false;
            //if(m_cd.JobType == ConfigurationData.JobTypeEnum.Scheduled)
            //{
            //    Log(Logging.Level.Exception, "Execution of scheduled jobs disabled in this beta version");
            //    m_sd.Started = false;
            //    Stop = true;
            //    return;
            //}

            if (m_stop)
            {
                m_sd.Started = false;
                SharesHandler.Handler.ReleaseFromConfiguration(m_cd); 
                return;
            }

            lock (m_licensedTasks)
            {
                m_licensedTasks.Clear();
            }

            bool bPostpone = TaskManager.Manager.DoPostponeProcessing;
            int minutes = TaskManager.Manager.PostponeMinutes;

            //wait until computer is fully started (if selected so)
            if (bPostpone)
            {
                while (((UInt32) System.Environment.TickCount)/60000 < minutes)
                {
                    if (m_stop)
                    {
                        m_sd.Started = false;
                        return;
                    }
                    Thread.Sleep(TimeSpan.FromSeconds(5.0));
                }
                UpdateConfiguration();
            }

            try
            {
                m_bTimersstopped = false;
                if (!TestLicensePlugins(false))
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.logLicenseNoStart);
                    m_sd.Started = false;
                    Stop = true;
                    return;
                }
                //Thread.CurrentThread.Priority = ThreadPriority.BelowNormal;
                if (m_cd.InitialScanEnabled)
                {
                    Thread initialScanThread = new Thread(OnRescanTimerTick);
                    initialScanThread.Name = "initial scan thread";
                    initialScanThread.Priority = ThreadPriority.BelowNormal;
                    initialScanThread.Start();
                }
                else if(m_cd.JobType == ConfigurationData.JobTypeEnum.Scheduled)
                {
                    //try to delete all previous hdq files
                    String searchPattern = m_cd.JobType == ConfigurationData.JobTypeEnum.DatTriggered ? "*.dat" : "*.hdq";
                    try
                    {
                        DirectoryInfo dirInfo = new DirectoryInfo(m_cd.HDQDirectory);
                        FileInfo[] fileInfos = dirInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
                        foreach(var file in fileInfos)
                        {
                            try
                            {
                                File.Delete(file.FullName);
                            }
                            catch
                            {
                            }
                        }
                    }
                    catch
                    {
                    }
                }

                //Thread.CurrentThread.Priority = ThreadPriority.Normal;
                if (m_stop)
                {
                    m_sd.Started = false;
                    return;
                }
                m_notifyTimer = null;
                try
                {
                    reprocessErrorsTimer = new System.Threading.Timer(new TimerCallback(OnReprocessErrorsTimerTick));
                    reprocessErrorsTimer.Change(m_cd.ReprocessErrorsTimeInterval, TimeSpan.Zero);
                   
                    if (m_cd.JobType != ConfigurationData.JobTypeEnum.Scheduled)
                    {
                        retryAccessTimer = new SafeTimer(OnAddNewDatFileTimerTick);
                        retryAccessTimer.Period = TimeSpan.FromSeconds(1);
                    }
                    else
                        ScheduleNextEvent();

                    if (!m_cd.NotificationData.NotifyImmediately)
                    {
                        m_notifyTimer = new System.Threading.Timer(new TimerCallback(OnNotifyTimerTick));
                        m_notifyTimer.Change(m_cd.NotificationData.TimeInterval, m_cd.NotificationData.TimeInterval);
                    }

                    //do initializations of custom tasks
                    foreach(TaskData t in m_cd.Tasks)
                    {
                        ICustomTaskData c = t as ICustomTaskData;
                        if (c != null)
                        {
                            IPluginTaskWorker w = c.Plugin.GetWorker();
                            if (!w.OnStart())
                                Log(iba.Logging.Level.Exception, w.GetLastError(), String.Empty, t);
                        }
                    }

                    while (!m_stop)
                    {
                        while (true)
                        {
                            string file = null;
                            lock(m_toProcessFiles)
                            {
                                file = m_toProcessFiles.OldestExistingFile();
                                fileCurrentlyBeingProcessed = file;
                                if(file == null) //no existing file
                                {
                                    m_toProcessFiles.Clear();
                                    break;
                                }
                            }
                            if (m_needIbaAnalyzer) StartIbaAnalyzer();
                            //might have waited, see if we can stop
                            if (m_stop)
                            {
                                if (m_ibaAnalyzer != null) YieldIbaAnalyzer();
                                break;
                            }
                            try
                            {
                                ProcessDatfile(file);
                            }
                            catch (Exception ex)
                            {
                                Stop = true;
                                Log(iba.Logging.Level.Exception, iba.Properties.Resources.UnexpectedErrorDatFile + ex.ToString(), file);
                            }
                            if (m_ibaAnalyzer != null) YieldIbaAnalyzer();
                            if (m_stop) break;
                            lock (m_toProcessFiles)
                            {
                                m_toProcessFiles.Remove(file);
                                fileCurrentlyBeingProcessed = null;
                            }
                            bool neededIbaAnalyzerBeforeUpdate = m_needIbaAnalyzer;
                            UpdateConfiguration();
                            if (neededIbaAnalyzerBeforeUpdate && !m_needIbaAnalyzer)
                            {
                                IbaAnalyzerCollection.Collection.TryClearIbaAnalyzer(m_cd);
                            }
                        }
                        //clean up any updatetaskworkers //closes database connections and ibaFiles instances
                        foreach (UpdateDataTaskWorker udt in m_udtWorkers.Values)
                        {
                            udt.Dispose();
                        }
                        m_udtWorkers.Clear();
                        m_waitEvent.WaitOne();
                        UpdateConfiguration();
                    }
                    if (m_needIbaAnalyzer)
                        IbaAnalyzerCollection.Collection.TryClearIbaAnalyzer(m_cd);
                }
                finally
                {
                    m_bTimersstopped = true;
                    if (m_notifyTimer != null)
                    {
                        m_notifyTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        m_notifyTimer.Dispose();
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
                        retryAccessTimer.Dispose();
                        retryAccessTimer = null;
                    }
                    if (NextEventTimer != null)
                    {
                        NextEventTimer.Dispose();
                        NextEventTimer = null;
                    }
                    if (testLicenseTimer != null)
                    {
                        testLicenseTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        testLicenseTimer.Dispose();
                        testLicenseTimer = null;
                    }
                }
            }
            finally
            {
                DisposeFswt();
            }

            m_sd.Started = false;
            SharesHandler.Handler.ReleaseFromConfiguration(m_cd);
            Log(Logging.Level.Info, iba.Properties.Resources.logConfigurationStopped);
        }


        private void RunOneTimeJob()
        {
            Log(Logging.Level.Info, iba.Properties.Resources.logConfigurationStarted);
            if (m_stop)
            {
                m_sd.Started = false;
                SharesHandler.Handler.ReleaseFromConfiguration(m_cd);
                return;
            }


            lock (m_licensedTasks)
            {
                m_licensedTasks.Clear();
            }
            if (!TestLicensePlugins(false))
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logLicenseNoStart);
                m_sd.Started = false;
                Stop = true;
                return;
            }

            m_bTimersstopped = false;
            m_notifyTimer = null;
            try
            {
                if (!m_cd.NotificationData.NotifyImmediately)
                {
                    m_notifyTimer = new System.Threading.Timer(new TimerCallback(OnNotifyTimerTick));
                    m_notifyTimer.Change(m_cd.NotificationData.TimeInterval, m_cd.NotificationData.TimeInterval);
                }

                //do initializations of custom tasks
                foreach (TaskData t in m_cd.Tasks)
                {
                    ICustomTaskData c = t as ICustomTaskData;
                    if (c != null)
                    {
                        IPluginTaskWorker w = c.Plugin.GetWorker();
                        if (!w.OnStart())
                            Log(iba.Logging.Level.Exception, w.GetLastError(), String.Empty, t);
                    }
                }

                string[] unclines = m_cd.DatDirectoryUNC.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string line in unclines)
                {
                    if (m_stop) break;
                    lock (m_toProcessFiles)
                    {
                        m_toProcessFiles.Clear();
                    }
                    lock (m_processedFiles)
                    {
                        m_processedFiles.Clear();
                    }
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates.Clear();
                    }

                    string error;
                    SharesHandler.Handler.AddReferenceDirect(line, m_cd.Username, m_cd.Password, out error);
                    if (!String.IsNullOrEmpty(error))
                    {
                        Log(iba.Logging.Level.Exception, error);
                        continue;
                    }
                    List<FileInfo> fileInfosList = new List<FileInfo>();
                    if (Directory.Exists(line))
                    {
                        string scanmessage = String.Format(iba.Properties.Resources.OnTimeJobDirScanStarted, line);
                        Log(iba.Logging.Level.Info, scanmessage, line);
                        DirectoryInfo dirInfo = new DirectoryInfo(line);
                        try
                        {
                            if (m_cd.SubDirs)
                            {
                                fileInfosList = Utility.PathUtil.GetFilesInSubsSafe("*.dat", dirInfo);
                            }
                            else
                            {
                                FileInfo[] fileInfos = dirInfo.GetFiles("*.dat", SearchOption.TopDirectoryOnly);
                                fileInfosList.AddRange(fileInfos);
                            }
                            fileInfosList.Sort( delegate(FileInfo f1, FileInfo f2)
                            {
                                int onTime = f1.LastWriteTime.CompareTo(f2.LastWriteTime);
                                return onTime == 0 ? f1.FullName.CompareTo(f2.FullName) : onTime;
                            }); //oldest files first
                        }
                        catch
                        {
                            Log(Logging.Level.Exception, iba.Properties.Resources.logDatDirError);
                            m_sd.UpdatingFileList = false;
                            continue;
                        }
                        string message = string.Format(iba.Properties.Resources.OnTimeJobDirScanFinished, line, fileInfosList.Count);
                        Log(iba.Logging.Level.Info, message, line);
                    }
                    else if (File.Exists(line))
                    {
                        fileInfosList.Add(new FileInfo(line));
                    }
                    if (m_stop) break;

                    UpdateDatFileListOneTimeJob(fileInfosList);

                    while (!m_stop)
                    {
                        string file = m_toProcessFiles.OldestExistingFile();
                        if (file == null) //no existing file
                        {
                            lock (m_toProcessFiles)
                            {
                                m_toProcessFiles.Clear();
                            }
                            break;
                        }
                        if (m_needIbaAnalyzer) StartIbaAnalyzer();
                        //might have waited, see if we need to stop
                        if (m_stop)
                        {
                            if (m_ibaAnalyzer != null) YieldIbaAnalyzer();
                            break;
                        }
                        try
                        {
                            ProcessDatfile(file);
                        }
                        catch (Exception ex)
                        {
                            Stop = true;
                            Log(iba.Logging.Level.Exception, iba.Properties.Resources.UnexpectedErrorDatFile + ex.ToString(), line);
                        }
                        if (m_ibaAnalyzer != null) YieldIbaAnalyzer();
                        if (m_stop) break;
                        lock (m_toProcessFiles)
                        {
                            m_toProcessFiles.Remove(file);
                        }

                        if (fileInfosList.Count > 0)
                            UpdateDatFileListOneTimeJob(fileInfosList);
                    }
                    
                }
                //stop the com object
                if (m_needIbaAnalyzer) IbaAnalyzerCollection.Collection.TryClearIbaAnalyzer(m_cd);

                //clean up any updatetaskworkers //closes database connections and ibaFiles instances
                foreach (UpdateDataTaskWorker udt in m_udtWorkers.Values)
                {
                    udt.Dispose();
                }
                m_udtWorkers.Clear();
            }
            finally
            {
                m_bTimersstopped = true;
                if (m_notifyTimer != null)
                {
                    m_notifyTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    m_notifyTimer.Dispose();
                    m_notifier.Send(); //send one last time
                }
                if (testLicenseTimer != null)
                {
                    testLicenseTimer.Change(Timeout.Infinite, Timeout.Infinite);
                    testLicenseTimer.Dispose();
                    testLicenseTimer = null;
                }
                Debug.Assert(m_ibaAnalyzer == null, "ibaAnalyzer should have been closed");
            }
            m_sd.Started = false;
            SharesHandler.Handler.ReleaseFromConfiguration(m_cd);
            
            string[] unclines2 = m_cd.DatDirectoryUNC.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string line in unclines2)
                SharesHandler.Handler.ReleaseReferenceDirect(line);
            Log(Logging.Level.Info, iba.Properties.Resources.logConfigurationStopped);
        }

        void UpdateDatFileListOneTimeJob(List<FileInfo> infos)
        {
            const int enoughToProcess = 50;
            const int maxToProcess = 100;
            const int maxProcessedWithoutErrors = 5;
            const int maxProcessedWithErrors = 50;
            bool changed = false;
            lock (m_toProcessFiles)
            {
                if (m_toProcessFiles.Count < enoughToProcess)
                {
                    m_sd.UpdatingFileList = true;
                    while (infos.Count > 0 && m_toProcessFiles.Count < maxToProcess)
                    {
                        FileInfo candidate = infos[0];
                        infos.RemoveAt(0);
                        //TODO: test if the file should be processed
                        string DatFile = candidate.FullName;
                        DatFileStatus.State state = ProcessDatfileReadynessOneTimeJob(DatFile);
                        if (state != DatFileStatus.State.NOT_STARTED)
                        {
                            lock (m_processedFiles)
                            {
                                if (!m_processedFiles.Contains(DatFile))
                                    m_processedFiles.Add(DatFile);
                            }
                            lock (m_sd.DatFileStates)
                            {
                                foreach (TaskData t in m_cd.Tasks)
                                    m_sd.DatFileStates[DatFile].States[t] = state;
                            }
                        }
                        else
                        {
                            m_toProcessFiles.Add(DatFile);
                            lock (m_sd.DatFileStates)
                            {
                                m_sd.DatFileStates[DatFile] = new DatFileStatus();
                            }
                        }
                        changed = true;
                    }
                }
            }
            lock (m_processedFiles)
            {
                int count = m_processedFiles.Count;
                List<string> toCleanupWithoutErrors = new List<string>();
                List<string> toCleanupWithErrors = new List<string>();
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
                            toCleanupWithoutErrors.Add(filename);
                        else
                            toCleanupWithErrors.Add(filename);
                    }
                }
                List<string>[] cleanupLists = new List<string>[2] { toCleanupWithoutErrors, toCleanupWithErrors };
                foreach (List<string> cleanupList in cleanupLists)
                {
                    int maxCount = (cleanupList == toCleanupWithoutErrors ? maxProcessedWithoutErrors : maxProcessedWithErrors);
                    while (cleanupList.Count > maxCount)
                    {
                        changed = true;
                        m_sd.UpdatingFileList = true;
                        string filename = cleanupList[0];
                        lock (m_sd.DatFileStates)
                        {
                            m_processedFiles.Remove(filename);
                            if (m_sd.DatFileStates.ContainsKey(filename))
                                m_sd.DatFileStates.Remove(filename);
                        }
                        cleanupList.RemoveAt(0);
                    }
                }
            }

            m_sd.UpdatingFileList = false;
            if (changed)
            {
                m_sd.MergeProcessedAndToProcessLists();
                m_sd.Changed = true;
            }
        }

        private bool networkErrorOccured = false;
        private int tickCount = 0;

        void OnFileSystemError(object sender, ErrorEventArgs e)
        {
            networkErrorOccured = true;
            DisposeFswt();
            //can also happen without that connection is lost, alter error message
            if (m_cd.DatDirectory==m_cd.DatDirectoryUNC)
                Log(iba.Logging.Level.Exception, String.Format(iba.Properties.Resources.FileSystemWatcherProblem, m_cd.DatDirectoryUNC, e==null?"":e.GetException().Message));
            else
                Log(iba.Logging.Level.Exception, String.Format(iba.Properties.Resources.ConnectionLostFrom, m_cd.DatDirectoryUNC, e == null ? "" : e.GetException().Message));
        }


        private bool TestLicensePlugins()
        {
            return TestLicensePlugins(true);
        }

        private bool TestLicensePlugins(bool startTimerIfNotOk)
        {
            CDongleInfo info = null;
            bool ok = true;
            bool dongleFound = false;
            foreach (TaskData task in m_cd.Tasks)
            {
                ICustomTaskData cust = task as ICustomTaskData;
                if (cust != null)
                {
                    lock (m_licensedTasks)
                    {
                        if (m_licensedTasks.ContainsKey(task))
                            m_licensedTasks[task] = true;
                        else
                            m_licensedTasks.Add(task, true);
                    }
                    if (info == null)
                    {
                        info = CDongleInfo.ReadDongle();
                        if (info.DongleFound) dongleFound = true;
                    }
                    if (!info.PluginsLicensed() || !info.IsPluginLicensed(cust.Plugin.DongleBitPos))
                    {
                        ok = false;
                        lock (m_licensedTasks)
                        {
                            m_licensedTasks[task] = false;
                        }
                        //Log(Logging.Level.Exception, String.Format(iba.Properties.Resources.logCustomTaskNotLicensed, task.Name));
                    }
                }
                else
                {
                    UpdateDataTaskData ut = task as UpdateDataTaskData;
                    if (ut != null)
                    {
                        lock (m_licensedTasks)
                        {
                            if (m_licensedTasks.ContainsKey(task))
                                m_licensedTasks[task] = true;
                            else
                                m_licensedTasks.Add(task, true);
                        }
                        if (info == null)
                        {
                            info = CDongleInfo.ReadDongle();
                            if (info.DongleFound) dongleFound = true;
                        }
                        if (!info.IsPluginLicensed(2))
                        {
                            ok = false;
                            lock (m_licensedTasks)
                            {
                                m_licensedTasks[task] = false;
                            }
                            //Log(Logging.Level.Exception, String.Format(iba.Properties.Resources.logCustomTaskNotLicensed, task.Name));
                        }
                    }
                }
            }
            if (info != null && (ok||startTimerIfNotOk)) // start timer in two minutes if dongle found, 10 seconds if no dongle found, don't restart it if no custom jobs were found
            {
                if (testLicenseTimer == null) 
                    testLicenseTimer = new System.Threading.Timer(OnTestLicenseTimerTick);
                if (!m_bTimersstopped && !m_stop)
                    testLicenseTimer.Change(dongleFound ? TimeSpan.FromMinutes(2.0) : TimeSpan.FromSeconds(10.0), TimeSpan.Zero);
            }
            return ok;
        }

        internal void StartIbaAnalyzer()
        {
            m_ibaAnalyzer = IbaAnalyzerCollection.Collection.ClaimIbaAnalyzer(m_cd);
            if (m_ibaAnalyzer == null)
            {
                m_sd.Started = false;
                Stop = true;
            }
        }

        internal void StopIbaAnalyzer()
        {
            IbaAnalyzerCollection.Collection.RelinquishIbaAnalyzer(m_ibaAnalyzer, true, m_cd);
            m_ibaAnalyzer = null;
        }

        internal void YieldIbaAnalyzer()
        {
            IbaAnalyzerCollection.Collection.RelinquishIbaAnalyzer(m_ibaAnalyzer, false, m_cd);
            m_ibaAnalyzer = null;
        }

        internal void RestartIbaAnalyzer()
        {
            IbaAnalyzerCollection.Collection.RestartIbaAnalyzer(ref m_ibaAnalyzer, m_cd);
            if (m_ibaAnalyzer == null)
            {
                m_sd.Started = false;
                Stop = true;
            }
        }

        internal string IbaAnalyzerErrorMessage()
        {
            try
            {
                if (m_ibaAnalyzer != null)
                    return m_ibaAnalyzer.GetLastError();
                else
                {
                    RestartIbaAnalyzer();
                    return iba.Properties.Resources.IbaAnalyzerUndeterminedError;
                }
            }
            catch (Exception ex)
            {
                Log(Logging.Level.Exception, ex.Message);
                RestartIbaAnalyzer();
                return iba.Properties.Resources.IbaAnalyzerUndeterminedError;
            }
        }

        private DateTime m_onNewDatFileOrRenameFileLastCalled;

        private void OnNewDatFileOrRenameFile(object sender, FileSystemEventArgs args)
        {
            string filename = args.FullPath;
            lock (m_candidateNewFiles)
            {
                if (m_candidateNewFiles.Find(delegate(Pair<string,DateTime> arg) { return arg.First==filename; })==null)
                {
                    DateTime now = DateTime.Now;
                    m_candidateNewFiles.Add(new Pair<string,DateTime>(filename,now));
                    if (now > m_onNewDatFileOrRenameFileLastCalled + TimeSpan.FromSeconds(2)) //if .dat files arrive to fast don't wait a second for processing them
                        m_skipAddNewDatFileTimerTick = true; //wait one tick before checking access, to let PDA do stuff
                    m_onNewDatFileOrRenameFileLastCalled = now;
                }
            }
        }

        private object m_listUpdatingLock; //makes the timer routines mutually exclusive
        private bool m_skipAddNewDatFileTimerTick;

        //int timerBusy;

        private bool OnAddNewDatFileTimerTick()
        {

            if (m_bTimersstopped || m_stop || retryAccessTimer == null) return false;
            bool changed = false;
            List<string> toRemove = new List<string>();
            lock (m_listUpdatingLock)
            {
                m_newFiles = new Set<string>();
                lock (m_candidateNewFiles)
                {
                    if (m_skipAddNewDatFileTimerTick)
                    {
                        m_skipAddNewDatFileTimerTick = false;
                        return true;
                    }

                    foreach (Pair<string,DateTime> p in m_candidateNewFiles)
                    {
                        string filename = p.First;
                        FileStream fs = null;
                        try
                        {
                            //if marked by PDA as offline, don't process this file yet
                            if ((File.GetAttributes(filename) & FileAttributes.Offline) == FileAttributes.Offline)
                            {
                                if (Math.Abs((DateTime.Now - p.Second).TotalHours) > 1)
                                {
                                    Log(iba.Logging.Level.Warning, iba.Properties.Resources.Noaccess4, filename);
                                    p.Second = DateTime.Now;
                                }
                                continue; //don't remove, we look at it next time
                            }

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
                        catch//no access, do not remove from the list yet (unless the file doesnt exist), make available again when acces to it is restored
                        {
                            try
                            {
                                if (!File.Exists(filename))
                                    toRemove.Add(filename);
                                else
                                {
                                    if (Math.Abs((DateTime.Now - p.Second).TotalHours) > 1)
                                    {
                                        Log(iba.Logging.Level.Warning, iba.Properties.Resources.Noaccess5, filename);
                                        p.Second = DateTime.Now;
                                    }
                                }
                            }
                            catch
                            {
                            }
                        }
                    }
                    foreach (string filename in toRemove)
                        m_candidateNewFiles.RemoveAll(delegate(Pair<string,DateTime> arg) { return arg.First==filename; });
                }
                if (changed)
                {
                    updateDatFileList(WhatToUpdate.NEW);
                    m_waitEvent.Set();
                }
            }

            if (!changed && networkErrorOccured)
                tickCount++;

            if (tickCount >= 20) //retry restoring dataaccess every 20 seconds
            {
                tickCount = 0;
                if (m_cd.DatDirectoryUNC.StartsWith(@"\\"))
                {
                    bool directoryExists = Directory.Exists(m_cd.DatDirectoryUNC);
                    try
                    {
                        if (SharesHandler.Handler.TryReconnect(m_cd.DatDirectoryUNC, m_cd.Username, m_cd.Password))
                        {
                            if (Directory.Exists(m_cd.DatDirectoryUNC))
                            {
                                if (m_cd.DetectNewFiles)
                                {
                                    try
                                    {
                                        RenewFswt();
                                    }
                                    catch (System.IO.FileNotFoundException)
                                    {
                                        //Log(iba.Logging.Level.Warning, String.Format("Directory {0} exists but setting FileSystemWatcher failed, forcing reconnect.",m_cd.DatDirectoryUNC));
                                        SharesHandler.Handler.TryReconnectForce(m_cd.DatDirectoryUNC, m_cd.Username, m_cd.Password);
                                        RenewFswt();
                                    }
                                }
                                else
                                    DisposeFswt();
                                networkErrorOccured = false;
                                Log(iba.Logging.Level.Info, String.Format(iba.Properties.Resources.ConnectionRestoredTo, m_cd.DatDirectoryUNC));
                            }
                        }
                    }
                    catch (Exception /*ex*/)
                    {
                        //Log(iba.Logging.Level.Exception, m_cd.DatDirectoryUNC + "\r\n" + directoryExists + "\r\n" + ex.ToString());
                    }
                }
                else if (Directory.Exists(m_cd.DatDirectoryUNC))
                {
                    if (m_cd.DetectNewFiles)
                        RenewFswt();
                    else
                        DisposeFswt();
                    networkErrorOccured = false;
                       Log(iba.Logging.Level.Info, String.Format(iba.Properties.Resources.DirectoryFound, m_cd.DatDirectoryUNC));
                }
            }
            return (!m_bTimersstopped && !m_stop);
        }

        private void OnRescanTimerTick(object ignoreMe)
        {
            if (networkErrorOccured)
            {
                if (!m_bTimersstopped && !m_stop && rescanTimer != null)
                    rescanTimer.Change(m_cd.RescanTimeInterval, TimeSpan.Zero);
                return; //wait until problem is fixed
            }
            if (m_bTimersstopped || m_stop) return;
            if (rescanTimer != null)
                rescanTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ScanDirectory();
            if (!m_bTimersstopped && !m_stop && rescanTimer!=null)
                rescanTimer.Change(m_cd.RescanTimeInterval, TimeSpan.Zero);
        }

        private void OnReprocessErrorsTimerTick(object ignoreMe)
        {
            if (networkErrorOccured)
            {
                if (!m_bTimersstopped && !m_stop && reprocessErrorsTimer != null)
                    reprocessErrorsTimer.Change(m_cd.ReprocessErrorsTimeInterval, TimeSpan.Zero);
                return; //wait until problem is fixed
            }
            if (m_bTimersstopped || m_stop || reprocessErrorsTimer==null) return;
            reprocessErrorsTimer.Change(Timeout.Infinite, Timeout.Infinite);
            lock (m_listUpdatingLock)
            {
                Log(Logging.Level.Info, iba.Properties.Resources.logCheckingForErrorDatFiles);
                updateDatFileList(WhatToUpdate.ERRORS);
                m_waitEvent.Set();
            }
            if (!m_bTimersstopped && !m_stop)
                reprocessErrorsTimer.Change(m_cd.ReprocessErrorsTimeInterval, TimeSpan.Zero);
        }

        private void OnNotifyTimerTick(object ignoreMe)
        {
            lock (m_notifier)
            {
                if (m_bTimersstopped || m_stop) return;
                m_notifyTimer.Change(Timeout.Infinite, Timeout.Infinite);
                m_notifier.Send();
                if (!m_bTimersstopped && !m_stop)
                    m_notifyTimer.Change(m_cd.NotificationData.TimeInterval, TimeSpan.Zero);
            }
        }

        private void OnTestLicenseTimerTick(object ignoreMe)
        {
            if (m_bTimersstopped || m_stop) return;
            testLicenseTimer.Change(Timeout.Infinite, Timeout.Infinite);
            TestLicensePlugins();
            //below commented out, we no longer stop the job ...
            /*if (!TestLicensePlugins())
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logLicenseStopped);
                Stop = true;
            }*/
        }

        private enum WhatToUpdate {NEW, ERRORS, DIRECTORY};

        private void ScanDirectory()
        {
            if (m_toProcessFiles.Count >= 10000 && m_cd.RescanEnabled) //failsafe, if processed files is to large, 
            {  
            //don't add any extra (and hope that it will be small enough for the next timer tick
                Log(Logging.Level.Info, iba.Properties.Resources.logCheckingForUnprocessedDatFilesLimitReached);
                return;
            }
            Log(Logging.Level.Info, iba.Properties.Resources.logCheckingForUnprocessedDatFiles);

            string datDir = m_cd.DatDirectoryUNC;
            if (!Directory.Exists(datDir))
            {
                networkErrorOccured = true;
                Log(Logging.Level.Exception, iba.Properties.Resources.logDatDirError);
                return;
            }
 
            //before we do any damage, see if we can get the file info, otherwise quit it.
            DirectoryInfo dirInfo = new DirectoryInfo(datDir);
            FileInfo[] fileInfos = null;
            m_sd.UpdatingFileList = true;

            String searchPattern = m_cd.JobType == ConfigurationData.JobTypeEnum.DatTriggered ? "*.dat" : "*.hdq";
            try
            {
                if (m_cd.SubDirs)
                {
                    List<FileInfo> fileInfosList = Utility.PathUtil.GetFilesInSubsSafe(searchPattern, dirInfo);
                    fileInfos = fileInfosList.ToArray();
                }
                else
                    fileInfos = dirInfo.GetFiles(searchPattern, SearchOption.TopDirectoryOnly);
                Array.Sort(fileInfos, delegate(FileInfo f1, FileInfo f2)
                {
                    int onTime = f1.LastWriteTime.CompareTo(f2.LastWriteTime);
                    return onTime == 0 ? f1.FullName.CompareTo(f2.FullName) : onTime;
                }); //oldest files first
            }
            catch (Exception ex)
            {
                networkErrorOccured = true;
                string message = string.Format(iba.Properties.Resources.logDatDirError2, datDir, ex.Message);
                Log(Logging.Level.Exception, iba.Properties.Resources.logDatDirError2);
                m_sd.UpdatingFileList = false;
                return;
            }

            //second step, clear processed files
            lock (m_listUpdatingLock)
            {
                lock (m_processedFiles)
                {
                    lock (m_sd.DatFileStates)
                    {
                        foreach (string file in m_processedFiles)
                        {
                            if (m_sd.DatFileStates.ContainsKey(file))
                                m_sd.DatFileStates.Remove(file);
                        }
                    }
                    m_processedFiles.Clear();
                }
                m_sd.MergeProcessedAndToProcessLists();
            }
            int count = 0;
            m_directoryFiles = new List<string>();
            m_sd.UpdatingFileList = false;
            foreach (FileInfo info in fileInfos)
            {
                if (m_stop) break;
                string filename = info.FullName;
                lock (m_toProcessFiles)
                {
                    if (m_toProcessFiles.Count >= 10000 && m_cd.RescanEnabled) //failsafe, if processed files is to large, 
                        //don't add any extra (and hope that it will be small enough for the next timer tick
                    {
                        Log(Logging.Level.Info, iba.Properties.Resources.logCheckingForUnprocessedDatFilesLimitReached);
                        m_directoryFiles.Clear();
                        return;
                    }
                    if (m_toProcessFiles.Contains(filename))
                        continue;
                }
                if(m_cd.JobType == ConfigurationData.JobTypeEnum.Scheduled || !IsInvalidOrProcessed(filename)) //hdq files will be deleted.
                {
                    lock (m_candidateNewFiles)
                    {
                        if(m_candidateNewFiles.Find(delegate(Pair<string, DateTime> arg) { return arg.First == filename; }) == null)
                        {
                            m_directoryFiles.Add(filename);
                            count++;
                        }
                    }
                }
                
                if (count >= 50)
                {
                    lock (m_listUpdatingLock)
                    {
                        updateDatFileList(WhatToUpdate.DIRECTORY);
                    }
                    m_waitEvent.Set(); //if main thread was waiting, let it continue
                    count = 0;
                    m_directoryFiles.Clear();
                }
                Thread.Sleep(0);
            }
            if (count > 0 && !m_stop)
            {
                lock (m_listUpdatingLock)
                {
                    updateDatFileList(WhatToUpdate.DIRECTORY);
                }
                m_waitEvent.Set();
            }
            Log(Logging.Level.Info, iba.Properties.Resources.logCheckingForUnprocessedDatFilesFinished);
        }

        private bool IsInvalidOrProcessed(string DatFile)
        {   //returns true if processed flag is found or this is not an ibaFile
            //in both cases the file can be ignored for further processing
            FileStream fs;
            try
            {
                fs = new FileStream(DatFile, FileMode.Open, FileAccess.Read, FileShare.Read);
            }
            catch
            {
                return false; //can't open it, let UpdateFiles handle this case
            }
            
            Byte [] myBytes = new Byte[4];
            try
            {
                fs.Read(myBytes, 0, 4);
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();
                string str = enc.GetString(myBytes);
                if (str != "PDA1" && str != "PDA2" && str != "QDR1")
                    return true;
                fs.Read(myBytes, 0, 4);
                fs.Read(myBytes, 0, 4);
                uint offset = System.BitConverter.ToUInt32(myBytes, 0);
                if (offset < fs.Length)
                    fs.Seek(offset, SeekOrigin.Begin);
                else
                {
                    fs.Close();
                    fs.Dispose();
                    return true;
                }

                bool succes = false;
                using (StreamReader sr = new StreamReader(fs))
                {
                    while (true)
                    {
                        String line = sr.ReadLine();
                        if (line == null) break;
                        else if (line.StartsWith("endASCII")) break;
                        else if (line.StartsWith("status"))
                        {
                            if (line == "status:processed")
                            {
                                succes = true;
                                break;
                            }
                        }
                        else if (line.StartsWith("$DATCOOR_status"))
                        {
                            if (line == "$DATCOOR_status:processed")
                            {
                                succes = true;
                                break;
                            }
                        }
                    }
                }
                fs.Close();
                fs.Dispose();
                return succes;
            }
            catch
            {
                fs.Close();
                fs.Dispose();
                return true; // when failing in this stage it is not a PDA file, ignore this file
            }
        }

        private void updateDatFileList(WhatToUpdate what)
        {
            string datDir = m_cd.DatDirectoryUNC;
            FileInfo[] fileInfos = null;
            m_sd.UpdatingFileList = true;
            lock (m_processedFiles)
            {
                int count = m_processedFiles.Count;
                fileInfos = new FileInfo[count];
                if (what == WhatToUpdate.ERRORS)
                {
                    for (int i = 0; i < count; i++)
                    {
                        lock (m_sd.DatFileStates)
                        {
                            if(m_processedFiles[i] != fileCurrentlyBeingProcessed && File.Exists(m_processedFiles[i])
                                && (m_sd.DatFileStates.ContainsKey(m_processedFiles[i])))
                            {
                                fileInfos[i] = new FileInfo(m_processedFiles[i]);
                            }
                            else
                            {
                                fileInfos[i] = null;
                            }
                        }
                    }
                }
                else if (what == WhatToUpdate.NEW)//NEW
                {
                    count = m_newFiles.Count;
                    fileInfos = new FileInfo[count];
                    for (int i = 0; i < count; i++)
                    {
                        string filename = m_newFiles[i];
                        if(fileCurrentlyBeingProcessed != filename && File.Exists(filename)
                            && (!m_sd.DatFileStates.ContainsKey(filename)))
                        {
                            fileInfos[i] = new FileInfo(m_newFiles[i]);
                        }
                        else
                            fileInfos[i] = null;
                    }
                    m_newFiles.Clear();
                }
                else if (what == WhatToUpdate.DIRECTORY)//DIRECTORY
                {
                    count = m_directoryFiles.Count;
                    fileInfos = new FileInfo[count];
                    for (int i = 0; i < count; i++)
                    {
                        string filename = m_directoryFiles[i];
                        if(fileCurrentlyBeingProcessed != filename && File.Exists(filename)
                            && (!m_sd.DatFileStates.ContainsKey(filename)))
                        {
                            fileInfos[i] = new FileInfo(m_directoryFiles[i]);
                        }
                        else
                        {
                            fileInfos[i] = null;
                        }
                    }
                    m_directoryFiles.Clear();
                }

                for (int i = 0; i < count; i++)
                {
                    if(fileInfos[i] != null && m_processedFiles.Contains(fileInfos[i].FullName))
                    {
                        m_processedFiles.Remove(fileInfos[i].FullName);
                    }
                }

                count = m_processedFiles.Count;
                List<string> toCleanup = new List<string>();
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
                            toCleanup.Add(filename);
                        }
                    }
                    //keep the 5 most recent
                }

                while (toCleanup.Count > 5)
                {
                    string filename = toCleanup[0];
                    lock (m_sd.DatFileStates)
                    {
                        m_processedFiles.Remove(filename);
                        if (m_sd.DatFileStates.ContainsKey(filename))
                            m_sd.DatFileStates.Remove(filename);
                    }
                    toCleanup.RemoveAt(0);
                }
                //remaining, mark them, so they do not 
            }
            lock (m_toProcessFiles)
            {
                bool bPermanentErrorFilesChanged = false;
                foreach (FileInfo fi in fileInfos)
                {
                    if (m_stop) break;
                    if (fi == null) continue;
                    string filename = fi.FullName;
                    DatFileStatus.State state;
                    if (m_cd.JobType == ConfigurationData.JobTypeEnum.DatTriggered)
                        state = ProcessDatfileReadyness(filename);
                    else
                        state = ProcessHDQfileReadyness(filename);
                    switch (state)
                    {
                        case DatFileStatus.State.NOT_STARTED:
                            if (!m_toProcessFiles.Contains(filename))
                                m_toProcessFiles.Add(filename);
                            lock (m_sd.DatFileStates)
                            {
                                m_sd.DatFileStates[filename] = new DatFileStatus();
                                if(m_cd.JobType == ConfigurationData.JobTypeEnum.Scheduled)
                                    m_sd.DatFileStates[filename].AlternativeFileDescription = m_cd.CreateHDQFileDescription(filename);
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
                            if(!m_toProcessFiles.Contains(filename))
                            {
                                m_toProcessFiles.Add(filename);
                            }
                            break;
                        case DatFileStatus.State.NO_ACCESS:
                            lock (m_candidateNewFiles)
                            {
                                if (m_candidateNewFiles.Find(delegate(Pair<string, DateTime> arg) { return arg.First == filename; }) != null)
                                    break; //file being written and already monitored
                            }
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
                        case DatFileStatus.State.INVALID_DATFILE:
                        case DatFileStatus.State.RUNNING:
                            break;
                        case DatFileStatus.State.TRIED_TOO_MANY_TIMES:
                            lock (m_sd.PermanentErrorFiles)
                            {
                                m_sd.PermanentErrorFiles.Add(filename);
                                bPermanentErrorFilesChanged = true;
                            }
                            break;
                    }
                    Thread.Sleep(0); //allow other threads like filesystemwatcher
                }
                m_sd.Changed = true;
                m_sd.PermanentErrorFilesChanged = bPermanentErrorFilesChanged;
            }
            m_sd.UpdatingFileList = false;
            m_sd.MergeProcessedAndToProcessLists();
        }


        private DatFileStatus.State ProcessDatfileReadyness(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                    return DatFileStatus.State.COMPLETED_FAILURE;
                }
                try
                {
                    FileAttributes at = File.GetAttributes(filename);
                    //Log(Logging.Level.Warning, "attributes: " + at.ToString(), filename);
                    if ((at & FileAttributes.Offline) == FileAttributes.Offline)
                    {
                        lock (m_candidateNewFiles)
                        {
                            if (m_candidateNewFiles.Find(delegate(Pair<string,DateTime> arg) { return arg.First==filename; })==null)
                                Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess3, filename);
                            //else silent error because the file is already being monitored.
                        }
                        return DatFileStatus.State.NO_ACCESS;
                    }
                }
                catch
                {
                    Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                    return DatFileStatus.State.NO_ACCESS;
                }
                IbaFile ibaDatFile = new IbaFileClass();
                Nullable<DateTime> time = null;
                try
                {
                    ibaDatFile.OpenForUpdate(filename);
                    try
                    {
                        time = File.GetLastWriteTime(filename);
                    }
                    catch
                    {
                        time = null;
                    }
                    
                }
                catch (FileLoadException) //no access
                {
                    //reason 1, because of RUNNING
                    lock (m_sd.DatFileStates)
                    {
                        if (m_sd.DatFileStates.ContainsKey(filename))
                            foreach (DatFileStatus.State stat in m_sd.DatFileStates[filename].States.Values)
                                if (stat == DatFileStatus.State.RUNNING)
                                    return DatFileStatus.State.RUNNING;
                    }
                    try
                    {
                        FileAttributes at = File.GetAttributes(filename);
                        if ((at & FileAttributes.ReadOnly) != FileAttributes.ReadOnly)
                        {
                            lock (m_candidateNewFiles)
                            {
                                if (m_candidateNewFiles.Find(delegate(Pair<string,DateTime> arg) { return arg.First==filename; })==null)
                                    Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                            }
                        }
                        else
                            Log(Logging.Level.Exception, iba.Properties.Resources.Noaccess2, filename);
                    }
                    catch
                    {
                        Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                    }
                    return DatFileStatus.State.NO_ACCESS; //no access, try again next time
                }
                catch (ArgumentException)
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.InvalidDatFile, filename);
                    return DatFileStatus.State.INVALID_DATFILE;
                }
                catch (Exception)
                {
                    //Log(Logging.Level.Exception, iba.Properties.Resources.ibaFileProblem + ex.ToString(), filename);
                    //Log(Logging.Level.Exception, iba.Properties.Resources.ibaFileProblem + ex.Message, filename);
                    Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                    return DatFileStatus.State.NO_ACCESS; //no access, try again next time
                }
                
                try
                {
                    string status = "";
                    try
                    {
                        status = ibaDatFile.QueryInfoByName("$DATCOOR_status");
                        if (string.IsNullOrEmpty(status))
                            throw new Exception("no info field present");
                    }
                    catch //old way
                    {
                        status = ibaDatFile.QueryInfoByName("status");
                        if (string.IsNullOrEmpty(status))
                            throw new Exception("no info field present");
                    }
                    if (status == "processed")
                    {
                        ibaDatFile.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
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
                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].TimesTried = timesProcessed;
                        }
                        List<string> guids = null;
                        string guidstring = "";
                        //get guids
                        try
                        {
                            try
                            {
                                guidstring = ibaDatFile.QueryInfoByName("$DATCOOR_TasksDone");
                                if (string.IsNullOrEmpty(guidstring))
                                    throw new Exception("no info field present");
                            }
                            catch //old way
                            {
                                guidstring = ibaDatFile.QueryInfoByName("TasksDone");
                            }
                        }
                        catch (ArgumentException)
                        {
                        }
                        catch (Exception) //general exception
                        {
                            Log(Logging.Level.Exception, iba.Properties.Resources.ReadStatusError, filename);
                        }
                        if (!string.IsNullOrEmpty(guidstring))
                        {
                            guidstring = guidstring.Trim(new char[] { ';' });
                            guids = new List<string>(guidstring.Split(new char[] { ';' }));
                            guids.Sort();
                        }
                        else
                            guids = new List<string>();

                        //get outputfiles
                        string outputfilesString = "";
                        try
                        {
                            outputfilesString = ibaDatFile.QueryInfoByName("$DATCOOR_OutputFiles");
                        }
                        catch (ArgumentException)
                        {
                        }
                        catch (Exception) //general exception
                        {
                            Log(Logging.Level.Exception, iba.Properties.Resources.ReadStatusError, filename);
                        }
                        SortedList<string, string> keyvalues = null;
                        if (!string.IsNullOrEmpty(outputfilesString))
                        {
                            outputfilesString = outputfilesString.Trim(new char[] { ';' });
                            List<string> outputfilesKeyValuesString = new List<string>(outputfilesString.Split(new char[] { ';' }));
                            keyvalues = new SortedList<string, string>();
                            foreach (string pairString in outputfilesKeyValuesString)
                            {
                                string[] splitted = pairString.Split(new char[] { '|' });
                                keyvalues.Add(splitted[0], splitted[1]);
                            }
                        }
                        else
                        {
                            keyvalues = new SortedList<string, string>();
                        }

                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].OutputFiles.Clear();
                            foreach (TaskData task in m_cd.Tasks)
                            {
                                string key = task.Guid.ToString();
                                try
                                {
                                    if (guids.BinarySearch(key) >= 0)
                                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                                    if (keyvalues.ContainsKey(key))
                                        m_sd.DatFileStates[filename].OutputFiles.Add(task, keyvalues[key]);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("location1", ex);
                                }
                            }
                        }
                        ibaDatFile.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
                        if (m_cd.LimitTimesTried && timesProcessed >= m_cd.NrTryTimes)
                        {
                            try { if (time != null) File.SetLastWriteTime(filename, time.Value); }
                            catch { }
                            Log(Logging.Level.Warning, iba.Properties.Resources.TriedToManyTimes, filename);
                            return DatFileStatus.State.TRIED_TOO_MANY_TIMES;
                        }
                        try
                        {
                            if (time != null) File.SetLastWriteTime(filename, time.Value);
                        }
                        catch
                        {
                        }
                        return DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    else if (status == "restart" || status == "readyToProcess")
                    {
                        ibaDatFile.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
                        return DatFileStatus.State.NOT_STARTED;
                    }
                    else if (status == "")
                    {
                        throw new Exception("no info field present");
                    }
                    else
                    {
                        ibaDatFile.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
                        return DatFileStatus.State.COMPLETED_FAILURE;
                    }
                }
                catch //no status field
                {
                    ibaDatFile.Close();
                    ibaDatFile.OpenForUpdate(filename);

                    String frames = null;
                    try
                    {
                        frames = ibaDatFile.QueryInfoByName("frames");
                    }
                    catch
                    {
                    }
                    //modification: do not consider an emtpy frames field as 'still busy', in QDR 1 files it could mean they are simply missing.
                    if (/*String.IsNullOrEmpty(frames)*/ frames == null || frames == "1000000000")
                    {
                        try { 
                            ibaDatFile.Close();
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
                        }
                        catch { }
                        Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess3, filename);
                        //try { if (time != null) File.SetLastWriteTime(filename, time.Value); }catch {} 
                        return DatFileStatus.State.NO_ACCESS;
                    }
                    ibaDatFile.WriteInfoField("$DATCOOR_status", "readyToProcess");
                    ibaDatFile.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
                    try 
                    { 
                        if (time != null) File.SetLastWriteTime(filename, time.Value); 
                    }
                    catch 
                    {
                    }
                    return DatFileStatus.State.NOT_STARTED;
                }
            }
            catch //general exception that may have happened
            {
                Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                return DatFileStatus.State.COMPLETED_FAILURE;
            }
        }

        private DatFileStatus.State ProcessHDQfileReadyness(string filename)
        {
            try
            {
                if(!File.Exists(filename))
                {
                    Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                    return DatFileStatus.State.COMPLETED_FAILURE;
                }

                IniParser iniParser = new IniParser(filename);
                Nullable<DateTime> time = null;
                if (!iniParser.Read())
                {
                    Log(Logging.Level.Exception, "Could not read HDQ file", filename);
                    return DatFileStatus.State.COMPLETED_FAILURE;
                }
                try
                {
                    time = File.GetLastWriteTime(filename);
                }
                catch
                {
                    time = null;
                }

                var Section = iniParser.Sections["DatCoordinatorData"];

                try
                {
                    string status = "";
                    try
                    {
                        status = Section["$DATCOOR_status"];
                        if(string.IsNullOrEmpty(status))
                            throw new Exception("no info field present");
                    }
                    catch //old way
                    {
                        status = Section["status"];
                        if(string.IsNullOrEmpty(status))
                            throw new Exception("no info field present");
                    }
                    if(status == "processed")
                    {
                        try { if(time != null) File.SetLastWriteTime(filename, time.Value); }
                        catch { }
                        return DatFileStatus.State.COMPLETED_SUCCESFULY;
                    }
                    else if(status == "processingfailed")
                    {
                        int timesProcessed = 1;
                        try
                        {
                            string timesString = Section["$DATCOOR_times_tried"];
                            timesProcessed = int.Parse(timesString);
                        }
                        catch
                        {
                        }
                        lock(m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].TimesTried = timesProcessed;
                        }
                        List<string> guids = null;
                        string guidstring = Section["$DATCOOR_TasksDone"];
                        if(!string.IsNullOrEmpty(guidstring))
                        {
                            guidstring = guidstring.Trim(new char[] { ';' });
                            guids = new List<string>(guidstring.Split(new char[] { ';' }));
                            guids.Sort();
                        }
                        else
                            guids = new List<string>();

                        //get outputfiles
                        string outputfilesString =  Section["$DATCOOR_OutputFiles"];
                        SortedList<string, string> keyvalues = null;
                        if(!string.IsNullOrEmpty(outputfilesString))
                        {
                            outputfilesString = outputfilesString.Trim(new char[] { ';' });
                            List<string> outputfilesKeyValuesString = new List<string>(outputfilesString.Split(new char[] { ';' }));
                            keyvalues = new SortedList<string, string>();
                            foreach(string pairString in outputfilesKeyValuesString)
                            {
                                string[] splitted = pairString.Split(new char[] { '|' });
                                keyvalues.Add(splitted[0], splitted[1]);
                            }
                        }
                        else
                        {
                            keyvalues = new SortedList<string, string>();
                        }

                        lock(m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].OutputFiles.Clear();
                            foreach(TaskData task in m_cd.Tasks)
                            {
                                string key = task.Guid.ToString();
                                try
                                {
                                    if(guids.BinarySearch(key) >= 0)
                                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                                    if(keyvalues.ContainsKey(key))
                                        m_sd.DatFileStates[filename].OutputFiles.Add(task, keyvalues[key]);
                                }
                                catch(Exception ex)
                                {
                                    throw new Exception("location1", ex);
                                }
                            }
                        }
                        if(m_cd.LimitTimesTried && timesProcessed >= m_cd.NrTryTimes)
                        {
                            try { if(time != null) File.SetLastWriteTime(filename, time.Value); }
                            catch { }
                            Log(Logging.Level.Warning, iba.Properties.Resources.TriedToManyTimes, filename);
                            return DatFileStatus.State.TRIED_TOO_MANY_TIMES;
                        }
                        try
                        {
                            if(time != null) File.SetLastWriteTime(filename, time.Value);
                        }
                        catch
                        {
                        }
                        return DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    else if(status == "restart" || status == "readyToProcess")
                    {
                        return DatFileStatus.State.NOT_STARTED;
                    }
                    else if(status == "")
                    {
                        throw new Exception("no info field present");
                    }
                    else
                    {
                        return DatFileStatus.State.COMPLETED_FAILURE;
                    }
                }
                catch //no status field
                {
                    Section["$DATCOOR_status"] = "readyToProcess";
                    iniParser.Write();
                    return DatFileStatus.State.NOT_STARTED;
                }
            }
            catch (Exception ex) //general exception that may have happened
            {
                Log(Logging.Level.Exception, "General error accessing HDQ file:" + ex.Message, filename);
                return DatFileStatus.State.COMPLETED_FAILURE;
            }
        }

        private DatFileStatus.State ProcessDatfileReadynessOneTimeJob(string filename)
        {
            try
            {
                if (!File.Exists(filename))
                {
                    return DatFileStatus.State.COMPLETED_FAILURE;
                }
                try
                {
                    FileAttributes at = File.GetAttributes(filename);
                    //Log(Logging.Level.Warning, "attributes: " + at.ToString(), filename);
                    if ((at & FileAttributes.Offline) == FileAttributes.Offline)
                    {
                        Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess3, filename);
                        return DatFileStatus.State.NO_ACCESS;
                    }
                }
                catch (Exception)
                {
                    //Log(Logging.Level.Warning, "Exception at location 1: " + ex.ToString(), filename);
                    Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                    return DatFileStatus.State.NO_ACCESS;
                }
                
                string frames = null;
                IbaFile ibaDatFile = new IbaFileClass();
                ibaDatFile.Open(filename);
                try
                {
                    frames = ibaDatFile.QueryInfoByName("frames");
                    ibaDatFile.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
                }
                catch 
                {
                }

                //modification: do not consider an emtpy frames field as 'still busy', in QDR 1 files it could mean they are simply missing.
                if (/*String.IsNullOrEmpty(frames)*/ frames == null || frames == "1000000000")
                {
                    try
                    {
                        ibaDatFile.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
                    }
                    catch
                    {
                    }
                    Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess3, filename);
                    return DatFileStatus.State.NO_ACCESS;
                }
            }
            catch (Exception) //general exception that may have happened
            {
                //Log(Logging.Level.Warning, "Exception at location 2: " + ex2.ToString(), filename);
                Log(Logging.Level.Warning, iba.Properties.Resources.Noaccess, filename);
                return DatFileStatus.State.COMPLETED_FAILURE;
            }
            return DatFileStatus.State.NOT_STARTED;
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

        private DateTime? m_startTimeFromDatFile;

        private void ProcessDatfile(string InputFile) 
        {
            if (m_needIbaAnalyzer)
            {
                if (m_ibaAnalyzer == null) StartIbaAnalyzer(); //safety, ibaAnalyzer should be present
                if (m_ibaAnalyzer == null) return;
            }
            m_startTimeFromDatFile = null;

            lock (m_listUpdatingLock)
            {
                lock (m_processedFiles)
                {
                    if( ! m_processedFiles.Contains(InputFile))
                    m_processedFiles.Add(InputFile);
                }
              
                try
                {
                    if (m_cd.JobType == ConfigurationData.JobTypeEnum.DatTriggered) //exclusive access required when getting from PDA
                    {
                        FileStream fs = new FileStream(InputFile, FileMode.Open, FileAccess.Write, FileShare.None);
                        fs.Close();
                        fs.Dispose();
                    }
                    if(m_needIbaAnalyzer)
                    {
                        m_ibaAnalyzer.OpenDataFile(0, InputFile); //works both with hdq and .dat
                        try
                        {
                            DateTime dt = new DateTime();
                            int microsec = 0;
                            m_ibaAnalyzer.GetStartTime(ref dt, ref microsec);
                            dt.AddTicks(microsec * 10);
                            m_startTimeFromDatFile = dt;
                        }
                        catch
                        {
                            m_startTimeFromDatFile = null;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log(Logging.Level.Exception,ex.Message);
                    if (!m_needIbaAnalyzer) return;
                    try
                    {
                        m_ibaAnalyzer.CloseDataFiles();
                        StopIbaAnalyzer();
                    }
                    catch
                    {
                        Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, InputFile);
                    }
                    return;
                }

                bool completeSucces = true;
                foreach (TaskData task in m_cd.Tasks)
                {
                    if (!shouldTaskBeDone(task, InputFile))
                    {
                        if (task.WhenToExecute != TaskData.WhenToDo.DISABLED && task is TaskDataUNC)
                        {
                            TaskDataUNC tunc = task as TaskDataUNC;
                            if (tunc.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.SaveFreeSpace)
                                DoCleanupAnyway(InputFile, tunc);
                        }

                        //set the outputfile of previous run if any for the next batchtask
                        lock (m_sd.DatFileStates)
                        {
                            if(m_sd.DatFileStates.ContainsKey(InputFile))
                            {
                                string outFile;
                                if(m_sd.DatFileStates[InputFile].OutputFiles.TryGetValue(task, out outFile))
                                {
                                    if(task is ExtractData)
                                    {
                                        string ext = ExtractTaskWorker.GetSecondaryExtractExtension(task as ExtractData);
                                        if(!String.IsNullOrEmpty(ext) && ext != ".mp4") //mp4 is more than one file -> don't support
                                        {
                                            string outFile2 = Path.ChangeExtension(outFile, ext);
                                            m_outPutFilesPrevTask = new string[] { outFile, outFile2 };
                                        }
                                        else
                                        {
                                            m_outPutFilesPrevTask = new string[] { outFile };
                                        }
                                    }
                                    else
                                    {
                                        m_outPutFilesPrevTask = new string[] { outFile };
                                    }
                                }
                            }
                        }
                        continue;
                    }
                    if (!(task is BatchFileData) && !(task is CopyMoveTaskData))
                    {
                        m_outPutFilesPrevTask = null;
                    }
                    bool failedOnce = false;
                    bool continueProcessing = true;
                    if (task.ResourceIntensive)
                    {
                        using (TaskManager.Manager.CriticalTaskSemaphore.CreateClient())
                        {
                            if (m_stop)
                            {
                                completeSucces = false;
                                break;
                            }
                            continueProcessing = ProcessTask(InputFile, task, ref failedOnce);
                        }
                    }
                    else
                        continueProcessing = ProcessTask(InputFile, task, ref failedOnce);
                    if (!continueProcessing) return;

                    //touch the file
                    if (m_outPutFilesPrevTask != null)
                    {
                        foreach (string outputfile in m_outPutFilesPrevTask)
                        {
                            if(string.IsNullOrEmpty(outputfile) || !File.Exists(outputfile)) continue;
                            TaskDataUNC tunc = task as TaskDataUNC;
                            if (tunc != null && tunc.CopyModTime)
                            {
                                try
                                {
                                    DateTime utcTime = File.GetLastWriteTimeUtc(InputFile);
                                    File.SetLastWriteTimeUtc(outputfile, utcTime);
                                }
                                catch (System.Exception ex)
                                {
                                   Log(iba.Logging.Level.Warning, iba.Properties.Resources.WriteTimeModificationFailed + ex.Message, InputFile, task);
                                }
                            }
                        }
                    }

                    completeSucces = completeSucces && DoNotification(InputFile, task, failedOnce);

                    if (m_stop)
                    {
                        if (task.Index != m_cd.Tasks.Count-1) completeSucces = false;
                        break;
                    }
                }

                if (m_needIbaAnalyzer && m_ibaAnalyzer != null)
                {
                    try
                    {
                        m_ibaAnalyzer.CloseDataFiles();
                    }
                    catch
                    {
                        Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, InputFile);
                        StopIbaAnalyzer();
                    }
                }

                lock(m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[InputFile].TimesTried++;
                }
                if (m_cd.JobType == ConfigurationData.JobTypeEnum.DatTriggered) // written in the .dat file
                {
                    WriteStateInDatFile(InputFile, completeSucces);
                }
                else if(m_cd.JobType == ConfigurationData.JobTypeEnum.Scheduled)
                {
                    WriteStateInHDQFile(InputFile, completeSucces);
                }
                m_sd.Changed = true;
            }
        }

        private void WriteStateInDatFile(string InputFile, bool completeSucces)
        {
            Nullable<DateTime> time = null;
            try
            {
                IbaFile ibaDatFile = new IbaFileClass();
                try
                {
                    ibaDatFile.OpenForUpdate(InputFile);
                    time = File.GetLastWriteTime(InputFile);
                }
                catch //happens when timed out and proc has not released its resources yet
                {
                    m_sd.Changed = true;
                    return;
                }

                if(completeSucces)
                {
                    ibaDatFile.WriteInfoField("$DATCOOR_status", "processed");
                    ibaDatFile.WriteInfoField("$DATCOOR_OutputFiles", "");//erase any previous outputfiles;
                }
                else
                {
                    ibaDatFile.WriteInfoField("$DATCOOR_status", "processingfailed");
                    //write GUIDs of those that were succesfull
                    lock(m_sd.DatFileStates)
                    {
                        string guids = "";
                        string outputfiles = "";
                        foreach(KeyValuePair<TaskData, DatFileStatus.State> stat in m_sd.DatFileStates[InputFile].States)
                            if(stat.Value == DatFileStatus.State.COMPLETED_SUCCESFULY)
                            {
                                guids += stat.Key.Guid.ToString() + ";";
                                if(m_sd.DatFileStates[InputFile].OutputFiles.ContainsKey(stat.Key))
                                    outputfiles += stat.Key.Guid.ToString() + "|" + m_sd.DatFileStates[InputFile].OutputFiles[stat.Key] + ";";
                            }
                        ibaDatFile.WriteInfoField("$DATCOOR_TasksDone", guids);
                        ibaDatFile.WriteInfoField("$DATCOOR_OutputFiles", outputfiles);
                    }
                }

                lock(m_sd.DatFileStates)
                {
                    ibaDatFile.WriteInfoField("$DATCOOR_times_tried", m_sd.DatFileStates[InputFile].TimesTried.ToString());
                }
                ibaDatFile.Close();
                System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
            }
            catch(Exception ex)
            {
                Log(iba.Logging.Level.Exception, iba.Properties.Resources.DatFileCloseFailed + ex.Message, InputFile);
                lock(m_sd.DatFileStates)
                {
                    foreach(TaskData t in m_cd.Tasks)
                        m_sd.DatFileStates[InputFile].States[t] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
            if(time != null)
            {
                try
                {
                    File.SetLastWriteTime(InputFile, time.Value);
                }
                catch(Exception)
                {
                }
            }
        }

        private void WriteStateInHDQFile(string InputFile, bool completeSucces)
        {
            Nullable<DateTime> time = null;
            try
            {
                IniParser iniParser;
                try
                {
                    iniParser = new IniParser(InputFile);
                    iniParser.Read();
                    time = File.GetLastWriteTime(InputFile);
                }
                catch //happens when timed out and proc has not released its resources yet
                {
                    m_sd.Changed = true;
                    return;
                }

                var Section = iniParser.Sections["DatCoordinatorData"];

                if(completeSucces)
                {
                    File.Delete(InputFile); //hdq files get deleted
                    //Section["$DATCOOR_status"]="processed";
                    //Section["$DATCOOR_OutputFiles"]= ""; //erase any previous outputfiles;
                    return;
                }
                else
                {
                    Section["$DATCOOR_status"] = "processingfailed";
                    //write GUIDs of those that were succesfull
                    lock(m_sd.DatFileStates)
                    {
                        string guids = "";
                        string outputfiles = "";
                        foreach(KeyValuePair<TaskData, DatFileStatus.State> stat in m_sd.DatFileStates[InputFile].States)
                            if(stat.Value == DatFileStatus.State.COMPLETED_SUCCESFULY)
                            {
                                guids += stat.Key.Guid.ToString() + ";";
                                if(m_sd.DatFileStates[InputFile].OutputFiles.ContainsKey(stat.Key))
                                    outputfiles += stat.Key.Guid.ToString() + "|" + m_sd.DatFileStates[InputFile].OutputFiles[stat.Key] + ";";
                            }
                        Section["$DATCOOR_TasksDone"]=guids;
                        Section["$DATCOOR_OutputFiles"] = outputfiles;
                        Section["$DATCOOR_times_tried"] = m_sd.DatFileStates[InputFile].TimesTried.ToString();
                    }
                }
                iniParser.Write();
            }
            catch(Exception ex)
            {
                Log(iba.Logging.Level.Exception, iba.Properties.Resources.DatFileCloseFailed + ex.Message, InputFile);
                lock(m_sd.DatFileStates)
                {
                    foreach(TaskData t in m_cd.Tasks)
                        m_sd.DatFileStates[InputFile].States[t] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
            if(time != null)
            {
                try
                {
                    File.SetLastWriteTime(InputFile, time.Value);
                }
                catch(Exception)
                {
                }
            }
        }

        private bool ProcessTask(string DatFile, TaskData task, ref bool failedOnce)
        {
            lock (m_sd.DatFileStates)
            {
                failedOnce = m_sd.DatFileStates.ContainsKey(DatFile)
                    && m_sd.DatFileStates[DatFile].States.ContainsKey(task)
                    && DatFileStatus.IsError(m_sd.DatFileStates[DatFile].States[task])
                    && m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.NO_ACCESS; //all errors except no access
            }
            bool continueProcessing = true;
            if (task is ReportData)
            {
                Report(DatFile, task as ReportData);
                IbaAnalyzerCollection.Collection.AddCall(m_ibaAnalyzer);
            }
            else if (task is ExtractData)
            {
                Extract(DatFile, task as ExtractData);
                IbaAnalyzerCollection.Collection.AddCall(m_ibaAnalyzer);               
            }
            else if (task is BatchFileData)
            {
                Batchfile(DatFile, task as BatchFileData);
            }
            else if (task is IfTaskData)
            {
                IfTask(DatFile, task as IfTaskData);
                IbaAnalyzerCollection.Collection.AddCall(m_ibaAnalyzer);
            }
            else if (task is UpdateDataTaskData)
            {
                UpdateDataTask(DatFile, task as UpdateDataTaskData);
            }
            else if (task is PauseTaskData)
            {
                PauseTask(DatFile, task as PauseTaskData);
            }
            else if (task is CopyMoveTaskData)
            {
                CopyMoveTaskData dat = task as CopyMoveTaskData;
                if (dat.RemoveSource && dat.WhatFile == CopyMoveTaskData.WhatFileEnumA.DATFILE)
                {
                    try
                    {
                        if (m_needIbaAnalyzer && m_ibaAnalyzer != null) m_ibaAnalyzer.CloseDataFiles();
                    }
                    catch
                    {
                        Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, DatFile, task);
                        if (m_needIbaAnalyzer)
                        {
                            RestartIbaAnalyzer();
                        }
                    }
                    CopyDatFile(DatFile, dat);
                    if (m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_SUCCESFULY && task.Index != m_cd.Tasks.Count - 1) //was not last task
                    {
                        Log(Logging.Level.Warning, iba.Properties.Resources.logNextTasksIgnored, DatFile, task);
                        m_sd.Changed = true;
                        DoNotification(DatFile, task, failedOnce);
                        continueProcessing = false;
                    }
                    else if (m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.COMPLETED_SUCCESFULY)
                    {
                        try
                        {
                            if (m_needIbaAnalyzer) m_ibaAnalyzer.OpenDataFile(0, DatFile);
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
                                Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, DatFile, task);
                                if (m_needIbaAnalyzer)
                                {
                                    RestartIbaAnalyzer();
                                }
                            }
                            DoNotification(DatFile, task, failedOnce);
                            continueProcessing = false;
                        }
                    }
                    else
                    {
                        DoNotification(DatFile, task, failedOnce);
                        continueProcessing = false;
                    }
                }
                else
                    CopyDatFile(DatFile, dat);
            }
            else if (task is CustomTaskData)
            {
                DoCustomTask(DatFile, task as CustomTaskData);
            }
            else if (task is CustomTaskDataUNC)
            {
                DoCustomTaskUNC(DatFile, task as CustomTaskDataUNC);
            }
            if (m_needIbaAnalyzer && m_ibaAnalyzer == null) return false;
            return continueProcessing;
        }

        internal bool RestartIbaAnalyzerAndOpenDatFile(string datfile)
        {
            RestartIbaAnalyzer();
            if (m_ibaAnalyzer == null) return false;
            try
            {
                if (!m_cd.OnetimeJob)
                {
                    FileStream fs = new FileStream(datfile, FileMode.Open, FileAccess.Write, FileShare.None);
                    fs.Close();
                    fs.Dispose();
                }
                m_ibaAnalyzer.OpenDataFile(0, datfile);
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
                    Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, datfile);
                    RestartIbaAnalyzer();
                }
                return false;
            }
            return true;
        }

        private bool DoNotification(string DatFile, TaskData task, bool failedOnce)
        {
            bool completeSucces = true;
            lock (m_sd.DatFileStates)
            {
                if (m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.COMPLETED_SUCCESFULY
                    && m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.COMPLETED_TRUE
                    && m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.COMPLETED_FALSE)
                    completeSucces = false;

                if ((m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_SUCCESFULY ||
                    m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_TRUE)
                    && (task.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES || task.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE))
                {
                    lock (m_notifier)
                    {
                        m_notifier.AddSuccess(task, DatFile);
                        if (m_cd.NotificationData.NotifyImmediately)
                            m_notifier.Send();
                        else if (DateTime.Now - m_notifier.LastSendTime > m_cd.NotificationData.TimeInterval)
                        {
                            //restart timer;
                            if (m_notifyTimer == null)
                                m_notifyTimer = new System.Threading.Timer(OnNotifyTimerTick);
                            m_notifyTimer.Change(Timeout.Infinite, Timeout.Infinite);
                            m_notifier.Send();
                            if (!m_bTimersstopped && !m_stop)
                                m_notifyTimer.Change(m_cd.NotificationData.TimeInterval, TimeSpan.Zero);
                        }
                    }
                }
                else if ((((DatFileStatus.IsError(m_sd.DatFileStates[DatFile].States[task])
                                && m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.NO_ACCESS
                                && m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.TRIED_TOO_MANY_TIMES))
                            || m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_FALSE)
                        && (task.WhenToNotify == TaskData.WhenToDo.AFTER_FAILURE || task.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE
                            || (task.WhenToNotify == TaskData.WhenToDo.AFTER_1st_FAILURE && !failedOnce)))
                {
                    lock (m_notifier)
                    {
                        m_notifier.AddFailure(task, DatFile);
                        if (m_cd.NotificationData.NotifyImmediately)
                            m_notifier.Send();
                        else if (DateTime.Now - m_notifier.LastSendTime > m_cd.NotificationData.TimeInterval)
                        {
                            //restart timer;
                            if (m_notifyTimer == null)
                                m_notifyTimer = new System.Threading.Timer(OnNotifyTimerTick);
                            m_notifyTimer.Change(Timeout.Infinite, Timeout.Infinite);
                            m_notifier.Send();
                            if (!m_bTimersstopped && !m_stop)
                                m_notifyTimer.Change(m_cd.NotificationData.TimeInterval, TimeSpan.Zero);
                        }
                    }
                }
            } 
            return completeSucces;
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
                lock (m_licensedTasks)
                {
                    bool licensed = false;
                    m_licensedTasks.TryGetValue(task,out licensed);
                    if (!licensed)
                    {
                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[DatFile].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                        }
                        Log(Logging.Level.Exception, String.Format(iba.Properties.Resources.logCustomTaskNotLicensed, task.Name));
                        return;
                    }
                }

                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[DatFile].States[task] = DatFileStatus.State.RUNNING;
                }
                string message = string.Format(iba.Properties.Resources.logTaskStarted, task.Plugin.NameInfo);
                Log(Logging.Level.Info, message, DatFile, task);

                bool succes = task.Plugin.GetWorker().ExecuteTask(DatFile);
                if (succes)
                {
                    //code on succes
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[DatFile].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                    }
                    message = string.Format(iba.Properties.Resources.logTaskSuccess, task.Plugin.NameInfo);
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

        internal string[] m_outPutFilesPrevTask;

        void Extract(string filename, ExtractData task)
        {
            (new ExtractTaskWorker(this, task)).DoWork(filename);
        }

        internal string GetOutputDirectoryName(string filename, TaskDataUNC task)
        {
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

            string maindir = dir;

            if (dir == m_cd.DatDirectoryUNC)
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logOutputIsInput, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return null;
            }

            if (m_cd.SubDirs && task.Subfolder == TaskDataUNC.SubfolderChoice.SAME)
            {   //concatenate subfolder corresponding to dat subfolder
                string s2 = Path.GetFullPath(m_cd.DatDirectoryUNC);
                string s1 = Path.GetFullPath(filename);
                string s0 = s2.EndsWith(@"\") ? s1.Remove(0, s2.Length) : s1.Remove(0, s2.Length + 1);
                dir = Path.GetDirectoryName(Path.Combine(dir, s0));
            }
            if (task.Subfolder != TaskDataUNC.SubfolderChoice.NONE
                && task.Subfolder != TaskDataUNC.SubfolderChoice.SAME
                && task.Subfolder != TaskDataUNC.SubfolderChoice.INFOFIELD)
            {
                dir = Path.Combine(dir, TimeBasedSubFolder(task,filename));
            }
            else if(task.Subfolder == TaskDataUNC.SubfolderChoice.INFOFIELD)
            {
                dir = Path.Combine(dir, InfoFieldBasedSubFolder(task, filename));
            }
            return dir;
        }


        internal string GetOutputFileName(TaskDataUNC task, string filename)
        {
            if (task.UseInfoFieldForOutputFile)
            {
                IbaFile ibaDatFile = new IbaFileClass();
                string outputfile;
                try
                {
                    ibaDatFile.Open(filename);
                    outputfile = ibaDatFile.QueryInfoByName(task.InfoFieldForOutputFile);
                    if (task.InfoFieldForOutputFileLength == 0)
                    {
                        if (task.InfoFieldForOutputFileStart != 0)
                        {
                            outputfile = outputfile.Substring(task.InfoFieldForOutputFileStart);
                        }
                    }
                    else
                        outputfile = outputfile.Substring(task.InfoFieldForOutputFileStart, task.InfoFieldForOutputFileLength);
                    if (task.InfoFieldForOutputFileRemoveBlanksAll)
                        outputfile = outputfile.Replace(" ", String.Empty).Replace("\t", String.Empty);
                    else if (task.InfoFieldForOutputFileRemoveBlanksEnd)
                        outputfile = outputfile.TrimEnd(null);
                    outputfile = CPathCleaner.CleanFile(outputfile);
                }
                catch
                {
                    outputfile = "";
                }
                finally
                {
                    try
                    {
                        ibaDatFile.Close();
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
                    }
                    catch
                    {

                    }
                }
                if (!string.IsNullOrEmpty(outputfile)) return outputfile;
                //warn that we failed getting the infofield
                string message = string.Format(iba.Properties.Resources.WarningInfofieldFailed, task.InfoFieldForOutputFile);
                Log(iba.Logging.Level.Warning, message, filename, task);
            }
            return Path.GetFileNameWithoutExtension(filename);
        }

        private String TimeBasedSubFolder(TaskDataUNC task, String filename)
        {
            DateTime dt = DateTime.Now;
            switch (task.DirTimeChoice)
            {
                case TaskDataUNC.DirTimeChoiceEnum.InFile:
                    if(m_startTimeFromDatFile.HasValue)
                        dt = m_startTimeFromDatFile.Value;
                    else
                        goto case TaskDataUNC.DirTimeChoiceEnum.Modified;
                    break;
                case TaskDataUNC.DirTimeChoiceEnum.Modified:
                    try
                    {
                        dt = File.GetLastWriteTime(filename);
                    }
                    catch
                    {
                        dt = DateTime.Now;
                    }
                    break;
                default: 
                    break;
            }
            return task.GetSubDir(dt);
        }

        private String InfoFieldBasedSubFolder(TaskDataUNC task, String filename)
        {
            IbaFile ibaDatFile = new IbaFileClass();
            string Subdir = "";
            try
            {
                ibaDatFile.Open(filename);
                Subdir = ibaDatFile.QueryInfoByName(task.InfoFieldForSubdir);
                if(task.InfoFieldForSubdirLength == 0)
                {
                    if(task.InfoFieldForSubdirStart != 0)
                    {
                        Subdir = Subdir.Substring(task.InfoFieldForSubdirStart);
                    }
                }
                else
                    Subdir = Subdir.Substring(task.InfoFieldForSubdirStart, task.InfoFieldForSubdirLength);
                if(task.InfoFieldForSubdirRemoveBlanksAll)
                    Subdir = Subdir.Replace(" ", String.Empty).Replace("\t", String.Empty);
                else if(task.InfoFieldForSubdirRemoveBlanksEnd)
                    Subdir = Subdir.TrimEnd(null);
            }
            catch
            {
                Subdir = "";
            }
            finally
            {
                try
                {
                    ibaDatFile.Close();
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaDatFile);
                }
                catch
                {

                }
            }
            if(!string.IsNullOrEmpty(Subdir))
            {
                Subdir = CPathCleaner.CleanDirectory(Subdir);
                return Subdir;
            }
            Subdir = "unresolved";
            //warn that we failed getting the infofield
            string message = string.Format(iba.Properties.Resources.WarningInfofieldDirFailed, task.InfoFieldForSubdir);
            Log(iba.Logging.Level.Warning, message, filename, task);
            return Subdir;
        }

        private void Report(string filename, ReportData task)
        {
            string arg = "";
            string htmloutputdir = "";
            string htmlext = "";
            lock (m_sd.DatFileStates)
            {
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.RUNNING;
            }
            if (task.Output != ReportData.OutputChoice.PRINT)
            {
                string ext = "." + task.Extension;
                try
                {
                    if (task.UsesQuota)
                    {
                        if (task.Extension == "html" || task.Extension == "htm")
                        {
                            htmlext = ext + ",*.jpg";
                            CleanupWithQuota(filename, task, htmlext ); //also remove generated jpgs
                        }
                        else
                            CleanupWithQuota(filename, task, ext);
                    }
                }
                catch
                {
                }

                string actualFileName = GetOutputFileName(task,filename);
                string dir = task.DestinationMapUNC;
                try
                {
                    dir = GetOutputDirectoryName(filename, task);
                    if(dir == null) return;
                    if(!Directory.Exists(dir))
                    {
                        try
                        {
                            Directory.CreateDirectory(dir);
                        }
                        catch
                        {
                            bool failed = true;
                            if(SharesHandler.Handler.TryReconnect(dir, task.Username, task.Password))
                            {
                                failed = false;
                                if(!Directory.Exists(dir))
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
                            if(failed)
                            {
                                Log(Logging.Level.Exception, iba.Properties.Resources.logCreateDirectoryFailed + ": " + dir, filename, task);
                                lock(m_sd.DatFileStates)
                                {
                                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                                }
                                return;
                            }
                        }
                        //new directory created, do directory cleanup if that is the setting
                        if(task.Subfolder != TaskDataUNC.SubfolderChoice.NONE && task.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories)
                            task.DoDirCleanupNow = true;
                    }
                    if(task.DoDirCleanupNow)
                    {
                        try
                        {
                            CleanupDirs(filename, task, ext);
                        }
                        catch
                        {
                        }
                        task.DoDirCleanupNow = false;
                    }
                }
                catch(Exception ex) //sort of unexpected error;
                {
                    Log(Logging.Level.Exception, ex.Message, filename, task);
                    lock(m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    return;
                }



                if (task.Extension == "html" || task.Extension == "htm")
                {
                    htmloutputdir = Path.Combine(dir, actualFileName);
                    if (!task.OverwriteFiles)
                    {
                        for (int index = 0; Directory.Exists(htmloutputdir); index++)
                        {
                            string indexstr = index.ToString("d2");
                            htmloutputdir = Path.Combine(dir, actualFileName + '_' + indexstr);
                        }
                    }
                    dir = htmloutputdir;
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
                    if (task.Subfolder != ReportData.SubfolderChoice.NONE && task.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories)
                        task.DoDirCleanupNow = true;                    
                }
                if (task.DoDirCleanupNow)
                {
                    try
                    {
                        if (task.Extension == "html" || task.Extension == "htm")
                            CleanupDirsMultipleOutputFiles(filename, task, htmlext);
                        else
                            CleanupDirs(filename, task, ext);
                    }
                    catch
                    {
                    }
                    task.DoDirCleanupNow = false;
                }
                arg = Path.Combine(dir, actualFileName + ext);
                if (!task.OverwriteFiles)
                    arg = DatCoordinatorHostImpl.Host.FindSuitableFileName(arg);
            }

            if (!File.Exists(task.AnalysisFile))
            {
                string message = iba.Properties.Resources.AnalysisFileNotFound + task.AnalysisFile;
                Log(Logging.Level.Exception, message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return;
            }

            try
            {
                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, task.MonitorData))
                {
                    mon.Execute(delegate() { m_ibaAnalyzer.OpenAnalysis(task.AnalysisFile); });
                    Log(Logging.Level.Info, iba.Properties.Resources.logReportStarted, filename, task);
                    if (task.Output != ReportData.OutputChoice.PRINT)
                    {
                        if (task.OverwriteFiles && task.UsesQuota && File.Exists(arg))
                        {
                            if (task.Extension == "html" || task.Extension == "htm")
                            {
                                List<string> files = PathUtil.GetFilesMultipleExtensions(htmloutputdir,"*" + htmlext);
                                foreach(string file in files)
                                {
                                    try
                                    {
                                        m_quotaCleanups[task.Guid].RemoveFile(file);
                                        File.Delete(file);
                                    }
                                    catch
                                    {

                                    }
                                }
                            }
                            else
                                m_quotaCleanups[task.Guid].RemoveFile(arg);
                        }
                        mon.Execute(delegate() { m_ibaAnalyzer.Report(arg); });
                        m_outPutFilesPrevTask = new string[] { arg };
                        if (task.UsesQuota)
                        {
                            if (task.Extension == "html" || task.Extension == "htm")
                            {
                                List<string> files = PathUtil.GetFilesMultipleExtensions(htmloutputdir, "*" + htmlext);
                                foreach(string file in files)
                                {
                                    m_quotaCleanups[task.Guid].AddFile(file);
                                }
                            }
                            else
                                m_quotaCleanups[task.Guid].AddFile(m_outPutFilesPrevTask[0]);
                        }
                    }
                    else
                        mon.Execute(delegate() { m_ibaAnalyzer.Report(""); });
                    //Thread.Sleep(500);
                    //code on succes
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                        if (m_outPutFilesPrevTask == null)
                            m_sd.DatFileStates[filename].OutputFiles[task] = null;
                        else
                            m_sd.DatFileStates[filename].OutputFiles[task] = m_outPutFilesPrevTask[0];
                    }
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
                RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                Log(Logging.Level.Exception, me.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.MEMORY_EXCEEDED;
                }
                RestartIbaAnalyzerAndOpenDatFile(filename);
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
                        RestartIbaAnalyzer();
                    }
                }
            }
        }

        internal void CleanupDirsMultipleOutputFiles(string filename, TaskDataUNC task, string extension)
        {
            //html and .dat file with external video creates an additional subdirectory, only directories directly above it should count
            try
            {
                string rootmap = task.DestinationMapUNC;
                List<string> subdirs = GetDeepestDirectories(rootmap);
                
                //group by parentdir
                Set<string> parentdirs = new Set<string>();
                foreach (string subdir in subdirs)
                {
                    parentdirs.Add((new DirectoryInfo(subdir)).Parent.FullName);
                }

                if (parentdirs.Count <= task.SubfoldersNumber) return; //everything ok
                if (parentdirs.Count == task.SubfoldersNumber + 1) //optimization, don't sort the list, only remove oldest directory
                {
                    string oldestdir = null;
                    DateTime dtoldest = new DateTime();
                    foreach (string dirC in parentdirs)
                    {
                        DateTime dt = (new DirectoryInfo(dirC)).LastWriteTime;
                        if (oldestdir == null || dt < dtoldest)
                        {
                            oldestdir = dirC;
                            dtoldest = dt;
                        }
                    }
                    if (oldestdir != null)
                    {
                        string[] subdirs2 = Directory.GetDirectories(oldestdir);
                        foreach (string subdir in subdirs2)
                            RemoveDirectory(filename, task, extension, subdir);
                    }
                    return;
                }

                //from here on we use parentdirs as a set rather than a list;
                parentdirs.Sort(delegate(string dir1, string dir2)
                {
                    return (new DirectoryInfo(dir2)).LastWriteTime.CompareTo((new DirectoryInfo(dir1)).LastWriteTime);
                }); //oldest dirs last
                for (int i = parentdirs.Count - 1; i >= task.SubfoldersNumber; i--)
                {
                    string parentdir = parentdirs[i];
                    string[] subdirs2 = Directory.GetDirectories(parentdir);
                    foreach (string subdir in subdirs2)
                        RemoveDirectory(filename, task, extension, subdir);
                }
            }
            catch(Exception ex)
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.CleanupDirectoriesFailed + ": " + ex.Message, filename,task);
            }
        }

        internal void CleanupDirs(string filename, TaskDataUNC task, string extension)
        {
            try
            {
                string rootmap = task.DestinationMapUNC;
                List<string> subdirs = GetDeepestDirectories(rootmap);
                if (subdirs.Count <= task.SubfoldersNumber) return; //everything ok
                if (subdirs.Count == task.SubfoldersNumber + 1) //optimization, don't sort the list, only remove oldest directory
                {
                    Log(Logging.Level.Warning,String.Format("cleanup by folder number, counted {0} folders, which is one too many ",subdirs.Count), filename,task);
                    string dir=null;
                    DateTime dtoldest = new DateTime();
                    foreach (string dirC in subdirs)
                    {
                        DateTime dt = (new DirectoryInfo(dirC)).LastWriteTime;
                        if(dir==null || dt < dtoldest)
                        {
                            dir = dirC;
                            dtoldest = dt;
                        }
                    }

                    if (dir != null)
                    {
                        RemoveDirectory(filename,task,extension,dir);
                        Log(Logging.Level.Warning, "cleanup by folder number: Remove directory (one directory removal): " + dir, filename, task);
                    }
                    return;
                }

                Log(Logging.Level.Warning,String.Format("cleanup by folder number, counted {0} folders, which are several too many ",subdirs.Count), filename,task);

                subdirs.Sort(delegate(string dir1, string dir2)
                {
                    return (new DirectoryInfo(dir2)).LastWriteTime.CompareTo((new DirectoryInfo(dir1)).LastWriteTime);
                }); //oldest dirs last
                for (int i = subdirs.Count - 1; i >= task.SubfoldersNumber;i--)
                {
                    string dir = subdirs[i];
                    Log(Logging.Level.Warning, "cleanup by folder number: Remove directory (multiple directory removal): " + dir, filename, task);
                    RemoveDirectory(filename,task,extension,dir);
                }
            }
            catch(Exception ex)
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.CleanupDirectoriesFailed + ": " + ex.Message, filename,task);
            }
        }

        private void RemoveDirectory(string filename, TaskDataUNC task, string extension, string dir)
        {
            try
            {
                string rootmap = task.DestinationMapUNC;
                foreach (string file in Utility.PathUtil.GetFilesMultipleExtensions(dir, "*" + extension))
                {
                    File.Delete(file);
                }
                while (dir.Length > rootmap.Length)
                {
                    DirectoryInfo dirinf = new DirectoryInfo(dir);
                    string parentdir = dirinf.Parent.FullName;
                    if (dirinf.GetFiles().Length == 0 && dirinf.GetDirectories().Length == 0)
                    {
                        Directory.Delete(dir, false);
                        string message = string.Format(iba.Properties.Resources.logCleanupSuccessRemoveOldDirectory, dir);
                        Log(Logging.Level.Info, message, filename, task);
                    }
                    else break;
                    dir = parentdir;
                }
            }
            catch
            {
                Log(Logging.Level.Warning, iba.Properties.Resources.logRemoveDirectoryFailed + ": " + dir, filename, task);
            }
        }

        private List<string> GetDeepestDirectories(string dir)
        {
            List<string> subdirs = new List<string>(Directory.GetDirectories(dir));
            if (subdirs.Count == 0)
            {
                subdirs.Add(dir);
                return subdirs;
            }
            else
            {
                List<string> deepestdirs = new List<string>();
                foreach (string subdir in subdirs)
                {
                    deepestdirs.AddRange(GetDeepestDirectories(subdir));
                }
                return deepestdirs;
            }
        }

        private void CopyDatFile(string filename, CopyMoveTaskData task)
        {
            string[] destinations = null;
            string[] filesToCopy = new string[] {filename};

            lock (m_sd.DatFileStates)
            {
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.RUNNING;
            }

            if (!task.ActionDelete)
            {
                string dir = task.DestinationMapUNC;
                if (String.IsNullOrEmpty(dir))
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.logNoOutputPathSpecified, filename, task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    return;
                }

                string extension = new FileInfo(filename).Extension;
                if (task.WhatFile == CopyMoveTaskData.WhatFileEnumA.PREVOUTPUT)
                {
                    if(m_outPutFilesPrevTask == null || m_outPutFilesPrevTask.Length == 0 || string.IsNullOrEmpty(m_outPutFilesPrevTask[0]))
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                        Log(Logging.Level.Exception, iba.Properties.Resources.NoPreviousOutPutFileToCopy, filename, task);
                        return;
                    }
                    if (m_outPutFilesPrevTask != null)
                    {
                        filesToCopy = m_outPutFilesPrevTask;
                        extension = string.Join(",",m_outPutFilesPrevTask.Select((f) => Path.GetExtension(f)));
                    }
                }
                m_outPutFilesPrevTask = null;

                try
                {
                    if (task.UsesQuota)
                        CleanupWithQuota(filename, task, extension);
                }
                catch
                {
                }

                if (!Path.IsPathRooted(dir))
                {  //get Absolute path relative to dir
                    dir = Path.Combine(m_cd.DatDirectoryUNC, dir);
                }
                else dir = task.DestinationMapUNC;
                string maindir = dir;

                dir = null;
                try
                {
                    dir = GetOutputDirectoryName(filename, task);
                    if(dir == null) return;
                }
                catch(Exception ex) //sort of unexpected error;
                {
                    Log(Logging.Level.Exception, ex.Message, filename, task);
                    lock(m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    return;
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
                    if (task.Subfolder != TaskDataUNC.SubfolderChoice.NONE && task.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories)
                        task.DoDirCleanupNow = true;
                }
                if (task.DoDirCleanupNow)
                {
                    try
                    {
                        CleanupDirs(filename, task, extension);
                    }
                    catch
                    {
                    }
                    task.DoDirCleanupNow = false;
                }
                destinations = filesToCopy.Clone() as string[];
                int i = 0;
                foreach (string fileToCopy in filesToCopy)
                {
                    string currentext = Path.GetExtension(fileToCopy);
                    destinations[i] = Path.Combine(dir, GetOutputFileName(task,fileToCopy) + currentext);
                    i++;
                }

                if (!task.OverwriteFiles)
                {
                    for (i = 0; i < destinations.Length; i++)
                    {
                        destinations[i] = DatCoordinatorHostImpl.Host.FindSuitableFileName(destinations[i]);
                    }
                }
            }
            try
            {
                if (task.ActionDelete)
                {
                    foreach (string fileToCopy in filesToCopy)
                        File.Delete(fileToCopy);
                    Log(Logging.Level.Info, iba.Properties.Resources.logDeleteTaskSuccess, filename, task);
                    m_outPutFilesPrevTask = null;
                }
                else if (task.RemoveSource)
                {
                    for (int i = 0; i < destinations.Length; i++)
                    {
                            if (task.OverwriteFiles && task.UsesQuota && File.Exists(destinations[i]))
                            {
                                m_quotaCleanups[task.Guid].RemoveFile(destinations[i]);
                            }
                            File.Copy(filesToCopy[i], destinations[i], true);
                            File.Delete(filesToCopy[i]);
                    }
                    Log(Logging.Level.Info, iba.Properties.Resources.logMoveTaskSuccess, filename, task);
                    m_outPutFilesPrevTask = destinations;
                }
                else
                {
                    for (int i = 0; i < destinations.Length; i++)
                    {
                        if (task.OverwriteFiles && task.UsesQuota &&  File.Exists(destinations[i]))
                        {
                            m_quotaCleanups[task.Guid].RemoveFile(destinations[i]);
                        }
                        File.Copy(filesToCopy[i], destinations[i], true);
                    }
                    Log(Logging.Level.Info, iba.Properties.Resources.logCopyTaskSuccess, filename, task);
                    m_outPutFilesPrevTask = destinations;
                }

                if (!task.ActionDelete && task.UsesQuota && m_outPutFilesPrevTask != null)
                {
                    foreach (string file in m_outPutFilesPrevTask)
                    {
                        m_quotaCleanups[task.Guid].AddFile(file);
                    }
                }

                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                    if (!task.ActionDelete && m_outPutFilesPrevTask != null && m_outPutFilesPrevTask.Length > 0) 
                        m_sd.DatFileStates[filename].OutputFiles[task] = m_outPutFilesPrevTask[0];
                }
            }
            catch (Exception ex)
            {
                if (task.ActionDelete)
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.logDeleteTaskFailed + ": " + ex.Message, filename, task);
                }
                else if (task.RemoveSource)
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.logMoveTaskFailed + ": " + ex.Message, filename, task);
                }
                else
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.logCopyTaskFailed + ": " + ex.Message, filename, task);
                }
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
        }

        internal SortedDictionary<Guid, FileQuotaCleanup> m_quotaCleanups;

        internal void CleanupWithQuota(string filename, TaskDataUNC task, string extension)
        { //the parameter filename is used for logging, nothing else
            FileQuotaCleanup fqc = null;
            if (!m_quotaCleanups.TryGetValue(task.Guid, out fqc))
            {
                fqc = new FileQuotaCleanup(task, extension);
                fqc.Init();
                m_quotaCleanups.Add(task.Guid, fqc);
            }
            fqc.Clean(filename);
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
            string arguments = null;
            if (task.WhatFile == BatchFileData.WhatFileEnum.PREVOUTPUT)
            {
                string outputFile = null;
                if(m_outPutFilesPrevTask != null && m_outPutFilesPrevTask.Length > 0 && !string.IsNullOrEmpty(m_outPutFilesPrevTask[0]))
                    outputFile = m_outPutFilesPrevTask[0];
                if (outputFile == null)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    Log(Logging.Level.Exception, iba.Properties.Resources.NoPreviousOutPutFile, filename, task);
                    return;
                }
                if (outputFile != null)
                    arguments = task.ParsedArguments(outputFile);
            }
            else
            {
                arguments = task.ParsedArguments(filename);
            }

            if (arguments == null)
            {
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                Log(Logging.Level.Exception, iba.Properties.Resources.ScriptArgumentsCouldNotBeParsed, filename, task);
                return;
            }

            using (Process ibaProc = new Process())
            {
                ibaProc.StartInfo.Arguments = arguments;
                ibaProc.EnableRaisingEvents = false;
                ibaProc.StartInfo.FileName = task.BatchFile;
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
                    int MAXMINUTES = 15; //wait maximum 15 minutes  
                    int INCRECEMENTSECONDS = 10;
                    int MAXLOOPS = MAXMINUTES * 60 / INCRECEMENTSECONDS;
                    bool succeeded = false;
                    for (int i = 0; !succeeded && !m_stop && i < MAXLOOPS; i++)
                    {
                        succeeded = ibaProc.WaitForExit(INCRECEMENTSECONDS*1000);
                    }
                    if (!succeeded)
                    {
                        try
                        {
                            ibaProc.Kill();
                        }
                        catch 
                        {
                        }
                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                            if (!m_stop)
                            {
                                string msg = iba.Properties.Resources.logBatchfileTimeout;
                                Log(Logging.Level.Exception, msg, filename, task);
                            }
                            else
                            {
                                string msg = iba.Properties.Resources.logJobTerminated;
                                Log(Logging.Level.Warning, msg, filename, task);
                            }
                        }
                    }
                    else
                    {
                        lock (m_sd.DatFileStates)
                        {
                            if (ibaProc.ExitCode == 0)
                            {
                                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                                Log(Logging.Level.Info, iba.Properties.Resources.logBatchfileSuccess, filename, task);
                            }
                            else
                            {
                                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                                string msg = String.Format(string.Format(iba.Properties.Resources.logBatchfileFailed, ibaProc.ExitCode), ibaProc.ExitCode);
                                Log(Logging.Level.Exception, msg, filename, task);
                            }
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
            bool bUseAnalysis = !String.IsNullOrEmpty(task.AnalysisFile);
            if (bUseAnalysis && !File.Exists(task.AnalysisFile))
            {
                string message = iba.Properties.Resources.AnalysisFileNotFound + task.AnalysisFile;
                Log(Logging.Level.Exception, message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return;
            }
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
                RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                Log(Logging.Level.Exception, me.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.MEMORY_EXCEEDED;
                }
                RestartIbaAnalyzerAndOpenDatFile(filename);
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
                        RestartIbaAnalyzer();
                    }
                }
            }
        }

        private void PauseTask(string filename, PauseTaskData task)
        {
            lock (m_sd.DatFileStates)
            {
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.RUNNING;
            }
            TimeSpan duration = task.Interval;
            DateTime startTime = DateTime.Now;
            if (task.MeasureFromFileTime)
            {
                try
                {
                    DateTime lastWriteTime = (new FileInfo(filename)).LastWriteTime;
                    if (lastWriteTime < startTime) startTime = lastWriteTime;
                }
                catch 
                {
                    Log(iba.Logging.Level.Exception, iba.Properties.Resources.logPauseFailed, filename, task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    return;
                }
            }

            DateTime nextTime = startTime + duration;

            while (DateTime.Now < nextTime && !m_stop)
            {
                Thread.Sleep(TimeSpan.FromSeconds(0.5));
            }

            lock (m_sd.DatFileStates)
            {
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
            }
        }

        private void DoCustomTaskUNC(string filename, CustomTaskDataUNC task)
        {
            IPluginTaskDataUNC plugin = task.Plugin as IPluginTaskDataUNC;
            if (plugin==null) 
                throw new Exception("incorrectly implemented unc plugin");
            string ext = plugin.Extension;
            try
            {
                if (task.UsesQuota)
                    CleanupWithQuota(filename, task, ext);
            }
            catch
            {
            }
            string actualFileName = GetOutputFileName(task, filename);
            lock (m_licensedTasks)
            {
                bool licensed = false;
                m_licensedTasks.TryGetValue(task, out licensed);
                if (!licensed)
                {
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    Log(Logging.Level.Exception, String.Format(iba.Properties.Resources.logCustomTaskNotLicensed, task.Name));
                    return;
                }
            }

            String dir = null;
            try
            {
                dir = GetOutputDirectoryName(filename, task);
                if(dir == null) return;
            }
            catch(Exception ex) //sort of unexpected error;
            {
                Log(Logging.Level.Exception, ex.Message, filename, task);
                lock(m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return;
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
                if (task.Subfolder != ReportData.SubfolderChoice.NONE && task.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories)
                    task.DoDirCleanupNow = true;
            }
            if (task.DoDirCleanupNow)
            {
                try
                {
                    CleanupDirs(filename, task, ext);
                }
                catch
                {
                }
                task.DoDirCleanupNow = false;
            }
            string arg = Path.Combine(dir, actualFileName + ext);
            try
            {
                if (!task.OverwriteFiles)
                    arg = DatCoordinatorHostImpl.Host.FindSuitableFileName(arg);
                else if (task.UsesQuota && File.Exists(arg))
                {
                    m_quotaCleanups[task.Guid].RemoveFile(arg);
                }
                
                string message = string.Format(iba.Properties.Resources.logTaskStarted, task.Plugin.NameInfo);
                Log(Logging.Level.Info, message, filename, task);
                //do execution:
                if (!(plugin.GetWorker() as IPluginTaskWorkerUNC).ExecuteTask(filename ,arg))
                { //failure
                    Log(Logging.Level.Exception, plugin.GetWorker().GetLastError(), filename, task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    return;
                }
                //success
                m_outPutFilesPrevTask = new string[]{arg};
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                    m_sd.DatFileStates[filename].OutputFiles[task] = arg;
                }
                message = string.Format(iba.Properties.Resources.logTaskSuccess, task.Plugin.NameInfo);
                Log(Logging.Level.Info, message, filename, task);
                if (task.UsesQuota)
                {
                    m_quotaCleanups[task.Guid].AddFile(arg);
                }
            }
            catch (Exception ex)
            {
                Log(Logging.Level.Exception, ex.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }

        }


        private void UpdateDataTask(string filename, UpdateDataTaskData task)
        {
            lock (m_licensedTasks)
            {
                bool licensed = false;
                m_licensedTasks.TryGetValue(task, out licensed);
                if (!licensed)
                {
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    Log(Logging.Level.Exception, iba.Properties.Resources.logNoLicenseUpdateDataTask);
                    return;
                }
            }

            lock (m_sd.DatFileStates)
            {
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.RUNNING;
            }


            string ext = ".dat";
            try
            {
                if (task.UsesQuota)
                    CleanupWithQuota(filename, task, ext);
            }
            catch
            {
            }
            Log(Logging.Level.Info, iba.Properties.Resources.logUDTStarted, filename, task);

            bool doInit = false;

            if (!m_udtWorkers.ContainsKey(task.Guid))
            {
                m_udtWorkers[task.Guid] = new UpdateDataTaskWorker(task);
                doInit = true;
            }
            UpdateDataTaskWorker worker = m_udtWorkers[task.Guid];

            if (doInit || worker.TimesCalled > 100)
            {
                try
                {
                    worker.Init();
                }
                catch (Exception ex)
                {
                    Log(iba.Logging.Level.Exception, ex.Message, filename, task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    worker.Dispose();
                    m_udtWorkers.Remove(task.Guid);
                    return;
                }
            }

            string dir = null;
            try
            {
                dir = GetOutputDirectoryName(filename, task);
                if (dir == null) return;
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
                    //new directory created, do directory cleanup if that is the setting
                    if (task.Subfolder != TaskDataUNC.SubfolderChoice.NONE && task.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories)
                        task.DoDirCleanupNow = true;
                }
                if (task.DoDirCleanupNow)
                {
                    try
                    {
                        CleanupDirs(filename, task, ext);
                    }
                    catch
                    {
                    }
                    task.DoDirCleanupNow = false;
                }
            }
            catch (Exception ex) //sort of unexpected error;
            {
                Log(Logging.Level.Exception, ex.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return;
            }

            string outfile = null;
            try
            {
                outfile = worker.DoWork(dir, filename);
                m_outPutFilesPrevTask = new string[]{outfile};
            }
            catch (Exception ex)
            {
                Log(iba.Logging.Level.Exception, ex.Message, filename, task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                if (worker.FatalError)
                {
                    worker.Dispose();
                    m_udtWorkers.Remove(task.Guid);
                }
                return;
            }

            try
            {
                if (task.OverwriteFiles && task.UsesQuota)
                {
                    if (worker.FileOverWritten)
                    {
                        m_quotaCleanups[task.Guid].RemoveFile(outfile);
                        m_quotaCleanups[task.Guid].AddFile(outfile);
                    }
                    m_quotaCleanups[task.Guid].AddFile(outfile);
                }
            }
            catch
            {
            }
            lock (m_sd.DatFileStates)
            {
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                m_sd.DatFileStates[filename].OutputFiles[task] = outfile;
            }
            Log(Logging.Level.Info, iba.Properties.Resources.logUDTaskSuccess + " - " + iba.Properties.Resources.logUDTCreationTime + " " + worker.Created.ToString(), filename, task);
        }

        private void DoCleanupAnyway(string DatFile, TaskDataUNC task) //a unc task has free space cleanup strategy, 
            // the cleanup needs to be performed even is the task isn't
        {
            try
            {
                string ext = null;
                if (task.WhenToExecute == TaskData.WhenToDo.DISABLED) return;
                if (task is ReportData)
                {
                    ReportData rd = task as ReportData;
                    ext = "." + rd.Extension;
                    if (rd.Extension == "html" || rd.Extension == "htm")
                        ext += ",*.jpg";
                }
                else if (task is ExtractData)
                {
                    ext = ExtractTaskWorker.GetBothExtractExtensions(task as ExtractData);
                }
                else if (task is CopyMoveTaskData)
                {
                    CopyMoveTaskData cpm = task as CopyMoveTaskData;
                    if (!cpm.ActionDelete)
                    {
                        if (cpm.WhatFile == CopyMoveTaskData.WhatFileEnumA.PREVOUTPUT && m_outPutFilesPrevTask != null)
                            ext = string.Join(",", m_outPutFilesPrevTask.Select((f) => Path.GetExtension(f)));
                        else if (cpm.WhatFile == CopyMoveTaskData.WhatFileEnumA.DATFILE)
                            ext = new FileInfo(DatFile).Extension;
                    }
                }
                else if (task is UpdateDataTaskData)
                {
                    ext = ".dat";
                }
                else if (task is CustomTaskDataUNC)
                {
                    ext = ((task as CustomTaskDataUNC).Plugin as IPluginTaskDataUNC).Extension;
                }

                if (!String.IsNullOrEmpty(ext))
                    CleanupWithQuota(DatFile, task, ext);
            }
            catch
            {

            }
        }

        private DateTime m_nextTrigger;
        private System.Threading.Timer NextEventTimer;

        private void ScheduleNextEvent()
        {
            ScheduleNextEvent(DateTime.Now);
        }
        private void ScheduleNextEvent(DateTime triggerFrom)
        {
            TriggerCalculator tc = new TriggerCalculator(m_cd.ScheduleData);
            if(!tc.NextTrigger(triggerFrom, out m_nextTrigger))
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.ErrorScheduleNextTrigger);
                Stop = true;
                return;
            }
            if(NextEventTimer == null)
            {
                NextEventTimer = new System.Threading.Timer(OnScheduleCheckTick);
            }
            SetScheduleCheckTickTimer();
        }

        private void SetScheduleCheckTickTimer()
        {
            try
            {
                TimeSpan waitTime = m_nextTrigger - DateTime.Now;
                if(waitTime.Ticks < 0) waitTime = TimeSpan.Zero;
                if(waitTime > TimeSpan.FromSeconds(10)) waitTime = TimeSpan.FromSeconds(10);
                NextEventTimer.Change(waitTime, TimeSpan.Zero);
            }
            catch //disposed exceptions on timer
            {

            }
        }

        private void OnScheduleCheckTick(object ignoreMe)
        {
            if(m_bTimersstopped || m_stop) return;
            NextEventTimer.Change(Timeout.Infinite, Timeout.Infinite);
            if((DateTime.Now - m_nextTrigger).Ticks >= TimeSpan.FromMilliseconds(10).Ticks)
            { //FIRE
                String filename = Path.Combine(m_cd.HDQDirectory, string.Format("{0}_{1:yyyy-MM-dd_HH-mm-ss}.hdq", CPathCleaner.CleanFile(m_cd.Name),m_nextTrigger));
                try
                {
                    m_cd.GenerateHDQFile(m_nextTrigger, filename);
                }
                catch (Exception ex)
                {
                    Log(Logging.Level.Exception, "Failure creating HDQ file:" + ex.Message, filename);
                    ScheduleNextEvent(m_nextTrigger);
                    return;
                }
                m_sd.UpdatingFileList = true;
                lock(m_processedFiles)
                {
                    lock(m_toProcessFiles)
                    {
                        bool doit = !m_toProcessFiles.Contains(filename) && !m_processedFiles.Contains(filename);
                        if(doit)
                        {
                            m_toProcessFiles.Add(filename);
                            lock(m_sd.DatFileStates)
                            {
                                m_sd.DatFileStates[filename] = new DatFileStatus();
                                m_sd.DatFileStates[filename].AlternativeFileDescription = m_cd.CreateHDQFileDescription(filename);
                            }
                            m_waitEvent.Set();
                        }
                        m_sd.Changed = true;
                    }
                }
                m_sd.UpdatingFileList = false;
                m_sd.MergeProcessedAndToProcessLists();
                ScheduleNextEvent(m_nextTrigger);
            }
            else
                SetScheduleCheckTickTimer();
        }

        public void ForceTrigger()
        {
            DateTime nextTrigger = DateTime.Now;
            String filename = Path.Combine(m_cd.HDQDirectory, string.Format("{0}_{1:yyyy-MM-dd_HH-mm-ss}.hdq", CPathCleaner.CleanFile(m_cd.Name), nextTrigger));
            try
            {
                m_cd.GenerateHDQFile(nextTrigger, filename);
            }
            catch(Exception ex)
            {
                Log(Logging.Level.Exception, "Failure creating HDQ file:" + ex.Message, filename);
                return;
            }
            m_sd.UpdatingFileList = true;
            lock(m_processedFiles)
            {
                lock(m_toProcessFiles)
                {
                    bool doit = !m_toProcessFiles.Contains(filename) && !m_processedFiles.Contains(filename);
                    if(doit)
                    {
                        m_toProcessFiles.Add(filename);
                        lock(m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename] = new DatFileStatus();
                            m_sd.DatFileStates[filename].AlternativeFileDescription = m_cd.CreateHDQFileDescription(filename);
                        }
                        m_waitEvent.Set();
                    }
                    m_sd.Changed = true;
                }
            }
            m_sd.UpdatingFileList = false;
            m_sd.MergeProcessedAndToProcessLists();
        }
    }
}
