using System;
using System.Windows.Forms;
using iba.Processing;

namespace iba.Controls
{
    public partial class SnmpLeftPanelControl : UserControl 
    {
        //private readonly MainForm _mainForm;
        public SnmpLeftPanelControl(MainForm mainForm)
        {
            //_mainForm = mainForm;
            InitializeComponent();
        }

        private void SnmpLeftPanelControl_Load(object sender, EventArgs e)
        {
            SnmpWorker snmpWorker = TaskManager.Manager?.SnmpWorker;
            if (snmpWorker == null)
            {
                return;
            }
            //subscribe to status change
            snmpWorker.StatusChanged += SnmpWorker_StatusChanged;
            //set initial status
            snmpWorker.ApplyStatusToTextBox(tbStatus);
        }

        private void SnmpWorker_StatusChanged(object sender, StatusChangedEventArgs e)
        {
            tbStatus.BackColor = e.Color;
            tbStatus.Text = e.Message;
            cbEnabled.Checked = e.Status != SnmpWorkerStatus.Stopped;
        }

        private void buttonShowDebug_Click(object sender, EventArgs e)
        {
        }

        private void timerStatus_Tick(object sender, EventArgs e)
        {
        }
    }
}
