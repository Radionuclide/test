using System;
using System.Text;
using DevExpress.XtraEditors.Repository;
using iba.Plugins;


namespace S7_writer_plugin
{
    class PluginCollection : IDatCoPlugin2, IGridAnalyzer
	{
        #region IDatCoPlugin2 Members

        PluginTaskInfo m_info;
        public PluginTaskInfo[] GetTasks()
        {
            if(m_info == null)
            {
                m_info = new PluginTaskInfo("S7 Writer", "Writes data to S7 DBs.", iba.Icons.Gui.All.Images.S7(24));
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

		private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit m_channelEditor;
		private IAnalyzerManagerUpdateSource m_analyzer;
		public void SetGridAnalyzer(object e, IAnalyzerManagerUpdateSource analyzer)
		{
			m_channelEditor = e as DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit;
			m_analyzer = analyzer;
		}
		public IPluginTaskData CreateTask(string taskname, IJobData parentjob)
        {
			if (taskname == "S7 Writer")
			{
				var td = new S7TaskData(taskname, parentjob);
				td.SetGridAnalyzer(m_channelEditor, m_analyzer);
				return td;
			}
			else
				return null;
        }


		#endregion
	}
}
