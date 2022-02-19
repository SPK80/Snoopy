using System;
using System.Collections.Generic;


namespace Snoopy.Core
{
    public class SourcesList : List<Source>
    {
        public new bool Contains(Source source)
        {
            return base.Contains(source) ||
                (Find(s => s.Path == source.Path) != null);
        }
        public bool Contains(string path)
        {
            return (Find(s => s.Path == path) != null);
        }
    }

}
