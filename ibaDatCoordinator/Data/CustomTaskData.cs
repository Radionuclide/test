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
                m_plugin.Reset(PluginManager.Manager.PluginInfos.Find(delegate(PluginTaskInfo i) { return i.Name == name; }), 
                    DatCoordinatorHostImpl.Host);
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
            pd.m_name = m_name.Clone() as string;
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

    }
}
