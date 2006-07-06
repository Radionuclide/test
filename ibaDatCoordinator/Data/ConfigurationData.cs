using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;

using System.Diagnostics;


namespace iba.Data
{
    [ Serializable ]
    public class ConfigurationData : ICloneable, IComparable<ConfigurationData>
    {
        private string m_name;
        public string Name
        {
            get { return m_name ; }
            set { if (value.Length != 0) m_name = value; }
        }
        private string m_datDirectory;
        public string DatDirectory
        {
            get {return m_datDirectory;}
            set { if (value.Length != 0) m_datDirectory = value;}
        }

        private List<TaskData> m_tasks;

        [XmlElement(Type = typeof(ReportData)), XmlElement(Type = typeof(ExtractData)), XmlElement(Type = typeof(BatchFileData)), XmlElement(Type = typeof(CopyMoveTaskData))]
        public List<TaskData> Tasks
        {
            get { return m_tasks; }
            set { m_tasks = value; }
        }

        private string m_ibaAnalyserExe;
        public string IbaAnalyserExe
        {
            get { return m_ibaAnalyserExe; }
            set { if (value.Length != 0) m_ibaAnalyserExe = value; }
        }

        private bool m_doSubDirs;
        public bool SubDirs
        {
            get { return m_doSubDirs; }
            set { m_doSubDirs = value; }
        }
     
        private TimeSpan m_time1;
        [XmlIgnore]
        public TimeSpan ReprocessErrorsTimeInterval
        {
            get { return m_time1; }
            set { m_time1 = value; }
        }

        public long ReprocessErrorsTimeIntervalTicks
        {
            get { return m_time1.Ticks; }
            set { m_time1 = new TimeSpan(value); }
        }

        private TimeSpan m_time2;
        [XmlIgnore]
        public TimeSpan RescanTimeInterval
        {
            get { return m_time2; }
            set { m_time2 = value; }
        }

        public long RescanTimeIntervalTicks
        {
            get { return m_time2.Ticks; }
            set { m_time2 = new TimeSpan(value); }
        }


        private bool m_enabled;
        public bool Enabled
        {
            get { return m_enabled; }
            set { m_enabled = value; }
        }

        private bool m_autoStart;
        public bool AutoStart
        {
            get { return m_autoStart;}
            set { m_autoStart = value;}
        }

        static ulong m_idCounter = 0UL;
        private ulong m_id;
        public ulong ID
        {
            get { return m_id; }
        }

        static public ulong IdCounter
        {
            get { return m_idCounter; }
            set { m_idCounter = value; }
        }


        public ConfigurationData() : this("") 
        {

        }

        private NotificationData m_notify;
        public NotificationData NotificationData
        {
            get { return m_notify; }
            set { m_notify = value; }
        }

        public ConfigurationData(string name)
        {
            m_time1 = m_time2 = new TimeSpan(0, 0, 10);
            m_name = name;
            m_enabled = true;
            m_autoStart = false;
            m_doSubDirs = false;
            m_datDirectory = System.Environment.CurrentDirectory;
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                object o = key.GetValue("");
                m_ibaAnalyserExe = Path.GetFullPath(o.ToString());
            }
            catch
            {
                m_ibaAnalyserExe = iba.Properties.Resources.noIbaAnalyser;
            }
            m_tasks = new List<TaskData>();
            m_id = m_idCounter;
            m_idCounter++;
            m_notify = new NotificationData();
        }

        public void relinkChildData()
        {
            foreach (TaskData task in m_tasks)
            {
                task.ParentConfigurationData = this;
            }
        }

        public object Clone()
        {
            ConfigurationData cd = new ConfigurationData(m_name.Clone() as string);
            foreach (TaskData task in m_tasks)
                cd.m_tasks.Add(task.Clone() as TaskData);
            cd.relinkChildData();
            cd.m_ibaAnalyserExe = m_ibaAnalyserExe;
            cd.m_enabled = m_enabled;
            cd.m_autoStart = m_autoStart;
            cd.m_datDirectory = m_datDirectory.Clone() as string;
            cd.m_doSubDirs = m_doSubDirs;
            cd.m_time1 = m_time1;
            cd.m_time2 = m_time2;
            cd.m_notify = m_notify.Clone() as NotificationData;
            return cd;
        }

        public int CompareTo(ConfigurationData other)
        {
            return m_id.CompareTo(other.m_id);
        }
    }

    [Serializable]
    public class NotificationData
    {
        private string m_email;
        public string Email
        {
            get { return m_email; }
            set { m_email = value; }
        }
        private string m_host;
        public string Host
        {
            get { return m_host; }
            set { m_host = value; }
        }
        private string m_smtpServer;
        public string SMTPServer
        {
            get { return m_smtpServer; }
            set { m_smtpServer = value; }
        }

        public enum NotifyOutputChoice { EMAIL, NETSEND };
        private NotifyOutputChoice m_outputChoice;
        public NotifyOutputChoice NotifyOutput
        {
            get { return m_outputChoice; }
            set { m_outputChoice = value; }
        }

        private TimeSpan m_time;
        [XmlIgnore]
        public TimeSpan TimeInterval
        {
            get { return m_time; }
            set { m_time = value; }
        }

        public long TimeIntervalTicks
        {
            get { return m_time.Ticks; }
            set { m_time = new TimeSpan(value); }
        }

        private bool m_notifyImmediately;
        public bool NotifyImmediately
        {
            get { return m_notifyImmediately; }
            set { m_notifyImmediately = value; }
        }

        public NotificationData()
        {
            m_email = "";
            m_host = "";
            m_smtpServer = "";
            m_time = new TimeSpan(0, 10, 0);
            m_outputChoice = NotifyOutputChoice.EMAIL;
            m_notifyImmediately = true;
        }

        public object Clone()
        {
            NotificationData nd = new NotificationData();
            nd.m_time = m_time;
            nd.m_email = m_email;
            nd.m_host = m_host;
            nd.m_notifyImmediately = m_notifyImmediately;
            nd.m_outputChoice = m_outputChoice;
            nd.m_smtpServer = m_smtpServer;
            return nd;
        }
    }
}
