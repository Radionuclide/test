using System;
using iba.ibaFilesLiteDotNet;

namespace XmlExtract
{

    internal static class ibaFilesLibExtensions
    {

        internal static string ResolveSignalId(this IbaChannelReader channel, IdFieldLocation idField)
        {
            string value = String.Empty;
            switch (idField)
            {
                case IdFieldLocation.PDA_Comment1:
                    value = channel.Comment1;
                    break;
                case IdFieldLocation.PDA_Comment2:
                    value = channel.Comment2;
                    break;
            }

            if (!String.IsNullOrEmpty(value))
                return value.Trim();

            return channel.Name.Trim();
        }


    }
}

