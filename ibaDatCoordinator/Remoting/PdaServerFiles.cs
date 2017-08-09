using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using iba.Utility;

namespace iba.Remoting
{
    #region IPdaServerFiles
    public interface IPdaServerFiles
    {
        //string GetUNCPath(string localPath);
        //string GetLocalPath(string uncPath);

        ////Get info of all files in basePath with extension
        ////Extension is passed to DirectoryInfo.GetFiles(extension)!!!
        //ServerFileInfo[] GetFilesInfo(string basePath, string extension);
        IFileDownload DownloadFile(string localFileName, out long fileSize);
        void UploadFile(string destPath, IFileDownload file, long fileSize);
        void UploadFile(string[] destPathArray, IFileDownload file, long fileSize);

        string[] BrowseForDrives(bool onlyFixed);
        FileSystemEntry[] Browse(string basePath, bool includeFiles, string extensions="");

        //bool TestPath(string path, string username, string password, bool bCreatePath, out string errorMsg);
        //bool GetFreeSpace(string path, string username, string password, out UInt64 avBytes, out UInt64 totalBytes);
    }

    [Serializable]
    public class ServerFileInfo
    {
        public string LocalFileName;
        public string UNCFileName;
        public DateTime LastWriteTimeUtc;
        public long FileSize;

        public ServerFileInfo(string fileName)
        {
            LocalFileName = fileName;
            UNCFileName = iba.Utility.Shares.PathToUnc(fileName, true);
            System.IO.FileInfo fi = new System.IO.FileInfo(fileName);
            LastWriteTimeUtc = fi.LastWriteTimeUtc;
            FileSize = fi.Length;
        }

        public ServerFileInfo(string fileName, DateTime lastWrite, long size)
        {
            LocalFileName = fileName;
            UNCFileName = iba.Utility.Shares.PathToUnc(fileName, true);
            LastWriteTimeUtc = lastWrite;
            FileSize = size;
        }
    }

    [Serializable]
    public class FileSystemEntry
    {
        public string Name;
        public bool IsFile;

        public FileSystemEntry(string name, bool isFile)
        {
            Name = name;
            IsFile = isFile;
        }
    }

    #endregion

    #region PdaServerFiles
    public class PdaServerFiles : MarshalByRefObject, IPdaServerFiles
    {
        public FileSystemEntry[] Browse(string basePath, bool includeFiles, string extension)
        {
            DirectoryInfo baseDir = new DirectoryInfo(basePath);
            DirectoryInfo[] subDirs = baseDir.GetDirectories();
            FileInfo[] files;
            if (includeFiles)
                files = string.IsNullOrEmpty(extension)?baseDir.GetFiles():baseDir.GetFiles(extension);
            else
                files = new FileInfo[] { };

            FileSystemEntry[] entries = new FileSystemEntry[subDirs.Length + files.Length];
            for (int i = 0; i < subDirs.Length; i++)
            {
                entries[i] = new FileSystemEntry(subDirs[i].Name, false);
            }
            int index = subDirs.Length;
            for (int i = 0; i < files.Length; i++)
                entries[index + i] = new FileSystemEntry(files[i].Name, true);
            return entries;
        }

        public string[] BrowseForDrives(bool onlyFixed)
        {
            var allDrives = Directory.GetLogicalDrives();
            if (!onlyFixed)
                return allDrives;
            var fixedDrives = new List<string>(allDrives.Length);
            foreach (string drive in allDrives)
            {
                if (GetDriveType(drive) == 3)
                    fixedDrives.Add(drive);
            }
            if (fixedDrives.Count > 0) return fixedDrives.ToArray();
            else return new string[] { null };
        }

        [DllImport("kernel32.dll", EntryPoint = "GetDriveTypeW", CharSet = CharSet.Unicode, SetLastError = true, BestFitMapping = false)]
        internal static extern int GetDriveType(string drive);

        public IFileDownload DownloadFile(string localFileName, out long fileSize)
        {
            localFileName = NormalizePath(localFileName);
            FileStream file = File.Open(localFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            fileSize = file.Length;
            return new FileUploaderImpl(file);
        }

        internal static string NormalizePath(string localFileName)
        {
            return localFileName.Replace("%serverpath%", PathUtil.rootPath);
        }

        public void UploadFile(string destPath, IFileDownload file, long fileSize)
        {
            destPath = NormalizePath(destPath);
            String dir = Path.GetDirectoryName(destPath);
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
            FileStream stream = File.Create(destPath);
            new FileDownloaderImpl(stream, fileSize, file);
        }

        public void UploadFile(string[] destPathArray, IFileDownload file, long fileSize)
        {
            if (destPathArray.Length == 0)
                return;
            new FileDownloaderImpl(destPathArray, fileSize, file);
        }
    }
    #endregion
}
