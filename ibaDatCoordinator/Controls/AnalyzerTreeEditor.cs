using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using iba.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.Controls
{
    #region AnalyzerTree
    internal class AnalyzerManager : IDisposable
    {
        #region Members
        object lockAnalyzer;
        public IbaAnalyzer.IbaAnalyzer Analyzer { get; private set; }

        bool bFilesOpened;
        public bool IsOpened => bFilesOpened;
        DateTime m_dtLastWriteTime;
        string pdoFile, datFile, datFilePassword;

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
                        UnsafeClose();
                        Marshal.ReleaseComObject(Analyzer);
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

            lock (lockAnalyzer)
            {
                bool bErrorFromAnalyzer = false;

                try
                {
                    if (Analyzer == null)
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                        object o = key.GetValue("");
                        string ibaAnalyzerExe = Path.GetFullPath(o.ToString());

                        if (!VersionCheck.CheckVersion(ibaAnalyzerExe, "7.1.0"))
                            throw new Exception(string.Format(Properties.Resources.logAnalyzerVersionError, "7.1.0"));

                        Analyzer = new IbaAnalyzer.IbaAnalysis(); //TODO eventually replace by NonInteractive when tree images are supported
                    }

                    FileInfo info = new FileInfo(Program.RemoteFileLoader.GetLocalPath(pdoFile));
                    if (!info.Exists  || info.LastWriteTimeUtc != m_dtLastWriteTime)
                        UnsafeClose();

                    if (!bFilesOpened)
                    {
                        if (!Program.RemoteFileLoader.DownloadFile(pdoFile, out string localFile, out error))
                            return false;

                        info = new FileInfo(localFile);
                        DateTime dtLastWriteTime = info.LastWriteTimeUtc;

                        bErrorFromAnalyzer = true;

                        Analyzer.OpenAnalysis(localFile);
                        m_dtLastWriteTime = dtLastWriteTime;

                        if (!string.IsNullOrEmpty(datFile))
                        {
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
                        if (bErrorFromAnalyzer)
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

        public void UpdateSource(string pdoFile, string datFile, string datFilePassword)
        {
            lock (lockAnalyzer)
            {
                this.pdoFile = pdoFile;
                this.datFile = datFile;
                this.datFilePassword = datFilePassword;

                UnsafeClose();
            }

            SourceUpdated?.Invoke();
        }
    }

    [Flags]
    public enum ChannelTreeFilter
    {
        Analog = 0x01,
        Digital = 0x02,
        Text = 0x04,
    };

    internal class AnalyzerTreeControl : Panel
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

        int iState;
        AnalyzerManager manager;
        ChannelTreeFilter filter;

        List<CustomNode> customNodes;
        List<TreeNode> customTreeNodes;

        string pendingSelection;

        ImageList imageList;
        TreeView treeView;
        IbaAnalyzer.ISignalTree analyzerTree;

        public event Action<AnalyzerTreeControl, string> AfterSelect;
        public event Action<AnalyzerTreeControl, string> CustomDoubleClick;

        public bool IsLoading => iState == Loading;
        #endregion

        #region Initialize
        public AnalyzerTreeControl(AnalyzerManager manager, ChannelTreeFilter filter)
        {
            this.manager = manager;
            this.filter = filter;

            iState = Unloaded;

            customNodes = new List<CustomNode>();
            customTreeNodes = new List<TreeNode>();

            pendingSelection = "";

            Padding = new Padding(0);

            imageList = new ImageList();
            imageList.TransparentColor = Color.Magenta;
            treeView = new TreeView();
            treeView.ImageList = imageList;
            treeView.Dock = DockStyle.Fill;
            treeView.Location = Point.Empty;

            Controls.Add(treeView);

            manager.SourceUpdated += Reset;
        }
        #endregion

        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                Reset();

                Controls?.Clear();
                if (treeView != null)
                {
                    treeView.Dispose();
                    treeView = null;
                }

                manager.SourceUpdated -= Reset;
            }

            base.Dispose(disposing);
        }
        #endregion

        public string SelectedChannel
        {
            get
            {
                if (iState != Loaded || treeView.SelectedNode == null)
                    return "";

                if (treeView.SelectedNode.Tag is CustomNode cstNode)
                    return cstNode.ChannelID;

                if (treeView.SelectedNode.Tag is IbaAnalyzer.ISignalTreeNode sigNode)
                    return sigNode.channelId;

                return "";
            }
            set
            {
                if (iState == Loaded)
                {
                    TreeNode node = FindNode(value);
                    if (node != null)
                        treeView.SelectedNode = node;
                }
                else
                    pendingSelection = value;
            }
        }

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

            if (treeView != null)
            {
                treeView.BeforeSelect -= treeView_BeforeSelect;
                treeView.AfterSelect -= treeView_AfterSelect;
                treeView.AfterExpand -= treeView_AfterExpand;
                treeView.DoubleClick -= treeView_DoubleClick;
                ClearNodes();
            }

            if (analyzerTree != null)
                Marshal.ReleaseComObject(analyzerTree);
            analyzerTree = null;
            imageList.Images.Clear();
            customTreeNodes.Clear();
            pendingSelection = "";

            Interlocked.Exchange(ref iState, Unloaded);
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

        unsafe void FillImageList()
        {
            if (manager.Analyzer == null)
                return;
            int cnt = manager.Analyzer.SignalTreeImageCount;
            for (int i = 0; i < cnt; i++)
            {
                object myArrayObject = null;
                manager.Analyzer.SignalTreeImageData(i, out myArrayObject);
                byte[] thearray = myArrayObject as byte[];
                fixed (byte* p = thearray)
                {
                    IntPtr ptr = (IntPtr)p;
                    Bitmap bm = new Bitmap(16, 16, 16 * 4, System.Drawing.Imaging.PixelFormat.Format32bppRgb, ptr);
                    imageList.Images.Add(bm);
                }
            }
        }

        void ReleaseSignalNodes(TreeNodeCollection nodes)
        {
            foreach (TreeNode node in nodes)
            {
                ReleaseSignalNodes(node.Nodes);

                if (node.Tag is IbaAnalyzer.ISignalTreeNode sigNode)
                {
                    node.Tag = null;
                    Marshal.ReleaseComObject(sigNode);
                }
            }
        }

        void ClearNodes()
        {
            ReleaseSignalNodes(treeView.Nodes);
            treeView.Nodes.Clear();
        }

        void CreateTree(Task<string> task)
        {
            if (Disposing || IsDisposed || iState != Loading)
                return;

            if (InvokeRequired)
            {
                BeginInvoke(new Action<Task<string>>(CreateTree), task);
                return;
            }

            imageList.Images.Clear();
            ClearNodes();

            treeView.BeginUpdate();
            string error = "";
            if (task != null && (task.IsFaulted || !string.IsNullOrEmpty(task.Result)))
                error = task.Exception?.Message ?? task.Result ?? Properties.Resources.AnalyzerTree_UnknownError;
            else
            {
                try
                {
                    FillImageList();

                    object treeObj = manager.Analyzer.GetSignalTree((int)filter);
                    analyzerTree = treeObj as IbaAnalyzer.ISignalTree;
                    object nodeObj = analyzerTree.GetRootNode();
                    IbaAnalyzer.ISignalTreeNode node = nodeObj as IbaAnalyzer.ISignalTreeNode;
                    if (node != null)
                    {
                        TreeNode trnode = new TreeNode(node.Text, node.ImageIndex, node.ImageIndex);
                        trnode.Tag = node;
                        treeView.Nodes.Add(trnode);
                        RecursiveAdd(trnode);
                    }
                }
                catch (Exception ex)
                {
                    error = ex.Message;
                    if (analyzerTree != null)
                        Marshal.ReleaseComObject(analyzerTree);
                    analyzerTree = null;
                    imageList.Images.Clear();
                    ClearNodes();
                }
            }

            TreeNode errorNode = null;
            if (!string.IsNullOrEmpty(error))
            {
                int imgIndex = imageList.Images.Count;
                errorNode = new TreeNode(error, imgIndex, imgIndex);
                imageList.Images.Add(Properties.Resources.img_error);
            }

            foreach (var cstNode in customNodes)
                InsertSpecialNode(cstNode);

            if (errorNode != null)
                treeView.Nodes.Add(errorNode);

            treeView.EndUpdate();

            if (!string.IsNullOrEmpty(pendingSelection))
            {
                TreeNode trNode = FindNode(pendingSelection);
                if (trNode != null)
                    treeView.SelectedNode = trNode;

                pendingSelection = "";
            }

            treeView.BeforeSelect += treeView_BeforeSelect;
            treeView.AfterSelect += treeView_AfterSelect;
            treeView.AfterExpand += treeView_AfterExpand;
            treeView.DoubleClick += treeView_DoubleClick;

            Interlocked.Exchange(ref iState, Loaded);
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
                treeView.BeginUpdate();
                imageList.Images.Clear();
                ClearNodes();
                imageList.Images.Add(Properties.Resources.img_warning);
                treeView.Nodes.Add(new TreeNode(Properties.Resources.AnalyzerTree_Loading, 0, 0));
                treeView.EndUpdate();

                
                Task<string>.Factory.StartNew(OpenAnalyzer).ContinueWith(CreateTree);
            }

            return true;
        }

        void InsertSpecialNode(CustomNode customNode)
        {
            int imageIndex = imageList.Images.Count;
            imageList.Images.Add(customNode.Image);

            TreeNode trNode = new TreeNode(customNode.Text, imageIndex, imageIndex);
            trNode.Tag = customNode;
            customTreeNodes.Add(trNode);

            TreeNodeCollection collNodes = treeView.Nodes;
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

        private void RecursiveAdd(TreeNode trnode, bool addSiblings = true)
        {
            IbaAnalyzer.ISignalTreeNode signalTreeNode = trnode.Tag as IbaAnalyzer.ISignalTreeNode;
            IbaAnalyzer.ISignalTreeNode child = signalTreeNode?.GetFirstChildNode() as IbaAnalyzer.ISignalTreeNode;
            if (child != null)
            {
                TreeNode childtrnode = new TreeNode(child.Text, child.ImageIndex, child.ImageIndex);
                childtrnode.Tag = child;
                trnode.Nodes.Add(childtrnode);
                RecursiveAdd(childtrnode);
            }
            if (!addSiblings) return;
            IbaAnalyzer.ISignalTreeNode sibling = signalTreeNode.GetSiblingNode() as IbaAnalyzer.ISignalTreeNode;
            while (sibling != null)
            {
                TreeNode siblingtrnode = new TreeNode(sibling.Text, sibling.ImageIndex, sibling.ImageIndex);
                siblingtrnode.Tag = sibling;
                TreeNodeCollection parentCollection = (trnode.Parent == null) ? treeView.Nodes : trnode.Parent.Nodes;
                parentCollection.Add(siblingtrnode);
                RecursiveAdd(siblingtrnode, false);
                sibling = sibling.GetSiblingNode() as IbaAnalyzer.ISignalTreeNode;
            }
        }

        private void ExpandNode(TreeNode node)
        {
            if (node.Nodes.Count == 1 && node.Nodes[0].Text == "dummy")
            {
                node.Nodes.Clear();
                IbaAnalyzer.ISignalTreeNode parent = node.Tag as IbaAnalyzer.ISignalTreeNode;
                if (parent != null)
                {
                    parent.Expand();
                    treeView.BeginUpdate();
                    RecursiveAdd(node, false);
                    treeView.EndUpdate();
                }
            }
        }

        TreeNode FindNode(string id)
        {
            foreach (var trNode in customTreeNodes)
            {
                if ((trNode.Tag as CustomNode).ChannelID == id)
                    return trNode;
            }

            IbaAnalyzer.ISignalTreeNode inode = analyzerTree?.FindNodeWithID(id);
            if (inode == null) return null;
            Stack<IbaAnalyzer.ISignalTreeNode> parents = new Stack<IbaAnalyzer.ISignalTreeNode>();
            TreeNode node = null;
            while (inode != null)
            {
                parents.Push(inode);
                inode = inode.GetParentNode() as IbaAnalyzer.ISignalTreeNode;
            }
            TreeNodeCollection tc = treeView.Nodes;
            bool bFirstPop = true;
            while (parents.Count > 0)
            {
                inode = parents.Pop();
                node = bFirstPop ? tc[inode.IndexInCollection + customNodes.Count] : tc[inode.IndexInCollection];
                bFirstPop = false;
                ExpandNode(node);
                Marshal.ReleaseComObject(inode);
                tc = node.Nodes;
            }
            return node;
        }

        public string GetDisplayName(string id)
        {
            TreeNode trNode = FindNode(id);
            if (trNode == null)
                return null;

            if (trNode.Tag is CustomNode cstNode)
                return cstNode.Text;
            if (trNode.Tag is IbaAnalyzer.ISignalTreeNode sigNode)
                return sigNode.Text;

            return null;
        }

        public Image GetImage(string id)
        {
            TreeNode trNode = FindNode(id);
            if (trNode == null)
                return null;

            return trNode.ImageIndex > 0 && trNode.ImageIndex < imageList.Images.Count ? imageList.Images[trNode.ImageIndex] : null;
        }

        #region Handlers
        private void treeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            ExpandNode(e.Node);
        }

        private void treeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (e.Node?.Tag == null)
                e.Cancel = true;
        }

        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            //Action is only Unknown when setting the selected node in OnBeforeShowPopup
            //If we don't return here, the popup will be shown without any attached event handlers
            if (e.Action == TreeViewAction.Unknown)
                return;

            string id = "";
            if (e.Node?.Tag is CustomNode cstNode)
                id = cstNode.ChannelID;
            else if (e.Node?.Tag is IbaAnalyzer.ISignalTreeNode sigNode)
                id = sigNode.channelId;

            if (id != "")
                AfterSelect?.Invoke(this, id);
        }

        void treeView_DoubleClick(object sender, EventArgs e)
        {
            CustomDoubleClick?.Invoke(this, "");
        }
        #endregion
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
        AnalyzerTreeControl channelTree;
        public AnalyzerTreeControl ChannelTree => channelTree;
        public bool DrawImages;

        public RepositoryItemChannelTreeEdit()
        {
            bOriginalInstance = false;
            channelTree = null;
            specialNodes = new List<SpecialNode>();
            TextEditStyle = TextEditStyles.DisableTextEditor;
            DrawImages = true;

            CloseOnLostFocus = false;
            CloseOnOuterMouseClick = false;

            PopupSizeable = true;
        }

        public RepositoryItemChannelTreeEdit(AnalyzerManager manager, ChannelTreeFilter filter)
           :this()
        {
            channelTree = new AnalyzerTreeControl(manager, filter);
            bOriginalInstance = true;
        }

        public override string EditorTypeName
        {
            get { return "ChannelTreeEdit"; }
        }

        public override void Assign(RepositoryItem item)
        {
            RepositoryItemChannelTreeEdit channelTreeItem = item as RepositoryItemChannelTreeEdit;

            if (channelTreeItem != null)
            {
                channelTree = channelTreeItem.ChannelTree;
                specialNodes = channelTreeItem.specialNodes;
                DrawImages = channelTreeItem.DrawImages;
            }

            base.Assign(item);
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

                if (bOriginalInstance && channelTree != null)
                {
                    channelTree.Dispose();
                    channelTree = null;
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
                AnalyzerTreeControl tree = ChannelTreeProperties.ChannelTree;

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

        RepositoryItemChannelTreeEdit ChannelTreeProperties
        {
            get { return (RepositoryItemChannelTreeEdit)base.Properties; }
        }

        public bool IsLoading => ChannelTreeProperties.ChannelTree.IsLoading;

        #region popup

        protected override void OnBeforeShowPopup()
        {
            AnalyzerTreeControl tree = ChannelTreeProperties.ChannelTree;

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
            AnalyzerTreeControl tree = ChannelTreeProperties.ChannelTree;
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
            AnalyzerTreeControl tree = ChannelTreeProperties.ChannelTree;
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
        }

        #endregion
    }

    #endregion

    #region ChannelTreeEdit

    public class ChannelTreeEdit : PopupBaseEdit
    {
        static ChannelTreeEdit()
        {
            RepositoryItemChannelTreeEdit.RegisterChannelTreeEdit();
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
                    text = tree.GetDisplayName(id) ?? id;
                    image = tree.GetImage(id);
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
