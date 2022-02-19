using System;

namespace CommonLib
{
    public class GenericEventArgs<T>: EventArgs
    {        
        public GenericEventArgs(T value)
        {
            Value = value;
        }

        public T Value { get; set; }
    }

    public class GenericEventArgs<T1, T2> : EventArgs
    {
        public GenericEventArgs(T1 value1, T2 value2)
        {
            Value1 = value1;
            Value2 = value2;
        }

        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
    }

    public class GenericEventArgs<T1, T2, T3> : EventArgs
    {
        public GenericEventArgs(T1 value1, T2 value2, T3 value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public T1 Value1 { get; set; }
        public T2 Value2 { get; set; }
        public T3 Value3 { get; set; }
    }

    public static class GenericEventArgsFabric
    {
        public static GenericEventArgs<T> Create<T>(T value)
        {
            return new GenericEventArgs<T>(value);
        }

        public static GenericEventArgs<T1, T2> Create<T1, T2>(T1 value1, T2 value2)
        {
            return new GenericEventArgs<T1, T2>(value1, value2);
        }

        public static GenericEventArgs<T1, T2, T3> Create<T1, T2, T3>(T1 value1, T2 value2, T3 value3)
        {
            return new GenericEventArgs<T1, T2, T3>(value1, value2, value3);
        }
    }
}
