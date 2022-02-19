using System;

namespace CommonLib
{
    public class Flags
    {
        public int Value { get; private set; } = 0;
        
        public Flags(params object[] flags)
        {
            for (int i = 0; i < flags.Length; i++)
            {
                Value |= (int)flags[i];
            }
        }

        public Flags(int value)
        {
            Value = value;
        }

        
        public bool Contains(object flag) => (Value & (int)flag) == (int)flag;

        public static bool Contains(object value, object flag) => ((int)value & (int)flag) == (int)flag;

        public void Add(object flag)
        {
            Value |= (int)flag;
        }

        public void Remove(object flag)
        {
            Value ^= (int)flag;
        }

        public override string ToString()
        {
            return Convert.ToString(Value, 2);
        }
    }
}
