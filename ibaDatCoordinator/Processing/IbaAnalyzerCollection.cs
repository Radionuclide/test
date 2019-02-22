using System;
using System.Collections.Generic;
using System.Text;
using iba.Data;
namespace iba.Processing
{
    class IbaAnalyzerCollection : FifoSemaphore
    {
        private Dictionary<IbaAnalyzer.IbaAnalyzer, int> m_callCounts;
        private Object m_lock2;
        private Stack<IbaAnalyzer.IbaAnalyzer> m_analyzers;

        private int m_maxCallCount;

        public int MaxCallCount
        {
            get { return m_maxCallCount; }
            set { m_maxCallCount = value; }
        }

        bool m_bLimitCallCount;
        public bool LimitCallCount
        {
            get { return m_bLimitCallCount; }
            set { m_bLimitCallCount = value; }
        }

        private IbaAnalyzerCollection(int maxNumberOfRunningTasks) : base(maxNumberOfRunningTasks)
        {
            m_callCounts = new Dictionary<IbaAnalyzer.IbaAnalyzer, int>();
            m_lock2 = new Object();
            m_analyzers = new Stack<IbaAnalyzer.IbaAnalyzer>();
            m_maxCallCount = 20;
            m_bLimitCallCount = true;
            m_name = "ibaAnalyzer semaphore";
        }

        private static IbaAnalyzerCollection m_collection;

        public static IbaAnalyzerCollection Collection
        {
            get {
                if (m_collection == null) m_collection = new IbaAnalyzerCollection(5);
                return m_collection;
            }
        }

        public IbaAnalyzer.IbaAnalyzer ClaimIbaAnalyzer(ConfigurationData cd)
        {
            bool tryAgain = false;
            Enter();
            lock (m_lock2)
            {
                if (m_analyzers.Count > 0)
                {
                    var ana = m_analyzers.Pop();
                    if (!String.IsNullOrEmpty(cd.FileEncryptionPassword))
                        ana.SetFilePassword("", cd.FileEncryptionPassword);
                    TrySetHDCredentials(ana, cd);
                    return ana;
                }
                else //create one
                {
                    IbaAnalyzer.IbaAnalyzer ibaAnalyzer = StartIbaAnalyzer(cd);
                    if (ibaAnalyzer == null)
                    {
                        if (m_currentNumberOfRunningTasks > 0)
                        {
                            MaxNumberOfRunningTasks = m_currentNumberOfRunningTasks;
                            Leave();
                            tryAgain = true;
                            Log(iba.Logging.Level.Exception, string.Format(iba.Properties.Resources.errIbaAnalyzerDecrease, m_currentNumberOfRunningTasks), cd);

                        }
                        else
                        {//else catastrophic, not a single ibaAnalyzer can be created -> stop everything, handled by job because of the null ibaAnalyzer
                            Log(iba.Logging.Level.Exception, iba.Properties.Resources.errIbaAnalyzersIsZero, cd);
                            return null;
                        }
                    }
                    else
                    {
                        ibaAnalyzer = new ibaAnalyzerWrapper(ibaAnalyzer);
                        if (!String.IsNullOrEmpty(cd.FileEncryptionPassword))
                            ibaAnalyzer.SetFilePassword("", cd.FileEncryptionPassword);
                        TrySetHDCredentials(ibaAnalyzer, cd);
                        m_callCounts.Add(ibaAnalyzer, 0);
                        return ibaAnalyzer;
                    }
                }
            }
            if (tryAgain)
                return ClaimIbaAnalyzer(cd);
            return null;
        }

        public void AddCall(IbaAnalyzer.IbaAnalyzer ibaAnalyzer) //no need to lock 
        {
            if (ibaAnalyzer == null) return;
            lock (m_lock2)
            {
                if (!m_callCounts.ContainsKey(ibaAnalyzer))
                    m_callCounts.Add(ibaAnalyzer, 1);
                else
                    m_callCounts[ibaAnalyzer]++;
            }
        }

        /// <summary>
        /// Return an ibaAnalyzer, possibly kill it
        /// </summary>
        /// <param name="doKill">Set to true if the ibaAnalyzer instance needs to be disposed</param>
        /// <returns>false if kill was asked and the kill failed</returns>
        public bool RelinquishIbaAnalyzer(IbaAnalyzer.IbaAnalyzer ibaAnalyzer, bool doKill, ConfigurationData cd)
        {
            if (ibaAnalyzer == null)
            {
                Leave();
                return true;
            }
            lock (m_lock2)
            {
                int count;
                if (m_callCounts.TryGetValue(ibaAnalyzer, out count) && count >= m_maxCallCount && m_bLimitCallCount)
                {
                    Log(iba.Logging.Level.Debug, String.Format("ibaAnalyzer maximum invocation count reached ({0}), stopping ibaAnalyzer", m_maxCallCount), cd);
                    doKill = true;
                }
                if (doKill)
                {
                    if (!StopIbaAnalyzer(ibaAnalyzer, cd))
                    {
                        Leave();
                        return false;
                    }
                }
                else
                    m_analyzers.Push(ibaAnalyzer);
            }
            Leave();
            return true;
        }

        private bool StopIbaAnalyzer(IbaAnalyzer.IbaAnalyzer ibaAnalyzer, ConfigurationData cd)
        {
            if (ibaAnalyzer == null) return false;
            lock (m_lock2)
            {
                m_callCounts.Remove(ibaAnalyzer);
            }
            try
            {
                try
                {
                    Log(iba.Logging.Level.Debug, string.Format("Stopping ibaAnalyzer with process ID: {0}", ibaAnalyzer.GetProcessID()), cd);
                }
                catch
                {
                }

                ibaAnalyzerWrapper wrapper = ibaAnalyzer as ibaAnalyzerWrapper;
                if (wrapper != null)
                    wrapper.Release();
                else
                {
                    System.Diagnostics.Debug.Assert(false, "We should be only handling ibaAnalyzer wrappers.");
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaAnalyzer);
                }
            }
            catch (Exception ex)
            {
                Log(Logging.Level.Exception, iba.Properties.Resources.errIbaAnalyzerDestroy + ex.Message, cd);
                return false;
            }
            return true;
        }

        public void TryClearIbaAnalyzer(ConfigurationData cd)
        {
            lock (m_lock2)
            {
                if (m_analyzers.Count > 0)
                {
                    StopIbaAnalyzer(m_analyzers.Pop(),cd);
                }
            }
        }

        public void RestartIbaAnalyzer(ref IbaAnalyzer.IbaAnalyzer ibaAnalyzer, ConfigurationData cd)
        { //does not yield unless their is failure
            StopIbaAnalyzer(ibaAnalyzer,cd);
            IbaAnalyzer.IbaAnalyzer newIbaAnalyzer = StartIbaAnalyzer(cd);
            if (newIbaAnalyzer == null)
            {
                ibaAnalyzer = ClaimIbaAnalyzer(cd); //try claiming
            }
            else
            {
                ibaAnalyzer = new ibaAnalyzerWrapper(newIbaAnalyzer);
                if (!String.IsNullOrEmpty(cd.FileEncryptionPassword))
                    ibaAnalyzer.SetFilePassword("", cd.FileEncryptionPassword);
                TrySetHDCredentials(ibaAnalyzer, cd);
            }
        }

        public static void TrySetHDCredentials(IbaAnalyzer.IbaAnalyzer analyzer, ConfigurationData cd)
        {
            string username = "";
            string password = "";

            if (cd.JobType == ConfigurationData.JobTypeEnum.Scheduled)
            {
                username = cd.ScheduleData.HDUsername;
                password = cd.ScheduleData.HDPassword;
            }
            else if (cd.JobType == ConfigurationData.JobTypeEnum.Event)
            {
                username = cd.EventData.HDUsername;
                password = cd.EventData.HDPassword;
            }
            else
                return;

            analyzer?.SetHDCredentials(username, password);
        }

        private enum IbaAnalyzerServerStatus { UNDETERMINED, NONINTERACTIVE, CLASSIC };
        static private IbaAnalyzerServerStatus ibaAnalyzerServerStatus = IbaAnalyzerServerStatus.UNDETERMINED;

        internal void Log(Logging.Level level, string message, ConfigurationData cd)
        {
            LogExtraData data = new LogExtraData(String.Empty, null, cd);
            LogData.Data.Log(level, message, (object)data);
        }

        IbaAnalyzer.IbaAnalyzer StartIbaAnalyzer(ConfigurationData cd)
        {
             //start the com object
            IbaAnalyzer.IbaAnalyzer analyzer = null;
            try
            {
                switch (ibaAnalyzerServerStatus)
                {
                    case IbaAnalyzerServerStatus.NONINTERACTIVE:
                        analyzer = new IbaAnalyzer.IbaAnalysisNonInteractive();
                        break;
                    case IbaAnalyzerServerStatus.CLASSIC:
                        analyzer = new IbaAnalyzer.IbaAnalysis();
                        break;
                    case IbaAnalyzerServerStatus.UNDETERMINED:
                        try
                        {
                            Log(iba.Logging.Level.Debug, "Trying to create an instance of non interactive ibaAnalyzer",cd);
                            analyzer = new IbaAnalyzer.IbaAnalysisNonInteractive();
                            Log(iba.Logging.Level.Debug, "Create an instance of non interactive ibaAnalyzer successful",cd);
                            ibaAnalyzerServerStatus = IbaAnalyzerServerStatus.NONINTERACTIVE;
                        }
                        catch (Exception ex1)
                        {
                            Log(iba.Logging.Level.Debug, "Create an instance of non interactive ibaAnalyzer failed: " + ex1.Message,cd);
                            Log(iba.Logging.Level.Debug, "Trying to create an instance of interactive ibaAnalyzer",cd);
                            analyzer = new IbaAnalyzer.IbaAnalysis();
                            Log(iba.Logging.Level.Debug, "Create an instance of interactive ibaAnalyzer successful",cd);
                            ibaAnalyzerServerStatus = IbaAnalyzerServerStatus.CLASSIC;
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                Log(iba.Logging.Level.Debug, "Create an instance of ibaAnalyzer failed, ibaAnalyzer mode: " + ibaAnalyzerServerStatus.ToString(), cd);
                ibaAnalyzerServerStatus = IbaAnalyzerServerStatus.UNDETERMINED;
                Log(Logging.Level.Exception, iba.Properties.Resources.errIbaAnalyzerCreate +  ex.Message, cd);
                return null;
            }

            try
            {
                Log(iba.Logging.Level.Debug, string.Format("New ibaAnalyzer started with process ID: {0} Mode: {1}", analyzer.GetProcessID(), ibaAnalyzerServerStatus),cd);
            }
            catch
            {
                Log(iba.Logging.Level.Debug, "New ibaAnalyzer started, could not get ProcessID",cd);
            }
            return analyzer;
        }
    }
}
