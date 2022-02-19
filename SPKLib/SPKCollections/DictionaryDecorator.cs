using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SPKCollections.Extentions;

namespace SPKCollections
{
    public class DictionaryDecorator<T> : IEnumerable<Dictionary<string, T>>
    {
        private IEnumerable<T[]> items;

        public DictionaryDecorator(IEnumerable<T[]> items)
        {
            this.items = items ?? throw new ArgumentNullException(nameof(items));
        }

        public IEnumerator<Dictionary<string, T>> GetEnumerator()
        {
            var en = items.GetEnumerator();
            if (!en.MoveNext()) yield break;
            var header = en.Current.Select(i => i.ToString()).ToArray();
            while (en.MoveNext())
            {
                var d = new Dictionary<string, T>();
                var vals = en.Current.ToArray();

                for (int i = 0; i < Math.Min(header.Length, vals.Length); i++)
                    d.Add(header[i], vals[i]);

                yield return d;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }   
}
