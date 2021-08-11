using System;
using System.Drawing;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

using System.ServiceProcess;

using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;

using iba.Utility;
using iba.Logging;
using iba.Processing.IbaGrpc;


//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views;

namespace iba.Dialogs
{
	/// <summary>
	/// Summary description for ServerSelectionForm.
	/// </summary>
	internal class ServerSelectionForm : System.Windows.Forms.Form, IServiceDiscovered
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;
		private System.Windows.Forms.Button btCancel;
		private System.Windows.Forms.Button btApply;
		private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox ckAutoConnect;
        private System.Windows.Forms.Button btSearch;
        private System.Windows.Forms.Panel panel1;
        private iba.Controls.GradientWaitingBar waitBar;
        //private iba.Utility.Controls.BouncingProgressBar waitBar;
        private System.Windows.Forms.Timer stopTimer;
		//public CServerList list;

        private iba.Controls.ImageComboBox cbAddress;
        private DataGridView grid;
        private HelpProvider helpProvider;
        private NumericUpDown spPortNr;
        private iba.Controls.ImageComboBoxItem noneItem;
        private DataGridViewTextBoxColumn colName;
        private DataGridViewTextBoxColumn colIpAddress;
        private DataGridViewTextBoxColumn colPortNr;
        private DataGridViewTextBoxColumn colVersion;
        private DataGridViewTextBoxColumn colDataTrnasfer;
        private ServerConfiguration serverConfig;

        public ServerSelectionForm(ServerConfiguration _serverConfig)
		{
            serverConfig = _serverConfig;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            this.Icon = iba.Properties.Resources.ibaDatCoordinator_server;
            LoadMRUList();

            ckAutoConnect.Visible = false;
            //ckAutoConnect.Checked = clientConfig.AutoReconnect;

            ImageList list = new ImageList();
            list.ImageSize = new System.Drawing.Size(16, 16);
            list.TransparentColor = Color.Magenta;
            list.ColorDepth = ColorDepth.Depth24Bit;
            list.Images.AddStrip(iba.Properties.Resources.Toolbars16);
            cbAddress.ImageList = list;
            cbAddress.DefaultImageIndex = 21;

            if(serverConfig.Enabled)
            {
                cbAddress.Text = serverConfig.Address;
                spPortNr.SetIntValue(serverConfig.PortNr);
            }
            else
            {
                cbAddress.Text = noneItem.Text;
                spPortNr.SetIntValue(serverConfig.PortNr);
            }

			AcceptButton = btApply;
			CancelButton = btCancel;

            btSearch.Text = iba.Properties.Resources.Search;
            
            stopTimer = new System.Windows.Forms.Timer();
            stopTimer.Interval = 5000;
            stopTimer.Tick += new EventHandler(OnStopTimerTick);
		}

        private void LoadMRUList()
        {
            cbAddress.Items.Clear();

            //Add none item
            noneItem = new iba.Controls.ImageComboBoxItem(iba.Properties.Resources.NotConnected, 15);
            cbAddress.Items.Add(noneItem);

            String mruList = "";
            Profiler.ProfileString(true,"Client", "ServersMRUList", ref mruList, "");
            string[] list = mruList.Split(':');
            foreach(string s in list)
            {
                if(s.Length == 0)
                    continue;

                cbAddress.Items.Add(new iba.Controls.ImageComboBoxItem(s.ToUpper(), 14));
            }

            cbAddress.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cbAddress.AutoCompleteSource = AutoCompleteSource.ListItems;
            cbAddress.DisplayMember = "Text";
            cbAddress.FormattingEnabled = true;
        }

        private void SaveMRUList()
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            string currentAddress = cbAddress.Text;
            bool bIsNone = currentAddress == noneItem.Text;
            currentAddress = currentAddress.ToUpper();
            if((currentAddress.Length > 0) && !bIsNone) 
                sb.Append(currentAddress);

            for (int i=1; i<cbAddress.Items.Count && i<10; i++)
            {
                iba.Controls.ImageComboBoxItem item = cbAddress.Items[i] as iba.Controls.ImageComboBoxItem;
                string s = item.Text;
                if (s == currentAddress)
                    continue;

                if (sb.Length > 0)
                    sb.Append(":" + s);
                else
                    sb.Append(s);
            }
            string res = sb.ToString();
            Profiler.ProfileString(false,"Client", "ServersMRUList", ref res ,"");
        }

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        protected override void Dispose(bool disposing)
		{
			if( disposing )
			{
				if(components != null)
				{
					components.Dispose();
				}

				if(helpProvider != null)
					helpProvider.Dispose();
				helpProvider = null;
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerSelectionForm));
            this.btCancel = new System.Windows.Forms.Button();
            this.btApply = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.btSearch = new System.Windows.Forms.Button();
            this.ckAutoConnect = new System.Windows.Forms.CheckBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.grid = new System.Windows.Forms.DataGridView();
            this.colName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIpAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPortNr = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVersion = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataTrnasfer = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.spPortNr = new System.Windows.Forms.NumericUpDown();
            this.cbAddress = new iba.Controls.ImageComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPortNr)).BeginInit();
            this.SuspendLayout();
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Name = "btCancel";
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btApply
            // 
            resources.ApplyResources(this.btApply, "btApply");
            this.btApply.Name = "btApply";
            this.btApply.Click += new System.EventHandler(this.btApply_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // btSearch
            // 
            resources.ApplyResources(this.btSearch, "btSearch");
            this.btSearch.Name = "btSearch";
            this.btSearch.Click += new System.EventHandler(this.OnSearchServers);
            // 
            // ckAutoConnect
            // 
            resources.ApplyResources(this.ckAutoConnect, "ckAutoConnect");
            this.ckAutoConnect.Name = "ckAutoConnect";
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // grid
            // 
            this.grid.AllowUserToAddRows = false;
            this.grid.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.grid, "grid");
            this.grid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.grid.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllCells;
            this.grid.BackgroundColor = System.Drawing.SystemColors.Window;
            this.grid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colName,
            this.colIpAddress,
            this.colPortNr,
            this.colVersion,
            this.colDataTrnasfer});
            this.grid.MultiSelect = false;
            this.grid.Name = "grid";
            this.grid.ReadOnly = true;
            this.grid.RowHeadersVisible = false;
            this.grid.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.grid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid.SelectionChanged += new System.EventHandler(this.grid_SelectionChanged);
            this.grid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.grid_MouseDoubleClick);
            // 
            // colName
            // 
            this.colName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colName.FillWeight = 46.599F;
            resources.ApplyResources(this.colName, "colName");
            this.colName.Name = "colName";
            this.colName.ReadOnly = true;
            // 
            // colIpAddress
            // 
            this.colIpAddress.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colIpAddress.FillWeight = 25.62944F;
            resources.ApplyResources(this.colIpAddress, "colIpAddress");
            this.colIpAddress.Name = "colIpAddress";
            this.colIpAddress.ReadOnly = true;
            // 
            // colPortNr
            // 
            this.colPortNr.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colPortNr.FillWeight = 23.29949F;
            resources.ApplyResources(this.colPortNr, "colPortNr");
            this.colPortNr.Name = "colPortNr";
            this.colPortNr.ReadOnly = true;
            // 
            // colVersion
            // 
            this.colVersion.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colVersion.FillWeight = 25.62944F;
            resources.ApplyResources(this.colVersion, "colVersion");
            this.colVersion.Name = "colVersion";
            this.colVersion.ReadOnly = true;
            // 
            // colDataTrnasfer
            // 
            this.colDataTrnasfer.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.colDataTrnasfer.FillWeight = 82.84264F;
            resources.ApplyResources(this.colDataTrnasfer, "colDataTrnasfer");
            this.colDataTrnasfer.Name = "colDataTrnasfer";
            this.colDataTrnasfer.ReadOnly = true;
            // 
            // spPortNr
            // 
            resources.ApplyResources(this.spPortNr, "spPortNr");
            this.spPortNr.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.spPortNr.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.spPortNr.Name = "spPortNr";
            this.spPortNr.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // cbAddress
            // 
            resources.ApplyResources(this.cbAddress, "cbAddress");
            this.cbAddress.DefaultImageIndex = -1;
            this.cbAddress.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbAddress.ImageList = null;
            this.cbAddress.Name = "cbAddress";
            this.cbAddress.SelectedIndexChanged += new System.EventHandler(this.cbAddress_SelectedIndexChanged);
            // 
            // ServerSelectionForm
            // 
            this.AcceptButton = this.btApply;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.Controls.Add(this.spPortNr);
            this.Controls.Add(this.grid);
            this.Controls.Add(this.cbAddress);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.ckAutoConnect);
            this.Controls.Add(this.btSearch);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btApply);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerSelectionForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.spPortNr)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

		}
		#endregion

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad (e);

            DoInitialSearch();
        }


		private void btApply_Click(object sender, System.EventArgs e)
		{
            Cursor = Cursors.WaitCursor;

            if(cbAddress.SelectedItem == noneItem)
                serverConfig.Address = "";
            else
                serverConfig.Address = cbAddress.Text.ToUpper();

            serverConfig.PortNr = Convert.ToInt32(spPortNr.Value);
            //clientConfig.AutoReconnect = ckAutoConnect.Checked;

            DataTransferServerInfo service = (DataTransferServerInfo)_discoveredServices[cbAddress.Text];

            if (OnServerInfoSelected != null && grid.SelectedRows.Count == 1)
            {
                var dataInfoRow = grid.SelectedRows[0];

                const int serverColumn = 0;
                var server = dataInfoRow.Cells[serverColumn].Value.ToString();

                if (_discoveredServices[server] != null)
                {
                    service = (DataTransferServerInfo)_discoveredServices[server];

                    OnServerInfoSelected(server, service.Port.ToString(), service.RunsWithService);
                }

            }

            if (OnServerInfoSelected != null && grid.SelectedRows.Count == 0 && service != null)
            {
                OnServerInfoSelected(cbAddress.Text, spPortNr.Value.ToString(), service.RunsWithService);
            }

            if (OnServerInfoSelected != null && IsDataTransferTaskContext && grid.SelectedRows.Count == 0)
            {
                OnServerInfoSelected(cbAddress.Text, spPortNr.Value.ToString(), Program.ServiceEnum.NOSERVICE);
            }

            SaveMRUList();

			DialogResult = DialogResult.OK;
			Close();
		}

		private void btCancel_Click(object sender, System.EventArgs e)
		{
			DialogResult = DialogResult.Cancel;
			Close();
		}

        protected override void OnClosing(CancelEventArgs e)
        {
            if(ar != null)
            {
                stopTimer.Enabled = false;
                ServiceLocator.StopLocateService(ar);
            }

            base.OnClosing (e);
        }

        private void grid_SelectionChanged(object sender, EventArgs e)
        {
            if ((stopTimer.Interval != 1000) && (grid.SelectedRows.Count > 0))
            {
                cbAddress.Text = grid.SelectedRows[0].Cells[0].Value.ToString();

                string portVal;

                var serverInfo = _discoveredServices[cbAddress.Text] as DataTransferServerInfo;

                if (serverInfo != null && serverInfo.RunsWithService == Program.ServiceEnum.NOSERVICE)
                {
                    spPortNr.Text = string.Empty;
                }

                portVal = IsDataTransferTaskContext ? serverInfo?.Port.ToString() : grid.SelectedRows[0].Cells[2].Value.ToString();

                if (int.TryParse(portVal, out var portNr))
                {
                    spPortNr.SetIntValue(portNr);
                    spPortNr.Text = portNr.ToString();
                }

            }
            else
                grid.ClearSelection();
        }

        public bool IsDataTransferTaskContext { get; set; }

        private void grid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var hitInfo = grid.HitTest(e.X, e.Y);


            if((hitInfo.Type == DataGridViewHitTestType.Cell) && btApply.Enabled)
                btApply_Click(sender, e);
        }

        public event Action<string ,string, Program.ServiceEnum> OnServerInfoSelected;

        private void DoInitialSearch()
        {
            stopTimer.Interval = 1000;
            OnSearchServers(this, EventArgs.Empty);
        }

        IAsyncResult ar = null;
        private void OnSearchServers(object sender, System.EventArgs e)
        {
            Cursor = Cursors.WaitCursor;
            if(ar != null)
            {
                //Stop searching
                stopTimer.Enabled = false;
                try
                {
                    ServiceLocator.StopLocateService(ar);  
                }
                catch(Exception ex)
                {
                    ibaLogger.Log(Level.Exception, "Error stopping server search : " + ex.Message);
                }
                panel1.Controls.Remove(waitBar);
                waitBar.Dispose();
                waitBar = null;
                ar = null;

                btSearch.Text = iba.Properties.Resources.Search;
            }
            else
            {
                grid.Rows.Clear();

                waitBar = new iba.Controls.GradientWaitingBar();
                waitBar.Interval = 20;
                waitBar.Speed = 2;
                //waitBar = new iba.Utility.Controls.BouncingProgressBar();
                waitBar.Dock = DockStyle.Fill;
                panel1.Controls.Add(waitBar);
                //waitBar.Start();

                btSearch.Text = iba.Properties.Resources.StopStr;
                try
                {
                    ar = ServiceLocator.StartLocateService(DatcoServerDefaults.ServerGuid,
                        DatcoServerDefaults.GroupAddress, DatcoServerDefaults.GroupServerPort, this, false, true);
                    stopTimer.Enabled = true;
                }
                catch(Exception ex)
                {
                    ibaLogger.Log(Level.Exception, "Error starting server search : " + ex.Message);
                    
                    panel1.Controls.Remove(waitBar);
                    waitBar.Dispose();
                    waitBar = null;
                    ar = null;

                    btSearch.Text = iba.Properties.Resources.Search;
                }
            }
            Cursor = Cursors.Default;
        }

        #region IServiceDiscovered Members

        delegate void ServiceDiscoveredDelegate(iba.Utility.ServiceLocator.HostResponse host);

        private IDictionary _discoveredServices = new Dictionary<string, DataTransferServerInfo>();

        public void ServiceDiscovered(iba.Utility.ServiceLocator.HostResponse host)
        {
            if(InvokeRequired)
            {
                BeginInvoke(new ServiceDiscoveredDelegate(ServiceDiscovered), new object[] {host});
                return;
            }

            var key = host.EndpointProperties["HostName"];
            var value = host.EndpointProperties["DataTransferServer"];
            _discoveredServices[key] = value;

            string[] subItems = new string[5];
            subItems[0] = host.EndpointProperties["HostName"] as string;
            subItems[1] = host.IPAddress.ToString();
            subItems[2] = host.EndpointProperties["PortNr"] as string;
            subItems[3] = host.EndpointProperties["Version"] as string;

            DataTransferServerInfo dataTransferServerInfo = null;

            if (host.EndpointProperties["DataTransferServer"] != null)
            {
                dataTransferServerInfo = (DataTransferServerInfo)host.EndpointProperties["DataTransferServer"];

                subItems[4] = dataTransferServerInfo.IsServerEnabled ? $"Running on port: {dataTransferServerInfo.Port}" : "Not Running";
            }

            if (IsDataTransferTaskContext)
            {
                SetupFormForDataTransferTask(host);
                return;
            }

            int version = Math.Abs(DatCoVersion.CurrentVersion());
            //int clientVersion = Math.Abs(DatCoVersion.MinimumClientVersion());

            DataGridViewRow newRow = grid.Rows[grid.Rows.Add(subItems[0], subItems[1], subItems[2], subItems[3], subItems[4], "")];
            newRow.Tag = host;
            if(host.EndpointProperties["MinimumClientVersion"] != null)
            {
                int reqClientVersion = (int) host.EndpointProperties["MinimumClientVersion"];
                if (version >= reqClientVersion)
                {
                    newRow.DefaultCellStyle.ForeColor = Color.Green;
                }
                else
                    newRow.DefaultCellStyle.ForeColor = Color.Red;      //need to upgrade manually

                if (dataTransferServerInfo != null && dataTransferServerInfo.RunsWithService == Program.ServiceEnum.NOSERVICE)
                {
                    newRow.Cells[2].Value = string.Empty;
                    newRow.DefaultCellStyle.ForeColor = Color.Red;
                }
            }
        }

        private void SetupFormForDataTransferTask(ServiceLocator.HostResponse hostResponse)
        {
            var props = hostResponse.EndpointProperties;

            if (!props.Contains("DataTransferServer")) return;

            var serverInfo = (DataTransferServerInfo)props["DataTransferServer"];
            var hostname = props["HostName"];
            var ip = hostResponse.IPAddress.ToString();
            var port = serverInfo.Port;
            var version = props["Version"];

            var runInfo = serverInfo.IsServerEnabled ? "Running" : "Not Running";

            var newRow = grid.Rows[grid.Rows.Add(hostname,ip, port, version, runInfo, "")];

            newRow.DefaultCellStyle.ForeColor = serverInfo.IsServerEnabled ? Color.Green : Color.Red;
        }

        #endregion

        private void OnStopTimerTick(object sender, EventArgs e)
        {
            stopTimer.Enabled = false;
            if(ar != null)
                OnSearchServers(this, EventArgs.Empty);
            stopTimer.Interval = 5000;
        }

        private void cbAddress_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cb = (ComboBox)sender;

            var serverInfo = (DataTransferServerInfo)_discoveredServices[cb.Text];

            if (serverInfo != null && serverInfo.RunsWithService == Program.ServiceEnum.NOSERVICE)
            {
                if (IsDataTransferTaskContext)
                {
                    spPortNr.Value = serverInfo.Port;
                    spPortNr.Text = serverInfo.Port.ToString();
                }
                else
                {
                    spPortNr.Text = string.Empty;
                }
            }
            else if(serverInfo != null && serverInfo.RunsWithService != Program.ServiceEnum.NOSERVICE)
            {
                if (IsDataTransferTaskContext)
                {
                    spPortNr.Value = serverInfo.Port;
                    spPortNr.Text = serverInfo.Port.ToString();
                }
            }
            else
            {
                spPortNr.Value = serverConfig.PortNr;
                spPortNr.Text = serverConfig.PortNr.ToString();
            }
        }
    }

    public class ServerConfiguration
    {
        public string Address;
        public int PortNr;
        public bool Enabled
        {
            get { return !String.IsNullOrEmpty(Address); }
        }
    }

}
