
namespace iba.Processing
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Collections.Concurrent;

    using iba.Data;
    using iba.Utility;
    using System.Threading.Tasks;
    using System.Threading;
    using System.Diagnostics;

    public class GlobalCleanupManager
    {
        private ConcurrentDictionary<string, TaskControl> m_globalCleanupWorker;
        private List<GlobalCleanupData> m_globalCleanupDataList;

        public GlobalCleanupManager()
        {
            m_globalCleanupWorker = new ConcurrentDictionary<string, TaskControl>();
            m_globalCleanupDataList = new List<GlobalCleanupData>();
        }

        public List<GlobalCleanupData> GlobalCleanupDataList
        {
            get
            {
                SyncGlobalCleanupDataList();
                return m_globalCleanupDataList;
            }
            set
            {
                m_globalCleanupDataList = value;
                SyncGlobalCleanupDataList();
            }
        }


        private void SyncGlobalCleanupDataList()
        {
            foreach (var drive in DriveUtil.LocalDrives())
            {
                var gcd = m_globalCleanupDataList.FirstOrDefault(x => x.DriveName == drive.Name);
                if (gcd == null)
                {
                    gcd = new GlobalCleanupData() { DriveName = drive.Name };
                    m_globalCleanupDataList.Add(gcd);
                }
            }

            DisableCleanupForNonexistingDrives();
        }

        private void DisableCleanupForNonexistingDrives()
        {
            var localDriveNames = new HashSet<string>(DriveUtil.LocalDrives().Select(d => d.Name));
            m_globalCleanupDataList.ForEach(gcd => gcd.Active = gcd.Active && localDriveNames.Contains(gcd.DriveName));
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
                Log("Drive not ready for cleanup", Logging.Level.Warning, data.DriveName);
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
                    Log(Logging.Level.Info, "Stop Global Cleanup", taskData.DestinationMapUNC, taskData);
                });

            m_globalCleanupWorker.TryAdd(data.DriveName, new TaskControl(task, cts));
        }

        private void StartGlobalCleanupWorker(GlobalCleanupData data, GlobalCleanupTaskData taskData, CancellationToken ct)
        {
            if (!VerifySystemDriveCouldRun(data, taskData))
            {
                data.Active = false;
                data.WorkingFolder = String.Empty;
                return;
            }

            Log(Logging.Level.Info, "Start Global Cleanup", taskData.DestinationMapUNC, taskData);

            PostponeStartup(ct);

            taskData.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            double factor = 1 - data.PercentageFree / 100.0;
            taskData.Quota = (uint)((data.TotalSize / 1024 / 1024) * factor);

            var quota = new FileQuotaCleanup(taskData, ".dat", ct);
            quota.FastSearch = true;

            bool cancelled = ct.IsCancellationRequested;
            while (!cancelled)
            {
                var sw = Stopwatch.StartNew();
                Log(Logging.Level.Info, "Cleanup start", taskData.DestinationMapUNC, taskData);
                quota.Init();
                sw.Stop();
                Log(Logging.Level.Info, "Cleanup init finished " + (sw.ElapsedMilliseconds / 1000.0).ToString("0.000") + "s", taskData.DestinationMapUNC, taskData);

                if (ct.IsCancellationRequested)
                    return;

                sw.Restart();
                quota.Clean("Cleanup");
                sw.Stop();
                Log(Logging.Level.Info, "Cleanup finished " + (sw.ElapsedMilliseconds / 1000.0).ToString("0.000") + "s", taskData.DestinationMapUNC, taskData);

                Log(Logging.Level.Info, String.Format("Next cleanup at {0:T}", System.DateTime.Now.AddMinutes(data.RescanTime)), taskData.DestinationMapUNC, taskData);
                cancelled = ct.WaitHandle.WaitOne(data.RescanTime * 60000);
            }


        }

        private bool VerifySystemDriveCouldRun(GlobalCleanupData data, GlobalCleanupTaskData taskData)
        {
            if (!data.IsSystemDrive)
                return true;

            if (String.IsNullOrEmpty(data.WorkingFolder))
            {
                Log(Logging.Level.Warning, "System drive must have a sub folder defined!", taskData.DestinationMapUNC, taskData);
                return false;
            }

            if (data.WorkingFolder.Equals(data.DriveName, StringComparison.OrdinalIgnoreCase))
            {
                Log(Logging.Level.Warning, "System drive must have a sub folder defined!", taskData.DestinationMapUNC, taskData);
                return false;
            }

            if (!data.WorkingFolder.StartsWith(data.DriveName))
            {
                Log(Logging.Level.Warning, String.Format("Working folder must must be a sub folder of drive {0}!", data.DriveName), taskData.DestinationMapUNC, taskData);
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

            while (((UInt32)System.Environment.TickCount) / 60000 < minutes)
            {
                if (ct.IsCancellationRequested)
                    break;

                Thread.Sleep(TimeSpan.FromSeconds(5.0));
            }
        }

        private void Log(string message, string cleanupPath)
        {
            Log(message, iba.Logging.Level.Debug, cleanupPath);
        }

        private void Log(string message, iba.Logging.Level level, string cleanupPath)
        {
            if (!String.IsNullOrEmpty(cleanupPath))
                message = String.Concat(cleanupPath, " - ", message);
            LogData.Data.Log(level, message);
        }

        internal void Log(Logging.Level level, string message)
        {
            LogExtraData data = new LogExtraData(String.Empty, null, null);
            LogData.Data.Log(level, message, (object)data);
        }

        internal void Log(Logging.Level level, string message, string datfile)
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

        internal void Log(Logging.Level level, string message, string datfile, TaskData task)
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
