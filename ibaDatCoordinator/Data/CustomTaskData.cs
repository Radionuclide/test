using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;
using iba.Utility;
using System.Xml.Serialization;
using iba.Licensing;

namespace iba.Data
{
    public interface ICustomTaskData
    {
        IPluginTaskData Plugin
        {
            get;
        }

        Guid Guid
        {
            get;
            set;
        }

        string Name
        {
            get;
            set;
        }

    }

    [Serializable]
    public class CustomTaskData : TaskData, ICustomTaskData
    {
        private PluginTaskDataWrapper m_wrapper;
        
        [XmlIgnore]
        public IPluginTaskData Plugin
        {
            get { return m_wrapper.PluginData; }
        }

        [XmlElement("PluginXML")]
        public PluginTaskDataWrapper PluginWrapper
        {
            get => m_wrapper;
            set => m_wrapper = value;
        }

        public override bool Enabled
        {
            get
            {
                //Disable in case the plugin couldn't be loaded.
                return base.Enabled && !(Plugin is ErrorPluginTaskData);
            }
        }

        public CustomTaskData(ConfigurationData parent, PluginTaskInfo plugin)
            : base(parent)
        {
            m_name = plugin.Name;
            m_wrapper = new PluginTaskDataWrapper();
            m_wrapper.PluginData = PluginManager.Manager.CreateTask(m_name, parent);
        }

        public CustomTaskData() : base(null) 
        {

        }

        public override TaskData CloneInternal()
        {
            CustomTaskData pd = new CustomTaskData();
            pd.m_wrapper = m_wrapper.Clone();

            return pd;
        }

        [XmlIgnore]
        public override ConfigurationData ParentConfigurationData
        {
            get { return m_parentCD; }
            set
            {
                m_parentCD = value;
                Plugin.SetParentJob(value);
            }
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            CustomTaskData other = taskData as CustomTaskData;
            if (other == null) 
                return false;
            if (other == this) 
                return true;

            return m_wrapper.IsSame(other.m_wrapper);
        }

        public override int RequiredLicense => LicenseOptionInfo.ConvertMarxPluginBitToLicenseId(Plugin.DongleBitPos);
    }

    [Serializable]
    public class CustomTaskDataUNC : TaskDataUNC, ICustomTaskData
    {
        private PluginTaskDataWrapper m_wrapper;

        [XmlIgnore]
        public IPluginTaskData Plugin
        {
            get => m_wrapper.PluginData;
        }

        [XmlElement("PluginXML")]
        public PluginTaskDataWrapper PluginWrapper
        {
            get => m_wrapper;
            set => m_wrapper = value;
        }
        public override bool Enabled
        {
            get
            {
                //Disable in case the plugin couldn't be loaded.
                return base.Enabled && !(Plugin is ErrorPluginTaskData);
            }
        }

        public CustomTaskDataUNC(ConfigurationData parent, PluginTaskInfo plugin)
            : base(parent)
        {
            m_name = plugin.Name;

            m_wrapper = new PluginTaskDataWrapper();
            m_wrapper.PluginData = PluginManager.Manager.CreateTask(m_name, parent) as IPluginTaskData;
        }

        public CustomTaskDataUNC() : base(null) { }

        public override TaskData CloneInternal()
        {
            CustomTaskDataUNC pd = new CustomTaskDataUNC();
            CopyUNCData(pd);
            pd.m_wrapper = m_wrapper.Clone();

            return pd;
        }

        [XmlIgnore]
        public override ConfigurationData ParentConfigurationData
        {
            get { return m_parentCD; }
            set
            {
                m_parentCD = value;
                Plugin.SetParentJob(value);
            }
        }

        public override bool IsSameInternal(TaskData taskData)
        {
            CustomTaskDataUNC other = taskData as CustomTaskDataUNC;
            if (other == null) 
                return false;
            if (other == this) 
                return true;

            if (!UNCDataIsSame(other)) 
                return false;

            return m_wrapper.IsSame(other.m_wrapper);
        }

        public override int RequiredLicense => LicenseOptionInfo.ConvertMarxPluginBitToLicenseId(Plugin.DongleBitPos);
    }
}
