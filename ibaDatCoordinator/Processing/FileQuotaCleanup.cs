using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using iba.Data;
using System.Runtime.InteropServices;

namespace iba.Processing
{
    public class FileQuotaCleanup
    {
        private TaskDataUNC m_task;
        private LinkedList<String> m_files;
        string m_extension;
        UInt64 m_size;

        public FileQuotaCleanup(TaskDataUNC task, string extension)
        {
            m_task = task;
            m_files = new LinkedList<String>();
            m_extension = extension;
            Reset();
        }

        public void AddFile(string filename, bool bAddFileName)
        {
            try
            {
                FileInfo inf = new FileInfo(filename);
                m_size += (ulong)inf.Length;
                if (bAddFileName)
                    m_files.AddLast(filename);
            }
            catch
            {
            }
        }

        public void RemoveFile(string filename)
        {
            try
            {
                FileInfo inf = new FileInfo(filename);
                m_size -= (ulong)inf.Length;
            }
            catch
            {
            }
        }

        public void ResetTask(TaskDataUNC newTask, string extension)
        {
            if (newTask.DestinationMapUNC != m_task.DestinationMapUNC || newTask.Quota < m_task.Quota || extension != m_extension)
            {
                m_extension = extension;
                m_task = newTask;
                Reset();
            }
        }
        
        struct DateAndName
        {
            public DateTime time;
            public string filename;
        };

        private void Reset()
        {
            m_size = 0;
            m_files.Clear();
            Log(iba.Logging.Level.Info, iba.Properties.Resources.determiningQuota, "");

            List<DateAndName> DateAndNames = new List<DateAndName>();

            try
            {
                DateAndName datNam = new DateAndName();
                foreach (string dir in Directory.GetDirectories(m_task.DestinationMapUNC))
                {
                    try
                    {
                        foreach (string file in Directory.GetFiles(dir, "*" + m_extension, SearchOption.AllDirectories))
                        {
                            try
                            {
                                FileInfo inf = new FileInfo(file);
                                datNam.filename = file;
                                datNam.time = inf.LastWriteTime;
                                DateAndNames.Add(datNam);
                                m_size += (ulong)inf.Length;

                            }
                            catch (Exception ex)
                            {
                                Log(iba.Logging.Level.Exception, ex.Message + " (getting directories(1))", "");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log(iba.Logging.Level.Exception, ex.Message + " (getting directories(2))", "");
                    }
                }
                foreach (string file in Directory.GetFiles(m_task.DestinationMapUNC,"*" + m_extension,SearchOption.TopDirectoryOnly))
                {
                    try
                    {
                        FileInfo inf = new FileInfo(file);
                        datNam.filename = file;
                        datNam.time = inf.LastWriteTime;
                        DateAndNames.Add(datNam);
                        m_size += (ulong)inf.Length;

                    }
                    catch (Exception ex)
                    {
                        Log(iba.Logging.Level.Exception, ex.Message + " (getting files in directories)", "");
                    }
                }
                //Log(iba.Logging.Level.Exception, String.Format("size after counting {0}",m_size) , "");

                DateAndNames.Sort(delegate(DateAndName f1, DateAndName f2)
                {
                    return f1.time.CompareTo(f2.time);
                }); //oldest files last


                foreach (DateAndName entry in DateAndNames)
                    m_files.AddLast(entry.filename);



                //Log(iba.Logging.Level.Exception, "finished sorting", "");
            }
            catch (Exception)
            {
                //Log(iba.Logging.Level.Exception, "General exception while tallying the files for cleanup: " + ex.Message, "");
                m_files.Clear();
            }
        }

        private void Log(Logging.Level level, string message, string datfile)
        {
            if (LogData.Data.Logger.IsOpen)
            {
                LogExtraData data = new LogExtraData(datfile, m_task, m_task.ParentConfigurationData);
                LogData.Data.Logger.Log(level, message, (object)data);
            }
        }

        public void Clean(string datfile)
        {
            //bool bFirst = true;
            while (m_size > (((ulong) (m_task.Quota)) * 1024 * 1024) && m_files.Count > 0)
            {
                //if (bFirst)
                //{
                //    //string message = String.Format("Quota Exceeded: filesize: {0}  Quota: {1} Cleaning up files", m_size, m_task.Quota);
                //    //Log(iba.Logging.Level.Warning, message, datfile);
                //    bFirst = false;
                //}
                string file = m_files.First.Value;
                try
                {
                    if (!File.Exists(file))
                    {
                        if (Directory.Exists(m_task.DestinationMapUNC))
                        { //file was deleted manually
                            //string message = String.Format("File {0} does not exist anymore", file);
                            //Log(iba.Logging.Level.Warning, message, datfile);
                            Reset();
                            continue;
                        }
                        else
                        {
                            //string message = String.Format("File {0} does not exist anymore, neither does the folder {1}, Cleanup suspended", file, m_task.DestinationMapUNC);
                            //Log(iba.Logging.Level.Warning, message, datfile);
                            return; //forget about cleanup until problem is fixed.
                        }
                    }
                    string origPath = (new DirectoryInfo(m_task.DestinationMapUNC)).FullName;
                    FileInfo inf = new FileInfo(file);
                    DirectoryInfo parent = inf.Directory;
                    ulong size = (ulong) inf.Length;
                    try
                    {

                        File.Delete(file);
                        string message = String.Format(iba.Properties.Resources.logCleanupSuccessRemoveFile, file);
                        Log(iba.Logging.Level.Info, message, datfile);
                        //Log(iba.Logging.Level.Exception, String.Format("size before deleting {0}, filesize {1}, difference {2}", m_size, size, m_size - size), file);
                    }
                    catch (Exception ex)
                    {
                        string message = String.Format(iba.Properties.Resources.logCleanupErrorRemoveFile, file, ex.Message);
                        Log(iba.Logging.Level.Warning, message, datfile);
                        throw ex;
                    }
                    if (size > m_size)
                        m_size = 0;
                    else
                        m_size -= size;
                    while (DirectoryEmpty(parent.FullName) && parent.FullName != origPath)
                    {
                        DirectoryInfo grandParent = parent.Parent;
                        try
                        {
                            Directory.Delete(parent.FullName);
                            string message = String.Format(iba.Properties.Resources.logCleanupSuccessRemoveEmptyDirectory, parent.FullName);
                            Log(iba.Logging.Level.Info, message, datfile);
                        }
                        catch (Exception ex)
                        {
                            string message = String.Format(iba.Properties.Resources.logCleanupErrorRemoveDirectory, parent.FullName, ex.Message);
                            Log(iba.Logging.Level.Warning, message, datfile);
                            throw ex;
                        }
                        parent = grandParent;
                    }
                }
                catch
                {
                }
                m_files.RemoveFirst();
            }
            //if (bFirst)
            //{
            //    string message = String.Format("Quota Not Exceeded: filesize: {0}  Quota: {1}", m_size, m_task.Quota);
            //    Log(iba.Logging.Level.Warning, message, datfile);
            //}
        }

        [StructLayout(LayoutKind.Sequential)]
        struct FILETIME
        {
            public uint dwLowDateTime;
            public uint dwHighDateTime;
        };
        const int MAX_PATH = 260;
        const int MAX_ALTERNATE = 14;
        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
        struct WIN32_FIND_DATA
        {
            public FileAttributes dwFileAttributes;
            public FILETIME ftCreationTime;
            public FILETIME ftLastAccessTime;
            public FILETIME ftLastWriteTime;
            public int nFileSizeHigh;
            public int nFileSizeLow;
            public int dwReserved0;
            public int dwReserved1;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_PATH)]
            public string cFileName;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = MAX_ALTERNATE)]
            public string cAlternate;
        }

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern IntPtr FindFirstFile(string lpFileName, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        static extern bool FindNextFile(IntPtr hFindFile, out WIN32_FIND_DATA lpFindFileData);

        [DllImport("kernel32.dll")]
        static extern bool FindClose(IntPtr hFindFile);


        private bool DirectoryEmpty(string directory)
        {
            IntPtr INVALID_HANDLE_VALUE = new IntPtr(-1);
            WIN32_FIND_DATA findData;
            IntPtr findHandle = FindFirstFile(directory + @"\*", out findData);
            if (findHandle == INVALID_HANDLE_VALUE) return false; //not a valid directory

            if ((findData.dwFileAttributes & FileAttributes.Directory) != 0 && (findData.cFileName == "." || findData.cFileName == ".."))
                FindNextFile(findHandle, out findData);
            else
            {
                FindClose(findHandle);
                return false;
            }
            if (findHandle == INVALID_HANDLE_VALUE) return false; //not a valid directory
            if ((findData.dwFileAttributes & FileAttributes.Directory) != 0 && (findData.cFileName == "." || findData.cFileName == ".."))
            {
                if (!FindNextFile(findHandle, out findData))
                {
                    FindClose(findHandle);
                    return true;
                }
                else
                {
                    FindClose(findHandle);
                    return false;
                }
            }
            else
            {
                FindClose(findHandle);
                return false;
            }
        }


    }
}
