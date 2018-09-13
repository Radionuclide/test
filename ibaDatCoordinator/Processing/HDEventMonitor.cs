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
        bool m_bSkipChecks;

        Dictionary<string, LiveStoreData> m_liveData;

        object m_dataSetLock;
        HashSet<EventReaderData> m_liveDataSet;
        const int intervalSetCleanup = 10 * 60 * 1000;
        Timer m_tmrSetCleanup;

        object m_matchedEventsLock;
        Dictionary<string, List<EventDataRange>> m_dictMatchedEvents;
        const int intervalMatchChecker = 1000;
        Timer m_tmrMatchChecker;

        long m_startTimeTicks;
        object m_queueLock;
        List<Tuple<DateTime, DateTime>> m_eventQueue;

        const int intervalAdvance = 1000;
        Timer m_tmrAdvance;
        #endregion

        #region Initialize
        public HDEventMonitor(bool bSkipChecks = false)
        {
            m_bSkipChecks = bSkipChecks;
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
            m_eventQueue = new List<Tuple<DateTime, DateTime>>();

            m_tmrAdvance = new Timer(OnAdvanceTimerTick);

            m_dictMatchedEvents = new Dictionary<string, List<EventDataRange>>();
            m_matchedEventsLock = new object();
            m_tmrMatchChecker = new Timer(OnMatchCheckerTick);
        }

        public void Start()
        {
            m_startTimeTicks = DateTime.UtcNow.Ticks;
            m_tmrAdvance?.Change(intervalAdvance, Timeout.Infinite);
            m_tmrMatchChecker?.Change(intervalMatchChecker, Timeout.Infinite);
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

            lTimer = m_tmrMatchChecker;
            m_tmrMatchChecker = null;
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

            bool bRangesChanged = m_ejd == null ||
                                m_ejd.RangeCenter != ejd.RangeCenter ||
                                m_ejd.EnablePreTriggerRange != ejd.EnablePreTriggerRange ||
                                m_ejd.EnablePostTriggerRange != ejd.EnablePostTriggerRange ||
                                m_ejd.PreTriggerRange != ejd.PreTriggerRange ||
                                m_ejd.PostTriggerRange != ejd.PostTriggerRange ||
                                m_ejd.MaxTriggerRange != ejd.MaxTriggerRange;

            m_ejd = ejd.Clone() as EventJobData;

            IHdReader lHdReader = m_hdReader;
            if (lHdReader == null)
                return;

            if (bFirstTime || m_ejd.HDServer != lHdReader.ServerHost || m_ejd.HDPort != lHdReader.ServerPort)
            {
                if (lHdReader.IsConnected())
                    lHdReader.Disconnect();

                m_liveData.Clear();
                lHdReader.Connect(m_ejd.HDServer, m_ejd.HDPort);
            }

            m_startTimeTicks = DateTime.UtcNow.Ticks;
            if (UpdateLiveSubsets() || bRangesChanged)
            {
                lock (m_matchedEventsLock)
                {
                    m_dictMatchedEvents.Clear();
                }
                lock (m_queueLock)
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
        void Enqueue(List<EventDataRange> matchedEvents)
        {
            if (matchedEvents == null || matchedEvents.Count <= 0)
                return;

            lock (m_queueLock)
            {
                foreach (var matchedEvent in matchedEvents)
                    m_eventQueue.Add(Tuple.Create(matchedEvent.StartTime, matchedEvent.StopTime));
            }
        }

        public List<Tuple<DateTime,DateTime>> GetNewEvents()
        {
            List<Tuple<DateTime, DateTime>> res = new List<Tuple<DateTime, DateTime>>();

            if (m_eventQueue.Count <= 0)
                return res;

            lock (m_queueLock)
            {
                res = m_eventQueue;
                m_eventQueue = new List<Tuple<DateTime, DateTime>>();
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

        void OnMatchCheckerTick(object state)
        {
            List<EventDataRange> matchedEvents = new List<EventDataRange>();

            lock (m_matchedEventsLock)
            {
                foreach (var kvp in m_dictMatchedEvents)
                {
                    List<EventDataRange> lValues = new List<EventDataRange>(kvp.Value);
                    foreach (var evt in lValues)
                    {
                        if (m_bSkipChecks || evt.CanBeProcessed)
                        {
                            matchedEvents.Add(evt);
                            kvp.Value.Remove(evt);
                        }
                    }
                }
            }

            Enqueue(matchedEvents);

            m_tmrMatchChecker?.Change(intervalMatchChecker, Timeout.Infinite);
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

                        // Incoming events should be processed before outgoing
                        response.Events.Sort((a, b) => {
                            if (b.UtcTicks == a.UtcTicks)
                            {
                                if (b.TriggerIn && a.TriggerOut)
                                    return 1;
                                if (b.TriggerOut && a.TriggerIn)
                                    return -1;

                                return 0;
                            }

                            return a.UtcTicks.CompareTo(b.UtcTicks);
                        });

                        lock (m_dataSetLock)
                        {
                            lock (m_matchedEventsLock)
                            {
                                foreach (var data in response.Events)
                                {
                                    if (m_ejd.RangeCenter == EventJobRangeCenter.Incoming && !data.TriggerIn)
                                        continue;

                                    if (m_ejd.RangeCenter == EventJobRangeCenter.Outgoing && !data.TriggerOut)
                                        continue;

                                    if (!m_bSkipChecks && data.UtcTicks < m_startTimeTicks)
                                        continue;

                                    if (m_liveDataSet.Add(data))
                                    {
                                        string id = $"store:{data.Store};event:{data.Id};";
                                        List<EventDataRange> lMatchedEvents = null;
                                        if (!m_dictMatchedEvents.TryGetValue(id, out lMatchedEvents))
                                        {
                                            lMatchedEvents = new List<EventDataRange>();
                                            m_dictMatchedEvents[id] = lMatchedEvents;
                                        }

                                        if (m_ejd.RangeCenter == EventJobRangeCenter.Both)
                                        {
                                            if (data.TriggerIn)
                                                lMatchedEvents.Add(new MatchedEventDataRange(data.UtcTicks, m_ejd.EnablePreTriggerRange ? m_ejd.PreTriggerRange : TimeSpan.Zero, m_ejd.EnablePostTriggerRange ? m_ejd.PostTriggerRange : TimeSpan.Zero, m_ejd.MaxTriggerRange));
                                            else if (data.TriggerOut && lMatchedEvents.Count > 0)
                                                (lMatchedEvents[lMatchedEvents.Count - 1] as MatchedEventDataRange)?.Match(data.UtcTicks);
                                        }
                                        else //incorrect events are already filtered out
                                            lMatchedEvents.Add(new SingleEventDataRange(data.UtcTicks, m_ejd.EnablePreTriggerRange ? m_ejd.PreTriggerRange : TimeSpan.Zero, m_ejd.EnablePostTriggerRange ? m_ejd.PostTriggerRange : TimeSpan.Zero, m_ejd.MaxTriggerRange));

                                        receiveTime = Math.Max(receiveTime, data.UtcTicks);
                                    }
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

        #region EventDataRange
        abstract class EventDataRange
        {
            #region Members
            TimeSpan preRange, postRange, maxRange;
            protected DateTime dtIncoming, dtOutgoing;
            #endregion

            #region Properties
            public virtual bool CanBeProcessed { get; }

            public DateTime StartTime { get { return dtIncoming.Subtract(preRange); } }
            public DateTime StopTime
            {
                get
                {
                    DateTime dtMax = StartTime.Add(maxRange);
                    DateTime dtPost = dtOutgoing.Add(postRange);
                    return dtMax <= dtPost ? dtMax : dtPost;
                }
            }
            #endregion

            #region Initialize
            internal EventDataRange(long utcTicksIncoming, TimeSpan preRange, TimeSpan postRange, TimeSpan maxRange)
            {
                dtIncoming = new DateTime(utcTicksIncoming);
                this.preRange = preRange;
                this.postRange = postRange;
                this.maxRange = maxRange;
            }
            #endregion
        }

        class SingleEventDataRange : EventDataRange
        {
            #region Properties
            public override bool CanBeProcessed { get { return StopTime.AddSeconds(1.0) <= DateTime.UtcNow; } }
            #endregion

            internal SingleEventDataRange(long utcTicksIncoming, TimeSpan preRange, TimeSpan postRange, TimeSpan maxRange)
                : base (utcTicksIncoming, preRange, postRange, maxRange)
            {
                dtOutgoing = dtIncoming;
            }
        }

        class MatchedEventDataRange : EventDataRange
        {
            #region Members
            DateTime dtExpiration;
            bool bMatched;
            #endregion

            #region Properties
            public override bool CanBeProcessed { get { return (bMatched || dtExpiration <= DateTime.UtcNow) && StopTime.AddSeconds(1.0) <= DateTime.UtcNow; } }
            #endregion

            internal MatchedEventDataRange(long utcTicksIncoming, TimeSpan preRange, TimeSpan postRange, TimeSpan maxRange)
                : base(utcTicksIncoming, preRange, postRange, maxRange)
            {
                TimeSpan diffRange = maxRange - preRange;
                if (diffRange < TimeSpan.Zero)
                    diffRange = TimeSpan.Zero;

                dtOutgoing = dtIncoming.Add(diffRange);
                dtExpiration = dtOutgoing.AddSeconds(5.0); // 5 second reserve for delayed responses (to be safe)
            }

            #region Matching
            public void Match(long utcTicksOutgoing)
            {
                if (bMatched)
                    return;

                dtOutgoing = new DateTime(utcTicksOutgoing);
                bMatched = true;
            }
            #endregion
        }
        #endregion
    }
}
