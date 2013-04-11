using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;

namespace iba.TKS_XML_Plugin
{
    class PluginCollection : IDatCoPlugin
    {
        #region IDatCoPlugin Members

        PluginTaskInfo m_info;
        public PluginTaskInfo[] GetTasks()
        {
            if (m_info == null)
            {
                m_info = new PluginTaskInfoUNC("TKS-XML", "Thyssenkrupp Steel .xml extraktor", iba.TKS_XML_Plugin.Properties.Resources.TKSXMLTask);
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
            if (taskname == "TKS-XML")
                return new PluginXMLTask(taskname, DatCoordinatorHost, parentjob);
            else return null;
        }

        #endregion
    }
}
