using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iba.Data;
using Confluent.Kafka;

namespace iba.Processing
{
    class KafkaWriterTaskWorker
    {
        private ConfigurationWorker m_confWorker;
        private KafkaWriterTaskData m_data;
        private StatusData m_sd;
        private IbaAnalyzer.IbaAnalyzer m_ibaAnalyzer;

        internal KafkaWriterTaskWorker(ConfigurationWorker worker, KafkaWriterTaskData task)
        {
            m_confWorker = worker;
            m_data = task;
            m_sd = worker.m_sd;
            m_ibaAnalyzer = worker.m_ibaAnalyzer;
        }

        internal void DoWork(string filename)
        {
            bool bUseAnalysis = !String.IsNullOrEmpty(m_data.AnalysisFile);
            if (bUseAnalysis && !File.Exists(m_data.AnalysisFile))
            {
                string message = iba.Properties.Resources.AnalysisFileNotFound + m_data.AnalysisFile;
                m_confWorker.Log(Logging.Level.Exception, message, filename, m_data);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.COMPLETED_FAILURE;
                }
                return;
            }
            try
            {
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.RUNNING;
                }
                //m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logOPCUAWriterStarted, filename, m_data);

                using (IbaAnalyzerMonitor mon = new IbaAnalyzerMonitor(m_ibaAnalyzer, m_data.MonitorData))
                {
                    if (bUseAnalysis)
                        mon.Execute(delegate () { m_ibaAnalyzer.OpenAnalysis(m_data.AnalysisFile); });
                    mon.Execute(delegate ()
                    {
                        m_data.EvaluateValues(filename, m_ibaAnalyzer);
                    });

                    var config = new ProducerConfig { BootstrapServers = m_data.clusterAddress };

                    config.ClientId = m_data.identifier;// TODO test
                    config.MessageTimeoutMs = (int)(m_data.timeout*1000);
                    switch (m_data.AckMode) // TODO test
                    {
                        case KafkaWriterTaskData.RequiredAcks.None:
                            config.Acks = Confluent.Kafka.Acks.None;
                            break;
                        case KafkaWriterTaskData.RequiredAcks.Leader:
                            config.Acks = Confluent.Kafka.Acks.Leader;
                            break;
                        case KafkaWriterTaskData.RequiredAcks.All:
                            config.Acks = Confluent.Kafka.Acks.All;
                            break;
                    }

                    // Add expert parameters
                    foreach (var kvp in m_data.Params)
                    {
                        if (string.IsNullOrEmpty(kvp.Key))
                            continue;

                        if (kvp.Key.StartsWith("schema.registry."))
                            continue;

                        config.Set(kvp.Key, kvp.Value);
                    }

                    // If serializers are not specified, default serializers from
                    // `Confluent.Kafka.Serializers` will be automatically used where
                    // available. Note: by default strings are encoded as UTF8.

                    using (var p = new ProducerBuilder<string, string>(config).Build())
                    {
                        try
                        {
                            if (m_data.Format == KafkaWriterTaskData.DataFormat.JSONGrouped)
                            {
                                string message = "{ ";

                                for (int i = 0; i < m_data.Records.Count; i++)
                                {
                                    if (i > 0)
                                        message += ", ";
                                    message += "\"" + m_data.Records[i].Expression + "\": " + m_data.ToText(m_data.Records[i]);
                                }
                                message += "}";
                                Debug.WriteLine($"BEFORE AWAIT {System.Threading.Thread.CurrentThread.Name}");
                                //System.Threading.Thread.Sleep(7);
                                var dr = p.ProduceAsync(m_data.topicName, new Message<string, string> {Value = message }).Result;
                                Debug.WriteLine($"AFTER AWAIT {System.Threading.Thread.CurrentThread.Name}");
                            }
                            else if (m_data.Format == KafkaWriterTaskData.DataFormat.JSONPerSignal)
                            {
                                foreach (var rec in m_data.Records)
                                {
                                    string message = "{ \"Signal\": \"" + rec.Expression + "\", \"Value\": " + m_data.ToText(rec) + "}";
                                    var dr = p.ProduceAsync(m_data.topicName, new Message<string, string> { Value = message }).Result;
                                }
                            }
                            else if (m_data.Format == KafkaWriterTaskData.DataFormat.AVRO)
                            {
                                //Avro.RecordSchema.
                                throw new NotImplementedException();
                            }
                        }
                        catch (ProduceException<Null, string> ex)
                        {
                            Debug.WriteLine($"Delivery failed: {ex.Error.Reason}");
                        }
                    }


                    m_confWorker.Log(Logging.Level.Info, iba.Properties.Resources.logComputedValuesUpdated/*TODO*/, filename, m_data);
                    lock (m_sd.DatFileStates)
                    {
                        m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.COMPLETED_SUCCESFULY;
                    }
                }
            }
            catch (IbaAnalyzerExceedingTimeLimitException te)
            {
                m_confWorker.Log(Logging.Level.Exception, te.Message, filename, m_data);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.TIMED_OUT;
                }
                m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (IbaAnalyzerExceedingMemoryLimitException me)
            {
                m_confWorker.Log(Logging.Level.Exception, me.Message, filename, m_data);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.MEMORY_EXCEEDED;
                }
                m_confWorker.RestartIbaAnalyzerAndOpenDatFile(filename);
            }
            catch (Exception e)
            {
                m_confWorker.Log(Logging.Level.Exception, e.Message, filename, m_data);
                lock (m_sd.DatFileStates)
                {
                    m_sd.DatFileStates[filename].States[m_data] = DatFileStatus.State.COMPLETED_FAILURE;
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
                        m_confWorker.Log(iba.Logging.Level.Exception, iba.Properties.Resources.IbaAnalyzerUndeterminedError, filename, m_data);
                        m_confWorker.RestartIbaAnalyzer();
                    }
                }
            }
        }
    }
}
