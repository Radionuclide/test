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

        private bool m_shouldDeleteAfterTransfer;
        public bool ShouldDeleteAfterTransfer
        {
            get => m_shouldDeleteAfterTransfer;
            set => m_shouldDeleteAfterTransfer = value;
        }
        public DataTransferTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.DataTransferTaskTitle;
            m_remotePath = "/";
            m_hostname = System.Net.Dns.GetHostName();
            m_version = DatCoVersion.GetVersion();
            m_maxBandwidth = 0;
            m_port = "30051";
            WhatFileTransfer = WhatFileTransferEnum.DATFILE;
            CbBandwidth = 0;
            NumBandwidth = 64;
        }

        public enum WhatFileTransferEnum { DATFILE, PREVOUTPUT }

        private WhatFileTransferEnum m_WhatFileTransfer;
        private string m_version;
        
        private bool m_ChkLimitBandwidth;
        public bool ChkLimitBandwidth
        {
            get => m_ChkLimitBandwidth;
            set => m_ChkLimitBandwidth = value;
        }

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

        public int CbBandwidth { get; set; }
        public decimal NumBandwidth { get; set; }


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
            cd.m_shouldDeleteAfterTransfer = m_shouldDeleteAfterTransfer;
            cd.m_ChkLimitBandwidth = m_ChkLimitBandwidth;
            cd.CbBandwidth = CbBandwidth;
            cd.NumBandwidth = NumBandwidth;
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
                other.m_WhatFileTransfer == m_WhatFileTransfer &&
                other.m_shouldDeleteAfterTransfer == m_shouldDeleteAfterTransfer &&
                other.m_ChkLimitBandwidth == m_ChkLimitBandwidth &&
                other.CbBandwidth == CbBandwidth &&
                other.NumBandwidth == NumBandwidth;
        }
    }
}
