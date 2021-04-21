using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using System.Text;
using System.Runtime.InteropServices;

namespace iba.Controls
{
    #region ImageComboBox
    
    public class ImageComboBox : System.Windows.Forms.ComboBox
    {
        public ImageComboBox()
        {
            editBox = new ComboEditWindow();
            this.DrawMode = DrawMode.OwnerDrawFixed;
            defaultImageIndex = -1;
        }

        #region Variables

        private ImageList imgList; // the images to be used in the imagecombobox are stored in an imagelist.
        private ComboEditWindow editBox; // the NativeWindow object, used to access and repaint the TextBox.
        private int defaultImageIndex;

        #endregion Variables

        #region Properties

        /// <summary>
        /// The imagelist holds the images displayed with the items in the combobox.
        /// </summary>
        public ImageList ImageList
        {
            get
            {
                return imgList;
            }
            set
            {
                imgList = value;
                if(imgList != null)
                    ItemHeight = imgList.ImageSize.Height;
            }

        }

        public int SelectedImageIndex
        {
            get 
            {
                ImageComboBoxItem selectedItem = null;
                foreach(object item in Items)
                {
                    if(item.ToString() == Text)
                    {
                        selectedItem = item as ImageComboBoxItem;
                        break;
                    }
                }

                if(selectedItem != null)
                    return selectedItem.ImageIndex;
                else
                    return defaultImageIndex;
            }
        }

        public int DefaultImageIndex
        {
            get { return defaultImageIndex; }
            set { defaultImageIndex = value; }
        }

        #endregion

        #region Overrided Methods

        /// <summary>
        /// Once the handle of the ImageComboBox is available, we can release NativeWindow's 
        /// own handle and assign the TextBox'es handle to it.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if((this.DropDownStyle == ComboBoxStyle.DropDown) || (this.DropDownStyle == ComboBoxStyle.Simple))
                editBox.AssignTextBoxHandle(this);
        }

        /// <summary>
        /// The TextBox need to be updated when combobox'es selection changes.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnSelectedIndexChanged(EventArgs e)
        {
            base.OnSelectedIndexChanged(e);

            editBox.UpdateImage();
        }

        /// <summary>
        /// when the imagecombobox drawmode is ownerdraw variable, 
        /// each item's height and width need to be measured, inorder to display them properly.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnMeasureItem(MeasureItemEventArgs e)
        {
            base.OnMeasureItem(e);

            if(e.Index >= 0 && this.Items.Count > 0 && e.Index < this.Items.Count)
            {

                SizeF textSize = e.Graphics.MeasureString(this.Items[e.Index].ToString(), Font);
                e.ItemHeight = (int)textSize.Height;
                e.ItemWidth = (int)textSize.Width;
            }
        }

        /// <summary>
        /// Because the combobox is ownerdrawn we have draw each item along with the associated image.
        /// In the case of datasource the displaymember and imagemember are taken from the datasource.
        /// If datasource is not set the items in the Items collection are drawn.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            base.OnDrawItem(e);

            if(e.Index >= 0 && Items.Count > 0 && e.Index < Items.Count)
                DrawItemHelperForItems(e);
        }


        const int gap = 2;

        /// <summary>
        /// Helper method for drawing items .
        /// </summary>
        /// <param name="e"></param>
        private void DrawItemHelperForItems(DrawItemEventArgs e)
        {
            ImageComboBoxItem item = this.Items[e.Index] as ImageComboBoxItem;
            if(item == null)
                return;

            int IconWidth = ImageList == null ? 0 : ImageList.ImageSize.Width;
            
            if(!e.State.HasFlag(DrawItemState.NoFocusRect))
                e.DrawFocusRectangle();

            bool bSelected = e.State.HasFlag(DrawItemState.Selected) && !e.State.HasFlag(DrawItemState.ComboBoxEdit);
            
            //Draw background of image
            Rectangle imageRect = new Rectangle(e.Bounds.X, e.Bounds.Y, IconWidth + gap, e.Bounds.Height);
            e.Graphics.FillRectangle(SystemBrushes.Window, imageRect);
            
            //Draw background of text
            Rectangle itemRect = new Rectangle(e.Bounds.X + IconWidth + gap, e.Bounds.Y, e.Bounds.Width - IconWidth - gap, e.Bounds.Height);
            e.Graphics.FillRectangle(bSelected ? SystemBrushes.Highlight : SystemBrushes.Window, itemRect);

            //Draw image
            if(item.ImageIndex != -1)
                ImageList.Draw(e.Graphics, e.Bounds.X, e.Bounds.Y, item.ImageIndex);
            
            //Draw text
            StringFormat format = new StringFormat();
            format.FormatFlags = StringFormatFlags.NoClip | StringFormatFlags.NoWrap;
            format.Alignment = StringAlignment.Near;
            format.Trimming = StringTrimming.None;
            e.Graphics.DrawString(item.Text, Font, bSelected ? SystemBrushes.HighlightText : SystemBrushes.WindowText, itemRect, format);

            //For some reason the drawstring causes artefacts in the last line in case we have drawn in the textbox -> clear it again
            if(e.State.HasFlag(DrawItemState.ComboBoxEdit))
                e.Graphics.FillRectangle(SystemBrushes.Window, itemRect.X, e.Bounds.Bottom - 1, itemRect.Width, 1);
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);

            editBox.UpdateImage();
        }

        #endregion Overrided Methods

        #region ComboEditWindow
        /// <summary>
        /// The ComboEditWindow is a helper class, to get access to the windows message 
        /// stream directed towards the Edit portion of the ComboBox. This class gets 
        /// assigned the handle of the TextBox of the ComboBox.
        /// </summary>
        class ComboEditWindow : System.Windows.Forms.NativeWindow
        {
            [StructLayout(LayoutKind.Sequential)]
            private struct RECT
            {
                public int left;
                public int top;
                public int right;
                public int bottom;
            }

            [DllImport("user32")]
            private static extern bool GetComboBoxInfo(IntPtr hwndCombo, ref ComboBoxInfo info);

            [StructLayout(LayoutKind.Sequential)]
            private struct ComboBoxInfo
            {
                public int cbSize;
                public RECT rcItem;
                public RECT rcButton;
                public IntPtr stateButton;
                public IntPtr hwndCombo;
                public IntPtr hwndEdit;
                public IntPtr hwndList;
            }
            [DllImport("user32", CharSet = CharSet.Auto)]
            private extern static int SendMessage(
                IntPtr hwnd,
                int wMsg,
                int wParam,
                int lParam);

            private const int EC_LEFTMARGIN = 0x1;
            private const int EC_RIGHTMARGIN = 0x2;
            private const int WM_PAINT = 0xF;
            private const int WM_SETCURSOR = 0x20;
            private const int WM_MOUSEMOVE = 0x200;
            private const int WM_LBUTTONDOWN = 0x201;
            private const int WM_LBUTTONUP = 0x202;
            private const int WM_KEYDOWN = 0x100;
            private const int WM_KEYUP = 0x101;
            private const int WM_CHAR = 0x102;
            private const int WM_GETTEXTLENGTH = 0xe;
            private const int WM_GETTEXT = 0xd;
            private const int EM_SETMARGINS = 0xD3;

            private ImageComboBox Owner = null;

            public ComboEditWindow()
            {
            }

            /// <summary>
            /// The native window's original handle is released 
            /// and the handle of the TextBox is assigned to it.
            /// </summary>
            public void AssignTextBoxHandle(ImageComboBox owner)
            {
                Owner = owner;

                ComboBoxInfo cbxinfo = new ComboBoxInfo();
                cbxinfo.cbSize = Marshal.SizeOf(cbxinfo);
                GetComboBoxInfo(Owner.Handle, ref cbxinfo);

                if(!this.Handle.Equals(IntPtr.Zero))
                    this.ReleaseHandle();

                this.AssignHandle(cbxinfo.hwndEdit);
            }

            public void UpdateImage()
            {
                if((Owner == null) || (Owner.ImageList == null))
                    return;

                int margin = 0;
                int imageIndex = Owner.SelectedImageIndex;
                //if(imageIndex > -1)
                margin = Owner.ImageList.ImageSize.Width + 4;

                // To set the left margin, the lparam's loword is taken as the margin.
                SendMessage(this.Handle, EM_SETMARGINS, EC_LEFTMARGIN, margin);
                SendMessage(this.Handle, EM_SETMARGINS, EC_RIGHTMARGIN, 0);
            }

            /// <summary>
            /// Whenever the textbox is repainted, we have to draw the image.
            /// </summary>
            public void DrawImage()
            {
                if((Owner == null) || (Owner.ImageList == null))
                    return;

                // Gets a GDI drawing surface from the textbox.
                using(Graphics gfx = Graphics.FromHwnd(this.Handle))
                {
                    gfx.FillRectangle(SystemBrushes.Window, new Rectangle(0, 0, Owner.ImageList.ImageSize.Width, Owner.Height));

                    int imageIndex = Owner.SelectedImageIndex;
                    if(imageIndex > -1)
                        Owner.ImageList.Draw(gfx, 0, -1, imageIndex);
                }
            }

            // Override the WndProc method so that we can redraw the TextBox when the textbox is repainted.
            protected override void WndProc(ref Message m)
            {
                base.WndProc(ref m);

                switch(m.Msg)
                {
                    case WM_PAINT:
                    case WM_LBUTTONDOWN:
                    case WM_LBUTTONUP:
                    case WM_KEYDOWN:
                    case WM_KEYUP:
                    case WM_CHAR:
                        DrawImage();
                        break;

                    case WM_MOUSEMOVE:
                        if(Control.MouseButtons != MouseButtons.None)
                            DrawImage();
                        break;
                }
            }
        }

        #endregion NativeWindow for EditBox

    }

    #endregion

    #region ImageComboBoxItem
    
    public class ImageComboBoxItem
    {
        public ImageComboBoxItem()
        {

        }

        public ImageComboBoxItem(string text)
            : this(text, -1, null)
        {
        }

        public ImageComboBoxItem(string text, int imageIndex)
            : this(text, imageIndex, null)
        {
        }

        public ImageComboBoxItem(string text, int imageIndex, object tag)
        {
            this.text = text;
            this.imageIndex = imageIndex;
            this.tag = tag;
        }

        private int imageIndex;
        private string text;
        private object tag;

        public string Text
        {
            get
            {
                return text;

            }
            set
            {
                text = value;
            }
        }

        public int ImageIndex
        {
            get
            {
                return imageIndex;
            }
            set
            {
                imageIndex = value;
            }
        }

        public object Tag
        {
            get
            {
                return tag;
            }
            set
            {
                tag = value;
            }
        }

        public override string ToString()
        {
            return Text;
        }

        public object Clone()
        {
            return new ImageComboBoxItem(Text, ImageIndex, Tag);
        }
    }

    #endregion

}
