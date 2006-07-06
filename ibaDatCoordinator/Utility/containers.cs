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
}
