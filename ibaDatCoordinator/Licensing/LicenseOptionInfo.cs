using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Licensing
{
    public enum LicenseOptionType
    {
        Counter,
        YesNo
    }

    public class LicenseOptionInfo
    {
        public LicenseOptionInfo(int id, string text, LicenseOptionType optionType, int[] marxIds = null, int[] optionalWibuIds = null, int analyzerTransferId = -1)
        {
            Id = id;
            Text = text;
            OptionType = optionType;
            MarxIds = marxIds;
            OptionalWibuIds = optionalWibuIds;
            AnalyzerLicenseTransferId = analyzerTransferId;
        }

        public readonly int Id;

        public readonly LicenseOptionType OptionType;

        /// <summary>
        /// null: no Marx byte exists
        /// 0 - 255: The index of the corresponding Marx dongle byte.
        /// >= 1000: MarxId/1000 corresponds to Marx dongle byte and MarxId%1000 is the bit number
        /// </summary>
        public readonly int[] MarxIds;

        /// <summary>
        /// Optional wibu product codes that are acquired in station share mode when the main product code isn't available
        /// </summary>
        public readonly int[] OptionalWibuIds;

        public readonly string Text;

        /// <summary>
        /// Id used to transfer license to ibaAnalyzer. -1 if transfer isn't supported
        /// </summary>
        public readonly int AnalyzerLicenseTransferId;

        public override string ToString() => Text;

        public static LicenseOptionInfo[] Infos = new LicenseOptionInfo[] {
            new LicenseOptionInfo(LicenseId.DBExtract, "Database extract", LicenseOptionType.Counter, new int[] {49, 169 }, new int[] {LicenseId.AnalyzerDBExtract }, 5),
            new LicenseOptionInfo(LicenseId.FileExtract, "File extract", LicenseOptionType.Counter, new int[] {50, 170 }, new int[] {LicenseId.AnalyzerFileExtract }, 3),

            new LicenseOptionInfo(LicenseId.Publish, "Publish task", LicenseOptionType.Counter,null,null,9),
            new LicenseOptionInfo(LicenseId.ConvertCSV, "Convert CSV to dat", LicenseOptionType.Counter,null,null,10),
            new LicenseOptionInfo(LicenseId.ConvertDAS, "Convert DAS to dat", LicenseOptionType.Counter,null,null,11),
            new LicenseOptionInfo(LicenseId.ConvertCOMTRADE, "Convert COMTRADE to dat", LicenseOptionType.Counter,null,null,12),
            new LicenseOptionInfo(LicenseId.ConvertPARQUET, "Convert PARQUET to dat", LicenseOptionType.Counter,null,null,13),

            new LicenseOptionInfo(LicenseId.SinecH1, "Sinec H1 task", LicenseOptionType.YesNo, new int[] {61001 }),
            new LicenseOptionInfo(LicenseId.UpdateData, "Update data task", LicenseOptionType.YesNo, new int[] {61002 }),
            new LicenseOptionInfo(LicenseId.Roh, "Roh task", LicenseOptionType.YesNo, new int[] {61003 }),
            new LicenseOptionInfo(LicenseId.XmlExport, "XML export task", LicenseOptionType.YesNo, new int[] {61004 }),
            new LicenseOptionInfo(LicenseId.OSPC, "AM OSPC task", LicenseOptionType.YesNo, new int[] {61005 }),
            new LicenseOptionInfo(LicenseId.S7Writer, "S7 writer task", LicenseOptionType.YesNo, new int[] {61006 })
        };

        public static LicenseOptionInfo GetOptionInfo(int licenseId)
        {
            return Infos.FirstOrDefault(l => l.Id == licenseId);
        }

        public static int ConvertMarxPluginBitToLicenseId(int dongleBitPos)
        {
            if (dongleBitPos <= 0)
                return LicenseId.None;

            if (dongleBitPos < 8)
            {
                dongleBitPos += 61000;
                var info = Infos.FirstOrDefault(l => l.MarxIds != null && l.MarxIds.Contains(dongleBitPos));
                if (info != null)
                    return info.Id;
            }

            //A Wibu product code or an unknown dongle bit
            return dongleBitPos;
        }
    }
}
