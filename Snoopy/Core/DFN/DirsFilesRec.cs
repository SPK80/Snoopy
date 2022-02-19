using System;
using System.IO;

namespace IndFin.Core
{
    public class DirsFilesRec
    {
        /// Имя файла 
        public string Name { get; set; }
        /// Размер файла
        public long Length { get; set; }
        ///	Дата и время создания файла 
        public DateTime CreationTime { get; set; }
        ///	Дата и время изменения файла
		public DateTime LastWriteTime { get; set; }
        ///	Дата и время олкрытия файла
        public DateTime LastAccessTime { get; set; }

        public DirsFilesRec()
        {
            Name = "";
            Length = 0;
            CreationTime = DateTime.MinValue;
            LastWriteTime = DateTime.MinValue;
            LastAccessTime = DateTime.MinValue;          
        }
        public DirsFilesRec(FileInfo fi)
        {
            Name = fi.Name;
            Length = fi.Length;
            CreationTime = fi.CreationTime;
            LastWriteTime = fi.LastWriteTime;
            LastAccessTime = fi.LastAccessTime;
        }
        public DirsFilesRec(DirectoryInfo di)
        {
            Name = di.Name;
            Length = 0;
            CreationTime = di.CreationTime;
            LastWriteTime = di.LastWriteTime;
            LastAccessTime = di.LastAccessTime;
        }
        public override string ToString()
        {// совместима с JSON, но без {...} - это на совести вызывающего
            return string.Format(
                "\"Name\":\"{0}\"," +
                "\"Length\":{1}," +
                "\"CreationTime\":\"{2}\"," +
                "\"LastWriteTime\":\"{3}\"," +
                "\"LastAccessTime\":\"{4}\"", 
                Name, Length, CreationTime, LastWriteTime, LastAccessTime);
        }
    }
}
