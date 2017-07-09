using System;
using System.Text;
using iba.Plugins;

namespace S7_writer_plugin
{
    class PluginCollection : IDatCoPlugin
    {
        #region IDatCoPlugin Members

        PluginTaskInfo m_info;
        public PluginTaskInfo[] GetTasks()
        {
            if(m_info == null)
            {
                m_info = new PluginTaskInfo("S7 Writer", "Writes data to S7 DBs.", S7_writer_plugin.Properties.Resources.s7Icon);
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
            if(taskname == "S7 Writer")
                return new S7TaskData(taskname, DatCoordinatorHost, parentjob);
            else return null;
        }

        #endregion
    }
}
