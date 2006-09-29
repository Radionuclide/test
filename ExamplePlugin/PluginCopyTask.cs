using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;

namespace ExamplePlugin
{
    [Serializable]
    public class PluginCopyTask : IPluginTaskDataUNC
    {
        public PluginCopyTask()
        {
        }

        private IDatCoHost m_datcoHost;
        private string m_nameInfo;
        public string NameInfo
        {
            get { return m_nameInfo; }
            set { m_nameInfo = value; }
        }

        private IJobData m_parentJob;
        public void SetParentJob( IJobData data)
        {
            m_parentJob = data; 
        }

        public void Reset(IDatCoHost host)
        {
            m_datcoHost = host;
        }

        public PluginCopyTask(string name, IDatCoHost host, IJobData parentJob)
        {
            m_parentJob = parentJob;
            m_pass = "";
            m_username = "";
            m_destinationMapUNC = "";
            m_datcoHost = host;
            m_removeSource = false;
            m_subfolderChoice = SubfolderChoiceC.NONE;
            m_numbFolders = 10;
            m_nameInfo = name;
        }

        bool m_removeSource;
        public bool RemoveSource
        {
            get { return m_removeSource; }
            set { m_removeSource = value; }
        }

        private uint m_numbFolders;
        public uint SubfoldersNumber
        {
            get { return m_numbFolders; }
            set { m_numbFolders = value; }
        }

        //letter C added because namecollision in XML-Serialiser with SubFolderCHoice from reportdata
        public enum SubfolderChoiceC { NONE, HOUR, DAY, WEEK, MONTH,SAME};
        private SubfolderChoiceC m_subfolderChoice;
        public SubfolderChoiceC Subfolder
        {
            get { return m_subfolderChoice; }
            set { m_subfolderChoice = value; }
        }

        #region IPluginTaskData Members

        [NonSerialized]
        private PluginCopyTaskControl m_control;
        public IPluginControl GetControl()
        {
            if (m_control == null) m_control = new PluginCopyTaskControl(m_datcoHost);
            return m_control;
        }

        [NonSerialized]
        private PluginCopyTaskWorker m_worker;
        public IPluginTaskWorker GetWorker()
        {
            if (m_worker == null) m_worker = new PluginCopyTaskWorker(m_datcoHost, this, m_parentJob);
            return m_worker;
        }

        public void Destroy()
        {
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            PluginCopyTask cd = new PluginCopyTask(m_nameInfo,m_datcoHost,null);
            cd.m_removeSource = m_removeSource;
            cd.m_subfolderChoice = m_subfolderChoice;
            cd.m_destinationMap = m_destinationMap;
            cd.m_numbFolders = m_numbFolders;
            cd.m_username = m_username;
            cd.m_pass = m_pass;
            cd.m_destinationMapUNC = m_destinationMapUNC;
            return cd;
        }

        #endregion

        #region IPluginTaskDataUNC Members

        public void UpdateUNC()
        {
            m_destinationMapUNC = m_datcoHost.PathToUnc(m_destinationMap, false);
        }

        public string[][] UNCPaths()
        {
            return new string[][] { new string[] { m_destinationMap, m_username, m_pass } };
        }

        #endregion

        protected string m_destinationMap;
        public string DestinationMap
        {
            get { return m_destinationMap; }
            set { m_destinationMap = value; }
        }

        protected string m_destinationMapUNC;
        public string DestinationMapUNC
        {
            get { return m_destinationMapUNC; }
            set { m_destinationMapUNC = value; }
        }

        protected string m_username;
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        protected string m_pass;
        public string Password
        {
            get { return m_pass; }
            set { m_pass = value; }
        }
    }
}
