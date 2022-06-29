using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip;

namespace iba.Utility
{
    internal static class ZipCreator
    {
        public static string CreateZipArchive(string sourceFile, string compressedFile = "", string entryName = "")
        {
            if (string.IsNullOrEmpty(compressedFile))
            {
                compressedFile = sourceFile + ".zip";
            }

            if (string.IsNullOrEmpty(entryName))
            {
                entryName = Path.GetFileName(sourceFile);
            }

            using (var archive = ZipFile.Create(compressedFile))
            {
                IStaticDataSource dataSource = new StaticDiskDataSource(sourceFile);

                archive.BeginUpdate();
                archive.Add(dataSource, entryName);
                archive.CommitUpdate();
            }

            return compressedFile;
        }
    }
}
