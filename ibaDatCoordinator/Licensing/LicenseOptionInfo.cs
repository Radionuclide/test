using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Licensing
{
    public class LicenseOptionInfo
    {
        public LicenseOptionInfo(int id, string text, int[] marxIds = null, int[] optionalWibuIds = null)
        {
            Id = id;
            Text = text;
            MarxIds = marxIds;
            OptionalWibuIds = optionalWibuIds;
        }

        public readonly int Id;

        /// <summary>
        /// null: no Marx byte exists
        /// 0 - 255: The index of the corresponding Marx dongle byte.
        /// >= 1000: MarxId/1000 corresponds to Marx dongle byte and MarxId%1000 is the bit number
        /// </summary>
        public readonly int[] MarxIds;

        public readonly int[] OptionalWibuIds;

        public readonly string Text;
        //public LicenseType Type;

        public override string ToString() => Text;

        public static LicenseOptionInfo[] Infos = new LicenseOptionInfo[] {
            new LicenseOptionInfo(LicenseId.DBExtract, "Database extract", new int[] {49, 169 }, new int[] {LicenseId.AnalyzerDBExtract }),
            new LicenseOptionInfo(LicenseId.FileExtract, "File extract", new int[] {50, 170 }, new int[] {LicenseId.AnalyzerFileExtract }),

            new LicenseOptionInfo(LicenseId.ConvertCSV, "Convert CSV to dat"),
            new LicenseOptionInfo(LicenseId.ConvertDAS, "Convert DAS to dat"),
            new LicenseOptionInfo(LicenseId.ConvertCOMTRADE, "Convert COMTRADE to dat"),
            new LicenseOptionInfo(LicenseId.Publish, "Publish task"),

            new LicenseOptionInfo(LicenseId.SinecH1, "Sinec H1 task", new int[] {61001 }),
            new LicenseOptionInfo(LicenseId.UpdateData, "Update data task", new int[] {61002 }),
            new LicenseOptionInfo(LicenseId.Roh, "Roh task", new int[] {61003 }),
            new LicenseOptionInfo(LicenseId.XmlExport, "XML export task", new int[] {61004 }),
            new LicenseOptionInfo(LicenseId.OSPC, "AM OSPC task", new int[] {61005 }),
            new LicenseOptionInfo(LicenseId.S7Writer, "S7 writer task", new int[] {61006 })
        };

        public static LicenseOptionInfo GetOptionInfo(int licenseId)
        {
            return Infos.FirstOrDefault(l => l.Id == licenseId);
        }
    }
}
