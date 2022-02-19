using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SPKCollections
{
    public class BidirectDictionary<TKey, TValue>
    {
        private List<KeyValuePair<TKey, TValue>> items = new List<KeyValuePair<TKey, TValue>>();

        public BidirectDictionary()
        {
        }

        public void Add(TKey key, TValue value)
        {
            items.Add(new KeyValuePair<TKey, TValue>(key, value));
        }
        public void Remove(TKey key)
        {
            items.RemoveAt(items.FindIndex(i => i.Key.Equals(key)));            
        }
        public void Remove(TValue value)
        {
            items.RemoveAt(items.FindIndex(i => i.Value.Equals(value)));
        }

        public TValue this[TKey key]
        {
            get => items.Find(i => i.Key.Equals(key)).Value;
            set
            {
                var index = items.FindIndex(i => i.Key.Equals(key));
                var newKV = new KeyValuePair<TKey, TValue>(key, value);
                if (index >= 0)
                    items[index] = newKV;
                else
                    items.Add(newKV);
            }
        }

        public TKey this[TValue val]
        {
            get => items.Find(i => i.Value.Equals(val)).Key;
            set
            {
                var index = items.FindIndex(i => i.Value.Equals(val));
                var newKV = new KeyValuePair<TKey, TValue>(value, val);
                if (index >= 0)
                    items[index] = newKV;
                else
                    items.Add(newKV);
            }
        }

        public IEnumerable<TKey> Keys => items.Select(i => i.Key);
        public IEnumerable<TValue> Values => items.Select(i => i.Value);

        //class BidirectionalItem
        //{
        //    public TKey Key;
        //    public TValue Value;

        //    public BidirectionalItem(TKey key, TValue value)
        //    {
        //        Key = key;
        //        Value = value;
        //    }
        //}
    }

}
