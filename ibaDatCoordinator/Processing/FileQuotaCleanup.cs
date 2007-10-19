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
            throw new Exception("The method or operation is not implemented.");
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

        public void Clean()
        {
            while (m_size > m_task.Quota && m_files.Count > 0)
            {
                string file = m_files[0];
                try
                {
                    string origPath = (new DirectoryInfo(m_task.DestinationMapUNC)).FullName;
                    FileInfo inf = new FileInfo(file);
                    DirectoryInfo parent = inf.Directory;
                    ulong size = (ulong) inf.Length;
                    File.Delete(file);
                    if (size > m_size)
                        m_size = 0;
                    else
                        m_size -= size;
                    while (parent.GetDirectories().Length == 0 && parent.GetFiles().Length == 0 && parent.FullName != origPath)
                    {
                        DirectoryInfo grandParent = parent.Parent;
                        Directory.Delete(parent.FullName);
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
