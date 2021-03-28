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
        public enum JobTypeEnum
        {
            DatTriggered,
            Scheduled,
            OneTime,
            Event
        }

        private JobTypeEnum m_jobType;
        public iba.Data.ConfigurationData.JobTypeEnum JobType
        {
            get { return m_jobType; }
            set { m_jobType = value; }
        }

        private string m_name;
        public string Name
        {
            get 
            { 
                return m_name; 
            }
            set 
            { 
                if (value.Length != 0) 
                    m_name = value; 
            }
        }

        private string m_datDirectory;
        public string DatDirectory
        {
            get 
            {
                if(m_jobType == JobTypeEnum.Scheduled || m_jobType == JobTypeEnum.Event)
                    return HDQDirectory;
                return m_datDirectory;
            }
            set 
            { 
                if (!String.IsNullOrEmpty(value)) m_datDirectory = XMLMultilineTextFixer.Fix(value);
            }
        }

        private string m_hdqDirectory;
        [XmlIgnore]
        public string HDQDirectory
        {
            get
            {
                if(string.IsNullOrEmpty(m_hdqDirectory))
                    DetermineHDQFolder();
                return m_hdqDirectory;
            }
        }

        private List<TaskData> m_tasks;

        
        [XmlElement(Type = typeof(ReportData)), 
        XmlElement(Type = typeof(ExtractData)), 
        XmlElement(Type = typeof(BatchFileData)),
        XmlElement(Type = typeof(CopyMoveTaskData)),
        XmlElement(Type = typeof(IfTaskData)), 
        XmlElement(Type = typeof(UpdateDataTaskData)), 
        XmlElement(Type = typeof(PauseTaskData)),
        XmlElement(Type = typeof(TaskWithTargetDirData)),
        XmlElement(Type = typeof(CleanupTaskData)),
        XmlElement(Type = typeof(SplitterTaskData)),
        XmlElement(Type = typeof(HDCreateEventTaskData)),
        XmlElement(Type = typeof(CustomTaskData)),
        XmlElement(Type = typeof(CustomTaskDataUNC))]
        public List<TaskData> Tasks
        {
            get { return m_tasks; }
            set { m_tasks = value; }
        }

        private string m_datdirectoryUNC;
        public string DatDirectoryUNC
        {
            get 
            {
                if(m_jobType == JobTypeEnum.Scheduled || m_jobType == JobTypeEnum.Event)
                    return HDQDirectory;
                return m_datdirectoryUNC; 
            }
            set 
            {
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

        private string m_fileEncryptionPass;

        [XmlIgnore]
        public string FileEncryptionPassword
        {
            get { return m_fileEncryptionPass; }
            set { m_fileEncryptionPass = value; }
        }

        public string FileEncryptionPasswordCrypted
        {
            get { return Crypt.Encrypt(m_fileEncryptionPass); }
            set { m_fileEncryptionPass = Crypt.Decrypt(value); }
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

        private bool m_bReprocessErrors;
        public bool ReprocessErrors
        {
            get { return m_bReprocessErrors; }
            set { m_bReprocessErrors = value; }
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

        public ConfigurationData() : this("",JobTypeEnum.DatTriggered) 
        {
        }

        private NotificationData m_notify;
        public NotificationData NotificationData
        {
            get { return m_notify; }
            set { m_notify = value; }
        }

        private string ibaAnalyzerExe;

        public bool OnetimeJob
        {
            get { return m_jobType == JobTypeEnum.OneTime; }
            set { if (value) { m_jobType = JobTypeEnum.OneTime; } }
        }

        private ScheduledJobData m_scheduleData;
	    public iba.Data.ScheduledJobData ScheduleData
	    {
		    get { return m_scheduleData; }
		    set { m_scheduleData = value; }
	    }

        private EventJobData m_eventData;
        public iba.Data.EventJobData EventData
        {
            get { return m_eventData; }
            set { m_eventData = value; }
        }

        public ConfigurationData(string name, JobTypeEnum jobType)
        {
            m_jobType = jobType;
            if (m_jobType == JobTypeEnum.Scheduled) m_scheduleData = new ScheduledJobData();
            if (m_jobType == JobTypeEnum.Event) m_eventData = new EventJobData();
            m_reproccessTime = new TimeSpan(0, 10, 0);
            m_rescanTime = new TimeSpan(0, 60, 0);
            m_name = name;
            m_enabled = true;
            m_autoStart = false;
            m_doSubDirs = false;
            m_bInitialScanEnabled = m_jobType == JobTypeEnum.DatTriggered;
            m_bDetectNewFiles = m_jobType == JobTypeEnum.DatTriggered;
            m_bRescanEnabled = m_jobType == JobTypeEnum.DatTriggered;
            m_bReprocessErrors = m_jobType == JobTypeEnum.DatTriggered;
            
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
            m_fileEncryptionPass = "";
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
            ConfigurationData cd = new ConfigurationData(m_name,m_jobType);
            foreach (TaskData task in m_tasks)
                cd.m_tasks.Add(task.Clone() as TaskData);
            if(m_jobType == JobTypeEnum.Scheduled)
                cd.m_scheduleData = m_scheduleData.Clone() as ScheduledJobData;
            if (m_jobType == JobTypeEnum.Event)
                cd.m_eventData = m_eventData.Clone() as EventJobData;
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
            cd.m_bReprocessErrors = m_bReprocessErrors;
            cd.m_datdirectoryUNC = m_datdirectoryUNC;
            cd.m_username = m_username;
            cd.m_pass = m_pass;
            cd.m_bLimitTimesTried = m_bLimitTimesTried;
            cd.m_nrTimes = m_nrTimes;
            cd.m_treePosition = m_treePosition;
            cd.m_fileEncryptionPass = m_fileEncryptionPass;
            return cd;
        }

        public bool IsSame(ConfigurationData other)
        {
            //compares but don't care about transient fields like m_uncPath and tree position
            if (other == null) return false;
            if (other == this) return true;
            if (m_tasks.Count != other.m_tasks.Count) return false;
            for (int i = 0; i < m_tasks.Count; i++)
            {
                if (!other.m_tasks[i].IsSame(m_tasks[i]))
                {
                    System.Diagnostics.Debug.WriteLine(String.Format("Difference in job {0} task {1}", other.Name, other.Name));
                    return false;
                }
            }
            if(m_jobType != other.m_jobType) return false;
            if(m_jobType == JobTypeEnum.Scheduled && !m_scheduleData.IsSame(other.m_scheduleData)) return false;
            if (m_jobType == JobTypeEnum.Event && !m_eventData.IsSame(other.m_eventData)) return false;
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
                other.m_bReprocessErrors == m_bReprocessErrors &&
                //other.m_datdirectoryUNC == m_datdirectoryUNC && //don't care about this one
                other.m_username == m_username &&
                other.m_pass == m_pass &&
                other.m_bLimitTimesTried == m_bLimitTimesTried &&
                //other.m_treePosition == m_treePosition && //don't care about this one
                other.m_nrTimes == m_nrTimes &&
                other.m_fileEncryptionPass == m_fileEncryptionPass;
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

        private void DetermineHDQFolder()
        {
            string p1 = System.IO.Path.GetTempPath();
            string p2 = "{" + Guid.ToString()+ "_" + Name + "}";
            p2 = CPathCleaner.CleanFile(p2);
            string dir = Path.Combine(p1, p2);
            if(!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            m_hdqDirectory = dir;
        }

        private string lastHDQFile;
        private string lastHDQDescription;
        public string CreateHDQFileDescription(string hdqfile)
        {
            if (hdqfile == lastHDQFile) return lastHDQDescription;
            try
            {
                IniParser ini = new IniParser(hdqfile);
                if (!ini.Read()) return hdqfile;

                string eventName = "";
                string comment = ini.Sections["HDQ file"]["comment"];
                if (!string.IsNullOrWhiteSpace(comment) && comment.StartsWith("EVENTNAME:"))
                    eventName = comment.Substring("EVENTNAME:".Length);
                if (!string.IsNullOrWhiteSpace(eventName))
                    eventName += " "; 

                string desc =  ini.Sections["HDQ file"]["store"] + " " + eventName + ini.Sections["HDQ file"]["starttime"] + " - " + ini.Sections["HDQ file"]["stoptime"];
                lastHDQDescription = desc;
                lastHDQFile = hdqfile;
                return desc;
            }
            catch
            {
                return hdqfile;
            }
        }

        public void GenerateHDQFile(DateTime trigger, String path)
        {
            DateTime startTime = trigger - ScheduleData.StartRangeFromTrigger;
            DateTime stopTime = trigger - ScheduleData.StopRangeFromTrigger;
            GenerateHDQFile(startTime, stopTime, path);
        }

        public void GenerateHDQFile(DateTime startTime, DateTime stopTime, String path, string comment = "")
        {
            if (stopTime <= startTime)
            {
                throw new Exception(String.Format(Properties.Resources.HDQErrorStopBeforeStart, startTime, stopTime));
            }
            string lServer = string.Empty;
            int lPort = -1;
            string[] lStores = new string[0];
            bool bPreferredTimeBaseIsAuto = true;
            TimeSpan lPreferredTimeBase = new TimeSpan(0);
            if (JobType == JobTypeEnum.Event)
            {
                lServer = m_eventData.HDServer;
                lPort = m_eventData.HDPort;
                lStores = m_eventData.HDStores;
                bPreferredTimeBaseIsAuto = m_eventData.PreferredTimeBaseIsAuto;
                lPreferredTimeBase = m_eventData.PreferredTimeBase;
            }
            else if (JobType == JobTypeEnum.Scheduled)
            {
                lServer = ScheduleData.HDServer;
                lPort = ScheduleData.HDPort;
                lStores = ScheduleData.HDStores;
                bPreferredTimeBaseIsAuto = ScheduleData.PreferredTimeBaseIsAuto;
                lPreferredTimeBase = ScheduleData.PreferredTimeBase;
            }
            int count = 0;
            string dir = Path.GetDirectoryName(path);
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            using (StreamWriter sw = new StreamWriter(path, false,Encoding.UTF8))
            {
                //Add a section per store because ibaAnalyzer can't handle multiple stores in one hdq file
                for (int i = 0; i < lStores.Length; i++)
                {
                    if (String.IsNullOrEmpty(lStores[i]))
                        continue;
                    count++;
                    if (i == 0)
                        sw.WriteLine("[HDQ file]");
                    else
                        sw.WriteLine($"[HDQ file{i}]");
                    sw.WriteLine("type=time");
                    sw.WriteLine("server=" + lServer);
                    sw.WriteLine("portnumber=" + lPort);
                    sw.WriteLine("store=" + lStores[i]);
                    String temp = startTime.ToString("dd.MM.yyyy HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
                    sw.WriteLine("starttime=" + temp);
                    temp = stopTime.ToString("dd.MM.yyyy HH:mm:ss.ffffff", System.Globalization.CultureInfo.InvariantCulture.DateTimeFormat);
                    sw.WriteLine("stoptime=" + temp);
                    if (!string.IsNullOrWhiteSpace(comment))
                        sw.WriteLine("comment=" + comment);

                    if (!bPreferredTimeBaseIsAuto)
                        sw.WriteLine("timebase=" + lPreferredTimeBase.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    else
                    {
                        long ms = 10000; //10000 * 100 nanosec = 1 ms
                        long s = 1000 * ms;
                        long[] timeBases = new long[] { 1 * ms, 10 * ms, 100 * ms, s, 60 * s, 3600 * s, 24 * 3600 * s };
                        long duration = stopTime.Ticks - startTime.Ticks;
                        TimeSpan tb = TimeSpan.FromDays(1);
                        foreach (long tBase in timeBases)
                        {
                            double samples = ((double)(duration)) / ((double)(tBase)) * Math.Sqrt(40.0);
                            if (samples < 1.0e9)
                            {
                                tb = TimeSpan.FromTicks(tBase);
                                break;
                            }
                        }
                        sw.WriteLine("timebase=" + tb.TotalSeconds.ToString(System.Globalization.CultureInfo.InvariantCulture));
                    }
                }
                if (count == 0)
                    throw new Exception(Properties.Resources.HDQErrorNoStores);
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


        [XmlIgnore]
        public string HdUser
        {
            get
            {
                if (JobType == JobTypeEnum.Scheduled)
                    return m_scheduleData?.HDUsername;
                else if (JobType == JobTypeEnum.Event)
                    return m_eventData?.HDUsername;
                return "";
            }
        }

        [XmlIgnore]
        public string HdPass
        {
            get
            {
                if (JobType == JobTypeEnum.Scheduled)
                    return m_scheduleData?.HDPassword;
                else if (JobType == JobTypeEnum.Event)
                    return m_eventData?.HDPassword;
                return "";
            }
        }

        public bool DatTriggered
		{
			get
			{
				return JobType == ConfigurationData.JobTypeEnum.DatTriggered
				|| JobType == ConfigurationData.JobTypeEnum.OneTime;
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
            //m_sender = "ibaDatCoordinator <noreply@iba-ag.com>";
            m_sender = "";
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
