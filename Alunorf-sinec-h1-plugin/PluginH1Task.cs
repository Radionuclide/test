using System;
using System.Collections.Generic;
using System.Text;
using System.Net.NetworkInformation;
using iba.Plugins;

namespace Alunorf_sinec_h1_plugin
{
    [Serializable]
    public class PluginH1Task : IPluginTaskData
    {
        [Serializable]
        public struct TelegramRecord
        {
            public string Name;
            public string DataType;
            public string Comment;
        }

        [Serializable]
        public class Telegram
        {
            
            private List<TelegramRecord> m_data_info;
            public List<TelegramRecord> DataInfo
            {
                get { return m_data_info; }
                set { m_data_info = value; }
            }

            private List<TelegramRecord> m_data_signal;
            public List<TelegramRecord> DataSignal
            {
                get { return m_data_signal; }
                set { m_data_signal = value; }
            }

            private string m_description;

            public string Description
            {
                get { return m_description; }
                set { m_description = value; }
            }

            public Telegram()
            {
                m_data_info = new List<TelegramRecord>();
                m_data_signal = new List<TelegramRecord>();
                m_qdtType = 0;
                m_description = "";
            }

            public Telegram Clone()
            {
                Telegram tele = new Telegram();
                foreach (TelegramRecord rec in m_data_signal)
                {
                    tele.m_data_signal.Add(rec);
                }
                foreach (TelegramRecord rec in m_data_info)
                {
                    tele.m_data_info.Add(rec);
                }

                tele.m_qdtType = m_qdtType;
                tele.m_description = m_description;
                return tele;
            }


            private short m_qdtType;
            public short QdtType
            {
                get { return m_qdtType; }
                set { m_qdtType = value; }
            }
        }

        private List<Telegram> m_telegrams;
        public List<Telegram> Telegrams
        {
            get { return m_telegrams; }
            set { m_telegrams = value; }
        }

        private byte[] m_ownAddress;
        public byte[] OwnAddress
        {
            get { return m_ownAddress; }
            set { m_ownAddress = value; }
        }

        private byte[] m_NQSAddress1;
        public byte[] NQSAddress1
        {
            get { return m_NQSAddress1; }
            set { m_NQSAddress1 = value; }
        }

        private byte[] m_NQSAddress2;
        public byte[] NQSAddress2
        {
            get { return m_NQSAddress2; }
            set { m_NQSAddress2 = value; }
        }

        private string m_ownTSAPforNQS1;
        public string OwnTSAPforNQS1
        {
            get { return m_ownTSAPforNQS1; }
            set { m_ownTSAPforNQS1 = value; }
        }

        private string m_ownTSAPforNQS2;
        public string OwnTSAPforNQS2
        {
            get { return m_ownTSAPforNQS2; }
            set { m_ownTSAPforNQS2 = value; }
        }

        private string m_NQS_TSAPforNQS1;
        public string NQS_TSAPforNQS1
        {
            get { return m_NQS_TSAPforNQS1; }
            set { m_NQS_TSAPforNQS1 = value; }
        }

        private string m_NQS_TSAPforNQS2;
        public string NQS_TSAPforNQS2
        {
            get { return m_NQS_TSAPforNQS2; }
            set { m_NQS_TSAPforNQS2 = value; }
        }

        private int m_connectionTimeOut;
        public int ConnectionTimeOut
        {
            get { return m_connectionTimeOut; }
            set { m_connectionTimeOut = value; }
        }
        
        private int m_retryConnectTimeInterval;
        public int RetryConnectTimeInterval
        {
            get { return m_retryConnectTimeInterval; }
            set { m_retryConnectTimeInterval = value; }
        }

        private int m_sendTimeOut;
        public int SendTimeOut
        {
            get { return m_sendTimeOut; }
            set { m_sendTimeOut = value; }
        }

        private int m_ackTimeOut;

        public int AckTimeOut
        {
            get { return m_ackTimeOut; }
            set { m_ackTimeOut = value; }
        }

        #region IPluginTaskData Members

        [NonSerialized]
        private PluginH1TaskControl m_control;
        public IPluginControl GetControl()
        {
            if (m_control == null) m_control = new PluginH1TaskControl(m_datcoHost);
            return m_control;
        }

        [NonSerialized]
        private PluginH1TaskWorker m_worker;
        public IPluginTaskWorker GetWorker()
        {
            if (m_worker == null) m_worker = new PluginH1TaskWorker(this);
            return m_worker;
        }

        public void SetWorker(IPluginTaskWorker worker)
        {
            m_worker = worker as PluginH1TaskWorker;
        }

        private IDatCoHost m_datcoHost;

        private string m_nameInfo;

        public string NameInfo
        {
            get { return m_nameInfo; }
            set { m_nameInfo = value; }
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
        
        public PluginH1Task()
        {
            InitData(null, null, null);
        }

        public PluginH1Task(string name, IDatCoHost host, IJobData parentJob)
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

        private void InitData(string name, IDatCoHost host, IJobData parentJob)
        {
            m_ownAddress = new Byte[] { 0x00, 0x15, 0xBA, 0x00, 0x03, 0x7A };
            m_NQSAddress1 = new Byte[] { 0x0A, 0x00, 0x8E, 0x00, 0x00, 0x01 };
            m_NQSAddress2 = new Byte[] { 0x0A, 0x00, 0x8E, 0x00, 0x00, 0x02 };
            m_NQS_TSAPforNQS1 = "BR 1";
            m_NQS_TSAPforNQS2 = "BR 2";
            m_ownTSAPforNQS1 = "LT4NQI";
            m_ownTSAPforNQS2 = "LT4NQI";

            m_connectionTimeOut = 1;
            m_retryConnectTimeInterval = 1;
            m_sendTimeOut = 10;
            m_ackTimeOut = 10;

            m_parentJob = parentJob;
            m_datcoHost = host;
            m_nameInfo = name;
            m_lastSelectedTelegram = -1;
            m_telegrams = new List<Telegram>();
        }

        private int m_lastSelectedTelegram;

        public int LastSelectedTelegram
        {
            get { return m_lastSelectedTelegram; }
            set { m_lastSelectedTelegram = value; }
        }

        #region ICloneable Members

        public object Clone()
        {
            PluginH1Task ht = new PluginH1Task(m_nameInfo, m_datcoHost, null);
            ht.m_NQSAddress1 = m_NQSAddress1;
            ht.m_NQSAddress2 = m_NQSAddress2;
            ht.m_NQS_TSAPforNQS1 = m_NQS_TSAPforNQS1;
            ht.m_NQS_TSAPforNQS2 = m_NQS_TSAPforNQS2;
            ht.m_ownAddress = m_ownAddress;
            ht.m_ownTSAPforNQS1 = m_ownTSAPforNQS1;
            ht.m_ownTSAPforNQS2 = m_ownTSAPforNQS2;
            ht.m_connectionTimeOut = m_connectionTimeOut;
            ht.m_retryConnectTimeInterval = m_retryConnectTimeInterval;
            ht.m_sendTimeOut = m_sendTimeOut;
            ht.m_ackTimeOut = m_ackTimeOut;
            ht.m_lastSelectedTelegram = m_lastSelectedTelegram;
            foreach (Telegram tele in m_telegrams)
                ht.Telegrams.Add(tele.Clone());
            return ht;
        }

        #endregion

        #region IPluginTaskData Members


        public int DongleBitPos
        {
            get
            {
                return 1;
            }
        }

        #endregion
    }
}
