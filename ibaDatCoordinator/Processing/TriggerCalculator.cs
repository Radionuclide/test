using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iba.Data;
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
                case ScheduledJobData.TriggerTypeEnum.OneTime: return OneTimeTrigger(from, out res);
                case ScheduledJobData.TriggerTypeEnum.Daily: return DailyTrigger(from,out res);
                case ScheduledJobData.TriggerTypeEnum.Weekly: return WeeklyTrigger(from, out res);
                case ScheduledJobData.TriggerTypeEnum.Monthly: return MonthlyTrigger(from, out res);
            }
            return false;
        }

        private bool OneTimeTrigger(DateTime from, out DateTime res)
        {
            if(m_data.BaseTriggerTime > from)
            {
                res = m_data.BaseTriggerTime;
                return true;
            }
            return NextRepeat(m_data.BaseTriggerTime, from, out res);
        }

        private bool DailyTrigger(DateTime from, out DateTime res)
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

        private bool WeeklyTrigger(DateTime from, out DateTime res)
        {
            List<DateTime> initialTriggers = new List<DateTime>();

            for (int i = 0; i < 7; i++)
            {
                DateTime candidate = m_data.BaseTriggerTime.AddDays(i);
                if (m_data.WeekTriggerWeekDays.Contains((int)(candidate.DayOfWeek)))
                    initialTriggers.Add(candidate);
            }
            if (initialTriggers.Count == 0)
            {
                res = from;
                return false;
            }
            if (initialTriggers[0] > from)
            {
                res = initialTriggers[0];
                return true;
            }
            DateTime TimeBefore = DateTime.MinValue;
            DateTime TimeAfter = DateTime.MaxValue;
            foreach (DateTime initialTrigger in initialTriggers)
            {
                if (initialTrigger > from)
                {
                    if (initialTrigger < TimeAfter)
                        TimeAfter = initialTrigger;
                }
                else
                {
                    long times = (long)Math.Floor(((double)((from - m_data.BaseTriggerTime).Ticks)) / TimeSpan.FromDays(7*m_data.WeekTriggerEveryNWeek).Ticks);
                    DateTime TimeBeforeCandidate = m_data.BaseTriggerTime.AddDays(7 * times * m_data.WeekTriggerEveryNWeek);
                    if (TimeBeforeCandidate > TimeBefore)
                        TimeBefore = TimeBeforeCandidate;
                    DateTime TimeAfterCandidate = m_data.BaseTriggerTime.AddDays(7*(times + 1) * m_data.WeekTriggerEveryNWeek);
                    if (TimeAfterCandidate < TimeAfter)
                        TimeAfter = TimeAfterCandidate;
                }
            }
            DateTime nextRepeatCandidate;
            if (!NextRepeat(TimeBefore, from, out nextRepeatCandidate) || nextRepeatCandidate > TimeAfter)
                res = TimeAfter;
            else
                res = nextRepeatCandidate;
            res = from;
            return false;
        }

        private bool MonthlyTrigger(DateTime from, out DateTime res)
        {
            if (m_data.MonthTriggerUseDays)
                return MonthlyTriggerDays(from, out res);
            else
                return MonthlyTriggerWeekDays(from, out res);
        }

        private bool MonthlyTriggerDays(DateTime from, out DateTime res)
        {
            throw new NotImplementedException();
        }

        private bool MonthlyTriggerWeekDays(DateTime from, out DateTime res)
        {
            throw new NotImplementedException();
        }
        
        private bool NextRepeat(DateTime baseTime, DateTime from, out DateTime res)
        {
            if(m_data.Repeat)
            {
                if(m_data.RepeatDuration == TimeSpan.Zero || (from <= m_data.BaseTriggerTime + m_data.RepeatDuration)) //in repeat duration interval
                {
                    long times = (long)Math.Ceiling(((double)((from - m_data.BaseTriggerTime).Ticks)) / m_data.RepeatEveryTicks);                  
                    DateTime candidate = m_data.BaseTriggerTime.AddTicks(times * m_data.RepeatEveryTicks);
                    if((m_data.RepeatDuration == TimeSpan.Zero || (candidate <= m_data.BaseTriggerTime + m_data.RepeatDuration))&& candidate > from)
                    {
                        res = candidate;
                        return true;
                    }
                }
            }
            res = from;
            return false;
        }

        public void GenerateHDQFile(DateTime trigger)
        {

        }
    }
}
