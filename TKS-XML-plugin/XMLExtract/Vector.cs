using System;

namespace XmlExtract
{
    internal class Vector
    {
        public string Name { get; set; }
        public string Comment1 { get; set; } = String.Empty;
        public string Comment2 { get; set; } = String.Empty;
        public string Unit { get; set; } = String.Empty;
        public string ZoneUnit { get; set; } = String.Empty;

        public double ZoneOffset { get; set; } = 0;
    }
}