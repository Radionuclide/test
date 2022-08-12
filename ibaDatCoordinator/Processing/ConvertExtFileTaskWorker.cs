using System;
using System.IO;
using iba.Data;
using iba.Utility;

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

        public void DoWork(string filename, ConfigurationData data)
        {
            if (!String.IsNullOrEmpty(m_task.AnalysisFile) && !File.Exists(m_task.AnalysisFile))
            {
                SetSate(filename, DatFileStatus.State.COMPLETED_FAILURE, Properties.Resources.AnalysisFileNotFound + m_task.AnalysisFile);
                return;
            }

            if (string.IsNullOrEmpty(data.ExternalFileJobData.ProcessedFileTargedDirectory) && data.ExternalFileJobData.MoveExtFile)
            {
                SetSate(filename, DatFileStatus.State.COMPLETED_FAILURE, Properties.Resources.TargetDirectoryNotSpecified);
                return;
            }

            if (m_confWorker.RunningConfiguration.SubDirs)
            {
                if (CheckIfTargetIsSubdirectory(m_confWorker.RunningConfiguration.DatDirectory, m_confWorker.RunningConfiguration.ExternalFileJobData.ProcessedFileTargedDirectory))
                {
                    SetSate(filename, DatFileStatus.State.COMPLETED_FAILURE, Properties.Resources.SubdirectoryNotAllowed);
                    return;
                }
            }

            try
            {
                using IbaAnalyzerMonitor mon = new(m_ibaAnalyzer, m_task.MonitorData);

                SetSate(filename, DatFileStatus.State.RUNNING);

                m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logConvertStarded, filename, m_task);

                const string extension = ".dat";
                var outFile = GetOutputFile(filename, extension);

                if (!m_task.OverwriteFiles)
                    outFile = DatCoordinatorHostImpl.Host.FindSuitableFileName(outFile);

                if (m_task.UsesQuota)
                {
                    m_confWorker.CleanupWithQuota(outFile, m_task, extension);
                }

                if (m_task.DoDirCleanupNow)
                {
                    m_confWorker.CleanupDirs(outFile, m_task, extension);
                }

                if (data.ExternalFileJobData.FileFormat == ExternalFileJobData.FileFormatEnum.TEXTFILE)
                {
                    string pdoForTextFile = data.ExternalFileJobData.PdoFile;

                    if (string.IsNullOrEmpty(pdoForTextFile))
                    {
                        mon.Execute(() => { m_ibaAnalyzer.OpenDataFile(0, filename); });
                    }
                    else
                    {
                        mon.Execute(() => { m_ibaAnalyzer.OpenAnalysis(pdoForTextFile); });
                    }
                }

                if (string.IsNullOrEmpty(m_task.AnalysisFile))
                {
                    mon.Execute(() => { m_ibaAnalyzer.ExtractAll(1, outFile); });
                }
                else
                {
                    mon.Execute(() => { m_ibaAnalyzer.OpenAnalysis(m_task.AnalysisFile); });

                    mon.Execute(() => { m_ibaAnalyzer.Extract(1, outFile); });
                }
                mon.Execute(() => m_ibaAnalyzer.CloseDataFile(0));

                if (m_task.UsesQuota)
                {
                    m_confWorker.m_quotaCleanups[m_task.Guid].AddFile(outFile);
                }

                DeleteOrMoveSourceFile(filename);

                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                }
                m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logConvertSuccess, filename, m_task);
            }
            catch (IbaAnalyzerExceedingTimeLimitException te)
            {
                SetSate(filename,DatFileStatus.State.TIMED_OUT, te.Message);
                m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                SetSate(filename,DatFileStatus.State.MEMORY_EXCEEDED, me.Message);
                m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (Exception ex)
            {
                SetSate(filename, DatFileStatus.State.COMPLETED_FAILURE, ex.Message + "   " + m_confWorker.IbaAnalyzerErrorMessage());
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

        private string GetOutputFile(string filename, string extension)
        {
            string outfile = m_confWorker.GetOutputFileName(m_task, filename);
            string dir = m_confWorker.GetOutputDirectoryName(filename, m_task);

            return Path.Combine(dir, outfile + extension);
        }

        private void DeleteOrMoveSourceFile(string filename)
        {
            if (m_confWorker.RunningConfiguration.ExternalFileJobData.DeleteExtFile)
            {
                if (m_confWorker.RunningConfiguration.ExternalFileJobData.FileFormat == ExternalFileJobData.FileFormatEnum.COMTRADE)
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
                if (!string.IsNullOrEmpty(m_confWorker.RunningConfiguration.ExternalFileJobData.ProcessedFileTargedDirectory))
                {
                    if (m_confWorker.RunningConfiguration.ExternalFileJobData.FileFormat == ExternalFileJobData.FileFormatEnum.COMTRADE)
                    {
                        var cfgFilename = Path.ChangeExtension(filename, "cfg");

                        var pathToCopy = Path.Combine(Path.Combine(m_confWorker.RunningConfiguration.ExternalFileJobData.ProcessedFileTargedDirectory, Path.GetFileName(filename)));

                        var cfgPathToCopy = Path.ChangeExtension(pathToCopy, "cfg");

                        File.Copy(filename, pathToCopy, true);
                        File.Delete(filename);

                        File.Copy(cfgFilename, cfgPathToCopy, true);
                        File.Delete(cfgFilename);

                    }
                    else
                    {
                        var pathToCopy = Path.Combine(Path.Combine(m_confWorker.RunningConfiguration.ExternalFileJobData.ProcessedFileTargedDirectory, Path.GetFileName(filename)));
                        File.Copy(filename, pathToCopy, true);
                        File.Delete(filename);
                    }
                }

            }
        }

        private void SetSate(string filename, DatFileStatus.State state, string message = null, Logging.Level loggingLevel = null)
        {
            lock (m_sd.DatFileStates)
            {
                m_sd.DatFileStates[filename].States[m_task] = state;
            }

            if (message != null)
            {
                loggingLevel ??= Logging.Level.Exception;
                m_confWorker.Log(loggingLevel, message, filename, m_task);
            }
        }

        private bool CheckIfTargetIsSubdirectory(string source, string target)
        {
            var sourceDirectory = new Uri(source + "\\");
            var processedFileSubdirectory = new Uri(target + "\\");

            return sourceDirectory.IsBaseOf(processedFileSubdirectory);
        }
    }
}
