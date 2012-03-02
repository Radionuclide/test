﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class PauseTaskData : TaskData
    {
        public PauseTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.pauseTaskTitle;
            m_interval = TimeSpan.FromSeconds(30);
            m_measureFromFileTime = true;
        }

        public PauseTaskData()
            : this(null)
        {

        }

        public override object Clone()
        {
            PauseTaskData pd = new PauseTaskData(null);
            pd.m_name = m_name;
            pd.m_notify = m_notify;
            pd.m_wtodo = m_wtodo;

            pd.m_interval = m_interval;
            pd.m_measureFromFileTime = m_measureFromFileTime;

            return pd;
        }

        private TimeSpan m_interval;

        [XmlIgnore]
        public TimeSpan Interval
        {
            get { return m_interval; }
            set { m_interval = value; }
        }

        public long IntervalTicks
        {
            get { return m_interval.Ticks; }
            set { m_interval = new TimeSpan(value); }
        }

        bool m_measureFromFileTime;
        public bool MeasureFromFileTime
        {
            get { return m_measureFromFileTime; }
            set { m_measureFromFileTime = value; }
        }

        public override bool IsSame(TaskData taskData)
        {
            PauseTaskData other = taskData as PauseTaskData;
            if (other == null) return false;
            if (other == this) return true;
            return
            other.m_name == m_name &&
            other.m_notify == m_notify &&
            other.m_wtodo == m_wtodo &&
            other.m_interval == m_interval &&
            other.m_measureFromFileTime == m_measureFromFileTime;
        }
    }
}
