using System;
using System.Collections.Generic;
using System.Text;
using iba.Data;
using System.Diagnostics;
using iba.Logging;
using iba.Utility;

namespace iba.Processing
{
    class IbaAnalyzerMonitor : iba.Processing.IIbaAnalyzerMonitor, IDisposable
    {
        public IbaAnalyzerMonitor(IbaAnalyzer.IbaAnalyzer analyzer, MonitorData data)
        {
            m_analyzer = analyzer;
            m_data = data;
            m_status = MonitorStatus.OK;
            m_timeTimer = null;
            m_memoryTimer = null;
            if (data == null || analyzer == null /*|| (!data.MonitorTime && !data.MonitorMemoryUsage)*/) return;
            try
            {
                //deleted outdated version check..
                m_process = Process.GetProcessById(analyzer.GetProcessID());
                if (m_process == null) return;
            }
            catch
            {
                //something wrong with analyzer or could not create process, do nothing
                return;
            }

            if (data.MonitorTime)
            {
                m_timeTimer = new SafeTimer(OnTimeTimerTick);
                m_timeTimer.Period = m_data.TimeLimit;
            }
            if (data.MonitorMemoryUsage)
            {
                m_memoryTimer = new SafeTimer(OnMemoryTimerTick);
                m_memoryTimer.Period = TimeSpan.FromSeconds(5.0);
            }
        }

        private Process m_process;
        private MonitorData m_data;
        private IbaAnalyzer.IbaAnalyzer m_analyzer;
        private SafeTimer m_timeTimer;
        private SafeTimer m_memoryTimer;
        public void Execute(IbaAnalyzerCall ibaAnalyzerCall)
        {
            //previous execute might already have exceeded limits, check and throw instead of executing
            if (m_status == MonitorStatus.OUT_OF_MEMORY)
                throw new IbaAnalyzerExceedingMemoryLimitException();
            if (m_status == MonitorStatus.OUT_OF_TIME)
                throw new IbaAnalyzerExceedingTimeLimitException();
            
            //do actual execute;
            try
            {
                ibaAnalyzerCall();
                // use increase-only setter to calculate the max amount
                // between current value and 
                // all the intermediate measurements @ OnMemoryTimerTick() (if they exist)
                if (m_process != null)
                    m_data.MemoryUsed = (uint)(m_process.PrivateMemorySize64 >> 20);
            }
            catch
            {
                if (m_status == MonitorStatus.OK)
                {
                    throw new IbaAnalyzerOtherException();
                }
                else if (m_status == MonitorStatus.OUT_OF_MEMORY)
                    throw new IbaAnalyzerExceedingMemoryLimitException();
                else if (m_status == MonitorStatus.OUT_OF_TIME)
                    throw new IbaAnalyzerExceedingTimeLimitException();
            }
        }

        private MonitorStatus m_status;
        public MonitorStatus Status
        {
            get { return m_status; }
        }

        private bool OnTimeTimerTick()
        {
            //throw new IbaAnalyzerExceedingTimeLimitException();
            try
            {
                if (m_process.HasExited) return false;
                m_process.Kill();
                m_status = MonitorStatus.OUT_OF_TIME;
            }
            catch  (Exception ex) //could not kill process, do nothing
            {
                LogData.Data.Logger.Log(Level.Exception, "Time limit exceeded, could not kill ibaAnalyzer :" + ex.Message);
            }
            return false; //it never needs to be restarted
        }

        private bool OnMemoryTimerTick()
        {
            try
            {
                if (m_process == null || m_process.HasExited) return false;
                m_process.Refresh();
                long mem = m_process.PrivateMemorySize64;
                // added by kolesnik - begin
                // use increase-only setter to calculate the max amount
                // between current value and all the previous values 
                m_data.MemoryUsed = (uint) (mem >> 20);
                // added by kolesnik - end

                if (m_data.MonitorMemoryUsage && (mem > ((long)(1 << 20) * (long) m_data.MemoryLimit)))
                {
                    m_process.Kill();
                    m_status = MonitorStatus.OUT_OF_MEMORY;
                    return false;
                }
                else //look again in 5 seconds
                    return true;
            }
            catch (Exception ex) //could not kill process, do nothing
            {
                LogData.Data.Logger.Log(Level.Exception, "Memory limit exceeded, could not kill ibaAnalyzer :" + ex.Message);
                return false;
            }
        }

        #region IDisposable Members

        private bool disposed = false;
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (m_timeTimer != null)
                {
                    m_timeTimer.Dispose();
                    m_timeTimer = null;
                }
                if (m_memoryTimer != null)
                {
                    m_memoryTimer.Dispose();
                    m_memoryTimer = null;
                }
                if (m_process != null)
                {
                    m_process.Dispose();
                    m_process = null;
                }
            }
            disposed = true;
        }
        
        ~IbaAnalyzerMonitor()
        {
            Dispose(false);
        }       

        #endregion
    }

    public class IbaAnalyzerExceedingTimeLimitException : ApplicationException
    {
        public IbaAnalyzerExceedingTimeLimitException(string message)
            : base(message)
        {
        }

        public IbaAnalyzerExceedingTimeLimitException()
            : base(iba.Properties.Resources.errAnalyzerTime)
        {
        }

    }

    public class IbaAnalyzerExceedingMemoryLimitException : ApplicationException
    {
        public IbaAnalyzerExceedingMemoryLimitException(string message)
            : base(message)
        {
        }

        public IbaAnalyzerExceedingMemoryLimitException()
            : base(iba.Properties.Resources.errAnalyzerMemory)
        {
        }
    }

    public class IbaAnalyzerOtherException : ApplicationException
    {

    }

}
