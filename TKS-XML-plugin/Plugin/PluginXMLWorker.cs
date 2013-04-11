using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;

namespace iba.TKS_XML_Plugin
{
    class PluginXMLWorker : IPluginTaskWorkerUNC
    {
        private PluginXMLTask m_data;
        private PluginXMLTask m_dataToApply;
        private bool m_started;
        private XmlExtract.DatExtractor m_extractor;
        public PluginXMLWorker(PluginXMLTask data)
        {
            m_data = data;
            m_dataToApply = null;
            m_error = "";
            m_started = false;
        }

        #region IPluginTaskWorkerUNC Members

        public bool ExecuteTask(string datFile, string output)
        {
            if (!m_started)
            {
                throw new Exception("Plugin not started"); //should not happen
            }
            if (m_dataToApply != null)
            {
                m_data = m_dataToApply;
                m_dataToApply = null;
            }
            m_error = m_extractor.ExtractToXml(datFile,output,m_data.StandOrt);
            return string.IsNullOrEmpty(m_error);
        }

        #endregion

        #region IPluginTaskWorker Members

        public bool OnStart()
        {
            m_extractor = new XmlExtract.DatExtractor();
            m_started = true;
            return true;
        }

        public bool OnStop()
        {
            if (m_started)
            {
                m_extractor.Dispose();
                m_extractor = null;
            }
            m_started = false;
            return true;
        }

        public bool OnApply(IPluginTaskData newtask, IJobData newParentJob)
        {
            m_dataToApply = newtask as PluginXMLTask;
            return true;
        }

        public bool ExecuteTask(string datFile)
        {
            throw new NotImplementedException(); //keep this this way, ExecuteTask without a second argument should never be called
            //method is only here for historical reasons (i.e. specified in IPluginTaskWorker from which IPluginTaskWorkerUNC derives)
        }


        private string m_error;
        public string GetLastError()
        {
            return m_error;
        }

        public PluginTaskWorkerStatus GetWorkerStatus()
        {
            PluginTaskWorkerStatus res = new PluginTaskWorkerStatus();
            res.started = m_started;
            res.extraData = null;
            return res;
        }

        #endregion
    }
}
