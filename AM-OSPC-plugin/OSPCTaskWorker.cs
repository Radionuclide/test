using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iba.Plugins;
using System.IO;

namespace AM_OSPC_plugin
{
    public class OSPCTaskWorker : IPluginTaskWorker
    {
        private bool m_bStarted;
        private String m_error;
        public bool OnStart()
        {
            m_bStarted = true;
            return true;
        }

        public bool OnStop()
        {
            m_bStarted = false;
            return true;
        }

        public bool OnApply(IPluginTaskData newtask, IJobData newParentJob)
        {
            m_dataToApply = newtask as OSPCTaskData;
            return true;
        }

        public bool ExecuteTask(string datFile)
        {
            if(m_dataToApply != null)
            {
                if(!CheckIbaAnalyzer())
                {
                    m_error = Properties.Resources.ibaAnalyzerVersionError;
                    return false;
                }
                m_data = m_dataToApply;
                m_dataToApply = null;
            }
            m_error = "";
            
            //assumes ibaAnalyzer is loaded with the .dat File -> get .dat file date
            DateTime startTime = new DateTime();
            int microSecPart = 0;
            m_monitor.Execute(delegate() { m_analyzer.GetStartTime(ref startTime, ref microSecPart); }); //can throw
            startTime = startTime.AddTicks(microSecPart * 10);
            startTime = startTime.ToUniversalTime();
            int count = 20;
            for(int i = 19; i >= 0; i--)
            {
                if(!string.IsNullOrEmpty(m_data.Records[i].VariableName)
                    || !string.IsNullOrEmpty(m_data.Records[i].Expression)
                    || !string.IsNullOrEmpty(m_data.Records[i].ProcessName))
                    break;
                count = i;
            }

            if(count == 0)
            {
                m_error = Properties.Resources.NoValidEntriesSpecified;
                return false;
            }

            if(!string.IsNullOrEmpty(m_data.AnalysisFile))
            {
                if(!File.Exists(m_data.AnalysisFile))
                {
                    m_error = Properties.Resources.AnalysisFileNotFound + m_data.AnalysisFile;
                    return false;
                }
                m_monitor.Execute(delegate() { m_analyzer.OpenAnalysis(m_data.AnalysisFile); });
            }
            try
            {
                using (AMOSPCprotocol.OSPCConnector connector = new AMOSPCprotocol.OSPCConnector())
                {
                    int validCount = 0;
                    for (int i = 0; i < count; i++)
                    {
                        OSPCTaskData.Record record = m_data.Records[i];
                        double f = double.NaN;
                        if (!string.IsNullOrEmpty(record.Expression))
                        {
                            if (m_bNewVersion)
                                m_monitor.Execute(delegate () { f = m_analyzer.EvaluateDouble(record.Expression, 0); });
                            else
                                m_monitor.Execute(delegate () { f = (double)m_analyzer.Evaluate(record.Expression, 0); });
                        }
                        if (!Double.IsNaN(f) && !Double.IsInfinity(f))
                        {
                            validCount++;
                        }
                        connector.AddRecord(record.ProcessName, record.VariableName, f);
                    }
                    if (validCount == 0)
                    {
                        m_error = Properties.Resources.NoValidEntriesSpecified;
                        return false;
                    }
                    connector.Connect(m_data.OspcServerHost, m_data.OspcServerUser, m_data.OspcServerPassword);
                    connector.Send(startTime);
                }
            }        
            catch (Exception ex)
            {
                iba.Logging.ibaLogger.Log(iba.Logging.Level.Exception, ex.ToString());
                throw;
            }
            return true;
        }

        private bool m_bNewVersion;

        private bool CheckIbaAnalyzer()
        {
            string version = m_analyzer.GetVersion();
            int startindex = version.IndexOf(' ') + 1;
            int stopindex = startindex + 1;
            while(stopindex < version.Length && (char.IsDigit(version[stopindex]) || version[stopindex] == '.'))
                stopindex++;
            string[] nrs = version.Substring(startindex, stopindex - startindex).Split('.');
            if(nrs.Length < 3)
            {
                m_error = Properties.Resources.NoVersion;
                return false;
            }
            int major;
            if(!Int32.TryParse(nrs[0], out major))
            {
                m_error = Properties.Resources.NoVersion;
                return false;
            }
            int minor;
            if(!Int32.TryParse(nrs[1], out minor))
            {
                m_error = Properties.Resources.NoVersion;
                return false;
            }
            int bugfix;
            if(!Int32.TryParse(nrs[2], out bugfix))
            {
                m_error = Properties.Resources.NoVersion;
                return false;
            }
            if(major < 6 || (major == 6 && minor < 5))
            {
                m_error = Properties.Resources.ibaAnalyzerVersionError;
                return false;
            }
            m_bNewVersion = true;
            if(major < 6 || (major == 6 && minor < 7))
            {
                m_bNewVersion = false;
            }
            return true;
        }

        public string GetLastError()
        {
            return m_error;
        }

        public PluginTaskWorkerStatus GetWorkerStatus()
        {
            PluginTaskWorkerStatus status = new PluginTaskWorkerStatus();
            status.started = m_bStarted;
            return status;
        }

        private OSPCTaskData m_data;
        private OSPCTaskData m_dataToApply;
        public OSPCTaskWorker(OSPCTaskData data)
        {
            m_dataToApply = data;
        }

       IbaAnalyzer.IbaAnalyzer m_analyzer;
       iba.Processing.IIbaAnalyzerMonitor m_monitor;

        internal void SetIbaAnalyzer(IbaAnalyzer.IbaAnalyzer Analyzer, iba.Processing.IIbaAnalyzerMonitor Monitor)
        {
            m_analyzer = Analyzer;
            m_monitor = Monitor;
        }
    }
}
