

namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;


    internal class ResolveEinheit
    {
        readonly static Dictionary<string, EinheitEnum> _map = new Dictionary<string, EinheitEnum>() { 
            { "mm", EinheitEnum.mm },
            { "µm", EinheitEnum.mikrom },
            { "gradC", EinheitEnum.gradC },
            { "m/min", EinheitEnum.mmin },
            { "kW", EinheitEnum.kW },
            { "A", EinheitEnum.A },
            { "V", EinheitEnum.V },
            { "g/qm", EinheitEnum.gqm },
            { "%", EinheitEnum.Prozent },
            { "KN", EinheitEnum.KN },
            { "mbar", EinheitEnum.mbar },
            { "grad", EinheitEnum.grad },
            { "Keine/1", EinheitEnum.Keine1 },
            { "", EinheitEnum.Keine1 },
            { "min", EinheitEnum.min },
            { "m", EinheitEnum.m },
            { "As/m²", EinheitEnum.Asqm },
            { "U/min", EinheitEnum.Umin },
            { "Test", EinheitEnum.Test },
        };

        public static object Parse(string unit)
        {
            EinheitEnum einheit;
            if (_map.TryGetValue(unit, out einheit))
                return einheit;

            return unit;
        }

        public void Process(string unit)
        {
            Result(Parse(unit));
        }

        public event Action<object> Result = delegate { };

    }
}
