using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Snoopy.Views
{
    public interface ISourcesView : IView
    {
        event Func<string, object[], bool> OnNewSource;
        event Func<string, bool> OnAddSource;
        void AddSource(object source);

        event Func<object, bool> OnRemoveSource;
        void RemoveSource(object source);

        event Func<object, string, bool> OnRenameSource;
        void RenameSource(object source, string newName);

        event Func<object, bool> OnUpdateSource;

        event Func<object, bool> OnCancelProgress;

        void SetSourceProcess(object source, int value, string msg);

        event Func<string, object, object[], bool> OnFindInSource;

        //List<object> Items { get; }

    }
}
