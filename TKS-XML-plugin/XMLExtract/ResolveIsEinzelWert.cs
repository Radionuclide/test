using System;
using System.Linq;

namespace XmlExtract
{
    internal class ResolveIsEinzelWert
    {
        private static readonly string[] _einzelwertIndicators = { "__IE__", "__SE__", "__HE__", "__ME__"};

        public static bool Resolve(string signalName)
        {
            return _einzelwertIndicators.Any(ew => signalName.Contains(ew));
        }

        public void Process(string signalName)
        {
            Result(Resolve(signalName));
        }

        public event Action<bool> Result = delegate { };

    }
}
