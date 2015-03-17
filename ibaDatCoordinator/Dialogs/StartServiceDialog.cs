using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace iba.Dialogs
{
    public partial class StartServiceDialog : Form
    {
        public StartServiceDialog()
        {
            InitializeComponent();
            m_result = false;
        }

        private bool m_result;
        public bool Result
        {
            get {return m_result;}
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
                if (!iba.Utility.DataPath.IsAdmin) //elevated process start the service
                {
                    if (System.Environment.OSVersion.Version.Major < 6)
                    {
                        MessageBox.Show(this, iba.Properties.Resources.UACText, iba.Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        myController.Close();
                        m_result = false;
                        return;
                    }

                    System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                    procInfo.UseShellExecute = true;
                    procInfo.ErrorDialog = true;

                    procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                    procInfo.FileName = Application.ExecutablePath;

                    procInfo.Arguments = "/startservice";
                    procInfo.Verb = "runas";

                    try
                    {
                        System.Diagnostics.Process.Start(procInfo);
                    }
                    catch
                    {
                        MessageBox.Show(this, iba.Properties.Resources.UACText, iba.Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                        myController.Close();
                        m_result = false;
                        return;
                    }

                }
                else
                {
                    myController.Start();
                }

                myController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Running,TimeSpan.FromMinutes(1.0));
                if (myController.Status != System.ServiceProcess.ServiceControllerStatus.Running)
                {
                    MessageBox.Show(String.Format(iba.Properties.Resources.ServiceConnectProblem, iba.Properties.Resources.ServiceConnectProblem2, Environment.NewLine), iba.Properties.Resources.ServiceConnectProblemCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);                            
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
                MessageBox.Show(String.Format(iba.Properties.Resources.ServiceConnectProblem, ex.Message, Environment.NewLine), iba.Properties.Resources.ServiceConnectProblemCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                m_result = false;
                return;
            }
            CommunicationObject com = (CommunicationObject)Activator.GetObject(typeof(CommunicationObject), Program.CommObjectString);
            Program.CommunicationObject = new CommunicationObjectWrapper(com);
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