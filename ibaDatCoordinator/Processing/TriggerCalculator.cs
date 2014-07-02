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
            throw new NotImplementedException();
        }

        private bool MonthlyTrigger(DateTime from, out DateTime res)
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
                    if(m_data.RepeatDuration == TimeSpan.Zero || (candidate <= m_data.BaseTriggerTime + m_data.RepeatDuration))
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
