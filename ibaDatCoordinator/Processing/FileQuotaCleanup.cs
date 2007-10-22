using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

using iba.Data;

namespace iba.Processing
{
    public class FileQuotaCleanup
    {
        private TaskDataUNC m_task;
        private List<String> m_files;
        string m_extension;
        UInt64 m_size;

        public FileQuotaCleanup(TaskDataUNC task, string extension)
        {
            m_task = task;
            m_files = new List<String>();
            m_extension = extension;
            Reset();
            //TODO: set up lists;
        }

        public void AddFile(string filename)
        {
            try
            {
                FileInfo inf = new FileInfo(filename);
                m_size += (ulong)inf.Length;
                m_files.Add(filename);
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

        private void Reset()
        {
            m_size = 0;
            m_files.Clear();
            try
            {
                foreach (string file in Directory.GetFiles(m_task.DestinationMapUNC,"*" + m_extension,SearchOption.AllDirectories))
                {
                    try
                    {
                        FileInfo inf = new FileInfo(file);
                        m_files.Add(file);
                        m_size += (ulong)inf.Length;
                    }
                    catch
                    {
                    }
                }
                m_files.Sort(delegate(string file1, string file2)
                {
                    return File.GetLastWriteTime(file1).CompareTo(File.GetLastWriteTime(file2));
                }
                );
            }
            catch
            {
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
            while (m_size > (m_task.Quota * 1024 * 1024) && m_files.Count > 0)
            {
                string file = m_files[0];
                try
                {
                    if (!File.Exists(file))
                    {
                        Reset();
                        continue;
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
                    while (parent.GetDirectories().Length == 0 && parent.GetFiles().Length == 0 && parent.FullName != origPath)
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
                m_files.RemoveAt(0);
            }
        }
    }
}
