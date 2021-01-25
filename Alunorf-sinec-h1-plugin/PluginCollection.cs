using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;

namespace Alunorf_sinec_h1_plugin
{
    class PluginCollection : IDatCoPlugin2
    {
        #region IDatCoPlugin2 Members

        PluginTaskInfo m_info;
        public PluginTaskInfo[] GetTasks()
        {
            if (m_info == null)
            {
                m_info = new PluginTaskInfo("SINEC-H1", "Alunorf SINEC-H1", Alunorf_sinec_h1_plugin.Properties.Resources.H1Task);
                return new PluginTaskInfo[] { m_info };
            }

            return new PluginTaskInfo[] { m_info };
        }

        IDatCoHost m_host;
        public IDatCoHost DatCoordinatorHost
        {
            get
            {
                return m_host;
            }
            set
            {
                m_host = value;
            }
        }

        public IPluginTaskData CreateTask(string taskname, IJobData parentjob)
        {
            if (taskname == "SINEC-H1")
                return new PluginH1Task(taskname, DatCoordinatorHost, parentjob);
            else return null;
        }

        #endregion
    }
}
