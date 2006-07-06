using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iba.Data;
using IBAFILESLib;

namespace iba.Controls
{
    public partial class RemoveMarkingsDialog : Form
    {
        List<string> m_files;
        public RemoveMarkingsDialog(ConfigurationData data)
        {
            InitializeComponent();
            m_files = new List<string>();
            findFiles(data); 
        }

        private void findFiles(ConfigurationData data)
        {
            string datDir = data.DatDirectory;
            if (Directory.Exists(datDir))
            {
                DirectoryInfo dirInfo = new DirectoryInfo(datDir);
                FileInfo[] fileInfos = dirInfo.GetFiles("*.dat", data.SubDirs ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
                foreach (FileInfo fi in fileInfos)
                {
                    m_files.Add(fi.FullName);
                }
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            IbaFileUpdater ibaDatFile = new IbaFileClass();
            for (int count = 0; count < m_files.Count; count++)
            {
                string filename = m_files[count];
                DateTime time = DateTime.Now;
                try
                {
                    time = File.GetLastWriteTime(filename);
                    ibaDatFile.OpenForUpdate(filename);
                    ibaDatFile.WriteInfoField("status", "readyToProcess");
                    ibaDatFile.WriteInfoField("TasksDone", "");
                    backgroundWorker1.ReportProgress(0, new ProgressData(count, filename));
                }
                catch (Exception ex)//updating didn't work, forget about it
                {
                    string message = ex.Message;
                }
                finally
                {
                    ibaDatFile.Close();
                }
                try
                {
                    File.SetLastWriteTime(filename, time);
                }
                catch { }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ProgressData dat = (e.UserState as ProgressData);
            int progress = (int)((double)(dat.Count * 100) / (double)(m_files.Count));
            m_fileLabel.Text = dat.FileName;
            m_progressBar.Value = progress;
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
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
}