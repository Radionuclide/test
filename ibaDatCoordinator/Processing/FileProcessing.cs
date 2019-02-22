using System;
using System.Collections.Generic;
using System.Text;
using iba.Utility;
using System.IO;
using iba.ibaFilesLiteDotNet;

namespace iba.Processing
{
    class FileProcessing : IDisposable
    {
        public FileProcessing(string path, string username, string pass, string filepass)
        {
            m_error = String.Empty;
            m_path = path;
            m_filePass = filepass;
            SharesHandler.Handler.AddReferenceDirect(m_path, username, pass, out m_error);
        }

        #region IDisposable Members

        public void Dispose()
        {
            SharesHandler.Handler.ReleaseReferenceDirect(m_path);
        }

        #endregion

        private string m_path;
        private string m_error;
        private string m_filePass;

        public string ErrorString
        {
            get { return m_error; }
        }

        public void RemoveFiles(List<string> files, FileProcessingProgressBar myBar)
        {
            if (!String.IsNullOrEmpty(m_error)) return;
            bool stop = false;
            for (int count = 0; count < files.Count && !stop; count++)
            {
                string filename = files[count];
                stop = myBar.UpdateProgress(filename, count);
                try
                {
                    if (File.Exists(filename))
                        File.Delete(filename);
                }
                catch { }
            }
            myBar.Finished = true;
        }

        public void RemoveMarkings(List<string> files, FileProcessingProgressBar myBar)
        {
            try
            {
                if (!String.IsNullOrEmpty(m_error) || files.Count == 0)
                    return;

                IbaFileReader ibaDatFile = new IbaFileReader();
                bool stop = false;
                for (int count = 0; count < files.Count && !stop; count++)
                {
                    string filename = files[count];
                    stop = myBar.UpdateProgress(filename, count);
                    RemoveMarkingsFromFile(filename, m_filePass, ibaDatFile);
                }
            }
            finally
            {
                myBar.Finished = true;
            }
        }

        public static string RemoveMarkingsFromFile(string filename, string filepass, IbaFileReader ibaDatFile = null)
        {
            string errMessage = "";
            if(ibaDatFile == null)
            {
                ibaDatFile = new IbaFileReader();
            }

            DateTime time = DateTime.Now;
            try
            {
                time = File.GetLastWriteTime(filename);
                if(filename.EndsWith(".hdq"))
                {
                    IniParser parser = new IniParser(filename);
                    parser.Read();
                    parser.Sections["DatCoordinatorData"]["$DATCOOR_status"] = "readyToProcess";
                    parser.Sections["DatCoordinatorData"]["$DATCOOR_TasksDone"] = "";
                    parser.Sections["DatCoordinatorData"]["$DATCOOR_times_tried"] = "0";
                    parser.Sections["DatCoordinatorData"]["$DATCOOR_OutputFiles"] = "";
                    parser.Write();
                }
                else
                {
                    ibaDatFile.OpenForUpdate(filename, filepass);
                    ibaDatFile.InfoFields["$DATCOOR_status"] = "readyToProcess";
                    ConfigurationWorker.ClearFields(ref ibaDatFile);
                }
            }
            catch(Exception ex)//updating didn't work, forget about it
            {
                errMessage = ex.Message;
            }
            finally
            {
                try
                {
                    ibaDatFile.Close();
                }
                catch(Exception ex)//updating didn't work, forget about it
                {
                    errMessage = ex.Message;
                }
            }
            try
            {
                File.SetLastWriteTime(filename, time);
            }
            catch { }
            return "";
        }

        public List<string> FindFiles(bool recursive)
        {
            if (String.IsNullOrEmpty(m_error))
            {
                List<String> files = new List<string>();
                if (Directory.Exists(m_path))
                {
                    DirectoryInfo dirInfo = new DirectoryInfo(m_path);
                    try
                    {
                        if (recursive)
                        {
                            List<FileInfo> fileInfos = Utility.PathUtil.GetFilesInSubsSafe("*.dat", dirInfo);
                            foreach (FileInfo fi in fileInfos)
                            {
                                files.Add(fi.FullName);
                            }
                        }
                        else
                        {
                            //List<FileInfo> fileInfos = Utility.FileUtilities.GetFilesSave(
                            FileInfo[] fileInfos = dirInfo.GetFiles("*.dat", SearchOption.TopDirectoryOnly);
                            foreach (FileInfo fi in fileInfos)
                            {
                                files.Add(fi.FullName);
                            }
                        }
                    }
                    catch //ignore directories one has no access to
                    {
                    }
                }
                return files;
            }
            else return null; //return an empty list
        }
    }
}
