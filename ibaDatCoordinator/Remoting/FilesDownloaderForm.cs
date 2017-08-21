using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;

using System.IO;

namespace iba.Remoting
{
	/// <summary>
	/// Summary description for AddressbookDownloader.
	/// </summary>
	public class FilesDownloaderForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progress;
        private System.Windows.Forms.Label lbFileName;
        private System.Windows.Forms.Button btCancel;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        protected iba.Utility.Components.ibaBackgroundWorker backWorker;
        protected ServerFileInfo[] remoteFiles;
        protected string remoteBasePath;
        protected string localCachePath;
        protected IPdaServerFiles pdaFiles;
        protected bool bFullDestinationPath;

        public FilesDownloaderForm(ServerFileInfo[] remoteFiles, string localCachePath, IPdaServerFiles pdaFiles)
            : this(remoteFiles, null, localCachePath, pdaFiles, false)
        {
        }

        public FilesDownloaderForm(ServerFileInfo[] remoteFiles, string localCachePath, IPdaServerFiles pdaFiles, bool bFullDestinationPath)
            : this(remoteFiles, null, localCachePath, pdaFiles, bFullDestinationPath)
        {
        }

        public FilesDownloaderForm(ServerFileInfo[] remoteFiles, string remoteBasePath, string localCachePath, IPdaServerFiles pdaFiles)
            : this(remoteFiles, remoteBasePath, localCachePath, pdaFiles, false)
        {
        }

        public FilesDownloaderForm(ServerFileInfo[] remoteFiles, string remoteBasePath, string localCachePath, IPdaServerFiles pdaFiles, bool bFullDestinationPath)
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

            this.remoteFiles = remoteFiles;
            this.remoteBasePath = remoteBasePath;
            this.localCachePath = localCachePath;
            this.pdaFiles = pdaFiles;
            this.bFullDestinationPath = bFullDestinationPath;

            lbFileName.Text = "";
            progress.Value = 0;

            backWorker = new iba.Utility.Components.ibaBackgroundWorker();
            backWorker.WorkerReportsProgress = true;
            backWorker.WorkerSupportsCancellation = true;
            backWorker.DoWork += new iba.Utility.Components.ibaDoWorkEventHandler(OnDoWork);
            backWorker.RunWorkerCompleted += new iba.Utility.Components.ibaRunWorkerCompletedEventHandler(OnWorkCompleted);
            backWorker.ProgressChanged += new iba.Utility.Components.ibaProgressChangedEventHandler(OnProgressChanged);

            FormBorderStyle = FormBorderStyle.FixedDialog;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				if(!IsDisposed)
					backWorker.Dispose();
			}

			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilesDownloaderForm));
            this.label1 = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.lbFileName = new System.Windows.Forms.Label();
            this.btCancel = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // progress
            // 
            resources.ApplyResources(this.progress, "progress");
            this.progress.Name = "progress";
            // 
            // lbFileName
            // 
            resources.ApplyResources(this.lbFileName, "lbFileName");
            this.lbFileName.Name = "lbFileName";
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Name = "btCancel";
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // FilesDownloader
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ControlBox = false;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.lbFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progress);
            this.Name = "FilesDownloader";
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad (e);

            Cursor = Cursors.WaitCursor;

            backWorker.RunWorkerAsync();
        }


        private void OnDoWork(object sender, iba.Utility.Components.ibaDoWorkEventArgs e)
        {
            if((pdaFiles == null) || (remoteFiles == null) || (remoteFiles.Length == 0))
                return;
            if(!Directory.Exists(Path.GetDirectoryName(localCachePath)))
                Directory.CreateDirectory(Path.GetDirectoryName(localCachePath));
            long totalSize = 0;
            for(int i = 0; i < remoteFiles.Length; i++)
                totalSize += remoteFiles[i].FileSize;

            long receivedSize = 0;
            for(int i = 0; i < remoteFiles.Length && !backWorker.CancellationPending; i++)
            {
                backWorker.ReportProgress((int)((receivedSize * 100.0) / totalSize), remoteFiles[i].LocalFileName);

                //Download file
                long fileSize;
                string localFileName;
                if(bFullDestinationPath)
                    localFileName = localCachePath;
                else
                {
                    localFileName = Path.Combine(localCachePath, Path.GetFileName(remoteFiles[i].LocalFileName));
                    if(remoteBasePath != null)
                        localFileName = Path.Combine(localCachePath, remoteFiles[i].LocalFileName.Replace(remoteBasePath, ""));
                }

                //Check that destination directory exists
                string localDir = Path.GetDirectoryName(localFileName);
                if(!Directory.Exists(localDir))
                    Directory.CreateDirectory(localDir);

                FileStream localFile = File.Create(localFileName);
                try
                {
                    int currentPart = 0;
                    IFileDownload downloader = pdaFiles.DownloadFile(remoteFiles[i].LocalFileName, out fileSize);
                    while(fileSize > 0)
                    {
                        byte[] data = downloader.GetNextPortion();
                        if((data == null) || (data.Length == 0))
                            break;

                        fileSize -= data.Length;
                        receivedSize += data.Length;
                        localFile.Write(data, 0, data.Length);

                        currentPart += data.Length;
                        if(currentPart > 200000)
                        {
                            backWorker.ReportProgress((int)((receivedSize * 100.0) / totalSize), remoteFiles[i].LocalFileName);
                            currentPart = 0;
                        }

                        if(backWorker.CancellationPending)
                        {
                            downloader.Cancel();
                            break;
                        }
                    }
                }
                finally
                {
                    localFile.Flush();
                    localFile.Close();
                }

                File.SetLastWriteTimeUtc(localFileName, remoteFiles[i].LastWriteTimeUtc);
            }

            backWorker.ReportProgress((int)((receivedSize * 100.0) / totalSize));
        }

        private void OnWorkCompleted(object sender, iba.Utility.Components.ibaRunWorkerCompletedEventArgs e)
        {
            Cursor = Cursors.Default;

            if(e.Error != null)
            {
                string msg = String.Format(iba.Properties.Resources.ErrDownloadingFiles, e.Error.Message);
                MessageBox.Show(this, msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
            }
            else if(backWorker.CancellationPending)
                DialogResult = DialogResult.Cancel;
            else
                DialogResult = DialogResult.OK;
        }

        private void OnProgressChanged(object sender, iba.Utility.Components.ibaProgressChangedEventArgs e)
        {
            progress.Value = e.ProgressPercentage;
            lbFileName.Text = (e.Tag != null) ? e.Tag.ToString() : "";
        }

        private void btCancel_Click(object sender, System.EventArgs e)
        {
            backWorker.CancelAsync();
        }
    }
}
