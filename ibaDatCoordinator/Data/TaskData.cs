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
			AFTER_1st_FAILURE_DAT = 3,
			AFTER_1st_FAILURE = 3, //for backwards compatibility
			AFTER_1st_FAILURE_TASK = 4,
			DISABLED = 5
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

        public virtual bool Enabled
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

        private bool m_resourceIntensive;

        public bool ResourceIntensive
        {
            get { return m_resourceIntensive; }
            set { m_resourceIntensive = value; }
        }


        public TaskData(ConfigurationData parent)
        {
            m_parentCD = parent;
            m_pdoFile = "";
            m_wtodo = WhenToDo.AFTER_SUCCES_OR_FAILURE;
            m_notify = WhenToDo.DISABLED;
            m_guid = Guid.NewGuid();
            m_resourceIntensive = false;
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
            else if (this is HDCreateEventTaskData)
                m_name = iba.Properties.Resources.HDEventTaskTitle;
            else if (this is TaskWithTargetDirData)
                m_name = iba.Properties.Resources.cleanupTaskTitle;
            else if (this is OpcUaWriterTaskData)
                m_name = iba.Properties.Resources.opcUaWriterTaskTitle;
            else if (this is UploadTaskData)
                m_name = iba.Properties.Resources.UploadTaskTitle;
            else if (this is KafkaWriterTaskData)
                m_name = iba.Properties.Resources.kafkaWriterTastTitle;
            else if (this is DataTransferTaskData)
                m_name = iba.Properties.Resources.DataTransferTaskTitle;
        }

        public int CompareTo(TaskData other)
        {
            if (other.m_parentCD != m_parentCD)
                throw new InvalidOperationException("Can't compare tasks with different parent configurations");
            return Index.CompareTo(other.Index);
        }

        object ICloneable.Clone()
        {
            return Clone();
        }

        public TaskData Clone()
        {
            TaskData clone = CloneInternal();
            clone.m_wtodo = m_wtodo;
            clone.m_name = m_name;
            clone.m_notify = m_notify;
            clone.m_resourceIntensive = m_resourceIntensive;
            return clone;
        }

        abstract public TaskData CloneInternal();

        public virtual void AdditionalFileNames(List<KeyValuePair<string, string>> myList, string safeConfName)
        { //default, analysisfile
            if (!String.IsNullOrEmpty(m_pdoFile))
            {
                StringBuilder sb = new StringBuilder(safeConfName);
                sb.Append('\\');
                sb.Append(Utility.PathUtil.FilterInvalidFileNameChars(m_name));
                sb.Append('\\');
                sb.Append(System.IO.Path.GetFileName(m_pdoFile));
                myList.Add(new KeyValuePair<string, string>(m_pdoFile, sb.ToString()));
            }
        }

        public bool IsSame(TaskData other)
        {
            return IsSameInternal(other) &&
                other.m_name == m_name &&
                other.m_notify == m_notify &&
                other.m_wtodo == m_wtodo &&
                other.m_resourceIntensive == m_resourceIntensive;
        }

        abstract public bool IsSameInternal(TaskData taskData);
    }
}
