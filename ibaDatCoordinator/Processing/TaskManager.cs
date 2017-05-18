using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using iba.Data;
using iba.Utility;
using iba.Plugins;
using System.IO;

namespace iba.Processing
{
    public class TaskManager : MarshalByRefObject
    {
        SortedDictionary<ConfigurationData, ConfigurationWorker> m_workers;

        virtual public void AddConfiguration(ConfigurationData data)
        {
            ConfigurationWorker cw = new ConfigurationWorker(data);
            lock (m_workers)
            {
                m_workers.Add(data, cw);
            }
            // added by kolesnik - begin
            SnmpWorker.TmpLogLine($"SnmpConfigurationChanged. AddConfiguration {data.Name}");
            SnmpConfigurationChanged?.Invoke(this, EventArgs.Empty);
            // added by kolesnik - end
        }

        virtual public void AddConfigurations(List<ConfigurationData> datas)
        {
            foreach (ConfigurationData data in datas)
                AddConfiguration(data);
        }

        virtual public void RemoveConfiguration(ConfigurationData data)
        {
            lock (m_workers)
            {
                if (m_workers.ContainsKey(data))
                {
                    StopConfiguration(data);
                    m_workers.Remove(data);
                }
            }

            // added by kolesnik - begin
            SnmpWorker.TmpLogLine($"SnmpConfigurationChanged. RemoveConfiguration {data.Name}");
            SnmpConfigurationChanged?.Invoke(this, EventArgs.Empty);
            // added by kolesnik - end
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

            // added by kolesnik - begin
            SnmpWorker.TmpLogLine($"SnmpConfigurationChanged. ReplaceConfiguration {data.Name}");
            SnmpConfigurationChanged?.Invoke(this, EventArgs.Empty);
            // added by kolesnik - end
        }

        private void ReplaceOrAddConfiguration(ConfigurationData data)
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

            // added by kolesnik - begin
            SnmpWorker.TmpLogLine($"SnmpConfigurationChanged. ReplaceOrAddConfiguration {data.Name}");
            SnmpConfigurationChanged?.Invoke(this, EventArgs.Empty);
            // added by kolesnik - end
        }

        virtual public void ReplaceConfigurations(List<ConfigurationData> datas)
        {
            foreach (ConfigurationData data in datas) ReplaceOrAddConfiguration(data);
            List<ConfigurationData> toRemove = new List<ConfigurationData>();
            //remove spurious configurations;
            foreach (ConfigurationData dat in m_workers.Keys)
            {
                if (!datas.Contains(dat)) //contains works because we've already replaced all datas (it would fail otherwise because contains
                    // uses  equality comparer instead of CompareTo)
                    toRemove.Add(dat);
            }
            foreach (ConfigurationData dat in toRemove)
            {
                RemoveConfiguration(dat);
            }
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

            // added by kolesnik - begin
            SnmpWorker.TmpLogLine($"SnmpConfigurationChanged. UpdateConfiguration {data.Name}");
            SnmpConfigurationChanged?.Invoke(this, EventArgs.Empty);
            // added by kolesnik - end
        }

        virtual public bool CompareConfiguration(ConfigurationData data)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == data.Guid)
                    return data.IsSame(pair.Key);
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
        }

        virtual public void StartAllEnabledConfigurationsNoOneTime()
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                if (kvp.Key.Enabled && kvp.Key.JobType != ConfigurationData.JobTypeEnum.OneTime)
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

            // added by kolesnik - begin
            SnmpWorker.TmpLogLine($"SnmpConfigurationChanged. StartConfiguration {data.Name}");
            SnmpConfigurationChanged?.Invoke(this, EventArgs.Empty);
            // added by kolesnik - end
        }

        virtual public void StopConfiguration(ConfigurationData data)
        {
            m_workers[data].Stop = true;

            //// added by kolesnik - begin
            //SnmpWorker.TmpLogLine($"SnmpConfigurationChanged. StopConfiguration {data.Name}");
            //SnmpConfigurationChanged?.Invoke(this, EventArgs.Empty);
            //// added by kolesnik - end
        }

        virtual public void StopConfiguration(Guid guid)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid)
                {
                    pair.Value.Stop = true;
                    //// added by kolesnik - begin
                    //SnmpWorker.TmpLogLine($"SnmpConfigurationChanged. StopConfiguration(Guid) {pair.Value.RunningConfiguration.Name}");
                    //SnmpConfigurationChanged?.Invoke(this, EventArgs.Empty);
                    //// added by kolesnik - end
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
            throw new KeyNotFoundException(guid.ToString() + " not found");
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
                    return (pair.Value.RunningConfiguration.Tasks[taskindex] as ICustomTaskData).Plugin.GetWorker().GetWorkerStatus();
            }
            return null;
        }

        //virtual public StatusData GetStatusCopy(Guid guid)
        //{
        //    foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
        //    {
        //        if (pair.Key.Guid == guid) return pair.Value.Status.Clone();
        //    }
        //    throw new KeyNotFoundException(guid.ToString() + " not found");
        //}

        public enum AlterPermanentFileErrorListWhatToDo { AFTERDELETE, AFTERREFRESH };
        virtual public void AlterPermanentFileErrorList(AlterPermanentFileErrorListWhatToDo todo, Guid guid, List<string> files)
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


        #region Snmp Data - added by Kolesnik

        /// <summary> 
        /// Is fired when there is a chance (yes, at least a chance) that snmp data structure (amount of jobs, tasks, etc) is changed. 
        /// So, SnmpWorker can know this, and may rebuild its SNMP objects tree accordingly.
        /// It does not guarantee that data is really changed, but only just that it might have changed.
        /// (SnmpWorker's handler is very lightweight, so it's better to trigger this event
        /// more often (e.g. let even twice for each real change - no matter), than 
        /// to miss some point (even some that happens seldom) where it is changed.
        /// Event is not relevant to some 'small' data changes,
        /// i.e changes that do not alter the structure (hierarcy) of the snmp tree (e.g. status of the job, or some other value).
        /// </summary>
        public event EventHandler<EventArgs> SnmpConfigurationChanged;

        public SnmpWorker SnmpWorker { get; } = new SnmpWorker();

        /// <summary> Gets/sets data of SnmpWorker. 
        /// If data is set, then restart of snmp agent is performed if necessary. </summary>
        public virtual SnmpData SnmpData
        {
            get { return SnmpWorker.SnmpData; }
            set { SnmpWorker.SnmpData = value; }
        }

        internal bool SnmpRefreshLicenseInfo(SnmpObjectsData.LicenseInfo licenseInfo)
        {
            licenseInfo.Reset();

            // this feature is not licensed,
            // so does not need any condition to be true
            licenseInfo.IsValid = true;

            try
            {
                CDongleInfo info = CDongleInfo.ReadDongle();
                // ReSharper disable once RedundantBoolCompare
                if (info != null && info.DongleFound == true)
                {
                    licenseInfo.Sn = info.SerialNr;
                    licenseInfo.HwId = info.HwId;
                    licenseInfo.DongleType = info.DongleType;
                    licenseInfo.Customer = info.Customer;
                    licenseInfo.TimeLimit = info.TimeLimit;
                    licenseInfo.DemoTimeLimit = info.DemoTimeLimit;
                }

                licenseInfo.PutTimeStamp();
            }
            catch {/**/}

            return true;
        }

        internal bool SnmpRefreshGlobalCleanupDriveInfo(SnmpObjectsData.GlobalCleanupDriveInfo driveInfo)
        {
            // reset values for the case of an update error
            driveInfo.Reset();

            if (String.IsNullOrEmpty(driveInfo.DriveName))
            {
                return false; // failed to update
            }

            try
            {
                lock (m_workers)
                {
                    var gcData = GlobalCleanupDataList.FirstOrDefault(gc => gc.DriveName == driveInfo.DriveName);

                    if (gcData == null)
                    {
                        // needed GlobalCleanupDataList not found
                        return false; // failed to update
                    }

                    // set current values
                    SnmpRefreshGlobalCleanupDriveInfo(driveInfo, gcData);
                    return true; // data was updated
                }
            }
            catch (Exception ex)
            {
                // suppress
                // for the case of change of GlobalCleanupDataList 
                // during FirstOrDefault query or values update
                SnmpWorker.TmpLogLine(ex.ToString());
            }
            return false; // failed to update
        }

        private void SnmpRefreshGlobalCleanupDriveInfo(
            SnmpObjectsData.GlobalCleanupDriveInfo driveInfo, GlobalCleanupData gcData)
        {
            driveInfo.Reset();

            driveInfo.Active = gcData.Active; //1

            DriveInfo drive = new DriveInfo(gcData.DriveName);
            if (drive.IsReady)
            {
                driveInfo.SizeInMb = (uint)(drive.TotalSize >> 20); //2 
                driveInfo.CurrentFreeSpaceInMb = (uint)(drive.TotalFreeSpace >> 20); // 3 
            }

            // here I use the same formula  as in ServiceSettingsControl.cs, but with conversion to MB:
            // ... = PathUtil.GetSizeReadable((long)(driveSize * (data.PercentageFree / 100.0)));
            driveInfo.MinFreeSpaceInMb = (uint)(driveInfo.SizeInMb * (gcData.PercentageFree / 100.0)); // 4a // 
            driveInfo.MinFreeSpaceInPercent = (uint)gcData.PercentageFree; // 4b

            driveInfo.RescanTime = (uint)gcData.RescanTime; //5

            driveInfo.PutTimeStamp();
            SnmpWorker.TmpLogLine($"TskMgr. Refreshed Drive {driveInfo.DriveName}");
        }

        internal bool SnmpRefreshJobInfo(SnmpObjectsData.JobInfoBase jobInfo)
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
                    switch (cfg.JobType)
                    {
                        case ConfigurationData.JobTypeEnum.DatTriggered: // standard Job
                            SnmpRefreshStandardJobInfo(jobInfo as SnmpObjectsData.StandardJobInfo, cfg);
                            break;

                        case ConfigurationData.JobTypeEnum.Scheduled:
                            SnmpRefreshScheduledJobInfo(jobInfo as SnmpObjectsData.ScheduledJobInfo, cfg);
                            break;

                        case ConfigurationData.JobTypeEnum.OneTime:
                            SnmpRefreshOneTimeJobInfo(jobInfo as SnmpObjectsData.OneTimeJobInfo, cfg);
                            break;

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

                    // updated successfully
                    return true;
                }
            }
            catch
            {
                // suppress
                // for the case of change of GlobalCleanupDataList 
                // within forach loop by another thread
                // todo check for lock protection of GlobalCleanupDataList 
            }

            // error
            return false; // failed to update
        }

        private void SnmpRefreshStandardJobInfo(SnmpObjectsData.StandardJobInfo jobInfo, ConfigurationData cfg)
        {
            jobInfo.Reset();

            ConfigurationWorker worker = m_workers[cfg];
            StatusData s = worker.Status;

            SnmpRefreshJobInfoBase(jobInfo, s);

            jobInfo.PermFailedCount = (uint)s.PermanentErrorFiles.Count; // 5
            jobInfo.TimestampJobStarted = worker.TimestampJobStarted; //6
            jobInfo.LastCycleScanningTime_TimestampLastDirectoryScan = worker.TimestampLastDirectoryScan;  // todo 7a
            jobInfo.LastCycleScanningTime_TimestampLastReprocessErrorsScan = worker.TimestampLastReprocessErrorsScan; // todo 7b

            jobInfo.LastProcessingLastDatFileProcessed = worker.LastSuccessfulFileName; // 80
            jobInfo.LastProcessingStartTimeStamp = worker.LastSuccessfulFileStartProcessingTimeStamp; // 81
            jobInfo.LastProcessingFinishTimeStamp = worker.LastSuccessfulFileFinishProcessingTimeStamp; // 82

            jobInfo.PutTimeStamp();

            SnmpWorker.TmpLogLine($"TskMgr. Refreshed Job {jobInfo.JobName}");
        }

        private void SnmpRefreshScheduledJobInfo(SnmpObjectsData.ScheduledJobInfo jobInfo, ConfigurationData cfg)
        {
            jobInfo.Reset();

            ConfigurationWorker worker = m_workers[cfg];
            StatusData s = worker.Status;

            SnmpRefreshJobInfoBase(jobInfo, s);

            jobInfo.PermFailedCount = (uint)s.PermanentErrorFiles.Count; //5
            jobInfo.TimestampLastExecution = worker.LastSuccessfulFileStartProcessingTimeStamp; // 6
            jobInfo.TimestampNextExecution = worker.NextTrigger; // 7

            jobInfo.PutTimeStamp();
            SnmpWorker.TmpLogLine($"TskMgr. Refreshed Job {jobInfo.JobName}");
        }

        private void SnmpRefreshOneTimeJobInfo(SnmpObjectsData.OneTimeJobInfo jobInfo, ConfigurationData cfg)
        {
            jobInfo.Reset();

            ConfigurationWorker worker = m_workers[cfg];
            StatusData s = worker.Status;

            SnmpRefreshJobInfoBase(jobInfo, s);

            jobInfo.TimestampLastExecution = worker.TimestampJobStarted;//5

            jobInfo.PutTimeStamp();
            SnmpWorker.TmpLogLine($"TskMgr. Refreshed Job {jobInfo.JobName}");
        }

        private void SnmpRefreshJobInfoBase(SnmpObjectsData.JobInfoBase ji, StatusData s)
        {
            var cfg = s.CorrConfigurationData;
            ji.JobName = cfg.Name;
            ji.Status = !cfg.Enabled ?
                SnmpObjectsData.JobStatus.Disabled :
                (s.Started ?
                    SnmpObjectsData.JobStatus.Started :
                    SnmpObjectsData.JobStatus.Stopped);

            ji.TodoCount = (uint)s.ReadFiles.Count;
            ji.DoneCount = (uint)s.ProcessedFiles.Count;
            ji.FailedCount = (uint)s.CountErrors();

            SnmpRefreshTasks(ji,s);
        }

        private void SnmpRefreshTasks(SnmpObjectsData.JobInfoBase ji, StatusData s)
        {
            var cfg = s.CorrConfigurationData;

            // on first call for the job create a list first
            if (ji.Tasks == null)
            {
                ji.Tasks = new List<SnmpObjectsData.TaskInfo>();

                for (int i = 0; i < cfg.Tasks.Count; i++)
                {
                    ji.Tasks.Add(new SnmpObjectsData.TaskInfo { Parent = ji });
                }
            }

            // fill task's data
            if (ji.Tasks.Count != cfg.Tasks.Count)
            {
                ji.Tasks.Clear();
                SnmpWorker.TmpLogLine("SnmpRefreshTasks. Tasks amount has changed. Configuration invalidated");
                SnmpConfigurationChanged?.Invoke(this, EventArgs.Empty);
                return;
            }

            for (int i = 0; i < cfg.Tasks.Count; i++)
            {
                TaskData taskData = cfg.Tasks[i];
                var taskInfo = ji.Tasks[i];
                taskInfo.Reset();

                taskInfo.TaskName = taskData.Name;

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
                        
                        // todo for Michael. Please check the following name:
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
                {
                    if (taskData is BatchFileData)
                    {
                        taskTypeStr = "Script";
                        //var typedData = taskData as BatchFileData;
                    }
                    else if (taskData is CleanupTaskData)
                    {
                        taskTypeStr = "Cleanup";
                        //var typedData = taskData as CleanupTaskData;
                    }
                    else if (taskData is CustomTaskData)
                    {
                        var typedData = taskData as CustomTaskData;
                        // todo for Michael. Please check the following name:
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
                }

                taskInfo.TaskType = taskTypeStr;

                // if cleanup info is present, add it to the task
                var cleanupTaskData = taskData as CleanupTaskData;
                if (cleanupTaskData != null)
                {
                    taskInfo.CleanupInfo = new SnmpObjectsData.LocalCleanupInfo
                    {
                        LimitChoice = cleanupTaskData.OutputLimitChoice,
                        FreeDiskSpace = cleanupTaskData.QuotaFree,
                        Subdirectories = cleanupTaskData.SubfoldersNumber,
                        UsedDiskSpace = cleanupTaskData.Quota
                    };
                }
                else
                {
                    taskInfo.CleanupInfo = null;
                }

                //taskInfo.Success = taskData.Enabled;//2

                // todo
                taskInfo.DurationOfLastExecution = 99999; //taskData.;//3 
                // todo
                taskInfo.CurrentMemoryUsed = 99999; //4
                // todo
            }
        }

        internal bool SnmpRebuildObjectsData(SnmpObjectsData od)
        {
            try
            {
                SnmpWorker.TmpLogLine("TskMgr. ----------------------");
                SnmpWorker.TmpLogLine("TskMgr. Rebuilding ObjectsData");

                od.Reset();

                lock (m_workers)
                {
                    // PrGen.3. License
                    {
                        // nothing to create there
                        // just refresh data
                        SnmpRefreshLicenseInfo(od.License);
                    }

                    // 1. GlobalCleanup;
                    {
                        try
                        {
                            foreach (var gcData in GlobalCleanupDataList.OrderBy(gc => gc.DriveName))
                            {
                                // create entry
                                var driveInfo = new SnmpObjectsData.GlobalCleanupDriveInfo
                                {
                                    // set primary key
                                    DriveName = gcData.DriveName
                                };
                                od.GlobalCleanup.Add(driveInfo);

                                // set current values
                                SnmpRefreshGlobalCleanupDriveInfo(driveInfo, gcData);
                            }
                        }
                        catch (Exception ex)
                        {
                            // suppress
                            // for the case of change of GlobalCleanupDataList 
                            // within forach loop by another thread
                            // todo check for lock protection of GlobalCleanupDataList 

                            // todo log?
                            SnmpWorker.TmpLogLine(ex.ToString());
                        }
                    }
                    // 2...4. - Jobs
                    {
                        // get copy of configurations
                        List<ConfigurationData> confs = Configurations;
                        confs.Sort((a, b) => a.TreePosition.CompareTo(b.TreePosition));

                        foreach (ConfigurationData cfg in confs)
                        {
                            switch (cfg.JobType)
                            {
                                case ConfigurationData.JobTypeEnum.DatTriggered: // standard Job
                                    var stdJobInfo = new SnmpObjectsData.StandardJobInfo {Guid = cfg.Guid};
                                    od.StandardJobs.Add(stdJobInfo);
                                    // fill the data
                                    SnmpRefreshStandardJobInfo(stdJobInfo, cfg);
                                    break;

                                case ConfigurationData.JobTypeEnum.Scheduled:
                                    var schJobInfo = new SnmpObjectsData.ScheduledJobInfo {Guid = cfg.Guid};
                                    od.ScheduledJobs.Add(schJobInfo);
                                    // fill the data
                                    SnmpRefreshScheduledJobInfo(schJobInfo, cfg);
                                    break;

                                case ConfigurationData.JobTypeEnum.OneTime:
                                    var otJobInfo = new SnmpObjectsData.OneTimeJobInfo {Guid = cfg.Guid};
                                    od.OneTimeJobs.Add(otJobInfo);
                                    // fill the data
                                    SnmpRefreshOneTimeJobInfo(otJobInfo, cfg);
                                    break;

                                default:
                                    throw new ArgumentOutOfRangeException();
                            }
                        }
                    }
                }

                SnmpWorker.TmpLogLine("TskMgr. Snmp Rebuilt ObjectsData");
                SnmpWorker.TmpLogLine("TskMgr. ----------------------");
                return true; // success
            }
            catch (Exception ex)
            {
                // todo log?    
                return false; // error
            }
        }

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
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && RemoteTaskManager != null)
                {
                    return RemoteTaskManager;
                }
                else if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                {
                    RemoteTaskManager = new TaskManagerWrapper();
                    return RemoteTaskManager;
                }
                else if (theTaskManager == null)
                {
                    theTaskManager = new TaskManager();
                }
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
            set { m_password = value; }
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


        virtual public void AdditionalFileNames(List<KeyValuePair<string, string>> myList)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key != null)
                    pair.Key.AdditionalFileNames(myList);
            }
        }

        private GlobalCleanupManager m_globalCleanup;
        public virtual List<GlobalCleanupData> GlobalCleanupDataList
        {
            get { return m_globalCleanup.GlobalCleanupDataList; }
            set { m_globalCleanup.GlobalCleanupDataList = value; }
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
                errMessage = FileProcessing.RemoveMarkingsFromFile(file);
            }
            catch (System.Exception ex)
            {
                errMessage = ex.Message;
            }
            if(!string.IsNullOrEmpty(errMessage))
            {
                LogData.Data.Logger.Log(iba.Logging.Level.Exception, Properties.Resources.RemoveMarkingsProblem + " " + errMessage, new LogExtraData(file,null,data));
            }

            m_workers[data].ProcessFileDirect(file);
        }

        public virtual void CopyIbaAnalyzerFiles(string sourcePath)
        {
            string targetPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            targetPath = Path.Combine(targetPath, "iba", "ibaAnalyzer");
            if(!Directory.Exists(targetPath))
                Directory.CreateDirectory(targetPath);
            var extensions = new[] { ".mcr", ".fil", ".xml" }; 
            var files = (from file in Directory.EnumerateFiles(sourcePath)
                         where extensions.Contains(Path.GetExtension(file), StringComparer.InvariantCultureIgnoreCase) // comment this out if you don't want to filter extensions
                         select new
                         {
                             Source = file,
                             Destination = Path.Combine(targetPath, Path.GetFileName(file))
                         });

            foreach(var file in files)
            {
                File.Copy(file.Source, file.Destination,true);
            }
        }

        public virtual void RegisterIbaAnalyzerSettings(string outFile)
        {
            System.Diagnostics.Process regeditProcess = System.Diagnostics.Process.Start("regedit.exe", "/s \"" + outFile + "\"");
            regeditProcess.WaitForExit();
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
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.AddConfiguration(data);
            }
        }

        public override void AddConfigurations(List<ConfigurationData> datas)
        {
            try
            {
                Program.CommunicationObject.Manager.AddConfigurations(datas);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.AddConfigurations(datas);
            }
        }

        public override void ClearConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.ClearConfigurations();
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.Configurations;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.Configurations = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    Manager.Configurations = value;
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
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
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return Manager.GetWatchdogStatus();
            }
        }

        public override int Count
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.Count;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.Count;
                }
            }
        }

        //public override StatusData GetStatus(Guid guid)
        //{
        //    try
        //    {
        //        //if remote, get a copy instead of the status itself
        //        return Program.CommunicationObject.Manager.GetStatusCopy(guid);
        //    }
        //    catch (Exception)
        //    {
        //        Program.CommunicationObject.HandleBrokenConnection();
        //        return Manager.GetStatus(guid);
        //    }
        //}

        public override bool IsJobStarted(Guid guid)
        {
            try
            {
                return Program.CommunicationObject.Manager.IsJobStarted(guid);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return Manager.IsJobStarted(guid);
            }
        }

        public override ConfigurationData GetConfigurationFromWorker(Guid guid)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetConfigurationFromWorker(guid);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return Manager.GetConfigurationFromWorker(guid);
            }
        }

        public override MinimalStatusData GetMinimalStatus(Guid guid, bool permanentError)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetMinimalStatus(guid, permanentError);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return Manager.GetMinimalStatus(guid, permanentError);
            }
        }

        public override PluginTaskWorkerStatus GetStatusPlugin(Guid guid, int taskindex)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetStatusPlugin(guid, taskindex);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return Manager.GetStatusPlugin(guid, taskindex);
            }
        }

        //public override StatusData GetStatusCopy(Guid guid)
        //{
        //    try
        //    {
        //        return Program.CommunicationObject.Manager.GetStatusCopy(guid);
        //    }
        //    catch (Exception)
        //    {
        //        Program.CommunicationObject.HandleBrokenConnection();
        //        return Manager.GetStatusCopy(guid);
        //    }
        //}

        public override void AlterPermanentFileErrorList(TaskManager.AlterPermanentFileErrorListWhatToDo todo, Guid guid, List<string> files)
        {
            try
            {
                Program.CommunicationObject.Manager.AlterPermanentFileErrorList(todo, guid, files);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void RemoveConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.RemoveConfiguration(data);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.RemoveConfiguration(data);
            }
        }

        public override void ReplaceConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceConfiguration(data);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.ReplaceConfiguration(data);
            }
        }

        public override void ReplaceConfigurations(List<ConfigurationData> datas)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceConfigurations(datas);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.ReplaceConfigurations(datas);
            }
        }

        public override void UpdateConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.UpdateConfiguration(data);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.UpdateConfiguration(data);
            }
        }

        public override void ReplaceWatchdogData(WatchDogData data)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceWatchdogData(data);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.ReplaceWatchdogData(data);
            }
        }

        public override void ReplaceGlobalCleanupData(GlobalCleanupData data)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceGlobalCleanupData(data);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.ReplaceGlobalCleanupData(data);
            }
        }

        public override bool CompareConfiguration(ConfigurationData data)
        {
            try
            {
                return Program.CommunicationObject.Manager.CompareConfiguration(data);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return false;
            }
        }

        public override void StartAllEnabledConfigurationsNoOneTime()
        {
            try
            {
                Program.CommunicationObject.Manager.StartAllEnabledConfigurationsNoOneTime();
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StartConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.StartConfiguration(data);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopAllConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.StopAllConfigurations();
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopAndWaitForAllConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.StopAndWaitForAllConfigurations();
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopAndWaitForConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.StopAndWaitForConfiguration(data);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopAndWaitForConfiguration(Guid guid)
        {
            try
            {
                Program.CommunicationObject.Manager.StopAndWaitForConfiguration(guid);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.StopConfiguration(data);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopConfiguration(Guid guid)
        {
            try
            {
                Program.CommunicationObject.Manager.StopConfiguration(guid);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void UpdateTreePosition(Guid guid, int pos)
        {
            try
            {
                Program.CommunicationObject.Manager.UpdateTreePosition(guid, pos);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override bool TestPath(string dir, string user, string pass, out string errormessage, bool createnew, bool testWrite)
        {
            try
            {
                return Program.CommunicationObject.Manager.TestPath(dir, user, pass, out errormessage, createnew, testWrite);
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.DoPostponeProcessing;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.DoPostponeProcessing = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.PostponeMinutes;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.PostponeMinutes = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.MaxResourceIntensiveTasks;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.MaxResourceIntensiveTasks = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.MaxSimultaneousIbaAnalyzers;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.MaxSimultaneousIbaAnalyzers = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.MaxIbaAnalyzerCalls;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.MaxIbaAnalyzerCalls = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.IsIbaAnalyzerCallsLimited;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.IsIbaAnalyzerCallsLimited = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.ProcessPriority;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.ProcessPriority = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    Manager.ProcessPriority = value;
                }
            }
        }

        public override void AdditionalFileNames(List<KeyValuePair<string, string>> myList)
        {
            try
            {
                Program.CommunicationObject.Manager.AdditionalFileNames(myList);
            }
            catch (Exception)
            {
                myList.Clear();
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.AdditionalFileNames(myList);
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return "";
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.Password = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                }
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return TimeSpan.MinValue;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.RememberPassTime = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return false;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.RememberPassEnabled = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                }
            }
        }


        public override void StartAllEnabledGlobalCleanups()
        {
            try
            {
                Program.CommunicationObject.Manager.StartAllEnabledGlobalCleanups();
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopAllGlobalCleanups()
        {
            try
            {
                Program.CommunicationObject.Manager.StopAllGlobalCleanups();
            }
            catch (Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
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
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.GlobalCleanupDataList;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.GlobalCleanupDataList = value;
                }
                catch (Exception)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
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
            catch(Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void CleanAndProcessFileNow(ConfigurationData data, string file)
        {
            try
            {
                Program.CommunicationObject.Manager.CleanAndProcessFileNow(data,file);
            }
            catch(Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }


        public override void RegisterIbaAnalyzerSettings(string outFile)
        {
            try
            {
                Program.CommunicationObject.Manager.RegisterIbaAnalyzerSettings(outFile);
            }
            catch(Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void CopyIbaAnalyzerFiles(string folder)
        {
            try
            {
                Program.CommunicationObject.Manager.CopyIbaAnalyzerFiles(folder);
            }
            catch(Exception)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }
    }
}
