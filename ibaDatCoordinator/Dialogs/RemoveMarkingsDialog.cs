using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iba.Data;
//using ibaFilesLiteLib;
using iba.ibaFilesLiteDotNet;
using System.Threading;

namespace iba.Dialogs
{
    public partial class RemoveMarkingsDialog : Form
    {
        List<string> m_files;
        string m_username;
        string m_pass;
        string m_path;
        private bool runswithservice;
        private bool m_bRecursive;

        public RemoveMarkingsDialog(ConfigurationData data)
        {
            InitializeComponent();
            runswithservice = (Program.RunsWithService == Program.ServiceEnum.CONNECTED);
            m_path = data.DatDirectoryUNC;
            m_username = data.Username;
            m_pass = data.Password;
            m_bRecursive = data.SubDirs;
            if (!runswithservice)
            {
                m_files = new List<string>();
                findFiles(data);
            }
            else
                m_files = null;
        }

        public RemoveMarkingsDialog(string path, string username, string pass, List<string> files)
        {
            InitializeComponent();
            m_username = username;
            m_pass = pass;
            m_path = path;
            m_files = files;
            m_bRecursive = false;
            runswithservice = (Program.RunsWithService == Program.ServiceEnum.CONNECTED);
        }

        private void findFiles(ConfigurationData data)
        {
            string datDir = data.DatDirectory;
            if (Directory.Exists(datDir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(datDir);
                try
                {
                    if (data.SubDirs)
                    {
                        List<FileInfo> fileInfos = Utility.PathUtil.GetFilesInSubsSafe("*.dat",dirInfo);
                        foreach (FileInfo fi in fileInfos)
                        {
                            m_files.Add(fi.FullName);
                        }
                    }
                    else
                    {
                    //List<FileInfo> fileInfos = Utility.FileUtilities.GetFilesSave(
                        FileInfo[] fileInfos = dirInfo.GetFiles("*.dat", SearchOption.TopDirectoryOnly);
                        foreach (FileInfo fi in fileInfos)
                        {
                            m_files.Add(fi.FullName);
                        }
                    }
                }
                catch //ignore directories one has no access to
                {

                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            backgroundWorker1.RunWorkerAsync();
        }

        private bool m_stop;

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (runswithservice)
            {
                var pb = new RemoveMarkingsProgressBar(this);
                try
                {
                    if (m_files == null)
                        Program.CommunicationObject.RemoveMarkingsAsync(m_path, m_username, m_pass, m_bRecursive, pb);
                    else
                        Program.CommunicationObject.RemoveMarkingsAsync(m_path, m_username, m_pass, m_files, pb);
                    while (!pb.Finished)
                    {
                        Thread.Sleep(1000);
                    }
                }
                catch (Exception) //try old way in case of server client discrepancy
                {
                    if (m_files == null)
                        Program.CommunicationObject.RemoveMarkings(m_path, m_username, m_pass, m_bRecursive, pb);
                    else
                        Program.CommunicationObject.RemoveMarkings(m_path, m_username, m_pass, m_files, pb);
                }
            }
            else
            {
                IbaFileReader ibaDatFile = new IbaFileReader();
                m_stop = false;
                for (int count = 0; count < m_files.Count && !m_stop; count++)
                {
                    string filename = m_files[count];
                    DateTime time = DateTime.Now;
                    try
                    {
                        time = File.GetLastWriteTime(filename);
                        if(filename.EndsWith(".hdq"))
                        {
                            iba.Utility.IniParser parser = new iba.Utility.IniParser(filename);
                            parser.Read();
                            parser.Sections["DatCoordinatorData"]["$DATCOOR_status"] = "readyToProcess";
                            parser.Sections["DatCoordinatorData"]["$DATCOOR_TasksDone"] = "";
                            parser.Sections["DatCoordinatorData"]["$DATCOOR_times_tried"] = "0";
                            parser.Sections["DatCoordinatorData"]["$DATCOOR_OutputFiles"] = "";
                            parser.Write();
                        }
                        else
                        {
                            ibaDatFile.OpenForUpdate(filename);
                            ibaDatFile.InfoFields["$DATCOOR_status"] = "readyToProcess";
                            if (!ibaDatFile.InfoFields.ContainsKey("$DATCOOR_times_tried"))
                                ibaDatFile.InfoFields["$DATCOOR_times_tried"] = "0";
                            iba.Processing.ConfigurationWorker.ClearFields(ref ibaDatFile);
                        }
                        backgroundWorker1.ReportProgress(0, new ProgressData(count, filename));
                    }
                    catch(Exception ex)//updating didn't work, forget about it
                    {
                        string message = ex.Message;
                    }
                    finally
                    {
                        try
                        {
                            ibaDatFile.Close();
                        }
                        catch (Exception ex)//updating didn't work, forget about it
                        {
                            string message = ex.Message;
                        }
                    }
                    try
                    {
                        File.SetLastWriteTime(filename, time);
                    }
                    catch { }
                }
            }
        }

        private int filecount;
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressData dat = (e.UserState as ProgressData);
            if (dat.FileName == "filescount")
            {
                filecount = dat.Count;
                return;
            }
            int progress = (int)((double)(dat.Count * 100) / (double)(m_files==null?filecount:m_files.Count));
            m_fileLabel.Text = dat.FileName;
            m_progressBar.Value = progress;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            m_stop = true;
        }

        internal BackgroundWorker BW
        {
            get {return backgroundWorker1;}
        }

        internal bool Stop
        {
            get { return m_stop; }
        }
    }

    internal class ProgressData
    {
        private int m_count;
        public int Count
        {
            get { return m_count; }
            set { m_count = value; }
        }

        private string m_fileName;
        public string FileName
        {
            get { return m_fileName; }
            set { m_fileName = value; }
        }

        public ProgressData(int count, string filename)
        {
            m_count = count;
            m_fileName = filename;
        }
    }

    public class RemoveMarkingsProgressBar : FileProcessingProgressBar
    {
        public RemoveMarkingsProgressBar(RemoveMarkingsDialog parent)
        {
            m_parent = parent;
        }
        RemoveMarkingsDialog m_parent;

        public override bool UpdateProgress(string file, int count)
        {
            MethodInvoker m = delegate()
            {
                m_parent.BW.ReportProgress(0, new ProgressData(count, file));
            };
            m_parent.Invoke(m);
            return m_parent.Stop;
        }
    }
}