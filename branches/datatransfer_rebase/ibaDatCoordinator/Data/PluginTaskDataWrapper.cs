using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;
using iba.Utility;
using System.Xml.Serialization;

namespace iba.Data
{
    [Serializable]
    public class PluginTaskDataWrapper : IXmlSerializable
    {
        public PluginTaskDataWrapper()
        {

        }

        public IPluginTaskData PluginData;

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            if (PluginData is IXmlSerializable ixmls)
                return ixmls.GetSchema();
            else
                return null;
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            string dub = reader.GetAttribute("type");
            if (dub.Contains("Sidmar-OSPC")) //was wrongly named in the past...
                dub = dub.Replace("Sidmar-OSPC", "AM-OSPC");

            string pluginName = reader.GetAttribute("pluginName");

            Type type = Type.GetType(dub);
            if (type == null)
            {
                //Plugin is not installed
                CreateErrorPlugin(String.Format(iba.Properties.Resources.PluginNotInstalled, pluginName ?? dub),
                    dub, pluginName, reader);
                return;
            }
            else if (PluginManager.Manager.IsPluginOutdated(ref pluginName, type.Assembly))
            {
                //Plugin version is not compatible
                CreateErrorPlugin(String.Format(iba.Properties.Resources.PluginOutdated, pluginName, type.Assembly.GetName().Version.ToString(3)),
                    dub, pluginName, reader);
                return;
            }

            reader.ReadStartElement();

            PluginData = (new XmlSerializer(type)).Deserialize(reader) as IPluginTaskData;
            if (PluginData == null)
                throw new Exception($"Type {type} should derive from IPluginTaskData");

            PluginData.Reset(DatCoordinatorHostImpl.Host);

            reader.ReadEndElement();
        }

        private void CreateErrorPlugin(string errorMsg, string type, string pluginName, System.Xml.XmlReader reader)
        {
            reader.ReadStartElement();
            string config = reader.ReadOuterXml();
            reader.ReadEndElement();

            PluginData = new ErrorPluginTaskData()
            {
                ErrorMessage = errorMsg,
                SavedConfig = config,
                PluginType = type,
                PluginName = pluginName
            };
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            if (PluginData is ErrorPluginTaskData errorPlugin)
            {
                writer.WriteAttributeString("type", errorPlugin.PluginType);
                writer.WriteAttributeString("pluginName", errorPlugin.PluginName);

                writer.WriteRaw(errorPlugin.SavedConfig);
            }
            else
            { 
                Type type = PluginData.GetType();
                writer.WriteAttributeString("type", type.AssemblyQualifiedName);
                writer.WriteAttributeString("pluginName", PluginData.NameInfo);

                new XmlSerializer(type).Serialize(writer, PluginData);
            }
        }
        #endregion

        public PluginTaskDataWrapper Clone()
        {
            return new PluginTaskDataWrapper() {
                PluginData = PluginData.Clone() as IPluginTaskData
            };
        }

        public bool IsSame(PluginTaskDataWrapper other)
        {
            if (other == null)
                return false;
            else if (other == this)
                return true;

            if (PluginData.GetType() != other.PluginData.GetType())
                return false;

            if (PluginData is IPluginTaskDataIsSame dataIsSame)
                return dataIsSame.IsSame(other.PluginData as IPluginTaskDataIsSame);

            return Utility.SerializableObjectsCompare.Compare(PluginData, other.PluginData);
        }
    }
}

