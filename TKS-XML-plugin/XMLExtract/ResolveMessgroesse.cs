

namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Diagnostics;


    internal class ResolveMessgroesse
    {
        //readonly static Dictionary<string, MessungTypeEnum> _map = new Dictionary<string, MessungTypeEnum>() { 
        //    { "m", MessungTypeEnum.Laenge },
        //    { "mm", MessungTypeEnum.Laenge },
        //    { "µm", MessungTypeEnum.Laenge }, 
        //    { "%", MessungTypeEnum.Test }, 
        //};

        public static MessungTypeEnum Resolve(string unit)
        {
            //MessungTypeEnum einheit;
            //if (_map.TryGetValue(unit, out einheit))
            //    return einheit;

            // Debug.Assert(false, String.Format("No measurement unit mapping found for unit '{0}'", unit));

            return MessungTypeEnum.Test;

            //throw new InvalidOperationException(String.Format("No measurement unit mapping found for unit '{0}'", unit));
            // return MessungTypeEnum.@default;
        }

        public void Process(string unit)
        {
            Result(Resolve(unit));
        }

        public event Action<MessungTypeEnum> Result = delegate { };

    }
}
