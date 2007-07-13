using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace iba
{
    public partial class StopServiceDialog : Form
    {
        public StopServiceDialog()
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
                myController.Stop();
                myController.WaitForStatus(System.ServiceProcess.ServiceControllerStatus.Stopped, TimeSpan.FromHours(1.0));
                if (myController.Status != System.ServiceProcess.ServiceControllerStatus.Stopped)
                {
                    MessageBox.Show(String.Format(iba.Properties.Resources.ServiceConnectProblem3, iba.Properties.Resources.ServiceConnectProblem4, Environment.NewLine), iba.Properties.Resources.ServiceConnectProblemCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                MessageBox.Show(String.Format(iba.Properties.Resources.ServiceConnectProblem3, ex.Message, Environment.NewLine), iba.Properties.Resources.ServiceConnectProblemCaption, MessageBoxButtons.OK, MessageBoxIcon.Error); 
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