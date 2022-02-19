using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib
{
    public class Encrypter
    {
        private static byte rRol(byte value, int shift)
        {
            return (byte)(((value * 257) >> shift) & 255);
        }

        private static byte[] getKeys(int count)
        {
            var r = new Random();
            var keys = new byte[count];
            for(int i=0; i< count; i++)
            {
                keys[i] = (byte)(r.Next(0, 7));
            }
            return keys;
            //r.NextBytes(keys);
            //return keys.Select(k=>(byte)(k>>5)).ToArray();
        }        

        private static Encoding encoding  = Encoding.Unicode;

        public static string Encrypt(string value)
        {
            if (value == "") return value;
            var bytes = encoding.GetBytes(value);
            var keys = getKeys(bytes.Length);

            var mix = new List<byte>();
            for (int i = 0; i < keys.Length; i++)
            {
                mix.Add(keys[i]);
                mix.Add(rRol(bytes[i], keys[i]));
            }
            return Convert.ToBase64String(mix.ToArray());
        }

        public static string UnEncrypt(string value)
        {
            if (value == "") return value;
            var enc = Convert.FromBase64String(value);

            var unenc = new byte[enc.Length / 2];
            for (int i = 0; i < enc.Length; i += 2)
            {
                var key = enc[i];
                var val = enc[i + 1];
                unenc[i / 2] = rRol(val, 8 - key);
            }
            return encoding.GetString(unenc);
        }
    }
}