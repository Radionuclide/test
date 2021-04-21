using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Concurrent;

using iba.Data;
using iba.Utility;
using iba.Properties;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;
using System.IO;

namespace iba.Processing
{

    public class GlobalCleanupManager
    {
        private readonly ConcurrentDictionary<string, TaskControl> m_globalCleanupWorker;
        private List<GlobalCleanupData> m_globalCleanupDataList;

        public GlobalCleanupManager()
        {
            m_globalCleanupWorker = new ConcurrentDictionary<string, TaskControl>();
            m_globalCleanupDataList = new List<GlobalCleanupData>();
            UpdateGlobalCleanupData();
        }

        public List<GlobalCleanupData> GlobalCleanupDataList
        {
            get
            {
                UpdateGlobalCleanupData();
                return m_globalCleanupDataList;
            }
            set
            {
                m_globalCleanupDataList = value;
                UpdateGlobalCleanupData();
            }
        }

        private void UpdateGlobalCleanupData()
        {
            foreach (var drive in DriveUtil.LocalDrives())
            {
                var gcd = m_globalCleanupDataList.FirstOrDefault(x => x.DriveName == drive.Name);
                if (gcd == null)
                {
                    gcd = new GlobalCleanupData() { DriveName = drive.Name };
                    m_globalCleanupDataList.Add(gcd);
                }

                gcd.IsSystemDrive = drive.IsSystemDrive();
                gcd.IsReady = drive.IsReady;

                try
                {
                    gcd.VolumeLabel = drive.VolumeLabel;
                    gcd.TotalSize = drive.TotalSize;
                    gcd.TotalFreeSpace = drive.TotalFreeSpace;
                }
                catch (Exception ex)
                {
                    Log(Logging.Level.Debug, Resources.logGlobalCleanupDriveNotReadyForCleanup, gcd.DriveName);
                    Log(Logging.Level.Debug, ex.Message, gcd.DriveName);
                    gcd.IsReady = false;
                }
            }
        }

        public void StartAllEnabledGlobalCleanups()
        {
            foreach (var gcd in m_globalCleanupDataList)
                ReplaceGlobalCleanupData(gcd);
        }

        public void StopAllGlobalCleanups()
        {
            var workers = new List<TaskControl>(m_globalCleanupWorker.Values);
            workers.ForEach(tc => tc.Cts.Cancel());
            Task.WaitAll(workers.Select(tc => tc.Task).ToArray(), 60000);
        }

        private void StopGlobalCleanup(GlobalCleanupData data)
        {
            TaskControl tc;
            if (!m_globalCleanupWorker.TryRemove(data.DriveName, out tc))
                return;

            tc.Cts.Cancel();
            var ended = tc.Task.Wait(60000);
        }

        public void ReplaceGlobalCleanupData(GlobalCleanupData data)
        {
            StopGlobalCleanup(data);

            if (!data.Active)
                return;

            data.Active = DriveUtil.IsDriveReady(data.DriveName);
            if (!data.Active)
            {
                Log(Resources.logGlobalCleanupDriveNotReadyForCleanup, Logging.Level.Warning, data.DriveName);
                return;
            }

            var cts = new CancellationTokenSource();
            var taskData = new GlobalCleanupTaskData(new ConfigurationData("Global Cleanup", ConfigurationData.JobTypeEnum.DatTriggered));
            taskData.Name = "Cleanup drive " + data.DriveName;
            taskData.DestinationMapUNC = data.DriveName;

            if (data.IsSystemDrive && !String.IsNullOrEmpty(data.WorkingFolder))
                taskData.DestinationMapUNC = data.WorkingFolder;

            var task = Task.Factory.StartNew(() => StartGlobalCleanupWorker(data, taskData, cts.Token))
                .ContinueWith((_) =>
                {
                    TaskControl tc;
                    m_globalCleanupWorker.TryRemove(data.DriveName, out tc);
                    Log(Logging.Level.Info, Resources.logGlobalCleanupStop, taskData.DestinationMapUNC, taskData);
                });

            m_globalCleanupWorker.TryAdd(data.DriveName, new TaskControl(task, cts));
        }

        private void StartGlobalCleanupWorker(GlobalCleanupData data, GlobalCleanupTaskData taskData, CancellationToken ct)
        {
            Log(Logging.Level.Info, Resources.logGlobalCleanupStart, taskData.DestinationMapUNC, taskData);

            var drive = new DriveInfo(data.DriveName);
            if (!drive.IsReady)
            {
                Log(Logging.Level.Warning, iba.Properties.Resources.logGlobalCleanupDriveNotReadyForCleanup, taskData.DestinationMapUNC, taskData);
                return;
            }
            
            if (!VerifySystemDriveCouldRun(data, taskData))
            {
                data.Active = false;
                data.WorkingFolder = String.Empty;
                return;
            }

            PostponeStartup(ct);

            taskData.QuotaFree = (uint)((drive.TotalSize / 1024 / 1024) * (data.PercentageFree / 100.0));
            taskData.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.SaveFreeSpace;

            var globalQuota = new FileQuotaGlobalCleanup(taskData, ".dat", ct);
            var waitTimeSpan = TimeSpan.FromMinutes(data.RescanTime);

            bool cancelled = ct.IsCancellationRequested;
            while (!cancelled)
            {
                var sw = Stopwatch.StartNew();
                Log(Logging.Level.Info, Resources.logGlobalCleanupStartCleanup, taskData.DestinationMapUNC, taskData);
                globalQuota.Init();
                sw.Stop();
                Log(Logging.Level.Info, String.Format(Resources.logGlobalCleanupDetermineSizeFinished, sw.ElapsedMilliseconds / 1000.0), taskData.DestinationMapUNC, taskData);

                if (ct.IsCancellationRequested)
                    return;

                sw.Restart();
                globalQuota.Clean("Cleanup");
                sw.Stop();
                Log(Logging.Level.Info, String.Format(Resources.logGlobalCleanupFinished, sw.ElapsedMilliseconds / 1000.0), taskData.DestinationMapUNC, taskData);

                Log(Logging.Level.Info, String.Format(Resources.logGlobalCleanupNextCleanup, System.DateTime.Now.AddMinutes(data.RescanTime)), taskData.DestinationMapUNC, taskData);
                cancelled = ct.WaitHandle.WaitOne(waitTimeSpan);
            }
        }

        private bool VerifySystemDriveCouldRun(GlobalCleanupData data, GlobalCleanupTaskData taskData)
        {
            if (!data.IsSystemDrive)
                return true;

            if (String.IsNullOrEmpty(data.WorkingFolder))
            {
                Log(Logging.Level.Warning, Resources.logGlobalCleanupSystemDriveNeedsSubFolder, taskData.DestinationMapUNC, taskData);
                return false;
            }

            if (data.WorkingFolder.Equals(data.DriveName, StringComparison.OrdinalIgnoreCase))
            {
                Log(Logging.Level.Warning, Resources.logGlobalCleanupSystemDriveNeedsSubFolder, taskData.DestinationMapUNC, taskData);
                return false;
            }

            if (!data.WorkingFolder.StartsWith(data.DriveName))
            {
                Log(Logging.Level.Warning, String.Format(Resources.logGlobalCleanupFolderMustBeFolderOfDrive, data.DriveName), taskData.DestinationMapUNC, taskData);
                return false;
            }

            return true;
        }

        private static void PostponeStartup(CancellationToken ct)
        {
            bool bPostpone = TaskManager.Manager.DoPostponeProcessing;
            int minutes = TaskManager.Manager.PostponeMinutes;

            //wait until computer is fully started (if selected so)
            if (!bPostpone)
                return;

            var runningTicks = ((UInt32)System.Environment.TickCount);
            var postponeTicks = minutes * 60000;

            if (runningTicks >= postponeTicks)
                return;

            ct.WaitHandle.WaitOne(postponeTicks - (int)runningTicks);

        }

        private void Log(string message, string cleanupPath)
        {
            Log(message, iba.Logging.Level.Debug, cleanupPath);
        }

        private void Log(string message, Logging.Level level, string cleanupPath)
        {
            if (!String.IsNullOrEmpty(cleanupPath))
                message = String.Concat(cleanupPath, " - ", message);
            LogData.Data.Log(level, message);
        }

        private void Log(Logging.Level level, string message)
        {
            LogExtraData data = new LogExtraData(String.Empty, null, null);
            LogData.Data.Log(level, message, (object)data);
        }

        private void Log(Logging.Level level, string message, string datfile)
        {
            LogExtraData data = new LogExtraData(datfile, null, null);
            LogData.Data.Log(level, message, (object)data);
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
            LogExtraData data = new LogExtraData(datfile, task, task.ParentConfigurationData);
            LogData.Data.Log(level, message, (object)data);
            if (level == Logging.Level.Exception)
            {
                if (message != null && message.Contains("The operation"))
                {
                    string debug = message;
                }
            }
        }
    }
}
