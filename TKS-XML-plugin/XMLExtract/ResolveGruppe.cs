

namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics;
    using System.Text.RegularExpressions;

    internal class ResolveGruppe
    {
        //private static List<string> _suffixList = new List<string>() { "__IR__S", "__GR__S" };
        private static Regex _regex = new Regex("^(.*?)(__[A-Za-z0-9]+__[A-Za-z0-9]*)$");

        public static string Resolve(string signalName)
        {
            return _regex.Replace(signalName, "$1");
        }

        public void Process(string signalName)
        {
            Result(Resolve(signalName));
        }

        public event Action<string> Result = delegate { };

    }
}
