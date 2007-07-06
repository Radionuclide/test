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

    [Serializable]
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

    [Serializable]
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

    [Serializable]
    public class Set<T> : List<T>
        where T:IComparable<T>
    {
        public new void Add(T elem)
        {
            int pos = BinarySearch(elem);
            if (pos < 0) Insert(~pos, elem);
        }
        
        public new void  AddRange(IEnumerable<T> elems)
        {
            foreach (T elem in elems) Add(elem);
        }
        
        public Set()
        {

        }

        public new int IndexOf(T elem)
        {
            return Math.Max(-1, BinarySearch(elem));
        }

	    public new bool Contains(T elem )
	    { 
		    return BinarySearch( elem ) >= 0;
	    }

        public new bool Remove(T elem)
        {
            int pos = BinarySearch(elem);
            if (pos < 0) return false;
            else RemoveAt(pos);
            return true;
        }

        private enum SetOperationKind {UNION,INTERSECTION,DIFFERENCE }

        public static Set<T> Union(Set<T> A, Set<T> B)
        {
            return SetOperation(A, B, SetOperationKind.UNION);
        }

        public static Set<T> Intersection(Set<T> A, Set<T> B)
        {
            return SetOperation(A, B, SetOperationKind.INTERSECTION);
        }

        public static Set<T> Difference(Set<T> A, Set<T> B)
        {
            return SetOperation(A, B, SetOperationKind.DIFFERENCE);
        }
        
        private static Set<T> SetOperation(Set<T> A, Set<T> B, SetOperationKind kind)
        {
            Set<T> result = new Set<T>();
            if (A.Count == 0) 
            {
                if (kind == SetOperationKind.UNION)
                    (result as List<T>).AddRange(B);
                return result;
            }
            if (B.Count == 0) 
            {
                if (kind != SetOperationKind.INTERSECTION)
                    (result as List<T>).AddRange(A);
                return result;
            }
            IEnumerator<T> Ait = A.GetEnumerator();
            IEnumerator<T> Bit = B.GetEnumerator();
            while (true)
            {
                int compare = Ait.Current.CompareTo(Bit.Current);
                if (compare < 0)
                {
                    if (kind != SetOperationKind.INTERSECTION) 
                        (result as List<T>).Add(Ait.Current);
                    if (!Ait.MoveNext())
                    {
                        if (kind == SetOperationKind.UNION) 
                            while (Bit.MoveNext())
                                (result as List<T>).Add(Bit.Current);
                        return result;
                    }
                }
                else if (compare > 0)
                {
                    if (kind == SetOperationKind.UNION) 
                        (result as List<T>).Add(Bit.Current);
                    if (!Bit.MoveNext())
                    {
                        if (kind != SetOperationKind.INTERSECTION) 
                            while (Ait.MoveNext())
                                (result as List<T>).Add(Ait.Current);
                        return result;
                    }
                }
                else //equality
                {
                    if (kind != SetOperationKind.DIFFERENCE) 
                        (result as List<T>).Add(Ait.Current);
                    if (!Ait.MoveNext())
                    {
                        if (kind == SetOperationKind.UNION) 
                            while (Bit.MoveNext())
                                (result as List<T>).Add(Bit.Current);
                        return result;
                    }
                    if (!Bit.MoveNext())
                    {
                        if (kind != SetOperationKind.INTERSECTION) 
                            while (Ait.MoveNext())
                                (result as List<T>).Add(Ait.Current);
                        return result;
                    }
                }
            }
        }
    }
}
