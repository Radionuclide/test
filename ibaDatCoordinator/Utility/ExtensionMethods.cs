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

        public static T[][] ToPairArray<T>(this IEnumerable<Tuple<T, T>> collection)
        {
            return collection == null ? null : collection.Select(t => new[] { t.Item1, t.Item2 }).ToArray();
        }

        public static void SetFromPairArray<T>(this ICollection<Tuple<T, T>> collection, T[][] array)
        {
            if (collection == null)
                throw new ArgumentNullException();
            collection.Clear();
            foreach (var pair in array)
            {
                if (pair.Length != 2)
                    throw new ArgumentException("Inner array did not have length 2");
                collection.Add(Tuple.Create(pair[0], pair[1]));
            }
        }
    }
}
