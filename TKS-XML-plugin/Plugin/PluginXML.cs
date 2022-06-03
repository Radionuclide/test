using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;

namespace iba.TKS_XML_Plugin
{
    class PluginXML : IDatCoPlugin3
    {
        #region IDatCoPlugin3 Members

        PluginTaskInfo m_info;
        public PluginTaskInfo[] GetTasks()
        {
            if (m_info == null)
            {
                m_info = new PluginTaskInfoUNC("TKS-XML", "Thyssenkrupp Steel .xml extraktor", iba.Icons.Gui.All.Images.FileXml(24));
                return new PluginTaskInfo[] { m_info };
            }

            return new PluginTaskInfo[] { m_info };
        }

        static IDatCoHost m_host;
        internal static IDatCoHost Host => m_host;

        public IDatCoHost DatCoordinatorHost 
        {
            get => m_host;
            set => m_host = value;
        }

        public IPluginTaskData CreateTask(string taskname, IJobData parentjob)
        {
            if (taskname == "TKS-XML")
                return new PluginXMLTask(taskname, parentjob);
            else return null;
        }

        #endregion
    }
}
