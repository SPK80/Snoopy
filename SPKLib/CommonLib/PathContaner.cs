using System;
using System.ComponentModel;
using System.Data;
using System.Linq;

namespace CommonLib
{
    public class PathContaner : BindingList<FilePath>
    {
        private bool showPath = false;
        public bool ShowPath
        {
            get=> showPath;
            set
            {
                showPath = value;
                foreach (FilePath fp in this)
                    fp.ShowPath = showPath;
            }
        }
        public string[] Paths=>this.Select(i=>i.Path).ToArray();

        public void AddPath(string path)
        {
            Add(new FilePath(path, ShowPath));            
        }
    }

    public class FilePath
    {
        public string Path { get; }

        public FilePath(string path, bool showPath)
        {
            this.Path = path ?? throw new ArgumentNullException(nameof(path));
            ShowPath = showPath;
        }

        public bool ShowPath { get; set; }

        public override string ToString()
        {
            if (ShowPath)
                return Path;
            else
                return System.IO.Path.GetFileName(this.Path);
        }
    }

}
