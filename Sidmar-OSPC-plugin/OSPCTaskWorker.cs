using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iba.Plugins;

namespace AM_OSPC_plugin
{
    public class OSPCTaskWorker : IPluginTaskWorker
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
            m_dataToApply = newtask as OSPCTask;
            return true;
        }

        public bool ExecuteTask(string datFile)
        {
            if(m_dataToApply != null)
            {
                m_data = m_dataToApply;
                m_dataToApply = null;
            }

            m_error = "";
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

        private OSPCTask m_data;
        private OSPCTask m_dataToApply;
        public OSPCTaskWorker(OSPCTask data)
        {
            m_data = data;
        }
    }
}
