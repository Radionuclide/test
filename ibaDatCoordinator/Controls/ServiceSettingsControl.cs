using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Win32;
using iba.Utility;
using iba.Processing;
using iba.Data;
using System.IO;
using System.Diagnostics;
using System.IO.Pipes;
using iba.CertificateStore;
using iba.CertificateStore.Forms;
using iba.CertificateStore.Proxy;
using iba.CertificateStore.Manager;
using System.Security.Cryptography.X509Certificates;

namespace iba.Controls
{
    public partial class ServiceSettingsControl : UserControl, IPropertyPane, ICertificatesControlHost, ICertifiable
    {
        public ServiceSettingsControl()
        {
            InitializeComponent();
            certificatesControl.SetControlHost(this);
            bool certListUpdated = LoadCertificatesIfDifferent();
            if (certListUpdated)
                certificatesControl.LoadDataSource();
            if (Program.RunsWithService != Program.ServiceEnum.NOSERVICE) //hide non relevant 
            {
                Control[] ToHide = new Control[] {label5, m_tbAnalyzerExe, m_executeIBAAButton,
                    m_btTransferAnalyzerSettings};

                foreach (var ctrl in ToHide)
                {
                    ctrl.Visible = false;
                }
                int Offset = 30;
                gb_IbaAnalyzer.Height -= Offset;
                CollapsibleGroupBox[] gboxesLower = new CollapsibleGroupBox[] { gb_Password, gb_Certificates, gb_GlobalCleanup };
                foreach (var box in gboxesLower)
                {
                    //box.Location = new Point(box.Location.X, box.Location.Y - Offset);
                    box.Top -= Offset;
                }
                m_ceManager = new CollapsibleElementManager(this);
                CollapsibleGroupBox[] gboxes = new CollapsibleGroupBox[] { gb_Processing, gb_IbaAnalyzer, gb_Password, gb_Certificates, gb_GlobalCleanup };
                foreach (var box in gboxes)
                {
                    box.Init();
                    m_ceManager.AddElement(box);
                }
            }
            else
            {
                //m_toolTip.SetToolTip(m_registerButton, iba.Properties.Resources.RegisterIbaAnalyzer);
                m_ceManager = new CollapsibleElementManager(this);
                CollapsibleGroupBox[] gboxes = new CollapsibleGroupBox[] { gb_Processing, gb_IbaAnalyzer, gb_Password, gb_Certificates, gb_GlobalCleanup };
                foreach (var box in gboxes)
                {
                    box.Init();
                    m_ceManager.AddElement(box);
                }
            }

        }

        private CollapsibleElementManager m_ceManager;
        IPropertyPaneManager m_manager;

        #region IPropertyPane Members

        public void LoadData(object datasource, IPropertyPaneManager manager)
        {
            m_manager = manager;
            bool certListUpdated = LoadCertificatesIfDifferent();
            if (certListUpdated)
                certificatesControl.LoadDataSource();

            m_cbRestartIbaAnalyzer.Checked = TaskManager.Manager.IsIbaAnalyzerCallsLimited;
            m_nudRestartIbaAnalyzer.Value = Math.Max(1,TaskManager.Manager.MaxIbaAnalyzerCalls);
            m_nudRestartIbaAnalyzer.Enabled = m_cbRestartIbaAnalyzer.Checked;
            m_nudMaxIbaAnalyzers.Maximum = RegistryOptimizer.OptimizationPossible ? 6 : 60;
            m_nudMaxIbaAnalyzers.Value = Math.Min(Math.Max(1, TaskManager.Manager.MaxSimultaneousIbaAnalyzers),m_nudMaxIbaAnalyzers.Maximum);
            m_cbPostpone.Checked = TaskManager.Manager.DoPostponeProcessing;
            m_nudPostponeTime.Value = TaskManager.Manager.PostponeMinutes;
            m_nudPostponeTime.Enabled = m_cbPostpone.Checked;
            m_nudResourceCritical.Value = TaskManager.Manager.MaxResourceIntensiveTasks;

            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                string output = PathUtil.FindAnalyzerPath();
                m_tbAnalyzerExe.Text = output;
                m_executeIBAAButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);
                //m_registerButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);
            }

            m_pass = "";
            try
            {
                m_pass = TaskManager.Manager.Password;
                m_cbRememberPassword.Checked = TaskManager.Manager.RememberPassEnabled;
                m_nudRememberTime.Value = (decimal)TaskManager.Manager.RememberPassTime.TotalMinutes;
                m_nudRememberTime.Enabled = m_cbRememberPassword.Checked;
            }
            catch
            {
            }
            UpdatePassControls();

            m_globalCleanupData = TaskManager.Manager.GlobalCleanupDataList;
            InitGlobalCleanup();
        }

        private void UpdatePassControls()
        {
            if (string.IsNullOrEmpty(m_pass))
            {
                m_SetChangePassBtn.Text = iba.Properties.Resources.SetStr;
                m_ClearPassBtn.Enabled = false;
                m_passwordStatusLabel.Text = iba.Properties.Resources.PassNotSet;
            }
            else
            {
                m_SetChangePassBtn.Text = iba.Properties.Resources.ChangeStr;
                m_ClearPassBtn.Enabled = true;
                m_passwordStatusLabel.Text = iba.Properties.Resources.PassSet;
            }
        }


        private List<GlobalCleanupData> m_globalCleanupData = new List<GlobalCleanupData>();
        private string m_pass;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            WindowsAPI.SHAutoComplete(m_tbAnalyzerExe.Handle, SHAutoCompleteFlags.SHACF_FILESYS_ONLY |
            SHAutoCompleteFlags.SHACF_AUTOSUGGEST_FORCE_ON | SHAutoCompleteFlags.SHACF_AUTOAPPEND_FORCE_ON);
        }

        public void LeaveCleanup()
        {
        }

        public void SaveData()
        {
            TaskManager.Manager.DoPostponeProcessing = m_cbPostpone.Checked;
            TaskManager.Manager.PostponeMinutes = (int)m_nudPostponeTime.Value;
            TaskManager.Manager.MaxIbaAnalyzerCalls = (int)m_nudRestartIbaAnalyzer.Value;
            TaskManager.Manager.IsIbaAnalyzerCallsLimited = m_cbRestartIbaAnalyzer.Checked;
            TaskManager.Manager.MaxSimultaneousIbaAnalyzers = (int)m_nudMaxIbaAnalyzers.Value;
            TaskManager.Manager.RememberPassEnabled = m_cbRememberPassword.Checked;
            TaskManager.Manager.RememberPassTime = TimeSpan.FromMinutes((double)m_nudRememberTime.Value);
            TaskManager.Manager.MaxResourceIntensiveTasks = (int)m_nudResourceCritical.Value;
            TaskManager.Manager.GlobalCleanupDataList = m_globalCleanupData;
        }

        #endregion

        private void m_browseIbaAnalyzerButton_Click(object sender, EventArgs e)
        {
            m_openFileDialog.Filter = "ibaAnalyzer Executable (*.exe)|*.exe";
            try
            {
                if (File.Exists(m_tbAnalyzerExe.Text))
                    m_openFileDialog.FileName = m_tbAnalyzerExe.Text;
            }
            catch
            {
            }
            DialogResult result = m_openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
                m_tbAnalyzerExe.Text = m_openFileDialog.FileName;
        }

        private void m_executeIBAAButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_tbAnalyzerExe.Text;
                    ibaProc.Start();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void m_registerButton_Click(object sender, EventArgs e)
        {
            try
            {
                using (Process ibaProc = new Process())
                {
                    ibaProc.EnableRaisingEvents = false;
                    ibaProc.StartInfo.FileName = m_tbAnalyzerExe.Text;
                    ibaProc.StartInfo.Arguments = "/regserver";
                    ibaProc.Start();
                    ibaProc.WaitForExit();
                    ibaProc.StartInfo.Arguments = "-noninteractive /regserver";
                    ibaProc.Start();
                    ibaProc.WaitForExit();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "ibaDatCoordinator", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //private void m_tbAnalyzerExe_TextChanged(object sender, EventArgs e)
        //{
        //    m_executeIBAAButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);
        //    m_registerButton.Enabled = File.Exists(m_tbAnalyzerExe.Text);
        //}

        private void m_SetChangePassBtn_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword())
                return;

            using (iba.Dialogs.SpecifyPasswordDialog dlg = new iba.Dialogs.SpecifyPasswordDialog())
            {
                dlg.Pass = m_pass;
                dlg.StartPosition = FormStartPosition.CenterParent;
                dlg.ShowDialog(this);
                if (!dlg.Cancelled)
                {
                    m_pass = dlg.Pass;
                    TaskManager.Manager.ChangePassword(m_pass, Program.ClientName);

                    UpdatePassControls();
                }
            }
        }

        private void m_ClearPassBtn_Click(object sender, EventArgs e)
        {
            if (!Utility.Crypt.CheckPassword())
                return;

            m_pass = "";
            TaskManager.Manager.ChangePassword(m_pass, Program.ClientName);

            UpdatePassControls();
        }

        private void m_cbPostpone_CheckedChanged(object sender, EventArgs e)
        {
            m_nudPostponeTime.Enabled = m_cbPostpone.Checked;
        }

        private void m_cbRestartIbaAnalyzer_CheckedChanged(object sender, EventArgs e)
        {
            m_nudRestartIbaAnalyzer.Enabled = m_cbRestartIbaAnalyzer.Checked;
        }

        private void m_cbRememberPassword_CheckedChanged(object sender, EventArgs e)
        {
            m_nudRememberTime.Enabled = m_cbRememberPassword.Checked;
        }

        private void InitGlobalCleanup()
        {

            var rowIdx = 0;
            tbl_GlobalCleanup.SuspendLayout();

            while (tbl_GlobalCleanup.RowCount > 2)
            {
                int row = tbl_GlobalCleanup.RowCount - 1;
                for (int i = 0; i < tbl_GlobalCleanup.ColumnCount; i++)
                {
                    Control c = tbl_GlobalCleanup.GetControlFromPosition(i, row);
                    if (c == null) continue;

                    tbl_GlobalCleanup.Controls.Remove(c);
                    c.Dispose();
                }

                tbl_GlobalCleanup.RowStyles.RemoveAt(row);
                tbl_GlobalCleanup.RowCount--;
            }

            tbl_GlobalCleanup.ResumeLayout(false);
            tbl_GlobalCleanup.PerformLayout();


            foreach (var gcd in m_globalCleanupData.OrderBy(gc => gc.DriveName))
            {
                rowIdx = tbl_GlobalCleanup.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                tbl_GlobalCleanup.RowCount++;


                if (gcd.IsSystemDrive)
                    AddSystemDriveToTableLayout(gcd, 0, rowIdx, ContentAlignment.MiddleCenter);
                else
                    AddDriveToTableLayout(gcd, 0, rowIdx, ContentAlignment.TopLeft);

                if (gcd.IsReady)
                {
                    AddSizeToPanelLayout(gcd.TotalSize, 1, rowIdx, ContentAlignment.TopRight);
                    AddSizeToPanelLayout(gcd.TotalFreeSpace, 2, rowIdx, ContentAlignment.TopRight);

                    AddQuotaToTableLayout(gcd, gcd.TotalSize, 3, rowIdx);
                    AddRescanTimeToTableLayout(gcd, 4, rowIdx);
                }
                AddIsActiveToTableLayout(gcd, 5, rowIdx);
            }

            gb_GlobalCleanup.Height = (rowIdx + 1) * 32;
        }

        private void AddDriveToTableLayout(GlobalCleanupData data, int colIdx, int rowIdx, ContentAlignment textAlignment)
        {
            string driveName = $"{data.DriveName} - [{data.VolumeLabel}]";
            if (!data.IsReady)
                driveName = data.DriveName;

            var pnl = AddTextToPanel(driveName, textAlignment);
            tbl_GlobalCleanup.Controls.Add(pnl, colIdx, rowIdx);
        }

        private void AddSizeToPanelLayout(long size, int colIdx, int rowIdx, ContentAlignment textAlignment)
        {
            var pnl = AddTextToPanel(PathUtil.GetSizeReadable(size), textAlignment);
            tbl_GlobalCleanup.Controls.Add(pnl, colIdx, rowIdx);
        }


        private Panel AddTextToPanel(string displayValue, ContentAlignment textAlign)
        {
            Panel pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.AutoSize = true;
            pnl.Margin = new Padding(0, 5, 0, 0);

            Label lbl = new Label();
            lbl.Dock = DockStyle.Fill;
            lbl.Text = displayValue;
            lbl.TextAlign = textAlign;

            pnl.Controls.Add(lbl);
            return pnl;
        }

        private void AddSystemDriveToTableLayout(GlobalCleanupData gcd, int colIdx, int rowIdx, ContentAlignment textAlign)
        {
            string cleanupFolder = String.IsNullOrEmpty(gcd.WorkingFolder) ? gcd.DriveName : gcd.WorkingFolder;
            string volumeLabel = String.Format(" - [{0}]", gcd.VolumeLabel);

            Panel pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.AutoSize = true;
            pnl.Margin = new Padding(0, 0, 0, 0);

            Label lbl = new Label();
            lbl.Padding = new Padding(0, 5, 0, 0);
            lbl.Text = CompactDisplayFolder(cleanupFolder, volumeLabel, lbl);
            lbl.TextAlign = textAlign;
            lbl.AutoSize = true;

            pnl.Controls.Add(lbl);

            Button btn = new Button();
            btn.Left = lbl.Left + lbl.Width + 5;
            btn.Size = new Size(24, 24);
            btn.Image = Icons.Gui.All.Images.FolderOpen();
            btn.Enabled = (Program.RunsWithService == Program.ServiceEnum.CONNECTED || Program.RunsWithService == Program.ServiceEnum.NOSERVICE);
            btn.Click += (s, e) =>
            {
                if (Program.RunsWithService == Program.ServiceEnum.CONNECTED && !Program.ServiceIsLocal)
                    BrowseFolderRemote(gcd);
                else
                    BrowseFolderLocal(gcd);

                if (!String.IsNullOrEmpty(gcd.WorkingFolder))
                {
                    lbl.Text = CompactDisplayFolder(gcd.WorkingFolder, volumeLabel, lbl);
                    btn.Left = lbl.Left + lbl.Width + 5;
                }
            };

            pnl.Controls.Add(btn);

            tbl_GlobalCleanup.Controls.Add(pnl, colIdx, rowIdx);
        }

        private void BrowseFolderRemote(GlobalCleanupData gcd)
        {
            using (ServerFolderBrowser fd = new ServerFolderBrowser())
            {
                fd.FixedDrivesOnly = true;
                fd.ShowFiles = false;

                fd.SelectedPath = String.IsNullOrEmpty(gcd.WorkingFolder) ? gcd.DriveName : gcd.WorkingFolder;

                DialogResult result = fd.ShowDialog(this);

                if (result == DialogResult.OK && fd.SelectedPath.StartsWith(gcd.DriveName))
                    gcd.WorkingFolder = fd.SelectedPath;
                else
                    gcd.WorkingFolder = String.Empty;
            }
            
        }

        private void BrowseFolderLocal(GlobalCleanupData gcd)
        {
            m_folderBrowserDialog.ShowNewFolderButton = false;
            m_folderBrowserDialog.SelectedPath = String.IsNullOrEmpty(gcd.WorkingFolder) ? gcd.DriveName : gcd.WorkingFolder;
            DialogResult result = m_folderBrowserDialog.ShowDialog(this);

            if (result == DialogResult.OK && m_folderBrowserDialog.SelectedPath.StartsWith(gcd.DriveName))
                gcd.WorkingFolder = m_folderBrowserDialog.SelectedPath;
            else
                gcd.WorkingFolder = String.Empty;

        }



        private string CompactDisplayFolder(string cleanupFolder, string volumeLabel, Label lbl)
        {
            const int maxWidth = 450;
            var volSize = System.Windows.Forms.TextRenderer.MeasureText(volumeLabel, lbl.Font);
            var newText = PathUtil.CompactPath(cleanupFolder, maxWidth - volSize.Width, lbl.Font, TextFormatFlags.PathEllipsis);
            
            if (newText != cleanupFolder)
                m_toolTip.SetToolTip(lbl, cleanupFolder);
            else
                m_toolTip.SetToolTip(lbl, String.Empty);
            
            return String.Concat(newText, volumeLabel);
        }

        private void AddQuotaToTableLayout(GlobalCleanupData data, long driveSize, int colIdx, int rowIdx)
        {
            Panel pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.AutoSize = true;

            PercentUpDown nud = new PercentUpDown();
            nud.Location = new Point(5, 0);
            nud.Margin = new Padding(0);
            nud.Size = new Size(54, 20);
            nud.Value = (decimal)data.PercentageFree;

            Label lbl = new Label();
            lbl.Margin = new Padding(0);
            lbl.Text = PathUtil.GetSizeReadable((long)(driveSize * (data.PercentageFree / 100.0)));
            lbl.Location = new Point(nud.Location.X + nud.Size.Width + 6, nud.Location.Y + 2);

            nud.ValueChanged += (s, e) =>
            {
                data.PercentageFree = (int)nud.Value;
                lbl.Text = PathUtil.GetSizeReadable((long)(driveSize * (data.PercentageFree / 100.0)));
            };

            pnl.Controls.Add(nud);
            pnl.Controls.Add(lbl);

            tbl_GlobalCleanup.Controls.Add(pnl, colIdx, rowIdx);

        }

        private void AddRescanTimeToTableLayout(GlobalCleanupData data, int colIdx, int rowIdx)
        {
            var lblHeader = tbl_GlobalCleanup.GetControlFromPosition(colIdx, 0);

            Panel pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.AutoSize = true;

            NumericUpDown nud = new NumericUpDown();
            nud.Location = new Point(0, 0);
            nud.Margin = new Padding(0);
            nud.Size = new Size(54, 20);
            nud.Maximum = 86400; // one day 86400 min
            nud.Minimum = 1;
            nud.Value = (decimal)data.RescanTime;

            nud.ValueChanged += (s, e) => { data.RescanTime = (int)nud.Value; };

            Label lbl = new Label();
            lbl.Margin = new Padding(0);
            lbl.Text = "min";

            CenterControls(lblHeader.Width, nud, lbl);

            pnl.Controls.Add(nud);
            pnl.Controls.Add(lbl);

            tbl_GlobalCleanup.Controls.Add(pnl, colIdx, rowIdx);

        }

        private static void CenterControls(int headerWidth, NumericUpDown nud, Label lbl)
        {
            var width = nud.Size.Width + 5 + lbl.PreferredWidth;
            var left = headerWidth / 2 - width / 2;

            nud.Left = left;
            lbl.Location = new Point(nud.Location.X + nud.Size.Width + 5, nud.Location.Y + 2);
        }

        private void AddIsActiveToTableLayout(GlobalCleanupData data, int colIdx, int rowIdx)
        {
            var lblHeader = tbl_GlobalCleanup.GetControlFromPosition(colIdx, 0);

            Panel pnl = new Panel();
            pnl.Dock = DockStyle.Fill;
            pnl.AutoSize = true;

            CheckBox cb = new CheckBox();
            cb.Location = new Point(lblHeader.Width / 2 - 15 / 2, 0);
            cb.Checked = data.Active;
            cb.Enabled = data.IsReady;

            ChangeEnabledStateforRowControls(data, rowIdx);

            cb.CheckedChanged += (s, e) =>
            {
                data.Active = cb.Checked;
                ChangeEnabledStateforRowControls(data, rowIdx);

                TaskManager.Manager.ReplaceGlobalCleanupData(data);
            };

            pnl.Controls.Add(cb);

            tbl_GlobalCleanup.Controls.Add(pnl, colIdx, rowIdx);
        }

        private void ChangeEnabledStateforRowControls(GlobalCleanupData data, int rowIdx)
        {
            var enabled = (!data.Active && data.IsReady);
            for (int i = 0; i < tbl_GlobalCleanup.ColumnCount - 1; i++)
            {
                Control ctl = tbl_GlobalCleanup.GetControlFromPosition(i, rowIdx);
                if (ctl == null) continue;
                
                ctl.Enabled = enabled;
            }
        }

#region ICertificatesControlHost implementation
        public bool IsLocalHost { get; }
        public string ServerAddress { get; }

        public void OnSaveDataSource()
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// The root node of the container that contains all certificate infos (that can be assigned with a certificates combo box)
        /// </summary>
        /// <returns></returns>
        public ICertifiable GetCertifiableRootNode()
        {
            return this;
        }

        public IEnumerable<ICertifiable> GetCertifiableChildItems()
        {
            var tasks = TaskManager.Manager.Configurations.SelectMany(c => c.Tasks).OfType<ICertifiable>().ToList();

            List<ICertifiable> certifiables = new()
            {
                TaskManager.Manager.OpcUaData,
                TaskManager.Manager.DataTransferData
            };

            certifiables.Where(item => item != null).ToList().ForEach(certifiable => tasks.Add(certifiable));

            return tasks;
        }

        public IEnumerable<ICertificateInfo> GetCertificateInfo()
        {
            yield break;
        }

        public void ManageCertificates()
        {
            // this happens when "Manage certificates" is selected in a certificates combobox
            // go to the certificates control
            //tabControl1.SelectedIndex = 0;
        }


        /// <summary>
        /// Executed on double click in the "Used by" column to go to the double-clicked part that is used by a certificate.
        /// </summary>
        /// <param name="displayName"></param>
        public void JumpToCertificateInfoNode(string displayName)
        {
            (m_manager as MainForm)?.MoveToTaskByName(displayName);
        }

        /// <summary>
        /// Implement the ICertificateManagerProxy interface on the client side to communicate with the certificate manager on the server side
        /// 
        /// CertificateManagerProxyJsonAdapter can be used to implement the JSON interface
        /// As an alternative, other than JSON serialization can be used by implementing 
        /// ICertificateManagerProxy instead of ICertificateManagerJsonProxy
        /// 
        /// Please DON'T use [Serializable] for serialization.
        /// </summary>
        public ICertificateManagerProxy CertificateManagerProxy { get; } = new CertificateManagerProxyJsonAdapter(new AppCertificateManagerJsonProxy());
        public bool IsCertificatesReadonly => false;
        public bool IsReadOnly => false; // set to true in case of user restriction
        public string UsagePart { get; } = "EX"; // IO and DS are used in PDA
        public IWin32Window Instance => this;
        public ContextMenuStrip PopupMenu { get; } = new ContextMenuStrip(); // or reuse the context menu of an other control
        public List<int> ServerUsageIds => new List<int>();

        #endregion ICertificatesControlHost implementation

        private void certificatesUpdateTimer_Tick(object sender, EventArgs e)
        {
            // This Timer is active ONLY if the Settings pane is visible, so it doesn't have any computational impact most of time.
            bool certListUpdated = LoadCertificatesIfDifferent();
            if (certListUpdated)
                certificatesControl.LoadDataSource();
        }

        public static string[] currentlCertsList { get; private set; }
        public static string[] currentlCertsListEncrypted { get; private set; }
        public static bool LoadCertificatesIfDifferent()
        {
            var certs = (TaskManager.Manager.HandleCertificate("GetCertificates") as string[]);
            var encr = certs.Select(a => Crypt.Decrypt(a)).ToArray<string>();
            if (currentlCertsList == null || !currentlCertsListEncrypted.SequenceEqual<string>(encr))
            {
                currentlCertsListEncrypted = encr;
                currentlCertsList = certs;
                return true;
            }
            return false;
        }

        public bool IsCertificateTableTimerTicking
        {
            set
            {
                certificatesUpdateTimer.Enabled = value;
            }
        }
    }

    /// <summary>
    /// On the client side, implement the ICertificateManagerJsonProxy interface.
    /// 
    /// This implementation shows what needs to be called on the server side when the client proxy is used.
    /// </summary>
    class AppCertificateManagerJsonProxy : ICertificateManagerJsonProxy
    {
        public string GetCertificateUsage(string thumbprint)
        {
            return TaskManager.Manager.HandleCertificate("GetCertificateUsage", thumbprint) as string;
        }

        public string PathToUnc(string fileName, bool convertLocal)
        {
            // conversion from local to unc path is done in different classes in different applications.
            return fileName;

            // in real apps: @"\\...";
            // convertLocal: if true, also convert local files to UNC paths
        }

        public bool UploadCertificateFile(string certPath)
        {
            //string localFile = Path.GetFileName(certPath);
            byte[] cert = File.ReadAllBytes(certPath);
            var arg = new Tuple<byte[], string>(cert, Path.GetFileName(certPath));
            TaskManager.Manager.HandleCertificate("Upload", arg);

            //string serverFile = Path.Combine(serverCertificateDirectory, "Temp", localFile);
            // upload to the server certificate directory
            //if (File.Exists(serverFile))
            //File.Delete(serverFile);
            //File.Copy(certPath, serverFile); // this should be an upload, not just a file copy
            // upload and download of files is done differently in different Iba applications
            return true;
        }

        public string TryAddCertificateToServerStore(string addCertificateArgs)
        {
            string res = TaskManager.Manager.HandleCertificate("AddCertificate", addCertificateArgs) as string;

            //var e = new CAddCertificateResult { Success = false }.ToJson(); // if no communication possible
            return res;
        }

        public string AddCertificate(string addCertificateArgs)
        {
            return TaskManager.Manager.HandleCertificate("AddCertificate", addCertificateArgs) as string;
        }

        public CertificateExportState? ExportCertificate(string exportCertificateArgs)
        {
            return TaskManager.Manager.HandleCertificate("ExportCertificate", exportCertificateArgs) as CertificateExportState?;
        }

        public void SetPeriodicUpdate()
        {
            // if available, set a regularly returning diagnostic signal
        }

        public bool RemoveCertificate(string thumbprint)
        {
            return (TaskManager.Manager.HandleCertificate("RemoveCertificate", thumbprint) as bool?) ?? false;
        }

        public string DownloadToClientTempFile(bool includePrivateKey)
        {
            // download a file Temp\Export.pfx or Temp\Export.cer from the certificates
            // directory on the server side.
            // Then open an save file dialog.
            // returns the location of the downloaded file on the client side.
            // return null when cancelled
            // return "" in case of an exception.


            var cert = TaskManager.Manager.HandleCertificate("DownloadToClientTempFile", includePrivateKey) as byte[];
            var tempFile = $@"{DataPath.CertificateFolder(Program.ApplicationState.SERVICE)}\Temp\Export.{(includePrivateKey ? "pfx" : "cer")}";

            File.WriteAllBytes(tempFile, cert);
            return tempFile;
        }

        /// <summary>
        /// whether the server can be reached from the client
        /// Should be a dynamic value
        /// </summary>
        public bool Available { get; } = true;
        public string ServerAddress { get; } = "localhost";

        public string GenerateCertificate(string cGenerateCertificateArgs)
        {
            return TaskManager.Manager.HandleCertificate("GenerateCertificate", cGenerateCertificateArgs) as string;
        }

        public bool EditCertificate(string cEditCertificateArgs)
        {
            return (TaskManager.Manager.HandleCertificate("EditCertificate", cEditCertificateArgs) as bool?) ?? false;
        }

        public DateTime LastChangeTime => (DateTime)TaskManager.Manager.HandleCertificate("LastChangeTime");

        /// <summary>
        /// Get all the certificates from the certificate manager on the certificate manager json endpoint
        /// 
        /// !! private key data is NOT included
        /// </summary>
        public string[] GetCertificates()
        {
            ServiceSettingsControl.LoadCertificatesIfDifferent();
            return ServiceSettingsControl.currentlCertsList;
        }
    }
}
