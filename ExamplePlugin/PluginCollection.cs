using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;

namespace ExamplePlugin
{
    public class PluginCollection : IDatCoPlugin
    {

        #region IDatCoPlugin Members

        PluginTaskInfo m_info;

        public PluginTaskInfo[] GetTasks()
        {
            if (m_info == null)
                m_info = new PluginTaskInfo("CopyP", "The copy task implemented as a plugin", ExamplePlugin.Properties.Resources.copydat_running);

            return new PluginTaskInfo[] {m_info};
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

        public IPluginTaskData CreateTask(string taskname, IJobData m_parentJob)
        {
            if (taskname == "CopyP")
                return new PluginCopyTask(m_info, DatCoordinatorHost, m_parentJob);
            else return null;
        }

        #endregion
    }
}
