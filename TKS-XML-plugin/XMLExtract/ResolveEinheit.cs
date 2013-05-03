

namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    internal class ResolveEinheit
    {
        readonly static Dictionary<string, EinheitEnum> _map = new Dictionary<string, EinheitEnum>() { 
            { "µm", EinheitEnum.mikrom },
            { "°C", EinheitEnum.gradC },
            { "gradC", EinheitEnum.gradC },
            { "m/min", EinheitEnum.mmin },
            { "g/qm", EinheitEnum.gqm },
            { "g/m²", EinheitEnum.gqm },
            { "%", EinheitEnum.Prozent },
            { "Keine/1", EinheitEnum.Keine1 },
            { "int", EinheitEnum.Keine1 },
            { "", EinheitEnum.Keine1 },
            { "As/m²", EinheitEnum.Asqm },
            { "As/qm", EinheitEnum.Asqm },
            { "U/min", EinheitEnum.Umin },
        };

        public static object Parse(string unit)
        {
            EinheitEnum einheit;
            if (Enum<EinheitEnum>.TryParse(unit, true, out einheit))
                return einheit;

            if (_map.TryGetValue(unit, out einheit))
                return einheit;

            if (unit.StartsWith("[") && unit.EndsWith("]"))
                return EinheitEnum.Keine1;

            return unit;
        }

        public void Process(string unit)
        {
            Result(Parse(unit));
        }

        public event Action<object> Result = delegate { };

    }
}
