using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace iba.DatCoordinator.Status.Dialogs
{
    public partial class RestartServiceDialog : Form
    {
        public RestartServiceDialog()
        {
            InitializeComponent();
            m_result = false;
        }

        private bool m_result;
        public bool Result
        {
            get { return m_result; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            backgroundWorker1.RunWorkerAsync();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                System.ServiceProcess.ServiceController myController =
                new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                if (!Program.IsAdmin) //elevated process start the service
                {
                    if (System.Environment.OSVersion.Version.Major < 6)
                    {
                        MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        myController.Close();
                        m_result = false;
                        return;
                    }
                    System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                    procInfo.UseShellExecute = true;
                    procInfo.ErrorDialog = true;

                    procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                    procInfo.FileName = Application.ExecutablePath;

                    procInfo.Arguments = "/restartservice";
                    procInfo.Verb = "runas";

                    try
                    {
                        System.Diagnostics.Process.Start(procInfo);
                    }
                    catch
                    {
                        MessageBox.Show(this, Properties.Resources.UACText, Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        myController.Close();
                        m_result = false;
                        return;
                    }
                }
                else
                {
                    myController.Stop();
                    myController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped, TimeSpan.FromMinutes(3.0));
                    myController.Start();
                }

                myController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running, TimeSpan.FromMinutes(3.0));
                if (myController.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    MessageBox.Show(String.Format(Properties.Resources.ServiceConnectProblem3, Properties.Resources.ServiceConnectProblem4, Environment.NewLine), Properties.Resources.ServiceConnectProblemCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    m_result = false;
                    return;
                }
                else
                {
                    m_result = true;
                }
                myController.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(String.Format(Properties.Resources.ServiceConnectProblem3, ex.Message, Environment.NewLine), Properties.Resources.ServiceConnectProblemCaption, MessageBoxButtons.OK, MessageBoxIcon.Error); 
                m_result = false;
                return;
            }
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.Close();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            m_progressBar.PerformStep();
            if (m_progressBar.Value >= m_progressBar.Maximum) m_progressBar.Value = m_progressBar.Minimum;
        }
    }
}