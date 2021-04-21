using iba.Plugins;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Processing
{
    class ErrorPluginTaskWorker : IPluginTaskWorker
    {
        public ErrorPluginTaskWorker(string error)
        {
            m_error = error;
            m_status = new PluginTaskWorkerStatus();
        }

        private PluginTaskWorkerStatus m_status;
        private string m_error;

        public bool ExecuteTask(string datFile)
        {
            return false;
        }

        public string GetLastError()
        {
            return m_error;
        }

        public PluginTaskWorkerStatus GetWorkerStatus()
        {
            return m_status;
        }

        public bool OnApply(IPluginTaskData newtask, IJobData newParentJob)
        {
            return true;
        }

        public bool OnStart()
        {
            return true;
        }

        public bool OnStop()
        {
            return true;
        }
    }
}
