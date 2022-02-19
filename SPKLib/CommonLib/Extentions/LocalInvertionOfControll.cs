using System.Collections.Generic;
using System.Linq;

namespace CommonLib.LocalInvertionOfControll
{
    public static class LocalInvertionOfControl
    {
        public static bool IsOneOf<T>(this T self, params T[] items)
        {
            return items.Contains(self);
        }

        public static bool IsOneOf<T>(this T self, ICollection<T> collection)
        {
            return collection.Contains(self);
        }

        public static T AddTo<T>(this T self, ICollection<T> collection)
        {
            collection.Add(self);
            return self;
        }

        public static T RemoveFrom<T>(this T self, ICollection<T> collection)
        {
            collection.Remove(self);
            return self;
        }


    }
}
    
