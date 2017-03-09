

namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Xml;
    using System.Xml.XPath;


    internal class ResolveEinheit
    {
        private readonly static Dictionary<string, EinheitEnum> _specialsMap = new Dictionary<string, EinheitEnum>() { 
            { "µm", EinheitEnum.mikrom },
            { "°C", EinheitEnum.gradC },
            { "g/m²", EinheitEnum.gqm },
            { "%", EinheitEnum.Prozent },
            { "int", EinheitEnum.Keine1 },
            { "", EinheitEnum.Keine1 },
            { "As/m²", EinheitEnum.Asqm },
        };

        private Dictionary<string, string> _xsdEntries;

        public ResolveEinheit()
        {
            Error = String.Empty;
        }

        public string Error { get; set; }

        public string Parse(string unit)
        {
            EinheitEnum einheit;
            if (EnumWithXmlAttribute<EinheitEnum>.TryParse(unit, true, out einheit))
                return EnumWithXmlAttribute<EinheitEnum>.GetXmlName(einheit);

            if (_specialsMap.TryGetValue(unit, out einheit))
                return EnumWithXmlAttribute<EinheitEnum>.GetXmlName(einheit);

            if (_xsdEntries != null && _xsdEntries.ContainsKey(unit))
                return unit;

            if (unit.StartsWith("[") && unit.EndsWith("]"))
                return EnumWithXmlAttribute<EinheitEnum>.GetXmlName(EinheitEnum.Keine1);

            return null;
        }

        public void Process(string unit)
        {
            Result(Parse(unit));
        }

        public event Action<string> Result = delegate { };



        public void Open(string xsdPath)
        {
            if (_xsdEntries != null)
                return;

            _xsdEntries = new Dictionary<string, string>();

            if (!System.IO.File.Exists(xsdPath))
                return;

            ReadXsd(xsdPath);
        }

        private void ReadXsd(string filename)
        {
            var xpath = @"//xsd:simpleType[@name='EinheitEnum']//xsd:enumeration/@value";

            try
            {
                var doc = new XPathDocument(filename);
                var navigator = doc.CreateNavigator();

                var ns = new XmlNamespaceManager(navigator.NameTable);
                ns.AddNamespace("xsd", "http://www.w3.org/2001/XMLSchema");

                var nodes = navigator.Select(xpath, ns);

                while (nodes.MoveNext())
                {
                    _xsdEntries[nodes.Current.Value] = nodes.Current.Value;
                }

            }
            catch (Exception ex)
            {
                Error = String.Format("Loading EinheitEnum from {0} failed: {0}", ex.Message, filename);
            }

        }
    }
}
