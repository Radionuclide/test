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
}
