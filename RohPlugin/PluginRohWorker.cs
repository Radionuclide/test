using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;

namespace Alunorf_roh_plugin
{
    class PluginRohWorker : IPluginTaskWorker
    {
        #region IPluginTaskWorker Members

        public bool OnStart()
        {
            return true;
        }

        public bool OnStop()
        {
            return true;
        }

        public bool OnApply(IPluginTaskData newtask, IJobData newParentJob)
        {
            throw new NotImplementedException();
        }

        public bool ExecuteTask(string datFile)
        {
            throw new NotImplementedException();
        }

        public string GetLastError()
        {
            throw new NotImplementedException();
        }

        public PluginTaskWorkerStatus GetWorkerStatus()
        {
            throw new NotImplementedException();
        }

        #endregion

        private PluginRohTask m_data;
        public PluginRohWorker(PluginRohTask data)
        {
            m_data = data;
        }
    }
}
