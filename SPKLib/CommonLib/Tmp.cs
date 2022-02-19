using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace CommonLib.TmpHelper
{
    public class Tmp
    {
        private string filename;
        private string path;

        private static object sync = new object();

        public Tmp(int startCounter=0)
        {            
            // Путь .\\Tmp    
            path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tmp");
            var counter = startCounter;
            do
            {

                filename = Path.Combine(path, string.Format("{0}_{1:dd.MM.yyy}_{2}.tmp",
                    AppDomain.CurrentDomain.FriendlyName, DateTime.Now, counter));
                counter++;

            } while (File.Exists(filename));

        }        

        public void Write(IEnumerable<string> strings)
        {
            _Write(strings);
        }

        private void _Write(IEnumerable<string> strings)
        {
            try
            {                            
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path); // Создаем директорию, если нужно
                                
                lock (sync)
                {
                    File.AppendAllLines(filename, strings, Encoding.GetEncoding("Windows-1251"));
                }
            }
            catch
            {
                // Перехватываем все и ничего не делаем
            }
        }

    }
}

