using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace iba.Data
{

    [Serializable]
    public class ScheduledJobData : ICloneable, IHDQGenerator
    {
        private DateTime m_baseTriggerTime;
        public System.DateTime BaseTriggerTime
        {
            get { return m_baseTriggerTime; }
            set { m_baseTriggerTime = value; }
        }

        public enum TriggerTypeEnum
        {
            OneTime, Daily, Weekly, Monthly
        }

        private TriggerTypeEnum m_triggerType;
        public iba.Data.ScheduledJobData.TriggerTypeEnum TriggerType
        {
            get { return m_triggerType; }
            set { m_triggerType = value; }
        }

        private int m_dayTriggerEveryNDay;
	    public int DayTriggerEveryNDay
	    {
		    get { return m_dayTriggerEveryNDay; }
		    set { m_dayTriggerEveryNDay = value; }
	    }

        private int m_weekTriggerEveryNWeek;
        public int WeekTriggerEveryNWeek
        {
            get { return m_weekTriggerEveryNWeek; }
            set { m_weekTriggerEveryNWeek = value; }
        }

        private List<int> m_weekTriggerWeekDays;
        public List<int> WeekTriggerWeekDays
        {
            get { return m_weekTriggerWeekDays; }
            set { m_weekTriggerWeekDays = value; }
        }

        private List<int> m_monthTriggerMonths;
        public List<int> MonthTriggerMonths
        {
            get { return m_monthTriggerMonths; }
            set { m_monthTriggerMonths = value; }
        }

        private List<int> m_monthTriggerDays; //32 present means last day of month
        public List<int> MonthTriggerDays
        {
            get { return m_monthTriggerDays; }
            set { m_monthTriggerDays = value; }
        }

        private bool m_monthTriggerUseDays;
        public bool MonthTriggerUseDays
        {
            get { return m_monthTriggerUseDays; }
            set { m_monthTriggerUseDays = value; }
        }

        private List<int> m_monthTriggerOn; //0 = 1st, 1, = Second, 2 = Third, 3 = Fourth, 4 = Last
        public List<int> MonthTriggerOn
        {
            get { return m_monthTriggerOn; }
            set { m_monthTriggerOn = value; }
        }

        private List<int> m_monthTriggerWeekDay;
        public List<int> MonthTriggerWeekDay
        {
            get { return m_monthTriggerWeekDay; }
            set { m_monthTriggerWeekDay = value; }
        }

        #region ICloneable Members

        private bool m_doRepeat;
        public bool Repeat
        {
            get { return m_doRepeat; }
            set { m_doRepeat = value; }
        }
        private TimeSpan m_repeatEvery;

        [XmlIgnore]
        public System.TimeSpan RepeatEvery
        {
            get { return m_repeatEvery; }
            set { m_repeatEvery = value; }
        }
        public long RepeatEveryTicks
        {
            get { return m_repeatEvery.Ticks; }
            set { m_repeatEvery = TimeSpan.FromTicks(value); }
        }

        private int m_repeatTimes;
        public int RepeatTimes
        {
            get { return m_repeatTimes; }
            set { m_repeatTimes = value; }
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

        private static DateTime Round(DateTime dateTime, TimeSpan interval)
        {
            var halfIntervelTicks = ((interval.Ticks + 1) >> 1);

            return dateTime.AddTicks(halfIntervelTicks - ((dateTime.Ticks + halfIntervelTicks) % interval.Ticks));
        }


        internal static bool SerializingFlag = false;


        //time ranges
        public ScheduledJobData()
        {
            m_baseTriggerTime = Round(DateTime.Now, TimeSpan.FromHours(1));
            m_triggerType = TriggerTypeEnum.Daily;
            m_dayTriggerEveryNDay = 1;
            m_weekTriggerEveryNWeek = 1;
            m_monthTriggerUseDays = true;

            if (SerializingFlag) //apparently, for serializing, the default constructor needs to create empty lists...
            {
                m_weekTriggerWeekDays = new List<int>(); //monday (0 is sunday
                m_monthTriggerMonths = new List<int>(); //all months
                m_monthTriggerDays = new List<int>(); //1st day off month;
                m_monthTriggerOn = new List<int>();//1st
                m_monthTriggerWeekDay = new List<int>();//monday
            }
            else
            {
                m_weekTriggerWeekDays = new List<int>(Enumerable.Range(1, 1)); //monday (0 is sunday
                m_monthTriggerMonths = new List<int>(Enumerable.Range(1, 12)); //all months
                m_monthTriggerDays = new List<int>(Enumerable.Range(1, 1)); //1st day off month;
                m_monthTriggerOn = new List<int>(Enumerable.Repeat(0, 1));//1st
                m_monthTriggerWeekDay = new List<int>(Enumerable.Range(1, 1));//monday
            }

            m_doRepeat = false;
            m_repeatEvery = TimeSpan.FromHours(1);
            m_repeatTimes = 0;
            m_HDPort = 9180;
            m_HDStores = new string[] {""};
            m_HDServer = "";
            m_startRangeFromTrigger = TimeSpan.FromHours(1);
            m_stopRangeFromTrigger = TimeSpan.Zero;
            m_bUsePreviousTriggerAsStart = false;
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

        private bool m_bUsePreviousTriggerAsStart;
        public bool UsePreviousTriggerAsStart
        {
            get { return m_bUsePreviousTriggerAsStart; }
            set { m_bUsePreviousTriggerAsStart = value; }
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

        public TimeSpan RepeatDuration 
        {
            get
            {
                return TimeSpan.FromTicks(m_repeatEvery.Ticks * m_repeatTimes);
            }
        }

        public object Clone()
        {
            ScheduledJobData nsjd = new ScheduledJobData();
            nsjd.m_baseTriggerTime = m_baseTriggerTime;
            nsjd.m_triggerType = m_triggerType;
            nsjd.m_dayTriggerEveryNDay = m_dayTriggerEveryNDay;
            nsjd.m_weekTriggerEveryNWeek = m_weekTriggerEveryNWeek;
            nsjd.m_weekTriggerWeekDays = new List<int>(m_weekTriggerWeekDays);
            nsjd.m_monthTriggerMonths = new List<int>(m_monthTriggerMonths); 
            nsjd.m_monthTriggerUseDays = m_monthTriggerUseDays;
            nsjd.m_monthTriggerDays = new List<int>(m_monthTriggerDays);
            nsjd.m_monthTriggerOn = new List<int>(m_monthTriggerOn);
            nsjd.m_monthTriggerWeekDay = new List<int>(m_monthTriggerWeekDay);
            nsjd.m_doRepeat = m_doRepeat;
            nsjd.m_repeatEvery = m_repeatEvery;
            nsjd.m_repeatTimes = m_repeatTimes;
            nsjd.HDServer = m_HDServer;
            nsjd.m_HDStores = (string[]) m_HDStores.Clone();
            nsjd.m_HDPort = m_HDPort;
            nsjd.m_startRangeFromTrigger = m_startRangeFromTrigger;
            nsjd.m_stopRangeFromTrigger = m_stopRangeFromTrigger;
            nsjd.m_bUsePreviousTriggerAsStart = m_bUsePreviousTriggerAsStart;
            nsjd.m_preferredTimeBaseTicks = m_preferredTimeBaseTicks;
            nsjd.m_bPreferredTimeBaseIsAuto = m_bPreferredTimeBaseIsAuto;
            return nsjd;
        }

        #endregion

        public bool IsSame(ScheduledJobData other)
        {
            return
            other.m_baseTriggerTime == m_baseTriggerTime &&
            other.m_triggerType == m_triggerType &&
            other.m_dayTriggerEveryNDay == m_dayTriggerEveryNDay &&
            other.m_weekTriggerEveryNWeek == m_weekTriggerEveryNWeek &&
            other.m_weekTriggerWeekDays.SequenceEqual(m_weekTriggerWeekDays) &&
            other.m_monthTriggerMonths.SequenceEqual(m_monthTriggerMonths) &&
            other.m_monthTriggerUseDays == m_monthTriggerUseDays &&
            other.m_monthTriggerDays.SequenceEqual(m_monthTriggerDays) &&
            other.m_monthTriggerOn.SequenceEqual(m_monthTriggerOn) &&
            other.m_monthTriggerWeekDay.SequenceEqual(m_monthTriggerWeekDay) &&
            other.m_doRepeat == m_doRepeat &&
            other.m_repeatEvery == m_repeatEvery &&
            other.m_repeatTimes == m_repeatTimes &&
            other.m_HDServer == m_HDServer &&
            other.m_HDStores.SequenceEqual(m_HDStores) &&
            other.m_HDPort == m_HDPort &&
            other.m_startRangeFromTrigger == m_startRangeFromTrigger &&
            other.m_stopRangeFromTrigger == m_stopRangeFromTrigger &&
            other.m_bUsePreviousTriggerAsStart == m_bUsePreviousTriggerAsStart &&
            other.m_preferredTimeBaseTicks == m_preferredTimeBaseTicks &&
            other.m_bPreferredTimeBaseIsAuto == m_bPreferredTimeBaseIsAuto;
        }
    }
}
