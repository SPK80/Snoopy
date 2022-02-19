using System;
using System.Collections.Generic;

namespace SPKCollections.Extentions
{
    public static class ListExtentions
    {
        public static void Move<T>(this List<T> list, T item, int newIndex)
        {
            int oldIndex = list.IndexOf(item);
            list.Remove(item);
            list.Insert(newIndex, item);
        }

        public static void Move<T>(this List<T> list, int from, int to)
        {
            var elem = list[from];
            list.RemoveAt(from);
            list.Insert(to, elem);
        }

        public static void Swap<T>(this List<T> list, int i, int j)
        {
            var elem1 = list[i];
            var elem2 = list[j];

            list[i] = elem2;
            list[j] = elem1;
        }
    }
}

