using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace iba.Utility
{
    public static class ExtensionMethods
    {
        
        public static void SafeInvoke(this Control uiElement, MethodInvoker updater, bool forceSynchronous)
        {
            if (uiElement == null)
            {
                throw new ArgumentNullException("uiElement");
            }

            if (uiElement.InvokeRequired)
            {
                if (forceSynchronous)
                {
                    uiElement.Invoke((Action)delegate { SafeInvoke(uiElement, updater, forceSynchronous); });
                }
                else
                {
                    uiElement.BeginInvoke((Action)delegate { SafeInvoke(uiElement, updater, forceSynchronous); });
                }
            }
            else
            {
                if (!uiElement.IsHandleCreated)
                {
                    // Do nothing if the handle isn't created already.  The user's responsible
                    // for ensuring that the handle they give us exists.
                    return;
                }

                if (uiElement.IsDisposed)
                {
                    throw new ObjectDisposedException("Control is already disposed.");
                }

                updater();
            }
        }
    }
}
