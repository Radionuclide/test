using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace iba.Utility
{
    public delegate bool SafeTimerCallBack();

    public class SafeTimer : IDisposable
    {
        private SafeTimerCallBack m_callback;
        private System.Threading.Timer m_timer;
        private int m_timerbusy;
        bool m_bDisposed;

        public SafeTimer(SafeTimerCallBack callback)
        {
            m_callback = callback;
            m_timer = new Timer(OnSafeTimerTick);
        }

        private TimeSpan m_period;
        public System.TimeSpan Period
        {
            get { return m_period; }
            set 
            { 
                m_period = value;
                if (System.Threading.Interlocked.CompareExchange(ref m_timerbusy, 1, 0) != 0)
                    return;
                m_timer.Change((long)m_period.TotalMilliseconds, -1);
                m_timerbusy = 0;
            }
        }
        private void OnSafeTimerTick(object ignoreMe)
        {
            if (System.Threading.Interlocked.CompareExchange(ref m_timerbusy, 1, 0) != 0)
                return;
            if (m_bDisposed) return;
            m_timer.Change(Timeout.Infinite, Timeout.Infinite);
            if (m_callback())
                m_timer.Change((long)m_period.TotalMilliseconds, -1);
            m_timerbusy = 0;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (m_bDisposed) return;
            while (System.Threading.Interlocked.CompareExchange(ref m_timerbusy, 1, 0) != 0)
            {
                Thread.Sleep(100);
            }
            if (m_bDisposed) return;
            m_bDisposed = true;
            m_timer.Change(Timeout.Infinite, Timeout.Infinite);
            m_timer.Dispose();
            m_timer = null;
            m_timerbusy = 0;
        }

        ~SafeTimer()
        {
            Dispose(false);
        }  
    }
}
