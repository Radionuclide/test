using System;
using System.Collections.Generic;
using System.Text;
using iba.Processing;
using System.Xml.Serialization;

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

        private bool m_doPostPoning;
        public bool DoPostPoning
        {
            get { return m_doPostPoning; }
            set { m_doPostPoning = value; }
        }

        private int m_maxResourceIntensiveTasks;
        public int MaxResourceIntensiveTasks
        {
            get { return m_maxResourceIntensiveTasks; }
            set { m_maxResourceIntensiveTasks = value; }
        }
    

        private int m_PostponingMinutes;
        public int PostponingMinutes
        {
            get { return m_PostponingMinutes; }
            set { m_PostponingMinutes = value; }
        }

        private int m_ProcessPriority;
        public int ProcessPriority
        {
            get { return m_ProcessPriority; }
            set { m_ProcessPriority = value; }
        }

        private string m_pass;

        [XmlIgnore]
        public string Password
        {
            get { return m_pass; }
            set { m_pass = value; }
        }

        public string PasswordCrypted
        {
            get { return iba.Utility.Crypt.Encrypt(m_pass); }
            set { m_pass = iba.Utility.Crypt.Decrypt(value); }
        }

        private bool m_doRememberPass;
        public bool RememberPass
        {
            get { return m_doRememberPass; }
            set { m_doRememberPass = value; }
        }

        private int m_rememberTimeMinutes;
        public int RememberTimeMinutes
        {
            get { return m_rememberTimeMinutes; }
            set { m_rememberTimeMinutes = value; }
        }

        public static ibaDatCoordinatorData Create(TaskManager manager)
        {
            ibaDatCoordinatorData answer = new ibaDatCoordinatorData();
            answer.m_confs = manager.Configurations;
            answer.m_logItemCount = LogData.Data.MaxRows;
            answer.m_wd = manager.WatchDogData;
            answer.m_doPostPoning = manager.DoPostponeProcessing;
            answer.m_maxResourceIntensiveTasks = manager.MaxResourceIntensiveTasks;
            answer.m_PostponingMinutes = manager.PostponeMinutes;
            answer.m_ProcessPriority = manager.ProcessPriority;
            answer.m_pass = manager.Password;
            answer.m_doRememberPass = manager.RememberPassEnabled;
            answer.m_rememberTimeMinutes = (int) manager.RememberPassTime.TotalMinutes;
            return answer;
        }

        public List<ConfigurationData> ApplyToManager(TaskManager manager) //returns confs instead of applying them to manager immediately because they might need special treatment before
        {
            List<ConfigurationData> confs;
            manager.ReplaceWatchdogData(WatchDogData);
            manager.ProcessPriority = ProcessPriority;
            manager.PostponeMinutes = PostponingMinutes;
            manager.DoPostponeProcessing = DoPostPoning;
            manager.MaxResourceIntensiveTasks = MaxResourceIntensiveTasks;
            manager.RememberPassTime = TimeSpan.FromMinutes(RememberTimeMinutes);
            manager.RememberPassEnabled = RememberPass;
            manager.Password = Password;
            confs = Configurations;
            if (LogItemCount == 0) LogItemCount = 50;
            LogData.Data.MaxRows = LogItemCount;
            return confs;
        }

        private ibaDatCoordinatorData()
        {
            //defaults
            m_version = 5;
            m_wd = null;
            m_confs = null;
            m_logItemCount = 50;
            m_doPostPoning = true;
            m_PostponingMinutes = 5;
            m_ProcessPriority = (int) System.Diagnostics.ProcessPriorityClass.Normal;
            m_pass = "";
            m_doRememberPass = false;
            m_rememberTimeMinutes = 5;
            m_maxResourceIntensiveTasks = 6;
        }
    }
}
