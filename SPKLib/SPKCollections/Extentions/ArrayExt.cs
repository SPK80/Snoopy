using System;

namespace SPKCollections.Extentions
{
    public static class ArrayExt
    {
        public static void throwIfNullOrNotEnoughValues<T>(this T[] array, int enoughCount)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));
            if (array.Length < enoughCount)
                throw new ArgumentException($"Передано неодстаточно данных. Необходимо не менее {enoughCount}");
        }

        public static T[] Fill<T>(this T[] array, T val)
        {
            for (int i = 0; i < array.Length; i++)
                array[i] = val;
            return array;
        }

        public static T[] NewFillArray<T>(this T filling, int length)
        {
            var row = new T[length];
            row.Fill(filling);
            return row;
        }

        public static T[] ReplaceNull<T>(this T[] array, T val)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] == null) array[i] = val;
            }
            return array;
        }


        public static T[] StretchRight<T>(this T[] array, int length)
        {
            T[] result = new T[length];

            for (int i = 0; i < array.Length; i++)
            {
                result[i] = array[i];
            }
            return result;
        }

        public static T[] StretchLeft<T>(this T[] array, int length)
        {
            T[] result = new T[length];

            for (int i = 0; i < array.Length; i++)
            {
                result[i + length - array.Length] = array[i];
            }
            return result;
        }

        public static T[] Copy<T>(this T[] array, int start, int length=0)
        {
            //if (start == 0 && length == array.Length)
            //    return array;
            if (length == 0)
                length = array.Length- start;
            T[] result = new T[length - start+1];
            for(int i= 0; i< length; i++)
            {
                result[i] = array[i+ start];
            }
            return result;
        }

        

    }
}
