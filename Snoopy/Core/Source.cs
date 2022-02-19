using System;
using System.Collections;
using System.Collections.Generic;

namespace Snoopy.Core
{
    
    public enum SourceTypes { Unknown, AutoDetect, Index, Excel, Doc, Text, Directory}
    public enum ToStringModes { Unknown, Name, Path, NameAndPath, NameOrPath }

    public abstract class ResultClass
    {
        public ResultClass(object result)
        {
        }
    } 

    public abstract class Source
    {        
        protected Source(string name, string path, string extantion="")
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            Path = path ?? throw new ArgumentNullException(nameof(path));
            Extantion = extantion ?? throw new ArgumentNullException(nameof(extantion));
        }

        protected Action<object> ProcessDone;
        protected Action<object> ProcessCancelled;
        protected Action<object, int, string> ProcessChanged;
        protected Action<object, IEnumerable<object>> GotResults;

        public abstract void CancelProcess();

        /// <summary>
        /// Отображаемое в списке имя/псевдоним
        /// </summary>
        public abstract string Name { get; protected set; } 
        /// <summary>
        /// Полный путь/расположение, включая имя вайла и расширение
        /// </summary>
        public abstract string Path { get; protected set; }

        public abstract string Extantion { get; protected set; }

        public abstract void Find(string what, params object[] options);

        //public abstract bool Exists { get; }

        public ToStringModes ToStringMode => ToStringModes.NameOrPath;

        public override string ToString()
        {
            switch (ToStringMode)
            {
                case ToStringModes.NameOrPath: return Name!="" ? Name : Path;
                case ToStringModes.NameAndPath: return Name + ":" + Path;
                case ToStringModes.Name: return Name;
                case ToStringModes.Path: return Path;
                default: return base.ToString();
            }            
        }

        public static SourceTypes AutoDetectType(string path)
        {
            var ext = System.IO.Path.GetExtension(path);
            if (ext.Contains("xls"))
                return SourceTypes.Excel;
            else if (ext.Contains("doc"))
                return SourceTypes.Doc;
            else if (ext.Contains("txt"))
                return SourceTypes.Text;
            else if (ext.Contains("ind"))
                return SourceTypes.Index;
            else if (ext == "")
                return SourceTypes.Directory;
            else
                return SourceTypes.Unknown;
        }

        public SourceTypes SourceType => AutoDetectType(Path);

    }


}
