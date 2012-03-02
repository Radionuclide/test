using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using iba.Data;
using System.Net.Sockets;
using iba.Utility;
using iba.Plugins;
using System.Runtime.InteropServices;

namespace iba.Processing
{
    public class TaskManager : MarshalByRefObject
    {
        SortedDictionary<ConfigurationData, ConfigurationWorker> m_workers;
        
        virtual public void AddConfiguration(ConfigurationData data)
        {
            ConfigurationWorker cw = new ConfigurationWorker(data);
            lock (m_workers)
            {
                m_workers.Add(data, cw);
            }
        }

        virtual public void AddConfigurations(List<ConfigurationData> datas)
        {
            foreach (ConfigurationData data in datas)
                AddConfiguration(data);
        }

        virtual public void RemoveConfiguration(ConfigurationData data)
        {
            lock (m_workers)
            {
                if (m_workers.ContainsKey(data))
                {
                    StopConfiguration(data);
                    m_workers.Remove(data);
                }
            }
        }

        virtual public void ReplaceConfiguration(ConfigurationData data)
        {
            lock (m_workers)
            {
                ConfigurationWorker cw;
                if (m_workers.TryGetValue(data,out cw))
                {
                    m_workers.Remove(data); //data sorted on ID, remove it as we'll insert a
                    // new data with same ID
                    m_workers.Add(data, cw);
                }
                //else, ignore, replace is due to a belated save of an allready deleted configuration
            }
        }

        private void ReplaceOrAddConfiguration(ConfigurationData data)
        {
            lock (m_workers)
            {
                ConfigurationWorker cw;
                if (m_workers.TryGetValue(data, out cw))
                {
                    m_workers.Remove(data); //data sorted on ID, remove it as we'll insert a
                    // new data with same ID
                    m_workers.Add(data, cw);
                }
                else
                {
                    cw = new ConfigurationWorker(data);
                    m_workers.Add(data, cw);
                }
            }
        }

        virtual public void ReplaceConfigurations(List<ConfigurationData> datas)
        {
            foreach (ConfigurationData data in datas) ReplaceOrAddConfiguration(data);
            List<ConfigurationData> toRemove = new List<ConfigurationData>();
            //remove spurious configurations;
            foreach (ConfigurationData dat in m_workers.Keys)
            {
                if (!datas.Contains(dat)) //contains works because we've allready replaced all datas (it would fail otherwise because contains
                    // uses  equality comparer instead of CompareTo)
                    toRemove.Add(dat);
            }
            foreach (ConfigurationData dat in toRemove)
            {
                RemoveConfiguration(dat);
            }
        }

        virtual public void UpdateConfiguration(ConfigurationData data)
        {
            lock (m_workers)
            {
                try
                {
                    ConfigurationWorker cw = m_workers[data];
                    cw.ConfigurationToUpdate = data;
                    m_workers.Remove(data);
                    m_workers.Add(data, cw);
                    cw.Signal();
                }
                catch (KeyNotFoundException)
                {
                    LogData.Data.Logger.Log("key not found");
                    //doesn't matter
                }
            }
        }

        virtual public bool CompareConfiguration(ConfigurationData data)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == data.Guid)
                    return SerializableObjectsCompare.Compare(data, pair.Key);
            }
            return false;
        }

        virtual public void UpdateTreePosition(Guid guid, int pos)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid)
                {
                    pair.Key.TreePosition = pos;
                    if (pair.Value.ConfigurationToUpdate != null)
                        pair.Value.ConfigurationToUpdate.TreePosition = pos;
                    if (pair.Value.RunningConfiguration != null)
                        pair.Value.RunningConfiguration.TreePosition = pos;
                    if (pair.Value.Status.CorrConfigurationData != null)
                        pair.Value.Status.CorrConfigurationData.TreePosition = pos;
                    return;
                }
            }
        }

        virtual public void ClearConfigurations()
        {
            StopAndWaitForAllConfigurations();
            lock (m_workers)
            {
                m_workers.Clear();
            }
        }

        virtual public void StartAllEnabledConfigurations()
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                if (kvp.Key.Enabled)
                    StartConfiguration(kvp.Key);
        }

        virtual public void StopAllConfigurations()
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                StopConfiguration(kvp.Key);
        }

        virtual public void StartConfiguration(ConfigurationData data)
        {
            m_workers[data].ConfigurationToUpdate = data;
            m_workers[data].Start();
        }

        virtual public void StopConfiguration(ConfigurationData data)
        {
            m_workers[data].Stop = true;
        }

        virtual public void StopConfiguration(Guid guid)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid)
                {
                    pair.Value.Stop = true;
                    return;
                }
            }
        }

        virtual public void StopAndWaitForConfiguration(ConfigurationData data)
        {
            ConfigurationWorker worker = m_workers[data];
            worker.Stop = true;
            worker.Join(60000);
        }

        virtual public void StopAndWaitForConfiguration(Guid guid)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid)
                {
                    pair.Value.Stop = true;
                    pair.Value.Join(60000);
                    return;
                }
            }
        }

        virtual public void StopAndWaitForAllConfigurations()
        {
            //WaitDialog waitDialog = new WaitDialog();
            //System.Threading.ThreadPool.QueueUserWorkItem(new WaitCallback(stop), waitDialog);
            //waitDialog.ShowDialog();

            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                StopConfiguration(kvp.Key);
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                kvp.Value.Join(60000);
        }

        virtual public ConfigurationData GetConfigurationFromWorker(Guid guid)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
            {
                if (kvp.Key.Guid == guid)
                {
                    return (kvp.Value.ConfigurationToUpdate ?? kvp.Value.RunningConfiguration) ?? kvp.Key;
                }
            }
            throw new KeyNotFoundException(guid.ToString() + " not found");
        }

        virtual public MinimalStatusData GetMinimalStatus(Guid guid, bool permanentError)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid) return pair.Value.Status.GetMinimalStatusData(permanentError);
            }
            throw new KeyNotFoundException(guid.ToString() + " not found");
        }

        virtual public bool IsJobStarted(Guid guid)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid) return pair.Value.Status.Started;
            }
            return false;
        }
        
        virtual public PluginTaskWorkerStatus GetStatusPlugin(Guid guid, int taskindex)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid && pair.Key.Tasks[taskindex] is CustomTaskData && pair.Value.Status.Started) 
                    return (pair.Value.RunningConfiguration.Tasks[taskindex] as CustomTaskData).Plugin.GetWorker().GetWorkerStatus();
            }
            return null;
        }

        //virtual public StatusData GetStatusCopy(Guid guid)
        //{
        //    foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
        //    {
        //        if (pair.Key.Guid == guid) return pair.Value.Status.Clone();
        //    }
        //    throw new KeyNotFoundException(guid.ToString() + " not found");
        //}

        public enum AlterPermanentFileErrorListWhatToDo { AFTERDELETE, AFTERREFRESH };
        virtual public void AlterPermanentFileErrorList(AlterPermanentFileErrorListWhatToDo todo, Guid guid, List<string> files)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.Guid == guid)
                {
                    if (todo == AlterPermanentFileErrorListWhatToDo.AFTERDELETE)
                        pair.Value.Status.ClearPermanentFileErrorList(files);
                    else
                        pair.Value.MovePermanentFileErrorListToProcessedList(files);
                    return;
                }
            }
            throw new KeyNotFoundException(guid.ToString() + " not found");
        }

        //watchdog data
        private TCPWatchdog m_watchdog;
        public TCPWatchdog WatchDog
        {
            get { return m_watchdog; }        
        }

        virtual public WatchDogData WatchDogData
        {
            get { return m_watchdog.Settings; }
        }

        virtual public string GetWatchdogStatus()
        {
            return m_watchdog.StatusString;
        }   
            
        virtual public void ReplaceWatchdogData(WatchDogData data)
        {
            m_watchdog.Settings = data;
        }

        virtual public bool TestPath(string dir, string user, string pass, out string errormessage, bool createnew)
        {
            return SharesHandler.TestPath(dir, user, pass, out errormessage, createnew);
        }

        //singleton construction
        private static TaskManager theTaskManager=null; 
        //theTaskmanager has a triple purpose
        //1:) If standalone it is simply THE manager
        //2:) When installed as service and connected it is
        //      On the server instance of TaskManager: THE taskmanager
        //      On the client instance of TaskManager: nothing
        //3:) When installed as service and disconnected
        //      On the client instance of TaskManager: A temporary manager, allowing you to set settings that can be uploaded when reconnected
        //      On the server instance of Taskmanager (on condition that server is alive): another manager, that might be downloaded when reconnected
        private static TaskManagerWrapper RemoteTaskManager = null; 
        //wrapper, i.e. this should be returned when running on the client and being connected, 
        // it delegates calls to theTaskManager on the serverside through the communication object

        public TaskManager()
        {
            m_workers = new SortedDictionary<ConfigurationData, ConfigurationWorker>();
            m_watchdog = new TCPWatchdog();
            m_doPostPone = true;
            m_postponeMinutes = 5;
            m_processPriority = (int)System.Diagnostics.ProcessPriorityClass.Normal;
            m_password = "";
        }
        
        public static TaskManager Manager
        {
            get
            {
                if (Program.IsServer) //was set by the service
                    return theTaskManager;
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && RemoteTaskManager != null)
                {
                    return RemoteTaskManager;
                }
                else if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
                {
                    RemoteTaskManager = new TaskManagerWrapper();
                    return RemoteTaskManager;
                }
                else if (theTaskManager == null)
                {
                    theTaskManager = new TaskManager();
                }
                return theTaskManager;
            }

            set
            {
                theTaskManager = value;
            }
        }

        public static TaskManager ClientManager //to be called from client, gets the client manager;
        {
            get
            {
                return theTaskManager;
            }
            set
            {
                theTaskManager = value;
            }
        }

        virtual public List<ConfigurationData> Configurations
        {
            get
            {
                List<ConfigurationData> theC = new List<ConfigurationData>(m_workers.Keys);

                return theC;
            }
            set
            {
                ClearConfigurations();
                AddConfigurations(value);
            }
        }

        public override object InitializeLifetimeService()
        {
            return null; //immortal object
        }

        virtual public int Count
        {
            get { return m_workers.Count; }
        }

        //can't be called remote, so no virtual
        public string GetStatusForWatchdog()
        {
            try
            {
                StringBuilder message = new StringBuilder();
                lock (m_workers)
                {
                    List<ConfigurationData> confs = Configurations;
                    confs.Sort(delegate(ConfigurationData a, ConfigurationData b) { return a.TreePosition.CompareTo(b.TreePosition); });

                    foreach (ConfigurationData conf in confs)
                    {
                        StatusData s = m_workers[conf].Status;
                        message.Append(conf.Name);
                        message.Append(':');
                        message.Append(s.Started ? "started," : "stopped,");
                        message.Append(s.ReadFiles.Count);
                        message.Append(" todo,");
                        message.Append(s.ProcessedFiles.Count);
                        message.Append(" done,");
                        message.Append(s.CountErrors());
                        message.Append(" failed,");
                        message.Append(s.PermanentErrorFiles.Count);
                        message.Append(" perm. failed;");
                    }
                }
                return message.ToString();
            }
            catch
            {
                return null;
            }
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct BinaryWatchdogMessageLine
        {
            public int state;
            public int todoCount;
            public int doneCount;
            public int errorCount;
            public int permErrorCount;
            public int reserved1;
            public int reserved2;
        }

        [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
        struct BinaryWatchdogMessage
        {
            public int counter;
            public int version;
            public int reserved1;
            public int reserved2;

            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValArray, SizeConst = 16)]
            public BinaryWatchdogMessageLine[] jobs;
        }

        private int m_watchdogCounter;

        public unsafe byte[] GetStatusForWatchdogBinary()
        {
            try
            {
                BinaryWatchdogMessage pMsg = new BinaryWatchdogMessage();

                pMsg.counter = m_watchdogCounter++;
                pMsg.version = 1; //version 1
                pMsg.reserved1 = 0;
                pMsg.reserved2 = 0;
                pMsg.jobs = new BinaryWatchdogMessageLine[16];
                lock (m_workers)
                {
                    List<ConfigurationData> confs = Configurations;
                    confs.Sort(delegate(ConfigurationData a, ConfigurationData b) { return a.TreePosition.CompareTo(b.TreePosition); });
                    for (int i = 0; i < Math.Min(16, confs.Count); i++)
                    {
                        StatusData s = m_workers[confs[i]].Status;
                        pMsg.jobs[i].state = s.Started ? 1 : 0;
                        pMsg.jobs[i].todoCount = s.ReadFiles.Count;
                        pMsg.jobs[i].doneCount = s.ProcessedFiles.Count;
                        pMsg.jobs[i].errorCount = s.CountErrors();
                        pMsg.jobs[i].permErrorCount = s.PermanentErrorFiles.Count;
                        pMsg.jobs[i].reserved1 = 0;
                        pMsg.jobs[i].reserved2 = 0;
                    }
                    for (int i = confs.Count; i < 16; i++)
                    {
                        pMsg.jobs[i].state = 0;
                        pMsg.jobs[i].todoCount = 0;
                        pMsg.jobs[i].doneCount = 0;
                        pMsg.jobs[i].errorCount = 0;
                        pMsg.jobs[i].permErrorCount = 0;
                        pMsg.jobs[i].reserved1 = 0;
                        pMsg.jobs[i].reserved2 = 0;
                    }
                }

                byte[] answer = new byte[Marshal.SizeOf(typeof(BinaryWatchdogMessage))];
                fixed (byte* pAnswer = &answer[0])
                {
                    Marshal.StructureToPtr(pMsg, new IntPtr(pAnswer), false);
                }
                return answer;
            }
            catch
            {
                return null;
            }
        }


        bool m_doPostPone;
        virtual public bool DoPostponeProcessing
        {
            get { return m_doPostPone; }
            set { m_doPostPone = value; }
        }

        int m_postponeMinutes;
        virtual public int PostponeMinutes
        {
            get { return m_postponeMinutes; }
            set { m_postponeMinutes = value; }
        }

        int m_processPriority;
        virtual public int ProcessPriority
        {
            get { return m_processPriority; }
            set { 
                m_processPriority = value;
                System.Diagnostics.ProcessPriorityClass pc = (System.Diagnostics.ProcessPriorityClass) value;
                if (Program.IsServer || Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
                {
                    try
                    {
                        System.Diagnostics.Process.GetCurrentProcess().PriorityClass = pc;
                    }
                    catch
                    { }
                }
            }
        }

        string m_password;
        virtual public string Password
        {
            get { return m_password; }
            set { m_password = value; }
        }

        virtual public void AdditionalFileNames(List<KeyValuePair<string, string>> myList)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key != null)
                    pair.Key.AdditionalFileNames(myList);
            }
        }
    }


    //for remote calls
    public class TaskManagerWrapper : TaskManager
    {
        public override void AddConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.AddConfiguration(data);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.AddConfiguration(data);
            }
        }

        public override void AddConfigurations(List<ConfigurationData> datas)
        {
            try
            {
                Program.CommunicationObject.Manager.AddConfigurations(datas);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.AddConfigurations(datas);
            }
        }

        public override void ClearConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.ClearConfigurations();
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.ClearConfigurations();
            }
        }

        public override List<ConfigurationData> Configurations
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.Configurations;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.Configurations;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.Configurations = value;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    Manager.Configurations = value;
                }
            }
        }

        public override WatchDogData WatchDogData
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.WatchDogData;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.WatchDogData;
                }
            }
        }

        public override string GetWatchdogStatus()
        {
            try
            {
                return Program.CommunicationObject.Manager.GetWatchdogStatus();
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return Manager.GetWatchdogStatus();
            }
        }

        public override int Count
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.Count;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.Count;
                }
            }
        }

        //public override StatusData GetStatus(Guid guid)
        //{
        //    try
        //    {
        //        //if remote, get a copy instead of the status itself
        //        return Program.CommunicationObject.Manager.GetStatusCopy(guid);
        //    }
        //    catch (SocketException)
        //    {
        //        Program.CommunicationObject.HandleBrokenConnection();
        //        return Manager.GetStatus(guid);
        //    }
        //}

        public override bool IsJobStarted(Guid guid)
        {
            try
            {
                return Program.CommunicationObject.Manager.IsJobStarted(guid);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return Manager.IsJobStarted(guid);
            }
        }

        public override ConfigurationData GetConfigurationFromWorker(Guid guid)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetConfigurationFromWorker(guid);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return Manager.GetConfigurationFromWorker(guid);
            }
        }

        public override MinimalStatusData GetMinimalStatus(Guid guid, bool permanentError)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetMinimalStatus(guid, permanentError); 
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return Manager.GetMinimalStatus(guid,permanentError);
            }
        }

        public override PluginTaskWorkerStatus GetStatusPlugin(Guid guid, int taskindex)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetStatusPlugin(guid,taskindex);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return Manager.GetStatusPlugin(guid,taskindex);
            }
        }

        //public override StatusData GetStatusCopy(Guid guid)
        //{
        //    try
        //    {
        //        return Program.CommunicationObject.Manager.GetStatusCopy(guid);
        //    }
        //    catch (SocketException)
        //    {
        //        Program.CommunicationObject.HandleBrokenConnection();
        //        return Manager.GetStatusCopy(guid);
        //    }
        //}

        public override void AlterPermanentFileErrorList(TaskManager.AlterPermanentFileErrorListWhatToDo todo, Guid guid, List<string> files)
        {
            try
            {
                Program.CommunicationObject.Manager.AlterPermanentFileErrorList(todo, guid,files);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void RemoveConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.RemoveConfiguration(data);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.RemoveConfiguration(data);
            }
        }

        public override void ReplaceConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceConfiguration(data);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.ReplaceConfiguration(data);
            }
        }

        public override void ReplaceConfigurations(List<ConfigurationData> datas)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceConfigurations(datas);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.ReplaceConfigurations(datas);
            }
        }

        public override void UpdateConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.UpdateConfiguration(data);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.UpdateConfiguration(data);
            }
        }

        public override void ReplaceWatchdogData(WatchDogData data)
        {
            try
            {
                Program.CommunicationObject.Manager.ReplaceWatchdogData(data);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.ReplaceWatchdogData(data);
            }
        }

        public override bool CompareConfiguration(ConfigurationData data)
        {
            try
            {
                return Program.CommunicationObject.Manager.CompareConfiguration(data);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                return false;
            }
        }

        public override void StartAllEnabledConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.StartAllEnabledConfigurations();
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StartConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.StartConfiguration(data);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }                        

        public override void StopAllConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.StopAllConfigurations();
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopAndWaitForAllConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.StopAndWaitForAllConfigurations();
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopAndWaitForConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.StopAndWaitForConfiguration(data);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopAndWaitForConfiguration(Guid guid)
        {
            try
            {
                Program.CommunicationObject.Manager.StopAndWaitForConfiguration(guid);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopConfiguration(ConfigurationData data)
        {
            try
            {
                Program.CommunicationObject.Manager.StopConfiguration(data);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void StopConfiguration(Guid guid)
        {
            try
            {
                Program.CommunicationObject.Manager.StopConfiguration(guid);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override void UpdateTreePosition(Guid guid, int pos)
        {
            try
            {
                Program.CommunicationObject.Manager.UpdateTreePosition(guid, pos);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
            }
        }

        public override bool TestPath(string dir, string user, string pass, out string errormessage, bool createnew)
        {
            try
            {
                return Program.CommunicationObject.Manager.TestPath(dir, user, pass, out errormessage, createnew);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.HandleBrokenConnection();
                errormessage = iba.Properties.Resources.CouldNotTestPath;
                return false;
            }
        }

        public override bool DoPostponeProcessing
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.DoPostponeProcessing;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.DoPostponeProcessing;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.DoPostponeProcessing = value;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    Manager.DoPostponeProcessing = value;
                }
            }
        }

        public override int PostponeMinutes
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.PostponeMinutes;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.PostponeMinutes;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.PostponeMinutes = value;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    Manager.PostponeMinutes = value;
                }
            }
        }

       public override int ProcessPriority
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.ProcessPriority;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    return Manager.ProcessPriority;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.ProcessPriority = value;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.HandleBrokenConnection();
                    Manager.ProcessPriority = value;
                }
            }
        }

       public override void AdditionalFileNames(List<KeyValuePair<string, string>> myList)
       {
            try
            {
                Program.CommunicationObject.Manager.AdditionalFileNames(myList);
            }
            catch (SocketException)
            {
                myList.Clear();
                Program.CommunicationObject.HandleBrokenConnection();
                Manager.AdditionalFileNames(myList);
            }
        }

       public override string Password
       {
           get
           {
               try
               {
                   return Program.CommunicationObject.Manager.Password;
               }
               catch (SocketException)
               {
                   Program.CommunicationObject.HandleBrokenConnection();
                   return "";
               }
           }
           set
           {
               try
               {
                  Program.CommunicationObject.Manager.Password = value;
               }
               catch (SocketException)
               {
                   Program.CommunicationObject.HandleBrokenConnection();
               }
           }
       }
    }
}
