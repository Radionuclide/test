using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Threading;
using iba.Data;
using IBAFILESLib;
using iba.Utility;
using System.Windows.Forms;
using System.Diagnostics;
using System.ComponentModel;

namespace iba.Processing
{

    class ConfigurationWorker
    {
        private ConfigurationData m_cd;
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
            }
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
            m_sd.ProcessedFiles = m_processedFiles = new List<string>();
            m_sd.ReadFiles = m_toProcessFiles = new List<string>();
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

        private void UpdateConfiguration()
        {
            if (m_toUpdate != null)
            {
                if (m_toUpdate.NotificationData.TimeInterval < m_cd.NotificationData.TimeInterval 
                    && !m_toUpdate.NotificationData.NotifyImmediately)
                {
                    if (notifyTimer == null) notifyTimer = new System.Threading.Timer(OnNotifyTimerTick);
                    notifyTimer.Change(m_toUpdate.NotificationData.TimeInterval, TimeSpan.Zero);
                }
                if (m_toUpdate.RescanTimeInterval < m_cd.RescanTimeInterval && m_toUpdate.RescanEnabled)
                {
                    if (rescanTimer == null) rescanTimer = new System.Threading.Timer(OnRescanTimerTick);
                    rescanTimer.Change(m_toUpdate.RescanTimeInterval, TimeSpan.Zero);
                }
                if (m_toUpdate.ReprocessErrorsTimeInterval< m_cd.ReprocessErrorsTimeInterval)
                    notifyTimer.Change(m_toUpdate.ReprocessErrorsTimeInterval, TimeSpan.Zero);
                m_cd = m_toUpdate;
                m_sd.CorrConfigurationData = m_cd;
                if (m_notifier != null)
                {
                    m_notifier.Send();
                    m_notifier = new Notifier(m_cd);
                }
                m_toUpdate = null;
            }
        }

        public ConfigurationWorker(ConfigurationData cd)
        {
            m_cd = cd;
            m_sd = new StatusData(cd);
            m_stop = true;
            m_sd.ProcessedFiles = m_processedFiles = new List<string>();
            m_sd.ReadFiles = m_toProcessFiles = new List<string>();
            m_waitEvent = new AutoResetEvent(false);
            m_notifier = new Notifier(cd);
            m_candidateNewFiles = new List<string>();
            m_timerLock = new Object();
        }       
                
        List<string> m_processedFiles;
        List<string> m_toProcessFiles;
        List<string> m_candidateNewFiles;

        private void Log(Logging.Level level, string message)
        {
            if (LogData.Data.Logger.IsOpen)
            {
                LogExtraData data = new LogExtraData(String.Empty, null, m_cd);
                LogData.Data.Logger.Log(level, message, (object)data);
            }
        }

        private void Log(Logging.Level level, string message, string datfile)
        {
            if (LogData.Data.Logger.IsOpen)
            {
                LogExtraData data = new LogExtraData(datfile, null, m_cd);
                LogData.Data.Logger.Log(level, message, (object)data);
            }
        }

        private void Log(Logging.Level level, string message, string datfile, TaskData task)
        {
            if (LogData.Data.Logger.IsOpen)
            {
                LogExtraData data = new LogExtraData(datfile, task, m_cd);
                LogData.Data.Logger.Log(level, message, (object)data);
            }
        }

        private IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer = null;

        private string m_previousRunExecutable;

        private System.Threading.Timer notifyTimer;
        private System.Threading.Timer reprocessErrorsTimer;
        private System.Threading.Timer rescanTimer;
        private System.Threading.Timer retryAccessTimer;
        bool m_bTimersstopped;
        
        private void Run()
        {
            //Notifier not = new Notifier();
            Log(Logging.Level.Info, iba.Properties.Resources.logConfigurationStarted);
            if (!Directory.Exists(m_cd.DatDirectory))
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.logDatDirError);
                m_sd.Started = false;
                Stop = true;
                return;
            }
            using (FileSystemWatcher fswt = new FileSystemWatcher(m_cd.DatDirectory, "*.dat"))
            {
                fswt.NotifyFilter = NotifyFilters.FileName;
                fswt.IncludeSubdirectories = m_cd.SubDirs;
                fswt.Created += new FileSystemEventHandler(OnNewDatFile);
                fswt.EnableRaisingEvents = true;
                updateDatFileList(WhatToUpdate.ALL);
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
                    while (!m_stop)
                    {
                        //register this
                        if (m_previousRunExecutable != m_cd.IbaAnalyserExe)
                        {
                            try
                            {
                                string version = FileVersionInfo.GetVersionInfo(m_cd.IbaAnalyserExe).FileVersion;
                                if (version.CompareTo("5.0") < 0)
                                {
                                    Log(Logging.Level.Exception, iba.Properties.Resources.logFileVersionToLow);
                                    m_sd.Started = false;
                                    Stop = true;
                                    return;
                                };
                                Process ibaProc = new Process();
                                ibaProc.EnableRaisingEvents = false;
                                ibaProc.StartInfo.FileName = m_cd.IbaAnalyserExe;
                                ibaProc.StartInfo.Arguments = "/regserver";
                                ibaProc.Start();
                                ibaProc.WaitForExit(10000);
                                m_previousRunExecutable = m_cd.IbaAnalyserExe;
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
                            Trace.WriteLine("ibastarted\r\n");
                            string version = m_ibaAnalyzer.GetVersion();
                        }
                        catch (Exception ex)
                        {
                            Log(Logging.Level.Exception, ex.Message);
                            m_sd.Started = false;
                            Stop = true;
                            return;
                        }

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
                                        if (File.Exists(fileC))
                                        {
                                            FileInfo f = new FileInfo(fileC);
                                            DateTime n = f.CreationTime;
                                            if (n < mintime)
                                            {
                                                file = fileC;
                                                mintime = n;
                                            }
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
                            lock (m_timerLock)
                            {
                                ProcessDatfile(file);
                            }
                            if (m_stop) break;
                            lock (m_toProcessFiles)
                            {
                                int index = m_toProcessFiles.IndexOf(file);
                                if (index >= 0)
                                    m_toProcessFiles.RemoveAt(index);
                            }
                        }
                        //stop the com object
                        try
                        {
                            if (m_ibaAnalyzer == null)
                                return;
                            System.Runtime.InteropServices.Marshal.ReleaseComObject(m_ibaAnalyzer);
                            Trace.WriteLine("ibastopped\r\n");
                            m_ibaAnalyzer = null;
                        }
                        catch (Exception ex)
                        {
                            Trace.WriteLine("ibaerror\r\n");
                            Log(Logging.Level.Exception, ex.Message);
                            m_sd.Started = false;
                            Stop = true;
                            return;
                        }
                        m_waitEvent.WaitOne();
                        UpdateConfiguration();
                    }
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
                    }
                    if (reprocessErrorsTimer != null)
                    {
                        reprocessErrorsTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        reprocessErrorsTimer.Dispose();
                    }
                    if (retryAccessTimer != null)
                    {
                        retryAccessTimer.Change(Timeout.Infinite, Timeout.Infinite);
                        reprocessErrorsTimer.Dispose();
                    }
                    Debug.Assert(m_ibaAnalyzer == null, "ibaAnalyzer should have been closed");
                }
            }
            m_sd.Started = false;
            Log(Logging.Level.Info, iba.Properties.Resources.logConfigurationStopped);
        }

        private void OnNewDatFile(object sender, FileSystemEventArgs args)
        {
            string filename = args.FullPath;
            lock (m_candidateNewFiles)
            {
                if (!m_candidateNewFiles.Contains(filename))
                    m_candidateNewFiles.Add(filename);
            }
        }

        private object m_timerLock; //makes the timer routines mutually exclusive

        private void OnAddNewDatFileTimerTick(object ignoreMe)
        {
            if (m_bTimersstopped || m_stop) return;
            retryAccessTimer.Change(Timeout.Infinite, Timeout.Infinite);
            bool changed = false;
            List<string> added = new List<string>();
            lock (m_timerLock)
            {
                lock (m_candidateNewFiles)
                {
                    foreach (string filename in m_candidateNewFiles)
                    {
                        FileStream fs = null;
                        try
                        {
                            fs = new FileStream(filename, FileMode.Open, FileAccess.Write, FileShare.None);
                            fs.Close();
                            fs.Dispose();
                            bool doit = false;
                            lock (m_toProcessFiles)
                            {
                                doit = !m_toProcessFiles.Contains(filename) && !m_processedFiles.Contains(filename);
                                if (doit)
                                    m_toProcessFiles.Add(filename);
                            }
                            added.Add(filename);
                            changed = changed || doit;
                        }
                        catch //no access
                        {
                        }
                    }
                    if (changed)
                        foreach (string filename in added)
                        {
                            m_candidateNewFiles.Remove(filename);
                        }
                }
                if (changed)
                {
                    updateDatFileList(WhatToUpdate.NEW);
                    m_waitEvent.Set();
                }
            }
            if (!m_bTimersstopped && !m_stop)
                retryAccessTimer.Change(1000, Timeout.Infinite);
        }

        private void OnRescanTimerTick(object ignoreMe)
        {
            if (m_bTimersstopped || m_stop) return;
            rescanTimer.Change(Timeout.Infinite, Timeout.Infinite);
            lock (m_timerLock)
            {
                updateDatFileList(WhatToUpdate.ALL);
                m_waitEvent.Set();
            }
            if (!m_bTimersstopped && !m_stop)
                rescanTimer.Change(m_cd.RescanTimeInterval, TimeSpan.Zero);
        }

        private void OnReprocessErrorsTimerTick(object ignoreMe)
        {
            if (m_bTimersstopped || m_stop) return;
            reprocessErrorsTimer.Change(Timeout.Infinite, Timeout.Infinite);
            lock (m_timerLock)
            {
                updateDatFileList(WhatToUpdate.ERRORS);
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

        private enum WhatToUpdate { ALL, NEW, ERRORS };

        private void updateDatFileList(WhatToUpdate what)
        {
            Log(Logging.Level.Info, iba.Properties.Resources.logCheckingForNewDatFiles);
            string datDir = m_cd.DatDirectory;
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
                        for (int i = 0; i < count; i++)
                        {
                            lock (m_sd.DatFileStates)
                            {
                                if ((File.Exists(m_processedFiles[i]))
                                    && (
                                    (what == WhatToUpdate.ERRORS && m_sd.DatFileStates.ContainsKey(m_processedFiles[i]))
                                    ||
                                    (what == WhatToUpdate.NEW && !m_sd.DatFileStates.ContainsKey(m_processedFiles[i]))
                                    ))
                                {
                                    fileInfos[i] = new FileInfo(m_processedFiles[i]);
                                }
                                else
                                    fileInfos[i] = null;
                            }
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
                                        allclear = allclear && m_sd.DatFileStates[filename].States.ContainsKey(dat) && m_sd.DatFileStates[filename].States[dat] == DatFileStatus.State.COMPLETED_SUCCESFULY;
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
                        m_processedFiles.Clear();
                }
                lock (m_toProcessFiles)
                {
                    //m_toProcessFiles.Clear();
                    DirectoryInfo dirInfo = new DirectoryInfo(datDir);
                    if (what == WhatToUpdate.ALL)
                        fileInfos = dirInfo.GetFiles("*.dat", m_cd.SubDirs?SearchOption.AllDirectories:SearchOption.TopDirectoryOnly);
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
                                    foreach (TaskData dat in m_cd.Tasks)
                                    {
                                        if (dat.Enabled) status.States[dat] = DatFileStatus.State.NO_ACCESS;
                                    }
                                    m_sd.DatFileStates[filename] = status;
                                }
                                break;
                            case DatFileStatus.State.RUNNING:
                                break;
                        }
                    }
                    m_sd.Changed = true;
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
            IbaFileUpdater ibaDatFile = new IbaFileClass();
            try
            {
                ibaDatFile.OpenForUpdate(filename);
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
                string status = ibaDatFile.QueryInfoByName("status");
                if (status == "processed")
                {
                    ibaDatFile.Close();
                    return DatFileStatus.State.COMPLETED_SUCCESFULY;
                }
                else if (status == "processingfailed")
                {
                    List<string> guids = null;
                    string guidstring = null;
                    //get guids
                    try
                    {
                       guidstring = ibaDatFile.QueryInfoByName("TasksDone");
                    }
                    catch (ArgumentException)
                    {
                    }
                    catch (Exception) //general exception
                    {
                        Log(Logging.Level.Exception, iba.Properties.Resources.ReadStatusError, filename);
                    }
                    guidstring.Trim(new char[] { ';' });
                    guids = new List<string>(guidstring.Split(new char[] { ';' }));
                    guids.Sort();
                    foreach (TaskData task in m_cd.Tasks)
                    {
                        if (guids.BinarySearch(task.Guid.ToString())>0)
                        lock (m_sd.DatFileStates)
                        {
                                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                        }
                    }
                    ibaDatFile.Close();
                    return DatFileStatus.State.COMPLETED_FAILURE;
                }
                else if (status == "restart" || status == "readyToProcess")
                {
                    ibaDatFile.Close();
                    return DatFileStatus.State.NOT_STARTED;
                }
                else
                {
                    return DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
            catch //status field does not exist yet
            {
                ibaDatFile.Close();
                DateTime time = File.GetLastWriteTime(filename);
                ibaDatFile.OpenForUpdate(filename);
                ibaDatFile.WriteInfoField("status", "readyToProcess");
                ibaDatFile.Close();
                File.SetLastWriteTime(filename, time);
                return DatFileStatus.State.NOT_STARTED;
            }
        }

        private AutoResetEvent m_waitEvent;

        private bool shouldTaskBeDone(TaskData task, string filename)
        {
            lock (m_sd.DatFileStates)
            {
                if (task.Enabled == false)
                    return false;
                else if (task.WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE)
                    return m_sd.DatFileStates[filename].States[task] != DatFileStatus.State.COMPLETED_SUCCESFULY;
                else if (task.Index == 0) //first task and dependent on previous task, always do
                    return true;
                else if (task.WhenToExecute == TaskData.WhenToDo.AFTER_SUCCES)
                    return m_sd.DatFileStates[filename].States[task] != DatFileStatus.State.COMPLETED_SUCCESFULY &&
                        m_sd.DatFileStates[filename].States[m_cd.Tasks[task.Index - 1]] == DatFileStatus.State.COMPLETED_SUCCESFULY;
                else if (task.WhenToExecute == TaskData.WhenToDo.AFTER_FAILURE)
                    return m_sd.DatFileStates[filename].States[m_cd.Tasks[task.Index - 1]] == DatFileStatus.State.COMPLETED_FAILURE;
                else if (task.WhenToExecute == TaskData.WhenToDo.AFTER_1st_FAILURE)
                    return m_sd.DatFileStates[filename].States[task] != DatFileStatus.State.COMPLETED_SUCCESFULY
                       && m_sd.DatFileStates[filename].States[m_cd.Tasks[task.Index - 1]] == DatFileStatus.State.COMPLETED_FAILURE;
                return false;
            }
        }

        private void ProcessDatfile(string DatFile)
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
                m_ibaAnalyzer.CloseDataFiles();
                return;
            }
            foreach (TaskData task in m_cd.Tasks)
            {
                bool failedOnce = false;
                lock (m_sd.DatFileStates)
                {
                    failedOnce = m_sd.DatFileStates.ContainsKey(DatFile)
                        && m_sd.DatFileStates[DatFile].States.ContainsKey(task)
                        && m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_FAILURE;
                }
                if (!shouldTaskBeDone(task, DatFile))
                    continue;
                else if (task is ReportData)
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
                else if (task is CopyMoveTaskData)
                {
                    CopyMoveTaskData dat = task as CopyMoveTaskData;
                    if (dat.RemoveSource)
                    {
                        m_ibaAnalyzer.CloseDataFiles();
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
                                m_ibaAnalyzer.CloseDataFiles();
                                return;
                            }
                        }
                        else
                            return;
                    }
                    else
                        CopyDatFile(DatFile, dat);
                }
                lock (m_sd.DatFileStates)
                {
                    if (m_sd.DatFileStates[DatFile].States[task] != DatFileStatus.State.COMPLETED_SUCCESFULY)
                        completeSucces = false;
                    if (m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_SUCCESFULY
                        && (task.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES || task.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE))
                    {
                        m_notifier.AddSuccess(task, DatFile);
                        if (m_cd.NotificationData.NotifyImmediately)
                            m_notifier.Send();
                    }
                    else if ((m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.COMPLETED_FAILURE ||
                        m_sd.DatFileStates[DatFile].States[task] == DatFileStatus.State.TIMED_OUT) &&
                        (task.WhenToNotify == TaskData.WhenToDo.AFTER_FAILURE || task.WhenToNotify == TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE
                        || (task.WhenToNotify == TaskData.WhenToDo.AFTER_1st_FAILURE && failedOnce)))
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
            m_ibaAnalyzer.CloseDataFiles();
            
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
            if (completeSucces)
                ibaDatFile.WriteInfoField("status", "processed");
            else
            {
                ibaDatFile.WriteInfoField("status", "processingfailed");
                //write GUIDs of those that were succesfull
                lock (m_sd.DatFileStates)
                {
                    string guids = "";
                    foreach (KeyValuePair<TaskData, DatFileStatus.State> stat in m_sd.DatFileStates[DatFile].States)
                        if (stat.Value == DatFileStatus.State.COMPLETED_SUCCESFULY)
                            guids += stat.Key.Guid.ToString() + ";";
                    ibaDatFile.WriteInfoField("TasksDone",guids);
                }
            }
            ibaDatFile.Close();
            File.SetLastWriteTime(DatFile, time);
            m_sd.Changed = true;
        }

        private void Extract(string filename, ExtractData task)
        {
            try
            {
                m_ibaAnalyzer.OpenAnalysis(task.AnalysisFile);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.RUNNING;
                }
                Log(Logging.Level.Info, iba.Properties.Resources.logExtractStarted, filename, task);
                if (task.ExtractToFile)
                {
                    string outFile = GetExtractFileName(filename, task);
                    if (outFile == null) return;
                    m_ibaAnalyzer.Extract(1, outFile);
                }
                else
                {
                    m_ibaAnalyzer.Extract(0, String.Empty);
                }
                //code on succes
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                Log(Logging.Level.Info, iba.Properties.Resources.logExtractSuccess, filename,task);
            }
            catch 
            {
                Log(Logging.Level.Exception, m_ibaAnalyzer.GetLastError(), filename,task);
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
            }
            finally
            {
                if (m_ibaAnalyzer != null)
                    m_ibaAnalyzer.CloseAnalysis();
            }
        }

        private string GetExtractFileName(string filename, ExtractData task)
        {
            string actualFileName = Path.GetFileNameWithoutExtension(filename);
            string dir = task.DestinationMap;
            if (!Path.IsPathRooted(dir))
            {  //get Absolute path relative to dir
                dir = Path.Combine(m_cd.DatDirectory, dir);
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
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.logCreateDirectoryFailed + ": " + dir, filename, task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    return null;
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
            return arg;
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
            if (task.Output != ReportData.OutputChoice.PRINT)
            {
                string actualFileName = Path.GetFileNameWithoutExtension(filename);
                string dir = task.DestinationMap;
                if (!Path.IsPathRooted(dir))
                {  //get Absolute path relative to dir
                    dir = Path.Combine(m_cd.DatDirectory, dir);
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
                    try
                    {
                        Directory.CreateDirectory(dir);
                    }
                    catch
                    {
                        Log(Logging.Level.Exception, iba.Properties.Resources.logCreateDirectoryFailed + ": " + dir, filename,task);
                        lock (m_sd.DatFileStates)
                        {
                            m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                        }
                        return;
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
            }

            try
            {
                m_ibaAnalyzer.OpenAnalysis(task.AnalysisFile);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.RUNNING;
                }
                Log(Logging.Level.Info, iba.Properties.Resources.logReportStarted, filename, task);
                if (task.Output != ReportData.OutputChoice.PRINT)
                    m_ibaAnalyzer.Report(arg);
                else
                    m_ibaAnalyzer.Report("");
                //code on succes
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                Log(Logging.Level.Info, iba.Properties.Resources.logReportSuccess, filename,task);
            }
            catch
            {
                Log(Logging.Level.Exception, m_ibaAnalyzer.GetLastError(), filename,task);
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
            }
            finally
            {
                if (m_ibaAnalyzer != null)
                    m_ibaAnalyzer.CloseAnalysis();
            }
        }


        private void CopyDatFile(string filename, CopyMoveTaskData task)
        {
            string dir = task.DestinationMap;
            if (!Path.IsPathRooted(dir))
            {  //get Absolute path relative to dir
                dir = Path.Combine(m_cd.DatDirectory, dir);
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
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch
                {
                    Log(Logging.Level.Exception, iba.Properties.Resources.logCreateDirectoryFailed + ": " + dir, filename, task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    return;
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

            string dest = Path.Combine(dir, Path.GetFileName(filename));

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
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
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
                m_sd.DatFileStates[filename].States[task] = DatFileStatus.State.COMPLETED_FAILURE;
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
                ibaProc.StartInfo.Arguments = filename + " " + task.AnalysisFile;
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
    }
}
