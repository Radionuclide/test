namespace XmlExtract
{
    using System;
    using System.Collections.Generic;


    public static class Enum<T> where T : struct
    {
        private static readonly List<T> All = new List<T>();
        private static readonly Dictionary<string, T> InsensitiveNames = new Dictionary<string, T>();
        private static readonly Dictionary<string, T> SensitiveNames = new Dictionary<string, T>();
        private static readonly Dictionary<int, T> Values = new Dictionary<int, T>();
        private static readonly Dictionary<T, string> Names = new Dictionary<T, string>();


        static Enum()
        {
            foreach (T item in Enum.GetValues(typeof(T)))
            {
                All.Add(item);
                InsensitiveNames.Add(item.ToString().ToLowerInvariant(), item);
                SensitiveNames.Add(item.ToString(), item);
                Values.Add(Convert.ToInt32(item), item);
                Names.Add(item, item.ToString());
            }
        }

        public static bool IsDefined(T value)
        {
            return Names.ContainsKey(value);
        }

        public static bool IsDefined(string value)
        {
            return SensitiveNames.ContainsKey(value);
        }

        public static bool IsDefined(int value)
        {
            return Values.ContainsKey(value);
        }

        public static IEnumerable<T> GetValues()
        {
            return All;
        }

        public static string[] GetNames()
        {

            return new List<string>(Names.Values).ToArray();
        }

        public static string GetName(T value)
        {
            string name;
            Names.TryGetValue(value, out name);
            return name;
        }

        public static T Parse(string value)
        {
            T parsed = default(T);
            if (!SensitiveNames.TryGetValue(value, out parsed))
                throw new ArgumentException("Value is not one of the named constants defined for the enumeration", "value");
            return parsed;
        }

        public static T Parse(string value, bool ignoreCase)
        {
            if (!ignoreCase)
                return Parse(value);

            T parsed = default(T);
            if (!InsensitiveNames.TryGetValue(value.ToLowerInvariant(), out parsed))
                throw new ArgumentException("Value is not one of the named constants defined for the enumeration", "value");
            return parsed;
        }

        public static bool TryParse(string value, out T returnValue)
        {
            return SensitiveNames.TryGetValue(value, out returnValue);
        }

        public static bool TryParse(string value, bool ignoreCase, out T returnValue)
        {
            if (!ignoreCase)
                return TryParse(value, out returnValue);

            return InsensitiveNames.TryGetValue(value.ToLowerInvariant(), out returnValue);
        }

        public static T? ParseOrNull(string value)
        {
            if (String.IsNullOrEmpty(value))
                return null;

            T foundValue;
            if (InsensitiveNames.TryGetValue(value.ToLowerInvariant(), out foundValue))
                return foundValue;

            return null;
        }

        public static T? CastOrNull(int value)
        {
            T foundValue;
            if (Values.TryGetValue(value, out foundValue))
                return foundValue;

            return null;
        }
    }
}
