using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Linq;
using iba.TKS_XML_Plugin.Properties;
using iba.ibaFilesLiteDotNet;


namespace XmlExtract
{

    internal class ResolveInfo
    {
        // ReSharper disable InconsistentNaming
        private const string DE_BUNDNR = "$DE_BUNDNR";
        private const string DE_TKSIDENT = "$DE_TKSIDENT";
        private const string DE_MATERIALART = "$DE_MATERIALART";
        private const string DE_MESSZEITPUNKT = "$DE_MESSZEITPUNKT";
        private const string STARTTIME = "starttime";
        private const string DE_BANDLAUFRICHTUNG = "$DE_BANDLAUFRICHTUNG";
        private const string DE_ENDPRODUKT = "$DE_ENDPRODUKT";
        private const string DE_AGGREGAT = "$DE_AGGREGAT";
        private const string VECTOR_PREFIX = "Vector_name_";
        // ReSharper restore InconsistentNaming

        private static List<string> _missingFields;
        private static List<string> _wrongValueFields;

        public static Info Resolve(IbaFileReader reader, StandortType st)
        {
            _missingFields = new List<string>();
            _wrongValueFields = new List<string>();

            //BundNr = reader.QueryInfoByName("$DE_BUNDNR").Trim();
            //Dicke = Single.Parse(reader.QueryInfoByName("$DE_DICKE").Trim());
            //Breite = Single.Parse(reader.QueryInfoByName("$DE_BREITE").Trim());
            //ZinkAuflage = Single.Parse(reader.QueryInfoByName("$DE_ZINKAUFLAGE").Trim());
            //StahlMarke = reader.QueryInfoByName("$DE_STAHLMARKE").Trim();
            //Kunde = reader.QueryInfoByName("$DE_KUNDE").Trim();
            //Laenge = Single.Parse(reader.QueryInfoByName("$DE_LAENGE").Trim());

            var info = new Info();

            // return when tksident (is set && not empty && <> 0)
            if (reader.InfoFields.TryGetValue(DE_TKSIDENT, out var infoFieldValue) && !String.IsNullOrWhiteSpace(infoFieldValue) && infoFieldValue != "0")
                info.TKSIdent = infoFieldValue;
            else if (reader.InfoFields.TryGetValue(DE_BUNDNR, out infoFieldValue))
                info.LocalIdent = infoFieldValue;
            else
                _missingFields.Add(String.Concat(DE_TKSIDENT, "' oder '", DE_BUNDNR));


            if (st == StandortType.DU)
            {
                if (reader.InfoFields.TryGetValue(DE_MATERIALART, out infoFieldValue))
                {
                    if (Enum<MaterialArtType>.TryParse(infoFieldValue.Trim(), true, out var mat))
                        info.MaterialArt = mat;
                    else
                        _wrongValueFields.Add(DE_MATERIALART);
                }
                else
                    _missingFields.Add(DE_MATERIALART);
            }


            if (reader.InfoFields.TryGetValue(DE_BANDLAUFRICHTUNG, out infoFieldValue))
            {
                if (Enum<BandlaufrichtungEnum>.TryParse(infoFieldValue.Trim(), true, out var blr))
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
                if (TryConvertToBoolean(infoFieldValue.Trim(), out var ep))
                    info.Endprodukt = ep;
                else
                    _wrongValueFields.Add(DE_ENDPRODUKT);
            }
            else
                _missingFields.Add(DE_ENDPRODUKT);

            info.Messzeitpunkt = GetMesszeit(reader);

            var vectornames = reader.InfoFields.Where(i => i.Key.StartsWith(VECTOR_PREFIX)).Select(i => i.Value);
            info.Vektoren.AddRange(vectornames);
            
            info.Error = FormatError();
            return info;
        }

        private static string FormatError()
        {
            if (_wrongValueFields.Count + _missingFields.Count == 0)
                return String.Empty;

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

            return $"{misingFieldError} {wrongValuesError}".Trim();

        }

        private static DateTime GetMesszeit(IbaFileReader reader)
        {
            if (reader.InfoFields.TryGetValue(DE_MESSZEITPUNKT, out var dtValue))
            {
                if (GetDateTimeParseExact(dtValue, out var dt))
                {
                    return dt;
                }
                _wrongValueFields.Add(DE_MESSZEITPUNKT);
            }
            else 
            {
                return reader.StartTime;
            }
            return DateTime.Now;
        }

        private static readonly string[] _parseFormats = {
            "dd.MM.yyyy HH:mm:ss.fff",
            "dd.MM.yyyy HH:mm:ss.ffffff"
        };

        internal static bool GetDateTimeParseExact(string val, out DateTime date)
        {
            return DateTime.TryParseExact(val, _parseFormats, new CultureInfo("de-DE"), DateTimeStyles.None, out date);
        }


        internal static string StripPrefix(string value)
        {
            const string prefix = "$DE_";
            return value.Replace(prefix, "");
        }

        private static readonly List<string> _trueStrings = new List<string>(new string[] { "ja", "wahr", "true", "t", "yes", "y" });
        private static readonly List<string> _falseStrings = new List<string>(new string[] { "nein", "falsch", "false", "f", "no", "n" });

        internal static bool TryConvertToBoolean(string input, out bool result)
        {
            // Remove whitespace from string and lowercase
            input = input.Trim().ToLower();

            if (_trueStrings.Contains(input))
            {
                result = true;
                return true;
            }

            if (_falseStrings.Contains(input))
            {
                result = false;
                return true;
            }

            if (Int32.TryParse(input, out var intVal))
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