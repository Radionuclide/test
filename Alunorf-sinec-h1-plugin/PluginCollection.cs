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
                m_info = new PluginTaskInfo("SINEC-H1", "Alunorf SINEC-H1", null/*Alunorf_sinec_h1_plugin.Properties.Resources.H1Task*/);
                return new PluginTaskInfo[] { m_info };
            }

            return new PluginTaskInfo[] { m_info };
        }

        static IDatCoHost m_host;
        internal static IDatCoHost Host => m_host;

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
                return new PluginH1Task(taskname, parentjob);
            else return null;
        }

        #endregion
    }
}
