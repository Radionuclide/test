using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using iba.Plugins;

namespace ExamplePlugin
{
    public class PluginCopyTaskWorker : IPluginTaskWorker
    {
        private IDatCoHost m_datcoHost;
        private IJobData m_parentJob;
        private PluginCopyTask m_data;
        
        public PluginCopyTaskWorker(IDatCoHost host, PluginCopyTask data, IJobData parentJob )
        {
            m_datcoHost = host;
            m_data = data;
            m_parentJob = parentJob;
            m_error = null;
        }

        #region IPluginTaskWorker Members

        public bool OnStart()
        {
            m_error = null;
            //nothing do initialise
            return true;
        }

        public bool OnStop()
        {
            m_error = null;
            //nothing to finalize
            return true;
        }

        public bool OnApply(IPluginTaskData data, IJobData parentJob)
        {
            PluginCopyTask data2 = data as PluginCopyTask; 
            if (data2 != null)
                m_data = data2;
            m_parentJob = parentJob;
            m_error = null;
            return true;           
        }

        public bool ExecuteTask(string filename)
        {
            m_error = null;
            string dir = m_data.DestinationMap;
            if (!Path.IsPathRooted(dir))
            {  //get Absolute path relative to dir
                dir = Path.Combine(m_parentJob.DatDirectoryUNC, dir);
            }
            else dir = m_data.DestinationMapUNC;
            if (m_parentJob.SubDirs && m_data.Subfolder == PluginCopyTask.SubfolderChoiceC.SAME) //concatenate subfolder corresponding to dat subfolder
            {
                string s2 = Path.GetFullPath(m_parentJob.DatDirectoryUNC);
                string s1 = Path.GetFullPath(filename);
                string s0 = s1.Remove(0, s2.Length + 1);
                dir = Path.GetDirectoryName(Path.Combine(dir, s0));
            }
            if (m_data.Subfolder != PluginCopyTask.SubfolderChoiceC.NONE && m_data.Subfolder != PluginCopyTask.SubfolderChoiceC.SAME)
            {
                dir = Path.Combine(dir, SubFolder(m_data.Subfolder));
            }
            if (!Directory.Exists(dir))
            {
                try
                {
                    Directory.CreateDirectory(dir);
                }
                catch
                {
                    bool failed = true;
                    if (m_datcoHost.TryReconnect(dir, m_data.Username, m_data.Password))
                    {
                        failed = false;
                        if (!Directory.Exists(dir))
                        {
                            try
                            {
                                Directory.CreateDirectory(dir);
                            }
                            catch
                            {
                                failed = true;
                            }
                        }
                    }
                    if (failed)
                    {
                        m_error = "Failed to create destination directory: " + dir;
                        return false;
                    }
                }
            }

            if (m_data.Subfolder != PluginCopyTask.SubfolderChoiceC.NONE)
            {
                List<string> subdirs = new List<string>(Directory.GetDirectories(Directory.GetParent(dir).FullName));
                while (subdirs.Count > m_data.SubfoldersNumber)
                {
                    DateTime oldestDate = DateTime.MaxValue;
                    string candidate = subdirs[0];
                    foreach (string sd in subdirs)
                        if (sd.CompareTo(candidate)<0) candidate = sd;
                    try
                    {
                        Directory.Delete(candidate, true);
                    }
                    catch
                    {
                        m_error = "Failed to remove old directory " + candidate;
                        return false;
                    }
                    subdirs.Remove(candidate);
                }
            }

            string dest = Path.Combine(dir, Path.GetFileName(filename));

            try
            {
                if (m_data.RemoveSource)
                {
                    File.Copy(filename, dest, true);
                    File.Delete(filename);
                }
                else
                {
                    File.Copy(filename, dest, true);
                }
            }
            catch
            {
                if (m_data.RemoveSource)
                {
                    m_error = "Could not move " + filename + " to directory " + dest;                    
                }
                else
                {
                    m_error = "Could not copy " + filename + " to directory " + dest;
                }
                return false;
            }
            return true;
        }

        private string SubFolder(PluginCopyTask.SubfolderChoiceC choice)
        {
            DateTime now = DateTime.Now;
            switch (choice)
            {
                case PluginCopyTask.SubfolderChoiceC.HOUR:
                    return now.ToString("yyMMddHH");
                case PluginCopyTask.SubfolderChoiceC.DAY:
                    return now.ToString("yyMMdd");
                case PluginCopyTask.SubfolderChoiceC.MONTH:
                    return now.ToString("yyMM");
                case PluginCopyTask.SubfolderChoiceC.WEEK:
                    {
                        int weekNr = GetWeekNumber(now);
                        return (now.Year - 2000).ToString("d2") + weekNr.ToString("d2");
                    }
                default:
                    return null;
            }
        }

        private int GetWeekNumber(DateTime date)
        {
            // Get jan 1st of the year
            DateTime startOfYear = new DateTime(date.Year, 1, 1);
            // Get dec 31st of the year
            DateTime endOfYear = new DateTime(date.Year, 12, 31);

            // ISO 8601 weeks start with Monday 
            // The first week of a year includes the first Thursday 
            // DayOfWeek returns 0 for sunday up to 6 for saterday
            int[] iso8601Correction = { 6, 7, 8, 9, 10, 4, 5 };
            int nds = date.Subtract(startOfYear).Days + iso8601Correction[(int)startOfYear.DayOfWeek];
            int wk = nds / 7;
            if (wk == 0)
                // Return weeknumber of dec 31st of the previous year
                return GetWeekNumber(new DateTime(date.Year - 1, 12, 31));
            else if ((wk == 53) && (endOfYear.DayOfWeek < DayOfWeek.Thursday))
                // If dec 31st falls before thursday it is week 01 of next year
                return 1;
            else
                return wk;
        }

        private string m_error;

        public string GetLastError()
        {
            string error = m_error;
            m_error = null;
            return error;
        }

        #endregion

        #region IPluginTaskWorker Members


        public PluginTaskWorkerStatus GetWorkerStatus()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        #endregion
    }
}
