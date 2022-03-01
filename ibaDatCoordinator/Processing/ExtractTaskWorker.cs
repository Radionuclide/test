﻿using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using iba.Data;
using iba.Utility;

namespace iba.Processing
{
    internal class ExtractTaskWorker
    {
        private ConfigurationWorker m_confWorker;
        private ExtractData m_task;
        private StatusData m_sd;
        private IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer;

        internal ExtractTaskWorker(ConfigurationWorker worker, ExtractData task)
        {
            m_confWorker = worker;
            m_task = task;
            m_sd = worker.m_sd;
            m_ibaAnalyzer = worker.m_ibaAnalyzer;
        }

        internal void DoWork(string filename)
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
			List<string> toDelete = new List<string>();
			try
            {
                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_task.MonitorData))
                {
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.RUNNING;
                    }
                    mon.Execute(delegate() { m_ibaAnalyzer.OpenAnalysis(m_task.AnalysisFile); });
                    m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logExtractStarted, filename, m_task);
                    if (m_task.ExtractToFile)
                    {
                        string outFile = GetExtractFileName(filename);
                        if (outFile == null) return;
                        if (m_task.OverwriteFiles && m_task.UsesQuota && File.Exists(outFile))
                            m_confWorker.m_quotaCleanups[m_task.Guid].RemoveFile(outFile);
						toDelete.Add(outFile);

						string outFile2 = "";
                        if (!IsExternalVideoExport(m_task))
                        {
                            //fix cleanup for TDMS in case of overwrite
                            string ext2 = GetSecondaryExtractExtension();
                            if (!string.IsNullOrEmpty(ext2))
                                outFile2 = outFile.Replace(GetPrimeExtractExtension(), ext2);
                            if (m_task.OverwriteFiles && m_task.UsesQuota && !string.IsNullOrEmpty(outFile2) && File.Exists(outFile2))
                                m_confWorker.m_quotaCleanups[m_task.Guid].RemoveFile(outFile2);
							toDelete.Add(outFile2);
						}
                        else if (m_task.OverwriteFiles && m_task.UsesQuota )
                        { //fix cleanup for videos
                            try
                            {
                                string outdir = Path.GetDirectoryName(outFile);
                                string[] files = Directory.GetFiles(outdir, "*.mp4");
                                foreach (string file in files)
                                {
                                    m_confWorker.m_quotaCleanups[m_task.Guid].AddFile(file);
                                }
                            }
                            catch 
                            {
                            }
                        }

                        mon.Execute(delegate() { m_ibaAnalyzer.Extract(1, outFile); });
                        string ext = GetSecondaryExtractExtension(m_task);
                        if(!string.IsNullOrEmpty(outFile2) && File.Exists(outFile2))
                        {
                            m_confWorker.m_outPutFilesPrevTask = new string[] { outFile, outFile2 };
                        }
                        else
                            m_confWorker.m_outPutFilesPrevTask = new string[] { outFile };
                        if (m_task.UsesQuota)
                        {
                            m_confWorker.m_quotaCleanups[m_task.Guid].AddFile(m_confWorker.m_outPutFilesPrevTask[0]);
                            if (IsExternalVideoExport(m_task))
                            {
                                try
                                {
                                    string outdir = Path.GetDirectoryName(outFile);
                                    string[] files = Directory.GetFiles(outdir, "*.mp4");
                                    foreach (string file in files)
                                    {
                                        try
                                        {
                                            m_confWorker.m_quotaCleanups[m_task.Guid].RemoveFile(file);
                                            File.Delete(file);
                                        }
                                        catch
                                        {
                                        }
                                    }
                                }
                                catch 
                                {
                                }
                            }
                            else if (!string.IsNullOrEmpty(outFile2)) //TDMS
                                m_confWorker.m_quotaCleanups[m_task.Guid].AddFile(outFile2);
                        }
                    }
                    else
                    {
                        mon.Execute(delegate() { m_ibaAnalyzer.Extract(0, String.Empty); });
                    }
                    //code on succes
                    lock (m_sd.DatFileStates)
                    {
						toDelete.Clear();
                        m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                        m_sd.DatFileStates[filename].OutputFiles[m_task] = (m_confWorker.m_outPutFilesPrevTask == null)? null : (m_confWorker.m_outPutFilesPrevTask[0]);
                    }
                    m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logExtractSuccess, filename, m_task);
                }
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
			foreach (string file in toDelete)
			{
				if (String.IsNullOrEmpty(file))
					continue;
				try
				{
					if (File.Exists(file))
					{
						File.Delete(file);
						String msg = String.Format(iba.Properties.Resources.IbaAnalyzerDeletingRemFile, file);
						m_confWorker.Log(iba.Logging.Level.Warning, msg, filename, m_task);
					}
					string dir = Path.GetDirectoryName(file);
					while (Directory.Exists(dir) && dir != m_task.DestinationMapUNC && Directory.GetFiles(dir).Length == 0)
					{
						string dir2 = System.IO.Directory.GetParent(dir).FullName;
						Directory.Delete(dir);
						dir = dir2;
					}
				}
				catch
				{
				}
			}
        }

        private string GetExtractFileName(string filename)
        {
            string ext = GetPrimeExtractExtension();
            string bext = GetBothExtractExtensions();
            try
            {
                if (m_task.UsesQuota)
                    m_confWorker.CleanupWithQuota(filename, m_task, bext);
            }
            catch
            {
            }

            string actualFileName = m_confWorker.GetOutputFileName(m_task, filename);

            string dir = m_confWorker.GetOutputDirectoryName(filename, m_task);
            if (dir == null)
                return null;

            string mp4outputdir = "";

            if (IsExternalVideoExport(m_task))
            {
                mp4outputdir = Path.Combine(dir, actualFileName);
                if (!m_task.OverwriteFiles)
                {
                    for (int index = 0; Directory.Exists(mp4outputdir); index++)
                    {
                        string indexstr = index.ToString("d2");
                        mp4outputdir = Path.Combine(dir, actualFileName + '_' + indexstr);
                    }
                }
                dir = mp4outputdir;
            }
            if (!SharesHandler.Handler.TestTargetDirAvailable(m_task)) //will also try reconnect
            {
                m_confWorker.Log(Logging.Level.Exception, iba.Properties.Resources.cannotAccessTargetSystem + " " + dir, filename, m_task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return null;
            }
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch
                {
                    m_confWorker.Log(Logging.Level.Exception, iba.Properties.Resources.logCreateDirectoryFailed + ": " + dir, filename, m_task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_FAILURE;
                    }
                    return null;
                }
                //new directory created, do directory cleanup if that is the setting
                if (m_task.Subfolder != TaskDataUNC.SubfolderChoice.NONE && m_task.OutputLimitChoice == TaskDataUNC.OutputLimitChoiceEnum.LimitDirectories)
                    m_task.DoDirCleanupNow = true;
            }
            if (m_task.DoDirCleanupNow)
            {
                try
                {
                    if (IsExternalVideoExport(m_task))
                        m_confWorker.CleanupDirsMultipleOutputFiles(filename, m_task, bext);
                    else
                        m_confWorker.CleanupDirs(filename, m_task, bext);
                }
                catch
                {
                }
                m_task.DoDirCleanupNow = false;
            }
            string arg = Path.Combine(dir, actualFileName + ext);
            if (m_task.OverwriteFiles)
                return arg;
            else
                return DatCoordinatorHostImpl.Host.FindSuitableFileName(arg);
        }

      
        internal static string GetBothExtractExtensions(ExtractData task)
        {
            string secondary = GetSecondaryExtractExtension(task);
            if (string.IsNullOrEmpty(secondary)) return GetPrimeExtractExtension(task);
            return GetPrimeExtractExtension(task) + ",*" + secondary;
        }

        internal string GetBothExtractExtensions()
        {
            return GetBothExtractExtensions(m_task);
        }

        private static string GetPrimeExtractExtension(ExtractData task)
        {
            switch (task.FileType)
            {
                case ExtractData.ExtractFileType.TEXT: return ".txt";
                case ExtractData.ExtractFileType.TDMS: return ".tdms";
                case ExtractData.ExtractFileType.PARQUET: return ".parquet";
				case ExtractData.ExtractFileType.MATLAB: return ".mat";
				default: return ".dat";
            }
        }

        private string GetPrimeExtractExtension()
        {
            return GetPrimeExtractExtension(m_task);
        }

        internal static string GetSecondaryExtractExtension(ExtractData task)
        {
            switch (task.FileType)
            {
                case ExtractData.ExtractFileType.COMTRADE: return ".cfg";
                case ExtractData.ExtractFileType.TDMS: return ".tdms_index";
                case ExtractData.ExtractFileType.BINARY:
                    return IsExternalVideoExport(task)? ".mp4" : "";
                default: return "";
            }
        }

        private string GetSecondaryExtractExtension()
        {
            return GetSecondaryExtractExtension(m_task);
        }

        private static int m_exportFileTypeAsInt = -1;



        private static bool IsExternalVideoExport(ExtractData task, IbaAnalyzer.IbaAnalyzer ibaAnalyzer = null)
        {
            if (task.m_bExternalVideoResultIsCached)
                return task.m_bExternalVideoCacheResult;
            m_exportFileTypeAsInt = -1;
            bool res = false;
            bool dispose = false;
            try
            {
                if (ibaAnalyzer == null)
                {
                    ibaAnalyzer = new IbaAnalyzer.IbaAnalysis();
                    dispose = true;
                }
                ibaAnalyzer.OpenAnalysis(task.AnalysisFile);
                string parameters = ibaAnalyzer.GetFileExtractParameters();
                if (!string.IsNullOrEmpty(parameters))
                {
                    res = (parameters.IndexOf("Type:0") > -1 && parameters.IndexOf("Video:1") > -1 && parameters.IndexOf("VideoExternal:1") > -1);
                    int.TryParse(parameters.Substring(parameters.IndexOf("Type:") + 5, 1), out m_exportFileTypeAsInt);
                }
            }
            catch (System.Exception) //older ibaAnalyzer version or non existing .pdo file
            {
            }
            finally
            {
                try
                {
                    if (dispose)
                        System.Runtime.InteropServices.Marshal.ReleaseComObject(ibaAnalyzer);
                }
                catch { }
            }
            task.m_bExternalVideoCacheResult = res;
            task.m_bExternalVideoResultIsCached = true;
            return res;
        }

        static public int FileTypeAsInt (ExtractData task, IbaAnalyzer.IbaAnalyzer ibaAnalyzer = null)
        {
            if (!task.m_bExternalVideoResultIsCached)
                IsExternalVideoExport(task, ibaAnalyzer);
            return m_exportFileTypeAsInt;
        }
    }
}
