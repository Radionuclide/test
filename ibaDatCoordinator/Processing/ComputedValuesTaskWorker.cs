using iba.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Processing
{
    class ComputedValuesTaskWorker
    {
        private ConfigurationWorker m_confWorker;
        private ComputedValuesTaskData m_task;
        private StatusData m_sd;
        private IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer;
        internal ComputedValuesTaskWorker(ConfigurationWorker worker, ComputedValuesTaskData task)
        {
            m_confWorker = worker;
            m_task = task;
            m_sd = worker.m_sd;
            m_ibaAnalyzer = worker.m_ibaAnalyzer;
        }

        internal void DoWork(string filename, ExtMonData.TargetServer targetServer)
        {
            bool bUseAnalysis = !String.IsNullOrEmpty(m_task.AnalysisFile);
            if (bUseAnalysis && !File.Exists(m_task.AnalysisFile))
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
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.RUNNING;
                }
                m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logOPCUAWriterStarted, filename, m_task);

                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_task.MonitorData))
                {
                    if (bUseAnalysis)
                        mon.Execute(delegate () { m_ibaAnalyzer.OpenAnalysis(m_task.AnalysisFile); });
                    mon.Execute(delegate ()
                    {
                        m_task.EvaluateValues(filename, m_ibaAnalyzer);

                        ExtMonData od = ExtMonData.Instance;
                        List<ExtMonData.ExtMonNode> FolderChilren;
                        if (targetServer == ExtMonData.TargetServer.OPCUA)
                            FolderChilren = od.FolderComputedValuesOpcua.Children;
                        else
                            FolderChilren = od.FolderComputedValuesSnmp.Children;
                        foreach (var job in FolderChilren)
                        {
                            if (job is ExtMonData.ExtMonFolder jobFolder)
                                if (jobFolder.UaBrowseName == $@"Job{{{m_task.ParentConfigurationData.Guid}}}")
                                    foreach (var task in jobFolder.Children)
                                        if (task is ExtMonData.ComputedValuesInfo info)
                                            if (info.DataId == m_task.Guid)
                                            {
                                                info.UpdateValues(m_task);
                                                return;
                                            }
                        }
                    });

                    m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logComputedValuesUpdated, filename, m_task);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                    }
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
            catch
            {
                m_confWorker.Log(Logging.Level.Exception, m_confWorker.IbaAnalyzerErrorMessage(), filename, m_task);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_FAILURE;
                }
            }
            finally
            {
                if (m_ibaAnalyzer != null && bUseAnalysis)
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

            if (targetServer == ExtMonData.TargetServer.OPCUA &&!(TaskManager.Manager.OpcUaData?.Enabled ?? false))
            {
                m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_FAILURE;
                m_confWorker.Log(Logging.Level.Warning, iba.Properties.Resources.logOPCUAServerDisabled, filename, m_task);
            }
            else if (targetServer == ExtMonData.TargetServer.SNMP && !(TaskManager.Manager.SnmpData?.Enabled ?? false))
            {
                m_sd.DatFileStates[filename].States[m_task] = DatFileStatus.State.COMPLETED_FAILURE;
                m_confWorker.Log(Logging.Level.Warning, iba.Properties.Resources.logSNMPServerDisabled, filename, m_task);
            }
        }
    }
}
