using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SPKCollections.Extentions
{
    public static class IEnumerableExt
    {
        public static bool ContainsWith<T>(this IEnumerable<T> enumerable, Predicate<T> predicate)
        {
            foreach(var it in enumerable)
            {
                if (predicate(it)) return true;
            }
            return false;
        }
    }
}
