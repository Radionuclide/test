using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using iba.Data;
using ibaFilesLiteLib;
using System.IO;

namespace iba.Processing
{
    interface SplitterTaskProgress
    {
        void Update(string message, int progress);
        bool Aborted
        {
            get;
        }
    }


    public class SplitterTaskWorker
    {
        public SplitterTaskWorker(SplitterTaskData data)
        {
            m_task = data;
            //rest zero
        }

        public SplitterTaskWorker(ConfigurationWorker worker, SplitterTaskData task)
        {
            m_confWorker = worker;
            m_task = task;
            m_sd = worker.m_sd;
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
                        dt.AddTicks(microsec * 10);
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

        public void Split(string fileName = null, string outputFolder = null, SplitterTaskProgress progress =null)
        {
            if (string.IsNullOrEmpty(fileName)) fileName = m_task.TestDatFile;
            if (string.IsNullOrEmpty(outputFolder)) outputFolder = m_task.DestinationMapUNC;
            if (points == null)
                GetPoints(fileName);
            if (points == null) return; //failed and logged
            generatedFiles = new List<String>();
            IbaFileSplitter splitter = null;
            try
            {
                string fileNameWithoutPath = Path.GetFileName(fileName);
                splitter = new IbaFileSplitterClass();
                splitter.Open(fileName);
                int size = points.Count / 2;
                for (int i = 0; i < size; i++)
                {
                    string newFile = Path.Combine(outputFolder,GetName(i, fileNameWithoutPath));
                    if (progress != null)
                    {
                        if (progress.Aborted) break;
                        progress.Update(newFile, (int)(100.0 * i / size));
                    }
                    splitter.Split(newFile, points[2 * i], points[2 * i + 1]);
                    generatedFiles.Add(newFile);
                }
                splitter.Close();
            }
            catch (Exception ex)
            {
                LogError(Logging.Level.Exception, ex.Message, fileName);
            }
            finally
            {
                if (splitter != null) System.Runtime.InteropServices.Marshal.ReleaseComObject(splitter);
            }
        }

        public List<double> GetPoints(string filename = null)
        {
            if (filename==null) filename = m_task.TestDatFile;
            try
            {
                List<double> result = new List<double>();
                if (m_ibaAnalyzer == null)
                {
                    m_ibaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                }
                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_task.MonitorData))
                {
                    m_ibaAnalyzer.OpenDataFile(0, filename);
                    GetStartTime();
                    if (!string.IsNullOrEmpty(m_task.AnalysisFile))
                        mon.Execute(delegate() { m_ibaAnalyzer.OpenAnalysis(m_task.AnalysisFile); });
                    if (m_task.EdgeConditionType == SplitterTaskData.EdgeConditionTypeEnum.RISINGTORISING)
                        result.Add(0.0);
                    for (int i = 0; true;i++)
                    {
                        String expression;
                        if (m_task.EdgeConditionType == SplitterTaskData.EdgeConditionTypeEnum.RISINGTORISING)
                        {
                            expression = string.Format("XFirst({0},{1})", m_task.Expression, i);
                            double res = double.NaN;
                            mon.Execute(delegate() { res = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                            if (double.IsNaN(res) || res < 0 || res > 1.0e35)
                            {
                                expression = string.Format("XSize({0})", m_task.Expression);
                                res = double.NaN;
                                mon.Execute(delegate() { res = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                                result.Add(res);
                                break;
                            }
                            else if (res == 0.0f) continue; //is initial point
                            result.Add(res);
                            result.Add(res); //twice
                        }
                        else
                        {
                            expression = string.Format("XFirst({0},{1})", m_task.Expression, i);
                            double res1 = double.NaN;
                            mon.Execute(delegate() { res1 = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                            if (double.IsNaN(res1) || res1 < 0 || res1 > 1.0e35)
                                break;
                            expression = string.Format("XFirst(NOT({0}),{1},1)", m_task.Expression, i);
                            double res2 = double.NaN;
                            mon.Execute(delegate() { res2 = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                            if (double.IsNaN(res2) || res2 < 0 || res2 > 1.0e35) //find end...
                            {
                                expression = string.Format("XSize({0})", m_task.Expression);
                                res2 = double.NaN;
                                mon.Execute(delegate() { res2 = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                            }
                            if (res1 < res2 && !(double.IsNaN(res2) || res2 < 0 || res2 > 1.0e35))
                            {
                                result.Add(res1);
                                result.Add(res2);
                            }
                            else break;
                        }
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

        public string GetName(int i, string p)
        {
            return p.Replace(".", "_" + i.ToString() + ".");
        }
    }
}
