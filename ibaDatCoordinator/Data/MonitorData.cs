using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class MonitorData: ICloneable
    {
        private bool m_bMonitorTime;
        public bool MonitorTime
        {
            get { return m_bMonitorTime; }
            set { m_bMonitorTime = value; }
        }

        private bool m_bMonitorMemoryUsage;
        public bool MonitorMemoryUsage
        {
            get { return m_bMonitorMemoryUsage; }
            set { m_bMonitorMemoryUsage = value; }
        }
        
        private TimeSpan m_timeLimit;
        [XmlIgnore]
        public TimeSpan TimeLimit
        {
            get { return m_timeLimit; }
            set { m_timeLimit = value; }
        }
        public long TimeLimitTicks
        {
            get { return m_timeLimit.Ticks; }
            set { m_timeLimit = new TimeSpan(value); }
        }

        private uint m_memoryLimit;
        public uint MemoryLimit
        {
            get { return m_memoryLimit; }
            set { m_memoryLimit = value; }
        }

        public MonitorData()
        {
            m_bMonitorMemoryUsage = m_bMonitorTime = false;
            m_timeLimit = TimeSpan.FromMinutes(10.0);
            m_memoryLimit = 512;
        }
        
        public object Clone()
        {
            MonitorData md = new MonitorData();
            md.m_bMonitorMemoryUsage = m_bMonitorMemoryUsage;
            md.m_bMonitorTime = m_bMonitorTime;
            md.m_memoryLimit = m_memoryLimit;
            md.m_timeLimit = m_timeLimit;
            return md;
        }
    }
}
