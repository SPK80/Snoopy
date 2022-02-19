using System;
using System.Collections.Generic;
using System.Collections;

namespace SPKCollections
{
    public class NamedArray<T>:IEnumerable<T>
    {
        private string name;
        private T[] items;
        
        public NamedArray(string name, params T[] items)
        {
            this.name = name ?? throw new ArgumentNullException(nameof(name));
            this.items = items;
        }
        public T this[int index] => items[index];
        public override string ToString() => name;
        public IEnumerator<T> GetEnumerator()
        {
            foreach(var item in items)
            {
                yield return item;
            }
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }

}
