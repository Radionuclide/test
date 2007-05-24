using System;
using System.Collections.Generic;
using System.Text;

namespace iba.Utility
{
    [Serializable]
    public class STLLikeMap<Key, Value> : SortedDictionary<Key, Value> where Value : new()
    {
        new public Value this[Key key]
        {
            get
            {
                if (!ContainsKey(key))
                    Add(key, new Value());
                return base[key];
            }
            set { base[key] = value; }
        }
    }

    public class Pair<TFirst, TSecond>
    {

        public Pair(TFirst first, TSecond second)
        {
            _First = first;
            _Second = second;
        }

        public TFirst First
        {
            get { return _First; }
            set { _First = value; }
        }

        private TFirst _First;

        public TSecond Second
        {
            get { return _Second; }
            set { _Second = value; }
        }

        private TSecond _Second;
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
                if (FromTo.ContainsKey(s) && FromTo[s].CompareTo(value) == 0) return;
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
                if (ToFrom.ContainsKey(t) && ToFrom[t].CompareTo(value) == 0) return;
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
                return true;
            }
            return false;
        }
    }
}
