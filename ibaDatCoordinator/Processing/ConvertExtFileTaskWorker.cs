using System;
using System.IO;
using iba.Data;

namespace iba.Processing
{
    internal class ConvertExtFileTaskWorker
    {
        private readonly ConvertExtFileTaskData m_task;
        private readonly ConfigurationWorker m_confWorker;
        private readonly StatusData m_sd;
        private IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer;

        public ConvertExtFileTaskWorker(ConfigurationWorker worker, ConvertExtFileTaskData task)
        {
            m_confWorker = worker;
            m_task = task;
            m_sd = worker.m_sd;
            m_ibaAnalyzer = worker.m_ibaAnalyzer;
        }

        public void DoWork(string filename)
        {
            if (!File.Exists(m_task.AnalysisFile))
            {
                string message = iba.Properties.Resources.AnalysisFileNotFound + m_task.AnalysisFile;
                m_confWorker.Log(Logging.Level.Exception, message, filename, m_task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return;
            }

            try
            {
                using IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_task.MonitorData);

                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.RUNNING;
                }

                mon.Execute(() => { m_ibaAnalyzer.OpenAnalysis(m_task.AnalysisFile); });
                m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logExtractStarted, filename, m_task);

                string outFile =filename;
                
                if (outFile == null) return;


                if (m_task.OverwriteFiles && m_task.UsesQuota && File.Exists(outFile))
                    m_confWorker.m_quotaCleanups[m_task.Guid].RemoveFile(outFile);

                string outFile2 = Path.Combine(m_task.DestinationMap, Path.GetFileName(Path.ChangeExtension(filename, "dat")));

                mon.Execute(() => { m_ibaAnalyzer.Extract(1, outFile2); });

                m_confWorker.m_outPutFilesPrevTask = new string[] { outFile };

                if (m_task.UsesQuota)
                {
                    m_confWorker.m_quotaCleanups[m_task.Guid].AddFile(m_confWorker.m_outPutFilesPrevTask[0]);
                }

                mon.Execute(() => m_ibaAnalyzer.CloseDataFile(0));
                
                if (m_confWorker.RunningConfiguration.DeleteExtFile)
                {                    
                    if (m_confWorker.RunningConfiguration.FileFormat == ConfigurationData.FileFormatEnum.COMTRADE)
                    {
                        File.Delete(filename);
                        File.Delete(Path.ChangeExtension(filename, "cfg"));
                    }
                    else
                    {
                        File.Delete(filename);
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(m_confWorker.RunningConfiguration.ProcessedFileTargedDirectory))
                    {
                        if (m_confWorker.RunningConfiguration.FileFormat == ConfigurationData.FileFormatEnum.COMTRADE)
                        {
                            var cfgFilename = Path.ChangeExtension(filename, "cfg");

                            var pathToCopy = Path.Combine(Path.Combine(m_confWorker.RunningConfiguration.ProcessedFileTargedDirectory, Path.GetFileName(filename)));

                            var cfgPathToCopy = Path.ChangeExtension(pathToCopy, "cfg");

                            File.Copy(filename, pathToCopy, true);
                            File.Delete(filename);

                            File.Copy(cfgFilename, cfgPathToCopy, true);
                            File.Delete(cfgFilename);

                        }
                        else
                        {
                            var pathToCopy = Path.Combine(Path.Combine(m_confWorker.RunningConfiguration.ProcessedFileTargedDirectory, Path.GetFileName(filename)));
                            File.Copy(filename, pathToCopy, true);
                            File.Delete(filename);
                        }
                    }
                    
                }

                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                    m_sd.DatFileStates[filename].OutputFiles[m_task] =
                        m_confWorker != null && (m_confWorker.m_outPutFilesPrevTask == null)
                            ? null
                            : (m_confWorker.m_outPutFilesPrevTask[0]);
                }
                m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logExtractSuccess, filename, m_task);
            }
            catch (IbaAnalyzerExceedingTimeLimitException te)
            {
                m_confWorker.Log(Logging.Level.Exception, te.Message, filename, m_task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.TIMED_OUT;
                }
                m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                m_confWorker.Log(Logging.Level.Exception, me.Message, filename, m_task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.MEMORY_EXCEEDED;
                }
                m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (Exception ex)
            {
                m_confWorker.Log(Logging.Level.Exception, ex.Message + "   " + m_confWorker.IbaAnalyzerErrorMessage(), filename, m_task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
            finally
            {
                if (m_ibaAnalyzer != null)
                {
                    try
                    {
                        m_ibaAnalyzer.CloseAnalysis();
                    }
                    catch
                    {
                        m_confWorker.Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, filename, m_task);
                        m_confWorker.RestartIbaAnalyzer();
                    }
                }
            }
        }
    }
}

