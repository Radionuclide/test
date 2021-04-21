using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace iba.Utility
{

    public class XMLMultilineTextFixer
    {
        public static string Fix(string inputstr)
        {
            if (inputstr == null) return null;
            return inputstr.Replace("\r\n", "\n").Replace("\n", "\r\n");
        }
    }
}
