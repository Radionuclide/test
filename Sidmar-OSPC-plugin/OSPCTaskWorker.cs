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
        public bool OnStart()
        {
            //throw new Exception("The method or operation is not implemented.");
            m_bStarted = true;
            return true;
        }

        public bool OnStop()
        {
            //throw new Exception("The method or operation is not implemented.");
            m_bStarted = false;
            return true;
        }

        public bool OnApply(IPluginTaskData newtask, IJobData newParentJob)
        {
            //throw new Exception("The method or operation is not implemented.");
            return true;
        }

        public bool ExecuteTask(string datFile)
        {
            //throw new Exception("The method or operation is not implemented.");
            return true;
        }

        public string GetLastError()
        {
            //throw new Exception("The method or operation is not implemented.");
            return "";
        }

        public PluginTaskWorkerStatus GetWorkerStatus()
        {
            PluginTaskWorkerStatus status = new PluginTaskWorkerStatus();
            status.started = m_bStarted;
            return status;
        }

        private OSPCTask m_data;
        public OSPCTaskWorker(OSPCTask data)
        {
            m_data = data;
        }
    }
}
