using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using System.Runtime.InteropServices;
using iba.Data;
using iba.Utility;
using iba.Plugins;
using System.IO;
using iba.Logging;
using iba.Processing.IbaOpcUa;
using IbaSnmpLib;

namespace iba.Processing
{
    public class TaskManager : MarshalByRefObject
    {
        SortedDictionary<ConfigurationData, ConfigurationWorker> m_workers;

        virtual public void AddConfiguration(ConfigurationData data)
        {
            AddConfigurationInternal(data);
            IncreaseTaskManagerID();
        }

        private void AddConfigurationInternal(ConfigurationData data)
        {
            ConfigurationWorker cw = new ConfigurationWorker(data);
            lock (m_workers)
            {
                m_workers.Add(data, cw);
            }
            ExtMonConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        virtual public void AddConfigurations(List<ConfigurationData> datas)
        {
            foreach (ConfigurationData data in datas)
                AddConfigurationInternal(data);
            IncreaseTaskManagerID();
        }

        virtual public void RemoveConfiguration(ConfigurationData data)
        {
            RemoveConfigurationInternal(data);
            IncreaseTaskManagerID();
        }

        private void RemoveConfigurationInternal(ConfigurationData data)
        {
            lock (m_workers)
            {
                if (m_workers.ContainsKey(data))
                {
                    StopConfiguration(data);
                    m_workers.Remove(data);
                }
            }

            ExtMonConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        virtual public void ReplaceConfiguration(ConfigurationData data)
        {
            lock (m_workers)
            {
                ConfigurationWorker cw;

                if (m_workers.TryGetValue(data, out cw))
                {
                    m_workers.Remove(data); //data sorted on ID, remove it as we'll insert a
                                            // new data with same ID
                    m_workers.Add(data, cw);

                }
                //else, ignore, replace is due to a belated save of an already deleted configuration
            }

            ExtMonConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ReplaceOrAddConfigurationInternal(ConfigurationData data)
        {
            lock (m_workers)
            {
                ConfigurationWorker cw;
                if (m_workers.TryGetValue(data, out cw))
                {
                    m_workers.Remove(data); //data sorted on ID, remove it as we'll insert a
                    // new data with same ID
                    m_workers.Add(data, cw);
                }
                else
                {
                    cw = new ConfigurationWorker(data);
                    m_workers.Add(data, cw);
                }
            }
            ExtMonConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        virtual public void ReplaceConfigurations(List<ConfigurationData> datas)
        {
            foreach (ConfigurationData data in datas)
                ReplaceOrAddConfigurationInternal(data);
            List<ConfigurationData> toRemove = new List<ConfigurationData>();
            //remove spurious configurations;
            foreach (ConfigurationData dat in m_workers.Keys)
            {
                if (!datas.Contains(dat))
                    //contains works because we've already replaced all datas (it would fail otherwise because contains
                    // uses  equality comparer instead of CompareTo)
                    toRemove.Add(dat);
            }
            foreach (ConfigurationData dat in toRemove)
            {
                RemoveConfigurationInternal(dat);
            }
            IncreaseTaskManagerID();
        }

        virtual public void UpdateConfiguration(ConfigurationData data)
        {
            lock (m_workers)
            {
                try
                {
                    ConfigurationWorker cw = m_workers[data];
                    cw.ConfigurationToUpdate = data;
                    m_workers.Remove(data);
                    m_workers.Add(data, cw);
                    cw.Signal();
                }
                catch (KeyNotFoundException)
                {
                    LogData.Data.Logger.Log("key not found");
                    //doesn't matter
                }
            }

            ExtMonConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        virtual public bool CompareConfiguration(ConfigurationData data)
        {
            foreach (ConfigurationData d in m_workers.Keys)
            {
                if (d.Guid == data.Guid)
                {
                    return data.IsSame(d);
                }
            }
            return false;
        }

        virtual public void UpdateTreePosition(Guid guid, int pos)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid)
                {
                    pair.Key.TreePosition = pos;
                    if (pair.Value.ConfigurationToUpdate != null)
                        pair.Value.ConfigurationToUpdate.TreePosition = pos;
                    if (pair.Value.RunningConfiguration != null)
                        pair.Value.RunningConfiguration.TreePosition = pos;
                    if (pair.Value.Status.CorrConfigurationData != null)
                        pair.Value.Status.CorrConfigurationData.TreePosition = pos;
                    return;
                }
            }
        }

        virtual public void ClearConfigurations()
        {
            StopAndWaitForAllConfigurations();
            lock (m_workers)
            {
                m_workers.Clear();
            }
            IncreaseTaskManagerID();
        }

        virtual public void StartAllEnabledConfigurationsNoOneTime()
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                if (kvp.Key.Enabled && (kvp.Key.JobType != ConfigurationData.JobTypeEnum.OneTime))
                    StartConfiguration(kvp.Key);
        }

        virtual public void StopAllConfigurations()
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                StopConfiguration(kvp.Key);
        }

        virtual public void StartConfiguration(ConfigurationData data)
        {
            m_workers[data].ConfigurationToUpdate = data;
            m_workers[data].Start();

            ExtMonConfigurationChanged?.Invoke(this, EventArgs.Empty);
        }

        virtual public void StopConfiguration(ConfigurationData data)
        {
            m_workers[data].Stop = true;
        }

        virtual public void StopConfiguration(Guid guid)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid)
                {
                    pair.Value.Stop = true;
                    return;
                }
            }
        }

        virtual public void StopAndWaitForConfiguration(ConfigurationData data)
        {
            ConfigurationWorker worker = m_workers[data];
            worker.Stop = true;
            worker.Join(60000);
        }

        virtual public void StopAndWaitForConfiguration(Guid guid)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid)
                {
                    pair.Value.Stop = true;
                    pair.Value.Join(60000);
                    return;
                }
            }
        }

        virtual public void StopAndWaitForAllConfigurations()
        {
            //WaitDialog waitDialog = new WaitDialog();
            //System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(stop), waitDialog);
            //waitDialog.ShowDialog();

            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                StopConfiguration(kvp.Key);
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                kvp.Value.Join(60000);
        }

        virtual public ConfigurationData GetConfigurationFromWorker(Guid guid)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
            {
                if (kvp.Key.Guid == guid)
                {
                    return (kvp.Value.ConfigurationToUpdate ?? kvp.Value.RunningConfiguration) ?? kvp.Key;
                }
            }
            throw new KeyNotFoundException(guid.ToString() + " not found");
        }

        virtual public MinimalStatusData GetMinimalStatus(Guid guid, bool permanentError)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid) return pair.Value.Status.GetMinimalStatusData(permanentError);
            }
            return null;
        }

        virtual public bool IsJobStarted(Guid guid)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid) return pair.Value.Status.Started;
            }
            return false;
        }

        virtual public PluginTaskWorkerStatus GetStatusPlugin(Guid guid, int taskindex)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid && pair.Key.Tasks[taskindex] is ICustomTaskData && pair.Value.Status.Started)
                    return
                        (pair.Value.RunningConfiguration.Tasks[taskindex] as ICustomTaskData).Plugin.GetWorker()
                            .GetWorkerStatus();
            }
            return null;
        }

        public enum AlterPermanentFileErrorListWhatToDo
        {
            AFTERDELETE,
            AFTERREFRESH
        };

        virtual public void AlterPermanentFileErrorList(AlterPermanentFileErrorListWhatToDo todo, Guid guid,
            List<string> files)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid)
                {
                    if (todo == AlterPermanentFileErrorListWhatToDo.AFTERREFRESH)
                    {
                        pair.Value.AddFilesToProcess(files);
                    }
                    pair.Value.Status.ClearPermanentFileErrorList(files);
                    return;
                }
            }
            throw new KeyNotFoundException(guid.ToString() + " not found");
        }

        //watchdog data
        private TCPWatchdog m_watchdog;

        public TCPWatchdog WatchDog
        {
            get { return m_watchdog; }
        }

        virtual public WatchDogData WatchDogData
        {
            get { return m_watchdog.Settings; }
        }

        virtual public string GetWatchdogStatus()
        {
            return m_watchdog.StatusString;
        }

        virtual public void ReplaceWatchdogData(WatchDogData data)
        {
            m_watchdog.Settings = data;
        }


        #region External monitoring - SNMP and OPC UA interfaces


        #region External monitoring - Functionality common for SNMP and OPC UA

        /// <summary> 
        /// Is fired when there is a chance (yes, at least a chance) that external monitoring data structure (amount of jobs, tasks, etc) is changed. 
        /// So, SnmpWorker and OpcUaWorker can know this, and may rebuild their objects trees accordingly.
        /// It does not guarantee that data is really changed, but only just that it might have changed.
        /// (event handler is very lightweight, so it's better to trigger this event
        /// more often (e.g. let even twice for each real change - no matter), than 
        /// to miss some point (even some that happens seldom) where it is changed.
        /// Event is not relevant to some 'small' data changes,
        /// i.e changes that do not alter the structure (hierarchy) of the snmp tree (e.g. status of the job, or some other value).
        /// </summary>
        public event EventHandler<EventArgs> ExtMonConfigurationChanged;

        public bool ExtMonRefreshLicenseInfo(ExtMonData.LicenseInfo licenseInfo)
        {
            licenseInfo.Reset();

            // this feature is not licensed,
            // so does not need any condition to be true
            licenseInfo.IsValid.Value = true;

            try
            {
                CDongleInfo info = CDongleInfo.ReadDongle();
                // ReSharper disable once RedundantBoolCompare
                if (info != null && info.DongleFound == true)
                {
                    licenseInfo.Sn.Value = info.SerialNr;
                    licenseInfo.HwId.Value = info.HwId;
                    licenseInfo.DongleType.Value = info.DongleType;
                    licenseInfo.Customer.Value = info.Customer;
                    licenseInfo.TimeLimit.Value = info.TimeLimit;
                    licenseInfo.DemoTimeLimit.Value = info.DemoTimeLimit;
                }

                licenseInfo.TimeStamp.PutStamp();
            }
            catch {/**/}

            return true;
        }

        public bool ExtMonRefreshGlobalCleanupDriveInfo(ExtMonData.GlobalCleanupDriveInfo driveInfo)
        {
            // reset values for the case of an update error
            driveInfo.Reset();

            if (String.IsNullOrEmpty(driveInfo.Key))
            {
                return false; // failed to update
            }

            try
            {
                lock (m_workers)
                {
                    var gcData = GlobalCleanupDataList.FirstOrDefault(gc => gc.DriveName == driveInfo.Key);

                    if (gcData == null)
                    {
                        // needed GlobalCleanupDataList not found
                        return false; // failed to update
                    }

                    // set current values
                    ExtMonRefreshGlobalCleanupDriveInfo(driveInfo, gcData);
                    return true; // data was updated
                }
            }
            catch
            {
                // suppress, not critical
            }
            return false; // failed to update
        }

        private void ExtMonRefreshGlobalCleanupDriveInfo(
            ExtMonData.GlobalCleanupDriveInfo driveInfo, GlobalCleanupData gcData)
        {
            driveInfo.Reset();

            driveInfo.Active.Value = gcData.Active;

            DriveInfo drive = new DriveInfo(gcData.DriveName);
            if (drive.IsReady)
            {
                driveInfo.SizeInMb.Value = (uint)(drive.TotalSize >> 20);
                driveInfo.CurrentFreeSpaceInMb.Value = (uint)(drive.TotalFreeSpace >> 20); 
            }

            // here I use the same formula  as in ServiceSettingsControl.cs, but with conversion to MB:
            // ... = PathUtil.GetSizeReadable((long)(driveSize * (data.PercentageFree / 100.0)));
            //driveInfo.MinFreeSpaceInMb = (uint)(driveInfo.SizeInMb * (gcData.PercentageFree / 100.0)); 
            driveInfo.MinFreeSpaceInPercent.Value = (uint)gcData.PercentageFree; 

            driveInfo.RescanTime.Value = (uint)gcData.RescanTime;

            driveInfo.TimeStamp.PutStamp(); 
        }

        public bool ExtMonRefreshJobInfo(ExtMonData.JobInfoBase jobInfo)
        {
            jobInfo.Reset();

            if (jobInfo.Guid == Guid.Empty)
            {
                return false;
            }

            try
            {
                lock (m_workers)
                {
                    var cfg = Configurations.FirstOrDefault(cd => cd.Guid == jobInfo.Guid);

                    if (cfg == null)
                    {
                        // job with given GUID not found
                        return false; // failed to update
                    }

                    // ok, found needed configuration
                    // copy values from configuration to snmp jobInfo
                    ExtMonRefreshJobInfo(jobInfo, cfg);

                    // updated successfully
                    return true;
                }
            }
            catch
            {
                // suppress
                // for the case of change of the list 
                // within foreach loop by another thread
            }

            // error
            return false; // failed to update
        }

        private void ExtMonRefreshJobInfo(ExtMonData.JobInfoBase jobInfo, ConfigurationData cfg)
        {
            switch (jobInfo)
            {
                case ExtMonData.StandardJobInfo stdJobInfo:
                    ExtMonRefreshStandardJobInfo(stdJobInfo, cfg);
                    break;
                case ExtMonData.ScheduledJobInfo schJobInfo:
                    ExtMonRefreshScheduledJobInfo(schJobInfo, cfg);
                    break;
                case ExtMonData.OneTimeJobInfo otJobInfo:
                    ExtMonRefreshOneTimeJobInfo(otJobInfo, cfg);
                    break;
                case ExtMonData.EventBasedJobInfo ebJobInfo:
                    ExtMonRefreshEventJobInfo(ebJobInfo, cfg);
                    break;
            }
        }

        private void ExtMonRefreshStandardJobInfo(ExtMonData.StandardJobInfo jobInfo, ConfigurationData cfg)
        {
            Debug.Assert(cfg.JobType == ConfigurationData.JobTypeEnum.DatTriggered);
            jobInfo.Reset();

            try
            {
                ConfigurationWorker worker = m_workers[cfg];
                StatusData s = worker.Status;

                ExtMonRefreshJobInfoBase(jobInfo, worker, s);

                jobInfo.PermFailedCount.Value = (uint)s.PermanentErrorFiles.Count;
                jobInfo.TimestampJobStarted.Value = worker.TimestampJobStarted;
                jobInfo.TimestampLastDirectoryScan.Value = worker.TimestampLastDirectoryScan;
                jobInfo.TimestampLastReprocessErrorsScan.Value = worker.TimestampLastReprocessErrorsScan;

                jobInfo.LastProcessingLastDatFileProcessed.Value = worker.LastSuccessfulFileName;
                jobInfo.LastProcessingStartTimeStamp.Value = worker.LastSuccessfulFileStartProcessingTimeStamp;
                jobInfo.LastProcessingFinishTimeStamp.Value = worker.LastSuccessfulFileFinishProcessingTimeStamp;

                jobInfo.TimeStamp.PutStamp();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Debug, $@"{nameof(ExtMonRefreshStandardJobInfo)}(): " + ex.Message);
            }
        }

        private void ExtMonRefreshScheduledJobInfo(ExtMonData.ScheduledJobInfo jobInfo, ConfigurationData cfg)
        {
            Debug.Assert(cfg.JobType == ConfigurationData.JobTypeEnum.Scheduled);
            jobInfo.Reset();

            try
            {
                ConfigurationWorker worker = m_workers[cfg];
                StatusData s = worker.Status;

                ExtMonRefreshJobInfoBase(jobInfo, worker, s);

                jobInfo.PermFailedCount.Value = (uint)s.PermanentErrorFiles.Count; 
                jobInfo.TimestampJobStarted.Value = worker.TimestampJobStarted; 
                jobInfo.TimestampLastExecution.Value = worker.TimestampJobLastExecution; 
                jobInfo.TimestampNextExecution.Value = worker.NextTrigger; 

                jobInfo.TimeStamp.PutStamp();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Debug, $@"{nameof(ExtMonRefreshScheduledJobInfo)}(): " + ex.Message);
            }

        }

        private void ExtMonRefreshOneTimeJobInfo(ExtMonData.OneTimeJobInfo jobInfo, ConfigurationData cfg)
        {
            Debug.Assert(cfg.JobType == ConfigurationData.JobTypeEnum.OneTime);
            jobInfo.Reset();

            try
            {
                ConfigurationWorker worker = m_workers[cfg];
                StatusData s = worker.Status;

                ExtMonRefreshJobInfoBase(jobInfo, worker, s);

                jobInfo.TimestampLastExecution.Value = worker.TimestampJobStarted;

                jobInfo.TimeStamp.PutStamp();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Debug, $@"{nameof(ExtMonRefreshOneTimeJobInfo)}(): " + ex.Message);
            }

        }

        private void ExtMonRefreshEventJobInfo(ExtMonData.EventBasedJobInfo jobInfo, ConfigurationData cfg)
        {
            Debug.Assert(cfg.JobType == ConfigurationData.JobTypeEnum.Event);
            jobInfo.Reset();

            try
            {
                ConfigurationWorker worker = m_workers[cfg];
                StatusData s = worker.Status;

                ExtMonRefreshJobInfoBase(jobInfo, worker, s);

                jobInfo.PermFailedCount.Value = (uint)s.PermanentErrorFiles.Count; 
                jobInfo.TimestampJobStarted.Value = worker.TimestampJobStarted; 
                jobInfo.TimestampLastExecution.Value = worker.TimestampJobLastExecution; 

                jobInfo.TimeStamp.PutStamp();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Debug, $@"{nameof(ExtMonRefreshOneTimeJobInfo)}(): " + ex.Message);
            }

        }

        private void ExtMonRefreshJobInfoBase(ExtMonData.JobInfoBase ji, ConfigurationWorker worker, StatusData s)
        {
            var cfg = s.CorrConfigurationData;
            ji.JobName.Value = cfg.Name;
            ji.Status.Value = !cfg.Enabled ?
                ExtMonData.JobStatus.Disabled :
                (s.Started ?
                    ExtMonData.JobStatus.Started :
                    ExtMonData.JobStatus.Stopped);

            ji.TodoCount.Value = (uint)s.ReadFiles.Count;
            ji.DoneCount.Value = (uint)s.TotalFilesProcessed; 
            //ji.DoneCount.Value = (uint)s.ProcessedFiles.Count; // doesn't show total count, so it is not so helpful
            ji.FailedCount.Value = (uint)s.CountErrors();

            ji.UpTime.Value = (ji.Status.Value == ExtMonData.JobStatus.Started)
                ? (int)(DateTime.Now - worker.TimestampJobStarted).TotalSeconds
                : -1;

            // Lifebeat is a value like xxSssd,
            // where xx is a 2-digit job hash,
            // S is a job status (0, 1 or 2),
            // ss - seconds within current minute,
            // d - deci-second within current second;
            int hash = Math.Abs(cfg.Guid.GetHashCode()) % 90 + 10; // [10..99] i.e. a 2-digit hash
            ji.Lifebeat.Value = (hash * 10 + (int)(ji.Status.Value)) * 1000 + DateTime.Now.Second * 10 + DateTime.Now.Millisecond / 100;

            ExtMonRefreshTasks(ji, worker, s);
        }

        private void ExtMonRefreshTasks(ExtMonData.JobInfoBase ji, ConfigurationWorker worker, StatusData statusData)
        {
            var cfg = statusData.CorrConfigurationData;

            // on first call for the job create a list first
            if (ji.TaskCount == 0 && cfg.Tasks.Count != 0)
            {
                foreach (TaskData t in cfg.Tasks)
                {
                    ji.AddTask(t.Name, t.Guid);
                }
            }

            // if task count has changed then invalidate tree structure
            if (ji.TaskCount != cfg.Tasks.Count)
            {
                ji.ResetTasks();
                ExtMonConfigurationChanged?.Invoke(this, EventArgs.Empty);
                return;
            }

            // fill task's data
            for (int i = 0; i < cfg.Tasks.Count; i++)
            {
                var taskData = cfg.Tasks[i];
                var taskInfo = ji[i];

                // if tasks order has changed then invalidate tree structure
                if (taskData.Guid != taskInfo.Guid)
                {
                    ji.ResetTasks();
                    ExtMonConfigurationChanged?.Invoke(this, EventArgs.Empty);
                    return;
                }

                taskInfo.Reset();
                taskInfo.TaskName.Value = taskData.Name;

                // last execution - success, duration, memory
                try
                {
                    if (worker.TaskLastExecutionDict.TryGetValue(taskData, out ConfigurationWorker.TaskLastExecutionData lastExec))
                    {
                        taskInfo.Success.Value = lastExec.Success; 
                        taskInfo.DurationOfLastExecutionInSec.Value = (uint)(lastExec.DurationMs / 1000.0); 
                        taskInfo.MemoryUsedForLastExecutionInMb.Value = lastExec.MemoryUsed; 
                    }
                }
                catch
                {
                    worker.TaskLastExecutionDict.Clear();
                }

                // default is just a type name
                string taskTypeStr = taskData.GetType().Name;

                if (taskData is TaskDataUNC)
                // Derived from TaskDataUNC (alphabetically):
                //   CopyMoveTaskData
                //   CustomTaskDataUNC 
                //   ExtractData 
                //   GlobalCleanupTaskData 
                //   ReportData 
                //   SplitterTaskData 
                //   UpdateDataTaskData 
                {
                    if (taskData is CopyMoveTaskData)
                    {
                        taskTypeStr = "CopyMoveDelete";
                        //var typedData = taskData as CopyMoveTaskData;
                    }
                    if (taskData is CustomTaskDataUNC)
                    {
                        var typedData = taskData as CustomTaskDataUNC;
                        taskTypeStr = $"Custom_{typedData.Plugin.NameInfo}";
                    }
                    if (taskData is ExtractData)
                    {
                        taskTypeStr = "Extract";
                        //var typedData = taskData as ExtractData;
                    }
                    else if (taskData is GlobalCleanupTaskData)
                    {
                        taskTypeStr = "GlobalCleanup";
                        //var typedData = taskData as GlobalCleanupTaskData;
                    }
                    if (taskData is ReportData)
                    {
                        taskTypeStr = "Report";
                        //var typedData = taskData as ReportData;
                        //var monitorData = typedData.MonitorData;
                    }
                    if (taskData is SplitterTaskData)
                    {
                        taskTypeStr = "Splitter";
                        //var typedData = taskData as SplitterTaskData;
                    }
                    if (taskData is UpdateDataTaskData)
                    {
                        taskTypeStr = "UpdateData";
                        //var typedData = taskData as UpdateDataTaskData;
                    }
                }
                else
                // NOT derived from TaskDataUNC (alphabetically):
                //   BatchFileData 
                //   CleanupTaskData 
                //   CustomTaskData
                //   IfTaskData 
                //   PauseTaskData 
                //   HDCreateEventTaskData
                {
                    if (taskData is BatchFileData)
                    {
                        taskTypeStr = "Script";
                        //var typedData = taskData as BatchFileData;
                    }
                    else if (taskData is TaskWithTargetDirData)
                    {
                        taskTypeStr = "Cleanup";
                        //var typedData = taskData as CleanupTaskData;
                    }
                    else if (taskData is CustomTaskData)
                    {
                        var typedData = taskData as CustomTaskData;
                        taskTypeStr = $"Custom_{typedData.Plugin.NameInfo}";
                    }
                    else if (taskData is IfTaskData)
                    {
                        taskTypeStr = "Condition";
                        //var typedData = taskData as IfTaskData;
                    }
                    else if (taskData is PauseTaskData)
                    {
                        taskTypeStr = "Pause";
                        //var typedData = taskData as PauseTaskData;
                    }
                    else if (taskData is HDCreateEventTaskData)
                    {
                        taskTypeStr = "ibaHDCreateEvent";
                    }
                }

                taskInfo.TaskType.Value = taskTypeStr;

                // if cleanup info is present, add it to the task
                if (taskData is TaskWithTargetDirData cleanupTaskData)
                {
                    taskInfo.AddCleanupInfo();
                    taskInfo.CleanupInfo.LimitChoice.Value = cleanupTaskData.OutputLimitChoice;
                    taskInfo.CleanupInfo.FreeDiskSpace.Value = cleanupTaskData.QuotaFree;
                    taskInfo.CleanupInfo.Subdirectories.Value = cleanupTaskData.SubfoldersNumber;
                    taskInfo.CleanupInfo.UsedDiskSpace.Value = cleanupTaskData.Quota;
                }
                else
                {
                    taskInfo.DeleteCleanupInfo();
                }
            }
        }

        /// <summary> Rebuilds <see cref="ExtMonData"/> instance. </summary>
        /// <returns> true on success and false on error </returns>
        public bool ExtMonRebuildObjectsData()
        {
            ExtMonData.DebugWriteLine(nameof(TaskManager), "RebuildTree");
            var od = ExtMonData.Instance;
            try
            {
                od.Reset();

                lock (m_workers)
                {
                    // PrGen.3. License
                    {
                        // nothing to create there
                        // just refresh data
                        ExtMonRefreshLicenseInfo(od.License);
                    }

                    // 1. GlobalCleanup;
                    {
                        try
                        {
                            foreach (var gcData in GlobalCleanupDataList.OrderBy(gc => gc.DriveName))
                            {
                                // create entry
                                var driveInfo = od.AddNewGlobalCleanup(gcData.DriveName);

                                // set current values
                                ExtMonRefreshGlobalCleanupDriveInfo(driveInfo, gcData);
                            }
                        }
                        catch
                        {
                            // suppress, not critical
                        }
                    }
                    // 2...5. - Jobs
                    {
                        // get copy of configurations
                        List<ConfigurationData> confs = Configurations;
                        confs.Sort((a, b) => a.TreePosition.CompareTo(b.TreePosition));

                        foreach (ConfigurationData cfg in confs)
                        {
                            var jobInfo = od.AddNewJob(cfg.JobType, cfg.Name, cfg.Guid);
                            ExtMonRefreshJobInfo(jobInfo, cfg);
                        }
                    }
                }
                Debug.Assert(od.CheckConsistency());
                return true; // success
            }
            catch (Exception ex)
                            {
                LogData.Data.Logger.Log(Level.Exception,
                    $"{nameof(ExtMonRebuildObjectsData)}. Error during rebuilding object data. {ex.Message}.");
                return false; // error
            }
        }

        /// <summary> Gets Server host name (can be different from client's one if is run in c/s mode) </summary>
        public virtual string GetServerHostName()
        {
            return System.Net.Dns.GetHostName();
        }

        #endregion


        #region External monitoring - SNMP

        #region SNMP Configuration

        private SnmpWorker SnmpWorker { get; } = new SnmpWorker();

        public void SnmpWorkerInit()
        {
            SnmpWorker.Init();
                            }

        /// <summary> Gets/sets data of SnmpWorker. 
        /// If data is set, then restart of snmp agent is performed if necessary. </summary>
        public virtual SnmpData SnmpData
        {
            get => SnmpWorker?.SnmpData;
            set
            {
                if (SnmpWorker != null)
                {
                    SnmpWorker.SnmpData = value;
                        }
                    }
                }

        #endregion


        #region SNMP Objects

        public virtual void SnmpRebuildObjectTree()
        {
            try
            {
                SnmpWorker.RebuildTree();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(SnmpRebuildObjectTree)}. {ex.Message}");
            }
        }

        public virtual Dictionary<IbaSnmpOid, ExtMonData.GuiTreeNodeTag> SnmpGetObjectTreeSnapShot()
        {
            try
            {
                return SnmpWorker.GetObjectTreeSnapShot();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(SnmpGetObjectTreeSnapShot)}. {ex.Message}");
                return null;
            }
        }

        public virtual ExtMonData.GuiTreeNodeTag SnmpGetTreeNodeTag(IbaSnmpOid oid)
        {
            try
            {
                // refresh value and get information
                return SnmpWorker.GetTreeNodeTag(oid, true);
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(SnmpGetTreeNodeTag)}. {ex.Message}");
                return null;
            }
        }

        public virtual List<SnmpMibFileContainer> SnmpGenerateMibFiles()
        {
            try
            {
                return SnmpWorker.GenerateMibFiles();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(SnmpGenerateMibFiles)}. {ex.Message}");
                return null;
            }
        }

        #endregion

        
        #region SNMP Diagnostics

        public virtual Tuple<ExtMonWorkerStatus, string> SnmpGetBriefStatus()
        {
            try
            {
                return new Tuple<ExtMonWorkerStatus, string>(SnmpWorker.Status, SnmpWorker.StatusString);
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(SnmpGetBriefStatus)}. {ex.Message}");
                return null;
            }
        }

        public virtual List<IbaSnmpDiagClient> SnmpGetClients()
        {
            try
            {
                return SnmpWorker.IbaSnmp?.GetClients();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(SnmpGetClients)}. {ex.Message}");
                return null;
            }
        }

        public virtual void SnmpClearClients()
        {
            try
            {
                SnmpWorker.IbaSnmp?.ClearClients();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(SnmpClearClients)}. {ex.Message}");
            }
        }

        #endregion

        #endregion


        #region External monitoring - OPC UA


        #region OPC UA Configuration

        private OpcUaWorker OpcUaWorker { get; } = new OpcUaWorker();

        public void OpcUaWorkerInit()
        {
            OpcUaWorker.Init();
        }

        /// <summary> Gets/sets configuration data of <see cref="OpcUaWorker"/>. 
        /// If data is set, then restart of UA Server is performed if necessary. </summary>
        public virtual OpcUaData OpcUaData
        {
            get => OpcUaWorker?.OpcUaData;
            set
            {
                if (OpcUaWorker != null)
                {
                    OpcUaWorker.OpcUaData = value;
                }
            }
        }

        public virtual List<OpcUaData.CertificateTag> OpcUaHandleCertificate(string command, object args = null)
        {
            try
            {
                return OpcUaWorker.HandleCertificate(command, args);
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(OpcUaHandleCertificate)}. {ex.Message}");
                return null;
            }
        }

        public virtual OpcUaData.NetworkConfiguration OpcUaGetNetworkConfiguration()
        {
            try
            {
                return OpcUaWorker.GetNetworkConfiguration();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(OpcUaGetNetworkConfiguration)}. {ex.Message}");
                return null;
            }
        }

        #endregion


        #region OPC UA Objects

        public virtual void OpcUaRebuildObjectTree()
        {
            try
            {
                OpcUaWorker.RebuildTree();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(OpcUaRebuildObjectTree)}. {ex.Message}");
            }
        }

        public virtual List<ExtMonData.GuiTreeNodeTag> OpcUaGetObjectTreeSnapShot()
        {
            try
            {
                return OpcUaWorker.GetObjectTreeSnapShot();
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(OpcUaGetObjectTreeSnapShot)}. {ex.Message}");
                return null;
            }
        }

        public virtual ExtMonData.GuiTreeNodeTag OpcUaGetTreeNodeTag(string id)
        {
            try
            {
                return OpcUaWorker.GetTreeNodeTag(id);
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(OpcUaGetTreeNodeTag)}. {ex.Message}");
                return null;
            }
        }

        #endregion


        #region OPC UA Diagnostics

        public virtual Tuple<ExtMonWorkerStatus, string> OpcUaGetBriefStatus()
        {
            try
            {
                return new Tuple<ExtMonWorkerStatus, string>(OpcUaWorker.Status, OpcUaWorker.StatusString);
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(OpcUaGetBriefStatus)}. {ex.Message}");
                return null;
            }
        }

        public virtual Tuple<List<IbaOpcUaDiagClient>, string> OpcUaGetDiagnostics()
        {
            try
            {
                var clients = OpcUaWorker.GetClients();
                // todo. kls. delete before last beta
                var diagnosticString = OpcUaWorker.GetDiagnosticString();
                return new Tuple<List<IbaOpcUaDiagClient>, string>(clients, diagnosticString);
            }
            catch (Exception ex)
            {
                LogData.Data.Logger.Log(Level.Exception, $"{nameof(OpcUaGetDiagnostics)}. {ex.Message}");
                return null;
            }
        }

        #endregion


        #endregion


        #endregion


        virtual public bool TestPath(string dir, string user, string pass, out string errormessage, bool createnew, bool testWrite)
        {
            return SharesHandler.TestPath(dir, user, pass, out errormessage, createnew, testWrite);
        }

        //singleton construction
        private static TaskManager theTaskManager = null;
        //theTaskmanager has a triple purpose
        //1:) If standalone it is simply THE manager
        //2:) When installed as service and connected it is
        //      On the server instance of TaskManager: THE taskmanager
        //      On the client instance of TaskManager: nothing
        //3:) When installed as service and disconnected
        //      On the client instance of TaskManager: A temporary manager, allowing you to set settings that can be uploaded when reconnected
        //      On the server instance of Taskmanager (on condition that server is alive): another manager, that might be downloaded when reconnected
        private static TaskManagerWrapper RemoteTaskManager = null;
        //wrapper, i.e. this should be returned when running on the client and being connected, 
        // it delegates calls to theTaskManager on the serverside through the communication object

        public TaskManager()
        {
            m_workers = new SortedDictionary<ConfigurationData, ConfigurationWorker>();
            m_watchdog = new TCPWatchdog();
            m_doPostPone = true;
            m_postponeMinutes = 5;
            m_processPriority = (int)System.Diagnostics.ProcessPriorityClass.Normal;
            m_password = "";
            m_rememberPassTime = TimeSpan.FromMinutes(5);
            m_rememberPassEnabled = false;
            m_criticalTaskSemaphore = new FifoSemaphore(6);
            m_globalCleanup = new GlobalCleanupManager();
        }

        public static TaskManager Manager
        {
            get
            {
                if (Program.IsServer) //was set by the service
                    return theTaskManager;

                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                {
                    if (RemoteTaskManager == null)
                        RemoteTaskManager = new TaskManagerWrapper();
                    return RemoteTaskManager;
                }
                
                if (theTaskManager == null)
                    theTaskManager = new TaskManager();
                return theTaskManager;
            }

            set
            {
                theTaskManager = value;
            }
        }

        public static TaskManager ClientManager //to be called from client, gets the client manager;
        {
            get
            {
                return theTaskManager;
            }
            set
            {
                theTaskManager = value;
            }
        }

        virtual public List<ConfigurationData> Configurations
        {
            get
            {
                List<ConfigurationData> theC = new List<ConfigurationData>(m_workers.Keys);
                return theC;
            }
            set
            {
                ClearConfigurations();
                AddConfigurations(value);
            }
        }

        public override object InitializeLifetimeService()
        {
            return null; //immortal object
        }

        virtual public int Count
        {
            get { return m_workers.Count; }
        }

        //can't be called remote, so no virtual
        public string GetStatusForWatchdog()
        {
            try
            {
                StringBuilder message = new StringBuilder();
                lock (m_workers)
                {
                    List<ConfigurationData> confs = Configurations;
                    confs.Sort(delegate(ConfigurationData a, ConfigurationData b) { return a.TreePosition.CompareTo(b.TreePosition); });

                    foreach (ConfigurationData conf in confs)
                    {
                        StatusData s = m_workers[conf].Status;
                        message.Append(conf.Name);
                        message.Append(':');
                        message.Append(s.Started ? "started," : "stopped,");
                        message.Append(s.ReadFiles.Count);
                        message.Append(" todo,");
                        message.Append(s.ProcessedFiles.Count);
                        message.Append(" done,");
                        message.Append(s.CountErrors());
                        message.Append(" failed,");
                        message.Append(s.PermanentErrorFiles.Count);
                        message.Append(" perm. failed;");
                    }
                }
                return message.ToString();
            }
            catch
            {
                return null;
            }
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct BinaryWatchdogMessageLine
        {
            public int state;
            public int todoCount;
            public int doneCount;
            public int errorCount;
            public int permErrorCount;
            public int reserved1;
            public int reserved2;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct BinaryWatchdogMessage
        {
            public int counter;
            public int version;
            public int reserved1;
            public int reserved2;

            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 16)]
            public BinaryWatchdogMessageLine[] jobs;
        }

        private int m_watchdogCounter;

        public unsafe byte[] GetStatusForWatchdogBinary()
        {
            try
            {
                BinaryWatchdogMessage pMsg = new BinaryWatchdogMessage();

                pMsg.counter = m_watchdogCounter++;
                pMsg.version = 1; //version 1
                pMsg.reserved1 = 0;
                pMsg.reserved2 = 0;
                pMsg.jobs = new BinaryWatchdogMessageLine[16];
                lock (m_workers)
                {
                    List<ConfigurationData> confs = Configurations;
                    confs.Sort(delegate(ConfigurationData a, ConfigurationData b) { return a.TreePosition.CompareTo(b.TreePosition); });
                    for (int i = 0; i < Math.Min(16, confs.Count); i++)
                    {
                        StatusData s = m_workers[confs[i]].Status;
                        pMsg.jobs[i].state = s.Started ? 1 : 0;
                        pMsg.jobs[i].todoCount = s.ReadFiles.Count;
                        pMsg.jobs[i].doneCount = s.ProcessedFiles.Count;
                        pMsg.jobs[i].errorCount = s.CountErrors();
                        pMsg.jobs[i].permErrorCount = s.PermanentErrorFiles.Count;
                        pMsg.jobs[i].reserved1 = 0;
                        pMsg.jobs[i].reserved2 = 0;
                    }
                    for (int i = confs.Count; i < 16; i++)
                    {
                        pMsg.jobs[i].state = 0;
                        pMsg.jobs[i].todoCount = 0;
                        pMsg.jobs[i].doneCount = 0;
                        pMsg.jobs[i].errorCount = 0;
                        pMsg.jobs[i].permErrorCount = 0;
                        pMsg.jobs[i].reserved1 = 0;
                        pMsg.jobs[i].reserved2 = 0;
                    }
                }

                byte[] answer = new byte[Marshal.SizeOf(typeof(BinaryWatchdogMessage))];
                fixed (byte* pAnswer = &answer[0])
                {
                    Marshal.StructureToPtr(pMsg, new IntPtr(pAnswer), false);
                }
                return answer;
            }
            catch
            {
                return null;
            }
        }


        bool m_doPostPone;
        virtual public bool DoPostponeProcessing
        {
            get { return m_doPostPone; }
            set { m_doPostPone = value; }
        }

        virtual public int MaxResourceIntensiveTasks
        {
            get { return m_criticalTaskSemaphore.MaxNumberOfRunningTasks; }
            set { m_criticalTaskSemaphore.MaxNumberOfRunningTasks = value; }
        }

        virtual public int MaxSimultaneousIbaAnalyzers
        {
            get { return IbaAnalyzerCollection.Collection.MaxNumberOfRunningTasks; }
            set { IbaAnalyzerCollection.Collection.MaxNumberOfRunningTasks = value; }
        }

        virtual public int MaxIbaAnalyzerCalls
        {
            get { return IbaAnalyzerCollection.Collection.MaxCallCount; }
            set { IbaAnalyzerCollection.Collection.MaxCallCount = value; }
        }

        virtual public bool IsIbaAnalyzerCallsLimited
        {
            get { return IbaAnalyzerCollection.Collection.LimitCallCount; }
            set { IbaAnalyzerCollection.Collection.LimitCallCount = value; }
        }

        int m_postponeMinutes;
        virtual public int PostponeMinutes
        {
            get { return m_postponeMinutes; }
            set { m_postponeMinutes = value; }
        }

        private int m_processPriority;
        virtual public int ProcessPriority
        {
            get { return m_processPriority; }
            set
            {
                m_processPriority = value;
                System.Diagnostics.ProcessPriorityClass pc = (System.Diagnostics.ProcessPriorityClass)value;
                if (Program.IsServer || Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
                {
                    try
                    {
                        System.Diagnostics.Process.GetCurrentProcess().PriorityClass = pc;
                    }
                    catch
                    { }
                }
            }
        }

        private FifoSemaphore m_criticalTaskSemaphore;

        public FifoSemaphore CriticalTaskSemaphore
        {
            get { return m_criticalTaskSemaphore; }
            set { m_criticalTaskSemaphore = value; }
        }


        string m_password;
        virtual public string Password
        {
            get { return m_password; }
        }

        virtual public void ChangePassword(string newPassword, string initiator)
        {
            if (newPassword == null)
                newPassword = "";

            if (m_password == newPassword)
                return;

            m_password = newPassword;

            if (initiator != null)
                LogData.Data.Log(Level.Info, String.Format(newPassword == "" ? Properties.Resources.PasswordRemoved : Properties.Resources.PasswordSet, initiator));
        }

        TimeSpan m_rememberPassTime;
        virtual public TimeSpan RememberPassTime
        {
            get { return m_rememberPassTime; }
            set { m_rememberPassTime = value; }
        }

        bool m_rememberPassEnabled;
        virtual public bool RememberPassEnabled
        {
            get { return m_rememberPassEnabled; }
            set { m_rememberPassEnabled = value; }
        }


        virtual public KeyValuePair<string, string>[] AdditionalFileNames()
        {
            List<KeyValuePair<string, string>> myList = new List<KeyValuePair<string, string>>();
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key != null)
                    pair.Key.AdditionalFileNames(myList);
            }
            return myList.ToArray();
        }

        private GlobalCleanupManager m_globalCleanup;
        public virtual List<GlobalCleanupData> GlobalCleanupDataList
        {
            get { return m_globalCleanup.GlobalCleanupDataList; }
            set { m_globalCleanup.GlobalCleanupDataList = value; }
        }

        int m_taskManagerID;
        virtual public int TaskManagerID
        {
            get
            {
                return m_taskManagerID;
            }
        }

        int m_confStoppedID;
        virtual public int ConfStoppedID
        {
            get
            {
                return m_confStoppedID;
            }
        }

        virtual public int Version
        {
            get
            {
                return DatCoVersion.CurrentVersion();
            }
        }

        internal void IncreaseConfStoppedID()
        {
            System.Threading.Interlocked.Increment(ref m_confStoppedID);
        }


        internal void IncreaseTaskManagerID()
        {
            System.Threading.Interlocked.Increment(ref m_taskManagerID);
        }


        public virtual void StartAllEnabledGlobalCleanups()
        {
            m_globalCleanup.StartAllEnabledGlobalCleanups();
        }

        public virtual void StopAllGlobalCleanups()
        {
            m_globalCleanup.StopAllGlobalCleanups();
        }

        public virtual void ReplaceGlobalCleanupData(GlobalCleanupData data)
        {
            m_globalCleanup.ReplaceGlobalCleanupData(data);
        }


        //HDTrigger
        public virtual void ForceTrigger(ConfigurationData data)
        {
            m_workers[data].ForceTrigger();
        }

        public virtual void CleanAndProcessFileNow(ConfigurationData data, string file)
        {
            string errMessage;
            try
            {
                errMessage = FileProcessing.RemoveMarkingsFromFile(file, data?.FileEncryptionPassword);
            }
            catch (System.Exception ex)
            {
                errMessage = ex.Message;
            }
            if(!string.IsNullOrEmpty(errMessage))
            {
                LogData.Data.Logger.Log(iba.Logging.Level.Exception, iba.Properties.Resources.RemoveMarkingsProblem + " " + errMessage, new LogExtraData(file,null,data));
            }

            m_workers[data].ProcessFileDirect(file);
        }

        public virtual List<string> CheckPluginsAvailable(List<string> plugins)
        {
            return PluginManager.Manager.CheckPluginsAvailable(plugins);
        }
    }


    //for remote calls
    public class TaskManagerWrapper : TaskManager
    {
        public override void AddConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.AddConfiguration(data);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                Manager.AddConfiguration(data);
            }
        }

        public override void AddConfigurations(List<ConfigurationData> datas)
        {
            try
            {
                Program.CommunicationObject.Manager.AddConfigurations(datas);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                Manager.AddConfigurations(datas);
            }
        }

        public override void ClearConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.ClearConfigurations();
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                Manager.ClearConfigurations();
            }
        }

        public override List<ConfigurationData> Configurations
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.Configurations;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.Configurations;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.Configurations = value;
                }
                catch (Belikov.GenuineChannels.OperationException oe)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(oe);
                    Manager.Configurations = value;
                }
                catch (System.Runtime.Remoting.RemotingException re)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(re);
                    Manager.Configurations = value;
                }
                catch (Exception) //assume serialization exception;
                {
                    throw;
                }
            }
        }

        public override WatchDogData WatchDogData
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.WatchDogData;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.WatchDogData;
                }
            }
        }

        public override string GetWatchdogStatus()
        {
            try
            {
                return Program.CommunicationObject.Manager.GetWatchdogStatus();
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                return Manager.GetWatchdogStatus();
            }
        }

        #region External monitoring - SNMP and OPC UA interfaces


        #region External monitoring - Functionality common for SNMP and OPC UA

        /// <summary> Calls <see cref="CommunicationObjectWrapper.HandleBrokenConnection(ex)"/> 
        /// for <see cref="Program.CommunicationObject"/>  if it is not null. </summary>
        private void HandleBrokenConnection(Exception ex)
        {
            Program.CommunicationObject?.HandleBrokenConnection(ex);
        }

        public override string GetServerHostName()
        {
            try
            {
                return Program.CommunicationObject.Manager.GetServerHostName();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.GetServerHostName();
            }
        }

        #endregion


        #region External monitoring - SNMP


        #region SNMP Configuration

        public override SnmpData SnmpData
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.SnmpData;
                }
                catch (Exception ex)
                {
                    HandleBrokenConnection(ex);
                    return Manager.SnmpData;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.SnmpData = value;
                }
                catch (Exception ex)
                {
                    HandleBrokenConnection(ex);
                    Manager.SnmpData = value;
                }
            }
        }

        #endregion


        #region SNMP Objects

        public override void SnmpRebuildObjectTree()
        {
            try
            {
                Program.CommunicationObject.Manager.SnmpRebuildObjectTree();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                Manager.SnmpRebuildObjectTree();
            }
        }

        public override Dictionary<IbaSnmpOid, ExtMonData.GuiTreeNodeTag> SnmpGetObjectTreeSnapShot()
        {
            try
            {
                return Program.CommunicationObject.Manager.SnmpGetObjectTreeSnapShot();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.SnmpGetObjectTreeSnapShot();
            }
        }

        public override ExtMonData.GuiTreeNodeTag SnmpGetTreeNodeTag(IbaSnmpOid oid)
        {
            try
            {
                return Program.CommunicationObject.Manager.SnmpGetTreeNodeTag(oid);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.SnmpGetTreeNodeTag(oid);
            }
        }

        public override List<SnmpMibFileContainer> SnmpGenerateMibFiles()
        {
            try
            {
                return Program.CommunicationObject.Manager.SnmpGenerateMibFiles();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.SnmpGenerateMibFiles();
            }
        }

        #endregion


        #region SNMP Diagnostics

        public override Tuple<ExtMonWorkerStatus, string> SnmpGetBriefStatus()
        {
            try
            {
                return Program.CommunicationObject.Manager.SnmpGetBriefStatus();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.SnmpGetBriefStatus();
            }
        }
        
        public override List<IbaSnmpDiagClient> SnmpGetClients()
        {
            try
            {
                return Program.CommunicationObject.Manager.SnmpGetClients();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.SnmpGetClients();
            }
        }

        public override void SnmpClearClients()
        {
            try
            {
                Program.CommunicationObject.Manager.SnmpClearClients();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                Manager.SnmpClearClients();
            }
        }

        #endregion


        #endregion


        #region External monitoring - OPC UA


        #region OPC UA Configuration

        public override OpcUaData OpcUaData 
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.OpcUaData;
                }
                catch (Exception ex)
                {
                    HandleBrokenConnection(ex);
                    return Manager.OpcUaData;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.OpcUaData = value;
                }
                catch (Exception ex)
                {
                    HandleBrokenConnection(ex);
                    Manager.OpcUaData = value;
                }
            }
        }

        public override List<OpcUaData.CertificateTag> OpcUaHandleCertificate(string command, object args = null)
        {
            try
            {
                return Program.CommunicationObject.Manager.OpcUaHandleCertificate(command, args);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.OpcUaHandleCertificate(command, args);
            }
        }

        public override OpcUaData.NetworkConfiguration OpcUaGetNetworkConfiguration()
        {
            try
            {
                return Program.CommunicationObject.Manager.OpcUaGetNetworkConfiguration();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.OpcUaGetNetworkConfiguration();
            }
        }

        #endregion


        #region OPC UA Objects

        public override void OpcUaRebuildObjectTree()
        {
            try
            {
                Program.CommunicationObject.Manager.OpcUaRebuildObjectTree();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                Manager.OpcUaRebuildObjectTree();
            }
        }

        public override List<ExtMonData.GuiTreeNodeTag> OpcUaGetObjectTreeSnapShot()
        {
            try
            {
                return Program.CommunicationObject.Manager.OpcUaGetObjectTreeSnapShot();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.OpcUaGetObjectTreeSnapShot();
            }
        }

        public override ExtMonData.GuiTreeNodeTag OpcUaGetTreeNodeTag(string id)
        {
            try
            {
                return Program.CommunicationObject.Manager.OpcUaGetTreeNodeTag(id);
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.OpcUaGetTreeNodeTag(id);
            }
        }

        #endregion


        #region OPC UA Diagnostics

        public override Tuple<ExtMonWorkerStatus, string> OpcUaGetBriefStatus()
        {
            try
            {
                return Program.CommunicationObject.Manager.OpcUaGetBriefStatus();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.OpcUaGetBriefStatus();
            }
        }

        public override Tuple<List<IbaOpcUaDiagClient>, string> OpcUaGetDiagnostics()
        {
            try
            {
                return Program.CommunicationObject.Manager.OpcUaGetDiagnostics();
            }
            catch (Exception ex)
            {
                HandleBrokenConnection(ex);
                return Manager.OpcUaGetDiagnostics();
            }
        }

        #endregion


        #endregion


        #endregion


        public override int Count
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.Count;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.Count;
                }
            }
        }

        public override bool IsJobStarted(Guid guid)
        {
            try
            {
                return Program.CommunicationObject.Manager.IsJobStarted(guid);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                return Manager.IsJobStarted(guid);
            }
        }

        public override ConfigurationData GetConfigurationFromWorker(Guid guid)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetConfigurationFromWorker(guid);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                return Manager.GetConfigurationFromWorker(guid);
            }
        }

        public override MinimalStatusData GetMinimalStatus(Guid guid, bool permanentError)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetMinimalStatus(guid, permanentError);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                return Manager.GetMinimalStatus(guid, permanentError);
            }
        }

        public override PluginTaskWorkerStatus GetStatusPlugin(Guid guid, int taskindex)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetStatusPlugin(guid, taskindex);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                return Manager.GetStatusPlugin(guid, taskindex);
            }
        }

        public override void AlterPermanentFileErrorList(TaskManager.AlterPermanentFileErrorListWhatToDo todo, Guid guid, List<string> files)
        {
            try
            {
                Program.CommunicationObject.Manager.AlterPermanentFileErrorList(todo, guid, files);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void RemoveConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.RemoveConfiguration(data);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                Manager.RemoveConfiguration(data);
            }
        }

        public override void ReplaceConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceConfiguration(data);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                Manager.ReplaceConfiguration(data);
            }
        }

        public override void ReplaceConfigurations(List<ConfigurationData> datas)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceConfigurations(datas);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                Manager.ReplaceConfigurations(datas);
            }
        }

        public override void UpdateConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.UpdateConfiguration(data);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                Manager.UpdateConfiguration(data);
            }
        }

        public override void ReplaceWatchdogData(WatchDogData data)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceWatchdogData(data);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                Manager.ReplaceWatchdogData(data);
            }
        }

        public override void ReplaceGlobalCleanupData(GlobalCleanupData data)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceGlobalCleanupData(data);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                Manager.ReplaceGlobalCleanupData(data);
            }
        }

        public override bool CompareConfiguration(ConfigurationData data)
        {
            try
            {
                return Program.CommunicationObject.Manager.CompareConfiguration(data);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                return false;
            }
        }

        public override void StartAllEnabledConfigurationsNoOneTime()
        {
            try
            {
                Program.CommunicationObject.Manager.StartAllEnabledConfigurationsNoOneTime();
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void StartConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.StartConfiguration(data);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void StopAllConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.StopAllConfigurations();
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void StopAndWaitForAllConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.StopAndWaitForAllConfigurations();
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void StopAndWaitForConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.StopAndWaitForConfiguration(data);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void StopAndWaitForConfiguration(Guid guid)
        {
            try
            {
                Program.CommunicationObject.Manager.StopAndWaitForConfiguration(guid);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void StopConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.StopConfiguration(data);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void StopConfiguration(Guid guid)
        {
            try
            {
                Program.CommunicationObject.Manager.StopConfiguration(guid);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void UpdateTreePosition(Guid guid, int pos)
        {
            try
            {
                Program.CommunicationObject.Manager.UpdateTreePosition(guid, pos);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override bool TestPath(string dir, string user, string pass, out string errormessage, bool createnew, bool testWrite)
        {
            try
            {
                return Program.CommunicationObject.Manager.TestPath(dir, user, pass, out errormessage, createnew, testWrite);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                errormessage = iba.Properties.Resources.CouldNotTestPath;
                return false;
            }
        }

        public override bool DoPostponeProcessing
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.DoPostponeProcessing;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.DoPostponeProcessing;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.DoPostponeProcessing = value;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    Manager.DoPostponeProcessing = value;
                }
            }
        }

        public override int PostponeMinutes
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.PostponeMinutes;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.PostponeMinutes;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.PostponeMinutes = value;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    Manager.PostponeMinutes = value;
                }
            }
        }

        public override int MaxResourceIntensiveTasks
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.MaxResourceIntensiveTasks;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.MaxResourceIntensiveTasks;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.MaxResourceIntensiveTasks = value;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    Manager.MaxResourceIntensiveTasks = value;
                }
            }
        }

        public override int MaxSimultaneousIbaAnalyzers
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.MaxSimultaneousIbaAnalyzers;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.MaxSimultaneousIbaAnalyzers;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.MaxSimultaneousIbaAnalyzers = value;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    Manager.MaxSimultaneousIbaAnalyzers = value;
                }
            }
        }

        public override int MaxIbaAnalyzerCalls
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.MaxIbaAnalyzerCalls;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.MaxIbaAnalyzerCalls;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.MaxIbaAnalyzerCalls = value;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    Manager.MaxIbaAnalyzerCalls = value;
                }
            }
        }

        public override bool IsIbaAnalyzerCallsLimited
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.IsIbaAnalyzerCallsLimited;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.IsIbaAnalyzerCallsLimited;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.IsIbaAnalyzerCallsLimited = value;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    Manager.IsIbaAnalyzerCallsLimited = value;
                }
            }
        }

        public override int ProcessPriority
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.ProcessPriority;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.ProcessPriority;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.ProcessPriority = value;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    Manager.ProcessPriority = value;
                }
            }
        }

        public override KeyValuePair<string, string>[] AdditionalFileNames()
        {
            try
            {
                return Program.CommunicationObject.Manager.AdditionalFileNames();
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                return Manager.AdditionalFileNames();
            }
        }

        public override string Password
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.Password;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return "";
                }
            }
        }

        public override void ChangePassword(string newPassword, string initiator)
        {
            try
            {
                Program.CommunicationObject.Manager.ChangePassword(newPassword, initiator);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override TimeSpan RememberPassTime
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.RememberPassTime;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return TimeSpan.MinValue;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.RememberPassTime = value;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                }
            }
        }

        public override bool RememberPassEnabled
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.RememberPassEnabled;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return false;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.RememberPassEnabled = value;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                }
            }
        }


        public override void StartAllEnabledGlobalCleanups()
        {
            try
            {
                Program.CommunicationObject.Manager.StartAllEnabledGlobalCleanups();
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void StopAllGlobalCleanups()
        {
            try
            {
                Program.CommunicationObject.Manager.StopAllGlobalCleanups();
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override List<GlobalCleanupData> GlobalCleanupDataList
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.GlobalCleanupDataList;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return Manager.GlobalCleanupDataList;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.GlobalCleanupDataList = value;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    Manager.GlobalCleanupDataList = value;
                }
            }
        }


        public override void ForceTrigger(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.ForceTrigger(data);
            }
            catch(Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override void CleanAndProcessFileNow(ConfigurationData data, string file)
        {
            try
            {
                Program.CommunicationObject.Manager.CleanAndProcessFileNow(data,file);
            }
            catch(Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
            }
        }

        public override int ConfStoppedID
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.ConfStoppedID;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return 0;
                }
            }
        }

        public override int TaskManagerID
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.TaskManagerID;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return 0;
                }
            }
        }

        public override int Version
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.Version;
                }
                catch (Exception ex)
                {
                    if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                    return 0;
                }
            }
        }

        public override List<string> CheckPluginsAvailable(List<string> plugins)
        {
            int v = Version;
            if (v <  2000003 /*2.0.3*/) return null;
            try
            {
                return Program.CommunicationObject.Manager.CheckPluginsAvailable(plugins);
            }
            catch (Exception ex)
            {
                if (Program.CommunicationObject != null) Program.CommunicationObject.HandleBrokenConnection(ex);
                return null;
            }
        }
    }
}
