using System;
using System.Text;
using iba.Plugins;

namespace AM_OSPC_plugin
{
    class PluginCollection : IDatCoPlugin
    {
        #region IDatCoPlugin Members

        PluginTaskInfo m_info;
        public PluginTaskInfo[] GetTasks()
        {
            if(m_info == null)
            {
                m_info = new PluginTaskInfo("OSPC", "A.M. OSPC", AM_OSPC_plugin.Properties.Resources.OSPC);
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
            if(taskname == "OSPC")
                return new OSPCTask(taskname, DatCoordinatorHost, parentjob);
            else return null;
        }

        #endregion
    }
}
