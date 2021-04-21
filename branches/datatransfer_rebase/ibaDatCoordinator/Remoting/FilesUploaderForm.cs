using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using System.Threading;

namespace iba.Remoting
{
	/// <summary>
	/// Summary description for AddressbookUploader.
	/// </summary>
	public class FilesUploaderForm : System.Windows.Forms.Form
	{
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Label lbFileName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progress;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

        protected iba.Utility.Components.ibaBackgroundWorker backWorker;
        protected string[] files;
        protected string[] destPaths;
        protected IPdaServerFiles pdaFiles;
        protected float progressOffset;
        protected float progressScale;
        protected string progressFile;
        protected Int64 progressFileLength;

        public FilesUploaderForm(string[] files, string[] destPaths, IPdaServerFiles pdaFiles)
        {
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();

            this.files = files;
            this.destPaths = destPaths;
            this.pdaFiles = pdaFiles;

            lbFileName.Text = "";
            progress.Value = 0;
            progressOffset = 0;
            progressScale = 1;

            backWorker = new iba.Utility.Components.ibaBackgroundWorker();
            backWorker.WorkerReportsProgress = true;
            backWorker.WorkerSupportsCancellation = true;
            backWorker.DoWork += new iba.Utility.Components.ibaDoWorkEventHandler(OnDoWork);
            backWorker.RunWorkerCompleted += new iba.Utility.Components.ibaRunWorkerCompletedEventHandler(OnWorkCompleted);
            backWorker.ProgressChanged += new iba.Utility.Components.ibaProgressChangedEventHandler(OnProgressChanged);

            FormBorderStyle = FormBorderStyle.FixedDialog;
        }

        public FilesUploaderForm(string[] files, string destPath, IPdaServerFiles pdaFiles)
            : this(files, new string[] { destPath }, pdaFiles)
        {
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilesUploaderForm));
            this.btCancel = new System.Windows.Forms.Button();
            this.lbFileName = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.progress = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Name = "btCancel";
            // 
            // lbFileName
            // 
            resources.ApplyResources(this.lbFileName, "lbFileName");
            this.lbFileName.Name = "lbFileName";
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
            // FilesUploader
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.ControlBox = false;
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.lbFileName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.progress);
            this.Name = "FilesUploader";
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
            if((pdaFiles == null) || (files == null) || (files.Length == 0))
                return;

            FileInfo[] fInfo = new FileInfo[files.Length];
            long totalSize = 0;
            for(int i=0; i<files.Length; i++)
            {
                fInfo[i] = new FileInfo(files[i]);
                totalSize += fInfo[i].Length;
            }

            ManualResetEvent completed = new ManualResetEvent(false);
            long sentSize = 0;
            progressOffset = 0;
            for(int i=0; i<files.Length && !backWorker.CancellationPending; i++)
            {
				if(fInfo[i].Length == 0)
					continue;

                progressFileLength = fInfo[i].Length;
                progressScale = (progressFileLength*100.0F) / totalSize;
                progressFile = files[i];
                
                //Upload file
                completed.Reset();
				using(FileStream file = File.Open(files[i], FileMode.Open, FileAccess.Read, FileShare.Read))
				{
					FileUploader upload = new FileUploader(file, fInfo[i].LastWriteTimeUtc, completed, this);
                    string[] destFileNames = new string[destPaths.Length];
                    for(int j=0;j<destPaths.Length;j++)
                        destFileNames[j] = Path.Combine(destPaths[j], Path.GetFileName(files[i]));

                    pdaFiles.UploadFile(destFileNames, upload, fInfo[i].Length);

                    //Wait until upload is complete
					completed.WaitOne(TimeSpan.FromMinutes(3), false);

                    //Check if upload was successful
                    if (!String.IsNullOrEmpty(upload.ErrorMessage))
                        throw new Exception(upload.ErrorMessage);

					sentSize += fInfo[i].Length;
				}

                progressOffset = (sentSize * 100.0F) / totalSize;
                backWorker.ReportProgress((int) progressOffset, files[i]);
            }

            //Give server some time to close all its references to the files!!
            Thread.Sleep(1000);

            backWorker.ReportProgress(100);
        }

        private void OnWorkCompleted(object sender, iba.Utility.Components.ibaRunWorkerCompletedEventArgs e)
        {
            Cursor = Cursors.Default;

            if (e.Error != null)
            {
                string msg = String.Format(iba.Properties.Resources.ErrUploadingFiles, e.Error.Message);
                MessageBox.Show(this, msg, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                DialogResult = DialogResult.Abort;
            }
            else if(backWorker.CancellationPending)
                DialogResult = DialogResult.Cancel;
            else
                DialogResult = DialogResult.OK;
        }

        public void ReportProgress(Int64 filePos)
        {
            double percFile = (filePos * 100.0 / progressFileLength);
            backWorker.ReportProgress((int)(progressOffset + progressScale * percFile/100));
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

    public interface IFileDownload
    {
        void Cancel();
        byte[] GetNextPortion();
        //void ErrorOccurred(string errorMsg);
    }

    public interface IFileDownload2 : IFileDownload
    {
        void ErrorOccurred(string errorMsg);
    }

    public interface IFileDownload3 : IFileDownload2
    {
        DateTime LastWriteTimeUTC { get; }
    }

    internal class FileUploader : MarshalByRefObject, IFileDownload3
    {
        public FileUploader(FileStream file, DateTime lastWriteTimeUTC, ManualResetEvent completedEvent, FilesUploaderForm form)
        {
            buffer = new byte[64*1024];
            this.file = file;
            this.form = form;
            this.completedEvent = completedEvent;
            errorMsg = null;
            LastWriteTimeUTC = lastWriteTimeUTC;
        }

        protected FilesUploaderForm form;
        protected FileStream file;
        protected ManualResetEvent completedEvent;
        protected byte[] buffer;
        protected string errorMsg;

        #region IFileDownload Members

        public void Cancel()
        {
            file = null;
            form = null;
            completedEvent.Set();
        }

        public byte[] GetNextPortion()
        {
            if(file == null)
                return null;

            int nrBytesRead = file.Read(buffer, 0, buffer.Length);
            if(form != null)
                form.ReportProgress(file.Position);
            if(nrBytesRead < buffer.Length)
            {
                byte[] smallBuffer = new byte[nrBytesRead];
                if(nrBytesRead > 0)
                    Buffer.BlockCopy(buffer, 0, smallBuffer, 0, nrBytesRead);
                Cancel();
                return smallBuffer;
            }
            else
            {
                if(file.Position == file.Length)
                    Cancel();
                return buffer;
            }
        }

        #endregion

        #region IFileDownload2 Members

        public void ErrorOccurred(string errorMsg)
        {
            this.errorMsg = errorMsg;
            Cancel();
        }

        #endregion

        #region IFileDownload3 Members

        public DateTime LastWriteTimeUTC { get; private set; }

        #endregion

        public string ErrorMessage
        {
            get { return errorMsg; }
        }
    }

}
