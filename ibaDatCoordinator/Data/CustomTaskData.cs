using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;
using iba.Utility;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class CustomTaskData : TaskData
    {
        private IPluginTaskData m_plugin;
        
        [XmlIgnore]
        public IPluginTaskData Plugin
        {
            get { return m_plugin; }
            set { m_plugin = value; }
        }

        public XmlWrapper PluginXML
        {
            get 
            {
                return new XmlWrapper(m_plugin);
            }
            set 
            {
                m_plugin = value.ObjectToSerialize as IPluginTaskData;
                string name = m_plugin.NameInfo;
                m_plugin.Reset(DatCoordinatorHostImpl.Host);
            }
        }

        public CustomTaskData(ConfigurationData parent, PluginTaskInfo plugin)
            : base(parent)
        {
            m_name = plugin.Name;
            m_plugin = PluginManager.Manager.CreateTask(m_name, parent);
        }

        public CustomTaskData() : base(null) {}

        public override object Clone()
        {
            CustomTaskData pd = new CustomTaskData();
            pd.m_plugin = m_plugin.Clone() as IPluginTaskData;
            pd.m_wtodo = m_wtodo;
            pd.m_name = m_name;
            pd.m_notify = m_notify;
            return pd;
        }

        [XmlIgnore]
        public override ConfigurationData ParentConfigurationData
        {
            get { return m_parentCD; }
            set
            {
                m_parentCD = value;
                m_plugin.SetParentJob(value);
            }
        }

        public override bool IsSame(TaskData taskData)
        {
            CustomTaskData other = taskData as CustomTaskData;
            if (other == null) return false;
            if (other == this) return true;
            if (other.m_plugin.GetType() != m_plugin.GetType()) return false;
            if (other.m_wtodo != m_wtodo ||
                other.m_name != m_name ||
                other.m_notify != m_notify) return false;
            if (m_plugin is IPluginTaskDataIsSame)
            {
                return (m_plugin as IPluginTaskDataIsSame).IsSame(other.m_plugin as IPluginTaskDataIsSame);
            }
            else return Utility.SerializableObjectsCompare.Compare(m_plugin, other.m_plugin);
        }
    }
}
