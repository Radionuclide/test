using iba.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace iba.Data
{
    public enum EventJobRangeCenter
    {
        Incoming,
        Outgoing,
        Both
    }

    [Serializable]
    public class EventJobData : ICloneable
    {
        #region ICloneable Members
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

        private string m_HDUsername;
        public string HDUsername
        {
            get { return m_HDUsername; }
            set { m_HDUsername = value; }
        }

        private string m_HDPassword;

        [XmlIgnore]
        public string HDPassword
        {
            get { return m_HDPassword; }
            set { m_HDPassword = value; }
        }

        public string HDPasswordCrypted
        {
            get { return Crypt.Encrypt(m_HDPassword); }
            set { m_HDPassword = Crypt.Decrypt(value); }
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
            m_eventIDs = new List<string>();

            m_HDPort = 9180;
            m_HDStores = new string[] { "" };
            m_HDServer = "";
            m_HDUsername = "";
            m_HDPassword = "";
            m_rangeCenter = (int)EventJobRangeCenter.Incoming;
            m_maxTriggerRange = TimeSpan.FromHours(1.0);
            m_preTriggerRange = TimeSpan.Zero;
            m_postTriggerRange = TimeSpan.Zero;
            m_enablePreTrigger = false;
            m_enablePostTrigger = false;
            m_enableMaxTrigger = true;
            m_preferredTimeBaseTicks = 0;
            m_bPreferredTimeBaseIsAuto = true;
        }

        private int m_rangeCenter;
        [XmlIgnore]
        public EventJobRangeCenter RangeCenter
        {
            get { return (EventJobRangeCenter)m_rangeCenter; }
            set { m_rangeCenter = (int)value; }
        }
        public int RangeCenterInt
        {
            get { return m_rangeCenter; }
            set { m_rangeCenter = value; }
        }

        private bool m_enablePreTrigger;
        public bool EnablePreTriggerRange
        {
            get { return m_enablePreTrigger; }
            set { m_enablePreTrigger = value; }
        }

        private TimeSpan m_preTriggerRange;
        [XmlIgnore]
        public System.TimeSpan PreTriggerRange
        {
            get { return m_preTriggerRange; }
            set { m_preTriggerRange = value; }
        }
        public long PreTriggerRangeTicks
        {
            get { return m_preTriggerRange.Ticks; }
            set { m_preTriggerRange = TimeSpan.FromTicks(value); }
        }

        private bool m_enablePostTrigger;
        public bool EnablePostTriggerRange
        {
            get { return m_enablePostTrigger; }
            set { m_enablePostTrigger = value; }
        }

        private TimeSpan m_postTriggerRange;
        [XmlIgnore]
        public System.TimeSpan PostTriggerRange
        {
            get { return m_postTriggerRange; }
            set { m_postTriggerRange = value; }
        }
        public long PostTriggerRangeTicks
        {
            get { return m_postTriggerRange.Ticks; }
            set { m_postTriggerRange = TimeSpan.FromTicks(value); }
        }

        private bool m_enableMaxTrigger;
        public bool EnableMaxTriggerRange
        {
            get { return m_enableMaxTrigger; }
            set { m_enableMaxTrigger = value; }
        }

        private TimeSpan m_maxTriggerRange;
        [XmlIgnore]
        public System.TimeSpan MaxTriggerRange
        {
            get { return m_maxTriggerRange; }
            set { m_maxTriggerRange = value; }
        }
        public long MaxTriggerRangeTicks
        {
            get { return m_maxTriggerRange.Ticks; }
            set { m_maxTriggerRange = TimeSpan.FromTicks(value); }
        }

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

        private DateTime m_StartTimeHdQuery;
        public DateTime StartTimeHdQuery
        {
            get { return m_StartTimeHdQuery; }
            set { m_StartTimeHdQuery = value; }
        }

        public long StartTimeHdQueryTicksUTC
        {
            get { return m_StartTimeHdQuery.ToUniversalTime().Ticks; }
        }

        private DateTime m_StopTimeHdQuery;
        public DateTime StopTimeHdQuery
        {
            get { return m_StopTimeHdQuery; }
            set { m_StopTimeHdQuery = value; }
        }

        public long StopTimeHdQueryTicksUTC
        {
            get { return m_StopTimeHdQuery.ToUniversalTime().Ticks; }
        }

        [NonSerialized]
        private Dictionary<string, long> m_lastReceivedHistoricalTimeStamp;

        [XmlIgnore]
        public Dictionary<string, long> LastReceivedHistoricalTimeStamp
        {
            get { return m_lastReceivedHistoricalTimeStamp; }
            set { m_lastReceivedHistoricalTimeStamp = value; }
        }

        private bool m_bHdQueryTimeSpanChanged;
        public bool HdQueryTimeSpanChanged
        {
            get { return m_bHdQueryTimeSpanChanged; }
            set { m_bHdQueryTimeSpanChanged = value; }
        }

        private bool m_bHdQueryEnabled;
        public bool HdQueryEnabled
        {
            get { return m_bHdQueryEnabled; }
            set { m_bHdQueryEnabled = value; }
        }

        private bool m_bHdQueryUseEndTime;
        public bool HdQueryUseEndTime
        {
            get { return m_bHdQueryUseEndTime; }
            set { m_bHdQueryUseEndTime = value; }
        }

        public object Clone()
        {
            EventJobData nejd = new EventJobData();
            nejd.EventIDs = new List<string>(m_eventIDs);
            nejd.HDServer = m_HDServer;
            nejd.m_HDStores = (string[])m_HDStores.Clone();
            nejd.m_HDPort = m_HDPort;
            nejd.m_HDUsername = m_HDUsername;
            nejd.m_HDPassword = m_HDPassword;
            nejd.m_rangeCenter = m_rangeCenter;
            nejd.m_maxTriggerRange = m_maxTriggerRange;
            nejd.m_preTriggerRange = m_preTriggerRange;
            nejd.m_postTriggerRange = m_postTriggerRange;
            nejd.m_enableMaxTrigger = m_enableMaxTrigger;
            nejd.m_enablePreTrigger = m_enablePreTrigger;
            nejd.m_enablePostTrigger = m_enablePostTrigger;
            nejd.m_bPreferredTimeBaseIsAuto = m_bPreferredTimeBaseIsAuto;
            nejd.m_preferredTimeBaseTicks = m_preferredTimeBaseTicks;
            nejd.m_lastReceivedHistoricalTimeStamp = m_lastReceivedHistoricalTimeStamp;
            nejd.m_bHdQueryTimeSpanChanged = m_bHdQueryTimeSpanChanged;
            nejd.m_StartTimeHdQuery = m_StartTimeHdQuery;
            nejd.m_StopTimeHdQuery = m_StopTimeHdQuery;
            nejd.HdQueryEnabled = HdQueryEnabled;
            nejd.HdQueryUseEndTime = HdQueryUseEndTime;
            return nejd;
        }

        #endregion

        public bool IsSame(EventJobData other)
        {
            bool res = other.m_eventIDs.SequenceEqual(m_eventIDs) &&
            other.m_HDServer == m_HDServer &&
            other.m_HDStores.SequenceEqual(m_HDStores) &&
            other.m_HDPort == m_HDPort &&
            other.m_HDUsername == m_HDUsername &&
            other.m_HDPassword == m_HDPassword &&
            other.m_rangeCenter == m_rangeCenter &&
            other.m_maxTriggerRange == m_maxTriggerRange &&
            other.m_preTriggerRange == m_preTriggerRange &&
            other.m_postTriggerRange == m_postTriggerRange &&
            other.m_enablePreTrigger == m_enablePreTrigger &&
            other.m_enablePostTrigger == m_enablePostTrigger &&
            other.m_enableMaxTrigger == m_enableMaxTrigger &&
            other.m_bPreferredTimeBaseIsAuto == m_bPreferredTimeBaseIsAuto;

            if (!res)
                return res;

            if (!m_bPreferredTimeBaseIsAuto) //in case of auto, ticks is not compared
                res = res && other.m_preferredTimeBaseTicks == m_preferredTimeBaseTicks;

            return res;
        }
    }
}
