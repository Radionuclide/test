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
            COMPLETED_SUCCESFULY=3,
            COMPLETED_FAILURE = 4,
            NO_ACCESS = 5
        }

        private STLLikeMap<TaskData, State> m_states;
        public STLLikeMap<TaskData, State> States
        {
            get { return m_states; }
            set { m_states = value; }
        }

        public DatFileStatus()
        {
            m_states = new STLLikeMap<TaskData,State>();
        }

        public object Clone()
        {
            DatFileStatus copy = new DatFileStatus();
            foreach (KeyValuePair<TaskData, State> pair in m_states)
                copy.m_states.Add(pair.Key, pair.Value);
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
            set { m_cf = value; }
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

        private List<String> m_readFiles;
        public List<String> ReadFiles
        {
            get { return m_readFiles; }
            set { m_readFiles = value; }
        }
        private List<String> m_processedFiles;
        public List<String> ProcessedFiles
        {
            get { return m_processedFiles; }
            set { m_processedFiles = value; }
        }

        private List<String> m_readFilesCopy;
        public List<String> ReadFilesCopy
        {
            get { return m_readFilesCopy; }
            set { m_readFilesCopy = value; m_changed = true; }
        }
        
        private List<String> m_processedFilesCopy;
        public List<String> ProcessedFilesCopy
        {
            get { return m_processedFilesCopy; }
            set { m_processedFilesCopy = value; m_changed = true; }
        }

        public void TakeCopyOfFileList()
        {
            lock (m_processedFilesCopy)
            {
                m_processedFilesCopy.Clear();
                lock (m_processedFiles)
                {
                    foreach(string filename in m_processedFiles)
                    {
                        if (filename == null)
                        {
                            System.Diagnostics.Debug.WriteLine("ProcessedFiles");
                        }
                    }
                    m_processedFilesCopy.AddRange(m_processedFiles);
                }
            }
            lock (m_readFilesCopy)
            {
                foreach (string filename in m_readFilesCopy)
                {
                    if (filename == null)
                    {
                        System.Diagnostics.Debug.WriteLine("ReadFilesCopy");
                    }
                }
                m_readFilesCopy.Clear();
                lock (m_readFiles)
                {
                    m_readFilesCopy.AddRange(m_readFiles);
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

        public StatusData(ConfigurationData dat)
        {
            m_cf = dat;
            m_processedFiles = new List<string>();
            m_readFiles = new List<string>();
            m_processedFilesCopy = new List<string>();
            m_readFilesCopy = new List<string>();
            m_datFileStates = new STLLikeMap<string, DatFileStatus>();
            m_updatingFileList = false;
            m_changed = false;
            m_started = false;
        }

        public StatusData Clone()
        {
            StatusData returnValue = new StatusData(m_cf);
            returnValue.m_changed = m_changed;
            returnValue.m_started = m_started;
            returnValue.m_updatingFileList = m_updatingFileList;
            lock (m_datFileStates)
            {
                foreach (KeyValuePair<string, DatFileStatus> pair in m_datFileStates)
                    returnValue.m_datFileStates.Add((string) pair.Key.Clone(), (DatFileStatus) pair.Value.Clone());
            }
            returnValue.m_readFiles = returnValue.m_processedFiles = null; //dont copy, causes deadlocks and is data that is not referenced

            lock (m_processedFilesCopy)
            {
                returnValue.m_processedFilesCopy.AddRange(m_processedFilesCopy);
            }
            lock (m_readFilesCopy)
            {
                returnValue.m_readFilesCopy.AddRange(m_readFilesCopy);
            }
            return returnValue;
        }
    }
}
