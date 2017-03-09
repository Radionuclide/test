﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;


namespace iba.Processing
{
    public class FifoSemaphore
    {
        protected string m_name;

        public FifoSemaphore(int maxNumberOfRunningTasks)
        {
            m_maxNumberOfRunningTasks = maxNumberOfRunningTasks;
            m_lock = new Object();
            m_waitingTasks = new Queue<AutoResetEvent>();
            m_name = "Resource intensive tasks semaphore";
        }
        private Object m_lock;

        private Queue<AutoResetEvent> m_waitingTasks;
        private int m_maxNumberOfRunningTasks;
        protected int m_currentNumberOfRunningTasks;

        public void Enter()
        {
            AutoResetEvent ev = null;
            lock(m_lock)
            {
                if (m_currentNumberOfRunningTasks < m_maxNumberOfRunningTasks)
                {
                    m_currentNumberOfRunningTasks++;
                }
                else //wait
                {
                    ev = new AutoResetEvent(false);
                    m_waitingTasks.Enqueue(ev);
                }
            }
            if (ev != null)
            {
                iba.Data.LogData.Data.Log(iba.Logging.Level.Debug, m_name + ": Semaphore full, halting execution for thread: " + System.Threading.Thread.CurrentThread.Name);
                ev.WaitOne();
                iba.Data.LogData.Data.Log(iba.Logging.Level.Debug, m_name + ": Resuming execution for thread: " + System.Threading.Thread.CurrentThread.Name);
                ev.Close();
            }
        }

        public void Leave()
        {
            lock (m_lock)
            {
                if (m_waitingTasks.Count > 0)
                {
                    m_waitingTasks.Dequeue().Set();
                }
                else
                {
                    m_currentNumberOfRunningTasks--;
                    if (m_currentNumberOfRunningTasks < 0)
                        m_currentNumberOfRunningTasks = 0; //should not happen, means a mismatch of Enters/Leaves happened.
                }
            }
        }

        public FifoSemaphoreClient CreateClient()
        {
            return new FifoSemaphoreClient(this);
        }


        public class FifoSemaphoreClient : IDisposable
        {
            private FifoSemaphore m_semaphore;

            public FifoSemaphoreClient(FifoSemaphore semaphore)
            {
                m_semaphore = semaphore;
                m_semaphore.Enter();
            }

            #region IDisposable Members

            public void Dispose()
            {
                m_semaphore.Leave();
            }

            #endregion
        }

        public int MaxNumberOfRunningTasks
        {
            get { return m_maxNumberOfRunningTasks; }
            set
            {
                lock (m_lock)
                {
                    m_maxNumberOfRunningTasks = value;
                    while (m_waitingTasks.Count > 0 && m_currentNumberOfRunningTasks < m_maxNumberOfRunningTasks)
                    {
                        m_waitingTasks.Dequeue().Set();
                        m_currentNumberOfRunningTasks++;
                    }
                }
            }
        }
    }
}
