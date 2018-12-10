using iba.Data;
using iba.Logging;
using iba.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace iba.Utility
{
    internal class EventJobTestStatus
    {
        public string StatusMessage;
        public Level StatusLevel;
        public bool Finished;

        public EventJobTestStatus(Level lvl, string msg, bool bFinished)
        {
            StatusLevel = lvl;
            StatusMessage = msg;
            Finished = bFinished;
        }
    }

    internal class EventJobTestGenerator : IDisposable
    {
        #region Members
        ConfigurationData m_confData;
        EventJobData m_ejd;
        string m_fileName;
        int m_status;
        Thread m_runThread;

        const int maxTimeEventQuery = 5;

        public Action<EventJobTestStatus> StatusChanged;
        public List<string> GeneratedFiles { get; private set; }
        #endregion

        #region Initialize
        public EventJobTestGenerator(ConfigurationData confData, string fileName)
        {
            m_confData = confData.Clone() as ConfigurationData;
            m_ejd = m_confData.EventData;
            m_fileName = fileName;
            m_status = 0;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Cancel();
        }
        #endregion

        #region Generate
        public void Start()
        {
            if (Interlocked.CompareExchange(ref m_status, 1, 0) != 0)
                return;

            GeneratedFiles = null;

            m_runThread = new Thread(new ThreadStart(Run));
            m_runThread.Name = "Event job test file generator thread";
            m_runThread.Start();
        }

        public void Cancel()
        {
            if (Interlocked.CompareExchange(ref m_status, -1, 1) != 1)
                return;

            if (m_runThread != null)
            {
                if (m_runThread.IsAlive)
                    m_runThread.Join(2000);

                m_runThread = null;
            }
        }

        void Run()
        {
            bool bCancelled = false;
            string errorMsg = string.Empty;
            string[] lStores = m_ejd.HDStores;
            try
            {
                if (m_status == -1)
                {
                    bCancelled = true;
                    return;
                }

                StatusChanged?.Invoke(new EventJobTestStatus(Level.Info, Properties.Resources.EventJob_TestFile_SearchingEvent, false));

                //Query events
                List<HDEventMonitor.EventOccurrence> evts = null;
                using (HDEventMonitor monitor = new HDEventMonitor(true))
                {
                    monitor.UpdateConfiguration(m_ejd);
                    monitor.Start();

                    int lCounter = 0;
                    while (lCounter < maxTimeEventQuery)
                    {
                        evts = monitor.GetNewEvents();
                        if (evts != null && evts.Count > 0)
                            break;

                        Thread.Sleep(1000);
                        if (m_status == -1)
                        {
                            bCancelled = true;
                            return;
                        }
                        lCounter++;
                    }
                }

                if (m_status == -1)
                {
                    bCancelled = true;
                    return;
                }

                //Determine start/stop time
                DateTime dtNow = DateTime.UtcNow;
                DateTime startTime = m_ejd.EnablePreTriggerRange ? dtNow.Subtract(m_ejd.PreTriggerRange) : dtNow;
                DateTime dtPost = dtNow.Add(m_ejd.EnablePostTriggerRange ? m_ejd.PostTriggerRange : TimeSpan.Zero);
                DateTime dtMax = startTime.Add(m_ejd.MaxTriggerRange);
                DateTime stopTime = m_ejd.RangeCenter == EventJobRangeCenter.Both && dtMax < dtPost ? dtMax : dtPost;
                if (evts == null || evts.Count <= 0)
                {
                    StatusChanged?.Invoke(new EventJobTestStatus(Level.Warning, Properties.Resources.EventJob_TestFile_NoEventFound, false));
                    Thread.Sleep(3000);
                }
                else
                {
                    startTime = evts[evts.Count - 1].StartTime;
                    stopTime = evts[evts.Count - 1].StopTime;
                }

                if (m_status == -1)
                {
                    bCancelled = true;
                    return;
                }

                //Generate file
                StatusChanged?.Invoke(new EventJobTestStatus(Level.Info, Properties.Resources.EventJob_TestFile_GeneratingFile, false));

                GeneratedFiles = new List<string>();
                if (m_ejd.HDStores.Length > 1)
                {
                    foreach (var store in lStores)
                    {
                        string lFileName = $"{m_fileName.Substring(0, m_fileName.Length - 4)}_{store}.hdq";
                        m_ejd.HDStores = new string[1] { store };
                        m_confData.GenerateHDQFile(startTime, stopTime, lFileName);
                        GeneratedFiles.Add(lFileName);
                    }
                }
                else
                {
                    m_confData.GenerateHDQFile(startTime, stopTime, m_fileName);
                    GeneratedFiles.Add(m_fileName);
                }
            }
            catch (Exception ex)
            {
                errorMsg = ex.Message;
                return;
            }
            finally
            {
                m_ejd.HDStores = lStores;

                if (!string.IsNullOrWhiteSpace(errorMsg))
                    StatusChanged?.Invoke(new EventJobTestStatus(Level.Exception, string.Format(Properties.Resources.EventJob_TestFile_Error, errorMsg), true));
                else if (bCancelled)
                    StatusChanged?.Invoke(new EventJobTestStatus(Level.Info, Properties.Resources.EventJob_TestFile_Canceled, true));
                else
                    StatusChanged?.Invoke(new EventJobTestStatus(Level.Info, Properties.Resources.EventJob_TestFile_Finished, true));

                Interlocked.Exchange(ref m_status, 0);
            }
        }
        #endregion
    }
}
