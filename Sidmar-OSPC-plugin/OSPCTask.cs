using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iba.Plugins;

namespace AM_OSPC_plugin
{
    [Serializable]
    public class OSPCTask : IPluginTaskData
    {

        #region IPluginTaskData Members

        [NonSerialized]
        private OSPCTaskControl m_control;
        public IPluginControl GetControl()
        {
            if(m_control == null) m_control = new OSPCTaskControl(m_datcoHost);
            return m_control;
        }

        [NonSerialized]
        private OSPCTaskWorker m_worker;

        public IPluginTaskWorker GetWorker()
        {
            if(m_worker == null) m_worker = new OSPCTaskWorker(this);
            return m_worker;
        }

        public void SetWorker(IPluginTaskWorker worker)
        {
            m_worker = worker as OSPCTaskWorker;
        }

        public string NameInfo
        {
            get { return m_nameInfo; }
            set { m_nameInfo = value; }
        }

        public OSPCTask()
        {
            InitData(null, null, null);
        }

        public int DongleBitPos
        {
            get
            {
                return 5;
            } 
        }

        public void Reset(IDatCoHost host)
        {
            m_datcoHost = host;
        }

        public void SetParentJob(IJobData data)
        {
            m_parentJob = data;
        }

        #endregion

        #region ICloneable Members

        public object Clone()
        {
            throw new NotImplementedException();
        }

        #endregion

        public OSPCTask(string name, IDatCoHost host, IJobData parentJob)
        {
            //try
            //{
            //    IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            //    NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            //    m_ownAddress = nics[0].GetPhysicalAddress().GetAddressBytes();
            //}
            //catch (Exception)
            //{
            //    m_ownAddress = new Byte[] { 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF };
            //}

            InitData(name, host, parentJob);
        }



        private IDatCoHost m_datcoHost;
        private IJobData m_parentJob;

        private string m_nameInfo;

        private void InitData(string name, IDatCoHost host, IJobData parentJob)
        {
            m_parentJob = parentJob;
            m_datcoHost = host;
            m_nameInfo = name;
        }

    }
}
