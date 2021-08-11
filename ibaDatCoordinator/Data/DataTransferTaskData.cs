using System;
using System.Xml.Serialization;
using DevExpress.XtraSplashScreen;
using iba.Utility;

namespace iba.Data
{
    [Serializable]
    public class DataTransferTaskData : TaskData
    {
        private string m_clientId;
        public string ClientId
        {
            get => m_clientId;
            set => m_clientId = value;
        }
        private string m_hostname;
        public string Hostname
        {
            get => m_hostname;
            set => m_hostname = value;
        }

        private string m_server;

        public string Server
        {
            get => m_server; 
            set => m_server = value;
        }

        private string m_port;
        public string Port
        {
            get => m_port;
            set => m_port = value;
        }

        private string m_username;

        public string Username
        {
            get => m_username; 
            set => m_username = value;
        }
        private string m_password;

        [XmlIgnore]
        public string Password
        {
            get => m_password; 
            set => m_password = value;
        }

        private string m_remotePath;
        public string RemotePath
        {
            get => m_remotePath; 
            set => m_remotePath = value;
        }

        private int m_maxBandwidth;
        public int MaxBandwidth
        {
            get => m_maxBandwidth;
            set => m_maxBandwidth = value;
        }

        public DataTransferTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.DataTransferTaskTitle;
            m_remotePath = "/";
            m_hostname = System.Net.Dns.GetHostName();
            m_version = DatCoVersion.GetVersion();
            m_maxBandwidth = 0;
            m_port = "1";
            WhatFileTransfer = WhatFileTransferEnum.DATFILE;
        }

        public enum WhatFileTransferEnum { DATFILE, PREVOUTPUT }

        private WhatFileTransferEnum m_WhatFileTransfer;
        private string m_version;


        public WhatFileTransferEnum WhatFileTransfer
        {
            get => m_WhatFileTransfer;
            set => m_WhatFileTransfer = value;
        }

        public string Version
        {
            get => m_version;
            set => m_version = value;
        }

        public DataTransferTaskData()
            : this(null)
        {
        }

        public override TaskData CloneInternal()
        {
            DataTransferTaskData cd = new DataTransferTaskData(null);
            cd.m_hostname = m_hostname;
            cd.m_server = m_server;
            cd.m_version = m_version;
            cd.m_username = m_username;
            cd.m_password = m_password;
            cd.m_port = m_port;
            cd.m_remotePath = m_remotePath;
            cd.m_maxBandwidth = m_maxBandwidth;
            cd.m_WhatFileTransfer = m_WhatFileTransfer;
            return cd;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            DataTransferTaskData other = taskData as DataTransferTaskData;
            if (other == null) return false;
            if (other == this) return true;
            return
                other.m_hostname == m_hostname &&
                other.m_server == m_server &&
                other.m_version == m_version &&
                other.m_username == m_username &&
                other.m_password == m_password &&
                other.m_port == m_port &&
                other.m_remotePath == m_remotePath &&
                other.m_maxBandwidth == m_maxBandwidth &&
                other.m_WhatFileTransfer == m_WhatFileTransfer;
        }
    }
}
