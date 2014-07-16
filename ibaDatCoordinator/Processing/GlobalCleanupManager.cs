
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
            var task = Task.Factory.StartNew(() => StartGlobalCleanupWorker(data, cts.Token))
                .ContinueWith((_) =>
                {
                    TaskControl tc;
                    m_globalCleanupWorker.TryRemove(data.DriveName, out tc);
                    Log(Logging.Level.Info, "Stop Global Cleanup", data.DriveName);
                });

            m_globalCleanupWorker.TryAdd(data.DriveName, new TaskControl(task, cts));
        }

        private void StartGlobalCleanupWorker(GlobalCleanupData data, CancellationToken ct)
        {
            var taskData = new GlobalCleanupTaskData(new ConfigurationData(data.DriveName, ConfigurationData.JobTypeEnum.DatTriggered));
            taskData.DestinationMapUNC = data.DriveName;

            Log(Logging.Level.Info, "Start Global Cleanup", data.DriveName, taskData);

            PostponeStartup(ct);

            taskData.OutputLimitChoice = TaskDataUNC.OutputLimitChoiceEnum.LimitDiskspace;
            double factor = 1 - data.PercentageFree / 100.0;
            taskData.Quota = (uint)((data.TotalSize / 1024 / 1024) * factor);

            var quota = new FileQuotaCleanup(taskData, ".dat", ct);
            quota.FastSearch = true;
            //quota.LogMessageReceived += (s, e) => { Log(e.Level, e.Message, taskData.DestinationMapUNC); };


            bool cancelled = ct.IsCancellationRequested;
            while (!cancelled)
            {
                var sw = Stopwatch.StartNew();
                Log("Cleanup start", taskData.DestinationMapUNC);
                quota.Init();
                sw.Stop();
                Log("Cleanup init finished " + (sw.ElapsedMilliseconds / 1000.0).ToString("0.000") + "s", taskData.DestinationMapUNC);

                if (ct.IsCancellationRequested)
                    return;

                sw.Restart();
                quota.Clean("Cleanup");
                sw.Stop();
                Log("Cleanup finished " + (sw.ElapsedMilliseconds / 1000.0).ToString("0.000") + "s", taskData.DestinationMapUNC);

                Log(Logging.Level.Info, String.Format("Next cleanup at {0:T}", System.DateTime.Now.AddMinutes(data.RescanTime)), data.DriveName);
                cancelled = ct.WaitHandle.WaitOne(data.RescanTime * 60000);
            }


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
            LogExtraData data = new LogExtraData(datfile, task, null);
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
