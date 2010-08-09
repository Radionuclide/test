using System;
using System.Collections.Generic;
using System.Text;
using iba.Data;
using System.Diagnostics;

namespace iba.Processing
{
    class IbaAnalyzerMonitor : IDisposable
    {
        public IbaAnalyzerMonitor(IbaAnalyzer.IbaAnalyzer analyzer, MonitorData data)
        {
            m_analyzer = analyzer;
            m_data = data;
            m_status = MonitorStatus.OK;
            m_timeTimer = null;
            m_memoryTimer = null;
            if (data == null || analyzer == null || (!data.MonitorTime && !data.MonitorMemoryUsage)) return;
            try
            {
                string version = analyzer.GetVersion();
                int startindex = version.IndexOf(' ')+1;
                int stopindex = startindex + 1;
                while(stopindex < version.Length && (char.IsDigit(version[stopindex]) || version[stopindex] == '.'))
                    stopindex++;
                string [] nrs = version.Substring(startindex, stopindex-startindex).Split('.');
                if (nrs.Length < 3) return;
                int major;
                if (!Int32.TryParse(nrs[0],out major)) return;
                int minor;
                if (!Int32.TryParse(nrs[1],out minor)) return;
                int bugfix;
                if (!Int32.TryParse(nrs[2],out bugfix)) return;
                if (major < 5 || (major==5&&minor<8) || (major==5&&minor==8 && bugfix < 1)) return;

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
                m_timeTimer = new System.Threading.Timer(OnTimeTimerTick);
                m_timeTimer.Change(m_data.TimeLimit, TimeSpan.Zero);
            }
            if (data.MonitorMemoryUsage)
            {
                m_memoryTimer = new System.Threading.Timer(OnMemoryTimerTick);
                m_memoryTimer.Change(TimeSpan.FromSeconds(5.0), TimeSpan.Zero);
            }
        }

        private Process m_process;
        private MonitorData m_data;
        private IbaAnalyzer.IbaAnalyzer m_analyzer;
        private System.Threading.Timer m_timeTimer;
        private System.Threading.Timer m_memoryTimer;

        public delegate void IbaAnalyzerCall();

        public void Execute(IbaAnalyzerCall ibaAnalyzerCall)
        {
            //previous execute might allready have exceeded limits, check and throw instead of executing
            if (m_status == MonitorStatus.OUT_OF_MEMORY)
                throw new IbaAnalyzerExceedingMemoryLimitException();
            if (m_status == MonitorStatus.OUT_OF_TIME)
                throw new IbaAnalyzerExceedingTimeLimitException();
            
            //do actual execute;
            try
            {
                ibaAnalyzerCall();
            }
            catch
            {
                if (m_status == MonitorStatus.OK)
                    throw;
                else if (m_status == MonitorStatus.OUT_OF_MEMORY)
                    throw new IbaAnalyzerExceedingMemoryLimitException();
                else if (m_status == MonitorStatus.OUT_OF_TIME)
                    throw new IbaAnalyzerExceedingTimeLimitException();
            }
        }

        public enum MonitorStatus { OK, OUT_OF_MEMORY, OUT_OF_TIME };
        private MonitorStatus m_status;
        public MonitorStatus Status
        {
            get { return m_status; }
        }

        private void OnTimeTimerTick(object ignoreMe)
        {
            try
            {
                if (m_process.HasExited) return;
                m_process.Kill();
                m_status = MonitorStatus.OUT_OF_TIME;
            }
            catch //could not kill process, do nothing
            {
            }
        }

        private void OnMemoryTimerTick(object ignoreMe)
        {
            try
            {
                m_memoryTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                if (m_process.HasExited) return;
                m_process.Refresh();
                long mem = m_process.PrivateMemorySize64;
                if (mem > ((long)(1 << 20) * (long) m_data.MemoryLimit))
                {
                    m_process.Kill();
                    m_status = MonitorStatus.OUT_OF_MEMORY;
                }
                else //look again in 5 seconds
                    m_memoryTimer.Change(TimeSpan.FromSeconds(5.0), TimeSpan.Zero);
            }
            catch //could not kill process, do nothing
            {
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
                    m_timeTimer.Change(System.Threading.Timeout.Infinite,System.Threading.Timeout.Infinite);
                    m_timeTimer.Dispose();
                    m_timeTimer = null;
                }
                if (m_memoryTimer != null)
                {
                    m_memoryTimer.Change(System.Threading.Timeout.Infinite,System.Threading.Timeout.Infinite);
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
}
