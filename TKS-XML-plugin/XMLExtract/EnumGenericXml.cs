namespace XmlExtract
{
    using System;
    using System.Collections.Generic;


    static partial class Enum<T> where T : struct, IConvertible
    {
        private static readonly Dictionary<string, T> SensitiveXmlNames = new Dictionary<string, T>();
        private static readonly Dictionary<string, T> InsensitiveXmlNames = new Dictionary<string, T>();
        private static readonly Dictionary<T, string> XmlNames = new Dictionary<T, string>();

        static partial void InitPartialExtension(T item)
        {
            var xmlString = ReadXmlString(item);
            SensitiveXmlNames.Add(xmlString, item);
            InsensitiveXmlNames.Add(xmlString.ToLowerInvariant(), item);
            XmlNames.Add(item, xmlString);
        }

        public static bool TryParseXmlString(string value, out T returnValue)
        {
            return TryParseXmlString(value, false, out returnValue);
        }

        public static bool TryParseXmlString(string value, bool ignoreCase, out T returnValue)
        {
            if (!ignoreCase)
                return SensitiveXmlNames.TryGetValue(value, out returnValue);

            return InsensitiveXmlNames.TryGetValue(value.ToLowerInvariant(), out returnValue);
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
