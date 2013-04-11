﻿

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
            { "min", EinheitEnum.min },
            { "m", EinheitEnum.m },
            { "Test", EinheitEnum.Test },
        };

        public static EinheitEnum Parse(string unit)
        {
            EinheitEnum einheit;
            if (_map.TryGetValue(unit, out einheit))
                return einheit;

            // throw new InvalidOperationException(String.Format("No mapping found for unit '{0}'", unit));
            return EinheitEnum.Test;
        }

        public void Process(string unit)
        {
            Result(Parse(unit));
        }

        public event Action<EinheitEnum> Result = delegate { };

    }
}
