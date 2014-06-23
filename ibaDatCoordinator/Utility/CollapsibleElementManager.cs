using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace iba.Utility
{
    public interface IVariableHeightElement
    {
        int PrevHeight
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


    public class CollapsibleElementManager
    {
        private List<IVariableHeightElement> m_elements;
        IVariableHeightElement m_resizableElement;
        private Control m_parentControl;

        public CollapsibleElementManager(Control parent)
        {
            m_parentControl = parent;
            m_elements = new List<IVariableHeightElement>();
            m_resizableElement = null;
        }
        public void AddElement(IVariableHeightElement element)
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
            element.HeightChanged += new EventHandler(element_HeightChanged);
            m_elements.Add(element);
        }

        void element_HeightChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void m_resizableElement_AnchorChanged(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
