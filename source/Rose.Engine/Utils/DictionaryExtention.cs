using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rose.Engine.Utils
{
    public static class DictionaryExtention
    {
        public static TValue GetValue<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key) where TValue : new()
        {
            TValue value;
            if (source.TryGetValue(key, out value) == false)
            {
                value = new TValue();
                source.Add(key, value);
            }

            return value;
        }
    }
}
