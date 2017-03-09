using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iba.Utility
{
    public class XmlWrapper : IXmlSerializable
    {
        private object m_objectToSerialize;

        public object ObjectToSerialize
        {
            get { return m_objectToSerialize; }
            set { m_objectToSerialize = value; }
        }

        public XmlWrapper(object objectToSerialize)
        {
            m_objectToSerialize = objectToSerialize;
        }

        public XmlWrapper() : this(null)
        {
        }

        #region IXmlSerializable Members

        public System.Xml.Schema.XmlSchema GetSchema()
        {
            IXmlSerializable ixmls = m_objectToSerialize as IXmlSerializable;
            if (ixmls != null) return ixmls.GetSchema();
            else return null; 
        }

        public void ReadXml(System.Xml.XmlReader reader)
        {
            string dub= reader.GetAttribute("type");
            Type type = Type.GetType(dub);
            reader.ReadStartElement();
            m_objectToSerialize = (new XmlSerializer(type)).Deserialize(reader);
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            Type type = m_objectToSerialize.GetType();
            writer.WriteAttributeString("type", type.AssemblyQualifiedName);
            new XmlSerializer(type).Serialize(writer, m_objectToSerialize);
        }
        #endregion
    }

    public class XMLMultilineTextFixer
    {
        public static string Fix(string inputstr)
        {
            if (inputstr == null) return null;
            return inputstr.Replace("\r\n", "\n").Replace("\n", "\r\n");
        }
    }
}
