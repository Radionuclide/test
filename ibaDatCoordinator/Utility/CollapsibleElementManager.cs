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

    public class CollapsibleElementManager : IVariableHeightElement
    {
        private List<IVariableHeightElement> m_elements;
        IVariableHeightElement m_resizableElement;
        int m_resizableElementIndex;
        private Control m_control;
        private bool m_bTop;
	    public bool Anchored
	    {
		    get { return m_resizableElement != null; }
        }

        public CollapsibleElementManager(Control control, bool top)
        {
            //if top is true this is assumed to be the topmanager and the associated control is DockStyle Fill, 
            //          AutoScroll is set, AutoScrollMinSize is about the size of the entire control and MinSize is much smaller
            //if top is false this is a submanager, the associated control is DockStyle None, Top and Bottom anchors can or cannot be set depending if a resizable element is present or not.
            m_bTop = top;


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
            if(sender == m_resizableElement) throw new ArgumentException("the resizable element should signal anchor changed not height changed");
            int diff = element.MainControl.Height - element.PrevHeight;
            int elementIndex = m_elements.IndexOf(element);
            if(m_resizableElement == null) //move everything up or down
            {
                for(int index = elementIndex + 1; index < m_elements.Count; index++)
                {
                    m_elements[index].MainControl.Top += diff;
                }
                if(m_bTop)
                {
                    ScrollableControl scr = m_control as ScrollableControl;
                    if (scr != null)
                        scr.AutoScrollMinSize = new Size(scr.AutoScrollMinSize.Width,scr.AutoScrollMinSize.Height+diff);
                }
                else //signal topControl
                {
                    if(HeightChanged != null)
                    {
                        m_prevHeight = m_control.Height;
                        m_control.Height += diff;
                        if(HeightChanged != null) 
                            HeightChanged(this, EventArgs.Empty);
                    }
                }
            }
            else if (elementIndex < m_resizableElementIndex)
            {
                for(int index = elementIndex + 1; index <= m_resizableElementIndex; index++)
                {
                    m_elements[index].MainControl.Top += diff;
                }
                //adapt resizable
                if(m_resizableElement.MainControl.Height - diff >= m_resizableElement.MainControl.MinimumSize.Height)
                {
                    m_resizableElement.MainControl.Height -= diff;
                    return;                    
                }
                else
                {
                    diff -= m_resizableElement.MainControl.Height - m_resizableElement.MainControl.MinimumSize.Height;
                    m_resizableElement.MainControl.Height = m_resizableElement.MainControl.MinimumSize.Height;
                }
                if(m_bTop)
                {
                    ScrollableControl scr = m_control as ScrollableControl;
                    if(scr != null)
                        scr.AutoScrollMinSize = new Size(scr.AutoScrollMinSize.Width, scr.AutoScrollMinSize.Height + diff);
                }
                else //signal topControl
                {
                    if(HeightChanged != null)
                    {
                        m_prevHeight = m_control.Height;
                        m_control.Height += diff;
                        if(HeightChanged != null)
                            HeightChanged(this, EventArgs.Empty);
                    }
                }
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

        #region IVariableHeightElement Members


        private int m_prevHeight;
        public int PrevHeight
        {
            get { return m_prevHeight; }
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
