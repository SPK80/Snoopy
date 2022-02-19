using System;
using System.Text;
using System.IO;

namespace CommonLib.Extentions.String
{
    public static class PathExtentions
    {
        public static string GetFileExtention(this string path)
        {
            return Path.GetExtension(path);
            //int dotIndex = path.LastIndexOf('.');
            //return path.Substring(dotIndex + 1, path.Length - dotIndex - 1).ToLower();
        }
        public static bool IsFolder(this string path)
        {
            if (path[path.Length - 1] == Path.DirectorySeparatorChar)
                return true;
            else return Directory.Exists(path);
        }
    }

    public static class StringExtentions
	{
		public static bool isVoid(this string str)
		{
			return (str == null || str == "");
		}

		/// <summary>
		/// Возвращает колл-во слов
		/// </summary>
		/// <param name="anyOf">массив символов-разделителей</param>
		/// <returns></returns>
		public static int WordCount(this string str, params char[] anyOf)
        {
            return str.Split(anyOf,
                             StringSplitOptions.RemoveEmptyEntries).Length;
        }
        /// <summary>
        /// Возвращает последнее слово
        /// </summary>
        /// <param name="anyOf">массив символов-разделителей</param>
        /// <returns></returns>
        public static string LastWord(this string str, params char[] anyOf)
        {
            int li = str.LastIndexOfAny(anyOf);
            if (li > 0)
            {
                int l = str.Length;
                return str.Substring(li + 1, l - li - 1);
            }
            else
                return null;
        }

        public static void SplitByLast(this string str, out string left, out string right, params char[] anyOf)
        {
            int li = str.LastIndexOfAny(anyOf);
            if (li > 0)
            {
                int l = str.Length;
                left = str.Substring(0, l);
                right = str.Substring(li + 1, l - li - 1);
            }
            else            
                right = left = "";
        }


        /// <summary>
        /// Возвращает имя файла (вырезает путь)
        /// </summary>
        //public static String FileName(this String str)
        //{
        //    //return LastWord(str, new char[] { '/', '\\' });
        //    return Path.GetFileName(str);            
        //}

        //public static string FilePath(this string str)
        //{
        //    return Path.GetFullPath(str);
        //}

        public static string Repeat(this string str, int count)
		{
			var rv = new StringBuilder();
			for (int i = 0; i < count; i++) { rv.Append(str); };
			return rv.ToString();
		}

		public static bool CompareByPattern(this string str, string pattern, StringComparison stringComparison, char splitter = '*')
		{
			var qa = pattern.Split(splitter);
			int i = 0;
			int l = 0;
			int c = 0; //возвращаемый рейтинг
			foreach (var qi in qa)
			{
				i = str.IndexOf(qi, i + l, stringComparison);
				if (i < 0)
					return false;
				else c++;
				l = qi.Length;
			}
			return true;

		}



	}
}
