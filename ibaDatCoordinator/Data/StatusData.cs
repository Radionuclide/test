using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using iba.Utility;

namespace iba.Data
{
    [Serializable]
    public class DatFileStatus: ICloneable
    {
        public enum State 
        {
            NOT_STARTED=0,
            RUNNING=1,
            TIMED_OUT=2,
            MEMORY_EXCEEDED = 3,
            COMPLETED_SUCCESFULY=4,
            COMPLETED_FAILURE = 5,
            NO_ACCESS = 6,
            COMPLETED_TRUE = 7,
            COMPLETED_FALSE = 8,
            TRIED_TOO_MANY_TIMES = 9,
            INVALID_DATFILE = 10
        }

        private int m_timesTried;
        public int TimesTried
        {
            get { return m_timesTried; }
            set { m_timesTried = value; }
        }

        static public bool IsError(State stat)
        {
            return stat == State.TRIED_TOO_MANY_TIMES || 
                            stat == State.TIMED_OUT ||
                            stat == State.MEMORY_EXCEEDED ||
                            stat == State.COMPLETED_FAILURE || 
                            stat == State.NO_ACCESS;
        }

        private STLLikeMap<TaskData, State> m_states;
        public STLLikeMap<TaskData, State> States
        {
            get { return m_states; }
            set { m_states = value; }
        }

        private SortedList<TaskData,string> m_outputFiles;
        public SortedList<TaskData, string> OutputFiles
        {
            get { return m_outputFiles; }
            set { m_outputFiles = value; }
        }

        public DatFileStatus()
        {
            m_states = new STLLikeMap<TaskData,State>();
            m_outputFiles = new SortedList<TaskData,string>();
            m_timesTried = 0;
        }

        public object Clone()
        {
            DatFileStatus copy = new DatFileStatus();
            foreach (KeyValuePair<TaskData, State> pair in m_states)
                copy.m_states.Add(pair.Key, pair.Value);
            copy.m_timesTried = m_timesTried;
            foreach(KeyValuePair<TaskData,string> pair in m_outputFiles)
                copy.m_outputFiles.Add(pair.Key,pair.Value);
            return copy;
        }
    }

    [Serializable]
    public class StatusData
    {
        private STLLikeMap<string, DatFileStatus> m_datFileStates;
        public STLLikeMap<string, DatFileStatus> DatFileStates
        {
            get { m_changed = true; return m_datFileStates; }
            set { m_changed = true; m_datFileStates = value; }
        }

        private ConfigurationData m_cf; //corresponding configuration
        public ConfigurationData CorrConfigurationData
        {
            get { return m_cf; }
            set {
                if (m_cf != null && (value.LimitTimesTried != m_cf.LimitTimesTried || m_cf.NrTryTimes < value.NrTryTimes))
                {
                    UpdatePermanentFileErrorList(value,null);
                }
                m_cf = value; 
            }
        }

        public void ClearPermanentFileErrorList(List<string> files)
        {
            foreach (string filename in files)
            {
                lock (m_datFileStates)
                {
                    m_datFileStates.Remove(filename);
                }
                lock (m_permanentErrorFiles)
                {
                    m_permanentErrorFiles.Remove(filename);
                }
                lock (m_permanentErrorFilesCopy)
                {
                    m_permanentErrorFilesCopy.Remove(filename);
                }
            }
            PermanentErrorFilesChanged = true;
        }

        //folowing methods should be called with ConfigurationWorkers m_updatelistlock locked
        public void MovePermanentFileErrorListToProcessedList(List<string> files)
        {
            UpdatePermanentFileErrorList(null,files); //moves them all, resets count
        }

        private void UpdatePermanentFileErrorList(ConfigurationData newConf,List<string> files)
        {
            UpdatingFileList = true;
            bool bMoveAll = (newConf == null) || !newConf.LimitTimesTried;
            lock (m_processedFiles)
            {
                lock (m_permanentErrorFiles)
                {
                    for(int i = m_permanentErrorFiles.Count-1; i >= 0; i--)
                    {
                        string filename = m_permanentErrorFiles[i];
                        if (files != null && !files.Contains(filename)) continue;
                        lock (m_datFileStates)
                        {
                            DatFileStatus df = m_datFileStates[filename];
                            if (newConf == null) df.TimesTried = 0;
                            if (bMoveAll || df == null || df.TimesTried < newConf.NrTryTimes)
                            {
                                if (!m_processedFiles.Contains(filename))
                                    m_processedFiles.Add(filename);
                                m_permanentErrorFiles.RemoveAt(i);
                            }
                        }
                    }
                }
            }
            MergeProcessedAndToProcessLists();
            UpdatingFileList = false;
            PermanentErrorFilesChanged = true;
        }

        private bool m_started;
        public bool Started
        {
            get { return m_started; }
            set
            {
                if (value != m_started)
                {
                    m_started = value;
                    m_changed = true;
                }
            }
        }

        public int CountErrors()
        {
            int count = 0;
            lock (m_datFileStates)
            {
                foreach (DatFileStatus stat in m_datFileStates.Values)
                {
                    foreach (KeyValuePair<TaskData, DatFileStatus.State> taskstat in stat.States)
                    {
                        if (taskstat.Key != null && taskstat.Key.Enabled && DatFileStatus.IsError(taskstat.Value))
                            count++;
                    }
                }
            }
            return count;
        }

        private bool m_updatingFileList;
        public bool UpdatingFileList
        {
            get 
            { 
                return m_updatingFileList; 
            }
            set
            {
                m_updatingFileList = value;
            }
        }

        private FileSetWithTimeStamps m_readFiles;
        public FileSetWithTimeStamps ReadFiles
        {
            get { return m_readFiles; }
            set { m_readFiles = value; }
        }
        private FileSetWithTimeStamps m_processedFiles;
        public FileSetWithTimeStamps ProcessedFiles
        {
            get { return m_processedFiles; }
            set { m_processedFiles = value; }
        }
        private FileSetWithTimeStamps m_permanentErrorFiles;
        public FileSetWithTimeStamps PermanentErrorFiles
        {
            get { return m_permanentErrorFiles; }
            set { m_permanentErrorFiles = value; }
        }

        private FileSetWithTimeStamps m_filesCopy;
        public FileSetWithTimeStamps FilesCopy
        {
            get { return m_filesCopy; }
            set { m_filesCopy = value; m_changed = true; }
        }


        private List<String> m_permanentErrorFilesCopy;
        public List<String> PermanentErrorFilesCopy
        {
            get { return m_permanentErrorFilesCopy; }
            set { m_permanentErrorFilesCopy = value; }
        }

        public void MergeProcessedAndToProcessLists()
        {
            lock (m_filesCopy)
            {
                lock (m_processedFiles)
                {
                    lock (m_readFiles)
                    {
                        m_filesCopy = FileSetWithTimeStamps.Merge(m_processedFiles, m_readFiles);
                    }
                }
            }
            lock (m_permanentErrorFilesCopy)
            {
                m_permanentErrorFilesCopy.Clear();
                lock (m_permanentErrorFiles)
                {
                    m_permanentErrorFilesCopy.AddRange(m_permanentErrorFiles);
                }
            }
            m_changed = true;
        }

        private bool m_changed;
        public bool Changed
        {
            get { return m_changed; }
            set { m_changed = value; }
        }

        private bool m_permanentErrorFilesChanged;
        public bool PermanentErrorFilesChanged
        {
            get { return m_permanentErrorFilesChanged; }
            set { m_permanentErrorFilesChanged = value; }
        }

        public StatusData(ConfigurationData dat)
        {
            m_cf = dat;
            m_processedFiles = new FileSetWithTimeStamps();
            m_readFiles = new FileSetWithTimeStamps();
            m_permanentErrorFiles = new FileSetWithTimeStamps();
            m_filesCopy = new FileSetWithTimeStamps();
            m_permanentErrorFilesCopy = new List<string>();
            m_datFileStates = new STLLikeMap<string, DatFileStatus>();
            m_updatingFileList = false;
            m_changed = false;
            m_started = false;
        }

        public StatusData Clone()
        {
            StatusData returnValue = new StatusData(m_cf);
            returnValue.m_changed = m_changed;
            returnValue.m_permanentErrorFilesChanged = m_permanentErrorFilesChanged;
            //clear these after have taking the copy;
            m_changed = m_permanentErrorFilesChanged = false;
            returnValue.m_started = m_started;
            returnValue.m_updatingFileList = m_updatingFileList;
            lock (m_datFileStates)
            {
                foreach (KeyValuePair<string, DatFileStatus> pair in m_datFileStates)
                    returnValue.m_datFileStates.Add((string) pair.Key.Clone(), (DatFileStatus) pair.Value.Clone());
            }
            returnValue.m_permanentErrorFiles = returnValue.m_readFiles = returnValue.m_processedFiles = null; //dont copy, causes deadlocks and is data that is not referenced

            lock (m_filesCopy)
            {
                returnValue.m_filesCopy = m_filesCopy.Clone();
            }
            lock (m_permanentErrorFilesCopy)
            {
                returnValue.m_permanentErrorFilesCopy.AddRange(m_permanentErrorFilesCopy);
            }
            return returnValue;
        }
    }
}
