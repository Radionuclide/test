using iba.Data;
using iba.HD.Client;
using iba.HD.Client.Interfaces;
using iba.HD.Common;
using iba.Logging;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace iba.Processing
{
    class HDEventMonitor : IDisposable
    {
        #region Members
        EventJobData m_ejd;
        string m_jobName;
        IHdReader m_hdReader;
        bool m_bSkipChecks;

        int errorCode;
        Dictionary<string, HdStoreData> m_liveData;
        Dictionary<string, HdStoreData> m_queryData;

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
        List<EventOccurrence> m_eventQueue;

        const int intervalAdvance = 1000;
        Timer m_tmrAdvance;
        #endregion

        #region Initialize
        public HDEventMonitor(bool bSkipChecks = false)
        {
            m_bSkipChecks = bSkipChecks;
            m_ejd = null;
            m_jobName = string.Empty;
            m_hdReader = HdClient.CreateReader(HdUserType.PdaClient);
            object obj = m_hdReader.Authenticate(null);
            obj = HdReaderAuthenticator.GetInfo(obj);
            obj = m_hdReader.Authenticate(obj);
            m_hdReader.UserLoginOptions = HdUserLoginOptions.Never;
            m_hdReader.ShowConnectionError = false;
            m_hdReader.ConnectionChanged += OnHdConnectionChanged;
            m_hdReader.Advance += OnHdAdvance;

            m_liveData = new Dictionary<string, HdStoreData>();
            m_queryData = new Dictionary<string, HdStoreData>();

            m_dataSetLock = new object();
            m_liveDataSet = new HashSet<EventReaderData>();
            m_tmrSetCleanup = new Timer(OnCleanupTimerTick);

            m_startTimeTicks = DateTime.MaxValue.Ticks;
            m_queueLock = new object();
            m_eventQueue = new List<EventOccurrence>();

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
            Task.Factory.StartNew(() => { QueryHistoricalEvents(); });
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

                foreach (HdStoreData storeData in m_liveData.Values)
                {
                    if (storeData.SubsetId >= 0)
                        m_hdReader.EventManager.RemoveSubset(storeData.SubsetId);

                    if (storeData.RequestId >= 0)
                        m_hdReader.EventManager.CancelRequest(storeData.RequestId);
                }

                foreach (HdStoreData storeData in m_queryData.Values)
                {
                    if (storeData.RequestId >= 0)
                        m_hdReader.EventManager.CancelRequest(storeData.RequestId);
                }
            }

            m_liveData.Clear();
            m_queryData.Clear();
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
        public void UpdateConfiguration(EventJobData ejd, string jobName)
        {
            if (ejd == null)
                throw new Exception("HDEventMonitor: passed configuration cannot be null");

            m_jobName = jobName ?? string.Empty;

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
            ejd.HdQueryTimeSpanChanged = false;
            IHdReader lHdReader = m_hdReader;
            if (lHdReader == null)
                return;

            if (bFirstTime || m_ejd.HDServer != lHdReader.ServerHost || m_ejd.HDPort != lHdReader.ServerPort || !lHdReader.UserLoginInfo.UserName.Equals(m_ejd.HDUsername, StringComparison.CurrentCultureIgnoreCase) || lHdReader.UserLoginInfo.Password != m_ejd.HDPassword)
            {
                if (lHdReader.IsConnected())
                    lHdReader.Disconnect();

                lHdReader.UserLoginInfo.UserName = m_ejd.HDUsername;
                lHdReader.UserLoginInfo.Password = m_ejd.HDPassword;
                lHdReader.UserLoginInfo.UseCurrentWindowsUser = false;

                m_liveData.Clear();
                m_queryData.Clear();
                lHdReader.Connect(m_ejd.HDServer, m_ejd.HDPort);

                if (!lHdReader.IsConnected())
                    ibaLogger.Log(Level.Warning, $"HDEventMonitor HD reader {lHdReader.ServerHost}:{lHdReader.ServerPort} could not connect: {lHdReader.ConnectionError ?? ""}");
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

            HdStoreData liveStoreData;
            HdStoreData queryStoreData;

            EventStoreSubset subset;
            var newLiveData = new Dictionary<string, HdStoreData>();
            var newQueryData = new Dictionary<string, HdStoreData>();

            bool updated = m_liveData.Count != idsPerStore.Count;

            foreach (var kvp in idsPerStore)
            {
                subset = new EventStoreSubset(kvp.Key, kvp.Value);

                m_liveData.TryGetValue(kvp.Key, out liveStoreData);

                if (liveStoreData == null)
                {
                    updated = true;
                    liveStoreData = new HdStoreData(subset);
                }

                if (!liveStoreData.Subset.Equals(subset))
                {
                    m_hdReader?.EventManager.CancelRequest(liveStoreData.RequestId);
                    liveStoreData.RequestId = -1;

                    m_hdReader?.EventManager.RemoveSubset(liveStoreData.SubsetId);
                    liveStoreData.Subset = subset;
                    liveStoreData.SubsetId = -1;

                    updated = true;
                }

                newLiveData.Add(kvp.Key, liveStoreData);

                if (!m_queryData.TryGetValue(kvp.Key, out queryStoreData))
                {
                    updated = true;
                    queryStoreData = new HdStoreData(subset);

                    long endTime = 0;
                    if (m_ejd.LastReceivedHistoricalTimeStamp?.TryGetValue(subset.StoreName, out endTime) ?? false)
                    {
                        queryStoreData.ReceiveTime = endTime - 1; // Subtract one tick to have the last received event double
                    }
                    else
                    {
                        queryStoreData.ReceiveTime = m_ejd.StopTimeHdQueryTicksUTC;
                    }
                }

                if (!queryStoreData.Subset.Equals(subset))
                {
                    m_hdReader?.EventManager.CancelRequest(queryStoreData.RequestId);
                    queryStoreData.RequestId = -1;

                    updated = true;
                }

                newQueryData.Add(kvp.Key, queryStoreData);
            }

            Dictionary<string, HdStoreData> lLiveData = m_liveData;
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

            Dictionary<string, HdStoreData> lqueryData = m_queryData;
            foreach (var data in lqueryData.Values)
            {
                bool bDelete = true;
                foreach (var newData in newQueryData.Values)
                {
                    if (!data.Subset.Equals(newData.Subset))
                        bDelete = true;
                }

                if (bDelete)
                {
                    m_hdReader?.EventManager.CancelRequest(data.RequestId);
                    data.RequestId = -1;

                    updated = true;
                }
            }

            m_liveData = newLiveData;
            m_queryData = newQueryData;
            return updated;
        }

        public Dictionary<string, long> GetLastReceivedHistoricalTimeStamps()
        {
            Dictionary<string, long> stoptimes = new Dictionary<string, long>();
            foreach (var storeData in m_queryData.Values)
            {
                if (storeData.RequestId != -1)
                {
                    long lReceiveTime = storeData.ReceiveTime;
                    stoptimes.Add(storeData.Subset.StoreName, storeData.ReceiveTime);
                }
            }
            return stoptimes;
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
                //Update start time to prevent job executions for old event occurrences
                //Lowest receive time is enough (rest is handled by the hashset)
                long lowestReceiveTime = DateTime.MaxValue.Ticks;
                Dictionary<string, HdStoreData> lLiveData = m_liveData;

                foreach (var storeData in lLiveData.Values)
                {
                    long lReceiveTime = storeData.ReceiveTime;
                    if (lReceiveTime < lowestReceiveTime)
                        lowestReceiveTime = lReceiveTime;
                }

                if (m_startTimeTicks < lowestReceiveTime && lowestReceiveTime != DateTime.MaxValue.Ticks)
                    m_startTimeTicks = lowestReceiveTime;

                m_liveData.Clear();
                UpdateLiveSubsets();

                string error = lHdReader.ConnectionError;
                if (!string.IsNullOrWhiteSpace(error))
                    ibaLogger.LogFormat(Level.Warning, "{0} disconnected: {1}", prefix, error);
                else
                    ibaLogger.LogFormat(Level.Info, "{0} disconnected", prefix);
            }
        }

        void OnHdAdvance(IList<string> storeNames, IList<long> stamps)
        {
            HdStoreData storeData;

            for (int i = 0; i < storeNames.Count && i < stamps.Count; i++)
            {
                if (m_liveData.TryGetValue(storeNames[i], out storeData))
                    storeData.AdvanceTime = stamps[i];
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
                    m_eventQueue.Add(new EventOccurrence(matchedEvent.Name, matchedEvent.StartTime.ToLocalTime(), matchedEvent.StopTime.ToLocalTime()));
            }
        }

        public List<EventOccurrence> GetNewEvents()
        {
            List<EventOccurrence> res = new List<EventOccurrence>();

            if (m_eventQueue.Count <= 0)
                return res;

            lock (m_queueLock)
            {
                res = m_eventQueue;
                m_eventQueue = new List<EventOccurrence>();
            }

            return res;
        }

        internal class EventOccurrence
        {
            public string Name { get; private set; }
            public DateTime StartTime { get; private set; }
            public DateTime StopTime { get; private set; }

            public EventOccurrence(string name, DateTime start, DateTime stop)
            {
                Name = name;
                StartTime = start;
                StopTime = stop;
            }
        }

        #endregion

        #region Live data
        void OnCleanupTimerTick(object state)
        {
            long lowestReceiveTime = DateTime.MaxValue.Ticks;
            Dictionary<string, HdStoreData> lLiveData = m_liveData;

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
                            if (m_ejd.EnableMaxTriggerRange || !(evt is MatchedEventDataRange mEvt && !mEvt.IsMatched()))
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
                Dictionary<string, HdStoreData> lLiveData = m_liveData;
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

                                m_hdReader?.EventManager.SendRequest(storeData.RequestId, LiveResponse);
                            }
                        }
                    }
                }
            }

            m_tmrAdvance?.Change(intervalAdvance, Timeout.Infinite);
        }

        void OnNewSubsetId(string storeName, int newSubsetId)
        {
            HdStoreData storeData;

            if (m_liveData.TryGetValue(storeName, out storeData))
                storeData.SubsetId = newSubsetId;
        }

        string CreateEventID(string store, string subId)
        {
            return $"store:{store};event:{subId};".ToUpper();
        }

        void Response(EventResponse response, Dictionary<string, HdStoreData> lStoreData, long startTimeTicks, bool resetReceiveTime)
        {
            if (response == null)
                return;

            if (response.ErrorCode != errorCode)
            {
                errorCode = response.ErrorCode;
                if (errorCode != 0)
                {
                    ibaLogger.LogFormat(Level.Warning, "Event job <<{0}>>: {1} (error code: {2})", m_jobName, response.Error, response.ErrorCode);
                    return;
                }
            }
            else if (errorCode == 0 && !string.IsNullOrWhiteSpace(response.Error))
            {
                ibaLogger.LogFormat(Level.Warning, "Event job <<{0}>>: {1}", m_jobName, response.Error);
                return;
            }

            List<EventReaderData> newEvents = new List<EventReaderData>();
            foreach (var storeData in lStoreData.Values)
            {
                if (storeData.RequestId == response.RequestId)
                {
                    if (response.Events != null)
                    {
                        long receiveTime = storeData.ReceiveTime;

                        // Incoming events should be processed before outgoing
                        response.Events.Sort((a, b) =>
                        {
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

                                    if (!m_bSkipChecks && data.UtcTicks < startTimeTicks)
                                        continue;

                                    if (m_liveDataSet.Add(data))
                                    {
                                        string id = CreateEventID(data.Store, data.Id);
                                        List<EventDataRange> lMatchedEvents = null;
                                        if (!m_dictMatchedEvents.TryGetValue(id, out lMatchedEvents))
                                        {
                                            lMatchedEvents = new List<EventDataRange>();
                                            m_dictMatchedEvents[id] = lMatchedEvents;
                                        }

                                        if (m_ejd.RangeCenter == EventJobRangeCenter.Both)
                                        {
                                            if (data.TriggerIn)
                                                lMatchedEvents.Add(new MatchedEventDataRange(data.Name, data.UtcTicks, m_ejd.EnablePreTriggerRange ? m_ejd.PreTriggerRange : TimeSpan.Zero, m_ejd.EnablePostTriggerRange ? m_ejd.PostTriggerRange : TimeSpan.Zero, m_ejd.MaxTriggerRange));
                                            else if (data.TriggerOut && lMatchedEvents.Count > 0)
                                            {
                                                int matchIndex = lMatchedEvents.Count - 1;

                                                // If the event duration is set, use this to get a more robust match.
                                                // Without duration the matching will be wrong in case of overlapping incoming/outgoing pairs
                                                if (data.Duration != 0)
                                                {
                                                    for (int i = lMatchedEvents.Count - 1; i >= 0; i--)
                                                    {
                                                        if (lMatchedEvents[i].StartTime.Ticks == data.UtcTicks - data.Duration)
                                                        {
                                                            matchIndex = i;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (lMatchedEvents[matchIndex].StartTime.Ticks < data.UtcTicks)
                                                    (lMatchedEvents[matchIndex] as MatchedEventDataRange)?.Match(data.UtcTicks);
                                            }
                                        }
                                        else //incorrect events are already filtered out
                                            lMatchedEvents.Add(new SingleEventDataRange(data.Name, data.UtcTicks, m_ejd.EnablePreTriggerRange ? m_ejd.PreTriggerRange : TimeSpan.Zero, m_ejd.EnablePostTriggerRange ? m_ejd.PostTriggerRange : TimeSpan.Zero));

                                        receiveTime = Math.Max(receiveTime, data.UtcTicks);
                                    }
                                }
                            }
                        }

                        storeData.ReceiveTime = receiveTime;
                    }

                    if (response.Final)
                    {
                        if (storeData.RequestId == response.RequestId)
                        {
                            storeData.RequestId = -1;

                            if (resetReceiveTime)
                                storeData.ReceiveTime = DateTime.MinValue.Ticks;
                        }
                    }
                }
            }
        }

        void LiveResponse(EventResponse response)
        {
            Response(response, m_liveData, m_startTimeTicks, false);
        }
        #endregion

        #region LiveStoreData
        class HdStoreData
        {
            public int RequestId, SubsetId;
            public EventStoreSubset Subset;
            public long ReceiveTime, AdvanceTime;

            public HdStoreData(EventStoreSubset subset)
            {
                RequestId = -1;

                SubsetId = -1;
                Subset = subset;

                ReceiveTime = DateTime.MinValue.Ticks;
                AdvanceTime = DateTime.MaxValue.Ticks;
            }
        }
        #endregion

        #region Query data

        void QueryHistoricalEvents()
        {
            if (m_hdReader != null && m_hdReader.IsConnected())
            {
                Dictionary<string, HdStoreData> lQueryData = m_queryData;
                foreach (var storeData in lQueryData.Values)
                {
                    if (storeData.RequestId == -1)
                    {
                        TimeRangeUtc range;
                        if (m_ejd.LastReceivedHistoricalTimeStamp != null && !m_ejd.HdQueryTimeSpanChanged && m_ejd.LastReceivedHistoricalTimeStamp.TryGetValue(storeData.Subset.StoreName, out long endTime))
                            range = new TimeRangeUtc(m_ejd.StartTimeHdQueryTicksUTC, endTime, true);
                        else if (m_ejd.HdQueryTimeSpanChanged)
                            range = new TimeRangeUtc(m_ejd.StartTimeHdQueryTicksUTC, m_ejd.HdQueryUseEndTime ? m_ejd.StopTimeHdQueryTicksUTC : DateTime.Now.Ticks, true);
                        else
                            continue;

                        if (range.IsValid())
                        {
                            MultiEventQuery request = GenerateHdReaderRequest(range, storeData.Subset.EventIds, storeData.Subset.StoreName);
                            storeData.RequestId = m_hdReader != null ? m_hdReader.EventManager.AddQueryRequest(request, Int32.MaxValue) : -1;

                            m_hdReader?.EventManager.SendRequest(storeData.RequestId, QueryResponse);
                        }
                    }
                }

            }
        }

        private MultiEventQuery GenerateHdReaderRequest(TimeRangeUtc timeRange, List<string> channelIds, string storeName)
        {
            var multiEventQuery = new MultiEventQuery(String.Empty, String.Empty)
            {
                Range = new QueryRange(true, false, timeRange, QueryRange.RangeUnit.Days, timeRange.Span)
            };

            multiEventQuery.Queries.AddRange(channelIds.Select(y => new EventQuery
            { Event = new EventQuery.EventExpression { Id = y, StoreName = storeName } }));


            return multiEventQuery;
        }

        void QueryResponse(EventResponse response)
        {
            Response(response, m_queryData, m_ejd.StartTimeHdQueryTicksUTC, true);
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

            public string Name { get; private set; }

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
            internal EventDataRange(string name, long utcTicksIncoming, TimeSpan preRange, TimeSpan postRange, TimeSpan maxRange)
            {
                Name = name;
                dtIncoming = new DateTime(utcTicksIncoming, DateTimeKind.Utc);
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

            internal SingleEventDataRange(string name, long utcTicksIncoming, TimeSpan preRange, TimeSpan postRange)
                : base(name, utcTicksIncoming, preRange, postRange, preRange + postRange)
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

            internal MatchedEventDataRange(string name, long utcTicksIncoming, TimeSpan preRange, TimeSpan postRange, TimeSpan maxRange)
                : base(name, utcTicksIncoming, preRange, postRange, maxRange)
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

                dtOutgoing = new DateTime(utcTicksOutgoing, DateTimeKind.Utc);
                bMatched = true;
            }

            public bool IsMatched()
            {
                return bMatched;
            }
            #endregion
        }
        #endregion
    }
}
