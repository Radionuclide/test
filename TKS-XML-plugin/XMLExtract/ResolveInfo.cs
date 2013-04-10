

namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Globalization;
    using ibaFilesLiteLib;


    internal class ResolveInfo
    {

        public static Info Resolve(IbaFileReader reader)
        {
            const string DE_BUNDNR = "$DE_BUNDNR";
            const string DE_STANDORT = "$DE_STANDORT";
            const string DE_MATERIALART = "$DE_MATERIALART";
            const string DE_MESSZEITPUNKT = "$DE_MESSZEITPUNKT";
            const string STARTTIME = "starttime";

            var validationresult = new StringBuilder();
            var info = new Info();

            //BundNr = reader.QueryInfoByName("$DE_BUNDNR").Trim();
            //Dicke = Single.Parse(reader.QueryInfoByName("$DE_DICKE").Trim());
            //Breite = Single.Parse(reader.QueryInfoByName("$DE_BREITE").Trim());
            //ZinkAuflage = Single.Parse(reader.QueryInfoByName("$DE_ZINKAUFLAGE").Trim());
            //StahlMarke = reader.QueryInfoByName("$DE_STAHLMARKE").Trim();
            //Kunde = reader.QueryInfoByName("$DE_KUNDE").Trim();
            //Laenge = Single.Parse(reader.QueryInfoByName("$DE_LAENGE").Trim());

            if (Convert.ToBoolean(reader.IsInfoPresent(DE_BUNDNR)))
                info.LocalIdent = reader.QueryInfoByName(DE_BUNDNR).Trim();
            else
                validationresult.AppendFormat("Could not find info column '{0}'", DE_BUNDNR).AppendLine();

            if (Convert.ToBoolean(reader.IsInfoPresent(DE_STANDORT)))
                info.Standort = (StandortType)Enum.Parse(typeof(StandortType), reader.QueryInfoByName(DE_STANDORT).Trim());
            //else
            //    validationresult.AppendFormat("Could not find infofield '{0}'", DE_STANDORT).AppendLine();

            if (Convert.ToBoolean(reader.IsInfoPresent(DE_MATERIALART)))
                info.MaterialArt = (MaterialArtType)Enum.Parse(typeof(MaterialArtType), reader.QueryInfoByName(DE_MATERIALART).Trim());
            //else
            //    validationresult.AppendFormat("Could not find infofield '{0}'", DE_MATERIALART).AppendLine();


            info.Bandlaufrichtung = BandlaufrichtungEnum.InWalzRichtung;
            info.Endprodukt = true;

            if (Convert.ToBoolean(reader.IsInfoPresent(DE_MESSZEITPUNKT)))
                info.Messzeitpunkt = DateTime.Parse(reader.QueryInfoByName(DE_MESSZEITPUNKT));
            else if (Convert.ToBoolean(reader.IsInfoPresent(STARTTIME)))
                info.Messzeitpunkt = DateTime.Parse(reader.QueryInfoByName(STARTTIME));
            else
                validationresult.AppendFormat("Could not find measurement date neither at info column '{0}' nor at '{1}'", DE_MESSZEITPUNKT, STARTTIME).AppendLine();

            info.Error = validationresult.ToString();
            return info;
        }

        public void Process(IbaFileReader reader)
        {
            Result(Resolve(reader));
        }

        public event Action<Info> Result = delegate { };
    }
}