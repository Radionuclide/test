using System;
using System.Collections.Generic;
using System.Text;

using iba.Plugins;

namespace Alunorf_roh_plugin
{
    class PluginCollection : IDatCoPlugin
    {
        #region IDatCoPlugin Members

        PluginTaskInfo m_info;
        public PluginTaskInfo[] GetTasks()
        {
            if (m_info == null)
            {
                m_info = new PluginTaskInfo("ROH", "Alunorf .roh extraktor", Alunorf_roh_plugin.Properties.Resources.RohTask);
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
            if (taskname == "ROH")
                return new PluginRohTask(taskname, DatCoordinatorHost, parentjob);
            else return null;
        }

        #endregion
    }
}