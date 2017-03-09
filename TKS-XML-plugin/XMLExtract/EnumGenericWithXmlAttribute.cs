namespace XmlExtract
{
    using System;
    using System.Collections.Generic;


    static class EnumWithXmlAttribute<T> where T : struct, IConvertible
    {
        private static readonly Dictionary<string, T> SensitiveXmlNames = new Dictionary<string, T>();
        private static readonly Dictionary<string, T> InsensitiveXmlNames = new Dictionary<string, T>();
        private static readonly Dictionary<string, T> SensitivePlainNames = new Dictionary<string, T>();
        private static readonly Dictionary<string, T> InsensitivePlainNames = new Dictionary<string, T>();
        private static readonly Dictionary<T, string> XmlNames = new Dictionary<T, string>();

        static EnumWithXmlAttribute()
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                var xmlString = ReadXmlString(item);
                if (xmlString != item.ToString())
                {
                    SensitiveXmlNames.Add(xmlString, item);
                    InsensitiveXmlNames.Add(xmlString.ToLowerInvariant(), item);
                    XmlNames.Add(item, xmlString);
                }
                else
                {
                    SensitivePlainNames.Add(xmlString, item);
                    InsensitivePlainNames.Add(xmlString.ToLowerInvariant(), item);
                    XmlNames.Add(item, xmlString);
                }
            }
        }

        public static bool TryParse(string value, out T returnValue)
        {
            return TryParse(value, false, out returnValue);
        }

        public static bool TryParse(string value, bool ignoreCase, out T returnValue)
        {
            if (!ignoreCase)
                return TryParseSensitive(value, out returnValue);

            return TryParseInsensitive(value.ToLowerInvariant(), out returnValue);
        }

        private static bool TryParseSensitive(string value, out T returnValue)
        {
            if (SensitiveXmlNames.TryGetValue(value, out returnValue))
                return true;

            return SensitivePlainNames.TryGetValue(value, out returnValue);
        }

        private static bool TryParseInsensitive(string value, out T returnValue)
        {
            if (InsensitiveXmlNames.TryGetValue(value, out returnValue))
                return true;

            return InsensitivePlainNames.TryGetValue(value, out returnValue);
        }


        public static string GetXmlName(T value)
        {
            string xmlname;
            XmlNames.TryGetValue(value, out xmlname);
            return xmlname;
        }

        private static string ReadXmlString(T value)
        {
            var memberList = typeof(T).GetMember(value.ToString());
            if (memberList.Length == 0) return null;//or string.Empty, or throw exception

            var member = memberList[0];

            var attributesList = member.GetCustomAttributes(false);
            foreach (var item in attributesList)
            {
                var enumAttribute = item as System.Xml.Serialization.XmlEnumAttribute;
                if (enumAttribute == null) continue;

                return enumAttribute.Name;
            }

            return member.Name;
        }
    }
}
