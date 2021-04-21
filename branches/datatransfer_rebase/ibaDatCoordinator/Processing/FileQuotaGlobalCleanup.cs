using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using iba.Data;
using iba.Utility;

namespace iba.Processing
{
    /// <summary>
    /// Deletes only real iba .dat files. Customer might have config files with .dat extension.
    /// </summary>
    internal class FileQuotaGlobalCleanup : FileQuotaCleanup
    {

        public FileQuotaGlobalCleanup(TaskWithTargetDirData task, string extension) 
            : base(task, extension)
        { }

        public FileQuotaGlobalCleanup(TaskDataUNC task, string extension, CancellationToken ct) 
            : base(task, extension, ct)
        { }


        protected override void TraverseFileSystem(List<DateAndName> dateAndNames)
        {
            var dir = String.Empty;
            foreach (var inf in FastDirectoryEnumerator.EnumerateFiles(m_task.DestinationMapUNC, "*" + m_extension, SearchOption.AllDirectories))
            {
                try
                {
                    if (m_cancelToken.IsCancellationRequested)
                        return;

                    m_size += (ulong)inf.Length;
                    dateAndNames.Add(new DateAndName(inf.FullName, inf.LastWriteTime));

                }
                catch (Exception ex)
                {
                    Log(iba.Logging.Level.Exception, String.Format(iba.Properties.Resources.logCleanupTallyingErrorFile, dir) + ex.Message, "");
                    m_failureWhilePreviouslyScanning = true;
                }
            }
        }

        private const int _prefixLength = 3;
        private static readonly char[] _prefix = "PDA".ToCharArray() ; // usually "PDA2" where 2 defines the version  { 0x50, 0x44, 0x41, 0x32 }
        private readonly char[] _buffer = new char[_prefixLength];

        protected override bool IsIbaDatfile(string file)
        {
            // if is a real iba datfile: starts with PDA2
            int numChars = 0;
            using (var reader = new StreamReader(file))
            {
                numChars = reader.Read(_buffer, 0, _prefixLength);
                reader.Close();
            }

            if (numChars != _prefixLength)
                return false;

            for (int i = 0; i < _prefixLength; i++)
            {
                if (_buffer[i] != _prefix[i])
                    return false;
            }

            return true;
        }
    }
}