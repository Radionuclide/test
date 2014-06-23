using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iba.Utility
{
    interface CollapsablibleElement
    {
        bool Collapsed
        {
            get;
            set;
        }

        int CollapsedSize
        {
            get;
        }

        int FullSize
        {
            get;
        }

        Control MainControl
        {
            get;
        }

        event EventHandler CollapsedChanged;
        event EventHandler AnchorChanged;
    }


    public class CollapsablibleElementManager
    {
        private List<CollapsablibleElement> m_elements;
        CollapsablibleElement m_resizableElement;
        public event EventHandler MinSizeChanged;

        public CollapsablibleElementManager()
        {
            m_elements = new List<CollapsablibleElement>();
            m_resizableElement = null;
        }
        public void AddElement(CollapsablibleElement element)
        {
            if(element.MainControl.Anchor.HasFlag(AnchorStyles.Top) && element.MainControl.Anchor.HasFlag(AnchorStyles.Bottom))
            {
                if(m_resizableElement != null) throw new ArgumentException("Only one resizable element allowed");
                m_resizableElement = element;
                m_resizableElement.AnchorChanged += new EventHandler(m_resizableElement_AnchorChanged);
            }
            else if (element.MainControl.Anchor.HasFlag(AnchorStyles.Top) && m_resizableElement != null)
            {
                throw new ArgumentException("elements following the resizable element can only have bottom anchor");
            }
            else if (element.MainControl.Anchor.HasFlag(AnchorStyles.Bottom) && m_resizableElement == null)
            {
                throw new ArgumentException("elements before the resizable element can only have top anchor");
            }
            element.CollapsedChanged += new EventHandler(element_CollapsedChanged);
            m_elements.Add(element);
        }

        void element_CollapsedChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void m_resizableElement_AnchorChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
