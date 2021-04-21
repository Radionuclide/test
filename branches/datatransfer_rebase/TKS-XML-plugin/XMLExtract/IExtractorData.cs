using System;
using System.Collections.Generic;
using System.Text;
using iba.Plugins;

namespace XmlExtract
{
    public interface IExtractorData
    {
        StandortType StandOrt { get; set; }
        IdFieldLocation IdField { get; set; }
        string AndererStandort { get; set; }
        string XmlSchemaLocation { get; set; }
        string XsdLocation { get; set; }
    }
}
