using System;
using System.Xml.Serialization;
using DevExpress.XtraSplashScreen;
using iba.Utility;

namespace iba.Data
{
    [Serializable]
    public class DataTransferTaskData : TaskData
    {
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

        public DataTransferTaskData(ConfigurationData parent)
            : base(parent)
        {
            m_name = iba.Properties.Resources.DataTransferTaskTitle;
            RemotePath = "/";
        }

        public enum WhatFileUploadEnum { DATFILE, PREVOUTPUT }

        private WhatFileUploadEnum m_whatFileUpload;

        public WhatFileUploadEnum WhatFileUpload
        {
            get => m_whatFileUpload;
            set => m_whatFileUpload = value;
        }

        public DataTransferTaskData()
            : this(null)
        {
        }

        public override TaskData CloneInternal()
        {
            DataTransferTaskData cd = new DataTransferTaskData(null);
            cd.m_server = m_server;
            cd.m_username = m_username;
            cd.m_password = m_password;
            cd.m_port = m_port;
            cd.m_remotePath = m_remotePath;
            cd.m_whatFileUpload = m_whatFileUpload;
            return cd;
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            DataTransferTaskData other = taskData as DataTransferTaskData;
            if (other == null) return false;
            if (other == this) return true;
            return
                other.m_whatFileUpload == m_whatFileUpload &&
                other.m_server == m_server &&
                other.m_username == m_username &&
                other.m_password == m_password &&
                other.m_port == m_port &&
                other.m_remotePath == m_remotePath;
        }
    }
}
