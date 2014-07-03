

namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics;


    internal class ResolveGruppe
    {
        private static List<string> _suffixList = new List<string>() { "__IR__S", "__GR__S" };

        public static string Resolve(string signalName)
        {
            foreach (string suffix in _suffixList)
            {
                if (signalName.EndsWith(suffix))
                    return signalName.Substring(0, signalName.Length - suffix.Length);
            }
            return signalName;

        }

        public void Process(string signalName)
        {
            Result(Resolve(signalName));
        }

        public event Action<string> Result = delegate { };

    }
}
