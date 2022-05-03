using System;
using System.Collections.Generic;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Text;

namespace XmlExtract
{
    internal class Info
    {
        public string TKSIdent { get; set; }
        public string LocalIdent { get; set; }
        public BandlaufrichtungEnum Bandlaufrichtung { get; set; }
        public bool Endprodukt { get; set; }
        public DateTime Messzeitpunkt { get; set; }
        public StandortType Standort { get; set; }
        public MaterialArtType MaterialArt { get; set; }
        public string Aggregat { get; set; }

        public List<Vector> Vektoren { get; set; } = new List<Vector>();
        public BezugDimensionEnum VectorsDimensionX = BezugDimensionEnum.Laenge;

        public string Error { get; set; }
    }
}
