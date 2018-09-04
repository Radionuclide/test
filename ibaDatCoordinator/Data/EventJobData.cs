using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iba.Data
{
    public enum JobTrigger
    {
        Incoming,
        Outgoing,
        Both
    }

    [Serializable]
    public class EventJobData : ICloneable, IHDQGenerator
    {
        #region ICloneable Members
        private int m_jobTrigger;
        [XmlIgnore]
        public JobTrigger JobTriggerEvent
        {
            get { return (JobTrigger)m_jobTrigger; }
            set { m_jobTrigger = (int)value; }
        }

        private string m_eventHDServer;
        public string EventHDServer
        {
            get { return m_eventHDServer; }
            set { m_eventHDServer = value; }
        }

        private int m_eventHDPort;
        public int EventHDPort
        {
            get { return m_eventHDPort; }
            set { m_eventHDPort = value; }
        }

        private bool m_monitorAllEvents;
        public bool MonitorAllEvents
        {
            get { return m_monitorAllEvents; }
            set { m_monitorAllEvents = value; }
        }

        private List<string> m_eventIDs;
        public List<string> EventIDs
        {
            get { return m_eventIDs; }
            set { m_eventIDs = value; }
        }

        //HD parameters (two ranges, sample rate and hd connection params)
        private string m_HDServer;
        public string HDServer
        {
            get { return m_HDServer; }
            set { m_HDServer = value; }
        }

        private int m_HDPort;
        public int HDPort
        {
            get { return m_HDPort; }
            set { m_HDPort = value; }
        }

        private string[] m_HDStores;
        public string[] HDStores
        {
            get { return m_HDStores; }
            set
            {
                if ((value == null) || (value.Length == 0))
                    m_HDStores = new string[] { "" };
                else
                    m_HDStores = value;
            }
        }

        //triggering events
        public EventJobData()
        {
            m_jobTrigger = (int)JobTrigger.Incoming;
            m_eventHDServer = "localhost";
            m_eventHDPort = 9180;
            m_monitorAllEvents = false;
            m_eventIDs = new List<string>();

            m_HDPort = 9180;
            m_HDStores = new string[] { "" };
            m_HDServer = "";
            m_startRangeFromTrigger = TimeSpan.FromHours(1);
            m_stopRangeFromTrigger = TimeSpan.Zero;
            m_preferredTimeBaseTicks = 0;
            m_bPreferredTimeBaseIsAuto = true;
        }

        private TimeSpan m_startRangeFromTrigger;
        [XmlIgnore]
        public System.TimeSpan StartRangeFromTrigger
        {
            get { return m_startRangeFromTrigger; }
            set { m_startRangeFromTrigger = value; }
        }
        public long StartRangeFromTriggerTicks
        {
            get { return m_startRangeFromTrigger.Ticks; }
            set { m_startRangeFromTrigger = TimeSpan.FromTicks(value); }
        }
        private TimeSpan m_stopRangeFromTrigger;
        [XmlIgnore]
        public System.TimeSpan StopRangeFromTrigger
        {
            get { return m_stopRangeFromTrigger; }
            set { m_stopRangeFromTrigger = value; }
        }
        public long StopRangeFromTriggerTicks
        {
            get { return m_stopRangeFromTrigger.Ticks; }
            set { m_stopRangeFromTrigger = TimeSpan.FromTicks(value); }
        }

        public bool UsePreviousTriggerAsStart { get { return false; } }

        private long m_preferredTimeBaseTicks; //in ticks
        public long PreferredTimeBaseTicks
        {
            get { return m_preferredTimeBaseTicks; }
            set { m_preferredTimeBaseTicks = value; }
        }

        [XmlIgnore]
        public System.TimeSpan PreferredTimeBase
        {
            get { return TimeSpan.FromTicks(m_preferredTimeBaseTicks); }
            set { m_preferredTimeBaseTicks = value.Ticks; }
        }

        private bool m_bPreferredTimeBaseIsAuto;
        public bool PreferredTimeBaseIsAuto
        {
            get { return m_bPreferredTimeBaseIsAuto; }
            set { m_bPreferredTimeBaseIsAuto = value; }
        }

        public object Clone()
        {
            EventJobData nejd = new EventJobData();
            nejd.m_jobTrigger = m_jobTrigger;
            nejd.EventHDServer = m_eventHDServer;
            nejd.EventHDPort = m_eventHDPort;
            nejd.MonitorAllEvents = m_monitorAllEvents;
            nejd.EventIDs = new List<string>(m_eventIDs);
            nejd.HDServer = m_HDServer;
            nejd.m_HDStores = (string[])m_HDStores.Clone();
            nejd.m_HDPort = m_HDPort;
            nejd.m_startRangeFromTrigger = m_startRangeFromTrigger;
            nejd.m_stopRangeFromTrigger = m_stopRangeFromTrigger;
            nejd.m_preferredTimeBaseTicks = m_preferredTimeBaseTicks;
            nejd.m_bPreferredTimeBaseIsAuto = m_bPreferredTimeBaseIsAuto;
            return nejd;
        }

        #endregion

        public bool IsSame(EventJobData other)
        {
            return
            other.m_jobTrigger == m_jobTrigger &&
            other.m_eventHDServer == m_eventHDServer &&
            other.m_eventHDPort == m_eventHDPort &&
            other.m_monitorAllEvents == m_monitorAllEvents &&
            other.m_eventIDs.SequenceEqual(m_eventIDs) &&
            other.m_HDServer == m_HDServer &&
            other.m_HDStores.SequenceEqual(m_HDStores) &&
            other.m_HDPort == m_HDPort &&
            other.m_startRangeFromTrigger == m_startRangeFromTrigger &&
            other.m_stopRangeFromTrigger == m_stopRangeFromTrigger &&
            other.m_preferredTimeBaseTicks == m_preferredTimeBaseTicks &&
            other.m_bPreferredTimeBaseIsAuto == m_bPreferredTimeBaseIsAuto;
        }
    }
}
