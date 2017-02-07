using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using iba.Data;

namespace iba.Processing
{
    class SplitterTaskWorker
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


        public List<double> GetPoints(string filename)
        {
            try
            {
                List<double> result = new List<double>();
                if (m_ibaAnalyzer == null)
                {
                    m_ibaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                }
                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_task.MonitorData))
                {
                    if (m_task.AnalysisFile != null)
                        mon.Execute(delegate() { m_ibaAnalyzer.OpenAnalysis(m_task.AnalysisFile); });

                    for (int i = 0; true;i++)
                    {
                        String expression;
                        if (m_task.TriggerType == SplitterTaskData.TriggerTypeEnum.RISINGTORISING)
                        {
                            expression = string.Format("XFirst({0},{1})", m_task.Expression, i);
                            double res = double.NaN;
                            mon.Execute(delegate() { res = m_ibaAnalyzer.EvaluateDouble(expression, 0); });
                            if (double.IsNaN(res) || res < 0 || res > 1.0e35)
                                break;
                            result.Add(res);
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
                }
                if (result.Count == 0)
                {
                    LogError(Logging.Level.Warning,iba.Properties.Resources.splitterWarningNoResults,filename);
                    return null;
                }
                return result;
            }
            catch (IbaAnalyzerExceedingTimeLimitException te)
            {
                LogError(Logging.Level.Exception, te.Message, filename);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.TIMED_OUT;
                }
                if (m_confWorker != null) m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                LogError(Logging.Level.Exception, me.Message, filename);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.MEMORY_EXCEEDED;
                }
                m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (Exception ex)
            {
                LogError(Logging.Level.Exception, ex.Message + "   " + m_confWorker.IbaAnalyzerErrorMessage(), filename);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_FAILURE;
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
            }
            return null;
        }

    }
}
