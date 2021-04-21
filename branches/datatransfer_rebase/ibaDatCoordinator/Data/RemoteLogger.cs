using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using iba.Logging;

namespace iba.Data
{
    [Serializable]
    [System.Reflection.Obfuscation]
    public class DatCoEventData
    {
        public DatCoEventData()
        {

        }

        public DatCoEventData(Event ev)
        {
            Level = (int)ev.Level.Priority;
            Time = ev.Timestamp;
            Message = ev.Message;
            Data = ev.Data as LogExtraData;
        }

        public int Level;
        public DateTime Time;
        public string Message;
        public LogExtraData Data;
    }

    [System.Reflection.Obfuscation]
    public interface IEventForwarder
    {
        void Forward(DatCoEventData[] events);
    }

    public class RemoteLogger : Logger
    {
        public RemoteLogger()
        {
            m_efDict = new ConcurrentDictionary<Guid, EventDispatcher>();
            m_cache = new Utility.CircularBuffer<Data.DatCoEventData>(100, true);
        }

        private ConcurrentDictionary<Guid, EventDispatcher> m_efDict;
        private iba.Utility.CircularBuffer<DatCoEventData> m_cache;

        public void AddForwarder(IEventForwarder ev, Guid g, string clientName)
        {
            //Create dispatcher for the client
            EventDispatcher ed = new EventDispatcher(ev, clientName);
            m_efDict.TryAdd(g, ed);

            //Copy all cached events
            DatCoEventData[] cachedEvents;
            lock(m_cache)
            {
                cachedEvents = m_cache.ToArray();
            }

            //Send all cached events to the new dispatcher
            ed.Write(cachedEvents);

            ibaLogger.DebugFormat("Added event forwarder for {0} (GUID={1})", clientName, g);
        }

        public void RemoveForwarder(Guid g)
        {
            EventDispatcher ed;
            m_efDict.TryRemove(g, out ed);
            if (ed != null)
            {
                ed.Dispose();
                ibaLogger.DebugFormat("Removed event forwarder for {0} (GUID={1})", ed.ClientName, g);
            }
        }

        protected override void ReleaseResources()
        {
            foreach (var kvp in m_efDict)
                kvp.Value.Dispose();

            m_efDict.Clear();
        }

        protected override void Write(Event _event)
        {
            Write(new Event[] { _event }, 1);
        }

        protected override void Write(Event[] events, int length)
        {
            DatCoEventData[] datCoEvents = new DatCoEventData[length];
            for (int i = 0; i < length; i++)
                datCoEvents[i] = new DatCoEventData(events[i]);

            lock(m_cache)
            {
                m_cache.Put(datCoEvents);
            }

            List<Guid> guidsToDelete = new List<Guid>();
            foreach (var kvp in m_efDict)
            {
                if (!kvp.Value.Write(datCoEvents))
                    guidsToDelete.Add(kvp.Key);
            }

            EventDispatcher ed;
            foreach (Guid g in guidsToDelete)
            {
                if(m_efDict.TryRemove(g, out ed))
                {
                    ed.Dispose();
                    ibaLogger.DebugFormat("Removed event forwarder for {0} (GUID={1}) because of error", ed.ClientName, g);
                }

            }
        }
    }

    class EventDispatcher : IDisposable
    {
        public EventDispatcher(IEventForwarder ef, string clientName)
        {
            this.ef = ef;
            ClientName = clientName;

            hi = Belikov.GenuineChannels.GenuineUtility.CurrentSession as Belikov.GenuineChannels.TransportContext.HostInformation;

            queue = new ConcurrentQueue<DatCoEventData[]>();
            waitEv = new System.Threading.AutoResetEvent(false);
            bRun = true;
            thread = new System.Threading.Thread(new System.Threading.ThreadStart(Run));
            thread.Priority = System.Threading.ThreadPriority.BelowNormal;
            thread.Start();
        }

        public bool Write(DatCoEventData[] events)
        {
            if (!bRun)
                return false;

            if (queue.Count > 1000)
            {
                Disconnect(new Exception("{0} events in queue for client {1} -> client seems to be hanging"));
                return false;
            }

            queue.Enqueue(events);

            waitEv.Set();
            return true;
        }

        IEventForwarder ef;
        public string ClientName { get; }

        //Information about connection to event forwarder
        Belikov.GenuineChannels.TransportContext.HostInformation hi;

        ConcurrentQueue<DatCoEventData[]> queue;

        System.Threading.Thread thread;
        System.Threading.AutoResetEvent waitEv;
        bool bRun;

        void Disconnect(Exception ex)
        {
            Remoting.ServerRemotingManager.DisconnectClient(hi, ex);
        }

        public void Dispose()
        {
            if(thread != null)
            {
                bRun = false;
                waitEv.Set();

                thread.Join(10000);
                thread = null;

                waitEv.Close();
            }
        }

        void Run()
        {
            try
            {
                while (bRun)
                {
                    DatCoEventData[] events;
                    while (queue.TryDequeue(out events))
                        ef.Forward(events); //Send events to client

                    waitEv.WaitOne();
                }
            }
            catch (Exception ex)
            {
                ibaLogger.DebugFormat("Error when dispatching events to {0}: {1}", ClientName, ex);
                Disconnect(ex);
                bRun = false;
            }
        }
    }
}
