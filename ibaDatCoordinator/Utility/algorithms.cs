using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace iba.Utility
{

    class Algorithms
    {
        static public T max<T>(IEnumerable<T> container) where T : IComparable<T>
        {
            IEnumerator<T> iter = container.GetEnumerator();
            iter.MoveNext();
            T max = iter.Current;
            while (iter.MoveNext())
            {
                if (iter.Current.CompareTo(max) > 0) max = iter.Current;
            }
            return max;
        }

        static public T min<T>(IEnumerable<T> container) where T : IComparable<T>
        {
            IEnumerator<T> iter = container.GetEnumerator();
            iter.MoveNext();
            T min = iter.Current;
            while (iter.MoveNext())
            {
                if (iter.Current.CompareTo(min) < 0) min = iter.Current;
            }
            return min;
        }

        public delegate int CompareDelegate<T>(T t1, T t2);

        static public T max<T>(IEnumerable<T> container,CompareDelegate<T> comp)
        {
            IEnumerator<T> iter = container.GetEnumerator();
            iter.MoveNext();
            T max = iter.Current;
            while (iter.MoveNext())
            {
                if (comp(iter.Current, max) > 0) max = iter.Current;
            }
            return max;
        }

        static public T min<T>(IEnumerable<T> container, CompareDelegate<T> comp)
        {
            IEnumerator<T> iter = container.GetEnumerator();
            iter.MoveNext();
            T min = iter.Current;
            while (iter.MoveNext())
            {
                if (comp(iter.Current, min) < 0) min = iter.Current;
            }
            return min;
        }

        static public T max<T>(IEnumerable container, CompareDelegate<T> comp)
        {
            IEnumerator iter = container.GetEnumerator();
            iter.MoveNext();
            T max = (T)iter.Current;
            while (iter.MoveNext())
            {
                if (comp((T)iter.Current, max) > 0) max = (T)iter.Current;
            }
            return max;
        }

        static public T min<T>(IEnumerable container, CompareDelegate<T> comp)
        {
            IEnumerator iter = container.GetEnumerator();
            iter.MoveNext();
            T min = (T)iter.Current;
            while (iter.MoveNext())
            {
                if (comp((T)iter.Current, min) < 0) min = (T)iter.Current;
            }
            return min;
        }


    }
}


