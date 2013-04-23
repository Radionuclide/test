namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using ibaFilesLiteLib;


    internal static class ibaFilesLibExtensions
    {

        public static IEnumerable<IbaChannelReader> Channels(this IbaFileReader reader)
        {
            IbaEnumChannelReader channelEnum = (IbaEnumChannelReader)reader.EnumChannels();

            while (channelEnum.IsAtEnd() != 1)
            {
                yield return (IbaChannelReader)channelEnum.Next();
            }

        }

        public static IEnumerable<KeyValuePair<string, string>> InfoFields(this IbaChannelReader channel)
        {
            int cnt = 0;
            string name = string.Empty;
            string value = string.Empty;

            channel.QueryInfoByIndex(cnt, out name, out value);
            while (!String.IsNullOrEmpty(name))
            {
                yield return new KeyValuePair<string, string>(name.Trim(), value.Trim());
                cnt++;
                channel.QueryInfoByIndex(cnt, out name, out value);
            }
        }

        public static string Name(this IbaChannelReader channel)
        {
            return channel.QueryInfoByName("name").Trim();
        }

        public static string Unit(this IbaChannelReader channel)
        {
            if (Convert.ToBoolean(channel.IsInfoPresent("unit")))
                return channel.QueryInfoByName("unit").Trim();
            return string.Empty;
        }

        public static string PDA_Comment1(this IbaChannelReader channel)
        {
            if (Convert.ToBoolean(channel.IsInfoPresent("$PDA_Comment1")))
                return channel.QueryInfoByName("$PDA_Comment1").Trim();
            return string.Empty;
        }

        public static string PDA_Comment2(this IbaChannelReader channel)
        {
            if (Convert.ToBoolean(channel.IsInfoPresent("$PDA_Comment2")))
                return channel.QueryInfoByName("$PDA_Comment2").Trim();
            return string.Empty;
        }

        //create an Id like ibaAnalyzer would
        internal static string CreateIbaAnalyzerChannelId(this IbaChannelReader channel)
        {
            int moduleNr = channel.ModuleNumber;
            int nrInModule = channel.NumberInModule;
            int subchannelNr = channel.SubChannelNumber;
            bool digital = channel.IsDigital() == 1;

            char seperator = digital ? '.' : ':';
            if (subchannelNr != -1)
                return string.Format("[{0}.{1}{2}{3}]", moduleNr, nrInModule, seperator, subchannelNr);
            else
                return string.Format("[{0}{1}{2}]", moduleNr, seperator, nrInModule);
        }

        internal static string ResolveSignalId(this IbaChannelReader channel, IdFieldLocation idField)
        {
            string value = string.Empty;
            switch (idField)
            {
                case IdFieldLocation.PDA_Comment1:
                    value = channel.PDA_Comment1();
                    break;
                case IdFieldLocation.PDA_Comment2:
                    value = channel.PDA_Comment2();
                    break;
            }

            if (!String.IsNullOrEmpty(value))
                return value;

            return channel.Name();
        }


    }
}

namespace System.Runtime.CompilerServices
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class | AttributeTargets.Assembly)]
    public sealed class ExtensionAttribute : Attribute { }
}
