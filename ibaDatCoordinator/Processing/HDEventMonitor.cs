using iba.Data;
using iba.HD.Client;
using iba.HD.Client.Interfaces;
using iba.HD.Common;
using iba.Logging;
using System;
using System.Collections.Generic;
using System.Threading;

namespace iba.Processing
{
    class HDEventMonitor : IDisposable
    {
        #region Members
        EventJobData m_ejd;
        IHdReader m_hdReader;

        Dictionary<string, LiveStoreData> m_liveData;

        object m_dataSetLock;
        HashSet<EventReaderData> m_liveDataSet;
        const int intervalSetCleanup = 10 * 60 * 1000;
        Timer m_tmrSetCleanup;

        long m_startTimeTicks;
        object m_queueLock;
        List<EventReaderData> m_eventQueue;

        const int intervalAdvance = 1000;
        Timer m_tmrAdvance;
        #endregion

        #region Initialize
        public HDEventMonitor()
        {
            m_ejd = null;
            m_hdReader = HdClient.CreateReader(HdUserType.Analyzer);
            m_hdReader.ShowConnectionError = false;
            m_hdReader.ConnectionChanged += OnHdConnectionChanged;
            m_hdReader.Advance += OnHdAdvance;
            m_hdReader.SignalsUpdated += OnHdSignalsUpdated;

            m_liveData = new Dictionary<string, LiveStoreData>();

            m_dataSetLock = new object();
            m_liveDataSet = new HashSet<EventReaderData>();
            m_tmrSetCleanup = new Timer(OnCleanupTimerTick);

            m_startTimeTicks = DateTime.MaxValue.Ticks;
            m_queueLock = new object();
            m_eventQueue = new List<EventReaderData>();

            m_tmrAdvance = new Timer(OnAdvanceTimerTick);
        }

        public void Start()
        {
            m_startTimeTicks = DateTime.UtcNow.Ticks;
            m_tmrAdvance?.Change(intervalAdvance, Timeout.Infinite);
            m_tmrSetCleanup?.Change(intervalSetCleanup, Timeout.Infinite);
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            Timer lTimer = m_tmrAdvance;
            m_tmrAdvance = null;
            if (lTimer != null)
            {
                lTimer.Change(Timeout.Infinite, Timeout.Infinite);
                lTimer.Dispose();
            }

            lTimer = m_tmrSetCleanup;
            m_tmrSetCleanup = null;
            if (lTimer != null)
            {
                lTimer.Change(Timeout.Infinite, Timeout.Infinite);
                lTimer.Dispose();
            }

            if (m_hdReader != null)
            {
                m_hdReader.Advance -= OnHdAdvance;
                m_hdReader.SignalsUpdated -= OnHdSignalsUpdated;

                foreach (LiveStoreData storeData in m_liveData.Values)
                {
                    if (storeData.SubsetId >= 0)
                        m_hdReader.EventManager.RemoveSubset(storeData.SubsetId);

                    if (storeData.RequestId >= 0)
                        m_hdReader.EventManager.CancelRequest(storeData.RequestId);
                }
            }

            m_liveData.Clear();
            m_liveDataSet.Clear();

            if (m_hdReader != null)
            {
                m_hdReader.Dispose();
                m_hdReader.ConnectionChanged -= OnHdConnectionChanged;
                m_hdReader = null;
            }
        }
        #endregion

        #region Configuration
        public void UpdateConfiguration(EventJobData ejd)
        {
            if (ejd == null)
                throw new Exception("HDEventMonitor: passed configuration cannot be null");

            bool bFirstTime = m_ejd == null;
            if (!bFirstTime && ejd.IsSame(m_ejd))
                return;

            bool bDifferentJobTrigger = (m_ejd?.JobTriggerEvent ?? JobTrigger.Incoming) != ejd.JobTriggerEvent;
            m_ejd = ejd.Clone() as EventJobData;

            IHdReader lHdReader = m_hdReader;
            if (lHdReader == null)
                return;

            if (bFirstTime || m_ejd.EventHDServer != lHdReader.ServerHost || m_ejd.EventHDPort != lHdReader.ServerPort)
            {
                if (lHdReader.IsConnected())
                    lHdReader.Disconnect();

                m_liveData.Clear();
                lHdReader.Connect(m_ejd.EventHDServer, m_ejd.EventHDPort);
            }

            m_startTimeTicks = DateTime.UtcNow.Ticks;
            if (UpdateLiveSubsets() || bDifferentJobTrigger)
            {
                lock(m_eventQueue)
                {
                    m_eventQueue.Clear();
                }
            }            
        }

        bool UpdateLiveSubsets()
        {
            string subId;
            string storeName;
            List<string> subIds;
            var idsPerStore = new Dictionary<string, List<string>>();

            if (m_ejd.MonitorAllEvents)
            {
                foreach (string name in m_hdReader.EventManager.GetStoreNames())
                    idsPerStore.Add(name, null);
            }
            else
            {
                foreach (string hdId in m_ejd.EventIDs)
                {
                    storeName = HdId.GetStoreName(hdId);
                    subId = HdId.GetSubId(hdId);

                    if (!idsPerStore.TryGetValue(storeName, out subIds))
                    {
                        subIds = new List<string>();
                        idsPerStore.Add(storeName, subIds);
                    }

                    subIds.Add(subId);
                }
            }

            LiveStoreData storeData;
            EventStoreSubset subset;
            var newLiveData = new Dictionary<string, LiveStoreData>();

            bool updated = m_liveData.Count != idsPerStore.Count;

            foreach (var kvp in idsPerStore)
            {
                subset = new EventStoreSubset(kvp.Key, kvp.Value);

                m_liveData.TryGetValue(kvp.Key, out storeData);

                if (storeData == null)
                {
                    updated = true;
                    storeData = new LiveStoreData(subset);
                }

                if (!storeData.Subset.Equals(subset))
                {
                    m_hdReader?.EventManager.CancelRequest(storeData.RequestId);
                    storeData.RequestId = -1;

                    m_hdReader?.EventManager.RemoveSubset(storeData.SubsetId);
                    storeData.Subset = subset;
                    storeData.SubsetId = -1;

                    updated = true;
                }

                newLiveData.Add(kvp.Key, storeData);
            }

            Dictionary<string, LiveStoreData> lLiveData = m_liveData;
            foreach (var data in lLiveData.Values)
            {
                bool bDelete = true;
                foreach (var newData in newLiveData.Values)
                {
                    if (!data.Subset.Equals(newData.Subset))
                        bDelete = true;
                }

                if (bDelete)
                {
                    m_hdReader?.EventManager.CancelRequest(data.RequestId);
                    data.RequestId = -1;

                    m_hdReader?.EventManager.RemoveSubset(data.SubsetId);
                    data.SubsetId = -1;

                    updated = true;
                }
            }

            m_liveData = newLiveData;
            return updated;
        }
        #endregion

        #region HD handlers
        void OnHdConnectionChanged()
        {
            IHdReader lHdReader = m_hdReader;
            if (lHdReader == null)
                return;

            string prefix = string.Format("HDEventMonitor HD reader {0}:{1}", lHdReader.ServerHost, lHdReader.ServerPort);
            if (lHdReader.IsConnected())
                ibaLogger.LogFormat(Level.Info, "{0} connected", prefix);
            else
            {
                string error = lHdReader.ConnectionError;
                if (!string.IsNullOrWhiteSpace(error))
                    ibaLogger.LogFormat(Level.Warning, "{0} disconnected: {1}", prefix, error);
                else
                    ibaLogger.LogFormat(Level.Info, "{0} disconnected", prefix);
            }
        }

        void OnHdAdvance(IList<string> storeNames, IList<long> stamps)
        {
            LiveStoreData storeData;

            for (int i = 0; i < storeNames.Count && i < stamps.Count; i++)
            {
                if (m_liveData.TryGetValue(storeNames[i], out storeData))
                    storeData.AdvanceTime = stamps[i];
            }
        }

        void OnHdSignalsUpdated()
        {
            Dictionary<string, LiveStoreData> lLiveData = m_liveData;
            foreach (var storeData in lLiveData.Values)
            {
                if (storeData.RequestId >= 0)
                {
                    m_hdReader?.EventManager.CancelRequest(storeData.RequestId);
                    storeData.RequestId = -1;
                }

                storeData.ReceiveTime = DateTime.MinValue.Ticks;
                storeData.AdvanceTime = DateTime.MaxValue.Ticks;
            }
        }
        #endregion

        #region Event buffer
        void Enqueue(List<EventReaderData> newEvents)
        {
            if (newEvents == null || newEvents.Count <= 0)
                return;

            lock (m_queueLock)
            {
                m_eventQueue.AddRange(newEvents);
            }
        }

        public List<EventReaderData> GetNewEvents()
        {
            List<EventReaderData> res = new List<EventReaderData>();

            if (m_eventQueue.Count <= 0)
                return res;

            lock (m_queueLock)
            {
                res = m_eventQueue;
                m_eventQueue = new List<EventReaderData>();
            }

            return res;
        }
        #endregion

        #region Live data
        void OnCleanupTimerTick(object state)
        {
            long lowestReceiveTime = DateTime.MaxValue.Ticks;
            Dictionary<string, LiveStoreData> lLiveData = m_liveData;

            foreach (var storeData in lLiveData.Values)
            {
                long lReceiveTime = storeData.ReceiveTime;
                if (lReceiveTime < lowestReceiveTime)
                    lowestReceiveTime = lReceiveTime;
            }

            lock (m_dataSetLock)
            {
                m_liveDataSet.RemoveWhere((erd) => erd == null || erd.UtcTicks < lowestReceiveTime);
            }

            m_tmrSetCleanup?.Change(intervalSetCleanup, Timeout.Infinite);
        }

        void OnAdvanceTimerTick(object state)
        {
            if (m_hdReader != null && m_hdReader.IsConnected())
            {
                Dictionary<string, LiveStoreData> lLiveData = m_liveData;
                foreach (var storeData in lLiveData.Values)
                {
                    if (storeData.SubsetId == -1)
                    {
                        if (storeData.Subset.EventIds == null || storeData.Subset.EventIds.Count > 0)
                        {
                            storeData.SubsetId = -2;
                            m_hdReader?.EventManager.AddSubset(storeData.Subset, OnNewSubsetId);
                        }
                    }
                    else if (storeData.SubsetId >= 0)
                    {
                        if (storeData.RequestId == -1)
                        {
                            var range = TimeRangeUtc.Invalid;

                            if (storeData.AdvanceTime != DateTime.MaxValue.Ticks)
                            {
                                if (storeData.ReceiveTime < storeData.AdvanceTime)
                                {
                                    range = new TimeRangeUtc(storeData.ReceiveTime, storeData.AdvanceTime, true);
                                }
                                else if (storeData.ReceiveTime > storeData.AdvanceTime)
                                {
                                    storeData.ReceiveTime = storeData.AdvanceTime;
                                }
                            }

                            if (range.IsValid())
                            {
                                storeData.RequestId = m_hdReader != null ? m_hdReader.EventManager.AddLiveRequest(storeData.SubsetId, range) : -1;

                                m_hdReader?.EventManager.SendRequest(storeData.RequestId, Response);
                            }
                        }
                    }
                }
            }

            m_tmrAdvance?.Change(intervalAdvance, Timeout.Infinite);
        }

        void OnNewSubsetId(string storeName, int newSubsetId)
        {
            LiveStoreData storeData;

            if (m_liveData.TryGetValue(storeName, out storeData))
                storeData.SubsetId = newSubsetId;
        }

        void Response(EventResponse response)
        {
            if (response == null)
                return;

            List<EventReaderData> newEvents = new List<EventReaderData>();
            Dictionary<string, LiveStoreData> lLiveData = m_liveData;
            foreach (var storeData in lLiveData.Values)
            {
                if (storeData.RequestId == response.RequestId)
                {
                    if (response.Events != null)
                    {
                        long receiveTime = storeData.ReceiveTime;

                        lock (m_dataSetLock)
                        {
                            foreach (var data in response.Events)
                            {
                                if (data.UtcTicks < m_startTimeTicks)
                                    continue;

                                switch (m_ejd.JobTriggerEvent)
                                {
                                    case JobTrigger.Incoming:
                                        if (!data.TriggerIn)
                                            continue;
                                        break;
                                    case JobTrigger.Outgoing:
                                        if (!data.TriggerOut)
                                            continue;
                                        break;
                                }

                                if (m_liveDataSet.Add(data))
                                {
                                    newEvents.Add(data);
                                    receiveTime = Math.Max(receiveTime, data.UtcTicks);
                                }
                            }
                        }

                        storeData.ReceiveTime = receiveTime;
                    }

                    if (response.Final)
                    {
                        if (response.Final && storeData.RequestId == response.RequestId)
                            storeData.RequestId = -1;
                    }
                }
            }

            if (!string.IsNullOrEmpty(response.Error))
                ibaLogger.Log(Level.Warning, response.Error);

            Enqueue(newEvents);
        }
        #endregion

        #region LiveStoreData
        class LiveStoreData
        {
            public int RequestId, SubsetId;
            public EventStoreSubset Subset;
            public long ReceiveTime, AdvanceTime;

            public LiveStoreData(EventStoreSubset subset)
            {
                RequestId = -1;

                SubsetId = -1;
                Subset = subset;

                ReceiveTime = DateTime.MinValue.Ticks;
                AdvanceTime = DateTime.MaxValue.Ticks;
            }
        }
        #endregion
    }
}
