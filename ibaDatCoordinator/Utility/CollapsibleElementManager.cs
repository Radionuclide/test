using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;

namespace iba.Utility
{
    public interface IVariableHeightElement
    {
        int PrevHeight
        {
            get;
        }

        int PrevMinHeight
        {
            get;
        }

        Control MainControl
        {
            get;
        }

        event EventHandler AnchorChanged; 
        event EventHandler HeightChanged;
    }


    public class NonVariableElement : IVariableHeightElement
    {
        private Control m_control;
        public NonVariableElement(Control control)
        {
            m_control = control;
        }

        public int  PrevHeight
        {
	        get { return m_control.Height; }
        }

        public int PrevMinHeight
        {
            get { return m_control.Height; }
        }

        public Control  MainControl
        {
	        get { return m_control;}
        }

        public event EventHandler AnchorChanged
        {
            add {}
            remove {}   
        }
        public event EventHandler HeightChanged
        {
            add {}
            remove {}   
        }
    }

    abstract public class CollapsibleElementManagerBase
    {
        protected List<IVariableHeightElement> m_elements;
        protected IVariableHeightElement m_resizableElement;
        protected int m_resizableElementIndex;
        protected Control m_control;


        public int Count
        {
            get { return m_elements.Count; }
        }

        public CollapsibleElementManagerBase(Control control)
        {
            m_control = control;
            m_elements = new List<IVariableHeightElement>();
            m_resizableElement = null;
        }

        public virtual void AddElement(IVariableHeightElement element)
        {
            if(element.MainControl.Anchor.HasFlag(AnchorStyles.Top) && element.MainControl.Anchor.HasFlag(AnchorStyles.Bottom))
            {
                if(m_resizableElement != null) throw new ArgumentException("Maximum one resizable element allowed");
                m_resizableElementIndex = m_elements.Count;
                m_resizableElement = element;
            }
            else if (element.MainControl.Anchor.HasFlag(AnchorStyles.Top) && m_resizableElement != null)
            {
                throw new ArgumentException("elements following the resizable element can only have bottom anchor");
            }
            else if (element.MainControl.Anchor.HasFlag(AnchorStyles.Bottom) && m_resizableElement == null)
            {
                throw new ArgumentException("elements before the resizable element can only have top anchor");
            }
            element.AnchorChanged += new EventHandler(element_AnchorChanged);
            element.HeightChanged += new EventHandler(element_HeightChanged);
            m_elements.Add(element);
        }


        protected virtual void element_AnchorChanged(object sender, EventArgs e)
        {
            if(sender == m_resizableElement)
            {
                m_resizableElement = null;
                for(int index = m_resizableElementIndex + 1; index < m_elements.Count; index++)
                { //make all element anchoring top
                    m_elements[index].MainControl.Anchor &= ~AnchorStyles.Bottom;
                    m_elements[index].MainControl.Anchor |= AnchorStyles.Top;
                }
            }
            else
            {
                m_resizableElement = sender as IVariableHeightElement;
                m_resizableElementIndex = m_elements.FindIndex(element => element == m_resizableElement);
                for(int index = m_resizableElementIndex + 1; index < m_elements.Count; index++)
                { //make all element anchoring bottom
                    m_elements[index].MainControl.Anchor &= ~AnchorStyles.Top;
                    m_elements[index].MainControl.Anchor |= AnchorStyles.Bottom;
                }
            }
        }

        protected abstract void element_HeightChanged(object sender, EventArgs e);

        internal void DisableAnchors()
        {
            foreach (var element in m_elements) //make all top
            {
                element.MainControl.Anchor &= ~AnchorStyles.Bottom;
                element.MainControl.Anchor |= AnchorStyles.Top;
            }
        }

        internal void EnableAnchors()
        {
            if(m_resizableElement == null) //only top in the first place, nothing to do
                return;
            m_resizableElement.MainControl.Anchor |= AnchorStyles.Top | AnchorStyles.Bottom;
            for (int index = m_resizableElementIndex+1; index < m_elements.Count; index++)
            {
                m_elements[index].MainControl.Anchor |= AnchorStyles.Bottom;
                m_elements[index].MainControl.Anchor &= ~AnchorStyles.Top;
            }
        }
    }

    public class CollapsibleElementManager : CollapsibleElementManagerBase //manager for the upper control, is expected to be docked filled.
    {
        public CollapsibleElementManager(Control control) : base(control)
        {
        }

        public override void AddElement(IVariableHeightElement element)
        {
            base.AddElement(element);
            CollapsibleElementSubManager sub = element as CollapsibleElementSubManager;
            if (sub != null)
                sub.ParentManager = this;
        }

        protected override void element_HeightChanged(object sender, EventArgs e)
        {
            ScrollableControl scr = m_control as ScrollableControl;
            if(scr == null) throw new ArgumentException("The element manager expects its main control to be scrollable");
            IVariableHeightElement element = sender as IVariableHeightElement;
            int diff;
            int newscr;
            if (sender == m_resizableElement)
            {//control will have it min size correct set, not necessarily its size
                //resume and suspend layout handled and anchors handled by subcontrol
                newscr = scr.AutoScrollMinSize.Height + m_resizableElement.MainControl.MinimumSize.Height - m_resizableElement.PrevMinHeight;
                int newHeight = m_resizableElement.MainControl.MinimumSize.Height;
                if (m_control.Height > newscr)
                {
                    newHeight += m_control.Height - newscr;
                }
                diff = newHeight - m_resizableElement.PrevHeight;
                m_resizableElement.MainControl.Height = newHeight;
                for(int index = m_resizableElementIndex + 1; index < m_elements.Count; index++)
                {
                    m_elements[index].MainControl.Top += diff;
                }
                scr.AutoScrollMinSize = new Size(scr.AutoScrollMinSize.Width, newscr);
                return;
            }
            DisableAnchors();
            m_control.SuspendLayout();
            diff = element.MainControl.Height - element.PrevHeight;
            int elementIndex = m_elements.IndexOf(element);
            if(m_resizableElement == null) //move everything up or down
            {
                for(int index = elementIndex + 1; index < m_elements.Count; index++)
                {
                    m_elements[index].MainControl.Top += diff;
                }
                diff = element.MainControl.Height - element.PrevMinHeight;
                scr.AutoScrollMinSize = new Size(scr.AutoScrollMinSize.Width,scr.AutoScrollMinSize.Height+diff);
            }
            else if (elementIndex < m_resizableElementIndex)
            {
                for (int index = elementIndex + 1; index <= m_resizableElementIndex; index++)
                {
                    m_elements[index].MainControl.Top += diff;
                }
                newscr = scr.AutoScrollMinSize.Height + diff;
                //adapt resizable
                int newHeight = m_resizableElement.MainControl.MinimumSize.Height;
                if (scr.ClientSize.Height > newscr)
                {
                    newHeight += scr.ClientSize.Height - newscr;
                }
                diff += newHeight - m_resizableElement.MainControl.Height;
                m_resizableElement.MainControl.Height = newHeight;
                for (int index = m_resizableElementIndex + 1; index < m_elements.Count; index++)
                {
                    m_elements[index].MainControl.Top += diff;
                }
                scr.AutoScrollMinSize = new Size(scr.AutoScrollMinSize.Width, newscr);
            }
            else // one of the bottoms adapted
            {
                //adapt resizable
                newscr = scr.AutoScrollMinSize.Height + diff;
                int newHeight = m_resizableElement.MainControl.MinimumSize.Height;
                if (scr.ClientSize.Height > newscr)
                {
                    newHeight += scr.ClientSize.Height - newscr;
                }
                int resizediff = newHeight - m_resizableElement.MainControl.Height;
                m_resizableElement.MainControl.Height = newHeight;
                for (int index = m_resizableElementIndex + 1; index <= elementIndex; index++)
                {
                    m_elements[index].MainControl.Top += resizediff;
                }
                diff += resizediff;
                for (int index = elementIndex + 1; index < m_elements.Count; index++)
                {
                    m_elements[index].MainControl.Top += diff;
                }
                scr.AutoScrollMinSize = new Size(scr.AutoScrollMinSize.Width, newscr);
            }
            m_control.ResumeLayout(false);
            m_control.PerformLayout();
            EnableAnchors();
        }

        public void AddSubManagerFromControl(Control ctrl)
        {
            CollapsibleElementSubManager man = new CollapsibleElementSubManager(ctrl);
            foreach(Control c in ctrl.Controls)
            {
                CollapsibleGroupBox gb = c as CollapsibleGroupBox;
                if (gb != null)
                {
                    gb.Init();
                    man.AddElement(gb);
                }
            }
            if (man.Count == 0) //no groupbox elements
                AddElement(new NonVariableElement(ctrl));
            else
                AddElement(man);
        }
    }

    public class CollapsibleElementSubManager : CollapsibleElementManagerBase, IVariableHeightElement
    {
        public CollapsibleElementSubManager(Control control) : base(control)
        {
        }

        internal CollapsibleElementManager m_parentManager;
	    internal iba.Utility.CollapsibleElementManager ParentManager
	    {
		    get { return m_parentManager; }
		    set { m_parentManager = value; }
	    }

        protected override void element_HeightChanged(object sender, EventArgs e)
        {
            if(m_parentManager != null) m_parentManager.DisableAnchors();
            m_control.Parent.SuspendLayout();
            m_control.SuspendLayout();
            m_prevHeight = m_control.Height;
            m_prevMinHeight = m_control.MinimumSize.Height;
            IVariableHeightElement element = sender as IVariableHeightElement;
            int elementIndex = m_elements.IndexOf(element);
            int diff;

            if (sender == m_resizableElement)
            {
                diff = element.MainControl.MinimumSize.Height - element.PrevHeight;
                m_resizableElement.MainControl.Height = m_resizableElement.MainControl.MinimumSize.Height;
                for(int index = elementIndex + 1; index < m_elements.Count; index++)
                    m_elements[index].MainControl.Top += diff;
                diff = element.MainControl.MinimumSize.Height - element.PrevMinHeight;
                DisableAnchors();
                m_control.MinimumSize = new Size(0, m_control.MinimumSize.Height + diff);
                m_control.Height = m_control.MinimumSize.Height;               
                EnableAnchors();
            }
            else if(m_resizableElement == null)
            {
                diff = element.MainControl.Height - element.PrevHeight;
                for(int index = elementIndex + 1; index < m_elements.Count; index++ )
                    m_elements[index].MainControl.Top += diff;
                DisableAnchors();
                if (m_bResizableElementCollapsing)
                {
                    diff = element.MainControl.MinimumSize.Height - element.PrevMinHeight;
                    m_bResizableElementCollapsing = false;
                }
                m_control.MinimumSize = new Size(0, m_control.MinimumSize.Height + diff);
                m_control.Height = m_control.MinimumSize.Height;
                EnableAnchors();
            }
            else if (elementIndex < m_resizableElementIndex)
            {
                diff = element.MainControl.Height - element.PrevHeight;
                for(int index = elementIndex + 1; index <= m_resizableElementIndex; index++)
                {
                    m_elements[index].MainControl.Top += diff;
                }
                int resizeDiff = m_resizableElement.MainControl.Height - m_resizableElement.MainControl.MinimumSize.Height;
                m_resizableElement.MainControl.Height = m_resizableElement.MainControl.MinimumSize.Height;
                for(int index = m_resizableElementIndex + 1; index < m_elements.Count; index++)
                {
                    m_elements[index].MainControl.Top += diff - resizeDiff;
                }
                DisableAnchors();
                m_control.MinimumSize = new Size(0, m_control.MinimumSize.Height + diff);
                m_control.Height = m_control.MinimumSize.Height;
                EnableAnchors();
            }
            else
            {
                diff = element.MainControl.Height - element.PrevHeight;
                int resizeDiff = m_resizableElement.MainControl.Height - m_resizableElement.MainControl.MinimumSize.Height;
                m_resizableElement.MainControl.Height = m_resizableElement.MainControl.MinimumSize.Height;
                for(int index = m_resizableElementIndex + 1; index <= elementIndex; index++)
                {
                    m_elements[index].MainControl.Top -= resizeDiff;
                }
                for(int index = elementIndex + 1; index < m_elements.Count; index++)
                {
                    m_elements[index].MainControl.Top += diff - resizeDiff;
                }
                DisableAnchors();
                m_control.MinimumSize = new Size(0, m_control.MinimumSize.Height + diff);
                m_control.Height = m_control.MinimumSize.Height;
                EnableAnchors();
            }
            m_control.ResumeLayout(false);
            m_control.PerformLayout();
            if(HeightChanged != null) HeightChanged(this, e);
            m_control.Parent.ResumeLayout(false);
            m_control.Parent.PerformLayout();
            if(m_parentManager != null) m_parentManager.EnableAnchors();
        }

        private bool m_bResizableElementCollapsing;

        protected override void element_AnchorChanged(object sender, EventArgs e)
        {
            base.element_AnchorChanged(sender, e);
            if(m_resizableElement==null) //make ourselves not resizable
            {
                m_bResizableElementCollapsing = true;
                m_control.Anchor &= ~AnchorStyles.Bottom;
                m_control.Anchor |= AnchorStyles.Top;
            }
            else  //make ourselves  resizable
            {
                m_control.Anchor |= AnchorStyles.Top | AnchorStyles.Bottom;
            }
            //notify parent
            if(AnchorChanged != null) AnchorChanged(this, e);
        }

        #region IVariableHeightElement Members

        private int m_prevHeight;
        public int PrevHeight
        {
            get { return m_prevHeight; }
        }

        private int m_prevMinHeight;
        public int PrevMinHeight
        {
            get { return m_prevMinHeight; }
        }

        public Control MainControl
        {
            get { return m_control; }
        }

        public event EventHandler AnchorChanged;

        public event EventHandler HeightChanged;

        #endregion
    }
}
