using Crownwood.DotNetMagic.Controls;
using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using iba.HD.Client.Interfaces;
using iba.Plugins;
using iba.Remoting;
using iba.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.Controls
{
    #region AnalyzerManager
    internal class AnalyzerManager : IDisposable, IAnalyzerManagerUpdateSource
    {
        #region Members
        object lockAnalyzer;
        public IbaAnalyzer.IbaAnalyzer Analyzer { get; private set; }

        bool bFilesOpened;
        public bool IsOpened => bFilesOpened;
        DateTime m_dtLastWriteTime;
        string pdoFile, datFile, datFilePassword, user;

        public event Action SourceUpdated;
        #endregion

        #region Initialize
        public AnalyzerManager()
        {
            lockAnalyzer = new object();
            Analyzer = null;

            bFilesOpened = false;
            m_dtLastWriteTime = DateTime.MinValue.ToUniversalTime();
            pdoFile = datFile = datFilePassword = "";
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            lock (lockAnalyzer)
            {
                try
                {
                    if (Analyzer != null)
                    {
                        ((IDisposable)Analyzer)?.Dispose();
                        Analyzer = null;
                    }
                }
                catch
                { }
            }
        }
        #endregion

        public bool OpenAnalyzer(out string error)
        {
            error = "";
            if (Program.RunsWithService == Program.ServiceEnum.DISCONNECTED)
            {
                error = Properties.Resources.treeErrorNoService;
                return false;
            }
            lock (lockAnalyzer)
            {
                try
                {

                    if (Analyzer == null)
                    {
                        if (!VersionCheck.CheckIbaAnalyzerVersion("7.1.0"))
                            throw new Exception(string.Format(Properties.Resources.logAnalyzerVersionError, "7.1.0"));
                        Analyzer = ibaAnalyzerExt.Create(false); //noninteractive = false because of signal tree images...
                    }



                    if (!bFilesOpened)
                    {
                        if (!String.IsNullOrEmpty(pdoFile))
                        {
                            Analyzer.OpenAnalysis(pdoFile);
                        }
                        if (!string.IsNullOrEmpty(datFile))
                        {
                            if (Path.GetExtension(datFile) == ".hdq")
                            {
                                if (!string.IsNullOrEmpty(user))
                                {
                                    Analyzer.SetHDCredentials(user, datFilePassword);
                                }
                            }
                            else
                                if (!string.IsNullOrEmpty(datFilePassword))
                                Analyzer.SetFilePassword("", datFilePassword);
                            Analyzer.OpenDataFile(0, datFile);
                        }
                        bFilesOpened = true;
                    }

                    return true;
                }
                catch (Exception ex)
                {
                    try
                    {
                        error = Analyzer?.GetLastError();
                    }
                    catch
                    { }

                    if (string.IsNullOrEmpty(error))
                        error = ex.Message;

                    UnsafeClose();

                    return false;
                }
            }
        }

        void UnsafeClose()
        {
            try
            {
                if (Analyzer != null)
                {
                    Analyzer.CloseAnalysis();
                    Analyzer.CloseDataFiles();
                }
            }
            catch
            { }

            m_dtLastWriteTime = DateTime.MinValue.ToUniversalTime();
            bFilesOpened = false;
        }

        public void UpdateSource(string pdoFile, string datFile, string datFilePassword, IJobData data)
        {
            try
            {
                lock (lockAnalyzer)
                {
                    this.pdoFile = pdoFile;
                    this.datFile = datFile;
                    if (Path.GetExtension(datFile) == ".hdq")
                    {
                        this.user = data.HdUser;
                        this.datFilePassword = data.HdPass;
                    }
                    else this.datFilePassword = datFilePassword;
                    UnsafeClose();
                }

                SourceUpdated?.Invoke();
            }
            catch
            {
                //illegal characters in path...
            }
        }

        public void UnLoadAnalysis()
        {
            try
            {
                if (Analyzer != null)
                {
                    Analyzer.CloseAnalysis();
                }
            }
            catch
            { }
        }

        public void ReopenAnalysis()
        {
            try
            {
                if (Analyzer != null)
                {
                    Analyzer.OpenAnalysis(pdoFile);
                }
            }
            catch
            { }
        }

        public void OnLeave()
        {
            if (Program.RunsWithService == Program.ServiceEnum.NOSERVICE || Program.ServiceIsLocal) return;
            if (Analyzer is ibaAnalyzerClientWrapper && ((ibaAnalyzerClientWrapper)Analyzer).TestStale())
            {
                bFilesOpened = false;
                Analyzer = null;
            }
        }
    }
    #endregion

    #region AnalyzerTreeControl

    [Flags]
    public enum ChannelTreeFilter
    {
        Analog = 0x01,
        Digital = 0x02,
        Text = 0x04,
        Expressions = 0x100000,
        Logicals = 0x200000,
        Infofields = 0x400000
    };

    internal class AnalyzerTreeControl : Panel, ISignalDragDropHandler
    {
        private class CustomNode
        {
            public string Text;
            public string ChannelID;
            public Image Image;
        }

        #region Members
        const int Unloading = -1;
        const int Unloaded = 0;
        const int Loading = 1;
        const int Loaded = 2;
        bool ignoreCheckChange = false;

        int iState;
        AnalyzerManager manager;
        ChannelTreeFilter filter;

        List<Node> customTreeNodes;
        List<CustomNode> customNodes;

        string pendingSelection;

        protected Crownwood.DotNetMagic.Controls.TreeControl tree;

        IbaAnalyzer.ISignalTree analyzerTree;

        public event Action<AnalyzerTreeControl, string> AfterSelect;
        public event Action<AnalyzerTreeControl, string> CustomDoubleClick;

        public bool IsLoading => iState == Loading;

        public AnalyzerManager AnalyzerManager => manager;
        #endregion

        #region Initialize
        public AnalyzerTreeControl(AnalyzerManager manager, ChannelTreeFilter filter)
        {
            this.manager = manager;
            this.filter = filter;

            iState = Unloaded;

            customNodes = new List<CustomNode>();
            customTreeNodes = new List<Node>();

            pendingSelection = "";

            Padding = new Padding(0);

            tree = new Crownwood.DotNetMagic.Controls.TreeControl();
            tree.ImageList = emptyList;
            tree.Dock = DockStyle.Fill;
            tree.Location = Point.Empty;

            Controls.Add(tree);

            manager.SourceUpdated += Reset;
            this.tree.NodeDrag += new Crownwood.DotNetMagic.Controls.StartDragEventHandler(this.tree_NodeDrag);

            CreateHandle();
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Reset();

                Controls?.Clear();
                if (tree != null)
                {
                    tree.Dispose();
                    tree = null;
                }

                manager.SourceUpdated -= Reset;
            }

            base.Dispose(disposing);
        }
        #endregion

        #region Checkboxes
        public event Crownwood.DotNetMagic.Controls.NodeEventHandler TreeAfterCheck;
        protected void OnTreeAfterCheck(Crownwood.DotNetMagic.Controls.TreeControl tc, Crownwood.DotNetMagic.Controls.NodeEventArgs e)
        {
            if (TreeAfterCheck != null)
                TreeAfterCheck(tc, e);
        }

        public void UncheckAllNodes()
        {
            UncheckNodes(tree.Nodes);
        }

        private void UncheckNodes(NodeCollection nodes)
        {
            foreach (Node node in nodes)
            {
                if (node.CheckState != Crownwood.DotNetMagic.Controls.CheckState.Unchecked)
                {
                    node.CheckState = Crownwood.DotNetMagic.Controls.CheckState.Unchecked;
                    if (node.Nodes.Count > 0)
                        UncheckNodes(node.Nodes);
                }
            }
        }

        //Update check state of all parent nodes
        public void UpdateChecks()
        {
            UpdateChecksForNodes(tree.Nodes);
        }

        public bool UpdateAfterCheck(Node node)
        {
            if (ignoreCheckChange)
                return false;

            ignoreCheckChange = true;

            //Update all child nodes
            CheckChildNodes(node, node.Checked);

            //Update check state of parent nodes
            if (node.Parent != null)
                UpdateCheckStateNode(node.Parent);

            ignoreCheckChange = false;

            return true;
        }

        private void CheckChildNodes(Node parentNode, bool bCheck)
        {
            foreach (Node node in parentNode.Nodes)
            {
                node.Checked = bCheck;
                if (node.Nodes.Count > 0)
                    CheckChildNodes(node, bCheck);
            }
        }

        private void UpdateCheckStateNode(Node node)
        {
            Crownwood.DotNetMagic.Controls.CheckState savedState = node.CheckState;

            int nrChecked = 0;
            int nrMixed = 0;
            foreach (Node childNode in node.Nodes)
            {
                switch (childNode.CheckState)
                {
                    case Crownwood.DotNetMagic.Controls.CheckState.Checked:
                        nrChecked++;
                        break;
                    case Crownwood.DotNetMagic.Controls.CheckState.Mixed:
                        nrMixed++;
                        break;
                }
            }

            if ((nrChecked == 0) && (nrMixed == 0))
                node.CheckState = Crownwood.DotNetMagic.Controls.CheckState.Unchecked;
            else if ((nrChecked == node.Nodes.Count) && (nrMixed == 0))
                node.CheckState = Crownwood.DotNetMagic.Controls.CheckState.Checked;
            else
                node.CheckState = Crownwood.DotNetMagic.Controls.CheckState.Mixed;

            if (savedState != node.CheckState)
            {
                if (node.Parent != null)
                    UpdateCheckStateNode(node.Parent);
            }
        }

        private void tree_AfterCheck(Crownwood.DotNetMagic.Controls.TreeControl tc, Crownwood.DotNetMagic.Controls.NodeEventArgs e)
        {
            if (UpdateAfterCheck(e.Node))
                OnTreeAfterCheck(tc, e);
        }

        protected Crownwood.DotNetMagic.Controls.CheckState UpdateChecksForNodes(NodeCollection nodes)
        {
            bool uncheck = false;
            bool check = false;
            bool mixed = false;
            foreach (Node node in nodes)
            {
                if (node.Nodes.Count > 0)
                    node.CheckState = UpdateChecksForNodes(node.Nodes);

                switch (node.CheckState)
                {
                    case Crownwood.DotNetMagic.Controls.CheckState.Unchecked:
                        uncheck = true;
                        break;

                    case Crownwood.DotNetMagic.Controls.CheckState.Checked:
                        check = true;
                        break;

                    default:
                        mixed = true;
                        break;
                }
            }

            if (mixed || (check && uncheck))
                return Crownwood.DotNetMagic.Controls.CheckState.Mixed;
            else if (uncheck)
                return Crownwood.DotNetMagic.Controls.CheckState.Unchecked;
            else
                return Crownwood.DotNetMagic.Controls.CheckState.Checked;
        }
        #endregion

        #region Drag/Drop

        public event EventHandler OnDragOverChannelResponse;

        public event EventHandler OnDragDropChannelResponse;


        public void OnDragOverHandle(object sender, EventArgs drgevent)
        {
            DragDetailsEvent dragDetailsEvent = drgevent as DragDetailsEvent;
            SelectedNodeCollection inodes = (SelectedNodeCollection)dragDetailsEvent.drgEvent.Data.GetData(typeof(SelectedNodeCollection));
            if (inodes != null)
            {
                List<Tuple<string, string>> validSignals = new List<Tuple<string, string>>();
                foreach (Node node in inodes)
                {
                    IbaAnalyzer.ISignalTreeNode analyzerNode = (IbaAnalyzer.ISignalTreeNode)(node.Tag);
                    string id = analyzerNode.channelID;
                    if (ContainsNode(id))
                        validSignals.Add(Tuple.Create(id, node.Text));

                }
                if (validSignals.Count > 0)
                    OnDragOverChannelResponse?.Invoke(this, new DragDetailsResponseEvent(validSignals, dragDetailsEvent.numeric, true, dragDetailsEvent.drgEvent));

            }
        }

        public void OnDragDropHandle(object sender, EventArgs drgevent)
        {
            DragDetailsEvent dragDetailsEvent = drgevent as DragDetailsEvent;
            SelectedNodeCollection inodes = (SelectedNodeCollection)dragDetailsEvent.drgEvent.Data.GetData(typeof(SelectedNodeCollection));
            if (inodes != null)
            {
                List<Tuple<string, string>> validSignals = new List<Tuple<string, string>>();
                foreach (Node node in inodes)
                {
                    IbaAnalyzer.ISignalTreeNode analyzerNode = (IbaAnalyzer.ISignalTreeNode)(node.Tag);
                    string id = analyzerNode.channelID;
                    if (ContainsNode(id))
                        validSignals.Add(Tuple.Create(id, node.Text));
                }
                if (validSignals.Count > 0)
                    OnDragDropChannelResponse?.Invoke(this, new DragDetailsResponseEvent(validSignals, dragDetailsEvent.numeric, true, dragDetailsEvent.drgEvent));

            }
        }
        #endregion


        public string SelectedChannel
        {
            get
            {
                if (iState != Loaded || tree.SelectedNode == null)
                    return "";

                if (tree.SelectedNode.Tag is CustomNode cstNode)
                    return cstNode.ChannelID;

                if (tree.SelectedNode.Tag is IbaAnalyzer.ISignalTreeNode sigNode)
                    return sigNode.channelID;

                return "";
            }
            set
            {
                if (iState == Loaded)
                {
                    Node node = FindNode(value);
                    tree.ClearSelection();
                    if (node != null)
                        tree.SelectedNode = node;
                }
                else
                    pendingSelection = value;
            }
        }

        public Control TreeControl { get { return tree; } }

        public void Reset()
        {
            int prevState = Interlocked.Exchange(ref iState, Unloading);
            if (prevState == Unloaded)
            {
                Interlocked.Exchange(ref iState, Unloaded);
                return;
            }

            if (prevState == Unloading)
                return;

            if (tree != null)
            {
                tree.AfterSelect -= treeView_AfterSelect;
                tree.AfterExpand -= treeView_AfterExpand;
                tree.AfterCheck -= tree_AfterCheck;
                ClearNodes();
            }

            if (analyzerTree != null)
                ((IDisposable)analyzerTree).Dispose();
            analyzerTree = null;
            pendingSelection = "";

            Interlocked.Exchange(ref iState, Unloaded);
        }

        private bool checkBoxes;

        [DefaultValue(false)]
        public bool CheckBoxes
        {
            get { return checkBoxes; }
            set
            {
                checkBoxes = value;
                tree.CheckStates = value ? CheckStates.TwoStateCheck : CheckStates.None;
            }
        }

        [DefaultValue(false)]
        public bool MultiSelect
        {
            get { return tree.SelectMode == SelectMode.Multiple; }
            set
            {
                if (MultiSelect == value)
                    return;

                if (value)
                {
                    tree.SelectMode = SelectMode.Multiple;
                    tree.SelectSameLevel = true;
                }
                else
                {
                    tree.SelectMode = SelectMode.Single;
                }
            }
        }

        string OpenAnalyzer()
        {
            try
            {
                if (!manager.OpenAnalyzer(out string error))
                    return error;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return null;
        }

        #region ImageList

        const int ImageWidth = 16;
        const int ImageHeigth = 16;
        static ImageList imageList;
        static ImageList emptyList;
        static object imageListLock = new object();

        static AnalyzerTreeControl()
        {
            emptyList = new ImageList();
            emptyList.ImageSize = new Size(ImageWidth, ImageHeigth);
            emptyList.TransparentColor = Color.Magenta;
            emptyList.ColorDepth = ColorDepth.Depth32Bit;
            imageList = emptyList;
        }

        static ImageList GetImageList(AnalyzerManager manager)
        {
            lock (imageListLock)
            {
                if ((imageList != null) && (imageList.Images.Count > 0))
                    return imageList;

                imageList = CreateImageList(manager);
                return imageList;
            }
        }

        static unsafe ImageList CreateImageList(AnalyzerManager manager)
        {
            if (manager.Analyzer == null)
                return emptyList;

            ImageList newList = new ImageList();
            newList.ImageSize = new Size(ImageWidth, ImageHeigth);
            newList.TransparentColor = Color.Magenta;
            newList.ColorDepth = ColorDepth.Depth32Bit;

            int cnt = manager.Analyzer.SignalTreeImageCount;
            for (int i = 0; i < cnt; i++)
            {
                object myArrayObject = null;
                manager.Analyzer.SignalTreeImageData(i, out myArrayObject); //make sure you always provide "NULL"
                byte[] pixelData = myArrayObject as byte[];

                fixed (byte* p = pixelData)
                {
                    Bitmap bm = new Bitmap(ImageWidth, ImageHeigth, ImageWidth * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, new IntPtr(p));
                    newList.Images.Add(bm);
                }
            }

            return newList;
        }

        #endregion

        public Node GetSignalNode(string id)
        {
            return FindNode(id);
        }

        void ReleaseSignalNodes(NodeCollection nodes)
        {
            foreach (Node node in nodes)
            {
                ReleaseSignalNodes(node.Nodes);

                if (node.Tag is IDisposable)
                {
                    (node.Tag as IDisposable).Dispose();
                }
            }
        }

        void ClearNodes()
        {
            try
            {
                ReleaseSignalNodes(tree.Nodes);
            }
            catch (Exception)
            {

            }
            customTreeNodes.Clear();
            tree.Nodes.Clear();
        }

        private void UpdateTree()
        {
            if (tree.InvokeRequired)
            {
                tree.BeginInvoke(new Action(UpdateTree));
                return;
            }

            tree.Update();
        }

        void CreateTree(Task<string> task)
        {
            if (Disposing || IsDisposed || iState != Loading)
                return;

            if (tree.InvokeRequired)
            {
                tree.BeginInvoke(new Action<Task<string>>(CreateTree), task);
                return;
            }

            ClearNodes();

            //Check if there was an error when opening ibaAnalyzer
            string error = "";
            if (task != null && (task.IsFaulted || !string.IsNullOrEmpty(task.Result)))
                error = task.Exception?.Message ?? task.Result ?? Properties.Resources.AnalyzerTree_UnknownError;
            else
            {
                try
                {
                    analyzerTree = manager.Analyzer.GetSignalTree((int)filter) as IbaAnalyzer.ISignalTree; //could throw null ref exceptions when disconnected
                    if (analyzerTree == null) //likely a disconnect
                        error = Program.RunsWithService == Program.ServiceEnum.DISCONNECTED ? Properties.Resources.treeErrorNoService : Properties.Resources.treeErrorGeneric;
                }
                catch (Exception)
                {
                    error = Program.RunsWithService == Program.ServiceEnum.DISCONNECTED ? Properties.Resources.treeErrorNoService : Properties.Resources.treeErrorGeneric;
                }
            }

            foreach (var cstNode in customNodes)
                InsertSpecialNode(cstNode);

            if (!String.IsNullOrEmpty(error))
            {
                Node errorNode = new Node(error);
                errorNode.SelectedImage = errorNode.Image = Icons.Gui.All.Images.CircleCrossFilledRed(16);
                tree.Nodes.Add(errorNode);
            }
            else
            {
                try
                {
                    tree.ImageList = GetImageList(manager);

                    object nodeObj = analyzerTree.GetRootNode();
                    IbaAnalyzer.ISignalTreeNode node = nodeObj as IbaAnalyzer.ISignalTreeNode;
                    if (node != null)
                    {
                        Node trnode = new Node(node.Text, node.ImageIndex);
                        trnode.Tag = node;
                        RecursiveAdd(trnode);
                        //insert the root node (all other nodes are inserted by RecursiveAdd), it should be inserted right after the custom nodes...
                        tree.Nodes.Insert(customNodes.Count, trnode);
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    if (analyzerTree != null)
                    {
                        ((IDisposable)analyzerTree).Dispose();
                    }
                    analyzerTree = null;
                }
            }

            if (!string.IsNullOrEmpty(pendingSelection))
            {
                Node trNode = FindNode(pendingSelection);
                if (trNode != null)
                    tree.SelectedNode = trNode;

                pendingSelection = "";
            }

            tree.AfterSelect += treeView_AfterSelect;
            tree.AfterExpand += treeView_AfterExpand;
            tree.AfterCheck += tree_AfterCheck;

            Interlocked.Exchange(ref iState, Loaded);

            UpdateTree();
        }

        private void TreeSetAnalyzerLoading()
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(TreeSetAnalyzerLoading));
                return;
            }

            ClearNodes();

            Node loadingNode = new Node(Properties.Resources.AnalyzerTree_Loading);
            loadingNode.SelectedImage = loadingNode.Image = Icons.Gui.All.Images.WarningYellowFilled();
            tree.Nodes.Add(loadingNode);

            UpdateTree();
        }

        public bool Load()
        {
            if (Disposing || IsDisposed)
                return true;

            int prevState = Interlocked.CompareExchange(ref iState, Loading, Unloaded);
            if (prevState == Loading || prevState == Loaded)
                return true;
            if (prevState == Unloading)
                return false;

            if (manager.IsOpened)
                CreateTree(null);
            else
            {
                TreeSetAnalyzerLoading();
                Task<string>.Factory.StartNew(OpenAnalyzer).ContinueWith(CreateTree);
            }

            return true;
        }

        private void InsertSpecialNode(CustomNode customNode)
        {
            Node trNode = new Node(customNode.Text);
            trNode.SelectedImage = trNode.Image = customNode.Image;
            trNode.Tag = customNode;
            customTreeNodes.Add(trNode);

            NodeCollection collNodes = tree.Nodes;
            int insertIdx = 0;
            while (insertIdx < collNodes.Count && collNodes[insertIdx].Tag is CustomNode)
                insertIdx++;

            if (insertIdx == collNodes.Count)
                collNodes.Add(trNode);
            else
                collNodes.Insert(insertIdx, trNode);
        }

        public string AddSpecialNode(string text, Image image)
        {
            string key = "CSTM_" + customNodes.Count;

            CustomNode cstNode = new CustomNode() { Text = text, ChannelID = key, Image = image };
            customNodes.Add(cstNode);

            if (iState == Loaded)
                InsertSpecialNode(cstNode);

            return key;
        }

        private void RecursiveAdd(Node trnode, bool addSiblings = true)
        {
            IbaAnalyzer.ISignalTreeNode signalTreeNode = trnode.Tag as IbaAnalyzer.ISignalTreeNode;
            IbaAnalyzer.ISignalTreeNode child = signalTreeNode?.GetFirstChildNode() as IbaAnalyzer.ISignalTreeNode;
            if (child != null)
            {
                Node childtrnode = new Node(child.Text, child.ImageIndex);
                childtrnode.Tag = child;
                trnode.Nodes.Add(childtrnode);
                RecursiveAdd(childtrnode);
            }

            if (!addSiblings)
                return;

            IbaAnalyzer.ISignalTreeNode sibling = signalTreeNode.GetSiblingNode() as IbaAnalyzer.ISignalTreeNode;
            while (sibling != null)
            {
                Node siblingtrnode = new Node(sibling.Text, sibling.ImageIndex);
                siblingtrnode.Tag = sibling;
                NodeCollection parentCollection = (trnode.Parent == null) ? tree.Nodes : trnode.Parent.Nodes;
                parentCollection.Add(siblingtrnode);
                RecursiveAdd(siblingtrnode, false);
                sibling = sibling.GetSiblingNode() as IbaAnalyzer.ISignalTreeNode;
            }
        }

        private void ExpandNode(Node node)
        {
            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "dummy")
            {
                node.Nodes.Clear();

                IbaAnalyzer.ISignalTreeNode parent = node.Tag as IbaAnalyzer.ISignalTreeNode;
                if (parent != null)
                {
                    parent.Expand();

                    RecursiveAdd(node, false);
                }
            }
        }

        private Node FindNode(string id)
        {
            foreach (var trNode in customTreeNodes)
            {
                if ((trNode.Tag as CustomNode).ChannelID == id)
                    return trNode;
            }

            IbaAnalyzer.ISignalTreeNode inode = analyzerTree?.FindNodeWithID(id);
            if (inode == null) return null;
            Stack<IbaAnalyzer.ISignalTreeNode> parents = new Stack<IbaAnalyzer.ISignalTreeNode>();
            Node node = null;
            while (inode != null)
            {
                parents.Push(inode);
                inode = inode.GetParentNode() as IbaAnalyzer.ISignalTreeNode;
            }
            NodeCollection tc = tree.Nodes;
            bool bFirstPop = true;
            while (parents.Count > 0)
            {
                inode = parents.Pop();
                if (tc.Count <= inode.IndexInCollection || inode.IndexInCollection < 0)
                    return null;
                node = bFirstPop && tc.Count > inode.IndexInCollection + customNodes.Count ? tc[inode.IndexInCollection + customNodes.Count] : tc[inode.IndexInCollection];
                bFirstPop = false;
                ExpandNode(node);
                (inode as IDisposable)?.Dispose();
                tc = node.Nodes;
            }
            return node;
        }

        public string GetDisplayName(string id)
        {
            Node trNode = FindNode(id);
            if (trNode == null)
                return null;

            if (trNode.Tag is CustomNode cstNode)
                return cstNode.Text;
            if (trNode.Tag is IbaAnalyzer.ISignalTreeNode sigNode)
                return sigNode.Text;

            return null;
        }

        public bool ContainsNode(string id)
        {
            return FindNode(id) != null;
        }

        public IEnumerable<Node> EnumerateNodes(Crownwood.DotNetMagic.Controls.TreeControl tree)
        {
            NodeCollection nodes = tree.Nodes;
            List<Node> result = new List<Node>();
            if (nodes.Count == 0)
                return null;

            return EnumerateNodes(nodes);


        }

        private IEnumerable<Node> EnumerateNodes(NodeCollection nodes)
        {
            List<Node> result = new List<Node>();
            foreach (Node n in nodes)
            {
                result.Add(n);

                if (n.Nodes.Count > 0)
                {
                    foreach (Node subNode in EnumerateNodes(n.Nodes))
                        result.Add(subNode);
                }
            }
            return result;
        }

        public IEnumerable<Node> EnumerateCheckedNodes(Crownwood.DotNetMagic.Controls.TreeControl tree)
        {
            NodeCollection nodes = tree.Nodes;
            if (nodes.Count == 0)
                return null;

            return EnumerateCheckedNodes(nodes);
        }

        private IEnumerable<Node> EnumerateCheckedNodes(NodeCollection nodes)
        {
            List<Node> result = new List<Node>();
            foreach (Node n in nodes)
            {
                if (n.CheckState == Crownwood.DotNetMagic.Controls.CheckState.Checked)
                    result.Add(n);

                if ((n.CheckState != Crownwood.DotNetMagic.Controls.CheckState.Unchecked) && (n.Nodes.Count > 0))
                {
                    foreach (Node subNode in EnumerateCheckedNodes(n.Nodes))
                        result.Add(subNode);
                }
            }
            return result;
        }

        public Image GetImage(string id)
        {
            Node trNode = FindNode(id);
            if (trNode == null)
                return null;

            if (trNode.Image != null)
                return trNode.Image;

            return trNode.ImageIndex >= 0 && trNode.ImageIndex < imageList.Images.Count ? imageList.Images[trNode.ImageIndex] : null;
        }

        #region Handlers
        private void treeView_AfterExpand(Crownwood.DotNetMagic.Controls.TreeControl tc, Crownwood.DotNetMagic.Controls.NodeEventArgs e)
        {
            ExpandNode(e.Node);
        }

        private void treeView_AfterSelect(Crownwood.DotNetMagic.Controls.TreeControl tc, Crownwood.DotNetMagic.Controls.NodeEventArgs e)
        {
            //Action is only Unknown when setting the selected node in OnBeforeShowPopup
            //If we don't return here, the popup will be shown without any attached event handlers
            //TODO
            //if (e. == TreeViewAction.Unknown)
            //    return;

            string id = "";
            if (e.Node?.Tag is CustomNode cstNode)
                id = cstNode.ChannelID;
            else if (e.Node?.Tag is IbaAnalyzer.ISignalTreeNode sigNode)
                id = sigNode.channelID;

            if (id != "")
                AfterSelect?.Invoke(this, id);
        }

        void treeView_DoubleClick(Crownwood.DotNetMagic.Controls.TreeControl tc, Crownwood.DotNetMagic.Controls.NodeEventArgs e)
        {
            CustomDoubleClick?.Invoke(this, "");
        }
        #endregion

        private void tree_NodeDrag(Crownwood.DotNetMagic.Controls.TreeControl tc, Crownwood.DotNetMagic.Controls.StartDragEventArgs e)
        {
            // Move the dragged node when the left mouse button is used.
            Node node = e.Node;
            if ((node == null) || (node.Tag == null))
                return;

            bool multiSelect = tree.SelectMode == SelectMode.Multiple;

            if (!multiSelect)
            {
                Node draggedNode = e.Node;
                AfterSelect?.Invoke(this, draggedNode.Text);
                SelectedChannel = draggedNode.Text;
                if (!(draggedNode.Tag is NewConfigurationTreeItemDataBase) || draggedNode.Tag == null)
                    DoDragDrop(tree.SelectedNodes, DragDropEffects.Copy | DragDropEffects.Move);
            }
            else
                DoDragDrop(tree.SelectedNodes, DragDropEffects.Copy | DragDropEffects.Move);
        }

        //Deprecated
        private void m_configTreeView_ItemDrag(object sender, ItemDragEventArgs e)
        {
            // Move the dragged node when the left mouse button is used.
            if (e.Button == MouseButtons.Left)
            {
                TreeNode draggedNode = (TreeNode)e.Item;
                AfterSelect?.Invoke(this, draggedNode.Text);
                SelectedChannel = draggedNode.Text;
                if (!(draggedNode.Tag is NewConfigurationTreeItemDataBase) || draggedNode.Tag == null)
                    DoDragDrop(e.Item, DragDropEffects.Copy | DragDropEffects.Move);
            }
        }
    }
    #endregion

    #region RepositoryItemChannelTreeEdit

    class SpecialNode
    {
        public SpecialNode(string displayText, Image image, string value, string channelId)
        {
            DisplayText = displayText;
            Image = image;
            Value = value;
            ChannelId = channelId;
        }

        public string DisplayText;  //Display text in editor and tree
        public Image Image;         //Image in tree
        public string Value;        //Value in editor
        public string ChannelId;    //ID in tree
    }

    [UserRepositoryItem("RegisterChannelTreeEdit")]
    internal class RepositoryItemChannelTreeEdit : RepositoryItemPopupContainerEdit
    {
        #region static

        static RepositoryItemChannelTreeEdit()
        {
            RegisterChannelTreeEdit();
        }

        public static void RegisterChannelTreeEdit()
        {
            EditorRegistrationInfo.Default.Editors.Add(new EditorClassInfo("ChannelTreeEdit",
                typeof(ChannelTreeEdit), typeof(RepositoryItemChannelTreeEdit), typeof(PopupBaseEditViewInfo),
                new ChannelTreeEditPainter(), false, EditImageIndexes.PopupContainerEdit, typeof(DevExpress.Accessibility.PopupEditAccessible)));
        }

        #endregion

        #region instance
        bool bOriginalInstance;
        bool bOwnsManager;
        private AnalyzerTreeControl channelTree;

        public bool DrawImages;

        public RepositoryItemChannelTreeEdit()
        {
            bOriginalInstance = false;
            channelTree = null;
            specialNodes = new List<SpecialNode>();
            TextEditStyle = TextEditStyles.Standard;
            DrawImages = true;

            CloseOnLostFocus = false;
            CloseOnOuterMouseClick = false;

            PopupSizeable = true;
        }

        public RepositoryItemChannelTreeEdit(AnalyzerManager manager, ChannelTreeFilter filter)
           : this()
        {
            if (manager == null)
            {
                manager = new AnalyzerManager();
                bOwnsManager = true;
            }

            channelTree = new AnalyzerTreeControl(manager, filter);
            bOriginalInstance = true;
        }

        public override string EditorTypeName
        {
            get { return "ChannelTreeEdit"; }
        }

        public AnalyzerTreeControl ChannelTree => channelTree;

        public AnalyzerManager AnalyzerManager => channelTree.AnalyzerManager;

        public override void Assign(RepositoryItem item)
        {
            RepositoryItemChannelTreeEdit channelTreeItem = item as RepositoryItemChannelTreeEdit;

            if (channelTreeItem != null)
            {
                channelTree = channelTreeItem.ChannelTree;
                specialNodes = channelTreeItem.specialNodes;
                DrawImages = channelTreeItem.DrawImages;
                specialNodes = channelTreeItem.specialNodes;
            }

            base.Assign(item);
        }

        internal void TransferOwnership(RepositoryItemChannelTreeEdit other)
        {
            other.bOriginalInstance = bOriginalInstance;
            bOriginalInstance = false;

            other.bOwnsManager = bOwnsManager;
            bOwnsManager = false;
        }

        public new ChannelTreeEdit CreateEditor()
        {
            return base.CreateEditor() as ChannelTreeEdit;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                ResetChannelTree();

                var manager = channelTree?.AnalyzerManager;

                if (bOriginalInstance && channelTree != null)
                {
                    channelTree.Dispose();
                    channelTree = null;
                }

                if (bOwnsManager && manager != null)
                {
                    manager.Dispose();
                    manager = null;
                }
            }

            base.Dispose(disposing);
        }

        public void ResetChannelTree()
        {
            if (bOriginalInstance && channelTree != null)
                channelTree.Reset();
        }

        #endregion

        #region specialNodes

        List<SpecialNode> specialNodes;

        public void AddSpecialNode(string value, string displayText, Image image)
        {
            string id = channelTree.AddSpecialNode(displayText, image);
            specialNodes.Add(new SpecialNode(displayText, image, value, id));
        }

        internal SpecialNode GetSpecialNodeFromId(string id)
        {
            return specialNodes.Find(n => n.ChannelId == id);
        }

        internal SpecialNode GetSpecialNodeFromValue(string value)
        {
            return specialNodes.Find(n => n.Value == value);
        }

        internal SpecialNode GetFirstSpecialNode()
        {
            return specialNodes.Count > 0 ? specialNodes[0] : null;
        }

        #endregion
    }

    #endregion

    #region ChannelTreePopup

    internal class ChannelTreePopup : PopupBaseSizeableForm
    {
        public ChannelTreePopup(ChannelTreeEdit edit) : base(edit)
        {
            Font = SystemFonts.DefaultFont;
        }

        public override object ResultValue
        {
            get
            {
                AnalyzerTreeControl tree = Tree;

                if (tree == null || string.IsNullOrEmpty(tree.SelectedChannel))
                    return OwnerEdit.EditValue;  //return previous value because it isn't available in tree now and nothing else was selected

                string selected = tree.SelectedChannel;
                SpecialNode specialNode = ChannelTreeProperties.GetSpecialNodeFromId(selected);
                if (specialNode != null)
                    return specialNode.Value;
                else
                    return selected;
            }
        }

        AnalyzerTreeControl Tree
        {
            get { return ChannelTreeProperties.ChannelTree; }
        }

        RepositoryItemChannelTreeEdit ChannelTreeProperties
        {
            get { return (RepositoryItemChannelTreeEdit)base.Properties; }
        }

        public bool IsLoading => Tree.IsLoading;

        #region popup

        protected override void OnBeforeShowPopup()
        {
            AnalyzerTreeControl tree = Tree;
            if (tree != null)
            {
                while (!tree.Load()) ;

                if (!Controls.Contains(tree))
                {
                    tree.Location = Point.Empty;
                    tree.Dock = DockStyle.None;

                    UpdateBounds();
                    Controls.Add(tree);
                }

                string editValue = OwnerEdit.EditValue as string;
                SpecialNode special = ChannelTreeProperties.GetSpecialNodeFromValue(editValue);
                if (special != null)
                    editValue = special.ChannelId;
                else if ((editValue == null) && string.IsNullOrEmpty(tree.SelectedChannel))
                    editValue = ChannelTreeProperties.GetFirstSpecialNode()?.ChannelId; //Select first special node in case nothing was selected

                tree.SelectedChannel = editValue ?? "";
                tree.AfterSelect += tree_AfterSelect;
                tree.CustomDoubleClick += tree_AfterSelect;
            }

            base.OnBeforeShowPopup();
        }

        public override void HidePopupForm()
        {
            AnalyzerTreeControl tree = Tree;
            if (tree != null)
            {
                tree.AfterSelect -= tree_AfterSelect;
                tree.CustomDoubleClick -= tree_AfterSelect;
            }

            base.HidePopupForm();
        }

        void tree_AfterSelect(AnalyzerTreeControl sender, string id)
        {
            if ((MouseButtons & MouseButtons.Right) == MouseButtons.Right)
                return; //ignore opening of context menu

            ((ChannelTreeEdit)OwnerEdit).EditValue = id;

            if (sender != null && id != null)
                ClosePopup(PopupCloseMode.Normal);
        }
        #endregion

        #region resize

        Size resizedSize = Size.Empty;
        protected override Size CalcFormSizeCore()
        {
            if (resizedSize != Size.Empty)
                return resizedSize;

            int width = Math.Max(250, OwnerEdit.Width);
            return CalcFormSize(new Size(width, 250));
        }

        void DoUpdateBounds()
        {
            AnalyzerTreeControl tree = Tree;
            if (tree != null)
            {
                tree.Location = Point.Empty;
                tree.Dock = DockStyle.None;

                Rectangle r = ClientRectangle;
                SizeGripPosition gp = ViewInfo.GripObjectInfo.GripPosition;

                if (gp == SizeGripPosition.LeftTop || gp == SizeGripPosition.RightTop)
                    r.Y += 25;
                else
                    r.Height -= 25;

                tree.Bounds = r;
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            DoUpdateBounds();
            resizedSize = ClientSize;

            AnalyzerTreeControl tree = Tree;
            if (tree != null)
            {
                var s = Size;
                s.Height -= 20;
                tree.Size = s;
            }
        }

        #endregion
    }

    #endregion

    #region ChannelTreeEdit

    internal class ChannelTreeEdit : PopupBaseEdit
    {
        static ChannelTreeEdit()
        {
            RepositoryItemChannelTreeEdit.RegisterChannelTreeEdit();
        }

        public static ChannelTreeEdit CreateInstance(AnalyzerManager analyzerManager, ChannelTreeFilter filter)
        {
            //Let's create editor
            var repItem = new RepositoryItemChannelTreeEdit(analyzerManager, filter);
            ChannelTreeEdit edit = repItem.CreateEditor();
            edit.Properties.Assign(repItem);

            //Transfer the ownership of the manager and tree to the properties of the newly created editor
            repItem.TransferOwnership(edit.Properties as RepositoryItemChannelTreeEdit);

            return edit;
        }

        public ChannelTreeEdit() : base()
        {
        }

        public override string EditorTypeName
        {
            get { return "ChannelTreeEdit"; }
        }

        protected override PopupBaseForm CreatePopupForm()
        {
            return new ChannelTreePopup(this);
        }

        protected override void DoClosePopup(PopupCloseMode closeMode)
        {
            if (PopupForm is ChannelTreePopup popup && popup.IsLoading)
                return;

            base.DoClosePopup(closeMode);
        }

        internal AnalyzerTreeControl Tree => (Properties as RepositoryItemChannelTreeEdit)?.ChannelTree;
        public IAnalyzerManagerUpdateSource AnalyzerManager => (Properties as RepositoryItemChannelTreeEdit)?.AnalyzerManager;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }
    }

    #endregion

    #region ChannelTreeEditPainter

    internal class ChannelTreeEditPainter : ButtonEditPainter
    {
        protected override void DrawText(ControlGraphicsInfoArgs info)
        {
            PopupBaseEditViewInfo vi = info.ViewInfo as PopupBaseEditViewInfo;

            string id = vi.EditValue as string;

            RepositoryItemChannelTreeEdit item = vi.Item as RepositoryItemChannelTreeEdit;

            if (!string.IsNullOrEmpty(id))
            {
                string text;
                Image image;

                SpecialNode special = item.GetSpecialNodeFromValue(id);
                if (special != null)
                {
                    text = special.DisplayText;
                    image = special.Image;
                }
                else
                {
                    AnalyzerTreeControl tree = item.ChannelTree;
                    if (tree is null)
                        tree = (vi.EditValue as ChannelTreeEdit).Tree;

                    text = tree.GetDisplayName(id) ?? id;
                    image = tree.GetImage(id);
                    /*if (image == null)
                    {
                        SpecialNode unassigned = item.GetSpecialNodeFromValue("*UNASSIGNED*");
                        image = unassigned?.Image;
                    }*/
                }

                Rectangle r = vi.ContentRect;
                Point p = vi.ContentRect.Location;

                if ((image != null) && item.DrawImages)
                {
                    p = new Point(p.X + 1, p.Y + (r.Height - image.Height) / 2);
                    int rightPos = r.Right;
                    r.X = p.X + image.Width + 4;
                    r.Width = rightPos - r.X;
                    info.Graphics.DrawImage(image, p.X, p.Y, image.Width, image.Height);
                }
                vi.PaintAppearance.DrawString(info.Cache, text, r);
            }
        }
    }

    #endregion
}
