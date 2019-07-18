using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using iba.Data;
using iba.ibaFilesLiteDotNet;
using System.IO;
using iba.Utility;

namespace iba.Processing
{
    public interface ISplitterTaskProgress
    {
        void Update(string message, int progress);
        bool Aborted
        {
            get;
        }
    }


    class SplitterTaskWorker
    {

        private string splitExpression = "[ibaDatCoSplitterTaskTemp]";

        public SplitterTaskWorker(SplitterTaskData data)
        {
            m_task = data;
            splitExpression = m_task.Expression;
        }

        public SplitterTaskWorker(ConfigurationWorker worker, SplitterTaskData task)
        {
            m_confWorker = worker;
            m_task = task;
            m_sd = worker.m_sd;
            splitExpression = m_task.Expression;
            m_ibaAnalyzer = worker.m_ibaAnalyzer;
        }

        private ConfigurationWorker m_confWorker;
        private SplitterTaskData m_task;
        private StatusData m_sd;
        private IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer;
        private List<double> points;

        private void LogError(Logging.Level level, string message, string filename)
        {
            if (m_confWorker != null)
            {
                m_confWorker.Log(level, message,filename,m_task);
            }
            else
            {
                MessageBox.Show(message, "ibaDatCoordinator", MessageBoxButtons.OK, level == Logging.Level.Exception ? MessageBoxIcon.Error : MessageBoxIcon.Warning);
            }
        }

        private DateTime? m_startTime;

        public DateTime GetStartTime()
        {
            bool deleteIt = false;
            if (!m_startTime.HasValue)
            {
                try
                {
                    if (m_ibaAnalyzer == null)
                    {
                        m_ibaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                        deleteIt = true;
                    }
                    DateTime dt = new DateTime();
                    int microsec = 0;
                    using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_task.MonitorData))
                    {
                        m_ibaAnalyzer.GetStartTime(ref dt, ref microsec);
                        dt = dt.AddTicks(microsec * 10);
                    }
                    m_startTime = dt;
                }
                finally
                {
                    if (deleteIt)
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(m_ibaAnalyzer);
                        m_ibaAnalyzer = null;
                    }
                }
            }
            return m_startTime.Value;
        }

        private List<string> generatedFiles;
        public List<string> GeneratedFiles
        {
            get { return generatedFiles; }
            set { generatedFiles = value; }
        }

        public bool Split(string fileName = null, string outputFolder = null, ISplitterTaskProgress progress =null)
        {
            if (string.IsNullOrEmpty(fileName)) fileName = m_task.TestDatFile;
            if (string.IsNullOrEmpty(outputFolder)) outputFolder = m_task.DestinationMapUNC;
            if (points == null)
                GetPoints(fileName);
            if (progress == null && m_confWorker != null)
                progress = new ConfigurationStopListener(m_confWorker);
            if (points == null) return false; //failed and logged
            generatedFiles = new List<String>();
            
            try
            {
                string fileNameWithoutPath = Path.GetFileName(fileName);
                using (IbaFileSplitter splitter = new IbaFileSplitter())
                {
                    splitter.Open(fileName, m_task.ParentConfigurationData.FileEncryptionPassword);
                    int size = points.Count / 2;
                    for (int i = 0; i < size; i++)
                    {
                        string newFile = Path.Combine(outputFolder, GetName(i, fileNameWithoutPath));
                        if (progress != null)
                        {
                            if (progress.Aborted) break;
                            progress.Update(newFile, i);
                        }
                        splitter.Split(newFile, points[2 * i], points[2 * i + 1]);
                        generatedFiles.Add(newFile);
                    }
                    splitter.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                LogError(Logging.Level.Exception, ex.Message, fileName);
                return false;
            }
        }

        private IbaAnalyzerMonitor m_mon;


        public class CurrentInfo
        {
            public double start;
            public double stop;
            public bool valid;
        }

        private CurrentInfo m_currentPoints = new CurrentInfo();
        internal CurrentInfo CurrentPoints
        {
            get { return m_currentPoints; }
            set { m_currentPoints = value; }
        }
        
        public bool AddPairPoints(int i, ref List<double> result)
        {
            String expression;
            if (m_task.EdgeConditionType == SplitterTaskData.EdgeConditionTypeEnum.RISINGTORISING)
            {
                expression = string.Format("XFirst({0},{1},1)", splitExpression, i);
                double res = double.NaN;
                m_mon.Execute(delegate() { res = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                if (double.IsNaN(res) || res < 0 || res > 1.0e35)
                {
                    expression = string.Format("XSize({0})", splitExpression);
                    res = double.NaN;
                    m_mon.Execute(delegate() { res = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                    if (!double.IsNaN(res))
                    {
                        if (i == 0)
                            result.Add(0.0);
                        result.Add(res);
                        m_currentPoints.start = result[2 * i];
                        m_currentPoints.stop = result[2 * i+1];
                        m_currentPoints.valid = true;

                    }
                    else
                    {
                        m_currentPoints.valid = false;
                    }
                    return false;
                }
                else if (res != 0.0f)
                {
                    //System.Diagnostics.Debug.WriteLine(res);
                    if (i == 0)
                        result.Add(0.0);
                    result.Add(res);
                    result.Add(res); //twice, is second startpoint
                    m_currentPoints.start = result[2 * i];
                    m_currentPoints.stop = result[2 * i + 1];
                    m_currentPoints.valid = true;
                }
            }
            else
            {
                expression = string.Format("XFirst({0},{1})", splitExpression, i);
                double res1 = double.NaN;
                m_mon.Execute(delegate() { res1 = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                if (double.IsNaN(res1) || res1 < 0 || res1 > 1.0e35)
                {
                    m_currentPoints.valid = false;
                    return false;
                }
                expression = string.Format("XFirst(NOT({0}),{1},1)", splitExpression, i);
                double res2 = double.NaN;
                m_mon.Execute(delegate() { res2 = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                if (double.IsNaN(res2) || res2 < 0 || res2 > 1.0e35) //find end...
                {
                    expression = string.Format("XSize({0})", splitExpression);
                    res2 = double.NaN;
                    m_mon.Execute(delegate() { res2 = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                }
                if (res1 < res2 && !(double.IsNaN(res2) || res2 < 0 || res2 > 1.0e35))
                {
                    result.Add(res1);
                    result.Add(res2);
                    m_currentPoints.start = result[2 * i];
                    m_currentPoints.stop = result[2 * i + 1];
                    m_currentPoints.valid = true;
                }
                else
                {
                    m_currentPoints.valid = false;
                    return false;
                }
            }
            return true;
        }

        public List<double> GetPoints(string filename = null, ISplitterTaskProgress progress =null)
        {
            if (points != null) return points;
            if (filename==null) filename = m_task.TestDatFile;
            string pass = m_task.ParentConfigurationData.FileEncryptionPassword;
            if (progress == null && m_confWorker != null)
                progress = new ConfigurationStopListener(m_confWorker);
            try
            {
                List<double> result = new List<double>();
                if (m_ibaAnalyzer == null)
                {
                    m_ibaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                }
                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_task.MonitorData))
                {
                    m_mon = mon;
                    if (m_confWorker == null) //in test dialog
                    {
                        using (WaitCursor wait = new WaitCursor())
                        {
                            LoadStuff(filename, pass, ref result);
                        }
                    }
                    else
                        LoadStuff(filename, pass, ref result);

                    for (int i = 0; progress == null || !progress.Aborted; i++)
                    {
                        bool res = AddPairPoints(i, ref result);
                        if (progress != null && CurrentPoints.valid)
                            progress.Update(GetName(i, filename), i);
                        if (!res) break;
                    }
                    m_ibaAnalyzer.CloseAnalysis();
                    m_ibaAnalyzer.CloseDataFile(0);
                }
                if (result.Count < 2)
                {
                    LogError(Logging.Level.Warning,iba.Properties.Resources.splitterWarningNoResults,filename);
                    return null;
                }
                return points=result;
            }
            catch (IbaAnalyzerExceedingTimeLimitException te)
            {
                LogError(Logging.Level.Exception, te.Message, filename);
                if (m_sd != null)
                {
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.TIMED_OUT;
                    }
                }
                if (m_confWorker != null) m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                LogError(Logging.Level.Exception, me.Message, filename);
                if (m_sd != null)
                {
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.MEMORY_EXCEEDED;
                    }
                }
                if (m_confWorker != null) m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (Exception ex)
            {
                if (m_confWorker != null)
                {
                    LogError(Logging.Level.Exception, ex.Message + "   " + m_confWorker.IbaAnalyzerErrorMessage(), filename);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                }
                else
                {
                    string error = iba.Properties.Resources.IbaAnalyzerUndeterminedError;
                    if (m_ibaAnalyzer != null)
                    {
                        try
                        {
                            error = m_ibaAnalyzer.GetLastError();
                        }
                        catch
                        {
                            error = iba.Properties.Resources.IbaAnalyzerUndeterminedError;
                        }
                    }
                    LogError(Logging.Level.Exception, ex.Message + "   " + error, filename);
                }
            }
            finally
            {
                if (m_confWorker != null && m_ibaAnalyzer != null)
                {
                    try
                    {
                        m_ibaAnalyzer.CloseAnalysis();
                    }
                    catch
                    {
                        LogError(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, filename);
                        m_confWorker.RestartIbaAnalyzer();
                    }
                }
                else if (m_ibaAnalyzer != null)
                {
                    try
                    {
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(m_ibaAnalyzer);
                    }
                    catch
                    {
                    }
                }
                m_ibaAnalyzer = null;
            }
            return null;
        }

        private void LoadStuff(string filename, string pass, ref List<double> result)
        {
            if (!String.IsNullOrEmpty(pass))
                m_ibaAnalyzer.SetFilePassword("",pass);
            m_ibaAnalyzer.OpenDataFile(0, filename);
            GetStartTime();
            if (!string.IsNullOrEmpty(m_task.AnalysisFile))
                m_mon.Execute(delegate() { m_ibaAnalyzer.OpenAnalysis(m_task.AnalysisFile); });
        }

        public string GetName(int i, string p)
        {
            return p.Replace(".", "_" + i.ToString() + ".");
        }
    }

    public class ConfigurationStopListener : ISplitterTaskProgress
    {
        private ConfigurationWorker m_worker;
        public bool Aborted
        {
            get
            {
                return m_worker.Stop;
            }
        }

        internal ConfigurationStopListener(ConfigurationWorker w)
        {
            m_worker = w;
        }

        public void Update(string filename, int progress)
        {
            //do nothing;
        }
    }
}
