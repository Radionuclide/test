using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using iba.Data;

namespace iba.Controls
{
    public partial class DeleteDatFilesDialog : Form
    {
        List<string> m_files;
        string m_username;
        string m_pass;
        string m_path;
        
        public DeleteDatFilesDialog(string uncPath, string username, string pass, List<string> files)
        {
            InitializeComponent();
            m_files = files;
            m_path = uncPath;
            m_username = username;
            m_pass = pass;
            runswithservice = false;
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            backgroundWorker1.RunWorkerAsync();
        }

        private bool runswithservice;
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            if (Program.RunsWithService == Program.ServiceEnum.CONNECTED)
            {
                runswithservice = true;
                Program.CommunicationObject.DeleteFiles(m_path, m_username, m_pass, m_files, new DeleteFileProgressBar(this));
            }
            else
            {
                for (int count = 0; count < m_files.Count; count++)
                {
                    string filename = m_files[count];
                    try
                    {
                        if (File.Exists(filename))
                            File.Delete(filename);
                        backgroundWorker1.ReportProgress(0, new ProgressData(count, filename));
                    }
                    catch { }
                }
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (runswithservice)
            {
            }
            else
            {
                //progressdata is the same from RemoveMarkingsDialog
                ProgressData dat = (e.UserState as ProgressData);
                int progress = (int)((double)(dat.Count * 100) / (double)(m_files.Count));
                m_fileLabel.Text = dat.FileName;
                m_progressBar.Value = progress;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        internal BackgroundWorker BW 
        {
           get {return backgroundWorker1;}
        }
    }

    public class DeleteFileProgressBar : FileProcessingProgressBar
    {
        public DeleteFileProgressBar(DeleteDatFilesDialog parent)
        {
            m_parent = parent;
        }
        DeleteDatFilesDialog m_parent;

        public override bool UpdateProgress(string file, int count)
        {
            MethodInvoker m = delegate()
            {
                m_parent.BW.ReportProgress(0, new ProgressData(count, file));
            };
            m_parent.Invoke(m);
            return false;
        }
    }
}