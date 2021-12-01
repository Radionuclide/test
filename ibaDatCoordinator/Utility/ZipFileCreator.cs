using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace iba.Utility
{
    public class ZipFileCreator : IDisposable
    {

        public ZipFileCreator(string fileName)
        {
            zipFile = new ZipArchive(File.Create(fileName), ZipArchiveMode.Create);
        }

        //Function added for C++ CLI because Dispose can't be called directly only via delete.
        public void Close()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (zipFile == null)
                return;

            zipFile.Dispose();
            zipFile = null;
        }

        private ZipArchive zipFile;

        public void AddFile(string fileName)
        {
            AddFile(fileName, Path.GetFileName(fileName));
        }

        public void AddFile(string fileName, string relName)
        {
            //This is a copy of the CreateEntryFromFile extension method that also works for files that are being written to!
            using (Stream stream = File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                CompressionLevel level = CompressionLevel.Optimal;
                if (String.Compare(Path.GetExtension(fileName), ".zip", true) == 0)
                    level = CompressionLevel.NoCompression;

                ZipArchiveEntry zipArchiveEntry = zipFile.CreateEntry(relName, level);

                DateTime dateTime = File.GetLastWriteTime(fileName);
                if (dateTime.Year < 1980 || dateTime.Year > 2107)
                    dateTime = new DateTime(1980, 1, 1, 0, 0, 0);
                zipArchiveEntry.LastWriteTime = (DateTimeOffset)dateTime;

                using (Stream entryStream = zipArchiveEntry.Open())
                {
                    stream.CopyTo(entryStream);
                }
            }
        }

        public void AddDirectory(string dir, string relName)
        {
            AddDirectory(dir, relName, true);
        }

        public void AddDirectory(string dir, string relName, bool bIncludeSubdirs)
        {
            foreach (string file in Directory.GetFiles(dir))
                AddFile(file, file.Replace(dir, relName));

            if (bIncludeSubdirs)
            {
                foreach (string subDir in Directory.GetDirectories(dir))
                    AddDirectory(subDir, subDir.Replace(dir, relName), true);
            }
        }
    }
}
