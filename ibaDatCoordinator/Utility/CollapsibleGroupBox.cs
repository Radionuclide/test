using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Collections.Generic;
using System.Linq;

namespace iba.Utility
{
    /// <summary>
    /// GroupBox control that provides functionality to 
    /// allow it to be collapsed.
    /// </summary>
    [ToolboxBitmap(typeof(CollapsibleGroupBox))]
    public partial class CollapsibleGroupBox : GroupBox, IVariableHeightElement
    {
        public delegate void CollapseBoxClickedEventHandler(object sender);

        #region Fields

        private Rectangle m_toggleRect = new Rectangle(8, 2, 11, 11);
        private Boolean m_collapsed = false;
        private Boolean m_bResizingFromCollapse = false;
        private int m_origHeight;
        private const int m_collapsedHeight = 20;
        private Size m_FullSize = Size.Empty;

        #endregion

        #region Events & Delegates

        /// <summary>Fired when the Collapse Toggle button is pressed</summary>
        public event CollapseBoxClickedEventHandler CollapseBoxClickedEvent;

        #endregion

        #region Constructor

        public CollapsibleGroupBox()
        {
            InitializeComponent();
        }

        #endregion

        public void Init()
        {
            m_bResizable = Anchor.HasFlag(AnchorStyles.Bottom | AnchorStyles.Top);
            m_origHeight = Height;
            if(m_bResizable) MinimumSize = Size; //should already be the case;
            VisibleControls = new List<Control>();
            foreach (Control c in Controls)
            {
                if (c.Visible)
                    VisibleControls.Add(c);
            }

        }
        private List<Control> VisibleControls;

        #region Public Properties

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int FullHeight
        {
            get { return m_FullSize.Height; }
        }

        private bool m_bResizable;

        [DefaultValue(false), Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool IsCollapsed
        {
            get { return m_collapsed; }
            set
            {
                if(value != m_collapsed)
                {
                    m_collapsed = value;

                    if (!value)
                    {
                        // Expand
                        if (m_bResizable)
                        {
                            this.Anchor |= AnchorStyles.Top | AnchorStyles.Bottom;
                            if (AnchorChanged != null)
                                AnchorChanged(this, EventArgs.Empty);
                            this.MinimumSize = new Size(0, m_origHeight);
                        }
                        else
                        {
                            this.Size = m_FullSize;
                        }
                    }
                    else
                    {
                        // Collapse
                        if (m_bResizable)
                        {
                            this.Anchor |= AnchorStyles.Top;
                            this.Anchor &= ~AnchorStyles.Bottom;
                            if (AnchorChanged != null)
                                AnchorChanged(this, EventArgs.Empty);
                            this.MinimumSize = new Size(0, CollapsedHeight);
                        }
                        m_bResizingFromCollapse = true;
                        this.Height = m_collapsedHeight;
                        m_bResizingFromCollapse = false;
                    }
                    if(HeightChanged != null)
                        HeightChanged(this, EventArgs.Empty);


                    foreach(Control c in VisibleControls)
                        c.Visible = !value;
                    Invalidate();
                }
            }
        }

        

        [Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public int CollapsedHeight
        {
            get { return m_collapsedHeight; }
        }

        #endregion

        #region Overrides

        protected override void OnMouseUp(MouseEventArgs e)
        {
            if(m_toggleRect.Contains(e.Location))
                ToggleCollapsed();
            else
                base.OnMouseUp(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            HandleResize();
            DrawGroupBox(e.Graphics);
            DrawToggleButton(e.Graphics);
        }

        #endregion

        #region Implimentation

        void DrawGroupBox(Graphics g)
        {
            // Get windows to draw the GroupBox
            Rectangle bounds = new Rectangle(ClientRectangle.X, ClientRectangle.Y + 6, ClientRectangle.Width, ClientRectangle.Height - 6);
            GroupBoxRenderer.DrawGroupBox(g, bounds, Enabled ? GroupBoxState.Normal : GroupBoxState.Disabled);

            // Text Formating positioning & Size
            StringFormat sf = new StringFormat();
            int i_textPos = (bounds.X + 8) + m_toggleRect.Width + 2;
            int i_textSize = (int)g.MeasureString(Text, this.Font).Width;
            i_textSize = i_textSize < 1 ? 1 : i_textSize;
            int i_endPos = i_textPos + i_textSize + 1;

            // Draw a line to cover the GroupBox border where the text will sit
            g.DrawLine(SystemPens.Control, i_textPos, bounds.Y, i_endPos, bounds.Y);

            // Draw the GroupBox text
            using(SolidBrush drawBrush = new SolidBrush(Color.FromArgb(0, 70, 213)))
                g.DrawString(Text, this.Font, drawBrush, i_textPos, 0);
        }

        void DrawToggleButton(Graphics g)
        {
            if(IsCollapsed)
                g.DrawImage(Properties.Resources.plus, m_toggleRect);
            else
                g.DrawImage(Properties.Resources.minus, m_toggleRect);
        }

        void ToggleCollapsed()
        {
            IsCollapsed = !IsCollapsed;

            if(CollapseBoxClickedEvent != null)
                CollapseBoxClickedEvent(this);
        }

        void HandleResize()
        {
            if(!m_bResizingFromCollapse && !m_collapsed)
                m_FullSize = this.Size;
        }

        #endregion

        public Control MainControl
        {
            get { return this; }
        }


        public int PrevHeight
        {
            get { return IsCollapsed?FullHeight:CollapsedHeight; }
        }

        public int PrevMinHeight
        {
            get
            {
                if (m_bResizable)
                    return IsCollapsed ? m_origHeight : CollapsedHeight;
                else 
                    return PrevHeight;
            }

        }

        public event EventHandler AnchorChanged;
        public event EventHandler HeightChanged;
    }
}
