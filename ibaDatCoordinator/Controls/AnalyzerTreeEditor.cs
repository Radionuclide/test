using DevExpress.Utils.Drawing;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Controls;
using DevExpress.XtraEditors.Drawing;
using DevExpress.XtraEditors.Popup;
using DevExpress.XtraEditors.Registrator;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraEditors.ViewInfo;
using iba.Data;
using iba.Utility;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
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
        string pdoFile, datFile, datFilePassword;

        public event Action SourceUpdated;
        #endregion

        #region Initialize
        public AnalyzerManager()
        {
            lockAnalyzer = new object();
            Analyzer = null;

            bFilesOpened = false;
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
                try
                {
                    if (Analyzer == null)
                    {
                        RegistryKey key = Registry.LocalMachine.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\ibaAnalyzer.exe", false);
                        object o = key.GetValue("");
                        string ibaAnalyzerExe = Path.GetFullPath(o.ToString());

                        if (!VersionCheck.CheckVersion(ibaAnalyzerExe, "7.1.0"))
                            throw new Exception(string.Format(Properties.Resources.logAnalyzerVersionError, "7.1.0"));

                        Analyzer = new IbaAnalyzer.IbaAnalysis();
                    }
                    
                    if (!bFilesOpened)
                    {
                        Analyzer.OpenAnalysis(pdoFile);
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

    internal class AnalyzerChannelTree : IDisposable
    {
        [Flags]
        public enum ChannelTreeFilter
        {
            Analog = 0x01,
            Digital = 0x02,
            Text = 0x04,
        };

        private class CustomNode
        {
            public string Text;
            public string ChannelID;
            public bool Selectable;
        }

        #region Members
        ImageList imageList;
        TreeView treeView;
        IbaAnalyzer.ISignalTree analyzerTree;
        List<TreeNode> customNodes;
        #endregion

        #region Properties
        public event Action<AnalyzerChannelTree, string> AfterSelect;
        public event Action<AnalyzerChannelTree, string> DoubleClick;

        public Control Control => treeView;

        public string SelectedChannel
        {
            get
            {
                if (treeView.SelectedNode == null)
                    return "";

                if (treeView.SelectedNode.Tag is CustomNode cstNode)
                    return cstNode.ChannelID;

                if (treeView.SelectedNode.Tag is IbaAnalyzer.ISignalTreeNode sigNode)
                    return sigNode.channelId;

                return "";
            }
            set
            {                
                TreeNode node = FindNode(value);
                if (node != null)
                    treeView.SelectedNode = node;
            }
        }

        #endregion

        #region Initialize
        public AnalyzerChannelTree()
        {
            imageList = new ImageList();
            customNodes = new List<TreeNode>();
            treeView = new TreeView();
            treeView.ImageList = imageList;
            treeView.AfterExpand += treeView_AfterExpand;
            treeView.BeforeSelect += treeView_BeforeSelect;
            treeView.AfterSelect += treeView_AfterSelect;
            treeView.DoubleClick += treeView_DoubleClick;
        }

        public AnalyzerChannelTree(AnalyzerManager manager, ChannelTreeFilter filter)
            :this()
        {
            bool bAnalyzerOpened = false;
            try
            {
                if (!manager.OpenAnalyzer(out string error))
                    throw new Exception(error);

                bAnalyzerOpened = true;

                FillImageList(manager);

                treeView.BeginUpdate();
                treeView.Nodes.Clear();
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
                treeView.EndUpdate();
            }
            catch (Exception ex)
            {
                string error = null;
                try
                {
                    if (bAnalyzerOpened)
                        error = manager.Analyzer.GetLastError();
                }
                catch
                { }

                try
                {
                    Dispose();
                }
                catch
                { }

                throw string.IsNullOrEmpty(error) ? ex : new Exception(error);
            }
        }

        unsafe void FillImageList(AnalyzerManager manager)
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
            imageList.TransparentColor = Color.Magenta;
        }
        #endregion

        #region Dispose
        public void Dispose()
        {
            if (treeView != null)
            {
                treeView.Dispose();
                treeView = null;
            }
        }
        #endregion

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
            foreach (var trNode in customNodes)
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

        public string AddSpecialNode(string text, Image image, bool selectable)
        {
            string key = "CSTM_" + customNodes.Count;

            int imageIndex = imageList.Images.Count;
            imageList.Images.Add(image);

            CustomNode cstNode = new CustomNode() { Text = text, ChannelID = key, Selectable = selectable };

            TreeNode trNode = new TreeNode(text, imageIndex, imageIndex);
            trNode.Tag = cstNode;
            customNodes.Add(trNode);

            TreeNodeCollection collNodes = treeView.Nodes;
            int insertIdx = 0;
            while (insertIdx < collNodes.Count && collNodes[insertIdx].Tag is CustomNode)
                insertIdx++;

            if (insertIdx == collNodes.Count)
                collNodes.Add(trNode);
            else
                collNodes.Insert(insertIdx, trNode);

            return key;
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
            if (e.Node?.Tag is CustomNode cstNode && !cstNode.Selectable)
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
            DoubleClick?.Invoke(this, "");
        }
        #endregion
    }

    #endregion

    #region RepositoryItemChannelTreeEdit

    class SpecialNode
    {
        public SpecialNode(string displayText, Image image, string value, string channelId, bool selectable)
        {
            DisplayText = displayText;
            Image = image;
            Value = value;
            ChannelId = channelId;
            Selectable = selectable;
        }

        public string DisplayText;  //Display text in editor and tree
        public Image Image;         //Image in tree
        public string Value;        //Value in editor
        public string ChannelId;    //ID in tree
        public bool Selectable;     //Can be selected in tree
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
        AnalyzerManager manager;
        AnalyzerChannelTree channelTree;
        public bool DrawImages;
        public AnalyzerChannelTree.ChannelTreeFilter Filter;

        public RepositoryItemChannelTreeEdit()
        {
            manager = null;
            channelTree = null;
            specialNodes = new List<SpecialNode>();
            TextEditStyle = TextEditStyles.DisableTextEditor;
            DrawImages = true;

            CloseOnLostFocus = false;
            CloseOnOuterMouseClick = false;

            PopupSizeable = true;

            Filter = AnalyzerChannelTree.ChannelTreeFilter.Analog | AnalyzerChannelTree.ChannelTreeFilter.Digital | AnalyzerChannelTree.ChannelTreeFilter.Text;
        }

        public RepositoryItemChannelTreeEdit(AnalyzerManager manager)
           :this()
        {
            this.manager = manager;
            manager.SourceUpdated += DisposeChannelTree;
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
                channelTree = channelTreeItem.GetOrCreateChannelTree(); //TODO Create async
                specialNodes = channelTreeItem.specialNodes;
                DrawImages = channelTreeItem.DrawImages;
                Filter = channelTreeItem.Filter;
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
                DisposeChannelTree();
                if (manager != null)
                    manager.SourceUpdated -= DisposeChannelTree;
            }

            base.Dispose(disposing);
        }

        public void DisposeChannelTree()
        {
            if (manager != null && channelTree != null) //Only for the original instance is manager != null 
            {
                channelTree.Dispose();
                channelTree = null;
            }
        }

        public AnalyzerChannelTree GetChannelTree()
        {
            return channelTree;
        }

        public AnalyzerChannelTree GetOrCreateChannelTree()
        {
            if (channelTree != null)
                return channelTree;

            AnalyzerChannelTree newTree = null;
            string error = "";
            try
            {
                newTree = new AnalyzerChannelTree(manager, Filter);  //Only gets called on the original repository, thus the manager won't be null
            }
            catch (Exception ex)
            {
                error = ex.Message;
            }

            if (newTree == null)
                newTree = new AnalyzerChannelTree();

            channelTree = newTree;

            // Save externally added special nodes before clearing
            List<SpecialNode> selectableNodes = new List<SpecialNode>();
            foreach (var specialNode in specialNodes)
            {
                if (specialNode.Selectable)
                    selectableNodes.Add(specialNode);
            }

            specialNodes.Clear();

            foreach (var specialNode in selectableNodes)
                AddSpecialNode(specialNode.Value, specialNode.DisplayText, specialNode.Image);

            if (!string.IsNullOrEmpty(error))
                AddSpecialNode(error, error, iba.Properties.Resources.img_error, false);

            return channelTree;
        }

        #endregion

        #region specialNodes

        List<SpecialNode> specialNodes;

        void AddSpecialNode(string value, string displayText, Image image, bool selectable)
        {
            string id = channelTree?.AddSpecialNode(displayText, image, selectable) ?? "";
            specialNodes.Add(new SpecialNode(displayText, image, value, id, selectable));
        }

        public void AddSpecialNode(string value, string displayText, Image image)
        {
            AddSpecialNode(value, displayText, image, true);
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
                AnalyzerChannelTree tree = ChannelTreeProperties.GetChannelTree();

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

        #region popup

        protected override void OnBeforeShowPopup()
        {
            AnalyzerChannelTree tree = ChannelTreeProperties.GetChannelTree();

            if (tree != null)
            {
                if (!Controls.Contains(tree.Control))
                {
                    Controls.Clear();

                    tree.Control.Location = Point.Empty;
                    tree.Control.Dock = DockStyle.None;

                    UpdateBounds();
                    OnResize(EventArgs.Empty); //UpdateBounds doesn't always fire the Resize event so force it here
                    Controls.Add(tree.Control);
                }

                string editValue = OwnerEdit.EditValue as string;
                SpecialNode special = ChannelTreeProperties.GetSpecialNodeFromValue(editValue);
                if (special != null)
                    editValue = special.ChannelId;
                else if ((editValue == null) && string.IsNullOrEmpty(tree.SelectedChannel))
                    editValue = ChannelTreeProperties.GetFirstSpecialNode()?.ChannelId; //Select first special node in case nothing was selected

                tree.SelectedChannel = editValue ?? "";
                tree.AfterSelect += tree_AfterSelect;
                tree.DoubleClick += tree_AfterSelect;
            }

            base.OnBeforeShowPopup();
        }

        public override void HidePopupForm()
        {
            AnalyzerChannelTree tree = ChannelTreeProperties.GetChannelTree();
            if (tree != null)
            {
                tree.AfterSelect -= tree_AfterSelect;
                tree.DoubleClick -= tree_AfterSelect;
            }

            base.HidePopupForm();
        }

        void tree_AfterSelect(AnalyzerChannelTree sender, string id)
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
            AnalyzerChannelTree tree = ChannelTreeProperties.GetChannelTree();
            if (tree != null)
            {
                tree.Control.Location = Point.Empty;
                tree.Control.Dock = DockStyle.None;

                Rectangle r = ClientRectangle;
                SizeGripPosition gp = ViewInfo.GripObjectInfo.GripPosition;

                if (gp == SizeGripPosition.LeftTop || gp == SizeGripPosition.RightTop)
                    r.Y += 25;
                else
                    r.Height -= 25;

                tree.Control.Bounds = r;
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
                    AnalyzerChannelTree tree = item.GetChannelTree();
                    text = tree?.GetDisplayName(id) ?? id;
                    image = tree?.GetImage(id);
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
