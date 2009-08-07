using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    abstract public class TaskData : ICloneable, IComparable<TaskData>
    {
        protected string m_name;
        public string Name
        {
            get {return m_name;}
            set {m_name = string.IsNullOrEmpty(value)?"":value;}
        }

        public enum WhenToDo
        {
            AFTER_SUCCES = 0,
            AFTER_FAILURE = 1,
            AFTER_SUCCES_OR_FAILURE = 2,
            AFTER_1st_FAILURE = 3,
            DISABLED = 4
        }

        protected WhenToDo m_wtodo;
        public WhenToDo WhenToExecute
        {
            get { return m_wtodo; }
            set { m_wtodo = value; }
        }

        protected WhenToDo m_notify;
        public WhenToDo WhenToNotify
        {
            get { return m_notify; }
            set { m_notify = value; }
        }

        protected ConfigurationData m_parentCD;

        [XmlIgnore]
        virtual public ConfigurationData ParentConfigurationData
        {
            get { return m_parentCD; }
            set { m_parentCD = value; }
        }
        
        [XmlIgnore]
        public int Index
        {
            get { return m_parentCD.Tasks.IndexOf(this); }
        }

        public bool Enabled
        {
            get { return m_wtodo != WhenToDo.DISABLED; }
        }

        protected string m_pdoFile;
        public string AnalysisFile
        {
            get { return m_pdoFile; }
            set { m_pdoFile = value; }
        }

        private Guid m_guid;
        public Guid Guid
        {
            get { return m_guid; }
            set { m_guid = value; }
        }

        public TaskData(ConfigurationData parent)
        {
            m_parentCD = parent;
            m_pdoFile = "";
            m_wtodo = WhenToDo.AFTER_SUCCES_OR_FAILURE;
            m_notify = WhenToDo.DISABLED;
            m_guid = Guid.NewGuid();
            if (this is ReportData)
                m_name = iba.Properties.Resources.reportTitle;
            else if (this is ExtractData)
                m_name = iba.Properties.Resources.extractTitle;
            else if (this is BatchFileData)
                m_name = iba.Properties.Resources.batchfileTitle;
            else if (this is CopyMoveTaskData)
                m_name = iba.Properties.Resources.copyTitle;
            else if (this is IfTaskData)
                m_name = iba.Properties.Resources.iftaskTitle;
            else if (this is UpdateDataTaskData)
                m_name = iba.Properties.Resources.updateDataTaskTitle;
            else if (this is PauseTaskData)
                m_name = iba.Properties.Resources.pauseTaskTitle;
        }

        public int CompareTo(TaskData other)
        {
            if (other.m_parentCD != m_parentCD)
                throw new InvalidOperationException("Can't compare tasks with different parent configurations");
            return Index.CompareTo(other.Index);
        }

        abstract public object Clone();
    }
}
