using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Win32;

using System.Diagnostics;
using iba.Utility;
using iba.Plugins;

namespace iba.Data
{
    [ Serializable ]
    public class ConfigurationData : ICloneable, IComparable<ConfigurationData>, IJobData
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
            get 
            {
                return m_datDirectory;
            }
            set 
            { 
                if (!String.IsNullOrEmpty(value)) m_datDirectory = XMLMultilineTextFixer.Fix(value);
            }
        }

        private List<TaskData> m_tasks;

        [XmlElement(Type = typeof(ReportData)), XmlElement(Type = typeof(ExtractData)), XmlElement(Type = typeof(BatchFileData)), XmlElement(Type = typeof(CopyMoveTaskData)), XmlElement(Type = typeof(IfTaskData)), XmlElement(Type = typeof(UpdateDataTaskData)), XmlElement(Type = typeof(PauseTaskData)), XmlElement(Type = typeof(CustomTaskData)), XmlElement(Type = typeof(CustomTaskDataUNC))]
        public List<TaskData> Tasks
        {
            get { return m_tasks; }
            set { m_tasks = value; }
        }

        private string m_datdirectoryUNC;
        public string DatDirectoryUNC
        {
            get { return m_datdirectoryUNC; }
            set {
                m_datdirectoryUNC = XMLMultilineTextFixer.Fix(value);
            }
        }

        public void UpdateUNC()
        {
            UpdateUNC(false);
        }

        private int m_treePosition;
        public int TreePosition
        {
            get { return m_treePosition; }
            set { m_treePosition = value; }
        }

        public void UpdateUNC(bool updateChildren)
        {
            if (!OnetimeJob)
                m_datdirectoryUNC = Shares.PathToUnc(m_datDirectory, false);
            else
            {
                string[] lines = m_datDirectory.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                StringBuilder sb = new StringBuilder();
                foreach (string line in lines)
                {
                    string uncline = Shares.PathToUnc(line, false);
                    sb.AppendLine(uncline);
                }
                m_datdirectoryUNC = sb.ToString();
            }
            if (updateChildren)
            {
                foreach (TaskData task in m_tasks)
                {
                    TaskDataUNC t = task as TaskDataUNC;
                    if (t != null) t.UpdateUNC();
                }
            }
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
            get { return Crypt.Encrypt(m_pass); }
            set { m_pass = Crypt.Decrypt(value); }
        }

        private string m_username;
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
        }

        private bool m_doSubDirs;
        public bool SubDirs
        {
            get { return m_doSubDirs; }
            set { m_doSubDirs = value; }
        }

        private TimeSpan m_reproccessTime;
        [XmlIgnore]
        public TimeSpan ReprocessErrorsTimeInterval
        {
            get { return m_reproccessTime; }
            set { m_reproccessTime = value; }
        }

        public long ReprocessErrorsTimeIntervalTicks
        {
            get { return m_reproccessTime.Ticks; }
            set { m_reproccessTime = new TimeSpan(value); }
        }

        private bool m_bInitialScanEnabled;
        public bool InitialScanEnabled
        {
            get { return m_bInitialScanEnabled; }
            set { m_bInitialScanEnabled = value; }
        }

        private bool m_bRescanEnabled;
        public bool RescanEnabled
        {
            get { return m_bRescanEnabled; }
            set { m_bRescanEnabled = value; }
        }

        private bool m_bDetectNewFiles;
        public bool DetectNewFiles
        {
            get { return m_bDetectNewFiles; }
            set { m_bDetectNewFiles = value; }
        }
      

        private bool m_bLimitTimesTried;
        public bool LimitTimesTried
        {
            get { return m_bLimitTimesTried; }
            set { m_bLimitTimesTried = value; }
        }

        private int m_nrTimes;
        public int NrTryTimes
        {
            get { return m_nrTimes; }
            set { m_nrTimes = value; }
        }
        
        private TimeSpan m_rescanTime;
        [XmlIgnore]
        public TimeSpan RescanTimeInterval
        {
            get { return m_rescanTime; }
            set { m_rescanTime = value; }
        }

        public long RescanTimeIntervalTicks
        {
            get { return m_rescanTime.Ticks; }
            set { m_rescanTime = new TimeSpan(value); }
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

        private Guid m_guid;
        public Guid Guid
        {
            get { return m_guid; }
            set { m_guid = value; }
        }

        public ConfigurationData() : this("",false) 
        {

        }

        private NotificationData m_notify;
        public NotificationData NotificationData
        {
            get { return m_notify; }
            set { m_notify = value; }
        }

        private string ibaAnalyzerExe;


        private bool m_onetimeJob;
        public bool OnetimeJob
        {
            get { return m_onetimeJob; }
            set { m_onetimeJob = value; }
        }


        public ConfigurationData(string name, bool onetimejob)
        {
            m_onetimeJob = onetimejob;
            m_reproccessTime = new TimeSpan(0, 10, 0);
            m_rescanTime = new TimeSpan(0, 60, 0);
            m_name = name;
            m_enabled = true;
            m_autoStart = false;
            m_doSubDirs = false;
            m_bInitialScanEnabled = !onetimejob;
            m_bDetectNewFiles = !onetimejob;
            m_bRescanEnabled = !onetimejob;
            
            m_datDirectory = System.Environment.CurrentDirectory;
            try
            {
                RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                object o = key.GetValue("");
                ibaAnalyzerExe = Path.GetFullPath(o.ToString());
            }
            catch
            {
                ibaAnalyzerExe = iba.Properties.Resources.noIbaAnalyser;
            }
            m_tasks = new List<TaskData>();
            m_notify = new NotificationData();
            m_pass = "";
            m_username = "";
            m_datdirectoryUNC = "";
            m_treePosition = -1;
            m_guid = Guid.NewGuid();
            m_bLimitTimesTried = false;
            m_nrTimes = 10;
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
            ConfigurationData cd = new ConfigurationData(m_name,m_onetimeJob);
            foreach (TaskData task in m_tasks)
                cd.m_tasks.Add(task.Clone() as TaskData);
            cd.relinkChildData();
            cd.m_enabled = m_enabled;
            cd.m_autoStart = m_autoStart;
            cd.m_datDirectory = m_datDirectory;
            cd.m_doSubDirs = m_doSubDirs;
            cd.m_bDetectNewFiles = m_bDetectNewFiles;
            cd.m_reproccessTime = m_reproccessTime;
            cd.m_bInitialScanEnabled = m_bInitialScanEnabled;
            cd.m_rescanTime = m_rescanTime;
            cd.m_notify = m_notify.Clone() as NotificationData;
            cd.m_bRescanEnabled = m_bRescanEnabled;
            cd.m_datdirectoryUNC = m_datdirectoryUNC;
            cd.m_username = m_username;
            cd.m_pass = m_pass;
            cd.m_bLimitTimesTried = m_bLimitTimesTried;
            cd.m_nrTimes = m_nrTimes;
            cd.m_treePosition = m_treePosition;
            return cd;
        }

        public bool IsSame(ConfigurationData other)
        {
            //compares but don't care about transient fields like m_uncPath and treepostion
            if (other == null) return false;
            if (other == this) return true;
            if (m_tasks.Count != other.m_tasks.Count) return false;
            for (int i = 0; i < m_tasks.Count; i++)
            {
                if (!other.m_tasks[i].IsSame(m_tasks[i])) return false;
            }

            return
            other.m_enabled == m_enabled &&
            other.m_autoStart == m_autoStart &&
            other.m_datDirectory == m_datDirectory &&
            other.m_doSubDirs == m_doSubDirs &&
            other.m_bDetectNewFiles == m_bDetectNewFiles &&
            other.m_reproccessTime == m_reproccessTime &&
            other.m_bInitialScanEnabled == m_bInitialScanEnabled &&
            other.m_rescanTime == m_rescanTime &&
            other.m_notify.IsSame(m_notify) &&
            other.m_bRescanEnabled == m_bRescanEnabled &&
                //other.m_datdirectoryUNC == m_datdirectoryUNC && //don't care about this one
            other.m_username == m_username &&
            other.m_pass == m_pass &&
            other.m_bLimitTimesTried == m_bLimitTimesTried &&
                //other.m_treePosition == m_treePosition; //don't care about this one
            other.m_nrTimes == m_nrTimes;
        }

        public ConfigurationData Clone_AlsoCopyGuids()
        {
            ConfigurationData cd = this.Clone() as ConfigurationData;
            cd.m_guid = m_guid;
            for (int i = 0; i < m_tasks.Count; i++)
                cd.m_tasks[i].Guid = m_tasks[i].Guid;
            return cd;
        }


        public int CompareTo(ConfigurationData other)
        {
            return m_guid.CompareTo(other.m_guid);
        }

        public bool AdjustDependencies()
        {
            if (Tasks.Count == 0) return false;
            bool changed = false;
            if (Tasks[0].WhenToExecute != TaskData.WhenToDo.DISABLED && Tasks[0].WhenToExecute != TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE)
            {
                Tasks[0].WhenToExecute = TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
                changed = true;
            }
            for (int i = 1; i < Tasks.Count; i++)
            {
                if (Tasks[i - 1].WhenToExecute == TaskData.WhenToDo.DISABLED && Tasks[i].WhenToExecute != TaskData.WhenToDo.DISABLED && Tasks[i].WhenToExecute != TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE)
                {
                    Tasks[i].WhenToExecute = TaskData.WhenToDo.AFTER_SUCCES_OR_FAILURE;
                    changed = true;
                }
            }
            return changed;
        }

        public void AdditionalFileNames(List<KeyValuePair<string, string>> myList)
        {
            string safeConfName = Utility.PathUtil.FilterInvalidFileNameChars(m_name);
            foreach (TaskData task in m_tasks)
            {
                if (task != null)
                    task.AdditionalFileNames(myList, safeConfName);
            }
        }

        #region IJobData Members

        [XmlIgnore]
        public string IbaAnalyzerExe
        {
            get
            {
                return ibaAnalyzerExe;
            }
            set
            {
                ibaAnalyzerExe = value;
            }
        }

        #endregion

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

        private string m_sender;
        public string Sender
        {
            get { return m_sender; }
            set { m_sender = value; }
        }

        private bool m_authenticationRequired;
        public bool AuthenticationRequired
        {
            get { return m_authenticationRequired; }
            set { m_authenticationRequired = value; }
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
            get { return Crypt.Encrypt(m_pass); }
            set { m_pass = Crypt.Decrypt(value); }
        }
        
        private string m_username;
        public string Username
        {
            get { return m_username; }
            set { m_username = value; }
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
            m_sender = "ibaDatCoordinator <noreply@iba-ag.com>";
            m_email = "";
            m_host = "";
            m_smtpServer = "";
            m_time = new TimeSpan(0, 10, 0);
            m_outputChoice = NotifyOutputChoice.EMAIL;
            m_notifyImmediately = true;
            m_username = "";
            m_pass = "";
            m_authenticationRequired = false;
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
            nd.m_authenticationRequired = m_authenticationRequired;
            nd.m_pass = m_pass;
            nd.m_username = m_username;
            nd.m_sender = m_sender;
            return nd;
        }

        public bool IsSame(NotificationData other)
        {
            return other.m_time == m_time &&
            other.m_email == m_email &&
            other.m_host == m_host &&
            other.m_notifyImmediately == m_notifyImmediately &&
            other.m_outputChoice == m_outputChoice &&
            other.m_smtpServer == m_smtpServer &&
            other.m_authenticationRequired == m_authenticationRequired &&
            other.m_pass == m_pass &&
            other.m_username == m_username &&
            other.m_sender == m_sender;
        }
    }
}
