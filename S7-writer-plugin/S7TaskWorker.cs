using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iba.Plugins;
using System.IO;
using S7_writer;

namespace S7_writer_plugin
{
    public class S7TaskWorker : IPluginTaskWorker
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
            m_dataToApply = newtask as S7TaskData;
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
                
                //Take copy of task data instead of just a reference so that we can work independently
                m_data = m_dataToApply.Clone() as S7TaskData;

                //Sort records by address
                m_data.Records.Sort();

                m_dataToApply = null;
            }
            m_error = "";
            
            if(m_data.Records.Count == 0)
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
                List<double> values = new List<double>(m_data.Records.Count);
                List<S7Operand> operands = new List<S7Operand>(m_data.Records.Count);
                foreach (S7TaskData.Record record in m_data.Records)
                {
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
                        operands.Add(new S7Operand(record.GetOperandName(), (int)record.DataType));
                        values.Add(f);
                    }
                    else if (!m_data.AllowErrors)
                    {
                        m_error = String.Format(Properties.Resources.BadEvaluate, record.GetOperandName());
                        return false;
                    }
                }

                if (operands.Count == 0)
                {
                    m_error = Properties.Resources.NoValidEntriesSpecified;
                    return false;
                }

                using (S7_writer.S7Connection conn = new S7_writer.S7Connection())
                {
                    conn.Connect(m_data.GetConnectionParameters());
                    conn.WriteOperands(operands, values, false); //m_data.AllowErrors);
                }
            }
            catch (Exception ex)
            {
                m_error = ex.Message;
                return false;      	
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

        private S7TaskData m_data;
        private S7TaskData m_dataToApply;
        public S7TaskWorker(S7TaskData data)
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
