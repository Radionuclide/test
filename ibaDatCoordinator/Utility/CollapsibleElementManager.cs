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

        bool Anchored
        {
            get;
        }

        Control MainControl
        {
            get;
        }

        event EventHandler AnchorChanged; //
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

        public bool Anchored
        {
            get {return false;}
        }
        public event EventHandler  AnchorChanged; //will never be fired

        public event EventHandler  HeightChanged; //will never be fired

    }

    public class CollapsibleElementManager
    {
        private List<IVariableHeightElement> m_elements;
        IVariableHeightElement m_resizableElement;
        int m_resizableElementIndex;
        private Control m_control;
	    public bool Anchored
	    {
		    get { return m_resizableElement != null; }
        }

        public CollapsibleElementManager(Control control)
        {
            m_control = control;
            m_elements = new List<IVariableHeightElement>();
            m_resizableElement = null;
        }
        public void AddElement(IVariableHeightElement element)
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

        void element_HeightChanged(object sender, EventArgs e)
        {
            IVariableHeightElement element = sender as IVariableHeightElement;
            int diff;
            if (sender == m_resizableElement)
            {//control will have it minsize correct set
                int diffscr = m_resizableElement.MainControl.MinimumSize.Height - m_resizableElement.PrevMinHeight;
                int newHeight = m_resizableElement.MainControl.MinimumSize.Height;
                ScrollableControl scr = m_control as ScrollableControl;
                if (scr != null && m_control.Height > scr.AutoScrollMinSize.Height + diffscr)
                {
                    newHeight += m_control.Height - (scr.AutoScrollMinSize.Height + diffscr);
                }
                diff = newHeight - m_resizableElement.PrevHeight;
                m_resizableElement.MainControl.Height = newHeight;
                for (int index = m_resizableElementIndex+1; index  < m_elements.Count; index++)
                    m_elements[index].MainControl.Top += diff;
                if (scr != null)
                    scr.AutoScrollMinSize = new Size(scr.AutoScrollMinSize.Width, scr.AutoScrollMinSize.Height + diffscr);
                return;
            }
            diff = element.MainControl.Height - element.PrevHeight;
            int elementIndex = m_elements.IndexOf(element);
            if(m_resizableElement == null) //move everything up or down
            {
                for(int index = elementIndex + 1; index < m_elements.Count; index++)
                {
                    m_elements[index].MainControl.Top += diff;
                }
                ScrollableControl scr = m_control as ScrollableControl;
                if (scr != null)
                    scr.AutoScrollMinSize = new Size(scr.AutoScrollMinSize.Width,scr.AutoScrollMinSize.Height+diff);
            }
            else if (elementIndex < m_resizableElementIndex)
            {
                for(int index = elementIndex + 1; index <= m_resizableElementIndex; index++)
                {
                    m_elements[index].MainControl.Top += diff;
                }
                //adapt resizable
                int origdiff = diff;
                if(m_resizableElement.MainControl.Height - diff >= m_resizableElement.MainControl.MinimumSize.Height)
                {
                    m_resizableElement.MainControl.Height -= diff;
                    diff = 0;
                }
                else
                {
                    diff -= m_resizableElement.MainControl.Height - m_resizableElement.MainControl.MinimumSize.Height;
                    m_resizableElement.MainControl.Height = m_resizableElement.MainControl.MinimumSize.Height;
                }
                ScrollableControl scr = m_control as ScrollableControl;
                if(scr != null)
                    scr.AutoScrollMinSize = new Size(scr.AutoScrollMinSize.Width, scr.AutoScrollMinSize.Height + origdiff);
            }
            else
            {
                //TODO
            }
        }

        void element_AnchorChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
