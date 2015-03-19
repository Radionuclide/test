

namespace XmlExtract
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Globalization;

    using iba.TKS_XML_Plugin.Properties;
    using iba.ibaFilesLiteDotNet;


    internal class ResolveInfo
    {
        const string DE_BUNDNR = "$DE_BUNDNR";
        const string DE_TKSIDENT = "$DE_TKSIDENT";
        const string DE_MATERIALART = "$DE_MATERIALART";
        const string DE_MESSZEITPUNKT = "$DE_MESSZEITPUNKT";
        const string STARTTIME = "starttime";
        const string DE_BANDLAUFRICHTUNG = "$DE_BANDLAUFRICHTUNG";
        const string DE_ENDPRODUKT = "$DE_ENDPRODUKT";
        const string DE_AGGREGAT = "$DE_AGGREGAT";

        private static List<string> _missingFields;
        private static List<string> _wrongValueFields;

        public static Info Resolve(IbaFileReader reader, StandortType st)
        {


            _missingFields = new List<string>();
            _wrongValueFields = new List<string>();
            var info = new Info();

            //BundNr = reader.QueryInfoByName("$DE_BUNDNR").Trim();
            //Dicke = Single.Parse(reader.QueryInfoByName("$DE_DICKE").Trim());
            //Breite = Single.Parse(reader.QueryInfoByName("$DE_BREITE").Trim());
            //ZinkAuflage = Single.Parse(reader.QueryInfoByName("$DE_ZINKAUFLAGE").Trim());
            //StahlMarke = reader.QueryInfoByName("$DE_STAHLMARKE").Trim();
            //Kunde = reader.QueryInfoByName("$DE_KUNDE").Trim();
            //Laenge = Single.Parse(reader.QueryInfoByName("$DE_LAENGE").Trim());

            string infoFieldValue;
            // return when tksident (is set && not empty && <> 0)
            if (reader.InfoFields.TryGetValue(DE_TKSIDENT, out infoFieldValue) && !String.IsNullOrWhiteSpace(infoFieldValue) && infoFieldValue != "0")
                info.TKSIdent = infoFieldValue;
            else if (reader.InfoFields.TryGetValue(DE_BUNDNR, out infoFieldValue))
                info.LocalIdent = infoFieldValue;
            else
                _missingFields.Add(String.Concat(DE_TKSIDENT, "' oder '", DE_BUNDNR));


            if (st == StandortType.DU)
            {
                if (reader.InfoFields.TryGetValue(DE_MATERIALART, out infoFieldValue))
                {
                    MaterialArtType mat;
                    if (Enum<MaterialArtType>.TryParse(infoFieldValue.Trim(), true, out mat))
                        info.MaterialArt = mat;
                    else
                        _wrongValueFields.Add(DE_MATERIALART);
                }
                else
                    _missingFields.Add(DE_MATERIALART);
            }


            if (reader.InfoFields.TryGetValue(DE_BANDLAUFRICHTUNG, out infoFieldValue))
            {
                var blr = BandlaufrichtungEnum.InWalzRichtung;
                if (Enum<BandlaufrichtungEnum>.TryParse(infoFieldValue.Trim(), true, out blr))
                    info.Bandlaufrichtung = blr;
                else
                    _wrongValueFields.Add(DE_BANDLAUFRICHTUNG);
            }
            else
                _missingFields.Add(DE_BANDLAUFRICHTUNG);


            if (reader.InfoFields.TryGetValue(DE_AGGREGAT, out infoFieldValue))
                info.Aggregat = infoFieldValue.Trim();
            else
                _missingFields.Add(DE_AGGREGAT);


            if (reader.InfoFields.TryGetValue(DE_ENDPRODUKT, out infoFieldValue))
            {
                var ep = true;
                if (TryConvertToBoolean(infoFieldValue.Trim(), out ep))
                    info.Endprodukt = ep;
                else
                    _wrongValueFields.Add(DE_ENDPRODUKT);
            }
            else
                _missingFields.Add(DE_ENDPRODUKT);

            info.Messzeitpunkt = GetMesszeit(reader);

            info.Error = FormatError();
            return info;
        }

        private static string FormatError()
        {
            if (_wrongValueFields.Count + _missingFields.Count == 0)
                return string.Empty;

            _missingFields = _missingFields.ConvertAll(x => StripPrefix(x));
            _wrongValueFields = _wrongValueFields.ConvertAll(x => StripPrefix(x));

            string misingFieldError = String.Empty;
            string wrongValuesError = String.Empty;

            if (_missingFields.Count > 0)
            {
                misingFieldError = String.Format(Resources.MissingInfoFields, String.Join("', '", _missingFields.ToArray()));
            }

            if (_wrongValueFields.Count > 0)
            {
                wrongValuesError = String.Format(Resources.WrongValuesForInfoFields, String.Join("', '", _wrongValueFields.ToArray()));
            }

            return String.Format("{0} {1}", misingFieldError, wrongValuesError).Trim();

        }

        private static DateTime GetMesszeit(IbaFileReader reader)
        {
            string dtValue;
            DateTime dt;
            if (reader.InfoFields.TryGetValue(DE_MESSZEITPUNKT, out dtValue))
            {
                if (GetDateTimeParseExact(dtValue, out dt))
                    return dt;
                else
                    _wrongValueFields.Add(DE_MESSZEITPUNKT);
            }
            else 
            {
                return reader.StartTime;
            }
            return DateTime.Now;
        }

        internal static bool GetDateTimeParseExact(string val, out DateTime date)
        {
            return DateTime.TryParseExact(val, "dd.MM.yyyy HH:mm:ss.fff", new CultureInfo("de-DE"), DateTimeStyles.None, out date);
        }

        internal static string StripPrefix(string value)
        {
            const string prefix = "$DE_";
            return value.Replace(prefix, "");
        }


        internal static bool TryConvertToBoolean(string input, out bool result)
        {
            var TrueStrings = new List<string>(new string[] { "ja", "wahr", "true", "t", "yes", "y" });
            var FalseStrings = new List<string>(new string[] { "nein", "falsch", "false", "f", "no", "n" });

            // Remove whitespace from string and lowercase
            string formattedInput = input.Trim().ToLower();

            if (TrueStrings.Contains(formattedInput))
            {
                result = true;
                return true;
            }

            if (FalseStrings.Contains(formattedInput))
            {
                result = false;
                return true;
            }

            int intVal = 0;
            if (int.TryParse(formattedInput, out intVal))
            {
                result = Convert.ToBoolean(intVal);
                return true;
            }

            result = false;
            return false;
        }

        public void Process(IbaFileReader reader, StandortType st)
        {
            Result(Resolve(reader, st));
        }

        public event Action<Info> Result = delegate { };
    }
}