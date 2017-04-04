using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using iba.Data;
using iba.Dialogs;
using iba.Utility;

namespace iba
{
    public partial class StatusForm : Form, IExternalCommand
    {
        public StatusForm()
        {
            InitializeComponent();
            m_quitForm = new QuitForm(this);
            m_quitForm.CreateHandle(new CreateParams());

            WindowState = FormWindowState.Minimized;
            m_miRestoreCoordinator = new ToolStripMenuItem(iba.Properties.Resources.notifyIconMenuItemRestore, null, miRestore_Click);
            m_miRestoreCoordinator.Font = new Font(m_miRestoreCoordinator.Font, FontStyle.Bold);
            m_miStartService = new ToolStripMenuItem(iba.Properties.Resources.notifyIconMenuItemStartService, null, miStartService_Click);
            m_miStopService = new ToolStripMenuItem(iba.Properties.Resources.notifyIconMenuItemStopService, null, miStopService_Click);
            if (!Utility.DataPath.IsAdmin)
            {
                m_miStartService.Image = iba.Properties.Resources.shield;
                m_miStopService.Image = iba.Properties.Resources.shield;
            }


            m_miExit = new ToolStripMenuItem(iba.Properties.Resources.notifyIconMenuItemExit, null, miExit_Click);
            ToolStripItem seperator = new ToolStripSeparator();
            ToolStripItem seperator2 = new ToolStripSeparator();
            m_iconMenu = new ContextMenuStrip();
            m_iconMenu.Items.AddRange(new ToolStripItem[]
                {
                    m_miRestoreCoordinator,
                    seperator,
                    m_miStartService,
                    m_miStopService,
                    seperator2,
                    m_miExit
                });
            m_iconMenu.RightToLeft = System.Windows.Forms.RightToLeft.No;
            m_iconMenu.Opening += new CancelEventHandler(m_iconMenu_Opening);

            m_iconEx = new NotifyIcon();
            m_iconEx.ContextMenuStrip = m_iconMenu;
            m_iconEx.DoubleClick += new EventHandler(iconEx_DoubleClick);
            m_iconEx.Visible = false;

            ((Bitmap)m_executeIBAAButton.Image).MakeTransparent(Color.Magenta);
            m_toolTip.SetToolTip(m_registerButton, iba.Properties.Resources.RegisterIbaAnalyzer);

            Text = typeof(Program).Assembly.GetName().Name + " " + iba.Properties.Resources.StatusProgram +  " " + DatCoVersion.GetVersion();
        }

        private bool m_actualClose = false;
        protected override void OnClosing(CancelEventArgs e)
        {
            if (WindowState != FormWindowState.Minimized)
                FormStateSerializer.SaveSettings(this, "StatusForm");

            if (!m_actualClose)
            {
                if (WindowState != FormWindowState.Minimized)
                {
                    WindowState = FormWindowState.Minimized;
                    ShowInTaskbar = false;
                    e.Cancel = true;
                }
                return;
            }

            LogData.Data.Logger.Close();
            base.OnClosing(e);
        }

        private QuitForm m_quitForm;
        private NotifyIcon m_iconEx;
        public NotifyIcon NotifyIcon
        {
            get { return m_iconEx; }
        }

        private ContextMenuStrip m_iconMenu;
        private ToolStripMenuItem m_miRestoreCoordinator;
        private ToolStripMenuItem m_miStartService;
        private ToolStripMenuItem m_miStopService;
        private ToolStripMenuItem m_miExit;

        public void OnExternalActivate()
        {
            Show();
            Activate();
            WindowState = FormWindowState.Normal;
            FormStateSerializer.LoadSettings(this, "StatusForm", true);
            ShowInTaskbar = true;
        }

        #region menuOptions
        void m_iconMenu_Opening(object sender, CancelEventArgs e)
        {
            ServiceController service = new ServiceController("IbaDatCoordinatorService");
            try
            {
                if (service.Status == ServiceControllerStatus.Stopped)
                {
                    m_miStartService.Enabled = true;
                    m_miStopService.Enabled = false;
                }
                else
                {
                    m_miStartService.Enabled = false;
                    m_miStopService.Enabled = true;
                }
            }
            catch (Exception)
            {
                m_miStartService.Enabled = false;
                m_miStopService.Enabled = false;
            }
            service.Close();
        }

        private void miRestore_Click(object sender, System.EventArgs e)
        {
            OnExternalActivate();
        }

        private void miExit_Click(object sender, System.EventArgs e)
        {
            OnExternalClose();
        }

        private void miStartService_Click(object sender, System.EventArgs e)
        {
            //if (!Utility.Crypt.CheckPassword(this)) return;
            OnStartService();
        }

        private void miStopService_Click(object sender, System.EventArgs e)
        {
            //if (!Utility.Crypt.CheckPassword(this)) return;
            OnStopService();
        }

        private void iconEx_DoubleClick(object sender, System.EventArgs e)
        {
            miRestore_Click(null, null);
        }

        #endregion

        public void OnExternalClose()
        {
            m_actualClose = true;
            if (m_iconEx != null)
                m_iconEx.Dispose();
            Close();
        }

        #region Service related methods

        public void OnStopService()
        {
            //stopService dialog
            //Program.CommunicationObject.StoppingService = true;
            bool result = false;
            using (StopServiceDialog ssd = new StopServiceDialog())
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ssd.StartPosition = FormStartPosition.CenterScreen;
                    ssd.ShowDialog();
                    result = ssd.Result;
                }
                else
                {
                    ssd.StartPosition = FormStartPosition.CenterParent;
                    ssd.ShowDialog(this);
                    result = ssd.Result;
                }
            }
            if (result)
            {
                //Program.CommunicationObject.HandleBrokenConnection();
                LogData.Data.Logger.Log(Logging.Level.Info, iba.Properties.Resources.logServiceStopped);

            }
            //Program.CommunicationObject.StoppingService = false;
        }

        public void OnStartService()
        {
            //startservice dialog
            using (StartServiceDialog ssd = new StartServiceDialog())
            {
                if (WindowState == FormWindowState.Minimized)
                {
                    ssd.StartPosition = FormStartPosition.CenterScreen;
                    ssd.ShowDialog();
                }
                else
                {
                    ssd.StartPosition = FormStartPosition.CenterParent;
                    ssd.ShowDialog(this);
                }
            }
        }

        #endregion

        protected override void WndProc(ref System.Windows.Forms.Message m)
        {
            const int WM_QUERYENDSESSION = 0x011;
            if (m.Msg == WM_QUERYENDSESSION)
            {
                m_actualClose = true;
            }
            base.WndProc(ref m);

        } //WndProc 

        private void m_btnStart_Click(object sender, EventArgs e)
        {
            OnStartService();
        }

        private void m_btnStop_Click(object sender, EventArgs e)
        {
            OnStopService();
        }

        private void m_btTransferAnalyzerSettings_Click(object sender, EventArgs e)
        {
            if (!iba.Utility.DataPath.IsAdmin) //elevated process start the service
            {
                if (System.Environment.OSVersion.Version.Major < 6)
                {
                    MessageBox.Show(this, iba.Properties.Resources.UACText, iba.Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                System.Diagnostics.ProcessStartInfo procInfo = new System.Diagnostics.ProcessStartInfo();
                procInfo.UseShellExecute = true;
                procInfo.ErrorDialog = true;

                procInfo.WorkingDirectory = System.IO.Path.GetDirectoryName(Application.ExecutablePath);
                procInfo.FileName = Application.ExecutablePath;

                procInfo.Arguments = "/transfersettings";
                procInfo.Verb = "runas";

                try
                {
                    System.Diagnostics.Process.Start(procInfo);
                }
                catch
                {
                    MessageBox.Show(this, iba.Properties.Resources.UACText, iba.Properties.Resources.UACCaption, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
                OnTransferSettings();

        }


        private void OnTransferSettings()
        {
            try
            {
                System.ServiceProcess.ServiceController myController =
                    new System.ServiceProcess.ServiceController("IbaDatCoordinatorService");
                //copy mcr and other files
                string IbaAnalyzerPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                IbaAnalyzerPath = Path.Combine(IbaAnalyzerPath, "iba", "ibaAnalyzer");
                string tempDir = System.IO.Path.GetTempPath();
                string outFile = Path.Combine(tempDir, "ibaAnalyzer.reg");
                Utility.RegistryExporter.ExportIbaAnalyzerKey(outFile);

                using (NamedPipeServerStream pipeServer =
                    new NamedPipeServerStream("DatcoServiceStatusPipe", PipeDirection.Out))
                {
                    //instruct service to connnect
                    myController.ExecuteCommand(128);//instruct service to connect to the named pipe
                    pipeServer.WaitForConnection();
                    using (StreamWriter sw = new StreamWriter(pipeServer))
                    {
                        sw.AutoFlush = true;
                        sw.WriteLine(IbaAnalyzerPath);
                        sw.WriteLine(outFile);
                    }
                    myController.ExecuteCommand(129);//instruct service to copy files and register the outfile
                }
                myController.Close();
                File.Delete(outFile);
                MessageBox.Show(this, Properties.Resources.TransferIbaAnalyzerSettingsSuccess, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (System.Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
