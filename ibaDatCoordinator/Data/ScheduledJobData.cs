using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace iba.Data
{

    [Serializable]
    public class ScheduledJobData : ICloneable
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

        private TimeSpan m_repeatDuration;
        [XmlIgnore]
        public TimeSpan RepeatDuration
        {
            get { return m_repeatDuration; }
            set { m_repeatDuration = value; }
        }
        public long RepeatDurationTicks
        {
            get { return m_repeatDuration.Ticks; }
            set { m_repeatDuration= TimeSpan.FromTicks(value); }
        }

        //TODO HD parameters (two ranges, sample rate and hd connection params)

        public ScheduledJobData()
        {
            m_baseTriggerTime = DateTime.Now;
            m_triggerType = TriggerTypeEnum.Daily;
            m_dayTriggerEveryNDay = 1;
            m_weekTriggerEveryNWeek = 1;
            m_weekTriggerWeekDays = new List<int>(Enumerable.Range(1,1)); //monday (0 is sunday
            m_monthTriggerMonths = new List<int>(Enumerable.Range(1, 12)); //all months
            m_monthTriggerUseDays = true;
            m_monthTriggerDays = new List<int>(Enumerable.Range(1, 1)); //1st day off month;
            m_monthTriggerOn = new List<int>(Enumerable.Range(1, 1));//1st
            m_monthTriggerWeekDay = new List<int>(Enumerable.Range(1, 1));//monday
            m_doRepeat = false;
            m_repeatEvery = TimeSpan.FromHours(1);
            m_repeatDuration = TimeSpan.FromDays(1);
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
            nsjd.m_repeatDuration = m_repeatDuration;
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
            other.m_repeatDuration == m_repeatDuration;
        }
    }
}
