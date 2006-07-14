using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using iba.Data;
using System.Net.Sockets;

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
                try
                {
                    ConfigurationWorker cw = m_workers[data];
                    cw.ConfigurationToUpdate = data;
                    m_workers.Remove(data);
                    m_workers.Add(data, cw);
                }
                catch (KeyNotFoundException)
                {
                    //doesn't matter
                }
            }
        }

        virtual public void ReplaceWatchdogData(WatchDogData data)
        {
            m_watchdogData = data;
        }

        virtual public void ClearConfigurations()
        {
            StopAndWaitForAllConfigurations();
            lock (m_workers)
            {
                m_workers.Clear();
            }
        }

        virtual public void StartAllConfigurations()
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                StartConfiguration(kvp.Key);
        }

        virtual public void StopAllConfigurations()
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                StopConfiguration(kvp.Key);
        }

        virtual public void StartConfiguration(ConfigurationData data)
        {
            m_workers[data].Start();
        }

        virtual public void StopConfiguration(ConfigurationData data)
        {
            m_workers[data].Stop = true;
        }

        virtual public void StartConfiguration(ulong ID)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.ID == ID)
                {
                    pair.Value.Start();
                    return;
                }
            }
        }

        virtual public ulong IdCounter
        {
            get
            {
                return ConfigurationData.IdCounter;
            }
            set
            {
                ConfigurationData.IdCounter = value;
            }
        }

        virtual public void StopConfiguration(ulong ID)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.ID == ID)
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

        virtual public void StopAndWaitForConfiguration(ulong ID)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.ID == ID)
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

        virtual public StatusData GetStatus(ConfigurationData data)
        {
            return m_workers[data].Status;
        }

        virtual public StatusData GetStatus(ulong ID)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.ID == ID) return pair.Value.Status;
            }
            throw new KeyNotFoundException(ID.ToString() + " not found");
        }

        virtual public StatusData GetStatusCopy(ulong ID)
        {
            foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> pair in m_workers)
            {
                if (pair.Key.ID == ID) return pair.Value.Status.Clone();
            }
            throw new KeyNotFoundException(ID.ToString() + " not found");
        }

        //watchdog data
        private WatchDogData m_watchdogData;
        virtual public WatchDogData WatchDogData
        {
            get { return m_watchdogData; }
        }


        //singleton construction
        private static TaskManager theTaskManager=null;
        private static TaskManagerWrapper RemoteTaskManager = null;

        public TaskManager()
        {
            m_workers = new SortedDictionary<ConfigurationData, ConfigurationWorker>();
            m_watchdogData = new WatchDogData();
        }
        
        public static TaskManager Manager
        {
            get
            {
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

        virtual public List<StatusData> Statuses
        {
            get 
            {
                List<StatusData> theStati = new List<StatusData>();
                foreach (KeyValuePair<ConfigurationData, ConfigurationWorker> kvp in m_workers)
                    theStati.Add(kvp.Value.Status);
                return theStati;
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
            return "booha";
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
                Program.CommunicationObject.handleBrokenConnection();
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
                Program.CommunicationObject.handleBrokenConnection();
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
                Program.CommunicationObject.handleBrokenConnection();
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
                    Program.CommunicationObject.handleBrokenConnection();
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
                    Program.CommunicationObject.handleBrokenConnection();
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
                    Program.CommunicationObject.handleBrokenConnection();
                    return Manager.WatchDogData;
                }
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
                    Program.CommunicationObject.handleBrokenConnection();
                    return Manager.Count;
                }
            }
        }

        public override StatusData GetStatus(ConfigurationData data)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetStatus(data);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.handleBrokenConnection();
                return Manager.GetStatus(data);
            }
        }

        public override StatusData GetStatus(ulong ID)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetStatus(ID);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.handleBrokenConnection();
                return Manager.GetStatus(ID);
            }
        }

        public override StatusData GetStatusCopy(ulong ID)
        {
            try
            {
                return Program.CommunicationObject.Manager.GetStatusCopy(ID);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.handleBrokenConnection();
                return Manager.GetStatusCopy(ID);
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
                Program.CommunicationObject.handleBrokenConnection();
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
                Program.CommunicationObject.handleBrokenConnection();
                Manager.ReplaceConfiguration(data);
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
                Program.CommunicationObject.handleBrokenConnection();
                Manager.ReplaceWatchdogData(data);
            }
        }

        
        public override void StartAllConfigurations()
        {
            try
            {
                Program.CommunicationObject.Manager.StartAllConfigurations();
            }
            catch (SocketException)
            {
                Program.CommunicationObject.handleBrokenConnection();
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
                Program.CommunicationObject.handleBrokenConnection();
            }
        }                        

        public override void StartConfiguration(ulong ID)
        {
            try
            {
                Program.CommunicationObject.Manager.StartConfiguration(ID);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.handleBrokenConnection();
            }
        }

        public override ulong IdCounter
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.IdCounter;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.handleBrokenConnection();
                    return Manager.IdCounter;
                }
            }
            set
            {
                try
                {
                    Program.CommunicationObject.Manager.IdCounter = value;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.handleBrokenConnection();
                    Manager.IdCounter = value;
                }
            }
        }

        public override List<StatusData> Statuses
        {
            get
            {
                try
                {
                    return Program.CommunicationObject.Manager.Statuses;
                }
                catch (SocketException)
                {
                    Program.CommunicationObject.handleBrokenConnection();
                    return Manager.Statuses;
                }
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
                Program.CommunicationObject.handleBrokenConnection();
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
                Program.CommunicationObject.handleBrokenConnection();
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
                Program.CommunicationObject.handleBrokenConnection();
            }
        }

        public override void StopAndWaitForConfiguration(ulong ID)
        {
            try
            {
                Program.CommunicationObject.Manager.StopAndWaitForConfiguration(ID);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.handleBrokenConnection();
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
                Program.CommunicationObject.handleBrokenConnection();
            }
        }

        public override void StopConfiguration(ulong ID)
        {
            try
            {
                Program.CommunicationObject.Manager.StopConfiguration(ID);
            }
            catch (SocketException)
            {
                Program.CommunicationObject.handleBrokenConnection();
            }
        }
    }
}
