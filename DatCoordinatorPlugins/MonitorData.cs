using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class MonitorData : ICloneable
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

        // added by kolesnik - begin
        private uint m_memoryUsed;
        /// <summary>
        /// Max value of memory (in megabytes) used by ibaAnalyzer.
        /// Setter is of increase-only type.
        /// Getter resets the value in order to start the new max-evaluation. 
        /// Is used to be shown via SNMP.
        /// </summary>
        public uint MemoryUsed
        {
            get
            {
                // remember last value
                var val = m_memoryUsed;
                // reset in order to start the new max-evaluation
                m_memoryUsed = 0;
                // return what was there before the call
                return val;
            }
            set { m_memoryUsed = Math.Max(m_memoryUsed, value); }
        }
        // added by kolesnik - end

        public MonitorData()
        {
            m_bMonitorMemoryUsage = m_bMonitorTime = true;
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

        public bool IsSame(MonitorData other)
        {
            return other.m_bMonitorMemoryUsage == m_bMonitorMemoryUsage &&
            other.m_bMonitorTime == m_bMonitorTime &&
            other.m_memoryLimit == m_memoryLimit &&
            other.m_timeLimit == m_timeLimit;
        }
    }
}
