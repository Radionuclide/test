using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.IO;
using iba.Utility;
using System.Collections.Generic;

namespace iba.Controls
{
	/// <summary>
	/// Summary description for ServerFolderBrowser.
	/// </summary>
	public class ServerFolderBrowser : System.Windows.Forms.Form
	{
        private System.Windows.Forms.TreeView tree;
        private System.Windows.Forms.TextBox tbPath;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.ImageList imageList1;
        private System.ComponentModel.IContainer components;

        private IPdaServerFiles serverFiles;
        private bool bOnlyFixed = true;
        private bool bShowFiles = false;

        private object dummyTag = new object();
        private bool bIgnoreChange = false;
        private static readonly int DRIVE          = 0;
        private static readonly int FOLDER         = 1;
        private static readonly int SELECTEDFOLDER = 2;
        private static readonly int FILE           = 3;

        private string m_filter = "";
        private List<string> filterOptions = new List<string>();

        private ComboBox m_cbExtension;

        public ServerFolderBrowser(bool bShowFiles = false, string filter = "")
        {
            this.bShowFiles = bShowFiles;
            //
            // Required for Windows Form Designer support
            //
            InitializeComponent();
            UpdateCaption();
            m_filter = filter;
            UpdateComboSelection();
        }

        private void UpdateComboSelection()
        {
            m_cbExtension.Items.Clear();
            if (string.IsNullOrEmpty(m_filter))
            {
                m_cbExtension.Visible = false;
                return;
            }
            string[] tokenized = m_filter.Split('|');
            for (int i = 0; i < tokenized.Length; i+=2)
            {
                m_cbExtension.Items.Add(tokenized[i]);
                filterOptions.Add(tokenized[i + 1]);
            }
            m_cbExtension.Visible = true;
            m_cbExtension.SelectedIndex = 0;
        }

        private void UpdateCaption()
        {
            string formatter = bShowFiles ? iba.Properties.Resources.BrowseForFileOn : iba.Properties.Resources.BrowseForFolderOn;
            try
            {
                serverFiles = Program.CommunicationObject.GetServerSideFileHandler();
                Text = string.Format(formatter, Program.CommunicationObject.ServerName);
            }
            catch
            {
                serverFiles = null;
                Text = string.Format(formatter, iba.Properties.Resources.BrowseForFolderOn, "");
            }
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ServerFolderBrowser));
            this.tree = new System.Windows.Forms.TreeView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.tbPath = new System.Windows.Forms.TextBox();
            this.btOK = new System.Windows.Forms.Button();
            this.btCancel = new System.Windows.Forms.Button();
            this.m_cbExtension = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // tree
            // 
            resources.ApplyResources(this.tree, "tree");
            this.tree.HideSelection = false;
            this.tree.ImageList = this.imageList1;
            this.tree.ItemHeight = 16;
            this.tree.Name = "tree";
            this.tree.BeforeExpand += new System.Windows.Forms.TreeViewCancelEventHandler(this.tree_BeforeExpand);
            this.tree.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.tree_AfterSelect);
            // 
            // imageList1
            // 
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "");
            this.imageList1.Images.SetKeyName(1, "");
            this.imageList1.Images.SetKeyName(2, "");
            this.imageList1.Images.SetKeyName(3, "");
            // 
            // tbPath
            // 
            resources.ApplyResources(this.tbPath, "tbPath");
            this.tbPath.Name = "tbPath";
            this.tbPath.TextChanged += new System.EventHandler(this.tbPath_TextChanged);
            // 
            // btOK
            // 
            resources.ApplyResources(this.btOK, "btOK");
            this.btOK.Name = "btOK";
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btCancel
            // 
            resources.ApplyResources(this.btCancel, "btCancel");
            this.btCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btCancel.Name = "btCancel";
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // m_cbExtension
            // 
            this.m_cbExtension.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.m_cbExtension.FormattingEnabled = true;
            resources.ApplyResources(this.m_cbExtension, "m_cbExtension");
            this.m_cbExtension.Name = "m_cbExtension";
            this.m_cbExtension.SelectedIndexChanged += new System.EventHandler(this.m_cbExtension_SelectedIndexChanged);
            // 
            // ServerFolderBrowser
            // 
            this.AcceptButton = this.btOK;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btCancel;
            this.Controls.Add(this.m_cbExtension);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.tbPath);
            this.Controls.Add(this.tree);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ServerFolderBrowser";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.ResumeLayout(false);
            this.PerformLayout();

        }
		#endregion

        public bool ShowFiles
        {
            get
            {
                return bShowFiles;
            }
            set
            {
                if (value != bShowFiles)
                {
                    bShowFiles = value;
                    UpdateCaption();
                }
            }
        }

        public bool FixedDrivesOnly
        {
            get {return bOnlyFixed;}
            set {bOnlyFixed = value;}
        }

        public string SelectedPath
        {
            get {return tbPath.Text;}
            set {tbPath.Text = value;}
        }

        public string Filter
        {
            get
            {
                return m_filter;
            }
            set
            {
                if (value != m_filter)
                {
                    m_filter = value;
                    UpdateComboSelection();
                }
            }
        }

        private void Init()
        {
            if (serverFiles == null)
                return;

            using (WaitCursor wait = new WaitCursor())
            {
                string[] drives = null;
                try
                {
                    drives = serverFiles.BrowseForDrives(bOnlyFixed);
                    if (drives == null)
                        return;
                }
                catch (Exception)
                {
                    return;
                }

                bIgnoreChange = true;
                tree.BeginUpdate();
                tree.Nodes.Clear();
                for (int i = 0; i < drives.Length; i++)
                {
                    TreeNode driveNode = new TreeNode(drives[i], DRIVE, DRIVE);
                    driveNode.Tag = drives[i];
                    driveNode.Nodes.Add(CreateDummyNode());
                    tree.Nodes.Add(driveNode);
                }
                tree.EndUpdate();
                bIgnoreChange = false;

                SelectDir(tbPath.Text);
            }
        }

        protected override void OnLoad(EventArgs e)
        {           
            base.OnLoad (e);
            Init();
            return;
        }

        private TreeNode CreateDummyNode()
        {
            TreeNode dummy = new TreeNode("", FILE, FILE);
            dummy.Tag = dummyTag;
            return dummy;
        }

        private void SelectDir(string dir)
        {
            string[] parts = dir.Split('\\');
            if(parts.Length > 0)
                parts[0] += "\\";
            TreeNodeCollection nodes = tree.Nodes;
            TreeNode selectedNode = null;
            for(int i=0; i<parts.Length; i++)
            {
                bool bFound = false;
                for(int j=0; j<nodes.Count; j++)
                {
                    if(String.Compare(nodes[j].Text, parts[i], true) == 0)
                    {
                        selectedNode = nodes[j];
                        nodes = selectedNode.Nodes;
                        if(!selectedNode.IsExpanded)
                            selectedNode.Expand();
                        bFound = true;
                        break;
                    }       
                }

                if(!bFound)
                    break;
            }

            if(selectedNode != null)
            {
                bIgnoreChange = true;
                tree.SelectedNode = selectedNode;
                bIgnoreChange = false;
            }
        }

        private void tbPath_TextChanged(object sender, System.EventArgs e)
        {
            if(!Visible || bIgnoreChange)
                return;

            SelectDir(tbPath.Text);
        }

        private void tree_BeforeExpand(object sender, System.Windows.Forms.TreeViewCancelEventArgs e)
        {
            if(!bIgnoreChange)
                tree.SelectedNode = e.Node;

            if((e.Node == null) || (e.Node.Nodes.Count < 1) || (e.Node.Nodes[0].Tag != dummyTag))
                return;

            using(Utility.WaitCursor wait = new Utility.WaitCursor())
            {
                TreeNode baseNode = e.Node;
                baseNode.Nodes.Clear();
                
                if(serverFiles == null)
                    return;

                string basePath = baseNode.Tag as string;
                if(basePath == null)
                    return;

                FileSystemEntry[] entries = null;
                try
                {
                    string extension = "";
                    if (!string.IsNullOrEmpty(m_filter))
                    {
                        int sel = Math.Max(0,m_cbExtension.SelectedIndex);
                        extension = filterOptions[sel];
                    }
                    entries = serverFiles.Browse(basePath, bShowFiles,extension);
                    if(entries == null)
                        return;
                }
                catch(Exception)
                {
                    return;
                }

                tree.BeginUpdate();
                for(int i=0; i<entries.Length; i++)
                {
                    if(entries[i].IsFile)
                    {
                        TreeNode fileNode = new TreeNode(entries[i].Name, FILE, FILE);
                        fileNode.Tag = Path.Combine(basePath, entries[i].Name);
                        baseNode.Nodes.Add(fileNode);
                    }
                    else
                    {
                        TreeNode dirNode = new TreeNode(entries[i].Name, FOLDER, SELECTEDFOLDER);
                        dirNode.Tag = Path.Combine(basePath, entries[i].Name);
                        dirNode.Nodes.Add(CreateDummyNode());
                        baseNode.Nodes.Add(dirNode);
                    }
                }
                tree.EndUpdate();
            }
        }

        private void tree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if(bIgnoreChange || (e.Node == null))
                return;

            bIgnoreChange = true;
            tbPath.Text = e.Node.Tag as string;
            bIgnoreChange = false;
        }

        private void btOK_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void btCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void m_cbExtension_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(m_filter)) Init();
        }
    }
}
