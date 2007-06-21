using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Data
{
    [Serializable]
    public class ibaDatCoordinatorData
    {
        private int m_version;
        public int FileVersion
        {
            get { return m_version; }
            set {m_version = value;}
        }

        private WatchDogData m_wd;
        public WatchDogData WatchDogData
        {
            get { return m_wd; }
            set { m_wd = value; }
        }
        private List<ConfigurationData> m_confs;
        public List<ConfigurationData> Configurations
        {
            get { return m_confs; }
            set { m_confs = value; }
        }
        private int m_logItemCount;
        public int LogItemCount
        {
            get { return m_logItemCount; }
            set { m_logItemCount = value; }
        }

        public ibaDatCoordinatorData(WatchDogData wd, List<ConfigurationData> confs, int count)
        {
            m_version = 1;
            m_wd = wd;
            m_confs = confs;
            m_logItemCount = count;
        }

        public ibaDatCoordinatorData()
        {
            m_version = 2;
            m_wd = null;
            m_confs = null;
            m_logItemCount = 50;
        }
    }
}
