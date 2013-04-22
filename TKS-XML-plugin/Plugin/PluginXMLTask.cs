using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;

namespace iba.TKS_XML_Plugin
{
    [Serializable]
    public class PluginXMLTask : IPluginTaskDataUNC, IPluginTaskDataIsSame
    {
        public PluginXMLTask()
        {
            InitData(null, null, null);
        }

        public PluginXMLTask(string name, IDatCoHost host, IJobData parentJob)
        {
            InitData(name, host, parentJob);
        }

        private void InitData(string name, IDatCoHost host, IJobData parentJob)
        {
            m_parentJob = parentJob;
            m_datcoHost = host;
            m_nameInfo = name;
        }

        private IDatCoHost m_datcoHost;

        #region IPluginTaskDataUNC Members

        public string Extension
        {
            get { return ".xml"; }
        }

        #endregion

        #region IPluginTaskData Members

        [NonSerialized]
        private PluginXMLControl m_control;
        public IPluginControl GetControl()
        {
            if (m_control == null) m_control = new PluginXMLControl();
            return m_control;
        }

        [NonSerialized]
        private PluginXMLWorker m_worker;
        public IPluginTaskWorker GetWorker()
        {
            if (m_worker == null) m_worker = new PluginXMLWorker(this);
            return m_worker;
        }

        public void SetWorker(IPluginTaskWorker worker)
        {
            m_worker = worker as PluginXMLWorker;
        }



        private string m_nameInfo;
        public string NameInfo
        {
            get { return m_nameInfo; }
            set { m_nameInfo = value; }
        }

        public int DongleBitPos
        {
            get
            {
                return 4;
            } 
        }

        public void Reset(IDatCoHost host)
        {
            m_datcoHost = host;
        }

        private IJobData m_parentJob;
        public void SetParentJob(IJobData data)
        {
            m_parentJob = data;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            PluginXMLTask xt = new PluginXMLTask(m_nameInfo, m_datcoHost, null);
            xt.StandOrt = StandOrt;
            return xt;
        }

        #endregion

        //some data
        private XmlExtract.StandortType m_standOrt = XmlExtract.StandortType.DU;
        public XmlExtract.StandortType StandOrt
        {
            get { return m_standOrt; }
            set { m_standOrt = value; }
        }

        #region IPluginTaskDataIsSame Members

        public bool IsSame(IPluginTaskDataIsSame data)
        {
            return (data as PluginXMLTask).StandOrt == StandOrt;
        }

        #endregion
    }
}
