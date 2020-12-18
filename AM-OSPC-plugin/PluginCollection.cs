﻿using System;
using System.Text;
using DevExpress.XtraEditors.Repository;
using iba.Plugins;

namespace AM_OSPC_plugin
{
    class PluginCollection : IDatCoPlugin, IGridAnalyzer
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

		private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit m_channelEditor;
		private IAnalyzerManagerUpdateSource m_analyzer;
		public void SetGridAnalyzer(RepositoryItemPopupContainerEdit e, IAnalyzerManagerUpdateSource analyzer)
		{
			m_channelEditor = e;
			m_analyzer = analyzer;
		}
        public IPluginTaskData CreateTask(string taskname, IJobData parentjob)
        {
            if(taskname == "OSPC")
			{
				var td = new OSPCTaskData(taskname, DatCoordinatorHost, parentjob);
				td.SetGridAnalyzer(m_channelEditor, m_analyzer);
				return td;
			}
            else
				return null;
        }


		#endregion
	}
}
