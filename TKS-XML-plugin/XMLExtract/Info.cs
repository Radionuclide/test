namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;

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

        public string Error { get; set; }
    }
}
