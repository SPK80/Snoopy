using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SPKCollections
{
    public class TableDecorator<T> : ITable<T>
    {
        private IEnumerable<T[]> items;

        public TableDecorator(IEnumerable<T[]> items)
        {
            this.items = items ?? throw new ArgumentNullException(nameof(items));
        }
        
        public string[] Header => items.First().Select(i => i.ToString()).ToArray();

        public IEnumerator<T[]> GetEnumerator()
        {
            var en = items.GetEnumerator();
            en.MoveNext();
            while (en.MoveNext())
            {
                yield return en.Current;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
