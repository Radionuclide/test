using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;

namespace Alunorf_sinec_h1_plugin
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

        static public T max<T>(IEnumerable<T> container, CompareDelegate<T> comp)
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

    public class BiMap<S, T>
        where S : IComparable<S>
        where T : IComparable<T>
    {
        private SortedDictionary<S, T> FromTo;
        private SortedDictionary<T, S> ToFrom;
        public BiMap()
        {
            FromTo = new SortedDictionary<S, T>();
            ToFrom = new SortedDictionary<T, S>();
        }
        public bool Contains(S s)
        {
            return FromTo.ContainsKey(s);
        }
        public bool Contains(T t)
        {
            return ToFrom.ContainsKey(t);
        }

        public T this[S s]
        {
            get { return FromTo[s]; }
            set
            {
                if (FromTo[s].CompareTo(value) == 0) return;
                if (FromTo.ContainsKey(s)) ToFrom.Remove(FromTo[s]);
                if (ToFrom.ContainsKey(value)) FromTo.Remove(ToFrom[value]);
                FromTo[s] = value;
                ToFrom[value] = s;
            }
        }

        public S this[T t]
        {
            get { return ToFrom[t]; }
            set
            {
                if (ToFrom[t].CompareTo(value) == 0) return;
                if (ToFrom.ContainsKey(t)) FromTo.Remove(ToFrom[t]);
                if (FromTo.ContainsKey(value)) ToFrom.Remove(FromTo[value]);
                ToFrom[t] = value;
                FromTo[value] = t;
            }
        }

        public bool Remove(S s)
        {
            if (FromTo.ContainsKey(s))
            {
                ToFrom.Remove(FromTo[s]);
                FromTo.Remove(s);
                return true;
            }
            return false;
        }

        public bool Remove(T t)
        {
            if (ToFrom.ContainsKey(t))
            {
                FromTo.Remove(ToFrom[t]);
                ToFrom.Remove(t);
            }
            return true;
        }

        public void Clear()
        {
            FromTo.Clear();
            ToFrom.Clear();
        }
    }
}
