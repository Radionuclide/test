using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iba.Data;
using System.IO;
namespace iba.Processing
{
    public class TriggerCalculator
    {
        private ScheduledJobData m_data;

        public TriggerCalculator(ScheduledJobData data)
        {
            m_data = data;
        }

        public bool NextTrigger(DateTime from, out DateTime res)
        {
            res = from;
            switch(m_data.TriggerType)
            {
                case ScheduledJobData.TriggerTypeEnum.OneTime: return OneTimeNextTrigger(from, out res);
                case ScheduledJobData.TriggerTypeEnum.Daily: return DailyNextTrigger(from,out res);
                case ScheduledJobData.TriggerTypeEnum.Weekly: return WeeklyNextTrigger(from, out res);
                case ScheduledJobData.TriggerTypeEnum.Monthly: return MonthlyNextTrigger(from, out res);
            }
            return false;
        }

        public bool PrevTrigger(DateTime from, out DateTime res)
        {
            res = from;
            switch(m_data.TriggerType)
            {
                case ScheduledJobData.TriggerTypeEnum.OneTime: return OneTimePrevTrigger(from, out res);
                case ScheduledJobData.TriggerTypeEnum.Daily: return DailyPrevTrigger(from, out res);
                case ScheduledJobData.TriggerTypeEnum.Weekly: return WeeklyPrevTrigger(from, out res);
                case ScheduledJobData.TriggerTypeEnum.Monthly: return MonthlyPrevTrigger(from, out res);
            }
            return false;
        }

        private bool OneTimeNextTrigger(DateTime from, out DateTime res)
        {
            if(m_data.BaseTriggerTime > from)
            {
                res = m_data.BaseTriggerTime;
                return true;
            }
            return NextRepeat(m_data.BaseTriggerTime, from, out res);
        }

        private bool OneTimePrevTrigger(DateTime from, out DateTime res) 
        {
            if(PrevRepeat(m_data.BaseTriggerTime, from, out res))
                return true;
            res = m_data.BaseTriggerTime;
            return res < from;
        }

        private bool DailyNextTrigger(DateTime from, out DateTime res)
        {
            if(m_data.BaseTriggerTime > from)
            {
                res = m_data.BaseTriggerTime;
                return true;
            }
            long times = (long)Math.Floor(((double)((from - m_data.BaseTriggerTime).Ticks)) / TimeSpan.FromDays(m_data.DayTriggerEveryNDay).Ticks);
            DateTime TimeBefore = m_data.BaseTriggerTime.AddDays(times * m_data.DayTriggerEveryNDay);
            DateTime TimeAfter = m_data.BaseTriggerTime.AddDays((times+1) * m_data.DayTriggerEveryNDay);
            DateTime nextRepeatCandidate;
            if(!NextRepeat(TimeBefore, from, out nextRepeatCandidate) || nextRepeatCandidate > TimeAfter)
                res = TimeAfter;
            else
                res = nextRepeatCandidate;
            return true;
        }

        private bool DailyPrevTrigger(DateTime from, out DateTime res)
        {
            long times = (long)Math.Ceiling(((double)((from - m_data.BaseTriggerTime).Ticks)) / TimeSpan.FromDays(m_data.DayTriggerEveryNDay).Ticks);
            DateTime TimeBefore = m_data.BaseTriggerTime.AddDays((times - 1) * m_data.DayTriggerEveryNDay);
            DateTime prevRepeatCandidate;
            if(!PrevRepeat(TimeBefore, from, out prevRepeatCandidate) || prevRepeatCandidate < TimeBefore)
                res = TimeBefore;
            else
                res = prevRepeatCandidate;
            return true;
        }

        private bool WeeklyNextTrigger(DateTime from, out DateTime res)
        {
            List<DateTime> initialTriggers = new List<DateTime>();

            int offset = (m_data.BaseTriggerTime.DayOfWeek - System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek + 7) % 7;
            int start = -offset;
            int stop = start + 7;
            for(int i = start; i < stop; i++)
            {
                DateTime candidate = m_data.BaseTriggerTime.AddDays(i);
                if(m_data.WeekTriggerWeekDays.Contains((int)(candidate.DayOfWeek)))
                    initialTriggers.Add(candidate);
            }

            if(initialTriggers.Count == 0)
            {
                res = from;
                return false;
            }
            if(initialTriggers[0] > from)
            {
                res = initialTriggers[0];
                return true;
            }
            DateTime TimeBefore = DateTime.MinValue;
            DateTime TimeAfter = DateTime.MaxValue;
            foreach(DateTime initialTrigger in initialTriggers)
            {
                if(initialTrigger > from)
                {
                    if(initialTrigger < TimeAfter)
                        TimeAfter = initialTrigger;
                }
                else
                {
                    long times = (long)Math.Floor(((double)((from - initialTrigger).Ticks)) / TimeSpan.FromDays(7 * m_data.WeekTriggerEveryNWeek).Ticks);
                    DateTime TimeBeforeCandidate = initialTrigger.AddDays(7 * times * m_data.WeekTriggerEveryNWeek);
                    if(TimeBeforeCandidate > TimeBefore)
                        TimeBefore = TimeBeforeCandidate;
                    DateTime TimeAfterCandidate = initialTrigger.AddDays(7 * (times + 1) * m_data.WeekTriggerEveryNWeek);
                    if(TimeAfterCandidate < TimeAfter)
                        TimeAfter = TimeAfterCandidate;
                }
            }
            DateTime nextRepeatCandidate;
            if(!NextRepeat(TimeBefore, from, out nextRepeatCandidate) || nextRepeatCandidate > TimeAfter)
                res = TimeAfter;
            else
                res = nextRepeatCandidate;
            return true;
        }

        private bool WeeklyPrevTrigger(DateTime from, out DateTime res)
        {
            List<DateTime> initialTriggers = new List<DateTime>();
            int offset = (m_data.BaseTriggerTime.DayOfWeek - System.Globalization.DateTimeFormatInfo.CurrentInfo.FirstDayOfWeek + 7) % 7;
            int start = -offset;
            int stop = start + 7;
            for(int i = start; i < stop; i++)
            {
                DateTime candidate = m_data.BaseTriggerTime.AddDays(i);
                if(m_data.WeekTriggerWeekDays.Contains((int)(candidate.DayOfWeek)))
                    initialTriggers.Add(candidate);
            }

            if(initialTriggers.Count == 0)
            {
                res = from;
                return false;
            }

            DateTime TimeBefore = DateTime.MinValue;

            foreach(DateTime initialTrigger in initialTriggers)
            {
                long times = (long)Math.Ceiling(((double)((from - initialTrigger).Ticks)) / TimeSpan.FromDays(7 * m_data.WeekTriggerEveryNWeek).Ticks);
                DateTime TimeBeforeCandidate = initialTrigger.AddDays(7 * (times - 1) * m_data.WeekTriggerEveryNWeek);

                if(TimeBeforeCandidate > TimeBefore)
                    TimeBefore = TimeBeforeCandidate;
            }
            res = TimeBefore;
            DateTime prevRepeatCandidate;
            if(!PrevRepeat(TimeBefore, from, out prevRepeatCandidate) || prevRepeatCandidate < TimeBefore)
                res = TimeBefore;
            else
                res = prevRepeatCandidate;
            return true;
        }

        private bool MonthlyNextTrigger(DateTime from, out DateTime res)
        {
            DateTime TimeBefore = DateTime.MinValue;
            DateTime TimeAfter = DateTime.MaxValue;
            foreach(int month in m_data.MonthTriggerMonths)
            {
                for (int offset = -1; offset <= 1; offset++) //try year before, current and next year
                {
                    if(m_data.MonthTriggerUseDays)
                        MonthlyTriggerDaysOptionPart(from.Year + offset, month, from, ref TimeBefore, ref TimeAfter);
                    else
                        MonthlyTriggerWeekDaysOptionPart(from.Year + offset, month, from, ref TimeBefore, ref TimeAfter);
                }
            }
            if (TimeBefore == DateTime.MinValue && TimeAfter == DateTime.MaxValue)
            {//no candidates found
                res = from;
                return false;
            }
            else if(TimeBefore == DateTime.MinValue)
            {
                res = TimeAfter;
                return true;
            }
            else if (TimeAfter == DateTime.MaxValue)
            {
                return NextRepeat(TimeBefore,from,out res);
            }
            else
            {
                DateTime nextRepeatCandidate;
                if(!NextRepeat(TimeBefore, from, out nextRepeatCandidate) || nextRepeatCandidate > TimeAfter)
                    res = TimeAfter;
                else
                    res = nextRepeatCandidate;
                return true;
            }
        }

        void MonthlyTriggerDaysOptionPart(int year, int month, DateTime from, ref DateTime TimeBefore, ref DateTime TimeAfter)
        {
            foreach(int day in m_data.MonthTriggerDays)
            {
                try
                {
                    int dayc = day;
                    if(dayc == 32) //last
                        dayc = DateTime.DaysInMonth(year, month);
                    DateTime candidate = new DateTime(year, month, dayc, m_data.BaseTriggerTime.Hour,
                        m_data.BaseTriggerTime.Minute, m_data.BaseTriggerTime.Second, m_data.BaseTriggerTime.Millisecond);
                    if(candidate >= m_data.BaseTriggerTime)
                    {
                        if(candidate <= from && candidate > TimeBefore)
                            TimeBefore = candidate;
                        if(candidate > from && candidate < TimeAfter)
                            TimeAfter = candidate;
                    }
                }
                catch(ArgumentOutOfRangeException) //badly formed date, ignore
                {
                }
            }
        }

        void MonthlyTriggerWeekDaysOptionPart(int year, int month, DateTime from, ref DateTime TimeBefore, ref DateTime TimeAfter)
        {
            foreach(int weekday in m_data.MonthTriggerWeekDay)
            {
                List<DateTime> timesonweekday = new List<DateTime>(Enumerable.Range(1, DateTime.DaysInMonth(year, month)).Select(n => new DateTime(year, month, n, 
                    m_data.BaseTriggerTime.Hour,m_data.BaseTriggerTime.Minute, m_data.BaseTriggerTime.Second, m_data.BaseTriggerTime.Millisecond))
                    .Where(d => ((int)(d.DayOfWeek))==weekday));

                if(timesonweekday.Count == 0) continue;

                foreach(int index in m_data.MonthTriggerOn)
                {
                    DateTime candidate;
                    if(index == 4)
                    {
                        candidate = timesonweekday[timesonweekday.Count - 1];
                    }
                    else
                    {
                        if(index >= timesonweekday.Count) continue;
                        candidate = timesonweekday[index];
                    }
                    if(candidate >= m_data.BaseTriggerTime)
                    {
                        if(candidate <= from && candidate > TimeBefore)
                            TimeBefore = candidate;
                        if(candidate > from && candidate < TimeAfter)
                            TimeAfter = candidate;
                    }
                }
            }
        }

        private bool MonthlyPrevTrigger(DateTime from, out DateTime res)
        {
            DateTime TimeBefore = DateTime.MinValue;
            DateTime TimeAfter = DateTime.MaxValue; //don't need this
            foreach(int month in m_data.MonthTriggerMonths)
            {
                for(int offset = -1; offset <= 0; offset++) //try year before, and current year
                {
                    if(m_data.MonthTriggerUseDays)
                        MonthlyTriggerDaysOptionPart(from.Year + offset, month, from, ref TimeBefore, ref TimeAfter);
                    else
                        MonthlyTriggerWeekDaysOptionPart(from.Year + offset, month, from, ref TimeBefore, ref TimeAfter);
                }
            }
            if(TimeBefore == DateTime.MinValue)
            {
                res = from;
                return false;
            }
            DateTime prevRepeatCandidate;
            if(!PrevRepeat(TimeBefore, from, out prevRepeatCandidate) || prevRepeatCandidate < TimeBefore)
                res = TimeBefore;
            else
                res = prevRepeatCandidate;
            return true;
        }
        
        private bool NextRepeat(DateTime before, DateTime target, out DateTime res)
        {
            if(m_data.Repeat)
            {
                if(m_data.RepeatDuration == TimeSpan.Zero || (target <= before + m_data.RepeatDuration)) //in repeat duration interval
                {
                    long times = (long)Math.Ceiling(((double)((target - before).Ticks)) / m_data.RepeatEveryTicks);                  
                    DateTime candidate = before.AddTicks(times * m_data.RepeatEveryTicks);
                    if(candidate == target)
                        candidate = candidate.AddTicks(m_data.RepeatEveryTicks);
                    if((m_data.RepeatDuration == TimeSpan.Zero || (candidate <= before + m_data.RepeatDuration))&& candidate > target)
                    {
                        res = candidate;
                        return true;
                    }
                }
            }
            res = target;
            return false;
        }

        private bool PrevRepeat(DateTime before, DateTime target, out DateTime res)
        {
            if(m_data.Repeat)
            {
                if((m_data.RepeatDuration != TimeSpan.Zero) && (target > before + m_data.RepeatDuration)) 
                {
                    res =  before + m_data.RepeatDuration;
                    return true;
                }
                if(m_data.RepeatDuration == TimeSpan.Zero || (target <= before + m_data.RepeatDuration)) //in repeat duration interval
                {
                    long times = (long)Math.Floor(((double)((target - before).Ticks)) / m_data.RepeatEveryTicks);
                    DateTime candidate = before.AddTicks(times * m_data.RepeatEveryTicks);
                    if(candidate == target && times > 0)
                        candidate = candidate.AddTicks(-m_data.RepeatEveryTicks);
                    if((m_data.RepeatDuration == TimeSpan.Zero || (candidate <= before + m_data.RepeatDuration)) && candidate < target) 
                    {
                        res = candidate;
                        return true;
                    }
                }
            }
            res = target;
            return false;
        }
    }
}
