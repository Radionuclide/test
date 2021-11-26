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
        public static string CreateZipArchive(string sourceFile, string compressedFile = "")
        {
            if (string.IsNullOrEmpty(compressedFile))
            {
                compressedFile = sourceFile + ".zip";
            }

            using (var archive = ZipFile.Create(compressedFile))
            {
                archive.BeginUpdate();
                archive.Add(sourceFile, Path.GetFileName(sourceFile));
                archive.CommitUpdate();
            }

            return compressedFile;
        }
    }
}
